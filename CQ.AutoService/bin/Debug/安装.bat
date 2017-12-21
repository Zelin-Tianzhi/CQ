@echo AutoService 服务开始安装......
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %cd%\CQ.AutoService.exe
net start TxGameService
pause