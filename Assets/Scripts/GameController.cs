using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject pnl_LevelFailed;
    [SerializeField] private GameObject pnl_LevelCOmplete;

    [SerializeField] private PlayerSnake playerSnake;

    public void LevelEnd(bool endValue)
    {
        if(endValue)
            pnl_LevelCOmplete.gameObject.SetActive(true);
        else
            pnl_LevelFailed.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameStarted()
    {
        playerSnake.Started = true;
    }
}
