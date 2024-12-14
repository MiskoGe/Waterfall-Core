namespace Waterfall.System.Security.Runtime
{
    public static class GlobalConfig
    {
        public static bool VFSInitialized { get; set; } = false;

        public static string RootPath = @"0:\";
        public static bool CLIBoot = false;
    }
}
