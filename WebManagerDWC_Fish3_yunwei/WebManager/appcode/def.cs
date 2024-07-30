using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.Configuration;

public struct DefCC
{
    //���ͬ������ʧ�ܲ���
    public const string ASPX_SERVICE_MAIL = "/appaspx/service/ServiceMail.aspx";

    //���ͬ������ʧ���ֶ�ȷ��
    public const string ASPX_PLAYER_ITEM_RECORD = "/appaspx/operation/PlayerItemRecord.aspx";

    //�˺Ų�ѯ�¼ӱ�����ѯ
    public const string ASPX_ACCOUNT_BEIBAO = "/appaspx/service/ServiceAccountBeibao.aspx";

    // ����鿴ҳ����������������а�
    public const string ASPX_GAME_DETAIL_BAOJIN = "/appaspx/stat/fish/FishlordBaojinRankDetail.aspx";
    // ����鿴ҳ
    public const string ASPX_GAME_DETAIL = "/appaspx/stat/gamedetail/GameDetailViewer.aspx";
    // �ټ�������
    public const string ASPX_GAME_DETAIL_BACCARAT = "/appaspx/stat/gamedetail/GameDetailBaccarat.aspx?index={0}";
    // ţţ����
    public const string ASPX_GAME_DETAIL_COWS = "/appaspx/stat/gamedetail/GameDetailCows.aspx?index={0}";
    //����鿴ҳ��ţţ�ƾֲ�ѯ��
    public const string ASPX_GAME_DETAIL_COWS_CARDS = "/appaspx/stat/cows/CowsCardsDetail.aspx";
    //����鿴ҳ��ע����б�ţţ�ƾֲ�ѯ��
    public const string ASPX_GAME_DETAIL_COWS_CARDS_PLAYER_LIST = "/appaspx/stat/cows/CowsCardsPlayerList.aspx";
    // ����������
    public const string ASPX_GAME_DETAIL_CROCODILE = "/appaspx/stat/gamedetail/GameDetailCrocodile.aspx?index={0}";
    //ˮ��������
    public const string ASPX_GAME_DETAIL_FRUIT = "/appaspx/stat/gamedetail/GameDetailFruit.aspx?index={0}";
    //���۱���
    public const string ASPX_GAME_DETAIL_BZ = "/appaspx/stat/gamedetail/GameDetailBz.aspx?index={0}";
    //��ʯ��������
    public const string ASPX_GAME_DETAIL_JEWEL = "/appaspx/stat/gamedetail/GameDetailJewel.aspx?index={0}";
    // ��������
    public const string ASPX_GAME_DETAIL_DICE = "/appaspx/stat/gamedetail/GameDetailDice.aspx?index={0}";
    // ���㹫԰����
    public const string ASPX_GAME_DETAIL_FISH_PARK = "/appaspx/stat/gamedetail/GameDetailFishPark.aspx?index={0}";
    // ��������
    public const string ASPX_GAME_DETAIL_DRAGON = "/appaspx/stat/gamedetail/GameDetailDragon.aspx?index={0}";
    // ˮ䰴�����
    public const string ASPX_GAME_DETAIL_SHUIHZ = "/appaspx/stat/gamedetail/GameDetailShuihz.aspx?index={0}";

    // �ں�÷������
    public const string ASPX_GAME_DETAIL_SHCD = "/appaspx/stat/gamedetail/GameDetailShcd.aspx?index={0}";
    //����鿴ҳ���ں�÷���ƾֲ�ѯ��
    public const string ASPX_GAME_DETAIL_SHCD_CARDS = "/appaspx/stat/shcd/ShcdCardsDetail.aspx";
    //����鿴ҳ��ע����б��ں�÷���ƾֲ�ѯ��
    public const string ASPX_GAME_DETAIL_SHCD_CARDS_PLAYER_LIST = "/appaspx/stat/shcd/ShcdCardsPlayerList.aspx";

    public const string ASPX_LOGIN = "/appaspx/Login.aspx";
    //public const string ASPX_LOGIN = "~/Account/Login.aspx";

    // ����˱��� ����鿴ҳ
    public const string ASPX_PANIC_BOX_DETAIL = "/appaspx/stat/StatPanicBoxPlayer.aspx";

    //����������� �齱ͳ�� ����ҳ
    public const string ASPX_JINQIU_NATIONALDAY_ACT_DETAIL = "/appaspx/stat/StatJinQiuNationalDayActDetail.aspx";

    //ת���㲶��ͳ������
    public const string ASPX_STAT_TFISH_DEATIL = "/appaspx/stat/StatTurnTableFishDetail.aspx";

