using Assets.code.Scripts.Common;
using System.Collections;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    private GameObject AxeObj;
    private GameObject EpicClubObj;

    private Transform HandWeaponPos;
    private Transform BackWeaponPos;

    private BoxCollider AxeBC;
    private BoxCollider EpicClubBC;

    private int curShowWeapon = 0;

    void Start()
    {

        HandWeaponPos       = transform.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/HandSword");
        BackWeaponPos       = transform.Find("Hips/BackSword");
        AxeObj              = Instantiate(Resources.Load<GameObject>("Prefabs/Swords/Axe"), HandWeaponPos);
        EpicClubObj         = Instantiate(Resources.Load<GameObject>("Prefabs/Swords/EpicClub"), BackWeaponPos);
        AxeBC               = AxeObj.GetComponent<BoxCollider>();
        EpicClubBC          = EpicClubObj.GetComponent<BoxCollider>();
        AxeBC.enabled       = true;
        EpicClubBC.enabled  = false;
    }

    private void OnEnable()
    {
        //添加武器切换事件
        Messenger.AddListener(Constant.EventName.SWITCH_WEAPON, OnSwitchWeapon);
    }

    private void OnDisable()
    {
        //销毁武器切换事件
        Messenger.RemoveListener(Constant.EventName.SWITCH_WEAPON, OnSwitchWeapon);
    }

    private void OnSwitchWeapon()
    {
        StartCoroutine(SetSwitchWeapon());
    }
    
    //切换武器
    private IEnumerator SetSwitchWeapon()
    {
        //等待动画时间，等待完成执行切换
        yield return new WaitForSeconds(Constant.HeroAniTime.WEAPON_SWITCH_TIME);
        curShowWeapon++;
        curShowWeapon %= 2;
        //设置不同位置武器的父级对象和Collider的激活状态
        switch (curShowWeapon)
        {
            case 0:
                AxeObj.transform.parent = HandWeaponPos;
                AxeBC.enabled = true;
                EpicClubObj.transform.parent = BackWeaponPos;
                EpicClubBC.enabled = false;
                break;
            case 1:
                AxeObj.transform.parent = BackWeaponPos;
                AxeBC.enabled = false;
                EpicClubObj.transform.parent = HandWeaponPos;
                EpicClubBC.enabled = true;
                break;
        }

        //重置武器的本地位置与转向
        AxeObj.transform.localPosition      = Vector3.zero;
        EpicClubObj.transform.localPosition = Vector3.zero;
        AxeObj.transform.localRotation      = Quaternion.Euler(0, 0, 0);
        EpicClubObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnDestroy()
    {
        AxeObj = null;
        EpicClubObj = null;
        HandWeaponPos = null;
        BackWeaponPos = null;
        AxeBC = null;
        EpicClubBC = null;
    }
}
