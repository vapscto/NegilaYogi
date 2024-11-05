
(function () {
    'use strict';
    angular
.module('app')
.controller('PeriodAllocationController', PeriodAllocationController)

    PeriodAllocationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$stateParams', '$filter']
    function PeriodAllocationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $stateParams, $filter) {
        $scope.editEmployee = {};
        var curDate = new Date();
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

        // Time picker end here

        //Ui Grid view rendering data from data base
        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                   { name: 'ttmP_PeriodName', displayName: 'Period Name' },
                  // { name: 'asmcL_ClassName', displayName: 'Class Name' },
                 //  { name: 'ttmD_DayName', displayName: 'Day Name' },
                  // { name: 'tT_Period', displayName: 'No Of Periods' },               

                  {
                      field: 'id', name: '',
                      displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                    '<div class="grid-action-cell">' +
                   // '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a> &nbsp; &nbsp;' +
                   '<a ng-if="row.entity.ttmP_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                 '<span ng-if="row.entity.ttmP_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                  //'<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                   '</div>'

                  }
            ]

        };


        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                   { name: 'asmaY_Year', displayName: 'Academic Year' },
                   { name: 'asmcL_ClassName', displayName: 'Class Name' },
                  // { name: 'ttmD_DayName', displayName: 'Day Name' },
                   { name: 'ttmP_PeriodName', displayName: 'Period Name' },
                  // { name: 'ttmdpTyjyuj_StartTime', displayName: 'jjjgjvjifbk Time' },
                   //  { name: 'ttmdphjT_StartTime', displayName: 'School Time' },
                  // { name: 'ttmdpT_EndTime', displayName: 'End Time' },
                  {
                      field: 'id', name: '',
                      displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                    '<div class="grid-action-cell">' +
                   // '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue2(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a> ' +
                '<a ng-if="row.entity.ttmpC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmpC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
               // '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                    '</div>'

                  }
            ]

        };

        //to clear
        $scope.clearid1 = function () {
            
            $scope.ASMAY_Id1 = "";
            $scope.NOP_1 = 1;
            $scope.get_periods();
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";



        };
        //to save period
        $scope.submitted1 = false;
        $scope.save_period = function () {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                var data = {
                    tempperiods: $scope.temparray_period
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("PeriodAllocation/saveperiod/", data).
                    then(function (promise) {
                        
                        $scope.count = promise.count;

                        if (promise.cannot === true) {
                            swal('Periods Already Mapped To Classes !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                        $scope.BindData();
                        $scope.clearid1();
                    })
            }
            else {
                $scope.submitted1 = true;

            }
        }
        //get count
        $scope.getorgvalue1 = function () {
            $scope.NOP_1 = $scope.count;
            $scope.get_periods();

        }


        // TO Save The Data
        $scope.submitted2 = false;
        $scope.save_period_class = function () {
            $scope.submitted2 = true;


            if ($scope.myForm2.$valid) {

                $scope.checkclaaslst = [];
                angular.forEach($scope.classlist, function (role) {
                    if (role.class) $scope.checkclaaslst.push(role);
                })

                // var sttime=$scope.StartTime.tostring();
                var data = {


                    "TTMPC_Id": $scope.TTMPC_Id,
                    temp_period_Array: $scope.temparray_period_class,
                    "ASMAY_Id": $scope.ASMAY_Id2,
                    Temp_class_Array: $scope.checkclaaslst

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("PeriodAllocation/savedetail/", data).
                    then(function (promise) {
                        


                        if (promise.returnval === true) {
                            if ($scope.TTMPC_Id > 0 && $scope.TTMPC_Id != undefined) {
                                swal('Data Updated successfully');
                            }
                            else if ($scope.checkclaaslst.length > 1) {
                                swal('Select Only One Class !!!');
                            }
                            else {
                                swal('Data successfully Saved');
                            }
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                            // $state.reload();
                        }
                        else {
                            swal('Data Not Saved !');
                            // $state.reload();
                        }
                        // $scope.gridOptions2.data = promise.all_list;
                        $scope.BindData();

                    })
                $scope.clearid2();
                // $scope.BindData();
                // $scope.clearid();
                //  $scope.clearid2();
            }
            else {
                $scope.submitted2 = true;

            }

        };
        $scope.NOP_2 = "1";
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            $scope.get_periods();
            $scope.get_periods_class();
            
            //   $scope.categoryl=null;
            apiService.getDATA("PeriodAllocation/getalldetails").
       then(function (promise) {
           
           $scope.year_list = promise.acayear;
           $scope.Category_list = promise.catelist;
           $scope.temp_category_list = promise.catelist;
           $scope.daylist = promise.day_list;
           $scope.Temp_class_list = promise.tempararyArray;
           $scope.classlist = promise.tempararyArray;
           $scope.gridOptions1.data = promise.periodlist;
           $scope.gridOptions2.data = promise.all_list;
           $scope.count = promise.count;
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           //  $scope.gridOptions.data = promise.categorylist;
       })
        };
        //to get periods
        $scope.NOP_1 = "1";
        $scope.get_periods = function () {
            var number = $scope.NOP_1;
            $scope.temparray_period = [];
            for (var i = 1; i <= number; i++) {
                $scope.temparray_period.push({
                    key: i,
                    TTMP_PeriodName: i,
                })
            }
            $scope.albumNameArraycolumn = [];

        }

        //$scope.get_periods_class = function () {
        //    var number = $scope.NOP_2;
        //    $scope.temparray_period_class = [];
        //    for (var i = 1; i <= number; i++) {
        //        $scope.temparray_period_class.push({
        //            TTMP_Id: i,
        //            TTMP_PeriodName: i,
        //        })
        //    }
        //    $scope.albumNameArraycolumn = [];

        //}
        $scope.get_periods_class = function () {
            
            var number = $scope.NOP_2;
            var data = {
                "period_count": $scope.NOP_2
            }
            apiService.create("PeriodAllocation/getperiod_class", data).
     then(function (promise) {
         
         $scope.temparray_period_class = promise.periodlist_class;
         //if (promise.tempararyArray == "" || promise.tempararyArray == null) {
         //    swal("No classes Are Mapped To Selected Category");
         //}
     })

        };



        //to get class by category
        $scope.ASMAY_Id2 = "";
        $scope.categoryl = "";
        $scope.get_class = function () {
            
            if ($scope.ASMAY_Id2 === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id2 != "" && $scope.categoryl != "") {
                var data = {
                    "TTMC_Id": $scope.categoryl,
                    "ASMAY_Id": $scope.ASMAY_Id2,
                }
                apiService.create("PeriodAllocation/getclass_catg", data).
         then(function (promise) {

             $scope.classlist = promise.tempararyArray;
             if ($scope.TTMPC_Id != "" && $scope.TTMPC_Id != 0) {
                 angular.forEach($scope.classlist, function (role) {
                     
                     if (role.asmcL_Id == $scope.temp_class) {
                         role.class = true;
                         $scope.class = role.asmcL_Id;
                         //$scope.categoryl = role.ttmC_Id;
                         //role.Selected = true;
                     }
                 })

                 //angular.forEach($scope.classlist, function (role) {
                 //    if (role.asmcL_Id == promise.periodlistedit[0].asmcL_Id)
                 //        role.class = true;
                 //    $scope.temp_category = role.ttmC_Id;
                 //    $scope.temp_class = role.asmcL_Id;
                 //    $scope.class = role.asmcL_Id;
                 //    $scope.categoryl = role.ttmC_Id;
                 //})
             }
             if (promise.tempararyArray == "" || promise.tempararyArray == null) {
                 swal("No classes Are Mapped To Selected Category");
             }
         })
            }
        };

        //$scope.get_category = function () {
        //    
        //    if ($scope.ASMAY_Id2 === "") {
        //        swal("Please Select The Academic Year !");
        //    }
        //    else if ($scope.ASMAY_Id2 != "" ) {
        //        var data = {
        //           // "TTMC_Id": $scope.categoryl,
        //            "ASMAY_Id": $scope.ASMAY_Id2,
        //        }
        //        apiService.create("PeriodAllocation/get_catg", data).
        // then(function (promise) {
        //     
        //     $scope.Category_list = promise.tempararyArray_categ;
        //     $scope.categoryl = ""; 
        //     if ($scope.TTMPC_Id != "" && $scope.TTMPC_Id != 0) {
        //         angular.forEach($scope.Category_list, function (role) {
        //             
        //             if (role.ttmC_Id == $scope.temp_category) {
        //                 $scope.categoryl = role.ttmC_Id;
        //                 role.Selected = true;
        //             }
        //         })
        //     }

        //     if (promise.tempararyArray_categ == "" || promise.tempararyArray_categ == null) {
        //         swal("No Catogoriess Are Mapped To Selected Academic Year");
        //     }
        // })
        //    }
        //};


        //TO clear  data
        $scope.clearid2 = function () {
            $scope.ASMAY_Id2 = "";
            $scope.TTMPC_Id = 0;
            $scope.categoryl = "";
            $scope.NOP_2 = 1;
            $scope.get_periods_class();
            $scope.Category_list = $scope.temp_category_list;
            $scope.classlist = $scope.Temp_class_list;
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
            angular.forEach($scope.classlist, function (role) {
                role.class = false;
            })

        };


        $scope.isOptionsRequired = function () {

            return !$scope.classlist.some(function (options) {
                return options.class;
            });
        }
        //to edit record
        $scope.getorgvalue2 = function (period_class) {
            

            $scope.editperiod = period_class.ttmpC_Id;
            var editid = $scope.editperiod;
            apiService.getURI("PeriodAllocation/getdetails", editid).
            then(function (promise) {
                $scope.TTMPC_Id = promise.periodlistedit[0].ttmpC_Id;
                $scope.ASMAY_Id2 = promise.periodlistedit[0].asmaY_Id;
                $scope.categoryl = promise.periodlistedit[0].ttmC_Id;
                $scope.NOP_2 = promise.period_count;
                //$scope.get_periods_class();
                $scope.temp_period_class = [];
                $scope.temparray_period_class = [];

                $scope.temparray_period_class.push({
                    ttmP_Id: promise.period_count,
                    ttmP_PeriodName: promise.period_count,
                })

                for (var i = 0; i < $scope.classlist.length; i++) {
                    if ($scope.classlist[i].asmcL_Id == promise.periodlistedit[0].asmcL_Id) {
                        $scope.classlist[i].class = true;
                    }
                }
                // $scope.get_periods_class();

            })
            $scope.clearid2();


            //$scope.editperiod = period_class.asmcL_Id;
            //var editid = $scope.editperiod;
            //angular.forEach($scope.classlist, function (role) {
            //    role.class = false;
            //})
            //apiService.getURI("PeriodAllocation/getdetails", editid).
            //then(function (promise) {
            //    
            //    $scope.TTMPC_Id = promise.periodlistedit[0].ttmpC_Id;
            //    $scope.ASMAY_Id2 = promise.periodlistedit[0].asmaY_Id;
            //    //  $scope.categoryl = promise.periodlistedit[0].ttmC_Id;
            //    angular.forEach($scope.classlist, function (role) {
            //        
            //        if (role.asmcL_Id == promise.periodlistedit[0].asmcL_Id) {
            //            role.class = true;
            //            $scope.temp_category = role.ttmC_Id;
            //            $scope.temp_class = role.asmcL_Id;
            //            $scope.class = role.asmcL_Id;
            //            $scope.categoryl = role.ttmC_Id;
            //        }
            //    })

            //    if ($scope.ASMAY_Id2 != "" && $scope.categoryl != "") {
            //        $scope.get_class();
            //       $scope.class = $scope.temp_class;
            //    }
            //    $scope.NOP_2 = promise.period_count;
            //    $scope.get_periods_class();
            //    $scope.asmcL_Id = promise.periodlistedit[0].asmcL_Id;

            //})
        }



        //TO  Edit Record
        $scope.getorgvalue = function (period) {
            
            $scope.editperiod = period.ttmdpT_Id;
            var editid = $scope.editperiod;
            angular.forEach($scope.classlist, function (role) {
                role.class = false;
            })
            apiService.getURI("PeriodAllocation/getdetails", editid).
            then(function (promise) {
                
                $scope.timing = moment(promise.periodlistedit[0].ttmdpT_StartTime, 'h:mm a').format();
                $scope.timing1 = moment(promise.periodlistedit[0].ttmdpT_EndTime, 'h:mm a').format();

                $scope.TTMDPT_Id = promise.periodlistedit[0].ttmdpT_Id;
                $scope.categoryl = promise.periodlistedit[0].ttmC_Id;
                $scope.Period = promise.periodlistedit[0].tT_Period;
                $scope.Day_Name = promise.periodlistedit[0].tT_Day;

                angular.forEach($scope.classlist, function (role) {
                    if (role.asmcL_Id == promise.periodlistedit[0].asmcL_Id)
                        role.class = true;
                })
                $scope.StartTime = $scope.timing;
                $scope.EndTime = $scope.timing1;

                // return !$scope.classlist.some(function (options) {
                //  return options.class;
                //   $scope.TTMC_CategoryName = promise.categorylistedit[0].ttmC_CategoryName;
                //  $scope.TTMC_Id = promise.categorylistedit[0].ttmC_Id;
            })
        }


        //TO  delete Record
        $scope.deletedata = function (period, SweetAlert) {
            $scope.deleteId = period.ttmdpT_Id;
            var MdeleteId = $scope.deleteId;
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
                    apiService.DeleteURI("PeriodAllocation/deletepages", MdeleteId).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully!');
                        }
                        $scope.BindData();
                    })
                    //  $scope.BindData();
                }
                else {
                    swal("Record Deletion Cancelled");
                    $scope.BindData();
                }
            });
        };


        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmP_ActiveFlag === true) {
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

                apiService.create("PeriodAllocation/deactivate", employee).
                then(function (promise) {
                    if (promise.returnduplicatestatus == "active") {
                        swal("Already Period is active !!");
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "successfully.");
                        }
                        else {
                            swal(confirmmgs + " " + " successfully");
                        }
                        $scope.BindData();
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.deactive1 = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmpC_ActiveFlag === true) {
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

                apiService.create("PeriodAllocation/deactivate1", employee).
                then(function (promise) {
                    if (promise.returnduplicatestatus == "active") {

                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "successfully.");
                        }
                        else {
                            swal(confirmmgs + " " + " successfully");
                        }
                        $scope.BindData();
                    }
                    else {
                        swal("Master Period is deactivated !!!");
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }


        $scope.interacted1 = function (field) {

            return $scope.submitted1 || field.$dirty;
        };

        $scope.interacted2 = function (field) {

            return $scope.submitted2 || field.$dirty;
        };
        $scope.cance = function () {
            $state.reload();
            $scope.BindData();
        }

    }

})();