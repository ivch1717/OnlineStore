FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PaymentService/Api/Api.csproj PaymentService/Api/
COPY PaymentService/Entities/Entities.csproj PaymentService/Entities/
COPY PaymentService/Infrastructure/Infrastructure.csproj PaymentService/Infrastructure/
COPY PaymentService/Presentation/Presentation.csproj PaymentService/Presentation/
COPY PaymentService/UseCases/UseCases.csproj PaymentService/UseCases/

RUN dotnet restore PaymentService/Api/Api.csproj

COPY . .
RUN dotnet publish PaymentService/Api/Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "Api.dll"]
