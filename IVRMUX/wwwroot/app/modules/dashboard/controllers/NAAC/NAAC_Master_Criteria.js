(function () {
    'use strict';
    angular.module('app').controller('NAAC_Master_CriteriaController', NAAC_Master_CriteriaController)
    NAAC_Master_CriteriaController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$filter', '$timeout', 'Excel', '$sce']
    function NAAC_Master_CriteriaController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $filter, $timeout, Excel, $sce) {

        $scope.NAACSL_TextBoxFlg_Tab1 = false;
        $scope.NAACSL_UploadReq_Tab1 = true;

        $scope.NAACSL_TextBoxFlg_Tab2 = false;
        $scope.NAACSL_UploadReq_Tab2 = true;

        $scope.NAACSL_TextBoxFlg_Tab3 = false;
        $scope.NAACSL_UploadReq_Tab3 = true;

        $scope.myTabIndex = 0;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.itemsPerPage2 = 10;
        $scope.currentPage2 = 1;

        $scope.itemsPerPage3 = 10;
        $scope.currentPage3 = 1;

        $scope.OnChangeInstituionType = function (Tab, NAACType) {
            $scope.submitted = false;
            $scope.NAACSL_Id_Tab1 = 0;
            $scope.NAACSL_SLNo_Tab1 = "";
            $scope.NAACSL_SLNote_Tab1 = "";
            $scope.NAACSL_SLNoDescription_Tab1 = "";
            $scope.NAACSL_Percentage_Tab1 = "";
            $scope.NAASCL_Template_Tab1 = "";
            $scope.NAACSL_TextBoxFlg_Tab1 = false;
            $scope.NAACSL_UploadReq_Tab1 = true;
            $scope.GetZeroParentIdDetails = [];

            $scope.GetSavedZeroPatentIdDetails = [];
            $scope.GetTabTwoData = [];
            $scope.ParentId_Tab2 = "";
            $scope.NAACSL_Id_Tab2 = 0;
            $scope.NAACSL_SLNo_Tab2 = "";
            $scope.NAACSL_SLNote_Tab2 = "";
            $scope.NAACSL_SLNoDescription_Tab2 = "";
            $scope.NAACSL_Percentage_Tab2 = "";
            $scope.NAASCL_Template_Tab2 = "";
            $scope.NAACSL_TextBoxFlg_Tab2 = false;
            $scope.NAACSL_UploadReq_Tab2 = true;

            $scope.GetTabThreeData = [];
            $scope.ParentId_Tab3 = "";
            $scope.NAACSL_Id_Tab3 = 0;
            $scope.NAACSL_SLNo_Tab3 = "";
            $scope.NAACSL_SLNote_Tab3 = "";
            $scope.NAACSL_SLNoDescription_Tab3 = "";
            $scope.NAACSL_Percentage_Tab3 = "";
            $scope.NAASCL_Template_Tab3 = "";
            $scope.NAACSL_TextBoxFlg_Tab3 = false;
            $scope.NAACSL_UploadReq_Tab3 = true;

            var data = {
                "TabFlag": Tab,
                "NAACSL_InstitutionTypeFlg": NAACType
            };
            apiService.create("NAAC_User_Privileges/OnChangeInstituionType", data).then(function (promise) {
                if (promise !== null) {

                    if (Tab === "Tab1") {
                        $scope.GetZeroParentIdDetails = promise.getZeroParentIdDetails;
                        $scope.GetZeroParentIdOrderDetails = promise.getZeroParentIdOrderDetails;
                    } else if (Tab === "Tab2") {
                        $scope.GetSavedZeroPatentIdDetails = promise.getSavedZeroPatentIdDetails;
                    } else if (Tab === "Tab3") {
                        $scope.GetSavedZeroPatentIdDetails = promise.getSavedZeroPatentIdDetails;
                    }
                }
            });
        };

        $scope.OnChangeCriteriaName = function (tab, parentid, naactype) {
            $scope.NAACSL_Id_Tab2 = 0;
            $scope.NAACSL_SLNo_Tab2 = "";
            $scope.NAACSL_SLNote_Tab2 = "";
            $scope.NAACSL_SLNoDescription_Tab2 = "";
            $scope.NAACSL_Percentage_Tab2 = "";
            $scope.NAASCL_Template_Tab2 = "";
            $scope.NAACSL_TextBoxFlg_Tab2 = false;
            $scope.NAACSL_UploadReq_Tab2 = true;
            $scope.GetTabTwoData = [];

            $scope.NAACSL_Id_Tab3 = 0;
            $scope.NAACSL_SLNo_Tab3 = "";
            $scope.NAACSL_SLNote_Tab3 = "";
            $scope.NAACSL_SLNoDescription_Tab3 = "";
            $scope.NAACSL_Percentage_Tab3 = "";
            $scope.NAASCL_Template_Tab3 = "";
            $scope.NAACSL_TextBoxFlg_Tab3 = false;
            $scope.NAACSL_UploadReq_Tab3 = true;
            $scope.GetTabThreeData = [];


            var data = {
                "TabFlag": tab,
                "NAACSL_InstitutionTypeFlg": naactype,
                "NAACSL_Id": parentid
            };

            apiService.create("NAAC_User_Privileges/OnChangeCriteriaName", data).then(function (promise) {
                if (promise !== null) {
                    if (tab === "Tab2") {
                        $scope.GetTabTwoData = promise.getTabTwoData;
                        $scope.GetTabTwoDataOrder = promise.getTabTwoDataOrder;
                    }
                    if (tab === "Tab3") {
                        $scope.GetTabThreeData = promise.getTabThreeData;
                        $scope.GetTabTwoDataOrder = promise.getTabTwoDataOrder;
                    }
                }
            });
        };

        //Tab 1

        $scope.OnClickTab1 = function () {
            $scope.submitted = false;
            $scope.NAACSL_Id_Tab1 = 0;
            $scope.NAACSL_SLNo_Tab1 = "";
            $scope.NAACSL_SLNote_Tab1 = "";
            $scope.NAACSL_SLNoDescription_Tab1 = "";
            $scope.NAACSL_InstitutionTypeFlg_Tab1 = "";
            $scope.NAACSL_Percentage_Tab1 = "";
            $scope.NAASCL_Template_Tab1 = "";
            $scope.searchValue = "";
            $scope.NAACSL_TextBoxFlg_Tab1 = false;
            $scope.NAACSL_UploadReq_Tab1 = true;
            $scope.GetZeroParentIdDetails = [];
        };

        $scope.SaveTab1 = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "NAACSL_Id": $scope.NAACSL_Id_Tab1,
                    "NAACSL_InstitutionTypeFlg": $scope.NAACSL_InstitutionTypeFlg_Tab1,
                    "NAACSL_SLNo": $scope.NAACSL_SLNo_Tab1,
                    "NAACSL_SLNote": $scope.NAACSL_SLNote_Tab1,
                    "NAACSL_SLNoDescription": $scope.NAACSL_SLNoDescription_Tab1,
                    "NAACSL_TextBoxFlg": $scope.NAACSL_TextBoxFlg_Tab1,
                    "NAACSL_UploadReq": $scope.NAACSL_UploadReq_Tab1,
                    "NAASCL_Template": $scope.NAASCL_Template_Tab1,
                    "NAACSL_Percentage": $scope.NAACSL_Percentage_Tab1
                };

                apiService.create("NAAC_User_Privileges/SaveTab1", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmessage == "") {
                            if ($scope.NAACSL_Id_Tab1 > 0) {
                                if (promise.returnval === true) {
                                    swal("Record Updated Successfully");
                                } else {
                                    swal("Failed To Update Record");
                                }
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record Saved Successfully");
                                } else {
                                    swal("Failed To Save Record");
                                }
                            }
                        } else {
                            swal("Record Already Exists");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.EditTab1 = function (user) {
            var data = {
                "NAACSL_Id": user.NAACSL_Id,
            };
            apiService.create("NAAC_User_Privileges/EditTab1", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetEditTabOneDetails = promise.getEditTabOneDetails;

                    $scope.NAACSL_Id_Tab1 = $scope.GetEditTabOneDetails[0].naacsL_Id;
                    $scope.NAACSL_InstitutionTypeFlg_Tab1 = $scope.GetEditTabOneDetails[0].naacsL_InstitutionTypeFlg;
                    $scope.NAACSL_SLNote_Tab1 = $scope.GetEditTabOneDetails[0].NAACSL_SLNote;
                    $scope.NAACSL_SLNoDescription_Tab1 = $scope.GetEditTabOneDetails[0].naacsL_SLNoDescription;
                    $scope.NAACSL_UploadReq_Tab1 = $scope.GetEditTabOneDetails[0].naacsL_UploadReq === null ? false : $scope.GetEditTabOneDetails[0].naacsL_UploadReq;
                    $scope.NAACSL_TextBoxFlg_Tab1 = $scope.GetEditTabOneDetails[0].naacsL_TextBoxFlg === null ? false : $scope.GetEditTabOneDetails[0].naacsL_TextBoxFlg;
                    $scope.NAASCL_Template_Tab1 = $scope.GetEditTabOneDetails[0].naascL_Template;
                    $scope.NAACSL_Percentage = $scope.GetEditTabOneDetails[0].naacsL_Percentage_Tab1;

                    if ($scope.NAASCL_Template_Tab1 !== null && $scope.NAASCL_Template_Tab1 !== "") {
                        var img = $scope.NAASCL_Template_Tab1;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.filetype_Tab1 = lastelement;
                    }
                }
            });
        };

        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $scope.OnClickTab1();
        };


        // Tab2
        $scope.OnClickTab2 = function () {
            $scope.submitted_Tab2 = false;
            $scope.NAACSL_Id_Tab2 = 0;
            $scope.ParentId_Tab2 = "";
            $scope.searchValue2 = "";
            $scope.NAACSL_SLNo_Tab2 = "";
            $scope.NAACSL_SLNote_Tab2 = "";
            $scope.NAACSL_SLNoDescription_Tab2 = "";
            $scope.NAACSL_InstitutionTypeFlg_Tab2 = "";
            $scope.NAACSL_Percentage_Tab2 = "";
            $scope.NAASCL_Template_Tab2 = "";
            $scope.NAACSL_TextBoxFlg_Tab2 = false;
            $scope.NAACSL_UploadReq_Tab2 = true;
            $scope.GetZeroParentIdDetails = [];
            $scope.GetSavedZeroPatentIdDetails = [];
            $scope.GetTabTwoData = [];
            $scope.ParentId_Tab2 = "";
        };

        $scope.SaveTab2 = function () {
            if ($scope.myForm2.$valid) {
                var data = {
                    "NAACSL_Id": $scope.NAACSL_Id_Tab2,
                    "ParentId": $scope.ParentId_Tab2,
                    "NAACSL_InstitutionTypeFlg": $scope.NAACSL_InstitutionTypeFlg_Tab2,
                    "NAACSL_SLNo": $scope.NAACSL_SLNo_Tab2,
                    "NAACSL_SLNote": $scope.NAACSL_SLNote_Tab2,
                    "NAACSL_SLNoDescription": $scope.NAACSL_SLNoDescription_Tab2,
                    "NAACSL_TextBoxFlg": $scope.NAACSL_TextBoxFlg_Tab2,
                    "NAACSL_UploadReq": $scope.NAACSL_UploadReq_Tab2,
                    "NAASCL_Template": $scope.NAASCL_Template_Tab2,
                    "NAACSL_Percentage": $scope.NAACSL_Percentage_Tab2
                };

                apiService.create("NAAC_User_Privileges/SaveTab2", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmessage === "") {
                            if ($scope.NAACSL_Id_Tab2 > 0) {
                                if (promise.returnval === true) {
                                    swal("Record Updated Successfully");
                                } else {
                                    swal("Failed To Update Record");
                                }
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record Saved Successfully");
                                } else {
                                    swal("Failed To Save Record");
                                }
                            }
                        } else {
                            swal("Record Already Exists")
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted_Tab2 = true;
            }
        };

        $scope.EditTab2 = function (user) {
            var data = {
                "NAACSL_Id": user.naacsL_Id
            };

            apiService.create("NAAC_User_Privileges/EditTab2", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetEditTabTwoDetails = promise.getEditTabTwoDetails;

                    $scope.NAACSL_Id_Tab2 = $scope.GetEditTabTwoDetails[0].naacsL_Id;
                    $scope.ParentId_Tab2 = $scope.GetEditTabTwoDetails[0].naacsL_ParentId;
                    $scope.NAACSL_InstitutionTypeFlg_Tab2 = $scope.GetEditTabTwoDetails[0].naacsL_InstitutionTypeFlg;
                    $scope.NAACSL_SLNote_Tab2 = $scope.GetEditTabTwoDetails[0].naacsL_SLNote;
                    $scope.NAACSL_SLNo_Tab2 = $scope.GetEditTabTwoDetails[0].naacsL_SLNo;
                    $scope.NAACSL_SLNoDescription_Tab2 = $scope.GetEditTabTwoDetails[0].naacsL_SLNoDescription;
                    $scope.NAACSL_UploadReq_Tab2 = $scope.GetEditTabTwoDetails[0].naacsL_UploadReq === null ? false : $scope.GetEditTabTwoDetails[0].naacsL_UploadReq;
                    $scope.NAACSL_TextBoxFlg_Tab2 = $scope.GetEditTabTwoDetails[0].naacsL_TextBoxFlg === null ? false : $scope.GetEditTabTwoDetails[0].naacsL_TextBoxFlg;
                    $scope.NAASCL_Template_Tab2 = $scope.GetEditTabTwoDetails[0].naascL_Template;
                    $scope.NAACSL_Percentage = $scope.GetEditTabTwoDetails[0].naacsL_Percentage_Tab2;

                    if ($scope.NAASCL_Template_Tab2 !== null && $scope.NAASCL_Template_Tab2 !== "") {
                        var img = $scope.NAASCL_Template_Tab2;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.filetype_Tab2 = lastelement;
                    }
                }
            });
        };

        $scope.submitted_Tab2 = false;

        $scope.interacted2 = function (field) {
            return $scope.submitted_Tab2;
        };

        $scope.cancel2 = function () {
            $scope.OnClickTab2();
        };

        //Tab 3
        $scope.OnClickTab3 = function () {
            $scope.submitted_Tab3 = false;
            $scope.NAACSL_Id_Tab3 = 0;
            $scope.ParentId_Tab3 = "";
            $scope.ParentId_Tab3_1 = "";
            $scope.searchValue3 = "";
            $scope.NAACSL_SLNo_Tab3 = "";
            $scope.NAACSL_SLNote_Tab3 = "";
            $scope.NAACSL_SLNoDescription_Tab3 = "";
            $scope.NAACSL_InstitutionTypeFlg_Tab3 = "";
            $scope.NAACSL_Percentage_Tab3 = "";
            $scope.NAASCL_Template_Tab3 = "";
            $scope.NAACSL_TextBoxFlg_Tab3 = false;
            $scope.NAACSL_UploadReq_Tab3 = true;
            $scope.GetZeroParentIdDetails = [];
            $scope.GetSavedZeroPatentIdDetails = [];
            $scope.GetEditTabThreeDetails = [];
            $scope.GetTabThreeData = [];
        };

        $scope.OnChangeCriteriaNameLevelOne = function (tab) {
            $scope.ParentId_Tab3_1 = "";
            $scope.NAACSL_SLNo_Tab3 = "";
            $scope.NAACSL_SLNote_Tab3 = "";
            $scope.NAACSL_Percentage_Tab3 = "";
            $scope.NAACSL_SLNoDescription_Tab3 = "";
            $scope.NAASCL_Template_Tab3 = "";
            $scope.NAACSL_TextBoxFlg_Tab3 = false;
            $scope.NAACSL_UploadReq_Tab3 = true;
            $scope.GetSavedLevelOnePatentIdDetails = [];
            $scope.GetTabThreeData = [];
            $scope.GetSavedPatentIdDetails = [];

            var data = {
                "TabFlag": tab,
                "NAACSL_InstitutionTypeFlg": $scope.NAACSL_InstitutionTypeFlg_Tab3,
                "NAACSL_Id": $scope.ParentId_Tab3
            };

            apiService.create("NAAC_User_Privileges/OnChangeCriteriaNameLevelOne", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetSavedPatentIdDetails = promise.getSavedPatentIdDetails;
                }
            });
        };

        $scope.SaveTab3 = function () {

            if ($scope.myForm3.$valid) {
                var data = {
                    "NAACSL_Id": $scope.NAACSL_Id_Tab3,
                    "ParentId": $scope.ParentId_Tab3_1,
                    "NAACSL_InstitutionTypeFlg": $scope.NAACSL_InstitutionTypeFlg_Tab3,
                    "NAACSL_SLNo": $scope.NAACSL_SLNo_Tab3,
                    "NAACSL_SLNote": $scope.NAACSL_SLNote_Tab3,
                    "NAACSL_SLNoDescription": $scope.NAACSL_SLNoDescription_Tab3,
                    "NAACSL_TextBoxFlg": $scope.NAACSL_TextBoxFlg_Tab3,
                    "NAACSL_UploadReq": $scope.NAACSL_UploadReq_Tab3,
                    "NAASCL_Template": $scope.NAASCL_Template_Tab3,
                    "NAACSL_Percentage": $scope.NAACSL_Percentage_Tab3
                };

                apiService.create("NAAC_User_Privileges/SaveTab3", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmessage === "") {
                            if ($scope.NAACSL_Id_Tab3 > 0) {
                                if (promise.returnval === true) {
                                    swal("Record Updated Successfully");
                                } else {
                                    swal("Failed To Update Record");
                                }
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record Saved Successfully");
                                } else {
                                    swal("Failed To Save Record");
                                }
                            }
                        } else {
                            swal("Record Already Exists")
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted_Tab3 = true;
            }
        };

        $scope.EditTab3 = function (user) {
            $scope.GetEditTabThreeDetails = [];
            var data = {
                "NAACSL_Id": user.naacsL_Id
            };

            apiService.create("NAAC_User_Privileges/EditTab3", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetEditTabThreeDetails = promise.getEditTabThreeDetails;

                    $scope.NAACSL_Id_Tab3 = $scope.GetEditTabThreeDetails[0].naacsL_Id;
                    //$scope.ParentId_Tab3 = $scope.GetEditTabThreeDetails[0].naacsL_ParentId;
                    $scope.NAACSL_InstitutionTypeFlg_Tab3 = $scope.GetEditTabThreeDetails[0].naacsL_InstitutionTypeFlg;
                    $scope.NAACSL_SLNote_Tab3 = $scope.GetEditTabThreeDetails[0].naacsL_SLNote;
                    $scope.NAACSL_SLNo_Tab3 = $scope.GetEditTabThreeDetails[0].naacsL_SLNo;
                    $scope.NAACSL_SLNoDescription_Tab3 = $scope.GetEditTabThreeDetails[0].naacsL_SLNoDescription;
                    $scope.NAACSL_UploadReq_Tab3 = $scope.GetEditTabThreeDetails[0].naacsL_UploadReq === null ? false : $scope.GetEditTabThreeDetails[0].naacsL_UploadReq;
                    $scope.NAACSL_TextBoxFlg_Tab3 = $scope.GetEditTabThreeDetails[0].naacsL_TextBoxFlg === null ? false :
                        $scope.GetEditTabThreeDetails[0].naacsL_TextBoxFlg;
                    $scope.NAASCL_Template_Tab3 = $scope.GetEditTabThreeDetails[0].naascL_Template;
                    $scope.NAACSL_Percentage = $scope.GetEditTabThreeDetails[0].naacsL_Percentage_Tab3;

                    if ($scope.NAASCL_Template_Tab3 !== null && $scope.NAASCL_Template_Tab3 !== "") {
                        var img = $scope.NAASCL_Template_Tab3;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.filetype_Tab3 = lastelement;
                    }
                }
            });
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted_Tab3;
        };

        $scope.cancel3 = function () {
            $scope.OnClickTab3();
        };

        // General Functions

        $scope.SelectedFileForUploadzd = [];

        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {
                if ((input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword"
                    || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "image/jpeg")) {
                    if (input.files[0].size <= 2097152) {

                        if (document === "Tab1") {
                            $scope.filename_tab1 = input.files[0].name;
                        }
                        else if (document === "Tab2") {
                            $scope.filename_tab1 = input.files[0].name;
                        }

                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#id').attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        Uploadprofiled(document);
                    } else {
                        swal("File Size Should Be Less Than 2 MB"); // 2097152 bytes = 2MB 
                    }
                }
                else {
                    swal("Upload Only Pdf, Doc , Excel And Image Files Only");
                    return;
                }
            }
        };

        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            formData.append("Id", data.naacsL_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadMasterNaacDocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (data === "Tab1") {
                        $scope.NAASCL_Template_Tab1 = d;
                    }
                    else if (data === "Tab2") {
                        $scope.NAASCL_Template_Tab2 = d;
                    }
                    else if (data === "Tab2") {
                        $scope.NAASCL_Template_Tab3 = d;
                    }

                    var img = d;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];

                    if (data === "Tab1") {
                        $scope.filetype_tab1 = lastelement;
                    }
                    else if (data === "Tab2") {
                        $scope.filetype_Tab2 = lastelement;
                    }
                    else if (data === "Tab2") {
                        $scope.filetype_Tab3 = lastelement;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            console.log(data);
        }

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('#myModalimg').modal('show');
        };

        $scope.downloadview = function (pdfview) {

            var imagedownload = pdfview;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.onviewdocuments = function (filepath, filename) {
            var docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + filepath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

        $scope.downloaddirectimage = function (data, idd) {
            var studentreg = "Template";
            var imagedownload = "";

            var img = data;
            var imagarr = img.split('.');
            var lastelement = imagarr[imagarr.length - 1];

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.' + lastelement
                    })[0].click();
                });
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
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