using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBody : Enemy
{
    public float speedInc, initSpeed;
    public BossCrystal[] crystals;
    public int clCryst;
    Coroutine patCor;

    public void CrystalDestroyed(BossCrystal crys)
    {
        --clCryst;
    }
    public override void Update()
    {
        if (!GameManager.inst.gameOver)
        {
            transform.Translate(Vector3.Normalize(GameManager.inst.player.position - transform.position) * speed, Space.World);
            transform.LookAt(GameManager.inst.player);
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        }
    }
    IEnumerator BossPatern()
    {
        Vector3 stVec, edVec;
        Animator anit = GetComponent<Animator>();
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();
        rb.isKinematic = true;
        col.enabled = false;
        speed = 0;

        
        stVec = new Vector3(0, -10, 20); 
        edVec = new Vector3(0, 0, 20);
        for(float timer = 0; timer <= 3f; timer += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(stVec, edVec, timer / 3f);
            yield return null;
        }
        transform.position = edVec;

        rb.isKinematic = false;
        GetComponent<Collider>().enabled = true;
        anit.SetTrigger("start");


        while(clCryst > 0)
        {
            speed = initSpeed + speedInc * (6 - clCryst) / 2;
            anit.SetFloat("speed", speed * 50);

            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.material.SetInt("_MaskType", 0);
                r.material.SetColor("_Color", new Color(1f, 1f - 0.25f * (crystals.Length - clCryst) / 2f, 1f - 0.25f * (crystals.Length - clCryst) / 2f));
                r.material.SetInt("_StencilComp", 0);
                if (r.gameObject.tag.Contains("HeartFill")) r.material.SetColor("_Color", new Color(1, 0f, 0.8f));
            }

            foreach (BossCrystal b in crystals) { b.StartPhase((6 - clCryst) / 2 + 1); }

            int befCryst = clCryst;
            while (clCryst > befCryst - 2) yield return null;



            foreach (BossCrystal b in crystals) b.EndPhase();

            stVec = transform.position;
            edVec = new Vector3(0, 0, 20);

            for (float timer = 0; timer <= 1f; timer += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(stVec, edVec, timer);
                yield return null;
            }
            transform.position = edVec;

            if (clCryst <= 0)
            {
                Vector3 pos = transform.position;
                int parCnt = 0;
                float viTmr = 0, parTmr = 0;
                while (parCnt < 10)
                {
                    viTmr += Time.deltaTime;
                    parTmr += Time.deltaTime;
                    if(viTmr >= 0.05f)
                    {
                        transform.position = pos + Random.insideUnitSphere;
                        viTmr = 0;
                    }
                    if(parTmr >= 0.5f)
                    {
                        Vector3 parPos = new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(2f, 8f), transform.position.z - 0.2f);
                        Instantiate(GameManager.inst.hitParticle, parPos, Quaternion.identity);
                        parTmr = 0;
                        parCnt++;
                    }
                    yield return null;
                }

                GameObject p = Instantiate(GameManager.inst.hitParticle, transform.position, Quaternion.identity);
                p.transform.localScale = new Vector3(3, 3, 3);

                Instantiate(GameManager.inst.BossHead, transform.position, Quaternion.identity);
                Destroy(gameObject);
                /*GameManager.inst.gameOver = true;
                StartCoroutine(GameManager.inst.StageTextShow("CLEAR", -1, ""));
                anit.SetTrigger("death");
                yield break;*/
            }
        }
    }
    public override void Start()
    {
        clCryst = crystals.Length;
        
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_MaskType", 0);
            r.material.SetColor("_Color", Constants.Vision_Color(VisionType.White));
            r.material.SetInt("_StencilComp", 0);
            if (r.gameObject.tag.Contains("HeartFill")) r.material.SetColor("_Color", new Color(1, 0f, 0.8f));
        }

        patCor = StartCoroutine(BossPatern());

        foreach (BossCrystal b in crystals) b.boss = this;
    }

    public override void GameOver()
    {
        StopCoroutine(patCor);
        foreach (BossCrystal b in crystals)
        {
            b.GameOver();
        }
        if (GetComponent<Animator>()) GetComponent<Animator>().enabled = false;
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void Damaged()
    {

    }
}
