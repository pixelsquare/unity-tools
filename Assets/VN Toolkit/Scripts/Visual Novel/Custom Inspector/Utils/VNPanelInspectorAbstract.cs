using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEditor.AnimatedValues;
using UnityEditor;
using VNToolkit.VNEditor.VNUtility;
using System.Collections.Generic;
using System.IO;

namespace VNToolkit {
	namespace VNUtility {
		namespace VNCustomInspector {
			public abstract class VNPanelInspectorAbstract : MonoBehaviour, VNIPanel {

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

				private Texture2D panelRevertIcon;
				private Texture2D panelRefreshIcon;

				// Static Variables
				protected abstract bool IsPanelFoldable { get; }

				protected abstract bool IsPanelFlexible { get; }

				protected abstract float PanelWidth { get; }

				public abstract string PanelTitle { get; }

				public abstract string PanelControlName { get; }

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

					panelRevertIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/left149.png", typeof(Texture2D)) as Texture2D;
					panelRefreshIcon = AssetDatabase.LoadAssetAtPath("Assets/VN Toolkit/Icons/arrows91.png", typeof(Texture2D)) as Texture2D;
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

					GUILayout.FlexibleSpace();
					if (GUILayout.Button(panelRevertIcon, GUILayout.Width(22f), GUILayout.Height(22f)) && !IsRevertButtonEnabled) {
						PanelRevert();
					}

					if (GUILayout.Button(panelRefreshIcon, GUILayout.Width(22f), GUILayout.Height(22f))) {
						PanelRefresh();
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

				private void PanelRevert() {
					OnPanelEnable(Repaint);
					PanelRefresh();
				}

				private void PanelRefresh() {
					EditorUtility.SetDirty(this);
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