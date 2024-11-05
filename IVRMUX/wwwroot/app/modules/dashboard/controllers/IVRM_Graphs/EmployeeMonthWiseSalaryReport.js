(function () {
    'use strict'; angular.module('app')
        .controller('EmployeeMonthWiseSalaryReportController', EmployeeMonthWiseSalaryReportController)
    EmployeeMonthWiseSalaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function EmployeeMonthWiseSalaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {



        $("#chart123").kendoChart({
            title: {
                text: "EMPLOYEE WISE MONTHLY SALARY REPORT BAR GRAPH"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                color: "#2874A6 ",
                color: "#F3A00E",
                color: "#5D6D7E",
                color: "#979A9A",
                color: "#7B241C",
                name: "Jan",
                data: [252000, 274000, 278000, 310000]
            }, {
                    name: "Feb",
                    data: [200000, 241000, 241000, 188000]
            }, {
                    name: "Mar",
                    data: [215000, 218000, 204000, 216000]
            }, 
{
                    name: "April",
                    data: [215000, 218000, 20400, 216000]
            }, 
{
                    name: "May",
                    data: [215000, 218000, 60000, 216000]
            }, 
{
                    name: "Jun",
                    data: [85000, 70000, 204000, 216000]
            }, 
{
                    name: "Jul",
                    data: [40000, 80000, 204000, 216000]
            }, 
{
                    name: "Aug",
                    data: [215000, 218000, 55000, 216000]
            }, 
{
                    name: "Sep",
                    data: [80000, 218000, 60000, 216000]
            }, 
{
                    name: "Oct",
                    data: [215000, 12500, 204000, 216000]
            }, 
{
                    name: "Nov",
                    data: [215000, 65000, 204000, 216000]
            }, 
{
                    name: "Dec",
                    data: [6000, 218000, 10000, 216000]
            }, ],
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
                // categories: [Software, Accounts, ManagementAdmin, Admin, HumanResourse, HumanResourse],
                categories: ["2018-19", "2019-20", "2020-21", "2021-22"],
                // categories: ids1,
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






        function createChart1() {
            $("#chart1").kendoChart({
                title: {
                    text: "EMPLOYEE WISE MONTHLY SALARY REPORT LINE GRAPH"
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
                    color: "#2874A6 ",
                    color: "#F3A00E",
                    color: "#5D6D7E",
                    color: "#979A9A",
                    color: "#7B241C",
                    name: "Jan",
                     data: [252000, 274000, 278000, 310000]
            }, {
                    name: "Feb",
                    data: [200000, 241000, 241000, 188000]
            }, {
                    name: "Mar",
                    data: [215000, 218000, 204000, 216000]
            }, 
{
                    name: "April",
                    data: [215000, 218000, 20400, 216000]
            }, 
{
                    name: "May",
                    data: [215000, 218000, 60000, 216000]
            }, 
{
                    name: "Jun",
                    data: [85000, 70000, 204000, 216000]
            }, 
{
                    name: "Jul",
                    data: [40000, 80000, 204000, 216000]
            }, 
{
                    name: "Aug",
                    data: [215000, 218000, 55000, 216000]
            }, 
{
                    name: "Sep",
                    data: [80000, 218000, 60000, 216000]
            }, 
{
                    name: "Oct",
                    data: [215000, 12500, 204000, 216000]
            }, 
{
                    name: "Nov",
                    data: [215000, 65000, 204000, 216000]
            }, 
{
                    name: "Dec",
                    data: [6000, 218000, 10000, 216000]
            }, ],
                valueAxis: {
                    labels: {
                        format: "{0}"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -20
                },
                categoryAxis: {
                    categories: ["2019-20", "2020-21", "2021-22"],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value #"
                }
            });
        }

        $(document).ready(createChart1);
        $(document).bind("kendo:skinChange", createChart1);


        // Get form Details at onload 
        $scope.onLoadGetData = function () {

        }




    }
})();



