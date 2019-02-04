using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;//layerMask is stored as an integer
    float camRayLength = 100f;

    private void Awake() //Es como el start pero se hace aunque el script este desactivado
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    //el Update normal es para el rendering
    private void FixedUpdate() //se usa para las fisicas (el movimiento del jugador usando Rigidbody se controla desde aqui)
    {
        //GetAxisRaw --> More resposive feel
        //GetAxis --> Accelerating
        float hor = Input.GetAxisRaw("Horizontal"); //Raw te da un valor fijo (-1, 0 ,1). Sin Raw te da un valor entre -1 y 1
        float ver = Input.GetAxisRaw("Vertical");

        Move(hor, ver);
        Turning();
        Animating(hor, ver);
    }

    void Move (float h, float v)
    {
        movement.Set(h, 0f, v);

        //lo normalizamos para que el tamaño de los vectores sea unitario? asi si vas en diagonal no tendras ventaja
        //con normalize el tamaño siempre es 1, hagas la combinacion de teclas que hagas
        //si fuera solo el movimiento por la velocidad el jugador se moveria 6 unidades por iteracion que hagamos FixedUpdate (SERIA DEMASIADO RAPIDO) moveria el jugador 6 unidades cada 1segundo/50
        //delta time es el tiempo que pasa entre cada llamada(iteracion del update)
        //asi es como hacemos que el jugador se mueva 6 unidades por segundo
        movement = movement.normalized * speed * Time.deltaTime;

        //MovePosition mueve el jugador a una posicion del world space
        //movemos al jugador sumandole a su posicion hacia donde queremos que se mueva
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        //Camera.main.ScreenPointToRay --> Con esto creamos un rayo desde la pantalla hacia adelante
        //Input.mousePosition --> desde la posicion del raton
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;
        //Physics.Raycast es una funcion que devuelve true o false segun si ha golpeado o no. Ademas solo hace sus cosas si golpea
        //Comprueba el rayo de camRay
        //out guarda en la variable lo que ha calculado (the information back)
        //camRayLength es la distancia que recorre el rayo
        //Solor hara la funcion (y devolvera true) si golpeo objetos con el layer floorMask
        if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
        {
            //Vector de el jugador --> hacia --> donde ha golpeado el rayo
            //floorHit.point --> donde ha golpeado el rayo
            //transform.position --> donde esta el jugador
            Vector3 playerToMouseHit = floorHit.point - transform.position;
            playerToMouseHit.y = 0f; //no queremos que se mueva en y

            Quaternion newRotation = Quaternion.LookRotation(playerToMouseHit);// mira la rotacion de esa variable
            playerRigidbody.MoveRotation(newRotation);

        }



    }

    void Animating(float h, float v)
    {
        bool walking = (h != 0f || v != 0f);
        anim.SetBool("IsWalking", walking);
    }
}
