
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterVehicleController', MasterVehicleController)

    MasterVehicleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$sce']
    function MasterVehicleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter,$sce) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.sortKey = 'trmV_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.masterlist = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterVehicle/getdata", pageid).then(function (promise) {
                if (promise != null) {
                    if (promise.getloaddata.length > 0) {
                        $scope.mastervehicle = promise.getloaddata;
                        $scope.presentCountgrid = $scope.mastervehicle.length;
                        $scope.masterlist = true;
                    }
                    else {
                        swal("No Records Found")
                        $scope.masterlist = false;
                    }
                    $scope.getfuletype = promise.getfuletype;
                    $scope.getvehicletype = promise.getvehicletype;

                }
                else {
                    swal("No Records Found")
                }
            })

        }

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.tab2 = true;
        $scope.tab1 = false;
        //--Cancel--//
        $scope.clear = function () {
            $scope.TRMV_Id = 0;
            $state.reload();
        }

        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
            $scope.tab2 = true;
            $scope.tab1 = false;
        }
        $scope.next = function () {
            debugger;
            if ($scope.myForm.$valid) {
                $scope.tab2 = false;
                $scope.tab1 = true;
                //$scope.maxDate1 = new Date($scope.LMB_EntryDate);
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted = true;
                $scope.tab2 = true;
                $scope.tab1 = false;
            }

        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.trmV_VehicleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmV_VehicleNo)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmV_EngineNo)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmV_ChassisNo)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.trmV_Capacity)).indexOf($scope.searchValue) >= 0 ||
                ($filter('date')(obj.trmV_PurchaseDate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0)
        }

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                if ($scope.TRMV_ChassisNo != null) {
                    $scope.TRMV_ChassisNo = $scope.TRMV_ChassisNo;
                }
                else {
                    $scope.TRMV_ChassisNo = "";
                }
                if ($scope.TRMV_VehicleImage != null) {
                    $scope.TRMV_VehicleImage = $scope.TRMV_VehicleImage;
                }
                else {
                    $scope.TRMV_VehicleImage = "";
                }
                if ($scope.TRMV_PurchasedFrom != null) {
                    $scope.TRMV_PurchasedFrom = $scope.TRMV_PurchasedFrom;
                }
                else {
                    $scope.TRMV_PurchasedFrom = "";
                }


                var PurchaseDate = $scope.TRMV_PurchaseDate == null ? "" : $filter('date')($scope.TRMV_PurchaseDate, "yyyy-MM-dd");
                var RegistrationDate = $scope.TRMV_RegistrationDate == null ? "" : $filter('date')($scope.TRMV_RegistrationDate, "yyyy-MM-dd");
                var ManufacturedDate = $scope.TRMV_ManufacturedDate == null ? "" : $filter('date')($scope.TRMV_ManufacturedDate, "yyyy-MM-dd");
                var RegFCUpToDate = $scope.TRMV_RegFCUpToDate == null ? "" : $filter('date')($scope.TRMV_RegFCUpToDate, "yyyy-MM-dd");

                var data = {
                    "TRMV_Id": $scope.TRMV_Id,
                    "TRMVT_Id": $scope.TRMVT_Id,
                    "TRMFT_Id": $scope.TRMFT_Id,
                    "TRMV_VehicleName": $scope.TRMV_VehicleName,
                    "TRMV_VehicleNo": $scope.TRMV_VehicleNo,
                    "TRMV_PurchaseDate": PurchaseDate,
                    "TRMV_ChassisNo": $scope.TRMV_ChassisNo,
                    "TRMV_EngineNo": $scope.TRMV_EngineNo,
                    "TRMV_PurchasedFrom": $scope.TRMV_PurchasedFrom,
                    "TRMV_Cost": $scope.TRMV_Cost,
                    "TRMV_Capacity": $scope.TRMV_Capacity,
                    "TRMV_Desc": $scope.TRMV_Desc,
                    "TRMV_VehicleImage": $scope.TRMV_VehicleImage,
                    "TRMV_CompanyName": $scope.TRMV_CompanyName,
                    "TRMV_RegistrationDate": RegistrationDate,
                    "TRMV_ManufacturedDate": ManufacturedDate,
                    "TRMV_RegFCUpToDate": RegFCUpToDate,
                    "TRMV_SWDOff": $scope.TRMV_SWDOff,
                    "TRMV_Address": $scope.TRMV_Address,
                    "TRMV_Model": $scope.TRMV_Model,
                    "TRMV_Fuel": $scope.TRMV_Fuel,
                    "TRMV_Manufacturer": $scope.TRMV_Manufacturer,
                    "TRMV_Class": $scope.TRMV_Class,
                    "TRMV_Color": $scope.TRMV_Color,
                    "TRMV_Body": $scope.TRMV_Body,
                    "TRMV_NoOfCylinder": $scope.TRMV_NoOfCylinder,
                    "TRMV_WheelBase": $scope.TRMV_WheelBase,
                    "TRMV_UnladenWeight": $scope.TRMV_UnladenWeight,
                    "TRMV_CC": $scope.TRMV_CC,
                    "TRMV_TaxUpTo": $scope.TRMV_TaxUpTo,
                    "TRMV_OwnersName": $scope.TRMV_OwnersName,
                    filelist: $scope.materaldocuupload,
                }
                apiService.create("MasterVehicle/savedata", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.message == "Add") {
                            if (promise.retrunval == true) {
                                swal("Record Saved Successfully");
                            }
                            else {
                                swal("Failed To Saved Record");
                            }
                        }
                        else if (promise.message == "Update") {
                            if (promise.retrunval == true) {
                                swal("Record Updated Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                    }
                    else {

                    }
                    $state.reload();
                })
            } else {
                $scope.submitted = true;
            }
        }

        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRMV_Id": user.trmV_Id
            }
            apiService.create("MasterVehicle/edit", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.TRMVT_Id = Promise.geteditdata[0].trmvT_Id;
                    $scope.TRMFT_Id = Promise.geteditdata[0].trmfT_Id;
                    $scope.TRMV_Id = Promise.geteditdata[0].trmV_Id;
                    $scope.TRMV_VehicleName = Promise.geteditdata[0].trmV_VehicleName;
                    $scope.TRMV_VehicleNo = Promise.geteditdata[0].trmV_VehicleNo;
                   
                    $scope.TRMV_ChassisNo = Promise.geteditdata[0].trmV_ChassisNo;
                    $scope.TRMV_EngineNo = Promise.geteditdata[0].trmV_EngineNo;
                    $scope.TRMV_PurchasedFrom = Promise.geteditdata[0].trmV_PurchasedFrom;
                    $scope.TRMV_Cost = Promise.geteditdata[0].trmV_Cost;
                    $scope.TRMV_Capacity = Promise.geteditdata[0].trmV_Capacity;
                    $scope.TRMV_Desc = Promise.geteditdata[0].trmV_Desc;
                    $scope.TRMV_VehicleImage = Promise.geteditdata[0].trmV_VehicleImage;
                    $scope.TRMV_CompanyName = Promise.geteditdata[0].trmV_CompanyName;




                    if (Promise.geteditdata[0].trmV_PurchaseDate != null || Promise.geteditdata[0].trmV_PurchaseDate != undefined) {
                        $scope.TRMV_PurchaseDate = new Date(Promise.geteditdata[0].trmV_PurchaseDate);
                    }

                    if (Promise.geteditdata[0].trmV_RegistrationDate != null || Promise.geteditdata[0].trmV_RegistrationDate != undefined) {
                        $scope.TRMV_RegistrationDate = new Date(Promise.geteditdata[0].trmV_RegistrationDate);
                    }
                    if (Promise.geteditdata[0].trmV_ManufacturedDate != null || Promise.geteditdata[0].trmV_ManufacturedDate != undefined) {
                        $scope.TRMV_ManufacturedDate = new Date(Promise.geteditdata[0].trmV_ManufacturedDate);
                    }
                    if (Promise.geteditdata[0].trmV_RegFCUpToDate != null || Promise.geteditdata[0].trmV_RegFCUpToDate != undefined) {
                        $scope.TRMV_RegFCUpToDate = new Date(Promise.geteditdata[0].trmV_RegFCUpToDate);
                    }

                 
                    $scope.TRMV_SWDOff = Promise.geteditdata[0].trmV_SWDOff;
                    $scope.TRMV_Address = Promise.geteditdata[0].trmV_Address;
                    $scope.TRMV_Model = Promise.geteditdata[0].trmV_Model;
                    $scope.TRMV_Fuel = Promise.geteditdata[0].trmV_Fuel;
                    $scope.TRMV_Manufacturer = Promise.geteditdata[0].trmV_Manufacturer;
                    $scope.TRMV_Class = Promise.geteditdata[0].trmV_Class;
                    $scope.TRMV_Color = Promise.geteditdata[0].trmV_Color;
                    $scope.TRMV_Body = Promise.geteditdata[0].trmV_Body;
                    $scope.TRMV_NoOfCylinder = Promise.geteditdata[0].trmV_NoOfCylinder;
                    $scope.TRMV_WheelBase = Promise.geteditdata[0].trmV_WheelBase;
                    $scope.TRMV_UnladenWeight = Promise.geteditdata[0].trmV_UnladenWeight;
                    $scope.TRMV_CC = Promise.geteditdata[0].trmV_CC;
                    $scope.TRMV_TaxUpTo = Promise.geteditdata[0].trmV_TaxUpTo;
                    $scope.TRMV_OwnersName = Promise.geteditdata[0].trmV_OwnersName;


                    $scope.materaldocuupload = [{ id: 'Materal1' }];
                    if (Promise.editfiles != null && Promise.editfiles.length > 0) {
                        $scope.materaldocuupload = Promise.editfiles;
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

                }
            })
        }
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.mastervehicle, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
            if ($scope.mastervehicle.length === 0 && $scope.printstudents.length > 0) {
                angular.forEach($scope.printstudents, function (itm) {
                    $scope.printstudents.splice(itm);
                });
            }
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.mastervehicle.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmV_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("MasterVehicle/activedeactive/", user).
                    then(function (Promise) {
                        if (Promise.message != null) {
                            swal(Promise.message);
                    }
                    else {
                            if (Promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully.");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //---Duplicate Vehicle No--//
        $scope.validaevehicleno = function () {
            var data = {
                "TRMV_Id": $scope.trmV_Id,
                "TRMV_VehicleNo": $scope.TRMV_VehicleNo,
            }

            apiService.create("MasterVehicle/validaevehicleno/", data).then(function (promise) {
                if (promise.message == "Duplicate") {
                    swal("Record Already Exists");
                    $scope.TRMV_VehicleNo = "";
                }
                else {

                }
            })
        }

        //---Duplicate validaechassiseno --//
        $scope.validaevhassiseno = function () {
            var data = {
                "TRMV_Id": $scope.trmV_Id,
                "TRMV_ChassisNo": $scope.TRMV_ChassisNo,
            }

            apiService.create("MasterVehicle/validaevhassiseno/", data).then(function (promise) {
                if (promise.message == "Duplicate") {
                    swal("Record Already Exists");
                    $scope.TRMV_ChassisNo = "";
                }
                else {

                }
            })
        }

        $scope.rprint1 = function () {

            var innerContents = document.getElementById("printareaId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/trnrcreport.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }
        $scope.rcreportdata = [];
        $scope.rcreport = function () {
            $scope.rcreportdata = [];
            var vehids = [];
            angular.forEach($scope.printstudents, function (mm) {

                vehids.push({ trmV_Id: mm.trmV_Id})
            })

            var data = {
                "vclids": vehids            }

            apiService.create("MasterVehicle/rcreport/", data).then(function (promise) {
                $scope.rcreportdata = promise.rcreport;
            })
        }

        //---Duplicate Engine No--//
        $scope.validaeengineno = function () {
            var data = {
                "TRMV_Id": $scope.trmV_Id,
                "TRMV_EngineNo": $scope.TRMV_EngineNo,
            }

            apiService.create("MasterVehicle/validaeengineno/", data).then(function (promise) {
                if (promise.message == "Duplicate") {
                    swal("Record Already Exists");
                    $scope.TRMV_EngineNo = "";
                }
                else {

                }
            })

        }

        $scope.UploadMotherphoto1 = [];

        $scope.uploadvehiclephoto = function (input) {
            
            $scope.UploadMotherphoto1 = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    uploadvehiclephotopic();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }

            }
        }

        function uploadvehiclephotopic() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadvehiclephoto.length; i++) {
                formData.append("File", $scope.UploadMotherphoto1[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadvehicleimg", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
            .success(function (d) {

                defer.resolve(d);
                // swal(d);
                $scope.TRMV_VehicleImage = d;
            })
            .error(function () {
                defer.reject("File Upload Failed!");
            });
            // Uploads1(miid);
        }


        $scope.showmotherprofilepic = function (TRMV_VehicleImage) {
            $('#preview').attr('src', TRMV_VehicleImage);
        }


        $scope.zoomin = function () {
            

            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        }
        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
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
        // Save Function For Materal Guide Upload

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
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
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
            $('#preview1').attr('src', path);
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


        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("MasterVehicle/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    debugger;
                    $scope.uploadfilesdetails = promise.editfiles;
                    if (promise.editfiles !== null && promise.editfiles.length > 0) {
                        $scope.uploaddocfiles = promise.editfiles;

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


        $scope.onview = function (filepath, filename) {
            debugger;
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



        $scope.deleteuploadfile = function (obj) {

            var data = {
                "TRMVDO_Id": obj.cfileid
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
                        apiService.create("MasterVehicle/deleteuploadfile", data).then(function (promise) {
                            //if (promise.already_cnt === true) {
                            //    swal("You Can Not Deactivate This Record,It Has Dependency");
                            //}
                            //else {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                            //}
                            //$('#popup11').modal('hide');

                            $scope.viewdocument($scope.objdata)
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };



        //////end////

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

