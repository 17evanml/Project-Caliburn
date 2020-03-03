using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProperties : MonoBehaviour
{
    public bool invincible = false;
    private float iFrames = 2;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<PlaceSword>().setSword(GameObject.Find("Sword"));
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage)
    {
        if (!invincible)
        {
            StartCoroutine(wasHit());
            print("player: ouch");
        }
    }

    IEnumerator wasHit()
    {
        invincible = true;
        yield return new WaitForSeconds(iFrames);
        invincible = false;
    }
}
