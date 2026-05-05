using UnityEngine;
using UnityEngine.InputSystem;
public class CanonShoot : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject round;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform cannonBarrel;
    [SerializeField] private float maxShootingRange = 15f;
    [SerializeField] private float minShootingRange = 5f;
    [SerializeField] private float shootForce = 20f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private AudioClip shootingFX;
    
    private float nextFireTime = 0f;
 

    void Update()
    {



        // make the canon face the player
        Vector3 direction = player.position - firePoint.position;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        //  Adjust cannon barrel rotation (up/down)
        Vector3 target = player.position + Vector3.up * 1.0f;
        Vector3 dir = target - cannonBarrel.position;
        Vector3 localDir = transform.InverseTransformDirection(dir);

        float angleX = -Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;
        angleX = Mathf.Clamp(angleX, -1f, 10f);

        Quaternion targetRot = Quaternion.Euler(angleX, 0f, 0f);

        cannonBarrel.localRotation = Quaternion.Slerp(
            cannonBarrel.localRotation,
            targetRot,
            Time.deltaTime * 5f
        );
        
        // fire at the player
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            float distance = Vector3.Distance(transform.position, player.position);

            // If player withing shooting range
            if(distance <= maxShootingRange && distance >= minShootingRange)
            {
                Shoot();
            }
        }
    }


    void Shoot()
    {

        // Spawn visual effect
        float offset = 0.5f; // how far in front of the barrel
        Vector3 effectPosition = firePoint.position + firePoint.forward * offset;
        GameObject effect = Instantiate(fireEffect, effectPosition, Quaternion.identity);
        //effect.transform.localScale = Vector3.one * 0.5f;
        Destroy(effect, 2f);

        // Play sound
        SoundFXManager.Instance.PlaySoundFXClip_1Time(shootingFX, transform, 0.5f);   

        GameObject bullet = Instantiate(round, firePoint.position, Quaternion.identity);
        Vector3 direction = ((player.position + Vector3.up * 1.0f) - firePoint.position).normalized;
        if (bullet.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(firePoint.forward * shootForce, ForceMode.Impulse);
            //rb.linearVelocity = direction * shootForce * Time.deltaTime;
        }
        else
        {
            Debug.LogWarning("Bullet prefab has no Rigidbody!");
        }
    }

 
}
