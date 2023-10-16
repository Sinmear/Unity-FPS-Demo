using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon {
    public class WeaponManager : MonoBehaviour
    {
        public FireArms curWeapon;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (curWeapon == null) return;
            // ��ʼ���
            if (Input.GetMouseButton(0))
                curWeapon.HoldTrigger();
            // �������
            if (Input.GetMouseButtonUp(0))
                curWeapon.ReleaseTrigger();
            // ����
            if (Input.GetKeyDown(KeyCode.R))
                curWeapon.Reload();
            // ��׼
            if (Input.GetMouseButtonDown(1))
                curWeapon.Aiming(true);
            // ������׼
            if (Input.GetMouseButtonUp(1))
                curWeapon.Aiming(false);
        }
    }
}
