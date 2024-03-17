FROM dockerhost.charisma.tech/dotnet/sdk:8.0
WORKDIR /App
COPY . .

EXPOSE 80
EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]