using Duality;
using Duality.IO;
using SnowyPeak.Duality.Plugin.Data.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugin.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class DataLoader
	{
		/// <summary>
		/// Loads a data file at runtime
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <returns></returns>
		public static ContentRef<T> LoadFile<T>(string path) where T : TextFile
		{
			T result = null;

			if(FileOp.Exists(path))
			{
				using (Stream s = FileOp.Open(path, FileAccessMode.Read))
				using (StreamReader sr = new StreamReader(s))
				{
					string content = sr.ReadToEnd();
					result.SetData(content, sr.CurrentEncoding.GetByteCount(content), sr.CurrentEncoding);
				}
			}

			return new ContentRef<T>(result) { Path = path.Replace(@"/", @":").Replace(@"\", @":") };
		}
	}
}
