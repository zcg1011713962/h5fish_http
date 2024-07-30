using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.Configuration;

public struct DefCC
{
    //添加同步道具失败补单
    public const string ASPX_SERVICE_MAIL = "/appaspx/service/ServiceMail.aspx";

    //添加同步道具失败手动确认
    public const string ASPX_PLAYER_ITEM_RECORD = "/appaspx/operation/PlayerItemRecord.aspx";

    //账号查询新加背包查询
    public const string ASPX_ACCOUNT_BEIBAO = "/appaspx/service/ServiceAccountBeibao.aspx";

    // 详情查看页（爆金比赛场周排行榜）
    public const string ASPX_GAME_DETAIL_BAOJIN = "/appaspx/stat/fish/FishlordBaojinRankDetail.aspx";
    // 详情查看页
    public const string ASPX_GAME_DETAIL = "/appaspx/stat/gamedetail/GameDetailViewer.aspx";
    // 百家乐详情
    public const string ASPX_GAME_DETAIL_BACCARAT = "/appaspx/stat/gamedetail/GameDetailBaccarat.aspx?index={0}";
    // 牛牛详情
    public const string ASPX_GAME_DETAIL_COWS = "/appaspx/stat/gamedetail/GameDetailCows.aspx?index={0}";
    //详情查看页（牛牛牌局查询）
    public const string ASPX_GAME_DETAIL_COWS_CARDS = "/appaspx/stat/cows/CowsCardsDetail.aspx";
    //详情查看页下注玩家列表（牛牛牌局查询）
    public const string ASPX_GAME_DETAIL_COWS_CARDS_PLAYER_LIST = "/appaspx/stat/cows/CowsCardsPlayerList.aspx";
    // 鳄鱼大亨详情
    public const string ASPX_GAME_DETAIL_CROCODILE = "/appaspx/stat/gamedetail/GameDetailCrocodile.aspx?index={0}";
    //水果机详情
    public const string ASPX_GAME_DETAIL_FRUIT = "/appaspx/stat/gamedetail/GameDetailFruit.aspx?index={0}";
    //奔驰宝马
    public const string ASPX_GAME_DETAIL_BZ = "/appaspx/stat/gamedetail/GameDetailBz.aspx?index={0}";
    //宝石迷阵详情
    public const string ASPX_GAME_DETAIL_JEWEL = "/appaspx/stat/gamedetail/GameDetailJewel.aspx?index={0}";
    // 骰宝详情
    public const string ASPX_GAME_DETAIL_DICE = "/appaspx/stat/gamedetail/GameDetailDice.aspx?index={0}";
    // 鳄鱼公园详情
    public const string ASPX_GAME_DETAIL_FISH_PARK = "/appaspx/stat/gamedetail/GameDetailFishPark.aspx?index={0}";
    // 五龙详情
    public const string ASPX_GAME_DETAIL_DRAGON = "/appaspx/stat/gamedetail/GameDetailDragon.aspx?index={0}";
    // 水浒传详情
    public const string ASPX_GAME_DETAIL_SHUIHZ = "/appaspx/stat/gamedetail/GameDetailShuihz.aspx?index={0}";

    // 黑红梅方详情
    public const string ASPX_GAME_DETAIL_SHCD = "/appaspx/stat/gamedetail/GameDetailShcd.aspx?index={0}";
    //详情查看页（黑红梅方牌局查询）
    public const string ASPX_GAME_DETAIL_SHCD_CARDS = "/appaspx/stat/shcd/ShcdCardsDetail.aspx";
    //详情查看页下注玩家列表（黑红梅方牌局查询）
    public const string ASPX_GAME_DETAIL_SHCD_CARDS_PLAYER_LIST = "/appaspx/stat/shcd/ShcdCardsPlayerList.aspx";

    public const string ASPX_LOGIN = "/appaspx/Login.aspx";
    //public const string ASPX_LOGIN = "~/Account/Login.aspx";

    // 活动幸运宝箱 详情查看页
    public const string ASPX_PANIC_BOX_DETAIL = "/appaspx/stat/StatPanicBoxPlayer.aspx";

    //国庆中秋快乐 抽奖统计 详情页
    public const string ASPX_JINQIU_NATIONALDAY_ACT_DETAIL = "/appaspx/stat/StatJinQiuNationalDayActDetail.aspx";

    //转盘鱼捕鱼统计详情
    public const string ASPX_STAT_TFISH_DEATIL = "/appaspx/stat/StatTurnTableFishDetail.aspx";

