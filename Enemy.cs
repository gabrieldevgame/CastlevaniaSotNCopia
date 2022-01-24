using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float enemyXRange;
    public float enemyYRange;
    public int health;
    public GameObject itemDrop;
    public ConsumableItem item;
    public int damage;
    public int souls;
    [SerializeField]
    private Vector2 damageForce;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 playerDistance;
    private bool facingRight = false;
    private bool isDead = false;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDead){
            playerDistance = player.transform.position - transform.position;
            if(Mathf.Abs(playerDistance.x) < enemyXRange && Mathf.Abs(playerDistance.y) < enemyYRange){
                rb.velocity = new Vector2(speed * (playerDistance.x / Mathf.Abs(playerDistance.x)), rb.velocity.y);
            }

            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            if(rb.velocity.x > 0.5f && !facingRight){
                Flip();
            }
            else if(rb.velocity.x < 0.5f && facingRight){
                Flip();
            }
        }
    }

    private void Flip(){
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            anim.SetTrigger("Dead");
            isDead = true;
            rb.velocity = Vector2.zero;
            FindObjectOfType<Player>().souls += souls;
            FindObjectOfType<UIManager>().UpdateUI();
            if(item != null){
                GameObject tempItem = Instantiate(itemDrop, transform.position, transform.rotation);
                tempItem.GetComponent<ItemDrop>().item = item;
            }
        }
        else{
            StartCoroutine(DamageCoroutine());
        }
    }

    IEnumerator DamageCoroutine(){
        for (float i = 0; i < 0.2f; i+= 0.2f)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DestroyEnemy(){
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null && player.deadCheck == false){
            player.TakeDamage(damage);
            Vector2 newDamageForce = new Vector2(damageForce.x * (playerDistance.x / Mathf.Abs(playerDistance.x)), damageForce.y);
            player.GetComponent<Rigidbody2D>().AddForce(newDamageForce, ForceMode2D.Impulse);
        }
        else if(player != null && player.deadCheck == true){
            damageForce.x = 0;
            damageForce.y = 0;
            enemyXRange = 0;
        }
    }
}