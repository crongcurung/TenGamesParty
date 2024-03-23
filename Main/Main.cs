using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerData   //Json ���� �⺻
{
    public float Volume_BackGround = 100.0f;        // �� ĳ������ ��� �Ҹ�
    public float Volume_Effect = 100.0f;        // �� ĳ������ ��� �Ҹ�

    public int qualityLevel = 2;                // �׷��� ����

    public int[] maxScore_List = new int[10];
}

public class Main : MonoBehaviour     //Singleton<Main>
{



	private static Main instance;
    public PlayerData nowPlayer = new PlayerData();            // �� Ŭ���� �͵��� ���÷� ���̽�ȭ�Ұ���
    public string path;   // ������ �ſ� ���

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

    [SerializeField] GameObject LoadingUI;         // �ε� ������... �ڽĿ� �����Ǿ� �ִ�..

    CanvasGroup canvasGroup;                       // �ε� �������� ������Ʈ�� �޴´�.
    Image progressBar;                             // �ε� �� �̹����� �޴� ����
    string loadSceneName;         // �� �̸�

    int stageNum;

    WaitForSeconds delay_01;




    void Awake()
    {
        if (instance == null)             // �̱��� �۾�
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
		else
		{
			Destroy(gameObject);
		}


        // ������ ó�� ������ ���� /Save ������ �����״� �Ʒ�ó�� ����
        // �ι�°���ʹ� /Save ������ �����״� if �Լ��� ����
        if (!File.Exists(Application.persistentDataPath + "/Save"))      // ��ο� Save��� ������ ���ٸ�
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Save"); // ��ο� Save ��� ������ ������!
        }
        path = Application.persistentDataPath + "/Save/save";   // ��δ� Save������ save + �� �����.

        LoadData();                       //  ����ĭ���� json ���� ���� ���ε�
        QualitySettings.SetQualityLevel(nowPlayer.qualityLevel);            // ó�� �׷��� ����
    }


	void Start()
    {
        if (LoadingUI != null)
        {
            canvasGroup = LoadingUI.GetComponent<CanvasGroup>();                          // �ε� �������� ������Ʈ�� �޾ƿ´�.
            progressBar = LoadingUI.transform.GetChild(1).GetComponent<Image>();          // �ε� �������� �ε����� �̹����� �޾ƿ´�.  //////////////////////////////////////////////
        }

        Application.targetFrameRate = 60;        // Ÿ�� ������ 60���� ����

        delay_01 = new WaitForSeconds(1.0f);

        StartCoroutine(Chage());// 1�� ���� �޴��� ȸ���� �ߴ��� �˾ƺ��� �ڷ�ƾ ����



	}


    IEnumerator Chage()       // 1�� ���� �޴��� ȸ���� �ߴ��� �˾ƺ��� �ڷ�ƾ
    {
        while (true)
        {

            if (Input.acceleration.y > 0.5f)                 // ����� ȸ��
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
            yield return delay_01;   // 1��
        }
    }



    public void SaveData()             // �� �Լ��� ����Ǹ� �����(���̽�ȭ�ؼ� ����)
    {
        string data = JsonUtility.ToJson(nowPlayer);          // ���� nowPlayer�� string���� ��ȯ
        File.WriteAllText(path, data);   // ��ȯ�� nowPlayer�� ��� + ���Թ�ȣ�� ����, ���Թ�ȣ�� �̾��ϱ⿡�� �޾ƿ� ��ȣ�� �� �̾�����.
    }

    public void LoadData()             // �� �Լ��� ����Ǹ� ����� ���� �ҷ���(���̽� ������ ��ȯ���Ѽ� ��)
    {
        if (File.Exists(Main.ins.path))    // ��ο� ������ �ִ��� Ȯ��
        {
            string data = File.ReadAllText(path);   // ���Թ�ȣ�� �´� ��θ� �˷��ְ� json������ string���Ϸ� ��ȯ
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);         // string ������ ��ȯ�ؼ�, ���� nowPlayer������ �ִ´�.
        }
    }

    public void DataClear()       // ���� nowPlayer�� ������ json ������ ���� ���, ������ �� �� �־ �ٸ� ������ �ٲ� ���� Ŭ��� ���־�� ��
    {
        nowPlayer = new PlayerData();   // �׸��� nowPlayer�� ������ �ص�
    }

    public void DataSlotCansel()      // �̾��ϱ� �� ��, �ش� ������ ���� �� ���� �Լ���.
    {
        if (File.Exists(path))     // �켱, �ش� ���Կ� json ������ �����ϴ��� �˾ƺ���
        {
            File.Delete(path);     // ��������.
        }
    }





    public void LoadScene(string sceneName)            // �ε��ϰ� �ε� �����̸� Ű�� �Լ�
    {
        LoadingUI.SetActive(true);                     // �ε� �����̸� Ų��.
        SceneManager.sceneLoaded += onSceneLoaded;     // �ε� �۾�

        loadSceneName = sceneName;                     // �� �̸��� �޴´�.
        StartCoroutine(LoadSceneProcess());            // �ε� �۾� �ڷ�ƾ ����
        
    }

    AsyncOperation op;      // �񵿱� �۾�

    IEnumerator LoadSceneProcess()           // �ε� �۾� �ڷ�ƾ
    {
        progressBar.fillAmount = 0.0f;                            // �ε��� �̹��� ��ġ
        yield return StartCoroutine(Fade(true));                  // �ε� ���̵� �۾�?


        op = SceneManager.LoadSceneAsync(loadSceneName);          // �񵿱�� ���� �ҷ��´�.
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

                if (progressBar.fillAmount >= 1.0f)              // �ε��� �̹����� 1�� �Ѿ�� ����
                {
                    op.allowSceneActivation = true;              // �ϴ� ����

                    yield break;
                }
            }
        }
    }





    private void onSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
        if (arg0.name.Equals(loadSceneName))
        {
            StartCoroutine(Fade(false));                      // �ε� ������ ����
            SceneManager.sceneLoaded -= onSceneLoaded;
        }

    }


    IEnumerator Fade(bool isFadeIn)             // �ε� ���̵� �۾� �ڷ�ƾ
    {
        float timer = 0.0f;

        while (timer <= 1.0f)          // ��ٸ���
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3.0f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0.0f, 1.0f, timer) : Mathf.Lerp(1.0f, 0.0f, timer);
        }

        if (!isFadeIn)
        {
            LoadingUI.SetActive(false);         // �ε� �����̸� ������.
        }
    }





    public void PressButton(int num)             // �̴ϰ��� �÷��� ��ư �Լ�          ///////////////////////////////////////////////////////////////////////////////////////////////////
    {
        //int randInt = Random.Range(0, 2);

        stageNum = num;


		if (num.Equals(10))                      // �̴ϰ��� 03���� ����.
        {
			LoadScene("Mini_03");                // ���� �Լ� ����
			return;
        }

        

        LoadScene("MiniGame");                   // ���� �Լ� ����
    }



    public int MiniStageNum()               // �̴ϰ��� ������ �̴ϰ��� ���� ������Ʈ�� �ѱ� �Լ�
    {
        return stageNum;                    // ��� �̴ϰ��� �������� �����;� ���� �˷��ش�.
    }
}
