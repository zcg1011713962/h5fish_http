using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

// 导出管理
public class ExportMgr
{
    private Dictionary<string, ExportExcelBase> m_tables = new Dictionary<string, ExportExcelBase>();

    public string m_exportDir = "";

    public void init()
    {
        m_tables.Add(ExportRecharge.TABLE_NAME, new ExportRecharge());
        m_tables.Add(TableName.RECHARGE_TOTAL, new ExportRechargeNew()); //充值查询
        m_tables.Add(TableName.PUMP_PLAYER_MONEY, new ExportMoney());

        // 每日任务
        m_tables.Add(TableName.PUMP_DAILY_TASK, new ExportDailyTask());
        // 成就
        m_tables.Add(TableName.PUMP_TASK, new ExportAchievement());
        // 金币预警导出
        m_tables.Add(TableName.PLAYER_INFO, new ExportMoneyWarn());
        // 充值用户统计
        m_tables.Add(TableName.PUMP_RECHARGE_PLAYER, new ExportRechargePlayer());
        // 充值玩家监控
        m_tables.Add(TableName.PUMP_RECHARGE_FIRST, new ExportRechargePlayerMonitor());
        m_tables.Add(TableName.STAT_DRAGON_DAILY, new ExportDragonBallDaily());
        m_tables.Add(TableName.STAT_PLAYER_DRAGON, new ExportPlayerDragonBallMonitor());

        m_tables.Add(TableName.GIFT_CODE, new ExportCDKEY());

        //客服补单/大户随访/换包福利历史操作
        m_tables.Add(TableName.PUMP_REPAIR_ORDER,new ExportRepairOrder()); 

        //玩家炮数成长分布
        m_tables.Add(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, new ExportPlayerActivityTurret());

        XmlConfig xml = ResMgr.getInstance().getRes("dbserver.xml");
        m_exportDir = xml.getString("exportDir", "");
    }

    public void export(ExportParam param)
    {
        if (!m_tables.ContainsKey(param.m_tableName))
            return;

        m_tables[param.m_tableName].exportExcel(param, m_exportDir);
    }
}



