(function () {
    'use strict';
    angular.module('app').controller('StudentMappingController', StudentMappingController)
    StudentMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function StudentMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.Left_Flag = false;
        $scope.Deactive_Flag = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.RemoveRecordsList = [];
        $scope.examlist = [];
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("StudentMapping/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.catlist = promise.ctlist;
                $scope.grplist = promise.grouplist;
                $scope.asmaY_Id = promise.asmaY_Id;
                $scope.temp_year_id = promise.asmaY_Id;
                angular.forEach($scope.yearlt, function (opq) {
                    if (opq.asmaY_Id == $scope.asmaY_Id) {
                        opq.Selected = true;
                    }
                })
                $scope.OnAcdyear();
                $scope.gridOptions.data = promise.studmaplist;
            });
        };

        //To find Category from selected Acd Year
        $scope.OnAcdyear = function () {
            $scope.examlist = [];
            $scope.RemoveRecordsList = [];
            if ($scope.asmaY_Id > 0) {
                var id = $scope.asmaY_Id;
                apiService.getURI("StudentMapping/getcategory", id).then(function (promise) {
                    $scope.catlist = promise.ctlist;
                    if (promise.ctlist == null || promise.ctlist == "") {
                        swal("Categories Are Not Maapped With Selected Academic Year");
                        $scope.asmaY_Id = "";
                    }
                    $scope.emcA_Id = "";
                    $scope.emG_Id = "";
                    $scope.asmcL_Id = "";
                    $scope.obj.EME_Id = "";
                });
            } else {
                swal('Record Not Found');
                $scope.asmaY_Id = "";
            }
            $scope.studentlist = false;
        };

        //To find class from selected category
        $scope.Oncategoryname = function () {
            $scope.examlist = [];
            $scope.RemoveRecordsList = [];
            if ($scope.emcA_Id > 0) {
                var id = $scope.emcA_Id;
                var data = {
                    "EMCA_Id": $scope.emcA_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                };
                apiService.create("StudentMapping/getclassid", data).then(function (promise) {
                    $scope.clslist = promise.classlist;
                    $scope.grplist = promise.grouplist;
                    $scope.examlist = promise.examlist;
                    $scope.emG_Id = "";
                    $scope.asmcL_Id = "";
                    $scope.obj.EME_Id = "";
                });
            } else {
                swal('Record Not Found');
            }
            $scope.studentlist = false;
        };

        //To Select the Dynamic Subject
        $scope.Onsubjectchange = function (emgid) {
            $scope.RemoveRecordsList = [];
            $scope.temp_arr2 = [];
            $scope.obj.EME_Id = "";
            if ($scope.emG_Id > 0) {
                angular.forEach($scope.grplist, function (opq) {
                    if (opq.emG_Id == $scope.emG_Id) {
                        $scope.Max_appli_subs = opq.emG_MaxAplSubjects;
                        $scope.Min_appli_subs = opq.emG_MinAplSubjects;
                    }
                });
                var id = $scope.emG_Id;
                apiService.getURI("StudentMapping/getsubject", id).then(function (promise) {
                    $scope.subjectlt = promise.subjlist;
                    $scope.temp_arr2 = promise.subjlist;
                });
            } else {
                swal('Record Not Found');
            }
            $scope.studentlist = false;
        };

        $scope.valid_Class = function (id) {
            $scope.RemoveRecordsList = [];
            if (id == null) {
                swal('Please select the Exam Category')
                $scope.ecaC_Id = "";
            }
        };

        $scope.OnChangeExam = function (id) {
            $scope.RemoveRecordsList = [];
            $scope.subjectlt1 = [];
            $scope.studentlist = false;
        };

        //Validation  for Section
        $scope.save_update = "";
        $scope.valid_Section = function (id1, id2, id3, id4) {
            $scope.submitted = true;
            $scope.RemoveRecordsList = [];
            if ($scope.myForm.$valid) {

                if (id1 > 0 && id2 > 0 && id3 == null && id4 > 0) {
                    swal('Please select the Academic Year')
                    $scope.asmS_Id = "";
                }
                else if (id2 > 0 && id3 > 0 && id1 == null && id4 > 0) {
                    swal('Please select the Class')
                    $scope.asmS_Id = "";
                }
                else if (id1 > 0 && id2 > 0 && id3 > 0 && id4 > 0) {
                    $scope.save_update = "";

                    var emeid = 0;
                    if ($scope.examlist !== null && $scope.examlist.length > 0) {
                        emeid = $scope.obj.EME_Id;
                    }

                    var data = {
                        "EMCA_Id": id2,
                        "EMG_Id": $scope.emG_Id,
                        "ASMAY_Id": id3,
                        "ASMS_Id": id4,
                        "ASMCL_Id": id1,
                        "EME_Id": emeid,
                        "Deactive_Flag": $scope.Deactive_Flag,
                        "Left_Flag": $scope.Left_Flag,
                    };

                    apiService.create("StudentMapping/Studentdetails", data).then(function (promise) {
                        if (promise.studlist.length > 0 && promise.allstudent_details.length > 0) {
                            $scope.subjectlt1 = [];
                            $scope.studentlist = true;
                            for (var i = 0; i < promise.studlist.length; i++) {
                                var count = 0;
                                var overall_count = 0;
                                var list = [];
                                overall_count = 0;
                                angular.forEach(promise.allstudent_details, function (itm123) {
                                    if (itm123.amsT_Id == promise.studlist[i].amsT_Id) {
                                        overall_count = 1;
                                        angular.forEach($scope.subjectlt, function (itm2) {
                                            if (itm123.ismS_Id == itm2.ismS_Id) {
                                                count += 1;
                                                list.push({ id: itm2.ismS_Id, name: itm2.ismS_SubjectName });
                                            }
                                        });
                                    }
                                });

                                var newCol = "";
                                var list123 = [];

                                angular.forEach($scope.subjectlt, function (itm6) {
                                    var sub_count = 0;
                                    angular.forEach(list, function (itm5) {
                                        if (itm5.id == itm6.ismS_Id) {
                                            sub_count += 1;
                                        }
                                    })
                                    if (sub_count == 0) {
                                        list123.push({ id: itm6.ismS_Id, name: itm6.ismS_SubjectName, abc: false })
                                    }
                                    else if (sub_count > 0) {
                                        list123.push({ id: itm6.ismS_Id, name: itm6.ismS_SubjectName, abc: true })
                                    }
                                });
                                var list567 = [];
                                angular.forEach(list123, function (itm2) {
                                    if (itm2.abc == true) {
                                        list567.push({ id: itm2.id, name: itm2.name, abc: itm2.abc });
                                    }
                                });

                                if (count > 0) {
                                    newCol = {
                                        amsT_Id: promise.studlist[i].amsT_Id, amsT_FirstName: promise.studlist[i].amsT_FirstName,
                                        amsT_AdmNo: promise.studlist[i].amsT_AdmNo, amaY_RollNo: promise.studlist[i].amaY_RollNo, sub_list: list567,
                                        xyz: true, sub_list_view: list123, remove_flag: overall_count
                                    }
                                    $scope.subjectlt1.push(newCol);

                                }
                                else if (count == 0) {

                                    newCol = {
                                        amsT_Id: promise.studlist[i].amsT_Id, amsT_FirstName: promise.studlist[i].amsT_FirstName,
                                        amsT_AdmNo: promise.studlist[i].amsT_AdmNo, amaY_RollNo: promise.studlist[i].amaY_RollNo,
                                        sub_list: list567, xyz: false, sub_list_view: list123, remove_flag: overall_count
                                    }
                                    $scope.subjectlt1.push(newCol);
                                }
                            }

                            $scope.optionToggled();
                            var stu_cnt = 0;
                            angular.forEach($scope.subjectlt1, function (user) {
                                if (user.xyz == true) {
                                    stu_cnt += 1;
                                }
                            });
                            if (stu_cnt == 0) {
                                $scope.save_update = "Save";
                            }
                            else if (stu_cnt > 0) {
                                $scope.save_update = "Update";
                            }
                        }
                        else if (promise.studlist.length > 0) {
                            $scope.subjectlt1 = [];
                            $scope.temp_arr1 = [];
                            var overall_count = 0;
                            for (var a = 0; a < promise.studlist.length; a++) {
                                overall_count = 0;
                                var newCol = "";
                                var list123 = [];
                                angular.forEach($scope.subjectlt, function (itm6) {
                                    list123.push({ id: itm6.ismS_Id, name: itm6.ismS_SubjectName, abc: false });
                                });
                                newCol = {
                                    amsT_Id: promise.studlist[a].amsT_Id, amsT_FirstName: promise.studlist[a].amsT_FirstName,
                                    amsT_AdmNo: promise.studlist[a].amsT_AdmNo, amaY_RollNo: promise.studlist[a].amaY_RollNo, sub_list: [],
                                    xyz: false, sub_list_view: list123, remove_flag: 0
                                }
                                $scope.subjectlt1.push(newCol);
                                $scope.temp_arr1 = $scope.subjectlt1;
                            }
                            $scope.studentlist = true;
                            var stu_cnt = 0;
                            angular.forEach($scope.subjectlt1, function (user) {
                                if (user.xyz == true) {
                                    stu_cnt += 1;
                                }
                            })
                            if (stu_cnt == 0) {
                                $scope.save_update = "Save";
                            }
                            else if (stu_cnt > 0) {
                                $scope.save_update = "Update";
                            }
                        }
                        else {
                            swal('No Record Found')
                        }
                    });
                }

                else if (id2 > 0 && id1 == null && id3 == null && id4 > 0) {
                    swal('Please select the Academic Year,Class')
                    $scope.asmS_Id = "";
                }
                else if (id3 > 0 && id2 == null && id1 == null && id4 > 0) {
                    swal('Please select the Exam Category,Class')
                    $scope.asmS_Id = "";
                }
                else {
                    swal('Please select the Academic Year,Exam Category,Class')
                    $scope.asmS_Id = "";
                }
            }
        };


        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'amsT_FirstName', displayName: 'Student Name' },
                { name: 'asmcL_ClassName', displayName: 'Class Name' },
                { name: 'asmC_SectionName', displayName: 'Section Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.estsU_ElecetiveFlag === true" href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

                        '<a ng-if="row.entity.estsU_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.estsU_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ]

        };

        //Record pop up 
        $scope.viewrecordspopup = function (employee, SweetAlert) {
            $scope.editEmployee = employee.amsT_Id;
            $scope.RemoveRecordsList = [];
            var pageid = $scope.editEmployee;

            apiService.getURI("StudentMapping/getalldetailsviewrecords", pageid).then(function (promise) {
                $scope.viewrecordspopupdisplay = promise.gtdetailsview;
            });
        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
            $scope.RemoveRecordsList = [];
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            $scope.RemoveRecordsList = [];
            if (deactiveRecord.estsU_ActiveFlg === true) {
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
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        };
                        apiService.create("StudentMapping/deactivate", deactiveRecord).then(function (promise) {
                            if (promise.returnval === true) {
                                swal(confirmmgs + ' Successfully');
                            }
                            else {
                                swal('Record Not  Activated/Deactivated');
                            }
                            $scope.BindData();
                            $scope.clearid1();
                        });
                    }
                    else {
                        swal("Record" + mgs + "Cancelled");
                    }
                });
        };

        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var MEditId = EditRecord.amsT_Id;
            $scope.RemoveRecordsList = [];
            apiService.getURI("StudentMapping/editdetails", MEditId).then(function (promise) {
                if (promise.editlist.length > 0) {
                    $scope.te_sub_ls = promise.editlist;
                    $scope.AMST_Id = promise.editlist[0].amsT_Id;
                    $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                    $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                    $scope.asmS_Id = promise.editlist[0].asmS_Id;
                    $scope.emcA_Id = promise.edclasslist[0].emcA_Id;
                    $scope.emG_Id = promise.editlist[0].emG_Id;
                    $scope.Onsubjectchange($scope.emG_Id);
                    $scope.valid_Section($scope.asmcL_Id, $scope.emcA_Id, $scope.asmaY_Id, $scope.asmS_Id);
                    $scope.studentlist = true;
                } else {
                    swal('No Record Found')
                }
            })
        };

        //$scope.subchkbx = function (record1,record,mainary) {
        $scope.subchkbx = function (column, user) {
            angular.forEach($scope.subjectlt1, function (itm1) {
                if (itm1.amsT_Id == user.amsT_Id) {
                    angular.forEach(itm1.sub_list_view, function (itm2) {
                        if (itm2.id == column.id) {
                            itm2.abc = column.abc;
                        }
                    });
                    if (column.abc == false) {
                        itm1.sub_list = [];
                        angular.forEach(itm1.sub_list_view, function (itm2) {
                            if (itm2.abc == true) {
                                itm1.sub_list.push({ id: itm2.id, name: itm2.name, abc: itm2.abc });
                            }
                        })
                    }
                    else {
                        if (itm1.sub_list.length < $scope.Max_appli_subs) {
                            itm1.sub_list = [];
                            angular.forEach(itm1.sub_list_view, function (itm2) {
                                if (itm2.abc == true) {
                                    itm1.sub_list.push({ id: itm2.id, name: itm2.name, abc: itm2.abc });
                                }
                            })
                        }
                        else {
                            var sub_alrdy_cnt = 0;
                            angular.forEach(itm1.sub_list, function (itr) {
                                if (itr.id == column.id) {
                                    sub_alrdy_cnt += 1;
                                }
                            })
                            if (sub_alrdy_cnt == 0) {
                                swal("Beyond The Max.Applicable Subjects You can't Select");
                                column.abc = false;
                            }
                        }
                    }
                }
            });
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.RemoveRecordsList.length > 0) {

                    var mgs = "";
                    var confirmmgs = "";
                    swal({
                        title: "Are you sure",
                        text: "Do You Want To Remove The Subject Mapping For" + $scope.RemoveRecordsList.length + " Students",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Remove it!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.save_student_subject_mapping();
                            }
                            else {
                                swal("Record Remove Cancelled");
                            }
                        });
                } else {
                    $scope.save_student_subject_mapping();
                }
            }
        };

        $scope.save_student_subject_mapping = function () {
            var min_appli_subs_fill = 0;
            angular.forEach($scope.subjectlt1, function (stu1) {
                if (stu1.xyz == true) {
                    if (stu1.sub_list.length < $scope.Min_appli_subs) {
                        min_appli_subs_fill += 1;
                    }
                }
            })
            if (min_appli_subs_fill == 0) {
                $scope.selectedstudent = [];
                angular.forEach($scope.subjectlt1, function (stu) {
                    if (stu.xyz == true) {
                        $scope.selectedstudent.push(stu);
                    }
                });
                if ($scope.selectedstudent.length > 0 || $scope.RemoveRecordsList.length > 0) {

                    var emeid = 0;
                    if ($scope.examlist !== null && $scope.examlist.length > 0) {
                        emeid = $scope.obj.EME_Id;
                    }

                    var data = {
                        "ESTSU_Id": $scope.estsU_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "EMG_Id": $scope.emG_Id,
                        "EME_Id": emeid,
                        get_list: $scope.selectedstudent,
                        get_Removed_list: $scope.RemoveRecordsList
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("StudentMapping/savedetails", data).then(function (promise) {
                        $scope.newuser = promise.mastersubexam;
                        if (promise.returnval === true) {
                            if ($scope.save_update == "Save") {
                                swal('Record saved successfully');
                            }
                            else if ($scope.save_update == "Update") {
                                swal('Record updated successfully');
                            }
                            else {
                                swal('Record Saved/Updated Successfully', 'success');
                            }
                        }
                        else if (promise.returnduplicatestatus === true || promise.returnval === false) {
                            swal('Record already exist');
                        }
                        else {
                            if ($scope.save_update == "Save") {
                                swal('Failed to save, please contact administrator');
                            }
                            else if ($scope.save_update == "Update") {
                                swal('Failed to update, please contact administrator');
                            }
                            else {
                                swal('Record Not Saved/Updated Successfully', 'Failed');
                            }
                        }
                        $scope.BindData();
                        $scope.clear();
                    });
                }
                else if ($scope.selectedstudent.length == 0) {
                    swal("Select Atleast One Student ");
                }
            }
            else {
                swal("Selected Students Mapped Subjects Count Is Equal To Min.Applicable Subjects Of Selected Group");
            }
        };

        $scope.clear = function () {
            $scope.asmcL_Id = "";
            $scope.emcA_Id = "";
            $scope.asmaY_Id = $scope.temp_year_id;
            angular.forEach($scope.yearlt, function (opq) {
                if (opq.asmaY_Id == $scope.asmaY_Id) {
                    opq.Selected = true;
                }
            })
            $scope.OnAcdyear();
            $scope.emG_Id = "";
            $scope.Max_appli_subs = "";
            $scope.Min_appli_subs = "";
            $scope.asmS_Id = "";
            $scope.obj.EME_Id = "";
            $scope.subjectlt = "";
            $scope.subjectlt1 = "";
            $scope.studentlist = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.save_update = "";
            $scope.RemoveRecordsList = [];
            //$state.reload();
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                if (itm.disabled_flag !== true) {
                    itm.xyz = toggleStatus;
                }
            });
        };

        $scope.get_cls_sections = function (cls_id) {
            $scope.RemoveRecordsList = [];
            if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.asmaY_Id != null && $scope.emcA_Id != "" && $scope.emcA_Id != undefined && $scope.emcA_Id != null) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "EMCA_Id": $scope.emcA_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    //clssids: $scope.selectedclasses,
                }

                apiService.create("StudentMapping/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.asmS_Id = "";
                    if (promise.seclist == null || promise.seclist == "") {
                        swal("Sections are Not Mapped To Selected Class!!!");
                    }
                });
            }
            else {
                swal("Please Select Academic Year And Category First !!!");
                $scope.asmcL_Id = "";
            };

            $scope.studentlist = false;
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
        }

        $scope.toggleAll1 = function (sub_id, chk) {
            angular.forEach($scope.subjectlt1, function (opq) {
                if (opq.xyz == true) {
                    angular.forEach(opq.sub_list_view, function (opq1) {
                        if (opq1.id == sub_id) {
                            opq1.abc = chk;
                            $scope.subchkbx(opq1, opq);
                        }
                    })
                }
            });
        };

        $scope.OnClickRemove = function (stu_user) {
            var mgs = "";
            var confirmmgs = "";
            swal({
                title: "Are you sure",
                text: "Do You Want To Remove This Record",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Remove it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.Temparray = [];
                        angular.forEach(stu_user.sub_list_view, function (dd) {
                            $scope.Temparray.push({ id: dd.id, name: dd.name });
                        });

                        var data = {
                            "ASMAY_Id": $scope.asmaY_Id,
                            "ASMCL_Id": $scope.asmcL_Id,
                            "ASMS_Id": $scope.asmS_Id,
                            "EMG_Id": $scope.emG_Id,
                            "AMST_Id": stu_user.amsT_Id,
                            "EME_Id": $scope.obj.EME_Id,
                            Temp_SubjectList: $scope.Temparray
                        };

                        apiService.create("StudentMapping/OnClickRemove", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("You Can Not Delete This Record Because Marks Entered For The Selected Student");
                            }
                            else {
                                stu_user.disabled_flag = true;
                                stu_user.delete_flag = true;
                                stu_user.xyz = false;
                                $scope.RemoveRecordsList.push({
                                    amsT_Id: stu_user.amsT_Id, StudentName: stu_user.amsT_FirstName, Admno: stu_user.amsT_AdmNo, RollNo: stu_user.amaY_RollNo,
                                    stubjectlist: $scope.Temparray
                                });
                                swal("Details Removed");
                            }
                        });
                    }
                    else {
                        swal("Record Remove Cancelled");
                    }
                });
        };

        $scope.RemoveRecords = function (stu_rmv_user, index) {
            $scope.RemoveRecordsList.splice(index, 1);

            angular.forEach($scope.subjectlt1, function (dd) {
                if (dd.amsT_Id === stu_rmv_user.amsT_Id) {
                    dd.disabled_flag = false;
                    dd.delete_flag = false;
                    dd.xyz = true;
                }
            });
        };
    }
})();