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

    GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����

    List<int> intList_Object = new List<int>();         // ���� �ߺ� ������ ���� ����Ʈ
    List<int> intList_Coin = new List<int>();         // ���� �ߺ� ������ ���� ����Ʈ


    Queue<GameObject> queue_Ground = new Queue<GameObject>();     // �ٴ� ������Ʈ Ǯ��

    Queue<GameObject> queue_Straight = new Queue<GameObject>();     // ���� ���� ������Ʈ Ǯ��
    Queue<GameObject> queue_Left = new Queue<GameObject>();        // ���� ���� ������Ʈ Ǯ��
    Queue<GameObject> queue_Right = new Queue<GameObject>();           // ������ ���� ������Ʈ Ǯ��


    Queue<GameObject> queue_StopSign = new Queue<GameObject>();     // 1ĭ ��ֹ� ������Ʈ Ǯ��
    Queue<GameObject> queue_Ban = new Queue<GameObject>();          // 2ĭ ��ֹ� ������Ʈ Ǯ��
    Queue<GameObject> queue_Container = new Queue<GameObject>();    // 3ĭ ��ֹ� ������Ʈ Ǯ��
    Queue<GameObject> queue_Sandwich = new Queue<GameObject>();     // 4ĭ ��ֹ� ������Ʈ Ǯ��

    Queue<GameObject> queue_Coin = new Queue<GameObject>();     // ���� ������Ʈ Ǯ��

    void Awake()
    {
        mini07_Player = playerTrans.GetComponent<Mini07_Player>();

        GameObject p_object;

        prefab = Ground;               // �ٴ� ������Ʈ..
        for (int i = 0; i < 4; i++)       // �վ 20���� ���� �� ����
        {
            p_object = Instantiate(prefab);  // �վ ����
            p_object.GetComponent<Mini07_Area>().mini07_Spawn = this;
            p_object.GetComponent<Mini07_Area>().playerTrans = playerTrans;
            queue_Ground.Enqueue(p_object);       // ������Ʈ Ǯ��
        }




        prefab = rail_S;               // ���� ���� ������Ʈ..
        for (int i = 0; i < 10; i++)       
        {
            p_object = Instantiate(prefab);  
            p_object.GetComponent<Mini07_Rail>().mini07_Spawn = this;
            queue_Straight.Enqueue(p_object);       
        }

        prefab = rail_L;               // ���� ���� ������Ʈ..
        for (int i = 0; i < 10; i++)       
        {
            p_object = Instantiate(prefab); 
            p_object.GetComponent<Mini07_Rail>().mini07_Spawn = this;
            queue_Left.Enqueue(p_object);       
        }

        prefab = rail_R;               // ������ ���� ������Ʈ..
        for (int i = 0; i < 10; i++)       
        {
            p_object = Instantiate(prefab);  
            p_object.GetComponent<Mini07_Rail>().mini07_Spawn = this;
            queue_Right.Enqueue(p_object);      
        }




        prefab = obj_1;               // 1ĭ ������Ʈ..
        for (int i = 0; i < 20; i++)       
        {
            p_object = Instantiate(prefab);  
            prefab.GetComponent<Mini07_Obj>().mini07_Spawn = this;
            queue_StopSign.Enqueue(p_object);       
        }

        prefab = obj_2;               // 2ĭ ������Ʈ..
        for (int i = 0; i < 20; i++)       
        {
            p_object = Instantiate(prefab);  
            prefab.GetComponent<Mini07_Obj>().mini07_Spawn = this;
            queue_Ban.Enqueue(p_object);       
        }

        prefab = obj_3;               // 3ĭ ������Ʈ..
        for (int i = 0; i < 20; i++)       
        {
            p_object = Instantiate(prefab);  
            prefab.GetComponent<Mini07_Obj>().mini07_Spawn = this;
            queue_Container.Enqueue(p_object);       
        }

        prefab = obj_4;               // 4ĭ ������Ʈ..
        for (int i = 0; i < 20; i++)      
        {
            p_object = Instantiate(prefab);
            prefab.GetComponent<Mini07_Obj>().mini07_Spawn = this;
            queue_Sandwich.Enqueue(p_object);     
        }




        prefab = coin;               // ���� ������Ʈ..
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
            SpawnArea();                     // �ٴ� ����
        }
    }




	public void InsertQueue_Ground(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(�ٴ�)
    {
        queue_Ground.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Ground()           // ť���� ��ü�� �������� �Լ�(�ٴ�)
    {
        GameObject t_object = queue_Ground.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }








    public void InsertQueue_Straight(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(���� ����)
    {
        queue_Straight.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Straight()           // ť���� ��ü�� �������� �Լ�(���� ����)
    {
        GameObject t_object = queue_Straight.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    public void InsertQueue_Left(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(���� ����)
    {
        queue_Left.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Left()           // ť���� ��ü�� �������� �Լ�(���� ����)
    {
        GameObject t_object = queue_Left.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }




    public void InsertQueue_Right(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(������ ����)
    {
        queue_Right.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Right()           // ť���� ��ü�� �������� �Լ�(������ ����)
    {
        GameObject t_object = queue_Right.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }






    public void InsertQueue_StopSign(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(1ĭ ��ֹ�)
    {
        queue_StopSign.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_StopSign()           // ť���� ��ü�� �������� �Լ�(1ĭ ��ֹ�)
    {
        GameObject t_object = queue_StopSign.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    public void InsertQueue_Ban(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(2ĭ ��ֹ�)
    {
        queue_Ban.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Ban()           // ť���� ��ü�� �������� �Լ�(2ĭ ��ֹ�)
    {
        GameObject t_object = queue_Ban.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    public void InsertQueue_Container(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(3ĭ ��ֹ�)
    {
        queue_Container.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Container()           // ť���� ��ü�� �������� �Լ�(3ĭ ��ֹ�)
    {
        GameObject t_object = queue_Container.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    public void InsertQueue_Sandwich(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(4ĭ ��ֹ�)
    {
        queue_Sandwich.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Sandwich()           // ť���� ��ü�� �������� �Լ�(4ĭ ��ֹ�)
    {
        GameObject t_object = queue_Sandwich.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }







    public void InsertQueue_Coin(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(����)
    {
        queue_Coin.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Coin()           // ť���� ��ü�� �������� �Լ�(����)
    {
        GameObject t_object = queue_Coin.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    ////////////////////////   �ٴ� ����
    int areaIndex = 0;   // �ٴڿ� �ο��� ��ȣ

    public void SpawnArea()                          // �ٴ��� �����Ѵ�...
    {
        float zDistance = 150;                       // �ٴ� ������ �Ÿ�(�ٴ� �߾ӳ���...)
        float zDistance_Default = 150;                // Z �� ���� ��(ó���� 0���� �����ؾ��ؼ�..)  

        GameObject clone = null;        // �ٴ� �������� ���� ����

        clone = GetQueue_Ground();        // �ٴ��� ��Ȱ��ȭ�� �θ��� ù ��° �ڽ��� ���

        clone.transform.position = new Vector3(0, 0, areaIndex * zDistance + zDistance_Default);    // �ٴ��� ��ġ�� ������ �ٴ� �������� �ű�    

        RandomObject(clone.transform, areaIndex);    // �� �ٴڿ� ���� ��ֹ� ���ϱ�
        clone.SetActive(true);                   // Ȱ��ȭ ���·� �ٲ�

        areaIndex++;      // �����Ҷ����� ��ȣ�� �ø���.
    }



    public void RandomObject(Transform clone, int AreaNum)  // �ٴڿ� �ο��� ��ȣ�� ���� ���̵� �����ϴ� �Լ�  (�������� �θ�, ����� �ٴ�)
    {
        if (AreaNum < 4)            // �ٴ� ��ȣ�� 4 �̸��̸�.. (����Ʈ���� 5��)
        {
            InsertObject(clone, 2);   // ������ 2�� 
        }
        else if (AreaNum < 9)       // �ٴ� ��ȣ�� 9 �̸��̸�.. (����Ʈ���� 10��)
        {
            InsertObject(clone, 3);   // ������ 3��
        }
        else                        // �ٴ� ��ȣ�� 9 �̻��̸�..
        {
            InsertObject(clone, 4);   // ������ 4��
        }


        if (AreaNum >= 12 && (AreaNum % 2).Equals(0))     // �ٴ� ��ȣ�� 12 �̻��̰�, ¦���̸�.... (���� �̷��� 1100M���� ���ǵ� ���)
        {
            mini07_Player.SpeedUp();


        }
    }



	void InsertObject(Transform clone, int objectCount)  // ��ֹ��� �������� ��ġ ���ϴ� �Լ� (�θ� ��ġ, �� ��� ��Ÿ�� ������, ������ ���?)
    {
        for (int i = 0; i < 5; i++)                 // �� �ٴڿ� ���� 5���� ������ �־..
        {
            intList_Object.Add(i + 3);              // ������Ʈ ������ �´� ��ȣ�� ��
            intList_Coin.Add(i + 8);                // ���� ������ �´� ��ȣ�� ��
        }


        Mini07_Area mini07_Area = clone.GetComponent<Mini07_Area>();


        for (int i = 0; i < 5; i++)              // �� ��� ������?
        {
            int randInt_Object = Random.Range(0, objectCount);         // ������Ʈ ����
            int randInt_X = Random.Range(-2, 3);                        // ������ X���� �޴� ���� ����
            int randInt_Z = Random.Range(0, intList_Object.Count);     // �ٴ� ���� ��ġ ���ϱ�
            int randRotate = Random.Range(-1, 2);            // ������Ʈ�� ȸ���� �ϴ� ���� ����

            GameObject prefabs_Object = GetQueue_Coin();     // ���� �������� ������Ʈ Ǯ������ ��������
            Vector3 insertPos = Vector3.zero;      // ������ ��ġ�� �޴� ����

            insertPos = new Vector3(randInt_X * 3, 2, clone.GetChild(intList_Coin[randInt_Z]).transform.position.z);
            // ������ ��ġ ����
            prefabs_Object.transform.position = insertPos;       // ������ ��ġ ����
            //prefabs_Object.transform.parent = clone.GetChild(intList_Coin[randInt_Z]);


            mini07_Area.area_Action += prefabs_Object.GetComponent<Mini07_Coin>().End_Area;



            intList_Coin.RemoveAt(randInt_Z);                 // ����� ���� ����

            randInt_Z = Random.Range(0, intList_Coin.Count);        // ������ Z���� �޴� ���� ����
            switch (randInt_Object)
            {
                case 0:
                    prefabs_Object = GetQueue_StopSign();   // ť���� ������

                    randInt_X = Random.Range(-2, 3);
                    insertPos = new Vector3(randInt_X * 3, 0, clone.GetChild(intList_Object[randInt_Z]).transform.position.z);         // �ٴڿ����� ��ֹ� ��ġ ����

                    break;
                case 1:
                    prefabs_Object = GetQueue_Ban();   // ť���� ������

                    prefabs_Object.transform.rotation = Quaternion.Euler(new Vector3(-90, randRotate * 45.0f, 180));      // ȸ���� ����
                    randInt_X = Random.Range(-1, 3);         // -1, 0, 1, 2
                    insertPos = new Vector3((randInt_X * 3 - 1.5f), 0, clone.GetChild(intList_Object[randInt_Z]).transform.position.z);         // �ٴڿ����� ��ֹ� ��ġ ����

                    break;
                case 2:
                    prefabs_Object = GetQueue_Container();   // ť���� ������

                    prefabs_Object.transform.rotation = Quaternion.Euler(new Vector3(-90, randRotate * 45.0f, 0));      // ȸ���� ����
                    randInt_X = Random.Range(-1, 2);
                    insertPos = new Vector3(randInt_X * 3, 0, clone.GetChild(intList_Object[randInt_Z]).transform.position.z);         // �ٴڿ����� ��ֹ� ��ġ ����

                    break;
                default:
                    prefabs_Object = GetQueue_Sandwich();   // ť���� ������

                    prefabs_Object.transform.rotation = Quaternion.Euler(new Vector3(-90, randRotate * 45.0f, 0));      // ȸ���� ����

                    randInt_X = Random.Range(-1, 1);
                    insertPos = new Vector3((randInt_X * 3 + 1.5f), 0, clone.GetChild(intList_Object[randInt_Z]).transform.position.z);
                    // ��ġ �� ����

                    break;
            }
            prefabs_Object.transform.position = insertPos;                  // ���� ������ ���� ��ġ�� �ű��.
            //prefabs_Object.transform.parent = clone.GetChild(intList_Object[randInt_Z]);


            mini07_Area.area_Action += prefabs_Object.GetComponent<Mini07_Obj>().End_Area;


            intList_Object.RemoveAt(randInt_Z);                 // ����� ���� ����
        }

        intList_Coin.Clear();           // ���� �ߺ� ����(����) ����Ʈ �ʱ�ȭ
        intList_Object.Clear();         // ���� �ߺ� ���� ����Ʈ �ʱ�ȭ
    }
}
