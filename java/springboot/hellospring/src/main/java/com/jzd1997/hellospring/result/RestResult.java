package com.jzd1997.hellospring.result;

public class RestResult {
	int code;
	String msg;
	public static final int RESULT_OK =0;
	public int getCode() {
		return code;
	}
	public void setCode(int code) {
		this.code = code;
	}
	public String getMsg() {
		return msg;
	}
	public void setMsg(String msg) {
		this.msg = msg;
	}
}
