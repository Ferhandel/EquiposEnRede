using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        //Lista de Colores
        public NetworkVariable<Color> colorPlayer= new NetworkVariable<Color>();
        public static List<Color> avaliableColors = new List<Color>();

        Renderer ren;

        //START
        private void Start(){
            Position.OnValueChanged += OnPositionChange;
            ren = GetComponent<Renderer>();
            if (IsServer && IsOwner){
                avaliableColors.Add(Color.blue);
                avaliableColors.Add(Color.red);
                avaliableColors.Add(Color.white);
                SubmitRedTeamPositionServerRpc(true);
                SubmitNoTeamPositionServerRpc(true);
                SubmitBlueTeamPositionServerRpc(true);
                Debug.Log(avaliableColors.Count);
            }          
        }   
        public void OnPositionChange(Vector3 previusValue, Vector3 newValue){
            transform.position = Position.Value;
        }
        public override void OnNetworkSpawn()
        {
             if (IsOwner && IsClient)
            {  
                MoveRedTeam();
                MoveBlueTeam();
                MoveNoTeam();
            }
        }
        //NoTeam
        public void MoveNoTeam(){
            SubmitNoTeamPositionServerRpc();
        }
        [ServerRpc]
        void SubmitNoTeamPositionServerRpc(bool primeritaVez = false, ServerRpcParams rpcParams = default){
            Position.Value = GetNoTeamPosition();
            Color oldColor = colorPlayer.Value;
            Color NewNoTeamColor = avaliableColors[2];
            avaliableColors.Remove(NewNoTeamColor);
            if(!primeritaVez){
                return;
            }
            colorPlayer.Value = NewNoTeamColor;
        }
        static Vector3 GetNoTeamPosition(){
            return new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-3, -3));
        }

        //Equipo rojo.
        public void MoveRedTeam()
        {
           SubmitRedTeamPositionServerRpc();
        }
        //RPC
        [ServerRpc]
        public void SubmitRedTeamPositionServerRpc(bool primeritaVez = false, ServerRpcParams rpcParams = default){
            Position.Value = GetRedTeamPosition();
            Color oldColor = colorPlayer.Value;
            Color newRedTeamColor = avaliableColors[0];
            avaliableColors.Remove(newRedTeamColor);
            if(!primeritaVez){
                return;
            }
            colorPlayer.Value = newRedTeamColor;
        }
        static Vector3 GetRedTeamPosition(){
            return new Vector3(Random.Range(-15f, -7f), 1f, Random.Range(-3f, 3f));
        }
        //Equipo Azul.
        public void MoveBlueTeam(){
            SubmitBlueTeamPositionServerRpc();
        }

        [ServerRpc]
        void SubmitBlueTeamPositionServerRpc(bool primeritaVez = false, ServerRpcParams rpcParams = default){
            Position.Value = GetBlueTeamPosition();
            Color oldColor = colorPlayer.Value;
            Color newBlueTeamColor = avaliableColors[1];
            avaliableColors.Remove(newBlueTeamColor);
            if(!primeritaVez){
                return;
            }
            colorPlayer.Value = newBlueTeamColor;
        }
        
        static Vector3 GetBlueTeamPosition(){
            return new Vector3(Random.Range(15f, 7f), 1f, Random.Range(3f, -3f));
        }





        void Update()
        {
            ren.material.SetColor("_Color", colorPlayer.Value);
        }
    }
}
