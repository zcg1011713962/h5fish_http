using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Linq;

public struct TableName
{
    //金币排行榜
    public const string STAT_PUMP_DAILY_GOLD_RANK = "pumpDailyGoldRank";
    //青铜、白银、黄金、钻石
    public const string STAT_PUMP_DAILY_ITEM_RANK = "pumpDailyItemRank";

    //捕鱼王统计
    public const string STAT_PUMP_GROWTH_QUEST = "pumpGrowthQuest";

    //骰子鱼
    public const string STAT_PUMP_DICE_GAME = "pumpDiceGame";
    public const string STAT_PUMP_SHELL_GAME = "pumpShellGame";

    //圣兽场玩法统计
    //朱雀收入统计
    public const string STAT_PUMP_MYTHICAL_ROOM_EVENT_TRIGGER = "pumpMythicalRoomEventTrigger";

    //圣兽场 当前排行榜
    //public const string FISHLORD_MYTHICAL_POINTS_RANK = "fishlordMythicalPointsRank";
    //当前排行榜原始表
    public const string FISHLORD_MYTHICAL_PLAYER = "fishlordMythicalPlayer";

    //历史排行榜
    public const string STAT_PUMP_MYTHICAL_ROOM_RANK = "pumpMythicalRoomRank";

    //五一活动
    public const string STAT_PUMP_WUYI_TASK = "pumpWuYiTask";
    //五一宝库
    public const string STAT_PUMP_WUYI_LOTTERY = "pumpWuYiLottery";

    //红包方案
    public const string STAT_PUMP_RED_ENVELOP = "pumpNewPlayerRedPacket";

    //七日连充
    public const string STAT_PUMP_NP_7DAY_RECHARGE = "pumpNp7DayRecharge";
    public const string STAT_PUMP_NP_7DAY_RECHARGE_DETAIL = "pumpNp7DayRechargeDetail";

    //机器人积分管理
    public const string STAT_FISHLORD_ROBOT_RANK_CFG = "fishlordRobotRankCFG";

    //场次活动 
    //圣兽活动、巨鲨活动
    public const string STAT_PUMP_ROOM_QUEST_ACTIVITY = "pumpRoomQuestActivity";
    public const string STAT_PUMP_ROOM_QUEST_ACTIVITY_DETAIL = "pumpRoomQuestActivityDetail";

    //捕鱼活动
    public const string STAT_PUMP_HUNT_FISH_ACTIVITY = "pumpHuntFishActivity";
    //累充奖励
    public const string STAT_PUMP_RECHARGE_ACTIVITY = "pumpRechargeActivity";

    //捞鱼活动
    public const string STAT_PUMP_CATCH_FISH_ACTIVITY = "pumpCatchFishActivity";

    //巨鲲场玩法
    public const string STAT_FISHLORD_LEGENDARY_FISH_ROOM = "pumpLegendaryFishRoom";

    //巨鲲降世
    //当前
    public const string STAT_FISHLORD_LEGENDARY_FISH_PLAYER = "fishlordLegendaryFishPlayer";
    //历史
    public const string STAT_PUMP_LEGENDARY_FISH_ROOM_RANK = "pumpLegendaryFishRoomRank";

    //时段在线人数
    public const string STAT_PUMP_HOUR_MAX_ONLINE_PLAYER = "hour_max_online_player";

    //富豪计划
    public const string STAT_PUMP_VIP_EXCLUSIVE = "pumpVipExclusive";

    //公众号统计
    public const string STAT_PUMP_WECHAT_BENIFIT = "pumpWechatBenifit";

    //内部库
    public const string STAT_PUMP_INNER_PLAYER = "pumpInnerPlayer";

    //第一次登录到起航礼包
    public const string STAT_PUMP_NEW_GUILD_GIFT_TIME = "pumpNewGuildGiftTime";

    //激励视频统计
    public const string STAT_PUMP_PLAY_AD = "pumpPlayAd";

    //场次兑换
    public const string STAT_PUMP_ROOM_EXCHANGE = "pumpRoomExchange";

    //进入场次人数统计
    public const string STAT_PUMP_ROOM_PLAYER = "pumpRoomPlayer";

    //龙宫场玩家鱼雷兑换列表
    public const string STAT_PUMP_DRAGON_PALACE_PLAYER = "pumpDragonPalacePlayer";

    //玩家平均携带金币
    public const string STAT_PUMP_TURRET_ITEMS = "pumpTurretItems";

    //成长基金统计
    public const string STAT_PUMP_GROW_FUND = "pumpGrowFund";

    //玩家携带金币
    public const string STAT_PUMP_TURRET_PROPERTY = "pumpTurretProperty";

    //玩家携带金币脚本
    public const string STAT_TURRET_PROPERTY = "statTurretProperty";

    //游戏控制表
    public const string OP_CHANNEL_VER_CONTROL = "channelVerControl";

    //=========================================================================
    //追击蟹将活动统计
    //抽奖统计
    public const string STAT_PUMP_KILL_CRAB_LOTTERY = "pumpKillCrabLottery";
    //抽奖统计详情
    public const string STAT_PUMP_KILL_CRAB_LOTTERY_PLAYER = "pumpKillCrabLotteryPlayer";
    //追击蟹将场次消耗
    public const string STAT_PUMP_KILL_CRAB_ROOM_CONSUME = "pumpKillCrabRoomConsume";
    //追击蟹将任务统计
    public const string STAT_PUMP_KILL_CRAB_PLAYER_TASK = "pumpKillCrabPlayerTask";

    //=========================================================================
    //国庆活动
    //签到
    public const string STAT_PUMP_ACTIVITY_SIGN = "pumpActivitySign";

    //欢乐集字
    public const string STAT_PUMP_ACTIVITY_EXCHANGE = "pumpActivityExchange";

    //冒险之路
    public const string STAT_PUMP_ACTIVITY_TASK = "pumpActivityTask";

    //礼包购买
    public const string STAT_PUMP_ACTIVITY_GIFT = "pumpActivityGift";

    //礼包记录
    public const string STAT_PUMP_GIFT_RECORD = "pumpGiftRecord";

    //抽奖统计
    public const string STAT_PUMP_ACTIVITY_LOTTERY = "pumpActivityLottery";

    //新手炮倍完成率
    public const string STAT_PUMP_TURRET_LEVEL = "pumpNewPlayerTurretLevel";
    //炮倍任务
    public const string STAT_PUMP_TOTAL_PLAYER_TURRET_LEVEL = "pumpTurretLevel";

    //玩家手机验证表
    public const string PLAYER_PHONE_CODE = "playerPhoneCode";

    //背包道具购买
    public const string STAT_PUMP_PLAYER_ITEM = "statPumpPlayerItem";

    //在线奖励
    public const string STAT_PUMP_ONLINE_REWARD = "pumpOnlineReward";

    //开服活动统计
    public const string STAT_PUMP_SD_ACT = "pumpSdAct";

    //开服抽奖统计
    public const string STAT_PUMP_SD_LOTTERY_ACT = "pumpSdActLottery";

    //流失点统计
    public const string STAT_PUMP_NEW_GUILD_LOSE_POINT = "pumpNewGuildLosePoint";

    // VIP特权领取统计
    public const string STAT_VIP_RECORD = "statVipRecord";
    public const string STAT_TURRET_ITEMS = "statTurretItems";

    //南海寻宝
    //单计
    public const string STAT_PUMP_TREASURE_HUNT_PLAYER = "pumpTreasureHuntPlayer";
    //总计
    public const string STAT_PUMP_TREASURE_HUNT_ROOM = "pumpTreasureHuntRoom";
    //详情
    public const string STAT_PUMP_TREASURE_HUNT_PLAYER_DETAIL = "pumpTreasureHuntPlayerDetail";

    //高级场控制管理
    public const string FISHLORD_ADVANCED_ROOM_CTRL = "fishlordAdvancedRoomControl";
    //高级场奖池统计
    public const string FISHLORD_ADVANCED_ROOM_ACT = "pumpAdvancedRoom";
    //高级场奖池统计详情
    public const string FISHLORD_ADVANCED_ROOM_ACT_DETAIL = "pumpAdvancedRoomDetail";
    //高级场排行榜当前
    public const string FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR = "fishlordAdvancedPlayer";
    //高级场排行榜历史
    public const string FISHLORD_ADVANCED_ROOM_ACT_RANK_HIS = "pumpAdvancedRoomRank";
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //中级场玩家积分
    public const string STAT_FISHLORD_MIDDLE_PLAYER = "fishlordMiddlePlayer";

