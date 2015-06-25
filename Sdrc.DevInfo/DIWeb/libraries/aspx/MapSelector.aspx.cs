using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class libraries_aspx_MapSelector : System.Web.UI.Page
{
    #region "--Private--"

    #region "--Variables--"

    private enum CallbackType
    {
        None = 0,
        InitializeByMap = 1,
        ZoomInByMap = 2,
        ZoomOutByMap = 3,
        ZoomToRectangleByMap = 4,
        PanByMap = 5,
        FullExtentByMap = 6,
        SetMapSelection = 7,
        GetAreaSelection = 8,
        LabelByMap = 9
    }

    #endregion "--Variables--"

    #region "--Methods--"

    private void HandleCallBack()
    {
        string RetVal;
        string RequestParam;
        string RequestParam2;
        string RequestParam3;
        string RequestParam4;
        string RequestParam5;
        string RequestParam6;
        string dbNid = string.Empty;
        string lngCode = string.Empty;

        CallbackType OCallbackType;
        MapSelector OCallback;

        RetVal = string.Empty;
        RequestParam = string.Empty;
        RequestParam2 = string.Empty;
        RequestParam3 = string.Empty;
        RequestParam4 = string.Empty;
        RequestParam5 = string.Empty;
        RequestParam6 = string.Empty;

        OCallback = null;
        OCallbackType = CallbackType.None;

        try
        {
            if (Request.QueryString.Count > 0)
            {
                dbNid = Request.QueryString["dbnid"];
                lngCode = Request.QueryString["lngcode"];
            }

            if (Request.Params.Count > 0)
            {
                OCallbackType = (CallbackType)int.Parse(Request[Constants.RequestHeaderParamNames.CallBack].ToString());                
                RequestParam = Request[Constants.RequestHeaderParamNames.Param1];
                RequestParam2 = Request[Constants.RequestHeaderParamNames.Param2];
                RequestParam3 = Request[Constants.RequestHeaderParamNames.Param3];
                RequestParam4 = Request[Constants.RequestHeaderParamNames.Param4];
                RequestParam5 = Request[Constants.RequestHeaderParamNames.Param5];
                RequestParam6 = Request[Constants.RequestHeaderParamNames.Param6];

                OCallback = new MapSelector(this.Page);

                switch (OCallbackType)
                {
                    case CallbackType.InitializeByMap:
                        RetVal = OCallback.InitializeByMap(RequestParam, RequestParam2, RequestParam3, dbNid, lngCode);
                        break;
                    case CallbackType.ZoomInByMap:
                        RetVal = OCallback.ZoomInByMap();
                        break;
                    case CallbackType.ZoomOutByMap:
                        RetVal = OCallback.ZoomOutByMap();
                        break;
                    case CallbackType.ZoomToRectangleByMap:
                        RetVal = OCallback.ZoomToRectangleByMap(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.PanByMap:
                        RetVal = OCallback.PanByMap(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.FullExtentByMap:
                        RetVal = OCallback.FullExtentByMap();
                        break;
                    case CallbackType.GetAreaSelection:
                        RetVal = OCallback.GetAreaSelection(RequestParam);
                        break;
                    case CallbackType.SetMapSelection:
                        RetVal = OCallback.SetMapSelection(RequestParam, RequestParam2, dbNid, lngCode);
                        break;
                    case CallbackType.LabelByMap:
                        RetVal = OCallback.LabelByMap(RequestParam);
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            Response.Write(RetVal);
        }
    }    

    #endregion "--Methods--"

    #endregion "--Private--"

    #region "--Public--"

    #region "--Methods--"

    protected void Page_Load(object sender, EventArgs e)
    {
        this.HandleCallBack();
    }

    #endregion "--Methods--"

    #endregion "--Public--"
    
}
