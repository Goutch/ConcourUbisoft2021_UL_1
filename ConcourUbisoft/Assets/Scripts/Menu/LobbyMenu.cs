using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject RoomListElementPrefab = null;
    [SerializeField] private GameObject ContentPanel = null;
    [SerializeField] private GameObject LobbyPanel = null;
    [SerializeField] private GameObject CreateRoomPanel = null;
    [SerializeField] private GameObject TogglePrivate = null;
    [SerializeField] private GameObject JoinRoomPanel = null;
    [SerializeField] private LoadScreenMenuController LoadScreenMenuController = null;

    private NetworkController networkController = null;

    #region UI Actions
    public void OpenCreateRoomPanel()
    {
        CreateRoomPanel.SetActive(true);
    }
    public void CreateRoom()
    {
        networkController.CreateRoom(CreateRoomPanel.transform.Find("RoomNameInputField").GetComponent<InputField>().text, TogglePrivate.GetComponent<Toggle>().isOn);
        LoadScreenMenuController.Show("Creating Room...");
        CreateRoomPanel.SetActive(false);
    }
    public void BackFromCreateRoom()
    {
        CreateRoomPanel.SetActive(false);
    }
    public void OpenJoinRoomPanel()
    {
        JoinRoomPanel.SetActive(true);
    }
    public void JoinRoom()
    {
        string text = JoinRoomPanel.transform.Find("RoomNameInputField").GetComponent<InputField>().textComponent.text;
        networkController.JoinRoom(text);
        LoadScreenMenuController.Show("Joining Room...");
        JoinRoomPanel.SetActive(false);
    }
    public void BackFromJoinRoom()
    {
        JoinRoomPanel.SetActive(false);
    }
    #endregion
    #region Unity Callbacks
    private void Awake()
    {
        networkController = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>();
    }
    private void OnEnable()
    {
        networkController.OnRoomListUpdateEvent += NetworkController_OnLobbyRoomListUpdate;
    }
    private void OnDisable()
    {
        networkController.OnRoomListUpdateEvent -= NetworkController_OnLobbyRoomListUpdate;
    }
    #endregion
    #region Event Callbacks
    private void NetworkController_OnLobbyRoomListUpdate(IEnumerable<NetworkController.RoomInformation> roomInformations)
    {
        ClearElementsOfLobbyList();

        roomInformations.All((roomInfo) => {
            AddElementToLobbyList(roomInfo.RoomName, roomInfo.PlayerCount, () => { PhotonNetwork.JoinRoom(roomInfo.RoomName); }); return true;
        });
    }
    #endregion
    #region Private Functions
    private void AddElementToLobbyList(string name, int playerCount, UnityAction action)
    {
        GameObject roomListElement = Instantiate(RoomListElementPrefab, ContentPanel.transform);
        roomListElement.transform.Find("RoomName").GetComponent<Text>().text = name;
        roomListElement.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(action);
        roomListElement.transform.Find("PlayerCount").GetComponent<Text>().text = $"{playerCount.ToString()}/2";
    }
    private void ClearElementsOfLobbyList()
    {
        foreach (Transform child in ContentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion
}