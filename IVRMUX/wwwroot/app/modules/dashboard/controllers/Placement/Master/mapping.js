(function () {
    'use strict';
    angular
        .module('app')
        .controller('mappingController', mappingController)
    mappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function mappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.obj = {};
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        //load data
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("mapping/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getsavedata;
                $scope.savedata1 = promise.save;
                $scope.presentCountgrid = $scope.savedata1.length;

            })
        };
        $scope.submitted = false;
       
        //save data
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "PLMCLSMAP_Id": $scope.plmclsmaP_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "PLMCLSMAP_ClassName": $scope.obj.plmclsmaP_ClassName,
                    "PLMCLSMAP_ClassFlg": $scope.obj.plmclsmaP_ClassFlg,
                    "PLMCLSMAP_Remarks": $scope.obj.plmclsmaP_Remarks,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("mapping/savedata/", data).then(function (promise) {
                    if (promise.returnval == 'saved') {
                        swal("Record Saved Successfully");                        
                    }
                    else if (promise.returnval == 'notsaved') {
                        swal("Record Not saved");
                    }
                    else if (promise.returnval == 'Duplicate') {
                        swal("Record Already Exist");
                    }
                    else if (promise.returnval == 'update') {
                        swal("Record Update Sucessfully !");
                    }
                    else if (promise.returnval == 'Notupdate') {
                        swal("Record Not Update  !");

                    }
                    else if (promise.returnval == 'admin') {
                        swal("Please Contact Administrartor   !");

                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        //edit data
        $scope.edit = function (item) {
            var data = {
                "PLMCLSMAP_Id": item.plmclsmaP_Id
            }
            apiService.create("mapping/edit", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.editdata = promise.editdata;
                    $scope.plmclsmaP_Id = $scope.editdata[0].plmclsmaP_Id;
                    $scope.AMCO_Id = $scope.editdata[0].amcO_Id;
                    $scope.obj.plmclsmaP_ClassName = $scope.editdata[0].plmclsmaP_ClassName;
                    $scope.obj.plmclsmaP_ClassFlg = $scope.editdata[0].plmclsmaP_ClassFlg;
                    $scope.obj.plmclsmaP_Remarks = $scope.editdata[0].plmclsmaP_Remarks;
                  
                }
            })
        }

        //deactive
        $scope.deactive = function (item, SweetAlert) {
            $scope.PLMCLSMAP_Id = item.plmclsmaP_Id;
            var dystring = "";
            if (item.plmclsmaP_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.plmclsmaP_ActiveFlag == false) {
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
                        apiService.create("mapping/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == "true") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "!!!");
                                }
                               
                            })
                        $state.reload();
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }
        //cancel
        $scope.cancel = function () {
            $state.reload();
        }

        //validation
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }
})();