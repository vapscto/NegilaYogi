
(function () {
    'use strict';
    angular

        .module('app')
        .controller('DocumentViewClgController', DocumentViewClgController);
    DocumentViewClgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce', 'uiCalendarConfig', 'appSettings', '$compile'];

    function DocumentViewClgController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce, uiCalendarConfig, appSettings, $compile) {


        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });
        $('body').on('hidden.bs.modal', function () {
            if ($('.modal.in').length > 0) {
                $('body').addClass('modal-open');
            }
        });

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs != null) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }
       

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));
        if (configsettings != null && configsettings.length > 0) {
            if (configsettings.length > 0) {
                paginationformasters = configsettings[0].ispaC_ApplFeeFlag;
            }
        }

        var imagedownload = "";
        var docname = "";
        var ftype = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            }
        }
     
       // $scope.itemsPerPages = paginationformasters;
        $scope.itemsPerPages = 10;
        $scope.currentPages = 1;


        $scope.submitted = false;
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {


            apiService.get("DocumentViewClg/getdetails/2").then(function (promise) {

                $scope.year_list = promise.fillyear
                $scope.courselists = promise.allcourse;
            })

        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //TO clear  data
        $scope.clearid = function () {

            //$scope.asmaY_Id = "";
            //$scope.AMCO_Id = "";
            //$scope.reportgrid = false;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //$scope.courselists = [];

            $state.reload();

        };

        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');

        };


        $scope.doclist = [];

        $scope.loaddata = function () {
            $scope.submitted = true;
            $scope.currentPage = 1;
           // $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage = 10;
            if ($scope.myForm.$valid) {

                $scope.courselistarray = [];
                $scope.branchlistarray = [];
                $scope.semesterlistarray = [];

                if ($scope.courselists.length > 0 || $scope.courselists != null) {
                    angular.forEach($scope.courselists, function (qq) {
                        if (qq.selected == true) {
                            $scope.courselistarray.push({ AMCO_Id: qq.amcO_Id })

                        }
                    });
                }

                if ($scope.branchlist != null && $scope.branchlist.length > 0) {

                    angular.forEach($scope.branchlist, function (qq) {
                        if (qq.selected == true) {

                            $scope.branchlistarray.push({ AMB_Id: qq.amB_Id })
                        }
                    });
                }

                if ($scope.semesterlist != null && $scope.semesterlist.length > 0) {

                    angular.forEach($scope.semesterlist, function (qq) {
                        if (qq.selected == true) {

                            $scope.semesterlistarray.push({ AMSE_Id: qq.amsE_Id })
                        }
                    });
                }

                var data = {
                    "configurationsettings1": configsettings[0].ispaC_ApplFeeFlag,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "courselsttwo": $scope.courselistarray,
                    "branchlisttwo": $scope.branchlistarray,
                    "semesterlisttwo": $scope.semesterlistarray

                }
                apiService.create("DocumentViewClg/getclgstudata", data).then(function (promise) {


                    if (promise.registrationList == null || promise.registrationList.length == 0) {
                        $scope.reportgrid = false;
                        swal("Records Are Found For Your Selection !!!")
                    }
                    else {
                        $scope.pages = promise.registrationList;
                        $scope.presentCountgrid = promise.registrationList.length;
                        $scope.prospectusPaymentlist = promise.prospectusPaymentlist;
                        $scope.doclist = promise.ddoc;              
                        if (promise.admissioncatdrpall.length > 0) {
                            for (var i = 0; i < $scope.pages.length; i++) {
                                for (var j = 0; j < promise.admissioncatdrpall.length; j++) {
                                    if ($scope.pages[i].amcO_Id == promise.admissioncatdrpall[j].amcO_Id) {
                                        $scope.pages[i].classname = promise.admissioncatdrpall[j].amcO_CourseName;
                                    }
                                }
                            }
                        }


                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < $scope.pages.length; i++) {
                            if ($scope.pages[i].pacA_FirstName != '') {
                                if ($scope.pages[i].pacA_MiddleName !== null) {
                                    if ($scope.pages[i].pacA_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: $scope.pages[i].pacA_FirstName + " " + $scope.pages[i].pacA_MiddleName + " " + $scope.pages[i].pacA_LastName, pacA_Id: $scope.pages[i].pacA_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.pages[i].pacA_FirstName + '' + $scope.pages[i].pacA_MiddleName, pacA_Id: $scope.pages[i].pacA_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: $scope.pages[i].pacA_FirstName, pacA_Id: $scope.pages[i].pacA_Id });
                                }

                                $scope.pages[i].name = $scope.albumNameArray1[i].name;
                            }
                        }
                        for (var i = 0; i < configsettings.length; i++) {
                            if (configsettings.length > 0) {
                                $scope.configurationsettings = configsettings[i];
                                if ($scope.configurationsettings.ispaC_ApplFeeFlag === 1) {

                                    $scope.ispaC_ApplFeeFlag = $scope.configurationsettings.ispaC_ApplFeeFlag;
                                    $scope.prosH = true;
                                    $scope.prosL = true;
                                }
                                else {
                                    $scope.ispaC_ApplFeeFlag = 0;
                                    $scope.prosH = true;
                                    $scope.prosL = true;
                                }
                            }
                        }
                        // for pagination
                        //documnets
                        if (promise.ddoc.length > 0) {
                            angular.forEach($scope.doclist, function (obj) {
                                $('#' + obj.amsmD_Id).attr('src', obj.document_Path);
                                var img = obj.document_Path;
                                obj.document_Pathtemp = obj.document_Path;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];

                                obj.filetype = lastelement;


                                if (obj.filetype === 'xlsx' || obj.filetype === 'xls' || obj.filetype === 'xlsm' || obj.filetype === 'docx' || obj.filetype === 'doc') {
                                    obj.document_Path = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.document_Path;
                                }
                            });
                            console.log($scope.doclist);
                        }
                        $scope.currentPage = 1;
                       // $scope.itemsPerPage = paginationformasters;
                        $scope.itemsPerPage = 10;
                        $scope.reportgrid = true;
                    }
                });
            }
        };

        $scope.search = '';
        $scope.filterValue = function (obj) {

            return angular.lowercase(obj.name).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(obj.pacA_RegistrationNo).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(obj.classname).indexOf(angular.lowercase($scope.search)) >= 0;
        }

        $scope.clear = function () {
            $scope.pages = [];
        }
        $scope.cleartheapplication = function () {

            $scope.serilano = "";
            $scope.pasa_id = "";
            $scope.PACA_FirstName = "";
            $scope.PACA_MiddleName = "";
            $scope.PACA_LastName = "";
            $scope.namelength = 0;
            $scope.PACA_Date = "";
            $scope.PACA_RegistrationNo = "";
            $scope.PACA_Sex = "";
            $scope.PACA_TransferrableJobFlg = "";
            $scope.PACA_DOB = "";

            $scope.PACA_Age = "";
            $scope.PACA_DOBWords = "";
            $scope.amcO_CourseName = "";
            $scope.PACA_BloodGroup = "";
            $scope.PACA_Emisno = "";
            $scope.medium = "";
            $scope.PACA_Boarding = "";
            $scope.PACA_MotherTongue = "";
            $scope.religionname = "";
            $scope.concessioncat = "";

            $scope.IMCC_CategoryName = "";
            $scope.IMC_CasteName = "";

            $scope.PACA_PerStreet = "";
            $scope.PACA_PerArea = "";
            $scope.PACA_PerCity = "";
            $scope.PACA_PerStaten = "";
            $scope.PACA_PerCountryn = "";
            $scope.PACA_PerPincode = "";


            $scope.PACA_ConStreet = "";
            $scope.PACA_ConArea = "";
            $scope.PACA_ConCity = "";
            $scope.PACA_ConStaten = "";
            $scope.PACA_ConCountryn = "";
            $scope.PACA_ConPincode = "";

            $scope.PACA_AadharNo = "";

            $scope.PACA_Distirct = "";
            $scope.PACA_Taluk = "";
            $scope.PACA_Village = "";
            $scope.PACA_Stayingwith = "";

            $scope.PACA_Languagespeaking = "";
            $scope.PACA_FatherPanno = "";
            $scope.PACA_MotherPanno = "";

            $scope.PACA_MobileNo = "";
            $scope.PACA_emailId = "";
            $scope.PACA_MaritalStatus = "";

            $scope.PACA_FatherAliveFlag = "";
            $scope.PACA_FatherName = "";
            $scope.PACA_FatherAadharNo = "";
            $scope.PACA_FatherSurname = "";
            $scope.PACA_FatherEducation = "";
            $scope.PACA_FatherOccupation = "";
            $scope.PACA_FatherDesignation = "";
            $scope.PACA_FatherIncome = "";
            $scope.PACA_FatherMobleNo = "";
            $scope.PACA_FatheremailId = "";

            $scope.syllabussname = "";
            $scope.PACA_FatherReligion = "";
            $scope.PACA_FatherCaste = "";
            $scope.fatherSubCaste_Id = "";
            $scope.SubCaste_Id = "";
            $scope.motherSubCaste_Id = "";

            $scope.PACA_FatherTribe = "";
            $scope.PACA_Tribe = "";
            $scope.PACA_MotherReligion = "";
            $scope.PASR_MotherCaste = "";
            $scope.PACA_MotherTribe = "";

            $scope.PACA_FirstLanguage = "";
            $scope.PACA_Thirdlanguage = "";
            $scope.PACA_SecondLanguage = "";
            $scope.PACA_MotherAliveFlag = "";
            $scope.PACA_MotherName = "";
            $scope.PACA_MotherAadharNo = "";
            $scope.PACA_MotherSurname = "";
            $scope.PACA_MotherEducation = "";
            $scope.PACA_MotherOccupation = "";
            $scope.PACA_MotherDesignation = "";
            $scope.PACA_MotherIncome = "";
            $scope.PACA_MotherMobleNo = "";
            $scope.PACA_MotheremailId = "";


            $scope.PACA_ChurchAddress = "";
            $scope.PACA_Churchname = "";

            $scope.PACA_FatherOfficePhNo = "";
            $scope.PACA_FatherHomePhNo = "";
            $scope.PACA_MotherOfficePhNo = "";
            $scope.PACA_MotherHomePhNo = "";

            $scope.PACA_FatherPassingYear = "";
            $scope.PACA_MotherPassingYear = "";

            $scope.PACA_TotalIncome = "";
            $scope.PACA_BirthPlace = "";
            $scope.studentnationality = "";

            $scope.PACA_HostelReqdFlag = "";
            $scope.PACA_TransportReqdFlag = "";
            $scope.PACA_GymReqdFlag = "";
            $scope.PACA_ECSFlag = "";
            $scope.PACA_PaymentFlag = "";

            $scope.PACA_AmountPaid = "";
            $scope.PACA_PaymentType = "";
            $scope.PACA_PaymentDate = "";
            $scope.PACA_ReceiptNo = "";

            $scope.PACA_FinalpaymentFlag = "";
            $scope.PACA_LastPlayGrndAttnd = "";
            $scope.PACA_ExtraActivity = "";
            $scope.PACA_OtherResidential_Addr = "";
            $scope.PACA_OtherPermanentAddr = "";
            $scope.PACA_MotherOfficeAddr = "";
            $scope.PACA_UndertakingFlag = "";
            $scope.fathernationality = "";
            $scope.mothernationality = "";
            $scope.PACA_BirthCertificateNo = "";
            $scope.PACA_AltContactNo = "";
            $scope.PACA_AltContactEmail = "";

            $scope.pasrfathrnature = "";
            $scope.pasrfathradd = "";
            $scope.pasrfathrnature1 = "";
            $scope.pasrfathradd1 = "";
            $scope.pasrmothrnature = "";
            $scope.pasrmothradd = "";
            $scope.pasrmothrnature1 = "";
            $scope.pasrmothradd1 = "";
            $scope.selfMotherOccupation = "";
            $scope.selfMotherOccupation = "";

            $('#blahnew').attr('src', "");

            $scope.studentphoto = "";

            $scope.PACA_Noofbrothers = "";
            $scope.PACA_Noofsisters = "";
            $scope.PACA_lastclassperc = "";
            $scope.PACA_SibblingConcessionFlag = "";
            $scope.PACA_ParentConcessionFlag = "";



            $scope.pasrG_Id = "";
            $scope.PASRG_GuardianName = "";
            $scope.PASRG_GuardianAddress = "";
            $scope.PASRG_GuardianRelation = "";
            $scope.PASRG_emailid = "";
            $scope.PASRG_GuardianPhoneNo = "";
            $scope.PASRG_Occupation = "";
            $scope.PASRG_PhoneOffice = "";


            $scope.firstsibling = '';
            $scope.firstsiblingclass = '';
            $scope.secondsibling = '';
            $scope.secondsiblingclass = '';


            $scope.siblingsprint = "";

            $scope.firstsibling = "";
            $scope.firstsiblingclass = "";
            $scope.firstsiblingschoolAdmissionNo = "";
            $scope.secondsiblingschoolAdmissionNo = "";
            $scope.secondsiblingschool = "";
            $scope.firstsiblingschool = "";

            $scope.secondsibling = "";
            $scope.secondsiblingclass = "";



            $scope.sibl = "Yes!";
            $scope.showbr = false;
            $scope.siblingshow = true;

            $scope.albumNameArray1 = [];
            $scope.albumNameArray2 = [];
            $scope.Hstuname = "";
            $scope.Hstuage = "";
            $scope.HFirstName = "";
            $scope.VaccinationDate = "";

            $scope.HepatitisB = "";
            $scope.TyphoidFever = "";
            $scope.Ailment = "";

            $scope.HealthProblem = "";
            $scope.CronicDesease = "";
            $scope.MEDetails = "";
            $scope.MEContactNo = "";
            $scope.vaccinesprint = "";

            $scope.PreviousSchoolList = "";

            $scope.schoolname = "";
            $scope.schooladress = "";
            $scope.lastclass = "";
            $scope.lastsylaabus = "";
            $scope.percentage = "";
            $scope.leftdate = "";
            $scope.leftreason = "";

            $('#blahfnew').attr('src', "");
            $('#blahmnew').attr('src', "");
            $('#blahnewa').attr('src', "");
            $('#blahnewaa').attr('src', "");
            //var e1 = angular.element(document.getElementById("test"));
            //$compile(e1.html(promise.htmldata))(($scope));

            $('#blahnew').attr('src', "");
            $('#blahfnew').attr('src', "");
            $('#blahmnew').attr('src', "");
            $('#blahnewa').attr('src', "");
            $('#blahnewaa').attr('src', "");

        }

        $scope.Clgapplicationdata = function (studentid) {


            apiService.getURI("ApplicationForm/Clgapplicationdata/", studentid).then(function (promise) {
                if (promise.studentList != null || promise.studentList.length > 0) {

                    //$scope.documentList = promise.documentList;
                    //$scope.DOB = false;
                    //$scope.mi_id = promise.mI_Id;


                    if (promise.prevSchoolDetails != null) {
                        if (promise.prevSchoolDetails.length > 0) {

                            $scope.prevSchoolDetails = promise.prevSchoolDetails;
                            $scope.prevschlcount = promise.prevSchoolDetails.length;

                            for (var i = 0; i < promise.prevSchoolDetails.length; i++) {
                                $scope.pacstpS_PreSchoolBoard = promise.prevSchoolDetails[i].pacstpS_PreSchoolBoard;
                                $scope.pacstpS_PreviousExamPassed = promise.prevSchoolDetails[i].pacstpS_PreviousExamPassed;
                                $scope.pacstpS_PreSchoolType = promise.prevSchoolDetails[i].pacstpS_PreSchoolType;

                                $scope.pacstpS_PreviousMarks = promise.prevSchoolDetails[i].pacstpS_PreviousMarks;
                                $scope.pacstpS_PreviousMarksObtained = promise.prevSchoolDetails[i].pacstpS_PreviousMarksObtained;
                                $scope.pacstpS_PreviousGrade = promise.prevSchoolDetails[i].pacstpS_PreviousGrade;

                                $scope.pacstpS_PrvSchoolName = promise.prevSchoolDetails[i].pacstpS_PrvSchoolName;
                                $scope.pacstpS_PreviousExamPassed = promise.prevSchoolDetails[i].pacstpS_PreviousExamPassed;
                                $scope.pacstpS_Urbanrural = promise.prevSchoolDetails[i].pacstpS_Urbanrural;
                                $scope.pacstpS_Attempts = promise.prevSchoolDetails[i].pacstpS_Attempts;
                                $scope.pacstpS_PreviousClass = promise.prevSchoolDetails[i].pacstpS_PreviousClass;
                                $scope.pacstpS_PreviousTCNo = promise.prevSchoolDetails[i].pacstpS_PreviousTCNo;
                                $scope.pacstpS_PreviousRegNo = promise.prevSchoolDetails[i].pacstpS_PreviousRegNo;
                                $scope.pacstpS_PreviousBranch = promise.prevSchoolDetails[i].pacstpS_PreviousBranch;
                                $scope.pacstpS_MediumOfInst = promise.prevSchoolDetails[i].pacstpS_MediumOfInst;
                                $scope.pacstpS_PasssedMonthYear = promise.prevSchoolDetails[i].pacstpS_PasssedMonthYear;
                                $scope.pacstpS_LanguagesTaken = promise.prevSchoolDetails[i].pacstpS_LanguagesTaken;
                                $scope.pacstpS_TCDate = new Date(promise.prevSchoolDetails[i].pacstpS_TCDate);
                                $scope.pacstpS_LeftYear = promise.prevSchoolDetails[i].pacstpS_LeftYear;
                                $scope.studentpreviousstate = promise.studentpreviousstate.length > 0 ? promise.studentpreviousstate[0].studpreviousstate : "";

                                var doobTc = promise.prevSchoolDetails[i].pacstpS_TCDate;

                                var doobyrtc = doobTc.substring(0, 4);
                                var doobmnthtc = doobTc.substring(5, 7);
                                var doobdaystc = doobTc.substring(8, 10);

                                $scope.b1tc = doobdaystc.substring(0, 1);
                                $scope.b2tc = doobdaystc.substring(1, 2);
                                $scope.BTC1 = $scope.b1tc + $scope.b2tc;

                                $scope.b3tc = doobmnthtc.substring(0, 1);
                                $scope.b4tc = doobmnthtc.substring(1, 2);
                                $scope.BTC2 = $scope.b3tc + $scope.b4tc;

                                $scope.b5tc = doobyrtc.substring(0, 1);
                                $scope.b6tc = doobyrtc.substring(1, 2);
                                $scope.b7tc = doobyrtc.substring(2, 3);
                                $scope.b8tc = doobyrtc.substring(3, 4);
                                $scope.BTC3 = $scope.b5tc + $scope.b6tc + $scope.b7tc + $scope.b8tc;

                                //$scope.pacstpS_PreSchoolCountry = promise.prevSchoolDetails[i].pacstpS_PreSchoolCountry;
                                //  getPrevGetState(promise.prevSchoolDetails[i].pacstpS_PreSchoolCountry, promise.prevSchoolDetails[i].pacstpS_PreSchoolState);
                                //$scope.onselectprevCountry($scope.pacstpS_PreSchoolCountry);
                                $scope.pacstpS_PreSchoolState = promise.prevSchoolDetails[i].pacstpS_PreSchoolState;
                            }
                        }
                    }
                    else {
                        $scope.prevSchoolDetails = [{ id: 'prevSchool1' }];
                        $scope.prevschlcount = 0;
                        $scope.pacstpS_Attempts = 1;
                    }
                    if (promise.studentGuardianDetails != null) {
                        if (promise.studentGuardianDetails.length > 0) {
                            $scope.studentGuardianDetails = promise.studentGuardianDetails;
                            $scope.grddetcount = promise.studentGuardianDetails.length;
                        }
                    }

                    else {
                        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
                        $scope.grddetcount = 0;
                    }

                    if (promise.studentsubjectmarksarry != null) {
                        if (promise.studentsubjectmarksarry.length > 0) {

                            $scope.prevexammarksdetailsprint = promise.studentsubjectmarksarry;
                            $scope.prevexammarksdetailscountprint = promise.studentsubjectmarksarry.length;
                        }
                    }

                    //documnets
                    if (promise.documentList != null) {
                        if (promise.documentList.length > 0) {
                            $scope.document = {};
                            $scope.documentList = promise.documentList;
                            angular.forEach(promise.documentList, function (value, key) {
                                $('#' + value.amsmD_Id).attr('src', value.document_Path);
                            })
                        }
                    }




                    $('#blah').attr('src', promise.studentList[0].pacA_StudentPhoto);



                    $scope.fatherphoto = promise.studentList[0].pacA_FatherPhoto;
                    $scope.fatherSign = promise.studentList[0].pacA_FatherSign;
                    $scope.fatherFingerprint = promise.studentList[0].pacA_FatherFingerprint;
                    $scope.motherphoto = promise.studentList[0].pacA_MotherPhoto;
                    $scope.mothersign = promise.studentList[0].pacA_MotherSign;
                    $scope.motherfingerprint = promise.studentList[0].pacA_MotherFingerprint;
                    $scope.image = promise.studentList[0].pacA_StudentPhoto;

                    $scope.stuimage = promise.studentList[0].pacA_StudentPhoto;
                    $('#blahnewa').attr('src', promise.studentList[0].pacA_StudentPhoto);



                    if (promise.studentList != null || promise.studentList.length > 0) {
                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < promise.studentList.length; i++) {
                            if (promise.studentList[i].pacA_FirstName != '') {
                                if (promise.studentList[i].pacA_MiddleName !== null) {
                                    if (promise.studentList[i].pacA_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName + " " + promise.studentList[i].pacA_MiddleName + " " + promise.studentList[i].pacA_LastName, pacA_Id: promise.studentList[i].pacA_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName + " " + promise.studentList[i].pacA_MiddleName, pacA_Id: promise.studentList[i].pacA_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentList[i].pacA_FirstName, pacA_Id: promise.studentList[i].pacA_Id });
                                }

                                promise.studentList[i].pacA_FirstName = $scope.albumNameArray1[i].name;
                            }
                        }
                    }


                    $scope.PACA_FirstName = promise.studentList[0].pacA_FirstName;
                    $scope.PACA_MiddleName = promise.studentList[0].pacA_MiddleName;
                    $scope.PACA_LastName = promise.studentList[0].pacA_LastName;
                    $scope.PACA_Date = new Date(promise.studentList[0].pacA_Date);
                    $scope.PACA_RegistrationNo = promise.studentList[0].pacA_RegistrationNo;
                    $scope.PACA_AdmNo = promise.studentList[0].pacA_AdmNo;
                    $scope.PACA_StudentSubCaste = promise.studentList[0].pacA_StudentSubCaste;
                    $scope.ASMAY_Id = promise.studentList[0].asmaY_Id;
                    $scope.AMCO_Id = promise.studentList[0].amcO_Id;
                    $scope.AMB_Id = promise.studentList[0].amB_Id;
                    $scope.AMSE_Id = promise.studentList[0].amsE_Id;
                    $scope.ACMB_Id = promise.studentList[0].acmB_Id;
                    $scope.ACSS_Id = promise.studentList[0].acsS_Id;
                    $scope.ACST_Id = promise.studentList[0].acsT_Id;
                    if (promise.studentCategory.length > 0) {
                        $scope.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                    }
                    $scope.PACA_Sex = promise.studentList[0].pacA_Sex;
                    $scope.PACA_DOB = new Date(promise.studentList[0].pacA_DOB);
                    var doob = promise.studentList[0].pacA_DOB;

                    var doobyr = doob.substring(0, 4);
                    var doobmnth = doob.substring(5, 7);
                    var doobdays = doob.substring(8, 10);

                    $scope.b1 = doobdays.substring(0, 1);
                    $scope.b2 = doobdays.substring(1, 2);
                    $scope.BB1 = $scope.b1 + $scope.b2;

                    $scope.b3 = doobmnth.substring(0, 1);
                    $scope.b4 = doobmnth.substring(1, 2);
                    $scope.BB2 = $scope.b3 + $scope.b4;

                    $scope.b5 = doobyr.substring(0, 1);
                    $scope.b6 = doobyr.substring(1, 2);
                    $scope.b7 = doobyr.substring(2, 3);
                    $scope.b8 = doobyr.substring(3, 4);
                    $scope.BB3 = $scope.b5 + $scope.b6 + $scope.b7 + $scope.b8;

                    $scope.PACA_DOB_inwords = promise.studentList[0].pacA_DOB_inwords;
                    $scope.PACA_Age = promise.studentList[0].pacA_Age;
                    $scope.PACA_BloodGroup = promise.studentList[0].pacA_BloodGroup;
                    $scope.PACA_MotherTongue = promise.studentList[0].pacA_MotherTongue;
                    $scope.PACA_BirthPlace = promise.studentList[0].pacA_BirthPlace;
                    $scope.PACA_BirthCertNo = promise.studentList[0].pacA_BirthCertNo;
                    $scope.IVRMMR_Id = promise.studentList[0].ivrmmR_Id;
                    $scope.PACA_StudentSubCaste = promise.studentList[0].pacA_StudentSubCaste;
                    $scope.PACA_TPINNO = promise.studentList[0].pacA_TPINNO;

                    $scope.PACA_Village = promise.studentList[0].pacA_Village;
                    $scope.PACA_Taluk = promise.studentList[0].pacA_Taluk;
                    $scope.PACA_District = promise.studentList[0].pacA_District;
                    $scope.PACA_Urban_Rural = promise.studentList[0].pacA_Urban_Rural;

                    //$scope.IMCC_Id = promise.studentList[0].imcC_Id;
                    //for (var i = 0; i < $scope.allCaste.length; i++) {
                    //    $scope.allCaste[i].Selected = false;
                    //    $scope.IMC_Id = "";
                    //}


                    //if (promise.allCaste.length > 0) {
                    //    for (var i = 0; i < promise.allCaste.length; i++) {
                    //        if (promise.studentList[0].imC_Id == promise.allCaste[i].imC_Id) {
                    //            $scope.allCaste[i].Selected = true;
                    //            $scope.IMC_Id = promise.studentList[0].imC_Id;
                    //        }
                    //    }
                    //}
                    //else {
                    //    swal("Something has gone wrong.Please check Master Caste Category and Master Caste");
                    //}




                    $scope.PACA_Nationality = promise.studentList[0].pacA_Nationality;

                    $scope.IVRMMC_Id = promise.studentList[0].ivrmmC_Id;



                   // getSelectGetState($scope.IVRMMC_Id, promise.studentList[0].pacA_PerState);

                    $scope.PACA_PerState = promise.studentList[0].pacA_PerState;


                    $scope.PACA_PerStreet = promise.studentList[0].pacA_PerStreet;
                    $scope.PACA_PerArea = promise.studentList[0].pacA_PerArea;
                    $scope.PACA_PerCity = promise.studentList[0].pacA_PerCity;

                    $scope.PACA_PerPincode = promise.studentList[0].pacA_PerPincode;


                    $scope.PACA_StuBankAccNo = promise.studentList[0].pacA_StuBankAccNo;
                    $scope.PACA_StuBankIFSCCode = promise.studentList[0].pacA_StuBankIFSCCode;
                    $scope.PACA_AadharNo = promise.studentList[0].pacA_AadharNo;
                    $scope.PACA_BirthPlace = promise.studentList[0].pacA_BirthPlace;
                    $scope.PACA_StuCasteCertiNo = promise.studentList[0].pacA_StuCasteCertiNo;
                    $scope.PACA_MobileNo = promise.studentList[0].pacA_MobileNo;
                    $scope.PACA_emailId = promise.studentList[0].pacA_emailId;

                    $scope.PACA_PerStreet = promise.studentList[0].pacA_PerStreet;
                    $scope.PACA_ConPincode = promise.studentList[0].pacA_ConPincode;
                    $scope.PACA_ConArea = promise.studentList[0].pacA_ConArea;
                    $scope.PACA_ConStreet = promise.studentList[0].pacA_ConStreet;
                    $scope.PACA_ConCity = promise.studentList[0].pacA_ConCity;
                    $scope.PACA_ConCountryId = promise.studentList[0].pacA_ConCountryId;


                    $scope.studcourse = promise.studentcourse.length > 0 ? promise.studentcourse[0].amcO_CourseName : "";
                    $scope.studReligion = promise.studentReligion.length > 0 ? promise.studentReligion[0].ivrmmR_Name : "";
                    $scope.CasteName = promise.studentcastecate.length > 0 ? promise.studentcastecate[0].imC_CasteName : "";
                    $scope.studperstate = promise.studentperstate.length > 0 ? promise.studentperstate[0].studperstate : "";
                    $scope.studconstate = promise.studentconstate.length > 0 ? promise.studentconstate[0].studconstate : "";
                    $scope.studconcountry = promise.studentconcountry.length > 0 ? promise.studentconcountry[0].studconcountry : "";
                    $scope.studpercountry = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].studpercountry : "";

                    $scope.countrycode = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].countrycode : "";
                    $scope.statecode = promise.studentperstate.length > 0 ? promise.studentperstate[0].statecode : "";
                    $scope.CategoryName = promise.casteCategoryName.length > 0 ? promise.casteCategoryName[0].categoryName : "";


                    // getSelectGetState2($scope.PACA_ConCountryId, promise.studentList[0].pacA_ConState);

                    $scope.PACA_ConState = promise.studentList[0].pacA_ConState;

                    $scope.PACA_FatherAliveFlag = promise.studentList[0].pacA_FatherAliveFlag;
                    if (promise.studentList[0].pacA_FatherSurname != null) {
                        $scope.PACA_FatherName = promise.studentList[0].pacA_FatherName + ' ' + promise.studentList[0].pacA_FatherSurname;
                    }
                    else {
                        $scope.PACA_FatherName = promise.studentList[0].pacA_FatherName;
                    }

                    $scope.PACA_FatherSurname = promise.studentList[0].pacA_FatherSurname;
                    $scope.PACA_FatherAadharNo = promise.studentList[0].pacA_FatherAadharNo;
                    $scope.PACA_FatherEducation = promise.studentList[0].pacA_FatherEducation;
                    $scope.PACA_FatherOfficeAdd = promise.studentList[0].pacA_FatherOfficeAdd;
                    $scope.PACA_FatherOccupation = promise.studentList[0].pacA_FatherOccupation;
                    $scope.PACA_FatherDesignation = promise.studentList[0].pacA_FatherDesignation;
                    $scope.PACA_FatherBankAccNo = promise.studentList[0].pacA_FatherBankAccNo;
                    $scope.PACA_FatherBankIFSCCode = promise.studentList[0].pacA_FatherBankIFSCCode;
                    $scope.PACA_FatherCasteCertiNo = promise.studentList[0].pacA_FatherCasteCertiNo;
                    $scope.PACA_FatherNationality = promise.studentList[0].pacA_FatherNationality;
                    $scope.PACA_FatherMonIncome = promise.studentList[0].pacA_FatherMonIncome;
                    $scope.PACA_FatherAnnIncome = promise.studentList[0].pacA_FatherAnnIncome;
                    $scope.PACA_FatherMobleNo = promise.studentList[0].pacA_FatherMobleNo;
                    $scope.PACA_FatherEmailId = promise.studentList[0].pacA_FatherEmailId;
                    $scope.PACA_FatherReligion = promise.studentList[0].pacA_FatherReligion;
                    $scope.PACA_FatherCaste = promise.studentList[0].pacA_FatherCaste;
                    $scope.PACA_FatherSubCaste = promise.studentList[0].pacA_FatherSubCaste;

                    $scope.PACA_MotherAliveFlag = promise.studentList[0].pacA_MotherAliveFlag;
                    if (promise.studentList[0].pacA_MotherSurname != null) {
                        $scope.PACA_MotherName = promise.studentList[0].pacA_MotherName + ' ' + promise.studentList[0].pacA_MotherSurname;
                    }
                    else {
                        $scope.PACA_MotherName = promise.studentList[0].pacA_MotherName;
                    }

                    $scope.PACA_MotherSurname = promise.studentList[0].pacA_MotherSurname;
                    $scope.PACA_MotherAadharNo = promise.studentList[0].pacA_MotherAadharNo;
                    $scope.PACA_MotherEducation = promise.studentList[0].pacA_MotherEducation;
                    $scope.PACA_MotherOfficeAdd = promise.studentList[0].pacA_MotherOfficeAdd;
                    $scope.PACA_MotherOccupation = promise.studentList[0].pacA_MotherOccupation;
                    $scope.PACA_MotherDesignation = promise.studentList[0].pacA_MotherDesignation;
                    $scope.PACA_MotherBankAccNo = promise.studentList[0].pacA_MotherBankAccNo;
                    $scope.PACA_MotherBankIFSCCode = promise.studentList[0].pacA_MotherBankIFSCCode;
                    $scope.PACA_MotherCasteCertiNo = promise.studentList[0].pacA_MotherCasteCertiNo;
                    $scope.PACA_MotherNationality = promise.studentList[0].pacA_MotherNationality;
                    $scope.PACA_MotherMonIncome = promise.studentList[0].pacA_MotherMonIncome;
                    $scope.PACA_MotherAnnIncome = promise.studentList[0].pacA_MotherAnnIncome;
                    $scope.PACA_MotherMobleNo = promise.studentList[0].pacA_MotherMobleNo;
                    $scope.PACA_MotherEmailId = promise.studentList[0].pacA_MotherEmailId;

                    $scope.PACA_MotherReligion = promise.studentList[0].pacA_MotherReligion;
                    $scope.PACA_MotherCaste = promise.studentList[0].pacA_MotherCaste;
                    $scope.PACA_MotherSubCaste = promise.studentList[0].pacA_MotherSubCaste;

                    $scope.PACA_PassportNo = promise.studentList[0].pacA_PassportNo;

                    $scope.PACA_PassportIssuedAt = promise.studentList[0].pacA_PassportIssuedAt;
                    if (promise.studentList[0].pacA_PassportIssueDate != null) {


                        $scope.PACA_PassportIssueDate = new Date(promise.studentList[0].pacA_PassportIssueDate);

                        var do0ob = promise.studentList[0].pacA_PassportIssueDate;

                        var doobyrv = do0ob.substring(0, 4);
                        var doobmnthv = do0ob.substring(5, 7);
                        var doobdaysv = do0ob.substring(8, 10);

                        $scope.b1v = doobdaysv.substring(0, 1);
                        $scope.b2v = doobdaysv.substring(1, 2);
                        $scope.BV1 = $scope.b1v + $scope.b2v;

                        $scope.b3v = doobmnthv.substring(0, 1);
                        $scope.b4v = doobmnthv.substring(1, 2);
                        $scope.BV2 = $scope.b3v + $scope.b4v;

                        $scope.b5v = doobyrv.substring(0, 1);
                        $scope.b6v = doobyrv.substring(1, 2);
                        $scope.b7v = doobyrv.substring(2, 3);
                        $scope.b8v = doobyrv.substring(3, 4);
                        $scope.BV3 = $scope.b5v + $scope.b6v + $scope.b7v + $scope.b8v;
                    }
                    $scope.PACA_PassportIssuedCounrty = promise.studentList[0].pacA_PassportIssuedCounrty;
                    $scope.PACA_PassportIssuedPlace = promise.studentList[0].pacA_PassportIssuedPlace;
                    $scope.PACA_PassportExpiryDate = promise.studentList[0].pacA_PassportExpiryDate;

                    $scope.PACA_VISAIssuedBy = promise.studentList[0].pacA_VISAIssuedBy;
                    $scope.PACA_VISAValidFrom = promise.studentList[0].pacA_VISAValidFrom;
                    $scope.PACA_VISAValidTo = promise.studentList[0].pacA_VISAValidTo;




                    if (promise.studentpreffredbranch != null && promise.studentpreffredbranch.length > 0) {
                        $scope.studentpreffredbranch = promise.studentpreffredbranch;
                    }

                    if (promise.studentcurrenrtbranch != null && promise.studentcurrenrtbranch.length > 0) {
                        $scope.studentcurrenrtbranch = promise.studentcurrenrtbranch[0].studentbranchname;          
                    }

                    //comp marks and exam details
                    $scope.compexamstudetails = [];
                    $scope.compexammarksdetails = [];
                    $scope.compexammarksdetails = promise.studentCompSubDetails;
                    $scope.compexamstudetails = promise.studentCompDetails;

                    if ($scope.compexamstudetails.length > 0) {
                        $scope.editflg = true;
                        for (var i = 0; i < $scope.compexamstudetails.length; i++) {

                            $scope.compexamstudetails[i].pamcexM_Id = $scope.compexamstudetails[0].pamcexM_Id;
                            $scope.compexamstudetails[i].pacstceM_RollNo = $scope.compexamstudetails[0].pacstceM_RollNo;
                            $scope.compexamstudetails[i].pacstceM_MeritNo = $scope.compexamstudetails[0].pacstceM_MeritNo;
                            $scope.compexamstudetails[i].PACSTCEM_RegistrationId = $scope.compexamstudetails[0].pacstceM_RegistrationId;
                            $scope.compexamstudetails[i].PACSTCEM_TotalMaxMarks = $scope.compexamstudetails[0].pacstceM_TotalMaxMarks;
                            $scope.compexamstudetails[i].PACSTCEM_ObtdMarks = $scope.compexamstudetails[0].pacstceM_ObtdMarks;
                            $scope.compexamstudetails[i].PACSTCEM_ALLIndiaRank = $scope.compexamstudetails[0].pacstceM_ALLIndiaRank;
                            $scope.compexamstudetails[i].PACSTCEM_CategoryRank = $scope.compexamstudetails[0].pacstceM_CategoryRank;
                            $scope.compexamstudetails[i].PACSTCEM_Percentage = $scope.compexamstudetails[0].pacstceM_Percentage;
                            $scope.compexamstudetails[i].PACSTCEM_Percentile = $scope.compexamstudetails[0].pacstceM_Percentile;
                            $scope.compexamstudetails[i].PAMCEXM_CompetitiveExams = $scope.compexamstudetails[0].pamcexM_CompetitiveExams;

                        }


                    }
                    //else {

                    //    $scope.compexamstudetails = {};

                    //    $scope.compexamstudetails = [{ id: 'compExamStudetails1' }];
                    //    $scope.compexamstudetailscount = 0;

                    //    $scope.editflg = false;
                    //}

                    if ($scope.compexammarksdetails.length > 0) {
                        $scope.editflg = true;
                        for (var i = 0; i < $scope.compexammarksdetails.length; i++) {
                            $scope.compexammarksdetails[i].pamcexmsuB_Id = $scope.compexammarksdetails[0].pamcexmsuB_Id;
                            $scope.compexammarksdetails[i].pamcexmsuB_MaxMarks = $scope.compexammarksdetails[0].pamcexmsuB_MaxMarks;
                            $scope.compexammarksdetails[i].pacstcemS_SubjectMarks = $scope.compexammarksdetails[0].pacstcemS_SubjectMarks;
                            $scope.compexammarksdetails[i].PAMCEXM_CompetitiveExams = $scope.compexammarksdetails[0].pamcexM_CompetitiveExams;
                            $scope.compexammarksdetails[i].PAMCEXMSUB_SubjectName = $scope.compexammarksdetails[0].pamcexmsuB_SubjectName;

                        }
                    }
                    //else {

                    //    $scope.compexammarksdetails = {};
                    //    $scope.compexammarksdetails = [{ id: 'compExamdetails1' }];
                    //    $scope.compexammarksdetailscount = 0;

                    //}






                        //



                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.htmldata))(($scope));
                }
            });
        }


    

        $scope.sortKey = "name";   //set the sortKey to the param passed
        $scope.reverse = false; //if true make it false and vice versa
        $scope.order = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        var studentreg = "";
        $scope.showmodaldetails = function (data) {

            if ($scope.pages.length > 0) {
                for (var i = 0; i < $scope.pages.length; i++) {
                    if ($scope.pages[i].pacA_id == data.pacA_id) {
                        studentreg = $scope.pages[i].pacA_RegistrationNo;
                    }
                }
            }

            $scope.imagedownload = data.document_Path;
            imagedownload = data.document_Path;
            docname = data.amsmD_DocumentName;
            $('#preview').attr('src', data.document_Path);
        }

        $scope.showmodaldetailsimage = function (data, idd, typeofphoto) {

            if ($scope.pages.length > 0) {
                for (var i = 0; i < $scope.pages.length; i++) {
                    if ($scope.pages[i].pacA_id == idd) {
                        studentreg = $scope.pages[i].pacA_RegistrationNo;
                    }
                }
            }

            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;
            $('#preview').attr('src', data);
        }


        //added
        $scope.al_checkcourse = function (all, ASMCL_Id) {

            $scope.courselistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.courselists, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.courselists, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ AMCO_Id: qq.amcO_Id, AMCO_Id: qq.amcO_Id })
                }
            });


            if ($scope.obj.usercheckCC == true) {
                $scope.getbranch();
              
            }
            else {
                $scope.branchlist = [];

            }

        }
        $scope.all_checkC = function (all, ASMCL_Id) {
            $scope.semesterlistarray = [];
            $scope.obj.usercheckC = all;
            var toggleStatus = $scope.obj.usercheckC;
            angular.forEach($scope.branchlist, function (role) {
                role.selected = toggleStatus;
            });

         
            angular.forEach($scope.branchlist, function (qq) {
                if (qq.selected == true) {
                    $scope.semesterlistarray.push({ AMB_Id: qq.amB_Id })
                }
            });
            $scope.getsemester();
          

        };

        $scope.all_checkS = function (all) {
            $scope.usercheckS = all;
            var toggleStatus = $scope.usercheckS;
            angular.forEach($scope.semesterlist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.semesterlistarray = [];
            angular.forEach($scope.semesterlist, function (qq) {
                if (qq.selected == true) {
                    $scope.semesterlistarray.push({ AMSE_Id: qq.amsE_Id })
                }
            })



        };

        $scope.togchkbxS = function () {
            $scope.semesterlistarray = [];
            angular.forEach($scope.semesterlist, function (qq) {
                if (qq.selected == true) {
                    $scope.semesterlistarray.push({ AMSE_Id: qq.amsE_Id })
                }
            })
        }

        // get branch
        $scope.getbranch = function (AMCO_Id) {

        
            $scope.courselistarray = [];

            angular.forEach($scope.courselists, function (aa) {
                if (aa.selected == true) {
                    $scope.courselistarray.push({ AMCO_Id: aa.amcO_Id })
                }

            });


            var data = {
                "courselsttwo": $scope.courselistarray
            }
            apiService.create("DocumentViewClg/getbranch", data).then(function (promise) {
                if (promise.branchlist.length > 0 || promise.branchlist != null) {
                    $scope.branchlist = promise.branchlist;
                    
                }
                else {
                    swal('No data Found!!!');
                }
            });
            //
        }

        $scope.getsemester = function () {
            $scope.semesterlist = [];
           

            //added by roopa//
            $scope.courselistarray = [];
            $scope.branchlistarray = [];

            if ($scope.courselists.length > 0 || $scope.courselists != null) {
                angular.forEach($scope.courselists, function (qq) {
                    if (qq.selected == true) {
                        $scope.courselistarray.push({ AMCO_Id: qq.amcO_Id })

                    }
                });
            }

            if ($scope.branchlist != null && $scope.branchlist.length > 0) {
               
                angular.forEach($scope.branchlist, function (qq) {
                    if (qq.selected == true) {
                       
                        $scope.branchlistarray.push({ AMB_Id: qq.amB_Id })
                    }
                });
            }
            var data = {
            
                "courselsttwo": $scope.courselistarray,
                "branchlisttwo": $scope.branchlistarray
            }
            apiService.create("DocumentViewClg/getsemester", data).then(function (promise) {
                $scope.semesterlist1 = [];
                $scope.semesterlist = [];
                $scope.semesterlist1 = promise.semesterlist;
                if ($scope.semesterlist1.length > 0 || $scope.semesterlist1 != null) {
                    $scope.semesterlist = $scope.semesterlist1;
                }
                else {
                    swal('No Data Found!!!')
                }
            })
            // }
        }

        //


        $scope.isOptionsRequiredcourse = function () {
            return !$scope.courselists.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequiredbranch = function () {
            return !$scope.branchlist.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequiredsemester = function () {
            return !$scope.semesterlist.some(function (item) {
                return item.selected;
            });
        };
        //


        $scope.download = function () {
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
                })
        }

        $scope.downloaddirect = function (data, idd) {
            if ($scope.pages.length > 0) {
                for (var i = 0; i < $scope.pages.length; i++) {
                    if ($scope.pages[i].pasr_id == idd) {
                        studentreg = $scope.pages[i].pasR_RegistrationNo;
                    }
                }
            }

            $scope.imagedownload = data.document_Path;
            imagedownload = data.document_Path;
            docname = data.amsmD_DocumentName;

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
                })
        }

        $scope.downloaddirectimage = function (data, idd, typeofphoto) {
            if ($scope.pages.length > 0) {
                for (var i = 0; i < $scope.pages.length; i++) {
                    if ($scope.pages[i].pasr_id == idd) {
                        studentreg = $scope.pages[i].pasR_RegistrationNo;
                    }
                }
            }

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
                })
        }

        $scope.downloadpdf = function (data) {

            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = data.document_Pathtemp;

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


        $scope.TERESIANAPP = function () {

            var innerContents = document.getElementById("TeresianID1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Teresian/PreAdmission/Teresian_Admission_Pdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }




    }
})();