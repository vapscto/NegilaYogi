
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CallDashboardController2', CallDashboardController2);

    CallDashboardController2.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce'];
    function CallDashboardController2($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
        $scope.editEmployee = {};
        $scope.loadData = function () {
            var pageid = 2;
            apiService.getURI("CallDashboard/loadData", pageid).then(function (promise) {

                if (promise.flag == "valid") {
                  
                    $scope.IMCS_AssignedCall = promise.reportlist[0].imcS_AssignedCall;
                    $scope.IMCS_InboundCalls = promise.reportlist[0].imcS_InboundCalls;
                    $scope.IMCS_OutboundCalls = promise.reportlist[0].imcS_OutboundCalls;
                    $scope.IMCS_AvailableCalls = promise.reportlist[0].imcS_AvailableCalls;

                    $scope.received = promise.reportdatelist[0].ReceivedCount;
                    $scope.ConnectedCount = promise.reportdatelist[0].ConnectedCount;
                    $scope.NotConnectedCount = promise.reportdatelist[0].NotConnectedCount;
                    $scope.sum = promise.reportdatelist2[0].sum;
                    var chart1 = new CanvasJS.Chart("chartContainer1", {
                        animationEnabled: true,
                        theme: "light2",
                        title: {
                            text: "Call Flow"
                        },
                        axisY: {
                            includeZero: false
                        },
                        data: [{
                            type: "line",
                            dataPoints: [
                                { y: $scope.IMCS_AssignedCall, label: "Assigned Calls" },
                                { y: $scope.IMCS_InboundCalls, label: "Inbound Calls" },
                                { y: $scope.IMCS_OutboundCalls, label: "Outbound Calls" },
                                { y: $scope.IMCS_AvailableCalls, label: "Available Calls" }
                            ]
                        }]
                    });
                    chart1.render();
                    var chart = new CanvasJS.Chart("chartContainer", {
                        animationEnabled: true,
                        title: {
                            text: "Call Status"
                        },
                        data: [{
                            type: "pie",
                            startAngle: 240,
                            yValueFormatString: "##0\"\"",
                            indexLabel: "{label} {y}",
                            toolTipContent: "{y} - #percent %",
                            dataPoints: [
                                { y: $scope.received, label: "Received call" },
                                { y: $scope.ConnectedCount, label: "Connected call" },
                                { y: $scope.NotConnectedCount, label: "Not Connected call" }
                            ]
                        }]
                    });
                    chart.render();
                }

                if (promise.flag == "invalid") {
           
                    $scope.IMCS_AssignedCall = promise.imcS_AssignedCall;
                    $scope.IMCS_InboundCalls = promise.imcS_InboundCalls;
                    $scope.IMCS_OutboundCalls = promise.imcS_OutboundCalls;
                    $scope.IMCS_AvailableCalls = promise.imcS_AvailableCalls;

                    $scope.received = promise.received;
                    $scope.ConnectedCount = promise.connectedCount;
                    $scope.NotConnectedCount = promise.notConnectedCount;
                    $scope.sum = promise.sum;
                    var chart1 = new CanvasJS.Chart("chartContainer1", {
                        animationEnabled: true,
                        theme: "light2",
                        title: {
                            text: "Call Flow"
                        },
                        axisY: {
                            includeZero: false
                        },
                        data: [{
                            type: "line",
                            dataPoints: [
                                { y: $scope.IMCS_AssignedCall, label: "Assigned Calls" },
                                { y: $scope.IMCS_InboundCalls, label: "Inbound Calls" },
                                { y: $scope.IMCS_OutboundCalls, label: "Outbound Calls" },
                                { y: $scope.IMCS_AvailableCalls, label: "Available Calls" }
                            ]
                        }]
                    });
                    chart1.render();
                    var chart = new CanvasJS.Chart("chartContainer", {
                        animationEnabled: true,
                        title: {
                            text: "Call Status"
                        },
                        data: [{
                            type: "pie",
                            startAngle: 240,
                            yValueFormatString: "##0\"\"",
                            indexLabel: "{label} {y}",
                            toolTipContent: "{y} - #percent %",
                            dataPoints: [
                                { y: $scope.received, label: "Received call" },
                                { y: $scope.ConnectedCount, label: "Connected call" },
                                { y: $scope.NotConnectedCount, label: "Not Connected call" }
                            ]
                        }]
                    });
                    chart.render();
                }
            })
        }
    }
})();