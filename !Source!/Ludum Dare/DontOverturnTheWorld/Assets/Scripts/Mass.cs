using UnityEngine;


public sealed class Mass : MonoBehaviour
{
    public float Weight = 1;
    public float Gravity = 2;
    public bool WannaBonk = false;

    private Earth _earth;
    public Side Side;

    private void Start()
    {
        _earth = GetComponentInParent<Earth>();
        _earth.Register(this);
        Side = GetComponentInParent<Side>();
    }

    private void Update()
    {
        if (Mathf.Abs(transform.localPosition.y) < 0.2f
            && Mathf.Abs(transform.localPosition.x) < _earth.Radius)
        {
            Vector3 groundPosition = transform.localPosition;
            groundPosition.y = 0;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, groundPosition, Time.deltaTime * Gravity);
            if (WannaBonk)
            {
                WannaBonk = false;
                _earth.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            transform.Translate(Vector2.down * Gravity * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        _earth.Unregister(this);
    }
}
