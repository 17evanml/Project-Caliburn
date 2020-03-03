using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    static int[] layers = { 5, 5 };
    public GameObject player;
    NeuralNetwork nn /*= new NeuralNetwork(7, layers, 5)*/;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int decision(Vector3 playerPos, bool attacking, int previousChoice1, int previousChoice2, int previousChoice3)
    {
        int index = -1;
        int at = 0;
        if(attacking == true)
        {
            at = 1;
        }
        float[] inputs = { player.transform.position.x, player.transform.position.y, player.transform.position.z, at, previousChoice1, previousChoice2, previousChoice3 };
        float[] decision =  nn.feedForward(inputs);
        for (int i = 0; i < decision.Length; i++)
        {
            float max = float.MinValue;
            if (decision[i] > max)
            {
                max = decision[i];
                index = i;
            }
        }
        return index;
    }
}
