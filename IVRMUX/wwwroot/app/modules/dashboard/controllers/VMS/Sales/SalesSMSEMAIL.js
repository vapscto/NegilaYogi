(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalesSMSEMAILController', SalesSMSEMAILController);

    SalesSMSEMAILController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce','$compile'];
    function SalesSMSEMAILController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $compile) {

        $scope.maxDatemf = new Date();
        //loading start

        $scope.athflag = false;
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("SalesSMSEMAIL/getalldetails", id).
                then(function (promise) {

                    $scope.categorylist = promise.categorylist;
                    $scope.sourcelist = promise.sourcelist;
                    $scope.productlist = promise.productlist;
                    $scope.statuslist = promise.statuslist;
                    $scope.countrylist = promise.countrylist;
                    // $scope.All_Individual();
                    $scope.grid_view = false;
                });
        };

        $scope.searchValue = '';
        $scope.itemsPerPage = 15;
        $scope.currentPage = 1;

        ///CATEGORY SEARCH START
        $scope.searchchkbx = '';
        $scope.filterchkbxhous = function (obj) {
            return (angular.lowercase(obj.ismsmcA_CategoryName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.togchkbxhouse = function () {
            $scope.usercheckhous = $scope.categorylist.every(function (options) {
                return options.housselect;
            });
        }
        $scope.all_checkhous = function () {
            var checkStatus = $scope.usercheckhous;
            angular.forEach($scope.categorylist, function (itm) {
                itm.housselect = checkStatus;
            });
        }

        ///CATEGORY SEARCH END 
        ///SOURCE SEARCH START
        $scope.searchchkbx1 = '';
        $scope.filterchkbxhous1 = function (obj) {
            return (angular.lowercase(obj.ismsmsO_SourceName)).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        }

        $scope.togchkbxhouse1 = function () {
            $scope.usercheckhous1 = $scope.sourcelist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous1 = function () {
            var checkStatus = $scope.usercheckhous1;
            angular.forEach($scope.sourcelist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///SOURCE SEARCH END


        ///PRODUCT SEARCH START
        $scope.searchchkbx2 = '';
        $scope.filterchkbxhous2 = function (obj) {
            return (angular.lowercase(obj.ismsmpR_ProductName)).indexOf(angular.lowercase($scope.searchchkbx2)) >= 0;
        }

        $scope.togchkbxhouse2 = function () {
            $scope.usercheckhous2 = $scope.productlist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous2 = function () {
            var checkStatus = $scope.usercheckhous2;
            angular.forEach($scope.productlist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///PRODUCT SEARCH END
        ///STATUS SEARCH START
        $scope.searchchkbx3 = '';
        $scope.filterchkbxhous3 = function (obj) {
            return (angular.lowercase(obj.ismsmsT_StatusName)).indexOf(angular.lowercase($scope.searchchkbx3)) >= 0;
        }

        $scope.togchkbxhouse3 = function () {
            $scope.usercheckhous3 = $scope.statuslist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous3 = function () {
            var checkStatus = $scope.usercheckhous3;
            angular.forEach($scope.statuslist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///STATUS SEARCH END

        ///COUNTRY SEARCH START
        $scope.searchchkbx4 = '';
        $scope.filterchkbxhous4 = function (obj) {
            return (angular.lowercase(obj.ivrmmC_CountryName)).indexOf(angular.lowercase($scope.searchchkbx4)) >= 0;
        }

        $scope.togchkbxhouse4 = function () {
            $scope.usercheckhous5 = false;
            $scope.usercheckhous4 = $scope.countrylist.every(function (options) {
                return options.select;
            });
            $scope.get_state();
        }
        $scope.all_checkhous4 = function () {
            $scope.usercheckhous5 = false;
            var checkStatus = $scope.usercheckhous4;
            angular.forEach($scope.countrylist, function (itm) {
                itm.select = checkStatus;
            });

            $scope.get_state();
        }

        ///COUNTRY SEARCH END 
        ///COUNTRY SEARCH START
        $scope.searchchkbx5 = '';
        $scope.filterchkbxhous5 = function (obj) {
            return (angular.lowercase(obj.ivrmmS_Name)).indexOf(angular.lowercase($scope.searchchkbx5)) >= 0;
        }

        $scope.togchkbxhouse5 = function () {
            $scope.usercheckhous5 = $scope.statelist.every(function (options) {
                return options.select;
            });


        }
        $scope.all_checkhous5 = function () {
            var checkStatus = $scope.usercheckhous5;
            angular.forEach($scope.statelist, function (itm) {
                itm.select = checkStatus;
            });


        }

        ///COUNTRY SEARCH END



        //validation start
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.staff_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.Department_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired2 = function () {
            return !$scope.Designation_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.sort = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        //validation end
        //all-individual start




        //all-check button end
        // fill dep start
        $scope.get_state = function () {
            debugger;
            var groupidss = [];
            for (var i = 0; i < $scope.countrylist.length; i++) {
                if ($scope.countrylist[i].select == true) {
                    groupidss.push({ ivrmmC_Id: $scope.countrylist[i].ivrmmC_Id })
                }
            }
            if (groupidss != undefined) {
                var data = {
                    stateids: groupidss,
                };
                apiService.create("SalesSMSEMAIL/get_state", data).
                    then(function (promise) {

                        $scope.statelist = promise.statelist;


                    });
            }

        };




        //===================================== VIEW TEMPLATE
        $scope.columdetails = [];
        $scope.viewtemplate = function () {

            var data = {
                "ISES_Id": $scope.ISES_Id
            }
            apiService.create("SalesSMSEMAIL/viewtemplatedetails", data).
                then(function (promise) {

                    if (promise.templatelist !== null) {
                        $scope.htmldata = promise.templatelist[0].iseS_MailHTMLTemplate;
                        var e1 = angular.element(document.getElementById("test"));
                        $compile(e1.html($scope.htmldata))(($scope));
                        $("#myModal").modal('show');
                    }
                });

        }
        //===================================== VIEW TEMPLATE

        $scope.transtypechange = function () {
            debugger;
            var data = {
                "type": $scope.bookornonbook
            }
                apiService.create("SalesSMSEMAIL/loadtemplate", data).
                    then(function (promise) {
                        $scope.templatelist = promise.templatelist;
                    });
            

        };
        $scope.snd_email = false;
        $scope.snd_sms = false;
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.leadlist, function (itm) {
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
        }

        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.leadlist.every(function (options) {

                return options.selected;
            });
        }

        $scope.viewfunction = function () {
            $scope.grid_view = false;
            $scope.upcomingintvw = [];
            $scope.inprogressintvw = [];
            $scope.completedintvw = [];
        };
        $scope.namesearch = '';
        //fill employee end
        //get report start
        $scope.grid_view = false;
        $scope.submitted = false;
        $scope.leadlist = [];
        $scope.GetReport = function () {

            //if ($scope.myForm.$valid) {


            $scope.leadlist = [];

            var catIds;
            for (var i = 0; i < $scope.categorylist.length; i++) {
                if ($scope.categorylist[i].housselect == true) {

                    if (catIds == undefined)
                        catIds = $scope.categorylist[i].ismsmcA_Id;
                    else
                        catIds = catIds + "," + $scope.categorylist[i].ismsmcA_Id;
                }
            }

            var soursIds;
            for (var i = 0; i < $scope.sourcelist.length; i++) {
                if ($scope.sourcelist[i].select == true) {

                    if (soursIds == undefined)
                        soursIds = $scope.sourcelist[i].ismsmsO_Id;
                    else
                        soursIds = soursIds + "," + $scope.sourcelist[i].ismsmsO_Id;
                }
            }

            var prodidss;
            for (var i = 0; i < $scope.productlist.length; i++) {
                if ($scope.productlist[i].select == true) {

                    if (prodidss == undefined)
                        prodidss = $scope.productlist[i].ismsmpR_Id;
                    else
                        prodidss = prodidss + "," + $scope.productlist[i].ismsmpR_Id;
                }
            }


            var statussidss;
            for (var i = 0; i < $scope.statuslist.length; i++) {
                if ($scope.statuslist[i].select == true) {

                    if (statussidss == undefined)
                        statussidss = $scope.statuslist[i].ismsmsT_Id;
                    else
                        statussidss = statussidss + "," + $scope.statuslist[i].ismsmsT_Id;
                }
            }

            var contryidss;
            for (var i = 0; i < $scope.countrylist.length; i++) {
                if ($scope.countrylist[i].select == true) {

                    if (contryidss == undefined)
                        contryidss = $scope.countrylist[i].ivrmmC_Id;
                    else
                        contryidss = contryidss + "," + $scope.countrylist[i].ivrmmC_Id;
                }
            }

            var stsidss;

            if ($scope.statelist != undefined && $scope.statelist != null) {
                for (var i = 0; i < $scope.statelist.length; i++) {
                    if ($scope.statelist[i].select == true) {

                        if (stsidss == undefined)
                            stsidss = $scope.statelist[i].ivrmmS_Id;
                        else
                            stsidss = stsidss + "," + $scope.statelist[i].ivrmmS_Id;
                    }
                }
            }



            if (stsidss == undefined && contryidss == undefined && catIds == undefined && soursIds == undefined && statussidss == undefined && prodidss == undefined && ($scope.namesearch == undefined || $scope.namesearch == '' || $scope.namesearch == null) && ($scope.contactname == undefined || $scope.contactname == '' || $scope.contactname == null) && ($scope.mobilesearch == undefined || $scope.mobilesearch == '' || $scope.mobilesearch == null) && ($scope.emailsearch == undefined || $scope.emailsearch == '' || $scope.emailsearch == null)) {
                swal('Select Atleast one set of search Parameter')
            }
            else {


                debugger;
                var data = {
                    "searchstring": $scope.namesearch,
                    "contactname": $scope.contactname,
                    "mobilesearch": $scope.mobilesearch,
                    "emailsearch": $scope.emailsearch,
                    "statussidss": statussidss,
                    "contryidss": contryidss,
                    "catIds": catIds,
                    "soursIds": soursIds,
                    "statidss": stsidss,
                    "prodidss": prodidss,
                };
                apiService.create("SalesSMSEMAIL/getrpt", data).
                    then(function (promise) {

                        if (promise.leadlist.length > 0 && promise.leadlist != null) {


                            $scope.leadlist = promise.leadlist;
                            $scope.templatelist = promise.templatelist;
                        }
                        else {
                            swal('No Record Found')
                        }

                    });
            }
            //}
            //else {
            //    $scope.submitted = true;
            //}
        };

        $scope.selecteddata = [];
        $scope.template1 = '';
        $scope.sendsmsemail = function () {

            if ($scope.myForm1.$valid) {

                $scope.selecteddata = [];
                angular.forEach($scope.leadlist, function (rr) {
                    if (rr.selected == true) {
                        $scope.selecteddata.push(rr);
                    }

                })
                if ($scope.selecteddata.length > 0) {

                    angular.forEach($scope.templatelist, function (rr) {
                        if (rr.iseS_Id == $scope.ISES_Id) {
                            $scope.template1 = rr.iseS_Template_Name;
                            $scope.lead = rr.ismsmsO_SourceName;
                        }

                    });                 
                    var data = {
                        "type": $scope.bookornonbook,
                        "template": $scope.template1,
                        "msg": $scope.MSG,
                        "smsmsg": $scope.SMSMSG,
                        "snd_email": $scope.snd_email,
                        "snd_sms": $scope.snd_sms,
                        "esubject": $scope.esubject,
                        "Footer": $scope.Footer,
                        "athflag": $scope.athflag,
                        "FHEAD": $scope.FHEAD,
                        selected: $scope.selecteddata,
                        filelist: $scope.materaldocuupload
                    };

                    var swaltext = "";
                    if ($scope.bookornonbook === 'TPL') {
                        swaltext = "Do you want to send mail for " + $scope.lead + " lead?";
                    }
                    else {
                        swaltext = "Do you want to send mail for lead?";
                    }

                    swal({
                        title: "Are you sure?",
                        text: swaltext,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,send it",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("SalesSMSEMAIL/sendsmsemail", data).then(function (promise) {
                                    if (promise.retrunMsg === "true") {
                                        swal('EMAIL/SMS SENT SUCCESSFULLY');
                                        $state.reload();
                                    }
                                    else {
                                        swal('ERROR');
                                    }
                                });
                            }
                            else {
                                swal("Sending Cancelled!!!");
                            }
                        });
                }
                else {
                    swal('Select atleast one record');
                }


            }
            else {
                $scope.submitted = true;
            }


        }



        //get report end
        //print start
        $scope.printData = function () {
            var divToPrint = document.getElementById("table");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        };
        //print end
        //export start


        //export end
        //TO clear  data
        $scope.clearid = function () {
            $state.reload();
        };
        //clear end
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
                    debugger;
                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    //$('#').attr('src', data.cfilepath);
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