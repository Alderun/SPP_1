using System.IO;
using System.Xml.Serialization;

namespace LibraryTracer.serialization
{
    public class XmlResult:ISerializer
    {
        public string Serialize(object obj)
        {
            XmlSerializer xmlSerializer = new(obj.GetType());

            StringWriter textWriter = new();
            xmlSerializer.Serialize(textWriter, obj);
            return textWriter.ToString();
        }
    }
}
