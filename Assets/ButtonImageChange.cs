using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonImageChange : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image buttonImage; // Reference the Image component
    public Sprite normalImage;
    public Sprite pressedImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.sprite = pressedImage;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.sprite = normalImage;
    }
}
