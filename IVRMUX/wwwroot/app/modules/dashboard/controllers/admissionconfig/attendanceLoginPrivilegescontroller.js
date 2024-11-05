(function () {
    'use strict';
    angular.module('app').controller('attendanceLoginPrivilegescontroller', attendanceLoginPrivilegescontroller);
    attendanceLoginPrivilegescontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function attendanceLoginPrivilegescontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $route, superCache) {
        $scope.resultclasssectionData = [];
        $scope.resultData = [];
        $scope.subjectresultData = [];
        $scope.selected_cls_secflag = false;
        loadInitialData();
        function loadInitialData() {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            // load data
            apiService.getURI("AttendanceLP/getinitialdata", 1).then(function (promise) {
                $scope.teachers = promise.teacherList;
                $scope.accyear = promise.accyear;
            });
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // get data by type of selection
        $scope.labeldisable = true;

        $scope.getDataByType = function (type) {

            $scope.teachers = [];
            $scope.classsectionList = [];
            $scope.resultclasssectionData = [];
            $scope.subjects = [];
            $scope.obj.IVRMUL_Id = "";
            $scope.labeldisable = false;
            $scope.ASMAY_Id = "";

            apiService.getURI("AttendanceLP/getdatabyselectedtype", type).then(function (promise) {

                $scope.teachers = promise.teacherList;
                $scope.subjects = promise.subjectList;
                $scope.loginPData = promise.loginPData;
                if ($scope.loginPData !== null) {

                    if ($scope.loginPData.length > 0) {
                        $scope.count = 1;
                    } else {
                        $scope.count = 0;
                    }                    
                } else {
                    $scope.count = 0;
                }
                $scope.selectedAll = true;
                $scope.selectedAll1 = true;
            });
        };

        function getDataBySelectedType(type) {
            apiService.getURI("AttendanceLP/getdatabyselectedtype", type).then(function (promise) {
                $scope.subjects = promise.subjectList;
                $scope.classsectionList = promise.classsectionList;
                $scope.selectedAll = true;
                $scope.selectedAll1 = true;
            });
        }

        // get data by type of selection
        $scope.getyear = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASALU_EntryTypeFlag": $scope.type,
            }
            apiService.create("AttendanceLP/getyear", data).then(function (promise) {

                $scope.classsectionList = promise.classsectionList;
                if ($scope.type ==="1" &&($scope.classsectionList === null || $scope.classsectionList.length == 0)) {
                    swal("No Class Is Mapped For The Subject Wise Attendance , So Map The Subject Wise Attendance Type");
                }
                $scope.loginPData = promise.loginPData;
                if ($scope.loginPData != null) {
                    if ($scope.loginPData.length > 0) {
                        $scope.count = $scope.loginPData.length;
                    } else {
                        $scope.count = 0;
                    }
                   
                } else {
                    $scope.count = 0;
                }
                $scope.subjects = promise.subjectList;
                
                $scope.teachers = promise.teacherList;
                $scope.selectedAll = true;
                $scope.selectedAll1 = true;

            })
        }





        $scope.chckedIndexs = [];
        $scope.chckedIndexs3 = [];
        $scope.checkedIndexes2 = [];

        $scope.GetFirtTableData = function () {
            $scope.resultData = $scope.chckedIndexs;
            //console.log($scope.resultData);
        };

        $scope.GetFirtTableData2 = function () {
            $scope.resultclasssectionData = $scope.chckedIndexs3;
            //console.log($scope.resultclasssectionData);
        };

        $scope.getSelectedData = function () {
            $scope.subjectresultData = $scope.checkedIndexes2;
            console.log($scope.subjectresultData);
        };

        $scope.RemoveSecondTableData = function () {
            $scope.resultData = $scope.resultData.filter(function (r1) {
                return !r1.Selected1
            });
        };

        $scope.RemoveSecondTableData2 = function () {
            $scope.resultclasssectionData = $scope.resultclasssectionData.filter(function (r12) {
                return !r12.Selected12
            });
        };

        $scope.RemoveSecondSelectedData = function () {
            $scope.subjectresultData = $scope.subjectresultData.filter(function (r11) {
                return !r11.Selectedsub1
            });
        };

        $scope.test = function (data, classsectionList) {

            if ($scope.chckedIndexs.indexOf(data) === -1) {

                if ($scope.editflag == true) {
                    $scope.resultData.splice($scope.resultData);
                    $scope.resultData.push(data);
                    //$scope.resultData[0].Selected1 = true;

                    for (var i = 0; i < classsectionList.length; i++) {
                        var name = classsectionList[i].name;
                        if (name == $scope.resultData[0].name) {
                            classsectionList[i].Selected = true;
                            $scope.resultData[0].Selected1 = true;
                        } else {
                            classsectionList[i].Selected = false;
                        }

                    }
                }
                else {
                    $scope.chckedIndexs.push(data);
                    $scope.resultData = $scope.chckedIndexs;
                    for (var i = 0; i < $scope.chckedIndexs.length; i++) {
                        if ($scope.chckedIndexs[i].Selected == true) {
                            $scope.resultData[i].Selected1 = true;
                        } else {
                            $scope.resultData[i].Selected1 = false;
                        }
                    }

                }
                // $scope.chckedIndexs.push(data);
                $scope.checkclssec = true;
            }
            else {
                $scope.chckedIndexs.splice($scope.chckedIndexs.indexOf(data), 1);

            }
        };

        $scope.test3 = function (data, classsectionList) {

            if ($scope.chckedIndexs3.indexOf(data) === -1) {
                if ($scope.editflag == true) {
                    $scope.resultclasssectionData.splice($scope.resultclasssectionData);
                    $scope.resultclasssectionData.push(data);

                    for (var i = 0; i < classsectionList.length; i++) {
                        var name = classsectionList[i].name;
                        if (name == $scope.resultclasssectionData[0].name) {
                            classsectionList[i].Selectedcsm = true;
                            $scope.resultclasssectionData[0].Selectedcsm1 = true;
                        } else {
                            classsectionList[i].Selectedcsm = false;
                        }

                    }
                }
                else {
                    $scope.chckedIndexs3.push(data);
                    $scope.resultclasssectionData = $scope.chckedIndexs3;
                    for (var i = 0; i < $scope.chckedIndexs3.length; i++) {
                        if ($scope.chckedIndexs3[i].Selectedcsm == true) {
                            $scope.resultclasssectionData[i].Selectedcsm1 = true;
                        } else {
                            $scope.resultclasssectionData[i].Selectedcsm1 = false;
                        }
                    }
                }
            }
            else {
                $scope.chckedIndexs3.splice($scope.chckedIndexs3.indexOf(data), 1);
            }
        };
        $scope.revtest3 = function (revtst3) {

            if (revtst3.Selectedcsm1 == false) {
                if ($scope.editflag == true) {
                    for (var i = 0; i < $scope.classsectionList.length; i++) {
                        var name = $scope.classsectionList[i].name;
                        if (name == $scope.resultclasssectionData[0].name) {
                            $scope.classsectionList[i].Selectedcsm = false;
                            $scope.chckedIndexs3.splice($scope.chckedIndexs3.indexOf(revtst3), 1);
                        }

                    }
                }
                else {


                    for (var i = 0; i < $scope.classsectionList.length; i++) {
                        if ($scope.classsectionList[i].name == revtst3.name) {
                            $scope.classsectionList[i].Selectedcsm = false;
                            $scope.resultclasssectionData.splice($scope.resultclasssectionData.indexOf(revtst3), 1);
                            return;
                        }
                    }
                }
            }

        }
        $scope.testsub2 = function (data1, subjects) {
            if (data1.Selectedsub == true) {
                if ($scope.editflag == true) {
                    $scope.subjectresultData.splice($scope.subjectresultData);
                    $scope.subjectresultData.push(data1);

                    for (var i = 0; i < subjects.length; i++) {
                        var name = subjects[i].pamS_Id;
                        if (name == $scope.subjectresultData[0].pamS_Id) {
                            subjects[i].Selectedsub = true;
                            $scope.subjectresultData[0].Selectedsub1 = true;
                        } else {
                            subjects[i].Selectedsub = false;
                        }

                    }
                }
                else {
                    //$scope.checkedIndexes2.push(data);
                    //$scope.subjectresultData = $scope.checkedIndexes2;

                    $scope.checkedIndexes2.push(data1);
                    $scope.subjectresultData = $scope.checkedIndexes2;
                    for (var i = 0; i < $scope.checkedIndexes2.length; i++) {
                        if ($scope.checkedIndexes2[i].Selectedsub == true) {
                            $scope.subjectresultData[i].Selectedsub1 = true;
                        } else {
                            $scope.subjectresultData[i].Selectedsub1 = false;
                        }
                    }
                }

                //  $scope.checkedIndexes2.push(data);
            }
            else {
                $scope.checkedIndexes2.splice($scope.checkedIndexes2.indexOf(data1), 1);
            }
        };
        $scope.revtestsub2 = function (revdata) {
            if (revdata.Selectedsub1 == false) {
                if ($scope.editflag == true) {
                    for (var i = 0; i < $scope.subjects.length; i++) {
                        var name = $scope.subjects[i].pamS_Id;
                        if (name == $scope.subjectresultData[0].pamS_Id) {
                            $scope.subjects[i].Selectedsub = false;
                            $scope.checkedIndexes2.splice($scope.checkedIndexes2.indexOf(revdata), 1);
                        }

                    }
                }
                else {
                    for (var i = 0; i < $scope.subjects.length; i++) {
                        if ($scope.subjects[i].pamS_Id == revdata.pamS_Id) {
                            $scope.subjects[i].Selectedsub = false;
                            $scope.subjectresultData.splice($scope.subjectresultData.indexOf(revdata), 1);
                            return;
                        }
                    }
                }
            }
        }
        $scope.revtest = function (revtesdata) {
            if (revtesdata.Selected1 == false) {
                if ($scope.editflag == true) {
                    for (var i = 0; i < $scope.classsectionList.length; i++) {
                        var name = $scope.classsectionList[i].name;
                        if (name == $scope.resultData[0].name) {
                            $scope.classsectionList[i].Selected = false;
                            $scope.chckedIndexs.splice($scope.chckedIndexs.indexOf(revtesdata), 1);
                        }

                    }
                }
                else {
                    for (var i = 0; i < $scope.classsectionList.length; i++) {
                        if ($scope.classsectionList[i].name == revtesdata.name) {
                            $scope.classsectionList[i].Selected = false;
                            $scope.resultData.splice($scope.resultData.indexOf(revtesdata), 1);
                            return;
                        }
                    }
                }
            }
        }
        $scope.obj = {};
        $scope.SaveLoginPriviledges = function (obj) {
            $scope.submitted = true;
            if ($scope.isExpanded == true) {
                if ($scope.type == 1) {
                    if (($scope.resultclasssectionData.length > 1) || ($scope.resultData.length > 1) || ($scope.subjectresultData.length > 1)) {
                        swal("Only One Record Should Be Selected");
                        return;
                    }
                    else {
                        $scope.isExpanded = false;
                    }
                }
                if ($scope.type == 2 || $scope.type == 3) {
                    if (($scope.resultclasssectionData.length > 1) || ($scope.resultData.length > 1)) {
                        swal("Only One Record Should Be Selected");
                        return;
                    }
                    else {
                        $scope.isExpanded = false;
                    }
                }
            }

            if ($scope.myForm.$valid && $scope.isExpanded == false) {

                if ($scope.type == 1) {
                    if ($scope.resultData.length == 0) {
                        swal("Please Select At Least One Class-Section Option");
                        return;
                    }
                    else {
                        $scope.clssectionlist = $scope.resultData;
                    }
                    if ($scope.selectedcls_sec_subs.length == 0) {
                        swal("Please Select At Least One Subject Option");
                        return;
                    }
                }
                if ($scope.type == 2 || $scope.type == 3) {
                    if ($scope.resultclasssectionData.length == 0) {
                        swal("Please Select At Least One Class-Section Option");
                        return;
                    }
                    else {
                        $scope.clssectionlist = $scope.resultclasssectionData;
                    }
                }
                var data = {
                    "ASALUC_Id": $scope.asaluC_Id,
                    "ASALU_Id": $scope.asalU_Id,
                    "ASALUCS_Id": $scope.asalucS_Id,
                    "ASALU_EntryTypeFlag": $scope.type,
                    "ASALU_Att_Exam_Flag": "A",
                    "HRME_ID": obj.IVRMUL_Id.userId,
                    "classsectionList1": $scope.clssectionlist,
                    "subjectsList": $scope.subjectresultData,
                    "selectedcls_sec_subs": $scope.selectedcls_sec_subs,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("AttendanceLP/save", data).then(function (promise) {
                    $scope.loginPData = promise.loginPData;
                    if (promise.returnval == true && promise.operation == "updated") {
                        if (promise.message != "" && promise.message != null) {
                            swal("Data Updated Successfully", promise.message);
                        } else {
                            swal("Data Updated Successfully");
                        }
                        $state.reload();
                    }
                    else if (promise.returnval == true && promise.operation == "saved") {
                        if (promise.message != "" && promise.message != null) {
                            swal("Data Saved Successfully", promise.message);
                        } else {
                            swal("Data Saved Successfully");
                        }

                        $state.reload();
                    }
                    else if (promise.message == "" && promise.message == null && promise.returnval == false) {
                        swal("Failed to save/update");
                        $state.reload();
                    }
                    else {
                        swal(promise.message);
                        $state.reload();
                    }
                });
            }
        }

        $scope.edit = function (id1, id2, id3, classsectionList, subjects) {
            $scope.isExpanded = true;
            $scope.labeldisable = false;
            $scope.editflag = true;
            var data = {
                "ASALU_Id": id1,
                "ASALUC_Id": id2,
                "ASALUCS_Id": id3
            }
            apiService.create("AttendanceLP/geteditdata/", data).then(function (promise) {


                if (promise.asalU_EntryTypeFlag == 2 || promise.asalU_EntryTypeFlag == 3) {

                    $scope.chckedIndexs3 = promise.resultclasssectionData;
                    $scope.resultclasssectionData = promise.resultclasssectionData;
                    $scope.classsectionList = promise.resultclasssectionData;
                    $scope.ASMAY_Id = $scope.classsectionList[0].asmaY_Id;
                    for (var i = 0; i < $scope.classsectionList.length; i++) {
                        var name = $scope.classsectionList[i].name;
                        if (name == $scope.resultclasssectionData[0].name) {
                            $scope.classsectionList[i].Selectedcsm = true;
                            $scope.resultclasssectionData[0].Selectedcsm1 = true;
                        } else {
                            $scope.classsectionList[i].Selectedcsm = false;
                        }
                    }

                    angular.forEach(promise.teacherList, function (dd) {
                        dd.IVRMUL_Id = dd.userId;
                    })

                    $scope.obj.IVRMUL_Id = promise.teacherList[0];
                }

                else {
                    $scope.chckedIndexs = promise.resultclasssectionData;
                    $scope.resultData = promise.resultclasssectionData;
                    $scope.ASMAY_Id = $scope.resultData[0].asmaY_Id;
                    $scope.subjectresultData = promise.subjectList;
                    $scope.checkedIndexes2 = promise.subjectList;
                    $scope.checkclssec = true;
                    //for class-sec
                    for (var i = 0; i < classsectionList.length; i++) {
                        var name = classsectionList[i].name;
                        if (name == $scope.resultData[0].name) {
                            classsectionList[i].Selected = true;
                            $scope.resultData[0].Selected1 = true;
                        } else {
                            classsectionList[i].Selected = false;
                        }
                    }
                    //for subject
                    for (var i = 0; i < subjects.length; i++) {
                        var name = subjects[i].ismS_Id;
                        if (name == $scope.subjectresultData[0].ismS_Id) {
                            subjects[i].Selectedsub = true;
                            $scope.subjectresultData[0].Selectedsub1 = true;
                        } else {
                            subjects[i].Selectedsub = false;
                        }
                    }
                }
                $scope.asalU_Id = promise.asalU_Id;
                $scope.asaluC_Id = promise.asaluC_Id;
                $scope.asalucS_Id = promise.asalucS_Id;
                $scope.type = promise.asalU_EntryTypeFlag;
                $scope.disable = $scope.type;
                //$scope.IVRMUL_Id = promise.teacherList[0].userId;

            })
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.tchetslist = function (IVRMUL_Id) {
            var data = {
                "HRME_ID": IVRMUL_Id.IVRMUL_Id.userId,
                "ASALU_EntryTypeFlag": $scope.type,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("AttendanceLP/staffwisegrid", data).then(function (promise) {
                $scope.loginPData = promise.loginPData;
                $scope.classsectionList = promise.classsectionList;
            });
        }

        $scope.DeleteAttPrivileges = function (attprvlges) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete the record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AttendanceLP/DeleteRecord", attprvlges).
                            then(function (promise) {
                                if (promise.message != "" && promise.message != null) {
                                    swal(promise.message);
                                }
                                else if (promise.returnval == true) {
                                    swal("Record Deleted Successfully");
                                    $state.reload();
                                }
                                else if (promise.returnval == false) {
                                    swal("Failed To Delete Record");
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        // $state.reload();
                    }
                });
        }

        $scope.selectedcls_sec_subs = [];
        $scope.submitted = false;
        //$scope.selectedcls_sec_subs.length == 0
        $scope.Mapsubject = function (obj) {

            //$scope.temp_selected_subs=[];
            if ($scope.myForm.$valid) {

                angular.forEach($scope.teachers, function (stf) {
                    if (obj.IVRMUL_Id.userId == stf.userId) {
                        $scope.empname = stf.userName;
                    }
                })
                angular.forEach($scope.classsectionList, function (cls_sec) {
                    if (cls_sec.Selected == true) {
                        $scope.selected_cls_sec = cls_sec.name;
                        $scope.selected_cls_sec_id = cls_sec.classsection;
                        $scope.selected_cls_id = cls_sec.asmcL_Id;
                        $scope.selected_sec_id = cls_sec.asmC_Id;
                        // $scope.temp_selected_cls_secs.push(cls_sec);
                        $scope.temp_selected_subs = [];
                        angular.forEach($scope.subjects, function (sub) {
                            if (sub.Selectedsub == true) {
                                var subid = sub.ismS_Id;
                                var subname = sub.ismS_SubjectName;
                                // $scope.temp_selected_subs.push(sub);
                                $scope.temp_selected_subs.push({ ISMS_Id: subid, ISMS_SubjectName: subname });
                            }
                        })

                        var already_cnt = 0;
                        if ($scope.selectedcls_sec_subs.length > 0) {
                            $scope.selected_cls_secflag = true;
                            angular.forEach($scope.selectedcls_sec_subs, function (cls_sec_subs) {
                                if (cls_sec_subs.cls_sec == $scope.selected_cls_sec && cls_sec_subs.cls_sec_id == $scope.selected_cls_sec_id) {
                                    already_cnt += 1;
                                    angular.forEach($scope.temp_selected_subs, function (sub1) {
                                        var alrdy_sub_cnt = 0;
                                        angular.forEach(cls_sec_subs.subs, function (sub2) {
                                            if (sub1.ISMS_Id == sub2.ISMS_Id) {
                                                alrdy_sub_cnt += 1;
                                            }
                                        })
                                        if (alrdy_sub_cnt == 0) {
                                            // cls_sec_subs.subs.push(sub1); 
                                            var subid = sub1.ISMS_Id;
                                            var subname = sub1.ISMS_SubjectName;
                                            cls_sec_subs.subs.push({ ISMS_Id: subid, ISMS_SubjectName: subname });
                                        }
                                    })
                                }
                            })
                        }
                        if (already_cnt == 0) {
                            $scope.selectedcls_sec_subs.push({ username: $scope.empname, cls_sec_id: $scope.selected_cls_sec_id, cls_sec: $scope.selected_cls_sec, subs: $scope.temp_selected_subs, asmcL_Id: $scope.selected_cls_id, asmC_Id: $scope.selected_sec_id });
                        }
                    }
                })
                $scope.clearpush_data();
            }
            else {
                $scope.submitted = true;
                swal("Please Select The Class Section And Subject")
            }
        };

        $scope.clearpush_data = function () {
            angular.forEach($scope.classsectionList, function (cls_sec) {
                cls_sec.Selected = false;
            })
            angular.forEach($scope.subjects, function (sub) {
                sub.Selectedsub = false;
            })
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.delete = function (row, index) {

            for (var x = 0; x < $scope.selectedcls_sec_subs.length; x++) {
                if (x == index) {
                    $scope.selectedcls_sec_subs.splice(x, 1);
                }
            }
        }

        $scope.is_cls_sec_required = function () {

            return !$scope.classsectionList.some(function (options) {
                return options.Selected;
            });
        }

        $scope.is_sub_required = function () {
            return !$scope.subjects.some(function (options) {
                return options.Selectedsub;
            });
        }
    }
})();
