// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Text;

using Duality;
using Duality.IO;
using System.IO;
using Duality.Editor;

namespace SnowyPeak.Duality.Plugin.Data.Resources
{
    /// <summary>
    /// Abstract class allowing access to the contents arbitary text-based file.
    /// </summary>
    public abstract class TextFile : Resource
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly string[] NewLineSplitter = new string[] { Environment.NewLine, "\n" };

        /// <summary>
        ///
        /// </summary>
        private string _content;

        private int _bytes;
        private Encoding _encoding;

        /// <summary>
        /// [GET] The contents of the imported file.
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public string RawContent
        {
            get { return _content; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TextFile()
        { }

        /// <summary>
        /// [GET] The size, in bytes, of the source file
        /// </summary>
        public int Bytes
        {
            get { return _bytes; }
        }

		/// <summary>
		/// [GET] The encoding of the source file
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
        public Encoding Encoding
        {
            get { return _encoding; }
        }

		/// <summary>
		/// [GET] The encoding of the source file
		/// </summary>
		public string FileEncoding
		{
			get { return _encoding == null ? "UNKNOWN" : _encoding.WebName; }
		}

        /// <summary>
        /// Creates an empty file if it's not already existing.
        /// </summary>
        /// <param name="filePath">The path of the file to create.</param>
        public void CreateIfNotExisting(string filePath = null)
        {
            if (filePath != null)
            {
                if (!FileOp.Exists(filePath))
                {
                    using (Stream s = FileOp.Open(filePath, FileAccessMode.Write)) 
                    { 
                        // nothing to do, just create an empty file
                    }
                }
            }
        }

        /// <summary>
        /// Returns the default extension for the file type.
        /// </summary>
        /// <returns></returns>
        public abstract string GetDefaultExtension();

        /// <summary>
        /// Sets the SourcePath variable and calls the Reload method.
        /// </summary>
        /// <param name="filePath">The path of the file to be loaded</param>
        public void SetData(string content, int length, Encoding encoding)
        {
            _content = content;
            _bytes = length;
            _encoding = encoding;

            AfterLoad();
        }

        /*
        public void SaveFile(string filePath)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                using (System.IO.Stream s = FileOp.Open(filePath, FileAccessMode.Write))
                using (System.IO.StreamWriter sw = new StreamWriter(s, _encoding))
                {
                    sw.Write(_content);
                }
            }
        }
        */
        /// <summary>
        /// Overridden will perform custom logic after the file has been reloaded.
        /// </summary>
        protected virtual void AfterLoad()
        { }
    }
}