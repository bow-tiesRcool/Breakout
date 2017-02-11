using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour {

    public float shakeDuration = 1;
    public float shakeStrength = 1;

    private bool isShaking = false;

	public void Shake()
    {
        if (!isShaking) StartCoroutine("ShakeCoroutine");
    }

    IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        Vector3 startPos = transform.position;
        for (float t = 0; t < shakeDuration; t += Time.deltaTime)
        {
            float frac = t / shakeDuration;
            transform.position = startPos + Vector3.Lerp(Random.insideUnitCircle, Vector3.zero, frac) * shakeStrength;
            yield return new WaitForEndOfFrame();
        }
        transform.position = startPos;
        isShaking = false;
    }

}
