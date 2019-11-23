using System.Collections;
using UnityEngine;

public class BossSphere : Enemy
{
    public GameObject particle;
    public BossCrystal.Balloon balloon;
    public override void Damaged()
    {
        StartCoroutine(Destroying());
    }

    IEnumerator Destroying()
    {
        Vector3 stpos = transform.position;
        float dur = Vector3.Distance(stpos, balloon.crystal.transform.position) / 15f;

        for(float timer = 0; timer < dur; timer += Time.deltaTime)
        {
            float x = timer / dur;
            transform.position = Vector3.Lerp(stpos, balloon.crystal.transform.position, x * x * x);
            yield return null;
        }

        Instantiate(GameManager.inst.hitParticle, transform.position, Quaternion.identity).GetComponent<HitParticle>().SetColor(visionType);

        balloon.Destroyed();
    }

    public override void Start()
    {

    }

    public override void Update()
    {
        
    }
}
