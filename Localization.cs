using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Localization
{
    private Dictionary<string, string> _localizationData;

    private int _numberLanguage;

    private event Action _languageChanged;

    public event Action LanguageChanged
    {
        add => _languageChanged += value;
        remove => _languageChanged -= value;
    }

    public Localization()
    {
        _localizationData = new Dictionary<string, string>();
        ChangeLanguage();
    }

    public string GetLocaleText(string key)
    {
        if (_localizationData.ContainsKey(key))
        {
            string text = _localizationData[key];
            if (string.IsNullOrWhiteSpace(text))
            {
                return $"[No text - {key}]";
            }
            else
            {
                return text;
            }
        }
        else
        {
            return $"[No key - {key}]";
        }
    }

    public void ChangeLanguage()
    {
        if (_numberLanguage == 1)
        {
            _numberLanguage = 2;
        }
        else
        {
            _numberLanguage = 1;
        }

        _localizationData = new Dictionary<string, string>();

        using (FileStream stream = File.Open(Path.Combine(Application.streamingAssetsPath, "Localization", "Localization.xlsx"), FileMode.Open, FileAccess.Read))
        {
            using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                do
                {
                    while (excelReader.Read())
                    {
                        if (excelReader.GetString(0) != null)
                        {
                            _localizationData.Add(excelReader.GetString(0), excelReader.GetString(_numberLanguage));
                        }
                        else
                        {
                            _languageChanged?.Invoke();
                            return;
                        }
                    }
                } while (excelReader.NextResult());
            }
        }

        _languageChanged?.Invoke();
    }
}
