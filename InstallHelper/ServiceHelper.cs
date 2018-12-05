using System;
using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WindowsServiceInstaller.InstallHelper
{
    public static class ServiceHelper
    {
        /// <summary>
        /// 服务是否存在
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName"></param>
        public static void StartService(string serviceName)
        {
            if (IsServiceExisted(serviceName))
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Running &&
                    service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 60));
                }
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName"></param>
        public static void StopService(string serviceName)
        {
            if (IsServiceExisted(serviceName))
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status == ServiceControllerStatus.Running ||
                    service.Status == ServiceControllerStatus.StartPending)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 60));
                }
            }
        }

        /// <summary>
        /// 获取服务状态
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            return service.Status;
        }

        public static void ConfigService(
            string serviceName,
            bool install,
            ServiceStartMode mode = ServiceStartMode.Automatic,
            string assemblypath = "",
            string displayName = "",
            string description = "")
        {
            TransactedInstaller ti = new TransactedInstaller();
            ti.Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            });
            ti.Installers.Add(new ServiceInstaller
            {
                DisplayName = string.IsNullOrEmpty(displayName) == true ? serviceName : displayName,
                ServiceName = serviceName,
                Description = string.IsNullOrEmpty(description) == true ? serviceName : description,
                StartType = mode //运行方式
            });
            ti.Context = new InstallContext();

            if (install)
            {
                ti.Context.Parameters["assemblypath"] = "\"" + assemblypath + "\" /service";
                ti.Install(new Hashtable());
            }
            else
            {
                StopService(serviceName);
                ti.Uninstall(null);
            }
        }
    }
}
