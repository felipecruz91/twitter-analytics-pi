# twitter-analytics-pi
Twitter Analytics running in a Raspberry Pi 3 Model B

.NET Core 2.2 Console App which listens for tweets given a tracking word on the Twitter Stream feed and stores them in an Influx DB database. 

# Prerequisites

## Hardware
- A Raspberry Pi 3 Model B with [Raspbian](https://www.raspberrypi.org/downloads/raspbian/) already installed.

## Software
- A Docker installation in your Raspberry Pi. [Tutorial](https://iotbytes.wordpress.com/setting-up-docker-on-raspberry-pi-and-running-hello-world-container/)
- An Influx DB server running in your Raspberry Pi as a [Docker container](https://hub.docker.com/r/hypriot/rpi-influxdb).

# Running your InfluxDB image

Start your image binding the external port 8086 of your containers:

    docker run -d -p 8086:8086 hypriot/rpi-influxdb

# Twitter Analytics configuration setup

Set up the following configuration in the `appsettings.json` and `appsettings.*.json` files with your own values:

    {
      "InfluxDB": {
        "ServerAddress": "http://<INFLUX_DB_SERVER_ADDR>:<INFLUX_DB_PORT>"
      },
      "Twitter": {
        "ConsumerKey": "",
        "ConsumerSecret": "",
        "AccessToken": "",
        "AccessTokenSecret": ""
      },
      "TextAnalytics": {
        "Name": "",
        "Key1": "",
        "Key2": ""
      } 
    }



# Build the Twitter Analytics Docker image

    bash build.sh

# Upload the Docker image to your public or private registry 

    bash push.sh

# Running the container from Twitter Analytics Docker image in the Raspberry Pi

Log into your Raspberry Pi by ssh:

 `λ ssh pi@<YOUR_RASPBERRY_IP_ADDR>`
 
Run the container

  `docker run --rm -e "keyword=climate change" youraccount/twitteranalytics`
  
The container will start listening for tweets that contain the word `climate change` on the Twitter Stream feed and will store them in an Influx DB database.

# See the tweets in your Influx DB

Go inside your Influx DB container:

 `λ docker exec -it <INFLUX_DB_CONTAINER_ID> influx`
 
 ` >  use TwitterAnalytics`
 
 ` > SELECT * FROM tweet`
 
 

