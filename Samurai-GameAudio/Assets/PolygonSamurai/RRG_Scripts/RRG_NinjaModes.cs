using UnityEngine;

public class RRG_NinjaModes : MonoBehaviour
{
    public GameObject fatNinja;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fatNinja.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            fatNinja.SetActive(true);
        }
    }
}
