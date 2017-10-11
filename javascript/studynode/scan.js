var fs = require("fs");
console.log("查看 /tmp 目录");
var read = function(path){
    fs.readdir(path,function(err, files){
        if (err) {
            return console.error(err);
        }
        files.forEach( function (file){
            var subpath = path + '/' + file;
            console.log( subpath );
            fs.stat(subpath, function (err, stats) {
                if(stats.isDirectory()){
                    read(subpath);
                }
            })
        });
    });
};
read("d:/tmp");
