using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public class BulletEffect : MonoBehaviour
    {
        private float existTime = 2f;
        private float existTimer = 0;
        private bool isAcitve = false;

        private void Start()
        {

        }

        private void OnEnable()
        {
            existTimer = 0;
            isAcitve = true;
        }
        private void Update()
        {
            if (!isAcitve) return;
            if (existTimer < existTime)
            {
                existTimer += Time.deltaTime;
            }
            else
            {
                isAcitve = false;
                BulletManager.Instance.GetBulletPool().RecycleBulletEffect(this.gameObject);
            }
        }
    }
}

