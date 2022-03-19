using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private GameObject focalPoint;
    public bool hasPowerup;
    public GameObject powerupIndicator;
    //Сила удара
    private float powerupStrenght = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
        }
        StartCoroutine(PowerupCountdownRoutine());
        powerupIndicator.gameObject.SetActive(true);
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7f);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Если игрок с усилителем столкнулся с врагом 
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            //Получаем компонент rigidbody врага
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            //Высчитываем дистанцию, на которую отбросим врага
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            //Выводим сообщение в консоль
            Debug.Log("Столкновение с " + collision.gameObject.name + " с усилителем " + hasPowerup);
            //Добавляем вектор силы ускорения к врагу (отбрасываем его), который высчитывается как
            //вектор дистанции * силу удара
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrenght, ForceMode.Impulse);
        }
    }
}
