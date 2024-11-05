
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_MasterMemberController', CMS_MasterMemberController)

    CMS_MasterMemberController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function CMS_MasterMemberController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};


        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.Qualifi = true;
        $scope.Experience = true;
        $scope.MBLKED = true;
        $scope.mbno = true;
        $scope.mbemail = true;
        $scope.mdocument = true;
        $scope.searchValue = "";
        $scope.obj = {};
        $scope.mastercaste = [];
        $scope.stateall = [];
        $scope.myTabIndex = 0;
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("CMS_Member_Master/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                $scope.presentCountgrid = $scope.getreport.length;
                $scope.catlist = promise.catlist;
                $scope.memlist = promise.memlist;
                $scope.allcountry = promise.allcountry;
                $scope.countrylist = promise.allcountry;
                $scope.stateall = promise.allstate;
                $scope.gender = promise.gender;
                $scope.allcasteCategory = promise.castecategory;
                $scope.mastercaste = promise.mastercaste;
                $scope.allReligion = promise.allReligion;
                $scope.allAcademicYear = promise.accdemiYear;
            })

        };
        $scope.getcaste = function (imcC_Id) {
            $scope.allCaste = [];
            if (imcC_Id != null && imcC_Id > 0) {
                for (var i = 0; i < $scope.mastercaste.length; i++) {
                    if ($scope.mastercaste[i].imcC_Id == imcC_Id) {
                        $scope.allCaste.push({
                            imC_Id: $scope.mastercaste[i].imC_Id,
                            imC_CasteName: $scope.mastercaste[i].imC_CasteName
                        });
                    }
                }
            }
            else {
                swal("Select Caste Category");
            }


        };
        //qulification state
        $scope.QulificationState = function (CMSMMEMQULQ_Country) {
            $scope.statelist = [];
            if (CMSMMEMQULQ_Country != null && CMSMMEMQULQ_Country > 0) {
                for (var i = 0; i < $scope.stateall.length; i++) {
                    if ($scope.stateall[i].ivrmmC_Id == CMSMMEMQULQ_Country) {
                        $scope.statelist.push({
                            ivrmmS_Id: $scope.stateall[i].ivrmmS_Id,
                            ivrmmS_Name: $scope.stateall[i].ivrmmS_Name
                        });
                    }
                }
            }
            else {
                swal("Select State");
            }
        };
        //onSelectGetState

        $scope.onSelectGetStateone = function (CMSMMEM_LocCountry) {
            $scope.allStatetwo = [];
            if (CMSMMEM_LocCountry != null && CMSMMEM_LocCountry > 0) {
                for (var i = 0; i < $scope.stateall.length; i++) {
                    if ($scope.stateall[i].ivrmmC_Id == CMSMMEM_LocCountry) {
                        $scope.allStatetwo.push({
                            ivrmmS_Id: $scope.stateall[i].ivrmmS_Id,
                            ivrmmS_Name: $scope.stateall[i].ivrmmS_Name
                        });
                    }
                }
            }
            else {
                swal("Select State");
            }
        };
        $scope.onSelectGetState = function (IVRMMC_Id5) {
            $scope.allState = [];
            if (IVRMMC_Id5 != null && IVRMMC_Id5 > 0) {
                for (var i = 0; i < $scope.stateall.length; i++) {
                    if ($scope.stateall[i].ivrmmC_Id == IVRMMC_Id5) {
                        $scope.allState.push({
                            ivrmmS_Id: $scope.stateall[i].ivrmmS_Id,
                            ivrmmS_Name: $scope.stateall[i].ivrmmS_Name
                        });
                    }
                }
            }
            else {
                swal("Select State");
            }


        };
        

        $scope.UploadmemberProfilePic = [];
        $scope.UploadmemberProfilePic = function (input, document) {
            $scope.UploadmemberProfilePic = input.files;
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
        };
        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.UploadmemberProfilePic.length; i++) {
                formData.append("File", $scope.UploadmemberProfilePic[i]);
            }

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

        }
        $scope.submitted1 = false;
        $scope.saveddata1 = function () {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                var CMSMMEM_BlockedFlg = 0;
                var CMSMMEM_OtherClubMemberFlg = 0;
                var CMSMMEM_TerminatedFlg = 0;
                var CMSMMEM_LeftFlag = 0;
                var id = 0;
                if ($scope.obj.CMSMMEM_BlockedFlg === true) {
                    CMSMMEM_BlockedFlg = 1;
                } else {
                    CMSMMEM_BlockedFlg = 0;
                }

                if ($scope.obj.CMSMMEM_OtherClubMemberFlg === true) {
                    CMSMMEM_OtherClubMemberFlg = 1;
                }
                else {
                    CMSMMEM_OtherClubMemberFlg = 0;
                }
                if ($scope.obj.CMSMMEM_TerminatedFlg === true) {
                    CMSMMEM_TerminatedFlg = 1;
                }
                else {
                    CMSMMEM_TerminatedFlg = 0;
                }
                if ($scope.obj.CMSMMEM_LeftFlag === true) {
                    CMSMMEM_LeftFlag = 1;
                }
                else {
                    CMSMMEM_LeftFlag = 0;
                }

                if ($scope.obj.SpouseMobileNo === undefined) {
                    $scope.obj.SpouseMobileNo = 0;
                }
                if ($scope.obj.CMSMMEM_MemberFirstName === null || $scope.obj.CMSMMEM_MemberFirstName === undefined || $scope.obj.CMSMMEM_MemberFirstName === "") {
                    $scope.obj.CMSMMEM_MemberFirstName = "";
                } else {
                    $scope.obj.CMSMMEM_MemberFirstName = $scope.obj.CMSMMEM_MemberFirstName;
                }

                if ($scope.obj.CMSMMEM_MemberMiddleName === null || $scope.obj.CMSMMEM_MemberMiddleName === undefined || $scope.obj.CMSMMEM_MemberMiddleName === "") {
                    $scope.obj.CMSMMEM_MemberMiddleName = "";
                } else {
                    $scope.obj.CMSMMEM_MemberMiddleName = $scope.obj.CMSMMEM_MemberMiddleName;
                }

                if ($scope.obj.CMSMMEM_MemberLastName === null || $scope.obj.CMSMMEM_MemberLastName === undefined || $scope.obj.CMSMMEM_MemberLastName === "") {
                    $scope.obj.CMSMMEM_MemberLastName = "";
                } else {
                    $scope.obj.CMSMMEM_MemberLastName = $scope.obj.CMSMMEM_MemberLastName;
                }
                // var CMSMMEM_TerminatedDate = new Date($scope.CMSMMEM_TerminatedDate).toDateString();
                //var CMSMMEM_MembershipExpDate = new Date($scope.obj.CMSMMEM_MembershipExpDate).toDateString();

                //  var CMSMMEM_DOB = new Date($scope.CMSMMEM_DOB).toDateString();
                if ($scope.EditId == undefined || $scope.EditId == null || $scope.EditId == "") {
                    id = 0;
                } else {
                    id = $scope.EditId;
                }
                var sectionid = 0;
                if ($scope.obj.CMSMMEM_Id !== undefined && $scope.obj.CMSMMEM_Id !== null && $scope.obj.CMSMMEM_Id !== "") {
                    sectionid = $scope.obj.CMSMMEM_Id;
                } else {
                    sectionid = 0;
                }
                var CMSMMEM_Id = 0;
                if ($scope.obj.CMSMMEM_Id != null && $scope.obj.CMSMMEM_Id > 0) {
                    CMSMMEM_Id = $scope.obj.CMSMMEM_Id;
                }
                var data = {
                    "CMSMMEM_Id": CMSMMEM_Id,
                    "CMSMMEM_MemberFirstName": $scope.obj.CMSMMEM_MemberFirstName,
                    "IMCC_Id": $scope.obj.imcC_Id,
                    "IVRMMG_Id": $scope.obj.IVRMMG_Id,
                    "IVRMMR_Id": $scope.obj.ivrmmR_Id,
                    "IVRMMMS_Id": $scope.obj.CMSMMEM_PerState,
                    "IMC_Id": $scope.obj.iC_Id.imC_Id,
                    "CMSMCAT_Id": $scope.obj.CMSMCAT_Id,
                    "CMSMAPPL_Id": $scope.obj.CMSMAPPL_Id,
                    "CMSMMEM_BiometricCode": $scope.obj.CMSMMEM_BiometricCode,
                    "CMSMMEM_DOB": new Date($scope.obj.CMSMMEM_DOB).toDateString(),
                    "CMSMMEM_PerAdd1": $scope.obj.CMSMMEM_PerAdd1,
                    "CMSMMEM_PerCountry": $scope.obj.CMSMMEM_PerCountry,
                    "CMSMMEM_PerState": $scope.obj.CMSMMEM_PerState,
                    "CMSMMEM_FatherName": $scope.obj.CMSMMEM_FatherName,
                    "CMSMMEM_WeightUOM": $scope.obj.CMSMMEM_WeightUOM,
                    "CMSMMEM_Weight": $scope.obj.CMSMMEM_Weight,
                    "CMSMMEM_Proposedby": $scope.obj.CMSMMEM_Proposedby,
                    "CMSMMEM_MembershipNo": $scope.obj.CMSMMEM_MembershipNo,
                    "CMSMMEM_AadharCardNo": $scope.obj.CMSMMEM_AadharCardNo,
                    "CMSMMEM_UINo": $scope.obj.CMSMMEM_UINo,
                    "CMSMMEM_BloodGroup": $scope.obj.CMSMMEM_BloodGroup,
                    "CMSMMEM_LocCountry": $scope.obj.CMSMMEM_LocCountry,
                    "CMSMMEM_LocState": $scope.obj.CMSMMEM_LocState,
                    "CMSMMEM_SpouseMobileNo": $scope.obj.CMSMMEM_SpouseMobileNo,
                    "CMSMMEM_SpouseEmailId": $scope.obj.CMSMMEM_SpouseEmailId,
                    "CMSMMEM_DOL": new Date($scope.obj.CMSMMEM_DOL).toDateString(),
                    "CMSMMEM_MembershipExpDate": new Date($scope.obj.CMSMMEM_MembershipExpDate).toDateString(),
                    "CMSMMEM_HeightUOM": $scope.obj.CMSMMEM_HeightUOM,
                    "CMSMMEM_Height": $scope.obj.CMSMMEM_Height,
                    "CMSMMEM_LocAdd1": $scope.obj.CMSMMEM_LocAdd1,
                    "CMSMMEM_MemberMiddleName": $scope.obj.CMSMMEM_MemberMiddleName,
                    "CMSMMEM_MemberLastName": $scope.obj.CMSMMEM_MemberLastName,
                    "CMSMMEM_RFCardId": $scope.obj.CMSMMEM_RFCardId,
                    "CMSMMEM_PerAdd2": $scope.obj.CMSMMEM_PerAdd2,
                    "CMSMMEM_PerAdd3": $scope.obj.CMSMMEM_PerAdd3,
                    "CMSMMEM_PerAdd4": $scope.obj.CMSMMEM_PerAdd4,
                    "CMSMMEM_PerPincode": $scope.obj.CMSMMEM_PerPincode,
                    "CMSMMEM_LacAdd2": $scope.obj.CMSMMEM_LacAdd2,
                    "CMSMMEM_LocAdd3": $scope.obj.CMSMMEM_LocAdd3,
                    "CMSMMEM_LocAdd4": $scope.obj.CMSMMEM_LocAdd4,
                    "CMSMMEM_LocPincode": $scope.obj.CMSMMEM_LocPincode,
                    "CMSMMEM_SpouseName": $scope.obj.CMSMMEM_SpouseName,
                    "CMSMMEM_MotherName": $scope.obj.CMSMMEM_MotherName,
                    "CMSMMEM_SpouseOccupation": $scope.obj.CMSMMEM_SpouseOccupation,
                    "CMSMMEM_LeavingReason": $scope.obj.CMSMMEM_LeavingReason,
                    "CMSMMEM_LeftFlag": CMSMMEM_LeftFlag,
                    "CMSMMEM_TerminatedDate": new Date($scope.obj.CMSMMEM_TerminatedDate).toDateString(),
                    "CMSMMEM_TerminatedReason": $scope.obj.CMSMMEM_TerminatedReason,
                    "CMSMMEM_TerminatedFlg": CMSMMEM_TerminatedFlg,
                    "CMSMMEM_OtherClubMemberFlg": CMSMMEM_OtherClubMemberFlg,
                    "CMSMMEM_NationalSSN": $scope.obj.CMSMMEM_NationalSSN,
                    "CMSMMEM_PANCardNo": $scope.obj.CMSMMEM_PANCardNo,
                    "CMSMMEM_ApproverNo": $scope.obj.CMSMMEM_ApproverNo,
                    "CMSMMEM_ApprovedOn": $scope.obj.CMSMMEM_ApprovedOn,
                    "CMSMMEM_IdentificationMark": $scope.obj.CMSMMEM_IdentificationMark,
                    "CMSMMEM_EyeSightIssue": $scope.obj.CMSMMEM_EyeSightIssue,
                    "CMSMMEM_AnyHealthIssue": $scope.obj.CMSMMEM_AnyHealthIssue,
                    "CMSMMEM_SpouseAddress": $scope.obj.CMSMMEM_SpouseAddress,
                    "CMSMMEM_BlockedFlg": CMSMMEM_BlockedFlg,
                    "CMSMMEM_Photo": $scope.obj.image

                }
                apiService.create("CMS_Member_Master/savedetail1", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.Qualifi = false;
                            $scope.obj.CMSMMEM_Id = promise.cmsmmeM_Id;
                            //$scope.Experience = true;
                            //$scope.MBLKED = true;
                            //$scope.mbno = true;
                            //$scope.mbemail = true;
                            //$scope.mdocument = true;
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                            $scope.Qualifi = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.obj.CMSMMEM_Id = promise.cmsmmeM_Id;

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }


                    })

                // $scope.clear1();
            }

        };
        $scope.edit = function (user) {
            var data = {
                "CMSMMEM_Id": user.cmsmmeM_Id
            }
            apiService.create("CMS_Member_Master/editmember", data).
                then(function (promise) {
                    $scope.obj.image = "";
                    if (promise.editmember != null && promise.editmember.length > 0) {
                        $scope.obj.CMSMMEM_Id = promise.editmember[0].cmsmmeM_Id;                      
                        $scope.obj.CMSMCAT_Id = promise.editmember[0].cmsmcaT_Id;
                        $scope.obj.CMSMAPPL_Id = promise.editmember[0].cmsmappL_Id;
                        $scope.obj.CMSMMEM_Proposedby = promise.editmember[0].cmsmmeM_Proposedby;
                        $scope.obj.CMSMMEM_MemberFirstName = promise.editmember[0].cmsmmeM_MemberFirstName;
                        $scope.obj.CMSMMEM_MemberMiddleName = promise.editmember[0].cmsmmeM_MemberMiddleName;
                        $scope.obj.CMSMMEM_MemberLastName = promise.editmember[0].cmsmmeM_MemberLastName;
                        $scope.obj.CMSMMEM_MemberLastName = promise.editmember[0].cmsmmeM_MemberLastName;
                        $scope.obj.CMSMMEM_MembershipNo = promise.editmember[0].cmsmmeM_MembershipNo;
                        $scope.obj.CMSMMEM_BiometricCode = promise.editmember[0].cmsmmeM_BiometricCode;
                        $scope.obj.CMSMMEM_RFCardId = promise.editmember[0].cmsmmeM_RFCardId;
                        $scope.obj.CMSMMEM_PerAdd1 = promise.editmember[0].cmsmmeM_PerAdd1;
                        $scope.obj.CMSMMEM_PerAdd2 = promise.editmember[0].cmsmmeM_PerAdd2;
                        $scope.obj.CMSMMEM_PerAdd3 = promise.editmember[0].cmsmmeM_PerAdd3;
                        $scope.obj.CMSMMEM_PerAdd4 = promise.editmember[0].cmsmmeM_PerAdd4;
                        $scope.obj.CMSMMEM_PerCountry = promise.editmember[0].cmsmmeM_PerCountry;
                        if ($scope.obj.CMSMMEM_PerCountry != null && $scope.obj.CMSMMEM_PerCountry > 0) {
                            $scope.onSelectGetState($scope.obj.CMSMMEM_PerCountry);
                        }
                        $scope.obj.CMSMMEM_PerState = promise.editmember[0].cmsmmeM_PerState;                       
                        $scope.obj.CMSMMEM_PerPincode = promise.editmember[0].cmsmmeM_PerPincode;
                        $scope.obj.CMSMMEM_LocAdd1 = promise.editmember[0].cmsmmeM_LocAdd1;
                        $scope.obj.CMSMMEM_LacAdd2 = promise.editmember[0].cmsmmeM_LacAdd2;
                        $scope.obj.CMSMMEM_LocAdd3 = promise.editmember[0].cmsmmeM_LocAdd3;
                        $scope.obj.CMSMMEM_LocAdd4 = promise.editmember[0].cmsmmeM_LocAdd4;
                        $scope.obj.CMSMMEM_LocCountry = promise.editmember[0].cmsmmeM_LocCountry;
                        if ($scope.obj.CMSMMEM_LocCountry != null && $scope.obj.CMSMMEM_LocCountry > 0) {
                            $scope.onSelectGetStateone($scope.obj.CMSMMEM_LocCountry);
                        }
                        $scope.obj.CMSMMEM_LocState = promise.editmember[0].cmsmmeM_LocState;
                        $scope.obj.CMSMMEM_LocPincode = promise.editmember[0].cmsmmeM_LocPincode;                      
                        $scope.obj.IVRMMG_Id = promise.editmember[0].ivrmmG_Id;
                        $scope.obj.imcC_Id = promise.editmember[0].imcC_Id;
                        if ($scope.obj.imcC_Id != null && $scope.obj.imcC_Id > 0) {
                            $scope.getcaste($scope.obj.imcC_Id);
                        }                       
                        $scope.obj.ivrmmR_Id = promise.editmember[0].ivrmmR_Id;
                        $scope.obj.CMSMMEM_FatherName = promise.editmember[0].cmsmmeM_FatherName;
                        $scope.obj.CMSMMEM_MotherName = promise.editmember[0].cmsmmeM_MotherName;
                        $scope.obj.CMSMMEM_SpouseName = promise.editmember[0].cmsmmeM_SpouseName;
                        $scope.obj.CMSMMEM_SpouseOccupation = promise.editmember[0].cmsmmeM_SpouseOccupation;
                        $scope.obj.CMSMMEM_SpouseMobileNo = promise.editmember[0].cmsmmeM_SpouseMobileNo;
                        $scope.obj.CMSMMEM_SpouseEmailId = promise.editmember[0].cmsmmeM_SpouseEmailId;
                        $scope.obj.CMSMMEM_SpouseAddress = promise.editmember[0].cmsmmeM_SpouseAddress;
                        $scope.obj.CMSMMEM_DOB = new Date(promise.editmember[0].cmsmmeM_DOB);
                        $scope.obj.CMSMMEM_BloodGroup = promise.editmember[0].cmsmmeM_BloodGroup;
                        $scope.obj.CMSMMEM_Height = promise.editmember[0].cmsmmeM_Height;
                        $scope.obj.CMSMMEM_HeightUOM = promise.editmember[0].cmsmmeM_HeightUOM;
                        $scope.obj.CMSMMEM_Weight = promise.editmember[0].cmsmmeM_Weight;
                        $scope.obj.CMSMMEM_WeightUOM = promise.editmember[0].cmsmmeM_WeightUOM;
                        $scope.obj.CMSMMEM_AnyHealthIssue = promise.editmember[0].cmsmmeM_AnyHealthIssue;
                        $scope.obj.CMSMMEM_EyeSightIssue = promise.editmember[0].cmsmmeM_EyeSightIssue;
                        $scope.obj.CMSMMEM_IdentificationMark = promise.editmember[0].cmsmmeM_IdentificationMark;
                        $scope.obj.CMSMMEM_ApproverNo = promise.editmember[0].cmsmmeM_ApproverNo;
                        $scope.obj.CMSMMEM_ApprovedOn = promise.editmember[0].cmsmmeM_ApprovedOn;
                        $scope.obj.CMSMMEM_PANCardNo = promise.editmember[0].cmsmmeM_PANCardNo;
                        $scope.obj.CMSMMEM_AadharCardNo = promise.editmember[0].cmsmmeM_AadharCardNo;
                        $scope.obj.CMSMMEM_NationalSSN = promise.editmember[0].cmsmmeM_NationalSSN;
                        $scope.obj.CMSMMEM_UINo = promise.editmember[0].cmsmmeM_UINo;
                        $scope.obj.CMSMMEM_MembershipExpDate = new Date(promise.editmember[0].cmsmmeM_MembershipExpDate);
                        if (promise.editmember[0].cmsmmeM_OtherClubMemberFlg == true) {
                            $scope.obj.CMSMMEM_OtherClubMemberFlg = true;
                        }
                        else {
                            $scope.obj.CMSMMEM_OtherClubMemberFlg = false;
                        }
                        if (promise.editmember[0].cmsmmeM_BlockedFlg == true) {
                            $scope.obj.CMSMMEM_BlockedFlg = true;
                        }
                        else {
                            $scope.obj.CMSMMEM_BlockedFlg = false;
                        }
                        if (promise.editmember[0].cmsmmeM_TerminatedFlg == true) {
                            $scope.obj.CMSMMEM_TerminatedFlg = true;
                        }
                        else {
                            $scope.obj.CMSMMEM_TerminatedFlg = false;
                        }
                        $scope.obj.CMSMMEM_TerminatedReason = promise.editmember[0].cmsmmeM_TerminatedReason;
                        $scope.obj.CMSMMEM_TerminatedDate = new Date(promise.editmember[0].cmsmmeM_TerminatedDate);
                      
                        if (promise.editmember[0].cmsmmeM_LeftFlag == true) {
                            $scope.obj.CMSMMEM_LeftFlag = true;
                        }
                        else {
                            $scope.obj.CMSMMEM_LeftFlag = false;
                        }
                        $scope.obj.CMSMMEM_DOL = new Date(promise.editmember[0].cmsmmeM_DOL);
                        $scope.obj.CMSMMEM_LeavingReason = promise.editmember[0].cmsmmeM_LeavingReason;                                                                   
                        if ($scope.allCaste.length > 0) {
                            for (var i = 0; i < $scope.allCaste.length; i++) {
                                if (promise.editmember[0].imC_Id == $scope.allCaste[i].imC_Id) {
                                    $scope.allCaste[i].Selected = true;
                                    $scope.obj.iC_Id = $scope.allCaste[i];
                                    $scope.newcaste = promise.editmember[0].iC_Id;
                                }
                            }
                        }
                        $scope.obj.image = promise.editmember[0].cmsmmeM_Photo;
                        $scope.scroll();
                    }
                    else if (promise.returnval == "admin") {
                        swal('Please Contact  Administrator  !');
                    }
                    if (promise.editqulify != null && promise.editqulify.length > 0) {
                        $scope.obj.CMSMMEMQULQ_Id = promise.editqulify[0].cmsmmemqulQ_Id;
                        $scope.obj.CMSMMEMQUL_QualificationName = promise.editqulify[0].cmsmmemquL_QualificationName;
                        $scope.obj.CMSMMEMQULQ_CollegeName = promise.editqulify[0].cmsmmemqulQ_CollegeName;
                        $scope.obj.CMSMMEMQULQ_UniversityName = promise.editqulify[0].cmsmmemqulQ_UniversityName;
                        $scope.obj.CMSMMEMQULQ_YearOfPassing = promise.editqulify[0].cmsmmemqulQ_YearOfPassing;
                        $scope.obj.CMSMMEMQULQ_Country = promise.editqulify[0].cmsmmemqulQ_Country;
                        if ($scope.obj.CMSMMEMQULQ_Country > 0) {
                            $scope.QulificationState($scope.obj.CMSMMEMQULQ_Country);
                        }
                        
                        $scope.obj.CMSMMEMQULQ_State = promise.editqulify[0].cmsmmemqulQ_State;
                        $scope.obj.CMSMMEMQULQ_RegistrationNo = promise.editqulify[0].cmsmmemqulQ_RegistrationNo;
                        $scope.obj.CMSMMEMQULQ_Result = promise.editqulify[0].cmsmmemqulQ_Result;
                        $scope.obj.CMSMMEMQULQ_CGPAOrPerFlag = promise.editqulify[0].cmsmmemqulQ_CGPAOrPerFlag;
                        $scope.obj.CMSMMEMQULQ_PHDFlg = promise.editqulify[0].cmsmmemqulQ_PHDFlg;
                        $scope.obj.CMSMMEMQULQ_ThesisTitle = promise.editqulify[0].cmsmmemqulQ_ThesisTitle;
                        $scope.obj.CMSMMEMQULQ_RegistrationYear = promise.editqulify[0].cmsmmemqulQ_RegistrationYear;
                        $scope.obj.CMSMMEMQULQ_GuideName = promise.editqulify[0].cmsmmemqulQ_GuideName;
                        $scope.obj.CMSMMEMQULQ_CGPA = promise.editqulify[0].cmsmmemqulQ_CGPA;
                        $scope.obj.CMSMMEMQULQ_Percentage = promise.editqulify[0].cmsmmemqulQ_Percentage;
                        $scope.obj.CMSMMEMQULQ_AreaOfSpecialisation = promise.editqulify[0].cmsmmemqulQ_AreaOfSpecialisation;
                        $scope.obj.CMSMMEMQULQ_MedicalCouncil = promise.editqulify[0].cmsmmemqulQ_MedicalCouncil;
                        $scope.obj.CMSMMEMQULQ_Date = new Date( promise.editqulify[0].cmsmmemqulQ_Date);
                        $scope.obj.CMSMMEMQULQ_Hardcopy = promise.editqulify[0].cmsmmemqulQ_Hardcopy;
                        $scope.Qualifi = false;
                    }

                    if (promise.editexp != null && promise.editexp.length > 0) {
                        $scope.obj.CMSMMEMEXP_Id = promise.editexp[0].cmsmmemexP_Id;
                        $scope.obj.CMSMMEMEXP_OrganisationName = promise.editexp[0].cmsmmemexP_OrganisationName;
                        $scope.obj.CMSMMEMEXP_OrganisationAddress = promise.editexp[0].cmsmmemexP_OrganisationAddress;
                        $scope.obj.CMSMMEMEXP_Department = promise.editexp[0].cmsmmemexP_Department;
                        $scope.obj.CMSMMEMEXP_Designation = promise.editexp[0].cmsmmemexP_Designation;
                        $scope.obj.CMSMMEMEXP_JoinDate = new Date(promise.editexp[0].cmsmmemexP_JoinDate);
                        $scope.obj.CMSMMEMEXP_ExitDate = new Date(promise.editexp[0].cmsmmemexP_ExitDate);
                        $scope.obj.CMSMMEMEXP_NoOfYears = promise.editexp[0].cmsmmemexP_NoOfYears;
                        $scope.obj.CMSMMEMEXP_NoofMonths = promise.editexp[0].cmsmmemexP_NoofMonths;
                        $scope.obj.CMSMMEMEXP_AnnualSalary = promise.editexp[0].cmsmmemexP_AnnualSalary;
                        $scope.obj.CMSMMEMEXP_ReasonForLeaving = promise.editexp[0].cmsmmemexP_ReasonForLeaving;
                        $scope.Experience = false;
                    }
                    if (promise.editnumber != null && promise.editnumber.length > 0) {
                        $scope.obj.CMSMMEMMN_Id = promise.editnumber[0].cmsmmemmN_Id;
                        $scope.obj.CMSMMEMMN_MobileNo = promise.editnumber[0].cmsmmemmN_MobileNo;
                        $scope.obj.CMSMMEMMN_DeFaultFlag = promise.editnumber[0].cmsmmemmN_DeFaultFlag;
                        $scope.mbno = false;
                    }
                    if (promise.editemail != null && promise.editemail.length > 0) {                        
                        $scope.obj.CMSMMEMEID_Id = promise.editemail[0].cmsmmemeiD_Id;
                        $scope.obj.CMSMMEMEID_EmailId = promise.editemail[0].cmsmmemeiD_EmailId;
                        $scope.obj.CMSMMEMEID_DeFaultFlag = promise.editemail[0].cmsmmemeiD_DeFaultFlag;
                        $scope.mbemail = false;
                    }
                    if (promise.editdocument != null && promise.editdocument.length > 0) {
                        $scope.mdocument = false;
                        $scope.materaldocuupload = promise.editdocument;
                        angular.forEach($scope.materaldocuupload, function (dd) {
                            dd.VTADAAF_Remarks = dd.cmsmmemdoC_DocumentName;
                            dd.ismclT_FilePath = dd.cmsmmemdoC_FilePath;
                            dd.ismclT_FileName = dd.cmsmmemdoC_FileName;
                        });
                    }
                })
        };
        $scope.submitted2 = false;
        $scope.saveddata2 = function () {
            $scope.submitted2 = true;
          
            if ($scope.myForm2.$valid) {
                var CMSMMEMQULQ_Id = 0;
                if ($scope.obj.CMSMMEMQULQ_Id > 0) {
                    CMSMMEMQULQ_Id = $scope.obj.CMSMMEMQULQ_Id;
                }
                if ($scope.obj.CMSMMEMQULQ_CGPAOrPerFlag === true) {
                    $scope.obj.CMSMMEMQULQ_CGPAOrPerFlag = 1;
                }
                else {
                    $scope.obj.CMSMMEMQULQ_CGPAOrPerFlag = 0;
                }
                var data = {
                    "CMSMMEM_Id": $scope.obj.CMSMMEM_Id,
                    "CMSMMEMQULQ_Id": CMSMMEMQULQ_Id,
                    "CMSMMEMQULQ_Hardcopy": $scope.obj.CMSMMEMQULQ_Hardcopy,
                    "CMSMMEMQULQ_Date": new Date($scope.obj.CMSMMEMQULQ_Date).toDateString(),
                    "CMSMMEMQULQ_MedicalCouncil": $scope.obj.CMSMMEMQULQ_MedicalCouncil,
                    "CMSMMEMQULQ_AreaOfSpecialisation": $scope.obj.CMSMMEMQULQ_AreaOfSpecialisation,
                    "CMSMMEMQULQ_Percentage": $scope.obj.CMSMMEMQULQ_Percentage,
                    "CMSMMEMQULQ_CGPA": $scope.obj.CMSMMEMQULQ_CGPA,
                    "CMSMMEMQULQ_GuideName": $scope.obj.CMSMMEMQULQ_GuideName,
                    "CMSMMEMQULQ_RegistrationYear": $scope.obj.CMSMMEMQULQ_RegistrationYear,
                    "CMSMMEMQULQ_ThesisTitle": $scope.obj.CMSMMEMQULQ_ThesisTitle,
                    "CMSMMEMQULQ_PHDFlg": $scope.obj.CMSMMEMQULQ_PHDFlg,
                    "CMSMMEMQULQ_CGPAOrPerFlag": $scope.obj.CMSMMEMQULQ_CGPAOrPerFlag,
                    "CMSMMEMQULQ_Result": $scope.obj.CMSMMEMQULQ_Result,
                    "CMSMMEMQULQ_RegistrationNo": $scope.obj.CMSMMEMQULQ_RegistrationNo,
                    "CMSMMEMQULQ_Country": $scope.obj.CMSMMEMQULQ_Country,
                    "CMSMMEMQULQ_State": $scope.obj.CMSMMEMQULQ_State,
                    "CMSMMEMQULQ_YearOfPassing": $scope.obj.CMSMMEMQULQ_YearOfPassing,
                    "CMSMMEMQULQ_UniversityName": $scope.obj.CMSMMEMQULQ_UniversityName,
                    "CMSMMEMQULQ_CollegeName": $scope.obj.CMSMMEMQULQ_CollegeName,
                    "CMSMMEMQUL_QualificationName": $scope.obj.CMSMMEMQUL_QualificationName
                };
                apiService.create("CMS_Member_Master/savedetail2", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.obj.CMSMMEMQULQ_Id = promise.cmsmmemqulQ_Id;
                            $scope.Qualifi = false;
                            $scope.Experience = false;
                          
                           
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                            $scope.Qualifi = false;
                            $scope.Experience = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }


                    })

               
            }
            else {
                $scope.submitted2 = true;

            }
        };
        $scope.submitted3 = false;
        $scope.saveddata3 = function () {
            $scope.submitted3 = true;
            var CMSMMEMEXP_Id = 0;
            if ($scope.obj.CMSMMEMEXP_Id > 0) {
                CMSMMEMEXP_Id = $scope.obj.CMSMMEMEXP_Id;
            }
            if ($scope.myForm3.$valid) {

                var data = {
                    "CMSMMEMEXP_Id": CMSMMEMEXP_Id,
                    "CMSMMEM_Id": $scope.obj.CMSMMEM_Id,
                    "CMSMMEMEXP_ReasonForLeaving": $scope.obj.CMSMMEMEXP_ReasonForLeaving,
                    "CMSMMEMEXP_AnnualSalary": $scope.obj.CMSMMEMEXP_AnnualSalary,
                    "CMSMMEMEXP_NoofMonths": $scope.obj.CMSMMEMEXP_NoofMonths,
                    "CMSMMEMEXP_NoOfYears": $scope.obj.CMSMMEMEXP_NoOfYears,
                    "CMSMMEMEXP_ExitDate": new Date($scope.obj.CMSMMEMEXP_ExitDate).toDateString(),
                    "CMSMMEMEXP_JoinDate": new Date($scope.obj.CMSMMEMEXP_JoinDate).toDateString(),
                    "CMSMMEMEXP_Designation": $scope.obj.CMSMMEMEXP_Designation,
                    "CMSMMEMEXP_Department": $scope.obj.CMSMMEMEXP_Department,
                    "CMSMMEMEXP_OrganisationAddress": $scope.obj.CMSMMEMEXP_OrganisationAddress,
                    "CMSMMEMEXP_OrganisationName": $scope.obj.CMSMMEMEXP_OrganisationName

                };

                apiService.create("CMS_Member_Master/savedetail3", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                            $scope.obj.CMSMMEMEXP_Id = promise.cmsmmemexP_Id;
                            $scope.Experience = false;
                            $scope.mbno = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                           
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');

                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                            $scope.Experience = false;
                            $scope.mbno = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                           
                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }

                      
                    })
                
                
            }
            else {
                $scope.submitted3 = true;

            }
        };
        $scope.submitted4 = false;
        $scope.saveddata4 = function () {
            $scope.submitted4 = true;
            if ($scope.myForm5.$valid) {

                var data = {
                    "CMSMMEMBLK_Id": $scope.EditId,

                    "CMSMMEM_Id": $scope.obj.CMSMMEM_Id,
                    "CMSMMEMBLK_RenewalDate": $scope.obj.CMSMMEMBLK_RenewalDate,
                    "CMSMMEMBLK_ReasonForBlock": $scope.obj.CMSMMEMBLK_ReasonForBlock,
                    "CMSMMEMBLK_BlockedDate": $scope.obj.CMSMMEMBLK_BlockedDate


                };

                apiService.create("Fixing/savedetail4", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Staff Is Restricted For Selected Period !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');

                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                        // $scope.BindData();
                        //$scope.gridOptions4.data = promise.all_fix_period_staff_list;
                        $scope.gridOptions2.data = promise.Blocked
                    })
                //$scope.BindData();
                $scope.clear4();
            }
            else {
                $scope.submitted4 = true;

            }
        };
        $scope.submitted5 = false;
        $scope.saveddata5 = function () {
            $scope.submitted5 = true;
            if ($scope.myForm5.$valid) {
                var CMSMMEMMN_Id = 0;
                if ($scope.obj.CMSMMEMMN_Id > 0) {
                    CMSMMEMMN_Id = $scope.obj.CMSMMEMMN_Id;
                }
                if ($scope.obj.CMSMMEMMN_DeFaultFlag == '1') {
                    $scope.obj.CMSMMEMMN_DeFaultFlag = true;
                }
                else {
                    $scope.obj.CMSMMEMMN_DeFaultFlag = false;
                }

                var data = {
                    "CMSMMEMMN_Id": CMSMMEMMN_Id,
                    "CMSMMEM_Id": $scope.obj.CMSMMEM_Id,
                    "CMSMMEMMN_MobileNo": $scope.obj.CMSMMEMMN_MobileNo,
                    "CMSMMEMMN_DeFaultFlag": $scope.obj.CMSMMEMMN_DeFaultFlag

                }
                apiService.create("CMS_Member_Master/savedetail5", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                            $scope.obj.CMSMMEMMN_Id = promise.cmsmmemmN_Id;
                            $scope.mbno = false;
                            $scope.mbemail = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;

                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                            $scope.mbno = false;
                            $scope.mbemail = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                       // $scope.gridOptions5.data = promise.Mobile_list;
                    })
                
            }
            else {
                $scope.submitted5 = true;

            }
        };
        $scope.submitted6 = false;
        $scope.saveddata6 = function () {
            $scope.submitted6 = true;
            if ($scope.myForm6.$valid) {
                var CMSMMEMEID_Id = 0;
                if ($scope.obj.CMSMMEMEID_Id > 0) {
                    CMSMMEMEID_Id = $scope.obj.CMSMMEMEID_Id;
                }
                if ($scope.obj.CMSMMEMEID_DeFaultFlag === true) {
                    $scope.obj.CMSMMEMEID_DeFaultFlag = 1;
                }
                else {
                    $scope.obj.CMSMMEMEID_DeFaultFlag = 0;
                }
                var data = {
                    "CMSMMEMEID_Id": CMSMMEMEID_Id,
                    "CMSMMEM_Id": $scope.obj.CMSMMEM_Id,
                    "CMSMMEMEID_EmailId": $scope.obj.CMSMMEMEID_EmailId,
                    "CMSMMEMEID_DeFaultFlag": $scope.obj.CMSMMEMEID_DeFaultFlag

                }
                apiService.create("CMS_Member_Master/savedetail6", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                            $scope.obj.CMSMMEMEID_Id = promise.cmsmmemeiD_Id;
                            $scope.mbemail = false;
                            $scope.mdocument = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;

                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                            $scope.mbemail = false;
                            $scope.mdocument = false;
                            $scope.myTabIndex = $scope.myTabIndex + 1;

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                       

                    })
               
            }
            else {
                $scope.submitted6 = true;

            }
        };
        $scope.submitted7 = false;
        $scope.saveddata7 = function () {
            $scope.submitted7 = true;
            $scope.uploadarray = [];
            if ($scope.myForm7.$valid) {               
                if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.ismclT_FilePath !== undefined && itm.ismclT_FilePath !== null && itm.ismclT_FilePath !== "") {
                            $scope.uploadarray.push({
                                CMSMMEMDOC_FileName: itm.vtadaaF_FileName,
                                CMSMMEMDOC_FilePath: itm.ismclT_FilePath,
                                CMSMMEMDOC_DocumentName: itm.VTADAAF_Remarks
                            });
                        }
                    });
                }
                var data = {                   
                    "CMSMMEM_Id": $scope.obj.CMSMMEM_Id,
                    "documents": $scope.uploadarray,                   
                }
                apiService.create("CMS_Member_Master/savedetail7", data).
                    then(function (promise) {
                                       
                        if (promise.returnval == "save") {
                            swal('Record Saved /  Updated Successfully !');

                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                    })           
            }
            else {
                $scope.submitted7 = true;

            }
        };  
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {

            var data = {

                "cmsmmeM_Id": item.cmsmmeM_Id
            }
            var dystring = "";
            if (item.cmsmmeM_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.cmsmmeM_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("CMS_Member_Master/deactive", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            $scope.submitted7 = true;
            if ($scope.myForm7.$valid) {
                if ($scope.materaldocuupload.length > 1) {
                    for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                        var id = $scope.materaldocuupload[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });

            }
            

        };
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

        };    
        //TO clear  data
        $scope.clearid = function () {
            $scope.CMSMMEM_Id = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";

        };
        //interacted6
        $scope.interacted6 = function (field) {

            return $scope.submitted6 || field.$dirty;
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted1 || field.$dirty;
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
        $scope.interacted7 = function (field) {

            return $scope.submitted7 || field.$dirty;
        };
        //TO clear  data
        $scope.clear1 = function () {

            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
            $state.reload();

        };
        $scope.clear2 = function () {


            $scope.submitted2 = false;

            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
            $state.reload();
        };
        $scope.clear3 = function () {
            $scope.submitted3 = false;

            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
            $scope.search = "";
           

        };
        $scope.clear4 = function () {


            $scope.submitted4 = false;
            $scope.asmcL_Id4 = "";
            $scope.asmS_Id4 = "";
            $scope.hrmE_Id4 = "";
            $scope.NOD_4 = 0;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
            $scope.search = "";
            $state.reload();
        };
        $scope.clear6 = function () {
            $scope.submitted6 = false;
            $scope.myForm6.$setPristine();
            $scope.myForm6.$setUntouched();
            $scope.search = "";
            $state.reload();
        }
        $scope.clear5 = function () {
            $scope.submitted5 = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
            $scope.search = "";
            $state.reload();
        };
        $scope.clear7 = function () {
            $scope.submitted7 = false;
            $scope.myForm7.$setPristine();
            $scope.myForm7.$setUntouched();
            $scope.search = "";
            $state.reload();
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vtadaA_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 5242880) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("Document Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.ismclT_FilePath = d;
                    data.vtadaaF_FileName = $scope.vtadaA_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };

        //cms month
        $scope.previewimg_new = function (img) {
            $scope.ismclT_FilePath = img;
            var img = $scope.ismclT_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclT_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ismclT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismclT_FilePath;
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
            }
            else {
                $window.open($scope.ismclT_FilePath)
            }
        };
        //added by photo
        $scope.UploadStudentProfilePicT = [];

        $scope.uploadStudentProfilePic = function (input, document) {

            $scope.UploadStudentProfilePicT = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();

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
        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadStudentProfilePicT.length; i++) {
                formData.append("File", $scope.UploadStudentProfilePicT[i]);
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

                    $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }
    }

})();