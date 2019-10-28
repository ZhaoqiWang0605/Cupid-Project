using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    float radian = 0;           // 弧度  
    float perRadian = 3.0f;    // 每次变化的弧度速度 上下浮动
    float radius = 0.1f;        // 半径  
    Vector3 oldPos;             // 开始时候的位置坐标 

    // Start is called before the first frame update
    void Start()
    {
        oldPos = transform.position; // 将最初的位置保存到oldPos 
    }

    // Update is called once per frame
    void Update()
    {
        radian += perRadian * Time.deltaTime; // 弧度每次加3 * deltaTime
        float dy = Mathf.Sin(radian) * radius; // dy定义的是针对y轴的变量，也可以使用sin，找到一个适合的值就可以  
        transform.position = oldPos + new Vector3(0, dy, 0);
    }
}
