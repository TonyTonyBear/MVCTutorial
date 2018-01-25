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

		public enum Trigger { Missed, Right, Wrong}

		[SerializeField] RectTransform left;
		[SerializeField] RectTransform right;
		[SerializeField] RectTransform up;
		[SerializeField] RectTransform down;

		[SerializeField] RectTransform empty;

		RectTransform rTransform;
		List<Image> beatViews;

		private Vector2 _position;
		private float beatViewSize;
		private float spacing;
		
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

			beatViewSize = empty.rect.height;
			spacing = GetComponent<VerticalLayoutGroup>().spacing;

			beatViews = new List<Image>();

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

				Image view = GameObject.Instantiate(newObj, transform).GetComponent<Image>();
				view.transform.SetAsFirstSibling();

				beatViews.Add(view);
			}
		}

		void Start()
		{
			Init(GameplayController.instance.track);
		}

		void Update()
		{
			position -= (beatViewSize + spacing) * Time.deltaTime * GameplayController.instance.beatsPerSecond;
		}

		public void TriggerBeatView(int index, Trigger trigger)
		{
			switch(trigger)
			{
				case Trigger.Missed:
					beatViews[index].color = Color.gray;
					//Debug.Break();
					break;

				case Trigger.Right:
					beatViews[index].color = Color.yellow;
					break;

				case Trigger.Wrong:
					beatViews[index].color = Color.cyan;
					break;
			}
		}
	}
}
