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
            // 开始射击
            if (Input.GetMouseButton(0))
                curWeapon.HoldTrigger();
            // 结束射击
            if (Input.GetMouseButtonUp(0))
                curWeapon.ReleaseTrigger();
            // 换弹
            if (Input.GetKeyDown(KeyCode.R))
                curWeapon.Reload();
            // 瞄准
            if (Input.GetMouseButtonDown(1))
                curWeapon.Aiming(true);
            // 结束瞄准
            if (Input.GetMouseButtonUp(1))
                curWeapon.Aiming(false);
        }
    }
}
