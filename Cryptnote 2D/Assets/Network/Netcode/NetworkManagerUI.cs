using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using IngameDebugConsole;
using TMPro;
using Unity.Services.Authentication;
using System;

public class NetworkManagerUI : MonoBehaviour
{
    public GameObject buttons;
    public GameObject loading;
    public Button JoinGameBtn;
    public TMP_Text codeText;
    public TMP_InputField InputUser;
    [SerializeField] private string Code;
    private UnityTransport _transport;
    private void Awake()
    {
        DebugLogConsole.AddCommand( "CreateGame", "Create a game", CreateGame);
        DebugLogConsole.AddCommand<string>( "JoinGame", "Join a game", JoinGame);
        _transport = FindObjectOfType<UnityTransport>();
    }


    private void Start()
    {
        JoinGameBtn.onClick.AddListener(GetInputOnClickHandler);
        // try
        // {
        //     await UnityServices.InitializeAsync();
        //     await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //     Debug.Log($"Player Id: {AuthenticationService.Instance.PlayerId}");
        // }
        // catch(Exception e){
        //     Debug.LogError(e);
        //     return;
        // }
    }

    public async void CreateGame(){
        ServerManager.Instance.StartHost();
    }
    public async void JoinGame(string Code){
        ClientManager.Instance.StartClient(Code);
    }

    public void GetInputOnClickHandler(){
        JoinGame(InputUser.text);
    }
}
