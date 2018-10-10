using Assets.code.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private ScenesManager ScenesManager;

    //存放所有UI预制
    private Dictionary<string, GameObject> uiPrefabs = new Dictionary<string, GameObject>();
    //存放已经生成的UI面板
    private Dictionary<int, GameObject> desPool = new Dictionary<int, GameObject>();

    private Transform MainCanvas;
    private Transform ButtonCanvas;
    private Transform ModeldCanvas;

    private GameObject PlayerModel;
    private GameObject UIRoot;

    private int pnlId = 0;

    void Awake()
    {
        UIRoot = Instantiate(Resources.Load<GameObject>("Prefabs/ui/ui_root_ex"));
    }

    private void Start()
    {
        ScenesManager = GameManager.Instance.GetScenesMgrIns();

        DontDestroyOnLoad(UIRoot);
        Init();
    }

    //Load所有UI面板预制，并添加到字典进行管理
    private void Init()
    {
        PlayerModel = Resources.Load("Prefabs/char/PlayerUI") as GameObject;

        uiPrefabs.Add("Joystick",       Resources.Load<GameObject>("Prefabs/ui/joystick/PnlJoystick"));
        uiPrefabs.Add("Login",          Resources.Load<GameObject>("Prefabs/ui/login/PnlLogin"));
        uiPrefabs.Add("HeroSelect",     Resources.Load<GameObject>("Prefabs/ui/Player/PnlPlayerSelect"));
        uiPrefabs.Add("Fight",          Resources.Load<GameObject>("Prefabs/ui/Fight/PnlFight"));
        uiPrefabs.Add("ZombieLifebar",  Resources.Load<GameObject>("Prefabs/ui/Zombie/ZombieLifebar"));
        uiPrefabs.Add("GameOver",       Resources.Load<GameObject>("Prefabs/ui/GameOver/PnlGameOver"));

        MainCanvas = UIRoot.transform.Find("mainCanvas");
        ButtonCanvas = UIRoot.transform.Find("bottomCanvas");
        ModeldCanvas = UIRoot.transform.Find("3d_Canvas/Camera");
    }

    //加载角色模型到3D摄像机
    public GameObject Load3dModel()
    {
        GameObject player = Instantiate(PlayerModel, ModeldCanvas);
        player.transform.localPosition = new Vector3(-0.3f, -1.3f, 2.5f);
        return player;
    }

    //加载UI面板到主画布
    public int LoadMainCanvas(string uiName)
    {
        GameObject go = Instantiate(uiPrefabs[uiName], MainCanvas);
        desPool.Add(pnlId, go);
        return pnlId++;
    }

    //加载UI面板到底层画布
    public int LoadBottomCanvas(string uiName)
    {
        GameObject go = Instantiate(uiPrefabs[uiName], ButtonCanvas);
        desPool.Add(pnlId, go);
        return pnlId++;
    }

    //返回ID值对应的UI面板
    public GameObject GetUIObject(int id)
    {
        return desPool[id];
    }

    //删除所有UI面板
    public void DestroyAllPnl()
    {
        foreach(GameObject go in desPool.Values)
        {
            Destroy(go);
        }
        desPool.Clear();
    }

    //删除对应ID的UI面板
    public void DestroyPnl(int id)
    {
        if(desPool.ContainsKey(id))
        {
            Destroy(desPool[id]);
            desPool.Remove(id);
        }
    }

    private void OnDestroy()
    {
        uiPrefabs.Clear();
        desPool.Clear();
        PlayerModel = null;
        MainCanvas = null;
        ButtonCanvas = null;
    }
}
