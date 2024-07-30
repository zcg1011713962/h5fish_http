using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Web.Configuration;
using System.Text;

public enum DyOpType
{
    //实物审核管理
    opTypeOperationExchangeAudit,

    //机器人积分管理
    opTypeOperationFishlordRobotRankCFG,

    //游戏控制
    opTypeOPerationGameCtrl,

    //玩家手机验证
    opTypeServiceVertifyPhoneNo,

    //高级场控制管理
    opTypeFishlordAdvancedRoomCtrl,

    //系统空投
    opTypeStatAirdropSys,

    //围剿龙王自定义当天付费玩家最终排名
    opTypeWjlwDefRechargeReward,
    //围剿龙王金币玩法修改当前期望盈利率
    opTypeWjlwGoldEditCurrExpRate,

    //流失大户引导完成添加记录
    opTypeGuideLostPlayers,

    //客服补单/大户随访/换包福利-系统
    opTypeRepairOrder,

    //世界杯大竞猜赛事表
    opTypeWorldCupMatch,

    //绑定解绑玩家手机
    opTypeBindUnbindPhone,

	//添加同步道具失败设置是否已处理
	opTypeOperationIsDeal,

	//运营公告
	opTypeOperationNotify,

	//水果机参数调整 修改盈利率
	opTypeFruitParamAdjust,

	//游客快速开始添加账号
	opTypeFasterStartForVisitor,

	//修改机器人最高积分
	opTypeRobotMaxScoreEdit,

	//弹头统计区间修正系数修改
	opTypeFishlordBulletHeadRcParam,

	//弹头统计重置
	opTypeFishlordBulletHeadReset,
	//极光推送
	opTypePolarLightsPush,
	//水浒传点杀点送
	opTypeShuihzPlayerScorePool,
	//捕鱼点杀点送
	opTypePlayerScorePool,
	// 发邮件
	opTypeSendMail,

	// 修改密码
	opTypeModifyPwd,

	// 停封账号
	opTypeBlockAcc,

	// 停封玩家ID
	opTypeBlockId,

	// 停封IP
	opTypeBlockIP,

	// 后台充值
	opTypeRecharge,

	// 推送添加APP应用信息
	opTypePushApp,

	// 绑定手机
	opTypeBindPhone,

	// 礼包生成
	opTypeGift,

	// 礼包码生成
	opTypeGiftCode,

	//礼包码生成（新）
	opTypeGiftCodeNew,

	// 兑换
	opTypeExchange,

	// 通告
	opTypeNotify,

	// 运营维护
	opTypeMaintenance,

	// 捕鱼参数调整
	opTypeFishlordParamAdjust,
    //捕鱼参数调整（新）
    opTypeFishlordParamAdjustNew,

    //捕鱼个人后台参数调整
    opTypeFishlordSingleParamAdjustNew,

	// 鳄鱼公园参数调整
	opTypeFishParkParamAdjust,

	// 鳄鱼大亨参数调整
	opTypeCrocodileParamAdjust,

	//奔驰宝马参数调整
	opTypeBzParamAdjust,

	// 骰宝参数调整
	opTypeDiceParamAdjust,

	// 百家乐参数调整
	opTypeBaccaratParamAdjust,

	// 牛牛参数调整
	opTypeCowsParamAdjust,

	// 五龙参数调整
	opTypeDragonParamAdjust,

	// 清空鱼统计表
	opTypeClearFishTable,

	// 重新加载表格
	opTypeReLoadTable,

	// 客服信息
	opTypeServiceInfo,

	// 冻结头像
	opTypeFreezeHead,

	// 渠道编辑
	opTypeEditChannel,

	// 通告消息
	opTypeSpeaker,

	// 设置牛牛牌型
	opTypeSetCowsCard,
	// 游戏结果控制
	opTypeDyOpGameResult,

	// 祝福诅咒
	opTypeWishCurse,

	// 踢玩家
	opTypeKickPlayer,

	// 修改GM的后台密码
	opTypeModifyGmLoginPwd,

	// 删操作日志
	opTypeRemoveData,

	// 游戏参数调整
	opTypeGameParamAdjust,

	opTypeWeekChampionControl,

	opTypeFishBoss,

	opTypeGmTypeEdit,

	opTypeActivityPanicBuyingCfgParamAdjust,//限时活动参数

	opTypeShcdCardsSpecilList,//黑红梅方黑白名单设置

    opTypeStatWjlwDefRechargeReward,//围剿龙王

}

// 动态操作
class DyOpMgr : SysBase
{
	private Dictionary<DyOpType, DyOpBase> m_items = new Dictionary<DyOpType, DyOpBase>();

	public DyOpMgr()
	{
		m_sysType = SysType.sysTypeDyOp;
	}

	public OpRes doDyop(object param, DyOpType type, GMUser user)
	{
		if (!m_items.ContainsKey(type))
		{
		    LOGW.Info("DyOpMgr.doDyop不存在操作类型[{0}]", type);
		    return OpRes.op_res_failed;
		}
		return m_items[type].doDyop(param, user);
	}

	public DyOpBase getDyOp(DyOpType type)
	{
	    if (!m_items.ContainsKey(type))
	    {
	        LOGW.Info("DyOpMgr.getDyOp不存在操作类型[{0}]", type);
	        return null;
	    }
	    return m_items[type];
	}

	public object getResult(DyOpType type)
	{
	    if (!m_items.ContainsKey(type))
	    {
	        LOGW.Info("DyOpMgr.getDyOp不存在操作类型[{0}]", type);
	        return null;
	    }
	    return m_items[type].getResult();
	}

	public override void initSys()
	{
	    m_items.Add(DyOpType.opTypeSendMail, new DyOpSendMail());//发邮件
	    m_items.Add(DyOpType.opTypeModifyPwd, new DyOpModifyPwd());
	    m_items.Add(DyOpType.opTypeBlockAcc, new DyOpBlockAccount());
	    m_items.Add(DyOpType.opTypeBlockId, new DyOpBlockId()); //停封玩家ID
	    m_items.Add(DyOpType.opTypeBlockIP, new DyOpBlockIP());

	    m_items.Add(DyOpType.opTypeRecharge, new DyOpRecharge());
	    m_items.Add(DyOpType.opTypePushApp, new DyOpJPushAddApp());
	    m_items.Add(DyOpType.opTypeBindPhone, new DyOpBindPhone());
	    m_items.Add(DyOpType.opTypeGift, new DyOpGift());
	    m_items.Add(DyOpType.opTypeGiftCode, new DyOpGiftCode());

	    m_items.Add(DyOpType.opTypeExchange, new DyOpExchange());  //兑换激活
	    m_items.Add(DyOpType.opTypeNotify, new DyOpNotify());
	    m_items.Add(DyOpType.opTypeOperationNotify, new DyOpNotifyNew()); //运营公告
	    m_items.Add(DyOpType.opTypeMaintenance, new DyOpMaintenance());
	    m_items.Add(DyOpType.opTypeFishlordParamAdjust, new DyOpFishlordParamAdjust());
        m_items.Add(DyOpType.opTypeFishlordParamAdjustNew, new DyOpFishlordParamAdjustNew()); //经典捕鱼参数调整（新）
        m_items.Add(DyOpType.opTypeFishlordSingleParamAdjustNew, new DyOpFishlordSingleParamAdjustNew());//经典捕鱼个人后台管理参数调整

	    m_items.Add(DyOpType.opTypeFishParkParamAdjust, new DyOpFishParkParamAdjust());

	    m_items.Add(DyOpType.opTypeBzParamAdjust,new DyOpBzParamAdjust()); //奔驰宝马参数调整
	    m_items.Add(DyOpType.opTypeCrocodileParamAdjust, new DyOpCrocodileParamAdjust());

	    m_items.Add(DyOpType.opTypeClearFishTable, new DyOpClearFishTable());
	    m_items.Add(DyOpType.opTypeReLoadTable, new DyOpReLoadTable());
	    m_items.Add(DyOpType.opTypeDiceParamAdjust, new DyOpDiceParamAdjust());
	    m_items.Add(DyOpType.opTypeServiceInfo, new DyOpServiceInfo());
	    m_items.Add(DyOpType.opTypeFreezeHead, new DyOpFreezeHead());

	    m_items.Add(DyOpType.opTypeEditChannel, new DyOpAddChannel());
	    m_items.Add(DyOpType.opTypeBaccaratParamAdjust, new DyOpBaccaratParamAdjust());
	    m_items.Add(DyOpType.opTypeSpeaker, new DyOpSpeaker());
	    m_items.Add(DyOpType.opTypeCowsParamAdjust, new DyOpCowsParamAdjust());
	    m_items.Add(DyOpType.opTypeSetCowsCard, new DyOpAddCowsCardType());

	    m_items.Add(DyOpType.opTypeDyOpGameResult, new DyOpGameResult());

	    m_items.Add(DyOpType.opTypeDragonParamAdjust, new DyOpDragonParamAdjust());
	    m_items.Add(DyOpType.opTypeWishCurse, new DyOpAddWishCurse());
	    m_items.Add(DyOpType.opTypeKickPlayer, new DyOpKickPlayer());
	    m_items.Add(DyOpType.opTypeModifyGmLoginPwd, new DyOpModifyGmLoginPwd());
	    m_items.Add(DyOpType.opTypeRemoveData, new DyOpRemoveData());

	    m_items.Add(DyOpType.opTypeGameParamAdjust, new DyOpGameParamAdjust());  //黑红参数调整
	    m_items.Add(DyOpType.opTypeWeekChampionControl, new DyOpWeekChampionControl());
	    m_items.Add(DyOpType.opTypeFishBoss, new DyOpFishBoss());
	    m_items.Add(DyOpType.opTypeGmTypeEdit, new DyOpGmTypeEdit());   
	    m_items.Add(DyOpType.opTypeActivityPanicBuyingCfgParamAdjust,new DyOpActivityPanicBuyingCfgParamAdjust());//限时活动

	    m_items.Add(DyOpType.opTypePlayerScorePool, new DyOpAddScorePool());//捕鱼点杀点送
	    m_items.Add(DyOpType.opTypeShuihzPlayerScorePool,new DyOpShuihzAddScorePool());//水浒传点杀点送
	    m_items.Add(DyOpType.opTypePolarLightsPush,new DyOpPolarLightsPush());//极光推送
	    m_items.Add(DyOpType.opTypeFishlordBulletHeadReset,new DyOpFishlordBulletHeadReset()); //弹头统计重置
	    m_items.Add(DyOpType.opTypeFishlordBulletHeadRcParam,new DyOpFishlordBulletHeadRcParamEdit());//弹头统计区间修正系数修改

	    m_items.Add(DyOpType.opTypeRobotMaxScoreEdit,new DyOpRobotMaxScoreEdit());//修改机器人最高积分
	    m_items.Add(DyOpType.opTypeFasterStartForVisitor,new DyOpFasterStartForVisitor());//游客快速开始添加账号

	    m_items.Add(DyOpType.opTypeFruitParamAdjust, new DyOpFruitParamAdjust());//水果机参数调整修改盈利率

	    m_items.Add(DyOpType.opTypeOperationIsDeal,new DyOpWord2LogicItemErrorIsDeal());//添加同步道具失败设置是否已操作

	    m_items.Add(DyOpType.opTypeGiftCodeNew,new DyOpGiftCodeNew()); //可重复使用礼包码CDKEY生成器

        m_items.Add(DyOpType.opTypeBindUnbindPhone,new DyOpBindUnbindPhone());//绑定解绑玩家手机
        m_items.Add(DyOpType.opTypeServiceVertifyPhoneNo, new DyOpServiceVertifyPhoneNo());//验证手机

        m_items.Add(DyOpType.opTypeWorldCupMatch,new DyOpWorldCupMatch()); //世界杯大竞猜赛事表

        m_items.Add(DyOpType.opTypeRepairOrder,new DyOpRepairOrder());//客服补单/大户随访/换包福利
        m_items.Add(DyOpType.opTypeGuideLostPlayers,new DyOpGuideLostPlayers());//流失大户引导完成添加记录

        m_items.Add(DyOpType.opTypeWjlwDefRechargeReward, new DyOpWjlwDefRechargeReward()); //围剿龙王自定义当天付费玩家最终排名
        m_items.Add(DyOpType.opTypeStatWjlwDefRechargeReward, new DyOpWjlwDefRemove());//围剿龙王移除玩家
        m_items.Add(DyOpType.opTypeWjlwGoldEditCurrExpRate, new DyOpWjlwGoldEditCurrExpRate());//围剿龙王金币玩法修改期望盈利率

        m_items.Add(DyOpType.opTypeStatAirdropSys, new DyOpStatAirdropSysCtrl());//系统空投控制

        m_items.Add(DyOpType.opTypeFishlordAdvancedRoomCtrl, new DyOpFishlordAdvancedRoomCtrl());//高级场控制管理

        m_items.Add(DyOpType.opTypeOPerationGameCtrl, new DyOpOperationGameCtrl());//游戏控制
        m_items.Add(DyOpType.opTypeOperationFishlordRobotRankCFG, new DyOpOperationFishlordRobotRankCFG());//机器人积分管理

        m_items.Add(DyOpType.opTypeOperationExchangeAudit, new DyOpOperationExchangeAudit());//实物审核管理
	}
}

//////////////////////////////////////////////////////////////////////////

// GM的动态操作
public class DyOpBase
{
	public virtual OpRes doDyop(object param, GMUser user)
	{
	    return OpRes.op_res_failed;
	}

	public virtual object getResult() { return null; }

	public virtual object getResult(object param, GMUser user) { return null; }
}
//////////////////////////////////////////////////////////////////////////
//验证手机号码
public class DyOpServiceVertifyPhoneNo : DyOpBase
{
    private static string[] s_retFields = { "account" };
    private string regex_phone = "^((13[0-9])|(14[5|7])|(17[0-9])|(15([0-3]|[5-9]))|(18[0,5-9]))\\d{8}$";
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;

        if (p.m_type == 3)//移除
        {
            if (string.IsNullOrEmpty(p.m_param))
                return OpRes.op_res_failed;

            bool res1 = DBMgr.getInstance().remove(
                TableName.PLAYER_PHONE_CODE, "_id", new ObjectId(p.m_param), user.getDbServerID(), DbName.DB_PLAYER);

            return res1 ? OpRes.opres_success : OpRes.op_res_failed;
        }

        //添加
        OpRes res = OpRes.op_res_failed;
        Dictionary<string, object> data = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(p.m_playerId))
        {
            int playerId = 0;
            string acc = "";
            res = getPlayerId(p, ref playerId, ref acc, user);
            if (res != OpRes.opres_success)
                return OpRes.op_res_player_not_exist;

            data.Add("playerId", playerId);
        }

        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_param_not_valid;

        //Match match = Regex.Match(p.m_param, regex_phone);
        // if (!match.Success)
        //   return OpRes.op_res_param_not_valid;

        Random rand = new Random();
        string code = rand.Next(100000, 999999).ToString();
        res = sendMsgCode(p.m_param, code, 2);
        if (res == OpRes.opres_success)
        {  //写入数据表
            data.Add("genTime", DateTime.Now);
            data.Add("phoneNo", p.m_param);
            data.Add("code", Convert.ToInt32(code));

            bool res1 = DBMgr.getInstance().insertData(TableName.PLAYER_PHONE_CODE, data, user.getDbServerID(), DbName.DB_PLAYER);
            return res1 ? OpRes.opres_success : OpRes.op_res_failed;
        }
        return OpRes.op_res_failed;
    }

    OpRes getPlayerId(ParamQuery p, ref int playerId, ref string acc, GMUser user)
    {
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = QueryBase.getPlayerProperty(playerId, user, s_retFields);
        if (data == null)
            return OpRes.op_res_player_not_exist;

        if (!data.ContainsKey("account"))
            return OpRes.op_res_player_not_exist;

        acc = Convert.ToString(data["account"]);
        if (string.IsNullOrEmpty(acc))
            return OpRes.op_res_player_not_exist;

        return OpRes.opres_success;
    }

    OpRes sendMsgCode(string phoneNo, string code, int type)
    {
        string fmt = WebConfigurationManager.AppSettings["sendMsgCode"];
        string aspx = string.Format(fmt, phoneNo, code, type);
        try
        {
            var ret = HttpPost.Post(new Uri(aspx), null);
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr == "0")
                {
                    return OpRes.opres_success;
                }
            }
        }
        catch (System.Exception ex)
        {
        }

        return OpRes.op_res_failed;
    }
}
//////////////////////////////////////////////////////////////////////////
//绑定解绑手机号码
public class DyOpBindUnbindPhone : DyOpBase 
{
    private static string[] s_retFields = { "account" };
    private string regex_phone = "^((13[0-9])|(14[5|7])|(17[0-9])|(15([0-3]|[5-9]))|(18[0,5-9]))\\d{8}$";
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        OpRes res = OpRes.op_res_failed;
        int playerId = 0;
        string acc = "";

        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;

