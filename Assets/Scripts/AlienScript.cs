using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public float bulletSpeed;
    public float shootingDelay;
    public float lastTimeShot = 0f;

    public GameObject player;
    public GameObject bullet;

    public GameObject explosion;
    public SpriteRenderer sr;
    public Collider2D col;
    private bool disabled;

    public int points;

    public float timeBeforeSpawning;
    private float levelStartTime;
    public GameObject startPosition;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        levelStartTime = Time.time;
        timeBeforeSpawning = Random.Range(5f, 20f);
        Disable();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (disabled)
        {
            //respawn
            if (Time.time > levelStartTime + timeBeforeSpawning)
            {
                Enable();
            }
            return;
        }
        if (Time.time > lastTimeShot+ shootingDelay)
        {   //расчет направления выстрела         
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            //Выстрел
            GameObject newBullet = Instantiate(bullet, transform.position, q);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, bulletSpeed));
            lastTimeShot = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (disabled)
        {
            return;
        }
        //движение в сторону игрока
        direction = (player.transform.position- transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            //передать игроку значение полученных очков
            player.SendMessage("ScorePoints", points);
            //эффект взрыва 
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 2.0f);
            //отключение  
            Disable();
        }
    }

    void Enable()
    {
        //Переместить в точку старта
        transform.position = startPosition.transform.position;
        //Активировать
        col.enabled = true;
        sr.enabled = true;
        disabled = false;
    }
    void Disable()
    {   //Отключить отображение противника     
        col.enabled = false;
        sr.enabled = false;
        //неактивен
        disabled = true;
        //обновить значение переменной
        levelStartTime = Time.time;

    }
}
