using System.Reflection;

namespace W500Core
{
    public static class Utils
    {
        private static DateTime GetBuildDate(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<BuildDateAttribute>();
            return attribute?.DateTime ?? default(DateTime);
        }

        public static string GetBuildName()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var dt = GetBuildDate(assembly);
            return assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title + "-" + dt.ToString("yyyyMMddHHmmss");
        }
    }
}
