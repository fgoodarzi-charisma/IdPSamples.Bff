using Charisma.AuthenticationManager;
using Charisma.AuthenticationManager.Extensions;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using Serilog.Exceptions;
using System.Text;
#if !DEBUG
using Serilog.Formatting.Compact;
#endif

Console.OutputEncoding = Encoding.UTF8;
Log.Logger = new LoggerConfiguration()
#if DEBUG
            .WriteTo.Console()
#else
            .WriteTo.Console(new CompactJsonFormatter())
#endif
    .CreateBootstrapLogger();

Log.Information("Authentication manager application start-up completed, {DateTime}, {SecurityLog}",
    DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), 1);

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddReverseProxy()
        .AddBffExtensions()
        .LoadFromSimpleConfiguration(builder.Configuration);

    builder.Services.AddBff()
        .AddRemoteApis()
        .AddServerSideSessions();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    }).AddCookie("cookie", options =>
    {
        options.Cookie.Name = "bff.session";
        options.Cookie.SameSite = SameSiteMode.Strict;
    }).AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("Sts:BaseUrl");
        options.ClientId = builder.Configuration.GetValue<string>("Sts:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("Sts:ClientSecret");
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.SaveTokens = true;

        options.Scope.Clear();
        var scopes = builder.Configuration.GetValue<string>("Sts:Scopes")?.Split(' ') ?? [];
        foreach (var scope in scopes)
        {
            options.Scope.Add(scope);
        }
    });

    builder.Services.AddAuthorization();

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    builder.Host.UseSerilog((context, config) =>
    {
        config.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithAssemblyName()
            .Enrich.WithExceptionDetails()
#if DEBUG
            .WriteTo.Async(writeTo => writeTo.Console())
#else
            .WriteTo.Async(writeTo => writeTo.Console(new CompactJsonFormatter()))
#endif
            .ReadFrom.Configuration(context.Configuration);
    });

    var app = builder.Build();

    app.UseForwardedHeaders();

    app.UseLogEnricher();
    app.UseSerilogRequestLogging();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseBff();
    app.UseAuthorization();
    app.MapBffManagementEndpoints();

    app.MapReverseProxy(proxyApp =>
    {
        proxyApp.UseAntiforgeryCheck();
    });

    app.MapFallbackToFile("/index.html");

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    Log.Fatal(ex, "Unhandled exception occured in Authentication manager application, {DateTime}, {SecurityLog}",
        DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), 1);
}
finally
{
    Log.Information("Authentication manager application shut-down completed, {DateTime}, {SecurityLog}",
        DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), 1);
    Log.CloseAndFlush();
}