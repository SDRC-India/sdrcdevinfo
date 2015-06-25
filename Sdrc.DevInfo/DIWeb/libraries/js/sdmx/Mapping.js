// JScript File
var ErrorMessage = "";
var MappingType = '';
var SelectOneOption = '';
var MappingFileGenerateStatus = '';
var MapOneCode = '';
var MapOneConcept = '';
var AlreadyMappedConcept = '';
var SelectAllDropdowns = '';
var SelectOneMappedRow = '';
var PreviousMappingsRemoved = '';
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;

    createFormHiddenInputs("frm_sdmxMapping", "POST");
    SetCommonLinksHref("frm_sdmxMapping", "POST");
    SetOriginaldbnidInForm("frm_sdmxMapping", hOriginaldbnid);

    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegMapping.aspx', "frm_sdmxMapping", "POST");
    BindAreaLevels();
    ShowHideMappingdivs("divCodelistMapping");
    ShowHideMappings("divCodelistMapping");

    LanguageHandlingForAlerts();

}

function LanguageHandlingForAlerts() {
    SelectOneOption = document.getElementById('hSelectOneOption').value;
    MappingFileGenerateStatus = document.getElementById('hMappingFileGenerateStatus').value;
    MapOneCode = document.getElementById('hMapOneCode').value;
    MapOneConcept = document.getElementById('hMapOneConcept').value;
    AlreadyMappedConcept = document.getElementById('hAlreadyMappedConcept').value;
    SelectAllDropdowns = document.getElementById('hSelectAllDropdowns').value;
    SelectOneMappedRow = document.getElementById('hSelectOneMappedRow').value;
    PreviousMappingsRemoved = document.getElementById('hPreviousMappingsRemoved').value;
}

function ShowHideMappingdivs(divMapping) {
    if (divMapping == "divCodelistMapping") {
        z("divCodelistMapping").style.display = "";
        z("divIUSMappingOuter").style.display = "none";
        z("divMetadataMappingOuter").style.display = "none";
        z("aCodelist").className = "adm_lft_nav_seld";
        z("aIUS").className = "adm_lft_nav";
        z("aMetadata").className = "adm_lft_nav";

        BindMappedCodelists(true, null);
        // BindCodelistMappingLists(true, null);
    }
    else if (divMapping == "divMetadataMappingOuter") {
        z("divCodelistMapping").style.display = "none";
        z("divIUSMappingOuter").style.display = "none";
        z("divMetadataMappingOuter").style.display = "";
        z("aCodelist").className = "adm_lft_nav";
        z("aIUS").className = "adm_lft_nav";
        z("aMetadata").className = "adm_lft_nav_seld";

        BindMetadataMappingList(true);
    }
    else if (divMapping == "divIUSMappingOuter") {
        z("divCodelistMapping").style.display = "none";
        z("divIUSMappingOuter").style.display = "";
        z("divMetadataMappingOuter").style.display = "none";
        z("aCodelist").className = "adm_lft_nav";
        z("aIUS").className = "adm_lft_nav_seld";
        z("aMetadata").className = "adm_lft_nav";

        BindIUSMappingList(true);

    }

}
// this method show hide mapping based on input parameter
// also sets importexport href based on mapping input parameter
// in method ExportMappingExcel and OpenUploadExcellPopup pass mapping type parameter for further calculations
// 0-- Codelist, 1-- IUS, 2-- Metadata
function ShowHideMappings(divMapping) {
    if (divMapping == "divCodelistMapping") {
        z("divCodelistMapping").style.display = "";
        z("divIUSMappingOuter").style.display = "none";
        z("divMetadataMappingOuter").style.display = "none";
        z("aCodelist").className = "maping_type_nav_seld";
        z("aIUS").className = "";
        z("aMetadata").className = "";

        BindMappedCodelists(true, null);

        // set export href
        z("imgExportExcel").href = "javascript:ExportMappingExcel(0);"; // in case of codelist pass mapping type = 0
        z("lnkExportMapExcel").href = "javascript:ExportMappingExcel(0);"; // in case of codelist pass mapping type = 0      
        // set import href
        z('imgImportMapExcel').href = "javascript:OpenUploadExcellPopup(0);"; // in case of codelist pass mapping type = 0
        z('lnkImportMapExcel').href = "javascript:OpenUploadExcellPopup(0);"; // in case of codelist pass mapping type = 0

    }
    else if (divMapping == "divMetadataMappingOuter") {
        z("divCodelistMapping").style.display = "none";
        z("divIUSMappingOuter").style.display = "none";
        z("divMetadataMappingOuter").style.display = "";
        z("aCodelist").className = "";
        z("aIUS").className = "";
        z("aMetadata").className = "maping_type_nav_seld";

        BindMetadataMappingList(true);
        // set export href
        z("imgExportExcel").href = "javascript:ExportMappingExcel(2);"; // in case of Metadata pass mapping type = 2
        z("lnkExportMapExcel").href = "javascript:ExportMappingExcel(2);"; // in case of Metadata pass mapping type = 2
        // set import href
        z('imgImportMapExcel').href = "javascript:OpenUploadExcellPopup(2);"; // in case of Metadata pass mapping type = 2
        z('lnkImportMapExcel').href = "javascript:OpenUploadExcellPopup(2);"; // in case of Metadata pass mapping type = 2
    }
    else if (divMapping == "divIUSMappingOuter") {
        z("divCodelistMapping").style.display = "none";
        z("divIUSMappingOuter").style.display = "";
        z("divMetadataMappingOuter").style.display = "none";
        z("aCodelist").className = "";
        z("aIUS").className = "maping_type_nav_seld";
        z("aMetadata").className = "";

        BindIUSMappingList(true);
        // set export href
        z("imgExportExcel").href = "javascript:ExportMappingExcel(1);"; // in case of ius pass mapping type = 1
        z("lnkExportMapExcel").href = "javascript:ExportMappingExcel(1);"; // in case of ius pass mapping type = 1
        // set import href
        z('imgImportMapExcel').href = "javascript:OpenUploadExcellPopup(1);"; // in case of ius pass mapping type = 1
        z('lnkImportMapExcel').href = "javascript:OpenUploadExcellPopup(1);"; // in case of ius pass mapping type = 1
    }

}

function BindCodelistMappingLists(hideMaskingLoading, hdnSelectedCodelist) {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    z('divIndicatorMapping').style.display == "block";
    z('divUnitMapping').style.display == "block";
    z('divAgeMapping').style.display == "block";
    z('divSexMapping').style.display == "block";
    z('divLocationMapping').style.display == "block";
    z('divAreaMapping').style.display == "block";
    var dIndicator, dUnit, dArea, dAge, dSex, dLocation, ageCodeListId, sexCodeListId, locationCodelistId;
    dIndicator = true, dUnit = true, dArea = true, dAge = false, dSex = false, dLocation = false;
    var divHtml = '';
    var InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + di_jq("#selectAreaLevel").val();
    if (hdnSelectedCodelist == 'aAgeCodelistSelect') {
        InputParam += ParamDelimiter + z('hdnSelectedAgeCodelist').value;
        InputParam += ParamDelimiter + 'aAgeCodelistSelect';
    }
    else if (hdnSelectedCodelist == 'aSexCodelistSelect') {
        InputParam += ParamDelimiter + z('hdnSelectedSexCodelist').value;
        InputParam += ParamDelimiter + 'aSexCodelistSelect';
    }
    else if (hdnSelectedCodelist == 'aLocationCodelistSelect') {
        InputParam += ParamDelimiter + z('hdnSelectedLocationCodelist').value;
        InputParam += ParamDelimiter + 'aLocationCodelistSelect';
    }

    var htmlResp = d.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=127&param1=" + InputParam,
        async: hideMaskingLoading,
        success: function (data) {
            try {
                divHtml = decodeURIComponent(data);
                divHtml = ReplaceAll(divHtml, "+", " ");
                divHtml = ReplaceAll(divHtml, "$", "USD");

                var dataValues = divHtml.split(ParamDelimiter);
                if (dataValues.length > 3) {
                    dAge = true, dSex = true, dLocation = true;
                    GetAllMappingList(dataValues[0], dataValues[1], dataValues[2], dataValues[3], dataValues[4], dataValues[5]);
                    ageCodeListId = dataValues[6]; sexCodeListId = dataValues[7]; locationCodelistId = dataValues[8];
                    SetSelectedCodelistsInSpan(ageCodeListId, sexCodeListId, locationCodelistId);
                    ApplyCustomSelect(dIndicator, dUnit, dArea, dAge, dSex, dLocation);
                }
                else if (dataValues.length == 3) {
                    if (dataValues[0] != undefined && dataValues[1] != undefined) {
                        z('divIndicatorMapping').innerHTML = dataValues[0];
                        z('divUnitMapping').innerHTML = dataValues[1];
                        z('divAreaMapping').innerHTML = dataValues[2];
                        AddOptionsToDdlPerRow('tblIndicator', 'ddlUNSDIndicator');
                        AddOptionsToDdlPerRow('tblUnit', 'ddlUNSDUnit');
                        AddOptionsToDdlPerRow('tblArea', 'ddlUNSDArea');
                        new di_drawSearchBox('divIndicatorSearch', 'txtIndicatorSearch', '', '', z('hSearchIndicator').value, 'FilterRowsByTextSearch(\'tblIndicator\', \'aShowIndicatorAll\', \'aShowIndicatorMapped\', \'aShowIndicatorUnMapped\', \'aShowIndicatorUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblIndicator\', \'aShowIndicatorAll\', \'aShowIndicatorMapped\', \'aShowIndicatorUnMapped\', \'aShowIndicatorUnSaved\', this);');
                        new di_drawSearchBox('divUnitSearch', 'txtUnitSearch', '', '', z('hSearchUnit').value, 'FilterRowsByTextSearch(\'tblUnit\', \'aShowUnitAll\', \'aShowUnitMapped\', \'aShowUnitUnMapped\', \'aShowUnitUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblUnit\', \'aShowUnitAll\', \'aShowUnitMapped\', \'aShowUnitUnMapped\', \'aShowUnitUnSaved\', this);');
                        new di_drawSearchBox('divAreaSearch', 'txtAreaSearch', '', '', z('hSearchArea').value, 'FilterRowsByTextAreaSearch(\'tblArea\', \'aShowAreaAll\', \'aShowAreaMapped\', \'aShowAreaUnMapped\', \'aShowAreaUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblArea\', \'aShowAreaAll\', \'aShowAreaMapped\', \'aShowAreaUnMapped\', \'aShowAreaUnSaved\', this);');

                    }
                }
                else {
                    if (dataValues[0] != undefined && hdnSelectedCodelist == 'aAgeCodelistSelect') {
                        z('divAgeMapping').innerHTML = dataValues[0];
                        dIndicator = false, dUnit = false, dArea = false, dAge = true;

                        d("#divAgeMapping").find('select.cus_slct_dd').each(function (index, domelem) {
                            var eleid = this.id;

                            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDAge') });
                        });

                        new di_drawSearchBox('divAgeSearch', 'txtAgeSearch', '', '', z('hSearchAge').value, 'FilterRowsByTextSearch(\'tblAge\', \'aShowAgeAll\', \'aShowAgeMapped\', \'aShowAgeUnMapped\', \'aShowAgeUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblAge\', \'aShowAgeAll\', \'aShowAgeMapped\', \'aShowAgeUnMapped\', \'aShowAgeUnSaved\', this);');
                    }
                    if (dataValues[0] != undefined && hdnSelectedCodelist == 'aSexCodelistSelect') {
                        dIndicator = false, dUnit = false, dArea = false, dSex = true; //changesd false to true   dIndicator = true, dUnit = true, dArea = true
                        z('divSexMapping').innerHTML = dataValues[0];


                        d("#divSexMapping").find('select.cus_slct_dd').each(function (index, domelem) {
                            var eleid = this.id;

                            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDSex') });
                        });
                        new di_drawSearchBox('divSexSearch', 'txtSexSearch', '', '', z('hSearchSex').value, 'FilterRowsByTextSearch(\'tblSex\', \'aShowSexAll\', \'aShowSexMapped\', \'aShowSexUnMapped\', \'aShowSexUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblSex\', \'aShowSexAll\', \'aShowSexMapped\', \'aShowSexUnMapped\', \'aShowSexUnSaved\', this);');

                    }
                    if (dataValues[0] != undefined && hdnSelectedCodelist == 'aLocationCodelistSelect') {
                        dIndicator = false, dUnit = false, dArea = false, dLocation = true;
                        z('divLocationMapping').innerHTML = dataValues[0];
                        d("#divLocationMapping").find('select.cus_slct_dd').each(function (index, domelem) {
                            var eleid = this.id;

                            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDLocation') });
                        });
                        new di_drawSearchBox('divLocationSearch', 'txtLocationSearch', '', '', z('hSearchLocation').value, 'FilterRowsByTextSearch(\'tblLocation\', \'aShowLocationAll\', \'aShowLocationMapped\', \'aShowLocationUnMapped\', \'aShowLocationUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblLocation\', \'aShowLocationAll\', \'aShowLocationMapped\', \'aShowLocationUnMapped\', \'aShowLocationUnSaved\', this);');
                    }
                }
                if (hideMaskingLoading) {
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }

                LanguageHandlingOfCodelistMappingDivs(dIndicator, dUnit, dArea, dAge, dSex, dLocation);

            }
            catch (ex) {
                alert("Error : " + ex.message);

                if (hideMaskingLoading) {
                    HideLoadingDiv();

                }
            }
        },
        error: function () {
            if (hideMaskingLoading) {
                HideLoadingDiv();
                RemoveMaskingDiv();
            }
        },
        cache: false
    });
}


