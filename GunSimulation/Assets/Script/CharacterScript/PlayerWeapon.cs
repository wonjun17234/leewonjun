using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerWeapon : MonoBehaviour
{
    public GameObject OBJBulletPrefab; // 총알 프리펩 
    public Transform bulletSpawn; // 총알 프리펩 스폰 위치

    public GameObject OBJCasingPrefab; // 탄피 프리펩
    public Transform cartridgeCaseSpawn; // 탄피 프리펩 스폰 위치

    public GameObject OBJSlide; // 움직일 슬라이드

    public GameObject OBJTrigger; // 움직일 트리거

    public GameObject OBJMag; // 움직일 탄창
    public GameObject OBJMagI; // 탄창의 인스턴스

    public ParticleSystem particleObject; // 이펙트 

    public EventSystem eventSystem; //버튼 위에 있는지 배경에 있는지 확인하기 위해 가져옴

    public float bulletSpeed = 30; //총알 스피드
    public float bulletLifeTime = 5; //총알이 살아있는 시간

    public float cartridgeCaseSpeed = 30; //탄피 스피드
    public float cartridgeCaseLifeTime = 0.6f; //탄피 살아있는 시간

    public bool isPlayer;

    private Vector3 SlideStartPos; //슬라이드 움직임의 처음 
    private Vector3 SlideTargetPos; //슬라이드가 어디까지 당겨질건지
    private Vector3 MagStartPos; //탄창의 처음 위치
    private Vector3 MagTargetPos; //탄창이 어디까지 빠질건지

    private bool isShooting = false; //지금 총을 쏘는 중인지
    private bool isReloading = false; //지금 장전 중인지

    private int MaxBullet = 6; //최대 총알 개수
    private int currentBullet = 6; //현재 총알 개수


    public ParticleSystem particleLauncher;
    private AudioSource audio;
    public List<AudioClip> audioClips;
    private int audioIndex;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.loop = false;
        audio.mute = false;
        audio.playOnAwake = false;

        SlideStartPos = OBJSlide.transform.localPosition; //현재의 슬라이드 위치를 시작 슬라이드로 설정
        SlideTargetPos = new Vector3(SlideStartPos.x, SlideStartPos.y, SlideStartPos.z - 0.045f);
        MagStartPos = OBJMag.transform.localPosition;
        MagTargetPos = new Vector3(MagStartPos.x, MagStartPos.y - 0.25f, MagStartPos.z - 0.1057f);
        currentBullet = MaxBullet;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayer)
        {
            if (eventSystem != null)
            {
                if (Input.GetMouseButtonDown(0) && !isShooting && !isReloading && !eventSystem.IsPointerOverGameObject())
                {
                    isShooting = true;
                    StartCoroutine(shot());
                    currentBullet--;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && !isShooting && !isReloading)
                {
                    isShooting = true;
                    StartCoroutine(shot());
                    currentBullet--;
                }
            }
            if (currentBullet == 0)
            {
                OBJMag.GetComponent<MeshFilter>().mesh = transform.GetComponent<Mag_Mesh>().mesh[1];

            }
            if (currentBullet < 0)
            {
                audioIndex = 1;
            }
        }
        
        
        
    }
    public bool enemyShot()
    {
        if (!isShooting && !isReloading && currentBullet > 0)
        {
            isShooting = true;
            StartCoroutine(shot());
            currentBullet--;
            return true;
        }
        else
        {
            OBJMag.GetComponent<MeshFilter>().mesh = transform.GetComponent<Mag_Mesh>().mesh[1];
            Reload();
            return false;
        }
    }

    public void Reload()
    {
        if (!isReloading)
        {
            audioIndex = 0;
            isReloading = true;
            currentBullet = MaxBullet;


            StartCoroutine(moveReload());
        }

    }
    private IEnumerator moveReload()
    {
        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime * 2;
            OBJMag.transform.localPosition = Vector3.Lerp(MagStartPos, MagTargetPos, time);
            yield return null;
        }
        OBJMag.GetComponent<Rigidbody>().useGravity = true;
        OBJMag.GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(0.4f);
        StartCoroutine(DestroyObject(OBJMag, 3f));
        GameObject newOBJMag = Instantiate(OBJMagI, transform);
        newOBJMag.transform.localPosition = MagTargetPos;
        yield return new WaitForSeconds(0.3f);
        time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime * 2;
            newOBJMag.transform.localPosition = Vector3.Lerp(MagTargetPos, MagStartPos, time);
            yield return null;
        }
        OBJMag = newOBJMag;
        isReloading = false;
        isReloading = false;
    }

    private void Fire() // 총알이 날아가며 발사 이펙트 시작
    {

        //GameObject bullet = Instantiate(OBJBulletPrefab);

        //Physics.Ignore    ion(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>());

        //bullet.transform.localPosition = bulletSpawn.position;
        //Vector3 rotation = bulletSpawn.transform.rotation.eulerAngles;

        ////bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        //float randX = Random.Range(-0.5f, 0.5f);
        //float randY = Random.Range(-0.5f, 0.5f);
        //float randZ = Random.Range(-0.5f, 0.5f);
        //bullet.transform.rotation = Quaternion.Euler(-90 + randX, transform.eulerAngles.y + randY, 0 + randZ);
        //bullet.transform.GetComponent<Rigidbody>().AddForce(-bullet.transform.up * bulletSpeed, ForceMode.Impulse);


        
        particleObject.Play(); //발사 이펙트
        ParticleSystem.MainModule psMain = particleLauncher.main;
        psMain.startColor = Color.red;
        particleLauncher.Emit(1);

        //StartCoroutine(DestroyObject(bullet, bulletLifeTime));
    }

    private void sparking() // 탄피가 튀는 함수
    {
        {
            GameObject cartridgeCase = Instantiate(OBJCasingPrefab);


            cartridgeCase.transform.localPosition = cartridgeCaseSpawn.position;
            Vector3 rotation = cartridgeCase.transform.rotation.eulerAngles;

            cartridgeCase.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            float random = Random.Range(0.1f, 1.5f);
            cartridgeCase.GetComponent<Rigidbody>().AddForce((cartridgeCaseSpawn.forward + cartridgeCaseSpawn.right * random) * cartridgeCaseSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyObject(cartridgeCase, cartridgeCaseLifeTime));
        }
    }
    private IEnumerator shot() //trigger가 회전 하는 함수, 끝까지 회전하면 총알이 발사되는 함수 호출
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
        audio.PlayOneShot(audioClips[audioIndex]);
        if (currentBullet >= 0)//총알이 없어도 쏠수는 있게
        {
            Fire();
        }

        StartCoroutine(slideBack());
        time = 0;
        while (time < 0.05f)
        {
            time += Time.deltaTime;
            currentX = Mathf.Lerp(-60f, -90f, time * 20);
            OBJTrigger.transform.localRotation = Quaternion.Euler(currentX, 0f, 0f);
            yield return null;
        }
    }
    private IEnumerator slideBack() //슬라이드를 뒤로 넘기는 함수, 끝까지 가면 탄피가 튀는 함수 호출
    {
        float time = 0f;
        while (time < 0.05f)
        {
            time += Time.deltaTime;
            OBJSlide.transform.localPosition = Vector3.Lerp(SlideStartPos, SlideTargetPos, time * 20f);
            yield return null;
        }
        time = 0;
        if (!isReloading && currentBullet >= 0) //장전할때도 쓰기 위해서, 총알이 없어도 쏘기 위해서
        {
            sparking();
        }
        while (time < 0.05f)
        {
            time += Time.deltaTime;
            OBJSlide.transform.localPosition = Vector3.Lerp(SlideTargetPos, SlideStartPos, time * 20f);
            yield return null;
        }

        isShooting = false;
    }


    private IEnumerator DestroyObject(GameObject DObject, float delay) //삭제할 오브젝트와 시간을 넘기면 그 시간 후에 오브젝트 삭제
    {
        yield return new WaitForSeconds(delay);

        Destroy(DObject);
    }

}
