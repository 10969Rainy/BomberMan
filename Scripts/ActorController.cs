using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActorController : MonoBehaviour {

    public Vector2 targetPos;

    public GameObject model;
    private KeyboardInput pi;
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    public float restTime = 0.15f;
    private float restTimer = 0.0f;
    private bool canMove = true;

    public GameObject bomber;

    public static int hp = 3;
    public Text hpText;
    public static int point = 0;
    public Text pointText;

    public AudioSource place;

    void Awake () {

        targetPos = transform.position;
        
        pi = GetComponent<KeyboardInput>();
        anim = model.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
	}
	
	void Update () {

        if (hp > 0)
        {
            //根据键盘的输入设置动画状态
            anim.SetFloat("y", pi.dUp);
            anim.SetFloat("x", pi.dRight);

            //只能单向移动
            if (pi.dUp == 1.0f || pi.dUp == -1.0f)
            {
                pi.dRight = 0.0f;
            }
            else if (pi.dRight == 1.0f || pi.dRight == -1.0f)
            {
                pi.dUp = 0.0f;
            }

            //根据按下键的方向检测物体
            coll.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(pi.dRight, pi.dUp));
            coll.enabled = true;

            //没有检测到物体，可以移动
            if (hit.transform == null || hit.collider.name == "Apple")
            {
                if (pi.dUp == 1.0f || pi.dRight == 1.0f || pi.dUp == -1.0f || pi.dRight == -1.0f)
                {
                    if (canMove)
                    {
                        targetPos = transform.position + new Vector3(pi.dRight, pi.dUp, 0);
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
            }

            //放置炸弹
            if (pi.enter)
            {
                Instantiate(bomber, transform.position, Quaternion.identity);
                place.Play();
            }
        }

        pointText.text = "Point: " + point;
        hpText.text = "HP: " + hp;
        if (hp <= 0)
        {
            anim.SetTrigger("die");
        }
    }

    void FixedUpdate() {
        
        if (rb.position != targetPos)
        {
            rb.MovePosition(Vector2.Lerp(transform.position, targetPos, 0.5f));
        }
    }

    //重开游戏
    public void Restart()
    {
        point = 0;
        hp = 3;
        SceneManager.LoadScene(0);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        print(coll.name);
        switch (coll.name)
        {
            case "Apple":
                hp += 1;
                Destroy(coll.gameObject);
                break;
            default:
                break;
        }
    }
}
