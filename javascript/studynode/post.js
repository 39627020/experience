var express = require('express');
var bodyParser = require('body-parser');

var app = express();

// see https://github.com/expressjs/body-parser
// 添加 body-parser 中间件就可以了
app.use(bodyParser.urlencoded({extended: false}));
app.use(bodyParser.json());

app.post('/json_post', function (req, res) {
 
   // 输出 JSON 格式
   var response = {
       "first_name":req.body.first_name,
       "last_name":req.body.last_name
   };
   console.log(response);
   res.end(JSON.stringify(response));
})

app.post('/form_post', function (req, res) {
 
   // 输出 JSON 格式
   var response = {
       "first_name":req.first_name,
       "last_name":req.last_name
   };
   console.log(response);
   res.end(JSON.stringify(response));
})

 
var server = app.listen(8000, function () {
 
  var host = server.address().address
  var port = server.address().port
 
  console.log("应用实例，访问地址为 http://%s:%s", host, port)
 
})