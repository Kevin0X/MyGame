using Assets.code.Scripts.Common;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    private InputField InputCUser;
    private InputField InputCPassword;
    private Button BtnPrivateLogin;
    private Button BtnQQ;
    private Button BtnWeChat;

    private ScenesManager ScenesManager;

    void Start () {
        ScenesManager = GameManager.Instance.GetScenesMgrIns();

        Init();
    }

    //初始化登陆面板
    void Init()
    {
        Transform self = this.GetComponent<Transform>();

        InputCUser =        self.Find("SlopeLayer/PnlLogin/InputCUser").GetComponent<InputField>();
        InputCPassword =    self.Find("SlopeLayer/PnlLogin/InputCPassword").GetComponent<InputField>();
        BtnPrivateLogin =   self.Find("SlopeLayer/PnlLogin/BtnPrivateLogin").GetComponent<Button>();
        BtnQQ =             self.Find("SlopeLayer/BtnQQ").GetComponent<Button>();
        BtnWeChat =         self.Find("SlopeLayer/BtnWeChat").GetComponent<Button>();

        BtnQQ.onClick.AddListener(() => { });
        BtnWeChat.onClick.AddListener(() => { });
        BtnPrivateLogin.onClick.AddListener(() => { LoginClick(); });
    }

    //按钮按下场景跳转
    public void LoginClick(){
        Debug.Log(InputCUser.text + InputCPassword.text);
        ScenesManager.LoadScene(Constant.ScenesName.MENU);
    }

    private void OnDestroy()
    {
        InputCUser = null;
        InputCPassword = null;
        BtnPrivateLogin = null;
    }
}
