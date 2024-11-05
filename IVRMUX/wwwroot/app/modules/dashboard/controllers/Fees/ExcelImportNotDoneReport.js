

(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExcelImportNotDoneReportController', ExcelImportNotDoneReportController)

    ExcelImportNotDoneReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExcelImportNotDoneReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.IsHiddenup = true;
        $scope.print_flag = false;
        $scope.route = false;
        $scope.bus = true;
        var fromdate = "";
        $scope.groupwiseshow = false;
        $scope.headwiseshow = false;
        $scope.termwiseshow = false;
        $scope.classwiseshow = false;
        $scope.classcategoryshow = false;
        $scope.grpwisedata = false;
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        $scope.fromdate = new Date();
        $scope.todate = new Date();

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }



        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            $scope.bus = true;
            var pageid = 1;
            var data = {
                "reporttype": grouporterm,
            }

            apiService.create("ExcelImportNotDoneReport/getalldetails123", data).
                then(function (promise) {

                    $scope.arrlist6 = promise.acayear;


                    $scope.asmaY_Id = academicyrlst[0].asmaY_Id;

                   


                })
        }

        $scope.DeletRecord = function (employee) {
            $scope.FEIPST_Id = employee.FEIPST_Id;
            var orgid = $scope.FEIPST_Id;

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("ExcelImportNotDoneReport/Deletedetails", orgid).
                            then(function (promise) {

                                
                               
                                if (promise.returnval == true) {

                                    $scope.masterse = promise.masterSectionData;

                                    swal('Record Deleted Successfully');
                                    $scope.showreport();
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }
                                $scope.formload();
                            })
                        $scope.formload();
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $scope.formload();
                    }
                });


            //})
        }

     
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

       
      
       


    
      
        $scope.submitted = false;

        $scope.showreport = function () {

            if ($scope.myForm.$valid) {
              



                var data = {
                    "asmay_id": $scope.asmaY_Id,

                    "fromdate": new Date($scope.fromdate).toDateString(),
                    "todate": new Date($scope.todate).toDateString(),
                }


                apiService.create("ExcelImportNotDoneReport/getreport", data).then(function (promise) {


                    $scope.headwisereport = [];

                    if (promise.getreportdata != null && promise.getreportdata.length != null) {
                        $scope.grpwisedata = true;
                       
                        $scope.headwisereport = promise.getreportdata;
                            $scope.headwiseshow = true;
                       
                    }
                    else {
                        swal("Records Not Found")
                        $scope.grpwisedata = false;
                        $scope.headwiseshow = false;
                    } 






                })
            }
            else {
                $scope.submitted = true;

            }
        };

      

       


       
        $scope.Clearid = function () {

            $state.reload();
            $scope.loaddata();
        }



      



        $scope.printToCart = function () {

           

            var innerContents = document.getElementById("printSectionId").innerHTML;

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/BArrearReportPdf.css"/>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }





    }
})();

