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

public class DBMgr
{
    private static DBMgr s_db = null;
    // ip���±�Ķ�Ӧ
    private Dictionary<string, int> m_dbServer = new Dictionary<string, int>();
    private DBServer[] m_mongoServer = null;

    public static DBMgr getInstance()
    {
        if (s_db == null)
        {
            s_db = new DBMgr();
            s_db.open();
        }
        return s_db;
    }

    // ��ʼ��
    private void open()
    {
        Dictionary<string, DbServerInfo> allDb = ResMgr.getInstance().getAllDb();
        int count = allDb.Count;
        m_mongoServer = new DBServer[count + 2];

        int i = 0;
        foreach (var info in allDb.Values)
        {
            if (!m_dbServer.ContainsKey(info.m_serverIp))
            {
                m_dbServer.Add(info.m_serverIp, i);
                m_mongoServer[i] = null;
                i++;
            }
        }

        XmlConfig xml = ResMgr.getInstance().getRes("dbserver.xml");
        string acc = xml.getString("account", "");
        string pay = xml.getString("payment", "");
        if (!m_dbServer.ContainsKey(acc))
        {
            m_dbServer.Add(acc, i);
            m_mongoServer[i] = null;
            i++;
        }
        if (!m_dbServer.ContainsKey(pay))
        {
            m_dbServer.Add(pay, i);
            m_mongoServer[i] = null;
            i++;
        }
    }

    private DBServer create(string url, DbServerInfo dbInfo, bool special = false)
    {
        try
        {
            DBServer tmp = new DBServer();
            bool res = tmp.init(url, dbInfo, special);
            if (!res)
                return null;
            return tmp;
        }
        catch (System.Exception ex)
        {
            LOG.Info("�������ݿ� {0} ʱ�����쳣!", url);
        }
        return null;
    }

    // ���ݵ�ַ���ƣ�ȡ���±�
    public int getDbId(string pools, bool special = false)
    {
        if (m_dbServer.ContainsKey(pools))
        {
            DbServerInfo dbInfo = ResMgr.getInstance().getDbInfo(pools);

            int index = m_dbServer[pools];
            if (m_mongoServer[index] == null)
            {
                m_mongoServer[index] = create(pools, dbInfo, special);
                if (m_mongoServer[index] == null)
                {
                    LOG.Info("getDbId�������޷��������ݿ�!");
                    return -1;
                }
            }
            return index;
        }
        LOG.Info("û���ҵ���ַ:{0}��������ԭ��ַ", pools);
        return -1;
    }

    // ���Ƿ����
    public bool existTable(string tablename, int serverid, int dbid)
    {
        return m_mongoServer[serverid].getDB(dbid).TableExists(tablename);
    }

    // ��ձ��е���������
    public bool clearTable(string tablename, int serverid, int dbid)
    {
        if (!existTable(tablename, serverid, dbid))
            return true;

        return m_mongoServer[serverid].getDB(dbid).ExecuteRemoveAll(tablename);
    }

    // ���ݲ�ѯ��������table�ڲ�ѯ���
    public List<Dictionary<string, object>> executeQuery(string tablename,                 // ����
                                                         int serverid, 
                                                         int dbname,
                                                         IMongoQuery query = null,         // ��ѯ�������ⲿҪ��ƴ��
                                                         int skip = 0,
                                                         int limt = 0,
                                                         string[] fields = null,
                                                         string sort = "",
                                                         bool asc = true
                                                         )
    {
        return m_mongoServer[serverid].getDB(dbname).ExecuteQuery(tablename, query, fields, sort, asc, skip, limt);
    }

