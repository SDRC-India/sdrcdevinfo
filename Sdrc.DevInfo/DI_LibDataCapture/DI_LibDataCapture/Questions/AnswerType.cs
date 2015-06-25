//' **********************************************************************

//' Program Name: FrmQuestions.vb

//' Developed By: DG6

//' Creation date: 

//' Program Comments: 

//' **********************************************************************

//' **********************Change history*********************************

//' No.   Mod:Date    Mod:By  Change Description 

//' c1    2007-09-11  DG6     :Add new question type- "Date" with the option to define the format of the date and validation by date ranges

//' **********************************************************************


using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
   
    /// <summary>
    /// Returns the AnsType
    /// </summary>
    [Serializable()]
    public enum AnswerType
    {
        /// <summary>
        /// Returns Textbox questions
        /// </summary>
        TB = 0,

        /// <summary>
        /// Returns DateType questions
        /// </summary>
        TBN = 1,

        /// <summary>
        /// Returns ComboBox questions
        /// </summary>
        CB = 2,

        /// <summary>
        /// Returns ComboBox-Numeric questions
        /// </summary>
        SCB = 3,

        /// <summary>
        /// Returns RadioButton questions
        /// </summary>
        RB = 4,

        /// <summary>
        /// Returns CheckBox-Numeric questions
        /// </summary>
        SRB = 5,

        /// <summary>
        /// Returns Checkbox type questions
        /// </summary>
        CH = 6,

        /// <summary>
        /// Returns Grid type questions
        /// </summary>
        GridType = 7,

        /// <summary>
        /// Returns Calculate questions
        /// </summary>
        Calculate=8,

        /// <summary>
        /// Returns Aggregate questions
        /// </summary>
        Aggregate=9,

        #region "-- Start Change for c1 --"
        /// <summary>
        /// Returns DateType questions
        /// </summary>
        DateType= 10

        #endregion 
    }
}
