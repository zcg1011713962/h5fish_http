using System;
using System.Data.OleDb;
using System.Web;

public class AccessDb
{
    private static AccessDb s_obj = null;
    private const string CONNECT_STR = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}";
    private OleDbConnection m_cnn = null;

    // ���Ӵ�
    private string m_cnnStr = "";

    public static AccessDb getAccDb()
    {
        if (s_obj == null)
        {
            s_obj = new AccessDb();
        }
        return s_obj;
    }

    public void setConnDb(string dbName)
    {
        string str = HttpRuntime.BinDirectory + "..\\accdb\\" + dbName;
        m_cnnStr = string.Format(CONNECT_STR, str);
    }

    public OleDbDataReader startQuery(string sql)
    {
        try
        {
            m_cnn = new OleDbConnection(m_cnnStr);
            m_cnn.Open();
            OleDbCommand cmd = new OleDbCommand(sql, m_cnn);
            return cmd.ExecuteReader();
        }
        catch (System.Exception ex)
        {
            LOGW.Info("��AccessDB���ݿ�[{0}]ʧ�ܣ�sql���[{1}], �쳣��Ϣ[{2}]", m_cnnStr, sql, ex.ToString());
        }
        return null;
    }

    public int startOp(string sql)
    {
        try
        {
            m_cnn = new OleDbConnection(m_cnnStr);
            m_cnn.Open();
            OleDbCommand cmd = new OleDbCommand(sql, m_cnn);
            return cmd.ExecuteNonQuery();
        }
        catch (System.Exception ex)
        {
            LOGW.Info("��AccessDB���ݿ�[{0}]ʧ�ܣ�sql���[{1}], �쳣��Ϣ[{2}]", m_cnnStr, sql, ex.ToString());
        }
        return -1;
    }

    public void end()
    {
        if (m_cnn != null)
        {
            m_cnn.Close();
            m_cnn = null;
        }
    }
}




