using UnityEngine;

public class CardsPanelManager : MonoBehaviour
{
    [Header("Card Display Template")]
    [SerializeField] private GameObject cardDisplayPrefab;
    [Header("Content Object Of Scroll View")]
    [SerializeField] private Transform scrollContentObjectArchetype;
    [SerializeField] private Transform scrollContentObjectFate;
    [Header("Main Card Display Panels")]
    [SerializeField] private Transform archetypeView;
    [SerializeField] private Transform fateView;
    private bool showingArchetype;

    private int archetypeLastIndex = 60; //archetypes first in db, fates after
    private void Start()
    {
        PopulateArchetypeCardDisplays();
        PopulateFateCardDisplays();
        fateView.gameObject.SetActive(false);
        showingArchetype = true;
    }
    public void SwitchView()
    {
        fateView.gameObject.SetActive(showingArchetype);
        archetypeView.gameObject.SetActive(!showingArchetype);
        showingArchetype = !showingArchetype;
    }
    private void PopulateArchetypeCardDisplays()
    {
        CardDatabase cardDB = GameManager.instance.cardDatabase;
        for(int i = 0; i < archetypeLastIndex; i++)
        {
            if (cardDB.cards[i].name == "Default") continue;
            GameObject go = Instantiate(cardDisplayPrefab, scrollContentObjectArchetype);
            go.GetComponent<CardDisplayHandler>().InstantiatePrefab(cardDB.cards[i]);
        }
    }
    private void PopulateFateCardDisplays()
    {
        CardDatabase cardDB = GameManager.instance.cardDatabase;
        for(int i = archetypeLastIndex + 1; i < cardDB.cards.Length; i++)
        {
            if (cardDB.cards[i].name == "Default") continue;
            GameObject go = Instantiate(cardDisplayPrefab, scrollContentObjectFate);
            go.GetComponent<CardDisplayHandler>().InstantiatePrefab(cardDB.cards[i]);
        }
    }
}
