
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGConsecutiveController', CLGConsecutiveController)

    CLGConsecutiveController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function CLGConsecutiveController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
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
                { name: 'ASMAY_Year', displayName: 'Academic Year' },
                { name: 'TTMC_CategoryName', displayName: 'Category' },
                { name: 'AMCO_CourseName', displayName: 'Course' },
                { name: 'AMB_BranchName', displayName: 'Branch' },
                { name: 'AMSE_SEMName', displayName: 'Sem' },
                { name: 'ACMS_SectionName', displayName: 'Section' },
                { name: 'ENAME', displayName: 'Staff' },
                { name: 'ISMS_SubjectName', displayName: 'Subject' },
                { name: 'TTCC_NoOfPeriods', displayName: 'No. of Period' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                  '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.TTCC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.TTCC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

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

        $scope.yearchange = function () {
            $scope.AMB_Id = '';
            $scope.TTMC_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.HRME_Id = '';
            $scope.ISMS_Id = '';

        }
        $scope.objtype = {};
        $scope.get_course = function () {

            $scope.AMB_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.HRME_Id = '';
            $scope.ISMS_Id = '';
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

        $scope.get_staff = function () {
            $scope.ISMS_Id = '';
            $scope.HRME_Id = '';
            if ($scope.ASMAY_Id === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,

                }
                apiService.create("CLGTTCommon/get_staff", data).
                    then(function (promise) {

                        $scope.stafflist = promise.stafflist;

                        if (promise.stafflist == "" || promise.stafflist == null) {
                            swal("Staff are not mapped with selected parameter");
                        }
                    })
            }
        };


        $scope.get_subject = function () {
            $scope.ISMS_Id = '';
            if ($scope.ASMAY_Id === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "HRME_Id": $scope.HRME_Id,

                }
                apiService.create("CLGTTCommon/get_subject", data).
                    then(function (promise) {

                        $scope.subjectlist = promise.subjectlist;

                        if (promise.subjectlist == "" || promise.subjectlist == null) {
                            swal("Subjects are not mapped for selected parameter");
                        }
                    })
            }
        };
        $scope.getbranch_catg = function () {
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.HRME_Id = '';
            $scope.ISMS_Id = '';
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

            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.HRME_Id = '';
            $scope.ISMS_Id = '';
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

     
        //
        $scope.get_course_1 = function (qq) {

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
                        debugger;
                        $scope.courselist = promise.courselist;
                        $scope.AMCO_Id = qq;
                        if (promise.courselist == "" || promise.courselist == null) {
                            swal("No Course/Branch Are Mapped To Selected Category");
                        }
                    })
            }
        };
        //



        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            debugger;
            if ($scope.myForm.$valid) {
                if ($scope.ttC_NoOfPeriods != "") {

                    if ($scope.ttC_NoOfPeriods === ($scope.ttC_NoOfConPeriods * $scope.ttC_NoOfConDays)) {
                        if ($scope.ttC_BefAftApplFlag === "0" || $scope.ttC_BefAftApplFlag === "" || $scope.ttC_BefAftApplFlag === 0) {
                            $scope.ttC_BefAftFalg = 0;
                            $scope.ttC_BefAftPeriod = 0;
                            $scope.ttC_BefAftApplFlag = 0;
                        }

                        var data = {
                            "TTCC_Id": $scope.TTCC_Id,
                            "TTMC_Id": $scope.TTMC_Id,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "AMCO_Id": $scope.AMCO_Id,
                            "AMB_Id": $scope.AMB_Id,
                            "AMSE_Id": $scope.AMSE_Id,
                            "ACMS_Id": $scope.ACMS_Id,
                            "HRME_Id": $scope.HRME_Id,
                            "ISMS_Id": $scope.ISMS_Id,
                            "TTCC_NoOfPeriods": $scope.ttC_NoOfPeriods,
                            "TTCC_AllotPeriods": $scope.ttC_NoOfPeriods,
                            "TTCC_RemPeriods": $scope.ttC_NoOfPeriods,
                            "TTCC_NoOfConPeriods": $scope.ttC_NoOfConPeriods,
                            "TTCC_NoOfConDays": $scope.ttC_NoOfConDays,
                            "TTCC_BefAftApplFlag": $scope.ttC_BefAftApplFlag,
                            "TTCC_BefAftFalg": $scope.ttC_BefAftFalg,
                            "TTCC_BefAftPeriod": $scope.ttC_BefAftPeriod,
                            "TTCC_AllotedFlag": "N",
                        }
                        apiService.create("CLGConsecutive/savedetail", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal('Data Saved/Updated Successfully ');
                                    
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
                        swal('Please Check No Of Consecutive Periods !');
                    }

                }
                else {
                    swal('Please Check No Of Consecutive Periods !');
                }
            }


        };



        $scope.cancel11 = function () {
            $state.reload();
        }
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("CLGConsecutive/getalldetails", pageid).
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylist = promise.catelist;
           $scope.sectionlist = promise.sectionlist;
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

            apiService.create("CLGConsecutive/editconv", employee).
                then(function (promise) {
                    $scope.courselist = promise.courselist;
                    $scope.semisterlist = promise.semisterlist;
                    $scope.branchlist = promise.branchlist;
                    $scope.stafflist = promise.stafflist;
                    $scope.subjectlist = promise.subjectlist;
                    $scope.TTCC_Id = promise.consecutivelstedit[0].ttcC_Id;
                $scope.TTMC_Id = promise.consecutivelstedit[0].ttmC_Id;
                $scope.ASMAY_Id = promise.consecutivelstedit[0].asmaY_Id;
              
                    $scope.AMCO_Id = promise.consecutivelstedit[0].amcO_Id;
                    $scope.AMB_Id = promise.consecutivelstedit[0].amB_Id;
                    $scope.AMSE_Id = promise.consecutivelstedit[0].amsE_Id;
                    $scope.ACMS_Id = promise.consecutivelstedit[0].acmS_Id;
                    $scope.HRME_Id = promise.consecutivelstedit[0].hrmE_Id;
                    $scope.ISMS_Id = promise.consecutivelstedit[0].ismS_Id;
           
                $scope.ttC_NoOfPeriods = promise.consecutivelstedit[0].ttcC_NoOfPeriods;
                $scope.ttC_NoOfConPeriods = promise.consecutivelstedit[0].ttcC_NoOfConPeriods;
                $scope.ttC_NoOfConDays = promise.consecutivelstedit[0].ttcC_NoOfConDays;
                if (promise.consecutivelstedit[0].ttcC_BefAftApplFlag === 0) {
                    $scope.ttC_BefAftApplFlag = promise.consecutivelstedit[0].ttcC_BefAftApplFlag;
                }
                else {
                    $scope.ttC_BefAftApplFlag = promise.consecutivelstedit[0].ttcC_BefAftApplFlag;
                    $scope.ttC_BefAftFalg = promise.consecutivelstedit[0].ttcC_BefAftFalg;
                    $scope.ttC_BefAftPeriod = promise.consecutivelstedit[0].ttcC_BefAftPeriod;
                }
               
               
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
                    apiService.DeleteURI("CLGConsecutive/deletepages", pageid).
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

    
        $scope.deactive = function (employee, SweetAlert) {
            debugger;
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.TTCC_ActiveFlag === true) {
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

                apiService.create("CLGConsecutive/deactivate", employee).
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
                apiService.create("CLGConsecutive/get_catg", data).
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
                apiService.create("CLGConsecutive/searchfilter", data).
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