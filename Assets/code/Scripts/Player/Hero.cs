using System.Collections;
using Assets.code.Scripts.Common;
using UnityEngine;
using UnityEngine.AI;

public class Hero : MonoBehaviour {

    enum HeroState
    {
        run = 1,    //移动状态
        idle = 2,   //停滞状态
        swop = 3,   //切换武器状态
        attack = 4, //攻击状态
    }

    [SerializeField]
    private float distanceH = 10f;
    [SerializeField]
    private float distanceV = 10f;

    private float horizontal;
    private float vertical;

    private float swopTimer = 0f;
    private float attackTimer = 0f;

    private float idleTime = 0f;

    private float hp = 0f;

    private HeroState heroState = HeroState.idle;

    private Camera MainCanvas;
    private Animator HeroAni;
    private NavMeshAgent NavMeshAgent;
    private CharacterController HeroCC;

    private HeroData HeroData;

    private void Start () {
        HeroData = GameManager.Instance.GetHeroDataIns();

        HeroCC = GetComponent<CharacterController>();
        HeroAni = GetComponent<Animator>();
        NavMeshAgent = GetComponent<NavMeshAgent>();

        MainCanvas = Camera.main;
        NavMeshAgent.enabled = false;
        NavMeshAgent.enabled = true;

        hp = HeroData.hp;
    }

    private void OnEnable()
    {
        //添加攻击事件
        Messenger.AddListener(Constant.EventName.HERO_ATTACK, PlayAttackAni);
        //添加武器切换事件
        Messenger.AddListener(Constant.EventName.SWITCH_WEAPON, PlaySwitchWeapon);
        //添加角色扣血事件
        Messenger.AddListener<float>(Constant.EventName.HERO_HPUPDATE, HpUpdate);
    }

    private void OnDisable()
    {
        //删除攻击事件
        Messenger.RemoveListener(Constant.EventName.HERO_ATTACK, PlayAttackAni);
        //删除武器切换事件
        Messenger.RemoveListener(Constant.EventName.SWITCH_WEAPON, PlaySwitchWeapon);
        //删除角色扣血事件
        Messenger.RemoveListener<float>(Constant.EventName.HERO_HPUPDATE, HpUpdate);
    }

    private void LateUpdate()
    {
        //设置镜头跟随
        if(MainCanvas != null)
        {
            //利用角色位置加上水平与垂直的距离得出摄像机的位置
            Vector3 nextpos = Vector3.forward * distanceH + Vector3.up * distanceV + transform.position;
            MainCanvas.transform.position = nextpos;
            //相机一直看着角色
            MainCanvas.transform.LookAt(transform.position);
        }
    }

    private void Update ()
    {
        Move();
        SetAnimation();
        AttackAniTimer();
        SwopAniTimer();
    }

    private void HpUpdate(float dam)
    {
        hp -= dam;
        Messenger.Broadcast<float>(Constant.EventName.UI_HPUPDATE, hp);
        if (hp <= 0)
        {
            GameManager.Instance.GetScenesMgrIns().LoadScene(Constant.ScenesName.GAME_OVER);
        }
    }

    private void Move()
    {
        //只有当状态不为攻击与武器切换时才允许移动
        if (heroState == HeroState.idle || heroState == HeroState.run)
        {
            horizontal = Joystick.horizontal;
            vertical = Joystick.vertical;
            Vector3 moveForward = Vector3.zero;
            if (Camera.main)
            {
                Vector3 forward = Camera.main.transform.forward;
                if (forward.y < -0.999f)
                {
                    forward = Camera.main.transform.up;
                }
                forward.y = 0.0f;
                forward.Normalize();
                Vector3 right = Camera.main.transform.right;

                moveForward = forward * vertical + right * horizontal;
                moveForward.Normalize();
            }

            if (moveForward != Vector3.zero)
            {
                heroState = HeroState.run;
                //控制旋转
                HeroCC.transform.rotation = Quaternion.LookRotation(moveForward);
                //控制移动
                HeroCC.Move(HeroCC.transform.forward * HeroData.speed * Time.deltaTime);

                //heroCC.transform.position += heroCC.transform.forward * HeroData.speed * Time.deltaTime;
            }
            else
            {
                //停止移动
                heroState = HeroState.idle;
            }
        }
    }

    private void SetAnimation()
    {
        switch (heroState)
        {
            case HeroState.run: PlayRunAni(); break;
            case HeroState.idle: StopRunAni(); break;
        }
    }

    //随机播放Idle动画
    private void PlayIdleAni()
    {
        int num = Random.Range(1, 4);
        switch (num)
        {
            case 1: HeroAni.SetTrigger("Idle1"); break;
            case 2: HeroAni.SetTrigger("Idle2"); break;
            case 3: HeroAni.SetTrigger("Idle3"); break;
            case 4: HeroAni.SetTrigger("Idle4"); break;
        }
    }

    //攻击动画计时
    private void AttackAniTimer()
    {
        //处于攻击状态
        if (heroState == HeroState.attack)
        {
            attackTimer += Time.deltaTime;
            //MIN_ATTACK_TIME 与 MAX_ATTACK_TIME 为角色的有效攻击时间段
            if (attackTimer >= Constant.HeroAniTime.MIN_ATTACK_TIME)
            {
                //角色处于攻击状态（在Zombie中会检测，只有当角色处于攻击状态时才会收到伤害）
                HeroData.isAttack = true;
                if(attackTimer >= Constant.HeroAniTime.MAX_ATTACK_TIME)
                {
                    //角色攻击结束
                    attackTimer = 0f;
                    HeroData.isAttack = false;
                    heroState = HeroState.idle;//回归Idle状态
                }
            }
        }
    }

    //切换武器动画计时
    private void SwopAniTimer()
    {
        //处于武器切换状态
        if (heroState == HeroState.swop)
        {
            swopTimer += Time.deltaTime;
            if (swopTimer >= Constant.HeroAniTime.WEAPON_SWITCH_TIME)
            {
                //角色武器切换时间结束
                swopTimer = 0f;
                heroState = HeroState.idle;//回归Idle状态
            }
        }
    }

    //播放攻击动画
    private void PlayAttackAni()
    {
        //判断是否已经处于攻击状态，如果处于则不能重复攻击
        if (heroState != HeroState.attack)
        {
            idleTime = 0f;
            heroState = HeroState.attack;
            HeroAni.SetTrigger("Attack2");
        }
    }

    //播放Run动画
    private void PlayRunAni()
    {
        idleTime = 0f;
        HeroAni.SetBool("Run", true);
    }

    //停止Run动画，播放Idle动画
    private void StopRunAni()
    {
        HeroAni.SetBool("Run", false);

        //角色停滞五秒后就随机播放其他动作Idle
        idleTime += Time.deltaTime;
        if (idleTime >= 5f)
        {
            PlayIdleAni();
            idleTime = 0f;
        }
    }

    //播放角色受击动画
    private void PlayImpactAni()
    {
        HeroAni.SetTrigger("Impact");
    }

    //播放武器切换动画
    private void PlaySwitchWeapon()
    {
        idleTime = 0f;

        //判断是否处于武器切换中，处于则不能重复切换
        if (heroState != HeroState.swop)
        {
            heroState = HeroState.swop;
            HeroAni.SetTrigger("Sword");
        }
    }
}
