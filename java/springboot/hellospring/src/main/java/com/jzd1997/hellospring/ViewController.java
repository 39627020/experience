package com.jzd1997.hellospring;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
public class ViewController {
	@RequestMapping(value="/hello/{name}")
    public String index(@PathVariable String name,Model model) {
		model.addAttribute("name", "你好，" + name + "!");
		return "hello";
    }
}
