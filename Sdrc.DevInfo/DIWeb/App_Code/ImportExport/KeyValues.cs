using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeyValues
/// </summary>
public class KeyValues
{



    #region -- Private --

    #region  -- Properties --

    private string _Key = string.Empty;
    public string Key
    {
        get { return _Key; }
        set { _Key = value; }
    }

    private string _ValueSource = string.Empty;
    public string ValueSource
    {
        get { return _ValueSource; }
        set { _ValueSource = value; }
    }

    private string _ValueTarget = string.Empty;
    public string ValueTarget
    {
        get { return _ValueTarget; }
        set { _ValueTarget = value; }
    }
    

    #endregion

    #region -- Methods --

    #endregion

    #endregion

    #region -- Public --

    #region -- Methods --
    public KeyValues()
    {
        //Do Nothing
    }
    #endregion

    #endregion

   
}