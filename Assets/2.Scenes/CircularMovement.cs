using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UniRx;
//using UniRx.Triggers;
using Unity.VisualScripting;


// https://unity-programmer.tistory.com/3 
public class CircularMovement : MonoBehaviour
{
    [Header("속도, 반지름")]
 
    [SerializeField] [Range(0f, 10f)] private float speed = 1;
    [SerializeField] [Range(0f, 10f)] private float radius = 5;
    [SerializeField] [Range(0f, 10f)] private float posY = 3;
 
    private float runningTime = 0;
    private Vector3 newPos = new Vector2();
 
    // Use this for initialization
    void Start()
    {
        // this.UpdateAsObservable()
        //     .Subscribe(_ =>
        //     {
        //         runningTime += Time.deltaTime * speed;
        //         float x = radius * Mathf.Cos(runningTime);
        //         float y = radius * Mathf.Sin(runningTime);
        //         newPos = new Vector2(x, y);
        //         this.transform.position = newPos;
        //
        //     });
    }

    void FixedUpdate()
    {
        runningTime += Time.deltaTime * speed;
        float x = radius * Mathf.Cos(runningTime);
        float z = radius * Mathf.Sin(runningTime);
        newPos = new Vector3(x, posY, z);
        this.transform.position = newPos;
    }
}
