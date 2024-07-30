using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Linq;

public struct TableName
{
    //������а�
    public const string STAT_PUMP_DAILY_GOLD_RANK = "pumpDailyGoldRank";
    //��ͭ���������ƽ���ʯ
    public const string STAT_PUMP_DAILY_ITEM_RANK = "pumpDailyItemRank";

    //������ͳ��
    public const string STAT_PUMP_GROWTH_QUEST = "pumpGrowthQuest";

    //������
    public const string STAT_PUMP_DICE_GAME = "pumpDiceGame";
    public const string STAT_PUMP_SHELL_GAME = "pumpShellGame";

    //ʥ�޳��淨ͳ��
    //��ȸ����ͳ��
    public const string STAT_PUMP_MYTHICAL_ROOM_EVENT_TRIGGER = "pumpMythicalRoomEventTrigger";

    //ʥ�޳� ��ǰ���а�
    //public const string FISHLORD_MYTHICAL_POINTS_RANK = "fishlordMythicalPointsRank";
    //��ǰ���а�ԭʼ��
    public const string FISHLORD_MYTHICAL_PLAYER = "fishlordMythicalPlayer";

    //��ʷ���а�
    public const string STAT_PUMP_MYTHICAL_ROOM_RANK = "pumpMythicalRoomRank";

    //��һ�
    public const string STAT_PUMP_WUYI_TASK = "pumpWuYiTask";
    //��һ����
    public const string STAT_PUMP_WUYI_LOTTERY = "pumpWuYiLottery";

    //�������
    public const string STAT_PUMP_RED_ENVELOP = "pumpNewPlayerRedPacket";

    //��������
    public const string STAT_PUMP_NP_7DAY_RECHARGE = "pumpNp7DayRecharge";
    public const string STAT_PUMP_NP_7DAY_RECHARGE_DETAIL = "pumpNp7DayRechargeDetail";

    //�����˻��ֹ���
    public const string STAT_FISHLORD_ROBOT_RANK_CFG = "fishlordRobotRankCFG";

    //���λ 
    //ʥ�޻������
    public const string STAT_PUMP_ROOM_QUEST_ACTIVITY = "pumpRoomQuestActivity";
    public const string STAT_PUMP_ROOM_QUEST_ACTIVITY_DETAIL = "pumpRoomQuestActivityDetail";

    //����
    public const string STAT_PUMP_HUNT_FISH_ACTIVITY = "pumpHuntFishActivity";
    //�۳佱��
    public const string STAT_PUMP_RECHARGE_ACTIVITY = "pumpRechargeActivity";

    //����
    public const string STAT_PUMP_CATCH_FISH_ACTIVITY = "pumpCatchFishActivity";

    //���ﳡ�淨
    public const string STAT_FISHLORD_LEGENDARY_FISH_ROOM = "pumpLegendaryFishRoom";

    //���ｵ��
    //��ǰ
    public const string STAT_FISHLORD_LEGENDARY_FISH_PLAYER = "fishlordLegendaryFishPlayer";
    //��ʷ
    public const string STAT_PUMP_LEGENDARY_FISH_ROOM_RANK = "pumpLegendaryFishRoomRank";

    //ʱ����������
    public const string STAT_PUMP_HOUR_MAX_ONLINE_PLAYER = "hour_max_online_player";

    //�����ƻ�
    public const string STAT_PUMP_VIP_EXCLUSIVE = "pumpVipExclusive";

    //���ں�ͳ��
    public const string STAT_PUMP_WECHAT_BENIFIT = "pumpWechatBenifit";

    //�ڲ���
    public const string STAT_PUMP_INNER_PLAYER = "pumpInnerPlayer";

    //��һ�ε�¼�������
    public const string STAT_PUMP_NEW_GUILD_GIFT_TIME = "pumpNewGuildGiftTime";

    //������Ƶͳ��
    public const string STAT_PUMP_PLAY_AD = "pumpPlayAd";

    //���ζһ�
    public const string STAT_PUMP_ROOM_EXCHANGE = "pumpRoomExchange";

    //���볡������ͳ��
    public const string STAT_PUMP_ROOM_PLAYER = "pumpRoomPlayer";

    //������������׶һ��б�
    public const string STAT_PUMP_DRAGON_PALACE_PLAYER = "pumpDragonPalacePlayer";

    //���ƽ��Я�����
    public const string STAT_PUMP_TURRET_ITEMS = "pumpTurretItems";

    //�ɳ�����ͳ��
    public const string STAT_PUMP_GROW_FUND = "pumpGrowFund";

    //���Я�����
    public const string STAT_PUMP_TURRET_PROPERTY = "pumpTurretProperty";

    //���Я����ҽű�
    public const string STAT_TURRET_PROPERTY = "statTurretProperty";

    //��Ϸ���Ʊ�
    public const string OP_CHANNEL_VER_CONTROL = "channelVerControl";

    //=========================================================================
    //׷��з���ͳ��
    //�齱ͳ��
    public const string STAT_PUMP_KILL_CRAB_LOTTERY = "pumpKillCrabLottery";
    //�齱ͳ������
    public const string STAT_PUMP_KILL_CRAB_LOTTERY_PLAYER = "pumpKillCrabLotteryPlayer";
    //׷��з����������
    public const string STAT_PUMP_KILL_CRAB_ROOM_CONSUME = "pumpKillCrabRoomConsume";
    //׷��з������ͳ��
    public const string STAT_PUMP_KILL_CRAB_PLAYER_TASK = "pumpKillCrabPlayerTask";

    //=========================================================================
    //����
    //ǩ��
    public const string STAT_PUMP_ACTIVITY_SIGN = "pumpActivitySign";

    //���ּ���
    public const string STAT_PUMP_ACTIVITY_EXCHANGE = "pumpActivityExchange";

    //ð��֮·
    public const string STAT_PUMP_ACTIVITY_TASK = "pumpActivityTask";

    //�������
    public const string STAT_PUMP_ACTIVITY_GIFT = "pumpActivityGift";

    //�����¼
    public const string STAT_PUMP_GIFT_RECORD = "pumpGiftRecord";

    //�齱ͳ��
    public const string STAT_PUMP_ACTIVITY_LOTTERY = "pumpActivityLottery";

    //�����ڱ������
    public const string STAT_PUMP_TURRET_LEVEL = "pumpNewPlayerTurretLevel";
    //�ڱ�����
    public const string STAT_PUMP_TOTAL_PLAYER_TURRET_LEVEL = "pumpTurretLevel";

    //����ֻ���֤��
    public const string PLAYER_PHONE_CODE = "playerPhoneCode";

    //�������߹���
    public const string STAT_PUMP_PLAYER_ITEM = "statPumpPlayerItem";

    //���߽���
    public const string STAT_PUMP_ONLINE_REWARD = "pumpOnlineReward";

    //�����ͳ��
    public const string STAT_PUMP_SD_ACT = "pumpSdAct";

    //�����齱ͳ��
    public const string STAT_PUMP_SD_LOTTERY_ACT = "pumpSdActLottery";

    //��ʧ��ͳ��
    public const string STAT_PUMP_NEW_GUILD_LOSE_POINT = "pumpNewGuildLosePoint";

    // VIP��Ȩ��ȡͳ��
    public const string STAT_VIP_RECORD = "statVipRecord";
    public const string STAT_TURRET_ITEMS = "statTurretItems";

    //�Ϻ�Ѱ��
    //����
    public const string STAT_PUMP_TREASURE_HUNT_PLAYER = "pumpTreasureHuntPlayer";
    //�ܼ�
    public const string STAT_PUMP_TREASURE_HUNT_ROOM = "pumpTreasureHuntRoom";
    //����
    public const string STAT_PUMP_TREASURE_HUNT_PLAYER_DETAIL = "pumpTreasureHuntPlayerDetail";

    //�߼������ƹ���
    public const string FISHLORD_ADVANCED_ROOM_CTRL = "fishlordAdvancedRoomControl";
    //�߼�������ͳ��
    public const string FISHLORD_ADVANCED_ROOM_ACT = "pumpAdvancedRoom";
    //�߼�������ͳ������
    public const string FISHLORD_ADVANCED_ROOM_ACT_DETAIL = "pumpAdvancedRoomDetail";
    //�߼������а�ǰ
    public const string FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR = "fishlordAdvancedPlayer";
    //�߼������а���ʷ
    public const string FISHLORD_ADVANCED_ROOM_ACT_RANK_HIS = "pumpAdvancedRoomRank";
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //�м�����һ���
    public const string STAT_FISHLORD_MIDDLE_PLAYER = "fishlordMiddlePlayer";

    //�м����淨����ͳ��
    public const string STAT_FISHLORD_MIDDLE_ROOM_INCOME = "pumpMiddleRoomIncome";
    //�м����һ�ͳ��
    public const string STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE = "pumpMiddleRoomExchange";

    //�м����һ�ͳ������
    public const string STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE_DETAIL = "pumpMiddleRoomExchangeDetail";

