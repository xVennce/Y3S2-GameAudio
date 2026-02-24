using UnityEngine;

public class Testing : MonoBehaviour {
    private Vector3 position;
    private void Start() {
        position = GetComponent<Transform>().position;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player entered the trigger");
            FMODUnity.RuntimeManager.PlayOneShot("event:/TestAudio/GuitarSounds", position);
        }
    }
}
