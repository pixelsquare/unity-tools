using UnityEngine;
using System.Collections;
using VNToolkit.VNUtility.VNCustomInspector;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using VNToolkit.VNUtility;

namespace VNToolkit {
	namespace VNDialogue {
		[ExecuteInEditMode]
		public class VNDialogueManager : VNPanelInspectorAbstract {

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

			private VNDialogueReader dialogueReader;
			private VNDialogueWriter dialogueWriter;

			private List<string> dialogueList;
			private int dialogueIndx;

			//private float dialogueTextWidth;
			//private float dialogueTextHeight;

			private Texture2D dialoguePlayIcon;
			private Texture2D dialoguePauseIcon;
			private Texture2D dialogueStopIcon;
			private Texture2D dialogueIcon;

			private const string DIALOGUE_TEXT_WIDTH_FORMAT = "Dialogue Width: {0}";
			private const string DIALOGUE_TEXT_HEIGHT_FORMAT = "Dialogue Height: {0}";

			// Static Variables 

			private void OnEnable() {
				TextGenerationSettings textGenSettings = new TextGenerationSettings();
				textGenSettings.font = dialogueFont;
				textGenSettings.fontSize = dialogueTextUI.fontSize;

				dialogueReader = new VNDialogueReader();
				dialogueList = new List<string>();
				dialogueList.AddRange(dialogueReader.GetDialogues(dialogueText, textGenSettings));

				dialogueIndx = 0;
				dialogueInputText = dialogueList[dialogueIndx];

				dialogueWriter = new VNDialogueWriter();
				dialogueWriter.InitializeWriter(dialogueInputText, 0.02f);

				dialogueTextUI.text = string.Empty;

				dialoguePlayIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/play80.png", typeof(Texture2D)) as Texture2D;
				dialoguePauseIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/pause36.png", typeof(Texture2D)) as Texture2D;
				dialogueStopIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/square151.png", typeof(Texture2D)) as Texture2D;
			}

			private void Update() {
				dialogueTextUI.text = dialogueOutputText;
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

				TextGenerationSettings textGenSettings = new TextGenerationSettings();
				textGenSettings.font = dialogueFont;
				textGenSettings.fontSize = dialogueTextUI.fontSize;

				dialogueReader = new VNDialogueReader();
				dialogueList = new List<string>();
				dialogueList.AddRange(dialogueReader.GetDialogues(dialogueText, textGenSettings));

				dialogueIndx = 0;
				dialogueInputText = dialogueList[dialogueIndx];

				dialogueWriter = new VNDialogueWriter();
				dialogueWriter.InitializeWriter(dialogueInputText, 0.02f);

				dialogueTextUI.text = string.Empty;

				dialoguePlayIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/play80.png", typeof(Texture2D)) as Texture2D;
				dialoguePauseIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/pause36.png", typeof(Texture2D)) as Texture2D;
				dialogueStopIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/square151.png", typeof(Texture2D)) as Texture2D;
			}

			public void DialogueInspectorDraw() {
				//EditorGUILayout.BeginVertical();
				//string dialogueWidthText = string.Format(DIALOGUE_TEXT_WIDTH_FORMAT, dialogueTextWidth);
				//EditorGUILayout.LabelField(dialogueWidthText, EditorStyles.label);

				//string dialogueHeightText = string.Format(DIALOGUE_TEXT_HEIGHT_FORMAT, dialogueTextHeight);
				//EditorGUILayout.LabelField(dialogueHeightText, EditorStyles.label);
				//EditorGUILayout.EndVertical();

				//if (dialogueText != null) {
				//    dialogueTextWidth = dialogueText.rectTransform.rect.width;
				//    dialogueTextHeight = dialogueText.rectTransform.rect.height;
				//}

				//if (GUILayout.Button("Change Text")) {
				//    count++;
				//    dialogueText.text = dialogueList[count];
				//}

				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();

				dialogueIcon = (dialogueWriter.WriterIsPlaying) ? dialoguePauseIcon : dialoguePlayIcon;
				if (GUILayout.Button(dialogueIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
					if (!dialogueWriter.WriterIsPlaying) { dialogueWriter.StartWriting(); }
					else { dialogueWriter.PauseWriting(); }
				}

				if (GUILayout.Button(dialogueStopIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
					dialogueWriter.StopWrinting();
					dialogueTextUI.text = string.Empty;
				}

				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				dialogueWriter.UpdateWriter(this);
				dialogueOutputText = dialogueWriter.WriterOutputText;
			}
			# endregion Panel Inspector Abstract
		}
	}
}