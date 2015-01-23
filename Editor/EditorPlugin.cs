// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;

using SnowyPeak.Duality.Plugin.Data.Resources;

namespace SnowyPeak.Duality.Editor.Plugin.Data
{
    public class DualityDataEditorPlugin : EditorPlugin
    {
        public override string Id
        {
            get { return "SnowyPeak.Duality.Plugin.Data"; }
        }

        protected override void InitPlugin(MainForm main)
        {
            base.InitPlugin(main);

            FileEventManager.ResourceModified += this.FileEventManager_ResourceChanged;
            DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;

            //Revalidating all XmlData
            foreach (ContentRef<XmlData> xd in ContentProvider.GetLoadedContent<XmlData>())
            {
                xd.Res.Validate();
            }
        }

        private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
        {
            if (e.Objects.ResourceCount > 0)
            {
                foreach (var r in e.Objects.Resources)
                    this.OnResourceModified(r);
            }
        }

        private void FileEventManager_ResourceChanged(object sender, ResourceEventArgs e)
        {
            if (e.IsResource) this.OnResourceModified(e.Content);
        }

        private void OnResourceModified(ContentRef<Resource> resRef)
        {
            List<object> changedObj = null;

            if (resRef.Is<XmlData>() || resRef.Is<XmlSchema>())
            {
                ContentRef<XmlData> fileRef = resRef.As<XmlData>();
                ContentRef<XmlSchema> schemaRef = resRef.As<XmlSchema>();

                foreach (ContentRef<XmlData> xd in ContentProvider.GetLoadedContent<XmlData>())
                {
                    if (!xd.IsAvailable) continue;

                    // Issue #2. Missing check on Schema.IsExplicitNull would cause an endless loop 
                    if (!xd.Res.Schema.IsExplicitNull && xd.Res.Schema == schemaRef)
                    {
                        xd.Res.Validate();

                        if (changedObj == null) changedObj = new List<object>();
                        changedObj.Add(xd.Res);
                    }
                }
            }

            // Notify a change that isn't critical regarding persistence (don't flag stuff unsaved)
            if (changedObj != null)
                DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(changedObj as IEnumerable<object>), false);
        }
    }
}