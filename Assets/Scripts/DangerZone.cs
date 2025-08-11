using UnityEngine;

public class DangerZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isDangerZoneOccupied = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setDangerZoneOccupancy(bool newState)
    {
        isDangerZoneOccupied = newState;
    }
}
