using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerData   //Json 저장 기본
{
    public float Volume_BackGround = 100.0f;        // 이 캐릭터의 배경 소리
    public float Volume_Effect = 100.0f;        // 이 캐릭터의 배경 소리

    public int qualityLevel = 2;                // 그래픽 설정

    public int[] maxScore_List = new int[10];
}

public class Main : MonoBehaviour     //Singleton<Main>
{



	private static Main instance;
    public PlayerData nowPlayer = new PlayerData();            // 윗 클래스 것들을 예시로 제이슨화할거임
    public string path;   // 저장할 거에 경로

    public static Main ins
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    [SerializeField] GameObject LoadingUI;         // 로딩 유아이... 자식에 설정되어 있다..

    CanvasGroup canvasGroup;                       // 로딩 유아이의 컴포넌트를 받는다.
    Image progressBar;                             // 로딩 바 이미지를 받는 변수
    string loadSceneName;         // 씬 이름

    int stageNum;

    WaitForSeconds delay_01;




    void Awake()
    {
        if (instance == null)             // 싱글톤 작업
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
		else
		{
			Destroy(gameObject);
		}


        // 게임을 처음 시작할 때는 /Save 폴더가 없을테니 아래처럼 만듬
        // 두번째부터는 /Save 파일이 있을테니 if 함수를 만듬
        if (!File.Exists(Application.persistentDataPath + "/Save"))      // 경로에 Save라는 폴더가 없다면
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Save"); // 경로에 Save 라는 폴더를 만들어라!
        }
        path = Application.persistentDataPath + "/Save/save";   // 경로는 Save폴더에 save + 로 만든다.

        LoadData();                       //  슬롯칸마다 json 파일 정보 업로드
        QualitySettings.SetQualityLevel(nowPlayer.qualityLevel);            // 처음 그래픽 설정
    }


	void Start()
    {
        if (LoadingUI != null)
        {
            canvasGroup = LoadingUI.GetComponent<CanvasGroup>();                          // 로딩 유아이의 컴포넌트를 받아온다.
            progressBar = LoadingUI.transform.GetChild(1).GetComponent<Image>();          // 로딩 유아이의 로딩바의 이미지를 받아온다.  //////////////////////////////////////////////
        }

        Application.targetFrameRate = 60;        // 타겟 프래임 60으로 고정

        delay_01 = new WaitForSeconds(1.0f);

        StartCoroutine(Chage());// 1초 마다 휴대폰 회전을 했는지 알아보는 코루틴 실행



	}


    IEnumerator Chage()       // 1초 마다 휴대폰 회전을 했는지 알아보는 코루틴
    {
        while (true)
        {

            if (Input.acceleration.y > 0.5f)                 // 모바일 회전
            {
                if (Screen.orientation.Equals(ScreenOrientation.LandscapeRight))
                {
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                }
                else
                {
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                }
            }
            yield return delay_01;   // 1초
        }
    }



    public void SaveData()             // 이 함수가 실행되면 저장됨(제이슨화해서 저장)
    {
        string data = JsonUtility.ToJson(nowPlayer);          // 현재 nowPlayer를 string으로 변환
        File.WriteAllText(path, data);   // 변환된 nowPlayer를 경로 + 슬롯번호로 저장, 슬롯번호는 이어하기에서 받아온 번호로 쭉 이어진다.
    }

    public void LoadData()             // 이 함수가 실행되면 저장된 것을 불러옴(제이슨 파일을 변환시켜서 옴)
    {
        if (File.Exists(Main.ins.path))    // 경로에 파일이 있는지 확인
        {
            string data = File.ReadAllText(path);   // 슬롯번호에 맞는 경로를 알려주고 json파일을 string파일로 변환
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);         // string 파일을 변환해서, 현재 nowPlayer변수에 넣는다.
        }
    }

    public void DataClear()       // 현재 nowPlayer에 저장한 json 파일이 있을 경우, 오류가 뜰 수 있어서 다른 것으로 바꿀 때는 클리어를 해주어야 함
    {
        nowPlayer = new PlayerData();   // 그리고 nowPlayer를 깨끗히 해둠
    }

    public void DataSlotCansel()      // 이어하기 할 때, 해당 슬롯을 지울 때 쓰는 함수임.
    {
        if (File.Exists(path))     // 우선, 해당 슬롯에 json 파일이 존재하는지 알아보고
        {
            File.Delete(path);     // 지워버림.
        }
    }





    public void LoadScene(string sceneName)            // 로딩하고 로딩 유아이를 키는 함수
    {
        LoadingUI.SetActive(true);                     // 로딩 유아이를 킨다.
        SceneManager.sceneLoaded += onSceneLoaded;     // 로딩 작업

        loadSceneName = sceneName;                     // 씬 이름을 받는다.
        StartCoroutine(LoadSceneProcess());            // 로딩 작업 코루틴 실행
        
    }

    AsyncOperation op;      // 비동기 작업

    IEnumerator LoadSceneProcess()           // 로딩 작업 코루틴
    {
        progressBar.fillAmount = 0.0f;                            // 로딩바 이미지 수치
        yield return StartCoroutine(Fade(true));                  // 로딩 페이드 작업?


        op = SceneManager.LoadSceneAsync(loadSceneName);          // 비동기로 씬을 불러온다.
        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);

                if (progressBar.fillAmount >= 1.0f)              // 로딩바 이미지가 1이 넘어가면 실행
                {
                    op.allowSceneActivation = true;              // 일단 실행

                    yield break;
                }
            }
        }
    }





    private void onSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
        if (arg0.name.Equals(loadSceneName))
        {
            StartCoroutine(Fade(false));                      // 로딩 유아이 끄기
            SceneManager.sceneLoaded -= onSceneLoaded;
        }

    }


    IEnumerator Fade(bool isFadeIn)             // 로딩 페이드 작업 코루틴
    {
        float timer = 0.0f;

        while (timer <= 1.0f)          // 기다리고
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3.0f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0.0f, 1.0f, timer) : Mathf.Lerp(1.0f, 0.0f, timer);
        }

        if (!isFadeIn)
        {
            LoadingUI.SetActive(false);         // 로딩 유아이를 끈낸다.
        }
    }





    public void PressButton(int num)             // 미니게임 플레이 버튼 함수          ///////////////////////////////////////////////////////////////////////////////////////////////////
    {
        //int randInt = Random.Range(0, 2);

        stageNum = num;


		if (num.Equals(10))                      // 미니게임 03으로 간다.
        {
			LoadScene("Mini_03");                // 위에 함수 있음
			return;
        }

        

        LoadScene("MiniGame");                   // 위에 함수 있음
    }



    public int MiniStageNum()               // 미니게임 씬에서 미니게임 관리 오브젝트에 넘길 함수
    {
        return stageNum;                    // 몇번 미니게임 프리팹을 가져와야 할지 알려준다.
    }
}