    //双十一活动 任务统计 详情页
    public const string ASPX_JINQIU_NATIONALDAY_ACT_TASK = "/appaspx/stat/StatJinQiuNationalDayActTask.aspx";

    // 扑克牌型
    public static string[] s_poker = { "diamond", "club", "spade", "heart" };

    public static string[] s_pokerCows = { "diamond", "club", "heart", "spade" };
    public static string[] s_pokerColorCows = { "方块", "梅花", "红桃", "黑桃" };
    public static string[] s_pokerColorShcd = { "黑桃", "红心","梅花", "方块", "大小王" };
    // 骰宝结果描述串
    public static string[] s_diceStr = { "大", "小", "豹子" };

    public static string[] s_isBanker = { "是否上庄:是", "是否上庄:否" };

    // 扑克牌面值
    public static string[] s_pokerNum = { "", "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    // 百家乐的结果
    public static string[] s_baccaratResult = { "和", "闲", "闲对", "庄对", "庄" };

    // 黑红梅方的扑克牌
    public static string[] s_pokerShcd = { "spade", "heart", "club", "diamond", "joker" };

    public const int OP_ADD = 0;      // 添加
    public const int OP_REMOVE = 1;   // 移除
    public const int OP_MODIFY = 2;   // 修改
    public const int OP_VIEW = 3;     // 查看

    public static string HTTP_MONITOR = Convert.ToString(WebConfigurationManager.AppSettings["httpMonitor"]);

    public static string RRD_TOOL_PATH = Convert.ToString(WebConfigurationManager.AppSettings["rrdtoolPath"]);

    //围剿龙王 金币玩法统计
    //详情1
    public const string ASPX_WJLW_GOLD_EARN_TURN_INFO = "/appaspx/stat/StatWjlwGoldEarnTurnInfo.aspx";
    //详情2
    public const string ASPX_WJLW_GOLD_BET_PLAYER_LIST = "/appaspx/stat/StatWjlwGoldBetPlayerList.aspx";
    //围剿龙王 付费玩法统计
    //详情1
    public const string ASPX_WJLW_RECHARGE_WIN_INFO = "/appaspx/stat/StatWjlwRechargeWinInfo.aspx";
    //详情2
    public const string ASPX_WJLW_RECHARGE_BET_PLAYER = "/appaspx/stat/StatWjlwRechargeBetPlayer.aspx";

    //弹头礼包统计详情
    public const string ASPX_STAT_BULLET_HEAD_GIFT_DEATIL = "/appaspx/stat/StatBulletHeadGiftPlayer.aspx";

    //经典捕鱼大水池参数详情
    public const string ASPX_STAT_FISHLORD_CTRL_NEW_DETAIL = "/appaspx/stat/StatFishlordControlNewDetail.aspx";

    //幸运抽奖详情
    public const string ASPX_STAT_GOLD_FISH_LOTTERY_DETAIL = "/appaspx/stat/StatGoldFishLotteryDetail.aspx";

    //高级场奖池统计详情
    public const string ASPX_STAT_FISHLORD_ADVANCED_ROOM_ACT_DETAIL = "/appaspx/stat/StatFishlordAdvancedRoomActDetail.aspx";

    //每日礼包详情
    public const string ASPX_STAT_DAILY_GIFT_DETAIL = "/appaspx/stat/StatDailyGiftDetail.aspx";

    //南海寻宝统计详情
    public const string ASPX_STAT_TREASURE_HUNT_DETAIL = "/appaspx/stat/StatTreasureHuntDetail.aspx";

    //开服抽奖统计详情
    public const string ASPX_STAT_SD_LOTTERY_ACT_DETAIL = "/appaspx/stat/StatSdLotteryActDetail.aspx";

    //巨鲨场玩法收入统计详情
    public const string ASPX_STAT_FISHLORD_SHARK_ROOM_INCOME_DETAIL = "/appaspx/stat/StatFishlordSharkRoomIncomeDetail.aspx";

    //巨鲨场抽奖统计详情
    public const string ASPX_STAT_FISHLORD_SHARK_ROOM_LOTTERY_DETAIL = "/appaspx/stat/StatFishlordSharkRoomLotteryDetail.aspx";

    //十一活动抽奖统计详情
    public const string ASPX_STAT_NATIONAL_DAY_ACT_LOTTERY_DETAIL = "/appaspx/stat/StatNationalDayActLotteryDetail.aspx";

    //追击蟹将活动统计
    //抽奖统计 详情页
    public const string ASPX_STAT_KILL_CRAB_ACT_LOTTERY_DEATIL = "/appaspx/stat/StatKillCrabActLotteryDetail.aspx";

    //破产统计详情
    public const string ASPX_OPERATION_PLAYER_BANKRUPT_DETAIL = "/appaspx/operation/OperationPlayerBankruptDetail.aspx";

    //中级场兑换统计详情
    public const string ASPX_STAT_MIDDLE_ROOM_EXCHANGE_DETAIL = "/appaspx/stat/StatFishlordMiddleRoomActExchangeDetail.aspx";

    //捕鱼活动
    public const string ASPX_STAT_HUNT_FISH_ACTIVITY_DETAIL = "/appaspx/stat/StatPumpHuntFishActDetail.aspx";

    //累充奖励
    public const string ASPX_STAT_RECHARGE_ACTIVITY_DETAIL = "/appaspx/stat/StatPumpHuntFishActRechargeDetail.aspx"; 
    
    //七日连充 购买情况详情
    public const string ASPX_STAT_NP_7DAY_RECHARGE_DETAIL = "/appaspx/stat/StatPumpNp7DayRechargeDetail.aspx";

    //五一活动抽奖 详情
    public const string ASPX_STAT_PUMP_WUYI_ACT_DETAIL = "/appaspx/stat/StatPumpWuyiActDetail.aspx";

    //捕鱼王统计
    public const string ASPX_STAT_GROWTH_QUEST_DETAIL = "/appaspx/stat/StatGrowthQuestDetail.aspx";
}

public enum QueryType
{
    //场次活动 圣兽活动 巨鲨活动
    queryTypeStatRoomQuestAct,


