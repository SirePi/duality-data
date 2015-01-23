// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.IO;

using Duality;
using Duality.Editor;

using SnowyPeak.Duality.Plugin.Data.Resources;

namespace SnowyPeak.Duality.Editor.Plugin.Data
{
    public class PlainTextFileImporter : IFileImporter
    {
        public bool CanImportFile(string srcFile)
        {
            string ext = Path.GetExtension(srcFile).ToLower();
            return ext == ".txt";
        }

        public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
        {
            string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), TextFile.FileExt);
            return new string[] { targetResPath };
        }

        public void ImportFile(string srcFile, string targetName, string targetDir)
        {
            string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);

            PlainTextData res = new PlainTextData();
            res.LoadFile(srcFile);
            res.Save(output[0]);
        }

        public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
        {
            return r.As<PlainTextData>() != null && r.Res.SourcePath == srcFile;
        }

        public void ReimportFile(ContentRef<Resource> r, string srcFile)
        {
            PlainTextData f = r.Res as PlainTextData;

            if (f != null)
                f.LoadFile(srcFile);
        }
    }
}