        res = getPlayerId(p, ref playerId, ref acc, user);
        if(res==OpRes.opres_success)
        {
            string bindPhone = "";
            if (p.m_op == 0)//绑定手机
            {
                if (string.IsNullOrEmpty(p.m_param))
                    return OpRes.op_res_param_not_valid;
                //判定电话号码
               // Match match = Regex.Match(p.m_param,regex_phone);
               // if (!match.Success)
               //     return OpRes.op_res_param_not_valid;

                bindPhone = p.m_param;

                bool r = hasBind(bindPhone, user);
                if (r)
                    return OpRes.op_res_has_bind;
            }
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("bindPhone",bindPhone);
            bool code = DBMgr.getInstance().update(TableName.PLAYER_INFO,data,"player_id", playerId,user.getDbServerID(),DbName.DB_PLAYER);
            if (code)
            {
                string op = "bind";
                if (p.m_op == 1)
                    op = "unbind";
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BIND_PLAYER_PHONE, new LogBindPlayerPhone(playerId, op, bindPhone), user);
            }else 
            {
                res = OpRes.op_res_failed;
            }
        }
        return res;
    }

    bool hasBind(string phone, GMUser user)
    {
        Dictionary<string, object> data =
            DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "bindPhone", phone, new string[] { "player_id" },
            user.getDbServerID(), DbName.DB_PLAYER);

        if (data != null)
            return true;

        return false;
    }

    OpRes getPlayerId(ParamQuery p, ref int playerId, ref string acc, GMUser user)
    {
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = QueryBase.getPlayerProperty(playerId, user, s_retFields);
        if (data == null)
            return OpRes.op_res_player_not_exist;

        if (!data.ContainsKey("account"))
            return OpRes.op_res_player_not_exist;

        acc = Convert.ToString(data["account"]);
        if (string.IsNullOrEmpty(acc))
            return OpRes.op_res_player_not_exist;

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//可重复使用礼包码CDKEY生成器
public class DyOpGiftCodeNew : DyOpBase 
{
    public override OpRes doDyop(object param, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        ParamCDKEY p = (ParamCDKEY)param;
        bool res = false;

        if(p.m_op==0) //删除
        {
            if (string.IsNullOrEmpty(p.m_giftId))
                return OpRes.op_res_failed;

            res = DBMgr.getInstance().remove(TableName.CD_KEY_MULT, "_id",new ObjectId(p.m_giftId), serverId, DbName.DB_ACCOUNT);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
        ////////////////点击生成///////////////
        //失效时间
        if (string.IsNullOrEmpty(p.m_deadTime))
            return OpRes.op_res_time_format_error;
        DateTime setTime = DateTime.MinValue;
        if (!Tool.splitTimeStr(p.m_deadTime, ref setTime, 1))
            return OpRes.op_res_time_format_error;
        if(setTime < DateTime.Now) //大于当前时间
            return OpRes.op_res_param_not_valid;

        //礼包码
        if (p.m_pici.Length != 7 && p.m_pici.Length != 9)
            return OpRes.op_res_param_not_valid;
        res = DBMgr.getInstance().keyExists(TableName.CD_KEY_MULT, "keyCode", p.m_pici, serverId, DbName.DB_ACCOUNT);
        if (res)
            return OpRes.op_res_has_exist;


        //最大可用次数 >0
        if (string.IsNullOrEmpty(p.m_count))
            return OpRes.op_res_param_not_valid;
        int count = 0;
        if (!int.TryParse(p.m_count, out count))
            return OpRes.op_res_param_not_valid;
        if (count <= 0)
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("genTime",DateTime.Now);
        data.Add("deadTime",setTime);
        data.Add("keyType",p.m_type);
        data.Add("maxUseCount",count);
        data.Add("giftId",Convert.ToInt32(p.m_giftId));
        data.Add("keyCode",p.m_pici);
        if(!string.IsNullOrEmpty(p.m_comment))
        {
            data.Add("comment", p.m_comment);
        }
        res = DBMgr.getInstance().insertData(TableName.CD_KEY_MULT, data, serverId, DbName.DB_ACCOUNT);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
//////////////////////////////////////////////////////////////////////////
//添加同步道具失败设置是否已操作
public class DyOpWord2LogicItemErrorIsDeal : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        bool res = false;

        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_failed;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("isDeal", true);
        res = DBMgr.getInstance().update(TableName.PUMP_WORD2_LOGIC_ITEM_ERROR, data,
            "_id", ObjectId.Parse(p.m_param), user.getDbServerID(), DbName.DB_PUMP);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
////////////////////////////////////////////////////////////////////////////////
//世界杯大竞猜赛事表
public class WorldCupMatchParam 
{
    public int m_op; //0新增 1编辑 2删除
    public string m_id;
    public string m_matchId;
    public string m_matchStartTime;
    public string m_betEndTime;
    public string m_showTime;
    public int m_matchNameId;
    public string m_matchName;
    public int m_matchType;
    public int m_homeTeamId;
    public int m_visitTeamId;
    public string m_homeTeam;
    public string m_visitTeam;
    public int m_homeScore;
    public int m_visitScore;
    public int m_betMaxCount;
}
public class DyOpWorldCupMatch : DyOpBase 
{
    public static string[] fields = new string[] { "_id" };
    public override OpRes doDyop(object param,GMUser user) 
    {
        WorldCupMatchParam p = (WorldCupMatchParam)param;
        bool res = false;
        if (p.m_op == 2) //删除
        {
            res = DBMgr.getInstance().remove(TableName.WORLD_CUP_MATCH_INFO, "_id", new ObjectId(p.m_id), user.getDbServerID(), DbName.DB_PLAYER);
        }else //0新增 1编辑
        {
            //matchId是否已存在
            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
            int matchId = 0;
            if (!int.TryParse(p.m_matchId, out matchId))
                return OpRes.op_res_param_not_valid;

            if (p.m_op == 0)
            {
                res = DBMgr.getInstance().keyExists(TableName.WORLD_CUP_MATCH_INFO, "matchId", matchId, user.getDbServerID(), DbName.DB_PLAYER);
                if (res)
                    return OpRes.op_res_has_exist_id;
            }
            else {
                Dictionary<string, object> ret = DBMgr.getInstance().getTableData(TableName.WORLD_CUP_MATCH_INFO, "matchId", matchId, fields, dip);
                if (ret != null && ret.Count !=0 ) 
                {
                    if(Convert.ToString(ret["_id"]) != p.m_id)
                        return OpRes.op_res_has_exist_id;
                }  
            }

            //日期 比赛日期 竞猜截止时间 显示时间
            DateTime resultTime1 = DateTime.Now, resultTime2 = DateTime.Now, resultTime3 = DateTime.Now;
            res = Tool.splitTimeStr(p.m_matchStartTime.Replace('-','/'), ref resultTime1, 3);
            if (!res)
                return OpRes.op_res_time_format_error;

            res = Tool.splitTimeStr(p.m_betEndTime.Replace('-', '/'), ref resultTime2, 3);
            if (!res)
                return OpRes.op_res_time_format_error;

            res = Tool.splitTimeStr(p.m_showTime.Replace('-', '/'), ref resultTime3, 3);
            if (!res)
                return OpRes.op_res_time_format_error;

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("matchId", Convert.ToInt32(p.m_matchId));
            data.Add("matchStartTime", resultTime1);
            data.Add("betEndTime", resultTime2);
            data.Add("showTime", resultTime3);
            data.Add("matchName", p.m_matchNameId);
            data.Add("matchType", p.m_matchType);
            data.Add("homeTeamId", p.m_homeTeamId);
            data.Add("visitTeamId", p.m_visitTeamId);
            data.Add("homeScore", p.m_homeScore);
            data.Add("visitScore", p.m_visitScore);
            data.Add("betMaxCount", p.m_betMaxCount);
            if (p.m_op == 0){
                res = DBMgr.getInstance().insertData(TableName.WORLD_CUP_MATCH_INFO, data, user.getDbServerID(), DbName.DB_PLAYER);
            }else
            {
                res = DBMgr.getInstance().update(TableName.WORLD_CUP_MATCH_INFO, data, "_id", new ObjectId(p.m_id), user.getDbServerID(), DbName.DB_PLAYER);
            }
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
////////////////////////////////////////////////////////////////////////////////////
//客服补单
public class DyOpRepairOrder : DyOpBase 
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamRepairOrder p = (ParamRepairOrder)param;
        bool res = false;

        int rParam = 0;
        if (p.m_op == 0 && p.m_param != "")
        {
            if (!int.TryParse(p.m_param, out rParam))
                return OpRes.op_res_param_not_valid;

            if (rParam <= 0)
                return OpRes.op_res_param_not_valid;
        }

        string[] playerIds = Tool.split(p.m_playerId, ';', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < playerIds.Length; i++ ) 
        {
            res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "player_id", Convert.ToInt32(playerIds[i]), user.getDbServerID(), DbName.DB_PLAYER);
            if (!res)
                return OpRes.op_res_player_not_exist;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        data.Add("time", DateTime.Now);
        data.Add("opReason", p.m_op);
        data.Add("playerIdList", p.m_playerId);
        int k = 0;
        if (p.m_op == 0){
            k = 1;
        }else
        {
            k = 2;
        }
        data.Add("repairOrder", p.m_itemId);  //补单项目
        data.Add("repairBonus", p.m_bonusId);
        
        data.Add("operator",user.m_user);
        data.Add("comments",p.m_comments);

        if (p.m_op == 0)  //补单时，将部分信息写入gmRecharge表
        {
            for (int i = 0; i < playerIds.Length; i++)
            {
                Dictionary<string, object> data_repairOrder = new Dictionary<string, object>();
                data_repairOrder.Clear();
                data_repairOrder.Add("playerId", Convert.ToInt32(playerIds[i]));
                data_repairOrder.Add("param", rParam);
                data_repairOrder.Add("rtype", p.m_rtype);
                DBMgr.getInstance().insertData(TableName.GM_RECHARGE, data_repairOrder, user.getDbServerID(), DbName.DB_PLAYER);
            }
        }
        res = DBMgr.getInstance().insertData(TableName.PUMP_REPAIR_ORDER, data, user.getDbServerID(), DbName.DB_PUMP);
        if(res){
            //获取邮件内容 和 礼包
            RepublicLanguageData allData = RepublicLanguagerMail.getInstance().getValue(k);
            RepairOrderData giftBag = RepairOrderItem.getInstance().getValue(p.m_bonusId);
            if (allData != null && giftBag != null)
            {
                string giftName = giftBag.m_itemName;
                string[] items = Tool.split(giftBag.m_itemCusSerGiftbagItems,',', StringSplitOptions.RemoveEmptyEntries);
                string[] itemsNum = Tool.split(giftBag.m_itemCusSerGiftbagNum, ',', StringSplitOptions.RemoveEmptyEntries);

                string itemList="";
                for(int l=0; l<items.Length;l++)  //礼包 道具ID+count
                {
                    if(l==0){
                        itemList += items[l] + " " + itemsNum[l];
                    }else{
                        itemList += ";" + items[l] + " " + itemsNum[l];
                    }
                }

                for (int i = 0; i < playerIds.Length; i++)
                {
                    ParamSendMail param_mail = new ParamSendMail();
                    param_mail.m_title = allData.m_mailName;
                    param_mail.m_sender = allData.m_mailOperator;
                    param_mail.m_content = string.Format(allData.m_mailText,giftName);
                    param_mail.m_toPlayer = playerIds[i].ToString();
                    param_mail.m_target = 0;
                    param_mail.m_itemList = itemList.Trim();
                    DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
                    OpRes res_mail = mgr.doDyop(param_mail, DyOpType.opTypeSendMail, user);
                }
            }
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SERVICE_REPAIR_ORDER_OP,
                new LogServiceRepairOrder(p.m_op,p.m_playerId,p.m_itemId,p.m_bonusId,user.m_user,p.m_comments), user);
       }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
//流失大户引导完成添加记录
public class DyOpGuideLostPlayers : DyOpBase 
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        bool res = false;

        int playerId = 0;
        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        //验证是否之前存在于表中 同个账号不能添加两次
        res = DBMgr.getInstance().keyExists(TableName.GUIDE_LOST_PLAYER_INFO, "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (res)
            return OpRes.op_res_has_exist_id;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        DateTime time = DateTime.Now;
        data.Add("time", time);
        data.Add("player_id", playerId);
        data.Add("comments", p.m_param);
        res = DBMgr.getInstance().insertData(TableName.GUIDE_LOST_PLAYER_INFO, data, user.getDbServerID(), DbName.DB_PLAYER);
        if(res)
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_GUIDE_LOST_PLAYER,
                new LogGuideLostPlayer(time.ToLocalTime().ToString(), p.m_playerId, p.m_param), user);
        
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//围剿龙王自定义当天付费玩家最终排名
public class DyOpWjlwDefRechargeReward : DyOpBase 
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        bool res = false;
        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_param_not_valid;

        int limit = 0;
        switch(p.m_op)
        {
            case 1: limit = 1; break;
            case 2: limit = 2; break;
            case 3: limit = 3; break;
        }

        IMongoQuery imq = Query.EQ("rewardId",p.m_op);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_WJLW_Act_DEF_RECHARGE_REWARD, imq, user.getDbServerID(), DbName.DB_PLAYER);
        if (user.totalRecord >= limit)
            return OpRes.op_res_player_beyond_limit;

        res = DBMgr.getInstance().keyExists(TableName.STAT_WJLW_Act_DEF_RECHARGE_REWARD, "nickName", p.m_param, user.getDbServerID(), DbName.DB_PLAYER);
        if (res)
            return OpRes.op_res_has_set_account;

        Dictionary<string,object> data = new Dictionary<string,object>();
        data.Clear();
        data.Add("nickName",p.m_param);
        data.Add("rewardId",p.m_op);

        res = DBMgr.getInstance().insertData(TableName.STAT_WJLW_Act_DEF_RECHARGE_REWARD, data, user.getDbServerID(), DbName.DB_PLAYER);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
//围剿龙王移除玩家
public class DyOpWjlwDefRemove : DyOpBase 
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        bool res = false;
        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_param_not_valid;

        res = DBMgr.getInstance().remove(TableName.STAT_WJLW_Act_DEF_RECHARGE_REWARD, "_id", new ObjectId(p.m_param), user.getDbServerID(), DbName.DB_PLAYER);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
//围剿龙王修改当前期望盈利率
public class DyOpWjlwGoldEditCurrExpRate : DyOpBase 
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        bool res = false;
        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_param_not_valid;

        int goldExpRate = 0;
        if (!int.TryParse(p.m_param, out goldExpRate))
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        data.Add("goldExpRate",goldExpRate);
        int dataId = 1;
        res = DBMgr.getInstance().update(TableName.WJLW_ACT_DATA, data, "dataId", dataId, user.getDbServerID(), DbName.DB_PLAYER);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
////////////////////////////////////////////////////////////////////////////////////
//系统空投
public class DyOpStatAirdropSysCtrl : DyOpBase 
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamAirdropSysItem p = (ParamAirdropSysItem)param;
        bool res = false;

        Dictionary<string, object> data = new Dictionary<string, object>();

        //空投ID
        if (string.IsNullOrEmpty(p.m_uuid))
            return OpRes.op_res_param_not_valid;
        int uuid = 0;
        if (!int.TryParse(p.m_uuid, out uuid))
            return OpRes.op_res_param_not_valid;
        
        if (p.m_op == 0) 
        {
            if (uuid <= 900000)
                return OpRes.op_res_param_not_valid;

            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
            res = DBMgr.getInstance().keyExists(TableName.STAT_AIR_DROP_SYS, "uuid", uuid, dip);
            if (res)
                return OpRes.op_res_data_duplicate;

            //空投道具
            if (string.IsNullOrEmpty(p.m_itemId))
                return OpRes.op_res_param_not_valid;

            if (!Regex.IsMatch(p.m_itemId, Exp.TWO_NUM_BY_COLON))
                return OpRes.op_res_param_not_valid;

            string[] itemList = p.m_itemId.Split(':');

            int itemId = Convert.ToInt32(itemList[0]);
            int count = Convert.ToInt32(itemList[1]);
            if (count > 100)
                return OpRes.op_res_reward_beyond_limit;

            //验证道具ID是否存在
            var itemIsExist = ItemCFG.getInstance().getValue(itemId);
            if (itemIsExist == null)
                return OpRes.op_res_param_not_valid;

            //空投密码
            if (string.IsNullOrEmpty(p.m_pwd))
                return OpRes.op_res_param_not_valid;

            if (!Regex.IsMatch(p.m_pwd, Exp.PWD_RULE_AIR_DROP))
                return OpRes.op_res_param_not_valid;

            data.Clear();
            data.Add("uuid", uuid);
            DateTime now = DateTime.Now, end = now.AddHours(12);

            data.Add("genTime", now);
            data.Add("endTime", end);

            data.Add("playerId", 0);

            data.Add("itemID", itemId);
            data.Add("itemCount", count);

            data.Add("receiveID", 0);

            string pwd_encryption = AESHelper.AESEncrypt(p.m_pwd, ConstDef.AES_FOR_AIR_DROP);
            data.Add("password", pwd_encryption);

            res = DBMgr.getInstance().insertData(TableName.STAT_AIR_DROP_SYS, data, user.getDbServerID(), DbName.DB_PLAYER);
        }
        else if (p.m_op == 1) 
        {
            res = DBMgr.getInstance().remove(TableName.STAT_AIR_DROP_SYS, "uuid", uuid, user.getDbServerID(), DbName.DB_PLAYER);
        }

        //日志
        if (res) //日志
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_AIR_DROP_SYS_PUBLISH, new LogAirDropSysPublish(p.m_op, uuid), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

////////////////////////////////////////////////////////////////////////////////////
//实物审核管理
public class DyOpOperationExchangeAudit : DyOpBase 
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        bool res = false;

        Dictionary<string, object> data = new Dictionary<string, object>();

        //实物审核ID
        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_param_not_valid;

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        res = DBMgr.getInstance().keyExists(TableName.EXCHANGE, "exchangeId", p.m_param, dip);
        if (!res)
            return OpRes.op_res_failed;

        data.Clear();
        data.Add("status", p.m_op);
        data.Add("verifyTime", DateTime.Now);
        data.Add("opName", user.m_user);
        res = DBMgr.getInstance().update(TableName.EXCHANGE, data, "exchangeId", p.m_param, user.getDbServerID(), DbName.DB_PLAYER);

        //日志
        if (res) //日志
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_EXCHANGE_AUDIT, new LogServiceExchangeAudit(p.m_op, p.m_param), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
////////////////////////////////////////////////////////////////////////////////////
//高级场玩法控制管理
public class DyOpFishlordAdvancedRoomCtrl : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamFishlordAdvancedRoomItem p = (ParamFishlordAdvancedRoomItem)param;
        bool res = false;

        Dictionary<string, object> data = new Dictionary<string, object>();
        //奖池期望值/控制系数
        if (p.m_op == 3 || p.m_op == 4 || p.m_op == 5)
        {
            data.Clear();

            res = DBMgr.getInstance().keyExists(TableName.FISHLORD_ROOM, "room_id", 3, user.getDbServerID(), DbName.DB_GAME);
            if (res) 
            {
                if (p.m_op == 3)
                {
                    data.Add("LotterPoolExpect", p.m_ratio);
                }
                else if (p.m_op == 4)
                {
                    data.Add("LotterCtrRate", p.m_ratio);
                }
                else {
                    data.Add("JsckpotSmallPump", p.m_ratio/100.0);
                }
                res = DBMgr.getInstance().update(TableName.FISHLORD_ROOM, data, "room_id", 3, user.getDbServerID(), DbName.DB_GAME);

                if (res) //日志
                    OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_ADVANCED_ROOM_CTRL, new LogFishlordAdvancedRoomCtrl(p.m_op, p.m_ratio), user);
            }
        }
        else 
        {
            res = DBMgr.getInstance().keyExists(TableName.FISHLORD_ADVANCED_ROOM_CTRL, "level", p.m_op, user.getDbServerID(), DbName.DB_GAME);

            data.Clear();
            int level = 2 - p.m_op;
            if (!res)
                data.Add("level", level);

            data.Add("maxCount", p.m_maxWinCount);
            data.Add("ratio", p.m_ratio);

            if (res)
            {
                res = DBMgr.getInstance().update(TableName.FISHLORD_ADVANCED_ROOM_CTRL, data, "level", level, user.getDbServerID(), DbName.DB_GAME);
            }
            else
            {
                res = DBMgr.getInstance().insertData(TableName.FISHLORD_ADVANCED_ROOM_CTRL, data, user.getDbServerID(), DbName.DB_GAME);
            }

            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_ADVANCED_ROOM_CTRL, new LogFishlordAdvancedRoomCtrl(p.m_op, p.m_ratio, p.m_maxWinCount), user);

        }

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
////////////////////////////////////////////////////////////////////////////////////
//极光推送
public class PolarLightsParam
{
    public string m_id = "";
    public int m_op;  //0新增 1编辑 2删除 
    public string m_channelList = "";
    public string m_vipList = "";
    public string m_date = "";
    public string m_weekList = "";
    public string m_time = "";
    public string m_content = "";
    public string m_note = "";
}
public class DyOpPolarLightsPush : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        PolarLightsParam p = (PolarLightsParam)param;
        bool res = false;

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        if (p.m_op == 2) //删除
        {
            res = DBMgr.getInstance().remove(TableName.CONFIG_POLAR_LIGHTS_PUSH, "id", p.m_id, serverId, DbName.DB_CONFIG);
        }
        else //0新增 1编辑
        {
            //日期
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            res = Tool.splitTimeStr(p.m_date, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (p.m_op == 0)
            {
                data.Add("id", Guid.NewGuid().ToString());
            }
            data.Add("targetChannel", p.m_channelList);
            data.Add("targetVip", p.m_vipList);
            data.Add("week", p.m_weekList);
            data.Add("hour", p.m_time);
            data.Add("startTime", mint);
            data.Add("endTime", maxt);
            data.Add("content", p.m_content);
            data.Add("note", p.m_note);
            if (p.m_op == 0)
            {
                res = DBMgr.getInstance().insertData(TableName.CONFIG_POLAR_LIGHTS_PUSH, data, serverId, DbName.DB_CONFIG);
            }else if (p.m_op == 1)
            {
                res = DBMgr.getInstance().update(TableName.CONFIG_POLAR_LIGHTS_PUSH, data, "id", p.m_id, serverId, DbName.DB_CONFIG);
            }
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
///////////////////////////////////////////////////////////////////////////
//弹头统计重置
public class DyOpFishlordBulletHeadReset : DyOpBase
{
    static string[] m_fields = { "torpedoId", "useType" };
    public override OpRes doDyop(object param, GMUser user)
    {
        bool res = false;
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
                DBMgr.getInstance().executeQuery(TableName.PUMP_TORPEDO, dip, null, 0, 0, m_fields, "torpedoId", false);
        if (dataList != null && dataList.Count != 0)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].ContainsKey("torpedoId") && dataList[i].ContainsKey("useType"))
                {
                    int bulletHeadId = Convert.ToInt32(dataList[i]["torpedoId"]);
                    int useType = Convert.ToInt32(dataList[i]["useType"]);

                    IMongoQuery imq = Query.And(Query.EQ("torpedoId", bulletHeadId), Query.EQ("useType", useType));

                    data.Clear();
                    data.Add("useCount", 0L);
                    data.Add("outlayGold", 0L);

                    res = DBMgr.getInstance().update(TableName.PUMP_TORPEDO, data, imq, dip);
                }
            }
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
//弹头区间修正系数修改
public class DyOpFishlordBulletHeadRcParamEdit : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        bool res = false;
        ParamQuery p = (ParamQuery)param;

        string[] data_arr = p.m_param.Split(',');
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        res = DBMgr.getInstance().keyExists(TableName.FISHLORD_BULLET_HEAD, "bulletHeadId", p.m_op, dip);
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        if (!res)
        {
            data.Add("bulletHeadId", p.m_op);
        }
        int type = Convert.ToInt32(p.m_playerId);
        if (type == 4)
        {
            data.Add("goldUseMin", Convert.ToInt32(p.m_param));
            data.Add("goldUseMax", Convert.ToInt32(p.m_channelNo));
        }
        else {
            data.Add("goldKillMin", Convert.ToInt32(p.m_param));
            data.Add("goldKillMax", Convert.ToInt32(p.m_channelNo));
        }
        string strParam = p.m_param + ',' + p.m_channelNo;
        if (!res)
        {
            res = DBMgr.getInstance().insertData(TableName.FISHLORD_BULLET_HEAD, data, user.getDbServerID(), DbName.DB_GAME);
            if (res) //日志
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_BULLET_HEAD_RCPARAM_EDIT,
                   new LogBulletHeadRangeCorrectParamEdit(p.m_op, strParam, type), user);
        }else
        {
            res = DBMgr.getInstance().update(TableName.FISHLORD_BULLET_HEAD, data, "bulletHeadId", p.m_op, user.getDbServerID(), DbName.DB_GAME);
            if (res) //日志
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FISHLORD_BULLET_HEAD_RCPARAM_EDIT,
                   new LogBulletHeadRangeCorrectParamEdit(p.m_op, strParam, type), user);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

////////////////////////////////////////////////////////////////////////////
//修改机器人最高积分
public class DyOpRobotMaxScoreEdit : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        bool res = false;
        ParamQuery p = (ParamQuery)param;
        int key = 1;
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        data.Add("robotMaxScore", Convert.ToInt32(p.m_param));
        res = DBMgr.getInstance().update(TableName.FISHLORD_BAOJIN_SYS, data, "key", key, user.getDbServerID(), DbName.DB_GAME);
        if (res)
        {
            //日志
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_ROBOT_MAX_SCORE_EDIT,
            new LogFishlordRobotMaxScoreEdit(Convert.ToInt32(p.m_param)),
            user);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
///////////////////////////////////////////////////////////////////////////////
//游客快速开始添加账号
public class playerInfoItem
{
    public string m_playerId;
    public string m_account;
    public string m_bindPhone = "";
    public string m_platform = "";
    public string m_deviceId = "";
    public DateTime m_regedittime;
    public string m_regeditip = "";
    public string m_channel = "";
}
public class DyOpFasterStartForVisitor : DyOpBase
{
    static string[] s_fields = { "account", "platform", "deviceId", "ChannelID", "create_time", "regeditip", "bindPhone" };

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEditSpecial(RightDef.SVR_FASTER_START_FOR_VISITOR, user))
            return OpRes.op_res_no_right;

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        ParamPlayerInfo p = (ParamPlayerInfo)param;
        //玩家ID
        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
        {
            return OpRes.op_res_param_not_valid;
        }

        //玩家账号
        if (!Regex.IsMatch(p.m_account, Exp.ACCOUNT_RULE))
        {
            return OpRes.op_res_param_not_valid;
        }
        //密码
        if (!Regex.IsMatch(p.m_pwd, Exp.PWD_RULE))
            return OpRes.op_res_param_not_valid;

        bool exists = DBMgr.getInstance().keyExists(TableName.PLAYER_ACCOUNT, "acc", p.m_account, serverId, DbName.DB_ACCOUNT);
        if (exists)
            return OpRes.op_res_has_exist;

        IMongoQuery imq1 = Query.EQ("player_id", playerId);
        IMongoQuery imq2 = Query.EQ("is_robot", false);
        IMongoQuery imq = Query.And(imq1, imq2);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dip, imq, s_fields);
        if (data == null)
            return OpRes.op_res_player_not_exist;

        playerInfoItem tmp = new playerInfoItem();
        tmp.m_account = Convert.ToString(data["account"]);
        if (data.ContainsKey("platform"))
        {
            tmp.m_platform = Convert.ToString(data["platform"]);
        }

        if (data.ContainsKey("deviceId"))
        {
            tmp.m_deviceId = Convert.ToString(data["deviceId"]);
        }

        if (data.ContainsKey("ChannelID"))
        {
            tmp.m_channel = ItemHelp.channelToString(Convert.ToInt32(data["ChannelID"]));
        }

        if (data.ContainsKey("create_time"))
        {
            tmp.m_regedittime = Convert.ToDateTime(data["create_time"]).ToLocalTime();
        }
        if (data.ContainsKey("lastIp"))
        {
            tmp.m_regeditip = Convert.ToString(data["lastIp"]);
        }
        if (data.ContainsKey("bindPhone"))
        {
            tmp.m_bindPhone = Convert.ToString(data["bindPhone"]);
        }
        bool res = false;
        string pwd = Tool.getMD5Hash(p.m_pwd);
        Dictionary<string, object> data_update = new Dictionary<string, object>();
        Dictionary<string, object> dataAccount = DBMgr.getInstance().getTableData(TableName.PLAYER_ACCOUNT, "acc_real", tmp.m_account, serverId, DbName.DB_ACCOUNT);
        if (dataAccount == null)
        {
            if (tmp.m_platform == "default") //有时default平台会找不到账号
                return OpRes.op_res_has_default_acc;

            data_update.Clear();
            data_update.Add("acc", p.m_account);
            data_update.Add("acc_real", tmp.m_account);
            data_update.Add("pwd", pwd);
            data_update.Add("randkey", 0);
            data_update.Add("lasttime", 0L);
            data_update.Add("regedittime", tmp.m_regedittime);
            data_update.Add("regeditip", tmp.m_regeditip);
            data_update.Add("lastip", tmp.m_regeditip);
            data_update.Add("updatepwd", false);
            data_update.Add("platform", tmp.m_platform);
            data_update.Add("channel", tmp.m_channel);
            data_update.Add("deviceId", tmp.m_deviceId);

            res = DBMgr.getInstance().insertData(TableName.PLAYER_ACCOUNT, data_update, serverId, DbName.DB_ACCOUNT);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FASTER_START_FOR_VISITOR,
                    new LogFasterStartForVisitor(p.m_playerId), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
        else
        {
            if (tmp.m_platform != "default")
            {
                return OpRes.op_res_has_set_account;
            }
        }
        ///////////////////////////更新AccountTable表////////////////////////////////////////////////////
        //acc_dev 或 acc_visitor 不存在或为空 直接返回
        if ((!dataAccount.ContainsKey("acc_dev") || string.IsNullOrEmpty(Convert.ToString(dataAccount["acc_dev"]))) ||
            (!dataAccount.ContainsKey("acc_visitor") || string.IsNullOrEmpty(Convert.ToString(dataAccount["acc_visitor"]))))
        {
            return OpRes.op_res_not_visitor;
        }

        if (dataAccount.ContainsKey("acc") && !string.IsNullOrEmpty(Convert.ToString(dataAccount["acc"])))
        {
            return OpRes.op_res_has_set_account;
        }

        data_update.Clear();
        data_update.Add("acc", p.m_account);
        if (!dataAccount.ContainsKey("pwd") || string.IsNullOrEmpty(Convert.ToString(dataAccount["pwd"])))
        {
            data_update.Add("pwd", pwd);
        }

        if (!dataAccount.ContainsKey("channel") || string.IsNullOrEmpty(Convert.ToString(dataAccount["channel"])))
        {
            data_update.Add("channel", tmp.m_channel);
        }

        if (!dataAccount.ContainsKey("deviceId") || string.IsNullOrEmpty(Convert.ToString(dataAccount["deviceId"])))
        {
            data_update.Add("deviceId", tmp.m_deviceId);
        }
        if (!dataAccount.ContainsKey("regeditip") || string.IsNullOrEmpty(Convert.ToString(dataAccount["regeditip"])))
        {
            if (dataAccount.ContainsKey("lastip") && !string.IsNullOrEmpty(Convert.ToString(dataAccount["lastip"])))
            {
                data_update.Add("regeditip", Convert.ToString(dataAccount["lastip"]));
            }
            else
            {
                data_update.Add("regeditip", tmp.m_regeditip);
                data_update.Add("lastip", tmp.m_regeditip);
            }
        }

        if (!dataAccount.ContainsKey("regedittime") || string.IsNullOrEmpty(Convert.ToString(dataAccount["regedittime"])))
        {
            data_update.Add("regedittime", tmp.m_regedittime);
        }

        res = DBMgr.getInstance().update(TableName.PLAYER_ACCOUNT, data_update, "acc_real", tmp.m_account, serverId, DbName.DB_ACCOUNT);
        if (res)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_FASTER_START_FOR_VISITOR,
                new LogFasterStartForVisitor(p.m_playerId), user);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
//////////////////////////////////////////////////////////////////////////
public class ParamSendMail
{
    public string m_title = "";
    public string m_sender = "";
    public string m_content = "";
    public string m_toPlayer = "";
    public string m_itemList = "";
    public string m_validDay = "";
    public int m_target;
    public bool m_isCheck = false;
    public bool m_isAdmin = false;

    // 条件，下线时间
    public string m_condLogoutTime = "";
    // 条件，vip等级区间
    public string m_condVipLevel = "";

    public string m_comment = "";
    public string m_result = "";
}

public class ParamCheckMail : ParamSendMail
{
    public string m_id = "";
}

public class DyOpSendMail : DyOpBase
{
    private string m_successPlayer = "";

    public override OpRes doDyop(object param, GMUser user)
    {
        //发邮件权限 admin和service
        if (!RightMgr.getInstance().canEditSpecial(RightDef.SVR_SEND_MAIL, user))
            return OpRes.op_res_no_right;

        ParamSendMail p = (ParamSendMail)param;

        int days = 7;
        List<int> playerList = new List<int>();
        List<ParamItem> tmpItem = new List<ParamItem>();
        OpRes code = checkValid(p, user, ref days, tmpItem, playerList);
        if (code != OpRes.opres_success)
            return code;
        if (user.m_type != "service")
        {
            code = sendRewardCheck(user, tmpItem);
            if (code != OpRes.opres_success)
                return code;
        }

        if (p.m_isCheck) // 缓存
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id", Guid.NewGuid().ToString());
            data.Add("title", p.m_title);
            data.Add("sender", p.m_sender);
            data.Add("content", p.m_content);
            data.Add("validDay", p.m_validDay);
            data.Add("toPlayer", p.m_toPlayer);
            data.Add("itemList", p.m_itemList);
            data.Add("target", p.m_target);
            data.Add("time", DateTime.Now);
            data.Add("logOutTime", p.m_condLogoutTime);
            data.Add("vipLevel", p.m_condVipLevel);
            data.Add("comment", p.m_comment);
            bool res = DBMgr.getInstance().insertData(TableName.CHECK_MAIL, data, user.getDbServerID(), DbName.DB_PLAYER);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        if (p.m_target == 0) // 给指定玩家发送
        {
            BsonDocument mailItem = null;

            if (p.m_itemList != "")
            {
                Dictionary<string, object> dd = new Dictionary<string, object>();
                for (int i = 0; i < tmpItem.Count; i++)
                {
                    Dictionary<string, object> tmpd = new Dictionary<string, object>();
                    tmpd.Add("giftId", tmpItem[i].m_itemId);
                    tmpd.Add("count", tmpItem[i].m_itemCount);
                    tmpd.Add("receive", false);
                    dd.Add(i.ToString(), tmpd.ToBsonDocument());
                }
                mailItem = dd.ToBsonDocument();
            }
            return specialSend(p, user, days, mailItem, playerList);
        }

        return fullSend(p, user, days);
    }

    // 返回所有待检测邮件列表
    public void getCheckMailList(GMUser user, List<ParamCheckMail> result)
    {
        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.CHECK_MAIL, user.getDbServerID(), DbName.DB_PLAYER);
        if (data == null || data.Count == 0)
            return;

        for (int i = 0; i < data.Count; i++)
        {
            ParamCheckMail tmp = new ParamCheckMail();
            result.Add(tmp);

            tmp.m_id = Convert.ToString(data[i]["id"]);
            tmp.m_title = Convert.ToString(data[i]["title"]);
            tmp.m_sender = Convert.ToString(data[i]["sender"]);
            tmp.m_content = Convert.ToString(data[i]["content"]);
            tmp.m_validDay = Convert.ToString(data[i]["validDay"]);
            tmp.m_toPlayer = Convert.ToString(data[i]["toPlayer"]);
            tmp.m_itemList = Convert.ToString(data[i]["itemList"]);
            tmp.m_target = Convert.ToInt32(data[i]["target"]);
            tmp.m_result = Convert.ToDateTime(data[i]["time"]).ToLocalTime().ToString();
            tmp.m_condLogoutTime = Convert.ToString(data[i]["logOutTime"]);
            tmp.m_condVipLevel = Convert.ToString(data[i]["vipLevel"]);
            if (data[i].ContainsKey("comment"))
            {
                tmp.m_comment = Convert.ToString(data[i]["comment"]);
            }
        }
    }

    public Dictionary<string, object> getCheckMail(GMUser user, string id)
    {
        return DBMgr.getInstance().getTableData(TableName.CHECK_MAIL, "id", id, user.getDbServerID(), DbName.DB_PLAYER);
    }

    public void removeCheckMail(GMUser user, string id)
    {
        DBMgr.getInstance().remove(TableName.CHECK_MAIL, "id", id, user.getDbServerID(), DbName.DB_PLAYER);
    }

    private OpRes specialSend(ParamSendMail p, GMUser user, int days, BsonDocument mailItem, List<int> playerList)
    {
        bool res = false;
        m_successPlayer = "";
        DateTime now = DateTime.Now;
        DateTime nt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

        List<Dictionary<string, object>> docList = new List<Dictionary<string, object>>();

        for (int i = 0; i < playerList.Count; i++)
        {
            res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "player_id", playerList[i], user.getDbServerID(), DbName.DB_PLAYER);
            if (!res)
            {
                p.m_result += playerList[i] + " ";
                continue;
            }
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("title", p.m_title);
            data.Add("sender", p.m_sender);
            data.Add("content", p.m_content);

            data.Add("time", nt);
            data.Add("deadTime", nt.AddDays(days));
            data.Add("isReceive", false);
            data.Add("playerId", playerList[i]);

            // 标识是系统发送的邮件
            data.Add("senderId", 0);
            data.Add("mainReason", (int)PropertyReasonType.type_reason_receive_gm_mail);

            if (mailItem != null)
            {
                data.Add("gifts", mailItem);
            }
            m_successPlayer += playerList[i] + " ";
            docList.Add(data);
        }
        res = DBMgr.getInstance().insertData(TableName.PLAYER_MAIL, docList, user.getDbServerID(), DbName.DB_PLAYER);
        if (res)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SEND_MAIL,
                new LogSendMail(p.m_title, p.m_sender, p.m_content, m_successPlayer, p.m_itemList, days),
                user,
                p.m_comment);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    // 全局发放
    private OpRes fullSend(ParamSendMail p, GMUser user, int days)
    {
        ParamSendMailFullSvr param = new ParamSendMailFullSvr();
        param.m_dbServerIP = user.m_dbIP;
        param.m_title = p.m_title;
        param.m_sender = p.m_sender;
        param.m_content = p.m_content;
        param.m_itemList = p.m_itemList;
        param.m_validDay = days;
        param.m_condition = new Dictionary<string, object>();
        if (p.m_condLogoutTime != "")
        {
            param.m_condition.Add("logOutTime", p.m_condLogoutTime);
        }
        if (p.m_condVipLevel != "")
        {
            param.m_condition.Add("vipLevel", p.m_condVipLevel);
        }

        OpRes res = RemoteMgr.getInstance().reqSendMail(param);
        if (res == OpRes.opres_success)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SEND_MAIL,
                new LogSendMail(p.m_title, p.m_sender, p.m_content, "", p.m_itemList, days),
                user,
                p.m_comment);
        }
        return res;
    }

    // 邮件的合法性检验
    private OpRes checkValid(ParamSendMail p, GMUser user, ref int days, List<ParamItem> itemList, List<int> playerList)
    {
        if (!string.IsNullOrEmpty(p.m_validDay))
        {
            if (!int.TryParse(p.m_validDay, out days))
            {
                return OpRes.op_res_param_not_valid;
            }
        }

        if (p.m_itemList != "")
        {
            if (itemList != null)
            {
                ////////////////////////////////////////////////////////////
                //判断如果邮件道具中包含话费，数量*100
                string[] ilist_arr = Tool.split(p.m_itemList, ';');
                string item_list = "";
                foreach(var list in ilist_arr)
                {
                    string[] li = Tool.split(list, ' ');
                    if (li[0] == "3")
                        li[1] = (Convert.ToDouble(li[1]) * 100).ToString();

                    item_list += ';' + string.Join(" ", li);
                }

                p.m_itemList = item_list.Trim(';').Trim(' ');
                //////////////////////////////////////////////////////////

                bool res = Tool.parseItemList(p.m_itemList, itemList);
                if (!res)
                {
                    return OpRes.op_res_param_not_valid;
                }

                for (int i = 0; i < itemList.Count; i++)
                {
                    var t = ItemCFG.getInstance().getValue(itemList[i].m_itemId);
                    if (t == null)
                    {
                        p.m_result += itemList[i].m_itemId + " ";
                    }
                }

                if (p.m_result != "")
                    return OpRes.op_res_item_not_exist;
            }
            else
            {
                if (!Tool.isItemListValid(p.m_itemList, true))
                    return OpRes.op_res_param_not_valid;
            }
        }

        if (p.m_target == 0) // 给指定玩家
        {
            bool res = Tool.parseNumList(p.m_toPlayer, playerList);
            if (!res)
                return OpRes.op_res_param_not_valid;

            for (int i = 0; i < playerList.Count; i++)
            {
                res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "player_id", playerList[i], user.getDbServerID(), DbName.DB_PLAYER);
                if (!res)
                {
                    p.m_result += playerList[i] + " ";
                }
            }

            if (p.m_result != "")
                return OpRes.op_res_player_not_exist;

            if (p.m_condVipLevel != "")
                return OpRes.op_res_param_not_valid;

            if (p.m_condLogoutTime != "")
                return OpRes.op_res_time_format_error;
        }
        else // 全服发放
        {
            if (p.m_condVipLevel != "")
            {
                if (!Tool.isTwoNumValid(p.m_condVipLevel))
                    return OpRes.op_res_param_not_valid;
            }

            if (p.m_condLogoutTime != "")
            {
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                bool res = Tool.splitTimeStr(p.m_condLogoutTime, ref mint, ref maxt);
                if (!res)
                    return OpRes.op_res_time_format_error;
            }
        }

        return OpRes.opres_success;
    }

    private OpRes sendRewardCheck(GMUser user, List<ParamItem> itemList)
    {
        return OpRes.opres_success;

        double val = 0.0;
        foreach (var item in itemList)
        {
            val += transToRMB(item.m_itemId, item.m_itemCount);
        }

        OpRightInfo info = ResMgr.getInstance().getOpRightInfo(user.m_type);
        if (info == null)
            return OpRes.op_res_reward_beyond_limit;

        if (info.m_sendRewardLimit == 0) // 0表示没有限制
            return OpRes.opres_success;

        if (val > info.m_sendRewardLimit)
            return OpRes.op_res_reward_beyond_limit;
        return OpRes.opres_success;
    }

    private double transToRMB(int itemId, int count)
    {
        double val = 0.0;
        int r = 0;
        if (itemId == 1) // 金币
        {
            r = 10000;
        }
        else if (itemId == 2) // 礼券
        {
            r = 10;
        }
        if (r > 0)
        {
            val = (double)count / r;
        }
        return val;
    }
}
//////////////////////////////////////////////////////////////////////////
public class ParamModifyPwd
{
    public string m_account = "";
    public string m_phone = "";

