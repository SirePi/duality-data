// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.IO;

using Duality;
using Duality.Editor;

using SnowyPeak.Duality.Plugin.Data.Resources;

namespace SnowyPeak.Duality.Editor.Plugin.Data
{
    public class XmlFileImporter : IFileImporter
    {
        public bool CanImportFile(string srcFile)
        {
            string ext = Path.GetExtension(srcFile).ToLower();
            return ext == ".xml" || ext == ".xsd";
        }

        public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
        {
            string ext = Path.GetExtension(srcFile).ToLower();
            string targetResPath;
            if (ext == ".xml")
                targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), XmlData.FileExt);
            else
                targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), XmlSchema.FileExt);
            return new string[] { targetResPath };
        }

        public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
        {
            return r.Is<XmlData>() || r.Is<XmlSchema>();
        }

        public void ReImportFile(ContentRef<Resource> r, string srcFile)
        {
            TextFile f = r.Res as XmlData;
            if (f == null)
            {
                f = r.Res as XmlSchema;
            }

            f.LoadFile(srcFile);
        }

        public void ImportFile(string srcFile, string targetName, string targetDir)
        {
            string ext = Path.GetExtension(srcFile).ToLower();
            string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);

            TextFile res = (ext == ".xml") ? new XmlData() as TextFile : new XmlSchema() as TextFile;
            res.LoadFile(srcFile);
            res.Save(output[0]);
        }

        public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
        {
            return r.As<TextFile>() != null && r.Res.SourcePath == srcFile;
        }

        public void ReimportFile(ContentRef<Resource> r, string srcFile)
        {
            TextFile f = r.Res as TextFile;

            if (f != null)
                f.LoadFile(srcFile);
        }
    }
}