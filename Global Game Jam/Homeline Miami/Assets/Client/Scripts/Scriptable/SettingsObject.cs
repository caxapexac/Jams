using Client.Scripts.MonoBehaviours;
using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Settings")]
    public class SettingsObject : ScriptableObject
    {
        public int PlayerCount;

        public bool NoBlood;

        public bool GodMode;

        public bool IsMouse;
    }
}