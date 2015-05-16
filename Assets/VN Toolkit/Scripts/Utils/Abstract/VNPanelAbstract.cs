using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;
using VNToolkit.VNUtility;
using VNToolkit.VNUtility.VNIcon;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;

namespace VNToolkit.VNEditor.VNUtility {

	[System.Serializable]
	public abstract class VNPanelAbstract : ScriptableObject, VNIPanel {

		// Public Variables
		[SerializeField] public VNIPanel parent { get; set; }
		[SerializeField] public List<VNIPanel> children { get; set; }

		public bool PanelActive { get { return panelActive; } }
		public bool PanelEnabled { get { return panelEnabled; } }
		public VN_PANELSTATE PanelState { get { return panelState; } }

		// Private Variables
		private bool panelActive;
		private bool panelEnabled;
		private VN_PANELSTATE panelState;
		private Vector2 panelVerticalScroll;

		protected Event e;
		protected AnimBool panelAnim;
		protected UnityAction Repaint { get; set; }

		private Texture2D panelRefreshIcon;

		// Static Variables
		public virtual void OnEnable() {
			hideFlags = HideFlags.HideAndDontSave;

			if (children == null)
				children = new List<VNIPanel>();
		}

		public virtual void OnDisable() { }

		public abstract string PanelTitle { get; }

		public abstract string PanelControlName { get; }

		protected abstract bool IsPanelFoldable { get; }

		protected abstract bool IsPanelFlexible { get; }

		protected abstract bool IsRefreshable { get; }

		protected abstract bool IsScrollable { get; }

		protected abstract float PanelWidth { get; }

		protected abstract System.Action<Rect> OnPanelGUI { get; }

		public virtual void OnPanelEnable(UnityAction repaint) {
			Debug.Log("[PANEL] " + PanelTitle + " Initialized!");
			Repaint = repaint;
			panelEnabled = false;

			if (!IsPanelFoldable)
				panelEnabled = true;

			if (panelAnim == null)
				panelAnim = new AnimBool(panelEnabled);

			panelAnim.valueChanged.AddListener(Repaint);

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

		protected virtual void PanelSave() {
			if (!VNFileManager.IsPathValid()) {
				Debug.LogWarning("[PANEL] " + PanelTitle + " Path invalid!");
				return;
			}

			Debug.Log("[PANEL] " + PanelTitle + " Save!"); 
		}

		protected virtual void PanelLoad() {
			if (!VNFileManager.IsPathValid()) {
				Debug.LogWarning("[PANEL] " + PanelTitle + " Path invalid!"); 
				return;
			}

			Debug.Log("[PANEL] " + PanelTitle + " Load!"); 
		}

		protected virtual void PanelClear() { Debug.Log("[PANEL] " + PanelTitle + " Clear!"); }

		protected virtual void PanelReset() { Debug.Log("[PANEL] " + PanelTitle + " Reset!"); }

		protected virtual void PanelRefresh() {
			Debug.Log("[PANEL] " + PanelTitle + " Refresh!");
			Repaint();
		}

		public void OnPanelDraw(Rect position) {
			if (!PanelActive || OnPanelGUI == null)
				return;

			e = Event.current;
			if ((e.button == 0 && e.type == EventType.mouseDown) || e.keyCode == KeyCode.Return) {
				GUI.FocusControl(PanelControlName);
			}

			if (IsScrollable && ShowScrollView()) {
				panelVerticalScroll = EditorGUILayout.BeginScrollView(
					panelVerticalScroll, 
					false, 
					false, 
					GUILayout.Height(position.height - VNConstants.FOOTER_HEIGHT),
					GUILayout.ExpandHeight(false)
				);
			}

			GUI.SetNextControlName(PanelControlName);
			if (!IsPanelFlexible && PanelWidth > 0) { EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX, GUILayout.Width(PanelWidth)); }
			else { EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX); }

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
				if (GUILayout.Button(panelRefreshIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
					PanelRefresh();
				}

				GUI.color = originalColor;
			}
			EditorGUILayout.EndHorizontal();

			panelAnim.target = panelEnabled;
			if (EditorGUILayout.BeginFadeGroup(panelAnim.faded)) {
				if (OnPanelGUI != null) OnPanelGUI(position);
			}
			EditorGUILayout.EndFadeGroup();
			EditorGUILayout.EndVertical();

			if (IsScrollable && ShowScrollView()) {
				EditorGUILayout.EndScrollView();
			}
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
			else if (panelState == VN_PANELSTATE.REFRESH) { PanelRefresh(); }
		}

		public void AddChildren(VNIPanel child) {
			if (children == null)
				children = new List<VNIPanel>();

			if (child == null)
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

		public bool ShowScrollView() {
			int counter = 0;
			foreach (VNPanelAbstract child in children) {
				if (child.panelAnim.isAnimating) {
					counter++;
				}
			}

			return counter == 0;
		}
	}
}