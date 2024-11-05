
(function () {
    'use strict';
    angular
.module('app')
.controller('ConsecutiveController', ConsecutiveController)

    ConsecutiveController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function ConsecutiveController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
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

        // Time picker end here

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.staffNamelst).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }


        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                     { name: 'asmayYear', displayName: 'Academic Year' },
              { name: 'categoryName', displayName: 'Category' },
            { name: 'className', displayName: 'Class' },
            { name: 'sectionName', displayName: 'Section' },
             { name: 'staffName', displayName: 'Staff' },
               { name: 'subjectName', displayName: 'Subject' },
            { name: 'noOfPeriods', displayName: 'NoofPeriod' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                  '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]
        };
        $scope.calculatetotal = function () {
            var period = Number($scope.ttC_NoOfConPeriods);
            var day = Number($scope.ttC_NoOfConDays);
            if (Number($scope.ttC_NoOfConPeriods) != "" && Number($scope.ttC_NoOfConDays) != "") {
                $scope.ttC_NoOfPeriods = Number($scope.ttC_NoOfConPeriods) * Number($scope.ttC_NoOfConDays);
            }
            else {
                $scope.ttC_NoOfPeriods = "";
            }

        }

        $scope.objtype = {};


        $scope.get_staff1 = function () {
            if ($scope.asmaY_Id == "" && $scope.ttmC_Id == "" && $scope.asmcL_Id == "") {
                swal("Please Select The Academic Year And Category And Class !");
            }
            else if ($scope.asmaY_Id != "" && $scope.ttmC_Id != "" && $scope.asmcL_Id != "" && $scope.asmS_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMC_Id": $scope.ttmC_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                }
                apiService.create("Fixing/getstaff_section", data).
         then(function (promise) {
             
             $scope.hrmE_Id1 = "";
             if (promise.staffbyall == "" || promise.staffbyall == null) {
                 swal("No Staff Are Allocated To Selected Class and Section");
             }
             else {
                 $scope.staff_list1 = promise.staffbyall;               
                 $scope.maxvalue = promise.maxvalue;
             }
         })
            }
        }

        $scope.get_subject1 = function () {
            
            if ($scope.asmaY_Id == "" && $scope.ttmC_Id == "" && $scope.asmcL_Id == "" && $scope.asmS_Id == "") {
                swal("Please Select The Academic Year,Category,Class And Section !");
            }
            else if ($scope.asmaY_Id != "" && $scope.ttmC_Id != "" && $scope.asmcL_Id != "" && $scope.asmS_Id != "" && $scope.hrmE_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMC_Id": $scope.ttmC_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "HRME_Id": $scope.hrmE_Id,
                }
                apiService.create("Fixing/getsubject_staff", data).
         then(function (promise) {

             $scope.ismS_Id1 = "";

             if (promise.subjectbystaff == "" || promise.subjectbystaff == null) {
                 swal("No subject Are Mapped To Selected Staff");
             }
             else {
                 $scope.sublst = promise.subjectbystaff;
             }
         })
            }
        }


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                if ($scope.ttC_NoOfPeriods != "") {

                    if ($scope.ttC_NoOfPeriods === ($scope.ttC_NoOfConPeriods * $scope.ttC_NoOfConDays)) {
                        if ($scope.ttC_BefAftApplFlag === "0" || $scope.ttC_BefAftApplFlag === "" || $scope.ttC_BefAftApplFlag === 0) {
                            $scope.ttC_BefAftFalg = 0;
                            $scope.ttC_BefAftPeriod = 0;
                            $scope.ttC_BefAftApplFlag = 0;
                        }

                        var data = {
                            "TTC_Id": $scope.TTC_Id,
                            "TTMC_Id": $scope.ttmC_Id,
                            "ASMAY_Id": $scope.asmaY_Id,
                            "ASMCL_Id": $scope.asmcL_Id,
                            "ASMS_Id": $scope.asmS_Id,
                            "HRME_Id": $scope.hrmE_Id,
                            "ISMS_Id": $scope.ismS_Id,
                            "TTC_NoOfPeriods": $scope.ttC_NoOfPeriods,
                            "TTC_AllotPeriods": $scope.ttC_NoOfPeriods,
                            "TTC_RemPeriods": $scope.ttC_NoOfPeriods,
                            "TTC_NoOfConPeriods": $scope.ttC_NoOfConPeriods,
                            "TTC_NoOfConDays": $scope.ttC_NoOfConDays,
                            "TTC_BefAftApplFlag": $scope.ttC_BefAftApplFlag,
                            "TTC_BefAftFalg": $scope.ttC_BefAftFalg,
                            "TTC_BefAftPeriod": $scope.ttC_BefAftPeriod,
                            "TTC_AllotedFlag": "N",
                        }
                        apiService.create("Consecutive/savedetail", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal('Data successfully Saved');
                                    $scope.BindData();
                                    $scope.clearid();
                                }
                                else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                                    swal('Records Already Exist !');
                                }
                                else {
                                    swal('Data Not Saved !');
                                }
                                //  $scope.BindData();
                            })

                    }
                    else {
                        swal('Please Check No Of Consecutive Periods !');
                    }

                }
                else {
                    swal('Please Check No Of Consecutive Periods !');
                }
            }


        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("Consecutive/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylst = promise.catelist;
           $scope.arrlist2 = promise.classDrpDwn;
           $scope.section = promise.sectDrpDwn;
           $scope.stflst = promise.staffDrpDwn;
           $scope.sublst = promise.subjDrpDwn;
           $scope.gridOptions.data = promise.consecutivelst;

       })
        };
        //TO clear  data
        $scope.clearid = function () {
            $scope.TTC_Id = 0;
            $scope.ttmC_Id = "";
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.ismS_Id = "";
            $scope.ttC_NoOfPeriods = "";
            $scope.ttC_NoOfConPeriods = "";
            $scope.ttC_NoOfConDays = "";
            $scope.ttC_BefAftApplFlag = "";
            $scope.ttC_BefAftFalg = "";
            $scope.ttC_BefAftPeriod = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.hrmE_Id = "";

        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            
            $scope.editEmployee = employee.ttC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Consecutive/getdetails", pageid).
            then(function (promise) {

                $scope.TTC_Id = promise.consecutivelstedit[0].ttC_Id;
                $scope.ttmC_Id = promise.consecutivelstedit[0].ttmC_Id;
                $scope.asmaY_Id = promise.consecutivelstedit[0].asmaY_Id;
                $scope.asmcL_Id = promise.consecutivelstedit[0].asmcL_Id;
                $scope.asmS_Id = promise.consecutivelstedit[0].asmS_Id;
                $scope.ismS_Id = promise.consecutivelstedit[0].ismS_Id;
                $scope.ttC_NoOfPeriods = promise.consecutivelstedit[0].ttC_NoOfPeriods;
                $scope.ttC_NoOfConPeriods = promise.consecutivelstedit[0].ttC_NoOfConPeriods;
                $scope.ttC_NoOfConDays = promise.consecutivelstedit[0].ttC_NoOfConDays;
                if (promise.consecutivelstedit[0].ttC_BefAftApplFlag === 0) {
                    $scope.ttC_BefAftApplFlag = promise.consecutivelstedit[0].ttC_BefAftApplFlag;
                }
                else {
                    $scope.ttC_BefAftApplFlag = promise.consecutivelstedit[0].ttC_BefAftApplFlag;
                    $scope.ttC_BefAftFalg = promise.consecutivelstedit[0].ttC_BefAftFalg;
                    $scope.ttC_BefAftPeriod = promise.consecutivelstedit[0].ttC_BefAftPeriod;
                }
                $scope.get_staff1();
                $scope.hrmE_Id = promise.consecutivelstedit[0].hrmE_Id;
            })
        }

        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttC_Id;
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
                    apiService.DeleteURI("Consecutive/deletepages", pageid).
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

        $scope.get_class = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {
                if ($scope.ttmC_Id === "" || $scope.ttmC_Id === undefined) {
                }
                else {
                    var data = {
                        "TTMC_Id": $scope.ttmC_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                    }
                    apiService.create("Consecutive/getclass_catg", data).
             then(function (promise) {
                 $scope.asmcL_Id = "";
                 if (promise.classbycategory == "" || promise.classbycategory == null) {
                     swal("No classes Are Mapped To Selected Category");
                 }
                 else {
                     $scope.arrlist2 = promise.classbycategory;
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
            if (employee.ttC_ActiveFlag === true) {
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

                apiService.create("Consecutive/deactivate", employee).
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
                apiService.create("Consecutive/get_catg", data).
         then(function (promise) {

             $scope.categorylst = promise.catelist;

             if (promise.catelist == "" || promise.catelist == null) {
                 swal("No Category Are Mapped To Selected Academic Year");
             }
         })

            }


        };



        $scope.searchfilterqq = function (objj) {
            
            if (objj.search.length >= '2') {
                var data = {
                    "searchfilter": objj.search,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                }
                apiService.create("Consecutive/searchfilter", data).
            then(function (promise) {
                if (promise.staffDrpDwn > 0) {
                    $scope.stflst = promise.staffDrpDwn;
                } else {
                    $scope.hrmE_Id = "";
                    swal("No staff are found for your search");
                }
            })
            }

        };


        $scope.cal = function () {
           



        }
    }

})();