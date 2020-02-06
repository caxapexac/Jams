using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class CameraRefocus
    {
        public Camera CurrentCamera;
        public Vector3 LookAtPoint;
        public Transform Parent;

        private Vector3 originCameraPos;
        private bool refocus;


        public CameraRefocus(Camera camera, Transform parent, Vector3 origCameraPos)
        {
            originCameraPos = origCameraPos;
            CurrentCamera = camera;
            Parent = parent;
        }


        public void ChangeCamera(Camera camera)
        {
            CurrentCamera = camera;
        }


        public void ChangeParent(Transform parent)
        {
            Parent = parent;
        }


        public void GetFocusPoint()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Parent.transform.position + originCameraPos, Parent.transform.forward, out hitInfo,
                                100f))
            {
                LookAtPoint = hitInfo.point;
                refocus = true;
                return;
            }
            refocus = false;
        }


        public void SetFocusPoint()
        {
            if (refocus)
            {
                CurrentCamera.transform.LookAt(LookAtPoint);
            }
        }
    }
}
