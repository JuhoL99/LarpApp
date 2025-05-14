using TMPro;
using UnityEngine;

public class LinkedUserPanel : MonoBehaviour
{
    public User thisPanelUser;
    private TMP_Text userNameText;
    private Sprite userProfilePicture;

    private void Start()
    {
        userNameText = transform.GetChild(0).GetComponent<TMP_Text>();
        SetPanelText();
    }
    private void SetPanelText()
    {
        if (thisPanelUser == null) return;
        userNameText.text = $"{thisPanelUser.userName} {thisPanelUser.userID}";
    }
}
