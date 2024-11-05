(function () {
    'use strict';
    angular
.module('app')
        .controller('YearlyProgramController', YearlyProgramController)
    YearlyProgramController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function YearlyProgramController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
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
            apiService.getURI("YearlyProgram/getloaddata", pageid).
            then(function (promise) {
                $scope.fillyear = promise.fillyear;
                $scope.programlist = promise.programlist;


               


            })
        }
        /* loading end*/



        $scope.onselectyear = function () {

            angular.forEach($scope.fillyear, function (yea) {

                if (parseInt(yea.asmaY_Id) === parseInt($scope.ASMAY_Id)) {
                    $scope.from_date = new Date(yea.asmaY_From_Date);
                    $scope.to_date = new Date(yea.asmaY_To_Date);
                }
            });

        }

        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
              
                var data = {
                        "PRYR_Id": $scope.pryR_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "programname": $scope.pgname,
                        "Fromdate": $scope.from_date,
                        "Todate": $scope.to_date,
                        "start_time": $filter('date')($scope.srtime, "h:mm a"),
                        "end_time": $filter('date')($scope.edtime, "h:mm a"),
                        "description": $scope.pgdecpt,
                       
                    }
                
                apiService.create("YearlyProgram/Savedata", data).
                    then(function (promise) {
                            if (promise.returnval === true) {
                                if (promise.message != null) {
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
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pryR_Id;
            //var pageid = $scope.editEmployee;

            var data = {
                "PRYR_Id": $scope.editEmployee,
            }


            apiService.create("YearlyProgram/getdetails", data).
                then(function (promise) {

                    $scope.ASMAY_Id = 11;
                    $scope.pgname = promise.programlist[0].pryR_ProgramName;
                    $scope.from_date = new Date(promise.programlist[0].pryR_StartDate); 

                    $scope.to_date = new Date(promise.programlist[0].pryR_EndDate);
                    $scope.srtime = moment(promise.programlist[0].pryR_StartTime, 'h:mm a').format(),
                    $scope.edtime = moment(promise.programlist[0].pryR_EndTime, 'h:mm a').format(),
                    $scope.pgdecpt = promise.programlist[0].pryR_ProgramDescription;
                   

                    if (promise.groupHeadData[0].pryR_ActiveFlag == true) {
                        $scope.pryR_ActiveFlag = true;
                    }
                    else {
                        $scope.pryR_ActiveFlag = false;
                    }
                    
                })
        }


        $scope.deactive = function (groupHeadData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupHeadData.pryR_ActiveFlag == 1) {
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


                        apiService.create("YearlyProgram/deactivate", groupHeadData).
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







