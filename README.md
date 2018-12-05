# WindowsServiceInstaller
完成Windows服务的安装，可以指定服务的名称和描述，方便同一台机器部署多个相同的服务

**1.打开sln，编译程序（默认会在debug下生成WindowsServiceInstaller.exe文件）**
**2.使用方法有两种如下：**
**①直接以“管理员”方式启动程序**
按照提示step by step进行服务的安装
安装服务
![](http://doc.uc108.org:8002/Public/Uploads/2018-12-05/5c078026e6d0e.png)
安装完成如下：
![](http://doc.uc108.org:8002/Public/Uploads/2018-12-05/5c0780ec58b61.png)

卸载服务
![](http://doc.uc108.org:8002/Public/Uploads/2018-12-05/5c0780b808dfa.png)

**②打开cmd，输入如下命令执行安装：**
```
安装服务：
WindowsServiceInstaller.exe  /mode=Install  /serviceName=填写服务名称  /path=填写服务路径 /displayName="服务展示的名称" /description="服务描述"
```
![](http://doc.uc108.org:8002/Public/Uploads/2018-12-05/5c077f9e2cab2.png)

```
卸载服务：
WindowsServiceInstaller.exe  /mode=Install  /serviceName=填写服务名称 
```
![](http://doc.uc108.org:8002/Public/Uploads/2018-12-05/5c077fbc18228.png)
