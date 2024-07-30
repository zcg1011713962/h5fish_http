using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data.OleDb;

//////////////////////////////////////////////////////////////////////////
public struct CmdName
{
    public const string Adjust = "FishlordBaojinAdjust";
    public const string AlterScore = "FishlordBaojinScoreAlter";
    public const string SetShcdCardsSpecilList = "ShcdCardsSpecilListSet";
    public const string SetCowCardsSpecilList = "CowCardsSpecilListSet";
    public const string SetChannelOpenCloseGame = "ChannelOpenCloseGameSet";
    public const string SetCrocodileSpecilList = "CrocodileSpecilListSet";
    public const string SetBzSpecilList = "BzSpecilListSet";
    public const string SetFruitSpecilList = "FruitSpecilListSet";

    //国庆节活动作弊
    public const string SetNdActFishCount = "SetNdActFishCount";
    //万圣节活动作弊
    public const string SetHallowmasActPumpkinCount = "SetHallowmasActPumpkinCount";
    //金蟾夺宝活动作弊
    public const string SetSpittorSnatchActKillCount = "SetSpittorSnatchActKillCount";

    //修改玩家龙鳞
    public const string AlterDragonScaleNum = "DragonScaleNumAlter";

    //修改月饼数量
    public const string AlterJinQiuNationalDayAct = "JinQiuNationalDayActAlter";

    //修改当日获取龙珠数量
    public const string AlterKdActGaindb = "StatKdActivityRankEdit";

    //修改中级场得分
    public const string AlterFishlordMiddleRoomPlayerScore = "StatFishlordMiddleRoomPlayerScoreEdit";

    //修改高级场得分
    public const string AlterFishlordAdvancedRoomPlayerScore = "StatFishlordAdvancedRoomPlayerScoreEdit";

    //修改炸弹乐园积分
    public const string AlterBulletHeadPlayerScore = "StatBulletHeadPlayerScoreAlter";

    //巨鲨场作弊
    public const string AlterFishlordSharkRoomPlayerScore = "StatFishlordSharkRoomPlayerScoreEdit";

    //巨鲲降世积分
    public const string AlterFishlordLegendaryRankControl = "StatFishlordLegendaryRankControl";

    //圣兽场玩家积分
    public const string AlterFishlordMythicalRankControl = "StatFishlordMythicalRankControl";

    //排行榜作弊
    public const string AlterOperationRankCheat = "StatOperationRankPlayerScoreAlter";

    //广告投放时间设置
    public const string AlterOperationAd100003ActSet = "StatOperationAd100003ActEdit";
}

public class CmdFactorySet
{
    static CmdFactorySet s_obj = null;
    Dictionary<string, CmdFactoryBase> m_dic = new Dictionary<string, CmdFactoryBase>();

    public static CmdFactorySet getInstance()
    {
        if (s_obj == null)
        {
            s_obj = new CmdFactorySet();
            s_obj.init();
        }
        return s_obj;
    }

    public void init()
    {
        m_dic.Add(CmdName.Adjust, new CmdFactoryFishlordBaojinAdjust());
        m_dic.Add(CmdName.AlterScore,new CmdFactoryFishlordBaojinScoreAlter());
        m_dic.Add(CmdName.SetShcdCardsSpecilList, new CmdFactoryShcdCardsSpecilListSet());
        m_dic.Add(CmdName.SetCowCardsSpecilList, new CmdFactoryCowCardsSpecilListSet());
        m_dic.Add(CmdName.SetChannelOpenCloseGame,new CmdFactoryChannelOpenCloseGame());//小游戏开关设置
        m_dic.Add(CmdName.SetCrocodileSpecilList,new CmdFactioryCrocodileSpecilListSet());//鳄鱼大亨黑白名单设置
        m_dic.Add(CmdName.SetBzSpecilList,new CmdFactoryBzSpecilListSet());//奔驰宝马黑白名单设置
        m_dic.Add(CmdName.SetNdActFishCount,new CmdFactoryNdActPlayerFishCountSet());
        m_dic.Add(CmdName.SetHallowmasActPumpkinCount,new CmdFactoryHallowmasPumpkinCountSet());//万圣节南瓜数量设置
        m_dic.Add(CmdName.SetFruitSpecilList, new CmdFactoryFruitSpecilListSet());//水果机黑白名单设置
        m_dic.Add(CmdName.SetSpittorSnatchActKillCount,new CmdFactorySpittorSnatchKillCountSet()); //金蟾夺宝击杀数量设置

        m_dic.Add(CmdName.AlterDragonScaleNum,new CmdFactoryDragonScaleNumAlter());//修改龙鳞
        m_dic.Add(CmdName.AlterJinQiuNationalDayAct,new CmdFactoryJinQiuNationalDayActAlter());//中秋国庆修改玩家月饼数量

        m_dic.Add(CmdName.AlterKdActGaindb, new CmdFactoryAlterKdActGaindbEdit());//修改当日获取龙珠数量

        m_dic.Add(CmdName.AlterFishlordMiddleRoomPlayerScore, new CmdFactoryStatFishlordMiddleRoomPlayerScoreEdit());//中级场玩家积分修改

        m_dic.Add(CmdName.AlterFishlordAdvancedRoomPlayerScore, new CmdFactoryStatFishlordAdvancedRoomPlayerScoreEdit());//高级场玩家积分修改

        m_dic.Add(CmdName.AlterBulletHeadPlayerScore, new CmdFactoryStatBulletHeadPlayerScoreAlter());//炸弹乐园玩家积分修改

        m_dic.Add(CmdName.AlterFishlordSharkRoomPlayerScore, new CmdFactoryStatFishlordSharkRoomPlayerScoreEdit());//巨鲨场作弊
        m_dic.Add(CmdName.AlterFishlordLegendaryRankControl, new CmdFishlordLegendaryRankControl());//巨鲲降世积分
        m_dic.Add(CmdName.AlterFishlordMythicalRankControl, new CmdFactoryFishlordMythicalPlayerScoreAlter());//圣兽场积分

        m_dic.Add(CmdName.AlterOperationRankCheat, new CmdFactoryOperationRankPlayerScoreAlter());//排行榜作弊

        m_dic.Add(CmdName.AlterOperationAd100003ActSet, new CmdFactoryOperationAd100003ActAlter());//广告投放时间设置
    }

