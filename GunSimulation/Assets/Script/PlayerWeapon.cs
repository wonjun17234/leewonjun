using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public GameObject casingPrefab;

    public Transform bulletSpawn;

    public Transform cartridgeCaseSpawn;

    public float bulletSpeed = 30;

    public float cartridgeCaseSpeed = 30;

    public float bulletLifeTime = 5;

    public float cartridgeCaseLifeTime = 3;

    private bool isShooting = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isShooting)
        {
            isShooting = true;
            Fire();
            temp();
        }
    }

    private void Fire()
    {
        {
            GameObject bullet = Instantiate(bulletPrefab);

            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>()); //부모 객체와 충돌 X

            bullet.transform.position = bulletSpawn.position; //스폰 지점으로 객체 이동
            Vector3 rotation = bullet.transform.rotation.eulerAngles; 

            bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            bullet.GetComponent<Rigidbody>().AddForce( - bulletSpawn.up * bulletSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));
        }

    }

    private void temp()
    {
        {
            GameObject cartridgeCase = Instantiate(casingPrefab);

            Physics.IgnoreCollision(cartridgeCase.GetComponent<Collider>(), cartridgeCaseSpawn.parent.GetComponent<Collider>());

            cartridgeCase.transform.position = cartridgeCaseSpawn.position;
            Vector3 rotation = cartridgeCase.transform.rotation.eulerAngles;

            cartridgeCase.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            float random = Random.Range(0.1f, 1.5f);
            cartridgeCase.GetComponent<Rigidbody>().AddForce((cartridgeCaseSpawn.forward + cartridgeCaseSpawn.right * random) * cartridgeCaseSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyBulletAfterTime(cartridgeCase, cartridgeCaseLifeTime));
        }
        isShooting = false;
    }

    

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(bullet);
    }
}
