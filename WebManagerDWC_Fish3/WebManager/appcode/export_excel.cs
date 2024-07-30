using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using System.Web.Configuration;

public enum ExportType
{
    //玩家道具获取详情
    exportTypePlayerItemRecord,

    //vip玩家信息
    exportTypeVipPlayerInfo,

    //客服补单/大户随访/换包福利导出excel
    exportTypeRepairOrder,

    // 充值记录的导出
    exportTypeRecharge,

    exportTypeRechargeNew,

    // 玩家金币，钻石的变化情况
    exportTypeMoney,

    // 成就导出
    exportTypeAchievement,

    // 导出金币预警
    exportTypeMoneyWarn,
    
    // 充值用户
    exportTypeRechargePlayer,

    // 付费玩家监控
    exportTypeRechargePlayerMonitor,
    // 每日龙珠
    exportTypeDragonBallDaily,
    // 玩家龙珠监控
    exportTypeStatPlayerDragonBall,
    exportTypeCDKEY,
    
    //玩家炮数成长分布
    exportTypeOperationPlayerActTurretBySingle,
}

// 导出Excel管理
public class ExportMgr : SysBase
{
    private Dictionary<ExportType, ExportBase> m_items = new Dictionary<ExportType, ExportBase>();

    public ExportMgr()
    {
        m_sysType = SysType.sysTypeExport;
    }

    public OpRes doExport(object param, ExportType exportName, GMUser user)
    {
        if (!m_items.ContainsKey(exportName))
        {
            LOGW.Info("不存在名称为[{0}]的导出", exportName);
            return OpRes.op_res_failed;
        }
        return m_items[exportName].doExport(param, user);
    }

    public override void initSys()
    {
        m_items.Add(ExportType.exportTypeRecharge, new ExportRecharge());
        m_items.Add(ExportType.exportTypeRechargeNew, new ExportRechargeNew());  //充值查询导出（新）
        m_items.Add(ExportType.exportTypeMoney, new ExportMoney());
        m_items.Add(ExportType.exportTypeMoneyWarn, new ExportMoneyWarn());
        m_items.Add(ExportType.exportTypeRechargePlayer, new ExportRechagePlayer());
        m_items.Add(ExportType.exportTypeRechargePlayerMonitor, new ExportRechargePlayerMonitor()); //新进玩家付费监控

        m_items.Add(ExportType.exportTypeDragonBallDaily, new ExportDragonBallDaily());
        m_items.Add(ExportType.exportTypeStatPlayerDragonBall, new ExportPlayerDragonBallMonitor());
        m_items.Add(ExportType.exportTypeCDKEY, new ExportCDKEY());

        m_items.Add(ExportType.exportTypeRepairOrder,new ExportRepairOrder());//客服补单/大户随访/换包福利 操作历史查询

        m_items.Add(ExportType.exportTypeOperationPlayerActTurretBySingle, new ExportOperationPlayerActTurretBySingle());//玩家炮数成长分布
        m_items.Add(ExportType.exportTypeVipPlayerInfo, new ExportVipPlayerInfo()); //vip玩家信息导出
        m_items.Add(ExportType.exportTypePlayerItemRecord, new ExportPlayerItemRecord()); //玩家道具获取详情
    }
}

///////////////////////////////////////////////////////////////////////////////

public class ExportBase
{
    protected QueryCondition m_cond = new QueryCondition();

    public virtual OpRes doExport(object param, GMUser user) { return OpRes.op_res_failed; }
}

///////////////////////////////////////////////////////////////////////////////

