using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField]
    private string message;

    private bool enterSave = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enterSave){
            if(Input.GetKeyDown(KeyCode.Z)){
                GameManager.gm.Save();
            }
            else if(Input.GetKeyDown(KeyCode.C)){
                FindObjectOfType<UpgradeManager>().CallUpgradeManager();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            enterSave = true;
            FindObjectOfType<UIManager>().SetMessage(message);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            enterSave = false;
        }
    }
}
