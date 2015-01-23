// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Duality;
using Duality.Cloning;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Data.Properties;

namespace SnowyPeak.Duality.Plugin.Data.Resources
{
    /// <summary>
    /// Allows Row/Column based access to a valid Json file
    /// </summary>
    [Serializable]
    [EditorHintCategory(typeof(Res), ResNames.CategoryData)]
    [EditorHintImage(typeof(Res), ResNames.ImageJson)]
    public class JsonData : TextFile
    {
        /// <summary>
        /// A JsonData Resource file extension.
        /// </summary>
        public new static string FileExt = ".JsonData" + Resource.FileExt;

        private bool _isValid;
        private dynamic _jsonObject;

        /// <summary>
        /// Creates a new, empty JsonData.
        /// </summary>
        public JsonData()
        {
            _jsonObject = null;
        }

        /// <summary>
        /// [GET] Indicates if the json file is valid.
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }
        public dynamic JsonObject
        {
            get { return _jsonObject; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string GetDefaultExtension()
        {
            return ".json";
        }

        /// <summary>
        ///
        /// </summary>
        protected override void AfterReload()
        {
            base.AfterReload();
            Parse();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="operation"></param>
        protected override void OnCopyDataTo(object target, ICloneOperation operation)
        {
            base.OnCopyDataTo(target, operation);
            JsonData targetJson = target as JsonData;

            targetJson.Parse();
        }

        private void Parse()
        {
            _isValid = true;

            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.RegisterConverters(new JavaScriptConverter[] { new DynamicJsonConverter() });

            _jsonObject = null;
            try 
            { 
                _jsonObject = jss.Deserialize<object>(_content) as dynamic; 
            }
            catch(Exception ex)
            {
                _isValid = false;
                Log.Editor.WriteError(ex.Message);
            }
        }
    }
}