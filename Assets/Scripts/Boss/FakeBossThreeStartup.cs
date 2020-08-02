using UnityEngine;

namespace Boss
{
    public class FakeBossThreeStartup : MonoBehaviour
    {
        private FinalBossSpawner checker;
    
    
        //todo un trigger fa partire lo FakeSpawn, che crea un fake boss
        //todo il fake boss triggera la partenza del checker
        //todo il checker controlla che il fake boss sia morto o meno
    
        //todo 
        void Awake()
        {
            //Deve fare l'init al FinalBossSpawner, che determina la gestione dello spawn del boss finale
            checker = GameObject.Find("BossSpawner").GetComponent<FinalBossSpawner>();
            checker.enabled = true;
        }

  
    }
}
