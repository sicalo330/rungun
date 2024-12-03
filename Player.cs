using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public static Player obj;
    public float gravity = 9.5f;
    public bool isGrounded = false;
    public float jumpForce = 5.0f;
    public float moveSpeed = 5.0f;
    public float movHor;
    public bool isMoving = false;
    public float radius = 0.3f;
    public float groundRayDist = 0.5f;
    public LayerMask groundLayer;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    private int lastDirection = 1; // 1 para derecha, -1 para izquierda

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        movHor = Input.GetAxisRaw("Horizontal");
        isMoving = (movHor != 0f);

        // Mover al personaje
        rb.velocity = new Vector2(movHor * moveSpeed, rb.velocity.y);

        // Actualizar dirección según el movimiento
        if (movHor != 0) {
            lastDirection = movHor > 0 ? 1 : -1;
            Debug.Log(lastDirection);
        }

        // Voltear el personaje
        flip(movHor);

        // Saltar
        if(isGrounded && Input.GetKeyDown(KeyCode.Space)){
            rb.velocity = Vector2.up * jumpForce;
        }

        // Disparar
        if(Input.GetKeyDown(KeyCode.Z)){
            Shoot();
        }

        // Actualizar animaciones
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
    }

    void Shoot(){
        // Usar `lastDirection` para orientar la bala correctamente
        Quaternion bulletRotation = Quaternion.Euler(0, 0, lastDirection == -1 ? 180 : 0);
        Instantiate(bulletPrefab, firingPoint.position, bulletRotation);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground")){
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    void flip(float _xValue){
        Vector3 theScale = transform.localScale;

        if(_xValue < 0){
            theScale.x = Mathf.Abs(theScale.x) * -1;
            transform.localScale = theScale;
        }
        else if(_xValue > 0){
            theScale.x = Mathf.Abs(theScale.x);
            transform.localScale = theScale;
        }
    }
}
