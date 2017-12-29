using System.ComponentModel;

namespace CQ.Application
{
    public enum ArticleType
    {
        [Description("新闻")]
        News = 1,
        [Description("公告")]
        Notice = 2
    }

    public enum PropType
    {
        /// <summary>
        /// 道具购买
        /// </summary>
        [Description("道具购买")]
        PropBuy=0,
        /// <summary>
        /// 道具赠送
        /// </summary>
        [Description("道具赠送")]
        PropPresent = 1,
        /// <summary>
        /// 道具回收
        /// </summary>
        [Description("道具回收")]
        PropRecycle = 2,
        /// <summary>
        /// 道具使用
        /// </summary>
        [Description("道具使用")]
        PropUse = 3,
        /// <summary>
        /// 道具接收
        /// </summary>
        [Description("道具接收")]
        PropReceive = 4,
        /// <summary>
        /// 道具过期
        /// </summary>
        [Description("道具过期")]
        PropOverdue = 5,
        /// <summary>
        /// 赠送道具
        /// </summary>
        [Description("奖励道具")]
        PropBonus = 6,
        /// <summary>
        /// 道具交易增加
        /// </summary>
        [Description("道具交易增加")]
        PropExchangeAdd = 7,
        /// <summary>
        /// 道具交易删除
        /// </summary>
        [Description("道具交易删除")]
        PropExchangeDel = 8,
        /// <summary>
        /// 竞技场奖励
        /// </summary>
        [Description("竞技场奖励")]
        PropJjc = 9,
        /// <summary>
        /// 道具合成
        /// </summary>
        [Description("道具合成")]
        PropSynthesis = 10,
        /// <summary>
        /// 大卖场增加拍卖
        /// </summary>
        [Description("大卖场增加拍卖")]
        PropDmcAdd = 11,
        /// <summary>
        /// 大卖场撤销拍卖
        /// </summary>
        [Description("大卖场撤销拍卖")]
        PropDmcDel = 12,
        /// <summary>
        /// 大卖场交易获得
        /// </summary>
        [Description("大卖场交易获得")]
        PropDmcExchangeAdd = 13
    }
}