    //中级场玩法收入统计
    public const string STAT_FISHLORD_MIDDLE_ROOM_INCOME = "pumpMiddleRoomIncome";
    //中级场兑换统计
    public const string STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE = "pumpMiddleRoomExchange";

    //中级场兑换统计人数
    public const string STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE_DETAIL = "pumpMiddleRoomExchangeDetail";

    //中级场历史排行榜
    public const string STAT_FISHLORD_MIDDLE_ROOM_RANK = "pumpMiddleRoomRank";
    //中级场打点数据 //巨鲨场
    public const string STAT_FISHLOR_ROOM_FU_DAI = "pumpRoomGift";

    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    //巨鲨场玩法收入统计
    public const string STAT_FISHLORD_SHARK_ROOM_OUTLAY = "pumpSharkRoomOutlay";
    //详情
    public const string STAT_FISHLORD_SHARK_ROOM_DETAIL = "pumpSharkRoomDetail";

    //轰炸机统计
    public const string STAT_FISHLORD_SHARK_ROOM_BOMB = "pumpSharkRoomBomb";
    //轰炸机统计玩家
    public const string STAT_FISHLORD_SHARK_ROOM_BOMB_DETAIL = "pumpSharkRoomBombDetail";

    //巨鲨场抽奖统计
    public const string STAT_FISHLORD_SHARK_ROOM_LOTTERY = "pumpSharkRoomLottery";
    //排行榜
    //当前
    public const string STAT_FISHLORD_ARMED_SHARK_PLAYER = "fishlordArmedSharkPlayer";
    //历史
    public const string STAT_FISHLORD_SHARK_RANK = "pumpArmedSharkRoomRank";

    //能量统计
    public const string STAT_FISHLORD_SHARK_ROOM_ENERGY_DROP = "pumpSharkRoomEnergyDrop";
    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    //主线任务
    public const string STAT_PUMP_MAINLY_TASK = "pumpMainlyTask";

    ////////破产统计
    //破产玩家
    public const string STAT_PUMP_PLAYER_BROKEN_INFO = "pump_player_borken_info";
    //破产后充值玩家
    public const string STAT_PUMP_PLAYER_PAY_AFTER_BROKEN = "pump_player_pay_after_broken";

    //玩家炮数成长分布
    public const string STAT_PUMP_PLAYER_ACTIVITY_TURRET = "pump_player_activity_turret";

    //幸运抽奖
    public const string STAT_PUMP_GOLD_FISH_LOTTERY = "pumpGoldFishLottery";
    //幸运抽奖玩家详情
    public const string STAT_PUMP_GOLD_FISH_LOTTERY_PLAYER = "pumpPlayerGoldFishLottery";

    //每日任务统计
    public const string STAT_PUMP_DAILY_TASK = "pumpDailyTask";
    //每周任务统计
    public const string STAT_PUMP_WEEKLY_TASK = "pumpWeeklyTask";

    //每日奖励统计
    public const string STAT_PUMP_DAILY_REWARD = "pumpDailyTaskActivity";
    //每周奖励统计
    public const string STAT_PUMP_WEEK_REWARD = "pumpWeeklyTaskActivity";

    //系统空投
    public const string STAT_AIR_DROP_SYS = "airdropSys";
    //系统空投过期
    public const string STAT_PUMP_AIR_DROP_HISTORY = "pumpAirdropHistory";

    //玩家上线次数统计
    public const string PUMP_PLAYER_LOGIN = "pumpPlayerLogin";

    //场次情况
    public const string PUMP_PLAYER_PLAY_TIME = "pumpPlayerPlayTime";
    //场次情况
    public const string STAT_PLAYER_PLAY_TIME = "statPlayerPlayTime";

    //屠龙榜
    //周榜
    public const string STAT_PUMP_KD_WEEK_RANK = "pumpKdWeekRank";
    //当日榜
    public const string STAT_PUMP_KD_ACTIVITY = "kdActivity";
    //昨日榜
    public const string STAT_PUMP_KD_HISTORY_RANK = "kdHistoryRank";

    //弹头礼包统计
    public const string STAT_BULLET_HEAD_GIFT = "pumpBulletHeadGift";

    //弹头礼包详情
    public const string STAT_BULLET_HEAD_GIFT_PLAYER = "pumpBulletHeadGiftPlayer";

    //围剿龙王自定义付费玩家排名
    public const string STAT_WJLW_Act_DEF_RECHARGE_REWARD = "wjlwActDefRechargeReward";
    //金币玩法统计
    public const string STAT_WJLW_PUMP_GOLD_EARN = "pumpWjlwGoldEarn";
    //金币玩法修改当前期望盈利率
    public const string WJLW_ACT_DATA = "wjlwActData";
    //金币玩法统计每局详情
    public const string STAT_WJLW_PUMP_GOLD_TURN_INFO = "pumpWjlwGoldTurnInfo";
    //金币玩法统计奖励领取
    public const string STAT_WJLW_PUMP_GOLD_RECV_INFO = "pumpWjlwGoldRecvInfo";

    //金币玩法每局下注玩家列表
    public const string STAT_WJLW_PUMP_GOLD_BET_PLAYER = "pumpWjlwGoldBetPlayer";
    //付费玩法统计
    public const string STAT_WJLW_PUMP_RECHARGE_EARN = "pumpWjlwRechargeEarn";
    //获奖详情
    public const string STAT_WJLW_PUMP_RECHARGE_WIN_INFO = "pumpWjlwRechargeWinInfo";
    //是否领取
    public const string STAT_WJLW_PUMP_RECHARGE_RECV_INFO = "pumpWjlwRechargeRecvInfo";
    //下注玩家列表
    public const string STAT_WJLW_PUMP_RECHARGE_BET_PLAYER = "pumpWjlwRechargeBetPlayer";

    //库存数据统计
    public const string STAT_PLAYER_MONEY_REP = "statPlayerMoneyRep";

    //=============转盘鱼活动
    //排行榜当前
    public const string STAT_TURN_TABLE_FISH_ACTIVITY = "turntableFishActivity";
    //排行榜历史
    public const string STAT_PUMP_TFISH_RANK = "pumpTFishRank";
    //捕鱼统计
    public const string STAT_PUMP_TFISH = "pumpTFishStat";
    //详情
    public const string STAT_PUMP_TFISH_PLAYER = "pumpTFishPlayer";

    //场次统计
    public const string STAT_PUMP_TFISH_ROOM_STAT = "pumpTFishRoomStat";
    //=========================================================================

    //国庆中秋快乐
    //排行榜统计
    //历史
    public const string STAT_PUMP_NATIONAL_DAY_2018_RANk = "pumpNationalDay2018Rank";
    //当前
    public const string STAT_FISHLORD_NATIONAL_DAY_ACTIVITY_2018 = "fishlordNationalDayActivity2018";
    //场次统计
    public const string STAT_FISHLORD_NATIONAL_DAY_ROOM_STAT = "pumpNationalDay2018Drop";
    //抽奖统计
    public const string STAT_PUMP_NATIONAL_DAY_2018_LOTTERY = "pumpNationalDay2018Lottery";
    //金币消耗
    public const string STAT_PUMP_NATIONAL_DAY_2018_DROP = "pumpNationalDay2018Drop";
    //详情
    public const string STAT_PUMP_NATIONAL_DAY_2018_LOTTERY_PLAYER = "pumpNationalDay2018LotteryPlayer";

    //任务统计
    public const string STAT_PUMP_NATIONAL_DAY_2018_TASK = "pumpNationalDay2018Task";
    //任务统计详情
    public const string STAT_PUMP_NATIONAL_DAY_2018_PLAYER_TASK = "statNationalDay2018PlayerTask";

	// 龙宫场参与玩家分布
    public const string STAT_DRAGON_PALACE_JOIN_DISTRIBUTE = "statDragonPalaceDistribute";

    // 龙宫场统计
    public const string PUMP_DRAGON_PALACE_JOIN = "pumpDragonPalace";
	
	//客服补单/大户随访/换包福利-系统
    public const string PUMP_REPAIR_ORDER = "pumpRepairOrder";

