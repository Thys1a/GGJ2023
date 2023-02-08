using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UFramework;
using UnityEngine;
/// <summary>
/// ��ɫ����������ȡ��ͼ
/// 1.�ƶ�����
/// 2.��������
/// 3.������
/// </summary>
public class MainCharacter : MonoBehaviour
{
    List<Vector2> routine;
    int positionIndex=0;
    int direction = 1;
    public float moveSpeed, delJumpTime;

    private Sequence s;

    //���ܹ㲥�¼�
    const string m_MapLoaded = "MapLoaded";
    const string m_MapUnLoaded = "MapUnLoaded";
    const string m_UpDown = "Q";
    const string m_LeftRight = "S";
    const string m_Jump = "M";

    public CharacterAnimationController animationController;
    public GameObject child;
    public bool isMoving=false,isJumping = false, isFront=true;
    public int front2back, back2front;
    public float height = 0, ground = 0;
    private Action callback;
    //private float angle=0;
    private void Awake()
    {
        MessageCenter.Instance.Register(m_MapLoaded, SetRoutine);
        MessageCenter.Instance.Register(m_MapUnLoaded, DelRoutine);
        MessageCenter.Instance.Register(m_UpDown, TurnOver);
        MessageCenter.Instance.Register(m_LeftRight, DirectionChange);
        MessageCenter.Instance.Register(m_Jump, Jump);
    }
    // Start is called before the first frame update
    void Start()
    {
        //routine = new List<Vector2>();
        
        StartMoving();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (routine == null) return;
        
        if (!isMoving&&!isJumping)
        {
            StartMoving();
        }
        //else MoveToNextDestination();
    }

    private void OnDestroy()
    {
        StopMoving();
        MessageCenter.Instance.Remove(m_MapLoaded, SetRoutine);
        MessageCenter.Instance.Remove(m_MapUnLoaded, DelRoutine);
        MessageCenter.Instance.Remove(m_UpDown, TurnOver);
        MessageCenter.Instance.Remove(m_LeftRight, DirectionChange);
        MessageCenter.Instance.Remove(m_Jump, Jump);
    }
    #region Move
    /// <summary>
    /// �����ƶ���������������
    /// </summary>
    public void  StartMoving()
    {
        if (routine == null) return;
        
        if (animationController == null)
        {
            Debug.Log("δ�ҵ�����������");
            return;
        }
        animationController.Set(child.GetComponent<Animator>());
        animationController.Walk();
        
        ContinueMoving();
        
    }
    public void ContinueMoving()
    {
        
        isMoving = true;
        callback += MoveToNextDestination;
        MoveToNextDestination();
    }
    /// <summary>
    /// ֹͣ�ƶ�
    /// </summary>
    public void StopMoving() 
    {
        isMoving = false;
        callback -= MoveToNextDestination;
        AudioController.instance.Stop("STEP",true);
    }
    /// <summary>
    /// �ƶ���positionIndex��ָ���Ŀ�ĵ�
    /// </summary>
    private void MoveToNextDestination()
    {
        if (!isMoving) return;
        //if (isJumping) return;
        if (positionIndex == front2back)
        {
            isFront = false;
            FaceingDirectionChange();
        }
        else if (positionIndex == back2front)
        {
            isFront = true;
            FaceingDirectionChange();
        }
        //if (positionIndex >= routine.Count) positionIndex = 0;//�����յ�
        Vector2 vertical;
        if (isFront) vertical = GetVerticalVectorFrom(this.transform.position, routine[positionIndex]);
        else vertical = GetVerticalVectorFrom(routine[positionIndex],this.transform.position);
        //print(routine[positionIndex] + "��" + this.transform.position);
        
        float angle = Vector2.Angle(Vector2.up, vertical);
        if (vertical.x * vertical.y > 0) angle = -angle;
        //float angle = Vector2.Angle(routine[(positionIndex - 1 + routine.Count) % routine.Count],vertical);
        if (angle>90|| angle<-90||positionIndex == front2back|| positionIndex == front2back) MoveLoop(routine[positionIndex], 0);
        else MoveLoop(routine[positionIndex], angle);
        //print("rotate "+angle + "��" + this.transform.rotation);

        positionIndex += 1*direction;
        positionIndex += routine.Count;
        positionIndex %=routine.Count;

    }
    public void Move(Vector2 destion)
    {
        //Vector3 pos;
        //pos = this.transform.position;
        //pos.x += del * moving_speed * Time.fixedDeltaTime;
        ////pos.x = Mathf.Clamp(pos.x, .instance.left + width / 2, CameraController.instance.right - width / 2);
        ////pos.y += del * moving_speed;
        transform.DOMove(destion, moveSpeed);
    }
    public void MoveLoop(Vector2 destination, float rotation)
    {
        Vector2 pos = new Vector2(destination.x, destination.y);
        //pos = this.transform.position;
        //pos.x += del * moving_speed * Time.fixedDeltaTime;
        ////pos.x = Mathf.Clamp(pos.x, .instance.left + width / 2, CameraController.instance.right - width / 2);
        ////pos.y += del * moving_speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotation), 0.5f);//Rotate(0, 0, -rotation, Space.Self);
        //transform.position = Vector3.MoveTowards(transform.position, pos,moveSpeed* Time.deltaTime);
        Tweener twe;
        twe = transform.DOMove(pos, moveSpeed);
        twe.OnComplete(OnTweenCompleted);
        //s.Append(transform.DOMove(pos, moveSpeed));
        //s.AppendCallback(MoveToNextDestination);


    }

