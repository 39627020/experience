package test;

import org.junit.Test;

public class TestApi {
	@Test
	public void testRand() {
		double r = Math.random();
		int rand = (int)(10.0*r);
		System.out.println("r:" + r + " rand:" + rand);
	}
}
