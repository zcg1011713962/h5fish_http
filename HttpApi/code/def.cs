using System.Web.Configuration;

public struct DEF
{
    // 小啄
    public static string START_TIME_XIAOZHUO = WebConfigurationManager.AppSettings["START_TIME_XIAOZHUO"];//"20210329";
    public static string END_TIME_XIAOZHUO = WebConfigurationManager.AppSettings["END_TIME_XIAOZHUO"];//"20210429";

    // 聚享游
    public static string START_TIME_JUXIANGYOU = WebConfigurationManager.AppSettings["START_TIME_JUXIANGYOU"];//"20210421";// "20210305";
    public static string END_TIME_JUXIANGYOU = WebConfigurationManager.AppSettings["END_TIME_JUXIANGYOU"];//"20210523";//"20210406";

    // 麦子赚
    public static string START_TIME_MAIZIZHUAN = WebConfigurationManager.AppSettings["START_TIME_MAIZIZHUAN"];//"20210329";
    public static string END_TIME_MAIZIZHUAN = WebConfigurationManager.AppSettings["END_TIME_MAIZIZHUAN"];//"20210429";

    // 葫芦星球
    public static string START_TIME_HULU = WebConfigurationManager.AppSettings["START_TIME_HULU"];//"20210330";
    public static string END_TIME_HULU = WebConfigurationManager.AppSettings["END_TIME_HULU"];//"20210505";

    // 有赚
    public static string START_TIME_YOUZHUAN = WebConfigurationManager.AppSettings["START_TIME_YOUZHUAN"];//"20210329";//"20200724";
    public static string END_TIME_YOUZHUAN = WebConfigurationManager.AppSettings["END_TIME_YOUZHUAN"];//"20210429";//"20200823";

    // 泡泡赚
    public static string START_TIME_PAOPAOZHUAN = WebConfigurationManager.AppSettings["START_TIME_PAOPAOZHUAN"];//"20200826";
    public static string END_TIME_PAOPAOZHUAN = WebConfigurationManager.AppSettings["END_TIME_PAOPAOZHUAN"];//"20200925";

    // 豆豆趣玩
    public static string START_TIME_DDQW = WebConfigurationManager.AppSettings["START_TIME_DDQW"];//"20210329";
    public static string END_TIME_DDQW = WebConfigurationManager.AppSettings["END_TIME_DDQW"];//"20210429";
}