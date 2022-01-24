using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerSkill{
    dash, doubleJump
}

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public Transform groundCheck;
    public Transform wallCheck;
    public float jumpForce;
    public float fireRate;
    public ConsumableItem item;
    public int maxHealth;
    public int maxMana;
    public int strength;
    public int defense;
    public int souls;
    public bool deadCheck = false;
    public float dashForce;
    public bool doubleJumpSkill = false;
    public bool dashSkill = false;
    [SerializeField]
    private int manaCost;
    [SerializeField]
    private Rigidbody2D projectile;
    [SerializeField]
    private int projectileDamage;


    private float speed;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool onGround;
    private bool onRoofWall;
    private bool jump = false;
    private bool doubleJump;
    public Weapon weaponEquipped;
    private Animator anim;
    private Attack attack;
    private float nextAttack;
    private int health;
    public int Health{get{return health;}}
    private int mana;
    public int Mana{get{return mana;}}
    public Armor armor;
    private bool canDamage = true;
    private SpriteRenderer sprite;
    private bool isDead = false;
    private bool dash = false;
    private GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attack = GetComponentInChildren<Attack>();
        sprite = GetComponent<SpriteRenderer>();
        gm = GameManager.gm;
        SetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead){
            float h = Input.GetAxisRaw("Horizontal");
            
            onGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            if(onGround){
                doubleJump = false;
            }

            if(Input.GetButtonDown("Jump") && (onGround || (!doubleJump && doubleJumpSkill))){
                jump = true;
                if(!doubleJump && !onGround){
                    doubleJump = true;
                }
            }
            
            if(Input.GetButtonDown("Fire1") && Time.time > nextAttack && weaponEquipped != null){
                dash = false;
                anim.SetTrigger("Attack");
                attack.PlayAnimation(weaponEquipped.animation);
                nextAttack = Time.time + fireRate;
            }

            if(Input.GetButtonDown("Fire3") && FindObjectOfType<UIManager>().itemUI.text != "x" + 0 && health < maxHealth){
                UseItem(item);
                Inventory.inventory.RemoveItem(item);
                FindObjectOfType<UIManager>().UpdateUI();
            }

            if(Input.GetKeyDown(KeyCode.X) && onGround && !dash && dashSkill){
                rb.velocity = Vector2.zero;
                anim.SetTrigger("Dash");
            }
            else if(Input.GetKeyDown(KeyCode.S) && mana >= manaCost){
                Rigidbody2D tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
                tempProjectile.GetComponent<Attack>().SetWeapon(projectileDamage);
                if(facingRight){
                    tempProjectile.AddForce(new Vector2(5, 11), ForceMode2D.Impulse);
                }
                else{
                    tempProjectile.AddForce(new Vector2(-5, 11), ForceMode2D.Impulse);
                }

                mana -= manaCost;
                FindObjectOfType<UIManager>().UpdateUI();
            }
        }
    }

    private void FixedUpdate() {
        if(!isDead){
            float h = Input.GetAxisRaw("Horizontal");

            if(canDamage && !dash){
                rb.velocity = new Vector2(h * speed, rb.velocity.y);
                if(onGround){
                    anim.SetFloat("Impulse", Mathf.Abs(h));
                    anim.SetFloat("Speed", Mathf.Abs(h));
                }
            }

            if(h > 0 && !facingRight){
                Flip();
            }
            else if(h < 0 && facingRight){
                Flip();
            }

            if(jump){
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce);
                jump = false;
            }

            if(dash){
                int hForce = facingRight ? 1 : -1;
                rb.velocity = Vector2.left * dashForce * hForce;
                //dash andando
                if(dash && (speed > 0 && h == 0 || speed < 0 && h == 0)){
                    dash = true;
                }
            }
        }
    }

    void Flip(){
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void AddWeapon(Weapon weapon){
        weaponEquipped = weapon;
        attack.SetWeapon(weaponEquipped.damage);
    }

    public void AddArmor(Armor item){
        armor = item;
        defense = armor.defense;
    }

    public void UseItem(ConsumableItem item){
        health += item.healthGain;
        if(health > maxHealth){
            health = maxHealth;
        }

        mana += item.manaGain;
        if(mana > maxMana){
            mana = maxMana;
        }
    }

    public int GetHealth(){
        return health;
    }

    public int GetMana(){
        return mana;
    }

    public void TakeDamage(int damage){
        if(canDamage){
            canDamage = false;
            anim.SetTrigger("Hurt");
            health -= (damage - defense);
            FindObjectOfType<UIManager>().UpdateUI();
            if(health <= 0){
                anim.SetTrigger("Dead");
                Invoke("ReloadScene", 3f);
                isDead = true;
                deadCheck = true;
            }
            else{
                StartCoroutine(DamageCoroutine());
            }
        }
    }

    IEnumerator DamageCoroutine(){
        for (float i = 0; i < 0.6f; i+=0.2f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        canDamage = true;
    }

    void ReloadScene(){
        Souls.instance.gameObject.SetActive(true);
        Souls.instance.souls = souls;
        Souls.instance.transform.position = transform.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DashTrue(){
        dash = true;
    }
    
    public void DashFalse(){
        dash = false;
    }

    public void SetPlayerSkill(PlayerSkill skill){
        if(skill == PlayerSkill.dash){
            dashSkill = true;
        }
        else if(skill == PlayerSkill.doubleJump){
            doubleJumpSkill = true;
        }
    }

    public void SetPlayer(){
        Vector3 playerPos = new Vector3(gm.playerPosX, gm.playerPosY, 0);
        transform.position = playerPos;
        maxHealth = gm.health;
        maxMana = gm.mana;
        speed = maxSpeed;
        health = maxHealth;
        mana = maxMana;
        strength = gm.strength;
        souls = gm.souls;
        doubleJumpSkill = gm.canDoubleJump;
        dashSkill = gm.canDash;
        if(gm.currentArmorId > 0){
            AddArmor(Inventory.inventory.itemDataBase.GetArmor(gm.currentArmorId));
        }
        if(gm.currentWeaponId > 0){
            AddWeapon(Inventory.inventory.itemDataBase.GetWeapon(gm.currentWeaponId));
        }
    }

    public bool GetSkill(PlayerSkill skill){
        if(skill == PlayerSkill.dash){
            return dashSkill;
        }
        else if(skill == PlayerSkill.doubleJump){
            return doubleJumpSkill;
        }
        else{
            return false;
        }
    }
}
