using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL
{
    public class DIExcelCellMapping : DICellMapping
    {

        #region "--New/Dispose--"

        public DIExcelCellMapping():base()
        {
            this.CurrentCellMappingType = CellMappingType.DataValueMapping;
            this.CurrentSheetType = SheetType.SimpleDataSheet;
            
        
        }


        public DIExcelCellMapping(CellMappingType cellType, SheetType sheetType, string cellAddress, string cellValue): base(cellAddress,cellValue)
        {
            this.CurrentCellMappingType = cellType;
            this.CurrentSheetType = sheetType;
            this.CellAddress = cellAddress;
        }
        
        #endregion


        #region "-- Variables/Properties--"


        private CellMappingType _CurrentCellMappingType = CellMappingType.DataValueMapping;
        /// <summary>
        /// Gets or sets selected cell mapping type
        /// </summary>
        public CellMappingType CurrentCellMappingType
        {
            get
            {
                return this._CurrentCellMappingType;
            }
            set
            {
                this._CurrentCellMappingType = value;
                //this.AddExcelCellMappingTypeInfo();
                //EnableSubgroupDimensionValues();

            }
        }


        private SheetType _CurrentSheetType = SheetType.SimpleDataSheet;
        /// <summary>
        /// Gets or sets the sheet type for current sheet
        /// </summary>
        public SheetType CurrentSheetType
        {
            get
            {
                return this._CurrentSheetType;
            }
            set
            {
                this._CurrentSheetType = value;
                

            }
        }

       
    




        #endregion

        #region "-- Methods--"
        //public void AddExcelCellMappingTypeInfo()
        //{
        //    if (!String.IsNullOrEmpty(this.CellAddress))
        //    {
                
        //        if (!this.CellMappingTypeInfoCollection.ContainsKey(this.CellAddress))
        //        {
        //            this.CellMappingTypeInfoCollection.Add(this.CellAddress, this._CurrentCellMappingType);
        //        }
        //        else
        //        {
        //            this.CellMappingTypeInfoCollection[this.CellAddress] = _CurrentCellMappingType;
        //        }
        //    }

        //}


        
        #endregion

    }
}
