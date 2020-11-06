using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public float maxThrust;
    public float maxTorque;
    public Rigidbody2D rb;

    public float screenTop;
    public float screenRight;

    public int asteroidSize;
    public GameObject asteroidMedium;
    public GameObject asteroidSmall;

    public int points;
    public GameObject player;

    public GameObject explosion;

    public GameManagerScript gm;

    // Start is called before the first frame update
    void Start()
    {
        //движение астероида
        Vector2 thrust = new Vector2(Random.Range(-maxThrust, maxThrust), Random.Range(-maxThrust, maxThrust));
        float torque = Random.Range(-maxTorque, maxTorque);
        rb.AddForce(thrust);
        rb.AddTorque(torque);
        //Найти игрока
        player = GameObject.FindWithTag("Player");
        //Найти gm
        gm = GameObject.FindObjectOfType<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //действия при пересечении границы экрана
        Vector2 newPos = transform.position;
        if (transform.position.y > screenTop)
        {
            newPos.y = -screenTop;
        }
        if (transform.position.y < -screenTop)
        {
            newPos.y = screenTop;
        }
        if (transform.position.x > screenRight)
        {
            newPos.x = -screenRight;
        }
        if (transform.position.x < -screenRight)
        {
            newPos.x = screenRight;
        }
        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //выстрел в астероид
        //создание двух астероидов меньшего размера, если маленький - уничтожить
        if (collision.CompareTag("bullet"))
        {
            Destroy(collision.gameObject);
            if (asteroidSize == 3)
            {
                Instantiate(asteroidMedium, transform.position, transform.rotation);
                Instantiate(asteroidMedium, transform.position, transform.rotation);
                gm.UpdateNumberOfAsteroids(1);
            }
            else if (asteroidSize == 2)
            {
                Instantiate(asteroidSmall, transform.position, transform.rotation);
                Instantiate(asteroidSmall, transform.position, transform.rotation);
                gm.UpdateNumberOfAsteroids(1);
            }
            else if (asteroidSize == 1)
            {
                gm.UpdateNumberOfAsteroids(-1);
            }
            //передача значения очков
            player.SendMessage("ScorePoints", points);
            //эффект взрыва
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 2.0f);
            Destroy(gameObject);
        }
        
    }
}
