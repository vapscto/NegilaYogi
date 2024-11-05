
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Alumni_InteractionsController', Alumni_InteractionsController);
    Alumni_InteractionsController.$inject = ['$http', '$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$timeout', '$sce', '$q'];
    function Alumni_InteractionsController($http, $rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $interval, $timeout, $sce, $q) {

        $scope.isminT_DateTime = new Date();
        $scope.iintsS_Date = new Date();
        $scope.composetab = false;
        $scope.inboxtab = true;
        $scope.composehead = false;
        $scope.searchValue = "";
        //Auto Generate Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.obj = {};

        $scope.oninbox = function () {
            $scope.composetab = false;
            $scope.composehead = false;
            $scope.inboxtab = true;
            $scope.obj.userflag = false;
        };
        $scope.typechangeG = function () {
            $scope.ALSMINT_GroupOrIndFlg = "Group";
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
            $scope.ALSMINT_GroupOrIndFlg = "Individual";
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
            apiService.getURI("Alumni_Interactions/getloaddata", pageid).
                then(function (promise) {
                    $scope.roletype = promise.roletype;
                    $scope.roleflg = $scope.roletype[0].ivrmrT_Role;
                    $scope.userhrmE_Id = promise.userhrmE_Id;
                    // $scope.academicyear = promise.aludetails[0].asmaY_id;


                    $scope.chatwithstudent = false;
                    $scope.chatwithstaff = false;


                    //if (promise.rolename != 'student') {
                    //    if ($scope.STFtoSTF == true) {
                    //        $scope.chatwithstaff = true;
                    //    }
                    //    if (promise.rolename == 'principal') {
                    //        if ($scope.PRItoSTU == true) {
                    //            $scope.chatwithstudent = true;
                    //        }
                    //    }
                    //    if (promise.rolename == 'hod') {
                    //        if ($scope.HODtoSTU == true) {
                    //            $scope.chatwithstudent = true;
                    //        }
                    //    }

                    //    if (promise.classteacherlist != null && promise.classteacherlist.length > 0) {

                    //        if ($scope.CLTtoSTU == true) {
                    //            $scope.chatwithstudent = true;
                    //        }

                    //    }

                    //    if (promise.subteacherlist != null && promise.subteacherlist.length > 0) {
                    //        if ($scope.SUBTtoSTU == true) {
                    //            $scope.chatwithstudent = true;
                    //        }
                    //    }

                    //    if (promise.typelist != null && promise.typelist.length > 0) {
                    //        angular.forEach(promise.typelist, function (rr) {
                    //            if (rr.ihoD_Flg.toLowerCase() == 'hod') {
                    //                if ($scope.HODtoSTU == true) {
                    //                    $scope.chatwithstudent = true;
                    //                }

                    //            }
                    //            if (rr.ihoD_Flg.toLowerCase() == 'as') {
                    //                if ($scope.ACStoSTU == true) {
                    //                    $scope.chatwithstudent = true;
                    //                }

                    //            }
                    //            if (rr.ihoD_Flg.toLowerCase() == 'ec') {
                    //                if ($scope.SUBTtoSTU == true) {
                    //                    $scope.chatwithstudent = true;
                    //                }

                    //            }

                    //        })
                    //    }


                    //}




                    $scope.getinboxmsg = promise.getinboxmsg;
                    $scope.hidec = 0;
                    if (promise.userhrmE_Id > 0) {
                        $scope.userhrmE_Id = promise.userhrmE_Id;
                    }

                    $scope.message_list = [];
                    angular.forEach($scope.getinboxmsg, function (mg) {
                        if ($scope.message_list.length === 0) {
                            $scope.message_list.push({ ALSMINT_Id: mg.ALSMINT_Id, ALSTINT_Id: mg.ALSTINT_Id, ALSMINT_InteractionId: mg.ALSMINT_InteractionId, ALSMINT_Subject: mg.ALSMINT_Subject, ALSMINT_Interaction: mg.ALSMINT_Interaction, ALSMINT_ComposedByFlg: mg.ALSMINT_ComposedByFlg, Sender: mg.Sender, Receiver: mg.Receiver, ALSMINT_DateTime: mg.ALSMINT_DateTime, ALSTINT_Attachment: mg.ALSTINT_Attachment, ALSMINT_ComposedById: mg.ALSMINT_ComposedById, ALSMINT_GroupOrIndFlg: mg.ALSMINT_GroupOrIndFlg });
                        }
                        else if ($scope.message_list.length > 0) {
                            var al_cnt = 0;
                            angular.forEach($scope.message_list, function (sl) {
                                if (sl.ALSMINT_Id === mg.ALSMINT_Id) {
                                    al_cnt += 1;
                                }
                            });
                            if (al_cnt === 0) {
                                $scope.message_list.push({ ALSMINT_Id: mg.ALSMINT_Id, ALSTINT_Id: mg.ALSTINT_Id, ALSMINT_InteractionId: mg.ALSMINT_InteractionId, ALSMINT_Subject: mg.ALSMINT_Subject, ALSMINT_Interaction: mg.ALSMINT_Interaction, ALSMINT_ComposedByFlg: mg.ALSMINT_ComposedByFlg, Sender: mg.Sender, Receiver: mg.Receiver, ALSMINT_DateTime: mg.ALSMINT_DateTime, ALSTINT_Attachment: mg.ALSTINT_Attachment, ALSMINT_ComposedById: mg.ALSMINT_ComposedById, ALSMINT_GroupOrIndFlg: mg.ALSMINT_GroupOrIndFlg });
                            }
                        }

                        $scope.inboxtotal = $scope.message_list.length;
                    });


                    if ($scope.message_list.length > 0) {
                        angular.forEach($scope.message_list, function (qq) {

                            if (qq.ALSTINT_Attachment != undefined || qq.ALSTINT_Attachment != '' || qq.ALSTINT_Attachment != null) {
                                var img = qq.ALSTINT_Attachment;
                                if (img != null) {
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];

                                    qq.filetype2 = lastelement;
                                }

                            }
                        });
                    }


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
            apiService.create("Alumni_Interactions/getdetails", data).
                then(function (promise) {
                    if (promise.getdetails !== null && promise.getdetails.length > 0) {
                        $scope.getdetails = promise.getdetails;

                    }
                    else {
                        swal("No Record Found....");
                        $scope.getdetails.length = 0;
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
            //if ($scope.myForm.$valid) {



            if ($scope.roleflg === "Alumni") {


                if ($scope.ALSMINT_GroupOrIndFlg === "Group") {
                    $scope.arrayalumni = [];
                    angular.forEach($scope.getdetails, function (stu) {
                        if (stu.tck === true) {
                            $scope.arrayalumni.push({ ALMST_Id: stu.ALMST_Id });
                        }
                    });
                    data = {
                        "roleflg": $scope.roleflg,
                        "userflag": $scope.obj.userflag,
                        "arrayalumni": $scope.arrayalumni,
                        "ALSMINT_ComposedByFlg": $scope.roleflg,
                        "ALSMINT_GroupOrIndFlg": $scope.ALSMINT_GroupOrIndFlg,
                        "ALSMINT_Subject": $scope.obj.alsminT_Subject,
                        "ALSMINT_Interaction": $scope.obj.alsminT_Interaction,
                        "ALSMINT_Id": $scope.isminT_Id,
                        "images_paths": $scope.images_paths
                    };
                }
                else {
                    data = {
                        "roleflg": $scope.roleflg,
                        "userflag": $scope.obj.userflag,
                        "ALSTINT_ToId": $scope.obj.ALMST_Id1,
                        "ALSMINT_ComposedByFlg": $scope.roleflg,
                        "ALSMINT_GroupOrIndFlg": $scope.ALSMINT_GroupOrIndFlg,
                        "ALSMINT_Subject": $scope.obj.alsminT_Subject,
                        "ALSMINT_Interaction": $scope.obj.alsminT_Interaction,
                        "ALSMINT_Id": $scope.isminT_Id,
                        "images_paths": $scope.images_paths
                    };
                }
            }




            apiService.create("Alumni_Interactions/savedetails", data).then(function (promise) {

                if (promise.returnval === true) {
                    if (promise.alsminT_Id === 0 || promise.alsminT_Id < 0) {
                        swal('Message sent successfully');
                    }
                }
                else {
                    if (promise.alsminT_Id === 0 || promise.alsminT_Id < 0) {
                        swal('Failed to send, please contact administrator');
                    }
                    else if (promise.alsminT_Id > 0) {
                        swal('Failed to update, please contact administrator');
                    }
                }
                $state.reload();
            });
            //}
            //else {
            //    $scope.submitted = true;
            //}
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.replyshow = false;
        //===================
        $scope.reply = function (objs) {
            $scope.replyshow = false;
            var data = "";
            if (objs === undefined) {

                $scope.id = $scope.id;
                $scope.alstinT_Id = objs.ALSTINT_Id;
                data = {
                    "ALSMINT_Id": $scope.id
                };
            }
            else {
                $scope.id = objs.ALSMINT_Id;
                $scope.alstinT_Id = objs.ALSTINT_Id;
                data = {
                    "ALSMINT_Id": $scope.id
                };
            }

            apiService.create("Alumni_Interactions/reply", data).
                then(function (promise) {
                    $scope.replyshow = true;


                    //if (promise.rolename == 'student') {
                    //    if (promise.typelistrole != null && promise.typelistrole.length > 0) {
                    //        angular.forEach(promise.typelistrole, function (rr) {
                    //            if (rr.ivrmrT_Role.toLowerCase() == 'principal') {
                    //                if ($scope.PRPLforStudent == true) {
                    //                    $scope.replyshow = true;
                    //                }

                    //            }

                    //        })
                    //    }

                    //    if (promise.typelist != null && promise.typelist.length > 0) {
                    //        angular.forEach(promise.typelist, function (rr) {
                    //            if (rr.ihoD_Flg.toLowerCase() == 'hod') {
                    //                if ($scope.HODforStudent == true) {
                    //                    $scope.replyshow = true;
                    //                }

                    //            }
                    //            if (rr.ihoD_Flg.toLowerCase() == 'as') {
                    //                if ($scope.ASforStudent == true) {
                    //                    $scope.replyshow = true;
                    //                }

                    //            }
                    //            if (rr.ihoD_Flg.toLowerCase() == 'ec') {
                    //                if ($scope.ExCoforStudent == true) {
                    //                    $scope.replyshow = true;
                    //                }

                    //            }

                    //        })
                    //    }


                    //    if (promise.classteacherlist != null && promise.classteacherlist.length > 0) {


                    //        if ($scope.CTforStudent == true) {
                    //            $scope.replyshow = true;
                    //        }



                    //    }

                    //    if (promise.subteacherlist != null && promise.subteacherlist.length > 0) {
                    //        if ($scope.STforStudent == true) {
                    //            $scope.replyshow = true;
                    //        }



                    //    }
                    //}
                    //else {
                    //    if (promise.isminT_ComposedByFlg == 'student') {

                    //        if (promise.typelistrole != null && promise.typelistrole.length > 0) {
                    //            angular.forEach(promise.typelistrole, function (rr) {
                    //                if (rr.ivrmrT_Role.toLowerCase() == 'principal') {
                    //                    if ($scope.PRItoSTU == true) {
                    //                        $scope.replyshow = true;
                    //                    }

                    //                }

                    //            })
                    //        }

                    //        if (promise.typelist != null && promise.typelist.length > 0) {
                    //            angular.forEach(promise.typelist, function (rr) {
                    //                if (rr.ihoD_Flg.toLowerCase() == 'hod') {
                    //                    if ($scope.HODtoSTU == true) {
                    //                        $scope.replyshow = true;
                    //                    }

                    //                }
                    //                if (rr.ihoD_Flg.toLowerCase() == 'as') {
                    //                    if ($scope.ACStoSTU == true) {
                    //                        $scope.replyshow = true;
                    //                    }

                    //                }
                    //                if (rr.ihoD_Flg.toLowerCase() == 'ec') {
                    //                    if ($scope.ECtoSTU == true) {
                    //                        $scope.replyshow = true;
                    //                    }

                    //                }

                    //            })
                    //        }


                    //        if (promise.classteacherlist != null && promise.classteacherlist.length > 0) {


                    //            if ($scope.CLTtoSTU == true) {
                    //                $scope.replyshow = true;
                    //            }



                    //        }

                    //        if (promise.subteacherlist != null && promise.subteacherlist.length > 0) {
                    //            if ($scope.SUBTtoSTU == true) {
                    //                $scope.replyshow = true;
                    //            }



                    //        }



                    //    } else {

                    //        if (promise.composeedto == 'student') {

                    //            if (promise.typelistrole != null && promise.typelistrole.length > 0) {
                    //                angular.forEach(promise.typelistrole, function (rr) {
                    //                    if (rr.ivrmrT_Role.toLowerCase() == 'principal') {
                    //                        if ($scope.PRItoSTU == true) {
                    //                            $scope.replyshow = true;
                    //                        }

                    //                    }

                    //                })
                    //            }

                    //            if (promise.typelist != null && promise.typelist.length > 0) {
                    //                angular.forEach(promise.typelist, function (rr) {
                    //                    if (rr.ihoD_Flg.toLowerCase() == 'hod') {
                    //                        if ($scope.HODtoSTU == true) {
                    //                            $scope.replyshow = true;
                    //                        }

                    //                    }
                    //                    if (rr.ihoD_Flg.toLowerCase() == 'as') {
                    //                        if ($scope.ACStoSTU == true) {
                    //                            $scope.replyshow = true;
                    //                        }

                    //                    }
                    //                    if (rr.ihoD_Flg.toLowerCase() == 'ec') {
                    //                        if ($scope.ECtoSTU == true) {
                    //                            $scope.replyshow = true;
                    //                        }

                    //                    }

                    //                })
                    //            }


                    //            if (promise.classteacherlist != null && promise.classteacherlist.length > 0) {


                    //                if ($scope.CLTtoSTU == true) {
                    //                    $scope.replyshow = true;
                    //                }



                    //            }

                    //            if (promise.subteacherlist != null && promise.subteacherlist.length > 0) {
                    //                if ($scope.SUBTtoSTU == true) {
                    //                    $scope.replyshow = true;
                    //                }



                    //            }
                    //        }
                    //        else {
                    //            if ($scope.STFtoSTF == true) {
                    //                $scope.replyshow = true;
                    //            }
                    //        }


                    //    }
                    //}




                    if (promise.viewmessage !== null) {
                        $scope.viewmessage = promise.viewmessage;
                        $scope.subject = objs.ALSMINT_Subject;
                        $scope.subjdetails = objs.ALSMINT_Interaction;
                        $scope.ALSMINT_Attachment = objs.ALSMINT_Attachment;

                        var img1 = objs.ALSTINT_Attachment;
                        var imagarr1 = img1.split('.');
                        var lastelement1 = imagarr1[imagarr1.length - 1];
                        $scope.filetype1 = lastelement1;

                        if (promise.viewmessage != null && promise.viewmessage.length > 0) {
                            angular.forEach($scope.viewmessage, function (ddd) {
                                ddd.filetype = "";
                                if (ddd.ALSTINT_Attachment != undefined || ddd.ALSTINT_Attachment != '' || ddd.ALSTINT_Attachment != null) {
                                    var img = ddd.ALSTINT_Attachment;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];

                                    ddd.filetype = lastelement;
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        ddd.ALSTINT_Attachment1 = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.ALSTINT_Attachment;
                                    }
                                }
                            });
                        }

                    }
                });
        };

        $scope.replymsg = function (ee) {
            angular.element('#viewrplyModal').modal('hide');

        };


        //===================Save Staff/Student Reply       
        $scope.savereply = function (rply) {
            $scope.submittedR = true;
            if ($scope.myFormR.$valid) {
                if ($scope.roleflg === "HOD" || $scope.roleflg === "Principal" || $scope.roleflg === "AS" || $scope.roleflg === "EC") {
                    $scope.roleflg = "Staff";
                }
                var data = "";
                if ($scope.images_paths == undefined) {

                    data = {
                        "ALSMINT_Id": $scope.id,
                        "ALSTINT_ComposedByFlg": $scope.roleflg,
                        "ALSTINT_Interaction": $scope.istinT_Interaction,
                        "ALSTINT_Id": $scope.alstinT_Id


                    };
                }
                else {
                    data = {
                        "ALSMINT_Id": $scope.id,
                        "ALSTINT_ComposedByFlg": $scope.roleflg,
                        "ALSTINT_Interaction": $scope.istinT_Interaction,
                        "ALSTINT_Id": $scope.alstinT_Id,
                        "images_paths": $scope.images_paths

                    };
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("Alumni_Interactions/savereply", data).then(function (promise) {

                    if (promise.returnval === true) {

                        swal('Message sent successfully');
                    }

                    else {
                        {
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
                        apiService.create("Alumni_Interactions/deletemsg", data).
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
                        apiService.create("Alumni_Interactions/deleteinboxmsg", data).
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
            $scope.files = event.files;
            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i];
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL(file);
            }
            UploadImgs();
        };

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
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                $scope.filenames = "Videos";
                $scope.fileflg = true;
            }
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
        $scope.previewimg = function (img) {
            $scope.imagepreview = img;
            $('#preview').attr('src', $scope.imagepreview);
            $('#myModalPreview').modal('show');
        };
        //==================view phots
        $scope.viewphoto = function (reply) {
            $scope.imagepreview1 = reply.ALSTINT_Attachment;
            $('#preview').attr('src', $scope.imagepreview1);

            $('#myModalPreview').modal('show');

        }

        $scope.viewphoto1 = function (ALSTINT_Attachment) {
            $scope.imagepreview1 = ALSTINT_Attachment;
            $('#preview').attr('src', $scope.imagepreview1);

            $('#myModalPreview').modal('show');

        }
        $scope.viewphoto2 = function (ALSTINT_Attachment) {
            $scope.imagepreview2 = ALSTINT_Attachment;
            $('#preview1').attr('src', $scope.imagepreview2);
            $('#myModalPreview1').modal('show');

        }

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
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();
