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
		private static Dictionary<SoundEffects, float> effectDefaultVolumes;
		private static Dictionary<SoundEffects, float> effectModifiedVolumes;
		private static Dictionary<SoundEffects, float> effectPitches;

		private static GameObject soundGameObject;
		private static AudioSource effectAudioSource;
		private static AudioLowPassFilter effectLowPass;
		private static float effectVolumeModifier;		//Values have to be between 0 and 1


		private static Dictionary<SoundTracks, AudioClip> soundTracks;
		private static float trackDefaultVolume;
		private static float trackModifiedVolume;

		private static GameObject musicGameObject;
		private static AudioSource musicAudioSource;
		private static AudioLowPassFilter musicLowPass;
		private static float trackVolumeModifier;		//Values have to be between 0 and 1

		//Sound Effect Methods

		public static void InitializeSoundEffects()
        {
			soundEffects = new Dictionary<SoundEffects, AudioClip>();
			effectDefaultVolumes = new Dictionary<SoundEffects, float>();
			effectModifiedVolumes = new Dictionary<SoundEffects, float>();
			effectPitches = new Dictionary<SoundEffects, float>();
			AudioClip effect;

			effectVolumeModifier = 0.5f;

			effect = Resources.Load("Audio/Health") as AudioClip;
			soundEffects.Add(SoundEffects.HealthPickup, effect);
			effectDefaultVolumes.Add(SoundEffects.HealthPickup, 0.3f);
			effectModifiedVolumes.Add(SoundEffects.HealthPickup, 0.15f);
			effectPitches.Add(SoundEffects.HealthPickup, 1f);

			effect = Resources.Load("Audio/Collectible") as AudioClip;
			soundEffects.Add(SoundEffects.CollectiblePickup, effect);
			effectDefaultVolumes.Add(SoundEffects.CollectiblePickup, 0.2f);
			effectModifiedVolumes.Add(SoundEffects.CollectiblePickup, 0.1f);
			effectPitches.Add(SoundEffects.CollectiblePickup, 1f);

			effect = Resources.Load("Audio/Menu") as AudioClip;
			soundEffects.Add(SoundEffects.MenuBlip, effect);
			effectDefaultVolumes.Add(SoundEffects.MenuBlip, 0.1f);
			effectModifiedVolumes.Add(SoundEffects.MenuBlip, 0.05f);
			effectPitches.Add(SoundEffects.MenuBlip, 1f);

			effect = Resources.Load("Audio/Shot") as AudioClip;
			soundEffects.Add(SoundEffects.MeleeAttack, effect);
			effectDefaultVolumes.Add(SoundEffects.MeleeAttack, 2f);
			effectModifiedVolumes.Add(SoundEffects.MeleeAttack, 1f);
			effectPitches.Add(SoundEffects.MeleeAttack, 2f);

			effect = Resources.Load("Audio/Shot") as AudioClip;
			soundEffects.Add(SoundEffects.RangedAttack, effect);
			effectDefaultVolumes.Add(SoundEffects.RangedAttack, 2f);
			effectModifiedVolumes.Add(SoundEffects.RangedAttack, 1f);
			effectPitches.Add(SoundEffects.RangedAttack, 1f);

			effect = Resources.Load("Audio/Shot") as AudioClip;
			soundEffects.Add(SoundEffects.RangedAttackFail, effect);
			effectDefaultVolumes.Add(SoundEffects.RangedAttackFail, 1f);
			effectModifiedVolumes.Add(SoundEffects.RangedAttackFail, 0.5f);
			effectPitches.Add(SoundEffects.RangedAttackFail, 3f);

			effect = Resources.Load("Audio/FallingRocks") as AudioClip;
			soundEffects.Add(SoundEffects.FallingRocks, effect);
			effectDefaultVolumes.Add(SoundEffects.FallingRocks, 2f);
			effectModifiedVolumes.Add(SoundEffects.FallingRocks, 1f);
			effectPitches.Add(SoundEffects.FallingRocks, 1f);

			effect = Resources.Load("Audio/Reload") as AudioClip;
			soundEffects.Add(SoundEffects.WeaponReload, effect);
			effectDefaultVolumes.Add(SoundEffects.WeaponReload, 1f);
			effectModifiedVolumes.Add(SoundEffects.WeaponReload, 0.5f);
			effectPitches.Add(SoundEffects.WeaponReload, 1f);

			effect = Resources.Load("Audio/Reload") as AudioClip;
			soundEffects.Add(SoundEffects.ReloadFail, effect);
			effectDefaultVolumes.Add(SoundEffects.ReloadFail, 1f);
			effectModifiedVolumes.Add(SoundEffects.ReloadFail, 0.5f);
			effectPitches.Add(SoundEffects.ReloadFail, 2.5f);

			effect = Resources.Load("Audio/Player_Hurt") as AudioClip;
			soundEffects.Add(SoundEffects.PlayerHurt, effect);
			effectDefaultVolumes.Add(SoundEffects.PlayerHurt, 0.16f);
			effectModifiedVolumes.Add(SoundEffects.PlayerHurt, 0.08f);
			effectPitches.Add(SoundEffects.PlayerHurt, 1f);

			effect = Resources.Load("Audio/Lever") as AudioClip;
			soundEffects.Add(SoundEffects.LeverActivate, effect);
			effectDefaultVolumes.Add(SoundEffects.LeverActivate, 2.4f);
			effectModifiedVolumes.Add(SoundEffects.LeverActivate, 1.2f);
			effectPitches.Add(SoundEffects.LeverActivate, 1f);

			effect = Resources.Load("Audio/DoorOpen") as AudioClip;
			soundEffects.Add(SoundEffects.DoorOpen, effect);
			effectDefaultVolumes.Add(SoundEffects.DoorOpen, 2f);
			effectModifiedVolumes.Add(SoundEffects.DoorOpen, 1f);
			effectPitches.Add(SoundEffects.DoorOpen, 1f);

			effect = Resources.Load("Audio/DoorLocked") as AudioClip;
			soundEffects.Add(SoundEffects.DoorLocked, effect);
			effectDefaultVolumes.Add(SoundEffects.DoorLocked, 1f);
			effectModifiedVolumes.Add(SoundEffects.DoorLocked, 0.5f);
			effectPitches.Add(SoundEffects.DoorLocked, 1f);

			effect = Resources.Load("Audio/Boss3Laugh") as AudioClip;
			soundEffects.Add(SoundEffects.FinalBossLaugh, effect);
			effectDefaultVolumes.Add(SoundEffects.FinalBossLaugh, 1f);
			effectModifiedVolumes.Add(SoundEffects.FinalBossLaugh, 0.5f);
			effectPitches.Add(SoundEffects.FinalBossLaugh, 1f);

		}

		private static void ChangeEffectVolume()
        {
			foreach(var item in effectDefaultVolumes)
            {
				effectModifiedVolumes[item.Key] = effectDefaultVolumes[item.Key] * effectVolumeModifier;
            }
        }

		public static float GetEffectVolumeModifier()
        {
			return effectVolumeModifier;
        }

		public static void SetEffectVolumeModifier(float modifier)
        {
			effectVolumeModifier = modifier;

			ChangeEffectVolume();
        }

		//Sound Track Methods

		public static void InitializeSoundTracks()
        {
			soundTracks = new Dictionary<SoundTracks, AudioClip>();
			AudioClip track;

			trackDefaultVolume = 0.2f;
			trackModifiedVolume = 0.1f;
			trackVolumeModifier = 0.5f;

			track = Resources.Load("Audio/TitleTrack") as AudioClip;
			soundTracks.Add(SoundTracks.TitleTrack, track);

			track = Resources.Load("Audio/LevelTrack") as AudioClip;
			soundTracks.Add(SoundTracks.LevelTrack, track);

			track = Resources.Load("Audio/BossTrack") as AudioClip;
			soundTracks.Add(SoundTracks.BossTrack, track);
		}

		private static void ChangeTrackVolume()
        {
			trackModifiedVolume = trackDefaultVolume * trackVolumeModifier;
			musicAudioSource.volume = trackModifiedVolume;
		}

		public static float GetTrackVolumeModifier()
        {
			return trackVolumeModifier;
        }

		public static void SetTrackVolumeModifier(float modifier)
        {
			trackVolumeModifier = modifier;

			ChangeTrackVolume();
        }

		//Audio Player Methods

		public static void InitializeSoundPlayer()
        {
			soundGameObject = new GameObject("Sound Effect Player");
			effectAudioSource = soundGameObject.AddComponent<AudioSource>();
			effectLowPass = soundGameObject.AddComponent<AudioLowPassFilter>();

			effectLowPass.cutoffFrequency = 2000;
			effectLowPass.enabled = false;
		}

		public static void InitializeMusicPlayer()
		{
			musicGameObject = new GameObject("Soundtrack Player");
			musicAudioSource = musicGameObject.AddComponent<AudioSource>();
			musicLowPass = musicGameObject.AddComponent<AudioLowPassFilter>();

			musicLowPass.cutoffFrequency = 2000;
			musicLowPass.enabled = false;
		}

		public static void PlaySoundEffect(SoundEffects sound){

			effectAudioSource.volume = effectModifiedVolumes[sound];
			effectAudioSource.pitch = effectPitches[sound];
			effectAudioSource.clip = soundEffects[sound];
			effectAudioSource.Play();
		
		}

		public static void PlaySoundtrack(SoundTracks track)
        {
			musicAudioSource.volume = trackModifiedVolume;
			musicAudioSource.loop = true;
			musicAudioSource.clip = soundTracks[track];
			musicAudioSource.Play();
        }

		public static void EnableLowPass()
        {
			effectLowPass.enabled = true;
			musicLowPass.enabled = true;
        }
		public static void DisableLowPass()
		{
			effectLowPass.enabled = false;
			musicLowPass.enabled = false;
		}

	}

}