using UnityEngine;
using System.Collections;
using System.Text;
using VNToolkit.VNUtility.VNCustomInspector;
using UnityEditor;

namespace VNToolkit {
	namespace VNDialogue {
		public class VNDialogueWriter {

			// Public Variables

			// Private Variables
			private string writerInputText;
			private StringBuilder writerOutputText;
			private bool writerIsPlaying;

			private float writerTime;
			private float writerDelay;
			private int writerInputIndx;

			public string WriterOutputText { get { return writerOutputText.ToString(); } }
			public bool WriterIsPlaying { get { return writerIsPlaying; } }

			// Static Variables 

			public void InitializeWriter(string text, float delay) {
				writerInputText = text;
				writerDelay = delay;

				writerOutputText = new StringBuilder();
				writerTime = 0f;

				writerInputIndx = 0;
				writerIsPlaying = false;
			}

			public void StartWriting() {
				writerIsPlaying = true;
			}

			public void PauseWriting() {
				writerIsPlaying = false;
			}

			public void StopWrinting() {
				writerOutputText = new StringBuilder();
				writerTime = 0f;

				writerInputIndx = 0;
				writerIsPlaying = false;
			}

			public void UpdateWriter(VNPanelInspectorAbstract panel) {
				if (writerInputIndx >= writerInputText.Length || !writerIsPlaying)
					return;

				if (writerTime > writerDelay) {
					writerOutputText.Append(writerInputText[writerInputIndx]);
					writerTime = 0f;
					writerInputIndx++;
				}

				writerTime += Time.fixedDeltaTime;
				EditorUtility.SetDirty(panel);
			}
		}
	}
}