    //大户回流引导添加记录
    public const string GUIDE_LOST_PLAYER_INFO = "guideLostPlayerInfo";

   //世界杯大竞猜赛事竞猜
    public const string WORLD_CUP_MATCH_INFO = "worldCupMatchInfo";
    //世界杯大竞猜队伍押注
    public const string WORLD_CUP_MATCH_BET_INFO = "worldCupMatchBetInfo";
    // 世界杯玩家参与下注统计
    public const string STAT_WORLD_CUP_MATCH_PLAYER_JOIN = "statWorldCupPlayerJoin";
    //世界杯大竞猜奖励
    public const string WORLD_CUP_MATCH_REWARD = "worldCupReward";

    //新手引导埋点
    public const string PUMP_NEW_GUIDE = "pumpNewGuild";
    public const string PUMP_PLAYER_REG = "pumpPlayerReg";

    //欢乐/衰神炸炸炸
    public const string PUMP_BULLET_HEAD_RANK = "pumpTorpedoRank"; //历史排行
    public const string BULLET_HEAD_ACTIVITY = "torpedo_activity"; //当前
    public const string PUMP_BULLET_HEAD_GUARANTEE = "pumpTorpedoGuarantee";//入围历史

    //龙鳞排行
    public const string FISHLORD_DRAGON_PALACE_RANK = "fishlordDragonPalaceRank";//龙鳞历史排行
    public const string FISHLORD_DRAGON_PALACE_PLAYER = "fishlordDragonPalacePlayer";//龙鳞当前排行

    //礼包码CDKEY生成器
    public const string CD_KEY_MULT = "cdkeyMult";

    //刮刮乐运营状况
    public const string PUMP_SCRATCH_EARN = "pumpScratchEarn";

    //刮刮乐兑换
    public const string PUMP_SCRATCH_EXCHANGE_RES = "pumpScratchExchangeRes";

    //添加同步道具失败
    public const string PUMP_WORD2_LOGIC_ITEM_ERROR = "pumpWorld2LogicItemError";

    //玩家道具详情
    public const string PUMP_PLAYER_ITEM = "pumpPlayerItem";

    //经典捕鱼点杀点送
    public const string FISHLORD_KS_SCORE = "fishlordKsScore";

    //广告——极光效果通
    public const string ADVERTISEMENT_XGT = "advertisement_xgt";

    //水果机
    //水果机房间
    public const string FRUIT_ROOM = "fruit_room";
    //结果控制
    public const string FRUIT_RESULT_CONTROL = "fruitResultControl";
    //水果机黑白名单
    public const string FRUIT_BW_LIST = "fruitWhiteBlackList";

    //水果机每日盈利率
    public const string FRUIT_EVERY_DAY = "FruitEveryday";

    //修改机器人最高积分
    public const string FISHLORD_BAOJIN_SYS = "fishlord_baojin_sys";
    ////////////////////////////////////////////////////////////////////////////////
    //弹头统计
    public const string FISHLORD_BULLET_HEAD = "fishlordBulletHead";
    //捕鱼弹头统计
    public const string PUMP_BULLET_HEAD = "pumpBulletHead";
    //捕鱼每日弹头统计
    public const string PUMP_BULLET_HEAD_DAILY = "pumpBulletHeadDaily";
    //捕鱼打鱼弹头统计
    public const string PUMP_BULLET_HEAD_FISH_USE = "pumpBulletHeadFishUse";
    //捕鱼背包弹头统计
    public const string PUMP_BULLET_HEAD_BAG_USE = "pumpBulletHeadBagUse";
    //////////////////////////////////////////////////////////////////////////////
    //捕鱼鱼雷统计
    public const string PUMP_TORPEDO = "pumpTorpedo";
    //捕鱼每日鱼雷统计
    public const string PUMP_TORPEDO_DAILY = "pumpTorpedoDaily";
    //捕鱼打鱼鱼雷统计
    public const string PUMP_TORPEDO_FISH_USE = "pumpTorpedoFishUse";
    //捕鱼背包鱼雷统计
    public const string PUMP_TORPEDO_BAG_USE = "pumpTorpedoBagUse";
    /////////////////////////////////////////////////////////////////////////////////

    //极光推送
    public const string CONFIG_POLAR_LIGHTS_PUSH = "JpushMsgList";

    //限时活动
    public const string PB_ACTIVITY_CFG = "pbActivityCfg";

    //节日活动
    public const string PUMP_ACTIVITY_TEMPLATE = "pumpActivityTemplate";

    // 玩家信息表
    public const string PLAYER_INFO = "player_info";

    //实物历史发放金额
    public const string STAT_PLAYER_EXCHANGE = "statPlayerExchange";

    //兑换历史记录
    public const string PUMP_PLAYER_EXCHANGE = "pumpPlayerExchange";

    // 邮件表
    public const string PLAYER_MAIL = "playerMail";

    //已删除的邮件表
    public const string PUMP_MAIL_DEL = "pumpMailDel";

    //已领取邮件表
    public const string PUMP_MAIL_RECV = "pumpMailRecv";

    //竞技场邮件发送失败
    public const string PUMP_FISH_BAO_JIN_SEND_REWARD_FAIL = "pumpFishBaojinSendRewardFail";

    // 任务表
    public const string PLAYER_QUEST = "player_quest";

    // 邮件检测表
    public const string CHECK_MAIL = "checkMail";

    // 玩家账号表
    public const string PLAYER_ACCOUNT = "AccountTable";

    // GM账号表
    public const string GM_ACCOUNT = "GmAccount";

    // 计数表
    public const string COUNT_TABLE = "OpLogCurID_DWC";

    // GM操作日志表
    public const string OPLOG = "OpLogDWC";

    //永久日志表
    public const string OP_FORVER_LOG = "OpForverLogDWC";

    // 权限表
    public const string RIGHT = "rightDWC";

    // 玩家登陆历史
    public const string PLAYER_LOGIN = "LoginLog";

    // 封IP表
    public const string BLOCK_IP = "blockIP";

    // 后台充值
    public const string GM_RECHARGE = "gmRecharge";

    // 极光应用
    public const string JPUSH_APP = "jpushAppInfoList";

    // 玩家金币，钻石变化总表
    public const string PUMP_PLAYER_MONEY = "pumpPlayerMoney";
    // 牛牛生局的牌型表
    public const string PUMP_COWS_CARD = "logCowsInfo";

    // 玩家金币，钻石变化详细表
    public const string PUMP_PLAYER_MONEY_DETAIL = "logPlayerInfo";

    // 每日任务表
    public const string PUMP_DAILY_TASK = "pumpDailyTask";

    // 成就
    public const string PUMP_TASK = "pumpTask";

    // 邮件
    public const string PUMP_MAIL = "pumpMail";

    // 活跃次数
    public const string PUMP_ACTIVE_COUNT = "pumpActiveCount";

    // 活跃人数
    public const string PUMP_ACTIVE_PERSON = "pumpActivePerson";

    // 通用数据统计
    public const string PUMP_GENERAL_STAT = "pumpGeneralStat";

    // 赠送礼物
    public const string PUMP_SEND_GIFT = "pumpSendGift";

    // 相框统计
    public const string PUMP_PHOTO_FRAME = "pumpPhotoFrame";

    // 总的消耗统计
    public const string PUMP_TOTAL_CONSUME = "pumpTotalConsume";

    // 金币增长排行
    public const string PUMP_COIN_GROWTH = "pumpCoinGrowth";

    // 金币增长历史排行
    public const string PUMP_COIN_GROWTH_HISTORY = "pumpCoinGrowthHistory";

    // 经典捕鱼消耗表
    public const string FISH_CONSUME = "pumpFishConsume";
    // 锁定、急速、散射的消耗
    public const string FISH_CONSUME_ITEM = "pumpFishItemConsume";

    // 捕鱼每天的收益情况
    public const string PUMP_FISHLORD_EVERY_DAY = "fishlordEveryDay";

    // 鳄鱼每天的收益情况
    public const string PUMP_CROCODILE_EVERY_DAY = "CrocodileEveryday";

    // 骰宝每天的收益情况
    public const string PUMP_DICE_EVERY_DAY = "DiceEveryday";

    // 百家乐每天的收益情况
    public const string PUMP_BACCARAT_EVERY_DAY = "BaccaratEveryday";

