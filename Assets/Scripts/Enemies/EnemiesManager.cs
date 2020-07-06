    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

namespace Enemies
{
    public class EnemiesManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public List<GameObject> GetAllEnemies()
        {
            return GameObject.FindGameObjectsWithTag("enemy").ToList();
        }
    }
}
