using Duality;
using Duality.Editor.AssetManagement;
using SnowyPeak.Duality.Plugin.Data.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                    // Update pixel data from the input file
                    if (!String.IsNullOrWhiteSpace(input.Path))
                    {
                        using (System.IO.StreamReader sr = new StreamReader(input.Path))
                        {
                            string line = String.Empty;
                            StringBuilder sb = new StringBuilder();

                            do
                            {
                                line = sr.ReadLine();
                                if (line != null)
                                {
                                    sb.AppendLine(line);
                                }
                            } while (line != null);

                            string content = sb.ToString();
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
                using (System.IO.StreamWriter sw = new StreamWriter(outputPath, false, input.Encoding))
                {
                    sw.Write(input.RawContent);
                }
            }
        }
    }
}
