using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour {
    public enum input {Heavy, Light, Null};
    public input[] comboInputs = new input[3];
    public int currentHit;
    public float comboResetTime;
    private float timer;

    public string heavyButton;
    public string lightButton;
    public Animator anim;
    public bool attacking = false;


    // Start is called before the first frame update
    void Start() {
        resetCombo();
    }

    // Update is called once per frame
    void Update()
    {
        addInput();
        timerResetCombo();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Enter")
        || anim.GetCurrentAnimatorStateInfo(0).IsName("Enter 0")) {
            attacking = true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Exit")) {
            attacking = false;
        }

        testAnimation("test");


        
    }

    void LateUpdate() {
        // if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Strafing") || anim.GetCurrentAnimatorStateInfo(0).IsName("Running")) {
        //     timer = 0;
        //     resetCombo();
        // }
    }

    private void addInput() {
        if (Input.GetButtonDown(heavyButton)) { 
            if (currentHit < 3) { comboInputs[currentHit] = input.Heavy; currentHit++; }
            else if (!attacking) { resetCombo(); comboInputs[currentHit] = input.Heavy; currentHit++; }
            playAnimation();
            timer = comboResetTime;
        } else if (Input.GetButtonDown(lightButton)) { 
            if (currentHit < 3) { comboInputs[currentHit] = input.Light; currentHit++; } 
            else if (!attacking) { resetCombo(); comboInputs[currentHit] = input.Light; currentHit++; }
            playAnimation();
            timer = comboResetTime;
        }
    }

    private void resetCombo() {
        currentHit = 0; 
        for (int i = 0; i < comboInputs.Length; i++) {
            comboInputs[i] = input.Null;
        }
    }

    private void timerResetCombo() {
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            resetCombo();
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Exit")) {
            timer = 0;
            resetCombo();
        }
    }

    private void animReset() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Strafing") || anim.GetCurrentAnimatorStateInfo(0).IsName("Running")) {
            timer = 0;
            resetCombo();
        }
    }

    private void playAnimation() {
        if (comboInputs[2] == input.Heavy) {
            anim.SetTrigger("Heavy3");
        } else if (comboInputs[1] == input.Heavy) {
            anim.SetTrigger("Heavy2");
        } else if (comboInputs[0] == input.Heavy) {
            anim.SetTrigger("Heavy1");
        }

        if (comboInputs[2] == input.Light) {
            anim.SetTrigger("Light3");
        } else if (comboInputs[1] == input.Light) {
            anim.SetTrigger("Light2");
        } else if (comboInputs[0] == input.Light) {
            anim.SetTrigger("Light1");
        }
    }

    private void testAnimation(string trigger) {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            anim.SetTrigger(trigger);
        }
    }
}
