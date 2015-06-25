using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    internal static class Constants
    {
        internal static class SubgroupType
        {
            internal const string Total = "Total";
            internal const string Location = "Location";
            internal const string Sex = "Sex";
            internal const string Age = "Age";
            internal const string Other = "Other";

            internal static class Order
            {
                internal const int Total = 0;
                internal const int Sex = 1;
                internal const int Age = 2;                
                internal const int Location = 3;
                internal const int Other = 4;
            }
        }
        internal const string PrefixForNewValue = "#";
        
        internal static class Versions
        { 
            internal static string DI6_0_0_1="6.0.0.1";
            internal static string DI6_0_0_2="6.0.0.2";
            internal static string DI6_0_0_3 = "6.0.0.3";
            internal static string DI6_0_0_4 = "6.0.0.4";
            internal static string DI6_0_0_5 = "6.0.0.5";
            internal static string DI7_0_0_0 = "7.0.0.0";
            internal static string DI7_0_0_1 = "7.0.0.1";
        }

        internal static class VersionsChangedDates
        {
            internal static string DI6_0_0_1 = "1 July 2008";
            internal static string DI6_0_0_2 = "6 Oct 2008";
            internal static string DI6_0_0_3 = "27 May 2009";
            internal static string DI6_0_0_4 = "7 Sept 2009";
            internal static string DI6_0_0_5 = "25 May 2010";
            internal static string DI7_0_0_0 = "9 Feb 2012";
            internal static string DI7_0_0_1 = "10 Oct 2012";
        }

        internal static class DBFilePostFix
        {
            internal static string DI6_0_0_1 = "_v6";
            internal static string DI6_0_0_2 = "_v6";
            internal static string DI6_0_0_3 = "_v6";
            internal static string DI6_0_0_4 = "_v6";
            internal static string DI6_0_0_5 = "_v6";
            internal static string DI7_0_0_0 = "_v7";
            internal static string DI7_0_0_1 = "_v7";
            
            //internal static string DI7 = "_v7";
        }

        internal static class VersionComments
        {
            internal static string DI6_0_0_1 = string.Empty;
            internal static string DI6_0_0_2 = "Introduced Database Metadata Table";
            internal static string DI6_0_0_3 = "Introduced Recommended sources Table & added IC_IUS_Order column in UT_Data table";
            internal static string DI6_0_0_4 = "Data type of Data_NId column under Recommended sources Table changed to Long Integer and logic has been added to delete empty records from UT_Recommended_sources table";
            internal static string DI6_0_0_5 = "Metadata_Category table added & order columns inserted under IC, Subgroup, Subgroup_Val tables & new columns added in Indicator, IC, IUS & Area column and document table added ";
            internal static string DI7_0_0_0 = "DI 7 related chnages made, Metadata_Category table modified & new columns added in Data,Timeperiod,Area and IndicatorClassification table";
            internal static string DI7_0_0_1 = "DI 7 related chnages made, periodicity column data type updated from Number to Text";
        }


        internal static class SGTotalConstants
        {
            internal static string ForEnglish = "Total";
            internal static string ForFrench = "Total";
            internal static string ForChinese = "Total";
            internal static string ForRussian = "Total";
            internal static string ForSpanish = "Total";
            internal static string ForArabic = "Total";
            internal static string ForWithPrefix  = "#Total";
        }

        /// <summary>
        /// For DI7 Map server Encryption/Decryption Only
        /// </summary>
        internal static class EncryptDecryptConstants
        {
            /// <summary>
            /// For DI7 Map server Encryption/Decryption Only
            /// </summary>
            internal static string EncryptionKey = "<\"}#$7#%";
        }
    }
}
