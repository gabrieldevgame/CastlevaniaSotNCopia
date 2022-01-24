using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public Image image;
    public Text text;
    public ConsumableItem consumableItem;
    public Weapon weapon;
    public Key key;
    public Armor armor;

    public void SetUpItem(ConsumableItem item){
        consumableItem = item;
        image.sprite = consumableItem.image;
        text.text = consumableItem.itemName;
    }

    public void SetUpKey(Key item){
        key = item;
        image.sprite = key.image;
        text.text = key.keyName;
    }

    public void SetUpWeapon(Weapon item){
        weapon = item;
        image.sprite = weapon.image;
        text.text = weapon.weaponName;
    }
    
    public void SetUpArmor(Armor item){
        armor = item;
        image.sprite = armor.image;
        text.text = armor.armorName;
    }
}
