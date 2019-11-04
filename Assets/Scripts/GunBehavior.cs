using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : GrabbableObject
{
    public Transform firePoint;
    public GameObject bullet;
    private float bulletSpeed = 10;
    private int bullets = 6;
    private float triggerDelay = 1, previousTriggeredTime = 0;

    public void Shoot()
    {
        var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = firePoint.up.normalized * bulletSpeed;
        previousTriggeredTime = Time.time;
        Destroy(newBullet, 10);
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(1);
        bullets = 6;
    }

    public override void HairTrigger()
    {
        if(Time.time - previousTriggeredTime > triggerDelay)
        {
            if (bullets-- > 0)
            {
                Shoot();
            }
            else
            {
                Reload();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
