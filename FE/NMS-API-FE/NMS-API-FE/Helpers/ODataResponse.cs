namespace NMS_API_FE.Helpers
{
    public class ODataResponse<T>
    {
        public IEnumerable<T> Value { get; set; }
    }
}
