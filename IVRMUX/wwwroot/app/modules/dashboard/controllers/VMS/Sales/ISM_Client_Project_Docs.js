(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Client_Project_DocsController', ISM_Client_Project_DocsController)

    ISM_Client_Project_DocsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$sce', '$q']
    function ISM_Client_Project_DocsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $sce, $q) {

        $scope.sortKey = 'ISMCLTPRDOC_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }



        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        //===========
        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
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
        //============upload photo

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
            $http.post("/api/ImageUpload/UploadspaceBooking", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                  
                    defer.resolve(d);
                    $scope.cfilepath = d;
                    $scope.cfilepath1 = d;
                    $scope.cfilename = $scope.filename;
                    $scope.cfilename1 = $scope.filename;
                    //$('#').attr('src', data.cfilepath);
                    var img = $scope.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        //==============

        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.linkdata = [{ id: 'link1' }];
        //get data


        ////Multiple File Upload

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

        //====================
        $scope.viewdocument = function (ss) { 
           
            var img = ss.ISMCLTPRDOC_FilePath;
            var imagarr = img.split('.');
            var lastelement = imagarr[imagarr.length - 1];
            
            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                $scope.xl = "";
               $scope.xl= "https://view.officeapps.live.com/op/view.aspx?src=" + ss.ISMCLTPRDOC_FilePath;

                $('#myModalexl').modal('show');
            }
            else if (lastelement === 'jpg' || lastelement === 'jpeg') {
                $('#preview').attr('src', img);
                $('img').bind('contextmenu', function (e) {
                    return false;
                });
                $('#myModalimg').modal('show');
            }

            else if (lastelement === 'pdf') {
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(img, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        }
        //======================
        //get data
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("ISM_Client_Project/getdata_DOC", pageid)
                .then(function (promise) {
                    $scope.clientproject_dd = promise.clientproject_dd;
                    $scope.document_dd = promise.document_dd;
                    $scope.doc_list = promise.doc_list;
                    $scope.presentCountgrid = $scope.doc_list.length;
                })
        }

        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMMCLTPR_Id": $scope.ISMMCLTPR_Id,
                    "ISMCLTPRMDOC_Id": $scope.ISMCLTPRMDOC_Id,
                    "ISMCLTPRDOC_Date": $scope.ISMCLTPRDOC_Date,
                    "ISMCLTPRDOC_FilePath": $scope.cfilepath,
                    "ISMCLTPRDOC_FileName": $scope.cfilename,
                   
                    "ISMCLTPRDOC_Id": $scope.Id
                }
                apiService.create("ISM_Client_Project/SaveEdit_DOC", data).
                    then(function (promise) {


                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully');
                                                    }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully');
                            
                        }
                        else if (promise.returndata === "Error") {
                            swal('Record Not Saved/Updated successfully');
                           
                        }
                        $state.reload();
                    })

            }
            else {

                $scope.submitted = true;
            }

        };

        //edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.ISMCLTPRDOC_Id;
            var pageid = $scope.edibl;
            apiService.getURI("ISM_Client_Project/details_DOC", pageid).
                then(function (promise) {
                    $scope.Id = promise.doc_details[0].ISMCLTPRDOC_Id;
                    $scope.client_project_name = promise.doc_details[0].client_project_name;
                    $scope.ISMMCLTPR_Id = promise.doc_details[0].ISMMCLTPR_Id;
                    $scope.ISMCLTPRMDOC_Id = promise.doc_details[0].ISMCLTPRMDOC_Id;
                    $scope.ismcltprmdoC_Name = promise.doc_details[0].ISMCLTPRMDOC_Name;
                    $scope.ISMCLTPRDOC_Date = new Date(promise.doc_details[0].ISMCLTPRDOC_Date);
                    $scope.cfilepath = promise.doc_details[0].ISMCLTPRDOC_FilePath;

                    var img = promise.doc_details[0].ISMCLTPRDOC_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.filetype = lastelement;

                })
        };



        $scope.cancel = function () {
            $state.reload();

        }

        //deactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ISMCLTPRMP_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_Client_Project/deactivate_DOC", flr).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('Client Project Documents Deactivated Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('Client Project Documents Activated Successfully.');
                                }

                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }


                                $scope.getAllDetail();
                            });
                    } else {
                        swal("Cancelled");
                        $scope.getAllDetail();
                    }

                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



