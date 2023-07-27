using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FatalError : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rect;
    private Vector2 offset;
    private RectTransform canvas;
    private bool dragging;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    void Update()
    {
        if (dragging)
        {
            Vector2 canvasPoint = ScreenToCanvasPoint(Input.mousePosition);
            rect.anchoredPosition = canvasPoint - offset;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        dragging = true;
        offset = ScreenToCanvasPoint(Input.mousePosition) - rect.anchoredPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    private Vector2 ScreenToCanvasPoint(Vector2 point) 
    {
        Vector2 p;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, Camera.main, out p);
        return p;
    }
}
