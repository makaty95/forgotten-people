using UnityEditor.AssetImporters;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
 

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.Instance.IncrementScoreBy(1);
        }
    }
}
