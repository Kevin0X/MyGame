using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

    public UIManager UIManager { get; private set; }
    public ScenesManager ScenesManager { get; private set; }
    public HeroData HeroData { get; private set; }

    private void Awake()
	{
		if (Instance == null)
		{
            Instance = this;
		}
        
        //添加UI管理
        UIManager = gameObject.AddComponent<UIManager>();
        //添加场景管理
        ScenesManager = gameObject.AddComponent<ScenesManager>();
        //实例化玩家数据
        HeroData = new HeroData();
    }

    private void Start()
	{
        DontDestroyOnLoad(this);
    }

    //获取UI管理实例
    public UIManager GetUIMgrIns()
    {
        Assert.IsNotNull(UIManager);
        return UIManager;
    }

    //获取场景管理实例
    public ScenesManager GetScenesMgrIns()
    {
        Assert.IsNotNull(ScenesManager);
        return ScenesManager;
    }

    //获取玩家数据实例
    public HeroData GetHeroDataIns()
    {
        Assert.IsNotNull(HeroData);
        return HeroData;
    }

    private void OnDestroy()
    {

    }
}
