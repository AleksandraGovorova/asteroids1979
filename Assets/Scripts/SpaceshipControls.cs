using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceshipControls : MonoBehaviour
{
    public Rigidbody2D rb;
    public float thrust;
    public float turnThrust;
    private float thrustInput;
    private float turnInput;
    public float screenTop;
    public float screenRight;

    public GameObject bullet;
    public float bulletForce;

    public float deathForce;

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;

    public AudioSource audio;

    public Color inColor;
    public Color normalColor;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }

    // Update is called once per frame
    void Update()
    {
        //Проверка нажатия клавиш для движения
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        //Проверка нажатия клавиши выстрела
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(newBullet, 5.0f);
        }
        //Поворот спрайта игрока
        transform.Rotate(Vector3.forward * -turnInput * Time.deltaTime * turnThrust);
        //Пересение границ экрана - перемещение игрока
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

    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * thrustInput * thrust);
    }

    void ScorePoints(int pointsToAdd)
    {
        //подсчет очков
        score += pointsToAdd;
        scoreText.text = "Score: " + score;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //столковение с астероидом -жизнь
        if (collision.relativeVelocity.magnitude > deathForce)
        {
            LoseLife();
        }
    }

    void Respawn()
    {
        //восстановление после потери жизни
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = inColor;
        //3 секунды до снятия неуязвимости
        Invoke("Vulnerable", 3f);
    }

    void Vulnerable()
    {
        //уязвимость
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = normalColor;
    }

    void GameOver()
    {       
        CancelInvoke();
        gameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        //перезапуск
        SceneManager.LoadScene("SampleScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //потеря жизни при попадании выстрела пришельца
        if (collision.CompareTag("alienBullet"))
        {
            LoseLife();
        }
    }

    void LoseLife()
    {
        //потеря жизни
        audio.Play();
        lives--;
        livesText.text = "Lives: " + lives;
        //respawn
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Invoke("Respawn", 3f);
        if (lives <= 0)
        {
            //GameOver
            GameOver();
        }
    }
}
