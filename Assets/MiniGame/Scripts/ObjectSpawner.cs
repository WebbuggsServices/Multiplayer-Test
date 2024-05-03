using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ObjectSpawner : MonoBehaviour
{
    PhotonView photonView;
    public static ObjectSpawner Instance;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform spawnPoint;

    private Coroutine spawnCoroutine;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
    }
    public void StartSpawning(float intervalMin, float intervalMax)
    {
        Debug.Log("true");
        isRunning = true;
        spawnCoroutine = StartCoroutine(Spawner(intervalMin, intervalMax));
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
        isRunning = false;
    }

    public bool isPicked = false;
    IEnumerator Spawner(float intervalMin,float intervalMax)
    {
        //Debug.Log("AA" + isRunning + isPicked);
        while (isRunning)
        {
            yield return new WaitForSeconds(Random.Range(intervalMin, intervalMax));
            if (isPicked == false && PhotonNetwork.IsMasterClient)
            {
                isPicked = true;
                Debug.Log("Spawn");
                PhotonNetwork.Instantiate(objectToSpawn.name, spawnPoint.position, Quaternion.identity);
            }
        }
    }
    

}
