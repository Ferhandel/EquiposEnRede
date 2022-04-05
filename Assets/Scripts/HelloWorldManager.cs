using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitTeam1();
                SubmitTeam2();
                SubmitNoTeam();  
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

         static void SubmitTeam1()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Choose Team1" : "Choose Team 1" ))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.SubmitTeam1RequestServerRpc();
                  
            }
        }

        static void SubmitTeam2()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Choose Team2" : "Choose Team 2" ))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.SubmitTeam2RequestServerRpc();
                
            }
        }

        static void SubmitNoTeam()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Choose No Team" : "Choose No Team" ))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();

                player.MoveBlueTeam();
                player.SubmitNoTeamRequestServerRpc();
            

            }
        }
    }
}