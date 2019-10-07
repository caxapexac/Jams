//I - item

//S

using UnityEngine;

public enum Structures
{
    NULL,
    Altar,
    SMiner,
    MMiner,
    LMiner,
    Tree,
    TreeCutter,
    
}

//R
public enum Resource
{
    Coal,
    Iron,
    Gold,
    Rocks,
    Uran,
    Wood
}

//B
public enum Backs
{
    NULL,
    Swamp,
    Water,
    Sand,
    Ground,
    Grass,
    Stone,

}

public class Utils
{
    
}

static class RotateExtension
{
    public static void LookAt2D(this Transform me, Vector2 target)
    {
        Vector2 dir = target - (Vector2)me.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        me.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static void LookAt2D(this Transform me, Transform target)
    {
        me.LookAt2D(target.position);
    }

    public static void LookAt2D(this Transform me, GameObject target)
    {
        me.LookAt2D(target.transform.position);
    }
}
