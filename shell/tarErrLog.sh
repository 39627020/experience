#!/bin/bash
rm -rf /home/rockpile/error/GameServer
rm -rf /home/rockpile/error.tar.gz
cd /home/rockpile
cp -R ./GameServer ./error
cd /home/rockpile/error
find . ! -name '*Err.*'|egrep -e '(txt|ini|log|dat|linux|py|json)$'|xargs rm -rf
cd /home/rockpile
tar zcvf error.tar.gz ./error
