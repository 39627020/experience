package com.jzd1997.hellospring.controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import com.jzd1997.hellospring.domain.User;
import com.jzd1997.hellospring.domain.UserRepository;

@Controller
@RequestMapping("/user")
public class UserController {
	@Autowired
	private UserRepository userRepository;
	
    @GetMapping("/index")
    @ResponseBody
    public List<User> index(Model model) {
    	List<User> userList = userRepository.findAll();
        return userList;
    }

    @GetMapping("/from")
    public String from(User user,Model model) {
    	Long uid = user.getId();
    	if(uid!=null) {
    		user = userRepository.getOne(uid);
    	}
    	model.addAttribute("user", user);
        return "user/from";
    }
    
    @PostMapping("/save")
    public String save(User user,Model model) {
    	userRepository.save(user);
    	return "redirect:/user/index";
    }

}
