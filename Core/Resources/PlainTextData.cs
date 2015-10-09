// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Text;

using Duality;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Data.Properties;

namespace SnowyPeak.Duality.Plugin.Data.Resources
{
    /// <summary>
    /// Allow line-based access to the contents of a text (.txt) file.
    /// </summary>
    [EditorHintCategory(ResNames.CategoryData)]
    [EditorHintImage(ResNames.ImageText)]
    public class PlainTextData : TextFile
    {
        private string[] _lines;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlainTextData()
        { }

        /// <summary>
        /// [GET] The lines in the imported file.
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public string[] Lines
        {
            get { return _lines; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string GetDefaultExtension()
        {
            return ".txt";
        }

        /// <summary>
        ///
        /// </summary>
        protected override void AfterLoad()
        {
            base.AfterLoad();
            _lines = RawContent.Split(TextFile.NewLineSplitter, StringSplitOptions.None);
        }
    }
}