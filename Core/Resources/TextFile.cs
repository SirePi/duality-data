// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Text;

using Duality;

namespace SnowyPeak.Duality.Plugins.Data.Resources
{
    /// <summary>
    /// Abstract class allowing access to the contents arbitary text-based file.
    /// </summary>
    [Serializable]
    public abstract class TextFile : Resource
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly string[] NewLineSplitter = new string[] { Environment.NewLine, "\n" };

        /// <summary>
        ///
        /// </summary>
        protected string _content;

        private int _bytes;
        private Encoding _encoding;

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
        public string Encoding
        {
            get { return _encoding == null ? "[NULL]" : _encoding.EncodingName; }
        }

        /// <summary>
        /// Creates an empty file if it's not already existing.
        /// </summary>
        /// <param name="filePath">The path of the file to create.</param>
        public void CreateIfNotExisting(string filePath = null)
        {
            if (filePath != null)
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.WriteAllText(filePath, String.Empty);
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
        public void LoadFile(string filePath)
        {
            SourcePath = filePath;
            Reload();
        }

        /// <summary>
        /// Reloads the file and calls the abstract AfterReload method
        /// </summary>
        public void Reload()
        {
            if (!String.IsNullOrWhiteSpace(SourcePath))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(SourcePath))
                {
                    _encoding = sr.CurrentEncoding;

                    string line = String.Empty;
                    StringBuilder sb = new StringBuilder();

                    do
                    {
                        line = sr.ReadLine();
                        if (line != null)
                        {
                            sb.AppendLine(line);
                        }
                    } while (line != null);

                    _content = sb.ToString();
                    _bytes = sb.Length;
                }

                AfterReload();
            }
        }

        /// <summary>
        /// Overridden will perform custom logic after the file has been reloaded.
        /// </summary>
        protected virtual void AfterReload()
        { }
    }
}