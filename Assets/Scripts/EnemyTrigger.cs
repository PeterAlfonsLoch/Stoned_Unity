﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour {

    [Tooltip("Define it in the editor, or leave it blank to be auto-populated by the enemies in the trigger area")]
    public List<EnemySimple> enemies;

    void Start()
    {
        if (enemies.Count == 0)
        {
            RaycastHit2D[] rch2ds = new RaycastHit2D[100];
            GetComponent<BoxCollider2D>().Cast(Vector2.zero, rch2ds, 0, true);
            foreach (RaycastHit2D rch2d in rch2ds)
            {
                if (rch2d && !rch2d.collider.isTrigger)
                {
                    EnemySimple es = rch2d.collider.gameObject.GetComponent<EnemySimple>();
                    if (es != null)
                    {
                        enemies.Add(es);
                    }
                }
            }
        }
        foreach (EnemySimple es in enemies)
        {
            es.activeMove = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            foreach (EnemySimple es in enemies)
            {
                es.activeMove = true;
            }
        }
    }
}
