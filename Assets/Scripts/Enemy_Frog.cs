using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{   //继承自Enemy
    private Rigidbody2D rb;//获取青蛙的Rigidbody
    private Collider2D coll;
    public LayerMask ground;
    public float speed = 5;//设置青蛙移动速度
    public float jumpForce;//跳跃的力
    public Transform right, left;//获取移动范围左右边界
    float rightx, leftx;//储存左右边界
    bool facingLeft = true;//判断青蛙朝向

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        leftx = left.position.x;
        rightx = right.position.x;
        Destroy(left.gameObject);
        Destroy(right.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    //青蛙的移动
    void Movement()
    {
        if (facingLeft)
        {   //向左移动
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            if (transform.position.x < leftx) //到了左边界，掉头
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingLeft = false;
            }
        }
        else
        {   //向右
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
            if (transform.position.x > rightx) //到了右边界，掉头
            {
                transform.localScale = new Vector3(1, 1, 1);
                facingLeft = true;
            }
        }
    }

    //动画
    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

}
