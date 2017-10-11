var mysql  = require('mysql');

var connection = mysql.createConnection({
    host     : 'localhost',
    user     : 'root',
    password : '!QAZzaq1',
    port: '3306',
    database: 'test',
});

connection.connect();

var  addSql = 'INSERT INTO user(age,name) VALUES(?,?)';
var  addSqlParams = ['35', '江东东'];
//增
connection.query(addSql,addSqlParams,function (err, result) {
    if(err){
        console.log('[INSERT ERROR] - ',err.message);
        return;
    }

    console.log('--------------------------INSERT----------------------------');
    console.log('INSERT ID:',result);
    console.log('-----------------------------------------------------------------\n\n');
});

connection.end();