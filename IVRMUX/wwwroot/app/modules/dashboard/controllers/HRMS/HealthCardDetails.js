(function () {
    'use strict';
    angular.module('app').controller('HealthCardDetailsController', HealthCardDetailsController)
    HealthCardDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce']
    function HealthCardDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.obj = {};
        $scope.getreport = [];
        $scope.Yes = true;
        $scope.submitted = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.search = "";
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("HealthCardDetails/loaddata", pageid).then(function (promise) {
                $scope.getemployeelist = promise.getemployeelist;
                $scope.getsavedetails = promise.getsavedetails;
                $scope.getreport = promise.getreport;
            });
        };
        $scope.clikone = function () {
            if ($scope.Yes === true) {
                $scope.No = false;

            }
            else {
                $scope.No = true;
            }
        };

        $scope.clicktwo = function () {
            if ($scope.No === true) {
                $scope.Yes = false;

            }
            else {
                $scope.Yes = true;

            }
        };

        $scope.SaveDetails = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid && $scope.obj.HMTPD_Id > 0) {
                var data = {
                    "HMTPD_Id": $scope.obj.HMTPD_Id ,
                    "HMTRSCD_MemberId": $scope.HMTRSCD_MemberId,
                    "HMTRSCD_CompanyName": $scope.HMTRSCD_CompanyName,
                    "HMTRSCD_DOB": new Date($scope.obj.HMTRSCD_DOB).toDateString(),
                    "HMTRSCD_Patientname": $scope.HMTRSCD_Patientname,
                    "HMTRSCD_Gender": $scope.HMTRSCD_Gender,
                    "HMTRSCD_PatientPhNo": $scope.HMTRSCD_PatientPhNo,
                    "HMTRSCD_DateOfTreatment": new Date($scope.obj.HMTRSCD_DateOfTreatment).toDateString(),
                    "HMTRSCD_DateOfAdmission": new Date($scope.obj.HMTRSCD_DateOfAdmission).toDateString(),
                    "HMTRSCD_DateOfDischarge": new Date($scope.obj.HMTRSCD_DateOfDischarge).toDateString(),
                    "HMTRSCD_Symptomspresented": $scope.HMTRSCD_Symptomspresented,
                    "HMTRSCD_Occupation": $scope.HMTRSCD_Occupation,
                    "HMTRSCD_NameofHospital": $scope.HMTRSCD_NameofHospital,
                    "HMTRSCD_RoomCategory": $scope.HMTRSCD_RoomCategory,
                    "HMTRSCD_hospitalizationexpenses": $scope.HMTRSCD_hospitalizationexpenses,
                    "HMTRSCD_Address": $scope.HMTRSCD_Address,
                    "HMTRSCD_Pincode": $scope.HMTRSCD_Pincode,
                    "HMTRSCD_EmailId": $scope.HMTRSCD_EmailId,
                    "HMTRSCD_ClaimDocFilePath": $scope.hmtrscD_ClaimDocFilePath,
                    "HMTRSCD_RClaimNo": $scope.HMTRSCD_RClaimNo,
                    "HMTRSCD_SumOfInsuredAmt": $scope.HMTRSCD_SumOfInsuredAmt,
                    "HMTRSCD_RCCompanyName": $scope.HMTRSCD_RCCompanyName,
                    "HMTRSCD_Diagnosis": $scope.HMTRSCD_Diagnosis,
                    "HMTRSCD_RlptoPrimaryinsured": $scope.HMTRSCD_RlptoPrimaryinsured,
                    "HMTRSCD_Id": $scope.obj.HMTRSCD_Id
                };
                apiService.create("HealthCardDetails/SaveDetails", data).then(function (promise) {
                    if (promise.return_val === "RecordExist") {
                        swal("Record Already Exist");
                    }
                    if (promise.return_val === "save") {
                        swal("Record Saved Successfully");
                    }
                    if (promise.return_val === "Nosave") {
                        swal("Record Not  Saved !");
                    }
                    if (promise.return_val === "Update") {
                        swal("Record Update Successfully");
                    }
                    //Notupdate
                    if (promise.return_val === "Notupdate") {
                        swal("Record Not Updated !");
                    }
                    if (promise.return_val === "") {
                        swal("Please Contact Administrator !");
                    }
                    $state.reload();

                });
            }
        };
        $scope.edit = function (item) {
            var data = {
                "HMTRSCD_Id": item.hmtrscD_Id
            };

            apiService.create("HealthCardDetails/editmaster", data).then(function (promise) {
                $scope.HRME_Id = promise.editarray[0];
                $scope.HRME_Id.hrmE_Id = promise.editarray[0].hrmE_Id;
                $scope.obj.HMTRSCD_Id = promise.editarray[0].hmtrscD_Id;
                $scope.HMTRSCD_MemberId = promise.editarray[0].hmtrscD_MemberId;
                $scope.HMTRSCD_CompanyName = promise.editarray[0].hmtrscD_CompanyName;
                $scope.obj.HMTRSCD_DOB = new Date(promise.editarray[0].hmtrscD_DOB);
                $scope.HMTRSCD_Patientname = promise.editarray[0].hmtrscD_Patientname;
                $scope.HMTRSCD_Gender = promise.editarray[0].hmtrscD_Gender;
                $scope.HMTRSCD_PatientPhNo = promise.editarray[0].hmtrscD_PatientPhNo;
                $scope.obj.HMTRSCD_DateOfTreatment = new Date(promise.editarray[0].hmtrscD_DateOfTreatment);
                $scope.obj.HMTRSCD_DateOfAdmission = new Date(promise.editarray[0].hmtrscD_DateOfAdmission);
                $scope.obj.HMTRSCD_DateOfDischarge = new Date(promise.editarray[0].hmtrscD_DateOfDischarge);
                $scope.HMTRSCD_Symptomspresented = promise.editarray[0].hmtrscD_Symptomspresented;
                $scope.HMTRSCD_RlptoPrimaryinsured = promise.editarray[0].hmtrscD_RlptoPrimaryinsured;
                $scope.HMTRSCD_Occupation = promise.editarray[0].hmtrscD_Occupation;
                $scope.HMTRSCD_NameofHospital = promise.editarray[0].hmtrscD_NameofHospital;
                $scope.HMTRSCD_RoomCategory = promise.editarray[0].hmtrscD_RoomCategory;
                $scope.HMTRSCD_hospitalizationexpenses = promise.editarray[0].hmtrscD_hospitalizationexpenses;
                $scope.HMTRSCD_Address = promise.editarray[0].hmtrscD_Address;
                $scope.HMTRSCD_Pincode = promise.editarray[0].hmtrscD_Pincode;
                $scope.HMTRSCD_EmailId = promise.editarray[0].hmtrscD_EmailId;
                $scope.HMTRSCD_ClaimDocFilePath = promise.editarray[0].hmtrscD_ClaimDocFilePath;
                $scope.HMTRSCD_CurrentlyCoveredInsuranceFlag = promise.editarray[0].hmtrscD_CurrentlyCoveredInsuranceFlag;
                $scope.HMTRSCD_RClaimNo = promise.editarray[0].hmtrscD_RClaimNo;
                $scope.HMTRSCD_SumOfInsuredAmt = promise.editarray[0].hmtrscD_SumOfInsuredAmt;
                $scope.HMTRSCD_RCCompanyName = promise.editarray[0].hmtrscD_RCCompanyName;
                $scope.HMTRSCD_Diagnosis = promise.editarray[0].hmtrscD_Diagnosis;
               // $scope.obj.HMTPD_Id = promise.editarray[0].hmtpD_Id;
                $scope.OnChangeEmployee();

            });
        };
        $scope.Deletedata = function (item, SweetAlert) {

            var data = {
                "HMTRSCD_Id": item.hmtrscD_Id

            }
            var dystring = "";
            if (item.hmtrscD_ActiveFLag == true) {
                dystring = "Deactivate";
            }
            else if (item.hmtrscD_ActiveFLag == false) {
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
                        apiService.create("HealthCardDetails/deactiveM", data).
                            then(function (promise) {
                                if (promise.return_val === "delete") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.loaddata();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.OnChangeEmployee = function () {
                                 
            angular.forEach($scope.getemployeelist, function (itm) {
                if (itm.hrmE_Id !== undefined && itm.hrmE_Id !== null && itm.hrmE_Id !== "" && itm.hrmE_Id == $scope.HRME_Id.hrmE_Id) {
                    $scope.obj.HMTPD_Id= itm.hmtpD_Id
                }
            });
            angular.forEach($scope.getreport, function (itm) {
                if (itm.hrmE_Id !== undefined && itm.hrmE_Id !== null && itm.hrmE_Id !== "" && itm.hrmE_Id == $scope.HRME_Id.hrmE_Id) {
                    $scope.obj.HMTPD_Id = itm.hmtpD_Id
                }
            });
            //var data = {
            //    "HRME_Id": $scope.HRME_Id.hrmE_Id
            //};
            //apiService.create("HealthCardDetails/OnChangeEmployee", data).then(function (promise) {

            //});
        };
        $scope.Clearid = function () {
            $state.reload();
            // $scope.loaddata();
        };
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;
            $scope.reverse = !$scope.reverse;
        };
        //edit

        $scope.uploadtecherdocuments1 = [];
        $scope.uploadtecherdocuments = function (input, document) {
            $scope.uploadtecherdocuments1 = input.files;
            $scope.filename = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" ||
                    input.files[0].type === "image/jpg") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploaddianPhoto(data) {
            console.log("Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadEmployeeDocuments", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.hmtrscD_ClaimDocFilePath = d;
                    $scope.HREREM_FileName = $scope.filename;
                    $('#').attr('src', $scope.HREREM_FilePath);
                    var img = $scope.HREREM_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.filetype = lastelement;
                    console.log("data.filetype : " + $scope.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.HREREM_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });
})();

