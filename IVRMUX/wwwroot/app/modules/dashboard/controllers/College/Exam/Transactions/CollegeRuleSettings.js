(function () {
    'use strict';
    angular.module('app').controller('CollegeRuleSettingsController', CollegeRuleSettingsController)
    CollegeRuleSettingsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function CollegeRuleSettingsController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("CollegeRuleSettings/getalldetails", pageid).then(function (promise) {
                $scope.currentPage = 1;
                $scope.itemsPerPage = 5;
                $scope.getcourse = promise.getcourse;
                $scope.getsaveddetails = promise.getsaveddetails;
                $scope.gridOptions.data = $scope.getsaveddetails;
            });
        };

        // GET BRANCH
        $scope.getbranch = function () {
            $scope.ISMS_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.EMGR_Id = "";
            $scope.exam_list = [];
            $scope.EMRSSG_MarksValue = "";
            $scope.EMRSSG_PercentValue = "";
            $scope.EMRSSG_GroupName = "";
            $scope.EMRSSG_MaxOff = "";
            $scope.EMRSSG_BestOff = "";
            $scope.EMRSSG_DisplayName = "";

            var data = {
                "AMCO_Id": $scope.AMCO_Id
            };

            apiService.create("CollegeRuleSettings/getbranch", data).then(function (promise) {
                $scope.branch_list = promise.getbranch;
            });
        };

        // GET SEMESTER
        $scope.get_semesters = function () {
            $scope.ISMS_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.EMGR_Id = "";
            $scope.exam_list = [];
            $scope.EMRSSG_MarksValue = "";
            $scope.EMRSSG_PercentValue = "";
            $scope.EMRSSG_GroupName = "";
            $scope.EMRSSG_MaxOff = "";
            $scope.EMRSSG_BestOff = "";
            $scope.EMRSSG_DisplayName = "";

            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };

            apiService.create("CollegeRuleSettings/get_semesters", data).then(function (promise) {

                $scope.semisters_list = promise.getsemester;
            });
        };

        // GET SUBJECT SCHEME
        $scope.get_subjectscheme = function () {
            $scope.ISMS_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.EMGR_Id = "";
            $scope.exam_list = [];
            $scope.EMRSSG_MarksValue = "";
            $scope.EMRSSG_PercentValue = "";
            $scope.EMRSSG_GroupName = "";
            $scope.EMRSSG_MaxOff = "";
            $scope.EMRSSG_BestOff = "";
            $scope.EMRSSG_DisplayName = "";
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("CollegeRuleSettings/get_subjectscheme", data).then(function (promise) {

                $scope.getsubjectscheme = promise.getsubjectscheme;
            });
        };

        // GET SCHEME TYPE
        $scope.get_schemetype = function () {
            $scope.ISMS_Id = "";
            $scope.ACST_Id = "";
            $scope.EMGR_Id = "";
            $scope.exam_list = [];
            $scope.EMRSSG_MarksValue = "";
            $scope.EMRSSG_PercentValue = "";
            $scope.EMRSSG_GroupName = "";
            $scope.EMRSSG_MaxOff = "";
            $scope.EMRSSG_BestOff = "";
            $scope.EMRSSG_DisplayName = "";
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("CollegeRuleSettings/get_schemetype", data).then(function (promise) {

                $scope.getschemetype = promise.getschemetype;
            });
        };

        // GET SUBJECT EXAM AND GRADE
        $scope.get_subjects = function () {
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACST_Id": $scope.ACST_Id
            };

            apiService.create("CollegeRuleSettings/get_subjects", data).then(function (promise) {
                $scope.getsubjects = promise.getsubject;
                $scope.exam_list = promise.getexam;
                $scope.grade_list = promise.getgrade;
            });
        };

        // GET THE SELECTED SUBJECT NAME
        $scope.getsubjectsdetails = function () {
            angular.forEach($scope.getsubjects, function (sub) {
                if (sub.ismS_Id === parseInt($scope.ISMS_Id)) {
                    $scope.ISMS_SubjectName1 = sub.ismS_SubjectName;
                    $scope.ISMS_Id1 = sub.ismS_Id;
                }

                $scope.Maxmarks === "";
                $scope.Minmarks === "";
                $scope.convmarks === "";
            });
        };

        // GET THE SELECTED GRADE
        $scope.get_gradename = function () {
            $scope.EMGR_Id1 = $scope.EMGR_Id;
        };

        $scope.group_list_subwise = [];
        $scope.group_cnt = $scope.group_list_subwise.length;

        $scope.ECYS_Id = 0;

        // CLEAR THE DETAILS
        $scope.clr_max_bst = function () {

            if ($scope.ECYS_Id !== "" && $scope.ECYS_Id !== null && $scope.ECYS_Id !== undefined) {
                //gfg
            }
            else {
                angular.forEach($scope.exam_list, function (abc) {
                    abc.checked = false;
                });
            }
            $scope.EMRSSG_MaxOff = 0;
            angular.forEach($scope.exam_list, function (opt123) {
                if (opt123.checked === true) {
                    $scope.EMRSSG_MaxOff += 1;
                }
            });
            if ($scope.EMRS_MarksPerFlg === 'M') {
                $scope.EMRSSG_MarksValue = "";
            }
            $scope.EMRSSG_BestOff = "";
        };

        $scope.subjectdetails = [];

        // ADD
        $scope.saveddata1 = function (subjs_subs) {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                if ($scope.Maxmarks === null || $scope.Maxmarks === undefined || $scope.Maxmarks === "" || $scope.Minmarks === null || $scope.Minmarks === undefined || $scope.Minmarks === "" || $scope.convmarks === null || $scope.convmarks === undefined || $scope.convmarks === "") {
                    swal("Enter The Subject Marks Details");
                    return;
                } else {
                    if ($scope.subjectdetails.length === 0) {
                        $scope.subjectdetails.push({
                            ISMS_Id: $scope.ISMS_Id, ISMS_SubjectName: $scope.ISMS_SubjectName1, EMRSS_MaxMarks: $scope.Maxmarks, EMRSS_MinMarks: $scope.Minmarks,
                            EMRSS_ConvertForMarks: $scope.convmarks, EMGR_Id: $scope.EMGR_Id1, EMRSS_AppToResultFlg: $scope.EMPS_AppToResultFlg
                        });
                    } else if ($scope.subjectdetails.length > 0) {

                        var count = 0;
                        angular.forEach($scope.subjectdetails, function (ff) {
                            if (parseInt(ff.ISMS_Id) === parseInt($scope.ISMS_Id)) {
                                $scope.submaxmarksdetails = ff.EMRSS_MaxMarks;
                                count += 1;
                            }
                        });
                        if (count === 0) {
                            $scope.subjectdetails.push({
                                ISMS_Id: $scope.ISMS_Id, ISMS_SubjectName: $scope.ISMS_SubjectName1, EMRSS_MaxMarks: $scope.Maxmarks, EMRSS_MinMarks: $scope.Minmarks,
                                EMRSS_ConvertForMarks: $scope.convmarks, EMGR_Id: $scope.EMGR_Id1, EMRSS_AppToResultFlg: $scope.EMPS_AppToResultFlg
                            });
                        }
                    }
                }

                $scope.temp_exam_list = [];

                angular.forEach($scope.getsubjects, function (sub) {
                    if (sub.ismS_Id === parseInt($scope.ISMS_Id)) {
                        $scope.ISMS_SubjectName = sub.ismS_SubjectName;
                    }
                });

                angular.forEach($scope.exam_list, function (itm) {
                    if (itm.checked === true) {
                        $scope.temp_exam_list.push(itm);
                    }
                });


                if ($scope.group_list_subwise.length === 0) {
                    if ($scope.EMRS_MarksPerFlg === 'P') {
                        var percentage = 0;
                        angular.forEach($scope.group_list_subwise, function (opti) {
                            percentage += Number(opti.EMRSSG_PercentValue);
                        });

                        percentage += Number($scope.EMRSSG_PercentValue);
                        if (percentage <= 100) {
                            $scope.group_list_subwise.push({
                                ISMS_Id: $scope.ISMS_Id, ISMS_SubjectName1: $scope.ISMS_SubjectName1,
                                EMRSSG_GroupName: $scope.EMRSSG_GroupName, EMRSSG_DisplayName: $scope.EMRSSG_DisplayName,
                                EMRSSG_PercentValue: Number($scope.EMRSSG_PercentValue), EMRSSG_MarksValue: Number($scope.EMRSSG_MarksValue),
                                EMRSSG_MaxOff: Number($scope.EMRSSG_MaxOff), EMRSSG_BestOff: Number($scope.EMRSSG_BestOff),
                                Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list, Subjectlist: $scope.subjectdetails
                            });
                        }
                    }
                    else if ($scope.EMRS_MarksPerFlg === 'M') {
                        var max_marks_value = 0;
                        angular.forEach($scope.subject_list, function (opte) {
                            if (opte.checkedvalue === true) {
                                max_marks_value = Number(opte.EMPS_MaxMarks);
                            }
                        });
                        var chk_marksval_tot = 0;
                        angular.forEach($scope.group_list_subwise, function (opti) {
                            chk_marksval_tot += Number(opti.EMRSSG_MarksValue);
                        });
                        if (chk_marksval_tot <= max_marks_value) {
                            $scope.group_list_subwise.push({
                                ISMS_Id: $scope.ISMS_Id, ISMS_SubjectName1: $scope.ISMS_SubjectName1,
                                EMRSSG_GroupName: $scope.EMRSSG_GroupName, EMRSSG_DisplayName: $scope.EMRSSG_DisplayName,
                                EMRSSG_PercentValue: Number($scope.EMRSSG_PercentValue), EMRSSG_MarksValue: Number($scope.EMRSSG_MarksValue),
                                EMRSSG_MaxOff: Number($scope.EMRSSG_MaxOff), EMRSSG_BestOff: Number($scope.EMRSSG_BestOff),
                                Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list, Subjectlist: $scope.subjectdetails
                            });
                        }
                    }

                    $scope.group_cnt = $scope.group_list_subwise.length;
                }

                else if ($scope.group_list_subwise.length > 0) {
                    var dupli_count = 0;
                    angular.forEach($scope.group_list_subwise, function (opti) {
                        if (opti.EMRSSG_GroupName.toUpperCase() === $scope.EMRSSG_GroupName.toUpperCase()
                            && opti.EMRSSG_DisplayName.toUpperCase() === $scope.EMRSSG_DisplayName.toUpperCase()
                            && Number(opti.EMRSSG_PercentValue) === Number($scope.EMRSSG_PercentValue)
                            && Number(opti.EMRSSG_MarksValue) === Number($scope.EMRSSG_MarksValue) && Number(opti.EMRSSG_MaxOff) === Number($scope.EMRSSG_MaxOff)
                            && Number(opti.EMRSSG_BestOff) === Number($scope.EMRSSG_BestOff)) {
                            if (opti.Exm_M_Prom_Subj_Group_Exams_master.length === $scope.temp_exam_list.length) {
                                var exam_dupli_count = 0;
                                angular.forEach(opti.Exm_M_Prom_Subj_Group_Exams_master, function (s1) {
                                    angular.forEach($scope.temp_exam_list, function (s2) {
                                        if (s1.emE_Id === s2.emE_Id) {
                                            exam_dupli_count += 1;
                                        }
                                    });
                                });
                                if (exam_dupli_count === $scope.temp_exam_list.length) {
                                    dupli_count += 1;
                                }
                            }
                        }
                    });
                    if (dupli_count === 0) {
                        if ($scope.EMRS_MarksPerFlg === 'P') {
                            percentage = 0;
                            angular.forEach($scope.group_list_subwise, function (opti) {
                                percentage += Number(opti.EMRSSG_PercentValue);
                            });
                            percentage += Number($scope.EMRSSG_PercentValue);
                            if (percentage <= 100) {
                                $scope.group_list_subwise.push({
                                    ISMS_Id: $scope.ISMS_Id, ISMS_SubjectName1: $scope.ISMS_SubjectName1,
                                    EMRSSG_GroupName: $scope.EMRSSG_GroupName, EMRSSG_DisplayName: $scope.EMRSSG_DisplayName,
                                    EMRSSG_PercentValue: $scope.EMRSSG_PercentValue, EMRSSG_MarksValue: $scope.EMRSSG_MarksValue,
                                    EMRSSG_MaxOff: $scope.EMRSSG_MaxOff, EMRSSG_BestOff: $scope.EMRSSG_BestOff,
                                    Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list
                                });
                            }
                            else {
                                swal("Total of Groups Percentage Value is Not More Than 100");
                                $scope.clear1();
                            }
                        }
                        else if ($scope.EMRS_MarksPerFlg === 'M') {
                            max_marks_value = $scope.submaxmarksdetails;
                            //angular.forEach($scope.subject_list, function (opte) {
                            //    if (opte.checkedvalue === true) {
                            //        max_marks_value = Number(opte.EMPS_MaxMarks);
                            //    }
                            //});
                            chk_marksval_tot = 0;
                            angular.forEach($scope.group_list_subwise, function (opti) {
                                chk_marksval_tot += Number($scope.EMRSSG_MarksValue) + Number(opti.EMRSSG_MarksValue);
                            });
                            if (chk_marksval_tot <= parseInt(max_marks_value)) {
                                $scope.group_list_subwise.push({
                                    ISMS_Id: $scope.ISMS_Id, ISMS_SubjectName1: $scope.ISMS_SubjectName1,
                                    EMRSSG_GroupName: $scope.EMRSSG_GroupName, EMRSSG_DisplayName: $scope.EMRSSG_DisplayName,
                                    EMRSSG_PercentValue: Number($scope.EMRSSG_PercentValue), EMRSSG_MarksValue: Number($scope.EMRSSG_MarksValue),
                                    EMRSSG_MaxOff: Number($scope.EMRSSG_MaxOff), EMRSSG_BestOff: Number($scope.EMRSSG_BestOff),
                                    Exm_M_Prom_Subj_Group_Exams_master: $scope.temp_exam_list
                                });
                            }
                            else {
                                swal("Total of Groups Marks Value is Not More Than Subjects Max.Marks" + max_marks_value);
                                $scope.clear1();
                            }
                        }
                        $scope.group_cnt = $scope.group_list_subwise.length;
                    }
                    else if (dupli_count > 0) {
                        swal("Entered Details Are Already In Below List");
                    }
                }

                console.log($scope.group_list_subwise);
                console.log($scope.subjectdetails);
                $scope.all1 = true;
                $scope.toggleAll1();
                $scope.clear1();
            }
            else {
                $scope.submitted1 = true;
            }
        };

        // TOGGLE ALL
        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all1;
            angular.forEach($scope.group_list_subwise, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        };

        // CLEAR ADD DETAILS
        $scope.clear1 = function () {
            if ($scope.ECYS_Id !== null && $scope.ECYS_Id !== undefined && $scope.ECYS_Id !== "") {
                //angular.forEach($scope.exam_list, function (itm1) {
                //    itm1.checked = false;
                //})
            }
            else {
                $scope.exam_list = $scope.exam_list;

            }
            angular.forEach($scope.exam_list, function (itm1) {

                itm1.checked = false;
            });
            $scope.EMRSSG_MarksValue = "";
            $scope.EMRSSG_PercentValue = "";
            $scope.EMRSSG_GroupName = "";
            $scope.EMRSSG_DisplayName = "";
            $scope.EMRSSG_MaxOff = 0;
            $scope.EMRSSG_BestOff = "";
            //$scope.EMPSG_Id = 0;
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
            if ($scope.EMRS_MarksPerFlg === 'M') {
                $scope.EMRSSG_PercentValue = 0;
            }
            else if ($scope.EMRS_MarksPerFlg === 'P') {
                $scope.EMRSSG_MarksValue = 0;
            }
        };

        // VALIDATION
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        // VIEW ADD EXAM
        $scope.view_exams = function (index) {
            $scope.groupName = $scope.group_list_subwise[index].EMPSG_GroupName;
            $scope.view_exam_details = $scope.group_list_subwise[index].Exm_M_Prom_Subj_Group_Exams_master;
        };

        // DELETE ADD DETAILS
        $scope.delete = function (row, index) {
            for (var x = 0; x < $scope.group_list_subwise.length; x++) {
                if (x === parseInt(index)) {
                    $scope.group_list_subwise.splice(x, 1);
                }
            }
            $scope.group_cnt = $scope.group_list_subwise.length;
            $scope.clear1();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.exam_list.some(function (options) {
                return options.checked;
            });
        };

        // OVER ALL SAVE 
        $scope.saveddata = function () {

            var data = {

                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id,
                "EMGR_Id": $scope.EMGR_Id,
                "EMRS_MarksPerFlg": $scope.EMRS_MarksPerFlg,
                "Temp_Subject_ListDTO": $scope.subjectdetails,
                "Temp_Group_ListDTO": $scope.group_list_subwise
            };
            apiService.create("CollegeRuleSettings/saveddata", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    } else if (promise.message === "Update") {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Update Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        }
                    } else {
                        swal("Record Failed To Save / Update");
                    }
                    $state.reload();
                }
            });
        };

        $scope.savecategorycoursebranchmap = function () {
            var selected_semisters = [];
            angular.forEach($scope.semisters_list, function (stu3) {
                if (stu3.amsE_Id1 === true) {
                    selected_semisters.push(stu3);
                }
            });
            var data = {
                "ECYS_Id": $scope.ECYS_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "EMG_Id": $scope.EMG_Id,
                "ACST_Id": $scope.ACST_Id,
                selected_semis: selected_semisters,
                selected_subgrps: $scope.subject_groups
            };

            apiService.create("CollegeRuleSettings/savedetail2", data).
                then(function (promise) {
                    $scope.gridOptions.data = promise.alllist;
                    if (promise.returnval === true) {
                        if (promise.ecyS_Id === 0 || promise.ecyS_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ecyS_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    $scope.loadData();
                    $scope.clear2();
                });
        };

        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'amcO_CourseName', displayName: 'Course' },
                { name: 'amB_BranchName', displayName: 'Branch' },      
                { name: 'amsE_SEMName', displayName: 'Semester' },
                { name: 'acsS_SchmeName', displayName: 'Schme' },
                { name: 'acsT_SchmeType', displayName: 'SchmeType' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.emrS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.emrS_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };
        
        // VIEW THE SUBJECT DETAILS
        $scope.viewrecordspopup = function (employee, SweetAlert) {
            apiService.create("CollegeRuleSettings/getalldetailsviewrecords", employee).
                then(function (promise) {
                    $scope.amcO_CourseName = promise.getviewsubjects[0].amcO_CourseName;
                    $scope.amB_BranchName = promise.getviewsubjects[0].amB_BranchName;
                    $scope.acsS_SchmeName = promise.getviewsubjects[0].acsS_SchmeName;
                    $scope.acsT_SchmeType = promise.getviewsubjects[0].acsT_SchmeType;
                    $scope.amsE_SEMName = promise.getviewsubjects[0].amsE_SEMName;
                    $scope.viewrecordspopupdisplay = promise.getviewsubjects;
                    $scope.Category_Name = $scope.amcO_CourseName + '/' + $scope.amB_BranchName + '/' + $scope.amsE_SEMName;
                });
        };

        // VIEW THE GROUP DETAILS FOR THAT SUBJECT 
        $scope.viewrecordspopup_subgrps = function (employee) {

            apiService.create("CollegeRuleSettings/viewrecordspopup_subgrps", employee).then(function (promise) {
                $scope.pro_Subject = promise.getviewgroupdetails[0].ismS_SubjectName;
                $scope.viewrecordspopup_subgrps_list = promise.getviewgroupdetails;
            });
        };

        // VIEW THE EXAM DETAILS FOR THAT GROUP
        $scope.viewrecordspopup_sub_grp_exms = function (employee) {
            apiService.create("CollegeRuleSettings/getalldetailsviewrecords_sub_grp_exms", employee).
                then(function (promise) {
                    $scope.sub_groupName = promise.getviewexamdetails[0].emrssG_GroupName;
                    $scope.viewrecordspopup_subgrps_exms = promise.getviewexamdetails;
                });
        };

        // Deactivate The Main Table
        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            if (employee.emrS_ActiveFlag === true) {
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

                        apiService.create("CollegeRuleSettings/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.BindData();
                            });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.deactivatesubject = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            if (employee.emrsS_ActiveFlag === true) {
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

                        apiService.create("CollegeRuleSettings/deactivatesubject", employee).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.viewrecordspopup(employee);
                            });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.deactivategroup = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            if (employee.emrssG_ActiveFlag === true) {
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

                        apiService.create("CollegeRuleSettings/deactivategroup", employee).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.viewrecordspopup_subgrps(employee);
                            });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.deactivateexam = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            if (employee.emrssgE_ActiveFlg === true) {
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

                        apiService.create("CollegeRuleSettings/deactivateexam", employee).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.viewrecordspopup_sub_grp_exms(employee);
                            });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        $scope.clear2 = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.ACSS_Id = "";
            $scope.EMG_Id = "";
            $scope.ACST_Id = "";
            $scope.ECYS_Id = 0;
            $scope.subject_groups = "";
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.gridApi2.grid.clearAllFilters();
            $scope.search = "";
        };

        
          
        $scope.clear = function () {
            $state.reload();
        };
    }

})();