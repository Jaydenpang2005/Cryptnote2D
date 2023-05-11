using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using IngameDebugConsole;

public class LobbyScript : MonoBehaviour
{
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;
    private string playerName;

    private async void Start(){
        //Setting up Console Commands
        DebugLogConsole.AddCommand( "ListLobby", "List All Lobbies", ListLobbies);
        DebugLogConsole.AddCommand( "CreateLobby", "Create a Lobby", CreateLobby);
        DebugLogConsole.AddCommand<string>( "JoinLobby", "Join a Lobby", JoinLobbyByCode);
        DebugLogConsole.AddCommand( "QuickJoinLobby", "Join a Lobby", QuickJoinLobby);
        DebugLogConsole.AddCommand<string>( "UpdateLobbyGameMode", "Update the Gamemode", UpdateLobbyGameMode);
        DebugLogConsole.AddCommand( "PrintPlayers", "Print all players", PrintPlayers);
        DebugLogConsole.AddCommand<string>( "UpdatePlayerName", "Update player names", UpdatePlayerName);
        DebugLogConsole.AddCommand( "LeaveLobby", "Leave lobby", LeaveLobby);
        //Initialization
        playerName = "Doll" + UnityEngine.Random.Range(10, 99);
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log(playerName);
    }

    private void Update(){
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyHeartbeat(){
        if(hostLobby!=null)
        {
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f){
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdates(){
        if(joinedLobby!=null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if(lobbyUpdateTimer < 0f){
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }
    }
    private async void CreateLobby(){
        try{
            string lobbyName = "Game Lobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
                IsPrivate = false,
                Player = GetPlayer(),
                 Data = new Dictionary<string, DataObject>{
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "FFA")}, 
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "Dungeon")}
                 }
                
                
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            hostLobby = lobby;
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
        }
        catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void ListLobbies() {
        try{
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Count = 25, 
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                 Order = new List<QueryOrder>{
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                 }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
                foreach (Lobby lobby in queryResponse.Results)
                {
                    Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
                }
            }
        catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void JoinLobbyByCode(string lobbyCode){
        try{
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = GetPlayer()
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            Debug.Log("Joined Lobby with code " + lobbyCode);

            joinedLobby = lobby;
            PrintPlayers(lobby);
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void QuickJoinLobby(){
        try{
            await LobbyService.Instance.QuickJoinLobbyAsync();
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }

    }

    private Player GetPlayer(){
        return new Player {
            Data = new Dictionary<string, PlayerDataObject> {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
            }
        };
    }

    private void PrintPlayers(){
        PrintPlayers(joinedLobby);
    }
    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value + lobby.Data["Map"].Value);
        foreach(Player player in lobby.Players){
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    private async void UpdateLobbyGameMode(string gameMode){
        try{
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
                Data = new Dictionary<string, DataObject>{
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode)}
                }
            });

            joinedLobby = hostLobby;
            PrintPlayers(joinedLobby);
    } catch(LobbyServiceException e){
        Debug.Log(e);
    }
    }

    private async void UpdatePlayerName(string newPlayerName){
        try{
        playerName = newPlayerName;
        await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions{
         Data = new Dictionary<string, PlayerDataObject> {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
            }
        });
        } catch(LobbyServiceException e){
        Debug.Log(e);
        }
    }

    private async void LeaveLobby(){
        try{
        await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        } catch(LobbyServiceException e){
        Debug.Log(e);
        }
    }

    private async void KickPlayer(){
        try{
        await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        } catch(LobbyServiceException e){
        Debug.Log(e);
        }
    }

    private async void MigrateLobbyHost(){
        try{
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
                HostId = joinedLobby.Players[1].Id
            });

            joinedLobby = hostLobby;
            PrintPlayers(joinedLobby);
    } catch(LobbyServiceException e){
        Debug.Log(e);
    }
    }

}