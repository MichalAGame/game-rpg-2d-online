using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks,ILobbyCallbacks
{
    public string playerName;
    public GameObject nameInput;
    [Header("Screnns")]
    public GameObject mainScreen;
    public GameObject createRoomScreen;
    public GameObject lobbyScreen;
    public GameObject lobbyBrowserScreen;
    [Header("Main Screen")]
    public Button createRoomButton;
    public Button findRoomButton;

    [Header("Lobby")]
    public TextMeshProUGUI playerListText;
    public TextMeshProUGUI roomInfoText;
    public Button startGameButton;

    [Header("LobbyBrowser")]
    public RectTransform roomListContainer;
    public GameObject roomButtonPrefabs;

    private List<GameObject> roomButtons = new List<GameObject>();
    private List<RoomInfo> roomList = new List<RoomInfo>();
    // Start is called before the first frame update
    void Start()
    {
        createRoomButton.interactable = false;
        findRoomButton.interactable = false;

        Cursor.lockState = CursorLockMode.None;

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
        //Usuñ nazwê gracza
        //PlayerPrefs.DeleteKey("Name");
        if (PlayerPrefs.HasKey("Name"))
        {
            playerName = PlayerPrefs.GetString("Name");
            PhotonNetwork.NickName = playerName;
            nameInput.SetActive(false);
        }
        else
        {
            nameInput.SetActive(true);
        }
    }

    public void SetScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        lobbyBrowserScreen.SetActive(false);
        createRoomScreen.SetActive(false);
        lobbyScreen.SetActive(false);

        screen.SetActive(true);

        if (screen == lobbyBrowserScreen)
            UpdateLobbyBroserUI();
    }

    public void OnPlayerNameChanged(TMP_InputField playerNameInput)
    {
        playerName = playerNameInput.text;

        if (playerName.Length > 2)
            PlayerPrefs.SetString("Name", playerName);
        PhotonNetwork.NickName = playerName;
        AudioManager.instance.PlaySFX(1);
    }

    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        findRoomButton.interactable = true;
    }

    public void OnScreenRoomButton()
    {
        if (playerName.Length < 2)
        {
            return;
        }
        else
            SetScreen(createRoomScreen);
        AudioManager.instance.PlaySFX(1);
    }

    public void OnbackToMainScreen()
    {
        SetScreen(mainScreen);
        AudioManager.instance.PlaySFX(1);
    }

    public void OnFindRoomButton()
    {
        AudioManager.instance.PlaySFX(1);
        if (playerName.Length < 2)
        {
            return;
        }
        else
        {
            SetScreen(lobbyBrowserScreen);
        }
    }

    public void OncreateButton(TMP_InputField roomNameInput)
    {
        if (roomNameInput.text.Length < 2)
            return;
        else
            NetworkManager.instance.CreateRoom(roomNameInput.text);
        AudioManager.instance.PlaySFX(1);
    }

    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
        AudioManager.instance.PlaySFX(1);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AudioManager.instance.PlaySFX(1);
        UpdateLobbyUI();
    }

    [PunRPC]
    void UpdateLobbyUI()
    {
        startGameButton.interactable = PhotonNetwork.IsMasterClient;

        playerListText.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
            playerListText.text += player.NickName + "\n";

        roomInfoText.text = "<b>Serwer </b> \n" + PhotonNetwork.CurrentRoom.Name;
    }

    public void OnstartGameButton()
    {
        AudioManager.instance.PlaySFX(1);
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;

        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }

    public void OnleaveLobbyButton()
    {
        AudioManager.instance.PlaySFX(1);
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    GameObject CreateRoomButton()
    {
        GameObject buttonObject = Instantiate(roomButtonPrefabs, roomListContainer.transform);
        roomButtons.Add(buttonObject);
        return buttonObject;
    }

    void UpdateLobbyBroserUI()
    {
        foreach(GameObject button in roomButtons)
        {
            button.SetActive(false);
        }

        for(int x=0; x < roomList.Count; x++)
        {
            GameObject button = x >= roomButtons.Count ? CreateRoomButton() : roomButtons[x];

            button.SetActive(true);
            button.transform.Find("Room name Text").GetComponent<TextMeshProUGUI>().text = roomList[x].Name;
            button.transform.Find("Player Counter Text").GetComponent<TextMeshProUGUI>().text = roomList[x].PlayerCount + " / " + roomList[x].MaxPlayers;

            Button buttoncomp = button.GetComponent<Button>();
            string roomName = roomList[x].Name;

            buttoncomp.onClick.RemoveAllListeners();
            buttoncomp.onClick.AddListener(() => { OnJoinRoomButton(roomName); });
        }
    }

    public void OnRefreshButton()
    {
        AudioManager.instance.PlaySFX(1);
        UpdateLobbyBroserUI();
    }

    public void OnJoinRoomButton(string roomName)
    {
        AudioManager.instance.PlaySFX(1);
        NetworkManager.instance.JoinRoom(roomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> allRooms)
    {
        roomList = allRooms;
    }



}
