using UnityEngine;

public class MenuClick : MonoBehaviour
{
    [SerializeField] ParticleSystem menuParticles;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Casts the ray and get the first game object hit
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject == gameObject)
                {
                    Vector3 dir = (hit.point - ray.origin).normalized;
                    Vector3 particlePos = ray.origin + dir * 3f;
                    menuParticles.transform.position = particlePos;
                    menuParticles.Play();
                }
            }
        }
    }
}