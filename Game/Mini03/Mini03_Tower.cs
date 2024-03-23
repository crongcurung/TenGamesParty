using UnityEngine;

public class Mini03_Tower : MonoBehaviour
{
    Transform patrol;          // 현재 스테이지에 패트롤 오브젝트를 받는 변수

    bool patrolBool = false;     // 패트롤 최적화 변수

    [SerializeField] float speed = 7.0f;          // 타워 회전 스피드

       
    Vector3 randPos;

    void Start()
    {
        patrol = transform.parent.GetChild(4).transform;     // 현재 
    }

    void Update()
    {
        if (patrolBool.Equals(false))       // 타워 불빛이 아직 패트롤 상태가 아닐 경우
        {
            patrolBool = true;          // 패트롤 상태로 바꿈
            
            int randInt = randInt = Random.Range(0, patrol.childCount);   // 다음 패트롤 경로로!
            randPos = patrol.GetChild(randInt).position;
        }

        Vector3 dir = randPos - this.transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
        // 다음 경로로 이동

        float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));

        if (angle <= 1.0f)       // 경로가 가까이 갔다면...
        {
            patrolBool = false;      // 현재 패트롤이 끝났다고 알림
        }
    }
}
