using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform enemyPrefab;
        
        // Start is called before the first frame update
   


        public void Spawn()
        { 
            //Deve spawnarlo in una zona predeterminata. 
            //todo inserire elenco coordinate accettabili o qualcosa del genere
            Instantiate(enemyPrefab);       
        }
    }
}
