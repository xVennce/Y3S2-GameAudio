using UnityEngine;

public class MountainCollisionCheck : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            MountainCheck.isOnMountain = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            MountainCheck.isOnMountain = false;
        }
    }
}
