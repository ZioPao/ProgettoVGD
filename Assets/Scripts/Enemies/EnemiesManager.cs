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

        public GameObject[] GetAllEnemies()
        {
            return GameObject.FindGameObjectsWithTag("enemy");
        }
    }
}
