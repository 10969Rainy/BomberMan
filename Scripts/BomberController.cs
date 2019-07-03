using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberController : MonoBehaviour {

    public GameObject fireUp;
    public GameObject fireDown;
    public GameObject fireLeft;
    public GameObject fireRight;

    public AudioSource audioExplosion;
    public AudioSource audioBreak;

    public void Explosion() {
        
        audioExplosion.Play();
        
        RaycastHit2D hitUp = Physics2D.Linecast(transform.position, transform.position + new Vector3(0, 1, 0));
        RaycastHit2D hitDown = Physics2D.Linecast(transform.position, transform.position + new Vector3(0, -1, 0));
        RaycastHit2D hitLeft = Physics2D.Linecast(transform.position, transform.position + new Vector3(-1, 0, 0));
        RaycastHit2D hitRight = Physics2D.Linecast(transform.position, transform.position + new Vector3(1, 0, 0));
        
        DetectPos(hitUp, fireUp);
        DetectPos(hitDown, fireDown);
        DetectPos(hitLeft, fireLeft);
        DetectPos(hitRight, fireRight);
    }

    //根据炸弹的四个方向检测物体
    public void DetectPos(RaycastHit2D hitPos, GameObject firePos)
    {
        //没有物体，正常爆炸
        if (hitPos.transform == null)
        {
            firePos.SetActive(true);
        }
        else if (hitPos.transform != null)
        {
            //有玩家，爆炸，扣除主角的血量
            if (hitPos.transform.name == "PlayerHandler")
            {
                firePos.SetActive(true);
                hitPos.transform.gameObject.GetComponentInChildren<Animator>().SetTrigger("hurt");
                ActorController.hp -= 1;
            }
            //有可破坏的地形
            if (hitPos.transform.tag == "Jar")
            {
                ActorController.point += 1;
                hitPos.transform.gameObject.GetComponentInChildren<Animator>().SetTrigger("break");
                audioBreak.Play();
                Destroy(hitPos.transform.gameObject, 0.2f);
            }
            if (hitPos.transform.tag == "Enemy")
            {
                firePos.SetActive(true);
                ActorController.point += 2;
                hitPos.transform.gameObject.GetComponent<EnemyController>().hp -= 1;
                Destroy(hitPos.transform.gameObject, 0.2f);
            }
        }
    }

    public void EndExplosion()
    {
        Destroy(gameObject);
    }
}
