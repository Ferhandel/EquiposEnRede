using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        Renderer rend;
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<Color> ColorPlayer = new NetworkVariable<Color>();
        public static List<Color> colorList = new List<Color>();
        public static List<HelloWorldPlayer> Team1 = new List<HelloWorldPlayer>();
        public static List<HelloWorldPlayer> Team2 = new List<HelloWorldPlayer>();
        public static List<HelloWorldPlayer> NoTeam = new List<HelloWorldPlayer>();

        void Start(){
            rend = GetComponent<MeshRenderer>();
            Position.OnValueChanged += OnPositionChange;
            ColorPlayer.OnValueChanged += OnColorChange;

            colorList.Add(Color.blue);
            colorList.Add(Color.red);
            colorList.Add(Color.white);
        }
        public override void OnNetworkDespawn()
        {
            if (IsOwner){
                SubmitPositionRequestServerRpc();
            }
        }

        //Team1
        public void ChangeColorTeam1(){
            ColorPlayer.Value = colorList[0];
        }
        public void ChangeColorTeam2(){
            ColorPlayer.Value = colorList[1];
        }
        public void ChangeColorNoTeam(){
            ColorPlayer.Value = colorList[2];
        }
        public void OnPositionChange(Vector3 previousValue , Vector3 newValue){
            transform.position = Position.Value;
        }
        public void OnColorChange(Color previousValue, Color newValue){
           rend.material.color = ColorPlayer.Value;
        }

        [ServerRpc]
        public void SubmitPositionRequestServerRpc( ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }
        public Vector3 GetRandomPositionOnPlane(){
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        [ServerRpc]
        public void SubmitTeam1RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetPositionTeam1();
            ChangeColorTeam1();
            Team1.Add(new HelloWorldPlayer());
            Debug.Log(Team1.Count);     
        }
        public Vector3 GetPositionTeam1(){
             return new Vector3(Random.Range(-6f, -4f), 1f, Random.Range(-3f, 3f));
        }

        [ServerRpc]
        public void SubmitTeam2RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetPositionTeam2();
            ChangeColorTeam2();
            Team2.Add(new HelloWorldPlayer());
            Debug.Log(Team2.Count);     
        }
        public Vector3 GetPositionTeam2(){
             return new Vector3(Random.Range(6f, 4f), 1f, Random.Range(-3f, 3f));
        }

        [ServerRpc]
        public void SubmitNoTeamRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetPositionNoTeam();
            ChangeColorNoTeam();
            NoTeam.Add(new HelloWorldPlayer());
            Debug.Log(NoTeam.Count);     
        }
        public Vector3 GetPositionNoTeam(){
             return new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-3f, 3f));
        }

        

        

    }
}
