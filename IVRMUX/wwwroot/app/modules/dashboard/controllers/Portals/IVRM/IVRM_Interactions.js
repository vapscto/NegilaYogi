
(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_InteractionsController', IVRM_InteractionsController);
    IVRM_InteractionsController.$inject = ['$http', '$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$timeout', '$sce', '$q'];
    function IVRM_InteractionsController($http, $rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $interval, $timeout, $sce, $q) {

        $scope.isminT_DateTime = new Date();
        $scope.iintsS_Date = new Date();
        $scope.composetab = false;
        $scope.inboxtab = true;
        $scope.composehead = false;
        //Auto Generate Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.obj = {};
        $scope.obj.searchValue = "";
        $scope.obj.search1 = "";

        $scope.oninbox = function () {
            $scope.composetab = false;
            $scope.composehead = false;
            $scope.inboxtab = true;
            $scope.obj.userflag = false;
        };
        $scope.typechangeG = function () {
            $scope.ISMINT_GroupOrIndFlg = "Group";
            $scope.groupindvalue = true;
            $scope.asmcL_Id = "";
            $scope.get_student = "";
            $scope.composetab = true;
            $scope.composehead = true;
            $scope.inboxtab = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.obj.userflag = false;
        };
        $scope.typechangeI = function () {
            $scope.ISMINT_GroupOrIndFlg = "Individual";
            $scope.groupindvalue = false;
            $scope.asmcL_Id = "";
            $scope.get_student = "";
            $scope.composetab = true;
            $scope.composehead = true;
            $scope.inboxtab = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.obj.userflag = false;


        };

        $scope.Groupview = false;

        $scope.chatwithstudent = false;
        $scope.chatwithstaff = false;
        $scope.refresh = function () {
            $scope.loaddata();
        };
        $scope.Clearid = function () {
            $state.reload();
        }
        //===================Page Load
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.search1 = "";
            apiService.getURI("IVRM_Interactions/getloaddata", pageid).
                then(function (promise) {
                    $scope.roletype = promise.roletype;
                    $scope.roleflg = $scope.roletype[0].ivrmrT_Role;

                    //if ($scope.roleflg != 'Staff' && $scope.roleflg != 'Student') {
                    //    $scope.roleflg = 'Staff';
                    //}

                    $scope.configflag = promise.configflag;
                    if ($scope.configflag != null) {
                        if ($scope.configflag[0].ivrmgC_EnableSTIntFlg != null) {
                            $scope.stflag = $scope.configflag[0].ivrmgC_EnableSTIntFlg;
                        }
                        if ($scope.configflag[0].ivrmgC_EnableCTIntFlg != null) {
                            $scope.ctflag = $scope.configflag[0].ivrmgC_EnableCTIntFlg;
                        }
                        if ($scope.configflag[0].ivrmgC_EnableHODIntFlg != null) {
                            $scope.hodflag = $scope.configflag[0].ivrmgC_EnableHODIntFlg;
                        }
                        if ($scope.configflag[0].ivrmgC_EnablePrincipalIntFlg != null) {
                            $scope.principalflag = $scope.configflag[0].ivrmgC_EnablePrincipalIntFlg;
                        }
                        if ($scope.configflag[0].ivrmgC_EnableASIntFlg != null) {
                            $scope.asflag = $scope.configflag[0].ivrmgC_EnableASIntFlg;
                        }

                    }



                    //$scope.ecflag = $scope.configflag[0].ivrmgC_EnableECIntFlg;

                    //STUDENT CONFIG
                    $scope.STforStudent = $scope.configflag[0].ivrmgC_EnableSTSUBTIntFlg;
                    $scope.CTforStudent = $scope.configflag[0].ivrmgC_EnableSTCTIntFlg;
                    $scope.HODforStudent = $scope.configflag[0].ivrmgC_EnableSTHODIntFlg;
                    $scope.PRPLforStudent = $scope.configflag[0].ivrmgC_EnableSTPrincipalIntFlg;
                    $scope.ASforStudent = $scope.configflag[0].ivrmgC_EnableSTASIntFlg;
                    $scope.ExCoforStudent = $scope.configflag[0].ivrmgC_EnableSTECIntFlg;


                    //STAFF
                    $scope.STFtoSTF = $scope.configflag[0].ivrmgC_EnableStaffwiseIntFlg;
                    $scope.CLTtoSTU = $scope.configflag[0].ivrmgC_EnableCTSTIntFlg;
                    $scope.HODtoSTU = $scope.configflag[0].ivrmgC_EnableHODSTIntFlg;
                    $scope.PRItoSTU = $scope.configflag[0].ivrmgC_EnablePrincipalSTIntFlg;
                    $scope.ACStoSTU = $scope.configflag[0].ivrmgC_EnableASSTIntFlg;
                    $scope.ECtoSTU = $scope.configflag[0].ivrmgC_EnableECSTIntFlg;
                    $scope.SUBTtoSTU = $scope.configflag[0].ivrmgC_EnableSUBTSTUIntFlg;

                    $scope.Groupview = $scope.configflag[0].ivrmgC_GMRDTOALLFlg;


                    $scope.chatwithstudent = false;
                    $scope.chatwithstaff = false;


                    if (promise.rolename != 'student') {
                        if ($scope.STFtoSTF == true) {
                            $scope.chatwithstaff = true;
                        }
                        if (promise.rolename == 'principal') {
                            if ($scope.PRItoSTU == true) {
                                $scope.chatwithstudent = true;
                            }
                        }
                        if (promise.rolename == 'hod') {
                            if ($scope.HODtoSTU == true) {
                                $scope.chatwithstudent = true;
                            }
                        }

                        if (promise.classteacherlist != null && promise.classteacherlist.length > 0) {

                            if ($scope.CLTtoSTU == true) {
                                $scope.chatwithstudent = true;
                            }

                        }

                        if (promise.subteacherlist != null && promise.subteacherlist.length > 0) {
                            if ($scope.SUBTtoSTU == true) {
                                $scope.chatwithstudent = true;
                            }
                        }

                        if (promise.typelist != null && promise.typelist.length > 0) {
                            angular.forEach(promise.typelist, function (rr) {
                                if (rr.ihoD_Flg.toLowerCase() == 'hod') {
                                    if ($scope.HODtoSTU == true) {
                                        $scope.chatwithstudent = true;
                                    }

                                }
                                if (rr.ihoD_Flg.toLowerCase() == 'as') {
                                    if ($scope.ACStoSTU == true) {
                                        $scope.chatwithstudent = true;
                                    }

                                }
                                if (rr.ihoD_Flg.toLowerCase() == 'ec') {
                                    if ($scope.SUBTtoSTU == true) {
                                        $scope.chatwithstudent = true;
                                    }

                                }

                            })
                        }


                    }




                    $scope.getinboxmsg = promise.getinboxmsg;
                    $scope.getinboxmsg_readflg = promise.getinboxmsg_readflg;
                    $scope.hidec = 0;
                    if (promise.userhrmE_Id > 0) {
                        $scope.userhrmE_Id = promise.userhrmE_Id;
                    }

                    $scope.message_list = [];
                    angular.forEach($scope.getinboxmsg, function (mg) {
                        if ($scope.message_list.length === 0) {
                            $scope.message_list.push({
                                ISMINT_Id: mg.ISMINT_Id,
                                ISTINT_Id: mg.ISTINT_Id,
                                ISMINT_InteractionId: mg.ISMINT_InteractionId,
                                ISMINT_Subject: mg.ISMINT_Subject,
                                ISMINT_Interaction: mg.ISMINT_Interaction,
                                ISMINT_GroupOrIndFlg: mg.ISMINT_GroupOrIndFlg,
                                ISMINT_ComposedByFlg: mg.ISMINT_ComposedByFlg,
                                Sender: mg.Sender, Receiver: mg.Receiver,
                                ISMINT_DateTime: mg.ISTINT_DateTime,
                                ISTINT_Attachment: mg.ISTINT_Attachment,
                                ISMINT_Attachment: mg.ISMINT_Attachment,
                                ISMINT_ComposedById: mg.ISMINT_ComposedById,
                                ISTINT_ReadFlg: mg.ISTINT_ReadFlg
                            });
                        }
                        else if ($scope.message_list.length > 0) {
                            var al_cnt = 0;
                            angular.forEach($scope.message_list, function (sl) {
                                if (sl.ISMINT_Id === mg.ISMINT_Id) {
                                    al_cnt += 1;
                                }
                            });
                            if (al_cnt === 0) {
                                //   if (al_cnt > 0) {
                                $scope.message_list.push({
                                    ISMINT_Id: mg.ISMINT_Id,
                                    ISTINT_Id: mg.ISTINT_Id,
                                    ISMINT_InteractionId: mg.ISMINT_InteractionId,
                                    ISMINT_Subject: mg.ISMINT_Subject,
                                    ISMINT_Interaction: mg.ISMINT_Interaction,
                                    ISMINT_GroupOrIndFlg: mg.ISMINT_GroupOrIndFlg,
                                    ISMINT_ComposedByFlg: mg.ISMINT_ComposedByFlg,
                                    Sender: mg.Sender, Receiver: mg.Receiver,
                                    ISMINT_DateTime: mg.ISTINT_DateTime,
                                    ISTINT_Attachment: mg.ISTINT_Attachment,
                                    ISMINT_Attachment: mg.ISMINT_Attachment,

                                    ISMINT_ComposedById: mg.ISMINT_ComposedById,
                                    ISTINT_ReadFlg: mg.ISTINT_ReadFlg
                                });
                            }
                        }

                        $scope.inboxtotal = $scope.message_list.length;
                    });


                        if ($scope.message_list.length > 0) {
                        angular.forEach($scope.message_list, function (qq) {

                            if (qq.ISMINT_Attachment != undefined || qq.ISMINT_Attachment != '' || qq.ISMINT_Attachment != null) {
                                var img = qq.ISMINT_Attachment;
                                if (img != null) {
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];

                                    qq.filetype2 = lastelement;
                                }

                            }
                        });
                    }


                    $scope.message_listread = [];
                    //added
                    var al_cnt = 0;
                    if ($scope.message_list != null && $scope.getinboxmsg_readflg.length > 0) {                        angular.forEach($scope.message_list, function (mg) {                                                        angular.forEach($scope.getinboxmsg_readflg, function (mgg) {                                if (mg.ISMINT_Id === mgg.ISMINT_Id) {                                    al_cnt += 1;                                    $scope.message_listread.push({
                                        ISMINT_Id: mg.ISMINT_Id, ISTINT_Id: mg.ISTINT_Id,
                                        ISMINT_InteractionId: mg.ISMINT_InteractionId,
                                        ISMINT_Subject: mg.ISMINT_Subject,
                                        ISMINT_Interaction: mg.ISMINT_Interaction,
                                        ISMINT_GroupOrIndFlg: mg.ISMINT_GroupOrIndFlg,
                                        ISMINT_ComposedByFlg: mg.ISMINT_ComposedByFlg,
                                        Sender: mg.Sender, Receiver: mg.Receiver,
                                        ISMINT_DateTime: mg.ISTINT_DateTime,
                                        ISTINT_Attachment: mg.ISTINT_Attachment,
                                       // ISMINT_Attachment: mg.ISMINT_Attachment,
                                        ISMINT_ComposedById: mg.ISMINT_ComposedById,
                                        ISTINT_ReadFlg: mgg.ISTINT_ReadFlg

                                    });                                }                                                           });                            if (al_cnt === 0) {                                $scope.message_listread.push({                                    ISMINT_Id: mg.ISMINT_Id, ISTINT_Id: mg.ISTINT_Id,
                                    ISMINT_InteractionId: mg.ISMINT_InteractionId,
                                    ISMINT_Subject: mg.ISMINT_Subject,
                                    ISMINT_Interaction: mg.ISMINT_Interaction,
                                    ISMINT_GroupOrIndFlg: mg.ISMINT_GroupOrIndFlg,
                                    ISMINT_ComposedByFlg: mg.ISMINT_ComposedByFlg,
                                    Sender: mg.Sender, Receiver: mg.Receiver,
                                    ISMINT_DateTime: mg.ISTINT_DateTime,
                                    ISTINT_Attachment: mg.ISTINT_Attachment,
                                   // ISMINT_Attachment: mg.ISMINT_Attachment,
                                    ISMINT_ComposedById: mg.ISMINT_ComposedById,
                                    ISTINT_ReadFlg: 0                                });                            }                                                                                 //$scope.inboxtotal = $scope.message_listread.length;                        });                        if ($scope.message_list.length > 0) {
                            angular.forEach($scope.message_listread, function (mg) {

                                if (mg.ISTINT_Attachment != undefined || mg.ISTINT_Attachment != '' || mg.ISTINT_Attachment != null) {
                                    var img = mg.ISTINT_Attachment;
                                    if (img != null) {
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];

                                        mg.filetype2 = lastelement;
                                    }

                                }
                            });
                        }                                         }

                });


        };


        //===================On Radio Change
        $scope.getdetail = function () {
            $scope.empName = "";
            $scope.hrmE_Id = "";
            $scope.usercheckSTU = "";
            $scope.usercheckST = "";
            $scope.usercheckT = "";
            $scope.sectionList = "";
            $scope.getdetails = "";
            $scope.get_student = "";
            var data = {
                "roleflg": $scope.roleflg,
                "userflag": $scope.obj.userflag
            };
            apiService.create("IVRM_Interactions/getdetails", data).
                then(function (promise) {
                    $scope.configflag = promise.configflag;
                    if (promise.getdetails.length > 0 || promise.getdetails != null) {
                        if ($scope.obj.userflag == "SubjectTeacher") {
                            $scope.getdetails = promise.getdetails;

                        }
                        else {
                            $scope.getdetails = promise.getdetails;
                            $scope.empName = promise.getdetails[0].EmpName;
                            $scope.hrmE_Id = promise.getdetails[0].HRME_Id;
                            $scope.asmay_id = promise.getdetails[0].ASMAY_Id;

                        }
                        $scope.hidec = 0;
                    }
                    else {
                        swal("No Record Found....");
                        $scope.getdetails.length = 0;
                    }
                });
        };

        //========================= Class Change
        $scope.getsection = function (asmcL_Id) {
            $scope.get_student = "";
            $scope.asmcL_Id = asmcL_Id;
            var data = {
                "asmcL_Id": asmcL_Id,
                "ASMAY_Id": $scope.asmay_id,
                "roleflg": $scope.roleflg,
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("EmployeeStudentAttendenceDetails/Getsection", data).
                then(function (promise) {
                    $scope.obj.asmS_Id = '';
                    if (promise.sectionList != null && promise.sectionList.length > 0) {
                        $scope.sectionList = promise.sectionList;
                    }
                    else {
                        $scope.get_student = "";
                    }
                });
        };
        //=================== getstudent on Class Change
        $scope.getstudent = function () {
            $scope.get_student = "";
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.obj.asmS_Id
            };
            apiService.create("IVRM_Interactions/getstudent", data).
                then(function (promise) {
                    if (promise.get_student !== null && promise.get_student.length > 0) {
                        $scope.get_student = promise.get_student;
                    }
                    else {
                        swal("No Record Found....");
                    }
                });
        };

        //======================== Select All and multiselect validation
        //==== Student
        $scope.togchkbxSTU = function () {
            $scope.usercheckSTU = $scope.get_student.every(function (itm) {
                return itm.stuck;
            });
        };
        $scope.isOptionsRequiredSTU = function () {
            if ($scope.obj.userflag === "Student" && $scope.groupindvalue === true) {
                return !$scope.get_student.some(function (options) {
                    return options.stuck;
                });
            }
        };
        $scope.all_checkSTU = function (subj) {
            $scope.usercheckSTU = subj;
            var toggleStatus = $scope.usercheckSTU;
            angular.forEach($scope.get_student, function (st) {
                st.stuck = toggleStatus;
            });
        };
        //==== Teacher
        $scope.togchkbxT = function () {
            $scope.usercheckT = $scope.getdetails.every(function (itm) {
                return itm.tck;
            });
        };
        $scope.isOptionsRequiredT = function () {
            if ($scope.obj.userflag === "Teachers" && $scope.groupindvalue === true) {
                return !$scope.getdetails.some(function (options) {
                    return options.tck;
                });
            }
        };
        $scope.all_checkT = function (subj) {
            $scope.usercheckT = subj;
            var toggleStatus = $scope.usercheckT;
            angular.forEach($scope.getdetails, function (gd) {
                gd.tck = toggleStatus;
            });
        };
        //==== Subject Teacher
        $scope.togchkbxST = function () {
            $scope.usercheckST = $scope.getdetails.every(function (itm) {
                return itm.stck;
            });
        };
        $scope.isOptionsRequiredST = function () {
            if ($scope.obj.userflag === "SubjectTeacher" && $scope.groupindvalue === true) {
                return !$scope.getdetails.some(function (options) {
                    return options.stck;
                });
            }
        };
        $scope.all_checkST = function (subj) {
            $scope.usercheckST = subj;
            var toggleStatus = $scope.usercheckST;
            angular.forEach($scope.getdetails, function (gd) {
                gd.stck = toggleStatus;
            });
        };
        //===============
        $scope.change = function (ss) {
            $scope.hrmE_Id = ss.hrmE_Id;
        }
        //===================Save Data / Send Message     
        $scope.savedata = function () {
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                if ($scope.roleflg === "HOD" || $scope.roleflg === "Principal" || $scope.roleflg === "AS" || $scope.roleflg === "EC") {
                    $scope.roleflg = "Staff";
                }

                if ($scope.roleflg === "Student") {
                    data = {
                        "roleflg": $scope.roleflg,
                        "userflag": $scope.obj.userflag,
                        "HRME_Id": $scope.hrmE_Id,
                        "ISTINT_ToId": $scope.hrmE_Id,
                        "ISMINT_ComposedByFlg": $scope.roleflg,
                        "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                        "ISMINT_Subject": $scope.obj.isminT_Subject,
                        "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                        "ISMINT_Id": $scope.isminT_Id,
                        "images_paths": $scope.images_paths
                    };
                }

                else if ($scope.roleflg === "Staff") {
                    if ($scope.obj.userflag === "Student") {
                        $scope.arrayStudent = [];
                        angular.forEach($scope.get_student, function (stu) {
                            if (stu.stuck === true) {
                                $scope.arrayStudent.push(stu);
                            }
                        });

                        if ($scope.ISMINT_GroupOrIndFlg === "Group") {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "arrayStudent": $scope.arrayStudent,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.obj.isminT_Subject,
                                "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id,
                                "images_paths": $scope.images_paths
                            };
                        }
                        else {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "student_Id": $scope.obj.amsT_Id.AMST_Id,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.obj.isminT_Subject,
                                "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id,
                                "images_paths": $scope.images_paths
                            };
                        }
                    }

                    else if ($scope.obj.userflag === "Teachers") {
                        $scope.arrayTeachers = [];
                        angular.forEach($scope.getdetails, function (tec) {
                            if (tec.tck === true) {
                                $scope.arrayTeachers.push(tec);
                            }
                        });
                        if ($scope.ISMINT_GroupOrIndFlg === "Group") {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "arrayTeachers": $scope.arrayTeachers,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.obj.isminT_Subject,
                                "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id,
                                "images_paths": $scope.images_paths
                            };
                        }
                        else {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "employee_Id": $scope.obj.hrmE_Id.HRME_Id,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.obj.isminT_Subject,
                                "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id,
                                "images_paths": $scope.images_paths
                            };
                        }
                    }

                    else if ($scope.obj.userflag === "HOD" || $scope.obj.userflag === "Principal" || $scope.obj.userflag === "AS" || $scope.obj.userflag === "EC") {
                        data = {
                            "roleflg": $scope.roleflg,
                            "userflag": $scope.obj.userflag,
                            "ISTINT_ToId": $scope.hrmE_Id,
                            "ISMINT_ComposedByFlg": $scope.roleflg,
                            "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                            "ISMINT_Subject": $scope.obj.isminT_Subject,
                            "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                            "ISMINT_Id": $scope.isminT_Id,
                            "images_paths": $scope.images_paths
                        };
                    }
                }

                else if ($scope.roleflg === "HOD") {
                    if ($scope.obj.userflag === "Student") {
                        $scope.arrayStudent = [];
                        angular.forEach($scope.get_student, function (stu) {
                            if (stu.stuck === true) {
                                $scope.arrayStudent.push(stu);
                            }
                        });

                        if ($scope.ISMINT_GroupOrIndFlg === "Group") {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "arrayStudent": $scope.arrayStudent,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.obj.isminT_Subject,
                                "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id,
                                "images_paths": $scope.images_paths
                            };
                        }
                        else {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "student_Id": $scope.obj.amsT_Id.AMST_Id,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.obj.isminT_Subject,
                                "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id,
                                "images_paths": $scope.images_paths
                            };
                        }
                    }

                    else if ($scope.obj.userflag === "Teachers") {
                        $scope.arrayTeachers = [];
                        angular.forEach($scope.getdetails, function (tec) {
                            if (tec.tck === true) {
                                $scope.arrayTeachers.push(tec);
                            }
                        });
                        if ($scope.ISMINT_GroupOrIndFlg === "Group") {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "arrayTeachers": $scope.arrayTeachers,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.obj.isminT_Subject,
                                "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id,
                                "images_paths": $scope.images_paths
                            };
                        }
                        else {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "employee_Id": $scope.obj.hrmE_Id.HRME_Id,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.obj.isminT_Subject,
                                "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id,
                                "images_paths": $scope.images_paths
                            };
                        }
                    }

                    else if ($scope.obj.userflag === "HOD" || $scope.obj.userflag === "Principal" || $scope.obj.userflag === "AS" || $scope.obj.userflag === "EC") {
                        data = {
                            "roleflg": $scope.roleflg,
                            "userflag": $scope.obj.userflag,
                            "ISTINT_ToId": $scope.hrmE_Id,
                            "ISMINT_ComposedByFlg": $scope.roleflg,
                            "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                            "ISMINT_Subject": $scope.obj.isminT_Subject,
                            "ISMINT_Interaction": $scope.obj.isminT_Interaction,
                            "ISMINT_Id": $scope.isminT_Id,
                            "images_paths": $scope.images_paths
                        };
                    }
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("IVRM_Interactions/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.isminT_Id === 0 || promise.isminT_Id < 0) {
                            swal('Message sent successfully');
                        }
                    }
                    else {
                        if (promise.isminT_Id === 0 || promise.isminT_Id < 0) {
                            swal('Failed to send, please contact administrator');
                        }
                        else if (promise.isminT_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.replyshow = false;
        //===================
        $scope.reply = function (objs) {
            $scope.replyshow = false;
            if (objs == undefined) {

                $scope.id = $scope.id;
                var data = {
                    "ISMINT_Id": $scope.id
                };
            }
            else {
                $scope.id = objs.ISMINT_Id;
                var data = {
                    "ISMINT_Id": $scope.id
                };
            }

            apiService.create("IVRM_Interactions/reply", data).
                then(function (promise) {



                    if (promise.rolename == 'student') {
                        if (promise.typelistrole != null && promise.typelistrole.length > 0) {
                            angular.forEach(promise.typelistrole, function (rr) {
                                if (rr.ivrmrT_Role.toLowerCase() == 'principal') {
                                    if ($scope.PRPLforStudent == true) {
                                        $scope.replyshow = true;
                                    }

                                }

                            })
                        }

                        if (promise.typelist != null && promise.typelist.length > 0) {
                            angular.forEach(promise.typelist, function (rr) {
                                if (rr.ihoD_Flg.toLowerCase() == 'hod') {
                                    if ($scope.HODforStudent == true) {
                                        $scope.replyshow = true;
                                    }

                                }
                                if (rr.ihoD_Flg.toLowerCase() == 'as') {
                                    if ($scope.ASforStudent == true) {
                                        $scope.replyshow = true;
                                    }

                                }
                                if (rr.ihoD_Flg.toLowerCase() == 'ec') {
                                    if ($scope.ExCoforStudent == true) {
                                        $scope.replyshow = true;
                                    }

                                }

                            })
                        }


                        if (promise.classteacherlist != null && promise.classteacherlist.length > 0) {


                            if ($scope.CTforStudent == true) {
                                $scope.replyshow = true;
                            }



                        }

                        if (promise.subteacherlist != null && promise.subteacherlist.length > 0) {
                            if ($scope.STforStudent == true) {
                                $scope.replyshow = true;
                            }



                        }
                    }
                    else {
                        if (promise.isminT_ComposedByFlg == 'student') {

                            if (promise.typelistrole != null && promise.typelistrole.length > 0) {
                                angular.forEach(promise.typelistrole, function (rr) {
                                    if (rr.ivrmrT_Role.toLowerCase() == 'principal') {
                                        if ($scope.PRItoSTU == true) {
                                            $scope.replyshow = true;
                                        }

                                    }

                                })
                            }

                            if (promise.typelist != null && promise.typelist.length > 0) {
                                angular.forEach(promise.typelist, function (rr) {
                                    if (rr.ihoD_Flg.toLowerCase() == 'hod') {
                                        if ($scope.HODtoSTU == true) {
                                            $scope.replyshow = true;
                                        }

                                    }
                                    if (rr.ihoD_Flg.toLowerCase() == 'as') {
                                        if ($scope.ACStoSTU == true) {
                                            $scope.replyshow = true;
                                        }

                                    }
                                    if (rr.ihoD_Flg.toLowerCase() == 'ec') {
                                        if ($scope.ECtoSTU == true) {
                                            $scope.replyshow = true;
                                        }

                                    }

                                })
                            }


                            if (promise.classteacherlist != null && promise.classteacherlist.length > 0) {


                                if ($scope.CLTtoSTU == true) {
                                    $scope.replyshow = true;
                                }



                            }

                            if (promise.subteacherlist != null && promise.subteacherlist.length > 0) {
                                if ($scope.SUBTtoSTU == true) {
                                    $scope.replyshow = true;
                                }



                            }



                        } else {

                            if (promise.composeedto == 'student') {

                                if (promise.typelistrole != null && promise.typelistrole.length > 0) {
                                    angular.forEach(promise.typelistrole, function (rr) {
                                        if (rr.ivrmrT_Role.toLowerCase() == 'principal') {
                                            if ($scope.PRItoSTU == true) {
                                                $scope.replyshow = true;
                                            }

                                        }

                                    })
                                }

                                if (promise.typelist != null && promise.typelist.length > 0) {
                                    angular.forEach(promise.typelist, function (rr) {
                                        if (rr.ihoD_Flg.toLowerCase() == 'hod') {
                                            if ($scope.HODtoSTU == true) {
                                                $scope.replyshow = true;
                                            }

                                        }
                                        if (rr.ihoD_Flg.toLowerCase() == 'as') {
                                            if ($scope.ACStoSTU == true) {
                                                $scope.replyshow = true;
                                            }

                                        }
                                        if (rr.ihoD_Flg.toLowerCase() == 'ec') {
                                            if ($scope.ECtoSTU == true) {
                                                $scope.replyshow = true;
                                            }

                                        }

                                    })
                                }


                                if (promise.classteacherlist != null && promise.classteacherlist.length > 0) {


                                    if ($scope.CLTtoSTU == true) {
                                        $scope.replyshow = true;
                                    }



                                }

                                if (promise.subteacherlist != null && promise.subteacherlist.length > 0) {
                                    if ($scope.SUBTtoSTU == true) {
                                        $scope.replyshow = true;
                                    }



                                }
                            }
                            else {
                                if ($scope.STFtoSTF == true) {
                                    $scope.replyshow = true;
                                }
                            }


                        }
                    }




                    if (promise.viewmessage !== null) {
                        $scope.viewmessage = promise.viewmessage;
                        $scope.subject = objs.ISMINT_Subject;
                        $scope.subjdetails = objs.ISMINT_Interaction;

                        //$scope.ISTINT_Attachment = objs.ISTINT_Attachment;

                        //var img1 = objs.ISTINT_Attachment.ISTINT_Attachment;
                        //var imagarr1 = img1.split('.');
                        //var lastelement1 = imagarr1[imagarr1.length - 1];
                        // $scope.filetype1 = lastelement1;

                        if (promise.viewmessage != null && promise.viewmessage.length > 0) {
                            angular.forEach($scope.viewmessage, function (ddd) {
                                ddd.filetype = "";
                                if (ddd.ISTINT_Attachment != undefined || ddd.ISTINT_Attachment != '' || ddd.ISTINT_Attachment != null) {
                                    var img = ddd.ISTINT_Attachment;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];

                                    ddd.filetype = lastelement;
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'pdf') {
                                        ddd.ISTINT_Attachment1 = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.ISTINT_Attachment;
                                    }
                                }
                            });
                        }

                    }
                });
        };

        $scope.replymsg = function () {
            angular.element('#viewrplyModal').modal('hide');
        };


        //===================Save Staff/Student Reply       
        $scope.savereply = function (rply) {
            $scope.submittedR = true;
            if ($scope.myFormR.$valid) {
                if ($scope.roleflg === "HOD" || $scope.roleflg === "Principal" || $scope.roleflg === "AS" || $scope.roleflg === "EC") {
                    $scope.roleflg = "Staff";
                }
                if ($scope.images_paths == undefined) {

                    var data = {
                        "ISMINT_Id": $scope.id,
                        "ISTINT_ComposedByFlg": $scope.roleflg,
                        "ISTINT_Interaction": $scope.istinT_Interaction,
                        "ISTINT_Id": $scope.istinT_Id


                    };
                }
                else {
                    var data = {
                        "ISMINT_Id": $scope.id,
                        "ISTINT_ComposedByFlg": $scope.roleflg,
                        "ISTINT_Interaction": $scope.istinT_Interaction,
                        "ISTINT_Id": $scope.istinT_Id,
                        "images_paths": $scope.images_paths

                    };
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("IVRM_Interactions/savereply", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.istinT_Id === 0 || promise.istinT_Id < 0) {
                            swal('Message sent successfully');
                        }
                    }
                    else {
                        if (promise.istinT_Id === 0 || promise.istinT_Id < 0) {
                            swal('Failed to send, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submittedR = true;
            }
        };

        //========================================shivu====================================
        //===============Delete
        $scope.deletemsg = function (reply) {
            var data = {
                "ISTINT_Id": reply.ISTINT_Id
            }

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete This Message?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_Interactions/deletemsg", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record Deleted Successfully!!!");
                                }
                                else if (promise.returnval == false) {
                                    swal("Record Not Deleted Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  Delete Cancelled!!!");
                    }
                    angular.element('#viewrplyModal').modal('hide');
                });
        }

        $scope.deleteinboxmsg = function (reply) {
            var data = {
                "ISMINT_Id": reply.ISMINT_Id
            }

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete This Message?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_Interactions/deleteinboxmsg", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record Deleted Successfully!!!");
                                }
                                else {
                                    swal("Record Not Deleted Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  Delete Cancelled!!!");
                    }
                    angular.element('#viewrplyModal').modal('hide');
                });
        }


        //================================ Image  
        $scope.stepsModel = [];
        $scope.imageUpload = function (event) {
            $scope.stepsModel = [];
            $scope.files = event.files;
            //for (var i = 0; i < $scope.files.length; i++) {
            //    var file = $scope.files[i];
            //    $scope.fileimg = file;
            //    var reader = new FileReader();
            //    reader.onload = $scope.imageIsLoaded;
            //    reader.readAsDataURL(file);
            //}
            for (var i = 0; i < 1; i++) {
                var file = $scope.files[i];
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL(file);
            }
            UploadImgs();
        };

        //added by roopa
        $scope.removeNewurl = function (index, data) {
            var newItemNo = $scope.urldocumentlist.length - 1;
            $scope.urldocumentlist.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {
            var formData = new FormData();
            $scope.stepsModel = [];
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_GalleryImgVideos", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.images_paths = d;
                        //data.INTBFL_FilePath = d[0].path;
                        data.intbfL_FilePath = d;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };
        //
        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };
        $scope.remove_img = function (reimg) {
            for (var i = 0; i < $scope.files.length; i++) {
                var imgt1 = $scope.files[i];
                if (imgt1.name === reimg.name) {
                    $scope.stepsModel.splice(i, 1);
                }
            }
        };
        $scope.imagepath = $scope.stepsModel[0];

        //================================ Upload       
        $scope.view_videos = [];
        function UploadImgs() {
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.files[i]);
                $scope.filenames = "Videos";
                $scope.fileflg = true;
            }
            //for (var i = 0; i <= $scope.files.length; i++) {
            //     formData.append("File", $scope.files[i]);
            //     $scope.filenames = "Videos";
            //     $scope.fileflg = true;
            // }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_GalleryImgVideos", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.images_paths = d;

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }


        //=============================== Preview Image
        $scope.previewimg = function (item) {
            //  $scope.imagepreview = img;

            // $scope.imagepreview = img;
            $scope.imagepreview = $scope.images_paths[0];
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {


                $('#preview').attr('src', $scope.imagepreview);
                $('#myModalPreview').modal('show');

            }

            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'pdf') {
                $scope.previewpdf(img, $scope.filetype2);
                // $('#showpdf').modal('show');

            }

        };
        //==================view phots
        $scope.viewphoto = function (reply) {
            //$scope.imagepreview1 = reply.ISTINT_Attachment;
            //$('#preview').attr('src', $scope.imagepreview1);

            //$('#myModalPreview').modal('show');


            // $scope.imagepreview = img;
            $scope.imagepreview1 = reply.ISTINT_Attachment;
            $scope.view_videos = [];
            var img = $scope.imagepreview1;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {


                $('#preview').attr('src', $scope.imagepreview1);
                $('#myModalPreview').modal('show');

            }

            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview1)
            }

        }

        $scope.viewphoto1 = function (ISTINT_Attachment) {
            $scope.imagepreview1 = ISTINT_Attachment;
            $('#preview').attr('src', $scope.imagepreview1);

            $('#myModalPreview').modal('show');

        }
        $scope.viewphoto2 = function (ISTINT_Attachment) {
            $scope.imagepreview2 = ISTINT_Attachment;
            $('#preview1').attr('src', $scope.imagepreview2);
            $('#myModalPreview1').modal('show');

        }

        //$scope.previewimg_new = function (img) {
        //    $scope.imagepreview = img;
        //    $scope.view_videos = [];
        //    var img = $scope.imagepreview;
        //    if (img != null) {
        //        var imagarr = img.split('.');
        //        var lastelement = imagarr[imagarr.length - 1];
        //        $scope.filetype2 = lastelement;
        //    }
        //    if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

        //        $scope.view_videos.push({ id: 1, ihw_video: img });
        //        $('#myvideoPreview').modal('show');

        //    }
        //    else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

        //        $('#preview').attr('src', $scope.imagepreview);
        //        $('#myimagePreview').modal('show');

        //    }
        //    else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
        //        $window.open($scope.imagepreview)
        //    }
        //    else if ($scope.filetype2 == 'mp3') {
        //        $scope.view_videos.push({ id: 1, ihw_video: img });
        //        $('#myaudioPreview').modal('show');

        //    }
        //    else if ($scope.filetype2 == 'pdf') {

        //        ///=====================show pdf, img

        //        $('#showpdf').modal('hide');
        //        var imagedownload1 = "";
        //        imagedownload1 = $scope.imagepreview;
        //        $http.get(imagedownload1, { responseType: 'arraybuffer' })
        //            .success(function (response) {
        //                var fileURL = "";
        //                var file = "";
        //                var embed = "";
        //                var pdfId = "";
        //                file = new Blob([(response)], { type: 'application/pdf' });
        //                fileURL = URL.createObjectURL(file);

        //                pdfId = document.getElementById("pdfIdzz");
        //                pdfId.removeChild(pdfId.childNodes[0]);
        //                embed = document.createElement('embed');
        //                embed.setAttribute('src', fileURL);
        //                embed.setAttribute('type', 'application/pdf');
        //                embed.setAttribute('width', '100%');
        //                embed.setAttribute('height', '1000');
        //                pdfId.appendChild(embed);
        //                $('#showpdf').modal('show');
        //            });
        //    }
        //    else {
        //        $window.open($scope.imagepreview)
        //    }
        //};





        //$scope.closeimg = function () {
        //    $('#myModalPreview').modal('hide');
        //    $('.modal-backdrop').remove();
        //    $scope.reply();
        //    $('#viewrplyModal').modal('show');
        //    //$('#myModalResponse').modal('show');
        //    //$('.modal').on('hide.bs.modal', function (e) {
        //    //    e.stopPropagation();
        //    //    $('body').css('padding-right', '');
        //    //});
        //};
        //=================mp4========
        $scope.previewmp4 = function (qq) {
            $scope.view_videos = [];
            $scope.view_videos.push({ id: 1, video: qq });

            $('#myvideoPreview').modal('show');
        }
        //================pdf   ===============
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;
            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        };

        $scope.urlview = function (urlname) {
            $scope.img = urlname;
            if ($scope.img != null) {
                var imagarr = $scope.img.split('www');
                var lastelement = imagarr[0];
                if (lastelement == "https://") {
                    $window.open(urlname)
                }

            }
        }
        //=========================


        //============seen check=============//

        $scope.seen = function (ISTINT_Id) {

            $scope.ISTINT_Id = ISTINT_Id;
            var data = {
                "ISTINT_Id": ISTINT_Id,
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("IVRM_Interactions/seen", data).
                then(function (promise) {

                    if (promise.returnval == true) {

                    }
                    else {

                    }
                });
        };
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();
