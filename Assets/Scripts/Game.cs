using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD45
{
	public class Game : MonoBehaviour
	{
		// singleton
		public static Game instance = null;

		[Space, ReadOnly]
		public int currentRoomIndex = 0;
		public Room[] roomConfig;

		[Header("Scene Components")]
		public ScreenFader screenFader;
		public CameraRig cameraRig;
		public DragController dragController;

		[Header("Audio")]
		public AudioSource commonAudioSrc;
		[Space]
		public AudioClip roomSolvedSound;



		void Awake()
		{
			// singleton mgmt
			if (instance == null) instance = this;
			else if (instance != this) Destroy(gameObject);
		}

		void Start()
		{
			// TODO Music if (Music.instance) Music.instance.PlayMusicTrack(Music.MusicTrack.Default);

			screenFader.FadeScreenIn();

			GameBegin();
		}





		void GameBegin()
		{
		}
		public void CheckCurrentRoomCompletition()
		{
			if (roomConfig[currentRoomIndex].CheckRoomForCompletition())
			{
				Debug.Log($"Room {currentRoomIndex} finished");
				
				cameraRig.MoveToRoom(++currentRoomIndex);

				PlayAudio(roomSolvedSound);
			}
		}




		public void PlayAudio(AudioClip clip)
		{
			// if already playing the same audio clip, do nothing
			if (commonAudioSrc.isPlaying && commonAudioSrc.clip == clip) return;
			
			if (commonAudioSrc.isPlaying) commonAudioSrc.Stop();
			
			commonAudioSrc.clip = clip;
			commonAudioSrc.Play();
		}



		public void ButtonAndroidBackDown()
		{
			//GameController.Exit();
		}
	}
}