    public List<Dictionary<string, object>> executeQuery2(string tablename,                 // ����
                                                         int serverid,
                                                         int dbname,
                                                         IMongoQuery query = null,         // ��ѯ�������ⲿҪ��ƴ��
                                                         int skip = 0,
                                                         int limt = 0,
                                                         string[] fields = null,
                                                         SortByBuilder sort = null
                                                         )
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbname).ExecuteQuery(tablename, query, fields, skip, limt, sort);
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    public List<BsonDocument> executeQueryBsonDoc(string tablename,                 // ����
                                                   int serverid,
                                                   int dbname,
                                                   IMongoQuery query = null,         // ��ѯ�������ⲿҪ��ƴ��
                                                   int skip = 0,
                                                   int limt = 0,
                                                   string[] fields = null,
                                                   string sort = "",
                                                   bool asc = true,
                                                   string[] indexes = null          // ����
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

    // �ӱ�tablename�л�ȡһ�������б�
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
        return m_mongoServer[serverid].getDB(dbname).ExecuteGetListBykey(tablename, key, val, fields, sort, asc, skip, limt);
    }

    // ȡ�ñ�tablename�У� �ֶ���fieldname Ϊvalue��
    public Dictionary<string, object> getTableData(string tablename, string fieldname, object val, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteGetOneBykey(tablename, fieldname, val);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.Message);
            LOG.Info(ex.StackTrace);
        }
        return null;
    }

    // ȡ�ñ�tablename�У� �ֶ���fieldname Ϊvalue��
    public Dictionary<string, object> getTableData(string tablename, string fieldname, object val, string[] fields, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteGetOneBykey(tablename, fieldname, val, fields);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.Message);
            LOG.Info(ex.StackTrace);
        }
        return null;
    }

    // ȡ�ñ�tablename�У� �ֶ���fieldname Ϊvalue��
    public Dictionary<string, object> getTableData(string tablename, int serverid, int dbid, IMongoQuery query, string[] fields = null)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteGetByQuery(tablename, query, fields);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.Message);
            LOG.Info(ex.StackTrace);
        }
        return null;
    }

    // ���ر�tablename�����������ļ�¼����
    public long getRecordCount(string tablename, IMongoQuery query, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteGetCount(tablename, query);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.ToString());
        }
        return 0;
    }

    // ���ر�tablename�����������ļ�¼����
    public long getRecordCount(string tablename, string fieldname, object val, int serverid, int dbid)
    {
        IMongoQuery imq = Query.EQ(fieldname, BsonValue.Create(val));
        return getRecordCount(tablename, imq, serverid, dbid);
    }

    // �����ݱ��浽tablename��
    public bool save(string tablename, Dictionary<string, object> data, string filedname, object val, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            Dictionary<string, object> tmp = m_mongoServer[serverid].getDB(dbid).ExecuteGetBykey(tablename, filedname, val);
            // û�����ݣ��²���һ��
            if (tmp == null)
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteInsert(tablename, data);
            }
            else // �У����滻
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteStoreBykey(tablename, filedname, val, data);
            }
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.Message);
            LOG.Info(ex.StackTrace);
        }
        return false;
    }

    // �����ݱ��浽tablename��
    public bool save(string tablename, Dictionary<string, object> data, IMongoQuery query, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            Dictionary<string, object> tmp = m_mongoServer[serverid].getDB(dbid).ExecuteGetByQuery(tablename, query);
            // û�����ݣ��²���һ��
            if (tmp == null)
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteInsert(tablename, data);
            }
            else // �У����滻
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteStoreByQuery(tablename, query, data);
            }
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.ToString());
        }
        return false;
    }

    // �������ݱ���ĳЩ�ֶ�ֵ
    public bool update(string tablename, Dictionary<string, object> data, IMongoQuery query, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteUpdateByQuery(tablename, query, data, UpdateFlags.None);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.ToString());
        }
        return false;
    }

    public bool updateAll(string tablename, Dictionary<string, object> data, IMongoQuery query, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteUpdateByQuery(tablename, query, data, UpdateFlags.Multi);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.ToString());
        }
        return false;
    }

    // �������ݱ���ĳЩ�ֶ�ֵ
    public bool update(string tablename, Dictionary<string, object> data, string filedname, object val, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteUpdate(tablename, filedname, val, data, UpdateFlags.None);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.ToString());
        }
        return false;
    }

    // ���tablename���������ݣ���ָ���ؼ��ֵ������Ѵ��ڣ������
    public bool addTableData(string tablename, Dictionary<string, object> data, string filedname, object val, int serverid, int dbid)
    {
        if (data == null)
            return false;
        try
        {
            Dictionary<string, object> tmp = m_mongoServer[serverid].getDB(dbid).ExecuteGetBykey(tablename, filedname, val);
            // û�����ݣ��²���һ��
            if (tmp == null)
            {
                return m_mongoServer[serverid].getDB(dbid).ExecuteInsert(tablename, data);
            }
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.Message);
            LOG.Info(ex.StackTrace);
        }
        return false;
    }

    // �����ݿ��Ƴ���¼
    public bool remove(string tablename, string filedname, object val, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteRemoveBykey(tablename, filedname, val);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.ToString());
        }
        return false;
    }

    // ����һ����¼
    public bool insertData(string table, Dictionary<string, object> data, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).ExecuteInsert(table, data);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.ToString());
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
            LOG.Info(ex.ToString());
        }
        return false;
    }

    // ִ��map-reduce
    public MapReduceResult executeMapReduce(string tablename, int serverid, int dbname, IMongoQuery query,
                                                             string map_js, string reduce_js,
                                                             string outTableName = "")
    {
        return m_mongoServer[serverid].getDB(dbname).executeMapReduce(tablename, query, map_js, reduce_js, outTableName);
    }

    public bool keyExists(string tablename, string fieldname, object val, int serverid, int dbid)
    {
        try
        {
            return m_mongoServer[serverid].getDB(dbid).KeyExistsBykey(tablename, fieldname, val);
        }
        catch (System.Exception ex)
        {
            LOG.Info(ex.ToString());
        }
        return false;
    }
}

