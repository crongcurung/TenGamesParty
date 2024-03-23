using UnityEngine;

public class MiniGame : MonoBehaviour            // 미니게임 씬에 처음 오브젝트 부착되어 있음
{
	[SerializeField] GameObject[] Stage;         // 미니게임 프리팹들을 담는다.

	void Awake()
	{
		Time.timeScale = 1;                      // 한 번더 하는 경우 스케일이 0일 수도 있어서 여기서 1로 한다.

		int num = Main.ins.MiniStageNum();       // 몇번쨰 미니게임을 가지고 와야하는지 담는다.

		GameObject prefab = Stage[num];          // 미니게임 프리팹을 담는다.

		Instantiate(prefab, Vector3.zero, Quaternion.identity);    // 미니게임 프리팹을 깔아놓는다.
	}

}
