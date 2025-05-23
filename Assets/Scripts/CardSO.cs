using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "New card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public string imageName;
    public int cardId;
    public Sprite cardTopSprite;
    public Sprite cardBottomSprite;
    MarkerType markerType = MarkerType.Archetype;
    public Sprite[] GetCardVisual()
    {
        return new Sprite[] { cardTopSprite, cardBottomSprite };
    }
}
