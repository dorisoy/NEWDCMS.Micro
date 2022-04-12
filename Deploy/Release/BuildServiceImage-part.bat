docker system prune -f
cd ../../
docker build . -t accountservice:1.0 --target accountservice