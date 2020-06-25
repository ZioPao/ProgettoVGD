using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [SerializeField] private float projectileTimeMax = 15f;
    [SerializeField] private int projectileSpeed = 15;

    private float projectileTimeLeft;

    
    private MeshRenderer projMeshRenderer;
    private Transform enemyTransform;

    private void Start()
    {
        projectileTimeLeft = projectileTimeMax;
        projMeshRenderer = GetComponent<MeshRenderer>();
        projMeshRenderer.material.mainTexture =
            Resources.Load<Texture2D>("Enemies/Level1/projectile");
    }

    private void FixedUpdate()
    {
        if (projectileTimeLeft > 0)
        {
            transform.position += transform.forward * Time.deltaTime * projectileSpeed;
            projectileTimeLeft -= Time.deltaTime;

        }
        else
        {
            Destroy(this);
        }

    }
}
