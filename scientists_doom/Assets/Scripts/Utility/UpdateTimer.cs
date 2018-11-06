/// <summary>
/// Timer that uses the Unity Update method to run
/// </summary>
public class CustomUpdateTimer {

  private bool running;
  private float initialTime;
  private float timeLeft;

  /// <summary>
  /// Substracts the tick length from the remaining time and checks if there's any time left
  /// </summary>
  /// <param name="tickLength"></param>
  /// <returns></returns>
  public bool isLastTick (float tickLength) {
    timeLeft -= tickLength;
    if (timeLeft > 0) {
      return false;
    }
    return true;
  }

  /// <summary>
  /// Returns a value from 0 to 1 describing how much time left the timer has
  /// </summary>
  /// <returns></returns>
  public float GetTimeLeftPercent () {
    return timeLeft / initialTime;
  }

  /// <summary>
  /// Creates a new timer instance
  /// </summary>
  /// <param name="time">time to set the timer for</param>
  public CustomUpdateTimer (float time) {
    running = false;
    initialTime = time;
    timeLeft = initialTime;
  }

}