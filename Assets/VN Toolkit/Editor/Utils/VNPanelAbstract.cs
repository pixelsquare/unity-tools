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
				//public Dictionary<string, VNIPanel> children { get; set; }

				public bool PanelActive { get { return panelActive; } }
				public bool PanelEnabled { get { return panelEnabled; } }

				// Private Variables
				private bool panelActive;
				private bool panelEnabled;
				private bool panelFlexibleWidth;

				protected AnimBool panelAnim;
				protected UnityAction Repaint { get; set; }

				// Static Variables

				public abstract bool IsPanelFoldable { get; }

				public abstract string PanelTitle { get; }

				public abstract float PanelWidth { get; }

				public abstract string PanelControlName { get; }

				public abstract System.Action<Rect> OnEditorGUI { get; }

				public virtual void OnEditorEnable(UnityAction repaint) {
					Debug.Log("[PANEL] " + PanelTitle + " Initialized!");
					Repaint = repaint;
					panelEnabled = false;

					if (!IsPanelFoldable) {
						panelEnabled = true;
					}

					panelAnim = new AnimBool(panelEnabled);
					panelAnim.valueChanged.AddListener(Repaint);

					panelFlexibleWidth = PanelWidth < 0;
					parent = null;
					children = new List<VNIPanel>();
					panelActive = true;
				}

				public virtual void PanelOpen() {
					Debug.Log("[PANEL] " + PanelTitle + " Open!");
					panelEnabled = true;
				}

				public virtual void PanelClose() {
					Debug.Log("[PANEL] " + PanelTitle + " Close!");
					panelEnabled = false;
				}

				public virtual void PanelSave() { Debug.Log("[PANEL] " + PanelTitle + " Save!"); }

				public virtual void PanelLoad() { Debug.Log("[PANEL] " + PanelTitle + " Load!"); }

				public virtual void PanelClear() { Debug.Log("[PANEL] " + PanelTitle + " Clear!"); }

				public virtual void PanelReset() { Debug.Log("[PANEL] " + PanelTitle + " Reset!"); }

				public void OnEditorDraw(Rect position) {
					if (!PanelActive)
						return;

					// Back panel
					GUI.SetNextControlName(PanelControlName);
					EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX);

					// Front panel
					if (panelFlexibleWidth) { EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX);	}
					else { EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX, GUILayout.Width(PanelWidth)); }

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
						if (OnEditorGUI != null) OnEditorGUI(position);
					}

					EditorGUILayout.EndFadeGroup();
					EditorGUILayout.EndVertical();

					EditorGUILayout.EndVertical();
				}

				public void SetPanelActive(bool active) {
					panelActive = active;
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