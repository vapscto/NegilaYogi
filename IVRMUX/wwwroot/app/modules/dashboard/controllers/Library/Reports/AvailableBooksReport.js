(function () {
    'use strict';
    angular
        .module('app')
        .controller('AvailableBooksReportController', AvailableBooksReportController)

    AvailableBooksReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout','Excel']
    function AvailableBooksReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel) {

        $scope.submitted = false;
        $scope.tablediv = false;
        $scope.printd = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.statuscount = false;

        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //-------------Load-data...
        $scope.loaddata = function () {
            debugger;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("AvailableBooksReport/getdetails", pageid).then(function (promise) {

                $scope.deptlist = promise.deptlist;
                $scope.msterliblist1 = promise.msterliblist1;
            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //------------End-Load-data...

        //---------------Get-Report
        $scope.get_report = function () {
            if ($scope.myForm.$valid) {
                var data = {};
                if ($scope.statuscount == true) {
                    var fromdate1 = $scope.IssueFromDate == null ? "" : $filter('date')($scope.IssueFromDate, "yyyy-MM-dd");
                    var todate1 = $scope.IssueToDate == null ? "" : $filter('date')($scope.IssueToDate, "yyyy-MM-dd");
                    var data = {
                        "Type": $scope.Type,
                        "Type2": $scope.Type2,
                        "Fromdate": fromdate1,
                        "ToDate": todate1,
                        "LMD_Id": $scope.lmD_Id,
                        "statuscount": $scope.statuscount,
                        "LMAL_Id": $scope.LMAL_Id,
                    }
                }
                else {
                    var data = {
                        "Type": $scope.Type,
                        "Type2": $scope.Type2,
                        // "Issue_Date": fromdate1,
                        //"IssueToDate": todate1,
                        "LMD_Id": $scope.lmD_Id,
                        "statuscount": $scope.statuscount,
                        "LMAL_Id": $scope.LMAL_Id,
                    }
                }



                apiService.create("AvailableBooksReport/get_report", data).then(function (promise) {
                    if (promise.griddata.length > 0) {

                        $scope.getdata = promise.griddata;
                        angular.forEach($scope.getdata, function (ff) {
                            ff.LMB_EntryDate=ff.LMB_EntryDate == null ? "" : $filter('date')(ff.LMB_EntryDate, "dd/MM/yyyy");
                          
                        })

                        console.log($scope.getdata)

                        $scope.tablediv = true;;
                        $scope.printd = true;

                        $scope.colarrayall = [{

                            title: "SLNO",
                            template: "<span class='row-number'></span>", width: "80px"

                        },
                        {
                            name: 'LMB_EntryDate', field: 'LMB_EntryDate', title: 'Entry Date', width: "130px", template: "#= kendo.toString(LMB_EntryDate, 'dd/MM/yyyy') #"
                            }
                            ,
                        {
                            name: 'LMBANO_AccessionNo', field: 'LMBANO_AccessionNo', title: 'Accession No', width: "150px"
                            },
                            {
                                name: 'LMB_BookTitle', field: 'LMB_BookTitle', title: 'Book Title', width: "130px"
                            },
                            {
                                name: 'LMB_ISBNNo', field: 'LMB_ISBNNo', title: 'ISBN No', width: "100px"
                            },
                            {
                                name: 'LMB_CallNo', field: 'LMB_CallNo', title: 'Call No', width: "100px"
                            },
                            {
                                name: 'LMB_ClassNo', field: 'LMB_ClassNo', title: 'Class No', width: "130px"
                            },
                            {
                                name: 'LMB_NetPrice', field: 'LMB_NetPrice', title: 'Price', width: "130px"
                            },
                            {
                                name: 'LMB_BookType', field: 'LMB_BookType', title: 'Book Type', width: "130px"
                            },
                        {
                            name: 'LMD_DepartmentName', field: 'LMD_DepartmentName', title: 'Department', width: "130px"
                        },
                        {
                            name: 'AuthorName', field: 'AuthorName', title: 'Author', width: "130px"
                        },
                        {
                            name: 'LMP_PublisherName', field: 'LMP_PublisherName', title: 'Publisher', width: "130px"
                        },
                        {
                            name: 'LML_LanguageName', field: 'LML_LanguageName', title: 'Language', width: "130px"
                        },
                        {
                            name: 'LMS_SubjectName', field: 'LMS_SubjectName', title: 'Subject', width: "130px"
                        },
                        {
                            name: 'LMB_PurOrDonated', field: 'LMB_PurOrDonated', title: 'Purchased/Donated', width: "170px"
                        }
                        ];

                        $(document).ready(function () {
                            initGridall();
                        });
                        function initGridall() {
                            $('#gridall').empty();
                           // gridall = $("#gridall").kendoGrid({
                                $("#gridall").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: "AvailableBooksReport.xlsx",
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },
                                pdf: {
                                    //allPages: true,
                                    avoidLinks: true,
                                   //paperSize: "A3",
                                   // margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                                    landscape: true,
                                    repeatHeaders: true,
                                   // template: $("#page-template").html(),
                                  //  scale: 0.8,
                                    fileName: "AvailableBooksReport.pdf",
                                    allPages: true
                                },
                                dataSource: {
                                    //type: "odata",
                                    //transport: {
                                    //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                    //},
                                    data: $scope.getdata,
                                    pageSize: 10,
                                    //aggregate: [
                                    //    { name: 'totalpayable', field: 'totalpayable', aggregate: "sum" },
                                    //    { name: 'FSS_PaidAmount', field: 'FSS_PaidAmount', aggregate: "sum" },
                                    //    { name: 'balance', field: 'balance', aggregate: "sum" },
                                    //    { name: 'concession', field: 'concession', aggregate: "sum" },
                                    //    { name: 'fine', field: 'fine', aggregate: "sum" },
                                    //    { name: 'rebate', field: 'rebate', aggregate: "sum" },
                                    //    { name: 'waived', field: 'waived', aggregate: "sum" }

                                    //]
                                },
                                sortable: true,
                                //pageable: false,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,
                                columns: $scope.colarrayall,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize()
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-number");
                                        $(rowLabel).html(index);
                                    });
                                }

                            }).data("kendoGrid");
                        }


                    }
                    else {
                        swal('Record Not Available!!!');
                        $state.reload();
                    }
                })

            }
            else {
                $scope.submitted = true;
            }

        }
        //-------------End--Get-Report

        $scope.getdata = [];

        //===========print===========//
        $scope.printData = function () {
            var innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookTypeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        }

        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

