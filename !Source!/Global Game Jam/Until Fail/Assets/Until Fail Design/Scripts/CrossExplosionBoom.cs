using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossExplosionBoom : MonoBehaviour
{
    public List<ParticleSystem> Explosions;
    public List<DurabilityManager> Tiles;
    
    public void Boom()
    {
        foreach (ParticleSystem ps in Explosions) ps.Play();
        foreach (DurabilityManager dm in Tiles) dm.Break(1000); 
    }
}
