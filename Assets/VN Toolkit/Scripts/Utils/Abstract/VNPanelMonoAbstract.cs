using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine.Events;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using VNToolkit.VNUtility.VNIcon;
using VNToolkit.VNEditor.VNUtility;


namespace VNToolkit.VNUtility.VNCustomInspector {

	public abstract class VNPanelMonoAbstract : MonoBehaviour, VNIPanel {

		// Public Variables
		public VNIPanel parent { get; set; }
		public List<VNIPanel> children { get; set; }

		public bool PanelActive { get { return panelActive; } }
		public bool PanelEnabled { get { return panelEnabled; } }
		public VN_PANELSTATE PanelState { get { return panelState; } }

		// Private Variables
		private bool panelActive;
		private bool panelEnabled;
		private VN_PANELSTATE panelState;

		protected AnimBool panelAnim;
		protected UnityAction Repaint { get; set; }

		private Texture2D panelRefreshIcon;

		// Static Variables

		public abstract string PanelTitle { get; }

		public abstract string PanelControlName { get; }

		protected abstract bool IsPanelFoldable { get; }

		protected abstract bool IsPanelFlexible { get; }

		protected abstract bool IsRefreshable { get; }

		protected abstract float PanelWidth { get; }

		protected abstract System.Action OnPanelGUI { get; }

		protected abstract bool IsRevertButtonEnabled { get; }

		public virtual void OnPanelEnable(UnityAction repaint) {
			Repaint = repaint;
			panelEnabled = true;

			panelAnim = new AnimBool(panelEnabled);
			panelAnim.valueChanged.AddListener(Repaint);

			parent = null;
			children = new List<VNIPanel>();
			panelActive = true;

			panelRefreshIcon = VNIconDatabase.SharedInstance.GetIcon(VNIconName.ICON_REFRESH_1);
		}

		protected virtual void PanelOpen() {
			Debug.Log("[PANEL] " + PanelTitle + " Open!");
			panelEnabled = true;
		}

		protected virtual void PanelClose() {
			Debug.Log("[PANEL] " + PanelTitle + " Close!");
			panelEnabled = false;
		}

		protected virtual void PanelSave() { Debug.Log("[PANEL] " + PanelTitle + " Save!"); }

		protected virtual void PanelLoad() { Debug.Log("[PANEL] " + PanelTitle + " Load!"); }

		protected virtual void PanelClear() { Debug.Log("[PANEL] " + PanelTitle + " Clear!"); }

		protected virtual void PanelReset() { Debug.Log("[PANEL] " + PanelTitle + " Reset!"); }

		protected virtual void PanelRefresh() {
			Debug.Log("[PANEL] " + PanelTitle + " Refresh!");
			EditorUtility.SetDirty(this);
		}

		public void OnPanelDraw() {
			if (!PanelActive || OnPanelGUI == null)
				return;

			GUI.SetNextControlName(PanelControlName);
			if (!IsPanelFlexible && PanelWidth > 0) { EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX, GUILayout.Width(PanelWidth)); }
			else { EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX); }

			// Panel Trigger
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(PanelTitle, EditorStyles.boldLabel) && IsPanelFoldable) {
				panelEnabled = !panelEnabled;

				if (panelEnabled) { PanelOpen(); }
				else { PanelClose(); }
			}

			if (IsRefreshable && panelEnabled) {
				Color originalColor = GUI.color;
				GUI.color = Color.green;

				GUILayout.FlexibleSpace();
				if (GUILayout.Button(panelRefreshIcon, EditorStyles.miniButton, GUILayout.Width(22f), GUILayout.Height(22f))) {
					PanelRefresh();
				}

				GUI.color = originalColor;
			}
			EditorGUILayout.EndHorizontal();

			// Panel
			panelAnim.target = panelEnabled;
			if (EditorGUILayout.BeginFadeGroup(panelAnim.faded)) {
				EditorGUI.indentLevel++;
				if (OnPanelGUI != null) OnPanelGUI();
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.EndVertical();
		}

		public void SetPanelActive(bool active) {
			panelActive = active;
		}

		public void SetPanelState(VN_PANELSTATE state) {
			panelState = state;

			if (panelState == VN_PANELSTATE.OPEN) { PanelOpen(); }
			else if (panelState == VN_PANELSTATE.CLOSE) { PanelClose(); }
			else if (panelState == VN_PANELSTATE.SAVE) { PanelSave(); }
			else if (panelState == VN_PANELSTATE.LOAD) { PanelLoad(); }
			else if (panelState == VN_PANELSTATE.CLEAR) { PanelClear(); }
			else if (panelState == VN_PANELSTATE.RESET) { PanelReset(); }
		}

		public void AddChildren(VNIPanel child) {
			if (child == null || children == null)
				return;

			if (children.Contains(child))
				return;

			child.parent = this;
			children.Add(child);
		}

		public List<VNIPanel> GetChildren() {
			return children;
		}

		public VNIPanel GetChild(string title) {
			return children.Find(c => c.PanelTitle == title);
		}
	}
}