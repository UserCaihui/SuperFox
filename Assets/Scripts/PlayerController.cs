using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private Animator anim;
    public Collider2D playerCollider;
    public Collider2D disColl;
    public LayerMask ground;
    public Transform ceilingCheck, groundCheck;
    public float speed;
    public float jumpForce;
    bool isGround, isCrouch, isHurt;
    int jumpCount = 2;//跳跃次数实现多段跳

    public Text cherryNum,gemNum;   //用于显示收集的樱桃、宝石的数目
    int cherry, gem;                //储存数目，其实收集了也没什么用


    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        if (isCrouch)
        {   //趴下状态移速减半
            playerBody.velocity = new Vector2(direction * speed/2 * Time.fixedDeltaTime, playerBody.velocity.y);
        }
        else { playerBody.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, playerBody.velocity.y); }

        //转身
        if(direction != 0)
        {
            playerBody.transform.localScale = new Vector3(direction, 1, 1);
        }

        //下蹲
        if (!Physics2D.OverlapCircle(ceilingCheck.position, 0.1f, ground) && isGround)
        {
            if (Input.GetButtonDown("Crouch"))
            {
                disColl.enabled = false;
                isCrouch = true;
                anim.SetBool("crouching", true);
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                disColl.enabled = true;
                isCrouch = false;
                anim.SetBool("crouching", false);
            }
        }
    }

    //人物跳跃
    void Jump()
    {
        
        if (isGround)
        {
            jumpCount = 2;//每次落地后都能刷新跳跃次数
        }
        if (Input.GetButtonDown("Jump") && isGround&&!isCrouch)
        {   //一段
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
            jumpCount--;
        }
        else if (Input.GetButtonDown("Jump") && jumpCount > 0 && !isGround)
        {   //二段
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce);
            jumpCount--;
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
            //注意如果开始是趴下的状态，也要取消
            disColl.enabled = true;
            isCrouch = false;
            anim.SetBool("crouching", false);
        }
       
    }

    //触碰物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //分别检测樱桃和钻石
        if (collision.CompareTag("cherry"))
        {
            Destroy(collision.gameObject);
            cherry++;
            cherryNum.text = cherry.ToString();
        }
        else if (collision.CompareTag("gem"))
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
