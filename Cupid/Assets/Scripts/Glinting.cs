using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glinting : MonoBehaviour
{
    public Color color = new Color(1, 0, 1, 1);         // 闪烁颜色
    [Range(0.0f, 1.0f)]                                 // 最低发光亮度，取值范围[0,1]，需小于最高发光亮度
    public float minBrightness = 0.0f;
    [Range(0.0f, 1)]                                    // 最高发光亮度，取值范围[0,1]，需大于最低发光亮度
    public float maxBrightness = 0.5f;
    [Range(0.2f, 30.0f)]                                // 闪烁频率，取值范围[0.2,30.0]
    public float rate = 1;

    [Tooltip("勾选此项则启动时自动开始闪烁")]
    [SerializeField]
    private bool _autoStart = false;

    private float _h, _s, _v;                           // 色调，饱和度，亮度
    private float _deltaBrightness;                     // 最低最高亮度差
    private Renderer _renderer;
    private Material _material;
    private readonly string _keyword = "_EMISSION";
    private readonly string _colorName = "_EmissionColor";

    private Coroutine _glinting;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _material = _renderer.material;

        if (_autoStart)
        {
            StartGlinting();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 校验数据，并保证运行时的修改能够得到应用。
    // 该方法只在编辑器模式中生效！！！
    private void OnValidate()
    {
        // 限制亮度范围
        if (minBrightness < 0 || minBrightness > 1)
        {
            minBrightness = 0.0f;
            Debug.LogError("最低亮度超出取值范围[0, 1]，已重置为0。");
        }
        if (maxBrightness < 0 || maxBrightness > 1)
        {
            maxBrightness = 1.0f;
            Debug.LogError("最高亮度超出取值范围[0, 1]，已重置为1。");
        }
        if (minBrightness >= maxBrightness)
        {
            minBrightness = 0.0f;
            maxBrightness = 1.0f;
            Debug.LogError("最低亮度[MinBrightness]必须低于最高亮度[MaxBrightness]，已分别重置为0/1！");
        }

        // 限制闪烁频率
        if (rate < 0.2f || rate > 30.0f)
        {
            rate = 1;
            Debug.LogError("闪烁频率超出取值范围[0.2, 30.0]，已重置为1.0。");
        }

        // 更新亮度差
        _deltaBrightness = maxBrightness - minBrightness;

        // 更新颜色
        // 注意不能使用 _v ，否则在运行时修改参数会导致亮度突变
        float tempV = 0;
        Color.RGBToHSV(color, out _h, out _s, out tempV);
    }

    // 开始闪烁
    public void StartGlinting()
    {
        _material.EnableKeyword(_keyword);

        if (_glinting != null)
        {
            StopCoroutine(_glinting);
        }
        _glinting = StartCoroutine(IEGlinting());
    }

    // 停止闪烁
    public void StopGlinting()
    {
        _material.DisableKeyword(_keyword);

        if (_glinting != null)
        {
            StopCoroutine(_glinting);
        }
    }

    // 控制自发光强度
    private IEnumerator IEGlinting()
    {
        Color.RGBToHSV(color, out _h, out _s, out _v);
        _v = minBrightness;
        _deltaBrightness = maxBrightness - minBrightness;

        bool increase = true;
        while (true)
        {
            if (increase)
            {
                _v += _deltaBrightness * Time.deltaTime * rate;
                increase = _v <= maxBrightness;
            }
            else
            {
                _v -= _deltaBrightness * Time.deltaTime * rate;
                increase = _v <= minBrightness;
            }
            _material.SetColor(_colorName, Color.HSVToRGB(_h, _s, _v));
            yield return null;
        }
    }
}
