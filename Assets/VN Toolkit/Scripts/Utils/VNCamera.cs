using UnityEngine;
using System.Collections;

namespace VNToolkit {
	namespace VNUtility {
		using VNCustomInspector;
		using UnityEditor;
		using VNToolkit.VNEditor.VNUtility;
		using UnityEngine.Events;

		[RequireComponent(typeof(Camera))]
		public class VNCamera : VNPanelInspectorAbstract {

			// Public Variables
			public float CameraWidth { get; set; }
			public float CameraHeight { get; set; }
			public float CameraPixelsPerUnit { get; set; }
			public float CameraOrthographicSize { get; set; }
			public bool CameraIsMain { get; set; }

			// Private Variables

			private Camera cam;

			private const string SCREEN_WIDTH_FORMAT = "Width: {0}";
			private const string SCREEN_HEIGHT_FORMAT = "Height: {0}";
			private const string SCREEN_PIXELS_PER_UNIT_FORMAT = "Pixels per Unit: {0}";
			private const string SCREEN_ORTHOGRAPHIC_SIZE = "Orthographic Size: {0}";
			private const string SCREEN_IS_MAIN_CAMERA = "Is Main Camera: {0}";

			# region Panel Inspector Abstract
			public override bool IsPanelFoldable {
				get { return true; }
			}

			public override bool IsRevertButtonEnabled {
				get { return true; }
			}

			public override string PanelTitle {
				get { return VNPanelName.CAMERA_SETTING_NAME; }
			}

			public override System.Action OnInspectorGUI {
				get { return CameraInspectorDraw; }
			}

			public override void OnInspectorEnable(UnityAction repaint) {
				base.OnInspectorEnable(repaint);
				cam = GetComponent<Camera>();
				cam.isOrthoGraphic = true;
				cam.clearFlags = CameraClearFlags.Depth;
				//cam.cullingMask = 1 << LayerMask.NameToLayer(VNConstants.CAMERA_LAYER_MASK);
				cam.nearClipPlane = 0.3f;
				cam.farClipPlane = 2f;

				if (Camera.main != null) {
					CameraIsMain = Camera.main.Equals(cam);
				}
			}

			public void CameraInspectorDraw() {
				EditorGUILayout.BeginHorizontal();
				string widthText = string.Format(SCREEN_WIDTH_FORMAT, CameraWidth.ToString());
				EditorGUILayout.LabelField(widthText, EditorStyles.label, GUILayout.Width(VNConstants.INSPECTOR_LABEL_WIDTH));

				string heightText = string.Format(SCREEN_HEIGHT_FORMAT, CameraHeight.ToString());
				EditorGUILayout.LabelField(heightText, EditorStyles.label, GUILayout.Width(VNConstants.INSPECTOR_LABEL_WIDTH));
				EditorGUILayout.EndHorizontal();

				string pixelsPerUnitText = string.Format(SCREEN_PIXELS_PER_UNIT_FORMAT, CameraPixelsPerUnit.ToString());
				EditorGUILayout.LabelField(pixelsPerUnitText, EditorStyles.label);

				string orthographicSizeText = string.Format(SCREEN_ORTHOGRAPHIC_SIZE, CameraOrthographicSize.ToString());
				EditorGUILayout.LabelField(orthographicSizeText, EditorStyles.label);

				string cameraMainText = string.Format(SCREEN_IS_MAIN_CAMERA, CameraIsMain);
				EditorGUILayout.LabelField(cameraMainText, EditorStyles.label);
			}
			# endregion Panel Inspector Abstract
		}
	}
}
