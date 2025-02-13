using System.Reflection;

namespace sbdotnet
{
    public class AssemblyUtils
    {
        public static string LoadResourceString(string path)
        {
            string result = string.Empty;

            try
            {
                using (var resource = Assembly.GetCallingAssembly().GetManifestResourceStream(path))
                {
                    if (resource is not null)
                    {
                        using (var stream = new StreamReader(resource))
                        {
                            result = stream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

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
