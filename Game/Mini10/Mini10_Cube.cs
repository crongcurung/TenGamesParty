using System.Collections;
using UnityEngine;
using TMPro;

public class Mini10_Cube : MonoBehaviour         // 스테이지 안에 발판(스타트, 엔드 제외) 모두에 부착되어 있음
{
	[SerializeField] Mini10_End mini10_End;

	TextMeshPro textMesh;                        // 이 발판 위에 적힌 숫자를 받기 위한 변수
    int thisCount;                               // 발판 숫자를 조절하기 위한 변수

	Renderer render;                             // 여기 큐브 오브젝트 렌더링을 받음
	Rigidbody rigid;

	WaitForSeconds delay;                        // 코루틴 최적화

	Color color_0;
	Color color_1;
	Color color_2;
	Color color_3;
	Color color_4;
	Color color_5;
	Color color_6;
	Color color_7;
	Color color_8;
	Color color_9;


	void Start()
    {
		render = GetComponent<Renderer>();                                  // 큐브의 렌더를 받아옴
		textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();       // 이 발판 자식에 있는 글자 컴포넌트를 가져온다.
        thisCount = int.Parse(textMesh.text);       // 처음 설정된 숫자를 받아옴


		switch (thisCount)        // 처음 설정된 숫자에 따라 메테리얼을 받아옴
		{
			case 1:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				break;
			case 2:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				break;
			case 3:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				break;
			case 4:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				break;
			case 5:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				break;
			case 6:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				color_6 = new Color(158f / 255f, 186f / 255f, 232f / 255f);
				break;
			case 7:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				color_6 = new Color(158f / 255f, 186f / 255f, 232f / 255f);
				color_7 = new Color(110f / 255f, 165f / 255f, 183f / 255f);
				break;
			case 8:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				color_6 = new Color(158f / 255f, 186f / 255f, 232f / 255f);
				color_7 = new Color(110f / 255f, 165f / 255f, 183f / 255f);
				color_8 = new Color(60f / 255f, 195f / 255f, 123f / 255f);
				break;
			default:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				color_6 = new Color(158f / 255f, 186f / 255f, 232f / 255f);
				color_7 = new Color(110f / 255f, 165f / 255f, 183f / 255f);
				color_8 = new Color(60f / 255f, 195f / 255f, 123f / 255f);
				color_9 = new Color(45f / 255f, 140f / 255f, 89f / 255f);
				break;
		}

		color_0 = new Color(255f / 255f, 255f / 255f, 255f / 255f);
		rigid = GetComponent<Rigidbody>();
		Cube_Number(thisCount);          // 큐브 메테리얼 설정     

		delay = new WaitForSeconds(0.25f);             // 코루틴 최적화
	}


	public int StepPlayer()        // 이 발판을 밟았을 때, 숫자 조절하는 함수(public으로 둬서 플레이어에서 접근가능하도록 함)
    {
		if (thisCount.Equals(0))       // 이 큐브의 숫자가 0이면 끝냄
		{
			return 0;
		}


		thisCount--;       // 숫자를 줄인다.
		textMesh.text = thisCount.ToString();     // 형변환해서 글자로 보여줌

		Cube_Number(thisCount);     // 큐브 메테리얼 설정

		if (thisCount.Equals(0))                   // 줄어든 숫자가 0이라면..
		{
			mini10_End.fallCount++;                 // 큐브가 끝났다고 앤드에 알림

			StartCoroutine(FallingCoroutine());   // 이 발판을 깜빡이다가 떨구는 코루틴을 실행
        }

		return thisCount;      // 줄어든 숫자를 반환한다.
	}


	void Cube_Number(int num)          // 큐브 메테리얼 설정
	{
		switch (num)
		{
			case 0:
				render.material.color = color_0;
				break;
			case 1:
				render.material.color = color_1;
				break;
			case 2:
				render.material.color = color_2;
				break;
			case 3:
				render.material.color = color_3;
				break;
			case 4:
				render.material.color = color_4;
				break;
			case 5:
				render.material.color = color_5;
				break;
			case 6:
				render.material.color = color_6;
				break;
			case 7:
				render.material.color = color_7;
				break;
			case 8:
				render.material.color = color_8;
				break;
			case 9:
				render.material.color = color_9;
				break;
		}
	}


	void CubeFall(int randInt)                        // 떨어지는 발판 방향 정하는 함수
	{
		if (randInt.Equals(0))                        //  ----------------- 랜덤으로 뽑은 숫자로 떨어지는 방향을 정함
		{
			transform.rotation = Quaternion.Euler(20, 0, 20);
		}
		else if (randInt.Equals(1))
		{
			transform.rotation = Quaternion.Euler(-20, 0, 20);
		}
		else if (randInt.Equals(2))
		{
			transform.rotation = Quaternion.Euler(20, 0, -20);
		}
		else if (randInt.Equals(3))
		{
			transform.rotation = Quaternion.Euler(-20, 0, -20);
		}
	}

	/////////////////////////////////
	// 코루틴 구역...

	IEnumerator FallingCoroutine()        // 큐브가 0이라면 깜빡깜빡 거리는 코루틴
	{
		render.enabled = false;           // 깜빡이기...
		textMesh.enabled = false;
		yield return delay;               // 0.25초 마다...

		render.enabled = true;
		textMesh.enabled = true;
		yield return delay;

		render.enabled = false;
		textMesh.enabled = false;
		yield return delay;

		render.enabled = true;
		textMesh.enabled = true;
		yield return delay;

		rigid.isKinematic = false;              // 물리적인 힘을 받는다고 알린다.
		rigid.AddForce(Vector3.down * 500.0f);  // 밑으로 떨어트린다...
		gameObject.layer = 1;                  // 레이어를 바꿔서 플레이어에 적용안되도록 함

		int randInt = Random.Range(0, 4);       // 어느 쪽으로 떨어트릴지 랜덤 변수
		CubeFall(randInt);                      // 큐브 방향 정해침

		yield return new WaitForSeconds(3.0f);         // 3초 후, 비활성화

		transform.gameObject.SetActive(false);
	}
}
