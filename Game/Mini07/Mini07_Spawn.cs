using System.Collections.Generic;
using UnityEngine;

public class Mini07_Spawn : MonoBehaviour
{
    [SerializeField] GameObject Ground;

    [SerializeField] GameObject rail_S;
    [SerializeField] GameObject rail_L;
    [SerializeField] GameObject rail_R;

    [SerializeField] GameObject obj_1;
    [SerializeField] GameObject obj_2;
    [SerializeField] GameObject obj_3;
    [SerializeField] GameObject obj_4;

    [SerializeField] GameObject coin;

    [SerializeField] Transform playerTrans;
    Mini07_Player mini07_Player;

    GameObject prefab;          // 프리팹 매니져에서 오브젝트를 받아올 변수

    List<int> intList_Object = new List<int>();         // 랜덤 중복 방지를 위한 리스트
    List<int> intList_Coin = new List<int>();         // 랜덤 중복 방지를 위한 리스트


    Queue<GameObject> queue_Ground = new Queue<GameObject>();     // 바닥 오브젝트 풀링

    Queue<GameObject> queue_Straight = new Queue<GameObject>();     // 직선 레일 오브젝트 풀링
    Queue<GameObject> queue_Left = new Queue<GameObject>();        // 왼쪽 레일 오브젝트 풀링
    Queue<GameObject> queue_Right = new Queue<GameObject>();           // 오른쪽 레일 오브젝트 풀링


    Queue<GameObject> queue_StopSign = new Queue<GameObject>();     // 1칸 장애물 오브젝트 풀링
    Queue<GameObject> queue_Ban = new Queue<GameObject>();          // 2칸 장애물 오브젝트 풀링
    Queue<GameObject> queue_Container = new Queue<GameObject>();    // 3칸 장애물 오브젝트 풀링
    Queue<GameObject> queue_Sandwich = new Queue<GameObject>();     // 4칸 장애물 오브젝트 풀링

    Queue<GameObject> queue_Coin = new Queue<GameObject>();     // 코인 오브젝트 풀링

    void Awake()
    {
        mini07_Player = playerTrans.GetComponent<Mini07_Player>();

        GameObject p_object;

        prefab = Ground;               // 바닥 오브젝트..
        for (int i = 0; i < 4; i++)       // 뚫어뻥 20정도 생성 및 보관
        {
            p_object = Instantiate(prefab);  // 뚫어뻥 생성
            p_object.GetComponent<Mini07_Area>().mini07_Spawn = this;
            p_object.GetComponent<Mini07_Area>().playerTrans = playerTrans;
            queue_Ground.Enqueue(p_object);       // 오브젝트 풀링
        }




        prefab = rail_S;               // 직선 레일 오브젝트..
        for (int i = 0; i < 10; i++)       
        {
            p_object = Instantiate(prefab);  
            p_object.GetComponent<Mini07_Rail>().mini07_Spawn = this;
            queue_Straight.Enqueue(p_object);       
        }

        prefab = rail_L;               // 왼쪽 레일 오브젝트..
        for (int i = 0; i < 10; i++)       
        {
            p_object = Instantiate(prefab); 
            p_object.GetComponent<Mini07_Rail>().mini07_Spawn = this;
            queue_Left.Enqueue(p_object);       
        }

        prefab = rail_R;               // 오른쪽 레일 오브젝트..
        for (int i = 0; i < 10; i++)       
        {
            p_object = Instantiate(prefab);  
            p_object.GetComponent<Mini07_Rail>().mini07_Spawn = this;
            queue_Right.Enqueue(p_object);      
        }




        prefab = obj_1;               // 1칸 오브젝트..
        for (int i = 0; i < 20; i++)       
        {
            p_object = Instantiate(prefab);  
            prefab.GetComponent<Mini07_Obj>().mini07_Spawn = this;
            queue_StopSign.Enqueue(p_object);       
        }

        prefab = obj_2;               // 2칸 오브젝트..
        for (int i = 0; i < 20; i++)       
        {
            p_object = Instantiate(prefab);  
            prefab.GetComponent<Mini07_Obj>().mini07_Spawn = this;
            queue_Ban.Enqueue(p_object);       
        }

        prefab = obj_3;               // 3칸 오브젝트..
        for (int i = 0; i < 20; i++)       
        {
            p_object = Instantiate(prefab);  
            prefab.GetComponent<Mini07_Obj>().mini07_Spawn = this;
            queue_Container.Enqueue(p_object);       
        }

        prefab = obj_4;               // 4칸 오브젝트..
        for (int i = 0; i < 20; i++)      
        {
            p_object = Instantiate(prefab);
            prefab.GetComponent<Mini07_Obj>().mini07_Spawn = this;
            queue_Sandwich.Enqueue(p_object);     
        }




        prefab = coin;               // 코인 오브젝트..
        for (int i = 0; i < 20; i++)       
        {
            p_object = Instantiate(prefab);  
            p_object.GetComponent<Mini07_Coin>().mini07_Spawn = this;
            queue_Coin.Enqueue(p_object);    
        }
    }


