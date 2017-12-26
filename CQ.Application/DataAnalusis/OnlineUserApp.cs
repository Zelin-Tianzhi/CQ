using System;
using System.Collections.Generic;
using CQ.Core;

namespace CQ.Application.DataAnalusis
{
    public class OnlineUserApp : BaseApp
    {
        #region 属性



        #endregion

        #region 公共方法

        public List<object> GetOnlineUserCount()
        {
            string url = GetUrlStr() + $"ysfunction=getusercount";
            string Mess = HttpMethods.HttpGet(url);
            string[] counts = Mess.Split(',');
            List<object> list = new List<object>();
            if (counts.Length > 0)
            {
                list.Add(new
                {
                    F_Id = 1,
                    CurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ActiveUser = counts[0],
                    InsideUser = counts[3],
                    OrdinaryUser = counts[2],
                    TotalUser = int.Parse(counts[0]) + int.Parse(counts[1]) + int.Parse(counts[2]) + int.Parse(counts[3])
                });
            }

            return list;
        }


        #endregion

        #region 私有方法

        

        #endregion
    }
}