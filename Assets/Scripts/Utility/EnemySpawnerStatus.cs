using System;

namespace Utility
{
    [Serializable]
    public class EnemySpawnerStatus
    {
        private int counter;        //Numer of enemies spawned in all spawner life
        private int spawnedEnemiesCount;
        private int maxEnemiesSpawned, maxEnemiesConcurrently;

        public void Setup(int counter, int maxEnemiesSpawned, int maxEnemiesConcurrently)
        {
            this.counter = counter;
            this.maxEnemiesSpawned = maxEnemiesSpawned;
            this.maxEnemiesConcurrently = maxEnemiesConcurrently;
        }

        public void SetSpawnedEnemiesCount(int value)
        {
            this.spawnedEnemiesCount = value;
        }

        public int GetSpawnedEnemiesCount()
        {
            return spawnedEnemiesCount;
        }
        
        
        public int GetCounter()
        {
            return counter;
        }

        public int GetMaxEnemiesSpawned()
        {
            return maxEnemiesConcurrently;
        }

        public int GetMaxEnemiesConcurrently()
        {
            return maxEnemiesConcurrently;
        }
        
    }
}
