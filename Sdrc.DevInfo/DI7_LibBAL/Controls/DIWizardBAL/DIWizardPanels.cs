using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.DIWizardBAL
{
    /// <summary>
    /// Collection class of wizard panel
    /// </summary>
    public class DIWizardPanels : CollectionBase
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "


        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Add the panel in the collection.
        /// </summary>
        /// <param name="dIWizardPanel"></param>
        public void Add(DIWizardPanel dIWizardPanel)
        {
            this.List.Add(dIWizardPanel);
        }

        /// <summary>
        /// Get the panel on the basis of index
        /// </summary>
        /// <param name="panelIndex"></param>
        /// <returns></returns>
        public DIWizardPanel this[int panelIndex]
        {
            get
            {
                DIWizardPanel Retval = null;
                try
                {
                    if (panelIndex < 0)
                    {
                        Retval = null;
                    }
                    else
                    {
                        Retval = (DIWizardPanel)this.List[panelIndex];
                    }
                }
                catch (Exception)
                {
                    Retval = null;
                }
                return Retval;
            }
        }

        /// <summary>
        /// Get the panel on the basis of panel type.
        /// </summary>
        /// <param name="wizardPanel"></param>
        /// <returns></returns>
        public DIWizardPanel this[DIWizardPanel.PanelType wizardPanel]
        {
            get
            {
                DIWizardPanel Retval = null;
                try
                {
                    for (int i = 0; i < this.List.Count; i++)
                    {
                        if (this[i].Type == wizardPanel)
                        {
                            Retval = this[i];
                        }
                    }
                }
                catch (Exception)
                {
                    Retval = null;
                }
                return Retval;
            }
        }

        #endregion

        #endregion
    }
}
