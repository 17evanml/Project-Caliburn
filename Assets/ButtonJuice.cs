using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonJuice : MonoBehaviour {
    bool hover = false;
    bool clicked = false; 
    public Material material;

    // bool raised = false;

    Vector3 current;
    Vector3 raised;
    Quaternion rot;
    public Vector3 move = new Vector3(1f, 1f, 1f);
    public float raiseAmount = 3f;


    // Start is called before the first frame update
    void Start()
    {
        current = transform.position;
        raised = current + new Vector3(0, raiseAmount, 0);
        rot = transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (hover) { hover = false; }
            else { hover = true; }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (clicked) { clicked = false; hover = false; }
            else { clicked = true; hover = true; }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            raised = current + new Vector3(0, raiseAmount, 0);
        }

        onHover();
        onClick();
        
    }


    void onHover() {
        if (hover) {
            transform.position = Vector3.Lerp(transform.position, raised, 0.1f);
            Debug.Log("raised");
        } else {
            transform.position = Vector3.Lerp(transform.position, current, 0.1f);
            Debug.Log("lowered");
        }
    }

    void onClick() {
        if (clicked) {
            transform.rotation = transform.rotation * Quaternion.Euler(move);
        } else {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.1f);
        }
    }

    // void onHover() {
    //     if (hover) {
    //         transform.position = Vector3.Lerp(transform.position, raised, 0.1f);
    //         Debug.Log("raised");
    //     } else {
    //         transform.position = Vector3.Lerp(transform.position, current, 0.1f);
    //         Debug.Log("lowered");
    //     }
    // }

    // void onClick() {
    //     if (clicked) {
    //         transform.rotation = transform.rotation * Quaternion.Euler(move);
    //     } else {
    //         transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.1f);
    //     }
    // }
}
