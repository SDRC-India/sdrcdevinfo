using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.VisualBasic;

using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    internal static class Utility
    {

        internal static int CreateClassificationChainFromExtDB(int srcICNId, int srcParentNId, string srcICGid, string srcICName, ICType srcICType, string srcICInfo, bool isGlobal, DIQueries srcQueries, DIConnection srcDBConnection,DIQueries targetDBQueries, DIConnection targetDBConnection)
        {
            
            int RetVal;
            //int TrgParentNId; 
            //string TrgParentName; 
            int NewParentNId;
            DataTable TempTable;
            IndicatorClassificationInfo ICInfo;
            IndicatorClassificationBuilder ClassificationBuilder = new IndicatorClassificationBuilder(targetDBConnection, targetDBQueries);


            // -- STEP 1: If the Parent NID is -1 then create the Classification at the root 
            if (srcParentNId == -1)
            {
                // -- Create the Classification 

                // -------------------------------------------------------------- 
                // While importing the Classifications, if the NId of the Source Classification is _ 
                // the same as that of the one created, then the Duplicate check fails and a duplicate 
                // classification getscreated. PASS -99 as the first parameter to the calling function 
                // -------------------------------------------------------------- 
                ICInfo = new IndicatorClassificationInfo();
                ICInfo.Parent = new IndicatorClassificationInfo();
                ICInfo.Parent.Nid = srcParentNId;
                ICInfo.Nid = srcICNId;
                ICInfo.Name = srcICName;
                ICInfo.ClassificationInfo = srcICInfo;
                ICInfo.GID = srcICGid;
                ICInfo.IsGlobal = isGlobal;
                ICInfo.Type = srcICType;

                RetVal = ClassificationBuilder.ImportIndicatorClassification(ICInfo, srcICNId, srcQueries, srcDBConnection);

            }



            else
            {
                // -- STEP 2: If the Parent is not -1 then check for the existence of the Parent and then create the Classification 
                // Classification can only be created if the parent exists 
                // -- STEP 2.1: If the Parent Exists then create the Classification under that parent 
                // -- STEP 2.2: If the Parent does not Exist then create the Parent first and then the Classification under that parent 

                // -- STEP 2: Check the existence of the Parent in the Target Database 
                // -- get the parent from the source database 

                TempTable = srcDBConnection.ExecuteDataTable(srcQueries.IndicatorClassification.GetIC(FilterFieldType.NId, srcParentNId.ToString(), srcICType, FieldSelection.Heavy));
                {

                    // -------------------------------------------------------------- 
                    // While importing the Classifications, if the NId of the Source Classification is _ 
                    // the same as that of the one created, then the Duplicate check fails and a duplicate 
                    // classification getscreated. PASS -99 as the first parameter to the calling function 
                    // -------------------------------------------------------------- 
                    DataRow Row;
                    string ClassificationInfo = string.Empty;
                    Row = TempTable.Rows[0];
                    if (!Information.IsDBNull(Row["IC_Info"]))
                        ClassificationInfo = Row[IndicatorClassifications.ICInfo].ToString();

                    NewParentNId = Utility.CreateClassificationChainFromExtDB(
                       Convert.ToInt32(Row[IndicatorClassifications.ICNId]),
                       Convert.ToInt32(Row[IndicatorClassifications.ICParent_NId]),
                        Row[IndicatorClassifications.ICGId].ToString(),
                        Row[IndicatorClassifications.ICName].ToString(),
                        srcICType,
                        ClassificationInfo, Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]), srcQueries, srcDBConnection,targetDBQueries,targetDBConnection); ;
                }




                // -- Create the Child Now 
                ICInfo = new IndicatorClassificationInfo();
                ICInfo.Parent = new IndicatorClassificationInfo();
                ICInfo.Parent.Nid = NewParentNId;       // set new parent nid
                ICInfo.Nid = srcICNId;
                ICInfo.Name = srcICName;
                ICInfo.ClassificationInfo = srcICInfo;
                ICInfo.GID = srcICGid;
                ICInfo.IsGlobal = isGlobal;
                ICInfo.Type = srcICType;

                RetVal = ClassificationBuilder.ImportIndicatorClassification(ICInfo, srcICNId, srcQueries, srcDBConnection);

            }

            //import ic and ius relationship into indicator_classification_IUS table
            ClassificationBuilder.ImportICAndIUSRelations(srcICNId, RetVal, ICInfo.Type, srcQueries, srcDBConnection);

            return RetVal;
        } 
    }
}