    //�м�����ʷ���а�
    public const string STAT_FISHLORD_MIDDLE_ROOM_RANK = "pumpMiddleRoomRank";
    //�м���������� //���賡
    public const string STAT_FISHLOR_ROOM_FU_DAI = "pumpRoomGift";

    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    //���賡�淨����ͳ��
    public const string STAT_FISHLORD_SHARK_ROOM_OUTLAY = "pumpSharkRoomOutlay";
    //����
    public const string STAT_FISHLORD_SHARK_ROOM_DETAIL = "pumpSharkRoomDetail";

    //��ը��ͳ��
    public const string STAT_FISHLORD_SHARK_ROOM_BOMB = "pumpSharkRoomBomb";
    //��ը��ͳ�����
    public const string STAT_FISHLORD_SHARK_ROOM_BOMB_DETAIL = "pumpSharkRoomBombDetail";

    //���賡�齱ͳ��
    public const string STAT_FISHLORD_SHARK_ROOM_LOTTERY = "pumpSharkRoomLottery";
    //���а�
    //��ǰ
    public const string STAT_FISHLORD_ARMED_SHARK_PLAYER = "fishlordArmedSharkPlayer";
    //��ʷ
    public const string STAT_FISHLORD_SHARK_RANK = "pumpArmedSharkRoomRank";

    //����ͳ��
    public const string STAT_FISHLORD_SHARK_ROOM_ENERGY_DROP = "pumpSharkRoomEnergyDrop";
    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    //��������
    public const string STAT_PUMP_MAINLY_TASK = "pumpMainlyTask";

    ////////�Ʋ�ͳ��
    //�Ʋ����
    public const string STAT_PUMP_PLAYER_BROKEN_INFO = "pump_player_borken_info";
    //�Ʋ����ֵ���
    public const string STAT_PUMP_PLAYER_PAY_AFTER_BROKEN = "pump_player_pay_after_broken";

    //��������ɳ��ֲ�
    public const string STAT_PUMP_PLAYER_ACTIVITY_TURRET = "pump_player_activity_turret";

    //���˳齱
    public const string STAT_PUMP_GOLD_FISH_LOTTERY = "pumpGoldFishLottery";
    //���˳齱�������
    public const string STAT_PUMP_GOLD_FISH_LOTTERY_PLAYER = "pumpPlayerGoldFishLottery";

    //ÿ������ͳ��
    public const string STAT_PUMP_DAILY_TASK = "pumpDailyTask";
    //ÿ������ͳ��
    public const string STAT_PUMP_WEEKLY_TASK = "pumpWeeklyTask";

    //ÿ�ս���ͳ��
    public const string STAT_PUMP_DAILY_REWARD = "pumpDailyTaskActivity";
    //ÿ�ܽ���ͳ��
    public const string STAT_PUMP_WEEK_REWARD = "pumpWeeklyTaskActivity";

    //ϵͳ��Ͷ
    public const string STAT_AIR_DROP_SYS = "airdropSys";
    //ϵͳ��Ͷ����
    public const string STAT_PUMP_AIR_DROP_HISTORY = "pumpAirdropHistory";

    //������ߴ���ͳ��
    public const string PUMP_PLAYER_LOGIN = "pumpPlayerLogin";

    //�������
    public const string PUMP_PLAYER_PLAY_TIME = "pumpPlayerPlayTime";
    //�������
    public const string STAT_PLAYER_PLAY_TIME = "statPlayerPlayTime";

    //������
    //�ܰ�
    public const string STAT_PUMP_KD_WEEK_RANK = "pumpKdWeekRank";
    //���հ�
    public const string STAT_PUMP_KD_ACTIVITY = "kdActivity";
    //���հ�
    public const string STAT_PUMP_KD_HISTORY_RANK = "kdHistoryRank";

    //��ͷ���ͳ��
    public const string STAT_BULLET_HEAD_GIFT = "pumpBulletHeadGift";

    //��ͷ�������
    public const string STAT_BULLET_HEAD_GIFT_PLAYER = "pumpBulletHeadGiftPlayer";

    //Χ�������Զ��帶���������
    public const string STAT_WJLW_Act_DEF_RECHARGE_REWARD = "wjlwActDefRechargeReward";
    //����淨ͳ��
    public const string STAT_WJLW_PUMP_GOLD_EARN = "pumpWjlwGoldEarn";
    //����淨�޸ĵ�ǰ����ӯ����
    public const string WJLW_ACT_DATA = "wjlwActData";
    //����淨ͳ��ÿ������
    public const string STAT_WJLW_PUMP_GOLD_TURN_INFO = "pumpWjlwGoldTurnInfo";
    //����淨ͳ�ƽ�����ȡ
    public const string STAT_WJLW_PUMP_GOLD_RECV_INFO = "pumpWjlwGoldRecvInfo";

    //����淨ÿ����ע����б�
    public const string STAT_WJLW_PUMP_GOLD_BET_PLAYER = "pumpWjlwGoldBetPlayer";
    //�����淨ͳ��
    public const string STAT_WJLW_PUMP_RECHARGE_EARN = "pumpWjlwRechargeEarn";
    //������
    public const string STAT_WJLW_PUMP_RECHARGE_WIN_INFO = "pumpWjlwRechargeWinInfo";
    //�Ƿ���ȡ
    public const string STAT_WJLW_PUMP_RECHARGE_RECV_INFO = "pumpWjlwRechargeRecvInfo";
    //��ע����б�
    public const string STAT_WJLW_PUMP_RECHARGE_BET_PLAYER = "pumpWjlwRechargeBetPlayer";

    //�������ͳ��
    public const string STAT_PLAYER_MONEY_REP = "statPlayerMoneyRep";

    //=============ת����
    //���а�ǰ
    public const string STAT_TURN_TABLE_FISH_ACTIVITY = "turntableFishActivity";
    //���а���ʷ
    public const string STAT_PUMP_TFISH_RANK = "pumpTFishRank";
    //����ͳ��
    public const string STAT_PUMP_TFISH = "pumpTFishStat";
    //����
    public const string STAT_PUMP_TFISH_PLAYER = "pumpTFishPlayer";

    //����ͳ��
    public const string STAT_PUMP_TFISH_ROOM_STAT = "pumpTFishRoomStat";
    //=========================================================================

    //�����������
    //���а�ͳ��
    //��ʷ
    public const string STAT_PUMP_NATIONAL_DAY_2018_RANk = "pumpNationalDay2018Rank";
    //��ǰ
    public const string STAT_FISHLORD_NATIONAL_DAY_ACTIVITY_2018 = "fishlordNationalDayActivity2018";
    //����ͳ��
    public const string STAT_FISHLORD_NATIONAL_DAY_ROOM_STAT = "pumpNationalDay2018Drop";
    //�齱ͳ��
    public const string STAT_PUMP_NATIONAL_DAY_2018_LOTTERY = "pumpNationalDay2018Lottery";
    //�������
    public const string STAT_PUMP_NATIONAL_DAY_2018_DROP = "pumpNationalDay2018Drop";
    //����
    public const string STAT_PUMP_NATIONAL_DAY_2018_LOTTERY_PLAYER = "pumpNationalDay2018LotteryPlayer";

    //����ͳ��
    public const string STAT_PUMP_NATIONAL_DAY_2018_TASK = "pumpNationalDay2018Task";
    //����ͳ������
    public const string STAT_PUMP_NATIONAL_DAY_2018_PLAYER_TASK = "statNationalDay2018PlayerTask";

	// ������������ҷֲ�
    public const string STAT_DRAGON_PALACE_JOIN_DISTRIBUTE = "statDragonPalaceDistribute";

    // ������ͳ��
    public const string PUMP_DRAGON_PALACE_JOIN = "pumpDragonPalace";
	
	//�ͷ�����/�����/��������-ϵͳ
    public const string PUMP_REPAIR_ORDER = "pumpRepairOrder";

    //�󻧻���������Ӽ�¼
    public const string GUIDE_LOST_PLAYER_INFO = "guideLostPlayerInfo";

   //���籭�󾺲����¾���
    public const string WORLD_CUP_MATCH_INFO = "worldCupMatchInfo";
    //���籭�󾺲¶���Ѻע
    public const string WORLD_CUP_MATCH_BET_INFO = "worldCupMatchBetInfo";
    // ���籭��Ҳ�����עͳ��
    public const string STAT_WORLD_CUP_MATCH_PLAYER_JOIN = "statWorldCupPlayerJoin";
    //���籭�󾺲½���
    public const string WORLD_CUP_MATCH_REWARD = "worldCupReward";

    //�����������
    public const string PUMP_NEW_GUIDE = "pumpNewGuild";
    public const string PUMP_PLAYER_REG = "pumpPlayerReg";

    //����/˥��ըըը
    public const string PUMP_BULLET_HEAD_RANK = "pumpTorpedoRank"; //��ʷ����
    public const string BULLET_HEAD_ACTIVITY = "torpedo_activity"; //��ǰ
    public const string PUMP_BULLET_HEAD_GUARANTEE = "pumpTorpedoGuarantee";//��Χ��ʷ

