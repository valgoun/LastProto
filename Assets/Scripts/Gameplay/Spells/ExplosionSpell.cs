using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion Spell", menuName = "Spell/Explosion")]
public class ExplosionSpell : Spell {

    [Header("Explosion Spell")]
    public float Delay;
    public GameObject ExplosionPrefab;
    public GameObject UIPrefab;

    public override void StartCasting()
    {

    }

    public override void StopCasting()
    {

    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        SpellManager.Instance.StartCoroutine(ExplosionRoutine(target, Delay));
        GameObject ui = Instantiate(UIPrefab, target.transform);
        ui.GetComponent<TimerUI>().Initialize(Delay);
    }

    private IEnumerator ExplosionRoutine (GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target)
        {
            Instantiate(ExplosionPrefab, target.transform.position, target.transform.rotation);
            if (target.tag == "Aztec")
            {
                target.GetComponent<Unit>().Killed();
            }
            else
                Destroy(target);
        }
    }
}
