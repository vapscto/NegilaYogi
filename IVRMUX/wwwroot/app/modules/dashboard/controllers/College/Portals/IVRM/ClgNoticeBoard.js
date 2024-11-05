(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgNoticeBoardController', ClgNoticeBoardController)

    ClgNoticeBoardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$q', '$http', '$filter', 'superCache', '$window']
    function ClgNoticeBoardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $q, $http, $filter, superCache, $window) {

        //====================================== Upload
        $scope.obj = {};
        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/doc" || input.files[0].type === "application/docx" || input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        }
        //====================LOAD Data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getDATA("ClgNoticeBoard/getloaddata").
                then(function (promise) {

                    $scope.noticelist = promise.noticelist;
                    $scope.presentCountgrid = $scope.noticelist.length;
                    $scope.feegrplist = promise.fee_group;
                    $scope.feedeflist = promise.fee_heads;
                    $scope.yearlst = promise.fillyear;
                    $scope.departmentdropdown = promise.departmentList;

                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }

        //=========================Course all Check
        //$scope.toggleAll = function () {
        //    angular.forEach($scope.course_list, function (cors) {
        //        cors.course = $scope.all;
        //    })
        //    $scope.oncoursechange();
        //};
        //--------------------------on year change
        $scope.onyearchange = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY,
                // "AMCO_Id": $scope.obj.amcO_Id,
            }
            apiService.create("ClgNoticeBoard/getcoursedata", data).then(function (promise) {
                $scope.course_list = promise.course_list;
            })
        }

        ////=========================Course Selection Change
        //$scope.oncoursechange = function () {
        //    var data = {
        //      // "AMCO_Id": $scope.obj.amcO_Id,
        //        "ASMAY_Id": $scope.ASMAY

        //    }
        //    apiService.create("ClgNoticeBoard/getbranchdata", data).
        //        then(function (promise) {
        //            $scope.branch_list = promise.branch_list;
        //        })
        //}

        //--------------------------course all check
        $scope.toggleAllC = function () {
            angular.forEach($scope.course_list, function (crs) {
                crs.course = $scope.obj.allC;

            })
            $scope.oncoursechange();
        };

        $scope.isOptionsRequiredc = function () {
            return !$scope.course_list.some(function (options) {
                return options.course;
            });
        }

        //=========================Branch All Check
        $scope.toggleAllB = function () {
            angular.forEach($scope.branch_list, function (brch) {
                brch.branch = $scope.obj.allB;
            })
            $scope.onbranchchange();
        };

        $scope.isOptionsRequiredb = function () {
            return !$scope.branch_list.some(function (options) {
                return options.branch;
            });
        }
        //--------------------------------course selection change
        $scope.oncoursechange = function () {
            $scope.coursearray = [];
            $scope.branch_list = "";
            $scope.allC = $scope.course_list.every(function (crs) { return crs.course; });
            angular.forEach($scope.course_list, function (crs) {
                if (crs.course == true) {
                    $scope.coursearray.push(crs);
                }
            })
            var data = {
                "ASMAY_Id": $scope.ASMAY,
                "courseArray": $scope.coursearray,

            }
            apiService.create("ClgNoticeBoard/getbranchdata", data).
                then(function (promise) {
                    $scope.branch_list = promise.branch_list;
                })
        }
        //=========================Branch Selection Change
        $scope.onbranchchange = function () {
            $scope.brancharray = [];
            $scope.sem_list = "";
            $scope.allB = $scope.branch_list.every(function (brh) { return brh.branch; });
            angular.forEach($scope.branch_list, function (brch) {
                if (brch.branch == true) {
                    $scope.brancharray.push(brch);
                }
            })
            var data = {
                "ASMAY_Id": $scope.ASMAY,
                "branchArray": $scope.brancharray,

            }
            apiService.create("ClgNoticeBoard/getsemdata", data).
                then(function (promise) {
                    $scope.sem_list = promise.sem_list;
                })
        }

        //=========================Semester All Check
        $scope.toggleAllS = function () {
            angular.forEach($scope.sem_list, function (sm) {
                sm.sem = $scope.obj.allS;
            })

            $scope.getstudent();
        };

        $scope.isOptionsRequiredS = function () {
            return !$scope.sem_list.some(function (options) {
                return options.sem;
            });


        }


        //fee group
        $scope.togchkbxG = function () {
            $scope.defgrparray = [];
            angular.forEach($scope.feegrplist, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ FMG_Id: qq.fmG_Id })
                }
            })
        }
        //fee head
        $scope.togchkbxF = function () {
            $scope.deflistarray = [];
            angular.forEach($scope.feedeflist, function (qq) {
                if (qq.selected == true) {
                    $scope.deflistarray.push({ FMH_Id: qq.fmH_Id })
                }
            })
        }

        //fee head
        $scope.all_checkF = function (all, FMT_Id) {

            $scope.deflistarray = [];
            $scope.usercheckF = all;
            var toggleStatus = $scope.usercheckF;
            angular.forEach($scope.feedeflist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.deflistarray = [];
            angular.forEach($scope.feedeflist, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ FMH_Id: qq.fmH_Id })
                }
            });

        }

        ///--------------------------------group all selection

        $scope.all_checkG = function (all, FMG_Id) {

            $scope.defgrparray = [];
            $scope.usercheckG = all;
            var toggleStatus = $scope.usercheckG;
            angular.forEach($scope.feegrplist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.defgrparray = [];
            angular.forEach($scope.feegrplist, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ FMG_Id: qq.fmG_Id })
                }
            });


        }


        $scope.all_checkS = function (all) {
            $scope.usercheckS = all;
            var toggleStatus = $scope.usercheckS;
            angular.forEach($scope.studentlist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.studentarray = [];
            angular.forEach($scope.studentlist, function (qq) {
                if (qq.selected == true) {
                    $scope.studentarray.push({ AMCST_Id: qq.amcsT_Id })
                }
            })



        };


        $scope.getstudent = function () {
            $scope.studentlist = [];
            if ($scope.obj.fee_def == true) {
                $scope.defgrparray = [];
                angular.forEach($scope.feegrplist, function (qq) {
                    if (qq.selected == true) {
                        $scope.defgrparray.push({ FMG_Id: qq.fmG_Id })
                    }
                });
                $scope.deflistarray = [];
                angular.forEach($scope.feedeflist, function (qq) {
                    if (qq.selected == true) {
                        $scope.deflistarray.push({ FMH_Id: qq.fmH_Id })
                    }
                });

            }

            //added by roopa//
            $scope.branchArray = [];
            $scope.courseArray = [];
            $scope.semesterArray = [];

            angular.forEach($scope.course_list, function (crs) {
                if (crs.course == true) {
                    $scope.courseArray.push(crs);
                }
            });


            angular.forEach($scope.branch_list, function (brh) {
                if (brh.branch == true) {
                    $scope.branchArray.push(brh);
                }
            });
            angular.forEach($scope.sem_list, function (sm) {
                if (sm.sem == true) {
                    $scope.semesterArray.push(sm);
                }
            });


            //if ($scope.obj.select_student == true) {
            var data = {
                //"AMCO_Id": $scope.obj.amcO_Id,
                "defarray": $scope.deflistarray,
                "defheadarray": $scope.deflistarray,
                "fee_def": $scope.obj.fee_def,
                "courseArray": $scope.courseArray,
                "branchArray": $scope.branchArray,
                "semesterArray": $scope.semesterArray,
                "ASMAY_Id": $scope.ASMAY

            }
            apiService.create("ClgNoticeBoard/getstudent", data).then(function (promise) {
                $scope.studentlist1 = [];
                $scope.studentlist = [];
                $scope.studentlist1 = promise.studentlist;
                if ($scope.studentlist1.length > 0 || $scope.studentlist1 != null) {
                    $scope.studentlist = $scope.studentlist1;
                }
                else {
                    swal('No Data Found!!!')
                }
            })
            // }
        }

        $scope.togchkbxS = function () {
            $scope.studentarray = [];
            angular.forEach($scope.studentlist, function (qq) {
                if (qq.selected == true) {
                    $scope.studentarray.push({ AMCST_Id: qq.amcsT_Id })
                }
            })
        }
        //----
        //========================= Save Notice

        $scope.savedata = function () {
            $scope.courseArray = [];
            $scope.branchArray = [];
            $scope.semesterArray = [];

            if ($scope.myForm.$valid) {

                //document upload


                $scope.filedoc = [];
                $scope.filedoc1 = [];
                $scope.filedoc2 = [];
                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.INTBFL_FilePath != null) {
                            $scope.documentListOtherDetails11.push({ INTBFL_FilePath: qq.INTBFL_FilePath, FileName: qq.FileName });
                        }
                    })
                    $scope.filedoc = $scope.documentListOtherDetails11;
                }

                if ($scope.checklink == true) {
                    angular.forEach($scope.urldocumentlist, function (ee) {
                        if (ee.INTBFL_FilePath != null) {
                            $scope.filedoc1.push({ INTBFL_FilePath: ee.INTBFL_FilePath, FileName: ee.INTBFL_FilePath });
                        }
                    })
                }
                if ($scope.filedoc1 != null || $scope.filedoc1 > 0) {
                    angular.forEach($scope.filedoc1, function (ww) {
                        $scope.filedoc.push(ww);
                    })
                }

                //document upload end






                //======================== Date Time Format
                if ($scope.intB_DispalyDisableFlg == false) {
                    var displaydate = null;
                }
                else if ($scope.intB_DispalyDisableFlg == true) {
                    var displaydate = $scope.intB_DisplayDate == null ? "" : $filter('date')($scope.intB_DisplayDate, "yyyy-MM-dd");
                }

                var startdate = $scope.intB_StartDate == null ? "" : $filter('date')($scope.intB_StartDate, "yyyy-MM-dd");
                var enddate = $scope.intB_EndDate == null ? "" : $filter('date')($scope.intB_EndDate, "yyyy-MM-dd");

                //===========cours/Branch / Semester Selected values     
                angular.forEach($scope.course_list, function (cr) {
                    if (cr.course == true) {
                        $scope.courseArray.push(cr);
                    }
                });
                angular.forEach($scope.branch_list, function (brh) {
                    if (brh.branch == true) {
                        $scope.branchArray.push(brh);
                    }
                });
                angular.forEach($scope.sem_list, function (sm) {
                    if (sm.sem == true) {
                        $scope.semesterArray.push(sm);
                    }
                });
                $scope.studentarraynew = [];
                if ($scope.studentarray != null || $scope.studentarray > 0) {
                    $scope.studentarraynew = $scope.studentarray;
                }
                else {
                    angular.forEach($scope.studentlist, function (qq) {
                        $scope.studentarraynew.push({ AMCST_Id: qq.amcsT_Id });
                    })
                }

                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "INTB_Id": $scope.intB_Id,
                        "INTB_Title": $scope.intB_Title,
                        "INTB_Description": $scope.intB_Description,
                        "INTB_StartDate": startdate,
                        "INTB_EndDate": enddate,
                        "INTB_DisplayDate": displaydate,
                        "NTB_TTSylabusFlg": $scope.ntB_TTSylabusFlg,
                        "INTB_DispalyDisableFlg": $scope.intB_DispalyDisableFlg,
                        "AMCO_Id": $scope.obj.amcO_Id,
                        "branchArray": $scope.branchArray,
                        "semesterArray": $scope.semesterArray,
                        "INTB_ToStudentFlg": $scope.check_student,
                        "INTB_ToStaffFlg": $scope.check_staff,
                        "select_student": $scope.obj.select_student,
                        "studentarray": $scope.studentarraynew,
                        "courseArray": $scope.courseArray,
                        "FilePath_Array": $scope.filedoc,
                        "designationlist": $scope.arrayuserdesig,
                        "departmentlist": $scope.arrayuserdept,
                        "employeearraylist": $scope.employeearraylist
                    }
                }
                else {
                    var data = {
                        "INTB_Id": $scope.intB_Id,
                        "INTB_Title": $scope.intB_Title,
                        "INTB_Description": $scope.intB_Description,
                        "INTB_StartDate": startdate,
                        "INTB_EndDate": enddate,
                        "INTB_DisplayDate": displaydate,
                        "NTB_TTSylabusFlg": $scope.ntB_TTSylabusFlg,
                        "INTB_DispalyDisableFlg": $scope.intB_DispalyDisableFlg,
                        "AMCO_Id": $scope.obj.amcO_Id,
                        "branchArray": $scope.branchArray,
                        "semesterArray": $scope.semesterArray,
                        "INTB_Attachment": $scope.file_detail,
                        "INTB_FilePath": att_file11,
                        "INTB_ToStudentFlg": $scope.check_student,
                        "INTB_ToStaffFlg": $scope.check_staff,
                        "select_student": $scope.obj.select_student,
                        "courseArray": $scope.courseArray,
                        "FilePath_Array": $scope.filedoc,
                        "designationlist": $scope.arrayuserdesig,
                        "departmentlist": $scope.arrayuserdept,
                        "employeearraylist": $scope.employeearraylist

                    }
                }
                apiService.create("ClgNoticeBoard/savedata", data).then(function (promise) {

                        if (promise.returnval != null && promise.already_cnt != null) {
                            if (promise.already_cnt == false) {
                                if (promise.returnval == true) {
                                    if ($scope.intB_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.intB_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.noticemodel = function (id) {
            var data = {
                "INTB_Id": id,
            }
            apiService.create("ClgNoticeBoard/getNoticedata", data).
                then(function (promise) {
                    $scope.noticedetails = promise.noticedetails;
                    $scope.staffnoticedetails = promise.staffnoticedetails;
                    $scope.notice = $scope.staffnoticedetails[0].intB_Title;
                    $scope.notice = $scope.noticedetails[0].intB_Title;
                })
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.INTB_Id = item.intB_Id;
            var dystring = "";
            if (item.intB_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.intB_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgNoticeBoard/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.deactivedetails = function (item, SweetAlert) {
            $scope.INTBCB_Id = item.intbcB_Id;
            var dystring = "";
            if (item.intbcB_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.intbcB_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgNoticeBoard/deactivedetails", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.staffdeactivedetail = function (item, SweetAlert) {
            $scope.INTBCSTF_Id = item.intbcstF_Id;
            var dystring = "";
            if (item.intbcstF_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.intbcstF_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgNoticeBoard/deactivedetails", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        //=========================================================check for staff and student
        $scope.checkteru = function () {
            if ($scope.check_student == true && $scope.check_staff == false) {
                $scope.check_student = true;
            }
            else if ($scope.check_student == false && $scope.check_staff == true) {
                $scope.check_staff = true;
            }
            else if ($scope.check_student == true && $scope.check_staff == true) {
                $scope.check_staff = true;
                $scope.check_student = true;
            }
            else if ($scope.check_student == false && $scope.check_staff == false) {
                $scope.check_staff = false;
                $scope.check_student = true;
            }
        }
        //  -------------------------------------------------------------edit

        $scope.edit = function (id) {
            var data = {
                "INTB_Id": id
            }
            apiService.create("ClgNoticeBoard/editdetails", data).then(function (promise) {

                $scope.editdetails = [];
                if (promise.editdetails.length > 0) {
                    $scope.intB_Id = promise.editdetails[0].intB_Id;
                    $scope.intB_Title = promise.editdetails[0].intB_Title;
                    $scope.intB_Description = promise.editdetails[0].intB_Description;
                    $scope.file_detail = promise.editdetails[0].intB_Attachment;
                    $scope.intB_StartDate = new Date(promise.editdetails[0].intB_StartDate);
                    $scope.intB_EndDate = new Date(promise.editdetails[0].intB_EndDate);
                    $scope.intB_FilePath = promise.editdetails[0].intB_FilePath
                    $scope.notice = promise.editdetails[0].intB_FilePath
                    $scope.ntB_TTSylabusFlg = promise.editdetails[0].ntB_TTSylabusFlg;
                    $scope.intB_DispalyDisableFlg = promise.editdetails[0].intB_DispalyDisableFlg;
                    $scope.amcO_Id = promise.editdetails[0].amcO_Id;

                    if (promise.editdetails[0].intB_DispalyDisableFlg == false) {
                        $scope.intB_DisplayDate = ""
                    }
                    else {
                        $scope.intB_DispalyDisableFlg = true;
                        $scope.intB_DisplayDate = new Date(promise.editdetails[0].intB_DisplayDate);
                    }
                }

                $scope.editdetails = promise.editdetails;
                $scope.coursebranchsem = promise.coursebranchsem
                angular.forEach($scope.coursebranchsem, function (b) {
                    b.branch = false;
                    angular.forEach($scope.editdetails, function (ed) {
                        if (b.amB_Id == ed.amB_Id) {
                            b.branch = true;
                        }
                    })
                })
                $scope.allB = $scope.coursebranchsem.every(function (role) {
                    return role.branch;
                });

                angular.forEach($scope.coursebranchsem, function (s) {
                    b.branch = false;
                    angular.forEach($scope.editdetails, function (ed) {
                        if (s.amsE_Id == ed.amsE_Id) {
                            s.sem = true;
                        }
                    })
                })
                $scope.allS = $scope.coursebranchsem.every(function (role) {
                    return role.sem;
                });
            })

        }


        //-----------------------------------------------------------------


        //============Multiple file upload===========
        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //-------------------ADD------------------
        $scope.urldocumentlist = [{ id: 'document' }];
        $scope.addNewurl = function () {
            var newItemNo = $scope.urldocumentlist.length + 1;
            if (newItemNo <= 30) {
                $scope.urldocumentlist.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewurl = function (index, data) {
            var newItemNo = $scope.urldocumentlist.length - 1;
            $scope.urldocumentlist.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //======================= file upload
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
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/NoticeUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.INTBFL_FilePath = d[0].path;
                        data.FileName = d[0].name;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }


        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


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



            }
            else {
                $window.open($scope.imagepreview)
            }
        };

        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }





        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.arrayuserdept = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });

            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg == '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
        };


        $scope.Deptselectiondetails = function () {
            $scope.arrayuserdept = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });
            var data = {
                "departmentlist": $scope.arrayuserdept,
                  "ASMAY_Id": $scope.ASMAY

            };
            apiService.create("ClgNoticeBoard/Deptselectiondetails", data).then(function (promise) {
                if (promise.designation != null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;

                }
                else {

                    swal("No Record  Found..... !!");
                }
            });
        };


        ////===========designation============
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_userEmplist = [];
            $scope.arrayuserdesig = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdesig.push(em);
                }
            });

            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });

            $scope.des_test = true;
            $scope.togchkbxdesg();
        };

        $scope.togchkbxdesg = function (groupType) {
            //if ($scope.des_test == true) {
            //    var data = {
            //        "designationlist": $scope.arrayuserdesig,
            //        "departmentlist": $scope.arrayuserdept
            //    };
            //}
            //else {
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.arrayuserdesig = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdesig.push(em);
                }
            });

            $scope.arrayuserdept = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });
            var data = {
                "designationlist": $scope.arrayuserdesig,
                "departmentlist": $scope.arrayuserdept,

            };
           

            apiService.create("ClgNoticeBoard/Desgselectiondetails", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0) {

                    $scope.des_test = false;
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if ($scope.get_userEmplist.length > 0) {
                        angular.forEach($scope.get_userEmplist, function (uem) {
                            uem.empck = false;
                        });
                        $scope.checkall = false;
                    }
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };




        //---------all checkbox Select...
        $scope.all_check = function (checkall) {
            $scope.userc = checkall;
            var toggleStatus = $scope.userc;
            angular.forEach($scope.get_userEmplist, function (role) {
                role.empck = toggleStatus;
            });

            $scope.employeearraylist = [];
            angular.forEach($scope.get_userEmplist, function (qq) {
                if (qq.empck == true) {
                    $scope.employeearraylist.push({ HRME_Id: qq.HRME_Id })
                }
            })
        }



        $scope.all_checkS = function (all) {
            $scope.usercheckS = all;
            var toggleStatus = $scope.usercheckS;
            angular.forEach($scope.studentlist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.studentarray = [];
            angular.forEach($scope.studentlist, function (qq) {
                if (qq.selected == true) {
                    $scope.studentarray.push({ AMST_Id: qq.amsT_Id })
                }
            })



        };
        // get section
        $scope.getsection = function (ASMCL_Id) {

            //$scope.asmclid = ASMCL_Id;
            //if ($scope.asmclid === "All") {
            //    $scope.classlistarray = [];
            //    $scope.getclass = true;
            //    angular.forEach($scope.classlist, function (aa) {
            //        $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
            //    });
            //}
            //else {
            //    var data = {
            //        "ASMCL_Id": ASMCL_Id
            //    }
            //    apiService.create("NoticeBoard/getsection", data).then(function (promise) {
            //        if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
            //            $scope.sectionlist = promise.sectionlist;
            //            $scope.getclass = false;
            //        }
            //        else {
            //            swal('No data Found!!!');
            //        }
            //    });
            //}


            //added by roopa//
            $scope.classlistarray = [];

            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
                }

            });

            if ($scope.classlistarray != null) {
                $scope.classflag = true;
            }

            var data = {
                "classlsttwo": $scope.classlistarray
            }
            apiService.create("NoticeBoard/getsection", data).then(function (promise) {
                if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
                    $scope.sectionlist = promise.sectionlist;
                    $scope.getclass = false;
                }
                else {
                    swal('No data Found!!!');
                }
            });
            //
        }
        //============employee list======
        $scope.togchkbx = function () {
            $scope.employeearraylist = [];
            angular.forEach($scope.get_userEmplist, function (qq) {
                if (qq.empck == true) {
                    $scope.employeearraylist.push({ HRME_Id: qq.HRME_Id })
                }
            })
        }


    };
})();

