using System.Collections.Generic;
using UnityEngine;

public class AudioMng : MonoBehaviour       // 스타트 씬부터 실행
{
    [SerializeField] AudioSource effectAudio;       // 이 스크립트가 붙어있는 오브젝트에 붙어있는 오디오 소스(효과음용)
    [SerializeField] AudioSource BG_Audio;       // 이 스크립트가 붙어있는 오브젝트에 붙어있는 오디오 소스(배경음악용)

    Dictionary<string, AudioClip> effects;    // 딕셔너리로 클립 판단
    Dictionary<string, AudioClip> BGs;

    private static AudioMng instance;       // 싱글톤 작업
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




        LoadFile(ref effects, "Sound/Effect/");       // 딕셔너리에 효과음을 저장한다.
        LoadFile(ref BGs, "Sound/BackGround/");       // 딕셔너리에 배경음악을 저장한다.
    }


    void Start()
	{
        if (Main.ins.nowPlayer != null)            // 저장 내용이 있다면...(사실 필요없음)
        {
            SetBackGroundVolume(Main.ins.nowPlayer.Volume_BackGround);     // 저장된 배경음악 소리 크기를 가져온다.
            SetEffectVolume(Main.ins.nowPlayer.Volume_Effect);             // 저장된 효과음 소리 크기를 가져온다.
        }
    }


	public void LoadFile<T>(ref Dictionary<string, T> a, string path) where T : Object       // 딕셔너리에 프리팹(음악) 저장
    {
        string log = path + "에서 불러온 파일 \n";
        a = new Dictionary<string, T>();
        T[] particleSystems = Resources.LoadAll<T>(path);
        foreach (var particle in particleSystems)
        {
            log += particle.name + "\n";
            a.Add(particle.name, particle);
        }

    }



    //////////////////////// 효과음
    public void PlayEffect(string name)         // 효과음 소리 내기
    {
        effectAudio.clip = effects[name];       // 딕셔너리에 저장된 효과음을 클립으로 넘기고W
        effectAudio.Play();                     // 소리 내기
    }
    public void StopEffect() { effectAudio.Stop(); }       // 효과음 소리 멈추기
    public void LoopEffect(bool loopBool) { effectAudio.loop = loopBool; }       // 효과음 무한루프


    //0~1
    public void SetEffectVolume(float scale) {effectAudio.volume = scale;}     // 효과음 소리 조절
    public float GetEffectVolume() { return effectAudio.volume; }                // 효과음 소리 가져오기






    //////////////////////// 배경 음악
    public void Play_BG(string name)         // 배경음악 소리내기
    {
        BG_Audio.clip = BGs[name];       // 딕셔너리에 저장된 배경음악을 클립으로 넘기고
        BG_Audio.Play();                     // 소리 내기
    }
    public void Stop_BG() { BG_Audio.Stop(); }       // 효과음 소리 멈추기
    public void Pause_BG() { BG_Audio.Pause(); }         // 배경음악 멈추기
    public void UnPause_BG() { BG_Audio.UnPause(); }     // 멈춘 배경음악 풀기


    //0~1
    public void SetBackGroundVolume(float scale) { BG_Audio.volume = scale; }    // 배경음악 소리 조절
    public float GetBackGroundVolume() { return BG_Audio.volume; }                // 배경음악 소리 가져오기
}
