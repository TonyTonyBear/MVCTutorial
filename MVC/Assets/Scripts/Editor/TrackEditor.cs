﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Beats
{
	[CustomEditor (typeof (Track))]
	public class TrackEditor : Editor
	{
		Track track;

		Vector2 position;

		bool displayBeatsData;

		public void OnEnable()
		{
			track = (Track)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (track.beats.Count == 0)
			{
				EditorGUILayout.HelpBox("Track is empty", MessageType.Warning);

				if (GUILayout.Button("Generate Random Track", EditorStyles.miniButton))
					track.Randomize();
			}
			else
			{
				if (GUILayout.Button("Update Random Track", EditorStyles.miniButton))
					track.Randomize();

				displayBeatsData = EditorGUILayout.Foldout(displayBeatsData, "Display Beats");
				if (displayBeatsData)
				{
					position = EditorGUILayout.BeginScrollView(position);

					for (int i = 0; i < track.beats.Count; i++)
					{
						track.beats[i] = EditorGUILayout.IntSlider(track.beats[i], -1, Track.inputs - 1);
					}

					EditorGUILayout.EndScrollView();
				}
			}

			EditorUtility.SetDirty(track);
		}
	}
}