// 导出玩家的充值记录
public class ExportRecharge : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
        OpRes res = mgr.makeQuery(param, QueryType.queryTypeRecharge, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = WebConfigurationManager.AppSettings["payment"];
        ep.m_tableName = "recharge";
        ep.m_condition = m_cond.getCond();
        // 当前用户操作的DB服务器
        ep.m_condition.Add("userOpDbIp", user.m_dbIP);
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}
///////////////////////////////////////////////////////////////////////////////
// 导出玩家的充值记录（新）
public class ExportRechargeNew : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
        OpRes res = mgr.makeQuery(param, QueryType.queryTypeRechargeNew, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = WebConfigurationManager.AppSettings["account"];
        ep.m_tableName = TableName.RECHARGE_TOTAL;
        ep.m_condition = m_cond.getCond();
        // 当前用户操作的DB服务器
        ep.m_condition.Add("userOpDbIp", user.m_dbIP);
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}
///////////////////////////////////////////////////////////////////////////////
//客服补单/大户随访/换包福利 操作历史查询
public class ExportRepairOrder : ExportBase 
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
        OpRes res = mgr.makeQuery(param, QueryType.queryTypeRepairOrder,user,m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.PUMP_REPAIR_ORDER;
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}
////////////////////////////////////////////////////////////////////////////////
//vip玩家信息
public class ExportVipPlayerInfo : ExportBase 
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);

        OpRes res = mgr.makeQuery(param, QueryType.queryTypeSelectLostPlayers, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.PLAYER_INFO+"_vip";
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}
///////////////////////////////////////////////////////////////////////////////
//玩家道具获取详情
public class ExportPlayerItemRecord : ExportBase 
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);

        OpRes res = mgr.makeQuery(param, QueryType.queryTypePlayerItemRecord, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.PUMP_PLAYER_ITEM;
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}
///////////////////////////////////////////////////////////////////////////////
//玩家炮数成长分布
public class ExportOperationPlayerActTurretBySingle : ExportBase 
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);

        OpRes res = mgr.makeQuery(param, QueryType.queryTypeOperationPlayerActTurretBySingle, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET;
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}
///////////////////////////////////////////////////////////////////////////////
// 导出金币变化记录
public class ExportMoney : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
        OpRes res = mgr.makeQuery(param, QueryType.queryTypeMoneyDetail, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.PUMP_PLAYER_MONEY;
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}

///////////////////////////////////////////////////////////////////////////////
// 金币预警导出
public class ExportMoneyWarn : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        // 这个服务器上的成就
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.PLAYER_INFO;
        ep.m_condition = new Dictionary<string, object>();
        ep.m_condition.Add("sel", (int)p.m_way);
        ep.m_condition.Add("count", (int)p.m_countEachPage);
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}
///////////////////////////////////////////////////////////////////////////////
// 充值用户统计
public class ExportRechagePlayer : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        StatMgr mgr = user.getSys<StatMgr>(SysType.sysTypeStat);
        OpRes res = mgr.makeQuery(param, StatType.statTypeRechargePlayer, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.PUMP_RECHARGE_PLAYER_NEW;
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}

///////////////////////////////////////////////////////////////////////////////
// 付费玩家监控导出
public class ExportRechargePlayerMonitor : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
        OpRes res = mgr.makeQuery(param, QueryType.queryTypeRechargePlayerMonitor, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.PUMP_RECHARGE_FIRST;
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}

///////////////////////////////////////////////////////////////////////////////
// 每日龙珠导出
public class ExportDragonBallDaily : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
        OpRes res = mgr.makeQuery(param, QueryType.queryTypeDragonBallDaily, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.STAT_DRAGON_DAILY;
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}

///////////////////////////////////////////////////////////////////////////////
// 每日龙珠导出
public class ExportPlayerDragonBallMonitor : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        m_cond.startExport();
        StatMgr mgr = user.getSys<StatMgr>(SysType.sysTypeStat);
        OpRes res = mgr.makeQuery(param, StatType.statTypePlayerDragonBall, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = user.m_dbIP;
        ep.m_tableName = TableName.STAT_PLAYER_DRAGON;
        ep.m_condition = m_cond.getCond();
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}

///////////////////////////////////////////////////////////////////////////////
// CDKEY导出
public class ExportCDKEY : ExportBase
{
    public override OpRes doExport(object param, GMUser user)
    {
        string p = (string)param;
        int pici = 0;
        if (!int.TryParse(p, out pici))
            return OpRes.op_res_param_not_valid;

        ExportParam ep = new ExportParam();
        ep.m_account = user.m_user;
        ep.m_dbServerIP = WebConfigurationManager.AppSettings["account"];
        ep.m_tableName = TableName.GIFT_CODE;
        ep.m_condition = new Dictionary<string, object>();
        ep.m_condition.Add("pici", pici);
        return RemoteMgr.getInstance().reqExportExcel(ep);
    }
}

