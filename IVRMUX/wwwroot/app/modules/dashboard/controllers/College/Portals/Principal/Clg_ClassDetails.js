(function () {
    'use strict';
    angular
        .module('app')
        .controller('Clg_ClassDetails', Clg_ClassDetails)

    Clg_ClassDetails.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function Clg_ClassDetails($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        //======================page load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("Clg_ClassDetails/loaddata", pageid).then(function (promise) {
                $scope.allacademicyear = promise.allacademicyear;                
            })
        };
        $scope.tablegrid = false;
        $scope.flag = false;
        $scope.getcourse = function () {
            $scope.AMCO_Id = "";
            $scope.courselist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("Clg_ClassDetails/getcourse", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            })
        }
        $scope.report = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
            }
            apiService.create("Clg_ClassDetails/report", data).then(function (promise) {
                $scope.flag = true;
                if (promise != null && promise != undefined) {
                        $scope.tablegraph = true;
                        $scope.tablegrid = true;
                        $scope.asmaY_Id = promise.asmaY_Id;
                        $scope.yr = '2018-2019';
                        $scope.alldata11 = promise.reportlist.length;
                        $scope.categorylist1 = promise.categorylist.length;
                        $scope.yearfee = [];
                        $scope.yearfee.push({ yr: $scope.yr, r: $scope.alldata11})
                        $scope.loadcharts();
                }
            })
        }
       
        $scope.loadcharts = function () { 
            $scope.newadmissionstdtotal = [];
            $scope.newadmissionstdtotal1 = [];
            if ($scope.alldata11 != null) {               
                $scope.newadmissionstdtotal.push({ label: 'Course Wise Strength', "y": $scope.alldata11 })                
            }
            if ($scope.categorylist1 != null) {
                $scope.newadmissionstdtotal1.push({ label: 'Category Wise Strength', "y": $scope.categorylist1 })
            }
            var chart = new CanvasJS.Chart("columnchart",
                {
                    height: 350,
                    width: 1030,
                    axisX: {
                        labelFontSize: 24,
                        labelAngle: -6,
                    },
                    axisY: {
                        labelFontSize: 24,
                    },
                    data: [
                        {
                            type: "column",
                            showInLegend: true,
                            dataPoints: $scope.newadmissionstdtotal
                        }
                    ]
                });
            chart.render();
            var chart = new CanvasJS.Chart("columnchart1",
                {
                    height: 350,
                    width: 1030,
                    axisX: {
                        labelFontSize: 24,
                        labelAngle: -6,
                    },
                    axisY: {
                        labelFontSize: 24,
                    },
                    data: [
                        {
                            type: "column",
                            showInLegend: true,
                            dataPoints: $scope.newadmissionstdtotal1
                        }
                    ]
                });

            chart.render();
        }
        //======================Record Save
        $scope.search = "";
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }
})();

