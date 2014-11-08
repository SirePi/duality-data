// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Text;

using Duality;
using Duality.Editor;
using SnowyPeak.Duality.Data.Properties;

namespace SnowyPeak.Duality.Data.Resources
{
    /// <summary>
    /// Allow line-based access to the contents of a text (.txt) file.
    /// </summary>
    [Serializable]
    [EditorHintCategory(typeof(Res), ResNames.CategoryData)]
    [EditorHintImage(typeof(Res), ResNames.ImageText)]
    public class PlainTextData : TextFile
    {
        /// <summary>
        /// A PlainTextData Resource file extension.
        /// </summary>
        public new static string FileExt = ".PlainTextData" + Resource.FileExt;

        private string[] _lines;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlainTextData()
        { }

        /// <summary>
        /// [GET] The contents of the imported file.
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public string Content
        {
            get { return _content; }
        }

        /// <summary>
        /// [GET] The number of lines in the imported file.
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
        protected override void AfterReload()
        {
            base.AfterReload();
            _lines = _content.Split(TextFile.NewLineSplitter, StringSplitOptions.None);
        }
    }
}