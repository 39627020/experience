package com.sysssc.com.test;

import java.util.List;

import org.junit.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;

public class LoginShanghai {
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
//        System.setProperty("webdriver.chrome.driver", "D:/chromedriver.exe");
    	System.setProperty("webdriver.chrome.driver", "/Users/zedongjiang/Downloads/chromedriver");
        driver = new ChromeDriver();
//        获取登录页面
        driver.get("http://localhost:8080/shanghai/");
//        获取用户名输入框
        driver.findElement(By.name("username")).clear();
        driver.findElement(By.name("username")).sendKeys("superadmin");
        
//        休息500ms，否则，速度太快，会将密码内容填充到用户名输入框中
        sleep(500);
//        获取密码输入框
        driver.findElement(By.name("password")).clear();
        driver.findElement(By.name("password")).sendKeys("!QAZzaq1");
//        休息8秒等待用户输入验证码
        sleep(8000);
        driver.findElement(By.name("password")).submit();
        sleep(5000);
        //添加新用户
        driver.findElement(By.linkText("添加用户")).click();
        driver.findElement(By.name("username")).clear();
        driver.findElement(By.name("username")).sendKeys("jingjing.han");
        driver.findElement(By.name("name")).clear();
        driver.findElement(By.name("name")).sendKeys("韩晶晶");
        driver.findElement(By.name("password")).clear();
        driver.findElement(By.name("password")).sendKeys("shanghai123");
        driver.findElement(By.name("repeatPassword")).clear();
        driver.findElement(By.name("repeatPassword")).sendKeys("shanghai123");
        driver.findElement(By.name("mobile")).clear();
        driver.findElement(By.name("mobile")).sendKeys("18615756895");
        driver.findElement(By.name("weixinid")).clear();
        driver.findElement(By.name("weixinid")).sendKeys("xinxinxing489088");
        List<WebElement> checkboxs = driver.findElements(By.xpath("//input[@type='checkbox']"));
        for(WebElement element:checkboxs){
        	element.click();
        	break;
        }
        ((JavascriptExecutor)driver).executeScript("chooseDepartment();");
        sleep(5000);
        driver.switchTo().frame(0); 
        WebElement element = driver.findElement(By.partialLinkText("销售"));
        element.click();
        driver.switchTo().defaultContent();
        driver.findElement(By.name("weixinid")).submit();
    }

    @Test
    public void testLogin(){
    	login();
    }


}