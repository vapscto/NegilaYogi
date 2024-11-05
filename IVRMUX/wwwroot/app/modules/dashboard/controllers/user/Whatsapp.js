(function () {
    'use strict';
    angular.module('app').controller('WhatsappController', WhatsappController)

    //WhatsappController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter'];
    //function WhatsappController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter) {

    WhatsappController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$compile'];
    function WhatsappController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $compile) {
        $scope.submitted = false;

        $scope.obj = {};
      
        var vm = this;
        vm.gridOptions = {};
        $scope.tempemail = [];
        $scope.savemsg1 = function () {

            if ($scope.myForm.$valid) {
                var data = {

                    "message": "testing",
                    "MobileNumber": $scope.MobileNumber,
                    "Typedropdown": $scope.type,
                    "BirthdayName": $scope.obj.BirthdayName,
                    "InstituteNameB": $scope.obj.InstituteNameB,
                    "Attendance1": $scope.obj.Attendance1,
                    "Attendance2": $scope.obj.Attendance2,
                    "Attendance3": $scope.obj.Attendance3,
                    "Attendance4": $scope.obj.Attendance4,
                    "Attendance5": $scope.obj.Attendance5,
                    "FeeDue": $scope.obj.FeeDue,
                    "FeeTransaction1": $scope.obj.FeeTransaction1,
                    "FeeTransaction2": $scope.obj.FeeTransaction2,
                    "FeeTransaction3": $scope.obj.FeeTransaction3,
                    "FeeTransaction4": $scope.obj.FeeTransaction4,
                    "FeeTransaction5": $scope.obj.FeeTransaction5,
                    "LoginCredentials1": $scope.obj.LoginCredentials1,
                    "LoginCredentials2": $scope.obj.LoginCredentials2,
                    "LoginCredentials3": $scope.obj.LoginCredentials3,
                    "LoginCredentials4": $scope.obj.LoginCredentials4,
                    "LoginCredentials5": $scope.obj.LoginCredentials5,




                }
            }
            else {
                $scope.submitted = true;
            }
            apiService.create("Chatgpt/whatsapp", data).then(function () {
                swal("Successfully sent message")
                $state.reload();
                })
            
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cleardata = function () {
            $state.reload();
        }
     //excel
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;
                    // alert($scope.filename);
                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }
        };
        //to day 

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "image/JPEG" || input.files[0].type === "image/PNG" || input.files[0].type === "image/JPG") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
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
            $http.post("/api/ImageUpload/UploadspaceBooking", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    //$('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
    }
   

})();
angular
    .module('app').filter('keys', function () {

        return function (input) {
            if (!input) {
                return [];
            }
            delete input.$$hashKey;
            return Object.keys(input);
        }

    })

angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {

        return {
            scope: {
                opts: '='
            },
            link: function ($scope, $elm, $attrs) {
                $elm.on('change', function (changeEvent) {
                    var emailids = "";
                    var reader = new FileReader();
                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            var data = evt.target.result;
                            var workbook = XLSX.read(data, { type: 'binary' });
                            var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];
                            data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                            if (data.length === 0) {
                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;
                            }
                            else {
                                $scope.headers = ["MobileNo",];
                                $scope.opts = {};
                                $scope.opts.columnDefs = [];
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });
                                $scope.checkheaders = [];
                                headerNames.forEach(function (h) {
                                    $scope.checkheaders.push(h);
                                });
                                var excellvalidatecount = 0;
                                $scope.Missinghead = [];
                                angular.forEach($scope.headers, function (head1) {
                                    var missingHead = 0;
                                    angular.forEach($scope.checkheaders, function (head2) {
                                        if (head1 === head2) {
                                            excellvalidatecount = excellvalidatecount + 1;
                                            missingHead = 1;
                                        }
                                    });
                                    if (missingHead === 0) {
                                        $scope.Missinghead.push(head1);
                                    }
                                });

                                if (excellvalidatecount === 1) {
                                    var bind = true;
                                    var excellcellvalidate = 0;
                                    $scope.fushlast = [];
                                    $scope.fushlast.push(0);
                                    angular.forEach(data, function (valu, ke) {
                                        $scope.rowheaders = [];
                                        angular.forEach(valu, function (val, ey) {
                                            $scope.rowheaders.push(ey);
                                        });
                                        var checkrow = 0;
                                        angular.forEach($scope.checkheaders, function (head1) {
                                            angular.forEach($scope.rowheaders, function (head2) {
                                                if (head1 === head2) {
                                                    checkrow = checkrow + 1;
                                                }
                                            });
                                        });
                                        if (checkrow !== $scope.checkheaders.length) {
                                            $scope.fushlast.push(valu.__rowNum__ + 1);
                                        }
                                    });
                                    if ($scope.fushlast.length === 1) {

                                        $scope.opts.data = data;
                                        console.log($scope.opts.data);
                                        for (var i = 0; i < $scope.opts.data.length; i++) {
                                            emailids = $scope.opts.data[i].MobileNo;

                                        }
                                        $scope.tempemail = $scope.opts.data;
                                        console.log($scope.tempemail);
                                        //$scope.tempemailppp($scope.tempemail);

                                        $scope.opts.data = $scope.opts.data;
                                        // $scope.obj.emails = emailids;
                                        //for (var i = 0; i < $scope.opts.data.length - 1; i++) {
                                        //    emailids += $scope.opts.data[i].EmailId + ",";
                                        //}
                                        //for (var j = $scope.opts.data.length - 1; i < $scope.opts.data.length; i++) {
                                        //    emailids += $scope.opts.data[i].EmailId;
                                        //}
                                        //$scope.opts.data = emailids;
                                    }
                                    else {
                                        var Missingrows = "";
                                        if ($scope.fushlast.length > 1) {
                                            angular.forEach($scope.fushlast, function (head2) {
                                                if (head2 != 0) {
                                                    if (Missingrows === "") {
                                                        Missingrows = head2;
                                                    }
                                                    else {
                                                        Missingrows = Missingrows + "," + head2;
                                                    }
                                                }
                                            });
                                            Missingrows = "The Row " + Missingrows + " Contains Empty Cell Values, replace with the NULL for empty cell";
                                        }
                                        swal(Missingrows, "Excel Data is Not Proper!!");
                                    }
                                }
                                else {
                                    var Missingheads = "";
                                    if ($scope.Missinghead.length > 0) {
                                        angular.forEach($scope.Missinghead, function (head2) {
                                            if (head2 != 0) {
                                                if (Missingheads === "") {
                                                    Missingheads = head2;
                                                }
                                                else {
                                                    Missingheads = Missingheads + ", \n" + head2;
                                                }
                                            }
                                        });
                                        Missingheads = "The Missing Headers: \n" + Missingheads;
                                        swal(Missingheads, "Header Missing..!!");
                                    }
                                    else {
                                        swal("Header Missing..!!", "Please Import a Excel With All/Proper Headers.");
                                    }

                                }

                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);
                });
            }
        };
    }]);

