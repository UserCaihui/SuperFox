using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private Animator anim;
    public Collider2D playerCollider;
    public LayerMask ground;
    public Transform groundCheck;
    public float speed;
    public float jumpForce;
    bool isGround, jumpPressed, isHurt;
    int jumpCount = 2;//跳跃次数实现多段跳

    public Text cherryNum,gemNum;
    int cherry, gem;


    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (!isHurt)
        {   //未受伤时才能移动及跳跃
            Movement();
            Jump();
        }
        SwitchAnim();
    }

    //人物移动
    void Movement()
    {
        //float horizontalMove = Input.GetAxis("Horizontal"); //获取水平移动方向
        float direction = Input.GetAxisRaw("Horizontal");   //获取方向，移动、转身
        
        //移动
        playerBody.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, playerBody.velocity.y);

        //转身
        if(direction != 0)
        {
            playerBody.transform.localScale = new Vector3(direction, 1, 1);
        }
       
    }

    //人物跳跃
    void Jump()
    {
        
        if (isGround)
        {
            jumpCount = 2;//每次落地后都能刷新跳跃次数
        }
        if (jumpPressed && isGround)
        {   //一段
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && !isGround)
        {   //二段
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }

    }

    //动画的切换
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(playerBody.velocity.x)); //移动的动画

        //受伤动画
        if (isHurt)
        {
            anim.SetBool("hurting", true);
            if (Mathf.Abs(playerBody.velocity.x) < 0.5f)
            {   //退后了一定距离停下
                anim.SetBool("hurting", false);
                isHurt = false;
            }
        }

        //有关跳跃的动画
        if (isGround)
        {   //在地面时
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
        else if (playerBody.velocity.y > 0)
        {   //上升阶段
            anim.SetBool("jumping", true);
            anim.SetBool("idle", false);
        }
        else if (playerBody.velocity.y < 0)
        {   //下落阶段
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        if (!isGround && playerBody.velocity.y < 0.2f) 
        {   //直接掉落
            anim.SetBool("falling", true);
        }
       
    }

    //获取物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //分别检测樱桃和钻石
        if (collision.CompareTag("cherry"))
        {
            Destroy(collision.gameObject);
            cherry++;
            cherryNum.text = cherry.ToString();
        }
        if (collision.CompareTag("gem"))
        {
            Destroy(collision.gameObject);
            gem++;
            gemNum.text = gem.ToString();
        }
    }

    //触碰敌人的互动
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            //踩到敌人（掉落时）能消灭，同时被弹起
            if (anim.GetBool("falling") && transform.position.y > (collision.gameObject.transform.position.y + 1))
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
                anim.SetBool("jumping", true);
                enemy.Explosion();
            }
            //其他方式碰到会受伤并后退
            else
            {
                isHurt = true;
                float damage = transform.position.x - collision.transform.position.x;
                playerBody.velocity = new Vector2(6 * damage / Mathf.Abs(damage), playerBody.velocity.y);
            }
        }
    }
}
