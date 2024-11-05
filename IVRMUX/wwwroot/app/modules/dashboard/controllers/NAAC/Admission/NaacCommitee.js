(function () {
    'use strict';
    angular.module('app').controller('NaacCommitee', NaacCommitee)

    NaacCommitee.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$sce', 'myFactorynaac']
    function NaacCommitee($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $sce, myFactorynaac) {

        $scope.NCACCOMM_Flg = "Committee";
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }

        $scope.ncaccomM_Id = 0;
        $scope.instit = false;
        $scope.loaddata = function () {
            $scope.ncaccomM_Id = 0;
            $scope.change_institution();

            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }

            apiService.getURI("NaacCommitee/loaddata", $scope.mI_Id).then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                $scope.alldata1 = promise.alldata1;
                $scope.yeardata = promise.yearlist;
                $scope.filldepartment = promise.filldepartment;
            })
        };
        //======================================Save Data
        $scope.search = "";
        $scope.saverecord = function () {

            $scope.selectdStafflist = [];
            angular.forEach($scope.stafftlist, function (staf) {
                if (staf.selected) {
                    $scope.selectdStafflist.push({ HRME_Id: staf.hrmE_Id });
                }
            })
            if ($scope.myForm.$valid) {
                if ($scope.all1 === "12") {
                    var data = {
                        "NCACCOMM_Id": $scope.ncaccomM_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "NCACCOMM_CommitteeName": $scope.NCACCOMM_CommitteeName,
                        "NCACCOMMM_MemberName": $scope.NCACCOMMM_MemberName,
                        "NCACCOMMM_MemberDetails": $scope.NCACCOMMM_MemberDetails,
                        "NCACCOMMM_Role": $scope.NCACCOMMM_Role,
                        "NCACCOMMM_MemberPhoneNo": $scope.NCACCOMMM_MemberPhoneNo,
                        "NCACCOMMM_MemberEmailId": $scope.NCACCOMMM_MemberEmailId,
                        "NCACCOMM_Flg": $scope.NCACCOMM_Flg,
                        "all1": $scope.all1,
                        filelist: $scope.materaldocuupload,
                        "MI_Id": $scope.mI_Id,

                        filelist_staff: $scope.materaldocuuploadStaff,
                    }
                }
                else if ($scope.all1 === "11") {
                    var data = {
                        "NCACCOMM_Id": $scope.ncaccomM_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "NCACCOMM_CommitteeName": $scope.NCACCOMM_CommitteeName,
                        selectdStafflist: $scope.selectdStafflist,
                        "NCACCOMMM_Role": $scope.NCACCOMMM_Role,
                        "all1": $scope.all1,
                        "NCACCOMM_Flg": $scope.NCACCOMM_Flg,
                        filelist: $scope.materaldocuupload,
                        "MI_Id": $scope.mI_Id,
                        filelist_staff: $scope.materaldocuuploadStaff,
                    }
                }


                apiService.create("NaacCommitee/saverecord", data).then(function (promise) {

                   
                    if (promise.msg == "saved") {
                        swal("Data Saved Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == "notsaved") {
                        swal("Data Not Saved Successfully...!!!");
                    }
                    else if (promise.msg == "update") {
                        swal("Data Updated successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == "noupdate") {
                        swal("Data Not Updated Successfully...!!!");
                    }
                    else if (promise.duplicate == true) {
                        swal("Record Already Exist!");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //=========================edit
        $scope.EditData = function (user) {
          
            var data = {
                "NCACCOMM_Id": user.ncaccomM_Id,
                "MI_Id": user.mI_Id
            }
            apiService.create("NaacCommitee/EditData", data).then(function (promise) {
             
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.editlist = promise.editlist;
                    $scope.ncaccomM_Id = promise.editlist[0].ncaccomM_Id;
                    $scope.NCACCOMM_Year = promise.editlist[0].asmaY_Year;
                    $scope.asmaY_Id = promise.editlist[0].ncaccomM_Year;
                    $scope.NCACCOMM_CommitteeName = promise.editlist[0].ncaccomM_CommitteeName;
                    $scope.NCACCOMM_Flg = promise.editlist[0].ncaccomM_Flg;
                    $scope.mI_Id = promise.editlist[0].mI_Id;
                    if (promise.editlist[0].hrmE_Id == 0) {
                        $scope.all1 = '12';
                        $scope.NCACCOMMM_MemberName = promise.editlist[0].ncaccommM_MemberName;
                        $scope.NCACCOMMM_MemberDetails = promise.editlist[0].ncaccommM_MemberName;
                        $scope.NCACCOMMM_MemberPhoneNo = promise.editlist[0].ncaccommM_MemberPhoneNo;
                        $scope.NCACCOMMM_MemberEmailId = promise.editlist[0].ncaccommM_MemberEmailId;
                    }
                    else {
                        $scope.all1 = '11';
                        $scope.filldesignation = promise.filldesignation;
                        $scope.stafftlist = promise.stafftlist;
                        $scope.HRMD_Id = promise.hrmD_Id;
                        $scope.HRMDES_Id = promise.hrmdeS_Id;
                        angular.forEach($scope.stafftlist, function (yy) {
                            angular.forEach($scope.editlist, function (ex) {
                                if (yy.hrmE_Id == ex.hrmE_Id) {
                                    yy.selected = true;
                                }
                            })
                        })
                    }
                    $scope.NCACCOMMM_Role = promise.editlist[0].ncaccommM_Role;
                    $scope.materaldocuupload = promise.editMainSActFileslist;
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


                    $scope.materaldocuuploadStaff = promise.editStaffActFileslist;
                    if ($scope.materaldocuuploadStaff.length > 0) {
                        angular.forEach($scope.materaldocuuploadStaff, function (stt) {
                            var img = stt.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            stt.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                stt.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + stt.cfilepath;
                            }
                        })
                    }
                    else {
                        $scope.materaldocuuploadStaff = [{ id: 'Materal1' }];
                    }

                }
            })
        }

        //======================Active And Deactive
        $scope.deactiveStudent = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncaccomM_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncaccomM_ActiveFlg == false) {
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
                        apiService.create("NaacCommitee/deactiveStudent", usersem).
                            then(function (promise) {
                                if (promise.ret == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        $scope.onradiobuttonSelect = function (type) {
            $scope.labeldisable = false;
        }
        $scope.clear1 = function () {
            $scope.NCACCOMMM_MemberName = "";
            $scope.NCACCOMMM_MemberDetails = "";
            $scope.NCACCOMMM_MemberPhoneNo = "";
            $scope.NCACCOMMM_MemberEmailId = "";
        }
        $scope.clear2 = function () {
            $scope.HRMD_Id = "";
            $scope.HRMDES_Id = "";
        }
        $scope.get_Designation = function () {
            $scope.usercheckC = false;
            $scope.HRMDES_Id = "";
            $scope.filldesignation = [];
            $scope.stafftlist = [];
            if ($scope.HRMD_Id != "" && $scope.HRMD_Id != undefined) {
                var data = {
                    "HRMD_Id": $scope.HRMD_Id,
                    "MI_Id": $scope.mI_Id
                }
                apiService.create("NaacCommitee/get_Designation", data).then(function (promise) {
                    if (promise.filldesignation.length > 0) {
                        $scope.filldesignation = promise.filldesignation;
                    }
                    else {
                        swal('For Selected Department, Designation is not Available!');
                    }
                });
            }
        }

        //==============================get Employee
        $scope.get_Employee = function () {
            $scope.usercheckC = false;
            if ($scope.HRMD_Id != '' && $scope.HRMD_Id != undefined && $scope.HRMDES_Id != '' && $scope.HRMDES_Id != undefined) {
                $scope.stafftlist = [];
                var data = {
                    "HRMD_Id": $scope.HRMD_Id,
                    "HRMDES_Id": $scope.HRMDES_Id,
                    "MI_Id": $scope.mI_Id
                }
                apiService.create("NaacCommitee/get_Employee", data).then(function (promise) {
                    if (promise.stafftlist.length > 0) {
                        $scope.stafftlist = promise.stafftlist;
                    }
                    else {
                        swal('For Selected Designation,Staffs are not Available!');
                    }
                });
            }
        }

        //======================For Staff Count
        $scope.searchchkbx1 = "";
        $scope.NCACSA343_NoOfTeachers = 0;
        $scope.usercheckC = false;
        $scope.all_checkC = function () {
            var selectedstaf = $scope.usercheckC;
            var count2 = 0;
            angular.forEach($scope.stafftlist, function (stf) {
                stf.selected = selectedstaf;
                if (stf.selected == true) {
                    count2 += 1;
                }
                else {
                    count2 = 0;
                }
            })
            $scope.NCACSA343_NoOfTeachers = count2;
        }

        //======================For Staff selection
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.stafftlist.every(function (options) {
                return options.selected;
            });
            var count2 = 0;
            angular.forEach($scope.stafftlist, function (itm) {
                if (itm.selected == true) {
                    count2 = count2 + 1;
                }
            });
            $scope.NCACSA343_NoOfTeachers = count2;
        }

        //======================For Staff validation
        $scope.isOptionsRequired12 = function () {
            return !$scope.stafftlist.some(function (options) {
                return options.selected;
            });
        }

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //============Field Form interacted.
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.cancel = function () {
            $state.reload();
        }

        //===========================================Mapped Staff List
        $scope.mappedstafflist = [];
        $scope.ExistStaffflag = false;
        $scope.newstaffflag = false;
        $scope.get_MappedStaff = function (user) {
 
            var data = {
                "NCACCOMM_Id": user.ncaccomM_Id,
                "NCACCOMM_StaffFlg": user.ncaccomM_StaffFlg,
                "MI_Id": user.mI_Id
            }
            apiService.create("NaacCommitee/get_MappedStaff", data).then(function (promise) {
                
                $scope.modaldata = promise.mappedstafflist;
                if (promise.mappedstafflist[0].NCACCOMM_StaffFlg == 'EXStaf') {
                    $scope.ExistStaffflag = true;
                    $scope.newstaffflag = false;
                    $scope.mappedstafflist = promise.mappedstafflist;
                }
                else if (promise.mappedstafflist[0].NCACCOMM_StaffFlg == 'NEWStaf') {
                    $scope.ExistStaffflag = false;
                    $scope.newstaffflag = true;
                    $scope.mappedstafflist = promise.mappedstafflist;
                }
            })
        }


        //=========================Deactive And Active For Satff
        $scope.deactive_staff = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.NCACCOMMM_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.NCACCOMMM_ActiveFlg == false) {
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
                        apiService.create("NaacCommitee/deactive_staff", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
        //===============================view document files
        $scope.viewdocument_MainActUploadFiles = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NaacCommitee/viewdocument_MainActUploadFiles", obj).then(function (promise) {

                if (promise !== null) {

                    $scope.uploadfilesdetails = promise.viewdocument_MainActUploadFiles;
                    if (promise.viewdocument_MainActUploadFiles !== null && promise.viewdocument_MainActUploadFiles.length > 0) {
                        $scope.uploaddocfiles = promise.viewdocument_MainActUploadFiles;

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

        //===============================delete document files

        $scope.delete_MainActUploadFiles = function (docfile) {

            var data = {
                "NCACCOMM_Id": docfile.ncaccomM_Id,
                "NCACCOMMF_Id": docfile.ncaccommF_Id,
                "MI_Id": docfile.mI_Id
            }

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
                        apiService.create("NaacCommitee/delete_MainActUploadFiles", data).then(function (promise) {
                         
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                                //}
                                //else {
                                //    swal("Record Deletion Failed");
                                //}
                                $scope.viewuploadflies = promise.viewdocument_MainActUploadFiles;
                                //$scope.ncaC414BD_Id = promise.viewuploadflies[0].ncaC414BD_Id;
                                $scope.uploadfilesdetails = promise.viewuploadflies;
                                $scope.uploaddocfiles = promise.viewuploadflies;
                                $scope.uploadfilesdetails = promise.viewuploadflies;


                                if (promise.viewdocument_MainActUploadFiles !== null && promise.viewdocument_MainActUploadFiles.length > 0) {
                                    $scope.uploaddocfiles = promise.viewdocument_MainActUploadFiles;

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
                                //else {
                                //    $('#popup11').modal('hide');
                                //    swal("No Documents Found");
                                //}
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
        //=============================Activity upload
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
        // change 1

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
        //==================Staff Activity Upload   
        $scope.materaldocuuploadStaff = [{ id: 'Materal1' }];

        $scope.addmateralstaff = function () {
            var newItemNo = $scope.materaldocuuploadStaff.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuuploadStaff.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateralstaff = function (index) {
            var newItemNo = $scope.materaldocuuploadStaff.length - 1;
            $scope.materaldocuuploadStaff.splice(index, 1);

            if ($scope.materaldocuuploadStaff.length === 0) {
                //data
            }
        };

        $scope.showmothersignstaff = function (path) {
            $('#previewstaff').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimgstaff').modal('show');
        };

        $scope.showGuardianPhotonewstaff = function (data) {
            $scope.view_staff_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_staff_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_staff_videos);
        };
        // change 2

        $scope.onviewstaff = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz2");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#myModalimgstaff').modal('show');
                });
        };

        //$scope.onviewstaff = function (filepath, filename) {
        //    //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
        //    var imagedownload = filepath;
        //    $scope.contentstaff = "";
        //    var fileURL = "";
        //    var file = "";
        //    $http.get(imagedownload, { responseType: 'arraybuffer' })
        //        .success(function (response) {
        //            file = new Blob([(response)], { type: 'application/pdf' });
        //            fileURL = URL.createObjectURL(file);
        //            $scope.contentstaff = $sce.trustAsResourceUrl(fileURL);
        //            $('#myModalimgstaff').modal('show');
        //        });
        //};
        //===============================view Staff document files
        $scope.viewdocument_StaffActUploadFiles = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;
            apiService.create("NaacCommitee/viewdocument_StaffActUploadFiles", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.viewdocument_StaffActUploadFiles;
                    if ($scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.viewdocument_StaffActUploadFiles;
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
                        $('#popup20').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //===============================delete Staff document files
        $scope.delete_StaffActUploadFiles = function (docfile) {

            var data = {
                "NCACCOMMMF_Id": docfile.ncaccommmF_Id,
                "NCACCOMMM_Id": docfile.ncaccommM_Id,
                "MI_Id": docfile.mI_Id
            }

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
                        apiService.create("NaacCommitee/delete_StaffActUploadFiles", data).then(function (promise) {
                       
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                                //  $scope.viewuploadflies = promise.viewdocument_StaffActUploadFiles;
                                $scope.uploaddocfiles = promise.viewdocument_StaffActUploadFiles;
                                if ($scope.uploaddocfiles.length > 0) {
                                    //$scope.uploaddocfiles = promise.viewdocument_StaffActUploadFiles;
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

        // for comment
        $scope.getorganizationdata = function (obj) {

            apiService.create("NaacCommitee/getcomment", obj).then(function (promise) {

                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };
        $scope.addcomments = function (obje) {
            $scope.ccc = obje.ncaccomM_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };
        $scope.savedatawisecomments = function (obj) {
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };
            apiService.create("NaacCommitee/savemedicaldatawisecomments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments').modal('hide');
                    $('#mymodalviewuploaddocument').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };
        // main file
        $scope.getorganizationdata1 = function (obj) {
            apiService.create("NaacCommitee/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };
        $scope.addcomments1 = function (obje) {
          
            $scope.cc = obje.ncaccommF_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };
        $scope.savedatawisecomments1 = function (obj) {
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cc
            };
            apiService.create("NaacCommitee/savefilewisecomments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }

                    $('#mymodaladdcomments1').modal('hide');
                    $('#mymodalviewuploaddocument1').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };


        // for member
        //getorganizationdatamember

        $scope.getorganizationdatamember = function (obj) {
           
            apiService.create("NaacCommitee/getcommentmember", obj).then(function (promise) {
              
                if (promise !== null) {
                    $scope.commentlistmember = promise.commentlistmember;
                }
            });
        };
        $scope.addcommentsmember = function (obje) {
       
            $scope.ccc = obje.NCACCOMMM_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };
        $scope.savedatawisecommentsmember = function (obj) {
           
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };
            apiService.create("NaacCommitee/savemedicaldatawisecommentsmember", data).then(function (promise) {
              
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcommentsmember').modal('hide');
                    $('#mymodalviewuploaddocumentmember').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };


        $scope.getorganizationdata1member = function (obj) {
        
            apiService.create("NaacCommitee/getfilecommentmember", obj).then(function (promise) {
              
                if (promise !== null) {
                    $scope.commentlist1member = promise.commentlist1member;
                }
            });
        };
        $scope.addcomments1member = function (obje) {
            
           
            $scope.cccc = obje.ncaccommmF_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };
        $scope.savedatawisecomments1member = function (obj) {
         
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cccc
            };
            apiService.create("NaacCommitee/savefilewisecommentsmember", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }

                    $('#mymodaladdcomments1member').modal('hide');
                    $('#mymodalviewuploaddocument1member').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };

        $scope.change_institution = function () {
            $scope.ncaccomM_Id = 0;
            $scope.asmaY_Id = '';
            $scope.NCACCOMM_CommitteeName = '';
            $scope.NCACCOMMM_Role = '';
            $scope.NCACCOMMM_MemberName = '';
            $scope.NCACCOMMM_MemberDetails = '';
            $scope.NCACCOMMM_MemberPhoneNo = '';
            $scope.NCACCOMMM_MemberEmailId = '';
            $scope.HRMD_Id = '';
            $scope.HRMDES_Id = '';

            $scope.filldesignation = [];
            $scope.usercheckC = '';
            $scope.materaldocuuploadStaff = [];
            $scope.materaldocuupload = [];
            $scope.stafftlist = [];
            $scope.materaldocuuploadStaff = [{ id: 'Materal1' }];
            $scope.materaldocuupload = [{ id: 'Materal1' }];

            angular.forEach($scope.stafftlist, function (tt) {
                tt.selected = false;
            })


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




