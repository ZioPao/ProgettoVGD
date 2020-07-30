using UnityEditor;
using UnityEngine;

namespace Boss
{
    public class EndLevelTrigger : MonoBehaviour
    {
    
        [SerializeField] private int nextLevelId;
        [SerializeField] private GameObject currentLevel;


        private void OnTriggerEnter(Collider other)
        {

            print("will change level");
            if (other.name == "Player")
            {
                var level = Resources.Load<GameObject>("Prefabs/Levels/Level" + nextLevelId);
                var player = GameObject.Find("Player");

                PrefabUtility.InstantiatePrefab(level);

                //player position 
                switch (nextLevelId)
                {
                    case (2):
                        player.transform.position = new Vector3(16.51f, 26.632f, 13.16f);
                        break;
                    case (3):
                        player.transform.position = new Vector3(151, 55,45);
                        break;
                }

                Destroy(currentLevel); //Destroy the old level at the end, killing the script    }
            }
       
        }
    }
}