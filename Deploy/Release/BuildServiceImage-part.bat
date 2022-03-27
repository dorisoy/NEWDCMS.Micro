docker rmi tradeservice:release goodsservice:release
docker system prune -f
cd ../../
docker build . -t goodsservice:release --target goodsservice
docker build . -t tradeservice:release --target tradeservice