using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    void Update()
    {
        /*        float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
                transform.Translate(movement * speed * Time.deltaTime, Space.World);*/
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
    }

    public void DetectedByEnemy(Transform enemy)
    {
        Debug.Log("플레이어가 적에게 감지되었습니다: " + enemy.name);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었습니다.");
    }
}
