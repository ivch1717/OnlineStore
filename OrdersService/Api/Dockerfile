FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY OrdersService/Api/Api.csproj OrdersService/Api/
COPY OrdersService/Entities/Entities.csproj OrdersService/Entities/
COPY OrdersService/Infrastructure/Infrastructure.csproj OrdersService/Infrastructure/
COPY OrdersService/Presentation/Presentation.csproj OrdersService/Presentation/
COPY OrdersService/UseCases/UseCases.csproj OrdersService/UseCases/

RUN dotnet restore OrdersService/Api/Api.csproj

COPY . .
RUN dotnet publish OrdersService/Api/Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "Api.dll"]