    //��������
    public const string FISHLORD_DRAGON_PALACE_RANK = "fishlordDragonPalaceRank";//������ʷ����
    public const string FISHLORD_DRAGON_PALACE_PLAYER = "fishlordDragonPalacePlayer";//���۵�ǰ����

    //�����CDKEY������
    public const string CD_KEY_MULT = "cdkeyMult";

    //�ι�����Ӫ״��
    public const string PUMP_SCRATCH_EARN = "pumpScratchEarn";

    //�ι��ֶһ�
    public const string PUMP_SCRATCH_EXCHANGE_RES = "pumpScratchExchangeRes";

    //���ͬ������ʧ��
    public const string PUMP_WORD2_LOGIC_ITEM_ERROR = "pumpWorld2LogicItemError";

    //��ҵ�������
    public const string PUMP_PLAYER_ITEM = "pumpPlayerItem";

    //���䲶���ɱ����
    public const string FISHLORD_KS_SCORE = "fishlordKsScore";

    //��桪������Ч��ͨ
    public const string ADVERTISEMENT_XGT = "advertisement_xgt";

    //ˮ����
    //ˮ��������
    public const string FRUIT_ROOM = "fruit_room";
    //�������
    public const string FRUIT_RESULT_CONTROL = "fruitResultControl";
    //ˮ�����ڰ�����
    public const string FRUIT_BW_LIST = "fruitWhiteBlackList";

    //ˮ����ÿ��ӯ����
    public const string FRUIT_EVERY_DAY = "FruitEveryday";

    //�޸Ļ�������߻���
    public const string FISHLORD_BAOJIN_SYS = "fishlord_baojin_sys";
    ////////////////////////////////////////////////////////////////////////////////
    //��ͷͳ��
    public const string FISHLORD_BULLET_HEAD = "fishlordBulletHead";
    //���㵯ͷͳ��
    public const string PUMP_BULLET_HEAD = "pumpBulletHead";
    //����ÿ�յ�ͷͳ��
    public const string PUMP_BULLET_HEAD_DAILY = "pumpBulletHeadDaily";
    //������㵯ͷͳ��
    public const string PUMP_BULLET_HEAD_FISH_USE = "pumpBulletHeadFishUse";
    //���㱳����ͷͳ��
    public const string PUMP_BULLET_HEAD_BAG_USE = "pumpBulletHeadBagUse";
    //////////////////////////////////////////////////////////////////////////////
    //��������ͳ��
    public const string PUMP_TORPEDO = "pumpTorpedo";
    //����ÿ������ͳ��
    public const string PUMP_TORPEDO_DAILY = "pumpTorpedoDaily";
    //�����������ͳ��
    public const string PUMP_TORPEDO_FISH_USE = "pumpTorpedoFishUse";
    //���㱳������ͳ��
    public const string PUMP_TORPEDO_BAG_USE = "pumpTorpedoBagUse";
    /////////////////////////////////////////////////////////////////////////////////

    //��������
    public const string CONFIG_POLAR_LIGHTS_PUSH = "JpushMsgList";

    //��ʱ�
    public const string PB_ACTIVITY_CFG = "pbActivityCfg";

    //���ջ
    public const string PUMP_ACTIVITY_TEMPLATE = "pumpActivityTemplate";

    // �����Ϣ��
    public const string PLAYER_INFO = "player_info";

    //ʵ����ʷ���Ž��
    public const string STAT_PLAYER_EXCHANGE = "statPlayerExchange";

    //�һ���ʷ��¼
    public const string PUMP_PLAYER_EXCHANGE = "pumpPlayerExchange";

    // �ʼ���
    public const string PLAYER_MAIL = "playerMail";

    //��ɾ�����ʼ���
    public const string PUMP_MAIL_DEL = "pumpMailDel";

    //����ȡ�ʼ���
    public const string PUMP_MAIL_RECV = "pumpMailRecv";

    //�������ʼ�����ʧ��
    public const string PUMP_FISH_BAO_JIN_SEND_REWARD_FAIL = "pumpFishBaojinSendRewardFail";

    // �����
    public const string PLAYER_QUEST = "player_quest";

    // �ʼ�����
    public const string CHECK_MAIL = "checkMail";

    // ����˺ű�
    public const string PLAYER_ACCOUNT = "AccountTable";

    // GM�˺ű�
    public const string GM_ACCOUNT = "GmAccount";

    // ������
    public const string COUNT_TABLE = "OpLogCurID_DWC";

    // GM������־��
    public const string OPLOG = "OpLogDWC";

    //������־��
    public const string OP_FORVER_LOG = "OpForverLogDWC";

    // Ȩ�ޱ�
    public const string RIGHT = "rightDWC";

    // ��ҵ�½��ʷ
    public const string PLAYER_LOGIN = "LoginLog";

    // ��IP��
    public const string BLOCK_IP = "blockIP";

    // ��̨��ֵ
    public const string GM_RECHARGE = "gmRecharge";

    // ����Ӧ��
    public const string JPUSH_APP = "jpushAppInfoList";

    // ��ҽ�ң���ʯ�仯�ܱ�
    public const string PUMP_PLAYER_MONEY = "pumpPlayerMoney";
    // ţţ���ֵ����ͱ�
    public const string PUMP_COWS_CARD = "logCowsInfo";

    // ��ҽ�ң���ʯ�仯��ϸ��
    public const string PUMP_PLAYER_MONEY_DETAIL = "logPlayerInfo";

    // ÿ�������
    public const string PUMP_DAILY_TASK = "pumpDailyTask";

    // �ɾ�
    public const string PUMP_TASK = "pumpTask";

    // �ʼ�
    public const string PUMP_MAIL = "pumpMail";

    // ��Ծ����
    public const string PUMP_ACTIVE_COUNT = "pumpActiveCount";

    // ��Ծ����
    public const string PUMP_ACTIVE_PERSON = "pumpActivePerson";

    // ͨ������ͳ��
    public const string PUMP_GENERAL_STAT = "pumpGeneralStat";

    // ��������
    public const string PUMP_SEND_GIFT = "pumpSendGift";

    // ���ͳ��
    public const string PUMP_PHOTO_FRAME = "pumpPhotoFrame";

    // �ܵ�����ͳ��
    public const string PUMP_TOTAL_CONSUME = "pumpTotalConsume";

    // �����������
    public const string PUMP_COIN_GROWTH = "pumpCoinGrowth";

    // ���������ʷ����
    public const string PUMP_COIN_GROWTH_HISTORY = "pumpCoinGrowthHistory";

    // ���䲶�����ı�
    public const string FISH_CONSUME = "pumpFishConsume";
    // ���������١�ɢ�������
    public const string FISH_CONSUME_ITEM = "pumpFishItemConsume";

    // ����ÿ����������
    public const string PUMP_FISHLORD_EVERY_DAY = "fishlordEveryDay";

    // ����ÿ����������
    public const string PUMP_CROCODILE_EVERY_DAY = "CrocodileEveryday";

    // ����ÿ����������
    public const string PUMP_DICE_EVERY_DAY = "DiceEveryday";

    // �ټ���ÿ����������
    public const string PUMP_BACCARAT_EVERY_DAY = "BaccaratEveryday";

    // ����ÿ����������
    public const string PUMP_DRAGON_EVERY_DAY = "DragonEveryday";

    // ���㹫԰ÿ����������
    public const string PUMP_FISHPARK_EVERY_DAY = "fishParkEveryDay";

    // �ں�÷��ÿ����������
    public const string PUMP_SHCD_EVERY_DAY = "ShcdCardsEveryday";

    // ��ţÿ����������
    public const string PUMP_CALFROPING_EVERY_DAY = "ropingEveryDay";

    // �ټ��������ׯ�����ѯ
    public const string PUMP_PLAYER_BANKER = "pumpBaccaratPlayerBanker";

    // ţţÿ����������
    public const string PUMP_COWS_EVERY_DAY = "CowsEveryday";

    // ţţ�����ׯ�����ѯ
    public const string PUMP_PLAYER_BANKER_COWS = "pumpCowsPlayerBanker";

    // ������ע���񽱴���
    public const string PUMP_CROCODILE_BET = "CrocodileBetInfo";

    //���۱�����ע���񽱴���
    public const string PUMP_BZ_BET = "BzBetInfo";

    // ����������ע�������
    public const string PUMP_DICE = "dice_table";

    //ˮ������ע���񽱴���
    public const string PUMP_FRUIT_BET = "FruitBetInfo";

    // ���ͳ��
    public const string PUMP_ALL_FISH = "AllFishLog";
    // ���㹫԰���ͳ��
    public const string PUMP_ALL_FISH_PARK = "AllFishParkLog";

    // ����ʱ�ɵ�ӯ����
    public const string PUMP_OLD_EARNINGS_RATE = "pumpOldEarningsRate";

    // ���䲶��׶α�
    public const string PUMP_FISH_TABLE_LOG = "FishTableLog";
    // ���㹫԰�׶α�
    public const string PUMP_FISH_PARK_TABLE_LOG = "FishParkTableLog";

