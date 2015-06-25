// ***********************Copy Right Notice*****************************
// 
// **********************************************************************
// Program Name:									       
// Developed By: DG6
// Creation date: 2007-05-31							
// Program Comments: Brief description of the purpose of the program      ‘ being written 
// **********************************************************************
// **********************Change history*********************************
// No.	Mod: Date	      Mod: By	       Change Description		        
// c1   2007.11.30          DG6             Added new types : footnote and denominator
//											          
// **********************************************************************


using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL
    {
    /// <summary>
    /// Used to Map the Cells
    /// </summary>
    public class DICellMapping
        {

        #region "-- private --"

        #region "-- New/Dispose --"

       

        #endregion

        #endregion

        #region "-- Public / Friend --"

        #region "-- Variables and Properties --"

        private Mapping _CellMap;
        /// <summary>
        /// Gets or Sets mapping information of cell.
        /// </summary>
        public Mapping CellMap
            {
            get
                {
                return this._CellMap;
                }
            set
                {
                this._CellMap = value;
                }

            }

        private string _CellAddress;
        /// <summary>
        /// Gets or Sets address of cell
        /// </summary>
        public string CellAddress
            {
            get { return _CellAddress; }
            set { _CellAddress = value; }
            }

        //public string CellValue
        private string _CellValue;
        /// <summary>
        /// Gets or Sets cell value
        /// </summary>
        public string CellValue
            {
            get
                {
                return this._CellValue;
                }
            set
                {
                this._CellValue = value;
                }
            }


        #endregion

        #region "-- New/Dispose --"

        public DICellMapping()
        {
            this.CellMap = new Mapping(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Intializes the instance of the DICellMapping Object
        /// </summary>
        /// <param name="cellAddress">Cell Address</param>
        /// <param name="cellValue">Cell Value</param>
        public DICellMapping ( string cellAddress, string cellValue ):this()
            {
            this.CellAddress = cellAddress;
            this.CellValue = cellValue;

            //////-- ***Start of Change no: c1
            ////this.CellMap = new Mapping ( string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            ////    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            //////-- ***End of Change no: c1

            }

        #endregion

        #region "-- Method --"

        /// <summary>
        /// Clear Cell Mapping Value
        /// </summary>
        public void ClearMapping()
        {
            this._CellMap.ClearMappedValues();
        }
        
        #endregion

        #endregion

    }
    }
