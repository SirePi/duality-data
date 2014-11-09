// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Text;
using System.Xml;

using Duality;
using Duality.Editor;

using SnowyPeak.Duality.Plugins.Data.Properties;

namespace SnowyPeak.Duality.Plugins.Data.Resources
{
    /// <summary>
    /// Represents an arbitrary piece of XML-formatted data.
    /// </summary>
    [Serializable]
    [EditorHintCategory(typeof(Res), ResNames.CategoryData)]
    [EditorHintImage(typeof(Res), ResNames.ImageXsd)]
    public class XmlSchema : TextFile
    {
        /// <summary>
        /// A XmlSchema resources file extension.
        /// </summary>
        public new static string FileExt = ".XmlSchema" + Resource.FileExt;

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
        internal string Content
        {
            get { return _content; }
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
        protected override void AfterReload()
        {
            base.AfterReload();

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(_content);

            _validatingNamespace = xDoc.FirstChild.NextSibling.Attributes["targetNamespace"].Value;
        }
    }
}