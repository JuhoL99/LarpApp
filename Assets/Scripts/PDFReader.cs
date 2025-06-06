using UnityEngine;
using UnityEngine.UI;

public class PDFImageViewer : MonoBehaviour
{
    public GameObject imagePrefab;
    public Transform contentParent;
    public Sprite[] imageSprites;

    void Start()
    {
        LoadImages();
    }

    void LoadImages()
    {
        foreach (Sprite sprite in imageSprites)
        {
            GameObject newImage = Instantiate(imagePrefab, contentParent);
            Image imageComponent = newImage.GetComponent<Image>();
            imageComponent.sprite = sprite;
        }
    }
}