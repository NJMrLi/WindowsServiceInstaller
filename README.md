# WindowsServiceInstaller
完成Windows服务的安装，可以指定服务的名称和描述，方便同一台机器部署多个相同的服务

**1.打开sln，编译程序（默认会在debug下生成WindowsServiceInstaller.exe文件）**
**2.使用方法有两种如下：**
**①直接以“管理员”方式启动程序**
```
按照提示step by step进行服务的安装
```
**②打开cmd，输入如下命令执行安装：**
```
安装服务：
WindowsServiceInstaller.exe  /mode=Install  /serviceName=填写服务名称  /path=填写服务路径 /displayName="服务展示的名称" /description="服务描述"
```
```
卸载服务：
WindowsServiceInstaller.exe  /mode=uninstall  /serviceName=填写服务名称 
```