    // 玩家ID
    public string m_playerId = "";
    public string m_newPwd = "";
    public int m_pwdType;
}

public class DyOpModifyPwd : DyOpBase
{
    //  static string[] m_fields = { "phone", "acc" };

    // player_info中的
    static string[] s_fields1 = { "account", "platform" };

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.SVR_RESET_PLAYER_PWD, user))
            return OpRes.op_res_no_right;

        ParamModifyPwd p = (ParamModifyPwd)param;

        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        if (string.IsNullOrEmpty(p.m_newPwd))
            return OpRes.op_res_param_not_valid;

        if(p.m_newPwd.Length < 6 || p.m_newPwd.Length > 14)
            return OpRes.op_res_pwd_not_valid;

        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO,
                "player_id", playerId,
                s_fields1, user.getDbServerID(), DbName.DB_PLAYER);
        if (data == null)
            return OpRes.op_res_not_found_data;

        if (p.m_pwdType == 1) // 保险箱密码
        {
            Dictionary<string, object> tmp = new Dictionary<string, object>();
            tmp.Add("safeBoxPwd", Tool.getMD5Hash(p.m_newPwd));
            bool res1 = DBMgr.getInstance().update(TableName.PLAYER_INFO, tmp,
                "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            return res1 ? OpRes.opres_success : OpRes.op_res_failed;
        }

        if (!data.ContainsKey("platform"))
            return OpRes.op_res_failed;

        if (!data.ContainsKey("account"))
            return OpRes.op_res_failed;

        // 不论是不是第三方账号，都可以更改。第三方账号可以绑定到PLAYER_ACCOUNT里面。
        /*string plat = Convert.ToString(data["platform"]);
        if (plat != "default")
            return OpRes.op_res_third_part_platform;*/

        string acc = Convert.ToString(data["account"]);

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        bool res = DBMgr.getInstance().keyExists(TableName.PLAYER_ACCOUNT, "acc_real", acc,
            serverId, DbName.DB_ACCOUNT);
        if (!res)
            return OpRes.op_res_not_found_data;

        Dictionary<string, object> upData = new Dictionary<string, object>();
        upData.Add("pwd", Tool.getMD5Hash(p.m_newPwd + "&@*(#kas9581gajk"));
        res = DBMgr.getInstance().update(TableName.PLAYER_ACCOUNT, upData, "acc_real", acc, serverId, DbName.DB_ACCOUNT);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    /*public override OpRes _doDyop(object param, GMUser user)
    {
        ParamModifyPwd p = (ParamModifyPwd)param;

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.PLAYER_ACCOUNT, "acc", p.m_account, m_fields, serverId, DbName.DB_ACCOUNT);
        if (data == null)
            return OpRes.op_res_not_found_data;

        if (!data.ContainsKey("phone"))
        {
            return OpRes.op_res_not_bind_phone;
        }

        p.m_phone = Convert.ToString(data["phone"]);
        OpRes code = sendMsgToPhone(p.m_phone);
        if (code == OpRes.opres_success)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_RESET_PWD, new LogResetPwd(p.m_account, p.m_phone), user);
        }
        return code;
    }*/

    private OpRes sendMsgToPhone(string phone)
    {
        try
        {
            string url = string.Format("{0}?phone={1}&not_often=0", WebConfigurationManager.AppSettings["findAccountWeb"], phone);
            var ret = HttpPost.Post(new Uri(url), null, null);
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr == "resSuccess")
                {
                    return OpRes.opres_success;
                }
            }
        }
        catch (System.Exception ex)
        {
        }
        return OpRes.op_res_failed;
    }

    // 取得截止时间
    public long calEndTime(DateTime now)
    {
        DateTime t = now.AddDays(1);
        DateTime e = new DateTime(t.Year, t.Month, t.Day, 0, 0, 0);
        return e.Ticks;
    }
}
/////////////////////////////////////////////////////////////////////////
public class ParamBlock
{

    // 为true表示停封
    public bool m_isBlock;
    public string m_param;
    public string m_comment = "";
    public int m_op;
    public OpRes kickPlayer;
}

public class ResultBlock
{
    public string m_param = "";
    public string m_time = "";
}

// 停封账号
public class DyOpBlockAccount : DyOpBase
{
    static string[] s_fields = { "acc", "blockTime" };

    public override OpRes doDyop(object param, GMUser user)
    {
        ParamBlock p = (ParamBlock)param;
        bool res = false;
        int accServerId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (accServerId == -1)
            return OpRes.op_res_failed;

        if (p.m_isBlock)
        {
            IMongoQuery imq = Query.EQ("acc", BsonValue.Create(p.m_param));

            long count = DBMgr.getInstance().getRecordCount(TableName.PLAYER_ACCOUNT, imq, accServerId, DbName.DB_ACCOUNT);
            if (count == 0)
                return OpRes.op_res_not_found_data;

            Dictionary<string, object> data = new Dictionary<string, object>();
            // 账号
            data["acc"] = p.m_param;
            data["blockTime"] = DateTime.Now;
            data["block"] = true;
            res = DBMgr.getInstance().update(TableName.PLAYER_ACCOUNT, data, "acc", p.m_param, accServerId, DbName.DB_ACCOUNT);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BLOCK_ACC,
                    new LogBlockAcc(p.m_param, p.m_isBlock),
                    user,
                    p.m_comment);
            }
        }
        else
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["block"] = false;

            string[] str = Tool.split(p.m_param, ',');
            for (int i = 0; i < str.Length; i++)
            {
                res = DBMgr.getInstance().update(TableName.PLAYER_ACCOUNT, data, "acc", str[i], accServerId, DbName.DB_ACCOUNT);
            }
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BLOCK_ACC, new LogBlockAcc(p.m_param, p.m_isBlock), user);
            }
        }

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    // 返回当前停封的所有账号
    public void getAccountList(GMUser user, List<ResultBlock> result)
    {
        int accServerId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (accServerId == -1)
            return;

        IMongoQuery imq = Query.EQ("block", BsonValue.Create(true));
        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.PLAYER_ACCOUNT, accServerId, DbName.DB_ACCOUNT, imq, 0, 0, s_fields);
        for (int i = 0; i < data.Count; i++)
        {
            ResultBlock tmp = new ResultBlock();
            result.Add(tmp);
            tmp.m_param = Convert.ToString(data[i]["acc"]);
            tmp.m_time = Convert.ToDateTime(data[i]["blockTime"]).ToLocalTime().ToString();
        }
    }
}

//////////////////////////////////////////////////////////////////////////

// 停封Id
public class DyOpBlockId : DyOpBase
{
    static string[] s_fields = { "player_id", "blockTime" };
    private static string[] s_retFields = { "account" };

    public override OpRes doDyop(object param, GMUser user)
    {
        ParamBlock p = (ParamBlock)param;
        bool res = false;

        if (p.m_isBlock)
        {
            if (!RightMgr.getInstance().canEdit(RightDef.SVR_BLOCK_PLAYER_ID, user))
                return OpRes.op_res_no_right;

            int playerId = 0;
            if (!int.TryParse(p.m_param, out playerId))
            {
                return OpRes.op_res_param_not_valid;
            }
            IMongoQuery imq = Query.EQ("player_id", BsonValue.Create(playerId));
            long count = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, user.getDbServerID(), DbName.DB_PLAYER);
            if (count == 0)
                return OpRes.op_res_not_found_data;

            Dictionary<string, object> data = new Dictionary<string, object>();
            data["blockTime"] = DateTime.Now;
            data["delete"] = true;
            res = DBMgr.getInstance().update(TableName.PLAYER_INFO, data, imq, user.getDbServerID(), DbName.DB_PLAYER);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BLOCK_ID,
                    new LogBlockId(p.m_param, p.m_isBlock),
                    user,
                    p.m_comment);
            }

            //踢出玩家 
            p.kickPlayer = kickPlayerRes(playerId, user, s_retFields);
        }
        else
        {
            if (!RightMgr.getInstance().canEdit(RightDef.SVR_UN_BLOCK_PLAYER_ID, user))
                return OpRes.op_res_no_right;

            int playerId = 0;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["delete"] = false;
            string[] str = Tool.split(p.m_param, ',');
            for (int i = 0; i < str.Length; i++)
            {
                if (!int.TryParse(str[i], out playerId))
                {
                    continue;
                }
                res = DBMgr.getInstance().update(TableName.PLAYER_INFO, data, "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            }
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BLOCK_ID, new LogBlockId(p.m_param, p.m_isBlock), user);
            }
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    //踢出玩家
    OpRes kickPlayerRes(int playerId, GMUser user, string[] s_retFields)
    {
        Dictionary<string, object> getData = QueryBase.getPlayerProperty(playerId, user, s_retFields);
        if (getData == null)
        {
            return OpRes.op_res_player_not_exist;
        }

        return DyOpKickPlayer.kick(playerId, 600, user);
    }

    // 返回当前停封的所有玩家ID
    public void getIdList(GMUser user, List<ResultBlock> result)
    {
        IMongoQuery imq = Query.EQ("delete", BsonValue.Create(true));
        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO,
            user.getDbServerID(), DbName.DB_PLAYER, imq, 0, 0, s_fields, "blockTime", false);
        if (data == null)
            return;

        for (int i = 0; i < data.Count; i++)
        {
            ResultBlock tmp = new ResultBlock();
            result.Add(tmp);
            tmp.m_param = Convert.ToString(data[i]["player_id"]);
            tmp.m_time = Convert.ToDateTime(data[i]["blockTime"]).ToLocalTime().ToString();
        }
    }

    public void getIdList(GMUser user, ParamQuery param, List<ResultBlock> result)
    {
        ParamQuery p = (ParamQuery)param;
        QueryCondition m_cond = new QueryCondition();
        if (!string.IsNullOrEmpty(p.m_param))
        {
            int playerId = 0;
            if (!int.TryParse(p.m_param, out playerId))
            {
                return;
            }
            m_cond.addImq(Query.EQ("player_id", playerId));
        }
        m_cond.addImq(Query.EQ("delete", BsonValue.Create(true)));
        IMongoQuery imq = m_cond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, dip);
        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO,
            dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, s_fields, "blockTime", false);
        if (data == null)
            return;

        bool add = false;

        for (int i = 0; i < data.Count; i++)
        {
            add = false;
            ResultBlock tmp = new ResultBlock();

            Dictionary<string, object> da = data[i];
            if (da.ContainsKey("player_id"))
            {
                tmp.m_param = Convert.ToString(da["player_id"]);
                add = true;
            }
            if (da.ContainsKey("blockTime"))
            {
                tmp.m_time = Convert.ToDateTime(da["blockTime"]).ToLocalTime().ToString();
                add = true;
            }

            if (add)
            {
                result.Add(tmp);
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////

// 停封IP
public class DyOpBlockIP : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        ParamBlock p = (ParamBlock)param;
        bool res = false;
        int accServerId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (accServerId == -1)
            return OpRes.op_res_failed;

        if (p.m_isBlock)
        {
            Match match = Regex.Match(p.m_param, Exp.IP_ADDRESS);
            if (!match.Success)
            {
                match = Regex.Match(p.m_param, Exp.IP_ADDRESS1);
                if (!match.Success)
                    return OpRes.op_res_param_not_valid;
            }

            string ip = match.Groups[1].Value;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["blockTime"] = DateTime.Now;
            data["ip"] = ip;
            res = DBMgr.getInstance().save(TableName.BLOCK_IP, data, "ip", ip, accServerId, DbName.DB_ACCOUNT);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BLOCK_IP,
                    new LogBlockIP(ip, p.m_isBlock),
                    user,
                    p.m_comment);
            }
        }
        else
        {
            string[] str = Tool.split(p.m_param, ',');
            for (int i = 0; i < str.Length; i++)
            {
                res = DBMgr.getInstance().remove(TableName.BLOCK_IP, "ip", str[i], accServerId, DbName.DB_ACCOUNT);
            }
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BLOCK_IP, new LogBlockIP(p.m_param, p.m_isBlock), user);
            }
        }

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public void getIPList(GMUser user, List<ResultBlock> result)
    {
        int accServerId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (accServerId == -1)
            return;

        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.BLOCK_IP, accServerId, DbName.DB_ACCOUNT);
        for (int i = 0; i < data.Count; i++)
        {
            ResultBlock tmp = new ResultBlock();
            result.Add(tmp);
            tmp.m_param = Convert.ToString(data[i]["ip"]);
            tmp.m_time = Convert.ToDateTime(data[i]["blockTime"]).ToLocalTime().ToString();
        }
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamRecharge
{
    public int m_rtype;
    public string m_playerId = "";
    public string m_param = "";
    public string m_comment = "";
}

// 后台充值
public class DyOpRecharge : DyOpBase
{
    private Dictionary<string, object> m_data = new Dictionary<string, object>();

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.OP_BG_RECHARGE, user))
            return OpRes.op_res_no_right;

        ParamRecharge p = (ParamRecharge)param;
        bool res = false;
        int playerId = 0, rParam = 1;
        if (!int.TryParse(p.m_playerId, out playerId))
        {
            return OpRes.op_res_param_not_valid;
        }

        res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (!res)
            return OpRes.op_res_not_found_data;

        if (p.m_param != "")
        {
            if (!int.TryParse(p.m_param, out rParam))
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        if (rParam <= 0)
            return OpRes.op_res_param_not_valid;

        m_data.Clear();
        m_data.Add("playerId", playerId);
        m_data.Add("rtype", p.m_rtype);
        m_data.Add("param", rParam);
        res = DBMgr.getInstance().insertData(TableName.GM_RECHARGE, m_data, user.getDbServerID(), DbName.DB_PLAYER);

        if (res)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_GM_RECHARGE,
                new LogGmRecharge(playerId, p.m_rtype, rParam),
                user,
                p.m_comment);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamJPushAddApp
{
    public bool m_isAdd = true;
    public string m_platName = "";
    public string m_appName = "";
    public string m_appKey = "";
    public string m_apiSecret = "";

    public bool isValid()
    {
        return m_platName != "" && m_appName != "" && m_appKey != "" && m_apiSecret != "";
    }
}

// 增加一个极光应用
public class DyOpJPushAddApp : DyOpBase
{
    private Dictionary<string, object> m_data = new Dictionary<string, object>();

    public override OpRes doDyop(object param, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        ParamJPushAddApp p = (ParamJPushAddApp)param;
        if (p.m_isAdd)
        {
            if (!p.isValid())
                return OpRes.op_res_param_not_valid;

            m_data.Clear();
            m_data.Add("plat", p.m_platName);
            m_data.Add("appName", p.m_appName);
            m_data.Add("appKey", p.m_appKey);
            m_data.Add("apiSecret", p.m_apiSecret);
            bool res = DBMgr.getInstance().save(TableName.JPUSH_APP, m_data, "plat", p.m_platName, serverId, DbName.DB_ACCOUNT);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        string[] str = Tool.split(p.m_platName, ',');
        for (int i = 0; i < str.Length; i++)
        {
            DBMgr.getInstance().remove(TableName.JPUSH_APP, "plat", str[i], serverId, DbName.DB_ACCOUNT);
        }

        return OpRes.opres_success;
    }

    public void getAppList(GMUser user, List<ParamJPushAddApp> result)
    {
        int accServerId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (accServerId == -1)
            return;

        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.JPUSH_APP, accServerId, DbName.DB_ACCOUNT);
        for (int i = 0; i < data.Count; i++)
        {
            ParamJPushAddApp tmp = new ParamJPushAddApp();
            result.Add(tmp);
            tmp.m_platName = Convert.ToString(data[i]["plat"]);
            tmp.m_appName = Convert.ToString(data[i]["appName"]);
            tmp.m_appKey = Convert.ToString(data[i]["appKey"]);
            tmp.m_apiSecret = Convert.ToString(data[i]["apiSecret"]);
        }
    }
}

// 绑定手机
public class DyOpBindPhone : DyOpBase
{
    static string[] m_fields = { "phone", "acc" };

    public override OpRes doDyop(object param, GMUser user)
    {
        ParamModifyPwd p = (ParamModifyPwd)param;

        if (string.IsNullOrEmpty(p.m_phone))
            return OpRes.op_res_param_not_valid;

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.PLAYER_ACCOUNT, "acc", p.m_account, m_fields, serverId, DbName.DB_ACCOUNT);
        if (data == null)
            return OpRes.op_res_not_found_data;

        Dictionary<string, object> data1 = new Dictionary<string, object>();

