# docker build --no-cache -t edutrail-edu-trail-service:prod -f Dockerfile.Production .
# docker stack rm edutrail
# docker service ls
cd ~/EduTrail
# docker stack deploy -c docker-compose.prod.yml edutrail

# docker service logs edutrail_edu-trail-service --tail 100 -f 

# Frist Time Deployment 
-------------------------------------------------

1. Create folders
    mkdir -p ~/traefik
   

2. Create Traefik network
    docker --version
    docker swarm init 
    docker network create -d overlay dokploy-network

3. Traefik setup
    docker-compose.traefik.ym

4. 🔐 Create acme file
    mkdir -p ~/traefik/letsencrypt
    touch ~/traefik/letsencrypt/acme.json
    chmod 600 ~/traefik/letsencrypt/acme.json

5. 🚀 Deploy Traefik
    cd ~/traefik
    docker stack deploy -c docker-compose.traefik.yml traefik

6.  🗄️ Backend + DB stack (EduTrail)
    ~/EduTrail/docker-compose.prod.yml

7. 🔨  Build Docker image (API + Angular)
    cd ~/EduTrail
    docker build --no-cache -t edutrail-edu-trail-service:prod -f Dockerfile.Production .

8. 🚀  Deploy stack
    docker stack deploy -c docker-compose.prod.yml edutrail

9. docker service logs edutrail_edu-trail-service --tail 100 -f
