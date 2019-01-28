using Client.Scripts.Scriptable;
using UnityEngine;


namespace Client.Scripts.MonoBehaviours
{
    public class GunPickable : MonoBehaviour
    {
        public GunObject Gun;

        public void Enable()
        {
            GetComponent<SpriteRenderer>().sprite = Gun.DropTexture;
        }
    }
}
