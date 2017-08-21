#!/bin/bash
ssh rockpile@172.16.0.111 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.112 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.113 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.114 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.115 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.116 "/home/rockpile/tarDdzLog.sh"

ssh rockpile@172.16.0.121 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.122 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.123 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.124 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.125 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.126 "/home/rockpile/tarDdzLog.sh"

ssh rockpile@172.16.0.131 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.132 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.133 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.134 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.135 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.136 "/home/rockpile/tarDdzLog.sh"

ssh rockpile@172.16.0.141 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.142 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.143 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.144 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.145 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.146 "/home/rockpile/tarDdzLog.sh"

ssh rockpile@172.16.0.151 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.152 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.153 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.154 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.155 "/home/rockpile/tarDdzLog.sh"
ssh rockpile@172.16.0.156 "/home/rockpile/tarDdzLog.sh"

rm -rf /home/rockpile/ErrLog/111/*
rm -rf /home/rockpile/ErrLog/112/*
rm -rf /home/rockpile/ErrLog/113/*
rm -rf /home/rockpile/ErrLog/114/*
rm -rf /home/rockpile/ErrLog/115/*
rm -rf /home/rockpile/ErrLog/116/*

rm -rf /home/rockpile/ErrLog/121/*
rm -rf /home/rockpile/ErrLog/122/*
rm -rf /home/rockpile/ErrLog/123/*
rm -rf /home/rockpile/ErrLog/124/*
rm -rf /home/rockpile/ErrLog/125/*
rm -rf /home/rockpile/ErrLog/126/*

rm -rf /home/rockpile/ErrLog/131/*
rm -rf /home/rockpile/ErrLog/132/*
rm -rf /home/rockpile/ErrLog/133/*
rm -rf /home/rockpile/ErrLog/134/*
rm -rf /home/rockpile/ErrLog/135/*
rm -rf /home/rockpile/ErrLog/136/*

rm -rf /home/rockpile/ErrLog/141/*
rm -rf /home/rockpile/ErrLog/142/*
rm -rf /home/rockpile/ErrLog/143/*
rm -rf /home/rockpile/ErrLog/144/*
rm -rf /home/rockpile/ErrLog/145/*
rm -rf /home/rockpile/ErrLog/146/*

rm -rf /home/rockpile/ErrLog/151/*
rm -rf /home/rockpile/ErrLog/152/*
rm -rf /home/rockpile/ErrLog/153/*
rm -rf /home/rockpile/ErrLog/154/*
rm -rf /home/rockpile/ErrLog/155/*
rm -rf /home/rockpile/ErrLog/156/*

scp rockpile@172.16.0.111:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/111/
scp rockpile@172.17.0.112:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/112/
scp rockpile@172.16.0.113:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/113/
scp rockpile@172.17.0.114:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/114/
scp rockpile@172.16.0.115:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/115/
scp rockpile@172.17.0.116:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/116/

scp rockpile@172.16.0.121:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/121/
scp rockpile@172.17.0.122:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/122/
scp rockpile@172.16.0.123:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/123/
scp rockpile@172.17.0.124:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/124/
scp rockpile@172.16.0.125:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/125/
scp rockpile@172.17.0.126:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/126/

scp rockpile@172.16.0.131:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/131/
scp rockpile@172.17.0.132:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/132/
scp rockpile@172.16.0.133:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/133/
scp rockpile@172.17.0.134:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/134/
scp rockpile@172.16.0.135:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/135/
scp rockpile@172.17.0.136:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/136/

scp rockpile@172.16.0.141:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/141/
scp rockpile@172.17.0.142:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/142/
scp rockpile@172.16.0.143:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/143/
scp rockpile@172.17.0.144:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/144/
scp rockpile@172.16.0.145:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/145/
scp rockpile@172.17.0.146:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/146/

scp rockpile@172.16.0.151:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/151/
scp rockpile@172.17.0.152:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/152/
scp rockpile@172.16.0.153:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/153/
scp rockpile@172.17.0.154:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/154/
scp rockpile@172.16.0.155:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/155/
scp rockpile@172.17.0.156:/home/rockpile/ddzlog.tar.gz /home/rockpile/ErrLog/156/

cd /home/rockpile/ErrLog/111/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/112/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/113/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/114/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/115/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/116/
tar zxvf ddzlog.tar.gz

cd /home/rockpile/ErrLog/121/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/122/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/123/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/124/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/125/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/126/
tar zxvf ddzlog.tar.gz

cd /home/rockpile/ErrLog/131/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/132/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/133/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/134/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/135/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/136/
tar zxvf ddzlog.tar.gz

cd /home/rockpile/ErrLog/141/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/142/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/143/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/144/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/145/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/146/
tar zxvf ddzlog.tar.gz

cd /home/rockpile/ErrLog/151/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/152/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/153/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/154/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/155/
tar zxvf ddzlog.tar.gz
cd /home/rockpile/ErrLog/156/
tar zxvf ddzlog.tar.gz

cd /home/rockpile
find ./ErrLog -name "ddzlog*.*"|xargs rm -rf
rm -rf ddzlog_all.tar.gz
tar zcvf ddzlog_all.tar.gz ./ErrLog
echo "Done!"