using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Mp3SplitterCommon
{
	public class XmlFactory
	{
		public static T LoadFromFile<T>(string filename) where T : class
		{
			T config = null;
			try
			{
				var reader = new StreamReader(filename);
				var serializer = new XmlSerializer(typeof(T));
				config = (T)serializer.Deserialize(reader);
				reader.Close();
			}
			catch (XmlException ex)
			{
				throw new Exception("XML Parse Error: " + ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				throw new Exception("XML Serialization Error: " + ex.Message);
			}
			return config;
		}
		public static void WriteToFile<T>(T config, string filename)
		{
			var x = new XmlSerializer(config.GetType());
			var sw = new StreamWriter(filename);
			x.Serialize(sw, config);
			sw.Close();
		}
	}
}
