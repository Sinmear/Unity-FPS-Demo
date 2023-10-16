using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public class FireArms : MonoBehaviour
    {
        public Transform muzzlePoint;                   // 子弹射出点
        public ParticleSystem muzzleParticle;           // 枪口特效
        public ParticleSystem casingParticle;           // 弹壳特效
        public GameObject bulletPref;                   // 子弹预设

        public int curAmmo;                             // 当前子弹数
        public int curMaxCarried;                       // 当前携带子弹数
        public int ammoInMag;                           // 弹夹子弹数
        public int ammoMaxCarried;                      // 最大可携带子弹数
        public float fireRate;                          // 射击倍速
        private float lastFireTime;                     // 上一次射击时间点
        private Animator gunAnimator;                   // 状态机
        private AnimatorStateInfo stateInfo;            // 动画信息
        public AudioSource shootAudioSource;            // 射击音效
        public AudioSource reloadAudioSource;           // 换弹音效
        public FirearmsAudioData firearmsAudioData;     // 音效数据
        public Camera eyeCamera;                        // 视角相机              
        private float originFov;                        // 初始相机观察距离
        private bool isAiming;                          // 是否瞄准
        public float spredAngle;                        // 散射角度
        private bool isOnTrigger;                       // 是否正在射击
        private IEnumerator doAimChecker;               // 瞄准携程
        private IEnumerator reloadChecker;              // 换弹携程

        private void Start()
        {
            curAmmo = ammoInMag;
            curMaxCarried = ammoMaxCarried;
            gunAnimator = GetComponent<Animator>();
            originFov = eyeCamera.fieldOfView;
            //reloadChecker = CheckReloadAnimEnd();
            //doAimChecker = DoAim();
        }

        public void HoldTrigger()
        {
            isOnTrigger = true;
            Shooting();
        }

        public void ReleaseTrigger()
        {
            isOnTrigger = false;
        }

        // 开始射击
        private void Shooting()
        {
            Debug.Log(IsAllowShooting());
            if (!IsAllowShooting()) return;
            // 设置状态机参数
            gunAnimator.Play("Fire", isAiming ? 1 : 0, 0);
            // 播放特效
            muzzleParticle.Play();
            casingParticle.Play();
            // 播放音效
            shootAudioSource.clip = firearmsAudioData.shootAudio;
            shootAudioSource.Play();
            // 创建子弹实例
            CreateBullet();
            curAmmo -= 1;
            lastFireTime = Time.time;
        }

        // 检测当前子弹数和射击时间间隔，判断能否射击
        private bool IsAllowShooting()
        {
            return curAmmo > 0 && Time.time - lastFireTime > 1 / fireRate;
        }

        // 创建子弹
        private void CreateBullet()
        {
            GameObject bulletObj = BulletManager.Instance.GetBulletPool().GetBullet();
            bulletObj.transform.position = muzzlePoint.position;
            bulletObj.transform.localRotation = muzzlePoint.rotation;
            // 加上散射角度
            bulletObj.transform.eulerAngles += CalcSpreadOffset();
        }

        private IEnumerator DestroyBullet()
        {
            while (true)
            {
                yield return new WaitForSeconds(4);

            }
        }

        // 换弹
        public void Reload()
        {
            // 弹夹满的或者没有子弹时不能换弹
            if (curAmmo == ammoInMag || curMaxCarried <= 0) return;
            // 调整状态机层级权重
            gunAnimator.SetLayerWeight(2, 1);
            // 设置状态机参数
            gunAnimator.SetTrigger(curAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");
            // 播放音效
            reloadAudioSource.clip = curAmmo > 0 ? firearmsAudioData.reloadLeftAudio : firearmsAudioData.reloadOutofAudio;
            reloadAudioSource.Play();
            // 开启携程检测动画结束
            if (reloadChecker == null)
            {
                reloadChecker = CheckReloadAnimEnd();
                StartCoroutine(reloadChecker);
            }
            else
            {
                StopCoroutine(reloadChecker);
                reloadChecker = null;
                reloadChecker = CheckReloadAnimEnd();
                StartCoroutine(reloadChecker);
            }
        }

        public void Aiming(bool _isAiming)
        {
            isAiming = _isAiming;
            // 调整状态机层级权重
            gunAnimator.SetLayerWeight(1, 1);
            // 设置状态机参数
            gunAnimator.SetBool("Aim", isAiming);
            // 开启携程检测动画结束
            if (doAimChecker == null)
            {
                doAimChecker = DoAim();
                StartCoroutine(doAimChecker);
            }
            else
            {
                StopCoroutine(doAimChecker);
                doAimChecker = null;
                doAimChecker = DoAim();
                StartCoroutine(doAimChecker);
            }
        }

        // 检测换弹是否结束的携程
        protected IEnumerator CheckReloadAnimEnd()
        {
            while (true)
            {
                yield return null;
                stateInfo = gunAnimator.GetCurrentAnimatorStateInfo(2);
                if (stateInfo.IsTag("ReloadAmmo"))
                {
                    if (stateInfo.normalizedTime > 0.9f)
                    {
                        // 换弹动画播放结束，计算子弹数量
                        int needCount = ammoInMag - curAmmo;
                        int remainCount = curMaxCarried - needCount;
                        if (remainCount >= 0)
                            curAmmo = ammoInMag;
                        else
                            curAmmo = curMaxCarried;
                        curMaxCarried = remainCount < 0 ? 0 : remainCount;
                        yield break;
                    }
                }
            }
        }

        // 瞄准后相机观察距离变化的携程
        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;
                float tmpFov = 0f;
                eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, isAiming ? 40 : originFov, ref tmpFov, Time.deltaTime * 2f);
            }
        }

        // 获得散射角度
        protected Vector3 CalcSpreadOffset()
        {
            return spredAngle / eyeCamera.fieldOfView * Random.insideUnitCircle;
        }
    }
}