    // 五龙每天的收益情况
    public const string PUMP_DRAGON_EVERY_DAY = "DragonEveryday";

    // 鳄鱼公园每天的收益情况
    public const string PUMP_FISHPARK_EVERY_DAY = "fishParkEveryDay";

    // 黑红梅方每天的收益情况
    public const string PUMP_SHCD_EVERY_DAY = "ShcdCardsEveryday";

    // 套牛每天的收益情况
    public const string PUMP_CALFROPING_EVERY_DAY = "ropingEveryDay";

    // 百家乐玩家上庄情况查询
    public const string PUMP_PLAYER_BANKER = "pumpBaccaratPlayerBanker";

    // 牛牛每天的收益情况
    public const string PUMP_COWS_EVERY_DAY = "CowsEveryday";

    // 牛牛玩家上庄情况查询
    public const string PUMP_PLAYER_BANKER_COWS = "pumpCowsPlayerBanker";

    // 鳄鱼下注及获奖次数
    public const string PUMP_CROCODILE_BET = "CrocodileBetInfo";

    //奔驰宝马下注及获奖次数
    public const string PUMP_BZ_BET = "BzBetInfo";

    // 骰宝数据下注及获奖情况
    public const string PUMP_DICE = "dice_table";

    //水果机下注及获奖次数
    public const string PUMP_FRUIT_BET = "FruitBetInfo";

    // 鱼的统计
    public const string PUMP_ALL_FISH = "AllFishLog";
    // 鳄鱼公园鱼的统计
    public const string PUMP_ALL_FISH_PARK = "AllFishParkLog";

    // 重置时旧的盈利率
    public const string PUMP_OLD_EARNINGS_RATE = "pumpOldEarningsRate";

    // 经典捕鱼阶段表
    public const string PUMP_FISH_TABLE_LOG = "FishTableLog";
    // 鳄鱼公园阶段表
    public const string PUMP_FISH_PARK_TABLE_LOG = "FishParkTableLog";

    // 最高在线玩家
    public const string PUMP_MAXONLINE_PLAYER = "max_online_player";

    // 每日0点所有玩家的金币总和统计表
    public const string PUMP_PLAYER_TOTAL_MONEY = "pumpPlayerTotalMoney";

    // 兑换统计
    public const string PUMP_EXCHANGE = "pump_exchange";

    // 付费点统计
    public const string PUMP_RECHARGE = "pump_recharge";

    //七日活动
    public const string PUMP_7DAYACTIVITY = "pump7DaysActivity";

    //材料礼包每日购买
    public const string PUMP_MATERIAL_GIFT = "pumpMaterialGift";

    //转盘抽奖
    public const string PUMP_DIAL_LOTTERY = "pumpDialLottery";

    //拉霸抽奖档位统计
    public const string STAT_LABA_LOTTERY_PROB = "statLabaLotteryProb";

    //拉霸抽奖玩家抽奖记录
    public const string PUMP_LABA_LOTTERY = "pumpLabaLottery";

    //拉霸玩家抽奖次数统计
    public const string STAT_LABA_LOTTERY_PLAYER = "statLabaLotteryPlayer";
    // 拉霸整体数据
    public const string STAT_LABA_LOTTERY_TOTAL = "statLabaTotal";

    //拉霸活动统计
    public const string STAT_LABA_TOTAL = "statLabaTotal";

    //万炮盛典
    public const string PUMP_WP_PLAYER = "pumpWpPlayer";
    public const string PUMP_WP_COUNT = "pumpWpCount";
    public const string PUMP_WP_ITEM_CONSUME = "pumpWpItemConsume";

    //捕鱼盛宴活动
    public const string PUMP_GOLD_FEAST = "pumpGoldFeast";

    // 捐赠记录
    public const string PUMP_DONATE_PUPPET_REC = "pumpPlayerPuppetDonate"; //爆金比赛场统计
    public const string STAT_FISHLORD_BAOJIN_PLAYER = "statFishBaojinPlayer";
    public const string PUMP_FISH_GOLD_FLOW = "pumpFishGoldFlow";

    //爆金排行榜（历史）
    public const string STAT_FISHLORD_BAOJIN_WEEK_RANK = "fishlord_baojin_week_rank";
    public const string STAT_FISHLORD_BAOJIN_DAILY_RANK = "fishlord_baojin_daily_rank";
    public const string PUMP_FISH_BAOJIN_PLAYER = "pumpFishBaojinPlayer";
    //爆金排行榜（当前周/日）
    public const string STAT_FISHLORD_BAOJIN_RANK = "fishlord_baojin_player";

    //东海龙宫消耗统计
    public const string STAT_FISHLORD_DRAGON_PALACE_INCOME_OUTLAY = "pumpDragonPalace";
    //东海龙宫玩家分布
    public const string STAT_FISHLORD_DRAGON_PALACE_PLAYER_DISTRIBUTE = "statDragonPalaceDistribute";

    //竞技场消耗统计
    public const string STAT_FISHLORD_INCOME_OUTLAY = "pumpFishBaojinIncomeOutlay";

    //竞技场产出统计
    public const string STAT_FISHLORD_OUT_LAY = "pumpFishBaojinOutlay";

    //竞技场任务统计
    public const string STAT_FISHLORD_TASK = "pumpFishBaojinQuest";

	//发放玩偶数量
    public const string STAT_PUPPET_ACT = "pumpPuppetAct";

    //玩家/服务器档位捐赠
    public const string STAT_PUPPET_REWARD_RECV = "pumpPuppetRewardRecv";

    //服务器总捐赠玩偶次量
    public const string STAT_PUPPET_SVR_DONATE = "pumpPuppetSvrDonate";

    //活动期间
    public const string STAT_ACTIVITY_PUPPET = "activityPuppet";

    //活动结束（捐赠）
    public const string STAT_ACTIVITY_PUPPET_RANK = "activityPuppetRank";

    //活动结束（获得）
    public const string STAT_ACTIVITY_PUPPET_GAIN_RANK = "activityGainPuppetRank";

    // 爆金场玩家的统计
    public const string PUMP_BAOJIN_PLAYER = "pumpFishBaojinPlayer";
    public const string STAT_BAOJIN_PLAYER = "statFishBaojinPlayer";

    // 竞技场参与分布
    public const string STAT_BAOJIN_JOIN_DISTRIBUTE = "statFishBaojinJoinDistribute";

    // 爆金场每日排行
    public const string BAOJIN_DAILY_RANK = "fishlord_baojin_daily_rank";
    // 爆金场每周排行
    public const string BAOJIN_WEEK_RANK = "fishlord_baojin_week_rank";

    //爆金场
    public const string FISHLORD_BAOJIN_PLAYER = "fishlord_baojin_player";

    // 星星抽奖
    public const string PUMP_STAR_LOTTERY = "pumpStarLottery";
    public const string PUMP_STAR_LOTTERY2 = "pumpStarLottery2";

    //话费鱼统计
    public const string PUMP_CHIP_FISH = "pumpChipFish";

    // 对上面的数据的统计
    public const string STAT_STAR_LOTTERY2 = "statStarLottery2";

    public const string STAT_STAR_LOTTERY_DETAIL = "statStarLotteryDetail";

    // 充值用户统计
    public const string PUMP_RECHARGE_PLAYER = "pumpRechargePlayer";
    public const string PUMP_RECHARGE_PLAYER_NEW = "pump_recharge_player";

    // 玩家一天内各个游戏内，游戏时间累计
    public const string PUMP_GAME_TIME_FOR_PLAYER = "pumpGameTimeForPlayer";

    // 新增玩家发炮次数,捕鱼等级
    public const string PUMP_NEW_PLAYER_FIRECOUNT_FISHLEVEL = "pumpNewPlayerFireCountFishLevel";

    // 礼包
    public const string GIFT = "gift";

    // 礼包码表
    public const string GIFT_CODE = "cdkey";
    public const string CD_PICI = "cdkeyPICI";

    // 兑换表
    public const string EXCHANGE = "exchange";



    // 运营公告表
    public const string OPERATION_NOTIFY = "optionNotify";

    // 通告消息
    public const string OPERATION_SPEAKER = "operationSpeaker";

    // 捕鱼房间
    public const string FISHLORD_ROOM = "fishlord_room";

    //经典捕鱼每日
    public const string FISHLORD_EVERY_DAY = "fishlordEveryDay";

