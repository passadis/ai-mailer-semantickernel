FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the certificate to the container
# COPY advicebackend.pfx /app/advicebackend.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "SemanticKernelEmailAssistant.csproj"
RUN dotnet build "SemanticKernelEmailAssistant.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SemanticKernelEmailAssistant.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SemanticKernelEmailAssistant.dll"]