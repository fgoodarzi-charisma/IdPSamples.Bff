namespace Charisma.AuthenticationManager.Extensions;

public static partial class LoggerMessageDefinitions
{
    [LoggerMessage(EventId = 100, Level = LogLevel.Error,
        Message = "Something went wrong. {Message}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogFinalException(this ILogger logger, Exception exception, string message,
        DateTime dateTime, byte securityLog);

    public static void LogFinalException(this ILogger logger, Exception exception, string message)
    {
        LogFinalException(logger, exception, message, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 101, Level = LogLevel.Error,
        Message = "Consuming fault message. {MessageId}, {Exceptions}, {ElapsedTime}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogGeneralFaultConsumer(this ILogger logger, Guid messageId, string exceptions,
        TimeSpan elapsedTime, DateTime dateTime, byte securityLog);

    public static void LogGeneralFaultConsumer(this ILogger logger, Guid messageId, string exceptions, TimeSpan elapsedTime)
    {
        LogGeneralFaultConsumer(logger, messageId, exceptions, elapsedTime, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 102, Level = LogLevel.Error,
        Message = "Logout consuming message failed. {MessageId}, {exceptions}, {UserId}, {Ip}, {UserAgent}, {ElapsedTime}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogLogoutFaultConsumer(this ILogger logger, Guid messageId, string exceptions, long userId, string ip,
         string userAgent, TimeSpan elapsedTime, DateTime dateTime, byte securityLog);

    public static void LogLogoutFaultConsumer(this ILogger logger, Guid messageId, string exceptions, long userId,
        string ip, string userAgent, TimeSpan elapsedTime)
    {
        LogLogoutFaultConsumer(logger, messageId, exceptions, userId, ip, userAgent, elapsedTime, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 103, Level = LogLevel.Error,
        Message = "Force logout consuming message failed. {MessageId}, {exceptions}, {UserId}, {Ip}, {UserAgent}, {ElapsedTime}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogForceLogoutFaultConsumer(this ILogger logger, Guid messageId, string exceptions, long userId, string ip,
         string userAgent, TimeSpan elapsedTime, DateTime dateTime, byte securityLog);

    public static void LogForceLogoutFaultConsumer(this ILogger logger, Guid messageId, string exceptions, long userId,
        string ip, string userAgent, TimeSpan elapsedTime)
    {
        LogForceLogoutFaultConsumer(logger, messageId, exceptions, userId, ip, userAgent, elapsedTime, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 104, Level = LogLevel.Information,
        Message = "Consuming logout message; User added to be logged out in the first receiving request. {MessageId}, {UserId}, {Ip}, {UserAgent}, {SessionId}, {ElapsedTime}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogLogoutConsumer(this ILogger logger, Guid messageId, long userId, string ip,
         string userAgent, string? sessionId, TimeSpan elapsedTime, DateTime dateTime, byte securityLog);

    public static void LogLogoutConsumer(this ILogger logger, Guid messageId, long userId,
        string ip, string userAgent, string? sessionId, TimeSpan elapsedTime)
    {
        LogLogoutConsumer(logger, messageId, userId, ip, userAgent, sessionId, elapsedTime, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 105, Level = LogLevel.Information,
        Message = "Consuming force logout message. {MessageId}, {UserId}, {Ip}, {UserAgent}, {ElapsedTime}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogForceLogoutConsumer(this ILogger logger, Guid messageId, long userId, string ip,
         string userAgent, TimeSpan elapsedTime, DateTime dateTime, byte securityLog);

    public static void LogForceLogoutConsumer(this ILogger logger, Guid messageId, long userId,
        string ip, string userAgent, TimeSpan elapsedTime)
    {
        LogForceLogoutConsumer(logger, messageId, userId, ip, userAgent, elapsedTime, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 106, Level = LogLevel.Information,
        Message = "Host reformed in receiving request; Host replaced with x-host for receiving request. {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void HostReformedForReceivingRequest(this ILogger logger, DateTime dateTime, byte securityLog);

    public static void HostReformedForReceivingRequest(this ILogger logger)
    {
        HostReformedForReceivingRequest(logger, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 107, Level = LogLevel.Information,
        Message = "User logged out. {UserId}, {Ip}, {UserAgent}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void UserLoggedOut(this ILogger logger, long userId, string ip, string userAgent,
        DateTime dateTime, byte securityLog);

    public static void UserLoggedOut(this ILogger logger, long userId, string ip, string userAgent)
    {
        UserLoggedOut(logger, userId, ip, userAgent, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 108, Level = LogLevel.Information,
        Message = "Header x-forwarded-for exists. {XForwardedFor}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogXForwardedFor(this ILogger logger, string? xForwardedFor,
        DateTime dateTime, byte securityLog);

    public static void LogXForwardedFor(this ILogger logger, string? xForwardedFor)
    {
        LogXForwardedFor(logger, xForwardedFor, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 109, Level = LogLevel.Information,
        Message = "Ip extracted from request. {Ip}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogExtractedIp(this ILogger logger, string? ip, DateTime dateTime, byte securityLog);

    public static void LogExtractedIp(this ILogger logger, string? ip)
    {
        LogExtractedIp(logger, ip, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 110, Level = LogLevel.Information,
        Message = "Receiving user agent info. {UserId}, {Ip}, {UserAgent}, {SessionId}, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogReceivingUserAgentInfo(this ILogger logger, long userId, string? ip, string? userAgent,
        string? sessionId, DateTime dateTime, byte securityLog);

    public static void LogReceivingUserAgentInfo(this ILogger logger, long userId, string? ip,
        string? userAgent, string? sessionId)
    {
        LogReceivingUserAgentInfo(logger, userId, ip, userAgent, sessionId, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 111, Level = LogLevel.Error, Message = "User is unauthorized, {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogUnauthorizedUser(this ILogger logger, DateTime dateTime, byte securityLog);

    public static void LogUnauthorizedUser(this ILogger logger)
    {
        LogUnauthorizedUser(logger, DateTime.UtcNow, 1);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 112, Level = LogLevel.Error,
        Message = "Token endpoint is not enabled. If you need to get access_token and id_token you MUST include TokenEndpoint environment variable for the BFF application with the value of 'Enabled', {DateTime}, {SecurityLog}",
        SkipEnabledCheck = true)]
    private static partial void LogForbiddenTokenEndpoint(this ILogger logger, DateTime dateTime, byte securityLog);

    public static void LogForbiddenTokenEndpoint(this ILogger logger)
    {
        LogForbiddenTokenEndpoint(logger, DateTime.UtcNow, 0);
    }

    //=======================================================================================================================================//

    [LoggerMessage(EventId = 113, Level = LogLevel.Error, Message = "Failed on obtaining IP, {DateTime}, {SecurityLog}", SkipEnabledCheck = true)]
    private static partial void LogFailedOnObtainingIp(this ILogger logger, Exception ex, DateTime dateTime, byte securityLog);

    public static void LogFailedOnObtainingIp(this ILogger logger, Exception ex)
    {
        LogFailedOnObtainingIp(logger, ex, DateTime.UtcNow, 0);
    }

    //=======================================================================================================================================//
}
