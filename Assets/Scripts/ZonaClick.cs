using UnityEngine;

public class ZonaClick : MonoBehaviour
{
    public EstadoNino estadoAlClic = EstadoNino.Cleaning; // Configura según zona
    public NinoController nino; // Arrástralo desde el inspector

    void OnMouseDown()
    {
        Debug.Log("click registradoo!");
        if (nino != null)
        {
            nino.SetEstado(estadoAlClic);
            Debug.Log($"Estado cambiado a: {estadoAlClic} ({(int)estadoAlClic})");
        }
    }
}