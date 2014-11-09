// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.IO;

using Duality;
using Duality.Editor;

using SnowyPeak.Duality.Plugins.Data.Resources;

namespace SnowyPeak.Duality.Editor.Plugins.Data
{
    public class CsvFileImporter : IFileImporter
    {
        public bool CanImportFile(string srcFile)
        {
            string ext = Path.GetExtension(srcFile).ToLower();
            return ext == ".csv";
        }

        public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
        {
            string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), CsvData.FileExt);
            return new string[] { targetResPath };
        }

        public void ImportFile(string srcFile, string targetName, string targetDir)
        {
            string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);

            CsvData res = new CsvData();
            res.LoadFile(srcFile);
            res.Save(output[0]);
        }

        public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
        {
            return r.As<CsvData>() != null && r.Res.SourcePath == srcFile;
        }

        public void ReimportFile(ContentRef<Resource> r, string srcFile)
        {
            CsvData f = r.Res as CsvData;

            if (f != null)
                f.LoadFile(srcFile);
        }
    }
}