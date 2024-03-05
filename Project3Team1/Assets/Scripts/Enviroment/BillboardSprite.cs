using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.LookAt(cam.transform.position);
    }
}
