

(function () {    'use strict';
    angular
        .module('app')
        .controller('INV_MasterProductStagesController', INV_MasterProductStagesController);
    INV_MasterProductStagesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_MasterProductStagesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_MasterProductStages/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.gridproductTax;
                    $scope.get_product = promise.get_product;
                    $scope.get_item = promise.get_item;
                    $scope.get_productlist = promise.get_productlist;
                    $scope.presentCountgrid = $scope.get_productlist.length;

                    $scope.get_productItemlist = promise.get_productItemlist;
                    $scope.get_store_product = promise.get_store_product;
                });
        };

        ////-----------Tax Grid Select All
        $scope.toggleAll = function () {
            angular.forEach($scope.get_productTax, function (subj) {
                subj.xyz = $scope.all;
            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.get_productTax.every(function (itm) { return itm.xyz; });
        };

        //---------------------------------Save--------------------------------------------
        //Tax
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                var data = {
                    "INVMPS_Id": $scope.invmpS_Id,
                        "INVMP_Id": $scope.invmP_Id,
                        "INVMPS_Stages": $scope.invmpS_ProductStage,
                    };
                
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_MasterProductStages/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmpS_Id === 0 || promise.invmpS_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmpS_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmpS_Id === 0 || promise.invmpS_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmpS_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
            $state.reload();
        };



        $scope.savedata2 = function () {
            $scope.submitted = true;
            if ($scope.myForm2.$valid) {

                var data = {
                    "DCSSP_Id": $scope.dcssP_Id,
                    "INVMST_Id": $scope.invmsT_Id,
                    "INVMP_Id": $scope.invmP_Id,
                  
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_MasterProductStages/savestoreproduct", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmpS_Id === 0 || promise.invmpS_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmpS_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmpS_Id === 0 || promise.invmpS_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmpS_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
            $state.reload();
        };







        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.savedata1 = function () {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {

                $scope.taxchk = [];
                angular.forEach($scope.get_productTax, function (tax) {
                    if (tax.xyz) {
                        $scope.taxchk.push({ invmpS_Id: tax.invmpS_Id, invmpsS_Id: tax.invmpsS_Id, invmpsS_Status: tax.invmpsS_Status });
                    }
                })


                var data = {
                    "INVMP_Id": $scope.invmP_Id,
                    "INVMPI_ItemQty": $scope.invmpsS_Status,
                    "product_stage": $scope.taxchk
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_MasterProductStages/savedetailQty", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmpI_Id == 0 || promise.invmpI_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmpI_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmpI_Id == 0 || promise.invmpI_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmpI_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.clear();
                    $scope.loaddata();
                })            
            }
            else {
                $scope.submitted1 = true;
            }
        };


        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.clear = function () {
            $scope.invmP_Id = "";
            $scope.taxchk = [];

           // $state.reload();
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMPS_Id = item.invmpS_Id;
            var dystring = "";
            if (item.invmpS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmpS_ActiveFlg == false) {
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
                        apiService.create("INV_MasterProductStages/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                    $state.reload();
                });
        }

        //$scope.deactiveptax = function (item, SweetAlert) {
        //    $scope.INVMPT_Id = item.invmpT_Id;
        //    var dystring = "";
        //    if (item.invmpT_ActiveFlg == true) {
        //        dystring = "Deactivate";
        //    }
        //    else if (item.invmpT_ActiveFlg == false) {
        //        dystring = "Activate";
        //    }
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do You Want To " + dystring + " Record?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //        function (isConfirm) {
        //            if (isConfirm) {
        //                apiService.create("INV_MasterProductStages/deactiveptax", item).
        //                    then(function (promise) {
        //                        if (promise.returnval == true) {
        //                            swal("Record " + dystring + "d Successfully!!!");
        //                        }
        //                        else {
        //                            swal("Record Not " + dystring + "d Successfully!!!");
        //                        }
                              
        //                        angular.element('#myModal').modal('hide');
        //                        $scope.clear();
        //                        $scope.loaddata();
                                
        //                    })
        //            }
        //            else {
        //                swal("Record " + dystring + " Cancelled!!!");
        //            }
        //        });
        //}

        $scope.edit = function (item) {
            $scope.get_tax = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.invmP_Id = item.invmP_Id;
            $scope.invmpSS_Id = item.invmpSS_Id;
            $scope.invmpS_Id = item.invmpS_Id;
            var data = {
                "INVMP_Id": item.invmP_Id,
                "INVMPS_Id": item.invmps_Id,
                "INVMPSS_Id": item.invmpsS_Id,
            };
            apiService.create("INV_MasterProductStages/productTax", data).
                then(function (promise) {
                    
                    $scope.get_productTax = promise.get_productTax;
                    angular.forEach($scope.get_productTax, function (atax) {
                        angular.forEach($scope.get_productTax, function (itax) {
                            if (atax.invmpsS_Id == itax.invmpsS_Id) {
                                atax.invmpsS_Status = itax.invmpsS_Status;
                               // atax.invmpT_Id = itax.invmpT_Id;
                                atax.xyz = true;
                            }

                        });

                    });

                    
                });

        };


        $scope.getstages = function () {
            var data = {
                "INVMP_Id": $scope.invmP_Id
            };
            apiService.create("INV_MasterProductStages/getstages", data).
                then(function (promise) {

                    //$scope.get_tax = promise.get_tax;

                    $scope.get_productTax = promise.get_product;
                });

        };



        $scope.deactiveQty = function (item, SweetAlert) {
            $scope.INVMPSS_Id = item.invmpsS_Id;
            var dystring = "";
            if (item.invmpsS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmpsS_ActiveFlg == false) {
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
                        apiService.create("INV_MasterProductStages/deactiveQty", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                            
                                $scope.clear();
                                $scope.loaddata();
                               
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.editQty = function (item) {
            $scope.invmpS_Id = item.invmpS_Id;
            $scope.invmP_Id = item.invmP_Id;
            $scope.invmpS_ProductStage = item.invmpS_Stages;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };


        //$scope.onformclick = function (id) {
        //    var data = {
        //        "INVMP_Id": id
        //    };
        //    apiService.create("INV_MasterProductStages/productTax", data).
        //        then(function (promise) {
        //            $scope.gridproductTax = promise.gridproductTax;
                    
        //            $scope.product = $scope.gridproductTax[0].invmP_ProductName;
        //        });
        //};

        //$scope.sortBy = function (propertyName) {
        //    $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        //    $scope.propertyName = propertyName;
        //};
        //$scope.searchValue = '';
        //$scope.searchValue1 = '';

       

    }
})();