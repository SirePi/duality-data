// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using Duality;
using Duality.Cloning;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Data.Properties;
using Newtonsoft.Json.Linq;

namespace SnowyPeak.Duality.Plugin.Data.Resources
{
    /// <summary>
    /// Allows Row/Column based access to a valid Json file
    /// </summary>
    [EditorHintCategory(ResNames.CategoryData)]
    [EditorHintImage(ResNames.ImageJson)]
    public class JsonData : TextFile
    {
        private bool _isValid;
		
		[DontSerialize]
        private JObject _jsonObject;

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

		[EditorHintFlags(MemberFlags.Invisible)]
		public JObject JsonObject
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
        protected override void AfterLoad()
        {
            base.AfterLoad();
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

            _jsonObject = null;
            try 
            {
                _jsonObject = JObject.Parse(RawContent); 
            }
            catch(Exception ex)
            {
                _isValid = false;
                Log.Editor.WriteError(ex.Message);
            }
        }
    }
}