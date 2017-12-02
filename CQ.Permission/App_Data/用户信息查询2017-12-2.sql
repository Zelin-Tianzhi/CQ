select 
a.AccountID,a.AccountNum,a.Account,a.NickName,a.Sex,a.AccountType,a.AccountSecondType,a.Gold,a.GoldBank,a.TotalExp
,b.RegisterAddress,b.RegisterDate,c.LastLoginIP,c.LastLoginTime,c.OnlineTime 
from Account a
left join AccountRegInfo b on a.AccountID = b.AccountID
left join AccountLastLogin c on c.AccountID=a.AccountID