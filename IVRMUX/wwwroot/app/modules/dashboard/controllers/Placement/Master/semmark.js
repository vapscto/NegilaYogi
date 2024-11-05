(function () {
    'use strict';
    angular
        .module('app')
        .controller('semmarkController', semmarkController)
    semmarkController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function semmarkController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.obj = {};

        //load data
        $scope.loaddata = function () {
            var pageid = 2
            apiService.getURI("semmark/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getsavedata;
                $scope.joblist = promise.jobtitlelist;
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
                    "PLCISCHCOMJTSEM_Id": $scope.obj.plcischcomjtseM_Id,
                    "PLCISCHCOMJT_Id": $scope.obj.plcischcomjT_Id,
                    "AMSE_Id": $scope.obj.amsE_Id,
                    "PLCISCHCOMJTSEM_CutOfMarks": $scope.obj.plcischcomjtseM_CutOfMarks,
                    "PLCISCHCOMJTSEM_OtherDetails": $scope.obj.plcischcomjtseM_OtherDetails
                }
           
                apiService.create("semmark/savedata", data).then(function (promise) {
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

        //edit
        $scope.edit = function (item) {
            var data = {
                "PLCISCHCOMJTSEM_Id": item.plcischcomjtseM_Id
            }
            apiService.create("semmark/edit", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.editdata = promise.editdata;
                    
                    $scope.obj.plcischcomjtseM_Id = $scope.editdata[0].plcischcomjtseM_Id;
                    $scope.obj.plcischcomjT_Id = $scope.editdata[0].plcischcomjT_Id;
                    $scope.obj.amsE_Id = $scope.editdata[0].amsE_Id;
                    $scope.obj.plcischcomjtseM_CutOfMarks = $scope.editdata[0].plcischcomjtseM_CutOfMarks;
                    $scope.obj.ToDate = $scope.editdata[0].plciscH_DriveToDate;
                    $scope.obj.plcischcomjtseM_OtherDetails = $scope.editdata[0].plcischcomjtseM_OtherDetails;
                }
            })
        }

        //deactive
        $scope.deactive = function (item, SweetAlert) {
            $scope.PLCISCHCOMJTSEM_Id = item.plcischcomjtseM_Id;
            var dystring = "";
            if (item.plcischcomjtseM_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.plcischcomjtseM_ActiveFlag == false) {
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
                        apiService.create("semmark/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == "true") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "!!!");
                                }
                                $state.reload();
                            })
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