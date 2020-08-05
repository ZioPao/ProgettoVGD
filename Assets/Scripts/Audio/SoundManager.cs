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
			RangedAttackFail,
			FallingRocks,
			WeaponReload,
			ReloadFail,
			PlayerHurt,
			LeverActivate,
			DoorOpen,
			DoorLocked,
			FinalBossLaugh,
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
			effectVolumes.Add(SoundEffects.HealthPickup, 0.15f);
			effectPitches.Add(SoundEffects.HealthPickup, 1f);

			effect = Resources.Load("Audio/Collectible") as AudioClip;
			soundEffects.Add(SoundEffects.CollectiblePickup, effect);
			effectVolumes.Add(SoundEffects.CollectiblePickup, 0.1f);
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

			effect = Resources.Load("Audio/Shot") as AudioClip;
			soundEffects.Add(SoundEffects.RangedAttackFail, effect);
			effectVolumes.Add(SoundEffects.RangedAttackFail, 0.5f);
			effectPitches.Add(SoundEffects.RangedAttackFail, 3f);

			effect = Resources.Load("Audio/FallingRocks") as AudioClip;
			soundEffects.Add(SoundEffects.FallingRocks, effect);
			effectVolumes.Add(SoundEffects.FallingRocks, 1f);
			effectPitches.Add(SoundEffects.FallingRocks, 1f);

			effect = Resources.Load("Audio/Reload") as AudioClip;
			soundEffects.Add(SoundEffects.WeaponReload, effect);
			effectVolumes.Add(SoundEffects.WeaponReload, 0.5f);
			effectPitches.Add(SoundEffects.WeaponReload, 1f);

			effect = Resources.Load("Audio/Reload") as AudioClip;
			soundEffects.Add(SoundEffects.ReloadFail, effect);
			effectVolumes.Add(SoundEffects.ReloadFail, 0.5f);
			effectPitches.Add(SoundEffects.ReloadFail, 2.5f);

			effect = Resources.Load("Audio/Player_Hurt") as AudioClip;
			soundEffects.Add(SoundEffects.PlayerHurt, effect);
			effectVolumes.Add(SoundEffects.PlayerHurt, 0.08f);
			effectPitches.Add(SoundEffects.PlayerHurt, 1f);

			effect = Resources.Load("Audio/Lever") as AudioClip;
			soundEffects.Add(SoundEffects.LeverActivate, effect);
			effectVolumes.Add(SoundEffects.LeverActivate, 1.2f);
			effectPitches.Add(SoundEffects.LeverActivate, 1f);

			effect = Resources.Load("Audio/DoorOpen") as AudioClip;
			soundEffects.Add(SoundEffects.DoorOpen, effect);
			effectVolumes.Add(SoundEffects.DoorOpen, 1f);
			effectPitches.Add(SoundEffects.DoorOpen, 1f);

			effect = Resources.Load("Audio/DoorLocked") as AudioClip;
			soundEffects.Add(SoundEffects.DoorLocked, effect);
			effectVolumes.Add(SoundEffects.DoorLocked, 0.5f);
			effectPitches.Add(SoundEffects.DoorLocked, 1f);

			effect = Resources.Load("Audio/Boss3Laugh") as AudioClip;
			soundEffects.Add(SoundEffects.FinalBossLaugh, effect);
			effectVolumes.Add(SoundEffects.FinalBossLaugh, 0.5f);
			effectPitches.Add(SoundEffects.FinalBossLaugh, 1f);

		}

		public static void InitializeSoundTracks()
        {
			soundTracks = new Dictionary<SoundTracks, AudioClip>();
			AudioClip track;

			track = Resources.Load("Audio/TitleTrack") as AudioClip;
			soundTracks.Add(SoundTracks.TitleTrack, track);

			track = Resources.Load("Audio/LevelTrack") as AudioClip;
			soundTracks.Add(SoundTracks.LevelTrack, track);

			track = Resources.Load("Audio/BossTrack") as AudioClip;
			soundTracks.Add(SoundTracks.BossTrack, track);
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