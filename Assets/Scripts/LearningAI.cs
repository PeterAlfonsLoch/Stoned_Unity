using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningAI : MonoBehaviour {

    public GameObject target;
    public float acceptableRange = 3f;//how far from the target it is allowed to be
    public float speed;//how fast it can move
    public int situationCount = 0;//how many situations it remembers

    private List<Situation> situations = new List<Situation>();
    private Situation prevSituation;//the situation from the previous frame

    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();		
	}
	
	// Update is called once per frame
	void Update () {
        situationCount = situations.Count;
        if (!achievedGoal())
        {
            bool addNewSituation = true;
            Situation now = new Situation(transform.position, target.transform.position,rb2d.velocity,distanceToGoal());
            if (prevSituation != null)
            {
                prevSituation.followingSituation = now;
            }
            Situation mostLike = findMostLike(now);
            if (mostLike == null || mostLike.successDegree() <= 0)
            {
                Vector2 randomDir = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
                float randomSpeed = Random.Range(0, speed);
                //Debug.Log("randomDir: " + randomDir);
                now.movedDirection = randomDir;
                now.speed = randomSpeed;
                rb2d.AddForce(randomDir * randomSpeed);
            }
            else
            {
                now.movedDirection = mostLike.movedDirection;
                now.speed = mostLike.speed;
                rb2d.AddForce(now.movedDirection * now.speed);
                if (mostLike.matchDegree(now) > 0.999f)
                {//only add new situation if it's unique enough from the one it's most like
                    addNewSituation = false;
                }
            }
            if (addNewSituation)
            {
                situations.Add(now);
            }
            prevSituation = now;
        }
        cleanSituations();
	}

    /// <summary>
    /// How close it is to achieving its goal
    /// </summary>
    /// <returns></returns>
    float distanceToGoal()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        //Debug.Log("distance: " + (100 - 100*Mathf.InverseLerp(0, 100, distance)));
        return distance;
    }
    /// <summary>
    /// Returns whether or not this AI has achieved its goal
    /// </summary>
    /// <returns></returns>
    bool achievedGoal()
    {
        return distanceToGoal() <= acceptableRange || rb2d.velocity.magnitude > 0.01f;
    }

    Situation findMostLike(Situation current)
    {
        //Closest
        Situation closest = null;
        float closestMatchDegree = 0;
        //Most Successful
        float highestSuccessValue = 0;
        Situation mostSuccessful = null;
        float successfulmatchDegree = 0;
        foreach (Situation s in situations)
        {
            float degree = s.matchDegree(current);
            if (degree > closestMatchDegree)
            {
                if (s.successDegree() < highestSuccessValue)
                {//if this situation was less successful than the prev one
                    if (successfulmatchDegree + 0.1 > degree)
                    {//if the more successful one is relatively close in matching
                        continue;//skip this one
                    }
                }
                closestMatchDegree = degree;
                closest = s;
                if (s.successDegree() > 0)
                {
                    highestSuccessValue = s.successDegree();
                    mostSuccessful = s;
                    successfulmatchDegree = degree;
                }
            }
        }
        return closest;
    }

    /// <summary>
    /// Deletes the least successful situation
    /// </summary>
    void cleanSituations()
    {
        Situation leastSuccessful = null;
        float lowestSuccess = 0;
        foreach (Situation s in situations)
        {
            float success = s.successDegree();
            if (success < lowestSuccess)
            {
                lowestSuccess = success;
                leastSuccessful = s;
            }
        }
        if (leastSuccessful != null)
        {
            leastSuccessful.followingSituation = null;
            situations.Remove(leastSuccessful);
        }
    }
        
}

class Situation
{
    //Situation
    public Vector3 position;
    public Vector3 targetPosition;
    public Vector3 velocity;
    public float distanceToGoal;
    //Response
    public Vector3 movedDirection;
    public float speed;
    public Situation followingSituation;
    private float success;//the amount of success the response had

    public Situation(Vector3 pos, Vector3 tarPos, Vector3 vel, float dist)
    {
        position = pos;
        targetPosition = tarPos;
        velocity = vel;
        distanceToGoal = dist;
    }

    /// <summary>
    /// Returns the degree to which this situation matches the given situation
    /// A higher number means a higher degree of matching
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public float matchDegree(Situation other)
    {
        float posMatch = matchDegreeDistance(position, other.position);
        float tarPosMatch = matchDegreeDistance(targetPosition, other.targetPosition);
        float velMatch = matchDegreeDistance(velocity, other.velocity);
        float distMatch = matchDegreeFloat(distanceToGoal, other.distanceToGoal);
        return posMatch * tarPosMatch * velMatch * distMatch;
    }

    public float matchDegreeDistance(Vector3 pos, Vector3 pos2)
    {
        return (1 - Mathf.InverseLerp(0, 1, Vector3.Distance(pos, pos2)));
    }
    public float matchDegreeFloat(float f, float f2)
    {
        return (1 - Mathf.InverseLerp(0, 1, Mathf.Abs(f - f2)));
    }

    /// <summary>
    /// The extent to which this succeeded.
    /// Positive values indicate success.
    /// Negative values indicate failure.
    /// </summary>
    /// <returns></returns>
    public float successDegree()
    {
        //Debug.Log("Success: " + Mathf.Round(success*100));
        if (success != 0)
        {
            return success;
        }
        if (followingSituation != null)
        {
            success = distanceToGoal - followingSituation.distanceToGoal;
        }
        return success;
    }
}
