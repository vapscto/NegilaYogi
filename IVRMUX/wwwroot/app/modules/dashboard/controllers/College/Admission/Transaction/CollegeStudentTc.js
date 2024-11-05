(function () {
    'use strict';
    angular.module('app').controller('CollegeStudentTcController', CollegeStudentTcController)

    CollegeStudentTcController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache']
    function CollegeStudentTcController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $route, $q, superCache) {

        $scope.usr = {};
        $scope.tcdata = [];
        var date = new Date();
        var nextYear = date.getFullYear() + 1;
        var currYear = date.getFullYear();
        $scope.acdYear = currYear + "-" + nextYear;
        $scope.date1 = new Date();   
        //--------------------
        $scope.tcnumbering = [];
        //------
        $scope.ASTC_Id = "";
        $scope.Tc_payment = "";
        $scope.adm_flag = true;
        $scope.cat_flag = true;
        $scope.tempchk_tc = false;
        $scope.details_flag = false;
        $scope.class_flag = true;
        $scope.message_flag = true;
        $scope.message_flag_lang = true;
        $scope.other_details_flag = true;
        $scope.tc_details_flag = true;
        $scope.tab_hide = false;
        $scope.stu_details = true;
        $scope.stu_pers = true;
        $scope.due = true;
        $scope.other_details = true;
        $scope.tc_dis_flag = false;
        $scope.pic_show = false;
        $scope.stud_name = false;
        $scope.class_name = true;
        $scope.section_name = true;
        $scope.next_on_tc_no = false;
        $scope.yr_show = false;
        $scope.clas_show = false;
        $scope.sec_show = false;
        $scope.adm_number_flag = true;
        $scope.submitted1 = false;
        var subjectname1 = "";
        var subjectname2 = "";
        $scope.rommanclass = ["I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X"];


        $scope.validateStuDet = function () {
            if ($scope.myForm1.$valid) {
                $scope.stu_details = false;
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted1 = true;
                $scope.stu_details = true;
            }
        };

        $scope.submitted2 = false;

        $scope.tc_details = function () {
            if ($scope.myForm2.$valid) {
                $scope.stu_pers = false;
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted2 = true;
                $scope.stu_pers = true;
            }
        };

        $scope.next_tab = function () {
            $scope.other_details = false;
            $scope.myTabIndex = $scope.myTabIndex + 1;
        };

        $scope.submitted4 = false;

        $scope.other_details_next = function () {
            if ($scope.myForm4.$valid) {
                $scope.due = false;
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted4 = true;
                $scope.due = true;
            }
        };

        $scope.Previous_tab = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.interacted1 = function () {
            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2 || field.$dirty;
        };

        $scope.interacted4 = function (field) {
            return $scope.submitted4 || field.$dirty;
        };

        $scope.submitted = false;

        $scope.validationtab = function () {
            if ($scope.myForm1.$valid) {
                $scope.checkvalid = false;
            }
            else {
                $scope.checkvalid = true;
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.IsHiddenTC = true;

        $scope.ShowHideTC = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHiddenTC = $scope.IsHiddenTC ? false : true;
        };


        $scope.IsHiddenTC_Details = true;
        $scope.ShowHideTC_Details = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHiddenTC_Details = $scope.IsHiddenTC_Details ? false : true;
        };

        $scope.IsHidden_Other_details = true;
        $scope.ShowHide_Other_details = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden_Other_details = $scope.IsHidden_Other_details ? false : true;
        };

        $scope.IsHidden_Due_Details = true;
        $scope.ShowHide_Due_Details = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden_Due_Details = $scope.IsHidden_Due_Details ? false : true;
        };


        //Loading the Intial Data 
        $scope.loadInitialData = function (user) {

            $scope.IsHiddenTC_Details = false;
            $scope.ShowHideTC_Details = function () {
                //If DIV is hidden it will be visible and vice versa.
                $scope.IsHiddenTC_Details = $scope.IsHiddenTC_Details ? false : true;
            };

            $scope.IsHidden_Other_details = false;
            $scope.ShowHide_Other_details = function () {
                //If DIV is hidden it will be visible and vice versa.
                $scope.IsHidden_Other_details = $scope.IsHidden_Other_details ? false : true;
            };

            $scope.IsHidden_Due_Details = false;
            $scope.ShowHide_Due_Details = function () {
                //If DIV is hidden it will be visible and vice versa.
                $scope.IsHidden_Due_Details = $scope.IsHidden_Due_Details ? false : true;
            };

            $scope.studentlist = [];
            $scope.getconfigurationdetails = [];
            var id = 4;
            apiService.getURI("CollegeStudenttctransaction/loaddata", id).then(function (promise) {

                $scope.getyear = promise.getyear;


                if (promise.getconfigurationdetails !== null && promise.getconfigurationdetails.length > 0) {
                    if (promise.getconfigurationdetails[0].asC_AdmNo_RegNo_RollNo_DefaultFlag === "3") {
                        $scope.user.status = 'S';
                        $scope.user.admno_name = 'A';
                    }
                    else if (promise.getconfigurationdetails[0].asC_AdmNo_RegNo_RollNo_DefaultFlag === "1") {
                        $scope.user.status = 'S';
                        $scope.user.admno_name = 'N';
                    }
                    else {
                        $scope.user.status = 'S';
                        $scope.user.admno_name = 'N';
                    }
                }
                else {
                    $scope.user.status = 'S';
                    $scope.user.admno_name = 'N';
                }

                var transnumconfig = promise.admTransNumSetting;
                localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));
                var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
                if (transnumconfigsettings !== null && transnumconfigsettings.length > 0) {
                    for (var i = 0; i < transnumconfigsettings.length; i++) {
                        if (transnumconfigsettings.length > 0) {
                            $scope.transnumbconfigurationsettings = transnumconfigsettings[i];
                            if (transnumconfigsettings[i].imN_Flag === "tcno") {
                                $scope.tcnumbering = transnumconfigsettings[i];
                                if (transnumconfigsettings[i].imN_AutoManualFlag === "Manual") {
                                    $scope.reg_ = true;
                                }
                                else {

                                    $scope.reg_ = false;
                                }
                            }
                        }
                    }
                }
                else {
                    $scope.reg_ = true;
                    swal("Please Map The Transaction Numbering For TC Number Generation i.e., Auto/ Manual");
                }


                $scope.adm_number_flag = true;
                $scope.Tc_payment = promise.getconfigurationdetails[0].admC_TCAllowBalanceFlg;

            });
        };

        // on change year
        $scope.onchangeyear = function () {

            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.details_flag = false;
            $scope.tab_hide = false;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("CollegeStudenttctransaction/onchangeyear", data).then(function (promise) {
                $scope.getcourse = promise.getcourse;
            });
        };

        // ON CHANGE COURSE
        $scope.onchangecourse = function () {

            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.details_flag = false;
            $scope.tab_hide = false;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("CollegeStudenttctransaction/onchangecourse", data).then(function (promise) {
                $scope.getbranch = promise.getbranch;
            });
        };

        // ON CHANGE BRANCH
        $scope.onchangebranch = function () {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.details_flag = false;
            $scope.tab_hide = false;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id

            };
            apiService.create("CollegeStudenttctransaction/onchangebranch", data).then(function (promise) {
                $scope.getsemester = promise.getsemester;
            });
        };

        // ON CHANGE SEMESTER
        $scope.onchangesemester = function () {

            $scope.ACMS_Id = "";
            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.details_flag = false;
            $scope.tab_hide = false;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id

            };
            apiService.create("CollegeStudenttctransaction/onchangesemester", data).then(function (promise) {
                $scope.getssection = promise.getssection;
            });
        };

        // ON CHANGE SECTION
        $scope.onchangesection = function () {

            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.details_flag = false;
            $scope.tab_hide = false;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id

            };
            apiService.create("CollegeStudenttctransaction/onchangesection", data).then(function (promise) {

            });
        };

        // ON SEARCH  FILTER        
        $scope.searchfilter = function (objj, radioobj) {

            if (objj.search.length >= '2') {

                $scope.studentlst = "";
                var courseid = 0;
                var branchid = 0;
                var semesterid = 0;
                var sectionid = 0;

                if ($scope.Admnoallind === "All") {
                    courseid = 0;
                    branchid = 0;
                    semesterid = 0;
                    sectionid = 0;
                } else {
                    courseid = $scope.AMCO_Id;
                    branchid = $scope.AMB_Id;
                    semesterid = $scope.AMSE_Id;
                    sectionid = $scope.ACMS_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": courseid,
                    "AMB_Id": branchid,
                    "AMSE_Id": semesterid,
                    "ACMS_Id": sectionid,
                    "searchfilter": objj.search,
                    "AMCST_SOL": radioobj,
                    "allorindividual": $scope.Admnoallind,
                    "adm_num_flag": $scope.user.admno_name
                };
                apiService.create("CollegeStudenttctransaction/searchfilter", data).
                    then(function (promise) {
                        if (promise.getstudentlist !== null || promise.getstudentlist.length > 0) {
                            $scope.studentname = promise.getstudentlist;
                        } else {
                            $scope.AMST_Id = "";
                            swal("No students are found for your search");
                        }
                    });
            }
        };

        // ON CHANGE STUDENT
        $scope.onchangestudent = function () {

            $scope.details_flag = false;
            $scope.tab_hide = false;

            $scope.IsHiddenTC_Details = true;
            $scope.ShowHideTC_Details = function () {
                $scope.IsHiddenTC_Details = $scope.IsHiddenTC_Details ? false : true;
            };

            $scope.IsHidden_Other_details = true;
            $scope.ShowHide_Other_details = function () {
                $scope.IsHidden_Other_details = $scope.IsHidden_Other_details ? false : true;
            };

            $scope.IsHidden_Due_Details = true;
            $scope.ShowHide_Due_Details = function () {
                $scope.IsHidden_Due_Details = $scope.IsHidden_Due_Details ? false : true;
            };

            var courseid = 0;
            var branchid = 0;
            var semesterid = 0;
            var sectionid = 0;

            if ($scope.Admnoallind === "All") {
                courseid = 0;
                branchid = 0;
                semesterid = 0;
                sectionid = 0;
            } else {
                courseid = $scope.AMCO_Id;
                branchid = $scope.AMB_Id;
                semesterid = $scope.AMSE_Id;
                sectionid = $scope.ACMS_Id;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": courseid,
                "AMB_Id": branchid,
                "AMSE_Id": semesterid,
                "ACMS_Id": sectionid,
                "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                "AMCST_SOL": $scope.user.status,
                "adm_num_flag": $scope.user.admno_name
            };

            apiService.create("CollegeStudenttctransaction/onchangestudent", data).then(function (promise) {

                $scope.getstudentdetails = promise.getstudentdetails;

                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {


                    if ($scope.user.status === 'S' || $scope.user.status === 'D') {
                        $scope.details_flag = true;
                        $scope.tab_hide = true;
                        $scope.IsHidden_Other_details = true;
                        $scope.IsHiddenTC_Details = true;
                        $scope.IsHiddenTC_Details = true;
                        $scope.IsHidden_Due_Details = true;
                        $scope.pic_show = true;

                        $scope.studentname = $scope.getstudentdetails[0].studentname;
                        $scope.fathername = $scope.getstudentdetails[0].fathername;
                        $scope.coursename = $scope.getstudentdetails[0].coursename;
                        $scope.branchaname = $scope.getstudentdetails[0].branchname;
                        $scope.semestername = $scope.getstudentdetails[0].semestername;
                        $scope.sectionname = $scope.getstudentdetails[0].sectionname;
                        $scope.Stu_Img = $scope.getstudentdetails[0].studentphoto;
                        $scope.admno = $scope.getstudentdetails[0].admno;
                        $scope.regno = $scope.getstudentdetails[0].regno;
                        $scope.Admission_Date = new Date(promise.getstudentdetails[0].doa);
                        $scope.date_of_promotion = new Date();
                        $scope.last_class_studied = $scope.coursename + ' ' + $scope.branchaname + ' ' + $scope.semestername + ' ' + $scope.sectionname;
                        $scope.category = $scope.getstudentdetails[0].castename;
                        $scope.med_instruction = "";

                        $scope.scholarship = "";
                        $scope.is_medically_examined = "";


                        $scope.applicationdate = new Date();
                        $scope.leaving_reason = "";
                        $scope.tc_date = new Date();



                        $scope.Birth_Date = new Date($scope.getstudentdetails[0].dob);
                        $scope.Birth_Date_words = $scope.getstudentdetails[0].dobinwords;
                        $scope.stu_ifsc_code = $scope.getstudentdetails[0].ifsccode;
                        $scope.stu_birth_place = $scope.getstudentdetails[0].birthplace;
                        $scope.stu_age = $scope.getstudentdetails[0].age;
                        $scope.stu_gender = $scope.getstudentdetails[0].gender;
                        $scope.stu_mother_tongue = $scope.getstudentdetails[0].mothertounge;
                        $scope.stu_nationality = $scope.getstudentdetails[0].nationality;
                        $scope.stu_mobile = $scope.getstudentdetails[0].mobileno;
                        $scope.stu_email = $scope.getstudentdetails[0].emailid;
                        $scope.stu_aadhar_no = $scope.getstudentdetails[0].aadharno;

                        $scope.paid_fees = "";
                        $scope.fees_concession = "";
                        $scope.last_exam_detail = "";
                        $scope.results = "";
                        $scope.status = "";
                        $scope.promotion = "";
                        $scope.promotedtype = "";
                        $scope.ncc = "";
                        $scope.extra = "";
                        $scope.conduct = "";
                        $scope.remarks = "";


                        $scope.fees_due = "";
                        $scope.lib_due_trans = "";
                        $scope.store_tran_due = "";
                        $scope.pda_due = "";


                        $scope.getstudentattendancedetails = promise.getstudentattendancedetails;
                        if ($scope.getstudentattendancedetails !== null && $scope.getstudentattendancedetails.length > 0) {

                            if ($scope.getstudentattendancedetails[0].classheld !== null) {

                                $scope.no_school_days = $scope.getstudentattendancedetails[0].classheld;
                                $scope.no_attended_days = $scope.getstudentattendancedetails[0].classattened;
                                $scope.last_date_attendance = new Date($scope.getstudentattendancedetails[0].lastdate);
                                $scope.noscatt = true;
                            }
                            else {
                                $scope.no_school_days = "";
                                $scope.no_attended_days = "";
                                $scope.last_date_attendance = new Date();
                                $scope.noscatt = false;
                            }

                        } else {
                            $scope.no_school_days = "";
                            $scope.no_attended_days = "";
                            $scope.last_date_attendance = new Date();
                            $scope.noscatt = false;
                        }

                        $scope.getstudentfeedetails = promise.getstudentfeedetails;
                        if ($scope.getstudentfeedetails !== null && $scope.getstudentfeedetails.length > 0) {
                            $scope.fees_due = $scope.getstudentfeedetails[0].balance;
                            if ($scope.fees_due !== null) {
                                if ($scope.fees_due > 0) {
                                    $scope.paid_fees = 'No';
                                } else {
                                    $scope.paid_fees = 'Yes';
                                }
                            }
                            else {
                                $scope.fees_due = "";
                            }
                        } else {
                            $scope.fees_due = "";
                        }

                        $scope.getstudentlanguagesubjects = promise.getstudentlanguagesubjects;
                        if ($scope.getstudentlanguagesubjects !== null && $scope.getstudentlanguagesubjects.length > 0) {
                            var subjectname1 = "";
                            var subjectname2 = "";
                            for (var i = 0; i < $scope.getstudentlanguagesubjects.length; i++) {
                                if ($scope.getstudentlanguagesubjects[i].flag === 1) {
                                    if (i === 0) {
                                        subjectname1 = $scope.getstudentlanguagesubjects[i].subjectname;
                                    }
                                    else {
                                        subjectname1 = subjectname1 + ',' + promise.getstudentlanguagesubjects[i].subjectname;
                                    }
                                } else {
                                    if (i === 0) {
                                        subjectname2 = $scope.getstudentlanguagesubjects[i].subjectname;
                                    }
                                    else {
                                        subjectname2 = subjectname2 + ',' + promise.getstudentlanguagesubjects[i].subjectname;
                                    }
                                }
                            }
                            $scope.language_study = subjectname1;
                            $scope.elective_study = subjectname2;
                        } else {
                            $scope.language_study = "";
                            $scope.elective_study = "";
                        }
                    }


                    else if ($scope.user.status === "L" || $scope.user.status === "T") {

                        $scope.details_flag = true;
                        $scope.tab_hide = true;
                        $scope.IsHidden_Other_details = true;
                        $scope.IsHiddenTC_Details = true;
                        $scope.IsHiddenTC_Details = true;
                        $scope.IsHidden_Due_Details = true;
                        $scope.pic_show = true;

                        $scope.studentname = $scope.getstudentdetails[0].studentname;
                        $scope.fathername = $scope.getstudentdetails[0].fathername;
                        $scope.coursename = $scope.getstudentdetails[0].coursename;
                        $scope.branchaname = $scope.getstudentdetails[0].branchname;
                        $scope.semestername = $scope.getstudentdetails[0].semestername;
                        $scope.sectionname = $scope.getstudentdetails[0].sectionname;
                        $scope.Stu_Img = $scope.getstudentdetails[0].studentphoto;
                        $scope.admno = $scope.getstudentdetails[0].admno;
                        $scope.regno = $scope.getstudentdetails[0].regno;
                        $scope.Admission_Date = new Date(promise.getstudentdetails[0].doa);
                        $scope.date_of_promotion = new Date(promise.getstudentdetails[0].ACSTC_PromotionDate);
                        $scope.category = $scope.getstudentdetails[0].castename;
                        $scope.med_instruction = promise.getstudentdetails[0].ACSTC_MediumOfINStruction;
                        $scope.scholarship = promise.getstudentdetails[0].ACSTC_Scholarship;
                        $scope.is_medically_examined = promise.getstudentdetails[0].ACSTC_MedicallyExam;
                        $scope.applicationdate = new Date(promise.getstudentdetails[0].ACSTC_TCApplicationDate);
                        $scope.leaving_reason = promise.getstudentdetails[0].ACSTC_LeavingReason;
                        $scope.tc_date = new Date(promise.getstudentdetails[0].ACSTC_TCDate);
                        $scope.Birth_Date = new Date($scope.getstudentdetails[0].dob);
                        $scope.Birth_Date_words = $scope.getstudentdetails[0].dobinwords;
                        $scope.stu_ifsc_code = $scope.getstudentdetails[0].ifsccode;
                        $scope.stu_birth_place = $scope.getstudentdetails[0].birthplace;
                        $scope.stu_age = $scope.getstudentdetails[0].age;
                        $scope.stu_gender = $scope.getstudentdetails[0].gender;
                        $scope.stu_mother_tongue = $scope.getstudentdetails[0].mothertounge;
                        $scope.stu_nationality = $scope.getstudentdetails[0].nationality;
                        $scope.stu_mobile = $scope.getstudentdetails[0].mobileno;
                        $scope.stu_email = $scope.getstudentdetails[0].emailid;
                        $scope.stu_aadhar_no = $scope.getstudentdetails[0].aadharno;
                        $scope.usr.astC_TCNO = $scope.getstudentdetails[0].ACSTC_TCNO; 
                        
                        $scope.fees_concession = $scope.getstudentdetails[0].ACSTC_FeeConcession;       
                        $scope.last_exam_detail = $scope.getstudentdetails[0].ACSTC_LastExamDetails;       
                        $scope.results = $scope.getstudentdetails[0].ACSTC_Result;       
                        $scope.status = $scope.getstudentdetails[0].ACSTC_ResultDetails;       
                        $scope.promotion = $scope.getstudentdetails[0].ACSTC_Qual_PromotionFlag;
                        $scope.promotedtype = $scope.getstudentdetails[0].ACSTC_Qual_Course;       
                        $scope.ncc = $scope.getstudentdetails[0].ACSTC_NCCDetails;       
                        $scope.extra = $scope.getstudentdetails[0].ACSTC_ExtraActivities;       
                        $scope.conduct = $scope.getstudentdetails[0].ACSTC_Conduct;       
                        $scope.remarks = $scope.getstudentdetails[0].ACSTC_Remarks;       


                        $scope.fees_due = $scope.getstudentdetails[0].Fee_Due_Amnt;       
                        $scope.lib_due_trans = $scope.getstudentdetails[0].Library_Due_Amnt;       
                        $scope.store_tran_due = $scope.getstudentdetails[0].Store_Canteen_Due;       
                        $scope.pda_due = $scope.getstudentdetails[0].PDA_Due;       

                        if ($scope.getstudentdetails[0].acstC_WorkingDays !== null) {

                            $scope.no_school_days = $scope.getstudentdetails[0].ACSTC_WorkingDays;
                            $scope.no_attended_days = $scope.getstudentdetails[0].ACSTC_AttendedDays;
                            $scope.last_date_attendance = new Date($scope.getstudentdetails[0].ACSTC_LastAttendedDate);
                            $scope.noscatt = true;
                        }
                        else {
                            $scope.no_school_days = "";
                            $scope.no_attended_days = "";
                            $scope.last_date_attendance = new Date();
                            $scope.noscatt = false;
                        }


                        $scope.fees_due = $scope.getstudentdetails[0].Fee_Due_Amnt;
                        if ($scope.fees_due !== null) {
                            if ($scope.fees_due > 0) {
                                $scope.paid_fees = 'No';
                            } else {
                                $scope.paid_fees = 'Yes';
                            }
                        }
                        else {
                            $scope.fees_due = "";
                        }
                        $scope.language_study = $scope.getstudentdetails[0].ACSTC_LanguageStudied;
                        $scope.elective_study = $scope.getstudentdetails[0].ACSTC_ElectivesStudied;
                    }
                } else {
                    swal("No Records Found");
                    $scope.details_flag = false;
                    $scope.tab_hide = false;
                    $scope.IsHidden_Other_details = false;
                    $scope.IsHiddenTC_Details = false;
                    $scope.IsHiddenTC_Details = false;
                    $scope.IsHidden_Due_Details = false;
                }

            });
        };


        //Checking Duplicate Tc number
        $scope.check_dup_tc_no = function (val) {

            var data = {
                "ACSTC_TCNO": val
            };
            apiService.create("CollegeStudenttctransaction/chk_dup_tc", data).then(function (promise) {
                if (promise.message === "Exists") {
                    swal('TC Number Already Exists', 'Please Enter Different TC Number');
                    $scope.next_on_tc_no = true;
                }
                else {
                    $scope.next_on_tc_no = false;
                }
            });
        };

        $scope.prevsec = {};
        $scope.lastClassStudied = {};
        $scope.lastclsatu = {};
        $scope.classstu = {};
        $scope.studentDetails = [];
        $scope.studentname = [];
        $scope.classStudy = null;
        $scope.noscatt = false;

        $scope.studentListById = [];

        //Getting active student drop down list
        $scope.status_function = function (user) {

            $scope.AMST_Id = "";
            $scope.ASMAY_Year = null;
            $scope.Reg_No = null;
            $scope.Adm_No = null;
            $scope.class_name = null;
            $scope.section_name = null;
            $scope.category = null;
            $scope.Amc_Name = null;
            $scope.Stu_Img = null;
            $scope.IsHidden_Other_details = false;
            $scope.IsHiddenTC_Details = false;
            $scope.IsHidden_Due_Details = false;
            $scope.tab_hide = false;
            $scope.cls_section_name = null;
            $scope.details_flag = false;
            $scope.adm_number_flag = true;;

            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.details_flag = false;
            $scope.tab_hide = false;

            if ($scope.user.status === "S") {

                $scope.academic_year = "";
                $scope.classname = "";
                $scope.sectionname = "";
                $scope.tempchk_tc = false;
                $scope.tc_dis_flag = false;
                $scope.pic_show = false;
                $scope.stu_details = true;
                $scope.stu_pers = true;
                $scope.due = true;
                $scope.other_details = true;
            }


            if ($scope.user.status === "L") {
                $scope.reg_ = true;
                $scope.tcnoo = true;
                $scope.tempchk_tc = true;
                $scope.tc_dis_flag = true;
                $scope.pic_show = false;
                $scope.stu_details = true;
                $scope.stu_pers = true;
                $scope.due = true;
                $scope.other_details = true;
            }
            if ($scope.user.status === "D") {
                $scope.tempchk_tc = false;
                $scope.tc_dis_flag = false;
                $scope.pic_show = false;
                $scope.stu_details = true;
                $scope.stu_pers = true;
                $scope.due = true;
                $scope.other_details = true;
            }
            if ($scope.user.status === "T") {
                $scope.tempchk_tc = true;
                $scope.tc_dis_flag = true;
                $scope.pic_show = false;
                $scope.stu_details = true;
                $scope.stu_pers = true;
                $scope.due = true;
                $scope.other_details = true;
            }
        };

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $scope.submitted1 = false;
        $scope.submitted2 = false;
        $scope.submitted4 = false;
        //Saving The TC Data

        $scope.saveUser = function () {

            if ($scope.myForm1.$valid && $scope.myForm2.$valid && $scope.myForm4.$valid) {
                if ($scope.date_of_promotion !== "" && $scope.date_of_promotion !== null && $scope.date_of_promotion !== undefined) {
                    $scope.promotionDate = new Date($scope.date_of_promotion).toDateString();
                }
                else {
                    $scope.promotionDate = "";
                }

                var tempchk = 0;
                if ($scope.tempchk === true) {
                    $scope.tempchk = 1;
                } else {
                    $scope.tempchk = 0;
                }

                if ($scope.fees_due > 0) {
                    //checking payement required for permanent tc  1 means payment required
                    if ($scope.Tc_payment === 1 && ($scope.user.status === 'S' || $scope.user.status === 'D' || $scope.user.status === 'T')) {
                        //checking whether tc type is permanent or temparory
                        if ($scope.tempchk === true) {
                            swal({
                                title: $scope.fees_due + " " + "Rs." + " " + "Amount is Due!",
                                text: "Do you want to Submit the Details?",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes Submit it",
                                cancelButtonText: "Cancel..!",
                                closeOnConfirm: false,
                                closeOnCancel: false
                            },
                                function (isConfirm) {
                                    if (isConfirm) {
                                        $scope.getEmailsendingConfirmation();
                                        var year = "";
                                        year = $scope.academic_year;
                                        $scope.qualified_class = $scope.promotedtype;

                                        var data = {
                                            "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                                            "ACSTC_TCNO": $scope.usr.astC_TCNO,
                                            "ACSTC_TCApplicationDate": new Date($scope.applicationdate).toDateString(),
                                            "ACSTC_LeavingReason": $scope.leaving_reason,
                                            "ACSTC_TCDate": new Date($scope.tc_date).toDateString(),
                                            "ACSTC_TCIssueDate": new Date($scope.tc_date).toDateString(),
                                            "ACSTC_LastAttendedDate": new Date($scope.last_date_attendance).toDateString(),
                                            "ACSTC_WorkingDays": parseInt($scope.no_school_days),
                                            "ACSTC_AttendedDays": parseInt($scope.no_attended_days),
                                            "ACSTC_PromotionDate": $scope.promotionDate,
                                            "ACSTC_MediumOfINStruction": $scope.med_instruction,
                                            "ACSTC_LanguageStudied": $scope.language_study,
                                            "ACSTC_ElectivesStudied": $scope.elective_study,
                                            "ACSTC_Scholarship": $scope.scholarship,
                                            "ACSTC_MedicallyExam": $scope.is_medically_examined,
                                            "ACSTC_FeePaid": $scope.paid_fees,
                                            "ACSTC_FeeConcession": $scope.fees_concession,
                                            "ACSTC_LastExamDetails": $scope.last_exam_detail,
                                            "ACSTC_Result": $scope.results,
                                            "ACSTC_ResultDetails": $scope.status,
                                            "ACSTC_Qual_PromotionFlag": $scope.promotion,
                                            "ACSTC_Qual_Course": $scope.qualified_class,
                                            "ACSTC_NCCDetails": $scope.ncc,
                                            "ACSTC_ExtraActivities": $scope.extra,
                                            "ACSTC_Conduct": $scope.conduct,
                                            "ACSTC_Remarks": $scope.remarks,
                                            "ACSTC_TemporaryFlag": tempchk,
                                            "ACSTC_ActiveFlag": $scope.user.status,
                                            "Fee_Due_Amnt": $scope.fees_due,
                                            "Library_Due_Amnt": $scope.lib_due_trans,
                                            "Store_Canteen_Due": $scope.store_tran_due,
                                            "PDA_Due": $scope.pda_due,
                                            "Email_flag": $scope.Email_flag,
                                            "transnumconfigsettings": $scope.tcnumbering,
                                            "allorindividual": $scope.Admnoallind,
                                            "ASMAY_Id": $scope.ASMAY_Id
                                        };


                                        apiService.create("CollegeStudenttctransaction/savetc", data).then(function (promise) {

                                                if (promise.message === "Exists") {
                                                    swal('TC Number Already Exists', 'Please Enter Different TC Number');
                                                }
                                                else if (promise.returnval === true) {
                                                    swal('Record Saved Successfully');
                                                    $state.reload();
                                                    if (promise.email_flag === 'sent') {
                                                        alert('Mail Sent Successfully');
                                                        $state.reload();
                                                    }
                                                    else if (promise.email_flag === 'sms_sent') {
                                                        alert('SMS Sent Successfully');
                                                        $state.reload();
                                                    }
                                                    else {
                                                        //dd
                                                    }
                                                }
                                                else {
                                                    swal('Record Already Exists/Failed to Save');
                                                }
                                            });
                                    }
                                    else {
                                        swal("Cancelled");
                                    }
                                });//
                        }
                        //payment is required so it does not allow the to save tc for permanent
                        else {
                            swal($scope.fees_due + " " + "Rs." + " " + "Amount is Due! So You Can Not Proceed Untill Fee Due Is Clear");
                            $state.reload();
                        }
                    }
                    // else for when payment doesnot required to give permanent tc or temparory tc
                    else {
                        swal({
                            title: $scope.fees_due + " " + "Rs." + " " + "Amount is Due!",
                            text: "Do you want to Submit the Details?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes Submit it",
                            cancelButtonText: "Cancel..!",
                            closeOnConfirm: false,
                            closeOnCancel: false
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    $scope.getEmailsendingConfirmation();
                                    var year = "";

                                    year = $scope.academic_year;

                                    $scope.qualified_class = $scope.promotedtype;


                                    var data = {
                                        "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                                        "ACSTC_TCNO": $scope.usr.astC_TCNO,
                                        "ACSTC_TCApplicationDate": new Date($scope.applicationdate).toDateString(),
                                        "ACSTC_LeavingReason": $scope.leaving_reason,
                                        "ACSTC_TCDate": new Date($scope.tc_date).toDateString(),
                                        "ACSTC_TCIssueDate": new Date($scope.tc_date).toDateString(),
                                        "ACSTC_LastAttendedDate": new Date($scope.last_date_attendance).toDateString(),
                                        "ACSTC_WorkingDays": parseInt($scope.no_school_days),
                                        "ACSTC_AttendedDays": parseInt($scope.no_attended_days),
                                        "ACSTC_PromotionDate": $scope.promotionDate,
                                        "ACSTC_MediumOfINStruction": $scope.med_instruction,
                                        "ACSTC_LanguageStudied": $scope.language_study,
                                        "ACSTC_ElectivesStudied": $scope.elective_study,
                                        "ACSTC_Scholarship": $scope.scholarship,
                                        "ACSTC_MedicallyExam": $scope.is_medically_examined,
                                        "ACSTC_FeePaid": $scope.paid_fees,
                                        "ACSTC_FeeConcession": $scope.fees_concession,
                                        "ACSTC_LastExamDetails": $scope.last_exam_detail,
                                        "ACSTC_Result": $scope.results,
                                        "ACSTC_ResultDetails": $scope.status,
                                        "ACSTC_Qual_PromotionFlag": $scope.promotion,
                                        "ACSTC_Qual_Course": $scope.qualified_class,
                                        "ACSTC_NCCDetails": $scope.ncc,
                                        "ACSTC_ExtraActivities": $scope.extra,
                                        "ACSTC_Conduct": $scope.conduct,
                                        "ACSTC_Remarks": $scope.remarks,
                                        "ACSTC_TemporaryFlag": tempchk,
                                        "ACSTC_ActiveFlag": $scope.user.status,
                                        "Fee_Due_Amnt": $scope.fees_due,
                                        "Library_Due_Amnt": $scope.lib_due_trans,
                                        "Store_Canteen_Due": $scope.store_tran_due,
                                        "PDA_Due": $scope.pda_due,
                                        "Email_flag": $scope.Email_flag,
                                        "transnumconfigsettings": $scope.tcnumbering,
                                        "allorindividual": $scope.Admnoallind,
                                        "ASMAY_Id": $scope.ASMAY_Id
                                    };

                                    apiService.create("CollegeStudenttctransaction/savetc", data)
                                        .then(function (promise) {

                                            if (promise.tcflagexists === "Exists") {
                                                swal('TC Number Already Exists', 'Please Enter Different TC Number');
                                            }
                                            else if (promise.returnval === true) {
                                                swal('Record Saved Successfully');
                                                $state.reload();
                                                if (promise.email_flag === 'sent') {
                                                    alert('Mail Sent Successfully');
                                                    $state.reload();
                                                }
                                                else if (promise.email_flag === 'sms_sent') {
                                                    alert('SMS Sent Successfully');
                                                    $state.reload();
                                                }
                                                else {
                                                    // alert('Mail Not Sent', 'Failed');
                                                }
                                                // $state.reload();
                                            }

                                            else {
                                                swal('Record Already Exists/Failed to Save');
                                            }
                                        });
                                }
                                else {
                                    swal("Cancelled");
                                }
                            });
                    }
                }
                //when fee due is equal to zero no conditions to check
                else {

                    swal({
                        title: "Are you sure?",
                        text: "Do you want to submit the details?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes Submit it",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },


                        function (isConfirm) {
                            if (isConfirm) {

                                $scope.getEmailsendingConfirmation();
                                var year = "";

                                year = $scope.academic_year;


                                $scope.qualified_class = $scope.promotedtype;

                                var data = {
                                    "AMCST_Id": $scope.AMCST_Id.amcsT_Id,
                                    "ACSTC_TCNO": $scope.usr.astC_TCNO,
                                    "ACSTC_TCApplicationDate": new Date($scope.applicationdate).toDateString(),
                                    "ACSTC_LeavingReason": $scope.leaving_reason,
                                    "ACSTC_TCDate": new Date($scope.tc_date).toDateString(),
                                    "ACSTC_TCIssueDate": new Date($scope.tc_date).toDateString(),
                                    "ACSTC_LastAttendedDate": new Date($scope.last_date_attendance).toDateString(),
                                    "ACSTC_WorkingDays": parseInt($scope.no_school_days),
                                    "ACSTC_AttendedDays": parseInt($scope.no_attended_days),
                                    "ACSTC_PromotionDate": $scope.promotionDate,
                                    "ACSTC_MediumOfINStruction": $scope.med_instruction,
                                    "ACSTC_LanguageStudied": $scope.language_study,
                                    "ACSTC_ElectivesStudied": $scope.elective_study,
                                    "ACSTC_Scholarship": $scope.scholarship,
                                    "ACSTC_MedicallyExam": $scope.is_medically_examined,
                                    "transnumconfigsettings": $scope.tcnumbering,
                                    "ACSTC_FeePaid": $scope.paid_fees,
                                    "ACSTC_FeeConcession": $scope.fees_concession,
                                    "ACSTC_LastExamDetails": $scope.last_exam_detail,
                                    "ACSTC_Result": $scope.results,
                                    "ACSTC_ResultDetails": $scope.status,
                                    "ACSTC_Qual_PromotionFlag": $scope.promotion,
                                    "ACSTC_Qual_Course": $scope.qualified_class,
                                    "ACSTC_NCCDetails": $scope.ncc,
                                    "ACSTC_ExtraActivities": $scope.extra,
                                    "ACSTC_Conduct": $scope.conduct,
                                    "ACSTC_Remarks": $scope.remarks,
                                    "ACSTC_TemporaryFlag": tempchk,
                                    "ACSTC_ActiveFlag": $scope.user.status,
                                    "Fee_Due_Amnt": $scope.fees_due,
                                    "Library_Due_Amnt": $scope.lib_due_trans,
                                    "Store_Canteen_Due": $scope.store_tran_due,
                                    "PDA_Due": $scope.pda_due,
                                    "Email_flag": $scope.Email_flag,
                                    "allorindividual": $scope.Admnoallind,
                                    "ASMAY_Id": $scope.ASMAY_Id
                                };


                                apiService.create("CollegeStudenttctransaction/savetc", data)
                                    .then(function (promise) {

                                        if (promise.tcflagexists === "Exists") {
                                            swal('TC Number Already Exists', 'Please Enter Different TC Number');
                                        } else if (promise.returnval === true) {
                                            swal('Record Saved Successfully');
                                            $state.reload();
                                            if (promise.email_flag === 'sent') {
                                                alert('Mail Sent Successfully');
                                                $state.reload();
                                            } else if (promise.email_flag === 'sms_sent') {
                                                alert('SMS Sent Successfully');
                                                $state.reload();
                                            }
                                            else {
                                                //dd
                                            }
                                        }
                                        else {
                                            swal('Record Already Exists/Failed to Save');
                                        }
                                    });
                            }
                            else {
                                swal("Cancelled");
                            }
                        }
                    );
                }
            }
            else {
                swal("Please Enter Details in the  Previous Tabs");
                $scope.submitted1 = true;
                $scope.submitted2 = true;
                $scope.submitted4 = true;
            }
        };

        $scope.getEmailsendingConfirmation = function () {

            var answer = confirm("Do you want to send the Email?");
            if (answer) {
                $scope.Email_flag = "Yes";
            }
            else {
                $scope.Email_flag = "No";
            }
        };

        $scope.checkErr = function (applicationdate, tc_issue_date) {
            $scope.errMessage = '';
            //var curDate = new Date();
            if (new Date(applicationdate) > new Date(tc_issue_date)) {
                $scope.errMessage = 'Issue Date should be greater than the Application Date';
                return false;
            }
        };

        $scope.checkErr_TC = function (applicationdate, tc_date) {
            $scope.errMessage_tc = '';
            //var curDate = new Date();
            if (new Date(applicationdate) > new Date(tc_date)) {
                $scope.errMessage_tc = 'TC Date should be greater than the Application Date';
                return false;
            }
        };

        $scope.checkErr_Promo = function (Admission_Date, date_of_promotion, last_date_attendance) {
            $scope.errMessage_promo = '';
            //var curDate = new Date();

            if (new Date(Admission_Date) > new Date(date_of_promotion)) {
                $scope.errMessage_promo = 'Promotion Date should be greater than the Admission Date';
                return false;
            }
            if (new Date(date_of_promotion) > new Date(last_date_attendance)) {
                $scope.errMessage_promo = 'Promotion Date should be lesser than the Last Day Attendance';
                return false;
            }
        };

        $scope.checkErr_last_attend = function (last_date_attendance, Admission_Date, tc_date) {
            $scope.errMessage_last_day = '';
            //var curDate = new Date();

            if (new Date(last_date_attendance) > new Date(tc_date)) {
                $scope.errMessage_last_day = 'Last Date of Attendance should  be lesser than the TC Date';
                return false;
            }
            if (new Date(Admission_Date) > new Date(last_date_attendance)) {
                $scope.errMessage_last_day = 'Last Date of Attendance should be greater than the Admission Date';
                return false;
            }
        };

        $scope.checkErr_TC_apppli = function (applicationdate, Admission_Date) {
            $scope.errMessage_appli = '';
            //var curDate = new Date();

            if (new Date(Admission_Date) > new Date(applicationdate)) {
                $scope.errMessage_appli = 'TC Application Date should be greater than the Admission Date';
                return false;
            }
        };

        $scope.attdays = true;

        $scope.chk_No_school_days = function (no_school_days) {

            $scope.errMessage_No_school_day = '';
            no_school_days = parseInt(no_school_days);

            if (no_school_days > 365) {
                $scope.errMessage_No_school_day = 'Number Of School Days should  be lesser than or Equal to 365';
                return false;
            }
            else if (no_school_days === 0) {
                $scope.errMessage_No_school_day = 'Number Of School Days should be greater than 0';
                return false;
            }
            else {
                $scope.attdays = false;
                if ($scope.no_school_days > 0 && $scope.no_attended_days > 0) {
                    $scope.chk_school_days($scope.no_school_days, $scope.no_attended_days)
                }
            }
        };

        $scope.chk_school_days = function (no_school_days, no_attended_days) {
            $scope.errMessage_school_day = '';
            no_attended_days = parseInt(no_attended_days);
            no_school_days = parseInt(no_school_days);

            if (no_attended_days > no_school_days) {
                $scope.errMessage_school_day = 'Number Of Attended Days should be lesser than the Number of School Days';
                return false;
            }
        };

        $scope.pagesrecord = {};
        $scope.usr12 = [];



        //Clear Functions
        $scope.Clear_stu_details = function () {
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.stu_details = true;
            $scope.stu_pers = true;
            $scope.due = true;
            $scope.other_details = true;
            $scope.med_instruction = null;

            if ($scope.language_req === true) {
                //   $scope.language_study = null;
            } else {
                $scope.language_study = null;
            }
            if ($scope.elective_req === true) {
                // $scope.elective_study = null;
            } else {
                $scope.elective_study = null;
            }
            $scope.scholarship = null;
            $scope.is_medically_examined = null;
        };

        $scope.Clear_tc_details = function () {

            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.errMessage = "";
            $scope.errMessage_tc = "";
            $scope.errMessage_promo = "";
            $scope.errMessage_appli = "";
            $scope.errMessage_school_day = "";
            $scope.errMessage_No_school_day = "";
            $scope.errMessage_last_day = "";
            if ($scope.tc_dis_flag === true) {
                //$scope.tc_No = null;
            } else {
                $scope.usr.astC_TCNO = null;
            }

            // $scope.applicationdate = null;
            $scope.tc_issue_date = null;
            $scope.leaving_reason = null;
            // $scope.tc_date = null;
            // $scope.last_date_attendance = null;

            if ($scope.noscatt === true) {
                //$scope.no_school_days = null;
            } else {
                $scope.no_school_days = null;
            }
            if ($scope.attdays === true) {
                // $scope.no_attended_days = null;
            } else {
                $scope.no_attended_days = null;
            }

            // $scope.Admission_Date = null;
            $scope.date_of_promotion = null;
            $scope.last_class_studied = null;

            //$scope.academic_year = null;
            //$scope.classname = null;
            //$scope.sectionname = null;
            // $scope.details_flag = false;
            $scope.attdays = true;
        };

        $scope.Clear_other_details = function () {
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();

            //  $scope.paid_fees = null;

            $scope.fees_concession = null;
            $scope.last_exam_detail = null;
            $scope.results = null;
            $scope.status = null;
            $scope.promotion = null;
            $scope.qualified_class = null;
            $scope.ncc = null;
            $scope.extra = null;
            $scope.conduct = null;
            $scope.remarks = null;

            $scope.promotedtype = null;
            $scope.qualified_class = null;
            $scope.promoted2 = false;

            // $scope.academic_year = null;
            //  $scope.classname = null;
            //  $scope.sectionname = null;
            // $scope.details_flag = false;
        };

        $scope.Clear_due_details = function () {
            $state.reload();
            $scope.stu_details = true;
            $scope.stu_pers = true;
            $scope.due = true;
            $scope.other_details = true;
            $scope.pda_due = null;
            $scope.store_tran_due = null;
            $scope.lib_due_trans = null;
            $scope.fees_due = null;
        };

        $scope.Clear_details = function () {
            //$scope.AMST_Id = "";
            $scope.class_studying = "";
            $scope.admission_year = "";
            $scope.tc.regno = null;
            $scope.tc.admno = null;
        };

        $scope.Clear_all_details = function () {
            $state.reload();

        };

        $scope.chklength = function (med_instruction) {
            // swal("hi");
            if ($scope.med_instruction.length === 20)
                $scope.message_flag = false;
        };

        $scope.chklength_lan = function (language_study) {
            // swal("hi");
            if ($scope.language_study.length === 30)
                $scope.message_flag_lang = false;
        };

        //for qualification to next class
        $scope.promotedtype1 = function () {
            if ($scope.promotedtype === "Granted" || $scope.promotedtype === "Promoted") {
                $scope.promoted2 = true;
            }
            else {
                $scope.promoted2 = false;
            }
        };
    }

})();