    public const string FISHLORD_LOBBY = "fishlord_lobby";

    // 鳄鱼公园房间
    public const string FISHPARK_ROOM = "fishpark_room";

    // 捕鱼桌子
    public const string FISHLORD_ROOM_DESK = "fishlord_table";
    // 鳄鱼公园桌子
    public const string FISHPARK_ROOM_DESK = "fishpark_table";

    // 鳄鱼房间
    public const string CROCODILE_ROOM = "crocodile_room";
    
    //鳄鱼大亨黑白名单设置
    public const string CROCODILE_WB_LIST = "crocodileWhiteBlackList";

    //奔驰宝马结果控制
    public const string DB_BZ_ROOM = "bz_room";

    //宝石迷阵每日游戏详情
    public const string JEWEL_EVERY_DAY_STAT = "JewelEveryday";

    //宝石迷阵全局参数
    public const string JEWEL_PARAM_CONTROL = "jewel_cfg";

    //宝石迷阵个人盈亏上限调整
    public const string JEWEL_ROOM = "jewel_room";

    //奔驰宝马每日盈利率
    public const string BZ_DAILY = "BzEveryday";

    //奔驰宝马黑白名单设置
    public const string Bz_WB_LIST = "bzWhiteBlackList";

    // 骰宝房间
    public const string DICE_ROOM = "dice_room";

    // 百家乐房间
    public const string BACCARAT_ROOM = "baccarat_room";

    // 牛牛房间
    public const string COWS_ROOM = "cows_room";

    // 五龙房间
    public const string DRAGON_ROOM = "dragons_room";

    // 黑红梅方房间
    public const string SHCDCARDS_ROOM = "shcdcards_room";

    //黑红梅方牌局查询
    public const string SHCD_CARD_BOARD = "logShcdCardBoard";

    //黑红梅方黑白名单
    public const string SHCD_CARD_SPECIL_LIST = "shcdcards_specil_list";

    //黑红梅方杀分放分LOG记录列表
    public const string SHCD_CARDS_CTRL_LIST = "logShcdWBKillSendScore";

    //水浒传单个玩家盈利率
    public const string SHUIHZ_PLAYER = "shuihz_player";

    public const string SHUIHZ_PLAYER_EVERY_DAY = "shuihzPlayerEveryDay";
    //水浒传总盈利率
    public const string SHUIHZ_ROOM = "shuihz_room";

    //水浒传每日盈利率
    public const string SHUIHZ_DAILY = "shuihzEveryDay";

    //水浒传每日游戏情况查看
    public const string SHUIHZ_DAILY_STATE = "logShuihzBonus";

    //水浒传每日达上下限人数统计
    public const string SHUIHZ_REACH_LIMIT = "shuihzReachLimit";

    // 套牛房间
    public const string CALF_ROPING_ROOM = "calfRoping_lobby";
    // 套牛 牛的分类 统计
    public const string CALF_ROPING_LOG = "ropingLog";
    // 套牛关卡统计
    public const string CALF_ROPING_PASS_LOG = "ropingPassLog";

    // 五龙游戏模式下的盈利率
    public const string DRAGON_TABLE = "dragons_table";

    // 牛牛的牌型表
    public const string COWS_CARDS = "cows_cards";

    //牛牛牌局查询
    public const string LOG_COWS_CARD_BOARD = "logCowCardBoard";

    //牛牛黑白名单设置列表
    public const string COW_CARD_SPECIL_LIST = "cowsWhiteBlackList";

    //牛牛杀分放分LOG记录列表
    public const string COWS_CARD_CTRL_LIST = "logCowWBKillSendScore";

    // 黑红梅方的结果控制
    public const string SHCD_RESULT = "shcdcards_gm_cards";

    // 重新加载鱼表
    public const string RELOAD_FISHCFG = "fishlord_cfg";
    public const string RELOAD_FISHPARK_CFG = "fishpark_cfg";

    // 客服信息表
    public const string SERVICE_INFO = "serviceInfo";

    public const string COMMON_CONFIG = "common_config";

    // 游戏充值信息
    public const string GAME_RECHARGE_INFO = "pay_infos";

    // 支付总表
    public const string RECHARGE_TOTAL = "player_pay";

    //通过爱贝支付
    public const string RECHARGE_TOTAL_AIBEI = "aibei_pay";

    // 付费点分布
    public const string RECHARGE_DISTRIBUTION = "rechargeDistribution";

    // 测试游戏表
    public const string TEST_SERVER = "TestServers";

    // 经典捕鱼玩家表
    public const string FISHLORD_PLAYER = "fishlord_player";
    // 鳄鱼公园玩家表
    public const string FISHPARK_PLAYER = "fishpark_player";

    // 头像举报
    public const string INFORM_HEAD = "informHead";

    // 踢出玩家
    public const string KICK_PLAYER = "KickPlayer";

    public const string DAY_ACTIVATION = "day_activation";
    // 渠道数据统计日
    public const string CHANNEL_STAT_DAY = "channelStatDay";

    // 渠道通过爱贝产生的充值
    public const string CHANNEL_RECHARGE_AIBEI = "channelRechargeByAibei";

    // 渠道相关的统计数据
    public const string CHANNEL_TD = "channelTalkingData";
    // 渠道相关的充值统计
    public const string CHANNEL_TD_PAY = "channelTalkingDataPay";


    //非新增玩家首次付费
    public const string PUMP_OLD_FIRST_RECHARGE = "pumpOldFirstRecharge";

    // 玩家拥有的总金币统计日
    public const string TOTAL_MONEY_STAT_DAY = "totalMoneyStatDay";

    // VIP流失
    public const string RLOSE = "vipLose";

    // 大奖赛周冠军
    public const string MATCH_GRAND_PRIX_WEEK_CHAMPION = "fishlord_match_champion";
    // boss记录
    public const string PUMP_BOSSINFO = "logBossInfo";

    public const string MATCH_GRAND_PRIX_DAY = "fishlord_match_day";

    // 安全账号列表
    public const string MATCH_GRAND_SAFE_ACCOUNT = "fishlord_match_safe_account";

    // 玩家龙珠统计
    public const string STAT_PLAYER_DRAGON = "statPlayerDragonBall";
    // 每日龙珠总计
    public const string STAT_DRAGON_DAILY = "statDragonBallDaily";

    // 玩家付费监控
    public const string PUMP_RECHARGE_FIRST = "pumpRechargeFirst";
    // 玩家累计的游戏时间
    public const string STAT_PLAYER_GAME_TIME = "statPlayerGameTime";

    // 玩家在线时间段
    public const string PUMP_PLAYER_ONLINE_TIME = "pumpPlayerOnlineTime";

    // 收支总计
    public const string STAT_INCOME_EXPENSES = "statIncomeExpenses";

    // 收支总计错误
    public const string STAT_INCOME_EXPENSES_ERROR= "statIncomeExpenses_error";

    // 每天收支的总数据库结余
    public const string STAT_INCOME_EXPENSES_REMAIN = "statIncomeExpensesRemain";

    // 收支总计 新
    public const string STAT_INCOME_EXPENSES_NEW = "statIncomeExpensesNew";

    // 每小时收入统计
    public const string STAT_RECHARGE_HOUR = "statRechargeHour";

    // 每小时收入统计分渠道
    public const string STAT_RECHARGE_HOUR_BYCHANNEL = "statRechargeHourByChannel";

    // 每小时在线人数
    public const string STAT_ONLINE_HOUR = "statOnlinePlayerNumHour";

    // 活跃行为--用户喜好 在线时间
    public const string STAT_GAME_TIME_FOR_PLAYER_FAVOR_RESULT = "statGameTimeForPlayerFavorResult";
    // 时长分布
    public const string STAT_GAME_TIME_FOR_DISTRIBUTION_RESULT = "statGameTimeForDistributionResult";
    // 首付游戏时长分布
    public const string STAT_FIRST_RECHARGE_GAME_TIME_DISTRIBUTION_RESULT = "statFirstRechargeGameTimeDistributionResult";
    // 首次购买计费点分布
    public const string STAT_FIRST_RECHARGE_POINT_DISTRIBUTION_RESULT = "statFirstRechargePointDistributionResult";
    // 玩家下注情况统计
    public const string STAT_PLAYER_GAME_BET_RESULT = "statPlayerGameBetResult";
    // 当日新增用户金币下注分布
    public const string STAT_NEW_PLAYER_OUTLAY_DISTRIBUTION = "statNewPlayerOutlayDistributionResult";

