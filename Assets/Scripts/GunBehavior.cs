using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : GrabbableObject
{
    public Animator animator;
    public Transform firePoint;
    public GameObject bullet;
    private bool isReloading = false;
    private float bulletSpeed = 10;
    [SerializeField]
    private int bullets = 6;

    public void Shoot()
    {
        var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = firePoint.up.normalized * bulletSpeed;
        bullets--;
        //animator.SetTrigger("Fire");
        Destroy(newBullet, 10);
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        //animator.SetTrigger("Reload");
        yield return new WaitForSeconds(1);
        bullets = 6;
        isReloading = false;
    }

    public override void HairTrigger()
    {
        if (bullets > 0 && !isReloading)
        {
            Shoot();
        }
        else
        {
            StartCoroutine(Reload());
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
