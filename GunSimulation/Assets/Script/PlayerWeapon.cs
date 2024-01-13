using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public GameObject casingPrefab;

    public GameObject slide;

    public Transform bulletSpawn;

    public Transform cartridgeCaseSpawn;

    public float bulletSpeed = 30;

    public float cartridgeCaseSpeed = 30;

    public float bulletLifeTime = 5;

    public float cartridgeCaseLifeTime = 3;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isShooting = false;
    void Start()
    {
        startPos = slide.transform.localPosition;
        targetPos = new Vector3(startPos.x, startPos.y, startPos.z - 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            isShooting = true;
            Fire();
        }
    }

    private void Fire()
    {
        {
            GameObject bullet = Instantiate(bulletPrefab);

            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>()); //�θ� ��ü�� �浹 X

            bullet.transform.localPosition = bulletSpawn.position; //���� �������� ��ü �̵�
            Vector3 rotation = bullet.transform.rotation.eulerAngles; 

            bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            bullet.GetComponent<Rigidbody>().AddForce( - bulletSpawn.up * bulletSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));
            StartCoroutine(slideBack());
        }
    }
    private IEnumerator slideBack()
    {
        float time = 0f;
        while(time < 0.05f)
        {
            time += Time.deltaTime;
            slide.transform.localPosition = Vector3.Lerp(startPos, targetPos, time * 20f);
            yield return null;
        }
        time = 0;
        sparking();
        while (time < 0.05f)
        {
            time += Time.deltaTime;
            slide.transform.localPosition = Vector3.Lerp(targetPos, startPos, time * 20f);
            yield return null;
        }
        
        isShooting = false;
    }
    private void sparking()
    {
        {
            GameObject cartridgeCase = Instantiate(casingPrefab);

            Physics.IgnoreCollision(cartridgeCase.GetComponent<Collider>(), cartridgeCaseSpawn.parent.GetComponent<Collider>());

            cartridgeCase.transform.localPosition = cartridgeCaseSpawn.position;
            Vector3 rotation = cartridgeCase.transform.rotation.eulerAngles;

            cartridgeCase.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            float random = Random.Range(0.1f, 1.5f);
            cartridgeCase.GetComponent<Rigidbody>().AddForce((cartridgeCaseSpawn.forward + cartridgeCaseSpawn.right * random) * cartridgeCaseSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyBulletAfterTime(cartridgeCase, cartridgeCaseLifeTime));
        }
    }


    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(bullet);
    }
}
