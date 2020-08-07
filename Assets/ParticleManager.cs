using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    
    void Start()
    {
        particles.Play();
    }

    void Update()
    {
        if (particles.isStopped && particles.particleCount == 0)
            Destroy(gameObject);
    }
}