    public CmdFactoryBase getFactory(string cmdName)
    {
        if (m_dic.ContainsKey(cmdName))
        {
            return m_dic[cmdName];
        }

        return null;
    }
}

//////////////////////////////////////////////////////////////////////////
public class CmdFactoryBase
{
    public virtual CommandBase createCMD() { return null; }
}

public class CmdFactoryFishlordBaojinAdjust : CmdFactoryBase
{
    public override CommandBase createCMD()
    {
        return new CommandFishlordBaojinAdjust();
    }
}

public class CmdFactoryDragonScaleNumAlter : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandDragonScaleNumAlter();
    }
}

public class CmdFishlordLegendaryRankControl : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandFishlordLegendaryRankControl();
    }
}

public class CmdFactoryFishlordMythicalPlayerScoreAlter : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandFishlordMythicalRankControl();
    }
}

public class CmdFactoryJinQiuNationalDayActAlter : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandJinQiuNationalDayActAlter();
    }
}

public class CmdFactoryFishlordBaojinScoreAlter : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandFishlordBaojinScoreAlter();
    }
}

public class CmdFactoryShcdCardsSpecilListSet : CmdFactoryBase 
{
    public override CommandBase createCMD() 
    {
        return new CommandShcdCardsSpecilListSet();
    }
}

public class CmdFactioryCrocodileSpecilListSet:CmdFactoryBase
{
    public override CommandBase createCMD()
    {
        return new CommandCrocodileSpecilListSet();
    }
}

public class CmdFactoryBzSpecilListSet : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandBzSpecilListSet();
    }
}

public class CmdFactoryCowCardsSpecilListSet : CmdFactoryBase
{
    public override CommandBase createCMD()
    {
        return new CommandCowCardsSpecilListSet();
    }
}

public class CmdFactoryChannelOpenCloseGame : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandChannelOpenCloseGameSet();
    }
}

public class CmdFactoryNdActPlayerFishCountSet : CmdFactoryBase 
{
    public override CommandBase createCMD() 
    {
        return new CommandNdActPlayerFishCountSet();
    }
}

public class CmdFactoryHallowmasPumpkinCountSet : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandHallowmasActPumpkinCountSet();
    }
}

public class CmdFactorySpittorSnatchKillCountSet : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandSpittorSnatchKillCountSet();
    }
}

public class CmdFactoryFruitSpecilListSet : CmdFactoryBase
{
    public override CommandBase createCMD()
    {
        return new CommandFruitSpecilListSet();
    }
}

public class CmdFactoryAlterKdActGaindbEdit : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandKdActRankGaindbEdit();
    }
}

public class CmdFactoryStatFishlordMiddleRoomPlayerScoreEdit : CmdFactoryBase
{
    public override CommandBase createCMD()
    {
        return new CommandFishlordMiddleRoomPlayerScoreEdit();
    }
}

public class CmdFactoryStatFishlordAdvancedRoomPlayerScoreEdit : CmdFactoryBase
{
    public override CommandBase createCMD()
    {
        return new CommandFishlordAdvancedRoomPlayerScoreEdit();
    }
}

public class CmdFactoryStatBulletHeadPlayerScoreAlter : CmdFactoryBase
{
    public override CommandBase createCMD()
    {
        return new CommandBulletHeadPlayerScoreEdit();
    }
}

public class CmdFactoryOperationRankPlayerScoreAlter : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandOperationRankPlayerScoreEdit();
    }
}

public class CmdFactoryOperationAd100003ActAlter : CmdFactoryBase 
{
    public override CommandBase createCMD()
    {
        return new CommandOperationAd100003ActEdit();
    }
}

public class CmdFactoryStatFishlordSharkRoomPlayerScoreEdit : CmdFactoryBase
{
    public override CommandBase createCMD()
    {
        return new CommandFishlordSharkRoomPlayerScoreEdit();
    }
}


