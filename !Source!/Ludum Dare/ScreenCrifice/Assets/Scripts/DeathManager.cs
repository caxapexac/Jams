using System.Collections;
using System.Collections.Generic;
using LeopotamGroup.Collections;
using LeopotamGroup.Math;
using UnityEngine;
using LeopotamGroup.Pooling;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class DeathManager : MonoBehaviour
{
	[HideInInspector]
	public Man Man;
	public PoolContainer Pool;
	public Transform PixelParent;
	public int Extra;

	public int ScreenHSize;
	public int ScreenWSize;
	private IPoolObject[,] _pixels;
	public float Tick;
	public int MaxPixels;
	public int PixelCount;
	public int LastFloor;
		
	private void Start()
	{
		Man = GetComponent<Man>();
		Random.InitState(Man.Saeed);
		Pool = PoolContainer.CreatePool("Prefabs/Pixel", PixelParent);
		ScreenWSize = Man.ScreenW / Man.PixelSize;
		ScreenHSize = Man.ScreenH / Man.PixelSize;
		MaxPixels = ScreenHSize * ScreenWSize;
		_pixels = new IPoolObject[Man.ScreenW / Man.PixelSize, Man.ScreenH / Man.PixelSize];
		Tick = 0;
		PixelCount = 0;
		Extra = MaxPixels / 7;
		LastFloor = 0;
	}

	private void FixedUpdate()
	{
		
		if (Man.Alive && Man.Difficulty >= 0.01f)
		{
			Man.Difficulty *= Man.DiffMultipiler;
		}
	}

	void Update ()
	{
		Tick += Time.deltaTime;
		Man.GlobalTime += Time.deltaTime;
		if (Mathf.FloorToInt(Man.GlobalTime) / 5 != LastFloor)
		{
			Man.MouseM.TextColorGen(Man.MousePos, (int)(Man.GlobalTime) + " SECONDS", Color.red);
			LastFloor = Mathf.FloorToInt(Man.GlobalTime) / 5;
			if (Man.GlobalTime > 600)
			{
				WinMgs();
			}
		}
		while (Extra > 0 && Man.Alive)
		{
			Extra = Mathf.Clamp(Extra - 1, 0, 300);
			if (PixelCount >= MaxPixels - 3)
			{
				Man.Alive = false;
				LoseMsg();
				return;
			}

			int x;
			int y;
			do
			{
				if (Random.value >= 0.5f)
				{
					x = (int)(Random.value * 10000 % ScreenWSize);
					if (Random.value >= 0.5f)
					{
						y = 0;
						while (y != ScreenHSize - 1 && _pixels[x, y] != null) y++;
					}
					else
					{
						y = ScreenHSize - 1;
						while (y != 0 && _pixels[x, y] != null) y--;
					}
				}
				else
				{
					y = (int)(Random.value * 10000 % ScreenHSize);
					if (Random.value >= 0.5f)
					{
						x = 0;
						while (x != ScreenWSize - 1 && _pixels[x, y] != null) x++;
					}
					else
					{
						x = ScreenWSize - 1;
						while (x != 0 && _pixels[x, y] != null) x--;
					}
					
				}
			} while (_pixels[x, y] != null);
			_pixels[x, y] = Pool.Get();
			_pixels[x, y].PoolTransform.GetComponent<RectTransform>()
				.anchoredPosition = new Vector2(x*Man.PixelSize + Man.PixelSize/2, -y*Man.PixelSize - Man.PixelSize/2);
			_pixels[x, y].PoolTransform.gameObject.SetActive(true);
			PixelCount++;
		}
		while (Tick > Man.Difficulty && Man.Alive)
		{
			if (PixelCount >= MaxPixels - 3)
			{
				Man.Alive = false;
				LoseMsg();
				return;
			}

			int x;
			int y;
			do
			{
				if (Random.value >= 0.5f)
				{
					x = (int)(Random.value * 10000 % ScreenWSize);
					if (Random.value >= 0.5f)
					{
						y = 0;
						while (y != ScreenHSize - 1 && _pixels[x, y] != null) y++;
					}
					else
					{
						y = ScreenHSize - 1;
						while (y != 0 && _pixels[x, y] != null) y--;
					}
				}
				else
				{
					y = (int)(Random.value * 10000 % ScreenHSize);
					if (Random.value >= 0.5f)
					{
						x = 0;
						while (x != ScreenWSize - 1 && _pixels[x, y] != null) x++;
					}
					else
					{
						x = ScreenWSize - 1;
						while (x != 0 && _pixels[x, y] != null) x--;
					}
					
				}
			} while (_pixels[x, y] != null);
			_pixels[x, y] = Pool.Get();
			_pixels[x, y].PoolTransform.GetComponent<RectTransform>()
				.anchoredPosition = new Vector2(x*Man.PixelSize + Man.PixelSize/2, -y*Man.PixelSize - Man.PixelSize/2);
			_pixels[x, y].PoolTransform.gameObject.SetActive(true);
			
			PixelCount++;
			Tick -= Man.Difficulty;
		}
	}
	
	public void FixPixel()
	{
		//Debug.Log((_pixelCount == 0) + " " + (Man == null));
		if (PixelCount > 0 && PixelCount < MaxPixels - 10 && Man.Alive)
		{
			int x = ScreenWSize / 2;
			int y = ScreenHSize / 2;
			do
			{
				if (Random.value >= 0.5f && x > 0 && x < ScreenWSize - 1)
				{
					if (Random.value >= 0.5f)
					{
						x++;
					}
					else
					{
						x--;
					}
				}
				else if (y > 0 && y < ScreenHSize - 1)
				{
					if (Random.value >= 0.5f)
					{
						y++;
					}
					else
					{
						y--;
					}
				}
				else
				{
					x = ScreenWSize / 2;
					y = ScreenHSize / 2;
				}
			} while (_pixels[x, y] == null);
			Pool.Recycle(_pixels[x, y]);
			PixelCount--;
			_pixels[x, y] = null;
		}
	}

	void LoseMsg()
	{
		//SceneManager.LoadScene("GameOverScene");
		Man.ErrImage.transform.SetAsLastSibling();
		Man.ErrImage.SetActive(true);
		Man.ErrSound.Play();
		StartCoroutine(Restart());
	}

	private IEnumerator Restart()
	{
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene("SampleScene");
	}

	void WinMgs()
	{
		StartCoroutine(WinText());
	}

	private IEnumerator WinText()
	{
		for (int i = 0; i < 50; i++)
		{
			Man.MouseM.TextColorGen(Man.PlayerM.Player.localPosition + Random.onUnitSphere * Random.value * Man.ScreenW / 50, "YOU WON!", Color.red);
			yield return new WaitForSeconds(0.3f);
		}
		LoseMsg();
	}
}

