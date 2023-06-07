#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CarsUnlimited.InventoryWorker/CarsUnlimited.InventoryWorker.csproj", "CarsUnlimited.InventoryWorker/"]
RUN dotnet restore "CarsUnlimited.InventoryWorker/CarsUnlimited.InventoryWorker.csproj"
COPY . .
WORKDIR "/src/CarsUnlimited.InventoryWorker"
RUN dotnet build "CarsUnlimited.InventoryWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarsUnlimited.InventoryWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarsUnlimited.InventoryWorker.dll"]