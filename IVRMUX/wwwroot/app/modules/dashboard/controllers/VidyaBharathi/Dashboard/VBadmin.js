
(function () {
    'use strict';
    angular.module('app').controller('VBadminController', VBadminController);
    VBadminController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http'];
    function VBadminController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 3;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 3;
        $scope.searchValue = '';
        
        $scope.LoadData = function () {
            var pageid = 2;
            apiService.getURI("VBadmin/LoadData", pageid).then(function (promise) {

                $scope.getsateusers = promise.getsateusers;
                $scope.presentCountgrid = $scope.getsateusers.length;

                $scope.getdistrictusers = promise.getdistrictusers;
                $scope.presentCountgrid1 = $scope.getdistrictusers.length;


                if (promise.getgraphsreport !== null && promise.getgraphsreport.length > 0) {
                    $scope.statecount = promise.getgraphsreport[0].statecount;
                    $scope.districtcount = promise.getgraphsreport[0].districtcount;
                }

                var chart = new CanvasJS.Chart("chartContainer", {
                    animationEnabled: true,
                    axisY: {
                        titleFontColor: "#6599FF",
                        lineColor: "#6599FF",
                        labelFontColor: "#6599FF",
                        tickColor: "#6599FF"
                    },
                    axisY2: {
                        titleFontColor: "#FF9900",
                        lineColor: "#FF9900",
                        labelFontColor: "#FF9900",
                        tickColor: "#FF9900"
                    },
                    toolTip: {
                        shared: true
                    },
                    legend: {
                        cursor: "pointer",
                        itemclick: toggleDataSeries
                    },
                    data: [{
                        type: "column",
                        name: "state",
                        legendText: "state",
                        color: "#6599FF",
                        showInLegend: true,
                        dataPoints: $scope.statecount
                    },
                    {
                        type: "column",
                        name: " District",
                        legendText: "District",
                        axisYType: "secondary",
                        color: "#FF9900",
                        showInLegend: true,
                        dataPoints: $scope.districtcount
                    }]
                });
                chart.render();
                function toggleDataSeries(e) {
                    if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                        e.dataSeries.visible = false;
                    }
                    else {
                        e.dataSeries.visible = true;
                    }
                    chart.render();
                }


            });
        };
        
        $scope.ViewCOEDetails = function () {
            var id = 2;
            apiService.getURI("VBadmin/ViewCOEDetails", id).then(function (promise) {
                
                if (promise !== null) {
                    if (promise.getCOEEventDetails !== null && promise.getCOEEventDetails.length > 0) {
                        $scope.GetCOEEventDetails = promise.getCOEEventDetails;
                        $scope.Eventcount = $scope.GetCOEEventDetails.length;
                        $('#myModalcoe').modal('show');
                    } else {
                        swal("No Record Found");
                    }
                }
            });
        };
        

    }
})();