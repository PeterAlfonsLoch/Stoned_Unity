using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour {

    GameObject player;
    public GameObject teleportParticleEffects;
    protected ParticleSystemController particleController;
    public Color effectColor;//the color used for the particle system upon activation

    public GameObject abilityIndicatorParticleEffects;

    // Use this for initialization
    protected void Start () {
        player = gameObject;
        particleController = teleportParticleEffects.GetComponent<ParticleSystemController>();
        if (abilityIndicatorParticleEffects != null)
        {
            abilityIndicatorParticleEffects.GetComponent<ParticleSystem>().Play();
        }
    }

    public bool effectsGroundCheck()
    {
        return false;
    }

    public bool effectsAirPorts()
    {
        return false;
    }

    public bool takesGesture()
    {
        return false;
    }

    public bool takesHoldGesture()
    {
        return true;
    }

    public void processHoldGesture(Vector3 pos, float holdTime, bool finished)
    {

    }

}
