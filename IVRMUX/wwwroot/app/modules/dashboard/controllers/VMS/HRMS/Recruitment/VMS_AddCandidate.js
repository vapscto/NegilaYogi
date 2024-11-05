(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsAddCandidateController', vmsAddCandidateController);

    //vmsAddCandidateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', '$q', 'FormSubmitter'];
    //function vmsAddCandidateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, $q, FormSubmitter) {
    vmsAddCandidateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function vmsAddCandidateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {

        $scope.PaymentMode = false;
        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.hrcD_Resume = "";

        $scope.Qualification = true;
        $scope.Experiance = true;
        $scope.Family = true;
        $scope.Language = true;

        $scope.paginate2 = "paginate2";
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        var searchObject = $location.search();
        //*****MAXMINAGE****
        //$scope.classdesable = true;
        $scope.classdesable = false;
        // alert(searchObject.status);
        if (searchObject.status == "failure" || searchObject.status == "TXN_FAILURE") {
            swal("Payment Unsuccessful");
            //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "success" || searchObject.status == "TXN_SUCCESS") {
            //swal("Payment successful and Please submit the filled application to the School office");
            swal("Payment Successful..!!");
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!', 'Try again after some time');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }

        $scope.Back = function () {
            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;
            $state.reload();
        }

        $scope.clearAll = function () {
            $state.reload();
            $scope.scroll();
        }

        $scope.paynowbefore = function (hrcD_Id) {

            angular.forEach($scope.candidateslist, function (item) {
                if (item.hrcD_Id == hrcD_Id) {
                    $scope.reg.PASR_FirstName = item.hrcD_FullName;
                    $scope.reg.PASR_emailId = item.hrcD_EmailId;
                    $scope.reg.PASR_MobileNo = item.hrcD_MobileNo;
                    $scope.hrcD_Id = hrcD_Id;
                }
            })
            $scope.PaymentMode = true;
            $scope.Qualification = false;
            $scope.Experiance = false;
            $scope.Family = false;
            $scope.Language = false;

            //if ($scope.myTabIndex == 1) {
            $scope.myTabIndex = 5;
            //}
            swal.close();
            showConfirmButton: false
            $scope.scroll();
        }



        $scope.pamentsave = function (hrcD_Id) {
            // $scope.submitted = true;
            $scope.paynowbefore(hrcD_Id);
            var data = {
                "HRCD_Id": hrcD_Id,
                "onlinepaygteway": 'RAZORPAY'
            }
            apiService.create("AddCandidateVMS/paynow", data).then(function (promise) {
                //if ($scope.result === 1) {
                if (promise.paydet != null) {
                    $scope.txnamt = Number(promise.paydet[0].fmA_Amount) * 100;
                    $scope.SaltKey = promise.paydet[0].ivrmoP_MERCHANT_KEY;
                    $scope.orderid = promise.paydet[0].trans_id;

                    $scope.institutioname = promise.paydet[0].institutioname;
                    $scope.institulogo = promise.paydet[0].institulogo;

                    $scope.stuname = promise.paydet[0].firstname;
                    $scope.stuemailid = promise.paydet[0].email;
                    $scope.stuaddress = promise.paydet[0].stuaddress;
                    $scope.stumobileno = promise.paydet[0].mobile;
                    $scope.stuadmno = promise.paydet[0].pasR_RegistrationNo;
                    $scope.splitpayinfor = promise.paydet[0].splitpayinformation;

                    $scope.mI_Id = promise.mI_Id;
                    $scope.asmaY_Id = promise.asmaY_Id;
                    $scope.amst_Id = promise.paydet[0].pasR_ID;



                    swal.close();
                    showConfirmButton: false
                }
                else {
                    swal('Registered Successfully,But Payment gateway details are not mapped to institute', 'Contact Administrator..!!');
                    $state.reload();
                }
                //}
                //else {
                //    swal("Kindly visit school to pay the registration fees before the last date");
                //    $state.reload();
                //}
            });
        };


        $scope.Candidate = {};
        $scope.Candidate.hrcD_Id = 0;
        $scope.Candidate.mI_Id = 0;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            $scope.taskDate = new Date($scope.today);
            var pageid = 2;
            apiService.getURI("AddCandidateVMS/getalldetails", pageid).then(function (promise) {
                if (promise.masterPosTypeList !== null && promise.masterPosTypeList.length > 0) {
                    $scope.masterPosTypeList = promise.masterPosTypeList;
                }

                if (promise.vmsmrfList !== null && promise.vmsmrfList.length > 0) {
                    $scope.candidateslist = promise.vmsmrfList;
                }

                if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                    $scope.masterQualification = promise.masterQualification;
                }

                if (promise.masterGender !== null && promise.masterGender.length > 0) {
                    $scope.masterGender = promise.masterGender;
                }

                if (promise.masterjob !== null && promise.masterjob.length > 0) {
                    $scope.masterjob = promise.masterjob;
                }

                if (promise.mastercountry !== null && promise.mastercountry.length > 0) {
                    $scope.mastercountry = promise.mastercountry;
                }

                if (promise.masterReligion !== null && promise.masterReligion.length > 0) {
                    $scope.masterReligion = promise.masterReligion;
                }

                if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                    $scope.masterCaste = promise.masterCaste;
                }

                if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                    $scope.mastermaritalstatus = promise.mastermaritalstatus;
                }
            });

        };

        $scope.EditData = function (jobid) {
            var pageid = jobid;
            apiService.getURI("CandidateListVMS/editRecord", pageid).
                then(function (promise) {

                    $scope.qualificationDetails = [{ id: 'qualification' }];
                    $scope.experienceDetails = [{ id: 'experience' }];
                    $scope.familyDetails = [{ id: 'family' }];
                    $scope.languageDetails = [{ id: 'language' }];
                    $scope.myTabIndex = 0;
                    $('#blah').removeAttr('src');

                    $scope.mrfCand = promise.vmsEditValue[0];
                    $scope.Candidate = promise.vmsEditValue[0];

                    if (promise.qualificationlist != null && promise.qualificationlist.length > 0) {
                        $scope.qualificationDetails = promise.qualificationlist;
                    }

                    if (promise.experiencelist != null && promise.experiencelist.length > 0) {
                        $scope.experienceDetails = promise.experiencelist;

                        $scope.EmployeeExperience = 1;
                        angular.forEach(promise.experiencelist, function (value, key) {

                            if (value.hrcexP_From != null) {
                                $scope.experienceDetails[key].hrcexP_From = new Date(value.hrcexP_From);
                            }
                            else {
                                $scope.experienceDetails[key].hrcexP_From = null;
                            }

                            if (value.hrcexP_To != null) {
                                $scope.experienceDetails[key].hrcexP_To = new Date(value.hrcexP_To);
                            }
                            else {
                                $scope.experienceDetails[key].hrcexP_To = null;
                            }
                        });
                    }
                    else { $scope.EmployeeExperience = 0; }

                    if (promise.familylist != null && promise.familylist.length > 0) {
                        $scope.familyDetails = promise.familylist;
                    }

                    if (promise.languagelist != null && promise.languagelist.length > 0) {
                        $scope.languageDetails = promise.languagelist;
                    }

                    if (promise.vmsEditValue[0].hrcD_DOB !== null) {
                        $scope.mrfCand.hrcD_DOB = new Date(promise.vmsEditValue[0].hrcD_DOB);
                    }
                    else {
                        $scope.mrfCand.hrcD_DOB = null;
                    }

                    if (promise.vmsEditValue[0].hrcD_AppDate !== null) {
                        $scope.mrfCand.hrcD_AppDate = new Date(promise.vmsEditValue[0].hrcD_AppDate);
                        $scope.taskDate = new Date(promise.vmsEditValue[0].hrcD_AppDate);
                    }
                    else {
                        $scope.mrfCand.hrcD_AppDate = null;
                    }

                    if (promise.vmsEditValue[0].hrcD_InterviewDate !== null) {
                        $scope.mrfCand.hrcD_InterviewDate = new Date(promise.vmsEditValue[0].hrcD_InterviewDate);
                    }
                    else {
                        $scope.mrfCand.hrcD_InterviewDate = null;
                    }

                    if (promise.vmsEditValue[0].hrcD_Photo != null && promise.vmsEditValue[0].hrcD_Photo != "") {
                        $('#blah').attr('src', promise.vmsEditValue[0].hrcD_Photo);
                    }

                    $scope.Editable = true;
                    $scope.Qualification = false;
                    $scope.Experiance = false;
                    $scope.Family = false;
                    $scope.Language = false;
                    //$state.go('app.vmsaddjob');
                });
        };

        // clear form data
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.getaddress = function () {
            if ($scope.mrfCand.copyaddress == true) {
                $scope.mrfCand.hrcD_AddressPermanent = $scope.mrfCand.hrcD_AddressLocal;
                $scope.mrfCand.hrcD_AddressPermanent2 = $scope.mrfCand.hrcD_AddressLocal2;
                $scope.mrfCand.hrcD_AddPermanentPlace = $scope.mrfCand.hrcD_AddLocalPlace;
                $scope.mrfCand.hrcD_AddPermanentPIN = $scope.mrfCand.hrcD_AddLocalPIN;
            }
        };

        $scope.submitted = false;
        $scope.submitted3 = false;
        $scope.submitted4 = false;
        $scope.submitted2 = false;
        $scope.submitted5 = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2 || field.$dirty;
        };
        $scope.interacted3 = function (field) {
            return $scope.submitted3 || field.$dirty;
        };
        $scope.interacted4 = function (field) {
            return $scope.submitted4 || field.$dirty;
        };
        $scope.interacted5 = function (field) {
            return $scope.submitted5 || field.$dirty;
        };

        $scope.qualificationDetails = [{ id: 'qualification' }];
        $scope.addNewQualification = function () {
            var newItemNo = $scope.qualificationDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.qualificationDetails.push({ 'id': 'qualification' + newItemNo });
            }
        };

        $scope.removeNewQualification = function (index, data) {
            var newItemNo = $scope.qualificationDetails.length - 1;
            $scope.qualificationDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };


        $scope.Qualification_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateQualificationDetails = function () {
            if ($scope.myForm3.$valid) {
                var data = {
                    Employeedto: $scope.Candidate,
                    QualificationsDTO: $scope.qualificationDetails
                };
                apiService.create("AddCandidateVMS/addQualification", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Experiance = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Experiance = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted3 = true;
            }
        };

        $scope.clear_third_tab = function (data) {
            $scope.qualificationDetails = [{ id: 'qualification' }];
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
        };

        $scope.experienceDetails = [{ id: 'experience' }];
        $scope.addNewExperience = function () {
            var newItemNo = $scope.experienceDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.experienceDetails.push({ 'id': 'experience' + newItemNo });
                $scope.experienceDetails[newItemNo - 1].exitDateDis = true;
            }
        };

        $scope.removeNewExperience = function (index, data) {
            var newItemNo = $scope.experienceDetails.length - 1;
            $scope.experienceDetails.splice(index, 1);
            if (data.hrmeE_Id > 0) {
                $scope.DeleteExperienceData(data);
            }
        };

        $scope.EmployeeExperience = 0;

        $scope.validateExperiencePrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateExperiencedetails = function () {
            if ($scope.myForm4.$valid) {
                var data = {
                    Employeedto: $scope.Candidate,
                    ExperienceDTO: $scope.experienceDetails
                };
                apiService.create("AddCandidateVMS/addexperience", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Family = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Family = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted4 = true;
            }
        };

        $scope.clear_fourth_tab = function () {
            $scope.experienceDetails = [{ id: 'experience' }];
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
        };

        //FAMILY DETAILS
        $scope.familyDetails = [{ id: 'family' }];
        $scope.addNewfamily = function () {
            var newItemNo = $scope.familyDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.familyDetails.push({ 'id': 'family' + newItemNo });
            }
        };

        $scope.removeNewfamily = function (index, data) {
            var newItemNo = $scope.familyDetails.length - 1;
            $scope.familyDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };

        $scope.family_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validatefamilyDetails = function () {
            if ($scope.myForm2.$valid) {
                var data = {
                    Employeedto: $scope.Candidate,
                    FamilyDTO: $scope.familyDetails
                };
                apiService.create("AddCandidateVMS/addfamily", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Language = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Language = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.clear_second_tab = function (data) {
            $scope.familyDetails = [{ id: 'family' }];
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };
        //FAMILY DETAILS

        //LANGUAGE DETAILS
        $scope.languageDetails = [{ id: 'language' }];
        $scope.addNewLanguage = function () {
            var newItemNo = $scope.languageDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.languageDetails.push({ 'id': 'language' + newItemNo });
            }
        };

        $scope.removeNewLanguage = function (index, data) {
            var newItemNo = $scope.languageDetails.length - 1;
            $scope.languageDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };

        $scope.language_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validatelanguageDetails = function () {
            if ($scope.myForm5.$valid) {
                var data = {
                    Employeedto: $scope.Candidate,
                    LanguagesDTO: $scope.languageDetails
                };
                apiService.create("AddCandidateVMS/addlanguage", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                $scope.Qualification = true;
                                $scope.Experiance = true;
                                $scope.Family = true;
                                $scope.Language = true;
                                swal("Record Saved Successfully..");
                                $state.reload;
                            }
                            else if (promise.retrunMsg === "Update") {
                                $scope.Qualification = true;
                                $scope.Experiance = true;
                                $scope.Family = true;
                                $scope.Language = true;
                                swal("Record Updated Successfully..");
                                $state.reload;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted5 = true;
            }
        };

        $scope.clear_fifth_tab = function (data) {
            $scope.languageDetails = [{ id: 'language' }];
            $scope.submitted5 = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        };
        //LANGUAGE DETAILS

        $scope.savejob = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.mrfCand.hrcD_RecruitmentStatus = "New";
                var data = $scope.mrfCand;

                apiService.create("AddCandidateVMS/", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Candidate.hrcD_Id = promise.hrcD_Id;
                                $scope.Candidate.mI_Id = promise.mI_Id;
                                $scope.Qualification = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Candidate.hrcD_Id = promise.hrcD_Id;
                                $scope.Candidate.mI_Id = promise.mI_Id;
                                $scope.Qualification = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                            //if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {
                            //    $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
                            //}
                            //$scope.cancel();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.mrfCand = {};

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf")
                    && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocument(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocument(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        data.hrcD_Resume = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        //COMMENT
        $scope.uploadmateraldocuments1 = [];
        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;
            $scope.filename = input.files[0].name;
            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Addcandidate", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.hrcD_Resume = d;
                    data.cfilename = $scope.filename;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //COMMENT

        //upload Employee Profile pic
        $scope.UploadEmployeeProfilePic = [];
        $scope.uploadEmployeeProfilePic = function (input, document) {
            $scope.UploadEmployeeProfilePic = input.files;
            $('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofile(document);
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    angular.element("input[type='file']").val(null);
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    angular.element("input[type='file']").val(null);
                    return;
                }
            }
        };

        function UploadEmployeeprofile(data) {

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
                        $scope.mrfCand.hrcD_Photo = d;
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
    }

})();