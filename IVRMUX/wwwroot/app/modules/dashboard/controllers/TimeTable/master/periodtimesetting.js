
(function () {
    'use strict';
    angular
.module('app')
.controller('PeriodTimeSettingController', PeriodTimeSettingController)

    PeriodTimeSettingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter']
    function PeriodTimeSettingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter) {
        $scope.editEmployee = {};

        // Time picker starts
        //$scope.timedis = true;
        //$scope.ScheduleTime = new Date();

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

        $scope.isOptionsRequired = function () {

            return !$scope.Day.some(function (options) {
                return options.cls;
            });
        }

         $scope.validateTomintime = function (timedata) {

            //$scope.timedis1 = false;
            //$scope.timedis2 = true;
             $scope.EndTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.FOMST_IHalfLogoutTime = "";
        }


        $scope.validatemax = function (maxdata) {

            // $scope.StartTime = maxdata;
            //$scope.EndTime = "";
            var dsttimee = $scope.StartTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            //var startTime = moment(dsttimee, "HH:mm:ss a");
            //  var endTime = moment(maxdata, "HH:mm:ss a");
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.FOMST_FDWHrMin = $scope.ttst;
            //   $scope.FOMST_HDWHrMin = $scope.ttst;


            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);




            if (maxdata >= new Date($scope.StartTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.EndTime = "";
            }

            // $scope.FOMST_IHalfLogoutTime = "";
        }
        // Time picker end here

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 { name: 'Sl No', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
              { name: 'ttmC_CategoryName', displayName: 'Category Name' },
            { name: 'ttmP_PeriodName', displayName: ' Period' },
            { name: 'ttmD_DayName', displayName: 'Day' },
             { name: 'ttmdpT_StartTime', displayName: 'Start Time' },
             { name: 'ttmdpT_EndTime', displayName: 'End Time' },

            {
                field: 'id', name: '',
                displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                '<div class="grid-action-cell">' +
                '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
               '<a ng-if="row.entity.ttmdpT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
          '<span ng-if="row.entity.ttmdpT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
            }
            ]

        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var dayyylst = [];
                angular.forEach($scope.Day, function (gg) {
                    if (gg.cls == true) {
                        dayyylst.push(gg);
                    }  

                })

                var data = {
                    "TTMDPT_Id": $scope.TTMDPT_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMC_Id": $scope.ttmC_Id,
                    "TTMP_Id": $scope.ttmP_Id,
                   // "TTMD_Id": $scope.ttmD_Id,
                    "TTMDPT_StartTime": $filter('date')($scope.StartTime, "h:mm a"),
                    "TTMDPT_EndTime": $filter('date')($scope.EndTime, "h:mm a"),
                    datidss: dayyylst

                }
                apiService.create("PeriodTimeSetting/savedetail", data).
                    then(function (promise) {


                        swal('Record Saved/Updated Successfully');


                        $scope.clearid();
                        $scope.BindData();
                        //if (promise.returnval === true) {
                        //    swal('Data Updated successfully');                           
                        //    $scope.clearid();
                        //    $scope.BindData();
                        //}
                    })
            }
            else {
                $scope.submitted = true;

            }

        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("PeriodTimeSetting/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.AYear = promise.academicdrp;
           $scope.Category = promise.categorylist;
           $scope.Day = promise.daydrp;
           $scope.Period = promise.perioddrp;
           $scope.gridOptions.data = promise.gridview;
       })
        };
        //TO clear  data
        $scope.clearid = function () {
            
            $scope.TTMC_CategoryName = "";
            $scope.TTMC_Id = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.StartTime = "";
            $scope.EndTime = "";
            $scope.asmaY_Id = "";
            $scope.ttmD_Id = "";
            $scope.ttmC_Id = "";
            $scope.ttmP_Id = "";
            $scope.TTMDPT_Id =0;
        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            
            $scope.editperiod = employee.ttmdpT_Id;
            var editid = $scope.editperiod;
            angular.forEach($scope.classlist, function (role) {
                role.class = false;
            })
            apiService.getURI("PeriodTimeSetting/getpagedetails", editid).
            then(function (promise) {
                $scope.timing = moment(promise.periodlistedit[0].ttmdpT_StartTime, 'h:mm a').format();
                $scope.timing1 = moment(promise.periodlistedit[0].ttmdpT_EndTime, 'h:mm a').format();
                $scope.TTMDPT_Id = promise.periodlistedit[0].ttmdpT_Id;
                $scope.asmaY_Id = promise.periodlistedit[0].asmaY_Id;
                $scope.ttmC_Id = promise.periodlistedit[0].ttmC_Id;
             //   $scope.ttmD_Id = promise.periodlistedit[0].ttmD_Id;
                $scope.ttmP_Id = promise.periodlistedit[0].ttmP_Id;
                $scope.StartTime = $scope.timing;
                $scope.EndTime = $scope.timing1;

                angular.forEach($scope.Day, function (ff) {

                    if (ff.ttmD_Id == promise.periodlistedit[0].ttmD_Id) {
                        ff.cls = true;
                    }
                })


            })
        }

        $scope.deactive1 = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmdpT_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("PeriodTimeSetting/deactive", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

    }

})();