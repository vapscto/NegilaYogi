(function () {
    'use strict';
    angular
.module('app')
.controller('FeeRefundController', FeeRefundController)

    FeeRefundController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','Excel','$timeout']
    function FeeRefundController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
       

        $scope.search = "";
        $scope.loaddata = function () {
            $scope.allorindorcos = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            $scope.report = false;
            var pageid = 2;
           
            apiService.getURI("FeeRefund/getalldetails", pageid).
        then(function (promise) {
          
            $scope.acayyearbind = promise.academicyr;
            $scope.headlist = promise.fillfeehead;
            $scope.classlist = promise.fillclass;
          
            $scope.onselectclass = function () {
                
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.fmG_Class,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                
                apiService.create("FeeRefund/getsection", data).
                   then(function (promise) {
                       $scope.sectioncount = promise.fillsec;
                   })
            }


            $scope.get_total_student_print = function () {
                var totA_p = 0;
                var totB_p = 0;
                angular.forEach($scope.printdatatable, function (gp) {
                    totA_p += gp.paid;
                    totB_p += gp.FR_RefundAmount;
                })
                $scope.totA_p = totA_p;
                $scope.totB_p = totB_p;
            }

            //$scope.onselectyear = function () {

            //    var data = {
            //        "ASMAY_Id": $scope.asmaY_Id,
            //        // "ASMCL_Id": clsobj.asmcL_Id,s
            //    }
            //    apiService.create("FeeRefund/gethead", data).
            //       then(function (promise) {
            //           $scope.headlist = promise.fillfeehead;
            //       })
            //}

            $scope.allorconsorall = function () {
               
                if($scope.allorindiorcon=="all")
                {
                    $scope.report = false;
                    $scope.asmaY_Id = "";
                    $scope.fmG_Class = 0;
                    $scope.asmC_Id = 0;
                    $scope.Amst_Id = "";
                    $scope.fromDate = "";
                    $scope.toDate = "";
                    $scope.fmh_id = "";
                    $scope.showbutton = false;
                   
                }
                else if($scope.allorindiorcon=="indi")
                {
                    $scope.report = false;
                    $scope.asmaY_Id = "";
                    $scope.fmG_Class = "";
                    $scope.asmC_Id = "";
                    $scope.fromDate = "";
                    $scope.toDate = "";
                    $scope.fmh_id = "";
                    $scope.showbutton = false;
                }
                $scope.refund = [];
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
               
            }


            $scope.onselectyear = function () {

                var data = {
                    "asmyid": $scope.asmaY_Id,
                    //"ASMCL_Id": $scope.fmG_Class,
                    //"AMSC_Id": $scope.asmC_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                
                apiService.create("FeeRefund/getstudent", data).
                   then(function (promise) {
                       $scope.headlist = promise.fillfeehead;

                       //  $scope.arrlstinst1 = promise.fillinstallment;

                   })
            }
        })
        }
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.Clearid = function () {

            $scope.report = false;
            $scope.asmaY_Id = "";
            $scope.fmG_Class = "";
            $scope.asmC_Id = "";
            $scope.Amst_Id = "";
            $scope.fromDate = "";
            $scope.toDate = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.loaddata();

        }

      
        $scope.submitted = false;
        $scope.ShowReport = function (asmaY_Id, fmG_Class, asmC_Id, Amst_Id, fromDate, toDate, FMH_Id) {
            
            if ($scope.myForm.$valid) {
                $scope.from_date = new Date($scope.fromDate).toDateString();
                $scope.to_date = new Date($scope.toDate).toDateString();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.fmG_Class,
                    "AMSC_Id": $scope.asmC_Id,
                    "Fromdate": $scope.from_date,
                    "Todate": $scope.to_date,
                    "fmh_id": $scope.fmh_id,
                    "regornamedetails": $scope.allorindiorcon
                }
                apiService.create("FeeRefund/radiobtndata", data).
                    then(function (promise) {

                        if (promise.refunddata != null && promise.refunddata != "") {
                            $scope.refund = promise.refunddata;
                            $scope.totcountfirst = promise.refunddata.length;
                            $scope.report = true;
                            $scope.showbutton = true;
                        }
                        else {
                             $scope.refund = {};
                            swal("No Record Found");
                        }
                })

            }

            else {
                $scope.submitted = true;

            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }




        $scope.printdatatable = [];
        $scope.exportToExcel = function (table1) {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(table1, 'WireWorkbenchDataExport');

                $timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        }

        $scope.optionToggledstd = function () {
            
             $scope.printdatatable = [];
           // var toggleStatus = $scope.ind;
            angular.forEach($scope.refund, function (itm) {
               // itm.stdselected = toggleStatus;
                if (itm.stdselected == true) {
                    $scope.printdatatable.push(itm);
                }
                //else {
                //    $scope.printdatatable.splice(itm);
                //}
            });
            $scope.get_total_student_print();

        }


        $scope.toggleAllstd = function () {
            
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.refund, function (itm) {
                itm.stdselected = toggleStatus;
                if (itm.stdselected == true) {
                    $scope.printdatatable.push(itm);
                }
                //else {
                //    $scope.printdatatable.splice(itm);
                //}
            });
            $scope.get_total_student_print();

        }


        $scope.optionToggind = function (SelectedStudentRecord, index) {
            
            $scope.ind = $scope.refund.every(function (itm)
            { return itm.selected; });
            $scope.printdatatable = [];
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }


        $scope.printData = function (printSectionId) {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                 '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
               // $state.reload();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

    }
})();
