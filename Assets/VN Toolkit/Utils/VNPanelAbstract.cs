using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
using System.Collections.Generic;

namespace VNToolkit {
	namespace VNEditor {
		namespace VNUtility {
			public abstract class VNPanelAbstract : VNIPanel {

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

				// Static Variables

				public abstract bool IsPanelFoldable { get; }

				protected abstract bool IsPanelFlexible { get; }

				protected abstract float PanelWidth { get; }

				public abstract string PanelTitle { get; }

				public abstract string PanelControlName { get; }

				public abstract System.Action<Rect> OnPanelGUI { get; }

				public virtual void OnPanelEnable(UnityAction repaint) {
					Debug.Log("[PANEL] " + PanelTitle + " Initialized!");
					Repaint = repaint;
					panelEnabled = false;

					if (!IsPanelFoldable) {
						panelEnabled = true;
					}

					panelAnim = new AnimBool(panelEnabled);
					panelAnim.valueChanged.AddListener(Repaint);

					parent = null;
					children = new List<VNIPanel>();
					panelActive = true;
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

				public void OnPanelDraw(Rect position) {
					if (!PanelActive || OnPanelGUI == null)
						return;

					// Back panel
					GUI.SetNextControlName(PanelControlName);
					EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX);

					// Front panel
					if (!IsPanelFlexible && PanelWidth > 0) { EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX, GUILayout.Width(PanelWidth)); }
					else { EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX); }

					// Panel Trigger
					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button(PanelTitle, EditorStyles.boldLabel) && IsPanelFoldable) {
						panelEnabled = !panelEnabled;

						if (panelEnabled)	{ PanelOpen(); }
						else				{ PanelClose(); }
					}
					EditorGUILayout.EndHorizontal();

					// Panel
					panelAnim.target = panelEnabled;
					if (EditorGUILayout.BeginFadeGroup(panelAnim.faded)) {
						if (OnPanelGUI != null) OnPanelGUI(position);
					}

					EditorGUILayout.EndFadeGroup();
					EditorGUILayout.EndVertical();

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
	}
}