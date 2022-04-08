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
        public NetworkVariable<int> numeroEquipo= new NetworkVariable<int>();
        public static List<int> teams = new List<int>();
        public static int maxPlayersOnTeam = 2;
        //teams[numeroEquipo]
        Renderer ren;

        //START
        private void Start(){
            Position.OnValueChanged += OnPositionChange;
            ren = GetComponent<Renderer>();           
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
            if (team == 0f){
                teams[0]++;
                colorPlayer.Value = Color.white;
                Position.Value = new Vector3(Random.Range(-1.5f,1.5f),1f,Random.Range(-3f,3f));
                numeroEquipo.Value = 0;
                Debug.Log("BIENVENIDO//JUGADOR SIN EQUIPO");
            }else if (team == 0){
                teams[0]++;
                teams[numeroEquipo.Value]--;
                colorPlayer.Value = Color.white;
                Position.Value = new Vector3(Random.Range(-1.5f, 1.5f),1f,Random.Range(-3f,3f));
                numeroEquipo.Value = 0;
                Debug.Log("JUGADOR SIN EQUIPO");
            }else if (team == 1){
                if(teams[1] < maxPlayersOnTeam){
                teams[1]++;
                teams[numeroEquipo.Value]--;
                colorPlayer.Value = Color.blue;
                Position.Value = new Vector3(Random.Range(-1.5f,-3f),1f,Random.Range(-3f,3f));
                numeroEquipo.Value = 1;
                Debug.Log("JUGADOR A EQUIPO AZUL");
               } else{
                    if(numeroEquipo.Value == team){
                    colorPlayer.Value = Color.blue;
                    Position.Value = new Vector3(Random.Range(-1.5f,-3f),1f,Random.Range(-3f,3f));
                } else{
                    Debug.Log("QUIETO PARAO. El equipo azul esta completo, colega.");
                }
               }
            } 
            if(team == 2){
                if(teams[2] < maxPlayersOnTeam){
                teams[2]++;
                teams[numeroEquipo.Value]--;
                colorPlayer.Value = Color.red;
                Position.Value = new Vector3 (Random.Range(1.5f,3f),1f,Random.Range(-3f,3f));
                numeroEquipo.Value = 2;
                Debug.Log("JUGADOR A EQUIPO ROJO");
              }else{
                  Debug.Log("TRANQUILO AMIGO. Afóro máximo del equipo Rojo debido al coronavirus :(");
              }
            }
            Debug.Log(teams[0] + " " + teams[1]+ " " + teams[2]);
            
            
        }
      

        void Update()
        {
            ren.material.SetColor("_Color", colorPlayer.Value);
            transform.position = Position.Value;
        }
    }
}
