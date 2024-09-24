FROM mcr.microsoft.com/dotnet/aspnet:5.0
ARG source
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY ${source:-bin/Debug/net5.0/publish} .
ENTRYPOINT ["dotnet", "Sofisoft.Accounts.WorkingSpace.API.dll"]