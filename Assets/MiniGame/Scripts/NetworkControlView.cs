using UnityEngine;

public class NetworkControlView : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnHost()
    {
        // Test only, change this to host instead of starting the game
        Debug.Log("Host Pressed");
        //gameManager.StartGame();
    }

    public void OnJoin()
    {
        Debug.Log("Join Pressed");
    }

    private void ShowButtons()
    {
        //if(GUI.Button(new Rect(10, 10, 100, 20), "Host"))
        //{
        //    OnHost();
        //}

        //if (GUI.Button(new Rect(10, 40, 100, 20), "Join"))
        //{
        //    OnJoin();
        //}
    }

    private void OnGUI()
    {
        if(!gameManager.isPlaying)
            ShowButtons();
    }
}
