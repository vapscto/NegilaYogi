//import { Promise } from "./C:/Users/vaps/AppData/Local/Microsoft/TypeScript/2.6/node_modules/@types/bluebird";

(function () {
    'use strict';
    angular
        .module('app')
        .controller('ParentsSmartCardController', ParentsSmartCardController)

    ParentsSmartCardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', '$window', 'superCache', '$compile']
    function ParentsSmartCardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, $window, superCache, $compile) {

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.showgrid = false;
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
            apiService.create("ParentsSmartCard/getstudata", data).
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

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("ParentsSmartCard/getloaddata").
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
                        $scope.ftname = $scope.cardData[0].fatherName;
                        $scope.mtname = $scope.cardData[0].mothername;
                        $scope.EmailidforCandidate = $scope.cardData[0].AMST_emailId;
                        $scope.Mobilenumber = $scope.cardData[0].AMST_MobileNo;
                        $scope.imageF = $scope.cardData[0].AMST_Photoname;
                        $('#blahF').attr('src', $scope.cardData[0].AMST_Photoname);
                  
                    }
                    if ($scope.parent == 'F') {

                        $scope.NameoftheCandidate = $scope.cardData[0].fatherName;
                        $scope.FMobilenumber = $scope.cardData[0].AMST_FatherMobleNo;
                        $scope.EmailidforCandidate = $scope.cardData[0].AMST_FatheremailId;
                        $scope.imageF = $scope.cardData[0].ANST_FatherPhoto;
                        $('#blahF').attr('src', $scope.cardData[0].ANST_FatherPhoto);
                        $('#blahF').attr('src', $scope.cardData[0].ANST_MotherPhoto);
                    }
                    if ($scope.parent == 'M') {

                        $scope.fatherphoto = $scope.cardData[0].ANST_FatherPhoto;
                        $scope.motherphoto = $scope.cardData[0].ANST_MotherPhoto;

                        $scope.NameoftheCandidate = $scope.cardData[0].mothername;
                        $scope.MMobilenumber = $scope.cardData[0].AMST_MotherMobileNo;
                        $scope.EmailidforCandidate = $scope.cardData[0].AMST_MotherEmailId;
                        $scope.imageF = $scope.cardData[0].ANST_MotherPhoto;

                        $('#blahF').attr('src', $scope.cardData[0].ANST_MotherPhoto);
                        $('#blahF').attr('src', $scope.cardData[0].ANST_FatherPhoto);

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
        };
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
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {

                var sname = "";
                var Fname = "";
                var Mname = "";
                var Semail = "";
                var Femail = "";
                var Memail = "";
                var Smobile = "";
                var Fmobile = "";
                var Mmobile = "";
                var sphoto = "";
                var Fphoto = "";
                var Mphoto = "";
                if ($scope.existsornot == true) {
                    if ($scope.parent == 'S') {
                        sname = $scope.NameoftheCandidate;
                        Semail = $scope.EmailidforCandidate;
                        Smobile = $scope.Mobilenumber;
                        sphoto = $scope.imageF;
                    }
                    else if ($scope.parent == 'F') {
                        Fname = $scope.NameoftheCandidate;
                        Femail = $scope.EmailidforCandidate;
                        Fmobile = $scope.Mobilenumber;
                        Fphoto = $scope.imageF;
                    }
                    else if ($scope.parent == 'M') {
                        Mname = $scope.NameoftheCandidate;
                        Memail = $scope.EmailidforCandidate;
                        Mmobile = $scope.Mobilenumber;
                        Mphoto = $scope.imageF;
                    }
                }
                else {
                    if ($scope.parent == 'S') {
                        sname = $scope.NameoftheCandidate;
                        Semail = $scope.EmailidforCandidate;
                        Smobile = $scope.Mobilenumber;
                        sphoto = $scope.imageF;

                        Fname = $scope.cardData[0].fatherName;
                        Femail = $scope.cardData[0].AMST_FatheremailId;
                        Fmobile = $scope.cardData[0].AMST_FatherMobleNo;
                        Fphoto = $scope.cardData[0].ANST_FatherPhoto;

                        Mname = $scope.cardData[0].mothername;
                        Memail = $scope.cardData[0].AMST_MotherEmailId;
                        Mmobile = $scope.cardData[0].AMST_MotherMobileNo;
                        Mphoto = $scope.cardData[0].ANST_MotherPhoto;
                    }
                    else if ($scope.parent == 'F') {
                        Fname = $scope.NameoftheCandidate;
                        Femail = $scope.EmailidforCandidate;
                        Fmobile = $scope.Mobilenumber;
                        Fphoto = $scope.imageF;

                        sname = $scope.cardData[0].studentname;
                        Semail = $scope.cardData[0].AMST_emailId;
                        Smobile = $scope.cardData[0].AMST_MobileNo;
                        sphoto = $scope.cardData[0].AMST_Photoname;

                        Mname = $scope.cardData[0].mothername;
                        Memail = $scope.cardData[0].AMST_MotherEmailId;
                        Mmobile = $scope.cardData[0].AMST_MotherMobileNo;
                        Mphoto = $scope.cardData[0].ANST_MotherPhoto;
                    }
                    else if ($scope.parent == 'M') {
                        Mname = $scope.NameoftheCandidate;
                        Memail = $scope.EmailidforCandidate;
                        Mmobile = $scope.Mobilenumber;
                        Mphoto = $scope.imageF;

                        Fname = $scope.cardData[0].fatherName;
                        Femail = $scope.cardData[0].AMST_FatheremailId;
                        Fmobile = $scope.cardData[0].AMST_FatherMobleNo;
                        Fphoto = $scope.cardData[0].ANST_FatherPhoto;

                        sname = $scope.cardData[0].studentname;
                        Semail = $scope.cardData[0].AMST_emailId;
                        Smobile = $scope.cardData[0].AMST_MobileNo;
                        sphoto = $scope.cardData[0].AMST_Photoname;
                    }
                }


                $scope.docs = [];
                angular.forEach($scope.images_temp, function (itm) {
                    $scope.docs.push(itm);
                });


                var data = {
                    "STP_SNAME": sname,
                    "STP_SEMAIL": Semail,
                    "STP_SMOBILENO": Smobile,
                    "STP_SBLOOD": $scope.Bloodgroup,
                    "STP_SPHOTO": sphoto,
                    "STP_FNAME": Fname,
                    "STP_FEMAIL": Femail,
                    "STP_FMOBILENO": Fmobile,
                    "STP_FBLOOD": "",
                    "STP_FPHOTO": Fphoto,
                    "STP_MNAME": Mname,
                    "STP_MEMAIL": Memail,
                    "STP_MMOBILENO": Mmobile,
                    "STP_MBLOOD": "",
                    "STP_MPHOTO": Mphoto,
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
                    "STP_FLAG": $scope.parent,
                }



                apiService.create("ParentsSmartCard/savedata", data).then(function (promise) {


                    if (promise.returnval == "true") {
                        swal("Data Saved Successfully,To Update It Please Visit School!!");
                        $state.reload();
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == "false") {
                        swal("Failed to Save/Update");
                    }
                    else if (promise.returnval == "Y") {
                        swal('Failed to Update Data', 'Application approved Already!!');
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };



        //-----Save Data Admin
        $scope.submitted = false;
        $scope.savedataadmin = function () {

            if ($scope.myForm.$valid) {

                var sname = "";
                var Fname = "";
                var Mname = "";
                var Semail = "";
                var Femail = "";
                var Memail = "";
                var Smobile = "";
                var Fmobile = "";
                var Mmobile = "";
                var sphoto = "";
                var Fphoto = "";
                var Mphoto = "";
                if ($scope.existsornot == true) {
                    if ($scope.parent == 'S') {
                        sname = $scope.NameoftheCandidate;
                        Semail = $scope.EmailidforCandidate;
                        Smobile = $scope.Mobilenumber;
                        sphoto = $scope.imageF;
                    }
                    else if ($scope.parent == 'F') {
                        Fname = $scope.NameoftheCandidate;
                        Femail = $scope.EmailidforCandidate;
                        Fmobile = $scope.Mobilenumber;
                        Fphoto = $scope.imageF;
                    }
                    else if ($scope.parent == 'M') {
                        Mname = $scope.NameoftheCandidate;
                        Memail = $scope.EmailidforCandidate;
                        Mmobile = $scope.Mobilenumber;
                        Mphoto = $scope.imageF;
                    }
                }
                else {
                    sname = $scope.NameoftheCandidate;
                    Semail = $scope.EmailidforCandidate;
                    Smobile = $scope.Mobilenumber;
                    sphoto = $scope.imageF;

                    Fname = $scope.cardData[0].fatherName;
                    Femail = $scope.cardData[0].AMST_FatheremailId;
                    Fmobile = $scope.cardData[0].AMST_FatherMobleNo;
                    Fphoto = $scope.cardData[0].ANST_FatherPhoto;

                    Mname = $scope.cardData[0].mothername;
                    Memail = $scope.cardData[0].AMST_MotherEmailId;
                    Mmobile = $scope.cardData[0].AMST_MotherMobileNo;
                    Mphoto = $scope.cardData[0].ANST_MotherPhoto;
                }


                $scope.docs = [];
                angular.forEach($scope.images_temp, function (itm) {
                    $scope.docs.push(itm);
                });


                var data = {
                    "STP_SNAME": sname,
                    "STP_SEMAIL": Semail,
                    "STP_SMOBILENO": Smobile,
                    "STP_SBLOOD": $scope.Bloodgroup,
                    "STP_SPHOTO": sphoto,
                    "STP_FNAME": Fname,
                    "STP_FEMAIL": Femail,
                    "STP_FMOBILENO": Fmobile,
                    "STP_FBLOOD": "",
                    "STP_FPHOTO": Fphoto,
                    "STP_MNAME": Mname,
                    "STP_MEMAIL": Memail,
                    "STP_MMOBILENO": Mmobile,
                    "STP_MBLOOD": "",
                    "STP_MPHOTO": Mphoto,
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
                    "STP_FLAG": $scope.parent,
                    "AMST_ID": $scope.Amst_Id.amsT_ID,
                }

                apiService.create("ParentsSmartCard/savedataadmin", data).then(function (promise) {

                    if (promise.returnval == "true") {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == "false") {
                        swal("Failed to Save/Update");
                    }
                    else if (promise.returnval == "Y") {
                        swal('Failed to Update Data', 'Application approved Already!!');
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.onSelectGetState2 = function (countryidd) {
            apiService.getURI("ParentsSmartCard/getdpstate", countryidd).then(function (promise) {
                $scope.statedropp = promise.stateDrpDown;
            })
        }

        $scope.onSelectGetState1 = function (countryidd) {
            apiService.getURI("ParentsSmartCard/getdpstate", countryidd).then(function (promise) {
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

                apiService.create("ParentsSmartCard/searchfilter", data).
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