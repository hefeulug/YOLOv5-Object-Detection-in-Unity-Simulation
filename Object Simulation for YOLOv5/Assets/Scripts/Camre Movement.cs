using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform orbitCenter;       // Kamera etrafında döneceği merkez
    public float orbitSpeed = 150f;      // Döngü hızı
    private float orbitRadius = 25f;    // Yarıçap
    private float height = 0f;          // Yükseklik
    private float angle = 270f;         // Başlangıç açısı (saat 6 yönü)

    void Start()
    {
        // Kamerayı başlangıç pozisyonuna yerleştir
        float x = orbitCenter.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
        float z = orbitCenter.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;
        float y = orbitCenter.position.y + height;

        transform.position = new Vector3(x, y, z);
        transform.LookAt(orbitCenter);
    }

    void Update()
    {
       OrbitMovement();
    }

    void OrbitMovement()
    {
        // Kamera sürekli saat yönünde döner
        angle -= orbitSpeed * Time.deltaTime;

        float x = orbitCenter.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
        float z = orbitCenter.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;
        float y = orbitCenter.position.y + height;

        transform.position = new Vector3(x, y, z);
        transform.LookAt(orbitCenter);
        transform.Rotate(Vector3.forward, 180f);
    }
}
