using System.Collections;
using LeopotamGroup.Pooling;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [HideInInspector]
    public Man Man;
    
    public int Movable;
    
    public int Structure;
    
    public int Resource;

    public int Capacity;
    
    public int Back;
    
    public float Depth;

    public SpriteRenderer BSprite;
    public SpriteRenderer RSprite;
    public SpriteRenderer SSprite;
    private Color _lastColor;
    public bool IsMining;
    private float _tick;
    private int _times;
    
    private void Start()
    {
        IsMining = false;
        _tick = 1000;
        _times = 0;
        _lastColor = Color.white * Mathf.Sqrt(Depth);
        BSprite.color = _lastColor;
        Render();
    }

    private void Update()
    {
        if (IsMining)
        {
            RSprite.color = new Color(1, 0.95f, 0.95f, _tick / Man.HoldTick / 2 + 0.5f);
            _tick += Time.deltaTime;
            if (_tick >= Man.HoldTick)
            {
                _times++;
                Man.MouseM.Action(this);
                _tick = 0;
                if (_times >= Man.HoldTime)
                {
                    //_isMining = false;
                }
            }
        }
    }

    private void OnEnable()
    {
        StartCoroutine(UnRender());
    }

    public void Render()
    {
        BSprite.sprite = Man.WorldM.BSprites[Back];
        if (Resource != -1)
        {
            RSprite.sprite = Man.WorldM.RSprites[Resource];
        }
        else
        {
            RSprite.sprite = null;
        }
        if (Structure != -1)
        {
            //SSprite.sprite = Man.WorldM.SSprites[Structure];
        }
        else
        {
            //SSprite.sprite = null;
        }
    }
    
    private IEnumerator UnRender()
    {
        while (Mathf.Abs(Man.PlayerM.Player.localPosition.x - transform.localPosition.x) < Man.DistanceWMax
               && Mathf.Abs(Man.PlayerM.Player.localPosition.y - transform.localPosition.y) < Man.DistanceHMax
               || IsMining)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (Vector2.Distance(transform.localPosition, Man.PlayerM.Player.position) < 3 && Man.Alive && Capacity > 0)
        {
            _times = 0;
            IsMining = true;
            Man.MousePos = transform.localPosition;
            Man.Res.SetActive(true);
            Man.ResTransform.anchoredPosition = Camera.main.WorldToScreenPoint(transform.localPosition);
            if (Resource != -1)
            {
                Man.TextRes.text = Capacity + " " + ((Resource)(Resource)).ToString();
            }
            else
            {
                Man.TextRes.text = "Empty";
            }

            
            _lastColor = BSprite.color;
            BSprite.color = Color.green;
        }
        else
        {
            Man.Res.SetActive(false);
        }
    }

    void OnMouseExit()
    {
        if (BSprite.color == Color.green)
        {
            BSprite.color = _lastColor;
        }
    }

    void OnMouseDown()
    {
        if (Vector2.Distance(transform.localPosition, Man.PlayerM.Player.position) < 3 && Man.Alive)
        {
            GameObject go = Instantiate(Man.ParticleBreak);
            go.transform.localPosition = transform.localPosition;
            Man.MouseM.Action(this);
        }
        if (Resource != -1)
        {
            Man.TextRes.text = Capacity + " " + ((Resource)(Resource)).ToString();
        }
        else
        {
            Man.TextRes.text = "Empty";
        }
    }
}
