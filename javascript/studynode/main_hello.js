//main.js 
var Hello = require('./hello');
hello = new Hello();
hello.setName('Jiang Dong');
hello.sayHello();

process.on('exit', function(code) {
    console.log('退出码为:', code);
});
console.log("程序执行结束");