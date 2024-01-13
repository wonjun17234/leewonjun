using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerWeapon : MonoBehaviour
{
    public GameObject OBJBulletPrefab;
    public Transform bulletSpawn;

    public GameObject OBJCasingPrefab;
    public Transform cartridgeCaseSpawn;

    public GameObject OBJSlide;

    public GameObject OBJTrigger;

    public GameObject OBJMagEmpty;
    public GameObject OBJMagFull;
    private MeshFilter filter;

    public ParticleSystem particleObject;

    public EventSystem eventSystem;

    public float bulletSpeed = 30;
    public float bulletLifeTime = 5;

    public float cartridgeCaseSpeed = 30;
    public float cartridgeCaseLifeTime = 3;
    
    private Vector3 SlideStartPos;
    private Vector3 SlideTargetPos;
    private Vector3 MagStartPos;
    private Vector3 MagTargetPos;
    
    private bool isShooting = false;
    private bool isReloading = false;

    private int MaxBullet = 6;
    private int currentBullet = 0;
    void Start()
    {
        filter = OBJMagFull.GetComponent<MeshFilter>();
        SlideStartPos = OBJSlide.transform.localPosition;
        SlideTargetPos = new Vector3(SlideStartPos.x, SlideStartPos.y, SlideStartPos.z - 0.045f);
        MagStartPos = new Vector3(0, -0.0115f, -0.0545f);
        MagTargetPos = new Vector3(0, -0.1195f, -0.0962f);
        currentBullet = MaxBullet;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !isShooting && !isReloading &&  !eventSystem.IsPointerOverGameObject())
        {
            isShooting = true;
            StartCoroutine(shot());
        }
        /*if(currentBullet == 1)
        {
            mesh change
        }
        */
    }
    public void Reload() {
        isReloading = true;
        currentBullet = MaxBullet;
        /*
         mag Reload
         
         */
    }

    private void Fire()
    {
        {
            GameObject bullet = Instantiate(OBJBulletPrefab);

            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>()); 

            bullet.transform.localPosition = bulletSpawn.position; 
            Vector3 rotation = bullet.transform.rotation.eulerAngles; 

            bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            bullet.GetComponent<Rigidbody>().AddForce( - bulletSpawn.up * bulletSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyObject(bullet, bulletLifeTime));
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

            StartCoroutine(DestroyObject(cartridgeCase, cartridgeCaseLifeTime));
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
            OBJSlide.transform.localPosition = Vector3.Lerp(SlideStartPos, SlideTargetPos, time * 20f);
            yield return null;
        }
        time = 0;
        sparking();
        while (time < 0.05f)
        {
            time += Time.deltaTime;
            OBJSlide.transform.localPosition = Vector3.Lerp(SlideTargetPos, SlideStartPos, time * 20f);
            yield return null;
        }
        
        isShooting = false;
    }
    


    private IEnumerator DestroyObject(GameObject DObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(DObject);
    }
}
