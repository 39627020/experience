package com.jzd1997.hellospring.exception;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.ResponseBody;

@ControllerAdvice
class ErrorHandler {
    public static final String DEFAULT_ERROR_VIEW = "error";
//    @ExceptionHandler(value = Exception.class)
//    public ModelAndView defaultErrorHandler(HttpServletRequest req, Exception e) throws Exception {
//        ModelAndView mav = new ModelAndView();
//        mav.addObject("exception", e);
//        mav.addObject("url", req.getRequestURL());
//        mav.setViewName(DEFAULT_ERROR_VIEW);
//        return mav;
//    }

    @ExceptionHandler(value = Exception.class)
    @ResponseBody
    public Map<String,Object> defaultErrorHandler(HttpServletRequest req, Exception e) throws Exception {
    	Map<String,Object> error = new HashMap<String,Object>();
    	error.put("url",req.getRequestURL());
    	error.put("msg",e.getMessage());
        return error;
    }

}