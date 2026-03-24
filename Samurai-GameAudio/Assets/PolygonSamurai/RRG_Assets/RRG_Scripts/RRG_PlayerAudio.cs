using FMODUnity;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class RRG_PlayerAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private EventReference m_PlayerStartedJumpedEvent;
    [SerializeField] private EventReference m_PlayerEndedJumpedEvent;

    //fmod parameter feeders
    private int fallGruntSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayJumpGrunt()
    {
        RuntimeManager.PlayOneShot(m_PlayerStartedJumpedEvent, transform.position);
    }
    private void PlayLandingGrunt(float storedLinearVelocity)
    {
        if (storedLinearVelocity <= -10f)
        {
            fallGruntSelected = 1;
            print("GRUNT 1 SELECTED");
        }
        else if (storedLinearVelocity <= -4f)
        {
            fallGruntSelected = 0;
            print("GRUNT 0 SELECTED");
        }

        var eventInstance = RuntimeManager.CreateInstance(m_PlayerEndedJumpedEvent);
        eventInstance.setParameterByName("SelectJumpFallGrunt", fallGruntSelected);

        eventInstance.start();
        eventInstance.release();
    }

}
