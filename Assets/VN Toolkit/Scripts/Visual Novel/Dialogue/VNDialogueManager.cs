using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using VNToolkit.VNUtility;
using VNToolkit.VNUtility.VNIcon;
using System.Collections.Generic;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility.VNCustomInspector;


namespace VNToolkit.VNCore.VNDialogue {

	[ExecuteInEditMode]
	public class VNDialogueManager : VNPanelMonoAbstract {

		// Public Variables
		[SerializeField]
		private string dialogueText;

		[SerializeField]
		private Text dialogueTextUI;

		[SerializeField]
		private Font dialogueFont;

		// Private Variables
		private string dialogueInputText;
		private string dialogueOutputText;

		private VNDialogueInfo dialogueInfo;
		private VNDialogueReader dialogueReader;
		private VNDialogueWriter dialogueWriter;

		private List<string> dialogueList;
		private int dialogueIndx;

		private Texture2D dialoguePlayIcon;
		private Texture2D dialoguePauseIcon;
		private Texture2D dialogueStopIcon;
		private Texture2D dialogueIcon;

		private TextGenerationSettings textGenSettings;

		// Static Variables 

		private void OnEnable() {
			dialoguePlayIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_PLAY);
			dialoguePauseIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_PAUSE);
			dialogueStopIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_STOP);
		}

		private void Update() {
			if (dialogueTextUI != null) {
				dialogueTextUI.text = dialogueOutputText;
			}
		}

		private void OnWriterEnd() {
			if (dialogueIndx > dialogueList.Count)
				return;

			textGenSettings = new TextGenerationSettings();
			textGenSettings.font = dialogueFont;
			textGenSettings.fontSize = dialogueTextUI.fontSize;

			dialogueTextUI.text = string.Empty;

			dialogueIndx++;
			dialogueInputText = dialogueList[dialogueIndx];

			dialogueWriter.InitializeWriter(dialogueInputText, 0.03f);

			dialogueInfo.DialogueWidth = dialogueTextUI.rectTransform.rect.width;
			dialogueInfo.DialogueHeight = dialogueTextUI.rectTransform.rect.height;
			dialogueInfo.DialogueCount = dialogueList.Count;
			dialogueInfo.DialogueIndx = dialogueIndx;

			dialogueWriter.StartWriting();
		}

		# region Panel Inspector Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.DIALOGUE_NAME; }
		}

		public override string PanelControlName {
			get { return null; }
		}

		protected override bool IsPanelFoldable {
			get { return true; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override bool IsRefreshable {
			get { return true; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		protected override System.Action OnPanelGUI {
			get { return DialogueInspectorDraw; }
		}

		protected override bool IsRevertButtonEnabled {
			get { return false; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);

			if (dialogueText == string.Empty)
				return;

			if (dialogueTextUI == null)
				return;

			if (dialogueFont == null)
				return;

			textGenSettings = new TextGenerationSettings();
			textGenSettings.font = dialogueFont;
			textGenSettings.fontSize = dialogueTextUI.fontSize;

			dialogueReader = new VNDialogueReader();
			dialogueList = new List<string>();
			dialogueList.AddRange(dialogueReader.GetDialogues(dialogueText, textGenSettings));
			dialogueReader.OnPanelEnable(Repaint);
			AddChildren(dialogueReader);

			dialogueIndx = 0;
			dialogueInputText = dialogueList[dialogueIndx];

			dialogueWriter = new VNDialogueWriter();
			dialogueWriter.InitializeWriter(dialogueInputText, 0.03f);
			dialogueWriter.onWriterEnd = OnWriterEnd;
			dialogueWriter.OnPanelEnable(Repaint);
			AddChildren(dialogueWriter);

			dialogueInfo = new VNDialogueInfo();
			dialogueInfo.DialogueWidth = dialogueTextUI.rectTransform.rect.width;
			dialogueInfo.DialogueHeight = dialogueTextUI.rectTransform.rect.height;
			dialogueInfo.DialogueCount = dialogueList.Count;
			dialogueInfo.DialogueIndx = dialogueIndx;
			dialogueInfo.OnPanelEnable(Repaint);
			AddChildren(dialogueInfo);

			dialogueTextUI.text = string.Empty;
		}

		protected override void PanelRefresh() {
			if (dialogueText == string.Empty)
				return;

			if (dialogueTextUI == null)
				return;

			if (dialogueFont == null)
				return;

			textGenSettings.font = dialogueFont;
			textGenSettings.fontSize = dialogueTextUI.fontSize;

			dialogueList = new List<string>();
			dialogueList.AddRange(dialogueReader.GetDialogues(dialogueText, textGenSettings));

			dialogueIndx = 0;
			dialogueInputText = dialogueList[dialogueIndx];

			dialogueWriter.InitializeWriter(dialogueInputText, 0.03f);

			dialogueInfo.DialogueWidth = dialogueTextUI.rectTransform.rect.width;
			dialogueInfo.DialogueHeight = dialogueTextUI.rectTransform.rect.height;
			dialogueInfo.DialogueCount = dialogueList.Count;
			dialogueInfo.DialogueIndx = dialogueIndx;

			dialogueTextUI.text = string.Empty;

			base.PanelRefresh();
		}

		public void DialogueInspectorDraw() {
			if (dialogueReader == null || dialogueWriter == null || dialogueInfo == null)
				return;

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			dialogueIcon = (dialogueWriter.WriterIsPlaying) ? dialoguePauseIcon : dialoguePlayIcon;
			if (GUILayout.Button(dialogueIcon, EditorStyles.miniButton, GUILayout.Width(22f), GUILayout.Height(22f))) {
				if (!dialogueWriter.WriterIsPlaying) { dialogueWriter.StartWriting(); }
				else { dialogueWriter.PauseWriting(); }
			}

			if (GUILayout.Button(dialogueStopIcon, EditorStyles.miniButton, GUILayout.Width(22f), GUILayout.Height(22f))) {
				dialogueTextUI.text = string.Empty;
				dialogueWriter.StopWrinting();
			}

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			dialogueWriter.UpdateWriter(this);
			dialogueOutputText = dialogueWriter.WriterOutputText;

			dialogueInfo.OnPanelDraw(new Rect());
			dialogueReader.OnPanelDraw(new Rect());
			dialogueWriter.OnPanelDraw(new Rect());
		}
		# endregion Panel Inspector Abstract
	}
}