        if (data.ContainsKey("phone")) // 已绑定
        {
            data1["phone"] = p.m_phone; // 更换手机号
        }
        else
        {
            DateTime now = DateTime.Now;
            data1["phone"] = p.m_phone;
            data1["searchTime"] = now;
            data1["searchCount"] = 0;
            data1["resetTime"] = calEndTime(now);
        }

        bool res = DBMgr.getInstance().update(TableName.PLAYER_ACCOUNT, data1, "acc", p.m_account, serverId, DbName.DB_ACCOUNT);
        if (!res)
            return OpRes.op_res_failed;

        return OpRes.opres_success;
    }

    // 取得截止时间
    public long calEndTime(DateTime now)
    {
        DateTime t = now.AddDays(1);
        DateTime e = new DateTime(t.Year, t.Month, t.Day, 0, 0, 0);
        return e.Ticks;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamGift
{
    // 添加还是修改
    public bool m_isAdd = true;
    // 礼包ID
    public string m_giftId;
    // 礼包道具列表
    public string m_itemList = "";
    // 截止日期
    public string m_deadTime = "";

    public string m_result = "";
}

public class GiftInfo
{
    public int m_giftId;
    public List<ParamItem> m_itemList = new List<ParamItem>();
    public DateTime m_deadTime;
}

// 礼包生成
public class DyOpGift : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        ParamGift p = (ParamGift)param;
        bool res = false;

        if (p.m_isAdd)
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;

            res = Tool.splitTimeStr(p.m_deadTime, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            if (mint < DateTime.Now)
                return OpRes.op_res_time_format_error;

            mint = mint.AddDays(1);
            mint = mint.AddSeconds(-1);
            List<ParamItem> itemList = new List<ParamItem>();
            res = Tool.parseItemList(p.m_itemList, itemList, false);
            if (!res)
            {
                return OpRes.op_res_param_not_valid;
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                var t = ItemCFG.getInstance().getValue(itemList[i].m_itemId);
                if (t == null)
                {
                    return OpRes.op_res_item_not_exist;
                }
            }

            long giftId = 0;

            giftId = CountMgr.getInstance().getCurId(CountMgr.GIFT_KEY);
            res = DBMgr.getInstance().keyExists(TableName.GIFT, "giftId", giftId, serverId, DbName.DB_ACCOUNT);
            if (res)
                return OpRes.op_res_data_duplicate;

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("giftId", giftId);
            data.Add("deadTime", mint);
            data.Add("item", ItemHelp.genItemBsonArray(itemList));
            res = DBMgr.getInstance().insertData(TableName.GIFT, data, serverId, DbName.DB_ACCOUNT);
            if (!res)
                return OpRes.op_res_failed;
        }
        else
        {
            bool isAdd = false;
            List<GiftInfo> giftList = new List<GiftInfo>();
            constructGiftList(giftList, p.m_itemList, p);

            for (int i = 0; i < giftList.Count; i++)
            {
                res = DBMgr.getInstance().keyExists(TableName.GIFT, "giftId", giftList[i].m_giftId, serverId, DbName.DB_ACCOUNT);
                if (!res)
                {
                    p.m_result += giftList[i].m_giftId + " ";
                    continue;
                }

                List<ParamItem> itemList = giftList[i].m_itemList;
                isAdd = true;

                for (int j = 0; j < itemList.Count; j++)
                {
                    var t = ItemCFG.getInstance().getValue(itemList[j].m_itemId);
                    if (t == null)
                    {
                        p.m_result += giftList[i].m_giftId + " ";
                        isAdd = false;
                        break;
                    }
                }

                if (isAdd)
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("deadTime", giftList[i].m_deadTime);
                    data.Add("item", ItemHelp.genItemBsonArray(itemList));
                    res = DBMgr.getInstance().update(TableName.GIFT, data, "giftId", giftList[i].m_giftId, serverId, DbName.DB_ACCOUNT);
                    if (!res)
                    {
                        p.m_result += giftList[i].m_giftId + " ";
                    }
                }
            }
        }

        return OpRes.opres_success;
    }

    private OpRes constructGiftList(List<GiftInfo> giftList, string str, ParamGift pres)
    {
        int giftId = 0;
        bool res = false;
        string[] group = Tool.split(str, '#', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < group.Length; i++)
        {
            string[] et = Tool.split(group[i], '@', StringSplitOptions.RemoveEmptyEntries);

            if (int.TryParse(et[0], out giftId))
            {
                GiftInfo info = new GiftInfo();
                info.m_giftId = giftId;

                res = Tool.parseItemList(et[1], info.m_itemList, false);
                if (!res)
                {
                    pres.m_result += giftId + " ";
                    continue;
                }

                DateTime mint = DateTime.Now, maxt = DateTime.Now;

                res = Tool.splitTimeStr(et[2], ref mint, ref maxt);
                if (!res)
                {
                    pres.m_result += giftId + " ";
                    continue;
                }

                if (mint < DateTime.Now)
                {
                    pres.m_result += giftId + " ";
                    continue;
                }

                mint = mint.AddDays(1);
                mint = mint.AddSeconds(-1);
                info.m_deadTime = mint;

                giftList.Add(info);
            }
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultPici
{
    public string m_count;
    public string m_pici;
    public string m_giftId;
    //public string m_validDays;
    public string m_deadTime;
    public string m_comment = "";
    public int m_type = 1;
}

public class ParamCDKEY : ResultPici
{
    public int m_op;
}

// CDKEY生成
public class DyOpGiftCode : DyOpBase
{
    List<Dictionary<string, object>> m_result = new List<Dictionary<string, object>>();

    public override OpRes doDyop(object param, GMUser user)
    {
        ParamCDKEY p = (ParamCDKEY)param;
        OpRes code = OpRes.op_res_failed;
        switch (p.m_op)
        {
            case 1: // 生成cdkey
                {
                    if (!RightMgr.getInstance().canEdit(RightDef.OTHER_CD_KEY, user))
                        return OpRes.op_res_no_right;
                    code = genCDKEY(p, user);
                }
                break;
            case 2: // 查询批次
                {
                    code = query(user);
                }
                break;
            case 6: // 启用，禁用状态
                {
                    if (!RightMgr.getInstance().canEdit(RightDef.OTHER_CD_KEY, user))
                        return OpRes.op_res_no_right;
                    code = enablePici(p, user);
                }
                break;

            case 7: //修改
                {
                    if (!RightMgr.getInstance().canEdit(RightDef.OTHER_CD_KEY, user))
                        return OpRes.op_res_no_right;
                    code = modifyPici(p, user);
                }
                break;
            case 8:
                {
                    if (!RightMgr.getInstance().canEdit(RightDef.OTHER_CD_KEY, user))
                        return OpRes.op_res_no_right;
                    code = flushToGameServer(p, user);
                }
                break;
            case 9:
                {
                    code = queryCDKEY(p, user);
                }
                break;
        }

        return code;
    }

    public override object getResult() { return m_result; }

    OpRes addPici(GMUser user, int count, int pici, int giftId, DateTime deadTime)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("genTime", DateTime.Now.Date);
        data.Add("pici", pici);
        data.Add("count", count);
        data.Add("giftId", giftId);
        data.Add("deadTime", deadTime);
        data.Add("enable", false);
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId < 0)
            return OpRes.op_res_failed;

        DBMgr.getInstance().insertData(TableName.CD_PICI, data, serverId, DbName.DB_ACCOUNT);
        return OpRes.opres_success;
    }

    OpRes genCDKEY(ParamCDKEY p, GMUser user)
    {
        int count = 0, pici = 0, giftId = 0;
        DateTime deadTime = DateTime.Now;

        if (!int.TryParse(p.m_count, out count))
            return OpRes.op_res_param_not_valid;
        if (count <= 0)
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(p.m_pici, out pici))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(p.m_giftId, out giftId))
            return OpRes.op_res_param_not_valid;

        bool res = Tool.splitTimeStr(p.m_deadTime, ref deadTime, 1);
        if (!res)
            return OpRes.op_res_time_format_error;

        ParamGenGiftCode pcode = new ParamGenGiftCode();
        pcode.m_count = count;
        pcode.m_pici = (int)CountMgr.getInstance().getCurId(CountMgr.GIFT_KEY, true) + 1;
        pcode.m_dbServerIP = WebConfigurationManager.AppSettings["account"];

        OpRes code = RemoteMgr.getInstance().reqGenGiftCode(pcode);
        if (code == OpRes.opres_success)
        {
            addPici(user, count, pici, giftId, deadTime);
        }
        return code;
    }

    OpRes query(GMUser user)
    {
        m_result.Clear();
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId < 0)
            return OpRes.op_res_failed;

        m_result = DBMgr.getInstance().executeQuery(TableName.CD_PICI, serverId, DbName.DB_ACCOUNT);
        if (m_result != null)
        {
            for (int i = 0; i < m_result.Count; i++)
            {
                Dictionary<string, object> tmp = m_result[i];
                string str = "";
                DateTime genTime = Convert.ToDateTime(tmp["genTime"]).ToLocalTime();
                str = genTime.ToShortDateString();
                tmp.Remove("genTime");
                tmp.Add("genTime", str);

                DateTime deadTime = Convert.ToDateTime(tmp["deadTime"]).ToLocalTime();
                str = deadTime.ToShortDateString();
                tmp.Remove("deadTime");
                tmp.Add("deadTime", str);
            }
        }
        return OpRes.opres_success;
    }

    OpRes enablePici(ParamCDKEY p, GMUser user)
    {
        int pici = 0;
        bool enable = false;

        if (!int.TryParse(p.m_pici, out pici))
            return OpRes.op_res_param_not_valid;

        if (!bool.TryParse(p.m_deadTime, out enable))
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("enable", enable);

        IMongoQuery imq = Query.EQ("pici", pici);

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId < 0)
            return OpRes.op_res_failed;

        DBMgr.getInstance().update(TableName.CD_PICI, data, imq, serverId, DbName.DB_ACCOUNT);
        return OpRes.opres_success;
    }

    OpRes modifyPici(ParamCDKEY p, GMUser user)
    {
        int pici = 0, giftId = 0;
        DateTime deadTime = DateTime.Now;

        if (!int.TryParse(p.m_pici, out pici))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(p.m_giftId, out giftId))
            return OpRes.op_res_param_not_valid;

        bool res = Tool.splitTimeStr(p.m_deadTime, ref deadTime, 1);
        if (!res)
            return OpRes.op_res_time_format_error;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("giftId", giftId);
        data.Add("deadTime", deadTime);

        IMongoQuery imq = Query.EQ("pici", pici);

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId < 0)
            return OpRes.op_res_failed;

        DBMgr.getInstance().update(TableName.CD_PICI, data, imq, serverId, DbName.DB_ACCOUNT);
        return OpRes.opres_success;
    }

    // 刷新到游戏服务器
    public OpRes flushToGameServer(ParamCDKEY p, GMUser user)
    {
        string fmt = string.Format("cmd=2&pici={0}", p.m_pici);

        string url = string.Format(DefCC.HTTP_MONITOR, fmt);
        var ret = HttpPost.Get(new Uri(url));
        if (ret != null)
        {
            string retStr = Encoding.UTF8.GetString(ret);
            if (retStr == "ok")
            {
                return OpRes.opres_success;
            }
        }
        return OpRes.op_res_failed;
    }

    OpRes queryCDKEY(ParamCDKEY p, GMUser user)
    {
        m_result.Clear();
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId < 0)
            return OpRes.op_res_failed;

        string cdkey = AESHelper.AESEncrypt(p.m_pici, ConstDef.AES_FOR_CDKEY);

        Dictionary<string, object> data =
            DBMgr.getInstance().getTableData(TableName.GIFT_CODE, "cdkey", cdkey, serverId, DbName.DB_ACCOUNT);
        if (data != null)
        {
            m_result.Add(data);
            return OpRes.opres_success;
        }

        return OpRes.op_res_not_found_data;
    }
}

//////////////////////////////////////////////////////////////////////////

// 兑换写入
public class DyOpExchange : DyOpBase
{
    private string[] m_playerFields = { "itemName", "phone", "genTime", "playerId", "itemId", "chgId"};
    private Dictionary<int, int> m_items = new Dictionary<int, int>(){{1002, 30}, {1003, 50}, {1004, 100}, {1025, 50}};
  
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.SVR_EXCHANGE_MGR, user))
            return OpRes.op_res_no_right;

        string p = (string)param;
        if (p == "")
            return OpRes.op_res_failed;

        Dictionary<string, object> data = new Dictionary<string, object>();
        Dictionary<string, object> data1 = new Dictionary<string, object>();
        //data["isReceive"] = true;
        data1["status"] = 4;
        data1["giveOutTime"] = DateTime.Now;

        string[] arr = Tool.split(p, ',', StringSplitOptions.RemoveEmptyEntries);
        int i =0, money = 0, money_n = 0;
        for (i = 0; i < arr.Length; i++)
        {
            money_n = 0;
            DBMgr.getInstance().update(TableName.EXCHANGE, data1, "exchangeId", arr[i], user.getDbServerID(), DbName.DB_PLAYER);

            Dictionary<string, object> dataList = DBMgr.getInstance().getTableData(TableName.EXCHANGE, "exchangeId", arr[i], m_playerFields, user.getDbServerID(), DbName.DB_PLAYER);

            ////////////////////////////////////////更新历史发放金额表
            if (dataList.ContainsKey("itemId"))
            {
                int itemId = Convert.ToInt32(dataList["itemId"]);
                if (m_items.ContainsKey(itemId)) //发放的是实物
                {
                    money = m_items[itemId]; //本次发放金额
                    money_n = money;

                    int playerId = Convert.ToInt32(dataList["playerId"]);
                    
                    //查询历史发放金额
                    Dictionary<string, object> playerMoney = 
                        DBMgr.getInstance().getTableData(TableName.STAT_PLAYER_EXCHANGE, "playerId", playerId, null, user.getDbServerID(), DbName.DB_PUMP);
                    data.Clear();
                    if (playerMoney != null && playerMoney.Count != 0) //存在该玩家
                    {
                        if (playerMoney.ContainsKey("historyMoney"))
                            money += Convert.ToInt32(playerMoney["historyMoney"]);

                        data.Add("historyMoney", money);
                        DBMgr.getInstance().update(TableName.STAT_PLAYER_EXCHANGE, data, "playerId", playerId, user.getDbServerID(), DbName.DB_PUMP);
                    }
                    else 
                    {
                        data.Add("playerId", playerId);
                        data.Add("historyMoney", money);
                        DBMgr.getInstance().insertData(TableName.STAT_PLAYER_EXCHANGE, data, user.getDbServerID(), DbName.DB_PUMP);
                    }
                }
            }
            ////////////////////////////////////////////////

            bool flag = true; //为true发送邮件，false不发邮件（千元京东卡或百元京东卡）
            if (Convert.ToString(dataList["itemName"]).Contains("京东卡"))
            {
                flag = false;
            }
            if (flag)
            {
                ParamSendMail param_mail = new ParamSendMail();
                param_mail.m_title = "彩券兑换成功";
                param_mail.m_sender = "系统";
                param_mail.m_content = "您于" + dataList["genTime"] + "申请的兑换申请已经成功。" + dataList["itemName"] + "被充值入指定手机号" + dataList["phone"] + "。请您稍后查收。";
                param_mail.m_toPlayer = dataList["playerId"].ToString();
                param_mail.m_target = 0;
                DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
                OpRes res = mgr.doDyop(param_mail, DyOpType.opTypeSendMail, user);
            }

            ////////////   log表  ///////////////////
            data.Clear();

            data.Add("genTime", DateTime.Now);
            data.Add("playerId", Convert.ToInt32(dataList["playerId"]));
            data.Add("exchangeId", arr[i]);
            data.Add("money", money_n); //本次发放金额

            if (dataList.ContainsKey("chgId"))
                data.Add("chgId", Convert.ToInt32(dataList["chgId"]));

            if (dataList.ContainsKey("itemId"))
                data.Add("itemId", Convert.ToInt32(dataList["itemId"]));

            DBMgr.getInstance().insertData(TableName.PUMP_PLAYER_EXCHANGE, data, user.getDbServerID(), DbName.DB_PUMP);
            /////////////////////////////////////////////////////
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public enum NoticeOpType
{
    add,
    del,
    modify,
}

public class ParamNotify
{
    public int m_op;

    public string m_title = "";

    public string m_content = "";

    public string m_day = "";

    public NoticeOpType m_opType;

    public string m_id = "";

    public string m_comment = "";

    public string m_order = "";

    public string m_startTime = "";

    public string m_endTime = "";
}

// 通告
public class DyOpNotify : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.OP_OPERATION_NOTICE, user))
            return OpRes.op_res_no_right;

        ParamNotify p = (ParamNotify)param;

        if (p.m_opType == NoticeOpType.del)
        {
            string[] str = Tool.split(p.m_id, ',');
            for (int i = 0; i < str.Length; i++)
            {
                p.m_id = str[i];
                delNotice(p, user);
            }
            return OpRes.opres_success;
        }

        if (p.m_id != "")
        {
            return updateNotice(p, user);
        }
        return addNotice(p, user);
    }

    private OpRes addNotice(ParamNotify p, GMUser user)
    {
        if (string.IsNullOrEmpty(p.m_title) || string.IsNullOrEmpty(p.m_content))
            return OpRes.op_res_param_not_valid;

        int day = 0;
        if (!int.TryParse(p.m_day, out day))
        {
            return OpRes.op_res_param_not_valid;
        }
        if (day <= 0)
            return OpRes.op_res_param_not_valid;

        int order = 0;
        if (p.m_order != "" && !int.TryParse(p.m_order, out order))
            return OpRes.op_res_param_not_valid;

        DateTime now = DateTime.Now;
        DateTime nt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("genTime", nt);
        data.Add("title", p.m_title);
        data.Add("content", p.m_content);
        data.Add("deadTime", nt.AddDays(day));
        data.Add("noticeId", Guid.NewGuid().ToString());
        data.Add("comment", p.m_comment);
        data.Add("order", order);

        bool res = DBMgr.getInstance().insertData(TableName.OPERATION_NOTIFY, data, user.getDbServerID(), DbName.DB_PLAYER);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    private OpRes updateNotice(ParamNotify p, GMUser user)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        Dictionary<string, object> retData = DBMgr.getInstance().getTableData(TableName.OPERATION_NOTIFY,
            "noticeId", p.m_id, new string[] { "genTime" }, user.getDbServerID(), DbName.DB_PLAYER);

        if (retData == null)
            return OpRes.op_res_not_found_data;

        if (!string.IsNullOrEmpty(p.m_day))
        {
            int day = 0;
            if (!int.TryParse(p.m_day, out day))
            {
                return OpRes.op_res_param_not_valid;
            }
            if (day <= 0)
                return OpRes.op_res_param_not_valid;

            DateTime nt = Convert.ToDateTime(retData["genTime"]).ToLocalTime();
            data.Add("deadTime", nt.AddDays(day));
        }

        if (!string.IsNullOrEmpty(p.m_title))
        {
            data.Add("title", p.m_title);
        }
        if (!string.IsNullOrEmpty(p.m_content))
        {
            data.Add("content", p.m_content);
        }
        if (!string.IsNullOrEmpty(p.m_comment))
        {
            data.Add("comment", p.m_comment);
        }
        if (!string.IsNullOrEmpty(p.m_order))
        {
            int order = 0;
            if (!int.TryParse(p.m_order, out order))
                return OpRes.op_res_param_not_valid;

            data.Add("order", order);
        }
        bool res = DBMgr.getInstance().update(TableName.OPERATION_NOTIFY,
            data, "noticeId", p.m_id, user.getDbServerID(), DbName.DB_PLAYER);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    private OpRes delNotice(ParamNotify p, GMUser user)
    {
        bool res = DBMgr.getInstance().remove(TableName.OPERATION_NOTIFY, "noticeId", p.m_id,
            user.getDbServerID(), DbName.DB_PLAYER);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
//运营公告（新）
public class DyOpNotifyNew : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.OP_OPERATION_NOTICE, user))
            return OpRes.op_res_no_right;
        ParamNotify p = (ParamNotify)param;
        if (p.m_opType == NoticeOpType.del)
        {
            string[] str = Tool.split(p.m_id, ',');
            for (int i = 0; i < str.Length; i++)
            {
                p.m_id = str[i];
                delNotice(p, user);
            }
            return OpRes.opres_success;
        }
        return addOrUpdateNotice(p, user);
    }

    private OpRes addOrUpdateNotice(ParamNotify p, GMUser user)
    {
        bool res = false;
        /////////////////////////
        if (string.IsNullOrEmpty(p.m_title) || string.IsNullOrEmpty(p.m_content))
            return OpRes.op_res_param_not_valid;
        /////////////////////
        int order = 0;
        if (p.m_order != "" && !int.TryParse(p.m_order, out order))
            return OpRes.op_res_param_not_valid;
        ////////////////////////
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        //开始日期
        res = Tool.splitTimeStr(p.m_startTime, ref mint);
        if (!res)
            return OpRes.op_res_time_format_error;

        //结束日期
        res = Tool.splitTimeStr(p.m_endTime, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        if (p.m_endTime.IndexOf(':') == -1) //无时分
        {
            maxt = maxt.AddDays(1);
        }
        ///////////////////////
        DateTime now = DateTime.Now;
        DateTime nt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

        Dictionary<string, object> data = new Dictionary<string, object>();
        //转回到原来的 System.String。
        byte[] d = Convert.FromBase64String(p.m_content);
        string content = System.Text.Encoding.UTF8.GetString(d);

        data.Add("genTime", nt);
        data.Add("title", p.m_title);
        data.Add("content", content);
        if (p.m_id == "")
        {
            data.Add("noticeId", Guid.NewGuid().ToString());
        }
        data.Add("comment", p.m_comment);
        data.Add("order", order);
        data.Add("startTime", mint);
        data.Add("deadTime", maxt);
        if (p.m_id != "")
        {
            res = DBMgr.getInstance().update(TableName.OPERATION_NOTIFY, data, "noticeId", p.m_id, user.getDbServerID(), DbName.DB_PLAYER);
        }
        else
        {
            res = DBMgr.getInstance().insertData(TableName.OPERATION_NOTIFY, data, user.getDbServerID(), DbName.DB_PLAYER);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    private OpRes delNotice(ParamNotify p, GMUser user)
    {
        bool res = DBMgr.getInstance().remove(TableName.OPERATION_NOTIFY, "noticeId", p.m_id,
            user.getDbServerID(), DbName.DB_PLAYER);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

}
//////////////////////////////////////////////////////////////////////////

public class ParamSpeaker
{
    // 要显示的消息
    public string m_content = "";

    // 发出去的时间
    public string m_sendTime = "";

    // 重复时间
    public string m_repCount = "";

    // 发送间隔
    public string m_interval = "";
}

// 运营发布的通告消息，会显示到通告栏
public class DyOpSpeaker : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.OP_NOTIFY_MSG, user))
            return OpRes.op_res_no_right;

        ParamSpeaker p = (ParamSpeaker)param;
        if (string.IsNullOrEmpty(p.m_content))
            return OpRes.op_res_param_not_valid;

        bool res = false;
        DateTime sendTime = DateTime.MinValue;
        if (!string.IsNullOrEmpty(p.m_sendTime))
        {
            res = Tool.splitTimeStr(p.m_sendTime, ref sendTime, 3);
            if (!res)
                return OpRes.op_res_time_format_error;
        }

        int repCount = 1;
        if (!string.IsNullOrEmpty(p.m_repCount))
        {
            res = int.TryParse(p.m_repCount, out repCount);
            if (!res)
                return OpRes.op_res_param_not_valid;
        }

        int interval = 1;
        if (!string.IsNullOrEmpty(p.m_interval))
        {
            res = int.TryParse(p.m_interval, out interval);
            if (!res)
                return OpRes.op_res_param_not_valid;
        }

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("genTime", DateTime.Now);
        data.Add("content", p.m_content);
        data.Add("sendTime", sendTime);
        data.Add("repCount", repCount);
        data.Add("interval", interval);
        res = DBMgr.getInstance().insertData(TableName.OPERATION_SPEAKER, data, user.getDbServerID(), DbName.DB_PLAYER);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamMaintenance
{
    // 0取得当前信息, 1确定维护, 2撤消维护
    public int m_opType;

    // 内容
    public string m_content = "";
}

// 当前信息
public class ResultMaintenance
{
    // 0运行中，1维护中, 2未知
    public int m_curState;
    // 当前的维护信息
    public string m_info = "";
}

// 运营维护
public class DyOpMaintenance : DyOpBase
{
    private ResultMaintenance m_result = new ResultMaintenance();

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.OP_MAINTENANCE_NOTICE, user))
            return OpRes.op_res_no_right;

        ParamMaintenance p = (ParamMaintenance)param;
        if (p.m_opType == 0) // 取得当前信息
        {
            return fetchCurState();
        }
        if (p.m_opType == 1) // 确定维护
        {
            return doMaintenance("false", p.m_content);
        }
        if (p.m_opType == 2) // 撤消维护
        {
            return doMaintenance("true", p.m_content);
        }
        return OpRes.op_res_failed;
    }

    public override object getResult()
    {
        return m_result;
    }

    private OpRes fetchCurState()
    {
        string fmt = WebConfigurationManager.AppSettings["maintenaceWeb"];
        string aspx = string.Format(fmt, "", "");
        var ret = HttpPost.Get(new Uri(aspx));
        if (ret != null)
        {
            string retStr = Encoding.UTF8.GetString(ret);
            Dictionary<string, object> data = parseString(retStr);
            if (data != null)
            {
                m_result.m_info = Convert.ToString(data["info"]);
                string state = Convert.ToString(data["state"]);
                if (state == "true")
                {
                    m_result.m_curState = 0;
                }
                else
                {
                    m_result.m_curState = 1;
                }
                return OpRes.opres_success;
            }
        }
        else
        {
            m_result.m_curState = 2;
        }
        return OpRes.op_res_failed;
    }

    private Dictionary<string, object> parseString(string str)
    {
        byte[] arr = Convert.FromBase64String(str);
        string dst = Encoding.Default.GetString(arr);
        Dictionary<string, object> data = JsonHelper.ParseFromStr<Dictionary<string, object>>(dst);
        return data;
    }

    // 开始维护
    private OpRes doMaintenance(string state, string info)
    {
        string fmt = WebConfigurationManager.AppSettings["maintenaceWeb"];
        string aspx = string.Format(fmt, state, info);
        try
        {
            var ret = HttpPost.Post(new Uri(aspx), null);
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr == "0")
                    return OpRes.opres_success;
            }
        }
        catch (System.Exception ex)
        {
        }

        return OpRes.op_res_failed;
    }
}

