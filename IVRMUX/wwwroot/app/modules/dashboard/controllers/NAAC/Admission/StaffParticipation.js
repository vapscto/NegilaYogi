﻿
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffParticipationController', StaffParticipationController);
    StaffParticipationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce','myFactorynaac'];
    function StaffParticipationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
        $scope.searc_button = true;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.submitted = false;
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.searchValue = "";
        $scope.search = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        var vm = this;
        vm.gridOptions = {};
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.institute_flag = false;
        //==================Page Load
        $scope.loaddata = function () {
            $scope.asmaY_Id = "";           
            $scope.HRMD_Id = "";
            $scope.HRMDES_Id = "";

            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }  
            apiService.getURI("StaffParticipation/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                    $scope.MasterCourseList = promise.masterCourseList;
                    $scope.Masterbranch = promise.masterbranchList;
                    $scope.yearlist = promise.yearlist;
                    $scope.discontinuedyearlist = promise.discontinuedyearlist;
                    $scope.departmentlist = promise.departmentlist;
                    $scope.alldata = promise.alldata;
                })
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        //=====================Save Data
        $scope.selectedSectionList = [];
        $scope.savedata = function () {
            $scope.emplistdata = [];
            if ($scope.myForm.$valid) {
                var Pdate = $scope.NCACTP113_PDate == null ? "" : $filter('date')($scope.NCACTP113_PDate, "yyyy-MM-dd");
                angular.forEach($scope.emplist, function (cls) {
                    if (cls.selected == true) {
                        $scope.emplistdata.push(cls);
                    }
                });
                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "NCACTP113_Id": $scope.NCACTP113_Id,
                        "MI_Id": $scope.mI_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "NCACTP113_BodyName": $scope.NCACTP113_BodyName,
                        "NCACTP113_PDate": Pdate,
                        "emplstdata": $scope.emplistdata,
                        "filelist": $scope.materaldocuupload,
                    }
                }
                apiService.create("StaffParticipation/savedata", data).then(function (promise) {
                    if (promise.msg == 'saved') {
                        swal("Record Saved Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == "updated") {
                        swal("Record Updated Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == "Duplicate") {
                        swal("Record Already Exist");
                    }
                    else if (promise.msg == "savingFailed") {
                        swal("Failed to save record");
                    }
                    else if (promise.msg == "updateFailed") {
                        swal("Failed to Update Record");
                    }
                    else {
                        swal("Sorry...Something Went Wrong");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=================================edit
        $scope.edit = function (user) {
            var data = { "NCACTP113_Id": user.ncactP113_Id, "MI_Id": $scope.mI_Id, }
            apiService.create("StaffParticipation/editdata", user).then(function (promise) {
                $scope.institute_flag = true;
                //$scope.editflag = true;
                $scope.emplist = promise.emplist;
                $scope.NCACTP113_Id = promise.editlist[0].ncactP113_Id;
                $scope.HRMD_Id = promise.editlist[0].hrmD_Id;
                $scope.NCACTP113_PDate = new Date(promise.editlist[0].ncactP113_PDate);
                $scope.HRMDES_Id = promise.editlist[0].hrmdeS_Id;
                $scope.asmaY_Id = promise.editlist[0].ncactP113_ParticipatedYear;
                $scope.NCACTP113_BodyName = promise.editlist[0].ncactP113_BodyName;

                $scope.HRMD_Id = promise.editlist[0].hrmD_Id;
                $scope.designationlist = promise.designationlist;
                $scope.HRMDES_Id = promise.editlist[0].hrmdeS_Id;
                angular.forEach($scope.emplist, function (ss) {
                    angular.forEach(promise.editlist, function (tt) {
                        if (ss.hrmE_Id == tt.hrmE_Id) {
                            ss.selected = true;
                        }
                    })
                })
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
                
            });
        }
        //===========deactive and active 
        $scope.deactiveY = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncactP113_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncactP113_ActiveFlg == false) {
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
                        apiService.create("StaffParticipation/deactivY", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully...!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully...!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
        $scope.searchValue = "";
        $scope.filterValue123 = function (obj) {
            return (angular.lowercase(obj.hrmD_DepartmentName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ncactP113_ParticipatedYear)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ncactP113_BodyName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.empname)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmdeS_DesignationName)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }
        //For Cancel data record 
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.get_designation = function () {
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("StaffParticipation/get_designation", data).then(function (promise) {
                $scope.designationlist = promise.designationlist;
            })
        }
        //==================Get Employee List
        $scope.get_emp = function () {
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "HRMDES_Id": $scope.HRMDES_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("StaffParticipation/get_emp", data).then(function (promise) {
                $scope.emplist = promise.emplist;
            })
        }
        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.emplist.some(function (item) {
                return item.selected;
            });
        }
        //=======selection of checkbox....
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.emplist.every(function (role) {
                return role.selected;
            });
        }
        //---------all checkbox Select...
        $scope.all_checkC = function (all) {
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.emplist, function (role) {
                role.selected = toggleStatus;
            });
        };

        //===========view record
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;
            apiService.create("StaffParticipation/viewuploadflies", obj).then(function (promise) {
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
                "NCACTP113F_Id": docfile.ncactP113F_Id,
                "NCACTP113_Id": docfile.ncactP113_Id,
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
                        apiService.create("StaffParticipation/deleteuploadfile", data).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record Deleted successfully");
                                $scope.viewuploadflies = promise.viewuploadflies;
                                //$scope.ncaC414BD_Id = promise.viewuploadflies[0].ncaC414BD_Id;
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
                //dd
            }
        };
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

        // change
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

        
        // view row wise comments
        $scope.getorganizationdata = function (obj) {
            
            apiService.create("StaffParticipation/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // view file wise comments
        $scope.getorganizationdata1 = function (obj) {
            
            apiService.create("StaffParticipation/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };

        $scope.addcomments = function (obje) {                
            $scope.ccc = obje.ncactP113F_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {
            
            $scope.cc = obje.ncactP113F_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        //*************** save row wise comments ***************//
        $scope.savedatawisecomments = function (obj) {
            
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };
            apiService.create("StaffParticipation/savemedicaldatawisecomments", data).then(function (promise) {
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


        // save file wise comments 

        // $scope.obj.generalcomments = "";
        $scope.savedatawisecomments1 = function (obj) {
            
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cc
            };
            apiService.create("StaffParticipation/savefilewisecomments", data).then(function (promise) {
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

        //added by sanjeev       
        $scope.exportEXCEL = false;

        $scope.excelReport = function () {
            if ($scope.exportEXCEL == false) {


            }
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

        $scope.savedataadavance = function (gridOptions) {
            $scope.modalre = [];
            $scope.modalduplicate = [];
            $scope.headers = ["ParticipationYear", "DepartmentName", "DesignationName", "Employeecode", "Date", "Month", "Year", "Body"];
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

            if (excellvalidatecount == 8) {
                data = {
                    "advimppln": $scope.albumNameArray,

                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("StaffParticipation/saveadvance", data).
                    then(function (promise) {
                        $scope.modalre = promise.modal;
                        if (promise.returnval == true) {
                            swal("Some Record Saved Successfully");
                        }
                       
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
        //added end 

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
//added by sanjeev
//---adede by sanjeev
angular.module('app').filter('keys', function () {
    return function (input) {
        if (!input) {
            return [];
        }
        delete input.$$hashKey;
        return Object.keys(input);
    }
})
angular.module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {
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
                            $scope.headers = ["ParticipationYear", "DepartmentName", "DesignationName", "Employeecode", "Date", "Month", "Year", "Body"];
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
                            if (excellvalidatecount === 8) {
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