using UnityEngine;

public class CollectableKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.UpdateKeysLeft();
        Destroy(this.gameObject);
    }
}
