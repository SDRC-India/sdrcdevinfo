<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoogleMapTest1.aspx.cs" Inherits="libraries_aspx_GoogleMapTest1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="content-type" content="text/html; charset=UTF-8"/>

<title> Google Maps JavaScript API v3 Example: KmlLayer KML Features</title>
<link  href="http://code.google.com/apis/maps/documentation/javascript/examples/standard.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
<%--
<script type="text/javascript">
    function initialize() {
        var myLatlng = new google.maps.LatLng(55.4312553, 154.268845);
        var myOptions = {
            zoom: 1,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(
      document.getElementById("map_canvas"),
      myOptions);

        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(55.4312553, 154.268845),
            map: map,
            title: 'My workplace',
            clickable: true,
            icon: 'http://localhost:62298/DI%20WEB/stock/tempCYV/DI1884164.png'
        });
    }

    //    var ge;
    //    google.load("earth", "1");

    //    function init() {
    //        google.earth.createInstance('map3d', initCB, failureCB);
    //    }

    //    function initCB(instance) {
    //        ge = instance;
    //        ge.getWindow().setVisibility(true);
    //        ge.getNavigationControl().setVisibility(ge.VISIBILITY_SHOW);

    //        var la = ge.createLookAt('');
    //        la.set(55.4312553, 154.268845, 0, ge.ALTITUDE_RELATIVE_TO_GROUND, -8.541, 66.213, 20000);
    //        ge.getView().setAbstractView(la);

    //        // Create the GroundOverlay.
    //        var groundOverlay = ge.createGroundOverlay('');

    //        // Specify the image path and assign it to the GroundOverlay.
    //        var icon = ge.createIcon('');
    //        icon.setHref("http://localhost:62298/DI%20WEB/stock/tempCYV/DI1884164.png");
    //        groundOverlay.setIcon(icon);

    //        // Specify the geographic location.
    //        var latLonBox = ge.createLatLonBox('');
    //        latLonBox.setBox(55.4312553, 154.268845, -121.77, -121.85, 0);
    //        groundOverlay.setLatLonBox(latLonBox);

    //        // Add the GroundOverlay to Earth.
    //        ge.getFeatures().appendChild(groundOverlay);
    //    }

    //    function failureCB(errorCode) {
    //    }

    //    google.setOnLoadCallback(init);

</script>
--%>
<script type="text/javascript">
    function initialize() {
        var myLatlng = new google.maps.LatLng(55.4312553, -10.9418736);
        var myOptions = {
            zoom: 12,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(
      document.getElementById("map_canvas"),
      myOptions);

        var nyLayer = new google.maps.KmlLayer(
      'http://devinfolive.info/di7beta2/stock/map.kmz',
      { suppressInfoWindows: true,
          map: map
      });        
    }

</script>

<script type="text/javascript" src="https://www.google.com/jsapi?key=ABCDEFG"> </script>
   <script type="text/javascript">
       var ge;
       google.load("earth", "1");

       function init() {
           google.earth.createInstance('map3d', initCB, failureCB);
       }

       function initCB(instance) {
           ge = instance;
           ge.getWindow().setVisibility(true);

           // Earth is ready, we can add features to it
           addKmlFromUrl('http://devinfolive.info/di7beta2/stock/map.kmz');
       }

       function failureCB(errorCode) {
       }

       google.setOnLoadCallback(init);

       var map = new OpenLayers.Map("mapDiv");
       var gmapLayer = new OpenLayers.Layer.Google("GMaps");
       map.addLayer(gmapLayer);

       function addKmlFromUrl(kmlUrl) {
           var link = ge.createLink('');
           link.setHref(kmlUrl);

           var networkLink = ge.createNetworkLink('');
           networkLink.setLink(link);
           networkLink.setFlyToView(true);

           ge.getFeatures().appendChild(networkLink);
       }

   </script>


</head>

<body onload="initialize()">
    <div id="map_canvas" style="width:79%; height:100%; float:left"></div>
    <div id="content_window" style="width:19%; height:100%; float:left"></div>
    <div id="map3d" style="height: 400px; width: 600px;"></div>
  </body>

</html>

