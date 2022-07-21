using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGain : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
            Destroy(gameObject);
            FindObjectOfType<GameSession>().IncreaseLife();
        }
    }
}
