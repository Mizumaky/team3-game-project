using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {

  private Canvas canvas;
  [SerializeField] private Image healthSlider;
  private new Transform camera;
  public int timeToHideHealthBar;
  private float time;
  private bool rotatingCanvas;

  // Use this for initialization
  void Start () {
    canvas = GetComponentInChildren<Canvas> ();
    canvas.enabled = false;
    camera = Camera.main.transform;
    rotatingCanvas = false;
  }

  public void AdjustHealthBar (float fillAmount) {
    canvas.enabled = true;
    healthSlider.fillAmount = fillAmount;
    if (!rotatingCanvas) {
      time = timeToHideHealthBar;
      StartCoroutine (RotateCanvas ());
    } else {
      time = timeToHideHealthBar;
    }

  }

  IEnumerator RotateCanvas () {
    float divide = 0.1f;
    while (time >= 0) {
      time -= divide;
      canvas.transform.rotation = camera.rotation;
      yield return new WaitForSeconds (divide);
    }
    canvas.enabled = false;
  }
}