    //˫ʮһ� ����ͳ�� ����ҳ
    public const string ASPX_JINQIU_NATIONALDAY_ACT_TASK = "/appaspx/stat/StatJinQiuNationalDayActTask.aspx";

    // �˿�����
    public static string[] s_poker = { "diamond", "club", "spade", "heart" };

    public static string[] s_pokerCows = { "diamond", "club", "heart", "spade" };
    public static string[] s_pokerColorCows = { "����", "÷��", "����", "����" };
    public static string[] s_pokerColorShcd = { "����", "����","÷��", "����", "��С��" };
    // �������������
    public static string[] s_diceStr = { "��", "С", "����" };

    public static string[] s_isBanker = { "�Ƿ���ׯ:��", "�Ƿ���ׯ:��" };

    // �˿�����ֵ
    public static string[] s_pokerNum = { "", "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    // �ټ��ֵĽ��
    public static string[] s_baccaratResult = { "��", "��", "�ж�", "ׯ��", "ׯ" };

    // �ں�÷�����˿���
    public static string[] s_pokerShcd = { "spade", "heart", "club", "diamond", "joker" };

    public const int OP_ADD = 0;      // ���
    public const int OP_REMOVE = 1;   // �Ƴ�
    public const int OP_MODIFY = 2;   // �޸�
    public const int OP_VIEW = 3;     // �鿴

    public static string HTTP_MONITOR = Convert.ToString(WebConfigurationManager.AppSettings["httpMonitor"]);

    public static string RRD_TOOL_PATH = Convert.ToString(WebConfigurationManager.AppSettings["rrdtoolPath"]);

    //Χ������ ����淨ͳ��
    //����1
    public const string ASPX_WJLW_GOLD_EARN_TURN_INFO = "/appaspx/stat/StatWjlwGoldEarnTurnInfo.aspx";
    //����2
    public const string ASPX_WJLW_GOLD_BET_PLAYER_LIST = "/appaspx/stat/StatWjlwGoldBetPlayerList.aspx";
    //Χ������ �����淨ͳ��
    //����1
    public const string ASPX_WJLW_RECHARGE_WIN_INFO = "/appaspx/stat/StatWjlwRechargeWinInfo.aspx";
    //����2
    public const string ASPX_WJLW_RECHARGE_BET_PLAYER = "/appaspx/stat/StatWjlwRechargeBetPlayer.aspx";

    //��ͷ���ͳ������
    public const string ASPX_STAT_BULLET_HEAD_GIFT_DEATIL = "/appaspx/stat/StatBulletHeadGiftPlayer.aspx";

    //���䲶���ˮ�ز�������
    public const string ASPX_STAT_FISHLORD_CTRL_NEW_DETAIL = "/appaspx/stat/StatFishlordControlNewDetail.aspx";

    //���˳齱����
    public const string ASPX_STAT_GOLD_FISH_LOTTERY_DETAIL = "/appaspx/stat/StatGoldFishLotteryDetail.aspx";

    //�߼�������ͳ������
    public const string ASPX_STAT_FISHLORD_ADVANCED_ROOM_ACT_DETAIL = "/appaspx/stat/StatFishlordAdvancedRoomActDetail.aspx";

    //ÿ���������
    public const string ASPX_STAT_DAILY_GIFT_DETAIL = "/appaspx/stat/StatDailyGiftDetail.aspx";

    //�Ϻ�Ѱ��ͳ������
    public const string ASPX_STAT_TREASURE_HUNT_DETAIL = "/appaspx/stat/StatTreasureHuntDetail.aspx";

    //�����齱ͳ������
    public const string ASPX_STAT_SD_LOTTERY_ACT_DETAIL = "/appaspx/stat/StatSdLotteryActDetail.aspx";

    //���賡�淨����ͳ������
    public const string ASPX_STAT_FISHLORD_SHARK_ROOM_INCOME_DETAIL = "/appaspx/stat/StatFishlordSharkRoomIncomeDetail.aspx";

    //���賡�齱ͳ������
    public const string ASPX_STAT_FISHLORD_SHARK_ROOM_LOTTERY_DETAIL = "/appaspx/stat/StatFishlordSharkRoomLotteryDetail.aspx";

    //ʮһ��齱ͳ������
    public const string ASPX_STAT_NATIONAL_DAY_ACT_LOTTERY_DETAIL = "/appaspx/stat/StatNationalDayActLotteryDetail.aspx";

    //׷��з���ͳ��
    //�齱ͳ�� ����ҳ
    public const string ASPX_STAT_KILL_CRAB_ACT_LOTTERY_DEATIL = "/appaspx/stat/StatKillCrabActLotteryDetail.aspx";

    //�Ʋ�ͳ������
    public const string ASPX_OPERATION_PLAYER_BANKRUPT_DETAIL = "/appaspx/operation/OperationPlayerBankruptDetail.aspx";

    //�м����һ�ͳ������
    public const string ASPX_STAT_MIDDLE_ROOM_EXCHANGE_DETAIL = "/appaspx/stat/StatFishlordMiddleRoomActExchangeDetail.aspx";

    //����
    public const string ASPX_STAT_HUNT_FISH_ACTIVITY_DETAIL = "/appaspx/stat/StatPumpHuntFishActDetail.aspx";

    //�۳佱��
    public const string ASPX_STAT_RECHARGE_ACTIVITY_DETAIL = "/appaspx/stat/StatPumpHuntFishActRechargeDetail.aspx"; 
    
    //�������� �����������
    public const string ASPX_STAT_NP_7DAY_RECHARGE_DETAIL = "/appaspx/stat/StatPumpNp7DayRechargeDetail.aspx";

    //��һ��齱 ����
    public const string ASPX_STAT_PUMP_WUYI_ACT_DETAIL = "/appaspx/stat/StatPumpWuyiActDetail.aspx";

    //������ͳ��
    public const string ASPX_STAT_GROWTH_QUEST_DETAIL = "/appaspx/stat/StatGrowthQuestDetail.aspx";
}

public enum QueryType
{
    //���λ ʥ�޻ ����
    queryTypeStatRoomQuestAct,


