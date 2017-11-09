use CQBase
go
alter table Bus_Products add F_Remark nvarchar(500)
go
alter table Bus_Images add F_CreatorUserId nvarchar(128)
go
select * from Sys_User where  F_Account='admin'
go
delete Sys_User where F_Account<> 'admin' and  F_Account<>'system'
go
select * from Sys_Role