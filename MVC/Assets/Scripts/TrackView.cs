using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Beats
{
	[RequireComponent(typeof (VerticalLayoutGroup))]
	[RequireComponent(typeof (ContentSizeFitter))]
	[RequireComponent(typeof(RectTransform))]
	public class TrackView : MonoBehaviour
	{
		[SerializeField] private Track track;

		[SerializeField] RectTransform left;
		[SerializeField] RectTransform right;
		[SerializeField] RectTransform up;
		[SerializeField] RectTransform down;

		[SerializeField] RectTransform empty;

		RectTransform rTransform;

		private Vector2 _position;
		public float position
		{
			get
			{
				return _position.y;
			}
			set
			{
				if (value != _position.y)
				{
					_position.y = value;
					rTransform.anchoredPosition = _position;
				}
			}
		}

		public void Init(Track track)
		{
			rTransform = (RectTransform)transform;
			_position = rTransform.anchoredPosition;

			foreach(int i in track.beats)
			{
				GameObject newObj;

				switch(i)
				{
					case 0:
						newObj = left.gameObject;
						break;

					case 1:
						newObj = down.gameObject;
						break;

					case 2:
						newObj = up.gameObject;
						break;

					case 3:
						newObj = right.gameObject;
						break;

					default:
						newObj = empty.gameObject;
						break;
				}

				Transform view = GameObject.Instantiate(newObj, transform).transform;
				view.SetAsFirstSibling();
			}
		}

		void Start()
		{
			Init(track);
		}

		void Update()
		{
			position -= Time.deltaTime * 200f;
		}
	}
}
