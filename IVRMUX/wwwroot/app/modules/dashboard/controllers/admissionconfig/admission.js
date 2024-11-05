(function () {
    'use strict';
    angular.module('app').controller('studentadmissionController', studentadmissionController)
    studentadmissionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams']
    function studentadmissionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams) {

        $scope.EditId = 0;
        $scope.editdatasec = false;
        var monthname;
        var datename;
        var yearname;
        $scope.search = "";
        $scope.amsT_Date = new Date();
        $scope.sortKey = 'amsT_Id';
        $scope.sortReverse = true;

        $scope.address = true;
        $scope.Parents = true;
        $scope.Others = true;
        $scope.Medical = true;
        $scope.DocumentUpload = true;
        $scope.photoupload_flag = 'Default';

        var maxageeyear;
        var maxageemonth;
        var maxageedays;
        var minageeyear;
        var minageemonth;
        var minageedays;
        $scope.studentGuardianDetails = {};
        $scope.studentReferenceDetails = [];
        $scope.studentSourceDetails = [];
        $scope.studentActivityDetails = [];
        $scope.allActivity = [];
        $scope.allSources = [];
        $scope.allRefrence = [];
        $scope.RegistrationNumbering = [];
        $scope.AdmissionNumbering = [];
        $scope.govtBondList = [];
        $scope.EditRecord = [];
        $scope.newcaste = {};

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.DOB = true;
        this.minDate = new Date();

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        $scope.upper_grid = true;
        $scope.show_upper_grid = function () {
            $scope.upper_grid = $scope.upper_grid ? false : true;
        };

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                    console.log($scope.userPrivileges);
                }
            }
        }

        var paginationformasters = 0;
        var copty = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.appcutoffdate = false;

        $scope.paginate2 = "paginate2";
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 5;

        $scope.itemsPerPages = 5;
        $scope.currentPages = 1;
        $scope.myDate = new Date();

        $scope.myTabIndex = 0;
        $scope.obj = {};
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };

        //LOAD DATA
        $scope.BindData = function () {
            $scope.obj.amsT_FirstName = "";
            $scope.obj.amsT_MiddleName = "";
            $scope.obj.amsT_LastName = "";

            apiService.getDATA("StudentAdmission/Getdetails").then(function (promise) {
                $scope.currentPage = 1;
                $scope.itemsPerPage = paginationformasters;
                $scope.statelabel = true;
                $scope.statelabel2 = true;

                $scope.districtlabel = true;
                $scope.districtlabel2 = true;

                $scope.allAcademicYear = promise.academicYearOnLoad;
                $scope.allAcademicYear1 = promise.allAcademicYear;

                $scope.allAcademicYearsearch = promise.academicYearOnLoad;
                $scope.allAcademicYear1search = promise.allAcademicYear;

                //previous  last year
                $scope.previouspassyear = promise.academicYearOnLoad;
                for (var i = 0; i < $scope.allAcademicYear.length; i++) {
                    name = $scope.allAcademicYear[i].asmaY_Id;
                    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                        if (parseInt(name) === parseInt($scope.allAcademicYear1[j].asmaY_Id)) {
                            $scope.allAcademicYear[i].Selected = true;
                            $scope.obj.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                            $scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                        }
                    }
                }

                $scope.yearre = promise.academicyearforreadmit;
                $scope.allClass = promise.allClass;
                $scope.obj.asmS_Id = "";
                $scope.mastersection = promise.mastersection;
                if ($scope.mastersection != null && $scope.mastersection.length > 0) {
                    angular.forEach($scope.mastersection, function (value1) {
                        if (value1.asmC_SectionName == "A") {
                            $scope.obj.asmS_Id = value1.asmS_Id;
                        }
                    });
                }
                if ($scope.obj.asmS_Id != "") {

                }
                else {
                    $scope.obj.asmS_Id = $scope.mastersection[0].asmS_Id;
                }
              
                //previous class
                $scope.previousclassd = promise.allClass;
                $scope.allSection = promise.allSection;
                $scope.allcountry = promise.allCountry;
                // var countryid = $scope.allcountry[0].countryco

                var countryid = 0;
                angular.forEach($scope.allcountry, function (value1) {
                    if (value1.ivrmmC_Default == 1) {
                        countryid = value1.ivrmmC_Id;
                    }
                });

                $scope.allcountryforbirthplace = promise.allcountryforbirthplace;

                //previous country
                $scope.allcountry1 = promise.allCountry;
                $scope.allReligion = promise.allReligion;
                $scope.allReligionfather = promise.allReligion;
                $scope.allReligionmother = promise.allReligion;
                $scope.allcasteCategory = promise.allcasteCategory;
                angular.forEach($scope.allcasteCategory, function (value1) {
                    if (value1.imcC_CategoryName == 'GENERAL') {
                        $scope.obj.imcC_Id = value1.imcC_Id;

                    }
                });
                $scope.allCaste = promise.allCaste;
                //angular.forEach($scope.allCaste, function (value1) {
                //    if (value1.imC_CasteName == 'GENERAL') {
                //        //$scope.obj.iC_Id = value1.iC_Id;
                //        $scope.obj.iC_Id = $scope.allCaste[0];
                //    }
                //});
                if ($scope.allCaste != null && $scope.allCaste.length > 0) {
                    for (var i = 0; i < $scope.allCaste.length; i++) {
                        if ($scope.allCaste[i].imC_CasteName == 'GENERAL') {
                            $scope.obj.iC_Id = $scope.allCaste[i];

                        }

                    }

                }



                //$scope.obj.iC_Id = $scope.allCaste[0].iC_Id;
                $scope.allCastefather = promise.allCaste;
                $scope.allCastemother = promise.allCaste;
                $scope.allRefrence = promise.allRefrence;
                $scope.allSources = promise.allSources;
                $scope.adm_m_student = promise.adm_m_student;
                $scope.presentCountgrid = $scope.adm_m_student.length;
                $scope.allActivity = promise.allActivity;
                $scope.adm_m_stud_cat = promise.adm_m_stud_cat;

                //$scope.allState = promise.allState_new;
                $scope.allStateonchangeNationality = promise.allState;
                $scope.allState1 = promise.allState;
                $scope.siblingClass = promise.allClass;

                $scope.documentList = promise.documentList;
                angular.forEach($scope.documentList, function (value1, i) {
                    $scope.documentList[i].document_Path = "";
                });

                var admissioncongiguration = promise.admissioncongigurationList;
                localStorage.setItem("admissionconfigsettings", JSON.stringify(admissioncongiguration));

                var admissionSettings = JSON.parse(localStorage.getItem("admissionconfigsettings"));

                if (admissionSettings !== null && admissionSettings !== "") {
                    if (admissionSettings.length > 0) {
                        $scope.maxage = admissionSettings[0].asC_MaxAgeApl_Flag;
                        $scope.minage = admissionSettings[0].asC_MinAgeApl_Flag;
                        $scope.gender = admissionSettings[0].asC_Default_Gender;
                        $scope.parents_m_income = admissionSettings[0].asC_ParentsMonthlyIncome_Flag;
                        $scope.parents_a_income = admissionSettings[0].asC_ParentsAnnualIncome_Flag;
                        $scope.default_sms_email = admissionSettings[0].asC_DefaultSMS_Flag;
                        $scope.photo = admissionSettings[0].asC_DefaultPhotoUpload;
                    }
                }
                else {
                    $scope.gender = 0;
                    $scope.maxage = 0;
                    $scope.minage = 0;
                    $scope.parents_m_income = 0;
                    $scope.parents_a_income = 0;
                    $scope.default_sms_email = 0;
                    $scope.photo = 0;
                }

                if ($scope.gender === "M") {
                    $scope.amsT_Sex = 'Male';
                }
                else if ($scope.gender === "F") {
                    $scope.amsT_Sex = 'Female';
                }
                else if ($scope.gender === "Ot") {
                    $scope.amsT_Sex = 'Other';
                }
                else {
                    $scope.amsT_Sex = '';
                }
                if (parseInt($scope.parents_m_income) === 1) {
                    $scope.parents_monthly_income = 1;
                }
                else {
                    $scope.parents_monthly_income = 0;
                }
                if (parseInt($scope.parents_a_income) === 1) {
                    $scope.parents_annual_income = 1;
                }
                else {
                    $scope.parents_annual_income = 0;
                }
                if (parseInt($scope.photo) === 1) {
                    $scope.profile_photo = 1;
                }
                else {
                    $scope.profile_photo = 2;
                }

                var transnumconfig = promise.admTransNumSetting;
                localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));
                var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

                var admregcount = 0;
                var admnocount = 0;
                for (var i1 = 0; i1 < transnumconfigsettings.length; i1++) {
                    if (transnumconfigsettings.length > 0) {
                        $scope.transnumbconfigurationsettings = transnumconfigsettings[i1];

                        if (transnumconfigsettings[i1].imN_Flag === "AdmissionReg") {
                            admregcount += 1;
                            $scope.RegistrationNumbering = transnumconfigsettings[i1];
                            if (transnumconfigsettings[i1].imN_AutoManualFlag === "Manual") {
                                $scope.reg_ = true;
                                $scope.regpreventduplicateflag = transnumconfigsettings[i1].imN_DuplicatesFlag;
                            }
                            else {
                                $scope.reg_ = false;
                            }
                        }

                        if (transnumconfigsettings[i1].imN_Flag === "Admission") {
                            admnocount += 1;
                            $scope.AdmissionNumbering = transnumconfigsettings[i1];
                            if (transnumconfigsettings[i1].imN_AutoManualFlag === "Manual") {
                                $scope.Adm_ = true;
                                $scope.admpreventduplicateflag = transnumconfigsettings[i1].imN_DuplicatesFlag;
                            }
                            else {
                                $scope.Adm_ = false;
                            }
                        }
                    }
                }

                //Getting Transaction Numbering.

                if (promise.adm_m_student !== null && promise.adm_m_student !== "") {
                    for (var i2 = 0; i2 < promise.adm_m_student.length; i2++) {
                        if (promise.adm_m_student[i2].amsT_SOL === "S") {
                            promise.adm_m_student[i2].amsT_SOL = "Active";
                        }
                        else if (promise.adm_m_student[i2].amsT_SOL === "D") {
                            promise.adm_m_student[i2].amsT_SOL = "Deactive";
                        }
                        else if (promise.adm_m_student[i2].amsT_SOL === "L") {
                            promise.adm_m_student[i2].amsT_SOL = "Left";
                        }
                        else if (promise.adm_m_student[i2].amsT_SOL === "WD") {
                            promise.adm_m_student[i2].amsT_SOL = "Withdrawn";
                        }
                    }
                }

                if (promise.govtBondList !== null && promise.govtBondList !== "") {
                    $scope.govtBondList = promise.govtBondList;
                    angular.forEach($scope.govtBondList, function (value1, i) {
                        $scope.govtBondList[i].amstB_BandNo = "";
                    });
                }
                if (promise.boardList !== null && promise.boardList !== "") {
                    $scope.boardList = promise.boardList;
                }

                //fee concession list.
                if (promise.concessionList !== null && promise.concessionList !== "") {
                    $scope.concessionList = promise.concessionList;
                }
                else {
                    swal("Fee Concession Is Not Created For This Institute.Please Create Fee Concession Using Fee Master Concession Page In Fees Module And Then Proceed.");
                }
                //School type.
                $scope.Schooltypelist = promise.schooltypelist;

                //Previous School Class.
                $scope.prevSchlCls = promise.allClass;

                //Previous School Year.
                $scope.prevYr = promise.academicYearOnLoad;

                //Previous School Country.
                $scope.prevCountry = promise.allCountry;
                $scope.obj.IVRMMC_Id = countryid;
                $scope.obj.IVRMMC_Id3 = countryid;
                $scope.obj.IVRMMC_Id5 = countryid;
                $scope.obj.amsT_ConCountry = countryid;
                $scope.streamlist = promise.masterstream;
                $scope.yearlist = promise.yearlist;
                $scope.scroll();

                if (admnocount === 0) {
                    swal("Transaction Numbering For Admission Number Not Updated, Kindly Update It And Proceed It");
                    return;
                }
                if (admregcount === 0) {
                    swal("Transaction Numbering For Register Number Not Updated , Kindly Update It And Proceed It");
                    return;
                }
            });
        };
        //webcam

        $scope.WebcamGenrate = function () {
            $scope.photoupload_flag = 'Webcam';
            $scope.ImagePath = "";
            $scope.wbcamurl = "";
            Webcam.snap;
            Webcam.snap(function (data_uri) {
                $scope.wbcamurl = data_uri;
            });


            if ($scope.wbcamurl != "") {
                $scope.reg.PASR_Student_Pic_Path = $scope.wbcamurl;
            }
            $scope.uploadStudentProfilePic($scope.wbcamurl);
        }
        //CHECKING REG NO DUPLICATE
        $scope.checkregnoduplicate = function () {
            var id = $scope.EditId;
            var data = {
                "AMST_Id": id,
                "admRegManualFlag": $scope.reg_,
                "admAdmManualFlag": $scope.Adm_,
                "preventdupRegNo": $scope.regpreventduplicateflag,
                "preventdupAdmNo": $scope.admpreventduplicateflag,
                "AMST_RegistrationNo": $scope.obj.amsT_RegistrationNo
            };

            apiService.create("StudentAdmission/checkDuplicate", data).then(function (promise) {
                if (promise.duplicateRegNo > 0) {
                    $scope.obj.amsT_RegistrationNo = "";
                    swal("Student Reg.No. Already Exists");
                    return;
                }
            });
        };

        // CHECKING ADMNO DUPLICATE
        $scope.checkadmnoduplicate = function () {
            var id = $scope.EditId;
            var data = {
                "AMST_Id": id,
                "admRegManualFlag": $scope.reg_,
                "admAdmManualFlag": $scope.Adm_,
                "preventdupRegNo": $scope.regpreventduplicateflag,
                "preventdupAdmNo": $scope.admpreventduplicateflag,
                "AMST_AdmNo": $scope.obj.amsT_AdmNo
            };
            apiService.create("StudentAdmission/checkDuplicate", data).then(function (promise) {
                if (promise.duplicateAdmNo > 0) {
                    $scope.obj.amsT_AdmNo = "";
                    swal("Student Admission No. Already Exists");
                    return;
                }
            });
        };

        //CALCULATING AGE , CONFIRMATION OF AGE IF THERE IS ANY LESSTHAN AND GREATHER THAN, AND CONVERTING DOB IN WORDS
        $scope.calcage = function (dateString, SweetAlert) {
            angular.forEach($scope.allClass, function (c) {
                if (parseInt(c.asmcL_Id) === parseInt($scope.obj.asmcL_Id)) {
                    $scope.asmcL_ClassName = c.asmcL_ClassName;
                }
            });

            $scope.classname11 = $scope.asmcL_ClassName;
            //converting date into words			
            var dobwords = new Date(dateString);
            var monthid = dobwords.getMonth();
            var dateid = dobwords.getDate();
            var yearid = dobwords.getFullYear();
            $scope.getmontnames(monthid);
            $scope.getdatenames(dateid);
            $scope.getyearname(yearid);
            $scope.obj.amsT_DOB_Words = datename + ' ' + monthname + ' ' + yearname;

            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());

            var yearNow = now.getYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();
            var doob = new Date(dateString);
            var dob = new Date(doob.getYear(6, 10),
                doob.getMonth(0, 2) - 1,
                doob.getDate(3, 5)
            );

            var yearDob = doob.getYear();
            var monthDob = doob.getMonth();
            var dateDob = doob.getDate();
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";

            var yearAge = "";
            var monthAge = "";
            var dateAge = "";

            yearAge = yearNow - yearDob;

            if (monthNow >= monthDob)
                monthAge = monthNow - monthDob;
            else {
                yearAge--;
                monthAge = 12 + monthNow - monthDob;
            }

            if (dateNow >= dateDob)
                dateAge = dateNow - dateDob;
            else {
                monthAge--;
                dateAge = 31 + dateNow - dateDob;

                if (monthAge < 0) {
                    monthAge = 11;
                    yearAge--;
                }
            }

            age = {
                years: yearAge,
                months: monthAge,
                days: dateAge
            };

            // swal(ageString);
            $scope.obj.pasR_Age = age.years;

            if (parseInt($scope.minage) === 1) {
                if (age.years > minageeyear) {
                    //dd
                }
                else if (parseInt(age.years) === parseInt(minageeyear)) {
                    if (age.months > minageemonth) {
                        //dd
                    }
                    else if (parseInt(age.months) === parseInt(minageemonth)) {
                        if (age.days >= minageedays) {
                            //dd
                        }
                        else {
                            swal({
                                title: "Less than " + minageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                                text: "Do you want to continue !!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                                cancelButtonText: "Cancel!!",
                                closeOnConfirm: true,
                                closeOnCancel: true
                            }, function (isConfirm) {
                                if (isConfirm) {
                                    //dd
                                }
                                else {
                                    $scope.$apply(function () {
                                        $scope.obj.pasR_Age = "";
                                        $scope.obj.amsT_DOB = "";
                                        $scope.obj.amsT_DOB_Words = "";
                                    });
                                }
                            });
                        }
                    }
                    else {
                        swal({
                            title: "Less than " + minageeyear + " year ,You are not eligible for " + $scope.classname11 + " class",
                            text: "Do you want to continue !!",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                            cancelButtonText: "Cancel!!",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        }, function (isConfirm) {
                            if (isConfirm) {
                                //dd
                            }
                            else {
                                $scope.$apply(function () {
                                    $scope.obj.pasR_Age = "";
                                    $scope.obj.amsT_DOB = "";
                                    $scope.obj.amsT_DOB_Words = "";
                                });
                            }
                        });
                    }
                }
                else {
                    swal({
                        title: "Less than " + minageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                        text: "Do you want to continue !!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                        cancelButtonText: "Cancel!!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            //dd
                        }
                        else {
                            $scope.$apply(function () {
                                $scope.obj.pasR_Age = "";
                                $scope.obj.amsT_DOB = "";
                                $scope.obj.amsT_DOB_Words = "";
                            });
                        }
                    });
                }
            }

            if (parseInt($scope.maxage) === 1) {
                if (age.years < maxageeyear) {
                    //dd
                }
                else if (parseInt(age.years) === parseInt(maxageeyear)) {
                    if (age.months < maxageemonth) {
                        //dd
                    }
                    else if (parseInt(age.months) === parseInt(maxageemonth)) {
                        if (age.days <= maxageedays) {
                            //dd
                        }
                        else {

                            swal({
                                title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                                text: "Do you want to continue !!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                                cancelButtonText: "Cancel!!",
                                closeOnConfirm: true,
                                closeOnCancel: true
                            }, function (isConfirm) {
                                if (isConfirm) {
                                    //dd
                                }
                                else {
                                    $scope.$apply(function () {
                                        $scope.obj.pasR_Age = "";
                                        $scope.obj.amsT_DOB = "";
                                        $scope.obj.amsT_DOB_Words = "";
                                    });
                                }
                            });
                        }
                    }
                    else {

                        swal({
                            title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                            text: "Do you want to continue !!",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                            cancelButtonText: "Cancel!!",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        }, function (isConfirm) {
                            if (isConfirm) {
                                //dd
                            }
                            else {
                                $scope.$apply(function () {
                                    $scope.obj.pasR_Age = "";
                                    $scope.obj.amsT_DOB = "";
                                    $scope.obj.amsT_DOB_Words = "";

                                });
                            }
                        });
                    }
                }
                else {

                    swal({
                        title: "Greater than " + maxageeyear + " year , You are not eligible for " + $scope.classname11 + " class",
                        text: "Do you want to continue !!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                        cancelButtonText: "Cancel!!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            //dd
                        }
                        else {
                            $scope.$apply(function () {
                                $scope.obj.pasR_Age = "";
                                $scope.obj.amsT_DOB = "";
                                $scope.obj.amsT_DOB_Words = "";
                            });
                        }
                    });
                }
            }
        };

        //
        $scope.OnChangeLIBYear = function (flag) {
            $scope.librarydetails = [];
            var data = {
                "flag": flag,
                "ASMAY_Id": $scope.LIBYearId,
                "OnClickOrOnChange": 'OnChange',
            };
            apiService.create("StudentDashboard/onclick_LIB", data).then(function (promise) {
                if (promise.librarydetails !== null && promise.librarydetails.length > 0) {
                    $scope.librarydetails = promise.librarydetails;
                    $('#myModalLibrary').modal('show');
                }
                //else {
                //    swal('No Data Found..!!');
                //}
            });
        };
        //CHECKING MIN AND MAX AGE DETAILS
        $scope.onclasschange = function (classid, yr, flg) {

            if (parseInt(flg) === 0) {
                $scope.obj.amsT_DOB = "";
            }

            if (classid !== "") {
                var data = {
                    "ASMCL_Id": classid,
                    "ASMAY_Id": yr,
                    "admflag": "adm"
                };

                apiService.create("StudentAdmission/classchangemaxminage", data).then(function (promise) {
                    if (promise.admclassCapacity === "MaxCapacity") {
                        swal("Sorry,Class Capacity is Full");
                        $scope.applastdatedisable = true;
                    }
                    else {
                        $scope.applastdatedisable = false;
                    }
                    if (parseInt(flg) === 0) {
                        $scope.DOB = false;
                    }
                    $scope.obj.stucategory = "";
                    if (promise.studentCategory.length > 0) {
                        $scope.obj.stucategory = promise.studentCategory[0].asmcC_Id;
                    }
                    else {
                        swal("To get Student Category.Please Map Selected Class to Some category");
                    }
                    maxageeyear = promise.fillclass[0].asmcL_MaxAgeYear;
                    maxageemonth = promise.fillclass[0].asmcL_MaxAgeMonth;
                    maxageedays = promise.fillclass[0].asmcL_MaxAgeDays;
                    minageeyear = promise.fillclass[0].asmcL_MinAgeYear;
                    minageemonth = promise.fillclass[0].asmcL_MinAgeMonth;
                    minageedays = promise.fillclass[0].asmcL_MinAgeDays;
                    if (parseInt(flg) === 0) {
                        if (parseInt($scope.maxage) === 0 && parseInt($scope.minage) === 0) {
                            $scope.maxDate1 = new Date(
                                $scope.myDate.getFullYear(),
                                $scope.myDate.getMonth(),
                                $scope.myDate.getDate());
                        }
                        else if (parseInt($scope.maxage) === 1 && parseInt($scope.minage) === 0) {
                            $scope.maxDate1 = new Date(
                                $scope.myDate.getFullYear(),
                                $scope.myDate.getMonth(),
                                $scope.myDate.getDate());
                        }
                        else if (parseInt($scope.maxage) === 0 && parseInt($scope.minage) === 1) {
                            $scope.maxDate1 = new Date(
                                $scope.myDate.getFullYear(),
                                $scope.myDate.getMonth(),
                                $scope.myDate.getDate());
                        }
                        else if (parseInt($scope.maxage) === 1 && parseInt($scope.minage) === 1) {
                            $scope.maxDate1 = new Date(
                                $scope.myDate.getFullYear(),
                                $scope.myDate.getMonth(),
                                $scope.myDate.getDate());
                        }
                    }

                    if (promise.electivelist_Groups != null && promise.electivelist_Groups.length) {
                        $scope.electivelist_Groups = promise.electivelist_Groups;
                    }
                    if (promise.electivelist != null && promise.electivelist.length) {
                        $scope.electivelist = promise.electivelist;
                    }
                });
            }
        };

        //STUDENT FORM VALIDATION
        $scope.submitted1 = false;
        $scope.isduplicate = false;
        $scope.ismobileNoconfirmed = true;
        $scope.isemailIdconfirmed = true;
        $scope.isaadharNoconfirmed = true;

        //ADDRESS FORM VALIDATION
        $scope.submitted2 = false;
        $scope.validateadd = function () {
            if ($scope.myForm2.$valid) {
                $scope.Parents = false;
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted2 = true;
                $scope.Parents = true;
            }
        };

        //PARENTS FORM VALIDATION
        $scope.submitted3 = false;
        $scope.validateParDetails = function () {
            if ($scope.myForm3.$valid) {
                $scope.Others = false;
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted3 = true;
                $scope.Others = true;
            }
        };

        //OTHERS FORM VALIDATION
        $scope.submitted4 = false;
        $scope.validateOtherdetails = function () {
            if ($scope.errorMessage !== "" && $scope.errorMessage !== null || $scope.errorMessage1 !== "" && $scope.errorMessage1 !== null || $scope.errorMessage2 !== "" && $scope.errorMessage2 !== null) {
                //dd
            }
            else {
                if ($scope.myForm4.$valid) {
                    $scope.DocumentUpload = false;
                    $scope.myTabIndex = $scope.myTabIndex + 1;
                }
                else {
                    $scope.submitted4 = true;
                    $scope.DocumentUpload = true;
                }
            }
        };

        $scope.submitted6 = false;


        //ADDING AND REMOVING SIBLING DETAILS
        $scope.studentSiblingDetails = [{ id: 'sibling1' }];
        $scope.addNewsibling = function () {
            var newItemNo = $scope.studentSiblingDetails.length + 1;
            if (newItemNo <= 5) {
                $scope.studentSiblingDetails.push({ 'id': 'sibling' + newItemNo });
            }
        };
        $scope.removeNewsibling = function (index) {
            var newItemNo = $scope.studentSiblingDetails.length - 1;
            $scope.studentSiblingDetails.splice(index, 1);
            if ($scope.studentSiblingDetails.length === 0) {
                //dd
            }
        };

        // ADDING AND REMOVING GUARDIAN DETAILS
        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.studentGuardianDetails.length + 1;
            if (newItemNo <= 5) {
                $scope.studentGuardianDetails.push({ 'id': 'Guardian' + newItemNo });
            }
        };
        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.studentGuardianDetails.length - 1;
            $scope.studentGuardianDetails.splice(index, 1);
            if ($scope.studentGuardianDetails.length === 0) {
                //dd
            }
        };

        // ADDING AND REMOVING PREVIOUS SCHOOL DETAILS
        $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
        $scope.addNewsiblingprevsch = function () {
            var newItemNo = $scope.prevSchoolDetails.length + 1;
            if (newItemNo <= 5) {
                $scope.prevSchoolDetails.push({ 'id': 'prevSchool' + newItemNo });
            }
        };
        $scope.removeNewsiblingprevsch = function (index, data) {
            var newItemNo = $scope.prevSchoolDetails.length - 1;
            $scope.prevSchoolDetails.splice(index, 1);
            if (data.amstB_Id > 0) {
                $scope.DeletePrevSchoolData(data);
            }
            if ($scope.prevSchoolDetails.length === 0) {
                //dd
            }
        };

        // ADDING AND REMOVING ACTIVITY LIST
        $scope.Activitycheckboxchcked = [];
        $scope.CheckedActivityName = function (data) {
            if ($scope.Activitycheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allActivity.length; i++) {
                    if ($scope.allActivity[i].Selected == true) {
                        $scope.Activitycheckboxchcked.push($scope.allActivity[i]);
                    }
                }
            }
            else {
                $scope.Activitycheckboxchcked.splice($scope.Activitycheckboxchcked.indexOf(data), 1);
            }
        };

        // ADDING AND REMOVING REFRENCE DETAILS
        $scope.Refrencecheckboxchcked = [];
        $scope.CheckedRefrenceName = function (data) {
            if ($scope.Refrencecheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    if ($scope.allRefrence[i].Selected == true) {
                        $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                    }
                }
            }
            else {
                $scope.Refrencecheckboxchcked.splice($scope.Refrencecheckboxchcked.indexOf(data), 1);
            }
        };

        // ADDING AND REMOVING SOURCE DETAILS
        $scope.Sourcescheckboxchcked = [];
        $scope.CheckedSourcesName = function (data) {
            if ($scope.Sourcescheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allSources.length; i++) {
                    if ($scope.allSources[i].Selected == true) {
                        $scope.Sourcescheckboxchcked.push($scope.allSources[i]);
                    }
                }
            }
            else {
                $scope.Sourcescheckboxchcked.splice($scope.Sourcescheckboxchcked.indexOf(data), 1);
            }
        };

        //GET SATATE BY COUNTRY
        $scope.onSelectGetState = function (IVRMMC_Id5) {
            var countryidd = IVRMMC_Id5;
            apiService.getURI("StudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState = promise.allState;
                $scope.obj.amsT_PerState = promise.allState[0].amsT_PerState;
                $scope.statelabel = true;
            });
        };
        //GET DISTRICT BY COUNTRY
        $scope.onSelectGetDistrict = function (amsT_PerState) {
            var stateidd = amsT_PerState;
            apiService.getURI("StudentAdmission/getdpdistrict", stateidd).then(function (promise) {
                $scope.allDistrict = promise.allDistrict;
                $scope.obj.amsT_PerDistrict = promise.allState[0].amsT_PerDistrict;
                $scope.districtlabel = true;
            });
        };

        $scope.onchangebithplacecountry = function (AMST_PlaceOfBirthCountry) {

            var countryidd = AMST_PlaceOfBirthCountry;
            apiService.getURI("StudentAdmission/onchangebithplacecountry", countryidd).then(function (promise) {

                $scope.allStateForBirthPlace = promise.allStateForBirthPlace;

                //$scope.statelabel = true;
            });
        };
        $scope.onchangenationality = function (IVRMMC_Id) {

            var countryidd = IVRMMC_Id;
            apiService.getURI("StudentAdmission/onchangenationality", countryidd).then(function (promise) {

                $scope.allStateonchangeNationality = promise.allStateonchangeNationality;

                //$scope.statelabel = true;
            });
        };


        //GET THE STATE NAME BY COUNTRY PRE SCHOOL
        $scope.onSelectGetStatepre = function (IVRMMC_Id5) {
            var countryidd = IVRMMC_Id5;
            apiService.getURI("StudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState12 = promise.allState;
                // $scope.obj.amsT_PerState = promise.allState[0].amsT_PerState;
                $scope.statelabel = true;
            });
        };

        // GET CITY BY STATE
        $scope.onSelectGetCity = function (ivrmmS_Id) {
            var stateId = ivrmmS_Id;
            apiService.getURI("StudentAdmission/getdpcities", stateId).then(function (promise) {
                $scope.allCity = promise.allCity;
            });
        };

        //GET SATATE BY COUNTRY
        $scope.onSelectGetState1 = function (IVRMMC_Id2) {
            var countryidd = IVRMMC_Id2;
            apiService.getURI("StudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState1 = promise.allState;
                $scope.obj.amsT_ConState = promise.allState[0].amsT_ConState;
                $scope.statelabel2 = true;
            });
        };


        //GET District BY State
        $scope.onSelectGetDistrict1 = function (amsT_ConState) {
            var stateidd = amsT_ConState;
            apiService.getURI("StudentAdmission/getdpdistrict", stateidd).then(function (promise) {
                $scope.allDistrict1 = promise.allDistrict;
                $scope.obj.amsT_ConDistrict = promise.allState[0].amsT_ConDistrict;
                $scope.districtlabel2 = true;
            });
        };

        // GET CITY BY STATE
        $scope.onSelectGetCity1 = function (ivrmmS_Id2) {
            var stateId = ivrmmS_Id2;
            apiService.getURI("StudentAdmission/getdpcities", stateId).then(function (promise) {
                $scope.allCity1 = promise.allCity;
            });
        };

        //GET PERMENENT ADDRESS STATE WHILE EDITING
        function getSelectGetState(countryidd, stateid) {
            apiService.getURI("StudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.obj.amsT_PerState = sts;
                $scope.data = promise.allState;
                $scope.allState.push.apply($scope.allState, $scope.data);
                $scope.statelabel = false;
            });
        }

        //GET PREVIOUS SCHOOL STATE WHILE EDITING.
        $scope.getPrevGetState = function (countryidd, stateid) {
            $scope.prevState = [];
            var data = {
                countryName: countryidd
            };

            apiService.create("StudentAdmission/StateByCountryName", data).then(function (promise) {
                var sts = stateid;
                $scope.prevSchool.amstpS_PreSchoolState = sts;
                $scope.prevState = promise.prevStateList;
            });
        };

        //GET RESIDENTIAL ADDRESS STATE WHILE EDITING
        function getSelectGetState2(countryidd, stateid) {
            apiService.getURI("StudentAdmission/getdpstate", countryidd).then(function (promise) {
                $scope.allState1 = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.obj.amsT_ConState = sts;

                $scope.data2 = promise.allState;
                $scope.allState1.push.apply($scope.allState1, $scope.data2);
                $scope.statelabel2 = false;
            });
        }
        //GET RESIDENTIAL ADDRESS DISTRICT WHILE EDITING
        function getSelectGetdistrict2(stateid) {
            var stateidd = stateid;
            apiService.getURI("StudentAdmission/getdpdistrict", stateidd).then(function (promise) {
                $scope.allDistrict1 = [{ "ivrmmD_Id": "", "ivrmmD_Name": "Select District" }];
                var sts = Number(distidd);
                $scope.obj.amsT_ConDistrict = sts;
                $scope.allDistrictt = promise.allDistrict;
                $scope.allDistrict1.push.apply($scope.allDistrict1, $scope.allDistrictt);

                //$scope.obj.amsT_PerState = sts;
                //$scope.data = promise.allState;
                //$scope.allState.push.apply($scope.allState, $scope.data);
                //$scope.statelabel = false;

                //$scope.obj.amsT_ConDistrict = promise.allDistrict1[0].amsT_ConDistrict;
                //$scope.districtlabel2 = true;
                // $scope.obj.amsT_ConState = sts;
            });

        }

        function getSelectGetdistrict1(stateid) {
            var stateidd = stateid;
            //var data = {
            //    countryName: stateid
            //};
            apiService.getURI("StudentAdmission/getdpdistrict", stateidd).then(function (promise) {
                //$scope.allDistrict = promise.allDistrict;
                //$scope.obj.amsT_PerDistrict = promise.allState[0].amsT_PerDistrict;


                $scope.allDistrict = [{ "ivrmmD_Id": "", "ivrmmD_Name": "Select District" }];
                $scope.allDistrict1 = promise.allDistrict;
                var sts = Number(distidd);
                $scope.obj.amsT_PerDistrict = sts;
                $scope.districtlabel = true;

            });
        }

        //GET STATE FOR FATHER NATIONALITY       
        $scope.onchangefathernationality = function (IVRMMC_Id3) {
            var countryidd = IVRMMC_Id3;
            var data = {
                "AMST_FatherNationality": countryidd
            };
            apiService.create("StudentAdmission/onchangefathernationality", data).then(function (promise) {
                if (promise !== null) {
                    $scope.fatherstatelist = promise.fatherstatelist;
                }
            });
        };

        //GET STATE FOR MOTHER NATIONALITY       
        $scope.onchangemothernationality = function (IVRMMC_Id4) {
            var countryidd = IVRMMC_Id4;
            var data = {
                "AMST_MotherNationality": countryidd
            };
            apiService.create("StudentAdmission/onchangemothernationality", data).then(function (promise) {
                if (promise !== null) {
                    $scope.motherstatelist = promise.motherstatelist;
                }
            });
        };

        $scope.address_copy = function () {
            if ($scope.obj.chkbox_address == 1) {
                $scope.obj.amsT_ConStreet = $scope.obj.amsT_PerStreet;
                $scope.obj.amsT_ConArea = $scope.obj.amsT_PerArea;
                $scope.obj.amsT_ConCountry = $scope.obj.IVRMMC_Id5;
                $scope.obj.amsT_ConDistrict = $scope.obj.amsT_PerDistrict;

                $scope.allState1 = [{ "ivrmmS_Id": "", "ivrmmS_Name": "Select State" }];
                var sts = Number($scope.obj.amsT_PerState);
                $scope.obj.amsT_ConState = sts;

                $scope.data2 = $scope.allState;
                $scope.allState1.push.apply($scope.allState1, $scope.data2);
                $scope.statelabel2 = false;
                $scope.obj.amsT_ConCity = $scope.obj.amsT_PerCity;
                $scope.obj.amsT_ConPincode = $scope.obj.amsT_PerPincode;
            }

            if ($scope.obj.chkbox_address == 0) {
                $scope.obj.amsT_ConStreet = "";
                $scope.obj.amsT_ConArea = "";
                $scope.obj.amsT_ConCountry = "";
                $scope.obj.amsT_ConState = "";
                $scope.obj.amsT_ConCity = "";
                $scope.obj.amsT_ConPincode = "";
                $scope.obj.amsT_ConDistrict = "";
            }
        };

        $scope.SelectedGovtBonds = [];

        $scope.getcaste = function (imcC_Id) {
            $scope.allCaste = [];
            $scope.obj.iC_Id = "";
            if (imcC_Id !== "") {
                var data = {
                    "IMCC_Id": imcC_Id
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("StudentAdmission/getCaste", data).then(function (promise) {
                    if (promise.allCaste.length > 0) {
                        $scope.allCaste = promise.allCaste;
                        $scope.castedisble = false;
                    }
                    else {
                        swal("No Caste is mapped to selected Caste Category");
                        $scope.castedisble = true;
                        $scope.obj.iC_Id = "";
                        $scope.allCaste = [];
                    }
                });
            }
        };

        $scope.getcaste1 = function (imcC_Id) {
            if (imcC_Id !== "") {
                var data = {
                    "IMC_Id": imcC_Id
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("StudentAdmission/getCaste", data).then(function (promise) {

                    if (promise.allCaste.length > 0) {
                        $scope.allCaste = promise.allCaste;
                        $scope.castedisble = false;
                    }
                    else {
                        swal("No Caste is mapped to selected Caste Category");
                        $scope.castedisble = true;
                        $scope.obj.iC_Id = "";
                        $scope.allCaste = [];
                    }
                });
            }
        };

        $scope.onselectprevCountry = function (amstpS_PreSchoolCountry, country) {
            var countryname = {
                "countryName": amstpS_PreSchoolCountry
            };

            apiService.create("StudentAdmission/StateByCountryName/", countryname).then(function (promise) {
                $scope.prevState = promise.prevStateList;

                country.prevState = promise.prevStateList;

            });
        };

        $scope.searchByColumn = function (search, searchColumn, searchyear) {

            if (search !== "" && search !== null && search !== undefined) {
                if (searchyear === null || searchyear === "") {
                    searchyear = 0;
                }
                var data = {
                    "EnteredData": search,
                    "SearchColumn": searchColumn,
                    "ASMAY_Id": searchyear
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("StudentAdmission/SearchByColumn", data).then(function (promise) {
                    if (promise.count === 0) {
                        swal("No Records Found");
                        $state.reload();
                    }
                    if (promise.message !== "" && promise.message !== null) {
                        swal(promise.message);
                        if (promise.adm_m_student !== null && promise.adm_m_student !== "") {
                            for (var i = 0; i < promise.adm_m_student.length; i++) {
                                if (promise.adm_m_student[i].amsT_SOL === "S") {
                                    promise.adm_m_student[i].amsT_SOL = "Active";
                                }
                                else if (promise.adm_m_student[i].amsT_SOL === "D") {
                                    promise.adm_m_student[i].amsT_SOL = "Deactive";
                                }
                                else if (promise.adm_m_student[i].amsT_SOL === "L") {
                                    promise.adm_m_student[i].amsT_SOL = "Left";
                                }
                                else if (promise.adm_m_student[i2].amsT_SOL === "WD") {
                                    promise.adm_m_student[i2].amsT_SOL = "Withdrawn";
                                }
                            }
                        }
                        $scope.adm_m_student = promise.adm_m_student;
                        $scope.presentCountgrid = $scope.adm_m_student.length;

                    }
                    else {
                        $scope.search = "";
                        if (promise.adm_m_student !== null && promise.adm_m_student !== "") {
                            for (var i = 0; i < promise.adm_m_student.length; i++) {
                                if (promise.adm_m_student[i].amsT_MiddleName === null || promise.adm_m_student[i].amsT_MiddleName === '') {
                                    promise.adm_m_student[i].amsT_MiddleName = "";
                                }

                                if (promise.adm_m_student[i].amsT_LastName === null || promise.adm_m_student[i].amsT_LastName === '') {
                                    promise.adm_m_student[i].amsT_LastName = "";
                                }
                                if (promise.adm_m_student[i].amsT_SOL === "S") {
                                    promise.adm_m_student[i].amsT_SOL = "Active";
                                }
                                else if (promise.adm_m_student[i].amsT_SOL === "D") {
                                    promise.adm_m_student[i].amsT_SOL = "Deactive";
                                }
                                else if (promise.adm_m_student[i].amsT_SOL === "L") {
                                    promise.adm_m_student[i].amsT_SOL = "Left";
                                }
                            }
                        }
                        $scope.adm_m_student = promise.adm_m_student;
                        $scope.presentCountgrid = $scope.adm_m_student.length;
                    }
                });
            }
            else {
                swal("Please Enter Value To Be Searched In Search here.....Text Box  And Then Click On Search Icon");
            }
        };

        $scope.getcolumnId = function (ColumnId) {
            if (ColumnId === "14") {
                swal("Enter data in for ex: 2015-2016 format");
            }
        };

        $scope.guardianNameEntered = function (guardianDet, index) {

            if ($scope.EditId > 0) {
                if (parseInt($scope.grddetcount) === 0) {
                    if ($scope.studentGuardianDetails[index].amstG_GuardianName === "" || $scope.studentGuardianDetails[index].amstG_GuardianName === null || $scope.studentGuardianDetails[index].amstG_GuardianName === undefined) {
                        $scope.studentGuardianDetails[index].amstG_GuardianAddress = "";
                        $scope.studentGuardianDetails[index].amstG_emailid = "";
                        $scope.studentGuardianDetails[index].amstG_GuardianPhoneNo = "";
                    }
                }
                else {
                    if ($scope.studentGuardianDetails[index].amstG_GuardianName === "" || $scope.studentGuardianDetails[index].amstG_GuardianName === null || $scope.studentGuardianDetails[index].amstG_GuardianName === undefined) {
                        $scope.errorMessage = 'Guardian Name Required';
                    }
                    else {
                        $scope.errorMessage = '';
                    }
                }
            }
            else {
                if ($scope.studentGuardianDetails[index].amstG_GuardianName === "" || $scope.studentGuardianDetails[index].amstG_GuardianName === null || $scope.studentGuardianDetails[index].amstG_GuardianName === undefined) {
                    $scope.studentGuardianDetails[index].amstG_GuardianAddress = "";
                    $scope.studentGuardianDetails[index].amstG_emailid = "";
                    $scope.studentGuardianDetails[index].amstG_GuardianPhoneNo = "";
                }
            }
        };

        $scope.siblingNameEntered = function (index) {
            if ($scope.EditId > 0) {
                if (parseInt($scope.sibcount) === 0) {
                    if ($scope.studentSiblingDetails[index].amstS_SiblingsName === "" || $scope.studentSiblingDetails[index].amstS_SiblingsName === null || $scope.studentSiblingDetails[index].amstS_SiblingsName === undefined) {
                        $scope.studentSiblingDetails[index].amcL_Id = "";
                        $scope.studentSiblingDetails[index].amstS_SiblingsRelation = "";
                    }
                }
                else {
                    if ($scope.studentSiblingDetails[index].amstS_SiblingsName === "" || $scope.studentSiblingDetails[index].amstS_SiblingsName === null || $scope.studentSiblingDetails[index].amstS_SiblingsName === undefined) {
                        $scope.errorMessage1 = 'Sibling Name Required';
                    }
                    else {
                        $scope.errorMessage1 = '';
                    }
                }
            }
            else {
                if ($scope.studentSiblingDetails[index].amstS_SiblingsName === "" || $scope.studentSiblingDetails[index].amstS_SiblingsName === null || $scope.studentSiblingDetails[index].amstS_SiblingsName === undefined) {
                    $scope.studentSiblingDetails[index].amcL_Id = "";
                    $scope.studentSiblingDetails[index].amstS_SiblingsRelation = "";

                }
            }
        };

        $scope.prevSchoolNameEntered = function (index) {
            if ($scope.EditId > 0) {
                if (parseInt($scope.prevschlcount) === 0) {
                    if ($scope.prevSchoolDetails[index].amstpS_PrvSchoolName === "" || $scope.prevSchoolDetails[index].amstpS_PrvSchoolName === null || $scope.prevSchoolDetails[index].amstpS_PrvSchoolName === undefined) {
                        $scope.prevSchoolDetails[index].amstpS_PreSchoolType = "";
                        $scope.prevSchoolDetails[index].amstpS_PreviousClass = "";
                        $scope.prevSchoolDetails[index].amstpS_PreviousPer = "";
                        $scope.prevSchoolDetails[index].amstpS_PreviousGrade = "";
                        $scope.prevSchoolDetails[index].amstpS_LeftYear = "";
                        $scope.prevSchoolDetails[index].amstpS_PreviousMarks = "";
                        $scope.prevSchoolDetails[index].amstpS_PreSchoolBoard = "";
                        $scope.prevSchoolDetails[index].amstpS_PreSchoolCountry = "";
                        $scope.prevSchoolDetails[index].amstpS_PreSchoolState = "";
                        $scope.prevSchoolDetails[index].amstpS_Address = "";
                        $scope.prevSchoolDetails[index].amstpS_LeftReason = "";
                        $scope.prevSchoolDetails[index].amstpS_MediumOfInst = "";
                        $scope.prevSchoolDetails[index].amstpS_ConcOrScholarshipFlg = "";
                        $scope.prevSchoolDetails[index].amstpS_ConcOrScholarshipDate = "";
                        $scope.prevSchoolDetails[index].amstpS_PrvTCNO = "";
                        $scope.prevSchoolDetails[index].amstpS_PrvTCDate = "";
                    }
                }
                else {
                    if ($scope.prevSchoolDetails[index].amstpS_PrvSchoolName === "" || $scope.prevSchoolDetails[index].amstpS_PrvSchoolName === null || $scope.prevSchoolDetails[index].amstpS_PrvSchoolName === undefined) {
                        $scope.errorMessage2 = 'School Name Required';
                    }
                    else {
                        $scope.errorMessage2 = '';
                    }
                }
            }
            else {
                if ($scope.prevSchoolDetails[index].amstpS_PrvSchoolName === "" || $scope.prevSchoolDetails[index].amstpS_PrvSchoolName === null || $scope.prevSchoolDetails[index].amstpS_PrvSchoolName === undefined) {
                    $scope.prevSchoolDetails[index].amstpS_PreSchoolType = "";
                    $scope.prevSchoolDetails[index].amstpS_PreviousClass = "";
                    $scope.prevSchoolDetails[index].amstpS_PreviousPer = "";
                    $scope.prevSchoolDetails[index].amstpS_PreviousGrade = "";
                    $scope.prevSchoolDetails[index].amstpS_LeftYear = "";
                    $scope.prevSchoolDetails[index].amstpS_PreviousMarks = "";
                    $scope.prevSchoolDetails[index].amstpS_PreSchoolBoard = "";
                    $scope.prevSchoolDetails[index].amstpS_PreSchoolCountry = "";
                    $scope.prevSchoolDetails[index].amstpS_PreSchoolState = "";
                    $scope.prevSchoolDetails[index].amstpS_Address = "";
                    $scope.prevSchoolDetails[index].amstpS_LeftReason = "";
                    $scope.prevSchoolDetails[index].amstpS_MediumOfInst = "";
                    $scope.prevSchoolDetails[index].amstpS_ConcOrScholarshipFlg = "";
                    $scope.prevSchoolDetails[index].amstpS_ConcOrScholarshipDate = "";
                    $scope.prevSchoolDetails[index].amstpS_PrvTCNO = "";
                    $scope.prevSchoolDetails[index].amstpS_PrvTCDate = "";

                }
            }
        };

        //adding multiple mobile number father
        $scope.mobiles = [{ id: 'mobile1' }];
        $scope.selmobs = [{ id: 'selmobile1' }];
        $scope.addNewMobile = function () {
            var newItemNo = $scope.mobiles.length + 1;
            if (newItemNo <= 5) {
                $scope.mobiles.push({ 'id': 'mobile' + newItemNo });
            }
            var newItemNo1 = $scope.selmobs.length + 1;
            if (newItemNo1 <= 5) {
                $scope.selmobs.push({ 'id': 'selmobile' + newItemNo1 });
            }
        };
        $scope.delm = [];
        $scope.removeNewMobile = function (index, curval1) {
            var newItemNo = $scope.mobiles.length - 1;
            if (newItemNo !== 0) {
                $scope.delm = $scope.mobiles.splice(index, 1);
            }
        };

        //adding email id  father
        $scope.emails = [{ id: 'email1' }];
        $scope.addNewEmail = function () {
            var newItemNo = $scope.emails.length + 1;
            if (newItemNo <= 5) {
                $scope.emails.push({ 'id': 'email1' + newItemNo });
            }
        };
        $scope.removeNewEmail = function (index) {
            var newItemNo = $scope.emails.length - 1;
            if (newItemNo !== 0) {
                $scope.delm = $scope.emails.splice(index, 1);
            }
        };
        $scope.showAddEmail = function (email) {
            return email.id === $scope.emails[$scope.emails.length - 1].id;
        };

        //adding mobile number mother
        $scope.mobiles1 = [{ id: 'mobile2' }];
        $scope.selmobs1 = [{ id: 'selmobile2' }];
        $scope.addNewMobile1 = function () {
            var newItemNo2 = $scope.mobiles1.length + 1;
            if (newItemNo2 <= 5) {
                $scope.mobiles1.push({ 'id': 'mobile2' + newItemNo2 });
            }
            var newItemNo3 = $scope.selmobs1.length + 1;
            if (newItemNo3 <= 5) {
                $scope.selmobs1.push({ 'id': 'selmobile2' + newItemNo3 });
            }
        };
        $scope.delm1 = [];
        $scope.removeNewMobile1 = function (index, curval11) {
            var newItemNo2 = $scope.mobiles1.length - 1;
            if (newItemNo2 !== 0) {
                $scope.delm1 = $scope.mobiles1.splice(index, 1);
            }
        };

        //adding email id mother
        $scope.emails1 = [{ id: 'email2' }];
        $scope.addNewEmail1 = function () {
            var newItemNo2 = $scope.emails1.length + 1;
            if (newItemNo2 <= 5) {
                $scope.emails1.push({ 'id': 'email' + newItemNo2 });
            }
        };
        $scope.removeNewEmail1 = function (index) {
            var newItemNo = $scope.emails1.length - 1;
            if (newItemNo !== 0) {
                $scope.delm1 = $scope.emails1.splice(index, 1);
            }
        };
        $scope.showAddEmail1 = function (email) {
            return email.id === $scope.emails1[$scope.emails1.length - 1].id;
        };

        //adding mobile number student
        $scope.mobilesstd = [{ id: 'mobilestd1' }];
        $scope.selmobsstd = [{ id: 'selmobilestd1' }];

        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.mobilesstd.length + 1;
            if (newItemNostd <= 5) {
                $scope.mobilesstd.push({ 'id': 'mobilestd1' + newItemNostd });
            }
            if (newItemNostd == 4) {
                $scope.myForm1.$setPristine();
            }
        };

        $scope.addtopdays = function (user, index) {
            for (var k = 0; k < $scope.mobilesstd.length; k++) {
                var roll = parseInt(user.amsT_MobileNo);
                var arryind = $scope.mobilesstd.indexOf($scope.mobilesstd[k]);
                console.log(arryind);
                if (arryind != index) {
                    if ($scope.mobilesstd[k].amsT_MobileNo == roll) {
                        swal("Already Exist");
                        $scope.mobilesstd[index].amsT_MobileNo = "";
                        break;
                    }
                }
            }
        };

        //removing mobile number student
        $scope.delmsrd = [];
        $scope.removeNewMobile1std = function (index, curval1std) {
            var newItemNostd2 = $scope.mobilesstd.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.mobilesstd.splice(index, 1);
            }
        };

        //adding email id student
        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd1' + newItemNostd2 });
            }
        };
        $scope.removeNewEmail1std = function (index) {
            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);
            }
        };
        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };
        $scope.removeall1 = function () {
            $('#myModalecs').modal('hide');
            $scope.submittedecs = false;
            $scope.myFormecs.$setPristine();
            $scope.myFormecs.$setUntouched();
        };

        $scope.objee = [];
        $scope.objj = [];

        $scope.ecsdetailscheck = function (objj) {
            if ($scope.ecsdetailslist.length === 0) {
                $('#myModalecs').modal('hide');
                return;
            }

            if (objj == false) {
                swal({
                    title: "Do You Want To Delete The ECS Details Of This Student",
                    text: "Do you want to continue !!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                    cancelButtonText: "Cancel!!",
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        $('#myModalecs').modal('hide');
                        $scope.ecsdetailslist = [];
                        $scope.submittedecs = false;
                        $scope.myFormecs.$setPristine();
                        $scope.myFormecs.$setUntouched();

                    }
                    else {
                        $('#myModalecs').modal('show');
                        $scope.obj.amsT_ECSFlag = true;
                    }
                });

            } else {
                //dd
            }
        };

        $scope.interactedecs = function (field) {
            return $scope.submittedecs;
        };
        $scope.submittedecs = false;
        $scope.ecsdetailslist = [];

        $scope.addtocartecs = function (objee) {
            if ($scope.myFormecs.$valid) {
                if (objee.asecS_Id == undefined) {
                    objee.asecS_Id = 0;
                }
                $scope.ecsdetailslist = objee;
                $('#myModalecs').modal('hide');
            }
            else {
                $scope.submittedecs = true;
            }
        };

        $scope.checkbiometriccode = function () {
            var id = 0;
            if ($scope.EditId == undefined) {
                id = 0;
            } else {
                id = $scope.EditId;
            }

            var data = {
                "AMST_BiometricId": $scope.obj.amsT_BiometricId,
                "AMST_Id": id
            };
            apiService.create("StudentAdmission/checkbiometriccode", data).then(function (promise) {
                if (promise.message === "Duplicate") {
                    swal("Biometric Code Already Exists");
                    $scope.obj.amsT_BiometricId = "";
                }
            });
        };

        $scope.checkrfcardduplicate = function () {
            var id = 0;
            if ($scope.EditId == undefined) {
                id = 0;
            } else {
                id = $scope.EditId;
            }

            var data = {
                "AMST_RFCardNo": $scope.obj.amsT_RFCardNo,
                "AMST_Id": id
            };
            apiService.create("StudentAdmission/checkrfcardduplicate", data).then(function (promise) {
                if (promise.message === "Duplicate") {
                    swal("RF Card Number Already Exists");
                    $scope.obj.amsT_RFCardNo = "";
                }
            });
        };


        //First Tab Saving
        $scope.savefirsttab = function () {

            if ($scope.myForm1.$valid) {
                var amsT_BPLCardFlag = 0;
                var amsT_ECSFlag = 0;
                var id = 0;
                if ($scope.obj.amsT_BPLCardFlag === true) {
                    amsT_BPLCardFlag = 1;
                } else {
                    amsT_BPLCardFlag = 0;
                }

                if ($scope.obj.amsT_ECSFlag === true) {
                    amsT_ECSFlag = 1;
                }
                else {
                    amsT_ECSFlag = 0;
                }
                if ($scope.obj.amsT_MobileNo === undefined) {
                    $scope.obj.amsT_MobileNo = 0;
                }


                if ($scope.obj.amsT_FirstName === null || $scope.obj.amsT_FirstName === undefined || $scope.obj.amsT_FirstName === "") {
                    $scope.obj.amsT_FirstName = "";
                } else {
                    $scope.obj.amsT_FirstName = $scope.obj.amsT_FirstName;
                }
                if ($scope.obj.ASMCE_Id == undefined || $scope.obj.ASMCE_Id == null || $scope.obj.ASMCE_Id == "") {
                    $scope.obj.ASMCE_Id = null;
                }
                if ($scope.obj.amsT_MiddleName === null || $scope.obj.amsT_MiddleName === undefined || $scope.obj.amsT_MiddleName === "") {
                    $scope.obj.amsT_MiddleName = "";
                } else {
                    $scope.obj.amsT_MiddleName = $scope.obj.amsT_MiddleName;
                }

                if ($scope.obj.amsT_LastName === null || $scope.obj.amsT_LastName === undefined || $scope.obj.amsT_LastName === "") {
                    $scope.obj.amsT_LastName = "";
                } else {
                    $scope.obj.amsT_LastName = $scope.obj.amsT_LastName;
                }
                if ($scope.obj.amst_GovtAdmno === null) {
                    $scope.obj.amst_GovtAdmno = '';
                }
                if ($scope.obj.amsT_GPSTrackingId === null) {
                    $scope.obj.amsT_GPSTrackingId = '';
                }


                var amsT_Date = new Date($scope.amsT_Date).toDateString();
                var amsT_DOB = new Date($scope.obj.amsT_DOB).toDateString();

                if ($scope.EditId == undefined || $scope.EditId == null || $scope.EditId == "") {
                    id = 0;
                } else {
                    id = $scope.EditId;
                }
                var sectionid = 0;
                if ($scope.obj.asmS_Id !== undefined && $scope.obj.asmS_Id !== null && $scope.obj.asmS_Id !== "") {
                    sectionid = $scope.obj.asmS_Id;
                } else {
                    sectionid = 0;
                }

                var data = {
                    "AMST_Id": id,
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "AMST_FirstName": $scope.obj.amsT_FirstName,
                    "AMST_MiddleName": $scope.obj.amsT_MiddleName,
                    "AMST_LastName": $scope.obj.amsT_LastName,
                    "AMST_Date": amsT_Date,
                    "AMST_RegistrationNo": $scope.obj.amsT_RegistrationNo,
                    "AMST_AdmNo": $scope.obj.amsT_AdmNo,
                    "IMCC_Id": $scope.obj.imcC_Id,
                    "IVRMMR_Id": $scope.obj.ivrmmR_Id,
                    "IMC_Id": $scope.obj.iC_Id.imC_Id,
                    "AMST_Sex": $scope.amsT_Sex,
                    "AMST_DOB": amsT_DOB,
                    "AMST_DOB_Words": $scope.obj.amsT_DOB_Words,
                    "PASR_Age": $scope.obj.pasR_Age,
                    "ASMCL_Id": $scope.obj.asmcL_Id,
                    "AMST_BloodGroup": $scope.obj.amsT_BloodGroup,
                    "AMST_MotherTongue": $scope.obj.amsT_MotherTongue,
                    "AMST_LanguageSpoken": $scope.obj.amsT_LanguageSpoken,
                    "AMST_BirthCertNO": $scope.obj.amsT_BirthCertNO,
                    "AMST_AadharNo": $scope.obj.amsT_AadharNo,
                    "AMST_StuBankAccNo": $scope.obj.amsT_StuBankAccNo,
                    "AMST_BankName": $scope.obj.AMST_BankName,
                    "AMST_BranchName": $scope.obj.AMST_BranchName,
                    "AMST_StuBankIFSC_Code": $scope.obj.amsT_StuBankIFSC_Code,
                    "AMST_StuCasteCertiNo": $scope.obj.amsT_StuCasteCertiNo,
                    "AMST_StudentPANNo": $scope.obj.amsT_StudentPANNo,
                    "AMC_Id": $scope.obj.stucategory,
                    "AMST_BirthPlace": $scope.obj.amsT_BirthPlace,
                    "AMST_Nationality": $scope.obj.IVRMMC_Id,

                    "AMST_State": $scope.obj.AMST_State,
                    "AMST_BPLCardFlag": amsT_BPLCardFlag,
                    "AMST_BPLCardNo": $scope.obj.amsT_BPLCardNo,
                    "AMST_ECSFlag": amsT_ECSFlag,
                    "AMST_Concession_Type": $scope.obj.feeconcession,
                    "AMST_GPSTrackingId": $scope.obj.amsT_GPSTrackingId,
                    "Adm_M_Student_MobileNoDTO": $scope.mobilesstd,
                    "Adm_M_Student_EmailIdDTO": $scope.emailsstd,
                    "AMST_SubCasteIMC_Id": $scope.obj.amsT_SubCasteIMC_Id,
                    "AMST_Tribe": $scope.obj.amsT_Tribe,
                    "AMST_Photoname": $scope.obj.image,
                    "AMST_MobileNo": $scope.obj.amsT_MobileNo,
                    "AMST_emailId": $scope.obj.amsT_emailId,
                    "transnumconfigsettings": $scope.RegistrationNumbering,
                    "admissionNumbering": $scope.AdmissionNumbering,
                    "AMST_GovtAdmno": $scope.obj.amsT_GovtAdmno,
                    "ASMST_Id": $scope.obj.asmsT_Id,
                    "AMST_MOInstruction": $scope.obj.amsT_MOInstruction,
                    "AMST_BiometricId": $scope.obj.amsT_BiometricId,
                    "AMST_RFCardNo": $scope.obj.amsT_RFCardNo,
                    "AMST_AppDownloadedDeviceId": $scope.AMST_AppDownloadedDeviceId,
                    "ASMS_Id": sectionid

                    //"Adm_M_Student_ECS": $scope.ecsdetailslist
                };
                if ($scope.obj.amsT_ECSFlag === true) {
                    data.Adm_M_Student_ECS = $scope.ecsdetailslist;
                }


                apiService.create("StudentAdmission/savefirsttab", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                            $scope.EditId = promise.amsT_Id;
                            $scope.address = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.scroll();
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Update Successfully");
                            $scope.EditId = promise.amsT_Id;
                            $scope.address = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.scroll();
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }

                    else if (promise.message === "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        $scope.submitted1 = true;
                        $scope.address = true;

                    }
                });
            }
            else {
                $scope.submitted1 = true;
                $scope.address = true;
            }
        };

        //Second Tab Saving
        $scope.savesecondtab = function () {
            if ($scope.myForm2.$valid) {
                var id = $scope.EditId;
                var data = {
                    "AMST_Id": $scope.EditId,
                    "AMST_PerStreet": $scope.obj.amsT_PerStreet,
                    "AMST_PerArea": $scope.obj.amsT_PerArea,
                    "AMST_PerCountry": $scope.obj.IVRMMC_Id5,
                    "AMST_PerState": $scope.obj.amsT_PerState,
                    "AMST_PerDistrict": $scope.obj.amsT_PerDistrict,
                    "AMST_PerCity": $scope.obj.amsT_PerCity,
                    "AMST_PerPincode": $scope.obj.amsT_PerPincode,
                    "AMST_ConStreet": $scope.obj.amsT_ConStreet,
                    "AMST_ConArea": $scope.obj.amsT_ConArea,
                    "AMST_ConCountry": $scope.obj.amsT_ConCountry,
                    "AMST_ConState": $scope.obj.amsT_ConState,
                    "AMST_ConDistrict": $scope.obj.amsT_ConDistrict,
                    "AMST_ConCity": $scope.obj.amsT_ConCity,
                    "AMST_ConPincode": $scope.obj.amsT_ConPincode
                };

                apiService.create("StudentAdmission/savesecondtab", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                            $scope.EditId = promise.amsT_Id;
                            $scope.Parents = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Update Successfully");
                            $scope.EditId = promise.amsT_Id;
                            $scope.Parents = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }

                    else if (promise.message === "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        $scope.submitted2 = true;
                        $scope.Parents = true;
                    }
                });
            }
            else {
                $scope.submitted2 = true;
                $scope.Parents = true;
            }
        };

        //Third Tab Saving
        $scope.savethirdtab = function () {
            if ($scope.myForm3.$valid) {
                if ($scope.obj.amsT_FatherAliveFlag === true) {
                    $scope.obj.amsT_FatherAliveFlag = "true";
                }
                else {
                    $scope.obj.amsT_FatherAliveFlag = "false";
                }

                if ($scope.obj.amsT_MotherAliveFlag === true) {
                    $scope.obj.amsT_MotherAliveFlag = "true";
                }
                else {
                    $scope.obj.amsT_MotherAliveFlag = "false";
                }

                var id = $scope.EditId;
                var fathermobileno = $scope.mobiles;
                var fatheremailid = $scope.emails;
                var mothermobileno = $scope.mobiles1;
                var motheremailid = $scope.emails1;

                var data = {
                    "AMST_Id": $scope.EditId,
                    "AMST_FatherAliveFlag": $scope.obj.amsT_FatherAliveFlag,
                    "AMST_FatherName": $scope.obj.amsT_FatherName,
                    "AMST_FatherSurname": $scope.obj.amsT_FatherSurname,
                    "AMST_FatherAadharNo": $scope.obj.amsT_FatherAadharNo,
                    "AMST_FatherEducation": $scope.obj.amsT_FatherEducation,
                    "AMST_FatherMaritalStatus": $scope.obj.amsT_FatherMaritalStatus,
                    "AMST_FatherOfficeAdd": $scope.obj.amsT_FatherOfficeAdd,

                    "AMST_FatherPresentAddress": $scope.obj.amsT_FatherPresentAddress,
                    "AMST_FatherPresentCity": $scope.obj.amsT_FatherPresentCity,
                    "AMST_FatherPresentState": $scope.obj.amsT_FatherPresentState,
                    "AMST_FatherPresentPS": $scope.obj.amsT_FatherPresentPS,
                    "AMST_FatherPresentPO": $scope.obj.amsT_FatherPresentPO,
                    "AMST_FatherPresentPinCode": $scope.obj.amsT_FatherPresentPinCode,

                    "AMST_FatherPermanentAddress": $scope.obj.amsT_FatherPermanentAddress,
                    "AMST_FatherPermanentCity": $scope.obj.amsT_FatherPermanentCity,
                    "AMST_FatherPermanentState": $scope.obj.amsT_FatherPermanentState,
                    "AMST_FatherPermanentPS": $scope.obj.amsT_FatherPermanentPS,
                    "AMST_FatherPermanentPO": $scope.obj.amsT_FatherPermanentPO,
                    "AMST_FatherPermanentPinCode": $scope.obj.amsT_FatherPermanentPinCode,

                    "AMST_FatherOccupation": $scope.obj.amsT_FatherOccupation,
                    "AMST_FatherDesignation": $scope.obj.amsT_FatherDesignation,
                    "AMST_FatherBankName": $scope.obj.amsT_FatherBankName,
                    "AMST_FatherBankBranch": $scope.obj.amsT_FatherBankBranch,
                    "AMST_FatherBankAccNo": $scope.obj.amsT_FatherBankAccNo,
                    "AMST_FatherBankIFSC_Code": $scope.obj.amsT_FatherBankIFSC_Code,
                    "AMST_FatherCasteCertiNo": $scope.obj.amsT_FatherCasteCertiNo,
                    "AMST_FatherHomePhNo": $scope.obj.amsT_FatherHomePhNo,
                    "AMST_FatherPassingYear": $scope.obj.amsT_FatherPassingYear,
                    "AMST_FatherOfficePhNo": $scope.obj.amsT_FatherOfficePhNo,
                    "AMST_FatherTribe": $scope.obj.amsT_FatherTribe,
                    "AMST_FatherNationality": $scope.obj.IVRMMC_Id3,
                    "AMST_FatherReligion": $scope.obj.amsT_FatherReligion,
                    "AMST_FatherCaste": $scope.obj.amsT_FatherCaste,
                    "AMST_FatherSubCaste": $scope.obj.amsT_FatherSubCaste,
                    "AMST_FatherMonIncome": $scope.obj.amsT_FatherMonIncome,
                    "AMST_FatherAnnIncome": $scope.obj.amsT_FatherAnnIncome,
                    "AMST_FatherMobleNo": $scope.obj.amsT_FatherMobleNo,
                    "AMST_FatheremailId": $scope.obj.amsT_FatheremailId,
                    "AMST_FatherPANNo": $scope.obj.amsT_FatherPANNo,
                    "AMST_FatherChurchAffiliation": $scope.obj.amsT_FatherChurchAffiliation,
                    "AMST_FatherSelfEmployedFlg": $scope.obj.amsT_FatherSelfEmployedFlg,

                    //Mother details
                    "MotherAlive": $scope.obj.amsT_MotherAliveFlag,
                    "MotherName": $scope.obj.amsT_MotherName,
                    "MotherSurname": $scope.obj.amsT_MotherSurname,
                    "MotherAadharNo": $scope.obj.amsT_MotherAadharNo,
                    "MotherEducation": $scope.obj.amsT_MotherEducation,
                    "MotherOfficesAddress": $scope.obj.amsT_MotherOfficeAdd,

                    "AMST_MotherMaritalStatus": $scope.obj.amsT_MotherMaritalStatus,

                    "AMST_MotherPresentAddress": $scope.obj.amsT_MotherPresentAddress,
                    "AMST_MotherPresentCity": $scope.obj.amsT_MotherPresentCity,
                    "AMST_MotherPresentState": $scope.obj.amsT_MotherPresentState,
                    "AMST_MotherPresentPS": $scope.obj.amsT_MotherPresentPS,
                    "AMST_MotherPresentPO": $scope.obj.amsT_MotherPresentPO,
                    "AMST_MotherPresentPinCode": $scope.obj.amsT_MotherPresentPinCode,

                    "AMST_MotherPermanentAddress": $scope.obj.amsT_MotherPermanentAddress,
                    "AMST_MotherPermanentCity": $scope.obj.amsT_MotherPermanentCity,
                    "AMST_MotherPermanentState": $scope.obj.amsT_MotherPermanentState,
                    "AMST_MotherPermanentPS": $scope.obj.amsT_MotherPermanentPS,
                    "AMST_MotherPermanentPO": $scope.obj.amsT_MotherPermanentPO,
                    "AMST_MotherPermanentPinCode": $scope.obj.amsT_MotherPermanentPinCode,

                    "MotherOcupation": $scope.obj.amsT_MotherOccupation,
                    "MotherDesignation": $scope.obj.amsT_MotherDesignation,

                    "AMST_MotherBankBranch": $scope.obj.amsT_MotherBankBranch,
                    "AMST_MotherBankName": $scope.obj.amsT_MotherBankName,
                    "MotherBankAccountNo": $scope.obj.amsT_MotherBankAccNo,
                    "MotherIFSCcode": $scope.obj.amsT_MotherBankIFSC_Code,

                    "MotherCasteCertificateNo": $scope.obj.amsT_MotherCasteCertiNo,
                    "AMST_MotherReligion": $scope.obj.amsT_MotherReligion,
                    "AMST_MotherCaste": $scope.obj.amsT_MotherCaste,
                    "AMST_MotherSubCaste": $scope.obj.amsT_MotherSubCaste,
                    "MotherMonthlyIncome": $scope.obj.amsT_MotherMonIncome,
                    "MotherAnnualIncome": $scope.obj.amsT_MotherAnnIncome,
                    "AMST_MotherPANNo": $scope.obj.amsT_MotherPANNo,
                    "AMST_MotherMobileNo": $scope.obj.amsT_MotherMobileNo,
                    "AMST_MotherEmailId": $scope.obj.amsT_MotherEmailId,
                    "AMST_MotherChurchAffiliation": $scope.obj.amsT_MotherChurchAffiliation,
                    "AMST_MotherSelfEmployedFlg": $scope.obj.amsT_MotherSelfEmployedFlg,

                    "AMST_MotherOfficePhNo": $scope.obj.amsT_MotherOfficePhNo,
                    "AMST_MotherHomePhNo": $scope.obj.amsT_MotherHomePhNo,
                    "AMST_MotherTribe": $scope.obj.amsT_MotherTribe,
                    "AMST_MotherPassingYear": $scope.obj.amsT_MotherPassingYear,

                    "AMST_MotherNationality": $scope.obj.IVRMMC_Id4,
                    "multiplemobileno": fathermobileno,
                    "multipleemailid": fatheremailid,
                    "multiplemobilenomother": mothermobileno,
                    "multipleemailidmother": motheremailid
                };

                apiService.create("StudentAdmission/savethirdtab", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                            $scope.EditId = promise.amsT_Id;
                            $scope.Others = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.scroll();
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Update Successfully");
                            $scope.EditId = promise.amsT_Id;
                            $scope.Others = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.scroll();
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message === "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        $scope.submitted3 = true;
                        $scope.Others = true;

                    }
                });
            }
            else {
                $scope.submitted3 = true;
                $scope.Others = true;
            }
        };

        //Fourth Tab Saving
        $scope.savefourthtab = function () {

            if ($scope.myForm4.$valid) {
                var id = $scope.EditId;
                if ($scope.allActivity !== "" && $scope.allActivity !== null) {
                    if ($scope.allActivity.length > 0) {
                        for (var i = 0; i < $scope.allActivity.length; i++) {
                            if ($scope.allActivity[i].Selected === true) {
                                $scope.Activitycheckboxchcked.push($scope.allActivity[i]);
                            }
                        }
                    }
                }

                if ($scope.allRefrence !== "" && $scope.allRefrence !== null) {
                    if ($scope.allRefrence.length > 0) {
                        for (var i = 0; i < $scope.allRefrence.length; i++) {
                            if ($scope.allRefrence[i].Selected === true) {
                                $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                            }
                        }
                    }
                }

                if ($scope.allSources !== "" && $scope.allSources !== null) {
                    if ($scope.allSources.length > 0) {
                        for (var i1 = 0; i1 < $scope.allSources.length; i1++) {
                            if ($scope.allSources[i1].Selected === true) {
                                $scope.Sourcescheckboxchcked.push($scope.allSources[i1]);
                            }
                        }
                    }
                }

                if ($scope.govtBondList !== "" && $scope.govtBondList !== null) {
                    if ($scope.govtBondList.length > 0) {
                        for (var i2 = 0; i2 < $scope.govtBondList.length; i2++) {
                            if ($scope.govtBondList[i2].Selected === true) {
                                $scope.SelectedGovtBonds.push($scope.govtBondList[i2]);
                            }
                        }
                    }
                }

                var ActivityIDs = $scope.Activitycheckboxchcked;
                var RefrenceIds = $scope.Refrencecheckboxchcked;
                var SourcesIds = $scope.Sourcescheckboxchcked;

                var PrevSchoolDet = $scope.prevSchoolDetails;

                var StuGuardianDet = $scope.studentGuardianDetails;
                var StuSiblingDetails = $scope.studentSiblingDetails;
                if ($scope.AMST_PlaceOfBirthCountry == null && $scope.AMST_PlaceOfBirthCountry == undefined) {
                    $scope.AMST_PlaceOfBirthCountry = 0;
                }
                if ($scope.AMST_PlaceOfBirthState == null && $scope.AMST_PlaceOfBirthState == undefined) {
                    $scope.AMST_PlaceOfBirthState = 0;
                }

                var data = {
                    "AMST_Id": $scope.EditId,
                    "AMST_NoOfElderBrothers": $scope.obj.amsT_NoOfElderBrothers,
                    "AMST_NoOfYoungerBrothers": $scope.obj.amsT_NoOfYoungerBrothers,
                    "AMST_NoOfElderSisters": $scope.obj.amsT_NoOfElderSisters,
                    "AMST_NoOfYoungerSisters": $scope.obj.amsT_NoOfYoungerSisters,
                    "AMST_NoOfSiblings": $scope.obj.AMST_NoOfSiblings,
                    "AMST_NoOfSiblingsSchool": $scope.obj.AMST_NoOfSiblingsSchool,
                    "AMST_NoOfDependencies": $scope.obj.AMST_NoOfDependencies,


                    "AMST_Noofbrothers": $scope.obj.amsT_Noofbrothers,
                    "AMST_Noofsisters": $scope.obj.amsT_Noofsisters,
                    "SelectedActivityDetails": ActivityIDs,
                    "SelectedRefrenceDetails": RefrenceIds,
                    "SelectedSourceDetails": SourcesIds,
                    "SelectedAchivementDetails": $scope.obj.amsteC_Extracurricular,

                    "AMST_ChurchName": $scope.obj.AMST_ChurchName,
                    "AMST_ChurchAddress": $scope.obj.AMST_ChurchAddress,

                    "AMST_ChurchBaptisedDate": $scope.obj.amsT_ChurchBaptisedDate,
                    "SelectedBondDetails": $scope.SelectedGovtBonds,
                    "SelectedPrevSchoolDetails": PrevSchoolDet,
                    "SelectedSiblingDetails": StuSiblingDetails,
                    "SelectedGuardianDetails": StuGuardianDet,
                    "AMST_Village": $scope.obj.AMST_Village,
                    "AMST_Town": $scope.obj.AMST_Town,
                    "AMST_Distirct": $scope.obj.AMST_Distirct,
                    "AMST_Taluk": $scope.obj.AMST_Taluk,
                    "AMST_PlaceOfBirthCountry": $scope.obj.AMST_PlaceOfBirthCountry,
                    "AMST_PlaceOfBirthState": $scope.obj.AMST_PlaceOfBirthState
                };

                apiService.create("StudentAdmission/savefourthtab", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                            $scope.EditId = promise.amsT_Id;

                            $scope.Medical = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.scroll();
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Update Successfully");
                            $scope.EditId = promise.amsT_Id;

                            $scope.Medical = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.scroll();
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }

                    else if (promise.message === "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        $scope.submitted4 = true;

                        $scope.Medical = true;
                    }
                });
            }
            else {
                $scope.submitted4 = true;

                $scope.Medical = true;
            }
        };

        $scope.savesixthtab = function () {

            if ($scope.myForm6.$valid) {
                if ($scope.obj.AMST_TransferrableJobFlg == '1') {
                    $scope.AMST_TransferrableJobFlg = true;
                }
                else {
                    $scope.obj.AMST_TransferrableJobFlg = false;
                }
                if ($scope.obj.AMST_VaccinatedFlg == '1') {
                    $scope.obj.AMST_VaccinatedFlg = true;
                }
                else {
                    $scope.obj.AMST_VaccinatedFlg = false;
                }
                if ($scope.obj.AMST_Tcflag == '1') {
                    $scope.obj.AMST_Tcflag = true;
                }
                else {
                    $scope.obj.AMST_Tcflag = false;
                }
                var data = {
                    "AMST_Id": $scope.EditId,
                    "AMST_Studentillness": $scope.obj.AMST_Studentillness,
                    "AMST_Illnessdetails": $scope.obj.AMST_Illnessdetails,
                    "AMST_UnderAge": $scope.obj.AMST_UnderAge,
                    "AMST_MedicalComplaints": $scope.obj.AMST_MedicalComplaints,
                    "AMST_Boarding": $scope.obj.AMST_Boarding,
                    "AMST_LastPlayGrndAttnd": $scope.obj.AMST_LastPlayGrndAttnd,
                    "AMST_AdmissionReason": $scope.obj.AMST_AdmissionReason,
                    "AMST_OtherResidential_Addr": $scope.obj.AMST_OtherResidential_Addr,
                    "AMST_SchoolDISECode": $scope.obj.AMST_SchoolDISECode,
                    "AMST_FirstLanguage": $scope.obj.AMST_FirstLanguage,
                    "AMST_Thirdlanguage": $scope.obj.AMST_Thirdlanguage,
                    "AMST_OverAge": $scope.obj.AMST_OverAge,
                    "AMST_OtherInformations": $scope.obj.AMST_OtherInformations,
                    "AMST_ExtraActivity": $scope.obj.AMST_ExtraActivity,
                    "AMST_Stayingwith": $scope.obj.AMST_Stayingwith,
                    "AMST_MaritalStatus": $scope.obj.AMST_MaritalStatus,
                    "AMST_OtherPermanentAddr": $scope.obj.AMST_OtherPermanentAddr,
                    "AMST_Domicile": $scope.obj.AMST_Domicile,
                    "AMST_SecondLanguage": $scope.obj.AMST_SecondLanguage,
                    "AMST_TransferrableJobFlg": $scope.obj.AMST_TransferrableJobFlg,
                    "AMST_VaccinatedFlg": $scope.obj.AMST_VaccinatedFlg,
                    "AMST_Tcflag": $scope.obj.AMST_Tcflag,





                }

                apiService.create("StudentAdmission/savesixthtab", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                            $scope.EditId = promise.amsT_Id;
                            $scope.DocumentUpload = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.scroll();
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Update Successfully");
                            $scope.EditId = promise.amsT_Id;
                            $scope.DocumentUpload = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.scroll();
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }

                    else if (promise.message === "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        $scope.submitted6 = true;
                        $scope.DocumentUpload = true;
                    }
                });
            }
            else {
                $scope.submitted6 = true;
                $scope.DocumentUpload = true;
            }
        }

        //Fifth Tab Saving
        $scope.savefinaltab = function () {
            if ($scope.myForm5.$valid) {
                if ($scope.obj.amsT_HostelReqdFlag === true) {
                    $scope.obj.amsT_HostelReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_HostelReqdFlag = 0;
                }

                if ($scope.obj.amsT_TransportReqdFlag === true) {
                    $scope.obj.amsT_TransportReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_TransportReqdFlag = 0;
                }

                if ($scope.obj.amsT_GymReqdFlag === true) {
                    $scope.obj.amsT_GymReqdFlag = 1;
                }
                else {
                    $scope.obj.amsT_GymReqdFlag = 0;
                }
                var data = {
                    "AMST_Id": $scope.EditId,
                    "AMST_Father_Signature": $scope.fatherSign,
                    "AMST_Father_FingerPrint": $scope.fatherFingerprint,
                    "AMST_Mother_Signature": $scope.mothersign,
                    "AMST_Mother_FingerPrint": $scope.motherfingerprint,
                    "ANST_FatherPhoto": $scope.fatherphoto,
                    "ANST_MotherPhoto": $scope.motherphoto,
                    "AMST_HostelReqdFlag": $scope.obj.amsT_HostelReqdFlag,
                    "AMST_TransportReqdFlag": $scope.obj.amsT_TransportReqdFlag,
                    "AMST_GymReqdFlag": $scope.obj.amsT_GymReqdFlag,
                    Uploaded_documentList: $scope.documentList
                }
                apiService.create("StudentAdmission/savefinaltab", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                            $state.reload();
                        }
                        else {
                            swal("Failed To Save Record");
                            $state.reload();
                        }
                    }
                    else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Update Successfully");
                            $state.reload();
                        }
                        else {
                            swal("Failed To Update Record");
                            $state.reload();
                        }
                    }
                    else {
                        $scope.submitted = true;
                        $scope.disableSaveButton = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
                $scope.disableSaveButton = false;
            }
        };




        // EDIT DATA
        $scope.Editdata = function (EditRecord, allRefrence, allSources, allActivity) {

            $scope.address = false;
            $scope.Parents = false;
            $scope.Others = false;
            $scope.Medical = false;
            $scope.DocumentUpload = false;
            $scope.editdatasec = false;
            $scope.obj.asmS_Id = "";
            $scope.disableinstname = true;
            $scope.mobiles = [{ id: 'mobile1' }];
            $scope.emails = [{ id: 'email1' }];

            $scope.mobiles1 = [{ id: 'mobile2' }];
            $scope.emails1 = [{ id: 'email2' }];

            $scope.mobilesstd = [{ id: 'mobilesstd' }];
            $scope.emailsstd = [{ id: 'emailsstd' }];

            $scope.EditId = EditRecord.amsT_Id;
            var MEditId = $scope.EditId;
            $scope.castedisble = false;
            $scope.obj.stucategory = "";
            $scope.obj.amsT_BiometricId = "";
            $scope.obj.amsT_RFCardNo = "";

            apiService.getURI("StudentAdmission/GetSelectedRowDetails/", MEditId).then(function (promise) {

                $scope.documentList = promise.documentList;
                $scope.DOB = false;
                $scope.mi_id = promise.mI_Id;
                $scope.myTabIndex = 0;
                $scope.scroll();

                if ($scope.govtBondList !== null && promise.bondList !== null) {

                    for (var i = 0; i < $scope.govtBondList.length; i++) {
                        $scope.govtBondList[i].Selected = false;
                        $scope.govtBondList[i].amstB_BandNo = "";
                    }
                    for (var i = 0; i < $scope.govtBondList.length; i++) {
                        name = $scope.govtBondList[i].imgB_Id;
                        for (var j = 0; j < promise.bondList.length; j++) {
                            if (name == promise.bondList[j].imgB_Id) {
                                $scope.govtBondList[i].Selected = true;
                                $scope.govtBondList[i].amstB_BandNo = promise.bondList[j].amstB_BandNo;
                                //  $scope.tempBond.push($scope.govtBondList[i]);
                            }
                        }
                    }
                }

                if (promise.prevSchoolDetails !== null && promise.prevSchoolDetails.length > 0) {
                    $scope.prevSchoolDetails = promise.prevSchoolDetails;
                    $scope.prevschlcount = promise.prevSchoolDetails.length;
                    for (var i = 0; i < promise.prevSchoolDetails.length; i++) {
                        $scope.prevSchoolDetails[i].amstpS_PreSchoolBoard = promise.prevSchoolDetails[i].amstpS_PreSchoolBoard;
                        $scope.prevSchoolDetails[i].amstpS_PreSchoolType = promise.prevSchoolDetails[i].amstpS_PreSchoolType;
                        $scope.prevSchoolDetails[i].amstpS_PreviousClass = promise.prevSchoolDetails[i].amstpS_PreviousClass;
                        $scope.prevSchoolDetails[i].amstpS_LeftYear = promise.prevSchoolDetails[i].amstpS_LeftYear;
                        $scope.prevSchoolDetails[i].amstpS_PreSchoolCountry = promise.prevSchoolDetails[i].amstpS_PreSchoolCountry;

                        if (promise.prevSchoolDetails[i].amstpS_PreSchoolCountry !== null && promise.prevSchoolDetails[i].amstpS_PreSchoolCountry !== undefined) {
                            $scope.onselectprevCountry(promise.prevSchoolDetails[i].amstpS_PreSchoolCountry, promise.prevSchoolDetails[i]);
                        }

                        $scope.prevSchoolDetails[i].amstpS_PreSchoolState = promise.prevSchoolDetails[i].amstpS_PreSchoolState;
                        $scope.prevSchoolDetails[i].amstpS_ConcOrScholarshipFlg = promise.prevSchoolDetails[i].amstpS_ConcOrScholarshipFlg;
                        $scope.prevSchoolDetails[i].amstpS_ConcOrScholarshipDate = new Date(promise.prevSchoolDetails[i].amstpS_ConcOrScholarshipDate);
                        $scope.prevSchoolDetails[i].amstpS_PrvTCNO = promise.prevSchoolDetails[i].amstpS_PrvTCNO;
                        $scope.prevSchoolDetails[i].amstpS_PrvTCDate = new Date(promise.prevSchoolDetails[i].amstpS_PrvTCDate);
                    }
                }

                else {
                    $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
                    $scope.prevschlcount = 0;
                }

                if (promise.studentGuardianDetails != null && promise.studentGuardianDetails.length > 0) {
                    $scope.studentGuardianDetails = promise.studentGuardianDetails;
                    $scope.grddetcount = promise.studentGuardianDetails.length;
                }

                else {
                    $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
                    $scope.grddetcount = 0;
                }

                if (promise.studentSiblingDetails !== null && promise.studentSiblingDetails.length > 0) {
                    $scope.studentSiblingDetails = promise.studentSiblingDetails;
                    $scope.sibcount = promise.studentSiblingDetails.length;
                }
                else {
                    $scope.studentSiblingDetails = [{ id: 'sibling1' }];
                    $scope.sibcount = 0;
                }


                $scope.studentReferenceDetails = promise.studentReferenceDetails;
                $scope.studentSourceDetails = promise.studentSourceDetails;
                $scope.studentActivityDetails = promise.studentActivityDetails;

                if (promise.adm_m_student[0].amsT_Village != null) {
                    $scope.obj.AMST_Village = promise.adm_m_student[0].amsT_Village;
                }

                $scope.obj.AMST_Town = promise.adm_m_student[0].amsT_Town;
                $scope.obj.AMST_Taluk = promise.adm_m_student[0].amsT_Taluk;
                $scope.obj.AMST_Distirct = promise.adm_m_student[0].amsT_Distirct;
                $scope.amsT_PENNo = promise.adm_m_student[0].amsT_PENNo;

                $scope.obj.AMST_Studentillness = promise.adm_m_student[0].amsT_Studentillness;
                $scope.obj.AMST_Illnessdetails = promise.adm_m_student[0].amsT_Illnessdetails;
                $scope.obj.AMST_UnderAge = promise.adm_m_student[0].amsT_UnderAge;
                $scope.obj.AMST_MedicalComplaints = promise.adm_m_student[0].amsT_MedicalComplaints;
                $scope.obj.AMST_Boarding = promise.adm_m_student[0].amsT_Boarding;
                $scope.obj.AMST_LastPlayGrndAttnd = promise.adm_m_student[0].amsT_LastPlayGrndAttnd;
                $scope.obj.AMST_AdmissionReason = promise.adm_m_student[0].amsT_AdmissionReason;
                $scope.obj.AMST_OtherResidential_Addr = promise.adm_m_student[0].amsT_OtherResidential_Addr;
                $scope.obj.AMST_SchoolDISECode = promise.adm_m_student[0].amsT_SchoolDISECode;
                $scope.obj.AMST_FirstLanguage = promise.adm_m_student[0].amsT_FirstLanguage;
                $scope.obj.AMST_Thirdlanguage = promise.adm_m_student[0].amsT_Thirdlanguage;
                $scope.obj.AMST_OverAge = promise.adm_m_student[0].amsT_OverAge;
                $scope.obj.AMST_OtherInformations = promise.adm_m_student[0].amsT_OtherInformations;
                $scope.obj.AMST_ExtraActivity = promise.adm_m_student[0].amsT_ExtraActivity;
                $scope.obj.AMST_Stayingwith = promise.adm_m_student[0].amsT_Stayingwith;
                $scope.obj.AMST_MaritalStatus = promise.adm_m_student[0].amsT_MaritalStatus;
                $scope.obj.AMST_OtherPermanentAddr = promise.adm_m_student[0].amsT_OtherPermanentAddr;
                $scope.obj.AMST_Domicile = promise.adm_m_student[0].amsT_Domicile;
                $scope.obj.AMST_SecondLanguage = promise.adm_m_student[0].amsT_SecondLanguage;
                $scope.obj.AMST_TransferrableJobFlg = promise.adm_m_student[0].amsT_TransferrableJobFlg;
                $scope.obj.AMST_VaccinatedFlg = promise.adm_m_student[0].amsT_VaccinatedFlg;
                $scope.obj.AMST_Tcflag = promise.adm_m_student[0].amsT_Tcflag;




                $scope.obj.AMST_ChurchName = promise.adm_m_student[0].amsT_ChurchName;
                $scope.obj.AMST_ChurchAddress = promise.adm_m_student[0].amsT_ChurchAddress;

                //$scope.obj.AMST_PlaceOfBirthState = promise.adm_m_student[0].amsT_PlaceOfBirthState;
                $scope.obj.AMST_PlaceOfBirthCountry = promise.adm_m_student[0].amsT_PlaceOfBirthCountry;



                $scope.allStateForBirthPlace = promise.allStateForBirthPlace;
                $scope.obj.AMST_PlaceOfBirthState = promise.adm_m_student[0].amsT_PlaceOfBirthState;

                $scope.allStateonchangeNationality = promise.allStateonchangeNationality;
                $scope.obj.AMST_State = promise.adm_m_student[0].amsT_State;





                $scope.obj.amsT_NoOfElderBrothers = promise.adm_m_student[0].amsT_NoOfElderBrothers;
                $scope.obj.amsT_NoOfYoungerBrothers = promise.adm_m_student[0].amsT_NoOfYoungerBrothers;
                $scope.obj.amsT_NoOfElderSisters = promise.adm_m_student[0].amsT_NoOfElderSisters;
                $scope.obj.amsT_NoOfYoungerSisters = promise.adm_m_student[0].amsT_NoOfYoungerSisters;

                $scope.obj.AMST_NoOfSiblings = promise.adm_m_student[0].amsT_NoOfSiblings;
                $scope.obj.AMST_NoOfSiblingsSchool = promise.adm_m_student[0].amsT_NoOfSiblingsSchool;
                $scope.obj.AMST_NoOfDependencies = promise.adm_m_student[0].amsT_NoOfDependencies;



                $scope.obj.amsT_Noofbrothers = promise.adm_m_student[0].amsT_Noofbrothers;
                $scope.obj.amsT_Noofsisters = promise.adm_m_student[0].amsT_Noofsisters;


                //attendance & fee done or not
                $scope.attandFeenotdone = false;
                $scope.AttandFee = promise.asmcE_Id;
                if ($scope.AttandFee > 0) {
                    $scope.attandFeenotdone = true;
                }
                if (promise.adm_m_student[0].amsT_ChurchBaptisedDate !== null && promise.adm_m_student[0].amsT_ChurchBaptisedDate !== "") {
                    $scope.obj.amsT_ChurchBaptisedDate = new Date(promise.adm_m_student[0].amsT_ChurchBaptisedDate);
                } else {
                    $scope.obj.amsT_ChurchBaptisedDate = null;
                }

                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    $scope.allRefrence[i].Selected = false;
                }
                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    name = $scope.allRefrence[i].pamR_Id;
                    for (var j = 0; j < promise.studentReferenceDetails.length; j++) {
                        if (name == promise.studentReferenceDetails[j].pamR_Id) {
                            $scope.allRefrence[i].Selected = true;
                            // $scope.Refrencecheckboxchcked.push(promise.studentReferenceDetails[j]);
                        }
                    }
                }
                for (var i = 0; i < $scope.allSources.length; i++) {
                    $scope.allSources[i].Selected = false;
                }
                for (var i = 0; i < $scope.allSources.length; i++) {
                    name = $scope.allSources[i].pamS_Id;
                    for (var j = 0; j < promise.studentSourceDetails.length; j++) {
                        if (name == promise.studentSourceDetails[j].pamS_Id) {
                            $scope.allSources[i].Selected = true;
                            // $scope.Sourcescheckboxchcked.push(promise.studentSourceDetails[j]);
                        }

                    }
                }
                for (var i = 0; i < $scope.allActivity.length; i++) {
                    $scope.allActivity[i].Selected = false;
                }
                for (var i = 0; i < $scope.allActivity.length; i++) {
                    name = $scope.allActivity[i].amA_Id;
                    for (var j = 0; j < promise.studentActivityDetails.length; j++) {
                        if (name == promise.studentActivityDetails[j].amA_Id) {
                            $scope.allActivity[i].Selected = true;
                        }
                    }
                }
                //documnets
                if (promise.documentList.length > 0) {
                    $scope.document = {};
                    $scope.documentList = promise.documentList;
                    angular.forEach(promise.documentList, function (value, key) {
                        $('#' + value.amsmD_Id).attr('src', value.document_Path);
                    });
                }

                $('#blah').attr('src', promise.adm_m_student[0].amsT_Photoname);

                $scope.fatherphoto = promise.adm_m_student[0].ansT_FatherPhoto;
                $scope.fatherSign = promise.adm_m_student[0].amsT_Father_Signature;
                $scope.fatherFingerprint = promise.adm_m_student[0].amsT_Father_FingerPrint;
                $scope.motherphoto = promise.adm_m_student[0].ansT_MotherPhoto;
                $scope.mothersign = promise.adm_m_student[0].amsT_Mother_Signature;
                $scope.motherfingerprint = promise.adm_m_student[0].amsT_Mother_FingerPrint;
                $scope.obj.image = promise.adm_m_student[0].amsT_Photoname;

                $scope.obj.amsT_FirstName = promise.adm_m_student[0].amsT_FirstName;

                $scope.obj.amsT_MiddleName = promise.adm_m_student[0].amsT_MiddleName;
                $scope.obj.amsT_LastName = promise.adm_m_student[0].amsT_LastName;
                $scope.amsT_Date = new Date(promise.adm_m_student[0].amsT_Date);
                $scope.obj.amsT_RegistrationNo = promise.adm_m_student[0].amsT_RegistrationNo;
                $scope.obj.amsT_AdmNo = promise.adm_m_student[0].amsT_AdmNo;
                $scope.obj.asmaY_Id = promise.adm_m_student[0].asmaY_Id;
                $scope.obj.asmcL_Id = promise.adm_m_student[0].asmcL_Id;

                if (promise.stud_catg_edit.length > 0) {
                    $scope.obj.stucategory = promise.stud_catg_edit[0].asmcC_Id;
                }
                
                $scope.amsT_PENNo = promise.adm_m_student[0].amsT_PENNo;
                    $scope.amsT_Sex = promise.adm_m_student[0].amsT_Sex;

                $scope.obj.amsT_DOB = new Date(promise.adm_m_student[0].amsT_DOB);
                $scope.obj.amsT_DOB_Words = promise.adm_m_student[0].amsT_DOB_Words;
                $scope.obj.amsT_BiometricId = promise.adm_m_student[0].amsT_BiometricId;
                $scope.obj.amsT_RFCardNo = promise.adm_m_student[0].amsT_RFCardNo;
                $scope.obj.pasR_Age = promise.adm_m_student[0].pasR_Age;
                $scope.obj.amsT_BloodGroup = promise.adm_m_student[0].amsT_BloodGroup;
                $scope.obj.amsT_MotherTongue = promise.adm_m_student[0].amsT_MotherTongue;
                $scope.obj.amsT_LanguageSpoken = promise.adm_m_student[0].amsT_LanguageSpoken;
                $scope.obj.amsT_BirthCertNO = promise.adm_m_student[0].amsT_BirthCertNO;
                $scope.obj.ivrmmR_Id = promise.adm_m_student[0].ivrmmR_Id;
                $scope.obj.amsT_SubCasteIMC_Id = promise.adm_m_student[0].amsT_SubCasteIMC_Id;
                $scope.obj.amsT_Tribe = promise.adm_m_student[0].amsT_Tribe;
                $scope.obj.amsT_Tpin = promise.adm_m_student[0].amsT_Tpin;
                $scope.obj.amsT_GPSTrackingId = promise.adm_m_student[0].amsT_GPSTrackingId;
                $scope.obj.amsT_GovtAdmno = promise.adm_m_student[0].amsT_GovtAdmno;

                $scope.obj.imcC_Id = promise.adm_m_student[0].imcC_Id;
                if ($scope.allCaste.length > 0) {
                    for (var i = 0; i < $scope.allCaste.length; i++) {
                        if (promise.adm_m_student[0].iC_Id == $scope.allCaste[i].imC_Id) {
                            $scope.allCaste[i].Selected = true;
                            $scope.obj.iC_Id = $scope.allCaste[i];
                            $scope.newcaste = promise.adm_m_student[0].iC_Id;
                        }
                    }
                }
                else {
                    swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                }

                $scope.obj.IVRMMC_Id = promise.adm_m_student[0].amsT_Nationality;

                $scope.obj.IVRMMC_Id5 = promise.adm_m_student[0].amsT_PerCountry;

                getSelectGetState($scope.obj.IVRMMC_Id5, promise.adm_m_student[0].amsT_PerState);

                $scope.obj.amsT_PerState = promise.adm_m_student[0].amsT_PerState;


                $scope.obj.amsT_PerStreet = promise.adm_m_student[0].amsT_PerStreet;
                $scope.obj.amsT_PerArea = promise.adm_m_student[0].amsT_PerArea;
                $scope.obj.amsT_PerCity = promise.adm_m_student[0].amsT_PerCity;

                $scope.allDistrict = promise.allDistrict;
                $scope.allDistrict1 = promise.allDistrict;

                // getSelectGetdistrict1(promise.adm_m_student[0].amsT_ConState);
                //  getSelectGetdistrict2(promise.adm_m_student[0].amsT_PerState);
                $scope.obj.amsT_ConDistrict = promise.adm_m_student[0].amsT_ConDistrict;
                $scope.obj.amsT_PerDistrict = promise.adm_m_student[0].amsT_PerDistrict;

                $scope.obj.amsT_PerPincode = promise.adm_m_student[0].amsT_PerPincode;


                $scope.obj.amsT_StuBankAccNo = promise.adm_m_student[0].amsT_StuBankAccNo;

                $scope.obj.AMST_BankName = promise.adm_m_student[0].amsT_BankName;
                $scope.obj.AMST_BranchName = promise.adm_m_student[0].amsT_BranchName;

                $scope.obj.amsT_StuBankIFSC_Code = promise.adm_m_student[0].amsT_StuBankIFSC_Code;
                $scope.obj.amsT_AadharNo = promise.adm_m_student[0].amsT_AadharNo;
                $scope.obj.amsT_BirthPlace = promise.adm_m_student[0].amsT_BirthPlace;
                $scope.obj.amsT_StuCasteCertiNo = promise.adm_m_student[0].amsT_StuCasteCertiNo;
                $scope.obj.amsT_StudentPANNo = promise.adm_m_student[0].amsT_StudentPANNo;
                $scope.obj.amsT_MobileNo = promise.adm_m_student[0].amsT_MobileNo;
                $scope.obj.amsT_emailId = promise.adm_m_student[0].amsT_emailId;

                $scope.obj.amsT_PerStreet = promise.adm_m_student[0].amsT_PerStreet;
                $scope.obj.amsT_ConPincode = promise.adm_m_student[0].amsT_ConPincode;
                $scope.obj.amsT_ConArea = promise.adm_m_student[0].amsT_ConArea;
                $scope.obj.amsT_ConStreet = promise.adm_m_student[0].amsT_ConStreet;
                $scope.obj.amsT_ConCity = promise.adm_m_student[0].amsT_ConCity;
                $scope.obj.amsT_ConCountry = promise.adm_m_student[0].amsT_ConCountry;

                getSelectGetState2($scope.obj.amsT_ConCountry, promise.adm_m_student[0].amsT_ConState);


                $scope.obj.amsT_ConState = promise.adm_m_student[0].amsT_ConState;

                if (promise.adm_m_student[0].amsT_FatherAliveFlag === "true") {
                    $scope.obj.amsT_FatherAliveFlag = true;
                }
                else {
                    $scope.obj.amsT_FatherAliveFlag = false;
                }

                $scope.obj.amsT_FatherName = promise.adm_m_student[0].amsT_FatherName;
                $scope.obj.amsT_FatherSurname = promise.adm_m_student[0].amsT_FatherSurname;
                $scope.obj.amsT_FatherAadharNo = promise.adm_m_student[0].amsT_FatherAadharNo;
                $scope.obj.amsT_FatherEducation = promise.adm_m_student[0].amsT_FatherEducation;
                $scope.obj.amsT_FatherMaritalStatus = promise.adm_m_student[0].amsT_FatherMaritalStatus;
                $scope.obj.amsT_FatherOfficeAdd = promise.adm_m_student[0].amsT_FatherOfficeAdd;

                if (promise.fatherstatelist !== null && promise.fatherstatelist.length > 0) {
                    $scope.fatherstatelist = promise.fatherstatelist;
                }

                $scope.obj.amsT_FatherPresentAddress = promise.adm_m_student[0].amsT_FatherPresentAddress;
                $scope.obj.amsT_FatherPresentCity = promise.adm_m_student[0].amsT_FatherPresentCity;
                $scope.obj.amsT_FatherPresentState = promise.adm_m_student[0].amsT_FatherPresentState;
                $scope.obj.amsT_FatherPresentPS = promise.adm_m_student[0].amsT_FatherPresentPS;
                $scope.obj.amsT_FatherPresentPO = promise.adm_m_student[0].amsT_FatherPresentPO;
                $scope.obj.amsT_FatherPresentPinCode = promise.adm_m_student[0].amsT_FatherPresentPinCode;

                $scope.obj.amsT_FatherPermanentAddress = promise.adm_m_student[0].amsT_FatherPermanentAddress;
                $scope.obj.amsT_FatherPermanentCity = promise.adm_m_student[0].amsT_FatherPermanentCity;
                $scope.obj.amsT_FatherPermanentState = promise.adm_m_student[0].amsT_FatherPermanentState;
                $scope.obj.amsT_FatherPermanentPS = promise.adm_m_student[0].amsT_FatherPermanentPS;
                $scope.obj.amsT_FatherPermanentPO = promise.adm_m_student[0].amsT_FatherPermanentPO;
                $scope.obj.amsT_FatherPermanentPinCode = promise.adm_m_student[0].amsT_FatherPermanentPinCode;

                $scope.obj.amsT_FatherOccupation = promise.adm_m_student[0].amsT_FatherOccupation;
                $scope.obj.amsT_FatherDesignation = promise.adm_m_student[0].amsT_FatherDesignation;
                $scope.obj.amsT_FatherBankName = promise.adm_m_student[0].amsT_FatherBankName;
                $scope.obj.amsT_FatherBankAccNo = promise.adm_m_student[0].amsT_FatherBankAccNo;
                $scope.obj.amsT_FatherBankBranch = promise.adm_m_student[0].amsT_FatherBankBranch;
                $scope.obj.amsT_FatherBankIFSC_Code = promise.adm_m_student[0].amsT_FatherBankIFSC_Code;
                $scope.obj.amsT_FatherCasteCertiNo = promise.adm_m_student[0].amsT_FatherCasteCertiNo;
                $scope.obj.amsT_FatherHomePhNo = promise.adm_m_student[0].amsT_FatherHomePhNo;
                $scope.obj.amsT_FatherPassingYear = promise.adm_m_student[0].amsT_FatherPassingYear;
                $scope.obj.amsT_FatherOfficePhNo = promise.adm_m_student[0].amsT_FatherOfficePhNo;
                $scope.obj.amsT_FatherTribe = promise.adm_m_student[0].amsT_FatherTribe;
                $scope.obj.IVRMMC_Id3 = promise.adm_m_student[0].amsT_FatherNationality;
                $scope.obj.amsT_FatherMonIncome = promise.adm_m_student[0].amsT_FatherMonIncome;
                $scope.obj.amsT_FatherAnnIncome = promise.adm_m_student[0].amsT_FatherAnnIncome;
                $scope.obj.amsT_FatheremailId = promise.adm_m_student[0].amsT_FatheremailId;
                $scope.obj.amsT_FatherMobleNo = promise.adm_m_student[0].amsT_FatherMobleNo;
                $scope.obj.amsT_FatherPANNo = promise.adm_m_student[0].amsT_FatherPANNo;
                $scope.obj.amsT_FatherReligion = promise.adm_m_student[0].amsT_FatherReligion;
                $scope.obj.amsT_FatherCaste = promise.adm_m_student[0].amsT_FatherCaste;
                $scope.obj.amsT_FatherSubCaste = promise.adm_m_student[0].amsT_FatherSubCaste;
                $scope.obj.amsT_FatherChurchAffiliation = promise.adm_m_student[0].amsT_FatherChurchAffiliation;
                $scope.obj.amsT_FatherSelfEmployedFlg = promise.adm_m_student[0].amsT_FatherSelfEmployedFlg;

                if (promise.adm_m_student[0].amsT_MotherAliveFlag === "true") {
                    $scope.obj.amsT_MotherAliveFlag = true;
                }
                else {
                    $scope.obj.amsT_MotherAliveFlag = false;
                }

                if (promise.motherstatelist !== null && promise.motherstatelist.length > 0) {
                    $scope.motherstatelist = promise.motherstatelist;
                }

                $scope.obj.amsT_MotherName = promise.adm_m_student[0].amsT_MotherName;
                $scope.obj.amsT_MotherSurname = promise.adm_m_student[0].amsT_MotherSurname;
                $scope.obj.amsT_MotherAadharNo = promise.adm_m_student[0].amsT_MotherAadharNo;
                $scope.obj.amsT_MotherEducation = promise.adm_m_student[0].amsT_MotherEducation;
                $scope.obj.amsT_MotherOfficeAdd = promise.adm_m_student[0].amsT_MotherOfficeAdd;
                $scope.obj.amsT_MotherOccupation = promise.adm_m_student[0].amsT_MotherOccupation;
                $scope.obj.amsT_MotherDesignation = promise.adm_m_student[0].amsT_MotherDesignation;
                $scope.obj.amsT_MotherBankBranch = promise.adm_m_student[0].amsT_MotherBankBranch;
                $scope.obj.amsT_MotherBankName = promise.adm_m_student[0].amsT_MotherBankName;
                $scope.obj.amsT_MotherBankAccNo = promise.adm_m_student[0].amsT_MotherBankAccNo;
                $scope.obj.amsT_MotherBankIFSC_Code = promise.adm_m_student[0].amsT_MotherBankIFSC_Code;
                $scope.obj.amsT_MotherCasteCertiNo = promise.adm_m_student[0].amsT_MotherCasteCertiNo;
                $scope.obj.IVRMMC_Id4 = promise.adm_m_student[0].amsT_MotherNationality;
                $scope.obj.amsT_MotherMonIncome = promise.adm_m_student[0].amsT_MotherMonIncome;
                $scope.obj.amsT_MotherAnnIncome = promise.adm_m_student[0].amsT_MotherAnnIncome;
                $scope.obj.amsT_MotherPANNo = promise.adm_m_student[0].amsT_MotherPANNo;
                $scope.obj.amsT_MotherMobileNo = promise.adm_m_student[0].amsT_MotherMobileNo;
                $scope.obj.amsT_MotherEmailId = promise.adm_m_student[0].amsT_MotherEmailId;

                $scope.obj.amsT_MotherChurchAffiliation = promise.adm_m_student[0].amsT_MotherChurchAffiliation;
                $scope.obj.amsT_MotherSelfEmployedFlg = promise.adm_m_student[0].amsT_MotherSelfEmployedFlg;

                $scope.obj.amsT_MotherPresentAddress = promise.adm_m_student[0].amsT_MotherPresentAddress;
                $scope.obj.amsT_MotherPresentCity = promise.adm_m_student[0].amsT_MotherPresentCity;
                $scope.obj.amsT_MotherPresentState = promise.adm_m_student[0].amsT_MotherPresentState;
                $scope.obj.amsT_MotherPresentPS = promise.adm_m_student[0].amsT_MotherPresentPS;
                $scope.obj.amsT_MotherPresentPO = promise.adm_m_student[0].amsT_MotherPresentPO;
                $scope.obj.amsT_MotherPresentPinCode = promise.adm_m_student[0].amsT_MotherPresentPinCode;
                $scope.streamlist = promise.masterstream;
                $scope.obj.amsT_MotherPermanentAddress = promise.adm_m_student[0].amsT_MotherPermanentAddress;
                $scope.obj.amsT_MotherPermanentCity = promise.adm_m_student[0].amsT_MotherPermanentCity;
                $scope.obj.amsT_MotherPermanentState = promise.adm_m_student[0].amsT_MotherPermanentState;
                $scope.obj.amsT_MotherPermanentPS = promise.adm_m_student[0].amsT_MotherPermanentPS;
                $scope.obj.amsT_MotherPermanentPO = promise.adm_m_student[0].amsT_MotherPermanentPO;
                $scope.obj.amsT_MotherPermanentPinCode = promise.adm_m_student[0].amsT_MotherPermanentPinCode;

                $scope.obj.amsT_MotherPassingYear = promise.adm_m_student[0].amsT_MotherPassingYear;
                $scope.obj.amsT_MotherOfficePhNo = promise.adm_m_student[0].amsT_MotherOfficePhNo;
                $scope.obj.amsT_MotherMaritalStatus = promise.adm_m_student[0].amsT_MotherMaritalStatus;
                $scope.obj.amsT_MotherHomePhNo = promise.adm_m_student[0].amsT_MotherHomePhNo;

                $scope.obj.amsT_MotherReligion = promise.adm_m_student[0].amsT_MotherReligion;
                $scope.obj.amsT_MotherCaste = promise.adm_m_student[0].amsT_MotherCaste;
                $scope.obj.amsT_MotherSubCaste = promise.adm_m_student[0].amsT_MotherSubCaste;
                $scope.obj.amsT_MotherTribe = promise.adm_m_student[0].amsT_MotherTribe;
                $scope.obj.asmsT_Id = promise.adm_m_student[0].asmsT_Id;

                if (promise.studentAchivementDetails !== null && promise.studentAchivementDetails.length > 0) {
                    $scope.obj.amsteC_Extracurricular = promise.studentAchivementDetails[0].amsteC_Extracurricular;
                }
                else {
                    $scope.obj.amsteC_Extracurricular = "";
                }

                $scope.obj.amsT_BPLCardFlag = promise.adm_m_student[0].amsT_BPLCardFlag;
                if (promise.adm_m_student[0].amsT_BPLCardFlag == 1) {

                    $scope.obj.amsT_BPLCardFlag = true;
                }
                else {
                    $scope.obj.amsT_BPLCardFlag = false;
                }

                $scope.obj.amsT_BPLCardNo = promise.adm_m_student[0].amsT_BPLCardNo;
                $scope.obj.amsT_HostelReqdFlag = promise.adm_m_student[0].amsT_HostelReqdFlag;
                if ($scope.obj.amsT_HostelReqdFlag === 1) {
                    $scope.obj.amsT_HostelReqdFlag = true;
                }
                else {
                    $scope.obj.amsT_HostelReqdFlag = false;
                }
                $scope.obj.amsT_TransportReqdFlag = promise.adm_m_student[0].amsT_TransportReqdFlag;
                if ($scope.obj.amsT_TransportReqdFlag === 1) {
                    $scope.obj.amsT_TransportReqdFlag = true;
                }
                else {
                    $scope.obj.amsT_TransportReqdFlag = false;
                }
                $scope.obj.amsT_GymReqdFlag = promise.adm_m_student[0].amsT_GymReqdFlag;
                if ($scope.obj.amsT_GymReqdFlag === 1) {
                    $scope.obj.amsT_GymReqdFlag = true;
                }
                else {
                    $scope.obj.amsT_GymReqdFlag = false;
                }
                $scope.obj.amsT_ECSFlag = promise.adm_m_student[0].amsT_ECSFlag;
                if ($scope.obj.amsT_ECSFlag === 1) {
                    $scope.obj.amsT_ECSFlag = true;
                }
                else {
                    $scope.obj.amsT_ECSFlag = false;
                }
                $scope.obj.chkbox_address = 0;
                if ($scope.obj.amsT_PerStreet == $scope.obj.amsT_ConStreet && $scope.obj.amsT_PerArea == $scope.obj.amsT_ConArea && $scope.obj.IVRMMC_Id5 == $scope.obj.amsT_ConCountry && $scope.obj.amsT_PerState == $scope.obj.amsT_ConState && $scope.obj.amsT_PerCity == $scope.obj.amsT_ConCity && $scope.obj.amsT_PerPincode == $scope.obj.amsT_ConPincode) {
                    $scope.obj.chkbox_address = 1;
                }
                $scope.obj.feeconcession = promise.adm_m_student[0].amsT_Concession_Type;

                $scope.leftorcurrent = "Current";

                if (promise.classsection != null && promise.classsection != "") {
                    $scope.lblclasssection = promise.classsection[0].asmaY_Year + '-' + promise.classsection[0].classname + '-' + promise.classsection[0].sectionname;
                    $scope.obj.asmS_Id = promise.classsection[0].asms_id;
                    $scope.editdatasec = true;
                    $scope.leftorcurrent = promise.adm_m_student[0].amsT_SOL === "L" ? "Left" : "Current";
                }
                else {
                    $scope.lblclasssection = "N/A";
                }

                
                if ($scope.obj.asmS_Id != "") {

                }
                else {
                    if ($scope.mastersection != null && $scope.mastersection.length > 0) {
                        angular.forEach($scope.mastersection, function (value1) {
                            if (value1.asmC_SectionName == "A") {
                                $scope.obj.asmS_Id = value1.asmS_Id;
                            }
                        });
                    }
                }

                //father mobile no
                if (promise.multiplemobileno.length > 0) {
                    $scope.mobiles = promise.multiplemobileno;
                }
                //father email no
                if (promise.multipleemailid.length > 0) {
                    $scope.emails = promise.multipleemailid;
                }
                //mother mobileno
                if (promise.multiplemobilenomother.length > 0) {
                    $scope.mobiles1 = promise.multiplemobilenomother;
                }
                //mother email
                if (promise.multipleemailidmother.length > 0) {
                    $scope.emails1 = promise.multipleemailidmother;
                }


                //student mobileno
                if (promise.adm_M_Student_MobileNoDTO.length > 0) {
                    $scope.mobilesstd = promise.adm_M_Student_MobileNoDTO;
                }
                //student email
                if (promise.adm_M_Student_EmailIdDTO.length > 0) {
                    $scope.emailsstd = promise.adm_M_Student_EmailIdDTO;
                }

                //$scope.onclasschange($scope.obj.asmcL_Id, $scope.obj.asmaY_Id, 1);                  

                if (promise.adm_M_Student_ECSDTo.length > 0) {
                    $scope.ecsdetailslist = promise.adm_M_Student_ECSDTo[0];
                    $scope.objecs = {};
                    $scope.tempecs = promise.adm_M_Student_ECSDTo;

                    $scope.objecs.asecS_AccountNo = $scope.tempecs[0].asecS_AccountNo;
                    $scope.objecs.asecS_AccountHolderName = $scope.tempecs[0].asecS_AccountHolderName;
                    $scope.objecs.asecS_BankName = $scope.tempecs[0].asecS_BankName;
                    $scope.objecs.asecS_AccountType = $scope.tempecs[0].asecS_AccountType;
                    $scope.objecs.asecS_MICRNo = $scope.tempecs[0].asecS_MICRNo;
                    $scope.objecs.asecS_Branch = $scope.tempecs[0].asecS_Branch;
                    $scope.objecs.asecS_Id = $scope.tempecs[0].asecS_Id;
                }

                if (promise.streamexams != null && promise.streamexams.length > 0) {
                    $scope.streamexams = promise.streamexams;
                    $scope.obj.ASMCE_Id = promise.asmcE_Id;
                    $scope.ASMCE_Id = promise.asmcE_Id;
                }
                else {
                    $scope.streamexams = [];
                }
            });
        };
        $scope.GetSubjectsofinstitute = function (ASMCL_Id) {
            if ($scope.reg.ASMST_Id == undefined || $scope.reg.ASMST_Id == null) {
                $scope.reg.ASMST_Id = 0;
            }
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMST_Id": $scope.obj.asmsT_Id
            }
            apiService.create("StudentAdmission/GetSubjectsofinstitute", data).then(function (promise) {
                if (promise != null) {
                    $scope.electivegrouplist = promise.electivegrouplist;

                    $scope.electivesubgrouplist = promise.electivesubgrouplist;
                    if (promise.streamexams != null && promise.streamexams.length > 0) {
                        $scope.streamexams = promise.streamexams;
                    }
                    else {
                        $scope.streamexams = [];
                    }

                    angular.forEach($scope.electivesubgrouplist, function (opqr) {
                        opqr.ismS_Id = "";
                    }
                    )
                    if ($scope.electivesubgrouplist.length >= 1) {
                        //angular.forEach($scope.electivegrouplist, function (opqr) {
                        //    angular.forEach($scope.electivesubgrouplist, function (opqr1) {
                        //        if (opqr.EMG_Id == opqr1.EMG_Id) {
                        //            opqr1.ismS_Id = opqr.ISMS_Id;
                        //        }
                        //    })
                        //})
                        $scope.grouplength = $scope.electivesubgrouplist.length;
                    }
                    else {
                        $scope.electivesubgrouplist = {};
                    }

                    if ($scope.electivesubgrouplist != null && $scope.electivesubgrouplist.length >= 0) {
                        $scope.electives = [];
                        $scope.compulsory = [];
                        angular.forEach($scope.electivesubgrouplist, function (opqr1) {
                            if (opqr1.EMG_ElectiveFlg == false) {
                                $scope.compulsory.push(opqr1);
                            }
                            else {
                                $scope.electives.push(opqr1);
                            }
                        })
                    }
                }
            })
        }
        //DELETING DATA
        $scope.Deletedata = function (DeleteRecord, SweetAlert) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.deleteId = DeleteRecord.amsT_Id;
                        var MdeleteId = $scope.deleteId;

                        var data = {
                            "AMST_Id": MdeleteId
                        };
                        apiService.create("StudentAdmission/DeleteEntry", data).then(function (promise) {
                            if (promise.message != "" && promise.message != null) {
                                return swal(promise.message);
                            }
                            else if (promise.returnval == true) {
                                swal("Record Deleted Successfully");
                                $state.reload();
                            }
                            else {
                                swal("Failed To Delete Record");
                            }
                        });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        };

        //READMIT DETAILS
        $scope.getstudentlistre = function () {
            $scope.stdname = [];
            $scope.AMST_Id = "";

            var data = {
                "ASMAY_Id": $scope.yearid1
            };
            apiService.create("StudentAdmission/yearwisetcstd/", data).then(function (promise) {
                if (promise !== null) {
                    $scope.stdname = promise.studentList1;
                }
                else {
                    swal("No Records Found From Selected Year..");
                }
            });
        };

        $scope.addtocart = function () {
            if ($scope.yearid1 === undefined || $scope.AMST_Id.amsT_Id === undefined) {
                swal("Select All The Fields ");
                return;
            }

            var data = {
                "ASMAY_Id": $scope.yearid1,
                "AMST_Id": $scope.AMST_Id.amsT_Id
            };

            apiService.create("StudentAdmission/addtocart/", data).then(function (promise) {

                $scope.documentList = promise.documentList;
                $scope.DOB = false;
                $scope.mi_id = promise.mI_Id;
                $scope.EditId = 0;

                $scope.mobiles = [{ id: 'mobile1' }];
                $scope.emails = [{ id: 'email1' }];

                $scope.mobiles1 = [{ id: 'mobile2' }];
                $scope.emails1 = [{ id: 'email2' }];

                $scope.mobilesstd = [{ id: 'mobilesstd' }];
                $scope.emailsstd = [{ id: 'emailsstd' }];


                //father mobile no
                if (promise.multiplemobileno.length > 0) {
                    $scope.mobiles = promise.multiplemobileno;
                }

                //father email no
                if (promise.multipleemailid.length > 0) {
                    $scope.emails = promise.multipleemailid;
                }

                //mother mobileno
                if (promise.multiplemobilenomother.length > 0) {
                    $scope.mobiles1 = promise.multiplemobilenomother;
                }

                //mother email
                if (promise.multipleemailidmother.length > 0) {
                    $scope.emails1 = promise.multipleemailidmother;
                }

                //student mobileno
                if (promise.adm_M_Student_MobileNoDTO.length > 0) {
                    $scope.mobilesstd = promise.adm_M_Student_MobileNoDTO;
                }

                //student email
                if (promise.adm_M_Student_EmailIdDTO.length > 0) {
                    $scope.emailsstd = promise.adm_M_Student_EmailIdDTO;
                }

                if (promise.bondList !== null && promise.bondList.length > 0) {
                    $scope.bondList = promise.bondList;
                }
                if (promise.prevSchoolDetails !== null && promise.prevSchoolDetails.length > 0) {
                    $scope.prevSchoolDetails = promise.prevSchoolDetails;
                }
                if (promise.studentGuardianDetails !== null && promise.studentGuardianDetails.length > 0) {
                    $scope.studentGuardianDetails = promise.studentGuardianDetails;
                }

                if (promise.studentSiblingDetails !== null && promise.studentSiblingDetails.length > 0) {
                    $scope.studentSiblingDetails = promise.studentSiblingDetails;
                }

                $scope.studentReferenceDetails = promise.studentReferenceDetails;
                $scope.studentSourceDetails = promise.studentSourceDetails;
                $scope.studentActivityDetails = promise.studentActivityDetails;


                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    $scope.allRefrence[i].Selected = false;
                }
                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    name = $scope.allRefrence[i].pamR_Id;
                    for (var j = 0; j < promise.studentReferenceDetails.length; j++) {
                        if (name == promise.studentReferenceDetails[j].pamR_Id) {
                            $scope.allRefrence[i].Selected = true;
                        }
                    }
                }
                for (var i = 0; i < $scope.allSources.length; i++) {
                    $scope.allSources[i].Selected = false;
                }
                for (var i = 0; i < $scope.allSources.length; i++) {
                    name = $scope.allSources[i].pamS_Id;
                    for (var j = 0; j < promise.studentSourceDetails.length; j++) {
                        if (name == promise.studentSourceDetails[j].pamS_Id) {
                            $scope.allSources[i].Selected = true;
                        }
                    }
                }
                for (var iaa = 0; iaa < $scope.allActivity.length; iaa++) {
                    $scope.allActivity[iaa].Selected = false;
                }
                for (var iad = 0; iad < $scope.allActivity.length; iad++) {
                    name = $scope.allActivity[iad].amA_Id;
                    for (var j = 0; j < promise.studentActivityDetails.length; j++) {
                        if (parseInt(name) === parseInt(promise.studentActivityDetails[j].amA_Id)) {
                            $scope.allActivity[iad].Selected = true;
                        }
                    }
                }
                //documnets
                if (promise.documentList.length > 0) {
                    $scope.document = {};
                    $scope.documentList = promise.documentList;
                    angular.forEach(promise.documentList, function (value, key) {
                        $('#' + value.amsmD_Id).attr('src', value.document_Path);
                    });
                }

                $('#blah').attr('src', promise.amsT_Photoname);
                $scope.Editfatherphoto = promise.ansT_FatherPhoto;
                $scope.EditfatherSign = promise.amsT_Father_Signature;
                $scope.EditfatherFingerprint = promise.amsT_Father_FingerPrint;
                $scope.Editmotherphoto = promise.ansT_MotherPhoto;
                $scope.Editmothersign = promise.amsT_Mother_Signature;
                $scope.Editmotherfingerprint = promise.amsT_Mother_FingerPrint;
                $scope.fatherphoto = promise.adm_m_student[0].ansT_FatherPhoto;
                $scope.fatherSign = promise.adm_m_student[0].amsT_Father_Signature;
                $scope.fatherFingerprint = promise.adm_m_student[0].amsT_Father_FingerPrint;
                $scope.motherphoto = promise.adm_m_student[0].ansT_MotherPhoto;
                $scope.mothersign = promise.adm_m_student[0].amsT_Mother_Signature;
                $scope.motherfingerprint = promise.adm_m_student[0].amsT_Mother_FingerPrint;
                $scope.obj.image = promise.adm_m_student[0].amsT_Photoname;
                $scope.obj.amsT_FirstName = promise.adm_m_student[0].amsT_FirstName;
                $scope.obj.amsT_MiddleName = promise.adm_m_student[0].amsT_MiddleName;
                $scope.obj.amsT_LastName = promise.adm_m_student[0].amsT_LastName;
                $scope.amsT_Date = new Date(promise.adm_m_student[0].amsT_Date);
                $scope.obj.asmcL_Id = promise.adm_m_student[0].asmcL_Id;
                if (promise.stud_catg_edit.length > 0) {
                    $scope.obj.stucategory = promise.stud_catg_edit[0].asmcC_Id;
                }
                $scope.amsT_PENNo = promise.adm_m_student[0].amsT_PENNo;
                $scope.amsT_Sex = promise.adm_m_student[0].amsT_Sex;
                $scope.obj.amsT_DOB = new Date(promise.adm_m_student[0].amsT_DOB);
                $scope.obj.amsT_DOB_Words = promise.adm_m_student[0].amsT_DOB_Words;
                $scope.obj.pasR_Age = promise.adm_m_student[0].pasR_Age;
                $scope.obj.amsT_BloodGroup = promise.adm_m_student[0].amsT_BloodGroup;
                $scope.obj.amsT_MotherTongue = promise.adm_m_student[0].amsT_MotherTongue;
                $scope.obj.amsT_LanguageSpoken = promise.adm_m_student[0].amsT_LanguageSpoken;
                $scope.obj.amsT_BirthCertNO = promise.adm_m_student[0].amsT_BirthCertNO;
                $scope.obj.ivrmmR_Id = promise.adm_m_student[0].ivrmmR_Id;
                $scope.obj.amsT_Tpin = promise.adm_m_student[0].amsT_Tpin;
                $scope.obj.amsT_GPSTrackingId = promise.adm_m_student[0].amsT_GPSTrackingId;
                $scope.obj.amsT_GovtAdmno = promise.adm_m_student[0].amsT_GovtAdmno;
                $scope.obj.amsT_Tribe = promise.adm_m_student[0].amsT_Tribe;
                $scope.obj.amsT_SubCasteIMC_Id = promise.adm_m_student[0].amsT_SubCasteIMC_Id;

                $scope.obj.amsT_NoOfElderBrothers = promise.adm_m_student[0].amsT_NoOfElderBrothers;
                $scope.obj.amsT_NoOfYoungerBrothers = promise.adm_m_student[0].amsT_NoOfYoungerBrothers;
                $scope.obj.amsT_NoOfElderSisters = promise.adm_m_student[0].amsT_NoOfElderSisters;
                $scope.obj.amsT_NoOfYoungerSisters = promise.adm_m_student[0].amsT_NoOfYoungerSisters;
                $scope.obj.AMST_NoOfSiblings = promise.adm_m_student[0].amsT_NoOfSiblings;
                $scope.obj.AMST_NoOfSiblingsSchool = promise.adm_m_student[0].amsT_NoOfSiblingsSchool;
                $scope.obj.AMST_NoOfDependencies = promise.adm_m_student[0].amsT_NoOfDependencies;
                $scope.obj.asmS_Id = promise.adm_m_student[0].asmS_Id;


                $scope.obj.amsT_Noofbrothers = promise.adm_m_student[0].amsT_Noofbrothers;
                $scope.obj.amsT_Noofsisters = promise.adm_m_student[0].amsT_Noofsisters;
                $scope.obj.amsT_ChurchBaptisedDate = new Date(promise.adm_m_student[0].amsT_ChurchBaptisedDate);

                $scope.obj.imcC_Id = promise.adm_m_student[0].imcC_Id;
                for (var ic = 0; ic < $scope.allCaste.length; ic++) {
                    $scope.allCaste[ic].Selected = false;
                    $scope.obj.iC_Id = "";
                }


                if (promise.allCaste.length > 0) {
                    for (var iac = 0; iac < promise.allCaste.length; iac++) {
                        if (parseInt(promise.adm_m_student[0].iC_Id) === parseInt(promise.allCaste[iac].imC_Id)) {
                            $scope.allCaste[iac].Selected = true;
                            // $scope.obj.iC_Id = promise.adm_m_student[0].iC_Id;
                            $scope.obj.iC_Id = promise.allCaste[iac];

                        }
                    }
                }
                else {
                    swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                }

                $scope.obj.IVRMMC_Id = promise.adm_m_student[0].amsT_Nationality;

                $scope.obj.IVRMMC_Id5 = promise.adm_m_student[0].amsT_PerCountry;


                getSelectGetState($scope.obj.IVRMMC_Id5, promise.adm_m_student[0].amsT_PerState);

                $scope.obj.amsT_PerState = promise.adm_m_student[0].amsT_PerState;

                $scope.obj.amsT_PerStreet = promise.adm_m_student[0].amsT_PerStreet;
                $scope.obj.amsT_PerArea = promise.adm_m_student[0].amsT_PerArea;
                $scope.obj.amsT_PerCity = promise.adm_m_student[0].amsT_PerCity;

                $scope.obj.amsT_PerPincode = promise.adm_m_student[0].amsT_PerPincode;

                $scope.obj.amsT_StuBankAccNo = promise.adm_m_student[0].amsT_StuBankAccNo;

                $scope.obj.AMST_BranchName = promise.adm_m_student[0].amsT_BranchName;
                $scope.obj.AMST_BankName = promise.adm_m_student[0].amsT_BankName;


                $scope.obj.amsT_StuBankIFSC_Code = promise.adm_m_student[0].amsT_StuBankIFSC_Code;
                $scope.obj.amsT_AadharNo = promise.adm_m_student[0].amsT_AadharNo;
                $scope.obj.amsT_BirthPlace = promise.adm_m_student[0].amsT_BirthPlace;
                $scope.obj.amsT_StuCasteCertiNo = promise.adm_m_student[0].amsT_StuCasteCertiNo;
                $scope.obj.amsT_StudentPANNo = promise.adm_m_student[0].amsT_StudentPANNo;
                $scope.obj.amsT_MobileNo = promise.adm_m_student[0].amsT_MobileNo;
                $scope.obj.amsT_emailId = promise.adm_m_student[0].amsT_emailId;

                $scope.obj.amsT_PerStreet = promise.adm_m_student[0].amsT_PerStreet;
                $scope.obj.amsT_ConPincode = promise.adm_m_student[0].amsT_ConPincode;
                $scope.obj.amsT_ConArea = promise.adm_m_student[0].amsT_ConArea;
                $scope.obj.amsT_ConStreet = promise.adm_m_student[0].amsT_ConStreet;
                $scope.obj.amsT_ConCity = promise.adm_m_student[0].amsT_ConCity;
                $scope.obj.amsT_ConCountry = promise.adm_m_student[0].amsT_ConCountry;

                getSelectGetState2($scope.obj.amsT_ConCountry, promise.adm_m_student[0].amsT_ConState);

                $scope.obj.amsT_ConState = promise.adm_m_student[0].amsT_ConState;

                if (promise.adm_m_student[0].amsT_FatherAliveFlag === "true") {
                    $scope.obj.amsT_FatherAliveFlag = true;
                }
                else {
                    $scope.obj.amsT_FatherAliveFlag = false;
                }

                $scope.obj.amsT_FatherName = promise.adm_m_student[0].amsT_FatherName;
                $scope.obj.amsT_FatherSurname = promise.adm_m_student[0].amsT_FatherSurname;
                $scope.obj.amsT_FatherAadharNo = promise.adm_m_student[0].amsT_FatherAadharNo;
                $scope.obj.amsT_FatherEducation = promise.adm_m_student[0].amsT_FatherEducation;
                $scope.obj.amsT_FatherMaritalStatus = promise.adm_m_student[0].amsT_FatherMaritalStatus;
                $scope.obj.amsT_FatherOfficeAdd = promise.adm_m_student[0].amsT_FatherOfficeAdd;

                $scope.obj.amsT_FatherPresentAddress = promise.adm_m_student[0].amsT_FatherPresentAddress;
                $scope.obj.amsT_FatherPresentCity = promise.adm_m_student[0].amsT_FatherPresentCity;
                $scope.obj.amsT_FatherPresentState = promise.adm_m_student[0].amsT_FatherPresentState;
                $scope.obj.amsT_FatherPresentPS = promise.adm_m_student[0].amsT_FatherPresentPS;
                $scope.obj.amsT_FatherPresentPO = promise.adm_m_student[0].amsT_FatherPresentPO;
                $scope.obj.amsT_FatherPresentPinCode = promise.adm_m_student[0].amsT_FatherPresentPinCode;

                $scope.obj.amsT_FatherPermanentAddress = promise.adm_m_student[0].amsT_FatherPermanentAddress;
                $scope.obj.amsT_FatherPermanentCity = promise.adm_m_student[0].amsT_FatherPermanentCity;
                $scope.obj.amsT_FatherPermanentState = promise.adm_m_student[0].amsT_FatherPermanentState;
                $scope.obj.amsT_FatherPermanentPS = promise.adm_m_student[0].amsT_FatherPermanentPS;
                $scope.obj.amsT_FatherPermanentPO = promise.adm_m_student[0].amsT_FatherPermanentPO;
                $scope.obj.amsT_FatherPermanentPinCode = promise.adm_m_student[0].amsT_FatherPermanentPinCode;

                $scope.obj.amsT_FatherOccupation = promise.adm_m_student[0].amsT_FatherOccupation;
                $scope.obj.amsT_FatherDesignation = promise.adm_m_student[0].amsT_FatherDesignation;
                $scope.obj.amsT_FatherBankName = promise.adm_m_student[0].amsT_FatherBankName;
                $scope.obj.amsT_FatherBankBranch = promise.adm_m_student[0].amsT_FatherBankBranch;
                $scope.obj.amsT_FatherBankAccNo = promise.adm_m_student[0].amsT_FatherBankAccNo;

                $scope.obj.amsT_FatherHomePhNo = promise.adm_m_student[0].amsT_FatherHomePhNo;
                $scope.obj.amsT_FatherPassingYear = promise.adm_m_student[0].amsT_FatherPassingYear;
                $scope.obj.amsT_FatherOfficePhNo = promise.adm_m_student[0].amsT_FatherOfficePhNo;
                $scope.obj.amsT_FatherTribe = promise.adm_m_student[0].amsT_FatherTribe;

                $scope.obj.amsT_FatherBankIFSC_Code = promise.adm_m_student[0].amsT_FatherBankIFSC_Code;
                $scope.obj.amsT_FatherCasteCertiNo = promise.adm_m_student[0].amsT_FatherCasteCertiNo;
                $scope.obj.IVRMMC_Id3 = promise.adm_m_student[0].amsT_FatherNationality;
                $scope.obj.amsT_FatherMonIncome = promise.adm_m_student[0].amsT_FatherMonIncome;
                $scope.obj.amsT_FatherAnnIncome = promise.adm_m_student[0].amsT_FatherAnnIncome;
                $scope.obj.amsT_FatherMobleNo = promise.adm_m_student[0].amsT_FatherMobleNo;
                $scope.obj.amsT_FatheremailId = promise.adm_m_student[0].amsT_FatheremailId;
                $scope.obj.amsT_FatherPANNo = promise.adm_m_student[0].amsT_FatherPANNo;
                $scope.obj.amsT_FatherSubCaste = promise.adm_m_student[0].amsT_FatherChurchAffiliation;
                $scope.obj.amsT_FatherSubCaste = promise.adm_m_student[0].amsT_FatherSelfEmployedFlg;

                if (promise.adm_m_student[0].amsT_MotherAliveFlag === "true") {
                    $scope.obj.amsT_MotherAliveFlag = true;
                }
                else {
                    $scope.obj.amsT_MotherAliveFlag = false;
                }

                $scope.obj.amsT_MotherName = promise.adm_m_student[0].amsT_MotherName;
                $scope.obj.amsT_MotherSurname = promise.adm_m_student[0].amsT_MotherSurname;
                $scope.obj.amsT_MotherAadharNo = promise.adm_m_student[0].amsT_MotherAadharNo;
                $scope.obj.amsT_MotherEducation = promise.adm_m_student[0].amsT_MotherEducation;
                $scope.obj.amsT_MotherOfficeAdd = promise.adm_m_student[0].amsT_MotherOfficeAdd;

                $scope.obj.amsT_MotherMaritalStatus = promise.adm_m_student[0].amsT_MotherMaritalStatus;

                $scope.obj.amsT_MotherPresentAddress = promise.adm_m_student[0].amsT_MotherPresentAddress;
                $scope.obj.amsT_MotherPresentCity = promise.adm_m_student[0].amsT_MotherPresentCity;
                $scope.obj.amsT_MotherPresentState = promise.adm_m_student[0].amsT_MotherPresentState;
                $scope.obj.amsT_MotherPresentPS = promise.adm_m_student[0].amsT_MotherPresentPS;
                $scope.obj.amsT_MotherPresentPO = promise.adm_m_student[0].amsT_MotherPresentPO;
                $scope.obj.amsT_MotherPresentPinCode = promise.adm_m_student[0].amsT_MotherPresentPinCode;

                $scope.obj.amsT_MotherPermanentAddress = promise.adm_m_student[0].amsT_MotherPermanentAddress;
                $scope.obj.amsT_MotherPermanentCity = promise.adm_m_student[0].amsT_MotherPermanentCity;
                $scope.obj.amsT_MotherPermanentState = promise.adm_m_student[0].amsT_MotherPermanentState;
                $scope.obj.amsT_MotherPermanentPS = promise.adm_m_student[0].amsT_MotherPermanentPS;
                $scope.obj.amsT_MotherPermanentPO = promise.adm_m_student[0].amsT_MotherPermanentPO;
                $scope.obj.amsT_MotherPermanentPinCode = promise.adm_m_student[0].amsT_MotherPermanentPinCode;

                $scope.obj.amsT_MotherOccupation = promise.adm_m_student[0].amsT_MotherOccupation;
                $scope.obj.amsT_MotherDesignation = promise.adm_m_student[0].amsT_MotherDesignation;
                $scope.obj.amsT_MotherBankBranch = promise.adm_m_student[0].amsT_MotherBankBranch;
                $scope.obj.amsT_MotherBankName = promise.adm_m_student[0].amsT_MotherBankName;
                $scope.obj.amsT_MotherBankAccNo = promise.adm_m_student[0].amsT_MotherBankAccNo;
                $scope.obj.amsT_MotherBankIFSC_Code = promise.adm_m_student[0].amsT_MotherBankIFSC_Code;
                $scope.obj.amsT_MotherCasteCertiNo = promise.adm_m_student[0].amsT_MotherCasteCertiNo;
                $scope.obj.IVRMMC_Id4 = promise.adm_m_student[0].amsT_MotherNationality;
                $scope.obj.amsT_MotherMonIncome = promise.adm_m_student[0].amsT_MotherMonIncome;
                $scope.obj.amsT_MotherAnnIncome = promise.adm_m_student[0].amsT_MotherAnnIncome;
                $scope.obj.amsT_MotherPANNo = promise.adm_m_student[0].amsT_MotherPANNo;
                $scope.obj.amsT_MotherMobileNo = promise.adm_m_student[0].amsT_MotherMobileNo;
                $scope.obj.amsT_MotherEmailId = promise.adm_m_student[0].amsT_MotherEmailId;
                $scope.obj.amsT_MotherChurchAffiliation = promise.adm_m_student[0].amsT_MotherChurchAffiliation;
                $scope.obj.amsT_MotherSelfEmployedFlg = promise.adm_m_student[0].amsT_MotherSelfEmployedFlg;

                $scope.obj.amsT_MotherOfficePhNo = promise.adm_m_student[0].amsT_MotherOfficePhNo;
                $scope.obj.amsT_MotherHomePhNo = promise.adm_m_student[0].amsT_MotherHomePhNo;
                $scope.obj.amsT_MotherTribe = promise.adm_m_student[0].amsT_MotherTribe;
                $scope.obj.amsT_MotherPassingYear = promise.adm_m_student[0].amsT_MotherPassingYear;
                $scope.obj.amsT_MotherMaritalStatus = promise.adm_m_student[0].amsT_MotherMaritalStatus;

                if (promise.studentAchivementDetails.length > 0) {
                    $scope.obj.amsteC_Extracurricular = promise.studentAchivementDetails[0].amsteC_Extracurricular;
                }

                $scope.obj.amsT_BPLCardFlag = promise.adm_m_student[0].amsT_BPLCardFlag;
                if (promise.adm_m_student[0].amsT_BPLCardFlag == 1) {

                    $scope.obj.amsT_BPLCardFlag = true;
                }
                else {
                    $scope.obj.amsT_BPLCardFlag = false;
                }

                $scope.obj.amsT_BPLCardNo = promise.adm_m_student[0].amsT_BPLCardNo;
                $scope.obj.amsT_HostelReqdFlag = promise.adm_m_student[0].amsT_HostelReqdFlag;
                if ($scope.obj.amsT_HostelReqdFlag === 1) {
                    $scope.obj.amsT_HostelReqdFlag = true;
                }
                else {
                    $scope.obj.amsT_HostelReqdFlag = false;
                }
                $scope.obj.amsT_TransportReqdFlag = promise.adm_m_student[0].amsT_TransportReqdFlag;
                if ($scope.obj.amsT_TransportReqdFlag === 1) {
                    $scope.obj.amsT_TransportReqdFlag = true;
                }
                else {
                    $scope.obj.amsT_TransportReqdFlag = false;
                }
                $scope.obj.amsT_GymReqdFlag = promise.adm_m_student[0].amsT_GymReqdFlag;
                if ($scope.obj.amsT_GymReqdFlag === 1) {
                    $scope.obj.amsT_GymReqdFlag = true;
                }
                else {
                    $scope.obj.amsT_GymReqdFlag = false;
                }
                $scope.obj.amsT_ECSFlag = promise.adm_m_student[0].amsT_ECSFlag;
                if ($scope.obj.amsT_ECSFlag === 1) {
                    $scope.obj.amsT_ECSFlag = true;
                }
                else {
                    $scope.obj.amsT_ECSFlag = false;
                }
                $('#myModalreadmit').modal('hide');
                $scope.readmit = false;
                $scope.readmitreload();
                $scope.amsT_Id = "";
            });
        };

        $scope.removeall = function () {
            $('#myModalreadmit').modal('hide');
            $scope.readmit = false;
            $scope.readmitreload();
            $scope.amsT_Id = "";
        };

        $scope.readmitreload = function () {
            $scope.stdname = [];
            $scope.AMST_Id = "";
            apiService.getDATA("StudentAdmission/Getdetails").then(function (promise) {
                $scope.yearre = promise.academicyearforreadmit;
            });
        };

        //DOCUMENT UPLOADING FUNCTION
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png")
                    && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.amsmD_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadStudentDocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.document_Path = d;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        // UPLOAD STUDENT PROFILE
        $scope.UploadStudentProfilePic = [];
        $scope.uploadStudentProfilePic = function (input, document) {


            if ($scope.photoupload_flag == 'Default') {
                $scope.UploadStudentProfilePic = input.files;
                if (input.files && input.files[0]) {
                    if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                    {
                        var reader = new FileReader();

                        reader.onload = function (e) {
                            $('#blah').attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        Uploadprofile();

                    }
                    else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                        swal("Please Upload the Image file");
                        return;
                    } else if (input.files[0].size > 2097152) {
                        swal("Image size should be less than 2MB");
                        return;
                    }
                }
            }
            else {
                $scope.obj.image = $scope.wbcamurl;
            }

        };
        function Uploadprofile() {



            var formData = new FormData();

            //if ($scope.photoupload_flag == 'Webcam' && $scope.wbcamurl != "") {


            //    var data = {
            //        "PASMD_Path": $scope.wbcamurl
            //    }



            //    apiService.create("StudentAdmission/Capture", data).then(function (promise) {
            //        if (promise.message == "MaxCapacity") {

            //        }


            //    })

            //}
            //else {
            for (var i = 0; i <= $scope.uploadStudentProfilePic.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePic[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    // swal(d);
                    $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // }

            // Uploads1(miid);
        }

        // UPLOAD GUARDIAN PHOTO
        $scope.UploadGuardianPhoto = [];
        $scope.uploadGuardianPhoto = function (input, document) {
            $scope.UploadGuardianPhoto = input.files;
            if (input.files && input.files[0]) {

                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploaddianPhoto(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianPhoto.length; i++) {
                formData.append("File", $scope.UploadGuardianPhoto[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.amstG_GuardianPhoto = d;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        // UPLOAD GUARDINA SIGN
        $scope.UploadGuardianSign = [];
        $scope.uploadGuardianSign = function (input, document) {
            $scope.UploadGuardianSign = input.files;
            if (input.files && input.files[0]) {

                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianSign(document);
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploaddianSign(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianSign.length; i++) {
                formData.append("File", $scope.UploadGuardianSign[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.amstG_GuardianSign = d;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        // UPLOAD GUARDINA FINGER PRINT
        $scope.UploadGuardianFingerprint = [];
        $scope.uploadGuardianFingerprint = function (input, document) {
            $scope.UploadGuardianFingerprint = input.files;
            if (input.files && input.files[0]) {

                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianFingerprint(document);
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image  file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploaddianFingerprint(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianFingerprint.length; i++) {
                formData.append("File", $scope.UploadGuardianFingerprint[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadGuardianDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.amstG_Fingerprint = d;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        // UPLOAD FATHER PHOTO
        $scope.UploadFatherPhoto = [];
        $scope.uploadFatherPhoto = function (input) {
            $scope.UploadFatherPhoto = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFatherProfile();
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploadFatherProfile() {

            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFatherPhoto.length; i++) {
                formData.append("File", $scope.UploadFatherPhoto[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.fatherphoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        // UPLOAD FATHER SIGNATURE
        $scope.UploadFatherSignature = [];
        $scope.uploadFatherSignature = function (input) {
            $scope.UploadFatherSignature = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFathersign();
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploadFathersign() {
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadFatherSignature.length; i++) {
                formData.append("File", $scope.UploadFatherSignature[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.fatherSign = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        // UPLOAD FATHER FINGER PRINTS
        $scope.UploadFatherFingerprints = [];
        $scope.uploadFatherFingerprints = function (input) {
            $scope.UploadFatherFingerprints = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadFatherFingerprnts();
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploadFatherFingerprnts() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFatherFingerprints.length; i++) {
                formData.append("File", $scope.UploadFatherFingerprints[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.fatherFingerprint = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        // UPLOAD MOTHER PHOTO
        $scope.UploadMotherphoto = [];
        $scope.uploadMotherphoto = function (input) {
            $scope.UploadMotherphoto = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMotherProfilepic();
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploadMotherProfilepic() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadMotherphoto.length; i++) {
                formData.append("File", $scope.UploadMotherphoto[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    // swal(d);
                    $scope.motherphoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        // UPLOAD MOTHER SIGN
        $scope.UploadMotherSign = [];
        $scope.uploadMotherSign = function (input) {
            $scope.UploadMotherSign = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMothersignature();
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploadMothersignature() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadMotherSign.length; i++) {
                formData.append("File", $scope.UploadMotherSign[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.mothersign = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        // UPLOAD MOTHER FINGER PRINTS
        $scope.UploadMotherFingerprints = [];
        $scope.uploadMotherFingerprints = function (input) {
            $scope.UploadMotherFingerprints = input.files;
            if (input.files && input.files[0]) {
                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadMotherFing();
                }
                else if (input.files[0].type !== "image/jpeg" && input.files[0].type !== "image/jpg" && input.files[0].type !== "image/png") {
                    swal("Please Upload the Image file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function UploadMotherFing() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadMotherFingerprints.length; i++) {
                formData.append("File", $scope.UploadMotherFingerprints[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadParentsDocs", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.motherfingerprint = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        // VIEW UPLOADED IMAGES        
        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.document_Path);
        };

        $scope.showGuardianPhoto = function (data) {
            $('#preview').attr('src', data.amstG_GuardianPhoto);
        };

        $scope.showGuardianSign = function (data) {
            $('#preview').attr('src', data.amstG_GuardianSign);
        };

        $scope.showGuardianFingerprint = function (data) {
            $('#preview').attr('src', data.amstG_Fingerprint);
        };

        $scope.showfatherPhoto = function (fatherphoto) {
            $('#preview').attr('src', fatherphoto);
        };

        $scope.showfathersign = function (fatherSign) {
            $('#preview').attr('src', fatherSign);
        };

        $scope.showfatherfingerprint = function (fatherFingerprint) {
            $('#preview').attr('src', fatherFingerprint);
        };

        $scope.showmotherprofilepic = function (motherphoto) {
            $('#preview').attr('src', motherphoto);
        };

        $scope.showmothersign = function (mothersign) {
            $('#preview').attr('src', mothersign);
        };

        $scope.showmotherfingerprint = function (motherfingerprint) {
            $('#preview').attr('src', motherfingerprint);
        };

        var imagedownload = "";
        var docname = "";
        $scope.downloaddirectimage = function (data, idd, typeofphoto) {
            var studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '-' + docname + '.jpg'
                    })[0].click();
                });
        };

        $scope.filterValue1 = function (obj) {
            return ($filter('amsT_Date')(obj.asmaY_From_Date, 'dd-MM-yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.studentname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.class)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_Sex)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_RegistrationNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_AdmNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_emailId)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.stdmobilenumber)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_SOL)).indexOf(angular.lowercase($scope.search)) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.interacted4 = function (field) {
            return $scope.submitted4;
        };
        $scope.interacted6 = function (field) {
            return $scope.submitted6;
        };

        //Clear functionality
        $scope.clear_first_tab = function (data) {

            $scope.obj.amsT_StuBankAccNo = "";
            $scope.obj.AMST_BankName = "";
            $scope.obj.AMST_BranchName = "";
            $scope.lblclasssection = "";

            $scope.obj.amsT_BPLCardFlag = "";
            if ($scope.EditId > 0) {
                //dd
            }
            else {
                $scope.obj.stucategory = "";
                $scope.obj.asmcL_Id = "";
                $scope.obj.pasR_Age = "";
                $scope.obj.amsT_BPLCardNo = "";
            }
            $scope.obj.amsT_BiometricId = "";
            $scope.obj.amsT_RFCardNo = "";
            $scope.obj.amsT_BloodGroup = "";
            $scope.obj.amsT_AadharNo = "";
            $scope.obj.amsT_emailId = "";
            $scope.obj.amsT_MobileNo = "";
            $scope.obj.IVRMMC_Id = "";
            $scope.obj.image = "";
            $scope.obj.ivrmmR_Id = "";
            $scope.obj.amsT_MotherTongue = "";
            $scope.obj.amsT_LanguageSpoken = "";
            $scope.amsT_Sex = "";
            $scope.obj.amsT_Tpin = "";
            $scope.obj.amsT_GPSTrackingId = "";
            $scope.obj.amsT_SubCasteIMC_Id = "";
            $scope.obj.amsT_Tribe = "";
            $scope.obj.amsT_BirthCertNO = "";
            $scope.obj.amsT_BirthPlace = "";
            $scope.obj.amsT_StuBankIFSC_Code = "";
            $scope.obj.iC_Id = "";
            $scope.obj.imcC_Id = "";
            $scope.obj.amsT_DOB_Words = "";
            $scope.obj.amsT_DOB = "";
            $scope.amsT_emailId = "";
            $scope.emailsstd = [];
            $scope.emailstd = {};
            $scope.emailsstd = [{ id: 'emailsstd' }];
            $scope.emailsstd[0].amsT_emailId = "";

            $scope.mobilesstd = {};
            $scope.mobilesstd = [{ id: 'mobilesstd' }];
            $scope.mobilesstd[0].amsT_MobileNo = "";

            $scope.obj.amsT_AdmNo = "";
            $scope.obj.amsT_RegistrationNo = "";
            $scope.obj.amsT_LastName = "";
            $scope.obj.amsT_MiddleName = "";
            $scope.obj.amsT_FirstName = "";
            $scope.obj.amsT_StuCasteCertiNo = "";
            $scope.obj.amsT_StudentPANNo = "";
            $scope.search = "";
            angular.forEach(
                angular.element("input[type='file']"),
                function (inputElem) {
                    angular.element(inputElem).val(null);
                });
            $scope.obj.image = "";
            $('#blah').removeAttr('src');
            $scope.obj.feeconcession = "";
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();

        };

        $scope.clear_second_tab = function (data) {
            $scope.obj.amsT_PerStreet = "";
            $scope.obj.amsT_PerArea = "";
            $scope.obj.IVRMMC_Id5 = "";
            $scope.obj.amsT_PerState = "";
            $scope.obj.amsT_PerCity = "";
            $scope.obj.amsT_PerPincode = "";
            $scope.obj.amsT_ConStreet = "";
            $scope.obj.amsT_ConArea = "";
            $scope.obj.amsT_ConCountry = "";
            $scope.obj.amsT_ConState = "";
            $scope.obj.amsT_ConCity = "";
            $scope.obj.amsT_ConPincode = "";
            $scope.search = "";
            $scope.obj.chkbox_address = false;
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };

        $scope.clear_third_tab = function (data) {
            $scope.obj.amsT_FatherAliveFlag = "";
            $scope.obj.amsT_FatherName = "";
            $scope.obj.amsT_FatherSurname = "";
            $scope.obj.amsT_FatherAadharNo = "";
            $scope.obj.amsT_FatherEducation = "";
            $scope.obj.amsT_FatherOfficeAdd = "";
            $scope.obj.amsT_FatherMaritalStatus = "";

            $scope.obj.amsT_FatherPresentAddress = "";
            $scope.obj.amsT_FatherPresentCity = "";
            $scope.obj.amsT_FatherPresentState = "";
            $scope.obj.amsT_FatherPresentPS = "";
            $scope.obj.amsT_FatherPresentPO = "";
            $scope.obj.amsT_FatherPresentPinCode = "";

            $scope.obj.amsT_FatherPermanentAddress = "";
            $scope.obj.amsT_FatherPermanentCity = "";
            $scope.obj.amsT_FatherPermanentState = "";
            $scope.obj.amsT_FatherPermanentPS = "";
            $scope.obj.amsT_FatherPermanentPO = "";
            $scope.obj.amsT_FatherPermanentPinCode = "";

            $scope.obj.amsT_FatherOccupation = "";
            $scope.obj.amsT_FatherDesignation = "";
            $scope.obj.amsT_FatherBankName = "";
            $scope.obj.amsT_FatherBankBranch = "";
            $scope.obj.amsT_FatherBankAccNo = "";
            $scope.obj.amsT_FatherBankIFSC_Code = "";
            $scope.obj.amsT_FatherCasteCertiNo = "";
            $scope.obj.amsT_FatherHomePhNo = "";
            $scope.obj.amsT_FatherPassingYear = "";
            $scope.obj.amsT_FatherOfficePhNo = "";
            $scope.obj.amsT_FatherTribe = "";
            $scope.obj.IVRMMC_Id3 = "";
            $scope.obj.amsT_FatherMonIncome = "";
            $scope.obj.amsT_FatherAnnIncome = "";
            $scope.obj.amsT_FatherSubCaste = "";
            $scope.obj.amsT_FatherSubCaste = false;
            $scope.obj.amsT_FatherMobleNo = "";
            $scope.obj.amsT_FatheremailId = "";
            $scope.obj.amsT_FatherPANNo = "";

            $scope.obj.amsT_MotherAliveFlag = "";
            $scope.obj.amsT_MotherName = "";
            $scope.obj.amsT_MotherSurname = "";
            $scope.obj.amsT_MotherAadharNo = "";
            $scope.obj.amsT_MotherEducation = "";
            $scope.obj.amsT_MotherOfficeAdd = "";

            $scope.obj.amsT_MotherMaritalStatus = "";

            $scope.obj.amsT_MotherPresentAddress = "";
            $scope.obj.amsT_MotherPresentCity = "";
            $scope.obj.amsT_MotherPresentState = "";
            $scope.obj.amsT_MotherPresentPS = "";
            $scope.obj.amsT_MotherPresentPO = "";
            $scope.obj.amsT_MotherPresentPinCode = "";

            $scope.obj.amsT_MotherPermanentAddress = "";
            $scope.obj.amsT_MotherPermanentCity = "";
            $scope.obj.amsT_MotherPermanentState = "";
            $scope.obj.amsT_MotherPermanentPS = "";
            $scope.obj.amsT_MotherPermanentPO = "";
            $scope.obj.amsT_MotherPermanentPinCode = "";

            $scope.obj.amsT_MotherOccupation = "";
            $scope.obj.amsT_MotherDesignation = "";
            $scope.obj.amsT_MotherBankBranch = "";
            $scope.obj.amsT_MotherBankName = "";
            $scope.obj.amsT_MotherBankAccNo = "";
            $scope.obj.amsT_MotherBankIFSC_Code = "";
            $scope.obj.amsT_MotherCasteCertiNo = "";
            $scope.obj.IVRMMC_Id4 = "";
            $scope.obj.amsT_MotherMonIncome = "";
            $scope.obj.amsT_MotherAnnIncome = "";
            $scope.obj.amsT_MotherChurchAffiliation = "";
            $scope.obj.amsT_MotherSelfEmployedFlg = false;
            $scope.obj.amsT_MotherPANNo = "";
            $scope.obj.amsT_MotherMobileNo = "";
            $scope.obj.amsT_MotherEmailId = "";

            $scope.obj.amsT_MotherOfficePhNo = "";
            $scope.obj.amsT_MotherHomePhNo = "";
            $scope.obj.amsT_MotherTribe = "";
            $scope.obj.amsT_MotherPassingYear = "";


            $scope.search = "";
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();

            $scope.obj.amsT_MotherSubCaste = "";
            $scope.obj.amsT_MotherCaste = "";
            $scope.obj.amsT_MotherReligion = "";

            $scope.obj.amsT_FatherSubCaste = "";
            $scope.obj.amsT_FatherCaste = "";
            $scope.obj.amsT_FatherReligion = "";


            $scope.mobiles = {};
            $scope.mobiles = [{ id: 'mobile1' }];
            $scope.mobiles[0].amsT_FatherMobleNo = "";

            $scope.emails = {};
            $scope.emails = [{ id: 'email1' }];
            $scope.emails[0].amsT_FatheremailId = "";

            $scope.mobiles1 = {};
            $scope.mobiles1 = [{ id: 'mobile2' }];
            $scope.mobiles1[0].amsT_MotherMobileNo = "";

            $scope.emails1 = {};
            $scope.emails1 = [{ id: 'email2' }];
            $scope.emails1[0].amsT_MotherEmailId = "";
        };

        var name = "";


        $scope.clear_sixth_tab = function () {
            $scope.obj.AMST_Studentillness = "";
            $scope.obj.AMST_Illnessdetails = "";
            $scope.obj.AMST_UnderAge = "";
            $scope.obj.AMST_MedicalComplaints = "";
            $scope.obj.AMST_Boarding = "";
            $scope.obj.AMST_LastPlayGrndAttnd = "";
            $scope.obj.AMST_AdmissionReason = "";
            $scope.obj.AMST_OtherResidential_Addr = "";
            $scope.obj.AMST_SchoolDISECode = "";
            $scope.obj.AMST_FirstLanguage = "";
            $scope.obj.AMST_Thirdlanguage = "";
            $scope.obj.AMST_OverAge = "";
            $scope.obj.AMST_OtherInformations = "";
            $scope.obj.AMST_ExtraActivity = "";
            $scope.obj.AMST_Stayingwith = "";
            $scope.obj.AMST_MaritalStatus = "";
            $scope.obj.AMST_OtherPermanentAddr = "";
            $scope.obj.AMST_Domicile = "";
            $scope.obj.AMST_SecondLanguage = "";
            $scope.obj.AMST_TransferrableJobFlg = "";
            $scope.obj.AMST_VaccinatedFlg = "";
            $scope.obj.AMST_Tcflag = "";
        };



        $scope.clear_fourth_tab = function () {
            $scope.obj.amsteC_Extracurricular = "";
            $scope.search = "";

            for (var i4 = 0; i4 < $scope.allActivity.length; i4++) {
                $scope.allActivity[i4].Selected = false;
            }

            for (var i1 = 0; i1 < $scope.allRefrence.length; i1++) {
                $scope.allRefrence[i1].Selected = false;
            }

            for (var i2 = 0; i2 < $scope.allSources.length; i2++) {
                $scope.allSources[i2].Selected = false;
            }

            for (var i3 = 0; i3 < $scope.govtBondList.length; i3++) {
                $scope.govtBondList[i3].Selected = false;
                $scope.govtBondList[i3].amstB_BandNo = "";
            }

            for (var i = 0; i < $scope.prevSchoolDetails.length; i++) {
                $scope.prevSchoolDetails[i].amstpS_PrvSchoolName = "";
                $scope.prevSchoolDetails[i].amstpS_PreSchoolType = "";
                $scope.prevSchoolDetails[i].amstpS_PreviousClass = "";
                $scope.prevSchoolDetails[i].amstpS_PreviousPer = "";
                $scope.prevSchoolDetails[i].amstpS_PreviousGrade = "";
                $scope.prevSchoolDetails[i].amstpS_LeftYear = "";
                $scope.prevSchoolDetails[i].amstpS_PreviousMarks = "";
                $scope.prevSchoolDetails[i].amstpS_PreSchoolBoard = "";
                $scope.prevSchoolDetails[i].amstpS_PreSchoolCountry = "";
                $scope.prevSchoolDetails[i].amstpS_PreSchoolState = "";
                $scope.prevSchoolDetails[i].amstpS_Address = "";
                $scope.prevSchoolDetails[i].amstpS_LeftReason = "";
                $scope.prevSchoolDetails[i].amstpS_MediumOfInst = "";
                $scope.prevSchoolDetails[i].amstpS_ConcOrScholarshipFlg = "";
                $scope.prevSchoolDetails[i].amstpS_ConcOrScholarshipDate = "";
                $scope.prevSchoolDetails[i].amstpS_PrvTCNO = "";
                $scope.prevSchoolDetails[i].amstpS_PrvTCDate = "";
            }

            for (var i0 = 0; i0 < $scope.studentGuardianDetails.length; i0++) {


                $scope.studentGuardianDetails[i0].amstG_GuardianName = null;
                // $scope.studentGuardianDetails[i0].amstG_GuardianName = "";

                $scope.studentGuardianDetails[i0].amstG_GuardianAddress = "";
                $scope.studentGuardianDetails[i0].amstG_emailid = "";
                $scope.studentGuardianDetails[i0].amstG_GuardianPhoneNo = "";
                $scope.studentGuardianDetails[i0].amstG_GuardianPhoto = null;
                $scope.studentGuardianDetails[i0].amstG_GuardianSign = null;
                $scope.studentGuardianDetails[i0].amstG_Fingerprint = null;
            }

            for (var i11 = 0; i11 < $scope.studentSiblingDetails.length; i11++) {
                $scope.studentSiblingDetails[i11].amstS_SiblingsName = "";
                $scope.studentSiblingDetails[i11].amcL_Id = "";
                $scope.studentSiblingDetails[i11].amstS_SiblingsRelation = "";
            }

            $scope.obj.amsT_NoOfYoungerSisters = "";
            $scope.obj.AMST_NoOfSiblings = "";
            $scope.obj.AMST_NoOfSiblingsSchool = "";
            $scope.obj.AMST_NoOfDependencies = "";




            $scope.obj.amsT_NoOfElderSisters = "";
            $scope.obj.amsT_NoOfYoungerBrothers = "";
            $scope.obj.amsT_NoOfElderBrothers = "";
            $scope.obj.amsT_Noofbrothers = "";
            $scope.obj.amsT_Noofsisters = "";
            $scope.obj.amsT_ChurchBaptisedDate = null;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
        };

        $scope.clear_fifth_tab = function () {
            $scope.obj.amsT_HostelReqdFlag = "";
            $scope.obj.amsT_TransportReqdFlag = "";
            $scope.obj.amsT_GymReqdFlag = "";
            $scope.obj.amsT_ECSFlag = "";
            $scope.search = "";

            angular.forEach(angular.element("input[type='file']"),
                function (inputElem) {
                    angular.element(inputElem).val(null);
                });

            for (var i = 0; i < $scope.documentList.length; i++) {
                $scope.documentList[i].document_Path = '';
            }

            $scope.fatherphoto = null;
            $scope.UploadFatherPhoto.fatherSign = null;
            $scope.fatherSign = null;
            $scope.fatherFingerprint = null;
            $scope.motherphoto = null;
            $scope.mothersign = null;
            $scope.motherfingerprint = null;
            $scope.submitted = false;
            $scope.submitted1 = false;
            $scope.submitted2 = false;
            $scope.submitted3 = false;
            $scope.submitted4 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        };

        $scope.clearAll = function () {
            $state.reload();
        };

        $scope.reload = function () {
            $state.reload();
        };

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.Parents_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateOtherPrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateMedical = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        }


        $scope.previous_document = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        //month names
        $scope.getmontnames = function (monthid) {
            monthname = "";
            switch (monthid) {
                case 0:
                    monthname = "JANUARY";
                    break;
                case 1:
                    monthname = "FEBRUARY";
                    break;
                case 2:
                    monthname = "MARCH";
                    break;
                case 3:
                    monthname = "APRIL";
                    break;
                case 4:
                    monthname = "MAY";
                    break;
                case 5:
                    monthname = "JUNE";
                    break;
                case 6:
                    monthname = "JULY";
                    break;
                case 7:
                    monthname = "AUGUST";
                    break;
                case 8:
                    monthname = "SEPTEMBER";
                    break;
                case 9:
                    monthname = "OCTOBER";
                    break;
                case 10:
                    monthname = "NOVEMBER";
                    break;
                case 11:
                    monthname = "DECEMBER";
                    break;
                default:
                    monthname = "";
                    break;
            }
            return monthname;
        };

        //get datename
        $scope.getdatenames = function (dateid) {

            datename = "";
            switch (dateid) {

                case 1:
                    datename = "FIRST";
                    break;
                case 2:
                    datename = "SECOND";
                    break;
                case 3:
                    datename = "THIRD";
                    break;
                case 4:
                    datename = "FOURTH";
                    break;
                case 5:
                    datename = "FIFTH";
                    break;
                case 6:
                    datename = "SIXTH";
                    break;
                case 7:
                    datename = "SEVENTH";
                    break;
                case 8:
                    datename = "EIGHTH";
                    break;
                case 9:
                    datename = "NINTH";
                    break;
                case 10:
                    datename = "TENTH";
                    break;
                case 11:
                    datename = "ELEVENTH";
                    break;
                case 12:
                    datename = "TWELFTH";
                    break;
                case 13:
                    datename = "THIRTEENTH";
                    break;
                case 14:
                    datename = "FOURTEENTH";
                    break;
                case 15:
                    datename = "FIFTEENTH";
                    break;
                case 16:
                    datename = "SIXTEENTH";
                    break;
                case 17:
                    datename = "SEVENTEENTH";
                    break;
                case 18:
                    datename = "EIGHTEENTH";
                    break;
                case 19:
                    datename = "NINTEENTH";
                    break;
                case 20:
                    datename = "TWENTY";
                    break;
                case 21:
                    datename = "TWENTY FIRST";
                    break;
                case 22:
                    datename = "TWENTY SECOND";
                    break;
                case 23:
                    datename = "TWENTY THIRD";
                    break;
                case 24:
                    datename = "TWENTY FOURTH";
                    break;
                case 25:
                    datename = "TWENTY FIFTH";
                    break;
                case 26:
                    datename = "TWENTY SIXTH";
                    break;
                case 27:
                    datename = "TWENTY SEVENTH";
                    break;
                case 28:
                    datename = "TWENTY EIGHTH";
                    break;
                case 29:
                    datename = "TWENTY NINTH";
                    break;
                case 30:
                    datename = "THIRTIETH";
                    break;
                case 31:
                    datename = "THIRTY FIRST";
                    break;

                default:
                    datename = "";
                    break;
            }
            return datename;

        }

        //getting year 1
        $scope.getyearname = function (yearid) {

            var yearid = yearid.toString();

            var yearsplit = yearid.substring(0, 2);
            var dob = parseInt(yearsplit);

            var yearsplit1 = yearid.substring(2, 4);
            var dob1 = parseInt(yearsplit1);

            yearname = "";
            var datad = yearid;
            var yearname1 = "";



            //var dob = datad(1, 2);
            //var dob1 = datad(3, 4);

            if (dob >= 20) {
                yearname = "TWO THOUSAND";
            }
            else {
                yearname = "NINTEEN";
            }
            switch (dob1) {

                case 1:
                    yearname1 = "ONE";
                    break;
                case 2:
                    yearname1 = "TWO";
                    break;
                case 3:
                    yearname1 = "THREE";
                    break;
                case 4:
                    yearname1 = "FOUR";
                    break;
                case 5:
                    yearname1 = "FIVE";
                    break;
                case 6:
                    yearname1 = "SIX";
                    break;
                case 7:
                    yearname1 = "SEVEN";
                    break;
                case 8:
                    yearname1 = "EIGHT";
                    break;
                case 9:
                    yearname1 = "NINE";
                    break;
                case 10:
                    yearname1 = "TEN";
                    break;
                case 11:
                    yearname1 = "ELEVEN";
                    break;
                case 12:
                    yearname1 = "TWELVE";
                    break;
                case 13:
                    yearname1 = "THIRTEEN";
                    break;
                case 14:
                    yearname1 = "FOURTEEN";
                    break;
                case 15:
                    yearname1 = "FIFTEEN";
                    break;
                case 16:
                    yearname1 = "SIXTEEN";
                    break;
                case 17:
                    yearname1 = "SEVENTEEN";
                    break;
                case 18:
                    yearname1 = "EIGHTEEN";
                    break;
                case 19:
                    yearname1 = "NINTEEN";
                    break;
                case 20:
                    yearname1 = "TWENTY";
                    break;
                case 21:
                    yearname1 = "TWENTY ONE";
                    break;
                case 22:
                    yearname1 = "TWENTY TWO";
                    break;
                case 23:
                    yearname1 = "TWENTY THREE";
                    break;
                case 24:
                    yearname1 = "TWENTY FOUR";
                    break;
                case 25:
                    yearname1 = "TWENTY FIVE";
                    break;
                case 26:
                    yearname1 = "TWENTY SIX";
                    break;
                case 27:
                    yearname1 = "TWENTY SEVEN";
                    break;
                case 28:
                    yearname1 = "TWENTY EIGHT";
                    break;
                case 29:
                    yearname1 = "TWENTY NINE";
                    break;
                case 30:
                    yearname1 = "THIRTY";
                    break;
                case 31:
                    yearname1 = "THIRTY ONE";
                    break;
                case 32:
                    yearname1 = "THIRTY TWO";
                    break;
                case 33:
                    yearname1 = "THIRTY THREE";
                    break;
                case 34:
                    yearname1 = "THIRTY FOUR";
                    break;
                case 35:
                    yearname1 = "THIRTY FIVE";
                    break;
                case 36:
                    yearname1 = "THIRTY SIX";
                    break;
                case 37:
                    yearname1 = "THIRTY SEVEN";
                    break;
                case 38:
                    yearname1 = "THIRTY EIGHT";
                    break;
                case 39:
                    yearname1 = "THIRTY NINE";
                    break;
                case 40:
                    yearname1 = "FOURTY";
                    break;
                case 41:
                    yearname1 = "FOURTY ONE";
                    break;
                case 42:
                    yearname1 = "FOURTY TWO";
                    break;
                case 43:
                    yearname1 = "FOURTY THREE";
                    break;
                case 44:
                    yearname1 = "FOURTY FOUR";
                    break;
                case 45:
                    yearname1 = "FOURTY FIVE";
                    break;
                case 46:
                    yearname1 = "FOURTY SIX";
                    break;
                case 47:
                    yearname1 = "FOURTY SEVEN";
                    break;
                case 48:
                    yearname1 = "FOURTY EIGHT";
                    break;
                case 49:
                    yearname1 = "FOURTY NINE";
                    break;
                case 50:
                    yearname1 = "FIFTY";
                    break;
                case 51:
                    yearname1 = "FIFTY ONE";
                    break;
                case 52:
                    yearname1 = "FIFTY TWO";
                    break;
                case 53:
                    yearname1 = "FIFTY THREE";
                    break;
                case 54:
                    yearname1 = "FIFTY FOUR";
                    break;
                case 55:
                    yearname1 = "FIFTY FIVE";
                    break;
                case 56:
                    yearname1 = "FIFTY SIX";
                    break;
                case 57:
                    yearname1 = "FIFTY SEVEN";
                    break;
                case 58:
                    yearname1 = "FIFTY EIGHT";
                    break;
                case 59:
                    yearname1 = "FIFTY NINE";
                    break;
                case 60:
                    yearname1 = "SIXTY";
                    break;

                case 61:
                    yearname1 = "SIXTY ONE";
                    break;
                case 62:
                    yearname1 = "SIXTY TWO";
                    break;
                case 63:
                    yearname1 = "SIXTY THREE";
                    break;
                case 64:
                    yearname1 = "SIXTY FOUR";
                    break;
                case 65:
                    yearname1 = "SIXTY FIVE";
                    break;
                case 66:
                    yearname1 = "SIXTY SIX";
                    break;
                case 67:
                    yearname1 = "SIXTY SEVEN";
                    break;
                case 68:
                    yearname1 = "SIXTY EIGHT";
                    break;
                case 69:
                    yearname1 = "SIXTY NINE";
                    break;
                case 70:
                    yearname1 = "SEVENTY";
                    break;

                case 71:
                    yearname1 = "SEVENTY ONE";
                    break;
                case 72:
                    yearname1 = "SEVENTY TWO";
                    break;
                case 73:
                    yearname1 = "SEVENTY THREE";
                    break;
                case 74:
                    yearname1 = "SEVENTY FOUR";
                    break;
                case 75:
                    yearname1 = "SEVENTY FIVE";
                    break;
                case 76:
                    yearname1 = "SEVENTY SIX";
                    break;
                case 77:
                    yearname1 = "SEVENTY SEVEN";
                    break;
                case 78:
                    yearname1 = "SEVENTY EIGHT";
                    break;
                case 79:
                    yearname1 = "SEVENTY NINE";
                    break;
                case 80:
                    yearname1 = "EIGHTY";
                    break;

                case 81:
                    yearname1 = "EIGHTY ONE";
                    break;
                case 82:
                    yearname1 = "EIGHTY TWO";
                    break;
                case 83:
                    yearname1 = "EIGHTY THREE";
                    break;
                case 84:
                    yearname1 = "EIGHTY FOUR";
                    break;
                case 85:
                    yearname1 = "EIGHTY FIVE";
                    break;
                case 86:
                    yearname1 = "EIGHTY SIX";
                    break;
                case 87:
                    yearname1 = "EIGHTY SEVEN";
                    break;
                case 88:
                    yearname1 = "EIGHTY EIGHT";
                    break;
                case 89:
                    yearname1 = "EIGHTY NINE";
                    break;
                case 90:
                    yearname1 = "NINTY";
                    break;

                case 91:
                    yearname1 = "NINTY ONE";
                    break;
                case 92:
                    yearname1 = "NINTY TWO";
                    break;
                case 93:
                    yearname1 = "NINTY THREE";
                    break;
                case 94:
                    yearname1 = "NINTY FOUR";
                    break;
                case 95:
                    yearname1 = "NINTY FIVE";
                    break;
                case 96:
                    yearname1 = "NINTY SIX";
                    break;
                case 97:
                    yearname1 = "NINTY SEVEN";
                    break;
                case 98:
                    yearname1 = "NINTY EIGHT";
                    break;
                case 99:
                    yearname1 = "NINTY NINE";
                    break;
                case 0:
                    yearname1 = "";
                    break;

                default:
                    yearname1 = "";
                    break;
            }
            yearname = yearname + ' ' + yearname1;
            return yearname;
        };

        $scope.ViewStudentProfile = function (dd) {
            $scope.myTabIndex2 = 0;
            var data = {
                "AMST_Id": dd.amsT_Id
            };

            apiService.create("StudentAdmission/ViewStudentProfile", data).then(function (promise) {

                if (promise !== null) {
                    $scope.viewstudentjoineddetails = promise.viewstudentjoineddetails;

                    if ($scope.viewstudentjoineddetails !== null && $scope.viewstudentjoineddetails.length > 0) {
                        $scope.studentname_view = $scope.viewstudentjoineddetails[0].studentname;
                        $scope.amstadmno_view = $scope.viewstudentjoineddetails[0].amsT_AdmNo;
                        $scope.amstregno_view = $scope.viewstudentjoineddetails[0].amsT_RegistrationNo;
                        $scope.year_view = $scope.viewstudentjoineddetails[0].asmaY_Year;
                        $scope.class_view = $scope.viewstudentjoineddetails[0].asmcL_ClassName;
                        $scope.photo_view = $scope.viewstudentjoineddetails[0].amsT_Photoname;
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
                            $scope.Father_photo = $scope.viewstudentdetails[0].ansT_FatherPhoto;


                            //Mother Details
                            $scope.MotherName = $scope.viewstudentdetails[0].amsT_MotherName;
                            $scope.MotherSurName = $scope.viewstudentdetails[0].amsT_MotherSurname === null || $scope.viewstudentdetails[0].amsT_MotherSurname === "" || $scope.viewstudentdetails[0].amsT_MotherSurname === "0" ? "" : ' ' + $scope.viewstudentdetails[0].amsT_MotherSurname;
                            $scope.Mother_MobileNo = $scope.viewstudentdetails[0].amsT_MotherMobileNo;
                            $scope.Mother_EmailId = $scope.viewstudentdetails[0].amsT_MotherEmailId;
                            $scope.Mother_photo = $scope.viewstudentdetails[0].ansT_MotherPhoto;

                        }

                        if (promise.viewstudentacademicyeardetails !== null && promise.viewstudentacademicyeardetails.length > 0) {
                            $scope.viewstudentacademicyeardetails = promise.viewstudentacademicyeardetails;
                        }

                        if (promise.viewstudentguardiandetails !== null && promise.viewstudentguardiandetails.length > 0) {
                            $scope.viewstudentguardiandetails = promise.viewstudentguardiandetails;
                        }

                        if (promise.viewstudentattendancetails !== null && promise.viewstudentattendancetails.length > 0) {
                            $scope.viewstudentattendancetails = promise.viewstudentattendancetails;
                        }

                        if (promise.viewstudentsubjectdetails !== null && promise.viewstudentsubjectdetails.length > 0) {
                            $scope.viewstudentsubjectdetails = promise.viewstudentsubjectdetails;
                        }

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
    }
})();