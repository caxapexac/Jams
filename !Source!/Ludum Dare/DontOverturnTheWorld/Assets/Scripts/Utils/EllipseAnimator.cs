using System;
using UnityEngine;


namespace Helpers
{
    public sealed class EllipseAnimator : MonoBehaviour
    {
        [Serializable]
        public struct AnimatedObject
        {
            public float Offset;
            public Transform Transform;
        }


        public float _verticalRadius;
        public float _horizontalRadius;

        [SerializeField]
        private AnimatedObject[] _objects;

        public float Angle
        {
            set
            {
                for (int i = 0; i < _objects.Length; i++)
                {
                    ref AnimatedObject obj = ref _objects[i];
                    Transform transform = obj.Transform;
                    if (transform == null)
                        continue;
                    float angle = (value + obj.Offset) * Mathf.Deg2Rad;
                    transform.localPosition = ElipsePosition(angle);
                }
            }
        }

        private Vector2 ElipsePosition(float radians)
        {
            return new Vector2()
            {
                x = _horizontalRadius * Mathf.Cos(radians),
                y = _verticalRadius * Mathf.Sin(radians),
            };
        }

        private void OnDrawGizmosSelected()
        {
            int points = 40;
            float step = (Mathf.PI * 2) / points;

            Gizmos.color = Color.gray;

            for (int i = 0; i < points; i++)
            {
                Vector2 start = ElipsePosition(i * step);
                Vector2 end = ElipsePosition(i * step + step);
                Gizmos.DrawLine(start, end);
            }

            Angle = 0; // Update
        }
    }
}
