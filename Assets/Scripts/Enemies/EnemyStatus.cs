using System;

namespace Enemies
{
    [Serializable]
    public class EnemyStatus
    {

        //Base
        private int maxHealth, health;
        private bool isHit;
        public void SetupBase(int maxHealth, int health, bool isHit)
        {
            this.maxHealth = maxHealth;
            this.health = health;
            this.isHit = isHit;
        }

        public int GetHealth()
        {
            return health;
        }

        public void SetHealth(int health)
        {
            this.health = health;
        }

        public void ModifyHealth(int mod)
        {
            this.health += mod;
        }

        public bool GetIsHit()
        {
            return isHit;
        }
        public void SetIsHit(bool value)
        {
            isHit = value;
        }
        
        //intelligence
        private int viewDistance;
        private bool isPlayerInView, isStopped;
        private float maxMemoryTime, memoryTimeLeft, waitingTimeLeft;
        
        public void SetupIntelligence(int viewDistance, bool isPlayerInView, bool isStopped, float maxMemoryTime, float memoryTimeLeft, float waitingTimeLeft)
        {
            this.viewDistance = viewDistance;
            this.isStopped = isStopped;
            this.isPlayerInView = isPlayerInView;
            this.maxMemoryTime = maxMemoryTime;
            this.memoryTimeLeft = memoryTimeLeft;
            this.waitingTimeLeft = waitingTimeLeft;
        }

        public void SetIsStopped(bool value)
        {
            isStopped = value;
        }

        public void SetIsPlayerInView(bool value)
        {
            isPlayerInView = value;
        }

        public bool GetIsPlayerInView()
        {
            return isPlayerInView;
        }

        public float GetWaitingTimeLeft()
        {
            return waitingTimeLeft;
        }
        public void ModifyWaitingTimeLeft(float value)
        {
            waitingTimeLeft += value;
        }
        
        //Movement
        private float maxTimerAlternativeMovement, timerAlternativeMovement, minPlayerDistance;
        public void SetupMovement(float maxTimerAlternativeMovement, float timerAlternativeMovement, float minPlayerDistance)
        {
            this.maxTimerAlternativeMovement = maxTimerAlternativeMovement;
            this.timerAlternativeMovement = timerAlternativeMovement;
            this.minPlayerDistance = minPlayerDistance;
        }

        public float GetTimerAlternativeMovement()
        {
            return timerAlternativeMovement;
        }

        public void SetTimerAlternativeMovement(float value)
        {
            timerAlternativeMovement = value;
        }
        public void ModifyTimerAlternativeMovement(float mod)
        {
            this.timerAlternativeMovement += mod;
        }
    }
}
