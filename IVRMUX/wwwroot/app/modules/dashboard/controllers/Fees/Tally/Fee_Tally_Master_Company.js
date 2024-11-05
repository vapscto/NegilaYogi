(function () {
    'use strict';
    angular
        .module('app')
        .controller('Fee_Tally_Master_Company_ReportController', Fee_Tally_Master_Company_ReportController)
    Fee_Tally_Master_Company_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function Fee_Tally_Master_Company_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;      
        $scope.formload = function () {
            var data = {
                "Id": 2
            }
            apiService.create("Fee_Tally_Master_Company/getalldetails", data).
                then(function (promise) {
                    $scope.Instititions = promise.instititions;
                    $scope.getarray = promise.getarray;
                })           
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.SaveData = function () {           
            $scope.submitted = true;
            var FTMCOM_Id = 0;
            if ($scope.FTMCOM_Id != null && $scope.FTMCOM_Id > 0) {
                FTMCOM_Id = $scope.FTMCOM_Id;
            }
            if ($scope.myform.$valid) { 
                var data = {
                    "FTMCOM_CompanyName": $scope.FTMCOM_CompanyName,
                    "FTMCOM_CompanyCode": $scope.FTMCOM_CompanyCode,                   
                    "FTMCOM_Id": FTMCOM_Id

                }
                apiService.create("Fee_Tally_Master_Company/savedata", data).
                    then(function (promise) {

                        if (promise.return_val == "Update") {
                            swal("Record Update Successfully !");
                        }
                        if (promise.return_val == "Notupdate") {
                            swal("Record Not Update !");
                        }
                        if (promise.return_val == "save") {
                            swal("Record Saved Successfully !");
                        }
                        if (promise.return_val == "Notsave") {
                            swal("Record Not Saved !");
                        }
                        if (promise.return_val == "RecordExist") {
                            swal("Record Already Exist !");
                        }
                        if (promise.return_val === "") {
                            swal("Please Contact Administrator !");
                        }
                        $state.reload();
                    })

            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.edit = function (user) {
            $scope.FTMCOM_Id = user.ftmcoM_Id;
            $scope.FTMCOM_CompanyName = user.ftmcoM_CompanyName;
            $scope.FTMCOM_CompanyCode = user.ftmcoM_CompanyCode;
                      
        };
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.FTMCOM_Id = item.ftmcoM_Id;
            var data = {
                "FTMCOM_Id": item.ftmcoM_Id,

            }
            var dystring = "";
            if (item.ftmcoM_ActiveId == true) {
                dystring = "Deactivate";
            }
            else if (item.ftmcoM_ActiveId == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Fee_Tally_Master_Company/deletedata", data).
                            then(function (promise) {
                                if (promise.return_val == "Delete") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                if (promise.return_val == "NotDelete") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                               //NotDelete
                               
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }      
        $scope.cleardata = function () {
            $state.reload();
        }    
    }

})();



