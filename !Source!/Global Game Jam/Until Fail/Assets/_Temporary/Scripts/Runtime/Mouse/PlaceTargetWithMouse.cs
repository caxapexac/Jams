using System;
using UnityEngine;


namespace UnityStandardAssets.SceneUtils
{
    public class PlaceTargetWithMouse : MonoBehaviour
    {
        public float SurfaceOffset = 1.5f;
        public GameObject Prefab;
        public GameObject Parent;

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (Prefab == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return;
            transform.position = hit.point + hit.normal * SurfaceOffset; //?
            Instantiate(Prefab, transform.position, transform.rotation, Parent ? Parent.transform : null);
        }
    }
}
