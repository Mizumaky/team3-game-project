using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

		public enum FocusLayer { Game, UI }

		[SerializeField] public static FocusLayer currentFocusLayer;

}
