var fs = require('fs');

fs.stat('d:/tmp', function (err, stats) {
    console.log(stats.isFile());         //true
})