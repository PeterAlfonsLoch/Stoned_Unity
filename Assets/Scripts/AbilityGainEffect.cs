using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGainEffect : MonoBehaviour {

    public float animSpeed = 0;//arc degrees per second
    public float animTime = 1.0f;//how many seconds to complete one section of anim
    private new ParticleSystem particleSystem;
    private float originalEmission;
    private float originalArc;
    private float originalStartLifetime;
    private Quaternion originalQuat;

    private float arcEmissionRatio;

	// Use this for initialization
	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        originalEmission = particleSystem.emission.rateOverTime.constant;
        originalArc = particleSystem.shape.arc;
        originalQuat = particleSystem.gameObject.transform.localRotation;
        originalStartLifetime = particleSystem.main.startLifetime.constant;
        arcEmissionRatio = originalEmission / originalArc;
        setArc(0);
        if (animSpeed == 0)
        {
            animSpeed = 360 / animTime;
        }
        ParticleSystem.MainModule psmm = particleSystem.main;
        psmm.startLifetime = 0.1f;
    }
	
	// Update is called once per frame
	void Update () {
        setArc(particleSystem.shape.arc + animSpeed * Time.deltaTime);
        if (particleSystem.shape.arc <= 0 || particleSystem.shape.arc >= 360)
        {
            animSpeed *= -1;
        }
	}

    void setArc(float newArc)
    {
        ParticleSystem.ShapeModule pssm = particleSystem.shape;
        pssm.arc = newArc;
        particleSystem.emissionRate = newArc * arcEmissionRatio * 100;
        particleSystem.gameObject.transform.localRotation = Quaternion.Euler(originalQuat.eulerAngles + new Vector3(0, 0, originalArc - newArc));
    }
}
