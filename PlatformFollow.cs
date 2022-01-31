using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformFollow : MonoBehaviour
{
    public float speed;
    public float platformXRange;
    public float platformYRange;


    private Vector3 playerDistance;
    private Transform target;
    private Animator anim;

    private int damage = 500;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = target.transform.position - transform.position;
        if(Mathf.Abs(playerDistance.x) < platformXRange && Mathf.Abs(playerDistance.y) < platformYRange){
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Player target = other.gameObject.GetComponent<Player>();
        if(target != null && target.deadCheck == false){
            target.TakeDamage(damage);
            Invoke("ReloadScene", 2f);
        }
    }
}