/// //////////////////////////////////////////////////////////////////////
public class paramActivityPanicBuyingCfgParamAdjust
{
    //活动列表
    public string m_activityList = "";
    //最大次数
    public string m_maxCount = "";
    public string m_rightId;
}

public class DyOpActivityPanicBuyingCfgParamAdjust : DyOpBase
{
    // 继承类需要赋值
    protected int m_activityId;

    protected string m_activityTableName = "";

    public override OpRes doDyop(object param, GMUser user)
    {
        paramActivityPanicBuyingCfgParamAdjust p = (paramActivityPanicBuyingCfgParamAdjust)param;
        if (!RightMgr.getInstance().canEdit(p.m_rightId, user))
            return OpRes.op_res_no_right;

        if (string.IsNullOrEmpty(p.m_activityList))
            return OpRes.op_res_param_not_valid;

        return modifyMaxCount(user, p);
    }
    // 修改最大次数
    protected OpRes modifyMaxCount(GMUser user, paramActivityPanicBuyingCfgParamAdjust p)
    {
        bool res = false;
        int maxCount = 0;
        if (!int.TryParse(p.m_maxCount, out maxCount))
            return OpRes.op_res_param_not_valid;
        if (maxCount <= 0)
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] activities = Tool.split(p.m_activityList, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < activities.Length; i++)
        {
            data.Clear();
            int activityId = Convert.ToInt32(activities[i]);
            IMongoQuery imq = Query.EQ("actId", activityId);
            data.Add("actId", activityId);
            data.Add("maxCount", maxCount);
            res = DBMgr.getInstance().update(TableName.PB_ACTIVITY_CFG, data, imq, user.getDbServerID(), DbName.DB_PLAYER, true);
            if (!res) { break; }
        }

        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_ACTIVITY_CFG, new LogModifyActivityPanicBuyingCfg(p.m_activityList, maxCount, m_activityId), user);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

/// //////////////////////////////////////////////////////////////////////

public class ParamFishlordParamAdjust
{
    // 是否重置
    public bool m_isReset;

    // 期望盈利率
    public string m_expRate = "";

    // 房间列表
    public string m_roomList = "";

    public GameId m_gameId;

    // 操作模式 0 修改期望盈利率 1 修改难度(对黑红梅方有用) 2 修改大小王个数(对黑红梅方有用) 4 修改最大阀值
    public int m_op;

    public string m_rightId;
}

public class DyOpParamAdjust : DyOpBase
{
    protected static string[] s_fields = new string[] { "room_income", "room_outcome", "ExpectEarnRate" };

    // 继承类需要赋值
    protected int m_gameId;

    protected string m_roomTableName = "";

    public DyOpParamAdjust(int gameId, string roomTableName)
    {
        m_gameId = gameId;
        m_roomTableName = roomTableName;
    }

    public override OpRes doDyop(object param, GMUser user)
    {
        ParamFishlordParamAdjust p = (ParamFishlordParamAdjust)param;
        if (!RightMgr.getInstance().canEdit(p.m_rightId, user))
            return OpRes.op_res_no_right;

        if (string.IsNullOrEmpty(p.m_roomList))
            return OpRes.op_res_param_not_valid;

        if (p.m_isReset)
        {
            return resetExp(user, p);
        }
        return modifyExp(user, p);
    }

    protected virtual OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        return OpRes.op_res_failed;
    }

    protected virtual OpRes resetExp(GMUser user, ParamFishlordParamAdjust p)
    {
        bool res = false;
        DateTime now = DateTime.Now;
        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < rooms.Length; i++)
        {
            data.Clear();
            data.Add("room_income", -1L);
            data.Add("room_outcome", -1L);
            int roomId = Convert.ToInt32(rooms[i]);
            addOldEarningsRate(user, roomId, now);
            res = DBMgr.getInstance().update(m_roomTableName, data, "room_id", roomId,
                    user.getDbServerID(), DbName.DB_GAME);
            if (!res)
            {
                break;
            }
        }

        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_RESET_FISHLORD_GAIN_RATE,
            new LogResetFishlordRoomExpRate(p.m_roomList, m_gameId), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    // 增加旧的盈利率
    protected OpRes addOldEarningsRate(GMUser user, int roomId, DateTime now)
    {
        ResultExpRateParam oldParam = getOldParam(roomId, user);
        if (oldParam == null)
            return OpRes.op_res_failed;

        Dictionary<string, object> old = new Dictionary<string, object>();
        old.Add("gameId", m_gameId);
        old.Add("roomId", roomId);
        old.Add("time", now);
        old.Add("income", oldParam.m_totalIncome);
        old.Add("outlay", oldParam.m_totalOutlay);
        old.Add("expRate", oldParam.m_expRate);
        DBMgr.getInstance().insertData(TableName.PUMP_OLD_EARNINGS_RATE, old, user.getDbServerID(), DbName.DB_PUMP);
        return OpRes.opres_success;
    }

    // 返回旧的参数
    protected virtual ResultExpRateParam getOldParam(int roomId, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(m_roomTableName, "room_id", roomId, s_fields, dip);

        if (data == null)
            return null;

        ResultExpRateParam param = new ResultExpRateParam();

        if (data.ContainsKey("room_income"))
        {
            param.m_totalIncome = Convert.ToInt64(data["room_income"]);
        }
        if (data.ContainsKey("room_outcome"))
        {
            param.m_totalOutlay = Convert.ToInt64(data["room_outcome"]);
        }
        if (data.ContainsKey("ExpectEarnRate"))
        {
            param.m_expRate = Convert.ToDouble(data["ExpectEarnRate"]);
        }
        return param;
    }

    // 修改盈利率
    // expRateFieldName 期望盈利率字段名  roomIdFieldName 房间ID字段名称
    protected OpRes modifyExpImp(GMUser user,
                                ParamFishlordParamAdjust p,
                                string expRateFieldName,
                                string roomIdFieldName = "room_id")
    {
        double expRate = 0.0;
        if (!double.TryParse(p.m_expRate, out expRate))
            return OpRes.op_res_param_not_valid;
        if (expRate <= 0.0)
            return OpRes.op_res_param_not_valid;

        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < rooms.Length; i++)
        {
            data.Clear();
            data.Add(expRateFieldName, expRate);
            int roomId = Convert.ToInt32(rooms[i]);
            bool res = DBMgr.getInstance().update(m_roomTableName, data, roomIdFieldName, roomId,
                    user.getDbServerID(), DbName.DB_GAME);
            if (!res)
                return OpRes.op_res_failed;
        }

        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_MODIFY_FISHLORD_GAIN_RATE,
            new LogModifyFishlordRoomExpRate(p.m_roomList, expRate, m_gameId), user);

        return OpRes.opres_success;
    }

    protected OpRes resetExpImp(GMUser user, ParamFishlordParamAdjust p, string incomeFieldName,
        string outcomeFieldName, string roomFieldName)
    {
        bool res = false;
        DateTime now = DateTime.Now;
        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < rooms.Length; i++)
        {
            data.Clear();
            data.Add(incomeFieldName, -1L);   // 字段 room_income
            data.Add(outcomeFieldName, -1L);  // 字段 room_outcome
            int roomId = Convert.ToInt32(rooms[i]);
            addOldEarningsRate(user, roomId, now);
            // 字段room_id
            res = DBMgr.getInstance().update(m_roomTableName, data, roomFieldName, roomId,
                    user.getDbServerID(), DbName.DB_GAME);
            if (!res)
            {
                break;
            }
        }

        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_RESET_FISHLORD_GAIN_RATE,
            new LogResetFishlordRoomExpRate(p.m_roomList, m_gameId), user);

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    protected ResultExpRateParam getOldParamImp(GMUser user, int roomId,
                                                string roomFieldName,
                                                string incomeFieldName,
                                                string outcomeFieldName,
                                                string expectEarnRateFieldName)
    {
        // room_id 字段名
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(m_roomTableName, roomFieldName, roomId, null, dip);

        if (data == null)
            return null;

        ResultExpRateParam param = new ResultExpRateParam();

        if (data.ContainsKey(incomeFieldName)) // room_income
        {
            param.m_totalIncome = Convert.ToInt64(data[incomeFieldName]);
        }
        if (data.ContainsKey(outcomeFieldName)) //  room_outcome
        {
            param.m_totalOutlay = Convert.ToInt64(data[outcomeFieldName]);
        }
        if (data.ContainsKey(expectEarnRateFieldName)) // ExpectEarnRate
        {
            param.m_expRate = Convert.ToDouble(data[expectEarnRateFieldName]);
        }
        return param;
    }
}

public class ParamFishlordNew : ParamFishlordParamAdjust
{
    public bool m_isNewAlg = false; // 新的算法调整
    public string m_minEarnValue;
    public string m_maxEarnValue;
    public string m_startEarnValue;
    public string m_minControlEarnValue;
    public string m_maxControlEarnValue;
}

public class ParamFishlord5 : ParamFishlordParamAdjust
{
    public bool m_isNewAlg = false;
    public string m_jsckpotGrandPump;
    public string m_jsckpotSmallPump;
    public string m_normalFishRoomPoolPumpParam;

    public string m_percentCtrl5;
    public string m_percentCtrl20;
    public string m_percentCtrl60;

    public string m_baseRate;
    public string m_deviationFix;
    public string m_checkRate;
    public string m_trickDeviationFix;

    public string m_incomeThreshold;
    public string m_earnRatemCtrMax;
    public string m_earnRatemCtrMin;

    public string m_legendaryFishRate;

    public string m_mythicalScoreTurnRate;
    public string m_mythicalFishRate;
}

// 捕鱼参数调整
public class DyOpFishlordParamAdjust : DyOpParamAdjust
{
    private static string[] s_fishlordFields = new string[] { "TotalIncome", "TotalOutlay", "EarningsRate" };

    public DyOpFishlordParamAdjust()
        : base((int)GameId.fishlord, TableName.FISHLORD_ROOM)
    {

    }

    public DyOpFishlordParamAdjust(int gameId, string roomTableName)
        : base(gameId, roomTableName)
    {
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        ParamFishlordNew param = (ParamFishlordNew)p;
        if (param.m_isNewAlg)
        {
            ParamInner result = new ParamInner();
            OpRes code = calParamInner(param, result);
            if (code == OpRes.opres_success)
            {
                Dictionary<string, object> updata = new Dictionary<string, object>();
                updata.Add("WinRateAverage", result.m_expEarn);
                updata.Add("WinRateMax", result.m_maxEarnValue);
                updata.Add("WinRateMin", result.m_minEarnValue);
                updata.Add("WinRateCtrValue", result.m_startEarnValue);
                updata.Add("WinRateControlMin",result.m_minCtrEarnValue);
                updata.Add("WinRateControlMax",result.m_maxCtrEarnValue);
                string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < rooms.Length; i++)
                {
                    int roomId = Convert.ToInt32(rooms[i]);
                    bool res = DBMgr.getInstance().update(TableName.FISHLORD_ROOM, updata, "room_id", roomId,
                            user.getDbServerID(), DbName.DB_GAME);
                    if (!res)
                        return OpRes.op_res_failed;
                }
            }

            return code;
        }

        return modifyExpImp(user, p, "EarningsRate");
    }

    protected override OpRes resetExp(GMUser user, ParamFishlordParamAdjust p)
    {
        DateTime now = DateTime.Now;
        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < rooms.Length; i++)
        {
            data.Clear();
            data.Add("TotalIncome", -1L);
            data.Add("TotalOutlay", -1L);
            data.Add("Abandonedbullets", 0L);
            data.Add("MissileCount", 0L);

            int roomId = Convert.ToInt32(rooms[i]);
            addOldEarningsRate(user, roomId, now);
            DBMgr.getInstance().update(m_roomTableName, data, "room_id", roomId,
                user.getDbServerID(), DbName.DB_GAME);
        }

        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_RESET_FISHLORD_GAIN_RATE,
            new LogResetFishlordRoomExpRate(p.m_roomList, m_gameId), user);

        return OpRes.opres_success;
    }

    // 返回旧的参数
    protected override ResultExpRateParam getOldParam(int roomId, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(m_roomTableName, "room_id", roomId, s_fishlordFields, dip);

        if (data == null)
            return null;

        ResultExpRateParam param = new ResultExpRateParam();

        if (data.ContainsKey("TotalIncome"))
        {
            param.m_totalIncome = Convert.ToInt64(data["TotalIncome"]);
        }
        if (data.ContainsKey("TotalOutlay"))
        {
            param.m_totalOutlay = Convert.ToInt64(data["TotalOutlay"]);
        }
        if (data.ContainsKey("EarningsRate"))
        {
            param.m_expRate = Convert.ToDouble(data["EarningsRate"]);
        }
        return param;
    }

    class ParamInner
    {
        public int m_minEarnValue;
        public int m_maxEarnValue;
        public int m_startEarnValue; // 非负
        public int m_expEarn; // 介于最小与最大之间
        public int m_minCtrEarnValue;
        public int m_maxCtrEarnValue;
    }

    OpRes calParamInner(ParamFishlordNew param, ParamInner result)
    {
        if (!int.TryParse(param.m_expRate, out result.m_expEarn))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(param.m_minEarnValue, out result.m_minEarnValue))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(param.m_maxEarnValue, out result.m_maxEarnValue))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(param.m_minControlEarnValue, out result.m_minCtrEarnValue))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(param.m_maxControlEarnValue, out result.m_maxCtrEarnValue))
            return OpRes.op_res_param_not_valid;

        if (!int.TryParse(param.m_startEarnValue, out result.m_startEarnValue))
            return OpRes.op_res_param_not_valid;

        if (result.m_startEarnValue < 0)
            return OpRes.op_res_param_not_valid;

        if (result.m_minEarnValue > result.m_maxEarnValue)
            return OpRes.op_res_param_not_valid;

        if (result.m_expEarn < result.m_minEarnValue || result.m_expEarn > result.m_maxEarnValue)
            return OpRes.op_res_param_not_valid;

        if ((result.m_maxEarnValue > result.m_maxCtrEarnValue) &&
            (result.m_maxCtrEarnValue > result.m_expEarn) &&
            (result.m_expEarn > result.m_minCtrEarnValue) &&
            (result.m_minCtrEarnValue > result.m_minEarnValue)){
            return OpRes.opres_success;
        }else 
        {
            return OpRes.op_res_param_not_valid;
        }
    }
}

//捕鱼参数调整（新）
public class DyOpFishlordParamAdjustNew : DyOpParamAdjust
{
    private static string[] s_fishlordFields = new string[] { "TotalIncome", "TotalOutlay", "EarningsRate" };

    public DyOpFishlordParamAdjustNew()
        : base((int)GameId.fishlord, TableName.FISHLORD_ROOM)
    {
    }

    public DyOpFishlordParamAdjustNew(int gameId, string roomTableName)
        : base(gameId, roomTableName)
    {
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        ParamFishlord5 param = (ParamFishlord5)p;
        if (param.m_isNewAlg)
        {
            ParamInner result = new ParamInner();
            OpRes code = calParamInner(param, result);
            if (code == OpRes.opres_success)
            {
                Dictionary<string, object> updata = new Dictionary<string, object>();
                updata.Add("JsckpotGrandPump", result.m_jsckpotGrandPump/100.0);
                updata.Add("JsckpotSmallPump", result.m_jsckpotSmallPump/100.0);
                updata.Add("NormalFishRoomPoolPumpParam", result.m_normalFishRoomPoolPumpParam/100.0);
                updata.Add("RateCtr", result.m_baseRate);
                updata.Add("CheckRate", result.m_checkRate);
                updata.Add("TrickDeviationFix", result.m_trickDeviationFix);
                updata.Add("IncomeThreshold", result.m_incomeThreshold);
                updata.Add("EarnRatemCtrMax", result.m_earnRatemCtrMax);
                updata.Add("EarnRatemCtrMin", result.m_earnRatemCtrMin);

                if (result.m_roomId == 8)
                {
                    updata.Add("LegendaryFishRate", result.m_legendaryFishRate / 1000.0);
                }

                if (result.m_roomId == 9)
                {
                    updata.Add("MythicalScoreTurnRate", result.m_mythicalScoreTurnRate);
                    updata.Add("MythicalFishRate", result.m_mythicalFishRate);
                }

                int roomId = Convert.ToInt32(param.m_roomList);
                bool res = DBMgr.getInstance().update(TableName.FISHLORD_ROOM, updata, "room_id", roomId,
                        user.getDbServerID(), DbName.DB_GAME);

                if(!res)
                    return OpRes.op_res_failed;

                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_MODIFY_FISHLORD_CTRL_PARAM,
                        new LogModifyFishlordCtrlNewParam(roomId, result.m_jsckpotGrandPump, result.m_jsckpotSmallPump, result.m_normalFishRoomPoolPumpParam, result.m_baseRate,
                            result.m_checkRate, result.m_trickDeviationFix, result.m_incomeThreshold, result.m_earnRatemCtrMax,
                            result.m_earnRatemCtrMin,result.m_legendaryFishRate, result.m_mythicalScoreTurnRate, result.m_mythicalFishRate), user);
            }

            return code;
        }

        return modifyExpImp(user, p, "EarningsRate");
    }

    protected override OpRes resetExp(GMUser user, ParamFishlordParamAdjust p)
    {
        DateTime now = DateTime.Now;
        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < rooms.Length; i++)
        {
            data.Clear();
            data.Add("ClearData", true);
            int roomId = Convert.ToInt32(rooms[i]);
            addOldEarningsRate(user, roomId, now);
            DBMgr.getInstance().update(m_roomTableName, data, "room_id", roomId,
                user.getDbServerID(), DbName.DB_GAME);
        }

        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_RESET_FISHLORD_GAIN_RATE,
            new LogResetFishlordRoomExpRate(p.m_roomList, m_gameId), user);

        return OpRes.opres_success;
    }

    // 返回旧的参数
    protected override ResultExpRateParam getOldParam(int roomId, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(m_roomTableName, "room_id", roomId, s_fishlordFields, dip);

        if (data == null)
            return null;

        ResultExpRateParam param = new ResultExpRateParam();

        if (data.ContainsKey("TotalIncome"))
        {
            param.m_totalIncome = Convert.ToInt64(data["TotalIncome"]);
        }
        if (data.ContainsKey("TotalOutlay"))
        {
            param.m_totalOutlay = Convert.ToInt64(data["TotalOutlay"]);
        }
        if (data.ContainsKey("EarningsRate"))
        {
            param.m_expRate = Convert.ToDouble(data["EarningsRate"]);
        }
        return param;
    }

    class ParamInner
    {
        public int m_roomId;
        public int m_jsckpotGrandPump;
        public int m_jsckpotSmallPump;
        public int m_normalFishRoomPoolPumpParam;

        public double m_baseRate;
        public double m_checkRate;
        public double m_trickDeviationFix;

        public long m_incomeThreshold;
        public double m_earnRatemCtrMax;
        public double m_earnRatemCtrMin;

        public int m_legendaryFishRate;

        public int m_mythicalScoreTurnRate;
        public int m_mythicalFishRate;
    }

    OpRes calParamInner(ParamFishlord5 param, ParamInner result)
    {
        result.m_roomId = Convert.ToInt32(param.m_roomList);
        if (result.m_roomId == 3)
        {
            if (!int.TryParse(param.m_jsckpotGrandPump, out result.m_jsckpotGrandPump))
                return OpRes.op_res_param_not_valid;

            if (!int.TryParse(param.m_jsckpotSmallPump, out result.m_jsckpotSmallPump))
                return OpRes.op_res_param_not_valid;

            if (!int.TryParse(param.m_normalFishRoomPoolPumpParam, out result.m_normalFishRoomPoolPumpParam))
                return OpRes.op_res_param_not_valid;

            if (result.m_jsckpotGrandPump > 100 || result.m_jsckpotGrandPump < 0 ||
                result.m_jsckpotSmallPump >100 || result.m_jsckpotSmallPump < 0 ||
                result.m_normalFishRoomPoolPumpParam > 100 || result.m_normalFishRoomPoolPumpParam < 0 )
                return OpRes.op_res_param_not_valid;
        }

        if (result.m_roomId == 8) 
        {
            if(!int.TryParse(param.m_legendaryFishRate, out result.m_legendaryFishRate))
                return OpRes.op_res_param_not_valid;
        }

        ///////////////////////////////////
        if (result.m_roomId == 9) 
        {
            if (!int.TryParse(param.m_mythicalScoreTurnRate, out result.m_mythicalScoreTurnRate))
                return OpRes.op_res_param_not_valid;

            if (!int.TryParse(param.m_mythicalFishRate, out result.m_mythicalFishRate))
                return OpRes.op_res_param_not_valid;
        }

        //////////////////////////////
        if (!double.TryParse(param.m_baseRate, out result.m_baseRate))
            return OpRes.op_res_param_not_valid;

        if (result.m_baseRate > 2.0 || result.m_baseRate < 0.1)
            return OpRes.op_res_param_not_valid;
        ///////////////////////////////

        if (!double.TryParse(param.m_checkRate, out result.m_checkRate))
            return OpRes.op_res_param_not_valid;

        if (result.m_checkRate > 300 || result.m_checkRate < 1)
            return OpRes.op_res_param_not_valid;
        ///////////////////////////////////
        //控制盈利率大小
        if (!double.TryParse(param.m_earnRatemCtrMax, out result.m_earnRatemCtrMax) || !double.TryParse(param.m_earnRatemCtrMin, out result.m_earnRatemCtrMin))
            return OpRes.op_res_param_not_valid;

        if (result.m_earnRatemCtrMax > 2 || result.m_earnRatemCtrMax < 0.01 || result.m_earnRatemCtrMin > 1 || result.m_earnRatemCtrMin < 0.02)
            return OpRes.op_res_param_not_valid;
        //////////////////////////////////////////////
        //码量控制值
        if (!long.TryParse(param.m_incomeThreshold, out result.m_incomeThreshold))
            return OpRes.op_res_param_not_valid;

        if (result.m_incomeThreshold < 0)
            return OpRes.op_res_param_not_valid;

        /////////////////////////////////////
        if (result.m_roomId == 2 || result.m_roomId == 3 || result.m_roomId == 5 || result.m_roomId == 6 || result.m_roomId == 7)
        {
            if (!double.TryParse(param.m_trickDeviationFix, out result.m_trickDeviationFix))
                return OpRes.op_res_param_not_valid;

            if (result.m_trickDeviationFix > 10 || result.m_trickDeviationFix < 0 )
                return OpRes.op_res_param_not_valid;
        }

        return OpRes.opres_success;
    }
}

//捕鱼个人参数调整（新）
public class DyOpFishlordSingleParamAdjustNew : DyOpParamAdjust 
{
    public DyOpFishlordSingleParamAdjustNew()
        : base((int)GameId.fishlord, TableName.FISHLORD_LOBBY)
    {

    }

    public DyOpFishlordSingleParamAdjustNew(int gameId, string roomTableName)
        : base(gameId, roomTableName)
    {
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        ParamFishlord5 param = (ParamFishlord5)p;
        ParamInner result = new ParamInner();
        OpRes code = calParamInner(param, result);
        if (code == OpRes.opres_success)
        {
            Dictionary<string, object> updata = new Dictionary<string, object>();
            updata.Add("PersonalBaseRate", result.m_baseRate);
            updata.Add("PersonalDeviationFix", result.m_deviationFix);
            updata.Add("NoValuePlayerRate", result.m_checkRate);
            bool res = DBMgr.getInstance().update(TableName.FISHLORD_LOBBY, updata, "key", 1, user.getDbServerID(), DbName.DB_GAME);

            if (res)
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_MODIFY_FISHLORD_SINGLE_CTRL_PARAM,
               new LogModifyFishlordSingleCtrlParam(result.m_baseRate, result.m_deviationFix, result.m_checkRate), user);

            if (!res)
                return OpRes.op_res_failed;
        }

        return code;
    }

    class ParamInner
    {
        public double m_baseRate;
        public double m_deviationFix;
        public double m_checkRate;
    }

    OpRes calParamInner(ParamFishlord5 param, ParamInner result)
    {
        
        if (!double.TryParse(param.m_baseRate, out result.m_baseRate))
            return OpRes.op_res_param_not_valid;

        if (!double.TryParse(param.m_deviationFix, out result.m_deviationFix))
            return OpRes.op_res_param_not_valid;

        if (!double.TryParse(param.m_checkRate, out result.m_checkRate))
            return OpRes.op_res_param_not_valid;

        if (result.m_baseRate > 10 || result.m_baseRate < 0 ||
            result.m_deviationFix > 10 || result.m_deviationFix < 0 || 
            result.m_checkRate > 10 || result.m_checkRate < 0)
            return OpRes.op_res_param_not_valid;

        return OpRes.opres_success;
    }
}

