(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeDashboardController', EmployeeDashboardController);

    EmployeeDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http']

    function EmployeeDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http) {

        $scope.tempcldrlst = [];


        //$('.carousel .item').each(function () {
        //    var next = $(this).next();
        //    if (!next.length) {
        //        next = $(this).siblings(':first');
        //    }
        //    next = next.next();
        //    if (!next.length) {
        //        next = $(this).siblings(':first');

        //    }
        //});
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.LoadData = function () {

            var sal_list = [];
            var TT_list = [];
            apiService.getDATA("EmployeeDashboard/getalldetails").then(function (promise) {

                $scope.yearlist = promise.yearlist;
                $scope.NoticeBoardYearId = promise.asmaY_Id;
                $scope.EventsYearId = promise.asmaY_Id;

                //$scope.Month_list = promise.monthName;
                if (promise.filldepartment !== null && promise.filldepartment.length > 0) {
                    $scope.empDetails = promise.filldepartment;
                    $scope.HRME_EmployeeFirstName = $scope.empDetails[0].hrmE_EmployeeFirstName;
                    $scope.HRME_DOJ = $scope.empDetails[0].hrmE_DOJ;
                    $scope.HRMD_DepartmentName = $scope.empDetails[0].hrmD_DepartmentName;
                    $scope.HRMDES_DesignationName = $scope.empDetails[0].hrmdeS_DesignationName;
                    $scope.HRME_EmployeeCode = $scope.empDetails[0].hrmE_EmployeeCode;
                    $scope.HRME_DOB = $scope.empDetails[0].hrmE_DOB;
                    $scope.photo = $scope.empDetails[0].hrmE_PhotoNo;
                    $('#blah').attr('src', $scope.photo);
                    $scope.coedata = promise.coedata;
                    $scope.studentcount = promise.studentcount;
                    $scope.assetcount = promise.assetcount;
                    $scope.purchasecount = promise.purchasecount;
                    $scope.bookcount = promise.bookcount;
                    $scope.hrmE_TechNonTeachingFlg = promise.hrmE_TechNonTeachingFlg;
                    $scope.medicalCount = promise.medicalCount;                 
                    if ($scope.hrmE_TechNonTeachingFlg === "Teaching") {
                        $scope.hrmE_TechNonTeachingFlg = "Teaching";
                    }
                    else if ($scope.hrmE_TechNonTeachingFlg === "NonTeaching") {
                        $scope.hrmE_TechNonTeachingFlg = "NonTeaching";
                    }
                    else {
                        $scope.hrmE_TechNonTeachingFlg = "Teaching";
                    }
                }


                $scope.classsectionn = promise.classsection;
                $scope.classhandlg = promise.classhandling;
                $scope.displyam = promise.displyamessage;



                //if (promise.yearlist !== null && promise.yearlist.length > 0) {
                //    $('#wellcome').modal('show');
                //}
                //else if (promise.yearlist !== null && promise.yearlist.length > 0) {
                //    $('#wellcome').modal('show');
                //}


                if (promise.mobile.length > 0) {
                    $scope.HRME_MobileNo = promise.mobile[0].hrmE_MobileNo;
                }
                if (promise.email.length > 0) {
                    $scope.HRME_EmailId = promise.email[0].hrmE_EmailId;
                }

                if ($scope.coedata != "" && $scope.coedata != null) {
                    if ($scope.coedata.length > 0) {
                        $scope.hidecoe = true;
                    }
                }
                else {
                    $scope.hidecoe = false;
                }
                //for salary count
                //var totalsalary = 0;
                //if (promise.salarylist.length > 0) {
                //    $scope.salary_list = promise.salarylist;
                //    for (var i = 0; i < $scope.salary_list.length; i++) {
                //        totalsalary += $scope.salary_list[i].salary;
                //    }
                //}

                //$scope.SalaryC = totalsalary;

                //for timetable count
                var totalPeriod = 0;

                if (promise.tT_final_generation.length > 0) {
                    $scope.tt_final = promise.tT_final_generation;
                    for (var i = 0; i < $scope.tt_final.length; i++) {
                        totalPeriod += $scope.tt_final[i].periodCount;
                    }
                }

                $scope.PeriodC = totalPeriod;
                $scope.AllPeriod = promise.allperiods;
                //----------------------------------Calender
                $scope.coedata = promise.calenderlist;
                angular.forEach($scope.coedata, function (qwe) {
                    qwe.title = qwe.coemE_EventName;
                    var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                    var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                    qwe.start = new Date(xyz);
                    $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                });

                if ($scope.salary_list != null) {

                    for (var i = 0; i < $scope.salary_list.length; i++) {
                        sal_list.push({ label: $scope.salary_list[i].monthName, "y": $scope.salary_list[i].salary })
                    }
                }
                if ($scope.tt_final != null && $scope.AllPeriod != null) {
                    for (var i = 0; i < $scope.AllPeriod.length; i++) {
                        for (var j = 0; j < $scope.tt_final.length; j++) {
                            if ($scope.tt_final[j].dayName == $scope.AllPeriod[i].ttmD_DayCode) {
                                TT_list.push({ label: $scope.AllPeriod[i].ttmD_DayName, "y": $scope.tt_final[j].periodCount })
                            }

                        }
                    }
                }

                if (promise.paymentNootificationStaff === 0) {
                    if (promise.getpaymentnotificationdetails !== null && promise.getpaymentnotificationdetails.length > 0) {
                        var ISMCLTPRP_InstallmentAmt = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentAmt;
                        var ISMCLTPRP_InstallmentName = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentName;
                        var ISMCLTPRP_PaymentDate = new Date(promise.getpaymentnotificationdetails[0].ISMCLTPRP_PaymentDate);
                        var dated = $filter('date')(new Date(ISMCLTPRP_PaymentDate), 'dd/MM/yyyy');

                        var stringdisplay = "Dear Sir/Madam,\n Digital Campus Project Bill Payment is overdue as on " + dated + " Please pay for uninterrupted service.\n If already paid kindly ignore.";
                        $scope.DeletRecord(stringdisplay);
                    }
                }

                //var chart = new CanvasJS.Chart("columnchart", {
                //    axisX: {
                //        labelFontSize: 12
                //    },
                //    axisY: {
                //        labelFontSize: 12
                //    },
                //    data: [{
                //        type: "column",
                //        showInLegend: true,
                //        dataPoints: sal_list
                //    }]
                //});
                //chart.render();

                ////rangeBarChat
                //var chart1 = new CanvasJS.Chart("areachart", {
                //    axisX: {
                //        labelFontSize: 12
                //    },
                //    axisY: {
                //        labelFontSize: 12
                //    },
                //    data: [{
                //        type: "column",
                //        showInLegend: true,
                //        dataPoints: TT_list
                //    }]
                //});
                //chart1.render();

                $scope.period_list = promise.periods;
                $scope.Class_Section_List = promise.class_sectons;
            });
        };
        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd) {
            var studentreg = "PdfFile";
            $scope.imagedownload = data;
            imagedownload = data;

            //if ($scope.studentname !== undefined && $scope.studentname !== null && $scope.studentname !== '') {
            //    studentreg = $scope.studentname + '_' + idd;
            //}

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
        $scope.onclick_notice = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange" :"OnClick"
            };
            apiService.create("EmployeeDashboard/onclick_notice", data).then(function (promise) {
                if (promise.noticeboard !== null && promise.noticeboard.length > 0) {
                    $scope.noticeboard = promise.noticeboard;                    
                }
                else {
                    swal('No Data Found..!!');
                }
                $('#myModalNotice').modal('show');
            });
        };

        $scope.OnChangeNoticeBoardYear = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": "OnChange",
                "ASMAY_Id": $scope.NoticeBoardYearId
            };
            apiService.create("EmployeeDashboard/onclick_notice", data).then(function (promise) {

                if (promise.noticeboard !== null && promise.noticeboard.length > 0) {
                    $scope.noticeboard = promise.noticeboard;                    
                }
                else {
                    swal('No Data Found..!!');
                }
            });
        };

        $scope.onclick_events = function (flag) {
            $scope.coedata = [];
            var data = {
                "OnClickOrOnChange": "OnClick",
                "flag": flag
            };
            apiService.create("EmployeeDashboard/onclick_events", data).then(function (promise) {
                if (promise.coedata !== null && promise.coedata.length > 0) {
                    $scope.coedata = promise.coedata;
                    angular.forEach($scope.coedata, function (qwe) {
                        qwe.title = qwe.eventName;
                        var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                        var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                        qwe.start = new Date(xyz);
                        $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                    });                   
                }
                else {                   
                    swal('No Data Found..!!');
                }
                $('#myModalCOE').modal('show');
            });
        };


        $scope.onclick_asset = function (flag) {
            $scope.coedata = [];
            var data = {
                "flag": "Asset"
            };
            apiService.create("EmployeeDashboard/onclick_asset", data).then(function (promise) {
                if (promise.assetlist !== null && promise.assetlist.length > 0) {
                    $scope.assetlist = promise.assetlist;
              
                }
                else {
                    swal('No Data Found..!!');
                }
                $('#myModalAssetList').modal('show');
            }
            );
        };

        $scope.onclick_purchase = function (flag) {
            $scope.coedata = [];
            var data = {
                "flag": "Purchase"
            };
            apiService.create("EmployeeDashboard/onclick_asset", data).then(function (promise) {
                if (promise.assetlist !== null && promise.assetlist.length > 0) {
                    $scope.purchaselist = promise.assetlist;

                }
                else {
                    swal('No Data Found..!!');
                }
                $('#myModalpurchaselist').modal('show');
            }
            );
        };

        $scope.onclick_booklist = function (flag) {
            $scope.booklist = [];
            $('#myModalbooklist').modal('hide');
            var data = {
                "flag": "BOOK"
            };
            apiService.create("EmployeeDashboard/onclick_asset", data).then(function (promise) {
                if (promise.assetlist !== null && promise.assetlist.length > 0) {
                    $scope.booklist = promise.assetlist;
                    $('#myModalbooklist').modal('show');

                }
                else {
                    swal('No Data Found..!!');
                
                }
               
            }
            );
        };

        //Praveen Gouda Added       
        $scope.onclick_medicalpdflist = function () {
            $scope.attachementlist = []; 
            var data = {               
                "HRME_Id": $scope.HRME_Id
            }
            apiService.create("Employee_MedicalRecord/onclick_employee", data) .then(function (promise) {
                    if (promise.attachementlist != null && promise.attachementlist.length > 0) {
                        angular.forEach(promise.attachementlist, function (ww) {
                            $scope.attachementlist.push({
                                INTBFL_FilePath: ww.hremrF_FilePath,
                                FilePath: ww.hremrF_FilePath,
                                cc: ww.hremrF_FileName,
                                HREMRF_Attachment: ww.hremrF_FileName,
                            })
                        })

                        $('#myModalCoverviews').modal('show');

                    }
                    else {
                        swal("No Data Found.")

                    }

                });
        };
        /////Ended-----------------

        $scope.onclick_booklistwo = function (flag) {
            $scope.bookFillist = [];
           
            var data = {
                "flag": "BOOKLIST",
                "INTB_Id": $scope.LMB_Id.lmB_Id
            };
            apiService.create("EmployeeDashboard/onclick_asset", data).then(function (promise) {
                if (promise.assetlist !== null && promise.assetlist.length > 0) {
                    $scope.bookFillist = promise.assetlist;
                  
                    //angular.forEach(promise.bookFillist, function (qq) {
                    //    $scope.img = qq.LMBFILE_FileName;
                    //    if ($scope.img != null) {
                    //        var imagarr = $scope.img.split('.');
                    //        var lastelement = imagarr[imagarr.length - 1];

                    //        $scope.filetype2 = lastelement;
                    //    }

                    //    if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                    //        || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                    //        $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                    //        || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                    //        || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                    //        $scope.bookFillist.push({
                    //            LMBFILE_FileName: qq.lmbfilE_FileName,
                    //            LMBFILE_FilePath: qq.lmbfilE_FilePath
                    //        })
                    //    }
                    //    else {
                    //        $scope.bookFillist.push({

                    //            LMBFILE_FileName: qq.lmbfilE_FileName,
                    //            LMBFILE_FilePath: qq.lmbfilE_FilePath,

                    //        });
                    //    }
                    //});

                    //$scope.attachementlist = $scope.attachementlist1

                    //$('#myModalCoverview').modal('show');
                    //$scope.docshowary = true;
                    //$scope.docshow = false;
                }
                else if (promise.displyamessage.length > 0) {
                    $scope.displyamessages = $scope.displyamessage
                }
         
                else {
                    swal('No Data Found..!!');

                }

            }
            );
        };



        $scope.OnChangeEventsYear = function (flag) {
            $scope.coedata = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": "OnChange",
                "ASMAY_Id": $scope.EventsYearId
            };
            apiService.create("EmployeeDashboard/onclick_events", data).then(function (promise) {

                if (promise.coedata !== null && promise.coedata.length > 0) {

                    $scope.coedata = promise.coedata;
                    angular.forEach($scope.coedata, function (qwe) {
                        qwe.title = qwe.eventName;
                        var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                        var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                        qwe.start = new Date(xyz);
                        $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                    });                    
                }
                else {
                    swal('No Data Found..!!');
                }
            });
        };



        $scope.viewData1 = function () {
            var data = {
                "msgdis": $scope.msgdis = 1
            };
            apiService.create("EmployeeDashboard/viewnotice", data).then(function (promise) {
                if (promise.displyamessage.length > 0) {
                    $scope.displyamessages = promise.displyamessage;
                    $('#myModalDiaplymessg').modal('show');
                }
                else {
                    swal("No Data Found.");
                }
            });
        };



        //====================
        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "INTB_Id": option.INTB_Id

            };
            apiService.create("EmployeeDashboard/viewnotice", data).then(function (promise) {

                if (promise.attachementlist.length > 0) {
                    $scope.attachementlist1 = [];
                    angular.forEach(promise.attachementlist, function (qq) {
                        $scope.img = qq.intbfL_FilePath;
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
                                INTBFL_FileName: qq.intbfL_FileName,
                                INTBFL_FilePath: qq.intbfL_FilePath,
                                INTB_Attachment: qq.intB_Attachment,
                                INTB_Id: qq.intB_Id
                            })
                        }
                        else {
                            $scope.attachementlist1.push({

                                INTBFL_FileName: qq.intbfL_FileName,
                                INTBFL_FilePath: qq.intbfL_FilePath,

                                INTB_Attachment: 'HyperLink',
                                INTB_Id: qq.intB_Id
                            });
                        }
                    });

                    $scope.attachementlist = $scope.attachementlist1

                    $('#myModalCoverview').modal('show');
                    $scope.docshowary = true;
                    $scope.docshow = false;
                }
                else if (promise.displyamessage.length > 0) {
                    $scope.displyamessages = $scope.displyamessage
                }
                else {
                    swal("No Data Found.");
                }
            });
        };

        $scope.UpdateEmployeeProfilePic = function () {

            if ($scope.hrmE_Photo !== undefined && $scope.hrmE_Photo !== null && $scope.hrmE_Photo !== "") {
                swal({
                    title: "Are You Sure , Do You Want To Change The Profile Photo",
                    text: "Do you want to continue !!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                    cancelButtonText: "Cancel!!",
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "HRME_Photo": $scope.hrmE_Photo,
                            "HRME_EmployeeCode": $scope.HRME_EmployeeCode
                        };

                        apiService.create("EmployeeDashboard/UpdateEmployeeProfilePic", data).then(function (promise) {


                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
            } else {
                swal("Upload Profile Photo");
            }
        };


        $scope.UploadEmployeeProfilePic = [];
        $scope.uploadEmployeeProfilePic = function (input, document) {
            $scope.UploadEmployeeProfilePic = input.files;
            $('#blah').removeAttr('src');

            if (input.files && input.files[0]) {

                if ((input.files[0].type == "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah').attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofile();
                }
                else if (input.files[0].type != "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the jpef, png, jpg file");
                    angular.element("input[type='file']").val(null);
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    angular.element("input[type='file']").val(null);
                    return;
                }
            }
        }

        function UploadEmployeeprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadEmployeeProfilePic.length; i++) {
                formData.append("File", $scope.UploadEmployeeProfilePic[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/EmployeeDocumentUpload/UploadEmployeeprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    if (d != "PathNotFound") {
                        $scope.hrmE_Photo = d;
                    } else {
                        swal('File Storage Path Not Found ..!!');
                        angular.element("input[type='file']").val(null);
                    }

                })
                .error(function () {
                    $('#blah').removeAttr('src');
                    defer.reject("File Upload Failed!");
                    angular.element("input[type='file']").val(null);
                });
        }

        //=====================view doc
        $scope.previewimg_new = function (img) {
            $('#myvideoPreview').modal('hide');
            $('#myimagePreview').modal('hide');
            $('#showpdf').modal('hide');
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

        //=======================

        $scope.DeletRecord = function (stringdisplay, SweetAlert) {
            swal({
                //html: true,
                title: "Payment Subscription !",
                //text: stringdisplay,
                text: stringdisplay,
                type: "input",
                showCancelButton: false,
                allowEscapeKey: false,
                closeOnClickOutside: false,
                closeOnConfirm: false,
                inputPlaceholder: "Enter Remarks",
                confirmButtonText: "OK"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false;
                }
                if (inputValue !== "") {
                    var data = {
                        "subscriptionremarks": inputValue,
                        "paymentsubscriptiontype": "PaymentNootificationStaff"

                    };
                    apiService.create("InstitutionUserMapping/savepaymentremarks", data).then(function (promise) {
                        if (promise !== null) {
                            swal("Remarks Captured");
                        }
                    });
                }
            });
        };



        //-------------------------------------------------------PORTAL-CALENDER

        // #region PortalCalender

        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        $scope.changeTo = 'Hungarian';
        $scope.currentView = 'month';

        /* event source that contains custom events on the scope */
        $scope.events = $scope.tempcldrlst;
        /* event source that calls a function on every view switch */
        $scope.eventsF = function (start, end, timezone, callback) {

            var s = new Date(start).getTime() / 1000;
            //  var e = new Date(end).getTime() / 1000;
            var m = new Date(start).getMonth();
            var events = [{
                title: 'Feed Me ' + m,
                start: s + (50000),
                // end: s + (100000),
                allDay: false,
                className: ['customFeed']
            }];
            callback(events);
        };
        $scope.calEventsExt = {
            color: '#f00',
            textColor: 'yellow',
            events: []
        };
        $scope.ev = {};
        /* alert on dayClick */
        $scope.alertOnDayClick = function (date) {
            $scope.alertMessage = (date.toString() + ' was clicked ');
            $scope.ev = {
                from: date.format('YYYY-MM-DD'),
                to: date.format('YYYY-MM-DD'),
                title: '',
                allDay: true
            };
        };
        /* alert on eventClick */
        $scope.alertOnEventClick = function (date, jsEvent, view) {
            //$scope.alertMessage = (date.title + ' was clicked ');
            swal({
                title: date.title,
                text: "Day Event!",
                imageUrl: 'https://jshsstorage.blob.core.windows.net/files/events-icon-4.jpg'
            });
        };
        /* alert on Drop */
        $scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
            $scope.alertMessage = ('Event Dropped to make dayDelta ' + delta);
        };
        /* alert on Resize */
        $scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
            $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
        };
        /* add and removes an event source of choice */
        $scope.addRemoveEventSource = function (sources, source) {
            var canAdd = 0;
            angular.forEach(sources, function (value, key) {
                if (sources[key] === source) {
                    sources.splice(key, 1);
                    canAdd = 1;
                }
            });
            if (canAdd === 0) {
                sources.push(source);
            }
        };
        /* add custom event*/
        $scope.addEvent = function () {
            $scope.events.push({
                title: $scope.ev.title,
                start: moment($scope.ev.from),
                //   end: moment($scope.ev.to),
                allDay: true,
                className: ['openSesame']
            });
        };
        /* remove event */
        /*$scope.remove = function (index) {
            $scope.events.splice(index, 1);
        };*/
        /* Change View */
        $scope.changeView = function (view, calendar) {
            uiCalendarConfig.calendars.myCalendar1.fullCalendar('removeEvents');
            uiCalendarConfig.calendars.myCalendar1.fullCalendar('addEventSource',
                $scope.tempcldrlst);
        };
        /* Change View */
        $scope.renderCalender = function (calendar) {
            $timeout(function () {
                if (uiCalendarConfig.calendars[calendar]) {
                    uiCalendarConfig.calendars[calendar].fullCalendar('render');
                }
            });
        };
        /* Render Tooltip */
        $scope.eventRender = function (event, element, view) { };
        /* config object */
        $scope.uiConfig = {
            calendar: {
                height: 325,

                editable: false,
                viewRender: $scope.changeView,
                //customButtons: {
                //    myCustomButton: {
                //        text: 'custom!',
                //        click: function () {
                //            alert('clicked the custom button!');
                //        }
                //    }
                //},
                header: {
                    left: 'title',
                    // center: 'myCustomButton',
                    right: 'today prev,next'
                },
                dayClick: $scope.alertOnDayClick,
                eventClick: $scope.alertOnEventClick,
                eventDrop: $scope.alertOnDrop,
                eventResize: $scope.alertOnResize,
                eventRender: $scope.eventRender,
                businessHours: {
                    start: '12:00', // a start time (10am in this example)
                    //     end: '18:00', // an end time (6pm in this example)

                    dow: [1, 2, 3, 4]
                    // days of week. an array of zero-based day of week integers (0=Sunday)
                    // (Monday-Thursday in this example)
                }
            }
        };
        /* event sources array*/
        $scope.eventSources = [$scope.events, $scope.eventsF];
        $scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];


        $scope.eventRender = function (event, element, view) {
            element.attr({
                'tooltip': event.events,
                'tooltip-append-to-body': true
            });
            $compile(element)($scope);
        };
        // #endregion

        $scope.saveakpkfile = function () {
            var data = {
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("EmployeeDashboard/saveakpkfile", data).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal('File Download!');
                    }
                });
        }


        var HostName = location.host;
        $scope.clicknotice = function () {

            $window.location.href = 'http://' + HostName + '/#/app/NoticeBoardUploadReport/';
            $('.modal-backdrop').remove();

        };

        $scope.showStudent = function () {

            $window.location.href = 'http://' + HostName + '/#/app/employeeStudentSearch';

        };

        $scope.showTT = function () {

            $window.location.href = 'http://' + HostName + '/#/app/employeeTimetable';

        };
        $scope.showSalary = function () {

            $window.location.href = 'https://' + HostName + '/#/app/employeeSalaryDetails';

        };
    };
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

})();