    //广告投放活动设置
    queryTypeOperationAdActSet,

    //作弊排行榜
    queryTypeOperationRankCheat,

    //捕鱼王统计
    queryTypeStatGrowthQuest,
    //详情
    queryTypeStatGrowthQuestDetail,

    //玩法鱼统计
    //骰子鱼
    queryTypeStatDiceGame,

    //贝壳
    queryTypeStatShellGame,

    //钻头虾炸弹蟹
    queryTypeStatXiaXieGame,

    //圣兽场玩法统计
    //朱雀收入统计
    queryTypeStatMythicalRoomZhuque,
    //白虎收入统计
    queryTypeStatMythicalRoomBaihu,
    //玄武收入统计
    queryTypeStatMythicalRoomXuanwu,
    //财神双倍玩法统计
    queryTypeStatMythicalRoomCaishen,

    //排行榜 当前
    queryTypeStatMythicalRoomCurrRank,
    //排行榜 历史
    queryTypeStatMythicalRoomHisRank,
    //当前排行榜 玩家积分
    queryTypeStatMythicalRoomRankPlayer,

    //五一活动统计
    //五一活动
    queryTypeStatPumpWuyiSetAct,
    //五一宝库
    queryTypeStatPumpWuyiSetActLottery,

    //五一宝库详情
    queryTypeStatPumpWuyiSetActLotteryList,

    //七日连充
    //购买情况
    queryTypeStatPumpNp7DaysRecharge,
    //购买情况详情
    queryTypeStatPumpNp7DaysRechargeDetail,
    //兑换数据
    queryTypeStatPumpNp7DaysRechangeExg,

    //红包方案
    queryTypeStatPumpRedEnvelop,

    //红包方案兑换
    queryTypeStatPumpRedEnvelopExchange,

    //机器人积分管理
    queryTypeOperationRobotRankCFG,

    //场次活动
    //捕鱼活动
    queryTypeStatPumpHuntFishActivity,
    //捕鱼活动详情
    queryTypeStatPumpHuntFishActDetail,

    //累充奖励
    queryTypeStatPumpRechargeActivity,
    //累充奖励详情
    queryTypeStatPumpRechargeActivityDetail,

    //捞鱼活动
    queryTypeStatPumpCatchFishActivity,

    //玩家积分
    queryTypeStatFishlordGoldHoopTorpedoScore,

    //巨鲲降世
    //当前
    queryTypeStatFishlordLegendaryCurRank,
    //历史
    queryTypeStatFishlordLegendaryHisRank,
    //定海神针
    queryTypeStatFishlordGoldHoopTorpedoRank,

    //玩法收入统计
    queryTypeStatFishlordLegendaryFishRoomplay,
    //孵化巨鲲统计
    queryTypeStatFishlordLegendaryFishRoom,

    //时段在线人数
    queryTypeStatDailyHourMaxOnlinePlayer,

    //富豪计划
    queryTypeStatVipExclusiveTask,

