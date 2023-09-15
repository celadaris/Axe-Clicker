using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField] ParticleSystem pe;
    [SerializeField] GameObject scoreGO;
    Score score;

    private void Start()
    {
        score = scoreGO.GetComponent<Score>();
    }

    async void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Casts the ray and get the first game object hit
            Physics.Raycast(ray, out hit);
            if (!Pause.gamePaused)
            {
                if (hit.collider != null)
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        Vector3 dir = (hit.point - ray.origin).normalized;
                        Vector3 particlePos = ray.origin + dir * 1.25f;
                        pe.transform.position = particlePos;
                        pe.Play();
                        gameObject.SetActive(false);
                        AxePool.axePool.Enqueue(gameObject);
                        //need to update score
                        await score.AddScore();
                    }
                }
            }

        }
    }
}