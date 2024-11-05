
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PI_ToSupplierController', INV_PI_ToSupplierController);
    INV_PI_ToSupplierController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_PI_ToSupplierController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.totsel = 0;
        $scope.newdata = false;
        var date = new Date();
        $scope.invmpI_Doc_Date = date;
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


        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.itemsPerPage2 = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_PI_ToSupplier/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_piNo = promise.get_piNo;
                    $scope.get_supplier = promise.get_supplier;
                })
        };

        //===================================== PI Change
        $scope.onpichange = function (itemid) {
            $scope.get_indentDetail = "";
            var item_id = itemid.invmpI_Id;
            var data = {
                "INVMPI_Id": item_id
            }
            apiService.create("INV_PI_ToSupplier/getpiDetail", data).
                then(function (promise) {
                    $scope.get_pidetails = promise.get_pidetails;
                    var prQty = 0;
                    angular.forEach($scope.get_pidetails, function (aq) {
                        prQty += parseFloat(aq.invtpI_PRQty);
                    })
                    if (prQty == "0") {
                        $scope.pirQty = false;
                    }
                    else {
                        $scope.pirQty = true;
                    }


                    angular.forEach($scope.get_piNo, function (dd) {
                        if (dd.invmpI_Id == item_id) {
                            $scope.pname = dd.invmpI_PINo;
                        }

                    })
                })
        }
        //===================================== Radio Chang e
        $scope.onradiochange = function () {
            $scope.get_supplier = "";
            $scope.invpitS_SupplierName = "";
            $scope.invpitS_ContactNo = "";
            $scope.invpitS_EmailId = "";
            $scope.loaddata();
        }
        //===================================== Supplier Grid Select All
        $scope.toggleAll = function () {
            var selck1 = 0;
            angular.forEach($scope.get_supplier, function (subj) {
                subj.xyz = $scope.all;
                selck1 += 1;
            })
            $scope.totsel = selck1;
        };

        $scope.optionToggled = function () {
            var selck = 0;
            $scope.all = $scope.get_supplier.every(function (itm) {
                return itm.xyz;
            });

            angular.forEach($scope.get_supplier, function (sup) {
                if (sup.xyz) {
                    selck += 1;
                }
            })
            $scope.totsel = selck;
        };
        //===================================== SAVE DATA
        $scope.pname = "";
        $scope.savedata = function () {
            $scope.submitted = true;
           
            if ($scope.myForm.$valid) {

                var Template = document.getElementById('abcd').innerHTML;
               
                debugger;
                $scope.supplierArray = [];
                if ($scope.supplierflag=='S') {
                    angular.forEach($scope.get_supplier, function (sp) {
                        if (sp.xyz) {
                            $scope.supplierArray.push({ invmS_Id: sp.invmS_Id, invpitS_SupplierName: sp.invmS_SupplierName, invpitS_ContactNo: sp.invmS_SupplierConatctNo, invpitS_EmailId: sp.invmS_EmailId });
                        }
                    })
                }
                else {
                    $scope.supplierArray.push({ invmS_Id: 1, invpitS_SupplierName: $scope.invpitS_SupplierName, invpitS_ContactNo: $scope.invpitS_ContactNo, invpitS_EmailId: $scope.invpitS_EmailId });
                }

                


                var data = {
                    "INVMPI_Id": $scope.obj.invmpI_Id.invmpI_Id,
                    "supplierArray": $scope.supplierArray,
                    "INVPITS_Id": $scope.invpitS_Id,
                    "email": $scope.email,
                    "sms": $scope.sms,
                    "supplierflag": $scope.supplierflag,
                    "atchtempl": Template,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_PI_ToSupplier/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invpitS_Id == 0 || promise.invpitS_Id < 0) {
                            swal('Message Sent Successfully');
                        }
                        else if (promise.message == "SMS") {
                            swal("SMS Send Successfully..!!");
                        }
                        else if (promise.message == "Email") {
                            swal("Email Send Successfully..!!");
                        }
                    }
                    else {
                        if (promise.invpitS_Id == 0 || promise.invpitS_Id < 0) {
                            swal('Failed to Send, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }





        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();