function BindMappedCodelists(hideMaskingLoading, hdnSelectedCodelist) {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    var dIndicator, dUnit, dArea, dAge, dSex, dLocation, ageCodeListId, sexCodeListId, locationCodelistId;
    dIndicator = false, dUnit = false, dArea = false, dAge = false, dSex = false, dLocation = false;
    var divHtml = '';
    var InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + di_jq("#selectAreaLevel").val();
    if (hdnSelectedCodelist == 'aAgeCodelistSelect') {
        InputParam += ParamDelimiter + z('hdnSelectedAgeCodelist').value;
        InputParam += ParamDelimiter + 'aAgeCodelistSelect';
    }
    else if (hdnSelectedCodelist == 'aSexCodelistSelect') {
        InputParam += ParamDelimiter + z('hdnSelectedSexCodelist').value;
        InputParam += ParamDelimiter + 'aSexCodelistSelect';
    }
    else if (hdnSelectedCodelist == 'aLocationCodelistSelect') {
        InputParam += ParamDelimiter + z('hdnSelectedLocationCodelist').value;
        InputParam += ParamDelimiter + 'aLocationCodelistSelect';
    }
    var htmlResp = d.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=127&param1=" + InputParam,
        async: hideMaskingLoading,
        success: function (data) {
            try {
                divHtml = decodeURIComponent(data);
                divHtml = ReplaceAll(divHtml, "+", " ");
                divHtml = ReplaceAll(divHtml, "$", "USD");
                var IndicatorHtml = '', UnitHtml = '', AreaHtml = '';
                var dataValues = divHtml.split(ParamDelimiter);
                if (dataValues.length > 3) {
                    dAge = true, dSex = true, dLocation = true;
                    if (dataValues[0] != null) {
                        dIndicator = true;
                        IndicatorHtml = dataValues[0];
                    }
                    else {
                        dIndicator = false;
                        IndicatorHtml = null;
                    }
                    if (dataValues[1] != null) {
                        dUnit = true;
                        UnitHtml = dataValues[1];
                    }
                    else {
                        dUnit = false;
                        UnitHtml = null;
                    } if (dataValues[5] != null) {
                        dArea = true;
                        AreaHtml = dataValues[5];
                    }
                    else {
                        dArea = false;
                        AreaHtml = null;
                    }
                    GetAllMappingList(IndicatorHtml, UnitHtml, dataValues[2], dataValues[3], dataValues[4], AreaHtml);
                    ageCodeListId = dataValues[6]; sexCodeListId = dataValues[7]; locationCodelistId = dataValues[8];
                    SetSelectedCodelistsInSpan(ageCodeListId, sexCodeListId, locationCodelistId);
                    ApplyCustomSelect(dIndicator, dUnit, dArea, dAge, dSex, dLocation);

                }

                if (hideMaskingLoading) {
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
                LanguageHandlingOfCodelistMappingDivs(dIndicator, dUnit, dArea, dAge, dSex, dLocation)

            }
            catch (ex) {
                alert("Error : " + ex.message);

                if (hideMaskingLoading) {
                    HideLoadingDiv();
                 }
            }
        },
        error: function () {
             if (hideMaskingLoading) {
                HideLoadingDiv();
                RemoveMaskingDiv();
            }
        },
        cache: false
    });

}




function BindIndicatorCodelist(hideMaskingLoading) {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    var dIndicator, dUnit, dArea, dAge, dSex, dLocation;
    dIndicator = true, dUnit = false, dArea = false, dAge = false, dSex = false, dLocation = false;
    if (z('tblIndicator') == null) {
        var InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        var htmlResp = d.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=296&param1=" + InputParam,
            async: hideMaskingLoading,
            success: function (data) {
                try {
                    divHtml = decodeURIComponent(data);
                    divHtml = ReplaceAll(divHtml, "+", " ");
                    divHtml = ReplaceAll(divHtml, "$", "USD");

                    var dataValues = divHtml.split(ParamDelimiter);
                    d('#divIndicatorMapping').html(dataValues[0]);
                    ExpandCollapseDivs('imgdivIndicatorMapping', 'divIndicatorMapping');
                    new di_drawSearchBox('divIndicatorSearch', 'txtIndicatorSearch', '', '', z('hSearchIndicator').value, 'FilterRowsByTextSearch(\'tblIndicator\', \'aShowIndicatorAll\', \'aShowIndicatorMapped\', \'aShowIndicatorUnMapped\', \'aShowIndicatorUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblIndicator\', \'aShowIndicatorAll\', \'aShowIndicatorMapped\', \'aShowIndicatorUnMapped\', \'aShowIndicatorUnSaved\', this);');
                    if (hideMaskingLoading) {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                    LanguageHandlingOfCodelistMappingDivs(dIndicator, dUnit, dArea, dAge, dSex, dLocation)

                    d("#divIndicatorMapping").find('select.cus_slct_dd').each(function (index, domelem) {
                        var eleid = this.id;


                        d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDIndicator') });
                    });
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                    if (hideMaskingLoading) {
                        HideLoadingDiv();
                      }
                }
            },
            error: function () {
              if (hideMaskingLoading) {
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            cache: false
        });
    }
    else {
        ExpandCollapseDivs('imgdivIndicatorMapping', 'divIndicatorMapping');
        z('divIndicatorMapping').style.display == "block";
    }
}


function BindUnitCodelist(hideMaskingLoading) {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    var dIndicator, dUnit, dArea, dAge, dSex, dLocation;
    dIndicator = false, dUnit = true, dArea = false, dAge = false, dSex = false, dLocation = false;
    if (z('tblUnit') == null) {
        var InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        var htmlResp = d.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=297&param1=" + InputParam,
            async: hideMaskingLoading,
            success: function (data) {
                try {
                    divHtml = decodeURIComponent(data);
                    divHtml = ReplaceAll(divHtml, "+", " ");
                    divHtml = ReplaceAll(divHtml, "$", "USD");

                    var dataValues = divHtml.split(ParamDelimiter);
                    d('#divUnitMapping').html(dataValues[0]);
                    //z('divUnitMapping').innerHTML = dataValues[0];
                    ExpandCollapseDivs('imgdivUnitMapping', 'divUnitMapping');
                    new di_drawSearchBox('divUnitSearch', 'txtUnitSearch', '', '', z('hSearchUnit').value, 'FilterRowsByTextSearch(\'tblUnit\', \'aShowUnitAll\', \'aShowUnitMapped\', \'aShowUnitUnMapped\', \'aShowUnitUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblUnit\', \'aShowUnitAll\', \'aShowUnitMapped\', \'aShowUnitUnMapped\', \'aShowUnitUnSaved\', this);');
                    if (hideMaskingLoading) {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }

                    LanguageHandlingOfCodelistMappingDivs(dIndicator, dUnit, dArea, dAge, dSex, dLocation)
                    // d("#divUnitMapping select.cus_slct_dd").customSelect();
                    d("#divUnitMapping").find('select.cus_slct_dd').each(function (index, domelem) {
                        var eleid = this.id;

                        d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDUnit') });
                    });

                }
                catch (ex) {
                    alert("Error : " + ex.message);

                    if (hideMaskingLoading) {
                        HideLoadingDiv();
                        //  RemoveMaskingDiv();
                    }
                }
            },
            error: function () {
                //    

                if (hideMaskingLoading) {
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            cache: false
        });
    }
    else {
        ExpandCollapseDivs('imgdivUnitMapping', 'divUnitMapping');
        z('divUnitMapping').style.display == "block";
    }
}


function BindAreaCodelist(hideMaskingLoading) {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    var dIndicator, dUnit, dArea, dAge, dSex, dLocation;
    dIndicator = false, dUnit = false, dArea = true, dAge = false, dSex = false, dLocation = false;
    //   if (z('tblArea') == null) {
    var InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + di_jq("#selectAreaLevel").val();

    var htmlResp = d.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=298&param1=" + InputParam,
        async: true,
        success: function (data) {
            try {
                divHtml = decodeURIComponent(data);
                divHtml = ReplaceAll(divHtml, "+", " ");
                divHtml = ReplaceAll(divHtml, "$", "USD");

                var dataValues = divHtml.split(ParamDelimiter);
                d('#divAreaMapping').html(dataValues[0]);
                ExpandCollapseDivs('imgdivAreaMapping', 'divAreaMapping');
                new di_drawSearchBox('divAreaSearch', 'txtAreaSearch', '', '', z('hSearchArea').value, 'FilterRowsByTextAreaSearch(\'tblArea\', \'aShowAreaAll\', \'aShowAreaMapped\', \'aShowAreaUnMapped\', \'aShowAreaUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblArea\', \'aShowAreaAll\', \'aShowAreaMapped\', \'aShowAreaUnMapped\', \'aShowAreaUnSaved\', this);');
                if (hideMaskingLoading) {
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }

                LanguageHandlingOfCodelistMappingDivs(dIndicator, dUnit, dArea, dAge, dSex, dLocation)
                d("#divAreaMapping").find('select.cus_slct_dd').each(function (index, domelem) {
                    var eleid = this.id;

                    d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDArea') });
                });

            }
            catch (ex) {
                alert("Error : " + ex.message);

                if (hideMaskingLoading) {
                    HideLoadingDiv();
                }
            }
        },
        error: function () {
            // 

            if (hideMaskingLoading) {
                HideLoadingDiv();
                RemoveMaskingDiv();
            }
        },
        cache: false
    });
    //     }
    //  else {
    //     ExpandCollapseDivs('imgdivAreaMapping', 'divAreaMapping');
    //   z('divAreaMapping').style.display == "block";
    //   }
}




