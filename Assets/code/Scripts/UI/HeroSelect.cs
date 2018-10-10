using Assets.code.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelect : MonoBehaviour
{
    private HeroData HeroData;
    private UIManager UIManager;
    private ScenesManager ScenesManager;
    private GameObject PlayerUI;

    private Button BtnStart;
    private Button BtnRetreat;
    private Button BtnSilverAdd;
    private Button BtnGoldAdd;
    private Button BtnGoldBarAdd;
    private Button BtnSwitchWeapon;

    private Text TextCSilverNum;
    private Text TextCGoldNum;
    private Text TextCGoldBarNum;

    private Text TextCAttackNum;
    private Text TextCDefNum;
    private Text TextCHpNum;

    private int gold;
    private int silver;
    private int goldBar;

    private void Start () {
        HeroData = GameManager.Instance.GetHeroDataIns();
        UIManager = GameManager.Instance.GetUIMgrIns();
        ScenesManager = GameManager.Instance.GetScenesMgrIns();

        silver  = HeroData.silver;
        gold    = HeroData.gold;
        goldBar = HeroData.goldBar;

        Init();
    }

    //初始化面板
    private void Init()
    {
        Transform self = GetComponent<Transform>();

        BtnStart        = self.Find("BtnStart").GetComponent<Button>();
        BtnRetreat      = self.Find("BtnRetreat").GetComponent<Button>();
        BtnSwitchWeapon = self.Find("BtnSwitchWeapon").GetComponent<Button>();

        BtnSilverAdd    = self.Find("ImgTitle/TitleMsg/SilverMsgBg/BtnSilverAdd").GetComponent<Button>();
        BtnGoldAdd      = self.Find("ImgTitle/TitleMsg/GoldMsgBg/BtnGoldAdd").GetComponent<Button>();
        BtnGoldBarAdd   = self.Find("ImgTitle/TitleMsg/GoldBarMsgBg/BtnGoldBarAdd").GetComponent<Button>();

        TextCSilverNum  = self.Find("ImgTitle/TitleMsg/SilverMsgBg/TextCSilverNum").GetComponent<Text>();
        TextCGoldNum    = self.Find("ImgTitle/TitleMsg/GoldMsgBg/TextCGoldNum").GetComponent<Text>();
        TextCGoldBarNum = self.Find("ImgTitle/TitleMsg/GoldBarMsgBg/TextCGoldBarNum").GetComponent<Text>();

        TextCAttackNum  = self.Find("HeroMsgBg/Msg/ImgAttack/ImgFontBg/TextCAttackNum").GetComponent<Text>();
        TextCDefNum     = self.Find("HeroMsgBg/Msg/ImgDef/ImgFontBg/TextCDefNum").GetComponent<Text>();
        TextCHpNum      = self.Find("HeroMsgBg/Msg/ImgHp/ImgFontBg/TextCHpNum").GetComponent<Text>();

        BtnStart.onClick.AddListener(() => { OnStartGame(); });
        BtnRetreat.onClick.AddListener(() => { OnRetreat(); });
        BtnSwitchWeapon.onClick.AddListener(() => { OnSwitchWeapon(); });

        BtnSilverAdd.onClick.AddListener(() => { OnSilverAdd(); });
        BtnGoldAdd.onClick.AddListener(() => { OnGoldAdd(); });
        BtnGoldBarAdd.onClick.AddListener(() => { OnGoldBarAdd(); });

        InitPnlInfo();
    }

    //开始游戏按钮，场景跳转
    private void OnStartGame()
    {
        ScenesManager.LoadScene(Constant.ScenesName.LEVEL_1);
    }

    //后退按钮，场景跳转到登陆界面
    private void OnRetreat()
    {
        ScenesManager.LoadScene(Constant.ScenesName.LOGIN);
    }

    private void OnSwitchWeapon()
    {

    }

    private void OnSilverAdd()
    {
        silver++;
        TextCSilverNum.text = silver.ToString();
        HeroData.silver = silver;
    }

    private void OnGoldAdd()
    {
        gold++;
        TextCGoldNum.text = gold.ToString();
        HeroData.gold = gold;
    }

    private void OnGoldBarAdd()
    {
        goldBar++;
        TextCGoldBarNum.text = goldBar.ToString();
        HeroData.goldBar = goldBar;
    }

    //初始化面板显示
    private void InitPnlInfo()
    {
        PlayerUI = UIManager.Load3dModel();

        TextCSilverNum.text = silver.ToString();
        TextCGoldNum.text = gold.ToString();
        TextCGoldBarNum.text = goldBar.ToString();

        TextCAttackNum.text = HeroData.attack.ToString();
        TextCDefNum.text = HeroData.def.ToString();
        TextCHpNum.text = HeroData.hp.ToString();
    }

    private void OnDestroy()
    {
        Destroy(PlayerUI);

        BtnStart = null;
        BtnRetreat = null;
        BtnSilverAdd = null;
        BtnGoldAdd = null;
        BtnGoldBarAdd = null;
        BtnSwitchWeapon = null;

        TextCSilverNum = null;
        TextCGoldNum = null;
        TextCGoldBarNum = null;

        TextCAttackNum = null;
        TextCDefNum = null;
        TextCHpNum = null;

        HeroData = null;
        PlayerUI = null;
    }
}
