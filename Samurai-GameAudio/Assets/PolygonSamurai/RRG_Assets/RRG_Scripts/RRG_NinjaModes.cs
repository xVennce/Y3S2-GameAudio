using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using FMODUnity;

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

    private GameObject particle;
    private ParticleSystem playerPetals;
    private ParticleSystem.EmissionModule petalEmission;

    [Header("Fullscreen Shader References")]
    [SerializeField] Material speedShaderMat;

    private bool isLockedIn;
    
    //ninja mode snapshot
    private FMOD.Studio.EventInstance modeSnapshot;


    private void Awake()
    {
        particle = Instantiate(transformParticle, particleParent.transform, false);
        playerPetals = particle.GetComponent<ParticleSystem>();
        petalEmission = playerPetals.emission;
        petalEmission.rateOverTime = 0f;
        modeSnapshot = RuntimeManager.CreateInstance("snapshot:/RRG-LockedInMode");
        modeSnapshot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
    }

    private void ChangeNinjaMode(int mode) //0 = fat  1 = skinny
    {
        if (mode == 0)
        {
            TransferNinjaScale(fatNinjaScale);
            playerController.m_MoveSpeedMultiplier = fatNinjaMoveSpeedMultiplier;
            playerController.m_JumpPower = fatNinjaJumpPower;
            targetFOV = defaultFOV; fovChangeSpeed = fovToDefaultSpeed; //camera fov changes
            SetFullscreenSpeedAplha(0.0f);
            modeSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isLockedIn = false;

        }
        if (mode == 1)
        {
            TransferNinjaScale(Vector3.one);
            playerController.m_MoveSpeedMultiplier = skinnyNinjaMoveSpeedMultiplier;
            playerController.m_JumpPower = skinnyNinjaJumpPower;
            targetFOV = increasedFOV; fovChangeSpeed = fovToIncreasedSpeed; //camera fov changes
            modeSnapshot.start();
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

        ChangeNinjaMode(1); // change to skinny

        float currentValue = skinnyTimeDuration;
        lockedInSlider.value = currentValue;
        lockedInSlider.gameObject.SetActive(true);

        while (currentValue > 0) //runs until duration finishes
        {
            currentValue -= Time.deltaTime;
            lockedInSlider.value = currentValue;
            yield return null;
        }

        lockedInSlider.gameObject.SetActive(false);
        ChangeNinjaMode(0);
    }
    IEnumerator TransformParticle()
    {
        petalEmission.rateOverTime = 50;

        yield return new WaitForSeconds(2f); //wait 2 secs for particle transition before transforming player

        StartCoroutine(SkinnyNinjaDuration());
        petalEmission.rateOverTime = 5;

        yield return new WaitForSeconds(skinnyTimeDuration); //wait until player transforms back to fat before destroying particle

        petalEmission.rateOverTime = 0;
    }

    private void SetFullscreenSpeedAplha(float alpha)
    {
        speedShaderMat.SetFloat("_Alpha", alpha);
    }
}
