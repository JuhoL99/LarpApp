using UnityEngine;

[CreateAssetMenu(fileName = "Card database", menuName = "New card database")]
public class CardDatabase : ScriptableObject
{
    public CardSO[] cards;

    public CardSO GetCardByName(string name)
    {
        foreach(CardSO card in cards)
        {
            if(card.cardName == name) return card;
        }
        return null;
    }
    public CardSO GetCardByID(int id)
    {
        foreach(CardSO card in cards)
        {
            if(card.cardId == id) return card;
        }
        return null;
    }
    public void NamesFromImageFile()
    {
        foreach (CardSO card in cards)
        {
            card.imageName = card.cardTopSprite.name;
        }
    }
}
