using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
    [Serializable()]
    /// <summary>
    /// Stores Collection of All Mapped jump box question of current sheet 
    /// </summary>
    /// <example>
    /// <code>
    /// combobox1.DisplayMember="Value";
    /// combobox1.ValueMember="Key";
    /// combobox1.DataSource=objectOfJumpBoxQuestionCollection;
    /// </code>
    /// </example>
    public class JumpBoxQuesitonCollection : CollectionBase
    {

        #region "-- Public --"

        #region "-- New/Dispose --"

        /// <summary>
        /// To add question.
        /// </summary>
        /// <param name="jumpBoxQuestion"></param>
        public void Add(JumpBoxQuestion jumpBoxQuestion)
        {
            this.List.Add(jumpBoxQuestion);
        }

        /// <summary>
        /// Retruns True/False. True if jumpBoxQuestion Exists. 
        /// </summary>
        /// <param name="value">Question no like Q1.a,Q1.b,...</param>
        /// <returns>True (If Exists) otherwise False </returns>
        public bool Contains(string value)
        {
            bool RetVal = false;
            for (int i = 0; i < this.List.Count; i++)
            {
                if (((JumpBoxQuestion)this.List[i]).Value.ToUpper() == value.ToUpper())
                {
                    RetVal = true;
                    break;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Returns key like Q1,Q1,... or page no.
        /// </summary>
        /// <param name="value">Question no like Q1.a,Q1.b,... or page no.</param>
        /// <returns>Jump box question's key or page no</returns>
        public string GetKey(string value)
        {
            string RetVal = string.Empty;
            for (int i = 0; i < this.List.Count; i++)
            {
                if (((JumpBoxQuestion)this.List[i]).Value.ToUpper() == value.ToUpper())
                {
                    RetVal = ((JumpBoxQuestion)this.List[i]).Key;
                    break;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Returns value like Q1.a,Q1.b,...  etc.
        /// </summary>
        /// <param name="key">Question key or page no</param>
        /// <returns>Jump box question's value or page no</returns>
        public string GetValue(string key)
        {
            string RetVal = string.Empty;
            for (int i = 0; i < this.List.Count; i++)
            {
                if (((JumpBoxQuestion)this.List[i]).Key.ToUpper() == key.ToUpper())
                {
                    RetVal = ((JumpBoxQuestion)this.List[i]).Value;
                    break;
                }
            }
            return RetVal;
        }


        /// <summary>
        /// To Items Jump Box Question info 
        /// </summary>
        /// <param name="Index">zero base index number</param>
        /// <returns></returns>
        public JumpBoxQuestion Items(int Index)
        {

            return (JumpBoxQuestion)this.List[Index];
        }


        /// <summary>
        /// Remove jump box question  
        /// </summary>
        /// <param name="key">Question key or page no</param>
        public void Remove(string key)
        {
            for (int i = 0; i < this.List.Count; i++)
            {
                if (((JumpBoxQuestion)this.List[i]).Key.ToUpper() == key.ToUpper())
                {
                    this.List.RemoveAt(i);
                    break;
                }
            }
        }

        #endregion

        #endregion
    }

}



    

