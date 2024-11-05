
(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeReceiptImportStthomasController', FeeReceiptImportStthomasController)

    FeeReceiptImportStthomasController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache', '$filter']
    function FeeReceiptImportStthomasController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache, $filter) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.submitted = false;
        $scope.reverse = false;

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.receipt, function (itm) {
                itm.selected = toggleStatus;               
            });            
        }

        $scope.optionToggled = function () {
            $scope.all2 = $scope.receipt.every(function (itm) { return itm.selected; });           
        }
            
        $scope.loaddata = function () {            
            var pageid = 2;
            apiService.getURI("FeeReceiptImportStthomas/getdetails",
                pageid).then(function (promise) {

                    $scope.yearDropdown = promise.academicyearlist; 

                    $scope.asmaY_Id = academicyrlst[0].asmaY_Id;                                    
            })
        }

        $scope.submitted = false;
        $scope.showReport = function () {
            $scope.searchValue = "";

            if ($scope.myForm.$valid) {
                var fromdate = new Date($scope.fromdate).toDateString();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "fypdate": fromdate,
                };
                apiService.create("FeeReceiptImportStthomas/getreportdetails", data)
                    .then(function (promise) {                   
                        
                        if (promise.receiptdelete !== null && promise.receiptdelete.length > 0) {                            
                            $scope.receipt = promise.receiptdelete;
                            $scope.presentCountgrid = $scope.receipt.length;
                        }
                        else {
                            swal("No Records Found!");                                                     
                        }
                    });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $scope.asmaY_Id = "";
            $scope.fromdate = "";
            $state.reload();
        };


        $scope.deleterec = function () {
            
            $scope.receipttarray = [];
            angular.forEach($scope.receipt, function (aa) {

                $scope.receipttarray.push({ ASMAY_Id: aa.ASMAY_ID, AMST_Id: aa.AMST_Id, FYP_Id: aa.FYP_Id})
            });         
           
            var data = {
                "receipt": $scope.receipttarray
            }
            apiService.create("FeeReceiptImportStthomas/Deletereceipt", data).
                then(function (promise) {

                    if (promise.updatereceipt != null || promise.updatereceipt.length > 0) {
                        $scope.receiptlist = promise.updatereceipt;
                    }
                    else {
                        swal("Records Deleted Successfully!!");
                    }
                    $state.reload();
            });
        }
    }
})();
