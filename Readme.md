# DCMS.Micro

DCMS.Micro 分布式版本，是华润雪花啤酒晋陕区域公司基于Saas的经销商快消解决方案，皆在满足区域营销管理业务快速变化需求，系统基于Docker + .Net core + Mysql Inner db cluster 的分布式微服务框架,提供高性能RPC远程服务调用，采用Zookeeper、Consul作为surging服务的注册中心，集成了哈希，随机，轮询，压力最小优先作为负载均衡的算法，RPC集成采用的是netty框架，采用异步传输，客户端APP 采用 Android Xamarin/ Xamarin.Forms 支持Android 5.0 以上 所有Android 最新版本

## 环境依赖

windows平台：
​	docker for windows on kubernetes 1.19+
​	helm3
​	dapr  --version 1.0

linux平台：

​	docker for linux
​	kubernetes 1.19+
​	helm3
​	dapr --version1.0

## 安装手册



## License

MIT
