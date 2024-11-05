
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgStudentMappingController', StudentMappingController)

    StudentMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function StudentMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.studentlist = false;
        $scope.BindData = function () {
            apiService.getDATA("ClgStudentMapping/getalldetails").
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                    $scope.course_list = promise.courseslist;
                    $scope.branch_list = promise.branchlist;
                    $scope.semisters_list = promise.semisters;
                    $scope.seclist = promise.sections;
                    $scope.subjectgrp_list = promise.subjectgrplist;
                });
        };

        $scope.getcourse = function () {

            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EMG_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ClgStudentMapping/getcourse", data).then(function (promise) {
                $scope.course_list = promise.courseslist;
            });
        };

        $scope.getbranch = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EMG_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("ClgStudentMapping/getbranch", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };

        $scope.getsemester = function () {           
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EMG_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("ClgStudentMapping/getsemester", data).then(function (promise) {
                $scope.semisters_list = promise.semisters;
            });
        };

        $scope.getsection = function () {           
            $scope.ACMS_Id = "";
            $scope.EMG_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("ClgStudentMapping/getsection", data).then(function (promise) {
                $scope.seclist = promise.sections;
                $scope.subjectgrp_list = promise.subjectgrplist;
            });
        };









        $scope.search_studata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "EMG_Id": $scope.EMG_Id
                };

                apiService.create("ClgStudentMapping/Studentdetails", data).
                    then(function (promise) {
                        if (promise.studlist.length > 0 && promise.allstudent_details.length > 0) {
                            $scope.subjectlt1 = [];
                            $scope.subjectlt = promise.allsubject_list;
                            $scope.studentlist = true;
                            for (var i = 0; i < promise.studlist.length; i++) {
                                var count = 0;
                                var list = [];
                                angular.forEach(promise.allstudent_details, function (itm123) {
                                    if (itm123.amcsT_Id == promise.studlist[i].amcsT_Id) {
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
                                    });
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
                                    newCol = { amcsT_Id: promise.studlist[i].amcsT_Id, amcsT_Name: promise.studlist[i].amcsT_Name, amcsT_AdmNo: promise.studlist[i].amcsT_AdmNo, amcsT_RegistrationNo: promise.studlist[i].amcsT_RegistrationNo, sub_list: list567, xyz: true, sub_list_view: list123 }
                                    $scope.subjectlt1.push(newCol);
                                }
                                else if (count == 0) {

                                    newCol = { amcsT_Id: promise.studlist[i].amcsT_Id, amcsT_Name: promise.studlist[i].amcsT_Name, amcsT_AdmNo: promise.studlist[i].amcsT_AdmNo, amcsT_RegistrationNo: promise.studlist[i].amcsT_RegistrationNo, sub_list: list567, xyz: false, sub_list_view: list123 }
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
                            $scope.subjectlt = promise.allsubject_list
                            $scope.temp_arr1 = [];
                            for (var a = 0; a < promise.studlist.length; a++) {
                                var newCol = "";
                                var list123 = [];
                                angular.forEach($scope.subjectlt, function (itm6) {
                                    list123.push({ id: itm6.ismS_Id, name: itm6.ismS_SubjectName, abc: false })
                                });
                                newCol = { amcsT_Id: promise.studlist[a].amcsT_Id, amcsT_Name: promise.studlist[a].amcsT_Name, amcsT_AdmNo: promise.studlist[a].amcsT_AdmNo, amcsT_RegistrationNo: promise.studlist[a].amcsT_RegistrationNo, sub_list: [], xyz: false, sub_list_view: list123 }
                                $scope.subjectlt1.push(newCol);
                                $scope.temp_arr1 = $scope.subjectlt1;
                            }
                            $scope.studentlist = true;
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
                        else {
                            swal('No Record Found')
                            $scope.studentlist = false;
                        }
                    });
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.onchangegrpmaxmin = function (emgid) {
            if (emgid > 0) {
                angular.forEach($scope.subjectgrp_list, function (opq) {
                    if (opq.emG_Id == emgid) {
                        $scope.Max_appli_subs = opq.emG_MaxAplSubjects;
                        $scope.Min_appli_subs = opq.emG_MinAplSubjects;
                    }
                })
            }
        }
        $scope.subchkbx = function (column, user) {
            angular.forEach($scope.subjectlt1, function (itm1) {
                if (itm1.amcsT_Id == user.amcsT_Id) {
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
                        });
                    }
                    else {
                        if (itm1.sub_list.length < $scope.Max_appli_subs) {
                            itm1.sub_list = [];
                            angular.forEach(itm1.sub_list_view, function (itm2) {
                                if (itm2.abc == true) {
                                    itm1.sub_list.push({ id: itm2.id, name: itm2.name, abc: itm2.abc });
                                }
                            });
                        }
                        else {
                            var sub_alrdy_cnt = 0;
                            angular.forEach(itm1.sub_list, function (itr) {
                                if (itr.id == column.id) {
                                    sub_alrdy_cnt += 1;
                                }
                            });
                            if (sub_alrdy_cnt == 0) {
                                swal("Beyond The Max.Applicable Subjects You can't Select");
                                column.abc = false;
                            }
                        }
                    }
                }
            });
        };
        $scope.toggleAll1 = function (sub_id, chk) {
            angular.forEach($scope.subjectlt1, function (opq) {
                if (opq.xyz == true) {
                    angular.forEach(opq.sub_list_view, function (opq1) {
                        if (opq1.id == sub_id) {
                            opq1.abc = chk;
                            $scope.subchkbx(opq1, opq);
                        }
                    });
                }

            });
        }
        $scope.optionToggled = function () {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
        }
        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

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
                    })
                    if ($scope.selectedstudent.length > 0) {
                        var data = {
                            "ECSTSU_Id": $scope.ecstsU_Id,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "AMCO_Id": $scope.AMCO_Id,
                            "AMB_Id": $scope.AMB_Id,
                            "ACMS_Id": $scope.ACMS_Id,
                            "AMSE_Id": $scope.AMSE_Id,
                            "EMG_Id": $scope.EMG_Id,
                            get_list: $scope.selectedstudent
                        }
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }
                        apiService.create("ClgStudentMapping/savedetails", data).
                            then(function (promise) {
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
                            })
                    }
                    else if ($scope.selectedstudent.length == 0) {
                        swal("Select Atleast One Student ");
                    }
                }
                else {
                    swal("Selected Students Mapped Subjects Count Is Equal To Min.Applicable Subjects Of Selected Group");
                }

            }
        };
        $scope.clear = function () {

            $scope.ASMAY_Id = "";
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMSE_Id = "";
            $scope.EMG_Id = "";
            $scope.Max_appli_subs = "";
            $scope.Min_appli_subs = "";
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.save_update = "";
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;
            });
        }
    }

})();