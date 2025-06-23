using System.Text.Json.Serialization;

namespace NMS_API_FE.Helpers
{
    public class ODataResponse<T>
    {
        public IEnumerable<T> Value { get; set; }
    }

    public class JsonArrayWrapper<T>
    {
        [JsonPropertyName("$values")]
        public List<T> Values { get; set; }
    }
}
