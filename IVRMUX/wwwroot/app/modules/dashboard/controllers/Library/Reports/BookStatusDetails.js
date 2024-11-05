
(function () {
    'use strict';
    angular
        .module('app')
        .controller('BookStatusDetailsController', BookStatusDetailsController)

    BookStatusDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$q', '$filter']
    function BookStatusDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $q, $filter) {

        $scope.submitted = false;
        $scope.tablediv = false;
        $scope.printd = false;
        $scope.export = false;
        $scope.LMB_Id = "";
        //-------------Load-data...
        $scope.loaddata = function () {
            debugger;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 15;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("BookStatusDetails/getdetails", pageid).then(function (promise) {

                $scope.booktitle = promise.booklist;
               
            })

           
        }
          //------------End-Load-data...
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.changetable = function () {
            $scope.printbutton = false;
            $scope.reportbutton = true;
            $scope.tablegrd = false;
        }


        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '3') {

                var data = {
                    "searchfilter": objj.search,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("BookStatusDetails/searchfilter", data).
                    then(function (promise) {

                        $scope.booktitle = promise.booklist;
                    })
            }

        }

        $scope.totalcnt = 0;
        $scope.issedcnt = 0;
        $scope.availcnt = 0;
        $scope.newarray = [];
        $scope.get_bookdetails = function () {
            $scope.tablediv = false;
            $scope.printd = false;
            $scope.export = false;
            $scope.totalcnt = 0;
            $scope.issedcnt = 0;
            $scope.availcnt = 0;
            $scope.newarray = [];
            $scope.tablediv = false;
            debugger;
            var studid = $scope.LMB_Id.lmB_Id;
            
            var data = {
                "LMB_Id": studid,
            }
            apiService.create("BookStatusDetails/get_bookdetails", data).then(function (promise) {

                $scope.bookdetails = promise.bookdetails;

                if ($scope.bookdetails.length != 0 && $scope.bookdetails != null) {

                    $scope.LMB_EntryDate = new Date(promise.bookdetails[0].lmB_EntryDate);
                    $scope.LMB_BookTitle = promise.bookdetails[0].lmB_BookTitle;
                    $scope.LMB_VolNo = promise.bookdetails[0].lmB_VolNo;
                    $scope.LMB_Price = promise.bookdetails[0].lmB_Price;
                    $scope.LMB_ClassNo = promise.bookdetails[0].lmB_ClassNo;
                    $scope.authorname = promise.bookdetails[0].authorname;
                    $scope.callno = promise.bookdetails[0].LMB_CallNo;
                    $scope.cater = promise.bookdetails[0].LMC_CategoryName;
                    $scope.publ = promise.bookdetails[0].publishername;


                    $scope.totalcnt = $scope.bookdetails.length;
                    $scope.tablediv = true;
                    $scope.printd = true;
                    $scope.export = true;

                    console.log($scope.bookdetails);
                    $scope.newarray = [];
                   
                    angular.forEach($scope.bookdetails, function (rr) {
                        var cnt = 0;
                        var status = "";
                        var name = "";
                        var type = "";
                        if (rr.LBTRS_Id == null && rr.LBTRD_Id == null && (rr.LBTR_GuestName == null || rr.LBTR_GuestName == '') && rr.LBTRST_Id == null) {
                            status = rr.LMBANO_AvialableStatus;
                            name = '--';
                            type = '--';
                        }
                        else {
                            status = "Issued";
                           
                            if (rr.LBTRS_Id != null && rr.LBTRS_Id != undefined && rr.LBTRS_Id != '') {
                                $scope.issedcnt += 1;
                                name = rr.StuName;
                                type = 'STUDENT';
                            }
                            else if (rr.LBTRD_Id != null && rr.LBTRD_Id != undefined && rr.LBTRD_Id != '') {
                                $scope.issedcnt += 1;
                                name = rr.DeptName;
                                type = 'DEPARTMENT';
                            }
                            else if (rr.LBTRST_Id != null && rr.LBTRST_Id != undefined && rr.LBTRST_Id != '') {
                                $scope.issedcnt += 1;
                                name = rr.EmpName;
                                type = 'STAFF';
                            }
                            else if (rr.LBTR_GuestName != null && rr.LBTR_GuestName != undefined && rr.LBTR_GuestName != '') {
                                $scope.issedcnt += 1;
                                name = rr.LBTR_GuestName;
                                type = 'GUEST';
                            }
                        }

                        $scope.newarray.push({ lmbanO_Id: rr.lmbanO_Id, LMBANO_AccessionNo: rr.LMBANO_AccessionNo, status: status, name: name, type: type, Idate: rr.LBTR_IssuedDate, Ddate: rr.LBTR_DueDate, LBTR_Renewalcounter: rr.LBTR_Renewalcounter});


                    })

                }
                else {
                    
                    swal("Data Not Available!!");
                    $state.reload();
                }

            })
        }

          //-------------Get-Report...
        $scope.get_report = function () {
            debugger;
            if ($scope.myForm.$valid) {

                var data = {
                    //"LMRA_FloorName": $scope.lmrA_FloorName,
                    "LMRA_RackName": $scope.lmrA_RackName,
                    "LMAL_Id": $scope.LMAL_Id,
                }
                apiService.create("RackReport/get_report", data).then(function (promise) {
                    if (promise.reportlist.length > 0) {

                        $scope.reportlist = promise.reportlist;
                        $scope.tablediv = true;
                        $scope.printd = true;
                        $scope.export = true;
                    }
                    else {
                        $scope.printd = false;
                        $scope.export = false;
                        $scope.tablediv = false;
                        swal('Record Not Available!!!');
                        //$state.reload();

                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
          //------------End-Get-Report...


        //===========print===========//
        $scope.printData = function () {
            if ($scope.filterValue !== null && $scope.filterValue.length > 0) {
                var innerContents = document.getElementById("printtable").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/RackReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }

        //====================================================Excel
        $scope.exportToExcel = function (table) {
            debugger;
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //=========clear-field
        $scope.Clearid = function () {
          
            $state.reload();
        }

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


      








    }
})();

