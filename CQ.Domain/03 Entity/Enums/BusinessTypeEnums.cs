using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Domain.Enums
{
    public enum ArticleType
    {
        [Description("新闻")]
        News = 1,
        [Description("公告")]
        Notice = 2
    }
}
