using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverOverlay : MonoBehaviour
{
    public GameObject Contents;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameOver += ShowGameOverOverlay;
        Contents.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= ShowGameOverOverlay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowGameOverOverlay()
    {
        Debug.Log("Game over overlay shown");
        Contents.SetActive(true);
    }

    public void PlayAgain()
    {
        GameManager.Instance.PlayAgain();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
