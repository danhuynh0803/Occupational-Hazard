using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    private static float shakeTimer;
    private static float shakeAmount;
    private static bool isShaking;
    private Vector3 cameraPos;

    void Start()
    { 
        cameraPos = transform.position;
    }

    void FixedUpdate()
    {
        if(isShaking)
        {
            if (shakeTimer >= 0)
            {
                Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;
                transform.position = transform.position + new Vector3(ShakePos.x, ShakePos.y, 0f);
                shakeTimer -= Time.fixedDeltaTime;
            }

            // Return camera to original position after shaking
            else
            {
                isShaking = false;
                transform.position = cameraPos;
            }
        }

    }

    public static void Shake(float shakePower, float shakeDuration)
    {
        isShaking = true;
        shakeAmount = shakePower;
        shakeTimer = shakeDuration;
    }
}
