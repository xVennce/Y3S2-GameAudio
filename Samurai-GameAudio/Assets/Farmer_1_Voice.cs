using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class Farmer_1_Voice : MonoBehaviour

{
    [SerializeField] private EventReference Farmer_1_Voiceline;
    private EventInstance Farmer_1_Voiceline_Instance;
    private int Collision_Count;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Farmer_1_Voiceline_Instance = RuntimeManager.CreateInstance(Farmer_1_Voiceline);
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

            RuntimeManager.StudioSystem.setParameterByName("Voicelines", Collision_Count);
            //Farmer_1_Voiceline_Instance.setParameterByName("Voicelines", Collision_Count);
            Farmer_1_Voiceline_Instance.start();


                
        }
    }
}
