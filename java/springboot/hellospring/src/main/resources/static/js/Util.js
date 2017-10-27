var host = window.location.protocol + "//" + window.location.host;
var rootPath="/photo/";
var MEMBER_PRICE = 8;
var MEMBER_DISCOUNT = 0.8;
console.log(host);
/**
 * 表单序列化为JSON对象
 */
$.fn.serializeObject = function() {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function() {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [ o[this.name] ];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}

/**
 * 自定义post方法
 * url:路径
 * data:数据
 * func:回调函数
 */
$.custompost = function(url,data,func){
	$.ajax({ 
		type:"POST", 
		url:url, 
		dataType:"json",      
		contentType:"application/json",               
		data:JSON.stringify(data), 
		success:func
	});
}


/**
 * 自定义获取URL参数方法
 * name:URL中参数名
 */
$.getUrlParam = function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

/**
 * 获取纯文本编辑器
 * 
 * @param objId 
 * @param maxLength 最大长度
 * @returns
 */
function getTextEditor(objId,maxLength){
	if(!maxLength){
		maxLength = 80000;
	}
	return UE.getEditor(objId,{
    	toolbars:[['FullScreen','emotion', 'Undo', 'Redo','bold','Italic','Border','Superscript','Subscript','RemoveFormat','PastePlain','backColor','InsertOrderedList','InsertUnorderedList','Underline','Paragraph','FontFamily','FontSize','JustifyLeft','JustifyCenter','JustifyRight','JustifyJustify']]
		,maximumWords : maxLength
	});
}

/**
 * 获取图文编辑器
 * 
 * @param objId
 * @param maxLength 最大长度
 * @returns
 */
function getTextImageEditor(objId, maxLength){
	if(!maxLength){
		maxLength = 80000;
	}
	return UE.getEditor(objId,{
    	toolbars:[['FullScreen','emotion', 'simpleupload', 'Undo', 'Redo','bold','Italic','Border','Superscript','Subscript','RemoveFormat','PastePlain','backColor','InsertOrderedList','InsertUnorderedList','Underline','Paragraph','FontFamily','FontSize','JustifyLeft','JustifyCenter','JustifyRight','JustifyJustify']]
		,maximumWords : maxLength
	});
}

/**
 * 获取商城专业图文编辑器
 * @param objId
 * @param maxLength 最大长度 
 * @returns
 */
function getStoreEditor(objId, maxLength){
	if(!maxLength){
		maxLength = 10000;
	}
	return UE.getEditor(objId,{
    	toolbars:[['FullScreen','emotion', 'simpleupload', 'Undo', 'Redo','bold','Italic','Border','Superscript','Subscript','RemoveFormat','PastePlain','backColor','InsertOrderedList','InsertUnorderedList','Underline','Paragraph','FontFamily','FontSize','JustifyLeft','JustifyCenter','JustifyRight','JustifyJustify','mytemplet']]
		,maximumWords : maxLength
	});
}

//对Date的扩展，将 Date 转化为指定格式的String   
//月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
//年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
//例子：   
//(new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
//(new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function(fmt)   
{ //author: meizz   
var o = {   
 "M+" : this.getMonth()+1,                 //月份   
 "d+" : this.getDate(),                    //日   
 "h+" : this.getHours(),                   //小时   
 "m+" : this.getMinutes(),                 //分   
 "s+" : this.getSeconds(),                 //秒   
 "q+" : Math.floor((this.getMonth()+3)/3), //季度   
 "S"  : this.getMilliseconds()             //毫秒   
};   
if(/(y+)/.test(fmt))   
 fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));   
for(var k in o)   
 if(new RegExp("("+ k +")").test(fmt))   
fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));   
return fmt;   
} 

