using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlowVFX : MonoBehaviour
{
    [SerializeField] Light2D blowLight;
    ParticleSystem particle;
    ParticleSystem.MainModule mainModule;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        mainModule = particle.main;
    }

    void Update()
    {
        if (blowLight == null) return;
        float lifetimeFraction = 1f - (particle.time / mainModule.startLifetime.constant);
        blowLight.intensity = Mathf.Lerp(0, blowLight.intensity, lifetimeFraction);
    }
}
