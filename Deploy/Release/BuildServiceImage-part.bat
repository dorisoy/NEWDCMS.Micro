docker rmi tradeservice:release goodsservice:release accountservice:release apidocument:release
docker system prune -f
cd ../../
docker build . -t accountservice:release --target accountservice 
docker build . -t goodsservice:release --target goodsservice
docker build . -t tradeservice:release --target tradeservice
docker build . -t apidocument:release --target apidocument
