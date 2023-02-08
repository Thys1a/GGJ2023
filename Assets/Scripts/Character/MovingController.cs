using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UFramework;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Data;

/// <summary>
/// 物体移动控制器
/// </summary>
public class MovingController:MonoBehaviour
{
    const string m_PullCollide = "PullCollide";
    const string m_PushCollide = "PushCollide";

    public GameObject dialogPanel, tanpopo;
    public Text dialogContent, dialogName;

    public float movingSpeed,friction;
    public GameObject child;
    public CharacterAnimationController animationController;
    public bool isMoving=false,isPulling=false,isPushing=false, direction=true;
    private GameObject movingObject=null;
    public int success=0;
    public Sprite[] waitingBack;
    public GameObject noMaskBack,mask;
    public Dictionary<string, Vector2> des;
    public VideoController video;
    private TwistEffect twistEffect;

    public List<SpriteRenderer> items;

    private void Awake()
    {
        MessageCenter.Instance.Register(m_PullCollide, PullObjects);
        MessageCenter.Instance.Register(m_PushCollide, PushObjects);
    }
    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(m_PullCollide, PullObjects);
        MessageCenter.Instance.Remove(m_PushCollide, PushObjects);
    }

    

    private void Start()
    {
        twistEffect = GameObject.Find("TwistEffectLayer").GetComponent<TwistEffect>();
        twistEffect.gameObject.SetActive(false);
        video.gameObject.transform.localScale = Vector3.zero;
        if (animationController != null) animationController.Set(child.GetComponent<Animator>());
        des = new Dictionary<string, Vector2>();
        des.Add("1", new Vector2(12.55f,8.29f));
        des.Add("2", new Vector2(-14.23f,-3.84f));
        des.Add("3", new Vector2(-17.1f,-3.53f));
        des.Add("4", new Vector2(-5.07f, -2.02f));
        des.Add("5", new Vector2(-3.251f, -2.699f));

        dialogContent = dialogPanel.transform.Find("Content").gameObject.GetComponent<Text>();
        dialogName = dialogPanel.transform.Find("Name").gameObject.GetComponent<Text>();
        dialogPanel.SetActive(false);
        tanpopo.SetActive(false);
        Dialog(2);
    }
    private void FixedUpdate()
    {
        if (animationController == null) return;
        if (movingObject!=null) {
            if(Input.GetKey(KeyCode.Space)){
                if (movingObject.tag=="Item")
                {
                    if (!isPulling)
                    {
                        isPulling = true;
                        animationController.Pull();
                    }
                    PullItem(movingObject);
                }
                
            }
            
            else// (Input.GetKeyUp(KeyCode.Space))
            {
                print("space key up");
                //if (isPulling)
                //{

                //}
                isPulling = false;
                isMoving = false;

                itemDestinationCheck(movingObject);
                movingObject = null;
                if (this.transform.localScale.x > 0)
                {
                    if (!direction)
                    {
                        Turn();
                        direction = !direction;
                    }
                }
                else
                {
                    if (direction)
                    {
                        Turn();
                        direction = !direction;
                    }
                }
            }
            //if (Input.GetKeyUp(KeyCode.B))
            //{
            //    if (isPushing)
            //    {
            //        isPushing = false;
            //        isMoving = false;
            //        animationController.StopPushing();
            //        movingObject = null;
                    
            //    }
            //}
        }
        //else if (Input.GetKey(KeyCode.B))
        //{
        //    if (movingObject.tag == "Circle")
        //    {
        //        if (!isPulling)
        //        {
        //            isPulling = true;
        //            animationController.Push();
        //        }
        //        PushCircle(movingObject);
        //    }
        //}
        //if (Input.GetKeyUp(KeyCode.W)|| Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        //{
        //    if (isMoving)
        //    {
        //        animationController.StopWalking();
        //        isMoving = !isMoving;
        //    }
        //}

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0 || v != 0)
        {
            
            if (!isMoving)
            {
                if (!isPulling &&! isPushing) animationController.Walk();
                isMoving = !isMoving;
            }
            if (h > 0)
            {
                if (!direction)
                {
                    if (!isPulling)
                    {
                        Turn();
                        direction = !direction;
                    }
                        
                }
                else if (isPulling)
                {
                    Turn();
                    direction = !direction;
                }
            }
            else if(h<0){
                if (direction)
                {
                    if (!isPulling)
                    {
                        Turn();
                        direction = !direction;
                    }
                }
                else if (isPulling)
                {
                    Turn();
                    direction = !direction;
                }
            }
            Vector3 pos;
            pos = this.transform.position;
            pos.x += (movingSpeed * Time.fixedDeltaTime-friction) * h;
            pos.x = Mathf.Clamp(pos.x,-18f, 18f);
            pos.y += (movingSpeed * Time.fixedDeltaTime-friction)*v;
            pos.y = Mathf.Clamp(pos.y, -8f, 8f);
            this.transform.position = Vector3.MoveTowards(transform.position,pos, Time.deltaTime);
            
        }
        if (h==0&&v==0 )
        {
            if (isMoving)
            {
                animationController.StopWalking();
                isMoving = !isMoving;
            }
            
        }
    }
    int dialogIndex = 0;
    List<string[]> dialog = new List<string[]>();
    private void Dialog(int i)
    {
        
        Time.timeScale = 0;

        string csvpath = Application.streamingAssetsPath+"/" ;
        DataTable table = CSVUtil.OpenCSV(csvpath+ i + ".csv");
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
        dialogName.text = (dialog[dialogIndex][0] == "null") ? "" : dialog[dialogIndex][0];
        dialogContent.text = dialog[dialogIndex][1];
        dialogPanel.SetActive(true);
        if (dialogName.text == "たんぽぽ") tanpopo.SetActive(true);
        dialogIndex++;
    }
    private void DialogFinished()
    {
        MessageCenter.Instance.Remove("E", OnClickE);

        Time.timeScale = 1;

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
    private void itemDestinationCheck(GameObject movingObject)
    {
        Vector2 destination;
        des.TryGetValue(movingObject.name, out destination);
        if (destination == null) { 
            animationController.Fail();
            return;
        }
        if (Vector2.Distance(movingObject.transform.position, destination) < 0.5)
        {
            movingObject.transform.position = destination;
            movingObject.tag = "Untagged";
            animationController.Succeed();
            Complete();
        }
        else
        {
            animationController.Fail();
        }
    }

    private void Complete()
    {
        
        success += 1;
        switch (success)
        {
            case 1: GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().sprite = waitingBack[0]; break;
            case 2: StartCoroutine(TestEffect()); break;
            case 3: AudioController.instance.Play("出现听觉", isLoop: true); break;
            case 4:noMaskBack.transform.localScale = Vector3.one;
                foreach (var item in items)
                {
                    item.maskInteraction = SpriteMaskInteraction.None;
                }
                break;
            case 5:Destroy(mask);AudioController.instance.PlayOneShot("圈圈破碎"); SceneManager.Instance.PreLoadScene("Start"); EndAnimation(); break;
        }
    }
    public IEnumerator TestEffect()
    {
        
        twistEffect.speed = UnityEngine.Random.Range(0, 10) > 4 ? 300 : -300;
        twistEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        twistEffect.gameObject.SetActive(false);
    }
    private void EndAnimation()
    {
        this.transform.localScale = Vector3.zero;
        AudioController.instance.Stop("出现听觉");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.position = new Vector3(0, 0, camera.transform.position.z);
        Tween twe=camera.GetComponent<Camera>().DOOrthoSize((float)10.8, 4f);
        twe.OnComplete(EndVideo);
        //
    }

    private void EndVideo()
    {
        video.SetCallBack(ReturnToStart);
        video.gameObject.transform.localScale = Vector3.one;
        video.PlayVideo();
    }

    private void ReturnToStart()
    {
        SceneManager.Instance.LoadScene();
    }

    private void PullItem(GameObject movingObject)
    {
        var i = 0;
        float t=0,max=0.8f;

        if ((movingObject.transform.position.x - this.transform.position.x) > 0) i = 1;
        else if ((movingObject.transform.position.x - this.transform.position.x) < 0) i = -1;
        t = Mathf.Min(Mathf.Abs(movingObject.transform.position.x - this.transform.position.x), Mathf.Abs(max));
        var pos = new Vector2(transform.position.x + i * t, (float)(transform.position.y + 0.5f));
        movingObject.transform.position=Vector2.MoveTowards(movingObject.transform.position, pos, Time.deltaTime);
        
    }

    public void Turn()
    {
        //转方向
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }


    private void PushObjects(object arg0)
    {
        movingObject = (GameObject)arg0;
    }

    private void PullObjects(object arg0)
    {
        movingObject = (GameObject)arg0;
    }
}

