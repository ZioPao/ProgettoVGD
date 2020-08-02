using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio{
	
	public static class SoundManager
	{

		public enum Sound
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

		private static Dictionary<Sound, AudioClip> soundEffects;
		private static Dictionary<Sound, float> effectVolumes;
		private static Dictionary<Sound, float> effectPitches;

		private static GameObject soundGameObject;
		private static AudioSource audioSource;

		public static void InitializeSoundEffects()
        {
			soundEffects = new Dictionary<Sound, AudioClip>();
			effectVolumes = new Dictionary<Sound, float>();
			effectPitches = new Dictionary<Sound, float>();
			AudioClip effect;

			effect = Resources.Load("Audio/Health") as AudioClip;
			soundEffects.Add(Sound.HealthPickup, effect);
			effectVolumes.Add(Sound.HealthPickup, 0.3f);
			effectPitches.Add(Sound.HealthPickup, 1f);

			effect = Resources.Load("Audio/Collectible") as AudioClip;
			soundEffects.Add(Sound.CollectiblePickup, effect);
			effectVolumes.Add(Sound.CollectiblePickup, 0.2f);
			effectPitches.Add(Sound.CollectiblePickup, 1f);

			effect = Resources.Load("Audio/Menu") as AudioClip;
			soundEffects.Add(Sound.MenuBlip, effect);
			effectVolumes.Add(Sound.MenuBlip, 0.05f);
			effectPitches.Add(Sound.MenuBlip, 1f);

			effect = Resources.Load("Audio/Shot") as AudioClip;
			soundEffects.Add(Sound.MeleeAttack, effect);
			effectVolumes.Add(Sound.MeleeAttack, 1f);
			effectPitches.Add(Sound.MeleeAttack, 2f);

			effect = Resources.Load("Audio/Shot") as AudioClip;
			soundEffects.Add(Sound.RangedAttack, effect);
			effectVolumes.Add(Sound.RangedAttack, 1f);
			effectPitches.Add(Sound.RangedAttack, 1f);

			effect = Resources.Load("Audio/FallingRocks") as AudioClip;
			soundEffects.Add(Sound.FallingRocks, effect);
			effectVolumes.Add(Sound.FallingRocks, 1f);
			effectPitches.Add(Sound.FallingRocks, 1f);

			effect = Resources.Load("Audio/Reload") as AudioClip;
			soundEffects.Add(Sound.WeaponReload, effect);
			effectVolumes.Add(Sound.WeaponReload, 0.5f);
			effectPitches.Add(Sound.WeaponReload, 1f);

			effect = Resources.Load("Audio/Player_Hurt") as AudioClip;
			soundEffects.Add(Sound.PlayerHurt, effect);
			effectVolumes.Add(Sound.PlayerHurt, 0.5f);
			effectPitches.Add(Sound.PlayerHurt, 1f);

		}

		public static void InitializeSoundPlayer()
        {
			soundGameObject = new GameObject("Sound");
			audioSource = soundGameObject.AddComponent<AudioSource>();
		}

		public static void PlaySoundEffect(Sound sound){
			
			audioSource.volume = effectVolumes[sound];
			audioSource.pitch = effectPitches[sound];
			audioSource.PlayOneShot(soundEffects[sound]);
		
		}

	}

}