    //���Ͷ�Ż����
    queryTypeOperationAdActSet,

    //�������а�
    queryTypeOperationRankCheat,

    //������ͳ��
    queryTypeStatGrowthQuest,
    //����
    queryTypeStatGrowthQuestDetail,

    //�淨��ͳ��
    //������
    queryTypeStatDiceGame,

    //����
    queryTypeStatShellGame,

    //��ͷϺը��з
    queryTypeStatXiaXieGame,

    //ʥ�޳��淨ͳ��
    //��ȸ����ͳ��
    queryTypeStatMythicalRoomZhuque,
    //�׻�����ͳ��
    queryTypeStatMythicalRoomBaihu,
    //��������ͳ��
    queryTypeStatMythicalRoomXuanwu,
    //����˫���淨ͳ��
    queryTypeStatMythicalRoomCaishen,

    //���а� ��ǰ
    queryTypeStatMythicalRoomCurrRank,
    //���а� ��ʷ
    queryTypeStatMythicalRoomHisRank,
    //��ǰ���а� ��һ���
    queryTypeStatMythicalRoomRankPlayer,

    //��һ�ͳ��
    //��һ�
    queryTypeStatPumpWuyiSetAct,
    //��һ����
    queryTypeStatPumpWuyiSetActLottery,

    //��һ��������
    queryTypeStatPumpWuyiSetActLotteryList,

    //��������
    //�������
    queryTypeStatPumpNp7DaysRecharge,
    //�����������
    queryTypeStatPumpNp7DaysRechargeDetail,
    //�һ�����
    queryTypeStatPumpNp7DaysRechangeExg,

    //�������
    queryTypeStatPumpRedEnvelop,

    //��������һ�
    queryTypeStatPumpRedEnvelopExchange,

    //�����˻��ֹ���
    queryTypeOperationRobotRankCFG,

    //���λ
    //����
    queryTypeStatPumpHuntFishActivity,
    //��������
    queryTypeStatPumpHuntFishActDetail,

    //�۳佱��
    queryTypeStatPumpRechargeActivity,
    //�۳佱������
    queryTypeStatPumpRechargeActivityDetail,

    //����
    queryTypeStatPumpCatchFishActivity,

    //��һ���
    queryTypeStatFishlordGoldHoopTorpedoScore,

    //���ｵ��
    //��ǰ
    queryTypeStatFishlordLegendaryCurRank,
    //��ʷ
    queryTypeStatFishlordLegendaryHisRank,
    //��������
    queryTypeStatFishlordGoldHoopTorpedoRank,

    //�淨����ͳ��
    queryTypeStatFishlordLegendaryFishRoomplay,
    //��������ͳ��
    queryTypeStatFishlordLegendaryFishRoom,

    //ʱ����������
    queryTypeStatDailyHourMaxOnlinePlayer,

    //�����ƻ�
    queryTypeStatVipExclusiveTask,

    //VIP����
    queryTypeStatVipExclusiveDiamondMall,

    //VIP��֤
    queryTypeStatVipExclusiveBindPhone,

    //�����̳�
    queryTypeStatVipExclusivePointExchange,

    //��ֵ���а�
    queryTypeOperationRankRecharge,

