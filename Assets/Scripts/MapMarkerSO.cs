using UnityEngine;

[CreateAssetMenu(fileName = "MapMarkerSO", menuName = "Scriptable Objects/MapMarkerSO")]
public class MapMarkerSO : MarkerSO
{
    public MarkerType markerType = MarkerType.Map;
    public Vector3 location;
}
