using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : GrabbableObject
{
    public Animator animator;
    public Transform firePoint;
    public GameObject bullet;
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
        Destroy(newBullet, 10);
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(1);
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
        SetTable();
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
