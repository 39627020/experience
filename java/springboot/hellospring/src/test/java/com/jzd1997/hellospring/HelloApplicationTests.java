package com.jzd1997.hellospring;

import static org.hamcrest.Matchers.equalTo;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.content;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

import java.io.File;
import java.util.concurrent.Future;

import javax.mail.internet.MimeMessage;
import javax.transaction.Transactional;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.core.io.FileSystemResource;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.http.MediaType;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;

import com.jzd1997.hellospring.controller.JsonController;
import com.jzd1997.hellospring.domain.User;
import com.jzd1997.hellospring.domain.UserRepository;
import com.jzd1997.hellospring.task.AsyncTask;

import junit.framework.Assert;

@RunWith(SpringRunner.class)
@SpringBootTest
public class HelloApplicationTests {
	
	private MockMvc mvc;
	@Before
	public void setUp() throws Exception {
		mvc = MockMvcBuilders.standaloneSetup(new JsonController()).build();
	}
	@Test
	public void getHello() throws Exception {
		mvc.perform(MockMvcRequestBuilders.get("/jsonstring").accept(MediaType.APPLICATION_JSON))
				.andExpect(status().isOk())
				.andExpect(content().string(equalTo("json string")));
	}
	
	@Autowired
	private StringRedisTemplate stringRedisTemplate;
	@Autowired
	private UserRepository userRepository;
	
	@Test
	public void test() throws Exception {
		// 保存字符串
		stringRedisTemplate.opsForValue().set("aaa", "111");
		Assert.assertEquals("111", stringRedisTemplate.opsForValue().get("aaa"));
    }
	
	@Test
	public void testJpa(){
		userRepository.save(new User("韩菁菁",35));
		userRepository.save(new User("卓洋洋",36));
    }

//	@Test
//	public void testRedis(){
//		System.out.println(userRepository.findByName("卓洋洋").getAge());
//		System.out.println(userRepository.findByName("卓洋洋").getAge());
//    }

	@Test
	@Transactional
	public void testTransaction(){
		userRepository.save(new User("韩菁菁韩菁菁",35));
		userRepository.save(new User("卓洋洋卓洋洋卓洋洋卓洋洋卓洋洋卓洋洋卓洋洋卓洋洋卓洋洋卓洋",36));
    }	

	@Autowired
	private AsyncTask task;
	
	@Test
	public void testTask() throws Exception {
		long start = System.currentTimeMillis();
		Future<String> task1 = task.doTaskOne();
		Future<String> task2 = task.doTaskTwo();
		Future<String> task3 = task.doTaskThree();
		while(true) {
			if(task1.isDone() && task2.isDone() && task3.isDone()) {
				// 三个任务都调用完成，退出循环等待
				break;
			}
			Thread.sleep(1000);
		}
		long end = System.currentTimeMillis();
		System.out.println("任务全部完成，总耗时：" + (end - start) + "毫秒");
	}
	
	@Autowired
	private JavaMailSender mailSender;
	@Test
	public void testSendMail() throws Exception {
		SimpleMailMessage message = new SimpleMailMessage();
		message.setFrom("39627020@qq.com");
		message.setTo("1739001514@qq.com");
		message.setSubject("主题：简单邮件");
		message.setText("测试邮件内容");
		mailSender.send(message);
	}
	
	@Test
	public void sendAttachmentsMail() throws Exception {
		MimeMessage mimeMessage = mailSender.createMimeMessage();
		MimeMessageHelper helper = new MimeMessageHelper(mimeMessage, true);
		helper.setFrom("39627020@qq.com");
		helper.setTo("1739001514@qq.com");
		helper.setSubject("主题：有附件");
		helper.setText("有附件的邮件");
		FileSystemResource file = new FileSystemResource(new File("C:\\Users\\jiangzd\\Pictures\\docker.jpg"));
		helper.addAttachment("附件-1.jpg", file);
		mailSender.send(mimeMessage);
	}
}