    //VIP宝库
    queryTypeStatVipExclusiveDiamondMall,

    //VIP认证
    queryTypeStatVipExclusiveBindPhone,

    //积分商城
    queryTypeStatVipExclusivePointExchange,

    //充值排行榜
    queryTypeOperationRankRecharge,

    //公众号统计
    queryTypeStatWechatGift,

    //内部库玩家统计
    queryTypeStatInnerPlayer,

    //激励视频统计
    queryTypeStatPlayAd,

    //vip特权统计
    queryTypeStatVipRecord,

    //月卡购买统计
    queryTypeStatMonthCard,

    //玩家平均携带物品
    queryTypeStatTurretItemsOnPlayer,

    //VIP特权
    queryTypeStatVipGift,

    //成长基金统计
    queryTypeStatPumpGrowFund,

    //玩家邮件统计
    queryTypePlayerMail,

    //鱼雷碎片统计
    queryTypeStatTurretChip,

    //返利礼包
    queryTypeStatRebateGift,

    //玩家携带金币
    queryTypeStatGoldOnPlayer,

    //炮倍任务
    queryTypeStatPlayerOpenRateTask,

    /////////////////////////////////////////////
    //追击蟹将抽奖统计
    queryTypeStatKillCrabActLottery,
    //追击蟹将抽奖统计详情
    queryTypeStatKillCrabActLotteryDetail,

    //追击蟹将场次统计
    queryTypeStatKillCrabActRoom,

    //追击蟹将任务统计
    queryTypeStatKillCrabActTask,

    //元旦活动
    queryTypeStatYuandanSign,
    
    //////////////十一活动
    //签到
    queryTypeStatActSign,
    //欢乐集字
    queryTypeStatActExchange,
    //冒险之路
    queryTypeStatActTask,
    //礼包购买
    queryTypeStatActGift,
    //抽奖统计
    queryTypeStatActLottery,
    //抽奖统计详情
    queryTypeStatNationalDayActLotteryDetail,

    ///////////////////////////////////////////////////////////////////////////
    //新手炮倍完成率
    queryTypeStatNewPlayerOpenRate,

    //新进玩家付费监控
    queryTypeTdNewPlayerMonitor,

    //玩家手机验证
    queryTypeStatPlayerPhoneVertify,

    //背包购买
    queryTypeStatPlayerItemRecharge,

    //在线奖励
    queryTypeStatOnlineReward,

    //开服活动统计
    queryTypeStatSdAct,

    //开服抽奖统计
    queryTypeStatSdLotteryAct,
    //开服抽奖统计详情
    queryTypeStatSdLotteryActDetail,

    //明日礼包
    queryTypeStatTomorrowGift,

    //流失点统计
    queryTypeStatNewGuildLosePoint,

    //南海寻宝
    queryTypeStatTreasureHunt,

    //南海寻宝详情
    queryTypeStatTreasureHuntDetail,

    //每日礼包统计
    queryTypeStatDailyGift,

    //每日礼包统计
    queryTypeStatDailyGiftDetail,

    ////////////////////////////////////////////////
    //高级场玩家积分修改
    queryTypeStatFishlordAdvancedRoomScore,

    //高级场控制管理
    queryTypeStatFishlordAdvancedRoom,
    //高级场奖池统计
    queryTypeStatFishlordAdvancedRoomAct,
    //高级场奖池统计详情
    queryTypeStatFishlordAdvancedRoomActDetail,
    //高级场排行榜
    queryTypeStatFishlordAdvancedRoomActRank,

    /////////////////////////////////////////////////
    //中级场玩家积分修改
    queryTypeStatFishlordMiddleRoomPlayerScore,
    //中级场玩家积分排行榜
    queryTypeStatFishlordMiddleRoomPlayerRank,
    //中级场玩法收入统计
    queryTypeStatFishlordMiddleRoomIncome,
    //中级场兑换统计
    queryTypeStatFishlordMiddleRoomExchange,
    //中级场兑换统计详情
    queryTypeStatFishlordMiddleRoomExchangeDetail,
    //中级场排行榜
    queryTypeStatFishlordMiddleRoomRank,
    //中级场打点统计
    queryTypeStatFishlordMiddleRoomFuDai,

    //中级场礼包统计
    queryTypeStatMiddleRoomExchange,
    ////////////////////////////////////////////////

    //巨鲨场
    //巨鲨场玩法收入统计
    queryTypeStatFishlordSharkRoomIncome,
    //详情
    queryTypeStatFishlordSharkRoomIncomeDetail,

