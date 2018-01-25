using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beats
{
	[CreateAssetMenu (menuName = "Beats/New Track", fileName = "New Beats Track.asset")]
	public class Track : ScriptableObject
	{
		[Header("Playback Settings")]
		[Tooltip("# of beats per minute.")]
		[Range(30, 360)]
		public int bpm = 120;

		[HideInInspector] public List<int> beats = new List<int>();

		static public int inputs = 4;

		[Header("Settings")]

		[Tooltip ("# of preroll (empty) beats")]
		[Range (0, 10)] [SerializeField] private int _preroll = 10;
		[Tooltip("Minimum # of beats per block")]
		[Range(1, 20)] [SerializeField] private int _minBlock = 2;
		[Tooltip("Maximum # of beats per block")]
		[Range(1, 20)] [SerializeField] private int _maxBlock = 5;
		[Tooltip("Minimum # of empty beats between blocks")]
		[Range(1, 20)] [SerializeField] private int _minInterval = 1;
		[Tooltip("Maximum # of empty beats between blocks")]
		[Range(1, 20)] [SerializeField] private int _maxInterval = 2;
		[Tooltip("# of blocks")]
		[Range(1, 20)] [SerializeField] private int _blocks = 10;

		public void Randomize()
		{
			beats = new List<int>();

			//Add preroll beats
			for(int i = 0; i < _preroll; i++)
			{
				beats.Add(-1);
			}

			//Add blocks
			for(int i = 0; i < _blocks; i++)
			{
				int blockLength = Random.Range(_minBlock, _maxBlock + 1);

				//Add beats
				for(int j = 0; j < blockLength; j++)
				{
					int beat = Random.Range(0, inputs);
					beats.Add(beat);
				}

				if (i == _blocks - 1) //No interval if it's the last beat block.
					break;

				int intervalLength = Random.Range(_minInterval, _maxInterval + 1);

				//Add intervals
				for (int j = 0; j < intervalLength; j++)
				{
					beats.Add(-1);
				}
			}
		}
	}
}
