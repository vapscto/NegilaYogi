(function () {
    'use strict';
    angular.module('app').controller('AdmissionStandardController', AdmissionStandardController)
    AdmissionStandardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$q']
    function AdmissionStandardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q) {

        $scope.pagesrecord = {};

        var studclear = [];
        $scope.imgflag = false;;
        var ascid;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        ///////////////////////////////////////Saving Functionlity///////////////////////////////////////////////////////////////////////////
        $scope.savedata = function (ascid) {
            if ($scope.myForm.$valid) {
                if ($scope.idvalue != null) {
                    ascid = $scope.idvalue;
                }
                else {
                    ascid = 0;
                }

                if ($scope.ASC_Adm_AddFieldsFlag == true) {
                    $scope.ASC_Adm_AddFieldsFlag = 1;
                }
                else {
                    $scope.ASC_Adm_AddFieldsFlag = 0;
                }
                if ($scope.ASC_TC_AddFieldsFlag == true) {
                    $scope.ASC_TC_AddFieldsFlag = 1;
                }
                else {
                    $scope.ASC_TC_AddFieldsFlag = 0;
                }
                if ($scope.ASC_MaxAgeApl_Flag == true) {
                    $scope.ASC_MaxAgeApl_Flag = 1;
                }
                else {
                    $scope.ASC_MaxAgeApl_Flag = 0;
                }
                if ($scope.ASC_MinAgeApl_Flag == true) {
                    $scope.ASC_MinAgeApl_Flag = 1;
                }
                else {
                    $scope.ASC_MinAgeApl_Flag = 0;
                }

                if ($scope.ASC_TC_Payment == true) {
                    $scope.ASC_TC_Payment = 1;
                }
                else {
                    $scope.ASC_TC_Payment = 0;
                }
                //Newly addded
                if ($scope.ASC_TC_Library == true) {
                    $scope.ASC_TC_Library = 1;
                }
                else {
                    $scope.ASC_TC_Library = 0;
                }

                var ASC_ECS_Flag_New = 0;
                if ($scope.ASC_ECS_Flag == true) {
                    ASC_ECS_Flag_New = 1;
                }
                else {
                    ASC_ECS_Flag_New = 0;
                }

                if ($scope.ASC_Att_DefaultEntry_Type == 'A' || $scope.ASC_Att_DefaultEntry_Type == 'P') {
                    //dd
                }
                else {
                    $scope.ASC_Att_DefaultEntry_Type = 0;
                }

                if ($scope.ASC_Default_Gender == 'M' || $scope.ASC_Default_Gender == 'F' || $scope.ASC_Default_Gender == 'Ot') {
                    //dd
                }
                else {
                    $scope.ASC_Default_Gender = 0;
                }

                if ($scope.ASC_DefaultSMS_Flag == 'F') {
                    $scope.ASC_DefaultSMS_Flag = 'F';
                }
                else if ($scope.ASC_DefaultSMS_Flag == 'M') {
                    $scope.ASC_DefaultSMS_Flag = 'M';
                }
                else if ($scope.ASC_DefaultSMS_Flag == 'S') {
                    $scope.ASC_DefaultSMS_Flag = 'S';
                }
                else {
                    $scope.ASC_DefaultSMS_Flag = 0;
                }

                if ($scope.ASC_DefaultPhotoUpload == 1 || $scope.ASC_DefaultPhotoUpload == 2) {
                    //dd
                }
                else {
                    $scope.ASC_DefaultPhotoUpload = 0;
                }

                if ($scope.ASC_ParentsMonthlyIncome_Flag == true) {
                    $scope.ASC_ParentsMonthlyIncome_Flag = 1;
                }
                else {
                    $scope.ASC_ParentsMonthlyIncome_Flag = 0;
                }

                if ($scope.ASC_ParentsAnnualIncome_Flag == true) {
                    $scope.ASC_ParentsAnnualIncome_Flag = 1;
                }
                else {
                    $scope.ASC_ParentsAnnualIncome_Flag = 0;
                }

                if ($scope.ASC_School_Address == true) {
                    $scope.ASC_School_Address = 1;
                }
                else {
                    $scope.ASC_School_Address = 0;
                }

                if ($scope.ASC_Category_Address == true) {
                    $scope.ASC_Category_Address = 1;
                }
                else {
                    $scope.ASC_Category_Address = 0;
                }

                if ($scope.ASC_Att_Default_OrderFlag != 0 || $scope.ASC_Att_Default_OrderFlag != null) {
                    //dd
                }
                else {
                    $scope.ASC_Att_Default_OrderFlag = 0;
                }

                var data = {
                    "ASC_Adm_AddFieldsFlag": $scope.ASC_Adm_AddFieldsFlag,
                    "ASC_TC_AddFieldsFlag": $scope.ASC_TC_AddFieldsFlag,
                    "ASC_MaxAgeApl_Flag": $scope.ASC_MaxAgeApl_Flag,
                    "ASC_MinAgeApl_Flag": $scope.ASC_MinAgeApl_Flag,
                    "ASC_Att_DefaultEntry_Type": $scope.ASC_Att_DefaultEntry_Type,
                    "ASC_Default_Gender": $scope.ASC_Default_Gender,
                    "ASC_ParentsMonthlyIncome_Flag": $scope.ASC_ParentsMonthlyIncome_Flag,
                    "ASC_ParentsAnnualIncome_Flag": $scope.ASC_ParentsAnnualIncome_Flag,
                    "ASC_School_Address": $scope.ASC_School_Address,
                    "ASC_Category_Address": $scope.ASC_Category_Address,
                    "ASC_DefaultPhotoUpload": $scope.ASC_DefaultPhotoUpload,
                    "ASC_Id": $scope.ascid,
                    "ASC_Att_Default_OrderFlag": $scope.ASC_Att_Default_OrderFlag,
                    "ASC_Stu_Photo_Path": $scope.ASC_Stu_Photo_Path,
                    "ASC_Staff_Photo_Path": $scope.ASC_Staff_Photo_Path,
                    "ASC_Logo_Path": $scope.ASC_Logo_Path,
                    "ASC_Doc_Path": $scope.ASC_Doc_Path,
                    "ASC_DefaultSMS_Flag": $scope.ASC_DefaultSMS_Flag,
                    "ASC_TC_Payment": $scope.ASC_TC_Payment,
                    "ASC_TC_Library": $scope.ASC_TC_Library,
                    "ASC_ECS_Flag": ASC_ECS_Flag_New,
                    "ASC_Att_Scheduler_Flag": $scope.ASC_Att_Scheduler_Flag
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("AdmissionStandard/savedata/", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Record Saved Successfully', 'success');
                        $state.reload();
                    }
                    else {
                        //swal('Record Successfully Updated');
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };




        //image upload
        $scope.SelectedFileForUploadzl = [];
        $scope.selectFileforUploadzl = function (input) {
            $scope.imgflag = true;
            $scope.SelectedFileForUploadzl = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#Logo')
                        .attr('src', e.target.result)
                };
                reader.readAsDataURL(input.files[0]);
                $scope.AMC_Logo = input.files[0].name;

                for (var i = 0; i < 1; i++) {
                    var file = $scope.SelectedFileForUploadzl[i];
                    $scope.fileimg = file;
                    var reader = new FileReader();
                    reader.onload = $scope.imageIsLoaded;
                    reader.readAsDataURL(file);
                }
                Uploads1();
            }
        }

        function Uploads1() {
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzl.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzl[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_GalleryImgVideos", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.images_paths = d;
                    //swal(d);
                    $scope.ASC_Logo_Path = d[0];
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }
        $scope.stepsModel = [];

        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };
        $scope.remove_img = function (reimg) {
            for (var i = 0; i < $scope.files.length; i++) {
                var imgt1 = $scope.files[i];
                if (imgt1.name === reimg.name) {
                    $scope.stepsModel.splice(i, 1);
                }
            }
        };
        $scope.imagepath = $scope.stepsModel[0];

//

        //=============================== Preview Image
        $scope.previewimg = function (item) {
            //  $scope.imagepreview = img;

            // $scope.imagepreview = img;
            $scope.imagepreview = $scope.images_paths[0];
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {


                $('#preview').attr('src', $scope.imagepreview);
                $('#myModalPreview').modal('show');

            }

            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'pdf') {
                $scope.previewpdf(img, $scope.filetype2);
                // $('#showpdf').modal('show');

            }

        };
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        $scope.loaddata = function () {
            var data = 2;
            apiService.getURI("AdmissionStandard/loaddata/", data).then(function (promise) {

                if (promise.fillconfig.length > 0) {
                    $scope.cancelDis = true;
                    $scope.reset = false;
                    $scope.idvalue = promise.fillconfig[0].asC_Id;
                    if (promise.fillconfig[0].asC_Adm_AddFieldsFlag == 1) {
                        $scope.ASC_Adm_AddFieldsFlag = 1;
                    }
                    else {
                        $scope.ASC_Adm_AddFieldsFlag = 0;
                    }

                    if (promise.fillconfig[0].asC_TC_AddFieldsFlag == 1) {
                        $scope.ASC_TC_AddFieldsFlag = 1;
                    }
                    else {
                        $scope.ASC_TC_AddFieldsFlag = 0;
                    }
                    if (promise.fillconfig[0].asC_MaxAgeApl_Flag == 1) {
                        $scope.ASC_MaxAgeApl_Flag = 1;
                    }
                    else {
                        $scope.ASC_MaxAgeApl_Flag = 0;
                    }
                    if (promise.fillconfig[0].asC_MinAgeApl_Flag == 1) {
                        $scope.ASC_MinAgeApl_Flag = 1;
                    }
                    else {
                        $scope.ASC_MinAgeApl_Flag = 0;
                    }

                    if (promise.fillconfig[0].admC_TCAllowBalanceFlg == 1) {
                        $scope.ASC_TC_Payment = 1;
                    }
                    else {
                        $scope.ASC_TC_Payment = 0;
                    }
                    if (promise.fillconfig[0].asC_LibraryAllowBalanceFlg == 1) {
                        $scope.ASC_TC_Library = 1;
                    }
                    else {
                        $scope.ASC_TC_Library = 0;
                    }

                    if (promise.fillconfig[0].asC_ECS_Flag == 1) {
                        $scope.ASC_ECS_Flag = 1;
                    }
                    else {
                        $scope.ASC_ECS_Flag = 0;
                    }

                    if (promise.fillconfig[0].asC_Att_DefaultEntry_Type == 'P') {
                        $scope.ASC_Att_DefaultEntry_Type = 'P';
                    }
                    else if (promise.fillconfig[0].asC_Att_DefaultEntry_Type == 'A') {
                        $scope.ASC_Att_DefaultEntry_Type = 'A';
                    }

                    if (promise.fillconfig[0].asC_Default_Gender == 'M') {
                        $scope.ASC_Default_Gender = 'M';
                    }
                    else if (promise.fillconfig[0].asC_Default_Gender == 'F') {
                        $scope.ASC_Default_Gender = 'F';
                    }
                    else if (promise.fillconfig[0].asC_Default_Gender == 'Ot') {
                        $scope.ASC_Default_Gender = 'Ot';
                    }

                    if (promise.fillconfig[0].asC_DefaultSMS_Flag == 'F') {
                        $scope.ASC_DefaultSMS_Flag = 'F';
                    }
                    else if (promise.fillconfig[0].asC_DefaultSMS_Flag == 'M') {
                        $scope.ASC_DefaultSMS_Flag = 'M';
                    }
                    else if (promise.fillconfig[0].asC_DefaultSMS_Flag == 'S') {
                        $scope.ASC_DefaultSMS_Flag = 'S';
                    }

                    if (promise.fillconfig[0].asC_ParentsMonthlyIncome_Flag == 1) {
                        $scope.ASC_ParentsMonthlyIncome_Flag = 1;
                    }
                    else {
                        $scope.ASC_ParentsMonthlyIncome_Flag = 0;
                    }

                    if (promise.fillconfig[0].asC_ParentsAnnualIncome_Flag == 1) {
                        $scope.ASC_ParentsAnnualIncome_Flag = 1;
                    }
                    else {
                        $scope.ASC_ParentsAnnualIncome_Flag = 0;
                    }
                    if (promise.fillconfig[0].asC_School_Address == 1) {
                        $scope.ASC_School_Address = 1;
                    }
                    else {
                        $scope.ASC_School_Address = 0;
                    }
                    if (promise.fillconfig[0].asC_Category_Address == 1) {
                        $scope.ASC_Category_Address = 1;
                    }
                    else {
                        $scope.ASC_Category_Address = 0;
                    }

                    if (promise.fillconfig[0].asC_DefaultPhotoUpload == "1" || promise.fillconfig[0].asC_DefaultPhotoUpload == "2") {
                        $scope.ASC_DefaultPhotoUpload = promise.fillconfig[0].asC_DefaultPhotoUpload;
                    }
                    else {
                        $scope.ASC_DefaultPhotoUpload = "0";
                    }
                    if (promise.fillconfig[0].asC_Att_Default_OrderFlag != 0) {
                        $scope.ASC_Att_Default_OrderFlag = promise.fillconfig[0].asC_Att_Default_OrderFlag;
                    }
                    else {
                        $scope.ASC_Att_Default_OrderFlag = 0;
                    }

                    $scope.ASC_Stu_Photo_Path = promise.fillconfig[0].asC_Stu_Photo_Path;
                    $scope.ASC_Staff_Photo_Path = promise.fillconfig[0].asC_Staff_Photo_Path;
                    $scope.ASC_Logo_Path = promise.fillconfig[0].asC_Logo_Path;
                    //var reader = new FileReader();
                    //reader.onload = function (e) {
                    //    $('#Logo')
                    //        .attr('src', $scope.ASC_Logo_Path)
                    //};
                    $scope.ASC_Doc_Path = promise.fillconfig[0].asC_Doc_Path;
                    $scope.ASC_Att_Scheduler_Flag = promise.fillconfig[0].asC_Att_Scheduler_Flag;
                    $scope.ascid = promise.fillconfig[0].asC_Id;
                }
                else {
                    $scope.cancelDis = false;
                    $scope.reset = true;
                }
            });
        };


        $scope.cancel = function () {
            $state.reload();
        };

        $scope.resetData = function () {
            $scope.loaddata();
        };
    }
})();

