using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEditor.AnimatedValues;
using UnityEditor;
using VNToolkit.VNEditor.VNUtility;

namespace VNToolkit {
	namespace VNUtility {
		namespace VNCustomInspector {
			public abstract class VNPanelInspectorAbstract : MonoBehaviour, VNIPanelInspector {

				// Public Variables
				public bool PanelActive { get { return panelActive; } }
				public bool PanelEnabled { get { return panelEnabled; } }

				// Private Variables
				private bool panelActive;
				private bool panelEnabled;

				protected AnimBool panelAnim;
				protected UnityAction Repaint { get; set; }

				// Static Variables
				public abstract bool IsPanelFoldable { get; }

				public abstract bool IsRevertButtonEnabled { get; }

				public abstract string PanelTitle { get; }

				public abstract System.Action OnInspectorGUI { get; }

				public virtual void OnInspectorEnable(UnityAction repaint) {
					Repaint = repaint;
					panelEnabled = true;

					//if (!IsPanelFoldable) {
					//    panelEnabled = true;
					//}

					panelAnim = new AnimBool(panelEnabled);
					panelAnim.valueChanged.AddListener(Repaint);

					panelActive = true;
				}

				public void OnInspectorDraw() {
					if (!PanelActive)
						return;

					EditorGUILayout.BeginVertical(VNConstants.DEFAULT_STYLE_BOX);

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
						EditorGUI.indentLevel++;
						if (OnInspectorGUI != null) OnInspectorGUI();
						EditorGUI.indentLevel--;
					}

					EditorGUILayout.EndFadeGroup();

					EditorGUILayout.EndVertical();

					if (!IsRevertButtonEnabled)
						return;

					EditorGUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Revert Changes", GUILayout.Width(110f))) {
						OnInspectorEnable(Repaint);
					}
					EditorGUILayout.EndHorizontal();
				}

				public virtual void PanelOpen() { }

				public virtual void PanelClose() { }

				public void SetPanelActive(bool active) {
					panelActive = active;
				}
			}
		}
	}
}