using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Utility
{
    public static class TimerController
    {
        //Default timers keys

        public static readonly string KNIFEATTACK_K = "KnifeAttack";
        public static readonly string KNIFECOOLDOWN_K = "KnifeCooldown";
        public static readonly string PISTOLATTACK_K = "PistolAttack";
        public static readonly string PISTOLCOOLDOWN_K = "PistolCooldown";
        public static readonly string SMGATTACK_K = "SMGAttack";
        public static readonly string SMGCOOLDOWN_K = "SMGCooldown";
        public static readonly string RELOADTIME_K = "ReloadTime";
        public static readonly string ENEMYSPAWN_K = "EnemySpawn";
        public static readonly string BOSSTWOPHASE = "BossTwoPhase";
        public static readonly string TIP_K = "Tip";
        
        //Timer Types
 

        
        //Timer Durations
        private static Dictionary<string, float> timerLength;
        private static Dictionary<string, float> currentTime;
        
        public static void Setup()
        {
            InitializeTimerLength();
            InitializeCurrentTime();

            AddTimer(KNIFEATTACK_K, 0.35f);
            AddCurrentTime(KNIFEATTACK_K, 0f);
            
            AddTimer(KNIFECOOLDOWN_K, 0.35f);
            AddCurrentTime(KNIFECOOLDOWN_K, 0f);
            
            AddTimer(PISTOLATTACK_K, 0.30f);
            AddCurrentTime(PISTOLATTACK_K, 0f);
            
            AddTimer(PISTOLCOOLDOWN_K, 0.35f);
            AddCurrentTime(PISTOLCOOLDOWN_K, 0f);
            
            AddTimer(SMGATTACK_K, 0.10f);
            AddCurrentTime(SMGATTACK_K, 0f);
            
            AddTimer(SMGCOOLDOWN_K, 0.15f);
            AddCurrentTime(SMGCOOLDOWN_K, 0f);
            
            AddTimer(RELOADTIME_K, 1f);
            AddCurrentTime(RELOADTIME_K, 0f);
            
                        
            //Enemies stuff
            AddTimer(ENEMYSPAWN_K, 10f);
            AddCurrentTime(ENEMYSPAWN_K, 0f);

            //Timer for tips
            AddTimer(TIP_K, 3f);
            AddCurrentTime(TIP_K, 0f);


            Values.SetIsTimerLoaded(true);


        }
        
        /*Getters*/
        
        public static Dictionary<string, float> GetTimerLength()
        {
            return timerLength;
        }
        public static Dictionary<string, float> GetCurrentTime()
        {
            return currentTime;
        }
        
        /*Timer Management*/
        private static void InitializeTimerLength()
        {
            timerLength = new Dictionary<string, float>();
        }
        private static void InitializeCurrentTime()
        {
            currentTime = new Dictionary<string, float>();
        }

        public static void AddTimer(string key, float value)
        {

            try
            {
                timerLength.Add(key, value);

            }
            catch (ArgumentException)
            {
                //Debug.Log("Already added timer");
            }
        }
        public static void AddCurrentTime(string key, float value)
        {
            try
            {
                currentTime.Add(key, value);

            }
            catch (ArgumentException)
            {
                //Debug.Log("Already added timer (Current)");
            }
        }

        public static void ResetTimer(string key)
        {
            currentTime[key] = timerLength[key];
        }
        public static void RunTimer(string key)
        {
            if (currentTime[key] - Time.deltaTime >= 0)
            {
                currentTime[key] -= Time.deltaTime;
            }
            else
            {
                currentTime[key] = 0;
            }
        }

        public static void DeleteTimer(string key)
        {
            currentTime.Remove(key);
        }
    }

}