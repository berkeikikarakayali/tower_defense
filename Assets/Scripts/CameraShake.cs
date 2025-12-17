using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
    public static CameraShake cameraShake;

    void Awake()
    {
        cameraShake = this;
    }

    public IEnumerator Shake(float duration)
    {
        Vector3 startPos = transform.localPosition; //store start position
        float elapsed = 0f;
    
        while (elapsed < duration)
        {   

            //random offsets
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);

            transform.localPosition = new Vector3(startPos.x + x, startPos.y + y, startPos.z);
            elapsed += Time.deltaTime;

            //Wait for the next frame
            yield return null;
        }

        //Reset
        transform.localPosition = startPos;
    }
}