    // ����������
    public const string PUMP_MAXONLINE_PLAYER = "max_online_player";

    // ÿ��0��������ҵĽ���ܺ�ͳ�Ʊ�
    public const string PUMP_PLAYER_TOTAL_MONEY = "pumpPlayerTotalMoney";

    // �һ�ͳ��
    public const string PUMP_EXCHANGE = "pump_exchange";

    // ���ѵ�ͳ��
    public const string PUMP_RECHARGE = "pump_recharge";

    //���ջ
    public const string PUMP_7DAYACTIVITY = "pump7DaysActivity";

    //�������ÿ�չ���
    public const string PUMP_MATERIAL_GIFT = "pumpMaterialGift";

    //ת�̳齱
    public const string PUMP_DIAL_LOTTERY = "pumpDialLottery";

    //���Գ齱��λͳ��
    public const string STAT_LABA_LOTTERY_PROB = "statLabaLotteryProb";

    //���Գ齱��ҳ齱��¼
    public const string PUMP_LABA_LOTTERY = "pumpLabaLottery";

    //������ҳ齱����ͳ��
    public const string STAT_LABA_LOTTERY_PLAYER = "statLabaLotteryPlayer";
    // ������������
    public const string STAT_LABA_LOTTERY_TOTAL = "statLabaTotal";

    //���Իͳ��
    public const string STAT_LABA_TOTAL = "statLabaTotal";

    //����ʢ��
    public const string PUMP_WP_PLAYER = "pumpWpPlayer";
    public const string PUMP_WP_COUNT = "pumpWpCount";
    public const string PUMP_WP_ITEM_CONSUME = "pumpWpItemConsume";

    //����ʢ��
    public const string PUMP_GOLD_FEAST = "pumpGoldFeast";

    // ������¼
    public const string PUMP_DONATE_PUPPET_REC = "pumpPlayerPuppetDonate"; //���������ͳ��
    public const string STAT_FISHLORD_BAOJIN_PLAYER = "statFishBaojinPlayer";
    public const string PUMP_FISH_GOLD_FLOW = "pumpFishGoldFlow";

    //�������а���ʷ��
    public const string STAT_FISHLORD_BAOJIN_WEEK_RANK = "fishlord_baojin_week_rank";
    public const string STAT_FISHLORD_BAOJIN_DAILY_RANK = "fishlord_baojin_daily_rank";
    public const string PUMP_FISH_BAOJIN_PLAYER = "pumpFishBaojinPlayer";
    //�������а񣨵�ǰ��/�գ�
    public const string STAT_FISHLORD_BAOJIN_RANK = "fishlord_baojin_player";

    //������������ͳ��
    public const string STAT_FISHLORD_DRAGON_PALACE_INCOME_OUTLAY = "pumpDragonPalace";
    //����������ҷֲ�
    public const string STAT_FISHLORD_DRAGON_PALACE_PLAYER_DISTRIBUTE = "statDragonPalaceDistribute";

    //����������ͳ��
    public const string STAT_FISHLORD_INCOME_OUTLAY = "pumpFishBaojinIncomeOutlay";

    //����������ͳ��
    public const string STAT_FISHLORD_OUT_LAY = "pumpFishBaojinOutlay";

    //����������ͳ��
    public const string STAT_FISHLORD_TASK = "pumpFishBaojinQuest";

	//������ż����
    public const string STAT_PUPPET_ACT = "pumpPuppetAct";

    //���/��������λ����
    public const string STAT_PUPPET_REWARD_RECV = "pumpPuppetRewardRecv";

    //�������ܾ�����ż����
    public const string STAT_PUPPET_SVR_DONATE = "pumpPuppetSvrDonate";

    //��ڼ�
    public const string STAT_ACTIVITY_PUPPET = "activityPuppet";

    //�������������
    public const string STAT_ACTIVITY_PUPPET_RANK = "activityPuppetRank";

    //���������ã�
    public const string STAT_ACTIVITY_PUPPET_GAIN_RANK = "activityGainPuppetRank";

    // ������ҵ�ͳ��
    public const string PUMP_BAOJIN_PLAYER = "pumpFishBaojinPlayer";
    public const string STAT_BAOJIN_PLAYER = "statFishBaojinPlayer";

    // ����������ֲ�
    public const string STAT_BAOJIN_JOIN_DISTRIBUTE = "statFishBaojinJoinDistribute";

    // ����ÿ������
    public const string BAOJIN_DAILY_RANK = "fishlord_baojin_daily_rank";
    // ����ÿ������
    public const string BAOJIN_WEEK_RANK = "fishlord_baojin_week_rank";

    //����
    public const string FISHLORD_BAOJIN_PLAYER = "fishlord_baojin_player";

    // ���ǳ齱
    public const string PUMP_STAR_LOTTERY = "pumpStarLottery";
    public const string PUMP_STAR_LOTTERY2 = "pumpStarLottery2";

    //������ͳ��
    public const string PUMP_CHIP_FISH = "pumpChipFish";

    // ����������ݵ�ͳ��
    public const string STAT_STAR_LOTTERY2 = "statStarLottery2";

    public const string STAT_STAR_LOTTERY_DETAIL = "statStarLotteryDetail";

    // ��ֵ�û�ͳ��
    public const string PUMP_RECHARGE_PLAYER = "pumpRechargePlayer";
    public const string PUMP_RECHARGE_PLAYER_NEW = "pump_recharge_player";

    // ���һ���ڸ�����Ϸ�ڣ���Ϸʱ���ۼ�
    public const string PUMP_GAME_TIME_FOR_PLAYER = "pumpGameTimeForPlayer";

    // ������ҷ��ڴ���,����ȼ�
    public const string PUMP_NEW_PLAYER_FIRECOUNT_FISHLEVEL = "pumpNewPlayerFireCountFishLevel";

    // ���
    public const string GIFT = "gift";

    // ������
    public const string GIFT_CODE = "cdkey";
    public const string CD_PICI = "cdkeyPICI";

    // �һ���
    public const string EXCHANGE = "exchange";



    // ��Ӫ�����
    public const string OPERATION_NOTIFY = "optionNotify";

    // ͨ����Ϣ
    public const string OPERATION_SPEAKER = "operationSpeaker";

    // ���㷿��
    public const string FISHLORD_ROOM = "fishlord_room";

    //���䲶��ÿ��
    public const string FISHLORD_EVERY_DAY = "fishlordEveryDay";

    public const string FISHLORD_LOBBY = "fishlord_lobby";

    // ���㹫԰����
    public const string FISHPARK_ROOM = "fishpark_room";

    // ��������
    public const string FISHLORD_ROOM_DESK = "fishlord_table";
    // ���㹫԰����
    public const string FISHPARK_ROOM_DESK = "fishpark_table";

    // ���㷿��
    public const string CROCODILE_ROOM = "crocodile_room";
    
    //������ڰ���������
    public const string CROCODILE_WB_LIST = "crocodileWhiteBlackList";

    //���۱���������
    public const string DB_BZ_ROOM = "bz_room";

    //��ʯ����ÿ����Ϸ����
    public const string JEWEL_EVERY_DAY_STAT = "JewelEveryday";

    //��ʯ����ȫ�ֲ���
    public const string JEWEL_PARAM_CONTROL = "jewel_cfg";

    //��ʯ�������ӯ�����޵���
    public const string JEWEL_ROOM = "jewel_room";

    //���۱���ÿ��ӯ����
    public const string BZ_DAILY = "BzEveryday";

    //���۱���ڰ���������
    public const string Bz_WB_LIST = "bzWhiteBlackList";

    // ��������
    public const string DICE_ROOM = "dice_room";

    // �ټ��ַ���
    public const string BACCARAT_ROOM = "baccarat_room";

    // ţţ����
    public const string COWS_ROOM = "cows_room";

    // ��������
    public const string DRAGON_ROOM = "dragons_room";

    // �ں�÷������
    public const string SHCDCARDS_ROOM = "shcdcards_room";

    //�ں�÷���ƾֲ�ѯ
    public const string SHCD_CARD_BOARD = "logShcdCardBoard";

    //�ں�÷���ڰ�����
    public const string SHCD_CARD_SPECIL_LIST = "shcdcards_specil_list";

    //�ں�÷��ɱ�ַŷ�LOG��¼�б�
    public const string SHCD_CARDS_CTRL_LIST = "logShcdWBKillSendScore";

    //ˮ䰴��������ӯ����
    public const string SHUIHZ_PLAYER = "shuihz_player";

    public const string SHUIHZ_PLAYER_EVERY_DAY = "shuihzPlayerEveryDay";
    //ˮ䰴���ӯ����
    public const string SHUIHZ_ROOM = "shuihz_room";

    //ˮ䰴�ÿ��ӯ����
    public const string SHUIHZ_DAILY = "shuihzEveryDay";

    //ˮ䰴�ÿ����Ϸ����鿴
    public const string SHUIHZ_DAILY_STATE = "logShuihzBonus";

