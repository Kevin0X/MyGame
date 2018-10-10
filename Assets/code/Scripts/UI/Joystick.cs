using Assets.code.Scripts.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler{

    [SerializeField]
    private float maxRadius = 100;

    private HeroData HeroData;

    private Transform JoystickControl;
    private Button BtnAttack;
    private Button BtnSwitchWeapon;

    private Vector2 moveBackPos = new Vector2(0, 0);

    public static float horizontal    = 0;
    public static float vertical      = 0;

    private void Start()
    {
        HeroData = GameManager.Instance.GetHeroDataIns();
        
        Init();
    }

    private void Init()
    {
        Transform self = transform;

        JoystickControl     = self.Find("Joystick/ImgC_JoystickBg/ImgC_JoystickControl");
        BtnAttack           = self.Find("BtnAttack").GetComponent<Button>();
        BtnSwitchWeapon     = self.Find("BtnSwitchWeapon").GetComponent<Button>();

        BtnAttack.onClick.AddListener(() => { HandlerBtnAttack(); });
        BtnSwitchWeapon.onClick.AddListener(() => { HandlerBtnSwitchWeapon(); });

        moveBackPos = JoystickControl.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandlerBtnAttack();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
        if (Input.GetKeyDown(KeyCode.S))
        {

        }
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
        if (Input.GetKeyDown(KeyCode.D))
        {

        }
    }

    private void HandlerBtnAttack()
    {
        //触发攻击按钮按下事件
        Messenger.Broadcast(Constant.EventName.HERO_ATTACK);
    }

    private void HandlerBtnSwitchWeapon()
    {
        //触发切换武器按钮按下事件
        Messenger.Broadcast(Constant.EventName.SWITCH_WEAPON);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //获取摇杆位置与初始位置之间的向量
        Vector2 oppsitionVec = eventData.position - moveBackPos;
        //获取向量的长度
        float distance = Vector3.Magnitude(oppsitionVec);
        //最小值与最大值之间取半径
        float radius = Mathf.Clamp(distance, 0, maxRadius);
        //限制半径长度
        JoystickControl.position = moveBackPos + oppsitionVec.normalized * radius;

        //拖拽时更新horizontal 与 vertical
        horizontal = JoystickControl.localPosition.x;
        vertical = JoystickControl.localPosition.y;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        JoystickControl.localPosition = Vector2.zero;

        //拖拽结束时还原位置
        horizontal = JoystickControl.localPosition.x;
        vertical = JoystickControl.localPosition.y;
    }

    private void OnDestroy()
    {
        JoystickControl = null;
        BtnAttack = null;
        HeroData = null;
    }
}
