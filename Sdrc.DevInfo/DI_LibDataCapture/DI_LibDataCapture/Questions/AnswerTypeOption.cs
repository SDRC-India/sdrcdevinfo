using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
    [Serializable()]
    public class AnswerTypeOption
    {
        #region "-- Private --"

        #region "-- New / Dispose --"

        private AnswerTypeOption()
        {
            // do not implement this
        }
        
        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/ Properties --"


        private string _Text = string.Empty;
        /// <summary>
        /// Gets or sets text for options
        /// </summary>
        public string Text
        {
            get
            
            {
                return this._Text;
            }
            set
            {
                this._Text = value;
            }
        }

        private string _StringID= string.Empty;
        /// <summary>
        /// Gets or sets string ID for options
        /// </summary>
        public string StringID
        {
            get
            {
                return this._StringID;
            }
            set
            {
                this._StringID = value;
            }
        }


        private string _NumericValue = string.Empty;
        /// <summary>
        /// Gets or sets numeric value for options
        /// </summary>
        public string NumericValue
        {
            get
            {
                return this._NumericValue;
            }
            set
            {
                this._NumericValue = value;
            }
        }

        private string _SubgroupGId;
        /// <summary>
        /// Gets or sets subgroup Gid. 
        /// </summary>
        public string SubgroupGId
        {
            get 
            {
                return this._SubgroupGId; 
            }
            set 
            {
                this._SubgroupGId = value; 
            }
        }
       
        #endregion

        #region "-- New / Dispose --"
        /// <summary>
        ///  Returns object of AnswerTypeOption.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="stringID"></param>
        /// <param name="numericValue"></param>
        /// <param name="SubgroupGId"></param>
        public AnswerTypeOption(string text, string stringID, string numericValue,string SubgroupGId)
        {
            this._Text = text;
            this._StringID = stringID;
            this._NumericValue = numericValue;
            this._SubgroupGId = SubgroupGId;
        }

        #endregion

        #endregion
    }
}
