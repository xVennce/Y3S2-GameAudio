using Cinemachine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

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
    private float defaultFOV;
    private float increasedFOV;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && fatSpine.transform.localScale != Vector3.one) //cant press F when in skinny mode
        {
            StartCoroutine(TransformParticle());
        }
    }

    private void ChangeNinjaMode(int mode) //0 = fat  1 = skinny
    {
        if (mode == 0)
        {
            TransferNinjaScale(fatNinjaScale);
            playerController.m_MoveSpeedMultiplier = fatNinjaMoveSpeedMultiplier;
            playerController.m_JumpPower = fatNinjaJumpPower;
            cinemachineCam.m_Lens.FieldOfView = defaultFOV;
        }
        if (mode == 1)
        {
            TransferNinjaScale(Vector3.one);
            playerController.m_MoveSpeedMultiplier = skinnyNinjaMoveSpeedMultiplier;
            playerController.m_JumpPower = skinnyNinjaJumpPower;
            cinemachineCam.m_Lens.FieldOfView = increasedFOV;
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
        GameObject particle = Instantiate(transformParticle, particleParent.transform, false);

        // Wait while the effect plays
        yield return new WaitForSeconds(2f);

        StartCoroutine(SkinnyNinjaDuration());
        Destroy(particle);
    }
}
