using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GunAmmo"))
        {
            GameManager.Instance.gunAmmo += other.gameObject.GetComponent<AmmoBox>().ammo;
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("HealthBox"))
        {
            GameManager.Instance.playerHealth += other.gameObject.GetComponent<HealthBox>().health;
            Destroy(other.gameObject);
        }
    }
}
