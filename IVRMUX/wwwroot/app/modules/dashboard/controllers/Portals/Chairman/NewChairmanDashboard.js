
(function () {
    'use strict';
    angular
        .module('app')
        .controller('NewChairmanDashboardController', NewChairmanDashboardController);
    NewChairmanDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce', 'uiCalendarConfig', 'appSettings'];

    function NewChairmanDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce, uiCalendarConfig, appSettings) {
        var miid = "";

        $scope.closeupdate = false;
        $scope.conformflag = true;

        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });



       

        $scope.showtext = false;
        $scope.showtextdetails = "";      
        $scope.currentPage1 = 1;
        $scope.currentPage5 = 1;
        $scope.currentPage4 = 1;
        $scope.currentPageS = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage5 = 10;
        $scope.itemsPerPage4 = 10;
        $scope.itemsPerPageS = 10;

        

        $scope.loaddata = function () {            $scope.currentPage = 1;                   $scope.search = "";            var pageid = 2;                                   apiService.getDATA("NewChairmanDashboard/Getdetails", pageid).then(function (promise) {                $scope.studentcount = promise.get_levl[0].studentcount;                $scope.employeescount = promise.get_levl[0].employeescount;                $scope.Paymentcount = promise.get_levl[0].Paymentcount;                $scope.Preadmissionscount = promise.get_levl[0].Preadmissionscount;                $scope.Admissioncount = promise.get_levl[0].Admissioncount;                $scope.Tccount = promise.get_levl[0].Tccount;                $scope.Salarycount = promise.get_levl[0].Salarycount;                $scope.passcount = promise.get_levl[0].passcount;                $scope.failcount = promise.get_levl[0].failcount;                $scope.Bookscount = promise.get_levl[0].Bookscount;                $scope.Eventscount = promise.get_levl[0].Eventscount;               $scope.Interactioncount = promise.get_levl[0].Interactioncount;                $scope.Transportscount = promise.get_levl[0].Transportscount;                $scope.absentcount = promise.get_levl[0].absentcount;                $scope.presentcount = promise.get_levl[0].presentcount;                $scope.Defaulterscount = promise.get_levl[0].Defaulterscount;            });        };

        $scope.showStudent = function () {            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";                       var data = {                                "flag": "Student"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.studentCount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.stdcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.stdcount.push(promise.getReport[i].studentcount);
                }                function createstudentchart() {                    $("#studentchart").kendoChart({                        title: {                            text: "Total Student"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total Student Count",                            data: $scope.stdcount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createstudentchart);                $(document).bind("kendo:skinChange", createstudentchart);            });        };

        $scope.showEmployee = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Employee"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.employeecount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.empcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.empcount.push(promise.getReport[i].Employeecount);
                }                function createemployeechart() {                    $("#employeechart").kendoChart({                        title: {                            text: "Total Employee"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total Employee Count",                            data: $scope.empcount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {                                visible: false                            },                            labels: {                                rotation: "auto"                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createemployeechart);                $(document).bind("kendo:skinChange", createemployeechart);            });        };

        $scope.showPayment = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Payment"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.paymentcount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.amountcollected = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.amountcollected.push(promise.getReport[i].FSS_PaidAmount);
                }
                $scope.amounttobepaid = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.amounttobepaid.push(promise.getReport[i].Tobepaid);
                }
                $scope.amountchargable = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.amountchargable.push(promise.getReport[i].FSS_CurrentYrCharges);
                }                function createpaymentchart() {                    $("#paymentchart").kendoChart({                        title: {                            text: "Total payment"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total Amount Collected",                            data: $scope.amountcollected,                        },                            {                                name: "Total Amount Due",                                data: $scope.amounttobepaid,                            },                            {                                name: "Total Amount Chargable",                                data: $scope.amountchargable,                            }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {                                visible: false                            },                            labels: {                                rotation: "auto"                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createpaymentchart);                $(document).bind("kendo:skinChange", createpaymentchart);            });        };


        $scope.showPreadmissions = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Preadmission"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.Preadmissioncount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.precount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.precount.push(promise.getReport[i].Preadmissioncount);
                }                function createPreadmissionchart() {                                        $("#Preadmissionchart").kendoChart({                            title: {                                text: "Total Preadmission"                            },                            legend: {                                position: "bottom"                            },                            chartArea: {                                background: ""                            },                            seriesDefaults: {                                type: "column"                            },                            series: [{                                name: "Total Preadmission Count",                                data: $scope.precount,                            }                            ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {                                visible: false                            },                            labels: {                                rotation: "auto"                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createPreadmissionchart);                $(document).bind("kendo:skinChange", createPreadmissionchart);            });        };

        $scope.showAdmissions = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Admission"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.admissioncount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.admcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.admcount.push(promise.getReport[i].admissioncount);
                }   
                

                function createadmissionchart() {                    $("#admissionchart").kendoChart({                        title: {                            text: "Total Admission"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total Admission Count",                            data: $scope.admcount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createadmissionchart);                $(document).bind("kendo:skinChange", createadmissionchart);            });        };


        $scope.showtcIssued = function () {
            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "TcIssued"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.tcIssuedcount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.tcissucount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.tcissucount.push(promise.getReport[i].tccount);
                }    
                function createtcchart() {                    $("#tcchart").kendoChart({                        title: {                            text: "Total Tc Issued"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total Tc Issued",                            data: $scope.tcissucount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }                                                   },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createtcchart);                $(document).bind("kendo:skinChange", createtcchart);            });        };

        $scope.showSalary = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Salary"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.salarycount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.salcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.salcount.push(promise.getReport[i].salarycount);
                }
                function createsalarychart() {                    $("#salarychart").kendoChart({                        title: {                            text: "Employee Salary"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Employee Salary",                            data: $scope.salcount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createsalarychart);                $(document).bind("kendo:skinChange", createsalarychart);            });        };


        $scope.showPassFail = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Result"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.Resultcount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.passcountS = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.passcountS.push(promise.getReport[i].PASS_PERCENTAGE);
                }                $scope.failcountS = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.failcountS.push(promise.getReport[i].FAIL_PERCENTAGE);
                }   


                function createresultchart() {                    $("#resultchart").kendoChart({                        title: {                            text: "Exam Result"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Pass COUNT",                            data: $scope.passcountS,                        },                        {                            name: "fAIL COUNT",                            data: $scope.failcountS,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createresultchart);                $(document).bind("kendo:skinChange", createresultchart);            });        };

        $scope.showBooks = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Book"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.Bookcount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.bokcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.bokcount.push(promise.getReport[i].bookcount);
                }     

                function createbookchart() {                    $("#bookchart").kendoChart({                        title: {                            text: "Total No:Of Books"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total No:Of Books",                            data: $scope.bokcount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createbookchart);                $(document).bind("kendo:skinChange", createbookchart);            });        };
        $scope.showEvents = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Event"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.Eventcount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.evecount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.evecount.push(promise.getReport[i].eventcount);
                }                function createventchart() {                    $("#eventchart").kendoChart({                        title: {                            text: "Total No:Of Events"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total No:Of Events",                            data: $scope.evecount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {
                                visible: false
                            },
                            labels: {
                                rotation: "auto"
                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createventchart);                $(document).bind("kendo:skinChange", createventchart);            });        };
     

        $scope.showTransport = function () {


            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Transport"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.Transportcount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.transcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.transcount.push(promise.getReport[i].transport);
                }                function createtransportchart() {                    $("#transportchart").kendoChart({                        title: {                            text: "Total No:Of Transport"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total No:Of Transport",                            data: $scope.transcount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {                                visible: false                            },                            labels: {                                rotation: "auto"                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createtransportchart);                $(document).bind("kendo:skinChange", createtransportchart);            });        };


        $scope.showDefaulters = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Defaulter"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.Defaultercount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.defalcount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.defalcount.push(promise.getReport[i].defaultercount);
                }     


                function createdefaulterchart() {                    $("#defaulterchart").kendoChart({                        title: {                            text: "Total No:Of Defaulter"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total No:Of Defaulter",                            data: $scope.defalcount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {                                visible: false                            },                            labels: {                                rotation: "auto"                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createdefaulterchart);                $(document).bind("kendo:skinChange", createdefaulterchart);            });        };

    

        $scope.showAttendance = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Attendence"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.Attendencecount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.absentcounts = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.absentcounts.push(promise.getReport[i].Absent);
                }                $scope.Presentcounts = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.Presentcounts.push(promise.getReport[i].present);
                }


                function createattendencechart() {                    $("#attendencechart").kendoChart({                        title: {                            text: "Attenence Count"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Absent Count",                            data: $scope.absentcounts,                        },                        {                            name: "Present Count",                            data: $scope.Presentcounts,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {                                visible: false                            },                            labels: {                                rotation: "auto"                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createattendencechart);                $(document).bind("kendo:skinChange", createattendencechart);            });        };

       


        $scope.showInteractions = function () {

            $scope.currentPage = 1;            $scope.itemsPerPage = 10;            $scope.search = "";            var data = {                "flag": "Interaction"            }            apiService.create("NewChairmanDashboard/ViewFiles", data).then(function (promise) {                $scope.Interactioncount = promise.getReport;                $scope.institutename = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.institutename.push(promise.getReport[i].MI_Name);
                }

                $scope.intercount = [];
                for (var i = 0; i < promise.getReport.length; i++) {
                    $scope.intercount.push(promise.getReport[i].interactioncount);
                }                function createinterchart() {                    $("#interchart").kendoChart({                        title: {                            text: "Total Interaction"                        },                        legend: {                            position: "bottom"                        },                        chartArea: {                            background: ""                        },                        seriesDefaults: {                            type: "column"                        },                        series: [{                            name: "Total Interaction Count",                            data: $scope.intercount,                        }                        ],                        valueAxis: {                            labels: {                                format: "{0}"                            },                            line: {                                visible: false                            },                            axisCrossingValue: -10                        },                        categoryAxis: {                            categories: $scope.institutename,                            majorGridLines: {                                visible: false                            },                            labels: {                                rotation: "auto"                            }                        },                        tooltip: {                            visible: true,                            format: "{0}",                            template: "#= series.name #: #= value #"                        }                    });                }                $(document).ready(createinterchart);                $(document).bind("kendo:skinChange", createinterchart);            });        };

    }
})();
