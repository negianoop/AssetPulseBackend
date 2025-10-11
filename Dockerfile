# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["AssetPulse.csproj", "./"]
RUN dotnet restore "AssetPulse.csproj"

# Copy the rest of the code and build
COPY . .
RUN dotnet build "AssetPulse.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "AssetPulse.csproj" -c Release -o /app/publish

# Stage 3: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssetPulse.dll"]