(function () {
    'use strict';
    angular.module('app').controller('MasterExamGradeController', MasterExamGradeController)

    MasterExamGradeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function MasterExamGradeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNo', displayName: 'SL NO', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'emgR_GradeName', displayName: 'Grade Name' },
                { name: 'emgR_MarksPerFlag', displayName: 'Percentage / Marks' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.emgR_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.emgR_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        $scope.EMGD_From = "";
        $scope.EMGD_To = "";
        $scope.EMGD_Name = "";
        $scope.EMGD_GradePoints = "";
        $scope.EMGD_Remarks = "";
        $scope.EMGR_MarksPerFlag = "P";
        $scope.BindData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            $scope.rows = [];
            $scope.rows.push({
                count: $scope.rows.length, EMGD_From: $scope.EMGD_From, EMGD_To: $scope.EMGD_To,
                EMGD_Name: $scope.EMGD_Name, EMGD_GradePoints: $scope.EMGD_GradePoints, EMGD_Remarks: $scope.EMGD_Remarks
            });

            apiService.getDATA("MasterExamGrade/getalldetails").then(function (promise) {

                angular.forEach(promise.grade_list, function (opq) {
                    if (opq.emgR_MarksPerFlag == 'M') {
                        opq.emgR_MarksPerFlag = 'Marks';
                    }
                    else if (opq.emgR_MarksPerFlag == 'P') {
                        opq.emgR_MarksPerFlag = 'Percentage';
                    }
                });
                $scope.gridOptions.data = promise.grade_list;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.submitted2 = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var array = $scope.rows;
                var data = {
                    "EMGR_Id": $scope.EMGR_Id,
                    "EMGD_Id": $scope.EMGD_Id,
                    "EMGR_GradeName": $scope.EMGR_GradeName,
                    "EMGR_MarksPerFlag": $scope.EMGR_MarksPerFlag,
                    sub_list: $scope.rows,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterExamGrade/savedetail", data).then(function (promise) {
                    angular.forEach(promise.grade_list, function (opq) {
                        if (opq.emgR_MarksPerFlag == 'M') {
                            opq.emgR_MarksPerFlag = 'Marks';
                        }
                        else if (opq.emgR_MarksPerFlag == 'P') {
                            opq.emgR_MarksPerFlag = 'Percentage';
                        }
                    });

                    $scope.gridOptions.data = promise.grade_list;
                    if (promise.returnval === true) {
                        if (promise.emgR_Id == 0 || promise.emgR_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.emgR_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.emgR_Id == 0 || promise.emgR_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.emgR_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.BindData();
                    $scope.clear();
                });
            }
            else {
                $scope.submitted2 = true;
            }
        }

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.emgR_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterExamGrade/getdetails", pageid).then(function (promise) {
                $scope.EMGR_Id = promise.edit_m_grade[0].emgR_Id;
                $scope.EMGR_GradeName = promise.edit_m_grade[0].emgR_GradeName;
                $scope.EMGR_MarksPerFlag = promise.edit_m_grade[0].emgR_MarksPerFlag;
                $scope.rows = [];

                if (promise.edit_m_grade_details.length > 0) {
                    for (var i = 0; i < promise.edit_m_grade_details.length; i++) {
                        $scope.rows.push({ count: i, EMGD_From: promise.edit_m_grade_details[i].emgD_From, EMGD_To: promise.edit_m_grade_details[i].emgD_To, EMGD_Name: promise.edit_m_grade_details[i].emgD_Name, EMGD_GradePoints: promise.edit_m_grade_details[i].emgD_GradePoints, EMGD_Remarks: promise.edit_m_grade_details[i].emgD_Remarks, EMGD_Id: promise.edit_m_grade_details[i].emgD_Id  });
                    }
                }

                else if (promise.edit_m_grade_details.length == 0) {
                    swal("This Grade Don't Have Details");
                    $scope.rows.push({ count: $scope.rows.length, EMGD_From: $scope.EMGD_From, EMGD_To: $scope.EMGD_To, EMGD_Name: $scope.EMGD_Name, EMGD_GradePoints: $scope.EMGD_GradePoints, EMGD_Remarks: $scope.EMGD_Remarks, EMGD_Id: 0 });
                }
            });
        }

        $scope.deactive = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.emgR_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
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
                        apiService.create("MasterExamGrade/deactivate", employee).then(function (promise) {
                            if (promise.already_cnt == true) {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.clear();
                            $scope.BindData();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.viewrecordspopup = function (employee, SweetAlert) {

            $scope.editEmployee = employee.emgR_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterExamGrade/getalldetailsviewrecords", pageid).then(function (promise) {
                $scope.Grade_Name = promise.emgR_GradeName;
                $scope.viewrecordspopupdisplay = promise.view_grade_details;
            });
        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };


        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.totalgridtest = [];
        $scope.addNew = function (totalgrid, index) {
            if ($scope.myForm.$valid) {
                $scope.rows.push({ count: totalgrid.length, EMGD_From: totalgrid.EMGD_From, EMGD_To: totalgrid.EMGD_To, EMGD_Name: totalgrid.EMGD_Name, EMGD_GradePoints: totalgrid.EMGD_GradePoints, EMGD_Remarks: totalgrid.EMGD_Remarks });
            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.removerow = function (totalgrid, index) {
            if (totalgrid.length > 1) {
                $scope.rows.splice(index, 1);
            }
        };

        $scope.valid_from = function (from) {
            if (from.EMGD_From != null && from.EMGD_From != "" && from.EMGD_From != undefined) {
                var num_from = Number(from.EMGD_From);
                angular.forEach($scope.rows, function (itm1) {
                    if (itm1.count == from.count) {
                        itm1.EMGD_From = num_from.toFixed(2);
                        num_from = Number(from.EMGD_From);
                        console.log(num_from.toFixed(2));
                    }
                });

                var max_from = 0;
                if ($scope.EMGR_MarksPerFlag == 'P') {
                    max_from = 100;
                }
                else if ($scope.EMGR_MarksPerFlag == 'M') {
                    max_from = 1000;
                }
                if (num_from > max_from) {
                    if ($scope.EMGR_MarksPerFlag == 'P') {
                        swal("Max Value Is 100");
                        angular.forEach($scope.rows, function (itm1) {
                            if (itm1.count == from.count) {
                                itm1.EMGD_From = "";
                            }
                        });
                    }
                    else if ($scope.EMGR_MarksPerFlag == 'M') {
                        if (num_from > 1000) {
                            swal("Max Value Is 1000");
                            angular.forEach($scope.rows, function (itm1) {
                                if (itm1.count == from.count) {
                                    itm1.EMGD_From = "";
                                }
                            });
                        }
                    }
                }
                else {
                    var dupli_count = 0;
                    angular.forEach($scope.rows, function (itm1) {
                        if (itm1.count != from.count) {
                            var num_from_all = Number(itm1.EMGD_From);
                            var num_to_all = Number(itm1.EMGD_To);
                            if (num_from_all > num_to_all) {
                                if ((num_from <= num_from_all && num_from >= num_to_all) || num_from == num_from_all || num_from == num_to_all) {
                                    dupli_count += 1;
                                }
                            }
                            else if (num_from_all < num_to_all) {
                                if ((num_from >= num_from_all && num_from <= num_to_all) || num_from == num_from_all || num_from == num_to_all) {
                                    dupli_count += 1;
                                }
                            }
                        }
                        else if (itm1.count == from.count) {
                            if (itm1.EMGD_To != null && itm1.EMGD_To != undefined && itm1.EMGD_To != "") {
                                var num_to_all = Number(itm1.EMGD_To);
                                if (num_from == num_to_all) {
                                    dupli_count += 1;
                                }
                            }
                        }
                    })
                    if (dupli_count > 0) {
                        swal("Value Is Already Existed In Other Details");
                        angular.forEach($scope.rows, function (itm1) {
                            if (itm1.count == from.count) {
                                itm1.EMGD_From = "";
                            }
                        })
                    }
                    else if (dupli_count == 0) {
                        if (from.EMGD_To != null && from.EMGD_To != undefined && from.EMGD_To != "") {
                            if ($scope.rows.length > 1) {
                                var sub_count = 0;
                                var num_to = Number(from.EMGD_To);
                                angular.forEach($scope.rows, function (itm1) {
                                    if (from.count != itm1.count) {
                                        var num_from_all1 = Number(itm1.EMGD_From);
                                        var num_to_all1 = Number(itm1.EMGD_To);

                                        if (num_from > num_to) {
                                            if (num_from_all1 <= num_from && num_to_all1 <= num_from && num_from_all1 >= num_to && num_to_all1 >= num_to) {
                                                sub_count += 1;
                                            }
                                        }
                                        else if (num_from < num_to) {
                                            if (num_from_all1 <= num_to && num_to_all1 <= num_to && num_from_all1 >= num_from && num_to_all1 >= num_from) {
                                                sub_count += 1;
                                            }
                                        }
                                    }
                                });

                                if (sub_count > 0) {
                                    swal("Entered Details Are Sub Values Of Existed One !!!!");
                                    angular.forEach($scope.rows, function (itm1) {
                                        if (itm1.count == from.count) {
                                            itm1.EMGD_From = "";
                                        }
                                    })
                                }
                            }
                        }
                    }
                }
            }
            else {
                if ($scope.myForm.$valid) {
                    // if ($scope.myForm.myForm2.$valid) {
                }
                else {
                    $scope.submitted2 = true;
                }
            }
        };

        $scope.valid_to = function (to) {
            if (to.EMGD_To != null && to.EMGD_To != "" && to.EMGD_To != undefined) {
                var num_to = Number(to.EMGD_To);

                angular.forEach($scope.rows, function (itm1) {
                    if (itm1.count == to.count) {
                        itm1.EMGD_To = num_to.toFixed(2);
                        num_to = Number(to.EMGD_To);
                        console.log(num_to.toFixed(2));
                    }
                });

                var max_to = 0;
                if ($scope.EMGR_MarksPerFlag == 'P') {
                    max_to = 100;
                }
                else if ($scope.EMGR_MarksPerFlag == 'M') {
                    max_to = 1000;
                }
                if (num_to > max_to) {
                    if ($scope.EMGR_MarksPerFlag == 'P') {
                        swal("Max Value Is 100");
                        angular.forEach($scope.rows, function (itm1) {
                            if (itm1.count == to.count) {
                                itm1.EMGD_To = "";
                            }
                        });
                    }
                    else if ($scope.EMGR_MarksPerFlag == 'M') {
                        if (num_to > 1000) {
                            swal("Max Value Is 1000");
                            angular.forEach($scope.rows, function (itm1) {
                                if (itm1.count == to.count) {
                                    itm1.EMGD_To = "";
                                }
                            })
                        }
                    }
                }
                else {
                    var dupli_count = 0;
                    angular.forEach($scope.rows, function (itm1) {
                        if (itm1.count != to.count) {
                            var num_from_all = Number(itm1.EMGD_From);
                            var num_to_all = Number(itm1.EMGD_To);
                            if (num_from_all > num_to_all) {
                                if ((num_to <= num_from_all && num_to >= num_to_all) || num_to == num_from_all || num_to == num_to_all) {
                                    dupli_count += 1;
                                }
                            }
                            else if (num_from_all < num_to_all) {
                                if ((num_to >= num_from_all && num_to <= num_to_all) || num_to == num_from_all || num_to == num_to_all) {
                                    dupli_count += 1;
                                }
                            }

                        }
                        else if (itm1.count == to.count) {

                            if (itm1.EMGD_From != null && itm1.EMGD_From != undefined && itm1.EMGD_From != "") {
                                var num_from_all = Number(itm1.EMGD_From);
                                if (num_to == num_from_all) {
                                    dupli_count += 1;
                                }
                            }
                        }
                    });

                    if (dupli_count > 0) {
                        swal("Value Is Already Existed In Other Details");
                        angular.forEach($scope.rows, function (itm1) {
                            if (itm1.count == to.count) {
                                itm1.EMGD_To = "";
                            }
                        })
                    }

                    else if (dupli_count == 0) {

                        if (to.EMGD_To != null && to.EMGD_To != undefined && to.EMGD_To != "") {
                            if ($scope.rows.length > 1) {
                                var sub_count = 0;
                                var num_from = Number(to.EMGD_From);
                                angular.forEach($scope.rows, function (itm1) {
                                    if (to.count != itm1.count) {
                                        var num_from_all1 = Number(itm1.EMGD_From);
                                        var num_to_all1 = Number(itm1.EMGD_To);

                                        if (num_from > num_to) {
                                            if (num_from_all1 <= num_from && num_to_all1 <= num_from && num_from_all1 >= num_to && num_to_all1 >= num_to) {
                                                sub_count += 1;
                                            }
                                        }
                                        else if (num_from < num_to) {
                                            if (num_from_all1 <= num_to && num_to_all1 <= num_to && num_from_all1 >= num_from && num_to_all1 >= num_from) {
                                                sub_count += 1;
                                            }
                                        }
                                    }
                                });

                                if (sub_count > 0) {
                                    swal("Entered Details Are Sub Values Of Existed One !!!!");
                                    angular.forEach($scope.rows, function (itm1) {
                                        if (itm1.count == to.count) {
                                            itm1.EMGD_To = "";
                                        }
                                    })
                                }
                            }
                        }
                    }
                }
            }
            else {

                if ($scope.myForm.$valid) {
                    // if ($scope.myForm.myForm2.$valid) {

                }
                else {
                    $scope.submitted2 = true;
                }
            }
        };

        $scope.valid_point = function (point) {
            var num_point = Number(point.EMGD_GradePoints);
            if (num_point > 10) {
                swal("Max Value Is 10");
                angular.forEach($scope.rows, function (itm1) {
                    if (itm1.count == point.count) {
                        itm1.EMGD_GradePoints = "";
                    }
                })
                //$scope.EMGD_From = "";
            }
            var gradepoint_cnt = 0;
            if ($scope.rows.length > 1) {
                angular.forEach($scope.rows, function (itn) {
                    if (itn.count != point.count) {
                        if (Number(itn.EMGD_GradePoints) == num_point) {
                            // swal("Duplicate Grade !!");
                            gradepoint_cnt += 1;
                        }
                    }

                })
                if (gradepoint_cnt > 0) {
                    swal("Duplicate Gradepoint !!!");
                    angular.forEach($scope.rows, function (itn) {
                        if (itn.count == point.count) {
                            itn.EMGD_GradePoints = "";
                        }
                    })
                }
            }
        };

        $scope.clear = function () {

            // $scope.TTLAB_LABLIBName = "";
            // $scope.popup = false;
            $scope.EMGR_Id = 0;
            $scope.EMGD_Id = 0;
            $scope.EMGR_GradeName = null;
            $scope.EMGD_From = "";
            $scope.EMGD_To = "";
            $scope.EMGD_Name = "";
            $scope.EMGD_GradePoints = "";
            $scope.EMGD_Remarks = "";
            $scope.EMGR_MarksPerFlag = "P";
            $scope.rows = [];
            $scope.rows.push({ count: $scope.rows.length, EMGD_From: $scope.EMGD_From, EMGD_To: $scope.EMGD_To, EMGD_Name: $scope.EMGD_Name, EMGD_GradePoints: $scope.EMGD_GradePoints, EMGD_Remarks: $scope.EMGD_Remarks });
            $scope.submitted2 = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
        };

        $scope.valid_grade = function (grade) {
            if (grade.EMGD_Name != "") {
                if (grade.EMGD_Name.toUpperCase() == 'AB' || grade.EMGD_Name.toUpperCase() == 'M' || grade.EMGD_Name.toUpperCase() == 'L') {
                    swal("AB, L, M Are Not Allowed For Grade Name");
                    grade.EMGD_Name = "";
                }
                else {
                    var grade_cnt = 0;
                    if ($scope.rows.length > 1) {
                        angular.forEach($scope.rows, function (itn) {
                            if (itn.count != grade.count) {
                                if (itn.EMGD_Name != "" && itn.EMGD_Name != undefined) {
                                    if (itn.EMGD_Name.toUpperCase() == grade.EMGD_Name.toUpperCase()) {
                                        grade_cnt += 1;
                                    }
                                }
                            }
                        });

                        if (grade_cnt > 0) {
                            swal("Duplicate Grade !!!");
                            angular.forEach($scope.rows, function (itn) {
                                if (itn.count == grade.count) {
                                    itn.EMGD_Name = "";
                                }
                            })
                        }
                    }
                }
            }
        };

        $scope.valid_remarks = function (remarks) {
            var remarks_cnt = 0;
            if ($scope.rows.length > 1) {
                angular.forEach($scope.rows, function (itn) {
                    if (itn.count != remarks.count) {
                        if (itn.EMGD_Remarks != "" && itn.EMGD_Remarks != undefined) {
                            if (itn.EMGD_Remarks.toUpperCase() == remarks.EMGD_Remarks.toUpperCase()) {
                                // swal("Duplicate Grade !!");
                                remarks_cnt += 1;
                            }
                        }
                    }

                })
                if (remarks_cnt > 0) {
                    swal("Duplicate Grade Remarks !!!");
                    angular.forEach($scope.rows, function (itn) {
                        if (itn.count == remarks.count) {
                            itn.EMGD_Remarks = "";
                        }
                    })
                }
            }
        };
    }
})();