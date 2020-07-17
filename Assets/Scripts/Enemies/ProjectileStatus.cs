using System;
using Saving;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class ProjectileStatus
    {
        private float projectileTimeLeft;
        private int projectileSpeed;
        //private Transform enemyTransform;
        //private Transform spriteTransform;


        private SerializableVector3 position;
        private SerializableQuaternion rotation;

        public ProjectileStatus(float projectileTimeMax, int projectileSpeed, GameObject gameObject)
        {
            projectileTimeLeft = projectileTimeMax;
            this.projectileSpeed = projectileSpeed;
            //spriteTransform = gameObject.transform.Find("Sprite");
        
        }

        public float GetProjectileTimeLeft()
        {
            return projectileTimeLeft;
        }

        public int GetProjectileSpeed()
        {
            return projectileSpeed;
        }
        public SerializableVector3 GetPosition()
        {
            return position;
        }

        public SerializableQuaternion GetRotation()
        {
            return rotation;
        }
        // public Transform GetSpriteTransform()
        // {
        //     return spriteTransform;
        // }
        public void DecreaseTimer()
        {
            projectileTimeLeft -= Time.deltaTime;
        }

        public void SetTransform(Transform t)
        {
            position = t.position;
            rotation = t.rotation;
        }


    }
}
