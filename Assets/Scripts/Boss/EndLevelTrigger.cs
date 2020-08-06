using System.Collections;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Boss
{
    //non è necessario utilizzare l'interfaccia in quanto non dobbiamo salvare lo status
    //di questo trigger, in quanto lo si esegue una sola volta a fine livello e poi si cambia scena
    public class EndLevelTrigger : MonoBehaviour
    {
    
        [SerializeField] private int nextLevelId;
        [SerializeField] private GameObject currentLevel;

        private GameObject oldPlayer;
        private bool alreadyExecuted = false;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Values.PlayerTag) && !alreadyExecuted)
            {
                alreadyExecuted = true;
                 //Saves the old player
                oldPlayer = GameObject.FindWithTag(Values.PlayerTag);
                oldPlayer.name = Values.OldPlayerName;

                DontDestroyOnLoad(oldPlayer);
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
                }
                yield return null;
            }
        }
    }
    
    
}