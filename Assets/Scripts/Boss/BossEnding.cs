using UnityEngine;

namespace Boss
{
    public class BossEnding : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

            this.transform.position = new Vector3(0, 0, 0);        //lo sposta in un punto randomico
            enabled = false;
        }

        // Update is called once per frame

    }
}
