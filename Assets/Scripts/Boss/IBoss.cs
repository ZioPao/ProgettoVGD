using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoss
{
    void SetPhase();

    void FindPathUnlocker();
    void ActivatePathUnlocker();

}
