// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Editor;

using SnowyPeak.Duality.Data.Editor.Properties;
using SnowyPeak.Duality.Data.Resources;

namespace SnowyPeak.Duality.Data.Editor.Actions
{
    /// <summary>
    /// Double-click binding for the editor
    /// </summary>
    public class EditorActionOpenFile : EditorSingleAction<TextFile>
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Description
        {
            get { return EditorRes.ActionDesc_OpenFile; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get { return EditorRes.ActionName_OpenFile; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool CanPerformOn(TextFile obj)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool MatchesContext(string context)
        {
            return context == DualityEditorApp.ActionContextOpenRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public override void Perform(TextFile file)
        {
            FileImportProvider.OpenSourceFile(file, file.GetDefaultExtension(), file.CreateIfNotExisting);
        }
    }
}