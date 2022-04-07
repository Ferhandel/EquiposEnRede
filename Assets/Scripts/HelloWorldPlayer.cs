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

            //SubmitNoTeamPositionServerRpc(true);
            //SubmitRedTeamPositionServerRpc(true);
            //SubmitBlueTeamPositionServerRpc(true);             
        }   
        public void OnPositionChange(Vector3 previusValue, Vector3 newValue){
            transform.position = Position.Value;
        }
        public override void OnNetworkSpawn()
        {
            if (IsServer && IsOwner){
                Move(0);

            }
             

        }
        public void Move(int team){
            SubmitTeamPositionServerRpc(team);

        }
        [ServerRpc]
        void SubmitTeamPositionServerRpc(int team, ServerRpcParams rpcParams = default){
            if (team == 0){
                colorPlayer.Value = Color.white;
                Position.Value = new Vector3(Random.Range(-1.5f,1.5f),1f,Random.Range(-3f,3f));
                Debug.Log("JUGADOR SIN EQUIPO");
            }
            if (team == 1){
                colorPlayer.Value = Color.blue;
                Position.Value = new Vector3(Random.Range(-3f, -1.5f),1f,Random.Range(-3f,3f));
                Debug.Log("JUGADOR EQUIPO A AZUL");
            }
            if (team == 2){
                colorPlayer.Value = Color.red;
                Position.Value = new Vector3(Random.Range(1.5f,3f),1f,Random.Range(-3f,3f));
                Debug.Log("JUGADOR A EQUIPO ROJO");
            }
        }
      

        void Update()
        {
            ren.material.SetColor("_Color", colorPlayer.Value);
            transform.position = Position.Value;
        }
    }
}
