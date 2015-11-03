// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Editor.Plugin.Data
{
	public class EditorBasePlugin : EditorPlugin
	{
		public override string Id
		{
			get { return "SnowyPeak.Duality.Editor.Plugin.Data"; }
		}

		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			FileEventManager.ResourceModified += this.FileEventManager_ResourceChanged;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
		}

		private void FileEventManager_ResourceChanged(object sender, ResourceEventArgs e)
		{
			if (e.IsResource) this.OnResourceModified(e.Content);
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.Objects.ResourceCount > 0)
			{
				foreach (var r in e.Objects.Resources)
					this.OnResourceModified(r);
			}
		}

		private void OnResourceModified(ContentRef<Resource> resRef)
		{
			List<object> changedObj = null;

			// Notify a change that isn't critical regarding persistence (don't flag stuff unsaved)
			if (changedObj != null)
				DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(changedObj as IEnumerable<object>), false);
		}
	}
}