(function () {
    'use strict';

    angular
        .module('app')
        .controller('IVRM_ClassWorkController', IVRM_ClassWorkController);
    IVRM_ClassWorkController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce'];
    function IVRM_ClassWorkController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce) {

        $scope.UploadFile = [];
        $scope.search = "";
        $scope.searchmarks = "";

        var id = 1;
        $scope.hide = true;
        $scope.obj = {};
        $scope.obj.ICW_IdTopic = "";
        $scope.obj.ICW_IdTopic = 0;
        //--------for sorting....
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.rotate = 0;
        $scope.rotateImage = function () {
            $scope.rotate = $scope.rotate + 1;
            var img = document.getElementById('preview');
            if ($scope.rotate === 1) {
                img.style.transform = 'rotate(90deg)';
            }
            else if ($scope.rotate === 2) {
                img.style.transform = 'rotate(180deg)';
            }
            else if ($scope.rotate === 3) {
                img.style.transform = 'rotate(270deg)';
            }
            else if ($scope.rotate === 4) {
                img.style.transform = 'rotate(360deg)';
                $scope.rotate = 0;
            }
        };

        $scope.loaddata = function () {
            $scope.ldrnew = true;
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage1 = 10;
            apiService.getURI("IVRM_ClassWork/Getdetails", id).
                then(function (promise) {
                    $scope.ldr = false;
                    $scope.hide = true;
                    $scope.checklink = false;
                    $scope.studetiallist = promise.yearlist;
                    $scope.classlist = promise.classlist;
                    $scope.marksupdate_list = promise.marksupdate_list;
                    $scope.asmaY_Id = promise.academicyear[0].asmaY_Id;
                    $scope.hrmE_Id = promise.hrmE_Id;
                    $scope.classworklst = promise.classwork;
                    $scope.homeworklist1 = [];
                    angular.forEach($scope.classworklst, function (qq) {
                        var img = qq.icW_FilePath;
                        if (img != null) {
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            $scope.filetype2 = lastelement;
                        }

                        if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                            $scope.homeworklist1.push({
                                asmcL_ClassName: qq.asmcL_ClassName, asmcL_Id: qq.asmcL_Id, asmC_SectionName: qq.asmC_SectionName, asmS_Id: qq.asmS_Id, ismS_SubjectName: qq.ismS_SubjectName, ismS_Id: qq.ismS_Id, icW_Id: qq.icW_Id, icW_Content: qq.icW_Content, icW_Topic: qq.icW_Topic, icW_SubTopic: qq.icW_SubTopic, icW_FromDate: qq.icW_FromDate, icW_ToDate: qq.icW_ToDate, icW_ActiveFlag: qq.icW_ActiveFlag, icW_Assignment: qq.icW_Assignment, icW_TeachingAid: qq.icW_TeachingAid, icW_Evaluation: qq.icW_Evaluation, icW_Attachment: qq.icW_Attachment, icW_FilePath: qq.icW_FilePath, mI_Id: qq.mI_Id, asmaY_Id: qq.asmaY_Id, imgvideo: '0', FilesCount: qq.FilesCount, createdDate: qq.CreatedDate, createdTime: qq.CreatedTime
                            })
                        }
                        else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' || $scope.filetype2 == 'pdf' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                            $scope.homeworklist1.push({
                                asmcL_ClassName: qq.asmcL_ClassName, asmcL_Id: qq.asmcL_Id, asmC_SectionName: qq.asmC_SectionName, asmS_Id: qq.asmS_Id, ismS_SubjectName: qq.ismS_SubjectName, ismS_Id: qq.ismS_Id, icW_Id: qq.icW_Id, icW_Content: qq.icW_Content, icW_Topic: qq.icW_Topic, icW_SubTopic: qq.icW_SubTopic, icW_FromDate: qq.icW_FromDate, icW_ToDate: qq.icW_ToDate, icW_ActiveFlag: qq.icW_ActiveFlag, icW_Assignment: qq.icW_Assignment, icW_TeachingAid: qq.icW_TeachingAid, icW_Evaluation: qq.icW_Evaluation, icW_Attachment: qq.icW_Attachment, icW_FilePath: qq.icW_FilePath, mI_Id: qq.mI_Id, asmaY_Id: qq.asmaY_Id, imgvideo: '1', FilesCount: qq.FilesCount, createdDate: qq.CreatedDate, createdTime: qq.CreatedTime
                            })
                        }
                        else {
                            $scope.homeworklist1.push({
                                asmcL_ClassName: qq.asmcL_ClassName, asmcL_Id: qq.asmcL_Id, asmC_SectionName: qq.asmC_SectionName, asmS_Id: qq.asmS_Id, ismS_SubjectName: qq.ismS_SubjectName, ismS_Id: qq.ismS_Id, icW_Id: qq.icW_Id, icW_Content: qq.icW_Content, icW_Topic: qq.icW_Topic, icW_SubTopic: qq.icW_SubTopic, icW_FromDate: qq.icW_FromDate, icW_ToDate: qq.icW_ToDate, icW_ActiveFlag: qq.icW_ActiveFlag, icW_Assignment: qq.icW_Assignment, icW_TeachingAid: qq.icW_TeachingAid, icW_Evaluation: qq.icW_Evaluation, icW_Attachment: qq.icW_Attachment, icW_FilePath: qq.icW_FilePath, mI_Id: qq.mI_Id, asmaY_Id: qq.asmaY_Id, FilesCount: qq.FilesCount, createdDate: qq.CreatedDate, createdTime: qq.CreatedTime
                            })
                        }
                    })

                    $scope.classworklst = $scope.homeworklist1;
                    $scope.roleflg = 'Staff';
                    $scope.ldrnew = false;
                });
        };
        //====================
        $scope.isOptionsRequired = function () {
            return !$scope.sectionlist.some(function (sec) {
                return sec.selectedd2;
            });
        }
        $scope.StudentTopiclist = function () {
            $scope.getclasswork_list = [];
            var startdate = ""; var enddate = ""; var ISMS_Id = 0; var ASMCL_Id = 0; var ASMS_Id = 0; var ASMAY_Id = 0;
            if ($scope.TopicList != null && $scope.TopicList.length > 0 && $scope.obj.ICW_IdTopic > 0) {
                angular.forEach($scope.TopicList, function (itm) {
                    if (itm.ICW_Id == $scope.obj.ICW_IdTopic) {
                        startdate = $filter('date')(itm.ICW_FromDate, "yyyy-MM-dd");
                        enddate = $filter('date')(itm.ICW_ToDate, "yyyy-MM-dd");
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
                    "ICW_Id": $scope.obj.ICW_IdTopic,
                    "fromdate": startdate,
                    "todate": enddate

                }
                apiService.create("IVRM_ClassWork/getclasswork_Topiclist", data).then(function (promise) {
                    if (promise.getclasswork_list != null && promise.getclasswork_list.length > 0) {

                        $scope.getclasswork_list = promise.getclasswork_list;
                        angular.forEach($scope.getclasswork_list, function (dd) {
                            dd.checkedvalue = false;
                            if (dd.ICWUPL_Marks !== null) {
                                dd.checkedvalue = true;
                            }
                        });

                        $scope.chck = false;
                        $scope.optionToggled();

                    }
                    else {
                        swal('No Data Found!!!');

                    }
                })



            }
            else {
                //swal("Please Select Topic");
                $scope.getclasswork_list12();

                //
            }
        }
        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {
            $scope.UploadFile = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "video/mp4" || input.files[0].type === "video/x-ms-wmv" || input.files[0].type === "application/pptx" || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation" &&
                    input.files[0].size <= 31457280) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 31457280) {
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
        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.notice = [];
            $scope.files = "";
            $scope.ldr = false;
            $scope.fileflg = false;
        };
        $scope.submitted = false;
        $scope.savedata = function () {

            if ($scope.myForm1.$valid) {
                $scope.ldrnew = true;
                $scope.hm_section_list = [];
                angular.forEach($scope.sectionlist, function (lia) {
                    if (lia.selectedd2 === true) {
                        $scope.hm_section_list.push({ ASMS_Id: lia.asmS_Id });
                    }
                });
                //-------file upload
                $scope.att_file11 = [];
                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.ICW_FilePath != null) {
                            $scope.documentListOtherDetails11.push({ ICW_FilePath: qq.ICW_FilePath, FileName: qq.FileName });
                        }
                    })
                    $scope.att_file11 = $scope.documentListOtherDetails11;
                }

                if ($scope.checklink == true) {
                    angular.forEach($scope.urldocumentlist, function (qq) {
                        if (qq.ICW_FilePath != null) {
                            $scope.att_file11.push({ ICW_FilePath: qq.ICW_FilePath, FileName: qq.ICW_FilePath });
                        }


                    })

                }

                var data = {
                    "ICW_Id": $scope.icW_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    // "ASMS_Id": $scope.asmS_Id,
                    hm_section_list: $scope.hm_section_list,
                    "ISMS_Id": $scope.ismS_Id,
                    "ICW_Topic": $scope.work_topic,
                    "ICW_SubTopic": $scope.work_subtopic,
                    "ICW_Content": $scope.work_content,
                    "ICW_TeachingAid": $scope.icW_TeachingAid,
                    "ICW_Assignment": $scope.icW_Assignment,
                    "ICW_Attachment": $scope.file_name,
                    "ICW_Evaluation": $scope.icW_Evaluation,
                    "ICW_FromDate": new Date($scope.notice_EStartDate).toDateString(),
                    "ICW_ToDate": new Date($scope.notice_EndDate).toDateString(),
                    "ICW_FilePath_Array": $scope.att_file11
                };

                apiService.create("IVRM_ClassWork/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval !== null && promise.dulicate !== null) {
                            if (promise.dulicate === false) {
                                if (promise.returnval === true) {
                                    if ($scope.icW_Id > 0) {
                                        swal('Record Updated Successfully!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                        $scope.deviceArray = promise.deviceArray;
                                    }
                                    $state.reload();
                                }
                                else {
                                    if (promise.returnval === false) {
                                        if ($scope.icW_Id > 0) {
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
                        $scope.ldrnew = false;
                    });
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.clear1 = function () {
            $state.reload();
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.studentlist.every(function (options) {
                return options.selectedd2;
            });
        }

        $scope.onyearchange = function (ASMAY_Id) {
            $scope.sectionlist = [];
            $scope.asmcL_Id = "";
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("IVRM_ClassWork/get_classes", data).then(function (promise) {
                $scope.classlist = [];
                if (promise.classlist.length > 0) {
                    $scope.classlist = promise.classlist;
                    $scope.classworklst = promise.class_gridlist;
                }
                else {
                    swal('Data Not Available');
                    $scope.asmcL_Id = "";
                }
            });
        };

        $scope.onclasschange = function (asmcL_Id) {
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            $scope.subjectlist = [];
            $scope.ismS_Id = "";
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("IVRM_ClassWork/getsectiondata", data).
                then(function (promise) {
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
                        swal('Records are not available..!');
                        $scope.asmS_Id = "";
                    }
                });
        };
        // $scope.sectionchange = function () {
        $scope.getsub = function () {
            if ($scope.sub > 0) {
                $scope.hm_section_list = [];
                $scope.hm_section_list.push({ ASMS_Id: $scope.sub });
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
                //"ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                hm_section_list: $scope.hm_section_list
            };
            apiService.create("IVRM_ClassWork/getsubject", data).
                then(function (promise) {
                    if (promise.subjectlist.length > 0) {
                        $scope.subjectlist = promise.subjectlist;
                    }
                    else {
                        swal('Subjects are not available for This Section..!');
                        $scope.ismS_Id = "";
                    }
                });
        };

        //======Edit Record =====//
        $scope.editData = function (option) {
            $scope.ldrnew = true;
            $scope.checklink = false;
            $scope.link_url_all = "";
            var data = {
                "ICW_Id": option.icW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            $scope.classwork_id = "";
            $scope.year_id = "";
            $scope.classwork_id = option.icW_Id;
            $scope.year_id = option.asmaY_Id;

            apiService.create("IVRM_ClassWork/editData", data)
                .then(function (promise) {
                    $scope.editlist = promise.editlist;
                    if (promise.editlist.length > 0) {
                        $scope.fileflg = true;
                        $scope.section_list_temp = [];
                        $scope.section_list_temp = promise.editlist_section;
                        $scope.icW_Id = promise.editlist[0].icW_Id;
                        $scope.notice_EStartDate = new Date(promise.editlist[0].icW_FromDate);
                        $scope.notice_EndDate = new Date(promise.editlist[0].icW_ToDate);
                        $scope.file_detail = promise.editlist[0].icW_Attachment;
                        if (promise.editlist[0].icW_Attachment == 'HyperLink') {
                            $scope.checklink = true;
                        }
                        else {
                            $scope.checklink = false;
                        }
                        $scope.work_topic = promise.editlist[0].icW_Topic;
                        $scope.work_subtopic = promise.editlist[0].icW_SubTopic;
                        $scope.icW_Assignment = promise.editlist[0].icW_Assignment;
                        $scope.icW_Evaluation = promise.editlist[0].icW_Evaluation;
                        $scope.icW_TeachingAid = promise.editlist[0].icW_TeachingAid;
                        $scope.work_content = promise.editlist[0].icW_Content;
                        $scope.icW_FilePath = promise.editlist[0].icW_FilePath;
                        // $scope.notice = promise.editlist[0].icW_FilePath;
                        $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                        $scope.onyearchange($scope.asmaY_Id);
                        $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                        $scope.sub = "";
                        $scope.sub = promise.editlist[0].asmS_Id;
                        $scope.onclasschange($scope.asmcL_Id);
                        $scope.asmS_Id = promise.editlist[0].asmS_Id;
                        $scope.getsub($scope.asmS_Id);
                        $scope.ismS_Id = promise.editlist[0].ismS_Id;

                        if (promise.attachementlist != null || promise.attachementlist > 0) {
                            $scope.documentListOtherDetails = [];
                            $scope.urldocumentlist = [];
                            angular.forEach(promise.attachementlist, function (aa) {
                                $scope.img = aa.icwatT_Attachment;
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
                                        id: 1, ICW_FilePath: aa.icwatT_Attachment,
                                        FileName: aa.icwatT_FileName
                                    });

                                }
                                else {
                                    $scope.urldocumentlist.push({
                                        id: 1, ICW_FilePath: aa.icwatT_Attachment,
                                        FileName: aa.icwatT_FileName
                                    });

                                }
                            })

                        }
                        if ($scope.urldocumentlist.length > 0) {
                            $scope.checklink = true;
                        }

                    }
                    $scope.ldrnew = false;
                });
        };
        //=================== view doc==========
        $scope.viewData_doc = function () {
            $scope.attachementlist1 = [];
            if ($scope.classwork_id == "" && $scope.year_id == "" || $scope.classwork_id == null && $scope.year_id == null || $scope.classwork_id == undefined && $scope.year_id == undefined) {
                $scope.attachementlist1 = $scope.notice;
                $scope.docshow = true;
                $scope.docshowary = false;
            }
            else {
                var data = {
                    "ICW_Id": $scope.classwork_id,
                    "ASMAY_Id": $scope.year_id
                };
                apiService.create("IVRM_ClassWork/viewData", data)
                    .then(function (promise) {
                        $scope.attachementlist2 = [];
                        $scope.attachementlist1 = [];
                        if (promise.attachementlist.length > 0) {
                            $scope.attachementlist1 = promise.attachementlist;

                            $scope.docshow = true;
                            $scope.docshowary = false;
                            $('#myModalCoverview').modal('show');
                        }
                        else {
                            swal("No Data Found.");
                            $state.reload();
                        }
                    });
            }
        };
        //=====================
        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
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
        //===============
        $scope.getmodeldetails = function (option) {
            var data = {
                "ICW_Id": option.icW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            apiService.create("IVRM_ClassWork/editData", data)
                .then(function (promise) {
                    $scope.allimages = [];
                    $scope.editlist = promise.editlist;
                    if (promise.editlist.length > 0) {
                        if (promise.editlist[0].icW_FilePath != undefined || promise.editlist[0].icW_FilePath != '' || promise.editlist[0].icW_FilePath != null) {
                            var img = promise.editlist[0].icW_FilePath;
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
                            $scope.allimages.push({ icW_FilePath: img })
                        }
                        else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' || $scope.filetype2 == 'pdf' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                            angular.forEach($scope.editlist, function (qq) {
                                $scope.allimages.push({ icW_FilePath: img })
                                $scope.imgfile = true;
                                $scope.videofile = false;
                            })
                        }
                    }
                });
        };
        //===============activate/deactivate
        $scope.Deactivate = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (employee.icW_ActiveFlag === true) {
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

                        apiService.create("IVRM_ClassWork/deactivate", employee).
                            then(function (promise) {
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


        //======================================== Shiva
        $scope.previewVideo = function () {
            $scope.allimages = [];
            $scope.videofile = true;
            $scope.imgfile = false;
            $scope.allimages.push({ icW_FilePath: $scope.notice[0] });

        };
        //=============================== Preview Image

        $scope.previewimg = function (img) {
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


        //================================ Section Selection
        $scope.mediachange = function () {
            $scope.images_paths = [];
            $scope.stepsModel = [];
            $scope.filenames = "Video";
            $scope.fileflg = false;
        };
        //================================ Image  
        $scope.stepsModel = [];
        $scope.imageUpload = function (event) {
            $scope.hide = false;
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
                else if (input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "xlsx" || input.files[0].type === "xls"
                    || input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg"
                    || input.files[0].type === "application/pdf" || input.files[0].type === "application/pptx"
                    || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation"
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
                $scope.fileflg = true;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/ClassworkUpload", formData,
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
                            $scope.notice.push({ ICW_FilePath: imgp.path, FileName: imgp.name });
                        });
                        $scope.ldr = false;
                        $scope.hide = true;
                    }
                    if ($scope.notice.length > 0) {
                        $scope.classwork_id = undefined;
                        $scope.year_id = undefined;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
        //==================
        $scope.viewData = function (option) {
            var data = {
                "ICW_Id": option.icW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            apiService.create("IVRM_ClassWork/viewData", data)
                .then(function (promise) {

                    if (promise.attachementlist.length > 0) {
                        $scope.attachementlist1 = [];
                        angular.forEach(promise.attachementlist, function (qq) {
                            $scope.img = qq.icwatT_Attachment;
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
                                    icwatT_FileName: qq.icwatT_FileName,
                                    icW_Attachment: qq.icwatT_Attachment,
                                    icwatT_Attachment: qq.icwatT_Attachment
                                })
                            }
                            else {
                                $scope.attachementlist1.push({
                                    icwatT_FileName: qq.icwatT_FileName,
                                    icW_Attachment: 'HyperLink',
                                    icwatT_Attachment: qq.icwatT_Attachment
                                })
                            }
                        })

                        $scope.attachementlistnew = $scope.attachementlist1;
                        $scope.docshowary = true;
                        $scope.docshow = false;
                        $('#myModalCoverview').modal('show');
                    }
                    else {
                        swal("No Data Found.")
                    }
                });
        };

        $scope.viewDatalink = function (option) {
            var data = {
                "ICW_Id": option.icW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            apiService.create("IVRM_ClassWork/viewData", data)
                .then(function (promise) {
                    $scope.attachementlistnew = [];
                    if (promise.attachementlist.length > 0) {

                        $scope.icwatT_filename = promise.attachementlist[0].icwatT_Attachment;
                        $window.open($scope.icwatT_filename);
                    }
                    else {
                        swal("No Data Found.")
                    }
                });
        };
        //============Class work marks entry update==============

        $scope.getclassework_student = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("IVRM_ClassWork/getclasswork_student", data).then(function (promise) {
                if (promise.studentlist1.length > 0) {
                    $scope.studentlist = promise.studentlist1;
                }
                else {
                    swal("No data Found!!!")
                }
            })
        }
        //------------------------------
        $scope.getsubject = function () {
            $scope.getsubject_list = [];
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("IVRM_ClassWork/getsubjectlist", data).then(function (promise) {
                if (promise.getsubject_list != null && promise.getsubject_list.length > 0) {
                    $scope.getsubject_list = promise.getsubject_list;
                }
            });
        }
        //------------------------
        $scope.getclasswork_list12 = function () {
            $scope.submitted = false;
            $scope.getclasswork_list = [];
            $scope.TopicList = [];
            if ($scope.myForm1.$valid) {
                //$scope.studentarray = [];
                //angular.forEach($scope.studentlist, function (qq) {
                //    if (qq.selectedd2 == true) {
                //        $scope.studentarray.push({ AMST_Id1: qq.AMST_Id })
                //    }
                //})
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
                    //  "studentarray": $scope.studentarray
                }
                apiService.create("IVRM_ClassWork/getclasswork_list", data).then(function (promise) {
                    $scope.getclasswork_list = [];
                    if (promise.getclasswork_list.length > 0) {
                        $scope.getclasswork_list = promise.getclasswork_list;
                        angular.forEach($scope.getclasswork_list, function (dd) {
                            dd.checkedvalue = false;
                            if (dd.ICWUPL_Marks !== null) {
                                dd.checkedvalue = true;
                            }
                        });
                        $scope.chck = false;
                        $scope.optionToggled();
                        $scope.TopicList = promise.topicList;
                    }
                    else {
                        swal('No Data Found!!!');
                        $scope.TopicList = [];
                        $scope.ICW_IdTopic = "";
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.isOptionsRequiredstudent = function () {
            return !$scope.studentlist.some(function (sec) {
                return sec.selectedd2;
            });
        }
        //---------------------------------
        $scope.classwork_marks_update = function () {
            $scope.submittedclasswork = false;
            if ($scope.myFormclasswork2.$valid) {
                $scope.getclasswork_list_array = [];

                angular.forEach($scope.getclasswork_list, function (qq) {
                    if (qq.checkedvalue == true) {
                        $scope.getclasswork_list_array.push({
                            AMST_Id: qq.AMST_Id, ICW_Id: qq.ICW_Id, Marks: qq.ICWUPL_Marks, ICWUPL_FileName: qq.FileName1,
                            ICWUPL_StaffUplaod: qq.ICW_FilePath1, ICWUPL_StaffRemarks: qq.ICWUPL_StaffRemarks, ICWUPL_Id: qq.ICWUPL_Id,
                            doclist_temp: qq.uploadfilestemp
                        });
                    }
                });

                var data = {
                    "getclasswork_list_array": $scope.getclasswork_list_array
                };

                apiService.create("IVRM_ClassWork/classwork_marks_update", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Marks Save Successfully.')
                        $state.reload();
                    }
                    else if (promise.returnval == false) {
                        swal('Marks Save Not Successfully.')
                    }
                })
            } else {
                $scope.submittedclasswork = true;
            }

        };

        $scope.edit_classwork_mark = function (wrk) {
            var data = {
                "AMST_Id": wrk.AMST_Id,
                "ICW_Id": wrk.ICW_Id
            }
            apiService.create("IVRM_ClassWork/edit_classwork_mark", data).then(function (promise) {
                if (promise.edit_mark_list.length > 0) {
                    $scope.getclasswork_list = [];
                    $scope.getclasswork_lis1 = [];
                    $scope.getclasswork_list = promise.edit_mark_list

                    angular.forEach($scope.getclasswork_list, function (dd) {
                        if (dd.ICWUPL_Marks !== null) {
                            dd.checkedvalue = true;
                        }
                    });

                    angular.forEach($scope.getclasswork_lis1, function (qq) {
                        $scope.gethomework_list.push({ studentname: qq.studentname, ICW_Topic: qq.ICW_Topic, ICW_SubTopic: qq.ICW_SubTopic, ICW_FromDate: qq.ICW_FromDate, ICW_ToDate: qq.ICW_ToDate, ICWUPL_Marks: qq.ICWUPL_Marks, AMST_Id: qq.AMST_Id, ICW_Id: qq.ICW_Id, checkedvalue: true });
                    })
                    if ($scope.getclasswork_list.length > 0) {
                        $scope.chck = true;
                    }
                    $scope.optionToggled();

                    $scope.studentname = promise.editstudent[0].studentname
                    $scope.studentclass = promise.editstudent[0].ASMCL_ClassName
                    $scope.studentsection = promise.editstudent[0].ASMC_SectionName
                }
                else {
                    swal('No Data Found!!!')
                }
            })
        }

        $scope.viewclasswork = function (work) {
            var data = {
                "ICW_Id": work.ICW_Id
            }
            apiService.create("IVRM_ClassWork/viewclasswork", data).then(function (promise) {
                if (promise.viewhomework.length > 0) {
                    $scope.viewclassworknew = [];
                    $scope.attachementlist1 = [];
                    angular.forEach(promise.viewhomework, function (qq) {
                        $scope.img = qq.icwatT_Attachment;
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
                                icwatT_FileName: qq.icwatT_FileName,
                                icW_Attachment: qq.icwatT_Attachment,
                                icwatT_Attachment: qq.icwatT_Attachment
                            })
                        }
                        else {
                            $scope.attachementlist1.push({
                                icwatT_FileName: qq.icwatT_FileName,
                                icW_Attachment: 'HyperLink',
                                icwatT_Attachment: qq.icwatT_Attachment
                            })
                        }
                    })


                    $scope.viewclassworknew = $scope.attachementlist1;
                }
                else {
                    swal('No Data Found!!!')
                }
            })
        }

        $scope.viewstudentupload = function (ee) {
            $scope.ICWUPL_Id_Temp = ee.ICWUPL_Id;
            $scope.tempupload = [];
            $scope.viewstudentuploadnew = [];
            angular.forEach($scope.getclasswork_list, function (dd) {
                if (dd.ICWUPL_Id === $scope.ICWUPL_Id_Temp) {
                    if (dd.uploadfilestemp !== undefined && dd.uploadfilestemp !== null && dd.uploadfilestemp.length > 0) {
                        $scope.tempupload = dd.uploadfilestemp;
                    }
                }
            });

            if ($scope.tempupload.length === 0) {
                var data = {
                    "AMST_Id": ee.AMST_Id,
                    "ICW_Id": ee.ICW_Id,
                    "ICWUPL_Id": ee.ICWUPL_Id
                }
                apiService.create("IVRM_ClassWork/viewstudentupload", data).then(function (promise) {
                    if (promise.viewstudentupload.length > 0) {
                      
                        $scope.viewstudentuploadnew = promise.viewstudentupload;

                        angular.forEach($scope.viewstudentuploadnew, function (dd) {
                            dd.FilePath1 = "";
                            dd.Remarks = dd.icwuplatT_StaffRemarks;
                            if (dd.icwuplatT_StaffUpload !== null && dd.icwuplatT_StaffUpload !== "") {
                                dd.FilePath1 = dd.icwuplatT_StaffUpload;
                                var nameArray = dd.icwuplatT_StaffUpload.split('.');
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

        $scope.interactedclasswork2 = function () {
            return $scope.submittedclasswork;            
        };

        $scope.isOptionsRequiredClasswork1 = function () {
            return !$scope.getclasswork_list.some(function (options) {
                return options.checkedvalue;
            });
        };


        //------------------- Preview Image
        $scope.previeclasswork = function (img) {
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
        };
        //-----------------------
        $scope.optionToggled = function (user) {
            $scope.all = $scope.getclasswork_list.every(function (itm) { return itm.checkedvalue; })
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.getclasswork_list, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }
        $scope.clear_data = function () {
            $state.reload();
        };
        //=============================================================
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
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                document.extention = nameArray[nameArray.length - 1];
                if (extention === "doc" || extention === "xlsx" || extention === "jpg" || extention === "jpeg" ||
                    extention === "xls" || extention === "png"
                    || extention === "pdf"
                    || extention === "pptx" || extention === "ppsx" || extention === "ppt"
                    || extention === "mp3"
                    || extention === "mp4"
                    || extention === "docx" || extention === "wmv") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentOtherDetail(document);
                }

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
            $http.post("/api/ImageUpload/ClassworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.ICW_FilePath = d[0].path;
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
        //======================
        $scope.selectFileforUploadzdOtherDetail1 = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail1 = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
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
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/ClassworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        $scope.ICW_FilePath1 = d[0].path;
                        $scope.FileName1 = d[0].name;

                        data.FileName1 = $scope.FileName1;
                        data.ICW_FilePath1 = $scope.ICW_FilePath1;

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
        $scope.interacted13 = function (field) {
            return $scope.submitted13;
        };
        //========================end===========================
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
        //===================
        $scope.selectFileforUploadzdOtherDetail2 = function (input, document) {
            $scope.ICWUPL_Id = document.icwupL_Id;
            //$('#' + document.id).removeAttr('src');
            $scope.selectFileforUploadzdOtherDetail222 = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail2(document, $scope.ICWUPL_Id);
            }
        };

        function UploadEmployeeDocumentOtherDetail2(data, ICWUPL_Id) {
            $scope.gethomework_list5 = [];

            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail2.length; i++) {
                formData.append("File", $scope.selectFileforUploadzdOtherDetail222[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/ClassworkUpload", formData,
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
                        $scope.ICWUPL_Id = ICWUPL_Id;

                        var imagarr = $scope.FilePath1.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.filetype2 = lastelement;
                        data.filetype = lastelement;
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

        //========================

        $scope.AddAndClose = function () {
            angular.forEach($scope.getclasswork_list, function (dd) {
                if (dd.ICWUPL_Id === $scope.ICWUPL_Id_Temp) {
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
                    $scope.gethomework_list4.push({ FilePath1: aa.FilePath1, FileName1: aa.FileName1, ICWUPL_Id: aa.icwupL_Id, Remarks: aa.Remarks });
                }
            });

            if ($scope.gethomework_list4.length > 0) {
                var data = {
                    "doclist": $scope.viewstudentuploadnew
                }
                apiService.create("IVRM_ClassWork/stfupload", data).then(function (promise) {
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
        }

        $scope.cls = function () {
            $scope.updshow = false;
        }

        //==============================

    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

})();
