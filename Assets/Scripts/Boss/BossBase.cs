using UnityEngine;

namespace Boss
{
    public class BossBase

    {

        private int movementSpeed;
        private int projectileSpeed, projectileSpawnRate;
        

        private int health;

        // Start is called before the first frame update

        public BossBase(int maxHealth)
        {
            health = maxHealth;
        }


        public void SetHealth(int h)
        {
            this.health = h;
        }

        public void SetProjectileSpawnRate(int rate)
        {
            this.projectileSpawnRate = rate;
        }

        public void SetProjectileSpeed(int speed)
        {
            this.projectileSpeed = speed;
        }
        
        public int GetHealth()
        {
            return health;
        }
    }
}
