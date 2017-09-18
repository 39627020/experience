package test;

import org.junit.Test;

import redis.clients.jedis.Jedis;

public class TestApi {
	@Test
	public void testRand() {
		double r = Math.random();
		int rand = (int)(10.0*r);
		System.out.println("r:" + r + " rand:" + rand);
	}
	
	@Test
	public void testRedis() {
		Jedis jedis = new Jedis("172.17.0.61", 7200);
		jedis.set("username","xinxin");
		jedis.expire("username", 300);
		jedis.close();
	}
}
