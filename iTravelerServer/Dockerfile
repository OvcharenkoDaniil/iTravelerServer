#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["iTravelerServer/iTravelerServer.csproj", "iTravelerServer/"]
COPY ["iTravelerServer.DAL/iTravelerServer.DAL.csproj", "iTravelerServer.DAL/"]
COPY ["iTravelerServer.Domain/iTravelerServer.Domain.csproj", "iTravelerServer.Domain/"]
COPY ["iTraveler.Auth/iTraveler.Auth.csproj", "iTraveler.Auth/"]
COPY ["iTravelerServer.Service/iTravelerServer.Service.csproj", "iTravelerServer.Service/"]
RUN dotnet restore "iTravelerServer/iTravelerServer.csproj"
COPY . .
WORKDIR "/src/iTravelerServer"
RUN dotnet build "iTravelerServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iTravelerServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iTravelerServer.dll"]