    private void OnTweenCompleted()
    {
        AudioController.instance.Play("STEP", isLoop: true);
        if (callback != null) callback.Invoke();
    }

    private void Jump(object obj)
    {
        //StopMoving();
        AudioController.instance.PlayOneShot("��Ծ��");
        AudioController.instance.Pause("STEP");
        isJumping = true;
        animationController.Jump();
        //Tween t = this.transform.DOMove(new Vector2(transform.position.x, transform.position.y+height * this.transform.localScale.y), delJumpTime);
        //t.OnComplete(Fall);
        Invoke(nameof(StopJumping), delJumpTime);

    }
    private void Fall()
    {
        Tween t = this.transform.DOMove(new Vector2(transform.position.x, ground), delJumpTime);
        t.OnComplete(StopJumping);
    }
    private void StopJumping()
    {
        AudioController.instance.Play("STEP");
        //animationController.StopJumping();
        animationController.Walk();
        isJumping = false;
        //ContinueMoving();


    }

    
    #endregion



    #region �ж�·��



    /// <summary>
    /// ��ͼ���غ���������·��
    /// </summary>
    /// <param name="obj"></param>
    private void SetRoutine(object obj)
    {
        routine = (List<Vector2>)obj;
        this.transform.position = routine[positionIndex];
        Vector2 vertical = GetVerticalVectorFrom(routine[(positionIndex - 1 + routine.Count) % routine.Count], routine[(positionIndex + 1) % routine.Count]);
        float rotation = Vector2.Angle(Vector2.up, vertical);
        transform.rotation = Quaternion.Euler(0, 0, rotation); //transform.Rotate(0, 0, rotation, Space.Self);
        positionIndex += 1;
        print("SetRoutine");
        
    }
    /// <summary>
    /// ��ͼж�غ�ɾ������·��
    /// </summary>
    /// <param name="obj"></param>
    private void DelRoutine(object obj)
    {
        routine = null;
        Reset();
    }

    private void TurnOver(object obj)
    {
        transform.localScale = new Vector3(transform.localScale.x , transform.localScale.y * -1, transform.localScale.z);
    }
    private void DirectionChange(object obj)
    {
        direction = direction * -1;
        FaceingDirectionChange();
    }
    private void FaceingDirectionChange()
    {
        child.transform.localScale = new Vector3(child.transform.localScale.x*-1, child.transform.localScale.y, child.transform.localScale.z);
    }
    #endregion

    private void Reset()
    {
        positionIndex = 0;
        isMoving = false;
       
    }

    private static Vector2 GetVerticalVectorFrom(Vector2 from, Vector2 to)
    {
        //from.x * -from.y + from.y * from.x
        // return new Vector2(from.y,-from.x);
        return new Vector2(-(to.y-from.y), (to.x-from.x));
    }
}
