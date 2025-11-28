using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioSource shootAudio;
    public AudioClip impactSound;
    public float impactVolume = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (impactSound != null)
            {
                // Crear un objeto temporal para el sonido
                GameObject audioObj = new GameObject("ImpactSound");
                audioObj.transform.position = transform.position;
                
                // Configurar el AudioSource
                AudioSource source = audioObj.AddComponent<AudioSource>();
                source.spatialBlend = 1f; // 3D
                source.minDistance = 2f; // Aumentar esto ayuda a que se escuche más fuerte cerca
                
                // Reproducir con el volumen deseado (PlayOneShot a veces permite > 1 dependiendo de la versión/config)
                source.PlayOneShot(impactSound, impactVolume);
                
                // Destruir el objeto cuando termine el sonido
                Destroy(audioObj, impactSound.length + 0.1f);

                Debug.Log("Bullet hit Enemy. Playing impact sound at: " + transform.position + " with volume: " + impactVolume);
            }
            else
            {
                Debug.LogWarning("Bullet hit Enemy but impactSound is NULL.");
            }
            //shootAudio.Play();
            Destroy(collision.gameObject);
        }
    }
}
