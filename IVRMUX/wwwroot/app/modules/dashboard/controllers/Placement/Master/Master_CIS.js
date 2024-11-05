(function () {
    'use strict';
    angular
        .module('app')
        .controller('Master_CISController', Master_CISController)
    Master_CISController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function Master_CISController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
            var pageid = 2;
            apiService.getURI("Master_CIS/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getsavedata;
                $scope.presentCountgrid = $scope.getreport.length;
            

            })
        };
        $scope.submitted = false;
       
        //save data
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.from_date = new Date($scope.Fromdate).toDateString();
            $scope.to_date = new Date($scope.Todate).toDateString();
            if ($scope.myForm.$valid) {
                var data = {
                   
                    "PLCISCH_JobDetails": $scope.obj.JobDetails,
                    "PLCISCH_InterviewVenue": $scope.obj.InterviewVenue,
                    "PLCISCH_DriveFromDate": new Date($scope.obj.FromDate).toDateString(),
                    "PLCISCH_DriveToDate": new Date($scope.obj.ToDate).toDateString(),
                    "PLCISCH_Id": $scope.obj.plciscH_Id,
                  
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Master_CIS/savedata", data).then(function (promise) {
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
                        swal("Please Contact Administrator!");

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
                "PLCISCH_Id": item.plciscH_Id
            }
            apiService.create("Master_CIS/edit", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.editdata = promise.editdata;

                    $scope.obj.InterviewVenue = $scope.editdata[0].plciscH_InterviewVenue;
                    $scope.obj.JobDetails = $scope.editdata[0].plciscH_JobDetails;
                    $scope.obj.FromDate = new Date($scope.editdata[0].plciscH_DriveFromDate);
                    $scope.obj.ToDate = new Date($scope.editdata[0].plciscH_DriveToDate);
                    $scope.obj.plciscH_Id = $scope.editdata[0].plciscH_Id;
                }
            })
        }
//deactive
        $scope.deactive = function (item, SweetAlert) {
            $scope.PLCISCH_Id = item.plciscH_Id;
            var dystring = "";
            if (item.plciscH_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.plciscH_ActiveFlag == false) {
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
                        apiService.create("Master_CIS/deactive", item).
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