using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private float intervalMin;
    [SerializeField] private float intervalMax;
    [SerializeField] private ObjectSpawner[] spawners;

    private int myScore,enemyScore = 0;
    [SerializeField] private int maxTime = 60;
    private int currentTime = 0;
    
    private WaitForSeconds oneSec = new WaitForSeconds(1);

    private PlayerInput playerInput;
    public bool isPlaying { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        //playerInput.OnStart -= StartGame;
    }

    public void StartGame()
    {
        Debug.Log("AA");
        currentTime = maxTime;
        foreach (var spawner in spawners)
        {
            spawner.StartSpawning(intervalMin, intervalMax);
        }

        myScore = 0;
        enemyScore = 0;
        StartCoroutine(GamePlay());
        isPlaying = true;
    }

    public void StopGame()
    {
        foreach (var spawner in spawners)
        {
            spawner.StopSpawning();
        }

        foreach(var spawnObjects in FindObjectsOfType<SpawnObject>())
        {
            Destroy(spawnObjects.gameObject);
        }
        isPlaying = false;
    }

    IEnumerator GamePlay()
    {
        while(currentTime > 0)
        {
            currentTime--;
            yield return oneSec;
        }

        StopGame();
    }

    public void AddPoint(bool isMine)
    {
        if (!isPlaying) return;

        if (isMine == true)
            myScore++;
        else
            enemyScore++;
    }


    private void OnGUI()
    {
       if (!isPlaying) return;
        
       ShowScoreUI();
       ShowTimerUI();
        
    }

    private void ShowScoreUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 40;
        GUI.Label(new Rect(10, 10, 500, 100), $"My Score: {myScore}",style);
        GUI.Label(new Rect(10, 50, 500, 100), $"Enemy Score: {enemyScore}", style);
    }

    private void ShowTimerUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 40;
        GUI.Label(new Rect(10, 90, 500, 100), currentTime>0 ? $"Time: {currentTime}" : "Game Over",style);
    }

}
