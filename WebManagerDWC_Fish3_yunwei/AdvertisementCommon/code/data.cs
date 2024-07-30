using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Security.Cryptography;
using System.Text;

public class AdvertisementXgt 
{
    string[] s_fieldName = { "muid" };
    Dictionary<string, object> dataInsert = new Dictionary<string, object>();

    public int Load(string muid, string active_cb, string reg_cb) 
    {
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetOneBykey(TableName.ADVERTISEMENT_XGT, "muid", muid, s_fieldName);
        if(data == null) //不存在
        {
            dataInsert.Clear();
            dataInsert.Add("muid",muid);
            dataInsert.Add("active_cb",active_cb);
            dataInsert.Add("reg_cb",reg_cb);
            //写入新的
            bool res = MongodbAccount.Instance.ExecuteInsert(TableName.ADVERTISEMENT_XGT, dataInsert);
            return res ? RetResult.RET_SUCCESS : RetResult.RET_FAIL;
        }
        else //存在 返回
        {
            return RetResult.RET_HAS_EXIST;
        }
    }
}