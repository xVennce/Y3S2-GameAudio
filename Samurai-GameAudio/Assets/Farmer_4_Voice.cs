using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class Farmer_4_Voice : MonoBehaviour

{
    [SerializeField] private EventReference Farmer_4_Voiceline;
    private EventInstance Farmer_4_Voiceline_Instance;
    private int Collision_Count;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Farmer_4_Voiceline_Instance = RuntimeManager.CreateInstance(Farmer_4_Voiceline);
        Collision_Count = -1;
    }

    // Update is called once per frame
    void Update()
    {
        Farmer_4_Voiceline_Instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
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

            Farmer_4_Voiceline_Instance.setParameterByName("Chav Voiceline", Collision_Count);
            Farmer_4_Voiceline_Instance.start();
        }
    }
}
