using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public class BulletPool : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public GameObject bulletEffectPrefab;
        private Queue<GameObject> bulletList = new Queue<GameObject>();
        private Queue<GameObject> bulletEffectList = new Queue<GameObject>();
        private Transform bulletParent;
        private Transform effectParent;

        private void Start()
        {
            bulletParent = transform.Find("BulletParent");
            effectParent = transform.Find("EffectParent");
        }

        public GameObject GetBullet()
        {
            GameObject bulletObj = null;
            if(bulletList.Count > 0)
            {
                bulletObj = bulletList.Dequeue();
            }
            else
            {
                bulletObj = GameObject.Instantiate(bulletPrefab);
                bulletObj.transform.SetParent(bulletParent);
            }
            bulletObj.SetActive(true);
            return bulletObj;
        }

        public void RecycleBullet(GameObject bullet)
        {
            if(bulletList.Count > 20)
            {
                Destroy(bullet);
            }
            else
            {
                bullet.SetActive(false);
                bulletList.Enqueue(bullet);
            }
        }

        public GameObject GetBulletEffect()
        {
            GameObject bulletEffectObj = null;
            if (bulletEffectList.Count > 0)
            {
                bulletEffectObj = bulletEffectList.Dequeue();
            }
            else
            {
                bulletEffectObj = GameObject.Instantiate(bulletEffectPrefab);
                bulletEffectObj.transform.SetParent(effectParent);
            }
            bulletEffectObj.SetActive(true);
            return bulletEffectObj;
        }

        public void RecycleBulletEffect(GameObject bulletEffect)
        {
            if (bulletEffectList.Count > 20)
            {
                Destroy(bulletEffect);
            }
            else
            {
                bulletEffect.SetActive(false);
                bulletEffectList.Enqueue(bulletEffect);
            }
        }
    }
}
