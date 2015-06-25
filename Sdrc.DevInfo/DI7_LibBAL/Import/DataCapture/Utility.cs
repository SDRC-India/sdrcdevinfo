using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.Import.DataCapture
{
    internal class Utility
    {

        #region"--Internal--"

        #region"--Method--"

        internal static string[] SplitStringNIncludeEmpyValue(string valueString, string delimiter)
        {
            string[] RetVal;

            //replace delimiter
            valueString = valueString.Replace(delimiter, "\n");
            RetVal = valueString.Split("\n".ToCharArray());

            return RetVal;
        }

        internal static string[] SplitString(string valueString, string delimiter)
        {
            string[] RetVal;
            int Index = 0;
            string Value;
            List<string> SplittedList = new List<string>();

            while (true)
            {
                Index = valueString.IndexOf(delimiter);
                if (Index == -1)
                {
                    if (!string.IsNullOrEmpty(valueString))
                    {
                        SplittedList.Add(valueString);
                    }
                    break;
                }
                else
                {
                    Value = valueString.Substring(0, Index);
                    valueString = valueString.Substring(Index + delimiter.Length);
                    SplittedList.Add(Value);
                }

            }

            RetVal = SplittedList.ToArray();

            return RetVal;
        }

        internal static string RemoveQuotes(string value)
        {
            return value.Replace("'", "''");
        }

        internal static void CopyFile(string sourceFile, string targetFile)
        {
            // copy xml file to temp location 
            try
            {
                if (File.Exists(targetFile))
                {
                    File.Delete(targetFile);
                }
                File.Copy(sourceFile, targetFile);
                File.SetAttributes(targetFile, FileAttributes.Normal);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        internal static string RemoveRegionalThousandSeperator(string dataValue)
        {
            string RetVal = dataValue;

            if (Microsoft.VisualBasic.Information.IsNumeric(dataValue))
            {
                RetVal = dataValue.Replace(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator, "");
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}
