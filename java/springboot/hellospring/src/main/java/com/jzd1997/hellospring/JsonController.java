package com.jzd1997.hellospring;

import java.util.HashMap;
import java.util.Map;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class JsonController {
	@RequestMapping(value="/jsonstring")
    public String index(String name) {
        return "json string";
    }
	
	@RequestMapping("/jsonmap")
	public Map<String,Object> json(){
		Map<String,Object> result = new HashMap<String,Object>();
		result.put("Status", 1);
		result.put("Msg", "Success");
		return result;
	}
}
