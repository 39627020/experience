//hello.js 
function Hello() {
    var name;
    this.setName = function(thyName) {
        name = thyName;
    };
    this.sayHello = function() {
        console.log('Hello ' + name);
        console.log( __filename );
        console.log( __dirname );
    };
};
module.exports = Hello;