    public const string STAT_NEW_PLAYER_ENTER_ROOM = "pumpNewPlayerGame";
    // 新增玩家发炮次数分布
    public const string STAT_NEW_PLAYER_FIRECOUNT_DISTRIBUTION = "statNewPlayerFireCountDistributionResult";
    // 新增玩家发捕鱼等级分布
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
    // GM账号类型分组
    public const string GM_TYPE = "gmTypeGroup";

    //////////////////////////////////活动.................////////////////////////////////////////
    public const string FISHLORD_ACT_BENEFIT_RECV = "FishlordActBenefitRecv";

    //玩家聊天记录查询
    public const string PUMP_PLAYER_CHAT = "pumpChat";

    //小游戏开关设置
    public const string CHANNEL_OPEN_CLOSE_GAME = "channelOpenCloseGame";

    //国庆节活动 
    //领取奖励
    public const string PUMP_ND_ACT = "pumpNdAct";
    //排行榜
    public const string PUMP_NATIONAL_DAY_ACTIVITY = "nationalDayActivity";

    //中秋特惠活动
    public const string PUMP_JIN_QIU_RECHARGE_LOTTERY = "pumpJinQiuRechargeLottery";

    //万圣节活动
    //排行榜
    public const string PUMP_HALLOWMAS_ACT_RANK = "hallowmasActivity";
    //领取奖励
    public const string PUMP_HALLOWMAS_ACT_RECV = "pumpHallowmas";

    //强更补偿查询
    public const string PUMP_FORCE_UPDATE_REWARD = "pumpForceUpdateReward";

    //圣诞节/元旦活动
    public const string PUMP_CHRISTMAS = "pumpChristmas";

    //金蟾夺宝排行榜
    public const string FISHLORD_SPITTOR_SNATCH_ACTIVITY = "fishlordSpittorSnatchActivity";

    //金蟾夺宝领取奖励人数统计
    public const string PUMP_SPITTOR_SNATCH = "pumpSpittorSnatch";

    //活动幸运宝箱
    public const string PUMP_PANIC_BOX = "pumpPanicBox";
    //活动幸运宝箱玩家
    public const string PUMP_PANIC_BOX_PLAYER = "pumpPanicBoxPlayer";

    //比武场数据统计
    public const string PUMP_BW = "pumpBw";

    //微信公众号签到统计
    public const string PLAYER_WECHAT_RECV_STAT = "player_wechatRecvStat";
    //微信公众号统计
    public const string PLAYER_OPENID = "player_openId";

    //////////////////////////////////////////////////////////////////////////
    // 活动设置，设置开始，结束时间
    public const string ACT_CHANNEL100003 = "actChannelPlayer100003";

    public const string STAT_CHANNEL100003 = "statChannelPlayer100003";
}

public static class StrName
{
    public static string[] s_rechargeType = { "人民币" };

    public static string[] s_statLobbyName = { "全部", "赠送礼物", "小喇叭", "vip等级分布情况", "上传头像",
                                               "昵称修改", "签名修改", "性别修改", "头像框购买", "在线奖励",
                                             "救济金","保险箱存入","保险箱取出"};

    //public static string[] s_gameName = { "大厅", "经典捕鱼", "鳄鱼大亨", "欢乐骰宝", "万人牛牛", "百家乐", "五龙", "套牛", 
    //                                        "抓姓姓", "鳄鱼公园", "黑红梅方","","水浒传","奔驰宝马"};

    public static string[] s_gameName = { "大厅", "经典捕鱼", "鳄鱼大亨", "万人牛牛", "黑红梅方","水浒传","奔驰宝马"};


    //public static string[] s_gameName1 = { "系统", s_gameName[1], s_gameName[2], s_gameName[3], 
    //                                         s_gameName[4], s_gameName[5], s_gameName[6],
    //                                         s_gameName[7], s_gameName[8], s_gameName[9],s_gameName[10] };

    public static string[] s_roomName = { "初级场", "中级场", "高级场", "碎片场" };

    public static string[] s_fishLordRoomName = { "初级场", "中级场", "高级场", "VIP专场","竞技场","至尊场","东海龙宫场"};

    public static string[] s_shcdRoomName = { "", "金币场", "龙珠场" };

    //public static string[] s_fishRoomName = { s_roomName[0], s_roomName[1], s_roomName[2], s_roomName[3], 
    //                                            "普通赛初级场", "普通赛中级场", "普通赛高级场", "普通赛大师场", "大奖赛", "", "竞技场", "", "", "", "至尊场","","","","","东海龙宫场" };

    public static string[] s_fishRoomName = { s_roomName[0], s_roomName[1], s_roomName[2], s_roomName[3], 
                                                "初级龙宫场", "巨鲨场", "高级龙宫场", "巨鲲场", "圣兽场", "", "竞技场", "", "", "", "至尊场","","","","","东海龙宫场" };

    public static string[] s_dragonRoomName = { "初级场", "高级场", "大师场" };

    public static string[] s_stageName = { "大天堂", "中天堂", "小天堂", "正常", "小地狱", "中地狱", "大地狱" };

    private static Dictionary<string, string> s_gameName2 = new Dictionary<string, string>();
    
    public static string[] s_cowsArea = { "东", "南", "西", "北" };

    public static string[] s_shcdArea = { "黑桃", "红心", "梅花", "方块", "大小王" };
    
    public static string[] s_dragonArea = { "最终倍率", "福袋倍率", "开花倍率" };

    public static string[] s_shuihzArea = { "最终倍率","小玛丽获得"};

    public static string[] s_wishCurse = { "祝福", "诅咒" };

    public static string[] s_starLotteryName = { "普通抽奖", "青铜抽奖", "白银抽奖", "黄金抽奖", "钻石抽奖", "至尊抽奖" };

    public static string[] s_bzArea = { "红奔驰","绿奔驰","黄奔驰","红宝马","绿宝马","黄宝马","红奥迪","绿奥迪","黄奥迪","红大众","绿大众","黄大众","人人有奖","幸运射灯","欢乐彩金"};

    // 当前上线的游戏ID列表
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
       //s_gameName3.Add((int)GameId.shuihz, "水浒传");
       //s_gameName3.Add((int)GameId.bz,"奔驰宝马");

       s_gameName3.Add((int)GameId.lobby, s_gameName[0]);
       s_gameName3.Add((int)GameId.fishlord, s_gameName[1]);

       //捕鱼游戏房间 //{ "初级场", "中级场", "高级场", "VIP专场","竞技场","至尊场","东海龙宫","巨鲲场"};
       s_roomList.Add((int)RoomId.room1, s_fishLordRoomName[0]);
       s_roomList.Add((int)RoomId.room2, s_fishLordRoomName[1]);
       s_roomList.Add((int)RoomId.room3, s_fishLordRoomName[2]);
       s_roomList.Add((int)RoomId.room4, "碎片场");
       s_roomList.Add((int)RoomId.room6, "巨鲨场");

       s_roomList.Add((int)RoomId.room5, "初级龙宫场");
       s_roomList.Add((int)RoomId.room7, "高级龙宫场");
       s_roomList.Add((int)RoomId.room8, "巨鲲场");
       s_roomList.Add((int)RoomId.room9, "圣兽场");

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
            //{ "大厅", "经典捕鱼", "鳄鱼大亨", "万人牛牛", "黑红梅方","水浒传","奔驰宝马"};
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

    // 充值状态
    public static string[] s_rechargeState = { "已到账", "请求支付", "支付成功" };
}

// public enum PaymentType
// {
//     e_pt_none = 0,
//     e_pt_anysdk,        //anysdk综合
//     e_pt_qbao,          //钱宝
//     e_pt_baidu,         //百度
//     e_pt_max,
// }

// 支付状态
public struct PayState
{
    //全部
    public const int PAYSTATE_ALL = -1;
    // 成功
    public const int PAYSTATE_SUCCESS = 0;
    // 已请求支付
    public const int PAYSTATE_HAS_REQ = 1;
    // 支付成功
    public const int PAYSTATE_HAS_PAY = 2;
}

