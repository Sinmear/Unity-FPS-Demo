using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public class FireArms : MonoBehaviour
    {
        public Transform muzzlePoint;                   // �ӵ������
        public ParticleSystem muzzleParticle;           // ǹ����Ч
        public ParticleSystem casingParticle;           // ������Ч
        public GameObject bulletPref;                   // �ӵ�Ԥ��

        public int curAmmo;                             // ��ǰ�ӵ���
        public int curMaxCarried;                       // ��ǰЯ���ӵ���
        public int ammoInMag;                           // �����ӵ���
        public int ammoMaxCarried;                      // ����Я���ӵ���
        public float fireRate;                          // �������
        private float lastFireTime;                     // ��һ�����ʱ���
        private Animator gunAnimator;                   // ״̬��
        private AnimatorStateInfo stateInfo;            // ������Ϣ
        public AudioSource shootAudioSource;            // �����Ч
        public AudioSource reloadAudioSource;           // ������Ч
        public FirearmsAudioData firearmsAudioData;     // ��Ч����
        public Camera eyeCamera;                        // �ӽ����              
        private float originFov;                        // ��ʼ����۲����
        private bool isAiming;                          // �Ƿ���׼
        public float spredAngle;                        // ɢ��Ƕ�
        private bool isOnTrigger;                       // �Ƿ��������
        private IEnumerator doAimChecker;               // ��׼Я��
        private IEnumerator reloadChecker;              // ����Я��

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

        // ��ʼ���
        private void Shooting()
        {
            Debug.Log(IsAllowShooting());
            if (!IsAllowShooting()) return;
            // ����״̬������
            gunAnimator.Play("Fire", isAiming ? 1 : 0, 0);
            // ������Ч
            muzzleParticle.Play();
            casingParticle.Play();
            // ������Ч
            shootAudioSource.clip = firearmsAudioData.shootAudio;
            shootAudioSource.Play();
            // �����ӵ�ʵ��
            CreateBullet();
            curAmmo -= 1;
            lastFireTime = Time.time;
        }

        // ��⵱ǰ�ӵ��������ʱ�������ж��ܷ����
        private bool IsAllowShooting()
        {
            return curAmmo > 0 && Time.time - lastFireTime > 1 / fireRate;
        }

        // �����ӵ�
        private void CreateBullet()
        {
            GameObject bulletObj = BulletManager.Instance.GetBulletPool().GetBullet();
            bulletObj.transform.position = muzzlePoint.position;
            bulletObj.transform.localRotation = muzzlePoint.rotation;
            // ����ɢ��Ƕ�
            bulletObj.transform.eulerAngles += CalcSpreadOffset();
        }

        private IEnumerator DestroyBullet()
        {
            while (true)
            {
                yield return new WaitForSeconds(4);

            }
        }

        // ����
        public void Reload()
        {
            // �������Ļ���û���ӵ�ʱ���ܻ���
            if (curAmmo == ammoInMag || curMaxCarried <= 0) return;
            // ����״̬���㼶Ȩ��
            gunAnimator.SetLayerWeight(2, 1);
            // ����״̬������
            gunAnimator.SetTrigger(curAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");
            // ������Ч
            reloadAudioSource.clip = curAmmo > 0 ? firearmsAudioData.reloadLeftAudio : firearmsAudioData.reloadOutofAudio;
            reloadAudioSource.Play();
            // ����Я�̼�⶯������
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
            // ����״̬���㼶Ȩ��
            gunAnimator.SetLayerWeight(1, 1);
            // ����״̬������
            gunAnimator.SetBool("Aim", isAiming);
            // ����Я�̼�⶯������
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

        // ��⻻���Ƿ������Я��
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
                        // �����������Ž����������ӵ�����
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

        // ��׼������۲����仯��Я��
        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;
                float tmpFov = 0f;
                eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, isAiming ? 40 : originFov, ref tmpFov, Time.deltaTime * 2f);
            }
        }

        // ���ɢ��Ƕ�
        protected Vector3 CalcSpreadOffset()
        {
            return spredAngle / eyeCamera.fieldOfView * Random.insideUnitCircle;
        }
    }
}

