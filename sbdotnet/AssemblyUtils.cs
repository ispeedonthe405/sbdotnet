using System.Reflection;

namespace sbdotnet
{
    public class AssemblyUtils
    {
        public static List<string> GetResourceNames(string prefix, string suffix)
        {
            List<string> names = [];

            try
            {
                names = Assembly.GetCallingAssembly().GetManifestResourceNames().Where(r =>
                    r.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase) &&
                    r.EndsWith(suffix, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return names;
        }
    }
}
