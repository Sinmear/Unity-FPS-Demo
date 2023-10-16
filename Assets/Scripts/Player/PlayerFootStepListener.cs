using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepListener : MonoBehaviour
{
    public FootStepAudioData footstepAudioData;
    private AudioSource footStepAudioSource;
    private CharacterController characterController;
    private Transform footStepTrs;
    private Vector3 rayDir;
    private float nextPlayTime;
    public LayerMask layerMask;

    private void Start()
    {
        footStepAudioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        footStepTrs = transform;
        rayDir = Vector3.down * (characterController.height / 2 + characterController.skinWidth - characterController.center.y);
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            if(characterController.velocity.normalized.magnitude >= 0.1f)
            {
                nextPlayTime += Time.fixedDeltaTime;
                // ���߼��ײ��Ӵ�����
                if (Physics.Linecast(footStepTrs.position,
                    footStepTrs.position + rayDir
                    , out RaycastHit hitInfo, layerMask))
                {
                    FootStepAudio audios = footstepAudioData.footStepAudios[0];
                    if (hitInfo.collider.CompareTag(audios.tag))
                    {
                        if (nextPlayTime >= audios.delay)
                        {
                            int audioCount = audios.audioClips.Count;
                            int audioIndex = Random.Range(0, audioCount);
                            // ���ѡһ����Ч����
                            footStepAudioSource.clip = audios.audioClips[audioIndex];
                            footStepAudioSource.Play();
                            nextPlayTime = 0;
                        }
                    }
                }
            }
        }
    }
}