// 鳄鱼公园参数调整
public class DyOpFishParkParamAdjust : DyOpFishlordParamAdjust
{
    public DyOpFishParkParamAdjust()
        : base((int)GameId.fishpark, TableName.FISHPARK_ROOM)
    {

    }
}

//////////////////////////////////////////////////////////////////////////

//奔驰宝马参数调整
public class DyOpBzParamAdjust : DyOpParamAdjust
{
    public DyOpBzParamAdjust()
        : base((int)GameId.bz, TableName.DB_BZ_ROOM)
    {
    }

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.CROD_PARAM_CONTROL, user))
            return OpRes.op_res_no_right;

        ParamFishlordParamAdjust p = (ParamFishlordParamAdjust)param;

        if (string.IsNullOrEmpty(p.m_roomList))
            return OpRes.op_res_param_not_valid;

        if (p.m_isReset)
        {
            return resetExp(user, p);
        }
        return modifyExp(user, p);
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        double expRate = 0.0;
        if (!double.TryParse(p.m_expRate, out expRate) && p.m_op == 0)
            return OpRes.op_res_param_not_valid;
        if (expRate <= 0.0 && p.m_op == 0)
            return OpRes.op_res_param_not_valid;

        string m_Newvalue = "";
        long prob = 0;
        if (!long.TryParse(p.m_expRate, out prob) && p.m_op == 1)
            return OpRes.op_res_param_not_valid;
        if (prob < 0 && p.m_op == 1)
            return OpRes.op_res_param_not_valid;
        m_Newvalue = Convert.ToString(prob);

        int prob_1 = 10000;
        if (!int.TryParse(p.m_expRate, out prob_1) && p.m_op >= 2)
            return OpRes.op_res_param_not_valid;
        if (prob_1 < 0 && p.m_op >= 2)
            return OpRes.op_res_param_not_valid;
        m_Newvalue = Convert.ToString(prob_1);

        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < rooms.Length; i++)
        {
            int roomId = Convert.ToInt32(rooms[i]);

            data.Clear();
            switch (p.m_op)
            {
                case 0: data.Add("ExpectEarnRate", expRate);
                    break;
                case 1: data.Add("AppearAdvertisementLessBet", prob);
                    break;
                case 2: data.Add("BigParadiseGiveScoreProb", prob_1);
                    break;
                case 3: data.Add("SmallParadiseGiveScoreProb", prob_1);
                    break;
                case 4: data.Add("BigHellKillScoreProb", prob_1);
                    break;
                case 5: data.Add("SmallHellKillScoreProb", prob_1);
                    break;
            }

            DBMgr.getInstance().update(TableName.DB_BZ_ROOM, data, "room_id", roomId, user.getDbServerID(), DbName.DB_GAME);
        }
        if (p.m_op == 0)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_MODIFY_FISHLORD_GAIN_RATE,
                new LogModifyFishlordRoomExpRate(p.m_roomList, expRate, (int)GameId.bz), user);
        }
        else
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_MODIFY_BZ_PARAM,
                new LogModifyBzParam(p.m_roomList, p.m_op, m_Newvalue), user);
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
// 鳄鱼大亨参数调整
public class DyOpCrocodileParamAdjust : DyOpParamAdjust
{
    public DyOpCrocodileParamAdjust()
        : base((int)GameId.crocodile, TableName.CROCODILE_ROOM)
    {
        //m_gameId = (int)GameId.crocodile;
        //m_roomTableName = TableName.CROCODILE_ROOM;
    }

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.CROD_PARAM_CONTROL, user))
            return OpRes.op_res_no_right;

        ParamFishlordParamAdjust p = (ParamFishlordParamAdjust)param;

        if (string.IsNullOrEmpty(p.m_roomList))
            return OpRes.op_res_param_not_valid;

        if (p.m_isReset)
        {
            return resetExp(user, p);
        }
        return modifyExp(user, p);
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        double expRate = 0.0;
        if (!double.TryParse(p.m_expRate, out expRate) && p.m_op == 0)
            return OpRes.op_res_param_not_valid;
        if (expRate <= 0.0 && p.m_op == 0)
            return OpRes.op_res_param_not_valid;

        string m_Newvalue = "";
        long prob = 0;
        if (!long.TryParse(p.m_expRate, out prob) && p.m_op == 1)
            return OpRes.op_res_param_not_valid;
        if (prob < 0 && p.m_op == 1)
            return OpRes.op_res_param_not_valid;
        m_Newvalue = Convert.ToString(prob);

        int prob_1 = 10000;
        if (!int.TryParse(p.m_expRate, out prob_1) && p.m_op >= 2)
            return OpRes.op_res_param_not_valid;
        if (prob_1 < 0 && p.m_op >= 2)
            return OpRes.op_res_param_not_valid;
        m_Newvalue = Convert.ToString(prob_1);

        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < rooms.Length; i++)
        {
            int roomId = Convert.ToInt32(rooms[i]);

            data.Clear();
            switch (p.m_op)
            {
                case 0: data.Add("ExpectEarnRate", expRate);
                    break;
                case 1: data.Add("AppearAdvertisementLessBet", prob);
                    break;
                case 2: data.Add("BigParadiseGiveScoreProb", prob_1);
                    break;
                case 3: data.Add("SmallParadiseGiveScoreProb", prob_1);
                    break;
                case 4: data.Add("BigHellKillScoreProb", prob_1);
                    break;
                case 5: data.Add("SmallHellKillScoreProb", prob_1);
                    break;
            }

            DBMgr.getInstance().update(TableName.CROCODILE_ROOM, data, "room_id", roomId, user.getDbServerID(), DbName.DB_GAME);
        }
        if (p.m_op == 0)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_MODIFY_FISHLORD_GAIN_RATE,
                new LogModifyFishlordRoomExpRate(p.m_roomList, expRate, (int)GameId.crocodile), user);
        }
        else
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_MODIFY_CROCODILE_PARAM,
                new LogModifyCrocodileParam(p.m_roomList, p.m_op, m_Newvalue), user);
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//水果机参数调整
public class DyOpFruitParamAdjust : DyOpParamAdjust
{
    public DyOpFruitParamAdjust()
        : base((int)GameId.fruit, TableName.FRUIT_ROOM)
    {
    }

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.FRUIT_PARAM_CONTROL, user))
            return OpRes.op_res_no_right;

        ParamFishlordParamAdjust p = (ParamFishlordParamAdjust)param;

        if (string.IsNullOrEmpty(p.m_roomList))
            return OpRes.op_res_param_not_valid;

        if (p.m_isReset)
        {
            return resetExp(user, p);
        }
        return modifyExp(user, p);
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        double expRate = 0.0;
        if (!double.TryParse(p.m_expRate, out expRate))
            return OpRes.op_res_param_not_valid;
        if (expRate <= 0.0 && p.m_op == 0)
            return OpRes.op_res_param_not_valid;


        Dictionary<string, object> data = new Dictionary<string, object>();
        string[] rooms = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
        bool res = false;
        for (int i = 0; i < rooms.Length; i++)
        {
            int roomId = Convert.ToInt32(rooms[i]);

            data.Clear();
            data.Add("ExpectEarnRate", expRate);

            res = DBMgr.getInstance().update(TableName.FRUIT_ROOM, data, "room_id", roomId, user.getDbServerID(), DbName.DB_GAME);
            if (!res)
            {
                continue;
            }
        }

        if (res)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_MODIFY_FISHLORD_GAIN_RATE,
                new LogModifyFishlordRoomExpRate(p.m_roomList, expRate, (int)GameId.fruit), user);
        }
        else
        {
            return OpRes.op_res_failed;
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
// 骰宝参数调整
public class DyOpDiceParamAdjust : DyOpParamAdjust
{
    public DyOpDiceParamAdjust()
        : base((int)GameId.dice, TableName.DICE_ROOM)
    {
        // m_gameId = (int)GameId.dice;
        // m_roomTableName = TableName.DICE_ROOM;
    }

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.DICE_EARINGS, user))
            return OpRes.op_res_no_right;
        ParamFishlordParamAdjust p = (ParamFishlordParamAdjust)param;

        if (string.IsNullOrEmpty(p.m_roomList))
            return OpRes.op_res_param_not_valid;

        return resetExp(user, p);
    }
}

//////////////////////////////////////////////////////////////////////////

// 百家乐参数调整
public class DyOpBaccaratParamAdjust : DyOpParamAdjust
{
    public DyOpBaccaratParamAdjust()
        : base((int)GameId.baccarat, TableName.BACCARAT_ROOM)
    {
        // m_gameId = (int)GameId.baccarat;
        // m_roomTableName = TableName.BACCARAT_ROOM;
    }

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.BACC_PARAM_CONTROL, user))
            return OpRes.op_res_no_right;

        ParamFishlordParamAdjust p = (ParamFishlordParamAdjust)param;

        if (string.IsNullOrEmpty(p.m_roomList))
            return OpRes.op_res_param_not_valid;

        return resetExp(user, p);
    }
}

//////////////////////////////////////////////////////////////////////////

// 牛牛参数调整
public class DyOpCowsParamAdjust : DyOpParamAdjust
{
    public DyOpCowsParamAdjust()
        : base((int)GameId.cows, TableName.COWS_ROOM)
    {
        //m_gameId = (int)GameId.cows;
        //m_roomTableName = TableName.COWS_ROOM;
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        return modifyExpImp(user, p, "ExpectEarnRate");
    }
}

//////////////////////////////////////////////////////////////////////////

// 五龙参数调整
public class DyOpDragonParamAdjust : DyOpParamAdjust
{
    public DyOpDragonParamAdjust()
        : base((int)GameId.dragon, TableName.DRAGON_ROOM)
    {
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        return modifyExpImp(user, p, "expect_earn_rate");
    }

    // 返回旧的参数
    protected override ResultExpRateParam getOldParam(int roomId, GMUser user)
    {
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(m_roomTableName, "room_id", roomId,
                null, user.getDbServerID(), DbName.DB_GAME);

        if (data == null)
            return null;

        ResultExpRateParam param = new ResultExpRateParam();

        if (data.ContainsKey("room_income"))
        {
            param.m_totalIncome = Convert.ToInt64(data["room_income"]);
        }
        if (data.ContainsKey("room_outcome"))
        {
            param.m_totalOutlay = Convert.ToInt64(data["room_outcome"]);
        }
        if (data.ContainsKey("expect_earn_rate"))
        {
            param.m_expRate = Convert.ToDouble(data["expect_earn_rate"]);
        }
        return param;
    }
}

//////////////////////////////////////////////////////////////////////////
// 游戏参数调整，入口
public class DyOpGameParamAdjust : DyOpBase
{
    private Dictionary<GameId, DyOpParamAdjust> m_game = new Dictionary<GameId, DyOpParamAdjust>();

    public DyOpGameParamAdjust()
    {
        m_game.Add(GameId.shcd, new DyOpShcdParamAdjust());
        m_game.Add(GameId.calf_roping, new DyOpCalfRopingParamAdjust());
    }

    public override OpRes doDyop(object param, GMUser user)
    {
        ParamFishlordParamAdjust p = (ParamFishlordParamAdjust)param;
        if (m_game.ContainsKey(p.m_gameId))
        {
            return m_game[p.m_gameId].doDyop(p, user);
        }
        return OpRes.op_res_failed;
    }
}

// 黑红梅方参数调整
public class DyOpShcdParamAdjust : DyOpParamAdjust
{
    public DyOpShcdParamAdjust()
        : base((int)GameId.shcd, TableName.SHCDCARDS_ROOM)
    {
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        if (p.m_op == 1) // 修改难度
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            string[] roomArr = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var roomId in roomArr)
            {
                data.Clear();
                data.Add("EarnRateControl", Convert.ToInt32(p.m_expRate));
                bool res = DBMgr.getInstance().update(TableName.SHCDCARDS_ROOM, data, "room_id", Convert.ToInt32(roomId),
                        user.getDbServerID(), DbName.DB_GAME);
                if (!res)
                    return OpRes.op_res_failed;
            }

            return OpRes.opres_success;
        }
        else if (p.m_op == 2) // 修改大小王
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            string[] roomArr = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var roomId in roomArr)
            {
                data.Clear();
                data.Add("next_joker_count", Convert.ToInt32(p.m_expRate));
                bool res = DBMgr.getInstance().update(TableName.SHCDCARDS_ROOM, data, "room_id", Convert.ToInt32(roomId),
                        user.getDbServerID(), DbName.DB_GAME);
                if (!res)
                    return OpRes.op_res_failed;
            }

            return OpRes.opres_success;
        }
        else if (p.m_op == 3) // 设置作弊局数
        {
            //string[] arr = Tool.split(p.m_expRate, '-');

            p.m_expRate = p.m_expRate.Trim();
            string[] roomArr = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var roomId in roomArr)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                //data.Add("beginCheatIndex", Convert.ToInt32(arr[0]));
                //data.Add("endCheatIndex", Convert.ToInt32(arr[1]));
                data.Add("cheatStr", p.m_expRate);
                bool res = DBMgr.getInstance().update(TableName.SHCDCARDS_ROOM, data, "room_id", Convert.ToInt32(roomId),
                        user.getDbServerID(), DbName.DB_GAME);
                if (!res)
                    return OpRes.op_res_failed;
            }

            return OpRes.opres_success;
        }
        else if (p.m_op == 4) //设置作弊最大阀值
        {
            p.m_expRate = p.m_expRate.Trim();
            Dictionary<string, object> data = new Dictionary<string, object>();
            string[] roomArr = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var roomId in roomArr)
            {
                data.Clear();
                data.Add("ExpectEarnMaxRate", Convert.ToDouble(p.m_expRate));
                bool res = DBMgr.getInstance().update(TableName.SHCDCARDS_ROOM, data, "room_id", Convert.ToInt32(roomId),
                        user.getDbServerID(), DbName.DB_GAME);
                if (!res)
                    return OpRes.op_res_failed;
            }

            return OpRes.opres_success;
        }
        return modifyExpImp(user, p, "ExpectEarnRate");
    }
}

// 套牛参数调整
public class DyOpCalfRopingParamAdjust : DyOpParamAdjust
{
    public DyOpCalfRopingParamAdjust()
        : base((int)GameId.calf_roping, TableName.CALF_ROPING_ROOM)
    {
    }

    protected override OpRes modifyExp(GMUser user, ParamFishlordParamAdjust p)
    {
        return modifyExpImp(user, p, "ExpectEarnRate", "calfRoping_lobby");
    }

    protected override OpRes resetExp(GMUser user, ParamFishlordParamAdjust p)
    {
        return resetExpImp(user, p, "lobby_income", "lobby_outcome", "calfRoping_lobby");
    }

    protected override ResultExpRateParam getOldParam(int roomId, GMUser user)
    {
        return getOldParamImp(user, roomId, "calfRoping_lobby", "lobby_income", "lobby_outcome", "ExpectEarnRate");
    }
}
//////////////////////////////////////////////////////////////////////////

// 清空鱼统计表
public class DyOpClearFishTable : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        string tableName = (string)param;
        DBMgr.getInstance().clearTable(tableName, user.getDbServerID(), DbName.DB_PUMP);
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

enum ReloadTable
{
    // 经典捕鱼鱼表
    fish,
    // 鳄鱼公园鱼表
    fishpark_fish,
}

// 重新加载表格
public class DyOpReLoadTable : DyOpBase
{
    public static string[] fields = new string[] { "_id" };

    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.DATA_RELOAD_TABLE, user))
            return OpRes.op_res_no_right;

        bool res = false;
        int tableIndex = (int)param;

        //服务器ID
        XmlConfig xml = ResMgr.getInstance().getRes("M_ReloadServiceCFG.xml");
        string ServerStr = xml.getString("serverId", "");
        string[] serverIdStr = Tool.split(ServerStr, ',');
        int[] ServerId_arr = Array.ConvertAll<string, int>(serverIdStr, s => int.Parse(s));

        switch (tableIndex)
        {
            case (int)ReloadTable.fish: // 加载鱼表
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

                    for (int i = 0; i < ServerId_arr.Length; i++)
                    {
                        //判定是否已经存在
                        IMongoQuery imq = Query.And(Query.EQ("key", "reload_cfg"), Query.EQ("serverId", ServerId_arr[i]));
                        List<Dictionary<string, object>> ret =
                            DBMgr.getInstance().executeQuery(TableName.RELOAD_FISHCFG, dip, imq, 0, 0, fields);
                        if (ret != null && ret.Count > 0) //已经存在
                            continue;

                        data.Clear();
                        data.Add("key", "reload_cfg");
                        data.Add("serverId", ServerId_arr[i]);
                        res = DBMgr.getInstance().insertData(TableName.RELOAD_FISHCFG, data, user.getDbServerID(), DbName.DB_GAME);
                    }
                }
                break;
            //case (int)ReloadTable.fishpark_fish: // 加载鱼表
            //    {
            //        Dictionary<string, object> data = new Dictionary<string, object>();
            //        data.Add("key", "reload_cfg");
            //        res = DBMgr.getInstance().insertData(TableName.RELOAD_FISHPARK_CFG, data, user.getDbServerID(), DbName.DB_GAME);
            //    }
            //    break;
        }

        if (res)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_RELOAD_TABLE, new LogReloadTable((int)GameId.fishlord), user);
        }

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamServiceInfo
{
    // 平台
    public string m_key = "";
    // 描述
    public string m_desc = "";

    public bool m_isAdd = true;
}

// 增加或修改客服信息
public class DyOpServiceInfo : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.SVR_ADD_SERVICE_INFO, user))
            return OpRes.op_res_no_right;

        int accServerId = user.getDbServerID();
        if (accServerId == -1)
            return OpRes.op_res_failed;

        ParamServiceInfo p = (ParamServiceInfo)param;
        if (p.m_isAdd)
        {
            return addServiceInfo(accServerId, p);
        }
        return delServiceInfo(accServerId, p);
    }

    private OpRes addServiceInfo(int accServerId, ParamServiceInfo p)
    {
        if (p.m_key == "" || p.m_desc == "")
        {
            return OpRes.op_res_param_not_valid;
        }
        Match m = Regex.Match(p.m_desc, Exp.SERVICE_HELP_M);
        if (!m.Success)
        {
            m = Regex.Match(p.m_desc, Exp.SERVICE_HELP1);
            if (!m.Success)
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        Dictionary<string, object> data = new Dictionary<string, object>();
        bool res = false;
        if (data != null)
        {
            data["plat"] = p.m_key;
            data["info"] = p.m_desc;

            res = DBMgr.getInstance().save(TableName.SERVICE_INFO, data, "plat", p.m_key, accServerId, DbName.DB_PLAYER);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    private OpRes delServiceInfo(int accServerId, ParamServiceInfo p)
    {
        string[] strs = Tool.split(p.m_key, ',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            DBMgr.getInstance().remove(TableName.SERVICE_INFO, "plat", strs[i], accServerId, DbName.DB_PLAYER);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamFreezeHeadInfo
{
    // 玩家ID
    public string m_playerId = "";
    // 冻结天数
    public string m_freezeDays = "";
}

// 冻结头像
public class DyOpFreezeHead : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.OP_FREEZE_HEAD, user))
            return OpRes.op_res_no_right;

        ParamFreezeHeadInfo p = (ParamFreezeHeadInfo)param;
        int playerId = 0, days = 7;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        if (!string.IsNullOrEmpty(p.m_freezeDays))
        {
            if (!int.TryParse(p.m_freezeDays, out days))
            {
                return OpRes.op_res_param_not_valid;
            }
            if (days <= 0)
                return OpRes.op_res_param_not_valid;
        }

        bool res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
        if (!res)
        {
            return OpRes.op_res_player_not_exist;
        }

        DateTime deadTime = DateTime.Now.AddDays(days);
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("playerId", playerId);
        data.Add("rtype", (int)RechargeType.delIconCustom);
        data.Add("paramTime", deadTime);
        res = DBMgr.getInstance().insertData(TableName.GM_RECHARGE, data, user.getDbServerID(), DbName.DB_PLAYER);

        if (res)
        {
            Dictionary<string, object> dataSet = new Dictionary<string, object>();
            dataSet.Add("iconCustom", "");
            res = DBMgr.getInstance().update(TableName.FISHLORD_BAOJIN_PLAYER, dataSet, "player_id", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_DEL_CUSTOM_HEAD,
                                new LogFreezeHead(playerId, deadTime),
                                user);
            }
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamAddChannel
{
    public bool m_isAdd;
    public List<string> m_channels = new List<string>();
}

// 渠道编辑
public class DyOpAddChannel : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.OP_CHANNEL_EDIT, user))
            return OpRes.op_res_no_right;

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        ParamAddChannel p = (ParamAddChannel)param;
        if (p.m_isAdd)
            return addChannel(serverId, p);

        return delChannel(serverId, p);
    }

    public string getTestChannel(GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return "";

        List<Dictionary<string, object>> data =
            DBMgr.getInstance().executeQuery(TableName.TEST_SERVER, serverId, DbName.DB_CONFIG);

        string str = "";
        foreach (var d in data)
        {
            // str += "'" + d["channel"].ToString() + "'" + ",";
            str += d["channel"].ToString() + ",";
        }
        return str;
    }

    private OpRes addChannel(int serverId, ParamAddChannel p)
    {
        List<string> channels = (List<string>)p.m_channels;
        if (channels.Count == 0)
            return OpRes.opres_success;

        string str = "";
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        foreach (string c in channels)
        {
            bool res = DBMgr.getInstance().keyExists(TableName.TEST_SERVER, "channel", c, serverId, DbName.DB_CONFIG);
            if (!res)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("channel", c);
                dataList.Add(data);

                // str += "'" + c + "'" + ",";
            }
        }

        if (dataList.Count > 0)
        {
            bool res = DBMgr.getInstance().insertData(TableName.TEST_SERVER, dataList, serverId, DbName.DB_CONFIG);
            /*if (res)
            {
                AccessDb.getAccDb().setConnDb("channel.mdb");

                string sql = string.Format("update  channel set enable=false where channelNo in({0})", str);
                int n = AccessDb.getAccDb().startOp(sql);
                AccessDb.getAccDb().end();
            }*/
        }

        return OpRes.opres_success;
    }

    private OpRes delChannel(int serverId, ParamAddChannel p)
    {
        List<string> channels = (List<string>)p.m_channels;
        if (channels.Count == 0)
            return OpRes.opres_success;

        // string str = "";
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        foreach (string c in channels)
        {
            bool res = DBMgr.getInstance().remove(TableName.TEST_SERVER, "channel", c, serverId, DbName.DB_CONFIG);
            if (res)
            {
                //    str += "'" + c + "'" + ",";
            }
        }

        /* if (str != "")
            {
                AccessDb.getAccDb().setConnDb("channel.mdb");
                string sql = string.Format("update  channel set enable=true where channelNo in({0})", str);
                int n = AccessDb.getAccDb().startOp(sql);
                AccessDb.getAccDb().end();
            }*/

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamCowsCard
{
    public int m_op = 0;
    public object m_data;
}

public class ParamAddCowsCard
{
    public int m_bankerType;
    public int m_other1Type;
    public int m_other2Type;
    public int m_other3Type;
    public int m_other4Type;
}

// 牛牛牌型设置
public class DyOpAddCowsCardType : DyOpBase
{
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.COW_CARD_TYPE, user))
            return OpRes.op_res_no_right;

        ParamCowsCard p = (ParamCowsCard)param;
        if (p.m_op == 0)
        {
            ParamAddCowsCard add = (ParamAddCowsCard)p.m_data;
            return addCardType(add, user);
        }

        string key = (string)p.m_data;
        return delCardType(key, user);
    }

    private OpRes addCardType(ParamAddCowsCard param, GMUser user)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("insert_time", DateTime.Now);
        data.Add("banker_cards", param.m_bankerType);
        data.Add("other_cards1", param.m_other1Type);
        data.Add("other_cards2", param.m_other2Type);
        data.Add("other_cards3", param.m_other3Type);
        data.Add("other_cards4", param.m_other4Type);
        data.Add("key", Guid.NewGuid().ToString());

        bool res = DBMgr.getInstance().insertData(TableName.COWS_CARDS,
            data, user.getDbServerID(), DbName.DB_GAME);
        if (res)
        {
            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_COWS_ADD_CARDS_TYPE,
                new LogCowsAddCardType(param.m_bankerType, param.m_other1Type,
                    param.m_other2Type, param.m_other3Type, param.m_other4Type),
                user);
        }
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    private OpRes delCardType(string key, GMUser user)
    {
        bool res =
            DBMgr.getInstance().remove(TableName.COWS_CARDS, "key", key, user.getDbServerID(), DbName.DB_GAME);
        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }
}
///////////////////////////////////////////////////////////////////////////
//游戏控制
public class OperationGameCtrlItem 
{
    public string m_id;
    public string m_time;
    public string m_channelNo;
    public string m_channelName;
    public string m_version;

    public int m_turret;
    public string m_turretName;
    public int m_vipLv;

    public string m_onOff;

    public string getTurretName()
    {
        if(m_turret == -1)
            return "-1";

        Fish_LevelCFGData turretInfo = Fish_TurretLevelCFG.getInstance().getValue(m_turret);
        if(turretInfo != null)
            return turretInfo.m_openRate.ToString();

        return "";
    }
}
public class DyOpOperationGameCtrl : DyOpBase
{
    List<OperationGameCtrlItem> m_ctrlList = new List<OperationGameCtrlItem>();
    public override OpRes doDyop(object param, GMUser user)
    {
        if (!RightMgr.getInstance().canEdit(RightDef.OP_OPERATION_GAME_CTRL, user))
            return OpRes.op_res_no_right;

        ParamQuery p = (ParamQuery)param;
        //p.m_op  0 添加 1删除 2查看
        if (p.m_op == 2)
        {
            viewGameVersionCtrl(user);
            return OpRes.opres_success;
        }

        //添加
        if (p.m_op == 0)
        {
            if (string.IsNullOrEmpty(p.m_channelNo) || string.IsNullOrEmpty(p.m_param)) //版本号
                return OpRes.op_res_param_not_valid;
        }

        return addVerControl(p, user);
    }