public class MongodbHelper
{
    private MongoDatabase mMongodbClient = null;

    public MongodbHelper()
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
            if (!mMongodbClient.CollectionExists(tablename))
            {
                CommandResult cr = mMongodbClient.CreateCollection(tablename);
                tmp = mMongodbClient.GetCollection(tablename);
            }
            else
            {
                tmp = mMongodbClient.GetCollection(tablename);
            }

            if (indexname != "" && !tmp.IndexExists(indexname))
                tmp.CreateIndex(indexname);

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

    // ����tablename�Ƿ����
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
            LOG.Info(ex.Message);
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
            LOG.Info(ex.ToString());
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

    // �ӱ�tableɾ��ĳ�����ݣ������ǣ��ֶ��� key = val
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
            LOG.Info(ex.ToString());
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

    // ���table������һ������
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

    public bool ExecuteUpdate(string table, string fieldname, object val, Dictionary<string, object> data, UpdateFlags flag)
    {
        return ExecuteUpdateByQuery(table, Query.EQ(fieldname, BsonValue.Create(val)), data, flag);
    }

    public bool ExecuteUpdateByQuery(string table, IMongoQuery queries, Dictionary<string, object> data, UpdateFlags flag)
    {
        try
        {
            var cb = check_table(table);

            UpdateBuilder ub = new UpdateBuilder();
            foreach (var item in data)
            {
                ub = ub.Set(item.Key, BsonValue.Create(item.Value));
            }

            var retu = cb.Update(queries, ub, flag);
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

    // ���ݲ�ѯ��������table�ڲ�ѯ���
    public List<Dictionary<string, object>> ExecuteQuery(string table,                 // ����
                                                         IMongoQuery query,            // ��ѯ�������ⲿҪ��ƴ��
                                                         string[] fields = null,
                                                         string sort = "",
                                                         bool asc = true,
                                                         int skip = 0,
                                                         int limt = 0)
    {
        // û����������ʱ������ȫ������
        if (query == null)
            return ExecuteGetAll(table, fields, sort, asc, skip, limt);

        // �����ڣ�ֱ�ӷ���
        if (!TableExists(table))
            return null;

        List<Dictionary<string, object>> retlist = new List<Dictionary<string, object>>();
        try
        {
            var cb = check_table(table);
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

    public List<Dictionary<string, object>> ExecuteQuery(string table,                 // ����
                                                        IMongoQuery query,            // ��ѯ�������ⲿҪ��ƴ��
                                                        string[] fields = null,
                                                        int skip = 0,
                                                        int limt = 0,
                                                        SortByBuilder sort = null)
    {
        // û����������ʱ������ȫ������
        if (query == null)
            return ExecuteGetAll(table, fields, skip, limt, sort);

        // �����ڣ�ֱ�ӷ���
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
            LOG.Info(ex.ToString());
        }
        return retval;
    }

    public List<BsonDocument> ExecuteQueryBsonDoc(string table,                 // ����
                                                    IMongoQuery query,            // ��ѯ�������ⲿҪ��ƴ��
                                                    string[] fields = null,
                                                    string sort = "",
                                                    bool asc = true,
                                                    int skip = 0,
                                                    int limt = 0,
                                                    string[] indexes = null)
    {
        // �����ڣ�ֱ�ӷ���
        if (!TableExists(table))
            return null;

        List<BsonDocument> retlist = new List<BsonDocument>();
        try
        {
            var cb = check_table(table);
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

    // ��ձ��ڵ���������
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

    // ���ر�table�У������ѯ�����ļ�¼����
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
            LOG.Info(ex.ToString());
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
            if (ret.Ok)
                return ret;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }
}

// һ��server�ϵ�db�б�
/*public class DbName
{
    // �����Ϣdb
    public const int DB_PLAYER = 0;

    // ͳ����ص����ݿ�
    public const int DB_PUMP = 1;

    // ����˺ſ�
    public const int DB_ACCOUNT = 2;

    // ��ֵ��¼���ݿ�
    public const int DB_PAYMENT = 3;

    // ������Ϸ������ݿ�
    public const int DB_GAME = 4;

    public const int DB_NAME_MAX = 5;
}*/

// һ��db������
public class DBServer
{
    private MongodbHelper[] m_client = null;
    public static string[] m_dbName = new string[] { "PlayerDB_DWC5", "LogDB_DWC5", "AccountDB5", "PaymentDB5", "GameDB5" };

    // ����ip��ַ 192.169.1.12
    public bool init(string url, DbServerInfo dbInfo, bool special = false)
    {
        m_client = new MongodbHelper[DbName.DB_NAME_MAX];
        XmlConfig xml = ResMgr.getInstance().getRes("dbserver.xml");
        string acc = xml.getString("account", "");
        string pay = xml.getString("payment", "");
        if (url == acc)
        {
            // ���˺����ݿ⣬����ֻ����һ��������
            m_client[DbName.DB_ACCOUNT] = new MongodbHelper();
            m_client[DbName.DB_ACCOUNT].MongoDb = create(m_dbName[DbName.DB_ACCOUNT], url);

            if (url == pay)
            {
                // ��ֵ���ݿ�ֻ�������
                m_client[DbName.DB_PAYMENT] = new MongodbHelper();
                m_client[DbName.DB_PAYMENT].MongoDb = create(m_dbName[DbName.DB_PAYMENT], url);
            }

            createOther(url, dbInfo);
        }
        else if (url == pay)
        {
            // ��ֵ���ݿ�ֻ�������
            m_client[DbName.DB_PAYMENT] = new MongodbHelper();
            m_client[DbName.DB_PAYMENT].MongoDb = create(m_dbName[DbName.DB_PAYMENT], url);
        }
        else // ������Ϸ������ݿ�
        {
            createOther(url, dbInfo);
        }
        /*for (int i = 0; i < DbName.DB_NAME_MAX; i++)
        {            
            m_client[i] = new MongodbHelper();
            m_client[i].MongoDb = create(m_dbName[i], url);
        }*/
        bool res = testDBConnect();
        if (res)
        {
            IndexMgr.getInstance().createIndex(this, url);
        }
        return res;
    }

    void createOther(string url, DbServerInfo dbInfo)
    {
        m_client[DbName.DB_PLAYER] = new MongodbHelper();
        m_client[DbName.DB_PLAYER].MongoDb = create(m_dbName[DbName.DB_PLAYER], url);

        m_client[DbName.DB_PUMP] = new MongodbHelper();
        m_client[DbName.DB_PUMP].MongoDb = create(m_dbName[DbName.DB_PUMP], dbInfo.m_logDbIp);

        m_client[DbName.DB_GAME] = new MongodbHelper();
        m_client[DbName.DB_GAME].MongoDb = create(m_dbName[DbName.DB_GAME], url);
    }

    public MongodbHelper getDB(int index)
    {
        if (index < 0 || index >= DbName.DB_NAME_MAX)
        {
            LOG.Info("��ȡĳ���ݿ��������ĳ��dbʱ��������������Χ, index = {0}", index);
            return null;
        }
        return m_client[index];
    }

    // pools��һ��db��������ַ
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
            LOG.Info("�������ݿ� {0} ʱ�����쳣, {1}", pools, ex.Message);
        }
        return null;
    }

    // �����Ƿ�����������ݿ�
    private bool testDBConnect()
    {
        bool res = true;
        try
        {
            m_client[0].MongoDb.CollectionExists("account");
        }
        catch (System.Exception ex)
        {
            // ��������쳣˵��û������
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

    public void createIndex(DBServer dbSvr, string url)
    {
        string name = "";
        for (int i = 0; i < DbName.DB_NAME_MAX; i++)
        {
            MongodbHelper m = dbSvr.getDB(i);
            if (m != null)
            {
                name = DBServer.m_dbName[i];
                if (m_index.ContainsKey(name))
                {
                    m_index[name].createIndex(m, url);
                }
            }
        }
    }

    private void init()
    {
        //m_index.Add("PlayerDB_DWC", new IndexPlayerDb());
        //m_index.Add("LogDB_DWC", new IndexPump());
        m_index.Add("AccountDB", new IndexAccount());
    }
}

abstract class IndexBase
{
    public abstract void createIndex(MongodbHelper db, string url);
}

class IndexPump : IndexBase
{
    public override void createIndex(MongodbHelper db, string url)
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

// ������ݿ�
class IndexPlayerDb : IndexBase
{
    public override void createIndex(MongodbHelper db, string url)
    {
        MongoDatabase mdb = db.MongoDb;
        if (mdb == null)
            return;

        return;

        var table = mdb.GetCollection(TableName.PLAYER_INFO);
        table.CreateIndex(IndexKeys.Ascending("gold"), IndexOptions.SetBackground(true));
        table.CreateIndex(IndexKeys.Ascending("ticket"), IndexOptions.SetBackground(true));
    }
}

class IndexPayment : IndexBase
{
    public override void createIndex(MongodbHelper db, string url)
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
    public override void createIndex(MongodbHelper db, string url)
    {
        MongoDatabase mdb = db.MongoDb;
        if (mdb == null)
            return;

        var table = mdb.GetCollection(TableName.GIFT_CODE);
        table.CreateIndex(IndexKeys.Ascending("cdkey"), IndexOptions.SetBackground(true));
    }
}






