    //拆解统计
    queryTypeStatFishlordSharkRoomChaijieIncome,
    queryTypeStatFishlordSharkRoomChaijieDetail,

    //轰炸机统计
    queryTypeStatFishlordSharkRoomBomb,
    //巨鲨抽奖统计
    queryTypeStatFishlordSharkRoomLottery,

    //巨鲨抽奖详情
    queryTypeStatFishlordSharkRoomLotteryDetail,

    //巨鲨场排行榜
    queryTypeStatFishlordSharkRoomRank,

    //巨鲨场作弊查询玩家
    queryTypeStatFishlordSharkRoomScore,

    //能量统计
    queryTypeStatFishlordSharkRoomEnergy,
    ////////////////////////////////////////////////

    //主线任务
    queryTypeStatMainlyTask,

    //炮数成长分布
    queryTypeOperationPlayerActTurret,

    //玩家炮数成长分布
    queryTypeOperationPlayerActTurretBySingle,

    //破产统计
    queryTypeOperationPlayerBankruptStat,
    //破产统计详情
    queryTypeOperationPlayerBankruptDetail,

    //破产炮倍详情
    queryTypeOperationPlayerOpenRateBankruptList,

    //幸运抽奖
    queryTypeStatGoldFishLottery,

    //幸运抽奖总计
    queryTypeStatGoldFishLotteryTotal,

    //幸运抽奖详情
    queryTypeStatGoldFishLotteryDetail,

    //每日周任务统计
    queryTypeStatDailyWeekTask,
    //每日周奖励统计
    queryTypeStatDailyWeekReward,

    //彩券鱼统计
    queryTypeStatLotteryExchange,

    //系统空投 发布
    queryTypeStatAirDropSys,

    //系统空投 打开
    queryTypeStatAirDropSysOpen,

    //后台大厅 统计当日信息
    queryTypeStatTodayInfo,

    //炮倍相关
    queryTypeOpnewTurretTimes,

    //玩家充值信息
    queryTypeOpnewPlayerRecharge,

    //统计玩家活跃数据
    queryTypeOpnewGameActive,

    //统计玩家付费
    queryTypeOpnewGameIncome,

    //渔场场次情况
    queryTypeStatFishingRoomInfo,

    //玩家基本信息
    queryTypeStatPlayerBasicInfo,

    //屠龙榜
    queryTypeStatKdActRank,

    //屠龙榜日排行
    queryTypeStatKdActDayRank,

    //转盘鱼排行榜
    queryTypeStatTurnTableFishRank,
    //转盘鱼捕鱼统计
    queryTypeStatTFish,
    //转盘鱼捕鱼统计详情
    queryTypeStatTFishDetail,
    //转盘鱼场次统计
    queryTypeStatTFishRoom,

    //弹头礼包统计
    queryTypeStatBulletHeadGift,
    //弹头礼包统计详情
    queryTypeStatBulletHeadGiftPlayer,

    //围剿龙王自定义付费玩家排名
    queryTypeWjlwRechargeReward,
    //金币玩法统计
    queryTypeWjlwGoldEarn,
    //金币玩法统计每局详情
    queryTypeWjlwGoldEarnTurnInfo,
    //金币玩法统计每局下注玩家列表
    queryTypeWjlwGoldBetPlayerList,

    //围剿龙王付费玩法统计
    queryTypeWjlwRechargeEarn,
    //获奖详情
    queryTypeWjlwRechargeWinInfo,
    //投注详情
    queryTypeWjlwRechargeBetPlayer,

    //微信公众号签到统计
    queryTypeWechatRecvRewardStat,

    //比武场数据统计
    queryTypeStatPlayerBw,

    //库存数据统计
    queryTypeStatPlayerMoneyRep,

    //小游戏日累计输赢
    queryTypeGameDailyTotalLoseWinRank,

    //小游戏实时输赢榜
    queryTypeGameRealTimeLoseWinRank,

    //玩家财富榜
    queryTypePlayerRichesRank,

    //国庆中秋快乐
    //修改玩家月饼数量
    queryTypeJinQiuNationalDayActCtrl,

    //排行榜统计
    queryTypeJinQiuNationalDayActRank,
    //抽奖统计
    queryTypeJinQiuNationalDayActLottery,
    //抽奖统计详情
    queryTypeJinQiuNationalDayActLotteryDetail,
    //场次统计
    queryTypeJinQiuNationDayActRoomStat,
    //任务统计
    queryTypeJinQiuNationalDayActTaskStat,
    //任务统计详情
    queryTypeJinQiuNationalDayActTaskDetail,

