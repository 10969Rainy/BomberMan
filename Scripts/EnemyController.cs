using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Vector2 targetPos;

    public GameObject model;
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private float dUp = -1;
    private float dRight = 0;

    public float restTime = 0.15f;
    private float restTimer = 0.0f;
    private bool canMove = true;

    public int hp = 1;

    void Awake() {

        targetPos = transform.position;
        
        anim = model.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update() {

        if (hp > 0)
        {
            //根据键盘的输入设置动画状态
            //anim.SetFloat("y", dUp);
            //anim.SetFloat("x", dRight);

            //只能单向移动
            if (dUp == 1.0f || dUp == -1.0f)
            {
                dRight = 0.0f;
            }
            else if (dRight == 1.0f || dRight == -1.0f)
            {
                dUp = 0.0f;
            }

            //根据按下键的方向检测物体
            coll.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(dRight, dUp));
            coll.enabled = true;

            //没有检测到物体，可以移动
            if (hit.transform == null)
            {
                if (dUp == 1.0f || dRight == 1.0f || dUp == -1.0f || dRight == -1.0f)
                {
                    if (canMove)
                    {
                        targetPos = transform.position + new Vector3(dRight, dUp, 0);
                        canMove = false;
                    }
                    else
                    {
                        restTimer += Time.deltaTime;
                        if (restTimer >= restTime)
                        {
                            canMove = true;
                            restTimer = 0.0f;
                        }
                    }
                }
                else
                {
                    //将移动的位置设为整数
                    transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
                }
            }
            else
            {
                //检测到不同物体，执行不同事件
                //print(hit.collider.name);
                if (hit.collider.name != "PlayerHandler")
                {
                    int x = Random.Range(0, 4);
                    switch (x)
                    {
                        case 0:
                            //向下走
                            dUp = -1;
                            dRight = 0;
                            break;
                        case 1:
                            //向上走
                            dUp = 1;
                            dRight = 0;
                            break;
                        case 2:
                            //向左走
                            dUp = 0;
                            dRight = -1;
                            break;
                        case 3:
                            //向右走
                            dUp = 0;
                            dRight = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            hp -= 1;
        }
        if (hp <= 0)
        {
            anim.SetTrigger("die");
            Destroy(gameObject, 0.25f);
        }
    }

    void FixedUpdate() {

        if (rb.position != targetPos)
        {
            rb.MovePosition(Vector2.Lerp(transform.position, targetPos, 0.5f));
        }
    }
}