    private OpRes addVerControl(ParamQuery p, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        bool res = false;

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Clear();
        data.Add("genTime", DateTime.Now);

        if (p.m_op == 0) //添加
        {
            data.Add("version", p.m_param);

            data.Add("turretLv", p.m_type); //炮倍率
            data.Add("vipLv", p.m_curPage);//VIP等级

            //功能开启
            string[] list = p.m_time.Split(',');
            List<int> onOff = new List<int>();
            foreach (var li in list)
            {
                int da = Convert.ToInt32(li);
                onOff.Add(da);
            }
            data.Add("onOff", onOff);

            if (p.m_channelNo == "-1")
            {
                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach (var item in cd.Values)
                {
                    data.Add("channel", Convert.ToInt32(item.m_channelNo));
                    res = DBMgr.getInstance().insertData(TableName.OP_CHANNEL_VER_CONTROL, data, serverId, DbName.DB_ACCOUNT);
                    data.Remove("channel");
                }
            }
            else {
                data.Add("channel", Convert.ToInt32(p.m_channelNo));
                res = DBMgr.getInstance().insertData(TableName.OP_CHANNEL_VER_CONTROL, data, serverId, DbName.DB_ACCOUNT);
            }
        }
        else //移除
        {
            res = DBMgr.getInstance().remove(TableName.OP_CHANNEL_VER_CONTROL, "_id", new ObjectId(p.m_param), serverId, DbName.DB_ACCOUNT);
        }

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    public override object getResult()
    {
        return m_ctrlList;
    }

    public void viewGameVersionCtrl(GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return;

        List<Dictionary<string, object>> dataList =
            DBMgr.getInstance().executeQuery(TableName.OP_CHANNEL_VER_CONTROL, serverId, DbName.DB_ACCOUNT, null, 0,0,null, "version");
        m_ctrlList.Clear();

        if (dataList == null || dataList.Count == 0)
            return;

        for (int i = 0; i < dataList.Count; i++)
        {
            OperationGameCtrlItem item = new OperationGameCtrlItem();
            m_ctrlList.Add(item);

            Dictionary<string, object> data = dataList[i];
            item.m_id = Convert.ToString(data["_id"]);

            item.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("channel"))
            {
                item.m_channelNo = Convert.ToString(data["channel"]).PadLeft(6, '0');
                item.m_channelName = getChannelName(item.m_channelNo);
            }

            if (data.ContainsKey("version"))
                item.m_version = Convert.ToString(data["version"]);

            if (data.ContainsKey("turretLv"))
            {
                item.m_turret = Convert.ToInt32(data["turretLv"]);
                item.m_turretName = item.getTurretName();

                if (data.ContainsKey("vipLv"))
                    item.m_vipLv = Convert.ToInt32(data["vipLv"]);

                string[] str_onOffName = new string[] { "排行", "空投", "兑换", "邮件", "客服" };
                string str = "";
                if (data.ContainsKey("onOff"))
                {
                    object[] arr = (object[])data["onOff"];
                    int k = 0;
                    foreach (var name in str_onOffName)
                    {
                        if (Convert.ToInt32(arr[k]) == 1)
                        {
                            str += str_onOffName[k] + ',';
                        }
                        k++;
                    }
                }
                item.m_onOff = str.Trim(',');
            }
        }
    }

