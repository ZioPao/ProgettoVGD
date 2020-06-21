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


    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
            isPlayerVisible = true;
        
            
    }


    private void OnCollisionExit(Collision collision)
    {
        if (isPlayerVisible && collision.gameObject.CompareTag("Player"))
        {
            isPlayerVisible = false;

        }
    }

    private void generateVisibilityCone()
    {
        visibilityConeMesh = GetComponent<MeshFilter>().mesh;

        coneVertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
                                        new Vector3(0,12,0), new Vector3(0,12,1),new Vector3(1,12,0) };
       
        triangles = new int[] { 0,1,2,0,1,2 };

        visibilityConeMesh.Clear();
        visibilityConeMesh.vertices = coneVertices;
        visibilityConeMesh.triangles = triangles;
    }
}
