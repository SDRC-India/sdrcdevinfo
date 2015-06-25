using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{

    [Serializable()]
  public class JumpBoxQuestion
  {

      #region "-- Private  --"

      #region "-- New / Dispose --"

      private JumpBoxQuestion()
      {
          // do not implement this
      }

      #endregion

      #endregion

      #region "-- Public --"

      #region "-- Variables / Properties --"

      private string _Key;
        /// <summary>
        /// Gets or Sets jump box question key or page no like Q1,Q2,... or P1,P2,...etc.
        /// </summary>
        public string Key
        {
            get 
            {
                return this._Key; 
            }
            set
            {
                this._Key = value; 
            }
        }

        private string _Value;
        /// <summary>
        /// Gets or Sets jump box question value like Q1.a,Q1.b ,...etc.
        /// </summary>
        public string Value
        {
            get
            {
                return this._Value; 
            }
            set
            { 
                this._Value = value; 
            }
        }

        #endregion 

        #region "-- Methods --"

        public JumpBoxQuestion(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        #endregion

        #endregion
    }
}
