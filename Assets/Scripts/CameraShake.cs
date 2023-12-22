using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;
    public static CameraShake Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Camera is null");
            }
            return _instance;
        }
    }

    public Transform cameraTransform = default;
    private Vector3 _originalPosOfCam = default;
    public float shakeFrequency = default;
    public bool cameraShake = false;
    private Player _player;
 


    void Start()
    {
        _instance = this;
        _originalPosOfCam = cameraTransform.position;
        _player = FindObjectOfType<Player>();
    }



    void Update()
    {
        if (cameraShake)
        {
            CameraShakes();
        }
        else
        {
            StopShake();
        }   
    }



    public void StartShaking()
    {
        if (!cameraShake)
        {
            cameraShake = true;
            StartCoroutine(StopShakeAfterDuration());
        }
    }

    private IEnumerator StopShakeAfterDuration()
    {
        if (_player != null && _player.GetSensorDamage())
        {
            yield return new WaitForSeconds(5.0f);
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
        }
        cameraShake = false;
    }

    public void CameraShakes()
    {
        cameraTransform.position = _originalPosOfCam + Random.insideUnitSphere * shakeFrequency;
    }

    private void StopShake()
    {
        cameraTransform.position = _originalPosOfCam;
    }
}
