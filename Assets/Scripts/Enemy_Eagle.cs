using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{   //继承自Enemy
    private Rigidbody2D rb;
    private Collider2D coll;
    public float speed = 3;
    public Transform top, bottom;//获取移动范围上下边界
    float topy, bottomy;//储存上下边界
    bool up = true;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        topy = top.position.y;
        bottomy = bottom.position.y;
        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //老鹰上下移动
    void Movement()
    {
        if (up)
        {   //向上移动
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > topy) //到了上边界，掉头
            {
                up = false;
            }
        }
        else
        {   //向下
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < bottomy) //到了下边界，掉头
            {
                up = true;
            }
        }
    }
}