function getRequestParam(paras){ 
	var url = location.href;  
	var paraString = url.substring(url.indexOf("?")+1,url.length).split("&");  
	var paraObj = {}  
	for (i=0; j=paraString[i]; i++){  
		paraObj[j.substring(0,j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=")+1,j.length);  
	}  
	var returnValue = paraObj[paras.toLowerCase()];  
	if(typeof(returnValue)=="undefined"){  
		return "";  
	}else{  
		return returnValue;  
	}  
	
}

//创建轮播函数
/**
 * 
 * @param obj 轮播对象
 * @param ref_time 刷新时间
 */
function startPPT(obj, ref_time, countObj) {
	// 轮播循环
	ppt = setInterval(function() {
		var count = countObj.val();
		if(!count){
			count = 0;
		}
		var len = obj.find("ul li").length;
		obj.find("ul li").each(function(){
			if($(this).index() == count){
				$(this).fadeIn(1000);
			}else if($(this).index() == count - 1 || (count == 0 && $(this).index() == len - 1)){
				$(this).fadeOut(1000);
			}else{
				$(this).hide();
			}
		});
		count++;
		if(count == len){
			count = 0;
		}
		countObj.val(count);
	}, ref_time);
	return ppt;
}

/**
 * 查看图片
 * @param title
 * @param attaId
 */
function openImage(title,attaId){
    var url = rootPath+"attachment/downloadatt.do?attaId=" + attaId; 
    Util.openImageWindow(800,600,title,url,{},function(data){
        
    });
}

/**
 * 显示Message
 * @param type
 * @param title
 * @param msg
 */
function showDialog(type,title,msg,func){
	var dialog;
	if(type=="info"){
		$("#msg-icon").attr("class","mif-info");
		$("#msg-title").html(title);
		$("#msg-message").html(msg);
		$("#dialog").removeClass("error");
		$("#dialog").removeClass("warning");
		$("#dialog").addClass("success");
        dialog = $("#dialog").data('dialog');
	}else if(type=="warn"){
		$("#msg-icon").attr("class","mif-warning");
		$("#msg-title").html(title);
		$("#msg-message").html(msg);
		$("#dialog").removeClass("error");
		$("#dialog").removeClass("success");
		$("#dialog").addClass("warning");
        dialog = $("#dialog").data('dialog');
	}else{
		$("#msg-icon").attr("class","mif-cancel");
		$("#msg-title").html(title);
		$("#msg-message").html(msg);
		$("#dialog").removeClass("warning");
		$("#dialog").removeClass("success");
		$("#dialog").addClass("error");
        dialog = $("#dialog").data('dialog');
	}
	$("#msg-message").parent().css("text-align","center").css("margin-top","2em");
    dialog.open();
    $("span.dialog-close-button").click(func);
    return dialog;
}
/**
 * 全选所有checkbox
 */
function checkAll(){
	$('input:checkbox').each(function() {
		    $(this).prop('checked',true)
	});
}

/**
 * 全不选所有checkbox
 */
function uncheckAll(){
	$('input:checkbox').each(function() {
		$(this).prop('checked',false)
	});
}

/**
 * 转换时间格式
 * @param date
 */
function formatTime(date,format){
	var hh = ('0' + date.getHours()).slice(-2),
    ii = ('0' + date.getMinutes()).slice(-2),
    ss = ('0' + date.getSeconds()).slice(-2);
	format = format || 'hh:ii:ss';
	format = format.replace(/hh/ig, hh).replace(/ii/ig, ii).replace(/ss/ig, ss);
	var yyyy = ('000' + date.getFullYear()).slice(-4),
	    yy = yyyy.slice(-2),
	    mm = ('0' + (date.getMonth()+1)).slice(-2),
	    dd = ('0' + date.getDate()).slice(-2);
		format = format || 'yyyy-mm-dd';
	return format.replace(/yyyy/ig, yyyy).replace(/yy/ig, yy).replace(/mm/ig, mm).replace(/dd/ig, dd);
}

/*
 * 取得url参数
 */
function GetQueryString(name)
{
     var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
     var r = window.location.search.substr(1).match(reg);
     if(r!=null)return  unescape(r[2]); return null;
}

//参数,中文字符串  
//返回值:拼音首字母串数组  
function makePy(str){  
if(typeof(str) != "string")  
throw new Error(-1,"函数makePy需要字符串类型参数!");  
var arrResult = new Array(); //保存中间结果的数组  
for(var i=0,len=str.length;i<len;i++){  
//获得unicode码  
var ch = str.charAt(i);  
//检查该unicode码是否在处理范围之内,在则返回该码对映汉字的拼音首字母,不在则调用其它函数处理  
arrResult.push(checkCh(ch));  
}  
//处理arrResult,返回所有可能的拼音首字母串数组  
return mkRslt(arrResult);  
}  
function checkCh(ch){  
var uni = ch.charCodeAt(0);  
//如果不在汉字处理范围之内,返回原字符,也可以调用自己的处理函数  
if(uni > 40869 || uni < 19968)  
return ch; //dealWithOthers(ch);  
//检查是否是多音字,是按多音字处理,不是就直接在strChineseFirstPY字符串中找对应的首字母  
return (oMultiDiff[uni]?oMultiDiff[uni]:(strChineseFirstPY.charAt(uni-19968)));  
}  
function mkRslt(arr){  
var arrRslt = [""];  
for(var i=0,len=arr.length;i<len;i++){  
var str = arr[i];  
var strlen = str.length;  
if(strlen == 1){  
for(var k=0;k<arrRslt.length;k++){  
arrRslt[k] += str;  
}  
}else{  
var tmpArr = arrRslt.slice(0);  
arrRslt = [];  
for(k=0;k<strlen;k++){  
//复制一个相同的arrRslt  
var tmp = tmpArr.slice(0);  
//把当前字符str[k]添加到每个元素末尾  
for(var j=0;j<tmp.length;j++){  
tmp[j] += str.charAt(k);  
}  
//把复制并修改后的数组连接到arrRslt上  
arrRslt = arrRslt.concat(tmp);  
}  
}  
}  
return arrRslt;  
}  
//两端去空格函数  
String.prototype.trim = function() {    return this.replace(/(^\s*)|(\s*$)/g,""); }  


function toString(str){
	return (str==null)?"":str;
}

function formatDate(date) {
	return date.substr(5,5);
}
