using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinkedUserPanel : MonoBehaviour
{
    public User thisPanelUser;
    public LinkedUserUI linkedUserUI;
    private TMP_Text userNameText;
    private Sprite userProfilePicture;
    private Button removeUser;

    private void Start()
    {
        userNameText = transform.GetChild(1).GetComponent<TMP_Text>();
        SetPanelText();
        removeUser = transform.GetChild(0).GetComponent<Button>();
        removeUser.onClick.AddListener(RemoveThisPanel);
    }
    private void SetPanelText()
    {
        if (thisPanelUser == null) return;
        userNameText.text = $"{thisPanelUser.userName} {thisPanelUser.userID}";
    }
    private void RemoveThisPanel()
    {
        linkedUserUI.RemoveLinkedUser(this.gameObject);
    }
}
