using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Key key;

    private Animator anim;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(Inventory.inventory.CheckKey(key)){
                anim.SetTrigger("doorOpen");
                boxCollider.enabled = false;
            }
            else{
                FindObjectOfType<UIManager>().SetMessage("Precisa da " + key.keyName);
            }
        }
    }
}
