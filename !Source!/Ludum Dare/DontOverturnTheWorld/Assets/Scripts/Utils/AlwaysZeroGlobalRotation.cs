using UnityEngine;


namespace Helpers
{
    public sealed class AlwaysZeroGlobalRotation : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
