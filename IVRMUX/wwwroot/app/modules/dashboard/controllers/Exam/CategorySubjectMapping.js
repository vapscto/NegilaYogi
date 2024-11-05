(function () {
    'use strict';
    angular.module('app').controller('CategorySubjectMappingController', CategorySubjectMappingController)

    CategorySubjectMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function CategorySubjectMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.EYC_BasedOnPaperTypeFlg = false;

        $scope.gridOptions = {

            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'emcA_CategoryName', displayName: 'Category Name' },
                { name: 'emG_GroupName', displayName: 'Group Name' },
                {
                    name: 'emG_ElectiveFlg', displayName: 'Elective Subjects', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.emG_ElectiveFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.emG_ElectiveFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'eyC_BasedOnPaperTypeFlg', displayName: 'Question Paper Type', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.eyC_BasedOnPaperTypeFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.eyC_BasedOnPaperTypeFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },

                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" ><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o"></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.eycG_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.eycG_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            $scope.usercheck = true;

            apiService.getDATA("CategorySubjectMapping/getalldetails").then(function (promise) {
                $scope.year_list = promise.yearlist;
                $scope.category_list = promise.categorylist;
                $scope.temp_cat_list = promise.categorylist;
                $scope.group_list = promise.grouplist;
                $scope.subject_list = promise.subjectlist;
                $scope.temp_sub_list = promise.subjectlist;
                $scope.subj_count = promise.subjectlist.length;
                $scope.gridOptions.data = promise.grid_Details_list;

                $scope.all_check();
            });
        };

        $scope.get_category = function (year) {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("CategorySubjectMapping/get_category", data).then(function (promise) {

                $scope.category_list = promise.categorylist;
                $scope.EMCA_Id = "";
                if ($scope.EYCG_Id != "" && $scope.EYCG_Id != 0) {
                    angular.forEach($scope.category_list, function (role) {

                        if (role.emcA_Id == $scope.temp_cat) {
                            $scope.EMCA_Id = role.emcA_Id;
                            role.Selected = true;
                        }
                    });
                }

                if (promise.categorylist == "" || promise.categorylist == null) {
                    swal("No Categories Are Mapped To Selected Academic Year");
                    $scope.ASMAY_Id = "";
                }
            });

        };

        $scope.get_subjects = function (group) {

            var data = {
                "EMG_Id": $scope.EMG_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("CategorySubjectMapping/get_subjects", data).then(function (promise) {

                $scope.subject_list = promise.subjectlist;
                $scope.usercheck = true;
                $scope.all_check();

                if ($scope.EYCG_Id != "" && $scope.EYCG_Id != 0 && $scope.EYCG_Id != undefined) {
                    $scope.usercheck = false;
                    angular.forEach($scope.subject_list, function (itm) {
                        itm.checked = false;
                    });
                    for (var i = 0; i < $scope.temp_sub.length; i++) {
                        angular.forEach($scope.subject_list, function (itm) {
                            if (itm.ismS_Id == $scope.temp_sub[i].ismS_Id) {
                                itm.checked = true;
                                $scope.togchkbx();
                            }
                        });
                    }
                }

                if (promise.subjectlist == "" || promise.subjectlist == null) {
                    swal("No Subjects Are Mapped To Selected Group");
                }
            });

        };

        $scope.valid_cat = function () {
            if ($scope.ASMAY_Id == null || $scope.ASMAY_Id == "" || $scope.ASMAY_Id == undefined) {
                swal("First Select Academic Year !!");
                $scope.EMCA_Id = "";
            }
        };

        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.subject_list, function (itm) {
                itm.checked = toggleStatus;
            });
        };

        $scope.togchkbx = function () {

            $scope.usercheck = $scope.subject_list.every(function (options) {
                return options.checked;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        // TO Save The Data         
        $scope.submitted = false;
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {

                $scope.array = [];

                var chk_count = 0;
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.checked == true) {
                        chk_count += 1;
                        $scope.array.push(itm);
                    }
                });

                var data = {
                    "EYC_Id": $scope.EYC_Id,
                    "EYCG_Id": $scope.EYCG_Id,
                    "EYCGS_Id": $scope.EYCGS_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EMG_Id": $scope.EMG_Id,
                    "EYC_BasedOnPaperTypeFlg": $scope.EYC_BasedOnPaperTypeFlg,
                    subj_list: $scope.array
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("CategorySubjectMapping/savedetail", data).then(function (promise) {
                    $scope.gridOptions.data = promise.grid_Details_list;
                    if (promise.returnval === true) {
                        if (promise.returnMsg == "Add") {
                            swal('Record saved successfully');
                        }
                        else if (promise.returnMsg == "Update") {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.returnMsg == "Add") {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.returnMsg == "Update") {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.BindData();
                    $scope.clear();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.eycG_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("CategorySubjectMapping/getdetails", pageid).then(function (promise) {
                $scope.EYC_Id = promise.edit_m_group[0].eyC_Id;
                $scope.EYC_BasedOnPaperTypeFlg = promise.edit_m_group[0].eyC_BasedOnPaperTypeFlg === null ? false : promise.edit_m_group[0].eyC_BasedOnPaperTypeFlg ;
                $scope.EYCG_Id = promise.edit_m_group[0].eycG_Id;
                $scope.ASMAY_Id = promise.edit_m_group[0].asmaY_Id;
                $scope.EMCA_Id = promise.edit_m_group[0].emcA_Id;
                $scope.temp_cat = promise.edit_m_group[0].emcA_Id; 
                $scope.get_category($scope.ASMAY_Id);
                $scope.EMG_Id = promise.edit_m_group[0].emG_Id;
                $scope.temp_sub = promise.edit_m_group_subjects;
                $scope.get_subjects($scope.EMG_Id);
            });
        };

        $scope.deactive = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (employee.eycG_ActiveFlg === true) {
                //mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                // mgs = "Active";
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
                        apiService.create("CategorySubjectMapping/deactivate", employee).
                            then(function (promise) {
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

            $scope.editEmployee = employee.eycG_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("CategorySubjectMapping/getalldetailsviewrecords", pageid).then(function (promise) {

                $scope.Category_Name = promise.view_group_subjects[0].emcA_CategoryName;
                $scope.Group_Name = promise.view_group_subjects[0].emG_GroupName;
                $scope.viewrecordspopupdisplay = promise.view_group_subjects;
            });
        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.totalgridtest = [];
        $scope.addNew = function (totalgrid, index) {
            if ($scope.myForm.$valid) {
                $scope.rows.push({ count: totalgrid.length, EMGD_From: totalgrid.EMGD_From, EMGD_To: totalgrid.EMGD_To, EMGD_Name: totalgrid.EMGD_Name, EMGD_GradePoints: totalgrid.EMGD_GradePoints, EMGD_Remarks: totalgrid.EMGD_Remarks });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.removerow = function (totalgrid, index) {
            if (totalgrid.length > 1) {
                $scope.rows.splice(index, 1);
            }
        };

        $scope.valid_max_subs = function (tot_subjs) {
            var num_tot_subjs = Number(tot_subjs);
            if (num_tot_subjs > 0) {
                if (num_tot_subjs > $scope.subj_count) {
                    swal("U can't set Maximum Subjects More Than Available Subjects[" + $scope.subj_count + "] !!!");
                    $scope.EMG_TotSubjects = "";
                }
            }
            else if (num_tot_subjs == 0) {
                swal("Max.Subjects Not Set To Zero ");
                $scope.EMG_TotSubjects = "";
            }
            $scope.EMG_MaxAplSubjects = "";
            $scope.EMG_MinAplSubjects = "";
            $scope.EMG_BestOff = "";
            $scope.usercheck = false;
            angular.forEach($scope.subject_list, function (itm) {
                itm.subject = false;
            });
            $scope.all = false;
            angular.forEach($scope.subject_list, function (itm) {
                itm.checkedvalue = false;
            });
        };

        $scope.valid_max_apl_subs = function (tot_subjs, max_apl_subjs) {

            if (tot_subjs != null && tot_subjs != undefined && tot_subjs != "") {
                var num_tot_subjs = Number(tot_subjs);
                var num_max_apl_subjs = Number(max_apl_subjs);
                if (num_max_apl_subjs > 0) {
                    if (num_max_apl_subjs > num_tot_subjs) {
                        swal("U can't set Max Appicable Subjects More Than Max.Subjects[" + $scope.EMG_TotSubjects + "] !!!");
                        $scope.EMG_MaxAplSubjects = "";
                    }
                }
                else if (num_max_apl_subjs == 0) {
                    swal("Max Applicable Subjects Not Set To Zero ");
                    $scope.EMG_MaxAplSubjects = "";
                }
            }
            else {
                swal("First Set Max.Subjects Field !!!");
                $scope.EMG_MaxAplSubjects = "";
            }
            $scope.EMG_MinAplSubjects = "";
            $scope.EMG_BestOff = "";
        };

        $scope.valid_min_apl_subs = function (max_apl_subjs, min_apl_subjs) {
            if (max_apl_subjs != null && max_apl_subjs != undefined && max_apl_subjs != "") {
                var num_max_apl_subjs = Number(max_apl_subjs);
                var num_min_apl_subjs = Number(min_apl_subjs);
                if (num_min_apl_subjs > 0) {
                    if (num_min_apl_subjs > num_max_apl_subjs) {
                        swal("U can't set Min Appicable Subjects More Than Max Applicable Subjects[" + $scope.EMG_MaxAplSubjects + "] !!!");
                        $scope.EMG_MinAplSubjects = "";
                    }
                }
                else if (num_min_apl_subjs == 0) {
                    swal("Min Applicable Subjects Not Set To Zero ");
                    $scope.EMG_MinAplSubjects = "";
                }
            }
            else {
                swal("First Set Max Applicable Subjects Field !!!");
                $scope.EMG_MinAplSubjects = "";
            }
            $scope.EMG_BestOff = "";
            if ($scope.EMG_MinAplSubjects == $scope.EMG_MaxAplSubjects && $scope.EMG_MaxAplSubjects == $scope.EMG_TotSubjects) {
                $scope.EMG_ElectiveFlg = 0;
            }
            else {
                $scope.EMG_ElectiveFlg = 1;
            }
        };

        $scope.valid_best = function (max_apl_subjs, min_apl_subjs, best_subjs) {
            if (max_apl_subjs != null && max_apl_subjs != undefined && max_apl_subjs != "" && min_apl_subjs != null && min_apl_subjs != undefined && min_apl_subjs != "") {

                var num_max_apl_subjs = Number(max_apl_subjs);
                var num_min_apl_subjs = Number(min_apl_subjs);
                var num_best_subjs = Number(best_subjs);
                if (num_best_subjs > num_max_apl_subjs) {
                    swal("U can't set Best Off Subjects More Than Max. Applicable Subjects [" + $scope.EMG_MaxAplSubjects + "] !!!");
                    $scope.EMG_BestOff = "";
                }
                if (num_best_subjs < num_min_apl_subjs) {
                    swal("U can't set Best Off Subjects Less Than Min. Applicable Subjects [" + $scope.EMG_MinAplSubjects + "] !!!");
                    $scope.EMG_BestOff = "";
                }
            }
            else {
                swal("First Set Max and Min  Applicable Subjects Field !!!");
                $scope.EMG_BestOff = "";
            }
        };

        $scope.isOptionsRequired = function () {

            return !$scope.subject_list.some(function (options) {
                return options.checked;
            });
        };

        $scope.EMG_ElectiveFlg = 1;

        $scope.clear = function () {
            $scope.EYC_Id = 0;
            $scope.EYCG_Id = 0;
            $scope.EYCGS_Id = 0;
            $scope.ASMAY_Id = "";
            $scope.EYC_ExamStartDate = '';
            $scope.EYC_ExamEndDate = '';
            $scope.EYC_MarksEntryLastDate = '';
            $scope.EYC_MarksProcessLastDate = '';
            $scope.EYC_MarksPublishDate = '';
            $scope.category_list = $scope.temp_cat_list;
            $scope.subject_list = $scope.temp_sub_list;
            $scope.EMCA_Id = "";
            $scope.EMG_Id = "";
            $scope.usercheck = true;
            $scope.all_check();
            $scope.submitted = false;
            $scope.EYC_BasedOnPaperTypeFlg = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
        };

        $scope.toggleAll = function () {
            var tot_subjs = $scope.EMG_TotSubjects;

            var toggleStatus = $scope.all;
            if (toggleStatus == true) {
                if (tot_subjs != null && tot_subjs != undefined && tot_subjs != "") {
                    var num_tot_subjs = Number(tot_subjs);
                    if ($scope.subject_list.length <= num_tot_subjs) {

                        angular.forEach($scope.subject_list, function (itm) {
                            itm.checkedvalue = toggleStatus;
                        });
                    }
                    else {
                        swal("Selection of Subjects Not More Than Of Max.Subjects");
                        $scope.all = false;
                    }
                }
                else {
                    swal("First Set Max.Subjects Field !!!");
                    angular.forEach($scope.subject_list, function (itm) {
                        itm.checkedvalue = false;
                    });
                    $scope.all = false;
                }
            }
            else if (toggleStatus == false) {
                angular.forEach($scope.subject_list, function (itm) {
                    itm.checkedvalue = toggleStatus;
                });
            }
        };

        $scope.optionToggled = function (chk_box) {
            var tot_subjs = $scope.EMG_TotSubjects;

            if (tot_subjs != null && tot_subjs != undefined && tot_subjs != "") {

                var chk_count = 0;
                var num_tot_subjs = Number(tot_subjs);
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.checkedvalue == true) {
                        chk_count += 1;
                    }
                });
                if (chk_count <= num_tot_subjs) {
                    $scope.all = $scope.subject_list.every(function (options) {
                        return options.checkedvalue;
                    });

                }
                else {
                    swal("Selection of Subjects Not More Than Of Max.Subjects");
                    angular.forEach($scope.subject_list, function (itm) {
                        if (itm.ismS_Id == chk_box.ismS_Id) {
                            itm.checkedvalue = false;
                        }
                    });
                }

            } else {
                swal("First Set Max.Subjects Field !!!");
                angular.forEach($scope.subject_list, function (itm) {
                    itm.checkedvalue = false;
                });
                $scope.all = false;
            }
        };

        /* CATEGORY DATE MAPPING */
        $scope.OnLoadCategoryDates = function () {

            $scope.yearlist_new = [];
            $scope.categorylist_new = [];
            $scope.getdetails = [];
            $scope.ASMAY_Id = "";
            $scope.EMCA_Id = "";

            $scope.EYC_ExamEndDate = null;
            $scope.EYC_MarksEntryLastDate = null;
            $scope.EYC_MarksProcessLastDate = null;
            $scope.EYC_MarksPublishDate = null;
            $scope.EYC_ExamStartDate = null;

            apiService.getURI("CategorySubjectMapping/OnLoadCategoryDates", 2).then(function (promise) {
                if (promise !== null) {
                    $scope.yearlist_new = promise.yearlist;
                    $scope.categorylist_new = promise.categorylist;
                    $scope.getdetails = promise.getdetails;
                }
            });
        };

        $scope.get_categoryDates = function (year) {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            angular.forEach($scope.yearlist_new, function (dd) {
                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                    $scope.maxdate = new Date(dd.asmaY_To_Date);
                    $scope.mindate = new Date(dd.asmaY_From_Date);
                }
            });

            apiService.create("CategorySubjectMapping/get_categoryDates", data).then(function (promise) {
                $scope.category_list_new = promise.categorylist;
                $scope.EMCA_Id = "";
                if (promise.categorylist === "" || promise.categorylist === null) {
                    swal("No Categories Are Mapped To Selected Academic Year");
                    $scope.ASMAY_Id = "";
                }
            });
        };

        $scope.nextdate = function () {
            $scope.EYC_ExamEndDate =null;
            $scope.EYC_MarksEntryLastDate = null;
            $scope.EYC_MarksProcessLastDate = null;
            $scope.EYC_MarksPublishDate = null;
            $scope.newdate = new Date($scope.EYC_ExamStartDate);
            $scope.ret_date = $scope.newdate.setDate($scope.newdate.getDate() + 1);
            $scope.Return_Date = new Date($scope.ret_date);
        };

        $scope.enddatechange = function () {
            $scope.EYC_MarksEntryLastDate = null;
            $scope.EYC_MarksProcessLastDate = null;
            $scope.EYC_MarksPublishDate = null;
            $scope.newdate = new Date($scope.EYC_ExamEndDate);
            $scope.ret_date = $scope.newdate.setDate($scope.newdate.getDate() + 1);
            $scope.Return_Date1 = new Date($scope.ret_date);

        };

        $scope.markentrychange = function () {
            $scope.EYC_MarksProcessLastDate = null;
            $scope.EYC_MarksPublishDate = null;
            $scope.newdate = new Date($scope.EYC_MarksEntryLastDate);
            $scope.ret_date = $scope.newdate.setDate($scope.newdate.getDate() + 1);
            $scope.Return_Date2 = new Date($scope.ret_date);
        };

        $scope.markprocesschange = function () {
            $scope.EYC_MarksPublishDate = null;
            $scope.newdate = new Date($scope.EYC_MarksProcessLastDate);
            $scope.ret_date = $scope.newdate.setDate($scope.newdate.getDate() + 1);
            $scope.Return_Date3 = new Date($scope.ret_date);
        };

        $scope.savedatadates = function () {
            if ($scope.myForm1.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EYC_ExamStartDate": new Date($scope.EYC_ExamStartDate).toDateString(),
                    "EYC_ExamEndDate": new Date($scope.EYC_ExamEndDate).toDateString(),
                    "EYC_MarksEntryLastDate": new Date($scope.EYC_MarksEntryLastDate).toDateString(),
                    "EYC_MarksProcessLastDate": new Date($scope.EYC_MarksProcessLastDate).toDateString(),
                    "EYC_MarksPublishDate": new Date($scope.EYC_MarksPublishDate).toDateString()
                };

                apiService.create("CategorySubjectMapping/savedatadates", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved / Updated Successfully");
                        } else {
                            swal("Failed To Save /Record");
                        }
                        $scope.OnLoadCategoryDates();
                    }
                });

            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.interacted1 = function () {
            return $scope.submitted1;
        };

        $scope.cleardates = function () {
            $scope.OnLoadCategoryDates();
        };

        $scope.filterValue1 = function (obj) {
            return ($filter('eyC_ExamStartDate')(obj.eyC_ExamStartDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                ($filter('eyC_ExamEndDate')(obj.asmaY_From_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                ($filter('eyC_MarksEntryLastDate')(obj.asmaY_From_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                ($filter('eyC_MarksProcessLastDate')(obj.asmaY_From_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                ($filter('eyC_MarksPublishDate')(obj.asmaY_From_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.emcA_CategoryName)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
    }
})();