docker rmi oauthservice:release tradeservice:release publicservice:release scheduleservice:release mediaservice:release goodsservice:release accountservice:release apidocument:release
docker system prune -f
cd ../../
docker build . -t accountservice:release --target accountservice 
docker build . -t goodsservice:release --target goodsservice
docker build . -t mediaservice:release --target mediaservice
docker build . -t scheduleservice:release --target scheduleservice
docker build . -t publicservice:release --target publicservice
docker build . -t tradeservice:release --target tradeservice
docker build . -t oauthservice:release --target oauthservice
docker build . -t apidocument:release --target apidocument
