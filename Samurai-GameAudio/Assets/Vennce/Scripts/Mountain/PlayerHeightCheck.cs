using UnityEngine;

public class PlayerHeightCheck : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mountainFloor;
    [SerializeField] private GameObject player;
    [SerializeField] private float maxRayLength = 1000f;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask mountainLayerMask;
    private void Update() {
        if (MountainCheck.isOnMountain) {
            CheckDistance();
        }
    }

    private void CheckDistance() {
        float height = PlayerHeight();
        if (height >= 0f) {
            Debug.Log("Player height above mountain floor: " + height);
        }
    }
    private void PlayAudioBasedOnHeight(float height) {
        // Implement audio logic based on height here
    }
    private float PlayerHeight() {
        Ray ray = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayLength, mountainLayerMask, QueryTriggerInteraction.Collide)) {
            Debug.Log("Player height: " + hit.distance);
            return hit.distance;
        }
        //should only run if player is too high above the mountain floor or raycast did not hit
        return -1f;
    }
    private void OnDrawGizmos() {
        if (player == null) return;
        if (MountainCheck.isOnMountain) return;
        Ray ray = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayLength)) {
            // Green = hit
            Gizmos.color = Color.green;
            Gizmos.DrawLine(player.transform.position, hit.point);

            // Draw a sphere at hit point
            Gizmos.DrawSphere(hit.point, 0.2f);
        }
        else {
            // Red = no hit
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                player.transform.position,
                player.transform.position + Vector3.down * maxRayLength
            );
        }
    }
}
