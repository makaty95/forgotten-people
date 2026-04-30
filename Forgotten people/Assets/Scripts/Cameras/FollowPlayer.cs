using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 offset = new Vector3(0, 7, -7);
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        
    }
}
