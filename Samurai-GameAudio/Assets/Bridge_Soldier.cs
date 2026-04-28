using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class Bridge_Soldier : MonoBehaviour

{
    [SerializeField] private EventReference Bridge_Soldier_Voiceline;
    private EventInstance Bridge_Soldier_Voiceline_Instance;
    private int Collision_Count;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bridge_Soldier_Voiceline_Instance = RuntimeManager.CreateInstance(Bridge_Soldier_Voiceline);
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

            if (Collision_Count <= 2)
            {
                Collision_Count++;
            }

            print(Collision_Count);
            Bridge_Soldier_Voiceline_Instance.setParameterByName("Bridge Soldier", Collision_Count);
            Bridge_Soldier_Voiceline_Instance.start();


        }
    }
}

