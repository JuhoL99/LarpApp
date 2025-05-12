using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "New card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public int cardId;
    public Sprite cardTopSprite;
    public Sprite cardBottomSprite;

    public Sprite[] GetCardVisual()
    {
        Debug.Log("in card visual");
        return new Sprite[] { cardTopSprite, cardBottomSprite };
    }
}
