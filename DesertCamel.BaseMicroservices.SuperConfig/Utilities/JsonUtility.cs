using Newtonsoft.Json;

namespace DesertCamel.BaseMicroservices.SuperConfig.Utilities
{
    public static class JsonUtility
    {
        public static string ToJson(this object self)
        {
            return JsonConvert.SerializeObject(self);
        }
    }
}
