﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class craftingScript : MonoBehaviour
{
    GameManager gm;
    enum mode { SELECT, CRAFT, SELECTED };
    mode playMode = mode.SELECT;
    //UI Elements
    public Button[] blockButtons;
    public Text remaining;
    public Text used;
    public Text nudgeIncText;

    //Crafting REsources
    public int blocksUsed = 0;
    public int blocksRemaining = 0;
    //Type of object currently crafting with
    public GameObject currentCraftingMat;
    public int selectedBlock;
    //The actual instance of crafting object
    public GameObject activeObj;

    //REference to camera
    public GameObject cam;
    //Reference to sword
    public GameObject sword;

    public Material selectMat;

    public float nudgeInc;
    public float rotInc;
    public float rotSpeed = 1000;
    public float scrollSpeed = 10;
    public Material savedMat;

    private CraftingHistory history = new CraftingHistory();

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        blocksRemaining = gm.blocks[0];
        sword = GameObject.Find("Sword");
        for (int i = 0; i < blockButtons.Length; i++)
        {   
            int index = i;
            blockButtons[i].onClick.AddListener(() => giveCraftingMat(index));
        }
        updateText();
        //getJoint.onClick.AddListener(giveJointMat);
    }

    // Update is called once per frame
    void Update()
    {
        float xRotation = Input.GetAxis("Mouse Y") * rotSpeed;
        float yRotation = Input.GetAxis("Mouse X") * rotSpeed;
        float scrollMove = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

        //yRotation *= Time.deltaTime;
        //xRotation *= Time.deltaTime;

        if (playMode == mode.CRAFT)
        {
            CheckTag();
        }
        else if (playMode == mode.SELECT)
        {
            SelectBlock();
        }
        else if (playMode == mode.SELECTED)
        {
            SelectBlock();
            selectionInteractions();

        }
        if (Input.GetMouseButton(1))
        {
            cam.transform.Rotate(0, yRotation, 0, Space.World);
            cam.transform.Rotate(-xRotation, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            interrupt(playMode);
            playMode = mode.SELECT;

        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            history.Undo(gm);
        }
        cam.transform.GetChild(0).transform.Translate(0, 0, scrollMove);
    }

    void CheckTag()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //print(hitInfo.transform.gameObject.name);

            if (hitInfo.collider.gameObject.tag == "buildable")
            {
                PositionBlock(hitInfo);
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceBlock(hitInfo);
                }
            }
        }
        else
        {
            activeObj.transform.position = cam.transform.position;
        }
    }

    Vector3 tanVector(RaycastHit hit)
    {
        return hit.point - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void recursiveLayerChange(LayerMask l, GameObject g)
    {
        g.layer = l;
        for (int i = 0; i < g.transform.childCount; i++)
        {
            recursiveLayerChange(l, g.transform.GetChild(i).gameObject);
        }
    }
    void recursiveLayerRestore(GameObject g)
    {
        g.layer = LayerMask.NameToLayer(g.name);
        for (int i = 0; i < g.transform.childCount; i++)
        {
            recursiveLayerRestore(g.transform.GetChild(i).gameObject);
        }
    }

    public void toggleSnapSpots(bool ac)
    {
        for (int i = 1; i < sword.transform.childCount; i++)
        {
            Transform piece = sword.transform.GetChild(i).transform.GetChild(0);
            for (int j = 0; j < piece.childCount; j++)
            {
                piece.GetChild(j).gameObject.SetActive(ac);
            }
        }
    }

    void giveCraftingMat(int blockIndex)
    {
        selectedBlock = blockIndex;
        interrupt(playMode);
        playMode = mode.CRAFT;
        if (gm.blocks[blockIndex] > 0)
        {
            activeObj = Instantiate(gm.blockObjects[selectedBlock]);
            activeObj.tag = "craftingObj";
            recursiveLayerChange(LayerMask.NameToLayer("Ignore Raycast"), activeObj);
        }
        else
        {
            interrupt(playMode);
        }
    }

    void SelectBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hitInfo.collider.gameObject.tag == "buildable")
                {
                    if (activeObj != null)
                    {
                        activeObj.GetComponent<Renderer>().material = savedMat;
                    }
                    if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("snapSpot"))
                    {
                        activeObj = hitInfo.transform.parent.gameObject;
                    }
                    else
                    {
                        activeObj = hitInfo.transform.gameObject;
                    }
                    savedMat = activeObj.GetComponent<Renderer>().material;
                    //activeObj.GetComponent<Renderer>().material = ;
                    playMode = mode.SELECTED;
                }
            }
        }
    }

    void updateText()
    {
        for(int i = 0; i < blockButtons.Length; i++)
        {
            blockButtons[i].GetComponentInChildren<Text>().text = ""+ gm.blocks[i];
        }
        remaining.text = "Blocks Remaining: " + blocksRemaining;
        used.text = "Blocks Used: " + blocksUsed;
        nudgeIncText.text = "Nudge Increment: " + nudgeInc;
    }

    void interrupt(mode pM)
    {
        if (pM == mode.SELECTED)
        {
            activeObj.GetComponent<Renderer>().material = savedMat;
            activeObj = null;
            playMode = mode.SELECT;
        }
        else if (pM == mode.CRAFT)
        {
            Destroy(activeObj);
            playMode = mode.SELECT;
        }
    }

    void selectionInteractions()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && activeObj.name != "baseBlade")
        {
            DeleteBlock();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            activeObj.transform.Translate(-Vector3.left * nudgeInc);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            activeObj.transform.Translate(Vector3.left * nudgeInc);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            activeObj.transform.Translate(Vector3.up * nudgeInc);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            activeObj.transform.Translate(-Vector3.up * nudgeInc);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            activeObj.transform.Rotate(Vector3.forward, rotInc);
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            activeObj.transform.Rotate(-Vector3.forward, rotInc);
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            nudgeInc -= 0.05f;
            updateText();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            nudgeInc += 0.05f;
            updateText();
        }
    }

    void PositionBlock(RaycastHit hitInfo)
    {
        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("snapSpot"))
        {
            activeObj.transform.position = hitInfo.transform.position;
            activeObj.transform.rotation = hitInfo.transform.rotation;
        }
        else
        {
            activeObj.transform.position = hitInfo.point;
            activeObj.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
        }
    }

    void PlaceBlock(RaycastHit hitInfo)
    {
        gm.blocks[selectedBlock]--;
        DontDestroyOnLoad(activeObj);
        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("snapSpot"))
        {
            swordHierarchyNode node = hitInfo.transform.parent.GetComponent<swordHierarchyNode>();
            node.addChild(GetNode(activeObj));
            GetNode(activeObj).SetParent(node);
            GetNode(activeObj).add(GenerateTree(activeObj, selectedBlock, hitInfo.transform.parent.parent.gameObject));
            activeObj.transform.SetParent(hitInfo.transform.parent);

        }
        else
        {
            swordHierarchyNode node = hitInfo.transform.parent.GetComponent<swordHierarchyNode>();
            node.addChild(GetNode(activeObj));
            GetNode(activeObj).SetParent(node);
            GetNode(activeObj).add(GenerateTree(activeObj, selectedBlock, hitInfo.transform.parent.gameObject));
            activeObj.transform.SetParent(hitInfo.transform);

        }
        //activeObj.transform.SetParent(sword.transform);
        GameObject baseObj = activeObj.transform.GetChild(0).gameObject;
        recursiveLayerRestore(baseObj);
        updateText();
        activeObj = null;
        giveCraftingMat(selectedBlock);
    }

    swordHierarchyNode GetNode(GameObject g)
    {
        return g.transform.GetChild(0).GetComponent<swordHierarchyNode>();
    }
    SwordHierarchyTree GenerateTree(GameObject g, int blockIndex, GameObject parent)
    {
        return new SwordHierarchyTree(blockIndex, g.transform.position, g.transform.rotation, GetNode(parent).Self);
    }

    void DeleteBlock()
    {
        swordHierarchyNode node = GetNode(activeObj.transform.parent.gameObject);
        history.Add(new Delete(node.Self, node.GetParent()));
        GetNode(activeObj.transform.parent.gameObject).DeleteBlock(gm);
        updateText();
        interrupt(playMode);
    }

}
