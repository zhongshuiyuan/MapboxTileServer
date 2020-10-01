FROM openjdk:14-alpine

RUN apk add --update wget

WORKDIR /app/photon
RUN wget https://github.com/komoot/photon/releases/download/0.3.4/photon-0.3.4.jar -q --show-progress
COPY . ./

# And start Photon:
CMD java -jar /app/photon/photon-0.3.4.jar -data-dir /Photon/photon-db-de-200809 -listen-ip 0.0.0.0 -listen-port 2322 -cors-any