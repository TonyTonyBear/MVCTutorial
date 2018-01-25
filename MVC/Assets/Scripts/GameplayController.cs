using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Beats
{
	public class GameplayController : MonoBehaviour
	{

		[Header("Inputs")]
		[SerializeField] private KeyCode left;
		[SerializeField] private KeyCode right;
		[SerializeField] private KeyCode up;
		[SerializeField] private KeyCode down;

		[Header("Track")]
		[Tooltip ("Track to play")]
		[SerializeField] private Track _track;

		/// <summary>
		/// The currently playing Track.
		/// </summary>
		public Track track
		{
			get { return _track; }
		}

		public float beatsPerSecond
		{
			get;
			private set;
		}
		public float secondsPerBeat
		{
			get;
			private set;
		}

		private bool played;
		private bool completed;

		TrackView trackView;

		private WaitForSeconds waitAndStop;

		#region Singleton Pattern

		private static GameplayController _instance;

		public static GameplayController instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = (GameplayController)GameObject.FindObjectOfType(typeof(GameplayController));
				}

				return _instance;
			}

			set
			{
				_instance = value;
			}
		}

		private void OnDestroy()
		{
			_instance = null;
		}

		#endregion

		#region Monobehavior Events

		private void Awake()
		{
			_instance = this;
			beatsPerSecond = track.bpm / 60f;
			secondsPerBeat = 60f / track.bpm;
			waitAndStop = new WaitForSeconds(secondsPerBeat * 2);

			trackView = FindObjectOfType<TrackView>();

			if(!trackView)
			{
				Debug.LogError("No TrackView found in scene.");
			}
		}

		// Use this for initialization
		void Start()
		{
			InvokeRepeating("NextBeat", 0f, secondsPerBeat);
		}

		// Update is called once per frame
		void Update()
		{
			if (played || completed)
				return;

			if(Input.GetKeyDown(left))
			{
				PlayBeat(0);
			}

			if (Input.GetKeyDown(down))
			{
				PlayBeat(1);
			}

			if (Input.GetKeyDown(up))
			{
				PlayBeat(2);
			}

			if (Input.GetKeyDown(right))
			{
				PlayBeat(3);
			}
		}

		#endregion

		#region Gameplay

		private int _current;
		public int current
		{
			get { return _current; }
			set
			{
				if(value != _current)
				{
					_current = value;

					if (_current == track.beats.Count)
					{
						CancelInvoke("NextBeat");
						completed = true;

						StartCoroutine(WaitAndStop());
					}
				}
			}
		}

		void PlayBeat(int input)
		{

			played = true;

			if(track.beats[current] == -1)
			{
				//Debug.Log(string.Format("{0} played untimely", input));
			}
			else if(track.beats[current] == input)
			{
				//Debug.Log(string.Format("{0} correctly", input));
				trackView.TriggerBeatView(current, TrackView.Trigger.Right);
			}
			else
			{
				//Debug.Log(string.Format("{0} played, but {1} was expected.", input, track.beats[current]));
				trackView.TriggerBeatView(current, TrackView.Trigger.Wrong);
			}
		}

		void NextBeat()
		{
			Debug.Log("Tick");

			if(!played && track.beats[current] != -1)
			{
				Debug.Log(string.Format("{0} missed", track.beats[current]));
				trackView.TriggerBeatView(current, TrackView.Trigger.Missed);
			}

			played = false;
			current++;	
		}

		private IEnumerator WaitAndStop()
		{
			yield return waitAndStop;
			enabled = false;	
		}

		#endregion

	}
}