(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterHostelController', MasterHostelController)

    MasterHostelController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q','$sce']
    function MasterHostelController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $sce) {

        $scope.submitted = false;
        $scope.hlmH_BoysGirlsFlg = "Boys";
      
        //=====================Loaddata.............
        $scope.Loaddata = function () {
           
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
          
            var pageid = 2;
            apiService.getURI("MasterHostel/Page_loaddata", pageid).then(function (promise) {
               
                $scope.countrylist = promise.countrylist;
                $scope.mess_list = promise.mess_list;
                $scope.facilities_list = promise.facilities_list;
                $scope.employee_list = promise.employee_list;
                $scope.get_gridlistdata = promise.get_gridlistdata;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //=====================End-----Load--data----//


        //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            $scope.selectedFacility = [];
            $scope.selectedWarden = []; 
            $scope.selectedmess = []; 
            angular.forEach($scope.facilities_list, function (fac) {
                if (fac.fselected) {
                    $scope.selectedFacility.push({ HLMFTY_Id: fac.hlmftY_Id })
                }                
            })
            angular.forEach($scope.employee_list, function (emp) {
                if (emp.empselected) {
                    $scope.selectedWarden.push({ HRME_Id: emp.hrmE_Id })
                }              
            })
            angular.forEach($scope.mess_list, function (emp) {
                if (emp.messselected) {
                    $scope.selectedmess.push({ HLMM_Id: emp.hlmM_Id })
                }
            })

            if ($scope.myForm.$valid) {              
                    var data = {
                        "HLMH_Id": $scope.hlmH_Id,
                        "HLMH_Name": $scope.hlmH_Name,
                        "IVRMMC_Id": $scope.ivrmmC_Id,
                        "IVRMMS_Id": $scope.ivrmmS_Id,
                        "HLMH_City": $scope.hlmH_City,
                        "HLMH_BoysGirlsFlg": $scope.hlmH_BoysGirlsFlg,
                        "HLMH_TotalFloor": $scope.hlmH_TotalFloor,
                        "HLMH_TotalRoom": $scope.hlmH_TotalRoom,
                        "HLMH_Desc": $scope.hlmH_Desc,
                        "HLMH_ContactNo": $scope.hlmH_ContactNo,
                        "HLMH_Pincode": $scope.hlmH_Pincode,
                        "HLMH_Address": $scope.hlmH_Address,  
                        //"HLMM_Id": $scope.hlmM_Id,
                        "HLMHW_WardenType": $scope.hlmhW_WardenType,
                        "HLMH_TotalCapacity": $scope.hlmH_TotalCapacity,
                        hostelPictureUploadDTO: $scope.hosteldocuupload,
                        selectedFacilityDTO: $scope.selectedFacility,
                        selectedWardenDTO: $scope.selectedWarden,
                        selectedMessDTO:$scope.selectedmess,
                    }             
                apiService.create("MasterHostel/Save_Hostel_Data", data).then(function (promise) {
                    if (promise.returnval !== null && promise.duplicate !== null) {
                        if (promise.duplicate === false) {
                            if (promise.returnval === true) {
                                if ($scope.hlmH_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                    $state.reload();
                                }
                            }
                            else {
                                if (promise.returnval === false) {
                                    if ($scope.hlmH_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=====================End---saverecord....


        //====================Delete file========
        $scope.deleteuploadfile = function (docfile) {

            var data = {
                "HLMHP_Id": docfile.hlmhP_Id,
                "HLMH_Id": docfile.hlmH_Id,
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
                        apiService.create("MasterHostel/deleteuploadfile", data).then(function (promise) {

                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");


                                $scope.hosteldocuupload = promise.viewuploadflies;
                                $scope.uploadfilesdetails = promise.viewuploadflies;
                                if (promise.viewuploadflies !== null && promise.viewuploadflies.length > 0) {
                                    $scope.hosteldocuupload = promise.viewuploadflies;

                                    angular.forEach($scope.hosteldocuupload, function (dd) {
                                        var img = dd.hlmhP_FilePath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.hlmhP_FilePath;
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


        //=====================view file==========
        $scope.viewdocument = function (obj) {
            //$scope.hosteldocuupload = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("MasterHostel/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {

                    $scope.uploadfilesdetails = promise.viewuploadflies;
                    if (promise.viewuploadflies !== null && promise.viewuploadflies.length > 0) {
                        $scope.hosteldocuupload = [];
                        $scope.hosteldocuupload = promise.viewuploadflies;

                        angular.forEach($scope.hosteldocuupload, function (dd) {
                            var img = dd.hlmhP_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.hlmhP_FilePath;
                            }
                        });
                    }
                    else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };
        //=================Activation/Deactivation
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.hlmH_Id = user.hlmH_Id;

            var dystring = "";
            if (user.hlmH_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hlmH_ActiveFlag === false) {
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
                        apiService.create("MasterHostel/Deactive_Hostel_Row", user).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record.........
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }
        //==========================Validation Selection For facility check box
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.facilities_list, function (itm) {
                itm.fselected = checkStatus;
            });
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.facilities_list.every(function (options) {
                return options.fselected;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.facilities_list.some(function (options) {
                return options.fselected;
            });
        }

        //============================================End
        //==========================Validation Selection For Warden check box
        $scope.searchchkbxEmp = "";
        $scope.all_Empcheck = function () {
            var checkStatusemp = $scope.usercheckEmp;
            angular.forEach($scope.employee_list, function (itm) {
                itm.empselected = checkStatusemp;
            });
        }

        $scope.togchkbxEmp = function () {
            $scope.usercheckEmp = $scope.employee_list.every(function (options) {
                return options.empselected;
            });
        }

        $scope.isOptionsRequiredEmp = function () {
            return !$scope.employee_list.some(function (options) {
                return options.empselected;
            });
        }
        //============================================End

        //==========================Validation Selection For mess check box
        $scope.searchchkbxEmp = "";
        $scope.all_messcheck = function () {
            var checkStatusmess = $scope.usercheckmess;
            angular.forEach($scope.mess_list, function (itm) {
                itm.messselected = checkStatusmess;
            });
        }

        $scope.togchkbxmess = function () {
            $scope.usercheckmess = $scope.mess_list.every(function (options) {
                return options.messselected;
            });
        }

        $scope.isOptionsRequiredEmp = function () {
            return !$scope.mess_list.some(function (options) {
                return options.messselected;
            });
        }
        //============================================End

        //==================== Get State List Data
        $scope.get_statelist = function () {
            var data = {
                "IVRMMC_Id": $scope.ivrmmC_Id,
            }
            apiService.create("MasterHostel/Get_StateData", data).then(function (promise) {
                $scope.statelistdata = promise.statelistdata;
            })
        }
        //=========================For Edit 
        $scope.editHosteldata = function (obj) {
            $scope.edit_mess = [];
            var data = {

                "HLMH_Id": obj.hlmH_Id,
            }
            apiService.create("MasterHostel/Edit_Hostel_Row", data).then(function (promise) {
                if (promise.edit_hostel_row.length > 0) {
                $scope.employee_list = promise.employee_list;
                $scope.edit_hostel_row = promise.edit_hostel_row;
                $scope.hlmH_Id = promise.edit_hostel_row[0].hlmH_Id;
                $scope.hlmH_Name = promise.edit_hostel_row[0].hlmH_Name;
                $scope.hlmH_City = promise.edit_hostel_row[0].hlmH_City;
                $scope.hlmF_Id = promise.edit_hostel_row[0].hlmH_TotalFloor;
                $scope.hrmrM_Id = promise.edit_hostel_row[0].hlmH_TotalRoom;
                $scope.hlmH_Desc = promise.edit_hostel_row[0].hlmH_Desc;
                $scope.hlmH_WardenName = promise.edit_hostel_row[0].hlmH_WardenName;
                $scope.hlmH_ContactNo = promise.edit_hostel_row[0].hlmH_ContactNo;
                $scope.hlmH_Pincode = promise.edit_hostel_row[0].hlmH_Pincode;
                $scope.hlmH_Address = promise.edit_hostel_row[0].hlmH_Address;
                $scope.hlmH_TotalFloor = promise.edit_hostel_row[0].hlmH_TotalFloor;
                $scope.hlmH_TotalRoom = promise.edit_hostel_row[0].hlmH_TotalRoom;
                $scope.hlmH_TotalCapacity = promise.edit_hostel_row[0].hlmH_TotalCapacity;
                $scope.ivrmmC_Id = promise.edit_hostel_row[0].ivrmmC_Id;
                $scope.statelistdata = promise.statelistdata;
                $scope.ivrmmS_Id = promise.ivrmmS_Id;
                    $scope.hlmH_BoysGirlsFlg = promise.edit_hostel_row[0].hlmH_BoysGirlsFlg;

                    $scope.hlmM_Id = promise.edithostelmess_id[0].hlmM_Id;
                    $scope.edit_facilitylist = promise.edit_facilitylist;
                    $scope.edit_emplist = promise.edit_emplist;
                    $scope.edit_mess = promise.edit_mess;
                    //  $scope.edit_photolist = promise.edit_photolist;

                    angular.forEach($scope.facilities_list, function (main) {
                        angular.forEach($scope.edit_facilitylist, function (edit) {
                            if (main.hlmftY_Id == edit.hlmftY_Id) {
                                main.fselected = true;
                            }
                        })
                    })
                    angular.forEach($scope.employee_list, function (main) {
                        angular.forEach($scope.edit_emplist, function (edit) {
                            if (main.hrmE_Id == edit.hrmE_Id) {
                                main.empselected = true;
                            }
                        })
                    })

                    angular.forEach($scope.mess_list, function (main) {
                        angular.forEach($scope.edit_mess, function (edit) {
                            if (main.hlmM_Id == edit.hlmM_Id) {
                                main.messselected = true;
                            }
                        })
                    })
                    //angular.forEach($scope.employee_list, function (main) {
                    //    angular.forEach($scope.edit_emplist, function (edit) {
                    //        if (main.hrmE_Id == edit.hrmE_Id) {
                    //            main.empselected = true;
                    //        }
                    //    })
                    //})
                    $scope.hlmhW_WardenType = $scope.edit_emplist[0].hlmhW_WardenType;


                $scope.hosteldocuupload = promise.editphotolist;

                    if ($scope.hosteldocuupload.length > 0) {
                angular.forEach($scope.hosteldocuupload, function (tt) {
                    var img = tt.hlmhP_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    tt.filetype = lastelement;
                });
                }
                else {
                        $scope.hosteldocuupload = [{ id: 'Teacher1' }];
                }


                //$scope.hlmhP_FilePath = promise.edit_photolist[0].hlmhP_FileName;
                //$scope.hlmhP_FilePath = promise.edit_photolist[0].hlmhP_FilePath;
                //$scope.notice = promise.edit_photolist[0].hlmhP_FilePath;
            }
            })
        }
        //=============== Function For Hostel Pictures Upload
        $scope.uploadtecherdocuments1 = [];
        $scope.uploadtecherdocuments = function (input, document) {
            $scope.uploadtecherdocuments1 = input.files;
            $scope.filename = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
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
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
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
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.hlmhP_FilePath = d;
                    data.hlmhP_FileName = $scope.filename; 
                    $('#').attr('src', data.hlmhP_FilePath); 
                    var img = data.hlmhP_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.hlmhP_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.hosteldocuupload = [{ id: 'Teacher1' }];
        // Upload Functions For Hostel Guides
        $scope.hosteldocuupload=[];
        $scope.hosteldocuupload = [{ id: 'Teacher1' }];
        //===========================Add extra Column
        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.hosteldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.hosteldocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.hosteldocuupload);
        };

         //===========================Remove extra Column
        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.hosteldocuupload.length - 1;
            $scope.hosteldocuupload.splice(index, 1);

            if ($scope.hosteldocuupload.length === 0) {
                //data
            }
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.hlmhP_FilePath;
            $scope.videdfd = data.hlmhP_FilePath;
            $scope.movie = { src: data.hlmhP_FilePath };
            $scope.movie1 = { src: data.hlmhP_FilePath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.hlmhP_FilePath });
            console.log($scope.view_videos);
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };
        //===================get  Mapped  facility
        $scope.get_Mappedfacility = function (user) {

            apiService.create("MasterHostel/Get_MappedFacility", user).then(function (promise) {
                $scope.mappedfacilitylist = promise.mappedfacilitylist;
                $scope.temparry = promise.mappedfacilitylist;               
            });
        }
        //===================get  Mapped  Employee data
        $scope.get_MappedEmpl = function (user) {
            apiService.create("MasterHostel/Get_MappedEmpl", user).then(function (promise) {
                $scope.mappedEmpllist = promise.mappedEmpllist;
                $scope.temparry = promise.mappedEmpllist;
            });
        }
        //==============================For View 
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

