using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class AudioInfo
// {
//     public GameObject audioObject;
//     public AudioClip audioClip;

//     public AudioInfo(GameObject audioObject, AudioClip audioClip)
//     {
//         this.audioObject = audioObject;
//         this.audioClip = audioClip;
//     }
// }


// public class AudioManager
// {
//     private Dictionary<string, AudioClip> AudioDict;
//     private AudioSource BGMSource;
//     private AudioSource EffectSource;

//     public AudioManager()
//     {
//         AudioDict = new Dictionary<string, AudioClip>();
//     }

//     public void PlayAudio(E_AudioType audioType, string name, bool isLoop = false)
//     {
//         //字典中有音频
//         if (AudioDict.ContainsKey(name))
//         {
//             //播放从对象池中获取的音频
//             GameObject poolObject = GameManager.Instance.m_ObjectPool.GetPoolObject(name);
//             PlayAudioClip(name, audioType, AudioDict[name], poolObject, isLoop);
//         }
//         //字典中没有音频
//         else
//         {
//             AudioClip audioClip = GameManager.Instance.m_ResourcesLoader.Load<AudioClip>(E_ResourcesPath.Audio, name);
//             if (audioClip == null)
//             {
//                 Debug.LogError("AudioManager:未找到该名称的音频：" + name + ",请检查Resources文件夹中的音频是否存在");
//                 return;
//             }

//             //创建新的AudioObject
//             GameObject newAudioObject = new GameObject(name);
//             AudioSource audioSource = newAudioObject.AddComponent<AudioSource>();
//             audioSource.clip = audioClip;

//             //添加新音频至字典
//             AudioDict.Add(name, audioClip);

//             //添加音频至对象池
//             GameObject poolObj = GameManager.Instance.m_ObjectPool.AddObject(name, newAudioObject);

//             //播放音频
//             PlayAudioClip(name, audioType, AudioDict[name], newAudioObject, isLoop);
//         }
//     }

//     public void BGMStop()
//     {
//         if (BGMSource != null)
//         {
//             GameManager.Instance.m_ObjectPool.ReturnObject(BGMSource.gameObject.name, BGMSource.gameObject, () => { });
//             BGMSource = null;
//         }
//     }

//     public void BGMPause()
//     {
//         if (BGMSource != null)
//             BGMSource.Pause();

//         // Debug.Log(BGMSource);
//     }

//     public void BGMResume()
//     {
//         if (BGMSource != null)
//             BGMSource.Play();
//     }

//     public void ClearAudio()
//     {
//         BGMStop();
//         AudioDict.Clear();
//         GameManager.Instance.m_ObjectPool.ClearPool();
//     }

//     public void SetBGMVolume(float volume)
//     {
//         if (BGMSource != null)
//         {
//             BGMSource.volume = volume;
//         }
//     }

//     public void SetEffectVolume(float volume)
//     {
//         if (EffectSource != null)
//         {
//             EffectSource.volume = volume;
//         }
//     }



//     private void PlayAudioClip(string name, E_AudioType audioType, AudioClip audioClip, GameObject audioObject, bool isLoop)
//     {
//         if (audioClip != null)
//         {
//             audioObject.transform.parent = GameManager.Instance.transform;
//             AudioSource audioSource = audioObject.GetComponent<AudioSource>();
//             switch (audioType)
//             {
//                 case E_AudioType.BGM:
//                     BGMStop();
//                     audioSource.loop = isLoop;
//                     audioSource.Play();
//                     BGMSource = audioSource;
//                     break;
//                 case E_AudioType.Effect:
//                     GameManager.Instance.StartCoroutine(IE_PlayOnceAudio(name, audioClip, audioObject));
//                     audioSource.loop = isLoop;
//                     audioSource.Play();
//                     EffectSource = audioSource;
//                     break;
//             }
//         }
//     }

//     private IEnumerator IE_PlayOnceAudio(string name, AudioClip clip, GameObject audioObj)
//     {
//         yield return new WaitForSeconds(clip.length);
//         GameManager.Instance.m_ObjectPool.ReturnObject(name, audioObj);
//     }

// }
