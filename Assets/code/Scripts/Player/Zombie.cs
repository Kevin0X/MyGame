using Assets.code.Scripts.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    enum ZombieState
    {
        hit = 1,        //受击状态
        attack = 2,     //攻击状态
        fly = 3,        //击飞状态
        death = 4,      //死亡状态
        run = 5,        //移动状态
        nil = 6         //空状态
    }

    private UIManager UIManager;
    private HeroData HeroData;

    private NavMeshAgent NavMeshAgent;
    private Transform LifebarObj;
    private new Rigidbody Rigidbody;
    private Transform Hero;
    private Slider Lifebar;
    private Animator Ani;

    private ZombieState state = ZombieState.run;

    private int id = 0;

    private float attackTimer = 0f;
    private float deathTimer = 0f;
    private float hitTimer = 0f;
    private float flyTimer = 0f;

    private float maxHp;

    [SerializeField]
    private float hp;
    [SerializeField]
    private Transform headPos;  //头顶位置
    [SerializeField]
    private float attackDir;    //攻击距离

    private void Start()
    {
        UIManager = GameManager.Instance.GetUIMgrIns();
        HeroData = GameManager.Instance.GetHeroDataIns();

        NavMeshAgent = GetComponent<NavMeshAgent>();
        Rigidbody = GetComponent<Rigidbody>();
        Ani = GetComponent<Animator>();

        Hero = GameObject.FindGameObjectWithTag("Hero").transform;

        maxHp = hp;
        
        LoadZombieLifebar();

        NavMeshAgent.enabled = false;
        NavMeshAgent.enabled = true;
    }

    private void LateUpdate()
    {
        Move();
        UpdateLifebar();
    }

    private void FixedUpdate()
    {
        AttackWhetherMeet();
        AttackAniTimer();
        HitAniTimer();
        FlyAniTimer();
        DeathAniTimer();
    }

    //创建头部血条显示
    void LoadZombieLifebar()
    {
        id = UIManager.LoadBottomCanvas("ZombieLifebar");
        LifebarObj = UIManager.GetUIObject(id).transform;
        Lifebar = LifebarObj.Find("Nego_ZombieLifebar").GetComponent<Slider>();
    }

    //Zombie寻路
    private void Move()
    {
        if (state == ZombieState.run)
        {
            Ani.SetInteger("State", 1);
            if (NavMeshAgent.enabled == true)
            {
                NavMeshAgent.SetDestination(Hero.position);
            }
        }
    }

    //Zombie攻击方法
    private void Attack()
    {
        //播放攻击动画
        Ani.SetTrigger("Attack");
        //设置寻路暂停
        NavMeshAgent.isStopped = true;
        //设置为攻击状态
        state = ZombieState.attack;
    }

    //检测Zombie攻击条件是否满足
    private void AttackWhetherMeet()
    {
        //状态是否为Run
        if (state == ZombieState.run)
        {
            //检测是否达到攻击距离
            float dis = Vector3.Distance(transform.position, Hero.position);
            if (dis < attackDir)
            {
                transform.LookAt(Hero.position);
                Attack();
            }
        }
    }

    //更新头部血条位置
    private void UpdateLifebar()
    {
        //将Zombie头上的位置转换到屏幕坐标
        Vector3 zombiePos = new Vector3(headPos.position.x, headPos.position.y + 0.5f, headPos.position.z);
        Vector2 lifebarPos = Camera.main.WorldToScreenPoint(zombiePos);

        if(LifebarObj != null)
        {
            LifebarObj.position = lifebarPos;
            Lifebar.value = hp / maxHp;
        }
    }

    //击飞方法
    private IEnumerator OnDiaup()
    {
        gameObject.layer = 11;  //设置层为击飞层 11
        yield return new WaitForSeconds(0.0f);  //短暂延迟后执行击飞

        NavMeshAgent.enabled = false;   //取消激活寻路

        //计算击飞效果
        //Vector3 forward = -transform.forward;
        //Quaternion rotetion = Quaternion.AngleAxis(60f, transform.right);
        //Vector3 des = (rotetion * forward).normalized;
        //rigidbody.AddForce(400f * des);
        //rigidbody.AddForce(transform.up * 400f);

        //使用Unity自带的爆炸击飞效果
        Rigidbody.AddExplosionForce(450f, Hero.position, 0.0f);
    }

    //检测受击是否满足
    private void HitWhetherMeet(string tag)
    {
        //在受击状态下不能重复受击，击飞状态下不能受击，死亡状态下不能受击
        if (state != ZombieState.hit && state != ZombieState.fly && state != ZombieState.death)
        {
            switch (tag)
            {
                case "Axe": //攻击武器为斧子
                    {
                        state = ZombieState.hit;    //切换状态为受击
                        hp -= 20;   //获取伤害值
                        if (hp <= 0)    //生命值等于零
                        {
                            state = ZombieState.death;  //切换状态为死亡
                            Ani.SetTrigger("Death");    //播放死亡动画
                        }
                        else
                        {
                            Ani.SetTrigger("Hit");  //播放受击动画
                        }
                    }
                    break;
                case "EpicClub":    //攻击武器为棒子
                    {
                        state = ZombieState.fly;    //切换状态为击飞
                        hp -= 10;//获取伤害值
                        if (hp <= 0)
                        {
                            state = ZombieState.death;  //切换状态为死亡
                            Ani.SetTrigger("Death");    //播放死亡动画
                        }
                        else
                        {
                            Ani.SetTrigger("Floating"); //播放击飞动画
                            StartCoroutine(OnDiaup());  //调用击飞方法
                        }
                    }
                    break;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Axe" || other.gameObject.tag == "EpicClub")
        {
            //检测角色是否发起攻击
            if (HeroData.isAttack)
            {
                HitWhetherMeet(other.gameObject.tag);
            }
        }
    }

    //检测角色受击是否满足
    private void HeroHitWhetherMeet()
    {
        //检测角色是否还在攻击距离
        float dis = Vector3.Distance(transform.position, Hero.position);
        if (dis < attackDir)
        {
            Vector3 targetDir = Hero.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(targetDir, forward);
            if (angle < 15.0f)
            {
                //调用角色受击事件
                Messenger.Broadcast<float>(Constant.EventName.HERO_HPUPDATE, 0.1f);
            }
        }
    }

    //攻击动画计时
    private void AttackAniTimer()
    {
        //当处于攻击状态
        if (state == ZombieState.attack)
        {
            attackTimer += Time.deltaTime;
            //Zombie设置一段有效的攻击时间，只有在此时间段内角色才能收到伤害
            if (attackTimer >= Constant.ZombieAniTime.MIN_HARM_TIME && attackTimer <= Constant.ZombieAniTime.MAX_HARM_TIME)
            {
                HeroHitWhetherMeet();
            }
            //大于最大攻击时间，将状态设置回Run，并将启动寻路
            if (attackTimer >= Constant.ZombieAniTime.ATTACK_TIME)
            {
                attackTimer = 0;
                state = ZombieState.run;
                NavMeshAgent.isStopped = false;
            }
        }
    }

    //受击动画计时
    private void HitAniTimer()
    {
        //当处于受击状态
        if(state == ZombieState.hit)
        {
            hitTimer += Time.deltaTime;
            //大于最大受击事件，将状态设置为Run，并将启动寻路
            if(hitTimer >= Constant.ZombieAniTime.HIT_TIME)
            {
                hitTimer = 0;
                state = ZombieState.run;
                NavMeshAgent.isStopped = false;
            }
        }
    }

    //击飞动画计时
    private void FlyAniTimer()
    {
        //当处于击飞状态
        if (state == ZombieState.fly)
        {
            flyTimer += Time.deltaTime;
            //大于最大击飞时间，将状态设置为Run
            if (flyTimer >= Constant.ZombieAniTime.FLY_TIME)
            {
                state = ZombieState.run;
                //当Zombie世界坐标的Y值低于-14证明已经掉落到水面下
                if (transform.position.y < -14f)
                {
                    Destroy(this);//销毁自身
                    //transform.position = new Vector3(transform.position.x, 8.0f, transform.position.z);
                }
                else if (transform.position.y < -5f)    //只有当高度低于-5时，才会启动寻路
                {
                    flyTimer = 0;
                    gameObject.layer = 10;  //设置层为Zombie层 10
                    NavMeshAgent.enabled = true;
                }
            }
        }
    }

    //死亡动画计时
    private void DeathAniTimer()
    {
        //当处于死亡状态时
        if (state == ZombieState.death)
        {
            deathTimer += Time.deltaTime;
            //大于最大死亡时间
            if (deathTimer >= Constant.ZombieAniTime.DEATH_TIME)
            {
                deathTimer = 0;
                gameObject.SetActive(false);
                Lifebar.gameObject.SetActive(false);
                Destroy(this);
            }
        }
    }

    private void OnDestroy()
    {
        UIManager.DestroyPnl(id);
        UIManager = null;
        NavMeshAgent = null;
        Ani = null;
        Hero = null;
        Lifebar = null;
        headPos = null;
        LifebarObj = null;
    }
}
