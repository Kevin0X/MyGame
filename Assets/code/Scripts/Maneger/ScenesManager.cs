using Assets.code.Scripts.Common;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ScenesManager : MonoBehaviour {

    private string curSceneName;

    private UIManager UIManager;

    private void Start()
    {
        UIManager = GameManager.Instance.GetUIMgrIns();

        if (curSceneName == null)
        {
            LoadScene(Constant.ScenesName.LOGIN);
        }
    }

    //切换场景
    public void LoadScene(string sceneName)
    {
        curSceneName = sceneName;
        SceneManager.LoadScene(sceneName);
        LoadUIPnl();
    }

    public string GetCurSceneName()
    {
        return curSceneName;
    }

    //场景切换时，Destroy掉上一场景的所有UI面板，加载当前场景的初始面板
    private void LoadUIPnl()
    {
        UIManager.DestroyAllPnl();
        switch (curSceneName)
        {
            case "Login":
                UIManager.LoadMainCanvas("Login");
                break;
            case "Menu":
                UIManager.LoadMainCanvas("HeroSelect");
                break;
            case "Level1":
                UIManager.LoadMainCanvas("Fight");
                break;
            case "Level2":
                UIManager.LoadMainCanvas("");
                break;
            case "GameOver":
                UIManager.LoadMainCanvas("GameOver");
                break;
        }
    }

    
}
