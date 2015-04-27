using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VNToolkit {
	namespace VNDialogue {
		public class VNDialogueReader {

			// Public Variables

			// Private Variables
			private string[] dialogueWordList;

			public string[] DialogueWordList { get { return dialogueWordList; } }

			public const string DIALOGUE_FORMAT = "{0} ";

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
		}
	}
}