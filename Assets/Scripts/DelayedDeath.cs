using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDeath : MonoBehaviour
{
    public float deathTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timer(deathTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
