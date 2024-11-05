(function () {
    'use strict';
    angular.module('app').controller('BranchChangeController', BranchChangeController);
    BranchChangeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];
    function BranchChangeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.ACSCOB_COBDate = new Date();
        $scope.max_ACSCOB_COBDate = new Date();


        $scope.itemsPerPage = paginationformasters;

        $scope.BindData = function () {
            $scope.currentPage = 1;
            // $scope.itemsPerPage = 5;
            $scope.search = "";
            apiService.getDATA("BranchChange/getalldetails").then(function (promise) {
                $scope.acdlist = promise.yearlist;
                $scope.acdlistnew = promise.yearlist;
                $scope.seclist = promise.sectionslist;
                $scope.gridOptions.data = promise.datalist;
            });
        };

        $scope.get_courses = function () {

            $scope.AMCO_Id = "";
            $scope.AMCO_Id1 = "";
            $scope.AMB_Id = "";
            $scope.ACSCOB_AMB_Id = "";
            $scope.AMSE_Id1 = "";
            $scope.AMSE_Id = "";
            $scope.acmS_Id = "";
            $scope.acmS_Id_COB = "";
            $scope.AMCST_Id = "";
            $scope.ACSCOB_OldRegNo = "";
            $scope.studentlist = [];
            if ($scope.ASMAY_Id1 !== undefined && $scope.ASMAY_Id1 !== "") {
                $scope.ASMAY_Id = $scope.ASMAY_Id1;
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id1
                };

                apiService.create("Atten_Subject_MaxPeriod/get_courses", data).then(function (promise) {
                    $scope.course_list = promise.courselist;
                    $scope.AMCO_Id = "";
                });
            }
            else {
                $scope.course_list = [];
                $scope.AMCO_Id = "";
            }
        };

        $scope.get_branches = function () {
            $scope.AMB_Id = "";
            $scope.ACSCOB_AMB_Id = "";
            $scope.AMSE_Id1 = "";
            $scope.AMSE_Id = "";
            $scope.acmS_Id = "";
            $scope.acmS_Id_COB = "";
            $scope.AMCST_Id = "";
            $scope.ACSCOB_OldRegNo = "";
            $scope.studentlist = [];
            if ($scope.AMCO_Id !== undefined && $scope.AMCO_Id !== "") {
                $scope.AMCO_Id1 = $scope.AMCO_Id;
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id1,
                    "AMCO_Id": $scope.AMCO_Id
                };

                apiService.create("Atten_Subject_MaxPeriod/get_branches", data).then(function (promise) {
                    $scope.branch_list = promise.branchlist;
                    $scope.AMB_Id = "";
                });
            }
            else {
                $scope.branch_list = [];
                $scope.AMB_Id = "";
            }
        };

        $scope.get_semisters = function () {
            $scope.AMSE_Id1 = "";
            $scope.AMSE_Id = "";
            $scope.acmS_Id = "";
            $scope.acmS_Id_COB = "";
            $scope.AMCST_Id = "";
            $scope.studentlist = [];
            $scope.ACSCOB_OldRegNo = "";
            if ($scope.AMB_Id !== undefined && $scope.AMB_Id !== "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id1,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id
                };
                apiService.create("Atten_Subject_MaxPeriod/get_semisters", data).then(function (promise) {
                    $scope.semisters_list = promise.semisterlist;
                    $scope.AMSE_Id = "";
                });
            }
            else {
                $scope.semister_list = [];
                $scope.AMSE_Id = "";
            }
        };

        $scope.getsection = function () {
            $scope.acmS_Id = "";
            $scope.acmS_Id_COB = "";
            $scope.AMCST_Id = "";
            $scope.studentlist = [];
            $scope.ACSCOB_OldRegNo = "";
        };

        $scope.checkbranch = function () {
            if ($scope.AMB_Id === $scope.ACSCOB_AMB_Id) {
                swal("Select Differnt Branch In To Branch");
                $scope.ACSCOB_AMB_Id = "";
            }
        };

        $scope.checksemester = function () {
            if ($scope.AMSE_Id1 === $scope.AMSE_Id) {
                swal("Select Differnt Semester In To Semester");
                $scope.AMSE_Id = "";
            }
        };

        $scope.onselectsem = function (asmay_id, amco_id, amb_id, amse_id) {
            $scope.studentlist = [];
            $scope.ACSCOB_OldRegNo = "";
            var data = {
                "ASMAY_Id": asmay_id,
                "AMCO_Id": amco_id,
                "AMB_Id": amb_id,
                "AMSE_Id": amse_id,
                "ACMS_Id_Old": $scope.acmS_Id
            };
            apiService.create("BranchChange/Studentdetails", data).
                then(function (promise) {
                    if (promise.studlist.length > 0) {
                        $scope.studentlist = promise.studlist;
                    }
                });
        };

        $scope.getstudentdetails = function () {
            $scope.ACSCOB_OldRegNo = "";
            angular.forEach($scope.studentlist, function (dd) {
                if (dd.amcsT_Id === parseInt($scope.AMCST_Id)) {
                    $scope.ACSCOB_OldRegNo = dd.amcsT_RegistrationNo;
                }
            });
        };

        //For Save data record 
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "ACSCOB_Id": $scope.ACSCOB_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ACSCOB_AMB_Id": $scope.ACSCOB_AMB_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ACSCOB_OldRegNo": $scope.ACSCOB_OldRegNo,
                    "ACSCOB_NewRegNo": $scope.ACSCOB_NewRegNo,
                    "ACSCOB_Remarks": $scope.ACSCOB_Remarks,
                    "ACSCOB_COBFees": $scope.ACSCOB_COBFees,
                    "AMSE_Id_Old": $scope.AMSE_Id1,
                    "AMSE_Id_New": $scope.AMSE_Id,
                    "ACMS_Id_Old": $scope.acmS_Id,
                    "ACMS_Id_New": $scope.acmS_Id_COB,
                    "ACSCOB_COBDate": new Date($scope.ACSCOB_COBDate).toDateString(),
                };
                apiService.create("BranchChange/Savedetails", data).then(function (promise) {

                    if (promise.returnval !== null) {
                        if (promise.returnval === true) {
                            swal("Data record Inserted Successfully!!!");
                        }
                        else if (promise.returnval === false) {
                            swal("Insertion Failed!!!");
                        }
                    }
                    else {
                        if (promise.returnduplicatestatus === Duplicate) {
                            swal("Record Already Exist!!!");
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNo', displayName: 'SL NO', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'sName', displayName: 'Student Name' },
                { name: 'AMCO_CourseName', displayName: 'Course Name' },
                { name: 'AMB_BranchName', displayName: 'Branch Name' },
                { name: 'NewBranch', displayName: 'NewBranch' },
                { name: 'ACSCOB_COBFees', displayName: 'COB Fees' },
                { name: 'ACSCOB_OldRegNo', displayName: 'OldRegNo' },
                { name: 'ACSCOB_NewRegNo', displayName: 'NewRegNo' },
                { name: 'ACSCOB_Remarks', displayName: 'Remarks' }
                //{
                //    field: 'id', name: '',
                //    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                //        '<div class="grid-action-cell">' +
                //        '<a ng-if="row.entity.ACSCOB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                //        '<span ng-if="row.entity.ACSCOB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                //        '</div>'
                //}
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };


        //For Cancel data record 
        $scope.Cancel = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        //For Delete data record
        $scope.deactive = function (item, SweetAlert) {
            $scope.ACSCOB_Id = item.ACSCOB_Id;
            var dystring = "";
            if (item.ACSCOB_ActiveFlag === 1) {
                dystring = "Deactivate";
            }
            else if (item.ACSCOB_ActiveFlag === 0) {
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
                        apiService.create("BranchChange/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {

                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {

                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };



        $scope.resultClick = function () {
            $scope.AMCO_Id = item.amcO_Id;
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        //Set Order

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };


        $scope.init = function () {
            $scope.resetLists();
        };
        $scope.init();


        $scope.getOrder = function (orderarray) {
            var data = {
                coursedto: orderarray
            };
            apiService.create("MasterCourse/getOrder", data).
                then(function (promise) {
                    if (promise.retrunMsg !== "" && promise.retrunMsg !== undefined && promise.retrunMsg !== null) {
                        swal(promise.retrunMsg);
                    }
                    $state.reload();
                });

        };


    }
})();
