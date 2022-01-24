using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class PlayerData{
    public int health;
    public int mana;
    public int strength;
    public float playerPosX, playerPosY;
    public float minCamX, maxCamX, minCamY, maxCamY;
    public int souls;
    public int[] itemId;
    public int[] weaponId;
    public int[] armorId;
    public int[] keyId;
    public int upgradeCost;
    public int currentWeaponId;
    public int currentArmorId;
    public bool canDoubleJump;
    public bool canDash;
}
public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public int health = 100;
    public int mana = 50;
    public int strength = 10;
    public float playerPosX, playerPosY;
    public float minCamX, maxCamX, minCamY, maxCamY;
    public int souls;
    public int[] itemId;
    public int[] weaponId;
    public int[] armorId;
    public int[] keyId;
    public int upgradeCost;
    public int currentWeaponId;
    public int currentArmorId;
    public bool canDoubleJump = false;
    public bool canDash = false;

    private string filePath;

    // Start is called before the first frame update
    void Awake()
    {
        if(gm == null){
            gm = this;
        }
        else if(gm != this){
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/playerInfo.dat";

        Load();
    }

    public void Save(){
        Player player = FindObjectOfType<Player>();
        CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();

        itemId = new int[Inventory.inventory.items.Count];
        weaponId = new int[Inventory.inventory.weapons.Count];
        armorId = new int[Inventory.inventory.armors.Count];
        keyId = new int[Inventory.inventory.keys.Count];

        for (int i = 0; i < itemId.Length; i++){
            itemId[i] = Inventory.inventory.items[i].itemID;
        }

        for (int i = 0; i < weaponId.Length; i++){
            weaponId[i] = Inventory.inventory.weapons[i].itemID;
        }

        for (int i = 0; i < armorId.Length; i++){
            armorId[i] = Inventory.inventory.armors[i].itemID;
        }

        for (int i = 0; i < keyId.Length; i++){
            keyId[i] = Inventory.inventory.keys[i].itemID;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData data = new PlayerData();

        data.health = player.maxHealth;
        data.mana = player.maxMana;
        data.playerPosX = player.transform.position.x;
        data.playerPosY = player.transform.position.y;
        data.souls = player.souls;
        data.strength = player.strength;
        data.upgradeCost = upgradeCost;
        data.maxCamX = cameraFollow.maxXAndY.x;
        data.minCamX = cameraFollow.minXAndY.x;
        data.maxCamY = cameraFollow.maxXAndY.y;
        data.minCamY = cameraFollow.minXAndY.y;
        if(player.weaponEquipped != null){
            data.currentWeaponId = player.weaponEquipped.itemID;
        }
        if(player.armor != null){
            data.currentArmorId = player.armor.itemID;
        }
        data.canDoubleJump = player.doubleJumpSkill;
        data.canDash = player.dashSkill;

        data.itemId = itemId;
        data.weaponId = weaponId;
        data.armorId = armorId;
        data.keyId = keyId;

        bf.Serialize(file, data);

        file.Close();

        Debug.Log("Jogo Salvo!");
        FindObjectOfType<UIManager>().SetMessage("Jogo Salvo");
    }

    public void Load(){
        if(File.Exists(filePath)){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            health = data.health;
            mana = data.mana;
            strength = data.strength;
            playerPosX = data.playerPosX;
            playerPosY = data.playerPosY;
            maxCamX = data.maxCamX;
            minCamX = data.minCamX;
            maxCamY = data.maxCamY;
            minCamY = data.minCamY;
            souls = data.souls;
            upgradeCost = data.upgradeCost;
            currentArmorId = data.currentArmorId;
            currentWeaponId = data.currentWeaponId;
            canDoubleJump = data.canDoubleJump;
            canDash = data.canDash;
            itemId = data.itemId;
            weaponId = data.weaponId;
            armorId = data.armorId;
            keyId = data.keyId;
        }
    }
}
