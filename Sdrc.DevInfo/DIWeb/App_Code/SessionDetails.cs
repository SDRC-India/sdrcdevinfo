using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Xml.Serialization;

/// <summary>
/// Summary description for SessionDetails
/// </summary>
[Serializable()]
[System.Xml.Serialization.XmlRootAttribute("SessionDetails")]
public class SessionDetails
{
    public SessionDetails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Loads the SessionDetails object from serialized xml SessionDetails file.
    /// </summary>
    /// <param name="serializeFileName">file Path of xml serialized SessionDetails object.</param>
    /// <returns>SessionDetails object.</returns>
    public static SessionDetails Load(string serializeFileName)
    {
        SessionDetails RetVal = null;
        FileStream _IO = null;
        if (File.Exists(serializeFileName))
        {
            try
            {
                _IO = new FileStream(serializeFileName, FileMode.Open);
                XmlSerializer _SRZFrmt = new XmlSerializer(typeof(SessionDetails));
                RetVal = (SessionDetails)_SRZFrmt.Deserialize(_IO);

            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                Global.CreateExceptionString(ex, null);
                //throw;
            }
            finally
            {
                if (_IO != null)
                {
                    _IO.Flush();
                    _IO.Close();
                }
            }
        }

        return RetVal;
    }

    private string _CurrentSelectedAreaNids = string.Empty;

    public string CurrentSelectedAreaNids
    {
        get { return _CurrentSelectedAreaNids; }
        set { _CurrentSelectedAreaNids = value; }
    }

    private bool _IsMyData = false;

    public bool IsMyData
    {
        get { return _IsMyData; }
        set { _IsMyData = value; }
    }

    private DataTable _DataViewNonQDS = null;

    public DataTable DataViewNonQDS
    {
        get { return _DataViewNonQDS; }
        set
        {
            this._DataViewNonQDS = value;
            this._DataViewNonQDS.TableName = "sessionData";
        }
    }

}