using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spire.Xls;
using System.IO;

/// <summary>
/// Summary description for exceltopng
/// </summary>
public class exceltopng
{
	public exceltopng()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string ExceltoPngFormat(string folderStructure, string RetVal)
    {
        Workbook workbook1 = new Workbook();
        workbook1.LoadFromFile(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".xls"));
        Worksheet sheet = workbook1.Worksheets[0];
        sheet.SaveToImage(RetVal + ".png");
        System.Diagnostics.Process.Start(RetVal + ".png");
        return "";
    }
}