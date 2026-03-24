using UnityEngine;

public class MountainCheck : MonoBehaviour {
    [Header("Player Bool")]
    public static bool isOnMountain = false;
    public bool isOnMountainInspector;
    private void Update() {
        isOnMountainInspector = isOnMountain;
    }
}
