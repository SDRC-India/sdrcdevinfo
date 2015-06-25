using System.Collections;
using System;
using System.Xml.Serialization;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public struct AreaInfo
    {
        //*** AreaName, DataValue, Subgroup and Time Shall be used for Label drawing
        //*** Enhancement 10 Oct 2006 Source
        public string IndicatorGID;
        public string UnitGID;
        public string SubgroupGID;
        //*** BugFix 21 Sep 2006 Subgroup label error
        public string Subgroup;
        public string Time;
        public string AreaName;
        public string Source;
        public decimal DataValue;

        //***DisplayInfo is added to allow textual dataValue to be displayed as part of Map Lable.
        public string DisplayInfo; // Variable to store Area textual dataValue string to be displayed as Label on Map.

        //Key(MD_SRC_1=Publisher)-Value(UNDP)
        [XmlIgnore()]
        public Hashtable MDFldVal;

        ////--This field is used for MDFldVal to get Serialized.
        //public HashtableSerializationProxy _XmlMDFldVal;

        //Below Property is used for "MDFldVal" object to get serialized
        [XmlElement("MDFldVal")]
        public HashtableSerializationProxy xmlMDFldVal
        {
            get
            {
                //At the time of serialization
                if (MDFldVal != null)
                {
                    return new HashtableSerializationProxy(MDFldVal);
                }
                else
                { return new HashtableSerializationProxy(); }
            }
            set
            {
                //At the time of Deserialization
                MDFldVal = value._hashTable;
            }
        }
        //*** Rendering Info contains either data value (dot density) or legend index(Color / Hatch Theme)
        //Integer
        public decimal RenderingInfo;

        
        //*** Chart Data Shall contain data for Chart theme. Comma dilimited Datavalues for Multiple Subgroup/Source , for each TimePeriods
        //-ChartData[Key, Value]
        // :Key - TimePeriod
        // :Value - Comma delimited dataValues for multiple Subgroups OR Source
        [XmlIgnore()]
        public Hashtable ChartData;

       
        /// <summary>
        /// Most recent data For current Chart.
        /// </summary>
        public string ChartMostRecentData;

        /// <summary>
        /// Copy of Most recent data For current Chart.
        /// </summary>
        public string ChartMostRecentDataCopy;

        /// <summary>
        /// This Property is used for "ChartData" property to get XML serialized
        /// </summary>
        [XmlElement("ChartData")]
        public HashtableSerializationProxy XmlChartData
        {
            get
            {
                //At the time of serialization
                if (ChartData != null)
                {
                    return new HashtableSerializationProxy(ChartData);
                }
                else
                { return new HashtableSerializationProxy(); }
            }
            set
            {
                //At the time of Deserialization
                this.ChartData = value._hashTable;
            }
        }
    }
}