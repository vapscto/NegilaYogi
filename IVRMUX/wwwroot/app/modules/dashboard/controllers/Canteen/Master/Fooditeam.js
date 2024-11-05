(function () {
    'use strict';
    angular
        .module('app')
        .controller('FooditeamController', FooditeamController)
    FooditeamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function FooditeamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {

        $scope.EditRecord = {};
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.CMMFI_OutofStockFlg = false;
        $scope.obj = {};
        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("Fooditeam/loaddata", pageid).
                then(function (promise) {
                    $scope.categeorylist = promise.categeorylist;

                    $scope.fooditeam = promise.fooditeamDeatils;
                    if ($scope.fooditeam != null && $scope.fooditeam.length > 0) {
                        $scope.presentCountgrid = $scope.fooditeam.length;
                    }
                })
        }



        $scope.submit = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.CMMFI_FoodItemFlag == 0) {
                    $scope.CMMFI_FoodItemFlag = true;
                } else {
                    $scope.CMMFI_FoodItemFlag = false;
                }
                var CMMFI_Id = 0;
                if ($scope.obj.CMMFI_Id > 0) {
                    CMMFI_Id = $scope.obj.CMMFI_Id;
                }

                //-------file upload
                $scope.filedoc = [];
                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.IHW_FilePath != null) {
                            $scope.documentListOtherDetails11.push({ IHW_FilePath: qq.IHW_FilePath, FileName: qq.FileName });
                        }
                    })
                    $scope.filedoc = $scope.documentListOtherDetails11;
                }

                $scope.filedoc = [];
                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.IHW_FilePath != null) {
                            $scope.documentListOtherDetails11.push({ IHW_FilePath: qq.IHW_FilePath, FileName: qq.FileName });
                        }
                    })
                    $scope.filedoc = $scope.documentListOtherDetails11;
                }


                if ($scope.checklink == true) {
                    angular.forEach($scope.urldocumentlist, function (qq) {
                        if (qq.IHW_FilePath != null) {
                            $scope.filedoc.push({ IHW_FilePath: qq.IHW_FilePath, FileName: qq.IHW_FilePath });
                        }
                    });
                }
                var data = {

                    "CMMFI_FoodItemName": $scope.CMMFI_FoodItemName,
                    "CMMFI_FoodItemDescription": $scope.CMMFI_FoodItemDescription,
                    "CMMFI_UnitRate": $scope.CMMFI_UnitRate,
                    "CMMFI_OutofStockFlg": $scope.CMMFI_OutofStockFlg,
                    "CMMFI_FoodItemFlag": $scope.CMMFI_FoodItemFlag,
                    "CMMFI_Id_FilePath_Array": $scope.filedoc,
                    "CMMCA_Id": $scope.cmmcA_Id,
                    "CMMFI_Id": CMMFI_Id,
                }
                apiService.create("Fooditeam/savedata", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !', "", "success");
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !', "", "success");

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !', "", "error");

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !', "", "error");
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !', "", "warning");
                        }
                        $state.reload();
                    })

            }
            else {
                $scope.submitted = true;
            }


        };


        $scope.imagedata = function (itm) {
            var data = {
                "CMMFI_Id": itm.cmmfI_Id,
            }
            apiService.create("Fooditeam/Getimagedata/", data).then(function (promise) {

                if (promise.imageDetails != null && promise.imageDetails.length > 0) {
                    $scope.icaI_Attachment = promise.imageDetails[0].icaI_Attachment ;
                    $('#imagePreview').modal('show');
                }
            })
        };
        
        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {


            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/HomeworkUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.IHW_FilePath = d[0].path;
                        data.FileName = d[0].name;
                        $scope.ldr = false;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        //==============================
        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);

                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }

        };

       

        //edit data 
        $scope.Editdata = function (EditRecord) {
            var data = {

                "CMMFI_Id": EditRecord.cmmfI_Id,
            }
            apiService.create("Fooditeam/GetEditdata/", data).then(function (promise) {

                $scope.CMMFI_FoodItemName = promise.gridviewDetails[0].cmmfI_FoodItemName;
                $scope.CMMFI_FoodItemDescription = promise.gridviewDetails[0].cmmfI_FoodItemDescription;
                $scope.CMMFI_UnitRate = promise.gridviewDetails[0].cmmfI_UnitRate;
                $scope.obj.CMMFI_Id = promise.gridviewDetails[0].cmmfI_Id;
                $scope.CMMFI_OutofStockFlg = promise.gridviewDetails[0].CMMFI_OutofStockFlg;
                $scope.cmmcA_CategoryName = promise.gridviewDetails[0].cmmcA_CategoryName;
                $scope.CMMFI_FoodItemFlag = promise.gridviewDetails[0].cmmfI_FoodItemFlag;
                $scope.ICAI_Attachment = promise.gridviewDetails[0].icaI_Attachment;
                $scope.cmmcA_Id = promise.gridviewDetails[0].cmmcA_Id;


                if (promise.attachementlist != null || promise.attachementlist > 0) {
                    $scope.documentListOtherDetails = [];
                    $scope.urldocumentlist = [];
                    angular.forEach(promise.attachementlist, function (aa) {
                        $scope.img = aa.icaI_Attachment;
                        if ($scope.img != null) {
                            var imagarr = $scope.img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];

                            $scope.filetype2 = lastelement;
                        }

                        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {

                            $scope.documentListOtherDetails.push({
                                id: 1, IHW_FilePath: aa.icaI_Attachment,
                                FileName: aa.icaI_FileName
                            });

                        }
                        else {
                            $scope.urldocumentlist.push({
                                id: 1, IHW_FilePath: aa.icaI_Attachment,
                                FileName: aa.icaI_FileName
                            });

                        }
                    })

                }



                

            })
        };
        //search filter
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.cmmfI_FoodItemName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.cmmfI_UnitRate)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        $scope.deactive = function (itm) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            var mgs = "";
            if (itm.cmmfI_ActiveFlg == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Category?",
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
                        apiService.create("Fooditeam/deactivate", itm).then(function (promise) {
                            swal(promise.returnval);
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });

        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.clear = function () {
            $state.reload();
        }
    }

})();