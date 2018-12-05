using System;
using System.ServiceProcess;
using WindowsServiceInstaller.InstallHelper;

namespace WindowsServiceInstaller
{
    public static class SelfInstaller
    {
        /// <summary>
        /// 安装命令
        /// </summary>
        private const string INSTALL_CMD = "install";

        /// <summary>
        /// 卸载名称
        /// </summary>
        private const string UNSTALL_CMD = "uninstall";

        /// <summary>
        /// 停止
        /// </summary>
        private const string STOP_CMD = "stop";

        /// <summary>
        /// 开始
        /// </summary>
        private const string START_CMD = "start";

        /// <summary>
        /// 服务启动控制台(可以指定更多信息)
        /// </summary>
        public static void RunServiceCmdWithMoreInfo()
        {
            while (true)
            {
                string guideMsg = $"请选择你要执行的操作——{1}：安装服务,{2}：卸载服务";
                string startMode =$"请选择你要执行的方式——{(int)ServiceStartMode.Automatic}:自动,{(int)ServiceStartMode.Manual}:手动,{(int)ServiceStartMode.Disabled}:禁用";


                Console.WriteLine("输入服务名称");
                string serviceName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(serviceName))
                {
                    Console.WriteLine("服务名称不能为空");
                    continue;
                }

                Console.WriteLine("输入操作指令参考如下：");
                Console.WriteLine(guideMsg);
                string cmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmd))
                {
                    Console.WriteLine(guideMsg);
                    continue;
                }

                if (cmd.Equals("1"))
                {
                    Console.WriteLine("输入服务exe文件所在路径");
                    var servicePath = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(servicePath))
                    {
                        Console.WriteLine("服务exe文件路径不能为空");
                        continue;
                    }

                    Console.WriteLine("输入启动模式参考如下：（默认为自动）:");
                    Console.WriteLine(startMode);
                    string modeStr = Console.ReadLine();
                    var mode = ServiceStartMode.Automatic;
                    try
                    {
                        mode = (ServiceStartMode)Convert.ToInt32(modeStr);
                    }
                    catch
                    {
                        Console.WriteLine("输入模式不存在，直接使用默认自动");
                    }


                    Console.WriteLine("输入服务描述（默认和服务名称相同）");
                    string description = Console.ReadLine();

                    Console.WriteLine("输入服务显示名称（默认和服务名称相同）");
                    string displayName = Console.ReadLine();

                    if (ServiceHelper.IsServiceExisted(serviceName))
                    {
                        Console.WriteLine("\n服务已存在......");
                        continue;
                    }

                    //执行安装
                    try
                    {
                        ServiceHelper.ConfigService(serviceName, true, mode, servicePath, displayName, description);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException is System.Security.SecurityException)
                        {
                            Console.WriteLine("请使用管理员权限启动！");
                        }
                        else
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        continue;
                    }

                }

                if (cmd.Equals("2"))
                {
                    if (ServiceHelper.IsServiceExisted(serviceName))
                    {
                        ServiceHelper.ConfigService(serviceName, false);
                        break;
                    }

                    Console.WriteLine("\n服务不存在......");
                }
            }
            Console.WriteLine("\n按任意键退出安装界面......");
            Console.ReadLine();
        }


        /// <summary>
        /// 部署Windows服务时, 在复制文件前后进行启停安装操作
        /// 
        /// 程序文件复制前, 停止(如果已经安装)指定服务
        /// XxxService /mode=stop /serviceName=MyService_dev
        /// 
        /// 程序文件复制后, 安装(如果没有安装过)并启动指定服务
        /// XxxService /mode=startOrInstall /serviceName=MyService_dev  /path=D:/www1/xxx.exe /displayName="xxx服务" /description="主要xx功能"
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool ConfigService(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return false;
            }

            var argsParser = new ArgsParser(args);
            var mode = argsParser.GetParamValueOrThrow("/mode");
            var serviceName = argsParser.GetParamValueOrThrow("/serviceName");


            if (STOP_CMD.Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                if (ServiceHelper.IsServiceExisted(serviceName))
                {
                    ServiceHelper.StopService(serviceName);
                }
            }
            else if (INSTALL_CMD.Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                var assemblypath = argsParser.GetParamValueOrThrow("/path");
                var displayName = argsParser.GetParamValueOrNull("/displayName") ?? string.Empty;
                var description = argsParser.GetParamValueOrNull("/description") ?? string.Empty;

                if (!ServiceHelper.IsServiceExisted(serviceName))
                {
                    const bool install = true;
                    ServiceHelper.ConfigService(serviceName,
                        install,
                        ServiceStartMode.Automatic,
                        assemblypath,
                        displayName,
                        description);
                }

                ServiceHelper.StartService(serviceName);
            }
            else if (START_CMD.Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                if (ServiceHelper.IsServiceExisted(serviceName))
                {
                    ServiceHelper.StartService(serviceName);
                }
            }
            else if (UNSTALL_CMD.Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                if (ServiceHelper.IsServiceExisted(serviceName))
                {
                    const bool install = false;
                    ServiceHelper.ConfigService(serviceName,
                        install,
                        ServiceStartMode.Automatic);
                }
            }
            return true;
        }


    }
}
