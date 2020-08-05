using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class KeyScript : MonoBehaviour, IInteractableMidGame
{

    public void InteractableBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public void ForceActivation()
    {
        Values.SetHasKey(true);
    }
}