    //ˮ䰴�ÿ�մ�����������ͳ��
    public const string SHUIHZ_REACH_LIMIT = "shuihzReachLimit";

    // ��ţ����
    public const string CALF_ROPING_ROOM = "calfRoping_lobby";
    // ��ţ ţ�ķ��� ͳ��
    public const string CALF_ROPING_LOG = "ropingLog";
    // ��ţ�ؿ�ͳ��
    public const string CALF_ROPING_PASS_LOG = "ropingPassLog";

    // ������Ϸģʽ�µ�ӯ����
    public const string DRAGON_TABLE = "dragons_table";

    // ţţ�����ͱ�
    public const string COWS_CARDS = "cows_cards";

    //ţţ�ƾֲ�ѯ
    public const string LOG_COWS_CARD_BOARD = "logCowCardBoard";

    //ţţ�ڰ����������б�
    public const string COW_CARD_SPECIL_LIST = "cowsWhiteBlackList";

    //ţţɱ�ַŷ�LOG��¼�б�
    public const string COWS_CARD_CTRL_LIST = "logCowWBKillSendScore";

    // �ں�÷���Ľ������
    public const string SHCD_RESULT = "shcdcards_gm_cards";

    // ���¼������
    public const string RELOAD_FISHCFG = "fishlord_cfg";
    public const string RELOAD_FISHPARK_CFG = "fishpark_cfg";

    // �ͷ���Ϣ��
    public const string SERVICE_INFO = "serviceInfo";

    public const string COMMON_CONFIG = "common_config";

    // ��Ϸ��ֵ��Ϣ
    public const string GAME_RECHARGE_INFO = "pay_infos";

    // ֧���ܱ�
    public const string RECHARGE_TOTAL = "player_pay";

    //ͨ������֧��
    public const string RECHARGE_TOTAL_AIBEI = "aibei_pay";

    // ���ѵ�ֲ�
    public const string RECHARGE_DISTRIBUTION = "rechargeDistribution";

    // ������Ϸ��
    public const string TEST_SERVER = "TestServers";

    // ���䲶����ұ�
    public const string FISHLORD_PLAYER = "fishlord_player";
    // ���㹫԰��ұ�
    public const string FISHPARK_PLAYER = "fishpark_player";

    // ͷ��ٱ�
    public const string INFORM_HEAD = "informHead";

    // �߳����
    public const string KICK_PLAYER = "KickPlayer";

    public const string DAY_ACTIVATION = "day_activation";
    // ��������ͳ����
    public const string CHANNEL_STAT_DAY = "channelStatDay";

    // ����ͨ�����������ĳ�ֵ
    public const string CHANNEL_RECHARGE_AIBEI = "channelRechargeByAibei";

    // ������ص�ͳ������
    public const string CHANNEL_TD = "channelTalkingData";
    // ������صĳ�ֵͳ��
    public const string CHANNEL_TD_PAY = "channelTalkingDataPay";


    //����������״θ���
    public const string PUMP_OLD_FIRST_RECHARGE = "pumpOldFirstRecharge";

    // ���ӵ�е��ܽ��ͳ����
    public const string TOTAL_MONEY_STAT_DAY = "totalMoneyStatDay";

    // VIP��ʧ
    public const string RLOSE = "vipLose";

    // �����ܹھ�
    public const string MATCH_GRAND_PRIX_WEEK_CHAMPION = "fishlord_match_champion";
    // boss��¼
    public const string PUMP_BOSSINFO = "logBossInfo";

    public const string MATCH_GRAND_PRIX_DAY = "fishlord_match_day";

    // ��ȫ�˺��б�
    public const string MATCH_GRAND_SAFE_ACCOUNT = "fishlord_match_safe_account";

    // �������ͳ��
    public const string STAT_PLAYER_DRAGON = "statPlayerDragonBall";
    // ÿ�������ܼ�
    public const string STAT_DRAGON_DAILY = "statDragonBallDaily";

    // ��Ҹ��Ѽ��
    public const string PUMP_RECHARGE_FIRST = "pumpRechargeFirst";
    // ����ۼƵ���Ϸʱ��
    public const string STAT_PLAYER_GAME_TIME = "statPlayerGameTime";

    // �������ʱ���
    public const string PUMP_PLAYER_ONLINE_TIME = "pumpPlayerOnlineTime";

    // ��֧�ܼ�
    public const string STAT_INCOME_EXPENSES = "statIncomeExpenses";

    // ��֧�ܼƴ���
    public const string STAT_INCOME_EXPENSES_ERROR= "statIncomeExpenses_error";

    // ÿ����֧�������ݿ����
    public const string STAT_INCOME_EXPENSES_REMAIN = "statIncomeExpensesRemain";

    // ��֧�ܼ� ��
    public const string STAT_INCOME_EXPENSES_NEW = "statIncomeExpensesNew";

    // ÿСʱ����ͳ��
    public const string STAT_RECHARGE_HOUR = "statRechargeHour";

    // ÿСʱ����ͳ�Ʒ�����
    public const string STAT_RECHARGE_HOUR_BYCHANNEL = "statRechargeHourByChannel";

    // ÿСʱ��������
    public const string STAT_ONLINE_HOUR = "statOnlinePlayerNumHour";

    // ��Ծ��Ϊ--�û�ϲ�� ����ʱ��
    public const string STAT_GAME_TIME_FOR_PLAYER_FAVOR_RESULT = "statGameTimeForPlayerFavorResult";
    // ʱ���ֲ�
    public const string STAT_GAME_TIME_FOR_DISTRIBUTION_RESULT = "statGameTimeForDistributionResult";
    // �׸���Ϸʱ���ֲ�
    public const string STAT_FIRST_RECHARGE_GAME_TIME_DISTRIBUTION_RESULT = "statFirstRechargeGameTimeDistributionResult";
    // �״ι���Ʒѵ�ֲ�
    public const string STAT_FIRST_RECHARGE_POINT_DISTRIBUTION_RESULT = "statFirstRechargePointDistributionResult";
    // �����ע���ͳ��
    public const string STAT_PLAYER_GAME_BET_RESULT = "statPlayerGameBetResult";
    // ���������û������ע�ֲ�
    public const string STAT_NEW_PLAYER_OUTLAY_DISTRIBUTION = "statNewPlayerOutlayDistributionResult";

    public const string STAT_NEW_PLAYER_ENTER_ROOM = "pumpNewPlayerGame";
    // ������ҷ��ڴ����ֲ�
    public const string STAT_NEW_PLAYER_FIRECOUNT_DISTRIBUTION = "statNewPlayerFireCountDistributionResult";
    // ������ҷ�����ȼ��ֲ�
    public const string STAT_NEW_PLAYER_FISHLEVEL_DISTRIBUTION = "statNewPlayerFishLevelDistributionResult";

    public const string PERSONTIME_GLOBAL_DAY = "personTimeGlobalDay";

    public const string PUMP_OLD_PLAYER_LOGIN = "pumpOldPlayerLogin";
    public const string PUMP_SIGN = "pumpSign";
    public const string PUMP_SIGN_PLAYER = "pumpSignPlayer";
    public const string STAT_SIGN_PLAYER = "statSignPlayer";
    public const string PUMP_SIGN_REWARD = "pumpSignReward";

    public const string PUMP_NEW_PLAYER_TASK = "pumpNewPlayerTask";
    public const string PUMP_NEW_SEVEN_DAY = "pumpNewSevenDay";

    public const string PUMP_NY_GIFT = "pumpNYGift";
    public const string STAT_NY_GIFT = "statNYGift";
    public const string PUMP_NY_ACC_RECHARGE = "pumpNYAccRecharge";
    public const string PUMP_NY_ADVENTURE = "pumpNYAdventure";
    public const string PUMP_NY_ADVENTURE_JOIN = "pumpNYAdventureJoin";

    public const string PUMP_WUYI_JOIN = "pumpWuyiJoin";
    public const string STAT_WUYI_JOIN = "statWuyiJoin";
    public const string PUMP_WUYI_REWARD_RESULT = "pumpWuyiRewardResult";

    public const string ANYSDK_PAY = "anysdk_pay";
    //////////////////////////////////////////////////////////////////////////
    // GM�˺����ͷ���
    public const string GM_TYPE = "gmTypeGroup";

    //////////////////////////////////�.................////////////////////////////////////////
    public const string FISHLORD_ACT_BENEFIT_RECV = "FishlordActBenefitRecv";

    //��������¼��ѯ
    public const string PUMP_PLAYER_CHAT = "pumpChat";

    //С��Ϸ��������
    public const string CHANNEL_OPEN_CLOSE_GAME = "channelOpenCloseGame";

    //����ڻ 
    //��ȡ����
    public const string PUMP_ND_ACT = "pumpNdAct";
    //���а�
    public const string PUMP_NATIONAL_DAY_ACTIVITY = "nationalDayActivity";

    //�����ػݻ
    public const string PUMP_JIN_QIU_RECHARGE_LOTTERY = "pumpJinQiuRechargeLottery";

