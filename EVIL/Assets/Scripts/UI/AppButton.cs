using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AppButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("ButtonState")]
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;

    [Header("Actions")]
    [SerializeField] private UnityEvent action;

    [Header("References")]
    private Image image;
    private RectTransform text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = transform.Find("Text").GetComponent<RectTransform>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = up;
        text.anchoredPosition = new Vector2(text.anchoredPosition.x, text.anchoredPosition.y + 2);
        action?.Invoke();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        image.sprite = down;
        text.anchoredPosition = new Vector2(text.anchoredPosition.x, text.anchoredPosition.y - 2);
    }
}
