using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using VNToolkit.VNUtility;
using System.Collections.Generic;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility.VNCustomInspector;

namespace VNToolkit.VNDialogue {

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
			textGenSettings = new TextGenerationSettings();
			textGenSettings.font = dialogueFont;
			textGenSettings.fontSize = dialogueTextUI.fontSize;

			dialogueReader = new VNDialogueReader();
			dialogueList = new List<string>();
			dialogueList.AddRange(dialogueReader.GetDialogues(dialogueText, textGenSettings));
			AddChildren(dialogueReader);

			dialogueIndx = 0;
			dialogueInputText = dialogueList[dialogueIndx];

			dialogueWriter = new VNDialogueWriter();
			dialogueWriter.InitializeWriter(dialogueInputText, 0.03f);
			dialogueWriter.onWriterEnd = OnWriterEnd;
			AddChildren(dialogueWriter);

			dialogueInfo = new VNDialogueInfo();
			dialogueInfo.DialogueWidth = dialogueTextUI.rectTransform.rect.width;
			dialogueInfo.DialogueHeight = dialogueTextUI.rectTransform.rect.height;
			dialogueInfo.DialogueCount = dialogueList.Count;
			dialogueInfo.DialogueIndx = dialogueIndx;
			AddChildren(dialogueInfo);

			dialogueTextUI.text = string.Empty;

			dialoguePlayIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/play80.png", typeof(Texture2D)) as Texture2D;
			dialoguePauseIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/pause36.png", typeof(Texture2D)) as Texture2D;
			dialogueStopIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/square151.png", typeof(Texture2D)) as Texture2D;
		}

		private void Update() {
			dialogueTextUI.text = dialogueOutputText;
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
		protected override bool IsPanelFoldable {
			get { return true; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		public override string PanelTitle {
			get { return VNPanelInfo.DIALOGUE_NAME; }
		}

		public override string PanelControlName {
			get { return null; }
		}

		protected override System.Action OnPanelGUI {
			get { return DialogueInspectorDraw; }
		}

		protected override bool IsRevertButtonEnabled {
			get { return false; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);

			textGenSettings.font = dialogueFont;
			textGenSettings.fontSize = dialogueTextUI.fontSize;

			dialogueList = new List<string>();
			dialogueList.AddRange(dialogueReader.GetDialogues(dialogueText, textGenSettings));
			dialogueReader.OnPanelEnable(repaint);

			dialogueIndx = 0;
			dialogueInputText = dialogueList[dialogueIndx];

			dialogueWriter.InitializeWriter(dialogueInputText, 0.03f);
			dialogueWriter.onWriterEnd = OnWriterEnd;
			dialogueWriter.OnPanelEnable(repaint);

			dialogueInfo.DialogueWidth = dialogueTextUI.rectTransform.rect.width;
			dialogueInfo.DialogueHeight = dialogueTextUI.rectTransform.rect.height;
			dialogueInfo.DialogueCount = dialogueList.Count;
			dialogueInfo.DialogueIndx = dialogueIndx;
			dialogueInfo.OnPanelEnable(repaint);

			dialogueTextUI.text = string.Empty;
		}

		public void DialogueInspectorDraw() {
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			dialogueIcon = (dialogueWriter.WriterIsPlaying) ? dialoguePauseIcon : dialoguePlayIcon;
			if (GUILayout.Button(dialogueIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
				if (!dialogueWriter.WriterIsPlaying) { dialogueWriter.StartWriting(); }
				else { dialogueWriter.PauseWriting(); }
			}

			if (GUILayout.Button(dialogueStopIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
				dialogueTextUI.text = string.Empty;

				dialogueIndx = 0;
				dialogueInputText = dialogueList[dialogueIndx];

				dialogueWriter.InitializeWriter(dialogueInputText, 0.03f);

				dialogueInfo.DialogueWidth = dialogueTextUI.rectTransform.rect.width;
				dialogueInfo.DialogueHeight = dialogueTextUI.rectTransform.rect.height;
				dialogueInfo.DialogueCount = dialogueList.Count;
				dialogueInfo.DialogueIndx = dialogueIndx;

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