using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    public List<Weapon> weapons;
    public List<Key> keys;
    public List<ConsumableItem> items;
    public List<Armor> armors;

    public ItemDataBase itemDataBase;

    // Start is called before the first frame update
    void Awake()
    {
        if(inventory == null){
            inventory = this;
        }
        else if(inventory != this){
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        LoadInventory();
    }

    private void Start(){
        FindObjectOfType<UIManager>().UpdateUI();
    }
    
    void LoadInventory(){
        for (int i = 0; i < GameManager.gm.weaponId.Length; i++){
            AddWeapon(itemDataBase.GetWeapon(GameManager.gm.weaponId[i]));
        }

        for (int i = 0; i < GameManager.gm.armorId.Length; i++){
            AddArmor(itemDataBase.GetArmor(GameManager.gm.armorId[i]));
        }

        for (int i = 0; i < GameManager.gm.itemId.Length; i++){
            AddItem(itemDataBase.GetConsumableItem(GameManager.gm.itemId[i]));
        }

        for (int i = 0; i < GameManager.gm.keyId.Length; i++){
            AddKey(itemDataBase.GetKey(GameManager.gm.keyId[i]));
        }
    }

    public void AddWeapon(Weapon weapon){
        weapons.Add(weapon);
    }
    
    public void AddArmor(Armor armor){
        armors.Add(armor);
    }

    public void AddKey(Key key){
        keys.Add(key);
    }

    public bool CheckKey(Key key){
        for (int i = 0; i < keys.Count; i++)
        {
            if(keys[i] == key){
                return true;
            }
        }
        return false;
    }

    public void AddItem(ConsumableItem item){
        items.Add(item);
    }

    public void RemoveItem(ConsumableItem item){
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i] == item){
                items.RemoveAt(i);
                break;
            }
        }
    }

    public int CountItems(ConsumableItem item){
        int numberOfItems = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if(item == items[i]){
                numberOfItems++;
            }
        }
        return numberOfItems;
    }
}
