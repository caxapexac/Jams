using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Man Man;

    void Start()
    {
        Man = GetComponent<Man>();
    }

    public void ClickButton(int type)
    {
        Man.PlayerM.Res[type] -= Man.Costs[type];
        int k;
        switch (type)
        {
            case 0:
                k = (int) (Random.value * 100 + 20);
                for (int i = 0; i < k; i++)
                {
                    Man.DeathM.FixPixel();
                }
                TextColorGen(Man.MousePos, "WOHOOOOOOOooooo", Color.red);
                break;
            case 1:
                Man.HoldTick *= 0.98f;
                Man.SpeedMultipiler *= 1.04f;
                TextColorGen(Man.MousePos + Vector2.right * Random.value + Vector2.down * Random.value, "Speed up ^^^^", Color.cyan);
                break;
            case 2:
                Man.ResourceMultipiler *= 1.08f;
                Man.HoldTime += 0.14f;
                TextColorGen(Man.MousePos + Vector2.right * Random.value + Vector2.down * Random.value, "MORE RESOURCES", Color.blue);
                break;
            case 3:
                Man.IdleMiner += 2;
                Man.HoldTime += 0.14f;
                TextColorGen(Man.MousePos + Vector2.right * Random.value + Vector2.down * Random.value, "JUST COOL OUT", Color.yellow);
                break;
            case 4:
                Man.InventSpeed *= 0.98f;
                Man.HoldTime += 0.14f;
                TextColorGen(Man.MousePos + Vector2.right * Random.value + Vector2.down * Random.value, "MORE PIXELS", Color.magenta);
                break;
            case 5:
                Man.InventMultipiler += 1;
                TextGen(Man.MousePos + Vector2.right * Random.value + Vector2.down * Random.value, "LOTS OF PIXELS");
                break;
        }
        k = (int) (Random.value * 40 + 30);
        for (int i = 0; i < k; i++)
        {
            Man.DeathM.FixPixel();
        }
        if (Man.HoldTime - 0.14f < 1 && Man.HoldTime > 1)
        {
            TextColorGen(Man.MousePos + Vector2.right * Random.value + Vector2.down * Random.value, "AUTOMINER", Color.red);
            TextColorGen(Man.MousePos + Vector2.right * Random.value + Vector2.down * Random.value, "AUTOMINER", Color.red);
            TextColorGen(Man.MousePos + Vector2.right * Random.value + Vector2.down * Random.value, "AUTOMINER", Color.red);
        }
        Man.Costs[type] = (int)(Man.Costs[type] * 1.35f);
        if (Man.PlayerM.Res[type] < Man.Costs[type])
        {
            Man.Sacrifs[type].gameObject.SetActive(false);
        }
    }

    public void Action(BlockManager block)
    {
        if (block.Resource != -1 && Man.Alive)
        {
            int give = Mathf.Clamp((int)(Random.value * 100 * Man.ResourceMultipiler), 0, block.Capacity);
            block.Capacity -= give;
            Man.PlayerM.Res[block.Resource] += give;
            if (Man.PlayerM.Res[block.Resource] >= Man.Costs[block.Resource])
            {
                Man.Sacrifs[block.Resource].gameObject.SetActive(true);
            }
            Man.Inventory[block.Resource].text = Man.PlayerM.Res[block.Resource].ToString();
            TextGen(block.transform.localPosition, "+" + give + " " +((Resource)(block.Resource)).ToString());
            if (block.Capacity <= 0)
            {
                block.Resource = -1;
                block.IsMining = false;
                block.Render();
            }
        }
    }

    public void TextColorGen(Vector2 pos, string text, Color color)
    {
        GameObject go = Instantiate(Man.TextPrefab);
        go.transform.localPosition = pos + Vector2.up;
        go.GetComponent<TextMesh>().text = text;
        go.GetComponent<TextMesh>().color = color;
        go.SetActive(true);
    }
    
    public void TextGen(Vector2 pos, string text)
    {
        GameObject go = Instantiate(Man.TextPrefab);
        go.transform.localPosition = pos + Vector2.up;
        go.GetComponent<TextMesh>().text = text;
        go.SetActive(true);
    }
}
