package com.jzd1997.hellospring.task;

import java.text.SimpleDateFormat;
import java.util.Date;

import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

@Component
public class ScheduleTask {
    private static final SimpleDateFormat dateFormat = new SimpleDateFormat("HH:mm:ss");
    @Scheduled(fixedRate = 5000)
    public void reportCurrentTime() {
        System.out.println("每隔5秒执行：" + dateFormat.format(new Date()));
    }

    @Scheduled(cron="*/5 * * * * *")
    public void execByCron() {
        System.out.println("Crontab每隔5秒执行：" + dateFormat.format(new Date()));
    }
}