using System.Text.Json;
namespace LibraryTracer.serialization
{
    public class JsonResult:ISerializer
    {

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }

    }
}
