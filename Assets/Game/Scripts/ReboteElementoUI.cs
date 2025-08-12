using UnityEngine;
using UnityEngine.UI;

public class ReboteElementoUI : MonoBehaviour
{
    [SerializeField] private Vector2 speed = new Vector2(200f, 200f); // Velocidad inicial
    private RectTransform rectTransform;
    private Vector2 direction;

    private RectTransform canvasRect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        direction = Random.insideUnitCircle.normalized; // Dirección aleatoria inicial
    }

    private void OnEnable()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        rectTransform.anchoredPosition += direction * speed * Time.deltaTime;

        Vector2 pos = rectTransform.anchoredPosition;
        Vector2 size = rectTransform.sizeDelta;
        Vector2 canvasSize = canvasRect.rect.size;

        // Rebote horizontal
        if (pos.x < -canvasSize.x / 2f + size.x / 2f || pos.x > canvasSize.x / 2f - size.x / 2f)
        {
            direction.x = -direction.x;
        }

        // Rebote vertical
        if (pos.y < -canvasSize.y / 2f + size.y / 2f || pos.y > canvasSize.y / 2f - size.y / 2f)
        {
            direction.y = -direction.y;
        }
    }
}
