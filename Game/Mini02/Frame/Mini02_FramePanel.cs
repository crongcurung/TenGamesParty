using UnityEngine;

public class Mini02_FramePanel : MonoBehaviour     // 틀 패널에 부착됨
{
    [SerializeField] Mini02_Player mini02_Player;

    [SerializeField] GameObject page01Panel;    // 페이지01 패널
    [SerializeField] GameObject page02Panel;    // 페이지02 패널


    public bool isFrameInput = false;           // 틀에 넣었는지 알리는 변수
    public bool isHoleOrStar = false;           // 구멍 혹은 스타를 클릭했는지 알리는 변수

    void OnEnable()      // 시작할 때..
    {
        page01Panel.SetActive(true);                               // 틀 패널에 들어갈때는 페이지01부터 시작하도록! 
    }

	void OnDisable()
	{
        page02Panel.SetActive(false);         // 페이지02 비활성화
    }


    public void Press_HoleButton()        // 구멍 틀 이미지에 등록
    {
        if (isFrameInput.Equals(false))   // 드래그 도넛 이미지가 도마에 닿지 않았는가?
        {
            return;                               // 그럼 리턴
        }

        AudioMng.ins.PlayEffect("Click03");      // 도넛 드래그 시작

        isHoleOrStar = false;

        page01Panel.SetActive(false);            // 페이지02로 감
        page02Panel.SetActive(true);
    }


    public void Press_StarButton()           // 별 틀 이미지에 등록
    {
        if (isFrameInput.Equals(false))   // 드래그 도넛 이미지가 도마에 닿지 않았는가?
        {
            return;                                  // 그럼 리턴
        }

        AudioMng.ins.PlayEffect("Click03");      // 도넛 드래그 시작

        isHoleOrStar = true;

        page01Panel.SetActive(false);         // 페이지02로 감
        page02Panel.SetActive(true);
    }

    public void EndButton()         // 완전 메인으로 가는 함수
    {
        mini02_Player.Origin_Panel();                // 패널을 비활성화 하는 함수
        transform.gameObject.SetActive(false);       // 이 패널을 비활성화
    }
}
