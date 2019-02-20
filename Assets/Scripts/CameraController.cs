using System;
using System.IO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// 摄像机控制器
public class CameraController : MonoBehaviour
{
    private AspectRatioFitter fitter;
    private RawImage background;

    void Start()
    {
        if (GameObject.Find("Background Image"))
        {
            background = GameObject.Find("Background Image").GetComponent<RawImage>();
            fitter = GameObject.Find("Background Image").GetComponent<AspectRatioFitter>();
            SetBackgroundByUrl(Path.Combine(Application.streamingAssetsPath, "Background" + (int)UnityEngine.Random.Range(1, 4) + ".jpg"));
        }
    }

    // 设置背景
    public void SetBackgroundByUrl(string path)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            path = "file://" + path;
        }
        StartCoroutine(SetBackgroundByUrlAsync(path));
    }

    // 设置背景异步实现
    private IEnumerator SetBackgroundByUrlAsync(string path)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(path, true))
        {
            yield return request.SendWebRequest();
            if (request.responseCode == 200)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                fitter.aspectRatio = texture.width * 1f / texture.height;
                background.texture = texture;
            }
            else
            {
                Debug.Log("Set " + path + " to background failed. Error: " + request.error);
            }
        }
    }

    // 摄像机x轴震动
    public void ShakeX()
    {
        Shake(
            0.1f,
            10f,
            1f,
            () => transform.position.x,
            (x) => transform.SetPositionAndRotation(new Vector3(x, transform.position.y, transform.position.z), Quaternion.Euler(90, 0, 0)),
            () => print("done")
        );
    }

    // 摄像机y轴震动
    public void ShakeY()
    {
        Shake(
            0.1f,
            10f,
            1f,
            () => transform.position.y,
            (y) => transform.SetPositionAndRotation(new Vector3(transform.position.x, y, transform.position.z), Quaternion.Euler(90, 0, 0)),
            () => print("done")
        );
    }

    public enum ShakeType
    {
        Smooth = 0,
        PerlinNoise,
        Random
    };

    // 震动
    public void Shake(
        float magnitude,
        float speed,
        float duration,
        Func<float> OnGetOriginal,
        Action<float> OnShake,
        Action OnComplete = null,
        ShakeType shakeType = ShakeType.Smooth
    )
    {
        StartCoroutine(ShakeRoutine(magnitude, speed, duration, OnGetOriginal, OnShake, OnComplete, shakeType));
    }

    // 震动异步实现
    public static IEnumerator ShakeRoutine(
        float magnitude,                        // 震动幅度
        float speed,                            // 震动速度
        float duration,                         // 震动时间
        Func<float> OnGetOriginal,              // 获取固定点坐标
        Action<float> OnShake,                  // 获取震动数值
        Action OnComplete = null,               // 完成回调
        ShakeType shakeType = ShakeType.Smooth  // 震动类型
    )
    {
        // 震动耗时
        float elapsed = 0.0f;
        // 随机起始点
        float random = UnityEngine.Random.Range(-1.5f, 1.5f);
        // 获得固定点
        float original = OnGetOriginal();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            // 当前波动
            float rps = random + percent * speed;
            // 波动映射到[-1, 1]
            float range;
            switch (shakeType)
            {
                case ShakeType.Smooth:
                    range = Mathf.Sin(rps) + Mathf.Cos(rps);
                    break;
                case ShakeType.PerlinNoise:
                    range = Mathf.PerlinNoise(rps, rps);
                    break;
                case ShakeType.Random:
                    range = UnityEngine.Random.value * 2.0f - 1.0f;
                    break;
                default:
                    range = 0.0f;
                    break;
            }
            // 震动总时间的50%后开始衰减
            if (percent < 0.5f)
            {
                OnShake(range * magnitude + original);
            }
            else
            {
                // 计算衰减
                OnShake(range * magnitude * (2.0f * (1.0f - percent)) + original);
            }
            yield return null;
        }
        if (OnComplete != null)
        {
            // 完成回调
            OnComplete();
        }
    }
}