    //���ں�ͳ��
    queryTypeStatWechatGift,

    //�ڲ������ͳ��
    queryTypeStatInnerPlayer,

    //������Ƶͳ��
    queryTypeStatPlayAd,

    //vip��Ȩͳ��
    queryTypeStatVipRecord,

    //�¿�����ͳ��
    queryTypeStatMonthCard,

    //���ƽ��Я����Ʒ
    queryTypeStatTurretItemsOnPlayer,

    //VIP��Ȩ
    queryTypeStatVipGift,

    //�ɳ�����ͳ��
    queryTypeStatPumpGrowFund,

    //����ʼ�ͳ��
    queryTypePlayerMail,

    //������Ƭͳ��
    queryTypeStatTurretChip,

    //�������
    queryTypeStatRebateGift,

    //���Я�����
    queryTypeStatGoldOnPlayer,

    //�ڱ�����
    queryTypeStatPlayerOpenRateTask,

    /////////////////////////////////////////////
    //׷��з���齱ͳ��
    queryTypeStatKillCrabActLottery,
    //׷��з���齱ͳ������
    queryTypeStatKillCrabActLotteryDetail,

    //׷��з������ͳ��
    queryTypeStatKillCrabActRoom,

    //׷��з������ͳ��
    queryTypeStatKillCrabActTask,

    //Ԫ���
    queryTypeStatYuandanSign,
    
    //////////////ʮһ�
    //ǩ��
    queryTypeStatActSign,
    //���ּ���
    queryTypeStatActExchange,
    //ð��֮·
    queryTypeStatActTask,
    //�������
    queryTypeStatActGift,
    //�齱ͳ��
    queryTypeStatActLottery,
    //�齱ͳ������
    queryTypeStatNationalDayActLotteryDetail,

    ///////////////////////////////////////////////////////////////////////////
    //�����ڱ������
    queryTypeStatNewPlayerOpenRate,

    //�½���Ҹ��Ѽ��
    queryTypeTdNewPlayerMonitor,

    //����ֻ���֤
    queryTypeStatPlayerPhoneVertify,

    //��������
    queryTypeStatPlayerItemRecharge,

    //���߽���
    queryTypeStatOnlineReward,

    //�����ͳ��
    queryTypeStatSdAct,

    //�����齱ͳ��
    queryTypeStatSdLotteryAct,
    //�����齱ͳ������
    queryTypeStatSdLotteryActDetail,

    //�������
    queryTypeStatTomorrowGift,

    //��ʧ��ͳ��
    queryTypeStatNewGuildLosePoint,

    //�Ϻ�Ѱ��
    queryTypeStatTreasureHunt,

    //�Ϻ�Ѱ������
    queryTypeStatTreasureHuntDetail,

    //ÿ�����ͳ��
    queryTypeStatDailyGift,

    //ÿ�����ͳ��
    queryTypeStatDailyGiftDetail,

    ////////////////////////////////////////////////
    //�߼�����һ����޸�
    queryTypeStatFishlordAdvancedRoomScore,

    //�߼������ƹ���
    queryTypeStatFishlordAdvancedRoom,
    //�߼�������ͳ��
    queryTypeStatFishlordAdvancedRoomAct,
    //�߼�������ͳ������
    queryTypeStatFishlordAdvancedRoomActDetail,
    //�߼������а�
    queryTypeStatFishlordAdvancedRoomActRank,

    /////////////////////////////////////////////////
    //�м�����һ����޸�
    queryTypeStatFishlordMiddleRoomPlayerScore,
    //�м�����һ������а�
    queryTypeStatFishlordMiddleRoomPlayerRank,
    //�м����淨����ͳ��
    queryTypeStatFishlordMiddleRoomIncome,
    //�м����һ�ͳ��
    queryTypeStatFishlordMiddleRoomExchange,
    //�м����һ�ͳ������
    queryTypeStatFishlordMiddleRoomExchangeDetail,
    //�м������а�
    queryTypeStatFishlordMiddleRoomRank,
    //�м������ͳ��
    queryTypeStatFishlordMiddleRoomFuDai,

    //�м������ͳ��
    queryTypeStatMiddleRoomExchange,
    ////////////////////////////////////////////////

    //���賡
    //���賡�淨����ͳ��
    queryTypeStatFishlordSharkRoomIncome,
    //����
    queryTypeStatFishlordSharkRoomIncomeDetail,

    //���ͳ��
    queryTypeStatFishlordSharkRoomChaijieIncome,
    queryTypeStatFishlordSharkRoomChaijieDetail,

    //��ը��ͳ��
    queryTypeStatFishlordSharkRoomBomb,
    //����齱ͳ��
    queryTypeStatFishlordSharkRoomLottery,

