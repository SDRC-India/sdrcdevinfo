using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;

namespace DevInfo.Lib.DI_LibBAL.Controls.ListViewBAL
{
    /// <summary>
    /// Initialize the instance of BaseListViewSource. Based on Factory design pattern
    /// </summary>
    public static class ListViewSourceFactory
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns the instance of BaseListViewSource. Based on Factory method.
        /// </summary>
        /// <param name="listType"></param>
        /// <param name="showCheckBoxColumn"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static BaseListViewSource CreateListViewSource(ListViewType listType, DIConnection dbConnection, DIQueries dbQueries)
        {
            BaseListViewSource RetVal = null;
            UserPreference DIUserPreferencce = new UserPreference();

            RetVal = ListViewSourceFactory.CreateListViewSource(listType, dbConnection, dbQueries, DIUserPreferencce);

            return RetVal;
        }

        /// <summary>
        /// Returns the instance of BaseListViewSource. Based on Factory method.
        /// </summary>
        /// <param name="listType"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="userprefenece"></param>
        /// <returns></returns>
        public static BaseListViewSource CreateListViewSource(ListViewType listType, DIConnection dbConnection, DIQueries dbQueries, UserPreference userprefenece)
        {
            BaseListViewSource RetVal = null;

            switch (listType)
            {
                case ListViewType.Indicator:
                    RetVal = new IndicatorListViewSource();
                    RetVal.RaiseInitializedIndicatorListViewEvent();
                    break;

                case ListViewType.Timeperiod:
                    RetVal = new TimeperiodListViewSource();
                    RetVal.RaiseInitializedTimperiodListViewEvent();
                    break;

                case ListViewType.Area:
                    RetVal = new AreaListViewSource();
                    RetVal.RaiseInitializedAreaListViewEvent();
                    break;

                case ListViewType.Source:
                    RetVal = new DISourceListViewSource();
                    RetVal.RaiseInitializedDISourceListViewEvent();
                    break;

                case ListViewType.Unit:
                    RetVal = new UnitListViewSource();
                    RetVal.RaiseInitializedUnitListViewEvent();
                    break;

                case ListViewType.SubgroupVal:
                    RetVal = new SubgroupValListViewSource();
                    RetVal.RaiseInitializedUnitListViewEvent();
                    break;

                case ListViewType.IC:
                    RetVal = new ICListViewSource();
                    break;

                default:
                    break;
            }

            if (RetVal != null)
            {
                //set variables
                RetVal.DBConnection = dbConnection;
                RetVal.DBQueries = dbQueries;
                RetVal.DIUserPreference = userprefenece;

                //set columns
                RetVal.SetColumnInfo();
            }

            return RetVal;
        }

        #endregion

        #endregion
        
    }
}
