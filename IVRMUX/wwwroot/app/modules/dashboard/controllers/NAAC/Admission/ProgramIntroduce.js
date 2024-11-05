(function () {
    'use strict';

    angular
        .module('app')
        .controller('ProgramIntroduceController', ProgramIntroduceController);

    ProgramIntroduceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function ProgramIntroduceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
        $scope.searc_button = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.currentPage = 1;
        $scope.currentPagetwo = 2;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";
        $scope.institute_flag = false;
        $scope.exportEXCEL = false;
        var vm = this;
        vm.gridOptions = {};
        $scope.excelReport = function () {
            if ($scope.exportEXCEL == false) {

            }
        }

        //=======================Page Load
        $scope.loaddata = function () {
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("ProgramIntroduce/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                $scope.MasterCourseList = promise.masterCourseList;
                $scope.yearlist = promise.yearlist;
                $scope.discontinuedyearlist = promise.discontinuedyearlist;
                $scope.alldata = promise.alldata;

                $scope.mappedlistdata = promise.mappedlistdata;

                $scope.programlist = promise.programlist;
            })
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //==============================Save Data
        $scope.selectedSectionList = [];
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "NCACMPR112_Id": $scope.ncacmpR112_Id,
                    //"AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.branch_id,
                    "NCACMPR112_DiplomaCertName": $scope.NCACMPR112_DiplomaCertName,
                    "NCACMPR112_IntroYear": $scope.ASMAY_Id,
                    "MI_Id": $scope.mI_Id,

                }
                apiService.create("ProgramIntroduce/savedata", data).then(function (promise) {
                    if (promise.returnval == true) {
                        if ($scope.ncacmpR112_Id > 0) {
                            swal('Record Updated Successfully!');
                        }
                        else {
                            swal('Record Saved Successfully!');
                        }
                        $scope.alldata = promise.alldata;
                    }
                    else {
                        if ($scope.ncacmpR112_Id > 0) {
                            swal('Record Not Updated Successfully!');
                        }
                        else {
                            swal('Record Not Saved Successfully!');
                        }
                    }
                    $scope.Clearid();
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //========================Edit
        $scope.edit = function (user) {
            apiService.create("ProgramIntroduce/editdata", user).then(function (promise) {
                $scope.institute_flag = true;
                $scope.editlist = promise.editlist;
                //$scope.Masterbranch = promise.masterbranchList;
                $scope.ncacmpR112_Id = promise.editlist[0].ncacmpR112_Id;
                $scope.AMCO_Id = promise.editlist[0].amcO_Id;
                $scope.AMB_Id = promise.editlist[0].amB_Id;
                //$scope.yearlist = promise.yearlist;
                //$scope.discontinuedyearlist = promise.discontinuedyearlist;
                $scope.ASMAY_Id = promise.editlist[0].ncacmpR112_IntroYear;
                $scope.NCACMPR112_DiplomaCertName = promise.editlist[0].ncacmpR112_DiplomaCertName;
                $scope.mI_Id = promise.editlist[0].mI_Id;

                $scope.get_CourseName($scope.mI_Id, $scope.ASMAY_Id);
                $scope.coursebrnchlist = promise.coursebrnchlist;

                $scope.AMB_Id = promise.editlist[0].amB_Id;


                $scope.branch_id = promise.editlist[0].amB_Id;

                $scope.get_branch($scope.mI_Id, $scope.AMCO_Id);

            })
        }
        //deactive and active semester
        $scope.deactiveY = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncacmpR112_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncacmpR112_ActiveFlg == false) {
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
                        apiService.create("ProgramIntroduce/deactivY", usersem).
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
        //For Cancel data record 
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.searchValue = "";
        $scope.filterValue123 = function (obj) {
            return (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }


        //==============================================Discountinued flag
        $scope.get_Discontinuedflagdata = function (user) {

            var data = {
                "NCACMPR112_Id": user.ncacmpR112_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("ProgramIntroduce/get_Discontinuedflagdata", data).then(function (promise) {

                $scope.mappedProgram = promise.mappedProgram;
                $scope.ncacmpR112_Id = promise.mappedProgram[0].ncacmpR112_Id;
                $scope.discontinuedyearlist = promise.discontinuedyearlist;

            })
        }
        //========================
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        //=====================Save for continued Year falg
        $scope.submitted2 = false;
        $scope.saveContinued = function () {
            if ($scope.myFormModal.$valid) {
                var data = {
                    "NCACMPR112_Id": $scope.ncacmpR112_Id,
                    "NCACPMR112_DiscontinuedYear": $scope.NCACPMR112_DiscontinuedYear,
                    "NCACMPR112_DiscontinuedReason": $scope.NCACMPR112_DiscontinuedReason,
                    "MI_Id": $scope.mI_Id,
                }
                apiService.create("ProgramIntroduce/saveContinued", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Record is Updated Successfully!');
                        $state.reload();
                    }
                    else {
                        swal('Record is Not Updated Successfully!');
                    }
                })
            }
            else {
                $scope.submitted2 = true;
            }
        }
        //====================tab2 data save
        $scope.interacted5 = function (field) {
            return $scope.submitted5;
        };
        $scope.ClearidModal = function () {
            $scope.NCACPMR112_DiscontinuedYear = '';
            $scope.NCACMPR112_DiscontinuedReason = '';
            $scope.submitted2 = false;
        }
        $scope.submitted5 = false;
        $scope.savemappingdata = function () {
            debugger;
            if ($scope.NCACPR112_RevcarriedSyllabusYerars == null || $scope.NCACPR112_RevcarriedSyllabusYerars == undefined || $scope.NCACPR112_RevcarriedSyllabusYerars == '') {
                $scope.NCACPR112_RevcarriedSyllabusYerars = 0;
            }
            if ($scope.NCACPMR112_DiscontinuedYear == null || $scope.NCACPMR112_DiscontinuedYear == undefined
                || $scope.NCACPMR112_DiscontinuedYear == '') {
                $scope.NCACPMR112_DiscontinuedYear = 0;
            }

            if ($scope.myFormTab2.$valid) {
                var prgmdate = $scope.ncacpR112_Date == null ? "" : $filter('date')($scope.ncacpR112_Date, "yyyy-MM-dd");
                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "MI_Id": $scope.mI_Id,
                        "NCACPR112_Id": $scope.ncacpR112_Id,
                        "NCACMPR112_Id": $scope.ncacmpR112_Id,
                        "NCACPR112_Year": $scope.ASMAY_Id,
                        "NCACPR112_Date": prgmdate,
                        "filelist": $scope.materaldocuupload,

                        "NCACPR112_DeptName": $scope.NCACPR112_DeptName,
                        "NCACPR112_RevcarriedSyllabusYerars": $scope.NCACPR112_RevcarriedSyllabusYerars,
                        "NCACPR112_RevisionYear": $scope.NCACPMR112_DiscontinuedYear,

                    }
                }
                apiService.create("ProgramIntroduce/savemappingdata", data).then(function (promise) {
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.ncacpR112_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $scope.mappedlistdata = promise.mappedlistdata;
                            $scope.canceltab2();
                        }
                        else {
                            if ($scope.ncacpR112_Id > 0) {
                                swal('Record Nolt Updated Successfully!');
                            }
                            else {
                                swal('Record Not Saved Successfully!');
                            }
                        }
                    }
                    else {
                        swal('Record Already Exist!');
                    }
                })
            }
            else {
                $scope.submitted5 = true;
            }
        }

        //deactive and active semester
        $scope.deactivYTab2 = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncacpR112_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncacpR112_ActiveFlg == false) {
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
                        apiService.create("ProgramIntroduce/deactivYTab2", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $scope.mappedlistdata = promise.mappedlistdata;
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

        //=========================Edit For Tab2 Mapping data
        $scope.edittab2 = function (user) {
            var data = {
                "NCACPR112_Id": user.ncacpR112_Id,
            }
            apiService.create("ProgramIntroduce/edittab2", data).then(function (promise) {
                $scope.edittab2data = promise.edittab2data;
                $scope.institute_flag = true;
                $scope.mI_Id = promise.edittab2data[0].mI_Id;
                $scope.ncacpR112_Date = new Date(promise.edittab2data[0].ncacpR112_Date);
                $scope.ncacmpR112_Id = promise.edittab2data[0].ncacmpR112_Id;
                $scope.ncacpR112_Id = promise.edittab2data[0].ncacpR112_Id;
                $scope.ASMAY_Id = promise.edittab2data[0].ncacpR112_Year;

                $scope.file_detail = promise.edittab2data[0].ncacpR112_FileName;
                $scope.att_file11 = promise.edittab2data[0].ncacpR112_FilePath;
                $scope.notice = promise.edittab2data[0].ncacpR112_FilePath;
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

                debugger;
                if (promise.edittab2data[0].ncacpR112_RevcarriedSyllabusYerars != null || promise.edittab2data[0].ncacpR112_RevcarriedSyllabusYerars != undefined) {
                    $scope.NCACPR112_RevcarriedSyllabusYerars = promise.edittab2data[0].ncacpR112_RevcarriedSyllabusYerars;
                }
                if (promise.edittab2data[0].ncacpR112_RevisionYear > 0 || promise.edittab2data[0].ncacpR112_RevisionYear > 0) {
                    $scope.NCACPMR112_DiscontinuedYear = promise.edittab2data[0].ncacpR112_RevisionYear;
                }

            })
        }
        //==========================cancel Button  for Tab2
        $scope.canceltab2 = function () {
            $scope.ASMAY_Id = "";
            $scope.ncacmpR112_Id = "";
            $scope.ncacpR112_Date = "";
            $scope.submitted5 = false;
            $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.institute_flag = false;
            $scope.NCACPMR112_DiscontinuedYear = '';
            $scope.NCACPR112_DeptName = '';
            $scope.NCACPR112_RevcarriedSyllabusYerars = '';
            $scope.ncacpR112_Id = '';
            // $state.reload();
        }
        //===========view record
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("ProgramIntroduce/viewuploadflies", obj).then(function (promise) {
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
        //delete/upload file
        $scope.deleteuploadfile = function (docfile) {
            var data = {
                "NCACPR112F_Id": docfile.ncacpR112F_Id,
                "NCACPR112_Id": docfile.ncacpR112_Id,
                "MI_Id": docfile.mI_Id,
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
                        apiService.create("ProgramIntroduce/deleteuploadfile", data).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record Deleted successfully");
                                $scope.viewuploadflies = promise.viewuploadflies;
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
                                else {
                                    $scope.uploaddocfiles = [];
                                }
                            }
                        })
                    }
                    else {
                        swal("Record Deletation Cancelled!!!");
                    }
                });
        }
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
        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {
            debugger;
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
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    $('#showpdf').modal('show');
                });
        };

        $scope.submitted = false;

        $scope.change_institution = function () {
            $scope.submitted = false
            $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.ASMAY_Id = "";
            $scope.ncacmpR112_Id = 0;
            $scope.ncacpR112_Id = 0;
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.NCACMPR112_DiplomaCertName = "";
            $scope.ncacpR112_Date = "";
            $scope.submitted2 = false;
        }

        $scope.get_branch = function () {
            $scope.Masterbranch = [];
            var data = {
                "MI_Id": $scope.mI_Id,
                //"AMCO_Id": id.AMCO_Id,
                "AMB_Id": $scope.AMB_Id

            }
            apiService.create("ProgramIntroduce/get_branch", data).then(function (promise) {
                if (promise.masterbranchList.length > 0) {
                    $scope.Masterbranch = promise.masterbranchList;
                    //$scope.AMB_Id = $scope.Masterbranch[0].amB_Id;
                }
                else {
                    swal('Record Not Found!');
                }
            })
        }


        $scope.get_program = function () {
            $scope.programlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("ProgramIntroduce/get_program", data).then(function (promise) {
                if (promise.programlist.length > 0) {
                    $scope.programlist = promise.programlist;
                }
                else {
                    swal('Record Not Found!');
                }
            })
        }

        $scope.get_CourseName = function () {
            $scope.coursebrnchlist = [];
            $scope.Masterbranch = [];
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("ProgramIntroduce/get_Course", data).then(function (promise) {

                if (promise.coursebrnchlist.length > 0) {
                    $scope.coursebrnchlist = promise.coursebrnchlist;
                }
                else {
                    swal('Record not Found!');
                }

            })
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;

                }
                else {

                    $scope.filename = input.files[0].name;
                }
            }
        };

        //savedata
        $scope.savedataadavance = function (gridOptions) {
            $scope.modalre = [];
            $scope.modalduplicate = [];
            $scope.headers = ["CertificateName", "YearOfIntroduction", "ProgramName", "BranchName"];
            var data = {};
            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                for (var i = 0; i < valu.length; i++) {
                    $scope.albumNameArray.push(valu[i]);
                }
            }
            $scope.albumNameArray1 = [];
            angular.forEach($scope.albumNameArray[0], function (value, key) {
                $scope.albumNameArray1.push(key);
            });
            var excellvalidatecount = 0;
            angular.forEach($scope.headers, function (head1) {
                angular.forEach($scope.albumNameArray1, function (head2) {
                    if (head1 === head2) {
                        excellvalidatecount = excellvalidatecount + 1;
                    }
                });
            });

            if (excellvalidatecount == 4) {
                data = {
                    "advimppln": $scope.albumNameArray,

                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ProgramIntroduce/saveadvance", data).
                    then(function (promise) {
                        $scope.modalre = promise.modal;
                        if (promise.returnval == true) {
                            swal("Some Record Saved Successfully");
                        }                       
                        //if ($scope.modal !== null && $scope.modal.length > 0) {
                        //    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                        //    $timeout(function () { location.href = exportHref; }, 100);
                        //}
                        if ($scope.modalre !== null && $scope.modalre.length > 0) {
                            var exportHref = Excel.tableToExcel(tabledtwo, 'sheet name');
                            $timeout(function () { location.href = exportHref; }, 100);
                        }                                               
                        $state.reload();
                    });
            }
            else {
                swal("Header Mismatch..!!", "Please Import a Excel With All/Proper Headers.");
            }
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
//---adede by sanjeev
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
                                $scope.headers = ["CertificateName", "YearOfIntroduction", "ProgramName", "BranchName"];
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
                                //debugger;
                                if (excellvalidatecount === 4) {
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