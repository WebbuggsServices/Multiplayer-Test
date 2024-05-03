using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SpawnObject>())
        {
            //Debug.Log("AA");
            gameManager.AddPoint(other.GetComponent<SpawnObject>().isMine);
            Destroy(other.gameObject);
        }
    }
}
