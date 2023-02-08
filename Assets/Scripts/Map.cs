using DG.Tweening;
using System.Collections.Generic;
using System.Data;
using UFramework;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class Map : MonoBehaviour
{
    
    MapConfig mapConfig;
    Dictionary<Ball,GameObject> emitedBalls;
    List<Ball> emitedBallsList;
    List<Sprite> waitingBalls;
    public GameObject dialogPanel,tanpopo;
    public Text dialogContent,dialogName;

    ObjectPool<GameObject> cacheGameObject;
    const string ballsPath = "Balls/";
    const string mapconfigPath = "MapConfig/";

    public string mapName;
    public Vector2 core=Vector2.zero;
    public float velocity = 1,delTime;
    public float left, right, up, down;
    public int initNum,pressureValue=0,value=0;
    private GameObject ball;
    public int note = 0,pressure=0;
    public GameObject blue, red, dynamicBack;
    public float[][] bounds=new float[4][];
    public Animator clockAnimator;

    public CharacterAnimationController character;

    //全局广播事件
    const string m_MapLoaded = "MapLoaded";
    const string m_MapUnLoaded = "MapUnLoaded";

    const string m_BallCollide = "Collide";

    #region life
    // Start is called before the first frame update
    void Start()
    {
        InitializeMap();// 初始化地图
        if (mapConfig == null) {
            Debug.LogError("未成功读取地图");
            return;
        }
        InitializeBalls();
        if (waitingBalls == null)
        {
            Debug.Log("等待弹珠数为0");
        }
        core = mapConfig.elementsPos[0];
        MessageCenter.Instance.Send(m_MapLoaded, mapConfig.elementsPos);
        AudioController.instance.PlayBGM("第一关bgm");
        clockAnimator.gameObject.transform.localScale = Vector2.zero;

       
        dialogContent = dialogPanel.transform.Find("Content").gameObject.GetComponent<Text>();
        dialogName = dialogPanel.transform.Find("Name").gameObject.GetComponent<Text>();
        dialogPanel.SetActive(false);
        tanpopo.SetActive(false);

        Dialog(1);
        
        for (int i = 0; i < initNum; i++)
        {
            PrepareBall();
        }
        if(delTime!=0) InvokeRepeating("PrepareBall", delTime, delTime);
    }
    private void Awake()
    {
        MessageCenter.Instance.Register(m_BallCollide, CollideEvent);
        MessageCenter.Instance.Register("L", LaunchNote);
    }

    private void LaunchNote(object o)
    {
        if (note == 1)
        {
            AudioController.instance.PlayOneShot("L键");
            value += 1;
            pressureValue -= 1;
            ValueCheck();
            for (int i = 0; i < 5; i++)
            {
                if(emitedBallsList.Count>0) DeleteBall(emitedBallsList[emitedBallsList.Count - 1]);
            }

            note = 0;
            character.LaunchNote();
            character.StopGettingNote();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        MessageCenter.Instance.Send(m_MapUnLoaded,null);
        MessageCenter.Instance.Remove(m_BallCollide, CollideEvent);
        MessageCenter.Instance.Remove("L", LaunchNote);
        cacheGameObject.Dispose();
    }
    
    /// <summary>
    /// 触碰到环
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)//触发器需要在Collider组件中勾选Trigger属性
    {
        CollideEvent(collision);

    }
    private void CollideEvent(Collider2D collision)
    {
        foreach (var item in emitedBalls)
        {
            if (item.Value == collision.gameObject)
            {
                pressureValue += 1;
                PressureCheck();
                DeleteBall(item.Key);
                return;
            }

        }
    }

    private void PressureCheck()
    {
        if (pressureValue >=30)
        {
            pressure += 1;
            pressureValue = 0;
            AudioController.instance.PlayOneShot("红色波浪");
            red.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, (float)(1 * 0.1 * pressure));
            blue.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, (float)(1 -1* 0.1 * pressure));
            dynamicBack.transform.DOMoveY((float)(-10.8 + 1.08 * pressure), 1f);
        }
        if (pressure == 10)
        {
            //压力值到达顶端
            //AudioController.instance.Stop("STEP");
            pressure = 0;
            pressureValue = 0;
            CancelInvoke("PrepareBall");
            AudioController.instance.Stop("第一关bgm");
            SceneManager.Instance.PreLoadScene("2");
            clockAnimator.gameObject.transform.localScale = Vector2.one;
            clockAnimator.Play("Clock", 0, 0);

        }
    }

    private void CollideEvent(object collision)
    {
        foreach (var item in emitedBalls)
        {
            if (item.Value ==( (Collider2D)collision).gameObject)
            {
                value += 1;
                pressureValue -= 1;
                ValueCheck();
                AudioController.instance.PlayOneShot("搓破飞来气泡");
                DeleteBall(item.Key);
                return;
            }

        }
    }

    private void ValueCheck()
    {
        if (value >= 10)
        {
            value = 0;
            note = 1;
            character.GetNote();
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    foreach (var item in emitedBalls)
    //    {
    //        if (item.Value == collision.gameObject) DeleteBall(item.Key);

    //    }
    //}
    #endregion
    int dialogIndex = 0;
    List<string[]> dialog = new List<string[]>();
    private void Dialog(int i)
    {
        AudioController.instance.Stop("STEP");
        Time.timeScale = 0;
        
        string csvpath = Application.streamingAssetsPath+"/";
        DataTable table = CSVUtil.OpenCSV(csvpath + i + ".csv");
        for (int j = 0; j < table.Rows.Count; j++)
        {

            dialog.Add(new string[] { (string)table.Rows[j][0], (string)table.Rows[j][1] });
        }
        //读取
        MessageCenter.Instance.Register("E", OnClickE);
        dialogIndex = 0;
        PlayDialog();
    }
    private void PlayDialog()
    {
        dialogName.text = (dialog[dialogIndex][0] == "null") ? "": dialog[dialogIndex][0];
        dialogContent.text= dialog[dialogIndex][1];
        dialogPanel.SetActive(true);
        if (dialogName.text == "たんぽぽ") tanpopo.SetActive(true);
        dialogIndex++;
    }
    private void DialogFinished()
    {
        MessageCenter.Instance.Remove("E", OnClickE);
        
        Time.timeScale = 1;
        AudioController.instance.Play("STEP");
    }
    private void OnClickE(object o)
    {
        dialogPanel.SetActive(false);
        tanpopo.SetActive(false);
        if (dialogIndex < dialog.Count)
        {
            PlayDialog();
        }
        else DialogFinished();
    }
    #region private methods
    /// <summary>
    /// 读取地图配置
    /// </summary>
    private void InitializeMap()
    {
        
        string path = mapconfigPath + mapName;
        mapConfig = Resources.Load(path, typeof(MapConfig)) as MapConfig;

        GameObject background = GameObject.FindGameObjectWithTag("Background");
        if (background != null)
        {
            left = background.GetComponent<SpriteRenderer>().bounds.min.x;
            down = background.GetComponent<SpriteRenderer>().bounds.min.y;
            right = background.GetComponent<SpriteRenderer>().bounds.max.x;
            up = background.GetComponent<SpriteRenderer>().bounds.max.y;
        }
        bounds[0] = new float[] { left - 5, right, up + 5, up };
        bounds[1] = new float[] { right, right + 5, up + 5,down };
        bounds[2] = new float[] { left, right + 5, down, down - 5 };
        bounds[3] = new float[] { left - 5, left, down, down - 5 };
        // 初始化已发射弹珠列表
        emitedBalls = new Dictionary<Ball, GameObject>();
        emitedBallsList = new List<Ball>();
        //初始化等候弹珠序列
        waitingBalls = new List<Sprite>();
        //初始化对象池
        cacheGameObject = new ObjectPool<GameObject>(CreateBall, OnGetBall, OnReleaseBall, OnDestroyBall, true, 10, 20);
    }

    private void InitializeBalls()
    {
        ball = Resources.Load(ResourceManager.Instance.GetGlobalconfig.spritePath + ballsPath + "Ball") as GameObject;
        ball.transform.localScale = Vector3.zero;
        waitingBalls = (Resources.Load(ResourceManager.Instance.GetGlobalconfig.spritePath + ballsPath + "SpritesAssest") as SpritesListAssest).sprites;
    }
    #endregion

    #region Pool
    private void OnDestroyBall(GameObject obj)
    {
        Destroy(obj);
    }

    private void OnReleaseBall(GameObject obj)
    {
        obj.transform.localScale = Vector3.zero;
    }

    private void OnGetBall(GameObject obj)
    {
        obj.transform.localScale = Vector3.one;
    }

    private GameObject CreateBall()
    {
        GameObject obj = Instantiate(ball);
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    #endregion

    #region 游戏界面元素控制
    /// <summary>
    /// 弹珠准备
    /// </summary>
    private void PrepareBall()//对于队列第一个弹珠：随机显示在场景中
    {
        if (waitingBalls.Count <= 0)
        {
            CancelInvoke("PrepareBall");
            return;
        }

        Sprite sp = waitingBalls[Random.Range(0, waitingBalls.Count - 1)];
        Ball ball = ScriptableObject.CreateInstance<Ball>();

        GameObject ballObj = CreateBall();//cacheGameObject.Get();
        ballObj.SetActive(true);
        //生成随机坐标
        float[] bound = bounds[Random.Range(0, 3)];
        float x = Random.Range(bound[0], bound[1]);
        float y = Random.Range(bound[3], bound[2]);
        ballObj.transform.position = new Vector2(x, y);
        print(ballObj.transform.position);
        SpriteRenderer spr = ballObj.GetComponent<SpriteRenderer>();
        //try { spr.sprite = Sprite.Create(texture, spr.sprite.textureRect, new Vector2(0.5f, 0.5f)); }
        //catch
        //{
        //    spr.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //}
        spr.sprite = sp;
        //显示处理
        emitedBalls.Add(ball, ballObj);
        emitedBallsList.Add(ball);
        EmitBall(ball);
    }
    /// <summary>
    /// 朝圆心按照速度v发射弹珠
    /// </summary>
    /// <param name="ball"></param>
    private void EmitBall(Ball ball)
    {
        Transform ballTransform = emitedBalls[ball].transform;
        float t=0,l=0;
        l = Vector2.Distance(core, ballTransform.position);
        t = l / velocity;
        ballTransform.DOMove(core, t);
    }

    private void EmitNote(object obj)
    {
        if (note != 1) return;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        //noteObj.transform.position=player.transform.position
        //noteObj.transform.rotation=player.transform.rotation
        //l = Vector2.Distance(core, ballTransform.position);
        //t = l / velocity;
        //ballTransform.DOMove(core, t);
    }

    /// <summary>
    /// 删除弹珠
    /// </summary>
    /// <param name="ball"></param>
    private void DeleteBall(Ball ball)
    {

        //cacheGameObject.Release(emitedBalls[ball]);
        
        Destroy(emitedBalls[ball]);
        emitedBalls.Remove(ball);
        emitedBallsList.Remove(ball);
    }
    #endregion

    //#region 弹珠队列控制
    ///// <summary>
    ///// 插入弹珠
    ///// </summary>
    ///// <param name="pos">碰撞位置</param>
    //private void InsertBall(Vector2 pos,Ball ball)//传入位置 本体：插入到空位→往前推进
    //{
    //    //插入到最近的空位

    //    //往前移动
    //}
    ///// <summary>
    ///// 插入弹珠
    ///// </summary>
    ///// <param name="pre"></param>碰撞到的球
    //private void InsertBall(Ball pre,Ball ball) //传入前一个球 本体：插入到前一个球的后面
    //{
    //    //对后面的弹珠进行移动

    //    //插入
    //}
    ///// <summary>
    ///// 对当前弹珠前后位检测
    ///// </summary>
    //private bool Check(Ball ball)
    //{
    //    int index= emitedBalls.IndexOf(ball);
    //    int num = 0;
    //    //向前检测：2/?
    //    if (index > 0)
    //    {
    //        if(emitedBalls[index - 1].GetType().Equals(ball.GetType()))
    //        {
    //            num += 1;
    //            if (index > 1)
    //            {
    //                if(emitedBalls[index - 2].GetType().Equals(ball.GetType()))
    //                {
    //                    num += 1;
    //                }
    //            }
    //        }
    //    }
    //    if (num >= 2) return true;
    //    //向后检测：2

    //    return false;
    //}
    ///// <summary>
    ///// 消除弹珠队列
    ///// </summary>
    //private void RemoveBalls(int index)
    //{
    //    //往前/后探测

    //    //cacheAudioEntity.Release(audioEntity);
    //}
    //#endregion
}