    //��ʥ�ڻ
    //���а�
    public const string PUMP_HALLOWMAS_ACT_RANK = "hallowmasActivity";
    //��ȡ����
    public const string PUMP_HALLOWMAS_ACT_RECV = "pumpHallowmas";

    //ǿ��������ѯ
    public const string PUMP_FORCE_UPDATE_REWARD = "pumpForceUpdateReward";

    //ʥ����/Ԫ���
    public const string PUMP_CHRISTMAS = "pumpChristmas";

    //���ᱦ���а�
    public const string FISHLORD_SPITTOR_SNATCH_ACTIVITY = "fishlordSpittorSnatchActivity";

    //���ᱦ��ȡ��������ͳ��
    public const string PUMP_SPITTOR_SNATCH = "pumpSpittorSnatch";

    //����˱���
    public const string PUMP_PANIC_BOX = "pumpPanicBox";
    //����˱������
    public const string PUMP_PANIC_BOX_PLAYER = "pumpPanicBoxPlayer";

    //���䳡����ͳ��
    public const string PUMP_BW = "pumpBw";

    //΢�Ź��ں�ǩ��ͳ��
    public const string PLAYER_WECHAT_RECV_STAT = "player_wechatRecvStat";
    //΢�Ź��ں�ͳ��
    public const string PLAYER_OPENID = "player_openId";

    //////////////////////////////////////////////////////////////////////////
    // ����ã����ÿ�ʼ������ʱ��
    public const string ACT_CHANNEL100003 = "actChannelPlayer100003";

    public const string STAT_CHANNEL100003 = "statChannelPlayer100003";
}

public static class StrName
{
    public static string[] s_rechargeType = { "�����" };

    public static string[] s_statLobbyName = { "ȫ��", "��������", "С����", "vip�ȼ��ֲ����", "�ϴ�ͷ��",
                                               "�ǳ��޸�", "ǩ���޸�", "�Ա��޸�", "ͷ�����", "���߽���",
                                             "�ȼý�","���������","������ȡ��"};

    //public static string[] s_gameName = { "����", "���䲶��", "������", "��������", "����ţţ", "�ټ���", "����", "��ţ", 
    //                                        "ץ����", "���㹫԰", "�ں�÷��","","ˮ䰴�","���۱���"};

    public static string[] s_gameName = { "����", "���䲶��", "������", "����ţţ", "�ں�÷��","ˮ䰴�","���۱���"};


    //public static string[] s_gameName1 = { "ϵͳ", s_gameName[1], s_gameName[2], s_gameName[3], 
    //                                         s_gameName[4], s_gameName[5], s_gameName[6],
    //                                         s_gameName[7], s_gameName[8], s_gameName[9],s_gameName[10] };

    public static string[] s_roomName = { "������", "�м���", "�߼���", "��Ƭ��" };

    public static string[] s_fishLordRoomName = { "������", "�м���", "�߼���", "VIPר��","������","����","����������"};

    public static string[] s_shcdRoomName = { "", "��ҳ�", "���鳡" };

    //public static string[] s_fishRoomName = { s_roomName[0], s_roomName[1], s_roomName[2], s_roomName[3], 
    //                                            "��ͨ��������", "��ͨ���м���", "��ͨ���߼���", "��ͨ����ʦ��", "����", "", "������", "", "", "", "����","","","","","����������" };

    public static string[] s_fishRoomName = { s_roomName[0], s_roomName[1], s_roomName[2], s_roomName[3], 
                                                "����������", "���賡", "�߼�������", "���ﳡ", "ʥ�޳�", "", "������", "", "", "", "����","","","","","����������" };

    public static string[] s_dragonRoomName = { "������", "�߼���", "��ʦ��" };

    public static string[] s_stageName = { "������", "������", "С����", "����", "С����", "�е���", "�����" };

    private static Dictionary<string, string> s_gameName2 = new Dictionary<string, string>();
    
    public static string[] s_cowsArea = { "��", "��", "��", "��" };

    public static string[] s_shcdArea = { "����", "����", "÷��", "����", "��С��" };
    
    public static string[] s_dragonArea = { "���ձ���", "��������", "��������" };

    public static string[] s_shuihzArea = { "���ձ���","С�������"};

    public static string[] s_wishCurse = { "ף��", "����" };

    public static string[] s_starLotteryName = { "��ͨ�齱", "��ͭ�齱", "�����齱", "�ƽ�齱", "��ʯ�齱", "����齱" };

    public static string[] s_bzArea = { "�챼��","�̱���","�Ʊ���","�챦��","�̱���","�Ʊ���","��µ�","�̰µ�","�ưµ�","�����","�̴���","�ƴ���","�����н�","�������","���ֲʽ�"};

    // ��ǰ���ߵ���ϷID�б�
    public static int[] s_onlineGameIdList = {0, (int)GameId.fishlord, /*(int)GameId.crocodile, (int)GameId.cows,(int)GameId.shcd,
                                             (int)GameId.shuihz,(int)GameId.bz, (int)GameId.fruit, (int)GameId.jewel*/};

    public static Dictionary<int, string> s_gameName3 = new Dictionary<int, string>();
    public static Dictionary<int, string> s_roomList = new Dictionary<int, string>();
    public static Dictionary<int, string> s_gameRealLoseWinRankGameItem = new Dictionary<int, string>();
    static StrName()
   {
       //s_gameName3.Add((int)GameId.lobby, s_gameName[0]);
       //s_gameName3.Add((int)GameId.fishlord, s_gameName[1]);
       //s_gameName3.Add((int)GameId.crocodile, s_gameName[2]);
       //s_gameName3.Add((int)GameId.dice, s_gameName[3]);
       //s_gameName3.Add((int)GameId.cows, s_gameName[4]);
       //s_gameName3.Add((int)GameId.baccarat, s_gameName[5]);
       //s_gameName3.Add((int)GameId.dragon, s_gameName[6]);
       //s_gameName3.Add((int)GameId.calf_roping, s_gameName[7]);
       //s_gameName3.Add((int)GameId.prize_claw, s_gameName[8]);
       //s_gameName3.Add((int)GameId.fishpark, s_gameName[9]);
       //s_gameName3.Add((int)GameId.shcd, s_gameName[10]);
       //s_gameName3.Add((int)GameId.shuihz, "ˮ䰴�");
       //s_gameName3.Add((int)GameId.bz,"���۱���");

       s_gameName3.Add((int)GameId.lobby, s_gameName[0]);
       s_gameName3.Add((int)GameId.fishlord, s_gameName[1]);

       //������Ϸ���� //{ "������", "�м���", "�߼���", "VIPר��","������","����","��������","���ﳡ"};
       s_roomList.Add((int)RoomId.room1, s_fishLordRoomName[0]);
       s_roomList.Add((int)RoomId.room2, s_fishLordRoomName[1]);
       s_roomList.Add((int)RoomId.room3, s_fishLordRoomName[2]);
       s_roomList.Add((int)RoomId.room4, "��Ƭ��");
       s_roomList.Add((int)RoomId.room6, "���賡");

       s_roomList.Add((int)RoomId.room5, "����������");
       s_roomList.Add((int)RoomId.room7, "�߼�������");
       s_roomList.Add((int)RoomId.room8, "���ﳡ");
       s_roomList.Add((int)RoomId.room9, "ʥ�޳�");

       //s_roomList.Add((int)RoomId.room11, s_fishLordRoomName[4]);
       //s_roomList.Add((int)RoomId.room15, s_fishLordRoomName[5]);
       //s_roomList.Add((int)RoomId.room20, s_fishLordRoomName[6]);
   }

    public static IOrderedEnumerable<KeyValuePair<int, string>> getAllUseGame()
    {
        var arr = from s in s_gameName3
                  orderby s.Key ascending
                  select s;
        return arr;
    }

   public static string getGameName3(int key) 
   {
       if (s_gameName3.ContainsKey(key))
           return s_gameName3[key];
       return key.ToString();
   }
       
    public static string getGameName(string key)
    {     
        if (s_gameName2.Count == 0)
        {
            //{ "����", "���䲶��", "������", "����ţţ", "�ں�÷��","ˮ䰴�","���۱���"};
            s_gameName2.Add("lobby", s_gameName[0]);
            s_gameName2.Add("fish", s_gameName[1]);
            s_gameName2.Add("crocodile", s_gameName[2]);
            s_gameName2.Add("Cows", s_gameName[3]);
            s_gameName2.Add("ShcdCards",s_gameName[4]);
            s_gameName2.Add("Shz", s_gameName[5]);
        }

        if (s_gameName2.ContainsKey(key))
            return s_gameName2[key];
        return key;
    }

    // ��ֵ״̬
    public static string[] s_rechargeState = { "�ѵ���", "����֧��", "֧���ɹ�" };
}

// public enum PaymentType
// {
//     e_pt_none = 0,
//     e_pt_anysdk,        //anysdk�ۺ�
//     e_pt_qbao,          //Ǯ��
//     e_pt_baidu,         //�ٶ�
//     e_pt_max,
// }

