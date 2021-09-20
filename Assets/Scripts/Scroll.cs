using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    
    private float right; //右边界
    private float left; //左边界
    private float distance; //左右边界距离
    public Transform player;//获取人物位置

    // Start is called before the first frame update
    void Start()
    {
        //计算左右边界。Bounds是当图形的边界框
        SpriteRenderer sRender = GetComponent<SpriteRenderer>();
        right = transform.position.x + sRender.bounds.extents.x / 3;
        left = transform.position.x - sRender.bounds.extents.x / 3;
        distance = right - left;
    }

    // Update is called once per frame
    void Update()
    {
        //判断人物的位置
        float decision = player.position.x;
        //确定地图的判断点
        float leftpoint = transform.position.x - distance / 3;
        float rightpoint = transform.position.x + distance / 3;

        //判断是否触碰判断点
        if (decision >= rightpoint)
        {
            //如果到达右侧，将背景图片的位置向前（x轴方向）调整一个背景图片长度的距离
            transform.position += new Vector3(distance, 0, 0);
        }
        else if(decision<leftpoint)
        {
            //如果到达左侧，将背景图片的位置向后（x轴方向）调整一个背景图片长度的距离
            transform.position -= new Vector3(distance, 0, 0);
        }
    }
}
