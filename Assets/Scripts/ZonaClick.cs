using UnityEngine;
using System.Collections;

public class ZonaClick : MonoBehaviour
{
    public EstadoNino estadoAlClic = EstadoNino.Playing; // Configura según zona
    public NinoController nino; // Arrástralo desde el inspector
    public float tiempoGracia = 1f; // acumulador en float para sumar suave por Time.deltaTime

    private Coroutine cambioIdleCoroutine;

    void OnMouseDown()
    {
        Debug.Log("click registradoo!");

        // Si había un cambio a Idle programado, lo cancelamos
        if (cambioIdleCoroutine != null)
        {
            StopCoroutine(cambioIdleCoroutine);
            cambioIdleCoroutine = null;
        }

        if (nino != null)
        {
            nino.SetEstado(estadoAlClic);
            Debug.Log($"Estado cambiado a: {estadoAlClic} ({(int)estadoAlClic})");
        }
    }

    void OnMouseUp()
    {
        Debug.Log("suelta click!");
        // Programar cambio a Idle después del tiempo de gracia
        if (nino != null)
        {
            cambioIdleCoroutine = StartCoroutine(CambiarIdleConRetraso());
        }
    }

    IEnumerator CambiarIdleConRetraso()
    {
        yield return new WaitForSeconds(tiempoGracia);
        nino.SetEstado(EstadoNino.Idle);
        cambioIdleCoroutine = null;
        Debug.Log("Tiempo de gracia terminado → Idle");
    }
}
