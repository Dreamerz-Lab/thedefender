using UnityEngine;
using UnityEngine.UI;
using Defender.Core;

namespace Defender.UI {
	public class UIManager : MonoBehaviour {
		public static UIManager instace;

		//The Heavy Shield UI Count and Timer
		public Image HeavyShieldCount_Img;


		private void Awake() {
			instace = this;
		}

		private void Start() {
			
		}
	}
}