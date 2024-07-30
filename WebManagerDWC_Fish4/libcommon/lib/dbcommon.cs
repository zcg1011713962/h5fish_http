using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

public class SortByParam
{
    // 排序字段
    public string m_sortFieldName = "";
    // 是否升序, true升序,false降序
    public bool m_asc = true;
}

public class DbInfoParam
{
    public const int SERVER_TYPE_MASTER = 0;  // 主
    public const int SERVER_TYPE_SLAVE = 1;   // 备

    // 服务器ID
    public int ServerId { get; set; }

    // db名称
    public int DbName { set; get; }

    // 主，备
    public int ServerType { set; get; }
}

public class DbServerInfo
{
    // 主数据库IP，PlayerDB，它也作为关键字
    public string m_serverIp = "";
    public int m_serverId;
    public string m_serverName = "";

    // 日志数据库所在IP
    public string m_logDbIp = "";

    // playerdb备
    public string m_playerDbSlave;
    // logdb备
    public string m_logDbSlave;

    public string m_monitor;
}

public class CDBMgr<T> where T : CDBMgr<T>, new()
{
    protected static T s_db = null;
    // ip与下标的对应
    protected Dictionary<string, int> m_dbServer = new Dictionary<string, int>();
    protected CDBServer[] m_mongoServer = null;

    public static T getInstance()
    {
        if (s_db == null)
        {
            s_db = new T();
            s_db.open();
        }
        return s_db;
    }

    // 初始化
    public virtual void open() { throw new Exception("not implement open"); }

    // 返回指定数据库所在的服务器ID
    public virtual int getSpecialServerId(int dbid) { throw new Exception("not implement getSpecialServerId"); }

    public virtual DbServerInfo getDbInfo(string url) { throw new Exception("not implement getDbInfo"); }

    public virtual bool isAccountDb(string url) { throw new Exception("not implement isAccountDb"); }

    public virtual string getAccountDbURL() { throw new Exception("not implement getAccountDbURL"); }

    protected CDBServer create(string url, DbServerInfo dbInfo, bool special = false)
    {
        try
        {
            CDBServer tmp = new CDBServer();
            bool res = tmp.init<T>(url, dbInfo, this, special);
            if (!res)
                return null;
            return tmp;
        }
        catch (System.Exception ex)
        {
            LOGW.Info("连接数据库 {0} 时发生异常，异常信息{1}!", url, ex.ToString());
        }
        return null;
    }

    // 根据地址名称，取得下标
    public int getDbId(string pools, bool special = false)
    {
        if (m_dbServer.ContainsKey(pools))
        {
            DbServerInfo dbInfo = getDbInfo(pools);
            
            int index = m_dbServer[pools];
            if (m_mongoServer[index] == null)
            {
                m_mongoServer[index] = create(pools, dbInfo, special);
                if (m_mongoServer[index] == null)
                {
                    LOGW.Info("getDbId函数，无法联接数据库!");
                    return -1;
                }
            }
            return index;
        }
        LOGW.Info("没有找到地址:{0}，将采用原地址");
        return -1;
    }

