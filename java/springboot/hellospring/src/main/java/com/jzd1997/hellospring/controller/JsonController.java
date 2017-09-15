package com.jzd1997.hellospring.controller;

import java.util.HashMap;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cloud.client.discovery.DiscoveryClient;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import io.swagger.annotations.ApiImplicitParam;
import io.swagger.annotations.ApiImplicitParams;
import io.swagger.annotations.ApiOperation;

@RestController
public class JsonController {
    @Autowired
    DiscoveryClient discoveryClient;

    @GetMapping("/dc")
    public String dc() {
        String services = "Services: " + discoveryClient.getServices();
        System.out.println(services);
        return services;
    }

    @ApiOperation(value="获取JSON String", notes="notes:获取JSON String")
    @ApiImplicitParam(name = "id", value = "用户ID", required = true, dataType = "Long")
	@RequestMapping(value="/jsonstring")
    public String index(String name) {
        return "json string";
    }
	
    @ApiOperation(value="通过Map返回JSON String", notes="notes:返回Map的JSON String")
    @ApiImplicitParams({
            @ApiImplicitParam(name = "id", value = "用户ID", required = true, dataType = "Long"),
            @ApiImplicitParam(name = "user", value = "用户详细实体user", required = true, dataType = "User")
    })
	@RequestMapping("/jsonmap")
	public Map<String,Object> json(){
		Map<String,Object> result = new HashMap<String,Object>();
		result.put("Status", 1);
		result.put("Msg", "Success");
		return result;
	}
}
