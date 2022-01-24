using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Transform cursor;
    public GameObject[] menuOptions;
    public GameObject optionPanel;
    public GameObject itemList;
    public GameObject itemListPrefab;
    public RectTransform content;
    public Text descriptionText;
    public Scrollbar scrollVertical;
    public Text healthText, manaText, strengthText, attackText, defenseText;
    public Text healthUI, manaUI, soulsUI, itemUI;
    public Text messageText;


    private bool pauseMenu = false;
    private int cursorIndex = 0;
    private Inventory inventory;
    public List<ItemList> items;
    private bool itemListActive = false;
    private Player player;
    private bool isMessageActive = false;
    private float textTimer;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.inventory;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMessageActive){
            Color color = messageText.color;
            color.a += 2f * Time.deltaTime;
            messageText.color = color;
            if(color.a >= 1){
                isMessageActive = false;
                textTimer = 0;
            }
        }
        else if(!isMessageActive){
            textTimer += Time.deltaTime;
            if(textTimer >= 2f){
                Color color = messageText.color;
                color.a -= 2f * Time.deltaTime;
                messageText.color = color;
                if(color.a <= 0){
                    messageText.text = "";
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.P)){
            pauseMenu = !pauseMenu;
            cursorIndex = 0;
            itemListActive = false;
            descriptionText.text = "";
            itemList.SetActive(false);
            optionPanel.SetActive(true);
            UpdateAtributes();
            UpdateUI();
            if(pauseMenu){
                pausePanel.SetActive(true);
            }
            else{
                pausePanel.SetActive(false);
            }
        }

        if(pauseMenu){
            Vector3 cursorPosition = new Vector3();

            if(!itemListActive){
                cursorPosition = menuOptions[cursorIndex].transform.position;
                cursor.position = new Vector3(cursorPosition.x - 100, cursorPosition.y, cursorPosition.z);
            }
            else if(itemListActive && items.Count > 0){
                cursorPosition = items[cursorIndex].transform.position;
                cursor.position = new Vector3(cursorPosition.x -60, cursorPosition.y, cursorPosition.z);
            }

            if(Input.GetKeyDown(KeyCode.DownArrow)){
                if(!itemListActive && cursorIndex >= menuOptions.Length - 1){
                    cursorIndex = menuOptions.Length - 1;
                }
                else if(itemListActive && cursorIndex >= items.Count - 1){
                    if(items.Count == 0){
                        cursorIndex = 0;
                    }
                    else{
                        cursorIndex = items.Count - 1;
                    }
                }
                else{
                    cursorIndex++;
                }
                if(itemListActive && items.Count > 0){
                    scrollVertical.value -= (1f / (items.Count - 1));
                    UpdateDescription();
                }
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow)){
                if(cursorIndex == 0){
                    cursorIndex = 0;
                }
                else{
                    cursorIndex--;
                }
                if(itemListActive && items.Count > 0){
                    scrollVertical.value += (1f / (items.Count -1));
                    UpdateDescription();
                }
            }

            if(Input.GetButtonDown("Submit") && !itemListActive){
                optionPanel.SetActive(false);
                itemList.SetActive(true);
                RefreshItemList();
                UpdateItemsList(cursorIndex);
                cursorIndex = 0;
                if(items.Count > 0){
                    UpdateDescription();
                }
                itemListActive = true;
            }
            else if((Input.GetButtonDown("Submit") && itemListActive && (player.Health < player.maxHealth)) || (Input.GetButtonDown("Submit") && itemListActive && (player.Mana < player.maxMana))){
                if(items.Count > 0){
                    UseItem();
                }
            }
            else if(Input.GetButtonDown("Cancel") && itemListActive){
                optionPanel.SetActive(true);
                itemList.SetActive(false);
                RefreshItemList();
                UpdateItemsList(cursorIndex);
                cursorIndex = 0;
                if(items.Count > 0){
                    UpdateDescription();
                }
                itemListActive = false;
            }
            else if(Input.GetButtonDown("Cancel") && pauseMenu){
                pausePanel.SetActive(false);
            }
        }
    }

    void UseItem(){
        if(items[cursorIndex].weapon != null){
            player.AddWeapon(items[cursorIndex].weapon);
        }
        else if(items[cursorIndex].consumableItem != null){
            player.UseItem(items[cursorIndex].consumableItem);
            inventory.RemoveItem(items[cursorIndex].consumableItem);
            cursorIndex = 0;
            RefreshItemList();
            UpdateItemsList(2);
            scrollVertical.value = 1;
        }
        else if(items[cursorIndex].armor != null){
            player.AddArmor(items[cursorIndex].armor);
        }
        UpdateAtributes();
        UpdateDescription();
    }

    void UpdateDescription(){
        if(items[cursorIndex].weapon != null){
            descriptionText.text = items[cursorIndex].weapon.description;
        }
        else if(items[cursorIndex].consumableItem != null){
            descriptionText.text = items[cursorIndex].consumableItem.description;
        }
        else if(items[cursorIndex].key != null){
            descriptionText.text = items[cursorIndex].key.description;
        }
        else if(items[cursorIndex].armor != null){
            descriptionText.text = items[cursorIndex].armor.description;
        }
    }

    void RefreshItemList(){
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();
    }

    void UpdateItemsList(int option){
        if(option == 0){
            for (int i = 0; i < inventory.weapons.Count; i++)
            {
                GameObject tempItem = Instantiate(itemListPrefab, content.transform);
                tempItem.GetComponent<ItemList>().SetUpWeapon(inventory.weapons[i]);
                items.Add(tempItem.GetComponent<ItemList>());
            }
        }
        else if(option == 1){
            for (int i = 0; i < inventory.armors.Count; i++)
            {
                GameObject tempItem = Instantiate(itemListPrefab, content.transform);
                tempItem.GetComponent<ItemList>().SetUpArmor(inventory.armors[i]);
                items.Add(tempItem.GetComponent<ItemList>());
            }
        }
        else if(option == 2){
            for (int i = 0; i < inventory.items.Count; i++)
            {
                GameObject tempItem = Instantiate(itemListPrefab, content.transform);
                tempItem.GetComponent<ItemList>().SetUpItem(inventory.items[i]);
                items.Add(tempItem.GetComponent<ItemList>());
            }
        }
        else if(option == 3){
            for (int i = 0; i < inventory.keys.Count; i++)
            {
                GameObject tempItem = Instantiate(itemListPrefab, content.transform);
                tempItem.GetComponent<ItemList>().SetUpKey(inventory.keys[i]);
                items.Add(tempItem.GetComponent<ItemList>());
            }
        }
    }

    void UpdateAtributes(){
        healthText.text = "Vida: " + player.GetHealth() + "/" + player.maxHealth;
        manaText.text = "Mana: " + player.GetMana() + "/" + player.maxMana;
        strengthText.text = "Força: " + player.strength;
        attackText.text = "Ataque: " + (player.strength + player.GetComponentInChildren<Attack>().GetDamage());
        defenseText.text = "Defesa: " + player.defense;
    }

    public void UpdateUI(){
        healthUI.text = player.GetHealth() + " / " + player.maxHealth;
        manaUI.text = player.GetMana() + " / " + player.maxMana;
        soulsUI.text = "Souls: " + player.souls;
        itemUI.text = "x" + inventory.CountItems(player.item);
    }

    public void SetMessage(string message){
        messageText.text = message;
        Color color = messageText.color;
        color.a = 0;
        messageText.color = color;
        isMessageActive = true;
    }
}
