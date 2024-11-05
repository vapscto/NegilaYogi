(function () {
    'use strict';
    angular.module('app').controller('MasterQuestionController', MasterQuestionController)

    MasterQuestionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce']
    function MasterQuestionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter
        , $q, $sce) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'LMSMOEQ_Id';
        $scope.sortReverse = true;
        $scope.searchValue = "";
        $scope.answer = "";
        $scope.show_ansOption = false;
        $scope.teacherdocuupload = {};

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        //-------------------Load Data
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterQuestion/getloaddata", pageid).then(function (promise) {
                $scope.currentPage1 = 1;
                $scope.currentPage2 = 1;
                $scope.currentPage3 = 1;
                $scope.itemsPerPage1 = paginationformasters;
                $scope.itemsPerPage2 = paginationformasters;
                $scope.itemsPerPage3 = paginationformasters;
                $scope.getQuestiondetails = promise.getQuestiondetails;
                $scope.presentCountgrid = $scope.getQuestiondetails.length;
                $scope.result = promise.result;
                $scope.getclass = promise.getclass;
                $scope.getSubjects = promise.getSubjects;
                $scope.getAnsOptions = promise.getAnsOptions;
                // $scope.getQdetails = promise.getQdetails;
                $scope.getFQuestiondetails = promise.getFQuestiondetails;


                $scope.getFQOptiondetails = promise.getFQOptiondetails;
                $scope.test = promise.getQuestiondetails;
                $scope.Option_Qus = [];
                $scope.presentCountgrid2 = promise.result.length;
                $scope.presentCountgrid3 = promise.getFQOptiondetails.length;
            });
        };


        //------------------1st Tab 
        $scope.savedata = function () {
            $scope.submitted1 = true;
            $scope.submitted2 = false;
            $scope.submitted3 = false;

            if ($scope.myForm.$valid) {

                if ($scope.LMSMOEQ_QuestionDesc === undefined || $scope.LMSMOEQ_QuestionDesc === null || $scope.LMSMOEQ_QuestionDesc === "") {
                    $scope.LMSMOEQ_QuestionDesc = "";
                }

                var data = {
                    "LMSMOEQ_Question": $scope.LMSMOEQ_Question,
                    "LMSMOEQ_QuestionDesc": $scope.LMSMOEQ_QuestionDesc,
                    "LMSMOEQ_Marks": $scope.LMSMOEQ_Marks,
                    "LMSMOEQ_Id": $scope.LMSMOEQ_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "uploadquestionfiles": $scope.teacherdocuupload
                };

                apiService.create("MasterQuestion/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.lmsmoeQ_Id === 0 || promise.lmsmoeQ_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.lmsmoeQ_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.lmsmoeQ_Id === 0 || promise.lmsmoeQ_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.lmsmoeQ_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.editQuestion = function (id) {
            var data = {
                "LMSMOEQ_Id": id
            };

            apiService.create("MasterQuestion/editQuestion", data).then(function (promise) {
                if (promise.editQus.length > 0) {
                    $scope.LMSMOEQ_Question = promise.editQus[0].lmsmoeQ_Question;
                    $scope.LMSMOEQ_QuestionDesc = promise.editQus[0].lmsmoeQ_QuestionDesc;
                    $scope.LMSMOEQ_Marks = promise.editQus[0].lmsmoeQ_Marks;
                    $scope.LMSMOEQ_Id = promise.editQus[0].lmsmoeQ_Id;
                    $scope.ismS_Id = promise.editQus[0].ismS_Id;

                    $scope.teacherdocuupload = promise.geteditdocs;

                    if ($scope.teacherdocuupload !== null && $scope.teacherdocuupload.length > 0) {
                        angular.forEach($scope.teacherdocuupload, function (dd) {
                            var img = dd.lmsmoeqF_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("data.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lmsmoeqF_FilePath;
                            }
                        });
                    } else {
                        $scope.teacherdocuupload = [{ id: 'Teacher1' }];
                    }
                }
            });
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.document_Path);
        };

        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.teacherdocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.teacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.teacherdocuupload);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.teacherdocuupload.length - 1;
            $scope.teacherdocuupload.splice(index, 1);

            if ($scope.teacherdocuupload.length === 0) {
                //data
            }
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
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

        $scope.viewdocumetns = function (dd) {
            var data = {
                "LMSMOEQ_Id": dd.lmsmoeQ_Id
            };
            apiService.create("MasterQuestion/viewdocumetns", data).then(function (promise) {
                if (promise !== null) {

                    $scope.viewdocarray = promise.viewdocarray;

                    angular.forEach($scope.viewdocarray, function (dd) {
                        var img = dd.lmsmoeqF_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.filetype = lastelement;
                        console.log("data.filetype : " + dd.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lmsmoeqF_FilePath;
                        }
                    });
                }
            });
        };

        $scope.deactiveparticulars = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lmsmoeqF_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LMSMOEQF_Id": deactiveRecord.lmsmoeqF_Id,
                "LMSMOEQ_Id": deactiveRecord.lmsmoeQ_Id
            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterQuestion/deactiveparticulars", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }

                            $scope.viewdocarray = promise.viewdocarray;

                            angular.forEach($scope.viewdocarray, function (dd) {
                                var img = dd.lmsmoeqF_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("data.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lmsmoeqF_FilePath;
                                }
                            });
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.searchValued = function (obj) {
            return (angular.lowercase(obj.lmsmoeQ_Question)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.lmsmoeQ_QuestionDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.lmsmoeQ_Marks)).indexOf($scope.searchValue) >= 0;
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
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
            $http.post("/api/ImageUpload/paonlinexam", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.lmsmoeqF_FilePath = d;
                    data.lmsmoeqF_FileName = $scope.filename;
                    $('#').attr('src', data.lmsmoeqF_FilePath);
                    var img = data.lmsmoeqF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lmsmoeqF_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        //2nd tab save
        $scope.savedataclass = function () {
            $scope.submitted2 = true;
            $scope.submitted1 = false;
            $scope.submitted3 = false;

            if ($scope.myForm2.$valid) {
                var data = {
                    "LMSMOEQ_Question": $scope.LMSMOEQ_Id.lmsmoeQ_Question,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "LMSMOEQ_Id": $scope.LMSMOEQ_Id.lmsmoeQ_Id,
                    "LMSMOEQC_Id": $scope.LMSMOEQC_Id
                };

                apiService.create("MasterQuestion/savedataclass", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if ($scope.LMSMOEQC_Id > 0) {
                            swal('Record Updated Successfully');
                        }
                        else {
                            swal('Record Saved Successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record Already Exist');
                    }
                    else {
                        swal('Failed To Save /Update, Please Contact Administrator');
                    }
                    $scope.clearall();
                    $scope.loaddata();
                });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.editQuestion1 = function (id, asmcL_Id, objuser) {
            angular.forEach($scope.result, function (opq) {
                if (id == opq.lmsmoeQ_Id) {
                    $scope.asmcL_Id = opq.asmcL_Id;
                    $scope.LMSMOEQ_Id = opq;
                }
                $scope.LMSMOEQC_Id = objuser.lmsmoeqC_Id;
            });
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.searchValue2 = "";
        $scope.searchValue2d = function (obj) {
            return (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0 ||
                (angular.lowercase(obj.lmsmoeQ_Question)).indexOf(angular.lowercase($scope.searchValue2)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.search_box = function () {
            if ($scope.searchValue !== "" || $scope.searchValue !== null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        };

        $scope.optionToggled = function (user) {
            $scope.ques_id = user.LMSMOEQ_Id;
            $scope.xyz = $scope.getQuestiondetails.every(function (itm) { return itm.xyz; });
            $scope.show_ansOption = true;
        };

        //---------------------------------------
        $scope.toggleAllC = function () {
            angular.forEach($scope.getAnsOptions, function (subj) {
                subj.abc = $scope.allC;
            });
        };

        $scope.optionToggledC = function () {
            $scope.allC = $scope.getAnsOptions.every(function (itm) { return itm.abc; });
        };

        $scope.addNew = function (totalgrid) {
            $scope.totalgrid = [];
            var LMBANO_No = '';
            if ($scope.getAnsOptions !== null || $scope.getAnsOptions !== '') {
                var a = $scope.getAnsOptions;
                for (var i = 0; i < a; i++) {
                    $scope.totalgrid.push({
                        LMSMOEQOA_Id: '',
                        LMSMOEQOA_Option: '',
                        LMSMOEQOA_OptionCode: ''
                    });
                }
                $scope.show_ansOption = true;
            }
        };


        $scope.optionChange = function () {
            var data = {
                "OptionType": $scope.att.xyz
            };
            apiService.create("MasterQuestion/optionChange", data).then(function (promise) {
                $scope.getAnsOptions = promise.noopt;
                $scope.addNew();
            });
        };

        $scope.att = {};

        $scope.savedata1 = function (att) {
            $scope.submitted3 = true;
            $scope.submitted1 = false;
            $scope.submitted2 = false;
            if ($scope.myForm3.$valid) {
                var cnt = 0;
                $scope.test = $scope.totalgrid;
                for (var m = 0; m < $scope.totalgrid.length; m++) {
                    var stu_id1 = $scope.totalgrid[m].LMSMOEQOA_Option;
                    var stu_id2 = $scope.totalgrid[m].LMSMOEQOA_OptionCode;
                    var already_cnt = 0;
                    angular.forEach($scope.test, function (itm1) {
                        if (itm1.LMSMOEQOA_Option == stu_id1 && itm1.LMSMOEQOA_OptionCode == stu_id2) {
                            already_cnt += 1;
                        }
                    });
                    if (already_cnt === 1) {
                        //dd
                    }
                    else {
                        cnt += 1;
                    }
                }

                var data = {
                    "LMSMOEQ_Id": $scope.att.xyz,
                    seleted_Ans: $scope.totalgrid,
                    "Answer": $scope.LMSMOEQOA_AnswerFlag
                };

                if (cnt < 1) {
                    apiService.create("MasterQuestion/savedetails1", data).then(function (promise) {
                        if (promise.returnval === true) {
                            if (promise.lmsmoeqoA_Id === 0 || promise.lmsmoeqoA_Id < 0) {
                                swal("Record saved Successfully");
                            } else if (promise.lmsmoeqoA_Id > 0) {
                                swal("Record Upadte Successfully");
                            }
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.lmsmoeqoA_Id === 0 || promise.lmsmoeqoA_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.lmsmoeqoA_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.clearall();
                        $scope.loaddata();
                    });
                }
                else {
                    swal('Duplicate Answer Options Entered');
                }

            }
            else {
                $scope.submitted3 = true;
            }
        };

        $scope.addColumn = function (role, indexx, totalgrid) {
            angular.forEach(totalgrid, function (subscription, index) {
                if (indexx != index)
                    subscription.LMSMOEQOA_AnswerFlag = false;
            });
        };

        $scope.onformclick = function (LMSMOEQ_Id) {
            var data = {
                "LMSMOEQ_Id": LMSMOEQ_Id
            };
            apiService.create("MasterQuestion/optiondetails", data).then(function (promise) {
                $scope.getoptiondetails = promise.getoptiondetails;
                $scope.Question = $scope.getoptiondetails[0].lmsmoeQ_Question;
            });
        };

        $scope.cancel1 = function (user) {
            $scope.ques_id = "";
            $scope.loaddata();
            $scope.show_ansOption = false;
        };

        $scope.search_box1 = function () {
            if ($scope.searchValue1 !== "" || $scope.searchValue1 !== null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        };


        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee;
            var data = {
                "LMSMOEQ_Id": $scope.editEmployee
            };

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterQuestion/Deletedetails", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Record Deleted Successfully');
                            }
                            else {
                                swal('Record Not Deleted Successfully');
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        $scope.clearall = function () {

            /*First Tab Clear*/
            $scope.ismS_Id = "";
            $scope.LMSMOEQ_Question = "";
            $scope.LMSMOEQ_QuestionDesc = "";
            $scope.LMSMOEQ_Marks = "";
            $scope.searchValue = "";
            $scope.submitted1 = false;
            $scope.teacherdocuupload = [{ id: 'Teacher1' }];

            /*Second Tab Clear */
            $scope.asmcL_Id = "";
            $scope.LMSMOEQ_Id = "";
            $scope.searchValue2 = "";
            $scope.submitted2 = false;

            /* Third Tab Clear*/
            $scope.totalgrid = [];
            $scope.show_ansOption = false;
            $scope.submitted3 = false;
            $scope.search = "";
            $scope.searchValue3 = "";

        };

        $scope.cleartabl1 = function () {
            $scope.ismS_Id = "";
            $scope.LMSMOEQ_Question = "";
            $scope.LMSMOEQ_QuestionDesc = "";
            $scope.LMSMOEQ_Marks = "";
            $scope.searchValue = "";
            $scope.submitted1 = false;
            $scope.teacherdocuupload = [{ id: 'Teacher1' }];
        };

        $scope.cleartabl2 = function () {
            $scope.asmcL_Id = "";
            $scope.LMSMOEQ_Id = "";
            $scope.searchValue2 = "";
            $scope.submitted2 = false;
        };

        $scope.cleartab3 = function () {
            $scope.totalgrid = [];
            $scope.show_ansOption = false;
            $scope.submitted3 = false;
            $scope.search = "";
            $scope.searchValue3 = "";
            $scope.att.xyz = "";
            $scope.loaddata();
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