package com.jzd1997.mybatis;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.annotation.Rollback;
import org.springframework.test.context.junit4.SpringRunner;

import com.jzd1997.mybatis.domain.User;
import com.jzd1997.mybatis.domain.UserMapper;

import junit.framework.Assert;

@RunWith(SpringRunner.class)
@SpringBootTest
public class MybatisApplicationTests {

	@Autowired
	private UserMapper userMapper;
	@Test
	@Rollback
	public void findByName() throws Exception {
		userMapper.insert("AAA", 20);
		User u = userMapper.findByName("AAA");
		Assert.assertEquals(20, u.getAge().intValue());
	}


}
