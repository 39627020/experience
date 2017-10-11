var mysql  = require('mysql');

var connection = mysql.createConnection({
    host     : 'localhost',
    user     : 'root',
    password : '!QAZzaq1',
    port: '3306',
    database: 'test',
});

connection.connect();

var  sql = 'SELECT * FROM user';
//查
connection.query(sql,function (err, result) {
    if(err){
        console.log('[SELECT ERROR] - ',err.message);
        return;
    }

    console.log('--------------------------SELECT----------------------------');
    for(i=0;i<result.length;i++){
        console.log(result[i].name);
    }
    result.forEach(function(re){
        console.log(re.name + ' ' + re.age);
    });
    console.log('------------------------------------------------------------\n\n');
});

connection.end();