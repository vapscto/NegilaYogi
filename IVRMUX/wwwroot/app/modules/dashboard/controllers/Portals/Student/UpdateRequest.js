//import { Promise } from "./C:/Users/vaps/AppData/Local/Microsoft/TypeScript/2.6/node_modules/@types/bluebird";

(function () {
    'use strict';
    angular
        .module('app')
        .controller('UpdateRequestController', UpdateRequestController)

    UpdateRequestController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', '$window', 'superCache', '$compile']
    function UpdateRequestController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, $window, superCache, $compile) {

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.showgrid = false;
        $scope.searchValue = '';
        var details = "";
        $scope.parent = "S";
        $scope.imgname = logopath;
        $scope.uploadDocs_img = [];
        $scope.images_temp = [];
        var id = 1;
        //-----student name selection change
        $scope.onchange = function (studentlst) {

            var studid = studentlst.amsT_ID;

            $scope.acc = true;
            $scope.accyer = $scope.ASMAY_Id

            var data = {
                "AMST_ID": studid
            }
            apiService.create("UpdateRequest/getstudata", data).
                then(function (promise) {

                    $scope.cardData = promise.cardData;
                    $scope.rolename = promise.roleName;
                    $scope.Griddata = promise.griddata;
                    if (promise.griddata.length != 0) {
                        $scope.showgrid = true;
                    }
                    details = promise.griddata;
                    $scope.countrydrop = promise.countryDrpDown;
                    $scope.statedrop = promise.stateDrpDown;
                    $scope.statedropp = promise.stateDrpDown;
                    $scope.existsornot = promise.existsornot;
                    if ($scope.parent == 'S') {
                        $scope.NameoftheCandidate = $scope.cardData[0].studentname;
                        $scope.EmailidforCandidate = $scope.cardData[0].AMST_emailId;
                        $scope.Mobilenumber = $scope.cardData[0].AMST_MobileNo;
                        $scope.Bloodgroup = $scope.cardData[0].AMST_BloodGroup;
                        $scope.imageF = $scope.cardData[0].AMST_Photoname;
                        $('#blahF').attr('src', $scope.cardData[0].AMST_Photoname);
                        //  $('#blahF').attr('src', $scope.cardData[0].ANST_FatherPhoto);
                        //  $('#blahF').attr('src', $scope.cardData[0].ANST_MotherPhoto);

                    }
                    if ($scope.parent == 'F') {
                        $scope.NameoftheCandidate = $scope.cardData[0].fatherName;
                        $scope.Mobilenumber = $scope.cardData[0].AMST_FatherMobleNo;
                        $scope.EmailidforCandidate = $scope.cardData[0].AMST_FatheremailId;
                        $scope.imageF = $scope.cardData[0].ANST_FatherPhoto;
                        $('#blahF').attr('src', $scope.cardData[0].ANST_FatherPhoto);
                        $('#blahF').attr('src', $scope.cardData[0].ANST_FatherPhoto);
                        $('#blahF').attr('src', $scope.cardData[0].ANST_MotherPhoto);

                    }
                    if ($scope.parent == 'M') {
                        $scope.NameoftheCandidate = $scope.cardData[0].mothername;
                        $scope.Mobilenumber = $scope.cardData[0].AMST_MotherMobileNo;
                        $scope.EmailidforCandidate = $scope.cardData[0].AMST_MotherEmailId;
                        $scope.imageF = $scope.cardData[0].ANST_MotherPhoto;
                        $('#blahF').attr('src', $scope.cardData[0].ANST_MotherPhoto);
                        $('#blahF').attr('src', $scope.cardData[0].ANST_FatherPhoto);
                        $('#blahF').attr('src', $scope.cardData[0].ANST_MotherPhoto);

                    }
                    $scope.Bloodgroup = $scope.cardData[0].AMST_BloodGroup,
                        $scope.Perstreett = $scope.cardData[0].AMST_PerStreet,
                        $scope.Percity = $scope.cardData[0].AMST_PerCity,
                        $scope.PerArea = $scope.cardData[0].AMST_PerArea,
                        $scope.PerPincode = $scope.cardData[0].AMST_PerPincode,
                        $scope.PerCountry = $scope.cardData[0].AMST_PerCountry,
                        $scope.PerCountryy = $scope.cardData[0].AMST_PerCountry,
                        $scope.PerStatee = $scope.cardData[0].AMST_PerState,
                        $scope.PerState = $scope.cardData[0].AMST_PerState,
                        $scope.Resstreet = $scope.cardData[0].AMST_ConStreet,
                        $scope.Rescity = $scope.cardData[0].AMST_ConCity,
                        $scope.ResArea = $scope.cardData[0].AMST_ConArea,
                        $scope.ResPincode = $scope.cardData[0].AMST_ConPincode,
                        $scope.Rescountryy = $scope.cardData[0].AMST_ConCountry,
                        $scope.Rescountry = $scope.cardData[0].AMST_ConCountry,
                        $scope.Resstatee = $scope.cardData[0].AMST_ConState,
                        $scope.Resstate = $scope.cardData[0].AMST_ConState;
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.htmldata))(($scope));

                })
        }

        //sent Request 
        $scope.submirequest = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASTUREQ_Id": $scope.ASTUREQ_Id,
                    "AMSTG_Id": $scope.AMSTG_Id,
                    "Mobilenumber": $scope.Mobilenumber,
                    "EmailidforCandidate": $scope.EmailidforCandidate,
                    "AMST_FatherMobleNo": $scope.AMST_FatherMobleNo,
                    "AMST_MotherMobileNo": $scope.AMST_MotherMobileNo,
                    "AMST_MotherEmailId": $scope.AMST_MotherEmailId,
                    "AMST_FatheremailId": $scope.AMST_FatheremailId,
                    "AMSTG_GuardianPhoneNo": $scope.AMSTG_GuardianPhoneNo,
                    "AMSTG_emailid": $scope.AMSTG_emailid,
                    "AMST_BloodGroup": $scope.Bloodgroup,

                    "STP_PERSTREET": $scope.Perstreett,
                    "STP_PERAREA": $scope.PerArea,
                    "STP_PERCITY": $scope.Percity,
                    "STP_PERSTATE": $scope.PerState,
                    "STP_PERCOUNTRY": $scope.PerCountry,
                    "STP_PERPIN": $scope.PerPincode,
                    "STP_CURSTREET": $scope.Resstreet,
                    "STP_CURAREA": $scope.ResArea,
                    "STP_CURCITY": $scope.Rescity,
                    "STP_CURSTATE": $scope.Resstate,
                    "STP_CURCOUNTRY": $scope.Rescountry,
                    "STP_CURPIN": $scope.ResPincode,
                }

                swal({
                    title: "Are you sure",
                    text: "Do you want to Send  Update Request??????",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send  it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("UpdateRequest/saverequest", data).
                                then(function (promise) {
                                    if (promise.returnval == true) {
                                        swal('Update Request Sent Successfully..!!!')
                                        $state.reload();
                                    }
                                    else {

                                    }

                                });
                        }
                        else {
                            swal("Update Request Cancelled");
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }

        }


        //end



        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("UpdateRequest/getloaddata").
                then(function (promise) {

                    $scope.cardData = promise.cardData;
                    $scope.rolename = promise.roleName;
                    $scope.Griddata = promise.griddata;
                    //if (promise.griddata.length != 0) {
                    //    $scope.showgrid = true;
                    //}
                    //details = promise.griddata;
                    $scope.countrydrop = promise.countryDrpDown;
                    $scope.statedrop = promise.stateDrpDown;
                    $scope.statedropp = promise.stateDrpDown;
                    $scope.existsornot = promise.existsornot;
                    $scope.updatestudetailslist = promise.updatestudetailslist;
                    if ((promise.updatestudetailslist != null && promise.updatestudetailslist.length > 0) && $scope.rolename == 'student') {
                    //STUDENT DETAILS
                        $scope.showgrid = true;
                    $scope.NameoftheCandidate = $scope.updatestudetailslist[0].studentname;
                    $scope.EmailidforCandidate = $scope.updatestudetailslist[0].AMST_emailId;
                    $scope.EmailidforCandidate1 = $scope.updatestudetailslist[0].AMST_emailId;
                    $scope.Mobilenumber = $scope.updatestudetailslist[0].AMST_MobileNo;
                    $scope.Mobilenumber1 = $scope.updatestudetailslist[0].AMST_MobileNo;
                        $scope.Bloodgroup = $scope.updatestudetailslist[0].AMST_BloodGroup;
                        $scope.Bloodgroup1 = $scope.updatestudetailslist[0].AMST_BloodGroup;
                    //FATHER DETAILS
                    $scope.fatherName = $scope.updatestudetailslist[0].fatherName;
                    $scope.AMST_FatherMobleNo = $scope.updatestudetailslist[0].AMST_FatherMobleNo;
                    $scope.AMST_FatherMobleNo1 = $scope.updatestudetailslist[0].AMST_FatherMobleNo;
                    $scope.AMST_FatheremailId = $scope.updatestudetailslist[0].AMST_FatheremailId;
                    $scope.AMST_FatheremailId1 = $scope.updatestudetailslist[0].AMST_FatheremailId;

                    //Mother Details

                    $scope.mothername = $scope.updatestudetailslist[0].mothername;
                    $scope.AMST_MotherMobileNo = $scope.updatestudetailslist[0].AMST_MotherMobileNo;
                    $scope.AMST_MotherMobileNo1 = $scope.updatestudetailslist[0].AMST_MotherMobileNo;
                    $scope.AMST_MotherEmailId = $scope.updatestudetailslist[0].AMST_MotherEmailId;
                    $scope.AMST_MotherEmailId1 = $scope.updatestudetailslist[0].AMST_MotherEmailId;


                    //  ADDRESS

                    $scope.AMSTG_Id = $scope.updatestudetailslist[0].AMSTG_Id,

                        $scope.ASTUREQ_Id = $scope.updatestudetailslist[0].ASTUREQ_Id,
                        $scope.Perstreett = $scope.updatestudetailslist[0].AMST_PerStreet,
                        $scope.Percity = $scope.updatestudetailslist[0].AMST_PerCity,
                        $scope.PerArea = $scope.updatestudetailslist[0].AMST_PerArea,
                        $scope.PerPincode = $scope.updatestudetailslist[0].AMST_PerPincode,
                        $scope.PerCountry = $scope.updatestudetailslist[0].AMST_PerCountry,
                        $scope.PerCountryy = $scope.updatestudetailslist[0].AMST_PerCountry,
                        $scope.PerStatee = $scope.updatestudetailslist[0].AMST_PerState,
                        $scope.PerState = $scope.updatestudetailslist[0].AMST_PerState,
                        $scope.Resstreet = $scope.updatestudetailslist[0].AMST_ConStreet,
                        $scope.Rescity = $scope.updatestudetailslist[0].AMST_ConCity,
                        $scope.ResArea = $scope.updatestudetailslist[0].AMST_ConArea,
                        $scope.ResPincode = $scope.updatestudetailslist[0].AMST_ConPincode,
                        $scope.Rescountryy = $scope.updatestudetailslist[0].AMST_ConCountry,
                        $scope.Rescountry = $scope.updatestudetailslist[0].AMST_ConCountry,
                        $scope.Resstatee = $scope.updatestudetailslist[0].AMST_ConState,
                        $scope.Resstate = $scope.updatestudetailslist[0].AMST_ConState;



                    $scope.Perstreett1 = $scope.updatestudetailslist[0].AMST_PerStreet,
                        $scope.Percity1 = $scope.updatestudetailslist[0].AMST_PerCity,
                        $scope.PerArea1 = $scope.updatestudetailslist[0].AMST_PerArea,
                        $scope.PerPincode1 = $scope.updatestudetailslist[0].AMST_PerPincode,
                        $scope.PerCountry1 = $scope.updatestudetailslist[0].AMST_PerCountry,
                        $scope.PerCountryy1 = $scope.updatestudetailslist[0].AMST_PerCountry,
                        $scope.PerStatee1 = $scope.updatestudetailslist[0].AMST_PerState,
                        $scope.PerState1 = $scope.updatestudetailslist[0].AMST_PerState,
                        $scope.Resstreet1 = $scope.updatestudetailslist[0].AMST_ConStreet,
                        $scope.Rescity1 = $scope.updatestudetailslist[0].AMST_ConCity,
                        $scope.ResArea1 = $scope.updatestudetailslist[0].AMST_ConArea,
                        $scope.ResPincode1 = $scope.updatestudetailslist[0].AMST_ConPincode,
                        $scope.Rescountryy1 = $scope.updatestudetailslist[0].AMST_ConCountry,
                        $scope.Rescountry1 = $scope.updatestudetailslist[0].AMST_ConCountry,
                        $scope.Resstatee1 = $scope.updatestudetailslist[0].AMST_ConState,
                        $scope.Resstate1 = $scope.updatestudetailslist[0].AMST_ConState;

                    if ($scope.ASTUREQ_Id > 0) {

                        $scope.ASTUREQ_Date = $scope.updatestudetailslist[0].ASTUREQ_Date;
                    }
                }
                
                })
        };

        $scope.approveedit = function (datalist)

        {
            $scope.showgrid = true;
            $scope.NameoftheCandidate = datalist.studentname;
            $scope.EmailidforCandidate = datalist.AMST_emailId;
            $scope.EmailidforCandidate1 = datalist.AMST_emailId;
            $scope.Mobilenumber = datalist.AMST_MobileNo;
            $scope.Mobilenumber1 = datalist.AMST_MobileNo;
            $scope.AMST_ID = datalist.AMST_Id;
            $scope.Bloodgroup = datalist.AMST_BloodGroup;
            $scope.Bloodgroup1 = datalist.AMST_BloodGroup;
            //alert($scope.Bloodgroup);
            //FATHER DETAILS
            $scope.fatherName = datalist.fatherName;
            $scope.AMST_FatherMobleNo = datalist.AMST_FatherMobleNo;
            $scope.AMST_FatherMobleNo1 = datalist.AMST_FatherMobleNo;
            $scope.AMST_FatheremailId = datalist.AMST_FatheremailId;
            $scope.AMST_FatheremailId1 = datalist.AMST_FatheremailId;

            //Mother Details

            $scope.mothername = datalist.mothername;
            $scope.AMST_MotherMobileNo = datalist.AMST_MotherMobileNo;
            $scope.AMST_MotherMobileNo1 = datalist.AMST_MotherMobileNo;
            $scope.AMST_MotherEmailId = datalist.AMST_MotherEmailId;
            $scope.AMST_MotherEmailId1 = datalist.AMST_MotherEmailId;


            //  ADDRESS

            $scope.AMSTG_Id = datalist.AMSTG_Id,

                $scope.ASTUREQ_Id = datalist.ASTUREQ_Id,
                $scope.Perstreett = datalist.AMST_PerStreet,
                $scope.Percity = datalist.AMST_PerCity,
                $scope.PerArea = datalist.AMST_PerArea,
                $scope.PerPincode = datalist.AMST_PerPincode,
                $scope.PerCountry = datalist.AMST_PerCountry,
                $scope.PerCountryy = datalist.AMST_PerCountry,
                $scope.PerStatee = datalist.AMST_PerState,
                $scope.PerState = datalist.AMST_PerState,
                $scope.Resstreet = datalist.AMST_ConStreet,
                $scope.Rescity = datalist.AMST_ConCity,
                $scope.ResArea = datalist.AMST_ConArea,
                $scope.ResPincode = datalist.AMST_ConPincode,
                $scope.Rescountryy = datalist.AMST_ConCountry,
                $scope.Rescountry = datalist.AMST_ConCountry,
                $scope.Resstatee = datalist.AMST_ConState,
                $scope.Resstate = datalist.AMST_ConState;



            $scope.Perstreett1 = datalist.AMST_PerStreet,
                $scope.Percity1 = datalist.AMST_PerCity,
                $scope.PerArea1 = datalist.AMST_PerArea,
                $scope.PerPincode1 = datalist.AMST_PerPincode,
                $scope.PerCountry1 = datalist.AMST_PerCountry,
                $scope.PerCountryy1 = datalist.AMST_PerCountry,
                $scope.PerStatee1 = datalist.AMST_PerState,
                $scope.PerState1 = datalist.AMST_PerState,
                $scope.Resstreet1 = datalist.AMST_ConStreet,
                $scope.Rescity1 = datalist.AMST_ConCity,
                $scope.ResArea1 = datalist.AMST_ConArea,
                $scope.ResPincode1 = datalist.AMST_ConPincode,
                $scope.Rescountryy1 = datalist.AMST_ConCountry,
                $scope.Rescountry1 = datalist.AMST_ConCountry,
                $scope.Resstatee1 = datalist.AMST_ConState,
                $scope.Resstate1 = datalist.AMST_ConState;

            if ($scope.ASTUREQ_Id > 0) {

                $scope.ASTUREQ_Date = datalist.ASTUREQ_Date;
            }

        }

        //-----------Radio change G
        $scope.radio_g = function () {
            if ($scope.parent == 'S') {
                $scope.NameoftheCandidate = $scope.cardData[0].studentname;
                $scope.EmailidforCandidate = $scope.cardData[0].AMST_emailId;
                $scope.Mobilenumber = $scope.cardData[0].AMST_MobileNo;
                $scope.imageF = $scope.cardData[0].AMST_Photoname;
                $('#blahF').attr('src', $scope.cardData[0].AMST_Photoname);
            }
            if ($scope.parent == 'F') {
                $scope.NameoftheCandidate = $scope.cardData[0].fatherName;
                $scope.Mobilenumber = $scope.cardData[0].AMST_FatherMobleNo
                $scope.EmailidforCandidate = $scope.cardData[0].AMST_FatheremailId;
                $scope.imageF = $scope.cardData[0].ANST_FatherPhoto;
                $('#blahF').attr('src', $scope.cardData[0].ANST_FatherPhoto);
            }
            if ($scope.parent == 'M') {
                $scope.NameoftheCandidate = $scope.cardData[0].mothername;
                $scope.Mobilenumber = $scope.cardData[0].AMST_MotherMobileNo;
                $scope.EmailidforCandidate = $scope.cardData[0].AMST_MotherEmailId;
                $scope.imageF = $scope.cardData[0].ANST_MotherPhoto;
                $('#blahF').attr('src', $scope.cardData[0].ANST_MotherPhoto);
            }
        };


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //-------------Save Data student
        $scope.submitted = false;
      



        //-----Save Data Admin
        $scope.submitted = false;
        $scope.savedataadmin = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "ASTUREQ_Id": $scope.ASTUREQ_Id,
                    "AMST_ID": $scope.AMST_ID,
                    "AMSTG_Id": $scope.AMSTG_Id,
                    "Mobilenumber": $scope.Mobilenumber,
                    "EmailidforCandidate": $scope.EmailidforCandidate,
                    "AMST_FatherMobleNo": $scope.AMST_FatherMobleNo,
                    "AMST_MotherMobileNo": $scope.AMST_MotherMobileNo,
                    "AMST_MotherEmailId": $scope.AMST_MotherEmailId,
                    "AMST_FatheremailId": $scope.AMST_FatheremailId,
                    "AMSTG_GuardianPhoneNo": $scope.AMSTG_GuardianPhoneNo,
                    "AMSTG_emailid": $scope.AMSTG_emailid,
                    "AMST_BloodGroup": $scope.Bloodgroup,

                    "STP_PERSTREET": $scope.Perstreett,
                    "STP_PERAREA": $scope.PerArea,
                    "STP_PERCITY": $scope.Percity,
                    "STP_PERSTATE": $scope.PerState,
                    "STP_PERCOUNTRY": $scope.PerCountry,
                    "STP_PERPIN": $scope.PerPincode,
                    "STP_CURSTREET": $scope.Resstreet,
                    "STP_CURAREA": $scope.ResArea,
                    "STP_CURCITY": $scope.Rescity,
                    "STP_CURSTATE": $scope.Resstate,
                    "STP_CURCOUNTRY": $scope.Rescountry,
                    "STP_CURPIN": $scope.ResPincode,
                }

                swal({
                    title: "Are you sure",
                    text: "Do you want to Approve the  Update Request??????",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approve  it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("UpdateRequest/savedataadmin", data).
                                then(function (promise) {
                                    if (promise.returnval == true) {
                                        swal('Update Request Approved Successfully..!!!')
                                        $state.reload();
                                    }
                                    else {

                                    }

                                });
                        }
                        else {
                            swal("Request Cancelled");
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }
        };



        $scope.savereject = function () {

         
                var data = {
                    "ASTUREQ_Id": $scope.ASTUREQ_Id,
                    "AMST_ID": $scope.AMST_ID,
                }

                swal({
                    title: "Are you sure",
                    text: "Do you want to Reject the  Update Request??????",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Reject  it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("UpdateRequest/savereject", data).
                                then(function (promise) {
                                    if (promise.returnval == true) {
                                        swal('Update Request Rejected Successfully..!!!')
                                        $state.reload();
                                    }
                                    else {

                                    }

                                });
                        }
                        else {
                            swal("Rejection Cancelled");
                        }
                    });

           
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.onSelectGetState2 = function (countryidd) {
            apiService.getURI("UpdateRequest/getdpstate", countryidd).then(function (promise) {
                $scope.statedropp = promise.stateDrpDown;
            })
        }

        $scope.onSelectGetState1 = function (countryidd) {
            apiService.getURI("UpdateRequest/getdpstate", countryidd).then(function (promise) {
                $scope.statedrop = promise.stateDrpDown;
            })
        }

        //--------Upload Father pic
        $scope.UploadStudentProfilePicF = [];
        $scope.uploadStudentProfilePicF = function (input, document) {

            $scope.UploadStudentProfilePicF = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blahF')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadprofileF();

                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function UploadprofileF() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadStudentProfilePicF.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePicF[i]);
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
                    $scope.imageF = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        $scope.onformclickstudent = function () {

            $scope.studentname = $scope.cardData[0].studentname;
            $scope.admno = $scope.cardData[0].admissionno;
            $scope.studentemail = $scope.cardData[0].AMST_emailId;
            $scope.studentmobile = $scope.cardData[0].AMST_MobileNo;
            $scope.studentphoto = $scope.cardData[0].AMST_Photoname;
            $('#blahstudent').attr('src', $scope.cardData[0].AMST_Photoname);

            $scope.pBloodgroup = $scope.cardData[0].AMST_BloodGroup;
                $scope.pPerstreett = $scope.cardData[0].AMST_PerStreet;
                $scope.pPercity = $scope.cardData[0].AMST_PerCity;
                $scope.pPerArea = $scope.cardData[0].AMST_PerArea;
                $scope.pPerPincode = $scope.cardData[0].AMST_PerPincode;
                $scope.pPerCountry = $scope.cardData[0].AMST_PerCountry;
                $scope.pPerCountryy = $scope.cardData[0].AMST_PerCountry;
                $scope.pPerStatee = $scope.cardData[0].AMST_PerState;
                $scope.pPerState = $scope.cardData[0].AMST_PerState;
                $scope.pResstreet = $scope.cardData[0].AMST_ConStreet;
                $scope.pRescity = $scope.cardData[0].AMST_ConCity;
                $scope.pResArea = $scope.cardData[0].AMST_ConArea;
                $scope.pResPincode = $scope.cardData[0].AMST_ConPincode;
                $scope.pRescountryy = $scope.cardData[0].AMST_ConCountry;
                $scope.pRescountry = $scope.cardData[0].AMST_ConCountry;
                $scope.pResstatee = $scope.cardData[0].ASMC_SectionName;
                $scope.pclasss = $scope.cardData[0].ASMC_SectionName;
            $scope.padmno = $scope.cardData[0].AMST_AdmNo;
        }

        $scope.onformclickfather = function () {


            $scope.mothername = $scope.cardData[0].mothername;
            $scope.fathername = $scope.cardData[0].fatherName;
            $scope.fathermobile = $scope.cardData[0].AMST_FatherMobleNo;
            $scope.fatheremail = $scope.cardData[0].AMST_FatheremailId;
            $scope.fatherphoto = $scope.cardData[0].ANST_FatherPhoto;
            $scope.motherphoto = $scope.cardData[0].ANST_MotherPhoto;
            $scope.admno = $scope.cardData[0].admissionno;
            $('#blahfather').attr('src', $scope.cardData[0].ANST_FatherPhoto);
            $('#blahmother').attr('src', $scope.cardData[0].ANST_MotherPhoto);



            $scope.pBloodgroup = "";
                $scope.pPerstreett = $scope.cardData[0].AMST_PerStreet;
                $scope.pPercity = $scope.cardData[0].AMST_PerCity;
                $scope.pPerArea = $scope.cardData[0].AMST_PerArea;
                $scope.pPerPincode = $scope.cardData[0].AMST_PerPincode;
                $scope.pPerCountry = $scope.cardData[0].AMST_PerCountry;
                $scope.pPerCountryy = $scope.cardData[0].AMST_PerCountry;
                $scope.pPerStatee = $scope.cardData[0].AMST_PerState;
                $scope.pPerState = $scope.cardData[0].AMST_PerState;
                $scope.pResstreet = $scope.cardData[0].AMST_ConStreet;
                $scope.pRescity = $scope.cardData[0].AMST_ConCity;
                $scope.pResArea = $scope.cardData[0].AMST_ConArea;
                $scope.pResPincode = $scope.cardData[0].AMST_ConPincode;
                $scope.pRescountryy = $scope.cardData[0].AMST_ConCountry;
                $scope.pRescountry = $scope.cardData[0].AMST_ConCountry;
            $scope.pResstatee = $scope.cardData[0].ASMC_SectionName;

        }


        $scope.onformclickmother = function () {

            $scope.mothername = $scope.cardData[0].mothername;
            $scope.fathername = $scope.cardData[0].fatherName;
            $scope.mothermobile = $scope.cardData[0].AMST_MotherMobileNo;
            $scope.motheremail = $scope.cardData[0].AMST_MotherEmailId;
            $scope.motherphoto = $scope.cardData[0].ANST_MotherPhoto;
            $scope.fatherphoto = $scope.cardData[0].ANST_FatherPhoto;
            $scope.admno = $scope.cardData[0].admissionno;
            $('#blahmother').attr('src', $scope.cardData[0].ANST_MotherPhoto);
            $('#blahfather').attr('src', $scope.cardData[0].ANST_FatherPhoto);


            $scope.pBloodgroup = "";
                $scope.pPerstreett = $scope.cardData[0].AMST_PerStreet;
                $scope.pPercity = $scope.cardData[0].AMST_PerCity;
                $scope.pPerArea = $scope.cardData[0].AMST_PerArea;
                $scope.pPerPincode = $scope.cardData[0].AMST_PerPincode;
                $scope.pPerCountry = $scope.cardData[0].AMST_PerCountry;
                $scope.pPerCountryy = $scope.cardData[0].AMST_PerCountry;
                $scope.pPerStatee = $scope.cardData[0].AMST_PerState;
                $scope.pPerState = $scope.cardData[0].AMST_PerState;
                $scope.pResstreet = $scope.cardData[0].AMST_ConStreet;
                $scope.pRescity = $scope.cardData[0].AMST_ConCity;
                $scope.pResArea = $scope.cardData[0].AMST_ConArea;
                $scope.pResPincode = $scope.cardData[0].AMST_ConPincode;
                $scope.pRescountryy = $scope.cardData[0].AMST_ConCountry;
                $scope.pRescountry = $scope.cardData[0].AMST_ConCountry;
            $scope.pResstatee = $scope.cardData[0].ASMC_SectionName;

        }

        $scope.smartcard = function () {

            var innerContents = document.getElementById("smartcardid").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" href="css/print/Smartcard/BaldwinSmartcardPdf.css" rel="stylesheet" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.smartcardmother = function () {

            var innerContents = document.getElementById("smartcardidmother").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" href="css/print/Smartcard/BaldwinSmartcardPdf.css" rel="stylesheet" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.smartcardstudent = function () {

            var innerContents = document.getElementById("smartcardidstudent").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" href="css/print/Smartcard/BaldwinSmartcardPdf.css" rel="stylesheet" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        //Student Search Dropdown
        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '2') {

                var data = {
                    "searchfilter": objj.search
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("UpdateRequest/searchfilter", data).
                    then(function (promise) {

                        $scope.studentlst = promise.fillstudent;

                        angular.forEach($scope.studentlst, function (objectt) {
                            if (objectt.amsT_FirstName.length > 0) {
                                var string = objectt.amsT_FirstName;
                                objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }

        }

        //Cancel
        $scope.cancel = function () {

            $state.reload();
            //$scope.clear();
        }

    };
})();