    public string getChannelName(string channelNo) 
    {
        string channelName = channelNo;
        TdChannelInfo channel = TdChannel.getInstance().getValue(channelNo);
        if (channel != null)
            return channel.m_channelName;

        return channelName;
    }
}
    ////////////////////////////////////////////////////////////////////////////
    //机器人积分管理
    public class DyOpOperationFishlordRobotRankCFG : DyOpBase
    {
        public static string[] fields = new string[] { "_id" };
        public override OpRes doDyop(object param, GMUser user)
        {
            ParamQuery p = (ParamQuery)param;
            bool res = false;
            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            if (string.IsNullOrEmpty(p.m_param))
                return OpRes.op_res_param_not_valid;

            if (!Regex.IsMatch(p.m_param, Exp.TWO_NUM_BY_COMMA))
                return OpRes.op_res_param_not_valid;

            string[] rankParam = p.m_param.Split(',');
            if (Convert.ToInt32(rankParam[0]) > Convert.ToInt32(rankParam[1]))
                return OpRes.op_res_param_not_valid;

            Dictionary<string, object> dt = new Dictionary<string, object>();
            res = DBMgr.getInstance().keyExists(TableName.STAT_FISHLORD_ROBOT_RANK_CFG, "type", p.m_op, dip);
            dt.Clear();
            dt.Add("rankPointMin", Convert.ToInt32(rankParam[0]));
            dt.Add("rankPointMax", Convert.ToInt32(rankParam[1]));
            if (!res) // 不存在则插入  存在则更新
            {
                dt.Add("type", p.m_op);
                res = DBMgr.getInstance().insertData(TableName.STAT_FISHLORD_ROBOT_RANK_CFG, dt, dip);
            }
            else
            {
                res = DBMgr.getInstance().update(TableName.STAT_FISHLORD_ROBOT_RANK_CFG, dt, "type", p.m_op, dip);
            }

            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_OPERATION_FISHLORD_ROBOT_RANK_CFG,
                                new LogFishlordRobotRankCFG(p.m_op, p.m_param), user);
            }

            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
    }
    ///////////////////////////////////////////////////////////////////////////
    //点杀点送
    public class ViewAddScorePoolItem
    {
        public int m_playerId;
        public string m_nickName;
        public string m_lastFixTime;
        public int m_playerScorePool;
        public string m_scorePoolTimeLeft;
        public int m_lastPoolSetVal;
        public int m_moneyType;
        public string m_id;
    }
    //捕鱼点杀点送
    public class DyOpAddScorePool : DyOpBase
    {
        static string[] FIELDS = { "playerId", "playerScorePool", "poolSetValue", "deadTime", "_id", "LastModifyTime", "moneyType" };

        List<ViewAddScorePoolItem> m_buffPlayerList = new List<ViewAddScorePoolItem>();
        public override OpRes doDyop(object param, GMUser user)
        {
            if (!RightMgr.getInstance().canEdit(RightDef.FISH_PLAYER_SCORE_POOL, user))
                return OpRes.op_res_no_right;

            ParamGameBItem p = (ParamGameBItem)param;
            //p.m_op  0 添加 1删除 2查看 3清零当前操作值
            if (p.m_op == 2)
            {
                viewScorePool(user);
                return OpRes.opres_success;
            }

            int m_playerId = 0;
            if (string.IsNullOrEmpty(p.m_playerId))
                return OpRes.op_res_param_not_valid;

            if (!int.TryParse(p.m_playerId, out m_playerId))
                return OpRes.op_res_param_not_valid;

            if (p.m_op == 0)
            {
                int m_setPoolVal = 0;
                if (string.IsNullOrEmpty(p.m_param))
                    return OpRes.op_res_param_not_valid;

                if (!int.TryParse(p.m_param, out m_setPoolVal))
                    return OpRes.op_res_param_not_valid;

                DateTime setTime = DateTime.MinValue;
                if (string.IsNullOrEmpty(p.m_time))
                    return OpRes.op_res_param_not_valid;

                if (!Tool.splitTimeStr(p.m_time, ref setTime, 3))
                    return OpRes.op_res_time_format_error;

                if (setTime < DateTime.Now)
                    return OpRes.op_res_param_not_valid;
            }
            return addScorePool(p, user);
        }

        private OpRes addScorePool(ParamGameBItem p, GMUser user)
        {
            IMongoQuery imq = Query.And(
                Query.EQ("playerId", Convert.ToInt32(p.m_playerId)),
                Query.EQ("moneyType", p.m_type));

            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            Dictionary<string, object> playerScoreInfo = DBMgr.getInstance().getTableData(TableName.FISHLORD_KS_SCORE, dip, imq);

            bool res = false;

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Clear();
            data.Add("LastModifyTime", DateTime.Now);

            if (p.m_op == 0) //添加
            {
                data.Add("deadTime", Convert.ToDateTime(p.m_time));

                if (playerScoreInfo == null)  //不存在时
                {
                    data.Add("playerId", Convert.ToInt32(p.m_playerId));
                    data.Add("moneyType", p.m_type);
                    data.Add("playerScorePool", Convert.ToInt32(p.m_param));
                    data.Add("poolSetValue", Convert.ToInt32(p.m_param));
                    res = DBMgr.getInstance().insertData(TableName.FISHLORD_KS_SCORE, data, user.getDbServerID(), DbName.DB_GAME);
                    if (res)
                    {
                        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SCORE_POOL_SET,
                                        new LogSetScorePool((int)GameId.fishlord, p.m_op, p.m_playerId, p.m_param, p.m_time, p.m_type), user);
                    }
                    return res ? OpRes.opres_success : OpRes.op_res_failed;
                }

                int scorePool_new = 0;//当前水池
                if (playerScoreInfo.ContainsKey("playerScorePool"))
                {
                    int scorePool_old = Convert.ToInt32(playerScoreInfo["playerScorePool"]);
                    scorePool_new = scorePool_old + Convert.ToInt32(p.m_param);
                }
                else
                {
                    scorePool_new = Convert.ToInt32(p.m_param);
                }

                int poolSetValue_new = 0; //操作值
                if (playerScoreInfo.ContainsKey("poolSetValue"))
                {
                    int poolSetValue_old = Convert.ToInt32(playerScoreInfo["poolSetValue"]);
                    poolSetValue_new = poolSetValue_old + Convert.ToInt32(p.m_param);
                }
                else
                {
                    poolSetValue_new = Convert.ToInt32(p.m_param);
                }

                data.Add("playerScorePool", scorePool_new);
                data.Add("poolSetValue", poolSetValue_new);

            }
            else if (p.m_op == 3)//清零当前操作值
            {
                data.Add("poolSetValue", 0);
                data.Add("deadTime", DateTime.Now);
            }
            else //移除
            {
                data.Add("playerScorePool", 0);
                data.Add("deadTime", DateTime.Now);
            }

            res = DBMgr.getInstance().update(TableName.FISHLORD_KS_SCORE, data, imq, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SCORE_POOL_SET,
                                new LogSetScorePool((int)GameId.fishlord, p.m_op, p.m_playerId, p.m_param, p.m_time, p.m_type), user);
            }

            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        public override object getResult()
        {
            return m_buffPlayerList;
        }

        public void viewScorePool(GMUser user)
        {
            IMongoQuery imq = Query.NE("poolSetValue", 0);

            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            List<Dictionary<string, object>> dataList =
                DBMgr.getInstance().executeQuery(TableName.FISHLORD_KS_SCORE, dip, imq, 0, 0, FIELDS);

            m_buffPlayerList.Clear();

            if (dataList == null || dataList.Count == 0)
                return;

            for (int i = 0; i < dataList.Count; i++)
            {
                ViewAddScorePoolItem item = new ViewAddScorePoolItem();
                m_buffPlayerList.Add(item);

                Dictionary<string, object> data = dataList[i];
                item.m_id = Convert.ToString(data["_id"]);
                if (data.ContainsKey("deadTime"))
                {
                    item.m_scorePoolTimeLeft = Convert.ToDateTime(data["deadTime"]).ToLocalTime().ToString();
                }
                if (data.ContainsKey("playerScorePool"))
                {
                    item.m_playerScorePool = Convert.ToInt32(data["playerScorePool"]);
                }
                if (data.ContainsKey("LastModifyTime"))
                {
                    item.m_lastFixTime = Convert.ToDateTime(data["LastModifyTime"]).ToLocalTime().ToString();
                }
                if (data.ContainsKey("poolSetValue"))
                {
                    item.m_lastPoolSetVal = Convert.ToInt32(data["poolSetValue"]);
                }

                item.m_playerId = Convert.ToInt32(data["playerId"]);

                if (data.ContainsKey("moneyType"))
                {
                    item.m_moneyType = Convert.ToInt32(data["moneyType"]);
                }

                //获取玩家昵称
                DbInfoParam dip_player = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
                Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", item.m_playerId,
                                                new string[] { "nickname" }, dip_player);
                if (playerInfo != null)
                {
                    item.m_nickName = playerInfo["nickname"].ToString();
                }
            }
        }
    }
    /////////////////////////////////////////////////////////////////////////
    //水浒传点杀点送
    public class DyOpShuihzAddScorePool : DyOpBase
    {
        static string[] FIELDS = { "player_id", "curSendScoreValue", "LastPoolSetValue", "LastSendScoreTime" };

        List<ViewAddScorePoolItem> m_buffPlayerList = new List<ViewAddScorePoolItem>();
        public override OpRes doDyop(object param, GMUser user)
        {
            if (!RightMgr.getInstance().canEdit(RightDef.SHUIHZ_PLAYER_SCORE_POOL, user))
                return OpRes.op_res_no_right;

            ParamQuery p = (ParamQuery)param;
            //p.m_op  0 添加 1删除 2查看

            if (p.m_op == 2)
            {
                viewScorePool(user);
                return OpRes.opres_success;
            }

            int m_playerId = 0;
            int m_setPoolVal = 0;
            if (!string.IsNullOrEmpty(p.m_playerId))
            {
                if (!int.TryParse(p.m_playerId, out m_playerId))
                {
                    return OpRes.op_res_param_not_valid;
                }
            }
            else
            {
                return OpRes.op_res_param_not_valid;
            }

            if (p.m_op == 0)
            {
                if (!string.IsNullOrEmpty(p.m_param))
                {
                    if (!int.TryParse(p.m_param, out m_setPoolVal))
                    {
                        return OpRes.op_res_param_not_valid;
                    }
                }
                else
                {
                    return OpRes.op_res_param_not_valid;
                }
            }
            return addScorePool(p, user);
        }

        private OpRes addScorePool(ParamQuery p, GMUser user)
        {
            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            bool res = DBMgr.getInstance().keyExists(TableName.SHUIHZ_PLAYER, "player_id", Convert.ToInt32(p.m_playerId), dip);
            if (!res)
                return OpRes.op_res_player_not_exist;

            Dictionary<string, object> data = new Dictionary<string, object>();

            data.Clear();
            data.Add("LastSendScoreTime", DateTime.Now);

            if (p.m_op == 0) //添加
            {
                Dictionary<string, object> playerScorePool = DBMgr.getInstance().getTableData(TableName.SHUIHZ_PLAYER, "player_id", Convert.ToInt32(p.m_playerId),
                                                    new string[] { "curSendScoreValue", "LastPoolSetValue" }, dip);
                if (playerScorePool != null && playerScorePool.Count != 0)
                {
                    int scorePool_new = 0;
                    if (playerScorePool.ContainsKey("curSendScoreValue"))
                    {
                        int scorePool_old = Convert.ToInt32(playerScorePool["curSendScoreValue"]);
                        scorePool_new = scorePool_old + Convert.ToInt32(p.m_param);
                    }
                    else
                    {
                        scorePool_new = Convert.ToInt32(p.m_param);
                    }

                    int lastPoolSetValue_new = 0;
                    if (playerScorePool.ContainsKey("LastPoolSetValue"))
                    {
                        int lastPoolSetValue_old = Convert.ToInt32(playerScorePool["LastPoolSetValue"]);
                        lastPoolSetValue_new = lastPoolSetValue_old + Convert.ToInt32(p.m_param);
                    }
                    else
                    {
                        lastPoolSetValue_new = Convert.ToInt32(p.m_param);
                    }

                    data.Add("curSendScoreValue", scorePool_new);
                    data.Add("LastPoolSetValue", lastPoolSetValue_new);
                }
                else
                {
                    data.Add("curSendScoreValue", Convert.ToInt32(p.m_param));
                    data.Add("LastPoolSetValue", Convert.ToInt32(p.m_param));
                }
            }
            else if (p.m_op == 3)//清零当前操作值
            {
                data.Add("LastPoolSetValue", 0);
            }
            else //移除
            {
                data.Add("curSendScoreValue", 0);
            }

            DBMgr.getInstance().update(TableName.SHUIHZ_PLAYER, data, "player_id", Convert.ToInt32(p.m_playerId), user.getDbServerID(), DbName.DB_GAME);

            OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_SCORE_POOL_SET,
                    new LogSetScorePool((int)GameId.shuihz, p.m_op, p.m_playerId, p.m_param, p.m_time), user);

            return OpRes.opres_success;
        }

        public override object getResult()
        {
            return m_buffPlayerList;
        }

        void viewScorePool(GMUser user)
        {
            IMongoQuery imq_1 = Query.LT("LastPoolSetValue", 0);
            IMongoQuery imq_2 = Query.GT("LastPoolSetValue", 0);
            IMongoQuery imq = Query.Or(imq_1, imq_2);

            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            List<Dictionary<string, object>> dataList =
                DBMgr.getInstance().executeQuery(TableName.SHUIHZ_PLAYER, dip, imq, 0, 0, FIELDS);

            m_buffPlayerList.Clear();

            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    ViewAddScorePoolItem item = new ViewAddScorePoolItem();
                    m_buffPlayerList.Add(item);

                    Dictionary<string, object> data = dataList[i];


                    if (data.ContainsKey("curSendScoreValue"))
                    {
                        item.m_playerScorePool = Convert.ToInt32(data["curSendScoreValue"]);
                    }
                    if (data.ContainsKey("LastSendScoreTime"))
                    {
                        item.m_lastFixTime = Convert.ToDateTime(data["LastSendScoreTime"]).ToLocalTime().ToString();
                    }
                    if (data.ContainsKey("LastPoolSetValue"))
                    {
                        item.m_lastPoolSetVal = Convert.ToInt32(data["LastPoolSetValue"]);
                    }
                    item.m_playerId = Convert.ToInt32(data["player_id"]);

                    //获取玩家昵称
                    DbInfoParam dip_player = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
                    Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", item.m_playerId,
                                                    new string[] { "nickname" }, dip_player);
                    if (playerInfo != null)
                    {
                        item.m_nickName = playerInfo["nickname"].ToString();
                    }
                }
            }
        }
    }
    //////////////////////////////////////////////////////////////////////////
    public class ParamAddWishCurse
    {
        public int m_opType; // 0添加, 1去除, 2查看
        public int m_gameId = 1; // 经典捕鱼or鳄鱼公园
        public int m_wishType;
        public string m_playerId;
        public string m_rate;

        public bool isAdd()
        {
            return m_opType == 0;
        }
    }

    public class ViewAddWishCurseItem
    {
        public string genTime = "";
        public string playerId;
        public double value;
    }

    // 祝福与诅咒
    public class DyOpAddWishCurse : DyOpBase
    {
        static string[] FIELDS = { "FixRateTime", "FixRate", "player_id" };

        List<ViewAddWishCurseItem> m_buffPlayerList = new List<ViewAddWishCurseItem>();

        public override OpRes doDyop(object param, GMUser user)
        {
            if (!RightMgr.getInstance().canEdit(RightDef.OP_WISH_CURSE, user))
                return OpRes.op_res_no_right;

            ParamAddWishCurse p = (ParamAddWishCurse)param;

            if (p.m_opType == 2)
            {
                viewWishCurse(user);
                return OpRes.opres_success;
            }

            int playerId = 0;
            double rate = 0.0;

            if (p.isAdd())
            {
                if (!double.TryParse(p.m_rate, out rate))
                    return OpRes.op_res_param_not_valid;

                if (rate <= 0.0)
                    return OpRes.op_res_param_not_valid;
            }

            if (!int.TryParse(p.m_playerId, out playerId))
                return OpRes.op_res_param_not_valid;

            if (p.m_wishType == 1) // 诅咒
            {
                rate = -rate;
            }
            return addWishCurse(p, playerId, rate, user);
        }

        public override object getResult()
        {
            return m_buffPlayerList;
        }

        private OpRes addWishCurse(ParamAddWishCurse param,
                                    int playerId,
                                    double rate,
                                    GMUser user)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (param.isAdd())
            {
                data.Add("FixRate", rate);
                data.Add("FixRateTime", DateTime.Now);
            }
            else
            {
                data.Add("FixRate", 0.0);
            }
            bool res = false;
            string tableName = "";
            if (param.m_gameId == (int)GameId.fishlord)
            {
                tableName = TableName.FISHLORD_PLAYER;
            }
            else
            {
                tableName = TableName.FISHPARK_PLAYER;
            }

            res = DBMgr.getInstance().keyExists(tableName, "player_id", playerId, user.getDbServerID(), DbName.DB_GAME);
            if (!res)
                return OpRes.op_res_player_not_exist;

            res = DBMgr.getInstance().update(tableName, data,
                    "player_id",
                    playerId, user.getDbServerID(), DbName.DB_GAME);

            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_WISH_CURSE,
                    new LogWishCurse(param.m_gameId, playerId, param.m_wishType, param.m_opType),
                    user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        void viewWishCurse(GMUser user)
        {
            IMongoQuery imq1 = Query.LT("FixRate", 0);
            IMongoQuery imq2 = Query.GT("FixRate", 0);
            IMongoQuery imq = Query.Or(imq1, imq2);

            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

            List<Dictionary<string, object>> dataList =
                DBMgr.getInstance().executeQuery(TableName.FISHLORD_PLAYER, dip, imq, 0, 0, FIELDS);

            m_buffPlayerList.Clear();

            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    ViewAddWishCurseItem item = new ViewAddWishCurseItem();
                    m_buffPlayerList.Add(item);

                    Dictionary<string, object> data = dataList[i];

                    if (data.ContainsKey("FixRateTime"))
                    {
                        item.genTime = Convert.ToDateTime(data["FixRateTime"]).ToLocalTime().ToString();
                    }
                    if (data.ContainsKey("FixRate"))
                    {
                        item.value = Convert.ToDouble(data["FixRate"]);
                    }
                    item.playerId = Convert.ToString(data["player_id"]);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////
    public class ParamPlayerOp
    {
        public string m_op;
        public string m_acc;
        public string m_prop;
        public string m_value;
    }

    // 踢玩家
    public class DyOpKickPlayer : DyOpBase
    {
        private static string[] s_retFields = { "account" };
        private List<int> m_result = new List<int>();

        public override OpRes doDyop(object param, GMUser user)
        {
            ParamPlayerOp p = (ParamPlayerOp)param;
            if (p.m_op == "logFish" || p.m_op == "getLogFishList")   //捕鱼LOG
            {
                if (!RightMgr.getInstance().canEdit(RightDef.DATA_BUYU_LOG, user))
                    return OpRes.op_res_no_right;
            }
            else
            {
                if (!RightMgr.getInstance().canEdit(RightDef.OP_PLAYER_OP, user))
                    return OpRes.op_res_no_right;
            }
            //玩家ID
            int playerId = 0;

            string acc = "";
            OpRes res = OpRes.op_res_failed;

            switch (p.m_op)
            {
                case "add":
                case "dec":
                    {
                        res = getPlayerId(p, ref playerId, ref acc, user);
                        if (res == OpRes.opres_success)
                        {
                            int val = 0;
                            if (!int.TryParse(p.m_value, out val) || val <= 0)
                                return OpRes.op_res_param_not_valid;

                            int oriVal = val;

                            if (p.m_op == "dec")
                            {
                                val = -val;
                            }

                            Dictionary<string, object> data = new Dictionary<string, object>();
                            data.Add("playerId", playerId);
                            data.Add("param", val);

                            if (p.m_prop == "gold")
                            {
                                data.Add("rtype", (int)RechargeType.gold);
                            }
                            else if (p.m_prop == "gem")
                            {
                                data.Add("rtype", (int)RechargeType.gem);
                            }
                            else if (p.m_prop == "vip")
                            {
                                data.Add("rtype", (int)RechargeType.vipExp);
                            }
                            else if (p.m_prop == "dragonBall")
                            {
                                data.Add("rtype", (int)RechargeType.dragonBall);
                            }
                            else if (p.m_prop == "chip") //碎片
                            {
                                data.Add("rtype", (int)RechargeType.chip);
                            }
                            else if (p.m_prop == "moshi") //魔石
                            {
                                data.Add("rtype", (int)RechargeType.moshi);
                            }
                            else if (p.m_prop == "xp")
                            {
                                data.Add("rtype", (int)RechargeType.playerXP);
                            }

                            bool code = DBMgr.getInstance().insertData(TableName.GM_RECHARGE, data, user.getDbServerID(), DbName.DB_PLAYER);

                            if (code)
                            {
                                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_PLAYER_OP, new LogPlayerOp(playerId, p.m_op == "add" ? 1 : 0, p.m_prop, oriVal), user);
                            }
                            else
                            {
                                res = OpRes.op_res_failed;
                            }
                        }
                    }
                    break;
                case "bind":  //绑定手机
                case "unbind":
                    {
                        res = getPlayerId(p, ref playerId, ref acc, user);
                        if (res == OpRes.opres_success)
                        {
                            string val = "";
                            if (p.m_op == "bind")
                            {
                                val = p.m_value;
                            }

                            Dictionary<string, object> data = new Dictionary<string, object>();
                            data.Add("bindPhone", val);

                            bool code = DBMgr.getInstance().update(TableName.PLAYER_INFO, data, "player_id",
                                    playerId, user.getDbServerID(), DbName.DB_PLAYER);

                            if (code)
                            {
                                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_BIND_PLAYER_PHONE, new LogBindPlayerPhone(playerId, p.m_op, val), user);
                            }
                            else
                            {
                                res = OpRes.op_res_failed;
                            }
                        }
                    }
                    break;
                case "kick":
                    {
                        res = getPlayerId(p, ref playerId, ref acc, user);
                        if (res == OpRes.opres_success)
                        {
                            res = kick(playerId, 600, user);
                        }
                    }
                    break;
                case "task":
                    {
                        ParamFullPlayerOp sp = new ParamFullPlayerOp();
                        sp.m_toServerIP = user.m_dbIP;
                        sp.m_opCode = ParamFullPlayerOp.ADD_NEW_TASK;
                        res = RemoteMgr.getInstance().reqDoService(sp, ServiceType.serviceTypeUpdatePlayerTask);
                    }
                    break;
                case "logFish":  //捕鱼LOG
                    {
                        res = getPlayerId(p, ref playerId, ref acc, user);
                        if (res == OpRes.opres_success)
                        {
                            Dictionary<string, object> data = new Dictionary<string, object>();
                            data.Add("LogFish", p.m_prop == "open" ? true : false);
                            bool code = DBMgr.getInstance().update(TableName.FISHLORD_PLAYER, data, "player_id",
                                    playerId, user.getDbServerID(), DbName.DB_GAME);
                            if (!code)
                            {
                                res = OpRes.op_res_failed;
                            }
                        }
                    }
                    break;
                case "getLogFishList":  //刷新列表
                    {
                        getLogFishPlayer(user);
                    }
                    break;
                case "LimitSendDb":
                    {
                        res = LimitDbSend(user, p);
                    }
                    break;
                case "getLimitDbSendPlayer":
                    {
                        getLimitDbSendPlayer(user);
                    }
                    break;
                case "resetGiftGuideFlag":
                    {
                        ParamFullPlayerOp sp = new ParamFullPlayerOp();
                        sp.m_toServerIP = user.m_dbIP;
                        sp.m_opCode = ParamFullPlayerOp.RESET_GIFT_GUIDE_FLAG;
                        res = RemoteMgr.getInstance().reqDoService(sp, ServiceType.serviceTypeUpdatePlayerTask);
                    }
                    break;
            }

            return res;
        }

        public override object getResult() { return m_result; }

        public static OpRes kick(int playerId, int nt, GMUser user)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["time"] = nt;
            data["key"] = playerId;

            bool res = DBMgr.getInstance().save(TableName.KICK_PLAYER, data, "key", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes getPlayerId(ParamPlayerOp p, ref int playerId, ref string acc, GMUser user)
        {
            if (!int.TryParse(p.m_acc, out playerId))
                return OpRes.op_res_param_not_valid;

            Dictionary<string, object> data = QueryBase.getPlayerProperty(playerId, user, s_retFields);
            if (data == null)
                return OpRes.op_res_player_not_exist;

            if (!data.ContainsKey("account"))
                return OpRes.op_res_player_not_exist;

            acc = Convert.ToString(data["account"]);
            if (string.IsNullOrEmpty(acc))
                return OpRes.op_res_player_not_exist;

            return OpRes.opres_success;
        }

        void getLogFishPlayer(GMUser user)
        {
            m_result.Clear();
            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            List<Dictionary<string, object>> data = DBMgr.getInstance().getDataListFromTable(TableName.FISHLORD_PLAYER,
                dip, "LogFish", true, new string[] { "player_id" });

            if (data != null)
            {
                foreach (var d in data)
                {
                    if (d.ContainsKey("player_id"))
                    {
                        m_result.Add(Convert.ToInt32(d["player_id"]));
                    }
                }
            }
        }

        OpRes LimitDbSend(GMUser user, ParamPlayerOp p)
        {
            string acc = "";
            int playerId = 0;
            OpRes res = getPlayerId(p, ref playerId, ref acc, user);
            if (res == OpRes.opres_success)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("canSendDragonBall", p.m_prop == "open" ? true : false);
                bool code = DBMgr.getInstance().update(TableName.PLAYER_INFO, data, "player_id",
                        playerId,
                        user.getDbServerID(), DbName.DB_PLAYER);
                if (!code)
                {
                    res = OpRes.op_res_failed;
                }
            }

            return res;
        }

        // 返回被限制转出龙珠的玩家
        void getLimitDbSendPlayer(GMUser user)
        {
            m_result.Clear();

            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
            List<Dictionary<string, object>> data = DBMgr.getInstance().getDataListFromTable(TableName.PLAYER_INFO,
                dip, "canSendDragonBall", false, new string[] { "player_id" });

            if (data != null)
            {
                foreach (var d in data)
                {
                    if (d.ContainsKey("player_id"))
                    {
                        m_result.Add(Convert.ToInt32(d["player_id"]));
                    }
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////
    public class ParamModifyLoginPwd
    {
        public string m_oriPwd;
        public string m_newPwd1;
        public string m_newPwd2;
    }

    // 修改GM的后台登录密码
    public class DyOpModifyGmLoginPwd : DyOpBase
    {
        public override OpRes doDyop(object param, GMUser user)
        {
            ParamModifyLoginPwd p = (ParamModifyLoginPwd)param;
            if (Tool.getMD5Hash(p.m_oriPwd) != user.m_pwd)
                return OpRes.op_res_failed;

            if (p.m_newPwd1 != p.m_newPwd2)
                return OpRes.op_res_failed;

            if (p.m_newPwd1 == "")
                return OpRes.op_res_pwd_not_valid;

            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (serverId == -1)
                return OpRes.op_res_failed;

            Dictionary<string, object> upData = new Dictionary<string, object>();
            upData.Add("password", Tool.getMD5Hash(p.m_newPwd1));
            bool res = DBMgr.getInstance().update(TableName.GM_ACCOUNT, upData, "user", user.m_user, serverId, DbName.DB_ACCOUNT);
            if (res)
            {
                user.m_pwd = Convert.ToString(upData["password"]);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    public class ParamDelData
    {
        public string m_tableName = "";
        public string m_param;
    }

    // 删除数据
    public class DyOpRemoveData : DyOpBase
    {
        private Dictionary<string, DyOpBase> m_items = new Dictionary<string, DyOpBase>();

        public DyOpRemoveData()
        {
            m_items.Add(TableName.OPLOG, new DyOpRemoveAllGmOpLog());
            m_items.Add(TableName.PUMP_DICE, new DyOpRemoveDiceTable());
        }

        public override OpRes doDyop(object param, GMUser user)
        {
            if (user.m_type != "admin")
                return OpRes.op_res_no_right;

            ParamDelData p = (ParamDelData)param;
            if (!m_items.ContainsKey(p.m_tableName))
                return OpRes.op_res_failed;

            return m_items[p.m_tableName].doDyop(param, user);
        }
    }

    // 删除所有GM操作日志
    public class DyOpRemoveAllGmOpLog : DyOpBase
    {
        public override OpRes doDyop(object param, GMUser user)
        {
            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (serverId == -1)
                return OpRes.op_res_failed;

            ParamDelData p = (ParamDelData)param;
            long id = 0;
            if (string.IsNullOrEmpty(p.m_param))
                return OpRes.op_res_param_not_valid;

            if (!long.TryParse(p.m_param, out id))
                return OpRes.op_res_param_not_valid;

            bool res = DBMgr.getInstance().remove(TableName.OPLOG, "id", id, serverId, DbName.DB_ACCOUNT);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
    }

    // 删除骰宝独立数据
    public class DyOpRemoveDiceTable : DyOpBase
    {
        public override OpRes doDyop(object param, GMUser user)
        {
            bool res = DBMgr.getInstance().removeAll(TableName.PUMP_DICE, user.getDbServerID(), DbName.DB_PUMP);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    public class ParamGameResult
    {
        public int m_gameId;
        public int m_roomId;
    }

    public class ParamGameResultCrocodile : ParamGameResult
    {
        public int m_result;
        public string m_playerId;
    }

    public class ParamGameResultDice : ParamGameResult
    {
        public int m_dice1;
        public int m_dice2;
        public int m_dice3;
    }

    // 游戏结果控制
    public class DyOpGameResult : DyOpBase
    {
        public override OpRes doDyop(object param, GMUser user)
        {
            if (!RightMgr.getInstance().canEdit(RightDef.CROD_RESULT_CONTROL, user))
                return OpRes.op_res_no_right;

            ParamGameResult p = (ParamGameResult)param;
            OpRes res = OpRes.op_res_failed;

            switch (p.m_gameId)
            {
                case (int)GameId.crocodile:
                    {
                        res = setResultCrocodile(param, user);
                    }
                    break;
                case (int)GameId.dice:
                    {
                        res = setResultDice(param, user);
                    }
                    break;
                case (int)GameId.baccarat:
                    {
                        res = setResultBaccarat(param, user);
                    }
                    break;
                case (int)GameId.shcd:
                    {
                        res = setResultShcd(param, user);
                    }
                    break;
                case (int)GameId.bz:
                    {
                        res = setResultBz(param, user);
                    }
                    break;
                case (int)GameId.fruit:
                    {
                        res = setResultFruit(param, user);
                    }
                    break;
            }

            return res;
        }

        OpRes setResultCrocodile(object param, GMUser user)
        {
            ParamGameResultCrocodile p = (ParamGameResultCrocodile)param;
            Dictionary<string, object> upData = new Dictionary<string, object>();
            upData.Add("GmIndex", p.m_result);
            bool res = DBMgr.getInstance().update(TableName.CROCODILE_ROOM, upData,
                "room_id", p.m_roomId, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_GAME_CROCODILE_RESULT, new LogGameCrocodileResult(p.m_roomId, p.m_result), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes setResultDice(object param, GMUser user)
        {
            ParamGameResultDice p = (ParamGameResultDice)param;
            Dictionary<string, object> upData = new Dictionary<string, object>();
            upData.Add("GmDice1", p.m_dice1);
            upData.Add("GmDice2", p.m_dice2);
            upData.Add("GmDice3", p.m_dice3);

            bool res = DBMgr.getInstance().update(TableName.DICE_ROOM, upData,
                "room_id", p.m_roomId, user.getDbServerID(), DbName.DB_GAME);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes setResultBaccarat(object param, GMUser user)
        {
            ParamGameResultCrocodile p = (ParamGameResultCrocodile)param;
            Dictionary<string, object> upData = new Dictionary<string, object>();
            upData.Add("GmIndex", p.m_result);
            bool res = DBMgr.getInstance().update(TableName.BACCARAT_ROOM, upData,
                "room_id", p.m_roomId, user.getDbServerID(), DbName.DB_GAME);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes setResultShcd(object param, GMUser user)
        {
            ParamGameResultCrocodile p = (ParamGameResultCrocodile)param;
            Dictionary<string, object> upData = new Dictionary<string, object>();
            upData.Add("next_card_type", p.m_result);
            upData.Add("insert_time", DateTime.Now);
            upData.Add("room_id", p.m_roomId);
            bool res = DBMgr.getInstance().insertData(TableName.SHCD_RESULT, upData, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_GAME_SHCD_RESULT, new LogGameShcdResult(p.m_roomId, p.m_result), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes setResultBz(object param, GMUser user)
        {
            ParamGameResultCrocodile p = (ParamGameResultCrocodile)param;
            Dictionary<string, object> upData = new Dictionary<string, object>();
            upData.Add("GmIndex", p.m_result);
            bool res = DBMgr.getInstance().update(TableName.DB_BZ_ROOM, upData,
                "room_id", p.m_roomId, user.getDbServerID(), DbName.DB_GAME);
            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_GAME_BZ_RESULT, new LogGameBzResult(p.m_roomId, p.m_result), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        //水果机
        OpRes setResultFruit(object param, GMUser user)
        {
            ParamGameResultCrocodile p = (ParamGameResultCrocodile)param;

            int playerId = 0;
            if (!string.IsNullOrEmpty(p.m_playerId))
            {
                if (!int.TryParse(p.m_playerId, out playerId))
                {
                    return OpRes.op_res_param_not_valid;
                }
            }
            bool res = false;
            res = DBMgr.getInstance().keyExists(TableName.PLAYER_INFO, "player_id", playerId, user.getDbServerID(), DbName.DB_PLAYER);
            if (!res)
            {
                return OpRes.op_res_player_not_exist;
            }

            res = DBMgr.getInstance().keyExists(TableName.FRUIT_RESULT_CONTROL, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            Dictionary<string, object> upData = new Dictionary<string, object>();
            upData.Add("result", p.m_result);
            upData.Add("insertTime", DateTime.Now);
            int op = 0;
            if (!res) //不存在则插入数据
            {
                upData.Add("playerId", playerId);
                res = DBMgr.getInstance().insertData(TableName.FRUIT_RESULT_CONTROL, upData, user.getDbServerID(), DbName.DB_GAME);
            }
            else
            {
                op = 1;
                res = DBMgr.getInstance().update(TableName.FRUIT_RESULT_CONTROL, upData, "playerId", playerId, user.getDbServerID(), DbName.DB_GAME);
            }

            if (res)
            {
                OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_GAME_FRUIT_RESULT, new LogGameFruitResult(op, p.m_result, p.m_playerId), user);
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

    }

    //////////////////////////////////////////////////////////////////////////
    // 大奖赛周冠军，控制
    public class ParamGrandPrixWeekChampion
    {
        // 添加保险账号
        public const int ADD_SAFE_ACCOUNT = 1;
        // 移除保险账号
        public const int REMOVE_SAFE_ACCOUNT = 2;
        // 修改最好成绩
        public const int MODIFY_BEST_SCORE = 3;
        // 现有安全账号
        public const int CUR_SAFE_ACCOUNT = 4;

        public int m_op;
        public string m_param;
        public string m_score;

        public string m_retNickName;
    }

    public class ResultSafeAccItem
    {
        public int m_playerId;
        public string m_acc;
        public string m_nickName;
    }

    // 大奖赛周冠军控制
    public class DyOpWeekChampionControl : DyOpBase
    {
        private List<ResultSafeAccItem> m_result = new List<ResultSafeAccItem>();

        public override OpRes doDyop(object param, GMUser user)
        {
            if (!RightMgr.getInstance().canEdit(RightDef.OP_WEEK_CHAMPION_SETTING, user))
                return OpRes.op_res_no_right;

            ParamGrandPrixWeekChampion p = (ParamGrandPrixWeekChampion)param;
            OpRes res = OpRes.op_res_failed;
            switch (p.m_op)
            {
                case ParamGrandPrixWeekChampion.ADD_SAFE_ACCOUNT:
                    {
                        res = addSafeAccount(p, user);
                    }
                    break;
                case ParamGrandPrixWeekChampion.REMOVE_SAFE_ACCOUNT:
                    {
                        res = removeSafeAccount(p, user);
                    }
                    break;
                case ParamGrandPrixWeekChampion.MODIFY_BEST_SCORE:
                    {
                        res = modifyBestScore(p, user);
                    }
                    break;
                case ParamGrandPrixWeekChampion.CUR_SAFE_ACCOUNT:
                    {
                        m_result.Clear();
                        res = queryCurSafeAcc(user);
                    }
                    break;
            }
            return res;
        }

        public override object getResult() { return m_result; }

        OpRes addSafeAccount(ParamGrandPrixWeekChampion p, GMUser user)
        {
            int playerId;
            if (!int.TryParse(p.m_param, out playerId))
            {
                return OpRes.op_res_param_not_valid;
            }
            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
            Dictionary<string, object> qd = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", playerId, new string[] { "nickname", "is_robot" }, dip);
            if (qd == null)
                return OpRes.op_res_player_not_exist;

            bool isRobot = Convert.ToBoolean(qd["is_robot"]);
            if (isRobot)
                return OpRes.op_res_failed;

            DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            bool res = DBMgr.getInstance().keyExists(TableName.MATCH_GRAND_SAFE_ACCOUNT, "playerId", playerId, dip_1);
            if (res)
                return OpRes.op_res_failed;

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("playerId", playerId);
            data.Add("bestScore", 0);
            data.Add("nickName", Convert.ToString(qd["nickname"]));
            p.m_retNickName = Convert.ToString(qd["nickname"]);
            res = DBMgr.getInstance().insertData(TableName.MATCH_GRAND_SAFE_ACCOUNT, data, user.getDbServerID(), DbName.DB_GAME);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes removeSafeAccount(ParamGrandPrixWeekChampion p, GMUser user)
        {
            int playerId;
            if (!int.TryParse(p.m_param, out playerId))
            {
                return OpRes.op_res_param_not_valid;
            }

            bool res = DBMgr.getInstance().remove(TableName.MATCH_GRAND_SAFE_ACCOUNT, "playerId", playerId,
                user.getDbServerID(), DbName.DB_GAME);

            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes modifyBestScore(ParamGrandPrixWeekChampion p, GMUser user)
        {
            int playerId;
            if (!int.TryParse(p.m_param, out playerId))
            {
                return OpRes.op_res_param_not_valid;
            }

            int score = 0;
            if (!int.TryParse(p.m_score, out score))
            {
                return OpRes.op_res_param_not_valid;
            }

            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            Dictionary<string, object> data1 = DBMgr.getInstance().getTableData(TableName.MATCH_GRAND_SAFE_ACCOUNT, "playerId", playerId, dip);
            if (data1 == null)
                return OpRes.op_res_player_not_exist;

            DateTime startT = DateTime.Now, endT = DateTime.Now;
            calMatchCycle(DateTime.Now.Date, ref startT, ref endT);

            IMongoQuery imq1 = Query.LTE("matchTime", BsonValue.Create(endT));
            IMongoQuery imq2 = Query.GTE("matchTime", BsonValue.Create(startT));
            IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("playerId", playerId));

            Dictionary<string, object> data2 = DBMgr.getInstance().getTableData(TableName.MATCH_GRAND_PRIX_DAY, "playerId", playerId, new string[] { "bestScore" }, dip);
            if (data2 == null) // 需要参加一场比赛，才可以改成绩
                return OpRes.op_res_not_join_match;

            int curScore = Convert.ToInt32(data2["bestScore"]);
            if (score < curScore)
                return OpRes.op_res_score_lower;

            Dictionary<string, object> upData = new Dictionary<string, object>();
            upData.Add("bestScore", score);
            bool res = DBMgr.getInstance().update(TableName.MATCH_GRAND_SAFE_ACCOUNT, upData, "playerId", playerId, dip);
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        void calMatchCycle(DateTime curDate, ref DateTime startT, ref DateTime endT)
        {
            int curWeekDay = (int)curDate.DayOfWeek;
            if (curWeekDay == 1)
            {
                startT = curDate.AddDays(-6);
                endT = curDate;
            }
            else
            {
                if (curWeekDay == 0)
                {
                    curWeekDay = 7;
                }

                startT = curDate.AddDays(2 - curWeekDay);
                endT = curDate.AddDays(8 - curWeekDay);
            }
        }

        OpRes queryCurSafeAcc(GMUser user)
        {
            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.MATCH_GRAND_SAFE_ACCOUNT, dip);

            if (dataList == null)
                return OpRes.opres_success;

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];
                if (data.ContainsKey("playerId") && data.ContainsKey("nickName"))
                {
                    ResultSafeAccItem tmp = new ResultSafeAccItem();
                    m_result.Add(tmp);

                    tmp.m_playerId = Convert.ToInt32(data["playerId"]);
                    tmp.m_nickName = Convert.ToString(data["nickName"]);
                }
            }

            return OpRes.opres_success;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    public class ResultBossItem
    {
        public int m_roomId;
        public int m_maxBossCount;
        public int m_createBossRand;
    }

    // boss控制
    public class ParamFishBossControl : ResultBossItem
    {
        public const int MODIFY_ROOM_BOSS = 1;  // 修改房间BOSS数量
        public const int VIEW_ROOM_BOSS = 2;    // 查看房间BOSS

        public int m_op;
        public string m_roomList;
    }

    // 设置BOSS最大数量，及出现概率
    public class DyOpFishBoss : DyOpBase
    {
        static string[] s_fields = { "room_id", "MaxBossCount", "CreateBossRand" };
        public List<ResultBossItem> m_result = new List<ResultBossItem>();

        public override OpRes doDyop(object param, GMUser user)
        {
            if (!RightMgr.getInstance().canEdit(RightDef.FISH_BOSS_CONTROL, user))
                return OpRes.op_res_no_right;

            ParamFishBossControl p = (ParamFishBossControl)param;
            OpRes res = OpRes.op_res_failed;
            switch (p.m_op)
            {
                case ParamFishBossControl.MODIFY_ROOM_BOSS:
                    {
                        res = modifyBoss(p, user);
                    }
                    break;
                case ParamFishBossControl.VIEW_ROOM_BOSS:
                    {
                        query(user);
                    }
                    break;
            }
            return res;
        }

        public override object getResult() { return m_result; }

        OpRes modifyBoss(ParamFishBossControl p, GMUser user)
        {
            Dictionary<string, object> updata = new Dictionary<string, object>();
            updata.Add("MaxBossCount", p.m_maxBossCount);
            updata.Add("CreateBossRand", p.m_createBossRand);

            string[] ids = Tool.split(p.m_roomList, ',', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ids.Length; i++)
            {
                int id = Convert.ToInt32(ids[i]);
                DBMgr.getInstance().update(TableName.FISHLORD_ROOM, updata, "room_id", id,
                        user.getDbServerID(), DbName.DB_GAME);
            }
            return OpRes.opres_success;
        }

        OpRes query(GMUser user)
        {
            m_result.Clear();

            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
            List<Dictionary<string, object>> dataList =
                DBMgr.getInstance().executeQuery(TableName.FISHLORD_ROOM, dip, null, 0, 0, s_fields, "room_id", true);

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];
                ResultBossItem item = new ResultBossItem();
                m_result.Add(item);
                item.m_roomId = Convert.ToInt32(data["room_id"]);
                if (data.ContainsKey("MaxBossCount"))
                {
                    item.m_maxBossCount = Convert.ToInt32(data["MaxBossCount"]);
                }
                if (data.ContainsKey("CreateBossRand"))
                {
                    item.m_createBossRand = Convert.ToInt32(data["CreateBossRand"]);
                }
            }

            return OpRes.opres_success;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    public class ParamGmTypeEdit
    {
        public int m_op;
        public string m_param;
        public string m_newValue;
    }

    // gm账号类型编辑
    public class DyOpGmTypeEdit : DyOpBase
    {
        private static string[] s_retFields = { "account" };
        List<Dictionary<string, object>> m_result = new List<Dictionary<string, object>>();

        public override OpRes doDyop(object param, GMUser user)
        {
            ParamGmTypeEdit p = (ParamGmTypeEdit)param;
            OpRes res = OpRes.op_res_failed;
            switch (p.m_op)
            {
                case DefCC.OP_ADD:
                    {
                        res = add(p, user);
                    }
                    break;
                case DefCC.OP_VIEW:
                    {
                        loadGm(user);
                    }
                    break;
                case DefCC.OP_MODIFY:
                    {
                        res = modify(p, user);
                    }
                    break;
                case DefCC.OP_REMOVE:
                    {
                        res = remove(p, user);
                    }
                    break;
            }

            return res;
        }

        public override object getResult() { return m_result; }

        OpRes add(ParamGmTypeEdit p, GMUser user)
        {
            if (string.IsNullOrEmpty(p.m_param))
                return OpRes.op_res_param_not_valid;

            bool res = hasTypeName(p.m_param);
            if (res)
                return OpRes.op_res_data_duplicate;

            Dictionary<string, object> data = new Dictionary<string, object>();
            data["typeName"] = p.m_param;
            data["id"] = ObjectId.GenerateNewId().ToString();
            data["genTime"] = DateTime.Now;

            p.m_newValue = Convert.ToString(data["id"]);
            res = DBMgr.getInstance().store(TableName.GM_TYPE, data, "typeName", p.m_param, 0, DbName.DB_ACCOUNT);
            if (res)
            {
                RightMgr.getInstance().readRightSet();
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes modify(ParamGmTypeEdit p, GMUser user)
        {
            if (string.IsNullOrEmpty(p.m_param) || string.IsNullOrEmpty(p.m_newValue))
                return OpRes.op_res_param_not_valid;

            bool res = hasTypeName(p.m_newValue);
            if (res)
                return OpRes.op_res_data_duplicate;

            Dictionary<string, object> data = new Dictionary<string, object>();
            data["typeName"] = p.m_newValue;

            res = DBMgr.getInstance().update(TableName.GM_TYPE, data, "id", p.m_param, 0, DbName.DB_ACCOUNT);
            if (res)
            {
                RightMgr.getInstance().readRightSet();
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        OpRes remove(ParamGmTypeEdit p, GMUser user)
        {
            if (string.IsNullOrEmpty(p.m_param))
                return OpRes.op_res_param_not_valid;

            if (p.m_param.Length < 24)
                return OpRes.op_res_failed;

            bool res = DBMgr.getInstance().remove(TableName.GM_TYPE, "id", p.m_param, 0, DbName.DB_ACCOUNT);
            if (res)
            {
                RightMgr.getInstance().readRightSet();
            }
            return res ? OpRes.opres_success : OpRes.op_res_failed;
        }

        void loadGm(GMUser user)
        {
            m_result = DBMgr.getInstance().executeQuery(TableName.GM_TYPE,
                0, DbName.DB_ACCOUNT, null, 0, 0, new string[] { "typeName", "id" });
        }

        bool hasTypeName(string type)
        {
            return DBMgr.getInstance().keyExists(TableName.GM_TYPE, "typeName", type, 0, DbName.DB_ACCOUNT);
        }
    }