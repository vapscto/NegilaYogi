
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGBreakTimeSettingController', CLGBreakTimeSettingController)

    CLGBreakTimeSettingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache']
    function CLGBreakTimeSettingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache) {

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

        $scope.validateTomintime = function (timedata) {

            //$scope.timedis1 = false;
            //$scope.timedis2 = true;
            $scope.TTMB_BreakEndTime = "";
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

            // $scope.TTMB_BreakStartTime = maxdata;
            //$scope.TTMB_BreakEndTime = "";
            var dsttimee = $scope.TTMB_BreakStartTime;
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




            if (maxdata >= new Date($scope.TTMB_BreakStartTime)) {
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

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'ttmC_CategoryName', displayName: 'Category' },
                { name: 'amcO_CourseName', displayName: 'Course' },
                { name: 'amB_BranchName', displayName: 'Branch' },
                { name: 'amsE_SEMName', displayName: 'Sem' },
                { name: 'ttmD_DayName', displayName: 'Day' },
                   { name: 'ttmbC_BreakName', displayName: 'Break Name' },
                  { name: 'ttmbC_BreakStartTime', displayName: 'Start Time' },
                  { name: 'ttmbC_BreakEndTime', displayName: 'End Time' },
                  { name: 'ttmbC_AfterPeriod', displayName: 'After Period' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity,$scope.academic);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmbC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmbC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

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
                debugger;
                $scope.albumNameArrayClassList = [];
                angular.forEach($scope.semisterlist, function (option) {
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
                debugger;
                if ($scope.albumNameArrayClassList.length > 0 && $scope.albumNameArrayDayList.length > 0) {
                    var data = {
                        "TTMBC_Id": $scope.TTMBC_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "TTMC_Id": $scope.TTMC_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "ArrayClassList": $scope.albumNameArrayClassList,
                        "ArrayDayList": $scope.albumNameArrayDayList,
                        "TTMBC_AfterPeriod": $scope.TTMB_AfterPeriod,
                        "TTMBC_BreakName": $scope.TTMB_BreakName,
                        "TTMBC_BreakStartTime": starttm,
                        "TTMBC_BreakEndTime": endtm,
                        "ArraybeforeperiodsList": $scope.albumNameArraybeforeperiodsList,
                        "ArrayafterperiodsList": $scope.albumNameArrayafterperiodsList,
                        "TTMB_ActiveFlag": true,
                    }
                    apiService.create("CLGBreakTimeSetting/savetimedetail", data).
                        then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Data successfully Saved');
                               // $scope.clearid();
                              //  $scope.BindData();

                            }
                            else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                                swal('Records Already Exist !');
                            }
                            else {
                                swal('Data Not Saved !');
                            }
                            $state.reload();
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
            var pageid = 1;
            apiService.getURI("CLGBreakTimeSetting/getalldetails", pageid).
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.categorylst = promise.categorylist;
           $scope.academic = promise.academiclist;
           $scope.arrlist3 = promise.daydropdown;
           $scope.gridOptions.data = promise.breaktimelist

       })
        };
        //TO clear  data
        $scope.clearid222 = function () {
            $state.reload();
        }
        $scope.clearid111 = function () {
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
            $scope.editEmployee = employee.ttmbC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("CLGBreakTimeSetting/geteditdetails", pageid).
            then(function (promise) {
                
              // $scope.clearid();
               $scope.asmaY_Id_flag = true;

                $scope.ASMAY_Id = promise.breaktimelistedit[0].asmaY_Id;
                $scope.TTMC_Id = promise.breaktimelistedit[0].ttmC_Id;
              

                $scope.get_course();
                $scope.AMCO_Id = promise.breaktimelistedit[0].amcO_Id;

                $scope.getbranch_catg();
                $scope.AMB_Id = promise.breaktimelistedit[0].amB_Id;

                $scope.get_semister1(promise.breaktimelistedit[0].amsE_Id);

               

                angular.forEach($scope.arrlist3, function (ff) {
                    if (ff.ttmD_Id == promise.breaktimelistedit[0].ttmD_Id) {
                        ff.selected = true;
                    }

                })

                //for (var k = 0; k < $scope.arrlist2.length; k++) {
                //    if ($scope.arrlist2[k].asmcL_Id == promise.breaktimelistedit[0].asmcL_Id) {
                //        $scope.arrlist2[k].selected = true;
                //    }
                //}
                //for (var k = 0; k < $scope.arrlist3.length; k++) {
                //    if ($scope.arrlist3[k].ttmD_Id == promise.breaktimelistedit[0].ttmD_Id) {
                //        $scope.arrlist3[k].selected = true;
                //    }
                //}

                $scope.TTMB_BreakName = promise.breaktimelistedit[0].ttmbC_BreakName;
                $scope.TTMB_BreakStartTime = moment(promise.breaktimelistedit[0].ttmbC_BreakStartTime, 'h:mm a').format();
                $scope.TTMB_BreakEndTime = moment(promise.breaktimelistedit[0].ttmbC_BreakEndTime, 'h:mm a').format();
                $scope.TTMB_AfterPeriod = promise.breaktimelistedit[0].ttmbC_AfterPeriod;
                $scope.TTMBC_Id = promise.breaktimelistedit[0].ttmbC_Id;
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
                    apiService.DeleteURI("CLGBreakTimeSetting/deletepages", pageid).
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
            angular.forEach($scope.semisterlist, function (option) {
                if (option.selected === true) {
                    count = count + 1;
                    clsid = option.amsE_Id;
                };
            });


            var data = {
                "classid": clsid,
                "classidscount": count,
            }
            apiService.create("CLGBreakTimeSetting/getmaximumperiodscount", data).
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

        $scope.get_course = function () {

            $scope.AMB_Id = '';
            $scope.semisterlist = [];
            if ($scope.ASMAY_Id === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("CLGTTCommon/getcourse_catg", data).
                    then(function (promise) {

                        $scope.courselist = promise.courselist;

                        if (promise.courselist == "" || promise.courselist == null) {
                            swal("No Course/Branch Are Mapped To Selected Category");
                        }
                    })
            }
        };

        $scope.getbranch_catg = function () {
            $scope.AMB_Id = '';
            $scope.semisterlist = [];
            var data = {
                "TTMC_Id": $scope.TTMC_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("CLGTTCommon/getbranch_catg", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("No Branch Are Mapped To Selected Category/Course");
                    }
                })

        };
        $scope.get_semister = function () {
            var data = {
                "TTMC_Id": $scope.TTMC_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("CLGTTCommon/get_semister", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;

                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("No Semester Are Mapped To Selected Course/Branch");
                    }
                })

        };

        $scope.get_semister1 = function (ww) {
            var data = {
                "TTMC_Id": $scope.TTMC_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("CLGTTCommon/get_semister", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;

                    angular.forEach($scope.semisterlist, function (gg) {
                        if (gg.amsE_Id == ww) {
                            gg.selected = true;
                        }

                    })

                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("No Semester Are Mapped To Selected Course/Branch");
                    }
                })

        };

        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmbC_ActiveFlag === true) {
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

                apiService.create("CLGBreakTimeSetting/deactivate", employee).
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
                apiService.create("CLGBreakTimeSetting/get_catg", data).
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