using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Vector2 startPosition;
    private float startZ;
    private Transform cameraTransform;
    private Vector2 initialCameraPos;

    [SerializeField] private float parallaxEffectMultiplier = 0.5f; // Valore tra 0 (fermo) e 1 (muove tanto)

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        startPosition = transform.position;
        startZ = transform.position.z;
        initialCameraPos = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector2 cameraTravel = (Vector2)cameraTransform.position - initialCameraPos;
        //Vector2 newPos = startPosition + cameraTravel * parallaxEffectMultiplier;
        Vector2 newPos = startPosition + new Vector2(cameraTravel.x * parallaxEffectMultiplier, 0);
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
