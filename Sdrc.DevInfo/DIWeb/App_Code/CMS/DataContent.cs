using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;

/// <summary>
/// model for data content
/// </summary>
public class DataContent
{
    #region  -- Properties --

    public int ContentId { get; set; }
    public string MenuCategory { get; set; }
    public string Title { get; set; }
    public SqlDateTime Date { get; set; }
    public string Thumbnail { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string PDFUpload { get; set; }
    public SqlDateTime DateAdded { get; set; }
    public SqlDateTime DateModified { get; set; }
    public Boolean Archived { get; set; }
    public int ArticleTagID { get; set; }
    public string UserNameEmail { get; set; }
    public string URL { get; set; }
    public string LngCode { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsHidden { get; set; }
    public string Fld1 { get; set; }
    public string Fld2 { get; set; }
    public string Fld3 { get; set; }
    public string Fld4 { get; set; }
    public string Fld5 { get; set; }
    public string Fld6 { get; set; }
    public string Fld1Text { get; set; }
    public string Fld2Text { get; set; }
    public string Fld3Text { get; set; }
    public string Fld4Text { get; set; }
    public string Fld5Text { get; set; }
    public string Fld6Text { get; set; }

    #endregion
}