// ֧��״̬
public struct PayState
{
    //ȫ��
    public const int PAYSTATE_ALL = -1;
    // �ɹ�
    public const int PAYSTATE_SUCCESS = 0;
    // ������֧��
    public const int PAYSTATE_HAS_REQ = 1;
    // ֧���ɹ�
    public const int PAYSTATE_HAS_PAY = 2;
}

//��������
public struct ErrorType 
{
    //ȫ��
    public const int ERRORTYPE_ALL = 0;
    //��Ҵ���
    public const int ERRORTYPE_GOLD = 1;
    //��ʯ����
    public const int ERRORTYPE_GEM = 2;
    //�������
    public const int ERRORTYPE_DB = 3;
    //����ȯ����
    public const int ERRORTYPE_CHIP = 4;
}

// ���ͬ������ʧ��ԭ��
public enum AddItemFailReason
{
	// ���ڲ����������
	fail_reason_player_not_in_fish = 0,

	// ��ҵ��ߣ���logic�Ͽ�����
	fail_reason_player_offline = 1,

	// ����ȷ��ʱ��
	fail_reason_beyond_verity_time = 2,    

	// û�ҵ����
	fail_reason_not_find_player = 3,
};

// ��ҽ�ң���ʯ�ı仯ԭ��
public enum PropertyReasonType
{
    // ÿ�յ�¼ת�̳齱
    //type_reason_dial_lottery = 1,

	// ����ĳ�  ʹ������
	type_reason_use_torpedo = 2,

	// ���������
    //type_reason_deposit_safebox = 3,

	// ������ȡ��
    //type_reason_draw_safebox = 4,

	// ��������
    type_reason_send_gift = 5,

	// ��������
    //type_reason_accept_gift = 6,

	// ��ҷ�С���ȣ�ȫ��ͨ��
    //type_reason_player_notify = 7,
	
	// ��Ҷһ�����
	type_reason_exchange = 8,

	// ������Ʒ���
	type_reason_buy_commodity_gain = 9,

	// ��ȡ�ȼý�
	type_reason_receive_alms = 10,

	// ���ֽ���
	type_reason_single_round_balance = 11,

	// ������Ʒ����
	type_reason_buy_commodity_expend = 12,
	
	//������ȼ�
	type_reason_buy_fishlevel = 13,

	//���������
	type_reason_buy_fishitem = 14,

	//�������
	type_reason_player_uplevel = 15,

	// ��������
	type_reason_new_guild = 16,

	// �޸�ͷ��
    type_reason_update_icon = 17,

	// ��ֵ
	type_reason_recharge = 18,

	// �޸��ǳ�
    type_reason_modify_nickname = 19,

	// ��ֵ����
	type_reason_recharge_send = 20,

	// ��̨��ֵ
	type_reason_gm_recharge = 21,

	// ��̨��ֵ����
	type_reason_gm_recharge_send = 22,

	// �¿�ÿ����ȡ
	type_reason_month_card_daily_recv = 23,

	// ��ֵ���
	type_reason_recharge_gift = 24,

	// ÿ��ǩ��
	type_reason_daily_sign = 25,

	// ÿ�ձ���齱
    //type_reason_daily_box_lottery = 26,

	// лл����һ�
    //type_reason_thank_you_exchange = 27,

	// ������С����
    //type_reason_continuous_send_speaker = 28,

	// ��ȡ�ʼ�
	type_reason_receive_mail = 29,

	// �������
	type_reason_fishlord_drop = 30,

	// �����˺�
    //type_reason_create_account = 31,

	// ��ȡ�����
	type_reason_receive_activity_reward = 32,
	
	//��һ�
    type_reason_receive_activity_exchange = 33,
	
	//�ټ�����ǰ��ׯ
    //type_reason_leave_banker = 34,

	// ʹ�ü���
	//type_reason_use_skill = 35,
	
	//����������Ϸʹ����ʯ   ����ĳɣ� ������
	//type_reason_double_game = 36,
	
	//�������� //��ʯ����ͽ��֧��
    //type_reason_dragons_lv = 37,	

	//���ǽ���
	//type_reason_lucky_award = 38,

	//���˳齱	
	type_reason_lucky_lottery = 39,

	//�������
	type_reason_new_player = 40,

	//ÿ������
	type_reason_daily_task = 41,

	//ÿ������
	type_reason_weekly_task = 42,

	//��������
	type_reason_mainly_task = 43,

	//�ɾ�
	//type_reason_achievement = 42;
	//��������
	//type_reason_missile = 43;

	// ��ֵ�齱
	type_reason_recharge_lottery = 44,
	
	//������ֵ���
	type_reason_recharge_guide_gift = 45,
	
	//��Ծ������
	type_reason_active_box = 46,

	// ��С��Ϸ�һ�����
    //type_reason_play_game = 47,
	// VIP����
	type_reason_get_vipgold = 48,

	// ������Ʊ
    //type_reason_match_ticket = 49,

	// ��̨����
	type_reason_gm_op = 50,

	// ����������
    type_reason_buy_material_gift = 51,

	// ʹ�ü���
    //type_reason_use_missile = 52,

	type_reason_receive_cdkey = 53,

	// ��ȡGM�ʼ�
	type_reason_receive_gm_mail = 54,
	
	// �������տ�
	type_reason_7_days_carnival = 55,

	// ����
    //type_reason_panic_buy = 56,

	// ��ȡ������ż�Ľ���
    //type_reason_recv_donate_puppet_reward = 57,

	// ������ż
    //type_reason_buy_puppet = 58,

	// ������齱
	//type_reason_chipfish_lottery = 59,

	// ��ȡ���ڽ���  ˫11�
    //type_reason_recv_wpreward = 60,

	// ���ʢ��
    //type_reason_gold_feast = 61,

	// ����������
    //type_reason_match_quest = 62,

	// ʥ��Ԫ���
    //type_reason_christmas = 63,
    
	//ˮ�����ȴ�С
    //type_reason_fruit_bs = 64,

	// �ι���
    //type_reason_scratch_ticket = 65,

	// �ۼƳ�ֵ
	//type_reason_acc_recharge = 66,

	// �´����
    //type_reason_ny_gift = 67,

	// ð��
    //type_reason_ny_adventure = 68,

    // ���ֻ�
	type_reason_account_update =69,

	//���鵥�ֽ���
    //type_reason_dragon_cow =69,    //ţţ

    //type_reason_dragon_shcdcard = 70, //�ں�÷��

	//���鲶�����
    //type_reason_dragon_deity_fish = 71,    //������

    //type_reason_dragon_king_fish = 72,   //����  

    //type_reason_dragon_monster = 73,      //����

	//����һ�
    //type_reason_chip_ex_dragon = 74,      //��ȯ�һ�����
    //type_reason_dragon_ex_fishitem = 75,      //����һ���ͷ
    //type_reason_dragon_ex_dragonchip = 76,     //����һ�������Ƭ
    //type_reason_dragonchip_ex_dragon = 77,    //������Ƭ�һ�����

	// ʹ�õ���
	type_reason_use_item = 79,

    //type_reason_world_cup_bet = 80,

	// ����˱���
    //type_reason_panic_buy_box = 81,

	// ħʯ�һ�
	type_reason_dimensity_exchange = 82,

	// �������䷿��
    //type_reason_create_bw_room = 83,

	// �������
    //type_reason_lose_bw = 84,

	// Ӯ�ñ���
    //type_reason_win_bw = 85,

	// ��ȡΧ����������
    //type_reason_recv_wjlw_reward = 86,

	// װ���ڵ�
    //type_reason_recv_wjlw_equip = 87,

	// ת���㽱��
    //type_reason_turnfish = 88,
	// �м����һ�
	type_reason_medium_exchange = 89,

	type_reason_kill_grandfish = 90,

	type_reason_airdrop = 91,

	// ��Ƭ�һ�
	type_reason_chip_exchange = 92,

	type_reason_weekly_clear = 93,
	type_reason_daily_clear = 94,

	//��̨����
	type_reason_turret_uplevel = 95,

	//��������
	type_reason_newplayer_task = 96, 

	//��������
    type_reason_parally_task = 97,

    //qq����
	type_reason_qq_reward = 98,

	// �Ϻ�Ѱ��
	type_reason_southsea = 99,

    //��ͷ�ϳ�
	type_torpedo_compse = 100,
	
	//���߽���
    type_reason_online_reward = 101,

    //���賡ʹ�ú�ը��
	type_reason_shark_bomb_aircraft = 102,

	//���賡�齱
	type_reason_shark_lottery = 103,

	//���賡ն����
	type_reason_shark_excute = 104,

    //�������
    type_reason_tommorrow_gift = 105,

    //׷��з��
    type_reason_kill_crab = 106,

    //��ͷϺ
    type_reason_drill_kill = 107,

    //����ת����ʯ
    type_reason_system_clear = 108,

    //��̨��������
	type_reason_turret_uplevel_task = 109,

	//����ĳ�  ʹ����������
	type_reason_use_new_player_torpedo = 110,

