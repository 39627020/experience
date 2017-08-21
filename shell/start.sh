#!/bin/sh

ulimit -n 10240

#for((i=0;i<4;i++))
for i in `seq 0 3`
do
	erl -name account_91_$i@172.17.0.91 -setcookie beyond -pa ebin -pa ../deps/*/ebin -s test start account_91_$i -detached
	echo $i
done
