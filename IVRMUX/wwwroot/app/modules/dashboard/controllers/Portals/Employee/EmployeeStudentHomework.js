(function () {
    'use strict';
    angular.module('app').controller('EmployeeStudentHomeworkController', HomeworkController);
    HomeworkController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce'];
    function HomeworkController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce) {

        $scope.UploadFile = [];
        $scope.updshow = false;
        $scope.images_temp = [];
        $scope.minDatemf = new Date();
        $scope.ihW_Date = new Date();
        var id = 1;
        $scope.search = "";
        $scope.searchmarks = "";

        $scope.cls = false;
        $scope.obj = {};
        $scope.obj.IHW_IdTopic = "";
        $scope.obj.IHW_IdTopic = 0;
        //--------for sorting....
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.all_check = function () {
            var checkStatus2 = $scope.usercheck;
            angular.forEach($scope.sectionlist, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };
        //==========Load data
        $scope.loaddata = function () {
            $scope.ldrnew = true;
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage2 = 10;
            apiService.getURI("EmployeeStudentHomework/Getdetails", id).then(function (promise) {
                $scope.ldr = false;
                $scope.hide = true;
                $scope.chck = false;
                $scope.checklink = false;
                $scope.hrmE_Id = promise.hrmE_Id;
                $scope.studetiallist = promise.yearlist;
                $scope.asmaY_Id = promise.academicyear[0].asmaY_Id;
                $scope.classwork = promise.classwork;
                $scope.classlist = promise.classlist;
                $scope.marksupdate_list = promise.marksupdate_list;
                $scope.homeworklist1 = [];
                angular.forEach($scope.classwork, function (qq) {
                    var img = qq.ihW_FilePath;
                    if (img != null) {
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.filetype2 = lastelement;
                    }

                    if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                        $scope.homeworklist1.push({ ismS_SubjectName: qq.ismS_SubjectName, asmcL_ClassName: qq.asmcL_ClassName, asmC_SectionName: qq.asmC_SectionName, ihW_Topic: qq.ihW_Topic, ihW_Assignment: qq.ihW_Assignment, ihW_AssignmentNo: qq.ihW_AssignmentNo, ihW_Attachment: qq.ihW_Attachment, ihW_Topic: qq.ihW_Topic, ihW_ActiveFlag: qq.ihW_ActiveFlag, ihW_Date: qq.ihW_Date, asmaY_Id: qq.asmaY_Id, ivrmuL_Id: qq.ivrmuL_Id, ihW_FilePath: qq.ihW_FilePath, ihW_Id: qq.ihW_Id, imgvideo: '0', FilesCount: qq.FilesCount, createdDate: qq.CreatedDate, createdTime: qq.CreatedTime })

                    }
                    else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' || $scope.filetype2 == 'pdf' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                        $scope.homeworklist1.push({ ismS_SubjectName: qq.ismS_SubjectName, asmcL_ClassName: qq.asmcL_ClassName, asmC_SectionName: qq.asmC_SectionName, ihW_Topic: qq.ihW_Topic, ihW_Assignment: qq.ihW_Assignment, ihW_AssignmentNo: qq.ihW_AssignmentNo, ihW_Attachment: qq.ihW_Attachment, ihW_Topic: qq.ihW_Topic, ihW_ActiveFlag: qq.ihW_ActiveFlag, ihW_Date: qq.ihW_Date, asmaY_Id: qq.asmaY_Id, ivrmuL_Id: qq.ivrmuL_Id, ihW_FilePath: qq.ihW_FilePath, ihW_Id: qq.ihW_Id, imgvideo: '1', FilesCount: qq.FilesCount, createdDate: qq.CreatedDate, createdTime: qq.CreatedTime })
                    }
                    else {
                        $scope.homeworklist1.push({ ismS_SubjectName: qq.ismS_SubjectName, asmcL_ClassName: qq.asmcL_ClassName, asmC_SectionName: qq.asmC_SectionName, ihW_Topic: qq.ihW_Topic, ihW_Assignment: qq.ihW_Assignment, ihW_AssignmentNo: qq.ihW_AssignmentNo, ihW_Attachment: qq.ihW_Attachment, ihW_Topic: qq.ihW_Topic, ihW_ActiveFlag: qq.ihW_ActiveFlag, ihW_Date: qq.ihW_Date, asmaY_Id: qq.asmaY_Id, ivrmuL_Id: qq.ivrmuL_Id, ihW_FilePath: qq.ihW_FilePath, ihW_Id: qq.ihW_Id, FilesCount: qq.FilesCount, createdDate: qq.CreatedDate, createdTime: qq.CreatedTime })
                    }
                })

                $scope.classwork = $scope.homeworklist1;
                $scope.ldrnew = false;
            });
        };


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/pptx" || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
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
        //----------End..........!

        //========Remove Selected File & Image!
        $scope.remove_file = function () {
            $scope.notice = [];
            $scope.files = "";
            $scope.ldr = false;

        };
        //===========================


        //=============For Save Data!

        $scope.selectedImages = [];
        $scope.submitted = false;
        $scope.savedata = function () {

            var data = {};
            var todaysdate = Date.now();
            var date = todaysdate === null ? "" : $filter('date')(todaysdate, "yyyy-MM-dd");
            var hwdate = $scope.ihW_Date === null ? "" : $filter('date')($scope.ihW_Date, "yyyy-MM-dd");
            if (hwdate !== date) {
                swal("Please Select Today's Date");
            }
            else {
                if ($scope.myForm1.$valid) {

                    $scope.hm_section_list = [];
                    angular.forEach($scope.sectionlist, function (lia) {
                        if (lia.selectedd2 === true) {
                            $scope.hm_section_list.push({ ASMS_Id: lia.asmS_Id });
                        }
                    });
                    if ($scope.myForm1.$valid) {
                        $scope.ldrnew = true;
                        //-------file upload
                        $scope.filedoc = [];
                        $scope.documentListOtherDetails11 = [];
                        if ($scope.documentListOtherDetails != null) {
                            angular.forEach($scope.documentListOtherDetails, function (qq) {
                                if (qq.IHW_FilePath != null) {
                                    $scope.documentListOtherDetails11.push({ IHW_FilePath: qq.IHW_FilePath, FileName: qq.FileName });
                                }
                            })
                            $scope.filedoc = $scope.documentListOtherDetails11;
                        }

                        if ($scope.checklink == true) {
                            angular.forEach($scope.urldocumentlist, function (qq) {
                                if (qq.IHW_FilePath != null) {
                                    $scope.filedoc.push({ IHW_FilePath: qq.IHW_FilePath, FileName: qq.IHW_FilePath });
                                }
                            });
                        }


                        //-------file upload
                        if ($scope.ihW_AssignmentNo == "" || $scope.ihW_AssignmentNo == undefined) {
                            $scope.ihW_AssignmentNo = undefined;
                        }
                        data = {
                            "IHW_Id": $scope.ihW_Id,
                            "ASMAY_Id": $scope.asmaY_Id,
                            "ASMCL_Id": $scope.asmcL_Id,
                            hm_section_list: $scope.hm_section_list,
                            "ISMS_Id": $scope.ismS_Id,
                            "IHW_Topic": $scope.ihW_Topic,
                            "IHW_Assignment": $scope.ihW_Assignment,
                            "IHW_AssignmentNo": $scope.ihW_AssignmentNo,
                            "IHW_Date": hwdate,
                            "IHW_Attachment": $scope.file_name,
                            "IHW_FilePath_Array": $scope.filedoc
                        };
                    }

                    apiService.create("EmployeeStudentHomework/savedetail", data).then(function (promise) {
                        $scope.ldrnew = false;
                        if (promise.returnval !== null && promise.dulicate !== null) {
                            if (promise.dulicate === false) {
                                if (promise.returnval === true) {
                                    if ($scope.ihW_Id > 0) {
                                        swal('Record Updated Successfully!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                    $state.reload();
                                }
                                else {
                                    if (promise.returnval === false) {
                                        if ($scope.ihW_Id > 0) {
                                            swal('Record Not Update Successfully!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already Exist!");
                            }

                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    });
                }
                else {
                    $scope.submitted = true;
                }
            }
        };



        //======Edit Record =====//
        $scope.editData = function (option) {
            $scope.ldrnew = true;
            $scope.checklink = false;
            var data = {
                "IHW_Id": option.ihW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            $scope.homework_id = "";
            $scope.year_id = "";
            $scope.homework_id = option.ihW_Id;
            $scope.year_id = option.asmaY_Id;
            apiService.create("EmployeeStudentHomework/editData", data).then(function (promise) {
                $scope.cls = true;

                $scope.editlist = promise.editlist;
                if (promise.editlist.length > 0) {
                    $scope.section_list_temp = [];
                    $scope.section_list_temp = promise.editlist_section;
                    $scope.ihW_Id = promise.editlist[0].ihW_Id;

                    $scope.ihW_Date = new Date(promise.editlist[0].ihW_Date);
                    if (promise.editlist[0].ihW_Attachment == 'HyperLink') {
                        $scope.checklink = true;
                    }
                    else {
                        $scope.checklink = false;
                    }
                    $scope.file_detail = promise.editlist[0].ihW_Attachment;
                    $scope.ihW_Topic = promise.editlist[0].ihW_Topic;
                    $scope.ihW_Assignment = promise.editlist[0].ihW_Assignment;
                    $scope.ihW_AssignmentNo = promise.editlist[0].ihW_AssignmentNo;

                    $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                    $scope.onyearchange($scope.asmaY_Id);

                    $scope.asmcL_Id = promise.editlist[0].asmcL_Id;

                    if (promise.attachementlist != null || promise.attachementlist > 0) {
                        $scope.documentListOtherDetails = [];
                        $scope.urldocumentlist = [];
                        angular.forEach(promise.attachementlist, function (aa) {
                            $scope.img = aa.ihwatT_Attachment;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];

                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {

                                $scope.documentListOtherDetails.push({
                                    id: 1, IHW_FilePath: aa.ihwatT_Attachment,
                                    FileName: aa.ihwatT_FileName
                                });

                            }
                            else {
                                $scope.urldocumentlist.push({
                                    id: 1, IHW_FilePath: aa.ihwatT_Attachment,
                                    FileName: aa.ihwatT_FileName
                                });
                            }
                        });
                    }
                    if ($scope.urldocumentlist.length > 0) {
                        $scope.checklink = true;
                    }

                    //$scope.attachementlistnew = [];
                    //$scope.attachementlistnew = promise.attachementlist;
                    //   if ($scope.checklink == false) {
                    //    if ($scope.attachementlistnew > 0 || $scope.attachementlistnew != null) {
                    //        $scope.documentListOtherDetails = [];
                    //        angular.forEach($scope.attachementlistnew, function (aa) {
                    //            $scope.documentListOtherDetails.push({ id: 1, IHW_FilePath: aa.ihwatT_Attachment, FileName: aa.ihwatT_FileName });
                    //        })
                    //    }
                    //}
                    //else {
                    //    $scope.link_url_all = $scope.attachementlistnew[0].ihwatT_Attachment;
                    //}

                    //====================================================
                    $scope.sub = "";
                    $scope.sub = promise.editlist[0].asmS_Id;
                    // $scope.onclasschange($scope.asmcL_Id);
                    $scope.asmS_Id = promise.editlist[0].asmS_Id;
                    $scope.getsub($scope.asmS_Id);
                    $scope.ismS_Id = promise.editlist[0].ismS_Id;
                    $scope.sectionlist = promise.sectionlist;

                    angular.forEach(promise.sectionlist, function (tt) {
                        angular.forEach($scope.section_list_temp, function (ss) {
                            if (tt.asmS_Id == ss.asmS_Id) {
                                tt.selectedd2 = true;
                            }
                        })
                    });
                }
                else {
                    swal('Data Not Available For Edit!');
                }
                $scope.ldrnew = false;
            });
        };


        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "IHW_Id": option.ihW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            apiService.create("EmployeeStudentHomework/viewData", data).then(function (promise) {

                if (promise.attachementlist.length > 0) {
                    $scope.attachementlist1 = [];
                    angular.forEach(promise.attachementlist, function (qq) {
                        $scope.img = qq.ihwatT_Attachment;
                        if ($scope.img != null) {
                            var imagarr = $scope.img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];

                            $scope.filetype2 = lastelement;
                        }

                        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                            $scope.attachementlist1.push({
                                ihwatT_FileName: qq.ihwatT_FileName,
                                ihW_Attachment: qq.ihW_Attachment,
                                ihW_Id: qq.ihW_Id, ihwatT_Attachment: qq.ihwatT_Attachment
                            })
                        }
                        else {
                            $scope.attachementlist1.push({
                                ihwatT_FileName: qq.ihwatT_FileName,
                                ihW_Attachment: 'HyperLink',
                                ihW_Id: qq.ihW_Id, ihwatT_Attachment: qq.ihwatT_Attachment
                            })
                        }
                    });

                    $scope.attachementlist = $scope.attachementlist1

                    $('#myModalCoverview').modal('show');
                    $scope.docshowary = true;
                    $scope.docshow = false;
                }
                else {
                    swal("No Data Found.");
                }
            });
        };

        $scope.viewDatalink = function (option) {
            $scope.attachementlist = [];
            var data = {
                "IHW_Id": option.ihW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            apiService.create("EmployeeStudentHomework/viewData", data).then(function (promise) {

                if (promise.attachementlist.length > 0) {

                    $scope.ihwatT_FileName = promise.attachementlist[0].ihwatT_Attachment;
                    $window.open($scope.ihwatT_FileName);
                }
                else {
                    swal("No Data Found.");
                }
            });
        };

        $scope.viewData_doc = function () {
            $scope.attachementlist1 = [];
            if ($scope.homework_id == "" && $scope.year_id == "" || $scope.homework_id == null && $scope.year_id == null || $scope.homework_id == undefined && $scope.year_id == undefined) {
                $scope.attachementlist1 = $scope.notice;
                $scope.docshow = true;
                $scope.docshowary = false;
            }
            else {
                var data = {
                    "IHW_Id": $scope.homework_id,
                    "ASMAY_Id": $scope.year_id
                };
                apiService.create("EmployeeStudentHomework/viewData", data).then(function (promise) {
                    $scope.attachementlist2 = [];

                    if (promise.attachementlist.length > 0) {
                        $scope.attachementlist2 = promise.attachementlist;
                        angular.forEach($scope.attachementlist2, function (qq) {
                            $scope.attachementlist1.push({ IHW_FilePath: qq.ihwatT_Attachment, IHWATT_FileName: qq.ihwatT_FileName })
                        })
                        $('##myModalCoverview').modal('show');
                        $scope.docshow = true;
                        $scope.docshowary = false;
                    }
                    else {
                        swal("No Data Found.")
                    }

                });
            }
        };
        //===========End Edit Record =======//

        $scope.getmodeldetails = function (option) {
            var data = {
                "IHW_Id": option.ihW_Id,
                "ASMAY_Id": option.asmaY_Id
            };

            apiService.create("EmployeeStudentHomework/editData", data).then(function (promise) {
                $scope.cls = true;
                $scope.allimages = [];
                $scope.editlist = promise.editlist;
                if (promise.editlist.length > 0) {
                    if (promise.editlist[0].ihW_FilePath != undefined || promise.editlist[0].ihW_FilePath != '' || promise.editlist[0].ihW_FilePath != null) {
                        var img = promise.editlist[0].ihW_FilePath;
                        if (img != null) {
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];

                            $scope.filetype2 = lastelement;
                        }

                    }
                    if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                        $scope.filenames = "Video";;
                        $scope.videofile = true;
                        $scope.imgfile = false;
                        $scope.stepsModel = [];
                        $scope.allimages.push({ ihW_FilePath: img })
                    }
                    else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' || $scope.filetype2 == 'pdf' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {

                        $scope.allimages.push({ ihW_FilePath: img })
                        $scope.imgfile = true;
                        $scope.videofile = false;

                    }

                    //$scope.notice = promise.editlist[0].ihW_FilePath;

                }
                else {
                    swal('Data Not Available For Edit!');
                }
            });
        };
        //=======================================
        $scope.isOptionsRequired = function () {

            return !$scope.sectionlist.some(function (sec) {
                return sec.selectedd2;
            });
        }
        $scope.isOptionsRequiredstudent = function () {

            return !$scope.studentlist.some(function (sec) {
                return sec.selectedd2;
            });
        }

        $scope.interacted2 = function (field) {

            return $scope.submitted;
        };


        //==========selected year!
        $scope.onyearchange = function (ASMAY_Id) {
            $scope.sectionlist = [];
            $scope.asmcL_Id = "";
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            $scope.ISMS_Id = "";
            $scope.getsubject_list = "";
            $scope.gethomework_list = [];
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("EmployeeStudentHomework/get_classes", data).then(function (promise) {
                if (promise.classlist.length > 0) {

                    $scope.classwork = promise.class_gridlist;
                    $scope.classlist = promise.classlist;

                }
                else {
                    swal('Data Not Available');
                    $scope.asmcL_Id = "";
                }
            });
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.sectionlist.every(function (options) {
                return options.selectedd2;
            });
        }
        $scope.togchkbx123 = function () {
            $scope.usercheck = $scope.studentlist.every(function (options) {
                return options.selectedd2;
            });
        }
        $scope.togchkbx1 = function () {
            $scope.usercheck = $scope.studentlist.every(function (options) {
                return options.selectedd2;
            });
        }

        //==========selected class!
        $scope.onclasschange = function () {

            $scope.ASMS_Id = "";
            $scope.sectionlist = [];
            $scope.ISMS_Id = "";
            $scope.getsubject_list = [];
            $scope.gethomework_list = [];
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("EmployeeStudentHomework/getsectiondata", data).then(function (promise) {

                if (promise.sectionlist.length > 0) {
                    $scope.sectionlist = promise.sectionlist;
                    if ($scope.sub > 0) {

                        if ($scope.section_list_temp.length > 0) {
                            angular.forEach($scope.sectionlist, function (ss) {
                                angular.forEach($scope.section_list_temp, function (qq) {
                                    {
                                        if (qq.asmS_Id == ss.asmS_Id) {
                                            ss.selectedd2 = true;
                                            $scope.sub = "";
                                        }
                                    }

                                });
                            });
                        }
                    }
                }
                else {
                    swal('Data Not Available!');
                    $scope.asmS_Id = "";
                }

            });
        };



        //==========selected setion.!
        //  $scope.sectionchange = function () {
        $scope.getsub = function () {
            if ($scope.sub > 0) {
                $scope.hm_section_list = [];
                $scope.hm_section_list.push({ ASMS_Id: $scope.sub });
                $scope.sub = "";
            }
            else {

                $scope.hm_section_list = [];
                angular.forEach($scope.sectionlist, function (lia) {
                    if (lia.selectedd2 === true) {
                        $scope.hm_section_list.push({ ASMS_Id: lia.asmS_Id });
                    }
                });
            }

            $scope.subjectlist = [];
            $scope.ismS_Id = "";
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                hm_section_list: $scope.hm_section_list,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("EmployeeStudentHomework/getsubject", data).then(function (promise) {

                if (promise.subjectlist.length > 0) {
                    $scope.subjectlist = promise.subjectlist;
                }
                else {
                    swal('Data Not Available!');
                    $scope.ismS_Id = "";
                }

            });
        };


        //=====for activation & deactivation
        $scope.Deactivate = function (data, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (data.ihW_ActiveFlag === true) {
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

                        apiService.create("EmployeeStudentHomework/deactivate", data).then(function (promise) {

                            if (promise.returnval === true) {
                                swal(confirmmgs + " " + "successfully.");
                            }
                            else {
                                swal(confirmmgs + " " + " successfully");
                            }
                            $state.reload();
                        });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.clear_data = function () {
            $state.reload();
        };


        //======================================== Shiva
        $scope.previewVideo = function () {

            $scope.allimages = [];
            $scope.videofile = true;
            $scope.imgfile = false;
            $scope.allimages.push({ ihW_FilePath: $scope.notice[0] });

        };
        //=============================== Preview Image

        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }

        $scope.previewimg = function (img) {

            $scope.imagepreview = img.ihwatT_Attachment;
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
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
                //$window.location.href = $scope.imagepreview;

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
        //================================ Section Selection
        $scope.mediachange = function () {
            $scope.images_paths = [];
            $scope.stepsModel = [];
            $scope.filenames = "Video";

        };
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
        //================================ Video 
        $scope.videoUpload = function (input, document) {
            $scope.ldr = true;
            $scope.hide = false;
            $scope.files = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "video/mp4" || input.files[0].type === "video/x-ms-wmv") {

                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadImgs();

                }
                else if (input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "xlsx" ||
                    input.files[0].type === "xls" || input.files[0].type === "image/jpeg" || input.files[0].type === "image/png"
                    || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf"
                    || input.files[0].type === "application/pptx" || input.files[0].type === "application/vnd.ms-powerpoint"
                    || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation"
                    || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                    || input.files[0].type === "audio/mpeg") {
                    $scope.stepsModel = [];
                    $scope.files = input.files;
                    for (var i = 0; i < $scope.files.length; i++) {
                        var file = $scope.files[i];
                        $scope.fileimg = file;
                        var reader = new FileReader();
                        reader.onload = $scope.imageIsLoaded;
                        reader.readAsDataURL(file);
                        UploadImgs();
                    }

                    $scope.imageIsLoaded = function (e) {
                        $scope.$apply(function () {
                            $scope.stepsModel.push(e.target.result);
                        });
                    };
                }

            }
        };

        //================================ Upload       
        $scope.view_videos = [];
        function UploadImgs() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                $scope.filenames = "Videos";

            }
            formData.append("Id", 0);
            var defer = $q.defer();

            $http.post("/api/ImageUpload/HomeworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.images_paths = d;

                    if ($scope.images_paths.length > 0) {
                        $scope.notice = [];
                        angular.forEach($scope.images_paths, function (imgp) {
                            $scope.notice.push({ IHW_FilePath: imgp.path, FileName: imgp.name });
                        });
                        $scope.ldr = false;
                        $scope.hide = true;
                    }
                    if ($scope.notice.length > 0) {
                        $scope.homework_id = undefined;
                        $scope.year_id = undefined;

                    }

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }




        //============Home work marks entry update==============

        $scope.gethomework_student = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("EmployeeStudentHomework/gethomework_student", data).then(function (promise) {
                if (promise.studentlist1.length > 0) {
                    $scope.studentlist = promise.studentlist1;
                }
                else {
                    swal("No data Found!!!")
                }
            })
        }
        //-------------------------------
        $scope.getsubject = function () {
            $scope.getsubject_list = [];
            $scope.ISMS_Id = "";
            $scope.gethomework_list = [];
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ASMAY_Id": $scope.asmaY_Id

            }
            apiService.create("EmployeeStudentHomework/getsubjectlist", data).then(function (promise) {
                if (promise.getsubject_list != null && promise.getsubject_list.length > 0) {
                    $scope.getsubject_list = promise.getsubject_list
                }
            });
        };

        //------------------------------
        $scope.gethomework_list12 = function () {
            $scope.submitted = false;
            $scope.gethomework_list = [];
            $scope.TopicList = [];
            if ($scope.myForm1.$valid) {
                if ($scope.fromdate != undefined) {
                    var startdate = $filter('date')($scope.fromdate, "yyyy-MM-dd");
                }
                else {
                    var startdate = "";
                }
                if ($scope.todate != undefined) {
                    var enddate = $filter('date')($scope.todate, "yyyy-MM-dd");
                }
                else {
                    var enddate = "";
                }
                if ($scope.ISMS_Id == "") {
                    $scope.ISMS_Id = undefined;
                }
                var data = {
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "fromdate": startdate,
                    "todate": enddate
                    // "studentarray": $scope.studentarray
                }
                apiService.create("EmployeeStudentHomework/gethomework_list", data).then(function (promise) {
                    $scope.gethomework_list = [];
                    if (promise.gethomework_list.length > 0) {
                        $scope.gethomework_list1 = [];
                        $scope.gethomework_list = promise.gethomework_list;

                        angular.forEach($scope.gethomework_list, function (dd) {
                            dd.checkedvalue = false;
                            if (dd.IHWUPL_Marks !== null) {
                                dd.checkedvalue = true;
                            }
                        });

                        $scope.optionToggled();
                        $scope.chck = false;
                        $scope.TopicList = promise.topicList;

                    }
                    else {
                        swal('No Date Found!!!');
                        $scope.TopicList = [];
                        $scope.obj.IHW_IdTopic = "";
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.StudentTopiclist = function () {
            $scope.getclasswork_list = [];
            var startdate = ""; var enddate = ""; var ISMS_Id = 0; var ASMCL_Id = 0; var ASMS_Id = 0; var ASMAY_Id = 0;
            if ($scope.TopicList != null && $scope.TopicList.length > 0 && $scope.obj.IHW_IdTopic > 0) {
                angular.forEach($scope.TopicList, function (itm) {
                    if (itm.IHW_Id == $scope.obj.IHW_IdTopic) {
                        startdate = $filter('date')(itm.IHW_Date, "yyyy-MM-dd");
                        enddate = $filter('date')($scope.todate, "yyyy-MM-dd");
                        ISMS_Id = itm.ISMS_Id;
                        ASMCL_Id = itm.ASMCL_Id;
                        ASMS_Id = itm.ASMS_Id;
                        ASMAY_Id = itm.ASMAY_Id;
                    }

                });

                var data = {
                    "ASMCL_Id": ASMCL_Id,
                    "ASMS_Id": ASMS_Id,
                    "ASMAY_Id": ASMAY_Id,
                    "ISMS_Id": ISMS_Id,
                    "IHW_Id": $scope.obj.IHW_IdTopic,
                    "fromdate": startdate,
                    "todate": enddate,



                }
                apiService.create("EmployeeStudentHomework/gethomework_listTopic", data).then(function (promise) {
                    $scope.gethomework_list = [];
                    if (promise.gethomework_list.length > 0) {
                        $scope.gethomework_list1 = [];
                        $scope.gethomework_list = promise.gethomework_list;

                        angular.forEach($scope.gethomework_list, function (dd) {
                            dd.checkedvalue = false;
                            if (dd.IHWUPL_Marks !== null) {
                                dd.checkedvalue = true;
                            }
                        });

                        $scope.optionToggled();
                        $scope.chck = false;
                    }
                    else {
                        swal('No Date Found!!!')
                    }
                })


            }
            else {
                $scope.gethomework_list12();
            }
        }
        //---------------------------------
        $scope.homework_marks_update = function () {
            $scope.submittedhomework2 = false;
            if ($scope.myFormhomework.$valid) {
                $scope.gethomework_list_array = [];

                angular.forEach($scope.gethomework_list, function (qq) {
                    if (qq.checkedvalue == true) {
                        $scope.gethomework_list_array.push({
                            AMST_Id: qq.AMST_Id, IHW_Id: qq.IHW_Id, Marks: qq.IHWUPL_Marks,
                            IHWUPL_StaffUpload: qq.IHW_FilePath1, IHWUPL_FileName: qq.FileName1, IHWUPL_StaffRemarks: qq.IHWUPL_StaffRemarks,
                            IHWUPL_Id: qq.IHWUPL_Id, doclist_temp: qq.uploadfilestemp
                        });
                    }
                });

                var data = {
                    "gethomework_list_array": $scope.gethomework_list_array
                };

                apiService.create("EmployeeStudentHomework/homework_marks_update", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Marks Save Successfully.');
                        $state.reload();
                    }
                    else if (promise.returnval == false) {
                        swal('Failed To Save Marks');
                    }
                })
            } else {
                $scope.submittedhomework2 = true;
            }
        };

        $scope.interactedhomework2 = function () {
            return $scope.submittedhomework2;
        };

        $scope.isoptionrequiredhomework1 = function () {
            return !$scope.gethomework_list.some(function (options) {
                return options.checkedvalue;
            });
        };

        $scope.AddAndClose = function () {
            angular.forEach($scope.gethomework_list, function (dd) {
                if (dd.IHWUPL_Id === $scope.IHWUPL_Id_Temp) {
                    dd.uploadfilestemp = $scope.viewstudentuploadnew;
                }
            });
            $('#studentupload').modal('hide');
        };

        $scope.stfupload = function () {
            $scope.doclist = [];
            $scope.gethomework_list4 = [];
            angular.forEach($scope.viewstudentuploadnew, function (aa) {
                if (aa.FilePath1 != null && aa.FilePath1 != '' || aa.Remarks != null) {
                    $scope.gethomework_list4.push({ FilePath1: aa.FilePath1, FileName1: aa.FileName1, IHWUPL_Id: aa.ihwupL_Id, Remarks: aa.Remarks });
                }
            });
            if ($scope.gethomework_list4.length > 0) {
                var data = {
                    "doclist": $scope.gethomework_list4
                }
                apiService.create("EmployeeStudentHomework/stfupload", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Upload Saved Successfully.');
                        $state.reload();

                    }
                    else if (promise.returnval == false) {
                        swal('Upload Not Saved Successfully.');
                        $state.reload();

                    }
                    $('.modal-backdrop').remove();
                })
            }
        };

        $scope.cls = function () {
            $scope.updshow = false;
        };
        //-=-------------------------
        $scope.edit_homework_mark = function (wrk) {
            var data = {
                "AMST_Id": wrk.AMST_Id,
                "IHW_Id": wrk.IHW_Id
            }
            apiService.create("EmployeeStudentHomework/edit_homework_mark", data).then(function (promise) {

                if (promise.edit_mark_list !== null && promise.edit_mark_list.length > 0) {
                    $scope.gethomework_list = [];
                    $scope.gethomework_lis1 = [];
                    $scope.gethomework_list = promise.edit_mark_list
                    angular.forEach($scope.gethomework_list, function (dd) {
                        dd.checkedvalue = false;
                        if (dd.IHWUPL_Marks !== null) {
                            dd.checkedvalue = true;
                        }
                    });

                    $scope.optionToggled();

                    if ($scope.gethomework_list.length > 0) {
                        $scope.chck = true;
                    }
                    $scope.studentname = promise.editstudent[0].studentname
                    $scope.studentclass = promise.editstudent[0].ASMCL_ClassName
                    $scope.studentsection = promise.editstudent[0].ASMC_SectionName
                }
                else {
                    swal('No Data Found!!!')
                }
            })
        }

        $scope.viewhomework = function (work) {
            var data = {
                "IHW_Id": work.IHW_Id
            }
            apiService.create("EmployeeStudentHomework/viewhomework", data).then(function (promise) {

                if (promise.viewhomework.length > 0) {
                    $scope.viewhomeworknew = [];
                    $scope.attachementlist1 = [];
                    angular.forEach(promise.viewhomework, function (qq) {
                        $scope.img = qq.ihwatT_Attachment;
                        if ($scope.img != null) {
                            var imagarr = $scope.img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];

                            $scope.filetype2 = lastelement;
                        }

                        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                            $scope.attachementlist1.push({
                                ihwatT_FileName: qq.ihwatT_FileName,
                                ihW_Attachment: qq.ihW_Attachment,
                                ihW_Id: qq.ihW_Id, ihwatT_Attachment: qq.ihwatT_Attachment
                            })
                        }
                        else {
                            $scope.attachementlist1.push({
                                ihwatT_FileName: qq.ihwatT_FileName,
                                ihW_Attachment: 'HyperLink',
                                ihW_Id: qq.ihW_Id, ihwatT_Attachment: qq.ihwatT_Attachment
                            })
                        }
                    })

                    $scope.viewhomeworknew = $scope.attachementlist1;
                }
                else {
                    swal('No Data Found!!!')
                }
            })
        }

        $scope.viewstudentupload = function (ee) {
            $scope.IHWUPL_Id_Temp = ee.IHWUPL_Id;
            $scope.viewstudentuploadnew = [];
            $scope.tempupload = [];
            angular.forEach($scope.gethomework_list, function (dd) {
                if (dd.IHWUPL_Id === $scope.IHWUPL_Id_Temp) {
                    if (dd.uploadfilestemp !== undefined && dd.uploadfilestemp !== null && dd.uploadfilestemp.length > 0) {
                        $scope.tempupload = dd.uploadfilestemp;
                    }
                }
            });

            if ($scope.tempupload.length === 0) {
                var data = {
                    "AMST_Id": ee.AMST_Id,
                    "IHW_Id": ee.IHW_Id,
                    "IHWUPL_Id": ee.IHWUPL_Id
                }
                apiService.create("EmployeeStudentHomework/viewstudentupload", data).then(function (promise) {
                    if (promise.viewstudentupload !== null && promise.viewstudentupload.length > 0) {
                        $scope.viewstudentuploadnew = promise.viewstudentupload;
                        angular.forEach($scope.viewstudentuploadnew, function (dd) {
                            dd.FilePath1 = "";
                            dd.Remarks = dd.ihwuplatT_StaffRemarks;
                            if (dd.ihwuplatT_StaffUpload !== null && dd.ihwuplatT_StaffUpload !== "") {
                                dd.FilePath1 = dd.ihwuplatT_StaffUpload;
                                var nameArray = dd.ihwuplatT_StaffUpload.split('.');
                                var extention = nameArray[nameArray.length - 1];
                                dd.filetype = extention;
                            }
                        });
                    }
                    else {
                        swal('No Data Found!!!')
                    }
                })
            } else {
                $scope.viewstudentuploadnew = $scope.tempupload;
            }
        }

        //------------------- Preview Image
        $scope.previehomework = function (img) {
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
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }

            else if ($scope.filetype2 == 'pdf') {

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
        $scope.closeall = function () {
            $state.reload();
        }
        //-----------------------
        $scope.optionToggled = function (user) {
            $scope.all = $scope.gethomework_list.every(function (itm) { return itm.checkedvalue; })
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.gethomework_list, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }
        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

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
        //==============================

        //====================ADD=========================================
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
        //==============================

        $scope.selectFileforUploadzdOtherDetail = function (input, document) {


            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
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
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/HomeworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.IHW_FilePath = d[0].path;
                        data.FileName = d[0].name;
                        $scope.ldr = false;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        //===================
        $scope.selectFileforUploadzdOtherDetail1 = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail1 = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD').attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail1(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail1(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail1.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail1[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/HomeworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        $scope.IHW_FilePath1 = d[0].path;
                        $scope.FileName1 = d[0].name;

                        data.IHW_FilePath1 = $scope.IHW_FilePath1;
                        data.FileName1 = $scope.FileName1;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.interacted13 = function (field) {
            return $scope.submitted13;
        };

        $scope.downloaddirectimage = function (data, idd) {
            var imagedownload = "";
            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg
                    })[0].click();
                });
        };


        $scope.selectFileforUploadzdOtherDetail = function (input, document) {


            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
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
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/HomeworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.IHW_FilePath = d[0].path;
                        data.FileName = d[0].name;
                        $scope.ldr = false;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        //===================
        $scope.selectFileforUploadzdOtherDetail2 = function (input, document, gg) {

            $scope.IHWUPL_Id = document.ihwupL_Id;

            $scope.selectFileforUploadzdOtherDetail222 = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD').attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail2(document, $scope.IHWUPL_Id);

            }
        };
        function UploadEmployeeDocumentOtherDetail2(data, IHWUPL_Id) {
            $scope.gethomework_list5 = [];
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail2.length; i++) {
                formData.append("File", $scope.selectFileforUploadzdOtherDetail222[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/HomeworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.FilePath1 = d[0].path;
                        $scope.FileName1 = d[0].name;
                        $scope.IHWUPL_Id = IHWUPL_Id;

                        data.FilePath1 = $scope.FilePath1;
                        data.FileName1 = $scope.FileName1;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        //========================end===========================
    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

})();
