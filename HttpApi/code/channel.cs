using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Common;

struct Const
{
    public const int CODE_SUCCESS = 0;

    public const int CODE_PARAM_ERROR = 1;

    public const int CODE_NOT_FIND = 2;

    public const string TABLE_CHANNEL_DATA = "channelTalkingData";
}

// 查询channel的留存数据
public class QueryChannel
{
    public string ChannelId { set; get; }

    public string TimeStr { set; get; }

    static string[] s_fields = { "regeditCount", "activeCount", "rechargePersonNum", "totalIncome" };

    public string queryData()
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        do
        {
            DateTime time = new DateTime();
            if (string.IsNullOrEmpty(TimeStr))
            {
                ret.Add("code", Const.CODE_PARAM_ERROR);
                break;
            }
            bool res = ApiQueryData.paraseTime(TimeStr, false, ref time);
            if (!res)
            {
                ret.Add("code", Const.CODE_PARAM_ERROR);
                break;
            }

            Dictionary<string, object> data = getData(time);
            if (data == null)
            {
                ret.Add("code", Const.CODE_NOT_FIND);
                break;
            }

            ret.Add("code", Const.CODE_SUCCESS);
            foreach (var e in data)
            {
                ret.Add(e.Key, e.Value);
            }

        } while (false);

        return JsonHelper.ConvertToStr(ret);
    }

    Dictionary<string, object> getData(DateTime time)
    {
        IMongoQuery imq1 = Query.EQ("channel", ChannelId);
        IMongoQuery imq2 = Query.EQ("genTime", time);
        IMongoQuery imq = Query.And(imq1, imq2);
        Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetByQuery(Const.TABLE_CHANNEL_DATA, imq, s_fields);
        return data;
    }
}

public class CQueryChannelRegInfo
{
    // 渠道传来的id, 查看这个id有没有注册过
    public string Id { set; get; }

    public string AccTable { set; get; }

    public string ChannelId { set; get; }

    public string queryData()
    {
        string acc = ChannelId + "_" + Id;
        IMongoQuery imq = Query.EQ("acc", acc);
        Dictionary<string, object> ret = new Dictionary<string, object>();

        do
        {
            if (string.IsNullOrEmpty(Id))
            {
                ret.Add("code", 0);
                break;
            }
            Dictionary<string, object> data = MongodbAccount.Instance.ExecuteGetByQuery(AccTable, imq);
            if (data == null)
            {
                ret.Add("code", 0);
                break;
            }

            ret.Add("code", 1);

        } while (false);

        ret.Add("id", Id);

        return JsonHelper.ConvertToStr(ret);
    }
}

