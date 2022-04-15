using System.Net;

namespace UtilityForRevenue.Helper
{
    public static class WebRequestHandler
    {
        static string ConfigurationReloadTokenP7 = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjczQ0Q4NERGRUJGQzk4NUU4RUZGOTU0QjY2NTg0OEFBMTYzNDExNkIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJjODJFMy12OG1GNk9fNVZMWmxoSXFoWTBFV3MifQ.eyJuYmYiOjE2NDg2NDEyMzUsImV4cCI6MTY0ODY0ODQzNSwiaXNzIjoiaHR0cHM6Ly9lbmdhZ2lmaWktcHJldmlldzctaWRlbnRpdHkuYXp1cmV3ZWJzaXRlcy5uZXQiLCJhdWQiOlsiaHR0cHM6Ly9lbmdhZ2lmaWktcHJldmlldzctaWRlbnRpdHkuYXp1cmV3ZWJzaXRlcy5uZXQvcmVzb3VyY2VzIiwiVXNlcnNBUEkiLCJBY2NyZWRpdGF0aW9uQVBJIiwiQmlsbHRyYWNraW5nQXBpIiwiQ29tbWVudEFwaSIsIk5vdGVzQXBpIl0sImNsaWVudF9pZCI6Im5nLkVuZ2FnaWZpaVVJIiwic3ViIjoiNzE5ZTgwOTgtOTg0YS00OTBmLThiNWEtM2M5MTk0ZDk2NzhmIiwiYXV0aF90aW1lIjoxNjQ4NjE5NjQyLCJpZHAiOiJsb2NhbCIsInNzLXBpZCI6IiIsInBpY3R1cmUiOiIiLCJwaWN0dXJlLXNtYWxsIjoiIiwicGljdHVyZS1pY29uIjoiIiwiZ2l2ZW5fbmFtZSI6IkNyZXNjZXJhbmNlIiwiZmFtaWx5X25hbWUiOiJBZG1pbiIsImVtYWlsIjoiYWRtaW5AY3Jlc2NlcmFuY2UuY29tIiwibGFzdC1sb2dpbiI6IjMvMzAvMjAyMiA4OjI4OjAyIEFNIiwiY3VycmVudC1sb2dpbiI6IjMvMzAvMjAyMiA4OjM2OjQ4IEFNIiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsImVtYWlsIiwiVXNlcnNBUEkiLCJBY2NyZWRpdGF0aW9uQVBJIiwiQmlsbHRyYWNraW5nQXBpIiwiQ29tbWVudEFwaSIsIk5vdGVzQXBpIl0sImFtciI6WyJwd2QiXX0.FuCrqaz0fD2qoEzxuDYQm7AugWpqY1jlI6-44b0IuA1ehFz5iznC8Wo3H676lzaeQE14kChtL5SsKIHOlbqkF0vzHpfVUZ-hlH5JNtUj-2dPTwBggq6MERtAO9fhjjCo1hUfA4dLxLyr7wcXpvg7WGCVPxOvjiBlpJBCST-MxZcUELe0hPhOsMGX1nrcPeok-PRq-hFWAOpB-RbcMP7cl6p3Ufyj6dz7UInrtGbMw3o1cAyZ1o9UIk5OZXd66QpOqPGQLbgjHdWtLV7-hwjUdKWz_CtpBVzevEB03M9xlrq-1B76isupreqa9B_Hf_UdAyBzvUSr1qZy9_hS8jEAng";
        static string ConfigurationReloadTokenProd = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjczQ0Q4NERGRUJGQzk4NUU4RUZGOTU0QjY2NTg0OEFBMTYzNDExNkIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJjODJFMy12OG1GNk9fNVZMWmxoSXFoWTBFV3MifQ.eyJuYmYiOjE2NDk0MDQyNjksImV4cCI6MTY0OTQxMTQ2OSwiaXNzIjoiaHR0cHM6Ly9lbmdhZ2lmaWktaWRlbnRpdHktbGl2ZS5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL2VuZ2FnaWZpaS1pZGVudGl0eS1saXZlLmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsIlVzZXJzQVBJIiwiQWNjcmVkaXRhdGlvbkFQSSIsIkJpbGx0cmFja2luZ0FwaSIsIkNvbW1lbnRBcGkiLCJOb3Rlc0FwaSJdLCJjbGllbnRfaWQiOiJuZy5FbmdhZ2lmaWlVSSIsInN1YiI6IjcxOWU4MDk4LTk4NGEtNDkwZi04YjVhLTNjOTE5NGQ5Njc4ZiIsImF1dGhfdGltZSI6MTY0OTQwNDI2OCwiaWRwIjoibG9jYWwiLCJzcy1waWQiOiIiLCJwaWN0dXJlIjoiIiwicGljdHVyZS1zbWFsbCI6IiIsInBpY3R1cmUtaWNvbiI6IiIsImdpdmVuX25hbWUiOiJDcmVzY2VyYW5jZSIsImZhbWlseV9uYW1lIjoiQWRtaW4iLCJlbWFpbCI6ImFkbWluQGNyZXNjZXJhbmNlLmNvbSIsImxhc3QtbG9naW4iOiI0LzgvMjAyMiA3OjUwOjQ5IEFNIiwiY3VycmVudC1sb2dpbiI6IjQvOC8yMDIyIDc6NTE6MDggQU0iLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiZW1haWwiLCJVc2Vyc0FQSSIsIkFjY3JlZGl0YXRpb25BUEkiLCJCaWxsdHJhY2tpbmdBcGkiLCJDb21tZW50QXBpIiwiTm90ZXNBcGkiXSwiYW1yIjpbInB3ZCJdfQ.U9z-hLduELCYIx_Li2NHzatG8QaqxjoZR_gy2AQDmiy9hwgQ6yrfomtFtNqHDIqjVL4NPNcad22_xfPoW4UXcxi2MRts5fQ8GphCSpuu9qlYP33qN_vfVf5tIb2ZhK6ez9jdvKqM7TyM0RTKRzZNdHAf87gJ5FuBVgG6eyDvMWXOQZLMNGUYkXOD9uOBrdkJMYTo3X9y88j5xmCBiVhAwJlT_IrJ7gJwOGPrEIZ-PHcIkvTgMlfrC8fgsuBgM-vmHagEx5Yu7naaM9lsh1bYqnAoqGuqPKEEZ65BdtPW1h2ntWzbYIDnw80E4cCjPN72nMTEwPmSN06xyDYFmo8lHw";
        static string BaseAddressP7 = "https://engagifii-preview7-revenue.azurewebsites.net/api/1.0/";
        static string BaseAddressProd = "https://engagifii-prod-revenue.azurewebsites.net/api/1.0/";
        public static string Client(string data, string url, bool isForProd)
        {
            var ConfigurationReloadToken = isForProd ? ConfigurationReloadTokenProd : ConfigurationReloadTokenP7;
            using (WebClient webClient = new WebClient())
            {
                webClient.BaseAddress = isForProd ? BaseAddressProd : BaseAddressP7;
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + ConfigurationReloadToken);
                webClient.Headers.Add("tenant-code", "VSBA");
                var response = webClient.UploadString(url, data);
                return response;
            }
        }

    }
}
