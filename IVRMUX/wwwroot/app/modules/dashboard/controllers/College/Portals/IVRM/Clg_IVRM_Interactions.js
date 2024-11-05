
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Clg_IVRM_InteractionsController', Clg_IVRM_InteractionsController);

    Clg_IVRM_InteractionsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$timeout', '$http', '$sce','$q'];
    function Clg_IVRM_InteractionsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $interval, $timeout, $http, $sce, $q) {

        $scope.isminT_DateTime = new Date();
        $scope.iintsS_Date = new Date();
        $scope.composetab = false;
        $scope.inboxtab = true;
        $scope.composehead = false;
        //Auto Generate Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.obj = {};

        $scope.oninbox = function () {
            $scope.composetab = false;
            $scope.composehead = false;
            $scope.inboxtab = true;
        };
        $scope.typechangeG = function () {
            $scope.ISMINT_GroupOrIndFlg = "Group";
            $scope.groupindvalue = true;
            $scope.amcO_Id = "";
            $scope.get_student = "";
            $scope.composetab = true;
            $scope.composehead = true;
            $scope.inboxtab = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
        $scope.typechangeI = function () {
            $scope.ISMINT_GroupOrIndFlg = "Individual";
            $scope.groupindvalue = false;
            $scope.amcO_Id = "";
            $scope.get_student = "";
            $scope.composetab = true;
            $scope.composehead = true;
            $scope.inboxtab = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        };
        $scope.refresh = function () {
            $scope.loaddata();
        };
        //===================Page Load
        $scope.loaddata = function () {
         
            var pageid = 2;
            $scope.search1 = "";
            apiService.getURI("Clg_IVRM_Interactions/getloaddata", pageid).
                then(function (promise) {
                   
                    $scope.roletype = promise.roletype;
                    $scope.roleflg = $scope.roletype[0].ivrmrT_Role;
                    if ($scope.roleflg == "College HOD") {
                        $scope.roleflg = "HOD";
                    }
                    $scope.configflag = promise.configflag;
                    $scope.stflag = $scope.configflag[0].ivrmgC_EnableSTCTIntFlg;
                    $scope.ctflag = $scope.configflag[0].ivrmgC_EnableSTCTIntFlg;
                    $scope.hodflag = $scope.configflag[0].ivrmgC_EnableSTHODIntFlg;
                    $scope.principalflag = $scope.configflag[0].ivrmgC_EnableSTPrincipalIntFlg;
                    $scope.asflag = $scope.configflag[0].ivrmgC_EnableSTASIntFlg;
                    //$scope.ecflag = $scope.configflag[0].ivrmgC_EnableECIntFlg;
                    $scope.getinboxmsg = promise.getinboxmsg;
                    $scope.hidec = 0;
                    if (promise.userhrmE_Id > 0) {
                        $scope.userhrmE_Id = promise.userhrmE_Id;
                    }
                    $scope.message_list = [];
                    angular.forEach($scope.getinboxmsg, function (mg) {
                        if ($scope.message_list.length === 0) {
                            $scope.message_list.push({ ISMINT_Id: mg.ISMINT_Id, ISTINT_Id: mg.ISTINT_Id, ISMINT_InteractionId: mg.ISMINT_InteractionId, ISMINT_Subject: mg.ISMINT_Subject, ISMINT_Interaction: mg.ISMINT_Interaction, ISMINT_GroupOrIndFlg: mg.ISMINT_GroupOrIndFlg, ISMINT_ComposedByFlg: mg.ISMINT_ComposedByFlg, Sender: mg.Sender, Receiver: mg.Receiver, ISMINT_DateTime: mg.ISTINT_DateTime, ISTINT_Attachment: mg.ISTINT_Attachment, ISMINT_ComposedById: mg.ISMINT_ComposedById });
                        }
                        else if ($scope.message_list.length > 0) {
                            var al_cnt = 0;
                            angular.forEach($scope.message_list, function (sl) {
                                if (sl.ISMINT_Id === mg.ISMINT_Id) {
                                    al_cnt += 1;
                                }
                            });
                            if (al_cnt === 0) {
                                $scope.message_list.push({ ISMINT_Id: mg.ISMINT_Id, ISTINT_Id: mg.ISTINT_Id, ISMINT_InteractionId: mg.ISMINT_InteractionId, ISMINT_Subject: mg.ISMINT_Subject, ISMINT_Interaction: mg.ISMINT_Interaction, ISMINT_GroupOrIndFlg: mg.ISMINT_GroupOrIndFlg, ISMINT_ComposedByFlg: mg.ISMINT_ComposedByFlg, Sender: mg.Sender, Receiver: mg.Receiver, ISMINT_DateTime: mg.ISTINT_DateTime, ISTINT_Attachment: mg.ISTINT_Attachment, ISMINT_ComposedById: mg.ISMINT_ComposedById });
                            }
                        }
                        $scope.inboxtotal = $scope.message_list.length;
                    });

                    if ($scope.message_list.length > 0) {
                        angular.forEach($scope.message_list, function (qq) {

                            if (qq.ISTINT_Attachment != undefined || qq.ISTINT_Attachment != '' || qq.ISTINT_Attachment != null) {
                                var img = qq.ISTINT_Attachment;
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
            $scope.branchList = "";
            $scope.semesterList = "";
            $scope.getdetails = "";
            $scope.get_student = "";
            var data = {
                "roleflg": $scope.roleflg,
                "userflag": $scope.obj.userflag
            };
            apiService.create("Clg_IVRM_Interactions/getdetails", data).
                then(function (promise) {
                   
                    if (promise.getdetails.length > 0) {
                        $scope.getdetails = promise.getdetails;
                        $scope.empName = promise.getdetails[0].EmpName;
                        $scope.hrmE_Id = promise.getdetails[0].HRME_Id;
                        $scope.asmay_id = promise.getdetails[0].ASMAY_Id;
                    }
                    else {
                        swal("No Record Found....");
                        $scope.getdetails.length = 0;
                    }
                });
        };

        //========================= course Change
        $scope.getbranch = function (amcO_Id) {
           
            //$scope.get_student = "";
            $scope.amcO_Id = amcO_Id;
            var data = {
                "AMCO_Id": amcO_Id,
                "ASMAY_Id": $scope.asmay_id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("Clg_IVRM_Interactions/Getbranch", data).
                then(function (promise) {
                    $scope.obj.asmS_Id = '';
                    if (promise.branchList != null && promise.branchList.length > 0) {
                        $scope.branchList = promise.branchList;
                    }
                    else {
                        $scope.get_student = "";
                    }
                });
        };



        //========================= branch Change
        $scope.getsemester = function () {
            //$scope.get_student = "";
          
            var data = {
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.obj.amB_Id,
                "ASMAY_Id": $scope.asmay_id
               
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("Clg_IVRM_Interactions/Getsemester", data).
                then(function (promise) {
                    $scope.obj.asmS_Id = '';
                    if (promise.semesterList != null && promise.semesterList.length > 0) {
                        $scope.semesterList = promise.semesterList;
                    }
                    else {
                        $scope.get_student = "";
                    }
                });
        };


        //====================== section =================


        $scope.getsection = function () {
            //$scope.get_student = "";
           
            var data = {
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.obj.amB_Id,
                "ASMAY_Id": $scope.asmay_id,
                "AMSE_Id": $scope.obj.amsE_Id,

            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("Clg_IVRM_Interactions/Getsection", data).
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
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.obj.amB_Id,
                "AMSE_Id": $scope.obj.amsE_Id,
                "ACMS_Id": $scope.obj.acmS_Id,
            };
            apiService.create("Clg_IVRM_Interactions/getstudent", data).
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
        //$scope.isOptionsRequiredST = function () {
        //    if ($scope.obj.userflag === "SubjectTeacher" && $scope.groupindvalue === true) {
        //        return !$scope.getdetails.some(function (options) {
        //            return options.stck;
        //        });
        //    }
        //};
        $scope.all_checkST = function (subj) {
            $scope.usercheckST = subj;
            var toggleStatus = $scope.usercheckST;
            angular.forEach($scope.getdetails, function (gd) {
                gd.stck = toggleStatus;
            });
        };

        //===================Save Data / Send Message     
        $scope.savedata = function () {
          
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
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
                        $scope.arrayStudent1 = [];
                        angular.forEach($scope.get_student, function (stu) {
                            if (stu.stuck) {
                                $scope.arrayStudent1.push(stu);
                            }
                        });

                        if ($scope.ISMINT_GroupOrIndFlg === "Group") {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.obj.userflag,
                                "arrayStudent1": $scope.arrayStudent1,
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
                                "student_Id": $scope.obj.amcsT_Id.AMCST_Id,
                               // "student_Id": $scope.obj.amcsT_Id.AMCST_Id,
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
                apiService.create("Clg_IVRM_Interactions/savedetails", data).then(function (promise) {
                
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
        //===================
        $scope.reply = function (objs) {

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

            apiService.create("Clg_IVRM_Interactions/reply", data).
                then(function (promise) {
                    if (promise.viewmessage !== null) {
                        $scope.viewmessage = promise.viewmessage;
                        $scope.subject = objs.ISMINT_Subject;
                        $scope.subjdetails = objs.ISMINT_Interaction;
                        $scope.ISTINT_Attachment = objs.ISTINT_Attachment;

                        var img1 = objs.ISTINT_Attachment;
                        var imagarr1 = img1.split('.');
                        var lastelement1 = imagarr1[imagarr1.length - 1];
                        $scope.filetype1 = lastelement1;

                        if (promise.viewmessage != null && promise.viewmessage.length > 0) {
                            angular.forEach($scope.viewmessage, function (ddd) {
                                ddd.filetype = "";
                                if (ddd.ISTINT_Attachment != undefined || ddd.ISTINT_Attachment != '' || ddd.ISTINT_Attachment != null) {
                                    var img = ddd.ISTINT_Attachment;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];

                                    ddd.filetype = lastelement;
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
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
                var data = {
                    "ISMINT_Id": $scope.id,
                    "ISTINT_ComposedByFlg": $scope.roleflg,
                    "ISTINT_Interaction": $scope.istinT_Interaction,
                    "ISTINT_Id": $scope.istinT_Id,
                    "images_paths": $scope.images_paths
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("Clg_IVRM_Interactions/savereply", data).then(function (promise) {

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
                        apiService.create("Clg_IVRM_Interactions/deletemsg", data).
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
                        apiService.create("Clg_IVRM_Interactions/deleteinboxmsg", data).
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
            $scope.imagepreview1 = reply.ISTINT_Attachment;
            $('#preview').attr('src', $scope.imagepreview1);
           $('#myModalPreview').modal('show');
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
     
      
        //================pdf   ===============
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;
            $scope.imagedown = filepath1;


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
        //=========================
    }
})();