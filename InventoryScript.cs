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
    [Tooltip("The UI tile/element you want to be highlighted upon selection.")]
    public GameObject ItemUiTile;
    [Tooltip("This audio clip will be played upon selection.")]
    public AudioClip Audio;
}


public class InventoryScript : MonoBehaviour
{
    [Tooltip("The sprite used to highlight which item is currently in use.")]
    public GameObject InventoryCursor;//The sprite that highlights which gameobject is currently selected.
    public Element[] Inventory;//We want a catlegory item varients which we can collect into the array "Inventory".

    //Private / hidden from view.
    private int PreviousPosition=0; //We use this to calculate the delta of IndexPosition per frame.
    private AudioSource Audiosource; //The component on the "PC-Player" used to play an audio clip.
    private Text ItemText;
    private int IndexPosition=0;
    /// <summary>
    /// Play's the linked sound-clip when a new item is selected.
    /// </summary>
    private void PlaySound(Element Item)
    {
        AudioSource As = GetComponent<AudioSource>();
        As.clip = Item.Audio;
        As.Play(0);
    }

    private void Start()
    {
        Audiosource = GetComponent<AudioSource>(); ItemText = GameObject.FindGameObjectWithTag("InventoryText").GetComponent<Text>();
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
            PlaySound(Inventory[IndexPosition]);
        }

        //UI:
        ItemText.text = Inventory[IndexPosition].ItemName;
        InventoryCursor.transform.position = Inventory[IndexPosition].ItemUiTile.transform.position;

        Debug.Log(IndexPosition);//For Testing.
        PreviousPosition = IndexPosition;
    }

}
