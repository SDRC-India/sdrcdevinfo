using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.CommonDelegates
{
    # region " -- Delegate progress Bar -- "

    /// <summary>
    /// A delegate for ProgressBar_Increment event.
    /// </summary>
    public delegate void IncrementProgressBar(int value);

    /// <summary>
    /// A delegate for ProgressBar_Initialize event.
    /// </summary>
    /// <param name="maximumValue"></param>
    public delegate void InitializeProgressBar(int maximumValue);

    /// <summary>
    /// A delegate for PrograssBar_Close event
    /// </summary>
    public delegate void CloseProgressBar();

    /// <summary>
    /// A delegate for Selected Value Changed event
    /// </summary>
    public delegate void SeletionChanged();

    /// <summary>
    /// Delegate to Initialize Process of Backgroud Worker
    /// </summary>
    /// <param name="currentProcess"></param>
    /// <param name="totalSheetCount"></param>
    /// <param name="maximumValue"></param>
    public delegate void InitializeWorkerProcess(string currentProcessText, int totalSheetCount, int maximumValue);
    
    /// <summary>
    /// Delegate to Set Process Info to Worker
    /// </summary>
    /// <param name="indicatorName"></param>
    /// <param name="unitName"></param>
    /// <param name="currentSheetNo"></param>
    public delegate void SetWorkerProcessName(string indicatorName, string unitName, int currentSheetNo);


    # endregion
    
}
