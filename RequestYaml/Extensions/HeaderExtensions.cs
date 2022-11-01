using System.Net.Http.Headers;

namespace RequestYaml.Extensions
{
    public static class HeaderExtensions
    {
        public static void SetHeaders(this HttpRequestHeaders httpRequestHeaders, Header headers)
        {
            httpRequestHeaders.Host = headers?.Host;
            httpRequestHeaders.Referrer = new Uri(headers?.Referrer);
        }
    }
}