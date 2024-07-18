using UnityEngine;

public class CCTVController : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform
    public Transform cctvHead;  // CCTV의 Head Transform
    public Light spotLight;  // CCTV의 Spot Light
    public float detectionRange = 10f;  // 인식 거리
    public float fieldOfView = 60f;  // 시야각
    public float rotationSpeed = 30f;  // CCTV 회전 속도
    public LayerMask detectionLayer;  // 탐지할 레이어 마스크
    public LayerMask wallLayer;  // 벽 레이어 마스크
    public float leftLimit = -90f;  // 좌측 회전 한계
    public float rightLimit = 90f;  // 우측 회전 한계

    private bool playerDetected = false;  // 플레이어 감지 여부
    private bool rotatingRight = true;  // 현재 회전 방향
    
    private float detectionTimer = 0f;  // 플레이어 감지 타이머
    private float detectionResetTime = 5f;  // 플레이어 감지 초기화 시간

    void Start()
    {
        // Spot Light 설정
        if (spotLight != null)
        {
            spotLight.type = LightType.Spot;
            spotLight.range = detectionRange;
            spotLight.spotAngle = fieldOfView;
            spotLight.transform.localRotation = Quaternion.Euler(60f, 0f, 0f);  // X축 회전을 60도로 설정
        }
    }

    void Update()
    {
        if (playerDetected)
        {
            // 플레이어를 바라보도록 Head 부분 회전
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
            // 탐색을 위한 회전
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

            // 레이캐스트를 사용하여 플레이어 탐지
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
