FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
ENTRYPOINT ["dotnet", "Host.dll"]

#docker build . -t accountservice:release --no-cache --progress=plain