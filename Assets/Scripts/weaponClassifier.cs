using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

public class weaponClassifier : MonoBehaviour
{
    public float learnR;
    public Vector3 CoM;
    public sVector Dist;
    public GameObject sword;
    public NeuralNetwork nn;
    public Button[] buttons = new Button[4];
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        //parseString();
        int[] layers = { 18, 15, 9 };  
        nn = new NeuralNetwork(9, layers, 3);
        nn.LearningRate = 1;
        buttons[0].onClick.AddListener(button0);
        buttons[1].onClick.AddListener(button1);
        buttons[2].onClick.AddListener(button2);
        buttons[3].onClick.AddListener(button3);
        if (7 + "hi" == "bubba")
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            setValues();
            print(CoM);
            print(Dist);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            setValues();
            print(CoM);
            print(Dist);
        }
        Debug.DrawLine(sword.transform.position, CoM, Color.green);
        //Debug.DrawLine(CoM, new Vector3(Dist.x, 0, 0), Color.red);
        //Debug.DrawLine(CoM, new Vector3(0, Dist.y, 0), Color.red);
        //Debug.DrawLine(CoM, new Vector3(0, 0, Dist.z), Color.red);
    }

    void setValues()
    {
        CoM = centerOfMass();
        Dist = distToCoM();
    }

    sVector genValues()
    {
        setValues();
        //print(CoM);
        //print(Dist);
        return new sVector(CoM.x, CoM.y, CoM.z, Dist.get(0), Dist.get(1), Dist.get(2), Dist.get(3), Dist.get(4), Dist.get(5));
    }


    Vector3 centerOfMass()
    {
        float[] pos = sumPos(getBase());
        int count = (int)pos[3];
        return new Vector3(pos[0] / count, pos[1] / count, pos[2] / count);
    }


    sVector distToCoM()
    {
        float[] dist = sumDist(getBase(), CoM);
        int count = (int)dist[3];
        return new sVector(dist[0] / count, dist[1] / count, dist[2] / count, dist[4], dist[5], dist[6]);
    }


    float[] sumDist(GameObject block, Vector3 Center)
    {
        float[] ret = new float[7];
        ret[0] = Mathf.Abs(block.transform.position.x - Center.x);
        ret[1] = Mathf.Abs(block.transform.position.y - Center.y);
        ret[2] = Mathf.Abs(block.transform.position.z - Center.z);
        ret[3] = 1;
        ret[4] = 0;
        ret[5] = 0;
        ret[6] = 0;


        if (hasChildren(block))
        {
            GameObject[] children = getBlockChildren(block);
            for (int i = 0; i < children.Length; i++)
            {
                float[] temp = sumDist(children[i], Center);
                ret[0] += temp[0];
                ret[1] += temp[1];
                ret[2] += temp[2];
                ret[3] += temp[3];
                for (int j = 4; j < 7; j++)
                {
                    if (temp[j - 4] > ret[j])
                    {
                        ret[j] = temp[j - 4];
                    }
                }

            }
        }
        return ret;
    }

    float[] sumPos(GameObject block)
    {
        float[] ret = new float[4];
        ret[0] = block.transform.position.x;
        ret[1] = block.transform.position.y;
        ret[2] = block.transform.position.z;
        ret[3] = 1;
        if (hasChildren(block))
        {
            GameObject[] children = getBlockChildren(block);
            for (int i = 0; i < children.Length; i++)
            {
                float[] temp = sumPos(children[i]);
                ret[0] += temp[0];
                ret[1] += temp[1];
                ret[2] += temp[2];
                ret[3] += temp[3];
            }
        }
        return ret;
    }

    GameObject getBase()
    {
        return sword.transform.GetChild(1).GetChild(0).gameObject;
    }

    GameObject getChild(GameObject block, int i)
    {
        int snapSpots = block.GetComponent<swordHierarchyNode>().getSnapSpots();
        return block.transform.GetChild(i + snapSpots).GetChild(0).gameObject;
    }

    GameObject[] getBlockChildren(GameObject block)
    {
        int snapSpots = block.GetComponent<swordHierarchyNode>().getSnapSpots();
        GameObject[] ret = new GameObject[block.transform.childCount - snapSpots];
        for (int i = snapSpots; i < block.transform.childCount; i++)
        {
            ret[i - snapSpots] = block.transform.GetChild(i).GetChild(0).gameObject;
        }
        return ret;
    }

    bool hasChildren(GameObject block)
    {
        int snapSpots = block.GetComponent<swordHierarchyNode>().getSnapSpots();
        if (block.transform.childCount > snapSpots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    sVector updateVal(float lr, int includes, sVector xs, sVector weights)
    {
        xs.scale(includes);
        xs.scale(lr);
        weights.add(xs);
        return weights;
    }

    string genString()
    {
        float[] values = genValues().toArray();
        string s = "";
        for (int i = 0; i < values.Length; i++)
        {
            s += values[i] + ",";
        }
        return s;
    }

    static void WritePTron(string s)
    {
        string path = "Assets/Resources/data.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(s);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (UnityEngine.TextAsset)Resources.Load("data");

        //Print the text from the file
        Debug.Log(asset.text);
    }

    //static void WriteCoMData(string s, int weapon)
    //{
    //    string path;
    //    if(weapon == 0)
    //    {
    //        path = "Assets/Resources/CoMS.txt";
    //    } else if (weapon == 1)
    //    {
    //        path = "Assets/Resources/CoMH.txt";
    //    }
    //    else
    //    {
    //        path = "Assets/Resources/CoMA.txt";
    //    }


    //    //Write some text to the test.txt file
    //    StreamWriter writer = new StreamWriter(path, true);
    //    writer.WriteLine(s);
    //    writer.Close();

    //    //Re-import the file to update the reference in the editor
    //    AssetDatabase.ImportAsset(path);
    //    TextAsset asset = (UnityEngine.TextAsset)Resources.Load("CoMS");

    //    //Print the text from the file
    //    Debug.Log(asset.text);
    //}
    //static void WriteADist(string s, int weapon)
    //{
    //    string path;
    //    if (weapon == 0)
    //    {
    //        path = "Assets/Resources/ADistS.txt";
    //    }
    //    else if (weapon == 1)
    //    {
    //        path = "Assets/Resources/ADistH.txt";
    //    }
    //    else
    //    {
    //        path = "Assets/Resources/ADistA.txt";
    //    }


    //    //Write some text to the test.txt file
    //    StreamWriter writer = new StreamWriter(path, true);
    //    writer.WriteLine(s);
    //    writer.Close();

    //    //Re-import the file to update the reference in the editor
    //    AssetDatabase.ImportAsset(path);
    //    TextAsset asset = (UnityEngine.TextAsset)Resources.Load("ADistS");

    //    //Print the text from the file
    //    Debug.Log(asset.text);
    //}
    //static void WriteMDist(string s, int weapon)
    //{
    //    string path;
    //    if (weapon == 0)
    //    {
    //        path = "Assets/Resources/MDistS.txt";
    //    }
    //    else if (weapon == 1)
    //    {
    //        path = "Assets/Resources/MDistH.txt";
    //    }
    //    else
    //    {
    //        path = "Assets/Resources/MDistA.txt";
    //    }


    //    //Write some text to the test.txt file
    //    StreamWriter writer = new StreamWriter(path, true);
    //    writer.WriteLine(s);
    //    writer.Close();

    //    //Re-import the file to update the reference in the editor
    //    AssetDatabase.ImportAsset(path);
    //    TextAsset asset = (UnityEngine.TextAsset)Resources.Load("MDistS");

    //    //Print the text from the file
    //    Debug.Log(asset.text);
    //}

    //static string ReadString()
    //{

    //    string path = "Assets/Resources/pTron.txt";

    //    //Read the text from directly from the test.txt file
    //    StreamReader reader = new StreamReader(path);
    //    string s = reader.ReadToEnd();
    //    //print(s);
    //    reader.Close();
    //    return s;
    //}

    //void parseString()
    //{
    //    string s = ReadString();
    //    int startIndex = 0;
    //    float val;
    //    int currentPTron = 0;
    //    int currentVal = 0;
    //    for (int i = 0; i < s.Length; i++)
    //    {
    //        if (s[i].Equals(','))
    //        {
    //            if (float.TryParse(s.Substring(startIndex, i - startIndex), out val))
    //            {
    //                pTron[currentPTron].set(currentVal, val);
    //                startIndex = i + 1;
    //                currentVal++;
    //            }
    //        }
    //        else if (s[i].Equals(';'))
    //        {
    //            startIndex = i + 1;
    //            currentPTron++;
    //            currentVal = 0;
    //        }
    //    }
    //}

    //private void OnApplicationQuit()
    //{
    //    WritePTron(genString());
    //}

    private void button0()
    {
        float[] target = { 1, 0, 0 };
        WritePTron(genString()+"0;");
        nn.Train(genValues().toArray(), target);

    }
    private void button1()
    {
        float[] target = { 0, 1, 0 };
        WritePTron(genString() + "1;");
        nn.Train(genValues().toArray(), target);

    }
    private void button2()
    {
        float[] target = { 0, 0, 1 };
        WritePTron(genString() + "2;");
        nn.Train(genValues().toArray(), target);
    }
    private void button3()
    {
        float[] output = nn.feedForward(genValues().toArray());
        float max = float.MinValue;
        int index = 0;
        for (int i = 0; i < output.Length; i++)
        {
            if (output[i] > max)
            {
                max = output[i];
                index = i;
            }
        }
        text.text = "Sword: " + output[0] + "\n" + "Hammer: " + output[1] + "\n" + "Axe: " + output[2];
    }
}





public class sVector
{
    private float[] vec;
    public sVector(params float[] val)
    {
        vec = val;
    }

    public void set(int i, float val)
    {
        vec[i] = val;
    }

    public int mag()
    {
        return vec.Length;
    }

    public float get(int x)
    {
        return vec[x];
    }

    public float dot(sVector other)
    {
        float ret = 0;
        if (this.mag() == other.mag())
        {
            for (int i = 0; i < this.mag(); i++)
            {
                ret += this.get(i) * other.get(i);
            }
        }
        return ret;
    }

    public void add(sVector other)
    {
        if (this.mag() == other.mag())
        {
            for (int i = 0; i < this.mag(); i++)
            {
                vec[i] = vec[i] + other.get(i);
            }
        }
    }
    public float[] toArray()
    {
        return vec;
    }

    public string print()
    {
        string ret = "";
        ret += "(";
        for (int i = 0; i < this.mag() - 1; i++)
        {
            ret += this.get(i) + ", ";
        }
        ret += this.get(this.mag() - 1);
        ret += ")";
        Debug.Log(ret);
        return ret;
    }
    public string printMDist()
    {
        string ret = "";
        ret += "(";
        for (int i = 3; i < this.mag() - 1; i++)
        {
            ret += this.get(i) + ", ";
        }
        ret += this.get(this.mag() - 1);
        ret += ")";
        Debug.Log(ret);
        return ret;
    }
    public string printADist()
    {
        string ret = "";
        ret += "(";
        for (int i = 0; i < 3; i++)
        {
            ret += this.get(i) + ", ";
        }
        ret += this.get(this.mag() - 1);
        ret += ")";
        Debug.Log(ret);
        return ret;
    }

    public void scale(float scl)
    {
        for (int i = 0; i < this.mag(); i++)
        {
            vec[i] *= scl;
        }
    }
}
