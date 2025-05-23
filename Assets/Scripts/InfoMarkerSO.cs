using UnityEngine;

[CreateAssetMenu(fileName = "InfoMarkerSO", menuName = "Scriptable Objects/InfoMarkerSO")]
public class InfoMarkerSO : MarkerSO
{
    public MarkerType markerType = MarkerType.Info;
    public string infoText;
}
