(function () {
    'use strict';
    angular
        .module('app')
        .controller('AcademicYearWiseCollectionController', AcademicYearWiseCollectionController)
    AcademicYearWiseCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function AcademicYearWiseCollectionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.route = false;
        $scope.print = false;


        $scope.search = "";

        $scope.ddate = {};
        $scope.ddate = new Date();
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



            apiService.getURI("StudentWiseFeeCollection/getalldetails", pageid).
                then(function (promise) {

                    $scope.arrlist6 = promise.adcyear;
                    $scope.studentlist = promise.studentname;
                    $scope.checkallhrd = true;
                    $scope.hrdallcheck();
                    $scope.ShowReport();


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


            apiService.create("AcademicYearWiseCollection/onchangeacademic", data).
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
                    ASMAY_Ids: ASMAY_Ids
                    //"ASMAY_Id": $scope.asmaY_Id,
                    //  "amstid": $scope.AMST_Id,


                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("AcademicYearWiseCollection/radiobtndata", data).
                    then(function (promise) {

                        if (promise.studentalldata.length > 0 && promise.studentalldata.length != null) {
                            $scope.yearmodel = true;
                            $scope.feedetails = promise.studentalldata;
                            $scope.academicyear = [];
                            $scope.acalength = [];

                            $scope.asmaylist = [];

                           

                            $scope.ASMAY_Year = [];
                            for (var i = 0; i < promise.studentalldata.length; i++) {
                                $scope.ASMAY_Year.push(promise.studentalldata[i].ASMAY_Year);
                            }
                            $scope.Pending = [];
                            for (var i = 0; i < promise.studentalldata.length; i++) {
                                $scope.Pending.push(promise.studentalldata[i].Pending);
                            }
                            $scope.Paid = [];
                            for (var i = 0; i < promise.studentalldata.length; i++) {
                                $scope.Paid.push(promise.studentalldata[i].Paid);
                            }

                            $scope.Payable = [];
                            for (var i = 0; i < promise.studentalldata.length; i++) {
                                $scope.Payable.push(promise.studentalldata[i].Payable);
                            }





                            function createChart1() {

                                $("#chart1").kendoChart({
                                    title: {
                                        text: "Academic Year Wise Graph"
                                    },
                                    legend: {
                                        position: "top"
                                    },
                                    seriesDefaults: {
                                        type: "column"
                                    },
                                    series: [{
                                        name: "Payable",
                                        data: $scope.Payable,
                                    },
                                    {
                                        name: "Paid",
                                        data: $scope.Paid,
                                    },
                                    {
                                        name: "Pending",
                                        data: $scope.Pending,
                                    }
                                    ],
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
                                            categories: $scope.ASMAY_Year,
                                            majorGridLines: {
                                                visible: false
                                            },
                                            labels: {
                                                rotation: "auto"
                                            }
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


                            $("#chart2").kendoChart({
                                title: {
                                    text: "Academic Year Wise Fee Collection"
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
                                series: [{
                                    name: "Payable",
                                    data: $scope.Payable,
                                },
                                {
                                    name: "Paid",
                                    data: $scope.Paid,
                                },
                                {
                                    name: "Pending",
                                    data: $scope.Pending,
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
                                    categories: $scope.ASMAY_Year,
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
