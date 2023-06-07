#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /
COPY ["CarsUnlimited.CartAPI/CarsUnlimited.CartAPI.csproj", "CarsUnlimited.CartAPI/"]
COPY ["CarsUnlimited.CartShared/CarsUnlimited.CartShared.csproj", "CarsUnlimited.CartShared/"]
COPY ["CarsUnlimited.Shared/CarsUnlimited.Shared.csproj", "CarsUnlimited.Shared/"]
RUN dotnet restore "CarsUnlimited.CartAPI/CarsUnlimited.CartAPI.csproj"
COPY . .
WORKDIR "/CarsUnlimited.CartAPI"
RUN dotnet build "CarsUnlimited.CartAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarsUnlimited.CartAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarsUnlimited.CartAPI.dll"]
