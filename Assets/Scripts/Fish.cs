using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Rigidbody2D _rb; //private değişken tanımlarken önüne alt tire koyarız.
    public float speed;
    int angle;
    readonly int maxAngle=20;
    readonly int minAngle = -60;
    public Score score;
    public GameManager gameManager;
    public ObstacleSpawner obstacleSpawner;
    public Sprite fishDied;
    SpriteRenderer _sp;
    Animator anim;
    [SerializeField] private AudioSource swim, hit, point;

    bool touchedGround;
    void Start()
    {        
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0; // oyun başladığında yerçekimine uğramasın.
        _sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FishSwim();

    }
    private void FixedUpdate()
    {

        FishRotation(); // açısal işlemler için fixed update kullanarak daha güzel bir görünüm sağlayayabiliriz.
    }
    void FishSwim()
    {
        if (Input.GetMouseButtonDown(0)&& GameManager.gameOver==false) //mobil cihazlar içinde dokunma işlemini tanımlar.
        {
            swim.Play();
            if (GameManager.gameStarted==false)
            {
                _rb.gravityScale = 1f;
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, speed);
                obstacleSpawner.InstantiateObstacle();
                gameManager.GameHasStarted();
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, speed);
            }            
        }
    }
    void FishRotation()
    {
        if (_rb.velocity.y > 0)
        {
            if (angle <= maxAngle)
            {
                angle = angle + 4;
            }

        }
        else if (_rb.velocity.y < -2.5f)
        {
            if (angle > minAngle)
            {
                angle = angle - 2;
            }
        }
        if (touchedGround==false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle); // objenin yönünü ayarlamak için kullanırız.
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            score.Scored();
            point.Play();
        }
        else if (collision.CompareTag("Column") && GameManager.gameOver == false)
        {
            gameManager.GameOver();
            FishDiedEfect();
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (GameManager.gameOver==false)
            {
                gameManager.GameOver();
                this.GameOver();
            }
            else
            {
                
            }
        }
    }
    void FishDiedEfect()
    {
        hit.Play();
    }
    void GameOver()
    {
        touchedGround = true;
        _sp.sprite = fishDied;
        anim.enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        
    }
}
