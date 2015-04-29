using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;
using VNToolkit.VNEditor.VNUtility;
using VNToolkit.VNUtility.VNCustomInspector;

namespace VNToolkit.VNUtility {

	[RequireComponent(typeof(Camera))]
	public class VNCamera : VNPanelMonoAbstract {

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

		// Static Variables

		# region Panel Inspector Abstract
		protected override bool IsPanelFoldable {
			get { return true; }
		}

		protected override bool IsPanelFlexible {
			get { return true; }
		}

		protected override float PanelWidth {
			get { return 0f; }
		}

		public override string PanelTitle {
			get { return VNPanelInfo.CAMERA_SETTING_NAME; }
		}

		public override string PanelControlName {
			get { return string.Empty; }
		}

		protected override System.Action OnPanelGUI {
			get { return CameraInspectorDraw; }
		}

		protected override bool IsRevertButtonEnabled {
			get { return true; }
		}

		public override void OnPanelEnable(UnityAction repaint) {
			base.OnPanelEnable(repaint);
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
			string widthText = string.Format(SCREEN_WIDTH_FORMAT, CameraWidth);
			EditorGUILayout.LabelField(widthText, EditorStyles.label, GUILayout.Width(VNConstants.INSPECTOR_LABEL_WIDTH));

			string heightText = string.Format(SCREEN_HEIGHT_FORMAT, CameraHeight);
			EditorGUILayout.LabelField(heightText, EditorStyles.label, GUILayout.Width(VNConstants.INSPECTOR_LABEL_WIDTH));
			EditorGUILayout.EndHorizontal();

			string pixelsPerUnitText = string.Format(SCREEN_PIXELS_PER_UNIT_FORMAT, CameraPixelsPerUnit);
			EditorGUILayout.LabelField(pixelsPerUnitText, EditorStyles.label);

			string orthographicSizeText = string.Format(SCREEN_ORTHOGRAPHIC_SIZE, CameraOrthographicSize);
			EditorGUILayout.LabelField(orthographicSizeText, EditorStyles.label);

			string cameraMainText = string.Format(SCREEN_IS_MAIN_CAMERA, CameraIsMain);
			EditorGUILayout.LabelField(cameraMainText, EditorStyles.label);
		}
		# endregion Panel Inspector Abstract
	}
}