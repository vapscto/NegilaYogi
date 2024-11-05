(function () {
    'use strict';
    angular
        .module('app')
        .controller('PrincipalDashboardController', PrincipalDashboardController)
    PrincipalDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce', 'uiCalendarConfig', 'appSettings']
    function PrincipalDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce, uiCalendarConfig, appSettings) {
        $scope.sms = 0;
        $scope.email = 0;
        $scope.staffbrthlist = [];
        $scope.studentbrthlist = [];
        $scope.stdabsentlist = [];
        $scope.loadbasicdata = function () {
            $scope.Todaydate = new Date();

            apiService.getDATA("PrincipalDashboard/Getdetails").then(function (promise) {

                $scope.yearlt = promise.yearlist;
                $scope.yearlist = promise.yearlist1;
                $scope.currentYear = promise.currentAcademicYear;
                $scope.noticelist = promise.noticelist;
                $scope.NoticeBoardYearId = promise.asmaY_Id;
                $scope.notification = promise.notification;
                $scope.leavenotification = promise.leavenotification;
                $scope.staffbrthlist = promise.staffbrthlist;
                $scope.studentbrthlist = promise.studentbrthlist;
                $scope.stdabsentlist = promise.stdabsentlist;
                $scope.coedata = promise.coedata;


                if (promise.classteacherlst != null) {
                    $scope.classteacherlst = promise.classteacherlst;
                }
                if (promise.subjecttealst != null) {
                    $scope.subjecttealst = promise.subjecttealst;
                }
                if (promise.lateinlst != null) {
                    $scope.lateinlst = promise.lateinlst;
                }
                if (promise.earlyoutlst != null) {
                    $scope.earlyoutlst = promise.earlyoutlst;
                }
                if (promise.absentlst != null) {
                    $scope.absentlst = promise.absentlst;
                }
                for (var i = 0; i < $scope.yearlt.length; i++) {
                    if ($scope.currentYear[0].asmaY_Id == $scope.yearlt[i].asmaY_Id) {
                        $scope.asmaY_Id = promise.yearlist[i].asmaY_Id;
                    }
                }


                

                if (promise.smscount != null) {
                    $scope.sms = promise.smscount.length;
                }
                if (promise.emailcount != null) {
                    $scope.email = promise.emailcount.length;
                }
                if (promise.paymentNootificationPrinicipal === 0) {
                    if (promise.getpaymentnotificationdetails !== null && promise.getpaymentnotificationdetails.length > 0) {
                        //var ISMCLTPRP_InstallmentAmt = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentAmt;
                        //var ISMCLTPRP_InstallmentName = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentName;
                        //var ISMCLTPRP_PaymentDate = new Date(promise.getpaymentnotificationdetails[0].ISMCLTPRP_PaymentDate);
                        //var dated = $filter('date')(new Date(ISMCLTPRP_PaymentDate), 'dd/MM/yyyy');

                        //var stringdisplay = "Payment Due For " + ISMCLTPRP_InstallmentName + " Is " + ISMCLTPRP_InstallmentAmt + "/- And Due Date Is " + dated;

                        var ISMCLTPRP_InstallmentAmt = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentAmt;
                        var ISMCLTPRP_InstallmentName = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentName;
                        var ISMCLTPRP_PaymentDate = new Date(promise.getpaymentnotificationdetails[0].ISMCLTPRP_PaymentDate);
                        var dated = $filter('date')(new Date(ISMCLTPRP_PaymentDate), 'dd/MM/yyyy');

                        var stringdisplay = "Dear Sir/Madam,\n Digital Campus Project Bill Payment is overdue as on " + dated + " Please pay for uninterrupted service.\n If already paid kindly ignore.";

                        $scope.DeletRecord(stringdisplay);
                    }
                }

                $scope.onclick_noticenew('O');
            });
        };
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage5 = 10;
        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;
        $scope.currentPage5 = 1;
        //=============Notice===========//
        $scope.searchValue = "";
        $scope.onclick_notice = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick'
            };
            apiService.create("PrincipalDashboard/onclick_notice", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeboard = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeboard.push({
                            INTB_Id: nt.INTB_Id,
                            ASMCL_ClassName: nt.ASMCL_ClassName,
                            ASMC_SectionName: nt.ASMC_SectionName,
                            INTB_title: nt.INTB_title,
                            INTB_Attachment: nt.INTB_Attachment,
                            INTB_StartDate: nt.INTB_StartDate,
                            INTB_EndDate: nt.INTB_EndDate,
                            INTB_FilePath: nt.INTB_FilePath,
                            INTB_Description: nt.INTB_Description,
                            Filecount: nt.Filecount
                        });
                    });
                }
                else {
                    swal('No Data Found..!!');
                }
                $('#myModalNotice').modal('show');
            });
        };

        $scope.onclick_noticenew = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "OnClickOrOnChange": 'OnClick'
            };
            apiService.create("PrincipalDashboard/onclick_notice", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    $scope.noticeboard = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeboard.push({
                            INTB_Id: nt.INTB_Id,
                            ASMCL_ClassName: nt.ASMCL_ClassName,
                            ASMC_SectionName: nt.ASMC_SectionName,
                            INTB_title: nt.INTB_title,
                            INTB_Attachment: nt.INTB_Attachment,
                            INTB_StartDate: nt.INTB_StartDate,
                            INTB_EndDate: nt.INTB_EndDate,
                            INTB_FilePath: nt.INTB_FilePath,
                            INTB_Description: nt.INTB_Description,
                            Filecount: nt.Filecount
                        });
                    });
                }
              
            });
        };

        $scope.OnChangeNoticeboardYear = function (flag) {
            $scope.noticeboard = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.NoticeBoardYearId,
                "OnClickOrOnChange": 'OnChange'
            };

            apiService.create("PrincipalDashboard/onclick_notice", data).then(function (promise) {
                if (promise.noticelist !== null && promise.noticelist.length > 0) {
                    $scope.noticelist = promise.noticelist;
                    //$scope.noticeboard = [];
                    angular.forEach($scope.noticelist, function (nt) {
                        $scope.noticeboard.push({
                            INTB_Id: nt.INTB_Id,
                            ASMCL_ClassName: nt.ASMCL_ClassName,
                            ASMC_SectionName: nt.ASMC_SectionName,
                            INTB_title: nt.INTB_title,
                            INTB_Attachment: nt.INTB_Attachment,
                            INTB_StartDate: nt.INTB_StartDate,
                            INTB_EndDate: nt.INTB_EndDate,
                            INTB_FilePath: nt.INTB_FilePath,
                            INTB_Description: nt.INTB_Description,
                            Filecount: nt.Filecount
                        });
                    });
                }
                else {
                    swal('No Data Found..!!');
                }
            });
        };

        //=====View Data===//

        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "INTB_Id": option.INTB_Id

            };
            apiService.create("PrincipalDashboard/viewnotice", data).then(function (promise) {
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

                            })
                        }
                    })

                    $scope.attachementlist = $scope.attachementlist1;

                    $('#myModalCoverview').modal('show');
                    $scope.docshowary = true;
                    $scope.docshow = false;
                }
                else {
                    swal("No Data Found.")

                }

            });
        };

        //===================== multiple add + - End ==================
        //============================== File Upload Start=========
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD').attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };

        $scope.selectFileforUploadzdOtherDetailhome = function (input, document) {
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
        $scope.interacted13 = function (field) {
            return $scope.submitted13;
        };
        //============================== File Upload End=========


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
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                        document.getElementById("pdfIdzz").innerHTML = htmlElements;

                        //pdfId = document.getElementById("pdfIdzz");
                        //pdfId.removeChild(pdfId.childNodes[0]);
                        //embed = document.createElement('embed');
                        //embed.setAttribute('src', fileURL);
                        //embed.setAttribute('type', 'application/pdf');
                        //embed.setAttribute('width', '100%');
                        //embed.setAttribute('height', '1000');
                        //pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }

        };
        
        $scope.getmodeldetails = function (option) {
            $scope.allimages = [];
            if (option.ihW_FilePath == undefined || option.ihW_FilePath == null || option.ihW_FilePath == '') {
                var img = option.icW_FilePath;
            }
            else {
                var img = option.ihW_FilePath;
            }

            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];

                $scope.filetype2 = lastelement;
            }

            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.videofile = true;
                $scope.imgfile = false;
                $scope.allimages.push({ ihW_FilePath: img })
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'pdf') {

                $scope.allimages.push({ ihW_FilePath: img })
                $scope.imgfile = true;
                $scope.videofile = false;
            }
        };


        $scope.DeletRecord = function (stringdisplay, SweetAlert) {
            swal({
                title: "Payment Subscription !",
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
                        "paymentsubscriptiontype": "PaymentNootificationPrinicipal"
                    };
                    apiService.create("InstitutionUserMapping/savepaymentremarks", data).then(function (promise) {
                        if (promise !== null) {
                            swal("Remarks Captured");
                        }
                    });
                }
            });
        };

        $scope.Principal = function () {
            var HostName = location.host;
            $window.location.href = 'http://' + HostName + '/#/app/SendSMS';
            $scope.sms = true;
        };

        $scope.Timetable = function () {
            // alert("p");
            var HostName = location.host;
            $window.location.href = 'http://' + HostName + '/#/app/TimeTable';
            $scope.sms = true;
        };

        //$scope.StudentSearch = function () {
        //    // alert("p");
        //    var HostName = location.host;
        //    $window.location.href = 'http://' + HostName + '/#/app/TimeTable';
        //    $scope.sms = true;
        //};

        $scope.OnAcdyear = function () {
            apiService.getURI("PrincipalDashboard/GetDataByYear/", $scope.asmaY_Id).
                then(function (promise) {
                    $scope.studentstrenth = promise.fillstudentstrenth;
                    $scope.coedata = promise.coedata;    //calenderlist
                    // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
                    $scope.absentgraph = promise.fillabsent;
                    $scope.asmaY_Id = promise.asmaY_Id;
                    $scope.feedetailsgraph = promise.fillfee;
                    if ($scope.coedata != "" && $scope.coedata != null) {
                        if ($scope.coedata.length > 0) {
                            $scope.hidecoe = true;
                        }
                    }
                    else {
                        $scope.hidecoe = false;
                    }
                    $scope.datagraph = [];
                    if ($scope.studentstrenth != null) {

                        for (var i = 0; i < $scope.studentstrenth.length; i++) {
                            $scope.datagraph.push({ label: $scope.studentstrenth[i].class_Name, "y": $scope.studentstrenth[i].stud_count })
                        }
                    }
                    console.log($scope.datagraph);
                  //count of total students added by adarsh
                    var totalnostudents;
                    for (var x = 0; x <= $scope.datagraph.length-1; x++) {
                        totalnostudents = totalnostudents + $scope.datagraph[x].y;
                    }
                    
                    $scope.dataabsentgraph = [];
                    if ($scope.absentgraph != null) {

                        for (var i = 0; i < $scope.absentgraph.length; i++) {
                            $scope.dataabsentgraph.push({ label: $scope.absentgraph[i].nameOfDesig, "y": $scope.absentgraph[i].absentee })
                        }
                    }
                    console.log($scope.dataabsentgraph);
                    $scope.feegraphseries1 = [];
                    if ($scope.feedetailsgraph != null) {

                        for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                            $scope.feegraphseries1.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].recived })
                        }
                    }
                    $scope.feegraphseries2 = [];
                    if ($scope.feedetailsgraph != null) {

                        for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                            $scope.feegraphseries2.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].paid })
                        }
                    }
                    $scope.feegraphseries3 = [];
                    if ($scope.feedetailsgraph != null) {

                        for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                            $scope.feegraphseries3.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].ballance })
                        }
                    }
                    console.log($scope.feegraphseries3);

                    //columnchart
                    var chart = new CanvasJS.Chart("chartContainer", {
                        //columnchart
                        responsive: true,
                        legend: {
                            maxWidth: 150,
                            fontSize:12,
                        },
                        axisX: {
                            labelFontSize: 9,
                            interval: 1,
                            // title:"Class",
                        },
                        axisY: {
                            labelFontSize: 9,
                            // title: "Students",
                        },
                        data: [
                            {
                                type: "pie",
                                showInLegend: true,
                                toolTipContent: "{y} - #percent %",
                                legendText: "{label}",
                                dataPoints: $scope.datagraph,
                                labelFontSize: 12,
                                indexLabelFontWeight: "bold",

                            }
                        ]

                    });
                    chart.render();
                    var chart = new CanvasJS.Chart("areachart",
                        {
                            responsive: true,
                           
                            axisX: {
                                labelFontSize: 9,
                                interval: 1,
                                //title: "Department",
                            },
                            axisY: {
                                labelFontSize: 9,
                                // title: "No.of. Staffs",

                            },
                        
                            legend: {
                                maxWidth: 300,
                                fontSize: 10,
                            },
                            data: [
                                {
                                    type: "pie",
                                    showInLegend: true,
                                    indexLabelFontSize: 12,
                                    indexLabelFontWeight: "bold",
                                    toolTipContent: "{y} - #percent %",
                                    legendText: "{label}",
                                    dataPoints: $scope.dataabsentgraph
                                }
                            ]
                        });
                    chart.render();
                    var chart = new CanvasJS.Chart("rangeBarChat",
                        {
                            responsive: true,

                            axisX: {
                                labelFontSize: 9,
                                interval: 1,
                                //title: "Department",
                            },
                            axisY: {
                                labelFontSize: 9,
                                // title: "No.of. Staffs",

                            },

                            legend: {
                                maxWidth: 300,
                                fontSize: 10,
                            },

                            data: [
                                {
                                    type: "pie",
                                    showInLegend: true,
                                    indexLabel: "{label}-{y}",
                                    indexLabelFontSize: 12,
                                    indexLabelFontWeight: "bold",
                                    toolTipContent: "{y} - #percent %",
                                    legendText: "{label}",
                                    dataPoints: $scope.feegraphseries2
                                }
                            ]
                        });

                    chart.render();


                    //var chart = new CanvasJS.Chart("rangeBarChat");

                    //chart.options.axisX = { interval: 1, labelAngle: -20, labelFontSize: 11 };

                    //// chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                    //var series1 = { //dataSeries - first quarter
                    //    type: "column",
                    //    name: "receivable",
                    //    showInLegend: true
                    //};

                    //var series2 = { //dataSeries - second quarter
                    //    type: "column",
                    //    name: "collected",
                    //    showInLegend: true
                    //};

                    //var series3 = { //dataSeries - Third quarter
                    //    type: "column",
                    //    name: "balance",
                    //    showInLegend: true
                    //};


                    //chart.options.data = [];
                    //chart.options.data.push(series1);
                    //chart.options.data.push(series2);
                    //chart.options.data.push(series3);

                    //series1.dataPoints = $scope.feegraphseries1;
                    //series2.dataPoints = $scope.feegraphseries2;
                    //series3.dataPoints = $scope.feegraphseries3;
                    //chart.render();
                });
        }

        var HostName = location.host;

        $scope.oncertificate = function () {
            $window.location.href = 'http://' + HostName + '/#/app/StudyCertificate/';
        };
        $scope.onleave = function () {
            $window.location.href = 'http://' + HostName + '/#/app/LeaveReport/';
        };

        $scope.stdabs = function () {
           // $window.location.href = 'http://' + HostName + '/#/app/ClassWiseDailyAttendance/';
        };
    }

})();