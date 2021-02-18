using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
//Script by Thomas Jacobs | S212046.
public class Element //The element class acts as the blueprints for each possible item in an inventory.
{
    [Tooltip("The text you want to appear in the UI upon selection.")]
    public string ItemName;
    [Tooltip("The phyisical item you want to spawn into the game-world.")]
    public GameObject Item;
    public GameObject ItemLocation;
    [Tooltip("The UI tile/element you want to be highlighted upon selection.")]
    public GameObject ItemUiTile;
    [Tooltip("This audio clip will be played upon selection.")]
    public AudioClip Audio;

    public void SpawnItem(Vector3 pos, AudioSource As)
    {
        GameObject NewObj = MonoBehaviour.Instantiate(Item); NewObj.transform.position = pos;
        NewObj.transform.position += new Vector3(0, NewObj.GetComponent<Collider>().bounds.size.y / 2, 0);
        PlaySound(As);
    }
    public void PlaySound(AudioSource As)
    {
        As.clip = Audio;
        As.Play(0);
    }
}
public class InventoryScript : MonoBehaviour
{
    [Tooltip("The sprite used to highlight which item is currently in use.")]
    public GameObject InventoryCursor;//The sprite that highlights which gameobject is currently selected.
    public Element[] Inventory;//We want a catlegory item varients which we can collect into the array "Inventory".
    private GameObject ObjLocation;

    //Private / hidden from view.
    private int PreviousPosition=0; //We use this to calculate the delta of IndexPosition per frame.
    private AudioSource Audiosource; //The component on the "PC-Player" used to play an audio clip.
    private Text ItemText;
    private int IndexPosition=0;
    /// <summary>
    /// Play's the linked sound-clip when a new item is selected.
    /// </summary>
    private void Start()
    {
        Audiosource = GetComponent<AudioSource>(); ItemText = GameObject.FindGameObjectWithTag("InventoryText").GetComponent<Text>();
        ObjLocation = Instantiate(Inventory[IndexPosition].ItemLocation);
    }

    private void Update()
    {
        IndexPosition += Mathf.FloorToInt(Input.mouseScrollDelta.y); //We round the delta of our input to an int rather than a float.
        //We do a simple check to make sure the user can never go past the min or max inventory items.
        if(IndexPosition < 0) { IndexPosition = Inventory.Length-1; }
        else if(IndexPosition > Inventory.Length - 1) { IndexPosition = 0; }
        //If a new item is selected...
        if (PreviousPosition != IndexPosition)
        {
            Inventory[IndexPosition].PlaySound(Audiosource);
            Destroy(ObjLocation.gameObject);
            ObjLocation = Instantiate(Inventory[IndexPosition].ItemLocation);
        }

        //UI:
        ItemText.text = Inventory[IndexPosition].ItemName;
        InventoryCursor.transform.position = Inventory[IndexPosition].ItemUiTile.transform.position;
        PreviousPosition = IndexPosition;


        //Raycast:
   
        RaycastHit hit;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            if(Input.GetMouseButtonDown(0)){ Inventory[IndexPosition].SpawnItem(hit.point, Audiosource); }
            else
            {
                ObjLocation.transform.position = hit.point + new Vector3(0, ObjLocation.GetComponent<Collider>().bounds.size.y/2, 0);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 10000, Color.red);
        }
    }
}