    //流失大户引导记录效果查询
    queryTypeGuideLostPlayersRes,

    //流失大户筛选
    queryTypeSelectLostPlayers,

    //客服补单/大户随访/换包福利-系统
    queryTypeRepairOrder,

    //活动幸运宝箱
    queryTypeStatPanicBox,
    //活动宝箱详情
    queryTypeStatPanicBoxDetail,

    //金蟾夺宝活动
    //活动排行榜
    queryTypeSpittorSnatchActRank,
    //领取奖励人数统计
    queryTypeSpittorSnatchActRewardList,

    //新手引导埋点统计
    queryTypePumpNewGuide,

    //五一充返活动
    queryTypeWuyiRewardResult,

    //欢乐炸炸炸
    queryTypeBulletHeadCurrRank, //当前排行
    queryTypeBulletHeadRank,//历史排行
    queryTypeBulletHeadPlayerScore,//炸弹乐园玩家积分
    queryTypeBulletHeadGuaranteed, //炸弹乐园入围当前
    queryTypeBulletHeadHisGuaranteed,//炸弹乐园入围历史
    
    //龙鳞排行
    queryTypeDragonScaleCurrRank,//当前排行
    queryTypeDragonScaleRank,//历史排行
    //龙鳞数量修改
    queryTypeDragonScaleControl,

    //礼包码cdkey生成器
    queryTypeGiftCodeNewList,

    //通过第三方订单号查询玩家ID
    queryTypeGetPlayerIdByOrderId,

    //勇者大冒险
    queryTypeStatNYAdventure,

    //新春重返
    queryTypeStatNYAccRecharge,

    //春节礼包
    queryTypeStatNYGift,

    //刮刮乐活动运营
    queryTypeScratchActEarn,

    //刮刮乐活动兑奖
    queryTypeScratchActExchangeRes,

    //添加同步道具失败
    queryTypeWord2LogicItemError,

    //玩家道具获取详情
    queryTypePlayerItemRecord,

    //邮件查询
    queryTypeMailQuery,

    //账号查询 背包查询
    queryTypeAccountBeibao,

    //圣诞节/元旦活动
    queryTypeChristmasOrYuandan,
    //强更补偿查询
    queryForceUpdateReward,

    //运营公告编辑查询
    queryTypeNoticeInfo,

    //水果机
    //水果机参数调整
    queryTypeFruitParamControl,

    //水果机黑白名单列表
    queryTypeFruitSpecilList,

    //水果机独立数据
    queryTypeIndependentFruit,

    //留存 通过爱贝查询
    queryTypeRechargeByAibei,
    //机器人最高积分
    queryTypeRobotMaxScore,
    //已停封玩家ID列表
    queryTypeServiceUnBlockIdList,

    //弹头统计
    queryTypeFishlordBulletHeadStat,
    //弹头产出剩余统计
    queryTypeFishlordBulletHeadOutput,

    //弹头查询
    queryTypeFishlordBulletHeadQuery,

    //鱼雷产出详情
    queryTypeFishlordBulletHeadList,

    //弹头话费查询
    queryTypeTorpedoTicket,

    //极光推送
    queryTypePolarLightsPush,

    //世界杯大竞猜赛事表
    queryTypeWorldCupMatch,

    //世界杯大竞猜玩家押注统计
    queryTypeWorldCupMatchPlayerJoin,

    //万圣节活动
    //排行榜
    queryTypeHallowmasActRank,
    //领取奖励人数
    queryTypeHallowmasActRecvCount,

    //中秋特惠活动
    queryTypeJinQiuRechargeLottery,
    //国庆节活动
    //领取奖励
    queryTypeNdActRecvCount,

    //活动排行榜
    queryTypeNdActRankList,

    //小游戏开关设置
    queryTypeChannelOpenCloseGame,
    //新七日
    queryTypeNewSevenDay,
    //新手任务
    queryTypeNewPlayerTask,
    //签到分布情况
    queryTypeSignByMonth,
    //每日签到
    queryTypeDailySign,
    //每日签到领取奖励
    queryTypeDailySignReward,

    //每日任务
    queryTypeDailyTask,

    //玩家聊天记录查询
    queryTypePlayerChatQuery,
    //玩家自定义头像统计
    queryTypePlayerIconCustomStat,

    //爆金排行榜详细
    queryTypeFishlordBaojinRankDetail,
    //爆金比赛场
    queryTypeFishlordBaojinStat,

    //东海龙宫消耗统计
    queryTypeFishlordDragonPalaceConsumeStat,