    //����齱����
    queryTypeStatFishlordSharkRoomLotteryDetail,

    //���賡���а�
    queryTypeStatFishlordSharkRoomRank,

    //���賡���ײ�ѯ���
    queryTypeStatFishlordSharkRoomScore,

    //����ͳ��
    queryTypeStatFishlordSharkRoomEnergy,
    ////////////////////////////////////////////////

    //��������
    queryTypeStatMainlyTask,

    //�����ɳ��ֲ�
    queryTypeOperationPlayerActTurret,

    //��������ɳ��ֲ�
    queryTypeOperationPlayerActTurretBySingle,

    //�Ʋ�ͳ��
    queryTypeOperationPlayerBankruptStat,
    //�Ʋ�ͳ������
    queryTypeOperationPlayerBankruptDetail,

    //�Ʋ��ڱ�����
    queryTypeOperationPlayerOpenRateBankruptList,

    //���˳齱
    queryTypeStatGoldFishLottery,

    //���˳齱�ܼ�
    queryTypeStatGoldFishLotteryTotal,

    //���˳齱����
    queryTypeStatGoldFishLotteryDetail,

    //ÿ��������ͳ��
    queryTypeStatDailyWeekTask,
    //ÿ���ܽ���ͳ��
    queryTypeStatDailyWeekReward,

    //��ȯ��ͳ��
    queryTypeStatLotteryExchange,

    //ϵͳ��Ͷ ����
    queryTypeStatAirDropSys,

    //ϵͳ��Ͷ ��
    queryTypeStatAirDropSysOpen,

    //��̨���� ͳ�Ƶ�����Ϣ
    queryTypeStatTodayInfo,

    //�ڱ����
    queryTypeOpnewTurretTimes,

    //��ҳ�ֵ��Ϣ
    queryTypeOpnewPlayerRecharge,

    //ͳ����һ�Ծ����
    queryTypeOpnewGameActive,

    //ͳ����Ҹ���
    queryTypeOpnewGameIncome,

    //�泡�������
    queryTypeStatFishingRoomInfo,

    //��һ�����Ϣ
    queryTypeStatPlayerBasicInfo,

    //������
    queryTypeStatKdActRank,

    //������������
    queryTypeStatKdActDayRank,

    //ת�������а�
    queryTypeStatTurnTableFishRank,
    //ת���㲶��ͳ��
    queryTypeStatTFish,
    //ת���㲶��ͳ������
    queryTypeStatTFishDetail,
    //ת���㳡��ͳ��
    queryTypeStatTFishRoom,

    //��ͷ���ͳ��
    queryTypeStatBulletHeadGift,
    //��ͷ���ͳ������
    queryTypeStatBulletHeadGiftPlayer,

    //Χ�������Զ��帶���������
    queryTypeWjlwRechargeReward,
    //����淨ͳ��
    queryTypeWjlwGoldEarn,
    //����淨ͳ��ÿ������
    queryTypeWjlwGoldEarnTurnInfo,
    //����淨ͳ��ÿ����ע����б�
    queryTypeWjlwGoldBetPlayerList,

    //Χ�����������淨ͳ��
    queryTypeWjlwRechargeEarn,
    //������
    queryTypeWjlwRechargeWinInfo,
    //Ͷע����
    queryTypeWjlwRechargeBetPlayer,

    //΢�Ź��ں�ǩ��ͳ��
    queryTypeWechatRecvRewardStat,

    //���䳡����ͳ��
    queryTypeStatPlayerBw,

    //�������ͳ��
    queryTypeStatPlayerMoneyRep,

    //С��Ϸ���ۼ���Ӯ
    queryTypeGameDailyTotalLoseWinRank,

    //С��Ϸʵʱ��Ӯ��
    queryTypeGameRealTimeLoseWinRank,

    //��ҲƸ���
    queryTypePlayerRichesRank,

    //�����������
    //�޸�����±�����
    queryTypeJinQiuNationalDayActCtrl,

    //���а�ͳ��
    queryTypeJinQiuNationalDayActRank,
    //�齱ͳ��
    queryTypeJinQiuNationalDayActLottery,
    //�齱ͳ������
    queryTypeJinQiuNationalDayActLotteryDetail,
    //����ͳ��
    queryTypeJinQiuNationDayActRoomStat,
    //����ͳ��
    queryTypeJinQiuNationalDayActTaskStat,
    //����ͳ������
    queryTypeJinQiuNationalDayActTaskDetail,

    //��ʧ��������¼Ч����ѯ
    queryTypeGuideLostPlayersRes,

    //��ʧ��ɸѡ
    queryTypeSelectLostPlayers,

