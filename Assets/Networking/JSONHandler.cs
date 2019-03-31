using Newtonsoft.Json;

public class JSONHandler
{
  public static T parseJson<T>(string data)
  {
    T value = JsonConvert.DeserializeObject<T>(data);
    return value;
  }
  public static T parseJson<T>(JSONObject data)
  {
    return parseJson<T>(data.ToString());
  }
  public static JSONObject classToJSONObject<T>(T data) where T : class
  {
    string json = JsonConvert.SerializeObject(data);
    JSONObject send = new JSONObject(json);
    return send;
  }
}
