(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentWiseFeeCollectionController', StudentWiseFeeCollectionController)
    StudentWiseFeeCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function StudentWiseFeeCollectionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.route = false;
        $scope.print = false;


        $scope.search = "";

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var copty;
        $scope.coptyright = copty;
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

            $scope.asmaY_Id = "";

            apiService.getURI("StudentWiseFeeCollection/getalldetails", pageid).
                then(function (promise) {

                    $scope.arrlist6 = promise.adcyear;
                    $scope.studentlist = promise.studentname;
                    $scope.asmaY_Id = promise.asmaY_Id;
                    $scope.onselectyear()
                   // $scope.ShowReport(); 
                })
        }

        $scope.onstudentchange = function () {

            $scope.yearmodel = false;
        }



        $scope.onselectyear = function () {

          
            var data = {
             
                "ASMAY_Id": $scope.asmaY_Id,
             
            }
         
            

            apiService.create("StudentWiseFeeCollection/onchangeacademic", data).
                then(function (promise) {

                    $scope.classlist = promise.classlist;
                    $scope.sectionlist = promise.sectionlist;
                    $scope.studentlist = promise.studentname;


                })
        }

        $scope.onselectclass = function () {


            var data = {

                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id":$scope.ASMCL_Id

            }



            apiService.create("StudentWiseFeeCollection/onselectclass", data).
                then(function (promise) {

                  
                 
                    $scope.studentlist = promise.studentname;


                })
        }

        $scope.onselectsection = function () {



                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id":$scope.ASMS_Id

                }

           



            apiService.create("StudentWiseFeeCollection/onselectsection", data).
                then(function (promise) {


                  
                    $scope.studentlist = promise.studentname;


                })
        }



        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.arrlist6, function (itm) {
                itm.selected = toggleStatus1;
            });
        }
     
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.submitted = false;
        $scope.ShowReport = function () {

            if ($scope.myForm.$valid) {



                var ASMAY_Ids = [];
                angular.forEach($scope.arrlist6, function (ty) {
                    if (ty.selected) {
                        ASMAY_Ids.push(ty.asmaY_Id);
                    }
                })

                var data = {
                   // ASMAY_Ids: ASMAY_Ids
                   "ASMAY_Id": $scope.asmaY_Id,
                   "amstid": $scope.AMST_Id,


                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StudentWiseFeeCollection/radiobtndata", data).
                    then(function (promise) {
                   
                        if (promise.studentalldata.length > 0 && promise.studentalldata.length != null) {
                            $scope.yearmodel = true;
                            $scope.feedetails = promise.studentalldata;
                            $scope.academicyear = [];
                            $scope.acalength = [];
                            //
                            $scope.asmaylist = [];
                        
                            $scope.exportsheet = true;

                            if (promise.studentalldata.length > 0) {
                                $scope.get_approvalreport = promise.studentalldata;
                                angular.forEach($scope.get_approvalreport, function (dev) {
                                    if ($scope.asmaylist.length === 0) {

                                        $scope.asmaylist.push(dev);
                                      $scope.academicyear.push(dev.ASMAY_Year);

                                    } else if ($scope.asmaylist.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.asmaylist, function (emp) {
                                            if (emp.ASMAY_Id === dev.ASMAY_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.asmaylist.push(dev);
                                           $scope.academicyear.push(dev.ASMAY_Year);
                                        }
                                    }
                                });

                                angular.forEach($scope.asmaylist, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach($scope.get_approvalreport, function (dd) {
                                        if (dd.ASMAY_Id === ddd.ASMAY_Id) {
                                            $scope.templist.push(dd);
                                        }
                                    });
                                    ddd.feedetails = $scope.templist;
                                    $scope.acalength.push($scope.templist.length);
                                });
                            }

                            //
                            //for (var i = 0; i < promise.studentalldata.length; i++) {
                            //    $scope.academicyear.push(promise.studentalldata[i].ASMAY_Year);
                            //}

                            $scope.feename = [];
                            for (var i = 0; i < promise.studentalldata.length; i++) {
                                $scope.feename.push(promise.studentalldata[i].FMH_FeeName);
                            }
                            $scope.tobepaid = [];
                            for (var i = 0; i < promise.studentalldata.length; i++) {
                                $scope.tobepaid.push(promise.studentalldata[i].tobepaid);
                            }

                            $scope.paid = [];
                            for (var i = 0; i < promise.studentalldata.length; i++) {
                                $scope.paid.push(promise.studentalldata[i].paid);
                            }
                            $scope.Pendingcount = [];
                            for (var i = 0; i < promise.studentalldata.length; i++) {
                                $scope.Pendingcount.push(promise.studentalldata[i].Pending);
                            }


                           
                 
                        

                            function createChart1() {

                                $("#chart1").kendoChart({
                                    title: {
                                        text: "Student Wise Graph"
                                    },
                                    legend: {
                                        position: "top"
                                    },
                                    seriesDefaults: {
                                        type: "column"
                                    },
                                    series: [{
                                        name: "To Be Paid",
                                        data: $scope.tobepaid,
                                    },
                                    {
                                        name: "Paid",
                                        data: $scope.paid,
                                    }, {
                                        name: "Pending",
                                        data: $scope.Pendingcount,
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
                                    categoryAxis: [
                                        {
                                            categories: $scope.feename,
                                            majorGridLines: {
                                                visible: false
                                            },
                                            labels: {
                                                rotation: "auto"
                                            }
                                        },
                                        {
                                            majorTicks: {
                                              //  step: $scope.acalength,
                                            },
                                            categories: $scope.academicyear,
                                            valueAxis: [{
                                               majorGridLines: {
                                                    visible: false
                                                }
                                            }],
                                             
                                            labels: {
                                                visible: true,
                                            
                                                rotation: {
                                                    angle: "auto"
                                                }
                                            },
                                            majorGridLines: {
                                               
                                           
                                                color: "black",
                                                visible: true
                                            },
                                       

                                        }],

                                  
                       
                                    tooltip: {
                                        visible: true,
                                        format: "{0}",
                                        template: "#= series.name #: #= value #"
                                    }
                                });



                            }

                            $(document).ready(createChart1);
                            $(document).bind("kendo:skinChange", createChart1);




                            var drawing = kendo.drawing;
                            var geometry = kendo.geometry;
                            function createChart2() {
                                $("#chart2").kendoChart({
                                    title: {
                                        text: "Student Wise Graph"
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
                                                // Don't create a highlight overlay,
                                                // we'll modify the existing visual instead
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
                                    series: [{
                                        name: "To Be Paid",
                                        data: $scope.tobepaid,
                                    },
                                    {
                                        name: "Paid",
                                        data: $scope.paid,
                                    }, {
                                        name: "Pending",
                                        data: $scope.Pendingcount,
                                    }],
                                    panes: [{
                                        clip: false
                                    }],
                                    valueAxis: {
                                        line: {
                                            visible: false
                                        }
                                    },
                                    categoryAxis: [
                                        {
                                            categories: $scope.feename,
                                            majorGridLines: {
                                                visible: false
                                            },
                                            labels: {
                                                rotation: "auto",
                                               
                                            }
                                        },
                                        {
                                            categories: $scope.academicyear,
                                           // length:$scope.acalength,

                                            majorGridLines: {
                                                visible: false
                                            },
                                            labels: {
                                                rotation: "auto"
                                            }
                                        }],
                               
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

                            $(document).ready(createChart2);
                            $(document).bind("kendo:skinChange", createChart2);



                            


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
