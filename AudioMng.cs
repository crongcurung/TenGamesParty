using System.Collections.Generic;
using UnityEngine;

public class AudioMng : MonoBehaviour       // ��ŸƮ ������ ����
{
    [SerializeField] AudioSource effectAudio;       // �� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� �پ��ִ� ����� �ҽ�(ȿ������)
    [SerializeField] AudioSource BG_Audio;       // �� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� �پ��ִ� ����� �ҽ�(������ǿ�)

    Dictionary<string, AudioClip> effects;    // ��ųʸ��� Ŭ�� �Ǵ�
    Dictionary<string, AudioClip> BGs;

    private static AudioMng instance;       // �̱��� �۾�
    public static AudioMng ins
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
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }




        LoadFile(ref effects, "Sound/Effect/");       // ��ųʸ��� ȿ������ �����Ѵ�.
        LoadFile(ref BGs, "Sound/BackGround/");       // ��ųʸ��� ��������� �����Ѵ�.
    }


    void Start()
	{
        if (Main.ins.nowPlayer != null)            // ���� ������ �ִٸ�...(��� �ʿ����)
        {
            SetBackGroundVolume(Main.ins.nowPlayer.Volume_BackGround);     // ����� ������� �Ҹ� ũ�⸦ �����´�.
            SetEffectVolume(Main.ins.nowPlayer.Volume_Effect);             // ����� ȿ���� �Ҹ� ũ�⸦ �����´�.
        }
    }


	public void LoadFile<T>(ref Dictionary<string, T> a, string path) where T : Object       // ��ųʸ��� ������(����) ����
    {
        string log = path + "���� �ҷ��� ���� \n";
        a = new Dictionary<string, T>();
        T[] particleSystems = Resources.LoadAll<T>(path);
        foreach (var particle in particleSystems)
        {
            log += particle.name + "\n";
            a.Add(particle.name, particle);
        }

    }



    //////////////////////// ȿ����
    public void PlayEffect(string name)         // ȿ���� �Ҹ� ����
    {
        effectAudio.clip = effects[name];       // ��ųʸ��� ����� ȿ������ Ŭ������ �ѱ��W
        effectAudio.Play();                     // �Ҹ� ����
    }
    public void StopEffect() { effectAudio.Stop(); }       // ȿ���� �Ҹ� ���߱�
    public void LoopEffect(bool loopBool) { effectAudio.loop = loopBool; }       // ȿ���� ���ѷ���


    //0~1
    public void SetEffectVolume(float scale) {effectAudio.volume = scale;}     // ȿ���� �Ҹ� ����
    public float GetEffectVolume() { return effectAudio.volume; }                // ȿ���� �Ҹ� ��������






    //////////////////////// ��� ����
    public void Play_BG(string name)         // ������� �Ҹ�����
    {
        BG_Audio.clip = BGs[name];       // ��ųʸ��� ����� ��������� Ŭ������ �ѱ��
        BG_Audio.Play();                     // �Ҹ� ����
    }
    public void Stop_BG() { BG_Audio.Stop(); }       // ȿ���� �Ҹ� ���߱�
    public void Pause_BG() { BG_Audio.Pause(); }         // ������� ���߱�
    public void UnPause_BG() { BG_Audio.UnPause(); }     // ���� ������� Ǯ��


    //0~1
    public void SetBackGroundVolume(float scale) { BG_Audio.volume = scale; }    // ������� �Ҹ� ����
    public float GetBackGroundVolume() { return BG_Audio.volume; }                // ������� �Ҹ� ��������
}