    //�ͷ�����/�����/��������-ϵͳ
    queryTypeRepairOrder,

    //����˱���
    queryTypeStatPanicBox,
    //���������
    queryTypeStatPanicBoxDetail,

    //���ᱦ�
    //����а�
    queryTypeSpittorSnatchActRank,
    //��ȡ��������ͳ��
    queryTypeSpittorSnatchActRewardList,

    //�����������ͳ��
    queryTypePumpNewGuide,

    //��һ�䷵�
    queryTypeWuyiRewardResult,

    //����ըըը
    queryTypeBulletHeadCurrRank, //��ǰ����
    queryTypeBulletHeadRank,//��ʷ����
    queryTypeBulletHeadPlayerScore,//ը����԰��һ���
    queryTypeBulletHeadGuaranteed, //ը����԰��Χ��ǰ
    queryTypeBulletHeadHisGuaranteed,//ը����԰��Χ��ʷ
    
    //��������
    queryTypeDragonScaleCurrRank,//��ǰ����
    queryTypeDragonScaleRank,//��ʷ����
    //���������޸�
    queryTypeDragonScaleControl,

    //�����cdkey������
    queryTypeGiftCodeNewList,

    //ͨ�������������Ų�ѯ���ID
    queryTypeGetPlayerIdByOrderId,

    //���ߴ�ð��
    queryTypeStatNYAdventure,

    //�´��ط�
    queryTypeStatNYAccRecharge,

    //�������
    queryTypeStatNYGift,

    //�ι��ֻ��Ӫ
    queryTypeScratchActEarn,

    //�ι��ֻ�ҽ�
    queryTypeScratchActExchangeRes,

    //���ͬ������ʧ��
    queryTypeWord2LogicItemError,

    //��ҵ��߻�ȡ����
    queryTypePlayerItemRecord,

    //�ʼ���ѯ
    queryTypeMailQuery,

    //�˺Ų�ѯ ������ѯ
    queryTypeAccountBeibao,

    //ʥ����/Ԫ���
    queryTypeChristmasOrYuandan,
    //ǿ��������ѯ
    queryForceUpdateReward,

    //��Ӫ����༭��ѯ
    queryTypeNoticeInfo,

    //ˮ����
    //ˮ������������
    queryTypeFruitParamControl,

    //ˮ�����ڰ������б�
    queryTypeFruitSpecilList,

    //ˮ������������
    queryTypeIndependentFruit,

    //���� ͨ��������ѯ
    queryTypeRechargeByAibei,
    //��������߻���
    queryTypeRobotMaxScore,
    //��ͣ�����ID�б�
    queryTypeServiceUnBlockIdList,

    //��ͷͳ��
    queryTypeFishlordBulletHeadStat,
    //��ͷ����ʣ��ͳ��
    queryTypeFishlordBulletHeadOutput,

    //��ͷ��ѯ
    queryTypeFishlordBulletHeadQuery,

    //���ײ�������
    queryTypeFishlordBulletHeadList,

    //��ͷ���Ѳ�ѯ
    queryTypeTorpedoTicket,

    //��������
    queryTypePolarLightsPush,

    //���籭�󾺲����±�
    queryTypeWorldCupMatch,

    //���籭�󾺲����Ѻעͳ��
    queryTypeWorldCupMatchPlayerJoin,

    //��ʥ�ڻ
    //���а�
    queryTypeHallowmasActRank,
    //��ȡ��������
    queryTypeHallowmasActRecvCount,

    //�����ػݻ
    queryTypeJinQiuRechargeLottery,
    //����ڻ
    //��ȡ����
    queryTypeNdActRecvCount,

    //����а�
    queryTypeNdActRankList,

    //С��Ϸ��������
    queryTypeChannelOpenCloseGame,
    //������
    queryTypeNewSevenDay,
    //��������
    queryTypeNewPlayerTask,
    //ǩ���ֲ����
    queryTypeSignByMonth,
    //ÿ��ǩ��
    queryTypeDailySign,
    //ÿ��ǩ����ȡ����
    queryTypeDailySignReward,

    //ÿ������
    queryTypeDailyTask,

    //��������¼��ѯ
    queryTypePlayerChatQuery,
    //����Զ���ͷ��ͳ��
    queryTypePlayerIconCustomStat,

    //�������а���ϸ
    queryTypeFishlordBaojinRankDetail,
    //���������
    queryTypeFishlordBaojinStat,

    //������������ͳ��
    queryTypeFishlordDragonPalaceConsumeStat,

    //����������ҷֲ�ͳ��
    queryTypeFishlordDragonPalacePlayerStat,

