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
}