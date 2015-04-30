using UnityEngine;
using System.Collections;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility;
using UnityEngine.Events;
using VNToolkit.VNUtility.VNIcon;
using UnityEditor;
using System.Collections.Generic;

namespace VNToolkit.VNEditor {

	public class VNChapterEditor : VNPanelAbstract {

		// Public Variables

		// Private Variables
		public class ChapterDataContainer {
			public bool containerToggle;
			public bool[] elementToggle;
		}

		private ChapterDataContainer[] chapterDataContainer;
		private int selectedContainerIndx = -1;
		private int selectedDataIndx = -1;

		private Texture2D chapterAddIcon;
		private Texture2D chapterMinusIcon;

		private const string CHAPTER_NAME_FORMAT = "Chapter {0}";

		// Static Variables

		# region Panel Editor Abstract
		public override string PanelTitle {
			get { return VNPanelInfo.PANEL_CHAPTER_NAME; }
		}

		public override string PanelControlName {
			get { return VNControlName.FOCUSED_PANEL_CHAPTER; }
		}

		protected override bool IsPanelFoldable {
			get { return true; }
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
			get { return ChapterWindow; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);

			chapterDataContainer = new ChapterDataContainer[0];
			AddDataContainer();

			chapterAddIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_ADD);
			chapterMinusIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_MINUS);
		}

		private void ChapterWindow(Rect position) {
			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);

			EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);
			if (GUILayout.Button(chapterAddIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
				AddData();
			}

			if (GUILayout.Button(chapterMinusIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {

			}

			EditorGUILayout.EndHorizontal();

			if (chapterDataContainer != null) {
				EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);
				for (int i = 0; i < chapterDataContainer.Length; i++) {
					bool toggle = chapterDataContainer[i].containerToggle;
					chapterDataContainer[i].containerToggle = GUILayout.Toggle(
						chapterDataContainer[i].containerToggle,
						(i + 1).ToString(),
						EditorStyles.miniButton,
						GUILayout.Width(25f),
						GUILayout.Height(25f)
					);

					if (chapterDataContainer[i].containerToggle != toggle) {
						if (chapterDataContainer[i].containerToggle) {
							SetContainerIndex(i);
						}
						else {
							selectedContainerIndx = -1;
						}
					}

					if (chapterDataContainer[i].containerToggle && i != selectedContainerIndx) {
						SetContainerIndex(i);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndHorizontal();

			if (chapterDataContainer != null && selectedContainerIndx >= 0) {
				EditorGUILayout.BeginHorizontal(VNConstants.DEFAULT_STYLE_BOX);
				if (chapterDataContainer[selectedContainerIndx].elementToggle != null) {
					for (int i = 0; i < chapterDataContainer[selectedContainerIndx].elementToggle.Length; i++) {
						bool toggle = chapterDataContainer[selectedContainerIndx].elementToggle[i];
						chapterDataContainer[selectedContainerIndx].elementToggle[i] = GUILayout.Toggle(
							chapterDataContainer[selectedContainerIndx].elementToggle[i],
							(i + 1).ToString(),
							EditorStyles.miniButton,
							GUILayout.Width(25f),
							GUILayout.Height(25f)
						);

						if (chapterDataContainer[selectedContainerIndx].elementToggle[i] != toggle) {
							if (chapterDataContainer[selectedContainerIndx].elementToggle[i]) {
								SetDataIndex(i);
							}
							else {
								selectedDataIndx = -1;
							}
						}

						if (chapterDataContainer[selectedContainerIndx].elementToggle[i] && i != selectedDataIndx) {
							SetDataIndex(i);
						}
					}
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		# endregion Panel Editor Abstract

		private void AddDataContainer() {
			ChapterDataContainer tmpData = new ChapterDataContainer();
			tmpData.containerToggle = false;

			bool[] tmpDataToggle = new bool[0];
			ArrayUtility.Add<bool>(ref tmpDataToggle, false);
			tmpData.elementToggle = tmpDataToggle;

			ArrayUtility.Add<ChapterDataContainer>(ref chapterDataContainer, tmpData);

			int containerIndx = chapterDataContainer.Length - 1;
			SetContainerIndex(containerIndx);

			int dataIndx = tmpDataToggle.Length - 1;
			SetDataIndex(dataIndx);
		}

		private void SetContainerIndex(int indx) {
			if (chapterDataContainer == null)
				return;

			for (int i = 0; i < chapterDataContainer.Length; i++) {
				if (i == indx) {
					if (!chapterDataContainer[i].containerToggle)
						chapterDataContainer[i].containerToggle = true;

					selectedContainerIndx = indx;
					continue;
				}

				chapterDataContainer[i].containerToggle = false;
			}
		}

		private void AddData() {
			if (chapterDataContainer == null)
				return;

			if (selectedContainerIndx < 0)
				return;

			int containerIndx = selectedContainerIndx;
			if (chapterDataContainer[containerIndx].elementToggle == null)
				return;

			if (chapterDataContainer[containerIndx].elementToggle.Length >= 10) {
				if (containerIndx >= (chapterDataContainer.Length - 1)) {
					AddDataContainer();
				}
				return;
			}

			ArrayUtility.Add<bool>(ref chapterDataContainer[containerIndx].elementToggle, false);

			if (chapterDataContainer[selectedContainerIndx].elementToggle.Length < 1)
				return;

			int dataIndx = chapterDataContainer[selectedContainerIndx].elementToggle.Length - 1;
			SetDataIndex(dataIndx);
		}

		private void SetDataIndex(int indx) {
			if (chapterDataContainer == null)
				return;

			if (selectedContainerIndx < 0)
				return;

			int containerIndx = selectedContainerIndx;
			if (chapterDataContainer[containerIndx].elementToggle == null)
				return;

			for (int i = 0; i < chapterDataContainer[containerIndx].elementToggle.Length; i++) {
				if (i == indx) {
					if (!chapterDataContainer[containerIndx].elementToggle[i])
						chapterDataContainer[containerIndx].elementToggle[i] = true;

					selectedDataIndx = indx;
					continue;
				}

				chapterDataContainer[containerIndx].elementToggle[i] = false;
			}
		}
	}
}