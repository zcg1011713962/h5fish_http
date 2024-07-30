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

    //����ڻ����
    public const string SetNdActFishCount = "SetNdActFishCount";
    //��ʥ�ڻ����
    public const string SetHallowmasActPumpkinCount = "SetHallowmasActPumpkinCount";
    //���ᱦ�����
    public const string SetSpittorSnatchActKillCount = "SetSpittorSnatchActKillCount";

    //�޸��������
    public const string AlterDragonScaleNum = "DragonScaleNumAlter";

    //�޸��±�����
    public const string AlterJinQiuNationalDayAct = "JinQiuNationalDayActAlter";

    //�޸ĵ��ջ�ȡ��������
    public const string AlterKdActGaindb = "StatKdActivityRankEdit";

    //�޸��м����÷�
    public const string AlterFishlordMiddleRoomPlayerScore = "StatFishlordMiddleRoomPlayerScoreEdit";

    //�޸ĸ߼����÷�
    public const string AlterFishlordAdvancedRoomPlayerScore = "StatFishlordAdvancedRoomPlayerScoreEdit";

    //�޸�ը����԰����
    public const string AlterBulletHeadPlayerScore = "StatBulletHeadPlayerScoreAlter";

    //���賡����
    public const string AlterFishlordSharkRoomPlayerScore = "StatFishlordSharkRoomPlayerScoreEdit";

    //���ｵ������
    public const string AlterFishlordLegendaryRankControl = "StatFishlordLegendaryRankControl";

    //ʥ�޳���һ���
    public const string AlterFishlordMythicalRankControl = "StatFishlordMythicalRankControl";

    //���а�����
    public const string AlterOperationRankCheat = "StatOperationRankPlayerScoreAlter";

    //���Ͷ��ʱ������
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
        m_dic.Add(CmdName.SetChannelOpenCloseGame,new CmdFactoryChannelOpenCloseGame());//С��Ϸ��������
        m_dic.Add(CmdName.SetCrocodileSpecilList,new CmdFactioryCrocodileSpecilListSet());//������ڰ���������
        m_dic.Add(CmdName.SetBzSpecilList,new CmdFactoryBzSpecilListSet());//���۱���ڰ���������
        m_dic.Add(CmdName.SetNdActFishCount,new CmdFactoryNdActPlayerFishCountSet());
        m_dic.Add(CmdName.SetHallowmasActPumpkinCount,new CmdFactoryHallowmasPumpkinCountSet());//��ʥ���Ϲ���������
        m_dic.Add(CmdName.SetFruitSpecilList, new CmdFactoryFruitSpecilListSet());//ˮ�����ڰ���������
        m_dic.Add(CmdName.SetSpittorSnatchActKillCount,new CmdFactorySpittorSnatchKillCountSet()); //���ᱦ��ɱ��������

        m_dic.Add(CmdName.AlterDragonScaleNum,new CmdFactoryDragonScaleNumAlter());//�޸�����
        m_dic.Add(CmdName.AlterJinQiuNationalDayAct,new CmdFactoryJinQiuNationalDayActAlter());//��������޸�����±�����

        m_dic.Add(CmdName.AlterKdActGaindb, new CmdFactoryAlterKdActGaindbEdit());//�޸ĵ��ջ�ȡ��������

        m_dic.Add(CmdName.AlterFishlordMiddleRoomPlayerScore, new CmdFactoryStatFishlordMiddleRoomPlayerScoreEdit());//�м�����һ����޸�

        m_dic.Add(CmdName.AlterFishlordAdvancedRoomPlayerScore, new CmdFactoryStatFishlordAdvancedRoomPlayerScoreEdit());//�߼�����һ����޸�

        m_dic.Add(CmdName.AlterBulletHeadPlayerScore, new CmdFactoryStatBulletHeadPlayerScoreAlter());//ը����԰��һ����޸�

        m_dic.Add(CmdName.AlterFishlordSharkRoomPlayerScore, new CmdFactoryStatFishlordSharkRoomPlayerScoreEdit());//���賡����
        m_dic.Add(CmdName.AlterFishlordLegendaryRankControl, new CmdFishlordLegendaryRankControl());//���ｵ������
        m_dic.Add(CmdName.AlterFishlordMythicalRankControl, new CmdFactoryFishlordMythicalPlayerScoreAlter());//ʥ�޳�����

        m_dic.Add(CmdName.AlterOperationRankCheat, new CmdFactoryOperationRankPlayerScoreAlter());//���а�����

        m_dic.Add(CmdName.AlterOperationAd100003ActSet, new CmdFactoryOperationAd100003ActAlter());//���Ͷ��ʱ������
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


