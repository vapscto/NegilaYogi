(function () {
    'use strict';
    angular
        .module('app')
        .controller('PurchaseDonateReportController', PurchaseDonateReportController)
    PurchaseDonateReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function PurchaseDonateReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        //by deepak
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        // $scope.obj = {};

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        //End by 
        // $scope.IsHiddenup = true;
        $scope.printdatatable = [];
        $scope.printdatatablegrp = [];
        $scope.printdatatablehad = [];
        $scope.printdatatablecls = [];
        $scope.itemterm = [];
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));


        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        //=====================Loaddata
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            $scope.stf = false;
            $scope.std = false;
            $scope.book_Flag = "BP";
            $scope.gridview = false;
          
           
        }
        //=====================End-----Loaddata----//

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.Clearid = function () {
            debugger;
            $state.reload();
        }
        $scope.filedetails = '';
        $scope.filedetails1 = '';
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {

                    var data = {
                        "LBCPA_Flg": $scope.LMB_BookType,
                        "Circ_Flag": $scope.book_Flag,
                    }

                    apiService.create("PurchaseDonateReport/getdata", data)
                        .then(function (promise) {
                            if (promise.alldata != null && promise.alldata.length > 0) {
                                $scope.alldata = promise.alldata;
                                $scope.result = true;
                                $scope.gridview = true;


                                if ($scope.book_Flag=='Purchased') {
                                    $scope.filedetails ='PurchasedBookReport.xlsx'
                                    $scope.filedetails1 ='PurchasedBookReport.pdf'
                                }
                                if ($scope.book_Flag == 'Donated') {
                                    $scope.filedetails = 'DonatedBookReport.xlsx'
                                    $scope.filedetails1 = 'DonatedBookReport.pdf'
                                }

                                angular.forEach($scope.alldata, function (ff) {
                                    ff.LMB_PurchaseDate = ff.LMB_PurchaseDate == null ? "" : $filter('date')(ff.LMB_PurchaseDate, "dd/MM/yyyy");

                                })

                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"

                                },
                                {
                                    name: 'LMB_BookType', field: 'LMB_BookType', title: 'Book Type', width: "130px", 
                                }
                                    ,
                                {
                                    name: 'LMBANO_AccessionNo', field: 'LMBANO_AccessionNo', title: 'Accession No', width: "150px"
                                },
                                {
                                    name: 'LMB_BookTitle', field: 'LMB_BookTitle', title: 'Book Title', width: "130px"
                                },
                                {
                                    name: 'Name', field: 'Name', title: 'Author Name', width: "130px"
                                },
                                {
                                    name: 'LMV_VendorName', field: 'LMV_VendorName', title: 'Vendor Name', width: "130px"
                                },
                                {
                                    name: 'LMB_BillNo', field: 'LMB_BillNo', title: 'Bill No', width: "130px"
                                },
                                {
                                    name: 'LMB_VoucherNo', field: 'LMB_VoucherNo', title: 'Voucher No', width: "130px"
                                },
                                {
                                    name: 'LMB_PurchaseDate', field: 'LMB_PurchaseDate', title: 'Purchased Date', width: "130px", template: "#= kendo.toString(LMB_PurchaseDate, 'dd/MM/yyyy') #"
                                },
                                {
                                    name: 'LMB_NoOfCopies', field: 'LMB_NoOfCopies', title: 'No. Of Copies', width: "130px"
                                },
                                {
                                    name: 'LMB_Price', field: 'LMB_Price', title: 'Price', width: "130px"
                                },
                                {
                                    name: 'LMB_Discount', field: 'LMB_Discount', title: 'Discount', width: "130px"
                                    },
                                    {
                                        name: 'LMB_NetPrice', field: 'LMB_NetPrice', title: 'Net Amount', width: "130px"
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
                                    
                                    $("#gridall").kendoGrid({
                                        toolbar: ["excel"],
                                        excel: {
                                            fileName: $scope.filedetails,
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            //allPages: true,
                                            avoidLinks: true,
                                
                                            landscape: true,
                                            repeatHeaders: true,
                                           
                                            fileName: $scope.filedetails1,
                                            allPages: true
                                        },
                                        dataSource: {
                                            
                                            data: $scope.alldata,
                                            pageSize: 20,
                                           
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
                                swal("No Records Found");
                                $scope.gridview = false;
                        }

                    })

            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.exportToExcel = function () {
            debugger;
         
            if ($scope.alldata !== null && $scope.alldata.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionIdgrp1, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("No records Found to Export");
                }
        }
        
        $scope.printData = function () {
           
            if ($scope.alldata !== null && $scope.alldata.length > 0) {
                    var innerContents = document.getElementById("printSectionIdgrp1").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                  
                }
                else {
                    swal("Please Select Records to be Printed");
                }
        }
        
    }
})();

