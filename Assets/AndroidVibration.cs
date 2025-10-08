using UnityEngine;

public class AndroidVibration : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static AndroidVibration Instance;

    private void Start()
    {
        Instance = this;
    }
    public void VibrateNow()
    {
        Handheld.Vibrate();
        Debug.Log("Vibration triggered!");
    }
}
