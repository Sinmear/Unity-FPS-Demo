using UnityEngine;
using System.Collections;

namespace Scripts.Weapon
{
    public class TargetScript : MonoBehaviour
    {
        private Animation animation;
        private float randomTime = 4f;
        private bool routineStarted = false;
        public bool isHit = false;

        private void Start()
        {
            animation = GetComponent<Animation>();

        }

        private void Update()
        {
            if (isHit)
            {
                if (!routineStarted)
                {
                    routineStarted = true;
                    animation.Play("target_down");
                    StartCoroutine(DelayTimer());
                }
            }
        }

        private IEnumerator DelayTimer()
        {
            yield return new WaitForSeconds(randomTime);
            animation.Play("target_up");
            isHit = false;
            routineStarted = false;
        }
    }
}
