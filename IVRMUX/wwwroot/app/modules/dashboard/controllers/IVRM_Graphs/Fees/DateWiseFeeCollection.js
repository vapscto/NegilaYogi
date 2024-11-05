(function () {
    'use strict';
    angular
        .module('app')
        .controller('DateWiseFeeCollectionController', DateWiseFeeCollectionController)
    DateWiseFeeCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function DateWiseFeeCollectionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.route = false;
        $scope.print = false;

        $scope.search = "";
        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var copty;
        $scope.coptyright = copty;
        $scope.totaldata = false;
        var count = 0;
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}


        //  $scope.imgname = logopath;
        $scope.loaddata = function () {
            $scope.grid_view = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.reporttype = false;
            $scope.wise = true;



            apiService.getURI("DateWiseFeeCollection/getalldetails", pageid).
                then(function (promise) {

                    $scope.arrlist6 = promise.adcyear;
                            $scope.obj.asmaY_Id=promise.asmaY_Id;
 $scope.ShowReport ()

                })
        }

        $scope.onstudentchange = function () {

            $scope.yearmodel = false;
        }



        $scope.onselectyear = function () {

            $scope.yearmodel = false;
            $scope.studentlist = [];
            $scope.checkallhrd = $scope.arrlist6.every(function (itm) {

                return itm.selected;
            });

            var ASMAY_Ids = [];
            angular.forEach($scope.arrlist6, function (ty) {
                if (ty.selected) {
                    ASMAY_Ids.push(ty.asmaY_Id);
                }
            })
            var data = {
                // "ASMAY_Id": $scope.asmaY_Id
                ASMAY_Ids: ASMAY_Ids

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("DateWiseFeeCollection/onchangeacademic", data).
                then(function (promise) {
                    $scope.studentlist = promise.studentname;


                })
        }




        // $scope.checkallhrd = true;
        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.arrlist6, function (itm) {
                itm.selected = toggleStatus1;
            });
        }











        // $scope.FMCB_fromDATE = new Date();
        //$scope.FMCB_toDATE = new Date();

        $scope.onclickloaddata = function () {
             $scope.feedetails=[];
            if ($scope.rpttyp == "year") {
                count = 0;
                $scope.totaldata = false;
                $scope.yearmodel = false;
                $scope.datemodel = false;                $scope.monthmodel = false;

            }
            else if ($scope.rpttyp == "date") {
                count = 0;
                $scope.totaldata = false;
                $scope.datemodel = false;
                $scope.yearmodel = false;                $scope.monthmodel = false;


            }
            else if ($scope.rpttyp == "month") {
                count = 0;
                $scope.totaldata = false;
                $scope.monthmodel = false;
                $scope.yearmodel = false;                $scope.datemodel = false;
            }
        }

        $scope.submitted = false;
        $scope.ShowReport = function () {

            if ($scope.myForm.$valid) {
                //$scope.from_date = ($scope.FMCB_fromDATE).toDateString();
                //$scope.to_date =($scope.FMCB_toDATE).toDateString();



                if ($scope.rpttyp == "date") {
                    var data = {
                        // ASMAY_Ids: ASMAY_Ids

                        "ASMAY_Id": $scope.obj.asmaY_Id,
                        "type": $scope.rpttyp,
                        "fromdate": new Date($scope.obj.FMCB_fromDATE).toDateString(),
                        "todate": new Date($scope.obj.FMCB_toDATE).toDateString()
                        //  "amstid": $scope.AMST_Id,


                    }
                }
                else if ($scope.rpttyp == "month") {
                    var data = {
                        // ASMAY_Ids: ASMAY_Ids

                        "ASMAY_Id": $scope.obj.asmaY_Id,
                        "type": $scope.rpttyp,

                        //  "amstid": $scope.AMST_Id,


                    }
                }
                else if ($scope.rpttyp == "year") {
                    var data = {



                        "type": $scope.rpttyp,



                    }
                }



                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("DateWiseFeeCollection/radiobtndata", data).
                    then(function (promise) {
                        $scope.Cash = []; $scope.total = []; $scope.Bank = []; $scope.Online = []; $scope.monthname = []; $scope.fypdate = []; $scope.academicyear = [];
                        if (promise.studentalldata.length > 0 && promise.studentalldata.length != null) {
                            $scope.totaldata = true;
                            $scope.feedetails = promise.studentalldata;
                            if ($scope.rpttyp == "date") {
                                $scope.datemodel = true;
                                $scope.yearmodel = false;
                                $scope.monthmodel = false;
                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.fypdate.push(new Date(promise.studentalldata[i].FYP_Date));
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Online.push(promise.studentalldata[i].Onlineamt);
                                }


                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Bank.push(promise.studentalldata[i].Bank);
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Cash.push(promise.studentalldata[i].Cash);
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.total.push(promise.studentalldata[i].Total);
                                }

                            }
                            else if ($scope.rpttyp == "year") {

                                $scope.yearmodel = true;
                                $scope.datemodel = false;

                                $scope.monthmodel = false;
                                $scope.feedetails = promise.studentalldata;


                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.academicyear.push(promise.studentalldata[i].ASMAY_Year);
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Online.push(promise.studentalldata[i].Online);
                                }


                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Bank.push(promise.studentalldata[i].Bank);
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Cash.push(promise.studentalldata[i].Cash);
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.total.push(promise.studentalldata[i].total);
                                }

                            }
                            else if ($scope.rpttyp == "month") {
                                $scope.monthmodel = true;
                                $scope.datemodel = false;
                                $scope.yearmodel = false;
                                $scope.feedetails = promise.studentalldata;


                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.monthname.push(promise.studentalldata[i].IVRM_Month_Name);
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Online.push(promise.studentalldata[i].Onlineamt);
                                }


                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Bank.push(promise.studentalldata[i].Bank);
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.Cash.push(promise.studentalldata[i].Cash);
                                }

                                for (var i = 0; i < promise.studentalldata.length; i++) {
                                    $scope.total.push(promise.studentalldata[i].Total);
                                }
                            }



                            if (count == 0 || count == 1) {

                                $scope.ShowReport();
                                count = count + 1;

                            }


                          

                            function createChart1() {                                $("#chart1").kendoChart({                                    title: {                                        text: "DateWise Fee Collection Graph"                                    },                                    legend: {                                        position: "bottom"                                    },                                    chartArea: {                                        background: ""                                    },                                    seriesDefaults: {                                        type: "line",                                        style: "smooth"                                    },                                    series: [{                                        name: "Online",                                        data: $scope.Online                                    },                                    {                                        name: "Bank",                                        data: $scope.Bank                                    }, {                                        name: "Cash",                                        data: $scope.Cash                                    }, {                                        name: "Total",                                        data: $scope.total                                    }],                                    valueAxis: {                                        labels: {                                            format: "{0}"                                        },                                        line: {                                            visible: false                                        },                                        axisCrossingValue: -10                                    },                                    categoryAxis: {                                        categories: $scope.fypdate,                                        majorGridLines: {                                            visible: false                                        },                                        labels: {                                            rotation: "auto"                                        }                                    },                                    tooltip: {                                        visible: true,                                        format: "{0}",                                        template: "#= series.name #: #= value #"                                    }                                });                            }                            $(document).ready(createChart1);                            $(document).bind("kendo:skinChange", createChart1);                            $("#chart2").kendoChart({
                                title: {
                                    text: "DateWise Fee Collection"
                                },
                                legend: {
                                    position: "top"
                                },
                                seriesDefaults: {
                                    type: "column"
                                },
                                series: [{                                    name: "Online",                                    data: $scope.Online,                                },                                {                                    name: "Bank",                                    data: $scope.Bank,                                }, {                                    name: "Cash",                                    data: $scope.Cash,
                                },
                                {                                    name: "Total",                                    data: $scope.total,
                                }],
                                valueAxis: {
                                    labels: {
                                        format: "{0}"
                                    },
                                    line: {
                                        visible: false
                                    },
                                    axisCrossingValue: 0
                                },
                                categoryAxis: {
                                    categories: $scope.fypdate,
                                    line: {
                                        visible: false
                                    },
                                    labels: {
                                        padding: { top: 35 }
                                    }
                                },
                                tooltip: {
                                    visible: true,
                                    format: "{0}",
                                    template: "#= series.name #: #= value #"
                                }
                            });
                            var drawing = kendo.drawing;
                            var geometry = kendo.geometry;
                            function createChart() {
                                $("#chart4").kendoChart({
                                    title: {
                                        text: "Month Wise Fee Collection"
                                    },
                                    legend: {
                                        position: "bottom",
                                        item: {
                                            visual: createLegendItem
                                        }
                                    },
                                    seriesDefaults: {
                                        type: "column",
                                        stack: true,
                                        highlight: {
                                            toggle: function (e) {

                                                e.preventDefault();

                                                var visual = e.visual;
                                                var opacity = e.show ? 0.8 : 1;

                                                visual.opacity(opacity);
                                            }
                                        },
                                        visual: function (e) {
                                            return createColumn(e.rect, e.options.color);
                                        }
                                    },


                                    series: [{                                        name: "Online",                                        data: $scope.Online                                    },                                    {                                        name: "Bank",                                        data: $scope.Bank                                    }, {                                        name: "Cash",                                        data: $scope.Cash                                    }, {                                        name: "Total",                                        data: $scope.total                                    }],
                                    panes: [{
                                        clip: false
                                    }],
                                    valueAxis: {
                                        line: {
                                            visible: false
                                        }
                                    },
                                    categoryAxis: {
                                        categories: $scope.monthname,
                                        majorGridLines: {
                                            visible: false
                                        },
                                        line: {
                                            visible: false
                                        }
                                    },
                                    tooltip: {
                                        visible: true
                                    }
                                });
                            }

                            function createColumn(rect, color) {
                                var origin = rect.origin;
                                var center = rect.center();
                                var bottomRight = rect.bottomRight();
                                var radiusX = rect.width() / 2;
                                var radiusY = radiusX / 3;
                                var gradient = new drawing.LinearGradient({
                                    stops: [{
                                        offset: 0,
                                        color: color
                                    }, {
                                        offset: 0.5,
                                        color: color,
                                        opacity: 0.9
                                    }, {
                                        offset: 0.5,
                                        color: color,
                                        opacity: 0.9
                                    }, {
                                        offset: 1,
                                        color: color
                                    }]
                                });

                                var path = new drawing.Path({
                                    fill: gradient,
                                    stroke: {
                                        color: "none"
                                    }
                                }).moveTo(origin.x, origin.y)
                                    .lineTo(origin.x, bottomRight.y)
                                    .arc(180, 0, radiusX, radiusY, true)
                                    .lineTo(bottomRight.x, origin.y)
                                    .arc(0, 180, radiusX, radiusY);

                                var topArcGeometry = new geometry.Arc([center.x, origin.y], {
                                    startAngle: 0,
                                    endAngle: 360,
                                    radiusX: radiusX,
                                    radiusY: radiusY
                                });

                                var topArc = new drawing.Arc(topArcGeometry, {
                                    fill: {
                                        color: color
                                    },
                                    stroke: {
                                        color: "#ebebeb"
                                    }
                                });
                                var group = new drawing.Group();
                                group.append(path, topArc);
                                return group;
                            }

                            function createLegendItem(e) {
                                var color = e.options.markers.background;
                                var labelColor = e.options.labels.color;
                                var rect = new geometry.Rect([0, 0], [120, 50]);
                                var layout = new drawing.Layout(rect, {
                                    spacing: 5,
                                    alignItems: "center"
                                });

                                var overlay = drawing.Path.fromRect(rect, {
                                    fill: {
                                        color: "#fff",
                                        opacity: 0
                                    },
                                    stroke: {
                                        color: "none"
                                    },
                                    cursor: "pointer"
                                });

                                var column = createColumn(new geometry.Rect([0, 0], [15, 10]), color);
                                var label = new drawing.Text(e.series.name, [0, 0], {
                                    fill: {
                                        color: labelColor
                                    }
                                })

                                layout.append(column, label);
                                layout.reflow();

                                var group = new drawing.Group().append(layout, overlay);

                                return group;
                            }

                            $(document).ready(createChart);
                            $(document).bind("kendo:skinChange", createChart);                            $("#chart5").kendoChart({
                                title: {
                                    text: "Month Wise Collection Report"
                                },
                                legend: {
                                    position: "top"
                                },
                                seriesDefaults: {
                                    type: "column"
                                },
                                series: [{                                    name: "Online",                                    data: $scope.Online                                },                                {                                    name: "Bank",                                    data: $scope.Bank                                }, {                                    name: "Cash",                                    data: $scope.Cash                                }, {                                    name: "Total",                                    data: $scope.total                                }],
                                valueAxis: {
                                    labels: {
                                        format: "{0}"
                                    },
                                    line: {
                                        visible: false
                                    },
                                    axisCrossingValue: 0
                                },
                                categoryAxis: {
                                    categories: $scope.monthname,
                                    line: {
                                        visible: false
                                    },
                                    labels: {
                                        padding: { top: 35 }
                                    }
                                },
                                tooltip: {
                                    visible: true,
                                    format: "{0}",
                                    template: "#= series.name #: #= value #"
                                }
                            });                            function createCharttest() {
                                $("#chart6").kendoChart({
                                    title: {
                                        text: "Year Wise Collection"
                                    },
                                    legend: {
                                        position: "bottom"
                                    },
                                    chartArea: {
                                        background: ""
                                    },
                                    seriesDefaults: {
                                        type: "line",
                                        style: "smooth"
                                    },
                                    series: [{                                        name: "Online",                                        data: $scope.Online,                                    },                                    {                                        name: "Bank",                                        data: $scope.Bank,                                    }, {                                        name: "Cash",                                        data: $scope.Cash,
                                    },
                                    {                                        name: "Total",                                        data: $scope.total,
                                    }],
                                    valueAxis: {
                                        labels: {
                                            format: "{0}"
                                        },
                                        line: {
                                            visible: false
                                        },
                                        axisCrossingValue: -10
                                    },
                                    categoryAxis: {
                                        categories: $scope.academicyear,
                                        majorGridLines: {
                                            visible: false
                                        },
                                        labels: {
                                            rotation: "auto"
                                        }
                                    },
                                    tooltip: {
                                        visible: true,
                                        format: "{0}",
                                        template: "#= series.name #: #= value #"
                                    }
                                });
                            }

                            $(document).ready(createCharttest);
                            $(document).bind("kendo:skinChange", createCharttest);

                        }
                        else {
                            swal("Record Not Found");
                        }







                    })
            }
            else {
                $scope.submitted = true;

            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
            //|| field.$dirty;
        };



        $scope.printdatatable = [];
        $scope.exportToExcel = function () {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var tableid = '';
                if ($scope.allorindiorcon == "all") {
                    if ($scope.result == 'FHW') {
                        tableid = '#table2';
                    }
                    else if ($scope.result == 'FSW') {
                        tableid = '#table1';
                    }
                    else if ($scope.result == 'FCW') {
                        tableid = '#table3';
                    }
                    else if ($scope.result == 'FRW') {
                        tableid = '#table4';
                    }

                }
                else {
                    if ($scope.report == 'WO') {
                        tableid = '#table5';
                    }
                    else if ($scope.report == 'AA') {
                        tableid = '#table6';
                    }
                    else {
                        tableid = '#table7';
                    }
                }

                var exportHref = Excel.tableToExcel(tableid, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
            //  $state.reload();

        }


        $scope.printData = function () {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = "";
                if ($scope.allorindiorcon == "all") {
                    if ($scope.result == 'FHW') {
                        innerContents = document.getElementById("printSectionIdhad").innerHTML;
                    }
                    else if ($scope.result == 'FSW') {
                        innerContents = document.getElementById("printSectionIdstd1").innerHTML;
                    }

                    else if ($scope.result == 'FCW') {
                        innerContents = document.getElementById("printSectionIdcls").innerHTML;
                    }
                    else if ($scope.result == 'FRW') {
                        innerContents = document.getElementById("printSectionIdgrp").innerHTML;
                    }

                }
                else {
                    if ($scope.report == 'WO') {
                        innerContents = document.getElementById("printwaive").innerHTML;
                    }
                    else if ($scope.report == 'AA') {
                        innerContents = document.getElementById("printadjust").innerHTML;
                    }
                    else {
                        innerContents = document.getElementById("printOB").innerHTML;
                    }
                }
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
                // $state.reload();
            }
            else {
                swal("Please Select Records to be Printed");
            }
            //  $state.reload();
        }




        $scope.Clearid = function () {

            $scope.yearmodel = false;
        }




    }
})();
