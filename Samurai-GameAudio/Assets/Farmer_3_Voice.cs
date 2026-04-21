using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class Farmer_3_Voice : MonoBehaviour

{
    [SerializeField] private EventReference Farmer_3_Voiceline;
    private EventInstance Farmer_3_Voiceline_Instance;
    private int Collision_Count;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Farmer_3_Voiceline_Instance = RuntimeManager.CreateInstance(Farmer_3_Voiceline);
        Collision_Count = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            if (Collision_Count <= 3)
            {
                Collision_Count++;
            }

            print(Collision_Count);

            Farmer_3_Voiceline_Instance.setParameterByName("WC Voiceline", Collision_Count);
            Farmer_3_Voiceline_Instance.start();
        }
    }
}