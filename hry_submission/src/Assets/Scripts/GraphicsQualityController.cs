using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GraphicsQualityController : MonoBehaviour
{

  [Header("Graphics Quality")]
  public GraphicsQualityPreset defaultPreset = GraphicsQualityPreset.Medium;
  public enum GraphicsQualityPreset { Low, Medium, High }

  [Header("PostProcessing")]
  public PostProcessVolume volume;
  public PostProcessProfile low;
  public PostProcessProfile med;
  public PostProcessProfile high;

  private void Awake()
  {
    SetQualityPresetIndex((int)defaultPreset);
  }

  private void Start()
  {
    UpdateQuality();
  }

  /// <summary>
  /// Sets graphics quality preset index
  /// </summary>
  /// <param name="value">Graphics preset index (0 - low, 1 - high)</param>
  public void SetQualityPresetIndex(int value)
  {
    defaultPreset = (GraphicsQualityPreset)value;
  }

  /// <summary>
  /// Updates current graphics quality and post processing preset
  /// </summary>
  public void UpdateQuality()
  {
    switch ((int)defaultPreset)
    {
      case 0:
        volume.profile = low;
        QualitySettings.SetQualityLevel(0);
        break;
      case 1:
        volume.profile = med;
        QualitySettings.SetQualityLevel(1);
        break;
      default:
        volume.profile = high;
        QualitySettings.SetQualityLevel(2);
        break;
    }
  }
}