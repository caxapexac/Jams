using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public sealed class Earth : MonoBehaviour
{
    [Header("Settings")]
    public float DampingForce = 2;
    public float LerpForce = 2;
    public float Radius = 5;
    public Sprite[] FaceSprites;
    public SpriteRenderer FaceRenderer;

    [Header("References")]
    public AudioSource HumanDown;
    public AudioSource BuildingDown;

    [Header("Read Only")]
    public float CurrentForce;
    public ContactFilter2D BuildingContactFilter;

    // public float CurrentRotationForce = 0;
    // public float CurrentMotionForce = 0;
    // [Range(0, 1)]
    // public float RotationFriction = 0.01f;
    // [Range(0, 1)]
    // public float MotionFriction = 0.01f;
    // public float RotationDamping = 1;

    private readonly List<Mass> _masses = new List<Mass>();
    private World _world;

    private void Start()
    {
        _world = GetComponentInParent<World>();
        BuildingContactFilter = new ContactFilter2D();
        BuildingContactFilter.SetLayerMask(LayerMask.GetMask("Building"));
    }

    private void Update()
    {
        // Vector2 center = transform.position;
        // Vector2 normal = transform.up;
        // Vector2 normalRight = transform.right;
        // float sumRotation = 0;
        // foreach (Mass mass in _masses)
        // {
        //     if (Mathf.Abs(mass.transform.localPosition.y) > 0.01f)
        //         continue;
        //     sumRotation -= mass.transform.localPosition.x * mass.Weight;
        // }
        // CurrentRotationForce -= CurrentRotationForce * RotationFriction * Time.deltaTime;
        // CurrentRotationForce += sumRotation * Time.deltaTime;
        // transform.Rotate(0, 0, CurrentRotationForce * Time.deltaTime);
        //
        // // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), RotationDamping * Time.deltaTime);
        //

        float sumRotation = 0;
        foreach (Mass mass in _masses)
        {
            if (mass == null)
                continue;

            if (mass.transform.parent != mass.Side.transform)
            {
                if (Mathf.Abs(mass.transform.parent.localPosition.y) > 0.01f)
                    continue;

                sumRotation -= mass.transform.parent.localPosition.x * mass.Weight;
            }
            else
            {
                if (Mathf.Abs(mass.transform.localPosition.y) > 0.01f)
                    continue;

                sumRotation -= mass.transform.localPosition.x * mass.Weight;
            }
        }

        if (!_world.IsGameOver)
        {
            float force = Mathf.Pow(Mathf.Abs(sumRotation), 1 / DampingForce);
            if (sumRotation < 0)
                force *= -1;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, force), LerpForce * Time.deltaTime);
            float dot = Vector3.Dot(transform.up, Vector3.up);
            if (dot < 0.8f)
            {
                FaceRenderer.sprite = FaceSprites[2];
            }
            else if (dot < 0.95f)
            {
                FaceRenderer.sprite = FaceSprites[1];
            }
            else
            {
                FaceRenderer.sprite = FaceSprites[0];
            }
        }
    }

    public void Register(Mass mass)
    {
        _masses.Add(mass);
    }

    public void Unregister(Mass mass)
    {
        _masses.Add(mass);
    }
}
