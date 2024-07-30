using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public interface ITable
{
    FieldCsv fetch(int col);
    FieldCsv fetch(string col_name);
}

public interface IUserTabe
{
    // ��ʼ��ȡ
    void beginRead();
    // ��ȡ���е�һ��
    void readLine(ITable table);
    // ������ȡ
    void endRead();
}

public class FieldCsv
{
    // ��������
    private string m_content;

    public FieldCsv(string c)
    {
        m_content = c;
    }

    public int toInt()
    {
        // �ַ���Ϊ�գ�����Ĭ��0
        if (m_content == string.Empty)
            return 0;

        int t = 0;
        try
        {
            t = Convert.ToInt32(m_content);
        }
        catch (Exception)
        {
            LOG.Info("���������ֵʱʧ��");
        }
        return t;
    }

    public float toFloat()
    {
        // �ַ���Ϊ�գ�����Ĭ��0
        if (m_content == string.Empty)
            return 0.0f;

        float t = 0.0f;
        try
        {
            t = (float)Convert.ToDouble(m_content);
        }
        catch (Exception)
        {
            LOG.Info("����񸡵�ֵʱʧ��");
        }
        return t;
    }

    public byte toByte()
    {
        if (m_content == string.Empty)
            return 0;

        byte t = 0;
        try
        {
            t = Convert.ToByte(m_content);
        }
        catch (Exception)
        {
            LOG.Info("�����byteֵʱʧ��!");
        }
        return t;
    }

    public string toStr()
    {
        return m_content;
    }
}

public class Csv
{
    public static bool load<T>(string file, T obj, char end_flag = ' ') where T : IUserTabe
    {
        if (obj == null)
        {
            LOG.Info("�����˿յ�����");
            return false;
        }

        CsvReader cr = new CsvReader();
        if (!cr.load(file, end_flag))
            return false;

        int row = 0;
        cr.gotoLine(row);
        obj.beginRead();
        while (!cr.isFinish())
        {
            obj.readLine(cr);
            row++;
            cr.gotoLine(row);
        }
        obj.endRead();
        return true;
    }

    public static bool loadXml<T>(string file, T obj) where T : IUserTabe
    {
        if (obj == null)
        {
            LOG.Info("loadXml �����˿յ�����");
            return false;
        }

        XmlReader cr = new XmlReader();
        if (!cr.load(file))
            return false;

        int row = 0;
        cr.gotoLine(row);
        obj.beginRead();
        while (!cr.isFinish())
        {
            obj.readLine(cr);
            row++;
            cr.gotoLine(row);
        }
        obj.endRead();
        return true;
    }
}

public class CsvReader : ITable
{
    // �洢����еĸ�������
    private Dictionary<int, List<FieldCsv>> m_contents = new Dictionary<int, List<FieldCsv>>();
    // �洢�������������Ķ�Ӧ��ϵ
    private Dictionary<string, int> m_nameToCol = new Dictionary<string, int>();
    // ������
    private int m_totalRows;
    // ��ǰ��
    private int m_curRow;

    public bool load(string file, char end_flag)
    {
        if (!File.Exists(file))
        {
            LOG.Info("�ļ�[{0}]������!", file);
            return false;
        }

        StreamReader sr = new StreamReader(file, Encoding.Default);
        string line = sr.ReadLine();
        if (line != null)
        {
            if (!init(line))
                return false;
        }

        int row = 0;
        line = readLine(sr, end_flag);//sr.ReadLine();
        while (line != null)
        {
            if (!isEmptyLine(line))
            {
                addLine(line, row);
                row++;
            }
            line = readLine(sr, end_flag); //sr.ReadLine();
        }
        m_totalRows = row;
        sr.Close();
        return true;
    }

    public FieldCsv fetch(int col)
    {
        return get(m_curRow, col);
    }

    public FieldCsv fetch(string col_name)
    {
        if(!m_nameToCol.ContainsKey(col_name))
        {
            LOG.Info("���������[{0}]������!", col_name);
            return null;
        }
        int col = m_nameToCol[col_name];
        return get(m_curRow, col);
    }

    // �Ƶ�ĳ��
    public void gotoLine(int row)
    {
        m_curRow = row;
    }

    // �Ƿ������
    public bool isFinish()
    {
        return m_curRow >= m_totalRows;
    }

    private FieldCsv get(int row, int col)
    {
        if (m_contents.ContainsKey(row))
        {
            List<FieldCsv> list = m_contents[row];
            return list[col];
        }
        return null;
    }

    // ��ʼ����ͷ
    private bool init(string line)
    {
        m_nameToCol.Clear();
        string[] strs = parseLine(line);
        for (int i = 0; i < strs.Length; i++)
        {
            if (m_nameToCol.ContainsKey(strs[i]))
            {
                LOG.Info("��ȡ�����󣬰�������ͬ������!");
                return false;
            }
            m_nameToCol[strs[i]] = i;
        }
        return true;
    }

    // ����һ������
    private void addLine(string line, int line_num)
    {
        // lineΪ�հ�ʱ�����Ա���
        if (line == "")
            return;
        string[] strs = parseLine(line);
        List<FieldCsv> arr = null;
        if (m_contents.ContainsKey(line_num))
        {
            arr = m_contents[line_num];
        }
        else
        {
            arr = new List<FieldCsv>();
            m_contents[line_num] = arr;
        }
        for (int i = 0; i < strs.Length; i++)
        {
            arr.Add(new FieldCsv(strs[i]));
        }

        int count = m_nameToCol.Count;
        count -= strs.Length;
        // ����wps������csv��excel�������е㲻һ��
        // �����ұ�ĳ�����ӣ���ʼ����д�κ�����ʱ��д��Ķ�����������еĸ���������
        // ���ﲹ���£�����ɿմ�
        for (int i = 0; i < count; i++)
        {
            arr.Add(new FieldCsv(""));
        }
    }

