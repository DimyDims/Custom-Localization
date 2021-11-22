using Cerera.Services;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TextLocalizer : MonoBehaviour, IDependentObject
{
    [SerializeField] [TextArea] private string _textTemplate;

    private Localization _localization;

    [Inject]
    private void Construct(Localization localization)
    {
        _localization = localization;
    }

    private void Start()
    {
        Localize();
        _localization.LanguageChanged += Localize;
    }

    private void OnDestroy()
    {
        _localization.LanguageChanged -= Localize;
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        SetValueInText(_textTemplate);
        SetValueInTextMeshPro(_textTemplate);
    }

#endif

    public void SetKey(string key)
    {
        SetText($"[{key}]");
    }

    public void SetText(string text)
    {
        _textTemplate = text;
        if (enabled)
        {
            Localize();
        }
    }

    private void Localize()
    {
        string pattern = @"(^|[^\\])\[([^]]*)]";
        string text = Regex.Replace(_textTemplate, pattern, ReplaceKey, RegexOptions.Multiline);
        SetValueInText(text);
        SetValueInTextMeshPro(text);
    }

    private string ReplaceKey(Match match)
    {
        string firstCharacter = match.Groups[1].Value;
        string key = match.Groups[2].Value;
        return $"{firstCharacter}{_localization.GetLocaleText(key)}";
    }

    private void SetValueInText(string value)
    {
        Text text = GetComponent<Text>();
        if (text != null)
        {
            text.text = value;
        }
    }

    private void SetValueInTextMeshPro(string value)
    {
        var text = GetComponent<TMPro.TextMeshProUGUI>();
        if (text != null)
        {
            text.SetText(value);
        }
    }
}
