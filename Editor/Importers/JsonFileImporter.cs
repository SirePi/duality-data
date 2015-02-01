// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.IO;

using Duality;
using Duality.Editor;

using SnowyPeak.Duality.Plugin.Data.Resources;

namespace SnowyPeak.Duality.Editor.Plugin.Data
{
    public class JsonFileImporter : IFileImporter
    {
        public bool CanImportFile(string srcFile)
        {
            string ext = Path.GetExtension(srcFile).ToLower();
            return ext == ".json";
        }

        public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
        {
            return r.Is<JsonData>();
        }

        public void ReImportFile(ContentRef<Resource> r, string srcFile)
        {
            JsonData f = r.Res as JsonData;
            f.LoadFile(srcFile);
        }

        public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
        {
            string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), JsonData.FileExt);
            return new string[] { targetResPath };
        }

        public void ImportFile(string srcFile, string targetName, string targetDir)
        {
            string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);

            JsonData res = new JsonData();
            res.LoadFile(srcFile);
            res.Save(output[0]);
        }

        public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
        {
            return r.As<JsonData>() != null && r.Res.SourcePath == srcFile;
        }

        public void ReimportFile(ContentRef<Resource> r, string srcFile)
        {
            JsonData f = r.Res as JsonData;

            if (f != null)
                f.LoadFile(srcFile);
        }
    }
}