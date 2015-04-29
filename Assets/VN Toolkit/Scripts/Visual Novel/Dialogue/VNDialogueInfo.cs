using UnityEngine;
using System.Collections;
using UnityEditor;
using VNToolkit.VNUtility;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNDialogue {

	public class VNDialogueInfo : VNPanelAbstract {

		// Public Variables
		public float DialogueWidth { get; set; }
		public float DialogueHeight { get; set; }
		public int DialogueCount { get; set; }
		public int DialogueIndx { get; set; }

		// Private Variables
		private const string DIALOGUE_TEXT_WIDTH_FORMAT = "Dialogue Width: {0}";
		private const string DIALOGUE_TEXT_HEIGHT_FORMAT = "Dialogue Height: {0}";
		private const string DIALOGUE_COUNT = "Dialogue Count: {0}";
		private const string DIALOGUE_INDEX = "Dialogue Index: {0}";

		// Static Variables

		# region Panel Inspector Abstract
		public override bool IsPanelFoldable {
			get { return false; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_DIALOGUE_INFO_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_DIALOGUE_INFO; }
		}

		public override System.Action<Rect> OnPanelGUI {
			get { return DialogueInfoWindow; }
		}

		public override void OnPanelEnable(UnityEngine.Events.UnityAction repaint) {
			base.OnPanelEnable(repaint);
		}

		private void DialogueInfoWindow(Rect position) {
			string dialogueWidthText = string.Format(DIALOGUE_TEXT_WIDTH_FORMAT, DialogueWidth);
			EditorGUILayout.LabelField(dialogueWidthText, EditorStyles.label);

			string dialogueHeightText = string.Format(DIALOGUE_TEXT_HEIGHT_FORMAT, DialogueHeight);
			EditorGUILayout.LabelField(dialogueHeightText, EditorStyles.label);

			string dialogueCount = string.Format(DIALOGUE_COUNT, DialogueCount);
			EditorGUILayout.LabelField(dialogueCount, EditorStyles.label);

			string dialogueIndex = string.Format(DIALOGUE_INDEX, DialogueIndx);
			EditorGUILayout.LabelField(dialogueIndex, EditorStyles.label);
		}
		# endregion Panel Inspector Abstract
	}
}