using UnityEngine;

public class StickToObject : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow; 
    [SerializeField] private Vector2 offset;
    private RectTransform uiElement; 
    private Canvas parentCanvas;

    void Start()
    {
        uiElement = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(objectToFollow.position);

        Vector2 worldObjectScreenPosition = new Vector2(
            (viewportPosition.x * parentCanvas.pixelRect.width) - (parentCanvas.pixelRect.width * 0.5f),
            (viewportPosition.y * parentCanvas.pixelRect.height) - (parentCanvas.pixelRect.height * 0.5f)
        );

        uiElement.anchoredPosition = worldObjectScreenPosition + offset;
    }
}
