using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// This class provide common events for processing 
    /// </summary>
   public class ProcessEventCreator
    {
        #region "-- Events --"

        /// <summary>
        /// Fire before processing records
        /// </summary>
        public event ProcessInfoDelegate BeforeProcess;

        /// <summary>
        /// Fire while processing into database
        /// </summary>
        public event ProcessInfoDelegate ProcessInfo;

        /// <summary>
        /// Fire when process is going to start
        /// </summary>
        public event ProcessDelegate StartProcess;

        /// <summary>
        /// Fire when process completed or stopped
        /// </summary>
        public event ProcessDelegate EndProcess;

        #endregion 

        #region "-- Methods: Rasie Events --"

        protected void RaiseBeforeProcessEvent(int recordsFound)
        {
            if (this.BeforeProcess != null & recordsFound>0)
                this.BeforeProcess(recordsFound);
        }

        protected void RaiseProcessInfoEvent(int currentRecordNumber)
        {
            if (this.ProcessInfo != null)
                this.ProcessInfo(currentRecordNumber);
        }

        protected void RaiseStartProcessEvent()
        {
            if (this.StartProcess != null)
                this.StartProcess();
        }

        protected void RaiseEndProcessEvent()
        {
            if (this.EndProcess != null)
                this.EndProcess();
        }



        #endregion
    }
}