function GetAllMappingList(indInnerHtml, unitInnerHtml, ageInnerHtml, sexInnerHtml, locationInnerHtml, areaInnerHtml) {

    if (indInnerHtml != null) {
        d('#divIndicatorMapping').html(indInnerHtml);
        new di_drawSearchBox('divIndicatorSearch', 'txtIndicatorSearch', '', '', z('hSearchIndicator').value, 'FilterRowsByTextSearch(\'tblIndicator\', \'aShowIndicatorAll\', \'aShowIndicatorMapped\', \'aShowIndicatorUnMapped\', \'aShowIndicatorUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblIndicator\', \'aShowIndicatorAll\', \'aShowIndicatorMapped\', \'aShowIndicatorUnMapped\', \'aShowIndicatorUnSaved\', this);');
    }
    if (unitInnerHtml != null) {
        d('#divUnitMapping').html(unitInnerHtml);
        new di_drawSearchBox('divUnitSearch', 'txtUnitSearch', '', '', z('hSearchUnit').value, 'FilterRowsByTextSearch(\'tblUnit\', \'aShowUnitAll\', \'aShowUnitMapped\', \'aShowUnitUnMapped\', \'aShowUnitUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblUnit\', \'aShowUnitAll\', \'aShowUnitMapped\', \'aShowUnitUnMapped\', \'aShowUnitUnSaved\', this);');
    }
    if (areaInnerHtml != null) {
        d('#divAreaMapping').html(areaInnerHtml);
        new di_drawSearchBox('divAreaSearch', 'txtAreaSearch', '', '', z('hSearchArea').value, 'FilterRowsByTextAreaSearch(\'tblArea\', \'aShowAreaAll\', \'aShowAreaMapped\', \'aShowAreaUnMapped\', \'aShowAreaUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblArea\', \'aShowAreaAll\', \'aShowAreaMapped\', \'aShowAreaUnMapped\', \'aShowAreaUnSaved\', this);');

    }
    if (ageInnerHtml != null) {
        d('#divAgeMapping').html(ageInnerHtml);
        new di_drawSearchBox('divAgeSearch', 'txtAgeSearch', '', '', z('hSearchAge').value, 'FilterRowsByTextSearch(\'tblAge\', \'aShowAgeAll\', \'aShowAgeMapped\', \'aShowAgeUnMapped\', \'aShowAgeUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblAge\', \'aShowAgeAll\', \'aShowAgeMapped\', \'aShowAgeUnMapped\', \'aShowAgeUnSaved\', this);');
    }
    if (sexInnerHtml != null) {
        d('#divSexMapping').html(sexInnerHtml);
        new di_drawSearchBox('divSexSearch', 'txtSexSearch', '', '', z('hSearchSex').value, 'FilterRowsByTextSearch(\'tblSex\', \'aShowSexAll\', \'aShowSexMapped\', \'aShowSexUnMapped\', \'aShowSexUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblSex\', \'aShowSexAll\', \'aShowSexMapped\', \'aShowSexUnMapped\', \'aShowSexUnSaved\', this);');
    }
    if (locationInnerHtml != null) {
        d('#divLocationMapping').html(locationInnerHtml);
        new di_drawSearchBox('divLocationSearch', 'txtLocationSearch', '', '', z('hSearchLocation').value, 'FilterRowsByTextSearch(\'tblLocation\', \'aShowLocationAll\', \'aShowLocationMapped\', \'aShowLocationUnMapped\', \'aShowLocationUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblLocation\', \'aShowLocationAll\', \'aShowLocationMapped\', \'aShowLocationUnMapped\', \'aShowLocationUnSaved\', this);');
    }

}


function GenerateCodelistsMappingFile() {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    var Message;
    Message = ConfirmationOnSavingNewMapping();
    if (Message != '') {
        if (confirm(Message)) {
            ValidatingNGeneratingCodelist();
        }
    }
    else {
        ValidatingNGeneratingCodelist();
    }
}

