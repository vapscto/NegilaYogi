


(function () {
    'use strict';
    angular
        .module('app')
        .controller('writtenController', WrittenTestMarksEntryController)

    WrittenTestMarksEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$compile']
    function WrittenTestMarksEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $compile) {


        $scope.hidestudents = false;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.page1 = "pag1";
        $scope.page2 = "pag2";
        $scope.reverse1 = true;
        $scope.reverse2 = true;
        $scope.hidesub = false;
        $scope.angularData1 = {
            'nameList1': []
        };
        $scope.vals1 = [];

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.StudentName, function (itm) {
                itm.checked = toggleStatus;

            });
        }

        $scope.optionToggled = function () {
            $scope.all = $scope.StudentName.every(function (itm) { return itm.checked; });
        }
        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.angularData = {
            'nameList': []
        };

        $scope.vals = [];
        $scope.fullname = "";
        $scope.BindGridData = {};
        $scope.stuRecord = {};
        $scope.writtenMarks = [];


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.checkmarks = function (total, obtain) {
            if (obtain > total) {
                swal("Obtain marks must be less than or equal " + total);
            }
        };

        //$scope.CheckedStudentName = function (data) {
        //    $scope.checkboxchcked = [];
        //    if ($scope.checkboxchcked.indexOf(data) === -1) {
        //        $scope.checkboxchcked.push(data);
        //    }
        //    else {
        //        $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
        //    }
        //}

        $scope.BindData = function () {

            $scope.stu_List = false;

            var pageid = 2;

            apiService.getURI("WrittenTestMarksEntry/Getdetails", pageid).
                then(function (promise) {

                    if (promise.chq_config == false) {
                        swal("PLEASE CHECK CONFIGURATION  !");
                    }
                    else {
                        if (promise.subjectNames.length > 0) {
                            $scope.SubjectName = promise.subjectNames;
                            $scope.hidesub = false;
                        }
                        else {
                            $scope.hidesub = true;
                            swal("No subjects found!!");
                        }

                        if (promise.writtenTestSchedule.length > 0) {
                            $scope.ScheduleName = promise.writtenTestSchedule;
                        }
                        else {
                            swal("No schedule's found!!");
                        }

                        //added by suryan
                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < promise.studentDetails.length; i++) {
                            if (promise.studentDetails[i].pasR_FirstName != '') {
                                if (promise.studentDetails[i].pasR_MiddleName !== null) {
                                    if (promise.studentDetails[i].pasR_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName + promise.studentDetails[i].pasR_MiddleName + promise.studentDetails[i].pasR_LastName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName + promise.studentDetails[i].pasR_MiddleName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                }
                            }
                        }
                        $scope.StudentName = $scope.albumNameArray1;
                        //$scope.StudentName = promise.studentDetails;
                        //for (var i = 0; i < promise.studentDetails.length; i++) {
                        //    var name = promise.studentDetails[i].pasR_FirstName;
                        //    if (promise.studentDetails[i].pasR_MiddleName != null) {
                        //        name += " " + promise.studentDetails[i].pasR_MiddleName;
                        //    }
                        //    if (promise.studentDetails[i].pasR_LastName != null) {
                        //        name += " " + promise.studentDetails[i].pasR_LastName;
                        //    }
                        //    $scope.vals.push(name);
                        //}
                        //angular.forEach($scope.vals, function (v, k) {
                        //    $scope.angularData.nameList.push({
                        //        'fullname': v
                        //    });
                        //});
                        //var j = 0;
                        //angular.forEach($scope.StudentName, function (obj) {
                        //    //Using bracket notation
                        //    obj["fullname"] = $scope.angularData.nameList[j].fullname;
                        //    j++;
                        //});

                        if (promise.writtenTestScheduleAppFlag === 1) {
                            $scope.myValue = true;
                        }
                        else if (promise.WrittenTestScheduleAppFlag === 0) {
                            $scope.myValue = false;
                        }
                    }
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };

        $scope.submitted = false;
        $scope.BindGrid = function (details) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.checkboxchcked = [];
                for (var i = 0; i < details.length; i++) {
                    if (details[i].checked === true) {
                        $scope.checkboxchcked.push(details[i]);
                    }
                }
                $scope.stu_List = true;
                var StudentIDs = $scope.checkboxchcked;
                if ($scope.ismS_Id === "Select ALL" || $scope.ismS_Id === "") {
                    $scope.SelectedSubjectId = 0;
                }
                else {
                    $scope.SelectedSubjectId = $scope.ismS_Id;
                }
                var data = {
                    "SelectedStudentDetails": StudentIDs,
                    "ISMS_Id": $scope.SelectedSubjectId,
                    "PAWTS_ID": $scope.pawtS_Id
                };
                apiService.create("WrittenTestMarksEntry/GetWrittenTestMarks", data).
                    then(function (promise) {
                        $scope.hidestudents = true;
                        $scope.studentlist = [];
                        //$scope.studentlist = $scope.StudentName;
                        angular.forEach($scope.StudentName, function (opqr1) {
                            if (opqr1.checked == true) {
                                $scope.studentlist.push(opqr1);
                            }
                        });
                        if ($scope.studentlist.length > 0) {
                            for (var j = 0; j < $scope.studentlist.length; j++) {
                                for (var l = 0; l < promise.admissioncatdrpall.length; l++) {
                                    if ($scope.studentlist[j].asmcL_Id == promise.admissioncatdrpall[l].asmcL_Id) {
                                        $scope.studentlist[j].classname = promise.admissioncatdrpall[l].asmcL_ClassName;
                                    }
                                }
                            }
                        }
                        $scope.wirettenTestStudentMarks = promise.allInOne1;
                        if (promise.registrationList.length > 0) {
                            for (var i = 0; i < $scope.wirettenTestStudentMarks.length; i++) {
                                for (var j = 0; j < promise.registrationList.length; j++) {
                                    if ($scope.wirettenTestStudentMarks[i].pasr_id == promise.registrationList[j].pasR_Id) {
                                        $scope.wirettenTestStudentMarks[i].regno = promise.registrationList[j].pasR_RegistrationNo;
                                    }
                                }
                            }
                        }
                        if (promise.registrationList.length > 0) {
                            for (var i = 0; i < $scope.wirettenTestStudentMarks.length; i++) {
                                for (var j = 0; j < promise.registrationList.length; j++) {
                                    if ($scope.wirettenTestStudentMarks[i].pasr_id == promise.registrationList[j].pasR_Id) {
                                        if (promise.admissioncatdrpall.length > 0) {
                                            for (var l = 0; l < promise.admissioncatdrpall.length; l++) {
                                                if (promise.registrationList[j].asmcL_Id == promise.admissioncatdrpall[l].asmcL_Id) {
                                                    $scope.wirettenTestStudentMarks[i].classname = promise.admissioncatdrpall[l].asmcL_ClassName;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        $scope.studentprintlist = promise.registrationList;
                        //$scope.writtenMarks.push.apply($scope.writtenMarks, promise);
                        //console.log($scope.writtenMarks);
                        //var myEl = angular.element(document.querySelector('#id' + promise[0].pasR_Id));
                        //myEl.val(promise[0].obtMarks);
                    });
            }
            else {
                $scope.stu_List = false;
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.StudentName.some(function (StuName) {
                return StuName.checked;
            });
        };

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
            $scope.firstsibling = "";
            $scope.firstsiblingclass = "";
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
        };

        //for print schedule
        $scope.printSchedulelist = function (printSchedule_data) {

            if ($scope.studentprintlist !== null && $scope.studentprintlist.length > 0) {

                $scope.dateofschedule = $scope.studentprintlist[0].ScheduleDate;
                $scope.timeofschedule = $scope.studentprintlist[0].ScheduleTime;
                var innerContents = document.getElementById("print_data_srkvs").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/RAMAKRISHNA_VIDYASHALA/RegistrationReceiptPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        };
        $scope.showprintdata = function (pasR_Id) {
            $scope.cleartheapplication();
            apiService.getURI("StudentApplication/getprintdata", pasR_Id).then(function (promise) {
                $scope.serilano = promise.studentReg_DTObj[0].pasR_Id;
                $scope.pasr_id = promise.studentReg_DTObj[0].pasR_RegistrationNo;
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
                var doob = promise.studentReg_DTObj[0].pasR_DOB;
                var doobyr = doob.substring(0, 4);
                var doobmnth = doob.substring(5, 7);
                var doobdays = doob.substring(8, 10);
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
                $scope.PASR_FatherAliveFlag = promise.studentReg_DTObj[0].pasR_FatherAliveFlag == 0 ? "Not Alive" : "Alive";
                $scope.PASR_FatherName = promise.studentReg_DTObj[0].pasR_FatherName == null ? "" : promise.studentReg_DTObj[0].pasR_FatherName;
                $scope.PASR_FatherAadharNo = promise.studentReg_DTObj[0].pasR_FatherAadharNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherAadharNo;
                $scope.PASR_FatherSurname = promise.studentReg_DTObj[0].pasR_FatherSurname == null ? "" : promise.studentReg_DTObj[0].pasR_FatherSurname;
                $scope.PASR_FatherEducation = promise.studentReg_DTObj[0].pasR_FatherEducation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherEducation;
                $scope.PASR_FatherOccupation = promise.studentReg_DTObj[0].pasR_FatherOccupation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherOccupation;
                $scope.PASR_FatherDesignation = promise.studentReg_DTObj[0].pasR_FatherDesignation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherDesignation;
                $scope.PASR_FatherIncome = promise.studentReg_DTObj[0].pasR_FatherIncome;
                $scope.PASR_FatherMobleNo = promise.studentReg_DTObj[0].pasR_FatherMobleNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherMobleNo;
                $scope.PASR_FatheremailId = promise.studentReg_DTObj[0].pasR_FatheremailId == null ? "" : promise.studentReg_DTObj[0].pasR_FatheremailId;
                $scope.syllabussname = promise.sylabusss.length > 0 ? promise.sylabusss[0].imC_CasteName : "";
                $scope.PASR_FatherReligion = promise.fatherreligion.length > 0 ? promise.fatherreligion[0].imC_CasteName : "";
                $scope.PASR_FatherCaste = promise.fathercaste.length > 0 ? promise.fathercaste[0].imC_CasteName : "";
                $scope.overgae = promise.studentReg_DTObj[0].pasR_OverAge;
                $scope.undergae = promise.studentReg_DTObj[0].pasR_UnderAge;
                $scope.fatherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Fathersubcaste == null ? "" : promise.studentReg_DTObj[0].pasR_Fathersubcaste;
                $scope.SubCaste_Id = promise.studentReg_DTObj[0].pasR_Subcaste == null ? "" : promise.studentReg_DTObj[0].pasR_Subcaste;
                $scope.motherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Mothersubcatse == null ? "" : promise.studentReg_DTObj[0].pasR_Mothersubcatse;
                $scope.PASR_FatherTribe = promise.studentReg_DTObj[0].pasR_FatherTribe;
                $scope.PASR_Tribe = promise.studentReg_DTObj[0].pasR_Tribe;
                $scope.PASR_MotherReligion = promise.motherreligion.length > 0 ? promise.motherreligion[0].imC_CasteName : "";
                $scope.PASR_MotherCaste = promise.mothercaste.length > 0 ? promise.mothercaste[0].imC_CasteName : "";
                $scope.PASR_MotherTribe = promise.studentReg_DTObj[0].pasR_MotherTribe;
                $scope.PASR_FirstLanguage = promise.studentReg_DTObj[0].pasR_FirstLanguage == null ? "" : promise.studentReg_DTObj[0].pasR_FirstLanguage;
                $scope.PASR_Thirdlanguage = promise.studentReg_DTObj[0].pasR_Thirdlanguage == null ? "" : promise.studentReg_DTObj[0].pasR_Thirdlanguage;
                $scope.PASR_SecondLanguage = promise.studentReg_DTObj[0].pasR_SecondLanguage == null ? "" : promise.studentReg_DTObj[0].pasR_SecondLanguage;
                $scope.PASR_MotherAliveFlag = promise.studentReg_DTObj[0].pasR_MotherAliveFlag == 0 ? "Not Alive" : "Alive";
                $scope.PASR_MotherName = promise.studentReg_DTObj[0].pasR_MotherName == null ? "" : promise.studentReg_DTObj[0].pasR_MotherName;
                $scope.PASR_MotherAadharNo = promise.studentReg_DTObj[0].pasR_MotherAadharNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherAadharNo;
                $scope.PASR_MotherSurname = promise.studentReg_DTObj[0].pasR_MotherSurname == null ? "" : promise.studentReg_DTObj[0].pasR_MotherSurname;
                $scope.PASR_MotherEducation = promise.studentReg_DTObj[0].pasR_MotherEducation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherEducation;
                $scope.PASR_MotherOccupation = promise.studentReg_DTObj[0].pasR_MotherOccupation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherOccupation;
                $scope.PASR_MotherDesignation = promise.studentReg_DTObj[0].pasR_MotherDesignation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherDesignation;
                $scope.PASR_MotherIncome = promise.studentReg_DTObj[0].pasR_MotherIncome;
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
                $scope.PASR_GymReqdFlag = promise.studentReg_DTObj[0].pasR_GymReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_ECSFlag = promise.studentReg_DTObj[0].pasR_ECSFlag == 1 ? "Yes" : "No";
                $scope.PASR_PaymentFlag = promise.studentReg_DTObj[0].pasR_PaymentFlag == 1 ? "Yes" : "No";
                $scope.PASR_AmountPaid = promise.studentReg_DTObj[0].pasR_AmountPaid;
                $scope.PASR_PaymentType = promise.studentReg_DTObj[0].pasR_PaymentType == null ? "" : promise.studentReg_DTObj[0].pasR_PaymentType;
                $scope.PASR_PaymentDate = promise.studentReg_DTObj[0].pasR_PaymentDate == null ? "" : promise.studentReg_DTObj[0].pasR_PaymentDate;
                $scope.PASR_ReceiptNo = promise.studentReg_DTObj[0].pasR_ReceiptNo == null ? "" : promise.studentReg_DTObj[0].pasR_ReceiptNo;
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
                $scope.PASR_AltContactNo = promise.studentReg_DTObj[0].pasR_AltContactNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_AltContactNo;
                $scope.PASR_AltContactEmail = promise.studentReg_DTObj[0].pasR_AltContactEmail == null ? "" : promise.studentReg_DTObj[0].pasR_AltContactEmail;
                $('#blahnew').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $scope.studentphoto = promise.studentReg_DTObj[0].pasR_Student_Pic_Path;
                $scope.PASR_Noofbrothers = promise.studentReg_DTObj[0].pasR_Noofbrothers == null ? "" : promise.studentReg_DTObj[0].pasR_Noofbrothers;
                $scope.PASR_Noofsisters = promise.studentReg_DTObj[0].pasR_Noofsisters == null ? "" : promise.studentReg_DTObj[0].pasR_Noofsisters;
                $scope.PASR_NoOfElderBrothers = promise.studentReg_DTObj[0].pasR_NoOfElderBrothers == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfElderBrothers;
                $scope.PASR_NoOfYoungerBrothers = promise.studentReg_DTObj[0].pasR_NoOfYoungerBrothers == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfYoungerBrothers;
                $scope.PASR_NoOfElderSisters = promise.studentReg_DTObj[0].pasR_NoOfElderSisters == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfElderSisters;
                $scope.PASR_NoOfYoungerSisters = promise.studentReg_DTObj[0].pasR_NoOfYoungerSisters == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfYoungerSisters;
                $scope.PASR_lastclassperc = promise.studentReg_DTObj[0].pasR_lastclassperc == null ? "" : promise.studentReg_DTObj[0].pasR_lastclassperc;
                $scope.PASR_SibblingConcessionFlag = promise.studentReg_DTObj[0].pasR_SibblingConcessionFlag == 1 ? "Yes" : "No";
                $scope.PASR_ParentConcessionFlag = promise.studentReg_DTObj[0].pasR_ParentConcessionFlag == 1 ? "Yes" : "No";
                if (promise.studentGuardian_DTObj != null && promise.studentGuardian_DTObj != "") {
                    $scope.pasrG_Id = promise.studentGuardian_DTObj[0].pasrG_Id;
                    $scope.PASRG_GuardianName = promise.studentGuardian_DTObj[0].pasrG_GuardianName;
                    $scope.PASRG_GuardianAddress = promise.studentGuardian_DTObj[0].pasrG_GuardianAddress;
                    $scope.PASRG_GuardianRelation = promise.studentGuardian_DTObj[0].pasrG_GuardianRelation;
                    $scope.PASRG_emailid = promise.studentGuardian_DTObj[0].pasrG_emailid;
                    $scope.PASRG_GuardianPhoneNo = promise.studentGuardian_DTObj[0].pasrG_GuardianPhoneNo;
                    $scope.PASRG_Occupation = promise.studentGuardian_DTObj[0].pasrG_Occupation;
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
                }
                $scope.firstsibling = '';
                $scope.firstsiblingclass = '';
                $scope.secondsibling = '';
                $scope.secondsiblingclass = '';
                if (promise.studentSbling_DTObj.length > 0) {
                    $scope.siblingsprint = promise.studentSbling_DTObj;
                    if ($scope.siblingsprint.length == 1) {
                        $scope.firstsibling = $scope.siblingsprint[0].pasrS_SiblingsName;
                        $scope.firstsiblingclass = $scope.siblingsprint[0].pasrS_SiblingsClass;
                        $scope.firstsiblingsection = $scope.siblingsprint[0].pasrS_SiblingsSection;
                    }
                    if ($scope.siblingsprint.length == 2) {
                        $scope.firstsibling = $scope.siblingsprint[0].pasrS_SiblingsName;
                        $scope.firstsiblingclass = $scope.siblingsprint[0].pasrS_SiblingsClass;
                        $scope.firstsiblingsection = $scope.siblingsprint[0].pasrS_SiblingsSection;
                        $scope.secondsibling = $scope.siblingsprint[1].pasrS_SiblingsName;
                        $scope.secondsiblingclass = $scope.siblingsprint[1].pasrS_SiblingsClass;
                        $scope.secondsiblingsection = $scope.siblingsprint[1].pasrS_SiblingsSection;
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
                        $scope.HepatitisB = new Date(promise.studenthelthDTO[0].pashD_HepatitisB);
                        $scope.TyphoidFever = new Date(promise.studenthelthDTO[0].pashD_TyphoidFever);
                        $scope.Ailment = promise.studenthelthDTO[0].pashD_Ailment;
                        $scope.HealthProblem = promise.studenthelthDTO[0].pashD_HealthProblem;
                        $scope.CronicDesease = promise.studenthelthDTO[0].pashD_CronicDesease;
                        $scope.MEDetails = promise.studenthelthDTO[0].pashD_MEDetails;
                        $scope.MEContactNo = promise.studenthelthDTO[0].pashD_MEContactNo;
                        if (promise.vaccines != null) {
                            if (promise.vaccines.length > 0) {
                                $scope.vaccinesprint = promise.vaccines;
                            }
                        }
                    }
                }
                if (promise.studentDocuments_DTObj.length > 0) {
                    $scope.document = {};
                    $scope.documentList = promise.studentDocuments_DTObj;
                }
                if (promise.studentSubjects_DTObj != null && promise.studentSubjects_DTObj.length > 0) {
                    $scope.electivegrouplistprint = promise.electivegrouplist;
                    $scope.electivesubgrouplistprint = promise.electivesubgrouplist;
                    angular.forEach(promise.studentSubjects_DTObj, function (opqr) {
                        angular.forEach($scope.electivesubgrouplistprint, function (opqr1) {
                            if (opqr.emG_Id == opqr1.EMG_Id) {
                                opqr1.ismS_Id = opqr.ismS_Id;
                            }
                        });
                    });
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
                    });
                }
                if (promise.streamexamsprint != null && promise.streamexamsprint.length > 0) {
                    $scope.streamexamsprint = promise.streamexamsprint;
                    $scope.asmceid = promise.asmceId;
                }
                else {
                    $scope.streamexamsprint = [];
                }
                if (promise.studentPrevSch_DTObj.length > 0) {
                    $scope.PreviousSchoolList = promise.studentPrevSch_DTObj;
                    $scope.schoolname = $scope.PreviousSchoolList[0].pasrpS_PrvSchoolName;
                    $scope.schooladress = $scope.PreviousSchoolList[0].pasrpS_Address;
                    $scope.lastclass = $scope.PreviousSchoolList[0].pasrpS_PreviousClass;
                    $scope.lastsylaabus = $scope.PreviousSchoolList[0].pasrpS_Board;
                    $scope.percentage = $scope.PreviousSchoolList[0].pasrpS_PreviousPer;
                    $scope.leftdate = $scope.PreviousSchoolList[0].pasrpS_LeftYear;
                    $scope.concessionscholer = $scope.PreviousSchoolList[0].pasrpS_ConcOrScholarshipFlg;
                    $scope.leftreason = $scope.PreviousSchoolList[0].pasrpS_LeftReason;
                    if ($scope.leftdate != null) {
                        $scope.leftreasondate = $scope.PreviousSchoolList[0].pasrpS_LeftDate;
                    }
                    else {
                        $scope.leftreasondate = "";
                    }
                    if ($scope.concessionscholer != null && $scope.concessionscholer != "" && $scope.concessionscholer != 'No' && $scope.concessionscholer != 'NAN' && $scope.concessionscholer != 'NA') {
                        $scope.concessiondate = $scope.PreviousSchoolList[0].pasrpS_ConcOrScholarshipDate
                    }
                    else {
                        $scope.concessiondate = "---";
                    }
                }
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahfnew1').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahmnew1').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $('#blahnewaa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                var e1 = angular.element(document.getElementById("test"));
                $compile(e1.html(promise.htmldata))(($scope));
                $('#blahnew').attr('src', $scope.studentphoto);
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahfnew1').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahmnew1').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $('#blahnewaa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
            });
        };
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
        };
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
        };

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
        };
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
        };
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
        };
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
        };
        $scope.BCEHSAPP = function () {
            var innerContents = document.getElementById("BGHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BGHSAPP.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BGHSAPPReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
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
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.onSelectsubject = function (schelist, schid) {
            angular.forEach(schelist, function (obj) {
                if (obj.ismS_Id == schid) {
                    $scope.subjectname = obj.ismS_SubjectName;
                }
            });
        };
        $scope.onSelectGetStudent = function (objd, id) {
            $scope.submitted = false;
            if (id === undefined || id === "") {
                id = 0;
            }
            angular.forEach(objd, function (obj) {
                if (obj.pawtS_Id == id) {
                    $scope.schedulename = obj.pawtS_ScheduleName;
                }
            });
            $scope.hidestudents = false;
            $scope.all = false;
            var data = {
                "PAWTS_Id": id
            };
            apiService.create("WrittenTestMarksEntry/GetdetailsOnSchedule", data).then(function (promise) {
                if (promise.studentDetails.length > 0) {
                    $scope.StudentName = promise.studentDetails;
                    $scope.albumNameArray1 = [];
                    for (var i = 0; i < $scope.StudentName.length; i++) {
                        if ($scope.StudentName[i].pasR_FirstName != '') {
                            if ($scope.StudentName[i].pasR_MiddleName != null && $scope.StudentName[i].pasR_MiddleName != '' && $scope.StudentName[i].pasR_MiddleName != "") {
                                if ($scope.StudentName[i].pasR_LastName != null && $scope.StudentName[i].pasR_LastName != '' && $scope.StudentName[i].pasR_LastName != "") {
                                    $scope.albumNameArray1.push({ name: $scope.StudentName[i].pasR_FirstName + " " + $scope.StudentName[i].pasR_MiddleName + " " + $scope.StudentName[i].pasR_LastName, pasR_Id: $scope.StudentName[i].pasR_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: $scope.StudentName[i].pasR_FirstName + " " + $scope.StudentName[i].pasR_MiddleName, pasR_Id: $scope.StudentName[i].pasR_Id });
                                }
                            }
                            else {
                                if ($scope.StudentName[i].pasR_LastName != null && $scope.StudentName[i].pasR_LastName != '' && $scope.StudentName[i].pasR_LastName != " ") {
                                    $scope.albumNameArray1.push({ name: $scope.StudentName[i].pasR_FirstName + " " + $scope.StudentName[i].pasR_LastName, pasR_Id: $scope.StudentName[i].pasR_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: $scope.StudentName[i].pasR_FirstName, pasR_Id: $scope.StudentName[i].pasR_Id });
                                }
                            }

                            $scope.StudentName[i].name = $scope.albumNameArray1[i].name;
                        }
                    }
                }
                else {
                    $scope.pawtS_Id = null;
                    swal("No Records found!!");
                }
            });
            $scope.wirettenTestStudentMarks = [];
            $scope.checkboxchcked = [];
        };

        $scope.submitted1 = false;
        $scope.saveWrittenTestMarksEntrydata = function (wirettenTestStudentMarks) {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Submit Details ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var StudentIDs = $scope.checkboxchcked;
                            var data = {
                                "SelectedStudentMarksData": wirettenTestStudentMarks,
                                "WrittenTestScheduleAppFlag": $scope.myValue,
                                "PAWTS_Id": $scope.pawtS_Id
                            };
                            apiService.create("WrittenTestMarksEntry/", data).
                                then(function (promise) {
                                    $scope.$apply();
                                    $scope.PostDataResponse = data;
                                    $scope.BindData();
                                    swal("Record Saved Successfully");
                                });
                            $scope.BindData(2);
                            $state.reload();
                        }
                        else {
                            //$state.reload();
                        }
                    });
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.fullname).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };
    }
})();