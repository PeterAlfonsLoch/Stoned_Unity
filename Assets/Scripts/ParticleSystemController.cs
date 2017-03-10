﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour {
    //2017-03-09 used to control an object with a ParticleSystem thats used for circular ranges

    private ParticleSystem teleportParticles;

    // Use this for initialization
    void Start () {
        teleportParticles = GetComponent<ParticleSystem>();
        activateTeleportParticleSystem(false);
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}

    public void activateTeleportParticleSystem(bool activate)
    {
        activateTeleportParticleSystem(activate, teleportParticles.main.startColor.color, transform.position, teleportParticles.shape.radius);
    }
    /// <summary>
    /// Activates the gesture particle system (used mainly for force wave)
    /// </summary>
    /// <param name="activate"></param>
    /// <param name="effectColor"></param>
    /// <param name="pos">Position in world coordinates, not local coordinates</param>
    /// <param name="radius"></param>
    public void activateTeleportParticleSystem(bool activate, Color effectColor, Vector3 pos, float radius)
    {
        if (activate)
        {
            teleportParticles.Play();
            //Position
            teleportParticles.transform.position = pos;
            //Range
            ParticleSystem.ShapeModule pssm = teleportParticles.shape;
            pssm.radius = radius;
            //Color
            ParticleSystem.MainModule psmm = teleportParticles.main;
            psmm.startColor = effectColor;
            //Lifetime
            ParticleSystem.MinMaxCurve psmmc = teleportParticles.main.startLifetime;
            psmmc.constant = radius / Mathf.Abs(teleportParticles.main.startSpeed.constant);
            psmm.startLifetime = psmmc;
        }
        else
        {
            teleportParticles.Pause();
            teleportParticles.Clear();
        }
    }
}
