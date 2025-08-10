using UnityEngine;
using UnityEngine.UI;

public class BotonAnimado : MonoBehaviour
{
    [SerializeField] private float amplitud = 10f;        // Altura del movimiento
    [SerializeField] private float velocidad = 2f;         // Velocidad del movimiento

    private RectTransform rectTransform;
    private Vector2 posicionInicial;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        posicionInicial = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float offsetY = Mathf.Sin(Time.time * velocidad) * amplitud;
        rectTransform.anchoredPosition = posicionInicial + new Vector2(0, offsetY);
    }
}
