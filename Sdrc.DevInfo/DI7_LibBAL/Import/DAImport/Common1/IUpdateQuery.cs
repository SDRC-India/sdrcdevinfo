using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Common
{
    /// <summary>
    /// This interface is used to declare signature of all SQL "Update" Queries required in Import process.
    /// <para>This is done because "UPDATE" queries having JOINS are now different for two different kinds of RDBMS (MSAccess, MySql) and (SQL Server & Oracle).</para>
    /// </summary>
    internal interface IUpdateQuery
    {

        string UpdateNIDForIndicatorGID();

        string UpdateNIDForSubgroupGID();

        string UpdateNIDForUnitGID();

        string UpdateNIDForIndicatorName();

        string UpdateNIDForUnitName();

        string UpdateNIDForSubgroupValName();

        string UpdateNIDForAreaNId();

        string UpdateIUSNidofMatchedRecords();

        string UpdateUnmatchedIUSNidFromTempUnmatchedIUSTable(bool updateByGID);

        string UpdateTimePeriodNid();

        string UpdateFootNoteNid();

        string UpdateSourceNidInTempData();

        string UpdateICGlobal(string ICTableName);

        string UpdateDataValue();

        string UpdateIUSNidOfDuplicateRecord();

        string CreateTempSheetTable();

    }
}