    //�����Ϻ�Ѱ��
	type_reason_new_player_southsea = 111,

    //�ɳ�����
    type_reason_new_player_grow_fund = 112,

    //���Ź��
    type_reason_paly_ad = 113,

    //��������������
    type_reason_dragon_excute = 114,

    //�����߼�������
    type_reason_dragon_excute2 = 115,

    //�ۼ�ǩ��
    type_reason_accum_sign = 116,

    //ǩ��VIP����
    type_reason_sign_vip_reward = 117,

    //����ը����
	type_reason_torpedo_activity_reward = 118,

	//��ȡ�����
	type_reason_new_player_gift = 119,

    // ��ȡ΢�����	type_reason_receive_wechat_gift = 120,

    // ΢����������	type_reason_invite_task = 121,	// ���ﳡ����	type_reason_legendary_fish_hatch = 122,    // ���ﳡ�ٻ�BOSS	type_reason_legendary_fish_call_boss = 123,	//VIPר��	type_reason_vip_exclusive = 124,

    //�������ճ�ֵ
	type_reason_new_player_seven_day_recharge = 125,

	//���ֺ��
	type_reason_redpacket = 126,

    //ʥ�����ת��
    type_reason_mythical_gift_lotter = 127,

    //ʥ�޻���ת��
    type_reason_mythical_point_award = 128,

    //�������ɳ��ƻ�
	type_reason_growth_quest = 129,

    //��������
	type_reason_room_quest = 130,

    //����
    type_reason_catch_fish_activity = 131,

    type_max,
};

// ���ӵ�е���������
public enum PropertyType
{
    property_type_full,

    // ���
    property_type_gold,

    // ��ʯ
    property_type_diamond,

    // ��ȯ
    property_type_ticket,

    // �����
    property_type_dragon_ball = 14,	

    //ħʯ
    property_type_moshi = 18,	

    //��Ƭ
    property_type_chip = 20,  

    //���
    property_type_hongbao = 106,

}

public enum DataStatType
{
    // ��������
    stat_send_gift = 1,

    // С����
    stat_player_notify,

    // vip�ȼ��ֲ����
    stat_player_vip_level,

    // �ϴ�ͷ��
    stat_upload_head_icon,

    // �ǳ��޸�
    stat_nickname_modify,

    // ǩ���޸�
    stat_self_signature_modify,

    // �Ա��޸�
    stat_sex_modify,

    // ͷ�����
    stat_photo_frame,

    // ���߽���
    stat_online_reward,

    // �ȼý�
    stat_relief,

    // ���������
    stat_safe_box_deposit,

    // ������ȡ��
    stat_safe_box_draw,

    stat_max,
};

public enum GameId 
{
    lobby = 0,   // ����
    fishlord = 1, // ���䲶��

    crocodile,    // ������

    dice,         // ��������

    cows,         // ����ţţ
    
    baccarat,     // �ټ���
    
    dragon,       // ����

    calf_roping,   // ��ţ
    prize_claw,    // ץ����

    fishpark,     // ���㹫԰

    shcd,         // �ں�÷��
    shuihz = 12,       //ˮ䰴�
    bz=13,        //���۱���
    fruit=14,     //ˮ����
    jewel = 15,     //��������
    gameMax,
}

public enum RoomId 
{
    room1 = 1,//������

    room2 = 2,//�м���

    room3 = 3,//�߼���

    room4 = 4,//��Ƭ��
    
    room5 = 5,//����������

    room6 = 6, //���賡

    room7 = 7,//�߼�������

    room8 = 8, //���ﳡ

    room9 = 9, //ʥ�޳�

    room11 = 11,//������

    room15 = 15,//����

    room20 = 20,//��������
}

//////////////////////////////////////////////////////////////////////////

// ������������
public enum FishLordExpend
{
    fish_buyitem_start,			//������Ʒ Fish_ItemCFG
    fish_buyitem_end = fish_buyitem_start + 31,

    fish_useskill_start = 100,	//ʹ�ü��� Fish_BuffCFG
    fish_useskill_end = fish_useskill_start + 10,

    fish_turrent_uplevel_start = 150,         // ��̨������ʼ
    fish_turrent_uplevel_end = fish_turrent_uplevel_start + 55,

    fish_unlock_level_start = 300,
    fish_unlock_level_end = fish_unlock_level_start + 55,

    // ��������
    fish_missile = 500,
    fish_missile_end = 500 + 3,
};

public enum RechargeType
{
    // �������
    rechargeRMB,

    // ɾ���Զ���ͷ��
    delIconCustom,

    gold,   // ���
    gem,    // ��ʯ
    vipExp,  // VIP����
    dragonBall, // ����

    chip, //��Ƭ
    moshi, //ħʯ

    resetGiftGuideFlag,  // ��������Ϊ1�����״̬
    rechargeRMBFromWeichat, // ͨ�����ںų�ֵ

    playerXP,  //��Ҿ���
};

public struct PlayerType
{
    public const int TYPE_ACTIVE = 1;          // ��Ծ�û�
    public const int TYPE_RECHARGE = 2;        // �����û�
    public const int TYPE_NEW = 3;             // �����û�
}

// ֧������
public struct PayType
{
    // ʹ�ù��ںŷ�ʽ��ֵ
    public const int WeChatPublicNumer = 1;
}

//////////////////////////////////////////////////////////////////////////
public class GameStatData
{
    // ����ע������
    public int m_regeditCount;

    // �����豸��������
    public int m_deviceActivationCount;

    // ��Ծ����
    public int m_activeCount;

    // �豸��Ծ����
    public int m_deviceLoginCount;

    // ����������
    public int m_totalIncome;

    // ��������
    public int m_rechargePersonNum;

    // ���Ѵ���
    public int m_rechargeCount;

    // �����������û������ܼ�
    public int m_newAccIncome;
    // �����������û��У������û�����
    public int m_newAccRechargePersonNum;

    // �����ʼ���ʱ����ע������
    //public int m_2DayRegeditCount;

    // ������������
    public int m_2DayRemainCount;

    //public int m_3DayRegeditCount;

    // 3����������
    public int m_3DayRemainCount;

    //public int m_7DayRegeditCount;

    // 7����������
    public int m_7DayRemainCount;

    //public int m_30DayRegeditCount;

    // 30����������
    public int m_30DayRemainCount;
    
    // 1���ܳ�ֵ�� -1��ʾ��û������
    public int m_1DayTotalRecharge = -1;
    // 3���ܳ�ֵ�� -1��ʾ��û������
    public int m_3DayTotalRecharge = -1;
    // 7���ܳ�ֵ�� -1��ʾ��û������
    public int m_7DayTotalRecharge = -1;
    // 14���ܳ�ֵ�� -1��ʾ��û������
    public int m_14DayTotalRecharge = -1;
    // 30���ܳ�ֵ�� -1��ʾ��û������
    public int m_30DayTotalRecharge = -1;
    // 60���ܳ�ֵ�� -1��ʾ��û������
    public int m_60DayTotalRecharge = -1;
    // 90���ܳ�ֵ�� -1��ʾ��û������
    public int m_90DayTotalRecharge = -1;

    //////////////////////////////////////////////////////////////////////////
    // �����豸������������ʱ����
    public int m_2DayDevRemainCount = -1;

    // 3���豸��������
    public int m_3DayDevRemainCount = -1;

    // 7���豸��������
    public int m_7DayDevRemainCount = -1;

    // 30���豸��������
    public int m_30DayDevRemainCount = -1;

    //////////////////////////////////////////////////////////////
    // ������������(����)
    public int m_2DayRemainCountRecharge;

    // 3����������(����)
    public int m_3DayRemainCountRecharge;

    // 7����������(����)
    public int m_7DayRemainCountRecharge;

    //////////////////////////////////////////////////////////////////////////
    // ���ո�������
    public int m_2DayRechargePersonNum = 0;
    //3�ո�������
    public int m_3DayRechargePersonNum = 0;
    //7�ո�������
    public int m_7DayRechargePersonNum = 0;

    // ע�ᵱ�ս����泡����
    public int m_enterFishRoomCount = 0;
}

//////////////////////////////////////////////////////////////////////////

public class ResultRPlayerItem
{
    public int m_playerId;
    public int m_rechargeCount;
    public int m_rechargeMoney;
    public int m_loginCount;
    public Dictionary<int, int> m_games = new Dictionary<int, int>();

    public DateTime m_regTime;
    public DateTime m_lastLoginTime;
    public int m_remainGold;
    public int m_mostGold;
    public string m_bindPhone;

    public void addEnterCount(int gameId, int count)
    {
        m_games.Add(gameId, count);
    }

    public int getEnterCount(int gameId)
    {
        if (m_games.ContainsKey(gameId))
        {
            return m_games[gameId];
        }

        return 0;
    }
}

public struct ConstDef
{
    public const string AES_FOR_CDKEY = "!$@*6102aZty^%vn";

    public const string AES_FOR_AIR_DROP = "&@*(#kas9081fajk";
}


