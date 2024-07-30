﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Linq;
using System.Web.UI.WebControls;

// 查询管理
public class QueryMgr : SysBase
{
    private Dictionary<QueryType, QueryBase> m_items = new Dictionary<QueryType, QueryBase>();

    public QueryMgr()
    {
        m_sysType = SysType.sysTypeQuery;
    }

    // 作查询
    public OpRes doQuery(object param, QueryType queryType, GMUser user)
    {
        if (!m_items.ContainsKey(queryType))
        {
            LOGW.Info("不存在名称为[{0}]的查询", queryType);
            return OpRes.op_res_failed;
        }
        return m_items[queryType].doQuery(param, user);
    }

    // 返回查询结果
    public object getQueryResult(QueryType queryType)
    {
        if (!m_items.ContainsKey(queryType))
        {
            LOGW.Info("不存在名称为[{0}]的查询", queryType);
            return null;
        }
        return m_items[queryType].getQueryResult();
    }

    public object getQueryResult(object param, QueryType queryType, GMUser user)
    {
        if (!m_items.ContainsKey(queryType))
        {
            LOGW.Info("不存在名称为[{0}]的查询", queryType);
            return null;
        }
        return m_items[queryType].getQueryResult(param, user);
    }

    // 构成查询条件
    public OpRes makeQuery(object param, QueryType queryType, GMUser user, QueryCondition imq)
    {
        if (!m_items.ContainsKey(queryType))
        {
            return OpRes.op_res_failed;
        }
        return m_items[queryType].makeQuery(param, user, imq);
    }

    public T getQuery<T>(QueryType queryType) where T : QueryBase
    {
        if (m_items.ContainsKey(queryType))
        {
            return (T)m_items[queryType];
        }
        return default(T);
    }

    public override void initSys()
    {
        m_items.Add(QueryType.queryTypeIncomeExpensesError, new QueryIncomeExpensesError());  //收支错误
        m_items.Add(QueryType.queryTypeFestivalActivity,new QueryFestivalActivity());   //节日活动
        m_items.Add(QueryType.queryTypeGmAccount, new QueryGMAccount());
        m_items.Add(QueryType.queryTypeMoney, new QueryPlayerMoney());
        m_items.Add(QueryType.queryTypeMoneyDetail, new QueryPlayerMoneyDetail()); //玩家金币变化详细

        m_items.Add(QueryType.queryTypeMail, new QueryMail());
        m_items.Add(QueryType.queryTypeServiceInfo, new QueryServiceInfo());
        m_items.Add(QueryType.queryTypeRecharge, new QueryRecharge());
        m_items.Add(QueryType.queryTypeRechargeNew, new QueryRechargeNew());  //充值查询

        m_items.Add(QueryType.queryTypeAccount, new QueryAccount());//账号查询
        m_items.Add(QueryType.queryTypeAccountBeibao, new QueryTypeAccountBeibao());//账号查询背包查询
        m_items.Add(QueryType.queryTypeLoginHistory, new QueryLogin());
        m_items.Add(QueryType.queryTypeGift, new QueryGift());
        m_items.Add(QueryType.queryTypeGiftCode, new QueryGiftCode());
        m_items.Add(QueryType.queryTypeExchange, new QueryExchange());  //兑换管理

        m_items.Add(QueryType.queryTypeLobby, new QueryLobby());
        //m_items.Add(QueryType.queryTypeServerEarnings, new QueryServerEarnings()); //游戏金币流动统计
        m_items.Add(QueryType.queryTypeServerEarnings, new QueryServerEarningsNew()); //游戏金币流动统计(新)
        m_items.Add(QueryType.queryTypeIndependentFishlord, new QueryIndependentFishlord());
        m_items.Add(QueryType.queryTypeIndependentCrocodile, new QueryIndependentCrocodile());
        m_items.Add(QueryType.queryTypeIndependentDice, new QueryIndependentDice());

        m_items.Add(QueryType.queryTypeIndependentBz,new QueryIndependentBz());//奔驰宝马独立数据
        m_items.Add(QueryType.queryTypeBzParam,new QueryBzParam()); //奔驰宝马参数调整

        m_items.Add(QueryType.queryTypeCurNotice, new QueryCurNotice());//运营公告列表
        m_items.Add(QueryType.queryTypeNoticeInfo,new QueryNoticeInfo());//运营公告编辑
        m_items.Add(QueryType.queryTypeFishlordParam, new QueryFishlordParam());   //经典捕鱼参数调准
        m_items.Add(QueryType.queryTypeFishlordNewParam, new QueryFishlordNewParam());//大水池参数调整（新）
        m_items.Add(QueryType.queryTypeFishlordRoomNewParam, new QueryFishlordRoomNewParam());//大水池场次参数
        m_items.Add(QueryType.queryTypeFishlordNewSingleParam, new QueryFishlordNewSingleParam());//个人后台管理参数

        m_items.Add(QueryType.queryTypeFishParkParam, new QueryFishParkParam());
        m_items.Add(QueryType.queryTypeFishlordBaojinParam,new QueryFishlordBaojinParam());//爆金比赛场参数调整
        m_items.Add(QueryType.queryTypeFishlordBaojinScoreParam, new QueryFishlordBaojinScoreParam());//竞技场得分修改

        m_items.Add(QueryType.queryTypeCrocodileParam, new QueryCrocodileParam());
        m_items.Add(QueryType.queryTypeFishStat, new QueryFish());
        m_items.Add(QueryType.queryTypeFishParkStat, new QueryFishParkStat());
        
        m_items.Add(QueryType.queryTypeMoneyAtMost, new QueryMoneyAtMost());

        m_items.Add(QueryType.queryTypeOldEaringsRate, new QueryOldEarningRate());
        m_items.Add(QueryType.queryTypeFishlordStage, new QueryFishlordStage());
        m_items.Add(QueryType.queryTypeFishParkStage, new QueryFishParkStage());

        m_items.Add(QueryType.queryTypeDiceEarnings, new QueryDiceEarningsParam());
        m_items.Add(QueryType.queryTypeOnlinePlayerCount, new QueryOnlinePlayerCount());
        m_items.Add(QueryType.queryTypeOpLog, new QueryOpLog());

        m_items.Add(QueryType.queryTypePlayerHead, new QueryPlayerHead());
        m_items.Add(QueryType.queryTypeTotalConsume, new QueryTotalConsume()); 
        m_items.Add(QueryType.queryTypeGameRecharge, new QueryGameRechargeByDay());
        m_items.Add(QueryType.queryTypeBaccaratEarnings, new QueryBaccaratEarningsParam());
        m_items.Add(QueryType.queryTypeCoinGrowthRank, new QueryCoinGrowthRank());

        m_items.Add(QueryType.queryTypePlayerRichesRank,new QueryPlayerRichesRank()); //玩家财富榜

        m_items.Add(QueryType.queryTypeFishlordDeskParam, new QueryFishlordDeskParam());
        m_items.Add(QueryType.queryTypeFishParkDeskParam, new QueryFishParkDeskParam());

        m_items.Add(QueryType.queryTypeAccountCoinLessValue, new QueryAccountCoinLessValue());
        m_items.Add(QueryType.queryTypeFishConsume, new QueryFishConsume());//捕鱼消耗统计

        m_items.Add(QueryType.queryTypeBaccaratPlayerBanker, new QueryBaccaratPlayerBanker());
        m_items.Add(QueryType.queryTypeCowsPlayerBanker, new QueryCowsPlayerBanker());
        m_items.Add(QueryType.queryTypeIndependentCows, new QueryIndependentCows());
        m_items.Add(QueryType.queryTypeQueryCowsParam, new QueryCowsParam());
        m_items.Add(QueryType.queryTypeCowsCardsType, new QueryCowsCardsType());
        m_items.Add(QueryType.queryTypeGameResultControl, new QueryGameResultControl());
        m_items.Add(QueryType.queryTypeCowsCardsQuery,new QueryCowsCardsQuery()); //牛牛牌局查询
        m_items.Add(QueryType.queryTypeCowsCardsPlayerList,new QueryCowsCardsPlayerList()); //牛牛下注玩家列表
        m_items.Add(QueryType.queryTypeCowsCardsDetail,new QueryCowsCardsDetail()); //牛牛牌局详情

        m_items.Add(QueryType.queryTypeShuihzTotalEarning, new QueryShuihzTotalEarning());  //水浒传总盈利率
        m_items.Add(QueryType.queryTypeShuihzDailyEarning, new QueryShuihzDailyEarning()); //水浒传每日盈利率
        m_items.Add(QueryType.queryTypeShuihzSingleEarning, new QueryShuihzSingleEarning()); //水浒传每日盈利率
        m_items.Add(QueryType.queryTypeShuihzDailyState, new QueryShuihzDailyState()); //水浒传每日游戏情况查看
        m_items.Add(QueryType.queryTypeShuihzReachLimit, new QueryShuihzReachLimit()); //水浒传每日达上下限人数统计

        m_items.Add(QueryType.queryTypeDragonParam, new QueryDragonParam());
        m_items.Add(QueryType.queryTypeDragonGameModeEarning, new QueryDragonGameModeEarning());
        m_items.Add(QueryType.queryTypeShcdParam, new QueryShcdParam());  //参数调整
        m_items.Add(QueryType.queryTypeShcdCardsQuery, new QueryShcdCardsQuery());//黑红牌局查询
        m_items.Add(QueryType.queryTypeShcdCardsPlayerList,new QueryShcdCardsPlayerList()); //黑红下注玩家列表
        m_items.Add(QueryType.queryTypeShcdCardsDetail, new QueryShcdCardsResultDetail()); //黑红开牌结果详细
        m_items.Add(QueryType.queryTypeIndependentShcd, new QueryIndependentShcd());
        m_items.Add(QueryType.queryTypeGameCalfRoping, new QueryGameCalfRoping());
        
        m_items.Add(QueryType.queryTypeInformHead, new QueryInformHead());

        m_items.Add(QueryType.queryTypeTdActivation, new QueryTdActivation());
        m_items.Add(QueryType.queryTypeLTV, new QueryTdLTV());

        m_items.Add(QueryType.queryTypeTdNewPlayerMonitor, new QueryTdNewPlayerMonitor());//新进玩家付费监控

        m_items.Add(QueryType.queryTypeStatNewPlayerOpenRate, new QueryStatNewPlayerOpenRate());//新手炮倍完成率

        m_items.Add(QueryType.queryTypeMaxOnline, new QueryMaxOnline());
        m_items.Add(QueryType.queryTypeTotalPlayerMoney, new QueryTotalPlayerMoney());

        m_items.Add(QueryType.queryTypeGrandPrix, new QueryGrandPrix());
        m_items.Add(QueryType.queryTypeFishBoss, new QueryFishBoss());
        m_items.Add(QueryType.queryTypeExchangeStat, new QueryExchangeStat());
        m_items.Add(QueryType.queryTypeRechargePointStat, new QueryRechargePointStat());  //付费点统计

        m_items.Add(QueryType.queryTypeSevenDayActivity, new QuerySevenDayActivityStat());//七日活动

        m_items.Add(QueryType.queryTypeWpActivityStat,new QueryWpActivityStat()); //万炮盛典活动
        m_items.Add(QueryType.queryTypeWpActivityPlayerStat, new QueryWpActivityPlayerStat()); //万炮盛典首位达炮玩家

        m_items.Add(QueryType.queryTypeFishlordFeastStat,new QueryFishlordFeastStat());//捕鱼盛宴活动

        m_items.Add(QueryType.queryTypeMaterialGiftRecharge, new QueryMaterialGiftRechargeStat());//材料礼包每日购买
        m_items.Add(QueryType.queryTypePumpChipFishStat,new QueryPumpChipFishStat());//话费鱼
        m_items.Add(QueryType.queryTypeStarLottery, new QueryStarLottery());  //星星抽奖
        m_items.Add(QueryType.queryTypeDialLottery,new QueryDialLottery()); //转盘抽奖
        m_items.Add(QueryType.queryTypeActivityPanicBuyingCfg,new QueryActivityPanicBuyingCfg()); //限时活动
        m_items.Add(QueryType.queryTypeLabaLotteryProb,new QueryLabaLotteryProb());//拉霸抽奖档位
        m_items.Add(QueryType.queryTypeLabaLotteryQuery,new QueryPlayerLabaLotteryRecord());//拉霸玩家抽奖记录查询
        m_items.Add(QueryType.queryTypeLabaLotteryStat,new QueryPlayerLabaLotteryCount());//拉霸玩家抽奖次数统计
        m_items.Add(QueryType.queryTypeLabaActivityStat,new QueryLabaActivityStat()); //拉霸活动统计

        m_items.Add(QueryType.queryTypePuppetActStat,new QueryPuppetActStat()); //集玩偶发放玩偶数量
        m_items.Add(QueryType.queryTypePuppetRewardRecv,new QueryPuppetRewardRecvStat());//玩家捐赠档位
        m_items.Add(QueryType.queryTypePuppetSvrDonate,new QueryPuppetSvrDonateStat()); //服务器总捐赠玩偶次数数量
        m_items.Add(QueryType.queryTypePuppetPlayerDonateRank,new QueryPuppetPlayerDonateRankStat()); //20个捐赠玩家排行榜
        m_items.Add(QueryType.queryTypePuppetPlayerGainRank,new QueryPuppetPlayerGainRankStat());//20个累计获得玩偶玩家排行榜

        m_items.Add(QueryType.queryTypeFishlordBaojinStat, new QueryFishlordBaojinStat());//爆金比赛场统计
        m_items.Add(QueryType.queryTypeFishlordBaojinRank, new QueryFishlordBaojinRank()); //爆金比赛排行榜
        m_items.Add(QueryType.queryTypeFishlordBaojinRankDetail,new QueryFishlordBaojinRankDetail());//爆金比赛场排行榜详情

        m_items.Add(QueryType.queryTypeRLose, new QueryRLose());

        m_items.Add(QueryType.queryTypeDragonBallDaily, new QueryDragonBallDaily());
        m_items.Add(QueryType.queryTypeRechargePlayerMonitor, new QueryRechargePlayerMonitor()); //新进玩家付费监控
        m_items.Add(QueryType.queryTypeRechargePerHour, new QueryRechargePerHour());

        m_items.Add(QueryType.queryTypeOnlinePlayerNumPerHour, new QueryOnlinePlayerNumPerHour());
        m_items.Add(QueryType.queryTypeOnlinePlayerNumPerHourNew,new QueryOnlinePlayerNumPerHourNew());//实时在线折线图新
        m_items.Add(QueryType.queryTypeGameTimeDistribution, new QueryGameTimeDistribution());
        m_items.Add(QueryType.queryTypeGameTimePlayerFavor, new QueryGameTimePlayerFavor());
        m_items.Add(QueryType.queryTypeFirstRechargeGameTimeDistribution, new QueryFirstRechargeGameTimeDistribution());
        m_items.Add(QueryType.queryTypeFirstRechargePointDistribution, new QueryFirstRechargePointDistribution());

        m_items.Add(QueryType.queryTypePlayerGameBet, new QueryPlayerGameBet());
        m_items.Add(QueryType.queryTypePlayerIncomeExpenses, new QueryPlayerIncomeExpenses());

        m_items.Add(QueryType.queryTypeNewPlayer, new QueryNewPlayer());

        m_items.Add(QueryType.queryTypePlayerIconCustomStat,new QueryPlayerIncomCustom());//玩家自定义头像

        m_items.Add(QueryType.queryTypeShcdCardsSpecilList,new QueryPlayerShcdCardsSpecilList()); //黑红黑白名单列表
        m_items.Add(QueryType.queryTypeShcdCardsCtrlList,new QueryShcdCardsCtrlList());//黑红杀分放分LOG记录列表
        m_items.Add(QueryType.queryTypeCowCardsSpecilList,new QueryPlayerCowCardsSpecilList()); //牛牛黑白名单列表
        m_items.Add(QueryType.queryTypeCowsCardCtrlList,new QueryCowsCardCtrlList());//牛牛杀分放分LOG记录列表

        m_items.Add(QueryType.queryTypeCrocodileSpecilList,new QueryCrocodileSpecilList());//鳄鱼大亨黑白名单列表设置
        m_items.Add(QueryType.queryTypeBzSpecilList,new QueryBzSpecilList());//奔驰宝马黑白名单列表

        m_items.Add(QueryType.queryTypePlayerChatQuery,new QueryPlayerChat());//玩家聊天记录查询

        m_items.Add(QueryType.queryTypeDailySign,new QueryDailySign()); //每日签到
        m_items.Add(QueryType.queryTypeSignByMonth,new QuerySignByMonth());//签到情况分布
        m_items.Add(QueryType.queryTypeDailySignReward, new QueryDailySignReward());//签到领取奖励

        m_items.Add(QueryType.queryTypeNewPlayerTask,new QueryNewPlayerTask()); //新手任务
        m_items.Add(QueryType.queryTypeNewSevenDay,new QueryNewSevenDay()); //新七日
        m_items.Add(QueryType.queryTypeDailyTask,new QueryDailyTask());//每日任务

        m_items.Add(QueryType.queryTypeChannelOpenCloseGame,new QueryChannelOpenCloseGame());//小游戏开关设置

        m_items.Add(QueryType.queryTypeNdActRecvCount,new QueryNdActRecvCount());//国庆节领取奖励人数
        m_items.Add(QueryType.queryTypeNdActRankList,new QueryNdActRankList());//排行榜

        m_items.Add(QueryType.queryTypeJinQiuRechargeLottery,new QueryJinQiuRechargeLottery());//中秋特惠活动

        m_items.Add(QueryType.queryTypeHallowmasActRank,new QueryHallowmasActRank()); //万圣节排行榜
        m_items.Add(QueryType.queryTypeHallowmasActRecvCount,new QueryHallowmasActRecvCount());//万圣节活动奖励领取人数

        m_items.Add(QueryType.queryTypePolarLightsPush,new QueryPolarLightsPush()); //极光推送

        m_items.Add(QueryType.queryTypeFishlordBulletHeadStat,new QueryFishlordBulletHeadStat()); //捕鱼弹头统计
        m_items.Add(QueryType.queryTypeFishlordBulletHeadQuery,new QueryFishlordBulletHeadQuery());//弹头查询
        m_items.Add(QueryType.queryTypeFishlordBulletHeadOutput, new QueryStatFishlordBulletHeadOutput());//捕鱼弹头产出统计


        m_items.Add(QueryType.queryTypeServiceUnBlockIdList,new QueryServiceUnBlockIdList());//已停封玩家ID列表
        m_items.Add(QueryType.queryTypeRobotMaxScore,new QueryRobotMaxScore());//机器人最高积分

        m_items.Add(QueryType.queryTypeRechargeByAibei,new QueryRechargeByAiBei()); //爱贝充值查询

        m_items.Add(QueryType.queryTypeFruitParamControl, new QueryFruitParamControl());//水果机参数调整 
        m_items.Add(QueryType.queryTypeFruitSpecilList, new QueryFruitSpecilList());//水果机黑白名单设置
        m_items.Add(QueryType.queryTypeIndependentFruit, new QueryIndependentFruit());//水果机独立数据

        m_items.Add(QueryType.queryTypeFishlordJingjiConsumeStat,new QueryFishlordJingjiConsume());//竞技场消耗统计
        m_items.Add(QueryType.queryTypeFishlordJingjiOutlayStat,new QueryFishlordJingjiOutlay());//竞技场产出统计
        m_items.Add(QueryType.queryTypeFishlordJingjiTaskStat,new QueryFishlordJingjiTask());//竞技场任务统计
        m_items.Add(QueryType.queryTypeFishlordJingjiPlayerStat,new QueryFishlordJingjiPlayer());//竞技场玩家分布统计

        m_items.Add(QueryType.queryTypeFishlordDragonPalaceConsumeStat,new QueryFishlordDragonPalaceConsume());//东海龙宫场消耗统计
        m_items.Add(QueryType.queryTypeFishlordDragonPalacePlayerStat,new QueryFishlordDragonPalacePlayer());//东海龙宫玩家分布统计
        m_items.Add(QueryType.queryTypeFishlordDragonPalaceKillDragon, new QueryFishlordDragonPalaceKill());//龙宫场诛龙箭统计

        m_items.Add(QueryType.queryForceUpdateReward,new QueryForceUpdateReward());//强更补偿查询

        m_items.Add(QueryType.queryTypeChristmasOrYuandan,new QueryTypeChristmasOrYuandan());//圣诞节/元旦活动

        m_items.Add(QueryType.queryTypeMailQuery,new QueryTypeMailQuery());//邮件查询

        m_items.Add(QueryType.queryTypePlayerItemRecord,new QueryTypePlayerItemRecord());//玩家道具详情

        m_items.Add(QueryType.queryTypeWord2LogicItemError,new QueryTypeWord2LogicItemError());//添加同步道具失败

        m_items.Add(QueryType.queryTypeScratchActEarn,new QueryTypeScratchActEarn());//刮刮乐活动运营
        m_items.Add(QueryType.queryTypeScratchActExchangeRes,new QueryTypeScratchActExchangeRes());//刮刮乐活动兑换

        m_items.Add(QueryType.queryTypeStatNYGift,new QueryTypeStatNYGift());//春节礼包
        m_items.Add(QueryType.queryTypeStatNYAccRecharge,new QueryTypeStatNYAccRecharge());//新春重返
        m_items.Add(QueryType.queryTypeStatNYAdventure,new QueryTypeStatNYAdventure());//勇者大冒险

        m_items.Add(QueryType.queryTypeGetPlayerIdByOrderId,new QueryTypeGetPlayerIdByOrderId());//通过第三方订单号查询玩家ID

        m_items.Add(QueryType.queryTypeGiftCodeNewList,new QueryTypeGiftCodeNewList());//礼包码cdKey生成器列表

        m_items.Add(QueryType.queryTypeBulletHeadRank,new QueryTypeBulletHeadRank()); //欢乐炸炸炸  //历史排行
        m_items.Add(QueryType.queryTypeBulletHeadCurrRank,new QueryTypeBulletHeadCurrRank());  //欢乐炸炸炸 当前排行
        m_items.Add(QueryType.queryTypeBulletHeadPlayerScore, new QueryTypeBulletHeadPlayerScore());//炸弹乐园玩家积分

        m_items.Add(QueryType.queryTypeDragonScaleRank, new QueryTypeDragonScaleRank());//龙鳞排行 //历史排行
        m_items.Add(QueryType.queryTypeDragonScaleCurrRank, new QueryTypeDragonScaleCurrRank());//龙鳞排行 //当前排行
        m_items.Add(QueryType.queryTypeDragonScaleControl,new QueryTypeDragonScaleControl());//玩家龙鳞数量信息

        m_items.Add(QueryType.queryTypeWuyiRewardResult,new QueryTypeWuyiRewardResult());//五一充返活动
        m_items.Add(QueryType.queryTypePumpNewGuide,new QueryTypePumpNewGuide());//新手引导埋点统计
        m_items.Add(QueryType.queryTypeSpittorSnatchActRank,new QuerySpittorSnatchActRank());//金蟾夺宝活动排行榜
        m_items.Add(QueryType.queryTypeSpittorSnatchActRewardList,new QuerySpittorSnatchActRewardList());//金蟾夺宝领取奖励人数统计

        m_items.Add(QueryType.queryTypeWorldCupMatch,new QueryWorldCupMatchList());//世界杯大竞猜赛事表
        m_items.Add(QueryType.queryTypeWorldCupMatchPlayerJoin,new QueryWorldCupMatchPlayerJoin());//世界杯大竞猜玩家押注统计

        m_items.Add(QueryType.queryTypeStatPanicBox,new QueryStatPanicBox());//活动幸运宝箱
        m_items.Add(QueryType.queryTypeStatPanicBoxDetail,new QueryStatPanicBoxDetail()); //活动幸运宝箱详情

        m_items.Add(QueryType.queryTypeRepairOrder,new QueryRepairOrder());//客服补单/大户随访/换包福利-系统
        m_items.Add(QueryType.queryTypeSelectLostPlayers,new QuerySelectLostPlayers());//流失大户筛选
        m_items.Add(QueryType.queryTypeGuideLostPlayersRes,new QueryGuideLostPlayersRes());//流失大户引导记录效果

        m_items.Add(QueryType.queryTypeStatTurnTableFishRank, new QueryStatTurnTableFishRank());//转盘鱼排行
        m_items.Add(QueryType.queryTypeStatTFish, new QueryStatTurnTableFish());//转盘鱼捕鱼统计
        m_items.Add(QueryType.queryTypeStatTFishDetail, new QueryStatTurnTableFishDetail());//转盘鱼捕鱼统计详情
        m_items.Add(QueryType.queryTypeStatTFishRoom, new QueryStatTurnTableFishRoom());//转盘鱼场次统计

        m_items.Add(QueryType.queryTypeJinQiuNationalDayActRank,new QueryJinQiuNationalDayActRankStat()); //国庆中秋快乐排行榜统计
        m_items.Add(QueryType.queryTypeJinQiuNationalDayActLottery, new QueryJinQiuNationalDayActLotteryStat());//国庆中秋快乐抽奖统计
        m_items.Add(QueryType.queryTypeJinQiuNationalDayActLotteryDetail,new QueryJinQiuNationalDayActLotteryDetail());//国庆中秋快乐抽奖统计详情
        m_items.Add(QueryType.queryTypeJinQiuNationalDayActCtrl,new QueryJinQiuNationalDayActCtrl());//国庆中秋快乐玩家月饼量
        m_items.Add(QueryType.queryTypeJinQiuNationDayActRoomStat, new QueryJinQiuNationalDayActRoomStat());//场次统计
        m_items.Add(QueryType.queryTypeJinQiuNationalDayActTaskStat, new QueryJinQiuNationalDayActTaskStat());//任务统计
        m_items.Add(QueryType.queryTypeJinQiuNationalDayActTaskDetail, new QueryJinQiuNationalDayActTaskDeatil());//任务统计详情

        m_items.Add(QueryType.queryTypeGameRealTimeLoseWinRank,new QueryGameRealTimeLoseWinRank());//小游戏实时输赢
        m_items.Add(QueryType.queryTypeGameDailyTotalLoseWinRank,new QueryGameDailyTotalLoseWinRank());//小游戏日累计输赢
        m_items.Add(QueryType.queryTypeStatPlayerMoneyRep,new QueryStatPlayerMoneyRep());//库存数据统计

        m_items.Add(QueryType.queryTypeStatPlayerBw, new QueryStatPlayerBw()); //比武场数据统计
        m_items.Add(QueryType.queryTypeWechatRecvRewardStat,new QueryStatWechatRecvReward());//微信公众号签到统计

        m_items.Add(QueryType.queryTypeWjlwRechargeReward, new QueryStatWjlwRechaargeReward());//围剿龙王
        m_items.Add(QueryType.queryTypeWjlwGoldEarn, new QueryStatWjlwGoldEarn());//围剿龙王金币玩法统计
        m_items.Add(QueryType.queryTypeWjlwGoldEarnTurnInfo, new QueryStatWjlwGoldEarnTurnInfo());//围剿龙王金币玩法统计每局详情
        m_items.Add(QueryType.queryTypeWjlwGoldBetPlayerList, new QueryStatWjlwGoldBetPlayerList());//围剿龙王金币玩法每局下注玩家列表
        m_items.Add(QueryType.queryTypeWjlwRechargeEarn, new QueryStatWjlwRechargeEarn());//围剿龙王付费玩法统计
        m_items.Add(QueryType.queryTypeWjlwRechargeWinInfo, new QueryStatWjlwRechargeWinInfo());//围剿龙王付费玩法获奖详情
        m_items.Add(QueryType.queryTypeWjlwRechargeBetPlayer, new QueryStatWjlwRechargeBetPlayer());//围剿龙王付费玩法下注详情

        m_items.Add(QueryType.queryTypeStatBulletHeadGift, new QueryStatBulletHeadGift());//弹头礼包统计
        m_items.Add(QueryType.queryTypeStatBulletHeadGiftPlayer, new QueryStatBulletHeadGiftPlayer());//弹头礼包统计详情

        m_items.Add(QueryType.queryTypeStatKdActRank, new QueryStatKdActRank());//屠龙榜
        m_items.Add(QueryType.queryTypeStatKdActDayRank, new QueryStatKdActDayRank());//屠龙榜日榜

        m_items.Add(QueryType.queryTypeStatPlayerBasicInfo, new QueryStatPlayerBasicInfo());//玩家基本信息

        m_items.Add(QueryType.queryTypeStatFishingRoomInfo, new QueryStatFishingRoomInfo());//渔场场次情况

        m_items.Add(QueryType.queryTypeOpnewGameIncome, new QueryOpnewGameIncome());//收益数据
        m_items.Add(QueryType.queryTypeOpnewGameActive, new QueryOpnewGameActive());//活跃数据

        m_items.Add(QueryType.queryTypeOpnewPlayerRecharge, new QueryOpnewPlayerRecharge());//玩家充值信息
        m_items.Add(QueryType.queryTypeOpnewTurretTimes, new QueryOpnewTurretTimes());//炮倍相关

        m_items.Add(QueryType.queryTypeStatTodayInfo, new QueryStatTodayInfo());//后台大厅当日信息

        m_items.Add(QueryType.queryTypeStatAirDropSys, new QueryStatAirDropSys()); //系统空投发布
        m_items.Add(QueryType.queryTypeStatAirDropSysOpen, new QueryStatAirDropSysOpen()); //系统空投 打开

        m_items.Add(QueryType.queryTypeStatLotteryExchange, new QueryStatFishlordLotteryExchange()); //彩券鱼统计

        m_items.Add(QueryType.queryTypeStatDailyWeekTask, new QueryStatDailyWeekTask());//每日周任务统计
        m_items.Add(QueryType.queryTypeStatDailyWeekReward, new QueryStatDailyWeekReward());//每日周奖励统计

        m_items.Add(QueryType.queryTypeStatMainlyTask, new QueryStatMainlyTask());//主线任务

        m_items.Add(QueryType.queryTypeStatGoldFishLottery, new QueryStatGoldFishLottery());//幸运抽奖
        m_items.Add(QueryType.queryTypeStatGoldFishLotteryTotal, new QueryStatGoldFishLotteryTotal());//幸运抽奖总计
        m_items.Add(QueryType.queryTypeStatGoldFishLotteryDetail, new QueryStatGoldFishLotteryDetail());//幸运抽奖详情

        m_items.Add(QueryType.queryTypeOperationPlayerBankruptStat, new QueryStatPlayerBankrupt());//破产统计
        m_items.Add(QueryType.queryTypeOperationPlayerBankruptDetail, new QueryStatPlayerBankruptDetail());//破产统计详情
        m_items.Add(QueryType.queryTypeOperationPlayerOpenRateBankruptList, new QueryStatPlayerOpenRateBankruptList());//破产炮倍详情

        m_items.Add(QueryType.queryTypeOperationPlayerActTurret, new QueryOperationPlayerActTurret());//炮数成长分布
        m_items.Add(QueryType.queryTypeOperationPlayerActTurretBySingle, new QueryOperationPlayerActTurretBySingle()); //玩家炮数成长分布

        m_items.Add(QueryType.queryTypeStatMiddleRoomExchange, new QueryTypeStatMiddleRoomExchange());//中级场礼包统计

        m_items.Add(QueryType.queryTypeStatFishlordMiddleRoomPlayerScore, new QueryTypeStatFishlordMiddleRoomPlayerScore());//中级场作弊玩家积分
        m_items.Add(QueryType.queryTypeStatFishlordMiddleRoomPlayerRank, new QueryTypeStatFishlordMiddleRoomPlayerRank());//中级场玩家积分排行榜

        m_items.Add(QueryType.queryTypeStatFishlordMiddleRoomIncome, new QueryTypeStatFishlordMiddleRoomIncome());//中级场玩法收入统计
        m_items.Add(QueryType.queryTypeStatFishlordMiddleRoomExchange, new QueryTypeStatFishlordMiddleRoomExchange());//中级场兑换统计
        m_items.Add(QueryType.queryTypeStatFishlordMiddleRoomExchangeDetail, new QueryTypeStatFishlordMiddleRoomExchangeDetail());//中级场兑换统计详情
        m_items.Add(QueryType.queryTypeStatFishlordMiddleRoomRank, new QueryTypeStatFishlordMiddleRoomRank());//中级场历史排行榜
        m_items.Add(QueryType.queryTypeStatFishlordMiddleRoomFuDai, new QueryTypeStatFishlordMiddleRoomFuDai());//打点数据

        m_items.Add(QueryType.queryTypeStatFishlordAdvancedRoomScore, new QueryTypeStatFishlordAdvancedRoomScore());//高级场作弊玩家积分
        m_items.Add(QueryType.queryTypeStatFishlordAdvancedRoom, new QueryTypeStatFishlordAdvancedRoom());//高级场控制管理
        m_items.Add(QueryType.queryTypeStatFishlordAdvancedRoomAct, new QueryTypeStatFishlordAdvancedRoomAct());//高级场奖池统计
        m_items.Add(QueryType.queryTypeStatFishlordAdvancedRoomActDetail, new QueryTypeStatFishlordAdvancedRoomActDetail());//高级场奖池统计详情
        m_items.Add(QueryType.queryTypeStatFishlordAdvancedRoomActRank, new QueryTypeStatFishlordAdvancedRoomActRank());//高级场排行榜

        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomIncome, new QueryTypeStatFishlordSharkRoomIncome());//巨鲨场玩法收入统计
        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomIncomeDetail, new QueryTypeStatFishlordSharkRoomIncomeDetail());//详情
        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomChaijieIncome, new QueryTypeStatFishlordSharkRoomChaijieIncome());//拆解统计

        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomBomb, new QueryTypeStatFishlordSharkRoomBomb());//轰炸机统计
        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomLottery, new QueryTypeStatFishlordSharkRoomLottery());//抽奖统计
        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomLotteryDetail, new QueryTypeStatFishlordSharkRoomLotteryDetail());//抽奖统计详情
        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomRank, new QueryTypeStatFishlordSharkRoomRank());//巨鲨场排行榜
        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomScore, new QueryTypeStatFishlordSharkRoomScore());//巨鲨场作弊玩家积分
        m_items.Add(QueryType.queryTypeStatFishlordSharkRoomEnergy, new QueryTypeStatFishlordSharkRoomEnergy());//巨鲨场能量统计

        m_items.Add(QueryType.queryTypeStatDailyGift, new QueryTypeStatDailyGift());//每日礼包统计
        m_items.Add(QueryType.queryTypeStatDailyGiftDetail, new QueryTypeStatDailyGiftDetail());//每日礼包详情

        m_items.Add(QueryType.queryTypeStatTreasureHunt, new QueryStatTreasureHunt());//南海寻宝
        m_items.Add(QueryType.queryTypeStatTreasureHuntDetail, new QueryStatTreasureHuntDetail()); //南海寻宝详情

        m_items.Add(QueryType.queryTypeStatNewGuildLosePoint, new QueryTypeStatNewGuildLosePoint());//流失点统计

        m_items.Add(QueryType.queryTypeStatTomorrowGift, new QueryTypeStatTomorrowGift());//明日礼包

        m_items.Add(QueryType.queryTypeStatSdAct, new QueryTypeStatSdAct());//开服活动统计
        m_items.Add(QueryType.queryTypeStatSdLotteryAct, new QueryTypeStatSdLotteryAct());//开服抽奖统计
        m_items.Add(QueryType.queryTypeStatSdLotteryActDetail, new QueryTypeStatSdLotteryActDetail());//开服抽奖统计详情

        m_items.Add(QueryType.queryTypeStatOnlineReward, new QueryTypeStatOnlineReward());//在线奖励

        m_items.Add(QueryType.queryTypeStatPlayerItemRecharge, new QueryTypeStatPlayerItemRecharge());//背包购买

        m_items.Add(QueryType.queryTypeStatPlayerPhoneVertify, new QueryTypeStatPlayerPhoneVertify());//查询玩家手机验证

        //十一活动
        m_items.Add(QueryType.queryTypeStatActSign, new QueryTypeStatActSign());//签到
        m_items.Add(QueryType.queryTypeStatActExchange, new QueryTypeStatActExchange());//欢乐集字
        m_items.Add(QueryType.queryTypeStatActTask, new QueryTypeStatActTask());//冒险之路
        m_items.Add(QueryType.queryTypeStatActGift, new QueryTypeStatActGift());//礼包购买
        m_items.Add(QueryType.queryTypeStatActLottery, new QueryTypeStatActLottery());//抽奖统计
        m_items.Add(QueryType.queryTypeStatNationalDayActLotteryDetail, new QueryTypeStatNationalDayActLotteryDetail());//抽奖统计详情

        //追击蟹将
        m_items.Add(QueryType.queryTypeStatKillCrabActLottery, new QueryTypeStatKillCrabActLottery());//追击蟹将抽奖统计
        m_items.Add(QueryType.queryTypeStatKillCrabActLotteryDetail, new QueryTypeStatKillCrabActLotteryDetail());//追击蟹将抽奖统计详情
        m_items.Add(QueryType.queryTypeStatKillCrabActRoom, new QueryTypeStatKillCrabActRoom());//追击蟹将场次统计
        m_items.Add(QueryType.queryTypeStatKillCrabActTask, new QueryTypeStatKillCrabActTask());//追击蟹将任务统计

        m_items.Add(QueryType.queryTypeStatPlayerOpenRateTask, new QueryTypeStatPlayerOpenRateTask());//炮倍任务

        m_items.Add(QueryType.queryTypeStatGoldOnPlayer, new QueryTypeStatGoldOnPlayer());//玩家携带金币
        m_items.Add(QueryType.queryTypeStatRebateGift, new QueryTypeStatRebateGift());//返利礼包
        m_items.Add(QueryType.queryTypeStatTurretChip, new QueryTypeStatTurretChip());//鱼雷碎片统计
        m_items.Add(QueryType.queryTypePlayerMail, new QueryTypeStatPlayerMail());//玩家邮件统计

        m_items.Add(QueryType.queryTypeStatPumpGrowFund, new QueryTypeStatPumpGrowFund());//成长基金统计
        m_items.Add(QueryType.queryTypeStatVipGift, new QueryTypeStatVipGift());//VIP特权打点
        m_items.Add(QueryType.queryTypeStatVipRecord, new QueryTypeStatVipRecord());//VIP特权统计
        m_items.Add(QueryType.queryTypeStatMonthCard, new QueryTypeStatMonthCard());//月卡购买统计

        m_items.Add(QueryType.queryTypeStatTurretItemsOnPlayer, new QueryTypeStatTurretItemsOnPlayer());//玩家平均携带物品

        m_items.Add(QueryType.queryTypeStatPlayAd, new QueryTypeStatPlayAd());//激励视频统计
    }
}

///////////////////////////////////////////////////////////////////////////////

public class QueryBase
{
    // 作查询
    public virtual OpRes doQuery(object param, GMUser user) { return OpRes.op_res_failed; }

    // 返回查询结果
    public virtual object getQueryResult() { return null; }

    public virtual object getQueryResult(object param, GMUser user) { return null; }

    public virtual OpRes makeQuery(object param, GMUser user, QueryCondition imq) { return OpRes.op_res_failed; }

    // 通过玩家ID，返回域
    public static Dictionary<string, object> getPlayerProperty(int playerId, GMUser user, string[] fields)
    {
        //DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        //Dictionary<string, object> ret =
        //    DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", playerId, fields, dip);
        Dictionary<string, object> ret = 
            DBMgr.getInstance().getTableData(TableName.PLAYER_INFO,"player_id",playerId,fields,user.getDbServerID(),DbName.DB_PLAYER);
        return ret;
    }

    // 通过账号返回玩家属性
    public static Dictionary<string, object> getPlayerPropertyByAcc(string acc, GMUser user, string[] fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> ret =
                        DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "account", acc, fields, dip);
        return ret;
    }

    // 通过昵称返回玩家属性
    public static Dictionary<string, object> getPlayerPropertyByNickName(string nickName, GMUser user, string[] fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> ret =
                        DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "nickname", nickName, fields, dip);
        return ret;
    }
}
///////////////////////////////////////////////////////////////////////////////
public enum QueryWay
{
    by_way0,        
    by_way1,   
    by_way2,   
    by_way3,   
    by_way4,   
    by_way5, 
}

public class ParamQueryBase
{
    // 当前查询第几页，以1开始计数
    public int m_curPage;
    // 每页多少条记录
    public int m_countEachPage;
}

public class ParamQuery : ParamQueryBase
{
    // 查询方式
    public int m_op;
    public int m_type;
    public QueryWay m_way;
    public string m_showWay = "";
    public string m_param = "";
    public string m_time = "";
    public string m_channelNo;
    public string m_playerId;
}

public class ParamFishlordAdvancedRoomItem : ParamQueryBase
{
    public int m_op;
    public int m_level;
    public int m_ratio;
    public int m_maxWinCount;
}

public class ParamAirdropSysItem 
{
    public int m_op;
    public string m_time;
    public string m_uuid;
    public string m_itemId;
    public string m_pwd;
    public string m_state;
}

public class ParamGameBItem
{
    public int m_op;
    public string m_playerId = "";
    public int m_roomId;
    public int m_type;
    public string m_param = "";
    public string m_logFlag = "";
    public string m_time = "";
}

public class ParamOnlinePerHour 
{
    public int m_gameId;
    public int m_roomId;
    public string m_time;
}

public class ParamRepairOrder
{
    public string m_time;
    public int m_op;
    public string m_playerId;
    public int m_itemId;
    public int m_bonusId;
    public string m_comments;
    public string m_operator;
    public int m_rtype;
    public string m_param;
}

public class ParamSelectLostPlayer : ParamQueryBase
{
    public string m_vipLevel;
    public string m_days;
    public string m_time;
    public int m_isBindPhone;
}

//玩家信息
public class ParamPlayerInfo 
{
    public string m_playerId;
    public string m_account;
    public string m_pwd;
}

public class ParamSignByMonth 
{
    public int m_op;
    public int m_year;
    public int m_month;
}

public class QueryCondition
{
    private bool m_isExport = false;
    private List<IMongoQuery> m_queryList = new List<IMongoQuery>();
    private Dictionary<string, object> m_cond = new Dictionary<string, object>();

    public void startQuery()
    { 
        m_isExport = false;
        m_queryList.Clear();
    }

    public void startExport() 
    {
        m_isExport = true;
        m_cond.Clear();
    }

    public bool isExport() { return m_isExport; }

    public void addCond(string name, object c)
    {
        m_cond.Add(name, c);
    }

    public Dictionary<string, object> getCond() { return m_cond; }

    public IMongoQuery getImq() 
    {
        return m_queryList.Count > 0 ? Query.And(m_queryList) : null;
    }

    public void addImq(IMongoQuery imq)
    {
        m_queryList.Add(imq);
    }

    // 根据情况增加查询条件
    public void addQueryCond(string name, object c)
    {
        if (m_isExport)
        {
            m_cond.Add(name, c);
        }
        else
        {
            m_queryList.Add(Query.EQ(name, BsonValue.Create(c)));
        }
    }
}
///////////////////////////////////////////////////////////////////////////////////////
//新手引导埋点统计
public class newGuideStepItem 
{
    public int m_step;
    public string m_stepName;
    public int m_totalFinish;
    public int m_thisDayFinish;
    public int m_followFinish;
    
    public string getFinishPercent(int num1, int num2) //完成总人数   当日新增
    {
        if (num2 == 0)
            return num1.ToString();
        return Math.Round((num1 * 100.0 / num2), 2) + "%";
    }
}
public class pumpNewGuideItem 
{
    public string m_time;
    public int m_thisDayAdd;
    public int m_flag = 1;
    public List<newGuideStepItem> m_data = new List<newGuideStepItem>();
}

public class QueryTypePumpNewGuide : QueryBase 
{
    private List<pumpNewGuideItem> m_result = new List<pumpNewGuideItem>();
    public pumpNewGuideItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag++;
                return d;
            }
        }

        pumpNewGuideItem item = new pumpNewGuideItem();
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        //时间
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time,ref mint,ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime",BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime",BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1,imq2);

        OpRes code = query(p,imq,user);
        return code;
        }

    //返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq,GMUser user) 
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list = DBMgr.getInstance().executeQuery(TableName.PUMP_NEW_GUIDE,dip,imq,
            0,0,null,"genTime",false);
        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["step"]).OrderByDescending(a=>a["genTime"]).ToList();
        for (int i = 0; i<dataList.Count; i++) 
        {
            Dictionary<string,object> data = dataList[i];
            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            pumpNewGuideItem tmp = IsCreate(time);
            tmp.m_time = time;
            newGuideStepItem list = new newGuideStepItem();
            tmp.m_data.Add(list);
            if (tmp.m_flag == 1) //第一次，新建tmp
            {
                //时间
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                bool res = Tool.splitTimeStr(time, ref mint, ref maxt);
                if (!res)
                    return OpRes.op_res_time_format_error;

                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                IMongoQuery imq_1 = Query.And(imq1, imq2);

                //当日新增
                List<Dictionary<string, object>> thisDayAddList = DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_REG, dip, imq_1,
                0, 0, null, "genTime", false);
                if (thisDayAddList == null || thisDayAddList.Count == 0)
                {
                    tmp.m_thisDayAdd = 0;
                }
                else 
                {
                    for (int k = 0; k < thisDayAddList.Count; k++)
                    {
                        if (thisDayAddList[k].ContainsKey("count"))
                            tmp.m_thisDayAdd += Convert.ToInt32(thisDayAddList[k]["count"]);
                    }
                }
            }

            list.m_step = Convert.ToInt32(data["step"]);
            var da = M_GUIDECFG.getInstance().getValue(list.m_step);
            if (da != null)
                list.m_stepName = da.m_itemName;
            if(data.ContainsKey("totalFinish"))
                list.m_totalFinish = Convert.ToInt32(data["totalFinish"]);
            if (data.ContainsKey("thisDayFinish"))
                list.m_thisDayFinish = Convert.ToInt32(data["thisDayFinish"]);
            if (data.ContainsKey("followFinish"))
                list.m_followFinish = Convert.ToInt32(data["followFinish"]);
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////
//五一充返活动
public class rewardResList
{
    public int m_rewardId;
    public string m_rewardType;
    public int m_rewardCount;
    public int m_rewardPerson;
}

public class wuYiRewardResItem 
{
    public string m_time;
    public int m_joinCount;
    public int m_joinPerson;
    public int m_flag = 1;
    public List<rewardResList> m_data = new List<rewardResList>();
}
public class QueryTypeWuyiRewardResult : QueryBase 
{
    private List<wuYiRewardResItem> m_result = new List<wuYiRewardResItem>();
    protected static string[] m_fields = { "genTime", "joinCount", "joinPerson" };
    public wuYiRewardResItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag++;
                return d;
            }
        }

        wuYiRewardResItem item = new wuYiRewardResItem();
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        //时间
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        OpRes code = query(p, imq, user);
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> data_list = DBMgr.getInstance().executeQuery(TableName.PUMP_WUYI_REWARD_RESULT, dip, imq,
             0, 0, null, "genTime", false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["rewardId"]).OrderByDescending(a => a["genTime"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            wuYiRewardResItem tmp = IsCreate(time);
            tmp.m_time = time;
            if(tmp.m_flag==1) //只需要查询一次
            {
                //时间
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                bool res = Tool.splitTimeStr(time, ref mint, ref maxt);
                if (!res)
                    return OpRes.op_res_time_format_error;

                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                IMongoQuery imq_1 = Query.And(imq1, imq2);

                Dictionary<string, object> joinList = DBMgr.getInstance().getTableData(TableName.STAT_WUYI_JOIN, dip, imq_1, m_fields);
                if (joinList != null)
                {
                    if (joinList.ContainsKey("joinCount"))
                        tmp.m_joinCount = Convert.ToInt32(joinList["joinCount"]);

                    if (joinList.ContainsKey("joinPerson"))
                        tmp.m_joinPerson = Convert.ToInt32(joinList["joinPerson"]);
                }
            }
            
            rewardResList list = new rewardResList();
            tmp.m_data.Add(list);
            list.m_rewardPerson=Convert.ToInt32(data["personCount"]);
            list.m_rewardId = Convert.ToInt32(data["rewardId"]);
            var da = M_WUYI_RECHARGECFG.getInstance().getValue(list.m_rewardId);
            if(da!=null)
            {
                list.m_rewardCount = list.m_rewardPerson * da.m_itemCount;//奖励数量
                ItemCFGData item = ItemCFG.getInstance().getValue(da.m_itemId);
                if (item != null)
                    list.m_rewardType = item.m_itemName;
            }
        }
        return OpRes.opres_success;
    }
}
////////////////////////////////////////////////////////////////////////////////////////
//欢乐炸炸炸
public class bulletHeadRankItem 
{
    public string m_time;
    public int m_type;
    public int m_rank;
    public string m_nickName;
    public int m_playerId;
    public int m_maxGold; //鱼雷分值
    public string m_rewardName;//获奖项
    public int m_useCount;
    public int m_rankType2;

    public string getContentType() 
    {
        string type = m_rankType2.ToString();
        switch(m_rankType2)
        {
            case 0: type = "欢乐炸"; break;
            case 1: type = "衰神炸"; break;
        }
        return type;
    }
}
public class QueryTypeBulletHeadRank : QueryBase
{
    private List<bulletHeadRankItem> m_result = new List<bulletHeadRankItem>();
    protected static string[] m_field = { "nickname" };
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p=(ParamQuery)param;
        //时间
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        imq = Query.And(imq,Query.EQ("rankType",p.m_op));

        OpRes code = query(p, imq, user);
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_BULLET_HEAD_RANK, imq, dip);
        List<Dictionary<string, object>> data_list = DBMgr.getInstance().executeQuery(TableName.PUMP_BULLET_HEAD_RANK, dip, imq,
             0, 0, null, "genTime", false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["rank"]).OrderByDescending(a=>a["genTime"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            bulletHeadRankItem tmp = new bulletHeadRankItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            Dictionary<string, object> ret = getPlayerProperty(tmp.m_playerId, user, m_field);
            if (ret != null && ret.ContainsKey("nickname"))
                tmp.m_nickName = Convert.ToString(ret["nickname"]);

            if (data.ContainsKey("maxGold"))
                tmp.m_maxGold = Convert.ToInt32(data["maxGold"]);

            if (data.ContainsKey("useCount"))
                tmp.m_useCount = Convert.ToInt32(data["useCount"]);

            if (data.ContainsKey("rankType2"))
                tmp.m_rankType2 = Convert.ToInt32(data["rankType2"]);

            //根据排名获取奖励
            tmp.m_rank = Convert.ToInt32(data["rank"]);
            if(tmp.m_rank!=0)
            {
                var allData = F_TORPEDO_RANK_REWARDCFG.getInstance().getAllData();
                if(allData!=null)
                {
                    foreach(var da in allData.Values)
                    {
                        if (da.m_RankType == param.m_op && (da.m_type -1) == tmp.m_rankType2 && (da.m_EndRank >= tmp.m_rank && da.m_StartRank <= tmp.m_rank)) 
                        {
                            string[] rewardList = Tool.split(da.m_RewardList, ',', StringSplitOptions.RemoveEmptyEntries);
                            string[] rewardCount = Tool.split(da.m_RewardCount, ',', StringSplitOptions.RemoveEmptyEntries);

                            for (int k = 0; k < rewardList.Length; k++)
                            {
                                ItemCFGData item = ItemCFG.getInstance().getValue(Convert.ToInt32(rewardList[k]));
                                if (item != null)
                                {
                                    tmp.m_rewardName += item.m_itemName + "：" + rewardCount[k] + "&ensp;&ensp;";
                                }
                                else
                                {
                                    tmp.m_rewardName += rewardList[k] + "：" + rewardCount[k] + "&ensp;&ensp;";
                                }
                            }
                        }
                    }
                }
            }
        }
        return OpRes.opres_success;
    }
}
//欢乐炸炸炸当前排行
public class QueryTypeBulletHeadCurrRank : QueryBase 
{
    private List<bulletHeadRankItem> m_result = new List<bulletHeadRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        OpRes code = query(p, user);
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        string queryIndex = "copperTorpedoMaxCoin";
        switch(param.m_op)
        {
            case 0: queryIndex = "copperTorpedoMaxCoin"; break;
            case 1: queryIndex = "sliverTorpedoMaxCoin"; break;
            case 2: queryIndex = "goldenTorpedoMaxCoin"; break;
            case 3: queryIndex = "diamondTorpedoMaxCoin"; break;
        }
        IMongoQuery imq = Query.GT(queryIndex,0);
        bool is_asc = false;  //降序  1 3 5 7
        string week = DateTime.Now.DayOfWeek.ToString();
        if (week == "Tuesday" || week == "Thursday" || week == "Saturday")
            is_asc = true;

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.BULLET_HEAD_ACTIVITY, dip, imq,
             0, 20, null, queryIndex, is_asc);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            bulletHeadRankItem tmp = new bulletHeadRankItem();
            m_result.Add(tmp);
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey(queryIndex))
                tmp.m_maxGold = Convert.ToInt32(data[queryIndex]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_type = Convert.ToInt32(data["vipLevel"]);

            if (data.ContainsKey("curRankType"))
                tmp.m_rankType2 = Convert.ToInt32(data["curRankType"]);

            //根据排名
            tmp.m_rank = (i+1);
        }
        return OpRes.opres_success;
    }
}
//炸弹乐园玩家排行
public class StatBulletHeadScoreItem 
{
    public int m_playerId;
    public string m_nickName;
    public int m_copperTorpedoMaxCoin;
    public int m_sliverTorpedoMaxCoin;
    public int m_goldenTorpedoMaxCoin;
    public int m_diamondTorpedoMaxCoin;
}
public class QueryTypeBulletHeadPlayerScore : QueryBase 
{
    private List<StatBulletHeadScoreItem> m_result = new List<StatBulletHeadScoreItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;

        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq = Query.EQ("playerId", playerId);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> playerScore = DBMgr.getInstance().getTableData(TableName.BULLET_HEAD_ACTIVITY, dip, imq);
        if (playerScore != null)
        {
            StatBulletHeadScoreItem tmp = new StatBulletHeadScoreItem();
            m_result.Add(tmp);

            if (playerScore.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(playerScore["nickName"]);

            if (playerScore.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(playerScore["playerId"]);

            if (playerScore.ContainsKey("copperTorpedoMaxCoin"))
                tmp.m_copperTorpedoMaxCoin = Convert.ToInt32(playerScore["copperTorpedoMaxCoin"]);

            if (playerScore.ContainsKey("sliverTorpedoMaxCoin"))
                tmp.m_sliverTorpedoMaxCoin = Convert.ToInt32(playerScore["sliverTorpedoMaxCoin"]);

            if (playerScore.ContainsKey("goldenTorpedoMaxCoin"))
                tmp.m_goldenTorpedoMaxCoin = Convert.ToInt32(playerScore["goldenTorpedoMaxCoin"]);

            if (playerScore.ContainsKey("diamondTorpedoMaxCoin"))
                tmp.m_diamondTorpedoMaxCoin = Convert.ToInt32(playerScore["diamondTorpedoMaxCoin"]);

            return OpRes.opres_success;
        }
        return OpRes.op_res_not_found_data;
    }
}
////////////////////////////////////////////////////////////////////////////////////////
//龙宫场排行统计历史排行
public class QueryTypeDragonScaleRank : QueryBase 
{
    private List<bulletHeadRankItem> m_result = new List<bulletHeadRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        //时间
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("balanceTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("balanceTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        OpRes code = query(p, imq, user);
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list = DBMgr.getInstance().executeQuery(TableName.FISHLORD_DRAGON_PALACE_RANK, dip, imq,
             0, 0, null, "balanceTime", false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["rank"]).OrderByDescending(a => a["balanceTime"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            bulletHeadRankItem tmp = new bulletHeadRankItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["balanceTime"]).ToLocalTime().ToShortDateString();
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("gainDimensity")) //排行时获得的魔石
                tmp.m_useCount = Convert.ToInt32(data["gainDimensity"]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_type = Convert.ToInt32(data["vipLevel"]);

            //根据排名获取奖励
            tmp.m_rank = Convert.ToInt32(data["rank"]);
            if (tmp.m_rank != 0)
            {
                var allData = F_DRAGON_SCALE_RANK_REWARDCFG.getInstance().getAllData();
                if (allData != null)
                {
                    foreach (var da in allData.Values)
                    {
                        if (da.m_EndRank >= tmp.m_rank && da.m_StartRank <= tmp.m_rank)
                        {
                            string[] rewardList = Tool.split(da.m_RewardList , ',', StringSplitOptions.RemoveEmptyEntries);
                            string[] rewardCount = Tool.split(da.m_RewardCount,',',StringSplitOptions.RemoveEmptyEntries);

                            for (int k = 0; k<rewardList.Length; k++)
                            {
                                int rewardId = Convert.ToInt32(rewardList[k]);
                                ItemCFGData item = ItemCFG.getInstance().getValue(rewardId);

                                double count = Convert.ToDouble(rewardCount[k]);
                                if (rewardId == 3)
                                    count = count / 100.0;

                                if (item != null)
                                {
                                    tmp.m_rewardName += item.m_itemName + "：" + count + "&ensp;&ensp;";
                                }
                                else 
                                {
                                    tmp.m_rewardName += rewardList[k] + "：" + count + "&ensp;&ensp;";
                                } 
                            }
                            break;
                        }
                    }
                }
            }
        }
        return OpRes.opres_success;
    }
}
//龙宫场排行当前排行
public class QueryTypeDragonScaleCurrRank : QueryBase 
{
    private List<bulletHeadRankItem> m_result = new List<bulletHeadRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        IMongoQuery imq = Query.GT("weekDimensityHistory", 0);
        OpRes code = query(imq, user);
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> data_list = DBMgr.getInstance().executeQuery(TableName.FISHLORD_DRAGON_PALACE_PLAYER, dip, imq,
             0, 50, null, "weekDimensityHistory", false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        //根据weekDragonScale降序，根据weekTime升序
        var dataList = data_list.OrderBy(
            a =>{
                if(a.ContainsKey("weekTime"))
                {
                    return a["weekTime"];
                }else{
                    return DateTime.Now;
                }
            }).OrderByDescending(a => a["weekDimensityHistory"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            bulletHeadRankItem tmp = new bulletHeadRankItem();
            m_result.Add(tmp);
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("weekDimensityHistory"))
                tmp.m_maxGold = Convert.ToInt32(data["weekDimensityHistory"]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_type = Convert.ToInt32(data["vipLevel"]);
            //根据排名
            tmp.m_rank = (i + 1);
        }
        return OpRes.opres_success;
    }
}
////////////////////////////////////////////////////////////////////////////////////////
//礼包码cdKey生成器列表
public class giftCodeNewItem 
{
    public string m_cdKey;
    public string m_time;
    public string m_deadTime;
    public int m_type;
    public string m_giftName;
    public int m_maxUseCount;
    public int m_curUseCount;
    public string m_comment;
    public string m_id;
}

public class QueryTypeGiftCodeNewList : QueryBase 
{
    private List<giftCodeNewItem> m_result = new List<giftCodeNewItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        
        OpRes code = query(user);
        return code;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.CD_KEY_MULT, serverId, DbName.DB_ACCOUNT,
            null, 0, 0, null,"genTime",false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            giftCodeNewItem tmp = new giftCodeNewItem();
            m_result.Add(tmp);
            tmp.m_id = Convert.ToString(data["_id"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();
            tmp.m_deadTime = Convert.ToDateTime(data["deadTime"]).ToLocalTime().ToShortDateString();
            tmp.m_type = Convert.ToInt32(data["keyType"]);
            tmp.m_cdKey = Convert.ToString(data["keyCode"]);
            if(data.ContainsKey("giftId"))
            {
                int giftId = Convert.ToInt32(data["giftId"]);
                var allData = M_CDKEY_GiftCFG.getInstance().getValue(giftId);
                if(allData != null)
                    tmp.m_giftName = allData.m_name;
            }

            tmp.m_maxUseCount = Convert.ToInt32(data["maxUseCount"]);
            if(data.ContainsKey("hasUseCount"))
                tmp.m_curUseCount = Convert.ToInt32(data["hasUseCount"]);

            if(data.ContainsKey("comment"))
                tmp.m_comment = Convert.ToString(data["comment"]);
        }
        return OpRes.opres_success;
    }
}


////////////////////////////////////////////////////////////////////////////////
//添加同步道具失败
public class addItemError 
{
    public string m_id;
    public string m_time;
    public string m_playerId;
    public int m_failReason;
    public int m_addItemReason;
    public string m_recCreateTime;
    public bool m_isDeal = false;
    public string m_syncKey="";
    public List<giftErrorItem> m_rewardList = new List<giftErrorItem>();
}

public class QueryTypeWord2LogicItemError : QueryBase 
{
    private List<addItemError> m_result = new List<addItemError>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        //时间
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //playerId
        int playerId = 0;
        if (!string.IsNullOrEmpty(p.m_playerId)) 
        {
            if (!int.TryParse(p.m_playerId, out playerId))
                return OpRes.op_res_param_not_valid;
            imq = Query.And(imq, Query.EQ("playerId", playerId));
        }

        //同步字段
        if (!string.IsNullOrEmpty(p.m_channelNo))
            imq = Query.And(imq,Query.EQ("syncKey",p.m_channelNo));

        OpRes code = query(p, imq, user);
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_WORD2_LOGIC_ITEM_ERROR, imq, dip);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_WORD2_LOGIC_ITEM_ERROR, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            addItemError tmp = new addItemError();
            m_result.Add(tmp);
            tmp.m_id = Convert.ToString(data["_id"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("syncKey"))
            {
                tmp.m_syncKey = Convert.ToString(data["syncKey"]);
            }

            if (data.ContainsKey("failReason"))
            {
                tmp.m_failReason = Convert.ToInt32(data["failReason"]);
            }
            if (data.ContainsKey("addItemReason"))
            {
                tmp.m_addItemReason= Convert.ToInt32(data["addItemReason"]);
            }
            if(data.ContainsKey("recCreateTime"))
            {
                tmp.m_recCreateTime = Convert.ToDateTime(data["recCreateTime"]).ToLocalTime().ToString();
            }

            //道具
            if (data.ContainsKey("gifts"))
            {
                Dictionary<string, object> gList = (Dictionary<string, object>)data["gifts"];
                Tool.parseItemFromDicError(gList, tmp.m_rewardList);
            }

            if(data.ContainsKey("isDeal"))
            {
                tmp.m_isDeal = Convert.ToBoolean(data["isDeal"]);
            }
        }
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////////////
//玩家道具详情
public class playerItemRecord 
{
    public string m_time;
    public string m_playerId;
    public int m_itemIId;
    public int m_moneyReasonId = 0;
    public int m_itemOldCount;
    public int m_itemNewCount;
    public int m_itemAddCount;
    public string m_syncKey;

    // 返回动作名称
    public string getActionName()
    {
        XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");
        if (xml != null)
        {
            return xml.getString(m_moneyReasonId.ToString(), "");
        }
        return "";
    }
}
public class QueryTypePlayerItemRecord : QueryBase 
{
    private List<playerItemRecord> m_result = new List<playerItemRecord>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        //时间
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);

        //playerId
        int playerId = 0;
        if (!string.IsNullOrEmpty(p.m_playerId))
        {
            if (!int.TryParse(p.m_playerId, out playerId))
                return OpRes.op_res_param_not_valid;

            imq = Query.And(imq1, Query.EQ("playerId", playerId));
        }

        //itemId
        int itemId = 0;
        if (!string.IsNullOrEmpty(p.m_param))
        {
            if (!int.TryParse(p.m_param, out itemId))
                return OpRes.op_res_param_not_valid;
            imq = Query.And(imq, Query.EQ("itemId", itemId));
        }

        //同步字段
        if(!string.IsNullOrEmpty(p.m_channelNo))
            imq = Query.And(imq, Query.EQ("syncKey", p.m_channelNo));

        OpRes code = query(p, imq, user);
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_PLAYER_ITEM, imq, dip);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_ITEM, dip, imq,
             (param.m_curPage-1)*param.m_countEachPage, param.m_countEachPage, null, "genTime", false );

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            playerItemRecord tmp = new playerItemRecord();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            if(data.ContainsKey("itemId"))
            {
                tmp.m_itemIId = Convert.ToInt32(data["itemId"]);
            }
            if(data.ContainsKey("reason"))
            {
                tmp.m_moneyReasonId = Convert.ToInt32(data["reason"]);
            }
            if(data.ContainsKey("oldCount"))
            {
                tmp.m_itemOldCount = Convert.ToInt32(data["oldCount"]);
            }
            if(data.ContainsKey("newCount"))
            {
                tmp.m_itemNewCount = Convert.ToInt32(data["newCount"]);
            }
            if(data.ContainsKey("addCount"))
            {
                tmp.m_itemAddCount = Convert.ToInt32(data["addCount"]);
            }
            if (data.ContainsKey("syncKey")) 
            {
                tmp.m_syncKey = Convert.ToString(data["syncKey"]);
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////
//邮件查询
public class mainQuery
{
    public string m_time;
    public string m_playerId;
    public string m_title = "";
    public string m_content = "";
    public bool m_isReceive;
    public List<mailGiftItem> m_rewardList = new List<mailGiftItem>();
}
//邮件发送失败
public class mailSendFail 
{
    public string m_time;
    public string m_playerId;
    public int m_rank;
    public string m_rankType="";

    public string getRankTypeName(int type) 
    {
        string rankTypeName = type.ToString();
        switch(type)
        {
            case 0: rankTypeName="日排行榜"; break;
            case 1: rankTypeName="周排行榜"; break;
        }
        return rankTypeName;
    }
}
public class QueryTypeMailQuery : QueryBase 
{
    private List<mainQuery> m_result = new List<mainQuery>();
    private List<mailSendFail> m_result_fail = new List<mailSendFail>();
    static string[] m_field1 = { "player_id" };

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_result_fail.Clear();
        ParamQuery p=(ParamQuery)param;
        int id = 0;
        switch (p.m_way)
        {
            case QueryWay.by_way0:  // 以玩家id查询
                if (!int.TryParse(p.m_param, out id))
                    return OpRes.op_res_param_not_valid;
                break;

            case QueryWay.by_way1:  // 以玩家账号查询
                queryByAccount(p.m_param, user, ref id);
                break;

            case QueryWay.by_way2:  // 以玩家昵称查询
                queryByNickname(p.m_param, user, ref id);
                break;
        }
        return query(id, user,p);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user) 
    {
        return m_result_fail; 
    }

    // 通过玩家账号查询
    private OpRes queryByAccount(string queryStr, GMUser user, ref int id)
    {
        if (queryStr != "")
        {
            Dictionary<string, object> data = getPlayerPropertyByAcc(queryStr, user, m_field1);
            if (data == null)
                return OpRes.op_res_not_found_data;

            if (data.ContainsKey("player_id"))
            {
                id = Convert.ToInt32(data["player_id"]);
            }
        }
        return OpRes.opres_success;
    }

    //通过昵称查询
    private OpRes queryByNickname(string queryStr, GMUser user,ref int id)
    {
        if (queryStr != "")
        {
            Dictionary<string, object> data = getPlayerPropertyByNickName(queryStr, user, m_field1);
            if (data == null)
                return OpRes.op_res_not_found_data;

            if (data.ContainsKey("player_id"))
            {
                id = Convert.ToInt32(data["player_id"]);
            }
        }
        return OpRes.opres_success;
    }

    private OpRes query(int id, GMUser user,ParamQuery param)
    {
        IMongoQuery imq_1 = Query.EQ("playerId", id);
        IMongoQuery imq = imq_1;
        if (!string.IsNullOrEmpty(param.m_time))  //如果写入了时间
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(param.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            string strTime = "time";
            if(param.m_op==3)
            {
                strTime = "genTime";
            }

            IMongoQuery imq1 = Query.LT(strTime, BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE(strTime, BsonValue.Create(mint));
            imq = Query.And(imq_1, Query.And(imq1, imq2));
        }
        
        string table = TableName.PLAYER_MAIL;
        int dbName = DbName.DB_PLAYER;
        switch(param.m_op)
        {
            case 0://未删除
                table = TableName.PLAYER_MAIL;
                dbName = DbName.DB_PLAYER;
                break;
            case 1://已删除
                table = TableName.PUMP_MAIL_DEL;
                dbName = DbName.DB_PUMP;
                break;
            case 2://领取
                table = TableName.PUMP_MAIL_RECV;
                dbName = DbName.DB_PUMP;
                break;
            case 3://竞技场发送失败
                table = TableName.PUMP_FISH_BAO_JIN_SEND_REWARD_FAIL;
                dbName = DbName.DB_PUMP;
                break;
        }

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), dbName, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(table, imq, dip);
        List<Dictionary<string, object>> data=new List<Dictionary<string,object>>();
        if (param.m_op == 3) //竞技场发送邮件失败
        {
             data = DBMgr.getInstance().executeQuery(table, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

            if (data == null || data.Count == 0)
                return OpRes.op_res_not_found_data;

            for (int i = 0; i < data.Count; i++)
            {
                mailSendFail tmp = new mailSendFail();
                m_result_fail.Add(tmp);

                tmp.m_time = Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToShortDateString();
                tmp.m_playerId = Convert.ToString(data[i]["playerId"]);
                if(data[i].ContainsKey("rank"))
                {
                    tmp.m_rank = Convert.ToInt32(data[i]["rank"]);
                }
                if(data[i].ContainsKey("rankType"))
                {
                    tmp.m_rankType = tmp.getRankTypeName(Convert.ToInt32(data[i]["rankType"]));
                }
            }
        }
        else //未删除/已删除/已领取
        {
            data = DBMgr.getInstance().executeQuery(table, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "time", false);

            if (data == null || data.Count == 0)
                return OpRes.op_res_not_found_data;

            for (int i = 0; i < data.Count; i++)
            {
                mainQuery tmp = new mainQuery();
                m_result.Add(tmp);
                tmp.m_time = Convert.ToDateTime(data[i]["time"]).ToLocalTime().ToString();
                tmp.m_playerId = Convert.ToString(data[i]["playerId"]);
                if (data[i].ContainsKey("title"))
                {
                    tmp.m_title = Convert.ToString(data[i]["title"]);
                }
                if (data[i].ContainsKey("content"))
                {
                    tmp.m_content = Convert.ToString(data[i]["content"]);
                }
                tmp.m_isReceive = Convert.ToBoolean(data[i]["isReceive"]);
                if (data[i].ContainsKey("gifts"))
                {
                    Dictionary<string, object> gList = (Dictionary<string, object>)data[i]["gifts"];
                    Tool.parseItemFromDicMail(gList, tmp.m_rewardList);
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////
//爱贝充值查询
public class rechargeByAiBeiItem
{
    public string m_time;
    public string m_channel;
    public int m_recharge;
    public int m_totalRecharge;
}
public class QueryRechargeByAiBei : QueryBase 
{
    private List<rechargeByAiBeiItem> m_result = new List<rechargeByAiBeiItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        int condCount = 0;
        if (p.m_param != "" && p.m_param!="-1" && p.m_param!="-2" && p.m_param!="-3" && p.m_param!="-4" && p.m_param!="-5")
        {
            queryList.Add(Query.EQ("channel", BsonValue.Create(p.m_param)));
        }
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            condCount++;
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            queryList.Add(Query.And(imq1, imq2));
        }

        if (condCount == 0)
            return OpRes.op_res_need_at_least_one_cond;

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        OpRes code = query(p, imq, user);
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        ///////////////////////////////////自有渠道或捕鱼达人////////////////////////////////////////////
        IMongoQuery imq_1 = null;
        if (param.m_param == "-4" || param.m_param == "-5" || param.m_param == "-2" || param.m_param == "-3")
        {
            Dictionary<string, TdChannelInfo> cd = new Dictionary<string, TdChannelInfo>();
            cd.Clear();
            if (param.m_param == "-4" || param.m_param == "-2")  //自有渠道
            {
                cd = TdChannelZiyou.getInstance().getAllData();
            }
            else if (param.m_param == "-5" || param.m_param == "-3")   //捕鱼达人2渠道
            {
                cd = TdChannelBuyu2.getInstance().getAllData();
            }
            int k = 0;
            foreach (var channel in cd.Values)
            {
                if (k == 0)
                {
                    imq_1 = Query.EQ("channel", channel.m_channelNo);
                }
                else
                {
                    imq_1 = Query.Or(imq_1, Query.EQ("channel", channel.m_channelNo));
                }
                k++;
            }
        }
        if (imq_1 != null)
            imq = Query.And(imq, imq_1);
        ///////////////////////////////////////////////////////////////////////////////////////////////

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.CHANNEL_RECHARGE_AIBEI, imq, serverId, DbName.DB_ACCOUNT);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_RECHARGE_AIBEI, serverId, DbName.DB_ACCOUNT, imq,
             0, 0, null, "genTime", false /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            rechargeByAiBeiItem tmp = new rechargeByAiBeiItem();
            m_result.Add(tmp);

            DateTime t = Convert.ToDateTime(data["genTime"]);
            tmp.m_time=t.ToLocalTime().ToShortDateString();
            tmp.m_channel = Convert.ToString(data["channel"]).PadLeft(6, '0');
            if(data.ContainsKey("recharge"))
            {
                tmp.m_recharge = Convert.ToInt32(data["recharge"]);
            }

            IMongoQuery q1 = Query.EQ("genTime", t);
            IMongoQuery q2 = Query.EQ("channel", tmp.m_channel);
            IMongoQuery sq = Query.And(q1, q2);
            string[] field = { "channel","totalIncome"};
            Dictionary<string, object> payTypeData =
                DBMgr.getInstance().getTableData(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, sq,field);
            if (payTypeData != null)
            {
                if (payTypeData.ContainsKey("totalIncome"))
                {
                    tmp.m_totalRecharge = Convert.ToInt32(payTypeData["totalIncome"]);
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////
//已封停玩家ID列表
public class QueryServiceUnBlockIdList : QueryBase 
{
    static string[] s_fields = { "player_id", "blockTime" };
    private List<ResultBlock> m_result = new List<ResultBlock>();
    public override OpRes doQuery(object param, GMUser user) 
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        QueryCondition m_cond = new QueryCondition();
        if (!string.IsNullOrEmpty(p.m_param))
        {
            int playerId = 0;
            if (!int.TryParse(p.m_param, out playerId))
            {
                return OpRes.op_res_param_not_valid;
            }
            m_cond.addImq(Query.EQ("player_id", playerId));
        }
        m_cond.addImq(Query.EQ("delete", BsonValue.Create(true)));
        IMongoQuery imq = m_cond.getImq();
        return query(user, p, imq);
    }
    //返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public OpRes query(GMUser user,ParamQuery param, IMongoQuery imq) 
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, dip);
        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO,
            dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, s_fields, "blockTime", false);
        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;
        bool add = false;
        for (int i = 0; i < data.Count; i++)
        {
            add = false;
            ResultBlock tmp = new ResultBlock();

            Dictionary<string, object> da = data[i];
            if (da.ContainsKey("player_id"))
            {
                tmp.m_param = Convert.ToString(da["player_id"]);
                add = true;
            }
            if (da.ContainsKey("blockTime"))
            {
                tmp.m_time = Convert.ToDateTime(da["blockTime"]).ToLocalTime().ToString();
                add = true;
            }
            if (add)
            {
                m_result.Add(tmp);
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////
//机器人最高积分
public class QueryRobotMaxScore : QueryBase 
{
    private int m_result;
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result = -1;
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        IMongoQuery imq = Query.EQ("key",1);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.FISHLORD_BAOJIN_SYS, dip,imq);
        if (data != null)
        {
            if (data.ContainsKey("robotMaxScore"))
            {
                m_result = Convert.ToInt32(data["robotMaxScore"]);
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//////////////////////////////////////////////////////////////////////////////
//弹头统计
public class fishlordBulletHeadStat 
{
    public int m_bulletHeadId;
    public int m_flag = 0;
    public Dictionary<int, fishlordBulletHeadDetail> m_data = new Dictionary<int, fishlordBulletHeadDetail>();
}
public class fishlordBulletHeadDetail
{
    public long m_useCount;
    public long m_outlayGold;
    public long m_avgoutlayGold;
    public string m_rangeParam;
    public long getAvg(long count,long total) 
    {
        if(count!=0)
        {
            long avg = Convert.ToInt64(Math.Round(total * 1.0 / count));
            return avg;
        }
        return 0;
    }
}
public class QueryFishlordBulletHeadStat : QueryBase 
{
    private List<fishlordBulletHeadStat> m_result = new List<fishlordBulletHeadStat>();
    public fishlordBulletHeadStat IsCreate(int bulletHeadId)
    {
        foreach (var d in m_result)
        {
            if (d.m_bulletHeadId == bulletHeadId) 
            {
                d.m_flag = 1;
                return d;
            }  
        }

        fishlordBulletHeadStat item = new fishlordBulletHeadStat();
        item.m_bulletHeadId = bulletHeadId;
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        IMongoQuery imq = Query.Exists("useType");
        return query(user,imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user,IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_TORPEDO, dip, imq, 0, 0, null, "torpedoId", true);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            
            if(data.ContainsKey("torpedoId"))
            {
                int bulletHeadId = Convert.ToInt32(data["torpedoId"]);
                fishlordBulletHeadStat item = IsCreate(bulletHeadId);
                fishlordBulletHeadDetail tmp = new fishlordBulletHeadDetail();

                int useType = Convert.ToInt32(data["useType"]);
                item.m_data.Add(useType, tmp);

                if(data.ContainsKey("useCount"))
                    tmp.m_useCount = Convert.ToInt64(data["useCount"]);

                if(data.ContainsKey("outlayGold"))
                    tmp.m_outlayGold = Convert.ToInt64(data["outlayGold"]);

                tmp.m_avgoutlayGold = tmp.getAvg(tmp.m_useCount,tmp.m_outlayGold);

                ////DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
                ////IMongoQuery imq1 = Query.EQ("bulletHeadId",bulletHeadId);
                ////Dictionary<string, object> queryData = DBMgr.getInstance().getTableData(TableName.FISHLORD_BULLET_HEAD, dip1, imq1);
                ////if (queryData != null)
                ////{
                ////    if (useType == 1) {
                ////        if (queryData.ContainsKey("goldKillMin") && queryData.ContainsKey("goldKillMax"))
                ////            tmp.m_rangeParam = Convert.ToString(queryData["goldKillMin"]) + ',' + Convert.ToString(queryData["goldKillMax"]);
                ////    }else if(useType == 2)
                ////    {
                ////        if (queryData.ContainsKey("goldUseMin") && queryData.ContainsKey("goldUseMax"))
                ////            tmp.m_rangeParam = Convert.ToString(queryData["goldUseMin"]) + ',' + Convert.ToString(queryData["goldUseMax"]);
                ////    }
                ////}
            }
        }
        return OpRes.opres_success;
    }
}

//弹头查询
public class fishlordBulletHeadQuery 
{
    public string m_time;
    public int m_flag = 0;
    public Dictionary<int, long[]> m_data = new Dictionary<int, long[]>();
}
public class QueryFishlordBulletHeadQuery : QueryBase 
{
    private List<fishlordBulletHeadQuery> m_result = new List<fishlordBulletHeadQuery>();
    public fishlordBulletHeadQuery IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag = 1;
                return d;
            }
        }

        fishlordBulletHeadQuery item = new fishlordBulletHeadQuery();
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p=(ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        return query(user,imq,p.m_op);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user,IMongoQuery imq, int op)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_TORPEDO_DAILY, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime t = Convert.ToDateTime(data["genTime"]);
            string time = t.ToLocalTime().ToShortDateString();
            fishlordBulletHeadQuery tmp = IsCreate(time);

            tmp.m_time = time;

            if(data.ContainsKey("torpedoId"))
            {
                int id = Convert.ToInt32(data["torpedoId"]);
                long[] da=new long [3];

                long bagUseCount = 0;
                if (data.ContainsKey("bagUseCount"))
                    bagUseCount = Convert.ToInt64(data["bagUseCount"]);

                long fishCount = 0;
                if (data.ContainsKey("useCount"))
                    fishCount = Convert.ToInt64(data["useCount"]);

                long bagOutlayGold = 0;
                if (data.ContainsKey("bagOutlayGold"))
                    bagOutlayGold = Convert.ToInt64(data["bagOutlayGold"]);

                long fishOutlayGold = 0;
                if (data.ContainsKey("outlayGold"))
                    fishOutlayGold = Convert.ToInt64(data["outlayGold"]);

                //打鱼
                IMongoQuery imq1 = Query.And(Query.EQ("genTime", t), Query.EQ("torpedoId", id));
                switch(op)
                {
                    case 0:
                        da[0] = bagUseCount + fishCount;
                        da[1] = bagOutlayGold + fishOutlayGold;
                        break;
                    case 1:
                        da[0] = fishCount;
                        da[1] = fishOutlayGold;
                        long fishPlayerCount = DBMgr.getInstance().getRecordCount(TableName.PUMP_TORPEDO_FISH_USE, imq1, dip);
                        da[2] = Convert.ToInt32(fishPlayerCount);
                        break;
                    case 2:
                        da[0] = bagUseCount;
                        da[1] = bagOutlayGold;
                        long bagPlayerCount = DBMgr.getInstance().getRecordCount(TableName.PUMP_TORPEDO_BAG_USE, imq1, dip);
                        da[2] = Convert.ToInt32(bagPlayerCount);
                        break;
                }

                if (!tmp.m_data.ContainsKey(id))
                {
                    tmp.m_data.Add(id, da);
                }
            }

            tmp.m_data = tmp.m_data.OrderBy(a => a.Key).ToDictionary(o=>o.Key,p=>p.Value);

        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////
//弹头产出统计
public class fishlordBulletHeadOutputStat 
{
    public string m_time;
    public int m_flag = 0;
    public Dictionary<int, long[]> m_data = new Dictionary<int, long[]>();
}
public class QueryStatFishlordBulletHeadOutput : QueryBase 
{
    private List<fishlordBulletHeadOutputStat> m_result = new List<fishlordBulletHeadOutputStat>();
    
    public fishlordBulletHeadOutputStat IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time) 
            {
                d.m_flag = 1;
                return d;
            }
        }

        fishlordBulletHeadOutputStat item = new fishlordBulletHeadOutputStat();
        m_result.Add(item); // "购买", "邮件领取", "直接发送",     "渔场","背包"
        item.m_data[23] = new long[] { 0, 0, 0, 0, 0};
        item.m_data[24] = new long[] { 0, 0, 0, 0, 0};
        item.m_data[25] = new long[] { 0, 0, 0, 0, 0};
        item.m_data[26] = new long[] { 0, 0, 0, 0, 0};
        item.m_data[27] = new long[] { 0, 0, 0, 0, 0};
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamTotalConsume p = (ParamTotalConsume)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.Or(
                                Query.EQ("itemId", 23), 
                                Query.EQ("itemId", 24), 
                                Query.EQ("itemId", 25), 
                                Query.EQ("itemId", 26), 
                                Query.EQ("itemId", 27)
                                );
        IMongoQuery imq4 = Query.GT("addCount",0);
        IMongoQuery imq = Query.And(imq1, imq2, imq3, imq4);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        string[] fields = { "useCount" };
        int[] idList = { 23, 24, 25, 26, 27};
        int i = 0, j = 0;
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_ITEM, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();
            fishlordBulletHeadOutputStat tmp = IsCreate(time);

            tmp.m_time = time;

            if (!data.ContainsKey("itemId") || !data.ContainsKey("addCount") || !data.ContainsKey("reason"))
                continue;

            int id = Convert.ToInt32(data["itemId"]);
            int value = Convert.ToInt32(data["addCount"]);
            int reason = Convert.ToInt32(data["reason"]);

            switch (reason)
            {
                    /////////////////购买
                case 14: //购买捕鱼道具
                case 18: //充值
                    tmp.m_data[id][0] += value;
                    break;

                    ///////////////邮件领取
                case 29: // 领取邮件
                case 54: //后台发邮件
                case 89: // 中级场兑换
                    tmp.m_data[id][1] += value;
                    break;

                    ///////////////直接发
                case 8: //彩券兑换
                case 25: // 签到
                case 32: // 活动  //开服狂欢  //国庆
                case 33: //活动兑换
                case 39://幸运抽奖
                case 41: //每日任务
                case 42: //每周任务
                case 46: //活跃开宝箱
                case 55: //七日礼包
                case 79: //使用道具
                case 82: //龙宫场魔石兑换
                case 91: //空投
                case 92: //碎片兑换
                case 99: //南海
                case 96: //新手任务
                case 100: //鱼雷合成
                case 102://使用轰炸机
                case 103://巨鲨场抽奖
                case 104://巨鲨场斩立决
                case 105://明日礼包
                case 106://追击蟹将
                case 109: //炮台任务
                case 110: //使用新手鱼雷
                case 111: //新手南海寻宝
                    tmp.m_data[id][2] += value;
                    break;
            }

            //使用
            if (tmp.m_flag == 0) 
            {
                IMongoQuery imq_use1 = Query.EQ("genTime", t.Date);

                foreach (int ID in idList) 
                {
                    IMongoQuery imq_use = Query.And(imq_use1, Query.EQ("torpedoId", ID));
                    //渔场
                    List<Dictionary<string, object>> dataList_fish =
                        DBMgr.getInstance().executeQuery(TableName.PUMP_TORPEDO_FISH_USE, dip, imq_use, 0, 0, fields, "genTime", false);
                    if (dataList_fish != null && dataList_fish.Count != 0)
                    {
                        for (j = 0; j < dataList_fish.Count; j++)
                        {
                            tmp.m_data[ID][3] += Convert.ToInt64(dataList_fish[j]["useCount"]);
                        }
                    }

                    //背包
                    List<Dictionary<string, object>> dataList_bag =
                        DBMgr.getInstance().executeQuery(TableName.PUMP_TORPEDO_BAG_USE, dip, imq_use, 0, 0, fields, "genTime", false);
                    if (dataList_bag != null && dataList_bag.Count != 0)
                    {
                        for (j = 0; j < dataList_bag.Count; j++)
                        {
                            tmp.m_data[ID][4] += Convert.ToInt64(dataList_bag[j]["useCount"]);
                        }
                    }
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////
//世界杯大竞猜赛事表
public class QueryWorldCupMatchList : QueryBase 
{
    private List<WorldCupMatchParam> m_result = new List<WorldCupMatchParam>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    //返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public OpRes query(GMUser user) 
    {
        List<Dictionary<string,object>> dataList =
            DBMgr.getInstance().executeQuery(TableName.WORLD_CUP_MATCH_INFO, user.getDbServerID(), DbName.DB_PLAYER, null, 0, 0, null, "matchId", true);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++ ) 
        {
            Dictionary<string,object> data = dataList[i];
            WorldCupMatchParam tmp = new WorldCupMatchParam();
            m_result.Add(tmp);
            tmp.m_id = Convert.ToString(data["_id"]);
            tmp.m_matchId = Convert.ToString(data["matchId"]);
            tmp.m_matchStartTime = Convert.ToDateTime(data["matchStartTime"]).ToLocalTime().ToString("yyyy-MM-dd HH:mm");
            tmp.m_betEndTime = Convert.ToDateTime(data["betEndTime"]).ToLocalTime().ToString("yyyy-MM-dd HH:mm");
            tmp.m_showTime = Convert.ToDateTime(data["showTime"]).ToLocalTime().ToString("yyyy-MM-dd HH:mm");
            tmp.m_matchNameId = Convert.ToInt32(data["matchName"]);
            var matchName = WorldCupGroupCFG.getInstance().getValue(tmp.m_matchNameId);
            if (matchName != null)
                tmp.m_matchName = matchName.m_groupName;

            tmp.m_matchType = Convert.ToInt32(data["matchType"]);

            tmp.m_homeTeamId = Convert.ToInt32(data["homeTeamId"]);
            var homeTeam = WorldCupNationCFG.getInstance().getValue(tmp.m_homeTeamId);
            if (homeTeam != null)
                tmp.m_homeTeam = homeTeam.m_nationName;

            tmp.m_visitTeamId = Convert.ToInt32(data["visitTeamId"]);
            var visitTeam = WorldCupNationCFG.getInstance().getValue(tmp.m_visitTeamId);
            if (visitTeam != null)
                tmp.m_visitTeam = visitTeam.m_nationName;

            tmp.m_homeScore = Convert.ToInt32(data["homeScore"]);
            tmp.m_visitScore = Convert.ToInt32(data["visitScore"]);
            tmp.m_betMaxCount = Convert.ToInt32(data["betMaxCount"]);
        }
        return OpRes.opres_success;
    }
}
//世界杯大竞猜玩家押注统计
public class worldCupMatchPlayerJoinItem 
{
    public int m_matchId;
    public string m_matchTime;
    public int m_matchName;
    public int m_homeTeam;
    public int m_visitTeam;
    public int m_homeBetPlayerCount;
    public long m_homeBet = 0;
    public double m_homeOdds;
    public int m_visitBetPlayerCount;
    public long m_visitBet = 0;
    public double m_visitOdds;
    public int m_drawBetPlayerCount;
    public long m_drawBet = 0;
    public double m_drawOdds;
    public long m_totalBet;
    public long m_rewardOutlay;
    public long m_totalEarn;
    public string m_isSendReward="否";

    public string getEarnRate() 
    {
        if (m_totalBet != 0)
            return Math.Round(m_totalEarn * 1.0 / m_totalBet, 2).ToString();
        return "";
    }

    public string getMatchName(int matchId) 
    {
        string matchName = matchId.ToString();
        var matchInfo = WorldCupGroupCFG.getInstance().getValue(matchId);
        if (matchInfo != null)
            matchName = matchInfo.m_groupName;
        return matchName;
    }

    public string getTeamName(int teamId) 
    {
        string teamName = teamId.ToString();
        var teamInfo = WorldCupNationCFG.getInstance().getValue(teamId);
        if (teamInfo != null)
            teamName = teamInfo.m_nationName;
        return teamName;
    }
}
public class QueryWorldCupMatchPlayerJoin : QueryBase 
{
    private List<worldCupMatchPlayerJoinItem> m_result = new List<worldCupMatchPlayerJoinItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }
    //返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user) 
    {
        DbInfoParam dip1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip2 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
           DBMgr.getInstance().executeQuery(TableName.WORLD_CUP_MATCH_BET_INFO, dip1, null, 0, 0, null, "matchId", true);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++) 
        {
            worldCupMatchPlayerJoinItem tmp = new worldCupMatchPlayerJoinItem();
            m_result.Add(tmp);
            tmp.m_matchId = Convert.ToInt32(dataList[i]["matchId"]);
            if (dataList[i].ContainsKey("rewardOutlay"))
                tmp.m_rewardOutlay = Convert.ToInt64(dataList[i]["rewardOutlay"]);

            if (dataList[i].ContainsKey("homeBet"))
                tmp.m_homeBet = Convert.ToInt64(dataList[i]["homeBet"]);
            if (dataList[i].ContainsKey("homeOdds"))
                tmp.m_homeOdds = Convert.ToDouble(dataList[i]["homeOdds"]);

            if (dataList[i].ContainsKey("visitBet"))
                tmp.m_visitBet = Convert.ToInt64(dataList[i]["visitBet"]);
            if (dataList[i].ContainsKey("visitOdds"))
                tmp.m_visitOdds = Convert.ToDouble(dataList[i]["visitOdds"]);

            if (dataList[i].ContainsKey("drawBet"))
                tmp.m_drawBet = Convert.ToInt64(dataList[i]["drawBet"]);
            if (dataList[i].ContainsKey("drawOdds"))
                tmp.m_drawOdds = Convert.ToDouble(dataList[i]["drawOdds"]);

            tmp.m_totalBet = tmp.m_homeBet + tmp.m_visitBet + tmp.m_drawBet;
            tmp.m_totalEarn = tmp.m_totalBet - tmp.m_rewardOutlay;

            if(dataList[i].ContainsKey("isSendReward"))
            {
                bool isSendReward = Convert.ToBoolean(dataList[i]["isSendReward"]);
                if (isSendReward){
                    tmp.m_isSendReward = "是";
                }else{
                    tmp.m_isSendReward = "否";
                }
            }

            Dictionary<string, object> data_matchInfo = 
                DBMgr.getInstance().getTableData(TableName.WORLD_CUP_MATCH_INFO, "matchId", tmp.m_matchId,dip1);
            if(data_matchInfo != null )
            {
                tmp.m_matchTime = Convert.ToDateTime(data_matchInfo["matchStartTime"]).ToLocalTime().ToString("yyyy-MM-dd HH:mm");
                tmp.m_matchName = Convert.ToInt32(data_matchInfo["matchName"]);
                tmp.m_homeTeam = Convert.ToInt32(data_matchInfo["homeTeamId"]);
                tmp.m_visitTeam = Convert.ToInt32(data_matchInfo["visitTeamId"]);
            }

            Dictionary<string, object> data_matchPlayerBet = 
                DBMgr.getInstance().getTableData(TableName.STAT_WORLD_CUP_MATCH_PLAYER_JOIN,"matchId",tmp.m_matchId,dip2);
            if (data_matchPlayerBet != null)
            {
                if (data_matchPlayerBet.ContainsKey("homeBetPlayerCount"))
                    tmp.m_homeBetPlayerCount = Convert.ToInt32(data_matchPlayerBet["homeBetPlayerCount"]);
                if (data_matchPlayerBet.ContainsKey("visitBetPlayerCount"))
                    tmp.m_visitBetPlayerCount = Convert.ToInt32(data_matchPlayerBet["visitBetPlayerCount"]);
                if (data_matchPlayerBet.ContainsKey("drawBetPlayerCount"))
                    tmp.m_drawBetPlayerCount = Convert.ToInt32(data_matchPlayerBet["drawBetPlayerCount"]);
            } else 
            {
                tmp.m_homeBetPlayerCount = -1;
                tmp.m_visitBetPlayerCount = -1;
                tmp.m_drawBetPlayerCount = -1;
            }
        }
        return OpRes.opres_success;
    }
}

//比武场数据统计
public class StatplayerBw
{
    public string m_time;
    public int m_roomType;
    public int m_ownerId;
    public int m_challengeId;
    public int m_enterDb;
    public int m_matchResult;
    public string getRoomTypeName()
    {
        switch (m_roomType)
        {
            case 1: return "双人竞技";
            case 2: return "挑战赛";
        }
        return m_roomType.ToString();
    }

    public string getLoseWinRes() 
    {
        switch (m_matchResult)
        {
            case 0: return "平";
            case 1: return "赢";
            case 2: return "输";
        }
        return m_matchResult.ToString();
    }
}
public class QueryStatPlayerBw : QueryBase 
{
    private List<StatplayerBw> m_result = new List<StatplayerBw>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_BW, imq, dip);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.PUMP_BW, dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["roomType"]).OrderByDescending(a => a["genTime"]).ToList();

        int i = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatplayerBw tmp = new StatplayerBw();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("roomType"))
                tmp.m_roomType= Convert.ToInt32(data["roomType"]);

            if (data.ContainsKey("ownerId"))
                tmp.m_ownerId = Convert.ToInt32(data["ownerId"]);

            if (data.ContainsKey("challengeId"))
                tmp.m_challengeId = Convert.ToInt32(data["challengeId"]);

            if (data.ContainsKey("enterDb"))
                tmp.m_enterDb = Convert.ToInt32(data["enterDb"]);

            if (data.ContainsKey("matchResult"))
                tmp.m_matchResult = Convert.ToInt32(data["matchResult"]);
        }
        return OpRes.opres_success;
    }
}

//围剿龙王
public class WjlwRechargeRewardItem 
{
    public string m_id;
    public string m_nickname;
    public int m_rewardId;
    public string getRewardName() 
    {
        switch (m_rewardId) {
            case 1: return "一等奖";
            case 2: return "二等奖";
            case 3: return "三等奖";
        }
        return "";
   }
}
public class QueryStatWjlwRechaargeReward : QueryBase 
{
    private List<WjlwRechargeRewardItem> m_result = new List<WjlwRechargeRewardItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_WJLW_Act_DEF_RECHARGE_REWARD, null, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_WJLW_Act_DEF_RECHARGE_REWARD, dip, null, 0, 0, null, "rewardId", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            WjlwRechargeRewardItem tmp = new WjlwRechargeRewardItem();
            m_result.Add(tmp);

            tmp.m_id = Convert.ToString(data["_id"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickname = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("rewardId"))
                tmp.m_rewardId = Convert.ToInt32(data["rewardId"]);
        }
        return OpRes.opres_success;

    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}

//围剿龙王金币玩法统计
public class WjlwGoldEarnStatItem 
{
    public string m_time;
    public long m_income;
    public long m_outlay;
    public int m_expRate = -1;

    public string getEarnRate() 
    {
        if (m_income == 0 && m_outlay == 0)
            return "0";
        if (m_income == 0)
            return "-∞";

        double factGain = (double)(m_income - m_outlay) / m_income;
        return Math.Round(factGain, 3).ToString();
    }

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_WJLW_GOLD_EARN_TURN_INFO;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryStatWjlwGoldEarn : QueryBase 
{
    private List<WjlwGoldEarnStatItem> m_result = new List<WjlwGoldEarnStatItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //当天的在WJLW_ACT_DATA表中查询
        DateTime now = DateTime.Now;
        if(now >= mint && now < maxt && p.m_curPage == 1)
            queryToday(user, imq, now);

        query(user, imq, p);

        if (m_result.Count == 0)
            return OpRes.op_res_not_found_data;

        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes queryToday(GMUser user, IMongoQuery imq, DateTime time)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.WJLW_ACT_DATA, "dataId", 1, null, dip);
        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;

        WjlwGoldEarnStatItem tmp = new WjlwGoldEarnStatItem();
        m_result.Add(tmp);

        tmp.m_time = time.ToLocalTime().ToShortDateString();

        if (data.ContainsKey("goldIncome"))
            tmp.m_income = Convert.ToInt64(data["goldIncome"]);

        if (data.ContainsKey("goldOutlay"))
            tmp.m_outlay = Convert.ToInt64(data["goldOutlay"]);

        if (data.ContainsKey("goldExpRate"))
            tmp.m_expRate = Convert.ToInt32(data["goldExpRate"]);

        return OpRes.opres_success;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_WJLW_PUMP_GOLD_EARN, imq, dip);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.STAT_WJLW_PUMP_GOLD_EARN, 
            dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            WjlwGoldEarnStatItem tmp = new WjlwGoldEarnStatItem();
            m_result.Add(tmp);
            DateTime time = Convert.ToDateTime(data["genTime"]);
            tmp.m_time = time.ToLocalTime().ToShortDateString();

            if (data.ContainsKey("income"))
                tmp.m_income = Convert.ToInt64(data["income"]);

            if (data.ContainsKey("outlay"))
                tmp.m_outlay = Convert.ToInt64(data["outlay"]);
        }
        return OpRes.opres_success;
    }
}

//金币玩法统计每局详情
public class WjlwGoldEarnTurnInfoItem 
{
    public string m_time;
    public int m_income;
    public int m_outlay;
    public int m_winPlayerId;
    public int m_equipCount;
    public long m_turnId;
    public bool m_isRecv = false;

    public string getEarnRate()
    {
        if (m_income == 0 && m_outlay == 0)
            return "0";
        if (m_income == 0)
            return "-∞";

        double factGain = (double)(m_income - m_outlay) / m_income;
        return Math.Round(factGain, 3).ToString();
    }

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "turnId";
        uParam.m_value = m_turnId.ToString();
        uParam.m_url = DefCC.ASPX_WJLW_GOLD_BET_PLAYER_LIST;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time.Replace(' ','+'));
        return Tool.genHyperlink(uParam);
    }
}
public class QueryStatWjlwGoldEarnTurnInfo : QueryBase 
{
    private List<WjlwGoldEarnTurnInfoItem> m_result = new List<WjlwGoldEarnTurnInfoItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_WJLW_PUMP_GOLD_TURN_INFO, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_WJLW_PUMP_GOLD_TURN_INFO, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "startTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            WjlwGoldEarnTurnInfoItem tmp = new WjlwGoldEarnTurnInfoItem();
            m_result.Add(tmp);
            if (data.ContainsKey("startTime"))
                tmp.m_time = Convert.ToDateTime(data["startTime"]).ToLocalTime().ToString();

            tmp.m_turnId = Convert.ToInt64(data["turnId"]);

            if (data.ContainsKey("income"))
                tmp.m_income = Convert.ToInt32(data["income"]);

            if (data.ContainsKey("outlay"))
                tmp.m_outlay = Convert.ToInt32(data["outlay"]);

            if (data.ContainsKey("winPlayerId"))
                tmp.m_winPlayerId = Convert.ToInt32(data["winPlayerId"]);

            if (data.ContainsKey("equipCount"))
                tmp.m_equipCount = Convert.ToInt32(data["equipCount"]);

            //是否领取
            tmp.m_isRecv = DBMgr.getInstance().keyExists(TableName.STAT_WJLW_PUMP_GOLD_RECV_INFO, 
                "turnId", tmp.m_turnId, user.getDbServerID(), DbName.DB_PUMP);
        }
        return OpRes.opres_success;
    }
}
//每局下注玩家列表
public class WjlwGoldBetPlayerItem 
{
    public string m_time;
    public int m_playerId;
    public string m_nickName;
    public int m_equipCount;
}
public class QueryStatWjlwGoldBetPlayerList : QueryBase 
{
    private List<WjlwGoldBetPlayerItem> m_result = new List<WjlwGoldBetPlayerItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = Query.EQ("turnId", Convert.ToInt64(p.m_param));
        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_WJLW_PUMP_GOLD_BET_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_WJLW_PUMP_GOLD_BET_PLAYER, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            WjlwGoldBetPlayerItem tmp = new WjlwGoldBetPlayerItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("equipCount"))
                tmp.m_equipCount = Convert.ToInt32(data["equipCount"]);
        }
        return OpRes.opres_success;
    }
}
//围剿龙王付费玩法统计
public class WjlwRechargeEarnItem 
{
    public string m_time;
    public long m_income;
    public long m_outlay;

    public string getEarnRate()
    {
        if (m_income == 0 && m_outlay == 0)
            return "0";
        if (m_income == 0)
            return "-∞";

        double factGain = (double)(m_income - m_outlay) / m_income;
        return Math.Round(factGain, 3).ToString();
    }

    public string getDetail1()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time.ToString();
        uParam.m_url = DefCC.ASPX_WJLW_RECHARGE_WIN_INFO;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }

    public string getDetail2()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time.ToString();
        uParam.m_url = DefCC.ASPX_WJLW_RECHARGE_BET_PLAYER;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryStatWjlwRechargeEarn : QueryBase 
{
    private List<WjlwRechargeEarnItem> m_result = new List<WjlwRechargeEarnItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_WJLW_PUMP_RECHARGE_EARN, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_WJLW_PUMP_RECHARGE_EARN, dip, imq, 
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            WjlwRechargeEarnItem tmp = new WjlwRechargeEarnItem();
            m_result.Add(tmp);
            DateTime time = Convert.ToDateTime(data["genTime"]);
            tmp.m_time = time.ToLocalTime().ToShortDateString();

            if (data.ContainsKey("income"))
                tmp.m_income = Convert.ToInt64(data["income"]);
            if (data.ContainsKey("outlay"))
                tmp.m_outlay = Convert.ToInt64(data["outlay"]);
        }
        return OpRes.opres_success;
    }
}
//获奖详情
public class WjlwRechargeWinInfoItem 
{
    public int m_rewardId;
    public string m_playerId;
    public string m_nickName;
    public int m_equipCount;
    public int m_vipLevel;
    public bool m_isRobot = true;
    public bool m_isRecv = false;
    public string m_rewardItem;
    public string getRewardName() 
    {
        switch (m_rewardId) {
            case 1: return "一等奖";
            case 2: return "二等奖";
            case 3: return "三等奖";
            case 4: return "四等奖";
        }
        return m_rewardId.ToString();
    }
}
public class QueryStatWjlwRechargeWinInfo : QueryBase 
{
    private List<WjlwRechargeWinInfoItem> m_result = new List<WjlwRechargeWinInfoItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_WJLW_PUMP_RECHARGE_WIN_INFO, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_WJLW_PUMP_RECHARGE_WIN_INFO, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "rewardId", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            WjlwRechargeWinInfoItem tmp = new WjlwRechargeWinInfoItem();
            m_result.Add(tmp);

            if (data.ContainsKey("rewardId"))
                tmp.m_rewardId = Convert.ToInt32(data["rewardId"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("equipCount"))
                tmp.m_equipCount = Convert.ToInt32(data["equipCount"]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);

            if (data.ContainsKey("isRobot") && Convert.ToInt32(tmp.m_playerId) >= 0)
                tmp.m_isRobot = Convert.ToBoolean(data["isRobot"]);

            if (data.ContainsKey("itemId"))
            {
                int itemId = Convert.ToInt32(data["itemId"]);
                int count = 0;
                if(data.ContainsKey("itemCount"))
                    count = Convert.ToInt32(data["itemCount"]);

                ItemCFGData da = ItemCFG.getInstance().getValue(itemId);
                string strName = "";
                if (da != null)
                    strName = da.m_itemName;
                tmp.m_rewardItem = strName + "：" + count;
            }

            //是否领取
            tmp.m_isRecv = DBMgr.getInstance().keyExists(TableName.STAT_WJLW_PUMP_RECHARGE_RECV_INFO,
                "playerId", Convert.ToInt32(tmp.m_playerId), user.getDbServerID(), DbName.DB_PUMP);
        }
        return OpRes.opres_success;
    }
}
//下注详情
public class WjlwRechargeBetPlayerItem 
{
    public string m_time;
    public string m_playerId;
    public string m_nickName;
    public int m_equipCount;
}
public class QueryStatWjlwRechargeBetPlayer : QueryBase 
{
    private List<WjlwRechargeBetPlayerItem> m_result = new List<WjlwRechargeBetPlayerItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_WJLW_PUMP_RECHARGE_BET_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_WJLW_PUMP_RECHARGE_BET_PLAYER, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            WjlwRechargeBetPlayerItem tmp = new WjlwRechargeBetPlayerItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("equipCount"))
                tmp.m_equipCount = Convert.ToInt32(data["equipCount"]);
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//弹头礼包统计
public class StatBulletHeadGiftItem 
{
    public string m_time;
    public Dictionary<int, StatBulletHeadGiftItemList> m_data = new Dictionary<int, StatBulletHeadGiftItemList>();
}
public class StatBulletHeadGiftItemList
{
    public string m_time;
    public int m_giftId;
    public int m_saleCount;
    public int m_sendItemId;
    public int m_sendItemCount;
    public int m_rechargeCount;

    public string getSendItemName()
    {
        ItemCFGData da = ItemCFG.getInstance().getValue(m_sendItemId);
        if (da != null)
            return da.m_itemName;
        return m_sendItemId.ToString();
    }

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "boxId";
        uParam.m_value = m_giftId.ToString();
        uParam.m_url = DefCC.ASPX_STAT_BULLET_HEAD_GIFT_DEATIL;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time.Replace(' ', '+'));
        return Tool.genHyperlink(uParam);
    }
}
public class QueryStatBulletHeadGift : QueryBase 
{
    private List<StatBulletHeadGiftItem> m_result = new List<StatBulletHeadGiftItem>();
    public StatBulletHeadGiftItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatBulletHeadGiftItem item = new StatBulletHeadGiftItem();
        item.m_time = time;
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_BULLET_HEAD_GIFT, imq, dip);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.STAT_BULLET_HEAD_GIFT, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["giftId"]).OrderByDescending(a => a["genTime"]).ToList();
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime time = Convert.ToDateTime(data["genTime"]);
            string m_time = time.ToLocalTime().ToShortDateString();
            StatBulletHeadGiftItem tmp1 = IsCreate(m_time);

            if (data.ContainsKey("giftId"))
            {
                StatBulletHeadGiftItemList tmp = new StatBulletHeadGiftItemList();
                tmp.m_time = m_time;
                tmp.m_giftId = Convert.ToInt32(data["giftId"]);

                //添加
                tmp1.m_data.Add(tmp.m_giftId,tmp);

                

                DateTime dt1 = Convert.ToDateTime(m_time);
                IMongoQuery imq_1 = Query.And(Query.EQ("giftId", tmp.m_giftId), Query.EQ("genTime", dt1));
                tmp.m_rechargeCount = Convert.ToInt32(DBMgr.getInstance().getRecordCount(TableName.STAT_BULLET_HEAD_GIFT_PLAYER, imq_1, dip));

                if (data.ContainsKey("saleCount"))
                    tmp.m_saleCount = Convert.ToInt32(data["saleCount"]);

                if (data.ContainsKey("sendItemId"))
                    tmp.m_sendItemId = Convert.ToInt32(data["sendItemId"]);

                if (data.ContainsKey("sendItemCount"))
                    tmp.m_sendItemCount = Convert.ToInt32(data["sendItemCount"]);
            }
        }
        return OpRes.opres_success;
    }
}
//弹头礼包统计详情
public class StatBulletHeadGiftPlayerItem 
{
    public int m_giftId;
    public string m_playerId;
    public int m_gainItemCount;

    public string getGiftName()
    {
        switch (m_giftId)
        {
            case 104: return "弹头礼包3元";
            case 105: return "弹头礼包30元";
            case 106: return "弹头礼包198元";
            case 107: return "弹头礼包648元";
        }
        return m_giftId.ToString();
    }
}
public class QueryStatBulletHeadGiftPlayer : QueryBase 
{
    private List<StatBulletHeadGiftPlayerItem> m_result = new List<StatBulletHeadGiftPlayerItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2,Query.EQ("giftId",p.m_op));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_BULLET_HEAD_GIFT_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_BULLET_HEAD_GIFT_PLAYER, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "giftId", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatBulletHeadGiftPlayerItem tmp = new StatBulletHeadGiftPlayerItem();
            m_result.Add(tmp);

            if (data.ContainsKey("giftId"))
                tmp.m_giftId = Convert.ToInt32(data["giftId"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("gainItemCount"))
                tmp.m_gainItemCount = Convert.ToInt32(data["gainItemCount"]);
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//微信公众号签到统计
public class wechatRecvRewardItem 
{
    public string m_time;
    public int m_day1;
    public int m_day2;
    public int m_day3;
    public int m_day4;
    public int m_day5;
    public int m_day6;
    public int m_day7;
    public int m_total;
    public int m_wechatTotal;
}
public class QueryStatWechatRecvReward : QueryBase 
{
    private List<wechatRecvRewardItem> m_result = new List<wechatRecvRewardItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_WECHAT_RECV_STAT, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PLAYER_WECHAT_RECV_STAT, dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            wechatRecvRewardItem tmp = new wechatRecvRewardItem();
            m_result.Add(tmp);
            DateTime time = Convert.ToDateTime(data["genTime"]);
            tmp.m_time = time.ToLocalTime().ToShortDateString();

            if (data.ContainsKey("1dayRecvCount"))
                tmp.m_day1 = Convert.ToInt32(data["1dayRecvCount"]);
            if (data.ContainsKey("2dayRecvCount"))
                tmp.m_day2 = Convert.ToInt32(data["2dayRecvCount"]);
            if (data.ContainsKey("3dayRecvCount"))
                tmp.m_day3 = Convert.ToInt32(data["3dayRecvCount"]);
            if (data.ContainsKey("4dayRecvCount"))
                tmp.m_day4 = Convert.ToInt32(data["4dayRecvCount"]);
            if (data.ContainsKey("5dayRecvCount"))
                tmp.m_day5 = Convert.ToInt32(data["5dayRecvCount"]);
            if (data.ContainsKey("6dayRecvCount"))
                tmp.m_day6 = Convert.ToInt32(data["6dayRecvCount"]);
            if (data.ContainsKey("7dayRecvCount"))
                tmp.m_day7 = Convert.ToInt32(data["7dayRecvCount"]);

            tmp.m_total = tmp.m_day1 + tmp.m_day2 + tmp.m_day3 + tmp.m_day4 + tmp.m_day5 + tmp.m_day6 + tmp.m_day7;

            IMongoQuery imq_1 = Query.EQ("genTime", time);
            tmp.m_wechatTotal = Convert.ToInt32(DBMgr.getInstance().getRecordCount(TableName.PLAYER_OPENID, imq_1, dip));
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//活动幸运宝箱
public class StatPanicBoxItem 
{
    public string m_time;
    public int m_boxType;
    public int m_lotteryCount;
    public int m_lotteryPerson;
    public Dictionary<int, int> m_reward = new Dictionary<int, int>();

    public string getBoxTypeName() 
    {
        switch(m_boxType)
        {
            case 1: return "免费宝箱";
            case 2: return "6元宝箱";
            case 3: return "68元宝箱";
        }
        return m_boxType.ToString();
    }

    public string getExParam(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "boxId";
        uParam.m_value = m_boxType.ToString();
        uParam.m_url = DefCC.ASPX_PANIC_BOX_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time);
        return Tool.genHyperlink(uParam);
    }

    //奖励详情
    public string getAwardDetail(int index) 
    {
        PanicBoxInfo awardInfo = PanicBoxCFG.getInstance().getValue(m_boxType);
        if (awardInfo != null)
        {
            int reward = m_reward[index];

            var arr_awardItem = awardInfo.m_awardsItemIds.Split(',');
            int itemId = Convert.ToInt32(arr_awardItem[index]);
            ItemCFGData itemCfg = ItemCFG.getInstance().getValue(itemId);
            string itemName = "";
            if(itemCfg!=null)
                itemName = itemCfg.m_itemName;

            var arr_awardCount = awardInfo.m_awardsItemCounts.Split(',');
            int itemCount = Convert.ToInt32(arr_awardCount[index]);

            return reward * itemCount + itemName;
        }
        return "";
    }
}
public class QueryStatPanicBox : QueryBase 
{
    private List<StatPanicBoxItem> m_result = new List<StatPanicBoxItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.PUMP_PANIC_BOX, dip, imq, 0, 0, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["boxId"]).OrderByDescending(a => a["genTime"]).ToList();

        int i = 0, k = 0;

        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatPanicBoxItem tmp = new StatPanicBoxItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            tmp.m_boxType = Convert.ToInt32(data["boxId"]);

            if (data.ContainsKey("lotteryCount"))
                tmp.m_lotteryCount = Convert.ToInt32(data["lotteryCount"]);
            if (data.ContainsKey("lotteryPerson"))
                tmp.m_lotteryPerson = Convert.ToInt32(data["lotteryPerson"]);

            for (k = 0; k<8; k++) 
            {
                string reward = string.Format("reward_{0}",k);

                if (data.ContainsKey(reward))
                    tmp.m_reward[k] = Convert.ToInt32(data[reward]);
            }
        }
        return OpRes.opres_success;
    }
}

//详情
public class PanicBoxDetail:StatPanicBoxItem
{
    public string m_playerId;
}
public class QueryStatPanicBoxDetail : QueryBase
{
    private List<PanicBoxDetail> m_result = new List<PanicBoxDetail>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("boxId",p.m_op) );

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq,ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_PANIC_BOX_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_PANIC_BOX_PLAYER, dip, imq, 
             (param.m_curPage-1)*param.m_countEachPage, param.m_countEachPage, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            PanicBoxDetail tmp = new PanicBoxDetail();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            tmp.m_boxType = Convert.ToInt32(data["boxId"]);
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            for (k = 0; k < 8; k++)
            {
                string reward = string.Format("reward_{0}", k);
                if (data.ContainsKey(reward))
                    tmp.m_reward[k] = Convert.ToInt32(data[reward]);
            }
        }
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////////////
//国庆中秋快乐排行榜统计
public class StatRankItem 
{
    public int m_rankId;
    public string m_playerId;
    public string m_nickName;
    public int m_moonCakeCount;
    public int m_fishLevel;
    public int m_vipLevel;
    public string m_isRobot;

    public string fishLevelName() {
        string fishLevelName = "";

        var fishLevel = Fish_LevelCFG.getInstance().getValue(m_fishLevel);
        if (fishLevel != null)
            fishLevelName = fishLevel.m_openRate.ToString();

        return fishLevelName;
    }
}
public class QueryJinQiuNationalDayActRankStat : QueryBase 
{
    private List<StatRankItem> m_result = new List<StatRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        string table = "";
        IMongoQuery imq = null;
        if (p.m_param == "0") //当前活动
        {
            table = TableName.STAT_FISHLORD_NATIONAL_DAY_ACTIVITY_2018;
            imq = Query.GT("gainMoonCakeCount",0);
            return query1(user,table,imq);
        }
        else
        {
            table = TableName.STAT_PUMP_NATIONAL_DAY_2018_RANk;
            if (p.m_param == "1")
            {
                imq = Query.EQ("actId", 181221);
            }
            else  if(p.m_param == "2")
            {
                imq = Query.EQ("actId", 181231);
            }
            return query2(user, imq, table);
        }
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query1(GMUser user,string table,IMongoQuery imq) //期间
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(table, dip, imq, 0, 50, null, "gainMoonCakeCount", false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a =>
            {
                if (a.ContainsKey("gainTime")){
                    return a["gainTime"]; 
                }else
                {
                    return DateTime.Now;
                }
            }).OrderByDescending(a => a["gainMoonCakeCount"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatRankItem tmp = new StatRankItem();
            m_result.Add(tmp);

            tmp.m_rankId = i + 1;
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("gainMoonCakeCount"))
                tmp.m_moonCakeCount = Convert.ToInt32(data["gainMoonCakeCount"]);

            if (data.ContainsKey("fishLevel"))
                tmp.m_fishLevel = Convert.ToInt32(data["fishLevel"]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);

            if (data.ContainsKey("isRobot"))
                tmp.m_isRobot = Convert.ToInt32(data["isRobot"]) == 0 ? "否" : "是";
        }
        return OpRes.opres_success;
    }

    private OpRes query2(GMUser user, IMongoQuery imq,string table)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(table, dip, imq, 0, 0, null, "rank", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatRankItem tmp = new StatRankItem();
            m_result.Add(tmp);

            tmp.m_rankId = Convert.ToInt32(data["rank"]);
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("gainMoonCakeCount"))
                tmp.m_moonCakeCount = Convert.ToInt32(data["gainMoonCakeCount"]);

            if (data.ContainsKey("fishLevel"))
                tmp.m_fishLevel = Convert.ToInt32(data["fishLevel"]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);

            if (data.ContainsKey("isRobot"))
                tmp.m_isRobot = Convert.ToInt32(data["isRobot"]) == 0 ? "否" : "是";
        }
        return OpRes.opres_success;
    }
}

//抽奖统计
public class StatLotteryItem 
{
    public string m_time;
    public int m_lotteryType;
    public int m_lotteryCount;
    public int m_lotteryPerson;
    public Dictionary<int, int> m_reward = new Dictionary<int, int>();

    public string getLotteryTypeName()
    {
        string typeName = "";
        switch (m_lotteryType)
        {
            case 1: typeName = "少月饼抽奖"; break;
            case 2: typeName = "多月饼抽奖"; break;
        }
        return typeName;
    }

    public long getLotteryAward(int index)  //个数*对应金币
    {
        PanicBoxInfo awardInfo = FishNationDay2018LotteryCFG.getInstance().getValue(m_lotteryType);

        int gold_excg = 1;
        if (awardInfo != null)
        {
            var arr_awardCount = awardInfo.m_awardsItemCounts.Split(',');
            var arr_awardItem = awardInfo.m_awardsItemIds.Split(',');

            switch(Convert.ToInt32(arr_awardItem[index]))
            {
                case 14: gold_excg = 5000; break;
                case 113: gold_excg = 50000; break;
                case 114: gold_excg = 500000; break;
                case 115: gold_excg = 1000000; break;
                case 116: gold_excg = 2500000; break;
                case 117: gold_excg = 250000; break;
            }

            long itemCount = Convert.ToInt32(arr_awardCount[index]) * gold_excg;
            return itemCount;
        }
        return 0;
    }

    public string getExParam(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "lotteryId";
        uParam.m_value = m_lotteryType.ToString();
        uParam.m_url = DefCC.ASPX_JINQIU_NATIONALDAY_ACT_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time);
        return Tool.genHyperlink(uParam);
    }
}

public class StatLotteryList 
{
    public int m_flag = 0;
    public string m_time;
    public List<StatLotteryItem> m_data = new List<StatLotteryItem>();
    public long m_goldIncome;
    public int m_dropCount;
}

public class QueryJinQiuNationalDayActLotteryStat : QueryBase 
{
    private List<StatLotteryList> m_result = new List<StatLotteryList>();
    public string[] m_fields = new string[] { "goldIncome","dropCount"};
    public StatLotteryList IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag++;
                return d;
            }
        }

        StatLotteryList item = new StatLotteryList();
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_param, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_NATIONAL_DAY_2018_LOTTERY, dip, imq, 0, 0, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["lotteryId"]).OrderByDescending(a => a["genTime"]).ToList();

        int i = 0, k = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();

            StatLotteryList tmp = IsCreate(time);
            tmp.m_time = time;

            StatLotteryItem item = new StatLotteryItem();
            tmp.m_data.Add(item);

            item.m_time = time;
            item.m_lotteryType = Convert.ToInt32(data["lotteryId"]);

            if (data.ContainsKey("lotteryCount"))
                item.m_lotteryCount = Convert.ToInt32(data["lotteryCount"]);
            if (data.ContainsKey("lotteryPerson"))
                item.m_lotteryPerson = Convert.ToInt32(data["lotteryPerson"]);

            for (k = 0; k < 9; k++)
            {
                string reward = string.Format("reward_{0}", k);
                if (data.ContainsKey(reward))
                    item.m_reward[k] = Convert.ToInt32(data[reward]);
            }

            if(tmp.m_flag == 0)
            {
                IMongoQuery sq = Query.EQ("genTime", t);
                List<Dictionary<string, object>> goldList =
                            DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_NATIONAL_DAY_2018_DROP, dip, sq, 0, 0, m_fields, "roomId", true);
                if (goldList != null) 
                {
                    for (j = 0; j < goldList.Count; j++) 
                    {
                        if (goldList[j].ContainsKey("goldIncome"))
                            tmp.m_goldIncome += Convert.ToInt64(goldList[j]["goldIncome"]);

                        if (goldList[j].ContainsKey("dropCount"))
                            tmp.m_dropCount += Convert.ToInt32(goldList[j]["dropCount"]);
                    }
                }
            }
        }
        return OpRes.opres_success;
    }
}

//抽奖统计详情
public class JinQiuNationalDayDetail : PanicBoxDetail 
{
    public string getLotteryTypeName(int key)
    {
        string typeName = "";
        switch (key)
        {
            case 1: typeName = "少月饼抽奖"; break;
            case 2: typeName = "多月饼抽奖"; break;
        }
        return typeName;
    }

    //奖励详情
    public string getLotteryAwardDetail(int index)
    {
        PanicBoxInfo awardInfo = FishNationDay2018LotteryCFG.getInstance().getValue(m_boxType);
        if (awardInfo != null)
        {
            int reward = m_reward[index];

            var arr_awardItem = awardInfo.m_awardsItemIds.Split(',');
            int itemId = Convert.ToInt32(arr_awardItem[index]);
            ItemCFGData itemCfg = ItemCFG.getInstance().getValue(itemId);
            string itemName = "";
            if (itemCfg != null)
                itemName = itemCfg.m_itemName;

            var arr_awardCount = awardInfo.m_awardsItemCounts.Split(',');
            int itemCount = Convert.ToInt32(arr_awardCount[index]);

            return reward * itemCount + itemName;
        }
        return "";
    }
}
public class QueryJinQiuNationalDayActLotteryDetail : QueryBase 
{
    private List<JinQiuNationalDayDetail> m_result = new List<JinQiuNationalDayDetail>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("lotteryId", p.m_op));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_NATIONAL_DAY_2018_LOTTERY_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_NATIONAL_DAY_2018_LOTTERY_PLAYER, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            JinQiuNationalDayDetail tmp = new JinQiuNationalDayDetail();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            tmp.m_boxType = Convert.ToInt32(data["lotteryId"]);
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            for (k = 0; k < 9; k++)
            {
                string reward = string.Format("reward_{0}", k);
                if (data.ContainsKey(reward))
                    tmp.m_reward[k] = Convert.ToInt32(data[reward]);
            }
        }
        return OpRes.opres_success;
    }
}
//中秋国庆玩家月饼数量查看
public class QueryJinQiuNationalDayActCtrl : QueryBase 
{
    static string[] m_fields = { "playerId", "nickName", "gainMoonCakeCount","gainTime"};

    protected List<FishlordBaojinScoreParamItem> m_result = new List<FishlordBaojinScoreParamItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        int playerId = 0;
        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_need_at_least_one_cond;

        if (!int.TryParse(p.m_param, out playerId))
            return OpRes.op_res_param_not_valid;

        playerId = Convert.ToInt32(p.m_param);
        IMongoQuery imq = Query.EQ("playerId", playerId);
        m_result.Clear();
        return query(user, TableName.STAT_FISHLORD_NATIONAL_DAY_ACTIVITY_2018, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user) { return m_result; }

    protected OpRes query(GMUser user, string tableName, IMongoQuery param)
    {
        Dictionary<string, object> dataList =
            DBMgr.getInstance().getTableData(tableName, user.getDbServerID(), DbName.DB_GAME, param, m_fields);
        if (dataList == null)
            return OpRes.op_res_not_found_data;
        FishlordBaojinScoreParamItem info = new FishlordBaojinScoreParamItem();
        m_result.Add(info);
        info.m_playerId = Convert.ToInt32(dataList["playerId"]);
        if (dataList.ContainsKey("nickName"))
            info.m_nickName = Convert.ToString(dataList["nickName"]);

        if (dataList.ContainsKey("gainMoonCakeCount"))
            info.m_weekMaxScore = Convert.ToInt32(dataList["gainMoonCakeCount"]);

        if (dataList.ContainsKey("gainTime"))
            info.m_time = Convert.ToDateTime(dataList["gainTime"]).ToLocalTime().ToString();
        return OpRes.opres_success;
    }
}
//场次统计
public class ActRoomStatItem 
{
    public int m_roomId;
    public long m_goldIncome;
    public int m_killCount;
}
public class ActRoomStatList
{
    public string m_time;
    public Dictionary<int, ActRoomStatItem> m_data = new Dictionary<int, ActRoomStatItem>();
}
public class QueryJinQiuNationalDayActRoomStat : QueryBase 
{
    private List<ActRoomStatList> m_result = new List<ActRoomStatList>();
    public ActRoomStatList IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        ActRoomStatList item = new ActRoomStatList();
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_param, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_NATIONAL_DAY_ROOM_STAT, dip, imq, 0, 0, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["roomId"]).OrderByDescending(a => a["genTime"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();

            ActRoomStatList tmp = IsCreate(time);
            tmp.m_time = time;

            ActRoomStatItem item = new ActRoomStatItem();
            
            if(data.ContainsKey("roomId"))
            {
                item.m_roomId = Convert.ToInt32(data["roomId"]);
                if (data.ContainsKey("goldIncome"))
                    item.m_goldIncome = Convert.ToInt64(data["goldIncome"]);

                if (data.ContainsKey("killCount"))
                    item.m_killCount = Convert.ToInt32(data["killCount"]);

                tmp.m_data.Add(item.m_roomId,item);
            }
        }
        return OpRes.opres_success;
    }
}
//任务统计
public class ActTaskItem 
{
    public string m_time;
    public int m_t1Finish;
    public int m_t2Finish;
    public int m_t3Finish;
    public int m_t1Receive;
    public int m_t2Receive;
    public int m_t3Receive;

    public string getExParam(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_JINQIU_NATIONALDAY_ACT_TASK;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryJinQiuNationalDayActTaskStat : QueryBase 
{
    private List<ActTaskItem> m_result = new List<ActTaskItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_param, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_NATIONAL_DAY_2018_TASK, dip, imq, 0, 0, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ActTaskItem tmp = new ActTaskItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("t1Finish"))
                tmp.m_t1Finish = Convert.ToInt32(data["t1Finish"]);
            if (data.ContainsKey("t1Receive"))
                tmp.m_t1Receive = Convert.ToInt32(data["t1Receive"]);

            if (data.ContainsKey("t2Finish"))
                tmp.m_t2Finish = Convert.ToInt32(data["t2Finish"]);
            if (data.ContainsKey("t2Receive"))
                tmp.m_t2Receive = Convert.ToInt32(data["t2Receive"]);

            if (data.ContainsKey("t3Finish"))
                tmp.m_t3Finish = Convert.ToInt32(data["t3Finish"]);
            if (data.ContainsKey("t3Receive"))
                tmp.m_t3Receive = Convert.ToInt32(data["t3Receive"]);
        }
        return OpRes.opres_success;
    }
}
//任务统计详情
public class ActTaskDetailItem
{
    public int m_pao;
    public int m_t1Finish;
    public int m_t1Receive;

    public int m_t2Finish;
    public int m_t2Receive;

    public int m_t3Finish;
    public int m_t3Receive;
}
public class ActTaskDetailList 
{
    public string m_time;
    public Dictionary<int, ActTaskDetailItem> m_data = new Dictionary<int, ActTaskDetailItem>();
}
public class QueryJinQiuNationalDayActTaskDeatil : QueryBase 
{
    private List<ActTaskDetailList> m_result = new List<ActTaskDetailList>();
    public ActTaskDetailList IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        ActTaskDetailList item = new ActTaskDetailList();
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_NATIONAL_DAY_2018_PLAYER_TASK, dip, imq, 0, 0, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["p"]).OrderByDescending(a => a["genTime"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();

            ActTaskDetailList tmp = IsCreate(time);
            tmp.m_time = time;

            ActTaskDetailItem item = new ActTaskDetailItem();

            if (data.ContainsKey("p"))
            {
                item.m_pao = Convert.ToInt32(data["p"]);

                if (data.ContainsKey("t1Finish"))
                    item.m_t1Finish = Convert.ToInt32(data["t1Finish"]);

                if (data.ContainsKey("t1Receive"))
                    item.m_t1Receive = Convert.ToInt32(data["t1Receive"]);

                if (data.ContainsKey("t2Finish"))
                    item.m_t2Finish = Convert.ToInt32(data["t2Finish"]);

                if (data.ContainsKey("t2Receive"))
                    item.m_t2Receive = Convert.ToInt32(data["t2Receive"]);

                if (data.ContainsKey("t3Finish"))
                    item.m_t3Finish = Convert.ToInt32(data["t3Finish"]);

                if (data.ContainsKey("t3Receive"))
                    item.m_t3Receive = Convert.ToInt32(data["t3Receive"]);

                tmp.m_data.Add(item.m_pao, item);
            }
        }
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////////////
//屠龙榜
public class kdActRankItem 
{
    public int m_rank;
    public string m_nickName;
    public int m_playerId;
    public int m_gaindb;
    public int m_useCallup;
    public long m_costGold;
    public string m_time;
}
public class QueryStatKdActRank : QueryBase 
{
    private List<kdActRankItem> m_result = new List<kdActRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        IMongoQuery imq = null;
        //====================选择数据表==================
        string table = "";
        int db = 0;
        if (p.m_op == 1) { //周榜
            table = TableName.STAT_PUMP_KD_WEEK_RANK;
            db = DbName.DB_PUMP;
            imq = Query.Exists("genTime");
            return query(user, imq, db, table,"gaindb","costGold","useCallUp", "genTime","rank");
        }
        else if (p.m_op == 0) 
        {
            if (p.m_param == "1") //当前日榜
            {
                table = TableName.STAT_PUMP_KD_ACTIVITY;
                db = DbName.DB_GAME;
                imq = Query.GT("todayGaindb",0);
                return query(user, imq, db, table,"todayGaindb","todayCostGold","todayUseCallup","todayGaindb","todayGainTime");
            }
            else//昨日榜
            {
                table = TableName.STAT_PUMP_KD_HISTORY_RANK;
                db = DbName.DB_GAME;

                if (string.IsNullOrEmpty(p.m_time))
                    return OpRes.op_res_need_at_least_one_cond;

                DateTime maxt = DateTime.Now;
                bool res = Tool.splitTimeStr(p.m_time, ref maxt,1);
                if (!res)
                    return OpRes.op_res_time_format_error;

                imq = Query.EQ("genTime", BsonValue.Create(maxt));
                return query(user, imq, db, table,"gaindb","costGold","useCallup","rank");
            }    
        }
        return OpRes.op_res_failed;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, int db, 
        string table, string gaindb, string costGold,string useCallup, string param1, string param2 = null)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), db, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list = DBMgr.getInstance().executeQuery(table, dip, imq, 0, 50, null,param1, true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list;
        if(param2!=null) //当前日排行 param2时间
            dataList = data_list.OrderBy(a =>
            { 
                if (a.ContainsKey(param2)) { 
                    return a[param2]; 
                } else { 
                    return DateTime.Now; 
                } 
            }).OrderByDescending(a => a[param1]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            kdActRankItem tmp = new kdActRankItem();
            m_result.Add(tmp);

            if (param1 == "genTime" && data.ContainsKey("genTime"))
                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("rank"))
            {
                tmp.m_rank = Convert.ToInt32(data["rank"]);
            }
            else { 
                tmp.m_rank = (i+1);
            }

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey(gaindb))
                tmp.m_gaindb = Convert.ToInt32(data[gaindb]);

            if (data.ContainsKey(useCallup))
                tmp.m_useCallup = Convert.ToInt32(data[useCallup]);

            if (data.ContainsKey(costGold))
                tmp.m_costGold = Convert.ToInt64(data[costGold]);
        }
        return OpRes.opres_success;
    }
}
//屠龙日榜
public class QueryStatKdActDayRank : QueryBase 
{
    private List<kdActRankItem> m_result = new List<kdActRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list = DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_KD_ACTIVITY, dip, null, 0, 50, null, "todayGaindb", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a =>
        {
            if (a.ContainsKey("todayGainTime"))
            {
                return a["todayGainTime"];
            }
            else
            {
                return DateTime.Now;
            }
        }).OrderByDescending(a => a["todayGaindb"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            kdActRankItem tmp = new kdActRankItem();
            m_result.Add(tmp);

            tmp.m_rank = (i + 1);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("todayGaindb"))
                tmp.m_gaindb = Convert.ToInt32(data["todayGaindb"]);

            if (data.ContainsKey("todayUseCallup"))
                tmp.m_useCallup = Convert.ToInt32(data["todayUseCallup"]);

            if (data.ContainsKey("todayCostGold"))
                tmp.m_costGold = Convert.ToInt64(data["todayCostGold"]);
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//转盘鱼排行
public class QueryStatTurnTableFishRank : QueryBase
{
    private List<StatRankItem> m_result = new List<StatRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        string table = "";
        IMongoQuery imq = null;
        if (p.m_param == "0") //当前活动
        {
            table = TableName.STAT_TURN_TABLE_FISH_ACTIVITY;
            imq = Query.GT("gainGold", 0);
            return query1(user, table, imq);
        }
        else
        {
            table = TableName.STAT_PUMP_TFISH_RANK;
            return query2(user, table);
        }
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query1(GMUser user, string table, IMongoQuery imq) //当前排行
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(table, dip, imq, 0, 50, null, "gainGold", false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a =>
        {
            if (a.ContainsKey("gainTime"))
            {
                return a["gainTime"];
            }
            else
            {
                return DateTime.Now;
            }
        }).OrderByDescending(a => a["gainGold"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatRankItem tmp = new StatRankItem();
            m_result.Add(tmp);

            tmp.m_rankId = i + 1;
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("gainGold"))
                tmp.m_moonCakeCount = Convert.ToInt32(data["gainGold"]);

            if (data.ContainsKey("fishLevel"))
                tmp.m_fishLevel = Convert.ToInt32(data["fishLevel"]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);

            if (data.ContainsKey("isRobot"))
                tmp.m_isRobot = Convert.ToInt32(data["isRobot"]) == 0 ? "否" : "是";
        }
        return OpRes.opres_success;
    }

    private OpRes query2(GMUser user, string table)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(table, dip, null, 0, 0, null, "rank", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatRankItem tmp = new StatRankItem();
            m_result.Add(tmp);

            tmp.m_rankId = Convert.ToInt32(data["rank"]);
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("gainCount"))
                tmp.m_moonCakeCount = Convert.ToInt32(data["gainCount"]);

            if (data.ContainsKey("fishLevel"))
                tmp.m_fishLevel = Convert.ToInt32(data["fishLevel"]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);

            if (data.ContainsKey("isRobot"))
                tmp.m_isRobot = Convert.ToInt32(data["isRobot"]) == 0 ? "否" : "是";
        }
        return OpRes.opres_success;
    }
}
//转盘鱼捕鱼统计
public class StatTFishItem
{
    public string m_time;
    public long m_goldOutlay;
    public long m_goldIncome;
    public int m_killCount;
    public int m_joinPerson;
    public long m_baseScore;
    public int m_useTimeCard;
    public Dictionary<int, int> m_correct = new Dictionary<int, int>();

    public string getDetail(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_STAT_TFISH_DEATIL;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryStatTurnTableFish : QueryBase 
{
    private List<StatTFishItem> m_result = new List<StatTFishItem>();
    public string[] m_fields = new string[] { "goldIncome", "goldOutlay", "killCount", "baseScore", "useTimeCard" };
  
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_param, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TFISH, dip, imq, 0, 0, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatTFishItem tmp = new StatTFishItem();
            m_result.Add(tmp);
            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();

            for (k = 0; k < 5; k++)
            {
                string correct = string.Format("correct{0}", k);
                if (data.ContainsKey(correct)){
                    tmp.m_correct[k] = Convert.ToInt32(data[correct]);
                }
                else{
                    tmp.m_correct[k] = 0;
                }
            }

            IMongoQuery sq = Query.EQ("genTime", time);
            List<Dictionary<string, object>> goldList =
                        DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TFISH_ROOM_STAT, dip, sq, 0, 0, m_fields, "roomId", true);
            if (goldList != null)
            {
                for (j = 0; j < goldList.Count; j++)
                {
                    if (goldList[j].ContainsKey("goldIncome"))
                        tmp.m_goldIncome += Convert.ToInt64(goldList[j]["goldIncome"]);

                    if (goldList[j].ContainsKey("goldOutlay"))
                        tmp.m_goldOutlay += Convert.ToInt64(goldList[j]["goldOutlay"]);

                    if (goldList[j].ContainsKey("killCount"))
                        tmp.m_killCount += Convert.ToInt32(goldList[j]["killCount"]);

                    if (goldList[j].ContainsKey("baseScore"))
                        tmp.m_baseScore += Convert.ToInt64(goldList[j]["baseScore"]);

                    if (goldList[j].ContainsKey("useTimeCard"))
                        tmp.m_useTimeCard += Convert.ToInt32(goldList[j]["useTimeCard"]);
                }
            }

            tmp.m_joinPerson = Convert.ToInt32(DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_TFISH_PLAYER, sq, dip));
        }
        return OpRes.opres_success;
    }
}
//转盘鱼捕鱼统计详情
public class StatTFishPlayer 
{
    public string m_time;
    public int m_playerId;
    public int m_gainGold;
    public int m_useTimeCard;
    public Dictionary<int, int> m_correct = new Dictionary<int, int>();
}
public class QueryStatTurnTableFishDetail : QueryBase 
{
    private List<StatTFishPlayer> m_result = new List<StatTFishPlayer>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_TFISH_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TFISH_PLAYER, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatTFishPlayer tmp = new StatTFishPlayer();
            m_result.Add(tmp);
            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("gainGold"))
                tmp.m_gainGold = Convert.ToInt32(data["gainGold"]);

            if (data.ContainsKey("useTimeCard"))
                tmp.m_useTimeCard = Convert.ToInt32(data["useTimeCard"]);

            for (k = 0; k < 5; k++)
            {
                string correct = string.Format("correct{0}", k);
                if (data.ContainsKey(correct))
                {
                    tmp.m_correct[k] = Convert.ToInt32(data[correct]);
                }
                else
                {
                    tmp.m_correct[k] = 0;
                }
            }
        }
        return OpRes.opres_success;
    }
}

//转盘鱼场次统计
public class QueryStatTurnTableFishRoom : QueryBase 
{
    private List<ActRoomStatList> m_result = new List<ActRoomStatList>();
    public ActRoomStatList IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        ActRoomStatList item = new ActRoomStatList();
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_param, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TFISH_ROOM_STAT, dip, imq, 0, 0, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["roomId"]).OrderByDescending(a => a["genTime"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();

            ActRoomStatList tmp = IsCreate(time);
            tmp.m_time = time;

            ActRoomStatItem item = new ActRoomStatItem();

            if (data.ContainsKey("roomId"))
            {
                item.m_roomId = Convert.ToInt32(data["roomId"]);
                if (data.ContainsKey("goldIncome"))
                    item.m_goldIncome = Convert.ToInt64(data["goldIncome"]);

                if (data.ContainsKey("killCount"))
                    item.m_killCount = Convert.ToInt32(data["killCount"]);

                tmp.m_data.Add(item.m_roomId, item);
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//小游戏实时输赢
public class MiniGameRealLoseWinRankItem 
{
    public int m_playerId;
    public string m_nickName;
    public int m_rank;
    public int m_turn;
    public long m_outlay;
    public long m_income;
    public long m_addValue;
    public int m_recharge;
    public string m_lastLogin;
}
//小游戏实时输赢
public class QueryGameRealTimeLoseWinRank : QueryBase 
{
    private List<MiniGameRealLoseWinRankItem> m_result = new List<MiniGameRealLoseWinRankItem>();
    //小游戏实时输赢排行
    public static string[] s_miniGameRealLoseWinRank = { "miniGameRealWinLoseRank_All",
                                                         "miniGameRealWinLoseRank_Crocodile", 
                                                         "miniGameRealWinLoseRank_Cows", 
                                                         "miniGameRealWinLoseRank_Shcd", 
                                                         "miniGameRealWinLoseRank_Shuihz", 
                                                         "miniGameRealWinLoseRank_Bz", 
                                                         "miniGameRealWinLoseRank_Fruit", 
                                                         "miniGameRealWinLoseRank_Jewel"
                            };
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        int gameType = Convert.ToInt32(p.m_param);
        string tableName =  s_miniGameRealLoseWinRank[gameType];

        IMongoQuery imq = null;
        bool rank_asc = true;
        switch(p.m_op)
        {
            case 0: imq = Query.And(Query.EQ("itemId",1),Query.GT("addValue",0));
                rank_asc = false;
                break;
            case 1: imq = Query.And(Query.EQ("itemId", 1), Query.LT("addValue", 0));
                rank_asc = true;
                break;
            case 2: imq = Query.And(Query.EQ("itemId", 14), Query.GT("addValue", 0));
                rank_asc = false;
                break;
            case 3: imq = Query.And(Query.EQ("itemId", 14), Query.LT("addValue", 0));
                rank_asc = true;
                break;
        }

        return query(user, tableName, imq, rank_asc);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, string tableName, IMongoQuery imq, Boolean rank_asc)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip, imq, 0, 20, null, "addValue", rank_asc);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string,object> data = dataList[i];
            MiniGameRealLoseWinRankItem info = new MiniGameRealLoseWinRankItem();
            m_result.Add(info);

            if(data.ContainsKey("playerId"))
                info.m_playerId = Convert.ToInt32(data["playerId"]);

            info.m_rank = (i + 1);

            if (data.ContainsKey("turn"))
                info.m_turn = Convert.ToInt32(data["turn"]);

            if (data.ContainsKey("outlay"))
                info.m_outlay = Convert.ToInt64(data["outlay"]);

            if (data.ContainsKey("income"))
                info.m_income = Convert.ToInt64(data["income"]);

            info.m_addValue = Convert.ToInt64(data["addValue"]);

            if (data.ContainsKey("recharge"))
                info.m_recharge = Convert.ToInt32(data["recharge"]);

            Dictionary<string, object> ret =
                QueryBase.getPlayerProperty(info.m_playerId, user, new string[] { "logout_time" ,"nickname"});
            if (ret != null)
            {
                if (ret.ContainsKey("nickname"))
                    info.m_nickName = Convert.ToString(ret["nickname"]);

                if (ret.ContainsKey("logout_time"))
                    info.m_lastLogin = Convert.ToDateTime(ret["logout_time"]).ToLocalTime().ToString();
            }
        }

        return OpRes.opres_success;
    }
}
//小游戏日累计输赢
public class QueryGameDailyTotalLoseWinRank : QueryBase 
{
    private List<MiniGameRealLoseWinRankItem> m_result = new List<MiniGameRealLoseWinRankItem>();
    //小游戏实时输赢排行
    public static string[] s_miniGameRealLoseWinRank = { "miniGameMonthWinLoseRank_All",
                                                         "miniGameMonthWinLoseRank_Crocodile", 
                                                         "miniGameMonthWinLoseRank_Cows", 
                                                         "miniGameMonthWinLoseRank_Shcd", 
                                                         "miniGameMonthWinLoseRank_Shuihz", 
                                                         "miniGameMonthWinLoseRank_Bz", 
                                                         "miniGameMonthWinLoseRank_Fruit", 
                                                         "miniGameMonthWinLoseRank_Jewel"
                            };
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        int gameType = Convert.ToInt32(p.m_param);
        string tableName = s_miniGameRealLoseWinRank[gameType];

        if (string.IsNullOrEmpty(p.m_time))
            p.m_time = "2018";

        int year = 2018;
        if (!int.TryParse(p.m_time, out year))
            return OpRes.op_res_time_format_error;

        year = year * 100 + Convert.ToInt32(p.m_showWay);
  
        IMongoQuery imq = null;
        bool rank_asc = true;
        switch (p.m_op)
        {
            case 0: imq = Query.And(Query.EQ("itemId", 1), Query.GT("addValue", 0),Query.EQ("month",year));
                rank_asc = false;
                break;
            case 1: imq = Query.And(Query.EQ("itemId", 1), Query.LT("addValue", 0), Query.EQ("month", year));
                rank_asc = true;
                break;
            case 2: imq = Query.And(Query.EQ("itemId", 14), Query.GT("addValue", 0), Query.EQ("month", year));
                rank_asc = false;
                break;
            case 3: imq = Query.And(Query.EQ("itemId", 14), Query.LT("addValue", 0), Query.EQ("month", year));
                rank_asc = true;
                break;
        }

        return query(user, tableName, imq, rank_asc);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, string tableName, IMongoQuery imq, Boolean rank_asc)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip, imq, 0, 20, null, "addValue", rank_asc);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            MiniGameRealLoseWinRankItem info = new MiniGameRealLoseWinRankItem();
            m_result.Add(info);

            if (data.ContainsKey("playerId"))
                info.m_playerId = Convert.ToInt32(data["playerId"]);

            info.m_rank = (i + 1);

            if (data.ContainsKey("turn"))
                info.m_turn = Convert.ToInt32(data["turn"]);

            if (data.ContainsKey("outlay"))
                info.m_outlay = Convert.ToInt64(data["outlay"]);

            if (data.ContainsKey("income"))
                info.m_income = Convert.ToInt64(data["income"]);

            info.m_addValue = Convert.ToInt64(data["addValue"]);

            if (data.ContainsKey("recharge"))
                info.m_recharge = Convert.ToInt32(data["recharge"]);

            Dictionary<string, object> ret =
                QueryBase.getPlayerProperty(info.m_playerId, user, new string[] { "logout_time", "nickname" });
            if (ret != null)
            {
                if (ret.ContainsKey("nickname"))
                    info.m_nickName = Convert.ToString(ret["nickname"]);

                if (ret.ContainsKey("logout_time"))
                    info.m_lastLogin = Convert.ToDateTime(ret["logout_time"]).ToLocalTime().ToString();
            }
        }

        return OpRes.opres_success;
    }
}

//库存数据统计
public class statPlayerMoneyRepItem 
{
    public string m_time;
    public long m_goldTotalRep;
    public long m_dragonBallTotalRep;
    public long m_goldActualRep;
    public long m_dragonBallActualRep;
    public long m_goldPlayerRep;
    public long m_dragonBallPlayerRep;

}
public class QueryStatPlayerMoneyRep : QueryBase 
{
    private List<statPlayerMoneyRepItem> m_result = new List<statPlayerMoneyRepItem>();
    public statPlayerMoneyRepItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        statPlayerMoneyRepItem item = new statPlayerMoneyRepItem();
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq_1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq_2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq_1, imq_2);

        return query(user, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(
            TableName.STAT_PLAYER_MONEY_REP, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            statPlayerMoneyRepItem info = IsCreate(time);

            info.m_time = time;
            if (data.ContainsKey("repType")) 
            {
                int repType = Convert.ToInt32(data["repType"]);
                long gold = 0, dragonBall = 0;
                if(data.ContainsKey("gold"))
                    gold = Convert.ToInt64(data["gold"]);
                
                if(data.ContainsKey("dragonBall"))
                    dragonBall = Convert.ToInt64(data["dragonBall"]);

                switch(repType)
                {
                    case 0:
                        info.m_goldTotalRep = gold;
                        info.m_dragonBallTotalRep = dragonBall;
                        break;
                    case 1:
                        info.m_goldActualRep = gold;
                        info.m_dragonBallActualRep = dragonBall;
                        break;
                    case 2:
                        info.m_goldPlayerRep = gold;
                        info.m_dragonBallPlayerRep = dragonBall;
                        break;
                }
            }
        }

        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//客服补单历史查询  
public class ParamRepairOrderItem : ParamRepairOrder 
{
    //操作原因
    public string getOpreason() 
    {
        string opName = "";
        switch(m_op){
            case 0: opName="补单";  break;
            case 1: opName = "换包福利"; break;
            case 2: opName = "访问客服福利"; break;
            case 3: opName = "大户回流引导"; break;
        }
        return opName;
    }

    //补单项目
    public string getRepairOrderName() 
    {
        string orderName = "";

        if (m_itemId == -1){
            return "false";
        }
        else {
            var orderItem = RechargeCFG.getInstance().getValue(m_itemId);
            if (orderItem != null)
                orderName = orderItem.m_name;
        }
        return orderName;
    }

    //补单补贴/客服回访福利
    public string getRepairBonusName() 
    {
        string bonusName = "";
        if (m_bonusId == -1){
            return "false";
        }
        else {
            var bonusItem = RepairOrderItem.getInstance().getValue(m_bonusId);
            if (bonusItem != null)
                bonusName = bonusItem.m_itemName;
        }
        return bonusName;
    }
}
public class QueryRepairOrder : QueryBase 
{
    private List<ParamRepairOrderItem> m_result = new List<ParamRepairOrderItem>();
    private QueryCondition m_cond = new QueryCondition();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_cond.startQuery();

        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        IMongoQuery imq = m_cond.getImq();
        ParamQuery p = new ParamQuery();
        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQuery p = (ParamQuery)param;
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            if (queryCond.isExport())
            {
                queryCond.addCond("time", p.m_time);
            }
            else
            {
                IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
                queryCond.addImq(Query.And(imq1, imq2));
            }
        }
        return OpRes.opres_success;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_REPAIR_ORDER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_REPAIR_ORDER, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "time", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ParamRepairOrderItem tmp = new ParamRepairOrderItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["time"]).ToLocalTime().ToString();
            tmp.m_op = Convert.ToInt32(data["opReason"]);
            if (data.ContainsKey("playerIdList"))
                tmp.m_playerId = Convert.ToString(data["playerIdList"]);

            if (data.ContainsKey("repairOrder"))
                tmp.m_itemId = Convert.ToInt32(data["repairOrder"]);

            if (data.ContainsKey("repairBonus"))
                tmp.m_bonusId = Convert.ToInt32(data["repairBonus"]);

            if (data.ContainsKey("operator"))
                tmp.m_operator = Convert.ToString(data["operator"]);

            if (data.ContainsKey("comments"))
                tmp.m_comments = Convert.ToString(data["comments"]);
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//流失大户筛选
public class SelectLostPlayersItem 
{
    public string m_playerId;
    public string m_nickName;
    public string m_phone;
    public string m_channel;
    public int m_RechargeRMB;
    public int m_vip;
    public string m_creatTime;
    public string m_lastLoginTime;
    public int m_leftGold;
    public string m_guideRecord;
    public string m_time;
}
public class QuerySelectLostPlayers : QueryBase 
{
    private List<SelectLostPlayersItem> m_result = new List<SelectLostPlayersItem>();
    static string[] m_fieldPlayer = new string[] { "player_id", "VipLevel", "bindPhone", "nickname", "ChannelID", "create_time", "logout_time", "gold"};
    static string[] m_fields = new string[] { "mobilePhone","rechargeMoney"};
    private QueryCondition m_cond = new QueryCondition();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_cond.startQuery();

        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        IMongoQuery imq = m_cond.getImq();
        ParamSelectLostPlayer p = (ParamSelectLostPlayer)param;
        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamSelectLostPlayer p = (ParamSelectLostPlayer)param;
        int vipLevel = 0;
        if (string.IsNullOrEmpty(p.m_vipLevel))
            return OpRes.op_res_param_not_valid;
        if(!int.TryParse(p.m_vipLevel, out vipLevel))
            return OpRes.op_res_param_not_valid;
        if (vipLevel >10)
            return OpRes.op_res_param_not_valid;

        int days = 0;
        if (string.IsNullOrEmpty(p.m_days))
            return OpRes.op_res_param_not_valid;
        if (!int.TryParse(p.m_days, out days))
            return OpRes.op_res_param_not_valid;
        if (days < 0)
            return OpRes.op_res_param_not_valid;

        if (queryCond.isExport())
        {
            //导出 queryCond.addCond("time", p.m_time);
        }
        else 
        {
            IMongoQuery imq1 = Query.GT("VipLevel",vipLevel);

            DateTime now = DateTime.Now.AddDays(0-days);
            IMongoQuery imq2 = Query.LT("logout_time", now);

            IMongoQuery imq = null;
            if (p.m_isBindPhone == 0) //不绑定
            {
                imq = Query.EQ("bindPhone", "");
            }
            else {
                imq = Query.NE("bindPhone", "");
            }

            queryCond.addImq(Query.And(imq1, imq2, imq));
        }
        return OpRes.opres_success;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamSelectLostPlayer param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO, dip, imq,0,0
             //(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage
             , m_fieldPlayer, "VipLevel", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            SelectLostPlayersItem tmp = new SelectLostPlayersItem();
            
            if (data.ContainsKey("player_id")) {
                tmp.m_playerId = Convert.ToString(data["player_id"]);

                if(data.ContainsKey("bindPhone"))
                    tmp.m_phone = Convert.ToString(data["bindPhone"]);

                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                bool res = Tool.splitTimeStr(param.m_time, ref mint, ref maxt);
                if (!res)
                    return OpRes.op_res_time_format_error;

                IMongoQuery imq_1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq_2 = Query.GTE("genTime", BsonValue.Create(mint));
                IMongoQuery imq_3 = Query.And(imq_1, imq_2, Query.EQ("playerId", Convert.ToInt32(tmp.m_playerId)));

                //玩家在指定时间段内是否有充值 无则continue
                List<Dictionary<string, object>> playerRechargeList = DBMgr.getInstance().executeQuery(TableName.PUMP_RECHARGE_PLAYER, dip_1, imq_3,
                    0, 0, m_fields, "rechargeMoney", false);
                if (playerRechargeList == null || playerRechargeList.Count == 0)
                    continue;
                int totalRechargeMoney = 0;
                for (k = 0; k < playerRechargeList.Count; k++) 
                {
                    if (playerRechargeList[k].ContainsKey("mobilePhone"))
                        tmp.m_phone = Convert.ToString(playerRechargeList[k]["mobilePhone"]);

                    if (playerRechargeList[k].ContainsKey("rechargeMoney"))
                        totalRechargeMoney += Convert.ToInt32(playerRechargeList[k]["rechargeMoney"]);
                }
                if (totalRechargeMoney == 0)
                    continue;
                tmp.m_RechargeRMB = totalRechargeMoney;

                //player_info信息///////////////////////////////////////
                if (data.ContainsKey("nickname"))
                    tmp.m_nickName = Convert.ToString(data["nickname"]);

                if (data.ContainsKey("ChannelID"))
                {
                    string channel = Convert.ToString(data["ChannelID"]).PadLeft(6, '0');
                    TdChannelInfo cd = TdChannel.getInstance().getValue(channel);
                    if (cd != null){
                        tmp.m_channel = cd.m_channelName;
                    }else
                    {
                        tmp.m_channel = channel;
                    }
                }

                if (data.ContainsKey("VipLevel"))
                    tmp.m_vip = Convert.ToInt32(data["VipLevel"]);

                if (data.ContainsKey("create_time"))
                    tmp.m_creatTime = Convert.ToDateTime(data["create_time"]).ToLocalTime().ToString();

                if (data.ContainsKey("logout_time"))
                    tmp.m_lastLoginTime = Convert.ToDateTime(data["logout_time"]).ToLocalTime().ToString();

                if (data.ContainsKey("gold"))
                    tmp.m_leftGold = Convert.ToInt32(data["gold"]);

                //流失大户引导信息
                Dictionary<string, object> guideInfo = DBMgr.getInstance().getTableData(
                    TableName.GUIDE_LOST_PLAYER_INFO,"player_id",Convert.ToInt32(tmp.m_playerId),null,dip);
                if (guideInfo != null) {
                    if (guideInfo.ContainsKey("comments"))
                        tmp.m_guideRecord = Convert.ToString(guideInfo["comments"]);
                }

                m_result.Add(tmp);
            }
        }

        if (m_result.Count == 0)
            return OpRes.op_res_not_found_data;

        return OpRes.opres_success;
    }
}
//流失大户引导记录效果
public class QueryGuideLostPlayersRes : QueryBase 
{
    private List<SelectLostPlayersItem> m_result = new List<SelectLostPlayersItem>();
    static string[] m_fieldPlayer = new string[] { "player_id", "VipLevel", "bindPhone", "nickname", "ChannelID", "create_time", "logout_time", "gold" };
    static string[] m_fields = new string[] { "playerId","rechargeMoney" };

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq_1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq_2 = Query.GTE("time", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq_1,imq_2);

        return query(user, imq, p, maxt, mint);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user,IMongoQuery imq,ParamQuery param,DateTime maxt, DateTime mint) 
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.GUIDE_LOST_PLAYER_INFO, imq, dip);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(
             TableName.GUIDE_LOST_PLAYER_INFO, dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null,"time", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            SelectLostPlayersItem tmp = new SelectLostPlayersItem();
            //添加引导表信息
            tmp.m_time = Convert.ToDateTime(dataList[i]["time"]).ToLocalTime().ToString();
            tmp.m_playerId = Convert.ToString(dataList[i]["player_id"]);

            if (dataList[i].ContainsKey("comments"))
                tmp.m_guideRecord = Convert.ToString(dataList[i]["comments"]);

            //player_info表信息
            Dictionary<string, object> playerInfo = DBMgr.getInstance().getTableData(
                TableName.PLAYER_INFO, "player_id", Convert.ToInt32(tmp.m_playerId), m_fieldPlayer, dip);
            if(playerInfo != null)
            {
                if (playerInfo.ContainsKey("nickname"))
                    tmp.m_nickName = Convert.ToString(playerInfo["nickname"]);

                if (playerInfo.ContainsKey("bindPhone"))
                    tmp.m_phone = Convert.ToString(playerInfo["bindPhone"]);

                if (playerInfo.ContainsKey("ChannelID")) 
                {
                    string channel = Convert.ToString(playerInfo["ChannelID"]).PadLeft(6, '0');
                    TdChannelInfo cd = TdChannel.getInstance().getValue(channel);
                    if (cd != null){
                        tmp.m_channel = cd.m_channelName;
                    }else
                    {
                        tmp.m_channel = channel;
                    }
                }

                if (playerInfo.ContainsKey("VipLevel"))
                    tmp.m_vip = Convert.ToInt32(playerInfo["VipLevel"]);

                if (playerInfo.ContainsKey("create_time"))
                    tmp.m_creatTime = Convert.ToDateTime(playerInfo["create_time"]).ToLocalTime().ToString();

                if (playerInfo.ContainsKey("logout_time"))
                    tmp.m_lastLoginTime = Convert.ToDateTime(playerInfo["logout_time"]).ToLocalTime().ToString();

                if (playerInfo.ContainsKey("gold"))
                    tmp.m_leftGold = Convert.ToInt32(playerInfo["gold"]);
            }

            //充值信息
            IMongoQuery imq_1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq_2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq_3 = Query.And(imq_1, imq_2, Query.EQ("playerId", Convert.ToInt32(tmp.m_playerId)));

            //玩家在指定时间段内是否有充值 无则continue
            List<Dictionary<string, object>> playerRechargeList = DBMgr.getInstance().executeQuery(TableName.PUMP_RECHARGE_PLAYER, dip_1, imq_3,
                0, 0, m_fields, "rechargeMoney", false);

            int totalRechargeMoney = 0;
            if (playerRechargeList != null) 
            {
                for (k = 0; k < playerRechargeList.Count; k++)
                {
                    if (playerRechargeList[k].ContainsKey("rechargeMoney"))
                        totalRechargeMoney += Convert.ToInt32(playerRechargeList[k]["rechargeMoney"]);
                }
            }

            tmp.m_RechargeRMB = totalRechargeMoney;

            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//极光推送
public class PolarLightsItems:PolarLightsParam
{
    public string channelList = "";
    public string vipList = "";
    public string weekList = "";
    public string date = "";
}
public class QueryPolarLightsPush:QueryBase
{
    private List<PolarLightsItems> m_result = new List<PolarLightsItems>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CONFIG_POLAR_LIGHTS_PUSH, serverId, DbName.DB_CONFIG,null, 0, 0, null, "_id", true);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            PolarLightsItems tmp = new PolarLightsItems();
            m_result.Add(tmp);
            tmp.m_id = Convert.ToString(data["id"]);
            //渠道
            if(data.ContainsKey("targetChannel"))
            {
                string channelList = Convert.ToString(data["targetChannel"]);
                tmp.channelList = channelList;
                if (channelList == "-1")
                {
                    tmp.m_channelList = "全部渠道";
                }
                else 
                {
                    string[] m_channels = channelList.Split(',');
                    List<string> channelsName = new List<string>();
                    foreach (var channel in m_channels)
                    {
                        TdChannelInfo tinfo = TdChannel.getInstance().getValue(channel.PadLeft(6, '0'));
                        if (tinfo != null)
                        {
                            channelsName.Add(tinfo.m_channelName);
                        }
                        else 
                        {
                            channelsName.Add(channel);
                        }
                    }
                    tmp.m_channelList = string.Join(",",channelsName.ToArray());
                }
            }
            //vip
            if(data.ContainsKey("targetVip"))
            {
                string vipList = Convert.ToString(data["targetVip"]);
                tmp.vipList = vipList;
                if (vipList == "0")
                {
                    tmp.m_vipList = "所有玩家";
                }
                else 
                {
                    string[] m_vips = vipList.Split(',');
                    List<string> vipName = new List<string>();
                    foreach(var vip in vipList)
                    {
                        vipName.Add("VIP"+vip);
                    }
                    tmp.m_vipList = string.Join(",",vipName.ToArray());
                }
            }
            //日期区间
            string startTime = "";
            string endTime = "";
            if(data.ContainsKey("startTime"))
            {
                startTime = Convert.ToDateTime(data["startTime"]).ToLocalTime().ToShortDateString();
            }
            if(data.ContainsKey("endTime"))
            {
                endTime = Convert.ToDateTime(data["endTime"]).ToLocalTime().ToShortDateString();
            }
            tmp.m_date = startTime + '-' + endTime;
            tmp.date = tmp.m_date;
            //星期
            if(data.ContainsKey("week"))
            {
                string weekList = Convert.ToString(data["week"]);
                tmp.weekList = weekList;
                if (weekList == "0")
                {
                    tmp.m_weekList = getWeekName(0);
                }
                else 
                {
                    string[] m_weeks = weekList.Split(',');
                    List<string> weekName = new List<string>();
                    foreach(var week in m_weeks)
                    {
                        int index = Convert.ToInt32(week);
                        weekName.Add(getWeekName(index));
                    }
                    tmp.m_weekList = string.Join(",",weekName.ToArray());
                }
            }
            //推送时间
            if(data.ContainsKey("hour"))
            {
                tmp.m_time = Convert.ToString(data["hour"]);
            }
            //推送内容
            if(data.ContainsKey("content"))
            {
                tmp.m_content = Convert.ToString(data["content"]);
            }
            //备注
            if(data.ContainsKey("note"))
            {
                tmp.m_note = Convert.ToString(data["note"]);
            }
        }
        return OpRes.opres_success;
    }

    public string getWeekName(int key) 
    {
        string str = "";
        switch(key)
        {
            case 0: str = "每天"; break;
            case 1: str = "星期一"; break;
            case 2: str = "星期二"; break;
            case 3: str = "星期三"; break;
            case 4: str = "星期四"; break;
            case 5: str = "星期五"; break;
            case 6: str = "星期六"; break;
            case 7: str = "星期日"; break;
        }
        return str;
    }
}
//////////////////////////////////////////////////////////////////////////////
//国庆节领取奖励人数统计
public class NdActRecvItem 
{
    public Dictionary<string, int[]> m_data = new Dictionary<string, int[]>();
    public string getPercent(int key, int num)
    {
        string data_percent = key.ToString();
        if (num != 0)
        {
            data_percent = Math.Round((key * 1.0 * 100) / num, 2) + "%";
        }
        return data_percent;
    }

    public void Reset() 
    {
        m_data.Clear();
    }
}
public class QueryNdActRecvCount : QueryBase
{
    private NdActRecvItem m_result = new NdActRecvItem();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Reset();
        return query(user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_ND_ACT, dip, null, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        NdActRecvItem tmp = new NdActRecvItem();
        m_result=tmp;
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            int loginCount = 0;
            if(data.ContainsKey("loginCount"))
            {
                loginCount = Convert.ToInt32(data["loginCount"]);
            }
            int recvCount = 0;
            if(data.ContainsKey("recvCount"))
            {
                recvCount = Convert.ToInt32(data["recvCount"]);
            }

            int[] loginRecvCount = new int[] { recvCount,loginCount};
            if(!tmp.m_data.ContainsKey(time))
            {
                tmp.m_data.Add(time, loginRecvCount);
            }
        }
        return OpRes.opres_success;
    }
}

//活动排行榜
public class NdActRankList
{
    public int m_rankId;
    public string m_playerId;
    public int m_fishCount;
    public string m_nickName;
}
public class QueryNdActRankList : QueryBase
{
    private List<NdActRankList> m_result = new List<NdActRankList>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_NATIONAL_DAY_ACTIVITY, dip, null, 0, 100, null, "fishCount", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        var data = dataList.OrderBy(a =>
                {
                    if (a.ContainsKey("fishTime"))
                    {
                        return a["fishTime"];
                    }
                    else 
                    {
                        return DateTime.Now;
                    }
                }
            ).OrderByDescending(a =>
                {
                    if(a.ContainsKey("fishCount"))
                    {
                        return a["fishCount"];
                    }else
                    {
                        return 0;
                    }
                }
            ).ToList();

        for (int i = 0; i < data.Count; i++)
        {
            Dictionary<string, object> da =data[i];
            NdActRankList tmp = new NdActRankList();
            m_result.Add(tmp);

            if (da.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToString(da["playerId"]);
            }

            tmp.m_rankId = i + 1;

            if (da.ContainsKey("fishCount"))
            {
                tmp.m_fishCount = Convert.ToInt32(da["fishCount"]);
            }

            if (da.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(da["nickName"]);
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////
//幸运转转转   中秋特惠活动
public class JinQiuRechargeLotteryItem
{
    public string m_time;
    public int m_giftId;
    public int m_price;
    public long m_buyCount;
    public long m_outlayGold;
    public string getAverage(long total, long count)
    {
        string data_avg = total.ToString();
        if (count != 0)
        {
            data_avg = Math.Round((total * 1.0) / count, 2).ToString();
        }
        return data_avg;
    }

    public string getGiftName() 
    {
        string giftName = "";
        switch(m_giftId)
        {
            case 99: giftName = "6元礼包"; break;
            case 100: giftName = "30元礼包"; break;
            case 101: giftName = "128元礼包"; break;
        }

        return giftName;
    }
}
public class QueryJinQiuRechargeLottery : QueryBase
{
    private List<JinQiuRechargeLotteryItem> m_result = new List<JinQiuRechargeLotteryItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_JIN_QIU_RECHARGE_LOTTERY, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            JinQiuRechargeLotteryItem tmp = new JinQiuRechargeLotteryItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if(data.ContainsKey("giftId"))
            {
                tmp.m_giftId = Convert.ToInt32(data["giftId"]);
                var gift = RechargeCFG.getInstance().getValue(tmp.m_giftId);
                if(gift!= null)
                    tmp.m_price = gift.m_price;
            }

            if(data.ContainsKey("buyCount"))
                tmp.m_buyCount = Convert.ToInt64(data["buyCount"]);

            if(data.ContainsKey("outlayGold"))
                tmp.m_outlayGold = Convert.ToInt64(data["outlayGold"]);
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////
//刮刮乐活动
//运营状况
public class ScratchActEarnItem 
{
    public int m_changCiId;
    public int m_joinCount;
    public long m_payGold;
    public long m_exchangeGold;
    public long m_payGem;
    public long m_exchangeGem;
}
public class QueryTypeScratchActEarn : QueryBase 
{
    private List<ScratchActEarnItem> m_result = new List<ScratchActEarnItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p=(ParamQuery)param;
        return query(user);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
            DBMgr.getInstance().executeQuery(TableName.PUMP_SCRATCH_EARN, dip, null, 0, 0, null, "changCiId", true);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        for (int i = 0; i<dataList.Count;i++ ) 
        {
            Dictionary<string,object> data=dataList[i];
            ScratchActEarnItem tmp = new ScratchActEarnItem();
            m_result.Add(tmp);
            tmp.m_changCiId = Convert.ToInt32(data["changCiId"]);
            if(data.ContainsKey("joinCount"))
            {
                tmp.m_joinCount = Convert.ToInt32(data["joinCount"]);
            }
            if(data.ContainsKey("payGold"))
            {
                tmp.m_payGold = Convert.ToInt64(data["payGold"]);
            }
            if(data.ContainsKey("exchangeGold"))
            {
                tmp.m_exchangeGold = Convert.ToInt64(data["exchangeGold"]);
            }
            if(data.ContainsKey("payGem"))
            {
                tmp.m_payGem = Convert.ToInt64(data["payGem"]);
            }
            if(data.ContainsKey("exchangeGem"))
            {
                tmp.m_exchangeGem = Convert.ToInt64(data["exchangeGem"]);
            }
        }
        return OpRes.opres_success;
    }
}

//兑奖状况
public class ScratchActExchangeResItem
{
    public int m_changCiId;
    public int m_rewardId;
    public int m_winCount;
    public long m_exchangeGold;
    public long m_exchangeGem;
}

public class ScratchActItem
{
    public int m_changCiId;
    public Dictionary<int, ScratchActExchangeResItem> m_data = new Dictionary<int, ScratchActExchangeResItem>();
}

public class QueryTypeScratchActExchangeRes : QueryBase
{
    private List<ScratchActItem> m_result = new List<ScratchActItem>();
    public ScratchActItem IsCreate(int changCiId, int rewardId)
    {
        foreach (var res in m_result)
        {
            if (res.m_changCiId == changCiId)
            {
                return res;
            }
        }

        ScratchActItem item = new ScratchActItem();
        item.m_changCiId = changCiId;
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        return query(user);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
            DBMgr.getInstance().executeQuery(TableName.PUMP_SCRATCH_EXCHANGE_RES, dip, null, 0, 0, null, "changCiId", true);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            ScratchActExchangeResItem tmp = new ScratchActExchangeResItem();
            tmp.m_changCiId=Convert.ToInt32(data["changCiId"]);
            tmp.m_rewardId = Convert.ToInt32(data["rewardId"]);
            tmp.m_winCount = Convert.ToInt32(data["winCount"]);
            if (data.ContainsKey("exchangeGold"))
            {
                tmp.m_exchangeGold = Convert.ToInt64(data["exchangeGold"]);
            }
            if (data.ContainsKey("exchangeGem")) 
            {
                tmp.m_exchangeGem = Convert.ToInt64(data["exchangeGem"]);
            }

            ScratchActItem da = IsCreate(tmp.m_changCiId, tmp.m_rewardId);

            if(!da.m_data.ContainsKey(tmp.m_rewardId))
            {
                da.m_data.Add(tmp.m_rewardId, tmp);
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////
//圣诞节/元旦活动
public class ChristmasOrYuandanItem 
{
    public string m_time = "";
    public string m_actName = "";
    public int m_lotteryCount=0;
    public int m_exchangeCount=0;
    public string m_toolName = "";
    public string m_exchangeTotal;
    public string getMultiply(int num_1,int num_2) 
    {
        string total = (num_1 * num_2).ToString();
        return total;
    }
}
public class QueryTypeChristmasOrYuandan : QueryBase
{
    private List<ChristmasOrYuandanItem> m_result = new List<ChristmasOrYuandanItem>();
    public override OpRes doQuery(object param, GMUser user) 
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user,IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.PUMP_CHRISTMAS, dip, imq, 0, 0, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["wordId"]).OrderByDescending(a => a["genTime"]).ToList();
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ChristmasOrYuandanItem tmp = new ChristmasOrYuandanItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            int wordId=Convert.ToInt32(data["wordId"]);
            ChristmasActItem actData = ActivityChristmasCFG.getInstance().getValue(wordId);
            if (actData != null)
            {
                tmp.m_actName = actData.m_actName;
                if(data.ContainsKey("LotteryCount"))
                {
                    tmp.m_lotteryCount = Convert.ToInt32(data["LotteryCount"]);
                }
                if (data.ContainsKey("exchangeCount")) 
                {
                    tmp.m_exchangeCount = Convert.ToInt32(data["exchangeCount"]);
                }
                ItemCFGData item = ItemCFG.getInstance().getValue(actData.m_chgItemId);
                if(item!=null)
                {
                    tmp.m_toolName = item.m_itemName;
                }

                tmp.m_exchangeTotal = tmp.getMultiply(actData.m_chgItemCount, tmp.m_exchangeCount);
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////
//金蟾夺宝活动
//活动排行榜
public class SpittorSnatchActRankItem 
{
    public int m_rankId;
    public string m_playerId;
    public string m_nickName;
    public int m_killCount;
}
public class QuerySpittorSnatchActRank : QueryBase 
{
    private List<SpittorSnatchActRankItem> m_result = new List<SpittorSnatchActRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        IMongoQuery imq = Query.GT("killCount",0);
        return query(user,imq);
    }
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq) 
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = 
            DBMgr.getInstance().executeQuery(TableName.FISHLORD_SPITTOR_SNATCH_ACTIVITY, dip, imq, 0, 5, null, "killCount", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        for (int i = 0; i<dataList.Count; i++) 
        {
            SpittorSnatchActRankItem tmp = new SpittorSnatchActRankItem();
            m_result.Add(tmp);
            tmp.m_rankId = (i+1);
            tmp.m_playerId = Convert.ToString(dataList[i]["playerId"]);
            if (dataList[i].ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(dataList[i]["nickName"]);
            if (dataList[i].ContainsKey("killCount"))
                tmp.m_killCount = Convert.ToInt32(dataList[i]["killCount"]);
        }
        return OpRes.opres_success;
    }
}
//活动领取奖励人数统计
public class SpittorSnatchActReward 
{
    public string m_time;
    public Dictionary<int, int> m_data = new Dictionary<int, int>();
    public int m_flag = 1;
}
public class QuerySpittorSnatchActRewardList : QueryBase 
{
    private List<SpittorSnatchActReward> m_result = new List<SpittorSnatchActReward>();
    public SpittorSnatchActReward IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag++;
                return d;
            }
        }

        SpittorSnatchActReward item = new SpittorSnatchActReward();
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user) 
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
            DBMgr.getInstance().executeQuery(TableName.PUMP_SPITTOR_SNATCH,dip,null,0,0,null,"genTime",false);
        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["levelId"]).OrderByDescending(a => a["genTime"]).ToList();

        for (int i = 0; i<dataList.Count; i++) 
        {
            string time = Convert.ToDateTime(dataList[i]["genTime"]).ToLocalTime().ToShortDateString();
            SpittorSnatchActReward tmp = IsCreate(time);
            tmp.m_time = time;
            if (dataList[i].ContainsKey("levelId"))
            {
                int level = Convert.ToInt32(dataList[i]["levelId"]);
                int count=0;
                if (dataList[i].ContainsKey("recvCount"))
                    count = Convert.ToInt32(dataList[i]["recvCount"]);
                tmp.m_data.Add(level,count);
            }
        }
        return OpRes.opres_success;
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////
//万圣节活动
//活动排行榜
public class HallowmasActRankItem
{
    public int m_rankId;
    public string m_playerId;
    public string m_nickName;
    public int m_pumpkinCount;
    public int m_buyPumpkinCount;
}
public class QueryHallowmasActRank : QueryBase
{
    private List<HallowmasActRankItem> m_result = new List<HallowmasActRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_HALLOWMAS_ACT_RANK, dip, null, 0, 500, null, "pumpkinCount", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        var data = dataList.OrderBy(a =>{
            if (a.ContainsKey("pumpkinTime"))
            {
                return a["pumpkinTime"];
            }
            else 
            {
                return DateTime.Now;
            }
        }).OrderByDescending(a =>{
            if (a.ContainsKey("pumpkinCount"))
            {
                return a["pumpkinCount"];
            }
            else 
            {
                return 0;
            }
        }).ToList();

        for (int i = 0; i < data.Count; i++)
        {
            Dictionary<string, object> da =data[i];
            HallowmasActRankItem tmp = new HallowmasActRankItem();
            m_result.Add(tmp);

            if (da.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToString(da["playerId"]);
            }

            if (da.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(da["nickName"]);
            }

            tmp.m_rankId = i + 1;

            if (da.ContainsKey("pumpkinCount"))
            {
                tmp.m_pumpkinCount= Convert.ToInt32(da["pumpkinCount"]);
            }

            if(da.ContainsKey("buyPumpkinCount"))
            {
                tmp.m_buyPumpkinCount = Convert.ToInt32(da["buyPumpkinCount"]);
            }
        }
        return OpRes.opres_success;
    }
}
//活动领取奖励人数
public class HallowmasActRecvItem 
{
    public string m_time;
    public List<int> m_data = new List<int>(){0,0,0};
    public int m_loginCount;
    public int flag = 0;
}
public class QueryHallowmasActRecvCount : QueryBase
{
    private List<HallowmasActRecvItem> m_result = new List<HallowmasActRecvItem>();
    public HallowmasActRecvItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.flag = 1;
                return d;
            }
        }

        HallowmasActRecvItem item = new HallowmasActRecvItem();
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_HALLOWMAS_ACT_RECV, dip, null, 0, 0, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
           
            Dictionary<string, object> data = dataList[i];
            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            HallowmasActRecvItem tmp = IsCreate(time);

            tmp.m_time = time;
            if(data.ContainsKey("levelId"))
            {
                int levelId = Convert.ToInt32(data["levelId"]);

                if(data.ContainsKey("recvCount"))
                {
                    tmp.m_data[levelId-1] = Convert.ToInt32(data["recvCount"]);
                }
            }

            if (tmp.flag != 0)
            {
                continue;
            }

            //渠道下的注册人数
            string[] m_fields = new string[] {"activeCount", "genTime" };

            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (serverId == -1)
                return OpRes.op_res_failed;
            QueryCondition queryCond = new QueryCondition();
            // 构造时间条件 
            DateTime mint = Convert.ToDateTime(tmp.m_time);
            DateTime maxt = mint.AddDays(1);
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq = Query.And(imq1, imq2);

            List<Dictionary<string, object>> dataActive =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq, 0, 0, m_fields, "genTime", false); //降序
            if (dataActive != null && dataActive.Count != 0)
            {
                foreach (var active in dataActive)
                {
                    int loginCount = Convert.ToInt32(active["activeCount"]);
                    tmp.m_loginCount += loginCount;
                }
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////
//春节礼包
public class StatNYGiftItem 
{
    public int m_giftId;
    public int[] m_data = new int[18];
}
public class QueryTypeStatNYGift : QueryBase 
{
    private List<StatNYGiftItem> m_result = new List<StatNYGiftItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_NY_GIFT, dip, null, 0, 0, null, "giftId");
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatNYGiftItem tmp = new StatNYGiftItem();
            m_result.Add(tmp);
            tmp.m_giftId = Convert.ToInt32(data["giftId"]);
            for (k = 1; k< 19 ; k++) 
            {
                string strParam = "count_"+ k;
                if (data.ContainsKey(strParam))
                {
                    tmp.m_data[k - 1] = Convert.ToInt32(data[strParam]);
                }
                else 
                {
                    tmp.m_data[k - 1] = 0;
                }
            }
        }
        return OpRes.opres_success;
    }
}
//新春充返
public class StatNYAccRechargeItem 
{
    public int m_rewardId;
    public int m_reachCount;
    public int m_recvCount;

    public int getRechargeRMB()
    {
        int m_rechargeRmb = 0;
        var NyAccRechargeItem = Act_NyAccRechargeCFG.getInstance().getValue(m_rewardId);
        if (NyAccRechargeItem != null)
            m_rechargeRmb = NyAccRechargeItem.m_rechargeCount;

        return m_rechargeRmb;
    }
}
public class QueryTypeStatNYAccRecharge : QueryBase 
{
    private List<StatNYAccRechargeItem> m_result = new List<StatNYAccRechargeItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_NY_ACC_RECHARGE, dip, null, 0, 0, null, "rewardId");
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatNYAccRechargeItem tmp = new StatNYAccRechargeItem();
            m_result.Add(tmp);
            tmp.m_rewardId = Convert.ToInt32(data["rewardId"]);

            if (data.ContainsKey("reachCount"))
                tmp.m_reachCount = Convert.ToInt32(data["reachCount"]);

            if (data.ContainsKey("recvCount"))
                tmp.m_recvCount = Convert.ToInt32(data["recvCount"]);
        }
        return OpRes.opres_success;
    }
}

//勇者大冒险
public class StatNYAdventureItem 
{
    public string m_time;
    public int m_joinCount;
    public int m_joinPerson;
    public int[] m_lvCount=new int[5];
    public int m_freeCount;
    public int m_notDieCount;
}
public class QueryTypeStatNYAdventure : QueryBase 
{
    private List<StatNYAdventureItem> m_result = new List<StatNYAdventureItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(imq,user);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_NY_ADVENTURE, dip, imq, 0, 0, null, "genTime");
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatNYAdventureItem tmp = new StatNYAdventureItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if(data.ContainsKey("joinCount"))
            {
                tmp.m_joinCount = Convert.ToInt32(data["joinCount"]);
            }
            for (k = 1; k < 6;k++ )
            {
                string strParam = "Level_"+k;
                if (data.ContainsKey(strParam))
                {
                    tmp.m_lvCount[k - 1] = Convert.ToInt32(data[strParam]);
                }
                else 
                {
                    tmp.m_lvCount[k - 1] = 0;
                }
            }
            if(data.ContainsKey("freeGameCount"))
            {
                tmp.m_freeCount = Convert.ToInt32(data["freeGameCount"]);
            }
            if (data.ContainsKey("useNotDieCardCount")) 
            {
                tmp.m_notDieCount = Convert.ToInt32(data["useNotDieCardCount"]);
            }

            IMongoQuery joinQuery = Query.EQ("genTime", Convert.ToDateTime(data["genTime"]).ToLocalTime());
            tmp.m_joinPerson = Convert.ToInt32(DBMgr.getInstance().getRecordCount(TableName.PUMP_NY_ADVENTURE_JOIN, joinQuery, dip));
        }
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////////
//通过第三方订单号查询玩家ID
public class PlayerOrderItem 
{
    public string m_playerId;
    public string m_payCode;
    public int m_money;
}
public class QueryTypeGetPlayerIdByOrderId : QueryBase 
{
    private PlayerOrderItem m_result = new PlayerOrderItem();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result = null;
        ParamQuery p=(ParamQuery)param;
        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_param_not_valid;
        //支付表及字段
        var paySdkItem = Pay_SdkCFG.getInstance().getValue(p.m_channelNo);
        if (paySdkItem != null && !string.IsNullOrEmpty(paySdkItem.m_fieldName))
        {
            string table = paySdkItem.m_value;
            IMongoQuery imq = Query.EQ(paySdkItem.m_fieldName, p.m_param);
            return query(table, imq, user);
        }

        return OpRes.op_res_failed;
    }
    //返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public OpRes query(string table,IMongoQuery imq,GMUser user) 
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        Dictionary<string, object> data = DBMgr.getInstance().getTableData(table, serverId, DbName.DB_PAYMENT, imq);
        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;

        PlayerOrderItem tmp = new PlayerOrderItem();
        m_result = tmp;
        if(data.ContainsKey("PlayerId"))
        {
            tmp.m_playerId = Convert.ToString(data["PlayerId"]);
        }

        if(data.ContainsKey("RMB"))
        {
            tmp.m_money = Convert.ToInt32(data["RMB"]);
        }

        if(data.ContainsKey("PayCode"))
        {
            int payCode = Convert.ToInt32(data["PayCode"]);
            RechargeCFGData payCode_list = RechargeCFG.getInstance().getValue(payCode);
            if (payCode_list != null)
            {
                tmp.m_payCode = payCode_list.m_name;
            }
            else 
            {
                tmp.m_payCode = payCode.ToString();
            }
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//小游戏开关设置
public class channelOpenCloseGameList
{
    public string m_channelNo;
    public string m_channelName;
    public string m_isCloseAll;
    public string m_condGameLevel;
    public string m_condVipLevel;
    public string isCloseAll(bool flag)
    {
        return flag ? "整体关闭" : "整体开启";
    }

    public string getfireCountLevel(int key) 
    {
        //炮倍率
        string fireCount = key.ToString();
        
        var fl = Fish_LevelCFG.getInstance().getValue(key);
       if(fl!=null)
       {
           fireCount = Convert.ToString(fl.m_openRate);
       }
       if (key == 0)
       {
           fireCount = "不设限";
       }
       return fireCount;
    }

    public string getChannelName(string num) 
    {
        string channelName = num;
        var cd = TdChannel.getInstance().getValue(num.PadLeft(6, '0'));
        if(cd!=null)
        {
            channelName = cd.m_channelName;
        }
        return channelName;
    }

    public string getVipName(int key) 
    {
        string vipName = key.ToString();
        if (key == -1)
        {
            vipName = "不设限";
        }
        else 
        {
            vipName = key+"级";
        }
        return vipName;
    }
}
public class QueryChannelOpenCloseGame : QueryBase
{
    private List<channelOpenCloseGameList> m_result = new List<channelOpenCloseGameList>();
    
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        return query(p, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_OPEN_CLOSE_GAME, dip, null, 0, 0, null, "channel", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            channelOpenCloseGameList tmp = new channelOpenCloseGameList();
            m_result.Add(tmp);
            tmp.m_channelNo = Convert.ToString(data["channel"]);
            tmp.m_channelName = tmp.getChannelName(tmp.m_channelNo);
            tmp.m_condGameLevel = tmp.getfireCountLevel(Convert.ToInt32(data["condGameLevel"]));
            tmp.m_condVipLevel = tmp.getVipName(Convert.ToInt32(data["condVipLevel"]));
            tmp.m_isCloseAll = tmp.isCloseAll(Convert.ToBoolean(data["isCloseAll"]));
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//每日任务
public class DailyTaskItem 
{
    public string m_time;
    public int m_taskId;
    public int m_taskGroup;
    public int m_startPersonCount;
    public int m_finishPersonCount;
}
public class QueryDailyTask : QueryBase
{
    private List<DailyTaskItem> m_result = new List<DailyTaskItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_DAILY_TASK,imq,dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_DAILY_TASK, dip, imq,
             (param.m_curPage-1)*param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DailyTaskItem tmp = new DailyTaskItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if(data.ContainsKey("taskId"))
            {
                tmp.m_taskId = Convert.ToInt32(data["taskId"]);
                MQuestCFGData TaskData = MDailyQuestCFG.getInstance().getValue(tmp.m_taskId);
                if(TaskData!=null)
                {
                    tmp.m_taskGroup = TaskData.m_group;
                }
            }

            if(data.ContainsKey("finishPersonCount"))
            {
                tmp.m_finishPersonCount = Convert.ToInt32(data["finishPersonCount"]);
            }

            if(data.ContainsKey("startPersonCount"))
            {
                tmp.m_startPersonCount = Convert.ToInt32(data["startPersonCount"]);
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//新七日
public class newSevenDayItem
{
    public string m_time;
    public string m_channel;
    public Dictionary<int, int[]> m_dayRecv = new Dictionary<int, int[]>();

    public int m_flag = 0;
    public int m_regeditCount;

    public string getChannelName(string channelNo)
    {
        string channelName = channelNo;
        if (channelNo == "")
            return "";

        TdChannelInfo channelInfo = TdChannel.getInstance().getValue(channelNo.PadLeft(6, '0'));
        if (channelInfo != null)
            return channelInfo.m_channelName;

        return channelName;
    }
}
public class QueryNewSevenDay : QueryBase
{
    private List<newSevenDayItem> m_result = new List<newSevenDayItem>();
    public newSevenDayItem IsCreate(string channel, string time)
    {
        if (channel == "")
        {
            foreach (var d in m_result)
            {
                if (d.m_time == time)
                {
                    d.m_flag++;
                    return d;
                }
            }
        }
        newSevenDayItem item = new newSevenDayItem();
        m_result.Add(item);
        for (int k = 2; k <= 8; k++)
        {
            item.m_dayRecv.Add(k, new int[] { 0, 0 });
        }
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        QueryCondition queryCond = new QueryCondition();

        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_time_format_error;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);

        queryCond.addImq(Query.Exists("channel"));
        if (p.m_channelNo != "")
            queryCond.addImq(Query.EQ("channel", p.m_channelNo));

        IMongoQuery imq = queryCond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_NEW_SEVEN_DAY, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0;
        string strIndex1 = "", strIndex2 = "";
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            string channel = Convert.ToString(data["channel"]).PadLeft(6, '0');
            newSevenDayItem tmp = IsCreate(param.m_channelNo, time);

            tmp.m_time = time;
            tmp.m_channel = channel;
            for (int j = 2; j<= 8; j++ ) 
            {
                strIndex1 = "day" + j + "_cpt";
                strIndex2 = "day" + j + "_rec";

                int cpt = 0, rec = 0;
                if (data.ContainsKey(strIndex1))
                    cpt = Convert.ToInt32(data[strIndex1]);

                if (data.ContainsKey(strIndex2))
                    rec = Convert.ToInt32(data[strIndex2]);

                tmp.m_dayRecv[j][0] += cpt;
                tmp.m_dayRecv[j][1] += rec;
            }
            //只有当全部渠道时才有不为0的情况  避免重复
            if(tmp.m_flag != 0)
                continue;

            //渠道下的注册人数
            string[] m_fields = new string[] { "channel", "regeditCount","genTime" };

            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (serverId == -1)
                return OpRes.op_res_failed;
            QueryCondition queryCond = new QueryCondition();
            // 构造时间条件 
            DateTime mint = Convert.ToDateTime(tmp.m_time);
            DateTime maxt = mint.AddDays(1);
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq3 = Query.And(imq1, imq2);
            queryCond.addImq(imq3);

            if (param.m_channelNo != "")
                queryCond.addImq(Query.EQ("channel", tmp.m_channel));

            IMongoQuery imq_new = queryCond.getImq();

            List<Dictionary<string, object>> dataRegedit =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq_new,0, 0, m_fields, "genTime", false); //降序
            if(dataRegedit!=null && dataRegedit.Count!=0)
            {
                foreach (var regedit in dataRegedit)
                {
                    int regeditCount = Convert.ToInt32(regedit["regeditCount"]);
                    tmp.m_regeditCount += regeditCount;
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//新手任务
public class newPlayerTaskItem 
{
    public string m_time;
    public string m_channel;
    public int m_regeditCount;

    public int m_flag = 0;

    public Dictionary<string, int> m_task = new Dictionary<string, int>();

    public void taskAdd(string key,int num) 
    {
        if(!m_task.ContainsKey(key))
        {
            m_task.Add(key,num);
        }else
        {
            int task=m_task[key];
            m_task[key] = task + num;
        }
    }

    public string getPercent(int key, int num)
    {
        string data_percent = key.ToString();
        if (num != 0)
        {
            data_percent = Math.Round((key * 1.0 * 100) / num, 2) + "%";
        }
        return data_percent;
    }
}
public class newPlayerTask 
{
    public List<newPlayerTaskItem> m_result = new List<newPlayerTaskItem>();

    public void reset() 
    {
        m_result.Clear();
    }

    public newPlayerTaskItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag = 1;
                return d;
            }
        }

        newPlayerTaskItem item = new newPlayerTaskItem();
        m_result.Add(item);
        return item;
    }
}
public class QueryNewPlayerTask : QueryBase 
{
    private newPlayerTask m_result = new newPlayerTask();

    private string[] m_fields = new string[] { "regeditCount" };
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;

        QueryCondition queryCond = new QueryCondition();

        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_time_format_error;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        //查询时间不大于一个月
        TimeSpan ts = maxt.Subtract(mint);  //maxt-mint
        if (ts.Days > 31)
        {
            return OpRes.op_res_not_in_range; //时间范围太大（不大于一个月）
        }

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);

        IMongoQuery imq = queryCond.getImq();
        return query(p, imq, user, m_fields);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user, string[] m_fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_NEW_PLAYER_TASK, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;


        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            newPlayerTaskItem tmp = m_result.IsCreate(time);
            tmp.m_time = time;

            if (data.ContainsKey("taskId")) 
            {
                int taskId = Convert.ToInt32(data["taskId"]);

                if (taskId <= 29 && taskId >= 1 && data.ContainsKey("completeCount"))
                {
                    int num = Convert.ToInt32(data["completeCount"]);
                    tmp.taskAdd("task_" + taskId, num);
                }
            }

            if (tmp.m_flag > 0)
                continue;

            QueryCondition queryCond = new QueryCondition();
            //注册人数  构造时间条件
            DateTime mint = Convert.ToDateTime(tmp.m_time);
            DateTime maxt = mint.AddDays(1);
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq3 = Query.And(imq1, imq2);
            queryCond.addImq(imq3);
            
            IMongoQuery imq_new = queryCond.getImq();

            tmp.m_regeditCount = 0;
            List<Dictionary<string, object>> dataAct =
            DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq_new, 0, 0, m_fields,"genTime",false);
            if (dataAct != null && dataAct.Count !=0 )
            {
                for (int l = 0; l < dataAct.Count;l++)
                {
                    tmp.m_regeditCount += Convert.ToInt32(dataAct[l]["regeditCount"]);
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//每日签到
public class dailySignItem 
{
    public string m_time;
    public string m_channel;
    public int m_signCount;
    public int m_oldPlayerLogin;
    public int m_actCount;
    public int m_flag = 0;

    public int m_uplevelSignCount;
    public int m_vipSignCount;
    public int m_getTicketCount;
    public int m_getVipExpCount;

    public string getChannelName(int id)
    {
        string channelName = "";
        TdChannelInfo channel = TdChannel.getInstance().getValue(id.ToString().PadLeft(6, '0'));
        if (channel != null)
        {
            return channel.m_channelName;
        }
        return channelName;
    }

    public string getPercent(int key, int num)
    {
        string data_percent = key.ToString();
        if (num != 0)
        {
            data_percent = Math.Round((key * 1.0 * 100) / num, 2) + "%";
        }
        return data_percent;
    }
}
public class QueryDailySign : QueryBase 
{
    public List<dailySignItem> m_result = new List<dailySignItem>();

    public string[] m_fields = new string[] { "activeCount" };
    public string[] m_fields1 = new string[] {"uplevelSignCount","vipLevel", "getTicketCount", "getVipExpCount"};
    public dailySignItem IsCreate(string channel, string time)
    {
        if (channel == "")
        {
            foreach (var d in m_result)
            {
                if (d.m_time == time)
                {
                    d.m_flag++;
                    return d;
                }
            }
        }

        dailySignItem item = new dailySignItem();
        m_result.Add(item);
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p=(ParamQuery)param;

        QueryCondition queryCond = new QueryCondition();

        if (string.IsNullOrEmpty(p.m_time))
             return OpRes.op_res_time_format_error;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);

        IMongoQuery imq = queryCond.getImq();
        return query(p, imq, user, m_fields);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user, string[] m_fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_SIGN, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            dailySignItem tmp = IsCreate(param.m_channelNo, time);
            tmp.m_time = time;
            
            if(data.ContainsKey("signCount"))
                tmp.m_signCount += Convert.ToInt32(data["signCount"]);

            //只有全部渠道时 m_flag有不为0的情况 避免注册人数重复相加
            if (tmp.m_flag != 0) 
                continue;

            //构造时间条件
            QueryCondition queryCond = new QueryCondition();
            DateTime mint = Convert.ToDateTime(tmp.m_time);
            DateTime maxt = mint.AddDays(1);
            IMongoQuery imq_1 = Query.And(Query.LT("genTime", BsonValue.Create(maxt)), Query.GTE("genTime", BsonValue.Create(mint)));
            //活跃老玩家人数
            tmp.m_oldPlayerLogin = DBMgr.getInstance().executeDistinct(TableName.PUMP_OLD_PLAYER_LOGIN, dip, "playerId", imq_1);

            //注册人数  
            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (serverId == -1)
                return OpRes.op_res_failed;

            List<Dictionary<string, object>> dataAct =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq_1, 0, 0, m_fields, "genTime", false); //降序
            if (dataAct != null && dataAct.Count != 0)
            {
                foreach (var aData in dataAct)
                {
                    tmp.m_actCount += Convert.ToInt32(aData["activeCount"]);
                }
            }

            //二挡奖励人数 VIP签到人数  获得话费人数 获得vip经验人数
            List<Dictionary<string, object>> dataResign =
             DBMgr.getInstance().executeQuery(TableName.PUMP_SIGN_PLAYER, dip, imq_1, 0, 0, m_fields1, "genTime");
            if (dataResign != null && dataResign.Count != 0)
            {
                foreach (var resign in dataResign)
                {
                    //二挡奖励人数
                    if (resign.ContainsKey("uplevelSignCount") && Convert.ToInt32(resign["uplevelSignCount"]) > 0)
                        tmp.m_uplevelSignCount += 1;

                    //VIP签到人数
                    if (resign.ContainsKey("vipLevel") && Convert.ToInt32(resign["vipLevel"]) > 0)
                        tmp.m_vipSignCount += 1;

                    //获得话费人数
                    if (resign.ContainsKey("getTicketCount") && Convert.ToInt32(resign["getTicketCount"]) > 0)
                        tmp.m_getTicketCount += 1;

                    //获得VIP经验人数
                    if (resign.ContainsKey("getVipExpCount") && Convert.ToInt32(resign["getVipExpCount"]) > 0)
                        tmp.m_getVipExpCount += 1;
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//签到情况分布表
public class paramSignByMonthItem
{
    public Dictionary<string, object> m_queryData = new Dictionary<string, object>();
    public Dictionary<string, object> m_curData = new Dictionary<string, object>();
    public Dictionary<string, object> m_lastData = new Dictionary<string, object>();

    public void reset()
    {
        m_queryData.Clear();
        m_curData.Clear();
        m_lastData.Clear();
    }
}
public class QuerySignByMonth : QueryBase
{
    private paramSignByMonthItem m_result = new paramSignByMonthItem();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamSignByMonth p = (ParamSignByMonth)param;

        int queryCond = p.m_year * 100 + p.m_month;
        IMongoQuery imq = Query.EQ("month", queryCond);
        query(p, imq, user, m_result.m_queryData);

        queryCond = DateTime.Now.Year * 100 + DateTime.Now.Month;
        imq = Query.EQ("month", queryCond);
        query(p, imq, user, m_result.m_curData);

        DateTime Last = DateTime.Now.AddMonths(-1);
        queryCond = Last.Year * 100 + Last.Month;
        imq = Query.EQ("month", queryCond);
        query(p, imq, user, m_result.m_lastData);

        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamSignByMonth param, IMongoQuery imq, GMUser user, Dictionary<string, object> dataRes)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.STAT_SIGN_PLAYER, dip, imq);

        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;

        int month = Convert.ToInt32(data["month"]);
        for (int j = 1; j <= 31; j++)
        {
            int day = Convert.ToInt32(data[j.ToString()]);
            dataRes.Add(j.ToString(), day);
        }
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//签到奖励领取
public class SignRewardItem 
{
    public string m_time;
    public Dictionary<int, int[]> m_signReward = new Dictionary<int, int[]>();
}
public class QueryDailySignReward : QueryBase 
{
    private List<SignRewardItem> m_result = new List<SignRewardItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        QueryCondition queryCond = new QueryCondition();

        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_need_at_least_one_cond;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq = Query.And(
            Query.LT("genTime", BsonValue.Create(maxt)), 
            Query.GTE("genTime", BsonValue.Create(mint))
        );

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_SIGN_REWARD, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int[] s_days = new int[] { 3, 6, 9, 12, 15};

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            SignRewardItem tmp = new SignRewardItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            foreach (int day in s_days) 
            {
                string strSign = "sign" + day;
                int sign = 0;
                if (data.ContainsKey(strSign))
                    sign = Convert.ToInt32(data[strSign]);

                string strReward = "reward" + day;
                int reward = 0;
                if (data.ContainsKey(strReward))
                    reward = Convert.ToInt32(data[strReward]);

                tmp.m_signReward.Add(day, new int[]{sign, reward});
            }
        }
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//玩家聊天记录查询
public class playerChatItem
{
    public string m_time;
    public string m_playerId;
    public string m_content;
}
public class QueryPlayerChat : QueryBase
{
    private List<playerChatItem> m_result = new List<playerChatItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        QueryCondition queryCond = new QueryCondition();

        if(string.IsNullOrEmpty(p.m_time) && string.IsNullOrEmpty(p.m_playerId))
        {
            return OpRes.op_res_need_at_least_one_cond;
        }

        if (!string.IsNullOrEmpty(p.m_time)) 
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq3 = Query.And(imq1, imq2);
            queryCond.addImq(imq3);
        }

        int playerId = 0;
        if (!string.IsNullOrEmpty(p.m_playerId))
        {
            if (!int.TryParse(p.m_playerId, out playerId))
            {
                return OpRes.op_res_param_not_valid;
            }
            queryCond.addImq(Query.EQ("playerId", playerId));
        }

        IMongoQuery imq = queryCond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_PLAYER_CHAT, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_CHAT, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            playerChatItem tmp = new playerChatItem();
            m_result.Add(tmp);

            tmp.m_playerId = Convert.ToString(data["playerId"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();
            tmp.m_content = Convert.ToString(data["content"]);
        }
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////
//自定义头像
public class PlayerIconCustomItem
{
    // 玩家ID
    public int m_playerId;
    // 玩家头像
    public string m_iconCustom = "";
    public string m_createTime;
    public int m_vipLevel;
    public int m_vipExp;
}
public class QueryPlayerIncomCustom : QueryBase
{
    private List<PlayerIconCustomItem> m_result = new List<PlayerIconCustomItem>();
    
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq_1 = Query.GTE("VipLevel", 2);
        IMongoQuery imq_2 = Query.NE("icon_custom","");
        IMongoQuery imq = Query.And(imq_1,imq_2);

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam .SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, dip);
        string[] fields = { "VipLevel", "VipExp", "icon_custom", "player_id", "create_time" };
        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, fields, "player_id", false);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, user.getDbServerID(),DbName.DB_PLAYER);

        if (data == null || data.Count <= 1)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < data.Count; i++)
        {
            PlayerIconCustomItem tmp = new PlayerIconCustomItem();
            tmp.m_playerId = Convert.ToInt32(data[i]["player_id"]);

            if(data[i].ContainsKey("icon_custom"))
            {
                string head = Convert.ToString(data[i]["icon_custom"]);
                if(string.IsNullOrEmpty(head))
                {
                    continue;
                }
                uint temp = Convert.ToUInt32(tmp.m_playerId) % 10000;
                string url = WebConfigurationManager.AppSettings["headURL"];
                tmp.m_iconCustom = string.Format(url, temp, head);

                if(data[i].ContainsKey("create_time"))
                {
                    tmp.m_createTime = Convert.ToDateTime(data[i]["create_time"]).ToLocalTime().ToString();
                }

                if(data[i].ContainsKey("VipExp"))
                {
                    tmp.m_vipExp = Convert.ToInt32(data[i]["VipExp"]);
                }

                if(data[i].ContainsKey("VipLevel"))
                {
                    tmp.m_vipLevel = Convert.ToInt32(data[i]["VipLevel"]);
                }
                m_result.Add(tmp);
            }
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
////////////////////////////////////////////////////////////////////////
public class GMAccountItem
{
    // 账号
    public string m_user = "";
    // 所在分组
    public string m_type = "";
}
// 查询当前所有GM账号
public class QueryGMAccount : QueryBase
{
    private List<GMAccountItem> m_result = new List<GMAccountItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.GM_ACCOUNT, 0, DbName.DB_ACCOUNT);

        if (data == null || data.Count <= 1)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            GMAccountItem tmp = new GMAccountItem();
            tmp.m_user = Convert.ToString(data[i]["user"]);
            tmp.m_type = Convert.ToString(data[i]["type"]);
            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//////////////////////////////////////////////////////////////////////////
// 玩家属性参数
public class PropParam
{
    public int m_playerId = 0;
    public string m_accountName = "";
    public string m_nickName = "";
}
public class MoneyItem
{
    // 生成时间
    public string m_genTime = "";
    // 动作类型
    public int m_actionType;
    // 属性类型
    public int m_propertyType;
    // 初始值
    public int m_startValue;
    // 结束值
    public int m_endValue;
    // 差额
    public int m_deltaValue;
    // 玩家ID
    public int m_playerId;
    // 昵称
    public string m_nickName = "";
    // 游戏id
    public int m_gameId;
    // 额外参数
    public string m_param = "";

    public int m_roomId;

    public string getPropertyName()
    {
        if (m_propertyType == (int)PropertyType.property_type_gold)
            return "金币";

        if (m_propertyType == (int)PropertyType.property_type_diamond)
            return "钻石";

        if (m_propertyType == (int)PropertyType.property_type_ticket)
            return "彩券";

        if (m_propertyType == (int)PropertyType.property_type_moshi)
            return "魔石";

        if (m_propertyType == (int)PropertyType.property_type_chip)
            return "碎片";

        return "";
    }

    // 返回动作名称
    public string getActionName()
    {
        XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");
        if (xml != null)
            return xml.getString(m_actionType.ToString(), "");
        return "";
    }
    //返回活动备注
    public string getNoteInformation() 
    {
        string m_result = "roomId:" + m_roomId + " ";
        if (string.IsNullOrEmpty(m_param))
        {
            return m_result;
        }else  //如果条件存在
        {
            //注：七日狂欢、每日任务、活跃开宝箱的m_param的形式是： quest：500001 
            //if (m_actionType == Convert.ToInt32(PropertyReasonType.type_reason_7_days_carnival) ||
            //    m_actionType == Convert.ToInt32(PropertyReasonType.type_reason_daily_task)) //七日狂欢  //每日任务 
            if (m_actionType == Convert.ToInt32(PropertyReasonType.type_reason_daily_task))//每日任务
            {
                string[] param = m_param.Split(':');
                int p = Convert.ToInt32(param[1]);
                MQuestCFGData tmp = new MQuestCFGData();
                var allMQuestData = MDailyQuestCFG.getInstance().getValue(p);
                if (allMQuestData == null)
                    return m_result;

                tmp.m_awardItemIDs = allMQuestData.m_awardItemIDs;
                tmp.m_awardItemCounts = allMQuestData.m_awardItemCounts;

                String[] tmpAwardItemIds = tmp.m_awardItemIDs.Split(',');
                String[] tmpAwardItemCounts = tmp.m_awardItemCounts.Split(',');
                int len = tmpAwardItemCounts.Length;
                for (int i = 0; i < len; i++)
                {
                    var mItemData = ItemCFG.getInstance().getValue(Convert.ToInt32(tmpAwardItemIds[i]));
                    if (mItemData == null)
                    {
                        m_result += "id:" + p + ",&ensp;奖励道具:" + tmpAwardItemIds[i] + ",&ensp;奖励数量:" + tmpAwardItemCounts[i] + "<br/>";
                    }
                    else
                    {
                        m_result += "id:" + p + ",&ensp;奖励道具:" + mItemData.m_itemName + ",&ensp;奖励数量:" + tmpAwardItemCounts[i] + "<br/>";
                    }
                    //switch (tmp.m_type)
                    //{
                    //    case 1:
                    //    case 2:  //1、2 每日任务
                            
                    //        break;
                    //    case 3: //七日狂欢
                    //        m_result += "id:" + p + ",&ensp;奖励道具:" + mItemData.m_itemName + ",&ensp;奖励数量:" + tmpAwardItemCounts[i] + "<br/>";
                    //        break;
                    //}
                }
                return m_result;
            }
            else if (m_actionType == Convert.ToInt32(PropertyReasonType.type_reason_active_box)) //活跃开宝箱
            {
                string[] param = m_param.Split(':');
                int p = Convert.ToInt32(param[1]);
                ActivityRewardCFGData tmp = new ActivityRewardCFGData();
                var allActivityRewardData = ActivityRewardCFG.getInstance().getValue(p);
                if (allActivityRewardData == null)
                    return m_result;

                tmp.m_activityRewardName = allActivityRewardData.m_activityRewardName;
                tmp.m_activityRewardCount = allActivityRewardData.m_activityRewardCount;
                m_result += "奖励道具:" + tmp.m_activityRewardName +",&ensp;奖励数量:" +tmp.m_activityRewardCount;
                return m_result;
            }else
            {
                return "roomId: "+ m_roomId +" "+ m_param;
            }
        }
    }

    public string getGameName()
    {
        return StrName.s_gameName3[m_gameId];
    }
}
// 玩家金币的查询参数
public class ParamMoneyQuery : ParamQuery
{
    // 过滤(动作类型)
    public int m_filter;

    // 属性
    public int m_property;

    // 值的范围
    public string m_range = "";

    public int m_gameId;
}
// 玩家金币查询
public class QueryPlayerMoney : QueryBase
{
    private List<MoneyItem> m_result = new List<MoneyItem>();
    protected static string[] m_field = { "nickname" };
    static string[] m_field1 = { "player_id" };
    private QueryCondition m_cond = new QueryCondition();

    // 作查询
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_cond.startQuery();
       // m_playerAcc = "";
        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

       // if (string.IsNullOrEmpty(m_playerAcc))
          //  return OpRes.op_res_not_found_data;

        IMongoQuery imq = m_cond.getImq();
        ParamMoneyQuery p = (ParamMoneyQuery)param;
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object p, GMUser user, QueryCondition queryCond)
    {
        ParamMoneyQuery param = (ParamMoneyQuery)p;
        int playerId = -1;

        switch (param.m_way)
        {
            case QueryWay.by_way0:  // 通过ID查询
                {
                    if (param.m_param != "")
                    {
                        if (!int.TryParse(param.m_param, out playerId))
                        {
                            return OpRes.op_res_param_not_valid;
                        }
                    }
                }
                break;
            case QueryWay.by_way1: // 通过账号查询
                {
                    OpRes res = queryByAccount(param, user, ref playerId);
                    if (res != OpRes.opres_success)
                    {
                        return res;
                    }
                }
                break;
            case QueryWay.by_way2: // 通过昵称查询
                {
                    OpRes res = queryByNickName(param, user, ref playerId);
                    if (res != OpRes.opres_success)
                    {
                        return res;
                    }
                }
                break;
        }

        int condCount = 0;

        if (param.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(param.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            condCount++;
            if (queryCond.isExport())
            {
                queryCond.addCond("time", param.m_time);
            }
            else
            {
                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                queryCond.addImq(Query.And(imq1, imq2));
            }
        }

        if (playerId != -1)
        {
            condCount++;
            queryCond.addQueryCond("playerId", playerId);
        }

        // 过滤
        if (param.m_filter != 0)
        {
            queryCond.addQueryCond("reason", param.m_filter);
        }
        // 属性
        if (param.m_property != (int)PropertyType.property_type_full)
        {
            queryCond.addQueryCond("itemId", param.m_property);
        }

        // 范围
        if (param.m_range != "")
        {
            if (!Tool.isTwoNumValid(param.m_range))
                return OpRes.op_res_param_not_valid;

            if (queryCond.isExport())
            {
                queryCond.addCond("range", param.m_range);
            }
            else
            {
                List<int> range = new List<int>();
                Tool.parseNumList(param.m_range, range);
                IMongoQuery imq1 = Query.LTE("addValue", BsonValue.Create(range[1]));
                IMongoQuery imq2 = Query.GTE("addValue", BsonValue.Create(range[0]));
                queryCond.addImq(Query.And(imq1, imq2));
            }
        }
        if (param.m_gameId != (int)GameId.gameMax)
        {
            queryCond.addQueryCond("gameId", param.m_gameId);
        }
        if (condCount == 0)
            return OpRes.op_res_need_at_least_one_cond;

        return OpRes.opres_success;
    }

    private OpRes queryByAccount(ParamMoneyQuery param, GMUser user, ref int id)
    {
        if (param.m_param != "")
        {
            Dictionary<string, object> data = getPlayerPropertyByAcc(param.m_param, user, m_field1);
            if (data == null)
            {
                return OpRes.op_res_not_found_data;
            }

            if (data.ContainsKey("player_id"))
            {
                id = Convert.ToInt32(data["player_id"]);
            }
        }

        return OpRes.opres_success;
    }

    private OpRes queryByNickName(ParamMoneyQuery param, GMUser user, ref int id)
    {
        if (param.m_param != "")
        {
            Dictionary<string, object> data = getPlayerPropertyByNickName(param.m_param, user, m_field1);
            if (data == null)
            {
                return OpRes.op_res_not_found_data;
            }

            if (data.ContainsKey("player_id"))
            {
                id = Convert.ToInt32(data["player_id"]);
            }
        }

        return OpRes.opres_success;
    }

    // 通过玩家ID查询
    protected virtual OpRes query(ParamMoneyQuery param, IMongoQuery imq, GMUser user)
    {
        // 查看满足条件的记当个数
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_PLAYER_MONEY, imq, user.getDbServerID(), DbName.DB_PUMP);

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_MONEY, user.getDbServerID(), DbName.DB_PUMP, imq,
                                              (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage,
                                              null, "genTime", false);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            MoneyItem tmp = new MoneyItem();
            tmp.m_genTime = Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToString();
            tmp.m_actionType = Convert.ToInt32(data[i]["reason"]);
            tmp.m_startValue = Convert.ToInt32(data[i]["oldValue"]);
            tmp.m_endValue = Convert.ToInt32(data[i]["newValue"]);
            tmp.m_deltaValue = Convert.ToInt32(data[i]["addValue"]);

            tmp.m_playerId = Convert.ToInt32(data[i]["playerId"]);
            Dictionary<string, object> ret = getPlayerProperty(tmp.m_playerId, user, m_field);
            if (ret != null && ret.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(ret["nickname"]);
            }
            tmp.m_propertyType = Convert.ToInt32(data[i]["itemId"]);
            if (data[i].ContainsKey("param"))
            {
                tmp.m_param = Convert.ToString(data[i]["param"]);
            }
            tmp.m_gameId = Convert.ToInt32(data[i]["gameId"]);

            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
public class MoneyItemDetail : MoneyItem
{
    //牌局ID
    public long m_cardBoardId = 0;
    // 投注
    public long m_outlay;
    // 返还
    public long m_income;

    public long m_playerWinBet;

    public string m_exInfo = "";

    public string getExParam(int index)
    {
        if (string.IsNullOrEmpty(m_exInfo))
            return "";

        if (m_gameId > 0)
        {
            URLParam uParam = new URLParam();
            uParam.m_text = "详情";
            uParam.m_key = "gameId";
            uParam.m_value = m_gameId.ToString();
            uParam.m_url = DefCC.ASPX_GAME_DETAIL;
            uParam.m_target = "_blank";
            uParam.addExParam("index", index);
            return Tool.genHyperlink(uParam);
        }
        return "";
    }
}
// 玩家金币查询
public class QueryPlayerMoneyDetail : QueryPlayerMoney
{
    private List<MoneyItemDetail> m_result1 = new List<MoneyItemDetail>();

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result1;
    }

    // 通过玩家ID查询
    protected override OpRes query(ParamMoneyQuery param, IMongoQuery imq, GMUser user)
    {
        m_result1.Clear();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        // 查看满足条件的记当个数
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_PLAYER_MONEY, imq, dip);

        List<Dictionary<string, object>> data = DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_MONEY, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage,null, "genTime", false);

        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;

//         string nickName = "";
//         Dictionary<string, object> ret = getPlayerPropertyByAcc(m_playerAcc, user, m_field);
//         if (ret != null && ret.ContainsKey("nickname"))
//         {
//             nickName = Convert.ToString(ret["nickname"]);
//         }

        for (int i = 0; i < data.Count; i++)
        {
            MoneyItemDetail tmp = new MoneyItemDetail();
            tmp.m_genTime = Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToString();
            tmp.m_actionType = Convert.ToInt32(data[i]["reason"]);
            tmp.m_startValue = Convert.ToInt32(data[i]["oldValue"]);
            tmp.m_endValue = Convert.ToInt32(data[i]["newValue"]);
            tmp.m_deltaValue = tmp.m_endValue - tmp.m_startValue; //Convert.ToInt64(data[i]["addValue"]);

            tmp.m_playerId = Convert.ToInt32(data[i]["playerId"]);
             Dictionary<string, object> retData = getPlayerProperty(tmp.m_playerId, user, m_field);
             if (retData != null && retData.ContainsKey("nickname"))
            {
                tmp.m_nickName = Convert.ToString(retData["nickname"]);
            }
            
            tmp.m_propertyType = Convert.ToInt32(data[i]["itemId"]);

            if(data[i].ContainsKey("cardBoardId"))
            {
                tmp.m_cardBoardId = Convert.ToInt64(data[i]["cardBoardId"]);
            }

            if (data[i].ContainsKey("exInfo"))
            {
                tmp.m_exInfo = Convert.ToString(data[i]["exInfo"]);
            }
            if (data[i].ContainsKey("param"))
            {
                tmp.m_param = Convert.ToString(data[i]["param"]);  
            }
            if (data[i].ContainsKey("playerWinBet"))
            {
                tmp.m_playerWinBet = Convert.ToInt64(data[i]["playerWinBet"]);
            }

            tmp.m_gameId = Convert.ToInt32(data[i]["gameId"]);
            //tmp.m_acc = m_playerAcc;

            if (data[i].ContainsKey("playerOutlay"))
            {
                tmp.m_outlay = Convert.ToInt32(data[i]["playerOutlay"]);
            }
            if (data[i].ContainsKey("playerIncome"))
            {
                tmp.m_income = Convert.ToInt32(data[i]["playerIncome"]);
            }

            if (data[i].ContainsKey("roomId"))
                tmp.m_roomId = Convert.ToInt32(data[i]["roomId"]);
            
            m_result1.Add(tmp);
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
public class LotteryItem
{
    // 生成时间
    public string m_genTime = "";
    public int m_playerId;
    public int m_boxId;
    public int m_cost;
    public List<ParamItem> m_rewardList = new List<ParamItem>();

    public string getRewardList()
    {
        return ItemHelp.getRewardList(m_rewardList);
    }
}
public class ParamLottery : ParamQuery
{
    public string m_playerId;
    public string m_boxId;
}
//////////////////////////////////////////////////////////////////////////
public class MailItem
{
    public string m_genTime = "";
    public int m_playerId;
    public List<ParamItem> m_rewardList = new List<ParamItem>();

    public string getRewardList()
    {
        return ItemHelp.getRewardList(m_rewardList);
    }
}

// 邮件
public class QueryMail : QueryBase
{
    private List<MailItem> m_result = new List<MailItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamLottery p = (ParamLottery)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        int condCount = 0;

        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            condCount++;
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            queryList.Add(Query.And(imq1, imq2));
        }
        if (!string.IsNullOrEmpty(p.m_playerId))
        {
            try
            {
                int id = Convert.ToInt32(p.m_playerId);
                queryList.Add(Query.EQ("playerId", BsonValue.Create(id)));
                condCount++;
            }
            catch (System.Exception ex)
            {
                return OpRes.op_res_param_not_valid;
            }
        }
        
        if (condCount == 0)
            return OpRes.op_res_need_at_least_one_cond;

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamLottery param, IMongoQuery imq, GMUser user)
    {
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_MAIL, imq, user.getDbServerID(), DbName.DB_PUMP);

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.PUMP_MAIL, user.getDbServerID(), DbName.DB_PUMP, imq,
                                              (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            MailItem tmp = new MailItem();
            tmp.m_genTime = Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToString();
            tmp.m_playerId = Convert.ToInt32(data[i]["playerId"]);
            Tool.parseItemFromDic(data[i]["items"] as Dictionary<string, object>, tmp.m_rewardList);
            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ServiceInfoItem
{
    public string m_platChaName = "";
    public string m_platEngName = "";
    public string m_info = "";
}

// 客服信息
public class QueryServiceInfo : QueryBase
{
    private List<ServiceInfoItem> m_result = new List<ServiceInfoItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        int accServerId = user.getDbServerID();
        if (accServerId == -1)
            return OpRes.op_res_failed;

        m_result.Clear();
        List<Dictionary<string, object>> data = 
            DBMgr.getInstance().executeQuery(TableName.SERVICE_INFO, accServerId, DbName.DB_PLAYER);

        if (data != null)
        {
            for (int i = 0; i < data.Count; i++)
            {
                ServiceInfoItem info = new ServiceInfoItem();
                m_result.Add(info);

                info.m_info = Convert.ToString(data[i]["info"]);
                info.m_platEngName = Convert.ToString(data[i]["plat"]);

                PlatformInfo pi = ResMgr.getInstance().getPlatformInfoByName(info.m_platEngName);
                if (pi != null)
                {
                    info.m_platChaName = pi.m_chaName;
                }
            }
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}

//////////////////////////////////////////////////////////////////////////

// 查询结果
public class ResultQueryAccount
{
    // 账号
    public string m_account = "";
    // ID
    public int m_id;
    // 昵称
    public string m_nickName = "";
    // 平台
    public string m_platForm = "";
    // 角色创建时间
    public string m_createTime = "";
    // VIP等级
    public int m_vipLevel;
    // VIP经验
    public int m_vipExp;

    //最大炮数
    public int m_MaxFireCount;
    //历史彩券
    public long m_HistoryLottery;

    // 上次登陆时间
    public string m_lastLoginTime = "";
    // 上次登陆IP
    public string m_lastLoginIP = "";
    // 金币
    public int m_gold;
    // 保险箱中的金币
    public int m_safeBoxGold;
    // 龙珠 
    public int m_dragonBall;
    // 礼券
    public int m_ticket;

    public int m_diamond;
    // 绑定手机
    public string m_bindMobile = "";
    // 账号状态
    public string m_accountState = "";
    //acc
    public string m_acc = "";

    //设备号
    public string m_deviceId = "";

    public string getExParam()
    {
        if (m_id > 0)
        {
            URLParam uParam = new URLParam();
            uParam.m_text = "详情";
            uParam.m_key = "playerId";
            uParam.m_value = m_id.ToString();
            uParam.m_url = DefCC.ASPX_ACCOUNT_BEIBAO;
            uParam.m_target = "_blank";
            return Tool.genHyperlink(uParam);
        }
        return "";
    }
}

// 账号查询
public class QueryAccount : QueryBase
{
    private List<ResultQueryAccount> m_result = new List<ResultQueryAccount>();

    private string[] m_playerFields = { "account", "player_id", "acc","nickname", "platform", "create_time", "VipLevel",
                                        "VipExp", "logout_time", "gold", "safeBoxGold", "ticket", "delete", "bindPhone",
                                        "diamond", "lastIp ","hasLotteryChipCount","GameLevel" ,"deviceId"};

    private string[] m_phoneFields = { "phone", "block", "acc" };

    private static string[] m_accFiled = { "acc_real" };

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        if (String.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_need_at_least_one_cond;

        switch (p.m_way)
        {
            case QueryWay.by_way0:  // 以玩家id查询
                {
                    int id = 0;
                    if (!int.TryParse(p.m_param, out id))
                    {
                        return OpRes.op_res_param_not_valid;
                    }
                    return queryById(id, user);
                }
            case QueryWay.by_way1:  // 以玩家账号查询
                {
                    return queryByAccount(p.m_param, user);
                }
            case QueryWay.by_way2:  // 以玩家昵称查询
                {
                    return queryByNickname(p.m_param, user);
                }
            case QueryWay.by_way3:  // 以登陆IP查询
                {
                    return queryByIP(p.m_param, user);
                }
            case QueryWay.by_way4:  // 以手机号查询
                {
                    return queryByBindPhone(p.m_param, user);
                }
            case QueryWay.by_way5: //通过设备号查询
                {
                    return queryByDeviceId(p,user);
                }
            ////case QueryWay.by_way5: //以deviceID查询
            ////    {
            ////        return queryByDeviceId(p.m_param,user);
            ////    }
            ////    break;
        }
        return OpRes.op_res_failed;
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    // 通过玩家ID查询
    private OpRes queryById(int id, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> ret = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", id, m_playerFields, dip);
        return query(ret, user);
    }

    private OpRes queryByAccount(string queryStr, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> ret = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "account", queryStr, m_playerFields, dip);
        if (ret == null)
        {
            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (serverId != -1)
            {
                ret = DBMgr.getInstance().getTableData(TableName.PLAYER_ACCOUNT, "acc", queryStr, m_accFiled, serverId, DbName.DB_ACCOUNT);
                if (ret != null)
                {
                    string realAcc = Convert.ToString(ret["acc_real"]);
                    ret = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "account", realAcc, m_playerFields, dip);
                }
            }
        }
        return query(ret, user);
    }

    private OpRes queryByNickname(string queryStr, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> ret = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "nickname", queryStr, m_playerFields, dip);
        return query(ret, user);
    }

    private OpRes queryByBindPhone(string queryStr, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> ret = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "bindPhone", queryStr, m_playerFields, dip);
        return query(ret, user);
    }

    private OpRes queryByIP(string queryStr, GMUser user)
    {
        return OpRes.op_res_failed;
    }

    //通过deviceID查询
    private OpRes queryByDeviceId(ParamQuery param, GMUser user)  
    {
        IMongoQuery imq = Query.EQ("deviceId",param.m_param);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, dip);
        List<Dictionary<string, object>> dataList =
            DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO, dip, imq,
                                             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, m_playerFields);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            query(data, user);
        }
        return OpRes.opres_success;

    }
    private OpRes query(Dictionary<string, object> ret, GMUser user)
    {
        if (ret == null)
            return OpRes.op_res_not_found_data;

        if (!ret.ContainsKey("account"))
            return OpRes.op_res_not_found_data;

        // 账号
        string acc = Convert.ToString(ret["account"]);
        int accServerId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
       
        ResultQueryAccount tmp = new ResultQueryAccount();
        tmp.m_account = acc;
        //if (ret["platform"].ToString() == "default")
        //{
            Dictionary<string, object> dataAccount = DBMgr.getInstance().getTableData(TableName.PLAYER_ACCOUNT, "acc_real", acc, m_phoneFields, accServerId, DbName.DB_ACCOUNT);
            if (dataAccount!=null)
            {
                 if (dataAccount.ContainsKey("acc")){
                    tmp.m_acc = Convert.ToString(dataAccount["acc"]);
                }else{
                    tmp.m_acc = "";
                }           
 			}else
            {
                tmp.m_acc = "";
            }
        //}
//         else {
//             tmp.m_acc = "";
//         }
        tmp.m_id = Convert.ToInt32(ret["player_id"]);
        if (ret.ContainsKey("nickname"))
            tmp.m_nickName = Convert.ToString(ret["nickname"]);

        tmp.m_platForm = Convert.ToString(ret["platform"]);
        tmp.m_createTime = Convert.ToDateTime(ret["create_time"]).ToLocalTime().ToString();
        tmp.m_vipLevel = Convert.ToInt32(ret["VipLevel"]);
        tmp.m_vipExp = Convert.ToInt32(ret["VipExp"]);
        tmp.m_lastLoginTime = Convert.ToDateTime(ret["logout_time"]).ToLocalTime().ToString();
        tmp.m_gold = Convert.ToInt32(ret["gold"]);

        if (ret.ContainsKey("safeBoxGold"))
            tmp.m_safeBoxGold = Convert.ToInt32(ret["safeBoxGold"]);
        
        tmp.m_ticket = Convert.ToInt32(ret["ticket"]);

        if (ret.ContainsKey("diamond"))
            tmp.m_diamond = Convert.ToInt32(ret["diamond"]);

        bool isBlock = false;
        if (ret.ContainsKey("delete"))
            isBlock = Convert.ToBoolean(ret["delete"]);
        tmp.m_accountState = isBlock ? "停封" : "正常";

        if (ret.ContainsKey("bindPhone"))
            tmp.m_bindMobile = Convert.ToString(ret["bindPhone"]);

        if (ret.ContainsKey("deviceId"))
            tmp.m_deviceId = Convert.ToString(ret["deviceId"]);

        if (ret.ContainsKey("lastIp"))
            tmp.m_lastLoginIP = Convert.ToString(ret["lastIp"]);

        if(ret.ContainsKey("hasLotteryChipCount"))
            tmp.m_HistoryLottery = Convert.ToInt64(ret["hasLotteryChipCount"]);

        if(ret.ContainsKey("GameLevel"))
        {
            int m_level = Convert.ToInt32(ret["GameLevel"]);
            var fish_Level = Fish_LevelCFG.getInstance().getValue(m_level);
            if (fish_Level != null)
            {
                tmp.m_MaxFireCount = fish_Level.m_openRate;
            }
        }

        m_result.Add(tmp);
        return OpRes.opres_success;
    }
}

public class AccountBeibaoItem 
{
    public string m_playerId;
    public ParamItem m_itemList = new ParamItem();
}

public class QueryTypeAccountBeibao : QueryBase
{
    private List<AccountBeibaoItem> m_result = new List<AccountBeibaoItem>();
    Dictionary<string, object> item = new Dictionary<string, object>();
    string[] m_fileName = {"player_id", "items" };
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p=(ParamQuery)param;
        IMongoQuery imq = Query.EQ("player_id",Convert.ToInt32(p.m_playerId));
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        
        Dictionary<string,object> data = DBMgr.getInstance().getTableData(TableName.FISHLORD_PLAYER, dip, imq,m_fileName);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        string playerId=Convert.ToString(data["player_id"]);
        object[] arr = (object[])data["items"];
        for (int k = 0; k < arr.Length; k++)
        {
            AccountBeibaoItem tmp = new AccountBeibaoItem();
            tmp.m_playerId = playerId;
            Tool.parseItemFromDicNew(arr[k] as Dictionary<string, object>, tmp.m_itemList);
            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}

//////////////////////////////////////////////////////////////////////////

public class LoginItem
{
    public string m_account = "";
    public string m_time = "";
    public string m_ip = "";
}

// 登陆历史
public class QueryLogin : QueryBase
{
    private List<LoginItem> m_result = new List<LoginItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        int condCount = 0;
        if (p.m_param != "")
        {
            queryList.Add(Query.EQ("acc", BsonValue.Create(p.m_param)));
        }
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            condCount++;
            IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
            queryList.Add(Query.And(imq1, imq2));
        }
        
        if (condCount == 0)
            return OpRes.op_res_need_at_least_one_cond;

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_LOGIN, imq, serverId, DbName.DB_ACCOUNT);

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.PLAYER_LOGIN, serverId, DbName.DB_ACCOUNT, imq,
                                              (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            LoginItem tmp = new LoginItem();
            tmp.m_time = Convert.ToDateTime(data[i]["time"]).ToLocalTime().ToString();
            tmp.m_account = Convert.ToString(data[i]["acc"]);
            tmp.m_ip = Convert.ToString(data[i]["ip"]);
            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class GiftItem
{
    public string m_giftId = "";
    public DateTime m_deadTime;
    public List<ParamItem> m_giftList = new List<ParamItem>();

    public string getGiftList()
    {
        return ItemHelp.getRewardList(m_giftList);
    }

    // 返回源串形式,以分号相隔
    public string getSrcGiftList()
    {
        string str = "";
        if (m_giftList.Count > 0)
        {
            str += m_giftList[0].m_itemId + " " + m_giftList[0].m_itemCount;
        }

        for (int i = 1; i < m_giftList.Count; i++)
        {
            str += ";" + m_giftList[i].m_itemId + " " + m_giftList[i].m_itemCount;
        }
        return str;
    }
}

// 查询礼包
public class QueryGift : QueryBase
{
    private List<GiftItem> m_result = new List<GiftItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;
        switch (p.m_way)
        {
            case QueryWay.by_way0:  // 已过期
                {
                    imq = Query.LTE("deadTime", BsonValue.Create(DateTime.Now));
                }
                break;
            case QueryWay.by_way1:  // 未过期
                {
                    imq = Query.GT("deadTime", BsonValue.Create(DateTime.Now));
                }
                break;
        }
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.GIFT, imq, serverId, DbName.DB_ACCOUNT);

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.GIFT, serverId, DbName.DB_ACCOUNT, imq,
                                              (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            GiftItem tmp = new GiftItem();
            tmp.m_giftId = Convert.ToString(data[i]["giftId"]);
            Tool.parseItemFromDic(data[i]["item"] as Dictionary<string, object>, tmp.m_giftList);
            tmp.m_deadTime = Convert.ToDateTime(data[i]["deadTime"]).ToLocalTime(); //.ToString();
            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class GiftCodeItem
{
    public string m_genTime = "";
    public string m_giftCode = "";
    public string m_giftId = "";
    public string m_plat = "";

    public int m_playerServerId;
    public string playerPlat = "";
    public int m_playerId;
    public string m_playerAcc = "";
    public string m_useTime = "";
}

// 查询礼包码
public class QueryGiftCode : QueryBase
{
    private List<GiftCodeItem> m_result = new List<GiftCodeItem>();
    private QueryCondition m_cond = new QueryCondition();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        m_cond.startQuery();
        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        IMongoQuery imq = m_cond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.GIFT_CODE, imq, serverId, DbName.DB_ACCOUNT);

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.GIFT_CODE, serverId, DbName.DB_ACCOUNT, imq,
                                              (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            GiftCodeItem tmp = new GiftCodeItem();
            tmp.m_genTime = Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToString();
            tmp.m_giftCode = Convert.ToString(data[i]["giftCode"]);
            tmp.m_giftId = Convert.ToString(data[i]["giftId"]);
            tmp.m_plat = Convert.ToString(data[i]["plat"]);

            tmp.m_playerServerId = Convert.ToInt32(data[i]["playerServerId"]);
            tmp.playerPlat = Convert.ToString(data[i]["playerPlat"]);
            tmp.m_playerId = Convert.ToInt32(data[i]["playerId"]);
            tmp.m_playerAcc = Convert.ToString(data[i]["playerAcc"]);
            tmp.m_useTime = Convert.ToDateTime(data[i]["useTime"]).ToLocalTime().ToString();
            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQuery p = (ParamQuery)param;

        if (!string.IsNullOrEmpty(p.m_param))
        {
            queryCond.addQueryCond("giftCode", p.m_param);
        }
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            if (queryCond.isExport())
            {
                queryCond.addCond("time", p.m_time);
            }
            else
            {
                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                queryCond.addImq(Query.And(imq1, imq2));
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamQueryGift : ParamQuery
{
    public int m_state;
    public int m_type;
    public int m_roomId;
}

public enum ExState
{
    success,
    wait,
}

public class ExchangeItem
{
    // 兑换
    public string m_exchangeId;

    //兑换ID
    public int m_chgId;

    public int m_playerId;

    // 手机号
    public string m_phone = "";

    // 道具名称
    public string m_itemName = "";

    // 兑换时间
    public string m_exchangeTime = "";

    // 发放时间
    public string m_giveOutTime = "";

    public bool m_isReceive;

    //历史充值金额
    public int m_recharged;

    //绑定手机号
    public string m_bindPhone = "";

    public string getStateName()
    {
        if (m_isReceive)
            return "已发放";

        return "未发放";
    }

    //道具
    public string[] getChgItem() 
    {
       string[] item = new string[2];
       var chgItem = ExchangeCfg.getInstance().getValue(m_chgId);
       if (chgItem != null)
       {
           item[0] = chgItem.m_name;
           item[1] = chgItem.m_itemCount.ToString();
       }

       return item;
    }
}

// 兑换查询
public class QueryExchange : QueryBase
{
    private List<ExchangeItem> m_result = new List<ExchangeItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQueryGift p = (ParamQueryGift)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();

        if (p.m_param != "")
        {
            int playerId = 0;
            if (!int.TryParse(p.m_param, out playerId))
                return OpRes.op_res_param_not_valid;

            queryList.Add(Query.EQ("playerId", BsonValue.Create(playerId)));
        }

        bool isReceive = (p.m_state == 0);  //兑换状态
        queryList.Add(Query.EQ("isReceive", BsonValue.Create(isReceive)));

        //兑换类型
        if (p.m_type == 0)   //话费 或 京东卡
        {
            queryList.Add(
                Query.Or(
                    Query.EQ("itemId", BsonValue.Create(1002)),
                    Query.EQ("itemId", BsonValue.Create(1003)),
                    Query.EQ("itemId", BsonValue.Create(1004))
                ));
        }
        else
        {
            queryList.Add(
                Query.And(
                    Query.NE("itemId", BsonValue.Create(1002)),
                    Query.NE("itemId", BsonValue.Create(1003)),
                    Query.NE("itemId", BsonValue.Create(1004))
                ));
        }

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.EXCHANGE, imq, dip);

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.EXCHANGE, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < data.Count; i++)
        {
            ExchangeItem tmp = new ExchangeItem();
            m_result.Add(tmp);
            tmp.m_exchangeId = Convert.ToString(data[i]["exchangeId"]);
            tmp.m_chgId = Convert.ToInt32(data[i]["chgId"]);
            tmp.m_playerId = Convert.ToInt32(data[i]["playerId"]);
            tmp.m_phone = Convert.ToString(data[i]["phone"]);
            tmp.m_exchangeTime = Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToString();
            tmp.m_isReceive = Convert.ToBoolean(data[i]["isReceive"]);

            if (data[i].ContainsKey("giveOutTime"))
                tmp.m_giveOutTime = Convert.ToDateTime(data[i]["giveOutTime"]).ToLocalTime().ToString();

            //bindPhone  recharged
            string[] m_field = { "bindPhone", "recharged" };
            Dictionary<string, object> daInfo = DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", tmp.m_playerId, m_field, dip);
            if (daInfo != null)
            {
                if (daInfo.ContainsKey("bindPhone"))
                    tmp.m_bindPhone = daInfo["bindPhone"].ToString();

                if (daInfo.ContainsKey("recharged"))
                    tmp.m_recharged = Convert.ToInt32(daInfo["recharged"]);
            }
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ResultLobby
{
    public int m_statType;

    public int[] m_generalStat = new int[(int)DataStatType.stat_max];

    // 赠送礼物
    public Dictionary<int, int> m_sendGift = new Dictionary<int, int>();

    // 相框
    public Dictionary<int, int> m_photoFrame = new Dictionary<int, int>();

    public void reset()
    {
        m_statType = -1;
        for (int i = 0; i < m_generalStat.Length; i++)
        {
            m_generalStat[i] = 0;
        }
        m_sendGift.Clear();
        m_photoFrame.Clear();
    }

    public void addGift(int key, int value)
    {
        if (m_sendGift.ContainsKey(key))
        {
            m_sendGift[key] += value;
        }
        else
        {
            m_sendGift.Add(key, value);
        }
    }

    public void addPhotoFrame(int key, int value)
    {
        if (m_photoFrame.ContainsKey(key))
        {
            m_photoFrame[key] += value;
        }
        else
        {
            m_photoFrame.Add(key, value);
        }
    }

    // 返回统计详情
    public string getValue(int statType)
    {
        if (statType == (int)DataStatType.stat_send_gift)
        {
            return ItemHelp.getRewardList(m_sendGift);
        }

        if (statType == (int)DataStatType.stat_player_vip_level)
            return "";

        if (statType == (int)DataStatType.stat_photo_frame)
        {
            return ItemHelp.getRewardList(m_photoFrame);
        }

        return m_generalStat[statType].ToString();
    }
}

// 查询大厅通用数据，独立数据
public class QueryLobby : QueryBase
{
    private ResultLobby m_result = new ResultLobby();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();

        int statType = (int)param;
        if (statType == 1) // 全部
        {
            return queryAll(user);
        }
        return OpRes.op_res_failed;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes queryAll(GMUser user)
    {
        // 一般统计
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_GENERAL_STAT, dip);
        for (int i = 0; i < dataList.Count; i++)
        {
            int key = Convert.ToInt32(dataList[i]["key"]);
            int value = Convert.ToInt32(dataList[i]["value"]);
            m_result.m_generalStat[key] += value;
        }

        // 赠送礼物
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_SEND_GIFT, dip);
        for (int i = 0; i < dataList.Count; i++)
        {
            int key = Convert.ToInt32(dataList[i]["key"]);
            int value = Convert.ToInt32(dataList[i]["value"]);
            m_result.addGift(key, value);
        }

        // 相框
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_PHOTO_FRAME, dip);
        for (int i = 0; i < dataList.Count; i++)
        {
            int key = Convert.ToInt32(dataList[i]["key"]);
            int value = Convert.ToInt32(dataList[i]["value"]);
            m_result.addPhotoFrame(key, value);
        }

        m_result.m_statType = 0;

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class EarningItem
{
    public string m_time = "";

    // 系统总支出
//    public long m_totalOutlay;

    // 系统总收入
 //   public long m_totalIncome;

    /*
     *      索引0 初级房  索引1 中级房 索引2 高级场 索引3 VIP场  索引4 至尊场 索引5 竞技场  索引6 总计 
     *      索引7 初级场赠予  索引8  中级场赠予
     */
    public long[] m_roomIncome = new long[10];
    public long[] m_roomOutlay = new long[10];
    public long[] m_roomPlayerPump = new long[10];
    /* 
     *  索引0  初级房废弹 
     *  索引1  中级房废弹 
     *  索引2  高级场废弹 
     *  索引3  VIP场废弹
     *  索引4  至尊场
     *  索引5  竞技场
     */
    public long[] m_roomAbandonedbullets = new long[10];

    // 返回实际盈利率
    public string getFactExpRate(int roomId)
    {
        if (m_roomIncome[roomId] == 0)
            return "0";
        double factGain = factGain = (double)(m_roomIncome[roomId] + m_roomPlayerPump[roomId] - m_roomOutlay[roomId]) / (m_roomIncome[roomId] + m_roomPlayerPump[roomId]);
        return Math.Round(factGain, 3).ToString();
    }

    //// 牛牛 返回实际盈利率
    //public string getFactExpRate1(int roomId)
    //{
    //    if (m_roomIncome[roomId] == 0)
    //        return "0";
    //    double factGain = factGain = (double)(m_roomIncome[roomId] + m_roomPlayerPump[roomId] - m_roomOutlay[roomId]) / (m_roomIncome[roomId] + m_roomPlayerPump[roomId]);
       
    //    return Math.Round(factGain, 3).ToString();
    //}

    // roomId从0开始
    public void addRoomIncome(int roomId, long addValue)
    {
        m_roomIncome[roomId] += addValue;
    }

    // roomId从0开始
    public void addRoomOutlay(int roomId, long addValue)
    {
        m_roomOutlay[roomId] += addValue;
    }

    //抽水
    public void addRoomPlayerPump(int roomId,long addValue) 
    {
        m_roomPlayerPump[roomId] += addValue;
    }

    public long getRoomIncome(int roomId)
    {
        return m_roomIncome[roomId];
    }

    public long getRoomOutlay(int roomId)
    {
        return m_roomOutlay[roomId];
    }

    public long getDelta(int roomId)
    {
        return m_roomIncome[roomId] + m_roomPlayerPump[roomId] - m_roomOutlay[roomId];
    }

    public long getRoomPlayerPump(int roomId)  //抽水
    {
        return m_roomPlayerPump[roomId];
    }

    // roomId从0开始
    public void addRoomAbandonedbullets(int roomId, long addValue)
    {
        m_roomAbandonedbullets[roomId] += addValue;
    }

    // roomId从0开始
    public long getRoomAbandonedbullets(int roomId)
    {
        return m_roomAbandonedbullets[roomId];
    }
}

// 收益查询参数
public class ParamServerEarnings : ParamQuery
{
    // 游戏ID
    public int m_gameId;
}

public class ResultServerEarningsTotal : PlayerTypeDataCollect<EarningItem>
{
    public PlayerTypeData<EarningItem> sum(int[] ids)
    {
        PlayerTypeData<EarningItem> result = new PlayerTypeData<EarningItem>();
        foreach (var data in m_data)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                var item = data.Value.getData(ids[i]);
                if (item != null)
                {
                    var res = result.getData(ids[i]);
                    if (res == null)
                    {
                        res = new EarningItem();
                        result.addData(ids[i], res);
                    }

                    res.addRoomIncome(6, item.getRoomIncome(6));
                    res.addRoomOutlay(6, item.getRoomOutlay(6));
                    res.addRoomIncome(0, item.getRoomIncome(0));
                }
            }
        }
        return result;
    }
}

// 服务器收益
public class QueryServerEarnings : QueryBase
{
    private List<EarningItem> m_result = new List<EarningItem>();

    private Dictionary<DateTime, EarningItem> m_total = new Dictionary<DateTime, EarningItem>();

    private ResultServerEarningsTotal m_totalResult = new ResultServerEarningsTotal();

    List<ResultActive> m_activePerson = null;

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        IMongoQuery imq = null;
        ParamServerEarnings p = (ParamServerEarnings)param;

        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_param_not_valid;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("Date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("Date", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);
        
        int gameId = p.m_gameId;
        if (gameId == (int)GameId.fishlord)
        {
            return queryCommon(user, imq, TableName.PUMP_FISHLORD_EVERY_DAY);
        }
        else if (gameId == (int)GameId.crocodile)
        {
            return queryCommon(user, imq, TableName.PUMP_CROCODILE_EVERY_DAY);
        }
        else if (gameId == (int)GameId.dice)
        {
            return queryCommon(user, imq, TableName.PUMP_DICE_EVERY_DAY);
        }
        else if (gameId == (int)GameId.cows)
        {
            return queryCommon(user, imq, TableName.PUMP_COWS_EVERY_DAY);
        }
        else if (gameId == (int)GameId.baccarat)
        {
            return queryCommon(user, imq, TableName.PUMP_BACCARAT_EVERY_DAY);
        }
        else if (gameId == (int)GameId.dragon)
        {
            return queryCommon(user, imq, TableName.PUMP_DRAGON_EVERY_DAY);
        }
        else if (gameId == (int)GameId.fishpark)
        {
            return queryCommon(user, imq, TableName.PUMP_FISHPARK_EVERY_DAY);
        }
        else if (gameId == (int)GameId.shcd)
        {
            return queryCommon(user, imq, TableName.PUMP_SHCD_EVERY_DAY);
        }
        else if (gameId == (int)GameId.calf_roping)
        {
            return queryCommon(user, imq, TableName.PUMP_CALFROPING_EVERY_DAY);
        }
        else if (gameId == (int)GameId.shuihz)
        {
            return queryCommon(user, imq, TableName.SHUIHZ_DAILY);
        }
        else if (gameId == (int)GameId.bz)
        {
            return queryCommon(user, imq, TableName.BZ_DAILY);
        }
        else if (gameId == (int)GameId.fruit)
        {
            return queryCommon(user, imq, TableName.FRUIT_EVERY_DAY);
        }
        else if (gameId == (int)GameId.jewel)
        {
            return queryCommon(user, imq, TableName.JEWEL_EVERY_DAY_STAT); //宝石迷阵
        }
        else if (gameId == 0)
        {
            statTotal(user, imq, p);
            return OpRes.opres_success;
        }
        return OpRes.op_res_failed;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user)
    {
        return m_totalResult;
    }

    private OpRes queryCommon(GMUser user, IMongoQuery imq, string tableName)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip, imq, 0, 0, null, "Date", false);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            EarningItem item = new EarningItem();
            m_result.Add(item);

            Dictionary<string, object> data = dataList[i];
            item.m_time = Convert.ToDateTime(data["Date"]).ToLocalTime().ToShortDateString();

            long todayOutlay = 0;
            long todayIncome = 0;
            if (tableName == TableName.JEWEL_EVERY_DAY_STAT)
            {
                if (dataList[i].ContainsKey("TodayOutlay"))
                    todayOutlay += Convert.ToInt64(dataList[i]["TodayOutlay"]);
            }
            else
            {
                if (dataList[i].ContainsKey("room1Outlay"))
                    todayOutlay += Convert.ToInt64(dataList[i]["room1Outlay"]);
            }
            if (dataList[i].ContainsKey("room2Outlay"))
            {
                if(tableName == TableName.PUMP_COWS_EVERY_DAY || tableName == TableName.PUMP_SHCD_EVERY_DAY) //牛牛黑红 龙珠场
                {
                    todayOutlay += 5000 * Convert.ToInt64(dataList[i]["room2Outlay"]);
                }else{
                    todayOutlay += Convert.ToInt64(dataList[i]["room2Outlay"]);
                }
            }
            if (dataList[i].ContainsKey("room3Outlay"))
            {
                todayOutlay += Convert.ToInt64(dataList[i]["room3Outlay"]);
            }
            if (dataList[i].ContainsKey("room4Outlay"))
            {
                todayOutlay += Convert.ToInt64(dataList[i]["room4Outlay"]);
            }
            if (dataList[i].ContainsKey("room11Outlay"))
            {
                todayOutlay += Convert.ToInt64(dataList[i]["room11Outlay"]);
            }
            if (dataList[i].ContainsKey("room15Outlay"))
            {
                todayOutlay += Convert.ToInt64(dataList[i]["room15Outlay"]);
            }
            if (dataList[i].ContainsKey("room20Outlay"))
            {
                todayOutlay += Convert.ToInt64(dataList[i]["room20Outlay"]);
            }
            item.addRoomOutlay(6, todayOutlay);

            if (tableName == TableName.JEWEL_EVERY_DAY_STAT)
            {
                if (dataList[i].ContainsKey("TodayIncome"))
                    todayIncome += Convert.ToInt64(dataList[i]["TodayIncome"]);
            }
            else
            {
                if (dataList[i].ContainsKey("room1Income"))
                    todayIncome += Convert.ToInt64(dataList[i]["room1Income"]);
            }
            if (dataList[i].ContainsKey("room2Income"))
            {
                if (tableName == TableName.PUMP_COWS_EVERY_DAY || tableName == TableName.PUMP_SHCD_EVERY_DAY) //牛牛黑红龙珠
                {
                    todayIncome += 5000 * Convert.ToInt64(dataList[i]["room2Income"]);
                }else 
                {
                    todayIncome += Convert.ToInt64(dataList[i]["room2Income"]);
                }
            }
            if (dataList[i].ContainsKey("room3Income"))
            {
                todayIncome += Convert.ToInt64(dataList[i]["room3Income"]);
            }
            if (dataList[i].ContainsKey("room4Income"))
            {
                todayIncome += Convert.ToInt64(dataList[i]["room4Income"]);
            }
            if (dataList[i].ContainsKey("room11Income"))
            {
                todayIncome += Convert.ToInt64(dataList[i]["room11Income"]);
            }
            if (dataList[i].ContainsKey("room15Income"))
            {
                todayIncome += Convert.ToInt64(dataList[i]["room15Income"]);
            }
            if (dataList[i].ContainsKey("room20Income"))
            {
                todayIncome += Convert.ToInt64(dataList[i]["room20Income"]);
            }
            item.addRoomIncome(6, todayIncome);
            ///////////////////////////////////////////////////////
            if (data.ContainsKey("TodayPlayerPump"))
            {
                item.addRoomPlayerPump(6, Convert.ToInt64(data["TodayPlayerPump"]));
            }
            if (data.ContainsKey("room1Protect")) //赠予
            {
                item.addRoomOutlay(7, Convert.ToInt64(data["room1Protect"]));
            }
            if (data.ContainsKey("room2Protect"))
            {
                item.addRoomOutlay(8, Convert.ToInt64(data["room2Protect"]));
            }

            if (tableName == TableName.JEWEL_EVERY_DAY_STAT) //宝石迷阵
            {
                if (data.ContainsKey("TodayIncome")) //初级
                    item.addRoomIncome(0, Convert.ToInt64(data["TodayIncome"]));
            }
            else
            {
                if (data.ContainsKey("room1Income")) //初级
                    item.addRoomIncome(0, Convert.ToInt64(data["room1Income"]));
            }
            if (data.ContainsKey("room2Income")) //中级
            {
                item.addRoomIncome(1, Convert.ToInt64(data["room2Income"]));
            }
            if (data.ContainsKey("room3Income")) //高级
            {
                item.addRoomIncome(2, Convert.ToInt64(data["room3Income"]));
            }
            if (data.ContainsKey("room4Income")) //VIP
            {
                item.addRoomIncome(3, Convert.ToInt64(data["room4Income"]));
            }
            if (data.ContainsKey("room15Income")) //至尊场
            {
                item.addRoomIncome(4, Convert.ToInt64(data["room15Income"]));
            }
            if (data.ContainsKey("room11Income")) //竞技
            {
                item.addRoomIncome(5, Convert.ToInt64(data["room11Income"]));
            }
            if (data.ContainsKey("room20Income"))  //东海龙宫场次
            {
                item.addRoomIncome(9, Convert.ToInt64(data["room20Income"]));
            }

            if (tableName == TableName.JEWEL_EVERY_DAY_STAT) //宝石迷阵
            {
                if (data.ContainsKey("TodayOutlay"))
                    item.addRoomOutlay(0, Convert.ToInt64(data["TodayOutlay"]));
            }
            else
            {
                if (data.ContainsKey("room1Outlay"))
                    item.addRoomOutlay(0, Convert.ToInt64(data["room1Outlay"]));
            }
            if (data.ContainsKey("room2Outlay"))
            {
                item.addRoomOutlay(1, Convert.ToInt64(data["room2Outlay"]));
            }
            if (data.ContainsKey("room3Outlay"))
            {
                item.addRoomOutlay(2, Convert.ToInt64(data["room3Outlay"]));
            }
            if (data.ContainsKey("room4Outlay"))
            {
                item.addRoomOutlay(3, Convert.ToInt64(data["room4Outlay"]));
            }
            if (data.ContainsKey("room15Outlay"))
            {
                item.addRoomOutlay(4, Convert.ToInt64(data["room15Outlay"]));
            }
            if (data.ContainsKey("room11Outlay"))
            {
                item.addRoomOutlay(5, Convert.ToInt64(data["room11Outlay"]));
            }
            if (data.ContainsKey("room20Outlay"))
            {
                item.addRoomOutlay(9, Convert.ToInt64(data["room20Outlay"]));
            }

            if (data.ContainsKey("room1PlayerPump"))
            {
                item.addRoomPlayerPump(0, Convert.ToInt64(data["room1PlayerPump"]));
            }
            if (data.ContainsKey("room2PlayerPump"))
            {
                item.addRoomPlayerPump(1, Convert.ToInt64(data["room2PlayerPump"]));
            }
            if (data.ContainsKey("room3PlayerPump"))
            {
                item.addRoomPlayerPump(2, Convert.ToInt64(data["room3PlayerPump"]));
            }
            if (data.ContainsKey("room4PlayerPump"))
            {
                item.addRoomPlayerPump(3, Convert.ToInt64(data["room4PlayerPump"]));
            }
            if (data.ContainsKey("room15PlayerPump"))
            {
                item.addRoomPlayerPump(4, Convert.ToInt64(data["room15PlayerPump"]));
            }
            if (data.ContainsKey("room11PlayerPump"))
            {
                item.addRoomPlayerPump(5, Convert.ToInt64(data["room11PlayerPump"]));
            }
            if (data.ContainsKey("room20PlayerPump"))
            {
                item.addRoomPlayerPump(9, Convert.ToInt64(data["room20PlayerPump"]));
            }

            // 废弹相关信息
            if (data.ContainsKey("room1Abandonedbullets"))
            {
                item.addRoomAbandonedbullets(0, Convert.ToInt64(data["room1Abandonedbullets"]));
            }
            if (data.ContainsKey("room2Abandonedbullets"))
            {
                item.addRoomAbandonedbullets(1, Convert.ToInt64(data["room2Abandonedbullets"]));
            }
            if (data.ContainsKey("room3Abandonedbullets"))
            {
                item.addRoomAbandonedbullets(2, Convert.ToInt64(data["room3Abandonedbullets"]));
            }
            if (data.ContainsKey("room4Abandonedbullets"))
            {
                item.addRoomAbandonedbullets(3, Convert.ToInt64(data["room4Abandonedbullets"]));
            }
            if (data.ContainsKey("room15Abandonedbullets"))
            {
                item.addRoomAbandonedbullets(4, Convert.ToInt64(data["room15Abandonedbullets"]));
            }
            if (data.ContainsKey("room11Abandonedbullets"))
            {
                item.addRoomAbandonedbullets(5, Convert.ToInt64(data["room11Abandonedbullets"]));
            }
            if (data.ContainsKey("room20Abandonedbullets"))
            {
                item.addRoomAbandonedbullets(9, Convert.ToInt64(data["room20Abandonedbullets"]));
            }
        }
        return OpRes.opres_success;
    }

    private OpRes queryTotal(GMUser user, IMongoQuery imq, string tableName, int gameId)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip, imq, 0, 0, null, "Date", false);
        if (dataList == null)
            return OpRes.op_res_failed;

        for (int i = 0; i < dataList.Count; i++)
        {
            EarningItem item = null;
            DateTime t = Convert.ToDateTime(dataList[i]["Date"]).ToLocalTime();
            item = new EarningItem();

            item.m_time = t.ToShortDateString();

            if (gameId == 10)
            {
                if (dataList[i].ContainsKey("room1Outlay"))
                {
                    item.addRoomOutlay(6, Convert.ToInt64(dataList[i]["room1Outlay"]));
                }
                if (dataList[i].ContainsKey("room1Income"))
                {
                    item.addRoomIncome(6, Convert.ToInt64(dataList[i]["room1Income"]));
                }
            }
            else if (gameId == 1)
            {
                long todayOutlay = 0;
                long todayIncome = 0;
                if (dataList[i].ContainsKey("room1Outlay"))
                {
                    todayOutlay += Convert.ToInt64(dataList[i]["room1Outlay"]);
                }
                if (dataList[i].ContainsKey("room2Outlay"))
                {
                    todayOutlay += Convert.ToInt64(dataList[i]["room2Outlay"]);
                }
                if (dataList[i].ContainsKey("room3Outlay"))
                {
                    todayOutlay += Convert.ToInt64(dataList[i]["room3Outlay"]);
                }
                if (dataList[i].ContainsKey("room4Outlay"))
                {
                    todayOutlay += Convert.ToInt64(dataList[i]["room4Outlay"]);
                }
                if (dataList[i].ContainsKey("room15Outlay"))
                {
                    todayOutlay += Convert.ToInt64(dataList[i]["room15Outlay"]);
                }
                if (dataList[i].ContainsKey("room11Outlay"))
                {
                    todayOutlay += Convert.ToInt64(dataList[i]["room11Outlay"]);
                }
                if (dataList[i].ContainsKey("room20Outlay"))
                {
                    todayOutlay += Convert.ToInt64(dataList[i]["room20Outlay"]);
                }
                item.addRoomOutlay(6, todayOutlay);

                if (dataList[i].ContainsKey("room1Income"))
                {
                    todayIncome += Convert.ToInt64(dataList[i]["room1Income"]);
                }
                if (dataList[i].ContainsKey("room2Income"))
                {
                    todayIncome += Convert.ToInt64(dataList[i]["room2Income"]);
                }
                if (dataList[i].ContainsKey("room3Income"))
                {
                    todayIncome += Convert.ToInt64(dataList[i]["room3Income"]);
                }
                if (dataList[i].ContainsKey("room4Income"))
                {
                    todayIncome += Convert.ToInt64(dataList[i]["room4Income"]);
                }
                if (dataList[i].ContainsKey("room15Income"))
                {
                    todayIncome += Convert.ToInt64(dataList[i]["room15Income"]);
                }
                if (dataList[i].ContainsKey("room11Income"))
                {
                    todayIncome += Convert.ToInt64(dataList[i]["room11Income"]);
                }
                if (dataList[i].ContainsKey("room20Income"))
                {
                    todayIncome += Convert.ToInt64(dataList[i]["room20Income"]);
                }
                item.addRoomIncome(6, todayIncome);
            }
            else
            {
                if (dataList[i].ContainsKey("TodayOutlay"))
                {
                    item.addRoomOutlay(6, Convert.ToInt64(dataList[i]["TodayOutlay"]));
                }
                if (dataList[i].ContainsKey("TodayIncome"))
                {
                    item.addRoomIncome(6, Convert.ToInt64(dataList[i]["TodayIncome"]));
                }
            }

            item.addRoomIncome(0, getActivePerson(t, gameId));
            m_totalResult.addData(t, gameId, item);
        }
        return OpRes.opres_success;
    }

    void statTotal(GMUser user, IMongoQuery imq, ParamServerEarnings p)
    {
        m_totalResult.reset();
        OpRes res = user.doStat(p.m_time, StatType.statTypeActivePerson);
        m_activePerson = (List<ResultActive>)user.getStatResult(StatType.statTypeActivePerson);

        queryTotal(user, imq, TableName.PUMP_FISHLORD_EVERY_DAY, (int)GameId.fishlord);
        queryTotal(user, imq, TableName.PUMP_CROCODILE_EVERY_DAY, (int)GameId.crocodile);
        queryTotal(user, imq, TableName.PUMP_COWS_EVERY_DAY, (int)GameId.cows);
        queryTotal(user, imq, TableName.PUMP_DRAGON_EVERY_DAY, (int)GameId.dragon);
        queryTotal(user, imq, TableName.PUMP_SHCD_EVERY_DAY, (int)GameId.shcd);
        queryTotal(user, imq, TableName.SHUIHZ_DAILY, (int)GameId.shuihz);
        queryTotal(user, imq, TableName.BZ_DAILY, (int)GameId.bz);
        queryTotal(user, imq, TableName.FRUIT_EVERY_DAY, (int)GameId.fruit);
        queryTotal(user, imq, TableName.JEWEL_EVERY_DAY_STAT, (int)GameId.jewel);
    }

    int getActivePerson(DateTime t, int gameId)
    {
        int res = 0;
        foreach (var item in m_activePerson)
        {
            if (item.m_time == t.ToShortDateString())
            {
                res = item.getCount(gameId);
                break;
            }
        }
        return res;
    }
}

//金币流动统计（新）
public class ServerEarningItem 
{
    public string m_time;
    public Dictionary<int, long[]> m_incomeOutlay = new Dictionary<int, long[]>(); //场次对应的 收入 支出 废弹
    public int m_flag = 0;
    public int m_activeCount;

    public string getEarnRate(long income, long outlay)
    {
        if (income == 0)
            return "0";
        double factGain = (double)(income - outlay) / (income);
        return Math.Round(factGain, 3).ToString();
    }
}
public class QueryServerEarningsNew : QueryBase 
{
    private List<ServerEarningItem> m_result = new List<ServerEarningItem>();

    public ServerEarningItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag = 1;
                return d;
            }
        }

        ServerEarningItem item = new ServerEarningItem();
        m_result.Add(item);
        item.m_time = time;

        item.m_incomeOutlay.Add(1, new long[3]);
        item.m_incomeOutlay.Add(2, new long[3]);
        item.m_incomeOutlay.Add(3, new long[3]);
        item.m_incomeOutlay.Add(4, new long[3]);
        item.m_incomeOutlay.Add(5, new long[3]);
        item.m_incomeOutlay.Add(6, new long[3]);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //时间选择
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("Date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("Date", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("Date").Ascending("ROOMID");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.PUMP_FISHLORD_EVERY_DAY, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;


        int accServer = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        ServerEarningItem item = new ServerEarningItem();
        m_result.Add(item);
        item.m_time = "总计";
        item.m_incomeOutlay.Add(1, new long[3]);
        item.m_incomeOutlay.Add(2, new long[3]);
        item.m_incomeOutlay.Add(3, new long[3]);
        item.m_incomeOutlay.Add(4, new long[3]);
        item.m_incomeOutlay.Add(5, new long[3]);
        item.m_incomeOutlay.Add(6, new long[3]);

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime t = Convert.ToDateTime(data["Date"]).ToLocalTime();
            string time = t.ToShortDateString();
            ServerEarningItem tmp = IsCreate(time);

            if (data.ContainsKey("ROOMID")) {
                int roomId = Convert.ToInt32(data["ROOMID"]);

                if (tmp.m_incomeOutlay.ContainsKey(roomId)) {

                    if (data.ContainsKey("DailyIncome"))
                        tmp.m_incomeOutlay[roomId][0] = Convert.ToInt64(data["DailyIncome"]);

                    if (data.ContainsKey("DailyOutlay"))
                        tmp.m_incomeOutlay[roomId][1] = Convert.ToInt64(data["DailyOutlay"]);

                    if (data.ContainsKey("Abandonedbullets"))
                        tmp.m_incomeOutlay[roomId][2] = Convert.ToInt64(data["Abandonedbullets"]);

                    item.m_incomeOutlay[roomId][0] += tmp.m_incomeOutlay[roomId][0];
                    item.m_incomeOutlay[roomId][1] += tmp.m_incomeOutlay[roomId][1];
                    item.m_incomeOutlay[roomId][2] += tmp.m_incomeOutlay[roomId][2];
                }
            }

            if (tmp.m_flag == 0) 
            {
                IMongoQuery imq1 = Query.EQ("genTime", t.Date);

                List<Dictionary<string, object>> activeCountList = DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, accServer, DbName.DB_ACCOUNT, imq1, 0, 0, new string[] { "activeCount" });

                if (activeCountList != null && activeCountList.Count != 0)
                {
                    foreach (var d in activeCountList)
                    {
                        if (d.ContainsKey("activeCount"))
                            tmp.m_activeCount += Convert.ToInt32(d["activeCount"]);
                    }
                }

                item.m_activeCount += tmp.m_activeCount;
            }
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////

public class ResultFishLord
{
    // 进入每个房间的次数
    public long[] m_enterRoomCount = new long[21];

    public void reset()
    {
        for (int i = 1; i < m_enterRoomCount.Length; i++)
        {
            m_enterRoomCount[i] = 0;
        }
    }

    public void addCount(int roomId, long count)
    {
        if (roomId >= m_enterRoomCount.Length)
            return;

        m_enterRoomCount[roomId] = count;
    }

    public int getRoomCount()
    {
        return m_enterRoomCount.Length;
    }

    public string getEnterRoomCount(int roomId)
    {
        return m_enterRoomCount[roomId].ToString();
    }

    public string getRoomName(int roomId)
    {
        return StrName.s_roomList[roomId];
    }
}

// 捕鱼独立数据
public class QueryIndependentFishlord : QueryBase
{
    private ResultFishLord m_result = new ResultFishLord();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.FISHLORD_ROOM, dip);
        for (int i = 0; i < dataList.Count; i++)
        {
            int roomId = Convert.ToInt32(dataList[i]["room_id"]);
            long count = 0;
            if (dataList[i].ContainsKey("EnterCount"))
            {
                count = Convert.ToInt64(dataList[i]["EnterCount"]);
            }
            m_result.addCount(roomId, count);
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class CrocodileInfo
{
    // 下注次数
    public long m_betCount;
    // 获奖次数
    public long m_winCount;
    // 总收入
    public long m_income;
    // 总支出
    public long m_outlay;

    // 返回实际盈利率
    public string getFactExpRate()
    {
        if (m_income == 0 && m_outlay == 0)
            return "0";
        if (m_income == 0)
            return "-∞";

        double factGain = (double)(m_income - m_outlay) / m_income;
        return Math.Round(factGain, 3).ToString();
    } 
}

public class ResultIndependent : ResultFishLord
{
    // 每个区域的信息
    public Dictionary<int, CrocodileInfo> m_betInfo = new Dictionary<int, CrocodileInfo>();

    public new void reset()
    {
        base.reset();
        m_betInfo.Clear();
    }

    public void addBetCount(int betId, long betCount, long winCount, long income, long outlay)
    {
        CrocodileInfo info = null;
        if (m_betInfo.ContainsKey(betId))
        {
            info = m_betInfo[betId];
        }
        else
        {
            info = new CrocodileInfo();
            m_betInfo.Add(betId, info);
        }
        info.m_betCount += betCount;
        info.m_winCount += winCount;
        info.m_income += income;
        info.m_outlay += outlay;
    }
}

public class ResultCrocodile : ResultIndependent
{
    public string getAreaName(int areaId)
    {
        Crocodile_RateCFGData data = Crocodile_RateCFG.getInstance().getValue(areaId);
        if (data != null)
            return data.m_name;
        return "";
    }
}

// 鳄鱼独立数据
public class QueryIndependentCrocodile : QueryBase
{
    private ResultCrocodile m_result = new ResultCrocodile();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.CROCODILE_ROOM, dip);
        for (int i = 0; i < dataList.Count; i++)
        {
            int roomId = Convert.ToInt32(dataList[i]["room_id"]);
            long count = 0;
            if (dataList[i].ContainsKey("enter_count"))
            {
                count = Convert.ToInt64(dataList[i]["enter_count"]);
            }
            m_result.addCount(roomId, count);
        }

        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_CROCODILE_BET, dip_1);
        for (int i = 0; i < dataList.Count; i++)
        {
            int betId = Convert.ToInt32(dataList[i]["BetID"]);
            long count1 = Convert.ToInt64(dataList[i]["BetCount"]);
            long count2 = Convert.ToInt64(dataList[i]["BetWin"]);

            long income = 0, outlay = 0;
            if (dataList[i].ContainsKey("BetInCome"))
            {
                income = Convert.ToInt64(dataList[i]["BetInCome"]);
            }
            if (dataList[i].ContainsKey("BetOutCome"))
            {
                outlay = Convert.ToInt64(dataList[i]["BetOutCome"]);
            }
            m_result.addBetCount(betId, count1, count2, income, outlay);
        }

        return OpRes.opres_success;
    }
}

//水果机独立数据
public class ResultFruit : ResultIndependent
{
    public string getAreaName(int areaId)
    {
        Crocodile_RateCFGData data = Fruit_RateCFG.getInstance().getValue(areaId);
        if (data != null)
            return data.m_name;
        return "";
    }
}
public class QueryIndependentFruit : QueryBase
{
    private ResultFruit m_result = new ResultFruit();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.FRUIT_ROOM, dip);
        for (int i = 0; i < dataList.Count; i++)
        {
            int roomId = Convert.ToInt32(dataList[i]["room_id"]);
            long count = 0;
            if (dataList[i].ContainsKey("enter_count"))
            {
                count = Convert.ToInt64(dataList[i]["enter_count"]);
            }
            m_result.addCount(roomId, count);
        }

        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_FRUIT_BET, dip_1);
        for (int i = 0; i < dataList.Count; i++)
        {
            int betId = Convert.ToInt32(dataList[i]["BetID"]);
            long count1 = Convert.ToInt64(dataList[i]["BetCount"]);
            long count2 = Convert.ToInt64(dataList[i]["BetWin"]);

            long income = 0, outlay = 0;
            if (dataList[i].ContainsKey("BetInCome"))
            {
                income = Convert.ToInt64(dataList[i]["BetInCome"]);
            }
            if (dataList[i].ContainsKey("BetOutCome"))
            {
                outlay = Convert.ToInt64(dataList[i]["BetOutCome"]);
            }
            m_result.addBetCount(betId, count1, count2, income, outlay);
        }

        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//奔驰宝马独立数据
public class ResultBz : ResultIndependent
{
    public string getAreaName(int areaId)
    {
        string m_name=StrName.s_bzArea[areaId];
        return m_name;
    }
}

public class QueryIndependentBz : QueryBase
{
    private ResultBz m_result = new ResultBz();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.DB_BZ_ROOM, dip);
        for (int i = 0; i < dataList.Count; i++)
        {
            int roomId = Convert.ToInt32(dataList[i]["room_id"]);
            long count = 0;
            if (dataList[i].ContainsKey("enter_count"))
            {
                count = Convert.ToInt64(dataList[i]["enter_count"]);
            }
            m_result.addCount(roomId, count);
        }

        DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_BZ_BET, dip_1);
        for (int i = 0; i < dataList.Count; i++)
        {
            int betId = Convert.ToInt32(dataList[i]["BetID"]);
            long count1 = Convert.ToInt64(dataList[i]["BetCount"]);
            long count2 = Convert.ToInt64(dataList[i]["BetWin"]);

            long income = 0, outlay = 0;
            if (dataList[i].ContainsKey("BetInCome"))
            {
                income = Convert.ToInt64(dataList[i]["BetInCome"]);
            }
            if (dataList[i].ContainsKey("BetOutCome"))
            {
                outlay = Convert.ToInt64(dataList[i]["BetOutCome"]);
            }
            m_result.addBetCount(betId, count1, count2, income, outlay);
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ResultDice : ResultIndependent
{
    public string getAreaName(int areaId)
    {
        Dice_BetAreaCFGData data = Dice_BetAreaCFG.getInstance().getValue(areaId);
        if (data != null)
            return data.m_name;
        return "";
    }
}

// 骰宝独立数据
public class QueryIndependentDice : QueryBase
{
    private ResultDice m_result = new ResultDice();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.DICE_ROOM, user.getDbServerID(), DbName.DB_GAME);
        for (int i = 0; i < dataList.Count; i++)
        {
            int roomId = Convert.ToInt32(dataList[i]["room_id"]);
            long count = 0;
            if (dataList[i].ContainsKey("enter_count"))
            {
                count = Convert.ToInt64(dataList[i]["enter_count"]);
            }
            m_result.addCount(roomId, count);
        }

        dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_DICE, user.getDbServerID(), DbName.DB_PUMP);
        for (int i = 0; i < dataList.Count; i++)
        {
            int betId = Convert.ToInt32(dataList[i]["bet_id"]);
            long count1 = Convert.ToInt64(dataList[i]["bet_count"]);
            long count2 = Convert.ToInt64(dataList[i]["win_count"]);
            long income = 0, outlay = 0;
            if (dataList[i].ContainsKey("Income"))
            {
                income = Convert.ToInt64(dataList[i]["Income"]);
            }
            if (dataList[i].ContainsKey("Outlay"))
            {
                outlay = Convert.ToInt64(dataList[i]["Outlay"]);
            }
            m_result.addBetCount(betId, count1, count2, income, outlay);
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ResultCows : ResultIndependent
{
    public string getAreaName(int areaId)
    {
        return StrName.s_cowsArea[areaId - 1];
    }

    // 返回盈利率
    public string getRate(long income, long outlay)
    {
        if (outlay == 0)
            return "1";

        double factGain = (double)income / outlay;
        return Math.Round(factGain, 3).ToString();
    } 
}

// 牛牛独立数据--各区域的下注，获奖情况
public class QueryIndependentCows : QueryBase
{
    private ResultCows m_result = new ResultCows();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.COWS_ROOM, 
            user.getDbServerID(), DbName.DB_GAME);

        for (int i = 0; i < dataList.Count; i++)
        {
            for (int k = 1; k <= 4; k++)
            {
                // 总收入
                string totalWin = string.Format("WinGold{0}", k);
                // 总支出
                string totalBetGold = string.Format("LoseGold{0}", k);
                // 盈的总次数
                string totalWinCount = string.Format("WinCount{0}", k);

                long count2 = Convert.ToInt64(dataList[i][totalWinCount]);
                long income = 0, outlay = 0;
                if (dataList[i].ContainsKey(totalWin))
                {
                    income = Convert.ToInt64(dataList[i][totalWin]);
                }
                if (dataList[i].ContainsKey(totalBetGold))
                {
                    outlay = Convert.ToInt64(dataList[i][totalBetGold]);
                }
                m_result.addBetCount(k, 0, count2, income, outlay);
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class QueryEarningsParam : QueryBase
{
    protected Dictionary<int, ResultFishlordExpRate> m_result = new Dictionary<int, ResultFishlordExpRate>();

    public override object getQueryResult()
    {
        return m_result;
    }

    protected OpRes query(GMUser user, string tableName)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName,
            user.getDbServerID(), DbName.DB_GAME);
        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultFishlordExpRate info = new ResultFishlordExpRate();
            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if (dataList[i].ContainsKey("room_income"))
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["room_income"]);
            }
            if (dataList[i].ContainsKey("room_outcome"))
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["room_outcome"]);
            }
            if (dataList[i].ContainsKey("player_charge"))
            {
                info.m_playerCharge = Convert.ToInt64(dataList[i]["player_charge"]);
            }

            if (dataList[i].ContainsKey("rob_income"))
            {
                info.m_robotIncome = Convert.ToInt64(dataList[i]["rob_income"]);
            }
            if (dataList[i].ContainsKey("rob_outcome"))
            {
                info.m_robotOutlay = Convert.ToInt64(dataList[i]["rob_outcome"]);
            }
            m_result.Add(info.m_roomId, info);
        }

        return OpRes.opres_success;
    }
}

// 骰宝盈利参数查询
public class QueryDiceEarningsParam : QueryEarningsParam
{
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user, TableName.DICE_ROOM);
    }
}

// 百家乐盈利参数查询
public class QueryBaccaratEarningsParam : QueryEarningsParam
{
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user, TableName.BACCARAT_ROOM);
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultNoticeInfo
{
    public string m_id = "";
    public string m_genTime = "";
    public string m_startTime = "";
    public string m_deadTime = "";
    public string m_title = "";
    public string m_content = "";
    public string m_comment = "";

    // 排序字段
    public int m_order;
}
// 当前公告信息
public class QueryCurNotice : QueryBase
{
    private List<ResultNoticeInfo> m_result = new List<ResultNoticeInfo>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.OPERATION_NOTIFY, 
            user.getDbServerID(), DbName.DB_PLAYER);

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultNoticeInfo info = new ResultNoticeInfo();
            m_result.Add(info);

            info.m_id = Convert.ToString(dataList[i]["noticeId"]);
            info.m_genTime = Convert.ToDateTime(dataList[i]["genTime"]).ToLocalTime().ToString();
            info.m_startTime = Convert.ToDateTime(dataList[i]["startTime"]).ToLocalTime().ToString();
            info.m_deadTime = Convert.ToDateTime(dataList[i]["deadTime"]).ToLocalTime().ToString();
            info.m_title = Convert.ToString(dataList[i]["title"]);
            info.m_content = Convert.ToString(dataList[i]["content"]);
            if (dataList[i].ContainsKey("comment"))
            {
                info.m_comment = Convert.ToString(dataList[i]["comment"]);
            }
            if (dataList[i].ContainsKey("order"))
            {
                info.m_order = Convert.ToInt32(dataList[i]["order"]);
            }
        }

        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//运营公告编辑
public class QueryNoticeInfo : QueryBase 
{
    private ResultNoticeInfo m_result = new ResultNoticeInfo();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result = new ResultNoticeInfo();
        ParamNotify p = (ParamNotify)param;
        string m_id = p.m_id;
        IMongoQuery imq = Query.EQ("noticeId",m_id);
        return query(user,imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user,IMongoQuery imq)
    {
        DbInfoParam dip=ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> dataList =
            DBMgr.getInstance().getTableData(TableName.OPERATION_NOTIFY, dip, imq);
        if (dataList == null)
        {
            return OpRes.op_res_not_found_data;
        }else
        {
            ResultNoticeInfo info = new ResultNoticeInfo();
            m_result = info;
            info.m_id = Convert.ToString(dataList["noticeId"]);

            info.m_genTime = Convert.ToDateTime(dataList["genTime"]).ToLocalTime().ToString();
            info.m_startTime = Convert.ToDateTime(dataList["startTime"]).ToLocalTime().ToString("yyyy/MM/dd HH:mm");
            info.m_deadTime = Convert.ToDateTime(dataList["deadTime"]).ToLocalTime().ToString("yyyy/MM/dd HH:mm");
            info.m_title = Convert.ToString(dataList["title"]);
            info.m_content = Convert.ToString(dataList["content"]);
            if (dataList.ContainsKey("comment"))
            {
                info.m_comment = Convert.ToString(dataList["comment"]);
            }
            if (dataList.ContainsKey("order"))
            {
                info.m_order = Convert.ToInt32(dataList["order"]);
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////

public class ResultFishlordExpRate
{
    public int m_roomId;
    public double m_expRate;
    public long m_totalIncome;
    public long m_totalOutlay;

    public long m_totalPlayerPump;

    public long m_appearAdvertisementLessBet=0;//广告版下注阀值
    public int m_bigParadiseGiveScoreProb = 10000;//大天堂放分概率
    public int m_smallParadiseGiveScoreProb = 10000;

    public int m_bigHellKillScoreProb = 10000;//大地狱杀分概率
    public int m_smallHellKillScoreProb = 10000;//小地域杀分概率

    // 手续费
    public long m_playerCharge = 0;

    // 玩家个数
    public int m_curPlayerCount = 0;

    // 机器人收入
    public long m_robotIncome = 0;
    // 机器人支出
    public long m_robotOutlay = 0;

    // 废弹(经典捕鱼，鳄鱼公园)
    public long m_abandonedbullets;

    // 捕鱼的导弹产出
    public long m_missileCount;

    // 返回实际盈利率
    public string getFactExpRate()
    {
        long totalIncome = getIncome();
        if (totalIncome == 0 && m_totalOutlay == 0)
            return "0";
        if (totalIncome == 0)
            return "-∞";

        double factGain = (double)(totalIncome + m_totalPlayerPump - m_totalOutlay) / (totalIncome + m_totalPlayerPump);
        return Math.Round(factGain, 3).ToString();
    }

    // 手续费计入总收入内
    public long getDelta()
    {
        return getIncome() - m_totalOutlay + m_totalPlayerPump;
    }

    public long getIncome()
    {
        return m_totalIncome + m_playerCharge;
    }

    // 返回盈利率
    public string getRate(long income, long outlay)
    {
        if (outlay == 0)
            return "1";

        double factGain = (double)income / outlay;
        return Math.Round(factGain, 3).ToString();
    }
}

public class ParamFishlordBoss
{
    public string m_midTime;
    public string m_highTime;
}

public class ResultFishlordNewExpRate : ResultFishlordExpRate
{
    public int m_minEarnValue;
    public int m_maxEarnValue;
    public int m_minControlEarnValue;
    public int m_maxControlEarnValue;
    public int m_startEarnValue;
    public int m_expEarn;
    public long m_totalHitLucky;
    public long m_totalHitUnlucky;
    public long m_totalChargeOutlay;
    public long m_totalNewPlayerOutlay;
    // 奖金池
    public long m_pricePool;
}

// 捕鱼参数查询
public class QueryFishlordParam : QueryBase
{
    protected Dictionary<int, ResultFishlordNewExpRate> m_result = new Dictionary<int, ResultFishlordNewExpRate>();
    protected List<ResultFishlordExpRate> m_result1 = new List<ResultFishlordExpRate>();

    public override OpRes doQuery(object param, GMUser user)
    {
        if (param == null)
        {
            m_result.Clear();
            return query(user, TableName.FISHLORD_ROOM);
        }

        ParamFishlordBoss p = (ParamFishlordBoss)param;
        m_result1.Clear();
        return queryBoss(user, p);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user) { return m_result1; }

    protected OpRes query(GMUser user, string tableName)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName,dip);
        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultFishlordNewExpRate info = new ResultFishlordNewExpRate();
            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if (dataList[i].ContainsKey("EarningsRate"))
            {
                info.m_expRate = Convert.ToDouble(dataList[i]["EarningsRate"]);
            }
            else
            {
                info.m_expRate = 0.05;
            }
            if (dataList[i].ContainsKey("TotalIncome"))
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["TotalIncome"]);
            }
            if (dataList[i].ContainsKey("TotalOutlay"))
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["TotalOutlay"]);
            }
            if (dataList[i].ContainsKey("player_count"))
            {
                info.m_curPlayerCount = Convert.ToInt32(dataList[i]["player_count"]);
            }
            if (dataList[i].ContainsKey("Abandonedbullets"))
            {
                info.m_abandonedbullets = Convert.ToInt64(dataList[i]["Abandonedbullets"]);
            }
            if (dataList[i].ContainsKey("MissileCount"))
            {
                info.m_missileCount = Convert.ToInt64(dataList[i]["MissileCount"]);
            }

            if (dataList[i].ContainsKey("WinRateAverage"))
            {
                info.m_expEarn = Convert.ToInt32(dataList[i]["WinRateAverage"]);
            }
            if (dataList[i].ContainsKey("WinRateMax"))
            {
                info.m_maxEarnValue = Convert.ToInt32(dataList[i]["WinRateMax"]);
            }
            if (dataList[i].ContainsKey("WinRateMin"))
            {
                info.m_minEarnValue = Convert.ToInt32(dataList[i]["WinRateMin"]);
            }
            if (dataList[i].ContainsKey("WinRateCtrValue"))
            {
                info.m_startEarnValue = Convert.ToInt32(dataList[i]["WinRateCtrValue"]);
            }
            if(dataList[i].ContainsKey("WinRateControlMin"))
            {
                info.m_minControlEarnValue = Convert.ToInt32(dataList[i]["WinRateControlMin"]);
            }
            if(dataList[i].ContainsKey("WinRateControlMax"))
            {
                info.m_maxControlEarnValue = Convert.ToInt32(dataList[i]["WinRateControlMax"]);
            }
            if (dataList[i].ContainsKey("PricePool"))
            {
                info.m_pricePool = Convert.ToInt64(dataList[i]["PricePool"]);
            }
            if (dataList[i].ContainsKey("TotalHitLucky"))
            {
                info.m_totalHitLucky = Convert.ToInt64(dataList[i]["TotalHitLucky"]);
            }
            if (dataList[i].ContainsKey("TotalHitUnlucky"))
            {
                info.m_totalHitUnlucky = Convert.ToInt64(dataList[i]["TotalHitUnlucky"]);
            }
            if(dataList[i].ContainsKey("TotalChargeOutlay"))
            {
                info.m_totalChargeOutlay = Convert.ToInt64(dataList[i]["TotalChargeOutlay"]);
            }
            if(dataList[i].ContainsKey("TotalNewPlayerOutlay"))
            {
                info.m_totalNewPlayerOutlay = Convert.ToInt64(dataList[i]["TotalNewPlayerOutlay"]);
            }
            m_result.Add(info.m_roomId, info);
        }

        return OpRes.opres_success;
    }

    OpRes queryBoss(GMUser user, ParamFishlordBoss param)
    {
        queryBoss(user, param.m_midTime, 2);
        queryBoss(user, param.m_highTime, 3);
        return OpRes.opres_success;
    }

    void queryBoss(GMUser user, string time, int roomId)
    {
        if (string.IsNullOrEmpty(time))
            return;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(time, ref mint, ref maxt);
        if (!res)
            return;

        DateTime curT = DateTime.Now.Date.AddDays(1);
        IMongoQuery imq1 = Query.LT("date", BsonValue.Create(curT));
        IMongoQuery imq2 = Query.GTE("date", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("roomid", roomId));

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_BOSSINFO, dip, imq, 0, 0, null, "date", false);
        if (dataList == null)
            return;

        ResultFishlordExpRate info = new ResultFishlordExpRate();
        info.m_roomId = roomId;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            info.m_robotIncome += Convert.ToInt64(data["consume_gold"]);

            if (data.ContainsKey("bossReleaseGold"))
            {
                info.m_robotOutlay += Convert.ToInt64(data["bossReleaseGold"]);
            }

            info.m_robotOutlay += Convert.ToInt64(data["dargonball"]) * 5000;
        }

        ResultFishlordExpRate f = m_result[roomId];
        info.m_totalIncome = f.m_totalIncome - info.m_robotIncome;
        info.m_totalOutlay = f.m_totalOutlay - info.m_robotOutlay;

        m_result1.Add(info);
    }
}

//大水池参数查询
public class FishlordNewParamItem
{
    public int m_roomId;
    public long m_totalIncome;
    public long m_totalOutlay;
    public long m_rechargePool;
    public long m_newPlayerPool;

    public int m_playerCount;
    public long m_trickOutLay;
    public long m_trickIncome;
    public double m_deviationFix;
    public long m_rewardOutlay;
    public long m_punishIncome;

    public long m_abandonedbullets;

    public double m_jsckpotGrandPump;
    public double m_jsckpotSmallPump;
    public double m_normalFishRoomPoolPumpParam;

    public int m_percentCtrl5;
    public int m_percentCtrl20;
    public int m_percentCtrl60;

    public double m_baseRate;
    public double m_checkRate;
    public double m_trickDeviationFix;

    public long m_incomeThreshold;
    public double m_earnRatemCtrMax;
    public double m_earnRatemCtrMin;

    public int m_flag = 0;

    public string getRate(long income, long outlay)
    {
        if (income == 0)
            return "1";

        double factGain = (double)outlay / income;
        return Math.Round(factGain, 3).ToString();
    }

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "roomId";
        uParam.m_value = m_roomId.ToString();
        uParam.m_url = DefCC.ASPX_STAT_FISHLORD_CTRL_NEW_DETAIL;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryFishlordNewParam : QueryBase 
{
    protected Dictionary<int, FishlordNewParamItem> m_result = new Dictionary<int, FishlordNewParamItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user, TableName.FISHLORD_ROOM);
    }
    public override object getQueryResult()
    {
        return m_result;
    }

    protected OpRes query(GMUser user, string tableName)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip);
        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string,object> data = dataList[i];

            FishlordNewParamItem info = new FishlordNewParamItem();
            info.m_roomId = Convert.ToInt32(data["room_id"]);

            if (data.ContainsKey("BigPoolIncome"))
                info.m_totalIncome = Convert.ToInt64(data["BigPoolIncome"]);

            if (data.ContainsKey("BigPoolOutLay"))
                info.m_totalOutlay = Convert.ToInt64(data["BigPoolOutLay"]);

            if (data.ContainsKey("player_count"))
                info.m_playerCount = Convert.ToInt32(data["player_count"]);

            if (data.ContainsKey("TrickIncome"))
                info.m_trickIncome = Convert.ToInt64(data["TrickIncome"]);

            if (data.ContainsKey("TrickOutLay"))
                info.m_trickOutLay = Convert.ToInt64(data["TrickOutLay"]);

            if (data.ContainsKey("DeviationFix"))
                info.m_deviationFix = Convert.ToDouble(data["DeviationFix"]);

            if (data.ContainsKey("RewardOutLay"))
                info.m_rewardOutlay = Convert.ToInt64(data["RewardOutLay"]);

            if (data.ContainsKey("PunishIncome"))
                info.m_punishIncome = Convert.ToInt64(data["PunishIncome"]);

            if (data.ContainsKey("Abandonedbullets"))
                info.m_abandonedbullets = Convert.ToInt64(data["Abandonedbullets"]);

            if (data.ContainsKey("JsckpotGrandPump"))
                info.m_jsckpotGrandPump = Convert.ToDouble(data["JsckpotGrandPump"])*100;

            if (data.ContainsKey("JsckpotSmallPump"))
                info.m_jsckpotSmallPump = Convert.ToDouble(data["JsckpotSmallPump"])*100;

            if (data.ContainsKey("NormalFishRoomPoolPumpParam"))
                info.m_normalFishRoomPoolPumpParam = Convert.ToDouble(data["NormalFishRoomPoolPumpParam"])*100;

            if (data.ContainsKey("PercentControl5"))
                info.m_percentCtrl5 = Convert.ToInt32(data["PercentControl5"]);

            if (data.ContainsKey("PercentControl20"))
                info.m_percentCtrl20 = Convert.ToInt32(data["PercentControl20"]);

            if (data.ContainsKey("PercentControl60"))
                info.m_percentCtrl60 = Convert.ToInt32(data["PercentControl60"]);

            if (data.ContainsKey("BaseRate"))
                info.m_baseRate = Convert.ToDouble(data["BaseRate"]);

            if (data.ContainsKey("CheckRate"))
                info.m_checkRate = Convert.ToDouble(data["CheckRate"]);

            if (data.ContainsKey("TrickDeviationFix"))
                info.m_trickDeviationFix = Convert.ToDouble(data["TrickDeviationFix"]);

            //码量
            if (data.ContainsKey("IncomeThreshold"))
                info.m_incomeThreshold = Convert.ToInt64(data["IncomeThreshold"]);

            if (data.ContainsKey("EarnRatemCtrMax"))
                info.m_earnRatemCtrMax = Convert.ToDouble(data["EarnRatemCtrMax"]);

            if (data.ContainsKey("EarnRatemCtrMin"))
                info.m_earnRatemCtrMin = Convert.ToDouble(data["EarnRatemCtrMin"]);
            ///////////////////////////
            //充值
            long totalChargeIncome = 0, totalChargeOutlay = 0;
            if (data.ContainsKey("TotalChargeIncome"))
                totalChargeIncome = Convert.ToInt64(data["TotalChargeIncome"]);

            if (data.ContainsKey("TotalChargeOutlay"))
                totalChargeOutlay = Convert.ToInt64(data["TotalChargeOutlay"]);

            info.m_rechargePool = totalChargeIncome - totalChargeOutlay;
            ///////////////////////////
            //新手
            long totalNewPlayerIncome = 0, totalNewPlayerOutlay = 0;
            if (data.ContainsKey("TotalNewPlayerIncome"))
                totalNewPlayerIncome = Convert.ToInt64(data["TotalNewPlayerIncome"]);

            if (data.ContainsKey("TotalNewPlayerOutlay")) 
               totalNewPlayerOutlay = Convert.ToInt64(data["TotalNewPlayerOutlay"]);

            info.m_newPlayerPool = totalNewPlayerIncome - totalNewPlayerOutlay;
            ///////////////////////
            m_result.Add(info.m_roomId, info);
        }

        return OpRes.opres_success;
    }
}

//大水池场次参数查询
public class FishlordRoomItemNew 
{
    public string m_time;
    public int m_roomId;
    public long m_dailyIncome;
    public long m_dailyOutlay;

    public long m_rechargeReward;
    public long m_newReward;

    public long m_dailyTrickIncome;
    public long m_dailyTrickOutlay;
    public long m_abandonedbullets;

    public string getRate(long income, long outlay)
    {
        if (outlay == 0)
            return "1";

        double factGain = (double)income / outlay;
        return Math.Round(factGain, 3).ToString();
    }
}
public class QueryFishlordRoomNewParam : QueryBase 
{
    protected List<FishlordRoomItemNew> m_result = new List<FishlordRoomItemNew>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("Date", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("Date", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }
        else {
            imq = Query.EQ("Date", DateTime.Now.Date);
        }

        imq = Query.And(imq, Query.EQ("ROOMID", p.m_op));
        return query(user, imq, p);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    protected OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.FISHLORD_EVERY_DAY, imq, dip);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.FISHLORD_EVERY_DAY, dip, imq, 
            (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "Date", false);

        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            FishlordRoomItemNew info = new FishlordRoomItemNew();
            m_result.Add(info);
            info.m_roomId = Convert.ToInt32(data["ROOMID"]);
            info.m_time = Convert.ToDateTime(data["Date"]).ToLocalTime().ToShortDateString();

            //玩法收入 支出
            if (data.ContainsKey("DailyTrickIncome"))
                info.m_dailyTrickIncome = Convert.ToInt64(data["DailyTrickIncome"]);

            if (data.ContainsKey("DailyTrickOutlay"))
                info.m_dailyTrickOutlay = Convert.ToInt64(data["DailyTrickOutlay"]);

            if (data.ContainsKey("Abandonedbullets"))
                info.m_abandonedbullets = Convert.ToInt64(data["Abandonedbullets"]);

            //每日大水池收入
            if (data.ContainsKey("DailyBigPoolIncome"))
                info.m_dailyIncome = Convert.ToInt64(data["DailyBigPoolIncome"]);

            if (data.ContainsKey("DailyBigPoolOutlay"))
                info.m_dailyOutlay = Convert.ToInt64(data["DailyBigPoolOutlay"]);

            //充值奖励  
            long dailyChargeIncome = 0, dailyChargeOutlay = 0, dailyNewPlayerIncome = 0, dailyNewPlayerOutlay = 0;
            if(data.ContainsKey("DailyChargeIncome"))
                dailyChargeIncome = Convert.ToInt64(data["DailyChargeIncome"]);

            if (data.ContainsKey("DailyChargeOutlay"))
                dailyChargeOutlay = Convert.ToInt64(data["DailyChargeOutlay"]);

            info.m_rechargeReward = dailyChargeIncome - dailyChargeOutlay;

            //新用户奖励
            if (data.ContainsKey("DailyNewPlayerIncome"))
                dailyNewPlayerIncome = Convert.ToInt64(data["DailyNewPlayerIncome"]);

            if (data.ContainsKey("DailyNewPlayerOutlay"))
                dailyNewPlayerOutlay = Convert.ToInt64(data["DailyNewPlayerOutlay"]);

            info.m_newReward = dailyNewPlayerIncome - dailyNewPlayerOutlay;
        }

        return OpRes.opres_success;
    }
}

//个人后台管理
public class FishlordNewSingleItem 
{
    public long m_100TurretPlayerIncome;
    public long m_100TurretPlayerOutlay;
    public long m_100TurretPlayerCount;
    public long m_100TurretNoValueCount;
    public long m_100TurretRewardOutlay;
    public long m_100TurretPunishIncome;

    public long m_1000TurretPlayerIncome;
    public long m_1000TurretPlayerOutlay;
    public long m_1000TurretPlayerCount;
    public long m_1000TurretNoValueCount;
    public long m_1000TurretRewardOutlay;
    public long m_1000TurretPunishIncome;

    public long m_5000TurretPlayerIncome;
    public long m_5000TurretPlayerOutlay;
    public long m_5000TurretPlayerCount;
    public long m_5000TurretNoValueCount;
    public long m_5000TurretRewardOutlay;
    public long m_5000TurretPunishIncome;

    public long m_10000TurretPlayerIncome;
    public long m_10000TurretPlayerOutlay;
    public long m_10000TurretPlayerCount;
    public long m_10000TurretNoValueCount;
    public long m_10000TurretRewardOutlay;
    public long m_10000TurretPunishIncome;

    public double m_baseRate;
    public double m_deviationFix;
    public double m_noValuePlayerRate;

    public string getRate(long income, long outlay)
    {
        if (outlay == 0)
            return "1";

        double factGain = (double)income / outlay;
        return Math.Round(factGain, 3).ToString();
    }
}
public class QueryFishlordNewSingleParam : QueryBase 
{
    protected List<FishlordNewSingleItem> m_result = new List<FishlordNewSingleItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user, TableName.FISHLORD_LOBBY);
    }
    public override object getQueryResult()
    {
        return m_result;
    }

    protected OpRes query(GMUser user, string tableName)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip);
        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            FishlordNewSingleItem info = new FishlordNewSingleItem();
            m_result.Add(info);

            if (data.ContainsKey("100TurretPlayerIncome"))
                info.m_100TurretPlayerIncome = Convert.ToInt64(data["100TurretPlayerIncome"]);

            if (data.ContainsKey("100TurretPlayerOutlay"))
                info.m_100TurretPlayerOutlay = Convert.ToInt64(data["100TurretPlayerOutlay"]);

            if (data.ContainsKey("100TurretPlayerCount"))
                info.m_100TurretPlayerCount = Convert.ToInt64(data["100TurretPlayerCount"]);

            if (data.ContainsKey("100TurretNoValueCount"))
                info.m_100TurretNoValueCount = Convert.ToInt64(data["100TurretNoValueCount"]);

            if (data.ContainsKey("100TurretRewardOutlay"))
                info.m_100TurretRewardOutlay = Convert.ToInt64(data["100TurretRewardOutlay"]);

            if (data.ContainsKey("100TurretPunishIncome"))
                info.m_100TurretPunishIncome = Convert.ToInt64(data["100TurretPunishIncome"]);

            ////
            if (data.ContainsKey("1000TurretPlayerIncome"))
                info.m_1000TurretPlayerIncome = Convert.ToInt64(data["1000TurretPlayerIncome"]);

            if (data.ContainsKey("1000TurretPlayerOutlay"))
                info.m_1000TurretPlayerOutlay = Convert.ToInt64(data["1000TurretPlayerOutlay"]);

            if (data.ContainsKey("1000TurretPlayerCount"))
                info.m_1000TurretPlayerCount = Convert.ToInt64(data["1000TurretPlayerCount"]);

            if (data.ContainsKey("1000TurretNoValueCount"))
                info.m_1000TurretNoValueCount = Convert.ToInt64(data["1000TurretNoValueCount"]);

            if (data.ContainsKey("1000TurretRewardOutlay"))
                info.m_1000TurretRewardOutlay = Convert.ToInt64(data["1000TurretRewardOutlay"]);

            if (data.ContainsKey("1000TurretPunishIncome"))
                info.m_1000TurretPunishIncome = Convert.ToInt64(data["1000TurretPunishIncome"]);

            ////
            if (data.ContainsKey("5000TurretPlayerIncome"))
                info.m_5000TurretPlayerIncome = Convert.ToInt64(data["5000TurretPlayerIncome"]);

            if (data.ContainsKey("5000TurretPlayerOutlay"))
                info.m_5000TurretPlayerOutlay = Convert.ToInt64(data["5000TurretPlayerOutlay"]);

            if (data.ContainsKey("5000TurretPlayerCount"))
                info.m_5000TurretPlayerCount = Convert.ToInt64(data["5000TurretPlayerCount"]);

            if (data.ContainsKey("5000TurretNoValueCount"))
                info.m_5000TurretNoValueCount = Convert.ToInt64(data["5000TurretNoValueCount"]);

            if (data.ContainsKey("5000TurretRewardOutlay"))
                info.m_5000TurretRewardOutlay = Convert.ToInt64(data["5000TurretRewardOutlay"]);

            if (data.ContainsKey("5000TurretPunishIncome"))
                info.m_5000TurretPunishIncome = Convert.ToInt64(data["5000TurretPunishIncome"]);

            ////
            if (data.ContainsKey("10000TurretPlayerIncome"))
                info.m_10000TurretPlayerIncome = Convert.ToInt64(data["10000TurretPlayerIncome"]);

            if (data.ContainsKey("10000TurretPlayerOutlay"))
                info.m_10000TurretPlayerOutlay = Convert.ToInt64(data["10000TurretPlayerOutlay"]);

            if (data.ContainsKey("10000TurretPlayerCount"))
                info.m_10000TurretPlayerCount = Convert.ToInt64(data["10000TurretPlayerCount"]);

            if (data.ContainsKey("10000TurretNoValueCount"))
                info.m_10000TurretNoValueCount = Convert.ToInt64(data["10000TurretNoValueCount"]);

            if (data.ContainsKey("10000TurretRewardOutlay"))
                info.m_10000TurretRewardOutlay = Convert.ToInt64(data["10000TurretRewardOutlay"]);

            if (data.ContainsKey("10000TurretPunishIncome"))
                info.m_10000TurretPunishIncome = Convert.ToInt64(data["10000TurretPunishIncome"]);

            ////
            if (data.ContainsKey("PersonalBaseRate"))
                info.m_baseRate = Convert.ToDouble(data["PersonalBaseRate"]);

            if (data.ContainsKey("PersonalDeviationFix"))
                info.m_deviationFix = Convert.ToDouble(data["PersonalDeviationFix"]);

            if (data.ContainsKey("NoValuePlayerRate"))
                info.m_noValuePlayerRate = Convert.ToDouble(data["NoValuePlayerRate"]);
        }

        return OpRes.opres_success;
    }
}

//爆金比赛场参数查询
public class FishlordBaojinParamItem 
{
    public int m_roomId;
    public long m_sysTotalIncome;
    public long m_sysTotalOutlay;
    public double m_expRate;
    public int m_currentPersonNum;
    public long m_killOutcome;
    public long m_pricePool;
    public double m_pumpRate;
}
public class QueryFishlordBaojinParam : QueryBase 
{
    protected List<FishlordBaojinParamItem> m_result = new List<FishlordBaojinParamItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        //房间11
        IMongoQuery imq =Query.EQ("room_id", 11);
        m_result.Clear();
        return query(user, TableName.FISHLORD_ROOM,imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user) { return m_result; }

    protected OpRes query(GMUser user, string tableName,IMongoQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME ,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip, param,0,0,null,"room_id",true);
        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            FishlordBaojinParamItem info = new FishlordBaojinParamItem();
            m_result.Add(info);
            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if(dataList[i].ContainsKey("TotalIncome"))
            {
                info.m_sysTotalIncome=Convert.ToInt64(dataList[i]["TotalIncome"]);
            }
            if (dataList[i].ContainsKey("TotalOutlay"))
            {
                info.m_sysTotalOutlay = Convert.ToInt64(dataList[i]["TotalOutlay"]);
            }

            if (info.m_sysTotalIncome!=0)
            {
                info.m_expRate = Math.Round((info.m_sysTotalIncome - info.m_sysTotalOutlay) * 1.0 / info.m_sysTotalIncome, 2);
            }
            else 
            {
                info.m_expRate = 0;
            }

            if(dataList[i].ContainsKey("player_count"))
            {
                info.m_currentPersonNum = Convert.ToInt32(dataList[i]["player_count"]);
            }
            if (dataList[i].ContainsKey("PumpRate"))
            {
                info.m_pumpRate = Convert.ToDouble(dataList[i]["PumpRate"]);
            }
            else 
            {
                info.m_pumpRate = 0.02;
            }
        }

        return OpRes.opres_success;
    }
}

//竞技场得分修改
public class FishlordBaojinScoreParamItem
{
    public int m_playerId;
    public string m_nickName;
    public int m_todayMaxScore;
    public int m_weekMaxScore;
    public bool m_isRobot = false;
    public string m_time;
}
public class QueryFishlordBaojinScoreParam : QueryBase
{
    static string[] m_fields = { "playerId", "nickName", "todayMaxScore", "weekMaxScore", "isRobot" };

    protected List<FishlordBaojinScoreParamItem> m_result = new List<FishlordBaojinScoreParamItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p=(ParamQuery)param;
        int playerId = 0;
        if (!string.IsNullOrEmpty(p.m_param))
        {
            if (!int.TryParse(p.m_param, out playerId))
            {
                return OpRes.op_res_param_not_valid;
            }
            else
            {
                playerId = Convert.ToInt32(p.m_param);
                IMongoQuery imq = Query.EQ("playerId", playerId);
                m_result.Clear();
                return query(user, TableName.STAT_FISHLORD_BAOJIN_RANK, imq);
            }
        }
        else 
        {
            return OpRes.op_res_need_at_least_one_cond;
        }
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user) { return m_result; }

    protected OpRes query(GMUser user, string tableName, IMongoQuery param)
    {
        Dictionary<string, object> dataList =
            DBMgr.getInstance().getTableData(tableName, user.getDbServerID(), DbName.DB_GAME, param, m_fields);
        if (dataList == null)
        {
            return OpRes.op_res_not_found_data;
        }else
        {
            FishlordBaojinScoreParamItem info = new FishlordBaojinScoreParamItem();
            m_result.Add(info);
            info.m_playerId = Convert.ToInt32(dataList["playerId"]);
            if(dataList.ContainsKey("nickName"))
            {
                info.m_nickName=Convert.ToString(dataList["nickName"]);
            }
            if(dataList.ContainsKey("todayMaxScore"))
            {
                info.m_todayMaxScore = Convert.ToInt32(dataList["todayMaxScore"]);
            }
            if(dataList.ContainsKey("weekMaxScore"))
            {
                info.m_weekMaxScore = Convert.ToInt32(dataList["weekMaxScore"]);
            }
            if (dataList.ContainsKey("isRobot"))
            {
                info.m_isRobot = Convert.ToBoolean(dataList["isRobot"]);
            }
            return OpRes.opres_success;
        }
    }
}
//玩家龙鳞数量信息
public class QueryTypeDragonScaleControl : QueryBase
{
    static string[] m_fields = { "playerId", "nickName", "dragonScale", "weekDimensityHistory", "isRobot" };

    protected List<FishlordBaojinScoreParamItem> m_result = new List<FishlordBaojinScoreParamItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        int playerId = 0;
        if (!string.IsNullOrEmpty(p.m_param))
        {
            if (!int.TryParse(p.m_param, out playerId))
            {
                return OpRes.op_res_param_not_valid;
            }
            else
            {
                playerId = Convert.ToInt32(p.m_param);
                IMongoQuery imq = Query.EQ("playerId", playerId);
                m_result.Clear();
                return query(user, TableName.FISHLORD_DRAGON_PALACE_PLAYER, imq);
            }
        }
        else
        {
            return OpRes.op_res_need_at_least_one_cond;
        }
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user) { return m_result; }

    protected OpRes query(GMUser user, string tableName, IMongoQuery param)
    {
        Dictionary<string, object> dataList =
            DBMgr.getInstance().getTableData(tableName, user.getDbServerID(), DbName.DB_GAME, param, m_fields);
        if (dataList == null)
            return OpRes.op_res_not_found_data;
        FishlordBaojinScoreParamItem info = new FishlordBaojinScoreParamItem();
        m_result.Add(info);
        info.m_playerId = Convert.ToInt32(dataList["playerId"]);
        if (dataList.ContainsKey("nickName"))
            info.m_nickName = Convert.ToString(dataList["nickName"]);

        if (dataList.ContainsKey("weekDimensityHistory"))
            info.m_weekMaxScore = Convert.ToInt32(dataList["weekDimensityHistory"]);
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
// 鳄鱼公园参数查询
public class QueryFishParkParam : QueryFishlordParam
{
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user, TableName.FISHPARK_ROOM);
    }
}

//////////////////////////////////////////////////////////////////////////
// 经典捕鱼桌子参数查询
public class QueryFishlordDeskParam : QueryBase
{
    // 以桌子ID为key
    protected List<ResultFishlordExpRate> m_result = new List<ResultFishlordExpRate>();

    public override OpRes doQuery(object param, GMUser user)
    {
        return _doQuery(param, user, TableName.FISHLORD_ROOM_DESK);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    protected OpRes _doQuery(object param, GMUser user, string tableName)
    {
        m_result.Clear();
        ParamQueryGift p = (ParamQueryGift)param;
        IMongoQuery imq = Query.EQ("room_id", BsonValue.Create(p.m_roomId));
        return query(user, imq, p, tableName);
    }

    private OpRes query(GMUser user,IMongoQuery imq, ParamQueryGift param, string tableName)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(tableName, imq, dip);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName,
            dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "table_id");

        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultFishlordExpRate info = new ResultFishlordExpRate();
            info.m_roomId = Convert.ToInt32(dataList[i]["table_id"]);
            if (dataList[i].ContainsKey("TotalIncome"))
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["TotalIncome"]);
            }
            if (dataList[i].ContainsKey("TotalOutlay"))
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["TotalOutlay"]);
            }
            if (dataList[i].ContainsKey("Abandonedbullets"))
            {
                info.m_abandonedbullets = Convert.ToInt64(dataList[i]["Abandonedbullets"]);
            }
            m_result.Add(info);
        }
        return OpRes.opres_success;
    }
}

public class QueryFishParkDeskParam : QueryFishlordDeskParam
{
    public override OpRes doQuery(object param, GMUser user)
    {
        return _doQuery(param, user, TableName.FISHPARK_ROOM_DESK);
    }
}

//////////////////////////////////////////////////////////////////////////
//奔驰宝马参数调整
public class QueryBzParam : QueryBase
{
    private Dictionary<int, ResultFishlordExpRate> m_result = new Dictionary<int, ResultFishlordExpRate>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.DB_BZ_ROOM, dip);
        if (dataList == null)
            return OpRes.opres_success;
 
        for (int i = 0; i < dataList.Count; i++)
        {
            ResultFishlordExpRate info = new ResultFishlordExpRate();
            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if (dataList[i].ContainsKey("ExpectEarnRate"))
            {
                info.m_expRate = Convert.ToDouble(dataList[i]["ExpectEarnRate"]);
            }
            else
            {
                info.m_expRate = 0.05;
            }
            if (dataList[i].ContainsKey("room_income"))
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["room_income"]);
            }
            if (dataList[i].ContainsKey("room_outcome"))
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["room_outcome"]);
            }
            if (dataList[i].ContainsKey("player_count"))
            {
                info.m_curPlayerCount = Convert.ToInt32(dataList[i]["player_count"]);
            }
            if(dataList[i].ContainsKey("AppearAdvertisementLessBet"))
            {
                info.m_appearAdvertisementLessBet = Convert.ToInt64(dataList[i]["AppearAdvertisementLessBet"]);
            }

            if (dataList[i].ContainsKey("BigParadiseGiveScoreProb")) 
            {
                info.m_bigParadiseGiveScoreProb = Convert.ToInt32(dataList[i]["BigParadiseGiveScoreProb"]);
            }

            if (dataList[i].ContainsKey("SmallParadiseGiveScoreProb")) 
            {
                info.m_smallParadiseGiveScoreProb = Convert.ToInt32(dataList[i]["SmallParadiseGiveScoreProb"]);
            }

            if (dataList[i].ContainsKey("BigHellKillScoreProb")) 
            {
                info.m_bigHellKillScoreProb = Convert.ToInt32(dataList[i]["BigHellKillScoreProb"]);
            }

            if (dataList[i].ContainsKey("SmallHellKillScoreProb")) 
            {
                info.m_smallHellKillScoreProb = Convert.ToInt32(dataList[i]["SmallHellKillScoreProb"]);
            }

            m_result.Add(info.m_roomId, info);
        }

        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////
// 鳄鱼参数查询
public class QueryCrocodileParam : QueryBase
{
    private Dictionary<int, ResultFishlordExpRate> m_result = new Dictionary<int, ResultFishlordExpRate>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.CROCODILE_ROOM, dip);
        if (dataList == null)
            return OpRes.opres_success;
 
        for (int i = 0; i < dataList.Count; i++)
        {
            ResultFishlordExpRate info = new ResultFishlordExpRate();
            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if (dataList[i].ContainsKey("ExpectEarnRate"))
            {
                info.m_expRate = Convert.ToDouble(dataList[i]["ExpectEarnRate"]);
            }
            else
            {
                info.m_expRate = 0.05;
            }
            if (dataList[i].ContainsKey("room_income"))
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["room_income"]);
            }
            if (dataList[i].ContainsKey("room_outcome"))
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["room_outcome"]);
            }
            if (dataList[i].ContainsKey("player_count"))
            {
                info.m_curPlayerCount = Convert.ToInt32(dataList[i]["player_count"]);
            }
            if(dataList[i].ContainsKey("AppearAdvertisementLessBet"))
            {
                info.m_appearAdvertisementLessBet = Convert.ToInt64(dataList[i]["AppearAdvertisementLessBet"]);
            }

            if (dataList[i].ContainsKey("BigParadiseGiveScoreProb")) 
            {
                info.m_bigParadiseGiveScoreProb = Convert.ToInt32(dataList[i]["BigParadiseGiveScoreProb"]);
            }

            if (dataList[i].ContainsKey("SmallParadiseGiveScoreProb")) 
            {
                info.m_smallParadiseGiveScoreProb = Convert.ToInt32(dataList[i]["SmallParadiseGiveScoreProb"]);
            }

            if (dataList[i].ContainsKey("BigHellKillScoreProb")) 
            {
                info.m_bigHellKillScoreProb = Convert.ToInt32(dataList[i]["BigHellKillScoreProb"]);
            }

            if (dataList[i].ContainsKey("SmallHellKillScoreProb")) 
            {
                info.m_smallHellKillScoreProb = Convert.ToInt32(dataList[i]["SmallHellKillScoreProb"]);
            }

            m_result.Add(info.m_roomId, info);
        }

        return OpRes.opres_success;
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////
//水果机参数调整
public class QueryFruitParamControl : QueryBase
{
    private Dictionary<int, ResultFishlordExpRate> m_result = new Dictionary<int, ResultFishlordExpRate>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.FRUIT_ROOM, dip);
        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultFishlordExpRate info = new ResultFishlordExpRate();
            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if (dataList[i].ContainsKey("ExpectEarnRate"))
            {
                info.m_expRate = Convert.ToDouble(dataList[i]["ExpectEarnRate"]);
            }
            else
            {
                info.m_expRate = 0.05;
            }
            if (dataList[i].ContainsKey("room_income"))
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["room_income"]);
            }
            if (dataList[i].ContainsKey("room_outcome"))
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["room_outcome"]);
            }
            if (dataList[i].ContainsKey("player_count"))
            {
                info.m_curPlayerCount = Convert.ToInt32(dataList[i]["player_count"]);
            }

            m_result.Add(info.m_roomId, info);
        }

        return OpRes.opres_success;
    }
}
////////////////////////////////////////////////////////////////////////////
public class ResultFish
{
    public int m_fishId;
    public string m_fishName = "";
    public long m_hitCount;
    public long m_dieCount;
    public long m_outlay;
    public long m_income;
    public int m_roomId;

    public long m_totaloutlayChip;
    public string m_time;

    // 死亡/击中
    public string getHit_Die()
    {
        if (m_hitCount == 0)
            return "无穷大";

        double val = (double)m_dieCount / m_hitCount * 10000;
        return "万分之 " + Math.Round(val, 2).ToString();
    }

    public string getOutlay_Income()
    {
        if (m_income == 0 && m_outlay == 0)
            return "0";
        if (m_income == 0)
            return "-∞";

        double factGain = (double)(m_income - m_outlay) * 100 / m_income;
        return Math.Round(factGain, 3).ToString() + "%";
    }

    // 总计折合盈利率
    public string getZheheEarn()
    {
        int goldValue = 0;
        var item = ItemCFG.getInstance().getValue(30); //获取碎片信息
        if (item != null && !string.IsNullOrEmpty(item.m_goldValue)) 
        {
            goldValue = Convert.ToInt32(item.m_goldValue);
        }

        long outlay = m_outlay + m_totaloutlayChip * goldValue;
        return ItemHelp.getFactExpRate(m_income, outlay, true);
    }
}

// 鱼
public class QueryFish : QueryBase
{
    private List<ResultFish> m_result = new List<ResultFish>();

    public override OpRes doQuery(object param, GMUser user)
    {
        return _doQuery(param, user, TableName.PUMP_ALL_FISH);
    }

    protected OpRes _doQuery(object param, GMUser user, string tableName)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq = null;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        int roomId = p.m_op;
        if (roomId > 0)
        {
            imq = Query.And(imq, Query.EQ("roomid", BsonValue.Create(roomId)));
        }
        else
        {
            imq = Query.And(imq,
                    Query.Or(Query.EQ("roomid", BsonValue.Create(1)),
                           Query.EQ("roomid", BsonValue.Create(2)),
                           Query.EQ("roomid", BsonValue.Create(3)),
                           Query.EQ("roomid", BsonValue.Create(4)),
                           Query.EQ("roomid", BsonValue.Create(5)),
                           Query.EQ("roomid", BsonValue.Create(6))
                           ));
        }
        OpRes res1 = query(user, imq, tableName, p);
        //m_result.Sort(sortFish);
        return res1;
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, string tableName, ParamQuery param)
    {

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(tableName, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("roomid").Ascending("fishid");

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery2(tableName, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, sort);
        if (dataList == null)
            return OpRes.op_res_failed;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultFish info = new ResultFish();
            info.m_fishId = Convert.ToInt32(dataList[i]["fishid"]);
            info.m_hitCount = Convert.ToInt64(dataList[i]["hitcount"]);
            info.m_dieCount = Convert.ToInt64(dataList[i]["deadcount"]);

            //碎片
            long chipIncome = 0, chipOutlay = 0;
            if (dataList[i].ContainsKey("chipIncome"))
                chipIncome = Convert.ToInt64(dataList[i]["chipIncome"]);

            if (dataList[i].ContainsKey("chipOutlay"))
                chipOutlay = Convert.ToInt64(dataList[i]["chipOutlay"]);

            long totalincome = 0,  totaloutlay = 0;
            if (dataList[i].ContainsKey("totalincome"))
                totalincome = Convert.ToInt64(dataList[i]["totalincome"]);

            if(dataList[i].ContainsKey("totaloutlay"))
                totaloutlay = Convert.ToInt64(dataList[i]["totaloutlay"]);

            info.m_income = chipIncome + totalincome;
            info.m_outlay = chipOutlay + totaloutlay;

            if (dataList[i].ContainsKey("roomid"))
                info.m_roomId = Convert.ToInt32(dataList[i]["roomid"]);
         
            //时间
            if (dataList[i].ContainsKey("genTime"))
                info.m_time = Convert.ToDateTime(dataList[i]["genTime"]).ToLocalTime().ToShortDateString();

            m_result.Add(info);
        }

        return OpRes.opres_success;
    }

    private int sortFish(ResultFish p1, ResultFish p2)
    {
        if(p1.m_roomId == p2.m_roomId)
            return p1.m_fishId - p2.m_fishId;
        return p1.m_roomId - p2.m_roomId;
    }
}

// 鳄鱼公园鱼的统计
public class QueryFishParkStat : QueryFish
{
    public override OpRes doQuery(object param, GMUser user)
    {
        return _doQuery(param, user, TableName.PUMP_ALL_FISH_PARK);
    }
}

//////////////////////////////////////////////////////////////////////////

public class ResultMoneyMost
{
    public int m_playerId;
    public string m_nickName = "";
    public int m_val;
    public int m_safeBox;
}

// 金币最多
public class QueryMoneyAtMost : QueryBase
{
    private List<ResultMoneyMost> m_result = new List<ResultMoneyMost>();
    static string[] s_fieldGold = new string[] { "player_id", "gold", "safeBoxGold" };
    static string[] s_fieldTicket = new string[] { "player_id", "ticket" };
    static string[] s_field = new string[] { "nickname" };

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = Query.EQ("is_robot", false);
        switch (p.m_way)
        {
            case QueryWay.by_way0:
                    return queryGold(user, p.m_countEachPage, imq);
            case QueryWay.by_way1:
                    return queryTicket(user, p.m_countEachPage, imq);
        }
        return OpRes.op_res_failed;
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes queryGold(GMUser user, int maxCount, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO,
            dip, imq, 0, maxCount, s_fieldGold, "gold", false);

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultMoneyMost info = new ResultMoneyMost();
            info.m_playerId = Convert.ToInt32(dataList[i]["player_id"]);
            info.m_val = Convert.ToInt32(dataList[i]["gold"]);

            Dictionary<string, object> ret = getPlayerProperty(info.m_playerId, user, s_field);
            if (ret != null && ret.ContainsKey("nickname"))
            {
                info.m_nickName = Convert.ToString(ret["nickname"]);
            }
            if (dataList[i].ContainsKey("safeBoxGold"))
            {
                info.m_safeBox = Convert.ToInt32(dataList[i]["safeBoxGold"]);
            }
            m_result.Add(info);
        }

        return OpRes.opres_success;
    }

    private OpRes queryTicket(GMUser user, int maxCount, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO,
            dip, imq, 0, maxCount, s_fieldTicket, "ticket", false);

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultMoneyMost info = new ResultMoneyMost();
            info.m_playerId = Convert.ToInt32(dataList[i]["player_id"]);
            info.m_val = Convert.ToInt32(dataList[i]["ticket"]);

            Dictionary<string, object> ret = getPlayerProperty(info.m_playerId, user, s_field);
            if (ret != null && ret.ContainsKey("nickname"))
            {
                info.m_nickName = Convert.ToString(ret["nickname"]);
            }
            m_result.Add(info);
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ResultOldEarningRateItem
{
    public int m_gameId;
    public string m_resetTime = "";
    public int m_roomId;
    public long m_income;
    public long m_outlay;
    public double m_expRate;

    // 返回实际盈利率
    public string getFactExpRate()
    {
        if (m_income == 0 && m_outlay == 0)
            return "0";
        if (m_income == 0)
            return "-∞";

        double factGain = (double)(m_income - m_outlay) / m_income;
        return Math.Round(factGain, 3).ToString();
    }
}

// 旧有盈利率查询
public class QueryOldEarningRate : QueryBase
{
    private List<ResultOldEarningRateItem> m_result = new List<ResultOldEarningRateItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamLottery p = (ParamLottery)param;
        List<IMongoQuery> queryList = new List<IMongoQuery>();
        queryList.Add(Query.EQ("gameId", (int)p.m_way));
        IMongoQuery imq = Query.And(queryList);
        return query(user, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_OLD_EARNINGS_RATE, dip, imq);

        if (dataList != null)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                ResultOldEarningRateItem info = new ResultOldEarningRateItem();
                info.m_gameId = Convert.ToInt32(dataList[i]["gameId"]);
                info.m_resetTime = Convert.ToDateTime(dataList[i]["time"]).ToLocalTime().ToString();
                info.m_roomId = Convert.ToInt32(dataList[i]["roomId"]);
                info.m_income = Convert.ToInt64(dataList[i]["income"]);
                info.m_outlay = Convert.ToInt64(dataList[i]["outlay"]);

                if (dataList[i].ContainsKey("expRate"))
                {
                    info.m_expRate = Convert.ToDouble(dataList[i]["expRate"]);
                }
                m_result.Add(info);
            }
        }
       
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class FishlordStageItem
{
    public string m_time = "";
    public int m_roomId;
    public int m_stage;
    public long m_outlay;
    public long m_income;

    // 返回实际盈利率
    public string getFactExpRate()
    {
        if (m_income == 0 && m_outlay == 0)
            return "0";
        if (m_income == 0)
            return "-∞";

        double factGain = (double)(m_income - m_outlay) / m_income;
        return Math.Round(factGain, 3).ToString();
    } 
}

// 经典捕鱼阶段
public class QueryFishlordStage : QueryBase
{
    private List<FishlordStageItem> m_result = new List<FishlordStageItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        return _doQuery(param, user, TableName.PUMP_FISH_TABLE_LOG);
    }

    protected OpRes _doQuery(object param, GMUser user, string tableName)
    {
        m_result.Clear();
        ParamQueryGift p = (ParamQueryGift)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();

        if (p.m_param != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_param, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
            queryList.Add(Query.And(imq1, imq2));
        }

        queryList.Add(Query.EQ("roomid", BsonValue.Create(p.m_state + 1)));

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return query(p, imq, user, tableName);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQueryGift param, IMongoQuery imq, GMUser user, string tableName)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(tableName, imq, dip);

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(tableName, dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "time", false);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            FishlordStageItem tmp = new FishlordStageItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data[i]["time"]).ToLocalTime().ToString();
            tmp.m_roomId = Convert.ToInt32(data[i]["roomid"]);
            tmp.m_stage = Convert.ToInt32(data[i]["type"]);
            tmp.m_outlay = Convert.ToInt64(data[i]["outlay"]);
            tmp.m_income = Convert.ToInt64(data[i]["income"]);
        }
        return OpRes.opres_success;
    }
}

// 鳄鱼公园阶段分析
public class QueryFishParkStage : QueryFishlordStage
{
    public override OpRes doQuery(object param, GMUser user)
    {
        return _doQuery(param, user, TableName.PUMP_FISH_PARK_TABLE_LOG);
    }
}

//////////////////////////////////////////////////////////////////////////

// 在线人数查询
public class QueryOnlinePlayerCount : QueryBase
{
    private int m_count = 0;

    public override OpRes doQuery(object param, GMUser user)
    {
        m_count = 0;
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.COMMON_CONFIG, "type", "cur_playercount", dip);
        if (data != null)
        {
            m_count = Convert.ToInt32(data["value"]);
        }
        return OpRes.opres_success;
    }

    public override object getQueryResult()
    {
        return m_count;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamQueryOpLog : ParamQuery
{
    public int m_logType;
}

// 查询操作日志
public class QueryOpLog : QueryBase
{
    List<Dictionary<string, object>> m_result = new List<Dictionary<string, object>>();
    private QueryCondition m_cond = new QueryCondition();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_cond.startQuery();
        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        IMongoQuery imq = m_cond.getImq();
        ParamQuery p = (ParamQuery)param;
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQueryOpLog p = (ParamQueryOpLog)param;
        if (p.m_logType != -1)
        {
            queryCond.addQueryCond("OpType", p.m_logType);
        }
        if(!string.IsNullOrEmpty(p.m_time))
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("OpTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("OpTime", BsonValue.Create(mint));
            queryCond.addImq(Query.And(imq1, imq2));
        }
        if (!user.isAdmin())
        {
            queryCond.addImq(Query.EQ("account", user.m_user));
        }
        return OpRes.opres_success;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.OPLOG, imq, 0, DbName.DB_ACCOUNT);

        m_result = DBMgr.getInstance().executeQuery(TableName.OPLOG,
                                                     0,
                                                     DbName.DB_ACCOUNT,
                                                     imq,
                                                     (param.m_curPage - 1) * param.m_countEachPage,
                                                     param.m_countEachPage,
                                                     null,
                                                     "OpTime",
                                                     false);
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

// 查询玩家头像
public class QueryPlayerHead : QueryBase
{
    // 头像所在的地址URL
    private string m_headUrl = "";
    private string[] m_retFields = { "icon_custom" };
    public override OpRes doQuery(object param, GMUser user)
    {
        m_headUrl = "#";
        string strId = (string)param;
        int playerId = 0;
        if (!int.TryParse(strId, out playerId))
        {
            return OpRes.op_res_param_not_valid;
        }

        Dictionary<string, object> data = QueryBase.getPlayerProperty(playerId, user, m_retFields);
        if (data == null)
            return OpRes.op_res_not_found_data;
        if (!data.ContainsKey("icon_custom"))
            return OpRes.op_res_not_found_data;
        
        string head = Convert.ToString(data["icon_custom"]);
        if (head == "")
            return OpRes.op_res_not_found_data;

        uint temp = Convert.ToUInt32(playerId) % 10000;
        string url = WebConfigurationManager.AppSettings["headURL"];
        m_headUrl = string.Format(url, temp, head);
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_headUrl;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ParamTotalConsume
{
    // 货币类型
    public int m_currencyType;

    // 收入or支出
    public int m_changeType;

    // 时间范围
    public string m_time = "";

    //房间类型
    public string m_roomType="0";
}

public class ConsumeOneItem
{
    public long m_totalValue; // 总量
    public long m_totalCount; // 总次数
}

public class TotalConsumeItem
{
    // 统计时间
    public DateTime m_time;
    // 原因-->消耗总量
   // public Dictionary<int, long> m_result = new Dictionary<int, long>();
    public Dictionary<int, ConsumeOneItem> m_result = new Dictionary<int, ConsumeOneItem>();
    public void add(int reason, long value, long count = 0)
    {
        if (!m_result.ContainsKey(reason))
        {
            ConsumeOneItem item = new ConsumeOneItem();
            item.m_totalValue = value;
            item.m_totalCount = count;
            m_result.Add(reason, item);
        }
        else 
        {
            m_result[reason].m_totalValue += value;
            m_result[reason].m_totalCount += count;
        }
    }
    public ConsumeOneItem getValue(int reason)
    {
        if (m_result.ContainsKey(reason))
            return m_result[reason];
        return null;
    }
}

public class ResultTotalConsume
{
    public HashSet<int> m_fields = new HashSet<int>();

    public List<TotalConsumeItem> m_result = new List<TotalConsumeItem>();

    public void addReason(int reason)
    {
        m_fields.Add(reason);
    }

    public void reset()
    {
        m_fields.Clear();
        m_result.Clear();
    }

    public TotalConsumeItem create(DateTime date)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == date)
                return d;
        }

        TotalConsumeItem item = new TotalConsumeItem();
        m_result.Add(item);
        item.m_time = date;
        return item;
    }

    public string getReason(int r)
    {
        XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");
        string result = xml.getString(r.ToString(), "");
        if (result == "")
            return r.ToString();
        return result;
        //return xml.getString(r.ToString(), "");
    }

    public string getFishReason(int r)
    {
        // 购买物品
        if (r >= (int)FishLordExpend.fish_buyitem_start && r < (int)FishLordExpend.fish_useskill_start)
        {
            ItemCFGData data = ItemCFG.getInstance().getValue(r);
            if (data != null)
            {
                return "购买 " + data.m_itemName;
            }
        }

        // 使用技能
        if (r >= (int)FishLordExpend.fish_useskill_start && r < (int)FishLordExpend.fish_turrent_uplevel_start)
        {
            r = r - (int)FishLordExpend.fish_useskill_start;
            Fish_BuffCFGData data = Fish_BuffCFG.getInstance().getValue(r);
            if (data != null)
            {
                return "使用技能 " + data.m_buffName;
            }
        }

        // 炮台升级
        if (r >= (int)FishLordExpend.fish_turrent_uplevel_start && r < (int)FishLordExpend.fish_unlock_level_start)
        {
            return "解锁房间-" + (r - (int)FishLordExpend.fish_turrent_uplevel_start);
        }

        // 解锁 数据库中fish_turrent_uplevel_start,  fish_unlock_level_start记反了
        if (r >= (int)FishLordExpend.fish_unlock_level_start && r < (int)FishLordExpend.fish_missile) 
        {
            return "炮台升级-" + (r - (int)FishLordExpend.fish_unlock_level_start);
        }

        if (r >= (int)FishLordExpend.fish_missile)
        {
            return string.Format("导弹消耗({0})", StrName.s_fishRoomName[r - (int)FishLordExpend.fish_missile]);
        }
        return "";
    }

    public int getResultCount()
    {
        return m_result.Count;
    }
}

// 总的货币消耗查询

public class StatTotalConsumeItem 
{
    public string m_time;
    public int m_flag;
    public Dictionary<int, long> m_totalConsume = new Dictionary<int, long>();
}

public class QueryTotalConsume : QueryBase
{
    private List<StatTotalConsumeItem> m_result = new List<StatTotalConsumeItem>();

    XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");

    public StatTotalConsumeItem IsCreate(string time) 
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag++;
                return d;
            }
        }

        StatTotalConsumeItem item = new StatTotalConsumeItem();
        m_result.Add(item);
        item.m_time = time;
  
        //获取变化原因 默认值
        foreach (PropertyReasonType type in Enum.GetValues(typeof(PropertyReasonType)))
        {
            if (type == PropertyReasonType.type_max)
                continue;
            item.m_totalConsume.Add((int)type, 0L);
        }
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamTotalConsume p = (ParamTotalConsume)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
        queryList.Add(Query.And(imq1, imq2));

        queryList.Add(Query.EQ("changeType", BsonValue.Create(p.m_changeType)));
        queryList.Add(Query.EQ("itemId", BsonValue.Create(p.m_currencyType)));

        IMongoQuery imq = Query.And(queryList);
        return query(user, imq, p.m_currencyType);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, int type)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("time").Ascending("reason");

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery2(TableName.PUMP_TOTAL_CONSUME, dip, imq, 0, 0, null, sort);

        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < data.Count; i++)
        {
            DateTime time = Convert.ToDateTime(data[i]["time"]).ToLocalTime();
            string t = time.ToShortDateString();
            StatTotalConsumeItem item = IsCreate(t);

            int reason = Convert.ToInt32(data[i]["reason"]);

            long val = 0;
            if(data[i].ContainsKey("value"))
                val = Convert.ToInt64(data[i]["value"]);
            item.m_totalConsume[reason] = val;

            if (item.m_flag != 0 || type != 1)
                continue;

            //新手引导 南海寻宝
            IMongoQuery q1 = Query.And(Query.EQ("genTime", time.Date), Query.EQ("roomId", 0));
            Dictionary<string, object> treasureHunt =
                DBMgr.getInstance().getTableData(TableName.STAT_PUMP_TREASURE_HUNT_ROOM, dip, q1);
            if (treasureHunt != null)
            {
                int count = 0;
                if (treasureHunt.ContainsKey("joinCount"))
                    count = Convert.ToInt32(treasureHunt["joinCount"]);

                item.m_totalConsume[111] = 15000 * count;
            }
        }
        
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class GameRechargeItem
{
    // 游戏名称-->充值总额
    public Dictionary<string, long> m_recharge = new Dictionary<string, long>();

    public DateTime m_time;

    // 总的金额
    public long m_totalRecharge = 0;

    public void add(string game, long value)
    {
        long t = 0;
        if (m_recharge.ContainsKey(game))
        {
            t = Convert.ToInt64(m_recharge[game]);
        }

        t += value;
        m_recharge[game] = t;
    }

    public long getValue(string game)
    {
        if (m_recharge.ContainsKey(game))
            return m_recharge[game];

        return 0;
    }
}

public class ResultGameRecharge
{
    public HashSet<string> m_fields = new HashSet<string>();

    public List<GameRechargeItem> m_result = new List<GameRechargeItem>();

    public void addGame(string game)
    {
        m_fields.Add(game);
    }

    public void reset()
    {
        m_fields.Clear();
        m_result.Clear();
    }

    public GameRechargeItem create(DateTime date)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == date)
                return d;
        }

        GameRechargeItem item = new GameRechargeItem();
        m_result.Add(item);
        item.m_time = date;
        return item;
    }
}

// 按天查询每个游戏的充值总额
public class QueryGameRechargeByDay : QueryBase
{
    private ResultGameRecharge m_result = new ResultGameRecharge();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        string time = (string)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("date", BsonValue.Create(mint));
        queryList.Add(Query.And(imq1, imq2));

        IMongoQuery imq = Query.And(queryList);

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.GAME_RECHARGE_INFO, serverId,
             DbName.DB_PAYMENT, imq);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            DateTime t = Convert.ToDateTime(data[i]["date"]).ToLocalTime();
            GameRechargeItem item = m_result.create(t);

            foreach (var game in data[i].Keys)
            {
                if (game == "date" || game == "_id")
                    continue;

                if (game == "total_rmb")
                {
                    item.m_totalRecharge += Convert.ToInt64(data[i]["total_rmb"]);
                }
                else
                {
                    long val = Convert.ToInt64(data[i][game]);
                    item.add(game, val);
                    m_result.addGame(game);
                }
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class ResultCoinGrowthRank
{
    public int m_playerId;
    public string m_acc = "";
    public string m_nickName = "";
    public int m_vipLevel;
    public long m_gold;
    public string m_time = "";
}

public class ResultRankItem
{
    public int m_playerId;
    public long m_value;
    public int m_rechargeTotal;
    public DateTime m_lastLogin;
    public int m_dailyRecharge;
    public string m_channel="";
}

public class ResultRichesRankItem 
{
    public int m_playerId;
    public string m_nickName;
    public int m_rank;
    public int m_rechargeTotal;
    public int m_dailyRecharge;
    public DateTime m_lastLogin;
}

public class ResultRank
{
    public Dictionary<DateTime, List<ResultRankItem>> m_result =
        new Dictionary<DateTime, List<ResultRankItem>>();

    public List<DateTime> m_timeList = new List<DateTime>();

    public void reset()
    {
        m_timeList.Clear();
        m_result.Clear();
    }

    public void add(DateTime t, ResultRankItem item)
    {
        List<ResultRankItem> res = null;
        if (m_result.ContainsKey(t))
        {
            res = m_result[t];
        }
        else
        {
            res = new List<ResultRankItem>();
            m_result.Add(t, res);
        }
        res.Add(item);
    }

    public List<ResultRankItem> getRank(DateTime t)
    {
        if (m_result.ContainsKey(t))
        {
            return m_result[t];
        }
        return null;
    }

    public string getJson(GMUser user)
    {
        string str = "";
        Dictionary<string, object> ret = new Dictionary<string, object>();
        for (int i = m_timeList.Count - 1; i >= 0; i--)
        {
            DateTime time = m_timeList[i];
            var r = getRank(time);
            if (r != null)
            {
                Table t = new Table();
                TableRank tr = new TableRank();
                tr.genTable(user, t, OpRes.opres_success, r);
                ret.Add(time.ToShortDateString(), ItemHelp.genHTML(t));
            }
        }
        str = ItemHelp.genJsonStr(ret);
        return str;
    }
}


public class PlayerRichesRank
{
    public Dictionary<DateTime, List<ResultRichesRankItem>> m_result = new Dictionary<DateTime, List<ResultRichesRankItem>>();

    public List<DateTime> m_timeList = new List<DateTime>();

    public void reset()
    {
        m_timeList.Clear();
        m_result.Clear();
    }

    public void add(DateTime t, ResultRichesRankItem item)
    {
        List<ResultRichesRankItem> res = null;
        if (m_result.ContainsKey(t))
        {
            res = m_result[t];
        }else
        {
            res = new List<ResultRichesRankItem>();
            m_result.Add(t, res);
        }
        res.Add(item);
    }

    public List<ResultRichesRankItem> getRank(DateTime t)
    {
        if (m_result.ContainsKey(t))
            return m_result[t];

        return null;
    }

    public string getJson(GMUser user)
    {
        string str = "";
        Dictionary<string, object> ret = new Dictionary<string, object>();
        ret.Clear();
        for (int i = m_timeList.Count - 1; i >= 0; i--)
        {
            DateTime time = m_timeList[i];
            var r = getRank(time);
            if (r != null)
            {
                Table t = new Table();
                TablePlayerRichesRank tr = new TablePlayerRichesRank();
                tr.genTable(user, t, OpRes.opres_success, r);
                ret.Add(time.ToShortDateString(), ItemHelp.genHTML(t));
            }
        }
        str = ItemHelp.genJsonStr(ret);
        return str;
    }
}

// 金币增长历史排行
public class QueryCoinGrowthRank : QueryBase
{
    static string[] s_tableName = { "rankGold", "rankGem"};
    ResultRank m_result = new ResultRank();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        int rankId = 0; // 排行表格
        if (!int.TryParse(p.m_param, out rankId))
            return OpRes.op_res_param_not_valid;
        
        m_result.reset();
        string fieldName = (p.m_way == QueryWay.by_way0) ? "growth" : "netGrowth";

        string channel = p.m_channelNo;
        while (mint < maxt)
        {
            query(user, s_tableName[rankId], fieldName, mint, channel);
            mint = mint.AddDays(1);
        }
        return OpRes.opres_success;
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, string tableName, string fieldName, DateTime time,string channel)
    {
        QueryCondition query_list = new QueryCondition();
        query_list.addImq(Query.EQ("genTime", time));
        if(!string.IsNullOrEmpty(channel))
            query_list.addImq(Query.EQ("channel", channel));

        IMongoQuery imq = query_list.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip, imq, 0, 50, null, fieldName, false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.opres_success;

        m_result.m_timeList.Add(time);
        for (int i = 0; i < dataList.Count; i++)
        {
            ResultRankItem info = new ResultRankItem();
            m_result.add(time, info);

            info.m_playerId = Convert.ToInt32(dataList[i]["playerId"]);
            //统计当日充值
            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
            if (serverId != -1)
            {
                QueryCondition queryList = new QueryCondition();
                queryList.addImq(Query.EQ("PlayerId", info.m_playerId));
                queryList.addImq(Query.EQ("status", 0));
                queryList.addImq(Query.And(Query.GTE("CreateTime", BsonValue.Create(time)), Query.LT("CreateTime", BsonValue.Create(time.AddDays(1)))));
                IMongoQuery imq1 = queryList.getImq();
                string[] field = new string[] { "RMB" };
                List<Dictionary<string, object>> rechargeList = DBMgr.getInstance().executeQuery(TableName.RECHARGE_TOTAL, serverId, DbName.DB_PAYMENT, imq1, 0, 0, field, "CreateTime", false);
                if (rechargeList != null && rechargeList.Count != 0)
                {
                    for (int j = 0; j < rechargeList.Count; j++)
                    {
                        if (rechargeList[j].ContainsKey("RMB"))
                        {
                            info.m_dailyRecharge += Convert.ToInt32(rechargeList[j]["RMB"]);
                        }
                    }
                }
            }
            //////
            if (dataList[i].ContainsKey(fieldName))
            {
                info.m_value = Convert.ToInt64(dataList[i][fieldName]);
            }

            if(!string.IsNullOrEmpty(channel))
            {
                string channelNo = Convert.ToString(dataList[i]["channel"]).PadLeft(6, '0');
                TdChannelInfo cd = TdChannel.getInstance().getValue(channelNo);
                if(cd!=null)
                {
                    info.m_channel = cd.m_channelName;
                }
            }
            
            Dictionary<string, object> ret =
                QueryBase.getPlayerProperty(info.m_playerId, user, new string[] { "logout_time", "recharged" });
            if (ret != null)
            {
                if (ret.ContainsKey("logout_time"))
                {
                    info.m_lastLogin = Convert.ToDateTime(ret["logout_time"]).ToLocalTime();
                }
                if (ret.ContainsKey("recharged"))
                {
                    info.m_rechargeTotal = Convert.ToInt32(ret["recharged"]);
                }
            }
        }

        return OpRes.opres_success;
    }
}

//玩家财富榜
public class QueryPlayerRichesRank : QueryBase 
{
    static string[] s_tableName = { "playerMoneyRank_Gold", "playerMoneyRank_DragonBall" };
    PlayerRichesRank m_result = new PlayerRichesRank();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.m_result.Clear();
        m_result.m_timeList.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        int rankId = 0; // 排行表格
        if (!int.TryParse(p.m_param, out rankId))
            return OpRes.op_res_param_not_valid;

        while (mint < maxt)
        {
            query(user, s_tableName[rankId], mint);
            mint = mint.AddDays(1);
        }
        return OpRes.opres_success;
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, string tableName, DateTime time)
    {
        QueryCondition query_list = new QueryCondition();
        query_list.addImq(Query.EQ("genTime", time));

        IMongoQuery imq = query_list.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, dip, imq, 0, 100, null, "rank", true);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        m_result.m_timeList.Add(time);
        for (int i = 0; i < dataList.Count; i++)
        {
            ResultRichesRankItem info = new ResultRichesRankItem();
            m_result.add(time, info);

            info.m_playerId = Convert.ToInt32(dataList[i]["playerId"]);

            if (dataList[i].ContainsKey("nickname"))
                info.m_nickName = Convert.ToString(dataList[i]["nickname"]);

            info.m_rank = Convert.ToInt32(dataList[i]["rank"]);

            if (dataList[i].ContainsKey("totalValue"))
                info.m_rechargeTotal = Convert.ToInt32(dataList[i]["totalValue"]);

            //统计当日充值
            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
            if (serverId != -1)
            {
                QueryCondition queryList = new QueryCondition();
                queryList.addImq(Query.EQ("PlayerId", info.m_playerId));
                queryList.addImq(Query.Or(Query.EQ("status", 0), Query.EQ("status", 2)));
                queryList.addImq(Query.And(Query.GTE("CreateTime", BsonValue.Create(time)), Query.LT("CreateTime", BsonValue.Create(time.AddDays(1)))));
                IMongoQuery imq1 = queryList.getImq();
                string[] field = new string[] { "RMB" };
                List<Dictionary<string, object>> rechargeList = DBMgr.getInstance().executeQuery(TableName.RECHARGE_TOTAL, serverId, DbName.DB_PAYMENT, imq1, 0, 0, field, "CreateTime", false);
                if (rechargeList != null && rechargeList.Count != 0)
                {
                    for (int j = 0; j < rechargeList.Count; j++)
                    {
                        if (rechargeList[j].ContainsKey("RMB"))
                            info.m_dailyRecharge += Convert.ToInt32(rechargeList[j]["RMB"]);
                    }
                }
            }

            Dictionary<string, object> ret =
                QueryBase.getPlayerProperty(info.m_playerId, user, new string[] { "logout_time"});
            if (ret != null)
            {
                if (ret.ContainsKey("logout_time"))
                    info.m_lastLogin = Convert.ToDateTime(ret["logout_time"]).ToLocalTime();
            }
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////

public class ResultAccountCoinLessValue
{
    // 总数量
    public int m_totalCount;

    // 小于指定数值的数量
    public int m_condCount;

    public void reset()
    {
        m_totalCount = 0;
        m_condCount = 0;
    }
}

// 查询账号金币数量少于指定的值
public class QueryAccountCoinLessValue : QueryBase
{
    private ResultAccountCoinLessValue m_result = new ResultAccountCoinLessValue();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamQuery p = (ParamQuery)param;
        if (p.m_time == "")
            return OpRes.op_res_time_format_error;

        int val = 0;
        if (!int.TryParse(p.m_param, out val))
            return OpRes.op_res_param_not_valid;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        m_result.reset();

        List<IMongoQuery> queryList = new List<IMongoQuery>();

        IMongoQuery imq1 = Query.LT("create_time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("create_time", BsonValue.Create(mint));
        IMongoQuery imqTime = Query.And(imq1, imq2);
        queryList.Add(imqTime);
        queryList.Add(Query.LT("gold", BsonValue.Create(val)));

        IMongoQuery imqGold = Query.And(queryList);

        return query(user, imqTime, imqGold);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imqTime, IMongoQuery imqGold)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        m_result.m_totalCount = (int)DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imqTime, dip);
        m_result.m_condCount = (int)DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imqGold, dip);
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//破产统计
public class StatBankruptItem 
{
    public string m_time;
    public int m_roomId;
    public int m_todayActivePlayerCheck;//活跃人数
    public int m_benifitPlayerCheck;//破产人数
    public int m_payAfterBenifitPlayerCheck;//破产付费人数
    public int m_todayBenifitTimes;//破产次数
    public int m_todayPayAfterBenifitTimes;//破产后充值次数

    public int m_flag = 0;

    public string getRate(int key1, int key2) 
    {
        string rate = "";
        if (key2 == 0)
        {
            rate = key1.ToString();
        }
        else {
            rate = Math.Round(key1*1.0/key2, 2).ToString();
        }

        return rate;
    }

    public string getPaoName(int key) 
    {
        string PaoName = "全部";
        var turret = Fish_TurretLevelCFG.getInstance().getValue(key);
        if (turret != null)
        {
            PaoName = turret.m_openRate + "炮";
        }

        return PaoName;
    }

    public string getDetail(int key)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_OPERATION_PLAYER_BANKRUPT_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("lotteryId", key.ToString());
        return Tool.genHyperlink(uParam);
    }
}
public class QueryStatPlayerBankrupt : QueryBase 
{
    private List<StatBankruptItem> m_result = new List<StatBankruptItem>();
    public StatBankruptItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag = 1;
                return d;
            }
        }

        StatBankruptItem item = new StatBankruptItem();
        m_result.Add(item);
        item.m_time = time;
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //时间选择
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //场次选择
        if (p.m_op != 0)
            imq = Query.And(imq, Query.EQ("turret_level", p.m_op));

        return query(user, imq, p);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("turret_level");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();

            StatBankruptItem tmp = IsCreate(time);

            tmp.m_todayBenifitTimes++; //破产次数

            if (tmp.m_flag == 0) 
            {
                /////////////////////////////////////////////////
                IMongoQuery imq1 = Query.EQ("time", t.Date);
                if (param.m_op != 0)
                    imq1 = Query.And(imq1, Query.EQ("turret_level", param.m_op));
                
                //活跃人数
                tmp.m_todayActivePlayerCheck = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, dip, "playerId", imq1);

                ////////////////////////////////////////////////
                DateTime mint = Convert.ToDateTime(t.Date);
                DateTime maxt = mint.AddDays(1);
                IMongoQuery imq2 = Query.And(
                    Query.LT("genTime", BsonValue.Create(maxt)),
                    Query.GTE("genTime", BsonValue.Create(mint))
                );

                if (param.m_op != 0)
                    imq2 = Query.And(imq2, Query.EQ("turret_level", param.m_op));

                //破产付费人数
                tmp.m_payAfterBenifitPlayerCheck = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_PLAYER_PAY_AFTER_BROKEN, dip, "playerId", imq2);
                //破产付费次数
                long todayPayAfterBenifitTimes = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_PAY_AFTER_BROKEN, imq2, dip);
                tmp.m_todayPayAfterBenifitTimes = Convert.ToInt32(todayPayAfterBenifitTimes);

                //////////////////////////////////////////////////////////////////////////////////
                //破产人数
                tmp.m_benifitPlayerCheck = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, dip, "playerId", imq2);
            }
        }

        return OpRes.opres_success;
    }
}
//破产统计详情
public class StatBankruptDetailItem 
{
    public string m_time;
    public int m_playerId;
    public int m_turret;
    public int m_roomId;

    public string getTurretName() 
    {
        string turretName = m_turret.ToString();
        Fish_LevelCFGData turret = Fish_TurretLevelCFG.getInstance().getValue(m_turret);
        if (turret != null)
            turretName = turret.m_openRate.ToString();

        return turretName;
    }
}
public class QueryStatPlayerBankruptDetail : QueryBase 
{
    private List<StatBankruptDetailItem> m_result = new List<StatBankruptDetailItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //时间选择
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //场次选择
        if (p.m_op != 0)
            imq = Query.And(imq, Query.EQ("turret_level", p.m_op));

        return query(user, imq, p);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("turret_level");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, dip, imq, 
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            StatBankruptDetailItem tmp = new StatBankruptDetailItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("turret_level"))
                tmp.m_turret = Convert.ToInt32(data["turret_level"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("roomid"))
                tmp.m_roomId = Convert.ToInt32(data["roomid"]);
        }

        return OpRes.opres_success;
    }
}
//破产炮倍详情
public class StatOpenRateBankruptListItem 
{
    public string m_time;
    public int m_playerId;
    public int m_turretId;
    public int m_colorValue;
    public int m_energyValue;
    public int m_dimensityValue;
    public int m_torpedoValue;
    public int m_chipValue;
    public int m_bankruptCount;

    public int m_flag = 0;

    public string getTurretName()
    {
        string turretName = m_turretId.ToString();
        Fish_LevelCFGData turret = Fish_TurretLevelCFG.getInstance().getValue(m_turretId);
        if (turret != null)
            turretName = turret.m_openRate.ToString();

        return turretName;
    }
}
public class QueryStatPlayerOpenRateBankruptList : QueryBase 
{
    private List<StatOpenRateBankruptListItem> m_result = new List<StatOpenRateBankruptListItem>();

    public StatOpenRateBankruptListItem IsCreate(string time, int playerId, int turretId)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time && d.m_playerId == playerId && d.m_turretId == turretId)
            {
                d.m_flag++;
                return d;
            }
        }

        StatOpenRateBankruptListItem item = new StatOpenRateBankruptListItem();
        m_result.Add(item);
        item.m_time = time;
        item.m_playerId = playerId;
        item.m_turretId = turretId;
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //时间选择
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //炮倍 为空则为全部炮倍
        if (!string.IsNullOrEmpty(p.m_param))
        {
            IMongoQuery imq3 = null;

            List<int> range = new List<int>();
            int turretId = 0;
            if (int.TryParse(p.m_param.Trim(), out turretId))
            {
                range.Add(turretId);
                range.Add(turretId);
            }
            else
            {
                Tool.parseNumList(p.m_param, range);
            }

            if (range[0] > 10000 || range[1] > 10000 || range[0] < 1 || range[1] < 1 || range[0] > range[1])
                return OpRes.op_res_param_not_valid;

            var da1 = Fish_TurretOpenRateCFG.getInstance().getValue(range[0]);
            var da2 = Fish_TurretOpenRateCFG.getInstance().getValue(range[1]);

            if (da1 == null || da2 == null)
                return OpRes.op_res_param_not_valid;

            range[0] = da1.m_level;
            range[1] = da2.m_level;

            imq3 = Query.And(Query.GTE("turret_level", range[0]), Query.LTE("turret_level", range[1]));

            imq = Query.And(imq, imq3);
        }

        //玩家ID
        if (!string.IsNullOrEmpty(p.m_playerId))
        {
            int playerId = 0;
            if (!int.TryParse(p.m_playerId, out playerId))
                return OpRes.op_res_param_not_valid;

            imq = Query.And(imq, Query.EQ("playerId", playerId));
        }

        return query(user, imq, p);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("turret_level").Ascending("playerId");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();

            if (!data.ContainsKey("playerId") || !data.ContainsKey("turret_level"))
                continue;
            
            int playerId = Convert.ToInt32(data["playerId"]);
            int turretId = Convert.ToInt32(data["turret_level"]);

            StatOpenRateBankruptListItem tmp = IsCreate(time, playerId, turretId);

            if (data.ContainsKey("colorValue"))
                tmp.m_colorValue += Convert.ToInt32(data["colorValue"]);

            if (data.ContainsKey("energyValue"))
                tmp.m_energyValue += Convert.ToInt32(data["energyValue"]);

            if (data.ContainsKey("dimensityValue"))
                tmp.m_dimensityValue += Convert.ToInt32(data["dimensityValue"]);

            if (data.ContainsKey("torpedoValue"))
                tmp.m_torpedoValue += Convert.ToInt32(data["torpedoValue"]);

            if (data.ContainsKey("chipValue"))
                tmp.m_chipValue += Convert.ToInt32(data["chipValue"]);

            //该玩家当日破产次数
            if (tmp.m_flag != 0)
                continue;
            DateTime t_max = t.Date.AddDays(1);
            IMongoQuery imq_broken = Query.And(
                                        Query.GTE("genTime", t.Date),
                                        Query.LT("genTime", t_max),
                                        Query.EQ("playerId", tmp.m_playerId));

            long count = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, imq_broken, dip);
            tmp.m_bankruptCount = Convert.ToInt32(count);
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//炮数成长分布
public class PlayerActTurretItem 
{
    public string m_time;
    public Dictionary<int, int[]> m_data = new Dictionary<int, int[]>();
}
public class QueryOperationPlayerActTurret : QueryBase 
{
    private List<PlayerActTurretItem> m_result = new List<PlayerActTurretItem>();

    public PlayerActTurretItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        PlayerActTurretItem item = new PlayerActTurretItem();
        m_result.Add(item);
        item.m_time = time;

        item.m_data.Add(1, new int[43]);
        item.m_data.Add(2, new int[43]);
        item.m_data.Add(3, new int[43]);
        item.m_data.Add(4, new int[43]);
        item.m_data.Add(5, new int[43]);
        item.m_data.Add(6, new int[43]);
        item.m_data.Add(7, new int[43]);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //时间选择
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("createTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("createTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //等级
        IMongoQuery imq3 = Query.And(Query.GTE("turret_level", 1), Query.LTE("turret_level", 43));
        //day
        IMongoQuery imq4 = Query.And(Query.GTE("day", 0), Query.LTE("day", 6));

        imq = Query.And(imq, imq3, imq4);
        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("createTime").Ascending("day").Ascending("turret_level");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string t = Convert.ToDateTime(data["createTime"]).ToLocalTime().ToShortDateString();
            PlayerActTurretItem tmp = IsCreate(t);

            if (data.ContainsKey("day") && data.ContainsKey("turret_level"))
            {
                int key = Convert.ToInt32(data["day"]) + 1;
                int level = Convert.ToInt32(data["turret_level"])-1;

                tmp.m_data[key][level]++;
            }
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//玩家炮数成长分布
public class PlayerActTurretBySingleItem 
{
    public string m_time;
    public string m_createTime;
    public int m_playerId;
    public int m_turret;
    public int m_loginDays;
    public long m_playTime;
    public long m_gold;

    public int m_totalRecharged;

    //次留 三留 七留
    public int m_remain2;
    public int m_remain3;
    public int m_remain7;

    //将等级转换为对应炮倍
    public string getTurretName()
    {
        string turretName = m_turret.ToString();
        Fish_LevelCFGData turret = Fish_TurretLevelCFG.getInstance().getValue(m_turret);
        if (turret != null)
            turretName = turret.m_openRate.ToString();

        return turretName;
    }

    //将秒转换为时分秒
    public string transTimeTohms(long avgTime)
    {
        string time_str = "";
        int h = 0, m = 0, s = 0;
        if (avgTime >= 3600)
        {
            h = Convert.ToInt32(avgTime / 3600);
            m = Convert.ToInt32((avgTime % 3600) / 60);
            s = Convert.ToInt32(avgTime % 3600 % 60);
            time_str = h + "h " + m + "m " + s + "s";
        }
        else if (avgTime >= 60)
        {
            m = Convert.ToInt32(avgTime / 60);
            s = Convert.ToInt32(avgTime % 60);
            time_str = m + "m " + s + "s";
        }
        else
        {
            time_str = avgTime + "s";
        }

        return time_str;
    }
}
public class QueryOperationPlayerActTurretBySingle : QueryBase 
{
    private List<PlayerActTurretBySingleItem> m_result = new List<PlayerActTurretBySingleItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //时间选择
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //炮倍 为空则为全部炮倍
        if (!string.IsNullOrEmpty(p.m_param)) 
        {
            IMongoQuery imq3 = null;

            List<int> range = new List<int>();
            int turretId = 0;
            if (int.TryParse(p.m_param.Trim(), out turretId))
            {
                range.Add(turretId);
                range.Add(turretId);
            }else
            {
                Tool.parseNumList(p.m_param, range);
            }

            if (range[0] > 10000 || range[1] > 10000 || range[0] < 1 || range[1] < 1 || range[0] > range[1])
                return OpRes.op_res_param_not_valid;

            var da1 = Fish_TurretOpenRateCFG.getInstance().getValue(range[0]);
            var da2 = Fish_TurretOpenRateCFG.getInstance().getValue(range[1]);
              
            if (da1 == null || da2 == null)
               return OpRes.op_res_param_not_valid;

            range[0] = da1.m_level;
            range[1] = da2.m_level;

            imq3 = Query.And(Query.GTE("turret_level", range[0]), Query.LTE("turret_level", range[1]));

            imq = Query.And(imq, imq3);
        }

        //day
        IMongoQuery imq4 = null;
        if (p.m_op > 0)
        {
            imq4 = Query.EQ("day", p.m_op - 1);
        }
        else {
            imq4 = Query.LTE("day", 6);
        }

        imq = Query.And(imq, imq4);
        return query(user, imq, p);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("time").Descending("createTime").Ascending("day").Ascending("turret_level");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, dip, imq, 
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            PlayerActTurretBySingleItem tmp = new PlayerActTurretBySingleItem();
            m_result.Add(tmp);

            if (data.ContainsKey("createTime"))
                tmp.m_createTime = Convert.ToDateTime(data["createTime"]).ToLocalTime().ToString();

            tmp.m_time = Convert.ToDateTime(data["time"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("turret_level"))
                tmp.m_turret = Convert.ToInt32(data["turret_level"]);

            if (data.ContainsKey("loginDay"))
                tmp.m_loginDays = Convert.ToInt32(data["loginDay"]);

            if (data.ContainsKey("todayPlayTime"))
                tmp.m_playTime = Convert.ToInt64(data["todayPlayTime"]);

            if (data.ContainsKey("gold"))
                tmp.m_gold = Convert.ToInt64(data["gold"]);

            //次留 三留 七留  1 2 6
            IMongoQuery q1 = Query.And(Query.EQ("playerId", tmp.m_playerId), Query.EQ("day", 1));
            long count1 = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, q1, dip);
            if (count1 > 0)
                tmp.m_remain2 = 1;

            IMongoQuery q2 = Query.And(Query.EQ("playerId", tmp.m_playerId), Query.EQ("day", 2));
            long count2 = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, q2, dip);
            if (count2 > 0)
                tmp.m_remain3 = 1;

            IMongoQuery q6 = Query.And(Query.EQ("playerId", tmp.m_playerId), Query.EQ("day", 6));
            long count6 = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, q6, dip);
            if (count6 > 0)
                tmp.m_remain7 = 1;

            //总充值金额
            IMongoQuery q4 = Query.EQ("player_id", tmp.m_playerId);
            Dictionary<string, object> playerRecharge =
                DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, user.getDbServerID(), DbName.DB_PLAYER, q4, new string[] { "player_id", "recharged"});
            if (playerRecharge != null && playerRecharge.ContainsKey("recharged"))
                tmp.m_totalRecharged = Convert.ToInt32(playerRecharge["recharged"]);
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//彩券鱼统计
public class LotteryExchangeItem 
{
    public string m_time;
    public int m_roomId;
    public long m_ticketFishIncome;
    public long m_ticketFishOutlay;
    public long m_ticketOutlay;

    public string getRoomName() 
    {
        string roomName = "";
        switch(m_roomId)
        {
            case 1: roomName = "初级场"; break;
            case 2: roomName = "中级场"; break;
            case 3: roomName = "高级场"; break;
        }
        return roomName;
    }
}
public class QueryStatFishlordLotteryExchange : QueryBase 
{
    private List<LotteryExchangeItem> m_result = new List<LotteryExchangeItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //时间选择
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("Date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("Date", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //场次选择
        if (p.m_op != 0)
        {
            imq = Query.And(imq, Query.EQ("ROOMID", p.m_op));
        }
        else {
            imq = Query.And(imq, Query.GTE("ROOMID",1), Query.LTE("ROOMID",2));
        }

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("Date").Ascending("ROOMID");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.PUMP_FISHLORD_EVERY_DAY, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        LotteryExchangeItem tmp1 = new LotteryExchangeItem();
        m_result.Add(tmp1);
        tmp1.m_time = "总计";

         for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            LotteryExchangeItem tmp = new LotteryExchangeItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["Date"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("ROOMID"))
                tmp.m_roomId = Convert.ToInt32(data["ROOMID"]);

            if (data.ContainsKey("TicketFishIncome"))
                tmp.m_ticketFishIncome = Convert.ToInt64(data["TicketFishIncome"]);

            if (data.ContainsKey("TicketFishOutLay"))
                tmp.m_ticketFishOutlay = Convert.ToInt64(data["TicketFishOutLay"]);

            if (data.ContainsKey("TicketOutLay"))
                tmp.m_ticketOutlay = Convert.ToInt64(data["TicketOutLay"]);

            tmp1.m_ticketFishIncome += tmp.m_ticketFishIncome;
            tmp1.m_ticketFishOutlay += tmp.m_ticketFishOutlay;
            tmp1.m_ticketOutlay += tmp.m_ticketOutlay;
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//每日周任务统计
public class StatDailyWeekTaskItem 
{
    public string m_time;
    public Dictionary<int, int[]> m_data = new Dictionary<int, int[]>();//任务ID  完成人数 系统支出
}
public class QueryStatDailyWeekTask : QueryBase 
{
    private List<StatDailyWeekTaskItem> m_result = new List<StatDailyWeekTaskItem>();
    public StatDailyWeekTaskItem IsCreate(string time, int taskId) 
    {
        foreach (var d in m_result) 
        {
            if (d.m_time == time)
                return d;
        }

        StatDailyWeekTaskItem item = new StatDailyWeekTaskItem();
        m_result.Add(item);
        item.m_time = time;
        item.m_data.Add(taskId, new int[] { 0, 0});
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        if (p.m_op == 0) //每日
            return query(user, imq, TableName.STAT_PUMP_DAILY_TASK);
        if (p.m_op == 1) //每周
            return query(user, imq, TableName.STAT_PUMP_WEEKLY_TASK);

        return OpRes.op_res_failed;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, string table)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("taskId");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(table, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            int taskId = Convert.ToInt32(data["taskId"]);

            StatDailyWeekTaskItem tmp = IsCreate(time, taskId);

            int finishPersonCount = 0;
            if (data.ContainsKey("completeCount"))
                finishPersonCount = Convert.ToInt32(data["completeCount"]);

            int outlay = 0;
            if (data.ContainsKey("outlay"))
                outlay = Convert.ToInt32(data["outlay"]);

            tmp.m_data[taskId] = new int[] { finishPersonCount, outlay };
        }

        return OpRes.opres_success;
    }
}

//每日周奖励统计
public class StatDailyWeekRewardItem 
{
    public string m_time;
    public int m_gear1Count;
    public int m_gear2Count;
    public int m_gear3Count;
    public int m_gear4Count;
    public int m_sysOutlay;
}
public class QueryStatDailyWeekReward : QueryBase 
{
    private List<StatDailyWeekRewardItem> m_result = new List<StatDailyWeekRewardItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        if (p.m_op == 0) //每日
            return query(user, imq, TableName.STAT_PUMP_DAILY_REWARD);
        if (p.m_op == 1) //每周
            return query(user, imq, TableName.STAT_PUMP_WEEK_REWARD);

        return OpRes.op_res_failed;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, string table)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(table, dip, imq, 0, 0, null,"genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            StatDailyWeekRewardItem tmp = new StatDailyWeekRewardItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if (data.ContainsKey("gear1count"))
                tmp.m_gear1Count = Convert.ToInt32(data["gear1count"]);

            if (data.ContainsKey("gear2count"))
                tmp.m_gear2Count = Convert.ToInt32(data["gear2count"]);

            if (data.ContainsKey("gear3count"))
                tmp.m_gear3Count = Convert.ToInt32(data["gear3count"]);

            if (data.ContainsKey("gear4count"))
                tmp.m_gear4Count = Convert.ToInt32(data["gear4count"]);

            if (data.ContainsKey("systemOutlay"))
                tmp.m_sysOutlay = Convert.ToInt32(data["systemOutlay"]);
        }

        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//主线任务
public class StatMainlyTaskItem
{
    public string m_time;
    public int m_activeCount;
    public Dictionary<int, int> m_taskComplete = new Dictionary<int, int>();
    public string getRate(int count)
    {
        if (m_activeCount == 0)
            return "-";

        double factGain = (double)(count * 100) / m_activeCount;
        return Math.Round(factGain)+"%";
    }
}
public class QueryStatMainlyTask : QueryBase 
{
    private List<StatMainlyTaskItem> m_result = new List<StatMainlyTaskItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(
              TableName.STAT_PUMP_MAINLY_TASK, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        int i = 0, j = 0;

        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            StatMainlyTaskItem tmp = new StatMainlyTaskItem();
            m_result.Add(tmp);

            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = t.ToShortDateString();

            for (j = 1; j <= 63;j++ ) 
            {
                string tk = "taskid_"+ j;
                int complete_da = 0;
                if (data.ContainsKey(tk))
                    complete_da = Convert.ToInt32(data[tk]);

                tmp.m_taskComplete.Add(j, complete_da);
            }

            //100炮及以上活跃人数
            IMongoQuery imq_1 = Query.EQ("time", t.Date);
            IMongoQuery imq_2 = Query.GTE("turret_level", 12);
            IMongoQuery imq_3 = Query.And(imq_1, imq_2);
            tmp.m_activeCount = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, dip, "playerId", imq_3);
        }

        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////
//幸运抽奖
public class StatGoldFishLotteryItem 
{
    public string m_time;
    public int m_gear;
    public int m_incomeGold;
    public int m_outlayGold;
    public int m_gear1Count;
    public int m_gear2Count;
    public int m_gear3Count;
    public int m_gear4Count;
    public int m_gear5Count;
    public int m_gear6Count;
    public int m_totalCount;
    public int m_totalPerson;

    public string getGearName() 
    {
        string gearName = "";
        switch(m_gear)
        {
            case 1: gearName = "普通抽奖"; break;
            case 2: gearName = "青铜抽奖"; break;
            case 3: gearName = "白银抽奖"; break;
            case 4: gearName = "黄金抽奖"; break;
            case 5: gearName = "钻石抽奖"; break;
            case 6: gearName = "至尊抽奖"; break;
        }
        return gearName;
    }

    public string getRate() 
    {
        if (m_incomeGold == 0) {
            return "-";
        }
        double factGain = (double)(m_incomeGold - m_outlayGold) / m_incomeGold;
        return Math.Round(factGain, 3).ToString();
    }

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_STAT_GOLD_FISH_LOTTERY_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("boxId", m_gear.ToString());
        return Tool.genHyperlink(uParam);
    }
}
public class QueryStatGoldFishLottery : QueryBase 
{
    private List<StatGoldFishLotteryItem> m_result = new List<StatGoldFishLotteryItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("date", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("gear", p.m_op));

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("date").Ascending("gear");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_PUMP_GOLD_FISH_LOTTERY, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            StatGoldFishLotteryItem tmp = new StatGoldFishLotteryItem();
            m_result.Add(tmp);

            DateTime t = Convert.ToDateTime(data["date"]).ToLocalTime();
            tmp.m_time = t.ToShortDateString();

            tmp.m_gear = Convert.ToInt32(data["gear"]);

            if (data.ContainsKey("gear1count"))
                tmp.m_gear1Count = Convert.ToInt32(data["gear1count"]);

            if (data.ContainsKey("gear2count"))
                tmp.m_gear2Count = Convert.ToInt32(data["gear2count"]);

            if (data.ContainsKey("gear3count"))
                tmp.m_gear3Count = Convert.ToInt32(data["gear3count"]);

            if (data.ContainsKey("gear4count"))
                tmp.m_gear4Count = Convert.ToInt32(data["gear4count"]);

            if (data.ContainsKey("gear5count"))
                tmp.m_gear5Count = Convert.ToInt32(data["gear5count"]);

            if (data.ContainsKey("gear6count"))
                tmp.m_gear6Count = Convert.ToInt32(data["gear6count"]);

            if (data.ContainsKey("income_gold"))
                tmp.m_incomeGold = Convert.ToInt32(data["income_gold"]);

            if (data.ContainsKey("outlay_gold"))
                tmp.m_outlayGold = Convert.ToInt32(data["outlay_gold"]);

            //总的抽奖次数
            tmp.m_totalCount = tmp.m_gear1Count + tmp.m_gear2Count + tmp.m_gear3Count + 
                tmp.m_gear4Count + tmp.m_gear5Count + tmp.m_gear6Count;

            //总的抽奖人数
            IMongoQuery imq_1 = Query.EQ("date", t.Date);
            IMongoQuery imq_2 = Query.EQ("gear", tmp.m_gear);
            IMongoQuery imq_3 = Query.And(imq_1, imq_2);
            tmp.m_totalPerson = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_GOLD_FISH_LOTTERY_PLAYER, dip, "playerId", imq_3);
        }

        return OpRes.opres_success;
    }
}
//幸运抽奖详情
public class StatGoldFishLotteryDetailItem 
{
    public string m_time;
    public int m_playerId;
    public int m_gearId;
    public int m_gear1Count;
    public int m_gear2Count;
    public int m_gear3Count;
    public int m_gear4Count;
    public int m_gear5Count;
    public int m_gear6Count;

    public string getGearName()
    {
        string gearName = "";
        switch (m_gearId)
        {
            case 1: gearName = "普通抽奖"; break;
            case 2: gearName = "青铜抽奖"; break;
            case 3: gearName = "白银抽奖"; break;
            case 4: gearName = "黄金抽奖"; break;
            case 5: gearName = "钻石抽奖"; break;
            case 6: gearName = "至尊抽奖"; break;
        }
        return gearName;
    }

    public string getReward(int key)
    {
        string rewardItem = "";
        int gearCount = 0;
        switch(key){
            case 1:gearCount = m_gear1Count;break;
            case 2:gearCount = m_gear2Count;break;
            case 3:gearCount = m_gear3Count;break;
            case 4:gearCount = m_gear4Count;break;
            case 5:gearCount = m_gear5Count;break;
            case 6:gearCount = m_gear6Count;break;
        }

        int index = 6 * (m_gearId -1) + key;

        LuckyDrawInfo luckyData = LuckyDrawCFG.getInstance().getValue(index);
        if(luckyData != null)
        {
            int itemId = luckyData.m_itemId;
            int itemCount = luckyData.m_itemCount;

            var itemInfo = ItemCFG.getInstance().getValue(itemId);
            if(itemInfo!=null){
                rewardItem = gearCount*itemCount + itemInfo.m_itemName ;
            }
        }
        return rewardItem;
    }

}
public class QueryStatGoldFishLotteryDetail : QueryBase 
{
    private List<StatGoldFishLotteryDetailItem> m_result = new List<StatGoldFishLotteryDetailItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("date", BsonValue.Create(mint));

        IMongoQuery imq3 = Query.EQ("gear", p.m_op);
        IMongoQuery imq = Query.And(imq1, imq2, imq3);

        return query(user, imq, p);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_GOLD_FISH_LOTTERY_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(
             TableName.STAT_PUMP_GOLD_FISH_LOTTERY_PLAYER, dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "playerId");

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            StatGoldFishLotteryDetailItem tmp = new StatGoldFishLotteryDetailItem();
            m_result.Add(tmp);

            DateTime t = Convert.ToDateTime(data["date"]).ToLocalTime();
            tmp.m_time = t.ToShortDateString();

            tmp.m_gearId = Convert.ToInt32(data["gear"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("gear1count"))
                tmp.m_gear1Count = Convert.ToInt32(data["gear1count"]);

            if (data.ContainsKey("gear2count"))
                tmp.m_gear2Count = Convert.ToInt32(data["gear2count"]);

            if (data.ContainsKey("gear3count"))
                tmp.m_gear3Count = Convert.ToInt32(data["gear3count"]);

            if (data.ContainsKey("gear4count"))
                tmp.m_gear4Count = Convert.ToInt32(data["gear4count"]);

            if (data.ContainsKey("gear5count"))
                tmp.m_gear5Count = Convert.ToInt32(data["gear5count"]);

            if (data.ContainsKey("gear6count"))
                tmp.m_gear6Count = Convert.ToInt32(data["gear6count"]);
        }

        return OpRes.opres_success;
    }
}

//幸运抽奖总计
public class StatGoldFishLotteryTotalItem 
{
    public string m_time;

    public int m_gear1Count;
    public int m_gear2Count;
    public int m_gear3Count;
    public int m_gear4Count;
    public int m_gear5Count;
    public int m_gear6Count;

    public int m_lotteryCount;
    public int m_lotteryPerson;

    public long m_incomeGold;
    public long m_outlayGold;

    public int m_flag = 0;
    public string getRate()
    {
        if (m_incomeGold == 0)
        {
            return "-";
        }
        double factGain = (double)(m_incomeGold - m_outlayGold) / m_incomeGold;
        return Math.Round(factGain, 3).ToString();
    }
}
public class QueryStatGoldFishLotteryTotal : QueryBase 
{
    private List<StatGoldFishLotteryTotalItem> m_result = new List<StatGoldFishLotteryTotalItem>();
    public StatGoldFishLotteryTotalItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag++;
                return d;
            }
        }

        StatGoldFishLotteryTotalItem item = new StatGoldFishLotteryTotalItem();
        m_result.Add(item);
        item.m_time = time;
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("date", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("date").Ascending("gear");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_PUMP_GOLD_FISH_LOTTERY, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime t = Convert.ToDateTime(data["date"]).ToLocalTime();
            string time = t.ToShortDateString();

            StatGoldFishLotteryTotalItem tmp = IsCreate(time);

            if (data.ContainsKey("gear1count"))
                tmp.m_gear1Count += Convert.ToInt32(data["gear1count"]);

            if (data.ContainsKey("gear2count"))
                tmp.m_gear2Count += Convert.ToInt32(data["gear2count"]);

            if (data.ContainsKey("gear3count"))
                tmp.m_gear3Count += Convert.ToInt32(data["gear3count"]);

            if (data.ContainsKey("gear4count"))
                tmp.m_gear4Count += Convert.ToInt32(data["gear4count"]);

            if (data.ContainsKey("gear5count"))
                tmp.m_gear5Count += Convert.ToInt32(data["gear5count"]);

            if (data.ContainsKey("gear6count"))
                tmp.m_gear6Count += Convert.ToInt32(data["gear6count"]);

            if (data.ContainsKey("income_gold"))
                tmp.m_incomeGold += Convert.ToInt64(data["income_gold"]);

            if (data.ContainsKey("outlay_gold"))
                tmp.m_outlayGold += Convert.ToInt64(data["outlay_gold"]);

            if (tmp.m_flag > 0)
                continue;

            //总的抽奖人数
            IMongoQuery imq_1 = Query.EQ("date", t.Date);
            tmp.m_lotteryPerson = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_GOLD_FISH_LOTTERY_PLAYER, dip, "playerId", imq_1);
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
public class CFishItem
{
    public int m_itemId;    // 道具ID
    public long m_buyCount;  // 购买个数(购买后即使用)
    public long m_useCount;  // 使用个数(不花钱的使用次数)

    public string getItemName()
    {
        Fish_ItemCFGData data = Fish_ItemCFG.getInstance().getValue(m_itemId);

        if (data != null)
            return data.m_itemName;

        return "";
    }
}

public class RooomItemConsume
{
    public Dictionary<int, CFishItem> m_dic = new Dictionary<int, CFishItem>();

    public void addItem(int itemId, int moneyType, long value)
    {
        CFishItem tmp = null;
        if (m_dic.ContainsKey(itemId))
        {
            tmp = m_dic[itemId];
        }
        else
        {
            tmp = new CFishItem();
            tmp.m_itemId = itemId;
            m_dic.Add(itemId, tmp);
        }

        /*if (moneyType > 0) // 这是通过钻石购买的
        {
            tmp.m_buyCount += value;
        }
        else
        {*/
            tmp.m_useCount += value; // 直接使用的
       // }
    }

    public void endOp()
    {
        foreach (var d in m_dic.Values)
        {
          //  d.m_useCount -= d.m_buyCount;
        }
    }

    public CFishItem getIem(int itemId)
    {
        if (m_dic.ContainsKey(itemId))
            return m_dic[itemId];

        return null;
    }
}

public class ResultConsumeItem
{
    public Dictionary<string, RooomItemConsume> m_dic = new Dictionary<string, RooomItemConsume>();
    private List<DateTime> m_list = new List<DateTime>();

    public void addItem(DateTime time, int roomId, int itemId, int moneyType, long value)
    {
        string key = time.ToString() + "_" + roomId;
        RooomItemConsume ric = null;
        if (m_dic.ContainsKey(key))
        {
            ric = m_dic[key];
        }
        else
        {
            ric = new RooomItemConsume();
            m_dic.Add(key, ric);
        }

        ric.addItem(itemId, moneyType, value);

        if (!m_list.Contains(time))
        {
            m_list.Add(time);
        }
    }

    public void endOp()
    {
        foreach (var d in m_dic.Values)
        {
            d.endOp();
        }
    }

    public void reset()
    {
        m_dic.Clear();
        m_list.Clear();
    }

    public RooomItemConsume getRooomItemConsume(DateTime t, int roomId)
    {
        string key = t.ToString() + "_" + roomId;
        if (m_dic.ContainsKey(key))
            return m_dic[key];

        return null;
    }

    public List<DateTime> timeList
    {
        get { return m_list; }
    }
}

// 经典捕鱼的消耗查询，只查消耗
public class QueryFishConsume : QueryBase
{
    private ResultTotalConsume m_result = new ResultTotalConsume();
    private ResultConsumeItem m_result1 = new ResultConsumeItem();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamTotalConsume p = (ParamTotalConsume)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
        queryList.Add(Query.And(imq1, imq2));

        if (p.m_currencyType != 3) 
        {
            queryList.Add(Query.EQ("moneyType", BsonValue.Create(p.m_currencyType)));
        }

        IMongoQuery imq = Query.And(queryList);

        if (p.m_currencyType == 3) //物品
        {
            return queryItem(user, imq, p.m_roomType);
        }

        return query(user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user)
    {
        return m_result1;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.FISH_CONSUME, dip, imq, 0, 0, null, "time", true);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < data.Count; i++)
        {
            DateTime t = Convert.ToDateTime(data[i]["time"]).ToLocalTime();
            TotalConsumeItem item = m_result.create(t);

            int reason = Convert.ToInt32(data[i]["consumeType"]);
            long val = Convert.ToInt64(data[i]["value"]);
            item.add(reason, val);

            m_result.addReason(reason);
        }

        return OpRes.opres_success;
    }

    // 查询购买消耗的道具。锁定、急速、散射
    //冰冻、锁定、神灯、狂暴、普通鱼雷、青铜鱼雷、白银鱼雷、黄金鱼雷、钻石鱼雷
    private OpRes queryItem(GMUser user, IMongoQuery imq,string rtype)
    {
        m_result1.reset();

         IMongoQuery imq1 = Query.Or(Query.EQ("roomId", BsonValue.Create(1)), 
                           Query.EQ("roomId", BsonValue.Create(2)),
                           Query.EQ("roomId", BsonValue.Create(3)),
                           Query.EQ("roomId", BsonValue.Create(4)),
                           Query.EQ("roomId", BsonValue.Create(5)),
                           Query.EQ("roomId", BsonValue.Create(6))
                           );

        imq = Query.And(imq, imq1);

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(TableName.FISH_CONSUME_ITEM, dip, imq, 0, 0, null, "time", false);

        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < data.Count; i++)
        {
            DateTime t = Convert.ToDateTime(data[i]["time"]).ToLocalTime();
            int itemId = Convert.ToInt32(data[i]["itemId"]);
            int moneyType = Convert.ToInt32(data[i]["moneyType"]);
            int roomId = Convert.ToInt32(data[i]["roomId"]);
            long value = Convert.ToInt32(data[i]["value"]);
            if (rtype == "0")
            {
                m_result1.addItem(t, roomId, itemId, moneyType, value);
            }

            if(rtype==roomId.ToString())
            {
                m_result1.addItem(t,roomId,itemId,moneyType,value);
            }
        }
        m_result1.endOp();
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultPlayerBankerInfo
{
    public string m_genTime = "";
    public int m_playerId;
    public string m_nickName = "";
    public int m_bankerCount;
    public int m_beforeGold = 0;
    public int m_nowGold;
    public int m_resultValue;
    public int m_sysGet;

    // 爆庄支出
    public int m_sysLose;
}

// 百家乐上庄情况
public class QueryBaccaratPlayerBanker : QueryBase
{
    List<ResultPlayerBankerInfo> m_result = new List<ResultPlayerBankerInfo>();
    private QueryCondition m_cond = new QueryCondition();

    protected string m_tableName = "";

    public QueryBaccaratPlayerBanker()
    {
        m_tableName = TableName.PUMP_PLAYER_BANKER;
    }

    public override OpRes doQuery(object param, GMUser user)
    {                                                                    
        m_result.Clear();
        m_cond.startQuery();
        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        IMongoQuery imq = m_cond.getImq();
        ParamQuery p = (ParamQuery)param;
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQuery p = (ParamQuery)param;
        bool res = false;
        if (!string.IsNullOrEmpty(p.m_param))
        {
            int playerId = 0;
            res = int.TryParse(p.m_param, out playerId);
            if (!res)
                return OpRes.op_res_param_not_valid;

            queryCond.addQueryCond("playerId", playerId);
        }
      
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        queryCond.addImq(Query.And(imq1, imq2));

        return OpRes.opres_success;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(m_tableName, imq, dip);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(m_tableName,
                                                     dip,
                                                     imq,
                                                     (param.m_curPage - 1) * param.m_countEachPage,
                                                     param.m_countEachPage);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultPlayerBankerInfo t = new ResultPlayerBankerInfo();
            m_result.Add(t);

            t.m_genTime = Convert.ToDateTime(dataList[i]["genTime"]).ToLocalTime().ToString();
            t.m_playerId = Convert.ToInt32(dataList[i]["playerId"]);
            t.m_nickName = Convert.ToString(dataList[i]["playerName"]);
            t.m_bankerCount = Convert.ToInt32(dataList[i]["bankerCount"]);
            t.m_beforeGold = Convert.ToInt32(dataList[i]["beforeGold"]);
            t.m_nowGold = Convert.ToInt32(dataList[i]["nowGold"]);
            t.m_resultValue = Convert.ToInt32(dataList[i]["resultValue"]);
            t.m_sysGet = Convert.ToInt32(dataList[i]["sysGet"]);

            if (dataList[i].ContainsKey("sysLose"))
            {
                t.m_sysLose = Convert.ToInt32(dataList[i]["sysLose"]);
            }
        }
        
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

// 牛牛上庄查询
public class QueryCowsPlayerBanker : QueryBaccaratPlayerBanker
{
    public QueryCowsPlayerBanker()
    {
        m_tableName = TableName.PUMP_PLAYER_BANKER_COWS;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultnformHeadItem
{
    public string m_time;
    public int m_informerId;
    public int m_dstPlayerId;
    public string m_headURL;
}

public class ParamInformHead
{
    public string m_playerList;
    public int m_opType;

    public ParamInformHead()
    {
        m_opType = 0;
    }

    public bool isView()
    {
        return m_opType == 0;
    }
}

// 举报头像查看
public class QueryInformHead : QueryBase
{
    private List<ResultnformHeadItem> m_result = new List<ResultnformHeadItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamInformHead p = (ParamInformHead)param;
        if (p.isView())
        {
            return _doQuery(user);
        }

        return _doDel(p, user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes _doQuery(GMUser user)
    {
        m_result.Clear();
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
            DBMgr.getInstance().executeQuery(TableName.INFORM_HEAD,dip, null, 0, 500);
        if (dataList == null)
            return OpRes.opres_success;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultnformHeadItem info = new ResultnformHeadItem();
            m_result.Add(info);

            Dictionary<string, object> data = dataList[i];
            info.m_time = Convert.ToDateTime(data["time"]).ToLocalTime().ToString();
            info.m_informerId = Convert.ToInt32(data["informerId"]);
            info.m_dstPlayerId = Convert.ToInt32(dataList[i]["destPlayerId"]);

            OpRes code = user.doQuery(info.m_dstPlayerId.ToString(), QueryType.queryTypePlayerHead);
            if (code == OpRes.opres_success)
            {
                info.m_headURL = (string)user.getQueryResult(QueryType.queryTypePlayerHead);
            }
            else
            {
                info.m_headURL = "";
            }
        }

        return OpRes.opres_success;
    }

    private OpRes _doDel(ParamInformHead param, GMUser user)
    {
        if (string.IsNullOrEmpty(param.m_playerList))
            return OpRes.op_res_param_not_valid;

        string[] arr = Tool.split(param.m_playerList, ',', StringSplitOptions.RemoveEmptyEntries);
        foreach (string id in arr)
        {
            DBMgr.getInstance().remove(TableName.INFORM_HEAD, "destPlayerId", Convert.ToInt32(id),
                user.getDbServerID(), DbName.DB_PLAYER);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultActivationItem : GameStatData
{
    public DateTime m_genTime;
    public string m_channel;

    public int m_oldFirstRechargePersonCount;

    // 微信公众号充值收入
    public int m_wchatPublicNumIncome;

    public string get2DayRemain()
    {
        return getRemainPercent(m_2DayRemainCount, m_regeditCount);
    }

    public string get3DayRemain()
    {
        return getRemainPercent(m_3DayRemainCount, m_regeditCount);
    }
    public string get7DayRemain()
    {
        return getRemainPercent(m_7DayRemainCount, m_regeditCount);
    }

    public string get30DayRemain()
    {
        return getRemainPercent(m_30DayRemainCount, m_regeditCount);
    }

    //////////////////////////////////////////////////////////////////////////
    public string get2DayDevRemain()
    {
        return getRemainPercent(m_2DayDevRemainCount, m_deviceActivationCount);
    }

    public string get3DayDevRemain()
    {
        return getRemainPercent(m_3DayDevRemainCount, m_deviceActivationCount);
    }
    public string get7DayDevRemain()
    {
        return getRemainPercent(m_7DayDevRemainCount, m_deviceActivationCount);
    }

    public string get30DayDevRemain()
    {
        return getRemainPercent(m_30DayDevRemainCount, m_deviceActivationCount);
    }

    public string getARPU()
    {
        if (m_activeCount == 0)
            return "0";

        double val = (double)m_totalIncome / m_activeCount;
        return Math.Round(val, 2).ToString();
    }

    public string getARPPU()
    {
        if (m_rechargePersonNum == 0)
            return "0";

        double val = (double)m_totalIncome / m_rechargePersonNum;
        return Math.Round(val, 2).ToString();
    }

    public string getNewARPPU()
    {
        if (m_newAccRechargePersonNum == 0)
            return "0";

        double val = (double)m_newAccIncome / m_newAccRechargePersonNum;
        return Math.Round(val, 2).ToString();
    }

    // 付费率=付费人数/注册人数
    public string getRechargeRate()
    {
        return getRemainPercent(m_rechargePersonNum, m_activeCount);
    }

    // 新增用户付费率
    public string getNewAccRechargeRate()
    {
        if (m_newAccRechargePersonNum == -1)
            return "";

        return getRemainPercent(m_newAccRechargePersonNum, m_regeditCount);
    }

    public string getAccNumberPerDev()
    {
        return ItemHelp.getRate(m_regeditCount, m_deviceActivationCount, 2);
    }

    public void reset()
    {
        m_genTime = DateTime.MinValue;
        m_channel = "";
        m_2DayDevRemainCount = m_3DayDevRemainCount = m_7DayDevRemainCount = m_30DayDevRemainCount = 0;
    }

    public string getRemainPercent(int up, int down)
    {
        if (down == 0)
            return "0%";

        if (up == -1)
        {
            return "暂无";
        }
        double val = (double)up / down * 100;
        return Math.Round(val, 2).ToString() + "%";
    }
}

// talking data活跃
public class QueryTdActivation: QueryBase
{
    private List<ResultActivationItem> m_result = new List<ResultActivationItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        int condCount = 0;

        if(p.m_param != "" && p.m_param != "-1" && p.m_param != "-2") //单渠道
            queryList.Add(Query.EQ("channel", BsonValue.Create(p.m_param)));

        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            condCount++;
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            queryList.Add(Query.And(imq1, imq2));
        }

        if (p.m_param == "-2")
        {
            var channelList = TdChannelExcept.getInstance().getAllData();
            if (channelList != null) 
            {
                foreach (var channel in channelList.Values) 
                {
                    queryList.Add(Query.NE("channel", channel.m_channelNo));
                }
            }
        }

        if (condCount == 0)
            return OpRes.op_res_need_at_least_one_cond;

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        OpRes code = query(p, imq, user);
        //总体的时候
        if ((p.m_param == "" || p.m_param == "-2") && code == OpRes.opres_success)
            sum();

        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.CHANNEL_TD, imq, serverId, DbName.DB_ACCOUNT);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList_1 = dataList.OrderByDescending(a => a["regeditCount"]).OrderByDescending(a => a["genTime"]).ToList();

        for (int i = 0; i < dataList_1.Count; i++)
        {
            Dictionary<string, object> data = dataList_1[i];
            ResultActivationItem tmp = new ResultActivationItem();
            m_result.Add(tmp);

            tmp.m_genTime = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_channel = Convert.ToString(data["channel"]).PadLeft(6, '0');
            
            tmp.m_regeditCount = Convert.ToInt32(data["regeditCount"]);
            tmp.m_deviceActivationCount = Convert.ToInt32(data["deviceActivationCount"]);
            tmp.m_activeCount = Convert.ToInt32(data["activeCount"]);
            if(data.ContainsKey("deviceLoginCount"))
                tmp.m_deviceLoginCount = Convert.ToInt32(data["deviceLoginCount"]);

            tmp.m_totalIncome = Convert.ToInt32(data["totalIncome"]);
            tmp.m_rechargePersonNum = Convert.ToInt32(data["rechargePersonNum"]);
            tmp.m_rechargeCount = Convert.ToInt32(data["rechargeCount"]);

            tmp.m_2DayRemainCount = Convert.ToInt32(data["2DayRemainCount"]);

            tmp.m_3DayRemainCount = Convert.ToInt32(data["3DayRemainCount"]);

            tmp.m_7DayRemainCount = Convert.ToInt32(data["7DayRemainCount"]);

            tmp.m_30DayRemainCount = Convert.ToInt32(data["30DayRemainCount"]);

            if (data.ContainsKey("Day2DevRemainCount"))
                tmp.m_2DayDevRemainCount = Convert.ToInt32(data["Day2DevRemainCount"]);

            if (data.ContainsKey("Day3DevRemainCount"))
                tmp.m_3DayDevRemainCount = Convert.ToInt32(data["Day3DevRemainCount"]);

            if (data.ContainsKey("Day7DevRemainCount"))
                tmp.m_7DayDevRemainCount = Convert.ToInt32(data["Day7DevRemainCount"]);

            if (data.ContainsKey("Day30DevRemainCount"))
                tmp.m_30DayDevRemainCount = Convert.ToInt32(data["Day30DevRemainCount"]);

            tmp.m_newAccIncome = -1;
            if (data.ContainsKey("newAccIncome"))
                tmp.m_newAccIncome = Convert.ToInt32(data["newAccIncome"]);

            tmp.m_newAccRechargePersonNum = -1;
            if (data.ContainsKey("newAccRechargePersonNum"))
                tmp.m_newAccRechargePersonNum = Convert.ToInt32(data["newAccRechargePersonNum"]);

            //付费次日留存
            if (data.ContainsKey("2DayRemainCountRecharge"))
                tmp.m_2DayRemainCountRecharge = Convert.ToInt32(data["2DayRemainCountRecharge"]);
            //付费3日留存
            if (data.ContainsKey("3DayRemainCountRecharge"))
                tmp.m_3DayRemainCountRecharge = Convert.ToInt32(data["3DayRemainCountRecharge"]);
            //付费7日留存
            if (data.ContainsKey("7DayRemainCountRecharge"))
                tmp.m_7DayRemainCountRecharge = Convert.ToInt32(data["7DayRemainCountRecharge"]);

            //次日付费人数
            if (data.ContainsKey("Day1RechargePersonNum"))
                tmp.m_2DayRechargePersonNum = Convert.ToInt32(data["Day1RechargePersonNum"]);

            IMongoQuery q1 = Query.EQ("genTime", tmp.m_genTime);
            IMongoQuery q2 = Query.EQ("channel", tmp.m_channel);
            IMongoQuery sq = Query.And(q1, q2);
            Dictionary<string, object> oldFirstRecharge =
                DBMgr.getInstance().getTableData(TableName.PUMP_OLD_FIRST_RECHARGE, user.getDbServerID(), DbName.DB_PUMP, sq);
            if (oldFirstRecharge != null)
            {
                if (oldFirstRecharge.ContainsKey("firstRechargeCount"))
                    tmp.m_oldFirstRechargePersonCount = Convert.ToInt32(oldFirstRecharge["firstRechargeCount"]);
            }
        }
        return OpRes.opres_success;
    }

    // 选择全部时，进行总和计算
    void sum()
    {
        List<ResultActivationItem> dataList = new List<ResultActivationItem>();

        foreach (var item in m_result)
        {
            ResultActivationItem res = findSameResult(dataList, item.m_genTime);
            res.m_regeditCount += item.m_regeditCount;
            res.m_deviceActivationCount += item.m_deviceActivationCount;
            res.m_activeCount += item.m_activeCount;
            res.m_deviceLoginCount += item.m_deviceLoginCount;
            res.m_totalIncome += item.m_totalIncome;
            res.m_rechargePersonNum += item.m_rechargePersonNum;
            res.m_rechargeCount += item.m_rechargeCount;
            res.m_newAccIncome += item.m_newAccIncome;
            res.m_newAccRechargePersonNum += item.m_newAccRechargePersonNum;

            res.m_2DayRechargePersonNum += item.m_2DayRechargePersonNum;

            res.m_oldFirstRechargePersonCount += item.m_oldFirstRechargePersonCount;

            if (item.m_2DayRemainCount > 0)
            {
                res.m_2DayRemainCount += item.m_2DayRemainCount;
            }
            if (item.m_3DayRemainCount > 0)
            {
                res.m_3DayRemainCount += item.m_3DayRemainCount;
            }
            if (item.m_7DayRemainCount > 0)
            {
                res.m_7DayRemainCount += item.m_7DayRemainCount;
            }
            if (item.m_30DayRemainCount > 0)
            {
                res.m_30DayRemainCount += item.m_30DayRemainCount;
            }

            if (item.m_2DayDevRemainCount > 0)
            {
                res.m_2DayDevRemainCount += item.m_2DayDevRemainCount;
            }
            if (item.m_3DayDevRemainCount > 0)
            {
                res.m_3DayDevRemainCount += item.m_3DayDevRemainCount;
            }
            if (item.m_7DayDevRemainCount > 0)
            {
                res.m_7DayDevRemainCount += item.m_7DayDevRemainCount;
            }
            if (item.m_30DayDevRemainCount > 0)
            {
                res.m_30DayDevRemainCount += item.m_30DayDevRemainCount;
            }

            //付费次日留存
            if (item.m_2DayRemainCountRecharge > 0)
                res.m_2DayRemainCountRecharge += item.m_2DayRemainCountRecharge;

            if (item.m_3DayRemainCountRecharge > 0)
                res.m_3DayRemainCountRecharge += item.m_3DayRemainCountRecharge;

            if (item.m_7DayRemainCountRecharge > 0)
                res.m_7DayRemainCountRecharge += item.m_7DayRemainCountRecharge;

            //res.m_wchatPublicNumIncome += item.m_wchatPublicNumIncome;
        }

        m_result = dataList;
    }
    ResultActivationItem findSameResult(List<ResultActivationItem> data, DateTime time)
    {
        ResultActivationItem res = null;
        foreach (var d in data)
        {
            if (time == d.m_genTime)
            {
                res = d;
                break;
            }
        }
        if (res == null)
        {
            res = new ResultActivationItem();
            res.reset();
            res.m_genTime = time;
            data.Add(res);
        }
        return res;
    }
}

///////////////////////////////////////////////////////////////////////////
//新进玩家付费监控
public class TdNewPlayerMonitorItem 
{
    public string m_time;
    public string m_channelNo;
    public int m_playerId;
    public int m_turretLevel;
    public int m_totalRecharge; //"recharged"
    public long m_totalOnlineTime; //TotalOnlineTime

    //首次付费
    public string m_firstRechargeTime;
    public long m_firstRechargePlayedTime;
    public int m_firstRechargePayId;
    public long m_firstRechargePlayerMoney;
    public int m_firstRechargePlayerTurretLevel;

    //再次付费
    public string m_secondRechargeTime;
    public long m_secondRechargePlayedTime;
    public int m_secondRechargePayId;
    public long m_secondRechargePlayerMoney;
    public int m_secondRechargePlayerTurretLevel;

    public long m_torpedoChip;//碎片结余
    public long m_totalGainChip;//碎片累计获得

    //渠道
    public string getChannelName()
    {
        string channelName = m_channelNo;
        TdChannelInfo channel = TdChannel.getInstance().getValue(m_channelNo);
        if (channel != null)
            channelName = channel.m_channelName;

        return channelName;
    }

    //获取炮倍
    public string getOpenRate(int level) 
    {
        string operRate = level.ToString();
        Fish_LevelCFGData turret = Fish_TurretLevelCFG.getInstance().getValue(level);
        if (turret != null)
            operRate = turret.m_openRate.ToString();

        return operRate;
    }

    //获取付费点
    public string getPayName(int payId) 
    {
        string payName = payId.ToString();
        RechargeCFGData recharge = RechargeCFG.getInstance().getValue(payId);
        if (recharge != null)
            payName = recharge.m_name;

        return payName;
    }

    //将时间秒转为时分秒
    public string getTimehsm(long time) 
    {
        string timeHsm = "";

        if (time < 60)
        {
            timeHsm = time + "秒".ToString();
        }
        else if (time < 3600)
        {
            int m = Convert.ToInt32(time / 60);
            int s = Convert.ToInt32(time  - 60*m);

            timeHsm = m + "分" + s + "秒";
        }
        else 
        {
            int h = Convert.ToInt32(time/3600);
            int m = Convert.ToInt32((time - 3600*h) / 60);
            int s = Convert.ToInt32(time - 3600*h - 60*m);

            timeHsm = h + "时" + m + "分" + s + "秒";
        }
        return timeHsm;
    }
}
public class QueryTdNewPlayerMonitor : QueryBase 
{
    private List<TdNewPlayerMonitorItem> m_result = new List<TdNewPlayerMonitorItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("create_time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("create_time", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(imq, Query.GT("recharged", 0));

        if (p.m_channelNo != "-1")
            imq = Query.And(imq, Query.EQ("ChannelID", Convert.ToInt32(p.m_channelNo)));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, dip);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO, dip, imq, 
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "create_time", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["create_time"]).ToLocalTime().ToString();

            TdNewPlayerMonitorItem tmp = new TdNewPlayerMonitorItem();
            m_result.Add(tmp);
            tmp.m_time = time;

            if (data.ContainsKey("player_id"))
                tmp.m_playerId = Convert.ToInt32(data["player_id"]);

            if (data.ContainsKey("ChannelID"))
                tmp.m_channelNo = Convert.ToString(data["ChannelID"]).PadLeft(6, '0');

            if (data.ContainsKey("TurretLevel"))
                tmp.m_turretLevel = Convert.ToInt32(data["TurretLevel"]);

            if (data.ContainsKey("recharged"))
                tmp.m_totalRecharge = Convert.ToInt32(data["recharged"]);

            if (data.ContainsKey("TotalOnlineTime"))
                tmp.m_totalOnlineTime = Convert.ToInt64(Math.Round(Convert.ToDouble(data["TotalOnlineTime"])));

            //首次付费
            if (data.ContainsKey("FirstRechargeTime"))
                tmp.m_firstRechargeTime = Convert.ToDateTime(data["FirstRechargeTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("FirstRechargePlayedTime"))
                tmp.m_firstRechargePlayedTime = Convert.ToInt64(Math.Round(Convert.ToDouble(data["FirstRechargePlayedTime"])));

            if (data.ContainsKey("FirstRechargePayId"))
                tmp.m_firstRechargePayId = Convert.ToInt32(data["FirstRechargePayId"]);

            if (data.ContainsKey("FirstRechargePlayerMoney"))
                tmp.m_firstRechargePlayerMoney = Convert.ToInt64(data["FirstRechargePlayerMoney"]);

            if (data.ContainsKey("FirstRechargePlayerTurretLevel"))
                tmp.m_firstRechargePlayerTurretLevel = Convert.ToInt32(data["FirstRechargePlayerTurretLevel"]);

            //再次付费
            if (data.ContainsKey("SecondRechargeTime"))
                tmp.m_secondRechargeTime = Convert.ToDateTime(data["SecondRechargeTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("SecondRechargePlayedTime"))
                tmp.m_secondRechargePlayedTime = Convert.ToInt64(Math.Round(Convert.ToDouble(data["SecondRechargePlayedTime"])));

            if (data.ContainsKey("SecondRechargePayId"))
                tmp.m_secondRechargePayId = Convert.ToInt32(data["SecondRechargePayId"]);

            if (data.ContainsKey("SecondRechargePlayerMoney"))
                tmp.m_secondRechargePlayerMoney = Convert.ToInt64(data["SecondRechargePlayerMoney"]);

            if (data.ContainsKey("SecondRechargePlayerTurretLevel"))
                tmp.m_secondRechargePlayerTurretLevel = Convert.ToInt32(data["SecondRechargePlayerTurretLevel"]);

            if (data.ContainsKey("torpedoChip"))
                tmp.m_torpedoChip = Convert.ToInt64(data["torpedoChip"]);

            if (data.ContainsKey("totalGainChip"))
                tmp.m_totalGainChip = Convert.ToInt64(data["totalGainChip"]);
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//新手炮倍完成率
public class StatNewPlayerOpenRateItem
{
    public string m_time;
    public string m_channelNo;

    public int m_regeditCount;

    public Dictionary<int, int> m_openRateData = new Dictionary<int, int>();

    public string getRate(int key1, int key2)
    {
        if (key2 == 0)
            return "";

        double factGain = (double)key1 * 100.0 / key2;
        return Math.Round(factGain) + "%";
    }

    public void reset()
    {
        m_time = DateTime.MinValue.ToString();
        m_channelNo = "";
    }

    //获取渠道名称
    public string getChannelName(string channelNo)
    {
        string channelName = channelNo;
        TdChannelInfo channel = TdChannel.getInstance().getValue(channelNo);
        if (channel != null)
            return channel.m_channelName;

        return channelName;
    }
}
public class QueryStatNewPlayerOpenRate : QueryBase 
{
    private List<StatNewPlayerOpenRateItem> m_result = new List<StatNewPlayerOpenRateItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);


        if (p.m_channelNo != "-1" && p.m_channelNo !="")
            imq = Query.And(imq, Query.EQ("channel", p.m_channelNo));

        OpRes code = query(user, imq, p);
        if (p.m_channelNo == "")
        {
            sum();
        }
        return code;

    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_TURRET_LEVEL, imq, dip);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TURRET_LEVEL, dip, imq, 0, 0,
             //(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, 
             null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();

            StatNewPlayerOpenRateItem tmp = new StatNewPlayerOpenRateItem();
            m_result.Add(tmp);
            tmp.m_time = time.ToShortDateString();

            if (data.ContainsKey("channel"))
                tmp.m_channelNo = Convert.ToString(data["channel"]);

            for (k = 1; k <= 17; k++) 
            {
                int count = 0;
                string str = k.ToString();
                if (data.ContainsKey(str))
                    count = Convert.ToInt32(data[str]);

                tmp.m_openRateData.Add(k, count);
            }

            //获取渠道注册人数
            int accServer = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (accServer == -1)
                continue;

            IMongoQuery imq1 =  Query.And(Query.EQ("genTime", time.Date),Query.EQ("channel", tmp.m_channelNo));
            Dictionary<string, object> ret =
                DBMgr.getInstance().getTableData(TableName.CHANNEL_TD, accServer, DbName.DB_ACCOUNT, imq1, new string[] { "regeditCount" });
            if (ret != null)
            {
                if (ret.ContainsKey("regeditCount"))
                    tmp.m_regeditCount = Convert.ToInt32(ret["regeditCount"]);
               
            }
        }
        return OpRes.opres_success;
    }

    public void sum() 
    {
        List<StatNewPlayerOpenRateItem> dataList = new List<StatNewPlayerOpenRateItem>();

        foreach (var item in m_result)
        {
            StatNewPlayerOpenRateItem res = findSameResult(dataList, item.m_time);

            foreach(var da in item.m_openRateData)
            {
                res.m_openRateData[da.Key] += da.Value;
            }

            res.m_regeditCount += item.m_regeditCount;
        }

        m_result = dataList;
    }

    StatNewPlayerOpenRateItem findSameResult(List<StatNewPlayerOpenRateItem> data, string time)
    {
        StatNewPlayerOpenRateItem res = null;
        foreach (var d in data)
        {
            if (time == d.m_time)
            {
                res = d;
                break;
            }
        }
        if (res == null)
        {
            res = new StatNewPlayerOpenRateItem();
            res.reset();
            res.m_time = time;
            for (int k = 1; k <= 17; k++)
            {
                res.m_openRateData.Add(k, 0);
            }
            data.Add(res);
        }
        return res;
    }
}
//////////////////////////////////////////////////////////////////////////
public class ResultMaxOnlineItem
{
    public string m_date;
    public string m_timePoint;
    public int m_playerNum;
}

public class QueryMaxOnline : QueryBase
{
    private List<ResultMaxOnlineItem> m_result = new List<ResultMaxOnlineItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("date", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("date", BsonValue.Create(mint));
            queryList.Add(Query.And(imq1, imq2));
        }

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_MAXONLINE_PLAYER, imq, dip);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_MAXONLINE_PLAYER, dip, imq
             /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ResultMaxOnlineItem tmp = new ResultMaxOnlineItem();
            m_result.Add(tmp);

            tmp.m_date = Convert.ToDateTime(data["date"]).ToLocalTime().ToShortDateString();
            tmp.m_timePoint = Convert.ToDateTime(data["maxTimePoint"]).ToLocalTime().ToString();
            tmp.m_playerNum = Convert.ToInt32(data["playerNum"]);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultTotalPlayerMoneyItem
{
    public string m_date;
    public long m_money;
    public long m_safeBox;
}

public class QueryTotalPlayerMoney : QueryBase
{
    private List<ResultTotalPlayerMoneyItem> m_result = new List<ResultTotalPlayerMoneyItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_time_format_error;

        List<IMongoQuery> queryList = new List<IMongoQuery>();

        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            queryList.Add(Query.And(imq1, imq2));
        }

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_PLAYER_TOTAL_MONEY, imq, dip);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_TOTAL_MONEY, dip, imq,0, 0, null, "genTime",false
            /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ResultTotalPlayerMoneyItem tmp = new ResultTotalPlayerMoneyItem();
            m_result.Add(tmp);

            tmp.m_date = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_money = Convert.ToInt64(data["money"]);
            if (data.ContainsKey("safeBox"))
            {
                tmp.m_safeBox = Convert.ToInt64(data["safeBox"]);
            }
            else
            {
                tmp.m_safeBox = -1;
            }
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultLTVItem : GameStatData
{
    public string m_genTime;
    public string m_channel;

    public string get1DayAveRecharge()
    {
        return getAveRecharge(m_1DayTotalRecharge, m_regeditCount);
    }
    public string get3DayAveRecharge()
    {
        return getAveRecharge(m_3DayTotalRecharge, m_regeditCount);
    }
    public string get7DayAveRecharge()
    {
        return getAveRecharge(m_7DayTotalRecharge, m_regeditCount);
    }
    public string get14DayAveRecharge()
    {
        return getAveRecharge(m_14DayTotalRecharge, m_regeditCount);
    }
    public string get30DayAveRecharge()
    {
        return getAveRecharge(m_30DayTotalRecharge, m_regeditCount);
    }
    public string get60DayAveRecharge()
    {
        return getAveRecharge(m_60DayTotalRecharge, m_regeditCount);
    }
    public string get90DayAveRecharge()
    {
        return getAveRecharge(m_90DayTotalRecharge, m_regeditCount);
    }
    private string getAveRecharge(int up, int down)
    {
        if (down == 0)
            return "0";

        if (up == -1)
        {
            return "暂无";
        }
        double val = (double)up / down;
        return Math.Round(val, 2).ToString();
    }
}

// 平均价值
public class QueryTdLTV : QueryBase
{
    private List<ResultLTVItem> m_result = new List<ResultLTVItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        int condCount = 0;
        if (p.m_param != "")
        {
            queryList.Add(Query.EQ("channel", BsonValue.Create(p.m_param)));
        }
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            condCount++;
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            queryList.Add(Query.And(imq1, imq2));
        }

        if (condCount == 0)
            return OpRes.op_res_need_at_least_one_cond;

        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

       // user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.CHANNEL_TD, imq, serverId, DbName.DB_ACCOUNT);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq,
             0, 0, null, "genTime", false
            /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            if (!data.ContainsKey("Day1TotalRecharge"))
                continue;
            if (!data.ContainsKey("Day3TotalRecharge"))
                continue;
            if (!data.ContainsKey("Day7TotalRecharge"))
                continue;
            if (!data.ContainsKey("Day14TotalRecharge"))
                continue;
            if (!data.ContainsKey("Day30TotalRecharge"))
                continue;
            if (!data.ContainsKey("Day60TotalRecharge"))
                continue;
            if (!data.ContainsKey("Day90TotalRecharge"))
                continue;

            ResultLTVItem tmp = new ResultLTVItem();
            tmp.m_genTime = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_channel = Convert.ToString(data["channel"]).PadLeft(6, '0');

            tmp.m_regeditCount = Convert.ToInt32(data["regeditCount"]);
            tmp.m_1DayTotalRecharge = Convert.ToInt32(data["Day1TotalRecharge"]);
            tmp.m_3DayTotalRecharge = Convert.ToInt32(data["Day3TotalRecharge"]);
            tmp.m_7DayTotalRecharge = Convert.ToInt32(data["Day7TotalRecharge"]);
            tmp.m_14DayTotalRecharge = Convert.ToInt32(data["Day14TotalRecharge"]);
            tmp.m_30DayTotalRecharge = Convert.ToInt32(data["Day30TotalRecharge"]);
            tmp.m_60DayTotalRecharge = Convert.ToInt32(data["Day60TotalRecharge"]);
            tmp.m_90DayTotalRecharge = Convert.ToInt32(data["Day90TotalRecharge"]);

            m_result.Add(tmp);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultExchangeStat : ResultTotalConsume
{
    public string getExchangeName(int id)
    {
        ExchangeData data = ExchangeCfg.getInstance().getValue(id);
        if (data != null)
            return data.m_name;
        return "";
    }
}

// 兑换统计
public class QueryExchangeStat : QueryBase
{
    ResultExchangeStat m_result = new ResultExchangeStat();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;
        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_time_format_error;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_EXCHANGE, dip, imq, 0, 0,
            /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/
            null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            TotalConsumeItem item = m_result.create(t);

            int reason = Convert.ToInt32(data["chgId"]);
            long val = Convert.ToInt64(data["value"]);
            item.add(reason, val);

            m_result.addReason(reason);
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
public class RechargePointItem
{
    public DateTime m_time;

    // 渠道-->该渠道数据
    public Dictionary<int, TotalConsumeItem> m_dic = new Dictionary<int, TotalConsumeItem>();

    public void add(int channel, int reason, long value, long count)
    {
        TotalConsumeItem item = null;
        if (m_dic.ContainsKey(channel))
        {
            item = m_dic[channel];
        }
        else
        {
            item = new TotalConsumeItem();
            m_dic.Add(channel, item);
        }
        item.add(reason, value, count);
    }
}
public class ResultRechargePointStat
{
    public HashSet<int> m_fields = new HashSet<int>();

    public List<int> m_sortFields = new List<int>();

    public List<RechargePointItem> m_result = new List<RechargePointItem>();

    public void addReason(int reason)
    {
        m_fields.Add(reason);
    }

    public void reset()
    {
        m_fields.Clear();
        m_result.Clear();
    }

    public RechargePointItem create(DateTime date)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == date)
                return d;
        }

        RechargePointItem item = new RechargePointItem();
        m_result.Add(item);
        item.m_time = date;
        return item;
    }

    public static string getPayName(int payId)
    {
        RechargeCFGData data = RechargeCFG.getInstance().getValue(payId);
        if (data != null)
        {
            return data.m_name;
        }
        return payId.ToString();
    }

    public void sortHeadField()
    {
        m_sortFields.Clear();
        XmlConfig cfg =  ResMgr.getInstance().getRes("M_RechangeNew.xml");
        foreach (var d in m_fields)
        {
            m_sortFields.Add(d);
        }
        m_sortFields.Sort((x, y) =>
        {
            int sv1 = cfg.getInt(x.ToString(), 0);
            int sv2 = cfg.getInt(y.ToString(), 0);
            return sv1 - sv2;
        });
    }
}

// 付费点统计
public class RechargePointItemNew
{
    public DateTime m_time;
    public Dictionary<int, long[]> m_number = new Dictionary<int, long[]>();
    public int m_channel;
   
    public void addActData(int key, long value,long count)
    {
        if (m_number.ContainsKey(key))
        {
            long m_value = m_number[key][0] + value;
            long m_count = m_number[key][1] + count;
            m_number[key][0] = m_value;
            m_number[key][1] = m_count;
        }
        else
        {
            m_number.Add(key, new long[] { value, count});
        }
    }
}
public class ResultRechargePoint
{
    public HashSet<int> m_fields = new HashSet<int>();
    public List<RechargePointItemNew> m_result = new List<RechargePointItemNew>();
    public List<int> m_sortFields = new List<int>();
    public List<DateTime> m_date = new List<DateTime>();

    public void addReason(int reason)
    {
        m_fields.Add(reason);
    }
    public void reset()
    {
        m_fields.Clear();
        m_result.Clear();
    }
    public RechargePointItemNew create(DateTime date)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == date)
                return d;
        }

        RechargePointItemNew item = new RechargePointItemNew();
        m_result.Add(item);
        item.m_time = date;
        return item;
    } 

    public string getChannelName(int id)
    {
        string channelName = id.ToString();
        TdChannelInfo channel = TdChannel.getInstance().getValue(id.ToString().PadLeft(6, '0'));
        if (channel != null)
        {
            return channel.m_channelName;
        }
        return channelName;
    }

    public static string getPayName(int payId)
    {
        RechargeCFGData data = RechargeCFG.getInstance().getValue(payId);
        if (data != null)
        {
            return data.m_name;
        }
        return payId.ToString();
    }

    public void sortHeadField()
    {
        m_sortFields.Clear();
        XmlConfig cfg = ResMgr.getInstance().getRes("M_RechangeNew.xml");
        foreach (var d in m_fields)
        {
            m_sortFields.Add(d);
        }
        m_sortFields.Sort((x, y) =>
        {
            int sv1 = cfg.getInt(x.ToString(), 0);
            int sv2 = cfg.getInt(y.ToString(), 0);
            return sv1 - sv2;
        });
    }
}
public class QueryRechargePointStat : QueryBase
{
    ResultRechargePoint m_result = new ResultRechargePoint();
    private QueryCondition m_cond = new QueryCondition();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;
        m_cond.startQuery();

        OpRes code = makeQuery(param, user, m_cond);
        if (code != OpRes.opres_success)
            return code;

        IMongoQuery imq = m_cond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        if (queryCond.isExport()) //导出
        {
            queryCond.addCond("time", p.m_time);
            queryCond.addCond("channel",p.m_param);
            queryCond.addCond("way",p.m_showWay);  //查看方式
        }
        else   
        {
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq = Query.And(imq1, imq2);
            queryCond.addImq(imq);
            if (!string.IsNullOrEmpty(p.m_param))
            {
                int channelId;
                if (!int.TryParse(p.m_param, out channelId))
                    return OpRes.op_res_param_not_valid;
                
                queryCond.addImq(Query.EQ("channel", channelId));
            }
        }
        return OpRes.opres_success;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_RECHARGE, dip, imq,0, 0, null, "genTime",false
            /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();

            int channel = Convert.ToInt32(data["channel"]);

            RechargePointItemNew item = m_result.create(t);

            int reason = Convert.ToInt32(data["payId"]);
            long val = Convert.ToInt64(data["value"]);
            long count = Convert.ToInt64(data["count"]);

            item.addActData(reason, val, count);
            item.m_channel = channel;
            m_result.addReason(reason);
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//七日活动
public class SevenDayItem
{
    public DateTime m_time;
    public Dictionary<string, int> m_number = new Dictionary<string, int>();
    public int m_regeditCount;
    public int m_channel;
    public string getPercent(int key,int num)
    {
        string data_percent = key.ToString();
        if(num!=0)
        {
            data_percent=Math.Round(((Convert.ToInt32(key)) * 1.0 * 100) / num,2) + "%";
        }
        return data_percent;
    }
    public void addActData(string key,int count) 
    {
        if (m_number.ContainsKey(key))
        {
            int num = m_number[key] + count;
            m_number[key]=num;
        }
        else 
        {
            m_number.Add(key, count);
        }
    }
}
public class ResultSevenDayActivity
{
    public HashSet<string> m_fields = new HashSet<string>();

    public List<SevenDayItem> m_result = new List<SevenDayItem>();
    public List<DateTime> m_date = new List<DateTime>();

    public void addReason(string reason)
    {
        m_fields.Add(reason);
    }
    public void reset()
    {
        m_fields.Clear();
        m_result.Clear();
    }
    public SevenDayItem create(string channel,DateTime date)
    {
        if(channel=="")
        {
            foreach (var d in m_result)
            {
                if (d.m_time == date)
                    return d;
            }
        }
        
        SevenDayItem item = new SevenDayItem();
        m_result.Add(item);
        item.m_time = date;
        return item;
    }       

    public string getName(string id) 
    {
        int key = Convert.ToInt32(id);
        SevenDayActivityData tmp = SevenDayActivityCFG.getInstance().getValue(key);
        if (tmp!=null)
        {
            return tmp.m_activityName;
        }
        return id;
    }

    public string getChannelName(int id) 
    {
        string channelName = id.ToString();
        TdChannelInfo channel = TdChannel.getInstance().getValue(id.ToString().PadLeft(6, '0'));
        if(channel!=null)
        {
            return channel.m_channelName;
        }
        return channelName;
    }
}

public class QuerySevenDayActivityStat : QueryBase
{
    private ResultSevenDayActivity m_result = new ResultSevenDayActivity();
    private QueryCondition m_cond = new QueryCondition();
    static string[] m_regeditCount = {"regeditCount"};

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;
        m_cond.startQuery();

        OpRes code = makeQuery(param, user, m_cond);
        if (code != OpRes.opres_success)
            return code;

        IMongoQuery imq = m_cond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        queryCond.addImq(imq);

        if (p.m_channelNo != "")
        {
            queryCond.addImq(Query.EQ("channel", p.m_channelNo));
        }
        return OpRes.opres_success;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_7DAYACTIVITY, dip, imq, 0, 0, null, "genTime",false
            /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

         for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();

            SevenDayItem item = m_result.create(param.m_channelNo,t);
           
            int channel = 0;
             if(data.ContainsKey("channel"))
            {
                channel = Convert.ToInt32(data["channel"]);
            }
            //注册人数  构造时间条件
            DateTime mint = Convert.ToDateTime(t);
            DateTime maxt = mint.AddDays(1);
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt)); 
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

            List<IMongoQuery> queryList = new List<IMongoQuery>();
            queryList.Add(Query.And(imq1, imq2));

             if(param.m_channelNo!="")  //渠道
             {
                 queryList.Add(Query.EQ("channel", param.m_channelNo));
             }

            IMongoQuery tmpImq = queryList.Count > 0 ? Query.And(queryList) : null;

            int accServer = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            List<Dictionary<string, object>> regeditList =DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, accServer,DbName.DB_ACCOUNT, tmpImq, 0, 0, m_regeditCount);

            int regeditCount = 0;
            if (regeditList != null && regeditList.Count!=0)
            {
                foreach (var d in regeditList)
                {
                    if (d.ContainsKey("regeditCount"))
                    {
                        regeditCount += Convert.ToInt32(d["regeditCount"]);
                    }
                }
            }

            foreach(var d in data)
            {
                string id = d.Key;
                if(id!="_id" && id!="genTime" && id!="channel")
                {
                    m_result.addReason(id);  //表头
                    if (data.ContainsKey(id))
                    {
                        item.addActData(id, Convert.ToInt32(data[id]));
                    }
                }
            }
            item.m_time = t;
            item.m_regeditCount = regeditCount;
            item.m_channel = channel;
        }
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//捕鱼盛宴活动
public class dailyFishlordFeastItem 
{
    public string m_time;
    public int m_prob1Count;
    public int m_prob2Count;
    public int m_prob3Count;
    public int m_prob4Count;
    public int m_prob5Count;
}
public class QueryFishlordFeastStat : QueryBase 
{
    List<dailyFishlordFeastItem> m_result = new List<dailyFishlordFeastItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        QueryCondition m_cond = new QueryCondition();

        IMongoQuery imq1 = Query.And(Query.LT("genTime", BsonValue.Create(maxt)), Query.GTE("genTime", BsonValue.Create(mint)));
        m_cond.addImq(imq1);

        IMongoQuery imq = m_cond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_GOLD_FEAST, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_GOLD_FEAST, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            dailyFishlordFeastItem tmp = new dailyFishlordFeastItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            switch(p.m_op)
            {
                case 0:
                    if(data.ContainsKey("Get100000"))
                    {
                        tmp.m_prob1Count = Convert.ToInt32(data["Get100000"]);
                    }
                    if (data.ContainsKey("Get500000"))
                    {
                        tmp.m_prob2Count = Convert.ToInt32(data["Get500000"]);
                    }
                    if (data.ContainsKey("Get1000000"))
                    {
                        tmp.m_prob3Count = Convert.ToInt32(data["Get1000000"]);
                    }
                    if (data.ContainsKey("Get2000000"))
                    {
                        tmp.m_prob4Count = Convert.ToInt32(data["Get2000000"]);
                    }
                    if (data.ContainsKey("Get3000000"))
                    {
                        tmp.m_prob5Count = Convert.ToInt32(data["Get3000000"]);
                    }
                    break;
                case 1:
                    if (data.ContainsKey("Recv100000"))
                    {
                        tmp.m_prob1Count = Convert.ToInt32(data["Recv100000"]);
                    }
                    if (data.ContainsKey("Recv500000"))
                    {
                        tmp.m_prob2Count = Convert.ToInt32(data["Recv500000"]);
                    }
                    if (data.ContainsKey("Recv1000000"))
                    {
                        tmp.m_prob3Count = Convert.ToInt32(data["Recv1000000"]);
                    }
                    if (data.ContainsKey("Recv2000000"))
                    {
                        tmp.m_prob4Count = Convert.ToInt32(data["Recv2000000"]);
                    }
                    if (data.ContainsKey("Recv3000000"))
                    {
                        tmp.m_prob5Count = Convert.ToInt32(data["Recv3000000"]);
                    }
                    break;
            }
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//////////////////////////////////////////////////////////////////////////
//万炮盛典活动
public class dailyReachFireCountPlayerStatItem 
{
    public string m_time;
    public Dictionary<int, int> m_fireCountNum = new Dictionary<int, int>();
    
}
public class QueryWpActivityStat : QueryBase 
{
    List<dailyReachFireCountPlayerStatItem> m_result = new List<dailyReachFireCountPlayerStatItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_WP_COUNT, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_WP_COUNT, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int[] OpenRate = new int[] {5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000 };

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            dailyReachFireCountPlayerStatItem tmp = new dailyReachFireCountPlayerStatItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            foreach(var d in OpenRate)
            {
                string l = Convert.ToString(getFishLevel(d));  //获取对应的捕鱼等级
                if (data.ContainsKey(l))
                {
                    tmp.m_fireCountNum.Add(d, Convert.ToInt32(data[l]));
                }
                else 
                {
                    tmp.m_fireCountNum.Add(d, 0);
                }
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果

    public int getFishLevel(int key) 
    {
        var fish_Level = Fish_OpenRate_LevelCFG.getInstance().getValue(key);
        if (fish_Level != null)
        {
            return fish_Level.m_level;
        }
        return key;
    }

    public override object getQueryResult()
    {
        return m_result;
    }
}

public class dailyFirstReachFireCountPlayerItem
{
    public string m_time;
    public string m_firstReachFireCount;
    public string m_nickName;
    public int m_playerId;
    public int m_openRate;
}

public class WpConsumeItem
{
    Dictionary<int, int> m_item = new Dictionary<int, int>();

    public void addItem(int itemId, int count)
    {
        if (m_item.ContainsKey(itemId))
        {
            int t = m_item[itemId];
            m_item[itemId] = t + count;
        }
        else
        {
            m_item.Add(itemId, count);
        }
    }

    public int getItemCount(int itemId)
    {
        if (m_item.ContainsKey(itemId))
        {
            return m_item[itemId];
        }
        return 0;
    }
}

public class WpItemConsumeResult : PlayerTypeDataCollect<WpConsumeItem>
{
    public static int[] S_ITEM = { 2, 160, 161, 162, 163 };

    public void addItemByTime(DateTime t, int fishLevel, int itemId, int count)
    {
        bool add = false;
        PlayerTypeData<WpConsumeItem> citem = getDataByTime(t);
        if (citem == null)
        {
            citem = new PlayerTypeData<WpConsumeItem>();
            add = true;
        }

        WpConsumeItem witem = citem.getData(fishLevel);
        if (witem == null)
        {
            witem = new WpConsumeItem();
            citem.addData(fishLevel, witem);
        }

        witem.addItem(itemId, count);

        if (add)
        {
            addData(t, fishLevel, witem);
        }
    }
}

public class QueryWpActivityPlayerStat : QueryBase
{
    List<dailyFirstReachFireCountPlayerItem> m_result = new List<dailyFirstReachFireCountPlayerItem>();
    WpItemConsumeResult m_result1 = new WpItemConsumeResult();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        {
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq = Query.And(imq1, imq2);
            return queryUpLevelItemConsume(imq, user);
        }
        
        int[] OpenRate = new int[] { 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000 };
        int[] fishLevel=new int[OpenRate.Length];
        int k=0;
        foreach (int d in OpenRate)
        {
            var fish_Level = Fish_OpenRate_LevelCFG.getInstance().getValue(d);
            if (fish_Level != null)
            {
                fishLevel[k] = fish_Level.m_level;
            }
            k++;
        }

        for (DateTime time = maxt; time >= mint;time=time.AddDays(-1)) //每一天
        {
            Dictionary<int, int> playerList = new Dictionary<int, int>();  //玩家列表

            DateTime time_end = time.AddDays(1).AddSeconds(-1);
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(time_end));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(time));
            IMongoQuery imq = Query.And(imq1, imq2);
            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
            List<Dictionary<string, object>> dataList =DBMgr.getInstance().executeQuery(TableName.PUMP_WP_PLAYER, dip, imq,
                 0, 0, null, "genTime", true);
            if (dataList == null || dataList.Count == 0)
            {
                continue;
            }
            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];

                if(data.ContainsKey("fishLevel"))
                {
                    foreach (int level in fishLevel)
                    {
                        if(playerList.ContainsKey(level))
                        {
                            continue;
                        }
                        if(level==Convert.ToInt32(data["fishLevel"]))
                        {
                            playerList.Add(level,Convert.ToInt32(data["playerId"]));
                        }
                    }
                }
            }

            var player_list = playerList.OrderBy(a => a.Key).ToList();
            foreach (var d in player_list)
            {
                int playerId=d.Value;
                dailyFirstReachFireCountPlayerItem tmp = new dailyFirstReachFireCountPlayerItem();
                switch(d.Key)
                {
                    case 1: tmp.m_firstReachFireCount = "首位达到6000"; break;
                    case 2: tmp.m_firstReachFireCount = "首位达到6500"; break;
                    case 3: tmp.m_firstReachFireCount = "首位达到7000"; break;
                    case 4: tmp.m_firstReachFireCount = "首位达到7500"; break;
                    case 5: tmp.m_firstReachFireCount = "首位达到8000"; break;
                    case 6: tmp.m_firstReachFireCount = "首位达到8500"; break;
                    case 7: tmp.m_firstReachFireCount = "首位达到9000"; break;
                    case 8: tmp.m_firstReachFireCount = "首位达到9500"; break;
                    case 9: tmp.m_firstReachFireCount = "首位达到10000"; break;
                }
                m_result.Add(tmp);
                tmp.m_time = time.ToShortDateString();
                tmp.m_playerId = playerId;

                QueryCondition queryCond1 = new QueryCondition();
                queryCond1.addImq(imq);
                queryCond1.addImq(Query.EQ("playerId", playerId));
                IMongoQuery imq_1 = queryCond1.getImq();

                string[] m_fields = { "nickname","GameLevel"};
                DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
                Dictionary<string, object> ret =
                DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", playerId, m_fields, dip_1);
                if(ret!=null)
                {
                    if(ret.ContainsKey("nickname"))
                    {
                        tmp.m_nickName=Convert.ToString(ret["nickname"]);
                    }
                    if(ret.ContainsKey("GameLevel"))
                    {
                        var fish_Level = Fish_LevelCFG.getInstance().getValue(Convert.ToInt32(ret["GameLevel"]));
                        if (fish_Level != null)
                        {
                            tmp.m_openRate = fish_Level.m_level;
                        }
                    }
                }
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result1;
    }

    OpRes queryUpLevelItemConsume(IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_WP_ITEM_CONSUME,
            dip, imq, 0, 0, null, "genTime", false);

        if (dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        m_result1.reset();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            int fishLevel = Convert.ToInt32(data["fishLevel"]);

            foreach (var d in data)
            {
                try
                {
                    int itemid = Convert.ToInt32(d.Key);
                    int count = Convert.ToInt32(d.Value);
                    m_result1.addItemByTime(t, fishLevel, itemid, count);
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//拉霸
//拉霸活动统计
public class LabaActivityStatItem
{
    public string m_time;
    public int m_dailyGameCount;
    public int m_dailyRechargePersonNum;
    public int m_dailyPlayerCount;
    public int m_dailyGainGameCount;
    public int m_dailyRemainGameCount;
}
public class QueryLabaActivityStat : QueryBase 
{
    List<LabaActivityStatItem> m_result = new List<LabaActivityStatItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_LABA_TOTAL, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_LABA_TOTAL, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            LabaActivityStatItem tmp = new LabaActivityStatItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            //每日游戏充值人数
            //活跃人数  构造时间条件
            DateTime time = Convert.ToDateTime(tmp.m_time);
            DateTime time_end=time.AddDays(1).AddSeconds(-1);
            IMongoQuery imq_1 = Query.LTE("genTime", BsonValue.Create(time_end));
            IMongoQuery imq_2 = Query.GTE("genTime", BsonValue.Create(time));
            IMongoQuery tmpImq = Query.And(imq_1, imq_2);

            int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (serverId == -1)
                return OpRes.op_res_failed;
            List<Dictionary<string, object>> rechargePersonNumList =
                 DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId,
                 DbName.DB_ACCOUNT, tmpImq, 0, 0, new string[]{"rechargePersonNum"});

            if (rechargePersonNumList != null)
            {
                foreach (var d in rechargePersonNumList)
                {
                    if (d.ContainsKey("rechargePersonNum"))
                    {
                        tmp.m_dailyRechargePersonNum += Convert.ToInt32(d["rechargePersonNum"]);
                    }
                }
            }

            if(data.ContainsKey("LaLotteryCount"))
            {
                tmp.m_dailyGameCount = Convert.ToInt32(data["LaLotteryCount"]);
            }
            if(data.ContainsKey("LaPlayerCount"))
            {
                tmp.m_dailyPlayerCount = Convert.ToInt32(data["LaPlayerCount"]);
            }
            if(data.ContainsKey("GainLaCount"))
            {
                tmp.m_dailyGainGameCount = Convert.ToInt32(data["GainLaCount"]);
            }
            if(data.ContainsKey("RemainLotteryCount"))
            {
                tmp.m_dailyRemainGameCount = Convert.ToInt32(data["RemainLotteryCount"]);
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}

public class LabaLotteryPlayerRecordItem 
{
    public string m_time;
    public int m_playerId;
    public int m_rewardId;
    public int m_rewardCount;
}
//玩家抽奖次数统计
public class QueryPlayerLabaLotteryCount : QueryBase 
{
    List<LabaLotteryPlayerRecordItem> m_result = new List<LabaLotteryPlayerRecordItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);

        queryCond.addImq(imq3);
        if (p.m_param != "")
        {
            queryCond.addImq(Query.EQ("playerId", Convert.ToInt32(p.m_param)));
        }
        IMongoQuery imq = queryCond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_LABA_LOTTERY_PLAYER,imq,dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_LABA_LOTTERY_PLAYER, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            LabaLotteryPlayerRecordItem tmp = new LabaLotteryPlayerRecordItem();
            m_result.Add(tmp);
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_rewardCount = Convert.ToInt32(data["lotteryCount"]);
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//玩家抽奖记录查询
public class QueryPlayerLabaLotteryRecord:QueryBase
{
    List<LabaLotteryPlayerRecordItem>m_result=new List<LabaLotteryPlayerRecordItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 =Query.And(imq1,imq2);

        queryCond.addImq(imq3);
        if(p.m_param!="")
        {
            queryCond.addImq(Query.EQ("playerId", Convert.ToInt32(p.m_param)));
        }
        IMongoQuery imq = queryCond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_LABA_LOTTERY, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_LABA_LOTTERY, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            LabaLotteryPlayerRecordItem tmp = new LabaLotteryPlayerRecordItem();
            m_result.Add(tmp);
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_rewardId = Convert.ToInt32(data["resultId"]);
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//拉霸抽奖档位统计
public class LabaLotteryProbItem
{
    public string m_time;
    public int m_labaProbId;
    public int m_appearCount;
    public long m_goldReward;
    public long m_diamondReward;
}
public class QueryLabaLotteryProb : QueryBase 
{
    List<LabaLotteryProbItem> m_result = new List<LabaLotteryProbItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_LABA_LOTTERY_PROB, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_LABA_LOTTERY_PROB, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            LabaLotteryProbItem tmp = new LabaLotteryProbItem();
            m_result.Add(tmp);
            tmp.m_labaProbId = Convert.ToInt32(data["resultId"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_appearCount = Convert.ToInt32(data["appearCount"]);

            var labaProbData = ActivityLabaCFG.getInstance().getValue(tmp.m_labaProbId);

            string[] rewardList = Tool.split(labaProbData.m_rewardList, ',', StringSplitOptions.RemoveEmptyEntries);
            string[] rewardCountList = Tool.split(labaProbData.m_rewardCount, ',', StringSplitOptions.RemoveEmptyEntries);

            foreach (string rewardId in rewardList)
            {
                int index = Array.IndexOf(rewardList,rewardId);
                if (Convert.ToInt32(rewardId) == 1)  //金币
                {
                    long goldReward=Convert.ToInt64(rewardCountList[index]);
                    tmp.m_goldReward = tmp.m_appearCount * goldReward;  //金币发放
                }
                if (Convert.ToInt32(rewardId) == 2)  //钻石
                {
                    long diamondReward = Convert.ToInt64(rewardCountList[index]);
                    tmp.m_diamondReward = tmp.m_appearCount * diamondReward; //钻石发放
                }
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
/////////////////////////////////////////////////////////////////////////
//强更补偿查询
public class forceUpdateReward 
{
    public string m_time;
    public int m_recvPersonCount;
    public int m_itemCount;
}
public class QueryForceUpdateReward : QueryBase 
{
    List<forceUpdateReward> m_result = new List<forceUpdateReward>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_FORCE_UPDATE_REWARD, dip, imq,
            0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            forceUpdateReward tmp = new forceUpdateReward();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if (data.ContainsKey("recvPersonCount"))
                tmp.m_recvPersonCount = Convert.ToInt32(data["recvPersonCount"]);

            if(data.ContainsKey("itemCount"))
                tmp.m_itemCount = Convert.ToInt32(data["itemCount"]);
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}

/////////////////////////////////////////////////////////////////////////
//竞技场消耗统计
public class JingjiConsumeStat 
{
    public string m_time;
    public int m_joinPerson;
    public int m_joinCount;
    public int m_enterTicket;
    public int m_item2;
    public int m_item1;
    public int m_itemTotal;
    public int m_quest;

    public int getSum(int num1,int num2) 
    {
        return (num2 * 18 + num1 * 2);
    }
}
public class QueryFishlordJingjiConsume : QueryBase 
{
    List<JingjiConsumeStat> m_result = new List<JingjiConsumeStat>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p=(ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time,ref mint,ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        IMongoQuery imq1=Query.LT("genTime",BsonValue.Create(maxt));
        IMongoQuery imq2=Query.GTE("genTime",BsonValue.Create(mint));
        IMongoQuery imq3=Query.EQ("roomId",11);
        IMongoQuery imq = Query.And(imq1,imq2,imq3);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_INCOME_OUTLAY,dip,imq,
            0,0,null,"genTime",false);
        if(dataList==null || dataList.Count==0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i<dataList.Count;i++ ) 
        {
            Dictionary<string,object> data=dataList[i];
            JingjiConsumeStat tmp = new JingjiConsumeStat();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if(data.ContainsKey("joinPerson"))
            {
                tmp.m_joinPerson = Convert.ToInt32(data["joinPerson"]);
            }
            if(data.ContainsKey("joinCount"))
            {
                tmp.m_joinCount = Convert.ToInt32(data["joinCount"]);
            }

            if(data.ContainsKey("enterTicket"))
            {
                tmp.m_enterTicket = Convert.ToInt32(data["enterTicket"]);
            }

            if(data.ContainsKey("item_2"))
            {
                tmp.m_item2 = Convert.ToInt32(data["item_2"]);
            }

            if(data.ContainsKey("item_1"))
            {
                tmp.m_item1 = Convert.ToInt32(data["item_1"]);
            }

            tmp.m_itemTotal = tmp.getSum(tmp.m_item1,tmp.m_item2);

            if(data.ContainsKey("quest"))
            {
                tmp.m_quest = Convert.ToInt32(data["quest"]);
            }
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//竞技场产出统计
public class JingjiOutlayStat 
{
    public string m_time;
    public int m_dbOutlay;
    public int m_copperBullet;  // 铜
    public int m_silverBullet;  // 银
    public int m_goldBullet;    // 金
    public int m_diamondBullet;    // 钻石
}
public class QueryFishlordJingjiOutlay : QueryBase 
{
    List<JingjiOutlayStat> m_result = new List<JingjiOutlayStat>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ("roomId", 11);
        IMongoQuery imq = Query.And(imq1, imq2, imq3);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_OUT_LAY, dip, imq,
            0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            JingjiOutlayStat tmp = new JingjiOutlayStat();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if(data.ContainsKey("item_14"))
            {
                tmp.m_dbOutlay = Convert.ToInt32(data["item_14"]);
            }

            if(data.ContainsKey("item_113"))
            {
                tmp.m_copperBullet = Convert.ToInt32(data["item_113"]);
            }

            if(data.ContainsKey("item_114"))
            {
                tmp.m_silverBullet = Convert.ToInt32(data["item_114"]);
            }

            if(data.ContainsKey("item_115"))
            {
                tmp.m_goldBullet = Convert.ToInt32(data["item_115"]);
            }
            if (data.ContainsKey("item_116"))
            {
                tmp.m_diamondBullet = Convert.ToInt32(data["item_116"]);
            }
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//竞技场任务统计
//任务id列表
public class TaskItem 
{
    public int m_taskId; //任务id
    public int m_taskJoin;//接取人数
    public int m_taskFinish;//完成人数
    public int m_groupId; //所属组
}

public class JingjiTaskStat
{
    public string m_time;
    public int m_idNum;
    public List<TaskItem> m_taskList = new List<TaskItem>();
}
public class QueryFishlordJingjiTask : QueryBase 
{
    List<JingjiTaskStat> m_result = new List<JingjiTaskStat>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ("roomId", 11);
        IMongoQuery imq = Query.And(imq1, imq2, imq3);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_TASK, dip, imq,
            0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;
        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            JingjiTaskStat tmp = new JingjiTaskStat();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            for (k = 1; k <= 36; k++)
            {
                string join = string.Format("join{0}", k);
                string finish = string.Format("finish{0}", k);
                if (data.ContainsKey(join)) 
                {
                    TaskItem task = new TaskItem();
                    tmp.m_taskList.Add(task);
                    task.m_taskJoin = Convert.ToInt32(data[join]);
                 
                    if (data.ContainsKey(finish))
                        task.m_taskFinish = Convert.ToInt32(data[finish]);
                    tmp.m_idNum++;
                    task.m_taskId = k;
                    task.m_groupId = (k - 1) / 3 + 1;
                } 
            }
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//竞技场玩家分布统计
public class JingjiPlayerStat 
{
    public string m_time;
    public int[] m_data = new int[33];
}
public class QueryFishlordJingjiPlayer : QueryBase 
{
    List<JingjiPlayerStat> m_result = new List<JingjiPlayerStat>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.STAT_BAOJIN_JOIN_DISTRIBUTE, dip, imq,
            0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            JingjiPlayerStat tmp = new JingjiPlayerStat();
            m_result.Add(tmp);
            tmp.m_time= Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            for (int k = 0; k< 41; k++ ) 
            {
                string level="Level_"+(k+19);
                if (data.ContainsKey(level))
                {
                    tmp.m_data[k] = Convert.ToInt32(data[level]);
                }
                else 
                {
                    tmp.m_data[k] = 0;
                }
            }
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
/////////////////////////////////////////////////////////////////////////
//东海龙宫消耗统计
public class DragonPalaceConsumeStat
{
    public string m_time;
    public int m_joinPerson;
    public int m_exchangeCount;
    public long m_goldConsume;
    public int m_killFish1;
    public int m_killFish2;
    public int m_killFish3;
    public long m_exchangeItem1;
    public int m_exchangeItem2;
    public int m_exchangeItem3;
    public int m_exchangeItem23;
    public int m_exchangeItem24;
    public int m_exchangeItem25;
    public int m_exchangeItem26;
    public int m_exchangeItem27;

    public Dictionary<int, int[]> m_exchangeItems = new Dictionary<int, int[]>();

    public long getItem(int key)
    {
       var item = ItemCFG.getInstance().getValue(key);
       if (item != null)
           return Convert.ToInt64(item.m_goldValue);
       return 0;
    }
}
public class QueryFishlordDragonPalaceConsume : QueryBase {
    List<DragonPalaceConsumeStat> m_result = new List<DragonPalaceConsumeStat>();
    public string[] fields = new string[] { "DailyTrickIncome", "TodayActivePlayerCount"};
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(
            TableName.STAT_FISHLORD_DRAGON_PALACE_INCOME_OUTLAY, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DragonPalaceConsumeStat tmp = new DragonPalaceConsumeStat();
            m_result.Add(tmp);

            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = t.ToShortDateString();
            if (data.ContainsKey("joinPerson"))
                tmp.m_joinPerson = Convert.ToInt32(data["joinPerson"]);

            if (data.ContainsKey("exchangeCount"))
                tmp.m_exchangeCount = Convert.ToInt32(data["exchangeCount"]);

            if (data.ContainsKey("killFish_19"))
                tmp.m_killFish1 = Convert.ToInt32(data["killFish_19"]);

            if (data.ContainsKey("killFish_20"))
                tmp.m_killFish2 = Convert.ToInt32(data["killFish_20"]);

            if (data.ContainsKey("killFish_705"))
                tmp.m_killFish3 = Convert.ToInt32(data["killFish_705"]);

            /////////////////////////////////////////////////////////////////////////////////////////////
            long item1 = 0;//金币
            if (data.ContainsKey("exchangeItem_1"))
                item1 = Convert.ToInt64(data["exchangeItem_1"]);

            int bulletHead_23 = 0; //普通鱼雷
            if (data.ContainsKey("exchangeItem_23"))
            {
                tmp.m_exchangeItem23 = Convert.ToInt32(data["exchangeItem_23"]);
                ItemCFGData item = ItemCFG.getInstance().getValue(23);
                if (item != null)
                    bulletHead_23 = tmp.m_exchangeItem23 * Convert.ToInt32(item.m_goldValue);
            }

            int bulletHead_24 = 0; //青铜鱼雷
            if (data.ContainsKey("exchangeItem_24"))
            {
                tmp.m_exchangeItem24 = Convert.ToInt32(data["exchangeItem_24"]);
                ItemCFGData item = ItemCFG.getInstance().getValue(24);
                if (item != null)
                    bulletHead_24 = tmp.m_exchangeItem24 * Convert.ToInt32(item.m_goldValue);
            }

            int bulletHead_25 = 0; //白银鱼雷
            if (data.ContainsKey("exchangeItem_25"))
            {
                tmp.m_exchangeItem25 = Convert.ToInt32(data["exchangeItem_25"]);
                ItemCFGData item = ItemCFG.getInstance().getValue(25);
                if (item != null)
                    bulletHead_25 = tmp.m_exchangeItem25 * Convert.ToInt32(item.m_goldValue);
            }

            int bulletHead_26 = 0; //黄金鱼雷
            if (data.ContainsKey("exchangeItem_26"))
            {
                tmp.m_exchangeItem26 = Convert.ToInt32(data["exchangeItem_26"]);
                ItemCFGData item = ItemCFG.getInstance().getValue(26);
                if (item != null)
                    bulletHead_26 = tmp.m_exchangeItem26 * Convert.ToInt32(item.m_goldValue);
            }

            int bulletHead_27 = 0; //钻石鱼雷
            if (data.ContainsKey("exchangeItem_27"))
            {
                tmp.m_exchangeItem27 = Convert.ToInt32(data["exchangeItem_27"]);
                ItemCFGData item = ItemCFG.getInstance().getValue(117);
                if (item != null)
                    bulletHead_27 = tmp.m_exchangeItem27 * Convert.ToInt32(item.m_goldValue);
            }
            //总的金币兑换产出
            tmp.m_exchangeItem1 = item1;// +bulletHead_23 + bulletHead_24 + bulletHead_25 + bulletHead_26 + bulletHead_27;

            //魔石总产出
            if (data.ContainsKey("dimensityOutlay"))
                tmp.m_exchangeItem2 = Convert.ToInt32(data["dimensityOutlay"]);

            //金币消耗 参与人数 龙宫场5
            IMongoQuery imq_1 = Query.And(
                    Query.EQ("Date", t.Date),
                    Query.EQ("ROOMID", 5)
                );
            Dictionary<string, object> dataInfo =
            DBMgr.getInstance().getTableData(TableName.PUMP_FISHLORD_EVERY_DAY, dip, imq_1, fields);
            if (dataInfo != null) {
                if (dataInfo.ContainsKey("DailyTrickIncome"))
                    tmp.m_goldConsume = Convert.ToInt64(dataInfo["DailyTrickIncome"]);

                if(dataInfo.ContainsKey("TodayActivePlayerCount"))
                    tmp.m_joinPerson = Convert.ToInt32(dataInfo["TodayActivePlayerCount"]);
            }

            //鱼雷兑换人数、次数
            //普通鱼雷
            //初始化
            for (int k = 23; k <= 27; k++)
            {
                tmp.m_exchangeItems.Add(k, new int[] { 0, 0 });
            }
            IMongoQuery imq_item = Query.EQ("genTime", t.Date);
            List<Dictionary<string, object>> exchangeItemList = DBMgr.getInstance().executeQuery(
                TableName.STAT_PUMP_DRAGON_PALACE_PLAYER, dip, imq_item, 0, 0, null, "genTime", false);
            if (exchangeItemList == null || exchangeItemList.Count == 0)
                continue;

            for (int j = 0; j < exchangeItemList.Count; j++)
            {
                for (int k = 23; k <= 27; k++)
                {
                    string exchangeItem = "exchangeItem_" + k;
                    int count = 0;
                    if (exchangeItemList[j].ContainsKey(exchangeItem))
                        count += Convert.ToInt32(exchangeItemList[j][exchangeItem]);

                    if (count > 0)
                    {
                        tmp.m_exchangeItems[k][0] += 1;
                        tmp.m_exchangeItems[k][1] += count;
                    }

                }
            }

        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}

public class QueryFishlordDragonPalacePlayer : QueryBase 
{
    List<JingjiPlayerStat> m_result = new List<JingjiPlayerStat>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(
            TableName.STAT_FISHLORD_DRAGON_PALACE_PLAYER_DISTRIBUTE, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            JingjiPlayerStat tmp = new JingjiPlayerStat();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            for (int k = 0; k < 33; k++)
            {
                string level = "Level_" + (k + 16);
                if (data.ContainsKey(level))
                {
                    tmp.m_data[k] = Convert.ToInt32(data[level]);
                }
                else
                {
                    tmp.m_data[k] = 0;
                }
            }
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}

//诛龙箭统计
public class DragonPalaceKillItem 
{
    public string m_time;
    public long m_item88Income;
    public long m_item88Outlay;

    public long m_item89Income;
    public long m_item89Outlay;
}
public class QueryFishlordDragonPalaceKill : QueryBase 
{
    List<DragonPalaceKillItem> m_result = new List<DragonPalaceKillItem>();
    private string[] m_field = new string[] { "genTime","item88income","item88outlay","item89income","item89outlay"};
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(imq, 
                        Query.Or(
                            Query.GT("item88income", 0),
                            Query.GT("item88outlay", 0),
                            Query.GT("item89income", 0),
                            Query.GT("item89outlay", 0)
                        ));

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_DRAGON_PALACE_INCOME_OUTLAY, dip, imq, 0, 0, m_field, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DragonPalaceKillItem tmp = new DragonPalaceKillItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("item88income"))
                tmp.m_item88Income = Convert.ToInt64(data["item88income"]);

            if (data.ContainsKey("item88outlay"))
                tmp.m_item88Outlay = Convert.ToInt64(data["item88outlay"]);

            if (data.ContainsKey("item89income"))
                tmp.m_item89Income = Convert.ToInt64(data["item89income"]);

            if (data.ContainsKey("item89outlay"))
                tmp.m_item89Outlay = Convert.ToInt64(data["item89outlay"]);

        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
/////////////////////////////////////////////////////////////////////////
//爆金比赛场
public class FishlordBaojinStatItem
{
    public string m_time; //日期
    public int m_joinCount;
    public long m_ticketIncome;//比赛门票收入
    public long m_giveOutGold;
    public int m_matchPerson1;//比赛1次人数
    public int m_matchPerson2;
    public int m_matchPerson3;
    public int m_matchPerson4;
    public int m_matchPerson5;
    public int m_baoji1;//比赛1档爆机次数
    public int m_baoji2;
    public int m_baoji3;
    public int m_baoji4;
    public int m_baoji5;
    public double m_matchTime;//比赛平均时间
    public int m_personCount;//参与人数
    public int m_activeCount;//活跃人数
    public double m_winRate;//玩家胜率
}
//爆金比赛场
public class QueryFishlordBaojinStat : QueryBase
{
    List<FishlordBaojinStatItem> m_result = new List<FishlordBaojinStatItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);
        queryCond.addImq(Query.EQ("roomId", 11));
        IMongoQuery imq = queryCond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_BAOJIN_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_BAOJIN_PLAYER, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishlordBaojinStatItem tmp = new FishlordBaojinStatItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if(data.ContainsKey("joinCount"))
            {
                tmp.m_joinCount = Convert.ToInt32(data["joinCount"]);
                if(data.ContainsKey("winCount")&&tmp.m_joinCount!=0)
                {
                    tmp.m_winRate = Math.Round((Convert.ToInt32(data["winCount"])*1.0/tmp.m_joinCount),2);
                }
                tmp.m_ticketIncome = Convert.ToInt64(tmp.m_joinCount*200000);
                tmp.m_matchTime = Math.Round((Convert.ToInt32(data["matchTime"])*1.0/tmp.m_joinCount),2);
            }

            if (data.ContainsKey("giveoutGold"))
            {
                tmp.m_giveOutGold = Convert.ToInt64(data["giveoutGold"]);
            }

            if (data.ContainsKey("matchPerson_1")) 
            {
                tmp.m_matchPerson1 = Convert.ToInt32(data["matchPerson_1"]);
            }
            if (data.ContainsKey("matchPerson_2"))
            {
                tmp.m_matchPerson2 = Convert.ToInt32(data["matchPerson_2"]);
            }
            if (data.ContainsKey("matchPerson_3"))
            {
                tmp.m_matchPerson3 = Convert.ToInt32(data["matchPerson_3"]);
            }
            if (data.ContainsKey("matchPerson_4"))
            {
                tmp.m_matchPerson4 = Convert.ToInt32(data["matchPerson_4"]);
            }
            if (data.ContainsKey("matchPerson_5"))
            {
                tmp.m_matchPerson5 = Convert.ToInt32(data["matchPerson_5"]);
            }

            if (data.ContainsKey("baoji_1")) 
            {
                tmp.m_baoji1 = Convert.ToInt32(data["baoji_1"]);
            }
            if (data.ContainsKey("baoji_2"))
            {
                tmp.m_baoji2 = Convert.ToInt32(data["baoji_2"]);
            }
            if (data.ContainsKey("baoji_3"))
            {
                tmp.m_baoji3 = Convert.ToInt32(data["baoji_3"]);
            }
            if (data.ContainsKey("baoji_4"))
            {
                tmp.m_baoji4 = Convert.ToInt32(data["baoji_4"]);
            }
            if (data.ContainsKey("baoji_5"))
            {
                tmp.m_baoji5 = Convert.ToInt32(data["baoji_5"]);
            }
            if (data.ContainsKey("personCount")) 
            {
                tmp.m_personCount = Convert.ToInt32(data["personCount"]);
            }
            
            //活跃人数  构造时间条件
            DateTime mint_1 = Convert.ToDateTime(tmp.m_time);
            DateTime maxt_1 = mint_1.AddDays(1).AddSeconds(-1);
            IMongoQuery imq_time = Query.And(Query.LTE("genTime", BsonValue.Create(maxt_1)), Query.GTE("genTime", BsonValue.Create(mint_1)));
            int accServerId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            List<Dictionary<string, object>> activeList =
                 DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, accServerId,
                 DbName.DB_ACCOUNT, imq_time, 0, 0, new string[] { "activeCount" });
            if (activeList != null)
            {
                foreach (var d in activeList)
                {
                    if (d.ContainsKey("activeCount"))
                    {
                        tmp.m_activeCount += Convert.ToInt32(d["activeCount"]);
                    }
                }
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//爆金排行榜
public class FishlordBaojinRankItem 
{
    public string m_time;
    public int m_rank;
    public int m_playerId;
    public int m_maxScore;
    public int m_joinCount;
    public int m_baoji_1;
    public int m_baoji_2;
    public int m_baoji_3;
    public int m_baoji_4;
    public int m_baoji_5;
    public int m_fishLevel;
    public int m_vipLevel=0;
    public string m_room;
    public string m_nickName;

    public string getExParam(int index)
    {
        if (m_playerId > 0)
        {
            URLParam uParam = new URLParam();
            uParam.m_text = "详情";
            uParam.m_key = "playerId";
            uParam.m_value = m_playerId.ToString();
            uParam.m_url = DefCC.ASPX_GAME_DETAIL_BAOJIN;
            uParam.m_target = "_blank";
            uParam.addExParam("index", index);
            return Tool.genHyperlink(uParam);
        }
        return "";
    }
    public long m_totalRecharge;
    public int m_weekChampionCount;
    public int m_weekTop10Count;
    public bool m_isRobot = false;
}
public class QueryFishlordBaojinRank : QueryBase
{
    List<FishlordBaojinRankItem> m_result = new List<FishlordBaojinRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        int rankType = Convert.ToInt32(p.m_param);
        int actType=Convert.ToInt32(p.m_showWay);
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        OpRes res_1 = OpRes.op_res_failed;
        if (rankType == 0) //日排行榜
        {
            switch(actType)
            {
                case 0: res_1= dailyCurrQuery(user); break;//当前活动
                case 1: res_1 = historyRankQuery(p, user, TableName.STAT_FISHLORD_BAOJIN_DAILY_RANK); break;//历史活动
            }
        }
        if (rankType == 1) //周排行榜
        {
            switch (actType)
            {
                case 0: res_1 = weekCurrQuery(user); break;//当前活动
                case 1: res_1 = historyRankQuery(p, user, TableName.STAT_FISHLORD_BAOJIN_WEEK_RANK); break;//历史活动
            }
        }
        return res_1;
    }
    int dailyComparison(Dictionary<string, object> x, Dictionary<string, object> y)
    {
        int maxScore1 = 0;
        int maxScore2 = 0;
        if (x.ContainsKey("todayMaxScore"))
        {
            maxScore1 = Convert.ToInt32(x["todayMaxScore"]);
        }
        if (y.ContainsKey("todayMaxScore"))
        {
            maxScore2 = Convert.ToInt32(y["todayMaxScore"]);
        }
        if (maxScore1 != maxScore2)
            return maxScore2 - maxScore1;

        int enter1 = int.MaxValue;
        int enter2 = int.MaxValue;
        if (x.ContainsKey("dailyEnterIndex"))
        {
            enter1 = Convert.ToInt32(x["dailyEnterIndex"]);
        }
        if (y.ContainsKey("dailyEnterIndex"))
        {
            enter2 = Convert.ToInt32(y["dailyEnterIndex"]);
        }
        return enter1 - enter2;
    }
    private OpRes dailyCurrQuery(GMUser user)
    {
        QueryCondition queryCond = new QueryCondition();
        queryCond.addImq(Query.GT("todayMaxScore", 0));
        IMongoQuery imq = queryCond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        dataList = DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_BAOJIN_RANK, dip, imq, 0, 100, null,
                    "todayMaxScore", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        dataList.Sort(dailyComparison);
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishlordBaojinRankItem tmp = new FishlordBaojinRankItem();
            
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            if (data.ContainsKey("todayMaxScore"))
            {
                tmp.m_maxScore = Convert.ToInt32(data["todayMaxScore"]);
            }
            else 
            {
                continue;
            }

            m_result.Add(tmp);

            tmp.m_rank = (i + 1);
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("fishLevel"))
            {
                var fish_level = Fish_LevelCFG.getInstance().getValue(Convert.ToInt32(data["fishLevel"]));
                if (fish_level != null)
                {
                    tmp.m_fishLevel = Convert.ToInt32(fish_level.m_openRate);
                }
            }

            if(data.ContainsKey("vipLevel"))
            {
                tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);
            }
            if (data.ContainsKey("isRobot"))
            {
                tmp.m_isRobot = Convert.ToBoolean(data["isRobot"]);
            }
            
            //QueryCondition queryCond1 = new QueryCondition();
            //DateTime mint_1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            //DateTime maxt_1 = mint_1.AddDays(1).AddSeconds(-1);
            //IMongoQuery imq_time = Query.And(Query.LT("genTime", BsonValue.Create(maxt_1)), Query.GTE("genTime", BsonValue.Create(mint_1)));
            //queryCond1.addImq(imq_time);
            //queryCond1.addImq(Query.EQ("playerId", tmp.m_playerId));
            //queryCond1.addImq(Query.EQ("roomId", 11));
            //IMongoQuery imq_1 = queryCond1.getImq();

            //string[] m_fields = { "joinCount", "baoji_1", "baoji_2", "baoji_3", "baoji_4", "baoji_5" };
            //DbInfoParam dip_1 = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
            //Dictionary<string, object> data_source =
            //    DBMgr.getInstance().getTableData(TableName.PUMP_FISH_BAOJIN_PLAYER, dip_1, imq_1, m_fields);
            //if (data_source != null)
            //{
            //    if (data_source.ContainsKey("joinCount"))
            //    {
            //        tmp.m_joinCount = Convert.ToInt32(data_source["joinCount"]);
            //    }

            //    if (data_source.ContainsKey("baoji_1"))
            //    {
            //        tmp.m_baoji_1 = Convert.ToInt32(data_source["baoji_1"]);
            //    }
            //    if (data_source.ContainsKey("baoji_2"))
            //    {
            //        tmp.m_baoji_2 = Convert.ToInt32(data_source["baoji_2"]);
            //    }
            //    if (data_source.ContainsKey("baoji_3"))
            //    {
            //        tmp.m_baoji_3 = Convert.ToInt32(data_source["baoji_3"]);
            //    }
            //    if (data_source.ContainsKey("baoji_4"))
            //    {
            //        tmp.m_baoji_4 = Convert.ToInt32(data_source["baoji_4"]);
            //    }
            //    if (data_source.ContainsKey("baoji_5"))
            //    {
            //        tmp.m_baoji_5 = Convert.ToInt32(data_source["baoji_5"]);
            //    }
            //}
        }
        return OpRes.opres_success;
    }

    int weekComparison(Dictionary<string, object> x, Dictionary<string, object> y)
    {
        int maxScore1 = 0;
        int maxScore2 = 0;
        if (x.ContainsKey("weekMaxScore"))
        {
            maxScore1 = Convert.ToInt32(x["weekMaxScore"]);
        }
        if (y.ContainsKey("weekMaxScore"))
        {
            maxScore2 = Convert.ToInt32(y["weekMaxScore"]);
        }
        if (maxScore1 != maxScore2)
            return maxScore2 - maxScore1;

        int enter1 = int.MaxValue;
        int enter2 = int.MaxValue;
        if (x.ContainsKey("weekEnterIndex"))
        {
            enter1 = Convert.ToInt32(x["weekEnterIndex"]);
        }
        if (y.ContainsKey("weekEnterIndex"))
        {
            enter2 = Convert.ToInt32(y["weekEnterIndex"]);
        }
        return enter1 - enter2;
    }
    private OpRes weekCurrQuery(GMUser user) 
    {
        QueryCondition queryCond = new QueryCondition();
        queryCond.addImq(Query.GT("weekMaxScore", 0));
        IMongoQuery imq = queryCond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        dataList = DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_BAOJIN_RANK, dip, imq, 0, 1, null, "weekMaxScore", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        dataList.Sort(weekComparison);
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishlordBaojinRankItem tmp = new FishlordBaojinRankItem();
            
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            if (data.ContainsKey("weekMaxScore"))
            {
                tmp.m_maxScore = Convert.ToInt32(data["weekMaxScore"]);
            }
            else 
            {
                continue;
            }

            m_result.Add(tmp);

            tmp.m_rank = (i + 1);
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("fishLevel"))
            {
                var fish_level = Fish_LevelCFG.getInstance().getValue(Convert.ToInt32(data["fishLevel"]));
                if (fish_level != null)
                {
                    tmp.m_fishLevel = Convert.ToInt32(fish_level.m_openRate);
                }
            }
            if (data.ContainsKey("isRobot"))
            {
                tmp.m_isRobot = Convert.ToBoolean(data["isRobot"]);
            }
        }
        return OpRes.opres_success;
    }
    private OpRes historyRankQuery(ParamQuery p, GMUser user, string table)
    {
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);
        IMongoQuery imq = queryCond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(table, imq, dip);
        List<Dictionary<string, object>> dataList =
            DBMgr.getInstance().executeQuery(table, dip, imq, (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        //先genTime 降序 后rank升序
        var dataList_1 = dataList.OrderByDescending(a => a["genTime"]).ThenBy(a => a["rank"]).ToList();
        for (int i = 0; i < dataList_1.Count; i++)
        {
            Dictionary<string, object> data = dataList_1[i];
            FishlordBaojinRankItem tmp = new FishlordBaojinRankItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_rank = Convert.ToInt32(data["rank"]);
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if(data.ContainsKey("totalRecharge"))
            {
                tmp.m_totalRecharge = Convert.ToInt64(data["totalRecharge"]);
            }
            if(data.ContainsKey("weekTop10Count"))
            {
                tmp.m_weekTop10Count = Convert.ToInt32(data["weekTop10Count"]);
            }
            if(data.ContainsKey("weekChampionCount"))
            {
                tmp.m_weekChampionCount = Convert.ToInt32(data["weekChampionCount"]);
            }
            if(data.ContainsKey("maxScore"))
            {
                tmp.m_maxScore = Convert.ToInt32(data["maxScore"]);
            }
            if(data.ContainsKey("vipLevel"))
            {
                tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);
            }
            if(data.ContainsKey("fishLevel"))
            {
                var fish_level = Fish_LevelCFG.getInstance().getValue(Convert.ToInt32(data["fishLevel"]));
                if (fish_level != null)
                {
                    tmp.m_fishLevel = Convert.ToInt32(fish_level.m_openRate);
                }
            }
            if (data.ContainsKey("isRobot"))
            {
                tmp.m_isRobot = Convert.ToBoolean(data["isRobot"]);
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//爆金排行榜详情
public class QueryFishlordBaojinRankDetail : QueryBase 
{
    List<FishlordBaojinRankItem> m_result = new List<FishlordBaojinRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime maxt = DateTime.Now;
        DateTime mint = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        string dt = DateTime.Now.DayOfWeek.ToString();
        switch(dt)
        {
            case "Thursday": break; //周四
            case "Friday": mint = mint.AddDays(-1); break;
            case "Saturday": mint = mint.AddDays(-2); break;
            case "Sunday": mint = mint.AddDays(-3); break;
            case "Monday": mint = mint.AddDays(-4); break;
            case "Tuesday": mint = mint.AddDays(-5); break;
            case "Wednesday": mint = mint.AddDays(-6); break;//周三
        }

        QueryCondition queryCond = new QueryCondition();
        queryCond.addImq(Query.And(Query.LT("genTime", BsonValue.Create(maxt)), Query.GTE("genTime", BsonValue.Create(mint))));
        queryCond.addImq(Query.EQ("playerId",Convert.ToInt32(p.m_param)));
        queryCond.addImq(Query.EQ("roomId", 11));
        IMongoQuery imq = queryCond.getImq();

        string[] m_fields = { "playerId","roomId","genTime","joinCount", "baoji_1", "baoji_2", "baoji_3", "baoji_4", "baoji_5"};
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_FISH_BAOJIN_PLAYER, dip, imq, 0, 0, m_fields, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishlordBaojinRankItem tmp = new FishlordBaojinRankItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            }
            if(Convert.ToInt32(data["roomId"])==11)
            {
                tmp.m_room ="富豪竞技场";
            }
            
            if (data.ContainsKey("joinCount"))
            {
                tmp.m_joinCount = Convert.ToInt32(data["joinCount"]);
            }

            if (data.ContainsKey("baoji_1"))
            {
                tmp.m_baoji_1 = Convert.ToInt32(data["baoji_1"]);
            }
            if (data.ContainsKey("baoji_2"))
            {
                tmp.m_baoji_2 = Convert.ToInt32(data["baoji_2"]);
            }
            if (data.ContainsKey("baoji_3"))
            {
                tmp.m_baoji_3 = Convert.ToInt32(data["baoji_3"]);
            }
            if (data.ContainsKey("baoji_4"))
            {
                tmp.m_baoji_4 = Convert.ToInt32(data["baoji_4"]);
            }
            if (data.ContainsKey("baoji_5"))
            {
                tmp.m_baoji_5 = Convert.ToInt32(data["baoji_5"]);
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
/////////////////////////////////////////////////////////////////////////
//发放玩偶数量
public class PuppetActItem
{
    public string m_time;
    public int m_actId;
    public int m_puppetCount;
}
public class QueryPuppetActStat : QueryBase
{
    List<PuppetActItem> m_result = new List<PuppetActItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUPPET_ACT, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUPPET_ACT, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            PuppetActItem tmp = new PuppetActItem();
            m_result.Add(tmp);
            tmp.m_actId = Convert.ToInt32(data["actId"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_puppetCount = Convert.ToInt32(data["puppetCount"]);
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//玩家捐赠档位
public class PuppetRewardRecvItem 
{
    public string m_time;
    public int m_rewardId;
    public int m_rewardType;
    public int m_reachCount;
    public int m_recvCount;
}
public class QueryPuppetRewardRecvStat : QueryBase 
{
    List<PuppetRewardRecvItem> m_result = new List<PuppetRewardRecvItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);

        queryCond.addImq(imq3);
        if (p.m_param != "")
        {
            queryCond.addImq(Query.EQ("rewardType", Convert.ToInt32(p.m_param)));
        }
        IMongoQuery imq = queryCond.getImq();
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUPPET_REWARD_RECV, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUPPET_REWARD_RECV, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            PuppetRewardRecvItem tmp = new PuppetRewardRecvItem();
            m_result.Add(tmp);
            tmp.m_rewardId = Convert.ToInt32(data["rewardId"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_rewardType = Convert.ToInt32(data["rewardType"]);
            if(data.ContainsKey("reachCount"))
            {
                tmp.m_reachCount = Convert.ToInt32(data["reachCount"]);
            }
            if(data.ContainsKey("recvCount"))
            {
                tmp.m_recvCount = Convert.ToInt32(data["recvCount"]);
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//服务器总捐赠玩偶次数数量
public class PuppetSvrDonateItem 
{
    public string m_time;
    public int m_donateAmount;
    public int m_donateCount;
}
public class QueryPuppetSvrDonateStat : QueryBase
{
    List<PuppetSvrDonateItem> m_result = new List<PuppetSvrDonateItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);
        IMongoQuery imq = queryCond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUPPET_SVR_DONATE, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUPPET_SVR_DONATE, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            PuppetSvrDonateItem tmp = new PuppetSvrDonateItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if (data.ContainsKey("donateAmount"))
            {
                tmp.m_donateAmount = Convert.ToInt32(data["donateAmount"]);
            }
            if (data.ContainsKey("donateCount"))
            {
                tmp.m_donateCount = Convert.ToInt32(data["donateCount"]);
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//排行榜
public class PuppetPlayerRankItem
{
    public int m_playerId;
    public string m_nickName;
    public int m_puppetCount;
    public int m_totalRecharge;
    public string m_time;
    public int m_rank;

}
//20个捐赠玩家排行榜
public class QueryPuppetPlayerDonateRankStat : QueryBase
{
    List<PuppetPlayerRankItem> m_result = new List<PuppetPlayerRankItem>();
    public Dictionary<string, int> m_date = new Dictionary<string, int>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime currentTime = DateTime.Now;

        int key = 1704123; //集玩偶ID
        var activity_data = ActivityCFG.getInstance().getValue(key);
        DateTime startTime = DateTime.Now, endTime = DateTime.Now;
        if (activity_data != null)
        {
            startTime = Convert.ToDateTime(activity_data.m_activityStartTime);
            endTime = Convert.ToDateTime(activity_data.m_activityEndTime);
        }

        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        List<Dictionary<string, object>> dataList_1 = new List<Dictionary<string, object>>();
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        if (Convert.ToInt32(p.m_showWay) == 1) //活动期间
        {
            dataList_1 = DBMgr.getInstance().executeQuery(TableName.STAT_ACTIVITY_PUPPET, dip, null,
                        0, 20, null, "hasDonatePuppetCount", false);
        }

        if (Convert.ToInt32(p.m_showWay) == 2) //活动结束
        {
            user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_ACTIVITY_PUPPET_RANK, null, dip);
            dataList = DBMgr.getInstance().executeQuery(TableName.STAT_ACTIVITY_PUPPET_RANK, dip, null,
            (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "time", false);
            dataList_1=dataList.OrderByDescending(a => a["time"]).ThenByDescending(a => a["hasDonatePuppetCount"]).ToList();
        }
        if (dataList_1 == null || dataList_1.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        
        for (int i = 0; i < dataList_1.Count; i++)
        {
            Dictionary<string, object> data = dataList_1[i];
            PuppetPlayerRankItem tmp = new PuppetPlayerRankItem();
            m_result.Add(tmp);
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("hasDonatePuppetCount"))
            {
                tmp.m_puppetCount = Convert.ToInt32(data["hasDonatePuppetCount"]);
            }
            if (data.ContainsKey("rmb"))
            {
                tmp.m_totalRecharge = Convert.ToInt32(data["rmb"]);
            }
            if (Convert.ToInt32(p.m_showWay) == 2 && data.ContainsKey("time"))
            {
                tmp.m_time = Convert.ToDateTime(data["time"]).ToLocalTime().ToShortDateString();
                if (m_date.ContainsKey(tmp.m_time))
                {
                    m_date[tmp.m_time]++;
                }
                else 
                {
                    m_date.Add(tmp.m_time,1);
                }
                tmp.m_rank=m_date[tmp.m_time];
            }
        }

        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//20个累计获得玩偶玩家排行榜
public class QueryPuppetPlayerGainRankStat : QueryBase
{
    List<PuppetPlayerRankItem> m_result = new List<PuppetPlayerRankItem>();
    public Dictionary<string, int> m_date = new Dictionary<string, int>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime currentTime = DateTime.Now;

        int key = 1704123; //集玩偶ID
        var activity_data = ActivityCFG.getInstance().getValue(key);
        DateTime startTime = DateTime.Now, endTime = DateTime.Now;
        if (activity_data != null)
        {
            startTime = Convert.ToDateTime(activity_data.m_activityStartTime);
            endTime = Convert.ToDateTime(activity_data.m_activityEndTime);
        }

        List<Dictionary<string, object>> dataList=new List<Dictionary<string,object>>();
        List<Dictionary<string, object>> dataList_1 = new List<Dictionary<string, object>>();
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        if (Convert.ToInt32(p.m_showWay) == 1)  //活动期间
        {
            dataList = DBMgr.getInstance().executeQuery(TableName.STAT_ACTIVITY_PUPPET, dip, null,
            0, 20, null, "accGainPupperCount", false);
            dataList_1 = dataList.OrderByDescending(a => a["accGainPupperCount"]).ToList();
        }
        if (Convert.ToInt32(p.m_showWay) == 2) //活动结束
        {
            user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_ACTIVITY_PUPPET_GAIN_RANK, null, dip);
            dataList = DBMgr.getInstance().executeQuery(TableName.STAT_ACTIVITY_PUPPET_GAIN_RANK, dip, null,
            (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "time", false);
            dataList_1 = dataList.OrderByDescending(a => a["time"]).ThenByDescending(a => a["accGainPupperCount"]).ToList();
        }

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList_1.Count; i++)
        {
            Dictionary<string, object> data = dataList_1[i];
            PuppetPlayerRankItem tmp = new PuppetPlayerRankItem();
            m_result.Add(tmp);
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            if (data.ContainsKey("nickName"))
            {
                tmp.m_nickName = Convert.ToString(data["nickName"]);
            }
            if (data.ContainsKey("accGainPupperCount"))
            {
                tmp.m_puppetCount = Convert.ToInt32(data["accGainPupperCount"]);
            }
            if(data.ContainsKey("rmb"))
            {
                tmp.m_totalRecharge=Convert.ToInt32(data["rmb"]);
            }
            if (Convert.ToInt32(p.m_showWay) == 2 && data.ContainsKey("time")) 
            {
                tmp.m_time = Convert.ToDateTime(data["time"]).ToLocalTime().ToShortDateString();

                //排名
                if (m_date.ContainsKey(tmp.m_time))
                {
                    m_date[tmp.m_time]++;
                }
                else
                {
                    m_date.Add(tmp.m_time, 1);
                }
                tmp.m_rank = m_date[tmp.m_time];
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
/////////////////////////////////////////////////////////////////////////
//限时活动操作
public class QueryActivityPanicBuyingCfg : QueryBase
{
    List<ActivityPanicBuyingItem> m_result = new List<ActivityPanicBuyingItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        IMongoQuery imq = null;

        var allActivity = ActivityPanicBuyingCFG.getInstance().getAllData();  //获取节日活动文件的所有信息
        var descData = from s in allActivity
                       orderby s.Key ascending
                       select s;
        foreach (var d in descData)
        {
            int id = d.Key;
            imq = Query.EQ("actId", id);
            ActivityPanicBuyingItem tmp = new ActivityPanicBuyingItem();
            tmp.m_activityId = id;
            tmp.m_activityName = d.Value.m_activityName;
            tmp.m_maxCount = d.Value.m_maxCount;
            query(imq,user,tmp);
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(IMongoQuery imq,GMUser user,ActivityPanicBuyingItem item)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.PB_ACTIVITY_CFG, dip, imq);

        ActivityPanicBuyingItem tmp = new ActivityPanicBuyingItem();
        if (data != null)
        {
            tmp.m_activityId = Convert.ToInt32(data["actId"]);
            tmp.m_maxCount = Convert.ToInt32(data["maxCount"]);
            tmp.m_activityName = item.m_activityName;
        }
        else 
        {
            tmp = item;
        }
        m_result.Add(tmp);
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//转盘抽奖 
public class QueryDialLottery : QueryBase
{
    private ResultMaterialGiftRecharge m_result = new ResultMaterialGiftRecharge();
    private QueryCondition m_cond = new QueryCondition();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;
        m_cond.startQuery();

        OpRes code = makeQuery(param, user, m_cond);
        if (code != OpRes.opres_success)
            return code;

        IMongoQuery imq = m_cond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        queryCond.addImq(imq);
        return OpRes.opres_success;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_DIAL_LOTTERY, imq,dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_DIAL_LOTTERY, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            MaterialGiftItem item = m_result.create(t);

            foreach (var d in data)
            {
                string id = d.Key;

                if (id != "_id" && id != "genTime")
                {
                    m_result.addReason(id);  //表头
                    if (data.ContainsKey(id))
                    {
                        item.m_number[id] = Convert.ToInt32(data[id]);
                    }
                    item.m_time = t;
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//材料礼包每日购买
public class MaterialGiftItem
{
    public DateTime m_time;
    public int m_giftId;
    public Dictionary<string, int> m_number = new Dictionary<string, int>();
}
public class ResultMaterialGiftRecharge
{
    public HashSet<string> m_fields = new HashSet<string>();

    public List<MaterialGiftItem> m_result = new List<MaterialGiftItem>();

    public void addReason(string reason)
    {
        m_fields.Add(reason);
    }
    public void reset()
    {
        m_fields.Clear();
        m_result.Clear();
    }
    public MaterialGiftItem create(DateTime date)
    {
        MaterialGiftItem item = new MaterialGiftItem();
        m_result.Add(item);
        item.m_time = date;
        return item;
    }


    public string getName(int id) 
    {
        string giftName = "";
        if(id==30)
        {
            giftName="黄金材料礼包";
        }
        else if (id == 31)
        {
            giftName="钻石材料礼包";
        }
        else 
        {
            return id.ToString();
        }
        return giftName;
    }

    public string getTitleName(int key)
    {
        ItemCFGData tmp = DialLotteryItemCFG.getInstance().getValue(key);
        if (tmp != null)
        {
            ItemCFGData item = ItemCFG.getInstance().getValue(Convert.ToInt32(tmp.m_itemName));
            if (item != null)
            {
                return "奖励"+key+"("+item.m_itemName+"X"+tmp.m_itemCount+")";  //奖励id(金币X100)
            }
            else 
            {
                return "奖励"+key+"("+tmp.m_itemName+"X"+tmp.m_itemCount+")";  //奖励ID（X100）
            }
        }
        return "奖励"+key.ToString();
    }
}

public class QueryMaterialGiftRechargeStat : QueryBase
{
    private ResultMaterialGiftRecharge m_result = new ResultMaterialGiftRecharge();
    private QueryCondition m_cond = new QueryCondition();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;
        m_cond.startQuery();

        OpRes code = makeQuery(param, user, m_cond);
        if (code != OpRes.opres_success)
            return code;

        IMongoQuery imq = m_cond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);
        queryCond.addImq(imq);
        return OpRes.opres_success;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_MATERIAL_GIFT, dip, imq, 0, 0, null, "genTime",false
            /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

         for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            MaterialGiftItem item = m_result.create(t);

            foreach(var d in data)
            {
                string id = d.Key;

                if(id!="_id" && id!="genTime" && id!="giftId")
                {
                    m_result.addReason(id);  //表头
                    if (data.ContainsKey(id))
                    {
                        item.m_number[id] = Convert.ToInt32(data[id]);
                    }
                    item.m_time = t;
                    item.m_giftId = Convert.ToInt32(data["giftId"]);
                }
            }
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
//节日活动
public class FestivalActivityItem
{
    public ActivityCFGData m_cfg;
    public int m_activeCount;
    public int m_finishPerson;
}
public class QueryFestivalActivity : QueryBase
{
    List<FestivalActivityItem> m_result = new List<FestivalActivityItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        IMongoQuery imq = null;

        int accServer = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT); 

        var allActivity = ActivityCFG.getInstance().getAllData();  //获取节日活动文件的所有信息
        var descData = from s in allActivity
                       orderby s.Key ascending
                       select s;
        foreach (var d in descData)
        {
            int id = d.Key;
            imq = Query.EQ("activityId", id);
            query(imq, d.Value, accServer, user);
        }
        return OpRes.opres_success;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(IMongoQuery imq, ActivityCFGData cfg, int accServerId, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.PUMP_ACTIVITY_TEMPLATE, dip, imq);

        FestivalActivityItem tmp = new FestivalActivityItem();
        if (data != null)
        {
            if (data.ContainsKey("finishPerson"))
            {
                tmp.m_finishPerson = Convert.ToInt32(data["finishPerson"]);
            }
        }
        m_result.Add(tmp);

        tmp.m_cfg = cfg;
        
        ////////////////////////////////////////////////////
        //活跃人数  构造时间条件
        DateTime mint = Convert.ToDateTime(cfg.m_activityStartTime);
        DateTime maxt = Convert.ToDateTime(cfg.m_activityEndTime);

        IMongoQuery imq1 = Query.LTE("genTime", BsonValue.Create(maxt)); 
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery tmpImq = Query.And(imq1, imq2);

        List<Dictionary<string, object>> activeList =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, accServerId,
             DbName.DB_ACCOUNT, tmpImq, 0, 0, new string[] { "activeCount" });

        if (activeList != null)
        {
            foreach (var d in activeList)
            {
                if (d.ContainsKey("activeCount"))
                {
                    tmp.m_activeCount += Convert.ToInt32(d["activeCount"]);
                }
            }
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//话费鱼统计
public class PumpChipFishStatItem
{
    public string m_time;
    public int m_fishId;
    public long m_sysIncome;
    public long m_sysOutlay;
    public int m_reward0;
    public int m_reward1;
    public int m_reward2;
    public int m_reward3;
    public int m_reward4;
}
public class QueryPumpChipFishStat : QueryBase 
{
    List<PumpChipFishStatItem> m_result = new List<PumpChipFishStatItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);
        if(p.m_param!="")
        {
            queryCond.addImq(Query.EQ("fishId", Convert.ToInt32(p.m_param)));
        }
        IMongoQuery imq = queryCond.getImq();

        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_CHIP_FISH, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_CHIP_FISH, dip, imq,
             (p.m_curPage - 1) * p.m_countEachPage, p.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            PumpChipFishStatItem tmp = new PumpChipFishStatItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_fishId = Convert.ToInt32(data["fishId"]);

            if(data.ContainsKey("income"))
            {
                tmp.m_sysIncome = Convert.ToInt64(data["income"]);
            }

            if(data.ContainsKey("outlay"))
            {
                tmp.m_sysOutlay = Convert.ToInt64(data["outlay"]);
            }

            if(data.ContainsKey("0"))
            {
                tmp.m_reward0 = Convert.ToInt32(data["0"]);
            }
            if (data.ContainsKey("1"))
            {
                tmp.m_reward1 = Convert.ToInt32(data["1"]);
            }
            if (data.ContainsKey("2"))
            {
                tmp.m_reward2 = Convert.ToInt32(data["2"]);
            }
            if (data.ContainsKey("3"))
            {
                tmp.m_reward3 = Convert.ToInt32(data["3"]);
            }
            if (data.ContainsKey("4"))
            {
                tmp.m_reward4 = Convert.ToInt32(data["4"]);
            }
        }
        return OpRes.opres_success;
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
}
//////////////////////////////////////////////////////////////////////////
// 星星抽奖查询
public class StarLotteryItem
{
    public DateTime m_time;
    public int m_level;
    public long m_totalOutlay;
    public Dictionary<string, int> m_gift = new Dictionary<string, int>();
    public int m_lotteryCount;//抽奖次数
    public int m_lotteryNum;//抽奖人次
    public long m_correspondingGold; //对应金币
    public double m_earningRate;
}

public class ResultStarLottery
{
    public HashSet<string> m_fields = new HashSet<string>();

    public List<StarLotteryItem> m_result = new List<StarLotteryItem>();

    public void addReason(string reason)
    {
        m_fields.Add(reason);
    }
    public void reset()
    {
        m_fields.Clear();
        m_result.Clear();
    }
    public StarLotteryItem create(DateTime date)
    {
        StarLotteryItem item = new StarLotteryItem();
        m_result.Add(item);
        item.m_time = date;
        return item;
    }
    public string getTitleName(string index)
    {
        string titleName = "";
        string title = "奖励";
        switch (index)
        {
            case "index0": titleName = title + 1; break;
            case "index1": titleName = title + 2; break;
            case "index2": titleName = title + 3; break;
            case "index3": titleName = title + 4; break;
            case "index4": titleName = title + 5; break;
            case "index5": titleName = title + 6; break;
            default: titleName = index; break;
        }
        return titleName;
    }
}

public class QueryStarLottery : QueryBase
{
    private ResultStarLottery m_result = new ResultStarLottery();
    private QueryCondition m_cond = new QueryCondition();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);

        if(p.m_param!="")
        {
            imq = Query.And(imq, Query.EQ("level", Convert.ToInt32(p.m_param)));
        }
        return query(p,user, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_STAR_LOTTERY_DETAIL, imq,dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_STAR_LOTTERY_DETAIL, dip, 
             imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            StarLotteryItem tmp = m_result.create(t);

            tmp.m_level = Convert.ToInt32(data["level"]);
            tmp.m_totalOutlay = Convert.ToInt64(data["totalOutlay"]);

            tmp.m_lotteryCount = Convert.ToInt32(data["lotteryCount"]);
            tmp.m_lotteryNum = Convert.ToInt32(data["personCount"]);
            tmp.m_correspondingGold = Convert.ToInt64(data["correspondingGold"]);
            if (tmp.m_totalOutlay != 0)
            {
                tmp.m_earningRate = Math.Round((tmp.m_totalOutlay - tmp.m_correspondingGold) / (1.0 * tmp.m_totalOutlay), 3);
            }
            else
            {
                tmp.m_earningRate = 0;
            }
            foreach (var d in data)
            {
                int index = d.Key.IndexOf("index");
                string id=d.Key;
                if(index!=-1)
                {
                    m_result.addReason(id);  //表头
                    if (data.ContainsKey(id))
                    {
                        tmp.m_gift[id] = Convert.ToInt32(data[id]);
                    }
                }
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class RLoseItem
{
    public int m_playerId;
    public string m_nickName;
    public int m_vipLevel;
    public int m_gold;
    public int m_gem;
    public int m_dragonBall;
    public string m_lastLoginTime;
}

public class QueryRLose : QueryBase
{
    private List<RLoseItem> m_result = new List<RLoseItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PLAYER,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.RLOSE, dip);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        string[] fields = { "logout_time" };
        for (i = 0; i < dataList.Count; i++)
        {
            RLoseItem tmp = new RLoseItem();
            m_result.Add(tmp);

            Dictionary<string, object> data = dataList[i];
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            tmp.m_nickName = Convert.ToString(data["nickName"]);
            tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);
            tmp.m_gold = Convert.ToInt32(data["gold"]);
            tmp.m_gem = Convert.ToInt32(data["gem"]);
            tmp.m_dragonBall = Convert.ToInt32(data["dragonBall"]);

            Dictionary<string, object> pd = QueryBase.getPlayerProperty(tmp.m_playerId, user, fields);
            if (pd != null)
            {
                tmp.m_lastLoginTime = Convert.ToDateTime(pd["logout_time"]).ToLocalTime().ToString();
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ParamDragonBallDaily : ParamQuery
{
    public string m_discount;
    public string m_eachValue;
}

public class DragonBallDailyItem : StatDragonDailyItem
{
    public double m_rmb;
}

// 每日龙珠
public class QueryDragonBallDaily : QueryBase
{
    private List<DragonBallDailyItem> m_result = new List<DragonBallDailyItem>();
    QueryCondition m_cond = new QueryCondition();
    double m_discount = 1, m_eachValue = 1;

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_cond.startQuery();
        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        return query(user, m_discount, m_eachValue, m_cond.getImq());
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition cond)
    {
        ParamDragonBallDaily p = (ParamDragonBallDaily)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        if (cond.isExport())
        {
            cond.addQueryCond("time", p.m_time);
        }
        else
        {
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            cond.addImq(Query.And(imq1, imq2));
        }

        if (!string.IsNullOrEmpty(p.m_discount))
        {
            if (!double.TryParse(p.m_discount, out m_discount))
            {
                return OpRes.op_res_param_not_valid;
            }

            if (cond.isExport())
            {
                cond.addQueryCond("discount", p.m_discount);
            }
        }
        else
        {
            m_discount = 0.45;
        }
        if (!string.IsNullOrEmpty(p.m_eachValue))
        {
            if (!double.TryParse(p.m_eachValue, out m_eachValue))
            {
                return OpRes.op_res_param_not_valid;
            }

            if (cond.isExport())
            {
                cond.addQueryCond("eachValue", p.m_eachValue);
            }
        }
        else
        {
            m_eachValue = 0.12;
        }

        return OpRes.opres_success;
    }

    private OpRes query(GMUser user, double discount, double eachValue, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =DBMgr.getInstance().executeQuery(TableName.STAT_DRAGON_DAILY, dip, imq);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            DragonBallDailyItem tmp = new DragonBallDailyItem();
            m_result.Add(tmp);

            Dictionary<string, object> data = dataList[i];
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_todayRecharge = Convert.ToInt32(data["todayRecharge"]);
            tmp.m_dragonBallGen = Convert.ToInt64(data["dragonBallGen"]);
            tmp.m_dragonBallConsume = Convert.ToInt64(data["dragonBallConsume"]);
            tmp.m_dragonBallRemain = tmp.m_dragonBallGen - tmp.m_dragonBallConsume;
            tmp.m_rmb = tmp.m_todayRecharge * discount - (tmp.m_dragonBallRemain) * eachValue;
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class RechargePlayerMonitorItem : RechargePlayerMonitorItemBase
{
    public string m_channel;

    public string getRechargePoint(int payId)
    {
        return ResultRechargePointStat.getPayName(payId);
    }

    public string getOpenRate(int level)
    {
        Fish_LevelCFGData data = Fish_LevelCFG.getInstance().getValue(level);
        if (data != null)
        {
            return data.m_openRate.ToString();
        }

        return level.ToString();
    }
}

// 付费玩家监控
public class QueryRechargePlayerMonitor : QueryBase
{
    static string[] FIELD_FISH_LEVEL = { "Level" };
    static string[] FIELD_PLAYER = { "recharged", "dragonBall", "GainDragonBallCount", "SendDragonBallCount", "create_time" };
    QueryCondition m_cond = new QueryCondition();
    private List<RechargePlayerMonitorItem> m_result = new List<RechargePlayerMonitorItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_cond.startQuery();
        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        IMongoQuery imq = m_cond.getImq();

        ParamQuery p = (ParamQuery)param;
        return query(user, imq, p);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition cond)
    {
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        if (p.m_channelNo != "")
        {
            if (cond.isExport())
            {
                cond.addQueryCond("channel", p.m_channelNo);
            }
            else
            {
                cond.addImq(Query.EQ("channel", p.m_channelNo));
            }
        }

        if (cond.isExport())
        {
            cond.addQueryCond("time", p.m_time);
        }
        else
        {
            IMongoQuery imq1 = Query.LT("regTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("regTime", BsonValue.Create(mint));
            cond.addImq(Query.And(imq1, imq2));
        }

        return OpRes.opres_success; 
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_RECHARGE_FIRST,imq,dip);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_RECHARGE_FIRST, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "firstRechargeTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            RechargePlayerMonitorItem tmp = new RechargePlayerMonitorItem();
            m_result.Add(tmp);

            Dictionary<string, object> data = dataList[i];
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);
            tmp.m_curFishLevel = getFishLevel(tmp.m_playerId, user);

            tmp.m_totalGameTime = getTotalGameTime(tmp.m_playerId, user);

            if (data.ContainsKey("channel"))
            {
                tmp.m_channel = Convert.ToString(data["channel"]).PadLeft(6, '0');
            }
            if (data.ContainsKey("firstRechargeTime"))
            {
                tmp.m_firstRechargeTime = Convert.ToDateTime(data["firstRechargeTime"]).ToLocalTime();
            }
            if (data.ContainsKey("firstRechargeGameTime"))
            {
                tmp.m_firstRechargeGameTime = Convert.ToInt32(data["firstRechargeGameTime"]);
            }
            if (data.ContainsKey("firstRechargePoint"))
            {
                tmp.m_firstRechargePoint = Convert.ToInt32(data["firstRechargePoint"]);
            }
            if (data.ContainsKey("firstRechargeGold"))
            {
                tmp.m_firstRechargeGold = Convert.ToInt32(data["firstRechargeGold"]);
            }
            if (data.ContainsKey("firstRechargeFishLevel"))
            {
                tmp.m_firstRechargeFishLevel = Convert.ToInt32(data["firstRechargeFishLevel"]);
            }
            if (data.ContainsKey("secondRechargeTime"))
            {
                tmp.m_secondRechargeTime = Convert.ToDateTime(data["secondRechargeTime"]).ToLocalTime();
            }
            if (data.ContainsKey("secondRechargeGameTime"))
            {
                tmp.m_secondRechargeGameTime = Convert.ToInt32(data["secondRechargeGameTime"]);
            }
            if (data.ContainsKey("secondRechargePoint"))
            {
                tmp.m_secondRechargePoint = Convert.ToInt32(data["secondRechargePoint"]);
            }
            if (data.ContainsKey("secondRechargeGold"))
            {
                tmp.m_secondRechargeGold = Convert.ToInt32(data["secondRechargeGold"]);
            }
            if (data.ContainsKey("secondRechargeFishLevel"))
            {
                tmp.m_secondRechargeFishLevel = Convert.ToInt32(data["secondRechargeFishLevel"]);
            }

            setOhterInfo(tmp.m_playerId, user, tmp);
        }

        return OpRes.opres_success;
    }

    int getFishLevel(int playerId, GMUser user)
    {
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.FISHLORD_PLAYER,
            "player_id", playerId,
            FIELD_FISH_LEVEL,
            user.getDbServerID(),
            DbName.DB_GAME);

        if (data != null)
        {
            return Convert.ToInt32(data["Level"]);
        }

        return 0;
    }

    int getTotalGameTime(int playerId, GMUser user)
    {
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.STAT_PLAYER_GAME_TIME,
            "playerId", playerId,
            null,
            user.getDbServerID(),
            DbName.DB_PLAYER);

        if (data != null)
        {
            return Convert.ToInt32(data["totalGameTime"]);
        }

        return 0;
    }

    void setOhterInfo(int playerId, GMUser user, RechargePlayerMonitorItem item)
    {
        Dictionary<string, object> data = QueryBase.getPlayerProperty(playerId, user, FIELD_PLAYER);
        if (data != null)
        {
            if (data.ContainsKey("recharged"))
            {
                item.m_totalRecharge = Convert.ToInt32(data["recharged"]);
            }
            if (data.ContainsKey("dragonBall"))
            {
                item.m_remainDragon = Convert.ToInt32(data["dragonBall"]);
            }
            if (data.ContainsKey("GainDragonBallCount"))
            {
                item.m_gainDragon = Convert.ToInt64(data["GainDragonBallCount"]);
            }
            if (data.ContainsKey("SendDragonBallCount"))
            {
                item.m_sendDragon = Convert.ToInt64(data["SendDragonBallCount"]);
            }
            if (data.ContainsKey("create_time"))
            {
                item.m_regTime = Convert.ToDateTime(data["create_time"]).ToLocalTime();
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
public class DataPerHour<T> where T : struct 
{
    public DateTime m_time;
    public T[] m_data = new T[24];

    // 最小值所在索引
    public int m_minIndex;
    // 最大值所在索引
    public int m_maxIndex;

    public void init()
    {
        for (int i = 0; i < m_data.Length; i++)
        {
            m_data[i] = default(T);
        }
    }

    // index 0-23范围
    public void addData(int index, T d)
    {
        m_data[index] = d;
    }

    public T getData(int index)
    {
        return m_data[index];
    }

    public int getCount() { return m_data.Length; }

    public void calMinMax(Comparison<T> comparison)
    {
        m_minIndex = m_maxIndex = 0;
        for (int i = 1; i < m_data.Length; i++)
        {
            if (comparison(m_data[i], m_data[m_maxIndex]) > 0)
            {
                m_maxIndex = i;
            }
            if (comparison(m_data[i], m_data[m_minIndex]) < 0)
            {
                m_minIndex = i;
            }
        }
    }
}

public class DataEachDay
{
    private List<DataPerHour<int>> m_data = new List<DataPerHour<int>>();

    public void addData(DateTime t, int h, int val)
    {
        DataPerHour<int> dph = null;
        for (int i = 0; i < m_data.Count; i++)
        {
            if (m_data[i].m_time == t)
            {
                dph = m_data[i];
                break;
            }
        }

        if (dph == null)
        {
            dph = new DataPerHour<int>();
            dph.m_time = t;
            dph.init();
            m_data.Add(dph);
        }

        dph.addData(h, val);
    }

    public void reset()
    {
        m_data.Clear();
    }

    public List<DataPerHour<int>> getData()
    {
        return m_data;
    }

    public void calMaxMin()
    {
        foreach (var d in m_data)
        {
            //calMinMax(d);
            d.calMinMax((a, b) => { return a - b; });
        }
    }

    public string average(DataPerHour<int> data)
    {
        long sum = 0;
        for (int i = 0; i < data.getCount(); i++)
        {
            sum += data.getData(i);
        }
        return ItemHelp.getRate(sum, data.getCount(), 2);
    }

    void calMinMax(DataPerHour<int> data)
    {
        data.m_minIndex = data.m_maxIndex = 0;
        for (int i = 1; i < data.m_data.Length; i++)
        {
            if (data.m_data[i] > data.m_data[data.m_maxIndex])
                data.m_maxIndex = i;

            if (data.m_data[i] < data.m_data[data.m_minIndex])
                data.m_minIndex = i;
        }
    }

    public int getCount()
    {
        return m_data.Count;
    }

    public DataPerHour<int> getData(int index)
    {
        return m_data[index];
    }
}

// 每小时的付费累加
public class QueryRechargePerHour : QueryBase
{
    DataEachDay m_result = new DataEachDay();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;
        //时间1
        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_time_format_error;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq3 = Query.And(imq1,imq2);
        
        if(p.m_op==1) //只是查询表格
            return query(p, user, imq3);

        //时间2
        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_time_format_error;

        DateTime mint1 = DateTime.Now, maxt1 = DateTime.Now;
        bool res1 = Tool.splitTimeStr(p.m_param, ref mint1, ref maxt1);
        if (!res1)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq_1 = Query.LT("genTime", BsonValue.Create(maxt1));
        IMongoQuery imq_2 = Query.GTE("genTime", BsonValue.Create(mint1));

        IMongoQuery imq_3 = Query.And(imq_1, imq_2);
        return query(p, user, imq3, imq_3);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, GMUser user, IMongoQuery imq, IMongoQuery imq1 = null)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string,object>> dataList= getStatRechargeHourData(dip,imq,param);
        
         int i = 0;
        string key = "";
        if (dataList != null && dataList.Count != 0)
        {
            for (i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];
                DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();

                for (int k = 0; k < 24; k++)
                {
                    key = "h" + k.ToString();
                    if (data.ContainsKey(key))
                    {
                        int val = Convert.ToInt32(data[key]);
                        m_result.addData(t, k, val);
                    }
                }
            }
        }
        else if (imq1 == null)
        {
            return OpRes.op_res_not_found_data;
        }

        if(imq1 == null)
            return OpRes.opres_success;

        //查询时间段
        List<Dictionary<string, object>> dataList2 = getStatRechargeHourData(dip, imq1,param);
        if (dataList2 == null || dataList2.Count == 0)
            return OpRes.op_res_not_found_data;
       
        for (i = 0; i < dataList2.Count; i++)
        {
            Dictionary<string, object> data = dataList2[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();

            for (int k = 0; k < 24; k++)
            {
                key = "h" + k.ToString();
                if (data.ContainsKey(key))
                {
                    int val = Convert.ToInt32(data[key]);
                    m_result.addData(t, k, val);
                }
            }
        }

        return OpRes.opres_success;
    }

    public List<Dictionary<string,object>> getStatRechargeHourData(DbInfoParam dip,IMongoQuery imq,ParamQuery param) 
    {
        string table = TableName.STAT_RECHARGE_HOUR;
        if(param.m_channelNo!="")
            table = TableName.STAT_RECHARGE_HOUR_BYCHANNEL;

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(table, dip, imq, 0, 0, null, "genTime", false);
        return dataList;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultOnlinePlayerNumPerHour
{
    // 游戏id->每天的数据
    public Dictionary<int, DataEachDay> m_data = new Dictionary<int, DataEachDay>();

    public void addData(int gameId, DateTime t, int h, int val)
    {
        DataEachDay d = null;
        if (m_data.ContainsKey(gameId))
        {
            d = m_data[gameId];
        }
        else
        {
            d = new DataEachDay();
            m_data.Add(gameId, d);
        }

        d.addData(t, h, val);
    }

    public void reset()
    {
        m_data.Clear();
    }

    public string toJson()
    {
        string str = "";
        Dictionary<string, object> ret = new Dictionary<string, object>();
        foreach(var d in m_data)
        {
            List<Dictionary<string, object>> dList = new List<Dictionary<string, object>>();
            addTodayData(d.Value, dList);
            addYesterdayData(d.Value, dList);
            add7dayData(d.Value, dList);
            add30dayData(d.Value, dList);

            ret.Add(d.Key.ToString(), dList);
        }
        str = ItemHelp.genJsonStr(ret);
        return str;
    }

    void addTodayData(DataEachDay data, List<Dictionary<string, object>> dList)
    {
        int count = data.getCount();
        if (count > 0)
        {
            var s = string.Join<int>(",", data.getData(0).m_data);
            Dictionary<string, object> tmp = new Dictionary<string, object>();
            tmp.Add("onlineList", s);
            dList.Add(tmp);
        }
    }

    void addYesterdayData(DataEachDay data, List<Dictionary<string, object>> dList)
    {
        int count = data.getCount();
        if (count > 1)
        {
            var s = string.Join<int>(",", data.getData(1).m_data);
            Dictionary<string, object> tmp = new Dictionary<string, object>();
            tmp.Add("onlineList", s);
            dList.Add(tmp);
        }
    }

    void add7dayData(DataEachDay data, List<Dictionary<string, object>> dList)
    {
        int count = data.getCount();
        if (count > 7)
        {
            List<double> res = ave(data, 7);
            var s = string.Join<double>(",", res);
            Dictionary<string, object> tmp = new Dictionary<string, object>();
            tmp.Add("onlineList", s);
            dList.Add(tmp);
        }
    }
    
    void add30dayData(DataEachDay data, List<Dictionary<string, object>> dList)
    {
        int count = data.getCount();
        if (count > 30)
        {
            List<double> res = ave(data, 30);
            var s = string.Join<double>(",", res);
            Dictionary<string, object> tmp = new Dictionary<string, object>();
            tmp.Add("onlineList", s);
            dList.Add(tmp);
        }
    }

    List<double> ave(DataEachDay data, int days)
    {
        List<double> res = new List<double>();
        int sum = 0;
        for (int i = 0; i < 24; i++)
        {
            sum = 0;
            for (int k = 1; k <= days; k++)
            {
                sum += data.getData(k).getData(i);
            }

            double r = (double)sum / days;
            res.Add(Math.Round(r, 2));
        }

        return res;
    }
}

// 每小时的实时在线
public class QueryOnlinePlayerNumPerHour : QueryBase
{
    DataEachDay m_result = new DataEachDay();
    ResultOnlinePlayerNumPerHour m_result1 = new ResultOnlinePlayerNumPerHour();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        ParamQuery p = (ParamQuery)param;
        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_time_format_error;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);

        OpRes code = OpRes.op_res_failed;
        switch (p.m_way)
        {
            case QueryWay.by_way0:
                {
                    int id = 0;
                    if (!int.TryParse(p.m_param, out id))
                    {
                        break;
                    }
                    imq = Query.And(imq, Query.EQ("gameId", id));
                    code = query(p, imq, user);
                }
                break;
            case QueryWay.by_way1:  //实时在线折线图
                {
                    code = query1(p, imq, user);
                }
                break;
        }
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override object getQueryResult(object param, GMUser user)
    {
        return m_result1;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_ONLINE_HOUR, dip, imq,
             0, 0, null, "genTime", false
            /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0;
        string key = "";

        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();

            for (int k = 0; k < 24; k++)
            {
                key = "h" + k.ToString();
                if (data.ContainsKey(key))
                {
                    int val = Convert.ToInt32(data[key]);
                    m_result.addData(t, k, val);
                }
            }
        }

        m_result.calMaxMin();
        return OpRes.opres_success;
    }

    private OpRes query1(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        m_result1.reset();
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_ONLINE_HOUR, dip, imq, 0, 0, null, "genTime", false
            /*(param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage*/);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        int i = 0, gameId = 0, roomId = 0;
        string key = "";

        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            if (data.ContainsKey("gameId"))
            {
                gameId = Convert.ToInt32(data["gameId"]);
            }
            else
            {
                gameId = 0;
            }
            if (data.ContainsKey("roomId"))
            {
                roomId = Convert.ToInt32(data["roomId"]);
            }
            else
            {
                roomId = 0;
            }
            if (gameId == (int)GameId.fishlord && roomId > 0)
            {
                gameId = gameId * 1000 + roomId;
            }

            for (int k = 0; k < 24; k++)
            {
                key = "h" + k.ToString();
                if (data.ContainsKey(key))
                {
                    int val = Convert.ToInt32(data[key]);
                    m_result1.addData(gameId, t, k, val);
                }
            }
        }

        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//每小时实时在线折线图
public class OutPngItem 
{
    public int m_gameId;
    public int m_roomId;
    public string m_fileName;
}

public class QueryOnlinePlayerNumPerHourNew : QueryBase 
{
    static int[] FISH_ROOMS = { 1, 2, 3, 4, 5, 6};
    private List<OutPngItem> m_result = new List<OutPngItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamOnlinePerHour p = (ParamOnlinePerHour)param;
        if (string.IsNullOrEmpty(p.m_time))
            return OpRes.op_res_time_format_error;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;
        OpRes code = OpRes.op_res_failed;

        ParamGraphExport pg = new ParamGraphExport();
        pg.m_gameId = p.m_gameId;
        pg.StartTime = mint;
        pg.EndTime = maxt;

        GraphExport data = new GraphExport();
        if (p.m_gameId == (int)GameId.fishlord) //捕鱼
        {
            foreach (int roomId in FISH_ROOMS)
            {
                pg.m_roomId = roomId;
                code = data.export(pg, user);
                if (code == OpRes.opres_success)
                {
                    OutPngItem item = new OutPngItem();
                    item.m_gameId = p.m_gameId;
                    item.m_roomId = roomId;
                    item.m_fileName = pg.m_fileName;
                    m_result.Add(item);
                }
            }
        }
        else 
        {
            pg.m_roomId = 0;
            code = data.export(pg, user);
            if (code == OpRes.opres_success)
            {
                OutPngItem item = new OutPngItem();
                item.m_gameId = p.m_gameId;
                item.m_roomId = 0;
                item.m_fileName = pg.m_fileName;
                m_result.Add(item);
            }
        }
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//黑白名单
public class specilListItem
{
    public string m_playerId;
    public string m_state;
    public string getListName(int type)
    {
        string listName = "";
        switch (type)
        {
            case 0: listName = "黑名单"; break;
            case 1: listName = "白名单"; break;
        }
        return listName;
    }
}
//奔驰宝马黑白名单列表
public class QueryBzSpecilList : QueryBase 
{
    private List<specilListItem> m_result = new List<specilListItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        IMongoQuery imq = Query.Or(Query.EQ("type", 0), Query.EQ("type", 1)); //type 为 0 或 1
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.Bz_WB_LIST, dip, imq, 0, 0, null, "type", true);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            specilListItem tmp = new specilListItem();
            m_result.Add(tmp);

            tmp.m_playerId = Convert.ToString(data["playerId"]);
            tmp.m_state = tmp.getListName(Convert.ToInt32(data["type"]));
        }
        return OpRes.opres_success;
    }
}
//鳄鱼大亨黑白名单列表
public class QueryCrocodileSpecilList : QueryBase
{
    private List<specilListItem> m_result = new List<specilListItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        
        IMongoQuery imq = Query.Or(Query.EQ("type",0),Query.EQ("type",1)); //type 为 0 或 1
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CROCODILE_WB_LIST, dip, imq, 0, 0, null, "type", true);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            specilListItem tmp = new specilListItem();
            m_result.Add(tmp);

            tmp.m_playerId = Convert.ToString(data["playerId"]);
            tmp.m_state=tmp.getListName(Convert.ToInt32(data["type"]));
        }
        return OpRes.opres_success;
    }
}
//牛牛黑白名单列表
public class QueryPlayerCowCardsSpecilList : QueryBase
{
    private List<specilListItem> m_result = new List<specilListItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        
        IMongoQuery imq = Query.Or(Query.EQ("type",0),Query.EQ("type",1)); //type 为 0 或 1
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.COW_CARD_SPECIL_LIST, dip, imq, 0, 0, null, "type", true);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            specilListItem tmp = new specilListItem();
            m_result.Add(tmp);

            tmp.m_playerId = Convert.ToString(data["playerId"]);
            tmp.m_state=tmp.getListName(Convert.ToInt32(data["type"]));
        }
        return OpRes.opres_success;
    }
}

//水果机黑白名单列表
public class QueryFruitSpecilList : QueryBase
{
    private List<specilListItem> m_result = new List<specilListItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        IMongoQuery imq = Query.Or(Query.EQ("type", 0), Query.EQ("type", 1)); //type 为 0 或 1
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.FRUIT_BW_LIST, dip, imq, 0, 0, null, "type", true);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            specilListItem tmp = new specilListItem();
            m_result.Add(tmp);

            tmp.m_playerId = Convert.ToString(data["playerId"]);
            tmp.m_state = tmp.getListName(Convert.ToInt32(data["type"]));
        }
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//黑红梅方黑白名单列表
public class QueryPlayerShcdCardsSpecilList : QueryBase
{
    private List<specilListItem> m_result = new List<specilListItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        
        IMongoQuery imq = Query.NE("type",0); //type 为 1 或 2
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.SHCD_CARD_SPECIL_LIST, dip, imq, 0, 0, null, "type", true);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            specilListItem tmp = new specilListItem();
            m_result.Add(tmp);

            tmp.m_playerId = Convert.ToString(data["player_id"]);
            tmp.m_state=tmp.getListName(Convert.ToInt32(data["type"])-1);
        }
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//黑红梅方杀分放分LOG列表
public class killSendScoreCtrlListItem 
{
    public string m_cardId;
    public string m_time;
    public string m_playerId;
    public string m_type;
    public int m_roomId;
    public string getOpType(int type)
    {
        if (type != 0)
        {
            string typeName = "";
            switch (type)
            {
                case 1: typeName = "杀分"; break;
                case 2: typeName = "放分"; break;
            }
            return typeName;
        }
        return "";
    }
}
public class QueryShcdCardsCtrlList : QueryBase
{
    private List<killSendScoreCtrlListItem> m_result = new List<killSendScoreCtrlListItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);

        if (!string.IsNullOrEmpty(p.m_param))
        {
            if (p.m_op == 0)  //牌局ID
            {
                Match match = Regex.Match(p.m_param, @"\s*\d+\s*$");
                if (!match.Success)
                {
                    return OpRes.op_res_param_not_valid;
                }
                queryCond.addImq(Query.EQ("id", p.m_param));
            }
            else  //玩家ID
            {
                int playerId = 0;
                if (!int.TryParse(p.m_param, out playerId))
                {
                    return OpRes.op_res_param_not_valid;
                }
                queryCond.addImq(Query.EQ("playerId", playerId));
            }
        }
        else
        {
            return OpRes.op_res_param_not_valid;
        }
        IMongoQuery imq = queryCond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.SHCD_CARDS_CTRL_LIST, imq, user.getDbServerID(),DbName.DB_PUMP);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.SHCD_CARDS_CTRL_LIST, dip, imq,
                (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            killSendScoreCtrlListItem tmp = new killSendScoreCtrlListItem();
            m_result.Add(tmp);
            tmp.m_playerId = Convert.ToString(data["playerId"]);
            tmp.m_cardId = Convert.ToString(data["id"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();
            tmp.m_type = tmp.getOpType(Convert.ToInt32(data["type"]));
            tmp.m_roomId = Convert.ToInt32(data["roomId"]);
        }
        return OpRes.opres_success;
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//牛牛杀分放分LOG记录列表
public class QueryCowsCardCtrlList : QueryBase
{
    private List<killSendScoreCtrlListItem> m_result = new List<killSendScoreCtrlListItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        queryCond.addImq(imq3);

        if (!string.IsNullOrEmpty(p.m_param))
        {
            if (p.m_op == 0)  //牌局ID
            {
                Match match = Regex.Match(p.m_param, @"\s*\d+\s*$");
                if (!match.Success)
                {
                    return OpRes.op_res_param_not_valid;
                }
                queryCond.addImq(Query.EQ("id",p.m_param));
            }
            else  //玩家ID
            {
                int playerId = 0;
                if (!int.TryParse(p.m_param, out playerId))
                {
                    return OpRes.op_res_param_not_valid;
                }
                queryCond.addImq(Query.EQ("playerId",playerId));
            }
        }
        else 
        {
            return OpRes.op_res_param_not_valid;
        }

        IMongoQuery imq = queryCond.getImq();
        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.COWS_CARD_CTRL_LIST, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.COWS_CARD_CTRL_LIST, dip, imq,
                (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            killSendScoreCtrlListItem tmp = new killSendScoreCtrlListItem();
            m_result.Add(tmp);
            tmp.m_playerId = Convert.ToString(data["playerId"]);
            tmp.m_cardId = Convert.ToString(data["id"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();
            tmp.m_type = tmp.getOpType(Convert.ToInt32(data["type"])+1);
            tmp.m_roomId = Convert.ToInt32(data["roomId"]);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//玩家基本信息
public class playerBasicInfo 
{
    public int m_playerId;
    public string m_nickname;
    public string m_channel;
    public int m_playerLv;
    public int m_turretLv;
    public int m_vipLv;
    public int m_gold;
    public int m_diamond;
    public int m_maxGold;
    public int m_maxDiamond;
    public int m_totalRecharge;
    public int m_rechargeCount;
    public int m_maxRecharge;
    public string m_lastRechargeTime;
    public string m_lastLogoutTime;
    public int m_ticket;
    public string m_createTime;

    //碎片
    public long m_torpedoChip;

    public int m_item17;
    public int m_item5;
    public int m_item8;
    public int m_item9;

    public int m_useItem17;
    public int m_useItem5;
    public int m_useItem8;
    public int m_useItem9;
    public int m_hasRecvAlmsCount;

    //玩家总金币消耗
    public long m_playerGoldOutlay;

    //当前工薪
    public long m_contributionValue;
    //历史贡献
    public long m_contributeHistory;

    //充值Buff
    public long m_playerRecDominantGoldPool;
    //新手Buff
    public long m_newPlayerDominantGoldPool;

    //特殊充值Buff
    public long m_sPlayerRecDominantGoldPool;
    //特殊新手Buff
    public long m_sNewPlayerDominantGoldPool;

    public string getExParam()
    {
        if (m_playerId > 0)
        {
            URLParam uParam = new URLParam();
            uParam.m_text = "详情";
            uParam.m_key = "playerId";
            uParam.m_value = m_playerId.ToString();
            uParam.m_url = DefCC.ASPX_ACCOUNT_BEIBAO;
            uParam.m_target = "_blank";
            return Tool.genHyperlink(uParam);
        }
        return "";
    }

    public string getTurretRate() 
    {
        string turretRate = m_turretLv.ToString();
        Fish_LevelCFGData turretData = Fish_TurretLevelCFG.getInstance().getValue(m_turretLv);
        if (turretData != null)
            turretRate = turretData.m_openRate.ToString();

        return turretRate;
    }

    //获取渠道名称
    public string getChannelName()
    {
        string channelName = m_channel;

        TdChannelInfo info = TdChannel.getInstance().getValue(m_channel.PadLeft(6, '0'));
        if (info != null)
            channelName = info.m_channelName;

        return channelName;
    }
} 
public class QueryStatPlayerBasicInfo : QueryBase 
{
    private List<playerBasicInfo> m_result = new List<playerBasicInfo>();
    
    private string regex_playerId = @"\d+$";
    private string[] fields = { "player_id", "nickname","ChannelID", "PlayerLevel", "TurretLevel", 
                                  "recharged","toprecharged","rechargecount","lastrechargetime","torpedoChip",
                                  "VipLevel", "gold", "maxGold", "diamond", "maxDiamond", "ticket", "logout_time","create_time"};

    private string[] fields1 = { "player_id", "items", "PlayerGoldOutlay", "ContributionValue", "ContributeHistory", "PlayerRecDominantGoldPool", "SpPlayerRecDominantGoldPool", "NewPlayerDominantGoldPool", "SpNewPlayerDominantGoldPool" };
    private string[] fields2 = { "itemId", "roomId", "userId", "time", "value"};
    private string[] fields3 = { "hasReceiveAlmsCount"};

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //玩家ID不为空
        if (string.IsNullOrEmpty(p.m_param))
            return OpRes.op_res_need_at_least_one_cond;

        Match match = Regex.Match(p.m_param, regex_playerId);

        IMongoQuery imq = null;
        if (match.Success) 
        {
            int playerId = 0;
            if (!int.TryParse(p.m_param, out playerId))
                return OpRes.op_res_param_not_valid;

            imq = Query.EQ("player_id", playerId);
        }
        else
        { //昵称
            imq = Query.EQ("nickname", p.m_param);
        }
         
        return query(user, imq, p, fields); 
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param, string[] fields)
    {
        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_INFO, imq, user.getDbServerID(), DbName.DB_PLAYER);
        Dictionary<string, object> data =
             DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, user.getDbServerID(), DbName.DB_PLAYER, imq, fields);
        if (data == null || data.Count == 0)
            return OpRes.op_res_not_found_data;

        playerBasicInfo tmp = new playerBasicInfo();
        m_result.Add(tmp);

        if (data.ContainsKey("player_id"))
            tmp.m_playerId = Convert.ToInt32(data["player_id"]);

        if (data.ContainsKey("nickname"))
            tmp.m_nickname = Convert.ToString(data["nickname"]);

        if (data.ContainsKey("ChannelID"))
            tmp.m_channel = Convert.ToString(data["ChannelID"]).PadLeft(6, '0');

        if (data.ContainsKey("PlayerLevel"))
            tmp.m_playerLv = Convert.ToInt32(data["PlayerLevel"]);

        if (data.ContainsKey("TurretLevel"))
            tmp.m_turretLv = Convert.ToInt32(data["TurretLevel"]);

        if (data.ContainsKey("VipLevel"))
            tmp.m_vipLv = Convert.ToInt32(data["VipLevel"]);

        if (data.ContainsKey("gold"))
            tmp.m_gold = Convert.ToInt32(data["gold"]);

        if (data.ContainsKey("maxGold"))
            tmp.m_maxGold = Convert.ToInt32(data["maxGold"]);

        if (data.ContainsKey("diamond"))
            tmp.m_diamond = Convert.ToInt32(data["diamond"]);

        if (data.ContainsKey("maxDiamond"))
            tmp.m_maxDiamond = Convert.ToInt32(data["maxDiamond"]);

        if (data.ContainsKey("logout_time"))
            tmp.m_lastLogoutTime = Convert.ToDateTime(data["logout_time"]).ToLocalTime().ToString();

        if (data.ContainsKey("ticket"))
            tmp.m_ticket = Convert.ToInt32(data["ticket"]);

        if (data.ContainsKey("torpedoChip"))
            tmp.m_torpedoChip = Convert.ToInt64(data["torpedoChip"]);

        if (data.ContainsKey("create_time"))
            tmp.m_createTime = Convert.ToDateTime(data["create_time"]).ToLocalTime().ToString();

        if (data.ContainsKey("recharged"))
            tmp.m_totalRecharge = Convert.ToInt32(data["recharged"]);

        if (data.ContainsKey("toprecharged"))
            tmp.m_maxRecharge = Convert.ToInt32(data["toprecharged"]);

        if (data.ContainsKey("rechargecount"))
            tmp.m_rechargeCount = Convert.ToInt32(data["rechargecount"]);

        if (data.ContainsKey("lastrechargetime"))
            tmp.m_lastRechargeTime = Convert.ToDateTime(data["lastrechargetime"]).ToLocalTime().ToString();

        //道具
        Dictionary<string, object> data1 =
            DBMgr.getInstance().getTableData(TableName.FISHLORD_PLAYER, "player_id", tmp.m_playerId, fields1, user.getDbServerID(), DbName.DB_GAME);
        if (data1 != null)
        {
            if (data1.ContainsKey("items"))
            {
                object[] arr = (object[])data1["items"];
                for (int k = 0; k < arr.Length; k++)
                {
                    Dictionary<string, object> da = (Dictionary<string, object>)arr[k];
                    if (da.ContainsKey("item_id"))
                    {
                        int itemId = Convert.ToInt32(da["item_id"]);
                        int val = Convert.ToInt32(da["item_count"]);
                        switch (itemId)
                        {
                            case 17: tmp.m_item17 = val; break;
                            case 5: tmp.m_item5 = val; break;
                            case 8: tmp.m_item8 = val; break;
                            case 9: tmp.m_item9 = val; break;
                        }
                    }
                }
            }

            if (data1.ContainsKey("PlayerGoldOutlay"))
                tmp.m_playerGoldOutlay = Convert.ToInt64(data1["PlayerGoldOutlay"]);

            if (data1.ContainsKey("ContributionValue"))
                tmp.m_contributionValue = Convert.ToInt64(data1["ContributionValue"]);

            if (data1.ContainsKey("ContributeHistory"))
                tmp.m_contributeHistory = Convert.ToInt64(data1["ContributeHistory"]);

            if (data1.ContainsKey("PlayerRecDominantGoldPool"))
                tmp.m_playerRecDominantGoldPool = Convert.ToInt64(data1["PlayerRecDominantGoldPool"]);

            if (data1.ContainsKey("NewPlayerDominantGoldPool"))
                tmp.m_newPlayerDominantGoldPool = Convert.ToInt64(data1["NewPlayerDominantGoldPool"]);

            if (data1.ContainsKey("SpPlayerRecDominantGoldPool"))
                tmp.m_sPlayerRecDominantGoldPool = Convert.ToInt64(data1["SpPlayerRecDominantGoldPool"]);

            if (data1.ContainsKey("SpNewPlayerDominantGoldPool"))
                tmp.m_sNewPlayerDominantGoldPool = Convert.ToInt64(data1["SpNewPlayerDominantGoldPool"]);
        }

        //道具使用情况
        IMongoQuery imq_2 = Query.And(
                Query.EQ("userId", tmp.m_playerId),
                Query.Or( Query.EQ("itemId", 17), Query.EQ("itemId", 5), Query.EQ("itemId", 8), Query.EQ("itemId", 9)));
        
        List<Dictionary<string, object>> data2 =
             DBMgr.getInstance().executeQuery(TableName.FISH_CONSUME_ITEM, user.getDbServerID(), DbName.DB_PUMP, imq_2, 0, 0, fields2, "time", false);
        if (data2 != null)
        {
            for (int i = 0; i < data2.Count; i++)
            {
                if (data2[i].ContainsKey("itemId") && data2[i].ContainsKey("value"))
                {
                    int itemId = Convert.ToInt32(data2[i]["itemId"]);
                    int value = Convert.ToInt32(data2[i]["value"]);
                    switch (itemId)
                    {
                        case 17:
                            tmp.m_useItem17 += value; break;
                        case 5:
                            tmp.m_useItem5 += value; break;
                        case 8:
                            tmp.m_useItem8 += value; break;
                        case 9:
                            tmp.m_useItem9 += value; break;
                    }
                }
            }
        }

        //救济金
        Dictionary<string, object> data3 =
             DBMgr.getInstance().getTableData(TableName.FISHLORD_PLAYER, "player_id", tmp.m_playerId, fields3, user.getDbServerID(), DbName.DB_GAME);
        if (data3 != null && data3.Count!=0 && data3.ContainsKey("hasReceiveAlmsCount"))
            tmp.m_hasRecvAlmsCount = Convert.ToInt32(data3["hasReceiveAlmsCount"]);

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////
//渔场场次情况
public class fishingRoomItem
{
    public string m_time;
    public int m_roomId;
    public Dictionary<int, int> m_personCount = new Dictionary<int, int>();
    public Dictionary<int, long> m_avgTime = new Dictionary<int, long>();
    public int m_activeCount;
    public Dictionary<int, int> m_brokenCount = new Dictionary<int, int>();

    //场次名称
    public string getRoomName(int roomId)
    {
        return StrName.s_roomList[roomId];
    }

    //百分比
    public string getRate(int da1, int da2)
    {
        if (da2 == 0)
            return "1";

        double factGain = (double)da1 * 100 / da2;
        return Math.Round(factGain, 2) + "%";
    }

    //将秒转换为时分秒
    public string transTimeTohms(long avgTime)
    {
        string time_str = "";
        int h = 0, m = 0, s = 0;
        if (avgTime >= 3600)
        {
            h = Convert.ToInt32(avgTime / 3600);
            m = Convert.ToInt32((avgTime % 3600) / 60);
            s = Convert.ToInt32(avgTime % 3600 % 60);
            time_str = h + "h " + m + "m " + s + "s";
        }
        else if (avgTime >= 60)
        {
            m = Convert.ToInt32(avgTime / 60);
            s = Convert.ToInt32(avgTime % 60);
            time_str = m + "m " + s + "s";
        }
        else
        {
            time_str = avgTime + "s";
        }

        return time_str;
    }
}
public class QueryStatFishingRoomInfo : QueryBase 
{
    private List<fishingRoomItem> m_result = new List<fishingRoomItem>();
    public string[] m_fields = new string[] { "activeCount" };

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(imq, Query.EQ("channel", "allChannel"));

        return query(user, imq, p, m_fields);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param, string[] m_fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PLAYER_PLAY_TIME, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, j = 0;
        for (i = 0; i < dataList.Count; i++) 
        {
            Dictionary<string, object> data = dataList[i];

            fishingRoomItem tmp = new fishingRoomItem();
            m_result.Add(tmp);

            if (!data.ContainsKey("genTime"))
                continue;

            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = t.ToShortDateString();

            //单个场次
            if (param.m_op > 0)
            {
                string strTime = "aveTimeRoom" + param.m_op;
                long avgTime = 0;
                if (data.ContainsKey(strTime))
                    avgTime = Convert.ToInt64(data[strTime]);

                tmp.m_avgTime.Add(param.m_op, avgTime);

                string strPerson = "personRoom" + param.m_op;
                int person = 0;
                if (data.ContainsKey(strPerson))
                    person = Convert.ToInt32(data[strPerson]);

                tmp.m_personCount.Add(param.m_op, person);

                //当日该场次破产人数
                DateTime t_max = t.Date.AddDays(1);
                IMongoQuery imq_broken = Query.And(
                                            Query.GTE("genTime", t.Date),
                                            Query.LT("genTime", t_max),
                                            Query.EQ("roomid", param.m_op));
                long brokenCount = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, dip, "playerId", imq_broken);
                tmp.m_brokenCount.Add(param.m_op, Convert.ToInt32(brokenCount));

                tmp.m_roomId = param.m_op;
            }
            else  //所有场次
            {
                for (j = 1; j <= StrName.s_roomList.Count; j++) 
                {
                    string strTime = "aveTimeRoom" + j;
                    long avgTime = 0;
                    if (data.ContainsKey(strTime))
                        avgTime = Convert.ToInt64(data[strTime]);

                    tmp.m_avgTime.Add(j, avgTime);

                    string strPerson = "personRoom" + j;
                    int person = 0;
                    if (data.ContainsKey(strPerson))
                        person = Convert.ToInt32(data[strPerson]);

                    tmp.m_personCount.Add(j, person);

                    //当日该场次破产人数
                    DateTime t_max = t.Date.AddDays(1);
                    IMongoQuery imq_broken = Query.And(
                                                Query.GTE("genTime", t.Date), 
                                                Query.LT("genTime", t_max), 
                                                Query.EQ("roomid", j));

                    long brokenCount = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_PLAYER_BROKEN_INFO, dip, "playerId", imq_broken);
                    tmp.m_brokenCount.Add(j, Convert.ToInt32(brokenCount));

                    tmp.m_roomId = j;
                }
            }

            //活跃玩家人数
            IMongoQuery imq_1 = Query.EQ("genTime", t.Date);
            List<Dictionary<string, object>> dataActive = DBMgr.getInstance().executeQuery(
                TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq_1, 0, 0, m_fields, "genTime", false);
            if (dataActive != null)
            {
                for (j = 0; j < dataActive.Count; j++)
                {
                    if (dataActive[j].ContainsKey("activeCount"))
                        tmp.m_activeCount += Convert.ToInt32(dataActive[j]["activeCount"]);
                }
            }
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////
//收益数据
public class gameIncomeItem : GameStatData
{
    public DateTime m_genTime;
    public string m_channel;

    public string getARPU()
    {
        if (m_activeCount == 0)
            return "0";

        double val = (double)m_totalIncome / m_activeCount;
        return Math.Round(val, 2).ToString();
    }

    public string getARPPU()
    {
        if (m_rechargePersonNum == 0)
            return "0";

        double val = (double)m_totalIncome / m_rechargePersonNum;
        return Math.Round(val, 2).ToString();
    }

    public string getRechargeRate(int up, int down)
    {
        if (down == 0)
            return "0%";

        if (up == -1)
        {
            return "暂无";
        }
        double val = (double)up / down * 100;
        return Math.Round(val, 2).ToString() + "%";
    }

    public void reset()
    {
        m_genTime = DateTime.MinValue;
        m_channel = "";
    }
}
public class QueryOpnewGameIncome : QueryBase 
{
    private List<gameIncomeItem> m_result = new List<gameIncomeItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        IMongoQuery imq = null;

        //查询时间
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }
        // -1总体 -2全渠道
        if (p.m_param != "-1" && p.m_param !="-2") 
            imq = Query.And(imq, Query.EQ("channel", p.m_param));

        if (imq == null)
            return OpRes.op_res_need_at_least_one_cond;

        OpRes code = query(p, imq, user);
        if (p.m_param == "-1" )
        {
            sum();
        }
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.CHANNEL_TD, imq, serverId, DbName.DB_ACCOUNT);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0;
        var dataList_1 = dataList.OrderBy(a => a["channel"]).OrderByDescending(a => a["genTime"]).ToList();
        for (i = 0; i < dataList_1.Count; i++)
        {
            Dictionary<string, object> data = dataList_1[i];
            gameIncomeItem tmp = new gameIncomeItem();
            m_result.Add(tmp);

            tmp.m_genTime = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_channel = Convert.ToString(data["channel"]).PadLeft(6,'0');

            //注册
            if (data.ContainsKey("regeditCount"))
                tmp.m_regeditCount = Convert.ToInt32(data["regeditCount"]);

            //设备激活
            tmp.m_deviceActivationCount = Convert.ToInt32(data["deviceActivationCount"]);

            //设备活跃
            if (data.ContainsKey("deviceLoginCount"))
                tmp.m_deviceLoginCount = Convert.ToInt32(data["deviceLoginCount"]);

            //活跃人数
            tmp.m_activeCount = Convert.ToInt32(data["activeCount"]);

            tmp.m_totalIncome = Convert.ToInt32(data["totalIncome"]);
            tmp.m_rechargePersonNum = Convert.ToInt32(data["rechargePersonNum"]);
            tmp.m_rechargeCount = Convert.ToInt32(data["rechargeCount"]);

            if (data.ContainsKey("newAccIncome"))
            {
                tmp.m_newAccIncome = Convert.ToInt32(data["newAccIncome"]);
            }else
            {
                tmp.m_newAccIncome = -1;
            }
            if (data.ContainsKey("newAccRechargePersonNum"))
            {
                tmp.m_newAccRechargePersonNum = Convert.ToInt32(data["newAccRechargePersonNum"]);
            }
            else
            {
                tmp.m_newAccRechargePersonNum = -1;
            }
        }
        return OpRes.opres_success;
    }

     //选择全部时，进行总和计算
    void sum()
    {
        List<gameIncomeItem> dataList = new List<gameIncomeItem>();

        foreach (var item in m_result)
        {
            gameIncomeItem res = findSameResult(dataList, item.m_genTime);
            res.m_totalIncome += item.m_totalIncome;
            //注册
            res.m_regeditCount += item.m_regeditCount;
            //设备激活
            res.m_deviceActivationCount += item.m_deviceActivationCount;
            //设备活跃
            res.m_deviceLoginCount += item.m_deviceLoginCount;
            //活跃人数
            res.m_activeCount += item.m_activeCount;
            res.m_rechargePersonNum += item.m_rechargePersonNum;
            res.m_rechargeCount += item.m_rechargeCount;

            if (item.m_newAccIncome > 0)
                res.m_newAccIncome += item.m_newAccIncome;

            if (item.m_newAccRechargePersonNum > 0)
            res.m_newAccRechargePersonNum += item.m_newAccRechargePersonNum;
        }

        m_result = dataList;
    }

    gameIncomeItem findSameResult(List<gameIncomeItem> data, DateTime time)
    {
        gameIncomeItem res = null;
        foreach (var d in data)
        {
            if (time == d.m_genTime)
            {
                res = d;
                break;
            }
        }
        if (res == null)
        {
            res = new gameIncomeItem();
            res.reset();
            res.m_genTime = time;
            data.Add(res);
        }
        return res;
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////
//活跃数据
public class QueryOpnewGameActive : QueryBase 
{
    private List<gameIncomeItem> m_result = new List<gameIncomeItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        IMongoQuery imq = null;

        //查询时间
        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
        }
        // -1总体 -2全渠道
        if (p.m_param != "-1" && p.m_param != "-2")
            imq = Query.EQ("channel", p.m_param);

        if (imq == null)
            return OpRes.op_res_need_at_least_one_cond;

        OpRes code = query(p, imq, user);
        if (p.m_param == "-1")
        {
            sum();
        }
        return code;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.CHANNEL_TD, imq, serverId, DbName.DB_ACCOUNT);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList_1 = dataList.OrderBy(a => a["channel"]).OrderByDescending(a => a["genTime"]).ToList();
        for (int i = 0; i < dataList_1.Count; i++)
        {
            Dictionary<string, object> data = dataList_1[i];
            gameIncomeItem tmp = new gameIncomeItem();
            m_result.Add(tmp);

            tmp.m_genTime = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_channel = Convert.ToString(data["channel"]).PadLeft(6, '0');

            //注册
            tmp.m_regeditCount = Convert.ToInt32(data["regeditCount"]);

            //设备激活
            tmp.m_deviceActivationCount = Convert.ToInt32(data["deviceActivationCount"]);

            //设备活跃
            if (data.ContainsKey("deviceLoginCount"))
                tmp.m_deviceLoginCount = Convert.ToInt32(data["deviceLoginCount"]);

            //活跃人数
            tmp.m_activeCount = Convert.ToInt32(data["activeCount"]);

            tmp.m_2DayRemainCount = Convert.ToInt32(data["2DayRemainCount"]);
            tmp.m_3DayRemainCount = Convert.ToInt32(data["3DayRemainCount"]);
            tmp.m_7DayRemainCount = Convert.ToInt32(data["7DayRemainCount"]);
            tmp.m_30DayRemainCount = Convert.ToInt32(data["30DayRemainCount"]);
        }
        return OpRes.opres_success;
    }

    //选择全部时，进行总和计算
    void sum()
    {
        List<gameIncomeItem> dataList = new List<gameIncomeItem>();

        foreach (var item in m_result)
        {
            gameIncomeItem res = findSameResult(dataList, item.m_genTime);
            res.m_regeditCount += item.m_regeditCount;
            res.m_deviceActivationCount += item.m_deviceActivationCount;
            res.m_activeCount += item.m_activeCount;

            //设备活跃
            res.m_deviceLoginCount += item.m_deviceLoginCount;

            if (item.m_2DayRemainCount > 0)
                res.m_2DayRemainCount += item.m_2DayRemainCount;

            if (item.m_3DayRemainCount > 0)
                res.m_3DayRemainCount += item.m_3DayRemainCount;

            if (item.m_7DayRemainCount > 0)
                res.m_7DayRemainCount += item.m_7DayRemainCount;

            if (item.m_30DayRemainCount > 0)
                res.m_30DayRemainCount += item.m_30DayRemainCount;
        }

        m_result = dataList;
    }

    gameIncomeItem findSameResult(List<gameIncomeItem> data, DateTime time)
    {
        gameIncomeItem res = null;
        foreach (var d in data)
        {
            if (time == d.m_genTime)
            {
                res = d;
                break;
            }
        }
        if (res == null)
        {
            res = new gameIncomeItem();
            res.reset();
            res.m_genTime = time;
            data.Add(res);
        }
        return res;
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////
//玩家充值信息 
public class PlayerRechargeItem 
{
    public int m_playerId;
    public string m_channel;
    public int m_rechargeCount;
    public int m_rechargeRmb;
    public string m_createTime;
    public int m_loginCount;
    public int m_leftGold;
    public string m_lastLoginTime;

    public string getChannelName() 
    {
        string channelName = m_channel;
        var cd = TdChannel.getInstance().getValue(m_channel.PadLeft(6, '0'));
        if (cd != null)
            channelName = cd.m_channelName;

        return channelName;
    }
}
public class QueryOpnewPlayerRecharge : QueryBase 
{
    private List<PlayerRechargeItem> m_result = new List<PlayerRechargeItem>();
    public override OpRes doQuery(object param, GMUser user) 
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //玩家ID
        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;
        int playerId = 0;
        if(!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        //查询时间
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        return query(maxt, mint, playerId, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(DateTime maxt, DateTime mint, int playerId, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
        int serverId2 = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1 || serverId2 == -1)
            return OpRes.op_res_failed;

        //玩家注册、剩余金币、最后上线时间
        string[] m_field = new string[] {"player_id", "create_time", "gold", "account", "ChannelID" };
        string[] m_field2 = new string[] { "time" };
        Dictionary<string, object> dataList =
            DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", playerId, m_field, user.getDbServerID(), DbName.DB_PLAYER);
        if (dataList == null)
            return OpRes.op_res_not_found_data;
        
        PlayerRechargeItem tmp = new PlayerRechargeItem();
        if(dataList.ContainsKey("player_id"))
            tmp.m_playerId = Convert.ToInt32(dataList["player_id"]);
        m_result.Add(tmp);

        if (dataList.ContainsKey("create_time"))
            tmp.m_createTime = Convert.ToDateTime(dataList["create_time"]).ToLocalTime().ToString();

        if (dataList.ContainsKey("ChannelID"))
            tmp.m_channel = Convert.ToString(dataList["ChannelID"]).PadLeft(6, '0');

        if (dataList.ContainsKey("gold"))
            tmp.m_leftGold = Convert.ToInt32(dataList["gold"]);

        //获取最后一次上线时间
        if (dataList.ContainsKey("account"))
        {
            string account = Convert.ToString(dataList["account"]);
            IMongoQuery imq_loginlog = Query.And(Query.GTE("time",mint),Query.LT("time",maxt), Query.EQ("acc_real", account));

            List<Dictionary<string, object>> loginlog = DBMgr.getInstance().executeQuery(
                TableName.PLAYER_LOGIN, serverId2, DbName.DB_ACCOUNT, imq_loginlog, 0, 0, null, "time", false);
            if (loginlog != null && loginlog.Count != 0)
                tmp.m_lastLoginTime = Convert.ToDateTime(loginlog[0]["time"]).ToLocalTime().ToString();
        }
        
        //玩家充值信息
        IMongoQuery imq_recharge = Query.And(
            Query.GTE("CreateTime", mint), 
            Query.LT("CreateTime", maxt),
            Query.Or(Query.EQ("status",0), Query.EQ("status", 2)), 
            Query.EQ("PlayerId", playerId)
            );
        List<Dictionary<string, object>> dataList1 =
             DBMgr.getInstance().executeQuery(TableName.RECHARGE_TOTAL, serverId, DbName.DB_PAYMENT, imq_recharge, 0, 0, null, "CreateTime", false);
        if (dataList1 != null && dataList1.Count != 0) 
        {
            tmp.m_rechargeCount = dataList1.Count;

            //充值金额
            for (int i = 0; i < dataList1.Count; i++)
            {
                if (dataList1[i].ContainsKey("RMB"))
                    tmp.m_rechargeRmb += Convert.ToInt32(dataList1[i]["RMB"]);
            }
        }

        //玩家上线次数信息
        IMongoQuery imq_login = Query.And(
            Query.GTE("genTime", mint),
            Query.LT("genTime",maxt),
            Query.EQ("playerId",playerId)
            );
        List<Dictionary<string, object>> dataList2 =
            DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_LOGIN, user.getDbServerID(), DbName.DB_PUMP, imq_login, 0, 0, null, "genTime", false);
        if (dataList2 != null)
        {
            for (int i = 0; i < dataList2.Count; i++) 
            {
                if (dataList2[i].ContainsKey("loginCount"))
                    tmp.m_loginCount += Convert.ToInt32(dataList2[i]["loginCount"]);
            }
        }
        else {
            tmp.m_loginCount = 0;
        }

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////
//系统空投
public class StatAirdropSysItem
{
    public string m_time;
    public string m_playerId;
    public int m_uuid;
    public int m_itemId;
    public int m_itemCount;
    public string m_pwd;
    public string m_state; //空投状态
    public int m_recvId;

    public int m_checkmapPerson;//打开人数
    public int m_checkmapCount;//被打开次数
    public int m_checkmapTotal;//打开次数收入总计

    //获取道具名称
    public string getItemName() 
    {
        string itemName = "";

        var item = ItemCFG.getInstance().getValue(m_itemId);
        if (item != null)
            itemName = item.m_itemName;

        return itemName;
    }
}

//系统空投发布
public class QueryStatAirDropSys : QueryBase 
{
    private List<StatAirdropSysItem> m_result = new List<StatAirdropSysItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;

        //道具
        if (p.m_op > 24)
        {
            imq = Query.EQ("itemID", p.m_op);
        }
        else {
            imq = Query.And(
                    Query.GTE("itemID", 25),
                    Query.LTE("itemID", 27)
              );
        }

        if (p.m_param == "0") //系统
        {
            imq = Query.And(imq, Query.GT("uuid", 900000));
        }
        else //玩家
        {
            imq = Query.And(imq, Query.LTE("uuid", 900000));

            //单个玩家
            if (!string.IsNullOrEmpty(p.m_playerId))
            {
                int playerId = 0;
                if (!int.TryParse(p.m_playerId, out playerId))
                    return OpRes.op_res_param_not_valid;

                imq = Query.And(imq, Query.EQ("playerId", playerId));
            }
        }

        return query(user, p, imq);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery param, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_AIR_DROP_SYS, imq, dip);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(
             TableName.STAT_AIR_DROP_SYS, dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatAirdropSysItem tmp = new StatAirdropSysItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("playerId"))
            {
                int playerId = Convert.ToInt32(data["playerId"]);
                if (playerId == 0)
                {
                    tmp.m_playerId = "系统操作";
                }
                else {
                    tmp.m_playerId = playerId.ToString();
                }
            }

            tmp.m_uuid = Convert.ToInt32(data["uuid"]);

            if (data.ContainsKey("password"))
            {
                string pwd_encryption = Convert.ToString(data["password"]);
                tmp.m_pwd = AESHelper.AESDecrypt(pwd_encryption, ConstDef.AES_FOR_AIR_DROP);
            }

            if (data.ContainsKey("itemID"))
                tmp.m_itemId = Convert.ToInt32(data["itemID"]);

            if (data.ContainsKey("itemCount"))
                tmp.m_itemCount = Convert.ToInt32(data["itemCount"]);

            if (data.ContainsKey("receiveID"))
            {
                tmp.m_recvId = Convert.ToInt32(data["receiveID"]);

                if (tmp.m_recvId != 0)
                {  //已领取
                    tmp.m_state = "已领取";
                }
                else 
                {
                    if(data.ContainsKey("endTime"))
                        tmp.m_state = "过期时间：" + Convert.ToDateTime(data["endTime"]).ToLocalTime().ToString();
                }
            }

            if (data.ContainsKey("checkmap"))
            {
                object[] arr = (object[])data["checkmap"];
                for (int k = 0; k < arr.Length; k++)
                {
                    checkmapItem checkmap = new checkmapItem();
                    Tool.parseItemFromDicNew1(arr[k] as Dictionary<string, object>, checkmap);
                    tmp.m_checkmapCount += checkmap.m_checkValue;
                }

                tmp.m_checkmapPerson = arr.Length;

                FishBaseInfoData item = FishBaseInfoCFG.getInstance().getValue("OpenGold");
                if (item != null) 
                    tmp.m_checkmapTotal = tmp.m_checkmapCount * item.m_value;
            }
        }

        return OpRes.opres_success;
    }
}
//系统空投 打开
public class StatAirdropSysOpenItem
{
    public string m_time;
    public int m_uuid;
    public int m_itemId;
    public int m_itemCount;
    public string m_pwd;

    public List<checkmapItem> m_checkmap = new List<checkmapItem>();

    //获取道具名称
    public string getItemName()
    {
        string itemName = "";

        var item = ItemCFG.getInstance().getValue(m_itemId);
        if (item != null)
            itemName = item.m_itemName;

        return itemName;
    }
}
public class QueryStatAirDropSysOpen : QueryBase 
{
    private List<StatAirdropSysOpenItem> m_result = new List<StatAirdropSysOpenItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;

        imq = Query.Exists("checkmap");

        //道具
        if (p.m_op > 24)
        {
            imq = Query.And(imq, Query.EQ("itemID", p.m_op));
        }
        else //全部
        {
            IMongoQuery imq_1 = Query.And(Query.GTE("itemID", 25), Query.LTE("itemID", 27));
            imq = Query.And(imq, imq_1);
        }

        //玩家
        if (!string.IsNullOrEmpty(p.m_playerId))
        {
            int playerId = 0;
            if (!int.TryParse(p.m_playerId, out playerId))
                return OpRes.op_res_param_not_valid;

            imq = Query.And(imq, Query.EQ("playerId", playerId));
        }

        if (p.m_param == "0") //系统
        {
            imq = Query.And(imq, Query.GT("uuid", 90000));
        }
        else {
            imq = Query.And(imq, Query.LTE("uuid", 90000));
        }

        if (Convert.ToInt32(p.m_param) == 0)//上架或领取
        {
            return query(user, p, imq, TableName.STAT_AIR_DROP_SYS, DbName.DB_PLAYER);
        }
        else
        {
            return query(user, p, imq, TableName.STAT_PUMP_AIR_DROP_HISTORY, DbName.DB_PUMP);
        }
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, ParamQuery param, IMongoQuery imq, string table, int db)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), db, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(table, imq, dip);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(
             table, dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatAirdropSysOpenItem tmp = new StatAirdropSysOpenItem();
            m_result.Add(tmp);

            object[] arr = (object[])data["checkmap"];
            for (k = 0; k < arr.Length; k++)
            {
                checkmapItem checkmap = new checkmapItem();
                Tool.parseItemFromDicNew1(arr[k] as Dictionary<string, object>, checkmap);
                tmp.m_checkmap.Add(checkmap);
            }

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();

            tmp.m_uuid = Convert.ToInt32(data["uuid"]);

            if (data.ContainsKey("password"))
            {
                string pwd_encryption = Convert.ToString(data["password"]);
                tmp.m_pwd = AESHelper.AESDecrypt(pwd_encryption, ConstDef.AES_FOR_AIR_DROP);
            }

            if (data.ContainsKey("itemID"))
                tmp.m_itemId = Convert.ToInt32(data["itemID"]);

            if (data.ContainsKey("itemCount"))
                tmp.m_itemCount = Convert.ToInt32(data["itemCount"]);
        }

        return OpRes.opres_success;
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////
//炮倍相关
public class TurretTimesItem 
{
    public string m_time;
    public int m_active;
    public int m_flag = 0;

    public Dictionary<int, int> m_turretCount = new Dictionary<int, int>();
}
public class QueryOpnewTurretTimes : QueryBase 
{
    private List<TurretTimesItem> m_result = new List<TurretTimesItem>();
    public TurretTimesItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag++;
                return d;
            }
        }

        TurretTimesItem item = new TurretTimesItem();
        m_result.Add(item);
        for (int i = 1; i <= 43; i++)
        {
            item.m_turretCount.Add(i, 0);
        }
        return item;
    }
    
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //查询时间
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq = Query.And(Query.GTE("logout_time",mint), Query.LT("logout_time",maxt));

        if (p.m_param != "-1") //总体 -1
            imq = Query.And(imq, Query.EQ("ChannelID", Convert.ToInt32(p.m_param)));

        string[] fields = new string[] { "logout_time", "ChannelID", "TurretLevel"};

        return query(imq, user, p, fields);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(IMongoQuery imq, GMUser user, ParamQuery param, string[] fields)
    {

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PLAYER_INFO, user.getDbServerID(), DbName.DB_PLAYER, imq, 0, 0, fields, "logout_time", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++) 
        {
            if (!dataList[i].ContainsKey("logout_time") || !dataList[i].ContainsKey("TurretLevel"))
                continue;

            DateTime t = Convert.ToDateTime(dataList[i]["logout_time"]).ToLocalTime();
            string time = t.ToShortDateString();

            TurretTimesItem tmp = IsCreate(time);
            tmp.m_time = time;

            //炮倍人数
            int turretLevel = Convert.ToInt32(dataList[i]["TurretLevel"]);
            if(tmp.m_turretCount.ContainsKey(turretLevel))
                tmp.m_turretCount[turretLevel] += 1;

            if(tmp.m_flag == 0) //活跃人数
            {
                 IMongoQuery imq_active = Query.EQ("genTime", t.Date);

                if (param.m_param != "-1") { 
                    imq_active = Query.And(imq_active, Query.EQ("channel", param.m_param));
                }
               
                List<Dictionary<string, object>> activeList =
                                DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq_active, 0, 0, null, "logout_time", false);
                if (activeList != null && activeList.Count != 0) 
                {
                    for (int j = 0; j < activeList.Count; j++) 
                    {
                        int active = Convert.ToInt32(activeList[j]["activeCount"]);
                        tmp.m_active += active;
                    }
                }
            }
        }

        return OpRes.opres_success;
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
//后台大厅 当日信息
public class TodayInfoItem 
{
    public int m_totalIncome;
    public int m_activeCount;
    public int m_rechargePersonNum;

    public string getARPU()
    {
        if (m_activeCount == 0)
            return "0";

        double val = (double)m_totalIncome / m_activeCount;
        return Math.Round(val, 2).ToString();
    }

    public string getARPPU()
    {
        if (m_rechargePersonNum == 0)
            return "0";

        double val = (double)m_totalIncome / m_rechargePersonNum;
        return Math.Round(val, 2).ToString();
    }

    public string getRechargeRate()
    {
        if (m_activeCount == 0)
            return "0%";

        if (m_rechargePersonNum == -1)
            return "暂无";

        double val = (double)m_rechargePersonNum / m_activeCount * 100;
        return Math.Round(val, 2).ToString() + "%";
    }
}
public class QueryStatTodayInfo : QueryBase 
{
    private List<TodayInfoItem> m_result = new List<TodayInfoItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        //查询时间
        DateTime mint = DateTime.Now.Date, maxt = DateTime.Now.Date;

        IMongoQuery imq = Query.EQ("genTime", mint);

        return query(imq,user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(IMongoQuery imq, GMUser user)
    {

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        TodayInfoItem tmp = new TodayInfoItem();
        m_result.Add(tmp);
        //注册信息
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.CHANNEL_TD, serverId, DbName.DB_ACCOUNT, imq, 0, 0, null, "genTime", false);

        if (dataList != null && dataList.Count != 0)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];

                if (data.ContainsKey("totalIncome"))
                    tmp.m_totalIncome += Convert.ToInt32(data["totalIncome"]);

                if (data.ContainsKey("activeCount"))
                    tmp.m_activeCount += Convert.ToInt32(data["activeCount"]);

                if (data.ContainsKey("rechargePersonNum"))
                    tmp.m_rechargePersonNum += Convert.ToInt32(data["rechargePersonNum"]);
            }
        }
        return OpRes.opres_success;
    }
}
////////////////////////////////////////////////////////////////////////////////////
//巨鲨场玩法收入统计
public class StatSharkRoomItem 
{
    public string m_time;
    public Dictionary<int, int[]> m_dataIndex = new Dictionary<int, int[]>();
    public long m_outlay;
    public long m_income;

    public string getLotteryName(int index) 
    {
        string lotteryName = index.ToString();
        switch(index)
        {
            case 22: lotteryName = "武装巨蟹"; break;
            case 23: lotteryName = "武装海豹"; break;
        }

        return lotteryName;
    }

    public string getDetail(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "lotteryId";
        uParam.m_value = index.ToString();
        uParam.m_url = DefCC.ASPX_STAT_FISHLORD_SHARK_ROOM_INCOME_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time);
        return Tool.genHyperlink(uParam);
    }
}

public class QueryTypeStatFishlordSharkRoomIncome : QueryBase 
{
    private List<StatSharkRoomItem> m_result = new List<StatSharkRoomItem>();

    public StatSharkRoomItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatSharkRoomItem item = new StatSharkRoomItem();
        m_result.Add(item);
        item.m_time = time;

        for (int i = 22; i < 24; i++) 
        {
            int[] arr = new int[10];
            for (int j = 0; j < 10; j++) 
            {
                arr[j] = 0;
            }

            item.m_dataIndex.Add(i, arr);
        }

        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("type", 1));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_SHARK_ROOM_OUTLAY, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_SHARK_ROOM_OUTLAY, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            StatSharkRoomItem tmp = IsCreate(time);
            tmp.m_time = time;

            if (data.ContainsKey("income"))
                tmp.m_income += Convert.ToInt64(data["income"]);

            if (data.ContainsKey("outlay"))
                tmp.m_outlay += Convert.ToInt64(data["outlay"]);

            if (data.ContainsKey("fishid"))
            {
                int fishId = Convert.ToInt32(data["fishid"]);

                for (k = 0; k < 10; k++ )
                {
                    string str = "index_" + (k+1);
                    if (data.ContainsKey(str))
                        tmp.m_dataIndex[fishId][k] = Convert.ToInt32(data[str]);
                }
            }
        }
        return OpRes.opres_success;
    }
}
//详情
public class StatSharkRoomDetailItem 
{
    public string m_time;
    public int m_playerId;
    public string m_nickName;
    public int m_fishId;
    public int m_index;
    public long m_gold;
    public int m_bulletrate;

    public string getIndexName() 
    {
        string indexName = m_index.ToString();

        var  fishShark = FishGiantSharkIntegralCFG.getInstance().getValue(m_index);
        if (fishShark != null)
            indexName = fishShark.m_itemName;

        return indexName;
    }
}
public class QueryTypeStatFishlordSharkRoomIncomeDetail : QueryBase 
{
    private List<StatSharkRoomDetailItem> m_result = new List<StatSharkRoomDetailItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        if (p.m_op == 0)
        {
            imq = Query.And(imq, Query.EQ("type", 2));
        }
        else 
        {
            imq = Query.And(imq, Query.EQ("fishId", p.m_op), Query.EQ("type", 1));
        }

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_SHARK_ROOM_DETAIL, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_SHARK_ROOM_DETAIL, dip, imq, 
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            StatSharkRoomDetailItem tmp = new StatSharkRoomDetailItem();
            m_result.Add(tmp);

            tmp.m_time = time;

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("index"))
                tmp.m_index = Convert.ToInt32(data["index"]);

            if (data.ContainsKey("gold"))
                tmp.m_gold = Convert.ToInt64(data["gold"]);

            if (data.ContainsKey("bulletrate"))
                tmp.m_bulletrate = Convert.ToInt32(data["bulletrate"]);
        }
        return OpRes.opres_success;
    }
}

//拆解统计
public class StatSharkRoomChaijieItem
{
    public string m_time;
    public int[] m_dataIndex = new int[7];
    public long m_outlay;
    public long m_income;

    public string getDetail(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "lotteryId";
        uParam.m_value = index.ToString();
        uParam.m_url = DefCC.ASPX_STAT_FISHLORD_SHARK_ROOM_INCOME_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time);
        return Tool.genHyperlink(uParam);
    }
}
public class QueryTypeStatFishlordSharkRoomChaijieIncome : QueryBase 
{
    private List<StatSharkRoomChaijieItem> m_result = new List<StatSharkRoomChaijieItem>();

    public StatSharkRoomChaijieItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatSharkRoomChaijieItem item = new StatSharkRoomChaijieItem();
        m_result.Add(item);
        item.m_time = time;

        int[] arr = new int[7];
        for (int j = 0; j < 7; j++)
        {
            item.m_dataIndex[j] = 0;
        }
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("type", 2));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_SHARK_ROOM_OUTLAY, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_SHARK_ROOM_OUTLAY, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            StatSharkRoomChaijieItem tmp = IsCreate(time);
            tmp.m_time = time;

            if (data.ContainsKey("income"))
                tmp.m_income += Convert.ToInt64(data["income"]);

            if (data.ContainsKey("outlay"))
                tmp.m_outlay += Convert.ToInt64(data["outlay"]);

            for (k = 0; k < 7; k++)
            {
                string str = "index_" + (k + 1);
                if (data.ContainsKey(str))
                    tmp.m_dataIndex[k] += Convert.ToInt32(data[str]);
            }
        }
        return OpRes.opres_success;
    }
}
//轰炸机统计
public class StatSharkRoomBombItem 
{
    public string m_time;
    public Dictionary<int, long[]> m_data = new Dictionary<int, long[]>();
}
public class QueryTypeStatFishlordSharkRoomBomb : QueryBase 
{
    private List<StatSharkRoomBombItem> m_result = new List<StatSharkRoomBombItem>();

    public StatSharkRoomBombItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatSharkRoomBombItem item = new StatSharkRoomBombItem();
        m_result.Add(item);
        item.m_time = time;

        item.m_data.Add(0, new long[] { 0, 0, 0, 0, 0 });
        item.m_data.Add(1, new long[] { 0, 0, 0, 0, 0 });
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        //user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_SHARK_ROOM_BOMB, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_SHARK_ROOM_BOMB, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time =  t.ToShortDateString();

            StatSharkRoomBombItem tmp = IsCreate(time);

            if (!data.ContainsKey("bombtype") || !data.ContainsKey("type"))
                continue;

            int bombtype = Convert.ToInt32(data["bombtype"]);
            int type = Convert.ToInt32(data["type"]);

            long count = 0, outlay = 0;
            if (data.ContainsKey("count"))
                count = Convert.ToInt64(data["count"]);

            if (data.ContainsKey("outlay"))
                outlay = Convert.ToInt64(data["outlay"]);

            //总轰炸产出金币   轰炸次数  轰炸人数  使用金币轰炸次数  使用金币轰炸产出金币

            IMongoQuery imq_person = Query.And( 
                                        Query.EQ("genTime", t.Date),
                                        Query.EQ("type", type),
                                        Query.EQ("bombtype", bombtype)
                                     );
            tmp.m_data[bombtype][2] += DBMgr.getInstance().executeDistinct(TableName.STAT_FISHLORD_SHARK_ROOM_BOMB_DETAIL, dip, "playerId", imq_person);


            if (type == 0) 
            {
                tmp.m_data[0][0] += outlay; 
                tmp.m_data[bombtype][1] += count;
            }

            if(type == 1)
            {
                tmp.m_data[bombtype][0] += outlay; 
                tmp.m_data[bombtype][1] += count;

                tmp.m_data[bombtype][2] += 0;

                tmp.m_data[bombtype][3] += count;
                tmp.m_data[bombtype][4] += outlay;
            }

        }
        return OpRes.opres_success;
    }
}

//抽奖统计
public class StatSharkRoomLotteryItem 
{
    public string m_time;
    public int m_playerCount;
    public int m_playerNum;
    public long m_outlay;

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_STAT_FISHLORD_SHARK_ROOM_LOTTERY_DETAIL;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryTypeStatFishlordSharkRoomLottery : QueryBase 
{
    private List<StatSharkRoomLotteryItem> m_result = new List<StatSharkRoomLotteryItem>();
    public StatSharkRoomLotteryItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatSharkRoomLotteryItem item = new StatSharkRoomLotteryItem();
        m_result.Add(item);
        item.m_time = time;

        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_SHARK_ROOM_LOTTERY, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            StatSharkRoomLotteryItem tmp = IsCreate(time);
            tmp.m_time = time;

            if (data.ContainsKey("playerId"))
                tmp.m_playerNum += 1;

            for (k = 1; k <= 6; k++) 
            {
                string str = "reward_" + k;
                if (data.ContainsKey(str))
                    tmp.m_playerCount += Convert.ToInt32(data[str]);
            }

            if (data.ContainsKey("outlay"))
                tmp.m_outlay += Convert.ToInt64(data["outlay"]);
        }
        return OpRes.opres_success;
    }
}
//详情
public class StatFishlordSharkRoomLotteryPlayerItem 
{
    public string m_time;
    public int m_playerId;
    public string m_nickName;
    public Dictionary<int, int> m_rewardCount = new Dictionary<int, int>();
}
public class QueryTypeStatFishlordSharkRoomLotteryDetail : QueryBase 
{
    private List<StatFishlordSharkRoomLotteryPlayerItem> m_result = new List<StatFishlordSharkRoomLotteryPlayerItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_SHARK_ROOM_LOTTERY, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            StatFishlordSharkRoomLotteryPlayerItem tmp = new StatFishlordSharkRoomLotteryPlayerItem();
            m_result.Add(tmp);
            tmp.m_time = time;

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickname"))
                tmp.m_nickName = Convert.ToString(data["nickname"]);

            for (k = 1; k <= 6; k++)
            {
                string str = "reward_" + k;
                int rewardCount = 0;
                if (data.ContainsKey(str))
                    rewardCount = Convert.ToInt32(data[str]);

                tmp.m_rewardCount.Add(k, rewardCount);
            }
        }
        return OpRes.opres_success;
    }
}

//巨鲨场排行榜
public class StatFishlordSharkRoomRank 
{
    public string m_time;
    public int m_playerId;
    public int m_rankId;
    public string m_nickName;
    public int m_vipLevel;
    public int m_points;
}
public class QueryTypeStatFishlordSharkRoomRank : QueryBase 
{
    private List<StatFishlordSharkRoomRank> m_result = new List<StatFishlordSharkRoomRank>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        string scope1 = "", scope2 = "";
        int skip = 0, limit = 0;
        IMongoQuery imq = null;
        if (p.m_type == 1) //当前
        {
            imq = Query.And(Query.GT("dailyTotalPoints", 0), Query.EQ("genTime", DateTime.Now.Date));

            scope1 = "pointModifyTime";
            scope2 = "dailyTotalPoints";
            skip = 0;
            limit = 10;
            return query(user, imq, TableName.STAT_FISHLORD_ARMED_SHARK_PLAYER, DbName.DB_GAME, scope1, scope2, skip, limit);
        }
        else  //历史
        {

            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            imq = Query.And(imq1, imq2);
            imq = Query.And(imq, Query.GT("dailyTotalPoints", 0), Query.Exists("rank"));

            scope1 = "rank";
            scope2 = "genTime";

            skip = 0;
            limit = 0;

            return query(user, imq, TableName.STAT_FISHLORD_SHARK_RANK, DbName.DB_PUMP, scope1, scope2, skip, limit);
        }
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, string tableName, int db, string scope1, string scope2, int skip, int limit)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), db, DbInfoParam.SERVER_TYPE_SLAVE);

        //user.totalRecord = DBMgr.getInstance().getRecordCount(tableName, imq, dip);

        List<Dictionary<string, object>> data_list =
            DBMgr.getInstance().executeQuery(tableName, dip, imq, skip, limit, null, "genTime", false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => 
        {
            if (scope1 == "pointModifyTime" && !a.ContainsKey(scope1))
            {
                return DateTime.Now;
            }
            else { 
                return a[scope1];
            }
        }).OrderByDescending(a => a[scope2]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatFishlordSharkRoomRank tmp = new StatFishlordSharkRoomRank();
            m_result.Add(tmp);

            tmp.m_rankId = i + 1;
            if (data.ContainsKey("rank"))
            {
                tmp.m_rankId = Convert.ToInt32(data["rank"]);

                tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            }

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("vip"))
                tmp.m_vipLevel = Convert.ToInt32(data["vip"]);

            if (data.ContainsKey("dailyTotalPoints"))
                tmp.m_points = Convert.ToInt32(data["dailyTotalPoints"]);
        }
        return OpRes.opres_success;
    }
}
//巨鲨场作弊玩家积分
public class QueryTypeStatFishlordSharkRoomScore : QueryBase 
{
    private List<StatFishlordAdvancedRoomActRankItem> m_result = new List<StatFishlordAdvancedRoomActRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;

        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq = Query.And(Query.EQ("playerId", playerId), Query.EQ("genTime", DateTime.Now.Date));

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> playerScore = DBMgr.getInstance().getTableData(TableName.STAT_FISHLORD_ARMED_SHARK_PLAYER, dip, imq);
        if (playerScore != null)
        {
            StatFishlordAdvancedRoomActRankItem tmp = new StatFishlordAdvancedRoomActRankItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(playerScore["genTime"]).ToLocalTime().ToShortDateString();

            if (playerScore.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(playerScore["nickName"]);

            if (playerScore.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToString(playerScore["playerId"]);

            if (playerScore.ContainsKey("dailyTotalPoints"))
                tmp.m_score = Convert.ToInt32(playerScore["dailyTotalPoints"]);

            return OpRes.opres_success;
        }
        return OpRes.op_res_not_found_data;
    }
}

//巨鲨场能量统计
public class StatFishlordSharkRoomEnergy 
{
    public string m_time;
    public int m_joinCount;
    public long m_goldConsume;
    public int m_sharkKillCount;
    public long m_energyOutlay;
}
public class QueryTypeStatFishlordSharkRoomEnergy : QueryBase 
{
    private List<StatFishlordSharkRoomEnergy> m_result = new List<StatFishlordSharkRoomEnergy>();
    public StatFishlordSharkRoomEnergy IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatFishlordSharkRoomEnergy item = new StatFishlordSharkRoomEnergy();
        m_result.Add(item);
        item.m_time = time;
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_SHARK_ROOM_ENERGY_DROP, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            StatFishlordSharkRoomEnergy tmp = IsCreate(time);

            if (data.ContainsKey("playerId"))
                tmp.m_joinCount += 1;

            if (data.ContainsKey("energyDrop"))
                tmp.m_energyOutlay += Convert.ToInt64(data["energyDrop"]);

            if (data.ContainsKey("killCount"))
                tmp.m_sharkKillCount += Convert.ToInt32(data["killCount"]);

            if (data.ContainsKey("goldOutlay"))
                tmp.m_goldConsume += Convert.ToInt64(data["goldOutlay"]);

        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//中级场玩家玩法
public class FishlordMiddleRoomPlayerRankItem
{
    public int m_rankId;
    public int m_playerId;
    public string m_nickName;
    public int m_accScore;
    public int m_maxScore;
}
//玩家当前积分
public class QueryTypeStatFishlordMiddleRoomPlayerScore : QueryBase
{
    private List<FishlordMiddleRoomPlayerRankItem> m_result = new List<FishlordMiddleRoomPlayerRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;

        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq = Query.And(Query.EQ("playerId", playerId), Query.EQ("genTime", DateTime.Now.Date));

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> playerScore = DBMgr.getInstance().getTableData(TableName.STAT_FISHLORD_MIDDLE_PLAYER, dip, imq);
        if (playerScore != null)
        {
            FishlordMiddleRoomPlayerRankItem tmp = new FishlordMiddleRoomPlayerRankItem();
            m_result.Add(tmp);

            if (playerScore.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(playerScore["nickName"]);

            if (playerScore.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(playerScore["playerId"]);

            if (playerScore.ContainsKey("singleMax"))
                tmp.m_maxScore = Convert.ToInt32(playerScore["singleMax"]);

            if (playerScore.ContainsKey("points"))
                tmp.m_accScore = Convert.ToInt32(playerScore["points"]);

            return OpRes.opres_success;
        }
        return OpRes.op_res_not_found_data;
    }
}

//玩家积分排行榜
public class QueryTypeStatFishlordMiddleRoomPlayerRank : QueryBase
{
    private List<FishlordMiddleRoomPlayerRankItem> m_result = new List<FishlordMiddleRoomPlayerRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        string scoreType = "";
        string scoreTime = "";
        int limit = 0;
        if (p.m_type == 1)
        {
            scoreType = "points";
            scoreTime = "pointModifyTime";
            limit = 10;
        }
        else
        {
            scoreType = "singleMax";
            scoreTime = "singleModifyTime";
            limit = 20;
        }

        IMongoQuery imq = Query.And(Query.GT(scoreType, 0), Query.EQ("genTime", DateTime.Now.Date));

        return query(user, imq, scoreType, scoreTime, limit);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, string scoreType, string scoreTime, int limit)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
            DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_MIDDLE_PLAYER, dip, imq, 0, limit, null, scoreType, false);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a =>
        {
            if (a.ContainsKey(scoreTime))
            {
                return a[scoreTime];
            }
            else
            {
                return DateTime.Now;
            }
        }).OrderByDescending(a => a[scoreType]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishlordMiddleRoomPlayerRankItem tmp = new FishlordMiddleRoomPlayerRankItem();
            m_result.Add(tmp);

            tmp.m_rankId = (i + 1);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("singleMax"))
                tmp.m_maxScore = Convert.ToInt32(data["singleMax"]);

            if (data.ContainsKey("points"))
                tmp.m_accScore = Convert.ToInt32(data["points"]);
        }
        return OpRes.opres_success;
    }
}
//玩法收入统计
public class StatFishlordMiddleRoomIncomeItem
{
    public string m_time;
    public long m_income;
    public long m_outlay;

    public long m_buffIncome;
    public long m_buffOutlay;

    public int m_item18;
    public int m_item19;
    public int m_item20;
    public int m_item21;
    public int m_item52;
}
public class QueryTypeStatFishlordMiddleRoomIncome : QueryBase
{
    private List<StatFishlordMiddleRoomIncomeItem> m_result = new List<StatFishlordMiddleRoomIncomeItem>();
    private string[] m_fields = new string[] { "DailyTrickIncome", "DailyTrickOutlay", "DailyTickIncomeByBuff", "DailyTickOutlayByBuff"};
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_MIDDLE_ROOM_INCOME, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_MIDDLE_ROOM_INCOME, dip, imq, 0, 0, null, "time", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatFishlordMiddleRoomIncomeItem tmp = new StatFishlordMiddleRoomIncomeItem();
            m_result.Add(tmp);
            DateTime time = Convert.ToDateTime(data["time"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();

            if (data.ContainsKey("item_18"))
                tmp.m_item18 = Convert.ToInt32(data["item_18"]);

            if (data.ContainsKey("item_19"))
                tmp.m_item19 = Convert.ToInt32(data["item_19"]);

            if (data.ContainsKey("item_20"))
                tmp.m_item20 = Convert.ToInt32(data["item_20"]);

            if (data.ContainsKey("item_21"))
                tmp.m_item21 = Convert.ToInt32(data["item_21"]);

            if (data.ContainsKey("item_52"))
                tmp.m_item52 = Convert.ToInt32(data["item_52"]);

            //玩法收入支出
            IMongoQuery imq_1 = Query.EQ("Date", time.Date);
            IMongoQuery imq_2 = Query.EQ("ROOMID", 2);
            IMongoQuery imq_3 = Query.And(imq_1, imq_2);
            List<Dictionary<string, object>> dailyIncomeOutlay =
           DBMgr.getInstance().executeQuery(TableName.PUMP_FISHLORD_EVERY_DAY, dip, imq_3, 0, 0, m_fields, "Date", false);
            if (dailyIncomeOutlay != null) 
            {
                for (k = 0; k < dailyIncomeOutlay.Count; k++) 
                {
                    if (dailyIncomeOutlay[k].ContainsKey("DailyTrickIncome"))
                        tmp.m_income += Convert.ToInt64(dailyIncomeOutlay[k]["DailyTrickIncome"]);

                    if (dailyIncomeOutlay[k].ContainsKey("DailyTrickOutlay"))
                        tmp.m_outlay += Convert.ToInt64(dailyIncomeOutlay[k]["DailyTrickOutlay"]);

                    if (dailyIncomeOutlay[k].ContainsKey("DailyTickIncomeByBuff"))
                        tmp.m_buffIncome += Convert.ToInt64(dailyIncomeOutlay[k]["DailyTickIncomeByBuff"]);

                    if (dailyIncomeOutlay[k].ContainsKey("DailyTickOutlayByBuff"))
                        tmp.m_buffOutlay += Convert.ToInt64(dailyIncomeOutlay[k]["DailyTickOutlayByBuff"]);
                }
            }
        }
        return OpRes.opres_success;
    }
}

//兑换统计
public class StatFishlordMiddleRoomExchangeItem
{
    public string m_time;
    public long m_goldOutlay;
    public int m_exchange1;
    public int m_exchange2;
    public int m_exchange3;
    public int m_exchange4;
    public int m_exchange5;
    public int m_exchange6;
    public int m_exchange7;
    public int m_exchange8;
    public int m_exchange9;

    public long m_exgoldvalue6;
    public long m_exgoldvalue7;
    public long m_exgoldvalue8;

    public Dictionary<int, int> m_exchangeCount = new Dictionary<int, int>();

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_STAT_MIDDLE_ROOM_EXCHANGE_DETAIL;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryTypeStatFishlordMiddleRoomExchange : QueryBase
{
    private List<StatFishlordMiddleRoomExchangeItem> m_result = new List<StatFishlordMiddleRoomExchangeItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0, j = 0;
        long count = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatFishlordMiddleRoomExchangeItem tmp = new StatFishlordMiddleRoomExchangeItem();
            m_result.Add(tmp);
            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();

            if (data.ContainsKey("goldOutlay"))
                tmp.m_goldOutlay = Convert.ToInt64(data["goldOutlay"]);

            if (data.ContainsKey("exchange_1"))
                tmp.m_exchange1 = Convert.ToInt32(data["exchange_1"]);

            if (data.ContainsKey("exchange_2"))
                tmp.m_exchange2 = Convert.ToInt32(data["exchange_2"]);

            if (data.ContainsKey("exchange_3"))
                tmp.m_exchange3 = Convert.ToInt32(data["exchange_3"]);

            if (data.ContainsKey("exchange_4"))
                tmp.m_exchange4 = Convert.ToInt32(data["exchange_4"]);

            if (data.ContainsKey("exchange_5"))
                tmp.m_exchange5 = Convert.ToInt32(data["exchange_5"]);

            if (data.ContainsKey("exchange_6"))
                tmp.m_exchange6 = Convert.ToInt32(data["exchange_6"]);

            if (data.ContainsKey("exchange_7"))
                tmp.m_exchange7 = Convert.ToInt32(data["exchange_7"]);

            if (data.ContainsKey("exchange_8"))
                tmp.m_exchange8 = Convert.ToInt32(data["exchange_8"]);

            if (data.ContainsKey("exchange_9"))
                tmp.m_exchange9 = Convert.ToInt32(data["exchange_9"]);

           //兑换678额外奖励的物品价值
            if (data.ContainsKey("exgoldvalue_6"))
                tmp.m_exgoldvalue6 = Convert.ToInt64(data["exgoldvalue_6"]);

            if (data.ContainsKey("exgoldvalue_7"))
                tmp.m_exgoldvalue7 = Convert.ToInt64(data["exgoldvalue_7"]);

            if (data.ContainsKey("exgoldvalue_8"))
                tmp.m_exgoldvalue8 = Convert.ToInt64(data["exgoldvalue_8"]);

            //兑换人数
            for (k = 1; k < 10; k++)
            {
                IMongoQuery imq_1 = Query.EQ("genTime", time.Date);
                string exchange = "exchange_" + k;

                imq_1 = Query.And(imq_1, Query.GT(exchange, 0));

                count = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE_DETAIL, imq_1, dip);

                tmp.m_exchangeCount.Add(k, Convert.ToInt32(count));
            }
        }
        return OpRes.opres_success;
    }
}
//兑换统计详情
public class StatMiddleRoomExchangeDetailItem
{  
    public string m_time;
    public int m_playerId;
    public Dictionary<int, int[]> m_exchangeList = new Dictionary<int, int[]>();
}
public class QueryTypeStatFishlordMiddleRoomExchangeDetail : QueryBase 
{
    private List<StatMiddleRoomExchangeDetailItem> m_result = new List<StatMiddleRoomExchangeDetailItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE_DETAIL, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE_DETAIL, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatMiddleRoomExchangeDetailItem tmp = new StatMiddleRoomExchangeDetailItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            for (k = 6; k <= 8; k++) 
            {
                string str1 = "exchange_" + k;
                int exchange = 0;
                if (data.ContainsKey(str1))
                    exchange = Convert.ToInt32(data[str1]);

                string str2 = "exCount_" + k;
                int exCount = 0;
                if (data.ContainsKey(str2))
                    exCount = Convert.ToInt32(data[str2]);

                tmp.m_exchangeList.Add(k, new int[]{exchange, exCount});
            }
        }
        return OpRes.opres_success;
    }
}

//历史排行榜
public class StatFishlordMiddleRoomRankItem
{
    public string m_time;
    public int m_rankId;
    public int m_playerId;
    public string m_nickName;
    public int m_score;
    public int m_vipLevel;
}
public class QueryTypeStatFishlordMiddleRoomRank : QueryBase
{
    private List<StatFishlordMiddleRoomRankItem> m_result = new List<StatFishlordMiddleRoomRankItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(imq, Query.EQ("rankType", p.m_type));
        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLORD_MIDDLE_ROOM_RANK, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_FISHLORD_MIDDLE_ROOM_RANK, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatFishlordMiddleRoomRankItem tmp = new StatFishlordMiddleRoomRankItem();
            m_result.Add(tmp);
            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();

            tmp.m_rankId = Convert.ToInt32(data["rank"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("score"))
                tmp.m_score = Convert.ToInt32(data["score"]);

            if (data.ContainsKey("vipLevel"))
                tmp.m_vipLevel = Convert.ToInt32(data["vipLevel"]);
        }
        return OpRes.opres_success;
    }
}
//打点数据
public class StatFishlordMiddleRoomFuDaiItem
{
    public string m_time;
    public int m_action1Count;
    public int m_action2Count;
}
public class QueryTypeStatFishlordMiddleRoomFuDai : QueryBase
{
    private List<StatFishlordMiddleRoomFuDaiItem> m_result = new List<StatFishlordMiddleRoomFuDaiItem>();
    public StatFishlordMiddleRoomFuDaiItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatFishlordMiddleRoomFuDaiItem item = new StatFishlordMiddleRoomFuDaiItem();
        m_result.Add(item);
        item.m_time = time;
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("giftType", p.m_op));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_FISHLOR_ROOM_FU_DAI, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("playerId");
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_FISHLOR_ROOM_FU_DAI, dip, imq, 0, 0, null, sort);

        int count = 0;
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            StatFishlordMiddleRoomFuDaiItem tmp = IsCreate(time);

            if (data.ContainsKey("playerId"))
            {
                count = 0;
                if (data.ContainsKey("action_1"))
                    count = Convert.ToInt32(data["action_1"]);

                if (count > 0) 
                {
                    tmp.m_action1Count++;
                    tmp.m_action2Count += count;
                }  
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//中级场礼包统计
public class StatMiddleRoomExchangeItem 
{
    public string m_time;
    public int m_giftId;
    public List<long> m_giftgoldlv = new List<long>();
}
public class QueryTypeStatMiddleRoomExchange : QueryBase 
{
    private List<StatMiddleRoomExchangeItem> m_result = new List<StatMiddleRoomExchangeItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        string[] m_fields = new string[10];
        m_fields[0] = "genTime";
        for (int i = 1; i < 10; i++) 
        {
            string str = "giftgoldlv" + p.m_op + "_" + i;

            m_fields[i] = str;
        }

        return query(p, imq, user, m_fields);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user, string[] m_fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE, dip, imq, 0, 0, m_fields, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatMiddleRoomExchangeItem tmp = new StatMiddleRoomExchangeItem();
            m_result.Add(tmp);

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();

            for (j = 1; j < 10; j++) 
            {
                string str = "giftgoldlv" + param.m_op + "_" + j;
                if (data.ContainsKey(str)){
                    long gold = Convert.ToInt64(data[str]);
                    tmp.m_giftgoldlv.Add(gold);
                }else{
                    tmp.m_giftgoldlv.Add(0L);
                }
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////
//高级场奖池控制
public class QueryTypeStatFishlordAdvancedRoom : QueryBase 
{
    private List<ParamFishlordAdvancedRoomItem> m_result = new List<ParamFishlordAdvancedRoomItem>();
    private string[] m_fields = new string[] { "LotterCtrRate", "LotterPoolExpect", "JsckpotSmallPump" };
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.FISHLORD_ADVANCED_ROOM_CTRL, null, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.FISHLORD_ADVANCED_ROOM_CTRL, dip, null, 0, 0, null, "level");

        ParamFishlordAdvancedRoomItem tmp1 = new ParamFishlordAdvancedRoomItem();
        m_result.Add(tmp1);
        tmp1.m_op = 0;
        tmp1.m_maxWinCount = 1;
        tmp1.m_ratio = 60;

        ParamFishlordAdvancedRoomItem tmp2 = new ParamFishlordAdvancedRoomItem();
        m_result.Add(tmp2);
        tmp2.m_op = 1;
        tmp2.m_maxWinCount = 2;
        tmp2.m_ratio = 20;

        ParamFishlordAdvancedRoomItem tmp3 = new ParamFishlordAdvancedRoomItem();
        m_result.Add(tmp3);
        tmp3.m_op = 2;
        tmp3.m_maxWinCount = 5;
        tmp3.m_ratio = 5;


        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            if (!data.ContainsKey("level"))
                continue;
            int level = Convert.ToInt32(data["level"]);

            int maxWinCount = 0, rewardRatio = 0;
            switch (level)
            {
                case 0:
                    maxWinCount = 1;
                    rewardRatio = 60;
                    break;
                case 1: maxWinCount = 2;
                    rewardRatio = 20;
                    break;
                case 2: maxWinCount = 5;
                    rewardRatio = 5;
                    break;
            }
            if (data.ContainsKey("maxCount"))
                maxWinCount = Convert.ToInt32(data["maxCount"]);

            if (data.ContainsKey("ratio"))
                rewardRatio = Convert.ToInt32(data["ratio"]);

            switch (level)
            {
                case 0:
                    tmp1.m_ratio = rewardRatio;
                    tmp1.m_maxWinCount = maxWinCount;
                    break;
                case 1:
                    tmp2.m_ratio = rewardRatio;
                    tmp2.m_maxWinCount = maxWinCount;
                    break;
                case 2:
                    tmp3.m_ratio = rewardRatio;
                    tmp3.m_maxWinCount = maxWinCount;
                    break;
            }
        }

        //奖池期望值   控制系数
        IMongoQuery imq_sys = Query.EQ("room_id", 3);

         Dictionary<string, object> advancedRoomList = DBMgr.getInstance().getTableData(TableName.FISHLORD_ROOM, dip, imq_sys, m_fields);
         ParamFishlordAdvancedRoomItem tmp4 = new ParamFishlordAdvancedRoomItem();
         m_result.Add(tmp4);
         tmp4.m_ratio = 0;

         ParamFishlordAdvancedRoomItem tmp5 = new ParamFishlordAdvancedRoomItem();
         m_result.Add(tmp5);
         tmp5.m_ratio = 0;

         ParamFishlordAdvancedRoomItem tmp6 = new ParamFishlordAdvancedRoomItem();
         m_result.Add(tmp6);
         tmp6.m_ratio = 0;
         if (advancedRoomList != null)
         {
             if (advancedRoomList.ContainsKey("LotterPoolExpect"))
                 tmp4.m_ratio = Convert.ToInt32(advancedRoomList["LotterPoolExpect"]);

            if (advancedRoomList.ContainsKey("LotterCtrRate"))
                tmp5.m_ratio = Convert.ToInt32(advancedRoomList["LotterCtrRate"]);

            if (advancedRoomList.ContainsKey("JsckpotSmallPump"))
                tmp6.m_ratio = Convert.ToInt32(advancedRoomList["JsckpotSmallPump"]);
         }

        return OpRes.opres_success;
    }
}

//高级场奖池统计
public class StatFishlordAdvancedRoomActItem
{
    public string m_time;
    public int m_lotteryCount;
    public int m_lotteryPerson;
    public long m_poolIncome;
    public long m_poolOutlay;
    public long m_recycleGold;

    public long m_dailyAdvanceRoomGrandPriceOutlay;

    public long m_level1;
    public long m_level2;
    public long m_level3;

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_STAT_FISHLORD_ADVANCED_ROOM_ACT_DETAIL;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryTypeStatFishlordAdvancedRoomAct : QueryBase
{
    private List<StatFishlordAdvancedRoomActItem> m_result = new List<StatFishlordAdvancedRoomActItem>();
    private string[] m_fields = new string[] { "Date", "DailyTrickIncome", "DailyTrickOutlay", "JsckpotPumpIncome", "DailyAdvanceRoomGrandPriceOutlay" };
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_param, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("Date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("Date", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(imq, Query.EQ("ROOMID", 3));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PUMP_FISHLORD_EVERY_DAY, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_FISHLORD_EVERY_DAY, dip, imq, 0, 0, m_fields, "Date", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        StatFishlordAdvancedRoomActItem tmp1 = new StatFishlordAdvancedRoomActItem();
        m_result.Add(tmp1);
        tmp1.m_time = "总计";
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatFishlordAdvancedRoomActItem tmp = new StatFishlordAdvancedRoomActItem();
            m_result.Add(tmp);

            DateTime t = Convert.ToDateTime(data["Date"]).ToLocalTime();
            tmp.m_time = t.ToShortDateString();

            //=========奖池收入 奖池支出 系统回收==================================================
            if (data.ContainsKey("DailyTrickIncome"))
                tmp.m_poolIncome = Convert.ToInt32(data["DailyTrickIncome"]);

            if (data.ContainsKey("DailyTrickOutlay"))
                tmp.m_poolOutlay = Convert.ToInt32(data["DailyTrickOutlay"]);

            if (data.ContainsKey("JsckpotPumpIncome"))
                tmp.m_recycleGold = Convert.ToInt64(data["JsckpotPumpIncome"]);

            if (data.ContainsKey("DailyAdvanceRoomGrandPriceOutlay"))
                tmp.m_dailyAdvanceRoomGrandPriceOutlay = Convert.ToInt64(data["DailyAdvanceRoomGrandPriceOutlay"]);

            IMongoQuery imq_sys = Query.EQ("genTime", t.Date);
            Dictionary<string, object> levelInfo = DBMgr.getInstance().getTableData(TableName.FISHLORD_ADVANCED_ROOM_ACT, dip, imq_sys);
            if (levelInfo != null)
            {
                if (levelInfo.ContainsKey("level_1"))
                    tmp.m_level1 = Convert.ToInt32(levelInfo["level_1"]);

                if (levelInfo.ContainsKey("level_2"))
                    tmp.m_level2 = Convert.ToInt32(levelInfo["level_2"]);

                if (levelInfo.ContainsKey("level_3"))
                    tmp.m_level3 = Convert.ToInt32(levelInfo["level_3"]);
            }

            //抽奖人数人次统计======================================================================
            DateTime t_max = t.AddDays(1);
            IMongoQuery imq_1 = Query.GTE("genTime", t);
            imq_1 = Query.And(imq_1, Query.LT("genTime", t_max));

            //当日抽奖人次
            long m_lotteryCount = DBMgr.getInstance().getRecordCount(TableName.FISHLORD_ADVANCED_ROOM_ACT_DETAIL, imq_1, dip);
            tmp.m_lotteryCount = Convert.ToInt32(m_lotteryCount);

            //当日抽奖人数
            tmp.m_lotteryPerson = DBMgr.getInstance().executeDistinct(TableName.FISHLORD_ADVANCED_ROOM_ACT_DETAIL, dip, "playerId", imq_1);

            //====总计================================================================================
            tmp1.m_lotteryCount += tmp.m_lotteryCount;
            tmp1.m_lotteryPerson += tmp.m_lotteryPerson;

            tmp1.m_poolIncome += tmp.m_poolIncome;
            tmp1.m_poolOutlay += tmp.m_poolOutlay;
            tmp1.m_recycleGold += tmp.m_recycleGold;

            tmp1.m_dailyAdvanceRoomGrandPriceOutlay += tmp.m_dailyAdvanceRoomGrandPriceOutlay;

            tmp1.m_level1 += tmp.m_level1;
            tmp1.m_level2 += tmp.m_level2;
            tmp1.m_level3 += tmp.m_level3;
        }
        return OpRes.opres_success;
    }
}
//高级场奖池统计详情
public class FishlordAdvancedRoomDetailItem
{
    public string m_time;
    public int m_gold;
    public int m_level;
    public int m_bulletRate;
    public int m_playerId;
    public string m_nickName;

    public string getLevelName()
    {
        string levelName = "";
        switch (m_level)
        {
            case -1: levelName = "普通中奖"; break;
            case 1: levelName = "一等奖"; break;
            case 2: levelName = "二等奖"; break;
            case 3: levelName = "三等奖"; break;
        }
        return levelName;
    }
}
public class QueryTypeStatFishlordAdvancedRoomActDetail : QueryBase
{
    private List<FishlordAdvancedRoomDetailItem> m_result = new List<FishlordAdvancedRoomDetailItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.FISHLORD_ADVANCED_ROOM_ACT_DETAIL, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.FISHLORD_ADVANCED_ROOM_ACT_DETAIL, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            FishlordAdvancedRoomDetailItem tmp = new FishlordAdvancedRoomDetailItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("gold"))
                tmp.m_gold = Convert.ToInt32(data["gold"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("bulletRate"))
                tmp.m_bulletRate = Convert.ToInt32(data["bulletRate"]);

            if (data.ContainsKey("level"))
                tmp.m_level = Convert.ToInt32(data["level"]);
        }
        return OpRes.opres_success;
    }
}

//高级场排行榜
public class StatFishlordAdvancedRoomActRankItem
{
    public string m_time;
    public int m_rankId;
    public string m_playerId;
    public string m_nickName;
    public int m_score;
}
public class QueryTypeStatFishlordAdvancedRoomActRank : QueryBase
{
    private List<StatFishlordAdvancedRoomActRankItem> m_result = new List<StatFishlordAdvancedRoomActRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq = null;
        if (p.m_param == "0") //当前活动
        {
            imq = Query.Exists("points");
            imq = Query.And(imq, Query.EQ("genTime", DateTime.Now.Date));
            return query1(user, imq);
        }
        else //历史活动
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

            imq = Query.And(imq1, imq2);
            return query2(user, imq);
        }
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    //当前
    private OpRes query1(GMUser user, IMongoQuery imq) 
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("points").Ascending("genTime");
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR, dip, imq, 0, 20, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatFishlordAdvancedRoomActRankItem tmp = new StatFishlordAdvancedRoomActRankItem();
            m_result.Add(tmp);

            tmp.m_rankId = i + 1;
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("points"))
                tmp.m_score = Convert.ToInt32(data["points"]);
        }
        return OpRes.opres_success;
    }

    //历史
    private OpRes query2(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("rank");
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_HIS, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatFishlordAdvancedRoomActRankItem tmp = new StatFishlordAdvancedRoomActRankItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_rankId = Convert.ToInt32(data["rank"]);

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToString(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            if (data.ContainsKey("killCount"))
                tmp.m_score = Convert.ToInt32(data["killCount"]);
        }
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////////////
//高级场玩家积分
public class QueryTypeStatFishlordAdvancedRoomScore : QueryBase 
{
    private List<StatFishlordAdvancedRoomActRankItem> m_result = new List<StatFishlordAdvancedRoomActRankItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;

        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq = Query.And(Query.EQ("playerId", playerId), Query.EQ("genTime", DateTime.Now.Date));

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_GAME, DbInfoParam.SERVER_TYPE_SLAVE);

        Dictionary<string, object> playerScore = DBMgr.getInstance().getTableData(TableName.FISHLORD_ADVANCED_ROOM_ACT_RANK_CURR, dip, imq);
        if (playerScore != null)
        {
            StatFishlordAdvancedRoomActRankItem tmp = new StatFishlordAdvancedRoomActRankItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(playerScore["genTime"]).ToLocalTime().ToShortDateString();

            if (playerScore.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(playerScore["nickName"]);

            if (playerScore.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToString(playerScore["playerId"]);

            if (playerScore.ContainsKey("points"))
                tmp.m_score = Convert.ToInt32(playerScore["points"]);

            return OpRes.opres_success;
        }
        return OpRes.op_res_not_found_data;
    }
}
//////////////////////////////////////////////////////////////////////////////////
//每日礼包统计
public class StatDailyGiftItem 
{
    public string m_time;
    public Dictionary<int, int[]> m_giftList = new Dictionary<int, int[]>();

    public string getDetail(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "boxId";
        uParam.m_value = index.ToString();
        uParam.m_url = DefCC.ASPX_STAT_DAILY_GIFT_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time.Replace(' ', '+'));
        return Tool.genHyperlink(uParam);
    }
}
public class QueryTypeStatDailyGift : QueryBase 
{
    private List<StatDailyGiftItem> m_result = new List<StatDailyGiftItem>();
    public StatDailyGiftItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatDailyGiftItem item = new StatDailyGiftItem();
        item.m_time = time;
        item.m_giftList.Add(0, new int[] {0,  0 });
        item.m_giftList.Add(1, new int[] { 0, 0 });
        item.m_giftList.Add(2, new int[] { 0, 0 });
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.And(
                            Query.LT("CreateTime", BsonValue.Create(maxt)),
                            Query.GTE("CreateTime", BsonValue.Create(mint))
                            );
        IMongoQuery imq2 = Query.And(Query.GTE("PayCode", 29), Query.LTE("PayCode", 31));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("status", 0));
        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.RECHARGE_TOTAL, serverId, DbName.DB_PAYMENT, imq,
             0, 0, null, "CreateTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["CreateTime"]).ToLocalTime().ToShortDateString();

            StatDailyGiftItem tmp = IsCreate(time);

            if (data.ContainsKey("PayCode"))
            {
                int payCode = Convert.ToInt32(data["PayCode"]) - 29;

                if (tmp.m_giftList.ContainsKey(payCode))
                {
                    tmp.m_giftList[payCode][0] += 1;

                    if (data.ContainsKey("exReward"))
                    {
                        bool res = Convert.ToBoolean(data["exReward"]);
                        if (res)
                            tmp.m_giftList[payCode][1] += 1;
                    }
                }

            }
        }
        return OpRes.opres_success;
    }
}
//每日礼包详情
public class StatDailyGiftDetailItem 
{
    public int m_playerId;
    public int m_item24;
    public int m_item25;
    public int m_item26;
    public int m_item27;
}
public class QueryTypeStatDailyGiftDetail : QueryBase 
{
    private List<StatDailyGiftDetailItem> m_result = new List<StatDailyGiftDetailItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("CreateTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("CreateTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ("PayCode", p.m_op);
        IMongoQuery imq = Query.And(imq1, imq2, imq3, Query.EQ("status", 0));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.RECHARGE_TOTAL, imq, serverId, DbName.DB_PAYMENT);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.RECHARGE_TOTAL, serverId, DbName.DB_PAYMENT, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "CreateTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatDailyGiftDetailItem tmp = new StatDailyGiftDetailItem();
            m_result.Add(tmp);

            if (data.ContainsKey("PlayerId"))
                tmp.m_playerId = Convert.ToInt32(data["PlayerId"]);

            if (data.ContainsKey("item_24"))
                tmp.m_item24 = Convert.ToInt32(data["item_24"]);

            if (data.ContainsKey("item_25"))
                tmp.m_item25 = Convert.ToInt32(data["item_25"]);

            if (data.ContainsKey("item_26"))
                tmp.m_item26 = Convert.ToInt32(data["item_26"]);

            if (data.ContainsKey("item_27"))
                tmp.m_item27 = Convert.ToInt32(data["item_27"]);
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////
//南海寻宝
public class statTreasureHuntItem
{
    public string m_time;
    public int m_playerId;
    public int m_roomId;
    public int m_joinCount;
    public long m_item1;
    public long m_item24;
    public long m_item25;
    public long m_item26;
    public long m_item27;
    public long m_item43;
    public long m_item44;
    public long m_item45;
    public long m_item46;

    public int m_cost24;
    public int m_cost25;
    public int m_cost26;
    public int m_cost27;
    public int m_cost2;

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "roomId";
        uParam.m_value = m_roomId.ToString();
        uParam.m_url = DefCC.ASPX_STAT_TREASURE_HUNT_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("playerId", m_playerId);
        uParam.addExParam("time", m_time);
        return Tool.genHyperlink(uParam);
    }

    public long getTotalGold()
    {
        long total = 0;
        total = m_item1 + m_item24 * 100000 + m_item25 * 400000 + m_item26 * 1000000 + m_item27 * 4000000
                        + m_item43 * 2000 + m_item44 * 8000 + m_item45 * 20000 + m_item46 * 80000;
        return total;
    }

    public string getRoomName()
    {
        string roomName = m_roomId.ToString();
        switch (m_roomId)
        {
            case 0:
                roomName = "新手引导";break;
            case 1:
                roomName = "青铜"; break;
            case 2:
                roomName = "白银"; break;
            case 3:
                roomName = "黄金"; break;
            case 4:
                roomName = "钻石"; break;
        }
        return roomName;
    }
}
public class QueryStatTreasureHunt : QueryBase
{
    private List<statTreasureHuntItem> m_result = new List<statTreasureHuntItem>();
    private string regex_playerId = @"\d+(,\d+)*$";

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);

        IMongoQuery imq = Query.And(imq3, Query.EQ("roomId", p.m_op));

        //玩家ID不为空
        IMongoQuery imq4 = null;
        if (!string.IsNullOrEmpty(p.m_param))
        {
            Match match = Regex.Match(p.m_param, regex_playerId);
            if (!match.Success)
                return OpRes.op_res_param_not_valid;
            string[] playerIds = p.m_param.Split(',');
            int playerId = 0;
            int k = 0;
            foreach (var player in playerIds)
            {
                playerId = Convert.ToInt32(player);

                if (k == 0)
                {
                    imq4 = Query.EQ("playerId", playerId);
                }
                else
                {
                    imq4 = Query.Or(imq4, Query.EQ("playerId", playerId));
                }
                k++;
            }
            imq = Query.And(imq, imq4);
            return query(user, imq, p); //个别玩家
        }

        return queryTotal(user, imq, p); //所有玩家
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_TREASURE_HUNT_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TREASURE_HUNT_PLAYER, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        statTreasureHuntItem tmp1 = new statTreasureHuntItem();
        m_result.Add(tmp1);

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            statTreasureHuntItem tmp = new statTreasureHuntItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            tmp.m_roomId = Convert.ToInt32(data["roomId"]);
            tmp1.m_roomId = tmp.m_roomId;

            if (data.ContainsKey("joinCount"))
                tmp.m_joinCount = Convert.ToInt32(data["joinCount"]);

            //==============门票
            //门票青铜鱼雷
            if (data.ContainsKey("cost_24"))
                tmp.m_cost24 = Convert.ToInt32(data["cost_24"]);

            //门票白银鱼雷
            if (data.ContainsKey("cost_25"))
                tmp.m_cost25 = Convert.ToInt32(data["cost_25"]);

            //门票黄金鱼雷
            if (data.ContainsKey("cost_26"))
                tmp.m_cost26 = Convert.ToInt32(data["cost_26"]);

            //门票钻石鱼雷
            if (data.ContainsKey("cost_27"))
                tmp.m_cost27 = Convert.ToInt32(data["cost_27"]);

            //门票钻石
            if (data.ContainsKey("cost_2"))
                tmp.m_cost2 = Convert.ToInt32(data["cost_2"]);

            ////////////////获取
            //金币
            if (data.ContainsKey("item_1"))
                tmp.m_item1 = Convert.ToInt64(data["item_1"]);

            //青铜鱼雷
            if (data.ContainsKey("item_24"))
                tmp.m_item24 = Convert.ToInt64(data["item_24"]);

            //白银鱼雷
            if (data.ContainsKey("item_25"))
                tmp.m_item25 = Convert.ToInt64(data["item_25"]);

            //黄金鱼雷
            if (data.ContainsKey("item_26"))
                tmp.m_item26 = Convert.ToInt64(data["item_26"]);

            //钻石鱼雷
            if (data.ContainsKey("item_27"))
                tmp.m_item27 = Convert.ToInt64(data["item_27"]);

            //////碎片///////
            //青铜鱼雷碎片
            if (data.ContainsKey("item_43"))
                tmp.m_item43 = Convert.ToInt64(data["item_43"]);

            //白银鱼雷碎片
            if (data.ContainsKey("item_44"))
                tmp.m_item44 = Convert.ToInt64(data["item_44"]);

            //黄金鱼雷碎片
            if (data.ContainsKey("item_45"))
                tmp.m_item45 = Convert.ToInt64(data["item_45"]);

            //钻石鱼雷碎片
            if (data.ContainsKey("item_46"))
                tmp.m_item46 = Convert.ToInt64(data["item_46"]);


            tmp1.m_cost24 += tmp.m_cost24;
            tmp1.m_cost25 += tmp.m_cost25;
            tmp1.m_cost26 += tmp.m_cost26;
            tmp1.m_cost27 += tmp.m_cost27;
            tmp1.m_cost2 += tmp.m_cost2;

            tmp1.m_item1 += tmp.m_item1;
            tmp1.m_item24 += tmp.m_item24;
            tmp1.m_item25 += tmp.m_item25;
            tmp1.m_item26 += tmp.m_item26;
            tmp1.m_item27 += tmp.m_item27;

            tmp1.m_item43 += tmp.m_item43;
            tmp1.m_item44 += tmp.m_item44;
            tmp1.m_item45 += tmp.m_item45;
            tmp1.m_item46 += tmp.m_item46;
            tmp1.m_joinCount += tmp.m_joinCount;
        }

        return OpRes.opres_success;
    }

    private OpRes queryTotal(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_TREASURE_HUNT_ROOM, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TREASURE_HUNT_ROOM, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        statTreasureHuntItem tmp1 = new statTreasureHuntItem();
        m_result.Add(tmp1);

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            statTreasureHuntItem tmp = new statTreasureHuntItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_roomId = Convert.ToInt32(data["roomId"]);
            tmp1.m_roomId = tmp.m_roomId;

            if (data.ContainsKey("joinCount"))
                tmp.m_joinCount = Convert.ToInt32(data["joinCount"]);

            //==============门票
            //门票青铜鱼雷
            if (data.ContainsKey("cost_24"))
                tmp.m_cost24 = Convert.ToInt32(data["cost_24"]);

            //门票白银鱼雷
            if (data.ContainsKey("cost_25"))
                tmp.m_cost25 = Convert.ToInt32(data["cost_25"]);

            //门票黄金鱼雷
            if (data.ContainsKey("cost_26"))
                tmp.m_cost26 = Convert.ToInt32(data["cost_26"]);

            //门票钻石鱼雷
            if (data.ContainsKey("cost_27"))
                tmp.m_cost27 = Convert.ToInt32(data["cost_27"]);

            //门票钻石
            if (data.ContainsKey("cost_2"))
                tmp.m_cost2 = Convert.ToInt32(data["cost_2"]);

            ////////////////获取
            //金币
            if (data.ContainsKey("item_1"))
                tmp.m_item1 = Convert.ToInt64(data["item_1"]);

            //青铜鱼雷
            if (data.ContainsKey("item_24"))
                tmp.m_item24 = Convert.ToInt64(data["item_24"]);

            //白银鱼雷
            if (data.ContainsKey("item_25"))
                tmp.m_item25 = Convert.ToInt64(data["item_25"]);

            //黄金鱼雷
            if (data.ContainsKey("item_26"))
                tmp.m_item26 = Convert.ToInt64(data["item_26"]);

            //钻石鱼雷
            if (data.ContainsKey("item_27"))
                tmp.m_item27 = Convert.ToInt64(data["item_27"]);

            //////碎片///////
            //青铜鱼雷碎片
            if (data.ContainsKey("item_43"))
                tmp.m_item43 = Convert.ToInt64(data["item_43"]);

            //白银鱼雷碎片
            if (data.ContainsKey("item_44"))
                tmp.m_item44 = Convert.ToInt64(data["item_44"]);

            //黄金鱼雷碎片
            if (data.ContainsKey("item_45"))
                tmp.m_item45 = Convert.ToInt64(data["item_45"]);

            //钻石鱼雷碎片
            if (data.ContainsKey("item_46"))
                tmp.m_item46 = Convert.ToInt64(data["item_46"]);


            tmp1.m_cost24 += tmp.m_cost24;
            tmp1.m_cost25 += tmp.m_cost25;
            tmp1.m_cost26 += tmp.m_cost26;
            tmp1.m_cost27 += tmp.m_cost27;
            tmp1.m_cost2 += tmp.m_cost2;

            tmp1.m_item1 += tmp.m_item1;
            tmp1.m_item24 += tmp.m_item24;
            tmp1.m_item25 += tmp.m_item25;
            tmp1.m_item26 += tmp.m_item26;
            tmp1.m_item27 += tmp.m_item27;

            tmp1.m_item43 += tmp.m_item43;
            tmp1.m_item44 += tmp.m_item44;
            tmp1.m_item45 += tmp.m_item45;
            tmp1.m_item46 += tmp.m_item46;
            tmp1.m_joinCount += tmp.m_joinCount;
        }

        return OpRes.opres_success;
    }
}
//南海寻宝详情
public class QueryStatTreasureHuntDetail : QueryBase
{
    private List<statTreasureHuntItem> m_result = new List<statTreasureHuntItem>();
    private string regex_playerId = @"\d+(,\d+)*$";

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.And(imq1, imq2);
        IMongoQuery imq = Query.And(Query.EQ("roomId", p.m_op), imq3);

        //玩家ID不为空
        if (Convert.ToInt32(p.m_param) != 0)
        {
            IMongoQuery imq4 = Query.EQ("playerId", Convert.ToInt32(p.m_param));
            imq = Query.And(imq, imq4);
        }
        return query(user, imq, p); //所有玩家
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_TREASURE_HUNT_PLAYER_DETAIL, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TREASURE_HUNT_PLAYER_DETAIL, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            statTreasureHuntItem tmp = new statTreasureHuntItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            tmp.m_roomId = Convert.ToInt32(data["roomId"]);

            //金币
            if (data.ContainsKey("item_1"))
                tmp.m_item1 = Convert.ToInt64(data["item_1"]);

            //青铜鱼雷
            if (data.ContainsKey("item_24"))
                tmp.m_item24 = Convert.ToInt64(data["item_24"]);

            //白银鱼雷
            if (data.ContainsKey("item_25"))
                tmp.m_item25 = Convert.ToInt64(data["item_25"]);

            //黄金鱼雷
            if (data.ContainsKey("item_26"))
                tmp.m_item26 = Convert.ToInt64(data["item_26"]);

            //钻石鱼雷
            if (data.ContainsKey("item_27"))
                tmp.m_item27 = Convert.ToInt64(data["item_27"]);

            ////////////////////////////碎片
            //青铜鱼雷碎片
            if (data.ContainsKey("item_43"))
                tmp.m_item43 = Convert.ToInt64(data["item_43"]);

            //白银鱼雷碎片
            if (data.ContainsKey("item_44"))
                tmp.m_item44 = Convert.ToInt64(data["item_44"]);

            //黄金鱼雷碎片
            if (data.ContainsKey("item_45"))
                tmp.m_item45 = Convert.ToInt64(data["item_45"]);

            //钻石鱼雷碎片
            if (data.ContainsKey("item_46"))
                tmp.m_item46 = Convert.ToInt64(data["item_46"]);
        }

        return OpRes.opres_success;
    }
}

/////////////////////////////////////////////////////////////////////////
//流失点统计
public class StatNewGuildLosePointItem
{
    public string m_time;

    public int m_clickStartGame;
    public int m_hitFirstGift;
    public int m_loadGameFromGift;
    public int m_completeLoadGame;
    public int m_fireFirstBullet;
    public int m_killFirstFish;
    public int m_updateFirstLevel;

    public int m_clientEnterPlatform;
    public int m_updateSecondLevel;
    public int m_clientFirstUseLock;

    public int m_room2ExchangePerson;
    public int m_vipExchangePerson;
    public int m_room2EnterCount;
    public int m_room5EnterCount;
    public int m_room6EnterCount;
    public int m_room3EnterCount;

    public int m_regeditCount;

    public Dictionary<int, long> m_turretTime = new Dictionary<int, long>();

    public string getRate(int da1)
    {
        if (m_regeditCount == 0)
            return da1.ToString();

        double factGain = (double)da1 / m_regeditCount;
        return  (Math.Round(factGain, 3)*100).ToString() + '%';
    }
}
public class QueryTypeStatNewGuildLosePoint : QueryBase
{
    private List<StatNewGuildLosePointItem> m_result = new List<StatNewGuildLosePointItem>();
    public string[] m_fields = new string[] { "clickStartGame", "hitFirstGift", "loadGameFromGift", 
        "completeLoadGame", "fireFirstBullet", "killFirstFish", "updateFirstLevel","clientEnterPlatform","updateSecondLevel","clientFirstUseLock",
        "turretTime_2","turretTime_3","turretTime_4","turretTime_5","turretTime_6","turretTime_7","turretTime_8","turretTime_9","turretTime_10",
        "turretTime_11","turretTime_12"};

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(p, imq, user, m_fields);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user, string[] m_fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_NEW_GUILD_LOSE_POINT, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatNewGuildLosePointItem tmp = new StatNewGuildLosePointItem();
            m_result.Add(tmp);

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();

            if (data.ContainsKey("clickStartGame"))
                tmp.m_clickStartGame = Convert.ToInt32(data["clickStartGame"]);

            if (data.ContainsKey("hitFirstGift"))
                tmp.m_hitFirstGift = Convert.ToInt32(data["hitFirstGift"]);

            if (data.ContainsKey("loadGameFromGift"))
                tmp.m_loadGameFromGift = Convert.ToInt32(data["loadGameFromGift"]);

            if (data.ContainsKey("completeLoadGame"))
                tmp.m_completeLoadGame = Convert.ToInt32(data["completeLoadGame"]);

            if (data.ContainsKey("fireFirstBullet"))
                tmp.m_fireFirstBullet = Convert.ToInt32(data["fireFirstBullet"]);

            if (data.ContainsKey("killFirstFish"))
                tmp.m_killFirstFish = Convert.ToInt32(data["killFirstFish"]);

            if (data.ContainsKey("updateFirstLevel"))
                tmp.m_updateFirstLevel = Convert.ToInt32(data["updateFirstLevel"]);

            if (data.ContainsKey("clientEnterPlatform"))
                tmp.m_clientEnterPlatform = Convert.ToInt32(data["clientEnterPlatform"]);

            if (data.ContainsKey("updateSecondLevel"))
                tmp.m_updateSecondLevel = Convert.ToInt32(data["updateSecondLevel"]);

            if (data.ContainsKey("clientFirstUseLock"))
                tmp.m_clientFirstUseLock = Convert.ToInt32(data["clientFirstUseLock"]);

            for (j = 2; j <= 20; j++) 
            {
                string str = "turretTime_" + j;
                long turretTime = 0;
                if (data.ContainsKey(str))
                    turretTime = Convert.ToInt64(data[str]);

                tmp.m_turretTime.Add(j, turretTime);
            }

            //中级场兑换人数
            IMongoQuery imq_1 =  Query.EQ("genTime", time.Date);
            tmp.m_room2ExchangePerson = DBMgr.getInstance().executeDistinct(TableName.STAT_FISHLORD_MIDDLE_ROOM_EXCHANGE_DETAIL, dip, "playerId", imq_1);

            //中级场第一次兑换vip经验人数
            IMongoQuery imq_2 = Query.And(imq_1, Query.EQ("roomId", 2), Query.GT("firstExexchange", 0));
            tmp.m_vipExchangePerson = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_ROOM_EXCHANGE, dip, "playerId", imq_2);

            //进入中级场人数
            IMongoQuery imq_3 = Query.And(imq_1, Query.EQ("roomId", 2), Query.GT("enterCount", 0));
            tmp.m_room2EnterCount = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_ROOM_PLAYER, dip, "playerId", imq_3);

            //进入龙宫场人数
            IMongoQuery imq_4 = Query.And(imq_1, Query.EQ("roomId", 5), Query.GT("enterCount", 0));
            tmp.m_room5EnterCount = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_ROOM_PLAYER, dip, "playerId", imq_4);

            //进入巨鲨场人数
            IMongoQuery imq_5 = Query.And(imq_1, Query.EQ("roomId", 6), Query.GT("enterCount", 0));
            tmp.m_room6EnterCount = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_ROOM_PLAYER, dip, "playerId", imq_5);

            //进入高级场人数
            IMongoQuery imq_6 = Query.And(imq_1, Query.EQ("roomId", 3), Query.GT("enterCount", 0));
            tmp.m_room3EnterCount = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_ROOM_PLAYER, dip, "playerId", imq_6);


            int accServer = DBMgr.getInstance().getSpecialServerId(DbName.DB_ACCOUNT);
            if (accServer == -1)
                continue;

            IMongoQuery imq1 = Query.EQ("genTime", time.Date);
            List<Dictionary<string, object>> regeditList = DBMgr.getInstance().executeQuery(
                TableName.CHANNEL_TD, accServer, DbName.DB_ACCOUNT, imq1, 0, 0, new string[]{"regeditCount"});

            if (regeditList != null && regeditList.Count != 0)
            {
                foreach (var d in regeditList)
                {
                    if (d.ContainsKey("regeditCount"))
                        tmp.m_regeditCount += Convert.ToInt32(d["regeditCount"]);
                }
            }
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
//明日礼包
public class StatTomorrowGiftItem 
{
    public string m_time;
    public int m_triggerTommorowGift;
    public int m_clickTommorowGift;
    public int m_clientClickTommorowGiftPersonTime;
    public int m_receiveTommorowGift;
}

public class QueryTypeStatTomorrowGift : QueryBase 
{
    private List<StatTomorrowGiftItem> m_result = new List<StatTomorrowGiftItem>();
    public string[] m_fields = new string[] { "genTime","triggerTommorowGift", "clickTommorowGift", "clientClickTommorowGiftPersonTime", "receiveTommorowGift" };

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(imq, Query.Or(
                    Query.Exists("triggerTommorowGift"),
                    Query.Exists("clickTommorowGift"),
                    Query.Exists("clientClickTommorowGiftPersonTime"),
                    Query.Exists("receiveTommorowGift")
               ));

        return query(p, imq, user, m_fields);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user, string[] m_fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_NEW_GUILD_LOSE_POINT, dip, imq, 0, 0, m_fields, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatTomorrowGiftItem tmp = new StatTomorrowGiftItem();
            m_result.Add(tmp);

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();

            if (data.ContainsKey("triggerTommorowGift"))
                tmp.m_triggerTommorowGift = Convert.ToInt32(data["triggerTommorowGift"]);

            if (data.ContainsKey("clickTommorowGift"))
                tmp.m_clickTommorowGift = Convert.ToInt32(data["clickTommorowGift"]);

            if (data.ContainsKey("clientClickTommorowGiftPersonTime"))
                tmp.m_clientClickTommorowGiftPersonTime = Convert.ToInt32(data["clientClickTommorowGiftPersonTime"]);

            if (data.ContainsKey("receiveTommorowGift"))
                tmp.m_receiveTommorowGift = Convert.ToInt32(data["receiveTommorowGift"]);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////
//开服活动统计
/**
 * type_int1_int2
 * int1对应以下 SignLog 0  TaskLog 1  ExchangeLog 2   BuyLog 3   ClickGiftLog 4
 */

//签到 任务 兑换 礼包
public class StatSdActItem 
{
    public string m_time;
    public Dictionary<int, int> m_sign_data = new Dictionary<int, int>();
    public Dictionary<int, int[]> m_task_data = new Dictionary<int, int[]>();
    public Dictionary<int, int[]> m_exchange_data = new Dictionary<int, int[]>();
    public Dictionary<int, int[]> m_gift_data = new Dictionary<int, int[]>();
}
public class QueryTypeStatSdAct : QueryBase 
{
    private List<StatSdActItem> m_result = new List<StatSdActItem>();
    public StatSdActItem IsCreate(string time, int op)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatSdActItem item = new StatSdActItem();
        m_result.Add(item);
        item.m_time = time;

        //初始化
         int i = 0;
        switch(op)
        {
            case 1:
                for (i = 1; i <= 15; i++) 
                {
                    item.m_sign_data.Add(i, 0);
                }
                break;
            case 2:
                for (i = 1; i <= 5; i++) 
                {
                    item.m_task_data.Add(i, new int[] { 0, 0 });
                }
                break;
            case 3:
                for (i = 1; i <= 5; i++)
                {
                    item.m_exchange_data.Add(i, new int[] { 0, 0 });
                }
                break;
            case 4:
                for (i = 0; i <= 3; i++)
                {
                    item.m_gift_data.Add(i, new int[] { 0, 0 });
                }
                break;
        }
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        string str = "";
        switch(p.m_op)
        {
            case 1:  
            //签到
                string[] m_fields1 = new string[16];
                m_fields1[0] = "genTime";
                for(int k=1; k < 16; k++)
                {
                    str = "type_0_" + k;
                    m_fields1[k] = str;
                }
                return query(p, imq, user, m_fields1);
            case 2: 
            //任务
                string[] m_fields2 = new string[6];
                m_fields2[0] = "genTime";
                for(int k=1; k < 6; k++)
                {
                    str = "type_1_" + k;
                    m_fields2[k] = str;
                }
                return query(p, imq, user, m_fields2);
            case 3: 
            //兑换
               string[] m_fields3 = new string[6];
               m_fields3[0] = "genTime";
                for(int k = 1; k < 6; k++)
                {
                    str = "type_2_" + k;
                    m_fields3[k] = str;
                }
                return query(p, imq, user, m_fields3);
            case 4: 
            //礼包
                string[] m_fields4 = new string[5];
                m_fields4[0] = "genTime";
                for(int k=1; k < 4; k++)
                {
                    str = "type_3_" + k;
                    m_fields4[k] = str;
                }
                m_fields4[4] = "type_4";
                return query(p, imq, user, m_fields4);
        }
        return OpRes.op_res_failed;
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user, string[] m_fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_SD_ACT, dip, imq, 0, 0, m_fields, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        string str = "", time = "";
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            
            time = Convert.ToDateTime(data["genTime"]).ToLocalTime().Date.ToShortDateString();

            StatSdActItem tmp = IsCreate(time, param.m_op);

            switch(param.m_op)
            {
                case 1: //签到
                    for (k = 1; k < 16; k++)
                    {
                        str = "type_0_" + k;
                        if (data.ContainsKey(str))
                            tmp.m_sign_data[k] += Convert.ToInt32(data[str]);
                    }
                    break;
                case 2: //任务
                    for (k = 1; k < 6; k++) 
                    {
                        str = "type_1_" + k;
                        if (data.ContainsKey(str))
                        {
                            tmp.m_task_data[k][0] += 1;
                            tmp.m_task_data[k][1] += Convert.ToInt32(data[str]);
                        }
                    }
                    break;
                case 3: //兑换
                    for (k = 1; k < 6; k++)
                    {
                        str = "type_2_" + k;
                        if(data.ContainsKey(str))
                        {
                            tmp.m_exchange_data[k][0] += 1;
                            tmp.m_exchange_data[k][1] += Convert.ToInt32(data[str]);
                        }
                    }
                    break;
                case 4://礼包
                    if (data.ContainsKey("type_4")) 
                    {
                        tmp.m_gift_data[0][0] += 1;
                        tmp.m_gift_data[0][1] += Convert.ToInt32(data["type_4"]);
                    }

                    if (data.ContainsKey("type_3_1")) 
                    {
                        tmp.m_gift_data[1][0] += 1;
                        tmp.m_gift_data[1][1] += Convert.ToInt32(data["type_3_1"]);
                    }

                    if (data.ContainsKey("type_3_2"))
                    {
                        tmp.m_gift_data[2][0] += 1;
                        tmp.m_gift_data[2][1] += Convert.ToInt32(data["type_3_2"]);
                    }

                    if (data.ContainsKey("type_3_3"))
                    {
                        tmp.m_gift_data[3][0] += 1;
                        tmp.m_gift_data[3][1] += Convert.ToInt32(data["type_3_3"]);
                    }
                    break;
            }
        }
        return OpRes.opres_success;
    }
}

//开服抽奖统计
public class StatSdLotteryActItem 
{
    public string m_time;
    public string m_channelNo;
    public Dictionary<int, int[]> m_lottery = new Dictionary<int, int[]>();

    public string getChannelName() 
    {
        string channelName = m_channelNo;
        var cd = TdChannel.getInstance().getValue(m_channelNo.PadLeft(6, '0'));
        if (cd != null)
            channelName = cd.m_channelName;

        return channelName;
    }

    public string getDetail(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "lotteryId";
        uParam.m_value = index.ToString();
        uParam.m_url = DefCC.ASPX_STAT_SD_LOTTERY_ACT_DETAIL;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time);
        uParam.addExParam("channel", m_channelNo);
        return Tool.genHyperlink(uParam);
    }
}
public class QueryTypeStatSdLotteryAct : QueryBase 
{
    private List<StatSdLotteryActItem> m_result = new List<StatSdLotteryActItem>();
    private string[] m_fields = new string[4];
    public StatSdLotteryActItem IsCreate(string time, string channel)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time && d.m_channelNo == channel)
                return d;
        }

        StatSdLotteryActItem item = new StatSdLotteryActItem();
        m_result.Add(item);
        item.m_time = time;
        item.m_channelNo = channel;
        item.m_lottery.Add(1, new int[] { 0, 0 });
        item.m_lottery.Add(2, new int[] { 0, 0 });
        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        m_fields[0] = "genTime";
        switch(p.m_op){
            case 1:
                m_fields[1] = "type_1_1";
                m_fields[2] = "type_1_10";
                break;
            case 2:
                m_fields[1] = "type_2_1";
                m_fields[2] = "type_2_10";
                break;
        }

        imq = Query.And(
                imq,
                Query.Or(
                    Query.Exists(m_fields[1]), Query.Exists(m_fields[2])
                )
            );

        if (p.m_channelNo != "")
            imq = Query.And(imq, Query.EQ("Channel", p.m_channelNo));

        m_fields[3] = "Channel";

        imq = Query.And(imq, Query.Exists("Channel"));
        return query(p, imq, user, m_fields);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user, string[] m_fields)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_SD_LOTTERY_ACT, dip, imq, 0, 0, m_fields, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().Date.ToShortDateString();
            string channelNo = Convert.ToString(data["Channel"]);
            StatSdLotteryActItem tmp = IsCreate(time, channelNo);

            string str1 = "type_" + param.m_op +"_1";
            if(data.ContainsKey(str1))
            {
                tmp.m_lottery[1][0] += 1;
                tmp.m_lottery[1][1] += Convert.ToInt32(data[str1]);
            }

            string str2 = "type_" + param.m_op + "_10";
            if (data.ContainsKey(str2))
            {
                tmp.m_lottery[2][0] += 1;
                tmp.m_lottery[2][1] += Convert.ToInt32(data[str2]);
            }

        }
        return OpRes.opres_success;
    }
}

//开服抽奖统计详情
public class StatSdLotteryActDetailItem 
{
    public string m_time;
    public string m_nickname;
    public int m_playerId;
    public List<int> m_reward = new List<int>();
}
public class QueryTypeStatSdLotteryActDetail : QueryBase 
{
    private List<StatSdLotteryActDetailItem> m_result = new List<StatSdLotteryActDetailItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(
               imq,
               Query.EQ("Channel", p.m_channelNo),
               Query.Or(
                   Query.Exists("type_" + p.m_op + "_1"),
                   Query.Exists("type_" + p.m_op + "_10")
               )
        );

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_SD_LOTTERY_ACT, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_SD_LOTTERY_ACT, dip, imq, 
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0;
        int index = 10 * (param.m_op - 1) + 1;
        int len = 10 * param.m_op + 1;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().Date.ToShortDateString();
            StatSdLotteryActDetailItem tmp = new StatSdLotteryActDetailItem();
            tmp.m_time = time;
            m_result.Add(tmp);

            if (data.ContainsKey("playerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

                Dictionary<string, object> ret = getPlayerProperty(tmp.m_playerId, user, new string[]{"nickname"});
                if (ret != null)
                    tmp.m_nickname = Convert.ToString(ret["nickname"]);
            }

            for (int k = index; k < len; k++) 
            {
                string rewardId = "id_" + k;
                if(data.ContainsKey(rewardId))
                {
                    int times = Convert.ToInt32(data[rewardId]);
                    tmp.m_reward.Add(times);
                }else{
                    tmp.m_reward.Add(0);
                }
            }
        }
        return OpRes.opres_success;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////
//背包购买
public class StatItemRecharge
{
    public string m_time;
    public string m_channelNo;
    public int m_playerId;
    public Dictionary<int, int[]> m_rechargeItem = new Dictionary<int, int[]>();

    //获取渠道名称
    public string getChannelName(string channel)
    {
        string channelName = channel;
        var cd = TdChannel.getInstance().getValue(channel.PadLeft(6, '0'));
        if (cd != null)
            channelName = cd.m_channelName;

        return channelName;
    }
}
public class QueryTypeStatPlayerItemRecharge : QueryBase 
{
    private List<StatItemRecharge> m_result = new List<StatItemRecharge>();
    //锁定5   冰冻8  狂暴17  火力符文11  瞄准符文12  炮管符文13 炮座符文14  召唤9  普通鱼雷23  升级石16
    private int[] m_itemList = new int[] { 5, 8, 17, 11, 12, 13, 14, 9, 23, 16 };
    public StatItemRecharge IsCreate(string time, string channel)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatItemRecharge item = new StatItemRecharge();
        m_result.Add(item);
        item.m_time = time;
        item.m_channelNo = channel;

        for (int i = 0; i < 10; i++) 
        {
            item.m_rechargeItem.Add(i, new int[] { 0, 0 });
        }

        return item;
    }
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //全部
        if (p.m_op == 0)
        {
            imq = Query.And(imq, Query.Exists("channel"));

            if (p.m_channelNo != "")
                imq = Query.And(imq, Query.EQ("channel", p.m_channelNo));

            return query1(p, imq, user);
        }
        else
        {  //个人

            if (string.IsNullOrEmpty(p.m_playerId))
                return OpRes.op_res_param_not_valid;

            int playerId = 0;
            if(!int.TryParse(p.m_playerId, out playerId))
                return OpRes.op_res_param_not_valid;

            imq = Query.And(imq, Query.EQ("playerId", playerId), Query.EQ("reason", 14));

            return query2(p, imq, user);
        }
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    //全部
    private OpRes query1(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_PLAYER_ITEM, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().Date.ToShortDateString();
            string channelNo = Convert.ToString(data["channel"]);
            StatItemRecharge tmp = IsCreate(time, channelNo);

            j = 0;
            foreach(int itemId in m_itemList)
            {
                string str1 = "itemPerson_" + itemId, str2 = "itemCount_" + itemId;

                int itemPerson = 0, itemCount = 0;
                if (data.ContainsKey(str1))
                    itemPerson = Convert.ToInt32(data[str1]);

                if (data.ContainsKey(str2))
                    itemCount = Convert.ToInt32(data[str2]);

                tmp.m_rechargeItem[j][0] += itemPerson;
                tmp.m_rechargeItem[j][1] += itemCount;

                j++;
            }
        }
        return OpRes.opres_success;
    }

    //个人
    private OpRes query2(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_ITEM, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, j = 0, index = 0, itemId = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().Date.ToShortDateString();

            string channelNo = "";
            if(data.ContainsKey("channel"))
                channelNo = Convert.ToString(data["channel"]);

            StatItemRecharge tmp = IsCreate(time, channelNo);

            tmp.m_channelNo = channelNo;
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("itemId")) 
            {
                itemId = Convert.ToInt32(data["itemId"]);
                index = Array.IndexOf(m_itemList, itemId);
                if (index != -1)
                    tmp.m_rechargeItem[index][1] += 1;
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////
//在线奖励
public class StatOnlineRewardItem 
{
    public string m_time;
    public string m_channelNo;
    public List<int> m_recvCount = new List<int>();

    public string getChannelName()
    {
        string channelName = m_channelNo;
        var cd = TdChannel.getInstance().getValue(m_channelNo.PadLeft(6, '0'));
        if (cd != null)
            channelName = cd.m_channelName;

        return channelName;
    }

}
public class QueryTypeStatOnlineReward : QueryBase 
{
    private List<StatOnlineRewardItem> m_result = new List<StatOnlineRewardItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //渠道号
        if (p.m_channelNo != "")
            imq = Query.And(imq, Query.EQ("ChannelID", p.m_channelNo));

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_ONLINE_REWARD, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_ONLINE_REWARD, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().Date.ToShortDateString();
            StatOnlineRewardItem tmp = new StatOnlineRewardItem();
            tmp.m_time = time;
            m_result.Add(tmp);

            if (data.ContainsKey("ChannelID"))
                tmp.m_channelNo = Convert.ToString(data["ChannelID"]);

            for (k = 1; k <= 6; k++)
            {
                string rewardId = "index_" + k;
                if (data.ContainsKey(rewardId))
                {
                    int times = Convert.ToInt32(data[rewardId]);
                    tmp.m_recvCount.Add(times);
                }
                else
                {
                    tmp.m_recvCount.Add(0);
                }
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//十一活动
//签到
public class StatActSignItem 
{
    public int m_actId;
    public Dictionary<int, int> m_sign = new Dictionary<int, int>();
    public long m_outlay;
}
public class QueryTypeStatActSign : QueryBase 
{
    private StatActSignItem m_result = new StatActSignItem();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result = new StatActSignItem();

        IMongoQuery imq = Query.EQ("ActivityId", 190720);

        return query(imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_ACTIVITY_SIGN, dip, imq,
             0, 0, null, "playerId", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        StatActSignItem tmp = new StatActSignItem();
        m_result = tmp;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            if (data.ContainsKey("outlay"))
                tmp.m_outlay += Convert.ToInt64(data["outlay"]);

            for (k = 1; k <= 7; k++) 
            {
                string sign = "sign_" + k;

                int signCount = 0;
                if (data.ContainsKey(sign))
                    signCount = Convert.ToInt32(data[sign]);

                if (tmp.m_sign.ContainsKey(k))
                {
                    tmp.m_sign[k] += signCount;
                }
                else
                {
                    tmp.m_sign.Add(k, signCount);
                }
            }
        }
        return OpRes.opres_success;
    }
}

//欢乐集字
public class StatActExchangeItem 
{
    public string m_time;
    public Dictionary<int, int[]> m_exchange = new Dictionary<int, int[]>();
    public long m_outlay;
}
public class QueryTypeStatActExchange : QueryBase 
{
    private List<StatActExchangeItem> m_result = new List<StatActExchangeItem>();

    public StatActExchangeItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatActExchangeItem item = new StatActExchangeItem();
        item.m_time = time;
        m_result.Add(item);
        for (int k = 1; k <= 6; k++) 
        {
            item.m_exchange.Add(k, new int[]{0, 0});
        }
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //活动号
        imq = Query.And(imq, Query.EQ("activityId", 190720));

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_ACTIVITY_EXCHANGE, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_ACTIVITY_EXCHANGE, dip, imq,
             0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0, count = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().Date.ToShortDateString();
            StatActExchangeItem tmp = IsCreate(time);

            if (data.ContainsKey("outlay"))
                tmp.m_outlay += Convert.ToInt64(data["outlay"]);

            for (k = 1; k <= 6; k++)
            {
                string str = k.ToString();
                if (data.ContainsKey(str) )
                {
                    count = Convert.ToInt32(data[str]);
                    tmp.m_exchange[k][1] += count;

                    if(count > 0)
                        tmp.m_exchange[k][0] += 1;
                }
            }
        }
        return OpRes.opres_success;
    }
}

//冒险之路
public class StatActTaskItem 
{
    public string m_time;

    public int m_branchId;

    public int m_firstSteepCount;
    public long m_firstSteepOutlay;

    public Dictionary<int, int[]> m_branches = new Dictionary<int, int[]>();

    public Dictionary<int, long> m_outlay = new Dictionary<int, long>();
}

public class QueryTypeStatActTask : QueryBase 
{
    private List<StatActTaskItem> m_result = new List<StatActTaskItem>();

    public StatActTaskItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatActTaskItem item = new StatActTaskItem();
        item.m_time = time;
        m_result.Add(item);
        for (int k = 1; k <= 3; k++)
        {
            item.m_branches.Add(k, new int[] { 0, 0, 0, 0 ,0});
            item.m_outlay.Add(k, 0);
        }
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //活动号
        imq = Query.And(imq, Query.EQ("activityId", 190720));

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_ACTIVITY_TASK, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_ACTIVITY_TASK, dip, imq,
             0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0, stage = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().Date.ToShortDateString();
            StatActTaskItem tmp = IsCreate(time);

            if (!data.ContainsKey("branch"))
                continue;
            tmp.m_branchId = Convert.ToInt32(data["branch"]);

            if (tmp.m_branchId == 0) 
            {
                if (data.ContainsKey("task_1"))
                    tmp.m_firstSteepCount += Convert.ToInt32(data["task_1"]);

                if (data.ContainsKey("outlay"))
                    tmp.m_firstSteepOutlay += Convert.ToInt64(data["outlay"]);

                continue;
            }

            //选择人数
            tmp.m_branches[tmp.m_branchId][0] += 1;

            //步骤1
            if (data.ContainsKey("task_1"))
                tmp.m_branches[tmp.m_branchId][1] += Convert.ToInt32(data["task_1"]);

            //步骤 2 3 4
            for (k = 2; k <= 10; k++)
            {
                var branchLine = ActivityNationalDayMissionCFG.getInstance().getValue(k);
                if (branchLine != null && branchLine.m_branchId == tmp.m_branchId)
                {
                    stage = branchLine.m_stage;

                    string str = "task_" + k;
                    if (data.ContainsKey(str))
                        tmp.m_branches[tmp.m_branchId][stage + 1] += Convert.ToInt32(data[str]);
                }
            }

            if (data.ContainsKey("outlay"))
                tmp.m_outlay[tmp.m_branchId] += Convert.ToInt32(data["outlay"]);

        }
        return OpRes.opres_success;
    }
}

//礼包购买
public class StatActGiftItem 
{
    public string m_time;
    public Dictionary<int, int[]> m_gifRecharge = new Dictionary<int, int[]>();
}
public class QueryTypeStatActGift : QueryBase 
{
    private List<StatActGiftItem> m_result = new List<StatActGiftItem>();

    public StatActGiftItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatActGiftItem item = new StatActGiftItem();
        item.m_time = time;
        m_result.Add(item);
        for (int k = 0; k <= 3; k++)
        {
            item.m_gifRecharge.Add(k, new int[] { 0, 0});
        }
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //活动号
        imq = Query.And(imq, Query.EQ("activityId", 190720));

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_ACTIVITY_GIFT, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_ACTIVITY_GIFT, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0, count = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            StatActGiftItem tmp = IsCreate(time.ToShortDateString());

            for (k = 1; k < 4; k++) 
            {
                string str = "giftId_" + (k + 93);

                count = 0;
                if (data.ContainsKey(str))
                    count = Convert.ToInt32(data[str]);

                if(count > 0)
                    tmp.m_gifRecharge[k][0] += 1;

                tmp.m_gifRecharge[k][1] += count;
            }

            if (data.ContainsKey("playerId"))
            {
                int playerId = Convert.ToInt32(data["playerId"]);
                IMongoQuery imq1 = Query.And(
                                        Query.EQ("genTime", time.Date), 
                                        Query.EQ("playerId", playerId), 
                                        Query.EQ("giftType", 13)
                                        );

                Dictionary<string, object> playerGift = DBMgr.getInstance().getTableData(TableName.STAT_FISHLOR_ROOM_FU_DAI, dip, imq1,
                    new string[] {"action_1"});
                if (playerGift != null)
                {
                    count = 0;
                    if (playerGift.ContainsKey("action_1"))
                        count = Convert.ToInt32(playerGift["action_1"]);

                    if(count > 0)
                    {
                        tmp.m_gifRecharge[0][0] += 1;
                        tmp.m_gifRecharge[0][1] += count;
                    } 
                }
            }
        }
        return OpRes.opres_success;
    }
}

//抽奖统计
public class StatActLotteryItem 
{
    public string m_time;
    public int m_lotteryCount;
    public int m_lotteryPerson;

    public string getDetail()
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "time";
        uParam.m_value = m_time;
        uParam.m_url = DefCC.ASPX_STAT_NATIONAL_DAY_ACT_LOTTERY_DETAIL;
        uParam.m_target = "_blank";
        return Tool.genHyperlink(uParam);
    }
}
public class QueryTypeStatActLottery : QueryBase 
{
    private List<StatActLotteryItem> m_result = new List<StatActLotteryItem>();

    public StatActLotteryItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatActLotteryItem item = new StatActLotteryItem();
        item.m_time = time;
        m_result.Add(item);
        
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //活动号
        imq = Query.And(imq, Query.EQ("activityId", 190720));

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_ACTIVITY_LOTTERY, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0,  count1 = 0, count2 = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            StatActLotteryItem tmp = IsCreate(time.ToShortDateString());

            if (data.ContainsKey("1_1"))
            {
                count1 = Convert.ToInt32(data["1_1"]);
                tmp.m_lotteryCount += count1;

                if(count1 > 0)
                    tmp.m_lotteryPerson += 1;
            }

            if (data.ContainsKey("2_1"))
            {
                count2 = Convert.ToInt32(data["2_1"]);
                tmp.m_lotteryCount += count2;

                if (count1 == 0 && count2 > 0)
                    tmp.m_lotteryPerson += 1;
            }
        }
        return OpRes.opres_success;
    }
}

//抽奖统计详情
public class StatActLotteryDetailItem 
{
    public string m_time;
    public string m_nickName;
    public int m_playerId;
    public string m_reward;
}
public class QueryTypeStatNationalDayActLotteryDetail : QueryBase 
{
    private List<StatActLotteryDetailItem> m_result = new List<StatActLotteryDetailItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //活动号
        imq = Query.And(imq, Query.EQ("activityId", 190720));

        return query(p, imq, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_ACTIVITY_LOTTERY, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_ACTIVITY_LOTTERY, dip, imq, 
             (param.m_curPage - 1)*param.m_countEachPage, param.m_countEachPage, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            StatActLotteryDetailItem tmp = new StatActLotteryDetailItem();
            m_result.Add(tmp);

            tmp.m_time = time.ToShortDateString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("nickName"))
                tmp.m_nickName = Convert.ToString(data["nickName"]);

            string reward = "";

            if (data.ContainsKey("item_1"))
                reward += getItemName(1) + "：" + Convert.ToInt32(data["item_1"]) + "；";

            if (data.ContainsKey("item_16"))
                reward += getItemName(16) + "：" + Convert.ToInt32(data["item_16"]) + "；";

            if (data.ContainsKey("item_24"))
                reward += getItemName(24) + "：" + Convert.ToInt32(data["item_24"]) + "；";

            tmp.m_reward = reward.Trim('；');
        }
        return OpRes.opres_success;
    }

    public string getItemName(int itemId) 
    {
        string itemName = "";
        var item = ItemCFG.getInstance().getValue(itemId);
        if (item != null)
            itemName = item.m_itemName;

        return itemName;
    }

}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//追击蟹将
//追击蟹将抽奖统计
public class StatKillCrabActLotteryItem
{
    public string m_time;
    public int m_lotteryType;
    public int m_lotteryCount;
    public int m_lotteryPerson;
    public Dictionary<int, int> m_reward = new Dictionary<int, int>();

    public long m_outlay;

    public string getLotteryTypeName()
    {
        string typeName = "";
        switch (m_lotteryType)
        {
            case 1: typeName = "100宝剑抽奖"; break;
            case 2: typeName = "1000宝剑抽奖"; break;
        }
        return typeName;
    }

    public string getExParam(int index)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "lotteryId";
        uParam.m_value = m_lotteryType.ToString();
        uParam.m_url = DefCC.ASPX_STAT_KILL_CRAB_ACT_LOTTERY_DEATIL;
        uParam.m_target = "_blank";
        uParam.addExParam("time", m_time);
        return Tool.genHyperlink(uParam);
    }
}
public class StatKillCrabActLotteryList
{
    public int m_flag = 0;
    public string m_time;
    public List<StatKillCrabActLotteryItem> m_data = new List<StatKillCrabActLotteryItem>();
    public long m_goldIncome;
    public int m_dropCount;
}
public class QueryTypeStatKillCrabActLottery : QueryBase
{
    private List<StatKillCrabActLotteryList> m_result = new List<StatKillCrabActLotteryList>();
    public string[] m_fields = new string[] { "income", "dropCount" };
    public StatKillCrabActLotteryList IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
            {
                d.m_flag++;
                return d;
            }
        }

        StatKillCrabActLotteryList item = new StatKillCrabActLotteryList();
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_KILL_CRAB_LOTTERY, dip, imq, 0, 0, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["lotteryId"]).OrderByDescending(a => a["genTime"]).ToList();

        int i = 0, k = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();

            StatKillCrabActLotteryList tmp = IsCreate(time);
            tmp.m_time = time;

            StatKillCrabActLotteryItem item = new StatKillCrabActLotteryItem();
            tmp.m_data.Add(item);

            item.m_time = time;
            item.m_lotteryType = Convert.ToInt32(data["lotteryId"]);

            if (data.ContainsKey("lotteryCount"))
                item.m_lotteryCount = Convert.ToInt32(data["lotteryCount"]);

            for (k = 0; k < 9; k++)
            {
                string reward = string.Format("reward_{0}", k);
                if (data.ContainsKey(reward))
                    item.m_reward[k] = Convert.ToInt32(data[reward]);
            }

            if (data.ContainsKey("outlay"))
                item.m_outlay = Convert.ToInt64(data["outlay"]);

            //当日抽奖人数
            IMongoQuery imq_1 = Query.And(
                Query.EQ("genTime", t.Date),
                Query.EQ("lotteryId", item.m_lotteryType)
                );
            item.m_lotteryPerson = DBMgr.getInstance().executeDistinct(TableName.STAT_PUMP_KILL_CRAB_LOTTERY_PLAYER, dip, "playerId", imq_1);


            if (tmp.m_flag == 0)
            {
                IMongoQuery sq = Query.EQ("genTime", t);
                List<Dictionary<string, object>> goldList =
                            DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_KILL_CRAB_ROOM_CONSUME, dip, sq, 0, 0, m_fields, "roomId", true);
                if (goldList != null)
                {
                    for (j = 0; j < goldList.Count; j++)
                    {
                        if (goldList[j].ContainsKey("income"))
                            tmp.m_goldIncome += Convert.ToInt64(goldList[j]["income"]);

                        if (goldList[j].ContainsKey("dropCount"))
                            tmp.m_dropCount += Convert.ToInt32(goldList[j]["dropCount"]);
                    }
                }
            }
        }
        return OpRes.opres_success;
    }
}

//抽奖统计详情
public class KillCrabActLotteryDetail : PanicBoxDetail
{
}
public class QueryTypeStatKillCrabActLotteryDetail : QueryBase
{
    private List<KillCrabActLotteryDetail> m_result = new List<KillCrabActLotteryDetail>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("lotteryId", p.m_op));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_KILL_CRAB_LOTTERY_PLAYER, imq, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_KILL_CRAB_LOTTERY_PLAYER, dip, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0, s_head = 0;

        if (param.m_op == 1)
            s_head = 6;
        else
            s_head = 7;

        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            KillCrabActLotteryDetail tmp = new KillCrabActLotteryDetail();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            tmp.m_boxType = Convert.ToInt32(data["lotteryId"]);
            tmp.m_playerId = Convert.ToString(data["playerId"]);

            for (k = 0; k < s_head; k++)
            {
                string reward = string.Format("reward_{0}", k);
                if (data.ContainsKey(reward))
                    tmp.m_reward[k] = Convert.ToInt32(data[reward]);
            }
        }
        return OpRes.opres_success;
    }
}

//追击蟹将场次统计
public class KillCrabActRoomStatItem
{
    public int m_roomId;
    public long m_goldIncome;
    public int m_killCount;
    public int m_dropCount;
}
public class KillCrabActRoomStatList
{
    public string m_time;
    public Dictionary<int, KillCrabActRoomStatItem> m_data = new Dictionary<int, KillCrabActRoomStatItem>();
}
public class QueryTypeStatKillCrabActRoom : QueryBase
{
    private List<KillCrabActRoomStatList> m_result = new List<KillCrabActRoomStatList>();
    public KillCrabActRoomStatList IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        KillCrabActRoomStatList item = new KillCrabActRoomStatList();
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_KILL_CRAB_ROOM_CONSUME, dip, imq, 0, 0, null, "genTime", true);

        if (data_list == null || data_list.Count == 0)
            return OpRes.op_res_not_found_data;

        var dataList = data_list.OrderBy(a => a["roomId"]).OrderByDescending(a => a["genTime"]).ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();

            KillCrabActRoomStatList tmp = IsCreate(time);
            tmp.m_time = time;

            KillCrabActRoomStatItem item = new KillCrabActRoomStatItem();

            if (data.ContainsKey("roomId"))
            {
                item.m_roomId = Convert.ToInt32(data["roomId"]);
                if (data.ContainsKey("income"))
                    item.m_goldIncome = Convert.ToInt64(data["income"]);

                if (data.ContainsKey("killcount"))
                    item.m_killCount = Convert.ToInt32(data["killcount"]);

                if (data.ContainsKey("dropCount"))
                    item.m_dropCount = Convert.ToInt32(data["dropCount"]);

                tmp.m_data.Add(item.m_roomId, item);
            }
        }
        return OpRes.opres_success;
    }
}

//追击蟹将任务统计
public class KillActTaskItem
{
    public string m_time;
    public long m_outlay;
    public Dictionary<int, int> m_killCount = new Dictionary<int, int>();
}
public class QueryTypeStatKillCrabActTask : QueryBase
{
    private List<KillActTaskItem> m_result = new List<KillActTaskItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_KILL_CRAB_PLAYER_TASK, dip, imq, 0, 0, null, "genTime", true);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            KillActTaskItem tmp = new KillActTaskItem();
            m_result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("outlay"))
                tmp.m_outlay = Convert.ToInt64(data["outlay"]);

            for (k = 1; k < 10; k++) 
            {
                string str = "killId_" + k;
                int killCount = 0;
                if (data.ContainsKey(str))
                    killCount = Convert.ToInt32(data[str]);

                tmp.m_killCount.Add(k, killCount);
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//炮倍任务
public class StatPlayerOpenRateTaskItem 
{
    public string m_time;
    public Dictionary<int, int> m_openRateCount = new Dictionary<int, int>();
    public long m_outlay;
}
public class QueryTypeStatPlayerOpenRateTask : QueryBase 
{
    public int[] openRateLevel = new int[] { 3, 4, 6, 11, 14, 16, 18, 20, 31, 32};
    private List<StatPlayerOpenRateTaskItem> m_result = new List<StatPlayerOpenRateTaskItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_TOTAL_PLAYER_TURRET_LEVEL, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatPlayerOpenRateTaskItem tmp = new StatPlayerOpenRateTaskItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("outlay"))
                tmp.m_outlay = Convert.ToInt64(data["outlay"]);

            k = 0;
            foreach (var openRate in openRateLevel)
            {
                string rate = openRate.ToString();

                int count = 0;
                if (data.ContainsKey(rate))
                    count = Convert.ToInt32(data[rate]);

                tmp.m_openRateCount.Add(k, count);
                k++;
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//玩家携带金币
public class statGoldOnPlayerItem 
{
    public string m_time;
    public int m_type;
    public Dictionary<int, long> m_goldList = new Dictionary<int, long>();
}
public class QueryTypeStatGoldOnPlayer : QueryBase 
{
    private List<statGoldOnPlayerItem> m_result = new List<statGoldOnPlayerItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        IMongoQuery imq3 = null;
        if (p.m_type != 0)
            imq3 = Query.EQ("type", 2 - p.m_type); //新玩家1 活跃0
        else
            imq3 = Query.Or(Query.EQ("type", 0), Query.EQ("type", 1));

        imq = Query.And(imq, imq3);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("type");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_PUMP_TURRET_PROPERTY, dip, imq, 0, 0, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            statGoldOnPlayerItem tmp = new statGoldOnPlayerItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            if (data.ContainsKey("type"))
                tmp.m_type = Convert.ToInt32(data["type"]);

            for (k = 1; k <= 43; k++) 
            {
                string index = k.ToString();
                int count = 0;
                if (data.ContainsKey(index))
                    count = Convert.ToInt32(data[index]);

                long goldValue = 0;
                //金币
                string gold = "gold_" + k;
                if (data.ContainsKey(gold))
                    goldValue = Convert.ToInt64(data[gold]);
                //道具折算金币
                string itemGold = "itemValue_" + k;
                if (data.ContainsKey(itemGold))
                    goldValue += Convert.ToInt64(data[itemGold]);

                long avg = getAvgGold(count, goldValue);

                tmp.m_goldList.Add(k, avg);
            }
        }
        return OpRes.opres_success;
    }

    public long getAvgGold(int count, long gold) 
    {
        if (count == 0)
           return gold;

        return Convert.ToInt64(Math.Floor(gold*1.0/count));
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//成长基金统计
public class StatPumpGrowFundItem
{
    public string m_time;

    public int m_activationPerson;
    public int m_openWithActivationCount;
    public int m_openWithActivationPerson;

    public int m_openWithoutActivationCount;
    public int m_openWithoutActivationPerson;

    public Dictionary<int, int> m_receive = new Dictionary<int, int>();
}
public class QueryTypeStatPumpGrowFund : QueryBase 
{
    private List<StatPumpGrowFundItem> m_result = new List<StatPumpGrowFundItem>();

    public StatPumpGrowFundItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatPumpGrowFundItem item = new StatPumpGrowFundItem();
        m_result.Add(item);
        item.m_time = time;

        for (int i = 2; i <= 15; i++) 
        {
            item.m_receive.Add(i, 0);
        }

        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_GROW_FUND, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            StatPumpGrowFundItem tmp = IsCreate(time);

            //激活人数
            int activation = 0;
            if (data.ContainsKey("activation"))
                activation = Convert.ToInt32(data["activation"]);
            if (activation > 0)
                tmp.m_activationPerson += 1;

            //打开人数 次数
            int openWithActivation = 0;
            if (data.ContainsKey("openWithActivation"))
                openWithActivation = Convert.ToInt32(data["openWithActivation"]);
            if (openWithActivation > 0) 
            {
                tmp.m_openWithActivationPerson += 1;
                tmp.m_openWithActivationCount += openWithActivation;
            }

            //未激活打开人数次数
            int openWithoutActivation = 0;
            if (data.ContainsKey("openWithoutActivation"))
                openWithoutActivation = Convert.ToInt32(data["openWithoutActivation"]);
            if (openWithoutActivation > 0)
            {
                tmp.m_openWithoutActivationPerson += 1;
                tmp.m_openWithoutActivationCount += openWithoutActivation;
            }

            //2-15等级人数
            if (data.ContainsKey("playerLv") && data.ContainsKey("receive"))
            {
                int playerLv = Convert.ToInt32(data["playerLv"]);
                if (playerLv > 15 || playerLv < 2)
                    continue;

                int receive = Convert.ToInt32(data["receive"]);
                if (tmp.m_receive.ContainsKey(playerLv) && receive > 0)
                    tmp.m_receive[playerLv] += 1;
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//VIP特权打点
public class StatVipGiftItem 
{
    public string m_time;
    public int m_openPerson;
    public int m_openCount;
}
public class QueryTypeStatVipGift : QueryBase 
{
    private List<StatVipGiftItem> m_result = new List<StatVipGiftItem>();

    public StatVipGiftItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatVipGiftItem item = new StatVipGiftItem();
        m_result.Add(item);
        item.m_time = time;

        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(imq, Query.EQ("giftType", 22));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLOR_ROOM_FU_DAI, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            StatVipGiftItem tmp = IsCreate(time);

            //打开人数 次数
            int count = 0;
            if (data.ContainsKey("action_1"))
                count = Convert.ToInt32(data["action_1"]);

            if (count > 0) 
            {
                tmp.m_openCount += count;
                tmp.m_openPerson += 1;
            }
        }
        return OpRes.opres_success;
    }
}
//VIP特权统计
public class StatVipRecordItem : StatVipGiftItem
{
    public int m_missCount;
    public Dictionary<int, int> m_vipLevel = new Dictionary<int, int>();
}
public class QueryTypeStatVipRecord : QueryBase 
{
    private List<StatVipRecordItem> m_result = new List<StatVipRecordItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_VIP_RECORD, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, j = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            StatVipRecordItem tmp = new StatVipRecordItem();
            m_result.Add(tmp);

            DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            tmp.m_time = time.ToShortDateString();
            
            if (data.ContainsKey("missCount"))
                tmp.m_missCount = Convert.ToInt32(data["missCount"]);

            for (j = 0; j < 10; j++) 
            {
                string str = "vipLevel_" + j;
                int count = 0;
                if (data.ContainsKey(str))
                    count = Convert.ToInt32(data[str]);

                tmp.m_vipLevel.Add(j, count);
            }

            //打开次数 打开人数
            IMongoQuery imq1 = Query.And(Query.EQ("genTime", time.Date), Query.EQ("giftType", 22), Query.GT("action_1", 0));
            List<Dictionary<string, object>> data_list =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLOR_ROOM_FU_DAI, dip, imq1, 0, 0, new string[]{"playerId","action_1"}, "playerId", false);
            if (data_list != null && data_list.Count != 0) 
            {
                for (j = 0; j < data_list.Count; j++) 
                {
                    int count = 0;
                    if (data_list[j].ContainsKey("action_1"))
                        count = Convert.ToInt32(data_list[j]["action_1"]);

                    if (count > 0) 
                    {
                        tmp.m_openCount += count;
                        tmp.m_openPerson += 1;
                    }
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//月卡购买统计
public class StatMonthCardItem : StatVipGiftItem 
{
    public int m_rechargePerson;
    public int m_possessPerson;
    public int m_flag = 0;
}
public class QueryTypeStatMonthCard : QueryBase 
{
    private List<StatMonthCardItem> m_result = new List<StatMonthCardItem>();

    public StatMonthCardItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatMonthCardItem item = new StatMonthCardItem();
        m_result.Add(item);
        item.m_time = time;

        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        imq = Query.And(imq, Query.EQ("giftType", 4));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLOR_ROOM_FU_DAI, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            DateTime t = Convert.ToDateTime(data["genTime"]).ToLocalTime();
            string time = t.ToShortDateString();
            StatMonthCardItem tmp = IsCreate(time);

            //打开人数 次数
            int count = 0;
            if (data.ContainsKey("action_1"))
                count = Convert.ToInt32(data["action_1"]);

            if (count > 0)
            {
                tmp.m_openCount += count;
                tmp.m_openPerson += 1;
            }

            int rechargeCount = 0;
            if (data.ContainsKey("action_3"))
                rechargeCount = Convert.ToInt32(data["action_3"]);
            if (rechargeCount > 0)
                tmp.m_rechargePerson += 1;

            //拥有月卡人数
            if (tmp.m_flag > 0)
                continue;

            IMongoQuery imq1 = Query.GTE("VipCardEndTime", t.Date);
            tmp.m_possessPerson = DBMgr.getInstance().executeDistinct(TableName.PLAYER_INFO, dip, "player_id", imq1);
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//激励视频统计
public class StatPlayAdItem 
{
    public string m_time;
    public int m_playCount;
    public int m_playPerson;

    public long getTotalOutlay() 
    {
        return m_playCount * 10100;
    }
}
public class QueryTypeStatPlayAd : QueryBase 
{
    private List<StatPlayAdItem> m_result = new List<StatPlayAdItem>();

    public StatPlayAdItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatPlayAdItem item = new StatPlayAdItem();
        m_result.Add(item);
        item.m_time = time;
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("date", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_PUMP_PLAY_AD, dip, imq, 0, 0, null, "date", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["date"]).ToLocalTime().ToShortDateString();
            StatPlayAdItem tmp = IsCreate(time);

            int count = 0;
            if (data.ContainsKey("playCount"))
                count = Convert.ToInt32(data["playCount"]);
            if (count > 0)
            {
                tmp.m_playCount += count;  //视频次数
                tmp.m_playPerson++; //视频人数
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//玩家平均携带物品
public class StatTurretItemsOnPlayerItem 
{
    public string m_time;
    public int m_turretId;
    public List<int> m_items = new List<int>();

    public string getOpenRate() 
    {
        string openRate = m_turretId.ToString();

        var openRateItem = Fish_TurretLevelCFG.getInstance().getValue(m_turretId);
        if (openRateItem != null)
            openRate = openRateItem.m_openRate.ToString();

        return openRate;
    }
}
public class QueryTypeStatTurretItemsOnPlayer : QueryBase 
{
    private List<StatTurretItemsOnPlayerItem> m_result = new List<StatTurretItemsOnPlayerItem>();
    private int[] m_items = new int[] {5,8,17,9,1,2,3,4,16,72, 18, 19, 20, 21, 52, 23, 24, 25, 26, 27};

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        //炮倍 为空则为全部炮倍
        if (!string.IsNullOrEmpty(p.m_param))
        {
            IMongoQuery imq3 = null;

            List<int> range = new List<int>();
            int turretId = 0;
            if (int.TryParse(p.m_param.Trim(), out turretId))
            {
                range.Add(turretId);
                range.Add(turretId);
            }
            else
            {
                Tool.parseNumList(p.m_param, range);
            }

            if (range[0] > 10000 || range[1] > 10000 || range[0] < 1 || range[1] < 1 || range[0] > range[1])
                return OpRes.op_res_param_not_valid;

            var da1 = Fish_TurretOpenRateCFG.getInstance().getValue(range[0]);
            var da2 = Fish_TurretOpenRateCFG.getInstance().getValue(range[1]);

            if (da1 == null || da2 == null)
                return OpRes.op_res_param_not_valid;

            range[0] = da1.m_level;
            range[1] = da2.m_level;

            imq3 = Query.And(Query.GTE("turretLevel", range[0]), Query.LTE("turretLevel", range[1]));

            imq = Query.And(imq, imq3);
        }

        //玩家类型 0活跃 1新增
        imq = Query.And(imq, Query.EQ("type", p.m_op));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_TURRET_ITEMS, imq, dip);

        SortByBuilder sort = new SortByBuilder();
        sort = sort.Descending("genTime").Ascending("turretLevel");

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery2(TableName.STAT_TURRET_ITEMS, dip, imq, 
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, sort);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            StatTurretItemsOnPlayerItem tmp = new StatTurretItemsOnPlayerItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            //0活跃 1新增
            if (data.ContainsKey("turretLevel"))
                tmp.m_turretId = Convert.ToInt32(data["turretLevel"]);

            foreach (var index in m_items) 
            {
                string str = "item" + index;
                switch(index)
                {
                    case 1: str = "useLockCount";break;
                    case 2: str = "useFrozeCount"; break;
                    case 3: str = "useViolentCount"; break;
                    case 4: str = "useCallCount"; break;
                }

                int count = 0;
                if (data.ContainsKey(str))
                    count = Convert.ToInt32(data[str]);

                tmp.m_items.Add(count);
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//返利礼包
public class StatRebateGiftItem 
{
    public string m_time;
    public int m_type;
    public int m_openCount;
    public int m_openPerson;
    public int m_rechargePerson;
}
public class QueryTypeStatRebateGift : QueryBase 
{
    private List<StatRebateGiftItem> m_result = new List<StatRebateGiftItem>();

    public StatRebateGiftItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatRebateGiftItem item = new StatRebateGiftItem();
        m_result.Add(item);
        item.m_time = time;
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        IMongoQuery imq3 = Query.EQ("giftType", p.m_type + 15);

        imq = Query.And(imq, imq3);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_FISHLOR_ROOM_FU_DAI, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        int i = 0, k = 0;
        for (i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            StatRebateGiftItem tmp = IsCreate(time);

            int count = 0;
            if (data.ContainsKey("action_1"))
                count = Convert.ToInt32(data["action_1"]);
            if (count > 0)
            {
                tmp.m_openCount += count;  //打开次数
                tmp.m_openPerson++; //打开人数
            }

            //购买人数
            int recharge = 0;
            if (data.ContainsKey("action_3"))
                recharge = Convert.ToInt32(data["action_3"]);

            if (recharge > 0)
                tmp.m_rechargePerson++;
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//鱼雷碎片统计
public class StatTurretChipItem 
{
    public string m_time;
    public int m_chipOutput43;
    public int m_chipOutput44;
    public int m_chipOutput45;
    public int m_chipOutput46;

    public int m_chipCompose43;
    public int m_chipCompose44;
    public int m_chipCompose45;
    public int m_chipCompose46;

    public int m_playerId;
}
public class QueryTypeStatTurretChip : QueryBase 
{
    private List<StatTurretChipItem> m_result = new List<StatTurretChipItem>();

    public StatTurretChipItem IsCreate(string time)
    {
        foreach (var d in m_result)
        {
            if (d.m_time == time)
                return d;
        }

        StatTurretChipItem item = new StatTurretChipItem();
        item.m_time = time;
        m_result.Add(item);
        return item;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;

        int playerId = 0;
        if(!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq3 = Query.EQ("playerId", playerId);

        IMongoQuery imq4 = Query.Or(
            Query.And(Query.GTE("itemId", 43), Query.LTE("itemId", 46)),
            Query.And(Query.GTE("itemId", 24), Query.LTE("itemId", 27)));

        imq = Query.And(imq4, imq, Query.GT("addCount", 0));
        imq = Query.And(imq, imq3);

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PUMP, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_ITEM, dip, imq, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            string time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            StatTurretChipItem tmp = IsCreate(time);

            tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (!data.ContainsKey("reason"))
                continue;
            int reason = Convert.ToInt32(data["reason"]);

            if (!data.ContainsKey("itemId"))
                continue;
            int itemId = Convert.ToInt32(data["itemId"]);

            int addCount = 0;
            if (data.ContainsKey("addCount"))
                addCount = Convert.ToInt32(data["addCount"]);

            //产出
            switch(itemId)
            {
                case 43: tmp.m_chipOutput43 += addCount;
                        break;
                case 44: tmp.m_chipOutput44 += addCount;
                        break;
                case 45: tmp.m_chipOutput45 += addCount; 
                        break;
                case 46: tmp.m_chipOutput46 += addCount; 
                        break;
            }

            //合成 即鱼雷产出  100
            if (reason == 100)
            {
                switch (itemId)
                {
                    case 24: tmp.m_chipCompose43 += addCount; 
                            break;
                    case 25: tmp.m_chipCompose44 += addCount; 
                            break;
                    case 26: tmp.m_chipCompose45 += addCount; 
                            break;
                    case 27: tmp.m_chipCompose46 += addCount; 
                            break;
                }
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//玩家邮件统计
public class StatPlayerMailItem 
{
    public string m_time;
    public int m_playerId;
    public string m_mailName;
    public long m_gold;
    public int m_isRecv;
}
public class QueryTypeStatPlayerMail : QueryBase 
{
    private List<StatPlayerMailItem> m_result = new List<StatPlayerMailItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2);

        if (string.IsNullOrEmpty(p.m_playerId))
            return OpRes.op_res_param_not_valid;
        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        imq = Query.And(imq, Query.EQ("playerId", playerId));

        return query(user, imq, p);
    }
    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }
    private OpRes query(GMUser user, IMongoQuery imq, ParamQuery param)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PLAYER_MAIL, dip, imq, 0, 0, null, "time", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            StatPlayerMailItem tmp = new StatPlayerMailItem();
            m_result.Add(tmp);

            tmp.m_time = Convert.ToDateTime(data["time"]).ToLocalTime().ToString();
            tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("title"))
                tmp.m_mailName = Convert.ToString(data["title"]);

            if (data.ContainsKey("isReceive"))
                tmp.m_isRecv = Convert.ToInt32(data["isReceive"]);

            if (data.ContainsKey("gifts"))
            {
                List<mailGiftItem> rewardList = new List<mailGiftItem>();

                Dictionary<string, object> gList = (Dictionary<string, object>)data["gifts"];
                Tool.parseItemFromDicMail(gList, rewardList);

                foreach (var reward in rewardList) 
                {
                    if (reward.m_giftId == 1)  //金币
                        tmp.m_gold = reward.m_count;
                }
            }
        }
        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//玩家手机验证查询
public class PlayerPhoneItem
{
    public string m_id;
    public string m_time;
    public int m_playerId;
    public string m_phoneNo;
    public int m_code;
}
public class QueryTypeStatPlayerPhoneVertify : QueryBase
{
    private List<PlayerPhoneItem> m_result = new List<PlayerPhoneItem>();
    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        return query(user);
    }

    //返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.PLAYER_PHONE_CODE, null, dip);
        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.PLAYER_PHONE_CODE, dip, null, 0, 0, null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            PlayerPhoneItem tmp = new PlayerPhoneItem();
            m_result.Add(tmp);

            tmp.m_id = Convert.ToString(data["_id"]);
            tmp.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToString();

            if (data.ContainsKey("playerId"))
                tmp.m_playerId = Convert.ToInt32(data["playerId"]);

            if (data.ContainsKey("phoneNo"))
                tmp.m_phoneNo = Convert.ToString(data["phoneNo"]);

            if (data.ContainsKey("code"))
                tmp.m_code = Convert.ToInt32(data["code"]);
        }
        return OpRes.opres_success;
    }
}