using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrystal : Enemy
{
    public GameObject crystalHeart;
    public GameObject cover;
    public GameObject spherePrefab;
    public GameObject linePrefab;
    public int maxHP;
    public Boss boss;
    public int currHP;
    bool dest;
    Vector3 heartScale, coverScale;
    VisionType currentColor;
    public Coroutine actCor;
    Collider coll;
    List<Balloon> balloons;

    public class Balloon
    {
        public GameObject sphere;
        public GameObject line;
        public VisionType visionType;
        BossCrystal crystal;
        public Balloon(GameObject sp, GameObject lp, BossCrystal _crystal)
        {
            sphere = Instantiate(sp);
            line = Instantiate(lp);
            crystal = _crystal;
            sphere.GetComponent<BossSphere>().balloon = this;
        }

        public void ChangeColor(VisionType _visionType)
        {
            visionType = _visionType;
            Renderer r = sphere.GetComponent<Renderer>();
            r.material.SetInt("_MaskType", (int)visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(visionType));
            r.material.SetInt("_StencilComp", 3);

            r = line.GetComponent<LineRenderer>();
            r.material.SetInt("_MaskType", (int)visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(visionType));
            r.material.SetInt("_StencilComp", 3);

            sphere.GetComponent<Renderer>().enabled = true;
            line.GetComponent<LineRenderer>().enabled = true;
        }

        public void GameOver()
        {
            Renderer r = sphere.GetComponent<Renderer>();
            r.material.SetInt("_MaskType", 0);
            r.material.SetInt("_StencilComp", 0);

            r = line.GetComponent<LineRenderer>();
            r.material.SetInt("_MaskType", 0);
            r.material.SetInt("_StencilComp", 0);
        }

        public void LineConnect(Transform trns)
        {
            line.GetComponent<LineRenderer>().SetPositions(new Vector3[] { trns.position, sphere.transform.position });
        }

        public void Destroyed()
        {
            if (crystal.balloons.Contains(this)) crystal.balloons.Remove(this);
            Destroy(sphere);
            Destroy(line);
        }
    }

    public override void Damaged()
    {
        currHP = Mathf.Max(currHP-1, 0);
        crystalHeart.transform.localScale = new Vector3(heartScale.x * currHP / maxHP, heartScale.y * currHP / maxHP, heartScale.z);
        Instantiate(GameManager.inst.hitParticle, transform.position, Quaternion.identity).GetComponent<HitParticle>().SetColor(new Color(1, 0, 0.8f));
        if (currHP == 0 && !dest)
        {
            dest = true;
            Killed();
        }
    }

    public override void Start()
    {
        balloons = new List<Balloon>();
        coll = GetComponent<Collider>();
        dest = false;
        currHP = maxHP;
        heartScale = crystalHeart.transform.localScale;
        coverScale = cover.transform.localScale;
        cover.SetActive(false);
    }

    public override void Killed()
    {
        gameObject.SetActive(false);
        boss.CrystalDestroyed(this);
    }
    
    public void StartPhase(int balNum)
    {
        if(currHP > 0)
        {
            actCor = StartCoroutine(CrystalCoroutine(balNum));
        }
    }

    public void EndPhase()
    {
        if (currHP > 0)
        {
            StopCoroutine(actCor);
            cover.SetActive(false);
            coll.enabled = false;
            while (balloons.Count > 0) balloons[0].Destroyed();
        }
    }

    public override void Update()
    {
        foreach (Balloon b in balloons) b.LineConnect(transform);
    }

    IEnumerator CrystalCoroutine(int balNum)
    {
        Renderer r;
        int prevHP;

        coll.enabled = false;

        yield return new WaitForSeconds(2f);

        while (currHP > 0)
        {
            currentColor = (VisionType)Random.Range(1, 4);
            r = cover.GetComponent<Renderer>();
            r.material.SetInt("_MaskType", 0);
            r.material.SetColor("_Color", Constants.Vision_Color(currentColor));
            r.material.SetInt("_StencilComp", 0);

            cover.transform.localScale = Vector3.zero;
            cover.SetActive(true);

            for (float timer = 0; timer <= 0.5f; timer += Time.deltaTime)
            {
                cover.transform.localScale = new Vector3(coverScale.x * timer / 0.5f, coverScale.y * timer / 0.5f, coverScale.z);
                yield return null;
            }
            cover.transform.localScale = coverScale;
            balloons = new List<Balloon>();

            for (int i = 0; i < balNum; i++)
            {
                Balloon b = new Balloon(spherePrefab, linePrefab, this);
                float rad = (Random.Range(0, 2) == 0 ? Random.Range(-90f, -20f) : Random.Range(20f, 90f)) * Mathf.PI / 180f;
                float dist = Random.Range(6, 19);
                b.sphere.transform.position = new Vector3(Mathf.Sin(rad) * dist, Random.Range(5, 8), Mathf.Cos(rad) * dist);
                b.ChangeColor(currentColor);
                balloons.Add(b);
            }

            while (balloons.Count > 0) yield return null;

            for (float timer = 0; timer <= 0.5f; timer += Time.deltaTime)
            {
                cover.transform.localScale = new Vector3(coverScale.x * (0.5f - timer) / 0.5f, coverScale.y * (0.5f - timer) / 0.5f, coverScale.z);
                yield return null;
            }
            cover.SetActive(false);
            coll.enabled = true;
            prevHP = currHP;

            for (float timer = 0; timer <= 9 - balNum * 2; timer += Time.deltaTime)
            {
                //if (prevHP - currHP >= maxHP / 2 + 1 || currHP <= 0) break;
                yield return null;
            }
            coll.enabled = false;
        }
    }
    public override void GameOver()
    {
        if (currHP > 0) StopCoroutine(actCor);
        foreach (Balloon b in balloons) b.GameOver();
    }
}
