(function () {
    'use strict';
    angular.module('app').controller('StudenttcController', StudenttcController)

    StudenttcController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache']
    function StudenttcController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $route, $q, superCache) {

        $scope.usr = {};
        $scope.tcdata = [];
        var date = new Date();
        var nextYear = date.getFullYear() + 1;
        var currYear = date.getFullYear();
        $scope.acdYear = currYear + "-" + nextYear;
        $scope.date1 = new Date();
        $scope.tcnumbering = [];
        $scope.ASTC_Id = "";
        $scope.Tc_payment = "";
        $scope.LibraryTc_payment = "";
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
        $scope.rommanclass1 = ['XI', 'XII'];
        $scope.maxapplicationdate = new Date();
        $scope.paymentTc = 1;
        $scope.libraryTc = 1;
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
            $scope.admissioncongigurationList = [];

            var year = $scope.academic_year = 0;
            var cls_name = $scope.classname = 0;
            var sec_name = $scope.sectionname = 0;
            var data = {
                "ASMAY_Id": year,
                "ASMCL_Id": cls_name,
                "ASMS_Id": sec_name,
                "flag": 'S'
            };

            apiService.create("StudentTC/getstudent_name_list", data).then(function (promise) {

                if (promise.admissioncongigurationList !== null && promise.admissioncongigurationList.length > 0) {
                    if (parseInt(promise.admissioncongigurationList[0].asC_AdmNo_RegNo_RollNo_DefaultFlag) === 3) {
                        $scope.user.status = 'S';
                        $scope.user.admno_name = 'A';
                    }
                    else if (parseInt(promise.admissioncongigurationList[0].asC_AdmNo_RegNo_RollNo_DefaultFlag) === 1) {
                        $scope.user.status = 'S';
                        $scope.user.admno_name = 'N';
                    }
                    else {
                        $scope.user.status = 'S';
                        $scope.user.admno_name = 'N';
                    }
                }

                $scope.paymentTc = promise.admissioncongigurationList[0].admC_TCAllowBalanceFlg;
                $scope.libraryTc = promise.admissioncongigurationList[0].asC_LibraryAllowBalanceFlg;
                $scope.adm_number_flag = true;

                var transnumconfig = promise.admTransNumSetting;
                localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));
                var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
                if (transnumconfigsettings !== null || transnumconfigsettings.length > 0) {
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
                    swal("Please Map The Transaction Numbering For TC Number Generation i.e., Auto/ Manual");
                }

                $scope.academicList = promise.academicList;
                $scope.currentYear = promise.currentYear;

                var name = "";
                for (var i1 = 0; i1 < $scope.academicList.length; i1++) {
                    name = $scope.academicList[i1].asmaY_Id;
                    for (var j = 0; j < $scope.currentYear.length; j++) {
                        if (parseInt(name) === parseInt($scope.currentYear[j].asmaY_Id)) {
                            $scope.academicList[i1].Selected = true;
                            $scope.academic_year = $scope.currentYear[j].asmaY_Id;

                            $scope.yearname = $scope.currentYear[j].asmaY_Year;
                            $scope.yearname1 = $scope.yearname.split("-");
                            $scope.lblyearr = $scope.yearname1[1];
                        }
                    }
                }

                //-------------------------------------------------------------------------

                //------------payment type
                $scope.Tc_payment = promise.tcpermanentpayment[0].admC_TCAllowBalanceFlg;
                $scope.LibraryTc_payment = promise.tcpermanentpayment[0].asC_LibraryAllowBalanceFlg;

                $scope.Qualifiedclass = promise.qualifiedclass;
                $scope.allclassname = promise.classlistnew;

                angular.forEach($scope.allclassname, function (xy) {
                    var classname = xy.asmcL_ClassName.toUpperCase();
                    angular.forEach($scope.rommanclass, function (z) {
                        if (classname === z) {
                            xy.asmcL_ClassName = "Std " + xy.asmcL_ClassName;
                        }
                    });

                    angular.forEach($scope.rommanclass1, function (z1) {
                        if (classname === z1) {
                            xy.asmcL_ClassName = "Class " + xy.asmcL_ClassName;
                        }
                    });
                });
            });
        };

        $scope.onsectionchange = function () {
            var year = "";
            var cls_name = "";
            var sec_name = "";
            if ($scope.academic_year !== null && $scope.classname === undefined && $scope.sectionname === undefined) {
                year = $scope.academic_year;
                cls_name = $scope.classname = 0;
                sec_name = $scope.sectionname = 0;
            }
            else if ($scope.academic_year !== null && $scope.classname !== null && $scope.sectionname === undefined) {
                year = $scope.academic_year;
                cls_name = $scope.classname;
                sec_name = $scope.sectionname = 0;
            }
            else {
                year = $scope.academic_year;
                cls_name = $scope.classname;
                sec_name = $scope.sectionname;
            }

            var data = {
                "ASMAY_Id": year,
                "ASMCL_Id": cls_name,
                "ASMS_Id": sec_name,
                "adm_num_flag": $scope.user.admno_name,
                "flag": $scope.user.status
            };
            apiService.create("StudentTC/getstudent_name_list", data).
                then(function (promise) {

                    if (promise.studentlist !== null) {
                        if (promise.studentlist.length > 0) {
                            $scope.studentname = promise.studentlist;
                            $scope.adm_number_flag = true;
                            $scope.stud_name = false;
                            $scope.class_name = false;
                            $scope.section_name = false;
                        }
                        else {
                            swal("Student is not Available");
                            $scope.studentname = "";
                            $scope.class_name = "";
                            $scope.section_name = "";
                            $scope.Amc_Name = "";
                            $scope.ASMAY_Year = "";
                            $scope.Reg_No = "";
                            $scope.Adm_No = "";
                            $scope.Stu_Img = "";
                            $scope.tab_hide = false;
                            $scope.details_flag = false;
                            $scope.adm_number_flag = false;
                            $scope.stud_name = true;
                        }
                    }

                    else {
                        swal("Student is not Available");
                        $scope.studentname = "";
                        $scope.class_name = "";
                        $scope.section_name = "";
                        $scope.Amc_Name = "";
                        $scope.ASMAY_Year = "";
                        $scope.Reg_No = "";
                        $scope.Adm_No = "";
                        $scope.Stu_Img = "";
                        $scope.tab_hide = false;
                        $scope.adm_number_flag = false;
                        $scope.details_flag = false;
                        $scope.stud_name = true;
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

        //on student name change//
        $scope.onstudentnamechange = function () {

            $scope.tab_hide = false;
            $scope.details_flag = false;

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
            $scope.pic_show = false;
            $scope.stu_details = true;
            $scope.stu_pers = true;
            $scope.due = true;
            $scope.other_details = true;
            $scope.cls_section_name = null;
            $scope.getapprovalmasterdetails = [];
            $scope.getapprovalresultdetails = [];
            var year = "";
            if ($scope.Admnoallind === 'Indi') {
                year = $scope.academic_year;
            }
            else {
                year = $scope.academic_year;
            }
            var studentname = {

                "AMST_Id": $scope.AMST_Id.amsT_Id,
                "Status_flag": $scope.user.status,
                "ASMAY_Id": year,
                "allorindividual": $scope.Admnoallind,
                "adm_num_flag": $scope.user.admno_name
            };

            $scope.class_flag = false;
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


            apiService.create("StudentTC/gettcdetails", studentname).then(function (promise) {

                if (promise.studentListById !== null && promise.studentListById.length > 0) {

                    $scope.studentDetails = promise.studentListById;

                    angular.forEach($scope.studentDetails, function (dd) {
                        dd.feetobepaid = promise.feetobepaid;
                    });
                    angular.forEach($scope.studentDetails, function (value, key) {
                        $scope.fees_due = value.feetobepaid;
                        if ($scope.fees_due > 0) {
                            $scope.paid_fees = "No";
                        }
                        else {
                            $scope.paid_fees = "Yes";
                        }
                    });

                    $scope.count_issuebooks = promise.count_issuebooks;
                    if ($scope.count_issuebooks !== null && $scope.count_issuebooks.length > 0) {
                        $scope.lib_due_trans = promise.count_issuebooks.length;
                    } else {
                        $scope.lib_due_trans = 0;
                    }


                    if ($scope.paymentTc == 1 && $scope.libraryTc == 1) {
                        if ($scope.fees_due > 0 && $scope.lib_due_trans > 0) {
                            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due and " + $scope.lib_due_trans + " Books Pending.. So You Can Not Generate TC Untill Fee Due Is Clear");
                        }
                        else if ($scope.fees_due > 0) {
                            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due! So You Can Not Generate TC Untill Fee Due Is Clear");
                        }
                        else if ($scope.lib_due_trans > 0) {
                            swal(+ $scope.lib_due_trans + "Books Pending ! So You Can Not Generate TC Untill Fee Due Is Clear");
                        }
                        else {
                            $scope.myTabIndex = 0;
                            console.log(promise.studentListById);

                            $scope.Stu_Img = promise.studentListById[0].AMST_Photoname;



                            $scope.class_name = promise.studentListById[0].ASMCL_ClassName;

                            $scope.section_name = promise.studentListById[0].ASMC_SectionName;

                            $scope.cls_section_name = $scope.class_name + ':' + $scope.section_name;

                            $scope.due_details_flag = true;
                            $scope.save_flag = true;
                            $scope.pic_show = true;
                            $scope.details_flag = true;



                            angular.forEach($scope.studentDetails, function (value, key) {
                                if (value.ASTC_PromotionDate !== null && value.ASTC_PromotionDate !== "" && value.ASTC_PromotionDate !== undefined) {
                                    $scope.date_of_promotion = new Date(value.ASTC_PromotionDate);
                                }
                                else {
                                    $scope.date_of_promotion = new Date();
                                }
                                if (value.ASTC_TCApplicationDate !== null && value.ASTC_TCApplicationDate !== undefined && value.ASTC_TCApplicationDate !== "") {
                                    $scope.applicationdate = new Date(value.ASTC_TCApplicationDate);
                                }
                                else {
                                    $scope.applicationdate = new Date();
                                }
                                if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                    $scope.tc_issue_date = new Date(value.ASTC_TCIssueDate);
                                }
                                else {
                                    $scope.tc_issue_date = new Date();
                                }
                                if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                    $scope.tc_date = new Date(value.ASTC_TCIssueDate);
                                }
                                else {
                                    $scope.tc_date = new Date();
                                }

                                if (value.ASTC_TCDate !== null && value.ASTC_TCDate !== undefined && value.ASTC_TCDate !== "") {
                                    $scope.tcleft_date = new Date(value.ASTC_TCDate);
                                }
                                else {
                                    $scope.tcleft_date = new Date();
                                }
                                if (value.ASTC_LastAttendedDate !== null && value.ASTC_LastAttendedDate !== undefined && value.ASTC_LastAttendedDate !== "") {
                                    $scope.last_date_attendance = new Date(value.ASTC_LastAttendedDate);
                                }
                                else {
                                    $scope.last_date_attendance = new Date();
                                }

                                $scope.studentnamed = value.student;
                                $scope.Admission_Date = value.AMST_Date;
                                $scope.fathername = value.father;
                                $scope.category = value.IMC_CasteName;
                                $scope.Birth_Date = value.AMST_DOB;
                                $scope.Birth_Date_words = value.AMST_DOB_Words;
                                $scope.stu_ifsc_code = value.AMST_StuBankIFSC_Code;
                                $scope.stu_birth_place = value.AMST_BirthPlace;
                                $scope.stu_birth_cer_no = value.AMST_BirthCertNO;

                                $scope.stu_caste_cer_no = value.AMST_StuCasteCertiNo;
                                $scope.stu_age = value.PASR_Age;
                                $scope.stu_gender = value.AMST_Sex;

                                $scope.stu_mother_tongue = value.AMST_MotherTongue;
                                $scope.stu_nationality = value.AMST_Nationality;
                                $scope.stu_mobile = value.AMST_MobileNo;
                                $scope.stu_email = value.AMST_emailId;

                                $scope.stu_aadhar_no = value.AMST_AadharNo;
                                $scope.stu_blood_group = value.AMST_BloodGroup;
                                $scope.stu_bank_account_no = value.AMST_StuBankAccNo;
                                $scope.stu_bpl_card_no = value.AMST_BPLCardNo;
                                $scope.ASMAY_Year = value.ASMAY_Year;

                                $scope.Reg_No = value.AMST_RegistrationNo;
                                $scope.Adm_No = value.admno;
                                $scope.Amc_Name = value.AMC_Name;
                                $scope.med_instruction = value.ASTC_MediumOfINStruction;
                                $scope.scholarship = value.ASTC_Scholarship;
                                $scope.is_medically_examined = value.ASTC_MedicallyExam;
                                $scope.usr.astC_TCNO = value.ASTC_TCNO;
                                $scope.leaving_reason = value.ASTC_LeavingReason;
                                $scope.no_school_days = value.ASTC_WorkingDays;
                                $scope.no_attended_days = value.ASTC_AttendedDays;
                                $scope.fees_concession = value.ASTC_FeeConcession;
                                $scope.last_exam_detail = value.ASTC_LastExamDetails;
                                $scope.results = value.ASTC_Result;
                                $scope.status = value.ASTC_ResultDetails;
                                $scope.promotion = value.ASTC_Qual_PromotionFlag;
                                $scope.promotedcheck = value.ASTC_Qual_Class;
                                $scope.astC_STSNO = value.stsno;

                                var str = $scope.promotedcheck;
                                if (str !== null && str !== undefined && str !== "" && str !== "Granted On Trial") {
                                    var res = str.split(" ", 1);
                                    var resall = str.split(" ");
                                    if (res[0] === "Granted" || res[0] === "Promoted") {
                                        $scope.promotedtype = res[0];
                                        var res2 = resall[2];
                                        var res3 = resall[3];
                                        var res4 = resall[4];

                                        if (res3 !== undefined && res3 !== null && res3 !== "") {
                                            $scope.qualified_class = res2 + ' ' + res3;
                                        } else {
                                            if (res2 !== undefined && res2 !== null && res2 !== "") {
                                                $scope.qualified_class = res2;
                                            } else {
                                                $scope.qualified_class = "";
                                            }
                                        }
                                        $scope.promoted2 = true;
                                    }
                                    else {
                                        if (str === "Not Applicable") {
                                            $scope.promotedtype = str;
                                            $scope.qualified_class = "";
                                        }
                                        else {
                                            $scope.promotedtype = str;
                                            $scope.qualified_class = "";
                                        }
                                    }
                                }
                                else {
                                    $scope.qualified_class = value.ASTC_Qual_Class;
                                }

                                //get_elective_subjects                       
                                if (promise.get_elective_subjects !== null && promise.get_elective_subjects.length > 0) {
                                    for (var i = 0; i < promise.get_elective_subjects.length; i++) {
                                        if (i === 0) {
                                            subjectname1 = promise.get_elective_subjects[i].subjectname;
                                        }
                                        else {
                                            subjectname1 = subjectname1 + ',' + promise.get_elective_subjects[i].subjectname;
                                        }
                                    }
                                    $scope.elective_study = subjectname1;
                                    $scope.elective_req = true;
                                }
                                else {
                                    $scope.elective_study = value.ASTC_ElectivesStudied;
                                    $scope.elective_req = false;
                                }

                                //language
                                if (promise.get_elective_subjects_language !== null && promise.get_elective_subjects_language.length > 0) {
                                    for (var j = 0; j < promise.get_elective_subjects_language.length; j++) {
                                        if (j === 0) {
                                            subjectname2 = promise.get_elective_subjects_language[j].subjectname;
                                        }
                                        else {
                                            subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_language[j].subjectname;
                                        }
                                    }
                                    $scope.language_study = subjectname2;
                                    $scope.language_req = true;
                                }
                                else {
                                    $scope.language_study = value.ASTC_LanguageStudied;
                                    $scope.language_req = false;
                                }

                                // All Subjects 
                                if (promise.get_elective_subjects_common !== null && promise.get_elective_subjects_common.length > 0) {
                                    for (var j1 = 0; j1 < promise.get_elective_subjects_common.length; j1++) {
                                        if (j1 === 0) {
                                            subjectname2 = promise.get_elective_subjects_common[j1].subjectname;
                                        }
                                        else {
                                            subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_common[j1].subjectname;
                                        }
                                    }
                                    $scope.subjects_study = subjectname2;
                                    $scope.subjets_req = true;
                                } else {
                                    $scope.subjects_study = value.ASTC_SubjectsStudied;
                                    $scope.subjets_req = false;
                                }

                                if ($scope.user.status === "S" || $scope.user.status === "D") {
                                    if (promise.getexamdetails !== null && promise.getexamdetails.length > 0) {
                                        $scope.last_exam_detail = promise.getexamdetails[0].EME_ExamName;
                                        $scope.results = promise.getexamdetails[0].ESTMP_Result;
                                        if ($scope.results.toUpperCase() === "PASS") {
                                            $scope.status = "Yes";
                                            $scope.promotion = "Yes";
                                        } else {
                                            $scope.status = "No";
                                            $scope.promotion = "No";
                                        }
                                    }
                                }

                                if ($scope.last_exam_detail !== null && $scope.last_exam_detail !== undefined && $scope.last_exam_detail !== "") {
                                    $scope.examdisable = true;
                                } else {
                                    $scope.examdisable = false;
                                }

                                $scope.ncc = value.ASTC_NCCDetails;
                                $scope.extra = value.ASTC_ExtraActivities;
                                $scope.conduct = value.ASTC_Conduct;
                                $scope.remarks = value.ASTC_Remarks;

                                if (promise.getjoineddetails !== null && promise.getjoineddetails.length > 0) {
                                    $scope.joinedclassname = promise.getjoineddetails[0].asmcL_ClassName;
                                }
                            });

                            if ($scope.user.status === "S" || $scope.user.status === "D") {

                                $scope.no_school_days = promise.countclass;

                                if ($scope.no_school_days === null) {
                                    $scope.noscatt = false;
                                } else {
                                    $scope.noscatt = true;
                                }

                                $scope.no_attended_days = promise.attclasscount;

                                if ($scope.no_attended_days === null) {
                                    $scope.attdays = false;
                                } else {
                                    $scope.attdays = true;
                                }
                            } else {

                                if ($scope.no_school_days === null) {
                                    $scope.noscatt = false;
                                } else {
                                    $scope.noscatt = true;
                                }

                                if ($scope.no_attended_days === null) {
                                    $scope.attdays = false;
                                } else {
                                    $scope.attdays = true;
                                }
                            }

                            if (promise.todateatt !== null) {
                                $scope.last_date_attendance = new Date(promise.todateatt);
                            }
                            else {
                                $scope.last_date_attendance = new Date();
                            }

                            $scope.count_issuebooks = promise.count_issuebooks;
                            if ($scope.count_issuebooks !== null && $scope.count_issuebooks.length > 0) {
                                $scope.lib_due_trans = promise.count_issuebooks.length;
                            } else {
                                $scope.lib_due_trans = 0;
                            }


                            if (promise.pdadata !== null && promise.pdadata.length > 0) {
                                $scope.pdadata = promise.pdadata;
                                $scope.pda_due = promise.pdadata[0].pdaS_CBStudentDue;
                            } else {
                                $scope.pda_due = 0;
                            }

                            $scope.getconcession = promise.getconcession;
                            if ($scope.getconcession !== null && $scope.getconcession.length > 0) {
                                $scope.fees_concession = $scope.getconcession[0].fmcC_ConcessionName;
                            }

                            $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                            $scope.getapprovalmasterdetails = promise.getapprovalmasterdetails;
                            $scope.getstudentapplieddetails = promise.getstudentapplieddetails;
                            $scope.getapprovalresultdetails = promise.getapprovalresultdetails;

                            $scope.tcgeneratedornot = promise.tcgeneratedornot;

                            // ***************** CHECKING APPROVAL PROCESS *********************** //

                            if ($scope.tcgeneratedornot === "") {
                                $scope.checktcbalanceflag = true;
                                $scope.checktcbalanceflagstatus = "";
                                $scope.checktcsaveflagstatus = "";

                                if ($scope.getapprovalmasterdetails !== null && $scope.getapprovalmasterdetails.length > 0) {
                                    $scope.allowapproval = $scope.getapprovalmasterdetails[0].acertapP_ApprovaReqlFlg;
                                    if ($scope.allowapproval === true) {
                                        $scope.checktcbalanceflag = false;
                                        if ($scope.getstudentapplieddetails !== null && $scope.getstudentapplieddetails.length > 0) {
                                            $scope.requeststatus = "";
                                            if ($scope.getapprovalresultdetails !== null && $scope.getapprovalresultdetails.length > 0) {
                                                $scope.requeststatus = $scope.getapprovalresultdetails[0].ascaP_Status;
                                                if ($scope.requeststatus !== "Approved") {
                                                    $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                    $scope.checktcsaveflagstatus = "Check";
                                                    swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                                } else {
                                                    $scope.checktcbalanceflag = false;
                                                    $scope.checktcbalanceflagstatus = "Dispaly";
                                                    $scope.checktcsaveflagstatus = "Not Check";
                                                }
                                            } else {
                                                $scope.requeststatus = $scope.getstudentapplieddetails[0].ascA_Status;
                                                $scope.checktcbalanceflag = false;
                                                $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                $scope.checktcsaveflagstatus = "Not Check";
                                                swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                            }
                                        } else {
                                            $scope.checktcbalanceflag = false;
                                            $scope.checktcbalanceflagstatus = "Not Dispaly";
                                            $scope.checktcsaveflagstatus = "Not Check";
                                            swal("For Approval Process ,First Student Need To Put Request For Certificate From Portal");
                                        }
                                    } else {
                                        $scope.checktcbalanceflag = true;
                                    }
                                } else {
                                    $scope.checktcbalanceflag = true;
                                }
                            } else {
                                $scope.checktcbalanceflag = false;
                            }

                            if ($scope.checktcbalanceflag === true) {
                                if ($scope.Tc_payment === 1) {
                                    $scope.feedetails = "";
                                    $scope.pdadetails = "";
                                    $scope.libraydetails = "";

                                    if ($scope.fees_due > 0) {
                                        $scope.feedetails = "Rs " + $scope.fees_due + "/-" + "Amount Is Due In Fee!";
                                    }

                                    if ($scope.lib_due_trans > 0) {
                                        $scope.libraydetails = "Total " + $scope.lib_due_trans + " Book Not Returned By Student!";
                                    }

                                    if ($scope.pda_due > 0) {
                                        $scope.pdadetails = "Rs " + $scope.pda_due + "/-" + "Amount Is Due In PDA From Student Side!";
                                    }

                                    if ($scope.feedetails === "" && $scope.pdadetails === "" && $scope.libraydetails === "") {
                                        $scope.adm_flag = true;
                                        $scope.cat_flag = true;
                                        $scope.tab_hide = true;
                                    }
                                    else {
                                        $scope.finaldisplay = "";
                                        if ($scope.feedetails !== "") {
                                            $scope.finaldisplay = $scope.feedetails;
                                        }
                                        if ($scope.pdadetails !== "") {
                                            if ($scope.finaldisplay !== "") {
                                                $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.pdadetails;
                                            } else {
                                                $scope.finaldisplay = $scope.pdadetails;
                                            }
                                        }
                                        if ($scope.libraydetails !== "") {
                                            if ($scope.finaldisplay !== "") {
                                                $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.libraydetails;
                                            } else {
                                                $scope.finaldisplay = $scope.libraydetails;
                                            }
                                        }
                                        $scope.detailsdue = $scope.feedetails + '\n' + $scope.pdadetails + '\n' + $scope.libraydetails;
                                        swal($scope.finaldisplay + '\n' + " So You Can Not Generate TC Untill Due Should Be Clear");
                                        $scope.tab_hide = false;
                                        $scope.details_flag = false;
                                        return;
                                    }
                                } else {
                                    $scope.checktcsaveflagstatus = "Check";
                                    $scope.adm_flag = true;
                                    $scope.cat_flag = true;
                                    $scope.tab_hide = true;
                                    $scope.details_flag = true;
                                }
                            } else {
                                if ($scope.checktcbalanceflagstatus === "Not Dispaly") {
                                    $scope.tab_hide = false;
                                    $scope.details_flag = false;
                                    $scope.checktcsaveflagstatus = "Not Check";
                                } else {
                                    if ($scope.checktcbalanceflagstatus === "Dispaly") { $scope.checktcsaveflagstatus = "Not Check"; }
                                    else { $scope.checktcsaveflagstatus = "Check"; }
                                    $scope.adm_flag = true;
                                    $scope.cat_flag = true;
                                    $scope.tab_hide = true;
                                    $scope.details_flag = true;
                                }
                            }
                        }
                    }
                    else if ($scope.paymentTc == 0 && $scope.libraryTc == 1) {
                        if ($scope.fees_due > 0 && $scope.lib_due_trans > 0) {
                            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due and " + $scope.lib_due_trans + " Books Pending.. So You Can Not Generate TC Untill Fee Due Is Clear");
                        }
                        else if ($scope.lib_due_trans > 0) {
                            swal(+ $scope.lib_due_trans + "Books Pending ! So You Can Not Generate TC Untill Fee Due Is Clear");

                        }
                        else if ($scope.fees_due > 0) {
                            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due! So You Can Not Generate TC Untill Fee Due Is Clear");
                        }
                        else {
                            if ($scope.fees_due > 0 && ($scope.user.status === 'S' || $scope.user.status === 'D' || $scope.user.status === 'T')) {
                                swal("Rs " + $scope.fees_due + "/-" + "Amount is Due! So You Can Not Generate TC Untill Fee Due Is Clear");
                                //    $state.reload();
                                //}
                                //else {
                                $scope.myTabIndex = 0;
                                console.log(promise.studentListById);

                                $scope.Stu_Img = promise.studentListById[0].AMST_Photoname;



                                $scope.class_name = promise.studentListById[0].ASMCL_ClassName;

                                $scope.section_name = promise.studentListById[0].ASMC_SectionName;

                                $scope.cls_section_name = $scope.class_name + ':' + $scope.section_name;

                                $scope.due_details_flag = true;
                                $scope.save_flag = true;
                                $scope.pic_show = true;
                                $scope.details_flag = true;



                                angular.forEach($scope.studentDetails, function (value, key) {
                                    if (value.ASTC_PromotionDate !== null && value.ASTC_PromotionDate !== "" && value.ASTC_PromotionDate !== undefined) {
                                        $scope.date_of_promotion = new Date(value.ASTC_PromotionDate);
                                    }
                                    else {
                                        $scope.date_of_promotion = new Date();
                                    }
                                    if (value.ASTC_TCApplicationDate !== null && value.ASTC_TCApplicationDate !== undefined && value.ASTC_TCApplicationDate !== "") {
                                        $scope.applicationdate = new Date(value.ASTC_TCApplicationDate);
                                    }
                                    else {
                                        $scope.applicationdate = new Date();
                                    }
                                    if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                        $scope.tc_issue_date = new Date(value.ASTC_TCIssueDate);
                                    }
                                    else {
                                        $scope.tc_issue_date = new Date();
                                    }
                                    if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                        $scope.tc_date = new Date(value.ASTC_TCIssueDate);
                                    }
                                    else {
                                        $scope.tc_date = new Date();
                                    }

                                    if (value.ASTC_TCDate !== null && value.ASTC_TCDate !== undefined && value.ASTC_TCDate !== "") {
                                        $scope.tcleft_date = new Date(value.ASTC_TCDate);
                                    }
                                    else {
                                        $scope.tcleft_date = new Date();
                                    }
                                    if (value.ASTC_LastAttendedDate !== null && value.ASTC_LastAttendedDate !== undefined && value.ASTC_LastAttendedDate !== "") {
                                        $scope.last_date_attendance = new Date(value.ASTC_LastAttendedDate);
                                    }
                                    else {
                                        $scope.last_date_attendance = new Date();
                                    }

                                    $scope.studentnamed = value.student;
                                    $scope.Admission_Date = value.AMST_Date;
                                    $scope.fathername = value.father;
                                    $scope.category = value.IMC_CasteName;
                                    $scope.Birth_Date = value.AMST_DOB;
                                    $scope.Birth_Date_words = value.AMST_DOB_Words;
                                    $scope.stu_ifsc_code = value.AMST_StuBankIFSC_Code;
                                    $scope.stu_birth_place = value.AMST_BirthPlace;
                                    $scope.stu_birth_cer_no = value.AMST_BirthCertNO;

                                    $scope.stu_caste_cer_no = value.AMST_StuCasteCertiNo;
                                    $scope.stu_age = value.PASR_Age;
                                    $scope.stu_gender = value.AMST_Sex;

                                    $scope.stu_mother_tongue = value.AMST_MotherTongue;
                                    $scope.stu_nationality = value.AMST_Nationality;
                                    $scope.stu_mobile = value.AMST_MobileNo;
                                    $scope.stu_email = value.AMST_emailId;

                                    $scope.stu_aadhar_no = value.AMST_AadharNo;
                                    $scope.stu_blood_group = value.AMST_BloodGroup;
                                    $scope.stu_bank_account_no = value.AMST_StuBankAccNo;
                                    $scope.stu_bpl_card_no = value.AMST_BPLCardNo;
                                    $scope.ASMAY_Year = value.ASMAY_Year;

                                    $scope.Reg_No = value.AMST_RegistrationNo;
                                    $scope.Adm_No = value.admno;
                                    $scope.Amc_Name = value.AMC_Name;
                                    $scope.med_instruction = value.ASTC_MediumOfINStruction;
                                    $scope.scholarship = value.ASTC_Scholarship;
                                    $scope.is_medically_examined = value.ASTC_MedicallyExam;
                                    $scope.usr.astC_TCNO = value.ASTC_TCNO;
                                    $scope.leaving_reason = value.ASTC_LeavingReason;
                                    $scope.no_school_days = value.ASTC_WorkingDays;
                                    $scope.no_attended_days = value.ASTC_AttendedDays;
                                    $scope.fees_concession = value.ASTC_FeeConcession;
                                    $scope.last_exam_detail = value.ASTC_LastExamDetails;
                                    $scope.results = value.ASTC_Result;
                                    $scope.status = value.ASTC_ResultDetails;
                                    $scope.promotion = value.ASTC_Qual_PromotionFlag;
                                    $scope.promotedcheck = value.ASTC_Qual_Class;
                                    $scope.astC_STSNO = value.stsno;

                                    var str = $scope.promotedcheck;
                                    if (str !== null && str !== undefined && str !== "" && str !== "Granted On Trial") {
                                        var res = str.split(" ", 1);
                                        var resall = str.split(" ");
                                        if (res[0] === "Granted" || res[0] === "Promoted") {
                                            $scope.promotedtype = res[0];
                                            var res2 = resall[2];
                                            var res3 = resall[3];
                                            var res4 = resall[4];

                                            if (res3 !== undefined && res3 !== null && res3 !== "") {
                                                $scope.qualified_class = res2 + ' ' + res3;
                                            } else {
                                                if (res2 !== undefined && res2 !== null && res2 !== "") {
                                                    $scope.qualified_class = res2;
                                                } else {
                                                    $scope.qualified_class = "";
                                                }
                                            }
                                            $scope.promoted2 = true;
                                        }
                                        else {
                                            if (str === "Not Applicable") {
                                                $scope.promotedtype = str;
                                                $scope.qualified_class = "";
                                            }
                                            else {
                                                $scope.promotedtype = str;
                                                $scope.qualified_class = "";
                                            }
                                        }
                                    }
                                    else {
                                        $scope.qualified_class = value.ASTC_Qual_Class;
                                    }

                                    //get_elective_subjects                       
                                    if (promise.get_elective_subjects !== null && promise.get_elective_subjects.length > 0) {
                                        for (var i = 0; i < promise.get_elective_subjects.length; i++) {
                                            if (i === 0) {
                                                subjectname1 = promise.get_elective_subjects[i].subjectname;
                                            }
                                            else {
                                                subjectname1 = subjectname1 + ',' + promise.get_elective_subjects[i].subjectname;
                                            }
                                        }
                                        $scope.elective_study = subjectname1;
                                        $scope.elective_req = true;
                                    }
                                    else {
                                        $scope.elective_study = value.ASTC_ElectivesStudied;
                                        $scope.elective_req = false;
                                    }

                                    //language
                                    if (promise.get_elective_subjects_language !== null && promise.get_elective_subjects_language.length > 0) {
                                        for (var j = 0; j < promise.get_elective_subjects_language.length; j++) {
                                            if (j === 0) {
                                                subjectname2 = promise.get_elective_subjects_language[j].subjectname;
                                            }
                                            else {
                                                subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_language[j].subjectname;
                                            }
                                        }
                                        $scope.language_study = subjectname2;
                                        $scope.language_req = true;
                                    }
                                    else {
                                        $scope.language_study = value.ASTC_LanguageStudied;
                                        $scope.language_req = false;
                                    }

                                    // All Subjects 
                                    if (promise.get_elective_subjects_common !== null && promise.get_elective_subjects_common.length > 0) {
                                        for (var j1 = 0; j1 < promise.get_elective_subjects_common.length; j1++) {
                                            if (j1 === 0) {
                                                subjectname2 = promise.get_elective_subjects_common[j1].subjectname;
                                            }
                                            else {
                                                subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_common[j1].subjectname;
                                            }
                                        }
                                        $scope.subjects_study = subjectname2;
                                        $scope.subjets_req = true;
                                    } else {
                                        $scope.subjects_study = value.ASTC_SubjectsStudied;
                                        $scope.subjets_req = false;
                                    }

                                    if ($scope.user.status === "S" || $scope.user.status === "D") {
                                        if (promise.getexamdetails !== null && promise.getexamdetails.length > 0) {
                                            $scope.last_exam_detail = promise.getexamdetails[0].EME_ExamName;
                                            $scope.results = promise.getexamdetails[0].ESTMP_Result;
                                            if ($scope.results.toUpperCase() === "PASS") {
                                                $scope.status = "Yes";
                                                $scope.promotion = "Yes";
                                            } else {
                                                $scope.status = "No";
                                                $scope.promotion = "No";
                                            }
                                        }
                                    }

                                    if ($scope.last_exam_detail !== null && $scope.last_exam_detail !== undefined && $scope.last_exam_detail !== "") {
                                        $scope.examdisable = true;
                                    } else {
                                        $scope.examdisable = false;
                                    }

                                    $scope.ncc = value.ASTC_NCCDetails;
                                    $scope.extra = value.ASTC_ExtraActivities;
                                    $scope.conduct = value.ASTC_Conduct;
                                    $scope.remarks = value.ASTC_Remarks;

                                    if (promise.getjoineddetails !== null && promise.getjoineddetails.length > 0) {
                                        $scope.joinedclassname = promise.getjoineddetails[0].asmcL_ClassName;
                                    }
                                });

                                if ($scope.user.status === "S" || $scope.user.status === "D") {

                                    $scope.no_school_days = promise.countclass;

                                    if ($scope.no_school_days === null) {
                                        $scope.noscatt = false;
                                    } else {
                                        $scope.noscatt = true;
                                    }

                                    $scope.no_attended_days = promise.attclasscount;

                                    if ($scope.no_attended_days === null) {
                                        $scope.attdays = false;
                                    } else {
                                        $scope.attdays = true;
                                    }
                                } else {

                                    if ($scope.no_school_days === null) {
                                        $scope.noscatt = false;
                                    } else {
                                        $scope.noscatt = true;
                                    }

                                    if ($scope.no_attended_days === null) {
                                        $scope.attdays = false;
                                    } else {
                                        $scope.attdays = true;
                                    }
                                }

                                if (promise.todateatt !== null) {
                                    $scope.last_date_attendance = new Date(promise.todateatt);
                                }
                                else {
                                    $scope.last_date_attendance = new Date();
                                }

                                $scope.count_issuebooks = promise.count_issuebooks;
                                if ($scope.count_issuebooks !== null && $scope.count_issuebooks.length > 0) {
                                    $scope.lib_due_trans = promise.count_issuebooks.length;
                                } else {
                                    $scope.lib_due_trans = 0;
                                }


                                if (promise.pdadata !== null && promise.pdadata.length > 0) {
                                    $scope.pdadata = promise.pdadata;
                                    $scope.pda_due = promise.pdadata[0].pdaS_CBStudentDue;
                                } else {
                                    $scope.pda_due = 0;
                                }

                                $scope.getconcession = promise.getconcession;
                                if ($scope.getconcession !== null && $scope.getconcession.length > 0) {
                                    $scope.fees_concession = $scope.getconcession[0].fmcC_ConcessionName;
                                }

                                $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                                $scope.getapprovalmasterdetails = promise.getapprovalmasterdetails;
                                $scope.getstudentapplieddetails = promise.getstudentapplieddetails;
                                $scope.getapprovalresultdetails = promise.getapprovalresultdetails;

                                $scope.tcgeneratedornot = promise.tcgeneratedornot;

                                // ***************** CHECKING APPROVAL PROCESS *********************** //

                                if ($scope.tcgeneratedornot === "") {
                                    $scope.checktcbalanceflag = true;
                                    $scope.checktcbalanceflagstatus = "";
                                    $scope.checktcsaveflagstatus = "";

                                    if ($scope.getapprovalmasterdetails !== null && $scope.getapprovalmasterdetails.length > 0) {
                                        $scope.allowapproval = $scope.getapprovalmasterdetails[0].acertapP_ApprovaReqlFlg;
                                        if ($scope.allowapproval === true) {
                                            $scope.checktcbalanceflag = false;
                                            if ($scope.getstudentapplieddetails !== null && $scope.getstudentapplieddetails.length > 0) {
                                                $scope.requeststatus = "";
                                                if ($scope.getapprovalresultdetails !== null && $scope.getapprovalresultdetails.length > 0) {
                                                    $scope.requeststatus = $scope.getapprovalresultdetails[0].ascaP_Status;
                                                    if ($scope.requeststatus !== "Approved") {
                                                        $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                        $scope.checktcsaveflagstatus = "Check";
                                                        swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                                    } else {
                                                        $scope.checktcbalanceflag = false;
                                                        $scope.checktcbalanceflagstatus = "Dispaly";
                                                        $scope.checktcsaveflagstatus = "Not Check";
                                                    }
                                                } else {
                                                    $scope.requeststatus = $scope.getstudentapplieddetails[0].ascA_Status;
                                                    $scope.checktcbalanceflag = false;
                                                    $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                    $scope.checktcsaveflagstatus = "Not Check";
                                                    swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                                }
                                            } else {
                                                $scope.checktcbalanceflag = false;
                                                $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                $scope.checktcsaveflagstatus = "Not Check";
                                                swal("For Approval Process ,First Student Need To Put Request For Certificate From Portal");
                                            }
                                        } else {
                                            $scope.checktcbalanceflag = true;
                                        }
                                    } else {
                                        $scope.checktcbalanceflag = true;
                                    }
                                } else {
                                    $scope.checktcbalanceflag = false;
                                }

                                if ($scope.checktcbalanceflag === true) {
                                    if ($scope.Tc_payment === 1) {
                                        $scope.feedetails = "";
                                        $scope.pdadetails = "";
                                        $scope.libraydetails = "";

                                        if ($scope.fees_due > 0) {
                                            $scope.feedetails = "Rs " + $scope.fees_due + "/-" + "Amount Is Due In Fee!";
                                        }

                                        if ($scope.lib_due_trans > 0) {
                                            $scope.libraydetails = "Total " + $scope.lib_due_trans + " Book Not Returned By Student!";
                                        }

                                        if ($scope.pda_due > 0) {
                                            $scope.pdadetails = "Rs " + $scope.pda_due + "/-" + "Amount Is Due In PDA From Student Side!";
                                        }

                                        if ($scope.feedetails === "" && $scope.pdadetails === "" && $scope.libraydetails === "") {
                                            $scope.adm_flag = true;
                                            $scope.cat_flag = true;
                                            $scope.tab_hide = true;
                                        }
                                        else {
                                            $scope.finaldisplay = "";
                                            if ($scope.feedetails !== "") {
                                                $scope.finaldisplay = $scope.feedetails;
                                            }
                                            if ($scope.pdadetails !== "") {
                                                if ($scope.finaldisplay !== "") {
                                                    $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.pdadetails;
                                                } else {
                                                    $scope.finaldisplay = $scope.pdadetails;
                                                }
                                            }
                                            if ($scope.libraydetails !== "") {
                                                if ($scope.finaldisplay !== "") {
                                                    $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.libraydetails;
                                                } else {
                                                    $scope.finaldisplay = $scope.libraydetails;
                                                }
                                            }
                                            $scope.detailsdue = $scope.feedetails + '\n' + $scope.pdadetails + '\n' + $scope.libraydetails;
                                            swal($scope.finaldisplay + '\n' + " So You Can Not Generate TC Untill Due Should Be Clear");
                                            $scope.tab_hide = false;
                                            $scope.details_flag = false;
                                            return;
                                        }
                                    } else {
                                        $scope.checktcsaveflagstatus = "Check";
                                        $scope.adm_flag = true;
                                        $scope.cat_flag = true;
                                        $scope.tab_hide = true;
                                        $scope.details_flag = true;
                                    }
                                } else {
                                    if ($scope.checktcbalanceflagstatus === "Not Dispaly") {
                                        $scope.tab_hide = false;
                                        $scope.details_flag = false;
                                        $scope.checktcsaveflagstatus = "Not Check";
                                    } else {
                                        if ($scope.checktcbalanceflagstatus === "Dispaly") { $scope.checktcsaveflagstatus = "Not Check"; }
                                        else { $scope.checktcsaveflagstatus = "Check"; }
                                        $scope.adm_flag = true;
                                        $scope.cat_flag = true;
                                        $scope.tab_hide = true;
                                        $scope.details_flag = true;
                                    }
                                }
                            }
                        }

                    }
                    else if ($scope.paymentTc == 1 && $scope.libraryTc == 0) {
                        if ($scope.fees_due > 0 && $scope.lib_due_trans > 0) {
                            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due and " + $scope.lib_due_trans + " Books Pending.. So You Can Not Generate TC Untill Fee Due Is Clear");
                        }
                        else if ($scope.lib_due_trans > 0) {
                            swal(+ $scope.lib_due_trans + "Books Pending ! So You Can Not Generate TC Untill Fee Due Is Clear");

                        }
                        else if ($scope.fees_due > 0) {
                            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due! So You Can Not Generate TC Untill Fee Due Is Clear");
                        }
                        else {
                            if ($scope.fees_due > 0 && ($scope.user.status === 'S' || $scope.user.status === 'D' || $scope.user.status === 'T')) {
                                swal("Rs " + $scope.fees_due + "/-" + "Amount is Due! So You Can Not Generate TC Untill Fee Due Is Clear");
                                //    $state.reload();
                                //}
                                //else {
                                $scope.myTabIndex = 0;
                                console.log(promise.studentListById);

                                $scope.Stu_Img = promise.studentListById[0].AMST_Photoname;



                                $scope.class_name = promise.studentListById[0].ASMCL_ClassName;

                                $scope.section_name = promise.studentListById[0].ASMC_SectionName;

                                $scope.cls_section_name = $scope.class_name + ':' + $scope.section_name;

                                $scope.due_details_flag = true;
                                $scope.save_flag = true;
                                $scope.pic_show = true;
                                $scope.details_flag = true;



                                angular.forEach($scope.studentDetails, function (value, key) {
                                    if (value.ASTC_PromotionDate !== null && value.ASTC_PromotionDate !== "" && value.ASTC_PromotionDate !== undefined) {
                                        $scope.date_of_promotion = new Date(value.ASTC_PromotionDate);
                                    }
                                    else {
                                        $scope.date_of_promotion = new Date();
                                    }
                                    if (value.ASTC_TCApplicationDate !== null && value.ASTC_TCApplicationDate !== undefined && value.ASTC_TCApplicationDate !== "") {
                                        $scope.applicationdate = new Date(value.ASTC_TCApplicationDate);
                                    }
                                    else {
                                        $scope.applicationdate = new Date();
                                    }
                                    if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                        $scope.tc_issue_date = new Date(value.ASTC_TCIssueDate);
                                    }
                                    else {
                                        $scope.tc_issue_date = new Date();
                                    }
                                    if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                        $scope.tc_date = new Date(value.ASTC_TCIssueDate);
                                    }
                                    else {
                                        $scope.tc_date = new Date();
                                    }

                                    if (value.ASTC_TCDate !== null && value.ASTC_TCDate !== undefined && value.ASTC_TCDate !== "") {
                                        $scope.tcleft_date = new Date(value.ASTC_TCDate);
                                    }
                                    else {
                                        $scope.tcleft_date = new Date();
                                    }
                                    if (value.ASTC_LastAttendedDate !== null && value.ASTC_LastAttendedDate !== undefined && value.ASTC_LastAttendedDate !== "") {
                                        $scope.last_date_attendance = new Date(value.ASTC_LastAttendedDate);
                                    }
                                    else {
                                        $scope.last_date_attendance = new Date();
                                    }

                                    $scope.studentnamed = value.student;
                                    $scope.Admission_Date = value.AMST_Date;
                                    $scope.fathername = value.father;
                                    $scope.category = value.IMC_CasteName;
                                    $scope.Birth_Date = value.AMST_DOB;
                                    $scope.Birth_Date_words = value.AMST_DOB_Words;
                                    $scope.stu_ifsc_code = value.AMST_StuBankIFSC_Code;
                                    $scope.stu_birth_place = value.AMST_BirthPlace;
                                    $scope.stu_birth_cer_no = value.AMST_BirthCertNO;

                                    $scope.stu_caste_cer_no = value.AMST_StuCasteCertiNo;
                                    $scope.stu_age = value.PASR_Age;
                                    $scope.stu_gender = value.AMST_Sex;

                                    $scope.stu_mother_tongue = value.AMST_MotherTongue;
                                    $scope.stu_nationality = value.AMST_Nationality;
                                    $scope.stu_mobile = value.AMST_MobileNo;
                                    $scope.stu_email = value.AMST_emailId;

                                    $scope.stu_aadhar_no = value.AMST_AadharNo;
                                    $scope.stu_blood_group = value.AMST_BloodGroup;
                                    $scope.stu_bank_account_no = value.AMST_StuBankAccNo;
                                    $scope.stu_bpl_card_no = value.AMST_BPLCardNo;
                                    $scope.ASMAY_Year = value.ASMAY_Year;

                                    $scope.Reg_No = value.AMST_RegistrationNo;
                                    $scope.Adm_No = value.admno;
                                    $scope.Amc_Name = value.AMC_Name;
                                    $scope.med_instruction = value.ASTC_MediumOfINStruction;
                                    $scope.scholarship = value.ASTC_Scholarship;
                                    $scope.is_medically_examined = value.ASTC_MedicallyExam;
                                    $scope.usr.astC_TCNO = value.ASTC_TCNO;
                                    $scope.leaving_reason = value.ASTC_LeavingReason;
                                    $scope.no_school_days = value.ASTC_WorkingDays;
                                    $scope.no_attended_days = value.ASTC_AttendedDays;
                                    $scope.fees_concession = value.ASTC_FeeConcession;
                                    $scope.last_exam_detail = value.ASTC_LastExamDetails;
                                    $scope.results = value.ASTC_Result;
                                    $scope.status = value.ASTC_ResultDetails;
                                    $scope.promotion = value.ASTC_Qual_PromotionFlag;
                                    $scope.promotedcheck = value.ASTC_Qual_Class;
                                    $scope.astC_STSNO = value.stsno;

                                    var str = $scope.promotedcheck;
                                    if (str !== null && str !== undefined && str !== "" && str !== "Granted On Trial") {
                                        var res = str.split(" ", 1);
                                        var resall = str.split(" ");
                                        if (res[0] === "Granted" || res[0] === "Promoted") {
                                            $scope.promotedtype = res[0];
                                            var res2 = resall[2];
                                            var res3 = resall[3];
                                            var res4 = resall[4];

                                            if (res3 !== undefined && res3 !== null && res3 !== "") {
                                                $scope.qualified_class = res2 + ' ' + res3;
                                            } else {
                                                if (res2 !== undefined && res2 !== null && res2 !== "") {
                                                    $scope.qualified_class = res2;
                                                } else {
                                                    $scope.qualified_class = "";
                                                }
                                            }
                                            $scope.promoted2 = true;
                                        }
                                        else {
                                            if (str === "Not Applicable") {
                                                $scope.promotedtype = str;
                                                $scope.qualified_class = "";
                                            }
                                            else {
                                                $scope.promotedtype = str;
                                                $scope.qualified_class = "";
                                            }
                                        }
                                    }
                                    else {
                                        $scope.qualified_class = value.ASTC_Qual_Class;
                                    }

                                    //get_elective_subjects                       
                                    if (promise.get_elective_subjects !== null && promise.get_elective_subjects.length > 0) {
                                        for (var i = 0; i < promise.get_elective_subjects.length; i++) {
                                            if (i === 0) {
                                                subjectname1 = promise.get_elective_subjects[i].subjectname;
                                            }
                                            else {
                                                subjectname1 = subjectname1 + ',' + promise.get_elective_subjects[i].subjectname;
                                            }
                                        }
                                        $scope.elective_study = subjectname1;
                                        $scope.elective_req = true;
                                    }
                                    else {
                                        $scope.elective_study = value.ASTC_ElectivesStudied;
                                        $scope.elective_req = false;
                                    }

                                    //language
                                    if (promise.get_elective_subjects_language !== null && promise.get_elective_subjects_language.length > 0) {
                                        for (var j = 0; j < promise.get_elective_subjects_language.length; j++) {
                                            if (j === 0) {
                                                subjectname2 = promise.get_elective_subjects_language[j].subjectname;
                                            }
                                            else {
                                                subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_language[j].subjectname;
                                            }
                                        }
                                        $scope.language_study = subjectname2;
                                        $scope.language_req = true;
                                    }
                                    else {
                                        $scope.language_study = value.ASTC_LanguageStudied;
                                        $scope.language_req = false;
                                    }

                                    // All Subjects 
                                    if (promise.get_elective_subjects_common !== null && promise.get_elective_subjects_common.length > 0) {
                                        for (var j1 = 0; j1 < promise.get_elective_subjects_common.length; j1++) {
                                            if (j1 === 0) {
                                                subjectname2 = promise.get_elective_subjects_common[j1].subjectname;
                                            }
                                            else {
                                                subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_common[j1].subjectname;
                                            }
                                        }
                                        $scope.subjects_study = subjectname2;
                                        $scope.subjets_req = true;
                                    } else {
                                        $scope.subjects_study = value.ASTC_SubjectsStudied;
                                        $scope.subjets_req = false;
                                    }

                                    if ($scope.user.status === "S" || $scope.user.status === "D") {
                                        if (promise.getexamdetails !== null && promise.getexamdetails.length > 0) {
                                            $scope.last_exam_detail = promise.getexamdetails[0].EME_ExamName;
                                            $scope.results = promise.getexamdetails[0].ESTMP_Result;
                                            if ($scope.results.toUpperCase() === "PASS") {
                                                $scope.status = "Yes";
                                                $scope.promotion = "Yes";
                                            } else {
                                                $scope.status = "No";
                                                $scope.promotion = "No";
                                            }
                                        }
                                    }

                                    if ($scope.last_exam_detail !== null && $scope.last_exam_detail !== undefined && $scope.last_exam_detail !== "") {
                                        $scope.examdisable = true;
                                    } else {
                                        $scope.examdisable = false;
                                    }

                                    $scope.ncc = value.ASTC_NCCDetails;
                                    $scope.extra = value.ASTC_ExtraActivities;
                                    $scope.conduct = value.ASTC_Conduct;
                                    $scope.remarks = value.ASTC_Remarks;

                                    if (promise.getjoineddetails !== null && promise.getjoineddetails.length > 0) {
                                        $scope.joinedclassname = promise.getjoineddetails[0].asmcL_ClassName;
                                    }
                                });

                                if ($scope.user.status === "S" || $scope.user.status === "D") {

                                    $scope.no_school_days = promise.countclass;

                                    if ($scope.no_school_days === null) {
                                        $scope.noscatt = false;
                                    } else {
                                        $scope.noscatt = true;
                                    }

                                    $scope.no_attended_days = promise.attclasscount;

                                    if ($scope.no_attended_days === null) {
                                        $scope.attdays = false;
                                    } else {
                                        $scope.attdays = true;
                                    }
                                } else {

                                    if ($scope.no_school_days === null) {
                                        $scope.noscatt = false;
                                    } else {
                                        $scope.noscatt = true;
                                    }

                                    if ($scope.no_attended_days === null) {
                                        $scope.attdays = false;
                                    } else {
                                        $scope.attdays = true;
                                    }
                                }

                                if (promise.todateatt !== null) {
                                    $scope.last_date_attendance = new Date(promise.todateatt);
                                }
                                else {
                                    $scope.last_date_attendance = new Date();
                                }

                                $scope.count_issuebooks = promise.count_issuebooks;
                                if ($scope.count_issuebooks !== null && $scope.count_issuebooks.length > 0) {
                                    $scope.lib_due_trans = promise.count_issuebooks.length;
                                } else {
                                    $scope.lib_due_trans = 0;
                                }


                                if (promise.pdadata !== null && promise.pdadata.length > 0) {
                                    $scope.pdadata = promise.pdadata;
                                    $scope.pda_due = promise.pdadata[0].pdaS_CBStudentDue;
                                } else {
                                    $scope.pda_due = 0;
                                }

                                $scope.getconcession = promise.getconcession;
                                if ($scope.getconcession !== null && $scope.getconcession.length > 0) {
                                    $scope.fees_concession = $scope.getconcession[0].fmcC_ConcessionName;
                                }

                                $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                                $scope.getapprovalmasterdetails = promise.getapprovalmasterdetails;
                                $scope.getstudentapplieddetails = promise.getstudentapplieddetails;
                                $scope.getapprovalresultdetails = promise.getapprovalresultdetails;

                                $scope.tcgeneratedornot = promise.tcgeneratedornot;

                                // ***************** CHECKING APPROVAL PROCESS *********************** //

                                if ($scope.tcgeneratedornot === "") {
                                    $scope.checktcbalanceflag = true;
                                    $scope.checktcbalanceflagstatus = "";
                                    $scope.checktcsaveflagstatus = "";

                                    if ($scope.getapprovalmasterdetails !== null && $scope.getapprovalmasterdetails.length > 0) {
                                        $scope.allowapproval = $scope.getapprovalmasterdetails[0].acertapP_ApprovaReqlFlg;
                                        if ($scope.allowapproval === true) {
                                            $scope.checktcbalanceflag = false;
                                            if ($scope.getstudentapplieddetails !== null && $scope.getstudentapplieddetails.length > 0) {
                                                $scope.requeststatus = "";
                                                if ($scope.getapprovalresultdetails !== null && $scope.getapprovalresultdetails.length > 0) {
                                                    $scope.requeststatus = $scope.getapprovalresultdetails[0].ascaP_Status;
                                                    if ($scope.requeststatus !== "Approved") {
                                                        $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                        $scope.checktcsaveflagstatus = "Check";
                                                        swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                                    } else {
                                                        $scope.checktcbalanceflag = false;
                                                        $scope.checktcbalanceflagstatus = "Dispaly";
                                                        $scope.checktcsaveflagstatus = "Not Check";
                                                    }
                                                } else {
                                                    $scope.requeststatus = $scope.getstudentapplieddetails[0].ascA_Status;
                                                    $scope.checktcbalanceflag = false;
                                                    $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                    $scope.checktcsaveflagstatus = "Not Check";
                                                    swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                                }
                                            } else {
                                                $scope.checktcbalanceflag = false;
                                                $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                $scope.checktcsaveflagstatus = "Not Check";
                                                swal("For Approval Process ,First Student Need To Put Request For Certificate From Portal");
                                            }
                                        } else {
                                            $scope.checktcbalanceflag = true;
                                        }
                                    } else {
                                        $scope.checktcbalanceflag = true;
                                    }
                                } else {
                                    $scope.checktcbalanceflag = false;
                                }

                                if ($scope.checktcbalanceflag === true) {
                                    if ($scope.Tc_payment === 1) {
                                        $scope.feedetails = "";
                                        $scope.pdadetails = "";
                                        $scope.libraydetails = "";

                                        if ($scope.fees_due > 0) {
                                            $scope.feedetails = "Rs " + $scope.fees_due + "/-" + "Amount Is Due In Fee!";
                                        }

                                        if ($scope.lib_due_trans > 0) {
                                            $scope.libraydetails = "Total " + $scope.lib_due_trans + " Book Not Returned By Student!";
                                        }

                                        if ($scope.pda_due > 0) {
                                            $scope.pdadetails = "Rs " + $scope.pda_due + "/-" + "Amount Is Due In PDA From Student Side!";
                                        }

                                        if ($scope.feedetails === "" && $scope.pdadetails === "" && $scope.libraydetails === "") {
                                            $scope.adm_flag = true;
                                            $scope.cat_flag = true;
                                            $scope.tab_hide = true;
                                        }
                                        else {
                                            $scope.finaldisplay = "";
                                            if ($scope.feedetails !== "") {
                                                $scope.finaldisplay = $scope.feedetails;
                                            }
                                            if ($scope.pdadetails !== "") {
                                                if ($scope.finaldisplay !== "") {
                                                    $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.pdadetails;
                                                } else {
                                                    $scope.finaldisplay = $scope.pdadetails;
                                                }
                                            }
                                            if ($scope.libraydetails !== "") {
                                                if ($scope.finaldisplay !== "") {
                                                    $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.libraydetails;
                                                } else {
                                                    $scope.finaldisplay = $scope.libraydetails;
                                                }
                                            }
                                            $scope.detailsdue = $scope.feedetails + '\n' + $scope.pdadetails + '\n' + $scope.libraydetails;
                                            swal($scope.finaldisplay + '\n' + " So You Can Not Generate TC Untill Due Should Be Clear");
                                            $scope.tab_hide = false;
                                            $scope.details_flag = false;
                                            return;
                                        }
                                    } else {
                                        $scope.checktcsaveflagstatus = "Check";
                                        $scope.adm_flag = true;
                                        $scope.cat_flag = true;
                                        $scope.tab_hide = true;
                                        $scope.details_flag = true;
                                    }
                                } else {
                                    if ($scope.checktcbalanceflagstatus === "Not Dispaly") {
                                        $scope.tab_hide = false;
                                        $scope.details_flag = false;
                                        $scope.checktcsaveflagstatus = "Not Check";
                                    } else {
                                        if ($scope.checktcbalanceflagstatus === "Dispaly") { $scope.checktcsaveflagstatus = "Not Check"; }
                                        else { $scope.checktcsaveflagstatus = "Check"; }
                                        $scope.adm_flag = true;
                                        $scope.cat_flag = true;
                                        $scope.tab_hide = true;
                                        $scope.details_flag = true;
                                    }
                                }
                            }
                        }
                    }
                    else if ($scope.paymentTc == 0 && $scope.libraryTc == 0) {

                        swal("Rs " + $scope.fees_due + "/-" + "Amount is Due and  " + $scope.lib_due_trans + " Books Pending.. So You Can Not Generate TC Untill Fee Due Is Clear");
                        //    $state.reload();
                        //}
                        //else {
                        $scope.myTabIndex = 0;
                        console.log(promise.studentListById);

                        $scope.Stu_Img = promise.studentListById[0].AMST_Photoname;



                        $scope.class_name = promise.studentListById[0].ASMCL_ClassName;

                        $scope.section_name = promise.studentListById[0].ASMC_SectionName;

                        $scope.cls_section_name = $scope.class_name + ':' + $scope.section_name;

                        $scope.due_details_flag = true;
                        $scope.save_flag = true;
                        $scope.pic_show = true;
                        $scope.details_flag = true;



                        angular.forEach($scope.studentDetails, function (value, key) {
                            if (value.ASTC_PromotionDate !== null && value.ASTC_PromotionDate !== "" && value.ASTC_PromotionDate !== undefined) {
                                $scope.date_of_promotion = new Date(value.ASTC_PromotionDate);
                            }
                            else {
                                $scope.date_of_promotion = new Date();
                            }
                            if (value.ASTC_TCApplicationDate !== null && value.ASTC_TCApplicationDate !== undefined && value.ASTC_TCApplicationDate !== "") {
                                $scope.applicationdate = new Date(value.ASTC_TCApplicationDate);
                            }
                            else {
                                $scope.applicationdate = new Date();
                            }
                            if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                $scope.tc_issue_date = new Date(value.ASTC_TCIssueDate);
                            }
                            else {
                                $scope.tc_issue_date = new Date();
                            }
                            if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                $scope.tc_date = new Date(value.ASTC_TCIssueDate);
                            }
                            else {
                                $scope.tc_date = new Date();
                            }

                            if (value.ASTC_TCDate !== null && value.ASTC_TCDate !== undefined && value.ASTC_TCDate !== "") {
                                $scope.tcleft_date = new Date(value.ASTC_TCDate);
                            }
                            else {
                                $scope.tcleft_date = new Date();
                            }
                            if (value.ASTC_LastAttendedDate !== null && value.ASTC_LastAttendedDate !== undefined && value.ASTC_LastAttendedDate !== "") {
                                $scope.last_date_attendance = new Date(value.ASTC_LastAttendedDate);
                            }
                            else {
                                $scope.last_date_attendance = new Date();
                            }

                            $scope.studentnamed = value.student;
                            $scope.Admission_Date = value.AMST_Date;
                            $scope.fathername = value.father;
                            $scope.category = value.IMC_CasteName;
                            $scope.Birth_Date = value.AMST_DOB;
                            $scope.Birth_Date_words = value.AMST_DOB_Words;
                            $scope.stu_ifsc_code = value.AMST_StuBankIFSC_Code;
                            $scope.stu_birth_place = value.AMST_BirthPlace;
                            $scope.stu_birth_cer_no = value.AMST_BirthCertNO;

                            $scope.stu_caste_cer_no = value.AMST_StuCasteCertiNo;
                            $scope.stu_age = value.PASR_Age;
                            $scope.stu_gender = value.AMST_Sex;

                            $scope.stu_mother_tongue = value.AMST_MotherTongue;
                            $scope.stu_nationality = value.AMST_Nationality;
                            $scope.stu_mobile = value.AMST_MobileNo;
                            $scope.stu_email = value.AMST_emailId;

                            $scope.stu_aadhar_no = value.AMST_AadharNo;
                            $scope.stu_blood_group = value.AMST_BloodGroup;
                            $scope.stu_bank_account_no = value.AMST_StuBankAccNo;
                            $scope.stu_bpl_card_no = value.AMST_BPLCardNo;
                            $scope.ASMAY_Year = value.ASMAY_Year;

                            $scope.Reg_No = value.AMST_RegistrationNo;
                            $scope.Adm_No = value.admno;
                            $scope.Amc_Name = value.AMC_Name;
                            $scope.med_instruction = value.ASTC_MediumOfINStruction;
                            $scope.scholarship = value.ASTC_Scholarship;
                            $scope.is_medically_examined = value.ASTC_MedicallyExam;
                            $scope.usr.astC_TCNO = value.ASTC_TCNO;
                            $scope.leaving_reason = value.ASTC_LeavingReason;
                            $scope.no_school_days = value.ASTC_WorkingDays;
                            $scope.no_attended_days = value.ASTC_AttendedDays;
                            $scope.fees_concession = value.ASTC_FeeConcession;
                            $scope.last_exam_detail = value.ASTC_LastExamDetails;
                            $scope.results = value.ASTC_Result;
                            $scope.status = value.ASTC_ResultDetails;
                            $scope.promotion = value.ASTC_Qual_PromotionFlag;
                            $scope.promotedcheck = value.ASTC_Qual_Class;
                            $scope.astC_STSNO = value.stsno;

                            var str = $scope.promotedcheck;
                            if (str !== null && str !== undefined && str !== "" && str !== "Granted On Trial") {
                                var res = str.split(" ", 1);
                                var resall = str.split(" ");
                                if (res[0] === "Granted" || res[0] === "Promoted") {
                                    $scope.promotedtype = res[0];
                                    var res2 = resall[2];
                                    var res3 = resall[3];
                                    var res4 = resall[4];

                                    if (res3 !== undefined && res3 !== null && res3 !== "") {
                                        $scope.qualified_class = res2 + ' ' + res3;
                                    } else {
                                        if (res2 !== undefined && res2 !== null && res2 !== "") {
                                            $scope.qualified_class = res2;
                                        } else {
                                            $scope.qualified_class = "";
                                        }
                                    }
                                    $scope.promoted2 = true;
                                }
                                else {
                                    if (str === "Not Applicable") {
                                        $scope.promotedtype = str;
                                        $scope.qualified_class = "";
                                    }
                                    else {
                                        $scope.promotedtype = str;
                                        $scope.qualified_class = "";
                                    }
                                }
                            }
                            else {
                                $scope.qualified_class = value.ASTC_Qual_Class;
                            }

                            //get_elective_subjects                       
                            if (promise.get_elective_subjects !== null && promise.get_elective_subjects.length > 0) {
                                for (var i = 0; i < promise.get_elective_subjects.length; i++) {
                                    if (i === 0) {
                                        subjectname1 = promise.get_elective_subjects[i].subjectname;
                                    }
                                    else {
                                        subjectname1 = subjectname1 + ',' + promise.get_elective_subjects[i].subjectname;
                                    }
                                }
                                $scope.elective_study = subjectname1;
                                $scope.elective_req = true;
                            }
                            else {
                                $scope.elective_study = value.ASTC_ElectivesStudied;
                                $scope.elective_req = false;
                            }

                            //language
                            if (promise.get_elective_subjects_language !== null && promise.get_elective_subjects_language.length > 0) {
                                for (var j = 0; j < promise.get_elective_subjects_language.length; j++) {
                                    if (j === 0) {
                                        subjectname2 = promise.get_elective_subjects_language[j].subjectname;
                                    }
                                    else {
                                        subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_language[j].subjectname;
                                    }
                                }
                                $scope.language_study = subjectname2;
                                $scope.language_req = true;
                            }
                            else {
                                $scope.language_study = value.ASTC_LanguageStudied;
                                $scope.language_req = false;
                            }

                            // All Subjects 
                            if (promise.get_elective_subjects_common !== null && promise.get_elective_subjects_common.length > 0) {
                                for (var j1 = 0; j1 < promise.get_elective_subjects_common.length; j1++) {
                                    if (j1 === 0) {
                                        subjectname2 = promise.get_elective_subjects_common[j1].subjectname;
                                    }
                                    else {
                                        subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_common[j1].subjectname;
                                    }
                                }
                                $scope.subjects_study = subjectname2;
                                $scope.subjets_req = true;
                            } else {
                                $scope.subjects_study = value.ASTC_SubjectsStudied;
                                $scope.subjets_req = false;
                            }

                            if ($scope.user.status === "S" || $scope.user.status === "D") {
                                if (promise.getexamdetails !== null && promise.getexamdetails.length > 0) {
                                    $scope.last_exam_detail = promise.getexamdetails[0].EME_ExamName;
                                    $scope.results = promise.getexamdetails[0].ESTMP_Result;
                                    if ($scope.results.toUpperCase() === "PASS") {
                                        $scope.status = "Yes";
                                        $scope.promotion = "Yes";
                                    } else {
                                        $scope.status = "No";
                                        $scope.promotion = "No";
                                    }
                                }
                            }

                            if ($scope.last_exam_detail !== null && $scope.last_exam_detail !== undefined && $scope.last_exam_detail !== "") {
                                $scope.examdisable = true;
                            } else {
                                $scope.examdisable = false;
                            }

                            $scope.ncc = value.ASTC_NCCDetails;
                            $scope.extra = value.ASTC_ExtraActivities;
                            $scope.conduct = value.ASTC_Conduct;
                            $scope.remarks = value.ASTC_Remarks;

                            if (promise.getjoineddetails !== null && promise.getjoineddetails.length > 0) {
                                $scope.joinedclassname = promise.getjoineddetails[0].asmcL_ClassName;
                            }
                        });

                        if ($scope.user.status === "S" || $scope.user.status === "D") {

                            $scope.no_school_days = promise.countclass;

                            if ($scope.no_school_days === null) {
                                $scope.noscatt = false;
                            } else {
                                $scope.noscatt = true;
                            }

                            $scope.no_attended_days = promise.attclasscount;

                            if ($scope.no_attended_days === null) {
                                $scope.attdays = false;
                            } else {
                                $scope.attdays = true;
                            }
                        } else {

                            if ($scope.no_school_days === null) {
                                $scope.noscatt = false;
                            } else {
                                $scope.noscatt = true;
                            }

                            if ($scope.no_attended_days === null) {
                                $scope.attdays = false;
                            } else {
                                $scope.attdays = true;
                            }
                        }

                        if (promise.todateatt !== null) {
                            $scope.last_date_attendance = new Date(promise.todateatt);
                        }
                        else {
                            $scope.last_date_attendance = new Date();
                        }

                        $scope.count_issuebooks = promise.count_issuebooks;
                        if ($scope.count_issuebooks !== null && $scope.count_issuebooks.length > 0) {
                            $scope.lib_due_trans = promise.count_issuebooks.length;
                        } else {
                            $scope.lib_due_trans = 0;
                        }


                        if (promise.pdadata !== null && promise.pdadata.length > 0) {
                            $scope.pdadata = promise.pdadata;
                            $scope.pda_due = promise.pdadata[0].pdaS_CBStudentDue;
                        } else {
                            $scope.pda_due = 0;
                        }

                        $scope.getconcession = promise.getconcession;
                        if ($scope.getconcession !== null && $scope.getconcession.length > 0) {
                            $scope.fees_concession = $scope.getconcession[0].fmcC_ConcessionName;
                        }

                        $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                        $scope.getapprovalmasterdetails = promise.getapprovalmasterdetails;
                        $scope.getstudentapplieddetails = promise.getstudentapplieddetails;
                        $scope.getapprovalresultdetails = promise.getapprovalresultdetails;

                        $scope.tcgeneratedornot = promise.tcgeneratedornot;

                        // ***************** CHECKING APPROVAL PROCESS *********************** //

                        if ($scope.tcgeneratedornot === "") {
                            $scope.checktcbalanceflag = true;
                            $scope.checktcbalanceflagstatus = "";
                            $scope.checktcsaveflagstatus = "";

                            if ($scope.getapprovalmasterdetails !== null && $scope.getapprovalmasterdetails.length > 0) {
                                $scope.allowapproval = $scope.getapprovalmasterdetails[0].acertapP_ApprovaReqlFlg;
                                if ($scope.allowapproval === true) {
                                    $scope.checktcbalanceflag = false;
                                    if ($scope.getstudentapplieddetails !== null && $scope.getstudentapplieddetails.length > 0) {
                                        $scope.requeststatus = "";
                                        if ($scope.getapprovalresultdetails !== null && $scope.getapprovalresultdetails.length > 0) {
                                            $scope.requeststatus = $scope.getapprovalresultdetails[0].ascaP_Status;
                                            if ($scope.requeststatus !== "Approved") {
                                                $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                $scope.checktcsaveflagstatus = "Check";
                                                swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                            } else {
                                                $scope.checktcbalanceflag = false;
                                                $scope.checktcbalanceflagstatus = "Dispaly";
                                                $scope.checktcsaveflagstatus = "Not Check";
                                            }
                                        } else {
                                            $scope.requeststatus = $scope.getstudentapplieddetails[0].ascA_Status;
                                            $scope.checktcbalanceflag = false;
                                            $scope.checktcbalanceflagstatus = "Not Dispaly";
                                            $scope.checktcsaveflagstatus = "Not Check";
                                            swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                        }
                                    } else {
                                        $scope.checktcbalanceflag = false;
                                        $scope.checktcbalanceflagstatus = "Not Dispaly";
                                        $scope.checktcsaveflagstatus = "Not Check";
                                        swal("For Approval Process ,First Student Need To Put Request For Certificate From Portal");
                                    }
                                } else {
                                    $scope.checktcbalanceflag = true;
                                }
                            } else {
                                $scope.checktcbalanceflag = true;
                            }
                        } else {
                            $scope.checktcbalanceflag = false;
                        }

                        if ($scope.checktcbalanceflag === true) {
                            if ($scope.Tc_payment === 1) {
                                $scope.feedetails = "";
                                $scope.pdadetails = "";
                                $scope.libraydetails = "";

                                if ($scope.fees_due > 0) {
                                    $scope.feedetails = "Rs " + $scope.fees_due + "/-" + "Amount Is Due In Fee!";
                                }

                                if ($scope.lib_due_trans > 0) {
                                    $scope.libraydetails = "Total " + $scope.lib_due_trans + " Book Not Returned By Student!";
                                }

                                if ($scope.pda_due > 0) {
                                    $scope.pdadetails = "Rs " + $scope.pda_due + "/-" + "Amount Is Due In PDA From Student Side!";
                                }

                                if ($scope.feedetails === "" && $scope.pdadetails === "" && $scope.libraydetails === "") {
                                    $scope.adm_flag = true;
                                    $scope.cat_flag = true;
                                    $scope.tab_hide = true;
                                }
                                else {
                                    $scope.finaldisplay = "";
                                    if ($scope.feedetails !== "") {
                                        $scope.finaldisplay = $scope.feedetails;
                                    }
                                    if ($scope.pdadetails !== "") {
                                        if ($scope.finaldisplay !== "") {
                                            $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.pdadetails;
                                        } else {
                                            $scope.finaldisplay = $scope.pdadetails;
                                        }
                                    }
                                    if ($scope.libraydetails !== "") {
                                        if ($scope.finaldisplay !== "") {
                                            $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.libraydetails;
                                        } else {
                                            $scope.finaldisplay = $scope.libraydetails;
                                        }
                                    }
                                    $scope.detailsdue = $scope.feedetails + '\n' + $scope.pdadetails + '\n' + $scope.libraydetails;
                                    swal($scope.finaldisplay + '\n' + " So You Can Not Generate TC Untill Due Should Be Clear");
                                    $scope.tab_hide = false;
                                    $scope.details_flag = false;
                                    return;
                                }
                            } else {
                                $scope.checktcsaveflagstatus = "Check";
                                $scope.adm_flag = true;
                                $scope.cat_flag = true;
                                $scope.tab_hide = true;
                                $scope.details_flag = true;
                            }
                        } else {
                            if ($scope.checktcbalanceflagstatus === "Not Dispaly") {
                                $scope.tab_hide = false;
                                $scope.details_flag = false;
                                $scope.checktcsaveflagstatus = "Not Check";
                            } else {
                                if ($scope.checktcbalanceflagstatus === "Dispaly") { $scope.checktcsaveflagstatus = "Not Check"; }
                                else { $scope.checktcsaveflagstatus = "Check"; }
                                $scope.adm_flag = true;
                                $scope.cat_flag = true;
                                $scope.tab_hide = true;
                                $scope.details_flag = true;
                            }
                        }
                    }



                    if ($scope.paymentTc == 0) {
                        if ($scope.fees_due > 0 && ($scope.user.status === 'S' || $scope.user.status === 'D' || $scope.user.status === 'T')) {
                            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due! So You Can Not Generate TC Untill Fee Due Is Clear");
                            //    $state.reload();
                            //}
                            //else {
                            $scope.myTabIndex = 0;
                            console.log(promise.studentListById);

                            $scope.Stu_Img = promise.studentListById[0].AMST_Photoname;



                            $scope.class_name = promise.studentListById[0].ASMCL_ClassName;

                            $scope.section_name = promise.studentListById[0].ASMC_SectionName;

                            $scope.cls_section_name = $scope.class_name + ':' + $scope.section_name;

                            $scope.due_details_flag = true;
                            $scope.save_flag = true;
                            $scope.pic_show = true;
                            $scope.details_flag = true;



                            angular.forEach($scope.studentDetails, function (value, key) {
                                if (value.ASTC_PromotionDate !== null && value.ASTC_PromotionDate !== "" && value.ASTC_PromotionDate !== undefined) {
                                    $scope.date_of_promotion = new Date(value.ASTC_PromotionDate);
                                }
                                else {
                                    $scope.date_of_promotion = new Date();
                                }
                                if (value.ASTC_TCApplicationDate !== null && value.ASTC_TCApplicationDate !== undefined && value.ASTC_TCApplicationDate !== "") {
                                    $scope.applicationdate = new Date(value.ASTC_TCApplicationDate);
                                }
                                else {
                                    $scope.applicationdate = new Date();
                                }
                                if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                    $scope.tc_issue_date = new Date(value.ASTC_TCIssueDate);
                                }
                                else {
                                    $scope.tc_issue_date = new Date();
                                }
                                if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                    $scope.tc_date = new Date(value.ASTC_TCIssueDate);
                                }
                                else {
                                    $scope.tc_date = new Date();
                                }

                                if (value.ASTC_TCDate !== null && value.ASTC_TCDate !== undefined && value.ASTC_TCDate !== "") {
                                    $scope.tcleft_date = new Date(value.ASTC_TCDate);
                                }
                                else {
                                    $scope.tcleft_date = new Date();
                                }
                                if (value.ASTC_LastAttendedDate !== null && value.ASTC_LastAttendedDate !== undefined && value.ASTC_LastAttendedDate !== "") {
                                    $scope.last_date_attendance = new Date(value.ASTC_LastAttendedDate);
                                }
                                else {
                                    $scope.last_date_attendance = new Date();
                                }

                                $scope.studentnamed = value.student;
                                $scope.Admission_Date = value.AMST_Date;
                                $scope.fathername = value.father;
                                $scope.category = value.IMC_CasteName;
                                $scope.Birth_Date = value.AMST_DOB;
                                $scope.Birth_Date_words = value.AMST_DOB_Words;
                                $scope.stu_ifsc_code = value.AMST_StuBankIFSC_Code;
                                $scope.stu_birth_place = value.AMST_BirthPlace;
                                $scope.stu_birth_cer_no = value.AMST_BirthCertNO;

                                $scope.stu_caste_cer_no = value.AMST_StuCasteCertiNo;
                                $scope.stu_age = value.PASR_Age;
                                $scope.stu_gender = value.AMST_Sex;

                                $scope.stu_mother_tongue = value.AMST_MotherTongue;
                                $scope.stu_nationality = value.AMST_Nationality;
                                $scope.stu_mobile = value.AMST_MobileNo;
                                $scope.stu_email = value.AMST_emailId;

                                $scope.stu_aadhar_no = value.AMST_AadharNo;
                                $scope.stu_blood_group = value.AMST_BloodGroup;
                                $scope.stu_bank_account_no = value.AMST_StuBankAccNo;
                                $scope.stu_bpl_card_no = value.AMST_BPLCardNo;
                                $scope.ASMAY_Year = value.ASMAY_Year;

                                $scope.Reg_No = value.AMST_RegistrationNo;
                                $scope.Adm_No = value.admno;
                                $scope.Amc_Name = value.AMC_Name;
                                $scope.med_instruction = value.ASTC_MediumOfINStruction;
                                $scope.scholarship = value.ASTC_Scholarship;
                                $scope.is_medically_examined = value.ASTC_MedicallyExam;
                                $scope.usr.astC_TCNO = value.ASTC_TCNO;
                                $scope.leaving_reason = value.ASTC_LeavingReason;
                                $scope.no_school_days = value.ASTC_WorkingDays;
                                $scope.no_attended_days = value.ASTC_AttendedDays;
                                $scope.fees_concession = value.ASTC_FeeConcession;
                                $scope.last_exam_detail = value.ASTC_LastExamDetails;
                                $scope.results = value.ASTC_Result;
                                $scope.status = value.ASTC_ResultDetails;
                                $scope.promotion = value.ASTC_Qual_PromotionFlag;
                                $scope.promotedcheck = value.ASTC_Qual_Class;
                                $scope.astC_STSNO = value.stsno;

                                var str = $scope.promotedcheck;
                                if (str !== null && str !== undefined && str !== "" && str !== "Granted On Trial") {
                                    var res = str.split(" ", 1);
                                    var resall = str.split(" ");
                                    if (res[0] === "Granted" || res[0] === "Promoted") {
                                        $scope.promotedtype = res[0];
                                        var res2 = resall[2];
                                        var res3 = resall[3];
                                        var res4 = resall[4];

                                        if (res3 !== undefined && res3 !== null && res3 !== "") {
                                            $scope.qualified_class = res2 + ' ' + res3;
                                        } else {
                                            if (res2 !== undefined && res2 !== null && res2 !== "") {
                                                $scope.qualified_class = res2;
                                            } else {
                                                $scope.qualified_class = "";
                                            }
                                        }
                                        $scope.promoted2 = true;
                                    }
                                    else {
                                        if (str === "Not Applicable") {
                                            $scope.promotedtype = str;
                                            $scope.qualified_class = "";
                                        }
                                        else {
                                            $scope.promotedtype = str;
                                            $scope.qualified_class = "";
                                        }
                                    }
                                }
                                else {
                                    $scope.qualified_class = value.ASTC_Qual_Class;
                                }

                                //get_elective_subjects                       
                                if (promise.get_elective_subjects !== null && promise.get_elective_subjects.length > 0) {
                                    for (var i = 0; i < promise.get_elective_subjects.length; i++) {
                                        if (i === 0) {
                                            subjectname1 = promise.get_elective_subjects[i].subjectname;
                                        }
                                        else {
                                            subjectname1 = subjectname1 + ',' + promise.get_elective_subjects[i].subjectname;
                                        }
                                    }
                                    $scope.elective_study = subjectname1;
                                    $scope.elective_req = true;
                                }
                                else {
                                    $scope.elective_study = value.ASTC_ElectivesStudied;
                                    $scope.elective_req = false;
                                }

                                //language
                                if (promise.get_elective_subjects_language !== null && promise.get_elective_subjects_language.length > 0) {
                                    for (var j = 0; j < promise.get_elective_subjects_language.length; j++) {
                                        if (j === 0) {
                                            subjectname2 = promise.get_elective_subjects_language[j].subjectname;
                                        }
                                        else {
                                            subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_language[j].subjectname;
                                        }
                                    }
                                    $scope.language_study = subjectname2;
                                    $scope.language_req = true;
                                }
                                else {
                                    $scope.language_study = value.ASTC_LanguageStudied;
                                    $scope.language_req = false;
                                }

                                // All Subjects 
                                if (promise.get_elective_subjects_common !== null && promise.get_elective_subjects_common.length > 0) {
                                    for (var j1 = 0; j1 < promise.get_elective_subjects_common.length; j1++) {
                                        if (j1 === 0) {
                                            subjectname2 = promise.get_elective_subjects_common[j1].subjectname;
                                        }
                                        else {
                                            subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_common[j1].subjectname;
                                        }
                                    }
                                    $scope.subjects_study = subjectname2;
                                    $scope.subjets_req = true;
                                } else {
                                    $scope.subjects_study = value.ASTC_SubjectsStudied;
                                    $scope.subjets_req = false;
                                }

                                if ($scope.user.status === "S" || $scope.user.status === "D") {
                                    if (promise.getexamdetails !== null && promise.getexamdetails.length > 0) {
                                        $scope.last_exam_detail = promise.getexamdetails[0].EME_ExamName;
                                        $scope.results = promise.getexamdetails[0].ESTMP_Result;
                                        if ($scope.results.toUpperCase() === "PASS") {
                                            $scope.status = "Yes";
                                            $scope.promotion = "Yes";
                                        } else {
                                            $scope.status = "No";
                                            $scope.promotion = "No";
                                        }
                                    }
                                }

                                if ($scope.last_exam_detail !== null && $scope.last_exam_detail !== undefined && $scope.last_exam_detail !== "") {
                                    $scope.examdisable = true;
                                } else {
                                    $scope.examdisable = false;
                                }

                                $scope.ncc = value.ASTC_NCCDetails;
                                $scope.extra = value.ASTC_ExtraActivities;
                                $scope.conduct = value.ASTC_Conduct;
                                $scope.remarks = value.ASTC_Remarks;

                                if (promise.getjoineddetails !== null && promise.getjoineddetails.length > 0) {
                                    $scope.joinedclassname = promise.getjoineddetails[0].asmcL_ClassName;
                                }
                            });

                            if ($scope.user.status === "S" || $scope.user.status === "D") {

                                $scope.no_school_days = promise.countclass;

                                if ($scope.no_school_days === null) {
                                    $scope.noscatt = false;
                                } else {
                                    $scope.noscatt = true;
                                }

                                $scope.no_attended_days = promise.attclasscount;

                                if ($scope.no_attended_days === null) {
                                    $scope.attdays = false;
                                } else {
                                    $scope.attdays = true;
                                }
                            } else {

                                if ($scope.no_school_days === null) {
                                    $scope.noscatt = false;
                                } else {
                                    $scope.noscatt = true;
                                }

                                if ($scope.no_attended_days === null) {
                                    $scope.attdays = false;
                                } else {
                                    $scope.attdays = true;
                                }
                            }

                            if (promise.todateatt !== null) {
                                $scope.last_date_attendance = new Date(promise.todateatt);
                            }
                            else {
                                $scope.last_date_attendance = new Date();
                            }

                            $scope.count_issuebooks = promise.count_issuebooks;
                            if ($scope.count_issuebooks !== null && $scope.count_issuebooks.length > 0) {
                                $scope.lib_due_trans = promise.count_issuebooks.length;
                            } else {
                                $scope.lib_due_trans = 0;
                            }


                            if (promise.pdadata !== null && promise.pdadata.length > 0) {
                                $scope.pdadata = promise.pdadata;
                                $scope.pda_due = promise.pdadata[0].pdaS_CBStudentDue;
                            } else {
                                $scope.pda_due = 0;
                            }

                            $scope.getconcession = promise.getconcession;
                            if ($scope.getconcession !== null && $scope.getconcession.length > 0) {
                                $scope.fees_concession = $scope.getconcession[0].fmcC_ConcessionName;
                            }

                            $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                            $scope.getapprovalmasterdetails = promise.getapprovalmasterdetails;
                            $scope.getstudentapplieddetails = promise.getstudentapplieddetails;
                            $scope.getapprovalresultdetails = promise.getapprovalresultdetails;

                            $scope.tcgeneratedornot = promise.tcgeneratedornot;

                            // ***************** CHECKING APPROVAL PROCESS *********************** //

                            if ($scope.tcgeneratedornot === "") {
                                $scope.checktcbalanceflag = true;
                                $scope.checktcbalanceflagstatus = "";
                                $scope.checktcsaveflagstatus = "";

                                if ($scope.getapprovalmasterdetails !== null && $scope.getapprovalmasterdetails.length > 0) {
                                    $scope.allowapproval = $scope.getapprovalmasterdetails[0].acertapP_ApprovaReqlFlg;
                                    if ($scope.allowapproval === true) {
                                        $scope.checktcbalanceflag = false;
                                        if ($scope.getstudentapplieddetails !== null && $scope.getstudentapplieddetails.length > 0) {
                                            $scope.requeststatus = "";
                                            if ($scope.getapprovalresultdetails !== null && $scope.getapprovalresultdetails.length > 0) {
                                                $scope.requeststatus = $scope.getapprovalresultdetails[0].ascaP_Status;
                                                if ($scope.requeststatus !== "Approved") {
                                                    $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                    $scope.checktcsaveflagstatus = "Check";
                                                    swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                                } else {
                                                    $scope.checktcbalanceflag = false;
                                                    $scope.checktcbalanceflagstatus = "Dispaly";
                                                    $scope.checktcsaveflagstatus = "Not Check";
                                                }
                                            } else {
                                                $scope.requeststatus = $scope.getstudentapplieddetails[0].ascA_Status;
                                                $scope.checktcbalanceflag = false;
                                                $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                $scope.checktcsaveflagstatus = "Not Check";
                                                swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                            }
                                        } else {
                                            $scope.checktcbalanceflag = false;
                                            $scope.checktcbalanceflagstatus = "Not Dispaly";
                                            $scope.checktcsaveflagstatus = "Not Check";
                                            swal("For Approval Process ,First Student Need To Put Request For Certificate From Portal");
                                        }
                                    } else {
                                        $scope.checktcbalanceflag = true;
                                    }
                                } else {
                                    $scope.checktcbalanceflag = true;
                                }
                            } else {
                                $scope.checktcbalanceflag = false;
                            }

                            if ($scope.checktcbalanceflag === true) {
                                if ($scope.Tc_payment === 1) {
                                    $scope.feedetails = "";
                                    $scope.pdadetails = "";
                                    $scope.libraydetails = "";

                                    if ($scope.fees_due > 0) {
                                        $scope.feedetails = "Rs " + $scope.fees_due + "/-" + "Amount Is Due In Fee!";
                                    }

                                    if ($scope.lib_due_trans > 0) {
                                        $scope.libraydetails = "Total " + $scope.lib_due_trans + " Book Not Returned By Student!";
                                    }

                                    if ($scope.pda_due > 0) {
                                        $scope.pdadetails = "Rs " + $scope.pda_due + "/-" + "Amount Is Due In PDA From Student Side!";
                                    }

                                    if ($scope.feedetails === "" && $scope.pdadetails === "" && $scope.libraydetails === "") {
                                        $scope.adm_flag = true;
                                        $scope.cat_flag = true;
                                        $scope.tab_hide = true;
                                    }
                                    else {
                                        $scope.finaldisplay = "";
                                        if ($scope.feedetails !== "") {
                                            $scope.finaldisplay = $scope.feedetails;
                                        }
                                        if ($scope.pdadetails !== "") {
                                            if ($scope.finaldisplay !== "") {
                                                $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.pdadetails;
                                            } else {
                                                $scope.finaldisplay = $scope.pdadetails;
                                            }
                                        }
                                        if ($scope.libraydetails !== "") {
                                            if ($scope.finaldisplay !== "") {
                                                $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.libraydetails;
                                            } else {
                                                $scope.finaldisplay = $scope.libraydetails;
                                            }
                                        }
                                        $scope.detailsdue = $scope.feedetails + '\n' + $scope.pdadetails + '\n' + $scope.libraydetails;
                                        swal($scope.finaldisplay + '\n' + " So You Can Not Generate TC Untill Due Should Be Clear");
                                        $scope.tab_hide = false;
                                        $scope.details_flag = false;
                                        return;
                                    }
                                } else {
                                    $scope.checktcsaveflagstatus = "Check";
                                    $scope.adm_flag = true;
                                    $scope.cat_flag = true;
                                    $scope.tab_hide = true;
                                    $scope.details_flag = true;
                                }
                            } else {
                                if ($scope.checktcbalanceflagstatus === "Not Dispaly") {
                                    $scope.tab_hide = false;
                                    $scope.details_flag = false;
                                    $scope.checktcsaveflagstatus = "Not Check";
                                } else {
                                    if ($scope.checktcbalanceflagstatus === "Dispaly") { $scope.checktcsaveflagstatus = "Not Check"; }
                                    else { $scope.checktcsaveflagstatus = "Check"; }
                                    $scope.adm_flag = true;
                                    $scope.cat_flag = true;
                                    $scope.tab_hide = true;
                                    $scope.details_flag = true;
                                }
                            }
                        }
                    }
                    else {

                        $scope.myTabIndex = 0;
                        console.log(promise.studentListById);

                        $scope.Stu_Img = promise.studentListById[0].AMST_Photoname;



                        $scope.class_name = promise.studentListById[0].ASMCL_ClassName;

                        $scope.section_name = promise.studentListById[0].ASMC_SectionName;

                        $scope.cls_section_name = $scope.class_name + ':' + $scope.section_name;

                        $scope.due_details_flag = true;
                        $scope.save_flag = true;
                        $scope.pic_show = true;
                        $scope.details_flag = true;



                        angular.forEach($scope.studentDetails, function (value, key) {
                            if (value.ASTC_PromotionDate !== null && value.ASTC_PromotionDate !== "" && value.ASTC_PromotionDate !== undefined) {
                                $scope.date_of_promotion = new Date(value.ASTC_PromotionDate);
                            }
                            else {
                                $scope.date_of_promotion = new Date();
                            }
                            if (value.ASTC_TCApplicationDate !== null && value.ASTC_TCApplicationDate !== undefined && value.ASTC_TCApplicationDate !== "") {
                                $scope.applicationdate = new Date(value.ASTC_TCApplicationDate);
                            }
                            else {
                                $scope.applicationdate = new Date();
                            }
                            if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                $scope.tc_issue_date = new Date(value.ASTC_TCIssueDate);
                            }
                            else {
                                $scope.tc_issue_date = new Date();
                            }
                            if (value.ASTC_TCIssueDate !== null && value.ASTC_TCIssueDate !== undefined && value.ASTC_TCIssueDate !== "") {
                                $scope.tc_date = new Date(value.ASTC_TCIssueDate);
                            }
                            else {
                                $scope.tc_date = new Date();
                            }

                            if (value.ASTC_TCDate !== null && value.ASTC_TCDate !== undefined && value.ASTC_TCDate !== "") {
                                $scope.tcleft_date = new Date(value.ASTC_TCDate);
                            }
                            else {
                                $scope.tcleft_date = new Date();
                            }
                            if (value.ASTC_LastAttendedDate !== null && value.ASTC_LastAttendedDate !== undefined && value.ASTC_LastAttendedDate !== "") {
                                $scope.last_date_attendance = new Date(value.ASTC_LastAttendedDate);
                            }
                            else {
                                $scope.last_date_attendance = new Date();
                            }

                            $scope.studentnamed = value.student;
                            $scope.Admission_Date = value.AMST_Date;
                            $scope.fathername = value.father;
                            $scope.category = value.IMC_CasteName;
                            $scope.Birth_Date = value.AMST_DOB;
                            $scope.Birth_Date_words = value.AMST_DOB_Words;
                            $scope.stu_ifsc_code = value.AMST_StuBankIFSC_Code;
                            $scope.stu_birth_place = value.AMST_BirthPlace;
                            $scope.stu_birth_cer_no = value.AMST_BirthCertNO;

                            $scope.stu_caste_cer_no = value.AMST_StuCasteCertiNo;
                            $scope.stu_age = value.PASR_Age;
                            $scope.stu_gender = value.AMST_Sex;

                            $scope.stu_mother_tongue = value.AMST_MotherTongue;
                            $scope.stu_nationality = value.AMST_Nationality;
                            $scope.stu_mobile = value.AMST_MobileNo;
                            $scope.stu_email = value.AMST_emailId;

                            $scope.stu_aadhar_no = value.AMST_AadharNo;
                            $scope.stu_blood_group = value.AMST_BloodGroup;
                            $scope.stu_bank_account_no = value.AMST_StuBankAccNo;
                            $scope.stu_bpl_card_no = value.AMST_BPLCardNo;
                            $scope.ASMAY_Year = value.ASMAY_Year;

                            $scope.Reg_No = value.AMST_RegistrationNo;
                            $scope.Adm_No = value.admno;
                            $scope.Amc_Name = value.AMC_Name;
                            $scope.med_instruction = value.ASTC_MediumOfINStruction;
                            $scope.scholarship = value.ASTC_Scholarship;
                            $scope.is_medically_examined = value.ASTC_MedicallyExam;
                            $scope.usr.astC_TCNO = value.ASTC_TCNO;
                            $scope.leaving_reason = value.ASTC_LeavingReason;
                            $scope.no_school_days = value.ASTC_WorkingDays;
                            $scope.no_attended_days = value.ASTC_AttendedDays;
                            $scope.fees_concession = value.ASTC_FeeConcession;
                            $scope.last_exam_detail = value.ASTC_LastExamDetails;
                            $scope.results = value.ASTC_Result;
                            $scope.status = value.ASTC_ResultDetails;
                            $scope.promotion = value.ASTC_Qual_PromotionFlag;
                            $scope.promotedcheck = value.ASTC_Qual_Class;
                            $scope.astC_STSNO = value.stsno;

                            var str = $scope.promotedcheck;
                            if (str !== null && str !== undefined && str !== "" && str !== "Granted On Trial") {
                                var res = str.split(" ", 1);
                                var resall = str.split(" ");
                                if (res[0] === "Granted" || res[0] === "Promoted") {
                                    $scope.promotedtype = res[0];
                                    var res2 = resall[2];
                                    var res3 = resall[3];
                                    var res4 = resall[4];

                                    if (res3 !== undefined && res3 !== null && res3 !== "") {
                                        $scope.qualified_class = res2 + ' ' + res3;
                                    } else {
                                        if (res2 !== undefined && res2 !== null && res2 !== "") {
                                            $scope.qualified_class = res2;
                                        } else {
                                            $scope.qualified_class = "";
                                        }
                                    }
                                    $scope.promoted2 = true;
                                }
                                else {
                                    if (str === "Not Applicable") {
                                        $scope.promotedtype = str;
                                        $scope.qualified_class = "";
                                    }
                                    else {
                                        $scope.promotedtype = str;
                                        $scope.qualified_class = "";
                                    }
                                }
                            }
                            else {
                                $scope.qualified_class = value.ASTC_Qual_Class;
                            }

                            //get_elective_subjects                       
                            if (promise.get_elective_subjects !== null && promise.get_elective_subjects.length > 0) {
                                for (var i = 0; i < promise.get_elective_subjects.length; i++) {
                                    if (i === 0) {
                                        subjectname1 = promise.get_elective_subjects[i].subjectname;
                                    }
                                    else {
                                        subjectname1 = subjectname1 + ',' + promise.get_elective_subjects[i].subjectname;
                                    }
                                }
                                $scope.elective_study = subjectname1;
                                $scope.elective_req = true;
                            }
                            else {
                                $scope.elective_study = value.ASTC_ElectivesStudied;
                                $scope.elective_req = false;
                            }

                            //language
                            if (promise.get_elective_subjects_language !== null && promise.get_elective_subjects_language.length > 0) {
                                for (var j = 0; j < promise.get_elective_subjects_language.length; j++) {
                                    if (j === 0) {
                                        subjectname2 = promise.get_elective_subjects_language[j].subjectname;
                                    }
                                    else {
                                        subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_language[j].subjectname;
                                    }
                                }
                                $scope.language_study = subjectname2;
                                $scope.language_req = true;
                            }
                            else {
                                $scope.language_study = value.ASTC_LanguageStudied;
                                $scope.language_req = false;
                            }

                            // All Subjects 
                            if (promise.get_elective_subjects_common !== null && promise.get_elective_subjects_common.length > 0) {
                                for (var j1 = 0; j1 < promise.get_elective_subjects_common.length; j1++) {
                                    if (j1 === 0) {
                                        subjectname2 = promise.get_elective_subjects_common[j1].subjectname;
                                    }
                                    else {
                                        subjectname2 = subjectname2 + ',' + promise.get_elective_subjects_common[j1].subjectname;
                                    }
                                }
                                $scope.subjects_study = subjectname2;
                                $scope.subjets_req = true;
                            } else {
                                $scope.subjects_study = value.ASTC_SubjectsStudied;
                                $scope.subjets_req = false;
                            }

                            if ($scope.user.status === "S" || $scope.user.status === "D") {
                                if (promise.getexamdetails !== null && promise.getexamdetails.length > 0) {
                                    $scope.last_exam_detail = promise.getexamdetails[0].EME_ExamName;
                                    $scope.results = promise.getexamdetails[0].ESTMP_Result;
                                    if ($scope.results.toUpperCase() === "PASS") {
                                        $scope.status = "Yes";
                                        $scope.promotion = "Yes";
                                    } else {
                                        $scope.status = "No";
                                        $scope.promotion = "No";
                                    }
                                }
                            }

                            if ($scope.last_exam_detail !== null && $scope.last_exam_detail !== undefined && $scope.last_exam_detail !== "") {
                                $scope.examdisable = true;
                            } else {
                                $scope.examdisable = false;
                            }

                            $scope.ncc = value.ASTC_NCCDetails;
                            $scope.extra = value.ASTC_ExtraActivities;
                            $scope.conduct = value.ASTC_Conduct;
                            $scope.remarks = value.ASTC_Remarks;

                            if (promise.getjoineddetails !== null && promise.getjoineddetails.length > 0) {
                                $scope.joinedclassname = promise.getjoineddetails[0].asmcL_ClassName;
                            }
                        });

                        if ($scope.user.status === "S" || $scope.user.status === "D") {

                            $scope.no_school_days = promise.countclass;

                            if ($scope.no_school_days === null) {
                                $scope.noscatt = false;
                            } else {
                                $scope.noscatt = true;
                            }

                            $scope.no_attended_days = promise.attclasscount;

                            if ($scope.no_attended_days === null) {
                                $scope.attdays = false;
                            } else {
                                $scope.attdays = true;
                            }
                        } else {

                            if ($scope.no_school_days === null) {
                                $scope.noscatt = false;
                            } else {
                                $scope.noscatt = true;
                            }

                            if ($scope.no_attended_days === null) {
                                $scope.attdays = false;
                            } else {
                                $scope.attdays = true;
                            }
                        }

                        if (promise.todateatt !== null) {
                            $scope.last_date_attendance = new Date(promise.todateatt);
                        }
                        else {
                            $scope.last_date_attendance = new Date();
                        }

                        $scope.count_issuebooks = promise.count_issuebooks;
                        if ($scope.count_issuebooks !== null && $scope.count_issuebooks.length > 0) {
                            $scope.lib_due_trans = promise.count_issuebooks.length;
                        } else {
                            $scope.lib_due_trans = 0;
                        }


                        if (promise.pdadata !== null && promise.pdadata.length > 0) {
                            $scope.pdadata = promise.pdadata;
                            $scope.pda_due = promise.pdadata[0].pdaS_CBStudentDue;
                        } else {
                            $scope.pda_due = 0;
                        }

                        $scope.getconcession = promise.getconcession;
                        if ($scope.getconcession !== null && $scope.getconcession.length > 0) {
                            $scope.fees_concession = $scope.getconcession[0].fmcC_ConcessionName;
                        }

                        $scope.viewstudentfeedetails = promise.viewstudentfeedetails;

                        $scope.getapprovalmasterdetails = promise.getapprovalmasterdetails;
                        $scope.getstudentapplieddetails = promise.getstudentapplieddetails;
                        $scope.getapprovalresultdetails = promise.getapprovalresultdetails;

                        $scope.tcgeneratedornot = promise.tcgeneratedornot;

                        // ***************** CHECKING APPROVAL PROCESS *********************** //

                        if ($scope.tcgeneratedornot === "") {
                            $scope.checktcbalanceflag = true;
                            $scope.checktcbalanceflagstatus = "";
                            $scope.checktcsaveflagstatus = "";

                            if ($scope.getapprovalmasterdetails !== null && $scope.getapprovalmasterdetails.length > 0) {
                                $scope.allowapproval = $scope.getapprovalmasterdetails[0].acertapP_ApprovaReqlFlg;
                                if ($scope.allowapproval === true) {
                                    $scope.checktcbalanceflag = false;
                                    if ($scope.getstudentapplieddetails !== null && $scope.getstudentapplieddetails.length > 0) {
                                        $scope.requeststatus = "";
                                        if ($scope.getapprovalresultdetails !== null && $scope.getapprovalresultdetails.length > 0) {
                                            $scope.requeststatus = $scope.getapprovalresultdetails[0].ascaP_Status;
                                            if ($scope.requeststatus !== "Approved") {
                                                $scope.checktcbalanceflagstatus = "Not Dispaly";
                                                $scope.checktcsaveflagstatus = "Check";
                                                swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                            } else {
                                                $scope.checktcbalanceflag = false;
                                                $scope.checktcbalanceflagstatus = "Dispaly";
                                                $scope.checktcsaveflagstatus = "Not Check";
                                            }
                                        } else {
                                            $scope.requeststatus = $scope.getstudentapplieddetails[0].ascA_Status;
                                            $scope.checktcbalanceflag = false;
                                            $scope.checktcbalanceflagstatus = "Not Dispaly";
                                            $scope.checktcsaveflagstatus = "Not Check";
                                            swal("Certificate Status Is " + $scope.requeststatus + " So TC Can Not Be Generated");
                                        }
                                    } else {
                                        $scope.checktcbalanceflag = false;
                                        $scope.checktcbalanceflagstatus = "Not Dispaly";
                                        $scope.checktcsaveflagstatus = "Not Check";
                                        swal("For Approval Process ,First Student Need To Put Request For Certificate From Portal");
                                    }
                                } else {
                                    $scope.checktcbalanceflag = true;
                                }
                            } else {
                                $scope.checktcbalanceflag = true;
                            }
                        } else {
                            $scope.checktcbalanceflag = false;
                        }

                        if ($scope.checktcbalanceflag === true) {
                            if ($scope.Tc_payment === 1) {
                                $scope.feedetails = "";
                                $scope.pdadetails = "";
                                $scope.libraydetails = "";

                                if ($scope.fees_due > 0) {
                                    $scope.feedetails = "Rs " + $scope.fees_due + "/-" + "Amount Is Due In Fee!";
                                }

                                if ($scope.lib_due_trans > 0) {
                                    $scope.libraydetails = "Total " + $scope.lib_due_trans + " Book Not Returned By Student!";
                                }

                                if ($scope.pda_due > 0) {
                                    $scope.pdadetails = "Rs " + $scope.pda_due + "/-" + "Amount Is Due In PDA From Student Side!";
                                }

                                if ($scope.feedetails === "" && $scope.pdadetails === "" && $scope.libraydetails === "") {
                                    $scope.adm_flag = true;
                                    $scope.cat_flag = true;
                                    $scope.tab_hide = true;
                                }
                                else {
                                    $scope.finaldisplay = "";
                                    if ($scope.feedetails !== "") {
                                        $scope.finaldisplay = $scope.feedetails;
                                    }
                                    if ($scope.pdadetails !== "") {
                                        if ($scope.finaldisplay !== "") {
                                            $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.pdadetails;
                                        } else {
                                            $scope.finaldisplay = $scope.pdadetails;
                                        }
                                    }
                                    if ($scope.libraydetails !== "") {
                                        if ($scope.finaldisplay !== "") {
                                            $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.libraydetails;
                                        } else {
                                            $scope.finaldisplay = $scope.libraydetails;
                                        }
                                    }
                                    $scope.detailsdue = $scope.feedetails + '\n' + $scope.pdadetails + '\n' + $scope.libraydetails;
                                    swal($scope.finaldisplay + '\n' + " So You Can Not Generate TC Untill Due Should Be Clear");
                                    $scope.tab_hide = false;
                                    $scope.details_flag = false;
                                    return;
                                }
                            } else {
                                $scope.checktcsaveflagstatus = "Check";
                                $scope.adm_flag = true;
                                $scope.cat_flag = true;
                                $scope.tab_hide = true;
                                $scope.details_flag = true;
                            }
                        } else {
                            if ($scope.checktcbalanceflagstatus === "Not Dispaly") {
                                $scope.tab_hide = false;
                                $scope.details_flag = false;
                                $scope.checktcsaveflagstatus = "Not Check";
                            } else {
                                if ($scope.checktcbalanceflagstatus === "Dispaly") { $scope.checktcsaveflagstatus = "Not Check"; }
                                else { $scope.checktcsaveflagstatus = "Check"; }
                                $scope.adm_flag = true;
                                $scope.cat_flag = true;
                                $scope.tab_hide = true;
                                $scope.details_flag = true;
                            }
                        }
                    }
                }
                else {
                    swal("Record not found");
                    $scope.tab_hide = false;
                    $scope.ASMAY_Year = null;
                    $scope.Reg_No = null;
                    $scope.Adm_No = null;
                    $scope.section_name = null;
                    $scope.class_name = null;
                    $scope.details_flag = false;
                }
            });
        };

        $scope.studentListById = [];

        //Adm.No:Name radio button toggle function//
        $scope.radion_button_func = function (user) {

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
            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.pic_show = false;
            $scope.stu_details = true;
            $scope.stu_pers = true;
            $scope.due = true;
            $scope.other_details = true;
            $scope.cls_section_name = null;
            $scope.details_flag = false;

            var year = "";
            var cls_name = "";
            var sec_name = "";
            if ($scope.academic_year !== null && $scope.classname === undefined && $scope.sectionname === undefined) {
                year = $scope.academic_year;
                cls_name = $scope.classname = 0;
                sec_name = $scope.sectionname = 0;
            }
            else if ($scope.academic_year !== null && $scope.classname !== null && $scope.sectionname === undefined) {
                year = $scope.academic_year;
                cls_name = $scope.classname;
                sec_name = $scope.sectionname = 0;
            } else {
                year = $scope.academic_year;
                cls_name = $scope.classname;
                sec_name = $scope.sectionname;
            }
            var data = {
                "ASMAY_Id": year,
                "ASMCL_Id": cls_name,
                "ASMS_Id": sec_name,
                "adm_num_flag": $scope.user.admno_name,
                "flag": $scope.user.status
            };

            apiService.create("StudentTC/get_student_status/", data).then(function (promise) {

                if (promise.studentlist !== null && promise.studentlist.length > 0) {
                    $scope.studentname = promise.studentlist;
                    $scope.stud_name = false;
                }
                else {
                    $scope.stud_name = true;
                }
            });
        };

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
            $scope.adm_number_flag = false;
            $scope.studentname = [];
            $scope.AMST_Id = "";
            if ($scope.user.status === "S") {
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

            if ($scope.user.status === "S" || $scope.user.status === "L" || $scope.user.status === "D" || $scope.user.status === "T") {
                var year = "";
                var cls_name = "";
                var sec_name = "";
                if ($scope.academic_year !== null && $scope.academic_year !== "" && $scope.classname !== null && $scope.classname !== "" && $scope.sectionname !== null
                    && $scope.sectionname !== "") {
                    year = $scope.academic_year;
                    cls_name = $scope.classname;
                    sec_name = $scope.sectionname;
                }
                else if ($scope.academic_year !== null && $scope.academic_year !== "" && $scope.classname !== null && $scope.classname !== ""
                    && $scope.sectionname === "") {
                    year = $scope.academic_year;
                    cls_name = $scope.classname;
                    sec_name = $scope.sectionname = 0;
                }
                else if ($scope.academic_year !== null && $scope.academic_year !== "" && $scope.classname === "" && $scope.sectionname === "") {
                    year = $scope.academic_year;
                    cls_name = $scope.classname = 0;
                    sec_name = $scope.sectionname = 0;
                }
                else {
                    year = $scope.academic_year = 0;
                    cls_name = $scope.classname = 0;
                    sec_name = $scope.sectionname = 0;
                }

                var data = {
                    "ASMAY_Id": year,
                    "ASMCL_Id": cls_name,
                    "ASMS_Id": sec_name,
                    "flag": $scope.user.status,
                    "adm_num_flag": $scope.user.admno_name
                };

                apiService.create("StudentTC/get_student_status/", data).then(function (promise) {
                    if (promise.studentlist !== null && promise.studentlist.length > 0) {
                        $scope.studentname = promise.studentlist;
                        $scope.stud_name = false;
                        $scope.adm_number_flag = true;
                    }
                    else {
                        $scope.AMST_Id = "";
                        $scope.stud_name = true;
                        $scope.adm_number_flag = true;
                    }
                });
            }
        };

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


                $scope.feedetails = "";
                $scope.pdadetails = "";
                $scope.libraydetails = "";
                $scope.finaldisplay = "";

                if ($scope.fees_due > 0) {
                    $scope.feedetails = "Rs " + $scope.fees_due + "/-" + "Amount Is Due In Fee!";
                }

                if ($scope.lib_due_trans > 0) {
                    $scope.libraydetails = "Total " + $scope.lib_due_trans + " Book Not Returned By Student!";
                }

                if ($scope.pda_due > 0) {
                    $scope.pdadetails = "Rs " + $scope.pda_due + "/-" + "Amount Is Due In PDA From Student Side!";
                }

                if ($scope.feedetails === "" && $scope.pdadetails === "" && $scope.libraydetails === "") {
                    //dd
                }
                else {
                    $scope.finaldisplay = "";
                    if ($scope.feedetails !== "") {
                        $scope.finaldisplay = $scope.feedetails;
                    }
                    if ($scope.pdadetails !== "") {
                        if ($scope.finaldisplay !== "") {
                            $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.pdadetails;
                        } else {
                            $scope.finaldisplay = $scope.pdadetails;
                        }
                    }
                    if ($scope.libraydetails !== "") {
                        if ($scope.finaldisplay !== "") {
                            $scope.finaldisplay = $scope.finaldisplay + '\n' + $scope.libraydetails;
                        } else {
                            $scope.finaldisplay = $scope.libraydetails;
                        }
                    }
                }

                if ($scope.promotedtype === "Granted" || $scope.promotedtype === "Promoted") {
                    if ($scope.qualified_class !== undefined && $scope.qualified_class !== null && $scope.qualified_class !== "") {
                        $scope.qualified_class = $scope.promotedtype + ' ' + 'To ' + $scope.qualified_class;
                    } else {
                        $scope.qualified_class = $scope.promotedtype;
                    }
                }
                else {
                    $scope.qualified_class = $scope.promotedtype;
                }

                var data = {
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "ASTC_TCNO": $scope.usr.astC_TCNO,
                    "ASTC_TCApplicationDate": new Date($scope.applicationdate).toDateString(),
                    "ASTC_LeavingReason": $scope.leaving_reason,
                    "ASTC_TCDate": new Date($scope.tc_date).toDateString(),
                    "ASTC_TCIssueDate": new Date($scope.tcleft_date).toDateString(),
                    "ASTC_LastAttendedDate": new Date($scope.last_date_attendance).toDateString(),
                    "ASTC_WorkingDays": $scope.no_school_days,
                    "ASTC_AttendedDays": $scope.no_attended_days,
                    "ASTC_PromotionDate": $scope.promotionDate,
                    "ASTC_MediumOfINStruction": $scope.med_instruction,
                    "ASTC_LanguageStudied": $scope.language_study,
                    "ASTC_ElectivesStudied": $scope.elective_study,
                    "ASTC_SubjectsStudied": $scope.subjects_study,
                    "ASTC_Scholarship": $scope.scholarship,
                    "ASTC_MedicallyExam": $scope.is_medically_examined,
                    "ASTC_FeePaid": $scope.paid_fees,
                    "ASTC_FeeConcession": $scope.fees_concession,
                    "ASTC_LastExamDetails": $scope.last_exam_detail,
                    "ASTC_Result": $scope.results,
                    "ASTC_ResultDetails": $scope.status,
                    "ASTC_Qual_PromotionFlag": $scope.promotion,
                    "ASTC_Qual_Class": $scope.qualified_class,
                    "ASTC_NCCDetails": $scope.ncc,
                    "ASTC_ExtraActivities": $scope.extra,
                    "ASTC_Conduct": $scope.conduct,
                    "ASTC_Remarks": $scope.remarks,
                    "ASTC_TemporaryFlag": $scope.tempchk,
                    "ASTC_ActiveFlag": $scope.user.status,
                    "Fee_Due_Amnt": $scope.fees_due,
                    "Library_Due_Amnt": $scope.lib_due_trans,
                    "Store_Canteen_Due": $scope.store_tran_due,
                    "PDA_Due": $scope.pda_due,
                    "Email_flag": $scope.Email_flag,
                    "transnumconfigsettings": $scope.tcnumbering,
                    "allorindividual": $scope.Admnoallind,
                    "ASMAY_Id": $scope.academic_year
                };


                if ($scope.checktcsaveflagstatus === "Check" && $scope.finaldisplay !== ''
                    && ($scope.user.status === 'S' || $scope.user.status === 'D' || $scope.user.status === 'T')) {
                    $scope.finaldisplay1 = $scope.finaldisplay;
                }

                else {
                    $scope.finaldisplay = "Are you sure?";
                }

                swal({
                    title: $scope.finaldisplay,
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
                            apiService.create("StudentTC/savedata", data).then(function (promise) {
                                if (promise.tcflagexists === "TC No. Already Exists") {
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
                                }
                                else {
                                    swal('Record Already Exists/Failed To Save');
                                }
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

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

        $scope.checkErr_TCLeft = function (applicationdate, tc_date) {
            $scope.errMessage_tcleft = '';
            //var curDate = new Date();
            if (new Date(applicationdate) > new Date(tc_date)) {
                $scope.errMessage_tcleft = 'Left Date should be greater than the Application Date';
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
                    $scope.chk_school_days($scope.no_school_days, $scope.no_attended_days);
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
            // Clear_tc_details

            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.errMessage = "";
            $scope.errMessage_tc = "";
            $scope.errMessage_promo = "";
            $scope.errMessage_appli = "";
            $scope.errMessage_school_day = "";
            $scope.errMessage_No_school_day = "";
            $scope.errMessage_last_day = "";


            if ($scope.tc_dis_flag == true) {
                //$scope.tc_No = null;
            } else {
                $scope.usr.astC_TCNO = null;
            }


            // $scope.applicationdate = null;
            $scope.tc_issue_date = null;
            $scope.leaving_reason = null;
            // $scope.tc_date = null;
            // $scope.last_date_attendance = null;

            if ($scope.noscatt == true) {
                //$scope.no_school_days = null;
            } else {
                $scope.no_school_days = null;
            }
            if ($scope.attdays == true) {
                // $scope.no_attended_days = null;
            } else {
                $scope.no_attended_days = null;
            }
            $scope.date_of_promotion = null;
            $scope.last_class_studied = null;
            $scope.attdays = true;
        };

        $scope.Clear_other_details = function () {
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
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
            $scope.class_studying = "";
            $scope.admission_year = "";
            $scope.tc.regno = null;
            $scope.tc.admno = null;
        };

        $scope.Clear_all_details = function () {
            $state.reload();
        };

        $scope.chklength = function (med_instruction) {
            if ($scope.med_instruction.length === 20)
                $scope.message_flag = false;
        };

        $scope.chklength_lan = function (language_study) {
            if ($scope.language_study.length === 30)
                $scope.message_flag_lang = false;
        };

        //Checking Duplicate Tc number
        $scope.check_dup_tc_no = function (val) {
            var data = {
                "ASTC_TCNO": val
            };
            apiService.create("StudentTC/chk_dup_tc", data).then(function (promise) {
                if (promise.tcflagexists === "TC No. Already Exists") {
                    swal('TC Number Already Exists', 'Please Enter Different TC Number');
                    $scope.next_on_tc_no = true;
                }
                else {
                    $scope.next_on_tc_no = false;
                }
            });
        };

        $scope.All_Individual = function (Admnoallind) {
            var data = "";
            if ($scope.Admnoallind === 'Indi') {
                $scope.AMST_Id = "";
                $scope.academic_year = "";
                $scope.classname = "";
                $scope.sectionname = "";
                $scope.yr_show = true;
                $scope.clas_show = false;
                $scope.sec_show = false;
                $scope.details_flag = false;
                $scope.tab_hide = false;
                $scope.stud_name = true;
                $scope.adm_number_flag = false;

                data = {
                    "adm_num_flag": $scope.user.admno_name,
                    "flag": $scope.user.status
                };



                apiService.create("StudentTC/getstudentdata", data).then(function (promise) {
                    if (promise.academicList !== null && promise.academicList.length > 0) {
                        $scope.academicList = promise.academicList;
                        $scope.currentYear = promise.currentYear;
                        var name = "";
                        for (var i = 0; i < $scope.academicList.length; i++) {
                            name = $scope.academicList[i].asmaY_Id;
                            for (var j = 0; j < $scope.currentYear.length; j++) {
                                if (parseInt(name) === parseInt($scope.currentYear[j].asmaY_Id)) {
                                    $scope.academicList[i].Selected = true;
                                    $scope.academic_year = $scope.currentYear[j].asmaY_Id;
                                }
                            }
                        }
                    }
                    if (promise.classlist !== null && promise.classlist.length > 0) {
                        $scope.classlist = promise.classlist;
                    }
                    if (promise.sectionList !== null && promise.sectionList.length > 0) {
                        $scope.SectionList = promise.sectionList;
                    }
                    $scope.studentname = promise.studentlist;
                    $scope.adm_number_flag = true;
                    $scope.class_name = false;
                    $scope.section_name = false;
                });
                $scope.adm_number_flag = false;
            }

            if ($scope.Admnoallind === 'All') {
                $scope.AMST_Id = "";
                $scope.yr_show = false;
                $scope.clas_show = false;
                $scope.sec_show = false;
                $scope.details_flag = false;
                $scope.tab_hide = false;
                $scope.academic_year = "";
                $scope.classname = "";
                $scope.sectionname = "";
                $scope.stud_name = false;

                if ($scope.AMST_Id !== null) {
                    var year = $scope.academic_year = 0;
                    var cls_name = $scope.classname = 0;
                    var sec_name = $scope.sectionname = 0;
                }

                data = {
                    "ASMAY_Id": year,
                    "ASMCL_Id": cls_name,
                    "ASMS_Id": sec_name,
                    "adm_num_flag": $scope.user.admno_name,
                    "flag": $scope.user.status
                };

                apiService.create("StudentTC/getstudent_name_list", data).
                    then(function (promise) {

                        $scope.academicList = promise.academicList;
                        $scope.currentYear = promise.currentYear;
                        var name = "";
                        for (var i = 0; i < $scope.academicList.length; i++) {
                            name = $scope.academicList[i].asmaY_Id;
                            for (var j = 0; j < $scope.currentYear.length; j++) {
                                if (parseInt(name) === parseInt($scope.currentYear[j].asmaY_Id)) {
                                    $scope.academicList[i].Selected = true;
                                    $scope.academic_year = $scope.currentYear[j].asmaY_Id;
                                }
                            }
                        }
                        if (promise.studentlist !== null) {
                            if (promise.studentlist.length > 0) {
                                $scope.studentname = promise.studentlist;
                                $scope.adm_number_flag = true;
                                $scope.stud_name = false;
                            }
                            else {
                                swal("Record Not Found");
                                $scope.studentname = "";
                                $scope.class_name = "";
                                $scope.section_name = "";
                                $scope.Amc_Name = "";
                                $scope.ASMAY_Year = "";
                                $scope.Reg_No = "";
                                $scope.Adm_No = "";
                                $scope.Stu_Img = "";
                                $scope.tab_hide = false;
                                $scope.details_flag = false;
                                $scope.adm_number_flag = false;
                                $scope.stud_name = true;
                            }
                        }
                    });
            }
        };

        //search filter
        $scope.searchfilter = function (objj, radioobj) {
            var data = "";
            if (objj.search.length >= '3') {
                $scope.studentlst = "";
                if ($scope.Admnoallind === 'All') {
                    data = {
                        "searchfilter": objj.search,
                        "AMST_SOL": radioobj,
                        "allorindividual": $scope.Admnoallind,
                        "flag": $scope.user.admno_name
                    };
                }
                else {
                    data = {
                        "searchfilter": objj.search,
                        "ASMAY_Id": $scope.academic_year,
                        "AMST_SOL": radioobj,
                        "allorindividual": $scope.Admnoallind,
                        "flag": $scope.user.admno_name
                    };
                }

                apiService.create("StudentTC/searchfilter", data).
                    then(function (promise) {
                        if (promise.studentlistsearch !== null || promise.studentlistsearch.length > 0) {
                            $scope.getstudenttcdetails = promise.studentlistsearch;
                        } else {
                            $scope.AMST_Id = "";
                            swal("No students are found for your search");
                        }
                    });
            }
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

        $scope.onchangeacademicyear = function () {
            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.details_flag = false;
            $scope.tab_hide = false;

            angular.forEach($scope.academicList, function (tt) {
                if (tt.asmaY_Id == $scope.academic_year) {
                    $scope.yearname = tt.asmaY_Year;
                    $scope.yearname1 = $scope.yearname.split("-");
                    $scope.lblyearr = $scope.yearname1[1];
                }
            });

        };

        //$scope.validatedue = function () {
        //    if ($scope.Tc_payment === 1 && ($scope.user.status === 'S' || $scope.user.status === 'D' || $scope.user.status === 'T')) {
        //        if ($scope.tempchk === true) {

        //        }
        //        else {
        //            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due! So You Can Not Generate TC Untill Fee Due Is Clear");
        //            $state.reload();
        //        }
        //    }
        //};

        $scope.saveUserold = function () {
            if ($scope.myForm1.$valid && $scope.myForm2.$valid && $scope.myForm4.$valid) {
                if ($scope.date_of_promotion !== "" && $scope.date_of_promotion !== null && $scope.date_of_promotion !== undefined) {
                    $scope.promotionDate = new Date($scope.date_of_promotion).toDateString();
                }
                else {
                    $scope.promotionDate = "";
                }

                if ($scope.fees_due > 0) {
                    if ($scope.Tc_payment === 1 && ($scope.user.status === 'S' || $scope.user.status === 'D' || $scope.user.status === 'T')) {
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
                                        if ($scope.Admnoallind === 'Indi') {
                                            year = $scope.academic_year;
                                        }
                                        else {
                                            year = 0;
                                        }
                                        if ($scope.promotedtype === "Granted" || $scope.promotedtype === "Promoted") {
                                            if ($scope.qualified_class !== undefined && $scope.qualified_class !== null && $scope.qualified_class !== "") {
                                                $scope.qualified_class = $scope.promotedtype + ' ' + 'To ' + $scope.qualified_class;
                                            } else {
                                                $scope.qualified_class = $scope.promotedtype;
                                            }
                                        }
                                        else {
                                            $scope.qualified_class = $scope.promotedtype;
                                        }

                                        var data = {
                                            "AMST_Id": $scope.AMST_Id.amsT_Id,
                                            "ASTC_TCNO": $scope.usr.astC_TCNO,
                                            "ASTC_TCApplicationDate": new Date($scope.applicationdate).toDateString(),
                                            "ASTC_LeavingReason": $scope.leaving_reason,
                                            "ASTC_TCDate": new Date($scope.tc_date).toDateString(),
                                            "ASTC_TCIssueDate": new Date($scope.tcleft_date).toDateString(),
                                            "ASTC_LastAttendedDate": new Date($scope.last_date_attendance).toDateString(),
                                            "ASTC_WorkingDays": $scope.no_school_days,
                                            "ASTC_AttendedDays": $scope.no_attended_days,
                                            "ASTC_PromotionDate": $scope.promotionDate,
                                            "ASTC_MediumOfINStruction": $scope.med_instruction,
                                            "ASTC_LanguageStudied": $scope.language_study,
                                            "ASTC_ElectivesStudied": $scope.elective_study,
                                            "ASTC_SubjectsStudied": $scope.subjects_study,
                                            "ASTC_Scholarship": $scope.scholarship,
                                            "ASTC_MedicallyExam": $scope.is_medically_examined,
                                            "ASTC_FeePaid": $scope.paid_fees,
                                            "ASTC_FeeConcession": $scope.fees_concession,
                                            "ASTC_LastExamDetails": $scope.last_exam_detail,
                                            "ASTC_Result": $scope.results,
                                            "ASTC_ResultDetails": $scope.status,
                                            "ASTC_Qual_PromotionFlag": $scope.promotion,
                                            "ASTC_Qual_Class": $scope.qualified_class,
                                            "ASTC_NCCDetails": $scope.ncc,
                                            "ASTC_ExtraActivities": $scope.extra,
                                            "ASTC_Conduct": $scope.conduct,
                                            "ASTC_Remarks": $scope.remarks,
                                            "ASTC_TemporaryFlag": $scope.tempchk,
                                            "ASTC_ActiveFlag": $scope.user.status,
                                            "Fee_Due_Amnt": $scope.fees_due,
                                            "Library_Due_Amnt": $scope.lib_due_trans,
                                            "Store_Canteen_Due": $scope.store_tran_due,
                                            "PDA_Due": $scope.pda_due,
                                            "Email_flag": $scope.Email_flag,
                                            "transnumconfigsettings": $scope.tcnumbering,
                                            "allorindividual": $scope.Admnoallind,
                                            "ASMAY_Id": year
                                        };
                                        apiService.create("StudentTC/savedata", data).then(function (promise) {

                                            if (promise.tcflagexists === "TC No. Already Exists") {
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
                                });
                        }
                        else {
                            swal("Rs " + $scope.fees_due + "/-" + "Amount is Due! So You Can Not Generate TC Untill Fee Due Is Clear");
                            $state.reload();
                        }
                    }
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
                                    if ($scope.Admnoallind === 'Indi') {
                                        year = $scope.academic_year;
                                    }
                                    else {
                                        year = 0;
                                    }
                                    if ($scope.promotedtype === "Granted" || $scope.promotedtype === "Promoted") {
                                        if ($scope.qualified_class !== undefined && $scope.qualified_class !== null && $scope.qualified_class !== "") {
                                            $scope.qualified_class = $scope.promotedtype + ' ' + 'To ' + $scope.qualified_class;
                                        } else {
                                            $scope.qualified_class = $scope.promotedtype;
                                        }
                                    }
                                    else {
                                        $scope.qualified_class = $scope.promotedtype;
                                    }

                                    var data = {
                                        "AMST_Id": $scope.AMST_Id.amsT_Id,
                                        "ASTC_TCNO": $scope.usr.astC_TCNO,
                                        "ASTC_TCApplicationDate": new Date($scope.applicationdate).toDateString(),
                                        "ASTC_LeavingReason": $scope.leaving_reason,
                                        "ASTC_TCDate": new Date($scope.tc_date).toDateString(),
                                        "ASTC_TCIssueDate": new Date($scope.tcleft_date).toDateString(),
                                        "ASTC_LastAttendedDate": new Date($scope.last_date_attendance).toDateString(),
                                        "ASTC_WorkingDays": $scope.no_school_days,
                                        "ASTC_AttendedDays": $scope.no_attended_days,
                                        "ASTC_PromotionDate": $scope.promotionDate,
                                        "ASTC_MediumOfINStruction": $scope.med_instruction,
                                        "ASTC_LanguageStudied": $scope.language_study,
                                        "ASTC_ElectivesStudied": $scope.elective_study,
                                        "ASTC_SubjectsStudied": $scope.subjects_study,
                                        "ASTC_Scholarship": $scope.scholarship,
                                        "ASTC_MedicallyExam": $scope.is_medically_examined,
                                        "ASTC_FeePaid": $scope.paid_fees,
                                        "ASTC_FeeConcession": $scope.fees_concession,
                                        "ASTC_LastExamDetails": $scope.last_exam_detail,
                                        "ASTC_Result": $scope.results,
                                        "ASTC_ResultDetails": $scope.status,
                                        "ASTC_Qual_PromotionFlag": $scope.promotion,
                                        "ASTC_Qual_Class": $scope.qualified_class,
                                        "ASTC_NCCDetails": $scope.ncc,
                                        "ASTC_ExtraActivities": $scope.extra,
                                        "ASTC_Conduct": $scope.conduct,
                                        "ASTC_Remarks": $scope.remarks,
                                        "ASTC_TemporaryFlag": $scope.tempchk,
                                        "ASTC_ActiveFlag": $scope.user.status,
                                        "Fee_Due_Amnt": $scope.fees_due,
                                        "Library_Due_Amnt": $scope.lib_due_trans,
                                        "Store_Canteen_Due": $scope.store_tran_due,
                                        "PDA_Due": $scope.pda_due,
                                        "Email_flag": $scope.Email_flag,
                                        "transnumconfigsettings": $scope.tcnumbering,
                                        "allorindividual": $scope.Admnoallind,
                                        "ASMAY_Id": year
                                    };
                                    apiService.create("StudentTC/savedata", data).then(function (promise) {
                                        if (promise.tcflagexists === "TC No. Already Exists") {
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
                                if ($scope.Admnoallind === 'Indi') {
                                    year = $scope.academic_year;
                                }
                                else {
                                    year = 0;
                                }
                                if ($scope.promotedtype === "Granted" || $scope.promotedtype === "Promoted") {
                                    if ($scope.qualified_class !== undefined && $scope.qualified_class !== null && $scope.qualified_class !== "") {
                                        $scope.qualified_class = $scope.promotedtype + ' ' + 'To ' + $scope.qualified_class;
                                    } else {
                                        $scope.qualified_class = $scope.promotedtype;
                                    }
                                    //$scope.qualified_class = $scope.promotedtype + ' ' + 'to' + ' ' + $scope.qualified_class;
                                }
                                else {
                                    $scope.qualified_class = $scope.promotedtype;
                                }
                                var data = {
                                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                                    "ASTC_TCNO": $scope.usr.astC_TCNO,
                                    "ASTC_TCApplicationDate": new Date($scope.applicationdate).toDateString(),
                                    "ASTC_LeavingReason": $scope.leaving_reason,
                                    "ASTC_TCDate": new Date($scope.tc_date).toDateString(),
                                    "ASTC_TCIssueDate": new Date($scope.tcleft_date).toDateString(),
                                    "ASTC_LastAttendedDate": new Date($scope.last_date_attendance).toDateString(),
                                    "ASTC_WorkingDays": $scope.no_school_days,
                                    "ASTC_AttendedDays": $scope.no_attended_days,
                                    "ASTC_PromotionDate": $scope.promotionDate,
                                    "ASTC_MediumOfINStruction": $scope.med_instruction,
                                    "ASTC_LanguageStudied": $scope.language_study,
                                    "ASTC_ElectivesStudied": $scope.elective_study,
                                    "ASTC_SubjectsStudied": $scope.subjects_study,
                                    "ASTC_Scholarship": $scope.scholarship,
                                    "ASTC_MedicallyExam": $scope.is_medically_examined,
                                    "transnumconfigsettings": $scope.tcnumbering,
                                    "ASTC_FeePaid": $scope.paid_fees,
                                    "ASTC_FeeConcession": $scope.fees_concession,
                                    "ASTC_LastExamDetails": $scope.last_exam_detail,
                                    "ASTC_Result": $scope.results,
                                    "ASTC_ResultDetails": $scope.status,
                                    "ASTC_Qual_PromotionFlag": $scope.promotion,
                                    "ASTC_Qual_Class": $scope.qualified_class,
                                    "ASTC_NCCDetails": $scope.ncc,
                                    "ASTC_ExtraActivities": $scope.extra,
                                    "ASTC_Conduct": $scope.conduct,
                                    "ASTC_Remarks": $scope.remarks,
                                    "ASTC_TemporaryFlag": $scope.tempchk,
                                    "ASTC_ActiveFlag": $scope.user.status,
                                    "Fee_Due_Amnt": $scope.fees_due,
                                    "Library_Due_Amnt": $scope.lib_due_trans,
                                    "Store_Canteen_Due": $scope.store_tran_due,
                                    "PDA_Due": $scope.pda_due,
                                    "Email_flag": $scope.Email_flag,
                                    "allorindividual": $scope.Admnoallind,
                                    "ASMAY_Id": year
                                };

                                var config = {
                                    headers: {
                                        'Content-Type': 'application/json;'
                                    }
                                };


                                apiService.create("StudentTC/savedata", data)
                                    .then(function (promise) {

                                        if (promise.tcflagexists === "TC No. Already Exists") {
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
                                            swal('Record Already Exists/Failed To Save');
                                        }
                                    });
                            }
                            else {
                                swal("Cancelled");
                            }
                        });
                }
            }
            else {
                swal("Please Enter Details in the  Previous Tabs");
                $scope.submitted1 = true;
                $scope.submitted2 = true;
                $scope.submitted4 = true;
            }
        };
    }

})();