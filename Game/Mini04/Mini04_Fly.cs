using UnityEngine;

public class Mini04_Fly : MonoBehaviour      // 날아다니는 오브젝트에 부착됨
{
    public Vector3 Distance;
    public Vector3 MovementFrequency;
    Vector3 Moveposition;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        Moveposition.x = startPosition.x + Mathf.Sin(Time.timeSinceLevelLoad * 3.0f * MovementFrequency.x) * Distance.x;
        Moveposition.y = startPosition.y + Mathf.Sin(Time.timeSinceLevelLoad * 3.0f * MovementFrequency.y) * Distance.y;
        Moveposition.z = startPosition.z + Mathf.Sin(Time.timeSinceLevelLoad * 3.0f * MovementFrequency.z) * Distance.z;
        transform.position = new Vector3(Moveposition.x, Moveposition.y, Moveposition.z);
    }
}