    // �������е�һ������
    private string[] parseLine(string line)
    {
        int pos = line.IndexOf('"');
        if (pos == -1)  // ���治��˫���ţ����������
        {
            char[] sp = { ',' };
            string[] strs = line.Split(sp);
            return strs;
        }
            
        StringBuilder sb = new StringBuilder(64);
        // �洢�������ĸ�����
        List<string> res = new List<string>();
        int state = 0;
        int i = 0;
        while (i < line.Length)
        {
            switch (state)
            {
            case 0:
                {
                    if (line[i] == '"') // ������˫����ת״̬
                    {
                        state = 1;
                    }
                    else if (line[i] == ',') // �������ţ�˵��һ������ʶ�������
                    {
                        res.Add(sb.ToString());
                        sb.Remove(0, sb.Length);
                    }
                    else // �������
                    {
                        sb.Append(line[i]);
                    }
                    i++;
                }
                break;
            case 1:
                {
                    // ȡһ��˫����
                    if (i + 1 < line.Length && line[i] == '"' && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i += 2;
                    }
                    // һ�����ʽ�����
                    else if (line[i] == '"' && (i + 1 >= line.Length || line[i + 1] == ','))
                    {
                        res.Add(sb.ToString());
                        sb.Remove(0, sb.Length);
                        i += 2;
                        state = 0;
                    }
                    else
                    {
                        sb.Append(line[i]);
                        i++;
                    }
                }
                break;
            }
        }
        if (sb.Length != 0)
        {
            res.Add(sb.ToString());
        }
        return res.ToArray();
    }

    // �Ƿ���У�ֻ���ո񣬻�ֻ��,
    private bool isEmptyLine(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] != ' ' && line[i] != ',')
                return false;
        }
        return true;
    }

    // ��ȡһ�����ݣ�csv��һ�����ݣ���,����
    // end_flag�ǽ������
    private string readLine(StreamReader sr, char end_flag = '$' )
    {
        string res = sr.ReadLine();
        // ���ǿո�ʱ����ʾʹ�ý������
        if (end_flag != ' ' && res != null)
        {
            while (res[res.Length - 1] != end_flag)
            {
                res += sr.ReadLine();
            }
        }

        return res;
    }
}

public class XmlReader : ITable
{
    private List<List<FieldCsv>> m_content = new List<List<FieldCsv>>();
    private string m_fileName = "";
    // �洢�������������Ķ�Ӧ��ϵ
    private Dictionary<string, int> m_nameToCol = new Dictionary<string, int>();
    // ������
    private int m_totalRows;
    // ��ǰ��
    private int m_curRow;

    public bool load(string file)
    {
        if (!File.Exists(file))
        {
            LOG.Info("�ļ�[{0}]������!", file);
            return false;
        }
        m_fileName = file;
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        m_content.Clear();
        m_nameToCol.Clear();
        m_totalRows = 0;
        parse(doc);
        return true;
    }

    public FieldCsv fetch(int col)
    {
        return get(m_curRow, col);
    }

    public FieldCsv fetch(string col_name)
    {
        if (!m_nameToCol.ContainsKey(col_name))
        {
            LOG.Info("���������[{0}]������!", col_name);
            return null;
        }
        int col = m_nameToCol[col_name];
        return get(m_curRow, col);
    }

    // �Ƶ�ĳ��
    public void gotoLine(int row)
    {
        m_curRow = row;
    }

    // �Ƿ������
    public bool isFinish()
    {
        return m_curRow >= m_totalRows;
    }

    private FieldCsv get(int row, int col)
    {
        if (row < 0 || row >= m_content.Count)
        {
            LOG.Info("����������Χ[{0}]!", row);
            return null;
        }
        List<FieldCsv> list = m_content[row];
        if (col < 0 || col >= list.Count)
        {
            LOG.Info("����������Χ[{0}]!", col);
            return null;
        }
        return list[col];
    }

    private void parse(XmlDocument doc)
    {
        XmlNode node = doc.SelectSingleNode("/configuration");
        for (node = node.FirstChild; node != null; node = node.NextSibling)
        {
            if (node is XmlComment)  // ����ע��
                continue;

            List<FieldCsv> res = parseOneItem(node);
            if (res.Count > 0)
            {
                m_content.Add(res);
                m_totalRows++;
            }
        }
    }

    private List<FieldCsv> parseOneItem(XmlNode node)
    {
        List<FieldCsv> result = new List<FieldCsv>();

        int i = 0;
        for (XmlNode tmp = node.FirstChild; tmp != null; tmp = tmp.NextSibling)
        {
            if (tmp is XmlComment)  // ����ע��
                continue;

            if (tmp.FirstChild != null)
            {
                result.Add(new FieldCsv(tmp.FirstChild.Value));
            }
            else
            {
                result.Add(new FieldCsv(""));
            }

            if (!m_nameToCol.ContainsKey(tmp.Name))
            {
                m_nameToCol[tmp.Name] = i;
                i++;
            }
        }
        return result;
    }
}
