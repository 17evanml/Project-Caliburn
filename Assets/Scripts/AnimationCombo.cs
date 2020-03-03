using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCombo : MonoBehaviour {
    public string animationName;
    public List<AnimationClip> animations = new List<AnimationClip>();
    public Animator animator = new Animator();
    public AnimatorOverrideController overrideAnim;
    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < animations.Count; i++) {
            animations[i].name = animationName + " " + i;

            
    
        }

        overrideAnim.clips[0].overrideClip = animations[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
