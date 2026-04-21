using FMODUnity;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayGrassSoundOnMovement : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private bool isInGrass;

    [Header("Settings")]
    [SerializeField] private float movementThreshold = 0.1f;
    [SerializeField] private float grassSoundTimer = 0f;
    [SerializeField] private float grassSoundInterval = 1f;

    [Header("FMOD Events")]
    [SerializeField] private EventReference GrassFootStepEvent;

    private void Update() {
        if (isInGrass && player != null) {
            PlaySoundBasedOnMovement();
        }
    }
    private void PlaySoundBasedOnMovement() {
        if (isInGrass && player.GetComponent<Rigidbody>().linearVelocity.magnitude > movementThreshold) {
            // Play grass sound effect here
            Debug.Log("Playing grass sound effect");
            if (player.GetComponent<ThirdPersonCharacter>().IsGroundedWrapper()) {
                grassSoundTimer += Time.deltaTime;
                if (grassSoundTimer >= grassSoundInterval) {
                    PlayGrassFootSteps();
                    grassSoundTimer = 0f;
                }
            }
            else {
                grassSoundTimer = 0f;
            }
        }
    }
    private void PlayGrassFootSteps() {
        var eventInstance = RuntimeManager.CreateInstance(GrassFootStepEvent);

        //eventInstance.setParameterByNameWithLabel("Footsteps", m_GroundTag);
        eventInstance.start();
        eventInstance.release();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            player = other.gameObject;
            isInGrass = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            player = null;
            isInGrass = false;
        }
    }
}