	void Start()
	{
		for (int i = 0; i < 3; i++)
		{
            SpawnArea();                     // 바닥 스폰
        }
    }




	public void InsertQueue_Ground(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(바닥)
    {
        queue_Ground.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Ground()           // 큐에서 객체를 빌려오는 함수(바닥)
    {
        GameObject t_object = queue_Ground.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }








    public void InsertQueue_Straight(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(직선 레일)
    {
        queue_Straight.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Straight()           // 큐에서 객체를 빌려오는 함수(직선 레일)
    {
        GameObject t_object = queue_Straight.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    public void InsertQueue_Left(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(왼쪽 레일)
    {
        queue_Left.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Left()           // 큐에서 객체를 빌려오는 함수(왼쪽 레일)
    {
        GameObject t_object = queue_Left.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }




    public void InsertQueue_Right(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(오른쪽 레일)
    {
        queue_Right.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Right()           // 큐에서 객체를 빌려오는 함수(오른쪽 레일)
    {
        GameObject t_object = queue_Right.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }






    public void InsertQueue_StopSign(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(1칸 장애물)
    {
        queue_StopSign.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_StopSign()           // 큐에서 객체를 빌려오는 함수(1칸 장애물)
    {
        GameObject t_object = queue_StopSign.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    public void InsertQueue_Ban(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(2칸 장애물)
    {
        queue_Ban.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Ban()           // 큐에서 객체를 빌려오는 함수(2칸 장애물)
    {
        GameObject t_object = queue_Ban.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    public void InsertQueue_Container(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(3칸 장애물)
    {
        queue_Container.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Container()           // 큐에서 객체를 빌려오는 함수(3칸 장애물)
    {
        GameObject t_object = queue_Container.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    public void InsertQueue_Sandwich(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(4칸 장애물)
    {
        queue_Sandwich.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Sandwich()           // 큐에서 객체를 빌려오는 함수(4칸 장애물)
    {
        GameObject t_object = queue_Sandwich.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }







    public void InsertQueue_Coin(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(코인)
    {
        queue_Coin.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Coin()           // 큐에서 객체를 빌려오는 함수(코인)
    {
        GameObject t_object = queue_Coin.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    ////////////////////////   바닥 스폰
    int areaIndex = 0;   // 바닥에 부여될 번호

    public void SpawnArea()                          // 바닥을 스폰한다...
    {
        float zDistance = 150;                       // 바닥 사이의 거리(바닥 중앙끼리...)
        float zDistance_Default = 150;                // Z 값 조정 값(처음에 0부터 시작해야해서..)  

        GameObject clone = null;        // 바닥 프리펩을 받을 변수

        clone = GetQueue_Ground();        // 바닥을 비활성화된 부모의 첫 번째 자식을 담고

        clone.transform.position = new Vector3(0, 0, areaIndex * zDistance + zDistance_Default);    // 바닥의 위치를 마지막 바닥 다음으로 옮김    

        RandomObject(clone.transform, areaIndex);    // 이 바닥에 붙일 장애물 정하기
        clone.SetActive(true);                   // 활성화 상태로 바꿈

        areaIndex++;      // 스폰할때마다 번호를 올린다.
    }



    public void RandomObject(Transform clone, int AreaNum)  // 바닥에 부여된 번호에 따라 난이도 조절하는 함수  (프리팹의 부모, 몇번쨰 바닥)
    {
        if (AreaNum < 4)            // 바닥 번호가 4 미만이면.. (디폴트까지 5개)
        {
            InsertObject(clone, 2);   // 종류는 2개 
        }
        else if (AreaNum < 9)       // 바닥 번호가 9 미만이면.. (디폴트까지 10개)
        {
            InsertObject(clone, 3);   // 종류는 3개
        }
        else                        // 바닥 번호가 9 이상이면..
        {
            InsertObject(clone, 4);   // 종류는 4개
        }


        if (AreaNum >= 12 && (AreaNum % 2).Equals(0))     // 바닥 번호가 12 이상이고, 짝수이면.... (보통 이러면 1100M에서 스피드 상승)
        {
            mini07_Player.SpeedUp();


        }
    }



	void InsertObject(Transform clone, int objectCount)  // 장애물을 꺼내오고 위치 정하는 함수 (부모 위치, 총 몇개를 나타낼 것인지, 종류는 몇개로?)
    {
        for (int i = 0; i < 5; i++)                 // 각 바닥에 각각 5개의 포스가 있어서..
        {
            intList_Object.Add(i + 3);              // 오브젝트 포스에 맞는 번호가 들어감
            intList_Coin.Add(i + 8);                // 코인 포스에 맞는 번호가 들어감
        }


        Mini07_Area mini07_Area = clone.GetComponent<Mini07_Area>();


        for (int i = 0; i < 5; i++)              // 총 몇개를 돌릴까?
        {
            int randInt_Object = Random.Range(0, objectCount);         // 오브젝트 종류
            int randInt_X = Random.Range(-2, 3);                        // 코인의 X값을 받는 랜덤 변수
            int randInt_Z = Random.Range(0, intList_Object.Count);     // 바닥 위에 위치 정하기
            int randRotate = Random.Range(-1, 2);            // 오브젝트의 회전을 하는 랜덤 변수

            GameObject prefabs_Object = GetQueue_Coin();     // 코인 프리펩을 오브젝트 풀링에서 가져오고
            Vector3 insertPos = Vector3.zero;      // 코인의 위치를 받는 변수

            insertPos = new Vector3(randInt_X * 3, 2, clone.GetChild(intList_Coin[randInt_Z]).transform.position.z);
            // 코인의 위치 조정
            prefabs_Object.transform.position = insertPos;       // 코인의 위치 조정
            //prefabs_Object.transform.parent = clone.GetChild(intList_Coin[randInt_Z]);


            mini07_Area.area_Action += prefabs_Object.GetComponent<Mini07_Coin>().End_Area;



            intList_Coin.RemoveAt(randInt_Z);                 // 사용한 숫자 제거

            randInt_Z = Random.Range(0, intList_Coin.Count);        // 코인의 Z값을 받는 랜덤 변수
            switch (randInt_Object)
            {
                case 0:
                    prefabs_Object = GetQueue_StopSign();   // 큐에서 가져옴

                    randInt_X = Random.Range(-2, 3);
                    insertPos = new Vector3(randInt_X * 3, 0, clone.GetChild(intList_Object[randInt_Z]).transform.position.z);         // 바닥에서의 장애물 위치 조정

                    break;
                case 1:
                    prefabs_Object = GetQueue_Ban();   // 큐에서 가져옴

                    prefabs_Object.transform.rotation = Quaternion.Euler(new Vector3(-90, randRotate * 45.0f, 180));      // 회전값 조정
                    randInt_X = Random.Range(-1, 3);         // -1, 0, 1, 2
                    insertPos = new Vector3((randInt_X * 3 - 1.5f), 0, clone.GetChild(intList_Object[randInt_Z]).transform.position.z);         // 바닥에서의 장애물 위치 조정

                    break;
                case 2:
                    prefabs_Object = GetQueue_Container();   // 큐에서 가져옴

                    prefabs_Object.transform.rotation = Quaternion.Euler(new Vector3(-90, randRotate * 45.0f, 0));      // 회전값 조정
                    randInt_X = Random.Range(-1, 2);
                    insertPos = new Vector3(randInt_X * 3, 0, clone.GetChild(intList_Object[randInt_Z]).transform.position.z);         // 바닥에서의 장애물 위치 조정

                    break;
                default:
                    prefabs_Object = GetQueue_Sandwich();   // 큐에서 가져옴

                    prefabs_Object.transform.rotation = Quaternion.Euler(new Vector3(-90, randRotate * 45.0f, 0));      // 회전값 조정

                    randInt_X = Random.Range(-1, 1);
                    insertPos = new Vector3((randInt_X * 3 + 1.5f), 0, clone.GetChild(intList_Object[randInt_Z]).transform.position.z);
                    // 위치 값 조정

                    break;
            }
            prefabs_Object.transform.position = insertPos;                  // 위에 정해진 랜덤 위치로 옮긴다.
            //prefabs_Object.transform.parent = clone.GetChild(intList_Object[randInt_Z]);


            mini07_Area.area_Action += prefabs_Object.GetComponent<Mini07_Obj>().End_Area;


            intList_Object.RemoveAt(randInt_Z);                 // 사용한 숫자 제거
        }

        intList_Coin.Clear();           // 랜덤 중복 방지(코인) 리스트 초기화
        intList_Object.Clear();         // 랜덤 중복 방지 리스트 초기화
    }
}
