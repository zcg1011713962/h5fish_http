using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

// ��������
public class ExportMgr
{
    private Dictionary<string, ExportExcelBase> m_tables = new Dictionary<string, ExportExcelBase>();

    public string m_exportDir = "";

    public void init()
    {
        m_tables.Add(ExportRecharge.TABLE_NAME, new ExportRecharge());
        m_tables.Add(TableName.RECHARGE_TOTAL, new ExportRechargeNew()); //��ֵ��ѯ
        m_tables.Add(TableName.PUMP_PLAYER_MONEY, new ExportMoney());

        // ÿ������
        m_tables.Add(TableName.PUMP_DAILY_TASK, new ExportDailyTask());
        // �ɾ�
        m_tables.Add(TableName.PUMP_TASK, new ExportAchievement());
        // ���Ԥ������
        m_tables.Add(TableName.PLAYER_INFO, new ExportMoneyWarn());
        // ��ֵ�û�ͳ��
        m_tables.Add(TableName.PUMP_RECHARGE_PLAYER, new ExportRechargePlayer());
        // ��ֵ��Ҽ��
        m_tables.Add(TableName.PUMP_RECHARGE_FIRST, new ExportRechargePlayerMonitor());
        m_tables.Add(TableName.STAT_DRAGON_DAILY, new ExportDragonBallDaily());
        m_tables.Add(TableName.STAT_PLAYER_DRAGON, new ExportPlayerDragonBallMonitor());

        m_tables.Add(TableName.GIFT_CODE, new ExportCDKEY());

        //�ͷ�����/�����/����������ʷ����
        m_tables.Add(TableName.PUMP_REPAIR_ORDER,new ExportRepairOrder()); 

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



