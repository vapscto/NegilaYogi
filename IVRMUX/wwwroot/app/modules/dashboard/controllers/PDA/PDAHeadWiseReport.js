(function () {
    'use strict';
    angular
.module('app')
.controller('PDAHeadWiseReportController', PDAHeadWiseReportController)
    PDAHeadWiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
 
    function PDAHeadWiseReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {
   
        $scope.printdatatable = [];

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        $scope.loaddata = function () {
            $scope.currentPage = 1;
         //   $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("PDAHeadWiseReport/getalldetails", pageid).
        then(function (promise) {
            $scope.acdyr = promise.fillyear;
            $scope.classcount = promise.classlist;
            $scope.sectioncount = promise.searcharray;
            $scope.pdaheadlst = promise.pdadata;

            //angular.forEach($scope.pdaheadlst, function (role) {
            //    $scope.albumNameArraygroupids.push({ columnID: role.ftI_Id, columnName: role.ftI_Name });
            //    $scope.header_list = [];
            //    angular.forEach($scope.pdaheadlst, function (role) {
            //         $scope.header_list.push(role);
            //    })
            //})


        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.submitted = false;
        $scope.ShowReport = function () {
            if ($scope.myForm.$valid) {

                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "PDAMH_ID": $scope.PDAMH_ID,
                        "From_Date": new Date($scope.fromDate).toDateString(),
                        "To_Date": new Date($scope.todate).toDateString(),
                    }
                
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                
                apiService.create("PDAHeadWiseReport/radiobtndata", data).
               then(function (promise) {
                   if (promise.headwise != null && promise.headwise != "") {
                       $scope.reportdetails = promise.headwise;
                       $scope.totcountfirst = promise.headwise.length;
                           $scope.Grid_view = true;
                           $scope.print_flag = true;
                          
                       }
                       else {
                           swal("No Record Found");
                           $scope.Grid_view = false;
                           $scope.print_flag = false;
                       }
                   
               })
            }
            else {
                $scope.submitted = true;

            }
        };


        $scope.Cancel = function () {
            $state.reload();
        }



        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all = $scope.reportdetails.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            $scope.get_total_student_print();
        }


        $scope.exportToExcel = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(table1, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        }

        $scope.printData = function () {
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
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }


        $scope.get_total_student_print = function () {
            var totA_p = 0;
            angular.forEach($scope.printdatatable, function (gp) {
                totA_p += gp.PDAE_TotAmount;
            })
            $scope.totA_p = totA_p;
          
        }


    }
})();
