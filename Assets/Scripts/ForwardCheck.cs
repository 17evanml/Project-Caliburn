﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardCheck : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward;
    }
}
