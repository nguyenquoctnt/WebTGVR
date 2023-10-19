using System;
using System.IO;
using System.Text;
using System.Xml;

namespace ApiClient.Serialization
{

    /// <summary>
    /// Xml Serialization
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// Serialize an object to a string
        /// </summary>
        /// <param name="value">Object to serialize</param>
        /// <returns>XML string</returns>
        public static string XmlSerializeToString(object value)
        {
            if (value == null)
            {
                return null;
            }

            var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, value);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Serialize an object to a byte array
        /// </summary>
        /// <param name="value">Object to serialize</param>
        /// <returns>Byte array</returns>
        public static byte[] XmlSerializeToBytes(object value)
        {
            return Encoding.UTF8.GetBytes(XmlSerializeToString(value));
        }

        /// <summary>
        ///  Deserilize object from byte array
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="objectData"></param>
        /// <returns></returns>
        public static T XmlDeserializeFromBytes<T>(byte[] objectData)
        {
            return (T)XmlDeserializeFromString(Encoding.UTF8.GetString(objectData ?? new byte[0]), typeof(T));
        }

        /// <summary>
        /// Deserilize object from string
        /// </summary>
        /// <param name="objectData">XML string</param>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <returns>Deserialized object</returns>
        public static T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        /// <summary>
        /// Deserilize object from string
        /// </summary>
        /// <remarks>
        /// Return null if invalid string
        /// </remarks>
        /// <param name="objectData">XML string</param>
        /// <param name="type">Type to deserialize into</param>
        /// <returns>Deserialized object</returns>
        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(type);
            object result = null;
            try
            {
                using (var stringReader  = new StringReader(objectData))
                {
                    XmlReader reader = new XmlTextReader(stringReader);
                    
                    if (serializer.CanDeserialize(reader))
                    {
                        result = serializer.Deserialize(reader);
                    }
                    
                }
            }
            //If the xml is not valid we want to return null
            catch (XmlException)
            {
                
            }
            return result;
        }
    }
}
