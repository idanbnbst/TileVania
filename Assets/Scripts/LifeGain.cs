using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGain : MonoBehaviour
{
    [SerializeField] AudioClip gainLifeSFX;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(gainLifeSFX, Camera.main.transform.position);
            Destroy(gameObject);
            FindObjectOfType<GameSession>().IncreaseLife();
        }
    }
}
