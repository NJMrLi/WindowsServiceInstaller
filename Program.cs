
using WindowsServiceInstaller.InstallHelper;

namespace WindowsServiceInstaller
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                SelfInstaller.RunServiceCmdWithMoreInfo();
            }

            SelfInstaller.ConfigService(args);
        }
    }
}
