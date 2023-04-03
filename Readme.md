# DCMS.Micro

注意：本仓库已经迁移至 [NEWDCMS](https://github.com/NEWDCMS)

DCMS.Micro 分布式版本，是基于Saas的经销商快消解决方案，皆在满足区域营销管理业务快速变化需求，系统基于Docker + .Net core + Mysql Inner db cluster 的分布式微服务框架,提供高性能RPC远程服务调用，采用Zookeeper、Consul作为surging服务的注册中心，集成了哈希，随机，轮询，压力最小优先作为负载均衡的算法，RPC集成采用的是netty框架，采用异步传输，客户端APP 采用 Android Xamarin/ Xamarin.Forms 支持Android 5.0 以上 所有Android 最新版本

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

## 技术栈：

.Net Core 6.0 微软全新跨平台应用运行时框架

Dapr 微软官方分布式运行时

Orleans 大规模分布式计算利器

Actor  分布式并发控制模型

Keda  基于事件驱动的弹性伸缩组件

Docker 容器引擎

Sentinel 接口流量防护

Zipkin 链路追踪

K8S 容器编排引擎

Refit  RESTAPI访问框架

ServerMesh  服务网格

Webassembly 浏览器中运行服务器代码技术

Blazor 替代Javascriipt/CSS的跨平台前端框架

kafka  流处理框架

Nginx 代理引擎

RabbitMQ 消息队列

MySQL MGR 数据库集群

Redis 状态/缓存存储

PostgreSQL 数据仓库和数据归档存储


## License

MIT
