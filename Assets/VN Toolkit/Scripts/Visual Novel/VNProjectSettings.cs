using UnityEngine;
using System.Collections;

namespace VNToolkit {
	public class VNProjectSettings : MonoBehaviour {

		// Public Variables

		private VNProjectSettings instance;
		public VNProjectSettings SharedInstance {
			get { return instance; }
		}

		private void Awake() {
			instance = this;
		}
	}
}