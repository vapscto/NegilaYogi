
(function () {
    'use strict';
    angular
.module('app')
.controller('BreakTimeSettingsController', BreakTimeSettingsController)

    BreakTimeSettingsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache']
    function BreakTimeSettingsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache) {

        // Time picker starts
        //$scope.timedis = true;
        //$scope.ScheduleTime = new Date();
        $scope.asmaY_Id_flag = false;
        $scope.asmaY_Id = "";
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
        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                  { name: 'asmayYear', displayName: 'Academic Year' },
                  { name: 'className', displayName: 'Class Name' },
                  { name: 'dayName', displayName: 'Day Name' },
                   { name: 'ttmB_BreakName', displayName: 'Break Name' },
                  { name: 'ttmB_BreakStartTime', displayName: 'BreakStartTime' },
                  { name: 'ttmB_BreakEndTime', displayName: 'BreakEndTime' },
                  { name: 'ttmB_AfterPeriod', displayName: 'AfterPeriod' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity,$scope.academic);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]

        };
        // Time picker end here

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                $scope.albumNameArrayClassList = [];
                angular.forEach($scope.arrlist2, function (option) {
                    if (!!option.selected) $scope.albumNameArrayClassList.push(option);
                })
                $scope.albumNameArrayDayList = [];
                angular.forEach($scope.arrlist3, function (option1) {
                    if (!!option1.selected) $scope.albumNameArrayDayList.push(option1);
                })
                $scope.albumNameArraybeforeperiodsList = [];
                angular.forEach($scope.arrbefor, function (optionb) {
                    $scope.albumNameArraybeforeperiodsList.push(optionb);
                })
                $scope.albumNameArrayafterperiodsList = [];
                angular.forEach($scope.arraftefoper, function (option) {
                    $scope.albumNameArrayafterperiodsList.push(option);
                })
                var starttm = $filter('date')($scope.TTMB_BreakStartTime, "h:mm a");
                var endtm = $filter('date')($scope.TTMB_BreakEndTime, "h:mm a");

                if ($scope.albumNameArrayClassList.length > 0 && $scope.albumNameArrayDayList.length > 0) {
                    var data = {
                        "TTMB_Id": $scope.TTMB_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "TTMC_Id": $scope.ttmC_Id,
                        "ArrayClassList": $scope.albumNameArrayClassList,
                        "ArrayDayList": $scope.albumNameArrayDayList,
                        "TTMB_AfterPeriod": $scope.TTMB_AfterPeriod,
                        "TTMB_BreakName": $scope.TTMB_BreakName,
                        "TTMB_BreakStartTime": starttm,
                        "TTMB_BreakEndTime": endtm,
                        "ArraybeforeperiodsList": $scope.albumNameArraybeforeperiodsList,
                        "ArrayafterperiodsList": $scope.albumNameArrayafterperiodsList,
                        "TTMB_ActiveFlag": true,
                    }
                    apiService.create("BreakTimeSettings/savedetail", data).
                        then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Data successfully Saved');
                                $scope.clearid();
                                $scope.BindData();

                            }
                            else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                                swal('Records Already Exist !');
                            }
                            else {
                                swal('Data Not Saved !');
                            }
                        })
                }
                else {
                    swal('Please Select Class And Day !');
                }
                //$scope.BindData();
                //$scope.clearid();
            }
        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("BreakTimeSettings/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
         //  $scope.asmaY_Id_flag = false;
           $scope.categorylst = promise.catelist;
           $scope.academic = promise.academiclist;
           $scope.arrlist2 = promise.classDrpDwn;
           $scope.arrlist3 = promise.daysDrpDwn;
           $scope.gridOptions.data = promise.breaktimelist

       })
        };
        //TO clear  data
        $scope.clearid = function () {
            angular.forEach($scope.arrlist2, function (option) {
                if (!!option.selected) option.selected = false;
            })

            angular.forEach($scope.arrlist3, function (option1) {
                if (!!option1.selected) option1.selected = false;
            })
            $scope.asmaY_Id_flag = false;
            $scope.asmaY_Id = "";
            $scope.TTMB_BreakName = "";
            $scope.TTMB_BreakStartTime = "";
            $scope.arrbefor = "";
            $scope.ttmC_Id = "";
            $scope.TTMB_AfterPeriod = "";
            $scope.TTMB_BreakEndTime = "";
            $scope.arraftefoper = "";

            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            

        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmB_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("BreakTimeSettings/getdetails", pageid).
            then(function (promise) {
                
               $scope.clearid();
               $scope.asmaY_Id_flag = true;
               
                $scope.asmaY_Id = promise.breaktimelistedit[0].asmaY_Id;
                $scope.ttmC_Id = promise.breaktimelistedit[0].ttmC_Id;

              
                for (var k = 0; k < $scope.arrlist2.length; k++) {
                    if ($scope.arrlist2[k].asmcL_Id == promise.breaktimelistedit[0].asmcL_Id) {
                        $scope.arrlist2[k].selected = true;
                    }
                }
                for (var k = 0; k < $scope.arrlist3.length; k++) {
                    if ($scope.arrlist3[k].ttmD_Id == promise.breaktimelistedit[0].ttmD_Id) {
                        $scope.arrlist3[k].selected = true;
                    }
                }

                $scope.TTMB_BreakName = promise.breaktimelistedit[0].ttmB_BreakName;
                $scope.TTMB_BreakStartTime = moment(promise.breaktimelistedit[0].ttmB_BreakStartTime, 'h:mm a').format();
                $scope.TTMB_BreakEndTime = moment(promise.breaktimelistedit[0].ttmB_BreakEndTime, 'h:mm a').format();
                $scope.TTMB_AfterPeriod = promise.breaktimelistedit[0].ttmB_AfterPeriod;
                $scope.TTMB_Id = promise.breaktimelistedit[0].ttmB_Id;
                $scope.get_periods();
            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmB_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("BreakTimeSettings/deletepages", pageid).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully!');
                        }
                        $scope.BindData();
                    })
                    $scope.BindData();
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.NOP_1 = "1";
        $scope.get_periods = function () {
            $scope.temparray_period = [];
            $scope.temparray_period1 = [];
            var clsid = 0;
            $scope.albumNameArraycolumn = [];
            var count = 0;
            angular.forEach($scope.arrlist2, function (option) {
                if (option.selected === true) {
                    count = count + 1;
                    clsid = option.asmcL_Id;
                };
            });


            var data = {
                "classid": clsid,
                "classidscount": count,
            }
            apiService.create("BreakTimeSettings/getmaximumperiodscount", data).
     then(function (promise) {
         
         $scope.countrrrrr = promise.classidscountreturn;
         if ($scope.countrrrrr < Number($scope.TTMB_AfterPeriod)) {
             swal("No of Maximum Periods for day exceed !!!");
             $scope.TTMB_AfterPeriod = "";
             $scope.arrbefor = "";
             $scope.arraftefoper = "";
         }
         else if ($scope.countrrrrr ===Number($scope.TTMB_AfterPeriod) ){
             var msg = "No of Maximum Periods is "+" " + $scope.TTMB_AfterPeriod + "  Kindly Enter Lessthan Maximum Periods";
             swal(msg);
             $scope.TTMB_AfterPeriod = "";
             $scope.arrbefor = "";
             $scope.arraftefoper = "";
         }
         else {
             var number = $scope.TTMB_AfterPeriod;
             for (var i = 1; i <= number; i++) {
                 $scope.temparray_period.push({
                     key: i,
                     TTPeriodnameB: i,
                 })
             }
             $scope.arrbefor = $scope.temparray_period;
             var number1 = $scope.TTMB_AfterPeriod;
             number1++;
             for (var i = number1; i <= promise.classidscountreturn; i++) {
                 $scope.temparray_period1.push({
                     key: i,
                     TTPeriodnameA: i,
                 })
             }
             $scope.arraftefoper = $scope.temparray_period1;
         }

     })

        };

        $scope.get_class = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {

                if ($scope.ttmC_Id === "" || $scope.ttmC_Id === undefined) {
                    // swal("Please Select the Academic Year !");
                }
                else {
                    var data = {
                        "TTMC_Id": $scope.ttmC_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                    }
                    apiService.create("BreakTimeSettings/getclass_catg", data).
             then(function (promise) {

                 $scope.arrlist2 = promise.classbycategory;
                 if (promise.classbycategory == "" || promise.classbycategory == null) {
                     swal("No classes Are Mapped To Selected Category");
                 }
             })
                }
            }


        };

        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmB_ActiveFlag === true) {
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

                apiService.create("BreakTimeSettings/deactivate", employee).
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


        $scope.get_category = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("BreakTimeSettings/get_catg", data).
         then(function (promise) {

             $scope.categorylst = promise.catelist;
             if (promise.catelist == "" || promise.catelist == null) {
                 swal("No Category Are Mapped To Selected Academic Year");
             }
         })

            }


        };


    }

})();