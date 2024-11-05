(function () {
    'use strict';

    angular
        .module('app')
        .controller('MC_Programs_112Controller', MC_Programs_112Controller);

    MC_Programs_112Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function MC_Programs_112Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
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
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";
        $scope.institute_flag = false;
        //=======================Page Load
        $scope.loaddata = function () {
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("MC_Programs_112/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.yearlist = promise.yearlist;

                $scope.emplylist_1 = promise.emplylist_1;
                $scope.emplylist_2 = promise.emplylist_2;

                $scope.alldata = promise.alldata;

            })
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.submitted = false;
        //========================
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        
     
        $scope.savedata = function () {
            $scope.staffBosList = [];
            $scope.staffCouncilList = [];

            if ($scope.check1 == '1') {
                $scope.flag_BOS = "BOS";
            }
            if ($scope.check2 == '1') {
                $scope.flag_COUN = "Council";
            }
            if ($scope.myFormTab2.$valid) {

                var countOfBOS = 0;
                var countOfCouncil = 0;
                var finalcount = 0;

                angular.forEach($scope.emplylist_1, function (bb) {
                    if (bb.selectedbos == true) {
                        countOfBOS += 1;
                        $scope.staffBosList.push({ HRME_Id: bb.hrmE_Id })
                    }
                })

                angular.forEach($scope.emplylist_2, function (bb) {
                    if (bb.selectedcouncil == true) {
                        countOfCouncil += 1;
                        $scope.staffCouncilList.push({ empid: bb.empid })
                    }
                })
                finalcount = countOfBOS + countOfCouncil;
                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "MI_Id": $scope.mI_Id,
                        "NCMCMPR112_Id": $scope.ncmcmpR112_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "filelist": $scope.materaldocuupload,
                        "staffBosList": $scope.staffBosList,
                        "staffCouncilList": $scope.staffCouncilList,
                        // "finalcount": finalcount,
                        "countOfBOS": countOfBOS,
                        "countOfCouncil": countOfCouncil,
                        "flag_BOS": $scope.flag_BOS,
                        "flag_COUN": $scope.flag_COUN,
                    }
                }
                apiService.create("MC_Programs_112/savedata", data).then(function (promise) {
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.ncmcmpR112_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.ncmcmpR112_Id > 0) {
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
                $scope.submitted = true;
            }
        }

        //deactive and active semester
        $scope.deactive_Y = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncmcmpR112_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncmcmpR112_ActiveFlag == false) {
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
                        apiService.create("MC_Programs_112/deactive_Y", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
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

        //=========================Edit For Tab2 Mapping data
        $scope.edittab2 = function (user) {
            var data = {
                "NCMCMPR112_Id": user.ncmcmpR112_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("MC_Programs_112/editdata", data).then(function (promise) {
                $scope.editdata = promise.editdata;
                $scope.editdataBOS = promise.editdataBOS;
                $scope.editdatacouncil = promise.editdatacouncil;
                $scope.institute_flag = true;
                $scope.ncmcmpR112_Id = promise.editdata[0].ncmcmpR112_Id;
                $scope.mI_Id = promise.editdata[0].mI_Id;
                $scope.ASMAY_Id = promise.editdata[0].ncmcmpR112_Year;
                $scope.NCMCMPR112_NoOfTeachersPartBos = promise.editdata[0].ncmcmpR112_NoOfTeachersPartBos;
                $scope.ncmcmpR112_NoOfTeachersAcu = promise.editdata[0].ncmcmpR112_NoOfTeachersAcu;

                if (promise.editdataBOS[0].ncmcmpR112D_PrgType =='BOS') {
                    $scope.check1 = '1';
                    $scope.check1 = 1;
                    $scope.flag_BOS = "BOS";                    

                    angular.forEach($scope.emplylist_1, function (bos) {
                        angular.forEach($scope.editdataBOS, function (tt) {
                            if (bos.hrmE_Id == tt.hrmE_Id) {
                                bos.selectedbos = true;
                            }
                        });
                    })                   
                }

                if (promise.editdatacouncil[0].ncmcmpR112D_PrgType == 'Council') {
                    $scope.check2 = '1';
                    $scope.check2 = 1;
                    $scope.flag_COUN = "Council";
                    
                    angular.forEach($scope.emplylist_2 , function (t2) {
                        angular.forEach($scope.editdatacouncil, function (tt) {
                            if (t2.empid == tt.empid) {
                                t2.selectedcouncil = true;
                            }
                        });
                    })
                }
                
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
            })
        }
        //==========================cancel Button
        $scope.canceltab2 = function ()
        {            
            $state.reload();
        }
        //===========view record
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("MC_Programs_112/viewuploadflies", obj).then(function (promise) {
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
                "NCMCMPR112_Id": docfile.ncmcmpR112_Id,
                "ncmcmpR112DF_Id": docfile.ncmcmpR112DF_Id,
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
                        apiService.create("MC_Programs_112/deleteuploadfile", data).then(function (promise) {
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
        
            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
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

        //===========================get Staff Details
        $scope.viewStaffBOS = function (user) {
            debugger; 
            var data = {
                "NCMCMPR112_Id": user.ncmcmpR112_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("MC_Programs_112/StaffList_Boss", data).then(function (promise) {
                if (promise.staflist_boos.length > 0) {
                    $scope.staflist_boos = promise.staflist_boos;
                }
                else {
                    swal('Record Not Found!');
                }
            })
        }

        $scope.viewStaffCouncil = function (user) {
            debugger;
            var data = {
                "NCMCMPR112_Id": user.ncmcmpR112_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("MC_Programs_112/StaffList_Council", data).then(function (promise) {
                if (promise.staflist_council.length > 0) {
                    $scope.staflist_council = promise.staflist_council;
                }
                else {
                    swal('Record Not Found!');
                }
            })
        }
        

       
        //=======selection of checkbox....BOS
        $scope.togchkbxBOS = function () {
            $scope.usercheckBOS = $scope.emplylist_1.every(function (role) {
                return role.selectedbos;
            });
        }

        //---------all checkbox Select...BOS
        $scope.all_checkBOS = function (all) {
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckBOS;
            angular.forEach($scope.emplylist_1, function (role) {
                role.selectedbos = toggleStatus;
            });
        }

        //========classlist CheckBox Field Validation===========//BOS
        $scope.isOptionsRequiredBOS = function () {
            return !$scope.emplylist_1.some(function (item) {
                return item.selectedbos;
            });
        }

        //=======selection of checkbox....Council
        $scope.togchkbxCOUN = function () {
            $scope.usercheckCOUN = $scope.emplylist_2.every(function (role) {
                return role.selectedcouncil;
            });
        }

        //---------all checkbox Select...Council
        $scope.all_checkCOUN = function (all) {
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckCOUN;
            angular.forEach($scope.emplylist_2, function (role) {
                role.selectedcouncil = toggleStatus;
            });
        }

        //========classlist CheckBox Field Validation===========//Council
        $scope.isOptionsRequiredBOS = function () {
            return !$scope.emplylist_2.some(function (item) {
                return item.selectedcouncil;
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
