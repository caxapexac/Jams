using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityManager : MonoBehaviour
{
    public GameObject Block;
    public MeshRenderer Renderer;
    public AudioSource BuildSound;
    public ParticleSystem BuildParticles;
    public AudioSource RepairSound;
    public ParticleSystem RepairParticles;
    public AudioSource BreakSound;
    public ParticleSystem BreakParticles;
    public int MaxDurability = 2;
    public TextMeshProUGUI DurabilityText;

    public int CurrentDurability;

    public void Repair()
    {
        if (Block.activeSelf) return;
        if (God.Instance.Money < God.Instance.Durability) return;
        God.Instance.ChangeMoney(-God.Instance.Durability);
        CurrentDurability = God.Instance.Durability;
        //CurrentDurability = Mathf.Clamp(CurrentDurability + durability, 0, MaxDurability);
        Destroy(Renderer.gameObject);
        Renderer = Instantiate(God.Instance.CurrentTile, transform.position, transform.rotation, Block.transform)
        .GetComponent<MeshRenderer>();
        TextMesh tm = Instantiate(God.Instance.DigitsPrefab, transform.position, transform.rotation, null).GetComponent<TextMesh>();
        tm.text = God.Instance.Durability.ToString();
        tm.color = Color.white;
        if (Block.activeSelf)
        {
            //Repair
            RepairSound.Play();
            RepairParticles.Play();
        }
        else
        {
            //Build
            Block.SetActive(true);
            BuildSound.Play();
            BuildParticles.Play();
        }
        Renderer.materials[1].color = Color.red * (1 - CurrentDurability / (float)MaxDurability);
        DurabilityText.text = CurrentDurability.ToString();
    }

    public void Break(int damage)
    {
        CurrentDurability -= damage;
        if (CurrentDurability <= 0)
        {
            CurrentDurability = 0;
            Block.SetActive(false);
            BreakSound.Play();
            BreakParticles.Play();
            CameraShaker.Instance.Shake();
        }
        DurabilityText.text = CurrentDurability.ToString();
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("AA");
        // CarComponent car = other.GetComponentInParent<CarComponent>();
        // if (car != null)
        // {
        //     Break(1);
        //     car.DamageMade += 1;
        //
        //     //Debug.Log(gameObject.name + " durability: " + durability);
        // }
    }

}
