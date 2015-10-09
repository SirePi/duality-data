// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.IO;

using Duality;
using Duality.Editor;

using SnowyPeak.Duality.Plugin.Data.Resources;
using Duality.Editor.AssetManagement;
using System;

namespace SnowyPeak.Duality.Editor.Plugin.Data.Importers
{
    public class PlainTextFileImporter : IAssetImporter
    {
        public static readonly string SourceFileExtPrimary = ".txt";

        public string Id
        {
            get { return "PlainTextFileImporter"; }
        }
        public string Name
        {
            get { return "TXT Importer"; }
        }
        public int Priority
        {
            get { return 0; }
        }

        public void PrepareImport(IAssetImportEnvironment env)
        {
            // Ask to handle all input that matches the conditions in AcceptsInput
            foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
            {
                // For all handled input items, specify which Resource the importer intends to create / modify
                env.AddOutput<PlainTextData>(input.AssetName, input.Path);

                global::Duality.Log.Editor.WriteWarning("");
            }
        }
        public void Import(IAssetImportEnvironment env)
        {
            // Handle all available input. No need to filter or ask for this anymore, as
            // the preparation step already made a selection with AcceptsInput. We won't
            // get any input here that didn't match.
            BaseAssetImporter.Import<PlainTextData>(env);
        }

        public void PrepareExport(IAssetExportEnvironment env)
        {
            // We can export any Resource that is a Pixmap
            if (env.Input is PlainTextData)
            {
                // Add the file path of the exported output we'll produce.
                env.AddOutputPath(env.Input.Name + SourceFileExtPrimary);
            }
        }
        public void Export(IAssetExportEnvironment env)
        {
            // Determine input and output path
			PlainTextData input = env.Input as PlainTextData;
            string outputPath = env.AddOutputPath(input.Name + SourceFileExtPrimary);

            // Take the input Resource's pixel data and save it at the specified location
            BaseAssetImporter.Export(input, outputPath);
        }

        private bool AcceptsInput(AssetImportInput input)
        {
            string inputFileExt = Path.GetExtension(input.Path);
            bool matchingFileExt = SourceFileExtPrimary.Equals(inputFileExt, StringComparison.CurrentCultureIgnoreCase);
            return matchingFileExt;
        }
/*



        public bool CanImportFile(string srcFile)
        {
            string ext = Path.GetExtension(srcFile).ToLower();
            return ext == ".txt";
        }

        public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
        {
            return r.Is<PlainTextData>();
        }

        public void ReImportFile(ContentRef<Resource> r, string srcFile)
        {
            PlainTextData f = r.Res as PlainTextData;
            f.LoadFile(srcFile);
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
        }*/
    }
}