#!/bin/bash
 for i in {6379..6408}
do
   nohup redis-server   /export/redis/product/$i/redis.conf  >/dev/null 2>&1 &
done

