#!/bin/bash

ps -ef|grep erl|grep -v grep|cut -c 9-15|xargs kill -9

cd /home/rockpile/work/nwiob/ebin
./study.sh

