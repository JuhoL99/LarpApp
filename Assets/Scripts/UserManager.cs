using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button createUserButton;
    [SerializeField] private Button verificationButton;
    [Header("Other UI Elements")]
    [SerializeField] private GameObject userCreationPanel;
    [SerializeField] private GameObject verificationPopup;
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_Text warningText;
    [Header("Variables")]
    [SerializeField] private int maxNameLength;
    [SerializeField] private int minNameLength;
    private bool isNameValid;

    private void Start()
    {
        EnableListeners();
        verificationPopup.SetActive(false);
    }
    private void EnableListeners()
    {
        createUserButton.onClick.AddListener(VerifyUserCreation);
        verificationButton.onClick.AddListener(CreatePlayerUser);
        userNameInputField.onValueChanged.AddListener(ValidateName);
    }
    private void DisableListeners()
    {
        createUserButton.onClick.RemoveListener(VerifyUserCreation);
        verificationButton.onClick.RemoveListener(CreatePlayerUser);
        userNameInputField.onValueChanged.RemoveListener(ValidateName);
    }
    private void VerifyUserCreation()
    {
        if (!isNameValid) return;
        if(GameManager.instance.player != null) verificationPopup.SetActive(true);
        else CreatePlayerUser();
    }
    private void ValidateName(string text)
    {
        string userName = userNameInputField.text;
        if(userName == null)
        {
            NameWarningText("name null, try again");
            isNameValid = false;
            return;
        }
        if(userName.Length < minNameLength)
        {
            NameWarningText($"name too short, try more than {minNameLength} characters");
            isNameValid = false;
            return;
        }
        if(userName.Length > maxNameLength)
        {
            NameWarningText($"name too long, try less than {maxNameLength} characters");
            isNameValid = false;
            return;
        }
        NameWarningText(string.Empty);
        isNameValid = true;
    }
    private void NameWarningText(string msg)
    {
        warningText.text = msg;
    }
    private void CreatePlayerUser()
    {
        if (verificationPopup.activeSelf) verificationPopup.SetActive(false);
        GameManager.instance.player = new User(userNameInputField.text, 0); ;
        DisableListeners();
        userCreationPanel.SetActive(false);
    }
}
