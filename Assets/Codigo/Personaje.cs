using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    private int movimientoHorizontal = 0;
    private int movimientoVertical = 0;
    private Vector2 mov = Vector2.zero;

    [SerializeField] private float speedInicial = 10f;
    [SerializeField] private float speedRapida = 20f;
    private float speedActual;

    private Rigidbody2D rb;
    private Animator animator;
    private bool giroIzq = false;
    private bool recibiendoDanio = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speedActual = speedInicial;
    }

    void Update()
    {
        EnX();
        EnY();
        Sprint();

        // Activar animación de movimiento si hay velocidad en cualquier dirección
        animator.SetFloat("xVelocity", rb.velocity.sqrMagnitude > 0 ? 1 : 0);

        Ataque();
        FlipSprite();
    }

    void FixedUpdate()
{
    if (!recibiendoDanio)
    {
        mov = new Vector2(movimientoHorizontal, movimientoVertical).normalized;
        rb.velocity = mov * speedActual;
    }
}


    private void EnX()
    {
        movimientoHorizontal = Input.GetKey(KeyCode.D) ? 1 :
                               Input.GetKey(KeyCode.A) ? -1 : 0;
    }

    private void EnY()
    {
        movimientoVertical = Input.GetKey(KeyCode.W) ? 1 :
                             Input.GetKey(KeyCode.S) ? -1 : 0;
    }

    private void Sprint()
    {
        speedActual = Input.GetKey(KeyCode.M) ? speedRapida : speedInicial;
    }

    private void Ataque()
    {
        if (Input.GetKeyDown(KeyCode.K))
            animator.SetTrigger("attack");
    }

    private void FlipSprite()
    {
        if (rb.velocity.x < 0 && !giroIzq || rb.velocity.x > 0 && giroIzq)
        {
            giroIzq = !giroIzq;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    public void recibeDanio(Vector2 direccion, int cantDanio)
{
    Debug.Log("Método recibeDanio() fue llamado con dirección: " + direccion);

    if (!recibiendoDanio)
    {
        recibiendoDanio = true;

        Vector2 rebote = ((Vector2)transform.position - direccion).normalized;

        Debug.Log("Vector de rebote: " + rebote);
        Debug.DrawRay(transform.position, rebote * 2, Color.red, 1f);

        rb.velocity = Vector2.zero;
        rb.AddForce(rebote * 10f, ForceMode2D.Impulse);

        animator.SetTrigger("recibedanio");

        Invoke(nameof(desactivardanio), 0.3f);
    }
}


    private void desactivardanio()
    {
        recibiendoDanio = false;
        //animator.SetBool("recibedanio", false);
        Debug.Log("Daño desactivado");
    }
}






