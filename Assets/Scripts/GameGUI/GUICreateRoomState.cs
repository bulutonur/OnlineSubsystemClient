using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

public class GUICreateRoomState : GUIState
{

    [SerializeField] private Button createRoomButton;
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private Toggle passwordToggle;
    [SerializeField] private Transform passwordArea;
    [SerializeField] private TMP_InputField passwordInputField;
    private bool pwprotected = false;
    private void Start()
    {
        passwordToggle.onValueChanged.AddListener(OnPasswordToggle);
    }
    
    public void OnClick_CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            // @TODO Display error message
            Debug.LogError("Room name cant be empty");
            return;
        }
        
        gui.networkManager.CreateServer(roomNameInputField.text,"map",2,"gameMode",pwprotected,passwordInputField.text);
        
    }
    
    
    public void OnClick_Back()
    {
        gui.fsm.ChangeState(gui.lobbyState);
    }

    private void OnPasswordToggle(bool enabled)
    {
        pwprotected = enabled;
        if (pwprotected)
        {
            passwordArea.gameObject.SetActive(true);
        }
        else
        {
            passwordArea.gameObject.SetActive(false);
        }
    }
}