//错误类型
public struct ErrorType 
{
    //全部
    public const int ERRORTYPE_ALL = 0;
    //金币错误
    public const int ERRORTYPE_GOLD = 1;
    //钻石错误
    public const int ERRORTYPE_GEM = 2;
    //龙珠错误
    public const int ERRORTYPE_DB = 3;
    //话费券错误
    public const int ERRORTYPE_CHIP = 4;
}

// 添加同步道具失败原因
public enum AddItemFailReason
{
	// 不在捕鱼服务器内
	fail_reason_player_not_in_fish = 0,

	// 玩家掉线，与logic断开连接
	fail_reason_player_offline = 1,

	// 超出确认时间
	fail_reason_beyond_verity_time = 2,    

	// 没找到玩家
	fail_reason_not_find_player = 3,
};

// 玩家金币，钻石的变化原因
public enum PropertyReasonType
{
    // 每日登录转盘抽奖
    //type_reason_dial_lottery = 1,

	// 含义改成  使用鱼雷
	type_reason_use_torpedo = 2,

	// 保险箱存入
    //type_reason_deposit_safebox = 3,

	// 保险箱取出
    //type_reason_draw_safebox = 4,

	// 赠送礼物
    type_reason_send_gift = 5,

	// 接收礼物
    //type_reason_accept_gift = 6,

	// 玩家发小喇叭，全服通告
    //type_reason_player_notify = 7,
	
	// 玩家兑换礼物
	type_reason_exchange = 8,

	// 购买商品获得
	type_reason_buy_commodity_gain = 9,

	// 领取救济金
	type_reason_receive_alms = 10,

	// 单局结算
	type_reason_single_round_balance = 11,

	// 购买商品消耗
	type_reason_buy_commodity_expend = 12,
	
	//购买捕鱼等级
	type_reason_buy_fishlevel = 13,

	//购买捕鱼道具
	type_reason_buy_fishitem = 14,

	//玩家升级
	type_reason_player_uplevel = 15,

	// 新手引导
	type_reason_new_guild = 16,

	// 修改头像
    type_reason_update_icon = 17,

	// 充值
	type_reason_recharge = 18,

	// 修改昵称
    type_reason_modify_nickname = 19,

	// 充值赠送
	type_reason_recharge_send = 20,

	// 后台充值
	type_reason_gm_recharge = 21,

	// 后台充值赠送
	type_reason_gm_recharge_send = 22,

	// 月卡每日领取
	type_reason_month_card_daily_recv = 23,

	// 充值礼包
	type_reason_recharge_gift = 24,

	// 每日签到
	type_reason_daily_sign = 25,

	// 每日宝箱抽奖
    //type_reason_daily_box_lottery = 26,

	// 谢谢参与兑换
    //type_reason_thank_you_exchange = 27,

	// 连续发小喇叭
    //type_reason_continuous_send_speaker = 28,

	// 领取邮件
	type_reason_receive_mail = 29,

	// 捕鱼掉落
	type_reason_fishlord_drop = 30,

	// 创建账号
    //type_reason_create_account = 31,

	// 领取活动奖励
	type_reason_receive_activity_reward = 32,
	
	//活动兑换
    type_reason_receive_activity_exchange = 33,
	
	//百家乐提前下庄
    //type_reason_leave_banker = 34,

	// 使用技能
	//type_reason_use_skill = 35,
	
	//五龙翻倍游戏使用钻石   含义改成： 零点清空
	//type_reason_double_game = 36,
	
	//五龙升级 //钻石收入和金币支出
    //type_reason_dragons_lv = 37,	

	//星星奖池
	//type_reason_lucky_award = 38,

	//幸运抽奖	
	type_reason_lucky_lottery = 39,

	//新手礼包
	type_reason_new_player = 40,

	//每日任务
	type_reason_daily_task = 41,

	//每周任务
	type_reason_weekly_task = 42,

	//主线任务
	type_reason_mainly_task = 43,

	//成就
	//type_reason_achievement = 42;
	//导弹产出
	//type_reason_missile = 43;

	// 充值抽奖
	type_reason_recharge_lottery = 44,
	
	//引导充值礼包
	type_reason_recharge_guide_gift = 45,
	
	//活跃开宝箱
	type_reason_active_box = 46,

	// 玩小游戏兑换而来
    //type_reason_play_game = 47,
	// VIP福利
	type_reason_get_vipgold = 48,

	// 比赛门票
    //type_reason_match_ticket = 49,

	// 后台操作
	type_reason_gm_op = 50,

	// 购买材料礼包
    type_reason_buy_material_gift = 51,

	// 使用激光
    //type_reason_use_missile = 52,

	type_reason_receive_cdkey = 53,

	// 获取GM邮件
	type_reason_receive_gm_mail = 54,
	
	// 新手七日狂欢
	type_reason_7_days_carnival = 55,

	// 抢购
    //type_reason_panic_buy = 56,

	// 领取捐增玩偶的奖励
    //type_reason_recv_donate_puppet_reward = 57,

	// 购买玩偶
    //type_reason_buy_puppet = 58,

	// 话费鱼抽奖
	//type_reason_chipfish_lottery = 59,

	// 领取万炮奖励  双11活动
    //type_reason_recv_wpreward = 60,

	// 金币盛宴
    //type_reason_gold_feast = 61,

	// 竞技场任务
    //type_reason_match_quest = 62,

	// 圣诞元旦活动
    //type_reason_christmas = 63,
    
	//水果机比大小
    //type_reason_fruit_bs = 64,

	// 刮刮乐
    //type_reason_scratch_ticket = 65,

	// 累计充值
	//type_reason_acc_recharge = 66,

	// 新春礼包
    //type_reason_ny_gift = 67,

	// 冒险
    //type_reason_ny_adventure = 68,

    // 绑定手机
	type_reason_account_update =69,

	//龙珠单局结算
    //type_reason_dragon_cow =69,    //牛牛

    //type_reason_dragon_shcdcard = 70, //黑红梅方

	//龙珠捕鱼掉落
    //type_reason_dragon_deity_fish = 71,    //金龙神

    //type_reason_dragon_king_fish = 72,   //龙王  

    //type_reason_dragon_monster = 73,      //海怪

	//龙珠兑换
    //type_reason_chip_ex_dragon = 74,      //彩券兑换龙珠
    //type_reason_dragon_ex_fishitem = 75,      //龙珠兑换弹头
    //type_reason_dragon_ex_dragonchip = 76,     //龙珠兑换龙珠碎片
    //type_reason_dragonchip_ex_dragon = 77,    //龙珠碎片兑换龙珠

	// 使用道具
	type_reason_use_item = 79,

    //type_reason_world_cup_bet = 80,

	// 活动幸运宝箱
    //type_reason_panic_buy_box = 81,

	// 魔石兑换
	type_reason_dimensity_exchange = 82,

	// 创建比武房间
    //type_reason_create_bw_room = 83,

	// 输掉比武
    //type_reason_lose_bw = 84,

	// 赢得比武
    //type_reason_win_bw = 85,

	// 领取围剿龙王奖励
    //type_reason_recv_wjlw_reward = 86,

	// 装配炮弹
    //type_reason_recv_wjlw_equip = 87,

	// 转盘鱼奖励
    //type_reason_turnfish = 88,
	// 中级场兑换
	type_reason_medium_exchange = 89,

	type_reason_kill_grandfish = 90,

	type_reason_airdrop = 91,

	// 碎片兑换
	type_reason_chip_exchange = 92,

	type_reason_weekly_clear = 93,
	type_reason_daily_clear = 94,

	//炮台升级
	type_reason_turret_uplevel = 95,

	//新手任务
	type_reason_newplayer_task = 96, 

	//主线任务
    type_reason_parally_task = 97,

    //qq蓝钻
	type_reason_qq_reward = 98,

	// 南海寻宝
	type_reason_southsea = 99,

    //弹头合成
	type_torpedo_compse = 100,
	
	//在线奖励
    type_reason_online_reward = 101,

    //巨鲨场使用轰炸机
	type_reason_shark_bomb_aircraft = 102,

	//巨鲨场抽奖
	type_reason_shark_lottery = 103,

	//巨鲨场斩立决
	type_reason_shark_excute = 104,

    //明日礼包
    type_reason_tommorrow_gift = 105,

    //追击蟹将
    type_reason_kill_crab = 106,

