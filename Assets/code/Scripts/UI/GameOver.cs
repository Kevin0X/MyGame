using Assets.code.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    private Button BtnContinueGame;
    private Button BtnQuitGame;

    private ScenesManager ScenesManager;

    private void Start()
    {
        ScenesManager = GameManager.Instance.GetScenesMgrIns();
        Init();
    }

    //初始化面板
    private void Init()
    {
        Transform self = this.transform;
        BtnContinueGame = self.Find("BtnContinueGame").GetComponent<Button>();
        BtnQuitGame = self.Find("BtnQuitGame").GetComponent<Button>();

        BtnContinueGame.onClick.AddListener(() => { OnContinueGame(); });
        BtnQuitGame.onClick.AddListener(() => { OnQuitGame(); });
    }

    //继续游戏
    private void OnContinueGame()
    {
        //选择继续游戏跳转到Menu场景
        ScenesManager.LoadScene(Constant.ScenesName.MENU);
    }

    //退出游戏
    private void OnQuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        BtnContinueGame = null;
        BtnQuitGame = null;
    }
}
