FROM mcr.microsoft.com/dotnet/sdk:6.0 as svcbuild
WORKDIR /src
copy . .
RUN dotnet publish -c Release DCMS.sln

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as goodsservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/GoodsService/Host/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "Host.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as accountservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/AccountService/Host/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "Host.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as publicservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/PublicService/Host/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "Host.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as tradeservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/TradeService/Host/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "Host.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as mediaservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/MediaService/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "MediaService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as scheduleservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/ScheduleService/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "ScheduleService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as oauthservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/OauthService/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "OauthService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as apidocument
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/ApiDocument/ApiDocument/bin/Release/net6.0/publish /app
ENTRYPOINT ["dotnet", "ApiDocument.dll"]



