using System.Text.Json;

namespace sbdotnet
{
    public class RestChannel
    {
        ///////////////////////////////////////////////////////////
        #region Fields

        private HttpClient _Http;
        private Uri _BaseUri;

        #endregion Fields
        ///////////////////////////////////////////////////////////



        ///////////////////////////////////////////////////////////
        #region Properties

        public HttpClient Http { get => _Http; }
        public Uri BaseUri { get => _BaseUri; }

        #endregion Properties
        ///////////////////////////////////////////////////////////



        ///////////////////////////////////////////////////////////
        #region Interface

        /// <summary>
        /// Specify base Uri as a string. The lib will take care of turning it into a Uri.
        /// </summary>
        /// <param name="baseUri"></param>
        public RestChannel(string baseUri)
        {
            _BaseUri = new(baseUri);
            _Http = new()
            {
                BaseAddress = _BaseUri
            };
        }

        /// <summary>
        /// Just pass in the string; the lib will take care of turning it into a Uri.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>The response string</returns>
        public async Task<string> GetAsync(string endpoint)
        {
            try
            {
                Uri endpointUri = new(endpoint, UriKind.RelativeOrAbsolute);
                using HttpResponseMessage response = await Http.GetAsync(endpointUri);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return jsonResponse;
                }
                else
                {
                    return $"ERROR: Response code = {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return string.Empty;
        }

        /// <summary>
        /// Just pass in the string; the lib will take care of turning it into a Uri.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns>An object of type T deserialized from the response string</returns>
        public async Task<T?> GetAsync<T>(string endpoint) where T : class, new()
        {
            try
            {
                Uri endpointUri = new(endpoint, UriKind.RelativeOrAbsolute);
                using HttpResponseMessage response = await Http.GetAsync(endpointUri);
                var stringResponse = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        return JsonSerializer.Deserialize<T>(stringResponse);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        #endregion Interface
        ///////////////////////////////////////////////////////////
    }
}
