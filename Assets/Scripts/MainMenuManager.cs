using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuCamera;
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private GameObject mapHubPrefab;

    private GameObject hub;

    private string playerParentPrefabPath;
    private GameObject playerParentPrefab;

    private MainMenuState state;

    private Vector3 spawnPoint;

    private MainMenuManager()
    {
        spawnPoint = Vector3.zero;

        playerParentPrefabPath = "PlayerTemplatePrefab/PlayerTemplate";
    }

    private void Start()
    {
        state = MainMenuState.MAIN;

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        playerParentPrefab = Resources.Load<GameObject>(playerParentPrefabPath);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == MainMenuState.IN_GAME)
            {
                mainMenuCamera.SetActive(true);
                playerCamera.SetActive(false);
                PhotonNetwork.Destroy(StaticObjects.Player.transform.parent.gameObject);
                StaticObjects.Player = null;
                StaticObjects.PlayerCamera = null;
                state = MainMenuState.CHARACTER_SELECT;
            }
        }
    }

    private void OnGUI()
    {
        switch (state)
        {
            case MainMenuState.MAIN:
                if (GUILayout.Button("Connect", GUILayout.Height(40)))
                {
                    state = MainMenuState.CONNECTING;
                    PhotonNetwork.ConnectUsingSettings("MOBA v1.0.0");
                }
                if (GUILayout.Button("Quit", GUILayout.Height(40)))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
                break;
            case MainMenuState.CONNECTING:
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
                break;
            case MainMenuState.CHARACTER_SELECT:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + " - Players: " + PhotonNetwork.room.PlayerCount);
                if (GUILayout.Button("Fighter", GUILayout.Height(40)))
                {
                    SpawnPlayer("Fighter");
                }
                if (GUILayout.Button("Gunner", GUILayout.Height(40)))
                {
                    SpawnPlayer("Gunner");
                }
                if (GUILayout.Button("Mage", GUILayout.Height(40)))
                {
                    SpawnPlayer("Mage");
                }
                if (GUILayout.Button("Quit", GUILayout.Height(30)))
                {
                    PhotonNetwork.Disconnect();
                    Destroy(hub);
                    state = MainMenuState.MAIN;
                }
                break;
            case MainMenuState.IN_GAME:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + " - Players: " + PhotonNetwork.room.PlayerCount);
                break;
        }
    }

    private void OnJoinedLobby()
    {
        hub = Instantiate(mapHubPrefab, Vector2.zero, Quaternion.identity);
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    private void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length > 1)
        {
            StartCoroutine(LoadEntities());
        }
        state = MainMenuState.CHARACTER_SELECT;
    }

    private void SpawnPlayer(string characterName)
    {
        state = MainMenuState.IN_GAME;

        GameObject playerTemplate = Instantiate(playerParentPrefab);
        GameObject player = PhotonNetwork.Instantiate(characterName, spawnPoint, Quaternion.identity, 0);
        player.transform.parent = playerTemplate.transform;
        StaticObjects.Player = player.GetComponent<Player>();
        StaticObjects.PlayerCamera = playerCamera.GetComponent<Camera>();

        playerCamera.SetActive(true);
        mainMenuCamera.SetActive(false);
    }

    private IEnumerator LoadEntities()
    {
        yield return null;

        foreach (Entity entity in FindObjectsOfType<Entity>())
        {
            entity.SendToServer_ConnectionInfoRequest();
        }
    }
}

enum MainMenuState
{
    MAIN,
    CONNECTING,
    CHARACTER_SELECT,
    IN_GAME,
}
