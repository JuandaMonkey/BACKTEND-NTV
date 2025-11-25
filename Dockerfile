FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BACKEND-NTV.csproj", "./"]
RUN dotnet restore "./BACKEND-NTV.csproj"
COPY . .
RUN dotnet publish "BACKEND-NTV.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BACKEND-NTV.dll"]
