(function () {
    'use strict';
    angular.module('app').controller('MarksEntrySubSubjectController', MarksEntrySubSubjectController)
    MarksEntrySubSubjectController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter']
    function MarksEntrySubSubjectController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.display = false;
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("ExamEntry/Getdetails").then(function (promise) {
                $scope.acdlist = promise.acdlist;
                $scope.ASMCL_Id = 'classdefualt';
                $scope.ASMS_Id = 'sectiondefualt';
                $scope.EME_Id = 'examdefualt';
                $scope.ISMS_Id = 'subjectdefualt';
            });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            $scope.display = false;
            $scope.subsubjectlist = [];
            $scope.subexamlist = [];
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("ExamEntry/onselectAcdYear", data).then(function (promise) {
                $scope.ctlist = promise.ctlist;
                $scope.ASMCL_Id = 'classdefualt';
            });
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            $scope.subsubjectlist = [];
            $scope.subexamlist = [];
            $scope.display = false;
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "ASMCL_Id": ASMCL_Id
            };
            apiService.create("ExamEntry/onselectclass", data).then(function (promise) {
                $scope.seclist = promise.seclist;
                $scope.ASMS_Id = 'sectiondefualt';
            });
        };

        $scope.onselectSection = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            $scope.subsubjectlist = [];
            $scope.subexamlist = [];
            $scope.display = false;
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamEntry/onselectSection", data).then(function (promise) {
                $scope.EME_Id = "";
                $scope.examlist = promise.examlist;
                $scope.EME_Id = 'examdefualt';
                $scope.subjectlist = promise.subjectlist;
                $scope.ISMS_Id = 'subjectdefualt';
            });
        };

        $scope.onselectExam = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id) {
            $scope.subsubjectlist = [];
            $scope.subexamlist = [];
            $scope.display = false;
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EME_Id": EME_Id
            };
            apiService.create("ExamEntry/onselectExam", data).then(function (promise) {
                $scope.subjectlist = promise.subjectlist;
            });
        };

        $scope.onselectSubject = function () {
            $scope.subsubjectlist = [];
            $scope.subexamlist = [];
            $scope.display = false;
            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "EME_Id": $scope.EME_Id,
                "ISMS_Id": $scope.ISMS_Id
            };
            apiService.create("ExamEntry/onselectSubject", data).then(function (promise) {
                if (promise.subsubjectlist !== null && promise.subsubjectlist.length > 0) {
                    $scope.subsubjectlist = promise.subsubjectlist;
                } else if (promise.subexamlist !== null && promise.subexamlist.length > 0) {
                    $scope.subexamlist = promise.subexamlist;
                }
            });
        };

        $scope.onchangesubsubject = function () {
            $scope.subexamlist = [];
            $scope.display = false;
            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "EME_Id": $scope.EME_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "EMSS_Id": $scope.obj.EMSS_Id
            };
            apiService.create("ExamEntry/onchangesubsubject", data).then(function (promise) {
                if (promise.subexamlist !== null && promise.subexamlist.length > 0) {
                    $scope.subexamlist = promise.subexamlist;
                }
            });
        };

        $scope.onchangesubexam = function () {
            $scope.display = false;
        };
        $scope.obj = {};

        $scope.onsearch = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id, EMSS_Id, EMSE_Id, obj) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.ASMCL_Id === 'classdefualt' || $scope.ASMS_Id === 'sectiondefualt' || $scope.EME_Id === 'examdefualt' || $scope.ISMS_Id === 'subjectdefualt') {
                    if ($scope.ASMCL_Id === 'classdefualt') {
                        swal('Please Select Class');
                        return;
                    }
                    else if ($scope.ASMS_Id === 'sectiondefualt') {
                        swal('Please Select Section');
                        return;
                    }
                    else if ($scope.EME_Id === 'examdefualt') {
                        swal('Please Select Exam');
                        return;
                    }
                    else if ($scope.ISMS_Id === 'subjectdefualt') {
                        swal('Please Select Subject');
                        return;
                    }
                }
                else {
                    var subsubjectid = 0;
                    if ($scope.subsubjectlist !== null && $scope.subsubjectlist.length > 0) {
                        subsubjectid = obj.EMSS_Id;
                    } else {
                        subsubjectid = 0;
                    }
                    var subexamid = 0;
                    if ($scope.subexamlist !== null && $scope.subexamlist.length > 0) {
                        subexamid = obj.EMSE_Id;
                    } else {
                        subexamid = 0;
                    }

                    var data = {
                        "ASMS_Id": ASMS_Id,
                        "ASMCL_Id": ASMCL_Id,
                        "ASMAY_Id": ASMAY_Id,
                        "EME_Id": EME_Id,
                        "ISMS_Id": ISMS_Id,
                        "EMSS_Id": subsubjectid,
                        "EMSE_Id": subexamid
                    };
                    apiService.create("ExamEntry/onsearch", data).then(function (promise) {
                        $scope.student = promise.studentList;

                        if (promise.subMorGFlag === "G") {
                            angular.forEach(promise.studentList, function (dd) {
                                dd.obtainmarks = dd.estM_Grade;
                            });
                        }
                        else {
                            angular.forEach(promise.studentList, function (dd) {
                                var sliptmarks = dd.obtainmarks.split('.');
                                if (sliptmarks[0] >= 1 && sliptmarks[0] < 10) {
                                    dd.obtainmarks = '0' + dd.obtainmarks;
                                }
                            });
                        }

                        $scope.subMorGFlag = promise.subMorGFlag;
                        $scope.gradname = promise.gradname;
                        $scope.marksdeleteflag = promise.marksdeleteflag;
                        $scope.update = promise.saveupdatecount;
                        $scope.eyceidlastdateentry = promise.lastdateentry;
                        $scope.savemarksbutton = promise.lastdateentryflag;
                        $scope.masterinst = promise.configuration;

                        $scope.colarrayall = [];
                        var count = 0;

                        $scope.colarrayall = [
                            { name: 'SLNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                            { name: 'studentname', displayName: 'Student Name', width: 300, cellClass: 'textleft' }
                        ];

                        if ($scope.masterinst !== null && $scope.masterinst.length > 0) {

                            if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay === true) {
                                $scope.colarrayall.push({ field: 'amsT_AdmNo', displayName: 'Adm.NO', width: 100 });
                                $scope.admno = true;
                                count = count + 1;
                            } else {
                                $scope.admno = false;
                            }

                            if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay === true) {
                                $scope.colarrayall.push({ field: 'amsT_RegistrationNo', displayName: 'Reg.NO', width: 100 });
                                $scope.regno = true;
                                count = count + 1;
                            } else {
                                $scope.regno = false;
                            }

                            if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay === true) {
                                $scope.colarrayall.push({ field: 'amaY_RollNo', displayName: 'Roll.NO', width: 100 });
                                $scope.rollno = true;
                                count = count + 1;
                            } else {
                                $scope.rollno = false;
                            }

                            if (count === 0) {
                                $scope.colarrayall.push({ field: 'amsT_AdmNo', displayName: 'Adm.NO', width: 100 });
                                $scope.colarrayall.push({ field: 'amaY_RollNo', displayName: 'Roll.NO', width: 100 });
                                $scope.admno = true;
                                $scope.rollno = true;
                            }
                        } else {
                            $scope.colarrayall.push({ field: 'amsT_AdmNo', displayName: 'Adm.NO', width: 100 });
                            $scope.colarrayall.push({ field: 'amaY_RollNo', displayName: 'Roll.NO', width: 100 });
                            $scope.admno = true;
                            $scope.rollno = true;
                        }

                        $scope.colarrayall.push({ name: 'totalMarks', displayName: 'Max Marks', width: 100 });
                        $scope.colarrayall.push({ name: 'minMarks', displayName: 'Min Marks', width: 100 });
                        $scope.colarrayall.push({ name: 'marksEnterFor', displayName: 'Marks Enter For', width: 120 });
                        $scope.colarrayall.push({ name: 'obtainmarks', displayName: 'Obtain Marks', cellTemplate: '<div class="ui-grid-cell-contents"><input type="text" ng-model="row.entity.obtainmarks"  style="text-align:center;" allow-pattern="[A-Z0-9+-.]" ng-blur="grid.appScope.changemarks(row.entity.marksEnterFor,row.entity.obtainmarks,row)"  class="form-control" value="0"  min="0" </input></div>' });


                        console.log($scope.colarrayall);

                        $scope.gridOptions = {
                            enableColumnMenus: false,
                            enableFiltering: true,
                            enableHorizontalScrollbar: 0,
                            enableVerticalScrollbar: 0,
                            columnDefs: $scope.colarrayall
                        };

                        $scope.gridOptions.data = promise.studentList;

                        $scope.gridOptions.onRegisterApi = function (gridApi) {
                            $scope.gridApi = gridApi;
                        };
                        $scope.display = true;

                        $scope.lastdateexamflag = promise.lastdateexamflag;

                        if ($scope.lastdateexamflag === false) {
                            $scope.display = false;
                            $scope.marksentrystatedate = promise.marksentrystatedate;
                            var dated = $filter('date')(new Date($scope.marksentrystatedate), 'MMM dd yyyy');
                            swal("From " + dated + " You Can Start Enter The Marks");
                        } else {
                            if ($scope.savemarksbutton === false) {
                                swal("Marks Entry Last Date Completed You Can Not Enter Marks");
                            }
                        }
                    });
                }
            }
        };

        $scope.changemarks = function (totalMarks, obtainmarks, row) {
            var flag = "";
            if ($scope.subMorGFlag === "G") {

                flag = "false";

                for (var i = 0; i < $scope.gradname.length; i++) {
                    if ($scope.gradname[i] == obtainmarks) {
                        flag = "true";
                    }
                }

                if (obtainmarks === "AB" || obtainmarks === "ab" || obtainmarks === "L" || obtainmarks === "l" || obtainmarks === "M" || obtainmarks === "m") {
                    flag = "true";
                }

                if (flag === "false") {
                    row.entity.obtainmarks = 0;
                    swal('Entered Grade cant be out of master setting...!');
                }
            }
            else {
                flag = "false";
                if (obtainmarks.match(/[a-z]/i)) {
                    if (obtainmarks === "AB" || obtainmarks === "ab" || obtainmarks === "L" || obtainmarks === "l" || obtainmarks === "M" || obtainmarks === "m") {
                        flag = "true";
                    }
                    if (flag === "false") {
                        row.entity.obtainmarks = 0;
                        swal('Entered value cant be out of master setting...!');
                    }
                }
                else {
                    if (totalMarks < obtainmarks) {
                        row.entity.obtainmarks = 0;
                        swal('Entered marks cant be more than Marks Enter For...!');
                    }
                    if (obtainmarks < 0) {
                        row.entity.obtainmarks = 0;
                        swal('Entered marks cant be in nagative values...!');
                    }
                }
            }
        };

        $scope.save = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id, EMSS_Id, EMSE_Id, obj) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.savedata = $scope.gridOptions.data;

                var subsubjectid = 0;
                if ($scope.subsubjectlist !== null && $scope.subsubjectlist.length > 0) {
                    subsubjectid = $scope.obj.EMSS_Id;
                } else {
                    subsubjectid = 0;
                }
                var subexamid = 0;
                if ($scope.subexamlist !== null && $scope.subexamlist.length > 0) {
                    subexamid = $scope.obj.EMSE_Id;
                } else {
                    subexamid = 0;
                }


                var data = {
                    "ASMS_Id": ASMS_Id,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMAY_Id": ASMAY_Id,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id,
                    "marksdeleteflag": $scope.marksdeleteflag,
                    "detailsList": $scope.savedata,
                    "EMSS_Id": subsubjectid,
                    "EMSE_Id": subexamid
                };
                apiService.create("ExamEntry/SaveMarks", data).then(function (promise) {
                    if (promise.messagesaveupdate === "true") {
                        $scope.cancle();
                        swal('Data Saved Successfully');
                    }
                    else {
                        swal('Failed to Save/Update Data');
                    }
                });
            }
        };

        $scope.SaveMarks = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var flag = "false";

                if ($scope.marksdeleteflag === "true") {
                    swal({
                        title: "Are You Sure?",
                        text: "Marks Already Calculated, If You Update The Marks You Need To Recalculate Marks Again..Do You Want To Continue ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update It!",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.save(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false";
                            }
                        });
                }
                else {
                    $scope.save(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }
            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };

        $scope.cancel = function () {
            $state.reload();
            $scope.BindData();
        };

        $scope.cancle = function () {
            $scope.acdlist = $scope.acdlist;
            $scope.ctlist = $scope.ctlist;
            $scope.seclist = $scope.seclist;
            $scope.examlist = $scope.examlist;
            $scope.subjectlist = $scope.subjectlist;
            $scope.gridOptions.data = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.submitted = false;
    }
})();