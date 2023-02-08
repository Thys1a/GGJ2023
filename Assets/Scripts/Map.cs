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

    //ȫ�ֹ㲥�¼�
    const string m_MapLoaded = "MapLoaded";
    const string m_MapUnLoaded = "MapUnLoaded";

    const string m_BallCollide = "Collide";

    #region life
    // Start is called before the first frame update
    void Start()
    {
        InitializeMap();// ��ʼ����ͼ
        if (mapConfig == null) {
            Debug.LogError("δ�ɹ���ȡ��ͼ");
            return;
        }
        InitializeBalls();
        if (waitingBalls == null)
        {
            Debug.Log("�ȴ�������Ϊ0");
        }
        core = mapConfig.elementsPos[0];
        MessageCenter.Instance.Send(m_MapLoaded, mapConfig.elementsPos);
        AudioController.instance.PlayBGM("��һ��bgm");
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
            AudioController.instance.PlayOneShot("L��");
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
    /// ��������
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)//��������Ҫ��Collider����й�ѡTrigger����
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
            AudioController.instance.PlayOneShot("��ɫ����");
            red.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, (float)(1 * 0.1 * pressure));
            blue.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, (float)(1 -1* 0.1 * pressure));
            dynamicBack.transform.DOMoveY((float)(-10.8 + 1.08 * pressure), 1f);
        }
        if (pressure == 10)
        {
            //ѹ��ֵ���ﶥ��
            //AudioController.instance.Stop("STEP");
            pressure = 0;
            pressureValue = 0;
            CancelInvoke("PrepareBall");
            AudioController.instance.Stop("��һ��bgm");
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
                AudioController.instance.PlayOneShot("���Ʒ�������");
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
        //��ȡ
        MessageCenter.Instance.Register("E", OnClickE);
        dialogIndex = 0;
        PlayDialog();
    }
    private void PlayDialog()
    {
        dialogName.text = (dialog[dialogIndex][0] == "null") ? "": dialog[dialogIndex][0];
        dialogContent.text= dialog[dialogIndex][1];
        dialogPanel.SetActive(true);
        if (dialogName.text == "����ݤ�") tanpopo.SetActive(true);
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
    /// ��ȡ��ͼ����
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
        // ��ʼ���ѷ��䵯���б�
        emitedBalls = new Dictionary<Ball, GameObject>();
        emitedBallsList = new List<Ball>();
        //��ʼ���Ⱥ�������
        waitingBalls = new List<Sprite>();
        //��ʼ�������
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

    #region ��Ϸ����Ԫ�ؿ���
    /// <summary>
    /// ����׼��
    /// </summary>
    private void PrepareBall()//���ڶ��е�һ�����飺�����ʾ�ڳ�����
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
        //�����������
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
        //��ʾ����
        emitedBalls.Add(ball, ballObj);
        emitedBallsList.Add(ball);
        EmitBall(ball);
    }
    /// <summary>
    /// ��Բ�İ����ٶ�v���䵯��
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
    /// ɾ������
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

    //#region ������п���
    ///// <summary>
    ///// ���뵯��
    ///// </summary>
    ///// <param name="pos">��ײλ��</param>
    //private void InsertBall(Vector2 pos,Ball ball)//����λ�� ���壺���뵽��λ����ǰ�ƽ�
    //{
    //    //���뵽����Ŀ�λ

    //    //��ǰ�ƶ�
    //}
    ///// <summary>
    ///// ���뵯��
    ///// </summary>
    ///// <param name="pre"></param>��ײ������
    //private void InsertBall(Ball pre,Ball ball) //����ǰһ���� ���壺���뵽ǰһ����ĺ���
    //{
    //    //�Ժ���ĵ�������ƶ�

    //    //����
    //}
    ///// <summary>
    ///// �Ե�ǰ����ǰ��λ���
    ///// </summary>
    //private bool Check(Ball ball)
    //{
    //    int index= emitedBalls.IndexOf(ball);
    //    int num = 0;
    //    //��ǰ��⣺2/?
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
    //    //����⣺2

    //    return false;
    //}
    ///// <summary>
    ///// �����������
    ///// </summary>
    //private void RemoveBalls(int index)
    //{
    //    //��ǰ/��̽��

    //    //cacheAudioEntity.Release(audioEntity);
    //}
    //#endregion
}
