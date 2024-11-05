(function () {
    'use strict';
    angular
        .module('app')
        .controller('DocumentviewReportController', documentviewReportController)

    documentviewReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile', '$sce']
    function documentviewReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile, $sce) {

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
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.showpdf = false;

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };
        $scope.showrepatsib = true;
        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        var imagedownload = "";
        var docname = "";
        var ftype = "";
        $scope.uppercase = function (str) {
            var array1 = str.split(' ');
            var newarray1 = [];

            for (var x = 0; x < array1.length; x++) {
                newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
            }
            $scope.camelcase = newarray1;
            return $scope.camelcase = newarray1.join(' ');
        };
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            }
        }

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.currentPage8 = 1;
        $scope.itemsPerPage8 = paginationformasters;

        $scope.currentPage12 = 1;
        $scope.itemsPerPage12 = paginationformasters;

        $scope.currentPage9 = 1;
        $scope.itemsPerPage9 = paginationformasters;

        $scope.submitted = false;
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {


            apiService.get("DocumentviewReport/getdetails/2").then(function (promise) {

                $scope.year_list = promise.fillyear
                $scope.classlists = promise.admissioncatdrp;
            })

        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //TO clear  data
        $scope.clearid = function () {

            $scope.asmaY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.reportgrid = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');

        };


        $scope.doclist = [];

        $scope.regformfour = function () {
            $scope.submitted = true;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;

            //angular.forEach($scope.documentList, function (value, key) {

            //    $('#' + value.amsmD_Id).removeAttr('src');
            //    $scope.documentList[key].document_Path = '';
            //})

            if ($scope.myForm.$valid) {
                var data = {
                    "configurationsettings": configsettings[0].ispaC_ApplFeeFlag,
                    "asmay_id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id
                }
                apiService.create("DocumentviewReport/getdpforreg", data).then(function (promise) {

                    // for pagination 
                    if (promise.registrationList == null || promise.registrationList.length == 0) {
                        $scope.reportgrid = false;
                        swal("No Records Are Found For Your Selection !!!")
                    }
                    else {
                        $scope.pages = promise.registrationList;
                        $scope.presentCountgrid = promise.registrationList.length;
                        $scope.prospectusPaymentlist = promise.prospectusPaymentlist;
                        $scope.doclist = promise.ddoc;
                        // $scope.classlists = promise.admissioncatdrp;

                        if (promise.admissioncatdrpall.length > 0) {
                            for (var i = 0; i < $scope.pages.length; i++) {
                                for (var j = 0; j < promise.admissioncatdrpall.length; j++) {
                                    if ($scope.pages[i].asmcL_Id == promise.admissioncatdrpall[j].asmcL_Id) {
                                        $scope.pages[i].classname = promise.admissioncatdrpall[j].asmcL_ClassName;
                                    }
                                }
                            }
                        }


                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < $scope.pages.length; i++) {
                            if ($scope.pages[i].pasR_FirstName != '') {
                                if ($scope.pages[i].pasR_MiddleName !== null) {
                                    if ($scope.pages[i].pasR_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: $scope.pages[i].pasR_FirstName + " " + $scope.pages[i].pasR_MiddleName + " " + $scope.pages[i].pasR_LastName, pasR_Id: $scope.pages[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.pages[i].pasR_FirstName + '' + $scope.pages[i].pasR_MiddleName, pasR_Id: $scope.pages[i].pasR_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: $scope.pages[i].pasR_FirstName, pasR_Id: $scope.pages[i].pasR_Id });
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
                            //$scope.document = {};
                            //$scope.documentList = promise.studentDocuments_DTObj;
                            //angular.forEach($scope.doclist, function (obj) {
                            angular.forEach($scope.doclist, function (obj) {
                                //$('#' + value.amsmD_Id).removeAttr('src');
                                //$scope.documentList[key].document_Path = '';
                                //if (obj.amsmD_Id === obj1.amsmD_Id) {
                                $('#' + obj.amsmD_Id).attr('src', obj.document_Path);
                                //  obj.document_Path = obj.document_Path;
                                //$scope.documentList[key].pasrD_Id = value1.pasrD_Id;
                                //$scope.documentList[key].pasr_id = value1.pasr_id;
                                var img = obj.document_Path;
                                obj.document_Pathtemp = obj.document_Path;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];

                                obj.filetype = lastelement;


                                if (obj.filetype === 'xlsx' || obj.filetype === 'xls' || obj.filetype === 'xlsm' || obj.filetype === 'docx' || obj.filetype === 'doc') {
                                    obj.document_Path = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.document_Path;
                                }
                                //}
                            });
                            //});
                            console.log($scope.doclist);
                        }
                        $scope.currentPage = 1;
                        $scope.itemsPerPage = 10;
                        $scope.reportgrid = true;
                    }
                });
            }
        };

        $scope.showprinthelthdata = function (pashD_Id) {
            apiService.getURI("StudentApplication/printgethelthData", pashD_Id).
                then(function (promise) {
                    //added by suryan
                    $scope.albumNameArray1 = [];
                    $scope.albumNameArray2 = [];
                    for (var i = 0; i < promise.studenthelthDTO.length; i++) {
                        if (promise.studenthelthDTO[i].pasR_FirstName != '') {
                            if (promise.studenthelthDTO[i].pasR_MiddleName !== null) {
                                if (promise.studenthelthDTO[i].pasR_LastName !== null) {

                                    $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + promise.studenthelthDTO[i].pasR_MiddleName + promise.studenthelthDTO[i].pasR_LastName });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + promise.studenthelthDTO[i].pasR_MiddleName });
                                }
                            }
                            else {
                                $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName });
                            }
                        }
                    }

                    $scope.Hstuname = $scope.albumNameArray1[0].name;
                    $scope.Hstuage = promise.studenthelthDTO[0].pasR_Age;
                    $scope.HFirstName = promise.studenthelthDTO[0].pasR_FatherName;
                    $scope.VaccinationDate = promise.studenthelthDTO[0].pashD_VaccinationDate;
                    if (promise.studenthelthDTO[0].pashD_FitsFlag == 1) {
                        $scope.FitsFlag = "Yes";
                        $scope.FitsDate = promise.studenthelthDTO[0].pashD_FitsDate;
                    }
                    else {
                        $scope.FitsFlag = "No";
                        $scope.FitsDate = " ";
                    }
                    $scope.Illness = promise.studenthelthDTO[0].pashD_Illness;
                    $scope.HepatitisB = new Date(promise.studenthelthDTO[0].pashD_HepatitisB);
                    $scope.TyphoidFever = new Date(promise.studenthelthDTO[0].pashD_TyphoidFever);
                    $scope.Ailment = promise.studenthelthDTO[0].pashD_Ailment;
                    if (promise.studenthelthDTO[0].pashD_Allergy == 1) {
                        $scope.Allergy = "Yes";
                    }
                    else {
                        $scope.Allergy = "No";
                    }

                    $scope.HealthProblem = promise.studenthelthDTO[0].pashD_HealthProblem;
                    $scope.BloodGroup = promise.studenthelthDTO[0].pashD_BloodGroup;


                });

        }
        $scope.printstuhelth = function () {

            var innerContents = document.getElementById("BGHSHealthCertReport").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/AplicationForms/BGHSHealthCertReportPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.search = '';
        $scope.filterValue = function (obj) {
            //
            return angular.lowercase(obj.name).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(obj.pasR_RegistrationNo).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(obj.classname).indexOf(angular.lowercase($scope.search)) >= 0;
        }


        $scope.cleartheapplication = function () {

            $scope.serilano = "";
            $scope.pasr_id = "";
            $scope.PASR_FirstName = "";
            $scope.PASR_MiddleName = "";
            $scope.PASR_LastName = "";
            $scope.namelength = 0;
            $scope.PASR_Date = "";
            $scope.PASR_RegistrationNo = "";
            $scope.PASR_Sex = "";
            $scope.PASA_TransferrableJobFlg = "";
            $scope.PASR_DOB = "";



            $scope.PASR_Age = "";
            $scope.PASR_DOBWords = "";
            $scope.asmcL_ClassName = "";
            $scope.PASR_BloodGroup = "";
            $scope.PASR_Emisno = "";
            $scope.medium = "";
            $scope.PASR_Boarding = "";
            $scope.PASR_MotherTongue = "";
            $scope.religionname = "";
            $scope.concessioncat = "";

            $scope.IMCC_CategoryName = "";
            $scope.IMC_CasteName = "";

            $scope.PASR_PerStreet = "";
            $scope.PASR_PerArea = "";
            $scope.PASR_PerCity = "";
            $scope.PASR_PerStaten = "";
            $scope.PASR_PerCountryn = "";
            $scope.PASR_PerPincode = "";


            $scope.PASR_ConStreet = "";
            $scope.PASR_ConArea = "";
            $scope.PASR_ConCity = "";
            $scope.PASR_ConStaten = "";
            $scope.PASR_ConCountryn = "";
            $scope.PASR_ConPincode = "";

            $scope.PASR_AadharNo = "";

            $scope.PASR_Distirct = "";
            $scope.PASR_Taluk = "";
            $scope.PASR_Village = "";
            $scope.PASR_Stayingwith = "";

            $scope.PASR_Languagespeaking = "";
            $scope.PASR_FatherPanno = "";
            $scope.PASR_MotherPanno = "";

            $scope.PASR_MobileNo = "";
            $scope.PASR_emailId = "";
            $scope.PASR_MaritalStatus = "";

            $scope.PASR_FatherAliveFlag = "";
            $scope.PASR_FatherName = "";
            $scope.PASR_FatherAadharNo = "";
            $scope.PASR_FatherSurname = "";
            $scope.PASR_FatherEducation = "";
            $scope.PASR_FatherOccupation = "";
            $scope.PASR_FatherDesignation = "";
            $scope.PASR_FatherIncome = "";
            $scope.PASR_FatherMobleNo = "";
            $scope.PASR_FatheremailId = "";

            $scope.syllabussname = "";
            $scope.PASR_FatherReligion = "";
            $scope.PASR_FatherCaste = "";
            $scope.fatherSubCaste_Id = "";
            $scope.SubCaste_Id = "";
            $scope.motherSubCaste_Id = "";

            $scope.PASR_FatherTribe = "";
            $scope.PASR_Tribe = "";
            $scope.PASR_MotherReligion = "";
            $scope.PASR_MotherCaste = "";
            $scope.PASR_MotherTribe = "";

            $scope.PASR_FirstLanguage = "";
            $scope.PASR_Thirdlanguage = "";
            $scope.PASR_SecondLanguage = "";
            $scope.PASR_MotherAliveFlag = "";
            $scope.PASR_MotherName = "";
            $scope.PASR_MotherAadharNo = "";
            $scope.PASR_MotherSurname = "";
            $scope.PASR_MotherEducation = "";
            $scope.PASR_MotherOccupation = "";
            $scope.PASR_MotherDesignation = "";
            $scope.PASR_MotherIncome = "";
            $scope.PASR_MotherMobleNo = "";
            $scope.PASR_MotheremailId = "";


            $scope.PASR_ChurchAddress = "";
            $scope.PASR_Churchname = "";

            $scope.PASR_FatherOfficePhNo = "";
            $scope.PASR_FatherHomePhNo = "";
            $scope.PASR_MotherOfficePhNo = "";
            $scope.PASR_MotherHomePhNo = "";

            $scope.PASR_FatherPassingYear = "";
            $scope.PASR_MotherPassingYear = "";

            $scope.PASR_TotalIncome = "";
            $scope.PASR_BirthPlace = "";
            $scope.studentnationality = "";

            $scope.PASR_HostelReqdFlag = "";
            $scope.PASR_TransportReqdFlag = "";
            $scope.PASR_GymReqdFlag = "";
            $scope.PASR_ECSFlag = "";
            $scope.PASR_PaymentFlag = "";

            $scope.PASR_AmountPaid = "";
            $scope.PASR_PaymentType = "";
            $scope.PASR_PaymentDate = "";
            $scope.PASR_ReceiptNo = "";
            //Activeflag
            //Applstatus
            $scope.PASR_FinalpaymentFlag = "";
            $scope.PASR_LastPlayGrndAttnd = "";
            $scope.PASR_ExtraActivity = "";
            $scope.PASR_OtherResidential_Addr = "";
            $scope.PASR_OtherPermanentAddr = "";
            $scope.PASR_MotherOfficeAddr = "";
            $scope.PASR_UndertakingFlag = "";
            $scope.fathernationality = "";
            $scope.mothernationality = "";
            $scope.PASR_BirthCertificateNo = "";
            $scope.PASR_AltContactNo = "";
            $scope.PASR_AltContactEmail = "";

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

            $scope.PASR_Noofbrothers = "";
            $scope.PASR_Noofsisters = "";
            $scope.PASR_lastclassperc = "";
            $scope.PASR_SibblingConcessionFlag = "";
            $scope.PASR_ParentConcessionFlag = "";



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

        $scope.showprintdata = function (pasR_Id) {
            $scope.cleartheapplication();

            apiService.getURI("StudentApplication/getprintdata", pasR_Id).then(function (promise) {

                $scope.serilano = promise.studentReg_DTObj[0].pasR_Id;
                $scope.pasr_id = promise.studentReg_DTObj[0].pasR_RegistrationNo;
                $scope.applicationno_view = promise.studentReg_DTObj[0].pasR_Applicationno;

                $scope.PASR_FirstName = promise.studentReg_DTObj[0].pasR_FirstName == null ? "" : promise.studentReg_DTObj[0].pasR_FirstName;
                $scope.PASR_MiddleName = promise.studentReg_DTObj[0].pasR_MiddleName == null ? "" : promise.studentReg_DTObj[0].pasR_MiddleName;
                $scope.PASR_LastName = promise.studentReg_DTObj[0].pasR_LastName == null ? "" : promise.studentReg_DTObj[0].pasR_LastName;
                $scope.namelength = Number($scope.PASR_FirstName.length) + Number($scope.PASR_MiddleName.length) + Number($scope.PASR_LastName.length);
                $scope.PASR_Date = new Date(promise.studentReg_DTObj[0].pasR_Date);
                $scope.PASR_RegistrationNo = promise.studentReg_DTObj[0].pasR_RegistrationNo == null ? "" : promise.studentReg_DTObj[0].pasR_RegistrationNo;
                //AMC ID
                $scope.PASR_Sex = promise.studentReg_DTObj[0].pasR_Sex == null ? "" : promise.studentReg_DTObj[0].pasR_Sex;
                $scope.PASA_TransferrableJobFlg = promise.studentReg_DTObj[0].pasA_TransferrableJobFlg == null ? "" : promise.studentReg_DTObj[0].pasA_TransferrableJobFlg;
                $scope.PASR_DOB = new Date(promise.studentReg_DTObj[0].pasR_DOB);
                $scope.PASR_ChurchBaptisedDate = new Date(promise.studentReg_DTObj[0].pasR_ChurchBaptisedDate);
                var doob = promise.studentReg_DTObj[0].pasR_DOB;
                var doobyr = doob.substring(0, 4);
                var doobmnth = doob.substring(5, 7);
                var doobdays = doob.substring(8, 10);


                $scope.doobyr2 = doob.substring(0, 4);
                $scope.doobdays2 = doob.substring(8, 10);
                var monthNames = ["January", "February", "March", "April", "May", "June",
                    "July", "August", "September", "October", "November", "December"];
                var monthNumber = parseInt(doob.substring(5, 7), 10);
                var monthName = monthNames[monthNumber - 1];
                $scope.doobmnth2 = monthName;


                $scope.b1 = doobdays.substring(0, 1);
                $scope.b2 = doobdays.substring(1, 2);


                $scope.b3 = doobmnth.substring(0, 1);
                $scope.b4 = doobmnth.substring(1, 2);

                $scope.b5 = doobyr.substring(0, 1);
                $scope.b6 = doobyr.substring(1, 2);
                $scope.b7 = doobyr.substring(2, 3);
                $scope.b8 = doobyr.substring(3, 4);


                $scope.PASR_Age = promise.studentReg_DTObj[0].pasR_Age;
                $scope.PASR_DOBWords = promise.studentReg_DTObj[0].pasR_DOBWords;
                $scope.asmcL_ClassName = promise.studentClass.length > 0 ? promise.studentClass[0].asmcL_ClassName : "";
                $scope.PASR_BloodGroup = promise.studentReg_DTObj[0].pasR_BloodGroup == null ? "" : promise.studentReg_DTObj[0].pasR_BloodGroup;
                $scope.PASR_Emisno = promise.studentReg_DTObj[0].pasR_Emisno == null ? "" : promise.studentReg_DTObj[0].pasR_Emisno;;
                $scope.medium = promise.studentReg_DTObj[0].pasR_Medium == null ? "" : promise.studentReg_DTObj[0].pasR_Medium;;
                $scope.PASR_Boarding = promise.studentReg_DTObj[0].pasR_Boarding == null ? "" : promise.studentReg_DTObj[0].pasR_Boarding;;
                $scope.PASR_MotherTongue = promise.studentReg_DTObj[0].pasR_MotherTongue == null ? "" : promise.studentReg_DTObj[0].pasR_MotherTongue;;
                $scope.religionname = promise.studentReligion.length > 0 ? promise.studentReligion[0].ivrmmR_Name : "";//
                if (promise.concessioncategory.length > 0) {
                    $scope.concessioncat = promise.concessioncategory.length > 0 ? promise.concessioncategory[0].concessioncats : "";
                }
                $scope.studentarea = promise.studentarea;
                $scope.studentroue = promise.studentroue;
                $scope.studentlocation = promise.studentlocation;
                $scope.studentsource = promise.studentsource;

                $scope.IMCC_CategoryName = promise.studentcastecategory.length > 0 ? promise.studentcastecategory[0].imcC_CategoryName : "";
                $scope.IMC_CasteName = promise.studentcaste.length > 0 ? promise.studentcaste[0].imC_CasteName : "";

                $scope.PASR_PerStreet = promise.studentReg_DTObj[0].pasR_PerStreet == null ? "" : promise.studentReg_DTObj[0].pasR_PerStreet;
                $scope.PASR_PerArea = promise.studentReg_DTObj[0].pasR_PerArea == null ? "" : promise.studentReg_DTObj[0].pasR_PerArea;
                $scope.PASR_PerCity = promise.studentReg_DTObj[0].pasR_PerCity == null ? "" : promise.studentReg_DTObj[0].pasR_PerCity;
                $scope.PASR_PerStaten = promise.studentperstate.length > 0 ? promise.studentperstate[0].pasR_PerStaten : "";
                $scope.PASR_PerCountryn = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].pasR_PerCountryn : "";
                $scope.PASR_PerPincode = promise.studentReg_DTObj[0].pasR_PerPincode != 0 ? promise.studentReg_DTObj[0].pasR_PerPincode : "";


                $scope.PASR_ConStreet = promise.studentReg_DTObj[0].pasR_ConStreet == null ? "" : promise.studentReg_DTObj[0].pasR_ConStreet;
                $scope.PASR_ConArea = promise.studentReg_DTObj[0].pasR_ConArea == null ? "" : promise.studentReg_DTObj[0].pasR_ConArea;
                $scope.PASR_ConCity = promise.studentReg_DTObj[0].pasR_ConCity == null ? "" : promise.studentReg_DTObj[0].pasR_ConCity;
                $scope.PASR_ConStaten = promise.studentconstate.length > 0 ? promise.studentconstate[0].pasR_ConStaten : "";
                $scope.PASR_ConCountryn = promise.studentconcountry.length > 0 ? promise.studentconcountry[0].pasR_ConCountryn : "";
                $scope.PASR_ConPincode = promise.studentReg_DTObj[0].pasR_ConPincode != 0 ? promise.studentReg_DTObj[0].pasR_ConPincode : "";


                $scope.PASR_AadharNo = promise.studentReg_DTObj[0].pasR_AadharNo != 0 ? promise.studentReg_DTObj[0].pasR_AadharNo : "";

                $scope.PASR_AccountNo = promise.studentReg_DTObj[0].pasR_AccountNo != 0 ? promise.studentReg_DTObj[0].pasR_AccountNo : "";

                $scope.PASR_BankName = promise.studentReg_DTObj[0].pasR_BankName != 0 ? promise.studentReg_DTObj[0].pasR_BankName : "";

                $scope.PASR_BranchName = promise.studentReg_DTObj[0].pasR_BranchName != 0 ? promise.studentReg_DTObj[0].pasR_BranchName : "";

                $scope.PASR_IFSCCode = promise.studentReg_DTObj[0].pasR_IFSCCode != 0 ? promise.studentReg_DTObj[0].pasR_IFSCCode : "";

                $scope.PASR_Domicile = promise.studentReg_DTObj[0].pasR_Domicile != 0 ? promise.studentReg_DTObj[0].pasR_Domicile : "";

                $scope.PASR_SchoolDISECode = promise.studentReg_DTObj[0].pasR_SchoolDISECode != 0 ? promise.studentReg_DTObj[0].pasR_SchoolDISECode : "";

                $scope.PASR_MedicalComplaints = promise.studentReg_DTObj[0].pasR_MedicalComplaints != 0 ? promise.studentReg_DTObj[0].pasR_MedicalComplaints : "";

                $scope.PASR_OtherInformations = promise.studentReg_DTObj[0].pasR_OtherInformations != 0 ? promise.studentReg_DTObj[0].pasR_OtherInformations : "";
                $scope.PASR_NoOfDependencies = promise.studentReg_DTObj[0].pasR_NoOfDependencies != 0 ? promise.studentReg_DTObj[0].pasR_NoOfDependencies : "";

                $scope.PASR_NewlyAdmisstedFlg = promise.studentReg_DTObj[0].pasR_NewlyAdmisstedFlg != 0 ? promise.studentReg_DTObj[0].pasR_NewlyAdmisstedFlg : "";

                $scope.PASR_VaccinatedFlg = promise.studentReg_DTObj[0].pasR_VaccinatedFlg != 0 ? promise.studentReg_DTObj[0].pasR_VaccinatedFlg : "";

                $scope.PASR_Tcflag = promise.studentReg_DTObj[0].pasR_Tcflag != 0 ? promise.studentReg_DTObj[0].pasR_Tcflag : "";


                $scope.PASR_Distirct = promise.studentReg_DTObj[0].pasR_Distirct != 0 ? promise.studentReg_DTObj[0].pasR_Distirct : "";
                $scope.PASR_Taluk = promise.studentReg_DTObj[0].pasR_Taluk != 0 ? promise.studentReg_DTObj[0].pasR_Taluk : "";
                $scope.PASR_Village = promise.studentReg_DTObj[0].pasR_Village != 0 ? promise.studentReg_DTObj[0].pasR_Village : "";
                $scope.PASR_Stayingwith = promise.studentReg_DTObj[0].pasR_Stayingwith != 0 ? promise.studentReg_DTObj[0].pasR_Stayingwith : "";

                $scope.PASR_Languagespeaking = promise.studentReg_DTObj[0].pasR_Languagespeaking != 0 ? promise.studentReg_DTObj[0].pasR_Languagespeaking : "";
                $scope.PASR_FatherPanno = promise.studentReg_DTObj[0].pasR_FatherPanno != 0 ? promise.studentReg_DTObj[0].pasR_FatherPanno : "";
                $scope.PASR_MotherPanno = promise.studentReg_DTObj[0].pasR_MotherPanno != 0 ? promise.studentReg_DTObj[0].pasR_MotherPanno : "";

                $scope.PASR_MobileNo = promise.studentReg_DTObj[0].pasR_MobileNo != 0 ? promise.studentReg_DTObj[0].pasR_MobileNo : "";
                $scope.PASR_emailId = promise.studentReg_DTObj[0].pasR_emailId == null ? "" : promise.studentReg_DTObj[0].pasR_emailId;
                $scope.PASR_MaritalStatus = promise.studentReg_DTObj[0].pasR_MaritalStatus == null ? "" : promise.studentReg_DTObj[0].pasR_MaritalStatus;
                $scope.PASR_FatherMaritalStatus = promise.studentReg_DTObj[0].pasR_FatherMaritalStatus == null ? "" : promise.studentReg_DTObj[0].pasR_FatherMaritalStatus;
                $scope.PASR_MotherMaritalStatus = promise.studentReg_DTObj[0].pasR_MotherMaritalStatus == null ? "" : promise.studentReg_DTObj[0].pasR_MotherMaritalStatus;

                $scope.PASR_FatherAliveFlag = promise.studentReg_DTObj[0].pasR_FatherAliveFlag == 0 ? "Not Alive" : "Alive";
                $scope.PASR_FatherName = promise.studentReg_DTObj[0].pasR_FatherName == null ? "" : promise.studentReg_DTObj[0].pasR_FatherName;
                $scope.PASR_FatherAadharNo = promise.studentReg_DTObj[0].pasR_FatherAadharNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherAadharNo;
                $scope.PASR_FatherSurname = promise.studentReg_DTObj[0].pasR_FatherSurname == null ? "" : promise.studentReg_DTObj[0].pasR_FatherSurname;
                $scope.PASR_FatherChurchAffiliation = promise.studentReg_DTObj[0].pasR_FatherChurchAffiliation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherChurchAffiliation;
                $scope.PASR_FatherSelfEmployedFlg = promise.studentReg_DTObj[0].pasR_FatherSelfEmployedFlg == null ? "" : promise.studentReg_DTObj[0].pasR_FatherSelfEmployedFlg;
                $scope.PASR_FatherEducation = promise.studentReg_DTObj[0].pasR_FatherEducation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherEducation;
                $scope.PASR_FatherOccupation = promise.studentReg_DTObj[0].pasR_FatherOccupation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherOccupation;

                $scope.PASR_FatherPresentAddress = promise.studentReg_DTObj[0].pasR_FatherPresentAddress == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentAddress;
                if ($scope.PASR_FatherPresentAddress.length <= 30) {
                    $scope.pasrfatherresi1 = $scope.PASR_FatherPresentAddress.substring(0, 30);
                    $scope.pasrfatherresi1 = $scope.uppercase($scope.pasrfatherresi1);
                }
                if ($scope.PASR_FatherPresentAddress.length > 30) {
                    $scope.pasrfatherresi1 = $scope.PASR_FatherPresentAddress.substring(0, 30);
                    $scope.pasrfatherresi1 = $scope.uppercase($scope.pasrfatherresi1);
                    $scope.pasrfatherresi2 = $scope.PASR_FatherPresentAddress.substring(30, $scope.PASR_FatherPresentAddress.length);
                    $scope.pasrfatherresi2 = $scope.uppercase($scope.pasrfatherresi2);
                }
                $scope.PASR_FatherPresentCity = promise.studentReg_DTObj[0].pasR_FatherPresentCity == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentCity;
                $scope.PASR_FatherPresentPS = promise.studentReg_DTObj[0].pasR_FatherPresentPS == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentPS;
                $scope.PASR_FatherPresentPO = promise.studentReg_DTObj[0].pasR_FatherPresentPO == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentPO;
                $scope.PASR_FatherPresentPinCode = promise.studentReg_DTObj[0].pasR_FatherPresentPinCode == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentPinCode;

                $scope.PASR_MotherPresentAddress = promise.studentReg_DTObj[0].pasR_MotherPresentAddress == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentAddress;
                $scope.PASR_MotherPresentCity = promise.studentReg_DTObj[0].pasR_MotherPresentCity == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentCity;
                $scope.PASR_MotherPresentPS = promise.studentReg_DTObj[0].pasR_MotherPresentPS == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentPS;
                $scope.PASR_MotherPresentPO = promise.studentReg_DTObj[0].pasR_MotherPresentPO == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentPO;
                $scope.PASR_MotherPresentPinCode = promise.studentReg_DTObj[0].pasR_MotherPresentPinCode == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentPinCode;

                $scope.PASR_MotherPermanentAddress = promise.studentReg_DTObj[0].pasR_MotherPermanentAddress == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentAddress;
                $scope.PASR_MotherPermanentCity = promise.studentReg_DTObj[0].pasR_MotherPermanentCity == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentCity;
                $scope.PASR_MotherPermanentPS = promise.studentReg_DTObj[0].pasR_MotherPermanentPS == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentPS;
                $scope.PASR_MotherPermanentPO = promise.studentReg_DTObj[0].pasR_MotherPermanentPO == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentPO;
                $scope.PASR_MotherPermanentPinCode = promise.studentReg_DTObj[0].pasR_MotherPermanentPinCode == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentPinCode;

                $scope.PASR_FatherPermanentAddress = promise.studentReg_DTObj[0].pasR_FatherPermanentAddress == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentAddress;
                $scope.PASR_FatherPermanentCity = promise.studentReg_DTObj[0].pasR_FatherPermanentCity == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentCity;
                $scope.PASR_FatherPermanentPS = promise.studentReg_DTObj[0].pasR_FatherPermanentPS == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentPS;
                $scope.PASR_FatherPermanentPO = promise.studentReg_DTObj[0].pasR_FatherPermanentPO == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentPO;
                $scope.PASR_FatherPermanentPinCode = promise.studentReg_DTObj[0].pasR_FatherPermanentPinCode == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentPinCode;

                $scope.PASR_FatherBankName = promise.studentReg_DTObj[0].pasR_FatherBankName == null ? "" : promise.studentReg_DTObj[0].pasR_FatherBankName;
                $scope.PASR_FatherBankBranch = promise.studentReg_DTObj[0].pasR_FatherBankBranch == null ? "" : promise.studentReg_DTObj[0].pasR_FatherBankBranch;
                $scope.PASR_FatherIFSC = promise.studentReg_DTObj[0].pasR_FatherIFSC == null ? "" : promise.studentReg_DTObj[0].pasR_FatherIFSC;
                $scope.PASR_FatherBankAccount = promise.studentReg_DTObj[0].pasR_FatherBankAccount == null ? "" : promise.studentReg_DTObj[0].pasR_FatherBankAccount;

                $scope.PASR_MotherBankName = promise.studentReg_DTObj[0].pasR_MotherBankName == null ? "" : promise.studentReg_DTObj[0].pasR_MotherBankName;
                $scope.PASR_MotherBankBranch = promise.studentReg_DTObj[0].pasR_MotherBankBranch == null ? "" : promise.studentReg_DTObj[0].pasR_MotherBankBranch;
                $scope.PASR_MotherIFSC = promise.studentReg_DTObj[0].pasR_MotherIFSC == null ? "" : promise.studentReg_DTObj[0].pasR_MotherIFSC;
                $scope.PASR_MotherBankAccount = promise.studentReg_DTObj[0].pasR_MotherBankAccount == null ? "" : promise.studentReg_DTObj[0].pasR_MotherBankAccount;

                $scope.PASR_FatherDesignation = promise.studentReg_DTObj[0].pasR_FatherDesignation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherDesignation;
                $scope.PASR_FatherIncome = promise.studentReg_DTObj[0].pasR_FatherIncome;
                $scope.PASR_FatherMonIncome = promise.studentReg_DTObj[0].pasR_FatherMonIncome;
                $scope.PASR_FatherAnnIncome = promise.studentReg_DTObj[0].pasR_FatherAnnIncome;
                $scope.PASR_FatherMobleNo = promise.studentReg_DTObj[0].pasR_FatherMobleNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherMobleNo;
                $scope.PASR_FatheremailId = promise.studentReg_DTObj[0].pasR_FatheremailId == null ? "" : promise.studentReg_DTObj[0].pasR_FatheremailId;

                $scope.syllabussname = promise.sylabusss.length > 0 ? promise.sylabusss[0].imC_CasteName : "";
                $scope.PASR_FatherReligion = promise.fatherreligion.length > 0 ? promise.fatherreligion[0].imC_CasteName : "";
                $scope.PASR_FatherCaste = promise.fathercaste.length > 0 ? promise.fathercaste[0].imC_CasteName : "";
                $scope.overgae = promise.studentReg_DTObj[0].pasR_OverAge;
                $scope.undergae = promise.studentReg_DTObj[0].pasR_UnderAge;

                //$scope.fatherSubCaste_Id = promise.fathersubcaste.length > 0 ? promise.fathersubcaste[0].imC_CasteName : "";;
                //$scope.motherSubCaste_Id = promise.mothersubcaste.length > 0 ? promise.mothersubcaste[0].imC_CasteName : "";;
                //$scope.SubCaste_Id = promise.subcaste.length > 0 ? promise.subcaste[0].imC_CasteName : "";;
                //$scope.reg.fatherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Fathersubcaste;
                //$scope.reg.motherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Mothersubcatse;
                //$scope.reg.SubCaste_Id = promise.studentReg_DTObj[0].pasR_Subcaste;
                $scope.fatherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Fathersubcaste == null ? "" : promise.studentReg_DTObj[0].pasR_Fathersubcaste;
                $scope.SubCaste_Id = promise.studentReg_DTObj[0].pasR_Subcaste == null ? "" : promise.studentReg_DTObj[0].pasR_Subcaste;
                $scope.motherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Mothersubcatse == null ? "" : promise.studentReg_DTObj[0].pasR_Mothersubcatse;

                $scope.PASR_FatherTribe = promise.studentReg_DTObj[0].pasR_FatherTribe;
                $scope.PASR_Tribe = promise.studentReg_DTObj[0].pasR_Tribe;
                $scope.PASR_MotherReligion = promise.motherreligion.length > 0 ? promise.motherreligion[0].imC_CasteName : "";
                $scope.PASR_MotherCaste = promise.mothercaste.length > 0 ? promise.mothercaste[0].imC_CasteName : "";;
                $scope.PASR_MotherTribe = promise.studentReg_DTObj[0].pasR_MotherTribe;

                $scope.PASR_FirstLanguage = promise.studentReg_DTObj[0].pasR_FirstLanguage == null ? "" : promise.studentReg_DTObj[0].pasR_FirstLanguage;
                $scope.PASR_Thirdlanguage = promise.studentReg_DTObj[0].pasR_Thirdlanguage == null ? "" : promise.studentReg_DTObj[0].pasR_Thirdlanguage;
                $scope.PASR_SecondLanguage = promise.studentReg_DTObj[0].pasR_SecondLanguage == null ? "" : promise.studentReg_DTObj[0].pasR_SecondLanguage;
                //   $scope.PASR_Thirdlanguage = promise.studentReg_DTObj[0].pasR_Thirdlanguage == null ? "" : promise.studentReg_DTObj[0].pasR_Thirdlanguage;


                $scope.PASR_MotherAliveFlag = promise.studentReg_DTObj[0].pasR_MotherAliveFlag == 0 ? "Not Alive" : "Alive";
                $scope.PASR_MotherName = promise.studentReg_DTObj[0].pasR_MotherName == null ? "" : promise.studentReg_DTObj[0].pasR_MotherName;
                $scope.PASR_MotherAadharNo = promise.studentReg_DTObj[0].pasR_MotherAadharNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherAadharNo;
                $scope.PASR_MotherSurname = promise.studentReg_DTObj[0].pasR_MotherSurname == null ? "" : promise.studentReg_DTObj[0].pasR_MotherSurname;
                $scope.PASR_MotherChurchAffiliation = promise.studentReg_DTObj[0].pasR_MotherChurchAffiliation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherChurchAffiliation;
                $scope.PASR_MotherSelfEmployedFlg = promise.studentReg_DTObj[0].pasR_MotherSelfEmployedFlg == null ? "" : promise.studentReg_DTObj[0].pasR_MotherSelfEmployedFlg;
                $scope.PASR_MotherEducation = promise.studentReg_DTObj[0].pasR_MotherEducation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherEducation;
                $scope.PASR_MotherOccupation = promise.studentReg_DTObj[0].pasR_MotherOccupation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherOccupation;
                $scope.PASR_MotherDesignation = promise.studentReg_DTObj[0].pasR_MotherDesignation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherDesignation;
                $scope.PASR_MotherIncome = promise.studentReg_DTObj[0].pasR_MotherIncome;
                $scope.PASR_MotherMonIncome = promise.studentReg_DTObj[0].pasR_MotherMonIncome;
                $scope.PASR_MotherAnnIncome = promise.studentReg_DTObj[0].pasR_MotherAnnIncome;
                $scope.PASR_MotherMobleNo = promise.studentReg_DTObj[0].pasR_MotherMobleNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherMobleNo;
                $scope.PASR_MotheremailId = promise.studentReg_DTObj[0].pasR_MotheremailId == null ? "" : promise.studentReg_DTObj[0].pasR_MotheremailId;


                $scope.PASR_ChurchAddress = promise.studentReg_DTObj[0].pasR_ChurchAddress == 0 ? "" : promise.studentReg_DTObj[0].pasR_ChurchAddress;
                $scope.PASR_Churchname = promise.studentReg_DTObj[0].pasR_ChurchName == 0 ? "" : promise.studentReg_DTObj[0].pasR_ChurchName;

                $scope.PASR_FatherOfficePhNo = promise.studentReg_DTObj[0].pasR_FatherOfficePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherOfficePhNo; $scope.PASR_FatherHomePhNo = promise.studentReg_DTObj[0].pasR_FatherHomePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherHomePhNo; $scope.PASR_MotherOfficePhNo = promise.studentReg_DTObj[0].pasR_MotherOfficePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherOfficePhNo;
                $scope.PASR_MotherHomePhNo = promise.studentReg_DTObj[0].pasR_MotherHomePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherHomePhNo;

                $scope.PASR_FatherPassingYear = promise.studentReg_DTObj[0].pasR_FatherPassingYear == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherPassingYear; $scope.PASR_MotherPassingYear = promise.studentReg_DTObj[0].pasR_MotherPassingYear == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherPassingYear;




                $scope.PASR_TotalIncome = $scope.PASR_FatherIncome + $scope.PASR_MotherIncome;
                $scope.PASR_BirthPlace = promise.studentReg_DTObj[0].pasR_BirthPlace == null ? "" : promise.studentReg_DTObj[0].pasR_BirthPlace;
                $scope.studentnationality = promise.studentnationalitys.length > 0 ? promise.studentnationalitys[0].studentnationality : "";

                $scope.PASR_HostelReqdFlag = promise.studentReg_DTObj[0].pasR_HostelReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_TransportReqdFlag = promise.studentReg_DTObj[0].pasR_TransportReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_Studentillness = promise.studentReg_DTObj[0].pasR_Studentillness == 1 ? "Yes" : "No";//true : false
                $scope.PASR_PovertyLine = promise.studentReg_DTObj[0].pasR_PovertyLine;//true : false


                //for (var i = 0; i < $scope.allRefrence.length; i++) {
                //    $scope.allRefrence[i].Selected = false;
                //}
                //for (var i = 0; i < $scope.allRefrence.length; i++) {
                //    name = $scope.allRefrence[i].pamR_Id;
                //    for (var j = 0; j < promise.studentReferenceDetails.length; j++) {
                //        if (name == promise.studentReferenceDetails[j].pamR_Id) {
                //            $scope.allRefrence[i].Selected = true;
                //            // $scope.Refrencecheckboxchcked.push(promise.studentReferenceDetails[j]);
                //        }
                //    }
                //}

                $scope.PASR_NickName = promise.studentReg_DTObj[0].pasR_NickName;//true : false
                $scope.PASR_FatherHobbies = promise.studentReg_DTObj[0].pasR_FatherHobbies;//true : false
                $scope.PASR_MotherHobbies = promise.studentReg_DTObj[0].pasR_MotherHobbies;//true : false
                $scope.PASR_NeighbourPhoneNo = promise.studentReg_DTObj[0].pasR_NeighbourPhoneNo;//true : false
                $scope.PASR_NeighbourAddr1 = promise.studentReg_DTObj[0].pasR_NeighbourAddr1;//true : false
                $scope.PASR_NeighbourAddr2 = promise.studentReg_DTObj[0].pasR_NeighbourAddr2;//true : false
                $scope.PASR_NeighbourName = promise.studentReg_DTObj[0].pasR_NeighbourName;//true : false
                $scope.PASR_PhysicalDisability = promise.studentReg_DTObj[0].pasR_PhysicalDisability;//true : false

                $scope.PASR_GymReqdFlag = promise.studentReg_DTObj[0].pasR_GymReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_ECSFlag = promise.studentReg_DTObj[0].pasR_ECSFlag == 1 ? "Yes" : "No";
                $scope.PASR_PaymentFlag = promise.studentReg_DTObj[0].pasR_PaymentFlag == 1 ? "Yes" : "No";

                $scope.PASR_AmountPaid = promise.studentReg_DTObj[0].pasR_AmountPaid;
                $scope.PASR_PaymentType = promise.studentReg_DTObj[0].pasR_PaymentType == null ? "" : promise.studentReg_DTObj[0].pasR_PaymentType;
                $scope.PASR_PaymentDate = promise.studentReg_DTObj[0].pasR_PaymentDate == null ? "" : promise.studentReg_DTObj[0].pasR_PaymentDate;
                $scope.PASR_ReceiptNo = promise.studentReg_DTObj[0].pasR_ReceiptNo == null ? "" : promise.studentReg_DTObj[0].pasR_ReceiptNo;
                //Activeflag
                //Applstatus
                $scope.PASR_FinalpaymentFlag = promise.studentReg_DTObj[0].pasR_FinalpaymentFlag == 1 ? "Yes" : "No";
                $scope.PASR_LastPlayGrndAttnd = promise.studentReg_DTObj[0].pasR_LastPlayGrndAttnd == null ? "" : promise.studentReg_DTObj[0].pasR_LastPlayGrndAttnd;
                $scope.PASR_ExtraActivity = promise.studentReg_DTObj[0].pasR_ExtraActivity == null ? "" : promise.studentReg_DTObj[0].pasR_ExtraActivity;
                $scope.PASR_OtherResidential_Addr = promise.studentReg_DTObj[0].pasR_OtherResidential_Addr == null ? "" : promise.studentReg_DTObj[0].pasR_OtherPermanentAddr;
                $scope.PASR_OtherPermanentAddr = promise.studentReg_DTObj[0].pasR_OtherPermanentAddr == null ? "" : promise.studentReg_DTObj[0].pasR_OtherPermanentAddr;
                $scope.PASR_FatherOfficeAddr = promise.studentReg_DTObj[0].pasR_FatherOfficeAddr == null ? "" : promise.studentReg_DTObj[0].pasR_FatherOfficeAddr;
                $scope.PASR_MotherOfficeAddr = promise.studentReg_DTObj[0].pasR_MotherOfficeAddr == null ? "" : promise.studentReg_DTObj[0].pasR_MotherOfficeAddr;
                $scope.PASR_UndertakingFlag = promise.studentReg_DTObj[0].pasR_UndertakingFlag == 1 ? "Yes" : "No";
                $scope.fathernationality = promise.fathernationalitys.length > 0 ? promise.fathernationalitys[0].fathernationality : "";
                $scope.mothernationality = promise.mothernationalitys.length > 0 ? promise.mothernationalitys[0].mothernationality : "";
                $scope.PASR_BirthCertificateNo = promise.studentReg_DTObj[0].pasR_BirthCertificateNo == null ? "" : promise.studentReg_DTObj[0].pasR_BirthCertificateNo;
                $scope.PASR_CatseCertificateNo = promise.studentReg_DTObj[0].pasR_CatseCertificateNo == null ? "" : promise.studentReg_DTObj[0].pasR_CatseCertificateNo;
                $scope.PASR_AdmissionReason = promise.studentReg_DTObj[0].pasR_AdmissionReason == null ? "" : promise.studentReg_DTObj[0].pasR_AdmissionReason;
                $scope.PASR_Illnessdetails = promise.studentReg_DTObj[0].pasR_Illnessdetails == null ? "" : promise.studentReg_DTObj[0].pasR_Illnessdetails;
                $scope.PASR_AltContactNo = promise.studentReg_DTObj[0].pasR_AltContactNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_AltContactNo;
                $scope.PASR_AltContactEmail = promise.studentReg_DTObj[0].pasR_AltContactEmail == null ? "" : promise.studentReg_DTObj[0].pasR_AltContactEmail;

                if ($scope.PASR_FatherOfficeAddr.length <= 30) {
                    $scope.pasrfathrnature = $scope.PASR_FatherOfficeAddr.substring(0, $scope.PASR_FatherOfficeAddr.length);
                    $scope.pasrfathrnature = $scope.uppercase($scope.pasrfathrnature);
                }
                if ($scope.PASR_FatherOfficeAddr.length > 30) {
                    $scope.pasrfathrnature = $scope.PASR_FatherOfficeAddr.substring(0, 30);
                    $scope.pasrfathrnature = $scope.uppercase($scope.pasrfathrnature);
                    $scope.pasrfathradd = $scope.PASR_FatherOfficeAddr.substring(30, $scope.PASR_FatherOfficeAddr.length);
                    $scope.pasrfathradd = $scope.uppercase($scope.pasrfathradd);
                }
                if ($scope.PASR_FatherOfficeAddr.length <= 65) {
                    $scope.pasrfathrnature1 = $scope.PASR_FatherOfficeAddr.substring(0, $scope.PASR_FatherOfficeAddr.length);
                    $scope.pasrfathrnature1 = $scope.uppercase($scope.pasrfathrnature1);
                }
                if ($scope.PASR_FatherOfficeAddr.length > 65) {
                    $scope.pasrfathrnature1 = $scope.PASR_FatherOfficeAddr.substring(0, 65);
                    $scope.pasrfathrnature1 = $scope.uppercase($scope.pasrfathrnature1);
                    $scope.pasrfathradd1 = $scope.PASR_FatherOfficeAddr.substring(65, $scope.PASR_FatherOfficeAddr.length);
                    $scope.pasrfathradd1 = $scope.uppercase($scope.pasrfathradd1);
                }

                if ($scope.PASR_MotherOfficeAddr.length <= 30) {
                    $scope.pasrmothrnature = $scope.PASR_MotherOfficeAddr.substring(0, $scope.PASR_MotherOfficeAddr.length);
                    $scope.pasrmothrnature = $scope.uppercase($scope.pasrmothrnature);
                }
                if ($scope.PASR_MotherOfficeAddr.length > 30) {
                    $scope.pasrmothrnature = $scope.PASR_MotherOfficeAddr.substring(0, 30);
                    $scope.pasrmothrnature = $scope.uppercase($scope.pasrmothrnature);
                    $scope.pasrmothradd = $scope.PASR_MotherOfficeAddr.substring(30, $scope.PASR_MotherOfficeAddr.length);
                    $scope.pasrmothradd = $scope.uppercase($scope.pasrmothradd);
                }
                if ($scope.PASR_MotherOfficeAddr.length <= 65) {
                    $scope.pasrmothrnature1 = $scope.PASR_MotherOfficeAddr.substring(0, $scope.PASR_MotherOfficeAddr.length);
                    $scope.pasrmothrnature1 = $scope.uppercase($scope.pasrmothrnature1);
                }
                if ($scope.PASR_MotherOfficeAddr.length > 65) {
                    $scope.pasrmothrnature1 = $scope.PASR_MotherOfficeAddr.substring(0, 65);
                    $scope.pasrmothrnature1 = $scope.uppercase($scope.pasrmothrnature1);
                    $scope.pasrmothradd1 = $scope.PASR_MotherOfficeAddr.substring(65, $scope.PASR_MotherOfficeAddr.length);
                    $scope.pasrmothradd1 = $scope.uppercase($scope.pasrmothradd1);
                }
                if ($scope.PASR_MotherOccupation.length >= 25) {
                    $scope.selfMotherOccupation = angular.lowercase($scope.PASR_MotherOccupation);
                }
                else {
                    $scope.selfMotherOccupation = $scope.PASR_MotherOccupation;
                }
                if ($scope.PASR_FatherOccupation.length >= 25) {
                    $scope.selfFatherOccupation = angular.lowercase($scope.PASR_FatherOccupation);
                }
                else {
                    $scope.selfFatherOccupation = $scope.PASR_FatherOccupation;
                }

                //PASR_Adm_Confirm_Flag
                //PAMS_Id
                //Id
                var photostd = "";
                if (promise.studentReg_DTObj[0].pasR_Student_Pic_Path != null && promise.studentReg_DTObj[0].pasR_Student_Pic_Path != '') {
                    photostd = promise.studentReg_DTObj[0].pasR_Student_Pic_Path;
                }
                else {
                    photostd = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                }

                $('#blahnew').attr('src', photostd);

                $scope.studentphoto = photostd;

                $('#blahnewj').attr('src', promise.studentReg_DTObj[0].pasR_JointPhoto);
                $scope.studentphotojoint = promise.studentReg_DTObj[0].pasR_JointPhoto;
                //PASL_ID
                //PASL_ID
                //Remark
                //Repeat_Class_Id
                // $scope.FMCC_Id = promise.studentReg_DTObj[0].fmcC_ID;
                $scope.PASR_Noofbrothers = promise.studentReg_DTObj[0].pasR_Noofbrothers == null ? "" : promise.studentReg_DTObj[0].pasR_Noofbrothers;
                $scope.PASR_Noofsisters = promise.studentReg_DTObj[0].pasR_Noofsisters == null ? "" : promise.studentReg_DTObj[0].pasR_Noofsisters;

                $scope.PASR_NoOfElderBrothers = promise.studentReg_DTObj[0].pasR_NoOfElderBrothers == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfElderBrothers;
                $scope.PASR_NoOfYoungerBrothers = promise.studentReg_DTObj[0].pasR_NoOfYoungerBrothers == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfYoungerBrothers;

                $scope.PASR_NoOfElderSisters = promise.studentReg_DTObj[0].pasR_NoOfElderSisters == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfElderSisters;
                $scope.PASR_NoOfYoungerSisters = promise.studentReg_DTObj[0].pasR_NoOfYoungerSisters == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfYoungerSisters;


                $scope.PASR_lastclassperc = promise.studentReg_DTObj[0].pasR_lastclassperc == null ? "" : promise.studentReg_DTObj[0].pasR_lastclassperc;
                $scope.PASR_SibblingConcessionFlag = promise.studentReg_DTObj[0].pasR_SibblingConcessionFlag == 1 ? "Yes" : "No";
                $scope.PASR_ParentConcessionFlag = promise.studentReg_DTObj[0].pasR_ParentConcessionFlag == 1 ? "Yes" : "No";

                //// guardian
                if (promise.studentGuardian_DTObj != null && promise.studentGuardian_DTObj != "") {
                    $scope.pasrG_Id = promise.studentGuardian_DTObj[0].pasrG_Id;
                    $scope.PASRG_GuardianName = promise.studentGuardian_DTObj[0].pasrG_GuardianName;
                    $scope.PASRG_GuardianAddress = promise.studentGuardian_DTObj[0].pasrG_GuardianAddress;
                    $scope.PASRG_GuardianRelation = promise.studentGuardian_DTObj[0].pasrG_GuardianRelation;
                    $scope.PASRG_emailid = promise.studentGuardian_DTObj[0].pasrG_emailid;
                    $scope.PASRG_GuardianPhoneNo = promise.studentGuardian_DTObj[0].pasrG_GuardianPhoneNo;
                    $scope.PASRG_Occupation = promise.studentGuardian_DTObj[0].pasrG_Occupation;
                    $scope.PASRG_FeeUndertakeFlg = promise.studentGuardian_DTObj[0].pasrG_FeeUndertakeFlg;
                    $scope.PASRG_PhoneOffice = promise.studentGuardian_DTObj[0].pasrG_PhoneOffice;
                }
                else {
                    $scope.reg.pasrG_Id = 0;
                    $scope.reg.PASRG_GuardianName = "";
                    $scope.reg.PASRG_GuardianAddress = "";
                    $scope.reg.PASRG_GuardianRelation = "";
                    $scope.reg.PASRG_emailid = "";
                    $scope.reg.PASRG_GuardianPhoneNo = "";
                    $scope.reg.PASRG_PhoneOffice = "";
                    $scope.reg.PASRG_Occupation = "";
                    $scope.reg.PASRG_FeeUndertakeFlg = "";
                }
                ////

                //// Sibling
                $scope.firstsibling = '';
                $scope.firstsiblingclass = '';
                $scope.secondsibling = '';
                $scope.secondsiblingclass = '';
                if (promise.studentSbling_DTObj.length > 0) {
                    $scope.showrepatsib = true;
                    $scope.siblingsprint = promise.studentSbling_DTObj;

                    if ($scope.siblingsprint.length == 1) {
                        $scope.firstsibling = $scope.siblingsprint[0].pasrS_SiblingsName;
                        ////if ($scope.firstsibling === null || $scope.firstsibling === '') {
                        //    $scope.showrepatsib = false;
                        ////}
                        $scope.firstsiblingclass = $scope.siblingsprint[0].pasrS_SiblingsClass;
                        $scope.firstsiblingsection = $scope.siblingsprint[0].pasrS_SiblingsSection;
                        $scope.firstsiblingschool = $scope.siblingsprint[0].pasrS_SchoolName;
                        $scope.firstsiblingschoolAdmissionNo = $scope.siblingsprint[0].pasrS_SiblingsAdmissionNo;
                        $scope.firstsiblinginst = $scope.siblingsprint[0].pasrS_Institution;
                    }
                    if ($scope.siblingsprint.length == 2) {
                        $scope.firstsibling = $scope.siblingsprint[0].pasrS_SiblingsName;
                        $scope.firstsiblingclass = $scope.siblingsprint[0].pasrS_SiblingsClass;
                        $scope.firstsiblingsection = $scope.siblingsprint[0].pasrS_SiblingsSection;
                        $scope.firstsiblingschool = $scope.siblingsprint[0].pasrS_SchoolName;
                        $scope.firstsiblingschoolAdmissionNo = $scope.siblingsprint[0].pasrS_SiblingsAdmissionNo;
                        $scope.firstsiblinginst = $scope.siblingsprint[0].pasrS_Institution;
                        $scope.secondsibling = $scope.siblingsprint[1].pasrS_SiblingsName;
                        $scope.secondsiblingclass = $scope.siblingsprint[1].pasrS_SiblingsClass;
                        $scope.secondsiblingsection = $scope.siblingsprint[1].pasrS_SiblingsSection;
                        $scope.secondsiblingschool = $scope.siblingsprint[1].pasrS_SchoolName;
                        $scope.secondsiblingschoolAdmissionNo = $scope.siblingsprint[1].pasrS_SiblingsAdmissionNo;
                        $scope.secondsiblingschoolinst = $scope.siblingsprint[1].pasrS_Institution;
                    }
                    $scope.sibl = "Yes!";
                    $scope.showbr = false;
                    $scope.siblingshow = true;
                }
                else {
                    $scope.sibl = "No!";
                    $scope.showbr = true;
                    $scope.siblingshow = false;
                }
                if (promise.academicdrp.length > 0) {
                    $scope.ASMAY_Year = promise.academicdrp[0].asmaY_Year;
                }
                $scope.allRefrence = promise.studentReferenceDetails;
                $scope.allSources = promise.studentSourceDetails;
                $scope.reference = "";
                $scope.source = "";
                if ($scope.allRefrence != null && $scope.allRefrence.length > 0) {
                    //$scope.reference = $scope.allRefrence[0].
                }
                if ($scope.allSources != null && $scope.allSources.length > 0) {
                    // $scope.source = $scope.allRefrence[0].
                }
                if (promise.studenthelthDTO != null) {
                    if (promise.studenthelthDTO.length > 0) {
                        $scope.albumNameArray1 = [];
                        $scope.albumNameArray2 = [];
                        for (var i = 0; i < promise.studenthelthDTO.length; i++) {
                            if (promise.studenthelthDTO[i].pasR_FirstName != '') {
                                if (promise.studenthelthDTO[i].pasR_MiddleName !== null) {
                                    if (promise.studenthelthDTO[i].pasR_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + " " + promise.studenthelthDTO[i].pasR_MiddleName + " " + promise.studenthelthDTO[i].pasR_LastName });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + " " + promise.studenthelthDTO[i].pasR_MiddleName });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName });
                                }
                            }
                        }

                        $scope.Hstuname = $scope.albumNameArray1[0].name;
                        $scope.Hstuage = promise.studenthelthDTO[0].pasR_Age;
                        $scope.HFirstName = promise.studenthelthDTO[0].pasR_FatherName;
                        $scope.VaccinationDate = promise.studenthelthDTO[0].pashD_VaccinationDate;
                        if (promise.studenthelthDTO[0].pashD_FitsFlag == 1) {
                            $scope.FitsFlag = "Yes";
                            $scope.FitsDate = promise.studenthelthDTO[0].pashD_FitsDate;
                        }
                        else {
                            $scope.FitsFlag = "No";
                            $scope.FitsDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_AllergyFlg == true) {
                            $scope.AllergyFlag = "Yes";
                            $scope.Allergy = promise.studenthelthDTO[0].pashD_Allergy;
                        }
                        else {
                            $scope.AllergyFlag = "No";
                            $scope.Allergy = " ";
                        }

                        if (promise.studenthelthDTO[0].pashD_ChickenpoxFlag == 1) {
                            $scope.chickenFlag = "Yes";
                            $scope.cihickenDate = promise.studenthelthDTO[0].pashD_ChickenpoxDate;
                        }
                        else {
                            $scope.chickenFlag = "No";
                            $scope.cihickenDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_DiptheriaFlag == 1) {
                            $scope.dipFlag = "Yes";
                            $scope.dipDate = promise.studenthelthDTO[0].pashD_DiptheriaDate;
                        }
                        else {
                            $scope.dipFlag = "No";
                            $scope.dipDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_EpidemicFlag == 1) {
                            $scope.epideFlag = "Yes";
                            $scope.epideDate = promise.studenthelthDTO[0].pashD_EpidemicDate;
                        }
                        else {
                            $scope.epideFlag = "No";
                            $scope.epideDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_MeaslesFlag == 1) {
                            $scope.measleFlag = "Yes";
                            $scope.measleDate = promise.studenthelthDTO[0].pashD_MeaslesDate;
                        }
                        else {
                            $scope.measleFlag = "No";
                            $scope.measleDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_MumpsFlag == 1) {
                            $scope.mumFlag = "Yes";
                            $scope.mumDate = promise.studenthelthDTO[0].pashD_MumpsDate;
                        }
                        else {
                            $scope.mumFlag = "No";
                            $scope.mumDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_RingwormFlag == 1) {
                            $scope.ringFlag = "Yes";
                            $scope.ringDate = promise.studenthelthDTO[0].pashD_RingwormDate;
                        }
                        else {
                            $scope.ringFlag = "No";
                            $scope.ringDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_ScarletFlag == 1) {
                            $scope.scarletFlag = "Yes";
                            $scope.scarletDate = promise.studenthelthDTO[0].pashD_ScarletDate;
                        }
                        else {
                            $scope.scarletFlag = "No";
                            $scope.scarletDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_SmallPoxFlag == 1) {
                            $scope.smallFlag = "Yes";
                            $scope.smallDate = promise.studenthelthDTO[0].pashD_SmallPoxDate;
                        }
                        else {
                            $scope.smallFlag = "No";
                            $scope.smallDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_WhoopingFlag == 1) {
                            $scope.whoFlag = "Yes";
                            $scope.whoDate = promise.studenthelthDTO[0].pashD_WhoopingDate;
                        }
                        else {
                            $scope.whoFlag = "No";
                            $scope.whoDate = " ";
                        }

                        if (promise.studenthelthDTO[0].pashD_Illness == 1) {
                            $scope.Illness = "Yes";

                        }
                        else {
                            $scope.Illness = "No";

                        }

                        //$scope.Illness = promise.studenthelthDTO[0].pashD_Illness;
                        $scope.HepatitisB = new Date(promise.studenthelthDTO[0].pashD_HepatitisB);
                        $scope.TyphoidFever = new Date(promise.studenthelthDTO[0].pashD_TyphoidFever);
                        $scope.Ailment = promise.studenthelthDTO[0].pashD_Ailment;
                        //if (promise.studenthelthDTO[0].pashD_Allergy == 1) {
                        //    $scope.Allergy = "Yes";
                        //}
                        //else {
                        //    $scope.Allergy = "No";
                        //}

                        $scope.HealthProblem = promise.studenthelthDTO[0].pashD_HealthProblem;
                        $scope.CronicDesease = promise.studenthelthDTO[0].pashD_CronicDesease;
                        $scope.MEDetails = promise.studenthelthDTO[0].pashD_MEDetails;
                        $scope.MEContactNo = promise.studenthelthDTO[0].pashD_MEContactNo;
                        if (promise.vaccines != null) {
                            if (promise.vaccines.length > 0) {
                                $scope.vaccinesprint = promise.vaccines;
                                //if (promise.vaccines.length > 0) {
                                //    angular.forEach($scope.vaccinesprint, function (opqr1) {
                                //        if (opqr1.pamvA_YesNoFlg == true) {
                                //            opqr1.pamvA_YesNoFlg = "1";
                                //        }
                                //        else if (opqr1.pamvA_YesNoFlg == false) {
                                //            opqr1.pamvA_YesNoFlg = "0";
                                //        }
                                //    })
                                //}
                            }
                        }
                        // $scope.BloodGroup = promise.studenthelthDTO[0].pashD_BloodGroup;
                    }
                }

                //documnets
                if (promise.studentDocuments_DTObj.length > 0) {
                    $scope.document = {};
                    //$scope.documentList = promise.studentDocuments_DTObj;
                    //angular.forEach(promise.studentDocuments_DTObj, function (value, key) {
                    //    //$scope.document.pasrD_Id = value.pasrD_Id;
                    //    //$scope.document.pasR_Id = value.pasR_Id;
                    //    //$scope.document.amsmD_Id = value.amsmD_Id;
                    //    //$scope.document.amsmD_DocumentName = value.amsmD_DocumentName;
                    //    //$scope.document.document_Path = value.document_Path;
                    //    $('#' + value.amsmD_Id).attr('src', value.document_Path);
                    //})
                }

                //// Subjects
                if (promise.studentSubjects_DTObj != null && promise.studentSubjects_DTObj.length > 0) {
                    //  $scope.electivesubgrouplist = promise.StudentSubjects_DTObj;
                    $scope.electivegrouplistprint = promise.electivegrouplist;
                    $scope.electivesubgrouplistprint = promise.electivesubgrouplist;

                    angular.forEach(promise.studentSubjects_DTObj, function (opqr) {
                        angular.forEach($scope.electivesubgrouplistprint, function (opqr1) {
                            if (opqr.emG_Id == opqr1.EMG_Id) {
                                opqr1.ismS_Id = opqr.ismS_Id;
                            }
                        })
                    })

                    $scope.grouplength = $scope.electivesubgrouplistprint.length;

                }
                else {
                    $scope.electivegrouplistprint = {};
                    $scope.electivesubgrouplistprint = {};
                }

                $scope.studentstream = promise.studentstream;

                if ($scope.electivesubgrouplistprint != null && $scope.electivesubgrouplistprint.length >= 0) {
                    $scope.electives = [];
                    $scope.compulsory = [];
                    angular.forEach($scope.electivesubgrouplistprint, function (opqr1) {
                        if (opqr1.EMG_ElectiveFlg == false) {
                            $scope.compulsory.push(opqr1);
                        }
                        else {
                            $scope.electives.push(opqr1);
                        }
                    })
                }
                if (promise.streamexamsprint != null && promise.streamexamsprint.length > 0) {
                    $scope.streamexamsprint = promise.streamexamsprint;
                    $scope.asmceid = promise.asmceId;
                }
                else {
                    $scope.streamexamsprint = [];
                }



                ////// Previous School

                if (promise.studentPrevSch_DTObj.length > 0) {
                    $scope.PreviousSchoolListprint = promise.studentPrevSch_DTObj;

                    $scope.schoolname = $scope.PreviousSchoolListprint[0].pasrpS_PrvSchoolName;
                    $scope.tcno = $scope.PreviousSchoolListprint[0].pasrpS_TcNo;
                    $scope.schooladress = $scope.PreviousSchoolListprint[0].pasrpS_Address;
                    $scope.lastclass = $scope.PreviousSchoolListprint[0].pasrpS_PreviousClass;
                    $scope.lastsylaabus = $scope.PreviousSchoolListprint[0].pasrpS_Board;
                    $scope.percentage = $scope.PreviousSchoolListprint[0].pasrpS_PreviousPer;
                    $scope.leftdate = $scope.PreviousSchoolListprint[0].pasrpS_LeftDate;
                    $scope.TcIssueDate = $scope.PreviousSchoolListprint[0].pasrpS_TcIssueDate;
                    $scope.TCProducingDate = $scope.PreviousSchoolListprint[0].pasrpS_TCProducingDate;
                    $scope.lefyear = $scope.PreviousSchoolListprint[0].pasrpS_LeftYear;
                    $scope.concessionscholer = $scope.PreviousSchoolListprint[0].pasrpS_ConcOrScholarshipFlg;
                    $scope.leftreason = $scope.PreviousSchoolListprint[0].pasrpS_LeftReason;
                    if ($scope.leftdate != null) {
                        $scope.leftreasondate = $scope.PreviousSchoolListprint[0].pasrpS_LeftDate;
                    }
                    else {
                        $scope.leftreasondate = "";
                    }

                    if ($scope.concessionscholer != null && $scope.concessionscholer != "" && $scope.concessionscholer != 'No' && $scope.concessionscholer != 'NAN' && $scope.concessionscholer != 'NA') {
                        $scope.concessiondate = $scope.PreviousSchoolListprint[0].pasrpS_ConcOrScholarshipDate
                    }
                    else {
                        $scope.concessiondate = "---";
                    }

                    $scope.obj.fatherimg = promise.studentReg_DTObj[0].pasR_FatherPhoto;
                    $scope.obj.motherimg = promise.studentReg_DTObj[0].pasR_MotherPhoto;

                }
                $('#blahfnewj').attr('src', promise.studentReg_DTObj[0].pasR_JointPhoto);
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahfnew1').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahmnew1').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $('#blahnewaa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                //var e1 = angular.element(document.getElementById("test"));
                //$compile(e1.html(promise.htmldata))(($scope));

                $('#blahnew').attr('src', $scope.studentphoto);
                $('#blahfnewj').attr('src', promise.studentReg_DTObj[0].pasR_JointPhoto);
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahfnew1').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahmnew1').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $('#blahnewaa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);


                //var e1 = angular.element(document.getElementById("test"));
                //$compile(e1.html(promise.htmldata))(($scope));
                $('#myModal1').modal('show');

            });
           
        }



        $scope.STJAMES = function () {

            var innerContents = document.getElementById("STJAMES").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/STJAMES/BGHSAPPPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.SSAPP = function () {

            var innerContents = document.getElementById("SSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Snehasagar/Snehasagar.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.BBSAPP = function () {

            var innerContents = document.getElementById("BBSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.BGHSAPP = function () {

            var innerContents = document.getElementById("BGHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPPPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPP.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPP.css"/>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.CBSC = function () {
            var innerContents1 = document.getElementById("CBSC").innerHTML;
            var popupWindow = window.open('');
            popupWindow.document.open();
            popupWindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents1 + '</html>');
            popupWindow.document.close();
        }
        $scope.CBSCXI = function () {
            var innerContents2 = document.getElementById("CBSCXI").innerHTML;
            var popupWindow = window.open('');
            popupWindow.document.open();
            popupWindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents2 + '</html>');
            popupWindow.document.close();
        }

        $scope.IDEAL = function () {

            var innerContents = document.getElementById("IDEAL").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPPPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.HHSAPP = function () {

            var innerContents = document.getElementById("HHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.HHSHAPP = function () {

            var innerContents = document.getElementById("HHSHAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/HutchingsHigher.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.JSHSAPP = function () {

            var innerContents = document.getElementById("JSHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.NDSJRAPP = function () {
            var innerContents = document.getElementById("NDSJRAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/NDSJRPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.BBHSAPP = function () {

            var innerContents = document.getElementById("BBHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.VIKASAAPP = function () {

            var innerContents = document.getElementById("VIKASAAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.BCEHSAPP = function () {

            var innerContents = document.getElementById("BCOESAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPP.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPPReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.BISAPP = function () {

            var innerContents = document.getElementById("BISAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPP.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPPReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
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
                    if ($scope.pages[i].pasr_id == data.pasr_id) {
                        studentreg = $scope.pages[i].pasR_RegistrationNo;
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
                    if ($scope.pages[i].pasr_id == idd) {
                        studentreg = $scope.pages[i].pasR_RegistrationNo;
                    }
                }
            }

            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;
            $('#preview').attr('src', data);
        }


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

            //if ($scope.pages.length > 0) {
            //    for (var i = 0; i < $scope.pages.length; i++) {
            //        if ($scope.pages[i].pasr_id == data.pasr_id) {
            //            studentreg = $scope.pages[i].pasR_RegistrationNo;
            //        }
            //    }
            //}

            //$scope.imagedownload = data.document_Pathtemp;
            //imagedownload = data.document_Pathtemp;
            //docname = data.amsmD_DocumentName;
            //ftype = data.filetype;
            //$http.get(imagedownload, {
            //    responseType: "arraybuffer"
            //})
            //    .success(function (data) {
            //        var anchor = angular.element('<a/>');
            //        var blob = new Blob([data]);
            //        anchor.attr({
            //            href: window.URL.createObjectURL(blob),
            //            target: '_blank',
            //            download: studentreg + '-' + docname + '.' + ftype
            //        })[0].click();
            //    })
        }

    }
})();

