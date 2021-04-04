using UnityEngine;

//public class RemoteSettingsStartup : MonoBehaviour
//{
//    private void Awake()
//    {
//        if(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) // checking if the player have internet
//        {
//            RemoteSettings.Updated += () =>
//             {
//                 GameManager.playerLives = RemoteSettings.GetInt("PlayerStartUpLives", GameManager.playerLives);
//             };
//        }
//    }
//}
