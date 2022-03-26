docker rmi mobilefrontend:release adminfrontend:release 
docker system prune -f  
cd ../../
cd Core/WebPage/Admin
docker build . -t adminfrontend:release 
cd ../www 
docker build . -t mobilefrontend:release  