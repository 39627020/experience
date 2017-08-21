#!/bin/bash
ssh rockpile@172.17.0.221 "/home/rockpile/tarErrLog.sh"
sleep 2
ssh rockpile@172.17.0.222 "/home/rockpile/tarErrLog.sh"
sleep 2
rm -rf /home/rockpile/ErrLog/221/*
rm -rf /home/rockpile/ErrLog/222/*
sleep 2
scp rockpile@172.17.0.221:/home/rockpile/error.tar.gz /home/rockpile/ErrLog/221/
sleep 2
scp rockpile@172.17.0.222:/home/rockpile/error.tar.gz /home/rockpile/ErrLog/222/
echo "Done!"
