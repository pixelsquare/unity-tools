using UnityEngine;
using System.Collections;
using VNToolkit.VNUtility.VNCustomInspector;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VNToolkit {
	public class VNDialogueManager : VNPanelInspectorAbstract {

		// Public Variables
		[SerializeField]
		private string dialogueInputText;

		[SerializeField]
		private Text dialogueText;

		[SerializeField]
		private Font dialogueFont;

		// Private Variables
		private string dialogueOutputText;

		private float dialogueTextWidth;
		private float dialogueTextHeight;

		private Dictionary<string, float> dialogueWordWidths;
		private List<string> dialogueWords;
		private List<string> dialogues;

		private const string DIALOGUE_TEXT_WIDTH_FORMAT = "Dialogue Width: {0}";
		private const string DIALOGUE_TEXT_HEIGHT_FORMAT = "Dialogue Height: {0}";
		private int count = 0;

		// Static Variables 

		private void Start() {
			dialogueWordWidths = new Dictionary<string, float>();
			dialogueWords = new List<string>();
			dialogues = new List<string>();
		}
		public void SplitDialogueText() {
			TextGenerationSettings settings = new TextGenerationSettings();
			settings.font = dialogueFont;
			settings.fontSize = dialogueText.fontSize;

			dialogueWordWidths = new Dictionary<string, float>();
			dialogueWords = new List<string>();
			dialogues = new List<string>();

			count = 0;
			float wordWidth = 0f;
			foreach (string word in dialogueInputText.Split(' ', '\n')) {
				TextGenerator generator = new TextGenerator();
				generator.Populate(word, settings);

				wordWidth = 0f;
				dialogueWords.Add(word);

				foreach (UICharInfo charInfo in generator.characters) {
					float charWidth = charInfo.charWidth;
					wordWidth += charWidth;
				}

				if (dialogueWordWidths.ContainsKey(word))
					continue;

				dialogueWordWidths.Add(word, wordWidth);
			}

			ComposeDialogue();
		}

		public void ComposeDialogue() {
			float wordWidth = 0f;
			string dialogue = string.Empty;

			for (int i = 0; i < dialogueWords.Count; i++) {
				string word = dialogueWords[i];
				if (dialogueWordWidths.ContainsKey(word)) {
					if ((wordWidth + (dialogueWordWidths[word])) < Screen.width) {
						dialogue += word + " ";
						wordWidth += dialogueWordWidths[word];
					}
					else {
						dialogues.Add(dialogue);
						dialogue = word + " ";
						wordWidth = dialogueWordWidths[word];
					}

					if (i > (dialogueWords.Count - 1)) {
						dialogues.Add(dialogue);
					}
				}
			}

			dialogueText.text = dialogues[count];
		}


		# region Panel Inspector Abstract
		public override bool IsPanelFoldable {
			get { return true; }
		}

		public override bool IsRevertButtonEnabled {
			get { return false; }
		}

		public override string PanelTitle {
			get { return VNPanelName.DIALOGUE_NAME; }
		}

		public override System.Action OnInspectorGUI {
			get { return CameraInspectorDraw; }
		}

		public override void OnInspectorEnable(UnityAction repaint) {
			base.OnInspectorEnable(repaint);

			SplitDialogueText();
		}

		public void CameraInspectorDraw() {
			EditorGUILayout.BeginVertical();
			string dialogueWidthText = string.Format(DIALOGUE_TEXT_WIDTH_FORMAT, dialogueTextWidth);
			EditorGUILayout.LabelField(dialogueWidthText, EditorStyles.label);

			string dialogueHeightText = string.Format(DIALOGUE_TEXT_HEIGHT_FORMAT, dialogueTextHeight);
			EditorGUILayout.LabelField(dialogueHeightText, EditorStyles.label);
			EditorGUILayout.EndVertical();

			if (dialogueText != null) {
				dialogueTextWidth = dialogueText.rectTransform.rect.width;
				dialogueTextHeight = dialogueText.rectTransform.rect.height;
			}

			if (GUILayout.Button("Change Text")) {
				count++;
				dialogueText.text = dialogues[count];
			}
		}
		# endregion Panel Inspector Abstract
	}
}