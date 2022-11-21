using Fusion;
using Fusion.Sockets;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{

    NetworkRunner _runner;

    [SerializeField]
    private NetworkPrefabRef _playerPrefab;

    [SerializeField]
    private NetworkPrefabRef _vanguardTitanPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    async void StartGame(GameMode gameMode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = "WeBallin",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to Server -> " + runner.name);
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("Connection failed to Server -> " + runner.name + " (" + remoteAddress.ToString() + ") " + reason.ToString());
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        // TODO:: for future menu screen.
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        // TODO:: for future authentication.
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("Disconnected from Server -> " + runner.name);
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("Host-Migration!");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkObject playerObject;
        _spawnedCharacters.TryGetValue(runner.LocalPlayer, out playerObject);

        if (playerObject == null) return;

        var data = new NetworkInputData();
        Vector3 currentInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        currentInput = playerObject.gameObject.transform.TransformDirection(currentInput);
        currentInput = Vector3.ClampMagnitude(currentInput, 1f);
        data.direction += currentInput;
        data.isJump = Input.GetKeyDown(KeyCode.Space);

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        // No need for this right now.
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player joined Server -> " + player.PlayerId);
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            NetworkObject networkPlayerTitanObject = runner.Spawn(_vanguardTitanPrefab, spawnPosition, Quaternion.identity, player);
            AccesTitan accesTitan = networkPlayerObject.GetComponent<AccesTitan>();
            accesTitan.titanObject = networkPlayerTitanObject;
            accesTitan.titanScript = networkPlayerTitanObject.GetComponent<EnterVanguardTitan>();

            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player left Server -> " + runner.name);
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject.GetComponent<AccesTitan>().titanObject);
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        // No need for this right now.
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        // TODO:: for future menu screen.
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        // TODO:: for future menu screen.
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        // No need for this right now.
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        // TODO:: for future menu screen.
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        // No need for this right now.
    }
}
