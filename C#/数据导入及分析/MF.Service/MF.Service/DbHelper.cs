using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
namespace MF.Service
{
    public class DbManager
    {

        static DbManager()
        {
            LoadDbConfig();
        }

        static Dictionary<string, DbHelper> DbHelper_Dictionary = new Dictionary<string, DbHelper>();


        public static DbHelper GetDbHelper(string key)
        {
            return DbHelper_Dictionary[key];
        }
        

   
       
        public DbHelper this[string key]
        {
            get
            {
                return DbHelper_Dictionary[key];
            }
        }

        public static void LoadDbConfig()
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db.config");
            using (var fs = File.Open(path, FileMode.Open))
            {
                try
                {
                    var bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);
                    var BOM = new byte[] { 0xEF, 0xBB, 0xBF };
                    var index = 0;
                    if (bytes.Length > BOM.Length)
                    {
                        if (bytes[0] == BOM[0] && bytes[1] == BOM[1] && bytes[2] == BOM[2])
                            index = 3;
                    }
                    var txt = Encoding.UTF8.GetString(bytes, index, bytes.Length - index);

                    //var config = Json.DeserializeObject<Dictionary<string, string>>(txt);
                    var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(txt);
                    foreach (var k in config.Keys)
                    {
                        string connectStr = config[k];
                        //connectStr = MF.Common.Security.AES.Decrypt(connectStr);
                        DbHelper_Dictionary.Add(k, new DbHelper(connectStr));
                    }
                }
                catch (Exception e)
                {
                    Log.WriteError("加载db.config文件时异常:", e.Message);
                    Log.WriteError(e.StackTrace);
                }
               
            }
        }
    }
    public sealed class DbHelper
    {
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        string connectionString;

        public void SetConnectionString(string connectStr)
        {
            connectionString = connectStr;
        }
        #region 构造函数
        /// <summary>
        /// DatHelper构造函数
        /// </summary>
        public DbHelper()
        {
        }
        public DbHelper(string connectStr)
        {
            connectionString = connectStr;
        }
        #endregion

        #region 命令执行类 
        /// <summary>
        /// 执行指定SQL语句
        /// </summary>
        /// <param name="sql">要执行的SQL语句。</param>
        /// <returns>返回所受影响的数据行数。</returns>
        public int Execute(string sql)
        {
            return this.Execute(connectionString, sql);
        }
        /// <summary>
        /// 执行指定SQL语句或存储过程
        /// </summary>
        /// <param name="sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="type">SQL命令类型</param>
        /// <returns></returns>
        public int Execute(string sql, CommandType type)
        {
            return Execute(connectionString, sql, type);
        }
        /// <summary>
        /// 执行指定SQL语句
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="sql">要执行的SQL语句。</param>
        /// <returns>返回所受影响的数据行数</returns>
        public int Execute(string connectStr, string sql)
        {
            return this.Execute(connectStr, sql, CommandType.Text);
        }
        /// <summary>
        /// 执行指定SQL语句或存储过程
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="type">SQL命令类型</param>
        /// <returns>返回所受影响的数据行数</returns>
        public int Execute(string connectStr, string sql, CommandType type)
        {
            return SQLHelper.ExecuteNonQuery(connectStr, type, sql);
        }
        #endregion

        #region 执行存储过程

        /// <summary>
        /// 执行指定的存储过程并返回受影响的数据行数
        /// </summary>
        /// <param name="procedureName">需要执行的存储过程名称</param>
        /// <param name="parameters">存储过程需要的参数</param>
        /// <returns>返回所受影响的数据行数</returns>
        public int Execute(string procedureName, SqlParameter[] parameters)
        {
            return this.Execute(connectionString, procedureName, parameters);
        }
        /// <summary>
        /// 执行指定的存储过程并返回受影响的数据行数
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="procedureName">需要执行的存储过程名称</param>
        /// <param name="parameters">存储过程需要的参数</param>
        /// <returns>返回所受影响的数据行数</returns>
        public int Execute(string connectStr, string procedureName, SqlParameter[] parameters)
        {
            return SQLHelper.ExecuteNonQuery(connectStr, CommandType.StoredProcedure, procedureName, parameters);
        }

        /// <summary>
        /// 执行指定的存储过程并返回受影响的数据行数
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="procedureName">需要执行的存储过程名称</param>
        /// <param name="parameters">Sql过程需要的参数</param>
        /// <returns>返回所受影响的数据行数</returns>
        public int Execute(string sql,CommandType commandType, SqlParameter[] parameters)
        {
            return SQLHelper.ExecuteNonQuery(connectionString, commandType, sql, parameters);
        }

        public SqlBulkCopy GetBCP()
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            SqlBulkCopy copy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.KeepIdentity) { BatchSize = 500000 };
            return copy;
        }


        public SqlDataReader ExecuteReader(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = commandType;
            cmd.Parameters.Clear();
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);

        }

        #endregion

        #region 获取数据集
        /// <summary>
        /// 根据指定SQL语句并返回数据集。
        /// </summary>
        /// <param name="sql">要执行的SQL语句。</param>
        /// <returns>数据集DataSet</returns>
        public DataSet Query(string sql)
        {
            return this.Query(connectionString, sql);
        }
        /// <summary>
        /// 根据指定SQL语句并返回数据集
        /// </summary>
        /// <param name="sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="type">SQL命令类型</param>
        /// <returns>数据集DataSet</returns>
        public DataSet Query(string sql, CommandType type)
        {
            return this.Query(connectionString, sql, type);
        }
        /// <summary>
        /// 根据指定SQL语句并返回数据集
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="type">SQL命令类型</param>
        /// <returns>数据集DataSet</returns>
        public DataSet Query(string connectStr, string sql, CommandType type)
        {
            if (type == CommandType.StoredProcedure)
                return this.Query(connectStr, sql, null);
            else
                return this.Query(connectStr, sql);
        }
        /// <summary>
        /// 根据指定SQL语句并返回数据集
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="sql">要执行的SQL语句</param>
        /// <returns>数据集DataSet</returns>
        public DataSet Query(string connectStr, string sql)
        {
            string[] tableNames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            var ds = new DataSet();
            var connection = new SqlConnection();
            try
            {
                connection.ConnectionString = connectStr;
                SQLHelper.FillDataset(connection, CommandType.Text, sql, ds, tableNames);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                return ds;
            }
            catch (Exception e) { throw e; }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public DataSet QueryParam(string connectStr, string sql, SqlParameter[] param)
        {
            string[] tableNames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            var ds = new DataSet();
            var connection = new SqlConnection();
            try
            {
                connection.ConnectionString = connectStr;
                SQLHelper.FillDataset(connection, CommandType.Text, sql,  ds, tableNames, param);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                return ds;
            }
            catch (Exception e) { throw e; }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public DataSet QueryParam( string sql, SqlParameter[] param) {
           return QueryParam(connectionString, sql, param);
        }

        public DataRow QueryParamRow(string sql, SqlParameter[] parameters)
        {
            var ds = this.QueryParam(sql, parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            return null;
        }


        /// <summary>
        /// 根据SQL语句查询并返回数据表。
        /// </summary>
        /// <param name="connectStr">要执行的SQL语句。</param>
        /// <param name="sql">数据库连接字符串</param>
        /// <returns>数据表 DataTable</returns>
        public DataTable QueryParamTable(string sql, SqlParameter[] param)
        {
            var ds = this.QueryParam(sql, param);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }



        /// <summary>
        /// 根据指定SQL语句并返回数据集
        /// </summary>
        /// <param name="procedureName">需要执行的存储过程名称</param>
        /// <param name="parameters">存储过程需要的参数</param>
        /// <returns>数据集DataSet</returns>
        public DataSet Query(string procedureName, SqlParameter[] parameters)
        {
            return this.Query(connectionString, procedureName, parameters);
        }
        /// <summary>
        /// 根据指定SQL语句并返回数据集
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="procedureName">需要执行的存储过程名称</param>
        /// <param name="parameters">存储过程需要的参数</param>
        /// <returns>数据集DataSet</returns>
        public DataSet Query(string connectStr, string procedureName, SqlParameter[] parameters)
        {
            string[] tableNames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            var ds = new DataSet();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectStr;
            try
            {
                SQLHelper.FillDataset(connection, CommandType.StoredProcedure, procedureName, ds, tableNames, parameters);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                return ds;
            }
            catch (Exception e) { throw e; }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
        #endregion

        #region 获取数据行
        /// <summary>
        /// 根据指定SQL语句查询并返回数据集的第一行数据
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <returns>数据行 DataRow</returns>
        public DataRow QueryRow(string sql)
        {
            return this.QueryRow(connectionString, sql);
        }
        /// <summary>
        /// 根据指定SQL语句查询并返回数据集的第一行数据
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="sql">要执行的SQL语句</param>
        /// <returns>数据行 DataRow</returns>
        public DataRow QueryRow(string connectStr, string sql)
        {
            var ds = this.Query(connectStr, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            return null;
        }
        /// <summary>
        /// 根据指定SQL语句查询并返回数据集的第一行数据
        /// </summary>
        /// <param name="sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="type">SQL命令类型</param>
        /// <returns>数据行 DataRow</returns>
        public DataRow QueryRow(string sql, CommandType type)
        {
            return this.QueryRow(connectionString, sql, type);
        }
        /// <summary>
        /// 根据指定SQL语句查询并返回数据集的第一行数据
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="type">SQL命令类型</param>
        /// <returns>数据行 DataRow</returns>
        public DataRow QueryRow(string connectStr, string sql, CommandType type)
        {
            var ds = this.Query(connectionString, sql, type);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            return null;
        }
        /// <summary>
        /// 根据指定SQL语句查询并返回数据集的第一行数据
        /// </summary>
        /// <param name="procedureName">需要执行的存储过程名称</param>
        /// <param name="parameters">存储过程需要的参数</param>
        /// <returns>数据行 DataRow</returns>
        public DataRow QueryRow(string procedureName, SqlParameter[] parameters)
        {
            return this.QueryRow(connectionString, procedureName, parameters);
        }
        /// <summary>
        /// 根据指定SQL语句查询并返回数据集的第一行数据
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="procedureName">需要执行的存储过程名称</param>
        /// <param name="parameters">存储过程需要的参数</param>
        /// <returns>数据行 DataRow</returns>
        public DataRow QueryRow(string connectStr, string procedureName, SqlParameter[] parameters)
        {
            var ds = this.Query(connectStr, procedureName, parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            return null;
        }
        #endregion

        #region 获取数据表
        /// <summary>
        /// 根据SQL语句查询并返回数据表
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <returns>数据表 DataTable</returns>
        public DataTable QueryTable(string sql)
        {
            return this.QueryTable(connectionString, sql);
        }

        /// <summary>
        /// 根据SQL语句查询并返回数据表。
        /// </summary>
        /// <param name="connectStr">要执行的SQL语句。</param>
        /// <param name="sql">数据库连接字符串</param>
        /// <returns>数据表 DataTable</returns>
        public DataTable QueryTable(string connectStr, string sql)
        {
            var ds = this.Query(connectionString, sql);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 根据指定存储过程查询并返回数据表。
        /// </summary>
        /// <param name="procedureName">要执行的存储过程</param>
        /// <param name="parameters">存储过程需要的参数</param>
        /// <returns>数据表 DataTable</returns>
        public DataTable QueryTable(string procedureName, SqlParameter[] parameters)
        {
            return this.QueryTable(connectionString, procedureName, parameters);
        }
        /// <summary>
        /// 根据指定存储过程查询并返回数据表
        /// </summary>
        /// <param name="connectStr">数据库连接字符串</param>
        /// <param name="procedureName">要执行的存储过程</param>
        /// <param name="parameters">存储过程需要的参数</param>
        /// <returns>数据表 DataTable</returns>
        public DataTable QueryTable(string connectStr, string procedureName, SqlParameter[] parameters)
        {
            var ds = this.Query(connectionString, procedureName, parameters);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        #endregion
    }
}
