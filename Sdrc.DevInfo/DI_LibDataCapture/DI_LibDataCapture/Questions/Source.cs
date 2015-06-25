
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;



namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
    [Serializable()]
    public class Source 
    {
        #region "-- Private --"

        private string SourceXmlFileName = string.Empty; 

        private Source()
        {
            // donot implement this.
        }


        #endregion


        #region "-- Public/Internal --"

        
        /// <summary>
        /// Fill the list with source
        /// </summary>
        /// <param name="XmlSourceFile"></param>
        public void FillList(string XmlSourceFile)
        {            
            XmlTextReader XmlReader=new XmlTextReader(XmlSourceFile);
            while (XmlReader.Read())
            {
                if(XmlReader.HasAttributes & XmlReader.NodeType==System.Xml.XmlNodeType.Element)
                {
                    if(XmlReader[0].ToUpper() != "ISO")
                    {
                        this._SourceList.Add(XmlReader[1]);
                    }
                }
                XmlReader.ReadAttributeValue();
            }
        }

        #region "-- Variables / Properties --"

        private Question _SourceValue;
        /// <summary>
        /// Gets complete source
        /// </summary>
        public Question SourceValue
        {
            get
            {
                return this._SourceValue;
            }
        }
        

        private Question _Publisher;
        /// <summary>
        /// Gets publisher value
        /// </summary>
        public Question Publisher
        {
            get
            {
                return this._Publisher;
            }
        }
        
        private Question _Title;
        /// <summary>
        /// Gets title value.
        /// </summary>
        public Question Title
        {
            get
            {
                return this._Title;
            }
        }

        private Question _Year;
        /// <summary>
        /// Gets year value.
        /// </summary>
        public Question Year
        {
            get
            {
                return this._Year;
            }
        }

        private List<string> _SourceList = new List<string>();
        /// <summary>
        /// Gets the Source List
        /// </summary>
        public List<string> SourceList
        {
            get
            {                
                return this._SourceList;
            }
        }

        #endregion

        #region "-- New/Dispose --"

        internal Source(DataSet xmlDataSet,string xmlFileName)
        {
            string Publisher = string.Empty;
            string Title = string.Empty;
            string Year = string.Empty;
            int IndexOfSeparator = -1;

            // get source question
            this._SourceValue = Questionnarie.GetQuestion(xmlDataSet, MandatoryQuestionsKey.SorucePublisher);

            this._Publisher = Questionnarie.GetQuestion(xmlDataSet, MandatoryQuestionsKey.SorucePublisher);
            this._Title = Questionnarie.GetQuestion(xmlDataSet, MandatoryQuestionsKey.SourceTitle);
            this._Year = Questionnarie.GetQuestion(xmlDataSet, MandatoryQuestionsKey.SourceYear);
            FillList(xmlFileName);

            //reset publisher,title and year values
            if (!string.IsNullOrEmpty(this._SourceValue.DataValue))
            {
                string SourceString = string.Empty;
                SourceString = this.SourceValue.DataValue;
                IndexOfSeparator = SourceString.IndexOf(Constants.SourceSeparator);
                
                if(IndexOfSeparator>0)
                {
                    Publisher = SourceString.Substring(0,IndexOfSeparator);

                    // get year 
                    Year = SourceString.Substring(SourceString.LastIndexOf(Constants.SourceSeparator)+ 1);

                    // get title                    
                    Title= SourceString.Replace(Publisher +"_","").Replace("_"+Year,"");
                    
                    

                }

                this._Publisher.DataValue = Publisher;
                this._Title.DataValue = Title;
                this._Year.DataValue = Year;
            }

            
        }

        #endregion

        #endregion

    }
}
