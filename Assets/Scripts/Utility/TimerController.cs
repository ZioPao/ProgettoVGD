using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class TimerController
    {
        
        //Timer Types
        public enum TimerEnum
        {
            KnifeAttack,
            KnifeCooldown,
            PistolAttack,
            PistolCooldown,
            SMGAttack,
            SMGCooldown,
            ReloadTime,
            EnemySpawn
        }

        //Timer Durations
        private static Dictionary<TimerEnum, float> timerLength;
        private static Dictionary<TimerEnum, float> currentTime;
        
        public static void Setup()
        {
            InitializeTimerLength();
            InitializeCurrentTime();

            AddTimer(TimerEnum.KnifeAttack, 0.70f);
            AddCurrentTime(TimerEnum.KnifeAttack, 0f);
            
            AddTimer(TimerEnum.KnifeCooldown, 0.75f);
            AddCurrentTime(TimerEnum.KnifeCooldown, 0f);
            
            AddTimer(TimerEnum.PistolAttack, 0.30f);
            AddCurrentTime(TimerEnum.PistolAttack, 0f);
            
            AddTimer(TimerEnum.PistolCooldown, 0.35f);
            AddCurrentTime(TimerEnum.PistolCooldown, 0f);
            
            AddTimer(TimerEnum.SMGAttack, 0.10f);
            AddCurrentTime(TimerEnum.SMGAttack, 0f);
            
            AddTimer(TimerEnum.SMGCooldown, 0.15f);
            AddCurrentTime(TimerEnum.SMGCooldown, 0f);
            
            AddTimer(TimerEnum.ReloadTime, 1.5f);
            AddCurrentTime(TimerEnum.ReloadTime, 0f);
            
                        
            AddTimer(TimerEnum.EnemySpawn, 5f);
            AddCurrentTime(TimerEnum.EnemySpawn, 0f);

        }
        
        /*Getters*/
        
        public static Dictionary<TimerEnum, float> GetTimerLength()
        {
            return timerLength;
        }
        public static Dictionary<TimerEnum, float> GetCurrentTime()
        {
            return currentTime;
        }
        
        /*Timer Management*/
        private static void InitializeTimerLength()
        {
            timerLength = new Dictionary<TimerEnum, float>();
        }
        private static void InitializeCurrentTime()
        {
            currentTime = new Dictionary<TimerEnum, float>();
        }

        private static void AddTimer(TimerEnum key, float value)
        {
            timerLength.Add(key, value);
        }
        private static void AddCurrentTime(TimerEnum key, float value)
        {
            currentTime.Add(key, value);
        }

        public static void ResetTimer(TimerEnum key)
        {
            currentTime[key] = timerLength[key];
        }
        public static void RunTimer(TimerEnum key)
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
        
    }

}