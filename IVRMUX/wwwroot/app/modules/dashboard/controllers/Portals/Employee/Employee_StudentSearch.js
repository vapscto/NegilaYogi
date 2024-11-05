(function () {
    'use strict';
    angular.module('app').controller('EmployeeStudentSearchController', EmployeeStudentSearchController)
    EmployeeStudentSearchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function EmployeeStudentSearchController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {

        $scope.LoadData = function () {            
            apiService.getDATA("EmployeeStudentSearch/getdetails").then(function (promise) {
                $scope.studentlst = promise.fillstudent;
            });
        };

        $scope.searchfilter = function (studentlst) {            
            var studid = studentlst.amst_Id;
            $scope.showStudentD = false;
            $scope.studentdivlist = [];
            var data = {
                "Amst_Id": studid
            };

            apiService.create("EmployeeStudentSearch/getstudentdetails", data).then(function (promise) {
                $scope.studentlistall = promise.fillstudentalldetails;
                $scope.examdetails = promise.examlist;

                if ($scope.studentlistall != null) {
                    $scope.showStudentD = true;
                } else {
                    $scope.showStudentD = false;
                }

                var studentphtoto = "";
                if ($scope.studentlistall[0].amsT_Photoname != null) {
                    studentphtoto = $scope.studentlistall[0].amsT_Photoname;
                }
                else {
                    studentphtoto = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                }

                $('#blah').attr('src', studentphtoto);

                if ($scope.examdetails != null && $scope.examdetails != 0) {
                    $scope.showExamD = true;
                } else {
                    $scope.showExamD = false;
                    swal("Student did't attend any Exam....!!")
                }

                if (promise.studentdivlist !== null && promise.studentdivlist.length > 0) {
                    $scope.studentdivlist = promise.studentdivlist;

                    angular.forEach($scope.studentdivlist, function (dd) {
                        if (dd.ASCOMP_FilePath !== null && dd.ASCOMP_FilePath !== "") {
                            var img = dd.ASCOMP_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("data.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ASCOMP_FilePath;
                            }
                        }
                    });
                }
            });
        };


        // View Student Profile

        $scope.ViewStudentProfile = function () {
            $scope.myTabIndex2 = 0;
            $scope.year_name = "";
            $scope.showStudentD = false;
            $scope.viewstudentexamsubjectdetails = [];
            $scope.viewstudentwiseexamdetails = [];
            $scope.viewstudentattendancetails = [];
            $scope.viewstudentsubjectdetails = [];
            $scope.viewstudentfeedetails = [];
            $scope.viewstudentexamsubjectdetails = [];
            $scope.studentdivlist = [];

            var data = {
                "AMST_Id": $scope.Amst_Id.amst_Id,
                "student_staffflag" :'Staff'
            };

            apiService.create("StudentDashboard/ViewStudentProfile", data).then(function (promise) {

                if (promise !== null) {
                    $scope.viewstudentjoineddetails = promise.viewstudentjoineddetails;

                    if ($scope.viewstudentjoineddetails !== null && $scope.viewstudentjoineddetails.length > 0) {
                        $scope.showStudentD = true;
                        $scope.studentname_view = $scope.viewstudentjoineddetails[0].studentname;
                        $scope.amstadmno_view = $scope.viewstudentjoineddetails[0].amsT_AdmNo;
                        $scope.amstregno_view = $scope.viewstudentjoineddetails[0].amsT_RegistrationNo;
                        $scope.year_view = $scope.viewstudentjoineddetails[0].asmaY_Year;
                        $scope.class_view = $scope.viewstudentjoineddetails[0].asmcL_ClassName;
                                                
                        if ($scope.viewstudentjoineddetails[0].amsT_Photoname != null) {
                            $scope.photo_view = $scope.viewstudentjoineddetails[0].amsT_Photoname;
                        }
                        else {
                            $scope.photo_view = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                        }

                        $scope.gender_view = $scope.viewstudentjoineddetails[0].amsT_Sex;
                        $scope.status_view = $scope.viewstudentjoineddetails[0].amsT_SOL;
                        $scope.doa_view = new Date($scope.viewstudentjoineddetails[0].amsT_Date);
                        $scope.dob_view = new Date($scope.viewstudentjoineddetails[0].amsT_DOB);

                        $scope.viewstudentdetails = promise.viewstudentdetails;

                        if ($scope.viewstudentdetails !== null && $scope.viewstudentdetails.length > 0) {
                            $scope.mobile_view = $scope.viewstudentdetails[0].amsT_MobileNo;
                            $scope.email_view = $scope.viewstudentdetails[0].amsT_emailId;

                            //Father Details
                            $scope.FatherName = $scope.viewstudentdetails[0].amsT_FatherName;
                            $scope.FatherSurName = $scope.viewstudentdetails[0].amsT_FatherSurname === null
                                || $scope.viewstudentdetails[0].amsT_FatherSurname === "" ? "" : $scope.viewstudentdetails[0].amsT_FatherSurname;
                            $scope.Father_MobileNo = $scope.viewstudentdetails[0].amsT_FatherMobleNo;
                            $scope.Father_EmailId = $scope.viewstudentdetails[0].amsT_FatheremailId;                           

                            if ($scope.viewstudentdetails[0].ansT_FatherPhoto != null) {
                                $scope.Father_photo = $scope.viewstudentdetails[0].ansT_FatherPhoto;
                            }
                            else {
                                $scope.Father_photo = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                            }

                            //Mother Details
                            $scope.MotherName = $scope.viewstudentdetails[0].amsT_MotherName;
                            $scope.MotherSurName = $scope.viewstudentdetails[0].amsT_MotherSurname === null || $scope.viewstudentdetails[0].amsT_MotherSurname === "" || $scope.viewstudentdetails[0].amsT_MotherSurname === "0" ? "" : ' ' + $scope.viewstudentdetails[0].amsT_MotherSurname;
                            $scope.Mother_MobileNo = $scope.viewstudentdetails[0].amsT_MotherMobileNo;
                            $scope.Mother_EmailId = $scope.viewstudentdetails[0].amsT_MotherEmailId;                            

                            if ($scope.viewstudentdetails[0].ansT_MotherPhoto != null) {
                                $scope.Mother_photo = $scope.viewstudentdetails[0].ansT_MotherPhoto;
                            }
                            else {
                                $scope.Mother_photo = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                            }
                        }

                        if (promise.viewstudentacademicyeardetails !== null && promise.viewstudentacademicyeardetails.length > 0) {
                            $scope.viewstudentacademicyeardetails = promise.viewstudentacademicyeardetails;
                        }

                        if (promise.viewstudentguardiandetails !== null && promise.viewstudentguardiandetails.length > 0) {
                            $scope.viewstudentguardiandetails = promise.viewstudentguardiandetails;
                        }

                        //Over All Attendance Details
                        $scope.att_workingdays = [];
                        $scope.att_presentdays = [];
                        $scope.att_percentage = [];
                        if (promise.viewstudentattendancetails !== null && promise.viewstudentattendancetails.length > 0) {
                            $scope.viewstudentattendancetails = promise.viewstudentattendancetails;

                            angular.forEach($scope.viewstudentattendancetails, function (d) {
                                $scope.att_workingdays.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.WORKINGDAYS });
                                $scope.att_presentdays.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.PRESENTDAYS });
                                $scope.att_percentage.push({ label: d.ASMAY_Year + '-' + d.ASMCL_ClassName + '-' + d.ASMC_SectionName, "y": d.PERCENTAGE });
                            });

                            var chart = new CanvasJS.Chart("att_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Working Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_workingdays
                                },
                                {
                                    name: "Present Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_presentdays
                                },
                                {
                                    name: "Percentage",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_percentage
                                }]
                            });
                            chart.render();
                        }

                        // Year Month Wise Attendance Details
                        $scope.viewstudentattendanceMonthdetails = [];
                        $scope.att_Month_workingdays = [];
                        $scope.att_Month_presentdays = [];
                        if (promise.viewstudentattendanceMonthdetails !== null && promise.viewstudentattendanceMonthdetails.length > 0) {
                            $scope.viewstudentattendanceMonthdetails = promise.viewstudentattendanceMonthdetails;

                            angular.forEach($scope.viewstudentattendanceMonthdetails, function (d) {
                                $scope.att_Month_workingdays.push({ label: d.Months + '-' + d.Years, "y": d.WorkingCount });
                                $scope.att_Month_presentdays.push({ label: d.Months + '-' + d.Years, "y": d.PresentCount });
                            });

                            var chart = new CanvasJS.Chart("att_Month_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Working Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_Month_workingdays
                                },
                                {
                                    name: "Present Days",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.att_Month_presentdays
                                }]
                            });
                            chart.render();
                        }


                        //Subject Details For Current Year
                        if (promise.viewstudentsubjectdetails !== null && promise.viewstudentsubjectdetails.length > 0) {
                            $scope.viewstudentsubjectdetails = promise.viewstudentsubjectdetails;
                        }

                        //Exam Details
                        if (promise.viewstudentwiseexamdetails !== null && promise.viewstudentwiseexamdetails.length > 0) {
                            $scope.viewstudentwiseexamdetails = promise.viewstudentwiseexamdetails;
                        }

                        // Fee Details
                        $scope.fee_yearlycharges = [];
                        $scope.fee_Concession = [];
                        $scope.fee_Payable = [];
                        $scope.fee_PaidAmount = [];
                        $scope.fee_Outstanding = [];
                        if (promise.viewstudentfeedetails !== null && promise.viewstudentfeedetails.length > 0) {
                            $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                            angular.forEach($scope.viewstudentfeedetails, function (d) {
                                $scope.fee_yearlycharges.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.YearlyCharges });
                                $scope.fee_Concession.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Concession });
                                $scope.fee_Payable.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Payable });
                                $scope.fee_PaidAmount.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.PaidAmount });
                                $scope.fee_Outstanding.push({ label: d.asmay_year + '-' + d.ClassSection, "y": d.Outstanding });
                            });

                            var chart = new CanvasJS.Chart("fee_profile_chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                colorSet: "graphcolor",
                                axisX: {
                                    labelFontSize: 13,
                                },
                                axisY: {
                                    labelFontSize: 13,
                                },

                                toolTip: {
                                    shared: true
                                },
                                data: [{
                                    name: "Yearly Charges",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_yearlycharges
                                },
                                {
                                    name: "Concession",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Concession
                                },
                                {
                                    name: "Payable",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Payable
                                },
                                {
                                    name: "Paid Amount",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_PaidAmount
                                },
                                {
                                    name: "Outstanding",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.fee_Outstanding
                                }]
                            });
                            chart.render();

                        }

                        // Fee Yearly Paid Details
                        $scope.viewstudenfeeyeardetails = [];
                        if (promise.viewstudenfeeyeardetails !== null && promise.viewstudenfeeyeardetails.length > 0) {
                            $scope.viewstudenfeeyeardetails = promise.viewstudenfeeyeardetails;
                        }

                        //Compliants list
                        if (promise.studentdivlist !== null && promise.studentdivlist.length > 0) {
                            $scope.studentdivlist = promise.studentdivlist;

                            angular.forEach($scope.studentdivlist, function (dd) {
                                if (dd.ASCOMP_FilePath !== null && dd.ASCOMP_FilePath !== "") {
                                    var img = dd.ASCOMP_FilePath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    console.log("data.filetype : " + dd.filetype);
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ASCOMP_FilePath;
                                    }
                                }
                            });
                        }

                        //Address
                        if (promise.viewstudentaddressdetails !== null && promise.viewstudentaddressdetails.length > 0) {
                            $scope.viewstudentaddressdetails = promise.viewstudentaddressdetails;

                            if ($scope.viewstudentaddressdetails[0].PermanentAddress !== null && $scope.viewstudentaddressdetails[0].PermanentAddress !== "") {
                                $scope.permanentaddress = $scope.viewstudentaddressdetails[0].PermanentAddress.split(',');
                            }

                            if ($scope.viewstudentaddressdetails[0].ContactAddress !== null && $scope.viewstudentaddressdetails[0].ContactAddress !== "") {
                                $scope.communicationaddress = $scope.viewstudentaddressdetails[0].ContactAddress.split(',');
                            }
                        }

                        $('#mymodalviewdetais').modal('show');
                    }
                }
            });
        };

        $scope.ViewMonthWiseAttendance = function (dd) {

            $scope.viewstudentattendanceMonthdetails = [];
            $scope.att_Month_workingdays = [];
            $scope.att_Month_presentdays = [];
            $scope.year_name = dd.ASMAY_Year;
            document.getElementById('att_Month_profile_chartContainer').innerHTML = "";
            var data = {
                "AMST_Id": $scope.Amst_Id.amst_Id,
                "ASMAY_Id": dd.ASMAY_Id,
                "student_staffflag": 'Staff'
            };

            apiService.create("StudentDashboard/ViewMonthWiseAttendance", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.viewstudentattendanceMonthdetails !== null && promise.viewstudentattendanceMonthdetails.length > 0) {
                        $scope.viewstudentattendanceMonthdetails = promise.viewstudentattendanceMonthdetails;

                        angular.forEach($scope.viewstudentattendanceMonthdetails, function (d) {
                            $scope.att_Month_workingdays.push({ label: d.Months + '-' + d.Years, "y": d.WorkingCount });
                            $scope.att_Month_presentdays.push({ label: d.Months + '-' + d.Years, "y": d.PresentCount });
                        });

                        var chart = new CanvasJS.Chart("att_Month_profile_chartContainer", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 13,
                            },
                            axisY: {
                                labelFontSize: 13,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [{
                                name: "Working Days",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.att_Month_workingdays
                            },
                            {
                                name: "Present Days",
                                showInLegend: true,
                                type: "column",
                                dataPoints: $scope.att_Month_presentdays
                            }]
                        });
                        chart.render();
                    }
                }
            });
        };

        $scope.ViewYearWiseFee = function (dd) {

            $scope.viewstudenfeeyeardetails = [];
            $scope.year_name = dd.asmay_year;
            var data = {
                "AMST_Id": $scope.Amst_Id.amst_Id,
                "ASMAY_Id": dd.ASMAY_Id,
                "student_staffflag": 'Staff'
            };

            apiService.create("StudentDashboard/ViewYearWiseFee", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.viewstudenfeeyeardetails !== null && promise.viewstudenfeeyeardetails.length > 0) {
                        $scope.viewstudenfeeyeardetails = promise.viewstudenfeeyeardetails;
                    }
                }
            });
        };

        $scope.ViewExamSubjectWiseDetails = function (dd) {
            $scope.viewstudentexamsubjectdetails = [];
            $scope.examname = dd.EME_ExamName;
            var data = {
                "EME_Id": dd.EME_Id,
                "ASMAY_Id": dd.ASMAY_Id,
                "AMST_Id": $scope.Amst_Id.amst_Id,
                "student_staffflag": 'Staff'
            };

            apiService.create("StudentDashboard/ViewExamSubjectWiseDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.viewstudentexamsubjectdetails = promise.viewstudentexamsubjectdetails;
                }

            });
        };



        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        $scope.cancel = function () {
            $state.reload();
        };
    };

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });

})();