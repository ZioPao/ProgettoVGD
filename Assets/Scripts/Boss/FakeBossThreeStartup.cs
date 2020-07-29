using UnityEngine;

namespace Boss
{
    public class FakeBossThreeStartup : MonoBehaviour
    {
        private CheckBossSpawner checker;
    
    
        //todo un trigger fa partire lo FakeSpawn, che crea un fake boss
        //todo il fake boss triggera la partenza del checker
        //todo il checker controlla che il fake boss sia morto o meno
    
        //todo 
        void Awake()
        {
            //Deve fare l'init a CheckBossSpawner

            checker = GameObject.Find("BossSpawner").GetComponent<CheckBossSpawner>();
            checker.enabled = true;
        }

  
    }
}
