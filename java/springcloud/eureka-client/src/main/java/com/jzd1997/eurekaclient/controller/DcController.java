package com.jzd1997.eurekaclient.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.cloud.client.discovery.DiscoveryClient;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;


@RestController
public class DcController {
    @Autowired
    DiscoveryClient discoveryClient;
    @Value("${server.name}")
    private String serverName;
    
    @GetMapping("/dc")
    public String dc() {
        String services = "From " + serverName + " Services: " + discoveryClient.getServices();
        System.out.println(services);
        return services;
    }
}