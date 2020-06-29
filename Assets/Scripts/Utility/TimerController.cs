using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Player;
using UnityEngine;

namespace Utility
{
    public static class TimerController
    {
        
        //Timer Types
        public enum TimerEnum
        {
            KnifeAttack,
            PistolAttack,
            SMGAttack,
        }

        //Timer Durations
        private static Dictionary<TimerEnum, float> timerLength;
        private static Dictionary<TimerEnum, float> currentTime;
        
        public static void Setup()
        {
            InitializeTimerLength();
            InitializeCurrentTime();

            AddTimer(TimerEnum.KnifeAttack, 3f);
            AddCurrentTime(TimerEnum.KnifeAttack, 0f);
            
            AddTimer(TimerEnum.PistolAttack, 0.35f);
            AddCurrentTime(TimerEnum.PistolAttack, 0f);
            
            AddTimer(TimerEnum.SMGAttack, 0.75f);
            AddCurrentTime(TimerEnum.SMGAttack, 0f);
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