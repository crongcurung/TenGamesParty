using UnityEngine;
using UnityEngine.UI;

public class Mini02_ToppingPanel : MonoBehaviour       // 토핑 패널에 부착됨
{
    [SerializeField] Material[] Mat_Array;

    [SerializeField] Mini02_Player mini02_Player;      // 플레이어 스크립트

    [SerializeField] Button redButton;                 // 딸기 토핑 이미지 버튼
    [SerializeField] Button chocoButton;               // 초코 토핑 이미지 버튼

    [SerializeField] Button mainButton;               // 토핑 패널에 있는 메인 버튼

    [SerializeField] GameObject bic_Circle;            //  회전 서클 오브젝트

    bool isHoleOrStar = true;                // true면 구멍 도넛(익은거), false면 스타 도넛(익은거)

    [SerializeField] GameObject One_Donut;            // 토핑이 뿌려질 구멍 도넛 오브젝트
    [SerializeField] GameObject Star_Donut;           // 토핑이 뿌려질 스타 도넛 오브젝트

    [SerializeField] GameObject Bottle;               // 토핑통 오브젝트
    [SerializeField] Renderer Bottle_Render;          // 토핑통 렌더링(메테리얼 받을거)

    Material strow_Mat;              // 딸기 토핑통 메테리얼
    Material choco_Mat;              // 초코 토핑통 메테리얼
    Material out_Mat;      // 배열때문에 어쩔수 없이 가져옴(토핑통 오브젝트)

    public bool isPinkOrChoco = false;

    void Awake()
	{
        strow_Mat = Mat_Array[0];
        choco_Mat = Mat_Array[1];
        out_Mat = Mat_Array[2];
    }

	void OnEnable()         // 켜질 때...
    {
        redButton.gameObject.SetActive(true);        // 딸기 버튼 활성화
        chocoButton.gameObject.SetActive(true);      // 초코 버튼 활성화

        isHoleOrStar = mini02_Player.isHoleOrStar;
        mainButton.interactable = false;    // 이걸 왜 하지?
    }

    void OnDisable()        // 끝나면...
    {
        bic_Circle.SetActive(false);          // 토핑 패널을 나갈 경우 서클을 끈다.

        redButton.interactable = true;        // 이걸 왜 하지?
        chocoButton.interactable = true;       // 이걸 왜 하지?

        One_Donut.SetActive(false);     // 도넛 오브젝트 비활성화
        Star_Donut.SetActive(false);

        Bottle.SetActive(false);        // 토핑통 오브젝트 비활성화
    }

    public void Press_RedButton()               // 토핑 패널에 있는 '딸기 도넛' 버튼을 눌렀을 경우
    {
        isPinkOrChoco = false;      // 딸기 토핑
        bic_Circle.SetActive(true);             // 회전 서클을 킨다.

        AudioMng.ins.PlayEffect("Click03");      // 도넛 드래그 시작

        if (isHoleOrStar.Equals(false))
        {
            One_Donut.SetActive(true);     // 구멍 도넛 오브젝트 활성화
        }
        else
        {
            Star_Donut.SetActive(true);    // 스타 도넛 오브젝트 활성화
        }
        Bottle.SetActive(true);  // 토핑통 활성화
        Bottle_Render.materials = new Material[2] { out_Mat, strow_Mat };   // 딸기 토핑통 메테리얼로 교체

        redButton.interactable = false;
        chocoButton.interactable = false;        // 두 버튼 모두 못 누르게 함

        mainButton.interactable = true;

        redButton.gameObject.SetActive(false);     // 버튼 둘다 비활성화
        chocoButton.gameObject.SetActive(false);
    }

    public void Press_ChocoButton()              // 토핑 패널에 있는 '초코 도넛' 버튼을 눌렀을 경우
    {
        isPinkOrChoco = true;       // 초코 토핑
        bic_Circle.SetActive(true);              // 회전 서클을 킨다.

        AudioMng.ins.PlayEffect("Click03");      // 도넛 드래그 시작

        if (isHoleOrStar.Equals(false))
        {
            One_Donut.SetActive(true);     // 구멍 도넛 활성화
        }
        else
        {
            Star_Donut.SetActive(true);    // 스타 도넛 활성화
        }

        Bottle.SetActive(true);  // 신작
        Bottle_Render.materials = new Material[2] { out_Mat, choco_Mat };   // 초코 토핑통 메테리얼로 교체

        redButton.interactable = false;
        chocoButton.interactable = false;        // 두 버튼 모두 못 누르게 함

        mainButton.interactable = true;

        redButton.gameObject.SetActive(false);     // 버튼 둘다 비활성화
        chocoButton.gameObject.SetActive(false);
    }



    public void Press_BackButton()         // 토핑 패널에 있는 빽버튼을 눌렀을 경우
    {
        gameObject.SetActive(false);     // 현재 토핑 패널을 끈다.
        mini02_Player.Origin_Panel();
    }
}
