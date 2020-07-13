using System;
using System.Numerics;
using Saving;

namespace Utility
{
    [Serializable]
    public class EnemySpawnerStatus
    {
        private string spawnerName;
        private SerializableVector3 position;
        private SerializableQuaternion rotation;
        
        private int counter;        //Numer of enemies spawned in all spawner life
        private int spawnedEnemiesCount;
        private int maxEnemiesSpawned, maxEnemiesConcurrently;

        public void Setup(string spawnerName, int counter, int maxEnemiesSpawned, int maxEnemiesConcurrently)
        {
            this.spawnerName = spawnerName;
            this.counter = counter;
            this.maxEnemiesSpawned = maxEnemiesSpawned;
            this.maxEnemiesConcurrently = maxEnemiesConcurrently;
        }

        public void SaveTransform(SerializableVector3 position, SerializableQuaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
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

        public void AddOneToCounter()
        {
            counter++;
        }

        public int GetMaxEnemiesSpawned()
        {
            return maxEnemiesConcurrently;
        }

        public int GetMaxEnemiesConcurrently()
        {
            return maxEnemiesConcurrently;
        }

        public string GetSpawnerName()
        {
            return spawnerName;
        }

        public SerializableVector3 GetPosition()
        {
            return position;
        }

        public SerializableQuaternion GetRotation()
        {
            return rotation;
        }
        
    }
}
