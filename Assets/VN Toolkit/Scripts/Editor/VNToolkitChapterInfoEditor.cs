using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Linq;
using UnityEngine.Events;

namespace VNToolkit {
	public class VNToolkitChapterInfoEditor : VNToolkitPanelAbstract {

		// Public Variables

		// Private Variables
		private int selectedChapterIndx;
		private Vector2 chapterScrollView;

		private AnimBool chapterInfoAnim;
		private bool[] showInfoWindow;

		private VNToolkitChapterData[] chapterInfo;
		private VNToolkitChapterData[] tmpChapterInfo;

		// Static Variables

		public override string ControlName {
			get { return VNToolkitControlName.FOCUSED_CHAPTER_INFO_PANEL; }
		}

		public override bool PanelActive {
			get { return selectedChapterIndx > -1; }
		}

		public override bool OnWindowChanged {
			get {
				if (selectedChapterIndx < tmpChapterInfo.Length) {
					return !chapterInfo[selectedChapterIndx].Equals(tmpChapterInfo[selectedChapterIndx]);
				}

				return chapterInfo.SequenceEqual(tmpChapterInfo);
			}
		}

		public override System.Action WindowGUI {
			get { return ChapterInformationWindow; }
		}

		public override void Initialize(UnityAction repaint, Rect windowPos) {
			selectedChapterIndx = VNToolkitChapterEditor.selectedChapterIndx;
			chapterInfo = VNToolkitChapterEditor.chapterInfo;

			chapterInfoAnim = new AnimBool(true);
			chapterInfoAnim.valueChanged.AddListener(repaint);
			showInfoWindow = new bool[0];

			RepaintPanel();
		}

		public override void RepaintPanel(bool forcedSave = false) {
			selectedChapterIndx = VNToolkitChapterEditor.selectedChapterIndx;
			chapterInfo = VNToolkitChapterEditor.chapterInfo;

			if (forcedSave) {
				tmpChapterInfo = new VNToolkitChapterData[0];
				tmpChapterInfo = chapterInfo.Select(data => (VNToolkitChapterData)data.Clone()).ToArray();		// Deep copy of chapterInfo
			}

			//if (tmpChapterInfo != null && tmpChapterInfo.Length > 0) {
			//    Debug.Log(chapterInfo[0].chapterDesc.Equals(tmpChapterInfo[0].chapterDesc));
			//    Debug.Log(chapterInfo.SequenceEqual<VNToolkitChapterData>(tmpChapterInfo));
			//    Debug.Log(chapterInfo[0].chapterDesc + " " + tmpChapterInfo[0].chapterDesc);
			//}

			int infoCount = chapterInfo.Length;
			showInfoWindow = new bool[infoCount];

			for (int i = 0; i < infoCount; i++) {
				showInfoWindow[i] = true;
			}
		}

		public override void SavePanel() {
			RepaintPanel(true);
		}

		public void ChapterInformationWindow() {
			EditorGUILayout.BeginVertical("box", GUILayout.Width(VNToolkitConstants.CHAPTER_INFO_WINDOW_FIXED_WIDTH));

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Chapter Information", EditorStyles.boldLabel)) {
				showInfoWindow[selectedChapterIndx] = !showInfoWindow[selectedChapterIndx];
			}
			GUILayout.FlexibleSpace();
			GUILayout.Label("ID: " + chapterInfo[selectedChapterIndx].chapterId, EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginVertical();
			chapterInfoAnim.target = showInfoWindow[selectedChapterIndx];
			if (EditorGUILayout.BeginFadeGroup(chapterInfoAnim.faded)) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name", EditorStyles.label, GUILayout.Width(70f));
				GUI.SetNextControlName(VNToolkitControlName.FOCUSED_CHAPTER_INFO_NAME);
				chapterInfo[selectedChapterIndx].chapterName = EditorGUILayout.TextArea(chapterInfo[selectedChapterIndx].chapterName, EditorStyles.textArea);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Description", EditorStyles.label, GUILayout.Width(70f));
				GUI.SetNextControlName(VNToolkitControlName.FOCUSED_CHAPTER_INFO_DESC);
				chapterInfo[selectedChapterIndx].chapterDesc = EditorGUILayout.TextArea(chapterInfo[selectedChapterIndx].chapterDesc, EditorStyles.textArea);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.EndVertical();

			EditorGUILayout.EndVertical();
		}
	}
}