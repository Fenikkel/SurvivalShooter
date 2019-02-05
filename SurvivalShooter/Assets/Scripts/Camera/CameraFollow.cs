using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothing = 5f;

    Vector3 offset; //la distancia inicial entre el jugador y la camera

    void Start()
    {
        offset = transform.position - target.position;
    }

    //FixedUpdate es para las fisicas, pero como movemos el jugador en fixedUpdate tambien lo haremos en la camera para que se mueva acorde
    private void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }


}