    //������ͳ��
    queryTypeFishlordDragonPalaceKillDragon,

    //����������ͳ��
    queryTypeFishlordJingjiConsumeStat,

    //����������ͳ��
    queryTypeFishlordJingjiOutlayStat,

    //����������ͳ��
    queryTypeFishlordJingjiTaskStat,

    //��������ҷֲ�ͳ��
    queryTypeFishlordJingjiPlayerStat,

    //�������а�(��ǰ)
    queryTypeFishlordBaojinRank,
    //�������а�
    queryTypeFishlordBaojinHistoryRank,
    //����ż���Ŵ���
    queryTypePuppetActStat,
    //��Ҿ�����λ ��������λ������ȡ
    queryTypePuppetRewardRecv,
    //�������ܾ�����ż
    queryTypePuppetSvrDonate,
    //20������������а�
    queryTypePuppetPlayerDonateRank,

    //20���ۼƻ����ż������а�
    queryTypePuppetPlayerGainRank,

    //������ҳ齱����ͳ��
    queryTypeLabaLotteryStat,
    //������ҳ齱��¼��ѯ
    queryTypeLabaLotteryQuery,
    //���Գ齱��λͳ��
    queryTypeLabaLotteryProb,

    //���Իͳ��
    queryTypeLabaActivityStat,

    //����ʢ��
    queryTypeWpActivityStat,
    queryTypeWpActivityPlayerStat,

    //����ʢ��
    queryTypeFishlordFeastStat,

    //��ʱ����
    queryTypeActivityPanicBuyingCfg,

    //����
    queryTypeIncomeExpensesError,

    //���ջ(��)
    queryTypeFestivalActivity,

    //���ջ
    queryTypeSevenDayActivity,

    //�������ÿ�չ���
    queryTypeMaterialGiftRecharge,

    // GM�˺�
    queryTypeGmAccount,

    // ��ң���ʯ�仯
    queryTypeMoney,
    // ��ϸ
    queryTypeMoneyDetail,

    // �ͷ���Ϣ��ѯ
    queryTypeServiceInfo,

    // �ʼ�
    queryTypeMail,

    // ��ֵ��¼
    queryTypeRecharge,

    // ��ֵ��¼(��)
    queryTypeRechargeNew,

    // �˺Ų�ѯ
    queryTypeAccount,

    // ��½��ʷ
    queryTypeLoginHistory,

    // �����ѯ
    queryTypeGift,

    // ������ѯ
    queryTypeGiftCode,

    // �һ�
    queryTypeExchange,

    //ʵ����˹���
    queryTypeExchangeAudit,

    // ����ͨ������
    queryTypeLobby,

    // ����������
    queryTypeServerEarnings,

    // �����������
    queryTypeIndependentFishlord,

    // �����������
    queryTypeIndependentCrocodile,
    
    //���۱����������
    queryTypeIndependentBz,

    // ������������
    queryTypeIndependentDice,

    // ţţ��������
    queryTypeIndependentCows,

    // ����ӯ����
    queryTypeDiceEarnings,

    // �ټ���ӯ����
    queryTypeBaccaratEarnings,

    // �ټ�����ׯ���
    queryTypeBaccaratPlayerBanker,

    // ţţ��ׯ���
    queryTypeCowsPlayerBanker,

    //ţţ�ƾֲ�ѯ
    queryTypeCowsCardsQuery,

    //ţţ�ƾֲ�ѯ��ע����б�
    queryTypeCowsCardsPlayerList,

    //ţţ�ƾ�����
    queryTypeCowsCardsDetail,

    //ţţɱ�ַŷ�LOG��¼�б�
    queryTypeCowsCardCtrlList,

    // ��ǰ����
    queryTypeCurNotice,

    // ���������ѯ
    queryTypeFishlordParam,
    
    //��ˮ�ز�����ѯ
    queryTypeFishlordNewParam,

    //��ˮ�س��β�����ѯ
    queryTypeFishlordRoomNewParam,

    //���˺�̨����
    queryTypeFishlordNewSingleParam,

    //���㱬���������������
    queryTypeFishlordBaojinParam,

    //���㾺������ҵ÷��޸�
    queryTypeFishlordBaojinScoreParam,

    // ���㹫԰������ѯ
    queryTypeFishParkParam,

    // �������Ӳ�����ѯ
    queryTypeFishlordDeskParam,
    // ���㹫԰���Ӳ�����ѯ
    queryTypeFishParkDeskParam,

    // �����������ѯ
    queryTypeCrocodileParam,

    //������ڰ������б�
    queryTypeCrocodileSpecilList,

    //���۱��������ѯ
    queryTypeBzParam,

