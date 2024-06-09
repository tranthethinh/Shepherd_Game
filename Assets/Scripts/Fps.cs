using UnityEngine;
using System.Collections;

public class Fps : MonoBehaviour
{
    private float count;

    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnGUI()
    {
        // Set the style for the label
        GUIStyle textStyle = new GUIStyle(GUI.skin.label);
        textStyle.fontSize = 20; // Set font size to 20
        textStyle.normal.textColor = Color.red; // Set text color to red

        // Display the FPS and round to 2 decimals
        GUI.Label(new Rect(50, 50, 100, 25), count.ToString("F2") + "FPS", textStyle);
    }

}