
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Master_CertificateController', Master_CertificateController);
    Master_CertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function Master_CertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //=====================================PAGE LOAD
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("TransferCertificate/getdetails", pageid).
                then(function (promise) {
                    $scope.certificate_dropdown = promise.certificate_dropdown;
                    $scope.get_certificate = promise.get_certificate;
                    $scope.presentCountgrid = $scope.get_certificate.length;
                })
        };
        $scope.get_code = function (AMCT_Id) {
            $scope.certificate_code = [];
            angular.forEach($scope.certificate_dropdown, function (qq) {
                if (AMCT_Id == qq.amcT_Id) {
                    $scope.certificate_code.push(qq)
                }
            })
            $scope.AMCT_Certificate_code = $scope.certificate_code[0].amcT_Certificate_code;
            $scope.AMCT_Certificate_Name = $scope.certificate_code[0].amcT_Certificate_Name;
        }

        //=====================================SAVE EDIT DEACTIVE
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "AMCT_Certificate_Name": $scope.AMCT_Certificate_Name,
                    "AMCT_Certificate_code": $scope.AMCT_Certificate_code,
                    "ACERTAPP_ApprovaReqlFlg": $scope.ACERTAPP_ApprovaReqlFlg,
                    "ACERTAPP_OnlineDownloadFlg": $scope.ACERTAPP_OnlineDownloadFlg,
                    "ACERTAPP_Id": $scope.id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("TransferCertificate/savedetails_certificate", data).then(function (promise) {

                    if (promise.returnvalues == 'Add') {
                        swal('Record saved successfully');
                        $state.reload();
                    }
                    else if (promise.returnvalues == 'Update') {
                        swal('Record updated successfully');
                        $state.reload();
                    }
                    else if (promise.returnvalues == 'Error') {
                        swal('Failed to Save/Update, please contact administrator');
                        $state.reload();
                    }
                    else if (promise.returnvalues == 'Duplicate') {
                        swal('Certificate Name already exist. Please others');
                    }



                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //================================== update
        $scope.edit = function (user) {

            var data = {

                "ACERTAPP_Id": user.acertapP_Id,
            }

            apiService.create("TransferCertificate/edit_certificate", data).then(function (promise) {
                $scope.get_certificate_dd = promise.get_certificate_dd;
                $scope.AMCT_Id = promise.get_certificate_dd[0].amcT_Id;
                $scope.amcT_Certificate_Name = promise.get_certificate_dd[0].amcT_Certificate_Name;
           
                $scope.AMCT_Certificate_code = promise.get_details[0].acertapP_CertificateCode;
                $scope.ACERTAPP_ApprovaReqlFlg = promise.get_details[0].acertapP_ApprovaReqlFlg;
                $scope.ACERTAPP_OnlineDownloadFlg = promise.get_details[0].acertapP_OnlineDownloadFlg;
                $scope.id = promise.get_details[0].acertapP_Id;
                //$scope.loaddata();
            })

        };
        //========================
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.AMCT_Id = item.amcT_Id;
            var dystring = "";
            if (item.acertapP_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.acertapP_ActiveFlg == false) {
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
                        apiService.create("TransferCertificate/deactive_certificate", item).
                            then(function (promise) {
                                if (promise.returnvalues == 'true') {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();