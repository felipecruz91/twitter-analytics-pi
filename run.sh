#!bin/bash

docker run --rm -e "influxdb-server-addr=http://192.168.1.177:8086" -e "trackingWord=hello" twitteranalytics