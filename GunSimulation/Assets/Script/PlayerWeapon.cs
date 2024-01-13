using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    public GameObject OBJBulletPrefab;
    public Transform bulletSpawn;

    public GameObject OBJCasingPrefab;
    public Transform cartridgeCaseSpawn;

    public GameObject OBJSlide;

    public GameObject OBJTrigger;

    public ParticleSystem particleObject;

    public float bulletSpeed = 30;
    public float bulletLifeTime = 5;

    public float cartridgeCaseSpeed = 30;
    public float cartridgeCaseLifeTime = 3;
    
    private Vector3 startPos;
    private Vector3 targetPos;
    
    private bool isShooting = false;
    void Start()
    {
        startPos = OBJSlide.transform.localPosition;
        targetPos = new Vector3(startPos.x, startPos.y, startPos.z - 0.045f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            isShooting = true;
            StartCoroutine(shot());
        }
    }
    
    private void Fire()
    {
        {
            GameObject bullet = Instantiate(OBJBulletPrefab);

            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>()); //�θ� ��ü�� �浹 X

            bullet.transform.localPosition = bulletSpawn.position; //���� �������� ��ü �̵�
            Vector3 rotation = bullet.transform.rotation.eulerAngles; 

            bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            bullet.GetComponent<Rigidbody>().AddForce( - bulletSpawn.up * bulletSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));
            StartCoroutine(slideBack());
            //이펙트 구현
            particleObject.Play();
        }
    }

    private void sparking()
    {
        {
            GameObject cartridgeCase = Instantiate(OBJCasingPrefab);

            Physics.IgnoreCollision(cartridgeCase.GetComponent<Collider>(), cartridgeCaseSpawn.parent.GetComponent<Collider>());

            cartridgeCase.transform.localPosition = cartridgeCaseSpawn.position;
            Vector3 rotation = cartridgeCase.transform.rotation.eulerAngles;

            cartridgeCase.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            float random = Random.Range(0.1f, 1.5f);
            cartridgeCase.GetComponent<Rigidbody>().AddForce((cartridgeCaseSpawn.forward + cartridgeCaseSpawn.right * random) * cartridgeCaseSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyBulletAfterTime(cartridgeCase, cartridgeCaseLifeTime));
        }
    }
    private IEnumerator shot()
    {
        float time = 0f;
        float currentX;
        while (time < 0.05f)
        {
            time += Time.deltaTime;
            currentX = Mathf.Lerp(-90f, -60f, time * 20);
            OBJTrigger.transform.localRotation = Quaternion.Euler(currentX, 0f, 0f);
            yield return null;
        }
        Fire();
        time = 0;
        while (time < 0.05f)
        {
            time += Time.deltaTime;
            currentX = Mathf.Lerp(-60f, -90f, time * 20);
            OBJTrigger.transform.localRotation = Quaternion.Euler(currentX, 0f, 0f);
            yield return null;
        }
    }
    private IEnumerator slideBack()
    {
        float time = 0f;
        while(time < 0.05f)
        {
            time += Time.deltaTime;
            OBJSlide.transform.localPosition = Vector3.Lerp(startPos, targetPos, time * 20f);
            yield return null;
        }
        time = 0;
        sparking();
        while (time < 0.05f)
        {
            time += Time.deltaTime;
            OBJSlide.transform.localPosition = Vector3.Lerp(targetPos, startPos, time * 20f);
            yield return null;
        }
        
        isShooting = false;
    }
    


    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(bullet);
    }
}
