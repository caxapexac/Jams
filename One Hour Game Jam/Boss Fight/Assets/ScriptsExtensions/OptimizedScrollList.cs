//
// Coded by afrokick 2016 
//
#pragma warning disable 0649
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FriendsPopup : MonoBehaviour
{
	private int ItemHeight = 100;
	private const int Spacing = 8;
	private const int Top = 16;
	private const int Bottom = 50;

	[SerializeField]
	private FriendItemView[] _views;

	[SerializeField]
	private RectTransform _content;

	public event Action<int, FriendItemView> ItemShowed = delegate { };

	private int Count { get; set; }

	private float _y;
	private int _oldInd = -1;
	private RectTransform _item;

	void Update ()
	{
		_y = _content.anchoredPosition.y - Spacing;

		if (_y < 0)
			return;
		
		var inx = Mathf.FloorToInt (_y / (ItemHeight + Spacing));

		if (_oldInd == inx)
			return;
		
		//added to end
		if (inx > _oldInd) {
			var newInd = inx % _views.Length;

			newInd--;

			if (newInd < 0)
				newInd = _views.Length - 1;

			var id = inx + _views.Length - 1;

			if (id < Count) {
				_item = _views [newInd].GetComponent<RectTransform> ();

				var pos = _item.anchoredPosition;

				pos.y = -(Top + id * Spacing + id * ItemHeight);

				_item.anchoredPosition = pos;

				ItemShowed (id, _views [newInd]);
			}
		}
		//added to begin
		else {
			var newInd = inx % _views.Length;

			_item = _views [newInd].GetComponent<RectTransform> ();

			var pos = _item.anchoredPosition;

			pos.y = -(Top + inx * Spacing + inx * ItemHeight);

			_item.anchoredPosition = pos;

			ItemShowed (inx, _views [newInd]);
		}

		_oldInd = inx;
	}

	public void SetData (int count)
	{
		_oldInd = 0;

		Count = count;

		var h = ItemHeight * count * 1f + Top + Bottom + (count == 0 ? 0 : ((count - 1) * Spacing));

		_content.sizeDelta = new Vector2 (_content.sizeDelta.x, h);

		var pos = _content.anchoredPosition;
		pos.y = 0;
		_content.anchoredPosition = pos;

		bool showed = false;

		var y = Top;

		for (int i = 0; i < _views.Length; i++) {
			showed = i < count;

			_views [i].gameObject.SetActive (showed);

			if (showed) {
				pos = _views [i].GetComponent<RectTransform> ().anchoredPosition;
				pos.y = -y;
				_views [i].GetComponent<RectTransform> ().anchoredPosition = pos;

				y += Spacing + ItemHeight;

				ItemShowed (i, _views [i]);
			}
		}
	}
}

public class FriendItemView : MonoBehaviour
{
	[SerializeField]
	private Text _nameText;

	public void SetData (string friendName)
	{
		_nameText.text = friendName;
	}
}

public class Controller : MonoBehaviour
{
	[SerializeField]
	private FriendsPopup _popup;

	// Use this for initialization
	void Start ()
	{
		_popup.ItemShowed += (index, view) => {
			view.SetData ("item " + index);
		};

		_popup.SetData (1000);
	}
}