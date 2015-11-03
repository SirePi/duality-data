// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality.Editor.AssetManagement;
using SnowyPeak.Duality.Plugin.Data.Resources;
using System;
using System.IO;

namespace SnowyPeak.Duality.Editor.Plugin.Data.Importers
{
	public class JsonFileImporter : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".json";

		public string Id
		{
			get { return "JsonFileImporter"; }
		}

		public string Name
		{
			get { return "JSON Importer"; }
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
				env.AddOutput<JsonData>(input.AssetName, input.Path);
			}
		}

		public void Import(IAssetImportEnvironment env)
		{
			// Handle all available input. No need to filter or ask for this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			BaseAssetImporter.Import<JsonData>(env);
		}

		public void PrepareExport(IAssetExportEnvironment env)
		{
			// We can export any Resource that is a Pixmap
			if (env.Input is JsonData)
			{
				// Add the file path of the exported output we'll produce.
				env.AddOutputPath(env.Input.Name + SourceFileExtPrimary);
			}
		}

		public void Export(IAssetExportEnvironment env)
		{
			// Determine input and output path
			JsonData input = env.Input as JsonData;
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