using System.Collections;
using UnityEngine;

public class NightLight : MonoBehaviour
{
    public Light spotLight;
    public float time = 140f;

    void Start()
    {
        StartCoroutine(LightOnOff());
    }

    IEnumerator LightOnOff()
    {
        while (true)
        {
            spotLight.enabled = !spotLight.enabled;
            yield return new WaitForSeconds(time);
        }
    }
}
