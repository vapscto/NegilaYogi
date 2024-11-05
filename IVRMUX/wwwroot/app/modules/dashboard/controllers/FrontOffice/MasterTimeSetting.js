(function () {
    'use strict';
    angular
.module('app')
.controller('MasterTimeSettingController', MasterTimeSettingController)

    MasterTimeSettingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function MasterTimeSettingController($rootScope, $scope, $state, $location, Flash,appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.hstep1 = 1;
        $scope.mstep1 = 1;

        $scope.options = {
            hstep1: [1, 2, 3],
            mstep1: [1, 5, 10, 15, 25, 30]
        };
        
        $scope.ismeridian1 = true;
        $scope.toggleMode = function () {
            $scope.ismeridian1 = !$scope.ismeridian1;
        };
        //start.. by hema 7/11/2017
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.hstep2 = 1;
        $scope.mstep2 = 1;

        $scope.options = {
            hstep2: [1, 2, 3],
            mstep2: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian2 = true;
        $scope.toggleMode = function () {
            $scope.ismeridian2 = !$scope.ismeridian2;
        };
        $scope.clearid = function () {
            
            $scope.MorningTime = null;
            $scope.MorningTimeLogout = null;
            $scope.Totwork = null;
            $scope.AfternoonTime = null;
            $scope.AfternoonTimeLogout = null;
            $scope.HalfdayWork = null;
            $scope.MinTimePuch = null;
            $scope.Dailyshift = null;
            $scope.EarlyShift = null;
            $scope.LunchHour = null;
            $scope.OverTimeWork = null;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.validatemorTomintime = function (timedata) {
            $scope.MorningTimeLogout = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }
        $scope.validateAftnoonTomintime = function (afttimedata) {
            $scope.AfternoonTimeLogout = "";
            $scope.afttotime = afttimedata;
            var afhh = $scope.afttotime.getHours();
            var afmm = $scope.afttotime.getMinutes();
            $scope.afmin = afttimedata;

            $scope.afmin.setMinutes(afhh);
            $scope.afmin.setMinutes(afmm);
        }
        //end by hema 7/11/2017
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("MasterTimeSetting/getalldetails", pageid).then(
                function (promise) {
                    if (promise.count > 0)
                    {
                        $scope.gridOptions.data = promise.getlist;
                    }
                    else {
                        swal("Record not found.");
                    }
                 
                }
                    )
        }

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SlNo', field: 'name', enableFiltering: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'fomtS_FDWHrMin', displayName: 'Total Work Hour' },
              { name: 'fomtS_HDWHrMin', displayName: 'Half Day Work Hour' },
              { name: 'fomtS_IHalfLoginTime', displayName: 'Morning Login' },
              { name: 'fomtS_IIHalfLoginTime', displayName: 'Morning Logout' },
              { name: 'fomtS_IhalfLogoutTime', displayName: 'Afternoon Login' },
              { name: 'fomtS_IIHalfLogoutTime', displayName: 'Afternoon Logout' },
              { name: 'fomtS_DelayPerShiftHrMin', displayName: 'Delay Shift' },
              { name: 'fomtS_EarlyPerShiftHrMin', displayName: 'Early Shift' },
              { name: 'fomtS_LunchHoursDuration', displayName: 'Lunch Hour' },
              { name: 'fomtS_BlockAttendance', displayName: 'Time Between Two Punching' },
            //  { name: 'fomtS_FixTimings', displayName: '' }
              //,

              // {
                //   field: 'id', name: '',
                  // displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 //'<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" > <i class="fa fa-pencil-square-o" ></i></a>' +
                // '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);" > <i class="fa fa-trash text-danger"></i></a>' +
                // '</div>'
              // }
            ]

            //ng-click="getorgvalue(user,categorylist,classlist)"
            //ng-click="deletedata(user)"
        };


        


        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            //$scope.editEmployee = employee.ttcC_Id;
            //var pageid = $scope.editEmployee;
            //swal({
            //    title: "Are you sure?",
            //    text: "Do you want to delete record !!!!!!!!",
            //    type: "warning",
            //    showCancelButton: true,
            //    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
            //    cancelButtonText: "Cancel!!!!!!",
            //    closeOnConfirm: false,
            //    closeOnCancel: false
            //},
            //function (isConfirm) {
            //    if (isConfirm) {
            //        apiService.DeleteURI("CategoryClassMapp/deletedetails", pageid).
            //        then(function (promise) {

            //            if (promise.returnval === true) {
            //                swal('Record Deleted Successfully!', 'success');
            //                $scope.detailslist = promise.detailslist;
            //                $state.reload();
            //            }
            //            else {
            //                swal('Record Not Deleted Successfully!', 'Failed');
            //            }
            //        })
            //    }
            //    else {
            //        swal("Record Deletion Cancelled", "Failed");
            //    }
            //});
        }

        $scope.getorgvalue = function (employee) {
            
            //$scope.editEmployee = employee;
            //var pageid = $scope.editEmployee;

            ////$scope.ttcC_Id = employee;

            //for (var i = 0; i < $scope.classlist.length; i++) {
            //    name1 = $scope.classlist[i].asmcL_Id1
            //    if (name1 == true) {
            //        $scope.classlist[i].asmcL_Id1 = '';
            //    }
            //}

            //var data = {
            //    "ASMAY_Id": $scope.asmaY_Id,
            //    "TTMC_Id": pageid
            //}

            //// apiService.getURI("CategoryClassMapp/getdetails", pageid).

            //apiService.create("CategoryClassMapp/getdetails", data).
            //then(function (promise) {

            //   // $scope.ttmC_Id = promise.binddetails[0].ttmC_Id;
            //   // $scope.asmaY_Id = promise.binddetails[0].asmaY_Id;
            //    $scope.classlist = promise.classlist;

            //    $scope.checkboxchckedcls = [];

            //    for (var i = 0; i < $scope.classlist.length; i++) {
            //        name1 = $scope.classlist[i].asmcL_Id
            //        for (var j = 0; j < promise.binddetails.length; j++) {
            //            if (name1 == promise.binddetails[j].asmcL_Id) {
            //                $scope.classlist[i].asmcL_Id1 = true;
            //                $scope.checkboxchckedcls.push(promise.binddetails[j]);
            //            }
            //        }
            //    }

            //   // $scope.asmcL_Id = promise.binddetails[0].asmcL_Id;

            //})
        }
        var name = "";
        var name1 = "";
 
        $scope.submitted = false;
        $scope.saveddata = function () {
            
        
            if ($scope.myForm.$valid) {
                
                var data = {
                    "FOMTS_Id":$scope.fomtS_Id,
                    "FOMTS_FDWHrMin": $filter('date')($scope.Totwork, "h:mm a"),
                    "FOMTS_HDWHrMin" :  $filter('date')($scope.HalfdayWork, "h:mm a"),
                    "FOMTS_IHalfLoginTime":  $filter('date')($scope.MorningTime, "h:mm a"),
                    "FOMTS_IhalfLogoutTime":  $filter('date')($scope.MorningTimeLogout, "h:mm a"),
                    "FOMTS_IIHalfLoginTime" :  $filter('date')($scope.AfternoonTime, "h:mm a"),
                    "FOMTS_IIHalfLogoutTime":  $filter('date')($scope.AfternoonTimeLogout, "h:mm a"),
                    "FOMTS_DelayPerShiftHrMin" :  $filter('date')($scope.Dailyshift, "h:mm a"),
                    "FOMTS_EarlyPerShiftHrMin" :  $filter('date')($scope.EarlyShift, "h:mm a"),
                    "FOMTS_LunchHoursDuration" :  $filter('date')($scope.LunchHour, "h:mm a"),
                    "FOMTS_BlockAttendance" :  $filter('date')($scope.MorningTime, "h:mm a"),
                    "FOMTS_FixTimings" :  $filter('date')($scope.MinTimePuch, "h:mm a"),
                    //"FOMHWD_ActiveFlg" : $scope.FOMHWD_ActiveFlg,
        
                }
                apiService.create("MasterTimeSetting/savedetail", data).
                    then(function (promise) {
                        if(promise.returnduplicatestatus=="Duplicate")
                        {
                            swal("Record already exist.");
                        }
                        else if(promise.returnval==true)
                        {
                            swal("Record Saved Successfully");
                        }
                    })
            }
            else
            {
                $scope.submitted = true;
            }
   
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }

})();