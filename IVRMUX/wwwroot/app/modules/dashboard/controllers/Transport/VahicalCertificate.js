
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VahicalCertificateController', VahicalCertificateController)

    VahicalCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$sce']
    function VahicalCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $sce) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        // $scope.vehicledocuupload = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.masterlist = false;
        $scope.sortKey = 'trdC_Id';
        $scope.sortReverse = true;
        $scope.vehicledocuupload = {};
        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";
        $scope.modeofPayment1 = function (val) {
            $scope.reqSelection = false;
            $scope.TRVCT_ModeOfPayment = val;
            if (val == 'Cheque' || val == 'DD' || val == 'Bank') {
                $scope.Cheque = true;
            }
            else {
                $scope.Cheque = false;
            }
            if (val == 'Online Payment') {
                $scope.online = true;

            } else {
                $scope.online = false;
            }
        }

        $scope.removeNewMobile1std = function (index, curval1std) {

            var newItemNostd2 = $scope.mobilesstd.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.mobilesstd.splice(index, 1);
            }


        };
        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };
        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd' + newItemNostd2 });
            }
        };
        $scope.mobilesstd = [{ id: 'mobilestd1' }];
        //  $scope.selmobsstd = [{ id: 'selmobilestd1' }];
        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.mobilesstd.length + 1;
            if (newItemNostd <= 5) {
                $scope.mobilesstd.push({ 'id': 'mobilestd' + newItemNostd });
            }


            if (newItemNostd == 4) {
                $scope.myForm1.$setPristine();
            }

        };


        $scope.removeNewEmail1std = function (index, id) {



            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);


            }

        }






        $scope.reqSelection = true;
        $scope.certdis = false;
        $scope.typelist = []
        // $scope.Cheque = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("VahicalCertificate/getdata/", pageid).then(function (promise) {
                $scope.masterdistancerate = promise.getloaddata;
                $scope.fillvahicleno = promise.fillvahicleno;
                $scope.modeOfPaymentList2 = promise.modeOfPaymentList;


                $scope.typelist = [];
                $scope.typelist.push({ id: 1, type: 'Vehicle Emission Test' })
                $scope.typelist.push({ id: 2, type: 'Vehicle Tax' })
                $scope.typelist.push({ id: 3, type: 'Vehicle Speed' })
                $scope.typelist.push({ id: 4, type: 'Vehicle Fitness Test' })
                $scope.typelist.push({ id: 5, type: 'Vehicle CeaseFire' })
                $scope.typelist.push({ id: 6, type: 'Vehicle Permit' })
                $scope.typelist.push({ id: 7, type: 'Vehicle Green Tax' })
                $scope.typelist.push({ id: 8, type: 'Vehicle Insurance' })
                $scope.typelist.push({ id: 9, type: 'GPRS' })


                if (promise.getloaddata.length > 0) {


                    //
                    //$scope.presentCountgrid = promise.getloaddata.length;
                    $scope.masterlist = true;
                }
                else {
                    swal("No Records Found");
                    $scope.masterlist = false;
                }
            })
        }



        $scope.delete = function (DeleteRecord, SweetAlert) {
            $scope.deleteId = DeleteRecord.trvcT_Id;
            var MdeleteId = $scope.deleteId;
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("VahicalCertificate/deleterecord", MdeleteId).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record Deleted successfully");

                                }
                                else {
                                    swal("Error");

                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                });
        }













        $scope.interacted1 = function (field) {

            return $scope.submitted1; //|| field.$dirty;
        };
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.certdis = true;

            if ($scope.myForm.$valid) {
                if ($scope.TRVCT_ChequeDDDate != undefined || $scope.TRVCT_ChequeDDDate != null) {
                    $scope.TRVCT_ChequeDDDate = new Date($scope.TRVCT_ChequeDDDate).toDateString();
                }
                else {
                    $scope.TRVCT_ChequeDDDate = null;
                }

                angular.forEach($scope.typelist, function (rrr) {

                    if (rrr.id == $scope.TRVCT_CertificateType) {
                        $scope.TRVCT_CertificateType1 = rrr.type;
                    }
                })

                //var fromdate = $scope.TRVCT_ObtainedDate == null ? "" : $filter('date')($scope.TRVCT_ObtainedDate, "dd-MM-yyyy");
                /// var tocdate = $scope.TRVCT_ValidTillDate == null ? "" : $filter('date')($scope.TRVCT_ValidTillDate, "yyyy-MM-dd");

                if ($scope.TRVCT_ObtainedDate != undefined || $scope.TRVCT_ObtainedDate != null) {
                    $scope.TRVCT_ObtainedDate = new Date($scope.TRVCT_ObtainedDate).toDateString();
                }
                if ($scope.TRVCT_ValidTillDate != undefined || $scope.TRVCT_ValidTillDate != null) {
                    $scope.TRVCT_ValidTillDate = new Date($scope.TRVCT_ValidTillDate).toDateString();
                }
                $scope.TRVCT_ModeOfPayment;

                var mobilesstd = [];
                angular.forEach($scope.mobilesstd, function (mobile) {
                    if (mobile.hrmemnO_MobileNo != undefined && mobile.hrmemnO_MobileNo != "") {
                        mobilesstd.push(mobile);
                    }
                });

                var emailsstd = [];
                angular.forEach($scope.emailsstd, function (email) {
                    if (email.hrmeM_EmailId != undefined && email.hrmeM_EmailId != "") {
                        emailsstd.push(email);
                    }
                });


                $scope.uploaddocments = [];

                angular.forEach($scope.vehicledocuupload, function (dd) {
                    if (dd.TRVCTD_FileLocation !== undefined && dd.TRVCTD_FileLocation !== null && dd.TRVCTD_FileLocation !== "") {
                        // $scope.uploaddocments.push(dd);
                        $scope.uploaddocments.push({ TRVCTD_FileName: dd.TRVCTD_FileName, TRVCTD_FileLocation: dd.TRVCTD_FileLocation, TRVCTD_Id: dd.TRVCTD_Id });
                    }
                });


                var data = {
                    "TRVCT_Id": $scope.TRVCT_Id,
                    "TRMV_Id": $scope.trmV_Id,
                    "TRVCT_ObtainedDate": $scope.TRVCT_ObtainedDate,
                    "TRVCT_ValidTillDate": $scope.TRVCT_ValidTillDate,
                    "TRVCT_AmountPaid": $scope.TRVCT_AmountPaid,
                    "TRVCT_ModeOfPayment": $scope.TRVCT_ModeOfPayment,
                    "TRVCT_Remarks": $scope.TRVCT_Remarks,

                    "TRVCT_PaymentReferenceNo": $scope.TRVCT_PaymentReferenceNo,
                    "TRVCT_ChequeDDNo": $scope.TRVCT_ChequeDDNo,
                    "TRVCT_ChequeDDDate": $scope.TRVCT_ChequeDDDate,
                    "TRVCT_CertificateType": $scope.TRVCT_CertificateType1,
                    "TRVCT_VECompanyName": $scope.TRVCT_VECompanyName,
                    "TRVCT_RTOName": $scope.TRVCT_RTOName,
                    "TRVCT_InsuranceCompany": $scope.TRVCT_InsuranceCompany,
                    "TRVCT_PolicyNo": $scope.TRVCT_PolicyNo,
                    mobile_list_dto: mobilesstd,
                    email_list_dto: emailsstd,
                    uploaddocments: $scope.uploaddocments,
                }
                apiService.create("VahicalCertificate/savedata/", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                            $state.reload();
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfully");
                            $state.reload();
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {

                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.edit = function (user) {
            $scope.TRVCT_CertificateType = '';
            var data = {
                "TRVCT_Id": user.trvcT_Id,
            }
            apiService.create("VahicalCertificate/edit/", data).then(function (promise) {
                if (promise.geteditdata.length > 0) {
                    $scope.certdis = true;

                    $scope.TRVCT_Id = promise.geteditdata[0].trvcT_Id;
                    $scope.trmV_Id = promise.geteditdata[0].trmV_Id;
                    $scope.TRVCT_ObtainedDate = new Date(promise.geteditdata[0].trvcT_ObtainedDate);
                    $scope.TRVCT_ValidTillDate = new Date(promise.geteditdata[0].trvcT_ValidTillDate);
                    $scope.TRVCT_AmountPaid = promise.geteditdata[0].trvcT_AmountPaid;

                    $scope.TRVCT_ModeOfPayment = promise.geteditdata[0].trvcT_ModeOfPayment;
                    $scope.TRVCT_Remarks = promise.geteditdata[0].trvcT_Remarks;
                    $scope.TRVCT_PaymentReferenceNo = promise.geteditdata[0].trvcT_PaymentReferenceNo;
                    $scope.TRVCT_ChequeDDNo = promise.geteditdata[0].trvcT_ChequeDDNo;
                    $scope.TRVCT_ChequeDDDate = new Date(promise.geteditdata[0].trvcT_ChequeDDDate);

                    $scope.TRVCT_VECompanyName = promise.geteditdata[0].trvcT_VECompanyName;
                    $scope.TRVCT_RTOName = promise.geteditdata[0].trvcT_RTOName;

                    $scope.TRVCT_InsuranceCompany = promise.geteditdata[0].trvcT_InsuranceCompany;
                    $scope.TRVCT_PolicyNo = promise.geteditdata[0].trvcT_PolicyNo;


                    var type1 = promise.geteditdata[0].trvcT_CertificateType;

                    angular.forEach($scope.typelist, function (xx) {

                        if (xx.type == type1) {
                            $scope.TRVCT_CertificateType = xx.id;
                        }
                    })

                    //$scope.TRVCT_CertificateType = promise.geteditdata[0].trvcT_CertificateType;

                    $scope.reqSelection = false;

                    if ($scope.TRVCT_ModeOfPayment == 'Cheque' || $scope.TRVCT_ModeOfPayment == 'DD' || $scope.TRVCT_ModeOfPayment == 'Bank') { //|| $scope.TRVCT_ModeOfPayment == 'Cash'
                        $scope.Cheque = true;
                        $scope.TRVCT_ChequeDDNo = promise.geteditdata[0].trvcT_ChequeDDNo;
                        $scope.TRVCT_ChequeDDDate = new Date(promise.geteditdata[0].trvcT_ChequeDDDate);
                    }
                    else {
                        $scope.Cheque = false;
                    }

                    if ($scope.TRVCT_ModeOfPayment == 'Online Payment') {
                        $scope.online = true;
                        $scope.TRVCT_PaymentReferenceNo = promise.geteditdata[0].trvcT_PaymentReferenceNo;

                    }
                    else {
                        $scope.online = false;
                    }
                    $scope.vehicledocuupload = [];
        
                    if (promise.documentdetails != null && promise.documentdetails.length > 0) {
                        angular.forEach(promise.documentdetails, function (ee) {
                            var img = ee.trvctD_FileLocation;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            $scope.filetype = lastelement;
                            $scope.vehicledocuupload.push({ TRVCTD_FileName: ee.trvctD_FileName, TRVCTD_FileLocation: ee.trvctD_FileLocation, TRVCT_Id: ee.trvcT_Id, TRVCTD_Id: ee.trvctD_Id, filetype: $scope.filetype })
                           
                        })
                    }

                    $scope.moblist = promise.mobilenolist;
                    $scope.emblist = promise.emailist;

                    $scope.mobilesstd1 = [];
                    angular.forEach($scope.moblist, function (ee) {
                        $scope.mobilesstd1.push({ hrmemnO_MobileNo: ee })

                    })
                    if ($scope.mobilesstd1.length > 0) {
                        $scope.mobilesstd = $scope.mobilesstd1;
                    }



                    $scope.emailsstd1 = [];
                    angular.forEach($scope.emblist, function (ee) {
                        $scope.emailsstd1.push({ hrmeM_EmailId: ee })

                    })
                    if ($scope.emailsstd1.length > 0) {
                        $scope.emailsstd = $scope.emailsstd1;
                    }

                    console.log($scope.moblist);
                    console.log($scope.mobilesstd);
                }
                else {
                }
            })
        }




        $scope.uploadeddocuemnt = function (user) {
            $scope.getviewdata = [];
            var data = {
                "TRVCT_Id": user.trvcT_Id,
            }
            apiService.create("VahicalCertificate/edit/", data).then(function (promise) {
                if (promise.documentdetails.length > 0) {
                    
                  
                    $scope.getviewdata = [];
                    if (promise.documentdetails != null && promise.documentdetails.length > 0) {
                        angular.forEach(promise.documentdetails, function (ee) {
                            var img = ee.trvctD_FileLocation;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            $scope.filetype = lastelement;                           
                            $scope.getviewdata.push({ TRVCTD_FileName: ee.trvctD_FileName, TRVCTD_FileLocation: ee.trvctD_FileLocation, TRVCT_Id: ee.trvcT_Id, TRVCTD_Id: ee.trvctD_Id, filetype: $scope.filetype })

                        })
                    }
                }
                else {
                    swal("No Record Found")
                    $('#mymodalviewdetailsfirsttab').modal('hide');
                }
            })
        }

        $scope.checkvalid1 = function () {
            $scope.TRDC_ToKM = parseInt($scope.TRDC_FromKM) + 1;
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }




        //Over all  Documents Upload
        $scope.uploadvisitordocuments1 = [];
        $scope.uploadvisitordocuments = function (input, document) {

            $scope.uploadvisitordocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                //$scope.size = input.files[0].size;
                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
                {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploadVisitorDocPhoto(document);
                }
                else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploadVisitorDocPhoto(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadvisitordocuments1.length; i++) {
                formData.append("File", $scope.uploadvisitordocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadvehicleimg", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.TRVCTD_FileLocation = d;
                    data.TRVCTD_FileName = $scope.filename;
                    $('#').attr('src', data.TRVCTD_FileLocation);
                    var img = data.TRVCTD_FileLocation;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.TRVCTD_FileLocation;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
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

        $scope.showGuardianSign = function (data) {
            $('#preview').attr('src', data.vmmvvI_VisitorPhoto);
        };

        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            //$('img').bind('contextmenu', function (e) {
            //    return false;
            //});          
            $('#myModalimg').modal('show');
        };


        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmtR_Resources;
            $scope.videdfd = data.lpmtR_Resources;
            $scope.movie = { src: data.lpmtR_Resources };
            $scope.movie1 = { src: data.lpmtR_Resources };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmtR_Resources });
            console.log($scope.view_videos);
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd, type) {

            var studentreg = idd;
            $scope.examstart_time = true;
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
                        download: studentreg + '.' + type
                    })[0].click();
                });
        };

        $scope.vehicledocuupload = [{ id: 'Teacher1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.vehicledocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.vehicledocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.vehicledocuupload);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.vehicledocuupload.length - 1;
            $scope.vehicledocuupload.splice(index, 1);

            if ($scope.vehicledocuupload.length === 0) {
                //data
            }
        };



        $scope.searchValue = '';
        //$scope.filterValue = function (obj) {
        //    
        //    return (JSON.stringify(obj.trdC_FromKM)).indexOf($scope.searchValue) >= 0 ||
        //        (JSON.stringify(obj.trdC_ToKM)).indexOf($scope.searchValue) >= 0 ||
        //        (JSON.stringify(obj.trdC_RateKm)).indexOf($scope.searchValue) >= 0
        //}

        $scope.clear = function () {
            $state.reload();
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


    };
})();


