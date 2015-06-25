using System;
//using System.Runtime.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public enum BufferStyle
    {
        Point = 0,
        Line = 1
    }

    [Serializable()]
    public enum LayoutType
    {
        Square = 0,
        Tall = 1,
        Wide = 2
    }

    [Serializable()]
    public enum ThemeType
    {
        Color = 0,
        DotDensity = 1,
        Chart = 2,
        Hatch = 3,
        Symbol = 4,
        Label = 5
    }

    [Serializable()]
    public enum ChartType
    {
        Column = 0,
        //*** associated with optBar in frmMapTheme
        Pie = 1,
        //*** associated with optPie in frmMapTheme
        Line = 2
        //*** associated with optLine in frmMapTheme

        //Area = 3,
        //Bar3D = 4
        //Column,Cone,Pyramid,
    }

    [Serializable()]
    public enum ChartSize
    {
        ValueBased = 0,
        //*** associated with optValue in frmMapTheme
        ShapeBased = 1
        //*** associated with optShape in frmMapTheme
    }

    [Serializable()]
    public enum ChartSeriesType
    {
        /// <summary>
        /// If Chart's dataValue is to be drawn group by subgroups.
        /// </summary>
        Subgroup = 0,
        /// <summary>
        /// If Chart's dataValue is to be drawn group by Sources.
        /// </summary>
        Source = 1
    }

    [Serializable()]
    public enum ShapeType
    {
        PointFeature = 0,
        Point = 1,
        PolyLineFeature = 2,
        PolyLine = 3,
        PolygonFeature = 4,
        Polygon = 5,
        PointCustom = 6,
        PolyLineCustom = 7,
        PolygonCustom = 8,
        PolygonBuffer = 9
    }

    [Serializable()]
    public enum BreakType
    {
        EqualCount = 0,
        EqualSize = 1,
        Continuous = 2,
        Discontinuous = 3
    }

    [Serializable()]
    public enum MarkerStyle
    {
        Circle = 0,
        Square = 1,
        Triangle = 2,
        Cross = 3,
        Custom = 4
    }

    [Serializable()]
    public enum LabelEffect
    {
        Shadow = 0,
        Embossed  = 1,
        Block = 2,
        Gradient = 3,
        Reflect = 4
    }

    [Serializable()]
    public enum LabelCase
    {
        Regular = 0,
        UpperCase = 1,
        LowerCase = 2
    }

    [Serializable()]
    public enum FillStyle
    {
        Solid = 1000,
        Transparent = 2000,

        Horizontal = HatchStyle.Horizontal,
        //0
        Vertical = HatchStyle.Vertical,
        //1
        ForwardDiagonal = System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal,
        //2
        BackwardDiagonal = System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal,
        //3
        Cross = System.Drawing.Drawing2D.HatchStyle.Cross,
        //4
        LargeGrid = System.Drawing.Drawing2D.HatchStyle.LargeGrid,
        //4
        DiagonalCross = System.Drawing.Drawing2D.HatchStyle.DiagonalCross,
        //5
        Percent05 = System.Drawing.Drawing2D.HatchStyle.Percent05,
        //6
        Percent10 = System.Drawing.Drawing2D.HatchStyle.Percent10,
        //7
        Percent20 = System.Drawing.Drawing2D.HatchStyle.Percent20,
        //8
        Percent25 = System.Drawing.Drawing2D.HatchStyle.Percent25,
        //9

        //1
        Percent30 = System.Drawing.Drawing2D.HatchStyle.Percent30,
        //10
        Percent40 = System.Drawing.Drawing2D.HatchStyle.Percent40,
        //11
        Percent50 = System.Drawing.Drawing2D.HatchStyle.Percent50,
        //12
        Percent60 = System.Drawing.Drawing2D.HatchStyle.Percent60,
        //13
        Percent70 = System.Drawing.Drawing2D.HatchStyle.Percent70,
        //14
        Percent75 = System.Drawing.Drawing2D.HatchStyle.Percent75,
        //15
        Percent80 = System.Drawing.Drawing2D.HatchStyle.Percent80,
        //16
        Percent90 = System.Drawing.Drawing2D.HatchStyle.Percent90,
        //17
        LightDownwardDiagonal = System.Drawing.Drawing2D.HatchStyle.LightDownwardDiagonal,
        //18
        LightUpwardDiagonal = System.Drawing.Drawing2D.HatchStyle.LightUpwardDiagonal,
        //19

        //2
        DarkDownwardDiagonal = System.Drawing.Drawing2D.HatchStyle.DarkDownwardDiagonal,
        //20
        DarkUpwardDiagonal = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal,
        //21
        WideDownwardDiagonal = System.Drawing.Drawing2D.HatchStyle.WideDownwardDiagonal,
        //22
        WideUpwardDiagonal = System.Drawing.Drawing2D.HatchStyle.WideUpwardDiagonal,
        //23
        LightVertical = System.Drawing.Drawing2D.HatchStyle.LightVertical,
        //24
        LightHorizontal = System.Drawing.Drawing2D.HatchStyle.LightHorizontal,
        //25
        NarrowVertical = System.Drawing.Drawing2D.HatchStyle.NarrowVertical,
        //26
        NarrowHorizontal = System.Drawing.Drawing2D.HatchStyle.NarrowHorizontal,
        //27
        DarkVertical = System.Drawing.Drawing2D.HatchStyle.DarkVertical,
        //28
        DarkHorizontal = System.Drawing.Drawing2D.HatchStyle.DarkHorizontal,
        //29

        //3
        DashedDownwardDiagonal = System.Drawing.Drawing2D.HatchStyle.DashedDownwardDiagonal,
        //30
        DashedUpwardDiagonal = System.Drawing.Drawing2D.HatchStyle.DashedUpwardDiagonal,
        //31
        DashedHorizontal = System.Drawing.Drawing2D.HatchStyle.DashedHorizontal,
        //32
        DashedVertical = System.Drawing.Drawing2D.HatchStyle.DashedVertical,
        //33
        SmallConfetti = System.Drawing.Drawing2D.HatchStyle.SmallConfetti,
        //34
        LargeConfetti = System.Drawing.Drawing2D.HatchStyle.LargeConfetti,
        //35
        ZigZag = System.Drawing.Drawing2D.HatchStyle.ZigZag,
        //36
        Wave = System.Drawing.Drawing2D.HatchStyle.Wave,
        //37
        DiagonalBrick = System.Drawing.Drawing2D.HatchStyle.DiagonalBrick,
        //38
        HorizontalBrick = System.Drawing.Drawing2D.HatchStyle.HorizontalBrick,
        //39

        //4
        Weave = System.Drawing.Drawing2D.HatchStyle.Weave,
        //40
        Plaid = System.Drawing.Drawing2D.HatchStyle.Plaid,
        //41
        Divot = System.Drawing.Drawing2D.HatchStyle.Divot,
        //42
        DottedGrid = System.Drawing.Drawing2D.HatchStyle.DottedGrid,
        //43
        DottedDiamond = System.Drawing.Drawing2D.HatchStyle.DottedDiamond,
        //44
        Shingle = System.Drawing.Drawing2D.HatchStyle.Shingle,
        //45
        Trellis = System.Drawing.Drawing2D.HatchStyle.Trellis,
        //46
        Sphere = System.Drawing.Drawing2D.HatchStyle.Sphere,
        //47
        SmallGrid = System.Drawing.Drawing2D.HatchStyle.SmallGrid,
        //48
        SmallCheckerBoard = System.Drawing.Drawing2D.HatchStyle.SmallCheckerBoard,
        //49

        //5
        LargeCheckerBoard = System.Drawing.Drawing2D.HatchStyle.LargeCheckerBoard,
        //50
        OutlinedDiamond = System.Drawing.Drawing2D.HatchStyle.OutlinedDiamond,
        //51
        SolidDiamond = System.Drawing.Drawing2D.HatchStyle.SolidDiamond
        //52

    }

    [Serializable()]
    public enum SourceType
    {
        Shapefile = 0,
        Database = 1
        //MapInfo = 2
        //BMP = 3
        //GeoTiff = 4
        //Jpeg = 5
    }

    [Serializable()]
    public enum LabelFieldType
    {
        AreaID = 0,
        AreaName = 1,
        DataValue = 2,
        Subgroup = 3,
        Unit = 4,
        Time = 5
    }

    [Serializable()]
    public enum ScaleUnit
    {
        KM = 0,
        Miles = 1,
        Meters = 2,
        Feet = 3
    }

    [Serializable()]
    public enum GoogleMapFileType
    {
        KML = 0,
        KMZ = 1
    }

    /// <summary>
    /// enums for Legends sequence order in Legend Image.
    /// </summary>
    public enum LegendsSequenceOrder
    {
        SingleRow = 0,
        SingleColumn = 1
    }

    /// <summary>
    /// enums for composite image order for Themes Legend image.
    /// </summary>
    public enum CompositeLegendImageOrder
    {
        Horizontal = 0,
        Vertical = 1
    }

}