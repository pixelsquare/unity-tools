using UnityEngine;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility;
using UnityEditor;

namespace VNToolkit.VNEditor {

	public class VNChapterInfoEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		private int chapterID;
		private string chapterName;
		private string chapterDesc;

		private string chapterIDText;
		private VNChapterListEditor chapterList;

		// Static Variables 

		private const string CHAPTER_ID_FORMAT = "Chapter ID: {0}";

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_CHAPTER_INFO_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_CHAPTER_INFO; }
		}

		protected override bool IsPanelFoldable {
			get { return false; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override bool IsRefreshable {
			get { return false; }
		}

		protected override bool IsScrollable {
			get { return false; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		protected override System.Action<Rect> OnPanelGUI {
			get { return ChapterInfoWindow; }
		}

		public override void OnPanelEnable(UnityEngine.Events.UnityAction repaint) {
			base.OnPanelEnable(repaint);
			chapterList = parent.GetChild(VNPanelInfo.PANEL_CHAPTER_LIST_NAME) as VNChapterListEditor;
			chapterID = (chapterList.CurrentElementData != null) ? chapterList.CurrentElementData.DATA_ID : 0;
			chapterName = (chapterList.CurrentElementData != null) ? chapterList.CurrentElementData.chapterName : string.Empty;
			chapterDesc = (chapterList.CurrentElementData != null) ? chapterList.CurrentElementData.chapterDesc : string.Empty;
			chapterIDText = (chapterList.CurrentElementData != null) ? string.Format(VNDataName.PROJECT_CHAPTER_FORMAT, chapterID) : string.Empty;
		}

		protected override void PanelRefresh() {
			base.PanelRefresh();

			chapterList = parent.GetChild(VNPanelInfo.PANEL_CHAPTER_LIST_NAME) as VNChapterListEditor;
			chapterID = (chapterList.CurrentElementData != null) ? chapterList.CurrentElementData.DATA_ID : 0;
			chapterName = (chapterList.CurrentElementData != null) ? chapterList.CurrentElementData.chapterName : string.Empty;
			chapterDesc = (chapterList.CurrentElementData != null) ? chapterList.CurrentElementData.chapterDesc : string.Empty;
			chapterIDText = (chapterList.CurrentElementData != null) ? string.Format(VNDataName.PROJECT_CHAPTER_FORMAT, chapterID) : string.Empty;
		}

		private void ChapterInfoWindow(Rect position) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(string.Format(CHAPTER_ID_FORMAT, chapterIDText), EditorStyles.label);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Chapter Name", EditorStyles.label, GUILayout.Width(120f));
			chapterName = EditorGUILayout.TextField(chapterName, EditorStyles.textField, GUILayout.Width(500f));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Chapter Description", EditorStyles.label, GUILayout.Width(120f));
			chapterDesc = EditorGUILayout.TextField(chapterDesc, EditorStyles.textField, GUILayout.Width(500f));
			EditorGUILayout.EndHorizontal();

			//if (chapterList.CurrentElementData != null) {
			//    if (chapterDesc != chapterList.CurrentElementData.chapterDesc) {
			//        chapterList.CurrentElementData.chapterDesc = chapterDesc;
			//    }
			//}
		}

		# endregion Panel Editor Abstract
	}
}