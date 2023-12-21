# Get Base Image (Full .NET Core SDK)
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
ENV OPENAI_API_KEY=MY_OPENAI_KEY
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PublicChat.dll"]