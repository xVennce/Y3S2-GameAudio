using UnityEngine;

public class RRG_Items : MonoBehaviour
{
    private Collider playerCollider;
    private Collider selfCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        selfCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(selfCollider, playerCollider);
    }
}
