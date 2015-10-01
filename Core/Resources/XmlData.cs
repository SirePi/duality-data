// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;

using Duality;
using Duality.Cloning;
using Duality.Editor;

using System.Xml.Serialization;

using SnowyPeak.Duality.Plugin.Data.Properties;
using System.IO;
using System.Text;

namespace SnowyPeak.Duality.Plugin.Data.Resources
{
    /// <summary>
    /// Represents a validated XmlDocument which consists of an XmlFile and an optional XmlSchema
    /// </summary>
    /// <seealso cref="SnowyPeak.Duality.Plugin.Data.Resources.XmlData"/>
    /// <seealso cref="SnowyPeak.Duality.Plugin.Data.Resources.XmlSchema"/>
    [EditorHintCategory(ResNames.CategoryData)]
    [EditorHintImage(ResNames.ImageXdata)]
    public class XmlData : TextFile
    {
        private bool _isValid;

        //[DontSerialize]
        //private System.Xml.Schema.ValidationEventHandler _validationFailed;

        [DontSerialize]
        private XDocument _xDocument;

        [DontSerialize]
        private XmlReader _xSchemaReader;

        private ContentRef<XmlSchema> _xsd;

        /// <summary>
        /// Constructor
        /// </summary>
        public XmlData()
        {
            //_validationFailed = new System.Xml.Schema.ValidationEventHandler(ValidationFailed);
        }

        /// <summary>
        /// [GET] Indicates if the file is a well formed xml and, if Schema is set, is valid.
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }
        /// <summary>
        /// [GET / SET] The XmlSchema against which the file should be validated.
        /// </summary>
        [EditorHintFlags(MemberFlags.AffectsOthers)]
        public ContentRef<XmlSchema> Schema
        {
            get { return _xsd; }
            set { _xsd = value; Validate(); }
        }

        /// <summary>
        /// The content of the file, as an XmlDocument object
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public XDocument XmlDocument
        {
            get { return _xDocument; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string GetDefaultExtension()
        {
            return ".xml";
        }

        /// <summary>
        /// Validates the XmlData. This is done automatically whenever the Resource or its componets are changed.
        /// </summary>
        public void Validate()
        {
            _isValid = !String.IsNullOrWhiteSpace(RawContent);
            
            if (_isValid)
            {
                /*
                if (_xDocument.Schemas.Count > 0)
                {
                    System.Xml.Schema.XmlSchema[] oldSchemas = new System.Xml.Schema.XmlSchema[_xDocument.Schemas.Count];
                    _xDocument.Schemas.CopyTo(oldSchemas, 0);

                    foreach (System.Xml.Schema.XmlSchema schema in oldSchemas)
                    {
                        _xDocument.Schemas.Remove(schema);
                    }
                }
                 * */
                try
                {
                    _xDocument = XDocument.Parse(RawContent);
                }
                catch (Exception ex)
                {
                    _isValid = false;
                    Log.Editor.WriteError(ex.Message);
                }
                /*
                if (_isValid && !_xsd.IsExplicitNull)
                {
                    using (System.IO.StringReader sr = new System.IO.StringReader(_xsd.Res.Content))
                    {
                        _xSchemaReader = XmlReader.Create(sr);
                        _xDocument.Schemas.Add(_xsd.Res.ValidatingNamespace, _xSchemaReader);
                        _xDocument.Validate(_validationFailed);
                    }
                }*/
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void AfterLoad()
        {
            base.AfterLoad();
            Validate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="operation"></param>
        protected override void OnCopyDataTo(object target, ICloneOperation operation)
        {
            base.OnCopyDataTo(target, operation);
            XmlData targetXml = target as XmlData;

            targetXml.Validate();
        }
        /*
        private void ValidationFailed(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            if (e.Severity == System.Xml.Schema.XmlSeverityType.Error)
                Log.Editor.WriteError(e.Message);
            else if (e.Severity == System.Xml.Schema.XmlSeverityType.Warning)
                Log.Editor.WriteWarning(e.Message);

            _isValid = false;
        }*/
    }
}