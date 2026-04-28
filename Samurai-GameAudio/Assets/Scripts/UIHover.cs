using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIHover : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] EventReference uiHoverEvent;
    [SerializeField] EventReference uiClickEvent;

    public void OnSelect(BaseEventData eventData) //Controller Hover
    {
        RuntimeManager.PlayOneShot(uiHoverEvent);
    }
    public void OnDeselect(BaseEventData eventData) //Controller Exit
    {
    }
    public void OnPointerEnter(PointerEventData eventData) //Mouse Hover
    {
        RuntimeManager.PlayOneShot(uiHoverEvent);
        print("Mouse Hover Check Working");
    }
    public void OnPointerExit(PointerEventData eventData) //Mouse Exit
    {
    }
    public void OnClick()
    {
        print("Button Clicked");
        RuntimeManager.PlayOneShot(uiClickEvent);
        StartCoroutine("StartGame");
    }

    IEnumerator StartGame()
    
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Demo");
    }
}
