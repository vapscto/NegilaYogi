(function () {
    'use strict';
    angular.module('app').controller('HealthCardMASTERController', HealthCardMASTERController)
    HealthCardMASTERController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function HealthCardMASTERController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

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
                $scope.getemployeelist = promise.getemployeelistttt;
                $scope.getreport = promise.getreport;

            });
        };
        $scope.submitted = false;
        $scope.SaveDetails = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "HMTPD_MemberId": $scope.HMTPD_MemberId,
                    "HMTPD_PolicyName": $scope.HMTPD_PolicyName,
                    "HMTPD_PlanName": $scope.HMTPD_PlanName,
                    "HMTPD_PolicyProvider": $scope.HMTPD_PolicyProvider,
                    "HMTPD_PlanStartDate": new Date($scope.HMTPD_PlanStartDate).toDateString(),
                    "HMTPD_PlanEndDate": new Date($scope.HMTPD_PlanEndDate).toDateString(),
                    "HMTPD_Id": $scope.HMTPD_Id
                };
                apiService.create("HealthCardDetails/Savemaster", data).then(function (promise) {
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
        $scope.edit = function (user) {
            $scope.HMTPD_Id = user.hmtpD_Id;
            $scope.HMTPD_PlanEndDate = new Date(user.hmtpD_PlanEndDate);
            $scope.HMTPD_PlanStartDate = new Date(user.hmtpD_PlanStartDate);
            $scope.HMTPD_PolicyProvider = user.hmtpD_PolicyProvider;
            $scope.HMTPD_PlanName = user.hmtpD_PlanName;
            $scope.HMTPD_PolicyName = user.hmtpD_PolicyName;
            $scope.HMTPD_MemberId = user.hmtpD_MemberId;
            $scope.HRME_Id = user;
            $scope.HRME_Id.hrmE_Id = user.hrmE_Id;
            //var data = {

            //};
            //apiService.create("HealthCardDetails/editmaster", data).then(function (promise) {

            //});
        };
        $scope.Deletedata = function (item, SweetAlert) {
            // $scope.HMTPD_Id = item.hmtpD_Id;
            var data = {
                "HMTPD_Id": item.hmtpD_Id

            }
            var dystring = "";
            if (item.hmtpD_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.hmtpD_ActiveFlag == false) {
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
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;
            $scope.reverse = !$scope.reverse;
        };
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
            console.log("Teacher Upload  :" + data);
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
                    $scope.HREREM_FilePath = d;
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

