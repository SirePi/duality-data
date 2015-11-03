// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality.Editor.AssetManagement;
using SnowyPeak.Duality.Plugin.Data.Resources;
using System;
using System.IO;

namespace SnowyPeak.Duality.Editor.Plugin.Data.Importers
{
	public class CsvFileImporter : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".csv";

		public string Id
		{
			get { return "CsvFileImporter"; }
		}

		public string Name
		{
			get { return "CSV Importer"; }
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
				env.AddOutput<CsvData>(input.AssetName, input.Path);
			}
		}

		public void Import(IAssetImportEnvironment env)
		{
			// Handle all available input. No need to filter or ask for this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			BaseAssetImporter.Import<CsvData>(env);
		}

		public void PrepareExport(IAssetExportEnvironment env)
		{
			// We can export any Resource that is a Pixmap
			if (env.Input is CsvData)
			{
				// Add the file path of the exported output we'll produce.
				env.AddOutputPath(env.Input.Name + SourceFileExtPrimary);
			}
		}

		public void Export(IAssetExportEnvironment env)
		{
			// Determine input and output path
			CsvData input = env.Input as CsvData;
			string outputPath = env.AddOutputPath(input.Name + SourceFileExtPrimary);

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
			return ext == ".csv";
		}

		public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
		{
			return r.Is<CsvData>();
		}

		public void ReImportFile(ContentRef<Resource> r, string srcFile)
		{
			CsvData f = r.Res as CsvData;
			f.LoadFile(srcFile);
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
		}*/
	}
}