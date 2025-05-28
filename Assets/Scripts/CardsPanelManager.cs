using UnityEngine;

public class CardsPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject cardDisplayPrefab;
    [SerializeField] private Transform scrollContentObject;
    private void Start()
    {
        PopulateCardDisplays();
    }
    private void PopulateCardDisplays()
    {
        CardDatabase cardDB = GameManager.instance.cardDatabase;
        for(int i = 0; i < cardDB.cards.Length; i++)
        {
            GameObject go = Instantiate(cardDisplayPrefab, scrollContentObject);
            go.GetComponent<CardDisplayHandler>().InstantiatePrefab(cardDB.cards[i]);
        }
    }
}
