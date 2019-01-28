using UnityEngine;
using UnityEngine.UI;
 

namespace Client.Scripts.Algorithms.Legacy
{

    public class FpsDisplay : MonoBehaviour
    {
        public int FrameRate;
        public Text Text;
 
        public void Update ()
        {
            float current = (int)(1f / Time.unscaledDeltaTime);
            FrameRate = (int)current;
            Text.text = FrameRate + " FPS";
        }
    }
}