using System.Collections;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Boss
{
    public class EndLevelTrigger : MonoBehaviour
    {
    
        [SerializeField] private int nextLevelId;
        [SerializeField] private GameObject currentLevel;

        private GameObject oldPlayer;


        private void OnTriggerEnter(Collider other)
        {

            print("will change level");
            if (other.name == "Player")
            {
                 //Saves the old player
                oldPlayer = GameObject.Find("Player");
                 oldPlayer.name = "oldPlayer";        //per evitare conflitti
                // DontDestroyOnLoad(oldPlayer);
                Values.SetIsChangingScene(true);
                StartCoroutine(ChangeScene());
            }
       
        }
        
        IEnumerator ChangeScene()
        {
            yield return null;

  
            
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextLevelId, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                    yield return new WaitUntil(() =>  GameObject.Find("Player") != null);
                    
                    Values.SetIsChangingScene(false);
                    
                    //Reset values of player
                    
                    
                  //Set correct values
                  ////player position 
                    // switch (nextLevelId)
                    // {
                    //     case (2):
                    //         oldPlayer.transform.position = new Vector3(16.51f, 26.632f, 13.16f);
                    //         break;
                    //     case (3):
                    //         oldPlayer.transform.position = new Vector3(151, 55,45);
                    //         break;
                    // }
                    
                    //Set 
                }


                yield return null;
            }
           

        }

        
    }
    
    
}