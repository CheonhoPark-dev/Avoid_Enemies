using UnityEngine;

public class CCTVController : MonoBehaviour
{
    public Transform player;  // �÷��̾��� Transform
    public Transform cctvHead;  // CCTV�� Head Transform
    public Light spotLight;  // CCTV�� Spot Light
    public float detectionRange = 10f;  // �ν� �Ÿ�
    public float fieldOfView = 60f;  // �þ߰�
    public float rotationSpeed = 30f;  // CCTV ȸ�� �ӵ�
    public LayerMask detectionLayer;  // Ž���� ���̾� ����ũ
    public LayerMask wallLayer;  // �� ���̾� ����ũ
    public float leftLimit = -90f;  // ���� ȸ�� �Ѱ�
    public float rightLimit = 90f;  // ���� ȸ�� �Ѱ�

    private bool playerDetected = false;  // �÷��̾� ���� ����
    private bool rotatingRight = true;  // ���� ȸ�� ����
    
    private float detectionTimer = 0f;  // �÷��̾� ���� Ÿ�̸�
    private float detectionResetTime = 5f;  // �÷��̾� ���� �ʱ�ȭ �ð�

    void Start()
    {
        // Spot Light ����
        if (spotLight != null)
        {
            spotLight.type = LightType.Spot;
            spotLight.range = detectionRange;
            spotLight.spotAngle = fieldOfView;
            spotLight.transform.localRotation = Quaternion.Euler(60f, 0f, 0f);  // X�� ȸ���� 60���� ����
        }
    }

    void Update()
    {
        if (playerDetected)
        {
            // �÷��̾ �ٶ󺸵��� Head �κ� ȸ��
            Vector3 direction = player.position - cctvHead.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            cctvHead.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            if (spotLight != null)
            {
                spotLight.transform.rotation = Quaternion.Euler(60f, cctvHead.rotation.eulerAngles.y, 0f);
            }

            detectionTimer += Time.deltaTime;
            if (detectionTimer >= detectionResetTime)
            {
                playerDetected = false;
                detectionTimer = 0f;
            }
        }
        else
        {
            // Ž���� ���� ȸ��
            if (rotatingRight)
            {
                cctvHead.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                if (cctvHead.localEulerAngles.y >= rightLimit && cctvHead.localEulerAngles.y < 180f)
                {
                    rotatingRight = false;
                }
            }
            else
            {
                cctvHead.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
                if (cctvHead.localEulerAngles.y <= leftLimit || cctvHead.localEulerAngles.y > 180f)
                {
                    rotatingRight = true;
                }
            }

            if (spotLight != null)
            {
                spotLight.transform.rotation = Quaternion.Euler(60f, cctvHead.rotation.eulerAngles.y, 0f);
            }

            // ����ĳ��Ʈ�� ����Ͽ� �÷��̾� Ž��
            DetectPlayer();
        }
    }

    void DetectPlayer()
    {
        RaycastHit hit;
        Vector3 rayDirection = cctvHead.forward;

        for (float angle = -fieldOfView / 2; angle <= fieldOfView / 2; angle += fieldOfView / 10)
        {
            Vector3 rayDirectionRotated = Quaternion.Euler(60f, angle, 0) * rayDirection;
            if (Physics.Raycast(cctvHead.position, rayDirectionRotated, out hit, detectionRange, detectionLayer | wallLayer))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    playerDetected = false;
                    detectionTimer = 0f;
                    return;
                }
                if (hit.transform == player)
                {
                    playerDetected = true;
                    return;
                }
            }
        }

        playerDetected = false;
    }

    void OnDrawGizmos()
    {
        if (cctvHead == null) return;

        Gizmos.color = Color.red;
        Vector3 rayDirection = cctvHead.forward;

        for (float angle = -fieldOfView / 2; angle <= fieldOfView / 2; angle += fieldOfView / 10)
        {
            Vector3 rayDirectionRotated = Quaternion.Euler(60f, angle, 0) * rayDirection;
            Gizmos.DrawRay(cctvHead.position, rayDirectionRotated * detectionRange);
        }
    }
}
