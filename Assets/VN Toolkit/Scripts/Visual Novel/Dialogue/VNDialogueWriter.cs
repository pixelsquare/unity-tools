using UnityEngine;
using System.Collections;
using System.Text;
using UnityEditor;
using VNToolkit.VNUtility;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility.VNCustomInspector;

namespace VNToolkit.VNCore.VNDialogue {

	public class VNDialogueWriter : VNPanelAbstract {

		// Public Variables
		public string WriterOutputText { get { return writerOutputText.ToString(); } }
		public bool WriterIsPlaying { get { return writerIsPlaying; } }

		public delegate void OnWriterEnd();
		public OnWriterEnd onWriterEnd;

		// Private Variables
		private string writerInputText;
		private StringBuilder writerOutputText;
		private bool writerIsPlaying;

		private float writerTimer;
		private float writerDelay;
		private int writerInputIndx;

		private const string WRITER_INPUT_LENGTH = "Input Text Length: {0}";
		private const string WRITER_OUTPUT_LENGTH = "Output Text Length: {0}";
		private const string WRITER_IS_PLAYING = "Is Playing: {0}";
		private const string WRITER_PRINT_TIMER = "Print Timer (ms): {0} ({1})";
		private const string WRITER_PRINT_DELAY = "Print Delay (ms): {0}";
		private const string WRITER_INPUT_INDEX = "Character Index: {0}";

		// Static Variables 

		public void InitializeWriter(string text, float delay) {
			writerInputText = text;
			writerDelay = delay;

			writerOutputText = new StringBuilder();
			writerTimer = 0f;

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
			writerTimer = 0f;

			writerInputIndx = 0;
			writerIsPlaying = false;
		}

		public void UpdateWriter(VNPanelMonoAbstract panel) {
			if (!writerIsPlaying)
				return;

			if (writerInputIndx >= writerInputText.Length) {
				writerIsPlaying = false;

				if (onWriterEnd != null)
					onWriterEnd();

				return;
			}

			if (writerTimer > writerDelay) {
				writerOutputText.Append(writerInputText[writerInputIndx]);
				writerTimer = 0f;
				writerInputIndx++;
			}

			writerTimer += Time.fixedDeltaTime;
			EditorUtility.SetDirty(panel);
		}

		# region Panel Inspector Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_DIALOGUE_WRITER_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_DIALOGUE_WRITER; }
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
			get { return DialogueWriterWindow; }
		}

		public override void OnPanelEnable(UnityEngine.Events.UnityAction repaint) {
			base.OnPanelEnable(repaint);
		}

		private void DialogueWriterWindow(Rect position) {
			string inputTextLength = string.Format(WRITER_INPUT_LENGTH, writerInputText.Length);
			EditorGUILayout.LabelField(inputTextLength, EditorStyles.label);

			string outputTextLength = string.Format(WRITER_OUTPUT_LENGTH, writerOutputText.Length);
			EditorGUILayout.LabelField(outputTextLength, EditorStyles.label);

			string isPlaying = string.Format(WRITER_IS_PLAYING, writerIsPlaying);
			EditorGUILayout.LabelField(isPlaying, EditorStyles.label);

			string printTimer = string.Format(WRITER_PRINT_TIMER, writerTimer, Time.fixedDeltaTime);
			EditorGUILayout.LabelField(printTimer, EditorStyles.label);

			string printDelay = string.Format(WRITER_PRINT_DELAY, writerDelay);
			EditorGUILayout.LabelField(printDelay, EditorStyles.label);

			string inputIndex = string.Format(WRITER_INPUT_INDEX, writerInputIndx);
			EditorGUILayout.LabelField(inputIndex, EditorStyles.label);
		}
		# endregion Panel Inspector Abstract
	}
}