using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using VNToolkit.VNUtility;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit.VNDialogue {

	public class VNDialogueReader : VNPanelAbstract {

		// Public Variables
		public string[] DialogueWordList { get { return dialogueWordList; } }

		// Private Variables
		private string[] dialogueWordList;

		private const string DIALOGUE_FORMAT = "{0} ";
		private const string READER_WORD_COUNT = "Word Count: {0}";

		// Static Variables 

		/// <summary>
		/// Formatted dialogue list. Text Generation Settings MUST include font and font size 
		/// </summary>
		/// <param name="dialogueText">Dialogue Input Text</param>
		/// <param name="textGenSettings">Text Generation Settings</param>
		/// <returns>Dialogue List</returns>
		public List<string> GetDialogues(string dialogueText, TextGenerationSettings textGenSettings) {
			List<string> result = new List<string>();

			Dictionary<string, float> dialogueWords = new Dictionary<string, float>();
			dialogueWords = DialogueTextToWords(dialogueText, textGenSettings);

			StringBuilder dialogueSB = new StringBuilder();
			float wordWidth = 0f;

			for (int i = 0; i < dialogueWordList.Length; i++) {
				string word = dialogueWordList[i];
				if (dialogueWords.ContainsKey(word)) {
					if ((wordWidth + dialogueWords[word]) < Screen.width) {
						dialogueSB.AppendFormat(DIALOGUE_FORMAT, word);
						wordWidth += dialogueWords[word];
					}
					else {
						result.Add(dialogueSB.ToString());
						dialogueSB = new StringBuilder();
						dialogueSB.AppendFormat(DIALOGUE_FORMAT, word);
						wordWidth = dialogueWords[word];
					}

					if (i > (dialogueWordList.Length - 1)) {
						result.Add(dialogueSB.ToString());
						dialogueSB = new StringBuilder();
						dialogueSB.AppendFormat(DIALOGUE_FORMAT, word);
						wordWidth = dialogueWords[word];
					}
				}
			}

			return result;
		}

		private Dictionary<string, float> DialogueTextToWords(string dialogueText, TextGenerationSettings textGenSettings) {
			Dictionary<string, float> result = new Dictionary<string, float>();
			dialogueWordList = dialogueText.Split(' ', '\n');

			float wordWidth = 0f;
			foreach (string word in dialogueWordList) {
				TextGenerator textGenerator = new TextGenerator();
				textGenerator.Populate(word, textGenSettings);

				wordWidth = 0f;
				foreach (UICharInfo charInfo in textGenerator.characters) {
					float charWidth = charInfo.charWidth;
					wordWidth += charWidth;
				}

				if (result.ContainsKey(word))
					continue;

				result.Add(word, wordWidth);
			}

			return result;
		}

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
			get { return VNPanelInfo.PANEL_DIALOGUE_READER_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_DIALOGUE_READER; }
		}

		public override System.Action<Rect> OnPanelGUI {
			get { return DialogueReaderWindow; }
		}

		public override void OnPanelEnable(UnityEngine.Events.UnityAction repaint) {
			base.OnPanelEnable(repaint);
		}

		private void DialogueReaderWindow(Rect position) {
			string wordCountText = string.Format(READER_WORD_COUNT, dialogueWordList.Length);
			EditorGUILayout.LabelField(wordCountText, EditorStyles.label);
		}
		# endregion Panel Inspector Abstract
	}
}