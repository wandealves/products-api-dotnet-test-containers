using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Tests;

public static class TestsExtensions
{
  public static void AtribuirJsonMediaType(this HttpClient client)
  {
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
  }

  public static StringContent ToStringContent(this object obj)
  {
    var jsonContent = JsonConvert.SerializeObject(obj);
    var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
    contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    return contentString;
  }
}