    //东海龙宫玩家分布统计
    queryTypeFishlordDragonPalacePlayerStat,

    //诛龙箭统计
    queryTypeFishlordDragonPalaceKillDragon,

    //竞技场消耗统计
    queryTypeFishlordJingjiConsumeStat,

    //竞技场产出统计
    queryTypeFishlordJingjiOutlayStat,

    //竞技场任务统计
    queryTypeFishlordJingjiTaskStat,

    //竞技场玩家分布统计
    queryTypeFishlordJingjiPlayerStat,

    //爆金排行榜(当前)
    queryTypeFishlordBaojinRank,
    //爆金场排行榜
    queryTypeFishlordBaojinHistoryRank,
    //集玩偶发放次数
    queryTypePuppetActStat,
    //玩家捐赠档位 服务器档位奖励领取
    queryTypePuppetRewardRecv,
    //服务器总捐赠玩偶
    queryTypePuppetSvrDonate,
    //20个捐赠玩家排行榜
    queryTypePuppetPlayerDonateRank,

    //20个累计获得玩偶玩家排行榜
    queryTypePuppetPlayerGainRank,

    //拉霸玩家抽奖次数统计
    queryTypeLabaLotteryStat,
    //拉霸玩家抽奖记录查询
    queryTypeLabaLotteryQuery,
    //拉霸抽奖档位统计
    queryTypeLabaLotteryProb,

    //拉霸活动统计
    queryTypeLabaActivityStat,

    //万炮盛典活动
    queryTypeWpActivityStat,
    queryTypeWpActivityPlayerStat,

    //捕鱼盛宴活动
    queryTypeFishlordFeastStat,

    //限时操作
    queryTypeActivityPanicBuyingCfg,

    //错误
    queryTypeIncomeExpensesError,

    //节日活动(加)
    queryTypeFestivalActivity,

    //七日活动
    queryTypeSevenDayActivity,

    //材料礼包每日购买
    queryTypeMaterialGiftRecharge,

    // GM账号
    queryTypeGmAccount,

    // 金币，钻石变化
    queryTypeMoney,
    // 详细
    queryTypeMoneyDetail,

    // 客服信息查询
    queryTypeServiceInfo,

    // 邮件
    queryTypeMail,

    // 充值记录
    queryTypeRecharge,

    // 充值记录(新)
    queryTypeRechargeNew,

    // 账号查询
    queryTypeAccount,

    // 登陆历史
    queryTypeLoginHistory,

    // 礼包查询
    queryTypeGift,

    // 礼包码查询
    queryTypeGiftCode,

    // 兑换
    queryTypeExchange,

    //实物审核管理
    queryTypeExchangeAudit,

    // 大厅通用数据
    queryTypeLobby,

    // 服务器收益
    queryTypeServerEarnings,

    // 捕鱼独立数据
    queryTypeIndependentFishlord,

    // 鳄鱼独立数据
    queryTypeIndependentCrocodile,
    
    //奔驰宝马独立数据
    queryTypeIndependentBz,

    // 骰宝独立数据
    queryTypeIndependentDice,

    // 牛牛独立数据
    queryTypeIndependentCows,

    // 骰宝盈利率
    queryTypeDiceEarnings,

    // 百家乐盈利率
    queryTypeBaccaratEarnings,

    // 百家乐上庄情况
    queryTypeBaccaratPlayerBanker,

    // 牛牛上庄情况
    queryTypeCowsPlayerBanker,

    //牛牛牌局查询
    queryTypeCowsCardsQuery,

    //牛牛牌局查询下注玩家列表
    queryTypeCowsCardsPlayerList,

    //牛牛牌局详情
    queryTypeCowsCardsDetail,

    //牛牛杀分放分LOG记录列表
    queryTypeCowsCardCtrlList,

    // 当前公告
    queryTypeCurNotice,

    // 捕鱼参数查询
    queryTypeFishlordParam,
    
    //大水池参数查询
    queryTypeFishlordNewParam,

    //大水池场次参数查询
    queryTypeFishlordRoomNewParam,

    //个人后台管理
    queryTypeFishlordNewSingleParam,

    //捕鱼爆金比赛场参数调整
    queryTypeFishlordBaojinParam,

    //捕鱼竞技场玩家得分修改
    queryTypeFishlordBaojinScoreParam,

    // 鳄鱼公园参数查询
    queryTypeFishParkParam,

    // 捕鱼桌子参数查询
    queryTypeFishlordDeskParam,
    // 鳄鱼公园桌子参数查询
    queryTypeFishParkDeskParam,

