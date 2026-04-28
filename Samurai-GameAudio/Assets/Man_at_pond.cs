using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class Man_at_pond_voice : MonoBehaviour

{
    [SerializeField] private EventReference Man_at_pond;
    private EventInstance Man_at_pond_Instance;
    private int Collision_Count;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Man_at_pond_Instance = RuntimeManager.CreateInstance(Man_at_pond);
        Collision_Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Man_at_pond_Instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        
    }
    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            

            if (Collision_Count==1)
            {
                Collision_Count--;
                print(Collision_Count);

                Man_at_pond_Instance.setParameterByName("Man at pond", Collision_Count);
                Man_at_pond_Instance.start();
            }

            if (Collision_Count ==0)
            {
                Collision_Count++;
                print(Collision_Count);

                Man_at_pond_Instance.setParameterByName("Man at pond", Collision_Count);
                Man_at_pond_Instance.start();
            }
            


            print(Collision_Count);

            Man_at_pond_Instance.setParameterByName("Man at pond", Collision_Count);
            Man_at_pond_Instance.start();
        }
    }

    void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            //Man_at_pond_Instance();
        }
    }
}
