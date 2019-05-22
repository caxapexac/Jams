using UnityEngine;
using UnityEngine.UI;


public class FpsCounter : MonoBehaviour
{
    public int FrameRate;
    public Text Text;

    public void Update()
    {
        float current = (int)(1f / Time.unscaledDeltaTime);
        FrameRate = (int)current;
        Text.text = FrameRate + " FPS";
    }
}