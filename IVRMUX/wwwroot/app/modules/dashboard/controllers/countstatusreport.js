



(function () {
    'use strict';
    angular
        .module('app')
        .controller('countstatusreportController', countstatusreportController)

    countstatusreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'Excel', '$compile', '$timeout', 'superCache']
    function countstatusreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, Excel, $compile, $timeout, superCache) {
        // Load initial data

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        $scope.user = {};
        $scope.remarks = [];
        $scope.rptStatus = false;
        $scope.angularData = {
            'nameList': []
        };

        $scope.graphshow = false;

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }



        $scope.loadbasicdata = function () {

            apiService.get("countstatusreport/getinitialdata/").then(function (promise) {
                if (promise != "") {
                    $scope.academiclist = promise.academicList;
                    $scope.ShowReport_today();

                    //var chart = new CanvasJS.Chart("chartContainer", {
                    //    title: {
                    //        text: "My First Chart in CanvasJS"
                    //    },
                    //    data: [
                    //    {
                    //        // Change type to "doughnut", "line", "splineArea", etc.
                    //        type: "column",
                    //        dataPoints: [
                    //            { label: "SELECT", y: 10 },
                    //            { label: "SELECTED AND SEAT-BOOKED", y: 15 },
                    //            { label: "ON HOLD BY SCHOOL", y: 25 },
                    //            { label: "ON HOLD BY PARENT", y: 30 },
                    //            { label: "WAITING", y: 14 },
                    //            { label: "REJECTED", y: 45 },
                    //            { label: "REPEA", y: 121 }
                    //        ]
                    //    }
                    //    ]
                    //});
                    //chart.render();

                }
            });
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (keyname) {
            $scope.columnSort = true;
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // search report with  year, 
        $scope.ShowReport = function (year) {

            var data = {
                // "MI_Id": 2,
                //"ASMAY_Id": 10,
                "ASMAY_Id": $scope.asmaY_Id,

            };
            apiService.create("countstatusreport/Getdetails", data).
                then(function (promise) {

                    //$scope.reportdetails = promise.countreportstatus;
                    $scope.newuser = [];
                    $scope.appstatus = [];
                    $scope.graphshow = false;
                    if (promise.countreportstatus != null && promise.countreportstatus.length > 0) {
                        $scope.newuser = promise.countreportstatus;
                        $scope.graphshow = true;
                    }
                    if (promise.applicationstatus != null && promise.applicationstatus.length > 0) {
                        $scope.appstatus = promise.applicationstatus;
                        $scope.graphshow = true;
                    }
                    if (($scope.newuser.length == 0) && ($scope.appstatus.length == 0)) {
                        swal("No Record found!!");
                    }

                    $scope.chartdata = [];
                    angular.forEach($scope.newuser, function (value, index) {

                        $scope.chartdata.push({ 'label': value.PAMST_Status, 'y': value.Data_count });

                    });


                    var chart = new CanvasJS.Chart("chartContainer", {
                        title: {
                            text: " Chart Representation Of Admission Status Count "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "column",
                                dataPoints: $scope.chartdata
                            }
                        ]
                    });
                    chart.render();

                    var chart1 = new CanvasJS.Chart("chartContainer1", {
                        title: {
                            text: " "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "doughnut",
                                dataPoints: $scope.chartdata
                            }
                        ]
                    });
                    chart1.render();

                    //appilication status chart
                    $scope.selectedclass = [];
                    $scope.appname = [];
                    $scope.appdata = [];
                    angular.forEach($scope.appstatus, function (opt) {
                        if (opt.PASRAPS_ID == 787926) {
                            $scope.appname = "APP WAITING"
                            $scope.appdata = opt.data
                            $scope.selectedclass.push({ Id: $scope.appname, Data: $scope.appdata });
                        }
                        else if (opt.PASRAPS_ID == 787927) {
                            $scope.appname = "APP REJECTED"
                            $scope.appdata = opt.data
                            $scope.selectedclass.push({ Id: $scope.appname, Data: $scope.appdata });
                        }
                        else if (opt.PASRAPS_ID == 787928) {
                            $scope.appname = "APP ACCEPTED"
                            $scope.appdata = opt.data
                            $scope.selectedclass.push({ Id: $scope.appname, Data: $scope.appdata });
                        }
                    })
                    //$scope.selectedclass.push({ Id: $scope.appname, Data: $scope.appdata });


                    $scope.appchartdata = [];
                    angular.forEach($scope.selectedclass, function (value, index) {

                        $scope.appchartdata.push({ 'label': value.Id, 'y': value.Data });

                    });


                    var chart = new CanvasJS.Chart("appchartContainer", {
                        title: {
                            text: " Chart Representation Of Application Status Count "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "column",
                                dataPoints: $scope.appchartdata
                            }
                        ]
                    });
                    chart.render();

                    var chart1 = new CanvasJS.Chart("appchartContainer1", {
                        title: {
                            text: "  "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "doughnut",
                                dataPoints: $scope.appchartdata
                            }
                        ]
                    });
                    chart1.render();


                })
        };

        $scope.ShowReport_today = function (year) {

            var data = {
                //  "MI_Id": 2,
                //  "ASMAY_Id": 10,
                //   "ASMAY_Id": $scope.asmaY_Id,

            };
            apiService.create("countstatusreport/Getdetails", data).
                then(function (promise) {

                    //$scope.reportdetails = promise.countreportstatus;
                    $scope.newuser = promise.countreportstatus;
                    $scope.appstatus = promise.applicationstatus;
                    $scope.chartdata = [];
                    angular.forEach($scope.newuser, function (value, index) {

                        $scope.chartdata.push({ 'label': value.PAMST_Status, 'y': value.Data_count });

                    });


                    var chart = new CanvasJS.Chart("chartContainer", {
                        title: {
                            text: " Chart Representation Of Admission Status Count "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "column",
                                dataPoints: $scope.chartdata
                            }
                        ]
                    });
                    chart.render();

                    var chart1 = new CanvasJS.Chart("chartContainer1", {
                        title: {
                            text: "  "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "doughnut",
                                dataPoints: $scope.chartdata
                            }
                        ]
                    });
                    chart1.render();


                    //appilication status chart
                    $scope.selectedclass = [];
                    $scope.appname = [];
                    $scope.appdata = [];
                    angular.forEach($scope.appstatus, function (opt) {
                        if (opt.PASRAPS_ID == 787926) {
                            $scope.appname = "APP WAITING"
                            $scope.appdata = opt.data
                            $scope.selectedclass.push({ Id: $scope.appname, Data: $scope.appdata });
                        }
                        else if (opt.PASRAPS_ID == 787927) {
                            $scope.appname = "APP REJECTED"
                            $scope.appdata = opt.data
                            $scope.selectedclass.push({ Id: $scope.appname, Data: $scope.appdata });
                        }
                        else if (opt.PASRAPS_ID == 787928) {
                            $scope.appname = "APP ACCEPTED"
                            $scope.appdata = opt.data
                            $scope.selectedclass.push({ Id: $scope.appname, Data: $scope.appdata });
                        }
                    })
                    //$scope.selectedclass.push({ Id: $scope.appname, Data: $scope.appdata });


                    $scope.appchartdata = [];
                    angular.forEach($scope.selectedclass, function (value, index) {

                        $scope.appchartdata.push({ 'label': value.Id, 'y': value.Data });

                    });


                    var chart = new CanvasJS.Chart("appchartContainer", {
                        title: {
                            text: " Chart Representation Of Application Status Count "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "column",
                                dataPoints: $scope.appchartdata
                            }
                        ]
                    });
                    chart.render();

                    var chart1 = new CanvasJS.Chart("appchartContainer1", {
                        title: {
                            text: "  "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "doughnut",
                                dataPoints: $scope.appchartdata
                            }
                        ]
                    });
                    chart1.render();



                })
        };



    }

})();