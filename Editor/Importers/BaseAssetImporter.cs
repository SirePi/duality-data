// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Editor.AssetManagement;
using SnowyPeak.Duality.Plugin.Data.Resources;
using System;
using System.IO;
using System.Text;

namespace SnowyPeak.Duality.Editor.Plugin.Data.Importers
{
	internal class BaseAssetImporter
	{
		internal static void Import<T>(IAssetImportEnvironment env) where T : TextFile, new()
		{
			foreach (AssetImportInput input in env.Input)
			{
				ContentRef<T> targetRef = env.GetOutput<T>(input.AssetName);

				if (targetRef.IsAvailable)
				{
					T target = targetRef.Res;

					if (!String.IsNullOrWhiteSpace(input.Path))
					{
						using (System.IO.StreamReader sr = new StreamReader(input.Path))
						{
							string content = sr.ReadToEnd();
							target.SetData(content, sr.CurrentEncoding.GetByteCount(content), sr.CurrentEncoding);
						}
					}

					env.AddOutput(targetRef, input.Path);
				}
			}
		}

		internal static void Export(TextFile input, string outputPath)
		{
			if (!String.IsNullOrWhiteSpace(outputPath))
			{
				Encoding encoding = input.Encoding == null ? Encoding.UTF8 : input.Encoding;

				using (System.IO.StreamWriter sw = new StreamWriter(outputPath, false, encoding))
				{
					sw.Write(input.RawContent);
				}
			}
		}
	}
}