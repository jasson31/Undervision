using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : GrabbableObject
{
    public AudioClip fire, reload;
    public Animator animator;
    public Transform firePoint;
    public GameObject bullet;
    public GameObject[] bulletUI;
    private bool isReloading = false;
    private float bulletSpeed = 50;
    private float triggerDelay = 0.5f, previousTriggeredTime;
    [SerializeField]
    private int bullets = 6;

    public void Shoot()
    {
        var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = firePoint.up.normalized * bulletSpeed;
        bullets--;
        animator.SetTrigger("Fire");
        bulletUI[bullets].SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(fire);
        Destroy(newBullet, 10);
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        animator.SetTrigger("Reload");
        GetComponent<AudioSource>().PlayOneShot(reload);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 6; i++) bulletUI[i].SetActive(true);
        bullets = 6;
        isReloading = false;
    }

    public override void HairTrigger()
    {
        if(Time.time - previousTriggeredTime > triggerDelay && !isReloading)
        {
            if (bullets > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
            previousTriggeredTime = Time.time;
        }
    }
    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        ToTable();
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - previousTriggeredTime > triggerDelay && !isReloading)
            {
                if (bullets > 0)
                {
                    Shoot();
                }
                else
                {
                    StartCoroutine(Reload());
                }
                previousTriggeredTime = Time.time;
            }
        }
    }
}
