using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;        // ��ɫ���������
    private Transform characterTrs;             // ��ɫ�ڵ�
    public float walkSpeed;                     // �ƶ��ٶ�
    public float runSpeed;                      // �����ٶ�
    public float gravity = 15f;                 // ����
    private float tmpMoveX;                     // �ƶ�����
    private float tmpMoveZ;
    private Vector3 tmpMovement;                // �ƶ�����
    private bool moveForward;                   // �Ƿ�ǰ��
    public float jumpHeight;                    // ��Ծ�߶�
    public float crouchHeight = 1f;
    private Animator characterAnimator;         // ����״̬��

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
            // ��һ��λ�ƾ���
            tmpMovement.Normalize();
            // תΪ���������λ�ƾ���
            tmpMovement = characterTrs.TransformDirection(tmpMovement);
            // �����Ƿ���Ծ����y�����
            if (Input.GetButtonDown("Jump"))
                tmpMovement.y = jumpHeight;
            else
                tmpMovement.y = -2f;
            // �ж��ƶ��ٶ�
            bool shiftDown = Input.GetKey(KeyCode.LeftShift);
            tmp_speed = shiftDown ? runSpeed : walkSpeed;
            Vector3 velocity = characterController.velocity;
            velocity.y = 0;
            float velocityValue = velocity.magnitude;
            // ��ǰ���ܲŲ����ܶ���
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
