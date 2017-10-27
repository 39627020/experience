package com.jzd1997.hellospring.controller;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import com.jzd1997.hellospring.domain.User;
import com.jzd1997.hellospring.result.RestResult;

@Controller
public class ViewController {
	@RequestMapping(value="/hello/{name}")
    public String index(@PathVariable String name,Model model) {
		model.addAttribute("name", "你好，" + name + "!");
		return "hello";
    }
	
	@RequestMapping(value="/index")
    public String index() {
		return "index";
    }

	@GetMapping(value="/login")
    public String login() {
		return "login";
    }

	@PostMapping(value="/login")
	@ResponseBody
    public RestResult logon(@RequestBody User user) {
		RestResult result = new RestResult();
		if("admin".equals(user.getUsername())) {
			result.setCode(RestResult.RESULT_OK);
		}else {
			result.setCode(-1);
			result.setMsg("错误的账号密码");
		}
		return result;
    }
}
