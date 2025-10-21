using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
     private Rigidbody2D rb2D;
        [Header("Movimiento")]
        private float movimientoHorizontal = 0f;
        [SerializeField] private float velMovi;
        [SerializeField] private float suavizadoMov;
        private Vector3 velocidad = Vector3.zero;
        private bool mirandoDerecha = true;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;
    private bool salto = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //Se actualiza cada que pulse algo el update
   private void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal")*velMovi;
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
       {
            salto = true;
        }
    }
    private void FixedUpdate()
    {
        //mover
        Mover(movimientoHorizontal * Time.fixedDeltaTime);
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        Jump(salto);
        salto = false;
    }


    private void Mover(float mover)
    {
        Vector3 velocidadObjetivo = new Vector2(mover,rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoMov);
        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover<0 && mirandoDerecha)
        {
            Girar();
        }

    }        
    

    private void Girar()
        {
            mirandoDerecha = !mirandoDerecha;
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;

        }
    public void Jump(bool saltar)
    {
        if(enSuelo && saltar)
        {
            enSuelo = false;
            rb2D.AddForce(new Vector2(0f, JumpForce));
        }
    }
    //Metodo para ver las dimensiones de la caja
    private void OnDrawGizmos()
    {
        //Gizmos nos permite ver lineas y dimensiones, creamos un cubo en lineas
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}
