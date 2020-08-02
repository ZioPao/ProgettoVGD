using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio{
	
	public static class SoundManager
	{

		public enum SoundEffects
        {
			HealthPickup,
			CollectiblePickup,
			MenuBlip,
			MeleeAttack,
			RangedAttack,
			FallingRocks,
			WeaponReload,
			PlayerHurt,
        }

		public enum SoundTracks
        {
			TitleTrack,
			LevelTrack,
			BossTrack,
        }

		private static Dictionary<SoundEffects, AudioClip> soundEffects;
		private static Dictionary<SoundEffects, float> effectVolumes;
		private static Dictionary<SoundEffects, float> effectPitches;

		private static GameObject soundGameObject;
		private static AudioSource effectAudioSource;


		private static Dictionary<SoundTracks, AudioClip> soundTracks;

		private static GameObject musicGameObject;
		private static AudioSource musicAudioSource;

		public static void InitializeSoundEffects()
        {
			soundEffects = new Dictionary<SoundEffects, AudioClip>();
			effectVolumes = new Dictionary<SoundEffects, float>();
			effectPitches = new Dictionary<SoundEffects, float>();
			AudioClip effect;

			effect = Resources.Load("Audio/Health") as AudioClip;
			soundEffects.Add(SoundEffects.HealthPickup, effect);
			effectVolumes.Add(SoundEffects.HealthPickup, 0.3f);
			effectPitches.Add(SoundEffects.HealthPickup, 1f);

			effect = Resources.Load("Audio/Collectible") as AudioClip;
			soundEffects.Add(SoundEffects.CollectiblePickup, effect);
			effectVolumes.Add(SoundEffects.CollectiblePickup, 0.2f);
			effectPitches.Add(SoundEffects.CollectiblePickup, 1f);

			effect = Resources.Load("Audio/Menu") as AudioClip;
			soundEffects.Add(SoundEffects.MenuBlip, effect);
			effectVolumes.Add(SoundEffects.MenuBlip, 0.05f);
			effectPitches.Add(SoundEffects.MenuBlip, 1f);

			effect = Resources.Load("Audio/Shot") as AudioClip;
			soundEffects.Add(SoundEffects.MeleeAttack, effect);
			effectVolumes.Add(SoundEffects.MeleeAttack, 1f);
			effectPitches.Add(SoundEffects.MeleeAttack, 2f);

			effect = Resources.Load("Audio/Shot") as AudioClip;
			soundEffects.Add(SoundEffects.RangedAttack, effect);
			effectVolumes.Add(SoundEffects.RangedAttack, 1f);
			effectPitches.Add(SoundEffects.RangedAttack, 1f);

			effect = Resources.Load("Audio/FallingRocks") as AudioClip;
			soundEffects.Add(SoundEffects.FallingRocks, effect);
			effectVolumes.Add(SoundEffects.FallingRocks, 1f);
			effectPitches.Add(SoundEffects.FallingRocks, 1f);

			effect = Resources.Load("Audio/Reload") as AudioClip;
			soundEffects.Add(SoundEffects.WeaponReload, effect);
			effectVolumes.Add(SoundEffects.WeaponReload, 0.5f);
			effectPitches.Add(SoundEffects.WeaponReload, 1f);

			effect = Resources.Load("Audio/Player_Hurt") as AudioClip;
			soundEffects.Add(SoundEffects.PlayerHurt, effect);
			effectVolumes.Add(SoundEffects.PlayerHurt, 0.5f);
			effectPitches.Add(SoundEffects.PlayerHurt, 1f);

		}

		public static void InitializeSoundTracks()
        {
			soundTracks = new Dictionary<SoundTracks, AudioClip>();
			AudioClip track;

			track = Resources.Load("Audio/TitleTrack") as AudioClip;
			soundTracks.Add(SoundTracks.TitleTrack, track);

			track = Resources.Load("Audio/LevelTrack") as AudioClip;
			soundTracks.Add(SoundTracks.LevelTrack, track);
		}

		public static void InitializeSoundPlayer()
        {
			soundGameObject = new GameObject("Sound Effect Player");
			effectAudioSource = soundGameObject.AddComponent<AudioSource>();
		}

		public static void InitializeMusicPlayer()
		{
			musicGameObject = new GameObject("Soundtrack Player");
			musicAudioSource = musicGameObject.AddComponent<AudioSource>();
		}

		public static void PlaySoundEffect(SoundEffects sound){

			effectAudioSource.volume = effectVolumes[sound];
			effectAudioSource.pitch = effectPitches[sound];
			effectAudioSource.clip = soundEffects[sound];
			effectAudioSource.Play();
		
		}

		public static void PlaySoundtrack(SoundTracks track)
        {
			musicAudioSource.volume = 0.1f;
			musicAudioSource.loop = true;
			musicAudioSource.clip = soundTracks[track];
			musicAudioSource.Play();

        }

	}

}