using System;
using System.Collections;
using System.Collections.Generic;
using LeopotamGroup.Collections;
using LeopotamGroup.Math;
using LeopotamGroup.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldManager : MonoBehaviour
{
	[HideInInspector]
	public Man Man;
	//public PoolContainer BlockPool;
	public Sprite[] BSprites;
	public Sprite[] RSprites;
	public Sprite[] SSprites;
	public Sprite[] MSprites;
	
	private BlockManager[,] _map;
	private float[,] _noise;
	private float _tick;
	
	void Start ()
	{
		Man = GetComponent<Man>();
		//BlockPool = PoolContainer.CreatePool("Prefabs/Block");
		_noise = FractalNoise.Get(Man.Saeed, Man.WorldSize, Man.Octaves, Man.Persistance);
		_map = new BlockManager[Man.WorldSize, Man.WorldSize];
		_tick = Man.RenderTick;
		GenerateRes((int)Resource.Coal, Man.CoalSize, (int)(Man.WorldSize * Man.WorldSize * Man.Coal));
		GenerateRes((int)Resource.Iron, Man.IronSize, (int)(Man.WorldSize * Man.WorldSize * Man.Iron));
		GenerateRes((int)Resource.Gold, Man.GoldSize, (int)(Man.WorldSize * Man.WorldSize * Man.Gold));
		GenerateRes((int)Resource.Rocks, Man.RocksSize, (int)(Man.WorldSize * Man.WorldSize * Man.Rocks));
		GenerateRes((int)Resource.Uran, Man.UranSize, (int)(Man.WorldSize * Man.WorldSize * Man.Uran));
		GenerateRes((int)Resource.Wood, Man.WoodSize, (int)(Man.WorldSize * Man.WorldSize * Man.Wood));
	}

	private void GenerateRes(int type, int size, int count)
	{
		//Debug.Log(type + " " + size + " " + count);
		Random.InitState(Man.Saeed);
		for (int i = 0; i < count; i++)
		{
			int x = (int) (Random.value * Man.WorldSize);
			int y = (int) (Random.value * Man.WorldSize);
			GenerateRecurcively(x, y, type, size);
		}
	}

	private void GenerateRecurcively(int x, int y, int type, int size)
	{
		BlockManager block = Get(x, y);
		if (block.Back > 1 && block.Resource == -1)
		{
			block.Resource = type;
			block.Capacity = (int)(size * Random.value * 100 + 20);
			block.Render();
			if (Random.value < 0.1f * size)
				GenerateRecurcively(x+1, y, type, size - 1);
			if (Random.value < 0.1f * size)
				GenerateRecurcively(x-1, y, type, size - 1);
			if (Random.value < 0.1f * size)
				GenerateRecurcively(x, y+1, type, size - 1);
			if (Random.value < 0.1f * size)
				GenerateRecurcively(x, y-1, type, size - 1);
		}
	}

	public BlockManager Get(int x, int y)
	{
		x = (int)Mathf.Repeat(x, Man.WorldSize);
		y = (int)Mathf.Repeat(y,  Man.WorldSize);
		if (_map[x, y] == null)
		{
			GameObject block = Instantiate(Man.BlockPrefab);
			_map[x, y] = block.GetComponent<BlockManager>();
			_map[x, y].Depth = _noise[x, y];
			_map[x, y].Back = (int)(_map[x, y].Depth * BSprites.Length);
			_map[x, y].Resource = -1;
			_map[x, y].Structure = -1;
			_map[x, y].Man = Man;
		}
		return _map[x, y];
	}

	void Update ()
	{
		_tick += Time.deltaTime;
		if (_tick >= Man.RenderTick)
		{
			BlockManager block;
			int x = (int)(Man.PlayerM.Player.localPosition.x / Man.BlockSize);
			int y = (int)(Man.PlayerM.Player.localPosition.y / Man.BlockSize);
			Man.SlowedDown = Get(x, y).Back < 2; 
			for (int i = x - Man.DistanceW; i < x + Man.DistanceW; i++)
			{
				for (int k = y - Man.DistanceH; k < y + Man.DistanceH; k++)
				{
					block = Get(i, k);
					if (block.gameObject.activeSelf == false)
					{
						block.transform.localPosition = new Vector2(i * Man.BlockSize, k * Man.BlockSize);
						block.gameObject.SetActive(true);
					}
				}
			}
			_tick = 0;
		}
	}
}
