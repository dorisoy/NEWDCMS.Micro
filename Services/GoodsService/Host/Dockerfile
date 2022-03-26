#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/GoodsService/Host/Host.csproj", "Services/GoodsService/Host/"]
COPY ["Services/GoodsService/ApplicationService/ApplicationService.csproj", "Services/GoodsService/ApplicationService/"]
COPY ["Services/GoodsService/Infrastructure/Infrastructure.csproj", "Services/GoodsService/Infrastructure/"]
COPY ["Public/Base/InfrastructureBase/InfrastructureBase.csproj", "Public/Base/InfrastructureBase/"]
COPY ["Public/Base/DomainBase/DomainBase.csproj", "Public/Base/DomainBase/"]
COPY ["Public/Remote/IApplicationService/IApplicationService.csproj", "Public/Remote/IApplicationService/"]
COPY ["RPCDapr/src/RPCDapr.PubSub.Rabbitmq/RPCDapr.PubSub.Rabbitmq.csproj", "RPCDapr/src/RPCDapr.PubSub.Rabbitmq/"]
COPY ["RPCDapr/src/RPCDapr/RPCDapr.csproj", "RPCDapr/src/RPCDapr/"]
COPY ["RPCDapr/src/RPCDapr.PubSub.Dapr/RPCDapr.PubSub.Dapr.csproj", "RPCDapr/src/RPCDapr.PubSub.Dapr/"]
COPY ["Dapr/src/RPCDapr/RPCDapr.Common/RPCDapr.Common.csproj", "Dapr/src/RPCDapr/RPCDapr.Common/"]
COPY ["Dapr/src/RPCDapr.Client/RPCDapr.Client.ServerSymbol/RPCDapr.Client.ServerSymbol.csproj", "Dapr/src/RPCDapr.Client/RPCDapr.Client.ServerSymbol/"]
COPY ["Dapr/src/RPCDapr.Client/RPCDapr.Client.ServerProxyFactory/RPCDapr.Client.ServerProxyFactory.csproj", "Dapr/src/RPCDapr.Client/RPCDapr.Client.ServerProxyFactory/"]
COPY ["Dapr/src/RPCDapr/RPCDapr.PrRPCDaprerator/RPCDapr.PrRPCDaprerator.csproj", "Dapr/src/RPCDapr/RPCDapr.PrRPCDaprerator/"]
COPY ["Dapr/src/RPCDapr.Server/RPCDapr.Server.Kestrel/RPCDapr.Server.Kestrel.csproj", "Dapr/src/RPCDapr.Server/RPCDapr.Server.Kestrel/"]
COPY ["Dapr/src/RPCDapr.Mesh/RPCDapr.Mesh.Dapr/RPCDapr.Mesh.Dapr.csproj", "Dapr/src/RPCDapr.Mesh/RPCDapr.Mesh.Dapr/"]
COPY ["RPCDapr/src/RPCDapr.Store.Dapr/RPCDapr.Store.Dapr.csproj", "RPCDapr/src/RPCDapr.Store.Dapr/"]
COPY ["RPCDapr/src/RPCDapr.Store.Redis/RPCDapr.Store.Redis.csproj", "RPCDapr/src/RPCDapr.Store.Redis/"]
COPY ["Dapr/src/RPCDapr/RPCDapr.IocModule/RPCDapr.IocModule.csproj", "Dapr/src/RPCDapr/RPCDapr.IocModule/"]
COPY ["Services/GoodsService/Domain/Domain.csproj", "Services/GoodsService/Domain/"]
COPY ["Services/GoodsService/GoodsActorProxyGenerator/GoodsActorProxyGenerator.csproj", "Services/GoodsService/GoodsActorProxyGenerator/"]
RUN dotnet restore "Services/GoodsService/Host/Host.csproj"
COPY . .
WORKDIR "/src/Services/GoodsService/Host"
RUN dotnet build "Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.dll"]