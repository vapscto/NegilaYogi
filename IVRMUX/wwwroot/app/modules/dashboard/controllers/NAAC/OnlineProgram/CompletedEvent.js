(function () {
    'use strict';
    angular
        .module('app')
        .controller('CompletedEventController', CompletedEventController)
    CompletedEventController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function CompletedEventController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
        //$rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        /* loading start*/
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("CompletedEvent/getloaddata", pageid).
                then(function (promise) {
                    $scope.fillyear = promise.fillyear;
                    $scope.programlist = promise.programlist;
                    $scope.totcountfirst = promise.fillyear.length;
                })
        }
        /* loading end*/
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            if (obj.pryrA_ActiveFlag == true) {
                $scope.test = "Active";
            } else if (obj.pryrA_ActiveFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.programname).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.eventname).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.start_time).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.end_time).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.description).indexOf(angular.lowercase($scope.searchValue)) >= 0; 
        }

        $scope.submitted = false;
            $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
              
                var data = {
                    "PRYRA_Id": $scope.PRYRA_Id,
                    "PRYR_Id": $scope.pryR_Id,
                    "Eventname": $scope.pgname,
                    "start_time": $filter('date')($scope.srtime, "h:mm a"),
                    "end_time": $scope.duration,
                    "description": $scope.pgdecpt,
                    }
                
                apiService.create("CompletedEvent/Savedata", data).
                    then(function (promise) {
                        if (promise.returnresult === true) {
                            if (promise.message == "Update") {
                                    swal('Record Updated Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully');
                                    $state.reload();
                                }
                            }
                            $scope.loaddata();
                        
                      
                    })
            }
            else {
            
                $scope.submitted = true;
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pryrA_Id;
            //var pageid = $scope.editEmployee;

            var data = {
                "PRYRA_Id": $scope.editEmployee,
            }


            apiService.create("CompletedEvent/getdetails", data).
                then(function (promise) {
                    $scope.pgname = promise.programlist[0].pryrA_ActivityName;
                    $scope.srtime = moment(promise.programlist[0].pryrA_StartTime, 'h:mm a').format(),
                    $scope.duration = promise.programlist[0].pryrA_Duration,
                    $scope.pgdecpt = promise.programlist[0].pryrA_Description;
                    $scope.pryR_Id = promise.programlist[0].pryR_Id;
                    $scope.PRYRA_Id = promise.programlist[0].pryrA_Id;
                    if (promise.groupHeadData[0].pryrA_ActiveFlag == true) {
                        $scope.pryrA_ActiveFlag = true;
                    }
                    else {
                        $scope.pryrA_ActiveFlag = false;
                    }

                })
        }


        $scope.cancel = function () {
            $state.reload();
        }

        $scope.deactive = function (groupHeadData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupHeadData.pryrA_ActiveFlag == 1) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


                        apiService.create("CompletedEvent/deactivate", groupHeadData).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " Successfully");
                                }
                                else if (promise.message == "used") {
                                    swal("Record already Used");
                                }
                                else {
                                    swal("Record " + confirmmgs + " Successfully");
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                })
        }





    }
})();







