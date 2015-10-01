// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.IO;

using Duality;
using Duality.Editor;

using SnowyPeak.Duality.Plugin.Data.Resources;
using Duality.Editor.AssetManagement;
using System;

namespace SnowyPeak.Duality.Editor.Plugin.Data.Importers
{
    public class XmlFileImporter : IAssetImporter
    {
        public static readonly string SourceFileExtPrimary = ".xml";

        public string Id
        {
            get { return "XmlFileImporter"; }
        }
        public string Name
        {
            get { return "XML Importer"; }
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
                env.AddOutput<XmlData>(input.AssetName, input.Path);
            }
        }
        public void Import(IAssetImportEnvironment env)
        {
            // Handle all available input. No need to filter or ask for this anymore, as
            // the preparation step already made a selection with AcceptsInput. We won't
            // get any input here that didn't match.
            BaseAssetImporter.Import<XmlData>(env);
        }

        public void PrepareExport(IAssetExportEnvironment env)
        {
            // We can export any Resource that is a Pixmap
            if (env.Input is XmlData)
            {
                // Add the file path of the exported output we'll produce.
                env.AddOutputPath(env.Input.Name + SourceFileExtPrimary);
            }
        }
        public void Export(IAssetExportEnvironment env)
        {
            // Determine input and output path
            XmlData input = env.Input as XmlData;
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
    }
}