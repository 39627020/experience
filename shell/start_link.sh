#!/bin/sh

#erl -name link_pro_81_0@172.17.0.81 -setcookie beyond -pa ebin -pa ../deps/*/ebin -s test start link_pro_81_0 -detached
#erl -name link_pro_81_1@172.17.0.81 -setcookie beyond -pa ebin -pa ../deps/*/ebin -s test start link_pro_81_1 -detached
#erl -name link_pro_81_2@172.17.0.81 -setcookie beyond -pa ebin -pa ../deps/*/ebin -s test start link_pro_81_2 -detached
#erl -name link_pro_81_3@172.17.0.81 -setcookie beyond -pa ebin -pa ../deps/*/ebin -s test start link_pro_81_3 -detached
#erl -name link_pro_81_4@172.17.0.81 -setcookie beyond -pa ebin -pa ../deps/*/ebin -s test start link_pro_81_4 -detached

ulimit -n 10240

for i in `seq 0 3`
do
	erl -name link_pro_81_$i@172.17.0.81 -setcookie beyond -pa ebin -pa ../deps/*/ebin -s test start link_pro_81_$i  -detached
	echo $i
done

