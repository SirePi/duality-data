// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality.Editor;

using SnowyPeak.Duality.Plugin.Data.Properties;

namespace SnowyPeak.Duality.Plugin.Data.Resources
{
	/// <summary>
	/// Represents an arbitrary piece of XML-formatted data.
	/// </summary>
	[EditorHintCategory(ResNames.CategoryData)]
	[EditorHintImage(ResNames.ImageXsd)]
	public class XmlSchema : TextFile
	{
		private string _validatingNamespace;

		/// <summary>
		/// Constructor
		/// </summary>
		public XmlSchema()
		{ }

		/// <summary>
		/// [GET] The namespace set as the xsd file's targetNamespace attribute
		/// </summary>
		public string ValidatingNamespace
		{
			get { return _validatingNamespace; }
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public override string GetDefaultExtension()
		{
			return ".xsd";
		}

		/// <summary>
		///
		/// </summary>
		protected override void AfterLoad()
		{
			base.AfterLoad();

			//XDocument xDoc = XDocument.Parse(_content);

			//_validatingNamespace = xDoc.Root.X.NextSibling.Attributes["targetNamespace"].Value;
		}
	}
}