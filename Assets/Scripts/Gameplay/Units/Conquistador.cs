using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Conquistador : MonoBehaviour {

    [Header("References")]
    public Animator MyAnimator;
    public GameObject AI_Brain;
    public GameObject Selection;

    [Header("Asset")]
    public GameObject Corpse;

    bool _destroyed = false;

    public void SpawnCorpse()
    {
        Instantiate(Corpse, transform.position, transform.rotation);
    }

    public void KillMe ()
    {
        if(!_destroyed)
        {
            if (MyAnimator)
            {
                MyAnimator.SetTrigger("Dead");
                GetComponent<NavMeshAgent>().isStopped = true;

                tag = "Untagged";
                Selection.tag = "Untagged";

                Destroy(AI_Brain);
                Destroy(GetComponent<Animator>());
                Destroy(GetComponent<AIBrain>());
                _destroyed = true;
            }
            else
            {
                SpawnCorpse();
                Destroy(gameObject);
            }
        }
    }
}
