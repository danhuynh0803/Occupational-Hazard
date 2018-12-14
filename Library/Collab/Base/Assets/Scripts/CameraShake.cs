using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    private float shakeTimer;
    private float shakeAmount;

    private Vector3 cameraPos;

    void Start()
    {
        cameraPos = transform.position;
    }

    void Update()
    {
        if (shakeTimer >= 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;
            transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z);
            shakeTimer -= Time.deltaTime;
        }

        // Return camera to original position after shaking
        else
        {
            transform.position = cameraPos;
        }
    }

    public void ShakeCamera(float shakePower, float shakeDuration)
    {
        shakeAmount = shakePower;
        shakeTimer = shakeDuration;
    }
}
