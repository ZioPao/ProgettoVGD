﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoss
{
    void ChangePhase();

    void FindPathUnlocker();
    void ActivatePathUnlocker();

}