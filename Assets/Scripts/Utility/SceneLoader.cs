using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public static class SceneLoader
    {
        // Start is called before the first frame update
        public static IEnumerator LoadScene(string path)
        {

            yield return null;        //Wait for button controller to finish whatever it needs to do
            
            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    #if UNITY_EDITOR
                        yield return new WaitForSeconds(1);
                    #endif
                    asyncOperation.allowSceneActivation = true;
                } //spawnPoint = new Vector3(145, 67, 40); //level 3

                Time.timeScale = 1;
                yield return null;
            }
        }
    }
}