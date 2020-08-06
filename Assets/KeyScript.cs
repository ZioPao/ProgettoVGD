using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class KeyScript : MonoBehaviour, IInteractableMidGame
{

    [SerializeField] private GameObject linkedDoor;
    public void InteractableBehaviour()
    {
        //totally useless
        throw new System.NotImplementedException();
    }

    public void ForceActivation()
    {

        if (!linkedDoor.CompareTag("InteractableOver"))
        {
            //todo necessario check aggiuntivo con la porta alla quale è collegata 
            Values.SetHasKey(true);
        }
        else
        {
            Values.SetHasKey(false);
        }

    }

    public bool GetIsEnabled()
    {
        return enabled;
    }
}
