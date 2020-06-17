using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWater : MonoBehaviour
{
    Terrain water;
    // Start is called before the first frame update
    void Start()
    {
        water = GetComponent<Terrain>();

    }

    private void FixedUpdate()
    {

        var layerTmp = water.terrainData.terrainLayers.GetValue(0);
        print("we've got it");
    }
}
