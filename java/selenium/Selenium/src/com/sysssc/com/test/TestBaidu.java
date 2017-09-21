package com.sysssc.com.test;

import java.util.List;

import org.junit.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;

public class TestBaidu {
    private static WebDriver driver;

    //    定义自己的休眠方法，精简代码量
    private static void sleep(long time) {
        try {
            Thread.sleep(time);
        } catch (InterruptedException e) {
            e.printStackTrace();
            System.out.println(e.getMessage());
        }
    }

    //    登录操作，负责将界面跳转到交易记录界面
    private static void login() {
//        启动Chrome浏览器
    	
    	try{
            System.setProperty("webdriver.chrome.driver", "D:/work/git/selenium/chromedriver.exe");
//        	System.setProperty("webdriver.chrome.driver", "/Users/zedongjiang/Downloads/chromedriver");
            driver = new ChromeDriver();
//            获取登录页面
            driver.get("http://www.baidu.com/");
//            获取用户名输入框
            driver.findElement(By.name("wd")).clear();
            driver.findElement(By.name("wd")).sendKeys("斗地主");
            sleep(1000);
            driver.findElement(By.name("wd")).submit();
            sleep(4000);
            List<WebElement> links = driver.findElements(By.xpath("//a"));
            for(int i=0;i<links.size();i++){
            	String cls = links.get(i).getAttribute("data-landurl");
            	if(cls!=null){
//            		String href = links.get(i).getAttribute("href");
//            		JavascriptExecutor executor = (JavascriptExecutor) driver;
//            		executor.executeScript("window.open('" + href + "')");
                	links.get(i).click();
                	sleep(500);
            	}
            	//links.get(i).click();
            }    		
    	}catch(Exception e){
    		e.printStackTrace();
    	}
    }

    @Test
    public void testLogin(){
    	login();
    }


}