    //���۱���ڰ������б�
    queryTypeBzSpecilList,

    // ţţ������ѯ
    queryTypeQueryCowsParam,

    //ţţ�ڰ������б�
    queryTypeCowCardsSpecilList,

    // ����������ѯ��ÿ�������ϵͳ�����룬��֧����ӯ����..
    queryTypeDragonParam,

    // ��������Ϸģʽ�µĲ�����ѯ
    queryTypeDragonGameModeEarning,

    // �ں�÷��������ѯ
    queryTypeShcdParam,
    // �ں�÷����������
    queryTypeIndependentShcd,
    //�ں�÷���ƾֲ�ѯ
    queryTypeShcdCardsQuery,

    //�ں�÷���ƾ�����
    queryTypeShcdCardsDetail,

    //�ں�÷����ע����б�
    queryTypeShcdCardsPlayerList,

    //�ں�÷���ڰ������б�
    queryTypeShcdCardsSpecilList,

    //�ں�÷��ɱ�ַŷ�LOG��¼�б�
    queryTypeShcdCardsCtrlList,

    //ˮ䰴���ӯ���ʲ鿴
    queryTypeShuihzTotalEarning,

    //ˮ䰴�ÿ��ӯ���ʲ鿴
    queryTypeShuihzDailyEarning,

    //ˮ䰴�����ӯ���ʲο�
    queryTypeShuihzSingleEarning,

    //ˮ䰴�ÿ�մ�����������ͳ��
    queryTypeShuihzReachLimit,

    //ˮ䰴�ÿ����Ϸ����鿴
    queryTypeShuihzDailyState,

    // ��ѯ��ţ��Ϸ���
    queryTypeGameCalfRoping,

    // ������ͳ��
    queryTypeFishStat,
    // ���㹫԰������ͳ��
    queryTypeFishParkStat,

    // �����������
    queryTypeMoneyAtMost,

    // �ɵ�ӯ����
    queryTypeOldEaringsRate,

    // ���䲶��׶η���
    queryTypeFishlordStage,
    // ���㹫԰�׶η���
    queryTypeFishParkStage,

    // ��ǰ����
    queryTypeOnlinePlayerCount,

    // ������־
    queryTypeOpLog,

    // ��ѯ���ͷ��
    queryTypePlayerHead,

    // �����ܼ�
    queryTypeTotalConsume,

    // ����Ϸ����
    queryTypeGameRecharge,

    // �����������
    queryTypeCoinGrowthRank,

    // ��ʧ��ѯ
    queryTypeAccountCoinLessValue,

    // ��������
    queryTypeFishConsume,

    // ţţ���Ͳ�ѯ
    queryTypeCowsCardsType,

    // ��Ϸ������Ʋ�ѯ
    queryTypeGameResultControl,

    // ͷ��ٱ�
    queryTypeInformHead,

    // ��ѯtd��Ծ
    queryTypeTdActivation,
    // LTV��ֵ
    queryTypeLTV,

    // ��ѯ�������
    queryTypeMaxOnline,

    // ��ҽ���ܺ�
    queryTypeTotalPlayerMoney,

    // ������ز�ѯ
    queryTypeGrandPrix,

    // bossͳ��
    queryTypeFishBoss,

    // �һ�ͳ��
    queryTypeExchangeStat,

    // ���ѵ�
    queryTypeRechargePointStat,

    // ���ǳ齱
    queryTypeStarLottery,

    //ת�̳齱
    queryTypeDialLottery,

    //������ͳ��
    queryTypePumpChipFishStat,

    queryTypeRLose,
    // ÿ������
    queryTypeDragonBallDaily,
    // ��ҳ�ֵ���
    queryTypeRechargePlayerMonitor,

    // ÿСʱ����
    queryTypeRechargePerHour,
    // ÿСʱ��������
    queryTypeOnlinePlayerNumPerHour,

    //ÿСʱ����������
    queryTypeOnlinePlayerNumPerHourNew,

    // ƽ����Ϸʱ���ֲ�
    queryTypeGameTimeDistribution,
    // �û�ϲ��-ƽ������ʱ��
    queryTypeGameTimePlayerFavor,
    // �׸���Ϸʱ���ֲ�
    queryTypeFirstRechargeGameTimeDistribution,
    // �״ι���Ʒѵ�ֲ�
    queryTypeFirstRechargePointDistribution,
    // �û���ע���
    queryTypePlayerGameBet,
    // ��ѯ�����֧ͳ��
    queryTypePlayerIncomeExpenses,

    // �����û�����
    queryTypeNewPlayer,

    // ����100003�Ļ����
    queryTypeQueryChannel100003ActCount,
}

