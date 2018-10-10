using Assets.code.Scripts.Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PnlFight : MonoBehaviour {
    //private Image ImgCHeadIcon;
    private Text TextCName;
    private Text TextCBloodNum;
    private Text TextCCountDowm;
    private Slider SliderCBloodBar;

    private HeroData HeroData;
    private UIManager UIManager;
    private ScenesManager ScenesManager;

    private float maxHp;
    private int time = 180;

    private void Start () {
        HeroData = GameManager.Instance.GetHeroDataIns();
        UIManager = GameManager.Instance.GetUIMgrIns();
        ScenesManager = GameManager.Instance.GetScenesMgrIns();

        Init();
        StartCoroutine(UpdateTime());
    }
    
    //初始化面板
    private void Init()
    {
        Transform self = this.GetComponent<Transform>();

        //ImgCHeadIcon  = self.Find("ImgHead/ImgCHeadIcon").GetComponent<Image>();
        TextCName       = self.Find("ImgHead/TextCName").GetComponent<Text>();
        TextCBloodNum   = self.Find("SliderCBloodBar/TextCBloodNum").GetComponent<Text>();
        SliderCBloodBar = self.Find("SliderCBloodBar").GetComponent<Slider>();
        TextCCountDowm  = self.Find("TextCCountDown").GetComponent<Text>();

        TextCName.text = HeroData.heroName;
        
        maxHp = HeroData.hp;
        OnHpupdate(maxHp);

        UIManager.LoadMainCanvas("Joystick");
    }

    private void OnEnable()
    {
        //添加玩家血条更新事件
        Messenger.AddListener<float>(Constant.EventName.UI_HPUPDATE, OnHpupdate);
    }

    private void OnDisable()
    {
        //销毁玩家血条更新事件
        Messenger.RemoveListener<float>(Constant.EventName.UI_HPUPDATE, OnHpupdate);
    }

    //更新血条长度与数值显示
    private void OnHpupdate(float hp)
    {
        SliderCBloodBar.value = hp / maxHp;
        TextCBloodNum.text = Math.Ceiling(hp).ToString();
    }

    //设置游戏计时器
    private IEnumerator UpdateTime()
    {
        while (time > 0)
        {
            time--;
            int m = time / 60;
            int s = time - m * 60;
            string strTime = "";
            if (m < 10)
            {
                strTime += '0' + m.ToString() + ':';
            }
            else
            {
                strTime += m.ToString() + ':';
            }
            if (s < 10)
            {
                strTime += '0' + s.ToString();
            }
            else
            {
                strTime += s.ToString();
            }
            TextCCountDowm.text = strTime;
            if(time == 0)
            {
                //时间为0时，结束游戏
                ScenesManager.LoadScene(Constant.ScenesName.GAME_OVER);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnDestroy()
    {
        TextCName = null;
        TextCBloodNum = null;
        SliderCBloodBar = null;
        HeroData = null;
    }
}
