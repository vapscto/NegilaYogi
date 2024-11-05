(function () {
    'use strict';
    angular.module('app').controller('EmployeeProfileUpdateController', EmployeeProfileUpdateController);
    EmployeeProfileUpdateController.$inject = ['$window', '$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce'];
    function EmployeeProfileUpdateController($window, $rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.obj = {};
        $scope.loaddataprofileupdate = function () {

            var pageid = 2;
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;

            apiService.getURI("EmployeeProfileUpdateApproval/loaddataprofileupdate", pageid).then(function (promise) {

                if (promise !== null) {

                    $scope.hrmE_Id = promise.hrmE_Id;
                    $scope.GetMartialStatusList = promise.getMartialStatusList;
                    $scope.GetGenderList = promise.getGenderList;
                    $scope.GetCountryList = promise.getCountryList;
                    $scope.GetReligionList = promise.getReligionList;
                    $scope.GetCasteCategoryList = promise.getCasteCategoryList;
                    $scope.GetCasteList = promise.getCasteList;
                    $scope.GetLocalStateList = promise.getLocalStateList;
                    $scope.GetPerStateList = promise.getPerStateList;

                    $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                    $scope.GetEmployeEmailIdDetails = promise.getEmployeEmailIdDetails;
                    $scope.GetEmployeMobileNoDetails = promise.getEmployeMobileNoDetails;

                    $scope.obj.EmployeeFirstName = $scope.GetEmployeeDetails[0].hrmereQ_EmployeeFirstName;
                    $scope.obj.EmployeeMiddleName = $scope.GetEmployeeDetails[0].hrmereQ_EmployeeMiddleName;
                    $scope.obj.EmployeeLastName = $scope.GetEmployeeDetails[0].hrmereQ_EmployeeLastName;
                    $scope.obj.IVRMMMS_Id = $scope.GetEmployeeDetails[0].ivrmmmS_Id;
                    $scope.obj.IVRMMG_Id = $scope.GetEmployeeDetails[0].ivrmmG_Id;
                    $scope.obj.BloodGroup = $scope.GetEmployeeDetails[0].hrmereQ_BloodGroup;
                    $scope.obj.FatherName = $scope.GetEmployeeDetails[0].hrmereQ_FatherName;
                    $scope.obj.MotherName = $scope.GetEmployeeDetails[0].hrmereQ_MotherName;

                    $scope.obj.religionId = $scope.GetEmployeeDetails[0].religionId;
                    $scope.obj.CasteCategoryId = $scope.GetEmployeeDetails[0].casteCategoryId;
                    $scope.obj.casteId = $scope.GetEmployeeDetails[0].casteId;
                    $scope.obj.Photo = $scope.GetEmployeeDetails[0].hrmereQ_Photo;

                    $scope.obj.DOB = new Date($scope.GetEmployeeDetails[0].hrmereQ_DOB);

                    $('#blah').attr('src', $scope.GetEmployeeDetails[0].hrmereQ_Photo);

                    $scope.mobilesstd = [];
                    if ($scope.GetEmployeMobileNoDetails !== null && $scope.GetEmployeMobileNoDetails.length > 0) {
                        angular.forEach($scope.GetEmployeMobileNoDetails, function (dd) {
                            var id = 0;
                            if (dd.hrmereqmN_Id !== undefined && dd.hrmereqmN_Id !== null) {
                                id = dd.hrmereqmN_Id;
                            }
                            $scope.mobilesstd.push({ MobileNo: dd.hrmereqmN_MobileNo, DeFaultFlag: dd.hrmereqmN_Flag, HRMEREQMN_Id: id });
                        });
                    } else {
                        $scope.mobilesstd = [{ id: 'mobilestd1' }];
                    }

                    $scope.emailsstd = [];
                    if ($scope.GetEmployeEmailIdDetails !== null && $scope.GetEmployeEmailIdDetails.length > 0) {
                        angular.forEach($scope.GetEmployeEmailIdDetails, function (dd) {
                            var id = 0;
                            if (dd.hrmereqeM_Id !== undefined && dd.hrmereqeM_Id !== null) {
                                id = dd.hrmereqeM_Id;
                            }
                            $scope.emailsstd.push({ EmailId: dd.hrmereqeM_EmailId, DeFaultFlag: dd.hrmereqeM_Flag, HRMEREQEM_Id: id });
                        });
                    } else {
                        $scope.emailsstd = [{ id: 'emailsstd1' }];
                    }

                    $scope.obj.PerStreet = $scope.GetEmployeeDetails[0].hrmereQ_PerStreet;
                    $scope.obj.PerArea = $scope.GetEmployeeDetails[0].hrmereQ_PerArea;
                    $scope.obj.PerCountryId = $scope.GetEmployeeDetails[0].hrmereQ_PerCountryId;
                    $scope.obj.PerCity = $scope.GetEmployeeDetails[0].hrmereQ_PerCity;
                    $scope.obj.PerPincode = $scope.GetEmployeeDetails[0].hrmereQ_PerPincode;
                    $scope.obj.PerStateId = $scope.GetEmployeeDetails[0].hrmereQ_PerStateId;

                    $scope.obj.LocStreet = $scope.GetEmployeeDetails[0].hrmereQ_LocStreet;
                    $scope.obj.LocArea = $scope.GetEmployeeDetails[0].hrmereQ_LocArea;
                    $scope.obj.LocCountryId = $scope.GetEmployeeDetails[0].hrmereQ_LocCountryId;
                    $scope.obj.LocCity = $scope.GetEmployeeDetails[0].hrmereQ_LocCity;
                    $scope.obj.LocPincode = $scope.GetEmployeeDetails[0].hrmereQ_LocPincode;
                    $scope.obj.LocStateId = $scope.GetEmployeeDetails[0].hrmereQ_LocStateId;

                    $scope.obj.SpouseName = $scope.GetEmployeeDetails[0].hrmereQ_SpouseName;
                    $scope.obj.SpouseOccupation = $scope.GetEmployeeDetails[0].hrmereQ_SpouseOccupation;
                    $scope.obj.SpouseMobileNo = $scope.GetEmployeeDetails[0].hrmereQ_SpouseMobileNo;
                    $scope.obj.SpouseEmailId = $scope.GetEmployeeDetails[0].hrmereQ_SpouseEmailId;
                    $scope.obj.SpouseAddress = $scope.GetEmployeeDetails[0].hrmereQ_SpouseAddress;

                    $scope.ShowSpouseDetails = false;
                    var single_object = $filter('filter')($scope.GetMartialStatusList, function (d) {
                        return d.ivrmmmS_Id === parseInt($scope.obj.IVRMMMS_Id);
                    })[0].ivrmmmS_MaritalStatus;

                    if (single_object.toUpperCase() === 'MARRIED') {
                        $scope.ShowSpouseDetails = true;
                    }

                    if (promise.hrmereQ_Id !== null && promise.hrmereQ_Id > 0) {
                        $scope.HRMEREQ_Id = promise.hrmereQ_Id;
                    }

                    $scope.GetRequestedData = promise.getRequestedData;
                };
            });
        };

        $scope.setMaritalStatus = function (ivrmmmS_Id) {
            if (ivrmmmS_Id != undefined && ivrmmmS_Id != "") {
                var single_object = $filter('filter')($scope.GetMartialStatusList, function (d) {
                    return d.ivrmmmS_Id === parseInt(ivrmmmS_Id);
                })[0].ivrmmmS_MaritalStatus;

                if (single_object.toUpperCase() === 'MARRIED') {
                    $scope.ShowSpouseDetails = true;
                } else {
                    $scope.ShowSpouseDetails = false;
                }
            } else {
                $scope.ShowSpouseDetails = false;
            }
        };

        $scope.getcastecatgory = function (religionId) {
            $scope.GetCasteCategoryList = [];
            $scope.GetCasteList = [];

            $scope.obj.CasteCategoryId = "";
            $scope.obj.casteId = "";

            var data = {
                "ReligionId": religionId
            };
            apiService.create("EmployeeProfileUpdateApproval/Getcastecatgory", data).then(function (promise) {
                if (promise != null) {
                    if (promise.getCasteCategoryList !== null && promise.getCasteCategoryList.length > 0) {
                        $scope.GetCasteCategoryList = promise.getCasteCategoryList;
                    }
                    else {
                        swal("No CasteCategory is mapped to selected Religion");
                        $scope.GetCasteCategoryList = [];
                    }
                }
                else {
                    swal("No CasteCategory is mapped to selected Religion");
                    $scope.GetCasteCategoryList = [];
                }
            });
        };

        $scope.getcaste = function (imcC_Id) {
            $scope.GetCasteList = [];
            $scope.obj.casteId = "";

            var data = {
                "CasteCategoryId": imcC_Id
            };
            apiService.create("EmployeeProfileUpdateApproval/Getcaste", data).then(function (promise) {
                if (promise != null) {
                    if (promise.getCasteList !== null && promise.getCasteList.length > 0) {
                        $scope.GetCasteList = promise.getCasteList;
                        $scope.castedisble = false;
                    }
                    else {
                        swal("No Caste is mapped to selected Caste Category");
                        $scope.GetCasteList = [];
                    }
                }
                else {
                    swal("No Caste is mapped to selected Caste Category");
                    $scope.GetCasteList = [];
                }
            });
        };

        $scope.SaveData = function (dd) {

            $scope.submitted = false;
            $scope.mobilenotemp = [];
            $scope.emailidtemp = [];

            if ($scope.myForm1.$valid) {
                swal({
                    title: "Do You Want To Update The Details",
                    text: "Do you want to continue !!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue!",
                    cancelButtonText: "Cancel!!",
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        angular.forEach($scope.mobilesstd, function (dd) {
                            if (dd.MobileNo !== null && dd.MobileNo !== "") {
                                var mobilenoid = 0;
                                if (dd.HRMEREQMN_Id !== undefined && dd.HRMEREQMN_Id !== null && dd.HRMEREQMN_Id !== "") {
                                    mobilenoid = dd.HRMEREQMN_Id;
                                }
                                $scope.mobilenotemp.push({ HRMEREQMN_Id: mobilenoid, HRMEREQMN_MobileNo: dd.MobileNo, HRMEREQMN_Flag: dd.DeFaultFlag });
                            }
                        });

                        angular.forEach($scope.emailsstd, function (dd) {
                            if (dd.EmailId !== null && dd.EmailId !== "") {
                                var emailidid = 0;
                                if (dd.HRMEREQMN_Id !== undefined && dd.HRMEREQMN_Id !== null && dd.HRMEREQMN_Id !== "") {
                                    emailidid = dd.HRMEREQMN_Id;
                                }
                                $scope.emailidtemp.push({ HRMEREQEM_Id: emailidid, HRMEREQEM_EmailId: dd.EmailId, HRMEREQEM_Flag: dd.DeFaultFlag });
                            }
                        });


                        var data = {
                            "HRME_Id": $scope.hrmE_Id,
                            "HRMEREQ_Id": $scope.HRMEREQ_Id !== undefined && $scope.HRMEREQ_Id !== null && $scope.HRMEREQ_Id !== "" ? $scope.HRMEREQ_Id : 0,
                            "HRMEREQ_EmployeeFirstName": $scope.obj.EmployeeFirstName,
                            "HRMEREQ_EmployeeMiddleName": $scope.obj.EmployeeMiddleName,
                            "HRMEREQ_EmployeeLastName": $scope.obj.EmployeeLastName,
                            "HRMEREQ_PerStreet": $scope.obj.PerStreet,
                            "HRMEREQ_PerArea": $scope.obj.PerArea,
                            "HRMEREQ_PerCity": $scope.obj.PerCity,
                            "HRMEREQ_PerStateId": $scope.obj.PerStateId,
                            "HRMEREQ_PerCountryId": $scope.obj.PerCountryId,
                            "HRMEREQ_PerPincode": $scope.obj.PerPincode,
                            "HRMEREQ_LocStreet": $scope.obj.LocStreet,
                            "HRMEREQ_LocArea": $scope.obj.LocArea,
                            "HRMEREQ_LocCity": $scope.obj.LocCity,
                            "HRMEREQ_LocStateId": $scope.obj.LocStateId,
                            "HRMEREQ_LocCountryId": $scope.obj.LocCountryId,
                            "HRMEREQ_LocPincode": $scope.obj.LocPincode,
                            "IVRMMMS_Id": $scope.obj.IVRMMMS_Id,
                            "IVRMMG_Id": $scope.obj.IVRMMG_Id,
                            "CasteCategoryId": $scope.obj.CasteCategoryId,
                            "CasteId": $scope.obj.casteId,
                            "ReligionId": $scope.obj.religionId,
                            "HRMEREQ_FatherName": $scope.obj.FatherName,
                            "HRMEREQ_MotherName": $scope.obj.MotherName,
                            "HRMEREQ_SpouseName": $scope.obj.SpouseName,
                            "HRMEREQ_SpouseOccupation": $scope.obj.SpouseOccupation,
                            "HRMEREQ_SpouseMobileNo": $scope.obj.SpouseMobileNo,
                            "HRMEREQ_SpouseEmailId": $scope.obj.SpouseEmailId,
                            "HRMEREQ_SpouseAddress": $scope.obj.SpouseAddress,
                            "HRMEREQ_DOB": new Date($scope.obj.DOB).toDateString(),
                            "HRMEREQ_BloodGroup": $scope.obj.BloodGroup,
                            "HRMEREQ_Photo": $scope.obj.Photo,
                            "Temp_MobileNo": $scope.mobilenotemp,
                            "Temp_EmailId": $scope.emailidtemp
                        };

                        apiService.create("EmployeeProfileUpdateApproval/SaveData", data).then(function (promise) {

                            if (promise !== null) {

                                if (promise.message === "Add") {
                                    if (promise.returnval === true) {
                                        swal("Record Saved Successfully, For Approval Contact Administrator");
                                    } else {
                                        swal("Failed To Save Record");
                                    }
                                } else if (promise.message === "Update") {
                                    if (promise.returnval === true) {
                                        swal("Record Updated Successfully, For Approval Contact Administrator");
                                    } else {
                                        swal("Failed To Update Record");
                                    }
                                } else {
                                    swal("Failed To Save/Update Record");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Request Cancelled");
                    }
                });              
            } else {
                $scope.submitted = true;
            }
        };


        $scope.mobilesstd = [{ id: 'mobilestd1' }];
        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.mobilesstd.length + 1;
            if (newItemNostd <= 5) {
                $scope.mobilesstd.push({ 'id': 'mobilestd' + newItemNostd });
            }

            if (newItemNostd == 4) {
                $scope.myForm1.$setPristine();
            }
        };

        $scope.delmsrd = [];
        $scope.removeNewMobile1std = function (index, curval1std) {
            var newItemNostd2 = $scope.mobilesstd.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.mobilesstd.splice(index, 1);
            }
        };

        $scope.chkdup_mob = function (user, index) {
            if (user.hrmemnO_MobileNo != "" && user.hrmemnO_MobileNo != undefined && user.hrmemnO_MobileNo != null) {
                for (var k = 0; k < $scope.mobilesstd.length; k++) {
                    var roll = parseInt(user.hrmemnO_MobileNo);
                    var arryind = $scope.mobilesstd.indexOf($scope.mobilesstd[k]);
                    if (arryind != index) {
                        if ($scope.mobilesstd[k].hrmemnO_MobileNo == roll) {
                            swal("Mobile Number Already Chosen");
                            $scope.mobilesstd[index].hrmemnO_MobileNo = "";
                            break;
                        }
                    }
                }
            }
        };

        $scope.defchange_mob = function (moM_Flag) {
            var newItemNo1 = $scope.mobilesstd.length;
            for (var i = 0; i < newItemNo1; i++) {
                if ($scope.mobilesstd[i].hrmemnO_DeFaultFlag == "default") {
                    $scope.disabled = true;
                    break;
                }
                else {
                    $scope.disabled = false;
                }
            }
        };

        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd' + newItemNostd2 });
            }
        };

        $scope.removeNewEmail1std = function (index, id) {
            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);
            }
        };

        $scope.chkdup_email = function (user, index) {
            if ($scope.hrmeM_EmailId != null || $scope.hrmeM_EmailId != "" || $scope.hrmeM_EmailId != undefined) {
                for (var k = 0; k < $scope.emailsstd.length; k++) {
                    var roll = user.hrmeM_EmailId;
                    var arryind = $scope.emailsstd.indexOf($scope.emailsstd[k]);
                    if (arryind != index) {
                        if ($scope.emailsstd[k].hrmeM_EmailId == roll) {
                            swal("Eamil_Id Already Chosen");
                            break;
                        }
                    }
                }
            }
        };

        $scope.defchange_email = function (moM_Flag) {

            var newItemNo2 = $scope.emailsstd.length;
            for (var i = 0; i < newItemNo2; i++) {
                if ($scope.emailsstd[i].hrmeM_DeFaultFlag == "default") {
                    $scope.disabled2 = true;
                    break;
                }
                else {
                    $scope.disabled2 = false;
                }
            }
        };

        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };

        $scope.interacted1 = function () {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.UploadEmployeeProfilePic = [];

        $scope.uploadEmployeeProfilePic = function (input, document) {
            $scope.UploadEmployeeProfilePic = input.files;
            $('#blah').removeAttr('src');

            if (input.files && input.files[0]) {

                if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/jpg" || input.files[0].type == "image/png")
                    && input.files[0].size <= 2097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah').attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofile();
                }
                else if (input.files[0].type != "image/jpeg" && input.files[0].type != "image/jpg" && input.files[0].type != "image/png") {
                    swal("Please Upload the Image file");
                    angular.element("input[type='file']").val(null);
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    angular.element("input[type='file']").val(null);
                    return;
                }
            }
        };

        function UploadEmployeeprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadEmployeeProfilePic.length; i++) {
                formData.append("File", $scope.UploadEmployeeProfilePic[i]);
            }                      
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/EmployeeDocumentUpload/UploadEmployeeprofilepic", formData,{
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).success(function (d) {
                    defer.resolve(d);
                    if (d != "PathNotFound") {
                        $scope.obj.Photo = d;
                    } else {
                        swal('File Storage Path Not Found ..!!');
                        angular.element("input[type='file']").val(null);
                    }
                }).error(function () {
                    $('#blah').removeAttr('src');
                    defer.reject("File Upload Failed!");
                    angular.element("input[type='file']").val(null);
                });
        }
    }
})();
