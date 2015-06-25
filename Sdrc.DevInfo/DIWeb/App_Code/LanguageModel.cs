using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LanguageModel
/// </summary>
public class LanguageModel
{
    public IDictionary<int, string> LanguagesFromDB { get; set; }
    public string DefaultLang { get; set; }
	public LanguageModel()
	{
        LanguagesFromDB = new Dictionary<int, string>();
	}
}