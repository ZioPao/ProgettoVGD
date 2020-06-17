using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingScript : MonoBehaviour

{
    MeshCollider meshCollider;
    Mesh visibilityConeMesh;

    Vector3[] coneVertices;
    int[] triangles;
    public bool isPlayerVisible;

    // Start is called before the first frame update
    void Start()
    {
        generateVisibilityCone();

        meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = visibilityConeMesh;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }


    void OnTriggerEnter(Collider a)
    {
        isPlayerVisible = a.gameObject.name.Equals("Player");
            
    }


    private void OnTriggerExit(Collider a)
    {
        if (isPlayerVisible && a.gameObject.name.Equals("Player"))
        {
            isPlayerVisible = false;

        }
    }

    private void generateVisibilityCone()
    {
        visibilityConeMesh = GetComponent<MeshFilter>().mesh;

        coneVertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0) };
        triangles = new int[] { 0, 1, 2 };

        visibilityConeMesh.Clear();
        visibilityConeMesh.vertices = coneVertices;
        visibilityConeMesh.triangles = triangles;
    }
}