    // 鳄鱼大亨参数查询
    queryTypeCrocodileParam,

    //鳄鱼大亨黑白名单列表
    queryTypeCrocodileSpecilList,

    //奔驰宝马参数查询
    queryTypeBzParam,

    //奔驰宝马黑白名单列表
    queryTypeBzSpecilList,

    // 牛牛参数查询
    queryTypeQueryCowsParam,

    //牛牛黑白名单列表
    queryTypeCowCardsSpecilList,

    // 五龙参数查询，每个房间的系统总收入，总支出，盈利率..
    queryTypeDragonParam,

    // 五龙各游戏模式下的参数查询
    queryTypeDragonGameModeEarning,

    // 黑红梅方参数查询
    queryTypeShcdParam,
    // 黑红梅方独立数据
    queryTypeIndependentShcd,
    //黑红梅方牌局查询
    queryTypeShcdCardsQuery,

    //黑红梅方牌局详情
    queryTypeShcdCardsDetail,

    //黑红梅方下注玩家列表
    queryTypeShcdCardsPlayerList,

    //黑红梅方黑白名单列表
    queryTypeShcdCardsSpecilList,

    //黑红梅方杀分放分LOG记录列表
    queryTypeShcdCardsCtrlList,

    //水浒传总盈利率查看
    queryTypeShuihzTotalEarning,

    //水浒传每日盈利率查看
    queryTypeShuihzDailyEarning,

    //水浒传单个盈利率参考
    queryTypeShuihzSingleEarning,

    //水浒传每日达上下限人数统计
    queryTypeShuihzReachLimit,

    //水浒传每日游戏情况查看
    queryTypeShuihzDailyState,

    // 查询套牛游戏相关
    queryTypeGameCalfRoping,

    // 鱼的情况统计
    queryTypeFishStat,
    // 鳄鱼公园鱼的情况统计
    queryTypeFishParkStat,

    // 货币最多的玩家
    queryTypeMoneyAtMost,

    // 旧的盈利率
    queryTypeOldEaringsRate,

    // 经典捕鱼阶段分析
    queryTypeFishlordStage,
    // 鳄鱼公园阶段分析
    queryTypeFishParkStage,

    // 当前在线
    queryTypeOnlinePlayerCount,

    // 操作日志
    queryTypeOpLog,

    // 查询玩家头像
    queryTypePlayerHead,

    // 消耗总计
    queryTypeTotalConsume,

    // 各游戏收入
    queryTypeGameRecharge,

    // 金币增长排行
    queryTypeCoinGrowthRank,

    // 流失查询
    queryTypeAccountCoinLessValue,

    // 捕鱼消耗
    queryTypeFishConsume,

    // 牛牛牌型查询
    queryTypeCowsCardsType,

    // 游戏结果控制查询
    queryTypeGameResultControl,

    // 头像举报
    queryTypeInformHead,

    // 查询td活跃
    queryTypeTdActivation,
    // LTV价值
    queryTypeLTV,

    // 查询最高在线
    queryTypeMaxOnline,

    // 玩家金币总和
    queryTypeTotalPlayerMoney,

    // 大奖赛相关查询
    queryTypeGrandPrix,

    // boss统计
    queryTypeFishBoss,

    // 兑换统计
    queryTypeExchangeStat,

    // 付费点
    queryTypeRechargePointStat,

    // 星星抽奖
    queryTypeStarLottery,

    //转盘抽奖
    queryTypeDialLottery,

    //话费鱼统计
    queryTypePumpChipFishStat,

    queryTypeRLose,
    // 每日龙珠
    queryTypeDragonBallDaily,
    // 玩家充值监控
    queryTypeRechargePlayerMonitor,

    // 每小时付费
    queryTypeRechargePerHour,
    // 每小时在线人数
    queryTypeOnlinePlayerNumPerHour,

    //每小时在线人数新
    queryTypeOnlinePlayerNumPerHourNew,

    // 平均游戏时长分布
    queryTypeGameTimeDistribution,
    // 用户喜好-平均在线时间
    queryTypeGameTimePlayerFavor,
    // 首付游戏时长分布
    queryTypeFirstRechargeGameTimeDistribution,
    // 首次购买计费点分布
    queryTypeFirstRechargePointDistribution,
    // 用户下注情况
    queryTypePlayerGameBet,
    // 查询玩家收支统计
    queryTypePlayerIncomeExpenses,

    // 新增用户分析
    queryTypeNewPlayer,

    // 渠道100003的活动数据
    queryTypeQueryChannel100003ActCount,
}

