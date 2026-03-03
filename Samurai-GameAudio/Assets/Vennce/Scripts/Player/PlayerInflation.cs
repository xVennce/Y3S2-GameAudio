using UnityEngine;
public class PlayerInflation : MonoBehaviour {
    [Header("Player References")]
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Vector3 playerDesiredScale;
    [SerializeField] private float scaleSpeed = 5f;

    private Vector3 playerInitialScale;
    private Vector3 targetScale;

    [Header("Toggle Check")]
    [SerializeField] public bool isScaled = false;

    private void Start() {
        playerInitialScale = playerModel.transform.localScale;
        targetScale = isScaled ? playerDesiredScale : playerInitialScale;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            isScaled = !isScaled;
            targetScale = isScaled ? playerDesiredScale : playerInitialScale;
            Debug.Log("Toggled fat: " + isScaled);
        }

        playerModel.transform.localScale = Vector3.Lerp(
            playerModel.transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }
}
