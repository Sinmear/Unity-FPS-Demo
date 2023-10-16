using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public class BulletManager
    {
        private static BulletManager instance = null;
        public static BulletManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new BulletManager();
                return instance;
            }
        }

        private Transform bulletTrs = GameObject.FindGameObjectWithTag("BulletPool").transform;
        private BulletPool bulletPool;

        public BulletPool GetBulletPool()
        {
            if(bulletPool == null)
            {
                bulletPool = bulletTrs.GetComponent<BulletPool>();
            }
            return bulletPool;
        }
    }
}

