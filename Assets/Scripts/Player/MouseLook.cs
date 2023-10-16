using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private Transform cameraTrs;
    [SerializeField]
    private Transform characterTrs;
    // ������
    public int cameraSensitivity;
    // ����Ƕ�
    private float rotationX;
    private float rotationY;
    // �����ת����
    public float maxAngle;
    public float minAngle;

    void Start()
    {
        cameraTrs = transform;
    }

    void Update()
    {
        rotationX = Input.GetAxis("Mouse X") * cameraSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * cameraSensitivity;
        rotationY = Mathf.Clamp(rotationY, minAngle, maxAngle);
        characterTrs.Rotate(Vector3.up * rotationX);
        cameraTrs.localRotation = Quaternion.Euler(rotationY, 0, 0);
    }
}
