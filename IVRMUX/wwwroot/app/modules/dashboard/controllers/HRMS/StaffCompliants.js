(function () {
    'use strict';
    angular.module('app').controller('StaffCompliantsController', StaffCompliantsController)
    StaffCompliantsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function StaffCompliantsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.editflag = false;

        $scope.NCACAW342_Id = 0;
        $scope.instit = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.disable = false;
        $scope.search = "";

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

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("StaffCompliants/loaddata", pageid).then(function (promise) {

                $scope.getserverdate();
                $scope.HREREM_Date = new Date($scope.today);
                $scope.maxdate = new Date($scope.today);
                $scope.getemployeelist = promise.getemployeelist;
                $scope.getsavedetails = promise.getsavedetails;

                angular.forEach($scope.getsavedetails, function (dd) {
                    if (dd.hrereM_FilePath !== null && dd.hrereM_FilePath !== "") {
                        var img = dd.hrereM_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.filetype = lastelement;
                        console.log("data.filetype : " + dd.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.hrereM_FilePath;
                        }
                    }
                });
            });
        };

        $scope.OnChangeEmployee = function () {
            $scope.HREREM_Subject = "";
            $scope.HREREM_Remarks = "";
            $scope.HREREM_FileName = "";
            $scope.HREREM_FilePath = "";
            $scope.studentname = "";
            $scope.empcode = "";
            $scope.desg = "";
            $scope.dept = "";

            var data = {
                "HRME_Id": $scope.HRME_Id.hrmE_Id
            };

            apiService.create("StaffCompliants/OnChangeEmployee", data).then(function (promise) {
                $scope.symbol = "/";
                $scope.getemployeesaveddetails = promise.getemployeesaveddetails;

                if ($scope.getemployeesaveddetails !== null && $scope.getemployeesaveddetails.length > 0) {

                    angular.forEach($scope.getemployeesaveddetails, function (dd) {
                        if (dd.hrereM_FilePath !== null && dd.hrereM_FilePath !== "") {
                            var img = dd.hrereM_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("data.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.hrereM_FilePath;
                            }
                        }
                    });
                }

                $scope.getemployeedetails = promise.getemployeedetails;

                $scope.studentname = $scope.getemployeedetails[0].hrmE_EmployeeFirstName;
                $scope.empcode = $scope.getemployeedetails[0].hrmE_EmployeeCode;
                $scope.desg = $scope.getemployeedetails[0].hrmdeS_DesignationName;
                $scope.dept = $scope.getemployeedetails[0].hrmD_DepartmentName;

            });
        };

        $scope.SaveDetails = function () {
            var HREREM_Date = new Date($scope.HREREM_Date).toDateString();
            if ($scope.myForm.$valid) {
                var data = {
                    "HREREM_Id": $scope.HREREM_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "HREREM_Date": HREREM_Date,
                    "HREREM_Remarks": $scope.HREREM_Remarks,
                    "HREREM_Subject": $scope.HREREM_Subject,
                    "HREREM_FileName": $scope.HREREM_FileName,
                    "HREREM_FilePath": $scope.HREREM_FilePath
                };

                apiService.create("StaffCompliants/SaveDetails", data).then(function (promise) {
                    if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Failed To Update Record");
                        }
                    } else if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    }
                    else {
                        swal("Failed To Save/Update Record");
                    }

                    $scope.Clearid();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditDetails = function (user) {

            $scope.HREREM_Subject = "";
            $scope.HREREM_Remarks = "";
            $scope.HREREM_FileName = "";
            $scope.HREREM_FilePath = "";
            $scope.studentname = "";
            $scope.empcode = "";
            $scope.desg = "";
            $scope.dept = "";           

            var data = {
                "HREREM_Id": user.hrereM_Id,
                "HRME_Id": user.hrmE_Id
            };

            apiService.create("StaffCompliants/EditDetails", data).then(function (promise) {
                $scope.editlist = promise.editlist;

                if ($scope.editlist !== null && $scope.editlist.length > 0) {
                    $scope.editflag = true;
                    $scope.HREREM_Id = promise.editlist[0].hrereM_Id;

                    $scope.HREREM_Remarks = promise.editlist[0].hrereM_Remarks;
                    $scope.HREREM_Subject = promise.editlist[0].hrereM_Subject;
                    $scope.HREREM_FileName = promise.editlist[0].hrereM_FileName;
                    $scope.HREREM_FilePath = promise.editlist[0].hrereM_FilePath;

                    $scope.HRME_Id = promise.geteditemployeedetails[0];
                    $scope.studentname = promise.geteditemployeedetails[0].hrmE_EmployeeFirstName;
                    $scope.empcode = promise.geteditemployeedetails[0].hrmE_EmployeeCode;
                    $scope.desg = promise.geteditemployeedetails[0].hrmdeS_DesignationName;
                    $scope.dept = promise.geteditemployeedetails[0].hrmD_DepartmentName;

                    if ($scope.HREREM_FilePath !== null && $scope.HREREM_FilePath !== "") {
                        var img = $scope.HREREM_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.filetype = lastelement;
                        console.log("data.filetype : " + $scope.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.HREREM_FilePath;
                        }
                    }

                    $scope.HREREM_Date = new Date(promise.editlist[0].hrereM_Date);

                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };       

        $scope.ActiveDeativeEmployeeCompliantsDetails = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.hrereM_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (usersem.hrereM_ActiveFlg === false) {
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
                        apiService.create("StaffCompliants/ActiveDeativeEmployeeCompliantsDetails", usersem).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                            }
                            else {
                                swal("Record Not " + dystring + "d " + "Successfully!!!");
                            }
                            $scope.Clearid();
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };        

        $scope.Clearid = function () {
            $scope.HREREM_Subject = "";
            $scope.HREREM_Remarks = "";
            $scope.HRME_Id = "";
            $scope.search = "";
            $scope.getemployeelist = [];
            $scope.getsavedetails = [];
            $scope.studentname = "";
            $scope.empcode = "";
            $scope.desg = "";
            $scope.dept = "";
            $scope.HREREM_FileName = "";
            $scope.HREREM_FilePath = "";

            $scope.loaddata();
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

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.onview = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };
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

