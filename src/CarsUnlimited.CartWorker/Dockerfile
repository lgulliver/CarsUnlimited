#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CarsUnlimited.CartWorker/CarsUnlimited.CartWorker.csproj", "CarsUnlimited.CartWorker/"]
RUN dotnet restore "CarsUnlimited.CartWorker/CarsUnlimited.CartWorker.csproj"
COPY . .
WORKDIR "/src/CarsUnlimited.CartWorker"
RUN dotnet build "CarsUnlimited.CartWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarsUnlimited.CartWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarsUnlimited.CartWorker.dll"]