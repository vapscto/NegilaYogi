
(function () {
    'use strict';

    angular
        .module('app')
        .controller('NAAC_HSU_Course_StaffMapping_122Controller', NAAC_HSU_Course_StaffMapping_122Controller);

    NAAC_HSU_Course_StaffMapping_122Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function NAAC_HSU_Course_StaffMapping_122Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
        $scope.searc_button = true;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   
            $scope.reverse = !$scope.reverse; 
        }
        $scope.submitted = false;
        $scope.search = "";
        //==============================page load
        var miid = myFactorynaac.get();

        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.NCHSUSM122_Id = 0;
        $scope.instit = false;
        $scope.loaddata = function () {
            
            $scope.NCHSUSM122_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NAAC_HSU_Course_StaffMapping_122/loaddata", $scope.mI_Id).
                then(function (promise) {
                    
                    $scope.institutionlist = promise.institutionlist;
                    $scope.mI_Id = promise.mI_Id;
                    $scope.allacademicyear = promise.allacademicyear;
                    $scope.departmentlist = promise.departmentlist;
                    $scope.alldata1 = promise.alldata1;
                });
        };
        //==============get_course
        $scope.get_course = function () {
            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NAAC_HSU_Course_StaffMapping_122/get_course", data).then(function (promise) {
                
                if (promise.courselist.length > 0) {
                    $scope.courselist = promise.courselist;
                }
                else {
                    swal('Course Not Available For This Academic Year!')
                }
               
            })
        };
        //==============get_designation
        $scope.get_designation = function () {
            $scope.employeelist = [];
            $scope.HRME_Id = "";
            $scope.designationlist = [];
            $scope.HRMDES_Id = "";
      
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NAAC_HSU_Course_StaffMapping_122/get_designation/", data).then(function (promise) {
                
                $scope.designationlist = promise.designationlist;
            })
        };
        //==============get_employee
        $scope.get_employee = function () {
            $scope.employeelist = [];
            $scope.HRME_Id = "";
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "HRMDES_Id": $scope.HRMDES_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NAAC_HSU_Course_StaffMapping_122/get_employee/", data).then(function (promise) {
                
                $scope.employeelist = promise.employeelist;
            })
        };

        ///=================================Clear
        $scope.Clearid = function () {
            $state.reload();
        };
        //==========================================Row Active/Deactive
        $scope.deactive = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.nchsusM122_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.nchsusM122_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("NAAC_HSU_Course_StaffMapping_122/deactive", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
        //===================================Record Edit
        $scope.edittab1 = function (user) {           
            var data = {
                "NCHSUSM122_Id": user.nchsusM122_Id,
                "MI_Id": user.mI_Id
            }
            apiService.create("NAAC_HSU_Course_StaffMapping_122/EditData", data).then(function (promise) {                
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.NCHSUSM122_Id = promise.nchsusM122_Id;
                    $scope.mI_Id = promise.editlist[0].mI_Id;
                    $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;
                    $scope.courselist = promise.courselist; 
                    $scope.AMCO_Id = promise.editlist[0].AMCO_Id;
                    $scope.acayC_Id = promise.editlist[0].acayC_Id;
                    $scope.HRMD_Id = promise.hrmD_Id;                   
                    $scope.designationlist = promise.designationlist;                    
                    $scope.HRMDES_Id = promise.hrmdeS_Id;
                    $scope.employeelist = promise.employeelist;
                    $scope.HRME_Id = promise.hrmE_Id;                    
                    $scope.materaldocuupload = promise.editFileslist;
                    if ($scope.materaldocuupload.length > 0) {
                        angular.forEach($scope.materaldocuupload, function (ddd) {
                            var img = ddd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            ddd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                            }
                        })
                    }
                    else {
                        $scope.materaldocuupload = [{ id: 'Materal1' }];
                    }
                }
            });
        }
        //===========================Save Data
        $scope.save = function () {          
            if ($scope.myForm.$valid) {
                var data = {
                    "NCHSUSM122_Id": $scope.NCHSUSM122_Id,
                    "NMC316DA_Id": $scope.NMC316DA_Id,
                    "asmaY_Id": $scope.ASMAY_Id,
                    "HRME_Id": $scope.HRME_Id,
                    "ACAYC_Id": $scope.acayC_Id,
                    "MI_Id": $scope.mI_Id,
                    filelist: $scope.materaldocuupload,
                }
                apiService.create("NAAC_HSU_Course_StaffMapping_122/save", data).then(function (promise) {                
                    if (promise.duplicate == true) {
                        swal("Data Already Exists");
                    }
                    else if (promise.msg == 'saved') {
                        swal("Data Saved Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data Not Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Updated Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == 'failed') {
                        swal("Data Not Updated Successfully...!!!");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //===========================View File
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;
            apiService.create("NAAC_HSU_Course_StaffMapping_122/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.viewuploadflies;
                    if (promise.viewuploadflies !== null && promise.viewuploadflies.length > 0) {
                        $scope.uploaddocfiles = promise.viewuploadflies;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };
        //===========================Delete File
        $scope.deleteuploadfile = function (docfile) {
            var data = {
                "NCHSUSM122F_Id": docfile.nchsusM122F_Id,
                "NCHSUSM122_Id": docfile.nchsusM122_Id,
                "MI_Id": docfile.mI_Id
            };
            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NAAC_HSU_Course_StaffMapping_122/deleteuploadfile", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                                $scope.uploaddocfiles = promise.viewuploadflies;
                                $scope.uploadfilesdetails = promise.viewuploadflies;
                                if (promise.viewuploadflies !== null && promise.viewuploadflies.length > 0) {
                                    $scope.uploaddocfiles = promise.viewuploadflies;
                                    angular.forEach($scope.uploaddocfiles, function (dd) {
                                        var img = dd.cfilepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                                        }
                                    });
                                }
                            }
                            else {
                                swal("Record Deletion Failed");
                            }

                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.materaldocuupload = [{ id: 'Materal1' }];
        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };
        // Save Function For Materal Guide Upload

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
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
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("size should be less than 2MB");
                    return;
                }
                else {
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
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    $('#').attr('src', data.cfilepath);
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

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };
        $scope.onview = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        };
        //$scope.onview = function (filepath, filename) {
        //    //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
        //    var imagedownload = filepath;
        //    $scope.content = "";
        //    var fileURL = "";
        //    var file = "";
        //    $http.get(imagedownload, { responseType: 'arraybuffer' })
        //        .success(function (response) {
        //            file = new Blob([(response)], { type: 'application/pdf' });
        //            fileURL = URL.createObjectURL(file);
        //            $scope.content = $sce.trustAsResourceUrl(fileURL);
        //            $('#showpdf').modal('show');
        //        });
        //};




        $scope.change_institution = function () {
            $scope.NCHSUSM122_Id = 0;
            $scope.asmaY_Id = '';          
            $scope.HRME_Id = '';
            $scope.ACAYC_Id = '';            
            $scope.usercheckC = '';
            $scope.materaldocuupload = [];
            $scope.materaldocuupload = [{ id: 'Materal1' }];
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