    // 表是否存在
    public bool existTable(string tablename, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).TableExists(tablename);
        }
        catch (System.Exception ex)
        {
            return false;
        }
    }

    // 清空表中的所有数据
    public bool clearTable(string tablename, int serverid, int dbid)
    {
        if (!existTable(tablename, serverid, dbid))
            return true;

        return m_mongoServer[serverid].getDB(dbid).ExecuteRemoveAll(tablename);
    }

    //根据查询条件，key去重，并返回去重后key的数量
    public int executeDistinct(string tablename, int serverid, int dbname, string key, IMongoQuery query = null)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbname).ExecuteDistinct(tablename, key, query);
        }
        catch (System.Exception ex)
        {
            return 0;
        }
    }

    public int executeDistinct(string tablename, DbInfoParam dbParam, string key, IMongoQuery query = null)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteDistinct(tablename, key, query);
        }
        catch (System.Exception ex)
        {
            return 0;
        }
    }

    // 根据查询条件，在table内查询结果
    public List<Dictionary<string, object>> executeQuery(string tablename,                 // 表名
                                                         int serverid, 
                                                         int dbname,
                                                         IMongoQuery query = null,         // 查询条件，外部要自拼接
                                                         int skip = 0,
                                                         int limt = 0,
                                                         string[] fields = null,
                                                         string sort = "",
                                                         bool asc = true,
                                                         string[] indexes = null          // 索引
                                                         )
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbname).ExecuteQuery(tablename, query, fields, sort, asc, skip, limt, indexes);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    public List<Dictionary<string, object>> executeQuery(string tablename,                 // 表名
                                                        DbInfoParam dbParam,
                                                        IMongoQuery query = null,         // 查询条件，外部要自拼接
                                                        int skip = 0,
                                                        int limt = 0,
                                                        string[] fields = null,
                                                        string sort = "",
                                                        bool asc = true,
                                                        string[] indexes = null          // 索引
                                                        )
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteQuery(tablename, query, fields, sort, asc, skip, limt, indexes);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    public List<Dictionary<string, object>> executeQuery2(string tablename,                 // 表名
                                                          DbInfoParam dbParam,
                                                          IMongoQuery query = null,         // 查询条件，外部要自拼接
                                                          int skip = 0,
                                                          int limt = 0,
                                                          string[] fields = null,
                                                          SortByBuilder sort = null
                                                          )
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteQuery(tablename, query, fields, skip, limt, sort);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    public List<BsonDocument> executeQueryBsonDoc(string tablename,                 // 表名
                                                    int serverid,
                                                    int dbname,
                                                    IMongoQuery query = null,         // 查询条件，外部要自拼接
                                                    int skip = 0,
                                                    int limt = 0,
                                                    string[] fields = null,
                                                    string sort = "",
                                                    bool asc = true,
                                                    string[] indexes = null          // 索引
                                                    )
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbname).ExecuteQueryBsonDoc(tablename, query, fields, sort, asc, skip, limt, indexes);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    public List<BsonDocument> executeQueryBsonDoc(string tablename,                 // 表名
                                                    DbInfoParam dbParam,
                                                    IMongoQuery query = null,         // 查询条件，外部要自拼接
                                                    int skip = 0,
                                                    int limt = 0,
                                                    string[] fields = null,
                                                    string sort = "",
                                                    bool asc = true,
                                                    string[] indexes = null          // 索引
                                                    )
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteQueryBsonDoc(tablename, query, fields, sort, asc, skip, limt, indexes);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    // 根据查询条件，在table内查询结果
    public List<Dictionary<string, object>> executeQuery1(string tablename,                 // 表名
                                                         int serverid,
                                                         int dbname,
                                                         IMongoQuery query = null,         // 查询条件，外部要自拼接
                                                         int skip = 0,
                                                         int limt = 0,
                                                         string[] fields = null,
                                                         SortByParam[] sorts = null,
                                                         string[] indexes = null          // 索引
                                                         )
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbname).ExecuteQuery(tablename, query, fields, sorts, skip, limt, indexes);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    // 从表tablename中获取一个数据列表
    public List<Dictionary<string, object>> getDataListFromTable(string tablename,
                                                                 int serverid,
                                                                 int dbname,
                                                                 string key,
                                                                 object val,
                                                                 string[] fields = null,
                                                                 string sort = "",
                                                                 bool asc = true,
                                                                 int skip = 0,
                                                                 int limt = 0)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbname).ExecuteGetListBykey(tablename, key, val, fields, sort, asc, skip, limt);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    public List<Dictionary<string, object>> getDataListFromTable(string tablename,
                                                                DbInfoParam dbParam,
                                                                 string key,
                                                                 object val,
                                                                 string[] fields = null,
                                                                 string sort = "",
                                                                 bool asc = true,
                                                                 int skip = 0,
                                                                 int limt = 0)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteGetListBykey(tablename, key, val, fields, sort, asc, skip, limt);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    // 取得表tablename中， 字段名fieldname 为value行
    public Dictionary<string, object> getTableData(string tablename, string fieldname, object val, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteGetOneBykey(tablename, fieldname, val);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return null;
    }

    public Dictionary<string, object> getTableData(string tablename, string fieldname, object val, DbInfoParam dbParam)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteGetOneBykey(tablename, fieldname, val);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return null;
    }

    // 取得表tablename中， 字段名fieldname 为value行
    public Dictionary<string, object> getTableData(string tablename, string fieldname, object val, string[] fields, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteGetOneBykey(tablename, fieldname, val, fields);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return null;
    }

    public Dictionary<string, object> getTableData(string tablename, string fieldname, object val, string[] fields, DbInfoParam dbParam)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteGetOneBykey(tablename, fieldname, val, fields);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return null;
    }

    // 取得表tablename中， 字段名fieldname 为value行
    public Dictionary<string, object> getTableData(string tablename, int serverid, int dbid, IMongoQuery query, string[] fields = null)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteGetByQuery(tablename, query, fields);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return null;
    }

    public Dictionary<string, object> getTableData(string tablename, DbInfoParam dbParam, IMongoQuery query, string[] fields = null)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteGetByQuery(tablename, query, fields);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return null;
    }

    // 返回表tablename中满足条件的记录个数
    public long getRecordCount(string tablename, IMongoQuery query, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteGetCount(tablename, query);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return 0;
    }

    public long getRecordCount(string tablename, IMongoQuery query, DbInfoParam dbParam)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteGetCount(tablename, query);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return 0;
    }

    // 返回表tablename中满足条件的记录个数
    public long getRecordCount(string tablename, string fieldname, object val, int serverid, int dbid)
    {
        IMongoQuery imq = Query.EQ(fieldname, BsonValue.Create(val));
        return getRecordCount(tablename, imq, serverid, dbid);
    }

    public long getRecordCount(string tablename, string fieldname, object val, DbInfoParam dbParam)
    {
        IMongoQuery imq = Query.EQ(fieldname, BsonValue.Create(val));
        return getRecordCount(tablename, imq, dbParam);
    }

    // 将数据保存到tablename内
    public bool save(string tablename, Dictionary<string, object> data, string filedname, object val, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            Dictionary<string, object> tmp = m_mongoServer[serverid].getDB(dbid).ExecuteGetBykey(tablename, filedname, val);
            // 没有数据，新插入一行
            if (tmp == null)
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteInsert(tablename, data);
            }
            else // 有，则替换
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteStoreBykey(tablename, filedname, val, data);
            }
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool save(string tablename, Dictionary<string, object> data, string filedname, object val, DbInfoParam dbParam)
    {
        if (data == null)
            return false;
        try
        {
            CMongodbHelper db = m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType);
            Dictionary<string, object> tmp = db.ExecuteGetBykey(tablename, filedname, val);
            // 没有数据，新插入一行
            if (tmp == null)
            {
                return db.ExecuteInsert(tablename, data);
            }
            else // 有，则替换
            {
                return db.ExecuteStoreBykey(tablename, filedname, val, data);
            }
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 将数据保存到tablename内
    public bool save(string tablename, Dictionary<string, object> data, IMongoQuery query, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            Dictionary<string, object> tmp = m_mongoServer[serverid].getDB(dbid).ExecuteGetByQuery(tablename, query);
            // 没有数据，新插入一行
            if (tmp == null)
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteInsert(tablename, data);
            }
            else // 有，则替换
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteStoreByQuery(tablename, query, data);
            }
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool save(string tablename, Dictionary<string, object> data, IMongoQuery query, DbInfoParam dbParam)
    {
        if (data == null)
            return false;
        try
        {
            CMongodbHelper db = m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType);
            Dictionary<string, object> tmp = db.ExecuteGetByQuery(tablename, query);
            // 没有数据，新插入一行
            if (tmp == null)
            {
                return db.ExecuteInsert(tablename, data);
            }
            else // 有，则替换
            {
                return db.ExecuteStoreByQuery(tablename, query, data);
            }
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 更新数据表中某些字段值
    public bool update(string tablename, Dictionary<string, object> data, IMongoQuery query, int serverid, int dbid, bool noAutoInsert = false)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteUpdateByQuery(tablename, query, data,
                noAutoInsert ? UpdateFlags.Upsert : UpdateFlags.None);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool update(string tablename, Dictionary<string, object> data, IMongoQuery query, DbInfoParam dbParam, bool noAutoInsert = false)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteUpdateByQuery(tablename, query, data,
                noAutoInsert ? UpdateFlags.Upsert : UpdateFlags.None);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 更新数据表中某些字段值
    public bool update(string tablename, Dictionary<string, object> data, string filedname, object val, int serverid, int dbid, bool noAutoInsert = false)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteUpdate(tablename, filedname, val, data,
                noAutoInsert ? UpdateFlags.Upsert : UpdateFlags.None);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool update(string tablename, Dictionary<string, object> data, string filedname, object val, DbInfoParam dbParam, bool noAutoInsert = false)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteUpdate(tablename, filedname, val, data,
                noAutoInsert ? UpdateFlags.Upsert : UpdateFlags.None);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 原记录存在，则更新，否则会添加一条新记录。
    public bool store(string tablename, Dictionary<string, object> data, string filedname, object val, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteStoreBykey(tablename, filedname, val, data);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool store(string tablename, Dictionary<string, object> data, string filedname, object val, DbInfoParam dbParam)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteStoreBykey(tablename, filedname, val, data);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 向表tablename中新增数据，若指定关键字的数据已存在，则不添加
    public bool addTableData(string tablename, Dictionary<string, object> data, string filedname, object val, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            Dictionary<string, object> tmp = m_mongoServer[serverid].getDB(dbid).ExecuteGetBykey(tablename, filedname, val);
            // 没有数据，新插入一行
            if (tmp == null)
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteInsert(tablename, data);
            }
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool addTableData(string tablename, Dictionary<string, object> data, string filedname, object val, DbInfoParam dbParam)
    {
        if (data == null)
            return false;
        try
        {
            var db = m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType);
            Dictionary<string, object> tmp = db.ExecuteGetBykey(tablename, filedname, val);
            // 没有数据，新插入一行
            if (tmp == null)
            {
                return db.ExecuteInsert(tablename, data);
            }
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 从数据库移除记录
    public bool remove(string tablename, string filedname, object val, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteRemoveBykey(tablename, filedname, val);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool remove(string tablename, string filedname, object val, DbInfoParam dbParam)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteRemoveBykey(tablename, filedname, val);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 删除表中所有记录
    public bool removeAll(string tablename, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteRemoveAll(tablename);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool removeAll(string tablename, DbInfoParam dbParam)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteRemoveAll(tablename);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 插入一条记录
    public bool insertData(string table, Dictionary<string, object> data,  int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteInsert(table, data);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool insertData(string table, Dictionary<string, object> data, DbInfoParam dbParam)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteInsert(table, data);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool insertData(string table, List<Dictionary<string, object>> data, int serverid, int dbid, List<int> failIndex = null)
    {
        try
        {
            List<BsonDocument> docList = new List<BsonDocument>();
            for (int i = 0; i < data.Count; i++)
            {
                docList.Add(new BsonDocument(data[i]));
            }
            return m_mongoServer[serverid].getDB(dbid).ExecuteInsterList(table, docList, failIndex);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool insertData(string table, List<Dictionary<string, object>> data, DbInfoParam dbParam, List<int> failIndex = null)
    {
        try
        {
            List<BsonDocument> docList = new List<BsonDocument>();
            for (int i = 0; i < data.Count; i++)
            {
                docList.Add(new BsonDocument(data[i]));
            }
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).ExecuteInsterList(table, docList, failIndex);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 是否存在某条数据
    public bool keyExists(string table, string key, object val, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).KeyExistsBykey(table, key, val);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public bool keyExists(string table, string key, object val, DbInfoParam dbParam)
    {
        try
        {
            return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).KeyExistsBykey(table, key, val);
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    // 执行map-reduce
    public MapReduceResult executeMapReduce(string tablename, int serverid, int dbname, IMongoQuery query,
                                            string map_js, string reduce_js,
                                            string outTableName = "")
    {
        return m_mongoServer[serverid].getDB(dbname).executeMapReduce(tablename, query, map_js, reduce_js, outTableName);
    }

    public MapReduceResult executeMapReduce(string tablename, DbInfoParam dbParam, IMongoQuery query,
                                           string map_js, string reduce_js,
                                           string outTableName = "")
    {
        return m_mongoServer[dbParam.ServerId].getDB(dbParam.DbName, dbParam.ServerType).executeMapReduce(tablename, query, map_js, reduce_js, outTableName);
    }
}

public class CMongodbHelper
{
    private MongoDatabase mMongodbClient = null;

    public CMongodbHelper()
    {
    }

    public MongoDatabase MongoDb
    {
        get
        {
            return mMongodbClient;
        }
        set
        {
            mMongodbClient = value;
        }
    }

    MongoCollection<BsonDocument> check_table(string tablename, string indexname = "")
    {
        MongoCollection<BsonDocument> tmp = null;

        try
        {
//             if (!mMongodbClient.CollectionExists(tablename))
//             {
//                 CommandResult cr = mMongodbClient.CreateCollection(tablename);
//                 tmp = mMongodbClient.GetCollection(tablename);
//             }
//             else
            {
                tmp = mMongodbClient.GetCollection(tablename);
            }

//             if (indexname != "" && !tmp.IndexExists(indexname))
//                 tmp.CreateIndex(indexname);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return tmp;
    }

    public MongoCollection<BsonDocument> check_table_keys(string tablename, string[] indexes = null)
    {
        MongoCollection<BsonDocument> tmp = null;

        try
        {
            if (!mMongodbClient.CollectionExists(tablename))
            {
                CommandResult cr = mMongodbClient.CreateCollection(tablename);
                tmp = mMongodbClient.GetCollection(tablename);
            }
            else
            {
                tmp = mMongodbClient.GetCollection(tablename);
            }

            if (indexes != null && indexes.Length > 0)
            {
                foreach (var it in indexes)
                {
                    if (it != "" && !tmp.IndexExists(it))
                        tmp.CreateIndex(it);
                }
            }
        }
        catch (Exception ex)
        {
            LOGW.Info(ex.ToString());
        }

        return tmp;
    }

    // 检测表tablename是否存在
    public bool TableExists(string tablename)
    {
        return mMongodbClient.CollectionExists(tablename);
    }

    public bool KeyExists(string table, object val)
    {
        return KeyExistsBykey(table, "_key", val);
    }

    public bool KeyExistsBykey(string table, string key, object val)
    {
        try
        {
            var cb = check_table(table, key);

            long count = cb.Count(Query.EQ(key, BsonValue.Create(val)));
            if (count > 0)
                return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return false;
    }

    public bool ExecuteStore(string table, object val, Dictionary<string, object> data)
    {
        return ExecuteStoreBykey(table, "_key", val, data);
    }

    public bool ExecuteStoreBykey(string table, string key, object val, Dictionary<string, object> data)
    {
        try
        {
            if (!data.ContainsKey(key))
                data.Add(key, val);

            var cb = check_table(table, key);
            var retu = cb.Update(Query.EQ(key, BsonValue.Create(val)), new UpdateDocument(data), UpdateFlags.Upsert);
            return retu.Ok;
            //return retu.LastErrorMessage;
        }
        catch (Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return true;
    }

    public bool ExecuteStoreByQuery(string table, IMongoQuery queries, Dictionary<string, object> data)
    {
        try
        {
            var cb = check_table(table);
            var retu = cb.Update(queries, new UpdateDocument(data), UpdateFlags.Upsert);

            return retu.Ok;
        }
        catch (Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public long ExecuteInc(string table, object val, long def = 1, long iv = 1)
    {
        return ExecuteIncBykey(table, "_key", val, "Count", def, iv);
    }

    public long ExecuteIncBykey(string table, string key, object val, string name = "Count", long def = 1, long iv = 1)
    {
        long nv = 0;
        try
        {
            var cb = check_table(table, key);
            var retf = cb.Find(Query.EQ(key, BsonValue.Create(val)));
            retf = retf.SetFields(new string[] { name });
            retf = retf.SetLimit(1);
            var it = retf.GetEnumerator();
            if (it.MoveNext())
            {
                nv = it.Current[name].AsInt64;
            }

            if (nv < def)
            {
                iv = def;
                nv += iv;
            }

            var retu = cb.Update(Query.EQ(key, BsonValue.Create(val)), Update.Inc(name, iv), UpdateFlags.Upsert);

            if (!retu.Ok)
                return nv;


            return nv;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return nv;
    }

    public void ExecuteRemove(string table, object val)
    {
        ExecuteRemoveBykey(table, "_key", val);
    }

    // 从表table删除某条数据，条件是：字段名 key = val
    public bool ExecuteRemoveBykey(string table, string key, object val)
    {
        try
        {
            var cb = check_table(table, key);
            var ret = cb.Remove(Query.EQ(key, BsonValue.Create(val)));
            return ret.Ok;
        }
        catch (Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return false;
    }

    public Dictionary<string, object> ExecuteGet(string table, object val)
    {
        return ExecuteGetBykey(table, "_key", val);
    }

    public Dictionary<string, object> ExecuteGetBykey(string table, string key, object val)
    {
        Dictionary<string, object> retval = null;
        try
        {
            var cb = check_table(table, key);
            var retf = cb.FindOne(Query.EQ(key, BsonValue.Create(val)));

            if (retf != null)
            {
                if (retf.Contains("_id"))
                    retf.Remove("_id");
                retval = retf.ToDictionary();
            }
            return retval;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return retval;
    }

    // 向表table中增加一行数据
    public bool ExecuteInsert(string table, Dictionary<string, object> data)
    {
        try
        {
            var cb = check_table(table);
            var retf = cb.Insert(new BsonDocument(data));
            return retf.Ok;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return false;
    }

    public bool ExecuteInsterList(string table, List<BsonDocument> blist, List<int> resIndex = null)
    {
        if (blist == null || blist.Count == 0)
            return false;
        try
        {

            var cb = mMongodbClient.GetCollection(table);
            var ret = cb.InsertBatch(blist);

            if (resIndex != null)
            {
                int i = 0;
                var it = ret.GetEnumerator();
                while (it.MoveNext())
                {
                    if (!it.Current.Ok)
                    {
                        resIndex.Add(i);
                    }
                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    public bool ExecuteUpdate(string table, string fieldname, object val, Dictionary<string, object> data, UpdateFlags flags = UpdateFlags.None)
    {
        return ExecuteUpdateByQuery(table, Query.EQ(fieldname, BsonValue.Create(val)), data, flags);
    }

    public bool ExecuteUpdateByQuery(string table, IMongoQuery queries, Dictionary<string, object> data, UpdateFlags flags = UpdateFlags.None)
    {
        try
        {
            var cb = check_table(table);

            UpdateBuilder ub = new UpdateBuilder();
            foreach (var item in data)
            {
                ub = ub.Set(item.Key, BsonValue.Create(item.Value));
            }

            var retu = cb.Update(queries, ub, flags);
            return retu.Ok;
        }
        catch (Exception ex)
        {
        }
        return false;
    }

    public List<Dictionary<string, object>> ExecuteGetList(string table, object val, string[] fields = null,
        string sort = "", bool asc = true, int skip = 0, int limt = 0)
    {
        return ExecuteGetListBykey(table, "_key", val, fields, sort, asc, skip, limt);
    }

    public List<Dictionary<string, object>> ExecuteGetListBykey(string table, string key, object val, string[] fields = null,
        string sort = "", bool asc = true, int skip = 0, int limt = 0)
    {
        List<Dictionary<string, object>> retlist = new List<Dictionary<string, object>>();
        try
        {
            var cb = check_table(table, key);
            //IMongoQuery imq = Query.And(new IMongoQuery[]{Query.EQ(key, BsonValue.Create(val)),Query.NotExists(""),})

            var ret = cb.Find(Query.EQ(key, BsonValue.Create(val)));

            if (fields != null)
                ret = ret.SetFields(fields);

            if (sort != string.Empty)
            {
                if (asc)
                    ret = ret.SetSortOrder(SortBy.Ascending(sort));
                else
                    ret = ret.SetSortOrder(SortBy.Descending(sort));
            }

            if (skip > 0)
                ret = ret.SetSkip(skip);
            if (limt > 0)
                ret = ret.SetLimit(limt);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
                if (it.Current.Contains("_id"))
                    it.Current.Remove("_id");
                retlist.Add(it.Current.ToDictionary());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            retlist.Clear();
        }
        return retlist;
    }

    //去重数量
    public int ExecuteDistinct(string table, string key, IMongoQuery imq)
    {
        int result = 0;
        try
        {
            var cb = mMongodbClient.GetCollection(table);
            var r = cb.Distinct(key, imq);
            foreach (var item in r)
            {
                result++;
            }
        }
        catch (Exception)
        {

        }
        return result;
    }

    // 根据查询条件，在table内查询结果
    public List<Dictionary<string, object>> ExecuteQuery(string table,                 // 表名
                                                         IMongoQuery query,            // 查询条件，外部要自拼接
                                                         string[] fields = null,
                                                         string sort = "",
                                                         bool asc = true,
                                                         int skip = 0,
                                                         int limt = 0,
                                                         string[] indexes = null)
    {
        // 没有设置条件时，返回全部数据
        if (query == null)
            return ExecuteGetAll(table, fields, sort, asc, skip, limt);

        // 表不存在，直接返回
        if (!TableExists(table))
            return null;

        List<Dictionary<string, object>> retlist = new List<Dictionary<string, object>>();
        try
        {
            var cb = check_table_keys(table, indexes);
            //IMongoQuery imq = Query.And(new IMongoQuery[]{Query.EQ(key, BsonValue.Create(val)),Query.NotExists(""),})

            var ret = cb.Find(query);

            if (fields != null)
                ret = ret.SetFields(fields);

            if (sort != string.Empty)
            {
                if (asc)
                    ret = ret.SetSortOrder(SortBy.Ascending(sort));
                else
                    ret = ret.SetSortOrder(SortBy.Descending(sort));
            }

            if (skip > 0)
                ret = ret.SetSkip(skip);
            if (limt > 0)
                ret = ret.SetLimit(limt);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
                //if (it.Current.Contains("_id"))
                   // it.Current.Remove("_id");
                retlist.Add(it.Current.ToDictionary());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            retlist.Clear();
        }
        return retlist;
    }

    public List<Dictionary<string, object>> ExecuteQuery(string table,                 // 表名
                                                         IMongoQuery query,            // 查询条件，外部要自拼接
                                                         string[] fields = null,
                                                         int skip = 0,
                                                         int limt = 0,
                                                         SortByBuilder sort = null)
    {
        // 没有设置条件时，返回全部数据
        if (query == null)
            return ExecuteGetAll(table, fields, skip, limt, sort);

        // 表不存在，直接返回
        if (!TableExists(table))
            return null;

        List<Dictionary<string, object>> retlist = new List<Dictionary<string, object>>();
        try
        {
            var cb = check_table_keys(table);
            //IMongoQuery imq = Query.And(new IMongoQuery[]{Query.EQ(key, BsonValue.Create(val)),Query.NotExists(""),})

            var ret = cb.Find(query);

            if (fields != null)
                ret = ret.SetFields(fields);

            if (sort != null)
            {
                ret = ret.SetSortOrder(sort);
            }
            //             if (sort != string.Empty)
            //             {
            //                 if (asc)
            //                     ret = ret.SetSortOrder(SortBy.Ascending(sort));
            //                 else
            //                     ret = ret.SetSortOrder(SortBy.Descending(sort));
            //             }

            if (skip > 0)
                ret = ret.SetSkip(skip);
            if (limt > 0)
                ret = ret.SetLimit(limt);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
                //if (it.Current.Contains("_id"))
                // it.Current.Remove("_id");
                retlist.Add(it.Current.ToDictionary());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            retlist.Clear();
        }
        return retlist;
    }

    public List<BsonDocument> ExecuteQueryBsonDoc(string table,                 // 表名
                                                    IMongoQuery query,            // 查询条件，外部要自拼接
                                                    string[] fields = null,
                                                    string sort = "",
                                                    bool asc = true,
                                                    int skip = 0,
                                                    int limt = 0,
                                                    string[] indexes = null)
    {
        // 表不存在，直接返回
        if (!TableExists(table))
            return null;

        List<BsonDocument> retlist = new List<BsonDocument>();
        try
        {
            var cb = check_table_keys(table, indexes);
            MongoCursor<BsonDocument> ret = null;
            if (query == null)
            {
                ret = cb.FindAll();
            }
            else
            {
                ret = cb.Find(query);
            }
            
            if (fields != null)
                ret = ret.SetFields(fields);

            if (sort != string.Empty)
            {
                if (asc)
                    ret = ret.SetSortOrder(SortBy.Ascending(sort));
                else
                    ret = ret.SetSortOrder(SortBy.Descending(sort));
            }

            if (skip > 0)
                ret = ret.SetSkip(skip);
            if (limt > 0)
                ret = ret.SetLimit(limt);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
                retlist.Add(it.Current);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            retlist.Clear();
        }
        return retlist;
    }

    // 根据查询条件，在table内查询结果
    public List<Dictionary<string, object>> ExecuteQuery(string table,                 // 表名
                                                         IMongoQuery query,            // 查询条件，外部要自拼接
                                                         string[] fields = null,
                                                         SortByParam []sort = null,
                                                         int skip = 0,
                                                         int limt = 0,
                                                         string[] indexes = null)
    {
        // 没有设置条件时，返回全部数据
        if (query == null)
            return ExecuteGetAll_1(table, fields, sort, skip, limt);

        // 表不存在，直接返回
        if (!TableExists(table))
            return null;

        List<Dictionary<string, object>> retlist = new List<Dictionary<string, object>>();
        try
        {
            var cb = check_table_keys(table, indexes);
            //IMongoQuery imq = Query.And(new IMongoQuery[]{Query.EQ(key, BsonValue.Create(val)),Query.NotExists(""),})

            var ret = cb.Find(query);

            if (fields != null)
                ret = ret.SetFields(fields);

//             if (sort != string.Empty)
//             {
//                 if (asc)
//                     ret = ret.SetSortOrder(SortBy.Ascending(sort));
//                 else
//                     ret = ret.SetSortOrder(SortBy.Descending(sort));
//             }

            if (skip > 0)
                ret = ret.SetSkip(skip);
            if (limt > 0)
                ret = ret.SetLimit(limt);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
                if (it.Current.Contains("_id"))
                    it.Current.Remove("_id");
                retlist.Add(it.Current.ToDictionary());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            retlist.Clear();
        }
        return retlist;
    }

    public Dictionary<string, object> ExecuteGetByQuery(string table, IMongoQuery queries, string[] fields = null)
    {
        Dictionary<string, object> retval = null;
        try
        {
            var cb = check_table(table);
            var retf = cb.Find(queries);

            if (fields != null)
                retf = retf.SetFields(fields);

            var it = retf.GetEnumerator();

            if (it.MoveNext())
            {
                if (it.Current.Contains("_id"))
                    it.Current.Remove("_id");
                retval = it.Current.ToDictionary();
            }
            return retval;
        }
        catch (Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return retval;
    }

    public List<Dictionary<string, object>> ExecuteGetAll(string table, string[] fields = null, string sort = "", bool asc = true, int skip = 0, int limt = 0)
    {
        List<Dictionary<string, object>> retlist = new List<Dictionary<string, object>>();
        try
        {
            var cb = check_table(table);
            var ret = cb.FindAll();

            if (fields != null)
                ret = ret.SetFields(fields);

            if (sort != string.Empty)
            {
                if (asc)
                    ret = ret.SetSortOrder(SortBy.Ascending(sort));
                else
                    ret = ret.SetSortOrder(SortBy.Descending(sort));
            }

            if (skip > 0)
                ret = ret.SetSkip(skip);
            if (limt > 0)
                ret = ret.SetLimit(limt);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
               // if (it.Current.Contains("_id"))
                  //  it.Current.Remove("_id");
                retlist.Add(it.Current.ToDictionary());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return retlist;
    }

    public List<Dictionary<string, object>> ExecuteGetAll(string table, string[] fields = null, int skip = 0, int limt = 0, SortByBuilder sort = null)
    {
        List<Dictionary<string, object>> retlist = new List<Dictionary<string, object>>();
        try
        {
            var cb = check_table(table);
            var ret = cb.FindAll();

            if (fields != null)
                ret = ret.SetFields(fields);

            if (sort != null)
            {
                ret = ret.SetSortOrder(sort);
            }
            //             if (sort != string.Empty)
            //             {
            //                 if (asc)
            //                     ret = ret.SetSortOrder(SortBy.Ascending(sort));
            //                 else
            //                     ret = ret.SetSortOrder(SortBy.Descending(sort));
            //             }

            if (skip > 0)
                ret = ret.SetSkip(skip);
            if (limt > 0)
                ret = ret.SetLimit(limt);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
                // if (it.Current.Contains("_id"))
                //  it.Current.Remove("_id");
                retlist.Add(it.Current.ToDictionary());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return retlist;
    }

    public List<Dictionary<string, object>> ExecuteGetAll_1(string table, string[] fields = null, 
       SortByParam [] sort = null,
        int skip = 0, 
        int limt = 0)
    {
        List<Dictionary<string, object>> retlist = new List<Dictionary<string, object>>();
        try
        {
            var cb = check_table(table);
            var ret = cb.FindAll();

            if (fields != null)
                ret = ret.SetFields(fields);

            if (sort != sort)
            {
//                 for(int i = 0; i < sort.Length; i++)
//                 {
// 
//                 }
//                 if (asc)
//                     ret = ret.SetSortOrder(SortBy.Ascending(sort));
//                 else
//                     ret = ret.SetSortOrder(SortBy.Descending(sort));
            }

            if (skip > 0)
                ret = ret.SetSkip(skip);
            if (limt > 0)
                ret = ret.SetLimit(limt);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
                if (it.Current.Contains("_id"))
                    it.Current.Remove("_id");
                retlist.Add(it.Current.ToDictionary());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return retlist;
    }

    // 清空表内的所有数据
    public bool ExecuteRemoveAll(string table)
    {
        try
        {
            var cb = check_table(table);
            var retf = cb.RemoveAll();
            return retf.Ok;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return false;
    }

    public Dictionary<string, object> ExecuteGetOne(string table, object val, string[] fields = null)
    {
        return ExecuteGetOneBykey(table, "_key", val, fields);
    }

    public Dictionary<string, object> ExecuteGetOneBykey(string table, string key, object val, string[] fields = null)
    {
        Dictionary<string, object> retlist = null;
        try
        {
            var cb = check_table(table, key);
            var ret = cb.Find(Query.EQ(key, BsonValue.Create(val)));

            if (fields != null)
                ret = ret.SetFields(fields);

            ret = ret.SetLimit(1);

            var it = ret.GetEnumerator();

            while (it.MoveNext())
            {
                if (it.Current.Contains("_id"))
                    it.Current.Remove("_id");
                retlist = it.Current.ToDictionary();
                break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return retlist;
    }

    public string ExecuteSaveList(string table, string key, List<Dictionary<string, object>> datalist)
    {
        if (datalist == null)
            return "ExecuteSaveList error: datalist is null";
        try
        {
            var cb = check_table(table, key);

            foreach (var it in datalist)
            {
                var ret = cb.Update(Query.EQ(key, BsonValue.Create(it[key])), new UpdateDocument(it), UpdateFlags.Upsert);
                if (!ret.Ok)
                    return ret.LastErrorMessage;
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return string.Empty;
    }

    // 返回表table中，满足查询条件的记录个数
    public long ExecuteGetCount(string table, IMongoQuery queries)
    {
        try
        {
            var cb = check_table(table);
            if (queries == null)
            {
                return cb.Count();
            }
            return cb.Count(queries);
        }
        catch (Exception ex)
        {
            LOGW.Info(ex.ToString());
        }
        return 0;
    }

    public MapReduceResult executeMapReduce(string table, IMongoQuery query, string map_js, string reduce_js,
        string outTableName = "")
    {
        try
        {
            var cb = check_table(table);
            MapReduceArgs args = new MapReduceArgs();
            args.MapFunction = new BsonJavaScript(map_js);
            args.ReduceFunction = new BsonJavaScript(reduce_js);
            args.Query = query;

            if (!string.IsNullOrEmpty(outTableName))
            {
                args.OutputMode = MapReduceOutputMode.Replace;
                args.OutputCollectionName = outTableName;
            }
          
            var ret = cb.MapReduce(args);
            if(ret.Ok)
                return ret;
        }
        catch (Exception ex)
        {
            LOGW.Info(ex.Message);
        }
        return null;
    }
}

// 一个server上的db列表
public class DbName
{
    // 玩家信息db
    public const int DB_PLAYER = 0;

    // 统计相关的数据库
    public const int DB_PUMP = 1;

    // 玩家账号库
    public const int DB_ACCOUNT = 2;

    // 充值记录数据库
    public const int DB_PAYMENT = 3;

    // 具体游戏相关数据库
    public const int DB_GAME = 4;

    // 配置数据库
    public const int DB_CONFIG = 5;

    public const int DB_NAME_MAX = 6;
}

// 一组db,含了主库，备库。主库，备库各连接具体数据库。
public class CDBServer
{
    private CMongodbHelper[] m_client = null;
    private CMongodbHelper[] m_clientSlave = null;
    public static string[] m_dbName = new string[] { "PlayerDB_DWC5", "LogDB_DWC5", "AccountDB5", "PaymentDB5", "GameDB5", "ConfigDB5" };

    // 传入ip地址 192.169.1.12
    public bool init<T>(string url, DbServerInfo dbInfo, CDBMgr<T> dbMgr, bool special = false) where T : CDBMgr<T>, new()
    {
        m_client = new CMongodbHelper[DbName.DB_NAME_MAX];
        m_clientSlave = new CMongodbHelper[DbName.DB_NAME_MAX];

        // 是账号数据库，这里只创建一个，即可
        if (dbMgr.isAccountDb(url))
        {
            string tmpURL = dbMgr.getAccountDbURL();
            m_client[DbName.DB_ACCOUNT] = new CMongodbHelper();
            m_client[DbName.DB_ACCOUNT].MongoDb = create(m_dbName[DbName.DB_ACCOUNT], tmpURL);

            // 充值数据库只连接这个
            m_client[DbName.DB_PAYMENT] = new CMongodbHelper();
            m_client[DbName.DB_PAYMENT].MongoDb = create(m_dbName[DbName.DB_PAYMENT], tmpURL);

            m_client[DbName.DB_CONFIG] = new CMongodbHelper();
            m_client[DbName.DB_CONFIG].MongoDb = create(m_dbName[DbName.DB_CONFIG], tmpURL);
        }
        else // 其他游戏相关数据库
        {
            createPLDB(m_client, url, dbInfo.m_logDbIp);
            createPLDB(m_clientSlave, dbInfo.m_playerDbSlave, dbInfo.m_logDbSlave);
        }

        bool res = testDBConnect();
        if (res)
        {
            IndexMgr.getInstance().createIndex(this, url);
        }
        return res;
    }

    public CMongodbHelper getDB(int index, int serverType = DbInfoParam.SERVER_TYPE_MASTER)
    {
        if (index < 0 || index >= DbName.DB_NAME_MAX)
        {
            LOGW.Info("获取某数据库服务器上某个db时出错，索引超出范围, index = {0}", index);
            return null;
        }
        if (serverType == DbInfoParam.SERVER_TYPE_MASTER)
        {
            return m_client[index];
        }

        return m_clientSlave[index];
    }

    void createPLDB(CMongodbHelper[] client, string playerURL, string logURL)
    {
        client[DbName.DB_PLAYER] = new CMongodbHelper();
        client[DbName.DB_PLAYER].MongoDb = create(m_dbName[DbName.DB_PLAYER], playerURL);

        client[DbName.DB_PUMP] = new CMongodbHelper();
        client[DbName.DB_PUMP].MongoDb = create(m_dbName[DbName.DB_PUMP], logURL);

        client[DbName.DB_GAME] = new CMongodbHelper();
        client[DbName.DB_GAME].MongoDb = create(m_dbName[DbName.DB_GAME], playerURL);
    }

    // pools是一个db服务器地址
    private MongoDatabase create(string dbname, string pools)
    {
        try
        {
            var connectionString = "mongodb://" + pools;
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            return server.GetDatabase(dbname);
        }
        catch (System.Exception ex)
        {
            LOGW.Info("连接数据库 {0} 时发生异常, {1}", pools, ex.Message);
        }
        return null;
    }

    // 测试是否可以连接数据库
    private bool testDBConnect()
    {
        bool res = true;
        try
        {
            m_client[0].MongoDb.CollectionExists("account");
        }
        catch (System.Exception ex)
        {
            // 出现这个异常说明没有连上
            if (ex.Message.IndexOf("Unable to connect to server") >= 0)
            {
                res = false;
            }
        }
        return res;
    }
}

//////////////////////////////////////////////////////////////////////////
class IndexMgr
{
    private static IndexMgr s_obj = null;
    private Dictionary<string, IndexBase> m_index = new Dictionary<string, IndexBase>();

    public static IndexMgr getInstance()
    {
        if (s_obj == null)
        {
            s_obj = new IndexMgr();
            s_obj.init();
        }
        return s_obj;
    }

    public void createIndex(CDBServer dbSvr, string url)
    {
        string name = "";
        for (int i = 0; i < DbName.DB_NAME_MAX; i++)
        {
            CMongodbHelper m = dbSvr.getDB(i);
            if (m != null)
            {
                name = CDBServer.m_dbName[i];
                if (m_index.ContainsKey(name))
                {
                    m_index[name].createIndex(m, url);
                }
            }
        }
    }

    private void init()
    {
        m_index.Add("PlayerDB_DWC5", new IndexPlayerDb());
        m_index.Add("LogDB_DWC5", new IndexPump());
        m_index.Add("AccountDB5", new IndexAccount());
    }
}

abstract class IndexBase
{
    public abstract void createIndex(CMongodbHelper db, string url);
}

class IndexPump : IndexBase
{
    public override void createIndex(CMongodbHelper db, string url)
    {
        MongoDatabase mdb = db.MongoDb;
        if (mdb == null)
            return;

        try
        {
            var table = mdb.GetCollection(TableName.PUMP_PLAYER_MONEY);
            table.CreateIndex(IndexKeys.Ascending("playerId"), IndexOptions.SetBackground(true));
            table.CreateIndex(IndexKeys.Ascending("gameId"), IndexOptions.SetBackground(true));
            table.CreateIndex(IndexKeys.Ascending("itemId"), IndexOptions.SetBackground(true));
            table.CreateIndex(IndexKeys.Ascending("reason"), IndexOptions.SetBackground(true));
        }
        catch (System.Exception ex)
        {	
        }
    }
}

// 玩家数据库
class IndexPlayerDb : IndexBase
{
    public override void createIndex(CMongodbHelper db, string url)
    {
     /*   MongoDatabase mdb = db.MongoDb;
        if (mdb == null)
            return;

        if (url == WebConfigurationManager.AppSettings["account"])
            return;

        var table = mdb.GetCollection(TableName.PLAYER_INFO);
        table.CreateIndex(IndexKeys.Ascending("gold"), IndexOptions.SetBackground(true));
        table.CreateIndex(IndexKeys.Ascending("ticket"), IndexOptions.SetBackground(true));
      */
    }
}

class IndexPayment : IndexBase
{
    public override void createIndex(CMongodbHelper db, string url)
    {
        MongoDatabase mdb = db.MongoDb;
        if (mdb == null)
            return;

        try
        {
        }
        catch (System.Exception ex)
        {	
        }
    }
}

class IndexAccount : IndexBase
{
    public override void createIndex(CMongodbHelper db, string url)
    {
        MongoDatabase mdb = db.MongoDb;
        if (mdb == null)
            return;

        var table = mdb.GetCollection(TableName.OPLOG);
        table.CreateIndex(IndexKeys.Ascending("OpType"), IndexOptions.SetBackground(true));
    }
}
































