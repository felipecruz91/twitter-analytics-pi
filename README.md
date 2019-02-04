# twitter-analytics-pi
Twitter Analytics running in a Raspberry Pi 3 Model B

.NET Core 2.2 Console App which listens for tweets given a tracking word on the Twitter Stream feed and stores them in an Influx DB database. 

# Docker image
https://cloud.docker.com/repository/docker/felipecruz/twitteranalytics

# Prerequisites

## Hardware
- A Raspberry Pi 3 Model B with [Raspbian](https://www.raspberrypi.org/downloads/raspbian/) already installed.

## Software
- A Docker installation in your Raspberry Pi. [Tutorial](https://iotbytes.wordpress.com/setting-up-docker-on-raspberry-pi-and-running-hello-world-container/)
- An Influx DB server running in your Raspberry Pi as a [Docker container](https://hub.docker.com/_/influxdb).

# Running the container in the Raspberry Pi

Log into your Raspberry Pi by ssh:

 `λ ssh pi@<YOUR_RASPBERRY_IP_ADDR>`
 
Run the container

  `docker run --rm -e "influxdb-server-addr=http://<YOUR_INFLUX_DB_IP_ADDRR>:<INFLUX_DB_PORT>" -e "trackingWord=climate change" felipecruz/twitteranalytics`
  
The container will start listening for tweets that contain the word `climate change` on the Twitter Stream feed and will store them in an Influx DB database.

# See the tweets in your Influx DB

Go inside your Influx DB container:

 `λ docker exec -it <INFLUX_DB_CONTAINER_ID> influx`
 
 ` >  use TwitterAnalytics`
 
 ` > SELECT * FROM tweet`
 
 

