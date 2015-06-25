using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL
{
    public static class MappingUtility
    {
        #region "-- Static --"

        #region "-- Cell Address --"

        /// <summary>
        /// Returns alpha part from cell address
        /// </summary>
        /// <param name="cellAddress"></param>
        /// <returns></returns>
        public static string GetAlphaPart(string cellAddress)
        {
            string RetVal = string.Empty;
            cellAddress = cellAddress.Replace(MappingConstants.Dollar, "");

            // check second character is numeric or not
            if (cellAddress.Substring(1, 1)[0] >= '0' & cellAddress.Substring(1, 1)[0] <= '9')
            {
                RetVal = cellAddress.Substring(0, 1);
            }
            else
            {
                RetVal = cellAddress.Substring(0, 2);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns numeric part from cell address
        /// </summary>
        /// <param name="cellAddress"></param>
        /// <returns></returns>
        public static string GetNumericPart(string cellAddress)
        {
            string RetVal = string.Empty;
            cellAddress = cellAddress.Replace(MappingConstants.Dollar, "");

            // check second character is numeric or not
            if (cellAddress.Substring(1, 1)[0] >= '0' & cellAddress.Substring(1, 1)[0] <= '9')
            {
                RetVal = cellAddress.Substring(1);
            }
            else
            {
                RetVal = cellAddress.Substring(2);
            }

            return RetVal;
        }

        /// <summary>
        /// To Get the Dyanmic Cell Address
        /// </summary>
        /// <param name="cellAddress">string CellAddress</param>
        /// <returns>string</returns>
        public static string GetDynamicCellAddress(string cellAddress)
        {
            // return Constants.Dollar + DataSource.GetAlphaPart ( cellAddress ) + Constants.Dollar + DataSource.GetNumericPart ( cellAddress );
            return MappingUtility.GetAlphaPart(cellAddress) + MappingUtility.GetNumericPart(cellAddress);
        }

        /// <summary>
        /// To Get the Static Cell Address
        /// </summary>
        /// <param name="cellAddress">string CellAddress</param>
        /// <returns>String</returns>
        public static string GetStaticCellAddress(string cellAddress)
        {
            //return DataSource.GetAlphaPart ( cellAddress ) + DataSource.GetNumericPart ( cellAddress );
            return MappingConstants.Dollar + MappingUtility.GetAlphaPart(cellAddress) + MappingConstants.Dollar + MappingUtility.GetNumericPart(cellAddress);
        }

        /// <summary>
        /// To check whether the address is Dynamic or Not
        /// </summary>
        /// <param name="cellAddress">Cell Address</param>
        /// <returns></returns>
        public static bool IsAddressDynamic(string cellAddress)
        {

            return !cellAddress.Contains(MappingConstants.Dollar);

        }

        #endregion

        
        #endregion
    }
}
