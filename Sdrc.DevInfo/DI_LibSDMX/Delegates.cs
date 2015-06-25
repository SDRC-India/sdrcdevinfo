using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibSDMX
{

    /// <summary>
    /// Delegate for intitializing Process of Background Worker.
    /// </summary>
    /// <param name="currentProcess"></param>
    /// <param name="totalProgressCount"></param>
    /// <param name="maximumValue"></param>
    public delegate void Initialize_Process(string mainProcessName, int totalProgressCount, int maximumValue);

    /// <summary>
    /// Delegate for setting Process Info to Background Worker.
    /// </summary>
    /// <param name="indicatorName"></param>
    /// <param name="unitName"></param>
    /// <param name="currentProgressCount"></param>
    public delegate void Set_Process_Name(string indicatorName, string unitName);

    /// <summary>
    /// Delegate for creating event for notifying progress of a process.
    /// </summary>
    /// <param name="recordNo"></param>
    public delegate void Notify_Progress(int recordNo);

    /// <summary>
    /// Delegate for creating event for notifying file name on file creation.
    /// </summary>
    /// <param name="fileNameWPath">The file name with path of created file.</param>
    public delegate void Notify_File_Name(string fileNameWPath);

    /// <summary>
    /// Delegate for creating event for IUS skipped during import into database
    /// </summary>
    /// <param name="indicatorName"></param>
    /// <param name="unitName"></param>
    /// <param name="subgroup"></param>
    public delegate void IUSSkipped(string indicatorName, string unitName, string subgroup);

}
