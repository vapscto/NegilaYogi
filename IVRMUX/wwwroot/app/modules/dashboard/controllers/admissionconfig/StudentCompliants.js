(function () {
    'use strict';
    angular.module('app').controller('StudentCompliants', StudentCompliants)

    StudentCompliants.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function StudentCompliants($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

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

        //======================page load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("StudentCompliants/loaddata", pageid).then(function (promise) {

                $scope.getserverdate();
                $scope.ASCOMP_Date = new Date($scope.today);
                $scope.maxdate = new Date($scope.today);

                $scope.allacademicyear = promise.allacademicyear;
                $scope.middate = new Date(promise.allacademicyear[0].asmaY_From_Date);
                // $scope.studentlist = promise.studentlist;
                $scope.allstudentdivlist = promise.allstudentdivlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.alldata1 = promise.alldata1;
            });
        };

        $scope.disable = false;

        $scope.getstudents = function () {
            $scope.studentinfolist = [];
            $scope.studentlist = [];
            $scope.allstudentdivlist = [];
            $scope.studentdivlist = [];
            $scope.AMST_Id = '';
            $scope.ASCOMP_FileName = "";
            $scope.ASCOMP_FilePath = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentCompliants/getstudents", data).then(function (promise) {
                if (promise.allstudentdivlist.length > 0) {
                    $scope.allstudentdivlist = promise.allstudentdivlist;
                }
            });
        };

        $scope.save = function () {
            var ASCOMP_Date = new Date($scope.ASCOMP_Date).toDateString();
            if ($scope.myForm.$valid) {
                var data = {
                    "ASCOMP_Id": $scope.ASCOMP_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "ASCOMP_Date": ASCOMP_Date,
                    "ASCOMP_Complaints": $scope.ASCOMP_Complaints,
                    "ASCOMP_Subject": $scope.ASCOMP_Subject,
                    "ASCOMP_FileName": $scope.ASCOMP_FileName,
                    "ASCOMP_FilePath": $scope.ASCOMP_FilePath
                };

                apiService.create("StudentCompliants/save", data).then(function (promise) {
                    if (promise.msg === "saved") {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.msg === "notsaved") {
                        swal("Record Not Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.msg === "updated") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.msg === "notupdated") {
                        swal("Record Not Updated Successfully");
                        $state.reload();
                    }
                    else {
                        swal("Something Went Wrong");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.getstudentdetails = function () {

            $scope.studentinfolist = [];
            $scope.studentname = "";
            $scope.AMST_AdmNo = "";
            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.ASCOMP_Complaints = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMST_Id": $scope.AMST_Id.amsT_Id
            };

            apiService.create("StudentCompliants/getstudentdetails", data).then(function (promise) {
                $scope.symbol = "/";
                $scope.studentinfolist = promise.studentinfolist;

                if ($scope.studentinfolist !== null && $scope.studentinfolist.length > 0) {

                    angular.forEach($scope.studentinfolist, function (dd) {
                        if (dd.ascomP_FilePath !== null && dd.ascomP_FilePath !== "") {
                            var img = dd.ascomP_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("data.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ascomP_FilePath;
                            }
                        }
                    });
                }

                $scope.studentdivlist = promise.studentdivlist;

                angular.forEach($scope.studentdivlist, function (a) {
                    $scope.studentname = a.amsT_FirstName;
                    $scope.AMST_AdmNo = a.amsT_AdmNo;
                    $scope.AMAY_RollNo = a.amaY_RollNo;
                    $scope.ASMCL_Id = a.asmcL_Id;
                    $scope.ASMS_Id = a.asmS_Id;
                    $scope.ASMCL_ClassName = a.asmcL_ClassName;
                    $scope.ASMC_SectionName = a.asmC_SectionName;

                });
            });
        };

        $scope.edittab1 = function (user) {

            $scope.ASCOMP_Complaints = "";
            var data = {
                "ASCOMP_Id": user.ascomP_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentCompliants/edittab1", data).then(function (promise) {
                $scope.editlist = promise.editlist;
                if ($scope.editlist !== null && $scope.editlist.length > 0) {
                    $scope.editflag = true;
                    $scope.ASCOMP_Id = promise.editlist[0].ascomP_Id;
                    //$scope.AMST_Id = promise.editlist[0];
                    $scope.ASCOMP_Complaints = promise.editlist[0].ascomP_Complaints;
                    $scope.ASCOMP_Subject = promise.editlist[0].ascomP_Subject;
                    $scope.ASCOMP_FileName = promise.editlist[0].ascomP_FileName;
                    $scope.ASCOMP_FilePath = promise.editlist[0].ascomP_FilePath;

                    if ($scope.ASCOMP_FilePath !== null && $scope.ASCOMP_FilePath !== "") {
                        var img = $scope.ASCOMP_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.filetype = lastelement;
                        console.log("data.filetype : " + $scope.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.ASCOMP_FilePath;
                        }
                    }

                    $scope.ASCOMP_Date = new Date(promise.editlist[0].ascomP_Date);

                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };



        $scope.edit = function (item) {
            $scope.newcaste = {};
            $scope.studentlist = [];
            var data = {
                "ASCOMP_Id": item.ASCOMP_Id,
                "ASMAY_Id": item.ASMAY_Id
            }
            apiService.create("StudentCompliants/editdetails", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.editdata = promise.editdata;
                    $scope.editflag = true;
                    $scope.studentlist = promise.studentlist;
                    // $scope.AMST_Id = $scope.editdata[0].ascomP_Id;
                    $scope.ASCOMP_Id = $scope.editdata[0].ascomP_Id;
                    $scope.ASMAY_Id = $scope.editdata[0].asmaY_Id;
                    // $scope.$parent.AMST_Id = $scope.editdata[0].amsT_Id;
                    $scope.ASCOMP_Date = new Date(promise.editdata[0].ascomP_Date);
                    $scope.ASCOMP_Subject = $scope.editdata[0].ascomP_Subject;
                    $scope.ASCOMP_Complaints = $scope.editdata[0].ascomP_Complaints;
                    $scope.ASCOMP_FilePath = $scope.editdata[0].ascomP_FilePath;
                    $scope.ASCOMP_FileName = $scope.editdata[0].ascomP_FileName;
                    $scope.studentname = $scope.editdata[0].amsT_FirstName;

                    $scope.AMST_AdmNo = $scope.editdata[0].amsT_AdmNo;
                    $scope.ASMCL_ClassName = $scope.editdata[0].asmcL_ClassName;
                    $scope.ASMC_SectionName = $scope.editdata[0].asmC_SectionName;


                    if ($scope.ASCOMP_FilePath !== null && $scope.ASCOMP_FilePath !== "") {
                        var img = $scope.ASCOMP_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.filetype = lastelement;
                        console.log("data.filetype : " + $scope.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.ASCOMP_FilePath;
                        }
                    }


                    if ($scope.studentlist != null && $scope.studentlist.length > 0) {                        for (var i = 0; i < $scope.studentlist.length; i++) {                            if ($scope.editdata[0].amsT_Id === $scope.studentlist[i].amsT_Id) {                                $scope.studentlist[i].selected = true;                                $scope.AMST_Id = $scope.studentlist[i];                                $scope.newcaste = $scope.studentlist[0].amsT_Id;                            }                        }                    }



                }
            })
        }


        $scope.deletedata = function (item) {
            var data = {
                'ASCOMP_Id': item.ASCOMP_Id
            };
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StudentCompliants/deletepagesY", data).
                            then(function (promise) {


                                if (promise.msg == 'updated') {
                                    swal('Record Deleted Successfully');
                                    $scope.loaddata();
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }

                });

        }

        $scope.getorganizationdata = function (user) {
            var data = "";
            $scope.viewlist = [];
            if (user === undefined) {
                data = {
                    "AMST_Id": $scope.AMST_Id
                };
            }
            else {
                data = {
                    "AMST_Id": user.amsT_Id
                };
            }

            apiService.create("StudentCompliants/getorganizationdata", data).then(function (promise) {
                $scope.viewlist = promise.viewlist;

                if ($scope.viewlist !== null && $scope.viewlist.length > 0) {

                    angular.forEach($scope.viewlist, function (dd) {
                        if (dd.ascomP_FilePath !== null && dd.ascomP_FilePath !== "") {
                            var img = dd.ascomP_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("data.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ascomP_FilePath;
                            }

                        }
                    });
                }
            });
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                //$scope.size = input.files[0].size;
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
            $http.post("/api/ImageUpload/UploadStudentDocuments", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.ASCOMP_FilePath = d;
                    $scope.ASCOMP_FileName = $scope.filename;
                    $('#').attr('src', $scope.ASCOMP_FilePath);
                    var img = $scope.ASCOMP_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.filetype = lastelement;
                    console.log("data.filetype : " + $scope.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.ASCOMP_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        //======================Record Save
        $scope.search = "";
        $scope.deactivYTab1 = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.ascomP_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (usersem.ascomP_ActiveFlg === false) {
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
                        apiService.create("StudentCompliants/deactive", usersem).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                                $scope.getstudentdetails();
                            }
                            else {

                                swal("Record Not " + dystring + "d " + "Successfully!!!");
                                $scope.getstudentdetails();
                            }
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        };

        $scope.deactivYTab = function (usersem, SweetAlert) {
            $scope.AMST_Id = usersem.amsT_Id;
            var dystring = "";
            if (usersem.ascomP_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (usersem.ascomP_ActiveFlg === false) {
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
                        apiService.create("StudentCompliants/deactive", usersem).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                                $scope.getorganizationdata();
                            }
                            else {
                                swal("Record Not " + dystring + "d " + "Successfully!!!");
                                $scope.getorganizationdata();
                            }
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        };

        // search name 
        $scope.searchfilter = function (objj) {
            if (objj.search.length >= '3') {
                var data = {
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.ASMAY_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("StudentCompliants/searchfilter", data).then(function (promise) {
                    $scope.studentlist = promise.studentlist;
                });
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

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
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
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

