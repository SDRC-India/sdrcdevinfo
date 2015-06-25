using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Map;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class MapInfo
    {

        private int _LayerNId = -1;

        public int LayerNId
        {
            get { return _LayerNId; }
            set { _LayerNId = value; }
        }

        private string _LayerSize;

        public string LayerSize
        {
            get { return _LayerSize; }
            set { _LayerSize = value; }
        }


        private string _MapFilePath;

        public string MapFilePath
        {
            get { return _MapFilePath; }
            set { _MapFilePath = value; }
        }


        private ShapeType _LayerType;

        public ShapeType LayerType
        {
            get { return _LayerType; }
            set { _LayerType = value; }
        }


        private bool _IsFeatureLayer = false;

        public bool IsFeatureLayer
        {
            get { return _IsFeatureLayer; }
            set { _IsFeatureLayer = value; }
        }


        private RectangleF _BoundingBox;
        /// <summary>
        /// MinX, MinY, MaxX, MaxY
        /// </summary>
        public RectangleF BoundingBox
        {
            get { return _BoundingBox; }
            set { _BoundingBox = value; }
        }


        private string _MapName;

        public string MapName
        {
            get { return _MapName; }
            set { _MapName = value; }
        }

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        private int _MetadataNId;

        public int MetadataNId
        {
            get { return _MetadataNId; }
            set { _MetadataNId = value; }
        }



        private DateTime _UpdateTimestamp;

        public DateTime UpdateTimestamp
        {
            get { return _UpdateTimestamp; }
            set { _UpdateTimestamp = value; }
        }

        //TODO properties agaginst all database columns
    }
}
