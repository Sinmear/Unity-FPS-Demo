using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float bulletSpeed = 100f;        // 子弹速度
        public GameObject prefab;               // 子弹预制体
        private Transform bulletTrs;            // 子弹节点
        private Vector3 prePosition;            // 子弹上一个位置
        private float existTime = 4f;
        private float existTimer = 0;
        private bool isAcitve = false;

        private void Start()
        {
            bulletTrs = transform;
            prePosition = bulletTrs.position;
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
                prePosition = bulletTrs.position;
                bulletTrs.Translate(0, 0, bulletSpeed * Time.deltaTime);
                if (Physics.Raycast(prePosition, (bulletTrs.position - prePosition).normalized, out RaycastHit hit, 1f))
                {
                    GameObject bulletEffect = BulletManager.Instance.GetBulletPool().GetBulletEffect();
                    bulletEffect.transform.position = hit.point;
                    bulletEffect.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        TargetScript target = hit.collider.GetComponent<TargetScript>();
                        target.isHit = true;
                    }
                }
            }
            else
            {
                isAcitve = false;
                BulletManager.Instance.GetBulletPool().RecycleBullet(this.gameObject);
            }
        }
    }
}
