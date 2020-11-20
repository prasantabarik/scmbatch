#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DeliveryMomentMessageGenerator/DeliveryMomentMessageGenerator.csproj", "DeliveryMomentMessageGenerator/"]
COPY ["DeliveryMomentReferenceServices/DeliveryMomentReferenceServices.csproj", "DeliveryMomentReferenceServices/"]
COPY ["DeliveryMomentReferenceModel/DeliveryMomentReferenceModel.csproj", "DeliveryMomentReferenceModel/"]
COPY ["DeliveryMomentReferenceRepository/DeliveryMomentReferenceRepository.csproj", "DeliveryMomentReferenceRepository/"]
RUN dotnet restore "DeliveryMomentMessageGenerator/DeliveryMomentMessageGenerator.csproj"
COPY . .
WORKDIR "/src/DeliveryMomentMessageGenerator"
RUN dotnet build "DeliveryMomentMessageGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeliveryMomentMessageGenerator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliveryMomentMessageGenerator.dll"]