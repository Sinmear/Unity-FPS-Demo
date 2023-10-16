using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;        // 角色控制器组件
    private Transform characterTrs;             // 角色节点
    public float walkSpeed;                     // 移动速度
    public float runSpeed;                      // 奔跑速度
    public float gravity = 15f;                 // 重力
    private float tmpMoveX;                     // 移动距离
    private float tmpMoveZ;
    private Vector3 tmpMovement;                // 移动向量
    private bool moveForward;                   // 是否前移
    public float jumpHeight;                    // 跳跃高度
    public float crouchHeight = 1f;
    private Animator characterAnimator;         // 动画状态机

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        characterTrs = transform;
        characterAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float tmp_speed = walkSpeed;
        if (characterController.isGrounded)
        {
            tmpMoveX = Input.GetAxis("Horizontal");
            tmpMoveZ = Input.GetAxis("Vertical");
            moveForward = Input.GetKey(KeyCode.W);
            tmpMovement = new Vector3(tmpMoveX, 0, tmpMoveZ);
            // 归一化位移距离
            tmpMovement.Normalize();
            // 转为世界坐标的位移距离
            tmpMovement = characterTrs.TransformDirection(tmpMovement);
            // 根据是否跳跃设置y轴距离
            if (Input.GetButtonDown("Jump"))
                tmpMovement.y = jumpHeight;
            else
                tmpMovement.y = -2f;
            // 判断移动速度
            bool shiftDown = Input.GetKey(KeyCode.LeftShift);
            tmp_speed = shiftDown ? runSpeed : walkSpeed;
            Vector3 velocity = characterController.velocity;
            velocity.y = 0;
            float velocityValue = velocity.magnitude;
            // 向前奔跑才播奔跑动画
            if (shiftDown && !moveForward && velocityValue > 7f)
                velocityValue = 7f;
            characterAnimator.SetFloat("Velocity", velocityValue, 0.25f, Time.deltaTime);
        }
        else
        {
            tmpMovement.y -= gravity * Time.deltaTime;
            characterAnimator.SetFloat("Velocity", 0, 0.25f, Time.deltaTime);
        }
        characterController.Move(tmpMovement * Time.deltaTime * tmp_speed);
    }
}
