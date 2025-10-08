using UnityEngine;
using System;
using System.IO;
using System.Collections;


public class ScreenshotToDesktop : MonoBehaviour
{
    void Update()
    {
        // Press SPACE to capture
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TakeScreenshot());
        }
    }

    IEnumerator TakeScreenshot()
    {
        //  Wait until rendering is done
        yield return new WaitForEndOfFrame();

        // Get desktop path
        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        // Make a simple file name
        string fileName = "Screenshot_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string path = Path.Combine(desktop, fileName);

        // Capture the screen
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // Save as PNG
        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        // Clean up
        Destroy(screenshot);

        // ✅ Log the path
        Debug.Log("📸 Screenshot saved: " + path);
    }
}