function ValidatingNGeneratingCodelist() {
    var selectedArea = '';
    var selectedAreaId = '';
    var selectedMappedAreaId = '';
    if (di_jq('input:radio[name=refArea]').length > 0) {
        if (di_jq("input:radio[name='refArea']").is(":checked") == true) {
            selectedArea = di_jq('input:radio[name=refArea]:checked').attr("id");
            selectedArea = selectedArea.split('_');
            selectedAreaId = selectedArea[1];
            selectedMappedAreaId = di_jq("#spanUNSDAreaGId_" + selectedAreaId).text();
        }
        else {
            SelectOneOption = document.getElementById('hSelectOneOption').value;
            alert(SelectOneOption);
            HideLoadingDiv();
            RemoveMaskingDiv();
            return false;
        }
    }
    else {
        selectedMappedAreaId = di_jq("[id^=spanUNSDAreaGId_]").text();
    }
    if (ValidateCodelistMappingData()) {
        var InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += ParamDelimiter + z('hdnSelectedAgeCodelist').value;
        InputParam += ParamDelimiter + z('hdnSelectedSexCodelist').value;
        InputParam += ParamDelimiter + z('hdnSelectedLocationCodelist').value;
        InputParam += ParamDelimiter + GetCodelistMappingData();
        InputParam += ParamDelimiter + selectedMappedAreaId;
        var htmlResp = d.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=128&param1=" + InputParam,
            async: true,
            success: function (data) {
                try {
                    if (data == "true") {

                        BindCodelistMappingLists(false, null);
                        MappingFileGenerateStatus = document.getElementById('hMappingFileGenerateStatus').value;
                        alert(MappingFileGenerateStatus);
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                    else {
                        alert(data);

                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            error: function () {
                HideLoadingDiv();
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
    else {
        if (ErrorMessage != '') {
            alert(ErrorMessage);
        }

        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function GetCodelistMappingData() {
    var RetVal;
    var tblCodelist, spanDevInfoCodelistGId, ddlUNSDCodelist, spanUNSDCodelistGId;
    if (z('hdnSelectedAgeCodelist').value != "" && z('hdnSelectedSexCodelist').value != "" && z('hdnSelectedLocationCodelist').value != "") {
        var tblCodelistIds = ["tblIndicator", "tblUnit", "tblAge", "tblSex", "tblLocation", "tblArea"];
        var CodelistIds = ["Indicator", "Unit", "Age", "Sex", "Location", "Area"];

    }
    else if (z('hdnSelectedAgeCodelist').value != "" || z('hdnSelectedSexCodelist').value != "" || z('hdnSelectedLocationCodelist').value != "") {
        if (z('hdnSelectedAgeCodelist').value != "") {
            var tblCodelistIds = ["tblIndicator", "tblUnit", "tblAge", "tblArea"];
            var CodelistIds = ["Indicator", "Unit", "Age", "Area"];
        }
        if (z('hdnSelectedSexCodelist').value != "") {

            var tblCodelistIds = ["tblIndicator", "tblUnit", "tblSex", "tblArea"];
            var CodelistIds = ["Indicator", "Unit", "Sex", "Area"];
        }
        if (z('hdnSelectedLocationCodelist').value != "") {

            var tblCodelistIds = ["tblIndicator", "tblUnit", "tblLocation", "tblArea"];
            var CodelistIds = ["Indicator", "Unit", "Location", "Area"];
        }

        if (z('hdnSelectedAgeCodelist').value != "" && z('hdnSelectedSexCodelist').value != "") {
            var tblCodelistIds = ["tblIndicator", "tblUnit", "tblAge", "tblSex", "tblArea"];
            var CodelistIds = ["Indicator", "Unit", "Age", "Sex", "Area"];

        }
        if (z('hdnSelectedAgeCodelist').value != "" && z('hdnSelectedLocationCodelist').value != "") {
            var tblCodelistIds = ["tblIndicator", "tblUnit", "tblAge", "tblLocation", "tblArea"];
            var CodelistIds = ["Indicator", "Unit", "Age", "Location", "Area"];

        }
        if (z('hdnSelectedSexCodelist').value != "" && z('hdnSelectedLocationCodelist').value != "") {
            var tblCodelistIds = ["tblIndicator", "tblUnit", "tblSex", "tblLocation", "tblArea"];
            var CodelistIds = ["Indicator", "Unit", "Sex", "Location", "Area"];
        }


    }
    else {
        var tblCodelistIds = ["tblIndicator", "tblUnit", "tblArea"];
        var CodelistIds = ["Indicator", "Unit", "Area"];
    }

    RetVal = "";

    for (var i = 0; i < tblCodelistIds.length; i++) {

        tblCodelist = z(tblCodelistIds[i]);
        if (tblCodelist != null) {
            for (var j = 0; j < tblCodelist.rows.length; j++) {
                spanDevInfoCodelistGId = tblCodelist.rows[j].cells[1].children[0];
                ddlUNSDCodelist = tblCodelist.rows[j].cells[2].children[0];
                spanUNSDCodelistGId = tblCodelist.rows[j].cells[3].children[0];

                if (ddlUNSDCodelist.value != '-1') {
                    RetVal += CodelistIds[i] + "[**[C]**]" + spanDevInfoCodelistGId.innerHTML + "[**[C]**]" + spanUNSDCodelistGId.innerHTML + "[**[R]**]";
                }
            }
        }
    }

    if (RetVal != '' && RetVal != undefined) {
        RetVal = RetVal.substr(0, RetVal.length - 9);
    }

    return RetVal;
}




function ValidateCodelistMappingData() {
    var RetVal;
    var tblCodelist, ddlUNSDCodelist;
    var allIndicatorRows = '';
    var allUnitRows = '';
    var allAreaRows = '';
    var tblCodelistIds;
    var AgeSexLocArray;
    if (z('hdnSelectedAgeCodelist').value != "" && z('hdnSelectedSexCodelist').value != "" && z('hdnSelectedLocationCodelist').value != "") {
        var tblCodelistIds = ["tblAge", "tblSex", "tblLocation"];
    }
    else if (z('hdnSelectedAgeCodelist').value != "" || z('hdnSelectedSexCodelist').value != "" || z('hdnSelectedLocationCodelist').value != "") {
        if (z('hdnSelectedAgeCodelist').value != "") {
            var tblCodelistIds = ["tblAge"];
        }
        if (z('hdnSelectedSexCodelist').value != "") {
            var tblCodelistIds = ["tblSex"];
        }
        if (z('hdnSelectedLocationCodelist').value != "") {
            var tblCodelistIds = ["tblLocation"];
        }
        if (z('hdnSelectedAgeCodelist').value != "" && z('hdnSelectedSexCodelist').value != "") {
            var tblCodelistIds = ["tblAge", "tblSex"];
        }
        if (z('hdnSelectedAgeCodelist').value != "" && z('hdnSelectedLocationCodelist').value != "") {
            var tblCodelistIds = ["tblAge", "tblLocation"];
        }
        if (z('hdnSelectedSexCodelist').value != "" && z('hdnSelectedLocationCodelist').value != "") {
            var tblCodelistIds = ["tblSex", "tblLocation"];
        }
    }

    if (tblCodelistIds != undefined) {
        AgeSexLocArray = tblCodelistIds.concat();
    }
    if (di_jq("#tblIndicator").find('tr').attr("status") != undefined) {
        var table = document.getElementById("tblIndicator");
        for (var i = 0, row; row = table.rows[i]; i++) {
            if (row.getAttribute('status') == "unsaved") {

                allIndicatorRows = "unsaved";
                break;
            }
            else {
                allIndicatorRows = "unmapped";

            }
        }
    }

    if (di_jq("#tblUnit").find('tr').attr("status") != undefined) {

        var table = document.getElementById("tblUnit");
        for (var i = 0, row; row = table.rows[i]; i++) {
            if (row.getAttribute('status') == "unsaved") {

                allUnitRows = "unsaved";
                break;
            }
            else {
                allUnitRows = "unmapped";

            }
        }


    }

    if (di_jq("#tblArea").find('tr').attr("status") != undefined) {

        var table = document.getElementById("tblArea");
        for (var i = 0, row; row = table.rows[i]; i++) {
            if (row.getAttribute('status') == "unsaved") {

                allAreaRows = "unsaved";
                break;
            }
            else {
                allAreaRows = "unmapped";

            }
        }
    }

    RetVal = false;
    var tblCodelistIds = IncludeDivs(allAreaRows, allIndicatorRows, allUnitRows);
    var IndicatorUnitAreaArray = '';
    if (tblCodelistIds != undefined) {
        IndicatorUnitAreaArray = tblCodelistIds.concat();
        if (AgeSexLocArray != undefined) {
            IndicatorUnitAreaArray = tblCodelistIds.concat(AgeSexLocArray);
         }
    }
    else {
        if (AgeSexLocArray != undefined) {
            IndicatorUnitAreaArray = AgeSexLocArray.concat();
        }
    } 


    //replacing tblCodelistIds with IndicatorUnitAreaArray
    if (IndicatorUnitAreaArray != undefined) {
        for (var i = 0; i < IndicatorUnitAreaArray.length; i++) {
            if (RetVal == false) {
                tblCodelist = z(IndicatorUnitAreaArray[i]);

                for (var j = 0; j < tblCodelist.rows.length; j++) {
                    ddlUNSDCodelist = tblCodelist.rows[j].cells[2].children[0];

                    if (ddlUNSDCodelist.value != '-1') {
                        RetVal = true;
                        ErrorMessage = '';
                        break;
                    }
                }
            }
            else {
                break;
            }
        }
    }
    else {
        RetVal = false;
        ErrorMessage = MapOneCode;  //"Please map atleast one DevInfo code to UNSD code!";
    }
    return RetVal;
}

function SelectCodelistCode(tblCodelistId, rowCodelistId, spanUNSDCodelistGIdId, ddlUNSDCodelist, orginalSelectedValue, originalRowColor, originalRowStatus) {
    var tblCodelist, ddlUNSDCodelistPerRow;
    var AlreadyMappedFlag;


    tblCodelist = z(tblCodelistId);
    AlreadyMappedFlag = false;

    if (ddlUNSDCodelist.value != '-1') {
        for (var i = 0; i < tblCodelist.rows.length; i++) {
            ddlUNSDCodelistPerRow = tblCodelist.rows[i].cells[2].children[0];

            if (ddlUNSDCodelistPerRow.value == ddlUNSDCodelist.value && ddlUNSDCodelistPerRow.id != ddlUNSDCodelist.id) {
                //AlreadyMappedFlag = true;     // Commented to lift dubplicate UNSD validation
                break;
            }
        }

        if (AlreadyMappedFlag == false) {
            z(spanUNSDCodelistGIdId).innerHTML = ddlUNSDCodelist.value;
        }
        else {
            ApplyMaskingDiv();
            ShowLoadingDiv();

            alert('This UNSD Code is already mapped to a different Devinfo Code');
            ddlUNSDCodelist.value = orginalSelectedValue;

            if (orginalSelectedValue != '-1') {
                z(spanUNSDCodelistGIdId).innerHTML = orginalSelectedValue;
            }
            else {
                z(spanUNSDCodelistGIdId).innerHTML = '';
            }

            HideLoadingDiv();
            RemoveMaskingDiv();
        }
    }
    else {
        z(spanUNSDCodelistGIdId).innerHTML = '';
    }

    if (ddlUNSDCodelist.value != orginalSelectedValue) {
        z(rowCodelistId).style.background = "rgb(221, 221, 255)";
        z(rowCodelistId).setAttribute("status", "unsaved");
    }
    else {
        z(rowCodelistId).style.background = originalRowColor;
        z(rowCodelistId).setAttribute("status", originalRowStatus);
    }
}

function ReplaceAll(Source, stringToFind, stringToReplace) {
    var RetVal = "";
    var TempArr;

    try {
        TempArr = Source.split(stringToFind);
        RetVal = TempArr.join(stringToReplace);
    }
    catch (err) { }

    return RetVal;
}

function BindMetadataMappingList(hideMaskingLoading) {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    z('divMetadataMapping').style.display == "block";

    var InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    var divHtml = '';
    var htmlResp = d.ajax({
        // var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=138&param1=" + InputParam,
        async: hideMaskingLoading,
        success: function (data) {
            try {
                divHtml = decodeURIComponent(data);
                divHtml = ReplaceAll(divHtml, "+", " ");
                if (divHtml.toLowerCase().indexOf("false") >= 0) {
                    var dataValues = divHtml.split("[****]");
                    z('divMetadataMapping').innerHTML = "DSD doesn't contains Metadata Structure Definition";

                    if (hideMaskingLoading) {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                }
                else {
                     z('divMetadataMapping').innerHTML = divHtml;
                    AddOptionsToDdlPerRow('tblMetadata', 'ddlUNSDMetadata');
                    new di_drawSearchBox('divMetadataSearch', 'txtMetadataSearch', '', '', z('hSearchMetadata').value, 'FilterRowsByTextSearch(\'tblMetadata\', \'aShowMetadataAll\', \'aShowMetadataMapped\', \'aShowMetadataUnMapped\', \'aShowMetadataUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblMetadata\', \'aShowMetadataAll\', \'aShowMetadataMapped\', \'aShowMetadataUnMapped\', \'aShowMetadataUnSaved\', this);');

                    if (hideMaskingLoading) {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }

                    LanguageHandlingOfMetadataMappingDiv();
                    d("#divMetadataMapping .chzn-select").chosen({
                        overflow_container: d(".chzn-drop").add(document)
                    });
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);

                if (hideMaskingLoading) {
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            }
        },
        error: function () {
            if (hideMaskingLoading) {
                HideLoadingDiv();
                RemoveMaskingDiv();
            }
        },
        cache: false
    });
}

function GenerateMetadataMappingFile() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (ValidateMetadataMappingData()) {
        var InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += ParamDelimiter + GetMetadataMappingData();

        var htmlResp = d.ajax({
            //    var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=137&param1=" + InputParam,
            async: true,
            success: function (data) {
                try {
                    if (data == "true") {
                        BindMetadataMappingList(false);
                        alert(MappingFileGenerateStatus);
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                    else {
                        alert(data);

                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            error: function () {
                HideLoadingDiv();
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
    else {
        if (ErrorMessage != '') {
            alert(ErrorMessage);
        }
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function GetMetadataMappingData() {
    var RetVal;
    var tblMetadata, spanDevInfoMetadataGId, ddlUNSDMetadata, spanUNSDMetadataGId;

    RetVal = "";
    tblMetadata = z('tblMetadata');

    for (var i = 0; i < tblMetadata.rows.length; i++) {
        spanDevInfoMetadataGId = tblMetadata.rows[i].cells[1].children[0];
        ddlUNSDMetadata = tblMetadata.rows[i].cells[2].children[0];
        spanUNSDMetadataGId = tblMetadata.rows[i].cells[3].children[0];

        if (ddlUNSDMetadata.value != '-1') {
            RetVal += spanDevInfoMetadataGId.innerHTML + "[**[C]**]" + spanUNSDMetadataGId.innerHTML + "[**[R]**]";
        }
    }

    if (RetVal != '' && RetVal != undefined) {
        RetVal = RetVal.substr(0, RetVal.length - 9);
    }

    return RetVal;
}

function ValidateMetadataMappingData() {
    var RetVal;
    var tblMetadata, ddlUNSDMetadata;

    RetVal = false;
    ErrorMessage = MapOneConcept; //"Please map atleast one DevInfo concept to UNSD concept!";
    tblMetadata = z('tblMetadata');

    for (var i = 0; i < tblMetadata.rows.length; i++) {
        ddlUNSDMetadata = tblMetadata.rows[i].cells[2].children[0];

        if (ddlUNSDMetadata.value != '-1') {
            RetVal = true;
            ErrorMessage = '';
            break;
        }
    }

    return RetVal;
}

function SelectMetadataCategory(rowMetadataId, spanUNSDMetadataGIdId, ddlUNSDMetadata, orginalSelectedValue, originalRowColor, originalRowStatus) {
    var tblMetadata, ddlUNSDMetadataPerRow;
    var AlreadyMappedFlag;

    tblMetadata = z('tblMetadata');
    AlreadyMappedFlag = false;

    if (ddlUNSDMetadata.value != '-1') {
        for (var i = 0; i < tblMetadata.rows.length; i++) {
            ddlUNSDMetadataPerRow = tblMetadata.rows[i].cells[2].children[0];

            if (ddlUNSDMetadataPerRow.value == ddlUNSDMetadata.value && ddlUNSDMetadataPerRow.id != ddlUNSDMetadata.id) {
                AlreadyMappedFlag = true;
                break;
            }
        }

        if (AlreadyMappedFlag == false) {
            z(spanUNSDMetadataGIdId).innerHTML = ddlUNSDMetadata.value;
        }
        else {
            ApplyMaskingDiv();
            ShowLoadingDiv();
            alert(AlreadyMappedConcept);
            ddlUNSDMetadata.value = orginalSelectedValue;

            if (orginalSelectedValue != '-1') {
                z(spanUNSDMetadataGIdId).innerHTML = orginalSelectedValue;
            }
            else {
                z(spanUNSDMetadataGIdId).innerHTML = '';
            }

            HideLoadingDiv();
            RemoveMaskingDiv();
        }
    }
    else {
        z(spanUNSDMetadataGIdId).innerHTML = '';
    }

    if (ddlUNSDMetadata.value != orginalSelectedValue) {
        z(rowMetadataId).style.background = "rgb(221, 221, 255)";
        z(rowMetadataId).setAttribute("status", "unsaved");
    }
    else {
        z(rowMetadataId).style.background = originalRowColor;
        z(rowMetadataId).setAttribute("status", originalRowStatus);
    }
}

function BindIUSMappingList(hideMaskingLoading) {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    z('divIUSMapping').style.display == "block";

    var InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hdnSelectedAgeCodelist').value;
    InputParam += ParamDelimiter + z('hdnSelectedSexCodelist').value;
    InputParam += ParamDelimiter + z('hdnSelectedLocationCodelist').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;

    var htmlResp = d.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=139&param1=" + InputParam,
        async: hideMaskingLoading,
        success: function (data) {
            try {

                if (data != "NRF") {
                    z('divIUSMapping').innerHTML = data;
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSIndicator', 4);
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSUnit', 5);
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSAge', 6);
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSSex', 7);
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSLocation', 8);
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSFrequency', 9);
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSSourceType', 10);
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSNature', 11);
                    AddOptionsToIUSDdlsPerRow('tblIUS', 'ddlUNSDIUSUnitMult', 12);

                    new di_drawSearchBox('divIUSSearch', 'txtIUSSearch', '', '', z('hSearchIUS').value, 'FilterRowsByIUSSearch(\'tblIUS\', \'aShowIUSAll\', \'aShowIUSMapped\', \'aShowIUSUnMapped\', \'aShowIUSUnSaved\', this);', 'HandleCrossClickForTextSearch(\'tblIUS\', \'aShowIUSAll\', \'aShowIUSMapped\', \'aShowIUSUnMapped\', \'aShowIUSUnSaved\', this);');

                    if (hideMaskingLoading) {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }

                    LanguageHandlingOfIUSMappingDiv();
                }
                else {
                    alert("No mapping found");
                    ShowHideMappings("divCodelistMapping");
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);

                if (hideMaskingLoading) {
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            }
        },
        error: function () {
            //

            if (hideMaskingLoading) {
                HideLoadingDiv();
                RemoveMaskingDiv();
            }
        },
        cache: false
    });
}

function GenerateIUSMappingFile() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (ValidateIUSMappingData()) {
        var InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += ParamDelimiter + GetIUSMappingData();

        var htmlResp = d.ajax({
            //  var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=140&param1=" + InputParam,
            async: true,
            success: function (data) {
                try {
                    if (data == "true") {
                        BindIUSMappingList(false);
                        //alert("IUS Mapping File successfully generated.");
                        alert(MappingFileGenerateStatus);
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                    else {
                        alert(data);

                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            error: function () {
                // 

                HideLoadingDiv();
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
    else {
        alert(ErrorMessage);

        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function GetIUSMappingData() {
    var RetVal;
    var tblIUS, chkIsMapped, spanIndicator, ddlIndicator, ddlUnit, ddlAge, ddlSex, ddlLocation, ddlFrequency, dllSourceType, ddlNature, ddlUnitMult;

    RetVal = "";
    tblIUS = z('tblIUS');

    for (var i = 0; i < tblIUS.rows.length; i++) {
        chkIsMapped = tblIUS.rows[i].cells[3].children[0];
        // spanIndicator = tblIUS.rows[i].cells[4].children[0];
        ddlIndicator = tblIUS.rows[i].cells[4].children[0];
        ddlUnit = tblIUS.rows[i].cells[5].children[0];
        ddlAge = tblIUS.rows[i].cells[6].children[0];
        ddlSex = tblIUS.rows[i].cells[7].children[0];
        ddlLocation = tblIUS.rows[i].cells[8].children[0];
        ddlFrequency = tblIUS.rows[i].cells[9].children[0];
        dllSourceType = tblIUS.rows[i].cells[10].children[0];
        ddlNature = tblIUS.rows[i].cells[11].children[0];
        ddlUnitMult = tblIUS.rows[i].cells[12].children[0];

        if (chkIsMapped.checked == true) {
            // spanIndicator.innerHTML + "[**[C]**]"
            RetVal += chkIsMapped.value + "[**[C]**]" + ddlIndicator.value + "[**[C]**]" + ddlUnit.value + "[**[C]**]" + ddlAge.value + "[**[C]**]" + ddlSex.value + "[**[C]**]" +
                      ddlLocation.value + "[**[C]**]" + ddlFrequency.value + "[**[C]**]" + dllSourceType.value + "[**[C]**]" + ddlNature.value + "[**[C]**]" +
                      ddlUnitMult.value + "[**[R]**]";
        }
    }

    if (RetVal != '' && RetVal != undefined) {
        RetVal = RetVal.substr(0, RetVal.length - 9);
    }

    return RetVal;
}

function ValidateIUSMappingData() {
    var RetVal;
    var AtleastOneCheckboxChecked;
    var tblIUS, chkIsMapped, ddlIndicator, ddlUnit, ddlAge, ddlSex, ddlLocation, ddlFrequency, dllSourceType, ddlNature, ddlUnitMult;

    RetVal = true;
    AtleastOneCheckboxChecked = false;
    tblIUS = z('tblIUS');

    for (var i = 1; i < tblIUS.rows.length; i++) {
        chkIsMapped = tblIUS.rows[i].cells[3].children[0];
        ddlIndicator = tblIUS.rows[i].cells[4].children[0];
        ddlUnit = tblIUS.rows[i].cells[5].children[0];
        ddlAge = tblIUS.rows[i].cells[6].children[0];
        ddlSex = tblIUS.rows[i].cells[7].children[0];
        ddlLocation = tblIUS.rows[i].cells[8].children[0];
        ddlFrequency = tblIUS.rows[i].cells[9].children[0];
        dllSourceType = tblIUS.rows[i].cells[10].children[0];
        ddlNature = tblIUS.rows[i].cells[11].children[0];
        ddlUnitMult = tblIUS.rows[i].cells[12].children[0];

        if (chkIsMapped.checked == true) {
            AtleastOneCheckboxChecked = true;

            if ((ddlIndicator.value == "-1") || (ddlUnit.value == "-1") || (ddlAge.value == "-1") || (ddlSex.value == "-1") || (ddlLocation.value == "-1") || (ddlFrequency.value == "-1") ||
                (dllSourceType.value == "-1") || (ddlNature.value == "-1") || (ddlUnitMult.value == "-1")) {
                RetVal = false;
                SelectAllDropdowns = document.getElementById('hSelectAllDropdowns').value;
                ErrorMessage = SelectAllDropdowns;   // "Please select values in all columns for any row that is mapped!";
                break;
            }
        }
    }

    if (AtleastOneCheckboxChecked == false) {
        RetVal = false;
        SelectOneMappedRow = document.getElementById('hSelectOneMappedRow').value;
        ErrorMessage = SelectOneMappedRow;  //"Please select Mapped column for atleast one row!";
    }

    return RetVal;
}

function HandleStateChange(rowIUSId, orginalState, originalRowColor, originalRowStatus) {
    var rowIUS, chkIsMapped, ddlIndicator, ddlUnit, ddlAge, ddlSex, ddlLocation, ddlFrequency, dllSourceType, ddlNature, ddlUnitMult;
    var newState;

    rowIUS = z(rowIUSId);
    chkIsMapped = rowIUS.cells[3].children[0];
    ddlIndicator = rowIUS.cells[4].children[0];
    ddlUnit = rowIUS.cells[5].children[0];
    ddlAge = rowIUS.cells[6].children[0];
    ddlSex = rowIUS.cells[7].children[0];
    ddlLocation = rowIUS.cells[8].children[0];
    ddlFrequency = rowIUS.cells[9].children[0];
    dllSourceType = rowIUS.cells[10].children[0];
    ddlNature = rowIUS.cells[11].children[0];
    ddlUnitMult = rowIUS.cells[12].children[0];
    newState = "";

    if (chkIsMapped.checked) {
        newState += "true" + ParamDelimiter;
    }
    else {
        newState += "false" + ParamDelimiter;
    }
    newState += ddlIndicator.value + ParamDelimiter;
    newState += ddlUnit.value + ParamDelimiter;
    newState += ddlAge.value + ParamDelimiter;
    newState += ddlSex.value + ParamDelimiter;
    newState += ddlLocation.value + ParamDelimiter;
    newState += ddlFrequency.value + ParamDelimiter;
    newState += dllSourceType.value + ParamDelimiter;
    newState += ddlNature.value + ParamDelimiter;
    newState += ddlUnitMult.value;

    if (newState != orginalState) {
        rowIUS.style.background = "rgb(221, 221, 255)";
        rowIUS.setAttribute("status", "unsaved");
    }
    else {
        rowIUS.style.background = originalRowColor;
        rowIUS.setAttribute("status", originalRowStatus);
    }
}

function FilterRowsByStatus(tableId, aShowAllId, aShowMappedId, aShowUnMappedId, aShowUnSavedId, rowStatus) {
    var table, row;
    var aShowAll, aShowMapped, aShowUnMapped, aShowUnSaved;

    table = z(tableId);
    aShowAll = z(aShowAllId);
    aShowMapped = z(aShowMappedId);
    aShowUnMapped = z(aShowUnMappedId);
    aShowUnSaved = z(aShowUnSavedId);

    if (rowStatus == 'all') {
        aShowAll.style.color = "#000000";
        aShowMapped.style.color = "#1e90ff";
        aShowUnMapped.style.color = "#1e90ff";
        aShowUnSaved.style.color = "#1e90ff";
    }
    else if (rowStatus == 'mapped') {
        aShowAll.style.color = "#1e90ff";
        aShowMapped.style.color = "#000000";
        aShowUnMapped.style.color = "#1e90ff";
        aShowUnSaved.style.color = "#1e90ff";
    }
    else if (rowStatus == 'unmapped') {
        aShowAll.style.color = "#1e90ff";
        aShowMapped.style.color = "#1e90ff";
        aShowUnMapped.style.color = "#000000";
        aShowUnSaved.style.color = "#1e90ff";
    }
    else if (rowStatus == 'unsaved') {
        aShowAll.style.color = "#1e90ff";
        aShowMapped.style.color = "#1e90ff";
        aShowUnMapped.style.color = "#1e90ff";
        aShowUnSaved.style.color = "#000000";
    }

    for (var i = 0; i < table.rows.length; i++) {
        row = table.rows[i];
        row.style.display = "none";

        if (rowStatus == 'all') {
            row.style.display = "";
        }
        else if (row.getAttribute('status') == rowStatus) {
            row.style.display = "";
        }
    }
}

function FilterRowsByTextSearch(tableId, aShowAllId, aShowMappedId, aShowUnMappedId, aShowUnSavedId, txtSearch) {
    var table, row, value, RegEx;
    var aShowAll, aShowMapped, aShowUnMapped, aShowUnSaved;

    table = z(tableId);
    aShowAll = z(aShowAllId);
    aShowMapped = z(aShowMappedId);
    aShowUnMapped = z(aShowUnMappedId);
    aShowUnSaved = z(aShowUnSavedId);
    RegEx = new RegExp(txtSearch.value, "i");

    aShowAll.style.color = "#000000";
    aShowMapped.style.color = "#1e90ff";
    aShowUnMapped.style.color = "#1e90ff";
    aShowUnSaved.style.color = "#1e90ff";

    for (var i = 0; i < table.rows.length; i++) {
        row = table.rows[i];

        value = row.cells[0].children[0].getAttribute('value');

        if (RegEx.test(value)) {
            row.style.display = "";
        }
        else {
            row.style.display = "none";
        }
    }
}

function FilterRowsByIUSSearch(tableId, aShowAllId, aShowMappedId, aShowUnMappedId, aShowUnSavedId, txtSearch) {
    var table, row, value, RegEx;
    var aShowAll, aShowMapped, aShowUnMapped, aShowUnSaved;

    table = z(tableId);
    aShowAll = z(aShowAllId);
    aShowMapped = z(aShowMappedId);
    aShowUnMapped = z(aShowUnMappedId);
    aShowUnSaved = z(aShowUnSavedId);
    RegEx = new RegExp(txtSearch.value, "i");

    aShowAll.style.color = "#000000";
    aShowMapped.style.color = "#1e90ff";
    aShowUnMapped.style.color = "#1e90ff";
    aShowUnSaved.style.color = "#1e90ff";

    for (var i = 1; i < table.rows.length; i++) {
        row = table.rows[i];

        value = row.cells[0].children[0].getAttribute('value');

        if (RegEx.test(value)) {
            row.style.display = "";
        }
        else {
            row.style.display = "none";
        }
    }
}


function FilterRowsByTextAreaSearch(tableId, aShowAllId, aShowMappedId, aShowUnMappedId, aShowUnSavedId, txtSearch) {
    var table, row, value, RegEx;
    var aShowAll, aShowMapped, aShowUnMapped, aShowUnSaved;

    table = z(tableId);
    aShowAll = z(aShowAllId);
    aShowMapped = z(aShowMappedId);
    aShowUnMapped = z(aShowUnMappedId);
    aShowUnSaved = z(aShowUnSavedId);
    RegEx = new RegExp(txtSearch.value, "i");

    aShowAll.style.color = "#000000";
    aShowMapped.style.color = "#1e90ff";
    aShowUnMapped.style.color = "#1e90ff";
    aShowUnSaved.style.color = "#1e90ff";

    for (var i = 0; i < table.rows.length; i++) {
        row = table.rows[i];
        value = row.cells[0].children[1].getAttribute('value');

        if (RegEx.test(value)) {
            row.style.display = "";
        }
        else {
            row.style.display = "none";
        }
    }
}

function HandleCrossClickForTextSearch(tableId, aShowAllId, aShowMappedId, aShowUnMappedId, aShowUnSavedId, txtSearch) {
    var table, row;
    var aShowAll, aShowMapped, aShowUnMapped, aShowUnSaved, searchBox;

    table = z(tableId);
    aShowAll = z(aShowAllId);
    aShowMapped = z(aShowMappedId);
    aShowUnMapped = z(aShowUnMappedId);
    aShowUnSaved = z(aShowUnSavedId);
    searchBox = di_jq(txtSearch).parents().find("input").attr("id");
    txtSearch.value = "";
    di_jq("#" + searchBox).val("");
    aShowAll.style.color = "#000000";
    aShowMapped.style.color = "#1e90ff";
    aShowUnMapped.style.color = "#1e90ff";
    aShowUnSaved.style.color = "#1e90ff";

    for (var i = 0; i < table.rows.length; i++) {

        row = table.rows[i];

        row.style.display = "";
    }
}

function ModifyDdlsByTextFilter(tableId, originalDdlId, txtFilter) {
    var table, row, dl;
    var ddlPerRow, originalDdl;
    var RegEx, OptionsToBeRemoved, counter;
    var dlarr;
    table = z(tableId);
    originalDdl = z(originalDdlId);
    RegEx = new RegExp(txtFilter.value, "i");
    /* this is the ID of our drop down */
    dl = z(originalDdlId);
    for (var i = 0; i < table.rows.length; i++) {
        row = table.rows[i];
        ddlPerRow = table.rows[i].cells[2].children[0];
        for (var j = 1; j < ddlPerRow.options.length; j++) {
            if (txtFilter.value != '') {
                if (!ddlPerRow.options[j].selected) {
                    if (!RegEx.test(ddlPerRow.options[j].text)) {
                        ddlPerRow.options[j].style.background = "rgb(255, 221, 221)";
                        //  hidevals(ddlPerRow, dl, RegEx);
                    }
                    else {
                        ddlPerRow.options[j].style.background = "rgb(221, 255, 221)";
                    }
                }
            }
            else {
                ddlPerRow.options[j].style.background = "rgb(255, 255, 255)";
            }
        }
    }
}

/*call this function to restore the original values */
function restore(dl) {
    var dlarr = new Array(0)
    for (optionCounter = 0; optionCounter < dl.length; optionCounter++) {
        dlarr.push(dl.options[optionCounter].value)
    }
    var obj = dl;
    for (i = 0; dlarr.length > i; i++) {
        obj.options[i] = new Option(dlarr[i], dlarr[i]);
    }
}

function hidevals(ddlPerRow, dl, RegEx) {
    /*before hiding, restore the original values */
    restore(dl);
    OptionsToBeRemoved = new Array();
    counter = 0;
    for (var j = 1; j < ddlPerRow.options.length; j++) {
        if (!RegEx.test(ddlPerRow.options[j].text) && !ddlPerRow.options[j].selected) {
            OptionsToBeRemoved.push(j);
        }
    }
    for (var k = 0; k < OptionsToBeRemoved.length; k++) {
        ddlPerRow.options.remove(OptionsToBeRemoved[k] - counter);
        counter++;
    }
}


function ReInitializeDropDown(ddlPerRow, originalDdl) {
    var selectedOptionValue, addOption;
    selectedOptionValue = ddlPerRow.value;
    ddlPerRow.options.length = 0;
    for (var j = 0; j < originalDdl.options.length; j++) {
        addOption = document.createElement('option');
        addOption.text = originalDdl.options[j].text;
        addOption.value = originalDdl.options[j].value;

        if (addOption.value == selectedOptionValue) {
            addOption.setAttribute('selected', 'selected');
        }

        ddlPerRow.options.add(addOption);
    }
}


function HandleCrossClickForTextFilter(tableId, originalDdlId, txtFilter) {
    var table, row;
    var ddlPerRow, originalDdl;

    table = z(tableId);
    originalDdl = z(originalDdlId);
    txtFilter.value = "";

    for (var i = 0; i < table.rows.length; i++) {
        row = table.rows[i];
        ddlPerRow = table.rows[i].cells[2].children[0];

        for (var j = 0; j < ddlPerRow.options.length; j++) {
            ddlPerRow.options[j].style.background = "rgb(255, 255, 255)";
        }
    }
}

function AddOptionsToDdlPerRow(tblCodelistId, ddlUNSDCodelistId) {
    var tblCodelist, ddlUNSDCodelist, ddlUNSDCodelistPerRow, OptionToAdd;

    tblCodelist = z(tblCodelistId);
    ddlUNSDCodelist = z(ddlUNSDCodelistId);
    ddlUNSDCodelistPerRow = null;
    OptionToAdd = null;
    for (var i = 0; i < tblCodelist.rows.length; i++) {
        ddlUNSDCodelistPerRow = tblCodelist.rows[i].cells[2].children[0];

        for (var j = 1; j < ddlUNSDCodelist.options.length; j++) {
            OptionToAdd = document.createElement("option");

            OptionToAdd.value = ddlUNSDCodelist.options[j].value;
            OptionToAdd.text = ddlUNSDCodelist.options[j].innerHTML;
            OptionToAdd.title = ddlUNSDCodelist.options[j].title;

            if (ddlUNSDCodelistPerRow.value != '-1') {
                if (OptionToAdd.value != ddlUNSDCodelistPerRow.value) {
                    ddlUNSDCodelistPerRow.options.add(OptionToAdd);
                }
            }
            else {
                ddlUNSDCodelistPerRow.options.add(OptionToAdd);
            }
        }
    }
}

function AddOptionstoSelect(elementId, hiddenDdlname) {

    d("#" + elementId).addClass('chzn-select');

    //  d("#" + elementId).nextAll('.customStyleSelectBox:first').css('display', 'none');
    var ddlUNSDCodelist = z(hiddenDdlname);
    var ddloptions = d("#" + ddlUNSDCodelist.id + " > option").clone();
    var ddlOption = d("#" + elementId);
    var item = null;
    var ddlElem = document.getElementById(elementId);
    for (i = 0; i < ddloptions.length; i++) {
        item = new Option;
        item.value = ddloptions[i].value;
        item.text = ddloptions[i].title;
        ddlElem.options.add(item);
    }

    d("#" + elementId).chosen({
        overflow_container: d(".chzn-drop").add(document)
    });
    d("#" + 'remove_' + elementId).hide();



}

function AddOptionsToIUSDdlsPerRow(tblIUSId, ddlUNSDIUSId, ddlUNSDIUSCellIndex) {
    var tblIUS, ddlUNSDIUS, ddlUNSDIUSPerRow, OptionToAdd;

    tblIUS = z(tblIUSId);
    ddlUNSDIUS = z(ddlUNSDIUSId);
    ddlUNSDIUSPerRow = null;
    OptionToAdd = null;
    //changed I index from 0 to 1 22 May 2013
    for (var i = 1; i < tblIUS.rows.length; i++) {
        ddlUNSDIUSPerRow = tblIUS.rows[i].cells[ddlUNSDIUSCellIndex].children[0];

        for (var j = 1; j < ddlUNSDIUS.options.length; j++) {
            OptionToAdd = document.createElement("option");

            OptionToAdd.value = ddlUNSDIUS.options[j].value;
            OptionToAdd.text = ddlUNSDIUS.options[j].innerHTML;
            OptionToAdd.title = ddlUNSDIUS.options[j].title;

            if (ddlUNSDIUSPerRow.value != '-1') {
                if (OptionToAdd.value != ddlUNSDIUSPerRow.value) {
                    ddlUNSDIUSPerRow.options.add(OptionToAdd);
                }
            }
            else {
                ddlUNSDIUSPerRow.options.add(OptionToAdd);
            }
        }
    }
}

function OpenCodelistPopup(object) {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    z('hSelectedCodelist').value = '';
    if (object != undefined) {
        z('hSelectedCodelist').value = object.id;
    }
    SetCloseButtonInPopupDiv(d('#CodelistPopup'), 'CodelistPopupCancel', '../../stock/themes/default/images/close.png');
    d('#CodelistPopup').show('slow');

    GetCodelistDivInnerHTML();

    GetWindowCentered(z("CodelistPopup"), 710, 520);
}

function CodelistPopupOk() {
    var SelectedText = "";
    var SelectedCodelistValue = "";
    var tblDSD = z('tblCodelist');
    for (i = 1; i < tblDSD.rows.length; i++) {
        if (tblDSD.rows[i].cells[0].childNodes.length > 0 && tblDSD.rows[i].cells[0].childNodes[0].checked == true) {
            SelectedCodelistValue = tblDSD.rows[i].cells[1].childNodes[0].innerHTML;
            SelectedText = tblDSD.rows[i].cells[0].childNodes[0].value;
            break;
        }
    }
    if (z('hSelectedCodelist').value == 'aAgeCodelistSelect') {
        z('hdnSelectedAgeCodelist').value = SelectedCodelistValue;
        SetSelectedText(SelectedText.toUpperCase(), 'spanAgeCodelist');
        BindCodelistMappingLists(false, 'aAgeCodelistSelect');
    }
    else if (z('hSelectedCodelist').value == 'aLocationCodelistSelect') {
        z('hdnSelectedLocationCodelist').value = SelectedCodelistValue;
        SetSelectedText(SelectedText.toUpperCase(), 'spanLocationCodelist');
        BindCodelistMappingLists(false, 'aLocationCodelistSelect');
    }
    else if (z('hSelectedCodelist').value == 'aSexCodelistSelect') {
        z('hdnSelectedSexCodelist').value = SelectedCodelistValue;
        SetSelectedText(SelectedText.toUpperCase(), 'spanSexCodelist');
        BindCodelistMappingLists(false, 'aSexCodelistSelect');
    }

    CodelistPopupCancel();
}

function SetSelectedCodelistsInSpan(ageCodeListId, sexCodeListId, locationCodeistId) {
    z('hdnSelectedAgeCodelist').value = ageCodeListId;
    z('hOrginialSelectedAgeCodelist').value = ageCodeListId;
    SetSelectedText(ageCodeListId.replace('CL_', ''), 'spanAgeCodelist');
    z('hdnSelectedLocationCodelist').value = locationCodeistId;
    z('hOrginialSelectedLocCodelist').value = locationCodeistId;
    SetSelectedText(locationCodeistId.replace('CL_', ''), 'spanLocationCodelist');
    z('hdnSelectedSexCodelist').value = sexCodeListId;
    z('hOrginialSelectedSexCodelist').value = sexCodeListId;
    SetSelectedText(sexCodeListId.replace('CL_', ''), 'spanSexCodelist');
}

function SetSelectedText(text, spanId) {
    z(spanId).innerHTML = '';
    z(spanId).title = '';
    if (text.length > 100) {
        z(spanId).innerHTML = text.substr(0, 100) + "...";
        z(spanId).title = text;
    }
    else {
        z(spanId).innerHTML = text;
        z(spanId).title = text;
    }
}

function CodelistPopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();
    d('#CodelistPopup').hide('slow');
}

function ClearCodelistSelections(object) {
    if (object.id == 'aAgeCodelistClear') {
        z('hdnSelectedAgeCodelist').value = '';
        z('spanAgeCodelist').innerHTML = '';
        z('spanAgeCodelist').title = '';
        CollapseCodelistOnClear('imgdivAgeMapping', 'divAgeMapping');
    }
    else if (object.id == 'aLocationCodelistClear') {
        z('hdnSelectedLocationCodelist').value = '';
        z('spanLocationCodelist').innerHTML = '';
        z('spanLocationCodelist').title = '';
        CollapseCodelistOnClear('imgdivLocationMapping', 'divLocationMapping');
    }
    else if (object.id == 'aSexCodelistClear') {
        z('hdnSelectedSexCodelist').value = '';
        z('spanSexCodelist').innerHTML = '';
        z('spanSexCodelist').title = '';
        CollapseCodelistOnClear('imgdivSexMapping', 'divSexMapping');
    }
}

function GetCodelistDivInnerHTML() {
    try {
        var InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];

        var htmlResp = d.ajax({
            // var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=294&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divCodelist').innerHTML = data;
                    }
                    if (z('hSelectedCodelist').value == 'aAgeCodelistSelect') {
                        if (z('radio_' + z('hdnSelectedAgeCodelist').value) != null) {
                            z('radio_' + z('hdnSelectedAgeCodelist').value).checked = true;
                        }
                    }
                    else if (z('hSelectedCodelist').value == 'aLocationCodelistSelect') {
                        if (z('radio_' + z('hdnSelectedLocationCodelist').value) != null) {
                            z('radio_' + z('hdnSelectedLocationCodelist').value).checked = true;
                        }
                    }
                    else if (z('hSelectedCodelist').value == 'aSexCodelistSelect') {
                        if (z('radio_' + z('hdnSelectedSexCodelist').value) != null) {
                            z('radio_' + z('hdnSelectedSexCodelist').value).checked = true;
                        }
                    }
                    LanguageHandlingOfCodelistPopupCaptions();
                }
                catch (ex) {
                    alert("message:" + ex.message);
                }
            },
            error: function () {
                //       
            },
            cache: false
        });
    }
    catch (ex) {
    }
}

function EnableDisableCodelistLinks(enableFlag) {
    if (enableFlag == false) {
        EnableDisableLinks(z('aAgeCodelistSelect'), true);
        EnableDisableLinks(z('aAgeCodelistClear'), true);
    }
    else {
        EnableDisableLinks(z('aAgeCodelistSelect'), false);
        EnableDisableLinks(z('aAgeCodelistClear'), false);
    }
}

function ExpandCollapseList(ImageToUse, ControlToToggle) {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    var hdnSelectedAgeCodelist, hdnSelectedSexCodelist, hdnSelectedLocationCodelist;
    hdnSelectedAgeCodelist = z('hdnSelectedAgeCodelist').value;
    hdnSelectedSexCodelist = z('hdnSelectedSexCodelist').value;
    hdnSelectedLocationCodelist = z('hdnSelectedLocationCodelist').value;
    if ((hdnSelectedAgeCodelist == "" | hdnSelectedAgeCodelist == null | hdnSelectedAgeCodelist == undefined) && ImageToUse == 'imgdivAgeMapping') {
        alert("Please Select DevInfo Codelist");
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
    else if ((hdnSelectedSexCodelist == "" | hdnSelectedSexCodelist == null | hdnSelectedSexCodelist == undefined) && ImageToUse == 'imgdivSexMapping') {
        alert("Please Select DevInfo Codelist");
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
    else if ((hdnSelectedLocationCodelist == "" | hdnSelectedLocationCodelist == null | hdnSelectedLocationCodelist == undefined) && ImageToUse == 'imgdivLocationMapping') {
        alert("Please Select DevInfo Codelist");
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
    else {
        if ((z(ControlToToggle).style.display == "") || (z(ControlToToggle).style.display == "block")) {
            z(ImageToUse).src = "../../stock/themes/default/images/expand.png";
            document.getElementById(ControlToToggle).style.display = 'none';
            //di_jq('#' + ControlToToggle).hide('slow');
        }
        else {
            z(ImageToUse).src = "../../stock/themes/default/images/collapse.png";
            document.getElementById(ControlToToggle).style.display = 'block';
            //di_jq('#' + ControlToToggle).show('slow');
        }
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}


function ExpandCollapseDivs(ImageToUse, ControlToToggle) {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if ((z(ControlToToggle).style.display == "") || (z(ControlToToggle).style.display == "block")) {
        z(ImageToUse).src = "../../stock/themes/default/images/expand.png";
        document.getElementById(ControlToToggle).style.display = 'none';

    }
    else {
        z(ImageToUse).src = "../../stock/themes/default/images/collapse.png";
        document.getElementById(ControlToToggle).style.display = 'block';

    }
    HideLoadingDiv();
    RemoveMaskingDiv();

}
function CollapseCodelistOnClear(ImageToUse, ControlToToggle) {
    if ((z(ControlToToggle).style.display == "") || (z(ControlToToggle).style.display == "block")) {
        z(ImageToUse).src = "../../stock/themes/default/images/expand.png";
        //di_jq('#' + ControlToToggle).hide('slow');
        document.getElementById(ControlToToggle).style.display = 'none';
    }
}

function ConfirmationOnSavingNewMapping() {
    var RetVal;
    var originalSelectedAgeCodelist, originalSelectedSexCodelist, originalSelectedLocCodelist, selectedAgeCodelist, selectedSexCodelist, selectedLocCodelist;
    RetVal = '';
    selectedAgeCodelist = z('hdnSelectedAgeCodelist').value;
    selectedSexCodelist = z('hdnSelectedSexCodelist').value;
    selectedLocCodelist = z('hdnSelectedLocationCodelist').value;
    originalSelectedAgeCodelist = z('hOrginialSelectedAgeCodelist').value;
    originalSelectedSexCodelist = z('hOrginialSelectedSexCodelist').value;
    originalSelectedLocCodelist = z('hOrginialSelectedLocCodelist').value;
    if (selectedAgeCodelist != originalSelectedAgeCodelist || selectedSexCodelist != originalSelectedSexCodelist || selectedLocCodelist != originalSelectedLocCodelist) {
        PreviousMappingsRemoved = document.getElementById('hPreviousMappingsRemoved').value;
        RetVal = PreviousMappingsRemoved;  //'Your Previous Mappings Will Get Removed';
    }
    return RetVal;
}
function LanguageHandlingOfCodelistMappingDivs(dIndicator, dUnit, dArea, dAge, dSex, dLocation) {

    if (dIndicator != false) {
        document.getElementById("aShowIndicatorAll").innerHTML = document.getElementById('hShowAll').value;
        document.getElementById("aShowIndicatorMapped").innerHTML = document.getElementById('hMapped').value;
        document.getElementById("aShowIndicatorUnMapped").innerHTML = document.getElementById('hUnMapped').value;
        document.getElementById("aShowIndicatorUnSaved").innerHTML = document.getElementById('hUnSaved').value;
        document.getElementById("lang_DevInfo_Indicator").innerHTML = document.getElementById('hDevInfoIndicator').value;
        document.getElementById("lang_Indicator_GIds").innerHTML = document.getElementById('hIndicatorGIds').value;
        document.getElementById("lang_UNSD_Indicator").innerHTML = document.getElementById('hUNSDIndicator').value;
        document.getElementById("lang_Indicator_Ids").innerHTML = document.getElementById('hIndicatorIds').value;
    }
    if (dUnit != false) {
        document.getElementById("aShowUnitAll").innerHTML = document.getElementById('hShowAll').value;
        document.getElementById("aShowUnitMapped").innerHTML = document.getElementById('hMapped').value;
        document.getElementById("aShowUnitUnMapped").innerHTML = document.getElementById('hUnMapped').value;
        document.getElementById("aShowUnitUnSaved").innerHTML = document.getElementById('hUnSaved').value;
        document.getElementById("lang_DevInfo_Unit").innerHTML = document.getElementById('hDevInfoUnit').value;
        document.getElementById("lang_Unit_GIds").innerHTML = document.getElementById('hUnitGIds').value;
        document.getElementById("lang_UNSD_Unit").innerHTML = document.getElementById('hUNSDUnit').value;
        document.getElementById("lang_Unit_Ids").innerHTML = document.getElementById('hUnitIds').value;
    }
    if (dArea != false) {

        document.getElementById("aShowAreaAll").innerHTML = document.getElementById('hShowAll').value;
        document.getElementById("aShowAreaMapped").innerHTML = document.getElementById('hMapped').value;
        document.getElementById("aShowAreaUnMapped").innerHTML = document.getElementById('hUnMapped').value;
        document.getElementById("aShowAreaUnSaved").innerHTML = document.getElementById('hUnSaved').value;
        document.getElementById("lang_DevInfo_Area").innerHTML = document.getElementById('hDevInfoArea').value;
        document.getElementById("lang_Area_GIds").innerHTML = document.getElementById('hAreaGIds').value;
        document.getElementById("lang_UNSD_Area").innerHTML = document.getElementById('hUNSDArea').value;
        document.getElementById("lang_Area_Ids").innerHTML = document.getElementById('hAreaIds').value;
    }

    if (dAge != false) {
        document.getElementById("aShowAgeAll").innerHTML = document.getElementById('hShowAll').value;
        document.getElementById("aShowAgeMapped").innerHTML = document.getElementById('hMapped').value;
        document.getElementById("aShowAgeUnMapped").innerHTML = document.getElementById('hUnMapped').value;
        document.getElementById("aShowAgeUnSaved").innerHTML = document.getElementById('hUnSaved').value;
        document.getElementById("lang_DevInfo_Age").innerHTML = document.getElementById('hDevInfoAge').value;
        document.getElementById("lang_Age_GIds").innerHTML = document.getElementById('hAgeGIds').value;
        document.getElementById("lang_UNSD_Age").innerHTML = document.getElementById('hUNSDAge').value;
        document.getElementById("lang_Age_Ids").innerHTML = document.getElementById('hAgeIds').value;
    }
    if (dSex != false) {
        document.getElementById("aShowSexAll").innerHTML = document.getElementById('hShowAll').value;
        document.getElementById("aShowSexMapped").innerHTML = document.getElementById('hMapped').value;
        document.getElementById("aShowSexUnMapped").innerHTML = document.getElementById('hUnMapped').value;
        document.getElementById("aShowSexUnSaved").innerHTML = document.getElementById('hUnSaved').value;
        document.getElementById("lang_DevInfo_Sex").innerHTML = document.getElementById('hDevInfoSex').value;
        document.getElementById("lang_Sex_GIds").innerHTML = document.getElementById('hSexGIds').value;
        document.getElementById("lang_UNSD_Sex").innerHTML = document.getElementById('hUNSDSex').value;
        document.getElementById("lang_Sex_Ids").innerHTML = document.getElementById('hSexIds').value;
    }
    if (dLocation != false) {
        document.getElementById("aShowLocationAll").innerHTML = document.getElementById('hShowAll').value;
        document.getElementById("aShowLocationMapped").innerHTML = document.getElementById('hMapped').value;
        document.getElementById("aShowLocationUnMapped").innerHTML = document.getElementById('hUnMapped').value;
        document.getElementById("aShowLocationUnSaved").innerHTML = document.getElementById('hUnSaved').value;
        document.getElementById("lang_DevInfo_Location").innerHTML = document.getElementById('hDevInfoLocation').value;
        document.getElementById("lang_Location_GIds").innerHTML = document.getElementById('hLocationGIds').value;
        document.getElementById("lang_UNSD_Location").innerHTML = document.getElementById('hUNSDLocation').value;
        document.getElementById("lang_Location_Ids").innerHTML = document.getElementById('hLocationIds').value;
    }
    LanguageHandlingOfSelectOptions();

}


function ApplyCustomSelect(dIndicator, dUnit, dArea, dAge, dSex, dLocation) {
    if (dIndicator != false) {
        d("#divIndicatorMapping").find('select.cus_slct_dd').each(function (index, domelem) {
            var eleid = this.id;

            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDIndicator') });
        });
    }
    if (dUnit != false) {
        d("#divUnitMapping").find('select.cus_slct_dd').each(function (index, domelem) {
            var eleid = this.id;

            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDUnit') });
        });
    }
    if (dArea != false) {
        d("#divAreaMapping").find('select.cus_slct_dd').each(function (index, domelem) {
            var eleid = this.id;

            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDArea') });
        });
    }

    if (dAge != false) {
        d("#divAgeMapping").find('select.cus_slct_dd').each(function (index, domelem) {
            var eleid = this.id;

            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDAge') });
        });
    }
    if (dSex != false) {
        d("#divSexMapping").find('select.cus_slct_dd').each(function (index, domelem) {
            var eleid = this.id;

            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDSex') });
        });
    }
    if (dLocation != false) {

        d("#divLocationMapping").find('select.cus_slct_dd').each(function (index, domelem) {
            var eleid = this.id;

            d(this).bind('mouseenter', function () { AddOptionstoSelect(eleid, 'ddlUNSDLocation') });
        });
    }


}

function LanguageHandlingOfIUSMappingDiv() {
    document.getElementById("aShowIUSAll").innerHTML = document.getElementById('hShowAll').value;
    document.getElementById("aShowIUSMapped").innerHTML = document.getElementById('hMapped').value;
    document.getElementById("aShowIUSUnMapped").innerHTML = document.getElementById('hUnMapped').value;
    document.getElementById("aShowIUSUnSaved").innerHTML = document.getElementById('hUnSaved').value;

    document.getElementById("lang_Indicator").innerHTML = document.getElementById('hIndicator').value;
    document.getElementById("lang_Unit").innerHTML = document.getElementById('hUnit').value;
    document.getElementById("lang_Subgroup").innerHTML = document.getElementById('hSubgroup').value;
    document.getElementById("lang_Mapped").innerHTML = document.getElementById('hMap').value;
    document.getElementById("lang_Unit_UNSD").innerHTML = document.getElementById('hUnit').value;
    document.getElementById("lang_Age").innerHTML = document.getElementById('hAge').value;
    document.getElementById("lang_Sex").innerHTML = document.getElementById('hSex').value;
    document.getElementById("lang_Location").innerHTML = document.getElementById('hLocation').value;
    document.getElementById("lang_Frequency").innerHTML = document.getElementById('hFrequency').value;
    document.getElementById("lang_SourceType").innerHTML = document.getElementById('hSourceType').value;
    document.getElementById("lang_Nature").innerHTML = document.getElementById('hNature').value;
    document.getElementById("lang_UnitMultiplier").innerHTML = document.getElementById('hUnitMultiplier').value;

    LanguageHandlingOfSelectOptions();
}

function LanguageHandlingOfMetadataMappingDiv() {
    document.getElementById("aShowMetadataAll").innerHTML = document.getElementById('hShowAll').value;
    document.getElementById("aShowMetadataMapped").innerHTML = document.getElementById('hMapped').value;
    document.getElementById("aShowMetadataUnMapped").innerHTML = document.getElementById('hUnMapped').value;
    document.getElementById("aShowMetadataUnSaved").innerHTML = document.getElementById('hUnSaved').value;
    document.getElementById("lang_DevInfo_Metadata").innerHTML = document.getElementById('hDevInfoMetadata').value;
    document.getElementById("lang_Category_GIds").innerHTML = document.getElementById('hCategoryGIds').value;
    document.getElementById("lang_UNSD_Metadata").innerHTML = document.getElementById('hUNSDMetadata').value;
    document.getElementById("lang_Concept_Ids").innerHTML = document.getElementById('hConceptIds').value;

    LanguageHandlingOfSelectOptions();
}
function LanguageHandlingOfCodelistPopupCaptions() {
    document.getElementById("lang_CodelistId").innerHTML = document.getElementById('hCodelistId').value;
    document.getElementById("lang_CodelistAgencyId").innerHTML = document.getElementById('hCodelistAgencyId').value;
    document.getElementById("lang_CodelistVersion").innerHTML = document.getElementById('hCodelistVersion').value;
    document.getElementById("lang_CodelistName").innerHTML = document.getElementById('hCodelistName').value;
}
function LanguageHandlingOfSelectOptions() {
    try {
        for (var i = 0; i < document.getElementsByTagName("option").length; i++) {
            if (document.getElementsByTagName("option")[i].id.indexOf("SelectUNSD") != -1) {
                document.getElementsByTagName("option")[i].innerHTML = document.getElementById('hSelect').value;
            }
        }
    }
    catch (ex) {
        alert("Error : " + ex.message);
    }
}

function ExportMappingExcel(SelectedMappingType) {
    //    if (ValidateCodelistMappingData()) {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    var SourceName = getDefaultDSDName(getDefaultDbId());
    var TargetName = getDefaultDSDName(z('hdbnid').value);
    var SelectedAgeCodelist = z('hdnSelectedAgeCodelist').value;
    var SelectedSexCodelist = z('hdnSelectedSexCodelist').value;
    var SelectedLocationCodelist = z('hdnSelectedLocationCodelist').value;

    var InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + SourceName;
    InputParam += ParamDelimiter + TargetName;
    InputParam += ParamDelimiter + SelectedAgeCodelist;
    InputParam += ParamDelimiter + SelectedSexCodelist;
    InputParam += ParamDelimiter + SelectedLocationCodelist
    InputParam += ParamDelimiter + SelectedMappingType;
    var htmlResp = d.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=1043&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                if (data != '') {
                    var ExcelFileUrl = getAbsURL('stock') + data;
                    window.open(ExcelFileUrl);
                }
                HideLoadingDiv();
                RemoveMaskingDiv();
            }
            catch (ex) {
                alert("Error : " + ex.message);
                HideLoadingDiv();
                RemoveMaskingDiv();
            }
        },
        error: function () {
            //  
            HideLoadingDiv();
            RemoveMaskingDiv();
        },
        cache: false
    });
   
}

function ImportMappingExcel(SelectedMappingType, SaveFilePath) {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    var UserNId = z('hLoggedInUserNId').value.split('|')[0];
    var AgeCodeListGID = z('hdnSelectedAgeCodelist').value;
    var SexCodeListGId = z('hdnSelectedSexCodelist').value;
    var LocationCodeListGID = z('hdnSelectedLocationCodelist').value;
   
    var InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + UserNId;
    InputParam += ParamDelimiter + AgeCodeListGID;
    InputParam += ParamDelimiter + SexCodeListGId;
    InputParam += ParamDelimiter + LocationCodeListGID;
    InputParam += ParamDelimiter + SaveFilePath;
    InputParam += ParamDelimiter + SelectedMappingType;
    var htmlResp = d.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=1047&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                if (data == "true") {
                    if (z('Lang_ImportSuccess').innerHTML != 'undefined' && z('Lang_ImportSuccess').innerHTML != undefined && z('Lang_ImportSuccess').innerHTML != "") {
                        alert(z('Lang_ImportSuccess').innerHTML);
                    }
                    else {
                        alert(z('Lang_ImportSuccess').innerText);
                    }
                    if (SelectedMappingType == "0") {
                        BindCodelistMappingLists(false, null);
                    }
                    else if (SelectedMappingType == "1") {
                        BindIUSMappingList(false);
                    }
                    else if (SelectedMappingType == "2") {
                        BindMetadataMappingList(false);
                    }
                }
                else {
                    var resultArray = data.split("[**]");
                    if (resultArray[1] != null && resultArray[1] != undefined && resultArray[1] != "undefined") {
                        alert(resultArray[1]);
                    }
                    else if (z('Lang_ImportSuccess').innerHTML != 'undefined' && z('Lang_ImportSuccess').innerHTML != undefined && z('Lang_ImportSuccess').innerHTML != "") {
                        alert(z('Lang_ImportFaliure').innerHTML);
                    }
                    else {
                        alert(z('Lang_ImportFaliure').innerText);
                    }
                }
                HideLoadingDiv();
                RemoveMaskingDiv();
                HideUploadExcellPopup();
            }
            catch (ex) {
                alert("Error : " + ex.message);
                HideLoadingDiv();
                HideUploadExcellPopup();
            }
        },
        error: function () {
            //   
            HideLoadingDiv();
            RemoveMaskingDiv();
            HideUploadExcellPopup();
        },
        cache: false
    });
 
}


function OpenUploadExcellPopup(SelectedMappingType) {
    document.getElementById("ImportMappingExcel").value = '';
    MappingType = SelectedMappingType;
    ApplyMaskingDiv(100);
    ShowLoadingDiv();

    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#divImportExcel'), 'HideUploadExcellPopup', '../../stock/themes/default/images/close.png');
    di_jq("#divImportExcel").show('slow');
    GetWindowCentered(document.getElementById('divImportExcel'), 502, 255);
    HideLoadingDiv();
}
function HideUploadExcellPopup() {
    document.getElementById("ImportMappingExcel").value = '';
    di_jq('#divImportExcel').hide('slow');
    RemoveMaskingDiv();
}




function IncludeDivs(allAreaRows, allIndicatorRows, allUnitRows) {
    if (allAreaRows == "unsaved" && allIndicatorRows == "unsaved" && allUnitRows == "unsaved") {
        var tblCodelistIds = ["tblIndicator", "tblUnit", "tblArea"];
    }
    else if (allAreaRows == "unmapped" || allIndicatorRows == "unmapped" || allUnitRows == "unmapped") {
        if (allAreaRows == "unmapped") {
            if (allIndicatorRows == "unsaved" && allUnitRows == "unsaved") {
                var tblCodelistIds = ["tblIndicator", "tblUnit"];
            }
            else if (allIndicatorRows == "unsaved" || allUnitRows == "unsaved") {
                if (allIndicatorRows == "unsaved" && allUnitRows == "unmapped") {
                    var tblCodelistIds = ["tblIndicator"];
                }
                else if (allIndicatorRows == "unmapped" && allUnitRows == "unsaved") {
                    var tblCodelistIds = ["tblUnit"];
                }
                else if (allIndicatorRows == "unmapped" && allUnitRows == "") {
                    var tblCodelistIds = ["tblIndicator"];
                }
                else if (allIndicatorRows == "unsaved" && allUnitRows == "") {
                    var tblCodelistIds = ["tblIndicator"];
                }
                else {
                    var tblCodelistIds = ["tblUnit"];
                }

            }

        }
        if (allIndicatorRows == "unmapped") {
            if (allAreaRows == "unsaved" && allUnitRows == "unsaved") {
                var tblCodelistIds = ["tblUnit", "tblArea"];
            }
            else if (allAreaRows == "unsaved" || allUnitRows == "unsaved") {
                if (allAreaRows == "unsaved" && allUnitRows == "unmapped") {
                    var tblCodelistIds = ["tblArea"];
                }
                else if (allAreaRows == "unmapped" && allUnitRows == "unsaved") {
                    var tblCodelistIds = ["tblUnit"];
                }
                else if (allAreaRows == "unmapped" && allUnitRows == "") {
                    var tblCodelistIds = ["tblArea"];
                }
                else if (allAreaRows == "unsaved" && allUnitRows == "") {
                    var tblCodelistIds = ["tblArea"];
                }
                else {
                    var tblCodelistIds = ["tblUnit"];
                }
            }
        }
        if (allUnitRows == "unmapped") {
            if (allIndicatorRows == "unsaved" && allAreaRows == "unsaved") {
                var tblCodelistIds = ["tblIndicator", "tblArea"];
            }
            else if (allIndicatorRows == "unsaved" || allAreaRows == "unsaved") {
                if (allIndicatorRows == "unsaved" && allAreaRows == "unmapped") {
                    var tblCodelistIds = ["tblIndicator"];
                }
                else if (allIndicatorRows == "unmapped" && allAreaRows == "unsaved") {
                    var tblCodelistIds = ["tblArea"];
                }
                else if (allIndicatorRows == "unmapped" && allAreaRows == "") {
                    var tblCodelistIds = ["tblIndicator"];
                }
                else if (allIndicatorRows == "unsaved" && allAreaRows == "") {
                    var tblCodelistIds = ["tblIndicator"];
                }
                else {
                    var tblCodelistIds = ["tblArea"];
                }

            }
        }

    }

    else if (allAreaRows == "unsaved" || allIndicatorRows == "unsaved" || allUnitRows == "unsaved") {

        if (allAreaRows == "unsaved") {
            if ((allIndicatorRows == "" || allIndicatorRows == "unmapped") && (allUnitRows == "" || allUnitRows == "unmapped")) {
                var tblCodelistIds = ["tblArea"];
            }

            if (allIndicatorRows == "unsaved" && (allUnitRows == "" || allUnitRows == "unmapped")) {
                var tblCodelistIds = ["tblIndicator", "tblArea"];
            }

            if (allUnitRows == "unsaved" && (allIndicatorRows == "" || allIndicatorRows == "unmapped")) {
                var tblCodelistIds = ["tblUnit", "tblArea"];
            }

        } //main if
        if (allIndicatorRows == "unsaved") {
            if ((allAreaRows == "" || allAreaRows == "unmapped") && (allUnitRows == "" || allUnitRows == "unmapped")) {
                var tblCodelistIds = ["tblIndicator"];
            }

            if (allAreaRows == "unsaved" && (allUnitRows == "" || allUnitRows == "unmapped")) {
                var tblCodelistIds = ["tblIndicator", "tblArea"];
            }
            ////&& (allIndicatorRows == "" || allIndicatorRows == "unmapped")
            if (allUnitRows == "unsaved" && (allAreaRows == "" || allAreaRows == "unmapped")) {
                var tblCodelistIds = ["tblIndicator", "tblUnit"];
            }

        } //main if
        if (allUnitRows == "unsaved") {
            if ((allIndicatorRows == "" || allIndicatorRows == "unmapped") && (allAreaRows == "" || allAreaRows == "unmapped")) {
                var tblCodelistIds = ["tblUnit"];
            }
            // && (allUnitRows == "" || allUnitRows == "unmapped")
            if (allIndicatorRows == "unsaved" && (allAreaRows == "" || allAreaRows == "unmapped")) {
                var tblCodelistIds = ["tblIndicator", "tblUnit"];
            }

            if (allAreaRows == "unsaved" && (allIndicatorRows == "" || allIndicatorRows == "unmapped")) {
                var tblCodelistIds = ["tblUnit", "tblArea"];
            }

        } //main if

    }

    return tblCodelistIds;
}


function BindAreaLevels() {
    var UserNId = z('hLoggedInUserNId').value.split('|')[0];
    var InputParam = z('hOriginaldbnid').value;
    var selectedVal = '';
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + UserNId;
    var OptionsArray = '';
    var htmlResp = d.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=1050&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                if (data) {
                    var mySelect = di_jq('#selectAreaLevel');
                    OptionsArray = data.split("[**[R]**]");

                    for (i = 0; i < OptionsArray.length; i++) {
                        OptionTextAndValue = OptionsArray[i].split("[**[C]**]");
                        if (OptionTextAndValue[0] != undefined) {
                            var defaultSelected = false;
                            var nowSelected = false;
                            if (OptionTextAndValue[2] == "true") {
                                nowSelected = true;
                                di_jq('#selectAreaLevel').append(new Option(OptionTextAndValue[0], OptionTextAndValue[1], defaultSelected, nowSelected));
                                selectedVal = OptionTextAndValue[1];

                            }
                            else {
                                di_jq('#selectAreaLevel').append(new Option(OptionTextAndValue[0], OptionTextAndValue[1], defaultSelected, nowSelected))
                            }
                        }
                    }
                }


                di_jq("#selectAreaLevel option[value='" + selectedVal + "']").attr('selected', true);
            }
            catch (ex) {
                alert("Error : " + ex.message);

            }
        },
        error: function () {

        },
        cache: false
    });
    //}
}


