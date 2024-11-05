(function () {
    'use strict';
    angular.module('app').controller('External_Training_ApprovalController', External_Training_ApprovalController)

    External_Training_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function External_Training_ApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            $scope.search = "";
            var pageid = 2;
            apiService.getURI("External_Training_Approval/onloaddata", pageid).then(function (promise) {
                $scope.getloaddetails = promise.getloaddetails;
                $scope.getloaddetailsTT = promise.loadgrid;

            });
        };

        //===========Get Item deatils On item Change
        $scope.all_checkCCCC = function () {
            var mI_Id = $scope.mI_Id;
            $scope.getdata = [];
            angular.forEach($scope.Institution, function (itm) {
                itm.selected = mI_Id;
            });
            $scope.storechanges();
        };
        $scope.togchkbxCCCC = function () {
            $scope.getdata = [];
            $scope.mI_Id = $scope.Institution.every(function (options) {
                return options.selected;
                return options.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.getloaddetails.some(function (options) {
                return options.chkitem;
            });
        };

        $scope.isOptionsRequiredtwo = function () {
            return !$scope.Institution.some(function (options) {
                return options.selected;
            });
        };




        $scope.viewcomment= function (aaa) {


                var data = {
                    "HREXTTRN_Id":aaa
                };
                apiService.create("External_Training_Approval/trainingdetails", data).then(function (promise) {

                    $scope.trainingdetails = promise.trainingdetails;

                    $scope.emplYoeeName = $scope.trainingdetails[0].emplYoeeName;


                });
        };



        $scope.submitted = false;

        $scope.approvalstatus = function (tp) {
            var HREXTTRN_ApprovedFlg = 0;
            $scope.submitted = true;
            if ($scope.myform2.$valid) {
                var HREXTTRN_ApprovedFlg = "";
                if (tp == 'A') {
                    HREXTTRN_ApprovedFlg ="A";
                }
                else {
                    HREXTTRN_ApprovedFlg = "R";
                }
                    $scope.Arraysthree = [];
                    angular.forEach($scope.getloaddetails, function (comp) {
                        if (comp.chkitem == true) {
                            $scope.Arraysthree.push({
                                HREXTTRN_Id: comp.HREXTTRN_Id,
                                HREXTTRNAPP_ApproverRemarks: comp.hrexttrnapP_ApproverRemarks,
                                HREXTTRNAPP_ApprovedHrs: comp.hrexttrnapP_ApprovedHrs
                            })
                        }
                    });
               
                var data = {
                    "multiapproval": $scope.Arraysthree,
                    "HREXTTRN_ApprovedFlg1": HREXTTRN_ApprovedFlg
                };
                apiService.create("External_Training_Approval/approvalstatus", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval == "admin") {
                            swal("Contact adminstator");
                        }
                        else if (promise.returnval == "save") {
                            swal("Record saved successfully")
                        }
                        else if (promise.returnval == "Notsave")
                        {
                            swal("Record not saved successfully")
                        }
                        $state.reload();
                    }
                    else {
                        swal("Contact adminstator");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.EditData = function (user) {
            $scope.HRMETRCEN_Id = user.hrmetrceN_Id;
            $scope.Training_Center_Name = user.hrmetrceN_TrainingCenterName;
            $scope.Contact_Person_Name = user.hrmetrceN_ContactPersonName;
            $scope.ContactNo = user.hrmetrceN_ContactNo;
            $scope.Contact_Email_Id = user.hrmetrceN_ContactEmailId;
            $scope.CenterAddress = user.hrmetrceN_CenterAddress;
        };


        $scope.deactiveY = function (user, SweetAlert) {
            var data = {
                "HRMETRCEN_Id": user.hrmetrceN_Id
            };

            var dystring = "";
            if (user.hrmetrceN_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hrmetrceN_ActiveFlag === false) {
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
                        apiService.create("External_Training_Approval/deactiveY", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };




        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
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
        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }


        $scope.content1 = "";
        ///=====================show pdf, img
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;


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
        };


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        ///=====================show pdf, img end





        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();

