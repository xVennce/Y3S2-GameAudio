using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using FMODUnity;
using FMOD.Studio;

public class RRG_NinjaModes : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] GameObject fatSpine;
    [SerializeField] GameObject childSpine;
    [SerializeField] GameObject transformParticle;
    [SerializeField] GameObject particleParent;

    [Header("Skinny Ninja Stats")]
    [Range(0f, 20f)][SerializeField] float skinnyTimeDuration = 100f;
    [SerializeField] float skinnyMovementMultiplier = 1.5f;
    [SerializeField] float skinnyJumpMultiplier = 1.5f;

    private Vector3 fatNinjaScale;

    private ThirdPersonCharacter playerController;
    private float fatNinjaMoveSpeedMultiplier;
    private float fatNinjaJumpPower;
    private float skinnyNinjaMoveSpeedMultiplier;
    private float skinnyNinjaJumpPower;

    private Slider lockedInSlider;
    private CinemachineFreeLook cinemachineCam;
    private float defaultFOV; float fovToDefaultSpeed = 1.0f;
    private float increasedFOV; float fovToIncreasedSpeed = 2.0f;
    private float targetFOV;
    private float fovChangeSpeed;
    private float currentValue;

    private GameObject particle;
    private ParticleSystem playerPetals;
    private ParticleSystem.EmissionModule petalEmission;

    [Header("Fullscreen Shader References")]
    [SerializeField] Material speedShaderMat;

    [Header("Fullscreen Shader References")]
    [SerializeField] GameObject skinnyWaterCanteen;
    [SerializeField] GameObject fatAlcoholContainer;
    [SerializeField] GameObject skinnySword;
    [SerializeField] GameObject fatFood;

    [SerializeField] GameObject fatFoodPrefab;
    [SerializeField] GameObject fatAlcoholContainerPrefab;
    [SerializeField] GameObject skinnySwordPrefab;
    [SerializeField] GameObject skinnyWaterCanteenPrefab;

    private Transform fatFoodOriginalTransform;


    private bool isLockedIn;
    
    //ninja mode snapshot
    private EventInstance modeSnapshot;

    [Header("Locked-In Events")]
    [SerializeField] EventReference heartBeatEvent;
    private EventInstance heartBeatInstance;
    private float heartBeatVolumeControl = 100;
    [SerializeField] EventReference drumFinishEvent;
    private EventInstance drumFinishInstance;
    [SerializeField] EventReference outOfBreathEvent;
    private EventInstance outOfBreathInstance;



    private void Awake()
    {
        particle = Instantiate(transformParticle, particleParent.transform, false);
        playerPetals = particle.GetComponent<ParticleSystem>();
        petalEmission = playerPetals.emission;
        petalEmission.rateOverTime = 0f;
        modeSnapshot = RuntimeManager.CreateInstance("snapshot:/RRG-LockedInMode");
        modeSnapshot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        heartBeatInstance = RuntimeManager.CreateInstance(heartBeatEvent);
        drumFinishInstance = RuntimeManager.CreateInstance(drumFinishEvent);
        outOfBreathInstance = RuntimeManager.CreateInstance(outOfBreathEvent);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<ThirdPersonCharacter>();
        lockedInSlider = GameObject.Find("NinjaModeDuration").GetComponent<Slider>();
        cinemachineCam = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();

        //mode changes
        fatNinjaScale = fatSpine.transform.localScale;
        fatNinjaMoveSpeedMultiplier = playerController.m_MoveSpeedMultiplier;
        fatNinjaJumpPower = playerController.m_JumpPower;
        skinnyNinjaMoveSpeedMultiplier = fatNinjaMoveSpeedMultiplier * skinnyMovementMultiplier;
        skinnyNinjaJumpPower = fatNinjaJumpPower * skinnyJumpMultiplier;

        //locked in slider changes
        lockedInSlider.maxValue = skinnyTimeDuration;
        lockedInSlider.value = skinnyTimeDuration;
        lockedInSlider.gameObject.SetActive(false);

        //camera changes
        defaultFOV = cinemachineCam.m_Lens.FieldOfView;
        increasedFOV = defaultFOV * 1.1f;
        targetFOV = defaultFOV;

        SetFullscreenSpeedAplha(0.0f);

        skinnyWaterCanteen.SetActive(false); skinnySword.SetActive(false);
        fatAlcoholContainer.SetActive(true); fatFood.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && fatSpine.transform.localScale != Vector3.one) //cant press F when in skinny mode
        {
            StartCoroutine(TransformParticle());
        }
        cinemachineCam.m_Lens.FieldOfView = Mathf.Lerp(cinemachineCam.m_Lens.FieldOfView, targetFOV, Time.deltaTime * fovChangeSpeed);

        if (isLockedIn)
        {
            SetFullscreenSpeedAplha(playerController.m_Rigidbody.linearVelocity.magnitude / 10);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isLockedIn)
        {
            currentValue += 5;
            if (currentValue > skinnyTimeDuration) currentValue = skinnyTimeDuration;
            
        }
    }

    private void ChangeNinjaMode(int mode) //0 = fat  1 = skinny
    {
        if (mode == 0)
        {
            TransferNinjaScale(fatNinjaScale);
            outOfBreathInstance.start();
            playerController.m_MoveSpeedMultiplier = fatNinjaMoveSpeedMultiplier;
            playerController.m_JumpPower = fatNinjaJumpPower;
            targetFOV = defaultFOV; fovChangeSpeed = fovToDefaultSpeed; //camera fov changes
            SetFullscreenSpeedAplha(0.0f);
            modeSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            heartBeatInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isLockedIn = false;
        }
        if (mode == 1)
        {
            TransferNinjaScale(Vector3.one);
            playerController.m_MoveSpeedMultiplier = skinnyNinjaMoveSpeedMultiplier;
            playerController.m_JumpPower = skinnyNinjaJumpPower;
            targetFOV = increasedFOV; fovChangeSpeed = fovToIncreasedSpeed; //camera fov changes
            modeSnapshot.start();
            heartBeatInstance.start();
            isLockedIn = true;
        }
    }
    private void TransferNinjaScale(Vector3 fatSpineScale)
    {
        childSpine.transform.SetParent(fatSpine.transform.parent);
        fatSpine.transform.localScale = fatSpineScale;
        childSpine.transform.SetParent(fatSpine.transform);
    }
    IEnumerator SkinnyNinjaDuration()
    {
        bool runOnce = false;

        ChangeNinjaMode(1); // change to skinny

        currentValue = skinnyTimeDuration;
        lockedInSlider.value = currentValue;
        lockedInSlider.gameObject.SetActive(true);

        float startHeartBeatVolumeControl = 100;
        heartBeatVolumeControl = startHeartBeatVolumeControl;

        while (currentValue > 0) //runs until duration finishes
        {
            currentValue -= Time.deltaTime;
            lockedInSlider.value = currentValue;
            heartBeatVolumeControl = currentValue / skinnyTimeDuration; //swap values, 1 = low volume // 0 = max volume
            heartBeatInstance.setParameterByName("VolumeValue", heartBeatVolumeControl);
            heartBeatInstance.setParameterByName("HeartPitch", heartBeatVolumeControl); //controls heart pitch
            if (currentValue <= 0.5f && !runOnce)
            {
                drumFinishInstance.start();
                runOnce = true;
            }
            yield return null;
        }

        
        lockedInSlider.gameObject.SetActive(false);
        ChangeNinjaMode(0);
    }
    IEnumerator TransformParticle()
    {
        petalEmission.rateOverTime = 50;
        SpawnItem(fatFoodPrefab, fatFood); SpawnItem(fatAlcoholContainerPrefab, fatAlcoholContainer);

        fatAlcoholContainer.SetActive(false); fatFood.SetActive(false);
        

        yield return new WaitForSeconds(2f); //wait 2 secs for particle transition before transforming player

        StartCoroutine(SkinnyNinjaDuration());
        petalEmission.rateOverTime = 5;
        skinnyWaterCanteen.SetActive(true); skinnySword.SetActive(true);

        yield return new WaitForSeconds(skinnyTimeDuration); //wait until player transforms back to fat before destroying particle

        petalEmission.rateOverTime = 0;
        SpawnItem(skinnySwordPrefab, skinnySword); SpawnItem(skinnyWaterCanteenPrefab, skinnyWaterCanteen);

        fatAlcoholContainer.SetActive(true); fatFood.SetActive(true);
        skinnyWaterCanteen.SetActive(false); skinnySword.SetActive(false);
    }

    private void SetFullscreenSpeedAplha(float alpha)
    {
        speedShaderMat.SetFloat("_Alpha", alpha);
    }

    private void SpawnItem(GameObject item, GameObject originalItem)
    {
        GameObject instanced = Instantiate(item, originalItem.transform.position, originalItem.transform.rotation);
        Destroy(instanced, 3);
    }
}
