#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Lkq.Api.RulesRepo/Lkq.Api.RulesRepo.csproj", "Lkq.Api.RulesRepo/"]
COPY ["Lkq.Core.RulesRepo/Lkq.Core.RulesRepo.csproj", "Lkq.Core.RulesRepo/"]
COPY ["Lkq.Models.RulesRepo/Lkq.Models.RulesRepo.csproj", "Lkq.Models.RulesRepo/"]
COPY ["Lkq.Data.RulesRepo/Lkq.Data.RulesRepo.csproj", "Lkq.Data.RulesRepo/"]
COPY ["TestRulesRepo/TestRulesRepo.csproj", "TestRulesRepo/"]
COPY ["nuget.config", "nuget.config"]
RUN dotnet restore "Lkq.Api.RulesRepo/Lkq.Api.RulesRepo.csproj"
COPY . .
WORKDIR "/src/Lkq.Api.RulesRepo"
RUN dotnet build "Lkq.Api.RulesRepo.csproj" -c Release -o /app/build

RUN dotnet test

FROM build AS publish
RUN dotnet publish "Lkq.Api.RulesRepo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lkq.Api.RulesRepo.dll"]