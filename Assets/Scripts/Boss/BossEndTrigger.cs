using System;
using Player;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Boss
{
    public class BossEndTrigger : MonoBehaviour, ITriggerMidGame
    {
        [SerializeField] private GameObject objectToMove;

        [SerializeField] private int weapon;
        [SerializeField] private GameObject upgrade;

        void Start()
        {
            if (objectToMove == null)
                enabled = false; //no need to unlock a path
        }

        void FixedUpdate()
        {
            try
            {
                var x = Values.GetCurrentBoss().transform.position;
            }
            catch (Exception)
            {
                //todo così rompe gli altri livelli
                RunTrigger();
            }
        }

        public void RunTrigger()
        {
            if (objectToMove)
            {
                objectToMove.transform.position = new Vector3(-1000, -1000, -1000); //lo sposta in un punto lontano
                Values.SetCanSave(true); //permette di salvare nuovamente


                //Fornisce una nuova arma al player

                //todo forse non è necessario il check
                if (weapon != -1)
                {
                    AddWeaponToPlayer();
                }

                if (upgrade)
                {
                    AddUpgradeToPlayer();
                }


                //Play Regular Level Soundtrack
                Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.LevelTrack);


                gameObject.SetActive(false);
            }
        }


        private void AddWeaponToPlayer()
        {
            Values.AddHeldWeapon((Values.WeaponEnum) weapon, true);
            Values.SetTip("Hai ottenuto un: " + (Values.WeaponEnum)weapon);

            //Reset del timer per farlo printare tramite uiscript
            TimerController.ResetTimer(TimerController.TIP_K);
        }

        private void AddUpgradeToPlayer()
        {
            //todo 
        }
    }
}