using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    private Text upgradeCostText;
    [SerializeField]
    private Text[] attributesText;
    [SerializeField]
    private GameObject upgradeMenu;

    private bool upgradeActive = false;
    private Player player;
    private int cursorIndex;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(upgradeActive){
            if(Input.GetKeyDown(KeyCode.DownArrow)){
                UpdateText();
                if(cursorIndex >= 3){
                    cursorIndex = 3;
                }
                else{
                    cursorIndex++;
                }
            }
            
            else if(Input.GetKeyDown(KeyCode.UpArrow)){
                UpdateText();
                if(cursorIndex <= 0){
                    cursorIndex = 0;
                }
                else{
                    cursorIndex--;
                }
            }

            if(cursorIndex == 0){
                attributesText[0].text = "Vida: " + player.maxHealth + " > " + Mathf.RoundToInt(player.maxHealth + (player.maxHealth * 0.1f));
                attributesText[0].color = new Color(0f/255f, 255f/255f, 252f/255f, 255f/255f);
            }
            else if(cursorIndex == 1){
                attributesText[1].text = "Mana: " + player.maxMana + " > " + Mathf.RoundToInt(player.maxMana + (player.maxMana * 0.1f));
                attributesText[1].color = new Color(0f/255f, 255f/255f, 252f/255f, 255f/255f);
            }
            else if(cursorIndex == 2){
                attributesText[2].text = "Força: " + player.strength + " > " + Mathf.RoundToInt(player.strength + (player.strength * 0.1f));
                attributesText[2].color = new Color(0f/255f, 255f/255f, 252f/255f, 255f/255f);
            }
            else if(cursorIndex == 3){
                if(player.defense <= 0){
                    attributesText[3].text = "Defense: " + player.defense + " > " + Mathf.RoundToInt(player.defense + 10 + (player.defense * 0.1f));
                    attributesText[3].color = new Color(0f/255f, 255f/255f, 252f/255f, 255f/255f);
                }
                else if(player.defense >= 1){
                    attributesText[3].text = "Defense: " + player.defense + " > " + Mathf.RoundToInt(player.defense + (player.defense * 0.1f));
                    attributesText[3].color = new Color(0f/255f, 255f/255f, 252f/255f, 255f/255f);
                }
            }

            if(Input.GetButtonDown("Submit") && player.souls >= GameManager.gm.upgradeCost){
                player.souls -= GameManager.gm.upgradeCost;
                GameManager.gm.upgradeCost += (int)(GameManager.gm.upgradeCost * 0.15f);
                if(cursorIndex == 0){
                    player.maxHealth += (int)(player.maxHealth * 0.1f);
                }
                else if(cursorIndex == 1){
                    player.maxMana += (int)(player.maxMana * 0.1f);
                }
                else if(cursorIndex == 2){
                    player.strength += (int)(player.strength * 0.1f);
                }
                else if(cursorIndex == 3){
                    if(player.defense <= 0){
                        player.defense += (int)(player.defense + (10 * 0.1f));
                    }
                    else if(player.defense >= 1){
                        player.defense += (int)(player.defense * 0.1f);
                    }
                }

                UpdateText();
                FindObjectOfType<UIManager>().UpdateUI();
            }
        }
    }

    void UpdateText(){
        upgradeCostText.text = "Custo: " + GameManager.gm.upgradeCost + " |" + " Souls: " + player.souls;
        attributesText[0].text = "Vida: " + player.maxHealth;
        attributesText[1].text = "Mana: " + player.maxMana;
        attributesText[2].text = "Força: " + player.strength;
        attributesText[3].text = "Defesa: " + player.defense;
        for (int i = 0; i < attributesText.Length; i++)
        {
            attributesText[i].color = Color.white;
        }
    }

    public void CallUpgradeManager(){
        upgradeActive = !upgradeActive;
        cursorIndex = 0;
        UpdateText();
        if(upgradeActive){
            upgradeMenu.SetActive(true);
        }
        else{
            upgradeMenu.SetActive(false);
        }
    }
}