    //钻头虾
    type_reason_drill_kill = 107,

    //材料转升级石
    type_reason_system_clear = 108,

    //炮台升级任务
	type_reason_turret_uplevel_task = 109,

	//含义改成  使用新手鱼雷
	type_reason_use_new_player_torpedo = 110,

    //新手南海寻宝
	type_reason_new_player_southsea = 111,

    //成长基金
    type_reason_new_player_grow_fund = 112,

    //播放广告
    type_reason_paly_ad = 113,

    //龙宫初级诛龙箭
    type_reason_dragon_excute = 114,

    //龙宫高级诛龙箭
    type_reason_dragon_excute2 = 115,

    //累计签到
    type_reason_accum_sign = 116,

    //签到VIP奖励
    type_reason_sign_vip_reward = 117,

    //欢乐炸奖励
	type_reason_torpedo_activity_reward = 118,

	//领取起航礼包
	type_reason_new_player_gift = 119,

    // 领取微信礼包	type_reason_receive_wechat_gift = 120,

    // 微信邀请任务	type_reason_invite_task = 121,	// 巨鲲场孵化	type_reason_legendary_fish_hatch = 122,    // 巨鲲场召唤BOSS	type_reason_legendary_fish_call_boss = 123,	//VIP专属	type_reason_vip_exclusive = 124,

    //新手七日充值
	type_reason_new_player_seven_day_recharge = 125,

	//新手红包
	type_reason_redpacket = 126,

    //圣兽礼包转盘
    type_reason_mythical_gift_lotter = 127,

    //圣兽积分转盘
    type_reason_mythical_point_award = 128,

    //捕鱼王成长计划
	type_reason_growth_quest = 129,

    //场次任务
	type_reason_room_quest = 130,

    //捞鱼活动
    type_reason_catch_fish_activity = 131,

    type_max,
};

// 玩家拥有的属性类型
public enum PropertyType
{
    property_type_full,

    // 金币
    property_type_gold,

    // 钻石
    property_type_diamond,

    // 彩券
    property_type_ticket,

    // 龙珠币
    property_type_dragon_ball = 14,	

    //魔石
    property_type_moshi = 18,	

    //碎片
    property_type_chip = 20,  

    //红包
    property_type_hongbao = 106,

}

public enum DataStatType
{
    // 赠送礼物
    stat_send_gift = 1,

    // 小喇叭
    stat_player_notify,

    // vip等级分布情况
    stat_player_vip_level,

    // 上传头像
    stat_upload_head_icon,

    // 昵称修改
    stat_nickname_modify,

    // 签名修改
    stat_self_signature_modify,

    // 性别修改
    stat_sex_modify,

    // 头像框购买
    stat_photo_frame,

    // 在线奖励
    stat_online_reward,

    // 救济金
    stat_relief,

    // 保险箱存入
    stat_safe_box_deposit,

    // 保险箱取出
    stat_safe_box_draw,

    stat_max,
};

public enum GameId 
{
    lobby = 0,   // 大厅
    fishlord = 1, // 经典捕鱼

    crocodile,    // 鳄鱼大亨

    dice,         // 欢乐骰宝

    cows,         // 万人牛牛
    
    baccarat,     // 百家乐
    
    dragon,       // 五龙

    calf_roping,   // 套牛
    prize_claw,    // 抓姓姓

    fishpark,     // 鳄鱼公园

    shcd,         // 黑红梅方
    shuihz = 12,       //水浒传
    bz=13,        //奔驰宝马
    fruit=14,     //水果机
    jewel = 15,     //海鲜联萌
    gameMax,
}

public enum RoomId 
{
    room1 = 1,//初级场

    room2 = 2,//中级场

    room3 = 3,//高级场

    room4 = 4,//碎片场
    
    room5 = 5,//初级龙宫场

    room6 = 6, //巨鲨场

    room7 = 7,//高级龙宫场

    room8 = 8, //巨鲲场

    room9 = 9, //圣兽场

    room11 = 11,//竞技场

    room15 = 15,//至尊场

    room20 = 20,//东海龙宫
}

//////////////////////////////////////////////////////////////////////////

// 捕鱼消耗类型
public enum FishLordExpend
{
    fish_buyitem_start,			//购买物品 Fish_ItemCFG
    fish_buyitem_end = fish_buyitem_start + 31,

    fish_useskill_start = 100,	//使用技能 Fish_BuffCFG
    fish_useskill_end = fish_useskill_start + 10,

    fish_turrent_uplevel_start = 150,         // 炮台升级开始
    fish_turrent_uplevel_end = fish_turrent_uplevel_start + 55,

    fish_unlock_level_start = 300,
    fish_unlock_level_end = fish_unlock_level_start + 55,

    // 导弹消耗
    fish_missile = 500,
    fish_missile_end = 500 + 3,
};

public enum RechargeType
{
    // 充人民币
    rechargeRMB,

    // 删除自定义头像
    delIconCustom,

    gold,   // 金币
    gem,    // 钻石
    vipExp,  // VIP经验
    dragonBall, // 龙珠

    chip, //碎片
    moshi, //魔石

    resetGiftGuideFlag,  // 重置类型为1的礼包状态
    rechargeRMBFromWeichat, // 通过公众号充值

    playerXP,  //玩家经验
};

public struct PlayerType
{
    public const int TYPE_ACTIVE = 1;          // 活跃用户
    public const int TYPE_RECHARGE = 2;        // 付费用户
    public const int TYPE_NEW = 3;             // 新增用户
}

// 支付类型
public struct PayType
{
    // 使用公众号方式充值
    public const int WeChatPublicNumer = 1;
}

//////////////////////////////////////////////////////////////////////////
public class GameStatData
{
    // 当日注册人数
    public int m_regeditCount;

    // 当日设备激活数量
    public int m_deviceActivationCount;

    // 活跃人数
    public int m_activeCount;

    // 设备活跃数量
    public int m_deviceLoginCount;

    // 当天总收入
    public int m_totalIncome;

    // 付费人数
    public int m_rechargePersonNum;

    // 付费次数
    public int m_rechargeCount;

    // 当日新增的用户付费总计
    public int m_newAccIncome;
    // 当日新增的用户中，付费用户数量
    public int m_newAccRechargePersonNum;

    // 留存率计算时的总注册人数
    //public int m_2DayRegeditCount;

    // 次日留存人数
    public int m_2DayRemainCount;

    //public int m_3DayRegeditCount;

    // 3日留存人数
    public int m_3DayRemainCount;

    //public int m_7DayRegeditCount;

    // 7日留存人数
    public int m_7DayRemainCount;

    //public int m_30DayRegeditCount;

    // 30日留存人数
    public int m_30DayRemainCount;
    
    // 1日总充值， -1表示还没有数据
    public int m_1DayTotalRecharge = -1;
    // 3日总充值， -1表示还没有数据
    public int m_3DayTotalRecharge = -1;
    // 7日总充值， -1表示还没有数据
    public int m_7DayTotalRecharge = -1;
    // 14日总充值， -1表示还没有数据
    public int m_14DayTotalRecharge = -1;
    // 30日总充值， -1表示还没有数据
    public int m_30DayTotalRecharge = -1;
    // 60日总充值， -1表示还没有数据
    public int m_60DayTotalRecharge = -1;
    // 90日总充值， -1表示还没有数据
    public int m_90DayTotalRecharge = -1;

    //////////////////////////////////////////////////////////////////////////
    // 次日设备留存人数，临时数据
    public int m_2DayDevRemainCount = -1;

    // 3日设备留存人数
    public int m_3DayDevRemainCount = -1;

    // 7日设备留存人数
    public int m_7DayDevRemainCount = -1;

    // 30日设备留存人数
    public int m_30DayDevRemainCount = -1;

    //////////////////////////////////////////////////////////////
    // 次日留存人数(付费)
    public int m_2DayRemainCountRecharge;

    // 3日留存人数(付费)
    public int m_3DayRemainCountRecharge;

    // 7日留存人数(付费)
    public int m_7DayRemainCountRecharge;

    //////////////////////////////////////////////////////////////////////////
    // 次日付费人数
    public int m_2DayRechargePersonNum = 0;
    //3日付费人数
    public int m_3DayRechargePersonNum = 0;
    //7日付费人数
    public int m_7DayRechargePersonNum = 0;

    // 注册当日进入渔场人数
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


