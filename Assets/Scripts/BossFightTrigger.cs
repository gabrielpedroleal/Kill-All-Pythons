using System;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]

public class BossFightTrigger : MonoBehaviour
{
    public event Action OnPlayerEnteredBossFight;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            OnPlayerEnteredBossFight?.Invoke();
            Destroy(gameObject);
        }
    }
}
