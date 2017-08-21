#!/bin/bash
rm -rf /home/rockpile/error/GameServer
rm -rf /home/rockpile/error.tar.gz
cd /home/rockpile
cp -R ./GameServer ./error
cd /home/rockpile
#find . ! -name '*DDZ*.*'|egrep -e '(txt|ini|log|dat|linux|py|json)$'|xargs rm -rf
for aaa in $(find ./error ! -name '*.txt'|egrep -e '(txt|ini|log|dat|linux|py|json)$')
do
  rm -rf "$aaa"
done

for bbb in $(find ./error -name '*.txt'|grep -v 'DDZ')
do
  rm -rf "$bbb"
done

tar zcvf ddzlog.tar.gz ./error
