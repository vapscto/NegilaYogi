

(function () {
    'use strict';
    angular
        .module('app')
        .controller('BookArrivalReportController', BookArrivalReportController)

    BookArrivalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', 'Excel', '$timeout']
    function BookArrivalReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, Excel, $timeout) {

        $scope.submitted = false;
        $scope.tablediv = false;;
        $scope.printd = false;
        $scope.searchvalue = '';

        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;


        //---------------Load --data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 15;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("BookArrivalReport/getdetails", pageid).then(function (promise) {

                /*$scope.donorlist = promise.donorlist;*/

                $scope.deptlist = promise.deptlist;

                $scope.lib_list = promise.lib_list;
            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //------------End---Load --data




        //---------------Get Repoet....
        $scope.get_report = function () {
            debugger;

            var fromdate = $scope.Fromdate == null ? "" : $filter('date')($scope.Fromdate, "yyyy-MM-dd");
            var todate = $scope.ToDate == null ? "" : $filter('date')($scope.ToDate, "yyyy-MM-dd");

            //if ($scope.lmD_Id=="0") {
            //    $scope.lmD_Id = "ALL"
            //}

            if ($scope.myForm.$valid) {

                var data = {
                    "Type": $scope.Type,
                    "Type2": $scope.Type2,
                    "LMD_Id": $scope.lmD_Id,
                    "Fromdate": fromdate,
                    "ToDate": todate,
                    "LMAL_Id": $scope.LMAL_Id,
                }
                apiService.create("BookArrivalReport/get_report", data).then(function (promise) {

                    if (promise.reportlist.length > 0) {
                        $scope.reportlist = promise.reportlist;

                        $scope.tablediv = true;
                        $scope.printd = true;

                        angular.forEach($scope.reportlist, function (ff) {
                            ff.LMB_EntryDate = ff.LMB_EntryDate == null ? "" : $filter('date')(ff.LMB_EntryDate, "dd/MM/yyyy");
                        })
                        angular.forEach($scope.reportlist, function (ff) {
                            ff.LMB_PurchaseDate = ff.LMB_PurchaseDate == null ? "" : $filter('date')(ff.LMB_PurchaseDate, "dd/MM/yyyy");
                        })

                        console.log($scope.reportlist);

                        $scope.colarrayall = [{

                            title: "SLNO",
                            template: "<span class='row-number'></span>", width: "80px"

                        },

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
                                name: 'LMB_ClassNo', field: 'LMB_ClassNo', title: 'Class No', width: "100px"
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
                            name: 'LMB_PurOrDonated', field: 'LMB_PurOrDonated', title: 'Purchased/Donated', width: "170px"
                        },
                        {
                            name: 'LMB_EntryDate', field: 'LMB_EntryDate', title: 'Entry Date', width: "130px", template: "#= kendo.toString(LMB_EntryDate, 'dd/MM/yyyy') #"
                        },
                        {
                            name: 'LMB_PurchaseDate', field: 'LMB_PurchaseDate', title: 'Purchase Date', width: "130px", template: "#= kendo.toString(LMB_PurchaseDate, 'dd/MM/yyyy') #"
                        },

                        ];

                        $(document).ready(function () {

                            initGridall();
                        });

                        function initGridall() {
                            $('#gridall').empty();
                       $("#gridall").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: "ArrivalBooksReport.xlsx",
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
                                    fileName: "ArrivalBooksReport.pdf",
                                    allPages: true
                                },
                                dataSource: {
                                    //type: "odata",
                                    //transport: {
                                    //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                    //},
                                    data: $scope.reportlist,
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
                        swal("Record Not Found!");
                        $scope.tablediv = false;
                        $scope.printd = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        //-------------End---Get Repoet....

        $scope.reportlist = [];


        //===========print===========//
        $scope.printdata = function () {
            if ($scope.filterValue !== null && $scope.filterValue.length > 0) {
                var innerContents = document.getElementById("printtable").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookArrivalReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }
        //==============End==============//


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //===========----Clear Field
        $scope.Clearid = function () {

            $state.reload();
        }

        $scope.ExportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }


    }
})();

